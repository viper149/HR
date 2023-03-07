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
    [Route("ChemicalIssue")]
    public class FChemIssueController : Controller
    {
        private readonly IF_CHEM_STORE_RECEIVE_DETAILS _fChemStoreReceiveDetails;
        private readonly IF_CHEM_REQ_MASTER _fChemReqMaster;
        private readonly IF_CHEM_ISSUE_MASTER _fChemIssueMaster;
        private readonly IF_CHEM_ISSUE_DETAILS _fChemIssueDetails;
        private readonly IF_CHEM_REQ_DETAILS _fChemReqDetails;
        //private readonly IF_CHEM_TRANSECTION _fChemTransection;
        private readonly IF_CHEM_STORE_PRODUCTINFO _fChemStoreProductinfo;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public FChemIssueController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_CHEM_REQ_MASTER fChemReqMaster,
            IF_CHEM_STORE_RECEIVE_DETAILS fChemStoreReceiveDetails,
            IF_CHEM_ISSUE_MASTER fChemIssueMaster,
            IF_CHEM_ISSUE_DETAILS fChemIssueDetails,
            IF_CHEM_TRANSECTION fChemTransection,
            IF_CHEM_REQ_DETAILS fChemReqDetails,
            IF_CHEM_STORE_PRODUCTINFO fChemStoreProductinfo,
            UserManager<ApplicationUser> userManager)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fChemReqMaster = fChemReqMaster;
            _fChemIssueMaster = fChemIssueMaster;
            _fChemIssueDetails = fChemIssueDetails;
            _fChemStoreProductinfo = fChemStoreProductinfo;
            _fChemStoreReceiveDetails = fChemStoreReceiveDetails;
            //_fChemTransection = fChemTransection;
            _fChemReqDetails = fChemReqDetails;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Details/{cIssueId?}")]
        public async Task<IActionResult> DetailsFChemIssue(string cIssueId)
        {
            return View(await _fChemIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(cIssueId))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFChemIssue(FChemIssueViewModel fChemIssueViewModel)
        {
            if (ModelState.IsValid)
            {
                var fChemIssueMaster = await _fChemIssueMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fChemIssueViewModel.FChemIssueMaster.EncryptedId)));

                if (fChemIssueMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fChemIssueViewModel.FChemIssueMaster.UPDATED_BY = user.Id;
                    fChemIssueViewModel.FChemIssueMaster.CISSUEID = fChemIssueMaster.CISSUEID;
                    fChemIssueViewModel.FChemIssueMaster.UPDATED_AT = DateTime.Now;
                    fChemIssueViewModel.FChemIssueMaster.CREATED_AT = fChemIssueMaster.CREATED_AT;
                    fChemIssueViewModel.FChemIssueMaster.CREATED_BY = fChemIssueMaster.CREATED_BY;

                    if (await _fChemIssueMaster.Update(fChemIssueViewModel.FChemIssueMaster))
                    {
                        var fChemIssueDetailses = fChemIssueViewModel.FChemIssueDetailsList.Where(e => e.CISSDID <= 0 || e.CISSUEID == null);

                        foreach (var item in fChemIssueDetailses)
                        {
                            item.CISSUEID = fChemIssueMaster.CISSUEID;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        }

                        if (await _fChemIssueDetails.InsertRangeByAsync(fChemIssueDetailses))
                        {
                            TempData["message"] = "Successfully Updated Chemical Information.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetChemIssueTable), $"FChemIssue");
                        }
                        TempData["message"] = "Failed To Update Chemical Information.";
                        TempData["type"] = "error";
                    }
                }
                else
                {
                    TempData["message"] = "Failed To Update Chemical Information.";
                    TempData["type"] = "error";
                }
            }
            else
            {
                TempData["message"] = "Invalid Form Submission.";
                TempData["type"] = "error";
            }

            return View(nameof(EditFChemIssue), await _fChemIssueMaster.GetInitObjByAsync(fChemIssueViewModel));
        }

        [HttpGet]
        [Route("Edit/{cIssueId?}")]
        public async Task<IActionResult> EditFChemIssue(string cIssueId)
        {
            return View(await _fChemIssueMaster.GetInitObjByAsync(await _fChemIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(cIssueId)), edit: true)));
        }

        [HttpGet]
        [Route("")]
        [Route("GetAll")]
        public IActionResult GetChemIssueTable()
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

                var data = (List<F_CHEM_ISSUE_MASTER>)await _fChemIssueMaster.GetAllChemIssueMasterList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CISSUEID.ToString().ToUpper().Contains(searchValue)
                                           || m.CISSUEDATE.ToString().ToUpper().Contains(searchValue)
                                           || m.CSRID.ToString().ToUpper().Contains(searchValue)
                                           || m.CISSUE.ISSUTYPE != null && m.CISSUE.ISSUTYPE.ToUpper().Contains(searchValue)
                                           || m.ISSUETO != null && m.ISSUETO.ToString().ToUpper().Contains(searchValue)
                                           || m.PURPOSE != null && m.PURPOSE.ToUpper().Contains(searchValue)
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
        [Route("Create")]
        public async Task<IActionResult> CreateChemicalIssue()
        {
            try
            {
                return View(await _fChemIssueMaster.GetInitObjByAsync(new FChemIssueViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateChemicalIssue(FChemIssueViewModel fChemIssueViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var atLeastOneInsert = false;

                fChemIssueViewModel.FChemIssueMaster.CREATED_BY = fChemIssueViewModel.FChemIssueMaster.UPDATED_BY = user.Id;
                fChemIssueViewModel.FChemIssueMaster.CREATED_AT = fChemIssueViewModel.FChemIssueMaster.UPDATED_AT = DateTime.Now;

                var fChemIssueMaster = await _fChemIssueMaster.GetInsertedObjByAsync(fChemIssueViewModel.FChemIssueMaster);

                if (fChemIssueMaster.CISSUEID != 0)
                {
                    foreach (var item in fChemIssueViewModel.FChemIssueDetailsList)
                    {
                        switch (fChemIssueMaster.ISSUEID)
                        {
                            case 300001:
                            {
                                var fChemStoreReceiveDetails = await _fChemStoreReceiveDetails.FindByIdAsync(item.PRODUCTID ?? 0);
                                item.PRODUCTID = fChemStoreReceiveDetails?.PRODUCTID;
                                break;
                            }
                        }

                        item.CISSUEID = fChemIssueMaster.CISSUEID;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        //var lastBalance = await _fChemTransection.GetLastBalanceByProductIdAsync(item.PRODUCTID, item.CRCVIDD??0);

                        //if (lastBalance != 0 && !(item.ISSUE_QTY > lastBalance))
                        //{
                        //    fChemIssueViewModel.FChemTransection = new F_CHEM_TRANSECTION
                        //    {
                        //        CTRDATE = item.CISSDDATE,
                        //        CRCVID = item.CRCVIDD,
                        //        PRODUCTID = item.PRODUCTID,
                        //        CISSUEID = fChemIssueDetails.CISSDID,
                        //        ISSUEID = fChemIssueViewModel.FChemIssueMaster.ISSUEID,
                        //        ISSUE_QTY = item.ISSUE_QTY,
                        //        REMARKS = item.REMARKS,
                        //        BALANCE = lastBalance - item.ISSUE_QTY,
                        //        OP_BALANCE = lastBalance,
                        //    };

                        //    var insertByAsync = await _fChemTransection.InsertByAsync(fChemIssueViewModel.FChemTransection);

                        //    if (insertByAsync)
                        //    {
                        //        atLeastOneInsert = true;
                        //    }
                        //    else
                        //    {
                        //        await _fChemIssueDetails.Delete(fChemIssueDetails);

                        //        TempData["message"] = "Failed to Add Chemical Issue Information";
                        //        TempData["type"] = "error";
                        //        return View(await _fChemIssueMaster.GetInitObjByAsync(fChemIssueViewModel));
                        //    }
                        //}
                        //else
                        //{
                        //    TempData["message"] = "Insufficient Chemical";
                        //    TempData["type"] = "error";
                        //    return View(await _fChemIssueMaster.GetInitObjByAsync(fChemIssueViewModel));
                        //}
                    }

                    if (!await _fChemIssueDetails.InsertRangeByAsync(fChemIssueViewModel.FChemIssueDetailsList))
                    {
                        await _fChemIssueMaster.Delete(fChemIssueMaster);
                        TempData["message"] = "Failed to add Chemical Issue Information.";
                        TempData["type"] = "error";
                        return RedirectToAction(nameof(GetChemIssueTable), $"FChemIssue");
                    }

                    TempData["message"] = "Successfully added Chemical Issue Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetChemIssueTable), $"FChemIssue");

                }

                TempData["message"] = "Failed to Add Chemical Issue Information";
                TempData["type"] = "error";
                return View(await _fChemIssueMaster.GetInitObjByAsync(fChemIssueViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetChemicalMasterList")]
        public async Task<IActionResult> GetFChemReqMaster(int id)
        {
            try
            {
                return Ok(await _fChemReqMaster.GetChemReqMaster(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                return Ok(await _fChemStoreProductinfo.GetProducts());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetRequirementDD")]
        public async Task<IActionResult> GetRequirementDd()
        {
            try
            {
                return Ok(await _fChemReqMaster.GetRequirementDD());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetChemReqDetails/{csrId?}/{productId?}")]
        public async Task<IActionResult> GetSingleChemReqDetails(int csrId, int productId)
        {
            try
            {
                return Ok(await _fChemReqDetails.GetSingleChemReqDetails(csrId, productId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetSingleChemReqDetailsAsync/{csrId?}/{productId?}")]
        public async Task<IActionResult> GetSingleChemReqDetailsAsync(int csrId, int productId)
        {
            try
            {
                return Ok(await _fChemReqDetails.GetSingleChemReqDetailsAsync(csrId, productId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetRemainingBalanceByBatchId/{productId?}/{batchNo?}")]
        public async Task<IActionResult> GetRemainingBalanceByBatchId(int productId, string batchNo)
        {
            try
            {
                return Ok(await _fChemStoreReceiveDetails.GetRemainingBalanceByBatchId(productId, batchNo));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// <see cref="F_CHEM_ISSUE_MASTER"/>
        /// <paramref name="fChemIssueViewModel.FChemIssueMaster.ISSUEID"/>
        /// 300001 => Loan
        /// </summary>
        /// <param name="fChemIssueViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromChemIssueDetails")]
        public async Task<IActionResult> AddToList(FChemIssueViewModel fChemIssueViewModel)
        {
            try
            {
                if (fChemIssueViewModel.IsDelete)
                {
                    fChemIssueViewModel.FChemIssueDetailsList.RemoveAt(fChemIssueViewModel.RemoveIndex);
                }
                else
                {
                    var lastBalance = await _fChemStoreReceiveDetails.GetRemainingBalanceByBatchId(fChemIssueViewModel.FChemIssueDetails.PRODUCTID, fChemIssueViewModel.BB);

                    //var lastBalance = await _fChemTransection.GetLastBalanceByProductIdAsync(fChemIssueViewModel.FChemIssueDetails.PRODUCTID,fChemIssueViewModel.FChemIssueDetails.CRCVIDD??0);

                    if (fChemIssueViewModel.FChemIssueMaster.ISSUEID != 300001 && ((fChemIssueViewModel.FChemIssueDetails.ISSUE_QTY > fChemIssueViewModel.FChemIssueDetails.REMAINING_AMOUNT) || lastBalance == 0 || fChemIssueViewModel.FChemIssueDetails.ISSUE_QTY > lastBalance))
                    {
                        return PartialView($"ChemicalReceiveDetailsPartialView", await _fChemIssueMaster.GetInitObjForDetailsByAsync(fChemIssueViewModel));
                    }

                    if (!fChemIssueViewModel.FChemIssueDetailsList.Any(e => e.PRODUCTID.Equals(fChemIssueViewModel.FChemIssueDetails.PRODUCTID) && e.CRCVIDD.Equals(fChemIssueViewModel.FChemIssueDetails.CRCVIDD) /*&& e.BATCH_NO.Equals(fChemIssueViewModel.FChemIssueDetails.BATCH_NO)*/))
                    {
                        fChemIssueViewModel.FChemIssueDetails.REMAINING_AMOUNT -= fChemIssueViewModel.FChemIssueDetails.ISSUE_QTY;
                        fChemIssueViewModel.FChemIssueDetails.CRCVIDD = fChemIssueViewModel.FChemIssueDetails.CRCVIDD;
                        fChemIssueViewModel.FChemIssueDetailsList.Add(fChemIssueViewModel.FChemIssueDetails);
                    }
                }

                return PartialView($"ChemIssueDetailsPartialView", await _fChemIssueMaster.GetInitObjForDetailsByAsync(fChemIssueViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}