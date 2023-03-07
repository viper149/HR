using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndAnalysisSheetController : Controller
    {
        private readonly IRND_ANALYSIS_SHEET _rndAnalysisSheet;
        private readonly IRND_ANALYSIS_SHEET_DETAILS _rndAnalysisSheetDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public RndAnalysisSheetController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_ANALYSIS_SHEET rndAnalysisSheet,
            IRND_ANALYSIS_SHEET_DETAILS rndAnalysisSheetDetails,
            UserManager<ApplicationUser> userManager
            )
        {
            _rndAnalysisSheet = rndAnalysisSheet;
            _rndAnalysisSheetDetails = rndAnalysisSheetDetails;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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

                var rndAnalysisSheets = await _rndAnalysisSheet.All();

                if (User.IsInRole("Marketing(DGM)") || User.IsInRole("Marketing"))
                {
                    rndAnalysisSheets = rndAnalysisSheets.Where(c => c.RND_HEAD_APPROVE.Equals(true));
                }

                foreach (var item in rndAnalysisSheets)
                {
                    item.EncryptedId = _protector.Protect(item.AID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                rndAnalysisSheets = rndAnalysisSheets.OrderBy(c => c.GetType().GetProperty(subStrings[0]).GetValue(c).GetType().GetProperty(subStrings[1]).GetValue(c.GetType().GetProperty(subStrings[0]).GetValue(c)));
                                break;
                            }
                        case "asc":
                            rndAnalysisSheets = rndAnalysisSheets.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    rndAnalysisSheets = rndAnalysisSheets.OrderByDescending(c => c.GetType().GetProperty(subStrings[0]).GetValue(c).GetType().GetProperty(subStrings[1]).GetValue(c.GetType().GetProperty(subStrings[0]).GetValue(c)));
                                }
                                else
                                {
                                    rndAnalysisSheets = rndAnalysisSheets.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                                }
                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    rndAnalysisSheets = rndAnalysisSheets
                        .Where(m => m.BUYER_REF.ToUpper().Contains(searchValue)
                                    || m.RND_QUERY_NO.ToUpper().Contains(searchValue)
                                    || m.BUYERID.ToString().ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));
                }

                var totalRecords = rndAnalysisSheets.Count();

                return Json(new
                {
                    draw,
                    recordsFiltered = totalRecords,
                    recordsTotal = totalRecords,
                    data = rndAnalysisSheets.Skip(skip).Take(pageSize).ToList()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetAnalysisSheetInfo()
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

        [HttpGet]
        public async Task<IActionResult> CreateAnalysisSheetInfo()
        {
            try
            {
                var rndQueryNo = await _rndAnalysisSheet.GetLastRndQueryNoAsync();

                var rndAnalysisSheetInfoViewModel = new RndAnalysisSheetInfoViewModel
                {
                    RndAnalysisSheet = new RND_ANALYSIS_SHEET
                    {
                        RND_QUERY_NO = rndQueryNo
                    }
                };

                return View(await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnalysisSheetInfo(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheet.CREATED_BY = user.Id;
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheet.UPDATED_BY = user.Id;
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheet.CREATED_AT = DateTime.Now;
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheet.UPDATED_AT = DateTime.Now;
                    var aid = await _rndAnalysisSheet.InsertAndGetIdAsync(rndAnalysisSheetInfoViewModel.RndAnalysisSheet);

                    if (aid != 0)
                    {
                        foreach (var item in rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList)
                        {
                            item.AID = aid;
                            await _rndAnalysisSheetDetails.InsertByAsync(item);
                        }

                        TempData["message"] = "Successfully added Analysis Sheet.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
                    }

                    TempData["message"] = "Failed to Add Analysis Sheet.";
                    TempData["type"] = "error";
                    return View(await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Analysis Sheet.";
                TempData["type"] = "error";
                return View(await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAnalysisSheetInfo(string id)
        {
            try
            {
                var rndAnalysisSheetInfoViewModel = new RndAnalysisSheetInfoViewModel();
                var yarnInfo = await _rndAnalysisSheet.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id)));

                if (yarnInfo != null)
                {
                    rndAnalysisSheetInfoViewModel = await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel);
                    var analysisYarnDetailsList = await _rndAnalysisSheetDetails.GetAnalysisYarnDetailsList(yarnInfo.AID);

                    yarnInfo.EncryptedId = _protector.Protect(yarnInfo.AID.ToString());

                    rndAnalysisSheetInfoViewModel.RndAnalysisSheet = yarnInfo;
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList = analysisYarnDetailsList.ToList();

                    return View(rndAnalysisSheetInfoViewModel);
                }

                TempData["message"] = "Analysis Sheet Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Analysis Sheet.";
                TempData["type"] = "error";
                return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAnalysisSheetInfo(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel)
        {
            var aid = int.Parse(_protector.Unprotect(rndAnalysisSheetInfoViewModel.RndAnalysisSheet.EncryptedId));

            try
            {
                if (ModelState.IsValid)
                {
                    if (rndAnalysisSheetInfoViewModel.RndAnalysisSheet.AID == aid && 
                        rndAnalysisSheetInfoViewModel.RndAnalysisSheet.TOTAL_ENDS.Equals(rndAnalysisSheetInfoViewModel.RndAnalysisSheet.UTOTAL_ENDS) &&
                        rndAnalysisSheetInfoViewModel.RndAnalysisSheet.REED_SPACE.Equals(rndAnalysisSheetInfoViewModel.RndAnalysisSheet.UREED_SPACE))
                    {
                        var analysisSheet = await _rndAnalysisSheet.FindByIdAsync(aid);

                        var user = await _userManager.GetUserAsync(User);
                        rndAnalysisSheetInfoViewModel.RndAnalysisSheet.UPDATED_BY = user.Id;
                        rndAnalysisSheetInfoViewModel.RndAnalysisSheet.UPDATED_AT = DateTime.Now;
                        rndAnalysisSheetInfoViewModel.RndAnalysisSheet.CREATED_AT = analysisSheet.CREATED_AT;
                        rndAnalysisSheetInfoViewModel.RndAnalysisSheet.CREATED_BY = analysisSheet.CREATED_BY;

                        var isScInfoUpdated = await _rndAnalysisSheet.Update(rndAnalysisSheetInfoViewModel.RndAnalysisSheet);

                        if (isScInfoUpdated)
                        {
                            var rndAnalysisSheetDetailses = rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList.Where(e => e.ADID == 0).ToList();

                            foreach (var item in rndAnalysisSheetDetailses)
                            {
                                item.AID = aid;
                            }

                            await _rndAnalysisSheetDetails.InsertRangeByAsync(rndAnalysisSheetDetailses);

                            TempData["message"] = "Successfully Updated Analysis Sheet.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
                        }

                        TempData["message"] = "Failed to Update Analysis Sheet.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
                    }

                    TempData["message"] = "Invalid Analysis Id.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList = (List<RND_ANALYSIS_SHEET_DETAILS>) await _rndAnalysisSheetDetails.GetAnalysisYarnDetailsList(aid);
                return View(await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Analysis Sheet.";
                TempData["type"] = "error";
                rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList = (List<RND_ANALYSIS_SHEET_DETAILS>)await _rndAnalysisSheetDetails.GetAnalysisYarnDetailsList(aid);
                return View(await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAnalysisSheetInfo(string id)
        {
            try
            {
                var rndAnalysisSheetInfoViewModel = new RndAnalysisSheetInfoViewModel();
                var yarnInfo = await _rndAnalysisSheet.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id)));

                if (yarnInfo != null)
                {
                    rndAnalysisSheetInfoViewModel = await _rndAnalysisSheet.GetInitObjects(rndAnalysisSheetInfoViewModel);
                    var analysisYarnDetailsList = await _rndAnalysisSheetDetails.GetAnalysisYarnDetailsList(yarnInfo.AID);

                    yarnInfo.EncryptedId = _protector.Protect(yarnInfo.AID.ToString());

                    rndAnalysisSheetInfoViewModel.RndAnalysisSheet = yarnInfo;
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList = analysisYarnDetailsList.ToList();

                    return View(rndAnalysisSheetInfoViewModel.RndAnalysisSheet);
                }

                TempData["message"] = "Analysis Sheet Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Analysis Sheet.";
                TempData["type"] = "error";
                return RedirectToAction("GetAnalysisSheetInfo", $"RndAnalysisSheet");
            }
        }

        public async Task<IActionResult> GetYarnDetails(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel)
        {
            var analysisYarnDetailsList = await _rndAnalysisSheetDetails.GetAnalysisYarnDetailsList(int.Parse(_protector.Unprotect(rndAnalysisSheetInfoViewModel.RndAnalysisSheet.EncryptedId)));
            rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList = analysisYarnDetailsList.ToList();

            return PartialView($"RndYarnDetailsTable", rndAnalysisSheetInfoViewModel);
        }

        public async Task<MKT_SWATCH_CARD> GetSwatchCardDetails(int swatchId)
        {
            var swatchCardDetails = await _rndAnalysisSheet.GetSwatchCardDetails(swatchId);
            return swatchCardDetails;
        }

        [HttpPost]
        public async Task<IActionResult> AddYarnDetails(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel)
        {
            try
            {
                var result = rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList.Where(e => e.COUNTID.Equals(rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetails.COUNTID ?? 0) && e.YARN_FOR.Equals(rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetails.YARN_FOR));
                
                if (!result.Any())
                {
                    rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList.Add(rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetails);
                }

                return PartialView($"RndYarnDetailsTable", await _rndAnalysisSheet.GetInitObjectsForAddYarnDetailsByAsync(rndAnalysisSheetInfoViewModel));

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return PartialView($"RndYarnDetailsTable", await _rndAnalysisSheet.GetInitObjectsForAddYarnDetailsByAsync(rndAnalysisSheetInfoViewModel));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveYarnFromList(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();

                if (rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList[int.Parse(removeIndexValue)].ADID != 0)
                {
                    await _rndAnalysisSheetDetails.Delete(rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList[int.Parse(removeIndexValue)]);
                }

                rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList.RemoveAt(int.Parse(removeIndexValue));

                return PartialView($"RndYarnDetailsTable", await _rndAnalysisSheet.GetInitObjectsForAddYarnDetailsByAsync(rndAnalysisSheetInfoViewModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}