using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FBasDepartmentController : Controller
    {
        private readonly IF_BAS_DEPARTMENT _fBasDepartment;
        private readonly IDataProtector _protector;

        public FBasDepartmentController(IDataProtectionProvider dataProtectionProvider,
             DataProtectionPurposeStrings dataProtectionPurposeStrings,
             IF_BAS_DEPARTMENT fBasDepartment)
        {
            _fBasDepartment = fBasDepartment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsDepartmentNameInUse(F_BAS_DEPARTMENT fBasDepartment)
        {
            var findByDepartmentNameAsync = await _fBasDepartment.FindByDepartmentNameAsync(fBasDepartment.DEPTNAME);
            return findByDepartmentNameAsync ? Json($"Department Name [{fBasDepartment.DEPTNAME}] is already in use.") : Json(true);
        }

        [HttpPost]
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
                var data = (List<F_BAS_DEPARTMENT>)await _fBasDepartment.GetAll();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.DEPTNAME.ToUpper().Contains(searchValue)
                                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.DEPTID.ToString());
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = finalData
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteEmpDepartment(string id)
        {
            try
            {
                var designation = await _fBasDepartment.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (designation != null)
                {
                    var result = await _fBasDepartment.Delete(designation);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Department.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
                    }
                    TempData["message"] = "Failed to Delete Department.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
                }
                TempData["message"] = "Department Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Delete Department.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
            }
        }

        [HttpGet]
        [Route("Employees/Departments")]
        [Route("Employees/Departments/GetAll")]
        public IActionResult GetEmpDepartment()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditEmpDepartment(string id)
        {
            try
            {
                var result = await _fBasDepartment.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.DEPTID.ToString());
                    ModelState.Clear();
                    return View(result);
                }
                TempData["message"] = "Failed to Retrieve Department.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Department.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditEmpDepartment(F_BAS_DEPARTMENT department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var des = await _fBasDepartment.FindByIdAsync(int.Parse(_protector.Unprotect(department.EncryptedId)));

                    if (des != null)
                    {
                        var result = await _fBasDepartment.Update(department);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Department.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
                        }

                        TempData["message"] = "Failed to Update Department.";
                        TempData["type"] = "error";
                        return View(department);
                    }

                    TempData["message"] = "Department Not Found.";
                    TempData["type"] = "error";
                    return View(department);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(department);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Department.";
                TempData["type"] = "error";
                return View(department);
            }
        }

        [HttpGet]
        public IActionResult CreateEmpDepartment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpDepartment(F_BAS_DEPARTMENT department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fBasDepartment.InsertByAsync(department);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Department.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetEmpDepartment", $"FBasDepartment");
                    }

                    TempData["message"] = "Failed to Add Department.";
                    TempData["type"] = "error";
                    return View(department);
                }
                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(department);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Department.";
                TempData["type"] = "error";
                return View(department);
            }
        }
    }
}