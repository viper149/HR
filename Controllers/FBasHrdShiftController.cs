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
    [Route("Shift")]
    public class FBasHrdShiftController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_BAS_HRD_SHIFT _fBasHrdShift;
        private readonly IDataProtector _protector;
        private const string Title = "Shift Information";

        public FBasHrdShiftController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_SHIFT fBasHrdShift)
        {
            _userManager = userManager;
            _fBasHrdShift = fBasHrdShift;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdShift()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdShift()
        {
            ViewData["Title"] = Title;
            return View(new FBasHrdShiftViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdShift(FBasHrdShiftViewModel fBasHrdShiftViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdShiftViewModel.FBasHrdShift.CREATED_AT = fBasHrdShiftViewModel.FBasHrdShift.UPDATED_AT = DateTime.Now;
                fBasHrdShiftViewModel.FBasHrdShift.CREATED_BY = fBasHrdShiftViewModel.FBasHrdShift.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdShift.InsertByAsync(fBasHrdShiftViewModel.FBasHrdShift))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdShift));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdShiftViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdShiftViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdShift(string id)
        {
            try
            {
                var fBasHrdShiftViewModel = new FBasHrdShiftViewModel
                {
                    FBasHrdShift = await _fBasHrdShift.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdShiftViewModel.FBasHrdShift.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdShiftViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdShift(FBasHrdShiftViewModel fBasHrdShiftViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdShiftE = await _fBasHrdShift.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdShiftViewModel.FBasHrdShift.EncryptedId)));
                if (fBasHrdShiftE != null)
                {
                    fBasHrdShiftViewModel.FBasHrdShift.SHIFTID = fBasHrdShiftE.SHIFTID;
                    fBasHrdShiftViewModel.FBasHrdShift.SHIFT_NAME = fBasHrdShiftE.SHIFT_NAME;
                    fBasHrdShiftViewModel.FBasHrdShift.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdShiftViewModel.FBasHrdShift.UPDATED_AT = DateTime.Now;
                    fBasHrdShiftViewModel.FBasHrdShift.CREATED_AT = fBasHrdShiftE.CREATED_AT;
                    fBasHrdShiftViewModel.FBasHrdShift.CREATED_BY = fBasHrdShiftE.CREATED_BY;

                    if (await _fBasHrdShift.Update(fBasHrdShiftViewModel.FBasHrdShift))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdShift));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdShift));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdShift));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdShiftViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdShift(string id)
        {
            var fBasHrdShift = await _fBasHrdShift.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdShift != null)
            {
                if (await _fBasHrdShift.Delete(fBasHrdShift))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdShift));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdShift));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdShift));
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
                var data = await _fBasHrdShift.GetAllFBasHrdShiftAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.SHIFT_NAME != null && m.SHIFT_NAME.ToUpper().Contains(searchValue))
                                           || (m.SHORT_NAME != null && m.SHORT_NAME.ToUpper().Contains(searchValue))
                                           || (m.TimeStart != null && m.TimeStart.ToUpper().Contains(searchValue))
                                           || (m.TimeEnd != null && m.TimeEnd.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> IsShiftInUse(FBasHrdShiftViewModel fBasHrdShiftViewModel)
        {
            var shift = fBasHrdShiftViewModel.FBasHrdShift.SHIFT_NAME;
            return await _fBasHrdShift.FindByShiftAsync(shift) ? Json(true) : Json($"Shift {shift} already exists");
        }
    }
}
