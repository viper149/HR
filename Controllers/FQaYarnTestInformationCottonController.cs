using System;
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
    public class FQaYarnTestInformationCottonController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IF_QA_YARN_TEST_INFORMATION_COTTON _fQaYarnTestInformationCotton;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string title = "Yarn Test Information (Cotton)";

        public FQaYarnTestInformationCottonController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_QA_YARN_TEST_INFORMATION_COTTON fQaYarnTestInformationCotton,
            UserManager<ApplicationUser> userManager)
        {
            _fQaYarnTestInformationCotton = fQaYarnTestInformationCotton;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetCountDetails(int yRcvId)
        {
            try
            {
                return Ok(await _fQaYarnTestInformationCotton.GetOtherDetailsOfYMaster(yRcvId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateFQaYarnTestInformationCotton()
        {
            return View(await _fQaYarnTestInformationCotton.GetInitObjByAsync(new FQaYarnTestInformationCottonViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFQaYarnTestInformationCotton(FQaYarnTestInformationCottonViewModel fQaYarnTestInformationCottonViewModel)
        {
            if (ModelState.IsValid)
            {
                fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.CREATED_AT = fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.UPDATED_AT = DateTime.Now;
                fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.CREATED_BY = fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fQaYarnTestInformationCotton.InsertByAsync(fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(CreateFQaYarnTestInformationCotton), "FQaYarnTestInformationCotton");
                }

                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(fQaYarnTestInformationCottonViewModel);
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _fQaYarnTestInformationCotton.GetInitObjByAsync(new FQaYarnTestInformationCottonViewModel()));
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

            var data = await _fQaYarnTestInformationCotton.GetAllAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.TESTDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.YRCV.CHALLANNO != null && m.YRCV.CHALLANNO.ToUpper().Contains(searchValue))
                                       || (m.COUNT_ACT != null && m.COUNT_ACT.ToString().ToUpper().Contains(searchValue))
                                       || (m.COUNT_CV != null && m.COUNT_CV.ToString().ToUpper().Contains(searchValue))
                                       || (m.CONE_LEN != null && m.CONE_LEN.ToString().ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> DeleteFQaYarnTestInformationCotton(string testId)
        {
            var redirectToActionResult = RedirectToAction(nameof(CreateFQaYarnTestInformationCotton), $"FQaYarnTestInformationCotton");
            var testInfo = await _fQaYarnTestInformationCotton.FindByIdAsync(int.Parse(_protector.Unprotect(testId)));

            if (testInfo != null)
            {
                if (await _fQaYarnTestInformationCotton.Delete(testInfo))
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
        public async Task<IActionResult> EditFQaYarnTestInformationCotton(string testId)
        {
            var redirectToActionResult = RedirectToAction(nameof(CreateFQaYarnTestInformationCotton), $"FQaYarnTestInformationCotton");
            try
            {
                var fQaYarnTestInformationCotton = await _fQaYarnTestInformationCotton.FindByIdAsync(int.Parse(_protector.Unprotect(testId)));

                if (fQaYarnTestInformationCotton != null)
                {
                    var fQaYarnTestInformationCottonViewModel = await _fQaYarnTestInformationCotton.GetInitObjByAsync(new FQaYarnTestInformationCottonViewModel());
                    fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton = fQaYarnTestInformationCotton;

                    fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.EncryptedId = _protector.Protect(fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.TESTID.ToString());
                    return View(fQaYarnTestInformationCottonViewModel);
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
        public async Task<IActionResult> EditFQaYarnTestInformationCotton(FQaYarnTestInformationCottonViewModel fQaYarnTestInformationCottonViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(CreateFQaYarnTestInformationCotton), $"FQaYarnTestInformationCotton");
                fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.TESTID = int.Parse(_protector.Unprotect(fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.EncryptedId));
                var fQaYarnTestInformationCotton = await _fQaYarnTestInformationCotton.FindByIdAsync(fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.TESTID);
                if (fQaYarnTestInformationCotton != null)
                {
                    fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.UPDATED_AT = DateTime.Now;
                    fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.CREATED_AT = fQaYarnTestInformationCotton.CREATED_AT;
                    fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton.CREATED_BY = fQaYarnTestInformationCotton.CREATED_BY;

                    if (await _fQaYarnTestInformationCotton.Update(fQaYarnTestInformationCottonViewModel.FQaYarnTestInformationCotton))
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
            return View(await _fQaYarnTestInformationCotton.GetInitObjByAsync(new FQaYarnTestInformationCottonViewModel()));
        }
    }
}