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
    [Route("Grade")]
    public class FBasHrdGradeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_BAS_HRD_GRADE _fBasHrdGrade;
        private readonly IDataProtector _protector;
        private const string Title = "Grade Information";

        public FBasHrdGradeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_GRADE fBasHrdGrade)
        {
            _userManager = userManager;
            _fBasHrdGrade = fBasHrdGrade;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdGrade()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdGrade()
        {
            ViewData["Title"] = Title;
            return View(new FBasHrdGradeViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdGrade(FBasHrdGradeViewModel fBasHrdGradeViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdGradeViewModel.FBasHrdGrade.CREATED_AT = fBasHrdGradeViewModel.FBasHrdGrade.UPDATED_AT = DateTime.Now;
                fBasHrdGradeViewModel.FBasHrdGrade.CREATED_BY = fBasHrdGradeViewModel.FBasHrdGrade.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdGrade.InsertByAsync(fBasHrdGradeViewModel.FBasHrdGrade))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdGrade));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdGradeViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdGradeViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdGrade(string id)
        {

            try
            {
                var fBasHrdGradeViewModel = new FBasHrdGradeViewModel
                {
                    FBasHrdGrade = await _fBasHrdGrade.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdGradeViewModel.FBasHrdGrade.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdGradeViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdGrade(FBasHrdGradeViewModel fBasHrdGradeViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdGrade = await _fBasHrdGrade.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdGradeViewModel.FBasHrdGrade.EncryptedId)));
                if (fBasHrdGrade != null)
                {
                    fBasHrdGradeViewModel.FBasHrdGrade.GRADEID = fBasHrdGrade.GRADEID;
                    fBasHrdGradeViewModel.FBasHrdGrade.GRADE_NAME = fBasHrdGrade.GRADE_NAME;
                    fBasHrdGradeViewModel.FBasHrdGrade.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdGradeViewModel.FBasHrdGrade.UPDATED_AT = DateTime.Now;
                    fBasHrdGradeViewModel.FBasHrdGrade.CREATED_AT = fBasHrdGrade.CREATED_AT;
                    fBasHrdGradeViewModel.FBasHrdGrade.CREATED_BY = fBasHrdGrade.CREATED_BY;

                    if (await _fBasHrdGrade.Update(fBasHrdGradeViewModel.FBasHrdGrade))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdGrade));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdGrade));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdGrade));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdGradeViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdGrade(string id)
        {
            var fBasHrdGrade = await _fBasHrdGrade.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdGrade != null)
            {
                if (await _fBasHrdGrade.Delete(fBasHrdGrade))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdGrade));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdGrade));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdGrade));
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
                var data = await _fBasHrdGrade.GetAllFBasHrdGradeAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.GRADE_NAME != null && m.GRADE_NAME.ToUpper().Contains(searchValue))
                                           || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> IsGradeNameInUse(FBasHrdGradeViewModel fBasHrdGradeViewModel)
        {
            var gradeName = fBasHrdGradeViewModel.FBasHrdGrade.GRADE_NAME;
            return await _fBasHrdGrade.FindByGradeNameAsync(gradeName) ? Json(true) : Json($"Grade {gradeName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetGrades")]
        public async Task<IActionResult> GetAllGrades()
        {
            try
            {
                return Ok(await _fBasHrdGrade.GetAllGradesAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
