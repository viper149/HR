using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.HR;
using DenimERP.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Route("OutReason")]
    public class FBasHrdOutReasonController : Controller
    {
        private readonly IF_BAS_HRD_OUT_REASON _fBasHrdOutReason;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private const string Title = "Out Reason";

        public FBasHrdOutReasonController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_OUT_REASON fBasHrdOutReason)
        {
            _fBasHrdOutReason = fBasHrdOutReason;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdOutReason()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdOutReason()
        {
            ViewData["Title"] = Title;
            return View(new FBasHrdOutReasonViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdOutReason(FBasHrdOutReasonViewModel fBasHrdOutReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdOutReasonViewModel.FBasHrdOutReason.CREATED_AT = fBasHrdOutReasonViewModel.FBasHrdOutReason.UPDATED_AT = DateTime.Now;
                fBasHrdOutReasonViewModel.FBasHrdOutReason.CREATED_BY = fBasHrdOutReasonViewModel.FBasHrdOutReason.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdOutReason.InsertByAsync(fBasHrdOutReasonViewModel.FBasHrdOutReason))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdOutReason));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdOutReasonViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdOutReasonViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdOutReason(string id)
        {
            try
            {
                var fBasHrdOutReasonViewModel = new FBasHrdOutReasonViewModel
                {
                    FBasHrdOutReason = await _fBasHrdOutReason.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdOutReasonViewModel.FBasHrdOutReason.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdOutReasonViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdOutReason(FBasHrdOutReasonViewModel fBasHrdOutReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdOutReasonE = await _fBasHrdOutReason.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdOutReasonViewModel.FBasHrdOutReason.EncryptedId)));
                if (fBasHrdOutReasonE != null)
                {
                    fBasHrdOutReasonViewModel.FBasHrdOutReason.RESASON_ID = fBasHrdOutReasonE.RESASON_ID;
                    fBasHrdOutReasonViewModel.FBasHrdOutReason.RESASON_NAME = fBasHrdOutReasonE.RESASON_NAME;
                    fBasHrdOutReasonViewModel.FBasHrdOutReason.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdOutReasonViewModel.FBasHrdOutReason.UPDATED_AT = DateTime.Now;
                    fBasHrdOutReasonViewModel.FBasHrdOutReason.CREATED_AT = fBasHrdOutReasonE.CREATED_AT;
                    fBasHrdOutReasonViewModel.FBasHrdOutReason.CREATED_BY = fBasHrdOutReasonE.CREATED_BY;

                    if (await _fBasHrdOutReason.Update(fBasHrdOutReasonViewModel.FBasHrdOutReason))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdOutReason));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdOutReason));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdOutReason));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdOutReasonViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdOutReason(string id)
        {
            var fBasHrdOutReason = await _fBasHrdOutReason.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdOutReason != null)
            {
                if (await _fBasHrdOutReason.Delete(fBasHrdOutReason))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdOutReason));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdOutReason));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdOutReason));
        }

        [HttpPost]
        [Route("GetData")]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var data = await _fBasHrdOutReason.GetAllFBasHrdOutReasonAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.RESASON_NAME != null && m.RESASON_NAME.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();
                return Json(new
                {
                    draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal,
                    data = finalData
                });
            }
            catch (Exception e)
            {
                return Json(BadRequest(e));
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUse")]
        public async Task<IActionResult> IsOutReasonInUse(FBasHrdOutReasonViewModel fBasHrdOutReasonViewModel)
        {
            var reason = fBasHrdOutReasonViewModel.FBasHrdOutReason.RESASON_NAME;
            return await _fBasHrdOutReason.FindByOutReasonAsync(reason) ? Json(true) : Json($"Out Reason {reason} already exists");
        }
    }
}
