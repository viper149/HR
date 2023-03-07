using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.GeneralStore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FGsItemCategoryController : Controller
    {
        private readonly IF_GS_ITEMCATEGORY _fGsItemcategory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Item Category Information";
        public FGsItemCategoryController(IF_GS_ITEMCATEGORY fGsItemcategory,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fGsItemcategory = fGsItemcategory;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFGsItemCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var data = (List<F_GS_ITEMCATEGORY>)await _fGsItemcategory.GetAllFGsItemCategoryAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.CATNAME != null && m.CATNAME.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();
            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        public IActionResult CreateFGsItemCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsItemCategory(FGsItemCategoryViewModel fGsItemCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                fGsItemCategoryViewModel.FGsItemcategory.CREATED_AT = fGsItemCategoryViewModel.FGsItemcategory.UPDATED_AT = DateTime.Now;
                fGsItemCategoryViewModel.FGsItemcategory.CREATED_BY = fGsItemCategoryViewModel.FGsItemcategory.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fGsItemcategory.InsertByAsync(fGsItemCategoryViewModel.FGsItemcategory))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFGsItemCategory), "FGsItemCategory");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(fGsItemCategoryViewModel);
                }
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(fGsItemCategoryViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsItemCategory(string catId)
        {
            var fGsItemcategory = await _fGsItemcategory.FindByIdAsync(int.Parse(_protector.Unprotect(catId)));

            if (fGsItemcategory != null)
            {
                var fGsItemCategoryViewModel = new FGsItemCategoryViewModel();
                fGsItemCategoryViewModel.FGsItemcategory = fGsItemcategory;
                fGsItemCategoryViewModel.FGsItemcategory.EncryptedId = _protector.Protect(fGsItemCategoryViewModel.FGsItemcategory.CATID.ToString());
                return View(fGsItemCategoryViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFGsItemCategory), $"FGsItemCategory");
        }

        [HttpPost]
        public async Task<IActionResult> EditFGsItemCategory(FGsItemCategoryViewModel fGsItemCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetFGsItemCategory), $"FGsItemCategory");
                fGsItemCategoryViewModel.FGsItemcategory.CATID = int.Parse(_protector.Unprotect(fGsItemCategoryViewModel.FGsItemcategory.EncryptedId));
                var fGsItemcategory = await _fGsItemcategory.FindByIdAsync(fGsItemCategoryViewModel.FGsItemcategory.CATID);
                if (fGsItemcategory != null)
                {
                    
                    fGsItemCategoryViewModel.FGsItemcategory.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fGsItemCategoryViewModel.FGsItemcategory.UPDATED_AT = DateTime.Now;
                    fGsItemCategoryViewModel.FGsItemcategory.CREATED_AT = fGsItemcategory.CREATED_AT;
                    fGsItemCategoryViewModel.FGsItemcategory.CREATED_BY = fGsItemcategory.CREATED_BY;

                    if (await _fGsItemcategory.Update(fGsItemCategoryViewModel.FGsItemcategory))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fGsItemCategoryViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsCatNameInUse(FGsItemCategoryViewModel fGsItemCategoryViewModel)
        {
            var catName = fGsItemCategoryViewModel.FGsItemcategory.CATNAME;
            return await _fGsItemcategory.FindByCatName(catName) ? Json(true) : Json($"Category {catName} is already in use");
        }
    }
}
