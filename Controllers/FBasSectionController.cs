using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FBasSectionController : Controller
    {
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IF_BAS_DEPARTMENT _fBasDepartment;
        private readonly IDataProtector _protector;

        public FBasSectionController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_SECTION fBasSection,
            IF_BAS_DEPARTMENT fBasDepartment
        )
        {
            _fBasSection = fBasSection;
            _fBasDepartment = fBasDepartment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsSectionNameInUse(F_BAS_SECTION fBasSection)
        {
            var findBySectionNameAsync = await _fBasSection.FindBySectionNameAsync(fBasSection.SECNAME);
            return findBySectionNameAsync ? Json($"Section name [{fBasSection.SECNAME}] is already in use.") : Json(true);
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
                var data = (List<F_BAS_SECTION>)await _fBasSection.GetAll();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SECNAME.ToUpper().Contains(searchValue)
                                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.SECID.ToString());
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
        [Route("Employees/Sections")]
        [Route("Employees/Sections/GetAll")]
        public IActionResult GetEmpSection()
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
        public async Task<IActionResult> DeleteEmpSection(string id)
        {
            try
            {
                var section = await _fBasSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (section != null)
                {
                    var result = await _fBasSection.Delete(section);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Section.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetEmpSection", $"FBasSection");
                    }
                    TempData["message"] = "Failed to Delete Section.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetEmpSection", $"FBasSection");
                }
                TempData["message"] = "Section Not Found.";
                TempData["type"] = "error";
                return RedirectToAction($"GetEmpSection", $"FBasSection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Delete Section.";
                TempData["type"] = "error";
                return RedirectToAction($"GetEmpSection", $"FBasSection");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmpSection()
        {
            try
            {
                var fBasSectionViewModel = new FBasSectionViewModel
                {
                    FBasDepartments = (List<F_BAS_DEPARTMENT>)await _fBasDepartment.GetAll()
                };

                return View(fBasSectionViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpSection(FBasSectionViewModel fBasSectionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fBasSection.InsertByAsync(fBasSectionViewModel.FBasSection);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Section.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetEmpSection", $"FBasSection");
                    }

                    TempData["message"] = "Failed to Add Section.";
                    TempData["type"] = "error";
                    return View(fBasSectionViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fBasSectionViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Section.";
                TempData["type"] = "error";
                return View(fBasSectionViewModel);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditEmpSection(string id)
        {
            try
            {
                var result = await _fBasSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.SECID.ToString());

                    var fBasSectionViewModel = new FBasSectionViewModel
                    {
                        FBasDepartments = (List<F_BAS_DEPARTMENT>) await _fBasDepartment.GetAll(),
                        FBasSection = result
                    };

                    return View(fBasSectionViewModel);
                }

                TempData["message"] = "Failed to Retrieve Section.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpSection", $"FBasSection");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Section.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpSection", $"FBasSection");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEmpSection(FBasSectionViewModel fBasSectionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var des = await _fBasSection.FindByIdAsync(int.Parse(_protector.Unprotect(fBasSectionViewModel.FBasSection.EncryptedId)));

                    if (des != null)
                    {
                        var result = await _fBasSection.Update(fBasSectionViewModel.FBasSection);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Section.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetEmpSection", $"FBasSection");
                        }

                        TempData["message"] = "Failed to Update Section.";
                        TempData["type"] = "error";
                        return View(fBasSectionViewModel);
                    }

                    TempData["message"] = "Section Not Found.";
                    TempData["type"] = "error";
                    return View(fBasSectionViewModel);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(fBasSectionViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Section.";
                TempData["type"] = "error";
                return View(fBasSectionViewModel);
            }
        }
    }
}