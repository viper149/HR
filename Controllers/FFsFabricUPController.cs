using System;
using System.Linq;
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
    public class FFsFabricUPController : Controller
    {
        private readonly IF_FS_UP_MASTER _fFsUpMaster;
        private readonly IF_FS_UP_DETAILS _fFsUpDetails;
        private readonly ICOM_EX_LCINFO _comExLcinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " Fabric Store UP Information";

        public FFsFabricUPController(IDataProtectionProvider dataProtectionProvider,
            IF_FS_UP_MASTER fFsUpMaster,
            IF_FS_UP_DETAILS fFsUpDetails,
            ICOM_EX_LCINFO comExLcinfo,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fFsUpMaster = fFsUpMaster;
            _fFsUpDetails = fFsUpDetails;
            _comExLcinfo = comExLcinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFFsFabricUP()
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
        public async Task<IActionResult> CreateFFsFabricUP()
        {
            return View(await _fFsUpMaster.GetInitObjByAsync(new FFsFabricUPViewModel()));
        }
        [HttpGet]
        public async Task<IActionResult> GetLcInfo(int lcId)
        {
            try
            {
                return Ok(await _fFsUpDetails.GetLcInfo(lcId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFFsFabricUP(FFsFabricUPViewModel fFsFabricUPViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                fFsFabricUPViewModel.FFsUPMaster.CREATED_BY = fFsFabricUPViewModel.FFsUPMaster.UPDATED_BY = user.Id;
                fFsFabricUPViewModel.FFsUPMaster.CREATED_AT = fFsFabricUPViewModel.FFsUPMaster.UPDATED_AT = DateTime.Now;

                var fFsUPMaster = await _fFsUpMaster.GetInsertedObjByAsync(fFsFabricUPViewModel.FFsUPMaster);
                if (fFsUPMaster.UP_ID != 0)
                {
                    fFsUPMaster.UP_NO = fFsUPMaster.UP_ID.ToString();
                    await _fFsUpMaster.Update(fFsUPMaster);

                    foreach (var item in fFsFabricUPViewModel.FFsFabricDetailsList)
                    {
                        item.CREATED_BY = user.Id;
                        item.UPDATED_BY = user.Id;
                        item.CREATED_AT = DateTime.Now;
                        item.UPDATED_AT = DateTime.Now;
                        item.UP_ID = fFsUPMaster.UP_ID;
                    }

                    if (fFsFabricUPViewModel.FFsFabricDetailsList.Any())
                    {
                        if (await _fFsUpDetails.InsertRangeByAsync(fFsFabricUPViewModel.FFsFabricDetailsList))
                        {
                            TempData["message"] = "Successfully added Fabric UP Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFFsFabricUP", $"FFsFabricUP");
                        }
                        TempData["message"] = "Failed to Add Fabric UP Information.";
                        TempData["type"] = "error";
                        return View($"CreateFFsFabricUP", await _fFsUpMaster.GetInitObjByAsync(fFsFabricUPViewModel));
                    }
                    TempData["message"] = "Failed to Add Fabric UP Information";
                    TempData["type"] = "error";
                    return View($"CreateFFsFabricUP", await _fFsUpMaster.GetInitObjByAsync(fFsFabricUPViewModel));
                }
                TempData["message"] = "Failed to Add Fabric UP Information";
                TempData["type"] = "error";
                return View($"CreateFFsFabricUP", await _fFsUpMaster.GetInitObjByAsync(fFsFabricUPViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Fabric UP Information";
                TempData["type"] = "error";
                return View($"CreateFFsFabricUP", await _fFsUpMaster.GetInitObjByAsync(fFsFabricUPViewModel));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrDeleteFFsFabricUPDetailsTable(FFsFabricUPViewModel fFsFabricUPViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fFsFabricUPViewModel.IsDeletable)
                {
                    var fFsFabricDetails = fFsFabricUPViewModel.FFsFabricDetailsList[fFsFabricUPViewModel.RemoveIndex];

                    if (fFsFabricDetails.UP_DID > 0)
                    {
                        await _fFsUpDetails.Delete(fFsFabricDetails);
                    }

                    fFsFabricUPViewModel.FFsFabricDetailsList.RemoveAt(fFsFabricUPViewModel.RemoveIndex);
                }
                else
                {
                    if (!fFsFabricUPViewModel.FFsFabricDetailsList.Any(e =>
                            e.UP_DID.Equals(fFsFabricUPViewModel.FFsFabricDetail.UP_DID)))
                    {
                        fFsFabricUPViewModel.FFsFabricDetailsList.Add(fFsFabricUPViewModel.FFsFabricDetail);
                    }
                }

                return PartialView($"FFsFabricUPDetailsPartialView", await _fFsUpDetails.GetInitObjectsOfSelectedItems(fFsFabricUPViewModel));

            }
            catch (Exception e )
            {
                Console.WriteLine();
                throw;
            }
        }

    }
}
