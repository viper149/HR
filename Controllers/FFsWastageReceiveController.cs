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
    public class FFsWastageReceiveController : Controller
    {
        private readonly IF_FS_WASTAGE_RECEIVE_M _fFsWastageReceiveM;
        private readonly IF_FS_WASTAGE_RECEIVE_D _fFsWastageReceiveD;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " Fabric Wastage Receive Information";

        public FFsWastageReceiveController(IDataProtectionProvider dataProtectionProvider,
            IF_FS_WASTAGE_RECEIVE_M fFsWastageReceiveM,
            IF_FS_WASTAGE_RECEIVE_D fFsWastageReceiveD,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
            )
        {
            _fFsWastageReceiveM = fFsWastageReceiveM;
            _fFsWastageReceiveD = fFsWastageReceiveD;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);

        }
        [HttpGet]
        public IActionResult GetFFsWastageReceiveInfo()
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

            var data = (List<F_FS_WASTAGE_RECEIVE_M>)await _fFsWastageReceiveM.GetAllFFsWastageReceiveAsync();

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
        public async Task<IActionResult> CreateFFsWastageReceiveInfo()
        {
            try
            {
                return View(await _fFsWastageReceiveM.GetInitObjByAsync(new FFsWastageReceiveViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFFsWastageReceiveInfo(FFsWastageReceiveViewModel fFsWastageReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.CREATED_BY = user.Id;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.UPDATED_BY = user.Id;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.CREATED_AT = DateTime.Now;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.UPDATED_AT = DateTime.Now;

                    var result = await _fFsWastageReceiveM.GetInsertedObjByAsync(fFsWastageReceiveViewModel.FFsWastageReceiveM);

                    if (result != null)
                    {
                        foreach (var item in fFsWastageReceiveViewModel.FFsWastageReceiveDList)
                        {
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.WRID = result.WRID;
                            await _fFsWastageReceiveD.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Added {title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageReceiveInfo), $"FFsWastageReceive");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fFsWastageReceiveViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fFsWastageReceiveViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fFsWastageReceiveViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFFsWastageReceiveInfo(string fwrId)
        {
            return View(await _fFsWastageReceiveM.GetInitObjByAsync(await _fFsWastageReceiveM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(fwrId)))));
        }

        [HttpPost]
        public async Task<IActionResult> EditFFsWastageReceiveInfo(FFsWastageReceiveViewModel fFsWastageReceiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var fFsWastageReceiveM = await _fFsWastageReceiveM.FindByIdAsync(int.Parse(_protector.Unprotect(fFsWastageReceiveViewModel.FFsWastageReceiveM.EncryptedId)));

                if (fFsWastageReceiveM != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.WRID =
                        fFsWastageReceiveM.WRID;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.CREATED_AT = fFsWastageReceiveM.CREATED_AT;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.CREATED_BY = fFsWastageReceiveM.CREATED_BY;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.UPDATED_AT = DateTime.Now;
                    fFsWastageReceiveViewModel.FFsWastageReceiveM.UPDATED_BY = user.Id;

                    if (await _fFsWastageReceiveM.Update(fFsWastageReceiveViewModel.FFsWastageReceiveM))
                    {

                        foreach (var item in fFsWastageReceiveViewModel.FFsWastageReceiveDList.Where(d => d.TRNSID <= 0).ToList())
                        {
                            item.WRID = fFsWastageReceiveM.WRID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            await _fFsWastageReceiveD.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageReceiveInfo), $"FFsWastageReceive");


                    }
                }
            }

            TempData["message"] = $"Failed to Add {title}.";
            TempData["type"] = "error";
            return View(await _fFsWastageReceiveM.GetInitObjByAsync((fFsWastageReceiveViewModel)));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetAllBywpId(int fwpId)
        {
            try
            {
                return Ok(await _fFsWastageReceiveD.GetAllBywpIdAsync(fwpId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        //[Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddFfsWastageRcvDetails(FFsWastageReceiveViewModel fFsWastageReceiveViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fFsWastageReceiveViewModel.IsDelete)
                {
                    var fGsWastageReceiveD = fFsWastageReceiveViewModel.FFsWastageReceiveDList
                        [fFsWastageReceiveViewModel.RemoveIndex];

                    if (fGsWastageReceiveD.TRNSID > 0)
                    {
                        await _fFsWastageReceiveD.Delete(fGsWastageReceiveD);
                    }

                    fFsWastageReceiveViewModel.FFsWastageReceiveDList.RemoveAt(fFsWastageReceiveViewModel.RemoveIndex);
                }
                else if (!fFsWastageReceiveViewModel.FFsWastageReceiveDList.Any(e => e.WPID.Equals(fFsWastageReceiveViewModel.FFsWastageReceiveD.WPID)))
                {
                    if (TryValidateModel(fFsWastageReceiveViewModel.FFsWastageReceiveD))
                    {
                        fFsWastageReceiveViewModel.FFsWastageReceiveDList.Add(fFsWastageReceiveViewModel.FFsWastageReceiveD);
                    }
                }

                Response.Headers["HasItems"] = $"{fFsWastageReceiveViewModel.FFsWastageReceiveDList.Any()}";

                return PartialView($"FfsWastageReceivePartialView", await _fFsWastageReceiveD.GetInitObjForDetailsByAsync(fFsWastageReceiveViewModel));
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }

    }
}
