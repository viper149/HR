using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Planning;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class PlBulkProgSetupController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPL_BULK_PROG_SETUP_M _plBulkProgSetupM;
        private readonly IPL_BULK_PROG_SETUP_D _plBulkProgSetupD;
        private readonly IPL_BULK_PROG_YARN_D _plBulkProgYarnD;
        private readonly IRND_PRODUCTION_ORDER _rndProductionOrder;
        private readonly IPL_PRODUCTION_PLAN_MASTER _plProductionPlanMaster;
        private readonly IPL_PRODUCTION_PLAN_DETAILS _plProductionPlanDetails;
        private readonly IPL_PRODUCTION_SO_DETAILS _plProductionSoDetails;
        private readonly IPL_PRODUCTION_SETDISTRIBUTION _plProductionSetdistribution;
        private readonly IBAS_YARN_LOTINFO _bAS_YARN_LOTINFO;

        public PlBulkProgSetupController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IPL_BULK_PROG_SETUP_M plBulkProgSetupM,
            IPL_BULK_PROG_SETUP_D plBulkProgSetupD,
            IPL_BULK_PROG_YARN_D plBulkProgYarnD,
            IRND_PRODUCTION_ORDER rndProductionOrder,
            IPL_PRODUCTION_PLAN_MASTER plProductionPlanMaster,
            IPL_PRODUCTION_PLAN_DETAILS plProductionPlanDetails,
            IPL_PRODUCTION_SO_DETAILS plProductionSoDetails,
            IPL_PRODUCTION_SETDISTRIBUTION plProductionSetdistribution,
            IBAS_YARN_LOTINFO bAS_YARN_LOTINFO
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _plBulkProgSetupM = plBulkProgSetupM;
            _plBulkProgSetupD = plBulkProgSetupD;
            _plBulkProgYarnD = plBulkProgYarnD;
            _rndProductionOrder = rndProductionOrder;
            _plProductionPlanMaster = plProductionPlanMaster;
            _plProductionPlanDetails = plProductionPlanDetails;
            _plProductionSoDetails = plProductionSoDetails;
            _plProductionSetdistribution = plProductionSetdistribution;
            _bAS_YARN_LOTINFO = bAS_YARN_LOTINFO;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProgNoInUse(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            var isProgNoExists = await _plBulkProgSetupD.FindByProgNoInUseAsync(plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO);
            return !isProgNoExists ? Json(true) : Json($"Program No [ {plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO} ] is already in use");
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData(string path = "/PlBulkProgSetup/GetProductionGroup")
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
                var data = await _plBulkProgSetupM.GetAllAsync();
                //data = data.Where(c => c.PL_BULK_PROG_SETUP_D.FirstOrDefault()?.PROCESS_TYPE == null);


                //if (path == "/PlBulkProgSetup/GetPlBulkProgSetup")
                //{
                //    data = data;
                //}
                if (path == "/PlBulkProgSetup/GetPlBulkProgSetupRope")
                {
                    data = data.Where(c => c.OPT2.Equals("ROPE"));
                }
                else if (path == "/PlBulkProgSetup/GetPlBulkProgSetupSlasher")
                {
                    data = data.Where(c => c.OPT2.Equals("SLASHER"));
                }
                else if (path == "/PlBulkProgSetup/GetPlBulkProgSetupSizing")
                {
                    data = data.Where(c => c.OPT2.Equals("SIZING") || c.OPT2.Equals("ECRU-SIZING"));
                }
                else if (path == "/PlBulkProgSetup/GetPlBulkProgSetupSectional")
                {
                    data = data.Where(c => c.OPT2.Equals("ECRU-SIZING"));
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.RndProductionOrder.ORDER_QTY_YDS != null && m.RndProductionOrder.ORDER_QTY_YDS.ToString().ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.ORDER_QTY_MTR != null && m.RndProductionOrder.ORDER_QTY_MTR.ToString().ToUpper().Contains(searchValue)
                                    || m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue)
                                    || m.OPT2 != null && m.OPT2.ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.OPT1 != null && m.RndProductionOrder.OPT1.ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.OPT2 != null && m.RndProductionOrder.OPT2.ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.OPT3 != null && m.RndProductionOrder.OPT3.ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.OPT4 != null && m.RndProductionOrder.OPT4.ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.OPT5 != null && m.RndProductionOrder.OPT5.ToUpper().Contains(searchValue)
                                    || m.RndProductionOrder.OPT6 != null && m.RndProductionOrder.OPT6.ToUpper().Contains(searchValue)
                                    || m.WARP_QTY != 0 && m.WARP_QTY.ToString().Contains(searchValue)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetPlBulkProgSetup()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreatePlBulkProgSetup()
        {
            return View(await GetInfo(new PlBulkProgSetupViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlBulkProgSetup(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    plBulkProgSetupViewModel.PlBulkProgSetupM.CREATED_BY = user.Id;
                    plBulkProgSetupViewModel.PlBulkProgSetupM.CREATED_AT = DateTime.Now;
                    plBulkProgSetupViewModel.PlBulkProgSetupM.IS_CLOSED = false;

                    var blkProgId = await _plBulkProgSetupM.InsertAndGetIdAsync(plBulkProgSetupViewModel.PlBulkProgSetupM);

                    if (blkProgId != 0)
                    {
                        foreach (var item in plBulkProgSetupViewModel.PlBulkProgSetupDList)
                        {
                            item.BLK_PROG_ID = blkProgId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.IS_AUTO_CREATE_PLAN = plBulkProgSetupViewModel.PlBulkProgSetupD.IS_AUTO_CREATE_PLAN;
                            var progId = await InsertPlBulkProgSetupD(item);
                            if (progId != 0)
                            {
                                foreach (var plBulkProgYarnD in item.PlBulkProgYarnDList)
                                {
                                    plBulkProgYarnD.YARN_ID = 0;
                                    plBulkProgYarnD.PROG_ = null;
                                    plBulkProgYarnD.PROG_ID = progId;
                                    await _plBulkProgYarnD.InsertByAsync(plBulkProgYarnD);
                                }
                            }
                        }
                        TempData["message"] = "Successfully added Program/Set No.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetPlBulkProgSetup", $"PlBulkProgSetup");
                    }
                    TempData["message"] = "Failed to Add Program/Set No.";
                    TempData["type"] = "error";
                    return View(await GetInfo(plBulkProgSetupViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(plBulkProgSetupViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Add Program/Set No.";
                TempData["type"] = "error";
                return View(await GetInfo(plBulkProgSetupViewModel));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> GetLastSetNo(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                var lastSetNo = await _plBulkProgSetupD.GetLastSetNo(plBulkProgSetupViewModel);

                return lastSetNo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditPlBulkProgSetup(string id)
        {
            try
            {
                var pId = int.Parse(_protector.Unprotect(id));
                var plBulkProgSetupViewModel = await _plBulkProgSetupM.FindAllByIdAsync(pId);

                if (plBulkProgSetupViewModel.PlBulkProgSetupM != null)
                {
                    plBulkProgSetupViewModel = await GetInfo(plBulkProgSetupViewModel);
                    plBulkProgSetupViewModel.PlBulkProgSetupM.EncryptedId = _protector.Protect(plBulkProgSetupViewModel.PlBulkProgSetupM.BLK_PROGID.ToString());

                    return View(plBulkProgSetupViewModel);
                }

                TempData["message"] = "Set Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetPlBulkProgSetup", $"PlBulkProgSetup");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return RedirectToAction("GetPlBulkProgSetup", $"PlBulkProgSetup");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditPlBulkProgSetup(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var pId = int.Parse(_protector.Unprotect(plBulkProgSetupViewModel.PlBulkProgSetupM.EncryptedId));
                    var exData = await _plBulkProgSetupM.FindByIdAsync(pId);

                    var user = await _userManager.GetUserAsync(User);
                    plBulkProgSetupViewModel.PlBulkProgSetupM.CREATED_BY = exData.CREATED_BY;
                    plBulkProgSetupViewModel.PlBulkProgSetupM.CREATED_AT = exData.CREATED_AT;
                    plBulkProgSetupViewModel.PlBulkProgSetupM.UPDATED_BY = user.Id;
                    plBulkProgSetupViewModel.PlBulkProgSetupM.UPDATED_AT = DateTime.Now;
                    var blkProgId = await _plBulkProgSetupM.Update(plBulkProgSetupViewModel.PlBulkProgSetupM);

                    if (blkProgId)
                    {
                        foreach (var item in plBulkProgSetupViewModel.PlBulkProgSetupDList.Where(c => c.PROG_ID.Equals(0)))
                        {
                            item.BLK_PROG_ID = exData.BLK_PROGID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            item.IS_AUTO_CREATE_PLAN = plBulkProgSetupViewModel.PlBulkProgSetupD.IS_AUTO_CREATE_PLAN;
                            var progId = await InsertPlBulkProgSetupD(item);

                            if (progId != 0)
                            {
                                foreach (var plBulkProgYarnD in item.PlBulkProgYarnDList)
                                {
                                    plBulkProgYarnD.YARN_ID = 0;
                                    plBulkProgYarnD.PROG_ = null;
                                    plBulkProgYarnD.PROG_ID = progId;
                                    await _plBulkProgYarnD.InsertByAsync(plBulkProgYarnD);
                                }
                            }
                        }
                        foreach (var item in plBulkProgSetupViewModel.PlBulkProgSetupDList.Where(c => c.PROG_ID > 0))
                        {
                            var oldDetailsD = await _plBulkProgSetupD.FindByIdAsync(item.PROG_ID);
                            item.BLK_PROG_ID = exData.BLK_PROGID;
                            item.CREATED_AT = oldDetailsD.CREATED_AT;
                            item.CREATED_BY = oldDetailsD.CREATED_BY;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var progId = await _plBulkProgSetupD.Update(item);

                            foreach (var plBulkProgYarnD in item.PlBulkProgYarnDList.Where(c => c.YARN_ID.Equals(0)))
                            {
                                plBulkProgYarnD.YARN_ID = 0;
                                plBulkProgYarnD.PROG_ = null;
                                plBulkProgYarnD.PROG_ID = item.PROG_ID;
                                await _plBulkProgYarnD.InsertByAsync(plBulkProgYarnD);
                            }

                            foreach (var plBulkProgYarnD in item.PlBulkProgYarnDList.Where(c => c.YARN_ID > 0))
                            {

                                var oldDetails = await _plBulkProgYarnD.FindByIdAsync(plBulkProgYarnD.YARN_ID);
                                plBulkProgYarnD.PROG_ID = oldDetails.PROG_ID;
                                plBulkProgYarnD.CREATED_AT = oldDetails.CREATED_AT;
                                plBulkProgYarnD.CREATED_BY = oldDetails.CREATED_BY;
                                plBulkProgYarnD.UPDATED_BY = user.Id;
                                plBulkProgYarnD.UPDATED_AT = DateTime.Now;

                                await _plBulkProgYarnD.Update(plBulkProgYarnD);
                            }
                        }
                        TempData["message"] = "Successfully Updated Program/Set No.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetPlBulkProgSetup", $"PlBulkProgSetup");
                    }
                    TempData["message"] = "Failed to Update Program/Set No.";
                    TempData["type"] = "error";
                    return View(await GetInfo(plBulkProgSetupViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(plBulkProgSetupViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Update Program/Set No.";
                TempData["type"] = "error";
                return View(await GetInfo(plBulkProgSetupViewModel));
            }
        }

        private async Task<int> InsertPlBulkProgSetupD(PL_BULK_PROG_SETUP_D itemBulkProgSetupD)
        {
            try
            {
                if (itemBulkProgSetupD.PROCESS_TYPE == "ROPE")
                {
                    var progId = await _plBulkProgSetupD.InsertAndGetIdAsync(itemBulkProgSetupD);
                    return progId;
                }
                else
                {
                    var progId = await _plBulkProgSetupD.InsertAndGetIdAsync(itemBulkProgSetupD);

                    if (!itemBulkProgSetupD.IS_AUTO_CREATE_PLAN) return progId;

                    var user = await _userManager.GetUserAsync(User);
                    var plProductionGroupViewModel = await _plProductionPlanMaster.GetPlanDetails(itemBulkProgSetupD);

                    plProductionGroupViewModel.PlProductionPlanMaster.CREATED_BY = user.Id;
                    plProductionGroupViewModel.PlProductionPlanMaster.UPDATED_BY = user.Id;
                    plProductionGroupViewModel.PlProductionPlanMaster.CREATED_AT = DateTime.Now;
                    plProductionGroupViewModel.PlProductionPlanMaster.UPDATED_AT = DateTime.Now;

                    var groupId = await _plProductionPlanMaster.InsertAndGetIdAsync(plProductionGroupViewModel.PlProductionPlanMaster);

                    if (groupId == 0) return progId;

                    foreach (var item in plProductionGroupViewModel.PlProductionPlanDetailsList)
                    {
                        item.GROUPID = groupId;
                        item.CREATED_AT = DateTime.Now;
                        item.CREATED_BY = user.Id;
                        item.UPDATED_AT = DateTime.Now;
                        item.UPDATED_BY = user.Id;
                        var subGroupId = await _plProductionPlanDetails.InsertAndGetIdAsync(item);
                        foreach (var i in item.PlProductionSetDistributionList)
                        {
                            i.SUBGROUP = null;
                            i.PROG_ = null;
                            i.SUBGROUPID = subGroupId;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;
                            await _plProductionSetdistribution.InsertByAsync(i);
                        }
                    }
                    foreach (var i in plProductionGroupViewModel.PlProductionSoDetailsList)
                    {
                        i.GROUPID = groupId;
                        i.CREATED_AT = DateTime.Now;
                        i.CREATED_BY = user.Id;
                        i.UPDATED_AT = DateTime.Now;
                        i.UPDATED_BY = user.Id;
                        await _plProductionSoDetails.InsertByAsync(i);
                    }

                    return progId;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<PlBulkProgSetupViewModel> GetInfo(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            plBulkProgSetupViewModel = await _plBulkProgSetupM.GetInitObjects(plBulkProgSetupViewModel);
            plBulkProgSetupViewModel.PlBulkProgSetupD = new PL_BULK_PROG_SETUP_D
            {
                SET_DATE = DateTime.Now,
                IS_AUTO_CREATE_PLAN = true
            };
            return plBulkProgSetupViewModel;
        }

        //[HttpGet]
        //public async Task<PL_ORDERWISE_LOTINFO> GetLotDetailsFromLotwiseTable(string lotId)
        //{
        //    try
        //    {
        //        var result = await _plBulkProgSetupM.GetLotDetailsFromLotwiseTable(lotId);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        [HttpGet]
        public async Task<BAS_YARN_LOTINFO> GetLotDetails(string lotId)
        {
            try
            {
                var result = await _bAS_YARN_LOTINFO.FindByIdAsync(int.Parse(lotId));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<RND_FABRIC_COUNTINFO> GetCountDetails(string countId)
        {
            try
            {
                var result = await _plBulkProgYarnD.GetCountDetails(int.Parse(countId));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public List<RND_FABRIC_COUNTINFO> GetCountFilterByYarnType(string orderNo, string yarnType)
        {
            try
            {
                var result = _rndProductionOrder.GetPoDetailsByPoIdAsync(orderNo);
                return result.RndFabricCountInfos.Where(c => c.YARNFOR.Equals(yarnType)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public RndProductionOrderDetailViewModel GetPoDetails(string orderNo)
        {
            try
            {
                var result = _rndProductionOrder.GetPoDetailsByPoIdAsync(orderNo);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public RndProductionOrderDetailViewModel GetRndPoDetails(string orderNo)
        {
            try
            {
                var result = _rndProductionOrder.GetRndPoDetailsByPoIdAsync(orderNo);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProgramSetNoDetails(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                if (plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO == null)
                {
                    GetSetQty(plBulkProgSetupViewModel);
                    plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);
                    return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
                }

                var result = plBulkProgSetupViewModel.PlBulkProgSetupDList.Any(c => c.PROG_NO == plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO);

                if (result)
                {
                    GetSetQty(plBulkProgSetupViewModel);
                    plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);
                    return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
                }

                plBulkProgSetupViewModel = await _plBulkProgSetupD.GetBulkProgList(plBulkProgSetupViewModel);

                plBulkProgSetupViewModel.PlBulkProgSetupDList.Add(plBulkProgSetupViewModel.PlBulkProgSetupD);
                GetSetQty(plBulkProgSetupViewModel);
                plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);

                return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);



                //var result = plBulkProgSetupViewModel.PlBulkProgSetupDList.Any(c =>
                //    c.PROG_NO == plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO);
                //if (result)
                //{
                //    var item = plBulkProgSetupViewModel.PlBulkProgSetupDList.FirstOrDefault(c => c.PROG_NO == plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO);

                //    var flag = item.PlBulkProgYarnDList.Any(c => c.COUNTID.Equals(plBulkProgSetupViewModel.PlBulkProgYarnD.COUNTID));

                //    if (!flag)
                //    {
                //        item?.PlBulkProgYarnDList.Add(plBulkProgSetupViewModel.PlBulkProgYarnD);
                //    }
                //    GetSetQty(plBulkProgSetupViewModel);
                //    plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);
                //    return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
                //}
                //plBulkProgSetupViewModel.PlBulkProgSetupD.PlBulkProgYarnDList.Add(plBulkProgSetupViewModel.PlBulkProgYarnD);
                //plBulkProgSetupViewModel.PlBulkProgSetupDList.Add(plBulkProgSetupViewModel.PlBulkProgSetupD);
                //GetSetQty(plBulkProgSetupViewModel);
                //plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);

                //return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                GetSetQty(plBulkProgSetupViewModel);
                plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);
                return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProgramSetNoFromList(PlBulkProgSetupViewModel plBulkProgSetupViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();

                if (plBulkProgSetupViewModel.PlBulkProgSetupDList[int.Parse(removeIndexValue)].PROG_ID != 0)
                {

                    var plBulkProgYarnDList = await _plBulkProgYarnD.GetYarnListByProgId(plBulkProgSetupViewModel.PlBulkProgSetupDList[int.Parse(removeIndexValue)].PROG_ID);

                    if (await _plBulkProgYarnD.DeleteRange(plBulkProgYarnDList))
                    {
                        await _plBulkProgSetupD.Delete(plBulkProgSetupViewModel.PlBulkProgSetupDList[int.Parse(removeIndexValue)]);
                    }
                }

                plBulkProgSetupViewModel.PlBulkProgSetupDList.RemoveAt(int.Parse(removeIndexValue));
                GetSetQty(plBulkProgSetupViewModel);
                plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);
                return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProgramSetNoYarnDetails(PlBulkProgSetupViewModel plBulkProgSetupViewModel, string removeIndexValue, string rateRemoveIndex)
        {
            try
            {
                ModelState.Clear();

                if (plBulkProgSetupViewModel.PlBulkProgSetupDList[int.Parse(removeIndexValue)].PlBulkProgYarnDList[int.Parse(rateRemoveIndex)].YARN_ID != 0)
                {
                    await _plBulkProgYarnD.Delete(plBulkProgSetupViewModel.PlBulkProgSetupDList[int.Parse(removeIndexValue)].PlBulkProgYarnDList[int.Parse(rateRemoveIndex)]);
                }

                plBulkProgSetupViewModel.PlBulkProgSetupDList[int.Parse(removeIndexValue)].PlBulkProgYarnDList.RemoveAt(int.Parse(rateRemoveIndex));
                GetSetQty(plBulkProgSetupViewModel);
                plBulkProgSetupViewModel = await GetNamesAsync(plBulkProgSetupViewModel);
                return PartialView($"AddProgramSetNoDetails", plBulkProgSetupViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void GetSetQty(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            var production = plBulkProgSetupViewModel.PlBulkProgSetupDList.Sum(c => c.SET_QTY);
            var pending = plBulkProgSetupViewModel.PlBulkProgSetupM.WARP_QTY - production;
            Response.Headers["Pending"] = pending.ToString();
            Response.Headers["Production"] = production.ToString();
        }

        public async Task<PlBulkProgSetupViewModel> GetNamesAsync(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            plBulkProgSetupViewModel = await _plBulkProgSetupM.GetInitData(plBulkProgSetupViewModel);
            plBulkProgSetupViewModel.PlBulkProgSetupD = new PL_BULK_PROG_SETUP_D
            {
                SET_DATE = DateTime.Now
            };
            return plBulkProgSetupViewModel;
        }


        [HttpGet]
        public IActionResult GetPlBulkProgSetupRope()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetPlBulkProgSetupSlasher()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetPlBulkProgSetupSizing()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }






        [HttpPost]
        public async Task<JsonResult> GetTableSetData()
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
                var data = await _plBulkProgSetupM.GetAllSetAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.SET_QTY != 0 && m.SET_QTY.ToString().ToUpper().Contains(searchValue)
                                    || m.PROG_NO != null && m.PROG_NO.ToString().ToUpper().Contains(searchValue)
                                    || m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue)
                                    || m.OPT2 != null && m.OPT2.ToUpper().Contains(searchValue)
                                    || m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue)
                                    || m.OPT2 != null && m.OPT2.ToUpper().Contains(searchValue)
                                    || m.OPT3 != null && m.OPT3.ToUpper().Contains(searchValue)
                                    || m.OPT4 != null && m.OPT4.ToUpper().Contains(searchValue)
                                    || m.PROCESS_TYPE != null && m.PROCESS_TYPE.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.PROG_ID.ToString());
                    //item.OPT2 = item.PL_BULK_PROG_SETUP_D.FirstOrDefault()?.PROCESS_TYPE??"No Set Assigned Yet";
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
        public IActionResult GetSetList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }


    }
}