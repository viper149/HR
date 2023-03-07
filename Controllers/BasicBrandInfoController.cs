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
    public class BasicBrandInfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_BRANDINFO _bAsBrandinfo;

        public BasicBrandInfoController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              IBAS_BRANDINFO bAS_BRANDINFO)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsBrandinfo = bAS_BRANDINFO;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsBrandNameInUse(string brandName)
        {
            var isBrandNameExists = _bAsBrandinfo.FindByBrandName(brandName);
            return isBrandNameExists ? Json(true) : Json($"Brand Name [ {brandName} ] is already in use");
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

                var data = await _bAsBrandinfo.GetAll();
                
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.BRANDNAME.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.BRANDID.ToString());
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
        public IActionResult GetBasBrandInfoWithPaged()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult CreateBasBrandInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasBrandInfo(BAS_BRANDINFO brandInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bAsBrandinfo.InsertByAsync(brandInfo);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Brand Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
                    }

                    TempData["message"] = "Failed to Add Brand Information.";
                    TempData["type"] = "error";
                    return View(brandInfo);
                }

                TempData["message"] = "Please Enter Valid Brand Information.";
                TempData["type"] = "error";
                return View(brandInfo);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Brand Information.";
                TempData["type"] = "error";
                return View(brandInfo);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBasBrandInfo(string brandInfoId)
        {
            try
            {
                var result = await _bAsBrandinfo.DeleteInfo(int.Parse(_protector.Unprotect(brandInfoId)));

                if (result)
                {
                    TempData["message"] = "Successfully Deleted Brand Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
                }

                TempData["message"] = "Failed to Delete Brand Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Brand Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasBrandInfo(string brandInfoId)
        {
            try
            {
                var result = await _bAsBrandinfo.FindByIdAsync(int.Parse(_protector.Unprotect(brandInfoId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.BRANDID.ToString());
                    return View(result);
                }

                TempData["message"] = "Failed to Retrieve Brand Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Brand Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBasBrandInfo(BAS_BRANDINFO brandInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var buyerInfoPrevious = await _bAsBrandinfo.FindByIdAsync(int.Parse(_protector.Unprotect(brandInfo.EncryptedId)));

                    if (buyerInfoPrevious != null)
                    {
                        var result = await _bAsBrandinfo.Update(brandInfo);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Brand Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasBrandInfoWithPaged", $"BasicBrandInfo");
                        }

                        TempData["message"] = "Failed to Update Brand Information.";
                        TempData["type"] = "error";
                        return View(brandInfo);
                    }

                    TempData["message"] = "Brand Information Not Found.";
                    TempData["type"] = "error";
                    return View(brandInfo);
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(brandInfo);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Brand Information.";
                TempData["type"] = "error";
                return View(brandInfo);
            }
        }
    }
}