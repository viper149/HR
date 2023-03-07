using System;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Basic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class BasicController : Controller
    {
        private readonly IBAS_COLOR _bAS_COLOR;
        private readonly IBAS_PRODCATEGORY bAS_PRODCATEGORY;
        private readonly IBAS_PRODUCTINFO bAS_PRODUCTINFO;
        private readonly IDataProtector protector;

        public BasicController(IBAS_COLOR bAS_COLOR, IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings, IBAS_PRODCATEGORY bAS_PRODCATEGORY, IBAS_PRODUCTINFO bAS_PRODUCTINFO)
        {
            _bAS_COLOR = bAS_COLOR;
            this.bAS_PRODCATEGORY = bAS_PRODCATEGORY;
            this.bAS_PRODUCTINFO = bAS_PRODUCTINFO;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> EditProductInfo(string productId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(productId);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {productId} ], not found!";
                return View("NotFound");
            }

            var result = await bAS_PRODUCTINFO.FindProductInfoByAsync(decryptedIntId);

            if (result != null)
            {
                result.EncryptedId = protector.Protect(result.PRODID.ToString());

                BasProductInfoViewModel finalResult = new BasProductInfoViewModel()
                {
                    BAS_PRODUCTINFO = result,
                    BAS_PRODCATEGORies = await bAS_PRODCATEGORY.GetAll()
                };

                return View(finalResult);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProductInfo(BasProductInfoViewModel productInfo)
        {
            if (ModelState.IsValid)
            {
                BAS_PRODUCTINFO product = new BAS_PRODUCTINFO
                {
                    PRODID = productInfo.BAS_PRODUCTINFO.PRODID,
                    EncryptedId = productInfo.BAS_PRODUCTINFO.EncryptedId,
                    PRODNAME = productInfo.BAS_PRODUCTINFO.PRODNAME,
                    DESCRIPTION = productInfo.BAS_PRODUCTINFO.DESCRIPTION,
                    CATID = productInfo.BAS_PRODUCTINFO.CATID,
                    UNIT = productInfo.BAS_PRODUCTINFO.UNIT,
                    REMARKS = productInfo.BAS_PRODUCTINFO.REMARKS
                };

                var result = await bAS_PRODUCTINFO.Update(product);
                if (result == true)
                {
                    return RedirectToAction("GetBasProducts", "Basic");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                productInfo.BAS_PRODCATEGORies = await bAS_PRODCATEGORY.GetAll();
                return View(productInfo);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProductInfo(string productId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(productId);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {productId} ], not found!";
                return View("NotFound");
            }

            var product = await bAS_PRODUCTINFO.FindByIdAsync(decryptedIntId);
            if (product != null)
            {
                await bAS_PRODUCTINFO.Delete(product);
                return RedirectToAction("GetBasProducts", "Basic");
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> FindProductInfo(string productId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(productId);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {productId} ], not found!";
                return View("NotFound");
            }

            var result = await bAS_PRODUCTINFO.FindProductInfoByAsync(decryptedIntId);

            if (result != null)
            {
                result.EncryptedId = protector.Protect(result.PRODID.ToString());
                return View(result);
            }
            else
            {
                ViewBag.ErrorMessage = $"Inavalid input [ {productId} ]. Please find the correct id to get the result";
                return View("NotFound");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBasProducts(int pi = 1, int ps = 20)
        {
            var products = await bAS_PRODUCTINFO.GetProductInfoListAllAsync();
            var totalItems = await bAS_PRODUCTINFO.TotalNumberOfProducts();

            var finalResult = new PagedResult<BAS_PRODUCTINFO>
            {
                Data = products.ToList(),
                TotalItems = totalItems,
                PageNumber = pi,
                PageSize = ps
            };

            finalResult.Data.Select(e => { e.EncryptedId = protector.Protect(e.PRODID.ToString()); return e; }).ToList();

            ViewData["controller"] = "Basic";
            ViewData["action"] = "GetBasProducts";

            return View(finalResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasProductInfo(BasProductInfoViewModel productInfo)
        {
            if (ModelState.IsValid)
            {
                if (productInfo.BAS_PRODUCTINFO.PRODNAME != null &&
                    productInfo.BAS_PRODUCTINFO.CATID > 0 &&
                    productInfo.BAS_PRODUCTINFO.UNIT != null)
                {
                    BAS_PRODUCTINFO product = new BAS_PRODUCTINFO()
                    {
                        PRODNAME = productInfo.BAS_PRODUCTINFO.PRODNAME,
                        DESCRIPTION = productInfo.BAS_PRODUCTINFO.DESCRIPTION,
                        CATID = productInfo.BAS_PRODUCTINFO.CATID,
                        UNIT = productInfo.BAS_PRODUCTINFO.UNIT,
                        REMARKS = productInfo.BAS_PRODUCTINFO.REMARKS
                    };

                    var result = await bAS_PRODUCTINFO.InsertByAsync(product);
                    if (result == true)
                    {
                        return RedirectToAction("GetBasProducts", "Basic");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"";
                        return View("Error");
                    }
                }
                else
                {
                    var categories = await bAS_PRODCATEGORY.GetAll();
                    productInfo.BAS_PRODCATEGORies = categories;

                    return View(productInfo);
                }
            }
            else
            {

                var categories = await bAS_PRODCATEGORY.GetAll();
                productInfo.BAS_PRODCATEGORies = categories;

                return View(productInfo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBasProductCategory(BAS_PRODCATEGORY productCategory)
        {
            if (ModelState.IsValid)
            {
                int decryptedIntId = 0;
                try
                {
                    string decryptedId = protector.Unprotect(productCategory.EncryptedId);
                    decryptedIntId = Convert.ToInt32(decryptedId);
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = $"Invalid input [ {productCategory.EncryptedId} ], not found!";
                    return View("NotFound");
                }

                var category = await bAS_PRODCATEGORY.FindByIdAsync(decryptedIntId);
                if (category != null)
                {
                    category.CATEGORY = productCategory.CATEGORY;
                    category.REMARKS = productCategory.REMARKS;

                    var result = await bAS_PRODCATEGORY.Update(category);
                    if (result == true)
                    {
                        return RedirectToAction("GetBasProductCategories", "Basic");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            return View(productCategory);
        }

        [HttpGet]
        public async Task<IActionResult> EditBasProductCategory(string productCategoryId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(productCategoryId);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {productCategoryId} ], not found!";
                return View("NotFound");
            }

            var result = await bAS_PRODCATEGORY.FindByIdAsync(decryptedIntId);
            if (result != null)
            {
                result.EncryptedId = protector.Protect(result.CATID.ToString());
                return View(result);
            }
            else
            {
                ViewBag.ErrorTitle = "Not found";
                ViewBag.ErrorMessage = $"Id [ {productCategoryId} not found. ]";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateBasProductInfo()
        {
            var categories = await bAS_PRODCATEGORY.GetAll();
            BasProductInfoViewModel productInfo = new BasProductInfoViewModel()
            {
                BAS_PRODCATEGORies = categories
            };

            return View(productInfo);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsBasProductCategory(string productCategoryId)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(productCategoryId);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {productCategoryId} ], not found!";
                return View("NotFound");
            }

            var result = await bAS_PRODCATEGORY.FindByIdAsync(decryptedIntId);

            if (result != null)
            {
                result.EncryptedId = protector.Protect(result.CATID.ToString());
                return View(result);
            }
            return RedirectToAction("GetBasProductCategories", "Basic");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBasProductCategory(string productCategoryId)
         {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(productCategoryId);
                decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await bAS_PRODCATEGORY.DeleteCategory(decryptedIntId);
                if (result == true)
                {
                    TempData["message"] = "Successfully Deleted Product Category.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasProductCategories", "Basic");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {productCategoryId} ], not found!";
                return View("NotFound");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsProductCategoryInUse(string CATEGORY)
        {
            var category = bAS_PRODCATEGORY.FindByProductCategoryName(CATEGORY);

            if (category == true)
            {
                return Json(true);
            }
            else
            {
                return Json($"Product category [ {CATEGORY} ] is already in use");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBasProductCategories(int pi = 1, int ps = 20)
        {
            var productCategories = await bAS_PRODCATEGORY.GetProductCategoryInfoWithPaged(pi, ps);
            var totalItems = await bAS_PRODCATEGORY.TotalNumberOfProductCategory();

            var finalResult = new PagedResult<BAS_PRODCATEGORY>
            {
                Data = productCategories.Select(e => { e.EncryptedId = protector.Protect(e.CATID.ToString()); return e; }).ToList(),
                TotalItems = totalItems,
                PageNumber = pi,
                PageSize = ps
            };

            ViewData["controller"] = "Basic";
            ViewData["action"] = "GetBasProductCategories";

            return View(finalResult);
        }

        [HttpGet]
        public IActionResult CreateBasProductCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasProductCategory(BAS_PRODCATEGORY product)
        {
            if (ModelState.IsValid)
            {
                var result = await bAS_PRODCATEGORY.InsertByAsync(product);

                if (result == true)
                {
                    TempData["message"] = "Successfully Added Product Category.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetBasProductCategories", "Basic");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to insert category";
                    return View("Error");
                }
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> BasicColors(int pi = 1, int ps = 20)
        {
            var colors = await _bAS_COLOR.GetColorsWithPaged(pi, ps);
            var totalItems = await _bAS_COLOR.GetAll();

            var finalResult = new PagedResult<BAS_COLOR>
            {
                Data = colors.Select(e => { e.EncryptedId = protector.Protect(e.COLORCODE.ToString()); return e; }).ToList(),
                TotalItems = totalItems.Count(),
                PageNumber = pi,
                PageSize = ps
            };

            ViewData["controller"] = "Basic";
            ViewData["action"] = "BasicColors";

            return View(finalResult);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ColorViewModel colorModel)
        {
            if (!ModelState.IsValid) return View(colorModel);

            if (colorModel != null)
            {
                var color = new BAS_COLOR()
                {
                    COLOR = colorModel.COLOR,
                    REMARKS = colorModel.REMARKS
                };

                var result = await _bAS_COLOR.InsertByAsync(color);

                if (result == true)
                {
                    return RedirectToAction($"BasicColors", $"Basic");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to insert color. Try again later.";
                    return View($"Error");

                }
            }
            else
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(id);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {id} ], not found!";
                return View("NotFound");
            }

            var result = await _bAS_COLOR.FindByIdAsync(decryptedIntId);

            if (result != null)
            {
                result.EncryptedId = protector.Protect(result.COLORCODE.ToString());
                return View(result);
            }
            else
            {
                ViewBag.ErrorTitle = "Not found";
                ViewBag.ErrorMessage = $"Id [ {id} not found. ]";
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(BAS_COLOR bAS_COLOR)
        {
            if (ModelState.IsValid)
            {
                int decryptedIntId = 0;
                try
                {
                    string decryptedId = protector.Unprotect(bAS_COLOR.EncryptedId);
                    decryptedIntId = Convert.ToInt32(decryptedId);

                    bAS_COLOR.COLORCODE = decryptedIntId;
                    var result = await _bAS_COLOR.Update(bAS_COLOR);

                    if (result == true)
                    {
                        return RedirectToAction("BasicColors", "Basic");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = $"Invalid color code [ {bAS_COLOR.EncryptedId} ], not found!";
                    return View("NotFound");
                }
            }
            return View(bAS_COLOR);
        }

        [HttpGet]

        public async Task<IActionResult> Find(string id)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(id);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {id} ], not found!";
                return View("NotFound");
            }

            var result = await _bAS_COLOR.FindByIdAsync(decryptedIntId);
            if (result != null)
            {
                result.EncryptedId = protector.Protect(result.COLORCODE.ToString());
                return View(result);
            }
            return View("Error");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            int decryptedIntId = 0;
            try
            {
                string decryptedId = protector.Unprotect(id);
                decryptedIntId = Convert.ToInt32(decryptedId);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {id} ], not found!";
                return View("NotFound");
            }

            var color = await _bAS_COLOR.FindByIdAsync(decryptedIntId);

            if (color != null)
            {
                await _bAS_COLOR.Delete(color);
                return RedirectToAction("BasicColors", "Basic");
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult CreateDemo()
        {
           return View();
        }
    }
}