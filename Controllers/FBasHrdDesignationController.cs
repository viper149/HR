using System;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInterfaces.HR;
using HRMS.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("Designation")]
    public class FBasHrdDesignationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_BAS_HRD_DESIGNATION _fBasHrdDesignation;
        private readonly IDataProtector _protector;
        private const string Title = "Designation Information";

        public FBasHrdDesignationController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_DESIGNATION fBasHrdDesignation)
        {
            _userManager = userManager;
            _fBasHrdDesignation = fBasHrdDesignation;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdDesignation()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdDesignation()
        {
            ViewData["Title"] = Title;
            return View(await _fBasHrdDesignation.GetInitObjByAsync(new FBasHrdDesignationViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdDesignation(FBasHrdDesignationViewModel fBasHrdDesignationViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdDesignationViewModel.FBasHrdDesignation.CREATED_AT = fBasHrdDesignationViewModel.FBasHrdDesignation.UPDATED_AT = DateTime.Now;
                fBasHrdDesignationViewModel.FBasHrdDesignation.CREATED_BY = fBasHrdDesignationViewModel.FBasHrdDesignation.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdDesignation.InsertByAsync(fBasHrdDesignationViewModel.FBasHrdDesignation))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdDesignation));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdDesignationViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdDesignationViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdDesignation(string id)
        {
            try
            {
                var fBasHrdDesignationViewModel = await _fBasHrdDesignation.GetInitObjByAsync(new FBasHrdDesignationViewModel());
                fBasHrdDesignationViewModel.FBasHrdDesignation = await _fBasHrdDesignation.FindByIdAsync(int.Parse(_protector.Unprotect(id)));
                fBasHrdDesignationViewModel.FBasHrdDesignation.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdDesignationViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdDesignation(FBasHrdDesignationViewModel fBasHrdDesignationViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdDesignation = await _fBasHrdDesignation.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdDesignationViewModel.FBasHrdDesignation.EncryptedId)));
                if (fBasHrdDesignation != null)
                {
                    fBasHrdDesignationViewModel.FBasHrdDesignation.DESID = fBasHrdDesignation.DESID;
                    fBasHrdDesignationViewModel.FBasHrdDesignation.DES_NAME = fBasHrdDesignation.DES_NAME;
                    fBasHrdDesignationViewModel.FBasHrdDesignation.DES_NAME_BN = fBasHrdDesignation.DES_NAME_BN;
                    fBasHrdDesignationViewModel.FBasHrdDesignation.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdDesignationViewModel.FBasHrdDesignation.UPDATED_AT = DateTime.Now;
                    fBasHrdDesignationViewModel.FBasHrdDesignation.CREATED_AT = fBasHrdDesignation.CREATED_AT;
                    fBasHrdDesignationViewModel.FBasHrdDesignation.CREATED_BY = fBasHrdDesignation.CREATED_BY;

                    if (await _fBasHrdDesignation.Update(fBasHrdDesignationViewModel.FBasHrdDesignation))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdDesignation));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdDesignation));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdDesignation));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdDesignationViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdDesignation(string id)
        {
            var fBasHrdDesignation = await _fBasHrdDesignation.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdDesignation != null)
            {
                if (await _fBasHrdDesignation.Delete(fBasHrdDesignation))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdDesignation));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdDesignation));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdDesignation));
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
                var data = await _fBasHrdDesignation.GetAllFBasHrdDesignationAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.DES_NAME != null && m.DES_NAME.ToUpper().Contains(searchValue))
                                           || (m.DES_NAME_BN != null && m.DES_NAME_BN.ToUpper().Contains(searchValue))
                                           || (m.SHORT_NAME != null && m.SHORT_NAME.ToUpper().Contains(searchValue))
                                           || (m.SHORT_NAME_BN != null && m.SHORT_NAME_BN.ToUpper().Contains(searchValue))
                                           || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
                                           || (m.GRADE != null && m.GRADE.GRADE_NAME.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> IsDesInUse(FBasHrdDesignationViewModel fBasHrdDesignationViewModel)
        {
            var desName = fBasHrdDesignationViewModel.FBasHrdDesignation.DES_NAME;
            return await _fBasHrdDesignation.FindByDesNameAsync(desName) ? Json(true) : Json($"Designation {desName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUseBn")]
        public async Task<IActionResult> IsDesBnInUse(FBasHrdDesignationViewModel fBasHrdDesignationViewModel)
        {
            var desName = fBasHrdDesignationViewModel.FBasHrdDesignation.DES_NAME_BN;
            return await _fBasHrdDesignation.FindByDesNameAsync(desName, true) ? Json(true) : Json($"ইতিমধ্যে {desName} নামের উপাধি আছে");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetDesignations")]
        public async Task<IActionResult> GetAllDesignations ()
        {
            try
            {
                return Ok(await _fBasHrdDesignation.GetAllDesignationsAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
