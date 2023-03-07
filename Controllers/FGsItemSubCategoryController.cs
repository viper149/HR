using System;
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
    public class FGsItemSubCategoryController : Controller
    {
        private readonly IF_GS_ITEMSUB_CATEGORY _fGsItemsubCategory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Item Sub-Category Information";

        public FGsItemSubCategoryController(IF_GS_ITEMSUB_CATEGORY fGsItemsubCategory,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fGsItemsubCategory = fGsItemsubCategory;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFGsItemSubCategory()
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
            var data = await _fGsItemsubCategory.GetAllFGsItemSubCategoryAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.SCATNAME != null && m.SCATNAME.ToUpper().Contains(searchValue))
                                       || (m.CAT.CATNAME != null && m.CAT.CATNAME.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                       ).ToList();
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
        public async Task<IActionResult> CreateFGsItemSubCategory()
        {
            return View(await _fGsItemsubCategory.GetInitObjByAsync(new FGsItemSubCategoryViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsItemSubCategory(FGsItemSubCategoryViewModel fGsItemSubCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                fGsItemSubCategoryViewModel.FGsItemsubCategory.CREATED_AT = fGsItemSubCategoryViewModel.FGsItemsubCategory.UPDATED_AT = DateTime.Now;
                fGsItemSubCategoryViewModel.FGsItemsubCategory.CREATED_BY = fGsItemSubCategoryViewModel.FGsItemsubCategory.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fGsItemsubCategory.InsertByAsync(fGsItemSubCategoryViewModel.FGsItemsubCategory))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFGsItemSubCategory), "FGsItemSubCategory");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(fGsItemSubCategoryViewModel);
                }
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _fGsItemsubCategory.GetInitObjByAsync(new FGsItemSubCategoryViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsItemSubCategory(string catId)
        {
            var fGsItemsubCategory = await _fGsItemsubCategory.FindByIdAsync(int.Parse(_protector.Unprotect(catId)));

            if (fGsItemsubCategory != null)
            {
                var fGsItemSubCategoryViewModel = new FGsItemSubCategoryViewModel();
                fGsItemSubCategoryViewModel.FGsItemsubCategory = fGsItemsubCategory;
                fGsItemSubCategoryViewModel.FGsItemsubCategory.EncryptedId = _protector.Protect(fGsItemSubCategoryViewModel.FGsItemsubCategory.SCATID.ToString());
                return View(fGsItemSubCategoryViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFGsItemSubCategory), $"FGsItemSubCategory");
        }

        [HttpPost]
        public async Task<IActionResult> EditFGsItemSubCategory(FGsItemSubCategoryViewModel fGsItemSubCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetFGsItemSubCategory), $"FGsItemSubCategory");
                fGsItemSubCategoryViewModel.FGsItemsubCategory.SCATID = int.Parse(_protector.Unprotect(fGsItemSubCategoryViewModel.FGsItemsubCategory.EncryptedId));
                var fGsItemcategory = await _fGsItemsubCategory.FindByIdAsync(fGsItemSubCategoryViewModel.FGsItemsubCategory.SCATID);
                if (fGsItemcategory != null)
                {

                    fGsItemSubCategoryViewModel.FGsItemsubCategory.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fGsItemSubCategoryViewModel.FGsItemsubCategory.UPDATED_AT = DateTime.Now;
                    fGsItemSubCategoryViewModel.FGsItemsubCategory.CREATED_AT = fGsItemcategory.CREATED_AT;
                    fGsItemSubCategoryViewModel.FGsItemsubCategory.CREATED_BY = fGsItemcategory.CREATED_BY;

                    if (await _fGsItemsubCategory.Update(fGsItemSubCategoryViewModel.FGsItemsubCategory))
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
            return View(fGsItemSubCategoryViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsScatNameInUse(FGsItemSubCategoryViewModel fGsItemSubCategoryViewModel)
        {
            var scatName = fGsItemSubCategoryViewModel.FGsItemsubCategory.SCATNAME;
            return await _fGsItemsubCategory.FindBySCatName(scatName) ? Json(true) : Json($"Sub-Category {scatName} is already in use");
        }
    }
}
