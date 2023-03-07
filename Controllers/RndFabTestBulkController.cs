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
    public class RndFabTestBulkController : Controller
    {
        private readonly IRND_FABTEST_BULK _rndFabtestBulk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Fabric Test Information (Bulk).";

        public RndFabTestBulkController(IRND_FABTEST_BULK rndFabtestBulk,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _rndFabtestBulk = rndFabtestBulk;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
        [HttpGet]
        public IActionResult GetRndFabTestBulk()
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
            var data = (List<RND_FABTEST_BULK>)await _rndFabtestBulk.GetAllRndFabTestBulkAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m =>(m.LTB_DATE != null && m.LTB_DATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.LAB_NO != null && m.LAB_NO.ToUpper().Contains(searchValue))
                                       || (m.PROG.PROG_.PROG_NO != null && m.PROG.PROG_.PROG_NO.ToUpper().Contains(searchValue))
                                       || (m.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME != null && m.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue))
                                       || (m.FINPROC.TROLLNONavigation.NAME != null && m.FINPROC.TROLLNONavigation.NAME.ToUpper().Contains(searchValue))
                                       || (m.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME != null && m.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> CreateRndFabTestBulk()
        {
            return View(await _rndFabtestBulk.GetInitObjByAsync(new RndFabTestBulkViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndFabTestBulk(RndFabTestBulkViewModel rndFabTestBulkViewModel)
        {
            if (ModelState.IsValid)
            {
                rndFabTestBulkViewModel.RndFabtestBulk.CREATED_AT = rndFabTestBulkViewModel.RndFabtestBulk.UPDATED_AT = DateTime.Now;
                rndFabTestBulkViewModel.RndFabtestBulk.CREATED_BY = rndFabTestBulkViewModel.RndFabtestBulk.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _rndFabtestBulk.InsertByAsync(rndFabTestBulkViewModel.RndFabtestBulk))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetRndFabTestBulk), "RndFabTestBulk");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(rndFabTestBulkViewModel);
                }
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _rndFabtestBulk.GetInitObjByAsync(new RndFabTestBulkViewModel()));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsLabNoInUse(RndFabTestBulkViewModel rndFabTestBulkViewModel)
        {
            var labNo = rndFabTestBulkViewModel.RndFabtestBulk.LAB_NO;
            return await _rndFabtestBulk.FindByLabNo(labNo) ? Json(true) : Json($"Lab No {labNo} is already in use");
        }

        [HttpGet]
        public async Task<IActionResult> EditRndFabTestBulk(string ltbId)
        {
            var rndFabtestBulk = await _rndFabtestBulk.FindByIdAsync(int.Parse(_protector.Unprotect(ltbId)));

            if (rndFabtestBulk != null)
            {
                var rndFabTestBulkViewModel = await _rndFabtestBulk.GetInitObjByAsync(new RndFabTestBulkViewModel());
                rndFabTestBulkViewModel.RndFabtestBulk = rndFabtestBulk;

                rndFabTestBulkViewModel.RndFabtestBulk.EncryptedId = _protector.Protect(rndFabTestBulkViewModel.RndFabtestBulk.LTBID.ToString());
                return View(rndFabTestBulkViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetRndFabTestBulk), $"RndFabTestBulk");
        }

        [HttpPost]
        public async Task<IActionResult> EditRndFabTestBulk(RndFabTestBulkViewModel rndFabTestBulkViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetRndFabTestBulk), $"RndFabTestBulk");
                rndFabTestBulkViewModel.RndFabtestBulk.LTBID = int.Parse(_protector.Unprotect(rndFabTestBulkViewModel.RndFabtestBulk.EncryptedId));
                var rndFabtestBulk = await _rndFabtestBulk.FindByIdAsync(rndFabTestBulkViewModel.RndFabtestBulk.LTBID);
                if (rndFabtestBulk!=null)
                {
                    rndFabTestBulkViewModel.RndFabtestBulk.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    rndFabTestBulkViewModel.RndFabtestBulk.UPDATED_AT = DateTime.Now;
                    rndFabTestBulkViewModel.RndFabtestBulk.CREATED_AT = rndFabtestBulk.CREATED_AT;
                    rndFabTestBulkViewModel.RndFabtestBulk.CREATED_BY = rndFabtestBulk.CREATED_BY;

                    if (await _rndFabtestBulk.Update(rndFabTestBulkViewModel.RndFabtestBulk))
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

            return View(await _rndFabtestBulk.GetInitObjByAsync(new RndFabTestBulkViewModel()));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetSetDetailsBySetId(string setId)
        {
            return Ok(await _rndFabtestBulk.GetSetDetailsAsync(int.Parse(setId)));
        }
        
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetFnProcessDetailsById(string id)
        {
            return Ok(await _rndFabtestBulk.GetFnProcessDetailsAsync(int.Parse(id)));
        }
    }
}
