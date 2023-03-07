using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels.SampleGarments.Fabric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Route("SampleFabric")]
    [Authorize(Policy = "SampleFabricIssue")]
    public class FSampleFabricIssueController : Controller
    {
        private readonly IF_SAMPLE_FABRIC_ISSUE _fSampleFabricIssue;
        private readonly IF_SAMPLE_FABRIC_ISSUE_DETAILS _fSampleFabricIssueDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IDataProtector _protector;

        public FSampleFabricIssueController(IF_SAMPLE_FABRIC_ISSUE fSampleFabricIssue,
            IF_SAMPLE_FABRIC_ISSUE_DETAILS fSampleFabricIssueDetails,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IAuthorizationService authorizationService)
        {
            _fSampleFabricIssue = fSampleFabricIssue;
            _fSampleFabricIssueDetails = fSampleFabricIssueDetails;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Post")]
        [Route("AddOrRemoveFromDetailsList")]
        public async Task<IActionResult> AddOrRemoveFromDetailsList(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            //if (TryValidateModel(createFSampleFabricIssueViewModel.FSampleFabricIssueDetails))
            //{ }

            if (createFSampleFabricIssueViewModel.IsDelete)
            {
                var fSampleFabricIssueDetails = createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses[createFSampleFabricIssueViewModel.RemoveIndex];

                if (fSampleFabricIssueDetails.SFIDID > 0)
                {
                    await _fSampleFabricIssueDetails.Delete(fSampleFabricIssueDetails);
                }

                createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses.RemoveAt(createFSampleFabricIssueViewModel.RemoveIndex);
            }
            else
            {
                if (createFSampleFabricIssueViewModel.FSampleFabricIssueDetails.SR_QTY == null)
                {
                    ModelState.AddModelError("", $"Please Add Sample Required Qty.");
                }
                else if (createFSampleFabricIssueViewModel.FSampleFabricIssueDetails.SR_QTY <
                         createFSampleFabricIssueViewModel.FSampleFabricIssueDetails.SR_ISSUE_QTY)
                {
                    ModelState.AddModelError("", "Overflow");
                }
                else
                {
                    if (!createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses.Any(e =>
                        e.FABCODE.Equals(createFSampleFabricIssueViewModel.FSampleFabricIssueDetails.FABCODE)) && 
                        createFSampleFabricIssueViewModel.FSampleFabricIssueDetails.FABCODE != null)
                    {
                        createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses.Add(createFSampleFabricIssueViewModel.FSampleFabricIssueDetails);
                    }
                    else
                    {
                        switch (createFSampleFabricIssueViewModel.FSampleFabricIssueDetails.FABCODE)
                        {
                            case null:
                                ModelState.AddModelError("", $"Please Add Fabric Code.");
                                break;
                            default:
                                ModelState.AddModelError("", $"Duplicate Fabric Code Not Allowed.");
                                break;
                        }
                    }
                }
            }

            return PartialView("CreateFSampleFabricIssueDetailsTable", await _fSampleFabricIssue.GetInitObjForDetailsTableByAsync(createFSampleFabricIssueViewModel));
        }

        //[AcceptVerbs("Get", "Post")]
        //[Route("IsLessThanOrEqualToSrQty")]
        //public async Task<IActionResult> IsLessThanOrEqualToSrQty(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        //{
        //    if (createFSampleFabricIssueViewModel.FSampleFabric.SR_QTY == null)
        //    {
        //        return Json($"Please add Sample Required Qty.");
        //    }

        //    return createFSampleFabricIssueViewModel.FSampleFabric.SR_QTY >= createFSampleFabricIssueViewModel.FSampleFabric.SR_ISSUE_QTY ? Json(true) : Json($"Issued Qty. [{createFSampleFabricIssueViewModel.FSampleFabric.SR_ISSUE_QTY}] must less than or equal to Sample Required Qty. [{ createFSampleFabricIssueViewModel.FSampleFabric.SR_QTY}]");
        //}
        [AcceptVerbs("Get", "Post")]
        [Route("GetSrNoPrefix")]
        public async Task<IActionResult> GetSrNoPrefix(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            return Ok(await _fSampleFabricIssue.GetSrNoPrefixByAsync(createFSampleFabricIssueViewModel));
        }

        [AcceptVerbs("Post")]
        [Route("Delete/{sfiId?}")]
        [Authorize(Policy = "SampleFabricIssueDelete")]
        public async Task<IActionResult> DeleteFSampleFabric(string sfiId)
        {
            var fSampleFabricIssue = await _fSampleFabricIssue.FindByIdIncludeAllForDeleteAsync(int.Parse(_protector.Unprotect(sfiId)));

            if (fSampleFabricIssue != null)
            {
                await _fSampleFabricIssueDetails.DeleteRange(fSampleFabricIssue.F_SAMPLE_FABRIC_ISSUE_DETAILS);
                await _fSampleFabricIssue.Delete(fSampleFabricIssue);
            }

            return RedirectToAction(nameof(GetFSampleFabric), $"FSampleFabricIssue");
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFSampleFabric(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var fSampleFabricIssue = await _fSampleFabricIssue.FindByIdAsync(int.Parse(_protector.Unprotect(createFSampleFabricIssueViewModel.FSampleFabric.EncryptedId)));

                if (createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses.Any(e =>
                    e.SR_ISSUE_QTY is > 0))
                {
                    if (createFSampleFabricIssueViewModel.FSampleFabric.ISSUE_DATE == null)
                    {
                        var authorizationResult = await _authorizationService.AuthorizeAsync(User, "SampleFabricIssueForFactoryView");
                        ModelState.AddModelError("FSampleFabric.ISSUE_DATE", $"The field Issue Date can not be empty.");
                        return View(authorizationResult.Succeeded ? "EditFSampleFabricForSample" : "EditFSampleFabric", await _fSampleFabricIssue.GetInitObjByAsync(await _fSampleFabricIssue.FindByIdIncludeAllAsync(fSampleFabricIssue.SFIID)));
                    }
                }

                createFSampleFabricIssueViewModel.FSampleFabric.SFIID = fSampleFabricIssue.SFIID;
                createFSampleFabricIssueViewModel.FSampleFabric.SRNO = fSampleFabricIssue.SRNO;
                createFSampleFabricIssueViewModel.FSampleFabric.CREATED_AT = fSampleFabricIssue.CREATED_AT;
                createFSampleFabricIssueViewModel.FSampleFabric.CREATED_BY = fSampleFabricIssue.CREATED_BY;
                createFSampleFabricIssueViewModel.FSampleFabric.UPDATED_AT = DateTime.Now;
                createFSampleFabricIssueViewModel.FSampleFabric.UPDATED_BY = user.Id;

                if (await _fSampleFabricIssue.Update(createFSampleFabricIssueViewModel.FSampleFabric))
                {
                    var fSampleFabricIssueDetailses = createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses.Where(e => e.SFIDID > 0).ToList();
                    var sampleFabricIssueDetailses = createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses.Where(e => e.SFIDID <= 0).ToList();

                    foreach (var item in createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses)
                    {
                        item.SFIID = fSampleFabricIssue.SFIID;
                    }

                    await _fSampleFabricIssueDetails.InsertRangeByAsync(sampleFabricIssueDetailses);
                    await _fSampleFabricIssueDetails.UpdateRangeByAsync(fSampleFabricIssueDetailses);

                    return RedirectToAction(nameof(GetFSampleFabric), "FSampleFabricIssue");
                }
            }

            return View(nameof(EditFSampleFabric), await _fSampleFabricIssue.GetInitObjByAsync(createFSampleFabricIssueViewModel));
        }

        [HttpGet]
        [Route("Edit/{sfiId?}")]
        public async Task<IActionResult> EditFSampleFabric(string sfiId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, "SampleFabricIssueForFactoryView");
            return View(authorizationResult.Succeeded ? "EditFSampleFabricForSample" : "EditFSampleFabric", await _fSampleFabricIssue.GetInitObjByAsync(await _fSampleFabricIssue.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(sfiId)))));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("IsSrNoInUse")]
        public async Task<IActionResult> IsSrNoInUse(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            if (createFSampleFabricIssueViewModel.FSampleFabric.MKT_TEAMID == null)
            {
                return Json($"Please select a Team Person");
            }

            return await _fSampleFabricIssue.IsSrNoInUseByAsync(createFSampleFabricIssueViewModel) ? Json($"SR No [{createFSampleFabricIssueViewModel.FSampleFabric.SRNO}] is already in use.") : Json(true);
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
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var data = (List<ExtendFSampleFabricIssueViewModel>)await _fSampleFabricIssue.GetAllForDataTableByAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }

                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SFIID.ToString().Contains(searchValue)
                                           || m.REQ_DATE.ToString(CultureInfo.InvariantCulture).ToUpper().Contains(searchValue)
                                           || m.ISSUE_DATE != null && m.ISSUE_DATE.ToString().ToUpper().Contains(searchValue)
                                           || m.SRNO != null && m.SRNO.ToUpper().Contains(searchValue)
                                           || m.BRAND.BRANDNAME != null && m.BRAND.BRANDNAME.ToUpper().Contains(searchValue)
                                           || m.MARCHANDISER_NAME != null && m.MARCHANDISER_NAME.ToUpper().Contains(searchValue)
                                           || m.BUYER.BUYER_NAME != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                           || m.MKT_TEAM.PERSON_NAME != null && m.MKT_TEAM.PERSON_NAME.ToUpper().Contains(searchValue)
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

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFSampleFabric(CreateFSampleFabricIssueViewModel fSampleFabricIssueViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var srNo = $"{await _fSampleFabricIssue.GetSrNoPrefixByAsync(fSampleFabricIssueViewModel)}{fSampleFabricIssueViewModel.FSampleFabric.SRNO}";

                if (await _fSampleFabricIssue.FindBySrNoAsync(srNo))
                {
                    ModelState.AddModelError("FSampleFabric.SRNO", "Duplicate SR No Found! Please reload the page or try again later.");
                    return View(nameof(CreateFSampleFabric), await _fSampleFabricIssue.GetInitObjByAsync(fSampleFabricIssueViewModel));
                }

                fSampleFabricIssueViewModel.FSampleFabric.SRNO = srNo;
                fSampleFabricIssueViewModel.FSampleFabric.CREATED_BY = fSampleFabricIssueViewModel.FSampleFabric.UPDATED_BY = user.Id;
                fSampleFabricIssueViewModel.FSampleFabric.CREATED_AT = fSampleFabricIssueViewModel.FSampleFabric.UPDATED_AT = DateTime.Now;

                var fSampleFabricIssue = await _fSampleFabricIssue.GetInsertedObjByAsync(fSampleFabricIssueViewModel.FSampleFabric);

                if (fSampleFabricIssue.SFIID > 0)
                {
                    foreach (var item in fSampleFabricIssueViewModel.FSampleFabricIssueDetailses)
                    {
                        item.SFIID = fSampleFabricIssue.SFIID;
                    }

                    await _fSampleFabricIssueDetails.InsertRangeByAsync(fSampleFabricIssueViewModel.FSampleFabricIssueDetailses);
                }

                return RedirectToAction(nameof(GetFSampleFabric), "FSampleFabricIssue");
            }

            return View(nameof(CreateFSampleFabric), await _fSampleFabricIssue.GetInitObjByAsync(fSampleFabricIssueViewModel));
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFSampleFabric()
        {
            var createFSampleFabricIssueViewModel = new CreateFSampleFabricIssueViewModel
            {
                FSampleFabric = new F_SAMPLE_FABRIC_ISSUE { REQ_DATE = DateTime.Now }
            };

            return View(await _fSampleFabricIssue.GetInitObjByAsync(createFSampleFabricIssueViewModel));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFSampleFabric()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports")]
        //[Authorize(Policy = "SampleFabricIssueForReport")]
        public IActionResult RFSampleFabric()
        {
            return View();
        }
    }
}
