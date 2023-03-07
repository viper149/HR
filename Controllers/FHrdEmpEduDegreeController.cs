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
    [Route("EduDegree")]
    public class FHrdEmpEduDegreeController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_HRD_EMP_EDU_DEGREE _fHrdEmpEduDegree;
        private readonly IDataProtector _protector;
        private const string Title = "Education Degree Information";

        public FHrdEmpEduDegreeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_HRD_EMP_EDU_DEGREE fHrdEmpEduDegree)
        {
            _userManager = userManager;
            _fHrdEmpEduDegree = fHrdEmpEduDegree;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFHrdEmpEduDegree()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFHrdEmpEduDegree()
        {
            ViewData["Title"] = Title;
            return View(new FHrdEmpEduDegreeViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFHrdEmpEduDegree(FHrdEmpEduDegreeViewModel fHrdEmpEduDegreeViewModel)
        {
            if (ModelState.IsValid)
            {
                fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.CREATED_AT = fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.UPDATED_AT = DateTime.Now;
                fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.CREATED_BY = fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fHrdEmpEduDegree.InsertByAsync(fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFHrdEmpEduDegree));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fHrdEmpEduDegreeViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fHrdEmpEduDegreeViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFHrdEmpEduDegree(string id)
        {
            try
            {
                var fHrdEmpEduDegreeViewModel = new FHrdEmpEduDegreeViewModel
                {
                    FHrdEmpEduDegree = await _fHrdEmpEduDegree.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fHrdEmpEduDegreeViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFHrdEmpEduDegree(FHrdEmpEduDegreeViewModel fHrdEmpEduDegreeViewModel)
        {
            if (ModelState.IsValid)
            {
                var fHrdEmpEduDegreeE = await _fHrdEmpEduDegree.FindByIdAsync(int.Parse(_protector.Unprotect(fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.EncryptedId)));
                if (fHrdEmpEduDegreeE != null)
                {
                    fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.DEGID = fHrdEmpEduDegreeE.DEGID;
                    fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.DEGNAME = fHrdEmpEduDegreeE.DEGNAME;
                    fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.UPDATED_AT = DateTime.Now;
                    fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.CREATED_AT = fHrdEmpEduDegreeE.CREATED_AT;
                    fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.CREATED_BY = fHrdEmpEduDegreeE.CREATED_BY;

                    if (await _fHrdEmpEduDegree.Update(fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFHrdEmpEduDegree));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFHrdEmpEduDegree));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFHrdEmpEduDegree));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fHrdEmpEduDegreeViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFHrdEmpEduDegree(string id)
        {
            var fHrdEmpEduDegree = await _fHrdEmpEduDegree.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fHrdEmpEduDegree != null)
            {
                if (await _fHrdEmpEduDegree.Delete(fHrdEmpEduDegree))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFHrdEmpEduDegree));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFHrdEmpEduDegree));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFHrdEmpEduDegree));
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
                var data = await _fHrdEmpEduDegree.GetAllFHrdEmpEduDegreeAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.DEGNAME != null && m.DEGNAME.ToUpper().Contains(searchValue))
                                           || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))).ToList();
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
        public async Task<IActionResult> IsDegreeInUse(FHrdEmpEduDegreeViewModel fHrdEmpEduDegreeViewModel)
        {
            var degree = fHrdEmpEduDegreeViewModel.FHrdEmpEduDegree.DEGNAME;
            return await _fHrdEmpEduDegree.FindByDegreeAsync(degree) ? Json(true) : Json($"Degree {degree} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetEduDegrees")]
        public async Task<IActionResult> GetEduDegrees()
        {
            try
            {
                return Ok(await _fHrdEmpEduDegree.GetAllEduDegreesAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
