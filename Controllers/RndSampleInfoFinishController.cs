using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd.Finish;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndSampleInfoFinishController : Controller
    {
        private readonly IRND_SAMPLEINFO_FINISHING _rndSampleinfoFinishing;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public RndSampleInfoFinishController(IRND_SAMPLEINFO_FINISHING rndSampleinfoFinishing,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _rndSampleinfoFinishing = rndSampleinfoFinishing;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRndSampleInfoFinish(string fnId)
        {
            try
            {
                var rndSampleinfoFinishing = await _rndSampleinfoFinishing.FindByIdAsync(int.Parse(_protector.Unprotect(fnId)));

                if (rndSampleinfoFinishing != null)
                {
                    if (await _rndSampleinfoFinishing.Delete(rndSampleinfoFinishing))
                    {
                        TempData["message"] = "Successfully Deleted RnD Sample Information Finish.";
                        TempData["type"] = "success";
                    }
                    else
                    {
                        TempData["message"] = "Failed To Delete RnD Sample Information Finish.";
                        TempData["type"] = "error";
                    }
                }

                return RedirectToAction("GetRndSampleInfoFinish", $"RndSampleInfoFinish");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsRndSampleInfoFinish(string fnId)
        {
            try
            {
                return View(await _rndSampleinfoFinishing.FindByLtgIdAsync(int.Parse(_protector.Unprotect(fnId))));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRndSampleInfoFinish(CreateRndSampleInfoFinishViewModel createRndSampleInfoFinishViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var rndSampleinfoFinishing = await _rndSampleinfoFinishing.FindByIdAsync(int.Parse(_protector.Unprotect(createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.EncryptedId)));

                    createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.CREATED_AT = rndSampleinfoFinishing.CREATED_AT;
                    createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.CREATED_BY = rndSampleinfoFinishing.CREATED_BY;
                    rndSampleinfoFinishing.UPDATED_AT = DateTime.Now;
                    rndSampleinfoFinishing.UPDATED_BY = user.Id;

                    if (await _rndSampleinfoFinishing.Update(createRndSampleInfoFinishViewModel.RndSampleinfoFinishing))
                    {
                        TempData["message"] = "Successfully Updated RnD Sample Finish Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRndSampleInfoFinish", $"RndSampleInfoFinish");
                    }

                    TempData["message"] = "Failed To Update RnD Sample Finish Information.";
                    TempData["type"] = "error";
                    return View(createRndSampleInfoFinishViewModel);
                }

                TempData["message"] = "Failed To Update RnD Sample Finish Information.";
                TempData["type"] = "error";
                return View(createRndSampleInfoFinishViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed To Update RnD Sample Finish Information .";
                TempData["type"] = "error";
                return View(createRndSampleInfoFinishViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRndSampleInfoFinish(string fnId)
        {
            try
            {
                var createRndSampleInfoFinishViewModel =
                    await _rndSampleinfoFinishing.FindByFnIdAsync(int.Parse(_protector.Unprotect(fnId)));
                createRndSampleInfoFinishViewModel = await _rndSampleinfoFinishing.GetInitObjects(createRndSampleInfoFinishViewModel);
                return View(createRndSampleInfoFinishViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPreviousData(int ltgId)
        {
            try
            {
                if (ltgId <= 0) throw new ArgumentOutOfRangeException(nameof(ltgId));
                var result = await _rndSampleinfoFinishing.GetPreviousData(ltgId);
                Response.Headers["StyleName"] = result?.RndFabtestGrey?.PROG?.RND_SAMPLE_INFO_WEAVING?.FirstOrDefault()?.FABCODE;
                return PartialView($"GetPreviousDataTable", result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndSampleInfoFinish(CreateRndSampleInfoFinishViewModel createRndSampleInfoFinishViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(await _rndSampleinfoFinishing.GetInitObjects(createRndSampleInfoFinishViewModel));

                var user = _userManager.GetUserAsync(User).Result;

                createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.CREATED_AT = DateTime.Now;
                createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.CREATED_BY = user.Id;
                createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.UPDATED_AT = DateTime.Now;
                createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.UPDATED_BY = user.Id;
                await _rndSampleinfoFinishing.InsertByAsync(createRndSampleInfoFinishViewModel.RndSampleinfoFinishing);
                return RedirectToAction("GetRndSampleInfoFinish", $"RndSampleInfoFinish");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateRndSampleInfoFinish()
        {
            try
            {
                return View(await _rndSampleinfoFinishing.GetInitObjects(new CreateRndSampleInfoFinishViewModel()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _rndSampleinfoFinishing.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                return Json(new
                {
                    draw = forDataTableByAsync.Draw,
                    recordsFiltered = forDataTableByAsync.RecordsTotal,
                    recordsTotal = forDataTableByAsync.RecordsTotal,
                    data = forDataTableByAsync.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetRndSampleInfoFinish()
        {
            return View();
        }
    }
}