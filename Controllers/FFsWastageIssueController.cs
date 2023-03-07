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
    public class FFsWastageIssueController : Controller
    {
        private readonly IF_FS_WASTAGE_ISSUE_M _fFsWastageIssueM;
        private readonly IF_FS_WASTAGE_ISSUE_D _fFsWastageIssueD;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Fabric Wastage Issue Information";

        public FFsWastageIssueController(IDataProtectionProvider dataProtectionProvider,
            IF_FS_WASTAGE_ISSUE_M fFsWastageIssueM,
            IF_FS_WASTAGE_ISSUE_D fFsWastageIssueD,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fFsWastageIssueM = fFsWastageIssueM;
            _fFsWastageIssueD = fFsWastageIssueD;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFFsWastageIssueInfo()
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

            var data = (List<F_FS_WASTAGE_ISSUE_M>)await _fFsWastageIssueM.GetAllFFsWastageIssueAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.WIDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.FFsWastageParty.PNAME.ToString().ToUpper().Contains(searchValue)
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
        public async Task<IActionResult> CreateFFsWastageIssueInfo()
        {
            try
            {
                return View(await _fFsWastageIssueM.GetInitObjByAsync(new FFsWastageIssueViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateFFsWastageIssueInfo(FFsWastageIssueViewModel fFsWastageIssueViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fFsWastageIssueViewModel.FFsWastageIssueM.CREATED_BY = user.Id;
                    fFsWastageIssueViewModel.FFsWastageIssueM.UPDATED_BY = user.Id;
                    fFsWastageIssueViewModel.FFsWastageIssueM.CREATED_AT = DateTime.Now;
                    fFsWastageIssueViewModel.FFsWastageIssueM.UPDATED_AT = DateTime.Now;

                    var fFsWastageIssueM = await _fFsWastageIssueM.GetInsertedObjByAsync(fFsWastageIssueViewModel.FFsWastageIssueM);
                    if (fFsWastageIssueM != null)
                    {
                        foreach (var item in fFsWastageIssueViewModel.FFsWastageIssueDList)
                        {
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.WIID = fFsWastageIssueM.WIID;
                            await _fFsWastageIssueD.InsertByAsync(item);
                        }

                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageIssueInfo), $"FFsWastageIssue");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fFsWastageIssueViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fFsWastageIssueViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fFsWastageIssueViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFFsWastageIssueInfo(string fwiId)
        {
            return View(await _fFsWastageIssueM.GetInitObjByAsync(await _fFsWastageIssueM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(fwiId)))));
        }

        [HttpPost]
        public async Task<IActionResult> EditFFsWastageIssueInfo(FFsWastageIssueViewModel fFsWastageIssueViewModel)
        {
            if (ModelState.IsValid)
            {
                var fGsWastageIssueM = await _fFsWastageIssueM.FindByIdAsync(int.Parse(_protector.Unprotect(fFsWastageIssueViewModel.FFsWastageIssueM.EncryptedId)));

                if (fGsWastageIssueM != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fFsWastageIssueViewModel.FFsWastageIssueM.WIID =
                        fGsWastageIssueM.WIID;
                    fFsWastageIssueViewModel.FFsWastageIssueM.CREATED_AT = fGsWastageIssueM.CREATED_AT;
                    fFsWastageIssueViewModel.FFsWastageIssueM.CREATED_BY = fGsWastageIssueM.CREATED_BY;
                    fFsWastageIssueViewModel.FFsWastageIssueM.UPDATED_AT = DateTime.Now;
                    fFsWastageIssueViewModel.FFsWastageIssueM.UPDATED_BY = user.Id;

                    if (await _fFsWastageIssueM.Update(fFsWastageIssueViewModel.FFsWastageIssueM))
                    {

                        foreach (var item in fFsWastageIssueViewModel.FFsWastageIssueDList.Where(d => d.TRNSID <= 0).ToList())
                        {
                            item.WIID = fGsWastageIssueM.WIID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            await _fFsWastageIssueD.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageIssueInfo), $"FFsWastageIssue");


                    }
                }
            }

            TempData["message"] = $"Failed to Add {title}.";
            TempData["type"] = "error";
            return View(await _fFsWastageIssueM.GetInitObjByAsync((fFsWastageIssueViewModel)));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetAllByfwpId(int fwpId)
        {
            try
            {
                return Ok(await _fFsWastageIssueD.GetAllByfwpIdAsync(fwpId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        //[Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddFfsWastageIssueDetails(FFsWastageIssueViewModel fFsWastageIssueViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fFsWastageIssueViewModel.IsDelete)
                {
                    var fGsWastageIssueD = fFsWastageIssueViewModel.FFsWastageIssueDList
                        [fFsWastageIssueViewModel.RemoveIndex];

                    if (fGsWastageIssueD.TRNSID > 0)
                    {
                        await _fFsWastageIssueD.Delete(fGsWastageIssueD);
                    }

                    fFsWastageIssueViewModel.FFsWastageIssueDList.RemoveAt(fFsWastageIssueViewModel.RemoveIndex);
                }
                else if (!fFsWastageIssueViewModel.FFsWastageIssueDList.Any(e => e.WPID.Equals(fFsWastageIssueViewModel.FFsWastageIssueD.WPID)))
                {
                    if (TryValidateModel(fFsWastageIssueViewModel.FFsWastageIssueD))
                    {
                        fFsWastageIssueViewModel.FFsWastageIssueDList.Add(fFsWastageIssueViewModel.FFsWastageIssueD);
                    }
                }

                Response.Headers["HasItems"] = $"{fFsWastageIssueViewModel.FFsWastageIssueDList.Any()}";

                return PartialView($"FFsWastageIssuePartialView", await _fFsWastageIssueD.GetInitObjForDetailsByAsync(fFsWastageIssueViewModel));
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }


    }
}
