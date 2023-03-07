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
    public class FPrInspectionRejectionBController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_INSPECTION_REJECTION_B _fPrInspectionRejectionB;
        private readonly IDataProtector _protector;
        private readonly string title = "Inspection Rejection (Bulk)";

        public FPrInspectionRejectionBController(UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_PR_INSPECTION_REJECTION_B fPrInspectionRejectionB)
        {
            _userManager = userManager;
            _fPrInspectionRejectionB = fPrInspectionRejectionB;
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
            var data = (List<F_PR_INSPECTION_REJECTION_B>)await _fPrInspectionRejectionB.GetAllFPrInspectionRejectionBAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.TRANS_DATE != null && m.TRANS_DATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.DOFF_.LOOM_NONavigation.LOOM_NO != null && m.DOFF_.LOOM_NONavigation.LOOM_NO.ToUpper().Contains(searchValue))
                                       || (m.SECTION_.SECNAME != null && m.SECTION_.SECNAME.ToUpper().Contains(searchValue))
                                       || (m.SHIFTNavigation.SHIFT != null && m.SHIFTNavigation.SHIFT.ToUpper().Contains(searchValue))
                                       || (m.DEFECT_.NAME != null && m.DEFECT_.NAME.ToUpper().Contains(searchValue))
                                       || (m.DOFFING_DATE != null && m.DOFFING_DATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.DOFFING_LENGTH != null && m.DOFFING_LENGTH.ToString().ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                    ).ToList();
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
        public async Task<IActionResult> CreateFPrInspectionRejectionB()
        {
            var fPrInspectionRejectionBViewModel = new FPrInspectionRejectionBViewModel
            {
                FPrInspectionRejectionB = new F_PR_INSPECTION_REJECTION_B
                {
                    TRANS_DATE = DateTime.Now.Date,
                    SearchDate = DateTime.Now.Date
                }
            };

            return View(await _fPrInspectionRejectionB.GetInitObjByAsync(fPrInspectionRejectionBViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFPrInspectionRejectionB(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel)
        {
            if (ModelState.IsValid)
            {
                fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.CREATED_AT = fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.UPDATED_AT = DateTime.Now;
                fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.CREATED_BY = fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fPrInspectionRejectionB.InsertByAsync(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(CreateFPrInspectionRejectionB), "FPrInspectionRejectionB");
                }
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(await _fPrInspectionRejectionB.GetInitObjByAsync(new FPrInspectionRejectionBViewModel()));
            }
            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _fPrInspectionRejectionB.GetInitObjByAsync(new FPrInspectionRejectionBViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditFPrInspectionRejectionB(string ibrId)
        {
            var fPrInspectionRejectionB = await _fPrInspectionRejectionB.FindByIdAsync(int.Parse(_protector.Unprotect(ibrId)));

            var fPrInspectionRejectionBViewModel = new FPrInspectionRejectionBViewModel();
            if (fPrInspectionRejectionB != null)
            {
                fPrInspectionRejectionB.SearchDate = (DateTime)fPrInspectionRejectionB.TRANS_DATE;
                fPrInspectionRejectionBViewModel.FPrInspectionRejectionB = fPrInspectionRejectionB;
                fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.EncryptedId = _protector.Protect(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.IBR_ID.ToString());
                return View(await _fPrInspectionRejectionB.GetInitObjByAsync(fPrInspectionRejectionBViewModel));
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return View(fPrInspectionRejectionBViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFPrInspectionRejectionB(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel)
        {
            if (ModelState.IsValid)
            {
                fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.IBR_ID = int.Parse(_protector.Unprotect(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.EncryptedId));
                var fPrInspectionWastageTransfer = await _fPrInspectionRejectionB.FindByIdAsync(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.IBR_ID);
                if (fPrInspectionWastageTransfer != null)
                {
                    fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.UPDATED_AT = DateTime.Now;
                    fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.CREATED_AT = fPrInspectionWastageTransfer.CREATED_AT;
                    fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.CREATED_BY = fPrInspectionWastageTransfer.CREATED_BY;

                    if (await _fPrInspectionRejectionB.Update(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(CreateFPrInspectionRejectionB), "FPrInspectionRejectionB");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(fPrInspectionRejectionBViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(fPrInspectionRejectionBViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fPrInspectionRejectionBViewModel);
        }

        [AcceptVerbs("get", "post")]
        public async Task<IActionResult> GetAllByDoffId(int wdId)
        {
            try
            {
                return Ok(await _fPrInspectionRejectionB.GetAllBywdIdAsync(wdId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("get", "post")]
        public async Task<IActionResult> GetDoffByInspectionDate(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel)
        {
            try
            {
                return Ok(await _fPrInspectionRejectionB.GetDoffByInspectionDate(fPrInspectionRejectionBViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpGet]
        public async Task<IActionResult> DeleteFPrInspectionRejectionB(string id)
        {
            try
            {
                var team = await _fPrInspectionRejectionB.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (team != null)
                {
                    var result = await _fPrInspectionRejectionB.Delete(team);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Rejection Entry";
                        TempData["type"] = "success";
                        return RedirectToAction("CreateFPrInspectionRejectionB", $"FPrInspectionRejectionB");
                    }

                    TempData["message"] = "Failed to Delete Rejection Entry";
                    TempData["type"] = "error";
                    return RedirectToAction("CreateFPrInspectionRejectionB", $"FPrInspectionRejectionB");
                }

                TempData["message"] = " Rejection Entry Not Found!.";
                TempData["type"] = "error";
                return RedirectToAction("CreateFPrInspectionRejectionB", $"FPrInspectionRejectionB");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete  Rejection Entry";
                TempData["type"] = "error";
                return RedirectToAction("CreateFPrInspectionRejectionB", $"FPrInspectionRejectionB");
            }
        }
    }
}
