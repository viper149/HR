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
    public class FGsProductInformationController : Controller
    {
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly IF_GS_ITEMSUB_CATEGORY _fGsItemsubCategory;
        private readonly IBAS_PRODUCTINFO _basProductinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string _title = "General Store Product Information";

        public FGsProductInformationController(IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            IF_GS_ITEMSUB_CATEGORY fGsItemsubCategory,
            IBAS_PRODUCTINFO basProductinfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fGsProductInformation = fGsProductInformation;
            _fGsItemsubCategory = fGsItemsubCategory;
            _basProductinfo = basProductinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFGsProductInformation()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
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
                var data = await _fGsProductInformation.GetAllProductInformationAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.PRODNAME != null && m.PRODNAME.ToUpper().Contains(searchValue))
                                           || (m.PARTNO != null && m.PARTNO.ToUpper().Contains(searchValue))
                                           || (m.PRODID.ToString().ToUpper().Contains(searchValue))
                                           || (m.PROD_LOC != null && m.PROD_LOC.ToUpper().Contains(searchValue))
                                           || (m.SCAT.SCATNAME != null && m.SCAT.SCATNAME.ToUpper().Contains(searchValue))
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
            catch (Exception)
            {
                return Json(BadRequest());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateFGsProductInformation()
        {
            return View(await _fGsProductInformation.GetInitObjByAsync(new FGsProductInformationViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsProductInformation(FGsProductInformationViewModel fGsProductInformationViewModel)
        {
            if (ModelState.IsValid)
            {
                fGsProductInformationViewModel.FGsProductInformation.CREATED_AT = fGsProductInformationViewModel.FGsProductInformation.UPDATED_AT = DateTime.Now;
                fGsProductInformationViewModel.FGsProductInformation.CREATED_BY = fGsProductInformationViewModel.FGsProductInformation.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;
                var fGsProductInformation = await _fGsProductInformation.GetInsertedObjByAsync(fGsProductInformationViewModel
                    .FGsProductInformation);

                if (fGsProductInformation != null)
                {
                    fGsProductInformationViewModel.BasProductinfo = new BAS_PRODUCTINFO
                    {
                        PRODNAME = fGsProductInformationViewModel.FGsProductInformation.PRODNAME,
                        CATID = 25,                                    //From BasProductCategory
                        UNIT = fGsProductInformationViewModel.FGsProductInformation.UNIT,
                        REMARKS = fGsProductInformationViewModel.FGsProductInformation.REMARKS,
                        DESCRIPTION = fGsProductInformationViewModel.FGsProductInformation.DESCRIPTION,
                        GSID = fGsProductInformation.PRODID
                    };
                    if (await _basProductinfo.InsertByAsync(fGsProductInformationViewModel.BasProductinfo))
                    {
                        TempData["message"] = $"Successfully added {_title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsProductInformation));
                    }
                    await _fGsProductInformation.Delete(fGsProductInformationViewModel
                        .FGsProductInformation);
                }
                TempData["message"] = $"Failed to Add {_title}";
                TempData["type"] = "error";
                return View(fGsProductInformationViewModel);
            }

            TempData["message"] = $"Please Enter Valid {_title}";
            TempData["type"] = "error";
            return View(await _fGsProductInformation.GetInitObjByAsync(new FGsProductInformationViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsProductInformation(string prodId)
        {
            return View(await _fGsProductInformation.GetInitObjByAsync(await _fGsProductInformation.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(prodId))), true));
        }

        [HttpPost]
        public async Task<IActionResult> EditFGsProductInformation(FGsProductInformationViewModel fGsProductInformationViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetFGsProductInformation));
                fGsProductInformationViewModel.FGsProductInformation.PRODID = int.Parse(_protector.Unprotect(fGsProductInformationViewModel.FGsProductInformation.EncryptedId));
                var fGsProductInformation = await _fGsProductInformation.FindByIdAsync(fGsProductInformationViewModel.FGsProductInformation.PRODID);
                if (fGsProductInformation != null)
                {
                    fGsProductInformationViewModel.FGsProductInformation.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fGsProductInformationViewModel.FGsProductInformation.UPDATED_AT = DateTime.Now;
                    fGsProductInformationViewModel.FGsProductInformation.CREATED_AT = fGsProductInformation.CREATED_AT;
                    fGsProductInformationViewModel.FGsProductInformation.CREATED_BY = fGsProductInformation.CREATED_BY;

                    if (await _fGsProductInformation.Update(fGsProductInformationViewModel.FGsProductInformation))
                    {
                        TempData["message"] = $"Successfully Updated {_title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"Failed to Update {_title}.";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{_title} Not Found.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(await _fGsProductInformation.GetInitObjByAsync(new FGsProductInformationViewModel()));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProdNameInUse(FGsProductInformationViewModel fGsProductInformationViewModel)
        {
            var prodName = fGsProductInformationViewModel.FGsProductInformation.PRODNAME;
            return await _fGsProductInformation.FindByProdName(prodName) ? Json(true) : Json($"Product '{prodName}' is already in use");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetSubCat(int catId)
        {
            try
            {
                return Ok(await _fGsItemsubCategory.GetSubCatByCatId(catId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
