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
    public class RndFabTestSampleController : Controller
    {
        private readonly IRND_FABTEST_SAMPLE _rndFabtestSample;
        private readonly IRND_SAMPLEINFO_FINISHING _rndSampleinfoFinishing;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "RnD Fabric Test Sample.";

        public RndFabTestSampleController(IRND_FABTEST_SAMPLE rndFabtestSample,
            IRND_SAMPLEINFO_FINISHING rndSampleinfoFinishing,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _rndFabtestSample = rndFabtestSample;
            _rndSampleinfoFinishing = rndSampleinfoFinishing;
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
            var data = (List<RND_FABTEST_SAMPLE>)await _rndFabtestSample.GetAllRndFabTestSampleAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.LTSDATE != null && m.LTSDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.LABNO != null && m.LABNO.ToUpper().Contains(searchValue))
                                       || (m.PROGNONavigation.PROG_.PROG_NO != null && m.PROGNONavigation.PROG_.PROG_NO.ToUpper().Contains(searchValue))
                                       || (m.SFIN.STYLE_NAME != null && m.SFIN.STYLE_NAME.ToUpper().Contains(searchValue))
                                       || (m.WASHEDBYNavigation.EMPNO != null && m.WASHEDBYNavigation.EMPNO.ToUpper().Contains(searchValue))
                                       || (m.UNWASHEDBYNavigation.EMPNO != null && m.UNWASHEDBYNavigation.EMPNO.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> CreateRndFabTestSample()
        {
            try
            {
                return View(await _rndFabtestSample.GetInitObjects(new RndFabTestSampleViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndFabTestSample(RndFabTestSampleViewModel rndFabTestSampleViewModel)
        {
            if (ModelState.IsValid)
            {
                rndFabTestSampleViewModel.RndFabtestSample.CREATED_AT = rndFabTestSampleViewModel.RndFabtestSample.UPDATED_AT = DateTime.Now;
                rndFabTestSampleViewModel.RndFabtestSample.CREATED_BY = rndFabTestSampleViewModel.RndFabtestSample.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _rndFabtestSample.InsertByAsync(rndFabTestSampleViewModel.RndFabtestSample))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetRndFabTestSample), "RndFabTestSample");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(rndFabTestSampleViewModel);
                }
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(await _rndFabtestSample.GetInitObjects(new RndFabTestSampleViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> EditRndFabTestSample(string ltsId)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetRndFabTestSample), $"RndFabTestSample");
            var rndFabtestSample = await _rndFabtestSample.FindByIdAsync(int.Parse(_protector.Unprotect(ltsId)));

            if (rndFabtestSample != null)
            {
                var rndFabTestSampleViewModel = await _rndFabtestSample.GetInitObjects(new RndFabTestSampleViewModel());
                rndFabTestSampleViewModel.RndFabtestSample = rndFabtestSample;

                rndFabTestSampleViewModel.RndFabtestSample.EncryptedId = _protector.Protect(rndFabTestSampleViewModel.RndFabtestSample.LTSID.ToString());
                return View(rndFabTestSampleViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return redirectToActionResult;
        }

        [HttpPost]
        public async Task<IActionResult> EditRndFabTestSample(RndFabTestSampleViewModel rndFabTestSampleViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetRndFabTestSample), $"RndFabTestSample");
                rndFabTestSampleViewModel.RndFabtestSample.LTSID = int.Parse(_protector.Unprotect(rndFabTestSampleViewModel.RndFabtestSample.EncryptedId));
                var rndFabtestBulk = await _rndFabtestSample.FindByIdAsync(rndFabTestSampleViewModel.RndFabtestSample.LTSID);

                if (rndFabtestBulk != null)
                {
                    rndFabTestSampleViewModel.RndFabtestSample.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    rndFabTestSampleViewModel.RndFabtestSample.UPDATED_AT = DateTime.Now;
                    rndFabTestSampleViewModel.RndFabtestSample.CREATED_AT = rndFabtestBulk.CREATED_AT;
                    rndFabTestSampleViewModel.RndFabtestSample.CREATED_BY = rndFabtestBulk.CREATED_BY;

                    if (await _rndFabtestSample.Update(rndFabTestSampleViewModel.RndFabtestSample))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(await _rndFabtestSample.GetInitObjects(new RndFabTestSampleViewModel()));
        }

        //[HttpGet]
        //public async Task<IActionResult> DeleteRndFabTestSample(string ltsId)
        //{
        //    try
        //    {
        //        var rndFabtestSample = await _rndFabtestSample.FindByIdAsync(int.Parse(_protector.Unprotect(ltsId)));

        //        if (rndFabtestSample != null)
        //        {
        //            if (await _rndFabtestSample.Delete(rndFabtestSample))
        //            {
        //                TempData["message"] = "Successfully Deleted RnD Fabric Test Sample.";
        //                TempData["type"] = "success";
        //            }
        //            else
        //            {
        //                TempData["message"] = "Failed To Delete RnD Fabric Test Sample.";
        //                TempData["type"] = "error";
        //            }
        //        }
        //        else
        //        {
        //            TempData["message"] = "RnD Fabric Test Sample Not Found";
        //            TempData["type"] = "error";
        //        }

        //        return RedirectToAction(nameof(GetRndFabTestSample), $"RndFabTestSample");
        //    }
        //    catch (Exception)
        //    {
        //        return View($"Error");
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> DetailsRndFabTestSample(string ltsId)
        //{
        //    try
        //    {
        //        return View(await _rndFabtestSample.FindObjectsByLtsIdIncludeAllAsync(int.Parse(_protector.Unprotect(ltsId))));
        //    }
        //    catch (Exception)
        //    {
        //        return View($"Error");
        //    }
        //}

        [HttpGet]
        public IActionResult GetRndFabTestSample()
        {
            return View();
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
        public async Task<IActionResult> IsLabNoInUse(RndFabTestSampleViewModel createRndFabTestSampleViewModel)
        {
            var labNo = createRndFabTestSampleViewModel.RndFabtestSample.LABNO;
            return await _rndFabtestSample.FindByLabNo(labNo) ? Json(true) : Json($"Lab No {labNo} is already in use");
        }
    }
}