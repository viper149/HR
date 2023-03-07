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
    public class FprInspectionCutPcsTransferController : Controller
    {
        private readonly IF_PR_INSPECTION_CUTPCS_TRANSFER _fPrInspectionCutpcsTransfer;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " Inspection Cut Pcs Transfer Information";

        public FprInspectionCutPcsTransferController(IDataProtectionProvider dataProtectionProvider,
            IF_PR_INSPECTION_CUTPCS_TRANSFER fPrInspectionCutpcsTransfer,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fPrInspectionCutpcsTransfer = fPrInspectionCutpcsTransfer;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFprInspectionCutPcsTransferInfo()
        {
            try{
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }
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

            var data = (List<F_PR_INSPECTION_CUTPCS_TRANSFER>)await _fPrInspectionCutpcsTransfer.GetAllFprInspectionCutPcsTransferAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.TRNS_DATE.ToString().ToUpper().Contains(searchValue)
                                       || m.ROLL_NO.ToString().ToUpper().Contains(searchValue)
                                       || m.QTY_YDS.ToString().ToUpper().Contains(searchValue)
                                       || m.QTY_KG.ToString().ToUpper().Contains(searchValue)
                                       || m.SHIFT.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
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
        //[Route("Create")]
        public async Task<IActionResult> CreateFprInspectionCutPcsTransferInfo()
        {
            try
            {
                return View(await _fPrInspectionCutpcsTransfer.GetInitObjByAsync(new FprInspectionCutPcsTransferViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFprInspectionCutPcsTransferInfo(FprInspectionCutPcsTransferViewModel fprInspectionCutPcsTransferViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CREATED_AT = fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.UPDATED_AT = DateTime.Now;
                    fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CREATED_BY = fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.UPDATED_BY =
                        (await _userManager.GetUserAsync(User)).Id;
                    var result = await _fPrInspectionCutpcsTransfer.InsertByAsync(fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFprInspectionCutPcsTransferInfo), $"FprInspectionCutPcsTransfer");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fprInspectionCutPcsTransferViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fprInspectionCutPcsTransferViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fprInspectionCutPcsTransferViewModel);
            }
        }


        //[HttpGet]
        //public async Task<IActionResult> EditFprInspectionCutPcsTransferInfo(string cpId)
        //{
        //    try
        //    {
        //        return View(await _fPrInspectionCutpcsTransfer.GetInitObjByAsync(await _fPrInspectionCutpcsTransfer.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(cpId)))));
        //    }
        //    catch (Exception )
        //    {
        //        return View($"Error");
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> EditFprInspectionCutPcsTransferInfo(string cpId)
        {
            var fPrInspectionCutpcsTransfer = await _fPrInspectionCutpcsTransfer.FindByIdAsync(int.Parse(_protector.Unprotect(cpId)));

            var fprInspectionCutPcsTransferViewModel = new FprInspectionCutPcsTransferViewModel();
            if (fPrInspectionCutpcsTransfer != null)
            {
                fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer = fPrInspectionCutpcsTransfer;
                fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.EncryptedId = _protector.Protect(fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CPID.ToString());
                return View(await _fPrInspectionCutpcsTransfer.GetInitObjByAsync(fprInspectionCutPcsTransferViewModel));
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return View(fprInspectionCutPcsTransferViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditFprInspectionCutPcsTransferInfo(FprInspectionCutPcsTransferViewModel fprInspectionCutPcsTransferViewModel)
        {
            if (ModelState.IsValid)
            {
                fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CPID = int.Parse(_protector.Unprotect(fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.EncryptedId));
                var fPrInspectionWastageTransfer = await _fPrInspectionCutpcsTransfer.FindByIdAsync(fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CPID);
                if (fPrInspectionWastageTransfer != null)
                {
                    fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.UPDATED_AT = DateTime.Now;
                    fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CREATED_AT = fPrInspectionWastageTransfer.CREATED_AT;
                    fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer.CREATED_BY = fPrInspectionWastageTransfer.CREATED_BY;

                    if (await _fPrInspectionCutpcsTransfer.Update(fprInspectionCutPcsTransferViewModel.FPrInspectionCutpcsTransfer))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFprInspectionCutPcsTransferInfo), "FprInspectionCutPcsTransfer");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(fprInspectionCutPcsTransferViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(fprInspectionCutPcsTransferViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fprInspectionCutPcsTransferViewModel);
        }

    }
}
