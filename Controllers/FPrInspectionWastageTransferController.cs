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
    public class FPrInspectionWastageTransferController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_INSPECTION_WASTAGE_TRANSFER _fPrInspectionWastageTransfer;
        private readonly IDataProtector _protector;
        private readonly string title = "Wastage Transfer (Inspection)";

        public FPrInspectionWastageTransferController(UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_PR_INSPECTION_WASTAGE_TRANSFER fPrInspectionWastageTransfer)
        {
            _userManager = userManager;
            _fPrInspectionWastageTransfer = fPrInspectionWastageTransfer;
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
            var data = (List<F_PR_INSPECTION_WASTAGE_TRANSFER>)await _fPrInspectionWastageTransfer.GetAllInspectionWastageTransferAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.TRANSDATE != null && m.TRANSDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            foreach (var item in finalData)
            {
                item.EncryptedId = _protector.Protect(item.TRANSID.ToString());
            }

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateFPrInspectionWastageTransfer()
        {
            return View(new FPrInspectionWastageTransferViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateFPrInspectionWastageTransfer(FPrInspectionWastageTransferViewModel fPrInspectionWastageTransferViewModel)
        {
            if (ModelState.IsValid)
            {
                fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.CREATED_AT = fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.UPDATED_AT = DateTime.Now;
                fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.CREATED_BY = fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fPrInspectionWastageTransfer.InsertByAsync(fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(CreateFPrInspectionWastageTransfer), "FPrInspectionWastageTransfer");
                }
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(fPrInspectionWastageTransferViewModel);
            }
            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(fPrInspectionWastageTransferViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditFPrInspectionWastageTransfer(string wastId)
        {
            var fPrInspectionWastageTransfer = await _fPrInspectionWastageTransfer.FindByIdAsync(int.Parse(_protector.Unprotect(wastId)));

            if (fPrInspectionWastageTransfer != null)
            {
                var fPrInspectionWastageTransferViewModel = new FPrInspectionWastageTransferViewModel();
                fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer = fPrInspectionWastageTransfer;
                fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.EncryptedId = _protector.Protect(fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.TRANSID.ToString());
                return View(fPrInspectionWastageTransferViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return View(new FPrInspectionWastageTransferViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> EditFPrInspectionWastageTransfer(FPrInspectionWastageTransferViewModel fPrInspectionWastageTransferViewModel)
        {
            if (ModelState.IsValid)
            {
                fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.TRANSID = int.Parse(_protector.Unprotect(fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.EncryptedId));
                var fPrInspectionWastageTransfer = await _fPrInspectionWastageTransfer.FindByIdAsync(fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.TRANSID);
                if (fPrInspectionWastageTransfer != null)
                {

                    fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.UPDATED_AT = DateTime.Now;
                    fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.CREATED_AT = fPrInspectionWastageTransfer.CREATED_AT;
                    fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer.CREATED_BY = fPrInspectionWastageTransfer.CREATED_BY;

                    if (await _fPrInspectionWastageTransfer.Update(fPrInspectionWastageTransferViewModel.FPrInspectionWastageTransfer))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(CreateFPrInspectionWastageTransfer), "FPrInspectionWastageTransfer");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(fPrInspectionWastageTransferViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(fPrInspectionWastageTransferViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fPrInspectionWastageTransferViewModel);
        }
    }
}
