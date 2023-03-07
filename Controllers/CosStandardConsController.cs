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
    public class CosStandardConsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICOS_STANDARD_CONS _cosStandardCons;
        private readonly IDataProtector _protector;

        public CosStandardConsController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            SignInManager<ApplicationUser> signInManager,
            ICOS_STANDARD_CONS cosStandardCons
        )
        {
            _signInManager = signInManager;
            _cosStandardCons = cosStandardCons;
            this._protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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
                var data = (List<COS_STANDARD_CONS>)await _cosStandardCons.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.SCID.ToString());
                    item.USERNAME = item.USERID != null ? _signInManager.UserManager.FindByIdAsync(item.USERID).Result.UserName : "";
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.MONTHLY_COST.ToString().Contains(searchValue)
                                           || m.PROD.ToString().Contains(searchValue)
                                           || m.OVERHEAD_BDT.ToString().Contains(searchValue)
                                           || m.CONV_RATE.ToString().Contains(searchValue)
                                           || m.OVERHEAD_USD.ToString().Contains(searchValue)
                                           || m.LOOMSPEED.ToString().Contains(searchValue)
                                           || m.EFFICIENCY.ToString().Contains(searchValue)
                                           || m.GRPPI.ToString().Contains(searchValue)
                                           || m.CONTRACTION.ToString().Contains(searchValue)
                                           || m.USERNAME.ToUpper().Contains(searchValue.ToUpper())
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
        public IActionResult GetCosStandardCons()
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
        public async Task<IActionResult> CreateStandardCost()
        {
            var result = await _cosStandardCons.GetAll();
            var standardCost = result.Where(c => c.STATUS.Equals(true)).OrderByDescending(c => c.SCID).FirstOrDefault();
            return View(standardCost);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStandardCost(COS_STANDARD_CONS cosStandardCons)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _signInManager.UserManager.GetUserAsync(User);
                    cosStandardCons.USERID = user.Id;
                    cosStandardCons.CREATED_AT = DateTime.Now;
                    cosStandardCons.UPDATED_AT = DateTime.Now;
                    cosStandardCons.STATUS = true;

                    var result = await _cosStandardCons.InsertByAsync(cosStandardCons);

                    if (result)
                    {
                        var data = await _cosStandardCons.GetAll();
                        data = data.OrderByDescending(c => c.SCID).Skip(1).ToList();
                        foreach (var item in data)
                        {
                            item.STATUS = false;
                            await _cosStandardCons.Update(item);
                        }
                        TempData["message"] = "Successfully Added Standard Cost.";
                        TempData["type"] = "success";
                        return RedirectToAction($"CreateStandardCost", $"CosStandardCons");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Standard Cost.";
                        TempData["type"] = "error";
                        return View(cosStandardCons);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(cosStandardCons);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Standard Cost.";
                TempData["type"] = "error";
                return View(cosStandardCons);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DetailsStandardCost(string scId)
        {
            try
            {
                var standardCons = await _cosStandardCons.FindByIdAsync(int.Parse(_protector.Unprotect(scId)));
                if (standardCons != null)
                {
                    standardCons.USERNAME = standardCons.USERID != null ? _signInManager.UserManager.FindByIdAsync(standardCons.USERID).Result.UserName : "";
                    return View(standardCons);
                }
                else
                {
                    return RedirectToAction($"GetCosStandardCons", $"CosStandardCons");
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

    }
}