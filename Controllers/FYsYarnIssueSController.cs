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
    [Route("YarnIssueS")]
    public class FYsYarnIssueSController : Controller
    {
        private readonly IF_YS_YARN_ISSUE_MASTER_S _fYsYarnIssueMaster;
        private readonly IF_YS_YARN_ISSUE_DETAILS_S _fYsYarnIssueDetails;
        private readonly IF_YARN_REQ_MASTER_S _fYarnReqMaster;
        private readonly IF_YARN_TRANSACTION_S _fYarnTransaction;
        private readonly IF_YARN_REQ_DETAILS_S _fYarnReqDetails;
        private readonly ICOM_EX_PI_DETAILS _comExPiDetails;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        public readonly string title = "Yarn Issue (Sample) Information";

        public FYsYarnIssueSController(
            IF_YS_YARN_ISSUE_MASTER_S fYsYarnIssueMaster,
            IF_YS_YARN_ISSUE_DETAILS_S fYsYarnIssueDetails,
            IF_YARN_REQ_MASTER_S fYarnReqMaster,
            IF_YARN_TRANSACTION_S fYarnTransaction,
            IF_YARN_REQ_DETAILS_S fYarnReqDetails,
            ICOM_EX_PI_DETAILS comExPiDetails,
            IBAS_YARN_COUNTINFO basYarnCountinfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fYsYarnIssueMaster = fYsYarnIssueMaster;
            _fYsYarnIssueDetails = fYsYarnIssueDetails;
            _fYarnReqMaster = fYarnReqMaster;
            _fYarnTransaction = fYarnTransaction;
            _fYarnReqDetails = fYarnReqDetails;
            _comExPiDetails = comExPiDetails;
            _basYarnCountinfo = basYarnCountinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{yIssueId}")]
        public async Task<IActionResult> DetailsFYsYarnIssueS(string yIssueId)
        {
            try
            {
                return View(await _fYsYarnIssueMaster.GetInitObjectsAsync(await _fYsYarnIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yIssueId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEditFYsYarnIssueS(FYsYarnIssueSViewModel fYsYarnIssueSViewModel)
        {
            var fYarnIssueMaster = await _fYsYarnIssueMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fYsYarnIssueSViewModel.YarnIssueMasterS.EncryptedId)));

            if (fYarnIssueMaster != null)
            {
                var user = await _userManager.GetUserAsync(User);

                fYsYarnIssueSViewModel.YarnIssueMasterS.UPDATED_BY = user.Id;
                fYsYarnIssueSViewModel.YarnIssueMasterS.YSRID = fYarnIssueMaster.YSRID;
                fYsYarnIssueSViewModel.YarnIssueMasterS.UPDATED_AT = DateTime.Now;
                fYsYarnIssueSViewModel.YarnIssueMasterS.CREATED_AT = fYarnIssueMaster.CREATED_AT;
                fYsYarnIssueSViewModel.YarnIssueMasterS.CREATED_BY = fYarnIssueMaster.CREATED_BY;

                if (await _fYsYarnIssueMaster.Update(fYsYarnIssueSViewModel.YarnIssueMasterS))
                {
                    foreach (var item in fYsYarnIssueSViewModel.YarnIssueDetailsSList.Where(c => c.TRANSID.Equals(0)))
                    {
                        var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.COUNTID, item.LOTID ?? 0);

                        if (fYsYarnIssueSViewModel.YarnIssueMasterS.YISSUEID != 0)
                        {
                            item.YISSUEID = fYsYarnIssueSViewModel.YarnIssueMasterS.YISSUEID;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                            item.TRANS = null;
                            var fYsYarnIssueDetails = await _fYsYarnIssueDetails.GetInsertedObjByAsync(item);

                            if (fYsYarnIssueDetails.TRANSID != 0)
                            {
                                fYsYarnIssueSViewModel.FYarnTransactionS = new F_YARN_TRANSACTION_S
                                {
                                    YTRNDATE = DateTime.Now,
                                    COUNTID = item.COUNTID,
                                    LOTID = item.LOTID,
                                    YISSUEID = fYsYarnIssueDetails.TRANSID,
                                    ISSUE_QTY = item.ISSUE_QTY,
                                    REMARKS = item.REMARKS,
                                    ISSUEID = fYsYarnIssueSViewModel.YarnIssueMasterS.ISSUEID,
                                    OP_BALANCE = lastBalance,
                                    BALANCE = lastBalance - item.ISSUE_QTY >= 0 ? lastBalance - item.ISSUE_QTY : 0
                                };

                                fYsYarnIssueSViewModel.FYarnTransactionS.CREATED_AT = fYsYarnIssueSViewModel.FYarnTransactionS.UPDATED_AT = DateTime.Now;
                                fYsYarnIssueSViewModel.FYarnTransactionS.CREATED_BY = fYsYarnIssueSViewModel.FYarnTransactionS.UPDATED_BY = user.Id;

                                if (!await _fYarnTransaction.InsertByAsync(fYsYarnIssueSViewModel.FYarnTransactionS))
                                {
                                    await _fYsYarnIssueDetails.Delete(fYsYarnIssueDetails);
                                }
                            }
                        }
                        else
                        {
                            TempData["message"] = $"Failed to Update {title}";
                            TempData["type"] = "error";
                            return View($"CreateFYsYarnIssueS", await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueSViewModel));
                        }
                    }

                    TempData["message"] = $"Successfully Updated {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFYsYarnIssueS), $"FYsYarnIssue");
                }
            }
            TempData["message"] = $"Failed to Update {title}";
            TempData["type"] = "error";
            return View($"CreateFYsYarnIssueS", await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueSViewModel));
        }

        [HttpGet]
        [Route("Edit/{yIssueId?}")]
        public async Task<IActionResult> EditFYsYarnIssueS(string yIssueId)
        {
            try
            {
                return View(await _fYsYarnIssueMaster.GetInitObjectsAsync(await _fYsYarnIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yIssueId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFYsYarnIssueS()
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

                var data = (List<F_YS_YARN_ISSUE_MASTER_S>)await _fYsYarnIssueMaster.GetAllIssueMasterList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.YISSUEDATE.ToString().ToUpper().Contains(searchValue)
                                        || m.ISSUE.ISSUTYPE != null && m.ISSUE.ISSUTYPE.ToUpper().Contains(searchValue)
                                        || m.ISSUETO != null && m.ISSUETO.ToString().ToUpper().Contains(searchValue)
                                        || m.PURPOSE != null && m.PURPOSE.ToUpper().Contains(searchValue)
                                        || m.ISREMARKABLE != null && m.ISREMARKABLE.ToString().ToUpper().Contains(searchValue)
                                        || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)
                    ).ToList();
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
        [Route("Create")]
        public async Task<IActionResult> CreateFYsYarnIssueS()
        {
            try
            {
                return View(await _fYsYarnIssueMaster.GetInitObjectsAsync(new FYsYarnIssueSViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFYsYarnIssueS(FYsYarnIssueSViewModel fYsYarnIssueSViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            fYsYarnIssueSViewModel.YarnIssueMasterS.CREATED_AT = fYsYarnIssueSViewModel.YarnIssueMasterS.UPDATED_AT = DateTime.Now;
            fYsYarnIssueSViewModel.YarnIssueMasterS.CREATED_BY = fYsYarnIssueSViewModel.YarnIssueMasterS.UPDATED_BY = user.Id;

            var fYsYarnIssueMaster = await _fYsYarnIssueMaster.GetInsertedObjByAsync(fYsYarnIssueSViewModel.YarnIssueMasterS);

            foreach (var item in fYsYarnIssueSViewModel.YarnIssueDetailsSList)
            {
                var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.COUNTID, item.LOTID ?? 0);

                if (fYsYarnIssueMaster.YISSUEID != 0)
                {
                    var REQ_DET_ID = await _fYsYarnIssueDetails.GetReqId(item.COUNTID ?? 0, item.TRANS.REQ_QTY ?? 0);

                    item.REQ_DET_ID = REQ_DET_ID != 0 ? REQ_DET_ID : null;
                    item.YISSUEID = fYsYarnIssueMaster.YISSUEID;
                    item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                    item.CREATED_BY = item.UPDATED_BY = user.Id;
                    //item.REQ_DET_ID ??= fYsYarnIssueSViewModel.YarnIssueMasterS.YSRID;
                    item.TRANS = null;
                    var fYsYarnIssueDetails = await _fYsYarnIssueDetails.GetInsertedObjByAsync(item);

                    if (fYsYarnIssueDetails.TRANSID != 0)
                    {
                        fYsYarnIssueSViewModel.FYarnTransactionS = new F_YARN_TRANSACTION_S
                        {
                            YTRNDATE = DateTime.Now,
                            COUNTID = item.COUNTID,
                            LOTID = item.LOTID,
                            YISSUEID = fYsYarnIssueDetails.TRANSID,
                            ISSUE_QTY = item.ISSUE_QTY,
                            REMARKS = item.REMARKS,
                            OP_BALANCE = lastBalance,
                            ISSUEID = fYsYarnIssueSViewModel.YarnIssueMasterS.ISSUEID,
                            BALANCE = lastBalance - item.ISSUE_QTY >= 0 ? lastBalance - item.ISSUE_QTY : 0
                        };

                        fYsYarnIssueSViewModel.FYarnTransactionS.CREATED_AT = fYsYarnIssueSViewModel.FYarnTransactionS.UPDATED_AT = DateTime.Now;
                        fYsYarnIssueSViewModel.FYarnTransactionS.CREATED_BY = fYsYarnIssueSViewModel.FYarnTransactionS.UPDATED_BY = user.Id;

                        if (!await _fYarnTransaction.InsertByAsync(fYsYarnIssueSViewModel.FYarnTransactionS))
                        {
                            await _fYsYarnIssueDetails.Delete(fYsYarnIssueDetails);
                            //await _fYsYarnIssueMaster.Delete(fYsYarnIssueMaster);
                        }
                    }
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View($"CreateFYsYarnIssueS", await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueSViewModel));
                }
            }

            TempData["message"] = $"Successfully added {title}.";
            TempData["type"] = "success";
            return RedirectToAction(nameof(GetFYsYarnIssueS), $"FYsYarnIssueS");
        }

        [HttpGet]
        [Route("GetYarnListOthers")]
        public async Task<IActionResult> GetFYarnReqMaster(int ysrId)
        {
            try
            {
                return Ok(await _fYsYarnIssueMaster.GetYarnReqMaster(ysrId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetYarnReqDetailsCountList")]
        public async Task<IActionResult> GetYarnReqDetailsCountList(int orderno, int ysrid)
        {
            try
            {
                return Ok(await _fYarnReqDetails.GetYarnReqCountList(orderno, ysrid));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetYarnReqDetails/{countId?}/{qty?}")]
        public async Task<IActionResult> GetYarnReqDetails(int countId, string qty)
        {
            try
            {
                return Ok(await _fYarnReqDetails.GetSingleYarnReqDetails(countId, double.Parse(qty)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetYarnLotDetails")]
        public async Task<IActionResult> GetYarnLotDetails(int countId)
        {
            try
            {
                return Ok(await _fYarnReqDetails.GetYarnLotDetails(countId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddToList")]
        public async Task<IActionResult> AddToList(FYsYarnIssueSViewModel fYsYarnIssueSViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fYsYarnIssueSViewModel.IsDelete)
                {
                    var fYsYarnIssueDetails = fYsYarnIssueSViewModel.YarnIssueDetailsSList[fYsYarnIssueSViewModel.RemoveIndex];

                    if (fYsYarnIssueDetails.TRANSID > 0)
                    {
                        await _fYsYarnIssueDetails.Delete(fYsYarnIssueDetails);
                    }

                    fYsYarnIssueSViewModel.YarnIssueDetailsSList.RemoveAt(fYsYarnIssueSViewModel.RemoveIndex);

                    return PartialView($"IssueDetailsPartialView", await _fYsYarnIssueDetails.GetInitObjectsByAsync(fYsYarnIssueSViewModel));
                }
                else
                {
                    var lastBalanceByCountIdAsync = await _fYarnTransaction.GetLastBalanceByCountIdAsync(fYsYarnIssueSViewModel.YarnIssueDetailsS.COUNTID, (int)fYsYarnIssueSViewModel.YarnIssueDetailsS.LOTID);

                    if (fYsYarnIssueSViewModel.YarnIssueDetailsS.ISSUE_QTY > lastBalanceByCountIdAsync)
                    {
                        Response.Headers["Status"] = "Stock Alert";
                        return PartialView($"IssueDetailsPartialView", await _fYsYarnIssueDetails.GetInitObjectsByAsync(fYsYarnIssueSViewModel));
                    }

                    //if (!fYsYarnIssueSViewModel.YarnIssueDetailsSList
                    //        .Any(e => e.COUNTID.Equals(fYsYarnIssueSViewModel.YarnIssueDetailsS.COUNTID)
                    //                  && e.TRANS.REQ_QTY.Equals(fYsYarnIssueSViewModel.YarnIssueDetailsS.TRANS.REQ_QTY))
                    //    && fYsYarnIssueSViewModel.YarnIssueDetailsS.ISSUE_QTY <= lastBalanceByCountIdAsync)
                    if (!fYsYarnIssueSViewModel.YarnIssueDetailsSList
                            .Any(e => e.COUNTID.Equals(fYsYarnIssueSViewModel.YarnIssueDetailsS.COUNTID) && e.LOTID.Equals(fYsYarnIssueSViewModel.YarnIssueDetailsS.LOTID) && fYsYarnIssueSViewModel.YarnIssueDetailsS.ISSUE_QTY.Equals(e.ISSUE_QTY) && fYsYarnIssueSViewModel.YarnIssueDetailsS.LOCATIONID.Equals(e.LOCATIONID)
                                      && fYsYarnIssueSViewModel.YarnIssueDetailsS.ISSUE_QTY < lastBalanceByCountIdAsync))
                    {
                        fYsYarnIssueSViewModel.YarnIssueDetailsSList.Add(fYsYarnIssueSViewModel.YarnIssueDetailsS);
                        Response.Headers["Status"] = "Success";
                    }
                    else
                    {
                        Response.Headers["Status"] = "Error";
                    }

                    return PartialView($"IssueDetailsPartialView", await _fYsYarnIssueDetails.GetInitObjectsByAsync(fYsYarnIssueSViewModel));
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetSoList")]
        public async Task<IActionResult> GetSoList()
        {
            try
            {
                return Ok(await _comExPiDetails.GetSoList());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetBasCountList")]
        public async Task<IActionResult> GetByCiList()
        {
            try
            {
                return Ok(await _basYarnCountinfo.GetAll());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetYarnList")]
        public async Task<IActionResult> GetYsrIdList()
        {
            try
            {
                return Ok(await _fYarnReqMaster.GetYsrIdList());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
