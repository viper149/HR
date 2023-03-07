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
    public class FGsWastageReceiveController : Controller
    {
        private readonly IF_GS_WASTAGE_RECEIVE_M _fGsWastageReceiveM;
        private readonly IF_GS_WASTAGE_RECEIVE_D _fGsWastageReceiveD;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " General Store Wastage Receive Information";

        public FGsWastageReceiveController(IDataProtectionProvider dataProtectionProvider,
            IF_GS_WASTAGE_RECEIVE_M fGsWastageReceiveM,
            IF_GS_WASTAGE_RECEIVE_D fGsWastageReceiveD,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fGsWastageReceiveM = fGsWastageReceiveM;
            _fGsWastageReceiveD = fGsWastageReceiveD;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFGsWastageReceiveInfo()
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

            var data = (List<F_GS_WASTAGE_RECEIVE_M>)await _fGsWastageReceiveM.GetAllFGsWastageReceiveAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.WRDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.SEC.SECNAME.ToUpper().Contains(searchValue)
                                       || m.WTRNO.ToString().ToUpper().Contains(searchValue)
                                       || m.WTRDATE.ToString().ToUpper().Contains(searchValue)
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
        public async Task<IActionResult> CreateFGsWastageReceiveInfo()
        {
            try
            {
                return View(await _fGsWastageReceiveM.GetInitObjByAsync(new FGsWastageReceiveViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsWastageReceiveInfo(FGsWastageReceiveViewModel fGsWastageReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.CREATED_BY = user.Id;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.UPDATED_BY = user.Id;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.CREATED_AT = DateTime.Now;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.UPDATED_AT = DateTime.Now;

                    var result = await _fGsWastageReceiveM.GetInsertedObjByAsync(fGsWastageReceiveViewModel.FGsWastageReceiveM);

                    if (result != null)
                    {
                        foreach (var item in fGsWastageReceiveViewModel.FGsWastageReceiveDList)
                        {
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.WRID = result.WRID;
                            await _fGsWastageReceiveD.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Added {title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsWastageReceiveInfo), $"FGsWastageReceive");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fGsWastageReceiveViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fGsWastageReceiveViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fGsWastageReceiveViewModel);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetAllBywpId(int wpId)
        {
            try
            {
                return Ok(await _fGsWastageReceiveD.GetAllBywpIdAsync(wpId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsWastageReceiveInfo(string wrId)
        {
            return View(await _fGsWastageReceiveM.GetInitObjByAsync(await _fGsWastageReceiveM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(wrId)))));
        }

        [HttpPost]
        public async Task<IActionResult> EditFGsWastageReceiveInfo(FGsWastageReceiveViewModel fGsWastageReceiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var fGsWastageReceiveM = await _fGsWastageReceiveM.FindByIdAsync(int.Parse(_protector.Unprotect(fGsWastageReceiveViewModel.FGsWastageReceiveM.EncryptedId)));

                if (fGsWastageReceiveM != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.WRID =
                        fGsWastageReceiveM.WRID;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.CREATED_AT = fGsWastageReceiveM.CREATED_AT;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.CREATED_BY = fGsWastageReceiveM.CREATED_BY;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.UPDATED_AT = DateTime.Now;
                    fGsWastageReceiveViewModel.FGsWastageReceiveM.UPDATED_BY = user.Id;

                    if (await _fGsWastageReceiveM.Update(fGsWastageReceiveViewModel.FGsWastageReceiveM))
                    {

                        foreach (var item in fGsWastageReceiveViewModel.FGsWastageReceiveDList.Where(d => d.TRNSID <= 0).ToList())
                        {
                            item.WRID = fGsWastageReceiveM.WRID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            await _fGsWastageReceiveD.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsWastageReceiveInfo), $"FGsWastageReceive");

                        
                    }
                }
            }

            TempData["message"] = $"Failed to Add {title}.";
            TempData["type"] = "error";
            return View(await _fGsWastageReceiveM.GetInitObjByAsync((fGsWastageReceiveViewModel)));
        }
        [HttpPost]
        //[Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddFgsWastageRcvDetails(FGsWastageReceiveViewModel fGsWastageReceiveViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGsWastageReceiveViewModel.IsDelete)
                {
                    var fGsWastageReceiveD = fGsWastageReceiveViewModel.FGsWastageReceiveDList
                        [fGsWastageReceiveViewModel.RemoveIndex];

                    if (fGsWastageReceiveD.TRNSID > 0)
                    {
                        await _fGsWastageReceiveD.Delete(fGsWastageReceiveD);
                    }

                    fGsWastageReceiveViewModel.FGsWastageReceiveDList.RemoveAt(fGsWastageReceiveViewModel.RemoveIndex);
                }
                else if (!fGsWastageReceiveViewModel.FGsWastageReceiveDList.Any(e => e.WPID.Equals(fGsWastageReceiveViewModel.FGsWastageReceiveD.WPID)))
                {
                    if (TryValidateModel(fGsWastageReceiveViewModel.FGsWastageReceiveD))
                    {
                        fGsWastageReceiveViewModel.FGsWastageReceiveDList.Add(fGsWastageReceiveViewModel.FGsWastageReceiveD);
                    }
                }

                Response.Headers["HasItems"] = $"{fGsWastageReceiveViewModel.FGsWastageReceiveDList.Any()}";

                return PartialView($"FGsWastageReceivePartialView", await _fGsWastageReceiveD.GetInitObjForDetailsByAsync(fGsWastageReceiveViewModel));
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }


    }
}
