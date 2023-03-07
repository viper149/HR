using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd.Grey;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndFabTestGreyController : Controller
    {
        private readonly IRND_FABTEST_GREY _rndFabtestGrey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Fabric Test Information (Grey).";

        public RndFabTestGreyController(IRND_FABTEST_GREY rndFabtestGrey,

            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _rndFabtestGrey = rndFabtestGrey;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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
            var data = (List<RND_FABTEST_GREY>)await _rndFabtestGrey.GetForDataTableByAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.LTGDATE != null && m.LTGDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.LAB_NO != null && m.LAB_NO.ToUpper().Contains(searchValue))
                                       || (m.PROG != null && m.PROG.PROG_NO.ToUpper().Contains(searchValue))
                                       || (m.OPTION1 != null && m.OPTION1.ToUpper().Contains(searchValue))
                                       || (m.EMP_WASHEDBY.FIRST_NAME != null && m.EMP_WASHEDBY.FIRST_NAME.ToUpper().Contains(searchValue))
                                       || (m.EMP_UNWASHEDBY.FIRST_NAME != null && m.EMP_UNWASHEDBY.FIRST_NAME.ToUpper().Contains(searchValue))
                                       || (m.PROGN != null && m.PROGN.PROG_ != null && m.PROGN.PROG_.PROG_NO.ToUpper().Contains(searchValue))
                                       || (m.PROGN != null && m.PROGN.OPT1.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
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
        public IActionResult GetRndFabTestGrey()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadDetails(int sdId)
        {
            var result = await _rndFabtestGrey.FindBySdIdAsync(sdId);
            Response.Headers["DEV_NO"] = result.RndSampleInfoWeaving.DEV_NO;
            Response.Headers["GREPI"] = result.RndSampleInfoWeaving.GR_EPI.ToString();
            Response.Headers["GRPPI"] = result.RndSampleInfoWeaving.GR_PPI.ToString();
            return PartialView($"LoadDetailsTable", result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRndFabTestGrey()
        {
            return View(await _rndFabtestGrey.GetInitObjects(new CreateRndFabTestGreyViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndFabTestGrey(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel)
        {
            if (!ModelState.IsValid) return View(await _rndFabtestGrey.GetInitObjects(createRndFabTestGreyViewModel));

            var user = _userManager.GetUserAsync(User);
            createRndFabTestGreyViewModel.RndFabtestGrey.CREATED_BY = createRndFabTestGreyViewModel.RndFabtestGrey.UPDATED_BY = user.Result.Id;
            createRndFabTestGreyViewModel.RndFabtestGrey.CREATED_AT = createRndFabTestGreyViewModel.RndFabtestGrey.UPDATED_AT = DateTime.Now;
            createRndFabTestGreyViewModel.RndFabtestGrey.PROGID = null;
            if (await _rndFabtestGrey.InsertByAsync(createRndFabTestGreyViewModel.RndFabtestGrey))
            {
                TempData["message"] = "Successfully Added RnD Fabric Test Sample.";
                TempData["type"] = "success";
                return RedirectToAction("GetRndFabTestGrey", $"RndFabTestGrey");
            }
            TempData["message"] = "Failed to Add RnD Fabric Test Sample.";
            TempData["type"] = "success";
            return RedirectToAction("GetRndFabTestGrey", $"RndFabTestGrey");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsRndFabTestGrey(string ltgId)
        {
            return View(await _rndFabtestGrey.FindByLtgIdAsync(int.Parse(_protector.Unprotect(ltgId))));
        }

        [HttpGet]
        public async Task<IActionResult> EditRndFabTestGrey(string ltgId)
        {
            try
            {
                var createRndFabTestGreyViewModel = await _rndFabtestGrey.GetRndFabTestGreyWithDetailsByAsnc(
                    int.Parse(_protector.Unprotect(ltgId)));
                createRndFabTestGreyViewModel = await _rndFabtestGrey.GetInitObjects(createRndFabTestGreyViewModel);

                return View(await _rndFabtestGrey.GetInitObjectsForEditView(createRndFabTestGreyViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRndFabTestGrey(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var redirectToActionResult = RedirectToAction(nameof(GetRndFabTestGrey), $"RndFabTestGrey");
                    createRndFabTestGreyViewModel.RndFabtestGrey.LTGID = int.Parse(_protector.Unprotect(createRndFabTestGreyViewModel.RndFabtestGrey.EncryptedId));

                    var rndFabtestGrey = await _rndFabtestGrey.FindByIdAsync(createRndFabTestGreyViewModel.RndFabtestGrey.LTGID);

                    if (rndFabtestGrey != null)
                    {
                        createRndFabTestGreyViewModel.RndFabtestGrey.UPDATED_AT = DateTime.Now;
                        createRndFabTestGreyViewModel.RndFabtestGrey.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                        createRndFabTestGreyViewModel.RndFabtestGrey.CREATED_AT = rndFabtestGrey.CREATED_AT;
                        createRndFabTestGreyViewModel.RndFabtestGrey.CREATED_BY = rndFabtestGrey.CREATED_BY;


                        if (await _rndFabtestGrey.Update(createRndFabTestGreyViewModel.RndFabtestGrey))
                        {
                            TempData["message"] = $"Successfully Updated {title}";
                            TempData["type"] = "success";
                            return redirectToActionResult;
                        }

                        TempData["message"] = $"Failed to Update {title}";
                        TempData["type"] = "error";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"{title} Not Found";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }

                TempData["message"] = "Invalid Operation. Try Again.";
                TempData["type"] = "error";
                return View(await _rndFabtestGrey.GetInitObjectsForEditView(createRndFabTestGreyViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRndFabTestGrey(string ltgId)
        {
            try
            {
                var rndFabtestGrey = await _rndFabtestGrey.FindByIdAsync(int.Parse(_protector.Unprotect(ltgId)));

                if (rndFabtestGrey != null)
                {
                    if (await _rndFabtestGrey.Delete(rndFabtestGrey))
                    {
                        TempData["message"] = "Successfully Deleted RnD Fabric Test Grey Information.";
                        TempData["type"] = "success";
                    }
                    else
                    {
                        TempData["message"] = "Failed To Delete RnD Fabric Test Grey Information.";
                        TempData["type"] = "error";
                    }
                }

                return RedirectToAction("GetRndFabTestGrey", $"RndFabTestGrey");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsLabNoInUse(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel)
        {
            var labNo = createRndFabTestGreyViewModel.RndFabtestGrey.LAB_NO;
            return await _rndFabtestGrey.FindByLabNo(labNo) ? Json(true) : Json($"Lab No {labNo} is already in use");
        }
    }
}