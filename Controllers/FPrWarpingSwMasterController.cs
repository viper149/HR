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
    public class FPrWarpingProcessSwMasterController : Controller
        {
            private readonly IDataProtector _protector;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IF_PR_WARPING_PROCESS_SW_MASTER _fPrWarpingProcessSwMaster;
            private readonly IF_PR_WARPING_PROCESS_SW_DETAILS _fPrWarpingProcessSwDetails;
            private readonly IF_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS _fPrWarpingProcessSwYarnConsumDetails;
            private readonly IPL_PRODUCTION_SETDISTRIBUTION _productionSetdistribution;
            private readonly IF_PR_WARPING_MACHINE _fPrWarpingMachine;
            private readonly IRND_FABRIC_COUNTINFO _rndFabricCountinfo;


            public FPrWarpingProcessSwMasterController(IDataProtectionProvider dataProtectionProvider,
                DataProtectionPurposeStrings dataProtectionPurposeStrings,
                UserManager<ApplicationUser> userManager,
                IF_PR_WARPING_PROCESS_SW_MASTER fPrWarpingProcessSwMaster,
                IF_PR_WARPING_PROCESS_SW_DETAILS fPrWarpingProcessSwDetails,
                IF_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS fPrWarpingProcessSwYarnConsumDetails,
                IPL_PRODUCTION_SETDISTRIBUTION productionSetdistribution,
                IF_PR_WARPING_MACHINE fPrWarpingMachine,
                IRND_FABRIC_COUNTINFO rndFabricCountinfo


            )
            {
                _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
                _userManager = userManager;
                _fPrWarpingProcessSwMaster = fPrWarpingProcessSwMaster;
                _fPrWarpingProcessSwDetails = fPrWarpingProcessSwDetails;
                _fPrWarpingProcessSwYarnConsumDetails = fPrWarpingProcessSwYarnConsumDetails;
                _productionSetdistribution = productionSetdistribution;
                _fPrWarpingMachine = fPrWarpingMachine;
                _rndFabricCountinfo = rndFabricCountinfo;
                

            }

            [HttpPost]
            public async Task<JsonResult> GetTableData()
            {
                try
                {
                    var draw = Request.Form["draw"].FirstOrDefault();
                    var start = Request.Form["start"].FirstOrDefault();
                    var length = Request.Form["length"].FirstOrDefault();
                    var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
                    var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                    var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                    var pageSize = length != null ? Convert.ToInt32(length) : 0;
                    var skip = start != null ? Convert.ToInt32(start) : 0;
                    var recordsTotal = 0;

                    var data = await _fPrWarpingProcessSwMaster.GetAllAsync();

                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    {
                        data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        data = data.Where(m =>
                                                    m.BALL_NO != null && m.BALL_NO.ToString().ToUpper().Contains(searchValue)
                                                || (m.WARPLENGTH != null && m.WARPLENGTH.ToString().Contains(searchValue))
                                                || (m.WARPRATIO != null && m.WARPRATIO.ToString().Contains(searchValue))
                                                || (m.DEL_DATE != null && m.DEL_DATE.ToString().Contains(searchValue))
                                                || (m.TIME_START != null && m.TIME_START.ToString().Contains(searchValue))
                                                || (m.TIME_END != null && m.TIME_END.ToString().Contains(searchValue))
                                                || (m.IS_DECLARE != null && m.IS_DECLARE.ToString().Contains(searchValue))
                                                || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                            ).ToList();
                    }
                    //data = data.ToList();
                    var cosStandardConses = data.ToList();
                    recordsTotal = cosStandardConses.Count();
                    var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                    //foreach (var item in finalData)
                    //{
                    //    item.EncryptedId = _protector.Protect(item.FN_PROCESSID.ToString());
                    //}
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                    return Json(jsonData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            [HttpGet]
            public IActionResult GetSwWarpingList()
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

        [HttpGet]
        public async Task<IActionResult> CreateSwWarping()
        {
            return View(await GetInfo(new FPrWarpingProcessSwMasterViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSwWarping(FPrWarpingProcessSwMasterViewModel fPrWarpingProcessSwMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster.CREATED_BY = user.Id;
                    fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster.UPDATED_BY = user.Id;
                    fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster.CREATED_AT = DateTime.Now;
                    fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster.UPDATED_AT = DateTime.Now;
                    var warpDetails = await _fPrWarpingProcessSwMaster.GetInsertedObjByAsync(fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster);

                    if (warpDetails != null)
                    {
                        foreach (var item in fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList)
                        {
                            item.SW_ID = warpDetails.SWID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrWarpingProcessSwDetails.InsertByAsync(item);
                        }
                        foreach (var i in fPrWarpingProcessSwMasterViewModel.FPrWarpingProcessSwYarnConsumDetailsList)
                        {
                            i.SW_ID = warpDetails.SWID;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessSwYarnConsumDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Warping Process(Sectional) Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetSwWarpingList", $"FPrWarpingProcessSwMasterController");
                    }

                    TempData["message"] = "Failed to Create Warping Process(Sectional).";
                    TempData["type"] = "error";
                    return View(await GetInfo(fPrWarpingProcessSwMasterViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrWarpingProcessSwMasterViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Warping Process(Sectional).";
                TempData["type"] = "error";
                return View(await GetInfo(fPrWarpingProcessSwMasterViewModel));
            }
        }

        public async Task<FPrWarpingProcessSwMasterViewModel> GetInfo(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel)
        {
            prWarpingProcessSwMasterViewModel = await _fPrWarpingProcessSwMaster.GetInitObjects(prWarpingProcessSwMasterViewModel);
            prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster = new F_PR_WARPING_PROCESS_SW_MASTER()
            {
                TIME_START = DateTime.Now,
                //PRODUCTION_DATE = DateTime.Now
            };
            return prWarpingProcessSwMasterViewModel;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountList(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterView)
        {
            try
            {
                var flag = prWarpingProcessSwMasterView.FPrWarpingProcessSwYarnConsumDetailsList.Where(c => c.COUNT_ID.Equals(prWarpingProcessSwMasterView.FPrWarpingProcessSwYarnConsumDetails.COUNT_ID));

                if (!flag.Any())
                {
                    prWarpingProcessSwMasterView.FPrWarpingProcessSwYarnConsumDetailsList.Add(prWarpingProcessSwMasterView.FPrWarpingProcessSwYarnConsumDetails);
                }
                prWarpingProcessSwMasterView = await GetCountNameAsync(prWarpingProcessSwMasterView);
                return PartialView($"AddCountList", prWarpingProcessSwMasterView);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSwMasterView = await GetCountNameAsync(prWarpingProcessSwMasterView);
                return PartialView($"AddCountList", prWarpingProcessSwMasterView);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCountFromList(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwYarnConsumDetailsList[int.Parse(removeIndexValue)].CONSM_ID != 0)
                {
                    await _fPrWarpingProcessSwYarnConsumDetails.Delete(prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwYarnConsumDetailsList[int.Parse(removeIndexValue)]);
                }

                prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwYarnConsumDetailsList.RemoveAt(int.Parse(removeIndexValue));
                prWarpingProcessSwMasterViewModel = await GetCountNameAsync(prWarpingProcessSwMasterViewModel);
                return PartialView($"AddCountList", prWarpingProcessSwMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSwMasterViewModel = await GetCountNameAsync(prWarpingProcessSwMasterViewModel);
                return PartialView($"AddCountList", prWarpingProcessSwMasterViewModel);
            }
        }

        public async Task<FPrWarpingProcessSwMasterViewModel> GetCountNameAsync(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel)
        {
            prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwYarnConsumDetailsList = (List<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS>)await _fPrWarpingProcessSwYarnConsumDetails.GetInitCountData(prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwYarnConsumDetailsList);
            return prWarpingProcessSwMasterViewModel;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBallNoList(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel)
        {
            try
            {
                var flag = prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList.Where(c => c.BALL_NO.Equals(prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetails.BALL_NO));

                if (!flag.Any())
                {
                    prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList.Add(prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetails);
                }
                prWarpingProcessSwMasterViewModel = await GetBallAsync(prWarpingProcessSwMasterViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSwMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSwMasterViewModel = await GetBallAsync(prWarpingProcessSwMasterViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSwMasterViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBallNoList(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList[int.Parse(removeIndexValue)].SW_D_ID != 0)
                {
                    await _fPrWarpingProcessSwDetails.Delete(prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList[int.Parse(removeIndexValue)]);
                }
                prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList.RemoveAt(int.Parse(removeIndexValue));
                prWarpingProcessSwMasterViewModel = await GetBallAsync(prWarpingProcessSwMasterViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSwMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSwMasterViewModel = await GetBallAsync(prWarpingProcessSwMasterViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSwMasterViewModel);
            }
        }

        public async Task<FPrWarpingProcessSwMasterViewModel> GetBallAsync(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel)
        {
            prWarpingProcessSwMasterViewModel = await _fPrWarpingProcessSwDetails.GetInitSoData(prWarpingProcessSwMasterViewModel);
            var ballCount = prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwDetailsList.Count();
            Response.Headers["BallCount"] = ballCount.ToString();
            return prWarpingProcessSwMasterViewModel;
        }

    }
}

