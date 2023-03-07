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
    [Route("YarnReceiveS")]
    public class FYsYarnReceiveSController : Controller
    {
        private readonly IF_YS_INDENT_DETAILS _fYsIndentDetails;
        private readonly IF_YS_YARN_RECEIVE_MASTER_S _fYsYarnReceiveMaster;
        private readonly IF_YS_YARN_RECEIVE_DETAILS_S _fYsYarnReceiveDetails;
        private readonly IF_YS_LOCATION _fYsLocation;
        private readonly IF_YS_LEDGER _fYsLedger;
        private readonly IF_YARN_TRANSACTION_S _fYarnTransaction;
        private readonly IF_YS_YARN_RECEIVE_REPORT_S _fYsYarnReceiveReport;
        private readonly IF_YARN_QC_APPROVE_S _fYarnQcApprove;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        public readonly string title = "Yarn Receive (Sample) Information";

        public FYsYarnReceiveSController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_YS_INDENT_DETAILS fYsIndentDetails,
            IF_YS_YARN_RECEIVE_MASTER_S fYsYarnReceiveMaster,
            IF_YS_YARN_RECEIVE_DETAILS_S fYsYarnReceiveDetails,
            IF_YS_LOCATION fYsLocation,
            IF_YS_LEDGER fYsLedger,
            IF_YARN_TRANSACTION_S fYarnTransaction,
            IF_YS_YARN_RECEIVE_REPORT_S fYsYarnReceiveReport,
            IF_YARN_QC_APPROVE_S fYarnQcApprove,
            IBAS_YARN_COUNTINFO basYarnCountInfo,
            UserManager<ApplicationUser> userManager)
        {
            _fYsIndentDetails = fYsIndentDetails;
            _fYsYarnReceiveMaster = fYsYarnReceiveMaster;
            _fYsYarnReceiveDetails = fYsYarnReceiveDetails;
            _fYsLocation = fYsLocation;
            _fYsLedger = fYsLedger;
            _fYarnTransaction = fYarnTransaction;
            _fYsYarnReceiveReport = fYsYarnReceiveReport;
            _fYarnQcApprove = fYarnQcApprove;
            _basYarnCountInfo = basYarnCountInfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{yrcvId?}")]
        public async Task<IActionResult> DetailsFYsYarnReceiveS(string yrcvId)
        {
            return View(await _fYsYarnReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yrcvId))));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFYsYarnReceiveS()
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
        [Route("GetTableData")]
        public async Task<JsonResult> GetTableData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;

            var data = (List<F_YS_YARN_RECEIVE_MASTER_S>)await _fYsYarnReceiveMaster.GetAllYarnReceiveAsync();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromDetails")]
        public async Task<IActionResult> AddToUpdateList(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fYsYarnReceiveSViewModel.IsDelete)
                {
                    var fYsYarnReceiveDetails = fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList[fYsYarnReceiveSViewModel.RemoveIndex];

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

                    fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.RemoveAt(fYsYarnReceiveSViewModel.RemoveIndex);
                }
                else
                {
                    var x = fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.Any(c =>
                        c.LOCATIONID.Equals(fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.LOCATIONID));
                    var y = fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.PRODID > 0;
                    var z = !fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.Any(e =>
                        e.PRODID.Equals(fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.PRODID));
                    if (fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.PRODID > 0 && !fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.Any(e => e.PRODID.Equals(fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.PRODID) && e.LOT.Equals(fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.LOT) && e.LOCATIONID.Equals(fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.LOCATIONID)))
                    {
                        fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.Add(fYsYarnReceiveSViewModel.FYsYarnReceiveDetail);
                    }
                }

                // INIT OBJECTS
                foreach (var item in fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList)
                {
                    item.PROD = await _basYarnCountInfo.FindByIdAsync(item.PRODID ?? 0);
                    item.LOCATION = await _fYsLocation.FindByIdAsync(item.LOCATIONID ?? 0);
                    item.LEDGER = await _fYsLedger.FindByIdAsync(item.LEDGERID ?? 0);

                    if (item.TRNSID > 0)
                    {
                        var getQcAndReceiveReportViewModel = await _fYarnQcApprove.FindByTrnsIdAsync(item.TRNSID);
                        item.F_YARN_QC_APPROVE_S = getQcAndReceiveReportViewModel.FYarnQcApproves;
                        item.F_YS_YARN_RECEIVE_REPORT_S = getQcAndReceiveReportViewModel.FYsYarnReceiveReports;
                    }
                }

                fYsYarnReceiveSViewModel = await _fYsYarnReceiveDetails.GetDetailsData(fYsYarnReceiveSViewModel);
                return PartialView($"YarnReceiveDetailsPartialView", fYsYarnReceiveSViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fYsYarnReceiveSViewModel = await _fYsYarnReceiveDetails.GetDetailsData(fYsYarnReceiveSViewModel);
                return PartialView($"YarnReceiveDetailsPartialView", fYsYarnReceiveSViewModel);
            }
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFYsYarnReceiveS()
        {
            try
            {
                var fYsYarnReceiveSViewModel = await _fYsYarnReceiveMaster.GetInitObjectsByAsync(new FYsYarnReceiveSViewModel());
                fYsYarnReceiveSViewModel.FYsYarnReceiveDetail.REJ_QTY = 0;
                return View(fYsYarnReceiveSViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFYsYarnReceiveS(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.CREATED_BY = fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.UPDATED_BY = user.Id;
                fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.CREATED_AT = fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.UPDATED_AT = DateTime.Now;

                var fYsYarnReceiveMaster = await _fYsYarnReceiveMaster.GetInsertedObjByAsync(fYsYarnReceiveSViewModel.FYsYarnReceiveMaster);

                if (fYsYarnReceiveMaster.YRCVID != 0)
                {
                    foreach (var item in fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList)
                    {
                        item.YRCVID = fYsYarnReceiveMaster.YRCVID;
                        item.CREATED_AT = DateTime.Now;
                        item.CREATED_BY = user.Id;
                        item.UPDATED_AT = DateTime.Now;
                        item.UPDATED_BY = user.Id;

                        var insertYarnReceiveDetails = await _fYsYarnReceiveDetails.GetInsertedObjByAsync(item);

                        var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.PRODID, item.LOT ?? 0);
                        item.REJ_QTY ??= 0;
                        fYsYarnReceiveSViewModel.FYarnTransaction = new F_YARN_TRANSACTION_S
                        {
                            YTRNDATE = DateTime.Now,
                            COUNTID = item.PRODID,
                            LOTID = item.LOT,
                            YRCVID = insertYarnReceiveDetails.TRNSID,
                            RCVTID = fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.RCVTID,
                            RCV_QTY = item.RCV_QTY,
                            REMARKS = item.REMARKS,
                            OP_BALANCE = lastBalance,
                            BALANCE = lastBalance + item.RCV_QTY - item.REJ_QTY,
                        };

                        var insertTransection = await _fYarnTransaction.InsertByAsync(fYsYarnReceiveSViewModel.FYarnTransaction);

                        if (insertTransection) continue;

                        await _fYsYarnReceiveDetails.Delete(insertYarnReceiveDetails);
                        //await _fYsYarnReceiveMaster.Delete(fYsYarnReceiveMaster);

                        TempData["message"] = $"Failed to Add {title}";
                        TempData["type"] = "error";
                        return View(nameof(CreateFYsYarnReceiveS), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(fYsYarnReceiveSViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFYsYarnReceiveS), $"FYsYarnReceiveS");
                }

                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(nameof(CreateFYsYarnReceiveS), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(fYsYarnReceiveSViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("Edit/{yrcvId?}")]
        public async Task<IActionResult> EditFYsYarnReceiveS(string yrcvId)
        {
            try
            {
                var findByIdIncludeAllAsync = await _fYsYarnReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yrcvId)));

                if (findByIdIncludeAllAsync.FYsYarnReceiveMaster != null)
                {
                    findByIdIncludeAllAsync.FYsYarnReceiveDetail.REJ_QTY = 0;
                    return View(await _fYsYarnReceiveMaster.GetInitObjectsByAsync(findByIdIncludeAllAsync));
                }

                TempData["message"] = $"Failed to Retrieve {title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFYsYarnReceiveS), $"FYsYarnReceiveS");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFYsYarnReceiveS(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fYsYarnReceiveMaster = await _fYsYarnReceiveMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.EncryptedId)));

                    if (fYsYarnReceiveMaster != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.YRCVID = fYsYarnReceiveMaster.YRCVID;
                        fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.CREATED_AT = fYsYarnReceiveMaster.CREATED_AT;
                        fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.CREATED_BY = fYsYarnReceiveMaster.CREATED_BY;

                        fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.UPDATED_AT = DateTime.Now;
                        fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.UPDATED_BY = user.Id;


                        if (await _fYsYarnReceiveMaster.Update(fYsYarnReceiveSViewModel.FYsYarnReceiveMaster))
                        {
                            if (fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.Any())
                            {
                                foreach (var item in fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList.Where(e => e.TRNSID <= 0))
                                {
                                    item.YRCVID = fYsYarnReceiveMaster.YRCVID;
                                    item.TRNSID = 0;
                                    item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                                    item.CREATED_BY = item.UPDATED_BY = user.Id;

                                    var fYsYarnReceiveDetails = await _fYsYarnReceiveDetails.GetInsertedObjByAsync(item);

                                    if (fYsYarnReceiveDetails.TRNSID != 0)
                                    {
                                        var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.PRODID, item.LOT ?? 0);

                                        item.REJ_QTY ??= 0;
                                        fYsYarnReceiveSViewModel.FYarnTransaction = new F_YARN_TRANSACTION_S
                                        {
                                            YTRNDATE = DateTime.Now,
                                            COUNTID = item.PRODID,
                                            LOTID = item.LOT,
                                            YRCVID = fYsYarnReceiveDetails.TRNSID,
                                            RCVTID = fYsYarnReceiveSViewModel.FYsYarnReceiveMaster.RCVTID,
                                            RCV_QTY = item.RCV_QTY,
                                            REMARKS = item.REMARKS,
                                            OP_BALANCE = lastBalance,
                                            BALANCE = lastBalance + item.RCV_QTY - item.REJ_QTY
                                        };

                                        fYsYarnReceiveSViewModel.FYarnTransaction.CREATED_AT = fYsYarnReceiveSViewModel.FYarnTransaction.UPDATED_AT = DateTime.Now;
                                        fYsYarnReceiveSViewModel.FYarnTransaction.CREATED_BY = fYsYarnReceiveSViewModel.FYarnTransaction.UPDATED_BY = user.Id;

                                        if (!await _fYarnTransaction.InsertByAsync(fYsYarnReceiveSViewModel.FYarnTransaction))
                                        {
                                            await _fYsYarnReceiveDetails.Delete(fYsYarnReceiveDetails);
                                        }
                                    }
                                }
                            }

                            TempData["message"] = $"Successfully Updated {title}";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFYsYarnReceiveS), $"FYsYarnReceiveS");
                        }

                        TempData["message"] = $"Failed to Update {title}";
                        TempData["type"] = "error";
                        return View(nameof(EditFYsYarnReceiveS), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(fYsYarnReceiveSViewModel));
                    }

                    TempData["message"] = $"Failed to Update {title}";
                    TempData["type"] = "error";
                    return View(nameof(EditFYsYarnReceiveS), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(fYsYarnReceiveSViewModel));
                }

                TempData["message"] = $"Failed to Update {title}";
                TempData["type"] = "error";
                return View(nameof(EditFYsYarnReceiveS), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(fYsYarnReceiveSViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetCountName/{indId?}")]
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
        [Route("GetInvoiceDetails/{invId?}")]
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
        [Route("CreateMrr")]
        public async Task<IActionResult> CreateMrr(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel)
        {
            try
            {
                ModelState.Clear();

                var fYsYarnReceiveReports = fYsYarnReceiveSViewModel.FYsYarnReceiveReports.Where(e => e.YRDID > 0 && e.MRR_QTY > 0).ToList();

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
        [Route("QcApprove")]
        public async Task<bool?> QcApprove(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var fYarnQcApprove = new F_YARN_QC_APPROVE_S
            {
                YRDID = id,
                YQCADATE = DateTime.Now,
                APPROVED_BY = user.Id
            };

            return await _fYarnQcApprove.InsertByAsync(fYarnQcApprove);
        }
    }
}
