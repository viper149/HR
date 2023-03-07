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
    public class FGsWastageIssueController : Controller
    {
        private readonly IF_GS_WASTAGE_ISSUE_M _fGsWastageIssueM;
        private readonly IF_GS_WASTAGE_ISSUE_D _fGsWastageIssueD;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " General Store Wastage Issue Information";

        public FGsWastageIssueController(IDataProtectionProvider dataProtectionProvider,
            IF_GS_WASTAGE_ISSUE_M fGsWastageIssueM,
            IF_GS_WASTAGE_ISSUE_D fGsWastageIssueD,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fGsWastageIssueM = fGsWastageIssueM;
            _fGsWastageIssueD = fGsWastageIssueD;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFGsWastageIssueInfo()
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
        //[Route("GetTableData")]
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

            var data = (List<F_GS_WASTAGE_ISSUE_M>)await _fGsWastageIssueM.GetAllFGsWastageIssueAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.WIDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.FGsWastageParty.PNAME.ToString().ToUpper().Contains(searchValue)
                                       || m.GPNO.ToString().ToUpper().Contains(searchValue)
                                       || m.GPDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.VEHICLENO.ToString().ToUpper().Contains(searchValue)
                                       || m.THROUGH.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
        //[Route("Create")]
        public async Task<IActionResult> CreateFGsWastageIssueInfo()
        {
            try
            {
                return View(await _fGsWastageIssueM.GetInitObjByAsync(new FGsWastageIssueViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsWastageIssueInfo(FGsWastageIssueViewModel fGsWastageIssueViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fGsWastageIssueViewModel.FGsWastageIssueM.CREATED_BY = user.Id;
                    fGsWastageIssueViewModel.FGsWastageIssueM.UPDATED_BY = user.Id;
                    fGsWastageIssueViewModel.FGsWastageIssueM.CREATED_AT = DateTime.Now;
                    fGsWastageIssueViewModel.FGsWastageIssueM.UPDATED_AT = DateTime.Now;

                    var result = await _fGsWastageIssueM.GetInsertedObjByAsync(fGsWastageIssueViewModel.FGsWastageIssueM);

                    if (result != null)
                    {
                        foreach (var item in fGsWastageIssueViewModel.FGsWastageIssueDList)
                        {
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.WIID = result.WIID;
                            await _fGsWastageIssueD.InsertByAsync(item);
                        }

                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsWastageIssueInfo), $"FGsWastageIssue");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fGsWastageIssueViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fGsWastageIssueViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fGsWastageIssueViewModel);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetAllBywpId(int wpId)
        {
            try
            {
                return Ok(await _fGsWastageIssueD.GetAllBywpIdAsync(wpId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        //[Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddFgsWastageIssueDetails(FGsWastageIssueViewModel fGsWastageIssueViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGsWastageIssueViewModel.IsDelete)
                {
                    var fGsWastageIssueD = fGsWastageIssueViewModel.FGsWastageIssueDList
                        [fGsWastageIssueViewModel.RemoveIndex];

                    if (fGsWastageIssueD.TRNSID > 0)
                    {
                        await _fGsWastageIssueD.Delete(fGsWastageIssueD);
                    }

                    fGsWastageIssueViewModel.FGsWastageIssueDList.RemoveAt(fGsWastageIssueViewModel.RemoveIndex);
                }
                else if (!fGsWastageIssueViewModel.FGsWastageIssueDList.Any(e => e.WPID.Equals(fGsWastageIssueViewModel.FGsWastageIssueD.WPID)))
                {
                    if (TryValidateModel(fGsWastageIssueViewModel.FGsWastageIssueD))
                    {
                        fGsWastageIssueViewModel.FGsWastageIssueDList.Add(fGsWastageIssueViewModel.FGsWastageIssueD);
                    }
                }

                Response.Headers["HasItems"] = $"{fGsWastageIssueViewModel.FGsWastageIssueDList.Any()}";

                return PartialView($"FGsWastageIssuePartialView", await _fGsWastageIssueD.GetInitObjForDetailsByAsync(fGsWastageIssueViewModel));
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsWastageIssueInfo(string wiId)
        {
            return View(await _fGsWastageIssueM.GetInitObjByAsync(await _fGsWastageIssueM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(wiId)))));
        }

        [HttpPost]
        public async Task<IActionResult>EditFGsWastageIssueInfo(FGsWastageIssueViewModel fGsWastageIssueViewModel)
        {
            if (ModelState.IsValid)
            {
                var fGsWastageIssueM = await _fGsWastageIssueM.FindByIdAsync(int.Parse(_protector.Unprotect(fGsWastageIssueViewModel.FGsWastageIssueM.EncryptedId)));

                if (fGsWastageIssueM != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fGsWastageIssueViewModel.FGsWastageIssueM.WIID =
                        fGsWastageIssueM.WIID;
                    fGsWastageIssueViewModel.FGsWastageIssueM.CREATED_AT = fGsWastageIssueM.CREATED_AT;
                    fGsWastageIssueViewModel.FGsWastageIssueM.CREATED_BY = fGsWastageIssueM.CREATED_BY;
                    fGsWastageIssueViewModel.FGsWastageIssueM.UPDATED_AT = DateTime.Now;
                    fGsWastageIssueViewModel.FGsWastageIssueM.UPDATED_BY = user.Id;

                    if (await _fGsWastageIssueM.Update(fGsWastageIssueViewModel.FGsWastageIssueM))
                    {

                        foreach (var item in fGsWastageIssueViewModel.FGsWastageIssueDList.Where(d => d.TRNSID <= 0).ToList())
                        {
                            item.WIID = fGsWastageIssueM.WIID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            await _fGsWastageIssueD.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsWastageIssueInfo), $"FGsWastageIssue");


                    }
                }
            }

            TempData["message"] = $"Failed to Add {title}.";
            TempData["type"] = "error";
            return View(await _fGsWastageIssueM.GetInitObjByAsync((fGsWastageIssueViewModel)));
        }
    }
}
