using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class FysYarnReceiveMasterController : Controller
    {
        private readonly IF_YS_INDENT_DETAILS _fYsIndentDetails;
        private readonly IF_YS_YARN_RECEIVE_MASTER _fYsYarnReceiveMaster;
        private readonly IF_YS_YARN_RECEIVE_DETAILS _fYsYarnReceiveDetails;
        private readonly IF_YS_LOCATION _fYsLocation;
        private readonly IF_YS_LEDGER _fYsLedger;
        private readonly IBAS_YARN_LOTINFO _basYarnLotinfo;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountInfo;
        private readonly IYARNFOR _yarnFor;
        private readonly IF_YARN_QC_APPROVE _fYarnQcApprove;
        private readonly IF_YARN_TRANSACTION _fYarnTransaction;
        private readonly IF_YS_YARN_RECEIVE_REPORT _fYsYarnReceiveReport;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public FysYarnReceiveMasterController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_YS_INDENT_DETAILS fYsIndentDetails,
            IF_YS_YARN_RECEIVE_MASTER fYsYarnReceiveMaster,
            IF_YS_YARN_RECEIVE_DETAILS fYsYarnReceiveDetails,
            IF_YS_LOCATION fYsLocation,
            IF_YS_LEDGER fYsLedger,
            IBAS_YARN_LOTINFO basYarnLotinfo,
            IF_YARN_TRANSACTION fYarnTransaction,
            IF_YS_YARN_RECEIVE_REPORT fYsYarnReceiveReport,
            IF_YARN_QC_APPROVE fYarnQcApprove,
            IBAS_YARN_COUNTINFO basYarnCountInfo,
            IYARNFOR yarnFor,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fYsYarnReceiveMaster = fYsYarnReceiveMaster;
            _fYsYarnReceiveDetails = fYsYarnReceiveDetails;
            _fYsLocation = fYsLocation;
            _fYsLedger = fYsLedger;
            _basYarnLotinfo = basYarnLotinfo;
            _fYsIndentDetails = fYsIndentDetails;
            _fYarnTransaction = fYarnTransaction;
            _fYsYarnReceiveReport = fYsYarnReceiveReport;
            _fYarnQcApprove = fYarnQcApprove;
            _basYarnCountInfo = basYarnCountInfo;
            _yarnFor = yarnFor;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/Receive/Details/{yrcvId?}")]
        public async Task<IActionResult> DetailsFysYarnReceiveMaster(string yrcvId)
        {
            return View(await _fYsYarnReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yrcvId))));
        }

        [HttpGet]
        [Route("/Receive")]
        [Route("/Receive/GetAll")]
        public IActionResult GetReceiveMaster()
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

                var data = (List<F_YS_YARN_RECEIVE_MASTER>)await _fYsYarnReceiveMaster.GetAllYarnReceiveAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.YRCVDATE.ToString().ToUpper().Contains(searchValue)
                                           || m.INV.INVNO != null && m.INV.INVNO.ToUpper().Contains(searchValue)
                                           || m.RCVT.RCVTYPE != null && m.RCVT.RCVTYPE.ToString().ToUpper().Contains(searchValue)
                                           || m.G_ENTRY_NO != null && m.G_ENTRY_NO.ToUpper().Contains(searchValue)
                                           || m.CHALLANNO != null && m.CHALLANNO.ToUpper().Contains(searchValue)
                                           || m.OPT4 != null && m.OPT4.ToUpper().Contains(searchValue)
                                           || m.OPT5 != null && m.OPT5.ToUpper().Contains(searchValue)
                                           || m.G_ENTRY_DATE.ToString().ToUpper().Contains(searchValue)).ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/YarnReceive/AddOrRemoveFromDetails")]
        public async Task<IActionResult> AddToUpdateList(YarnReceiveViewModel yarnReceiveViewModel)
        {
            try
            {
                ModelState.Clear();
                if (yarnReceiveViewModel.IsDelete)
                {
                    var fYsYarnReceiveDetails = yarnReceiveViewModel.FYsYarnReceiveDetailList[yarnReceiveViewModel.RemoveIndex];

                    if (fYsYarnReceiveDetails.TRNSID > 0)
                    {
                        var getQcAndReceiveReportViewModels = await _fYarnQcApprove.FindByTrnsIdAsync(fYsYarnReceiveDetails.TRNSID);

                        // REMOVE THE EXISTING OBJECTS FROM FOREIGN TABLE
                        if (getQcAndReceiveReportViewModels.FYarnQcApproves.Any())
                        {
                            await _fYarnQcApprove.DeleteRange(getQcAndReceiveReportViewModels.FYarnQcApproves);
                        }

                        // REMOVE THE EXISTING OBJECTS FROM FOREIGN TABLE
                        if (getQcAndReceiveReportViewModels.FYsYarnReceiveReports.Any())
                        {
                            await _fYsYarnReceiveReport.DeleteRange(getQcAndReceiveReportViewModels.FYsYarnReceiveReports);
                        }

                        // DELETE THE TARGET OBJECT
                        await _fYsYarnReceiveDetails.Delete(fYsYarnReceiveDetails);
                    }

                    yarnReceiveViewModel.FYsYarnReceiveDetailList.RemoveAt(yarnReceiveViewModel.RemoveIndex);
                }
                else
                {
                    var x = yarnReceiveViewModel.FYsYarnReceiveDetailList.Any(c =>
                        c.LOCATIONID.Equals(yarnReceiveViewModel.FYsYarnReceiveDetail.LOCATIONID));
                    var y = yarnReceiveViewModel.FYsYarnReceiveDetail.PRODID > 0;
                    var z = !yarnReceiveViewModel.FYsYarnReceiveDetailList.Any(e =>
                        e.PRODID.Equals(yarnReceiveViewModel.FYsYarnReceiveDetail.PRODID));
                    if (yarnReceiveViewModel.FYsYarnReceiveDetail.PRODID > 0 && !yarnReceiveViewModel.FYsYarnReceiveDetailList.Any(e => e.PRODID.Equals(yarnReceiveViewModel.FYsYarnReceiveDetail.PRODID) && e.LOT.Equals(yarnReceiveViewModel.FYsYarnReceiveDetail.LOT) && e.LOCATIONID.Equals(yarnReceiveViewModel.FYsYarnReceiveDetail.LOCATIONID)))
                    {
                        yarnReceiveViewModel.FYsYarnReceiveDetailList.Add(yarnReceiveViewModel.FYsYarnReceiveDetail);
                    }
                }

                // INIT OBJECTS
                foreach (var item in yarnReceiveViewModel.FYsYarnReceiveDetailList)
                {
                    item.PROD = await _basYarnCountInfo.FindByIdAsync(item.PRODID ?? 0);
                    item.LOCATION = await _fYsLocation.FindByIdAsync(item.LOCATIONID ?? 0);
                    item.LEDGER = await _fYsLedger.FindByIdAsync(item.LEDGERID ?? 0);
                    item.FYarnFor = await _yarnFor.FindByIdAsync(item.YARN_TYPE ?? 0);
                    item.BasYarnLotinfo = await _basYarnLotinfo.FindByIdAsync(item.LOT ?? 0);

                    if (item.TRNSID > 0)
                    {
                        var getQcAndReceiveReportViewModel = await _fYarnQcApprove.FindByTrnsIdAsync(item.TRNSID);
                        item.F_YARN_QC_APPROVE = getQcAndReceiveReportViewModel.FYarnQcApproves;
                        item.F_YS_YARN_RECEIVE_REPORT = getQcAndReceiveReportViewModel.FYsYarnReceiveReports;
                    }
                }

                yarnReceiveViewModel = await _fYsYarnReceiveDetails.GetDetailsData(yarnReceiveViewModel);
                await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel);
                return PartialView($"YarnReceiveDetailsPartialView", yarnReceiveViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                yarnReceiveViewModel = await _fYsYarnReceiveDetails.GetDetailsData(yarnReceiveViewModel);
                await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel);
                return PartialView($"YarnReceiveDetailsPartialView", yarnReceiveViewModel);
            }
        }

        [HttpGet]
        [Route("/Receive")]
        [Route("/Receive/Create")]
        public async Task<IActionResult> CreateFysYarnReceiveMasterDetails()
        {
            try
            {
                var yarnReceiveViewModel =
                    await _fYsYarnReceiveMaster.GetInitObjectsByAsync(new YarnReceiveViewModel());
                yarnReceiveViewModel.FYsYarnReceiveDetail.REJ_QTY = 0;
                return View(yarnReceiveViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCreateFysYarnReceiveMasterDetails(YarnReceiveViewModel yarnReceiveViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                yarnReceiveViewModel.FYsYarnReceiveMaster.CREATED_BY = yarnReceiveViewModel.FYsYarnReceiveMaster.UPDATED_BY = user.Id;
                yarnReceiveViewModel.FYsYarnReceiveMaster.CREATED_AT = yarnReceiveViewModel.FYsYarnReceiveMaster.UPDATED_AT = DateTime.Now;

                var fYsYarnReceiveMaster = await _fYsYarnReceiveMaster.GetInsertedObjByAsync(yarnReceiveViewModel.FYsYarnReceiveMaster);

                if (fYsYarnReceiveMaster.YRCVID != 0)
                {
                    foreach (var item in yarnReceiveViewModel.FYsYarnReceiveDetailList)
                    {
                        item.YRCVID = fYsYarnReceiveMaster.YRCVID;
                        item.CREATED_AT = DateTime.Now;
                        item.CREATED_BY = user.Id;
                        item.UPDATED_AT = DateTime.Now;
                        item.UPDATED_BY = user.Id;
                        item.INDENT_TYPE = fYsYarnReceiveMaster.INDENT_TYPE;

                        var poId = await _fYarnTransaction.GetPoidByIndId(fYsYarnReceiveMaster.INDID ?? 0);
                        
                        if (item.INDENT_TYPE == 0)
                        {
                            item.POID = poId;
                        }
                        else if (item.INDENT_TYPE == 1)
                        {
                            item.SDRFID = poId;
                        }

                        var insertYarnReceiveDetails = await _fYsYarnReceiveDetails.GetInsertedObjByAsync(item);

                        var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.PRODID, item.LOT ?? 0,item.INDENT_TYPE??0);
                        var lastBagBalance = await _fYarnTransaction.GetLastBagBalanceByCountIdAsync(item.PRODID, item.LOT ?? 0, item.INDENT_TYPE ?? 0);
                        item.REJ_QTY ??= 0;
                        //yarnReceiveViewModel.FYarnTransaction = new F_YARN_TRANSACTION
                        //{
                        //    YTRNDATE = DateTime.Now,
                        //    COUNTID = item.PRODID,
                        //    LOTID = item.LOT,
                        //    YRCVID = insertYarnReceiveDetails.TRNSID,
                        //    RCVTID = yarnReceiveViewModel.FYsYarnReceiveMaster.RCVTID,
                        //    RCV_QTY = item.RCV_QTY,
                        //    INDENT_TYPE = item.INDENT_TYPE,
                        //    REMARKS = item.REMARKS,
                        //    OP_BALANCE = lastBalance,
                        //    BALANCE = lastBalance + item.RCV_QTY - item.REJ_QTY,
                        //    BAG = lastBagBalance + item.BAG
                        //};

                        //var insertTransection = await _fYarnTransaction.InsertByAsync(yarnReceiveViewModel.FYarnTransaction);

                        //if (insertTransection) continue;

                        //await _fYsYarnReceiveDetails.Delete(insertYarnReceiveDetails);
                        ////await _fYsYarnReceiveMaster.Delete(fYsYarnReceiveMaster);

                        //TempData["message"] = "Failed to Add Yarn Receive Information";
                        //TempData["type"] = "error";
                        //return View(nameof(CreateFysYarnReceiveMasterDetails), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel));
                    }

                    TempData["message"] = "Successfully added Yarn Receive Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetReceiveMaster), $"FysYarnReceiveMaster");
                }

                TempData["message"] = "Failed to Add Yarn Receive Information";
                TempData["type"] = "error";
                return View(nameof(CreateFysYarnReceiveMasterDetails), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEditYarnReceive(YarnReceiveViewModel yarnReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fYsYarnReceiveMaster = await _fYsYarnReceiveMaster.FindByIdAsync(int.Parse(_protector.Unprotect(yarnReceiveViewModel.FYsYarnReceiveMaster.EncryptedId)));

                    if (fYsYarnReceiveMaster != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        yarnReceiveViewModel.FYsYarnReceiveMaster.YRCVID = fYsYarnReceiveMaster.YRCVID;
                        yarnReceiveViewModel.FYsYarnReceiveMaster.CREATED_AT = fYsYarnReceiveMaster.CREATED_AT;
                        yarnReceiveViewModel.FYsYarnReceiveMaster.CREATED_BY = fYsYarnReceiveMaster.CREATED_BY;

                        yarnReceiveViewModel.FYsYarnReceiveMaster.UPDATED_AT = DateTime.Now;
                        yarnReceiveViewModel.FYsYarnReceiveMaster.UPDATED_BY = user.Id;


                        if (await _fYsYarnReceiveMaster.Update(yarnReceiveViewModel.FYsYarnReceiveMaster))
                        {
                            if (yarnReceiveViewModel.FYsYarnReceiveDetailList.Any())
                            {
                                foreach (var item in yarnReceiveViewModel.FYsYarnReceiveDetailList.Where(e => e.TRNSID <= 0))
                                {
                                    item.YRCVID = fYsYarnReceiveMaster.YRCVID;
                                    item.TRNSID = 0;
                                    item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                                    item.CREATED_BY = item.UPDATED_BY = user.Id;
                                    item.INDENT_TYPE = fYsYarnReceiveMaster.INDENT_TYPE;

                                    var poId = await _fYarnTransaction.GetPoidByIndId(fYsYarnReceiveMaster.INDID ?? 0);

                                    if (item.INDENT_TYPE == 0)
                                    {
                                        item.POID = poId;
                                    }
                                    else if (item.INDENT_TYPE == 1)
                                    {
                                        item.SDRFID = poId;
                                    }

                                    var fYsYarnReceiveDetails = await _fYsYarnReceiveDetails.GetInsertedObjByAsync(item);

                                    //if (fYsYarnReceiveDetails.TRNSID != 0)
                                    //{
                                    //    var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.PRODID, item.LOT ?? 0, item.INDENT_TYPE ?? 0);
                                    //    var lastBagBalance = await _fYarnTransaction.GetLastBagBalanceByCountIdAsync(item.PRODID, item.LOT ?? 0, item.INDENT_TYPE ?? 0);

                                    //    item.REJ_QTY ??= 0;
                                    //    yarnReceiveViewModel.FYarnTransaction = new F_YARN_TRANSACTION
                                    //    {
                                    //        YTRNDATE = DateTime.Now,
                                    //        COUNTID = item.PRODID,
                                    //        LOTID = item.LOT,
                                    //        YRCVID = fYsYarnReceiveDetails.TRNSID,
                                    //        RCVTID = yarnReceiveViewModel.FYsYarnReceiveMaster.RCVTID,
                                    //        RCV_QTY = item.RCV_QTY,
                                    //        INDENT_TYPE = item.INDENT_TYPE,
                                    //        REMARKS = item.REMARKS,
                                    //        OP_BALANCE = lastBalance,
                                    //        BALANCE = lastBalance + item.RCV_QTY - item.REJ_QTY,
                                    //        BAG = lastBagBalance + item.BAG
                                    //    };

                                    //    yarnReceiveViewModel.FYarnTransaction.CREATED_AT = yarnReceiveViewModel.FYarnTransaction.UPDATED_AT = DateTime.Now;
                                    //    yarnReceiveViewModel.FYarnTransaction.CREATED_BY = yarnReceiveViewModel.FYarnTransaction.UPDATED_BY = user.Id;

                                    //    if (!await _fYarnTransaction.InsertByAsync(yarnReceiveViewModel.FYarnTransaction))
                                    //    {
                                    //        await _fYsYarnReceiveDetails.Delete(fYsYarnReceiveDetails);
                                    //    }
                                    //}
                                }
                                foreach (var item in yarnReceiveViewModel.FYsYarnReceiveDetailList.Where(e => e.TRNSID > 0))
                                {
                                    var details = await _fYsYarnReceiveDetails.FindByIdAsync(item.TRNSID);
                                    details.LOT = item.LOT != 0 ? item.LOT : details.LOT;
                                    details.YARN_TYPE = item.YARN_TYPE != 0 ? item.YARN_TYPE : details.YARN_TYPE;
                                    details.LEDGERID = item.LEDGERID != 0 ? item.LEDGERID : details.LEDGERID;
                                    details.LOCATIONID = item.LOCATIONID != 0 ? item.LOCATIONID:details.LOCATIONID;
                                    details.BAG = item.BAG != 0 ? item.BAG : details.BAG;
                                    details.RCV_QTY = item.RCV_QTY != 0?item.RCV_QTY : details.RCV_QTY;
                                    details.INV_QTY = item.RCV_QTY != 0?item.RCV_QTY : details.RCV_QTY;
                                    details.REJ_QTY = item.REJ_QTY != 0?item.REJ_QTY : details.REJ_QTY;
                                    details.PAGENO = item.PAGENO != 0?item.PAGENO : details.PAGENO;
                                    details.RAW = item.RAW != 0?item.RAW : details.RAW;
                                    details.REMARKS = item.REMARKS;
                                    details.IMPORT_TYPE = item.IMPORT_TYPE;

                                    await _fYsYarnReceiveDetails.Update(details);
                                }
                            }

                            TempData["message"] = "Successfully Updated Yarn Receive Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetReceiveMaster", $"FysYarnReceiveMaster");
                        }

                        TempData["message"] = "Failed to Update Yarn Receive Information.";
                        TempData["type"] = "error";
                        return View(nameof(EditYarnReceive), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel));
                    }

                    TempData["message"] = "Failed to Update Yarn Receive Information.";
                    TempData["type"] = "error";
                    return View(nameof(EditYarnReceive), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel));
                }

                TempData["message"] = "Failed to Update Yarn Receive Information.";
                TempData["type"] = "error";
                return View(nameof(EditYarnReceive), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yrcvId"> Belongs to YRCVID. Primary key. Must not to be null. <see cref="F_YS_YARN_RECEIVE_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Receive/Edit/{yrcvId?}")]
        public async Task<IActionResult> EditYarnReceive(string yrcvId)
        {
            try
            {
                var findByIdIncludeAllAsync = await _fYsYarnReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yrcvId)));

                if (findByIdIncludeAllAsync.FYsYarnReceiveMaster != null)
                {
                    findByIdIncludeAllAsync.FYsYarnReceiveDetail.REJ_QTY = 0;
                    return View(await _fYsYarnReceiveMaster.GetInitObjectsByAsync(findByIdIncludeAllAsync));
                }

                TempData["message"] = "Failed to Retrieve Yarn Receive Details.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetReceiveMaster), $"FysYarnReceiveMaster");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnReceive/GetCountName/{indId?}")]
        public async Task<IActionResult> GetCountNameListByIndId(int indId)
        {
            try
            {
                return Ok(await _fYsIndentDetails.GetIndentYarnListByIndidAsync(indId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnReceive/GetYarnFor/{indId?}/{prodId?}")]
        public async Task<IActionResult> GetYarnForByIndProd(int indId, int prodId)
        {
            try
            {
                return Ok(await _fYsIndentDetails.GetYarnForByIndProdAsync(indId,prodId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnReceive/GetInvoiceDetails/{invId?}")]
        public async Task<IActionResult> GetInvoiceDetailsByInvId(int invId)
        {
            try
            {
                return Ok(await _fYsIndentDetails.GetInvoiceDetailsByINVIDAsync(invId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMrr(YarnReceiveViewModel yarnReceiveViewModel)
        {
            try
            {
                ModelState.Clear();

                var fYsYarnReceiveReports = yarnReceiveViewModel.FYsYarnReceiveReports.Where(e => e.YRDID > 0 && e.MRR_QTY > 0).ToList();

                if (fYsYarnReceiveReports.Any())
                {
                    var user = await _userManager.GetUserAsync(User);
                    var lastMrrNo = await _fYsYarnReceiveReport.GetLastMrrNo();

                    foreach (var item in fYsYarnReceiveReports)
                    {
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.MRRNO = lastMrrNo;
                        lastMrrNo += 1;
                    }

                    return Ok(await _fYsYarnReceiveReport.InsertRangeByAsync(fYsYarnReceiveReports));
                }

                return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<bool?> QcApprove(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var fYarnQcApprove = new F_YARN_QC_APPROVE
                {
                    YRDID = id,
                    YQCADATE = DateTime.Now,
                    APPROVED_BY = user.Id
                };

                var isInsert = await _fYarnQcApprove.InsertByAsync(fYarnQcApprove);
                return isInsert;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}