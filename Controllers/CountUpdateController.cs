using System;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class CountUpdateController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRND_FABRIC_COUNTINFO _rndFabricCountInfo;
        private readonly IRND_YARNCONSUMPTION _rndYarnConsumption;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountInfo;
        private readonly IDataProtector _protector;

        public CountUpdateController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IRND_FABRIC_COUNTINFO rndFabricCountInfo,
            IRND_YARNCONSUMPTION rndYarnConsumption,
            IBAS_YARN_COUNTINFO basYarnCountInfo
        )
        {
            _userManager = userManager;
            _rndFabricCountInfo = rndFabricCountInfo;
            _rndYarnConsumption = rndYarnConsumption;
            _basYarnCountInfo = basYarnCountInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateCountNameByRnd()
        {
            try
            {
                var result = await _basYarnCountInfo.GetInitYarnObjects(new YarnCountUpdateViewModel());
                return View(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCountNameByRnd(YarnCountUpdateViewModel yarnCountUpdateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var oldCountDetails = await _basYarnCountInfo.FindByIdAsync(yarnCountUpdateViewModel.Old_Count);
                    var newCountDetails = await _basYarnCountInfo.FindByIdAsync(yarnCountUpdateViewModel.New_Count);

                    oldCountDetails.RND_COUNTNAME = newCountDetails.COUNTNAME;
                    oldCountDetails.UPDATED_AT = DateTime.Now;
                    oldCountDetails.UPDATED_BY = user.Id;

                    var isUpdate = await _basYarnCountInfo.Update(oldCountDetails);

                    if (isUpdate)
                    {
                        TempData["message"] = "Successfully Updated RND Count Name";
                        TempData["type"] = "success";
                        return RedirectToAction($"UpdateCountNameByRnd", $"CountUpdate");
                    }
                    TempData["message"] = "Failed to Update RND Count Name";
                    TempData["type"] = "error";
                    return View(await _basYarnCountInfo.GetInitYarnObjects(yarnCountUpdateViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _basYarnCountInfo.GetInitYarnObjects(yarnCountUpdateViewModel));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return View(await _basYarnCountInfo.GetInitYarnObjects(yarnCountUpdateViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateCountNameBySoNo()
        {
            try
            {
                var result = await _basYarnCountInfo.GetInitYarnObjects(new YarnCountUpdateViewModel());
                return View(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<bool> UpdateCountNameBySoNo(YarnCountUpdateViewModel yarnCountUpdateViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var exData = await _rndFabricCountInfo.FindByIdAsync(yarnCountUpdateViewModel.Old_Count);

                if (exData == null) return false;
                var consumptionDetails = await _rndYarnConsumption.GetPrimaryKeyByCountIdAndFabCodeAsync(exData.COUNTID??0, exData.FABCODE,exData.YARNFOR??0, exData.COLORCODE??0);
                //Count Info Data
                exData.COUNTID = yarnCountUpdateViewModel.New_Count;
                exData.UPDATED_AT = DateTime.Now;
                exData.UPDATED_BY = user.Id;
                //Consumption Data

                if (consumptionDetails == null)
                {
                    return false;
                }
                consumptionDetails.COUNTID = yarnCountUpdateViewModel.New_Count;

                return await _rndFabricCountInfo.Update(exData) && await _rndYarnConsumption.Update(consumptionDetails);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return false;
            }
        }
    }
}