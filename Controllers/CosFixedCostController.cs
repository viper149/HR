using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class CosFixedCostController : Controller
    {
        private readonly ICOS_FIXEDCOST _cosFixedCost;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataProtector _protector;

        public CosFixedCostController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOS_FIXEDCOST cosFixedCost,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _cosFixedCost = cosFixedCost;
            _signInManager = signInManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult RGetFixedCostList()
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
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var data = (List<COS_FIXEDCOST>)await _cosFixedCost.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.FCID.ToString());
                    item.USERNAME = item.USERID != null ? _signInManager.UserManager.FindByIdAsync(item.USERID).Result.UserName : "";
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.DYEINGCOST.ToString().Contains(searchValue)
                                            || m.UPCHARGE.ToString().Contains(searchValue)
                                            || m.PRINTINGCOST.ToString().Contains(searchValue)
                                            || m.OVERHEADCOST.ToString().Contains(searchValue)
                                            || m.SAMPLECOST.ToString().Contains(searchValue)
                                            || m.SIZINGCOST.ToString().Contains(searchValue)
                                            || m.USERNAME.ToUpper().Contains(searchValue)
                                            || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
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
        public IActionResult GetCosFixedCostWithPaged()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateFixedCost()
        {
            var result = await _cosFixedCost.GetAll();
            var fixedCost = result.Where(c=>c.STATUS.Equals(true)).OrderByDescending(c => c.FCID).FirstOrDefault();
            return View(fixedCost);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFixedCost(COS_FIXEDCOST cosFixedCost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _signInManager.UserManager.GetUserAsync(User);
                    cosFixedCost.USERID = user.Id;
                    cosFixedCost.CREATED_AT = DateTime.Now;
                    cosFixedCost.UPDATED_AT = DateTime.Now;

                    var result = await _cosFixedCost.InsertByAsync(cosFixedCost);

                    if (result)
                    {
                        var data = await _cosFixedCost.GetAll();
                        data = data.OrderByDescending(c=>c.FCID).Skip(1).ToList();
                        foreach (var item in data)
                        {
                            item.STATUS = false;
                            await _cosFixedCost.Update(item);   
                        }
                        TempData["message"] = "Successfully Added Fixed Cost.";
                        TempData["type"] = "success";
                        return RedirectToAction($"CreateFixedCost", $"CosFixedCost");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Fixed Cost.";
                        TempData["type"] = "error";
                        return View(cosFixedCost);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(cosFixedCost);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Fixed Cost.";
                TempData["type"] = "error";
                return View(cosFixedCost);
            }
        }


        [HttpGet]
        public async Task<IActionResult> DetailsFixedCost(string fcId)
        {
            try
            {
                var fixedCost = await _cosFixedCost.FindByIdAsync(int.Parse(_protector.Unprotect(fcId)));
                if (fixedCost != null)
                {
                    fixedCost.USERNAME = fixedCost.USERID != null ? _signInManager.UserManager.FindByIdAsync(fixedCost.USERID).Result.UserName : "";
                    return View(fixedCost);
                }
                else
                {
                    return RedirectToAction($"GetCosFixedCostWithPaged", $"CosFixedCost");
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

    }
}