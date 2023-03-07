using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ComTenorController : Controller
    {
        private readonly ICOM_TENOR _comTenor;
        private readonly IDataProtector _protector;

        public ComTenorController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_TENOR comTenor
        )
        {
            _comTenor = comTenor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }



        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsTenorInUse(COM_TENOR comTenor)
        {
            var type = await _comTenor.FindByTypeName(comTenor.NAME);
            return !type ? Json(true) : Json($"Tenor Name [ {comTenor.NAME} ] is already in use");
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

                var data = (List<COM_TENOR>)await _comTenor.GetAllForDataTableByAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.NAME.ToUpper().Contains(searchValue)
                                        || m.COST.ToString(CultureInfo.InvariantCulture).ToUpper().Contains(searchValue)
                                        || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize);

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
        public IActionResult GetComTenorWithPaged()
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
        public async Task<IActionResult> DeleteTenor(string tId)
        {
            try
            {
                var tenor = await _comTenor.FindByIdAsync(int.Parse(_protector.Unprotect(tId)));

                if (tenor != null)
                {
                    var result = await _comTenor.Delete(tenor);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Tenor.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetComTenorWithPaged", $"ComTenor");
                    }
                    TempData["message"] = "Failed to Delete Tenor.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetComTenorWithPaged", $"ComTenor");
                }
                TempData["message"] = "Tenor Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetComTenorWithPaged", $"ComTenor");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Delete Tenor.";
                TempData["type"] = "error";
                return RedirectToAction("GetComTenorWithPaged", $"ComTenor");
            }
        }

        [HttpGet]
        public IActionResult TenorReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateTenor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenor(COM_TENOR comTenor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    comTenor.CODE_LEVEL = 100;
                    var result = await _comTenor.InsertByAsync(comTenor);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Tenor.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetComTenorWithPaged", $"ComTenor");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Tenor.";
                        TempData["type"] = "error";
                        return View(comTenor);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(comTenor);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Tenor.";
                TempData["type"] = "error";
                return View(comTenor);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditTenor(string tId)
        {
            try
            {
                var result = await _comTenor.FindByIdAsync(int.Parse(_protector.Unprotect(tId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.TID.ToString());
                    ModelState.Clear();
                    return View(result);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Tenor.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetComTenorWithPaged", $"ComTenor");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Tenor.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComTenorWithPaged", $"ComTenor");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditTenor(COM_TENOR comTenor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tenor = await _comTenor.FindByIdAsync(int.Parse(_protector.Unprotect(comTenor.EncryptedId)));

                    if (tenor != null)
                    {
                        comTenor.CODE_LEVEL = tenor.CODE_LEVEL;
                        if (await _comTenor.Update(comTenor))
                        {
                            TempData["message"] = "Successfully Updated Tenor.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetComTenorWithPaged", $"ComTenor");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Tenor.";
                            TempData["type"] = "error";
                            return View(comTenor);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Update Tenor.";
                        TempData["type"] = "error";
                        return View(comTenor);
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input, Please Try Again.";
                    TempData["type"] = "error";
                    return View(comTenor);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Tenor.";
                TempData["type"] = "error";
                return View(comTenor);
            }
        }
    }
}