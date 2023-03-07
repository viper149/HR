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
    public class ComTradeTermsController : Controller
    {
        private readonly ICOM_TRADE_TERMS _comTradeTerms;
        private readonly IDataProtector _protector;

        public ComTradeTermsController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_TRADE_TERMS comTradeTerms
        )
        {
            _comTradeTerms = comTradeTerms;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
        
        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> IsTenorInUse(COM_TRADE_TERMS comTradeTerms)
        //{
        //    var type = await _comTradeTerms.FindByTypeName(comTradeTerms.TRADE_TERMS);
        //    return !type ? Json(true) : Json($"Trade Terms [ {comTradeTerms.TRADE_TERMS} ] is already in use");
        //}

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
                var data = await _comTradeTerms.GetAll();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.TRADE_TERMS.ToUpper().Contains(searchValue)
                                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize);

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.TTID.ToString());
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
        public IActionResult GetComTradeTerms()
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
        public async Task<IActionResult> DeleteTradeTerms(string tId)
        {
            try
            {
                var tenor = await _comTradeTerms.FindByIdAsync(int.Parse(_protector.Unprotect(tId)));

                if (tenor != null)
                {
                    var result = await _comTradeTerms.Delete(tenor);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Trade Terms.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
                    }
                    TempData["message"] = "Failed to Delete Trade Terms.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
                }
                TempData["message"] = "Trade Terms Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Delete Trade Terms.";
                TempData["type"] = "error";
                return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
            }
        }

        [HttpGet]
        public IActionResult CreateTradeTerms()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTradeTerms(COM_TRADE_TERMS comTradeTerms)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _comTradeTerms.InsertByAsync(comTradeTerms);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Trade Terms.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Trade Terms.";
                        TempData["type"] = "error";
                        return View(comTradeTerms);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(comTradeTerms);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Trade Terms.";
                TempData["type"] = "error";
                return View(comTradeTerms);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditTradeTerms(string tId)
        {
            try
            {
                var result = await _comTradeTerms.FindByIdAsync(int.Parse(_protector.Unprotect(tId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.TTID.ToString());
                    ModelState.Clear();
                    return View(result);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Trade Terms.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Trade Terms.";
                TempData["type"] = "error";
                return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditTradeTerms(COM_TRADE_TERMS comTradeTerms)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tenor = await _comTradeTerms.FindByIdAsync(int.Parse(_protector.Unprotect(comTradeTerms.EncryptedId)));

                    if (tenor != null)
                    {
                        var result = await _comTradeTerms.Update(comTradeTerms);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Trade Terms.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetComTradeTerms", $"ComTradeTerms");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Trade Terms.";
                            TempData["type"] = "error";
                            return View(comTradeTerms);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Update Trade Terms.";
                        TempData["type"] = "error";
                        return View(comTradeTerms);
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input, Please Try Again.";
                    TempData["type"] = "error";
                    return View(comTradeTerms);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Trade Terms.";
                TempData["type"] = "error";
                return View(comTradeTerms);
            }
        }
    }
}