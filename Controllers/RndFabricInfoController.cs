using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.TargetSegment;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndFabricInfoController : Controller
    {
        private readonly IRND_FABRICINFO _rNdFabricinfo;
        private readonly IRND_FABRIC_COUNTINFO _rNdFabricCountinfo;
        private readonly IRND_YARNCONSUMPTION _rNdYarnconsumption;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRND_FABRICINFO_APPROVAL_DETAILS _rndFabricinfoApprovalDetails;
        private readonly IAGEGROUPRNDFABRICS _agegrouprndfabrics;
        private readonly ITARGETGENDERRNDFABRICS _targetgenderrndfabrics;
        private readonly ITARGETCHARACTERRNDFABRICS _targetcharacterrndfabrics;
        private readonly ITARGETPRICESEGMENTRNDFABRICS _targetpricesegmentrndfabrics;
        private readonly ITARGETFITSTYLERNDFABRICS _targetfitstylerndfabrics;
        private readonly ISEGMENTSEASONRNDFABRICS _segmentseasonrndfabrics;
        private readonly ISEGMENTOTHERSIMILARRNDFABRICS _segmentothersimilarrndfabrics;
        private readonly ISEGMENTCOMSEGMENTRNDFABRICS _segmentcomsegmentrndfabrics;
        private readonly IRND_SAMPLE_INFO_WEAVING _rndSampleInfoWeaving;
        private readonly IRND_SAMPLEINFO_FINISHING _rndSampleinfoFinishing;
        private readonly IDataProtector _protector;

        public RndFabricInfoController(
            IRND_FABRICINFO rNdFabricinfo,
            IRND_FABRIC_COUNTINFO rNdFabricCountinfo,
            IRND_YARNCONSUMPTION rNdYarnconsumption,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_FABRICINFO_APPROVAL_DETAILS rndFabricinfoApprovalDetails,
            IAGEGROUPRNDFABRICS agegrouprndfabrics,
            ITARGETGENDERRNDFABRICS targetgenderrndfabrics,
            ITARGETCHARACTERRNDFABRICS targetcharacterrndfabrics,
            ITARGETPRICESEGMENTRNDFABRICS targetpricesegmentrndfabrics,
            ITARGETFITSTYLERNDFABRICS targetfitstylerndfabrics,
            ISEGMENTSEASONRNDFABRICS segmentseasonrndfabrics,
            ISEGMENTOTHERSIMILARRNDFABRICS segmentothersimilarrndfabrics,
            ISEGMENTCOMSEGMENTRNDFABRICS segmentcomsegmentrndfabrics,
            IRND_SAMPLE_INFO_WEAVING rndSampleInfoWeaving,
            IRND_SAMPLEINFO_FINISHING rndSampleinfoFinishing)
        {
            _rNdFabricinfo = rNdFabricinfo;
            _rNdFabricCountinfo = rNdFabricCountinfo;
            _rNdYarnconsumption = rNdYarnconsumption;
            _userManager = userManager;
            _rndFabricinfoApprovalDetails = rndFabricinfoApprovalDetails;
            _agegrouprndfabrics = agegrouprndfabrics;
            _targetgenderrndfabrics = targetgenderrndfabrics;
            _targetcharacterrndfabrics = targetcharacterrndfabrics;
            _targetpricesegmentrndfabrics = targetpricesegmentrndfabrics;
            _targetfitstylerndfabrics = targetfitstylerndfabrics;
            _segmentseasonrndfabrics = segmentseasonrndfabrics;
            _segmentothersimilarrndfabrics = segmentothersimilarrndfabrics;
            _segmentcomsegmentrndfabrics = segmentcomsegmentrndfabrics;
            _rndSampleInfoWeaving = rndSampleInfoWeaving;
            _rndSampleinfoFinishing = rndSampleinfoFinishing;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Authorize(Policy = "ApproveRndFabric")]
        public async Task<IActionResult> DoApprove(string devId)
        {
            try
            {
                var fabCode = int.Parse(_protector.Unprotect(devId));

                if (await _rndFabricinfoApprovalDetails.FindByFabCodeAsync(fabCode) == false)
                {
                    var user = _userManager.GetUserAsync(User).Result;
                    var roles = await _userManager.GetRolesAsync(user);

                    var findByIdAsync = await _rNdFabricinfo.FindByIdAsync(fabCode);
                    findByIdAsync.APPROVED = true;

                    if (await _rNdFabricinfo.Update(findByIdAsync))
                    {
                        await _rndFabricinfoApprovalDetails.InsertByAsync(new RND_FABRICINFO_APPROVAL_DETAILS
                        {
                            FABCODE = findByIdAsync.FABCODE,
                            APPROVED_BY = user.Id,
                            APPROVAL_ROLE = string.Join(",", roles),
                            CREATED_BY = user.Id,
                            UPDATED_BY = user.Id,
                            CREATED_AT = DateTime.Now,
                            UPDATED_AT = DateTime.Now
                        });
                    }

                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAssociateObjects(int wvId)
        {
            return Json(await _rNdFabricinfo.GetAssociateObjectsByWvIdAsync(wvId));
        }

        [HttpGet]
        public async Task<RND_SAMPLEINFO_FINISHING> GetAssociateObjectsBySFinId(int sFinId)
        {
            try
            {
                var result = await _rNdFabricinfo.GetAssociateObjectsBySFinId(sFinId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<RND_FABTEST_SAMPLE> GetLabTestResult(int ltsId)
        {
            try
            {
                var result = await _rNdFabricinfo.GetLabTestResult(ltsId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        //Count Partial View for Create View
        [HttpPost]
        public async Task<IActionResult> GetRndFabricCountInfoTable(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                rndFabricInfoViewModel = await GetCountConsumptionByWvIdAsync(rndFabricInfoViewModel);
                return PartialView($"GetRndFabricCountInfoTable", rndFabricInfoViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRndFabricCountInfoTableBySFinId(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            var wvId = await _rNdFabricinfo.GetWeavingIdBySFinId(rndFabricInfoViewModel.rND_FABRICINFO.SFINID ?? 0);
            rndFabricInfoViewModel.rND_FABRICINFO.WVID = wvId;
            var dyeingWeavingDetailsListViewModel = await GetCountConsumptionByWvIdAsync(rndFabricInfoViewModel);

            return PartialView($"GetRndFabricCountInfoTable", dyeingWeavingDetailsListViewModel);
        }


        private async Task<RndFabricInfoViewModel> GetCountConsumptionByWvIdAsync(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {

                //var dyeingWeavingDetailsListViewModel = await _rNdFabricinfo.GetDyeingWeavingDetailsByWvIdAsync(rndFabricInfoViewModel.rND_FABRICINFO.WVID);

                rndFabricInfoViewModel = await _rNdFabricinfo.GetDyeingWeavingDetailsByWvIdAsync(rndFabricInfoViewModel);
                rndFabricInfoViewModel = GetCountConsumption(rndFabricInfoViewModel);
                return rndFabricInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //Main Consumption Calculate Function
        public RndFabricInfoViewModel GetCountConsumption(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                var shrinkage = Math.Round((double)((rndFabricInfoViewModel.rND_FABRICINFO.FNPPI - rndFabricInfoViewModel.rND_FABRICINFO.GRPPI) / rndFabricInfoViewModel.rND_FABRICINFO.FNPPI * 100), 8);

                var crimp = rndFabricInfoViewModel.rND_FABRICINFO.CRIMP_PERCENTAGE ?? 12;

                var crimp_per = (100 - crimp) / 100;

                var sumOfWarpRatio = rndFabricInfoViewModel.RndFabricCountInfos.Where(e => e.YARNFOR == 1).Sum(e => e.RATIO);
                var sumOfWeftRatio = rndFabricInfoViewModel.RndFabricCountInfos.Where(e => e.YARNFOR == 2).Sum(e => e.RATIO);

                Response.Headers["TotalWeft"] = sumOfWeftRatio.ToString();

                foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos.Where(item => item.YARNFOR.Equals(1) && rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS != 0 && sumOfWarpRatio != 0 && item.RATIO != 0 && item.NE != 0 && shrinkage != 0))
                {
                    item.AMOUNT = Math.Round((double)(rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS * (item.RATIO / sumOfWarpRatio) / (840 * 2.2046 * item.NE) / crimp_per / ((100 - shrinkage) / 100) / 0.97), 8);

                    //foreach (var i in rndFabricInfoViewModel.RndYarnConsumptions.Where(c=>c.COUNTID.Equals(item.COUNTID) && c.YARNFOR.Equals(item.YARNFOR)))
                    //{
                    //    i.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS * (item.RATIO / sumOfWarpRatio) /
                    //                                       (840 * 2.2046 * item.NE)) / 0.97) / crimp_per) / ((100 - shrinkage) / 100)) / 0.95), 4);
                    //}
                }

                foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos.Where(item => item.YARNFOR.Equals(2) && rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != 0 && rndFabricInfoViewModel.rND_FABRICINFO.GRPPI != 0 && sumOfWeftRatio != 0 && item.RATIO != 0 && item.NE != 0 && shrinkage != 0))
                {

                    item.AMOUNT = Math.Round((double)((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + (rndFabricInfoViewModel.rND_FABRICINFO.LOOMID == 1 ? 3 : 6)) * rndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (item.RATIO / sumOfWeftRatio) / (840 * 2.2046 * item.NE) / ((100 - shrinkage) / 100) / 0.97), 8);

                    //foreach (var i in rndFabricInfoViewModel.RndYarnConsumptions.Where(c => c.COUNTID.Equals(item.COUNTID) && c.YARNFOR.Equals(item.YARNFOR)))
                    //{
                    //    i.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + (rndFabricInfoViewModel.rND_FABRICINFO.LOOMID==1?3:5)) * rndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (item.RATIO / sumOfWeftRatio) / (840 * 2.2046 * item.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);
                    //}

                }

                return rndFabricInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                var recordsTotal = 0;

                var data = await _rNdFabricinfo.GetForDataTableByAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (User.IsInRole("Marketing(DGM)") || User.IsInRole("Marketing"))
                {
                    data = data.Where(c => c.APPROVED).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.FABCODE.ToString().ToUpper().Contains(searchValue)
                                           || m.DEVID != null && m.DEVID.Contains(searchValue)
                                           || m.PROGNO != null && m.PROGNO.ToUpper().Contains(searchValue)
                                           || m.D != null && m.D.DTYPE.ToUpper().Contains(searchValue)
                                           || m.COLORCODENavigation != null && m.COLORCODENavigation.COLOR.ToUpper().Contains(searchValue)
                                           || m.LOOM != null && m.LOOM.LOOM_TYPE_NAME.ToUpper().Contains(searchValue)
                                           || m.BUYER != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                           || m.STYLE_NAME != null && m.STYLE_NAME.ToUpper().Contains(searchValue)).ToList();
                }


                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                if (User.IsInRole("Lock Edit"))
                {
                    foreach (var item in data)
                    {
                        item.APPROVED = false;
                    }
                }
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.FABCODE.ToString());
                }
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                return Json(jsonData);


                //var draw = Request.Form["draw"].FirstOrDefault();
                //var start = Request.Form["start"].FirstOrDefault();
                //var length = Request.Form["length"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                //var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                //int.TryParse(length, out var pageSize);
                //int.TryParse(start, out var skip);

                //var rndFabricInfoListAllAsync = await _rNdFabricinfo.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                //if (User.IsInRole("Lock Edit"))
                //{
                //    foreach (var item in rndFabricInfoListAllAsync.Data)
                //    {
                //        item.APPROVED = false;
                //    }
                //}

                //return Json(new
                //{
                //    draw = rndFabricInfoListAllAsync.Draw,
                //    recordsFiltered = rndFabricInfoListAllAsync.RecordsTotal,
                //    recordsTotal = rndFabricInfoListAllAsync.RecordsTotal,
                //    data = rndFabricInfoListAllAsync.Data
                //});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetRndFabricInfo()
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

        [HttpGet]
        public async Task<IActionResult> CreateRndFabricInfo()
        {
            return View(await _rNdFabricinfo.GetInitObjects(new RndFabricInfoViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndFabricInfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    rndFabricInfoViewModel.rND_FABRICINFO.USRID = user.Id;

                    var previousWeavingData = await _rndSampleInfoWeaving.FindByIdAsync(rndFabricInfoViewModel.rND_FABRICINFO.WVID ?? 0);
                    var previousFinishingData = await _rndSampleinfoFinishing.FindByIdAsync(rndFabricInfoViewModel.rND_FABRICINFO.SFINID ?? 0);

                    rndFabricInfoViewModel.rND_FABRICINFO.STYLE_NAME = previousFinishingData.STYLE_NAME;

                    var primaryKeyDuringInsertByAsync = await _rNdFabricinfo.GetInsertedObjByAsync(rndFabricInfoViewModel.rND_FABRICINFO);

                    if (primaryKeyDuringInsertByAsync != null)
                    {
                        // AGE GROUP
                        foreach (var item in rndFabricInfoViewModel.AgeGroupViewModels.Where(item => item.IsSelected))
                        {
                            await _agegrouprndfabrics.InsertByAsync(new AGEGROUPRNDFABRICS
                            {
                                AGEGROUPID = item.AgeGroupId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // GENDER
                        foreach (var item in rndFabricInfoViewModel.TargetGenderViewModels.Where(item => item.IsSelected))
                        {
                            await _targetgenderrndfabrics.InsertByAsync(new TARGETGENDERRNDFABRICS
                            {
                                GENDERID = item.GenderId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // CHARACTER
                        foreach (var item in rndFabricInfoViewModel.TargetCharacterViewModels.Where(item => item.IsSelected))
                        {
                            await _targetcharacterrndfabrics.InsertByAsync(new TARGETCHARACTERRNDFABRICS
                            {
                                CHARACTERID = item.CharacterId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // PRICE SEGMENT
                        foreach (var item in rndFabricInfoViewModel.TargetPriceSegmentViewModels.Where(item => item.IsSelected))
                        {
                            await _targetpricesegmentrndfabrics.InsertByAsync(new TARGETPRICESEGMENTRNDFABRICS
                            {
                                PRICESEGMENTID = item.PriceSegmentId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // FIT STYLE
                        foreach (var item in rndFabricInfoViewModel.TargetFitStyleViewModels.Where(item => item.IsSelected))
                        {
                            await _targetfitstylerndfabrics.InsertByAsync(new TARGETFITSTYLERNDFABRICS
                            {
                                FITSTYLEID = item.FitStyleId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // SEASON
                        foreach (var item in rndFabricInfoViewModel.SegmentSeasonViewModels.Where(item => item.IsSelected))
                        {
                            await _segmentseasonrndfabrics.InsertByAsync(new SEGMENTSEASONRNDFABRICS
                            {
                                SEASONID = item.SeasonId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // COM. SEGMENT
                        foreach (var item in rndFabricInfoViewModel.ComSegmentViewModels.Where(item => item.IsSelected))
                        {
                            await _segmentcomsegmentrndfabrics.InsertByAsync(new SEGMENTCOMSEGMENTRNDFABRICS
                            {
                                COMSEGMENTID = item.ComSegmentId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE
                            });
                        }

                        // OTHER SIMILAR
                        foreach (var item in rndFabricInfoViewModel.OtherSimilarViewModels.Where(item => item.IsSelected))
                        {
                            await _segmentothersimilarrndfabrics.InsertByAsync(new SEGMENTOTHERSIMILARRNDFABRICS
                            {
                                OTHERSIMILARID = item.OtherSimilarId,
                                FABCODE = primaryKeyDuringInsertByAsync.FABCODE,
                                INPUT = item.Input
                            });
                        }

                        //var countData = await GetCountConsumptionByWvIdAsync(rndFabricInfoViewModel);

                        // RND FABRIC COUNT INFO & YARN CONSUMPTION

                        foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos)
                        {
                            item.FABCODE = primaryKeyDuringInsertByAsync.FABCODE;

                            if (!await _rNdFabricCountinfo.InsertByAsync(item)) continue;

                            foreach (var i in rndFabricInfoViewModel.RndYarnConsumptions.Where(c => c.YARNFOR.Equals(item.YARNFOR) && c.COUNTID.Equals(item.COUNTID)))
                            {
                                i.FABCODE = primaryKeyDuringInsertByAsync.FABCODE;
                                i.COUNTID = item.COUNTID;
                                i.YARNFOR = item.YARNFOR;
                                i.AMOUNT = item.AMOUNT;
                                await _rNdYarnconsumption.InsertByAsync(i);
                            }
                        }

                        //foreach (var item in countData.RndSampleInfoWeavingDetailses)
                        //{
                        //    var fc = new RND_FABRIC_COUNTINFO
                        //    {
                        //        FABCODE = primaryKeyDuringInsertByAsync.FABCODE,
                        //        COUNTID = item.COUNTID,
                        //        YARNTYPE = item.YARNTYPE,
                        //        DESCRIPTION = item.DESCRIPTION,
                        //        COLORCODE = item.COLORCODE,
                        //        LOTID = item.LOTID,
                        //        SUPPID = item.SUPPID,
                        //        RATIO = (double)item.RATIO,
                        //        NE = (double)item.NE,
                        //        YARNFOR = 2
                        //    };
                        //    if (!await _rNdFabricCountinfo.InsertByAsync(fc) || item.BGT == null) continue;
                        //    var yc = new RND_YARNCONSUMPTION
                        //    {
                        //        FABCODE = primaryKeyDuringInsertByAsync.FABCODE,
                        //        COUNTID = (int)item.COUNTID,
                        //        YARNFOR = 2,
                        //        AMOUNT = item.BGT
                        //    };
                        //    await _rNdYarnconsumption.InsertByAsync(yc);
                        //}

                        TempData["message"] = "Successfully added Fabric Information.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
                    }

                    TempData["message"] = "Failed to Add Fabric Details";
                    TempData["type"] = "error";
                    return View(await _rNdFabricinfo.GetInitObjects(new RndFabricInfoViewModel()));
                }

                TempData["message"] = "Failed to Add Fabric Details";
                TempData["type"] = "error";
                return View(await _rNdFabricinfo.GetInitObjects(new RndFabricInfoViewModel()));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Fabric Details";
                TempData["type"] = "error";
                return View(await _rNdFabricinfo.GetInitObjects(new RndFabricInfoViewModel()));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRndFabricInfo(string rndFabricInfoId)
        {
            try
            {
                RND_FABRICINFO rndFabricInfo = null;

                if (User.IsInRole("Lock Edit"))
                {
                    rndFabricInfo = await _rNdFabricinfo.FindByFabCodeIAsync(int.Parse(_protector.Unprotect(rndFabricInfoId)), details: true);
                }
                else
                {
                    rndFabricInfo = await _rNdFabricinfo.FindByFabCodeIAsync(int.Parse(_protector.Unprotect(rndFabricInfoId)));
                }

                if (rndFabricInfo != null)
                {


                    return View(await _rNdFabricinfo.GetEditInfo(new RndFabricInfoViewModel()
                    {
                        rND_FABRICINFO = rndFabricInfo
                    }));
                }

                TempData["message"] = "Fabric details not found.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRndFabricInfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (rndFabricInfoViewModel.rND_FABRICINFO.FABCODE > 0)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        rndFabricInfoViewModel.rND_FABRICINFO.USRID = user.Id;
                        var isRndFabricInfoUpdated = await _rNdFabricinfo.Update(rndFabricInfoViewModel.rND_FABRICINFO);
                        var fabCode = int.Parse(_protector.Unprotect(rndFabricInfoViewModel.rND_FABRICINFO.EncryptedId));
                        //var fabCode = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE;

                        if (isRndFabricInfoUpdated)
                        {
                            var agegrouprndfabricses = await _agegrouprndfabrics.All();
                            var targetgenderrndfabricses = await _targetgenderrndfabrics.All();
                            var targetcharacterrndfabricses = await _targetcharacterrndfabrics.All();
                            var targetpricesegmentrndfabricses = await _targetpricesegmentrndfabrics.All();
                            var targetfitstylerndfabricses = await _targetfitstylerndfabrics.All();
                            var segmentseasonrndfabricses = await _segmentseasonrndfabrics.All();
                            var segmentcomsegmentrndfabricses = await _segmentcomsegmentrndfabrics.All();
                            var segmentothersimilarrndfabricses = await _segmentothersimilarrndfabrics.All();

                            // AGE GROUP
                            foreach (var item in rndFabricInfoViewModel.AgeGroupViewModels)
                            {
                                var firstOrDefaultAsync = agegrouprndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.AGEGROUPID.Equals(item.AgeGroupId));

                                if (item.IsSelected)
                                {
                                    if (firstOrDefaultAsync == null)
                                    {
                                        await _agegrouprndfabrics.InsertByAsync(new AGEGROUPRNDFABRICS
                                        {
                                            AGEGROUPID = item.AgeGroupId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (firstOrDefaultAsync != null)
                                    {
                                        await _agegrouprndfabrics.Delete(firstOrDefaultAsync);
                                    }
                                }
                            }

                            // GENDER
                            foreach (var item in rndFabricInfoViewModel.TargetGenderViewModels)
                            {
                                var firstOrDefault = targetgenderrndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.GENDERID.Equals(item.GenderId));

                                if (item.IsSelected)
                                {
                                    if (firstOrDefault == null)
                                    {
                                        await _targetgenderrndfabrics.InsertByAsync(new TARGETGENDERRNDFABRICS
                                        {
                                            GENDERID = item.GenderId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (firstOrDefault != null)
                                    {
                                        await _targetgenderrndfabrics.Delete(firstOrDefault);
                                    }
                                }
                            }

                            // CHARACTER
                            foreach (var item in rndFabricInfoViewModel.TargetCharacterViewModels)
                            {
                                var firstOrDefault = targetcharacterrndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.CHARACTERID.Equals(item.CharacterId));

                                if (item.IsSelected)
                                {
                                    if (firstOrDefault == null)
                                    {
                                        await _targetcharacterrndfabrics.InsertByAsync(new TARGETCHARACTERRNDFABRICS
                                        {
                                            CHARACTERID = item.CharacterId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (firstOrDefault != null)
                                    {
                                        await _targetcharacterrndfabrics.Delete(firstOrDefault);
                                    }
                                }
                            }

                            // PRICE SEGMENT
                            foreach (var item in rndFabricInfoViewModel.TargetPriceSegmentViewModels)
                            {
                                var targetpricesegmentrndfabrics = targetpricesegmentrndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.PRICESEGMENTID.Equals(item.PriceSegmentId));

                                if (item.IsSelected)
                                {
                                    if (targetpricesegmentrndfabrics == null)
                                    {
                                        await _targetpricesegmentrndfabrics.InsertByAsync(new TARGETPRICESEGMENTRNDFABRICS
                                        {
                                            PRICESEGMENTID = item.PriceSegmentId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (targetpricesegmentrndfabrics != null)
                                    {
                                        await _targetpricesegmentrndfabrics.Delete(targetpricesegmentrndfabrics);
                                    }
                                }
                            }

                            // FIT STYLE
                            foreach (var item in rndFabricInfoViewModel.TargetFitStyleViewModels)
                            {
                                var targetfitstylerndfabrics = targetfitstylerndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.FITSTYLEID.Equals(item.FitStyleId));

                                if (item.IsSelected)
                                {
                                    if (targetfitstylerndfabrics == null)
                                    {
                                        await _targetfitstylerndfabrics.InsertByAsync(new TARGETFITSTYLERNDFABRICS
                                        {
                                            FITSTYLEID = item.FitStyleId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (targetfitstylerndfabrics != null)
                                    {
                                        await _targetfitstylerndfabrics.Delete(targetfitstylerndfabrics);
                                    }
                                }
                            }

                            // SEASON
                            foreach (var item in rndFabricInfoViewModel.SegmentSeasonViewModels)
                            {
                                var segmentseasonrndfabrics = segmentseasonrndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.SEASONID.Equals(item.SeasonId));

                                if (item.IsSelected)
                                {
                                    if (segmentseasonrndfabrics == null)
                                    {
                                        await _segmentseasonrndfabrics.InsertByAsync(new SEGMENTSEASONRNDFABRICS
                                        {
                                            SEASONID = item.SeasonId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (segmentseasonrndfabrics != null)
                                    {
                                        await _segmentseasonrndfabrics.Delete(segmentseasonrndfabrics);
                                    }
                                }
                            }

                            // COM. SEGMENT
                            foreach (var item in rndFabricInfoViewModel.ComSegmentViewModels)
                            {
                                var segmentcomsegmentrndfabrics = segmentcomsegmentrndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.COMSEGMENTID.Equals(item.ComSegmentId));

                                if (item.IsSelected)
                                {
                                    if (segmentcomsegmentrndfabrics == null)
                                    {
                                        await _segmentcomsegmentrndfabrics.InsertByAsync(new SEGMENTCOMSEGMENTRNDFABRICS
                                        {
                                            COMSEGMENTID = item.ComSegmentId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE
                                        });
                                    }
                                }
                                else
                                {
                                    if (segmentcomsegmentrndfabrics != null)
                                    {
                                        await _segmentcomsegmentrndfabrics.Delete(segmentcomsegmentrndfabrics);
                                    }
                                }
                            }

                            // OTHER SIMILAR
                            foreach (var item in rndFabricInfoViewModel.OtherSimilarViewModels)
                            {
                                var segmentothersimilarrndfabrics = segmentothersimilarrndfabricses.FirstOrDefault(e => e.FABCODE.Equals(fabCode) && e.OTHERSIMILARID.Equals(item.OtherSimilarId));

                                if (item.IsSelected)
                                {
                                    if (segmentothersimilarrndfabrics == null)
                                    {
                                        await _segmentothersimilarrndfabrics.InsertByAsync(new SEGMENTOTHERSIMILARRNDFABRICS
                                        {
                                            OTHERSIMILARID = item.OtherSimilarId,
                                            FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE,
                                            INPUT = item.Input
                                        });
                                    }
                                    else
                                    {
                                        segmentothersimilarrndfabrics.OTHERSIMILARID = item.OtherSimilarId;
                                        segmentothersimilarrndfabrics.FABCODE = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE;
                                        segmentothersimilarrndfabrics.INPUT = item.Input;

                                        await _segmentothersimilarrndfabrics.Update(segmentothersimilarrndfabrics);
                                    }
                                }
                                else
                                {
                                    if (segmentothersimilarrndfabrics != null)
                                    {
                                        await _segmentothersimilarrndfabrics.Delete(segmentothersimilarrndfabrics);
                                    }
                                }
                            }

                            // RND FABRIC COUNT INFO & YARN CONSUMPTION


                            foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos.Where(c => c.TRNSID.Equals(0)))
                            {
                                item.FABCODE = fabCode;

                                if (!await _rNdFabricCountinfo.InsertByAsync(item)) continue;
                                //var yarnConsumption = await _rNdYarnconsumption.GetConsumptionByCountIdAndFabCodeAsync((int)item.COUNTID, item.FABCODE, (int)item.YARNFOR);

                                RND_YARNCONSUMPTION yarnconsumption = new RND_YARNCONSUMPTION
                                {
                                    FABCODE = fabCode,
                                    COUNTID = item.COUNTID,
                                    YARNFOR = item.YARNFOR,
                                    AMOUNT = item.AMOUNT,
                                    COLOR = item.COLORCODE
                                };
                                await _rNdYarnconsumption.InsertByAsync(yarnconsumption);
                                //foreach (var i in rndFabricInfoViewModel.RndYarnConsumptions.Where(c => c.YARNFOR.Equals(item.YARNFOR) && c.COUNTID.Equals(item.COUNTID)))
                                //{
                                //    i.FABCODE = fabCode;
                                //    i.COUNTID = item.COUNTID;
                                //    i.YARNFOR = item.YARNFOR;
                                //    i.AMOUNT = item.AMOUNT;
                                //    await _rNdYarnconsumption.InsertByAsync(i);
                                //}
                            }

                            foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos.Where(e => e.TRNSID > 0))
                            {
                                var a = await _rNdFabricCountinfo.Update(item);
                                var x = await _rNdYarnconsumption.GetPrimaryKeyByCountIdAndFabCodeAsync(item.COUNTID ?? 0,
                                    item.FABCODE, item.YARNFOR ?? 0, item.COLORCODE);
                                if (x == null)
                                {
                                    await _rNdYarnconsumption.InsertByAsync(new RND_YARNCONSUMPTION
                                    {
                                        FABCODE = fabCode,
                                        COUNTID = item.COUNTID,
                                        YARNFOR = item.YARNFOR,
                                        AMOUNT = item.AMOUNT,
                                        COLOR = item.COLORCODE
                                    });
                                }
                                x.AMOUNT = item.AMOUNT;
                                x.COLOR = item.COLORCODE;
                                var b = await _rNdYarnconsumption.Update(x);
                            }

                            TempData["message"] = "Fabric details successfully updated.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
                        }
                    }
                    TempData["message"] = "Fabric details not found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
                }
                var err = ModelState.Values.SelectMany(v => v.Errors).ToList();

                return View(rndFabricInfoViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRndFabricInfo(string rndFabricInfoId)
        {
            try
            {
                var rndFabricInfo = await _rNdFabricinfo.FindByFabCodeIAsync(int.Parse(_protector.Unprotect(rndFabricInfoId)));

                if (false)
                {
                    var findByFabCodeIAsync = (List<RND_FABRIC_COUNTINFO>)await _rNdFabricCountinfo.FindByFabCodeIAsync(rndFabricInfo.FABCODE);
                    var byFabCodeIAsync = (List<RND_YARNCONSUMPTION>)await _rNdYarnconsumption.FindByFabCodeIAsync(rndFabricInfo.FABCODE);
                    var agegrouprndfabricses = await _agegrouprndfabrics.All();
                    var targetgenderrndfabricses = await _targetgenderrndfabrics.All();
                    var targetcharacterrndfabricses = await _targetcharacterrndfabrics.All();
                    var targetpricesegmentrndfabricses = await _targetpricesegmentrndfabrics.All();
                    var targetfitstylerndfabricses = await _targetfitstylerndfabrics.All();
                    var segmentseasonrndfabricses = await _segmentseasonrndfabrics.All();
                    var segmentcomsegmentrndfabricses = await _segmentcomsegmentrndfabrics.All();
                    var segmentothersimilarrndfabricses = await _segmentothersimilarrndfabrics.All();

                    var ageGroups = agegrouprndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var genders = targetgenderrndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var characters = targetcharacterrndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var priceSegments = targetpricesegmentrndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var fitStyles = targetfitstylerndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var seasons = segmentseasonrndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var comSegments = segmentcomsegmentrndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();
                    var otherSimilars = segmentothersimilarrndfabricses.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList();

                    // DELETE AGE GROUP
                    if (ageGroups.Any())
                    {
                        await _agegrouprndfabrics.DeleteRange(ageGroups);
                    }

                    // DELETE GENDER
                    if (genders.Any())
                    {
                        await _targetgenderrndfabrics.DeleteRange(genders);
                    }

                    // DELETE CHARACTER
                    if (characters.Any())
                    {
                        await _targetcharacterrndfabrics.DeleteRange(characters);
                    }

                    // DELETE PRICE SEGMENT
                    if (priceSegments.Any())
                    {
                        await _targetpricesegmentrndfabrics.DeleteRange(priceSegments);
                    }

                    // DELETE FIT STYLE
                    if (fitStyles.Any())
                    {
                        await _targetfitstylerndfabrics.DeleteRange(fitStyles);
                    }

                    // DELETE SEASON
                    if (seasons.Any())
                    {
                        await _segmentseasonrndfabrics.DeleteRange(seasons);
                    }

                    // DELETE COM. SEGMENT
                    if (comSegments.Any())
                    {
                        await _segmentcomsegmentrndfabrics.DeleteRange(comSegments);
                    }

                    // DELETE OTHER SIMILAR
                    if (otherSimilars.Any())
                    {
                        await _segmentothersimilarrndfabrics.DeleteRange(otherSimilars);
                    }

                    // DELETE COUNT INFO
                    if (findByFabCodeIAsync.Where(e => e.FABCODE.Equals(rndFabricInfo.FABCODE)).ToList().Any())
                    {
                        await _rNdFabricCountinfo.DeleteRange(findByFabCodeIAsync);
                    }

                    // DELETE YARN CONSUMPTION
                    if (byFabCodeIAsync.Any())
                    {
                        await _rNdYarnconsumption.DeleteRange(byFabCodeIAsync);
                    }

                    // DELETE FABRIC INFO
                    await _rNdFabricinfo.Delete(rndFabricInfo);

                    TempData["message"] = "Successfully Removed RND Fabric Information Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
                }

                if (rndFabricInfo != null)
                {
                    rndFabricInfo.IS_DELETE = true;

                    if (await _rNdFabricinfo.Update(rndFabricInfo))
                    {
                        TempData["message"] = "Successfully Mute R&D Fabric Information";
                        TempData["type"] = "success";
                        //return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
                    }
                }

                TempData["message"] = "Invalid Operation.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
            catch (Exception)
            {
                TempData["message"] = $"Invalid Operation.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsRndFabricInfo(string rndFabricInfoId)
        {
            try
            {
                var rndFabricInfo = await _rNdFabricinfo.FindByFabCodeIAsync(int.Parse(_protector.Unprotect(rndFabricInfoId)), details: true);

                if (rndFabricInfo != null)
                {
                    rndFabricInfo.EncryptedId = _protector.Protect(rndFabricInfo.FABCODE.ToString());
                    var rndFabricInfoCountAndYarnConsumptionViewModel = new RndFabricInfoCountAndYarnConsumptionViewModel
                    {
                        rND_FABRICINFO = rndFabricInfo,
                        RndFabricCountInfoViewModels = (List<RndFabricCountInfoViewModel>)await _rNdFabricCountinfo.FindByFabCodeIAllAsync(rndFabricInfo.FABCODE)
                    };

                    return View(await _rNdFabricinfo.GetSelectedTargetSegmentAsync(rndFabricInfoCountAndYarnConsumptionViewModel));
                }

                TempData["message"] = "Fabric details not found.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Value with [ {rndFabricInfoId} ], Not found. Go back and try again";
                return View($"NotFound");
            }
        }

        [HttpGet]
        public async Task<RND_FABRIC_COUNTINFO> GetSingleCountDetails(string trnsId)
        {
            try
            {
                var rndFabricInfo = await _rNdFabricCountinfo.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId)));
                var rndCountYarnConsump = await _rNdYarnconsumption.GetConsumptionByCountIdAndFabCodeAsync(rndFabricInfo.COUNTID ?? 0, rndFabricInfo.FABCODE, rndFabricInfo.YARNFOR ?? 0);

                rndFabricInfo.EncryptedId = trnsId;

                if (rndCountYarnConsump != 0)
                {
                    rndFabricInfo.AMOUNT = rndCountYarnConsump;
                }
                return rndFabricInfo;
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Value Not found. Go back and try again";
                return null;
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult RemoveFabricCountinfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        rndFabricInfoViewModel.Items.RemoveAt(rndFabricInfoViewModel.RemoveIndexValue);


        //        var shrinkage = Math.Round((double)((((rndFabricInfoViewModel.rND_FABRICINFO.FNPPI + 1) - rndFabricInfoViewModel.rND_FABRICINFO.GRPPI) / rndFabricInfoViewModel.rND_FABRICINFO.FNPPI) * 100), 4);

        //        var crimp = rndFabricInfoViewModel.rND_FABRICINFO.CRIMP_PERCENTAGE ?? 12;

        //        var crimp_per = (100 - crimp) / 100;

        //        switch (rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR)
        //        {
        //            case 1:
        //                var sumOfRatio = rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals("1")).Sum(e => e.RATIO);
        //                rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals("1")).Select(e =>
        //                {
        //                    if (rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS != null)
        //                        //e.AMOUNT = (double?)Math.Round(
        //                        //    (decimal)((rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS *
        //                        //                (e.RATIO / sumOfRatio) * 1.4) /
        //                        //               (840 * 2.2046 * e.NE)), 4);


        //                        e.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS * (e.RATIO / sumOfRatio) /
        //                                                           (840 * 2.2046 * e.NE)) / 0.97) / crimp_per) / ((100 - shrinkage) / 100)) / 0.95), 4);

        //                    return e;
        //                }).ToList();
        //                break;
        //            case 2:

        //                var sumOfWeftRatio = rndFabricInfoViewModel.Items.Where(e => e.YARNFOR == 2).Sum(e => e.RATIO);

        //                switch (rndFabricInfoViewModel.rND_FABRICINFO.LOOM.LOOM_TYPE_NAME.ToLower())
        //                {
        //                    case "airjet":
        //                        rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals(2)).Select(e =>
        //                        {
        //                            if (rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != null && rndFabricInfoViewModel.rND_FABRICINFO.FNPPI != null)
        //                                //e.AMOUNT = (double?)Math.Round(
        //                                //    (decimal)((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 3) *
        //                                //                 rndFabricInfoViewModel.rND_FABRICINFO.FNPPI) * 1.09) /
        //                                //               (840 * 2.2046 * e.NE)), 4);


        //                                e.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 3) *
        //                                    rndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (e.RATIO / sumOfWeftRatio) / (840 * 2.2046 * e.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);

        //                            return e;
        //                        }).ToList();
        //                        break;
        //                    case "rapier":
        //                        rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals(2)).Select(e =>
        //                        {
        //                            if (rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != null && rndFabricInfoViewModel.rND_FABRICINFO.FNPPI != null)
        //                                //e.AMOUNT = (double?)Math.Round(
        //                                //    (decimal)((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 5) *
        //                                //                 rndFabricInfoViewModel.rND_FABRICINFO.FNPPI) * 1.09) /
        //                                //               (840 * 2.2046 * e.NE)), 4);


        //                                e.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 5) *
        //                                    rndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (e.RATIO / sumOfWeftRatio) / (840 * 2.2046 * e.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                            return e;
        //                        }).ToList();
        //                        break;
        //                }

        //                break;
        //        }

        //        return PartialView($"FabricCountinfoTable", rndFabricInfoViewModel);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new EmptyResult());
        //    }
        //}

        //private ModifyRndFabricInfoViewModel GetWithAmountForFEAddFabricCountinfo(ModifyRndFabricInfoViewModel modifyRndFabricInfoViewModel)
        //{

        //    try
        //    {
        //        var shrinkage = Math.Round((double)((((modifyRndFabricInfoViewModel.rND_FABRICINFO.FNPPI + 1) - modifyRndFabricInfoViewModel.rND_FABRICINFO.GRPPI) / modifyRndFabricInfoViewModel.rND_FABRICINFO.FNPPI) * 100), 4);


        //        var crimp = modifyRndFabricInfoViewModel.rND_FABRICINFO.CRIMP_PERCENTAGE ?? 12;

        //        var crimp_per = (100 - crimp) / 100;

        //        var sumOfWarpRatio = modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_FABRIC_COUNTINFO.YARNFOR == 1).Sum(e => e.rND_FABRIC_COUNTINFO.RATIO);
        //        var sumOfWeftRatio = modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_FABRIC_COUNTINFO.YARNFOR == 2).Sum(e => e.rND_FABRIC_COUNTINFO.RATIO);

        //        switch (modifyRndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR)
        //        {
        //            case 1:
        //                modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_YARNCONSUMPTION.YARNFOR.Equals(1)).Select(e =>
        //                {
        //                    if (modifyRndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS != null)
        //                        e.rND_YARNCONSUMPTION.AMOUNT = Math.Round((double)(((((modifyRndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS * (e.rND_FABRIC_COUNTINFO.RATIO / sumOfWarpRatio) / (840 * 2.2046 * e.rND_FABRIC_COUNTINFO.NE)) / 0.97) / crimp_per) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                    return e;
        //                }).ToList();
        //                break;
        //            case 2:
        //                switch (modifyRndFabricInfoViewModel.rND_FABRICINFO.LOOMID)
        //                {
        //                    case 1:
        //                        modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_YARNCONSUMPTION.YARNFOR.Equals(2)).Select(e =>
        //                        {
        //                            if (modifyRndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != null && modifyRndFabricInfoViewModel.rND_FABRICINFO.GRPPI != null && e.rND_FABRIC_COUNTINFO.RATIO != null && e.rND_FABRIC_COUNTINFO.NE != null)
        //                                e.rND_YARNCONSUMPTION.AMOUNT = Math.Round((double)(((((modifyRndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 3) *
        //                                    modifyRndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (e.rND_FABRIC_COUNTINFO.RATIO / sumOfWeftRatio) / (840 * 2.2046 * e.rND_FABRIC_COUNTINFO.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                            return e;
        //                        }).ToList();
        //                        break;
        //                    case 2:
        //                        modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_YARNCONSUMPTION.YARNFOR.Equals(2)).Select(e =>
        //                        {
        //                            if (modifyRndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != null && modifyRndFabricInfoViewModel.rND_FABRICINFO.GRPPI != null && e.rND_FABRIC_COUNTINFO.RATIO != null && e.rND_FABRIC_COUNTINFO.NE != null)
        //                                e.rND_YARNCONSUMPTION.AMOUNT = Math.Round((double)(((((modifyRndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 5) *
        //                                    modifyRndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (e.rND_FABRIC_COUNTINFO.RATIO / sumOfWeftRatio) / (840 * 2.2046 * e.rND_FABRIC_COUNTINFO.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                            return e;
        //                        }).ToList();
        //                        break;
        //                }
        //                break;
        //        }

        //        return modifyRndFabricInfoViewModel;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        private async Task<RndFabricInfoViewModel> GetCountDetailsInfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                rndFabricInfoViewModel = await _rNdFabricCountinfo.GetCountDetailsInfo(rndFabricInfoViewModel);
                return rndFabricInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FeAddFabricCountinfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                ModelState.Clear();
                var fabCode = int.Parse(_protector.Unprotect(rndFabricInfoViewModel.rND_FABRICINFO.EncryptedId));

                if (fabCode > 0)
                {
                    if (rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.EncryptedId != null)
                    {
                        rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.TRNSID = int.Parse(_protector.Unprotect(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.EncryptedId));
                    }
                    var rndFabricCountinfoList = await _rNdFabricCountinfo.FindByFabCodeIAsync(fabCode);

                    var ifExistInRndFabricCountinfoList = rndFabricCountinfoList.Where(e => e.COUNTID.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID) && e.YARNFOR.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR) && e.LOTID.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.LOTID) && e.COLORCODE.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COLORCODE));

                    if (rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.TRNSID > 0)
                    {
                        var oldCount = 0;
                        foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos.Where(c => c.TRNSID.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.TRNSID)))
                        {
                            oldCount = item.COUNTID ?? 0;
                            item.COUNTID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID;
                            item.YARNTYPE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNTYPE;
                            item.DESCRIPTION = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.DESCRIPTION;
                            item.LOTID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.LOTID;
                            item.SUPPID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.SUPPID;
                            item.COLORCODE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COLORCODE;
                            item.RATIO = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.RATIO;
                            item.NE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.NE;
                            item.YARNFOR = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR;

                            
                            
                        }
                        rndFabricInfoViewModel = GetCountConsumption(rndFabricInfoViewModel);
                        foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos.Where(c => c.TRNSID.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.TRNSID)))
                        {
                            var consumpDetails = await _rNdYarnconsumption.GetPrimaryKeyByCountIdAndFabCodeAsync(oldCount,
                                    item.FABCODE, item.YARNFOR ?? 0, item.COLORCODE);

                            consumpDetails.AMOUNT = item.AMOUNT;
                            consumpDetails.COUNTID = item.COUNTID;

                            await _rNdFabricCountinfo.Update(item);
                            await _rNdYarnconsumption.Update(consumpDetails);
                        }
                    }
                    else
                    {
                        var rndFabricCountinfoAndRndYarnConsumptionViewModels = rndFabricInfoViewModel.RndFabricCountInfos.Where(e => e.DESCRIPTION == rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.DESCRIPTION && e.COUNTID == rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID && e.YARNFOR.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR) && e.LOTID.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.LOTID) && e.COLORCODE.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COLORCODE));

                        if (!ifExistInRndFabricCountinfoList.Any() && !rndFabricCountinfoAndRndYarnConsumptionViewModels.Any())
                        {
                            rndFabricInfoViewModel.RndFabricCountInfos.Add(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO);
                            rndFabricInfoViewModel.RndYarnConsumptions.Add(rndFabricInfoViewModel.rND_YARNCONSUMPTION);

                            rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                            return PartialView($"FEAddFabricCountinfoTable", GetCountConsumption(rndFabricInfoViewModel));
                        }
                    }
                    rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                    return PartialView($"FEAddFabricCountinfoTable", GetCountConsumption(rndFabricInfoViewModel));
                }

                rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                return PartialView($"FEAddFabricCountinfoTable", GetCountConsumption(rndFabricInfoViewModel));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("We can not process your request right now.<br />1. Refresh your page.<br />2. Try again with the correct data.<br />3. Contact With the administrator.");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Obsolete]
        //public async Task<IActionResult> AddFabricCountinfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        var countName = await _bAsYarnCountinfo.FindCountNameByIdAsync(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID);
        //        var lotNo = await _bAsYarnLotinfo.FindLotNoByIdAsync(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.LOTID ?? 0);
        //        var suppName = await _bAsSupplierinfo.FindSupplierNameByIdAsync(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.SUPPID ?? 0);
        //        var color = await _basColor.FindByIdAsync(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COLORCODE ?? 0);
        //        var result = rndFabricInfoViewModel.Items
        //            .Where(e => e.COUNTID
        //                            .Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID) && e.YARNFOR.Equals(rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR))
        //            .Select(e => e.COUNTID);

        //        if (result.Any()) return PartialView($"FabricCountinfoTable", rndFabricInfoViewModel);
        //        switch (rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR)
        //        {
        //            case 1 when rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS.HasValue:
        //                rndFabricInfoViewModel.Items.Add(new RndFabricInfoAndYarnConsumption()
        //                {
        //                    COUNTID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID ?? 0,
        //                    COUNTNAME = countName,
        //                    YARNTYPE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNTYPE,
        //                    LOTID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.LOTID,
        //                    LOTNO = lotNo,
        //                    SUPPID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.SUPPID,
        //                    SUPPNAME = suppName,
        //                    COLORCODE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COLORCODE,
        //                    COLOR = color.COLOR,
        //                    RATIO = (double)rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.RATIO,
        //                    NE = (double)rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.NE,
        //                    YARNFOR = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR,
        //                    AMOUNT = rndFabricInfoViewModel.rND_YARNCONSUMPTION.AMOUNT ?? 0,
        //                    OverHead = rndFabricInfoViewModel.NumberOfItems
        //                });
        //                break;
        //            case 2 when rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE.HasValue && rndFabricInfoViewModel.rND_FABRICINFO.FNPPI.HasValue:
        //                rndFabricInfoViewModel.Items.Add(new RndFabricInfoAndYarnConsumption()
        //                {
        //                    COUNTID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COUNTID ?? 0,
        //                    COUNTNAME = countName,
        //                    YARNTYPE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNTYPE,
        //                    LOTID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.LOTID,
        //                    LOTNO = lotNo,
        //                    SUPPID = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.SUPPID,
        //                    SUPPNAME = suppName,
        //                    COLORCODE = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.COLORCODE,
        //                    COLOR = color.COLOR,
        //                    RATIO = (double)rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.RATIO,
        //                    NE = (double)rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.NE,
        //                    YARNFOR = rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR,
        //                    AMOUNT = rndFabricInfoViewModel.rND_YARNCONSUMPTION.AMOUNT ?? 0,
        //                    OverHead = rndFabricInfoViewModel.NumberOfItems
        //                });

        //                rndFabricInfoViewModel = GetWithAmount(rndFabricInfoViewModel);
        //                break;
        //        }
        //        return PartialView($"FabricCountinfoTable", rndFabricInfoViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        return View($"Error");
        //    }
        //}

        //[Obsolete]
        //private static RndFabricInfoViewModel GetWithAmount(RndFabricInfoViewModel rndFabricInfoViewModel)
        //{

        //    var shrinkage = Math.Round((double)((((rndFabricInfoViewModel.rND_FABRICINFO.FNPPI + 1) - rndFabricInfoViewModel.rND_FABRICINFO.GRPPI) / rndFabricInfoViewModel.rND_FABRICINFO.FNPPI) * 100), 4);


        //    var crimp = rndFabricInfoViewModel.rND_FABRICINFO.CRIMP_PERCENTAGE ?? 12;

        //    var crimp_per = (100 - crimp) / 100;

        //    var sumOfWarpRatio = rndFabricInfoViewModel.Items.Where(e => e.YARNFOR == 1).Sum(e => e.RATIO);
        //    var sumOfWeftRatio = rndFabricInfoViewModel.Items.Where(e => e.YARNFOR == 2).Sum(e => e.RATIO);

        //    switch (rndFabricInfoViewModel.rND_FABRIC_COUNTINFO.YARNFOR)
        //    {
        //        case 1:
        //            rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals("1")).Select(e =>
        //            {
        //                if (rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS != null)
        //                    e.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.TOTALENDS * (e.RATIO / sumOfWarpRatio) / (840 * 2.2046 * e.NE)) / 0.97) / crimp_per) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                return e;
        //            }).ToList();
        //            break;
        //        case 2:
        //            switch (rndFabricInfoViewModel.rND_FABRICINFO.LOOM.LOOM_TYPE_NAME.ToLower())
        //            {
        //                case "airjet":
        //                    rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals(2)).Select(e =>
        //                    {
        //                        if (rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != null && rndFabricInfoViewModel.rND_FABRICINFO.GRPPI != null && e.RATIO != null && e.NE != null)
        //                            e.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 3) *
        //                                rndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (e.RATIO / sumOfWeftRatio) / (840 * 2.2046 * e.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                        return e;
        //                    }).ToList();
        //                    break;
        //                case "rapier":
        //                    rndFabricInfoViewModel.Items.Where(e => e.YARNFOR.Equals(2)).Select(e =>
        //                    {
        //                        if (rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE != null && rndFabricInfoViewModel.rND_FABRICINFO.GRPPI != null && e.RATIO != null && e.NE != null)
        //                            e.AMOUNT = Math.Round((double)(((((rndFabricInfoViewModel.rND_FABRICINFO.REED_SPACE + 5) *
        //                                rndFabricInfoViewModel.rND_FABRICINFO.GRPPI * (e.RATIO / sumOfWeftRatio) / (840 * 2.2046 * e.NE)) / 0.97) / ((100 - shrinkage) / 100)) / 0.95), 4);
        //                        return e;
        //                    }).ToList();
        //                    break;
        //            }
        //            break;
        //    }
        //    return rndFabricInfoViewModel;
        //}

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsRndFabricInfoFabCodeInUse(RndFabricInfoViewModel rs)
        {
            try
            {
                var isExistFabCode = await _rNdFabricinfo.FindByRndFabricInfoFabCodeByAsync(rs.rND_FABRICINFO.FABCODE);
                return isExistFabCode
                    ? Json($"Fabric code [ {rs.rND_FABRICINFO.FABCODE} ] is already in use.")
                    : Json(true);
            }
            catch (Exception)
            {
                return Json($"Invalid operation");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFabricCountinfoAndYarnConsumption(RndFabricInfoViewModel rndFabricInfoViewModel, int RemoveIndex)
        {
            try
            {
                ModelState.Clear();

                // CHECK POINT
                if (rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex].TRNSID > 0)
                {
                    //if (await _rNdFabricCountinfo.Delete(rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex])
                    //    && await _rNdYarnconsumption.Delete(rndFabricInfoViewModel.RndYarnConsumptions[RemoveIndex]))
                    //{
                    if (await _rNdFabricCountinfo.Delete(rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex]))
                    {
                        var yarnConsumption = await _rNdYarnconsumption.GetPrimaryKeyByCountIdAndFabCodeAsync(
                            rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex].COUNTID ?? 0,
                            rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex].FABCODE,
                            rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex].YARNFOR ?? 0,
                            rndFabricInfoViewModel.RndFabricCountInfos[RemoveIndex].COLORCODE);

                        if (yarnConsumption != null)
                        {
                            await _rNdYarnconsumption.Delete(yarnConsumption);
                        }
                        rndFabricInfoViewModel.RndFabricCountInfos.RemoveAt(RemoveIndex);

                        rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                        rndFabricInfoViewModel = GetCountConsumption(rndFabricInfoViewModel);

                        foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos)
                        {
                            var x = await _rNdYarnconsumption.GetPrimaryKeyByCountIdAndFabCodeAsync(item.COUNTID ?? 0,
                                item.FABCODE, item.YARNFOR ?? 0, item.COLORCODE);
                            x.AMOUNT = item.AMOUNT;
                            rndFabricInfoViewModel.RndYarnConsumptions.Add(x);
                        }

                        await _rNdFabricCountinfo.UpdateRangeByAsync(rndFabricInfoViewModel.RndFabricCountInfos);
                        await _rNdYarnconsumption.UpdateRangeByAsync(rndFabricInfoViewModel.RndYarnConsumptions);
                    };
                    rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                    return PartialView($"FEAddFabricCountinfoTable", GetCountConsumption(rndFabricInfoViewModel));
                }

                rndFabricInfoViewModel.RndFabricCountInfos.RemoveAt(RemoveIndex);
                rndFabricInfoViewModel.RndYarnConsumptions.RemoveAt(RemoveIndex);
                rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                return PartialView($"FEAddFabricCountinfoTable", GetCountConsumption(rndFabricInfoViewModel));
            }
            catch (Exception ex)
            {
                return Json($"An error occurred during your submission.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCrimpPercentage(RND_FABRICINFO rndFabricinfo)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var fabInfo = await _rNdFabricinfo.GetFabInfoWithCount(rndFabricinfo.FABCODE);

                if (fabInfo == null || rndFabricinfo.CRIMP_PERCENTAGE == null)
                {
                    TempData["message"] = "Failed to Update Fabric Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
                }

                fabInfo.CRIMP_PERCENTAGE = rndFabricinfo.CRIMP_PERCENTAGE;

                var isUpdate = await _rNdFabricinfo.Update(fabInfo);

                if (isUpdate)
                {
                    var shrinkage = Math.Round((double)(((fabInfo.FNPPI - fabInfo.GRPPI) / fabInfo.FNPPI) * 100), 8);

                    var sumOfWarpRatio = fabInfo.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR == 1).Aggregate(0.0, (current, item) => current + Convert.ToInt32(item.RATIO));

                    var crimpPercentage = (100 - rndFabricinfo.CRIMP_PERCENTAGE) / 100;

                    foreach (var item in fabInfo.RND_YARNCONSUMPTION.Where(c => c.YARNFOR.Equals(1)))
                    {
                        var rndCount = fabInfo.RND_FABRIC_COUNTINFO
                            .FirstOrDefault(c => c.COUNTID.Equals(item.COUNTID) && c.FABCODE.Equals(item.FABCODE) && c.YARNFOR.Equals(item.YARNFOR));

                        if (fabInfo.TOTALENDS != null && rndCount != null)
                        {
                            item.OLD_CONSUMPTION = item.AMOUNT;

                            item.AMOUNT = Math.Round((double)((((fabInfo.TOTALENDS * (rndCount.RATIO / sumOfWarpRatio) / (840 * 2.2046 * rndCount.NE)) / crimpPercentage) / ((100 - shrinkage) / 100)) / 0.97), 8);
                            if (item.AMOUNT != null)
                            {
                                await _rNdYarnconsumption.Update(item);
                            }
                        }
                    }

                    TempData["message"] = "Successfully Updated Fabric Info.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
                }
                TempData["message"] = "Failed to Update Fabric Info.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CopyFabricInfo(RND_FABRICINFO rndFabricinfo)
        {
            try
            {
                var fabInfo = await _rNdFabricinfo.GetFabInfoWithCount(rndFabricinfo.FABCODE);

                if (fabInfo == null)
                {
                    TempData["message"] = "Fabric details not found.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
                }

                var style = new RND_FABRICINFO
                {
                    DEVID = fabInfo.DEVID,
                    STYLE_NAME = rndFabricinfo.STYLE_NAME,
                    WVID = fabInfo.WVID,
                    SFINID = fabInfo.SFINID,
                    PROGNO = fabInfo.PROGNO,
                    DID = fabInfo.DID,
                    DYCODE = fabInfo.DYCODE,
                    REED_COUNT = fabInfo.REED_COUNT,
                    REED_SPACE = fabInfo.REED_SPACE,
                    DENT = fabInfo.DENT,
                    COLORCODE = fabInfo.COLORCODE,
                    LOOMID = fabInfo.LOOMID,
                    LOOMSPEED = fabInfo.LOOMSPEED,
                    EFFICIENCY = fabInfo.EFFICIENCY,
                    WID = fabInfo.WID,
                    DOBBY = fabInfo.DOBBY,
                    TOTALENDS = fabInfo.TOTALENDS,
                    TOTALWEFT = fabInfo.TOTALWEFT,
                    TOTALROPE = fabInfo.TOTALROPE,
                    PICKLENGHT = fabInfo.PICKLENGHT,
                    FABRICTYPE = fabInfo.FABRICTYPE,
                    COMPOSITION = fabInfo.COMPOSITION,
                    COMPOSITION_PRO = fabInfo.COMPOSITION_PRO,
                    FINISH_ROUTE = fabInfo.FINISH_ROUTE,
                    CONCEPT = fabInfo.CONCEPT,
                    FINID = fabInfo.FINID,
                    MCID = fabInfo.MCID,
                    MEETING = fabInfo.MEETING,
                    BUYERID = fabInfo.BUYERID,
                    MASTERROLL = fabInfo.MASTERROLL,
                    BUYERREF = fabInfo.BUYERREF,
                    REVISENO = fabInfo.REVISENO,
                    LSBTESTNO = fabInfo.LSBTESTNO,
                    LTSID = fabInfo.LTSID,
                    GREPI = fabInfo.GREPI,
                    GRPPI = fabInfo.GRPPI,
                    FNEPI = fabInfo.FNEPI,
                    FNPPI = fabInfo.FNPPI,
                    AWEPI = fabInfo.AWEPI,
                    AWPPI = fabInfo.AWPPI,
                    WGGRBW = fabInfo.WGGRBW,
                    WGGRAW = fabInfo.WGGRAW,
                    WGFNBW = fabInfo.WGFNBW,
                    WGFNAW = fabInfo.WGFNAW,
                    WGDEC = fabInfo.WGDEC,
                    WIGRBW = fabInfo.WIGRBW,
                    WIGRAW = fabInfo.WIGRAW,
                    WIFNBW = fabInfo.WIFNBW,
                    WIFNCUT = fabInfo.WIFNCUT,
                    WIFNAW = fabInfo.WIFNAW,
                    WIDEC = fabInfo.WIDEC,
                    SRGRWARP = fabInfo.SRGRWARP,
                    SRGRWEFT = fabInfo.SRGRWEFT,
                    SRFNWARP = fabInfo.SRFNWARP,
                    SRFNWEFT = fabInfo.SRFNWEFT,
                    SRDECWARP = fabInfo.SRDECWARP,
                    SRDECWEFT = fabInfo.SRDECWEFT,
                    STGRWARP = fabInfo.STGRWARP,
                    STGRWEFT = fabInfo.STGRWEFT,
                    STFNWARP = fabInfo.STFNWARP,
                    STFNWEFT = fabInfo.STFNWEFT,
                    STDECWARP = fabInfo.STDECWARP,
                    STDECWEFT = fabInfo.STDECWEFT,
                    GRFNWARP = fabInfo.GRFNWARP,
                    GRFNWEFT = fabInfo.GRFNWEFT,
                    GRDECWARP = fabInfo.GRDECWARP,
                    GRDECWEFT = fabInfo.GRDECWEFT,
                    CRIMP_PERCENTAGE = fabInfo.CRIMP_PERCENTAGE,
                    SKEWMOVE = fabInfo.SKEWMOVE,
                    SLPWARP = fabInfo.SLPWARP,
                    SLPWEFT = fabInfo.SLPWEFT,
                    TNWARPFN = fabInfo.TNWARPFN,
                    TNWEFTFN = fabInfo.TNWEFTFN,
                    TRWARP = fabInfo.TRWARP,
                    TRWEFT = fabInfo.TRWEFT,
                    CFATDRY = fabInfo.CFATDRY,
                    CFATWET = fabInfo.CFATWET,
                    PH = fabInfo.PH,
                    REMARKS = fabInfo.REMARKS,
                    USRID = fabInfo.USRID,
                    LSGTESTNO = fabInfo.LSGTESTNO,
                    ARCHIVE_NO = fabInfo.ARCHIVE_NO,
                    PROTOCOL_NO = fabInfo.PROTOCOL_NO,
                    COMPOSITION_DEC = fabInfo.COMPOSITION_DEC,
                    TOTAL_ENDS_SAMPLE = fabInfo.TOTAL_ENDS_SAMPLE,
                    BWEPI = fabInfo.BWEPI,
                    BWPPI = fabInfo.BWPPI,
                    APPROVED = fabInfo.APPROVED,
                    IS_DELETE = fabInfo.IS_DELETE,
                    METHOD = fabInfo.METHOD,
                    SDRF = fabInfo.SDRF,
                    RS = fabInfo.RS,
                    UPD_BY = fabInfo.UPD_BY,
                    CONTRACTION = fabInfo.CONTRACTION,
                    ENDS = fabInfo.ENDS,
                    OPT3 = fabInfo.OPT3,
                    OPT2 = fabInfo.OPT2,
                    OPT1 = fabInfo.OPT1,
                    FINISH_METHOD = fabInfo.FINISH_METHOD,
                    PROTOCOL_METHOD = fabInfo.PROTOCOL_METHOD,
                    SPR_A_FIN = fabInfo.SPR_A_FIN,
                    SPR_B_FIN = fabInfo.SPR_B_FIN,
                    SPR_A_DEC = fabInfo.SPR_A_DEC,
                    SPR_B_DEC = fabInfo.SPR_B_DEC,
                    SKEW_FN = fabInfo.SKEW_FN,
                    TNWARP = fabInfo.TNWARP,
                    TNWEFT = fabInfo.TNWEFT,
                    SLPWARPFN = fabInfo.SLPWARPFN,
                    SLPWEFTFN = fabInfo.SLPWEFTFN,
                    RUBDRYFN = fabInfo.RUBDRYFN,
                    RUBDRYDEC = fabInfo.RUBDRYDEC,
                    RUBWETFN = fabInfo.RUBWETFN,
                    PROBWEPI = fabInfo.PROBWEPI,
                    PROBWPPI = fabInfo.PROBWPPI,
                    WGBWPRO = fabInfo.WGBWPRO,
                    WGAWPRO = fabInfo.WGAWPRO,
                    WIBWPRO = fabInfo.WIBWPRO,
                    WICUTPRO = fabInfo.WICUTPRO,
                    WIAWPRO = fabInfo.WIAWPRO,
                    SRWARPPRO = fabInfo.SRWARPPRO,
                    SRWEFTPRO = fabInfo.SRWEFTPRO,
                    STWARPPRO = fabInfo.STWARPPRO,
                    STWEFTPRO = fabInfo.STWEFTPRO,
                    GRWARPPRO = fabInfo.GRWARPPRO,
                    GRWEFTPRO = fabInfo.GRWEFTPRO,
                    SPR_A_PRO = fabInfo.SPR_A_PRO,
                    SPR_B_PRO = fabInfo.SPR_B_PRO,
                    SKEW_PRO = fabInfo.SKEW_PRO,
                    TNWARPPRO = fabInfo.TNWARPPRO,
                    TNWEFTPRO = fabInfo.TNWEFTPRO,
                    TEARWARPPRO = fabInfo.TEARWARPPRO,
                    TEARWEFTPRO = fabInfo.TEARWEFTPRO,
                    SLIWARPPRO = fabInfo.SLIWARPPRO,
                    SLIWEFTPRO = fabInfo.SLIWEFTPRO,
                    RUBDRYPRO = fabInfo.RUBDRYPRO,
                    RUBWET = fabInfo.RUBWET,
                    RUBWETPRO = fabInfo.RUBWETPRO,
                    PHPRO = fabInfo.PHPRO,
                    PHFN = fabInfo.PHFN,
                    COMPOSITIONPRO = fabInfo.COMPOSITIONPRO,
                    PROAWEPI = fabInfo.PROAWEPI,
                    PROAWPPI = fabInfo.PROAWPPI,
                    TRWARPFN = fabInfo.TRWARPFN,
                    TRWEFTFN = fabInfo.TRWEFTFN
                };

                var fabDetails = await _rNdFabricinfo.GetInsertedObjByAsync(style);

                if (fabDetails != null)
                {

                    foreach (var item in fabInfo.RND_FABRIC_COUNTINFO)
                    {
                        var count = new RND_FABRIC_COUNTINFO
                        {
                            FABCODE = fabDetails.FABCODE,
                            COUNTID = item.COUNTID,
                            YARNTYPE = item.YARNTYPE,
                            DESCRIPTION = item.DESCRIPTION,
                            COLORCODE = item.COLORCODE,
                            LOTID = item.LOTID,
                            SUPPID = item.SUPPID,
                            RATIO = item.RATIO,
                            NE = item.NE,
                            YARNFOR = item.YARNFOR,
                            AMOUNT = item.AMOUNT
                        };
                        await _rNdFabricCountinfo.InsertByAsync(count);
                    }
                    foreach (var i in fabInfo.RND_YARNCONSUMPTION)
                    {

                        var consump = new RND_YARNCONSUMPTION
                        {
                            FABCODE = fabDetails.FABCODE,
                            COUNTID = i.COUNTID,
                            COLOR = i.COLOR,
                            YARNFOR = i.YARNFOR,
                            AMOUNT = i.AMOUNT,
                            OLD_CONSUMPTION = i.OLD_CONSUMPTION
                        };

                        await _rNdYarnconsumption.InsertByAsync(consump);
                    }
                    TempData["message"] = "Successfully Copied Fabric Info.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
                }
                TempData["message"] = "Fabric Copy Failed";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");
            }
        }



        [HttpPost]
        public async Task<IActionResult> UpdateConsumptionData(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var fabInfo = await _rNdFabricinfo.GetFabInfoWithCount(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE);

                if (fabInfo == null)
                {
                    Request.Headers["Status"] = "Failed";
                    rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                    return PartialView($"FEAddFabricCountinfoTable", rndFabricInfoViewModel);
                }

                var shrinkage = Math.Round((double)(((fabInfo.FNPPI - fabInfo.GRPPI) / fabInfo.FNPPI) * 100), 8);
                var shrinkagePercentage = (100 - shrinkage) / 100;

                var sumOfWarpRatio = fabInfo.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR == 1).Aggregate(0.0, (current, item) => current + Convert.ToInt32(item.RATIO));
                var sumOfWeftRatio = fabInfo.RND_FABRIC_COUNTINFO.Where(e => e.YARNFOR == 2).Aggregate(0.0, (current, item) => current + Convert.ToInt32(item.RATIO));

                var crimp = rndFabricInfoViewModel.rND_FABRICINFO.CRIMP_PERCENTAGE ?? 12;

                var crimpPercentage = (100 - crimp) / 100;

                foreach (var item in fabInfo.RND_YARNCONSUMPTION.Where(c => c.YARNFOR.Equals(1)))
                {
                    var rndCount = fabInfo.RND_FABRIC_COUNTINFO
                        .FirstOrDefault(c => c.COUNTID.Equals(item.COUNTID) && c.FABCODE.Equals(item.FABCODE) && c.YARNFOR.Equals(item.YARNFOR));

                    if (fabInfo.TOTALENDS != null && rndCount != null)
                    {
                        item.OLD_CONSUMPTION = item.AMOUNT;

                        item.AMOUNT = Math.Round((double)((((fabInfo.TOTALENDS * (rndCount.RATIO / sumOfWarpRatio) / (840 * 2.2046 * rndCount.NE)) / crimpPercentage) / shrinkagePercentage) / 0.97), 8);
                        if (item.AMOUNT != null)
                        {
                            await _rNdYarnconsumption.Update(item);
                        }
                    }
                }

                foreach (var item in fabInfo.RND_YARNCONSUMPTION.Where(c => c.YARNFOR.Equals(2)))
                {
                    var rndCount = fabInfo.RND_FABRIC_COUNTINFO
                        .FirstOrDefault(c => c.COUNTID.Equals(item.COUNTID) && c.FABCODE.Equals(item.FABCODE) && c.YARNFOR.Equals(item.YARNFOR));
                    if (rndCount == null) continue;

                    item.OLD_CONSUMPTION = item.AMOUNT;
                    item.AMOUNT = Math.Round(((double)(((fabInfo.REED_SPACE + (fabInfo.LOOMID == 1 ? 3 : 6)) * fabInfo.GRPPI *
                        (rndCount.RATIO / sumOfWeftRatio) / (840 * 2.2046 * rndCount.NE)) / shrinkagePercentage) / 0.97), 8);
                    if (item.AMOUNT != null)
                    {
                        await _rNdYarnconsumption.Update(item);
                    }
                }

                Request.Headers["Status"] = "Success";
                rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                return PartialView($"FEAddFabricCountinfoTable", rndFabricInfoViewModel);
                //TempData["message"] = "Successfully Updated Fabric Info.";
                //TempData["type"] = "success";
                //return RedirectToAction("GetRndFabricInfo", $"RndFabricInfo");

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Request.Headers["Status"] = "Failed";
                rndFabricInfoViewModel = await GetCountDetailsInfo(rndFabricInfoViewModel);
                return PartialView($"FEAddFabricCountinfoTable", rndFabricInfoViewModel);
            }
        }



    }
}