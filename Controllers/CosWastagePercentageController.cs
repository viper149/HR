using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class CosWastagePercentageController : Controller
    {
        private readonly ICOS_WASTAGE_PERCENTAGE _cosWastagePercentage;
        private readonly IDataProtector _protector;
        private readonly string _title = "Wastage(%)";

        public CosWastagePercentageController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOS_WASTAGE_PERCENTAGE cosWastagePercentage)
        {
            _cosWastagePercentage = cosWastagePercentage;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetCosWastagePercentageList()
        {
            return View();
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
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var data = await _cosWastagePercentage.All();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c)) : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.DESCRIPTION.ToUpper().Contains(searchValue)
                                           || m.VALUE != 0 && m.VALUE.ToString().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));
                }

                var recordsTotal = data.Count();
                var finalData = data.Select(e => new COS_WASTAGE_PERCENTAGE
                {
                    EncryptedId = _protector.Protect(e.WESTAGE_ID.ToString()),
                    DESCRIPTION = e.DESCRIPTION,
                    VALUE = e.VALUE,
                    REMARKS = e.REMARKS
                }).Skip(skip).Take(pageSize).ToList();

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
        public IActionResult CreateCosWastagePercentage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCosWastagePercentage(COS_WASTAGE_PERCENTAGE cosWastagePercentage)
        {
            if (ModelState.IsValid)
            {
                if (cosWastagePercentage != null)
                {
                    if (await _cosWastagePercentage.InsertByAsync(cosWastagePercentage))
                    {
                        TempData["message"] = $"Successfully Added {_title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetCosWastagePercentageList), $"CosWastagePercentage");
                    }

                    TempData["message"] = $"Failed to Add  {_title}";
                    TempData["type"] = "error";
                    return View(cosWastagePercentage);
                }

                TempData["message"] = $"{_title} not found";
                TempData["type"] = "error";
                return View(cosWastagePercentage);
            }

            TempData["message"] = "Invalid Input.";
            TempData["type"] = "error";
            return View(cosWastagePercentage);
        }


        [HttpGet]
        public async Task<IActionResult> EditCostWastagePercentage(string id)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetCosWastagePercentageList), $"CosWastagePercentage");

            try
            {
                var cosWastagePercentage = await _cosWastagePercentage.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (cosWastagePercentage != null)
                {
                    cosWastagePercentage.EncryptedId = _protector.Protect(cosWastagePercentage.WESTAGE_ID.ToString());
                    return View(cosWastagePercentage);
                }
                TempData["message"] = $"{_title} Not Found";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            catch (Exception)
            {
                TempData["message"] = $"Failed to Retrieve {_title}";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCostWastagePercentage(COS_WASTAGE_PERCENTAGE cosWastagePercentage)
        {
            if (ModelState.IsValid)
            {
                if (await _cosWastagePercentage.FindByIdAsync(int.Parse(_protector.Unprotect(cosWastagePercentage.EncryptedId))) != null)
                {
                    if (await _cosWastagePercentage.Update(cosWastagePercentage))
                    {
                        TempData["message"] = $"Successfully Updated {_title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetCosWastagePercentageList), $"CosWastagePercentage");
                    }

                    TempData["message"] = $"Failed to Update {_title}";
                    TempData["type"] = "error";
                    return View(cosWastagePercentage);
                }

                TempData["message"] = $"Failed to Update {_title}";
                TempData["type"] = "error";
                return View(cosWastagePercentage);
            }

            TempData["message"] = "Invalid Input, Please Try Again.";
            TempData["type"] = "error";
            return View(cosWastagePercentage);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCosWastagePercentage(string id)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetCosWastagePercentageList), $"CosWastagePercentage");
            var cosWastagePercentage = await _cosWastagePercentage.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (cosWastagePercentage != null)
            {
                if (await _cosWastagePercentage.Delete(cosWastagePercentage))
                {
                    TempData["message"] = $"Successfully Deleted {_title}";
                    TempData["type"] = "success";
                    return redirectToActionResult;
                }

                TempData["message"] = $"Failed to Delete {_title}";
                TempData["type"] = "error";
                return redirectToActionResult;
            }

            TempData["message"] = $"{_title} Not Found.";
            TempData["type"] = "error";
            return redirectToActionResult;
        }
    }
}
