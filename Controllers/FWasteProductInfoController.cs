using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers

{ 


    public class FWasteProductInfoController : Controller
    {
        private readonly IF_WASTE_PRODUCTINFO _fWasteProductinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " Waste ";

        public FWasteProductInfoController(IDataProtectionProvider dataProtectionProvider,
            IF_WASTE_PRODUCTINFO fWasteProductinfo,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fWasteProductinfo = fWasteProductinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFWasteProductInfo()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        //[Route("GetTableData")]
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

            var data = (List<F_WASTE_PRODUCTINFO>)await _fWasteProductinfo.GetAllWasteProductInfoAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.PRODUCT_NAME.ToString().ToUpper().Contains(searchValue)
                                       || m.WPTYPE.ToString().ToUpper().Contains(searchValue)
                                       || m.WP.UNAME.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
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
        //[Route("Create")]
        public async Task<IActionResult> CreateFWasteProductInfo()
        {
            try
            {
                return View(await _fWasteProductinfo.GetInitObjByAsync(new FWasteProductInfoViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFWasteProductInfo(FWasteProductInfoViewModel fWasteProductInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fWasteProductinfo.InsertByAsync(fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFWasteProductInfo), $"FWasteProductInfo");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fWasteProductInfoViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fWasteProductInfoViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fWasteProductInfoViewModel);
            }
        }


        // edit data


        [HttpGet]
        public async Task<IActionResult> EditFWasteProductInfo(string lbId)
        {
            var fWasteProductinfo = await _fWasteProductinfo.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

            if (fWasteProductinfo != null)
            {
                var fWasteProductinfoViewModel = await _fWasteProductinfo.GetInitObjByAsync(new FWasteProductInfoViewModel());
                fWasteProductinfoViewModel.F_WASTE_PRODUCTINFO = fWasteProductinfo;

                fWasteProductinfoViewModel.F_WASTE_PRODUCTINFO.EncryptedId = _protector.Protect(fWasteProductinfoViewModel.F_WASTE_PRODUCTINFO.WPID.ToString());
                return View(fWasteProductinfoViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFWasteProductInfo), $"FWasteProductInfo");
        }

        [HttpPost]
        public async Task<IActionResult> EditFWasteProductInfo(FWasteProductInfoViewModel fWasteProductInfoViewModel)
        {
            if (ModelState.IsValid)
            {

                fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.WPID = int.Parse(_protector.Unprotect(fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.EncryptedId));

                var fHrdLeave = await _fWasteProductinfo.FindByIdAsync(fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.WPID);
                if (fHrdLeave != null)
                {
                    fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.UPDATED_AT = DateTime.Now;
                    fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.CREATED_AT = fHrdLeave.CREATED_AT;
                    fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _fWasteProductinfo.Update(fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFWasteProductInfo), $"FWasteProductInfo");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(fWasteProductInfoViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(fWasteProductInfoViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fWasteProductInfoViewModel);
        }



        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProductNameInUse(FWasteProductInfoViewModel fWasteProductInfoViewModel)
        {
            var pName = fWasteProductInfoViewModel.F_WASTE_PRODUCTINFO.PRODUCT_NAME;
            return await _fWasteProductinfo.FindByProductName(pName) ? Json(true) : Json($"Product {pName} is already in use");
        }







    }


}
