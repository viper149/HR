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
    public class BasicYarnCategoryController : Controller
    {
        private readonly IBAS_YARN_CATEGORY _basYarnCategory;
        private readonly IDataProtector protector;

        public BasicYarnCategoryController(IDataProtectionProvider dataProtectionProvider,
        DataProtectionPurposeStrings dataProtectionPurposeStrings,
        IBAS_YARN_CATEGORY basYarnCategory)
        {
            _basYarnCategory = basYarnCategory;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> FindByYarnCategoryName(BAS_YARN_CATEGORY cat)
        {
            var category =await _basYarnCategory.FindByYarnCategoryName(cat.CATEGORY_NAME);

            return !category ? Json(true) : Json($"Yarn Category [ {cat.CATEGORY_NAME} ] is already in use");
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> FindByYarnCode(BAS_YARN_CATEGORY cat)
        {
            if (cat.YARN_CODE == null)
            {
                return Json($"You Must Enter Yarn Code");
            }
            var category =cat.YARN_CODE != null && await _basYarnCategory.FindByYarnCode(cat.YARN_CODE);

            return !category ? Json(true) : Json($"Yarn Code [ {cat.YARN_CODE} ] is already in use");
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

                var data = await _basYarnCategory.GetAll();


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
                    data = data.Where(m => m.CATEGORY_NAME.ToUpper().Contains(searchValue)
                                           || (m.YARN_CODE != 0 && m.YARN_CODE.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                data = data.ToList();

                foreach (var item in data)
                {
                    item.EncryptedId = protector.Protect(item.YARN_CAT_ID.ToString());
                }
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
        public IActionResult GetBasYarnCategoryList()
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
        public IActionResult CreateBasYarnCategory()
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
        public async Task<IActionResult> CreateBasYarnCategory(BAS_YARN_CATEGORY category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _basYarnCategory.InsertByAsync(category);

                    if (result == true)
                    {
                        TempData["message"] = "Successfully Added Yarn Category.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Yarn Category.";
                        TempData["type"] = "error";
                        return View(category);
                    }
                }
                return View(category);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Yarn Category.";
                TempData["type"] = "error";
                return View(category);
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteBasYarnCategory(string id)
        {
            try
            {
                var decryptedId = protector.Unprotect(id);
                var decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await _basYarnCategory.DeleteCategory(decryptedIntId);
                if (result)
                {
                    TempData["message"] = "Successfully Deleted Yarn Category.";
                    TempData["type"] = "success";
                    return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
                }
                TempData["message"] = "Failed to Delete Yarn Category.";
                TempData["type"] = "error";
                return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Yarn Category.";
                TempData["type"] = "error";
                return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditBasYarnCategory(string id)
        {
            try
            {
                var decryptedId = protector.Unprotect(id);
                var decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await _basYarnCategory.FindByIdAsync(decryptedIntId);
                if (result != null)
                {
                    result.EncryptedId = protector.Protect(result.YARN_CAT_ID.ToString());
                    return View(result);
                }
                TempData["message"] = "Failed to Retrieve Yarn Category.";
                TempData["type"] = "error";
                return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Yarn Category.";
                TempData["type"] = "error";
                return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditBasYarnCategory(BAS_YARN_CATEGORY yarnCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var decryptedId = protector.Unprotect(yarnCategory.EncryptedId);
                    var decryptedIntId = Convert.ToInt32(decryptedId);

                    var category = await _basYarnCategory.FindByIdAsync(decryptedIntId);
                    if (category != null)
                    {
                        var result = await _basYarnCategory.Update(yarnCategory);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Yarn Category.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetBasYarnCategoryList", $"BasicYarnCategory");
                        }
                        TempData["message"] = "Failed to Update Yarn Category.";
                        TempData["type"] = "error";
                        return View(yarnCategory);
                    }
                    TempData["message"] = "Failed to Update Yarn Category.";
                    TempData["type"] = "error";
                    return View(yarnCategory);
                }
                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(yarnCategory);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Yarn Category.";
                TempData["type"] = "error";
                return View(yarnCategory);
            }
        }

    }
}