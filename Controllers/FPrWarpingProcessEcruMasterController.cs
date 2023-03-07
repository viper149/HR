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
    public class FPrWarpingProcessEcruMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_WARPING_PROCESS_ECRU_MASTER _fPrWarpingProcessEcruMaster;
        private readonly IF_PR_WARPING_PROCESS_ECRU_DETAILS _fPrWarpingProcessEcruDetails;
        private readonly IF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS _fPrWarpingProcessEcruYarnConsumDetails;
        private readonly IPL_PRODUCTION_SETDISTRIBUTION _productionSetdistribution;
        private readonly IF_PR_WARPING_MACHINE _fPrWarpingMachine;
        private readonly IRND_FABRIC_COUNTINFO _rndFabricCountinfo;

        public FPrWarpingProcessEcruMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_WARPING_PROCESS_ECRU_MASTER fPrWarpingProcessEcruMaster,
            IF_PR_WARPING_PROCESS_ECRU_DETAILS fPrWarpingProcessEcruDetails,
            IF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS fPrWarpingProcessEcruYarnConsumDetails,
            IPL_PRODUCTION_SETDISTRIBUTION productionSetdistribution,
            IF_PR_WARPING_MACHINE fPrWarpingMachine,
            IRND_FABRIC_COUNTINFO rndFabricCountinfo


        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrWarpingProcessEcruMaster = fPrWarpingProcessEcruMaster;
            _fPrWarpingProcessEcruDetails = fPrWarpingProcessEcruDetails;
            _fPrWarpingProcessEcruYarnConsumDetails = fPrWarpingProcessEcruYarnConsumDetails;
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

                var data = await _fPrWarpingProcessEcruMaster.GetAllAsync();

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
                                                m.BALL_NO != null && m.BALL_NO.ToString().ToUpper().Contains(searchValue)
                                            || (m.WARPLENGTH != null && m.WARPLENGTH.ToString().Contains(searchValue))
                                            || (m.WARPRATIO != null && m.WARPRATIO.ToString().Contains(searchValue))
                                            || (m.SET != null && m.SET.PROG_ != null && m.SET.PROG_.PROG_NO.ToString().Contains(searchValue))
                                            || (m.DEL_DATE != null && m.DEL_DATE.ToString().Contains(searchValue))
                                            || (m.TIME_START != null && m.TIME_START.ToString().Contains(searchValue))
                                            || (m.TIME_END != null && m.TIME_END.ToString().Contains(searchValue))
                                            || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.ECRUID.ToString());
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
        public IActionResult GetEcruWarpingList()
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
        public async Task<IActionResult> CreateFPrWarpingProcessEcruMaster()
        {

            var fPrWarpingProcessEkruMasterViewModel = await GetInfo(new FPrWarpingProcessEkruMasterViewModel());
            fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster = new F_PR_WARPING_PROCESS_ECRU_MASTER()
            {
                TIME_START = DateTime.Now,
                //PRODUCTION_DATE = DateTime.Now
            };
            return View(fPrWarpingProcessEkruMasterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFPrWarpingProcessEcruMaster(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster.CREATED_BY = user.Id;
                    fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster.UPDATED_BY = user.Id;
                    fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster.CREATED_AT = DateTime.Now;
                    fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster.UPDATED_AT = DateTime.Now;
                    //fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster.WARPLENGTH = await _fPrWarpingProcessDwMaster.GetWarpLength(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster.SETID);
                    var fPrWarpingProcessEcruMaster = await _fPrWarpingProcessEcruMaster.GetInsertedObjByAsync(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruMaster);

                    if (fPrWarpingProcessEcruMaster != null)
                    {
                        foreach (var item in fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList)
                        {
                            item.ECRU_ID = fPrWarpingProcessEcruMaster.ECRUID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrWarpingProcessEcruDetails.InsertByAsync(item);
                        }
                        foreach (var i in fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList)
                        {
                            i.ECRU_ID = fPrWarpingProcessEcruMaster.ECRUID;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessEcruYarnConsumDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Warping Process(Ecru) Created.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetEcruWarpingList), $"FPrWarpingProcessEcruMaster");
                    }

                    TempData["message"] = "Failed to Create Warping Process(Ecru).";
                    TempData["type"] = "error";
                    return View(await GetInfo(fPrWarpingProcessEkruMasterViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrWarpingProcessEkruMasterViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Warping Process(Ecru).";
                TempData["type"] = "error";
                return View(await GetInfo(fPrWarpingProcessEkruMasterViewModel));
            }
        }

        public async Task<FPrWarpingProcessEkruMasterViewModel> GetInfo(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel,bool e=false)
        {
            fPrWarpingProcessEkruMasterViewModel = await _fPrWarpingProcessEcruMaster.GetInitObjects(fPrWarpingProcessEkruMasterViewModel,e);
            return fPrWarpingProcessEkruMasterViewModel;
        }
        

        [HttpGet]
        public async Task<IActionResult> EditFPrWarpingProcessEcruMaster(string id)
        {
            try
            {
                var sId = int.Parse(_protector.Unprotect(id));
                var prWarpingProcessEcruViewModel = await _fPrWarpingProcessEcruMaster.FindAllByIdAsync(sId);

                if (prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster != null)
                {
                    prWarpingProcessEcruViewModel = await GetInfo(prWarpingProcessEcruViewModel,true);
                    prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.EncryptedId = _protector.Protect(prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.ECRUID.ToString());

                    prWarpingProcessEcruViewModel = await GetCountNameAsync(prWarpingProcessEcruViewModel);
                    prWarpingProcessEcruViewModel = await GetBallAsync(prWarpingProcessEcruViewModel);
                    return View(prWarpingProcessEcruViewModel);
                }

                TempData["message"] = "Ecru Warping Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetEcruWarpingList", $"FPrWarpingProcessEcruMaster");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetEcruWarpingList", $"FPrWarpingProcessEcruMaster");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFPrWarpingProcessEcruMaster(FPrWarpingProcessEkruMasterViewModel prWarpingProcessEcruViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var eId = int.Parse(_protector.Unprotect(prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.EncryptedId));
                    if (prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.ECRUID == eId)
                    {
                        var slasherDetails = await _fPrWarpingProcessEcruMaster.FindByIdAsync(eId);

                        var user = await _userManager.GetUserAsync(User);
                        prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.UPDATED_BY = user.Id;
                        prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.UPDATED_AT = DateTime.Now;
                        prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.CREATED_AT = slasherDetails.CREATED_AT;
                        prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster.CREATED_BY = slasherDetails.CREATED_BY;

                        var isUpdated = await _fPrWarpingProcessEcruMaster.Update(prWarpingProcessEcruViewModel.FPrWarpingProcessEcruMaster);
                        if (isUpdated)
                        {
                            foreach (var item in prWarpingProcessEcruViewModel.FPrWarpingProcessEcruDetailsList.Where(c => c.ECRU_D_ID == 0))
                            {
                                item.ECRU_ID = slasherDetails.ECRUID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrWarpingProcessEcruDetails.InsertByAsync(item);
                            }
                            foreach (var i in prWarpingProcessEcruViewModel.FPrWarpingProcessEcruYarnConsumDetailsList.Where(c => c.CONSM_ID == 0))
                            {
                                i.ECRU_ID = slasherDetails.ECRUID;
                                i.CREATED_AT = DateTime.Now;
                                i.CREATED_BY = user.Id;
                                i.UPDATED_AT = DateTime.Now;
                                i.UPDATED_BY = user.Id;

                                await _fPrWarpingProcessEcruYarnConsumDetails.InsertByAsync(i);
                            }
                            foreach (var item in prWarpingProcessEcruViewModel.FPrWarpingProcessEcruDetailsList.Where(c => c.ECRU_D_ID > 0))
                            {
                                var ecruD=await _fPrWarpingProcessEcruDetails.FindByIdAsync(item.ECRU_D_ID);
                                item.ECRU_ID = ecruD.ECRU_ID;
                                item.CREATED_AT = ecruD.CREATED_AT;
                                item.CREATED_BY = ecruD.CREATED_BY;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrWarpingProcessEcruDetails.Update(item);
                            }
                            foreach (var i in prWarpingProcessEcruViewModel.FPrWarpingProcessEcruYarnConsumDetailsList.Where(c => c.CONSM_ID > 0))
                            {
                                var ecruC = await _fPrWarpingProcessEcruYarnConsumDetails.FindByIdAsync(i.CONSM_ID);
                                ecruC.UPDATED_AT = DateTime.Now;
                                ecruC.UPDATED_BY = user.Id;
                                ecruC.CONSM = i.CONSM;
                                ecruC.WASTE = i.WASTE;
                                ecruC.WASTE_PERCENTAGE = (i.WASTE*100)/i.CONSM;
                                ecruC.REMARKS = i.REMARKS;

                                await _fPrWarpingProcessEcruYarnConsumDetails.Update(ecruC);
                            }
                            TempData["message"] = "Successfully Updated Warping Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetEcruWarpingList", $"FPrWarpingProcessEcruMaster");
                        }
                        TempData["message"] = "Failed to Update Warping Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetEcruWarpingList", $"FPrWarpingProcessEcruMaster");
                    }
                    TempData["message"] = "Invalid Warping Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetEcruWarpingList", $"FPrWarpingProcessEcruMaster");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                prWarpingProcessEcruViewModel = await GetInfo(prWarpingProcessEcruViewModel, true);
                prWarpingProcessEcruViewModel = await GetCountNameAsync(prWarpingProcessEcruViewModel);
                prWarpingProcessEcruViewModel = await GetBallAsync(prWarpingProcessEcruViewModel);
                return View(prWarpingProcessEcruViewModel);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                prWarpingProcessEcruViewModel = await GetInfo(prWarpingProcessEcruViewModel, true);
                prWarpingProcessEcruViewModel = await GetCountNameAsync(prWarpingProcessEcruViewModel);
                prWarpingProcessEcruViewModel = await GetBallAsync(prWarpingProcessEcruViewModel);
                return View(prWarpingProcessEcruViewModel);
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountList(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel)
        {
            try
            {
                var flag = fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList.Where(c => c.COUNT_ID.Equals(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetails.COUNT_ID));

                if (!flag.Any())
                {
                    fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList.Add(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetails);
                }
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetCountNameAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddCountList", fPrWarpingProcessEkruMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetCountNameAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddCountList", fPrWarpingProcessEkruMasterViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCountFromList(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList[int.Parse(removeIndexValue)].CONSM_ID != 0)
                {
                    await _fPrWarpingProcessEcruYarnConsumDetails.Delete(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList[int.Parse(removeIndexValue)]);
                }

                fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList.RemoveAt(int.Parse(removeIndexValue));
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetCountNameAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddCountList", fPrWarpingProcessEkruMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetCountNameAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddCountList", fPrWarpingProcessEkruMasterViewModel);
            }
        }

        public async Task<FPrWarpingProcessEkruMasterViewModel> GetCountNameAsync(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel)
        {
            fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList = (List<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>)await _fPrWarpingProcessEcruYarnConsumDetails.GetInitCountData(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruYarnConsumDetailsList);
            return fPrWarpingProcessEkruMasterViewModel;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBallNoList(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel)
        {
            try
            {
                var flag = fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList.Where(c => c.BALL_NO.Equals(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetails.BALL_NO));

                if (!flag.Any())
                {
                    fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList.Add(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetails);
                }
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetBallAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddBallNoList", fPrWarpingProcessEkruMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetBallAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddBallNoList", fPrWarpingProcessEkruMasterViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBallNoList(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList[int.Parse(removeIndexValue)].ECRU_D_ID != 0)
                {
                    await _fPrWarpingProcessEcruDetails.Delete(fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList[int.Parse(removeIndexValue)]);
                }
                fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList.RemoveAt(int.Parse(removeIndexValue));
                fPrWarpingProcessEkruMasterViewModel = await GetInfo(fPrWarpingProcessEkruMasterViewModel);
                fPrWarpingProcessEkruMasterViewModel = await GetBallAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddBallNoList", fPrWarpingProcessEkruMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrWarpingProcessEkruMasterViewModel = await GetBallAsync(fPrWarpingProcessEkruMasterViewModel);
                return PartialView($"AddBallNoList", fPrWarpingProcessEkruMasterViewModel);
            }
        }

        public async Task<FPrWarpingProcessEkruMasterViewModel> GetBallAsync(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel)
        {
            fPrWarpingProcessEkruMasterViewModel = await _fPrWarpingProcessEcruDetails.GetInitSoData(fPrWarpingProcessEkruMasterViewModel);
            var ballCount = fPrWarpingProcessEkruMasterViewModel.FPrWarpingProcessEcruDetailsList.Count();
            Response.Headers["BallCount"] = ballCount.ToString();
            return fPrWarpingProcessEkruMasterViewModel;
        }

    }
}
