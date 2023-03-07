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
    [Route("ClearanceMasterSampleRoll")]
    public class FFsClearanceMasterSampleRollController : Controller
    {
        private readonly IF_FS_CLEARANCE_MASTER_SAMPLE_ROLL _fFsClearanceMasterSampleRoll;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Master Sample Roll Declaration";

        public FFsClearanceMasterSampleRollController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_FS_CLEARANCE_MASTER_SAMPLE_ROLL fFsClearanceMasterSampleRoll,
            UserManager<ApplicationUser> userManager)
        {
            _fFsClearanceMasterSampleRoll = fFsClearanceMasterSampleRoll;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFFsClearanceMasterSampleRoll()
        {
            return View();
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

            var data = (List<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>)await _fFsClearanceMasterSampleRoll
                .GetAllClearanceMasterSampleRollAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.MSRDATE != null && m.MSRDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.MAILDATE != null && m.MAILDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.SETID != null && m.SET.PROG_.PROG_NO.ToUpper().Contains(searchValue)
                                       || m.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME != null && m.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue)
                                       || m.ROLLID != null && m.ROLL.ROLLNO.ToUpper().Contains(searchValue)
                                       || m.WTID != null && m.WT.WTNAME.ToUpper().Contains(searchValue)
                                       || m.RTID != null && m.RT.RTNAME.ToUpper().Contains(searchValue))
                    .ToList();
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
        public async Task<IActionResult> CreateFFsClearanceMasterSampleRoll()
        {
            return View(await _fFsClearanceMasterSampleRoll.GetInitObjByAsync(new FFsClearanceMasterSampleRollViewModel()));

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFFsClearanceMasterSampleRoll(FFsClearanceMasterSampleRollViewModel fFsClearanceMasterSampleRollViewModel)
        {
            if (ModelState.IsValid)
            {
                fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.CREATED_AT = fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.UPDATED_AT = DateTime.Now;
                fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.CREATED_BY = fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fFsClearanceMasterSampleRoll.InsertByAsync(fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFFsClearanceMasterSampleRoll), "FFsClearanceMasterSampleRoll");
                }
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(await _fFsClearanceMasterSampleRoll.GetInitObjByAsync(new FFsClearanceMasterSampleRollViewModel()));
            }
            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _fFsClearanceMasterSampleRoll.GetInitObjByAsync(new FFsClearanceMasterSampleRollViewModel()));
        }

        [HttpGet]
        [Route("Edit/{msrId?}")]
        public async Task<IActionResult> EditFFsClearanceMasterSampleRoll(string msrId)
        {
            var fFsClearanceMasterSampleRoll = await _fFsClearanceMasterSampleRoll.FindByIdAsync(int.Parse(_protector.Unprotect(msrId)));

            var fFsClearanceMasterSampleRollViewModel = new FFsClearanceMasterSampleRollViewModel();
            if (fFsClearanceMasterSampleRoll != null)
            {
                fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll = fFsClearanceMasterSampleRoll;
                fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.EncryptedId = _protector.Protect(fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.MSRID.ToString());
                return View(await _fFsClearanceMasterSampleRoll.GetInitObjByAsync(fFsClearanceMasterSampleRollViewModel));
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return View(fFsClearanceMasterSampleRollViewModel);
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFFsClearanceMasterSampleRoll(FFsClearanceMasterSampleRollViewModel fFsClearanceMasterSampleRollViewModel)
        {
            if (ModelState.IsValid)
            {
                var fFsClearanceMasterSampleRoll = await _fFsClearanceMasterSampleRoll.FindByIdAsync(int.Parse(_protector.Unprotect(fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.EncryptedId)));
                if (fFsClearanceMasterSampleRoll != null)
                {
                    fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.UPDATED_AT = DateTime.Now;
                    fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.CREATED_AT = fFsClearanceMasterSampleRoll.CREATED_AT;
                    fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll.CREATED_BY = fFsClearanceMasterSampleRoll.CREATED_BY;

                    if (await _fFsClearanceMasterSampleRoll.Update(fFsClearanceMasterSampleRollViewModel.FFsClearanceMasterSampleRoll))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsClearanceMasterSampleRoll), "FFsClearanceMasterSampleRoll");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(nameof(EditFFsClearanceMasterSampleRoll));
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(nameof(EditFFsClearanceMasterSampleRoll));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(nameof(EditFFsClearanceMasterSampleRoll));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetRollDetails/{rollId?}")]
        public async Task<IActionResult> GetRollDetailsByRoleId(int rollId)
        {
            try
            {
                return Ok(await _fFsClearanceMasterSampleRoll.GetRollDetailsAsync(rollId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetSetDetails/{setId?}")]
        public async Task<IActionResult> GetSetDetailsBySetId(string setId)
        {
            return Ok(await _fFsClearanceMasterSampleRoll.GetSetDetailsAsync(int.Parse(setId)));
        }
    }
}
