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
    public class BasicSupplierCategoryController : Controller
    {
        private readonly IDataProtector protector;
        private readonly IBAS_SUPP_CATEGORY bAS_SUPP_CATEGORY;

        public BasicSupplierCategoryController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings, IBAS_SUPP_CATEGORY bAS_SUPP_CATEGORY)
        {
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            this.bAS_SUPP_CATEGORY = bAS_SUPP_CATEGORY;
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult IsSupplierCategoryInUse(string catName)
        {
            var category = bAS_SUPP_CATEGORY.FindBySupplierCategoryName(catName);

            return category ? Json(true) : Json($"Supplier Category [ {catName} ] is already in use");
        }


        // BASIC SUPPLIER CATEGORY STARTED HERE


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

                var data = await bAS_SUPP_CATEGORY.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = protector.Protect(item.SCATID.ToString());
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
                    data = data.Where(m => m.CATNAME.ToUpper().Contains(searchValue)
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
        public IActionResult GetBasSupplierCategoriesWithPaged()
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
        public IActionResult CreateBasSupplierCategory()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasSupplierCategory(BAS_SUPP_CATEGORY category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await bAS_SUPP_CATEGORY.InsertByAsync(category);

                    if (result == true)
                    {
                        TempData["message"] = "Successfully Added Supplier Category.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Supplier Category.";
                        TempData["type"] = "error";
                        //ViewBag.ErrorMessage = "Failed to insert category";
                        return View(category);
                    }
                }
                return View(category);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Supplier Category.";
                TempData["type"] = "error";
                //ViewBag.ErrorMessage = "Failed to insert category";
                return View(category);
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteBasSupplierCategory(string supplierCategoryId)
        {
            try
            {
                var decryptedId = protector.Unprotect(supplierCategoryId);
                var decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await bAS_SUPP_CATEGORY.DeleteCategory(decryptedIntId);
                if (result)
                {
                    TempData["message"] = "Successfully Deleted Supplier Category.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
                }
                else
                {
                    TempData["message"] = "Failed to Delete Supplier Category.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
                }
            }
            catch (Exception)
            {
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategoryId} ], not found!";

                TempData["message"] = "Failed to Delete Supplier Category.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsBasSupplierCategory(string supplierCategoryId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(supplierCategoryId);
                decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await bAS_SUPP_CATEGORY.FindByIdAsync(decryptedIntId);

                if (result != null)
                {
                    result.EncryptedId = protector.Protect(result.SCATID.ToString());
                    return View(result);
                }
                return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
            }
            catch (Exception)
            {
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategoryId} ], not found!";
                //return View("NotFound");
                TempData["message"] = "Failed to Retrieve Supplier Category Details.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasSupplierCategory(string supplierCategoryId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(supplierCategoryId);
                decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await bAS_SUPP_CATEGORY.FindByIdAsync(decryptedIntId);
                if (result != null)
                {
                    result.EncryptedId = protector.Protect(result.SCATID.ToString());
                    return View(result);
                }
                else
                {
                    //ViewBag.ErrorTitle = "Not found";
                    //ViewBag.ErrorMessage = $"Id [ {supplierCategoryId} not found. ]";
                    TempData["message"] = "Failed to Retrieve Supplier Category.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
                }
            }
            catch (Exception)
            {
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategoryId} ], not found!";

                TempData["message"] = "Failed to Retrieve Supplier Category.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
            }

        }
               
        [HttpPost]
        public async Task<IActionResult> EditBasSupplierCategory(BAS_SUPP_CATEGORY supplierCategory)
        {
            int decryptedIntId = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    string decryptedId = protector.Unprotect(supplierCategory.EncryptedId);
                    decryptedIntId = Convert.ToInt32(decryptedId);

                    var category = await bAS_SUPP_CATEGORY.FindByIdAsync(decryptedIntId);
                    if (category != null)
                    {
                        category.CATNAME = supplierCategory.CATNAME;
                        category.REMARKS = supplierCategory.REMARKS;

                        var result = await bAS_SUPP_CATEGORY.Update(category);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Supplier Category.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasSupplierCategoriesWithPaged", "BasicSupplierCategory");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Supplier Category.";
                            TempData["type"] = "error";
                            return View(supplierCategory);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Update Supplier Category.";
                        TempData["type"] = "error";
                        return View(supplierCategory);
                    }
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(supplierCategory);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Supplier Category.";
                TempData["type"] = "error";
                //ViewBag.ErrorMessage = $"Invalid input [ {supplierCategory.EncryptedId} ], not found!";
                return View(supplierCategory);
            }
        }

        // BASIC SUPPLIER CATEGORY END HERE

    }
}