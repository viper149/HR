using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Models.Chart;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.Hubs;
using DenimERP.ViewModels.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DenimERP.Controllers
{
    [Authorize]
    public class HomeController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMESSAGE _message;
        private readonly IMESSAGE_INDIVIDUAL _messageIndividual;

        public HomeController(UserManager<ApplicationUser> userManager,
            IMESSAGE message,
            IMESSAGE_INDIVIDUAL messageIndividual)
        {
            _userManager = userManager;
            _message = message;
            _messageIndividual = messageIndividual;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var messages = await _message.GetAllIncludeOtherObjects();
                var users = await _userManager.Users.ToListAsync();
                var currentUser = await _userManager.GetUserAsync(User);

                ViewBag.CurrentUserId = currentUser.Id;

                return View(new ChatUsersMesaages<MESSAGE, ApplicationUser>(messages, users, currentUser));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersName(string recipient)
        {
            try
            {
                return PartialView("GetUsersName", await _userManager.Users
                    .Where(e => e.UserName.Contains(recipient))
                    .Select(e => new ApplicationUser
                    {
                        Id = e.Id,
                        UserName = e.UserName
                    }).ToListAsync());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetSpecificUserMessage(string userId)
        {
            try
            {
                var task = await _messageIndividual.All();
                var user = await _userManager.GetUserAsync(User);
                var messageIndividuals = task.Where(e => e.ReceiverId.Equals(userId) && e.SenderId.Equals(user.Id)).ToList();

                return PartialView("GetSpecificUserMessage", new ChatUsersMesaages<MESSAGE_INDIVIDUAL, ApplicationUser>(messageIndividuals, null, user));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

    }

}

