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
    [Route("Department")]
    public class FBasHrdDepartmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_BAS_HRD_DEPARTMENT _fBasHrdDepartment;
        private readonly IDataProtector _protector;
        private const string Title = "Department Information";

        public FBasHrdDepartmentController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_DEPARTMENT fBasHrdDepartment)
        {
            _userManager = userManager;
            _fBasHrdDepartment = fBasHrdDepartment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdDepartment()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdDepartment()
        {
            ViewData["Title"] = Title;
            return View(await _fBasHrdDepartment.GetInitObjByAsync(new FBasHrdDepartmentViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdDepartment(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdDepartmentViewModel.FBasHrdDepartment.CREATED_AT = fBasHrdDepartmentViewModel.FBasHrdDepartment.UPDATED_AT = DateTime.Now;
                fBasHrdDepartmentViewModel.FBasHrdDepartment.CREATED_BY = fBasHrdDepartmentViewModel.FBasHrdDepartment.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdDepartment.InsertByAsync(fBasHrdDepartmentViewModel.FBasHrdDepartment))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdDepartment));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdDepartmentViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdDepartmentViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdDepartment(string id)
        {

            try
            {
                var fBasHrdDepartmentViewModel = await _fBasHrdDepartment.GetInitObjByAsync(new FBasHrdDepartmentViewModel());
                fBasHrdDepartmentViewModel.FBasHrdDepartment = await _fBasHrdDepartment.FindByIdAsync(int.Parse(_protector.Unprotect(id)));
                fBasHrdDepartmentViewModel.FBasHrdDepartment.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdDepartmentViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdDepartment(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdDepartment = await _fBasHrdDepartment.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdDepartmentViewModel.FBasHrdDepartment.EncryptedId)));
                if (fBasHrdDepartment != null)
                {
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.DEPTID = fBasHrdDepartment.DEPTID;
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.DEPTNAME = fBasHrdDepartment.DEPTNAME;
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.DEPTNAME_BN = fBasHrdDepartment.DEPTNAME_BN;
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.UPDATED_AT = DateTime.Now;
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.CREATED_AT = fBasHrdDepartment.CREATED_AT;
                    fBasHrdDepartmentViewModel.FBasHrdDepartment.CREATED_BY = fBasHrdDepartment.CREATED_BY;

                    if (await _fBasHrdDepartment.Update(fBasHrdDepartmentViewModel.FBasHrdDepartment))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdDepartment));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdDepartment));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdDepartment));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdDepartmentViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdDepartment(string id)
        {
            var fBasHrdDepartment = await _fBasHrdDepartment.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdDepartment != null)
            {
                if (await _fBasHrdDepartment.Delete(fBasHrdDepartment))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdDepartment));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdDepartment));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdDepartment));
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
                var data = await _fBasHrdDepartment.GetAllFBasHrdDepartmentAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.DEPTNAME != null && m.DEPTNAME.ToUpper().Contains(searchValue))
                                           || (m.DEPTNAME_BN != null && m.DEPTNAME_BN.ToUpper().Contains(searchValue))
                                           || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
                                           || (m.LOCATION != null && m.LOCATION.LOC_NAME.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> IsDeptNameInUse(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel)
        {
            var deptName = fBasHrdDepartmentViewModel.FBasHrdDepartment.DEPTNAME;
            return await _fBasHrdDepartment.FindByDeptNameAsync(deptName) ? Json(true) : Json($"Department {deptName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUseBn")]
        public async Task<IActionResult> IsDeptNameBnInUse(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel)
        {
            var deptName = fBasHrdDepartmentViewModel.FBasHrdDepartment.DEPTNAME_BN;
            return await _fBasHrdDepartment.FindByDeptNameAsync(deptName,true) ? Json(true) : Json($"ইতিমধ্যে {deptName} নামের ডিপার্টমেন্ট আছে");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetAllDepartments ()
        {
            try
            {
                return Ok(await _fBasHrdDepartment.GetAllDepartmentsAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
