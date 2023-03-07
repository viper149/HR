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
    public class RndFabTestSampleBulkController : Controller
    {
        private readonly IRND_FABTEST_SAMPLE_BULK _rndFabtestSampleBulk;
        private readonly IRND_SAMPLEINFO_FINISHING _rndSampleinfoFinishing;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Fabric Test Information (Sample-Bulk).";

        public RndFabTestSampleBulkController(IRND_FABTEST_SAMPLE_BULK rndFabtestSampleBulk, IRND_SAMPLEINFO_FINISHING rndSampleinfoFinishing,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _rndFabtestSampleBulk = rndFabtestSampleBulk;
            _rndSampleinfoFinishing = rndSampleinfoFinishing;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetRndFabTestSampleBulk()
        {
            return View();
        }

        [HttpPost]
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
            var data = (List<RND_FABTEST_SAMPLE_BULK>)await _rndFabtestSampleBulk.GetAllRndFabTestSampleBulkAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.LTSGDATE != null && m.LTSGDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.LABNO != null && m.LABNO.ToUpper().Contains(searchValue))
                                       || (m.PROGNO != null && m.PROGNO.ToUpper().Contains(searchValue))
                                       || (m.EMPWASHBY.FIRST_NAME != null && m.EMPWASHBY.FIRST_NAME.ToUpper().Contains(searchValue))
                                       || (m.EMPUNWASHBY.FIRST_NAME != null && m.EMPUNWASHBY.FIRST_NAME.ToUpper().Contains(searchValue))
                                       || (m.COMMENTS != null && m.COMMENTS.ToUpper().Contains(searchValue))
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

        [HttpGet]
        public async Task<IActionResult> CreateRndFabTestSampleBulk()
        {
            return View(await _rndFabtestSampleBulk.GetInitObjByAsync(new RndFabTestSampleBulkViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndFabTestSampleBulk(RndFabTestSampleBulkViewModel rndFabTestSampleBulkViewModel)
        {
            if (ModelState.IsValid)
            {
                rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.CREATED_AT = rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.UPDATED_AT = DateTime.Now;
                rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.CREATED_BY = rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _rndFabtestSampleBulk.InsertByAsync(rndFabTestSampleBulkViewModel.RndFabtestSampleBulk))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetRndFabTestSampleBulk), "RndFabTestSampleBulk");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(rndFabTestSampleBulkViewModel);
                }
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _rndFabtestSampleBulk.GetInitObjByAsync(new RndFabTestSampleBulkViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditRndFabTestSampleBulk(string ltsgId)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetRndFabTestSampleBulk), $"RndFabTestSampleBulk");
            var rndFabtestSampleBulk = await _rndFabtestSampleBulk.FindByIdAsync(int.Parse(_protector.Unprotect(ltsgId)));

            if (rndFabtestSampleBulk != null)
            {
                var rndFabTestSampleBulkViewModel = await _rndFabtestSampleBulk.GetInitObjByAsync(new RndFabTestSampleBulkViewModel());
                rndFabTestSampleBulkViewModel.RndFabtestSampleBulk = rndFabtestSampleBulk;

                rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.EncryptedId = _protector.Protect(rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.LTSGID.ToString());
                return View(rndFabTestSampleBulkViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return redirectToActionResult;
        }

        [HttpPost]
        public async Task<IActionResult> EditRndFabTestSampleBulk(RndFabTestSampleBulkViewModel rndFabTestSampleBulkViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetRndFabTestSampleBulk), $"RndFabTestSampleBulk");
                rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.LTSGID = int.Parse(_protector.Unprotect(rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.EncryptedId));
                var rndFabtestSampleBulk = await _rndFabtestSampleBulk.FindByIdAsync(rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.LTSGID);
                if (rndFabtestSampleBulk != null)
                {
                    rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.UPDATED_AT = DateTime.Now;
                    rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.CREATED_AT = rndFabtestSampleBulk.CREATED_AT;
                    rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.CREATED_BY = rndFabtestSampleBulk.CREATED_BY;

                    if (await _rndFabtestSampleBulk.Update(rndFabTestSampleBulkViewModel.RndFabtestSampleBulk))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{title} Not Found";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(await _rndFabtestSampleBulk.GetInitObjByAsync(new RndFabTestSampleBulkViewModel()));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetProgNoBySfnIdAsync(int sfnId)
        {
            try
            {
                return Ok(await _rndSampleinfoFinishing.GetProgNoBySfnIdAsync(sfnId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsLabNoInUse(RndFabTestSampleBulkViewModel rndFabTestSampleBulkViewModel)
        {
            var labNo = rndFabTestSampleBulkViewModel.RndFabtestSampleBulk.LABNO;
            return await _rndFabtestSampleBulk.FindByLabNo(labNo) ? Json(true) : Json($"Lab No {labNo} is already in use");
        }
    }
}
