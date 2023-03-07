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
    public class FBasDesignationController : Controller
    {
        private readonly IF_BAS_DESIGNATION _fBasDesignation;
        private readonly IDataProtector _protector;

        public FBasDesignationController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_DESIGNATION fBasDesignation
        )
        {
            _fBasDesignation = fBasDesignation;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fBasDesignation"><see cref="F_BAS_DESIGNATION"/></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsDesignationNameInUse(F_BAS_DESIGNATION fBasDesignation)
        {
            var findByDesignationNameAsync = await _fBasDesignation.FindByDesignationNameAsync(fBasDesignation.DESNAME);
            return findByDesignationNameAsync ? Json($"Designation name [{fBasDesignation.DESNAME}] is already in use") : Json(true);
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
                var data = (List<F_BAS_DESIGNATION>) await _fBasDesignation.GetAll();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.DESNAME.ToUpper().Contains(searchValue)
                                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.DESID.ToString());
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
        [Route("Employees/Designations")]
        [Route("Employees/Designations/GetAll")]
        public IActionResult GetEmpDesignation()
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
        public async Task<IActionResult> DeleteEmpDesignation(string id)
        {
            try
            {
                var designation = await _fBasDesignation.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (designation != null)
                {
                    var result = await _fBasDesignation.Delete(designation);

                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Designation.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetEmpDesignation", $"FBasDesignation");
                    }
                    TempData["message"] = "Failed to Delete Designation.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetEmpDesignation", $"FBasDesignation");
                }

                TempData["message"] = "Designation Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDesignation", $"FBasDesignation");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Delete Designation.";
                TempData["type"] = "error";
                return RedirectToAction($"GetEmpDesignation", $"FBasDesignation");
            }
        }
        
        [HttpGet]
        public IActionResult CreateEmpDesignation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpDesignation(F_BAS_DESIGNATION designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fBasDesignation.InsertByAsync(designation);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Designation.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetEmpDesignation", $"FBasDesignation");
                    }

                    TempData["message"] = "Failed to Add Designation.";
                    TempData["type"] = "error";
                    return View(designation);
                }
                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(designation);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Designation.";
                TempData["type"] = "error";
                return View(designation);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditEmpDesignation(string id)
        {
            try
            {
                var result = await _fBasDesignation.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.DESID.ToString());
                    ModelState.Clear();
                    return View(result);
                }
                TempData["message"] = "Failed to Retrieve Designation.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDesignation", $"FBasDesignation");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Designation.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpDesignation", $"FBasDesignation");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditEmpDesignation(F_BAS_DESIGNATION designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var des = await _fBasDesignation.FindByIdAsync(int.Parse(_protector.Unprotect(designation.EncryptedId)));

                    if (des != null)
                    {
                        var result = await _fBasDesignation.Update(designation);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Designation.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetEmpDesignation", $"FBasDesignation");
                        }
                        TempData["message"] = "Failed to Update Designation.";
                        TempData["type"] = "error";
                        return View(designation);
                    }

                    TempData["message"] = "Designation Not Found.";
                    TempData["type"] = "error";
                    return View(designation);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(designation);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Designation.";
                TempData["type"] = "error";
                return View(designation);
            }
        }
    }
}