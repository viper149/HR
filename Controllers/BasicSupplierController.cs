using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class BasicSupplierController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_SUPPLIERINFO _bAsSupplierinfo;
        private readonly IBAS_SUPP_CATEGORY _bAsSuppCategory;

        public BasicSupplierController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IBAS_SUPPLIERINFO bAS_SUPPLIERINFO,
            IBAS_SUPP_CATEGORY bAS_SUPP_CATEGORY)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsSupplierinfo = bAS_SUPPLIERINFO;
            _bAsSuppCategory = bAS_SUPP_CATEGORY;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsSupplierInfoInUse(BasSupplierInfoViewModel info)
        {
            var category = _bAsSupplierinfo.FindBySupplierInfoName(info.BAS_SUPPLIERINFO.SUPPNAME);
            return category ? Json(true) : Json($"Supplier Name [ {info.BAS_SUPPLIERINFO.SUPPNAME} ] is already in use");
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
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = await _bAsSupplierinfo.GetBasSupplierInfoAllAsync();
                
                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.SUPPID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SUPPNAME.ToUpper().Contains(searchValue)
                                           || (m.SCAT.CATNAME != null && m.SCAT.CATNAME.ToUpper().Contains(searchValue))
                                           || (m.PHONE != null && m.PHONE.ToUpper().Contains(searchValue))
                                           || (m.EMAIL != null && m.EMAIL.ToUpper().Contains(searchValue))
                                           || (m.ADDRESS != null && m.ADDRESS.ToUpper().Contains(searchValue))
                                           || (m.CPERSON != null && m.CPERSON.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
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
        public IActionResult GetBasSupplierInfoWithPaged()
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
        public async Task<IActionResult> CreateBasSupplierInfo()
        {
            return View(new BasSupplierInfoViewModel()
            {
                bAS_SUPP_CATEGORies = await _bAsSuppCategory.GetAll()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasSupplierInfo(BasSupplierInfoViewModel sUPPLIERINFO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var info = new BAS_SUPPLIERINFO()
                    {
                        SUPPNAME = sUPPLIERINFO.BAS_SUPPLIERINFO.SUPPNAME,
                        SCATID = sUPPLIERINFO.BAS_SUPPLIERINFO.SCATID,
                        ADDRESS = sUPPLIERINFO.BAS_SUPPLIERINFO.ADDRESS,
                        PHONE = sUPPLIERINFO.BAS_SUPPLIERINFO.PHONE,
                        EMAIL = sUPPLIERINFO.BAS_SUPPLIERINFO.EMAIL,
                        CPERSON = sUPPLIERINFO.BAS_SUPPLIERINFO.CPERSON,
                        REMARKS = sUPPLIERINFO.BAS_SUPPLIERINFO.REMARKS
                    };

                    if (await _bAsSupplierinfo.InsertByAsync(info))
                    {
                        TempData["message"] = "Successfully Added Supplier Info.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Supplier Info.";
                        TempData["type"] = "error";
                        return View(sUPPLIERINFO);
                    }
                }
                return View(sUPPLIERINFO);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Supplier Info.";
                TempData["type"] = "error";
                return View(sUPPLIERINFO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBasSupplierInfo(string supplierInfoId)
        {
            try
            {
                var result = await _bAsSupplierinfo.DeleteInfo(int.Parse(_protector.Unprotect(supplierInfoId)));

                if (result)
                {
                    TempData["message"] = "Successfully Deleted Supplier Info.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
                }
                else
                {
                    TempData["message"] = "Failed to Delete Supplier Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Supplier Info.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsBasSupplierInfo(string supplierInfoId)
        {
            try
            {
                var result = await _bAsSupplierinfo.FindByIdAsync(int.Parse(_protector.Unprotect(supplierInfoId)));

                if (result == null) return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
                result.EncryptedId = _protector.Protect(result.SUPPID.ToString());
                return View(result);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {supplierInfoId} ], not found!";
                return View($"NotFound");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasSupplierInfo(string supplierInfoId)
        {
            try
            {
                var result = await _bAsSupplierinfo.FindSupplierInfoByAsync(int.Parse(_protector.Unprotect(supplierInfoId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.SUPPID.ToString());

                    var finalResult = new BasSupplierInfoViewModel()
                    {
                        BAS_SUPPLIERINFO = result,
                        bAS_SUPP_CATEGORies = await _bAsSuppCategory.GetAll()
                    };

                    return View(finalResult);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Supplier Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Supplier Info.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBasSupplierInfo(BasSupplierInfoViewModel supplierInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (await _bAsSupplierinfo.FindByIdAsync(int.Parse(_protector.Unprotect(supplierInfo.BAS_SUPPLIERINFO.EncryptedId))) != null)
                    {
                        var result = await _bAsSupplierinfo.Update(supplierInfo.BAS_SUPPLIERINFO);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Supplier Info.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasSupplierInfoWithPaged", $"BasicSupplier");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Supplier Info.";
                            TempData["type"] = "error";
                            supplierInfo.bAS_SUPP_CATEGORies = await _bAsSuppCategory.GetAll();
                            return View(supplierInfo);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Supplier Info Not Found.";
                        TempData["type"] = "error";
                        supplierInfo.bAS_SUPP_CATEGORies = await _bAsSuppCategory.GetAll();
                        return View(supplierInfo);
                    }
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                supplierInfo.bAS_SUPP_CATEGORies = await _bAsSuppCategory.GetAll();
                return View(supplierInfo);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Supplier Info.";
                TempData["type"] = "error";
                supplierInfo.bAS_SUPP_CATEGORies = await _bAsSuppCategory.GetAll();
                return View(supplierInfo);
            }
        }
    }
}