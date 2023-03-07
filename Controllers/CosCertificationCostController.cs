using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class CosCertificationCostController : Controller
    {
        private readonly ICOS_CERTIFICATION_COST _cosCertificationCost;
        private readonly IDataProtector protector;

        public CosCertificationCostController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              ICOS_CERTIFICATION_COST cosCertificationCost)
        {
            _cosCertificationCost = cosCertificationCost;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = await _cosCertificationCost.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = protector.Protect(item.CID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.DESCRIPTION.ToUpper().Contains(searchValue)
                                           || (m.VALUE != 0 && m.VALUE.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetCosCertificationCost()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCosCertificationCost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCosCertificationCost(COS_CERTIFICATION_COST cosCertificationCost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cosCertificationCost != null)
                    {
                        var result = await _cosCertificationCost.InsertByAsync(cosCertificationCost);

                        if (result == true)
                        {
                            TempData["message"] = "Successfully Added Certification Cost.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
                        }
                        TempData["message"] = "Failed to Add  Certification Cost..";
                        TempData["type"] = "error";
                        return View(cosCertificationCost);
                    }
                    TempData["message"] = "Failed to Add  Certification Cost..";
                    TempData["type"] = "error";
                    return View(cosCertificationCost);
                }
                TempData["message"] = "Invalid Input.";
                TempData["type"] = "error";
                return View(cosCertificationCost);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["message"] = "Failed to Add  Certification Cost..";
                TempData["type"] = "error";
                return View(cosCertificationCost);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditCosCertificationCost(string id)
        {
            try
            {
                var result = await _cosCertificationCost.FindByIdAsync(int.Parse(protector.Unprotect(id)));

                if (result != null)
                {
                    result.EncryptedId = protector.Protect(result.CID.ToString());
                    return View(result);
                }
                TempData["message"] = "Failed to Retrieve Certification Cost.";
                TempData["type"] = "error";
                return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Certification Cost.";
                TempData["type"] = "error";
                return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditCosCertificationCost(COS_CERTIFICATION_COST cosCertificationCost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var decryptedId = protector.Unprotect(cosCertificationCost.EncryptedId);
                    var decryptedIntId = Convert.ToInt32(decryptedId);

                    var color = await _cosCertificationCost.FindByIdAsync(decryptedIntId);
                    if (color != null)
                    {
                        var result = await _cosCertificationCost.Update(cosCertificationCost);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Certification Cost.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
                        }
                        TempData["message"] = "Failed to Update Certification Cost.";
                        TempData["type"] = "error";
                        return View(cosCertificationCost);
                    }
                    TempData["message"] = "Failed to Update Certification Cost.";
                    TempData["type"] = "error";
                    return View(cosCertificationCost);
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(cosCertificationCost);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Certification Cost.";
                TempData["type"] = "error";
                return View(cosCertificationCost);
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteCosCertificationCost(string id)
        {
            try
            {
                var decryptedId = protector.Unprotect(id);
                var decryptedIntId = Convert.ToInt32(decryptedId);
                var color = await _cosCertificationCost.FindByIdAsync(decryptedIntId);
                if (color != null)
                {
                    var result = await _cosCertificationCost.Delete(color);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Certification Cost.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
                    }
                    TempData["message"] = "Failed to Delete Certification Cost.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
                }
                TempData["message"] = "Color Not Found Certification Cost.";
                TempData["type"] = "error";
                return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Certification Cost.";
                TempData["type"] = "error";
                return RedirectToAction($"GetCosCertificationCost", $"CosCertificationCost");
            }
        }
    }
}