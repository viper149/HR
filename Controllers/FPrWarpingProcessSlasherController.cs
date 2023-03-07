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
    public class FPrWarpingProcessSlasherController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_WARPING_PROCESS_DW_MASTER _fPrWarpingProcessDwMaster;
        private readonly IF_PR_WARPING_PROCESS_DW_DETAILS _fPrWarpingProcessDwDetails;
        private readonly IF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS _fPrWarpingProcessDwYarnConsumDetails;

        public FPrWarpingProcessSlasherController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_WARPING_PROCESS_DW_MASTER fPrWarpingProcessDwMaster,
            IF_PR_WARPING_PROCESS_DW_DETAILS fPrWarpingProcessDwDetails,
            IF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS fPrWarpingProcessDwYarnConsumDetails
        )
        {
            this._protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            this._userManager = userManager;
            _fPrWarpingProcessDwMaster = fPrWarpingProcessDwMaster;
            _fPrWarpingProcessDwDetails = fPrWarpingProcessDwDetails;
            _fPrWarpingProcessDwYarnConsumDetails = fPrWarpingProcessDwYarnConsumDetails;
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
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = await _fPrWarpingProcessDwMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                              (m.SET.PROG_.PROG_NO != null && m.SET.PROG_.PROG_NO.ToUpper().Contains(searchValue))
                                           || (m.TIME_START.ToString().Contains(searchValue))
                                           || (m.TIME_END.ToString().Contains(searchValue))
                                           || (m.BALL_NO != null && m.BALL_NO.ToString().ToUpper().Contains(searchValue))
                                           || (m.WARPRATIO != null && m.WARPRATIO.ToString().ToUpper().Contains(searchValue))
                                           || (m.WARPLENGTH != null && m.WARPLENGTH.ToString().ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.WARPID.ToString());
                }
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
        public IActionResult GetWarpingProcessSlasherList()
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
        public async Task<IActionResult> CreateWarpingProcessSlasher()
        {
            var prWarpingProcessSlasherViewModel = await GetInfo(new PrWarpingProcessSlasherViewModel());
            prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster = new F_PR_WARPING_PROCESS_DW_MASTER()
            {
                TIME_START = DateTime.Now,
                //PRODUCTION_DATE = DateTime.Now
            };
            return View(prWarpingProcessSlasherViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarpingProcessSlasher(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.CREATED_BY = user.Id;
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.UPDATED_BY = user.Id;
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.CREATED_AT = DateTime.Now;
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.UPDATED_AT = DateTime.Now;
                    //prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.WARPLENGTH = await _fPrWarpingProcessDwMaster.GetWarpLength(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.SETID);
                    var warpDetails = await _fPrWarpingProcessDwMaster.GetInsertedObjByAsync(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster);

                    if (warpDetails != null)
                    {
                        foreach (var item in prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList)
                        {
                            item.WARP_ID = warpDetails.WARPID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrWarpingProcessDwDetails.InsertByAsync(item);
                        }
                        foreach (var i in prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList)
                        {
                            i.WARP_ID = warpDetails.WARPID;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessDwYarnConsumDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Warping Process(Slasher) Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetWarpingProcessSlasherList", $"FPrWarpingProcessSlasher");
                    }

                    TempData["message"] = "Failed to Create Warping Process(Slasher).";
                    TempData["type"] = "error";
                    return View(await GetInfo(prWarpingProcessSlasherViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(prWarpingProcessSlasherViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Warping Process(Slasher).";
                TempData["type"] = "error";
                return View(await GetInfo(prWarpingProcessSlasherViewModel));
            }
        }

        public async Task<PrWarpingProcessSlasherViewModel> GetInfo(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            prWarpingProcessSlasherViewModel = await _fPrWarpingProcessDwMaster.GetInitObjects(prWarpingProcessSlasherViewModel);
            return prWarpingProcessSlasherViewModel;
        }
        

        [HttpGet]
        public async Task<IActionResult> EditWarpingProcessSlasher(string id)
        {
            try
            {
                var sId = int.Parse(_protector.Unprotect(id));
                var prWarpingProcessSlasherViewModel = await _fPrWarpingProcessDwMaster.FindAllByIdAsync(sId);

                if (prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster != null)
                {
                    prWarpingProcessSlasherViewModel = await GetInfo(prWarpingProcessSlasherViewModel);
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.EncryptedId = _protector.Protect(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.WARPID.ToString());

                    prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                    prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                    return View(prWarpingProcessSlasherViewModel);
                }

                TempData["message"] = "Direct Warping Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetWarpingProcessSlasherList", $"FPrWarpingProcessSlasher");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetWarpingProcessSlasherList", $"FPrWarpingProcessSlasher");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWarpingProcessSlasher(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sId = int.Parse(_protector.Unprotect(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.EncryptedId));
                    if (prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.WARPID == sId)
                    {
                        var slasherDetails = await _fPrWarpingProcessDwMaster.FindByIdAsync(sId);

                        var user = await _userManager.GetUserAsync(User);
                        prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.UPDATED_BY = user.Id;
                        prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.UPDATED_AT = DateTime.Now;
                        prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.CREATED_AT = slasherDetails.CREATED_AT;
                        prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster.CREATED_BY = slasherDetails.CREATED_BY;

                        var isUpdated = await _fPrWarpingProcessDwMaster.Update(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster);
                        if (isUpdated)
                        {
                            foreach (var item in prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList.Where(c=>c.WARP_D_ID==0))
                            {
                                item.WARP_ID = slasherDetails.WARPID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrWarpingProcessDwDetails.InsertByAsync(item);
                            }
                            foreach (var i in prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList.Where(c=>c.CONSM_ID==0))
                            {
                                i.WARP_ID = slasherDetails.WARPID;
                                i.CREATED_AT = DateTime.Now;
                                i.CREATED_BY = user.Id;
                                i.UPDATED_AT = DateTime.Now;
                                i.UPDATED_BY = user.Id;

                                await _fPrWarpingProcessDwYarnConsumDetails.InsertByAsync(i);
                            }
                            TempData["message"] = "Successfully Updated Warping Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetWarpingProcessSlasherList", $"FPrWarpingProcessSlasher");
                        }
                        TempData["message"] = "Failed to Update Warping Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetWarpingProcessSlasherList", $"FPrWarpingProcessSlasher");
                    }
                    TempData["message"] = "Invalid Warping Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetWarpingProcessSlasherList", $"FPrWarpingProcessSlasher");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                prWarpingProcessSlasherViewModel = await GetInfo(prWarpingProcessSlasherViewModel);
                    prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                    prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                return View(prWarpingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                prWarpingProcessSlasherViewModel = await GetInfo(prWarpingProcessSlasherViewModel);
                prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                return View(prWarpingProcessSlasherViewModel);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountList(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            try
            {
                var flag = prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList.Where(c => c.COUNT_ID.Equals(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetails.COUNT_ID));

                if (!flag.Any())
                {
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList.Add(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetails);
                }
                prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddCountList", prWarpingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddCountList", prWarpingProcessSlasherViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCountFromList(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList[int.Parse(removeIndexValue)].CONSM_ID != 0)
                {
                    await _fPrWarpingProcessDwYarnConsumDetails.Delete(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList[int.Parse(removeIndexValue)]);
                }

                prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList.RemoveAt(int.Parse(removeIndexValue));
                prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddCountList", prWarpingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSlasherViewModel = await GetCountNameAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddCountList", prWarpingProcessSlasherViewModel);
            }
        }

        public async Task<PrWarpingProcessSlasherViewModel> GetCountNameAsync(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList = (List<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>)await _fPrWarpingProcessDwYarnConsumDetails.GetInitCountData(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwYarnConsumDetailsList);
            return prWarpingProcessSlasherViewModel;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBallNoList(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            try
            {
                var flag = prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList.Where(c => c.BALL_NO.Equals(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetails.BALL_NO));

                if (!flag.Any())
                {
                    prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList.Add(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetails);
                }
                prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSlasherViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBallNoList(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList[int.Parse(removeIndexValue)].WARP_D_ID != 0)
                {
                    await _fPrWarpingProcessDwDetails.Delete(prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList[int.Parse(removeIndexValue)]);
                }
                prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList.RemoveAt(int.Parse(removeIndexValue));
                prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessSlasherViewModel = await GetBallAsync(prWarpingProcessSlasherViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessSlasherViewModel);
            }
        }

        public async Task<PrWarpingProcessSlasherViewModel> GetBallAsync(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            prWarpingProcessSlasherViewModel = await _fPrWarpingProcessDwDetails.GetInitSoData(prWarpingProcessSlasherViewModel);
            var ballCount = prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList.Count();
            Response.Headers["BallCount"] = ballCount.ToString();
            return prWarpingProcessSlasherViewModel;
        }


        [HttpGet]
        public IActionResult RWarpingDeliveryReport(string setNO)
        {
            return View(model: setNO);
        }

    }
}
