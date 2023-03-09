using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures;
using HRMS.ServiceInterfaces;
using HRMS.ServiceInterfaces.CompanyInfo;
using HRMS.ServiceInterfaces.IdentityUser;
using HRMS.ServiceInterfaces.MenuMaster;
using HRMS.ServiceInterfaces.OtherInterfaces;
using HRMS.ViewModels;
using HRMS.ViewModels.Employee;
using HRMS.ViewModels.MenuMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Controllers
{
    [Authorize(Policy = "AdminRolePolicy")]
    //[RefreshLogin]
    public class AdministratorController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdministratorController> _logger;
        private readonly IProcessUploadFile _processUploadFile;
        private readonly IDeleteFileFromFolder _deleteFileFromFolder;
        private readonly IPDL_EMAIL_SENDER _pdlEmailSender;
        private readonly IMenuMaster _menuMaster;
        private readonly IMenuMasterRoles _menuMasterRoles;
        private readonly IAspNetUserTypes _aspNetUserTypes;
        private readonly IF_HRD_EMPLOYEE _fHrEmployee;
        private readonly ICOMPANY_INFO _companyInfo;
        private readonly IDataProtector _protector;

        public AdministratorController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AdministratorController> logger,
            IProcessUploadFile processUploadFile,
            IDeleteFileFromFolder deleteFileFromFolder,
            IPDL_EMAIL_SENDER pdlEmailSender,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IMenuMaster menuMaster,
            IMenuMasterRoles menuMasterRoles,
            IAspNetUserTypes aspNetUserTypes,
            IF_HRD_EMPLOYEE fHrEmployee,
            ICOMPANY_INFO companyInfo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _processUploadFile = processUploadFile;
            _deleteFileFromFolder = deleteFileFromFolder;
            _pdlEmailSender = pdlEmailSender;
            _menuMaster = menuMaster;
            _menuMasterRoles = menuMasterRoles;
            _aspNetUserTypes = aspNetUserTypes;
            _fHrEmployee = fHrEmployee;
            _companyInfo = companyInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenuMaster(string menuIdentityId)
        {
            try
            {
                var findByMenuIdentityIdAsync = await _menuMaster.FindByMenuIdentityIdAsync(int.Parse(_protector.Unprotect(menuIdentityId)));

                if (findByMenuIdentityIdAsync.MenuMaster != null)
                {
                    var menuMasterRoleses = await _menuMasterRoles.All();
                    await _menuMasterRoles.DeleteRange(menuMasterRoleses.Where(e => e.MenuIdentityId.Equals(findByMenuIdentityIdAsync.MenuMaster.MenuIdentity)));

                    if (await _menuMaster.Delete(findByMenuIdentityIdAsync.MenuMaster))
                    {
                        TempData["message"] = "Successfully Deleted Menu Master Information.";
                        TempData["type"] = "success";
                    }
                    else
                    {
                        TempData["message"] = "Failed To Delete Menu Master Information.";
                        TempData["type"] = "error";
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Menu Master Id Information.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetMenuMaster", $"Administrator");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMenuMaster(ExtendMenuMasterViewModel extendMenuMasterViewModel)
        {
            try
            {
                var findByIdAsync = await _menuMaster.FindByMenuIdentityIdAsync(int.Parse(_protector.Unprotect(extendMenuMasterViewModel.MenuMaster.EncryptedId)));

                if (findByIdAsync.MenuMaster != null)
                {
                    findByIdAsync.MenuMaster.MenuID = extendMenuMasterViewModel.MenuMaster.MenuID;
                    findByIdAsync.MenuMaster.MenuName = extendMenuMasterViewModel.MenuMaster.MenuName;
                    findByIdAsync.MenuMaster.Parent_MenuID = extendMenuMasterViewModel.MenuMaster.Parent_MenuID;
                    findByIdAsync.MenuMaster.MenuFileName = extendMenuMasterViewModel.MenuMaster.MenuFileName;
                    findByIdAsync.MenuMaster.MenuURL = extendMenuMasterViewModel.MenuMaster.MenuURL;
                    findByIdAsync.MenuMaster.USE_YN = extendMenuMasterViewModel.MenuMaster.USE_YN;
                    findByIdAsync.MenuMaster.ParentMenuIcon = extendMenuMasterViewModel.MenuMaster.ParentMenuIcon;
                    findByIdAsync.MenuMaster.Priority = extendMenuMasterViewModel.MenuMaster.Priority;

                    if (await _menuMaster.Update(findByIdAsync.MenuMaster))
                    {
                        var menuMasterRoleses = await _menuMasterRoles.All();
                        await _menuMasterRoles.DeleteRange(menuMasterRoleses.Where(e => e.MenuIdentityId.Equals(findByIdAsync.MenuMaster.MenuIdentity)));

                        foreach (var item in extendMenuMasterViewModel.UserRolesViewModels.Where(item => item.IsSelected))
                        {
                            await _menuMasterRoles.InsertByAsync(new MenuMasterRoles
                            {
                                MenuIdentityId = findByIdAsync.MenuMaster.MenuIdentity,
                                RoleId = item.RoleId
                            });
                        }

                        TempData["message"] = "Successfully Updated Menu Master Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetMenuMaster", "Administrator");
                    }

                    var model = new List<UserRolesViewModel>();

                    foreach (var role in _roleManager.Roles)
                    {
                        var userRolesViewModel = new UserRolesViewModel
                        {
                            RoleId = role.Id,
                            RoleName = role.Name
                        };

                        model.Add(userRolesViewModel);
                    }

                    extendMenuMasterViewModel.UserRolesViewModels = model;
                    var menuMasterViewModel = await _menuMaster.GetInitObjects(new MenuMasterViewModel());
                    extendMenuMasterViewModel.UserRolesViewModels = model;
                    extendMenuMasterViewModel.MenuMasters = menuMasterViewModel.MenuMasters;

                    TempData["message"] = "Failed To Update Menu Master Information.";
                    TempData["type"] = "error";

                    return View();
                }
                else
                {
                    var model = new List<UserRolesViewModel>();

                    foreach (var role in _roleManager.Roles)
                    {
                        var userRolesViewModel = new UserRolesViewModel
                        {
                            RoleId = role.Id,
                            RoleName = role.Name
                        };

                        model.Add(userRolesViewModel);
                    }

                    extendMenuMasterViewModel.UserRolesViewModels = model;
                    var menuMasterViewModel = await _menuMaster.GetInitObjects(new MenuMasterViewModel());
                    extendMenuMasterViewModel.UserRolesViewModels = model;
                    extendMenuMasterViewModel.MenuMasters = menuMasterViewModel.MenuMasters;

                    TempData["message"] = "Failed To Update Menu Master Information.";
                    TempData["type"] = "error";
                    return View(extendMenuMasterViewModel);
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMenuMaster(string menuIdentityId)
        {
            var byMenuIdentityIdAsync = (List<MenuMasterRoles>)await _menuMasterRoles.FindByMenuIdentityIdAsync(int.Parse(_protector.Unprotect(menuIdentityId)));
            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.OrderBy(e => e.Name))
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = byMenuIdentityIdAsync.Any(e => e.RoleId.Equals(role.Id))
                };

                model.Add(userRolesViewModel);
            }

            var findByMenuIdentityIdAsync = await _menuMaster.FindByMenuIdentityIdAsync(int.Parse(_protector.Unprotect(menuIdentityId)));
            findByMenuIdentityIdAsync.UserRolesViewModels = model;
            var menuMasterViewModel = await _menuMaster.GetInitObjects(new MenuMasterViewModel());
            findByMenuIdentityIdAsync.UserRolesViewModels = model;
            findByMenuIdentityIdAsync.MenuMasters = menuMasterViewModel.MenuMasters;

            return View(findByMenuIdentityIdAsync);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsMenuIdInUse(MenuMasterViewModel menuMasterViewModel)
        {
            return await _menuMaster.IsMenuIdAlreadyExistByAsync(menuMasterViewModel.MenuMaster.MenuID) ? Json("Menu ID Already Exist. Please Try Another One.") : Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsMenuNameInUse(MenuMasterViewModel menuMasterViewModel)
        {
            return await _menuMaster.IsMenuNameAlreadyExistByAsync(menuMasterViewModel.MenuMaster.MenuName) ? Json("Menu Name Already Exist. Please Try Another One.") : Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> GetMenuMasterTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _menuMaster.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                return Json(new
                {
                    draw = forDataTableByAsync.Draw,
                    recordsFiltered = forDataTableByAsync.RecordsTotal,
                    recordsTotal = forDataTableByAsync.RecordsTotal,
                    data = forDataTableByAsync.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetMenuMaster()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomSidebar(MenuMasterViewModel menuMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    menuMasterViewModel.MenuMaster.Parent_MenuID = string.IsNullOrEmpty(menuMasterViewModel.MenuMaster.Parent_MenuID) ? "*" : menuMasterViewModel.MenuMaster.Parent_MenuID;
                    menuMasterViewModel.MenuMaster.CreatedDate = DateTime.Now;
                    var insertedObjByAsync = await _menuMaster.GetInsertedObjByAsync(menuMasterViewModel.MenuMaster);

                    if (insertedObjByAsync != null)
                    {
                        foreach (var item in menuMasterViewModel.UserRolesViewModels.Where(item => item.IsSelected))
                        {
                            await _menuMasterRoles.InsertByAsync(new MenuMasterRoles
                            {
                                RoleId = item.RoleId,
                                MenuIdentityId = insertedObjByAsync.MenuIdentity
                            });
                        }

                        TempData["message"] = "Menu Added Successfully.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetMenuMaster", $"Administrator");
                    }

                    TempData["message"] = "Failed To Add Menu.";
                    TempData["type"] = "error";
                    return View(await _menuMaster.GetInitObjects(menuMasterViewModel));
                }

                TempData["message"] = "Failed To Add Menu.";
                TempData["type"] = "error";
                return View(await _menuMaster.GetInitObjects(menuMasterViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CustomSidebar()
        {
            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.OrderBy(e => e.Name))
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                model.Add(userRolesViewModel);
            }

            return View(await _menuMaster.GetInitObjects(new MenuMasterViewModel { UserRolesViewModels = model }));
        }

        [HttpPost]
        public async Task<IActionResult> EmailSetup(PDL_EMAIL_SENDER objPdlEmailSender)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;
                var pdlEmailSender = await _pdlEmailSender.FindByIdAsync(objPdlEmailSender.ID);

                if (pdlEmailSender != null)
                {
                    pdlEmailSender.FROM_EMAIL = objPdlEmailSender.FROM_EMAIL;
                    pdlEmailSender.SMTP_CLIENT = objPdlEmailSender.SMTP_CLIENT;
                    pdlEmailSender.PORT_NO = objPdlEmailSender.PORT_NO;
                    pdlEmailSender.USE_DEFAULT_CREDENTIALS = objPdlEmailSender.USE_DEFAULT_CREDENTIALS;
                    pdlEmailSender.ENABLE_SSL = objPdlEmailSender.ENABLE_SSL;
                    pdlEmailSender.NETWORK_CREDENTIAL_USERNAME = objPdlEmailSender.NETWORK_CREDENTIAL_USERNAME;
                    pdlEmailSender.NETWORK_CREDENTIAL_PASSWORD = objPdlEmailSender.NETWORK_CREDENTIAL_PASSWORD ?? pdlEmailSender.NETWORK_CREDENTIAL_PASSWORD;
                    pdlEmailSender.UPDATED_AT = DateTime.Now;
                    pdlEmailSender.UPDATED_BY = user.Id;

                    await _pdlEmailSender.Update(pdlEmailSender);
                }

                return RedirectToAction("EmailSetup", $"Administrator");
            }
            return View(objPdlEmailSender);
        }

        [HttpGet]
        public async Task<IActionResult> EmailSetup()
        {
            return View(await _pdlEmailSender.GetTop1Async());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new ApplicationRole()
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", $"Administrator");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetListRolesTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var navigationPropertyStrings = new[] { "" };
                var roles = _roleManager.Roles;

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    roles = OrderedResult<ApplicationRole>.GetOrderedResult(sortColumnDirection, sortColumn, null, roles);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    roles = roles.Where(m => m.Name.ToUpper().Contains(searchValue));

                    roles = OrderedResult<ApplicationRole>.GetOrderedResult(sortColumnDirection, sortColumn, null, roles);
                }

                var recordsTotal = await roles.CountAsync();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = await roles.Skip(skip).Take(pageSize).ToListAsync()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View($"NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View($"NotFound");
            }

            role.Name = model.RoleName;

            // Update the Role using UpdateAsync
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction($"ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View($"NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View($"NotFound");
            }

            for (var i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction($"EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction($"EditRole", new { Id = roleId });
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View($"NotFound");
            }
            else
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles", $"Administrator");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View($"ListRoles");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"Exception Occurred : {ex}");

                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View($"Error");
                }
            }
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsRoleInUse(string Name)
        {
            var role = await _roleManager.FindByNameAsync(Name);
            return role == null ? Json(true) : Json($"Role [ {Name} ] already in use. Choose a different one.");
        }

        [HttpPost]
        public async Task<JsonResult> GetListUsersTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var navigationPropertyStrings = new[] { "" };
                var users = _userManager.Users;

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    users = OrderedResult<ApplicationUser>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, users);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    users = users.Where(m => m.UserName.ToUpper().Contains(searchValue)
                                             || m.Email != null && m.Email.ToUpper().Contains(searchValue)
                                             || m.PhoneNumber != null && m.PhoneNumber.ToUpper().Contains(searchValue));

                    users = OrderedResult<ApplicationUser>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, users);
                }

                var recordsTotal = await users.CountAsync();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = await users.Skip(skip).Take(pageSize).ToListAsync()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View($"NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var aspNetUserTypeses = await _aspNetUserTypes.All();
            var fHrEmployees = await _fHrEmployee.All();
            var companies = await _companyInfo.All();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                OldPhotoPath = user.PhotoPath,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles,
                TYPEID = user.TYPEID,
                EMPID = user.EMPID,
                BID = user.BID,
                ExtendFHrEmployees = fHrEmployees.Select(e => new ExtendFHrdEmployee
                {
                    EMPID = e.EMPID,
                    EmpInfo = $"Employee ID: { (string.IsNullOrEmpty(e.EMPNO) ? "---" : e.EMPNO) }, Name: {e.FIRST_NAME} {e.LAST_NAME}"
                }).OrderBy(e => e.EmpInfo).ToList(),
                Companies = companies.Select(e => new COMPANY_INFO
                {
                    ID = e.ID,
                    COMPANY_NAME = $"{ e.COMPANY_NAME} - {e.TAGLINE}"
                }).OrderBy(e => e.COMPANY_NAME).ToList(),
                AspNetUserTypeses = aspNetUserTypeses.OrderBy(e => e.TYPENAME).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View($"NotFound");
            }
            else
            {
                user.TYPEID = model.TYPEID;
                user.EMPID = model.EMPID;
                user.BID = model.BID;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.UPDATED_AT = DateTime.Now;
                user.UPDATED_BY = loggedInUser.Id;

                if (model.NewPhotoPath != null)
                {
                    if (model.OldPhotoPath != null)
                    {
                        _deleteFileFromFolder.DeleteFileFromContentRootPath(model.OldPhotoPath, "profile_photos");
                    }
                    user.PhotoPath = _processUploadFile.ProcessUploadFileToProfilePhotos(model.NewPhotoPath);
                }
                else
                {
                    user.PhotoPath = model.OldPhotoPath;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction($"ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View($"NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction($"ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View($"ListUsers");
            }
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View($"NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.OrderBy(e => e.Name))
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View($"NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction($"EditUser", new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View($"NotFound");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (var claim in ClaimsStore.AllClaims)
            {
                var userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View($"NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user,
                model.Cliams.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction($"EditUser", new { Id = model.UserId });
        }
    }
}