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
    public class BasicBuyerInfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_BUYERINFO _bAsBuyerinfo;

        public BasicBuyerInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IBAS_BUYERINFO bAS_BUYERINFO)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsBuyerinfo = bAS_BUYERINFO;
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult IsBuyerNameInUse(string buyerName)
        {
            var isBuyerNameExists = _bAsBuyerinfo.FindByBuyerName(buyerName);

            if (isBuyerNameExists == true)
            {
                return Json(true);
            }
            else
            {
                return Json($"Buyer Name [ {buyerName} ] is already in use");
            }
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

                var data = await _bAsBuyerinfo.GetAllByAsync(orderBy: _bAsBuyerinfo.GetOrderBy(sortColumn, sortColumnDirection));

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.BUYER_NAME != null && m.BUYER_NAME.ToUpper().Contains(searchValue)
                                           || m.ADDRESS != null && m.ADDRESS.ToUpper().Contains(searchValue)
                                           || m.DEL_ADDRESS != null && m.DEL_ADDRESS.ToUpper().Contains(searchValue)
                                           || m.BIN_NO != null && m.BIN_NO.ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                data = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.BUYERID.ToString());
                }

                return Json(new
                {
                    draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetBasBuyerInfoWithPaged()
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
        public IActionResult CreateBasBuyerInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasBuyerInfo(BAS_BUYERINFO buyerInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bAsBuyerinfo.InsertByAsync(buyerInfo);

                    if (result == true)
                    {
                        TempData["message"] = "Successfully Added Buyer Info.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Buyer Info.";
                        TempData["type"] = "error";
                        //ViewBag.ErrorMessage = "Failed to Add Supplier Info";
                        return View(buyerInfo);
                    }
                }
                TempData["message"] = "Please Enter Valid Buyer Info.";
                TempData["type"] = "error";
                return View(buyerInfo);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Buyer Info.";
                TempData["type"] = "error";
                //ViewBag.ErrorMessage = "Failed to Add Supplier Info";
                return View(buyerInfo);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBasBuyerInfo(string buyerInfoId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = _protector.Unprotect(buyerInfoId);
                decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await _bAsBuyerinfo.DeleteInfo(decryptedIntId);
                if (result == true)
                {
                    TempData["message"] = "Successfully Deleted Buyer Info.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
                }
                else
                {
                    TempData["message"] = "Failed to Delete Buyer Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
                }
            }
            catch (Exception)
            {
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategoryId} ], not found!";
                TempData["message"] = "Failed to Delete Buyer Info.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasBuyerInfo(string buyerInfoId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = _protector.Unprotect(buyerInfoId);
                decryptedIntId = Convert.ToInt32(decryptedId);



                var result = await _bAsBuyerinfo.FindByIdAsync(decryptedIntId);

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.BUYERID.ToString());
                    return View(result);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Buyer Info.";
                    TempData["type"] = "error";
                    //return View("Error");
                    return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
                }
            }
            catch (Exception)
            {
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategoryId} ], not found!";

                TempData["message"] = "Failed to Retrieve Buyer Info.";
                TempData["type"] = "error";
                //return View("Error");
                return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditBasBuyerInfo(BAS_BUYERINFO buyerInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string decryptedId = _protector.Unprotect(buyerInfo.EncryptedId);
                    int decryptedIntId = Convert.ToInt32(decryptedId);

                    var buyerInfoPrevious = await _bAsBuyerinfo.FindByIdAsync(decryptedIntId);
                    if (buyerInfoPrevious != null)
                    {
                        var result = await _bAsBuyerinfo.Update(buyerInfo);
                        if (result == true)
                        {
                            TempData["message"] = "Successfully Updated Buyer Info.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasBuyerInfoWithPaged", "BasicBuyerInfo");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Buyer Info.";
                            TempData["type"] = "error";
                            return View(buyerInfo);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Buyer Info Not Found.";
                        TempData["type"] = "error";
                        return View(buyerInfo);
                    }
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(buyerInfo);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Buyer Info.";
                TempData["type"] = "error";
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategory.EncryptedId} ], not found!";
                return View(buyerInfo);
            }
        }
    }
}