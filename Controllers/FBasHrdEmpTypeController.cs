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
    [Route("EmployeeType")]
    public class FBasHrdEmpTypeController : Controller
    {
        private readonly IF_BAS_HRD_EMP_TYPE _fBasHrdEmpType;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private const string Title = "Employee Type Information";

        public FBasHrdEmpTypeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_EMP_TYPE fBasHrdEmpType)
        {
            _fBasHrdEmpType = fBasHrdEmpType;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdEmpType()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdEmpType()
        {
            ViewData["Title"] = Title;
            return View(new FBasHrdEmpTypeViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdEmpType(FBasHrdEmpTypeViewModel fBasHrdEmpTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdEmpTypeViewModel.FBasHrdEmpType.CREATED_AT = fBasHrdEmpTypeViewModel.FBasHrdEmpType.UPDATED_AT = DateTime.Now;
                fBasHrdEmpTypeViewModel.FBasHrdEmpType.CREATED_BY = fBasHrdEmpTypeViewModel.FBasHrdEmpType.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdEmpType.InsertByAsync(fBasHrdEmpTypeViewModel.FBasHrdEmpType))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdEmpType));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdEmpTypeViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdEmpTypeViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdEmpType(string id)
        {
            try
            {
                var fBasHrdEmpTypeViewModel = new FBasHrdEmpTypeViewModel
                {
                    FBasHrdEmpType = await _fBasHrdEmpType.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdEmpTypeViewModel.FBasHrdEmpType.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdEmpTypeViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdEmpType(FBasHrdEmpTypeViewModel fBasHrdEmpTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdEmpType = await _fBasHrdEmpType.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdEmpTypeViewModel.FBasHrdEmpType.EncryptedId)));
                if (fBasHrdEmpType != null)
                {
                    fBasHrdEmpTypeViewModel.FBasHrdEmpType.TYPEID = fBasHrdEmpType.TYPEID;
                    fBasHrdEmpTypeViewModel.FBasHrdEmpType.TYPE_NAME = fBasHrdEmpType.TYPE_NAME;
                    fBasHrdEmpTypeViewModel.FBasHrdEmpType.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdEmpTypeViewModel.FBasHrdEmpType.UPDATED_AT = DateTime.Now;
                    fBasHrdEmpTypeViewModel.FBasHrdEmpType.CREATED_AT = fBasHrdEmpType.CREATED_AT;
                    fBasHrdEmpTypeViewModel.FBasHrdEmpType.CREATED_BY = fBasHrdEmpType.CREATED_BY;

                    if (await _fBasHrdEmpType.Update(fBasHrdEmpTypeViewModel.FBasHrdEmpType))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdEmpType));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdEmpType));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdEmpType));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdEmpTypeViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdEmpType(string id)
        {
            var fBasHrdEmpType = await _fBasHrdEmpType.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdEmpType != null)
            {
                if (await _fBasHrdEmpType.Delete(fBasHrdEmpType))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdEmpType));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdEmpType));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdEmpType));
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
                var data = await _fBasHrdEmpType.GetAllFBasHrdEmpTypeAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.TYPE_NAME != null && m.TYPE_NAME.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> IsEmpTypeInUse(FBasHrdEmpTypeViewModel fBasHrdEmpTypeViewModel)
        {
            var typeName = fBasHrdEmpTypeViewModel.FBasHrdEmpType.TYPE_NAME;
            return await _fBasHrdEmpType.FindByEmpTypeAsync(typeName) ? Json(true) : Json($"Type {typeName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetEmpTypes")]
        public async Task<IActionResult> GetAllEmpTypes()
        {
            try
            {
                return Ok(await _fBasHrdEmpType.GetAllEmpTypesAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
