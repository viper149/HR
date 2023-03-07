using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Planning;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class PlProductionPlanGroupMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPL_PRODUCTION_PLAN_MASTER _productionPlanMaster;
        private readonly IPL_PRODUCTION_PLAN_DETAILS _productionPlanDetails;
        private readonly IPL_PRODUCTION_SO_DETAILS _plProductionSoDetails;
        private readonly IPL_PRODUCTION_SETDISTRIBUTION _plProductionSetDistribution;

        public PlProductionPlanGroupMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IPL_PRODUCTION_PLAN_MASTER productionPlanMaster,
            IPL_PRODUCTION_PLAN_DETAILS productionPlanDetails,
            IPL_PRODUCTION_SO_DETAILS plProductionSoDetails,
            IPL_PRODUCTION_SETDISTRIBUTION plProductionSetDistribution
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _productionPlanMaster = productionPlanMaster;
            _productionPlanDetails = productionPlanDetails;
            _plProductionSoDetails = plProductionSoDetails;
            _plProductionSetDistribution = plProductionSetDistribution;
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData(string path = "/PlProductionPlanGroupMaster/GetProductionGroup")
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
                IEnumerable<PL_PRODUCTION_PLAN_MASTER> data;
                switch (path)
                {
                    case "/PlProductionPlanGroupMaster/GetProductionGroup":
                        data = await _productionPlanMaster.GetAllAsync("Rope");
                        break;
                    case "/PlProductionPlanGroupMaster/GetProductionGroupSlasher":
                        data = await _productionPlanMaster.GetAllAsync("Slasher");
                        break;
                    case "/PlProductionPlanGroupMaster/GetProductionGroupSizing":
                        data = await _productionPlanMaster.GetAllAsync("Sizing");
                        break;
                    case "/PlProductionPlanGroupMaster/GetProductionGroupSectional":
                        data = await _productionPlanMaster.GetAllAsync("Sectional");
                        break;
                    default:
                        data = null;
                        break;
                }

                //var data = await _productionPlanMaster.GetAllAsync();

                //await UpdateDyeingCompleteSerial(data.Where(c => c.SERIAL_NO != null));
                //data = await _productionPlanMaster.GetAllAsync();

                //if (path == "/PlProductionPlanGroupMaster/GetProductionGroup")
                //{
                //    data = data.Where(c => c.RND_DYEING_TYPE.DTYPE.Equals("Rope"));
                //}
                //else if (path == "/PlProductionPlanGroupMaster/GetProductionGroupSlasher")
                //{
                //    data = data.Where(c => c.RND_DYEING_TYPE.DTYPE.Equals("Slasher"));
                //}
                //else if (path == "/PlProductionPlanGroupMaster/GetProductionGroupSizing")
                //{
                //    data = data.Where(c => c.RND_DYEING_TYPE.DTYPE.Equals("Sizing"));
                //}
                //else if (path == "/PlProductionPlanGroupMaster/GetProductionGroupSectional")
                //{
                //    data = data.Where(c => c.RND_DYEING_TYPE.DTYPE.Equals("Sectional"));
                //}

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.GROUP_NO.ToString().Contains(searchValue)
                                               || m.GROUPDATE != null && m.GROUPDATE.ToString().Contains(searchValue)
                                               || m.DYEING_REFERANCE != null && m.DYEING_REFERANCE.Contains(searchValue)
                                               || m.RND_DYEING_TYPE != null && m.RND_DYEING_TYPE.DTYPE.Contains(searchValue)
                                               || m.GROUPDATE != null && m.SERIAL_NO != 0 && m.SERIAL_NO.ToString().ToUpper().Contains(searchValue)
                                               || m.PRODUCTION_DATE != null && m.PRODUCTION_DATE.ToString().ToUpper().Contains(searchValue)
                                               || m.OPTION1 != null && m.OPTION1.ToUpper().Contains(searchValue)
                                               || m.OPTION2 != null && m.OPTION2.ToUpper().Contains(searchValue)
                                               || m.OPTION3 != null && m.OPTION3.ToUpper().Contains(searchValue)
                                               || m.OPTION4 != null && m.OPTION4.ToUpper().Contains(searchValue)
                                               || m.OPTION5 != null && m.OPTION5.ToUpper().Contains(searchValue)
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
        public IActionResult GetProductionGroup()
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
        public IActionResult GetProductionGroupSlasher()
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
        public IActionResult GetProductionGroupSizing()
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
        public async Task<JsonResult> GetTablePendingData()
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
                var data = await _productionPlanMaster.GetAllPendingAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.TRNSDATE.ToString().Contains(searchValue)
                                    || m.PROG_ != null && m.PROG_.PROG_NO.ToString().ToUpper().Contains(searchValue)
                                    || m.SUBGROUP != null && m.SUBGROUP.SUBGROUPNO.ToString().ToUpper().Contains(searchValue)
                                    || m.SUBGROUP != null && m.SUBGROUP.GROUP != null && m.SUBGROUP.GROUP.GROUP_NO.ToString().ToUpper().Contains(searchValue)
                                    || m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue)
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
        public IActionResult GetPendingList()
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
        public IActionResult GetProductionGroupSectional()
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
        public async Task<IActionResult> CreateProductionGroup()
        {
            var plProductionGroupViewModel = await GetGroupNo(new PlProductionGroupViewModel());
            return View(await GetInfo(plProductionGroupViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductionGroup(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    plProductionGroupViewModel = await AddSoList(plProductionGroupViewModel);
                    var user = await _userManager.GetUserAsync(User);
                    plProductionGroupViewModel.PlProductionPlanMaster.CREATED_BY = user.Id;
                    plProductionGroupViewModel.PlProductionPlanMaster.UPDATED_BY = user.Id;
                    plProductionGroupViewModel.PlProductionPlanMaster.CREATED_AT = DateTime.Now;
                    plProductionGroupViewModel.PlProductionPlanMaster.UPDATED_AT = DateTime.Now;
                    var groupId = await _productionPlanMaster.InsertAndGetIdAsync(plProductionGroupViewModel.PlProductionPlanMaster);

                    if (groupId != 0)
                    {
                        foreach (var item in plProductionGroupViewModel.PlProductionPlanDetailsList)
                        {
                            item.GROUPID = groupId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var subGroupId = await _productionPlanDetails.InsertAndGetIdAsync(item);
                            foreach (var i in item.PlProductionSetDistributionList)
                            {
                                i.SUBGROUP = null;
                                i.PROG_ = null;
                                i.SUBGROUPID = subGroupId;
                                i.CREATED_AT = DateTime.Now;
                                i.CREATED_BY = user.Id;
                                i.UPDATED_AT = DateTime.Now;
                                i.UPDATED_BY = user.Id;
                                await _plProductionSetDistribution.InsertByAsync(i);
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
                        TempData["message"] = "Successfully Group Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                    }

                    TempData["message"] = "Failed to Create Group.";
                    TempData["type"] = "error";


                    plProductionGroupViewModel = await GetGroupNo(plProductionGroupViewModel);
                    return View(await GetInfo(plProductionGroupViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                plProductionGroupViewModel = await GetGroupNo(plProductionGroupViewModel);
                return View(await GetInfo(plProductionGroupViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Group.";
                TempData["type"] = "error";
                plProductionGroupViewModel = await GetGroupNo(plProductionGroupViewModel);
                return View(await GetInfo(plProductionGroupViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditProductionGroup(string id)
        {
            try
            {
                var pId = int.Parse(_protector.Unprotect(id));
                var plProductionGroupViewModel = await _productionPlanMaster.FindAllByIdAsync(pId);

                if (plProductionGroupViewModel.PlProductionPlanMaster != null)
                {
                    plProductionGroupViewModel = await GetSoAsync(plProductionGroupViewModel);

                    plProductionGroupViewModel = await GetNamesAsync(plProductionGroupViewModel);

                    plProductionGroupViewModel = await GetInfo(plProductionGroupViewModel);
                    plProductionGroupViewModel.PlProductionPlanMaster.EncryptedId = _protector.Protect(plProductionGroupViewModel.PlProductionPlanMaster.GROUPID.ToString());

                    return View(plProductionGroupViewModel);
                }

                TempData["message"] = "Group Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProductionGroup(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var groupId = int.Parse(_protector.Unprotect(plProductionGroupViewModel.PlProductionPlanMaster.EncryptedId));
                    var exData = await _productionPlanMaster.FindByIdAsync(groupId);

                    var user = await _userManager.GetUserAsync(User);
                    plProductionGroupViewModel.PlProductionPlanMaster.CREATED_BY = exData.CREATED_BY;
                    plProductionGroupViewModel.PlProductionPlanMaster.CREATED_AT = exData.CREATED_AT;
                    plProductionGroupViewModel.PlProductionPlanMaster.UPDATED_BY = user.Id;
                    plProductionGroupViewModel.PlProductionPlanMaster.UPDATED_AT = DateTime.Now;
                    var blkProgId = await _productionPlanMaster.Update(plProductionGroupViewModel.PlProductionPlanMaster);

                    if (blkProgId)
                    {
                        foreach (var item in plProductionGroupViewModel.PlProductionPlanDetailsList.Where(c => c.SUBGROUPID == 0))
                        {
                            item.GROUPID = groupId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var subGroupId = await _productionPlanDetails.InsertAndGetIdAsync(item);
                            foreach (var i in item.PlProductionSetDistributionList.Where(c => c.SETID == 0))
                            {
                                i.SUBGROUP = null;
                                i.PROG_ = null;
                                i.SUBGROUPID = subGroupId;
                                i.CREATED_AT = DateTime.Now;
                                i.CREATED_BY = user.Id;
                                i.UPDATED_AT = DateTime.Now;
                                i.UPDATED_BY = user.Id;
                                await _plProductionSetDistribution.InsertByAsync(i);
                            }
                        }
                        foreach (var item in plProductionGroupViewModel.PlProductionPlanDetailsList.Where(c => c.SUBGROUPID > 0))
                        {
                            foreach (var i in item.PlProductionSetDistributionList.Where(c => c.SETID == 0))
                            {
                                i.SUBGROUP = null;
                                i.PROG_ = null;
                                i.SUBGROUPID = item.SUBGROUPID;
                                i.CREATED_AT = DateTime.Now;
                                i.CREATED_BY = user.Id;
                                i.UPDATED_AT = DateTime.Now;
                                i.UPDATED_BY = user.Id;
                                await _plProductionSetDistribution.InsertByAsync(i);
                            }
                        }
                        foreach (var i in plProductionGroupViewModel.PlProductionSoDetailsList.Where(c => c.PP_SO_ID == 0))
                        {
                            i.GROUPID = groupId;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;
                            await _plProductionSoDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Updated Group Info";
                        TempData["type"] = "success";
                        return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                    }
                    TempData["message"] = "Failed to Update Group data.";
                    TempData["type"] = "error";

                    return View(await GetInfo(plProductionGroupViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(plProductionGroupViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Update Group data.";
                TempData["type"] = "error";
                return View(await GetInfo(plProductionGroupViewModel));
            }
        }


        private async Task<bool> UpdateDyeingCompleteSerial(IEnumerable<PL_PRODUCTION_PLAN_MASTER> planningList)
        {
            try
            {

                foreach (var item in planningList.Where(c => c.OPTION2.Equals("Yes")))
                {
                    var planningDetails = await _productionPlanMaster.FindByIdAsync(item.GROUPID);
                    planningDetails.SERIAL_NO = 100000;
                    await _productionPlanMaster.Update(planningDetails);
                }

                var i = 1;
                foreach (var item in planningList.Where(c => c.OPTION2.Equals("No") && c.DYEING_TYPE.Equals(2001)))
                {
                    var planningDetails = await _productionPlanMaster.FindByIdAsync(item.GROUPID);
                    planningDetails.SERIAL_NO = i;
                    await _productionPlanMaster.Update(planningDetails);
                    i++;
                }
                i = 1;
                foreach (var item in planningList.Where(c => c.OPTION2.Equals("No") && c.DYEING_TYPE.Equals(2004)))
                {
                    var planningDetails = await _productionPlanMaster.FindByIdAsync(item.GROUPID);
                    planningDetails.SERIAL_NO = i;
                    await _productionPlanMaster.Update(planningDetails);
                    i++;
                }
                i = 1;
                foreach (var item in planningList.Where(c => c.OPTION2.Equals("No") && c.DYEING_TYPE.Equals(2005)))
                {
                    var planningDetails = await _productionPlanMaster.FindByIdAsync(item.GROUPID);
                    planningDetails.SERIAL_NO = i;
                    await _productionPlanMaster.Update(planningDetails);
                    i++;
                }
                i = 1;
                foreach (var item in planningList.Where(c => c.OPTION2.Equals("No") && c.DYEING_TYPE.Equals(2006)))
                {
                    var planningDetails = await _productionPlanMaster.FindByIdAsync(item.GROUPID);
                    planningDetails.SERIAL_NO = i;
                    await _productionPlanMaster.Update(planningDetails);
                    i++;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSerialProductionGroup(PL_PRODUCTION_PLAN_MASTER plProductionPlanMaster)
        {
            try
            {
                //var dateCheck = await _productionPlanMaster.DateSerialCheck(plProductionPlanMaster);

                //if (dateCheck)
                //{
                //    TempData["message"] = "This date already have this serial no.";
                //    TempData["type"] = "warning";
                //    return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                //}


                var user = await _userManager.GetUserAsync(User);
                var groupInfo = await _productionPlanMaster.FindByIdAsync(plProductionPlanMaster.GROUPID);


                var planningList = await _productionPlanMaster.getPlanListSerial(plProductionPlanMaster.SERIAL_NO, plProductionPlanMaster.GROUPID);

                var oldSerialNo = groupInfo.SERIAL_NO;
                var groupId = groupInfo.GROUPID;


                if (oldSerialNo != null && plProductionPlanMaster.SERIAL_NO != null && oldSerialNo < plProductionPlanMaster.SERIAL_NO)
                {
                    TempData["message"] = "Failed to Update Production Details.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                }

                if (oldSerialNo == 100000)
                {
                    TempData["message"] = "Dyeing Already Complete";
                    TempData["type"] = "warning";
                    return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                }

                groupInfo.UPDATED_AT = DateTime.Now;
                groupInfo.UPDATED_BY = user.Id;


                if (plProductionPlanMaster.SERIAL_NO != null)
                {
                    groupInfo.SERIAL_NO = plProductionPlanMaster.SERIAL_NO;
                }

                if (plProductionPlanMaster.SERIAL_NO == null)
                {
                    groupInfo.SERIAL_NO = 100000;

                    TempData["message"] = "Successfully Remove Production Serial.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                }

                if (plProductionPlanMaster.REMARKS != null)
                {
                    groupInfo.REMARKS = plProductionPlanMaster.REMARKS;
                }

                groupInfo.PRODUCTION_DATE = DateTime.Now;

                var isUpdate = await _productionPlanMaster.Update(groupInfo);

                if (isUpdate)
                {
                    if (plProductionPlanMaster.SERIAL_NO != null)
                    {
                        UpdateSerial(planningList, oldSerialNo, groupId);
                    }

                    TempData["message"] = "Successfully Production Details Updated.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
                }

                TempData["message"] = "Failed to Update Production Details Serial.";
                TempData["type"] = "error";
                return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Update Production Details Info";
                TempData["type"] = "error";
                return RedirectToAction("GetProductionGroup", $"PlProductionPlanGroupMaster");
            }
        }


        private async void UpdateSerial(List<PL_PRODUCTION_PLAN_MASTER> planningList, int? oldSerialNo, int groupId)
        {

            if (planningList != null && oldSerialNo == null)
            {
                foreach (var item in planningList)
                {
                    item.SERIAL_NO += 1;
                    await _productionPlanMaster.Update(item);
                }
            }

            if (planningList != null && oldSerialNo != null)
            {
                foreach (var item in planningList.TakeWhile(item => item.GROUPID != groupId))
                {
                    item.SERIAL_NO += 1;
                    await _productionPlanMaster.Update(item);
                }
            }
        }

        public async Task<PlProductionGroupViewModel> GetInfo(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            plProductionGroupViewModel = await _productionPlanMaster.GetInitObjects(plProductionGroupViewModel);
            return plProductionGroupViewModel;
        }

        private async Task<PlProductionGroupViewModel> GetGroupNo(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {

                var groupNo = await _productionPlanMaster.GetGroupNo();

                plProductionGroupViewModel.PlProductionPlanMaster = new PL_PRODUCTION_PLAN_MASTER
                {
                    GROUP_NO = groupNo
                };
                return plProductionGroupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetSubGroupNo(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                int subGroupNo;
                if (!plProductionGroupViewModel.PlProductionPlanDetailsList.Any())
                {
                    var masterData = await _productionPlanDetails.GetAll();
                    subGroupNo = masterData.Any() ? masterData.Select(c => c.SUBGROUPNO).Max() : 1000;
                }
                else
                {
                    subGroupNo = plProductionGroupViewModel.PlProductionPlanDetailsList.Select(c => c.SUBGROUPNO).Max();
                }
                return subGroupNo + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProgramSetNoList(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                var result = plProductionGroupViewModel.PlProductionPlanDetailsList.Any(c =>
                    c.SUBGROUPNO == plProductionGroupViewModel.PlProductionPlanDetails.SUBGROUPNO);
                if (result)
                {
                    var item = plProductionGroupViewModel.PlProductionPlanDetailsList.FirstOrDefault(c => c.SUBGROUPNO == plProductionGroupViewModel.PlProductionPlanDetails.SUBGROUPNO);

                    var flag = item.PlProductionSetDistributionList.Any(c => c.PROG_ID.Equals(plProductionGroupViewModel.PlProductionSetDistribution.PROG_ID));

                    if (!flag)
                    {
                        item.PlProductionSetDistributionList.Add(plProductionGroupViewModel.PlProductionSetDistribution);
                    }
                    plProductionGroupViewModel = await GetNamesAsync(plProductionGroupViewModel);
                    return PartialView($"AddProgramSetNoList", plProductionGroupViewModel);
                }
                plProductionGroupViewModel.PlProductionPlanDetails.PlProductionSetDistributionList.Add(plProductionGroupViewModel.PlProductionSetDistribution);
                plProductionGroupViewModel.PlProductionPlanDetailsList.Add(plProductionGroupViewModel.PlProductionPlanDetails);
                plProductionGroupViewModel = await GetNamesAsync(plProductionGroupViewModel);

                return PartialView($"AddProgramSetNoList", plProductionGroupViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                plProductionGroupViewModel = await GetNamesAsync(plProductionGroupViewModel);
                return PartialView($"AddProgramSetNoList", plProductionGroupViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSubGroupFromList(PlProductionGroupViewModel plProductionGroupViewModel, string removeIndexValue)
        {
            ModelState.Clear();


            if (plProductionGroupViewModel.PlProductionPlanDetailsList[int.Parse(removeIndexValue)].SUBGROUPID != 0)
            {

                var setList = await _plProductionSetDistribution.GetSetListBySubGroup(plProductionGroupViewModel.PlProductionPlanDetailsList[int.Parse(removeIndexValue)].SUBGROUPID);

                await _plProductionSetDistribution.DeleteRange(setList);

                await _productionPlanDetails.Delete(plProductionGroupViewModel
                    .PlProductionPlanDetailsList[int.Parse(removeIndexValue)]);
            }

            plProductionGroupViewModel.PlProductionPlanDetailsList.RemoveAt(int.Parse(removeIndexValue));
            plProductionGroupViewModel = await GetNamesAsync(plProductionGroupViewModel);
            return PartialView($"AddProgramSetNoList", plProductionGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProgramSetFromSubGroupList(PlProductionGroupViewModel plProductionGroupViewModel, string removeIndexValue, string setRemoveIndex)
        {
            ModelState.Clear();

            if (plProductionGroupViewModel.PlProductionPlanDetailsList[int.Parse(removeIndexValue)]
                .PlProductionSetDistributionList[int.Parse(setRemoveIndex)].SETID != 0)
            {
                await _plProductionSetDistribution.Delete(plProductionGroupViewModel
                    .PlProductionPlanDetailsList[int.Parse(removeIndexValue)]
                    .PlProductionSetDistributionList[int.Parse(setRemoveIndex)]);
            }


            plProductionGroupViewModel.PlProductionPlanDetailsList[int.Parse(removeIndexValue)]
                .PlProductionSetDistributionList.RemoveAt(int.Parse(setRemoveIndex));
            plProductionGroupViewModel = await GetNamesAsync(plProductionGroupViewModel);
            return PartialView($"AddProgramSetNoList", plProductionGroupViewModel);
        }

        private async Task<PlProductionGroupViewModel> AddSoList(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                foreach (var i in plProductionGroupViewModel.PlProductionPlanDetailsList.SelectMany(item => item.PlProductionSetDistributionList))
                {
                    plProductionGroupViewModel.PlProductionSoDetails = await _plProductionSoDetails.FindBySetIdAsync(i.PROG_ID ?? 0);

                    var flag = plProductionGroupViewModel.PlProductionSoDetailsList.Where(c => c.POID.Equals(plProductionGroupViewModel.PlProductionSoDetails.POID));

                    if (!flag.Any())
                    {
                        plProductionGroupViewModel.PlProductionSoDetailsList.Add(plProductionGroupViewModel.PlProductionSoDetails);
                    }
                }

                return plProductionGroupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSoNoList(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                var flag = plProductionGroupViewModel.PlProductionSoDetailsList.Where(c => c.POID.Equals(plProductionGroupViewModel.PlProductionSoDetails.POID));

                if (!flag.Any())
                {
                    plProductionGroupViewModel.PlProductionSoDetailsList.Add(plProductionGroupViewModel.PlProductionSoDetails);
                    plProductionGroupViewModel = await GetSoAsync(plProductionGroupViewModel);
                }
                plProductionGroupViewModel = await GetSoAsync(plProductionGroupViewModel);
                return PartialView($"AddSoNoList", plProductionGroupViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                plProductionGroupViewModel = await GetSoAsync(plProductionGroupViewModel);
                return PartialView($"AddSoNoList", plProductionGroupViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSoFromList(PlProductionGroupViewModel plProductionGroupViewModel, string removeIndexValue)
        {
            ModelState.Clear();

            if (plProductionGroupViewModel.PlProductionSoDetailsList[int.Parse(removeIndexValue)].PP_SO_ID != 0)
            {
                await _plProductionSoDetails.Delete(plProductionGroupViewModel
                    .PlProductionSoDetailsList[int.Parse(removeIndexValue)]);
            }
            plProductionGroupViewModel.PlProductionSoDetailsList.RemoveAt(int.Parse(removeIndexValue));
            plProductionGroupViewModel = await GetSoAsync(plProductionGroupViewModel);
            return PartialView($"AddSoNoList", plProductionGroupViewModel);
        }

        [HttpGet]
        public async Task<PL_BULK_PROG_SETUP_D> GetProgramLength(int progId)
        {
            try
            {
                var progLength = await _productionPlanMaster.GetProgramLength(progId);
                return progLength;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PlProductionGroupViewModel> GetNamesAsync(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            plProductionGroupViewModel = await _productionPlanDetails.GetInitData(plProductionGroupViewModel);
            return plProductionGroupViewModel;
        }

        public async Task<PlProductionGroupViewModel> GetSoAsync(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            plProductionGroupViewModel.PlProductionSoDetailsList = (List<PL_PRODUCTION_SO_DETAILS>)await _productionPlanDetails.GetInitSoData(plProductionGroupViewModel.PlProductionSoDetailsList);
            return plProductionGroupViewModel;
        }

        [HttpGet]
        public async Task<RND_PRODUCTION_ORDER> GetPoDetails(int soNo)
        {
            try
            {
                var result = await _productionPlanMaster.GetPoDetails(soNo);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpGet]
        public IActionResult RProductionGroup()
        {
            return View();
        }


    }
}