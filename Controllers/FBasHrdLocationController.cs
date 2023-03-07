using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.HR;
using DenimERP.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Route("Location")]
    public class FBasHrdLocationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_BAS_HRD_LOCATION _fBasHrdLocation;
        private readonly IDataProtector _protector;
        private const string Title = "Location Information";

        public FBasHrdLocationController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_LOCATION fBasHrdLocation)
        {
            _userManager = userManager;
            _fBasHrdLocation = fBasHrdLocation;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdLocation()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdLocation()
        {
            ViewData["Title"] = Title;
            return View(new FBasHrdLocationViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdLocation(FBasHrdLocationViewModel fBasHrdLocationViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdLocationViewModel.FBasHrdLocation.CREATED_AT = fBasHrdLocationViewModel.FBasHrdLocation.UPDATED_AT = DateTime.Now;
                fBasHrdLocationViewModel.FBasHrdLocation.CREATED_BY = fBasHrdLocationViewModel.FBasHrdLocation.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdLocation.InsertByAsync(fBasHrdLocationViewModel.FBasHrdLocation))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdLocation));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdLocationViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdLocationViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdLocation(string id)
        {

            try
            {
                var fBasHrdLocationViewModel = new FBasHrdLocationViewModel
                {
                    FBasHrdLocation = await _fBasHrdLocation.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdLocationViewModel.FBasHrdLocation.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdLocationViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdLocation(FBasHrdLocationViewModel fBasHrdLocationViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdLocation = await _fBasHrdLocation.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdLocationViewModel.FBasHrdLocation.EncryptedId)));
                if (fBasHrdLocation != null)
                {
                    fBasHrdLocationViewModel.FBasHrdLocation.LOCID = fBasHrdLocation.LOCID;
                    fBasHrdLocationViewModel.FBasHrdLocation.LOC_NAME = fBasHrdLocation.LOC_NAME;
                    fBasHrdLocationViewModel.FBasHrdLocation.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdLocationViewModel.FBasHrdLocation.UPDATED_AT = DateTime.Now;
                    fBasHrdLocationViewModel.FBasHrdLocation.CREATED_AT = fBasHrdLocation.CREATED_AT;
                    fBasHrdLocationViewModel.FBasHrdLocation.CREATED_BY = fBasHrdLocation.CREATED_BY;

                    if (await _fBasHrdLocation.Update(fBasHrdLocationViewModel.FBasHrdLocation))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdLocation));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdLocation));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdLocation));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdLocationViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdLocation(string id)
        {
            var fBasHrdLocation = await _fBasHrdLocation.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdLocation != null)
            {
                if (await _fBasHrdLocation.Delete(fBasHrdLocation))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdLocation));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdLocation));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdLocation));
        }

        [HttpPost]
        [Route("GetData")]
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
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var data = await _fBasHrdLocation.GetAllFBasHrdLocationAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.LOC_NAME != null && m.LOC_NAME.ToUpper().Contains(searchValue))
                                           || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();
                return Json(new
                {
                    draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal,
                    data = finalData
                });
            }
            catch (Exception e)
            {
                return Json(BadRequest(e));
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUse")]
        public async Task<IActionResult> IsLocNameInUse(FBasHrdLocationViewModel fBasHrdLocationViewModel)
        {
            var locName = fBasHrdLocationViewModel.FBasHrdLocation.LOC_NAME;
            return await _fBasHrdLocation.FindByLocNameAsync(locName) ? Json(true) : Json($"Location {locName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                return Ok(await _fBasHrdLocation.GetAllLocationsAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
