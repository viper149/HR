using System;
using System.Collections.Generic;
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
    public class FFsClearanceWastageTransferController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_FS_CLEARANCE_WASTAGE_TRANSFER _fFsClearanceWastageTransfer;
        private readonly IDataProtector _protector;
        private readonly string title = "Wastage Transfer";

        public FFsClearanceWastageTransferController(UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_FS_CLEARANCE_WASTAGE_TRANSFER fFsClearanceWastageTransfer)
        {
            _userManager = userManager;
            _fFsClearanceWastageTransfer = fFsClearanceWastageTransfer;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
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
            var data = (List<F_FS_CLEARANCE_WASTAGE_TRANSFER>)await _fFsClearanceWastageTransfer.GetAllFFsClearanceWastageTransferAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.TRANSDATE != null && m.TRANSDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.WTRNO != null && m.WTRNO.ToString().ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                       || (m.ITEMNavigation.INAME != null && m.ITEMNavigation.INAME.ToUpper().Contains(searchValue))
                                       || (m.CLRBYNavigation.FIRST_NAME != null && m.CLRBYNavigation.FIRST_NAME.ToUpper().Contains(searchValue))).ToList();
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
        public async Task<IActionResult> CreateFFsClearanceWastageTransfer()
        {
            return View(await _fFsClearanceWastageTransfer.GetInitObjByAsync(new FFsClearanceWastageTransferViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFFsClearanceWastageTransfer(FFsClearanceWastageTransferViewModel fFsClearanceWastageTransferViewModel)
        {
            if (ModelState.IsValid)
            {
                fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.CREATED_AT = fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.UPDATED_AT = DateTime.Now;
                fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.CREATED_BY = fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fFsClearanceWastageTransfer.InsertByAsync(fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(CreateFFsClearanceWastageTransfer), "FFsClearanceWastageTransfer");
                }
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(await _fFsClearanceWastageTransfer.GetInitObjByAsync(new FFsClearanceWastageTransferViewModel()));
            }
            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _fFsClearanceWastageTransfer.GetInitObjByAsync(new FFsClearanceWastageTransferViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditFFsClearanceWastageTransfer(string transId)
        {
            var fFsClearanceWastageTransfer = await _fFsClearanceWastageTransfer.FindByIdAsync(int.Parse(_protector.Unprotect(transId)));

            var fPrInspectionRejectionBViewModel = new FFsClearanceWastageTransferViewModel();
            if (fFsClearanceWastageTransfer != null)
            {
                fPrInspectionRejectionBViewModel.FFsClearanceWastageTransfer = fFsClearanceWastageTransfer;
                fPrInspectionRejectionBViewModel.FFsClearanceWastageTransfer.EncryptedId = _protector.Protect(fPrInspectionRejectionBViewModel.FFsClearanceWastageTransfer.TRANSID.ToString());
                return View(await _fFsClearanceWastageTransfer.GetInitObjByAsync(fPrInspectionRejectionBViewModel));
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return View(fPrInspectionRejectionBViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFFsClearanceWastageTransfer(FFsClearanceWastageTransferViewModel fFsClearanceWastageTransferViewModel)
        {
            if (ModelState.IsValid)
            {
                fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.TRANSID = int.Parse(_protector.Unprotect(fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.EncryptedId));
                var fPrInspectionWastageTransfer = await _fFsClearanceWastageTransfer.FindByIdAsync(fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.TRANSID);
                if (fPrInspectionWastageTransfer != null)
                {
                    fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.UPDATED_AT = DateTime.Now;
                    fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.CREATED_AT = fPrInspectionWastageTransfer.CREATED_AT;
                    fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer.CREATED_BY = fPrInspectionWastageTransfer.CREATED_BY;

                    if (await _fFsClearanceWastageTransfer.Update(fFsClearanceWastageTransferViewModel.FFsClearanceWastageTransfer))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(CreateFFsClearanceWastageTransfer), "FFsClearanceWastageTransfer");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(fFsClearanceWastageTransferViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(fFsClearanceWastageTransferViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fFsClearanceWastageTransferViewModel);
        }
    }
}
