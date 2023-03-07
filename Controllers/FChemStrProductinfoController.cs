using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    [Route("ChemicalEntry")]
    public class FChemStrProductinfoController : Controller
    {
        private readonly IF_CHEM_STORE_PRODUCTINFO _fChemStoreProductinfo;
        private readonly IBAS_PRODUCTINFO _basProductinfo;
        private readonly IDataProtector _protector;
        private readonly string _title = "Chemical Information";

        public FChemStrProductinfoController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_CHEM_STORE_PRODUCTINFO fChemStoreProductinfo,
            IBAS_PRODUCTINFO basProductinfo)
        {
            _fChemStoreProductinfo = fChemStoreProductinfo;
            _basProductinfo = basProductinfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFChemStrProductinfo(FChemProductEntryViewModel fChemProductEntryViewModel)
        {
            if (ModelState.IsValid)
            {
                var fChemStoreProductinfo = await _fChemStoreProductinfo.FindByIdAsync(int.Parse(_protector.Unprotect(fChemProductEntryViewModel.FChemStoreProductinfo.EncryptedId)));

                if (fChemStoreProductinfo != null)
                {
                    fChemProductEntryViewModel.FChemStoreProductinfo.PRODUCTID = fChemStoreProductinfo.PRODUCTID;
                    fChemProductEntryViewModel.FChemStoreProductinfo.PRODUCTNAME = fChemStoreProductinfo.PRODUCTNAME;

                    if (await _fChemStoreProductinfo.Update(fChemProductEntryViewModel.FChemStoreProductinfo))
                    {
                        TempData["message"] = $"Successfully Updated {_title}.";
                        TempData["type"] = "success";

                        return RedirectToAction(nameof(GetFChemStrProductinfoTable));
                    }
                    TempData["message"] = $"Failed To Update {_title}.";
                        TempData["type"] = "error";
                }
            }
            else
            {
                TempData["message"] = $"Failed To Update {_title}";
                TempData["type"] = "error";
            }

            return View(nameof(EditFChemStrProductinfo), await _fChemStoreProductinfo.GetInitObjByAsync(fChemProductEntryViewModel));
        }

        [HttpGet]
        [Route("Edit/{productId?}")]
        public async Task<IActionResult> EditFChemStrProductinfo(string productId)
        {
            return View(await _fChemStoreProductinfo.GetInitObjByAsync(await _fChemStoreProductinfo.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(productId)))));
        }

        [HttpGet]
        [Route("Details/{productId?}")]
        public async Task<IActionResult> DetailsFChemStrProductinfo(string productId)
        {
            return View(await _fChemStoreProductinfo.FindByIdIncludeAllForDetailsAsync(int.Parse(_protector.Unprotect(productId))));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFChemStrProductinfoTable()
        {
            return View();
        }

        [HttpPost]
        [Route("GetTableData")]
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

                var data = await _fChemStoreProductinfo.GetProductDD();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.PRODUCTNAME.ToUpper().Contains(searchValue)
                                        || m.UNITNAVIGATION.UNAME != null && m.UNITNAVIGATION.UNAME.ToUpper().Contains(searchValue)
                                        || m.TYPENAVIGATION.CTYPE != null && m.TYPENAVIGATION.CTYPE.ToUpper().Contains(searchValue)
                                        || m.COUNTRIES.COUNTRY_NAME != null && m.COUNTRIES.COUNTRY_NAME.ToUpper().Contains(searchValue)
                                        || m.SIZE != null && m.SIZE.ToUpper().Contains(searchValue)
                                        || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
        [Route("Create")]
        public async Task<IActionResult> CreateFChemProductinfo()
        {
            return View(await _fChemStoreProductinfo.GetInitObjByAsync(new FChemProductEntryViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFChemProductinfo(FChemProductEntryViewModel fChemProductEntryViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _fChemStoreProductinfo.InsertByAsync(fChemProductEntryViewModel.FChemStoreProductinfo))
                {
                    fChemProductEntryViewModel.BasProductinfo = new BAS_PRODUCTINFO
                    {
                        PRODNAME = fChemProductEntryViewModel.FChemStoreProductinfo.PRODUCTNAME,
                        CATID = 12,                                    //From BasProductCategory
                        UNIT = fChemProductEntryViewModel.FChemStoreProductinfo.UNIT,
                        REMARKS = fChemProductEntryViewModel.FChemStoreProductinfo.REMARKS,
                        CSID = fChemProductEntryViewModel.FChemStoreProductinfo.PRODUCTID
                    };
                    if (await _basProductinfo.InsertByAsync(fChemProductEntryViewModel.BasProductinfo))
                    {
                        TempData["message"] = $"Successfully added {_title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFChemStrProductinfoTable));
                    }
                    await _fChemStoreProductinfo.Delete(fChemProductEntryViewModel.FChemStoreProductinfo);
                }

                TempData["message"] = $"Failed to add {_title}.";
                TempData["type"] = "error";
                return View(await _fChemStoreProductinfo.GetInitObjByAsync(fChemProductEntryViewModel));
            }

            TempData["message"] = $"Failed to add {_title}.";
            TempData["type"] = "error";
            return View(await _fChemStoreProductinfo.GetInitObjByAsync(fChemProductEntryViewModel));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProdNameInUse(FChemProductEntryViewModel fChemProductEntryViewModel)
        {
            var prodName = fChemProductEntryViewModel.FChemStoreProductinfo.PRODUCTNAME;
            return await _fChemStoreProductinfo.FindByProdName(prodName) ? Json(true) : Json($"Chemical '{prodName}' is already in use");
        }
    }
}