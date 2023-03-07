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
    [Route("GeneralStoreIssue")]
    public class FGenSIssueController : Controller
    {
        private readonly IF_GEN_S_REQ_MASTER _fGenSReqMaster;
        private readonly IF_GEN_S_ISSUE_MASTER _fGenSIssueMaster;
        private readonly IF_GEN_S_ISSUE_DETAILS _fGenSIssueDetails;
        private readonly IF_GEN_S_REQ_DETAILS _fGenSReqDetails;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Issue Information";

        public FGenSIssueController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_GEN_S_REQ_MASTER fGenSReqMaster,
            IF_GEN_S_ISSUE_MASTER fGenSIssueMaster,
            IF_GEN_S_ISSUE_DETAILS fGenSIssueDetails,
            IF_GEN_S_REQ_DETAILS fGenSReqDetails,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            UserManager<ApplicationUser> userManager)
        {
            _fGenSReqMaster = fGenSReqMaster;
            _fGenSIssueMaster = fGenSIssueMaster;
            _fGenSIssueDetails = fGenSIssueDetails;
            _fGenSReqDetails = fGenSReqDetails;
            _fGsProductInformation = fGsProductInformation;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{gsIssueId?}")]
        public async Task<IActionResult> DetailsFGenSIssue(string gsIssueId)
        {
            return View(await _fGenSIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gsIssueId))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFGenSIssue(FGenSIssueViewModel fGenSIssueViewModel)
        {
            try
            {
                var fGenSIssueMaster = await _fGenSIssueMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fGenSIssueViewModel.FGenSIssueMaster.EncryptedId)));

                if (fGenSIssueMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fGenSIssueViewModel.FGenSIssueMaster.UPDATED_BY = user.Id;
                    fGenSIssueViewModel.FGenSIssueMaster.GISSUEID = fGenSIssueMaster.GISSUEID;
                    fGenSIssueViewModel.FGenSIssueMaster.UPDATED_AT = DateTime.Now;
                    fGenSIssueViewModel.FGenSIssueMaster.CREATED_AT = fGenSIssueMaster.CREATED_AT;
                    fGenSIssueViewModel.FGenSIssueMaster.CREATED_BY = fGenSIssueMaster.CREATED_BY;

                    if (await _fGenSIssueMaster.Update(fGenSIssueViewModel.FGenSIssueMaster))
                    {
                        var fGenSIssueDetailses = fGenSIssueViewModel.FGenSIssueDetailsesList.Where(e => e.GISSDID <= 0);

                        foreach (var item in fGenSIssueDetailses)
                        {
                            item.GISSUEID = fGenSIssueMaster.GISSUEID;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        }

                        if (await _fGenSIssueDetails.InsertRangeByAsync(fGenSIssueDetailses))
                        {
                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFGenSIssue), $"FGenSIssue");
                        }
                    }
                }

                TempData["message"] = $"Failed to Update {title}.";
                TempData["type"] = "error";
                return View(nameof(EditFGenSIssue), await _fGenSIssueMaster.GetInitObjByAsync(fGenSIssueViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("Edit/{gsIssueId?}")]
        public async Task<IActionResult> EditFGenSIssue(string gsIssueId)
        {
            return View(await _fGenSIssueMaster.GetInitObjByAsync(await _fGenSIssueMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gsIssueId)), true)));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFGenSIssue()
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

            var data = (List<F_GEN_S_ISSUE_MASTER>)await _fGenSIssueMaster.GetAllFGenSIssueMasterList();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.GISSUEID.ToString().ToUpper().Contains(searchValue)
                                       || m.GISSUEDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.GSRID.ToString().ToUpper().Contains(searchValue)
                                       || m.ISSUE.ISSUTYPE != null && m.ISSUE.ISSUTYPE.ToUpper().Contains(searchValue)
                                       || m.ISSUETO != null && m.ISSUETO.ToUpper().Contains(searchValue)
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

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFGenSIssue()
        {
            try
            {
                return View(await _fGenSIssueMaster.GetInitObjByAsync(new FGenSIssueViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFGenSIssue(FGenSIssueViewModel fGenSIssueViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fGenSIssueViewModel.FGenSIssueMaster.CREATED_BY = fGenSIssueViewModel.FGenSIssueMaster.UPDATED_BY = user.Id;
                fGenSIssueViewModel.FGenSIssueMaster.CREATED_AT = fGenSIssueViewModel.FGenSIssueMaster.UPDATED_AT = DateTime.Now;

                var fGenSIssueMaster = await _fGenSIssueMaster.GetInsertedObjByAsync(fGenSIssueViewModel.FGenSIssueMaster);

                if (fGenSIssueMaster.ISSUEID > 0)
                {
                    foreach (var item in fGenSIssueViewModel.FGenSIssueDetailsesList)
                    {
                        item.GISSUEID = fGenSIssueMaster.GISSUEID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                    }

                    if (await _fGenSIssueDetails.InsertRangeByAsync(fGenSIssueViewModel.FGenSIssueDetailsesList))
                    {
                        TempData["message"] = $"Successfully added {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGenSIssue), $"FGenSIssue");
                    }
                }

                await _fGenSIssueMaster.Delete(fGenSIssueMaster);
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(await _fGenSIssueMaster.GetInitObjByAsync(fGenSIssueViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetFGenSMasterList")]
        public async Task<IActionResult> GetFGenSReqMaster(int id)
        {
            try
            {
                return Ok(await _fGenSReqMaster.GetGenSReqMaster(id));
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
                return Ok(await _fGenSReqMaster.GetRequirementDD());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetFGenSReqDetails/{gsrId?}/{productId?}")]
        public async Task<IActionResult> GetSingleGenSReqDetails(int gsrId, int productId)
        {
            try
            {
                return Ok(await _fGenSReqDetails.GetSingleGenSReqDetails(gsrId, productId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetBalance/{productId?}")]
        public async Task<IActionResult> GetBalanceByPId(int productId)
        {
            try
            {
                return Ok(await _fGsProductInformation.GetSingleProductByProductId(productId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromDetails")]
        public async Task<IActionResult> AddToList(FGenSIssueViewModel fGenSIssueViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGenSIssueViewModel.IsDelete)
                {
                    var fGenSIssueDetails = fGenSIssueViewModel.FGenSIssueDetailsesList
                        [fGenSIssueViewModel.RemoveIndex];

                    if (fGenSIssueDetails.GISSDID > 0)
                    {
                        await _fGenSIssueDetails.Delete(fGenSIssueDetails);
                    }

                    fGenSIssueViewModel.FGenSIssueDetailsesList.RemoveAt(fGenSIssueViewModel.RemoveIndex);
                }
                else
                {
                    var fGsProductInformation = await _fGsProductInformation.GetSingleProductByProductId(
                        fGenSIssueViewModel.FGenSIssueDetails.PRODUCTID ?? 0);

                    if (fGsProductInformation.Balance <= 0 || fGenSIssueViewModel.FGenSIssueDetails.ISSUE_QTY > fGsProductInformation.Balance)
                    {
                        TempData["message"] = $"Insufficient Balance.Failed to Add {title}.";
                        TempData["type"] = "error";
                        return PartialView($"FGenSDetailsPartialView", await _fGenSIssueMaster.GetInitObjForDetailsByAsync(fGenSIssueViewModel));
                    }

                    if (!fGenSIssueViewModel.FGenSIssueDetailsesList.Any(e => e.PRODUCTID.Equals(fGenSIssueViewModel.FGenSIssueDetails.PRODUCTID)))
                    {
                        //fGenSIssueViewModel.FGenSIssueDetails.REMAINING_AMOUNT -= fGenSIssueViewModel.FGenSIssueDetails.ISSUE_QTY;
                        //fGenSIssueViewModel.FGenSIssueDetails.GRCVIDD = fGenSIssueViewModel.FGenSIssueDetails.GRCVIDD;
                        fGenSIssueViewModel.FGenSIssueDetailsesList.Add(fGenSIssueViewModel.FGenSIssueDetails);
                    }
                }

                return PartialView($"FGenSDetailsPartialView", await _fGenSIssueMaster.GetInitObjForDetailsByAsync(fGenSIssueViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
