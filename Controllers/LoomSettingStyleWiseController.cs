using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.LoomSetting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class LoomSettingStyleWiseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILOOM_SETTING_STYLE_WISE_M _loomSettingStyleWiseM;
        private readonly ILOOM_SETTING_CHANNEL_INFO _loomSettingChannelInfo;
        private readonly IDataProtector _protector;
        private readonly string title = "Loom Settings Information";

        public LoomSettingStyleWiseController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            ILOOM_SETTING_STYLE_WISE_M loomSettingStyleWiseM,
            ILOOM_SETTING_CHANNEL_INFO loomSettingChannelInfo,
            ILOOM_SETTINGS_FILTER_VALUE loomSettingsFilterValue
        )
        {
            _userManager = userManager;
            _loomSettingStyleWiseM = loomSettingStyleWiseM;
            _loomSettingChannelInfo = loomSettingChannelInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetLoomSettingInfoWithPaged()
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

            var data = (List<LOOM_SETTING_STYLE_WISE_M>)await _loomSettingStyleWiseM.GetAllLoomSettingStyleWiseAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.RPM.ToString().ToUpper().Contains(searchValue)
                                       || m.FABCODENavigation is { STYLE_NAME: { } } && m.FABCODENavigation.STYLE_NAME.ToString().ToUpper().Contains(searchValue)
                                       || m.LOOM_TYPENavigation is { LOOM_TYPE_NAME: { } } && m.LOOM_TYPENavigation.LOOM_TYPE_NAME.ToString().ToUpper().Contains(searchValue)
                                       || m.FILTER_VALUENavigation is { NAME: { } } && m.FILTER_VALUENavigation.NAME.ToString().ToUpper().Contains(searchValue)
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
        public async Task<IActionResult> CreateLoomSetting()
        {
            return View(await GetInfo(new LoomSettingStyleWiseViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoomSetting(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.CREATED_BY = user.Id;
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.UPDATED_BY = user.Id;
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.CREATED_AT = DateTime.Now;
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.UPDATED_AT = DateTime.Now;
                    var isLoomSettingInfoInsert = await _loomSettingStyleWiseM.GetInsertedObjByAsync(loomSettingStyleWiseViewModel.LoomSettingStyleWiseM);

                    if (isLoomSettingInfoInsert != null)
                    {
                        foreach (var item in loomSettingStyleWiseViewModel.LoomSettingChannelInfoList.Where(c => c.CHANNEL_ID == 0))
                        {
                            item.SETTING_ID = isLoomSettingInfoInsert.SETTING_ID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _loomSettingChannelInfo.InsertByAsync(item);
                        }
                        TempData["message"] = "Successfully added Loom Settings Information.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetLoomSettingInfoWithPaged", $"LoomSettingStyleWise");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Loom Settings Information";
                        TempData["type"] = "error";
                        return View(await GetInfo(loomSettingStyleWiseViewModel));
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input. Please Try Again.";
                    TempData["type"] = "error";
                    return View(await GetInfo(loomSettingStyleWiseViewModel));
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Loom Settings Information";
                TempData["type"] = "error";
                return View(await GetInfo(loomSettingStyleWiseViewModel));
            }
        }


        public async Task<LoomSettingStyleWiseViewModel> GetInfo(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel)
        {
            loomSettingStyleWiseViewModel = await _loomSettingStyleWiseM.GetInitObjects(loomSettingStyleWiseViewModel);
            return loomSettingStyleWiseViewModel;
        }

        [HttpGet]
        public async Task<IActionResult> EditLoomSetting(string lsId)
        {
            try
            {
                var loomSettingStyleWiseViewModel = await _loomSettingStyleWiseM.GetInitObjects(new LoomSettingStyleWiseViewModel(), true);
                loomSettingStyleWiseViewModel.LoomSettingStyleWiseM = await _loomSettingStyleWiseM.FindByIdAsync(int.Parse(_protector.Unprotect(lsId)));
                loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.EncryptedId = lsId;

                return View(loomSettingStyleWiseViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditLoomSetting(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(LoomSettingStyleWiseViewModel), $"LoomSettingStyleWise");
                loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.SETTING_ID = int.Parse(_protector.Unprotect(loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.EncryptedId));
                var fHrdLeave = await _loomSettingStyleWiseM.FindByIdAsync(loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.SETTING_ID);
                if (fHrdLeave != null)
                {
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.UPDATED_AT = DateTime.Now;
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.CREATED_AT = fHrdLeave.CREATED_AT;
                    loomSettingStyleWiseViewModel.LoomSettingStyleWiseM.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _loomSettingStyleWiseM.Update(loomSettingStyleWiseViewModel.LoomSettingStyleWiseM))
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
            return View(await _loomSettingStyleWiseM.GetInitObjects(new LoomSettingStyleWiseViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChannel(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel)
        {
            try
            {
                var flag = loomSettingStyleWiseViewModel.LoomSettingChannelInfoList.Where(c => c.CHANNEL_NO.Equals(loomSettingStyleWiseViewModel.LoomSettingChannelInfo.CHANNEL_NO));

                if (!flag.Any())
                {
                    loomSettingStyleWiseViewModel.LoomSettingChannelInfoList.Add(loomSettingStyleWiseViewModel.LoomSettingChannelInfo);
                }
                loomSettingStyleWiseViewModel = await GetChannelDetailsAsync(loomSettingStyleWiseViewModel);
                return PartialView($"AddChannel", loomSettingStyleWiseViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                loomSettingStyleWiseViewModel = await GetChannelDetailsAsync(loomSettingStyleWiseViewModel);
                return PartialView($"AddChannel", loomSettingStyleWiseViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveChannelFromList(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            loomSettingStyleWiseViewModel.LoomSettingChannelInfoList.RemoveAt(int.Parse(removeIndexValue));
            return PartialView($"AddChannel", loomSettingStyleWiseViewModel);
        }
        
        public async Task<LoomSettingStyleWiseViewModel> GetChannelDetailsAsync(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel)
        {
            loomSettingStyleWiseViewModel = await _loomSettingChannelInfo.GetInitChannelData(loomSettingStyleWiseViewModel);
            return loomSettingStyleWiseViewModel;
        }

        public async Task<RND_FABRICINFO> GetStyleDetails(int fabcode)
        {
            try
            {
                var fabDetails = await _loomSettingStyleWiseM.GetStyleDetails(fabcode);
                return fabDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<RND_FABRIC_COUNTINFO> GetCountDetails(int countId)
        {
            try
            {
                var countDetails = await _loomSettingStyleWiseM.GetCountDetails(countId);
                return countDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}