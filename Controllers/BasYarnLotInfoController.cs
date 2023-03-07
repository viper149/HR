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
    public class BasYarnLotinfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_YARN_LOTINFO _bAsYarnLotinfo;

        public BasYarnLotinfoController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              IBAS_YARN_LOTINFO bAS_YARN_LOTINFO)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsYarnLotinfo = bAS_YARN_LOTINFO;
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

                var data = await _bAsYarnLotinfo.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.LOTID.ToString());
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
                    data = data.Where(m => m.LOTNO.ToUpper().Contains(searchValue)
                                           || m.BRAND != null && m.BRAND.ToUpper().Contains(searchValue)
                                           || m.SLABCODE != null && m.SLABCODE.ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                data = data.ToList();
                recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

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
        [Route("/YarnLot")]
        [Route("/YarnLot/GetAll")]
        public IActionResult GetBasYarnLotinfoWithPaged()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditBasYarnLotinfo(BAS_YARN_LOTINFO yarnLotinfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _bAsYarnLotinfo.Update(yarnLotinfo))
                    {
                        return RedirectToAction(nameof(GetBasYarnLotinfoWithPaged), "BasYarnLotinfo");
                    }
                }
                return View(yarnLotinfo);

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasYarnLotinfo(string basYarnLotinfoId)
        {
            try
            {
                var result = await _bAsYarnLotinfo.FindByIdAsync(int.Parse(_protector.Unprotect(basYarnLotinfoId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.LOTID.ToString());
                    return View(result);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Not found {basYarnLotinfoId}, Please try again.";
                return View("NotFound");
            }
        }
        
        [HttpGet]
        public IActionResult CreateBasYarnLotinfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasYarnLotinfo(BAS_YARN_LOTINFO basYarnLotinfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _bAsYarnLotinfo.InsertByAsync(basYarnLotinfo);

                if (result == true)
                {
                    return RedirectToAction("GetBasYarnLotinfoWithPaged", "BasYarnLotinfo");
                }
                else
                {
                    return View("Error");
                }
            }
            return View(basYarnLotinfo);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteBasYarnLotinfo(string basYarnLotinfoId)
        {
            try
            {
                var result = await _bAsYarnLotinfo.FindByIdAsync(int.Parse(_protector.Unprotect(basYarnLotinfoId)));

                if (result != null)
                {
                    await _bAsYarnLotinfo.Delete(result);

                    TempData["message"] = "Successfully Deleted Basic Yarn Lot Info.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetBasYarnLotinfoWithPaged), "BasYarnLotinfo");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {basYarnLotinfoId} ], not found!";
                return View("NotFound");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsBasYarnLotinfo(string basYarnLotinfoId)
        {
            try
            {
                var result = await _bAsYarnLotinfo.FindByIdAsync(int.Parse(_protector.Unprotect(basYarnLotinfoId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.LOTID.ToString());
                    return View(result);
                }

                return RedirectToAction(nameof(GetBasYarnLotinfoWithPaged), "BasYarnLotinfo");

            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}