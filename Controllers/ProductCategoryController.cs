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
    public class ProductCategoryController : Controller
    {
        private readonly IBAS_PRODCATEGORY _basProdCategory;
        private readonly IDataProtector protector;

        public ProductCategoryController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IBAS_PRODCATEGORY basProdCategory)
        {
            _basProdCategory = basProdCategory;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult IsSupplierCategoryInUse(string PRODNAME)
        {
            var category = _basProdCategory.FindByProductCategoryName(PRODNAME);

            if (category)
            {
                return Json(true);
            }

            return Json($"Product Category [ {PRODNAME} ] is already in use");
        }


        // BASIC Product CATEGORY STARTED HERE


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

                var data = await _basProdCategory.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = protector.Protect(item.CATID.ToString());
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
                    data = data.Where(m => m.CATEGORY.ToUpper().Contains(searchValue)
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                data = data.ToList();
                recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                var jsonData = new {draw, recordsFiltered = recordsTotal, recordsTotal, data = finalData };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        [HttpGet]
        public IActionResult GetBasProductCategories()
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
        public IActionResult CreateBasProductCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasProductCategory(BAS_PRODCATEGORY prodCategory)
        {
            try
            {
                if (!ModelState.IsValid) return View(prodCategory);
                var result = await _basProdCategory.InsertByAsync(prodCategory);

                if (result)
                {
                    TempData["message"] = "Successfully Added Product Category.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasProductCategories", "ProductCategory");
                }
                TempData["message"] = "Failed to Add Product Category.";
                TempData["type"] = "error";
                return View(prodCategory);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Product Category.";
                TempData["type"] = "error";
                return View(prodCategory);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteBasProductCategory(string productCategoryId)
        {
            try
            {
                var result = await _basProdCategory.DeleteCategory(int.Parse(protector.Unprotect(productCategoryId)));
                if (result)
                {
                    TempData["message"] = "Successfully Deleted Product Category.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasProductCategories", "ProductCategory");
                }
                TempData["message"] = "Failed to Delete Product Category.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasProductCategories", "ProductCategory");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Product Category.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasProductCategories", "ProductCategory");
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditBasProductCategory(string productCategoryId)
        {
            try
            {
                var result = await _basProdCategory.FindByIdAsync(int.Parse(protector.Unprotect(productCategoryId)));
                if (result != null)
                {
                    result.EncryptedId = protector.Protect(result.CATID.ToString());
                    return View(result);
                }
                TempData["message"] = "Failed to Retrieve Product Category.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasProductCategories", "ProductCategory");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Product Category.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasProductCategories", "ProductCategory");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditBasProductCategory(BAS_PRODCATEGORY productCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string decryptedId = protector.Unprotect(productCategory.EncryptedId);
                    var decryptedIntId = Convert.ToInt32(decryptedId);

                    var category = await _basProdCategory.FindByIdAsync(decryptedIntId);
                    if (category != null)
                    {
                        category.CATEGORY = productCategory.CATEGORY;
                        category.REMARKS = productCategory.REMARKS;

                        var result = await _basProdCategory.Update(category);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Product Category.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasProductCategories", "ProductCategory");
                        }
                        TempData["message"] = "Failed to Update Product Category.";
                        TempData["type"] = "error";
                        return View(productCategory);
                    }
                    TempData["message"] = "Failed to Update Product Category.";
                    TempData["type"] = "error";
                    return View(productCategory);
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(productCategory);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Product Category.";
                TempData["type"] = "error";
                return View(productCategory);
            }
        }

        // BASIC Product CATEGORY END HERE
    }
}