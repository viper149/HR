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
    public class FYsYarnIssueController : Controller
    {
        private readonly IF_YS_YARN_ISSUE_MASTER _fYsYarnIssueMaster;
        private readonly IF_YS_YARN_ISSUE_DETAILS _fYsYarnIssueDetails;
        private readonly IF_YARN_REQ_MASTER _fYarnReqMaster;
        private readonly ICOM_EX_PI_DETAILS _comExPiDetails;
        private readonly IF_YARN_TRANSACTION _fYarnTransaction;
        private readonly IF_YARN_REQ_DETAILS _fYarnReqDetails;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRND_FABRIC_COUNTINFO _rndFabricCountInfo;
        private readonly IF_YS_YARN_RECEIVE_DETAILS _fYsYarnReceiveDetails;
        private readonly IDataProtector _protector;

        public FYsYarnIssueController(
            IF_YS_YARN_ISSUE_MASTER fYsYarnIssueMaster,
            IF_YS_YARN_ISSUE_DETAILS fYsYarnIssueDetails,
            IF_YARN_REQ_MASTER fYarnReqMaster,
            IF_YARN_TRANSACTION fYarnTransaction,
            IF_YARN_REQ_DETAILS fYarnReqDetails,
            ICOM_EX_PI_DETAILS comExPiDetails,
            IBAS_YARN_COUNTINFO basYarnCountinfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IRND_FABRIC_COUNTINFO rndFabricCountInfo,
            IF_YS_YARN_RECEIVE_DETAILS fYsYarnReceiveDetails
        )
        {
            _fYsYarnIssueMaster = fYsYarnIssueMaster;
            _fYsYarnIssueDetails = fYsYarnIssueDetails;
            _fYarnReqMaster = fYarnReqMaster;
            _fYarnReqDetails = fYarnReqDetails;
            _comExPiDetails = comExPiDetails;
            _basYarnCountinfo = basYarnCountinfo;
            _userManager = userManager;
            _rndFabricCountInfo = rndFabricCountInfo;
            _fYsYarnReceiveDetails = fYsYarnReceiveDetails;
            _fYarnTransaction = fYarnTransaction;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yIssueId"> Belongs to YISSUEID. Primary Key. Must not to be null. <see cref="F_YS_YARN_ISSUE_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/YarnIssue/Details/{yIssueId}")]
        public async Task<IActionResult> DetailsFYsYarnIssue(string yIssueId)
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

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="fYsYarnIssueViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostEditFYsYarnIssue(FYsYarnIssueViewModel fYsYarnIssueViewModel)
        {
            var fYarnIssueMaster = await _fYsYarnIssueMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fYsYarnIssueViewModel.YarnIssueMaster.EncryptedId)));

            if (fYarnIssueMaster != null)
            {
                var user = await _userManager.GetUserAsync(User);

                fYsYarnIssueViewModel.YarnIssueMaster.UPDATED_BY = user.Id;
                fYsYarnIssueViewModel.YarnIssueMaster.YSRID = fYarnIssueMaster.YSRID;
                fYsYarnIssueViewModel.YarnIssueMaster.UPDATED_AT = DateTime.Now;
                fYsYarnIssueViewModel.YarnIssueMaster.CREATED_AT = fYarnIssueMaster.CREATED_AT;
                fYsYarnIssueViewModel.YarnIssueMaster.CREATED_BY = fYarnIssueMaster.CREATED_BY;

                if (await _fYsYarnIssueMaster.Update(fYsYarnIssueViewModel.YarnIssueMaster))
                {
                    foreach (var item in fYsYarnIssueViewModel.YarnIssueDetailsList.Where(c => c.TRANSID.Equals(0)))
                    {
                        item.INDENT_TYPE = fYarnIssueMaster.INDENT_TYPE;
                        //var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.COUNTID, item.LOTID ?? 0, item.INDENT_TYPE ?? 0);
                        //var lastBagBalance = await _fYarnTransaction.GetLastBagBalanceByCountIdAsync(item.COUNTID, item.LOTID ?? 0, item.INDENT_TYPE ?? 0);
                        
                        var lastBalance = await _fYarnTransaction.GetLastBalanceByIndentAsync(fYsYarnIssueViewModel.YarnIssueDetails.RCVDID, fYsYarnIssueViewModel.YarnIssueMaster.INDENT_TYPE, fYsYarnIssueViewModel.YarnIssueMaster.YISSUEDATE);
                        var lastBagBalance = await _fYarnTransaction.GetLastBagBalanceByIndentAsync(fYsYarnIssueViewModel.YarnIssueDetails.RCVDID, fYsYarnIssueViewModel.YarnIssueMaster.INDENT_TYPE, fYsYarnIssueViewModel.YarnIssueMaster.YISSUEDATE);


                        if (fYsYarnIssueViewModel.YarnIssueMaster.YISSUEID != 0 && fYsYarnIssueViewModel.YarnIssueDetails.ISSUE_QTY<=lastBalance)
                        {
                            var poId = await _fYarnTransaction.GetPoidByRcvId(item.RCVDID ?? 0);

                            if (item.INDENT_TYPE == 0)
                            {
                                item.POID = poId;
                            }
                            else if (item.INDENT_TYPE == 1)
                            {
                                item.SDRFID = poId;
                            }

                            item.YISSUEID = fYsYarnIssueViewModel.YarnIssueMaster.YISSUEID;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                            item.TRANS = null;
                            var fYsYarnIssueDetails = await _fYsYarnIssueDetails.GetInsertedObjByAsync(item);

                            //if (fYsYarnIssueDetails.TRANSID != 0)
                            //{
                            //    fYsYarnIssueViewModel.FYarnTransaction = new F_YARN_TRANSACTION
                            //    {
                            //        YTRNDATE = DateTime.Now,
                            //        COUNTID = item.COUNTID,
                            //        LOTID = item.LOTID,
                            //        YISSUEID = fYsYarnIssueDetails.TRANSID,
                            //        ISSUE_QTY = item.ISSUE_QTY,
                            //        INDENT_TYPE = item.INDENT_TYPE,
                            //        REMARKS = item.REMARKS,
                            //        ISSUEID = fYsYarnIssueViewModel.YarnIssueMaster.ISSUEID,
                            //        OP_BALANCE = lastBalance,
                            //        BALANCE = lastBalance - item.ISSUE_QTY >= 0 ? lastBalance - item.ISSUE_QTY : 0,
                            //        BAG = lastBagBalance - item.BAG < 0 ? 0 : (lastBagBalance ?? 0) - (item.BAG ?? 0)
                            //    };

                            //    fYsYarnIssueViewModel.FYarnTransaction.CREATED_AT = fYsYarnIssueViewModel.FYarnTransaction.UPDATED_AT = DateTime.Now;
                            //    fYsYarnIssueViewModel.FYarnTransaction.CREATED_BY = fYsYarnIssueViewModel.FYarnTransaction.UPDATED_BY = user.Id;

                            //    if (!await _fYarnTransaction.InsertByAsync(fYsYarnIssueViewModel.FYarnTransaction))
                            //    {
                            //        await _fYsYarnIssueDetails.Delete(fYsYarnIssueDetails);
                            //    }
                            //}
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Yarn Issue Information";
                            TempData["type"] = "error";
                            return View(nameof(CreateIssue), await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueViewModel));
                        }
                    }

                    foreach (var item in fYsYarnIssueViewModel.YarnIssueDetailsList.Where(e => e.TRANSID > 0))
                    {
                        var details = await _fYsYarnIssueDetails.FindByIdAsync(item.TRANSID);
                        details.BAG = item.BAG != 0 ? item.BAG : details.BAG;
                        details.ISSUE_QTY = item.ISSUE_QTY != 0 ? item.ISSUE_QTY : details.ISSUE_QTY;
                        await _fYsYarnIssueDetails.Update(details);
                    }
                    TempData["message"] = "Successfully Updated Yarn Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFYsYarnIssue), $"FYsYarnIssue");
                }
                else
                {
                    TempData["message"] = "Failed to Update Yarn Issue Information";
                    TempData["type"] = "error";
                    return View(nameof(CreateIssue), await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueViewModel));
                }
            }

            TempData["message"] = "Failed to Update Yarn Issue Information";
            TempData["type"] = "error";
            return View(nameof(CreateIssue), await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueViewModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yIssueId"> Belongs to YISSUEID. Primary Key. Must not to be null. <see cref="F_YS_YARN_ISSUE_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/YarnIssue/Edit/{yIssueId?}")]
        public async Task<IActionResult> EditFYsYarnIssue(string yIssueId)
        {
            try
            {
                var fYsYarnIssueViewModel =
                    await _fYsYarnIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(yIssueId)));
                fYsYarnIssueViewModel = await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueViewModel);
                return View(fYsYarnIssueViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("/YarnIssue")]
        [Route("/YarnIssue/GetAll")]
        public IActionResult GetFYsYarnIssue()
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

                var data = (List<F_YS_YARN_ISSUE_MASTER>)await _fYsYarnIssueMaster.GetAllIssueMasterList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.YISSUEDATE != null && m.YISSUEDATE.ToString().ToUpper().Contains(searchValue)
                                        || m.ISSUE is { ISSUTYPE: { } } && m.ISSUE.ISSUTYPE.ToString().ToUpper().Contains(searchValue)
                                        || m.ISSUETO != null && m.ISSUETO.ToString().ToUpper().Contains(searchValue)
                                        || m.YSR is { YSRNO: { } } && m.YSR.YSRNO.ToString().ToUpper().Contains(searchValue)
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
        [Route("/YarnIssue/Create")]
        public async Task<IActionResult> CreateIssue()
        {
            try
            {
                return View(await _fYsYarnIssueMaster.GetInitObjectsAsync(new FYsYarnIssueViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("/YarnIssue/PostCreateIssue")]
        public async Task<IActionResult> PostCreateIssue(FYsYarnIssueViewModel fYsYarnIssueViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            fYsYarnIssueViewModel.YarnIssueMaster.CREATED_AT = fYsYarnIssueViewModel.YarnIssueMaster.UPDATED_AT = DateTime.Now;
            fYsYarnIssueViewModel.YarnIssueMaster.CREATED_BY = fYsYarnIssueViewModel.YarnIssueMaster.UPDATED_BY = user.Id;

            var fYsYarnIssueMaster = await _fYsYarnIssueMaster.GetInsertedObjByAsync(fYsYarnIssueViewModel.YarnIssueMaster);


            foreach (var item in fYsYarnIssueViewModel.YarnIssueDetailsList)
            {
                item.INDENT_TYPE = fYsYarnIssueMaster.INDENT_TYPE;
                //var lastBalance = await _fYarnTransaction.GetLastBalanceByCountIdAsync(item.COUNTID, item.LOTID ?? 0, item.INDENT_TYPE ?? 0);
                //var lastBagBalance = await _fYarnTransaction.GetLastBagBalanceByCountIdAsync(item.COUNTID, item.LOTID ?? 0, item.INDENT_TYPE ?? 0);


                var lastBalance = await _fYarnTransaction.GetLastBalanceByIndentAsync(fYsYarnIssueViewModel.YarnIssueDetails.RCVDID, fYsYarnIssueViewModel.YarnIssueMaster.INDENT_TYPE, fYsYarnIssueViewModel.YarnIssueMaster.YISSUEDATE);
                var lastBagBalance = await _fYarnTransaction.GetLastBagBalanceByIndentAsync(fYsYarnIssueViewModel.YarnIssueDetails.RCVDID, fYsYarnIssueViewModel.YarnIssueMaster.INDENT_TYPE, fYsYarnIssueViewModel.YarnIssueMaster.YISSUEDATE);


                if (fYsYarnIssueViewModel.YarnIssueDetails.ISSUE_QTY > lastBalance)
                {
                    TempData["message"] = "Failed to Issue Yarn";
                    TempData["type"] = "error";

                    var x = await _fYsYarnIssueMaster.FindByIdIncludeAllAsync(fYsYarnIssueMaster.YISSUEID);

                    await _fYsYarnIssueDetails.DeleteRange(x.YarnIssueDetailsList);
                    await _fYsYarnIssueMaster.Delete(x.YarnIssueMaster);

                    return View(nameof(CreateIssue), await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueViewModel));
                }

                if (fYsYarnIssueMaster.YISSUEID != 0)
                {
                    var poId = await _fYarnTransaction.GetPoidByRcvId(item.RCVDID ?? 0);
                    
                    if (item.INDENT_TYPE==0)
                    {
                        item.POID = poId;
                    }
                    else if(item.INDENT_TYPE==1)
                    {
                        item.SDRFID = poId;
                    }
                    //var REQ_DET_ID = await _fYsYarnIssueDetails.GetReqId(item.COUNTID ?? 0, item.TRANS.REQ_QTY ?? 0);

                    item.YISSUEID = fYsYarnIssueMaster.YISSUEID;
                    item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                    item.CREATED_BY = item.UPDATED_BY = user.Id;
                    //item.REQ_DET_ID ??= fYsYarnIssueViewModel.YarnIssueMaster.YSRID;
                    item.TRANS = null;
                    var fYsYarnIssueDetails = await _fYsYarnIssueDetails.GetInsertedObjByAsync(item);

                    //if (fYsYarnIssueDetails.TRANSID != 0)
                    //{
                    //    fYsYarnIssueViewModel.FYarnTransaction = new F_YARN_TRANSACTION
                    //    {
                    //        YTRNDATE = DateTime.Now,
                    //        COUNTID = item.COUNTID,
                    //        LOTID = item.LOTID,
                    //        YISSUEID = fYsYarnIssueDetails.TRANSID,
                    //        ISSUE_QTY = item.ISSUE_QTY,
                    //        INDENT_TYPE = item.INDENT_TYPE,
                    //        REMARKS = item.REMARKS,
                    //        OP_BALANCE = lastBalance,
                    //        ISSUEID = fYsYarnIssueViewModel.YarnIssueMaster.ISSUEID,
                    //        BALANCE = lastBalance - item.ISSUE_QTY >= 0 ? lastBalance - item.ISSUE_QTY : 0,
                    //        BAG = lastBagBalance - item.BAG < 0?0:(lastBagBalance ?? 0) - (item.BAG ?? 0)
                    //    };

                    //    fYsYarnIssueViewModel.FYarnTransaction.CREATED_AT = fYsYarnIssueViewModel.FYarnTransaction.UPDATED_AT = DateTime.Now;
                    //    fYsYarnIssueViewModel.FYarnTransaction.CREATED_BY = fYsYarnIssueViewModel.FYarnTransaction.UPDATED_BY = user.Id;

                    //    if (!await _fYarnTransaction.InsertByAsync(fYsYarnIssueViewModel.FYarnTransaction))
                    //    {
                    //        await _fYsYarnIssueDetails.Delete(fYsYarnIssueDetails);
                    //        //await _fYsYarnIssueMaster.Delete(fYsYarnIssueMaster);
                    //    }
                    //}
                }
                else
                {
                    TempData["message"] = "Failed to Add Yarn Issue Information";
                    TempData["type"] = "error";
                    return View(nameof(CreateIssue), await _fYsYarnIssueMaster.GetInitObjectsAsync(fYsYarnIssueViewModel));
                }
            }

            TempData["message"] = "Successfully added Yarn Issue Information.";
            TempData["type"] = "success";
            return RedirectToAction(nameof(GetFYsYarnIssue), $"FYsYarnIssue");
        }

        [HttpGet]
        [Route("/YarnIssue/GetYarnListOthers")]
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
        [Route("/YarnIssue/GetYarnReqDetails/{countId?}/{qty?}")]
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

        [HttpGet]
        public async Task<IActionResult> GetYarnLotDetailsByCount(int countId, int indentType)
        {
            try
            {
                return Ok(await _fYarnReqDetails.GetYarnLotDetailsByCount(countId, indentType));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetYarnIndentDetails(int countId, int indentType, DateTime? issueDate)
        {
            try
            {
                return Ok(await _fYsYarnReceiveDetails.GetYarnIndentDetails(countId, indentType, issueDate));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetCount(DateTime? issueDate)
        {
            try
            {
                return Ok(await _fYsYarnReceiveDetails.GetCountAsync(issueDate));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> GetAllByIndent(int id)
        {
            try
            {
                return Ok(await _fYsYarnReceiveDetails.GetAllByRcvdId(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetYarnLotDetailsByReqId(int countId)
        {
            try
            {
                var result = await _fYarnReqDetails.FindByIdAsync(countId);
                return Ok(await _fYarnReqDetails.GetYarnLotDetails(result.COUNTID ?? 0));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToList(FYsYarnIssueViewModel yrFYsYarnIssueViewModel)
        {
            try
            {
                ModelState.Clear();
                if (yrFYsYarnIssueViewModel.IsDelete)
                {
                    var fYsYarnIssueDetails = yrFYsYarnIssueViewModel.YarnIssueDetailsList[yrFYsYarnIssueViewModel.RemoveIndex];

                    if (fYsYarnIssueDetails.TRANSID > 0)
                    {
                        await _fYsYarnIssueDetails.Delete(fYsYarnIssueDetails);
                    }

                    yrFYsYarnIssueViewModel.YarnIssueDetailsList.RemoveAt(yrFYsYarnIssueViewModel.RemoveIndex);

                    return PartialView($"IssueDetailsPartialView", await _fYsYarnIssueDetails.GetInitObjectsByAsync(yrFYsYarnIssueViewModel));
                }
                
                var lastBalanceByCountIdAsync = await _fYarnTransaction.GetLastBalanceByIndentAsync(yrFYsYarnIssueViewModel.YarnIssueDetails.RCVDID, yrFYsYarnIssueViewModel.YarnIssueMaster.INDENT_TYPE, yrFYsYarnIssueViewModel.YarnIssueMaster.YISSUEDATE);

                if (yrFYsYarnIssueViewModel.YarnIssueDetails.ISSUE_QTY > lastBalanceByCountIdAsync)
                {
                    Response.Headers["Status"] = "Stock Alert";
                    return PartialView($"IssueDetailsPartialView", await _fYsYarnIssueDetails.GetInitObjectsByAsync(yrFYsYarnIssueViewModel));
                }


                //if (!yrFYsYarnIssueViewModel.YarnIssueDetailsList
                //        .Any(e => e.COUNTID.Equals(yrFYsYarnIssueViewModel.YarnIssueDetails.COUNTID)
                //                  && e.TRANS.REQ_QTY.Equals(yrFYsYarnIssueViewModel.YarnIssueDetails.TRANS.REQ_QTY))
                //    && yrFYsYarnIssueViewModel.YarnIssueDetails.ISSUE_QTY <= lastBalanceByCountIdAsync)
                if (!yrFYsYarnIssueViewModel.YarnIssueDetailsList
                        .Any(e => e.COUNTID.Equals(yrFYsYarnIssueViewModel.YarnIssueDetails.COUNTID) && e.LOTID.Equals(yrFYsYarnIssueViewModel.YarnIssueDetails.LOTID) && yrFYsYarnIssueViewModel.YarnIssueDetails.ISSUE_QTY.Equals(e.ISSUE_QTY) && yrFYsYarnIssueViewModel.YarnIssueDetails.LOCATIONID.Equals(e.LOCATIONID) && yrFYsYarnIssueViewModel.YarnIssueDetails.RCVDID.Equals(e.RCVDID)
                                  && yrFYsYarnIssueViewModel.YarnIssueDetails.ISSUE_QTY < lastBalanceByCountIdAsync))
                {
                    yrFYsYarnIssueViewModel.YarnIssueDetailsList.Add(yrFYsYarnIssueViewModel.YarnIssueDetails);
                    Response.Headers["Status"] = "Success";
                }
                else
                {
                    Response.Headers["Status"] = "Error";
                }

                return PartialView($"IssueDetailsPartialView", await _fYsYarnIssueDetails.GetInitObjectsByAsync(yrFYsYarnIssueViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
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
        [Route("/YarnIssue/GetBasCountList")]
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
        public async Task<int> GetCountIdByReqDId(int reqId)
        {
            return await _fYarnReqDetails.GetCountIdByReqDId(reqId);
        }

        [HttpGet]
        [Route("/YarnIssue/GetYarnList")]
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