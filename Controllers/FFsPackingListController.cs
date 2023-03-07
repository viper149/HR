using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FFsPackingListController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_FS_FABRIC_RCV_DETAILS _fFsFabricRcvDetails;
        private readonly IF_FS_DELIVERYCHALLAN_PACK_DETAILS _fFsDeliveryChallanPackDetails;
        private readonly IF_FS_DELIVERYCHALLAN_PACK_MASTER _fFsDeliveryChallanPackMaster;
        private readonly ICOM_EX_PI_DETAILS _comExPiDetails;

        public FFsPackingListController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_FS_FABRIC_RCV_DETAILS fFsFabricRcvDetails,
            IF_FS_DELIVERYCHALLAN_PACK_DETAILS fFsDeliveryChallanPackDetails,
            IF_FS_DELIVERYCHALLAN_PACK_MASTER fFsDeliveryChallanPackMaster,
            ICOM_EX_PI_DETAILS comExPiDetails
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fFsFabricRcvDetails = fFsFabricRcvDetails;
            _fFsDeliveryChallanPackDetails = fFsDeliveryChallanPackDetails;
            _fFsDeliveryChallanPackMaster = fFsDeliveryChallanPackMaster;
            _comExPiDetails = comExPiDetails;
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

                var data = await _fFsDeliveryChallanPackMaster.GetChallanListAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                        m.Do != null && m.Do.ToUpper().Contains(searchValue)
                        || m.D_CHALLANDATE != null && m.D_CHALLANDATE.ToString().ToUpper().Contains(searchValue)
                        || m.PI != null && m.PI.ToUpper().Contains(searchValue)
                        || m.StyleName != null && m.StyleName.ToUpper().Contains(searchValue)
                        || m.So_No != null && m.So_No.ToUpper().Contains(searchValue)
                        || m.VNUMBER != null && m.VNUMBER.ToUpper().Contains(searchValue)
                        || m.DEL_TYPE != null && m.DEL_TYPE.ToUpper().Contains(searchValue)
                        || m.LOCKNO != null && m.LOCKNO.ToUpper().Contains(searchValue)
                        || m.AUDITBY != null && m.AUDITBY.ToUpper().Contains(searchValue)
                        || m.DC_NO != null && m.DC_NO.Contains(searchValue)
                        || m.GP_NO != null && m.GP_NO.Contains(searchValue)
                        || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)
                    ).ToList();
                }

                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                //foreach (var item in finalData)
                //{
                //    item.EncryptedId = _protector.Protect(item.D_CHALLANID.ToString());
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
        public IActionResult GetChallanList()
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
        public async Task<IActionResult> CreateDeliveryChallan()
        {
            try
            {
                var fFsRollReceiveViewModel = await _fFsDeliveryChallanPackMaster.GetInitObjects(new FabDeliveryChallanViewModel());

                fFsRollReceiveViewModel.FFsDeliveryChallanPackMaster = new F_FS_DELIVERYCHALLAN_PACK_MASTER
                {
                    D_CHALLANDATE = DateTime.Now,
                    ISSUE_TYPE = 1
                };

                fFsRollReceiveViewModel.FFsDeliverychallanPackDetails = new F_FS_DELIVERYCHALLAN_PACK_DETAILS
                {
                    LENGTH1 = 0,
                    LENGTH2 = 0,
                    PACKING_LIST_ID = 1
                };

                return View(fFsRollReceiveViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDeliveryChallan(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.CREATED_BY = user.Id;
                    fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.UPDATED_BY = user.Id;
                    fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.CREATED_AT = DateTime.Now;
                    fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.UPDATED_AT = DateTime.Now;
                    if (fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.DELIVERY_TYPE == 5)
                    {
                        var challanList = await _fFsDeliveryChallanPackMaster.GetAllByIssueType();
                        if (challanList.Count != 0)
                        {
                            var challanNO = challanList.Select(c => c.DC_NO)
                                .FirstOrDefault();
                            challanNO = challanNO.Substring(3);
                            var challanSl = int.Parse(challanNO) + 1;

                            challanNO = $"PDL{challanSl.ToString().PadLeft(6, '0')}";
                            fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.DC_NO = challanNO;
                        }
                        else
                        {
                            fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.DC_NO = "PDL004001";
                        }
                    }

                    var deld = await _fFsDeliveryChallanPackMaster.InsertAndGetIdAsync(fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster);
                    fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList = fabDeliveryChallanViewModel
                        .FFsDeliverychallanPackDetailsList.GroupBy(c => c.ROLL_NO)
                        .Select(c => c.First())
                        .ToList();



                    if (deld != 0)
                    {
                        var packingList = new List<F_FS_DELIVERYCHALLAN_PACK_DETAILS>();


                        foreach (var item in fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Where(c => c.D_CHALLAN_D_ID.Equals(0) && !packingList.Any(d => d.ROLL_NO.Equals(c.ROLL_NO))))
                        {
                            packingList.Add(item);
                        }

                        foreach (var item in packingList)
                        {
                            item.D_CHALLANID = deld;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;

                            ModelState.Clear();
                            var x = await _fFsDeliveryChallanPackDetails.GetRollDetailsByRcvid(item.ROLL_NO ?? 0);
                            if (x == null)
                            {
                                var challanRoll = await _fFsDeliveryChallanPackDetails.GetInsertedObjByAsync(item);

                                if (challanRoll != null)
                                {
                                    var fullLength = challanRoll.LENGTH1 + challanRoll.LENGTH2;
                                    var rollDetails = await _fFsFabricRcvDetails.FindByIdAsync(item.ROLL_NO ?? 0);
                                    rollDetails.BALANCE_QTY -= fullLength;
                                    await _fFsFabricRcvDetails.Update(rollDetails);
                                }
                            }
                        }

                        TempData["message"] = "Successfully Delivery Pack Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetChallanList", $"FFsPackingList");
                    }

                    TempData["message"] = "Failed to Create Delivery Pack.";
                    TempData["type"] = "error";
                    return View(await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Delivery Pack.";
                TempData["type"] = "error";
                return View(await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditDeliveryChallan(string id)
        {
            try
            {

                var fabDeliveryChallanViewModel = _fFsDeliveryChallanPackMaster.FindAllByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster != null)
                {
                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.EncryptedId = _protector.Protect(fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.D_CHALLANID.ToString());

                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails = new F_FS_DELIVERYCHALLAN_PACK_DETAILS
                    {
                        PACKING_LIST_ID =
                            fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Max(c => c.PACKING_LIST_ID) ?? 1
                    };
                    return View(fabDeliveryChallanViewModel);
                }

                TempData["message"] = "Delivery Challan Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetChallanList", $"FFsPackingList");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDeliveryChallan(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var deliveryCallan = await _fFsDeliveryChallanPackMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.EncryptedId)));
                    if (deliveryCallan != null)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.CREATED_BY = deliveryCallan.CREATED_BY;
                        fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.UPDATED_BY = user.Id;
                        fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.CREATED_AT = deliveryCallan.CREATED_AT;
                        fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.UPDATED_AT = DateTime.Now;
                        var deld = await _fFsDeliveryChallanPackMaster.Update(fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster);

                        if (deld)
                        {
                            fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList = fabDeliveryChallanViewModel
                                .FFsDeliverychallanPackDetailsList.GroupBy(c => c.ROLL_NO)
                                .Select(c => c.First())
                                .ToList();



                            var packingList = new List<F_FS_DELIVERYCHALLAN_PACK_DETAILS>();


                            foreach (var item in fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Where(c => c.D_CHALLAN_D_ID.Equals(0) && !packingList.Any(d=>d.ROLL_NO.Equals(c.ROLL_NO))))
                            {
                                packingList.Add(item);
                            }

                            foreach (var item in packingList)
                            {
                                item.D_CHALLANID = fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.D_CHALLANID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;

                                ModelState.Clear();
                                var x = await _fFsDeliveryChallanPackDetails.GetRollDetailsByRcvid(item.ROLL_NO ?? 0);
                                if (x == null)
                                {
                                    var challanRoll = await _fFsDeliveryChallanPackDetails.GetInsertedObjByAsync(item);

                                    if (challanRoll != null)
                                    {
                                        var fullLength = challanRoll.LENGTH1 + challanRoll.LENGTH2;
                                        var rollDetails = await _fFsFabricRcvDetails.FindByIdAsync(item.ROLL_NO ?? 0);
                                        rollDetails.BALANCE_QTY -= fullLength;
                                        await _fFsFabricRcvDetails.Update(rollDetails);
                                    }
                                }
                            }

                            TempData["message"] = "Successfully Updated Delivery Package";
                            TempData["type"] = "success";
                            return RedirectToAction("GetChallanList", $"FFsPackingList");
                        }
                    }

                    TempData["message"] = "Failed to Update Delivery Package.";
                    TempData["type"] = "error";
                    return View(await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Update Delivery Package.";
                TempData["type"] = "error";
                return View(await _fFsDeliveryChallanPackMaster.GetInitObjects(fabDeliveryChallanViewModel));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRollList(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                ModelState.Clear();
                var flag = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Where(c => c.ROLL_NO.Equals(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO));

                if (!flag.Any())
                {
                    fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Add(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails);
                }
                fabDeliveryChallanViewModel = await GetRollDetailsAsync(fabDeliveryChallanViewModel);
                return PartialView($"AddRollList", fabDeliveryChallanViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fabDeliveryChallanViewModel = await GetRollDetailsAsync(fabDeliveryChallanViewModel);
                return PartialView($"AddRollList", fabDeliveryChallanViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRollFromList(FabDeliveryChallanViewModel fabDeliveryChallanViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList[int.Parse(removeIndexValue)].D_CHALLAN_D_ID != 0)
            {
                await _fFsDeliveryChallanPackDetails.Delete(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList[int.Parse(removeIndexValue)]);
            }
            fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.RemoveAt(int.Parse(removeIndexValue));
            fabDeliveryChallanViewModel = await GetRollDetailsAsync(fabDeliveryChallanViewModel);
            return PartialView($"AddRollList", fabDeliveryChallanViewModel);
        }

        private async Task<FabDeliveryChallanViewModel> GetRollDetailsAsync(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                fabDeliveryChallanViewModel =
                    await _fFsDeliveryChallanPackDetails.GetRollDetailsAsync(fabDeliveryChallanViewModel);
                return fabDeliveryChallanViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<bool> GetRollBalance(int rollId, double fullLength)
        {
            try
            {
                var isRollBalance = await _fFsFabricRcvDetails.GetRollBalance(rollId, fullLength);
                return isRollBalance;
            }
            catch (Exception)
            {
                return false;
            }
        }


        [HttpPost]
        public async Task<IActionResult> RollDetailsListByScan(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                ModelState.Clear();

                var rollId = await _fFsDeliveryChallanPackDetails.GetRollIdByRollNo(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO_N);
                var isShade = await _fFsDeliveryChallanPackDetails.GetRollShadeByRollNo(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO_N);

                var isStyle =
                    await _comExPiDetails.FindSoDetailsAsync(fabDeliveryChallanViewModel
                        .FFsDeliveryChallanPackMaster.SO_NO??0);
                    
                if (rollId == null)
                {
                    Response.Headers["Status"] = "Null";
                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                    return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                }

                var rollTotal = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Where(c => c.D_CHALLAN_D_ID.Equals(0)).Sum(c => c.LENGTH1 + c.LENGTH2);
                var tempTotal = double.Parse(rollId.OPT1) + rollTotal;

                #region Hidden

                //if (tempTotal > fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.DO_BALANCE)
                //{
                //Response.Headers["Status"] = "DO OVER";

                //fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                //fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                //return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                //}

                #endregion

                if (tempTotal > fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.PI_BALANCE && fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.ISSUE_TYPE == 1)
                {
                    Response.Headers["Status"] = "PI OVER";

                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                    return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                }

                if (tempTotal > fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.SO_BALANCE && fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.ISSUE_TYPE == 1)
                {
                    Response.Headers["Status"] = "SO OVER";

                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                    return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                }

                if ((isStyle==null || isStyle.STYLE.FABCODENavigation.FABCODE != rollId.FABCODE) && fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.ISSUE_TYPE == 1)
                {
                    Response.Headers["Status"] = "STYLE";
                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                    return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                }


                if (isShade == null && fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.ISSUE_TYPE == 1)
                {
                    Response.Headers["Status"] = "SHADE";

                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                    return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                }

                if (rollId.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Any())
                {
                    Response.Headers["Status"] = "DB";
                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                    fabDeliveryChallanViewModel = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any() ? await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel) : fabDeliveryChallanViewModel;
                    return PartialView($"AddRollList", fabDeliveryChallanViewModel);
                }

                fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO = rollId.TRNSID;
                var result = await _fFsFabricRcvDetails.FindByIdAsync(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO ?? 0);

                #region Hidden [Noyon]

                //var roll = await _fFsFabricRcvDetails.GetRollIDetails(fFsRollReceiveViewModel.FFsFabricRcvDetails
                //    .ROLL_ID ?? 0);

                //if (roll != null)
                //{
                //    Response.Headers["Status"] = "DB";
                //}

                #endregion

                if (result == null)
                {
                    Response.Headers["Status"] = "Null";
                }

                #region Hidden [Noyon]

                //////COMMENT FOR FUTURE USE,DON'T REMOVE-NOYON//////

                //else if (!result.SO_NO.Equals(fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.SO_NO))
                //{
                //    Response.Headers["Status"] = "Wrong Order";
                //}
                //else if (!result.PO_NO.Equals(fabDeliveryChallanViewModel.FFsDeliveryChallanPackMaster.PIID))
                //{
                //    Response.Headers["Status"] = "Wrong PI";
                //}

                #endregion

                else if (!fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any(c => c.ROLL_NO.Equals(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO)))
                {
                    fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollsByScanAsync(fabDeliveryChallanViewModel);
                    Response.Headers["Status"] = "Success";
                }
                else
                {
                    Response.Headers["Status"] = "Error";
                }

                fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetails(fabDeliveryChallanViewModel);
                fabDeliveryChallanViewModel = await _fFsDeliveryChallanPackDetails.GetRollDetailsList(fabDeliveryChallanViewModel);
                ModelState.Clear();
                return PartialView($"AddRollList", fabDeliveryChallanViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<double> GetDoBalance(int doId)
        {
            try
            {
                return await _fFsDeliveryChallanPackMaster.GetDoBalance(doId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<dynamic> GetPiBalance(int piId, int trnsId)
        {
            try
            {
                return await _fFsDeliveryChallanPackMaster.GetPiBalance(piId, trnsId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<double> GetSoBalance(int soId)
        {
            try
            {
                return await _fFsDeliveryChallanPackMaster.GetSoBalance(soId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPackingRollList(int doId)
        {
            try
            {
                var packingList = await _fFsDeliveryChallanPackMaster.GetDoWisePackingRollList(doId);
                return Ok(packingList);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<dynamic> GetChallanNo(int delType)
        {
            try
            {
                var packingList = await _fFsDeliveryChallanPackMaster.GetChallanNo(delType);
                return packingList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}