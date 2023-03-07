using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DenimERP.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IPDL_EMAIL_SENDER<bool> _emailSender;
        private readonly IProcessUploadFile _processUploadFile;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IPDL_EMAIL_SENDER<bool> emailSender,
            IProcessUploadFile processUploadFile)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _processUploadFile = processUploadFile;
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsAnyBulkEmailsInUse(string email)
        {
            var regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            var emails = email.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var errors = new List<string>();

            for (var i = 0; i < emails.Length; i++)
            {
                if (!regex.IsMatch(emails[i]))
                {
                    errors.Add($"Index Of {i}, Email {emails[i]} Is Invalid.");
                }

                if (await _userManager.FindByEmailAsync(emails[i]) != null)
                {
                    errors.Add($"Index Of {i}, Email {emails[i]} Is Already In Use.");
                }
            }

            return !errors.Any() ? Json(true) : Json($"{string.Join("<br />", errors)}");
        }

        [HttpPost]
        public async Task<IActionResult> ManualRegistration(ExtendRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var emails = model.Email.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var successful = new List<string>();
                    var failed = new List<int>();

                    foreach (var email in emails)
                    {
                        var username = email.Split("@")[0];

                        var user = new ApplicationUser
                        {
                            UserName = username,
                            Email = email,
                            PhotoPath = null,
                            CREATED_AT = DateTime.Now,
                            UPDATED_AT = DateTime.Now,
                            EmailConfirmed = true,
                            ManuallyCreated = true,
                            CREATED_BY = currentUser.Id,
                            UPDATED_BY = currentUser.Id
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (!result.Succeeded)
                        {
                            failed.Add(1);

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            foreach (var item in successful)
                            {
                                ModelState.AddModelError(string.Empty, item);
                            }
                        }
                        else
                        {
                            successful.Add($"Email {email} successfully registered!");
                        }
                    }

                    if (!failed.Any())
                    {
                        TempData["message"] = $"Successfully Added {emails.Length} Users.";
                        TempData["type"] = "success";
                        return RedirectToAction("ListUsers", "Administrator");
                    }
                }

                return View(model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult ManualRegistration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            System.Threading.Thread.Sleep(2000);

            var applicationUser = _userManager.GetUserAsync(User).Result;
            var result = _userManager.GetRolesAsync(applicationUser).Result;

            return View(new UserProfileInformation<string>(applicationUser, result));
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                return View("AddPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await _userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        //[Authorize(Policy = "DeveloperPolicy")]
        public IActionResult ResetClientPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetClientPassword(ResetUserPasswordViewModel resetUserPasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser();

                    if (!string.IsNullOrEmpty(resetUserPasswordViewModel.UserName))
                    {
                        user = await _userManager.FindByNameAsync(resetUserPasswordViewModel.UserName);
                    }
                    else if (!string.IsNullOrEmpty(resetUserPasswordViewModel.UserName))
                    {
                        user = await _userManager.FindByNameAsync(resetUserPasswordViewModel.Email);
                    }

                    if (user != null)
                    {
                        // reset the user password
                        var result = await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), resetUserPasswordViewModel.Password);

                        if (result.Succeeded)
                        {
                            // Upon successful password reset and user lockout time set to current time so that user can login again                        
                            if (await _userManager.IsLockedOutAsync(user))
                            {
                                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                            }

                            return View("ResetClientPasswordConfirmation", resetUserPasswordViewModel);
                        }

                        // Display validation errors. For example, password reset token already
                        // used to change the password or password complexity rules not met
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        ModelState.AddModelError("", "Invalid Submission. Please try again later.");
                        return View("ResetClientPassword", resetUserPasswordViewModel);
                    }

                    ModelState.AddModelError("", "Invalid Submission. Please try again later.");
                    return View("ResetClientPassword", resetUserPasswordViewModel);
                }

                //ModelState.AddModelError("", "Invalid Submission. Please try again later.");
                return View("ResetClientPassword", resetUserPasswordViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        // Upon successful password reset and user lockout time set to current time so that user can login again                        
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }

                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    // Log the password reset link
                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send link to user email
                    var result = _emailSender.SendEmailAsync(model.Email, "Reset password", $"Here is the link to reset password. <a href=\"{passwordResetLink}\">Click here</a>", true);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhotoPath = model.PhotoPath != null ? _processUploadFile.ProcessUploadFileToProfilePhotos(model.PhotoPath, "profile_photos") : null,
                    CREATED_AT = DateTime.Now,
                    UPDATED_AT = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    // Save logs
                    _logger.Log(LogLevel.Warning, confirmationLink);

                    // Send link to user email
                    _emailSender.SendEmailAsync(user.Email, "Confirm account", $"Here is the link to reset password. <a href=\"{confirmationLink}\">Click here</a>", true);

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administrator");
                    }

                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can login, please confirm " + "email, by clicking on the confirmation link we have emailed you";
                    return View("Error");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                var model = new LoginViewModel()
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };
                
                //return LocalRedirect(returnUrl);
                return View(model);
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // Get the email claim from external login provider (Google, Facebook etc)
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser user = null;

            if (email != null)
            {
                // Find the user
                user = await _userManager.FindByEmailAsync(email);

                // If email is not confirmed, display login view with validation error

                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                //var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    //var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on it.ho@pioneer-denim.com";

                return View("Error");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    //var user = await _userManager.FindByEmailAsync(model.Email);
                    ApplicationUser user;
                    if (model.Email.Contains("@") && model.Email.Contains("."))
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                    }
                    else
                    {
                        user = await _userManager.FindByNameAsync(model.Email);
                    }

                    if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
                    {
                        ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                        return View(model);
                    }

                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(
                            user, model.Password, model.RememberMe, true);

                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Dashboard", "Home");
                        }

                        // If account is lockedout
                        if (result.IsLockedOut)
                        {
                            return View("AccountLocked");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }

                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result == null ? Json(true) : Json($"Email [ {email} ] is already in use.");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserNameInUse(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            return result == null ? Json(true) : Json($"User name [ {userName} ] is already in use.");
        }
    }
}