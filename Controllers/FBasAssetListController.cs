using System;
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
    public class FBasAssetListController : Controller
    {
        private readonly IF_BAS_ASSET_LIST _fBasAssetList;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " Asset List Information";

        public FBasAssetListController(IDataProtectionProvider dataProtectionProvider,
            IF_BAS_ASSET_LIST fBasAssetList,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fBasAssetList = fBasAssetList;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFBasAssetList()
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

            var data = await _fBasAssetList.GetAllFBasAssetListAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m =>m.ASSET_NAME != null && m.ASSET_NAME.ToUpper().Contains(searchValue)
                                       || m.SEC_CODE != null && m.SEC.SECNAME.ToUpper().Contains(searchValue)
                                       || m.SEC_CODE != null && m.SEC_CODE.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
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
        public async Task<IActionResult> CreateFBasAssetList()
        {
            try
            {
                return View(await _fBasAssetList.GetInitObjByAsync(new FBasAssetListViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFBasAssetList(FBasAssetListViewModel fBasAssetListViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _fBasAssetList.FindByAssetName(
                        fBasAssetListViewModel.FBasAssetList.ASSET_NAME, fBasAssetListViewModel.FBasAssetList.SEC_CODE))
                    {
                        if (await _fBasAssetList.InsertByAsync(fBasAssetListViewModel.FBasAssetList))
                        {
                            TempData["message"] = "Successfully Added ";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFBasAssetList), $"FBasAssetList");
                        }
                    }

                    else
                    {
                        TempData["message"] = $"Asset {fBasAssetListViewModel.FBasAssetList.ASSET_NAME} is already in used for selected location.";
                        TempData["type"] = "error";
                        return View(await _fBasAssetList.GetInitObjByAsync(fBasAssetListViewModel));
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(await _fBasAssetList.GetInitObjByAsync(fBasAssetListViewModel));
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(await _fBasAssetList.GetInitObjByAsync(fBasAssetListViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(await _fBasAssetList.GetInitObjByAsync(fBasAssetListViewModel));
            }
        }
    }
}