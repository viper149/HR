using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FPrWarpingProcessRopeMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_WARPING_PROCESS_ROPE_MASTER _fPrWarpingProcessRopeMaster;
        private readonly IF_PR_WARPING_PROCESS_ROPE_DETAILS _fPrWarpingProcessRopeDetails;
        private readonly IF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS _fPrWarpingProcessRopeBallDetails;
        private readonly IF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS _fPrWarpingProcessRopeYarnConsumDetails;
        private readonly IPL_PRODUCTION_PLAN_DETAILS _productionPlanDetails;

        public FPrWarpingProcessRopeMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_WARPING_PROCESS_ROPE_MASTER fPrWarpingProcessRopeMaster,
            IF_PR_WARPING_PROCESS_ROPE_DETAILS fPrWarpingProcessRopeDetails,
            IF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS fPrWarpingProcessRopeBallDetails,
            IF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS fPrWarpingProcessRopeYarnConsumDetails,
        IPL_PRODUCTION_PLAN_DETAILS productionPlanDetails
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrWarpingProcessRopeMaster = fPrWarpingProcessRopeMaster;
            _fPrWarpingProcessRopeDetails = fPrWarpingProcessRopeDetails;
            _fPrWarpingProcessRopeBallDetails = fPrWarpingProcessRopeBallDetails;
            _fPrWarpingProcessRopeYarnConsumDetails = fPrWarpingProcessRopeYarnConsumDetails;
            _productionPlanDetails = productionPlanDetails;
        }

        [HttpPost]
        [Route("ProductionRopeWarping/GetTableData")]
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
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var data = (List<F_PR_WARPING_PROCESS_ROPE_MASTER>)await _fPrWarpingProcessRopeMaster.GetAllWithNameAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                              (m.SUBGROUP.SUBGROUPNO != 0 && m.SUBGROUP.SUBGROUPNO.ToString().Contains(searchValue))
                                           || (m.TIME_START.ToString().Contains(searchValue))
                                           || (m.TIME_END.ToString().Contains(searchValue))
                                           || (m.DELIVERY_DATE != null && m.DELIVERY_DATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.DELIVERY_DATE != null && m.DELIVERY_DATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue))
                                           || (m.OPT2 != null && m.OPT2.ToUpper().Contains(searchValue))
                                           || (m.OPT4 != null && m.OPT4.ToUpper().Contains(searchValue))
                                           || (m.OPT3 != null && m.OPT3.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.WARPID.ToString());
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = finalData
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        [HttpGet]
        [Route("ProductionRopeWarping")]
        [Route("ProductionRopeWarping/GetAll")]
        public IActionResult GetWarpingProcessRopeList()
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
        public async Task<IActionResult> CreateWarpingProcessRope()
        {

            var prWarpingProcessRopeViewModel = new PrWarpingProcessRopeViewModel();
            prWarpingProcessRopeViewModel = await GetInfo(prWarpingProcessRopeViewModel);
            prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster = new F_PR_WARPING_PROCESS_ROPE_MASTER
            {
                TIME_START = DateTime.Now,
                //PRODUCTION_DATE = DateTime.Now
            };
            return View(prWarpingProcessRopeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarpingProcessRope(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.CREATED_BY = user.Id;
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.UPDATED_BY = user.Id;
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.CREATED_AT = DateTime.Now;
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.UPDATED_AT = DateTime.Now;
                    //prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.WARP_LENGTH = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList.Select(c => c.WARP_LENGTH_PER_SET).FirstOrDefault();
                    var warpId = await _fPrWarpingProcessRopeMaster.InsertAndGetIdAsync(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster);

                    if (warpId != 0)
                    {
                        var setList = await _fPrWarpingProcessRopeDetails.GetSetList((int)prWarpingProcessRopeViewModel
                            .FPrWarpingProcessRopeMaster.SUBGROUPID);
                        var totalSetLength = setList.PL_PRODUCTION_SETDISTRIBUTION.Sum(c => c.PROG_.SET_QTY);
                        foreach (var i in setList.PL_PRODUCTION_SETDISTRIBUTION)
                        {
                            var setQty = (i.PROG_.SET_QTY * prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster
                                    .WARP_LENGTH) / totalSetLength;
                            var item = new F_PR_WARPING_PROCESS_ROPE_DETAILS();
                            item.SETID = i.SETID;
                            item.WARP_LENGTH_PER_SET = (double?)Math.Round((decimal)setQty);
                            item.BALL_NO = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetails.BALL_NO;
                            item.WARPID = warpId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var warpProgId = await _fPrWarpingProcessRopeDetails.InsertAndGetIdAsync(item);
                        }

                        foreach (var i in prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList)
                        {
                            i.WARP_PROG = null;
                            i.BALL_ID_FKNavigation = null;
                            i.COUNT_ = null;
                            i.BALL_ID_FK_Link = null;
                            i.WARPID = warpId;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessRopeBallDetails.InsertByAsync(i);
                        }
                        foreach (var i in prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList)
                        {
                            i.WARPID = warpId;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessRopeYarnConsumDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Warping Process(Rope) Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetWarpingProcessRopeList", $"FPrWarpingProcessRopeMaster");
                    }

                    TempData["message"] = "Failed to Create Warping Process(Rope).";
                    TempData["type"] = "error";
                    return View(await GetInfo(prWarpingProcessRopeViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(prWarpingProcessRopeViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Warping Process(Rope).";
                TempData["type"] = "error";
                return View(await GetInfo(prWarpingProcessRopeViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditWarpingProcessRope(string id)
        {
            try
            {
                var prWarpingProcessRopeViewModel = await _fPrWarpingProcessRopeMaster.FindAllByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster != null)
                {
                    prWarpingProcessRopeViewModel = await GetCountNameAsync(prWarpingProcessRopeViewModel);

                    prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);

                    prWarpingProcessRopeViewModel = await GetInfo(prWarpingProcessRopeViewModel);
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.EncryptedId = _protector.Protect(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.WARPID.ToString());

                    return View(prWarpingProcessRopeViewModel);
                }

                TempData["message"] = "Warping Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetWarpingProcessRopeList", $"FPrWarpingProcessRopeMaster");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return RedirectToAction("GetWarpingProcessRopeList", $"FPrWarpingProcessRopeMaster");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWarpingProcessRope(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var warpId = int.Parse(_protector.Unprotect(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.EncryptedId));
                    var exData = await _fPrWarpingProcessRopeMaster.FindByIdAsync(warpId);

                    var user = await _userManager.GetUserAsync(User);
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.CREATED_BY = exData.CREATED_BY;
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.CREATED_AT = exData.CREATED_AT;
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.UPDATED_BY = user.Id;
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.UPDATED_AT = DateTime.Now;
                    var blkProgId = await _fPrWarpingProcessRopeMaster.Update(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster);

                    if (blkProgId)
                    {
                        if (exData.WARP_LENGTH != prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster.WARP_LENGTH)
                        {
                            var warpSetList = await _fPrWarpingProcessRopeDetails.GetWarpSetList(warpId);
                            var totalSetLength = warpSetList.Sum(c => c.PL_PRODUCTION_SETDISTRIBUTION.PROG_.SET_QTY);
                            foreach (var item in warpSetList)
                            {
                                var setQty = (item.PL_PRODUCTION_SETDISTRIBUTION.PROG_.SET_QTY * prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster
                                    .WARP_LENGTH) / totalSetLength;

                                item.WARP_LENGTH_PER_SET = (double?)Math.Round((decimal)setQty);
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                var warpProgId = await _fPrWarpingProcessRopeDetails.Update(item);
                            }

                        }

                        foreach (var i in prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList.Where(c => c.BALLID == 0))
                        {
                            i.WARP_PROG = null;
                            i.BALL_ID_FKNavigation = null;
                            i.COUNT_ = null;
                            i.BALL_ID_FK_Link = null;
                            i.WARPID = warpId;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessRopeBallDetails.InsertByAsync(i);
                        }

                        foreach (var i in prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList.Where(c => c.CONSM_ID == 0))
                        {
                            i.WARPID = warpId;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fPrWarpingProcessRopeYarnConsumDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Updated Warping Info";
                        TempData["type"] = "success";
                        return RedirectToAction("GetWarpingProcessRopeList", $"FPrWarpingProcessRopeMaster");
                    }
                    TempData["message"] = "Failed to Update Warping data.";
                    TempData["type"] = "error";

                    return View(await GetInfo(prWarpingProcessRopeViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(prWarpingProcessRopeViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Update Warping data.";
                TempData["type"] = "error";
                return View(await GetInfo(prWarpingProcessRopeViewModel));
            }
        }


        public async Task<PrWarpingProcessRopeViewModel> GetInfo(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            prWarpingProcessRopeViewModel = await _fPrWarpingProcessRopeMaster.GetInitObjects(prWarpingProcessRopeViewModel);
            return prWarpingProcessRopeViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountList(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            try
            {
                var flag = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList.Where(c => c.COUNT_ID.Equals(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetails.COUNT_ID));

                if (!flag.Any())
                {
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList.Add(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetails);
                }
                prWarpingProcessRopeViewModel = await GetCountNameAsync(prWarpingProcessRopeViewModel);
                return PartialView($"AddCountList", prWarpingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessRopeViewModel = await GetCountNameAsync(prWarpingProcessRopeViewModel);
                return PartialView($"AddCountList", prWarpingProcessRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCountFromList(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();

                if (prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList[int.Parse(removeIndexValue)].CONSM_ID != 0)
                {
                    await _fPrWarpingProcessRopeYarnConsumDetails.Delete(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList[int.Parse(removeIndexValue)]);
                }
                prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList.RemoveAt(int.Parse(removeIndexValue));
                prWarpingProcessRopeViewModel = await GetCountNameAsync(prWarpingProcessRopeViewModel);
                return PartialView($"AddCountList", prWarpingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessRopeViewModel = await GetCountNameAsync(prWarpingProcessRopeViewModel);
                return PartialView($"AddCountList", prWarpingProcessRopeViewModel);
            }
        }

        public async Task<PrWarpingProcessRopeViewModel> GetCountNameAsync(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList = (List<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>)await _fPrWarpingProcessRopeYarnConsumDetails.GetInitCountData(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeYarnConsumDetailsList);
            return prWarpingProcessRopeViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBallNoList(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            try
            {
                var flag = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList.Any(c => c.BALL_ID_FK.Equals(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetails.BALL_ID_FK));

                if (!flag)
                {
                    prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList.Add(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetails);
                    prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
                    return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);
                }

                prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);



                //var result = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList.Any(c => c.SETID == prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetails.SETID);
                //if (result)
                //{
                //    var item = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList.FirstOrDefault(c => c.SETID == prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetails.SETID);

                //    var flag = item.FPrWarpingProcessRopeBallDetailsList.Any(c => c.BALL_ID_FK.Equals(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetails.BALL_ID_FK));

                //    if (!flag)
                //    {
                //        item.FPrWarpingProcessRopeBallDetailsList.Add(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetails);
                //    }

                //    prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
                //    return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);
                //}
                //prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetails.FPrWarpingProcessRopeBallDetailsList.Add(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetails);
                //prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList.Add(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetails);

                //prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
                //return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
                return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSetNoFromList(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();

            if (prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList[int.Parse(removeIndexValue)].WARP_PROG_ID != 0)
            {
                await _fPrWarpingProcessRopeDetails.Delete(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList[int.Parse(removeIndexValue)]);
            }

            prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList.RemoveAt(int.Parse(removeIndexValue));
            prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
            return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBallNoFromSetNoList(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            //prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList[int.Parse(removeIndexValue)]
            //    .FPrWarpingProcessRopeBallDetailsList.RemoveAt(int.Parse(setRemoveIndex));

            if (prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList[int.Parse(removeIndexValue)].BALLID != 0)
            {
                await _fPrWarpingProcessRopeBallDetails.Delete(prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList[int.Parse(removeIndexValue)]);
            }

            prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList.RemoveAt(int.Parse(removeIndexValue));
            prWarpingProcessRopeViewModel = await GetSoAsync(prWarpingProcessRopeViewModel);
            return PartialView($"AddBallNoList", prWarpingProcessRopeViewModel);
        }

        public async Task<PrWarpingProcessRopeViewModel> GetSoAsync(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            prWarpingProcessRopeViewModel = await _fPrWarpingProcessRopeDetails.GetInitSoData(prWarpingProcessRopeViewModel);
            var ballCount = prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList.Count();
            Response.Headers["BallCount"] = ballCount.ToString();
            return prWarpingProcessRopeViewModel;
        }

        [AcceptVerbs("Get")]
        [Route("/Production/RopeWarping/GetDataBySubGroup/{subGroup?}")]
        public async Task<IActionResult> GetDataBySubGroupIdAsync(string subGroup)
        {
            try
            {
                var set = await _fPrWarpingProcessRopeMaster.GetDataBySubGroupIdAsync(subGroup);
                return Ok(set);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<PrWarpingProcessRopeDataViewModel> GetDataBySetIdAsync(string setId)
        {
            try
            {
                var result = await _fPrWarpingProcessRopeMaster.GetDataBySetIdAsync(setId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpGet]
        public IActionResult RWarpingDeliveryReport(string groupNo)
        {
            return View(model: groupNo);
        }

        [HttpGet]
        public IActionResult RWarpingStrickerReport()
        {
            return View();
        }
    }
}