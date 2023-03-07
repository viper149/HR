using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    
    public class FFsFabricLoadingBillController : Controller
    {
        private readonly IF_BAS_VEHICLE_INFO _fBasVehicleInfo;
        private readonly IF_FS_FABRIC_LOADING_BILL _fFsFabricLoadingBill;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Fabric Loading Bill Information";

        public FFsFabricLoadingBillController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_VEHICLE_INFO fBasVehicleInfo,
            IF_FS_FABRIC_LOADING_BILL fFsFabricLoadingBill,
            UserManager<ApplicationUser> userManager
        )
        {
            _fBasVehicleInfo = fBasVehicleInfo;
            _fFsFabricLoadingBill = fFsFabricLoadingBill;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
        //[Route("GetTableData")]
        public async Task<JsonResult> GetTableData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;

            var data = (List<F_FS_FABRIC_LOADING_BILL>)await _fFsFabricLoadingBill.GetAllFFsFabricLoadingBillAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.TRANSDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.TRANSID.ToString().ToUpper().Contains(searchValue)
                                       || m.VEHICLE_ID.ToString().ToUpper().Contains(searchValue)
                                       || m.START_TIME != null && m.START_TIME.ToString().ToUpper().Contains(searchValue)
                                       || m.END_TIME != null && m.END_TIME.ToString().ToUpper().Contains(searchValue)
                                       || m.ROLL_QTY != null && m.ROLL_QTY.ToString().ToUpper().Contains(searchValue)
                                       || m.RATE != null && m.RATE.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateFFsFabricLoadingBillInfo()
        {
            try
            {
                return View(await _fFsFabricLoadingBill.GetInitObjByAsync(new FFsFabricLoadingBillViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFFsFabricLoadingBillInfo(FFsFabricLoadingBillViewModel fFsFabricLoadingBillViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fFsFabricLoadingBill.InsertByAsync(fFsFabricLoadingBillViewModel.FFsFabricLoadingBill);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return View(await _fFsFabricLoadingBill.GetInitObjByAsync(fFsFabricLoadingBillViewModel));
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(await _fFsFabricLoadingBill.GetInitObjByAsync(fFsFabricLoadingBillViewModel));
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(await _fFsFabricLoadingBill.GetInitObjByAsync(fFsFabricLoadingBillViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(await _fFsFabricLoadingBill.GetInitObjByAsync(fFsFabricLoadingBillViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFFsFabricLoadingBillInfo(string lbId)
        {
            try
            {
                var fFsFabricLoadingBillViewModel = await _fFsFabricLoadingBill.GetInitObjByAsync(new FFsFabricLoadingBillViewModel());
                fFsFabricLoadingBillViewModel.FFsFabricLoadingBill = await _fFsFabricLoadingBill.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));
                fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.EncryptedId = lbId;

                return View(fFsFabricLoadingBillViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFFsFabricLoadingBillInfo(FFsFabricLoadingBillViewModel fFsFabricLoadingBillViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(CreateFFsFabricLoadingBillInfo), $"FFsFabricLoadingBill");
                fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.TRANSID = int.Parse(_protector.Unprotect(fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.EncryptedId));
                var fHrdLeave = await _fFsFabricLoadingBill.FindByIdAsync(fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.TRANSID);
                if (fHrdLeave != null)
                {
                    fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.UPDATED_AT = DateTime.Now;
                    fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.CREATED_AT = fHrdLeave.CREATED_AT;
                    fFsFabricLoadingBillViewModel.FFsFabricLoadingBill.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _fFsFabricLoadingBill.Update(fFsFabricLoadingBillViewModel.FFsFabricLoadingBill))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(await _fFsFabricLoadingBill.GetInitObjByAsync(new FFsFabricLoadingBillViewModel()));
        }
    }
}
