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
    public class FBasSubSectionController : Controller
    {
        private readonly IF_BAS_SUBSECTION _fBasSubSection;
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IDataProtector _protector;

        public FBasSubSectionController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_SUBSECTION fBasSubSection,
            IF_BAS_SECTION fBasSection
        )
        {
            _fBasSubSection = fBasSubSection;
            _fBasSection = fBasSection;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsSubSectionNameInUse(F_BAS_SUBSECTION fBasSubsection)
        {
            var findBySubSectionNameAsync = await _fBasSubSection.FindBySubSectionNameAsync(fBasSubsection.SSECNAME);
            return findBySubSectionNameAsync ? Json($"Sub-Section name [{fBasSubsection.SSECNAME}] is already in use.") : Json(true);
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
                var data = (List<F_BAS_SUBSECTION>)await _fBasSubSection.GetAll();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SSECNAME.ToUpper().Contains(searchValue)
                                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.SSECID.ToString());
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
        [Route("Employees/Sub-Sections")]
        [Route("Employees/Sub-Sections/GetAll")]
        public IActionResult GetEmpSubSection()
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
        public async Task<IActionResult> CreateEmpSubSection()
        {
            try
            {
                var fBasSubSectionViewModel = new FBasSubSectionViewModel
                {
                    FBasSections = (List<F_BAS_SECTION>)await _fBasSection.GetAll()
                };

                return View(fBasSubSectionViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpSubSection(FBasSubSectionViewModel fBasSubSectionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fBasSubSection.InsertByAsync(fBasSubSectionViewModel.FBasSubSection);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added SubSection.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
                    }

                    fBasSubSectionViewModel.FBasSections = (List<F_BAS_SECTION>) await _fBasSection.GetAll();

                    TempData["message"] = "Failed to Add SubSection.";
                    TempData["type"] = "error";
                    return View(fBasSubSectionViewModel);
                }

                fBasSubSectionViewModel.FBasSections = (List<F_BAS_SECTION>)await _fBasSection.GetAll();

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fBasSubSectionViewModel);
            }
            catch (Exception)
            {
                fBasSubSectionViewModel.FBasSections = (List<F_BAS_SECTION>)await _fBasSection.GetAll();

                TempData["message"] = "Failed to Add SubSection.";
                TempData["type"] = "error";
                return View(fBasSubSectionViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEmpSubSection(string id)
        {
            try
            {
                var subSection = await _fBasSubSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (subSection != null)
                {
                    var result = await _fBasSubSection.Delete(subSection);

                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Sub-Section.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
                    }

                    TempData["message"] = "Failed to Delete Sub-Section.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
                }

                TempData["message"] = "Sub-Section Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Delete Sub-Section.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditEmpSubSection(string id)
        {
            try
            {
                var result = await _fBasSubSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.SSECID.ToString());

                    var fBasSubSectionViewModel = new FBasSubSectionViewModel
                    {
                        FBasSections = (List<F_BAS_SECTION>)await _fBasSection.GetAll(),
                        FBasSubSection = result
                    };

                    return View(fBasSubSectionViewModel);
                }

                TempData["message"] = "Failed to Retrieve Sub-Section.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Sub-Section.";
                TempData["type"] = "error";
                return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEmpSubSection(FBasSubSectionViewModel fBasSubSectionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var des = await _fBasSubSection.FindByIdAsync(int.Parse(_protector.Unprotect(fBasSubSectionViewModel.FBasSubSection.EncryptedId)));

                    if (des != null)
                    {
                        var result = await _fBasSubSection.Update(fBasSubSectionViewModel.FBasSubSection);
                        
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Sub-Section.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetEmpSubSection", $"FBasSubSection");
                        }

                        TempData["message"] = "Failed to Update Sub-Section.";
                        TempData["type"] = "error";
                        return View(fBasSubSectionViewModel);
                    }

                    TempData["message"] = "Sub-Section Not Found.";
                    TempData["type"] = "error";
                    return View(fBasSubSectionViewModel);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(fBasSubSectionViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Sub-Section.";
                TempData["type"] = "error";
                return View(fBasSubSectionViewModel);
            }
        }
    }
}