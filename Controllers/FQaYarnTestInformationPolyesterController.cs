using System;
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
    public class FQaYarnTestInformationPolyesterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IF_QA_YARN_TEST_INFORMATION_POLYESTER _fQaYarnTestInformationPolyester;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string title = "Yarn Test Information (Polyester)";

        public FQaYarnTestInformationPolyesterController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_QA_YARN_TEST_INFORMATION_POLYESTER fQaYarnTestInformationPolyester,
            UserManager<ApplicationUser> userManager)
        {
            _fQaYarnTestInformationPolyester = fQaYarnTestInformationPolyester;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetCountDetails(int yRcvId)
        {
            try
            {
                return Ok(await _fQaYarnTestInformationPolyester.GetOtherDetailsOfYMaster(yRcvId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateFQaYarnTestInformationPolyester()
        {
            return View(await _fQaYarnTestInformationPolyester.GetInitObjByAsync(new FQaYarnTestInformationPolyesterViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFQaYarnTestInformationPolyester(FQaYarnTestInformationPolyesterViewModel fQaYarnTestInformationPolyesterViewModel)
        {
            if (ModelState.IsValid)
            {
                fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.CREATED_AT = fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.UPDATED_AT = DateTime.Now;
                fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.CREATED_BY = fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;

                if (await _fQaYarnTestInformationPolyester.InsertByAsync(fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(CreateFQaYarnTestInformationPolyester), "FQaYarnTestInformationPolyester");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(fQaYarnTestInformationPolyesterViewModel);
                }
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(fQaYarnTestInformationPolyesterViewModel);
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

            var data = await _fQaYarnTestInformationPolyester.GetAllAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.TESTDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.YRCV.CHALLANNO != null && m.YRCV.CHALLANNO.ToUpper().Contains(searchValue))
                                       || (m.DENIER_ACT != null && m.DENIER_ACT.ToString().ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                    ).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            foreach (var item in finalData)
            {
                item.EncryptedId = _protector.Protect(item.TESTID.ToString());
            }

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFQaYarnTestInformationPolyester(string testId)
        {
            var redirectToActionResult = RedirectToAction(nameof(CreateFQaYarnTestInformationPolyester), $"FQaYarnTestInformationPolyester");
            var testInfo = await _fQaYarnTestInformationPolyester.FindByIdAsync(int.Parse(_protector.Unprotect(testId)));

            if (testInfo != null)
            {
                if (await _fQaYarnTestInformationPolyester.Delete(testInfo))
                {
                    TempData["message"] = $"Successfully Deleted {title}.";
                    TempData["type"] = "success";
                    return redirectToActionResult;
                }

                TempData["message"] = $"Failed to Delete {title}.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = $"{title} Not Found.";
            TempData["type"] = "error";
            return redirectToActionResult;
        }

        [HttpGet]
        public async Task<IActionResult> EditFQaYarnTestInformationPolyester(string testId)
        {
            var redirectToActionResult = RedirectToAction(nameof(CreateFQaYarnTestInformationPolyester), $"FQaYarnTestInformationPolyester");
            try
            {
                var fQaYarnTestInformationPolyester = await _fQaYarnTestInformationPolyester.FindByIdAsync(int.Parse(_protector.Unprotect(testId)));

                if (fQaYarnTestInformationPolyester != null)
                {
                    var fQaYarnTestInformationPolyesterViewModel = await _fQaYarnTestInformationPolyester.GetInitObjByAsync(new FQaYarnTestInformationPolyesterViewModel());
                    fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester = fQaYarnTestInformationPolyester;

                    fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.EncryptedId = _protector.Protect(fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.TESTID.ToString());
                    return View(fQaYarnTestInformationPolyesterViewModel);
                }

                TempData["message"] = $"{title} Not Found";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return redirectToActionResult;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFQaYarnTestInformationPolyester(FQaYarnTestInformationPolyesterViewModel fQaYarnTestInformationPolyesterViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(CreateFQaYarnTestInformationPolyester), $"FQaYarnTestInformationPolyester");
                fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.TESTID = int.Parse(_protector.Unprotect(fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.EncryptedId));
                var fQaYarnTestInformationPolyester = await _fQaYarnTestInformationPolyester.FindByIdAsync(fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.TESTID);

                if (fQaYarnTestInformationPolyester != null)
                {
                    fQaYarnTestInformationPolyester.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fQaYarnTestInformationPolyester.UPDATED_AT = DateTime.Now;
                    fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.CREATED_BY =
                        fQaYarnTestInformationPolyester.CREATED_BY;
                    fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester.CREATED_AT =
                        fQaYarnTestInformationPolyester.CREATED_AT;

                    if (await _fQaYarnTestInformationPolyester.Update(fQaYarnTestInformationPolyesterViewModel.FQaYarnTestInformationPolyester))
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
            }
            return View(await _fQaYarnTestInformationPolyester.GetInitObjByAsync(new FQaYarnTestInformationPolyesterViewModel()));
        }
    }
}