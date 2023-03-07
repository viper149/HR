using System;
using DenimERP.Models;
using DenimERP.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class YarnQcPassController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;


        public YarnQcPassController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
            
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult GetYarnQcPassInfo()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        //[HttpGet]
        //public IActionResult CreatYarnQcPassInfo()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
