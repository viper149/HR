using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class BasicProductController : Controller
    {
        private readonly IBAS_PRODUCTINFO _basProductInfo;
        private readonly IDataProtector _protector;

        public BasicProductController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IBAS_PRODUCTINFO basProductInfo)
        {
            _basProductInfo = basProductInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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
                var data = await _basProductInfo.GetProductInfoListAllAsync();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.PRODID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {

                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                data = data.OrderBy(c =>
                                    c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])
                                        ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c))
                                .ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType()
                                            .GetProperty(subStrings[1])
                                            ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }

                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.PRODNAME.ToUpper().Contains(searchValue)
                                           || (m.CAT.CATEGORY != null && m.CAT.CATEGORY.ToUpper().Contains(searchValue))
                                           || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
                                           || (m.UNIT != null && m.UNITNavigation.UNAME.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var totalRecords = data.Count();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = totalRecords,
                    recordsTotal = totalRecords,
                    data = data.Skip(skip).Take(pageSize)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                var productInfo = await _basProductInfo.GetInfo(new BasProductInfoViewModel());
                return View(productInfo);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(BasProductInfoViewModel basProductInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (basProductInfoViewModel.BAS_PRODUCTINFO != null)
                    {
                        var result = await _basProductInfo.InsertByAsync(basProductInfoViewModel.BAS_PRODUCTINFO);
                        if (result)
                        {
                            TempData["message"] = "Successfully Added Product.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetProducts", $"BasicProduct");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Add Product.";
                            TempData["type"] = "error";
                            return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Product.";
                        TempData["type"] = "error";
                        return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
                    }
                }
                TempData["message"] = "Invalid Input.";
                TempData["type"] = "error";
                return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["message"] = "Failed to Add Product.";
                TempData["type"] = "error";
                return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var product = await _basProductInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (product != null)
                {
                    var result = await _basProductInfo.Delete(product);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Product.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetProducts", $"BasicProduct");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Delete Product.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetProducts", $"BasicProduct");
                    }
                }
                else
                {
                    TempData["message"] = "Product Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetProducts", $"BasicProduct");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Product.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProducts", $"BasicProduct");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(string id)
        {
            try
            {
                var basProductinfo = await _basProductInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (basProductinfo != null)
                {
                    basProductinfo.EncryptedId = _protector.Protect(basProductinfo.PRODID.ToString());

                    return View(await _basProductInfo.GetInfo(new BasProductInfoViewModel
                    {
                        BAS_PRODUCTINFO = basProductinfo
                    }));
                }
                TempData["message"] = "Failed to Retrieve Product.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProducts", $"BasicProduct");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Product.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProducts", $"BasicProduct");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(BasProductInfoViewModel basProductInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var color = await _basProductInfo.FindByIdAsync(int.Parse(_protector.Unprotect(basProductInfoViewModel.BAS_PRODUCTINFO.EncryptedId)));

                    if (color != null)
                    {
                        var result = await _basProductInfo.Update(basProductInfoViewModel.BAS_PRODUCTINFO);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Product.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetProducts", $"BasicProduct");
                        }
                        TempData["message"] = "Failed to Update Product.";
                        TempData["type"] = "error";
                        return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
                    }
                    TempData["message"] = "Failed to Update Product.";
                    TempData["type"] = "error";
                    return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(await _basProductInfo.GetInfo(basProductInfoViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Error Occurred During Your Submission.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProducts", $"BasicProduct");
            }
        }
    }
}