using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize(Policy = "FSample")]
    public class FSampleLocationController : Controller
    {
        private readonly IF_SAMPLE_LOCATION _fSampleLocation;
        private readonly IDataProtector _protector;

        public FSampleLocationController(IF_SAMPLE_LOCATION fSampleLocation,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _fSampleLocation = fSampleLocation;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locId"> Belongs to LOCID. Primary key. Must not to be null. <see cref="F_SAMPLE_LOCATION"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailsFSampleLocation(string locId)
        {
            try
            {
                var fSampleLocation = await _fSampleLocation.FindByIdAsync(int.Parse(_protector.Unprotect(locId)));
                fSampleLocation.EncryptedId = _protector.Protect(fSampleLocation.LOCID.ToString());
                return View(fSampleLocation);
            }
            catch (Exception)
            {
                return View($"Error");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locId"> Belongs to LOCID. Primary key. Must not to be null. <see cref="F_SAMPLE_LOCATION"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteFSampleLocation(string locId)
        {
            try
            {
                var fSampleLocation = await _fSampleLocation.FindByIdAsync(int.Parse(_protector.Unprotect(locId)));
                if (fSampleLocation != null)
                {
                    await _fSampleLocation.Delete(fSampleLocation);
                    TempData["message"] = "Successfully Deleted Factory Sample Location.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Failed to Delete Factory Sample Location.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetFSampleLocation", $"FSampleLocation");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Factory Sample Location.";
                TempData["type"] = "error";
                return RedirectToAction("GetFSampleLocation", $"FSampleLocation");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fSampleLocation"><see cref="F_SAMPLE_LOCATION"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditFSampleLocation(F_SAMPLE_LOCATION fSampleLocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _fSampleLocation.Update(fSampleLocation);
                    TempData["message"] = "Successfully Updated Factory Sample Location.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetFSampleLocation", $"FSampleLocation");
                }

                TempData["message"] = "Failed To Update Factory Sample Location.";
                TempData["type"] = "error";
                return View(fSampleLocation);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"> Belongs to LOCID. Primary key. Must not to be null. <see cref="F_SAMPLE_LOCATION"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditFSampleLocation(string locationId)
        {
            try
            {
                var fSampleLocation = await _fSampleLocation.FindByIdAsync(int.Parse(_protector.Unprotect(locationId)));
                fSampleLocation.EncryptedId = _protector.Protect(fSampleLocation.LOCID.ToString());
                return View(fSampleLocation);
            }
            catch (Exception)
            {
                return View($"Error");
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _fSampleLocation.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        public IActionResult GetFSampleLocation()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fSampleLocation"><see cref="F_SAMPLE_LOCATION"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateFSampleLocation(F_SAMPLE_LOCATION fSampleLocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _fSampleLocation.InsertByAsync(fSampleLocation);
                    TempData["message"] = "Successfully Created Factory Sample Location.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetFSampleLocation", $"FSampleLocation");
                }
                else
                {
                    TempData["message"] = "Invalid Form Data.";
                    TempData["type"] = "error";
                    return View($"Error");
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult CreateFSampleLocation()
        {
            return View();
        }
    }
}