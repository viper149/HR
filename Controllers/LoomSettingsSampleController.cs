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
    public class LoomSettingsSampleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILOOM_SETTINGS_SAMPLE _loomSettingsSample;
        private readonly IDataProtector _protector;
        private readonly string title = "Loom Settings Information (Sample)";

        public LoomSettingsSampleController(UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ILOOM_SETTINGS_SAMPLE loomSettingsSample)
        {
            _userManager = userManager;
            _loomSettingsSample = loomSettingsSample;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetLoomSettingsSample()
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
            var data = (List<LOOM_SETTINGS_SAMPLE>)await _loomSettingsSample.GetAllLoomSettingsSampleAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.DEV.FABCODE != null && m.DEV.FABCODE.ToString().ToUpper().Contains(searchValue))
                                       || (m.DEV.PROG_ != null && m.DEV != null && m.DEV.PROG_.PROG_NO.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                    ).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            foreach (var item in finalData)
            {
                item.EncryptedId = _protector.Protect(item.SETTINGS_ID.ToString());
            }

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateLoomSettingsSample()
        {
            return View(await _loomSettingsSample.GetInitObjByAsync(new LoomSettingsSampleViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoomSettingsSample(LoomSettingsSampleViewModel loomSettingsSampleViewModel)
        {
            if (ModelState.IsValid)
            {
                loomSettingsSampleViewModel.LoomSettingsSample.CREATED_AT = loomSettingsSampleViewModel.LoomSettingsSample.UPDATED_AT = DateTime.Now;
                loomSettingsSampleViewModel.LoomSettingsSample.CREATED_BY = loomSettingsSampleViewModel.LoomSettingsSample.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _loomSettingsSample.InsertByAsync(loomSettingsSampleViewModel.LoomSettingsSample))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetLoomSettingsSample), "LoomSettingsSample");
                }
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(await _loomSettingsSample.GetInitObjByAsync(new LoomSettingsSampleViewModel()));
            }
            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _loomSettingsSample.GetInitObjByAsync(new LoomSettingsSampleViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditLoomSettingsSample(string settingsId)
        {
            var loomSettingsSample = await _loomSettingsSample.FindByIdAsync(int.Parse(_protector.Unprotect(settingsId)));

            if (loomSettingsSample != null)
            {
                var loomSettingsSampleViewModel = await _loomSettingsSample.GetInitObjByAsync(new LoomSettingsSampleViewModel());
                loomSettingsSampleViewModel.LoomSettingsSample = loomSettingsSample;

                loomSettingsSampleViewModel.LoomSettingsSample.EncryptedId = _protector.Protect(loomSettingsSampleViewModel.LoomSettingsSample.SETTINGS_ID.ToString());
                return View(loomSettingsSampleViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetLoomSettingsSample), $"LoomSettingsSample");
        }

        [HttpPost]
        public async Task<IActionResult> EditLoomSettingsSample(LoomSettingsSampleViewModel loomSettingsSampleViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetLoomSettingsSample), $"LoomSettingsSample");
                loomSettingsSampleViewModel.LoomSettingsSample.SETTINGS_ID = int.Parse(_protector.Unprotect(loomSettingsSampleViewModel.LoomSettingsSample.EncryptedId));
                var rndFabtestBulk = await _loomSettingsSample.FindByIdAsync(loomSettingsSampleViewModel.LoomSettingsSample.SETTINGS_ID);
                if (rndFabtestBulk != null)
                {
                    loomSettingsSampleViewModel.LoomSettingsSample.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    loomSettingsSampleViewModel.LoomSettingsSample.UPDATED_AT = DateTime.Now;
                    loomSettingsSampleViewModel.LoomSettingsSample.CREATED_AT = rndFabtestBulk.CREATED_AT;
                    loomSettingsSampleViewModel.LoomSettingsSample.CREATED_BY = rndFabtestBulk.CREATED_BY;

                    if (await _loomSettingsSample.Update(loomSettingsSampleViewModel.LoomSettingsSample))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{title} Not Found";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";

            return View(await _loomSettingsSample.GetInitObjByAsync(new LoomSettingsSampleViewModel()));
        }

        [AcceptVerbs("get", "post")]
        public async Task<IActionResult> GetAllByDevId(int devId)
        {
            try
            {
                return Ok(await _loomSettingsSample.GetAllByDevIdAsync(devId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
