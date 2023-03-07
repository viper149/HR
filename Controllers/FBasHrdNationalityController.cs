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
    [Route("Nationality")]
    public class FBasHrdNationalityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_BAS_HRD_NATIONALITY _fBasHrdNationality;
        private readonly IDataProtector _protector;
        private const string Title = "Nationality Information";

        public FBasHrdNationalityController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_NATIONALITY fBasHrdNationality)
        {
            _userManager = userManager;
            _fBasHrdNationality= fBasHrdNationality;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
        
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdNationality()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdNationality()
        {
            ViewData["Title"] = Title;
            return View(await _fBasHrdNationality.GetInitObjByAsync(new FBasHrdNationalityViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdNationality(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdNationalityViewModel.FBasHrdNationality.CREATED_AT = fBasHrdNationalityViewModel.FBasHrdNationality.UPDATED_AT = DateTime.Now;
                fBasHrdNationalityViewModel.FBasHrdNationality.CREATED_BY = fBasHrdNationalityViewModel.FBasHrdNationality.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdNationality.InsertByAsync(fBasHrdNationalityViewModel.FBasHrdNationality))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdNationality));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdNationalityViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdNationalityViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdNationality(string id)
        {

            try
            {
                var fBasHrdNationalityViewModel = await _fBasHrdNationality.GetInitObjByAsync(new FBasHrdNationalityViewModel());
                fBasHrdNationalityViewModel.FBasHrdNationality = await _fBasHrdNationality.FindByIdAsync(int.Parse(_protector.Unprotect(id)));
                fBasHrdNationalityViewModel.FBasHrdNationality.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdNationalityViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdNationality(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdNationality = await _fBasHrdNationality.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdNationalityViewModel.FBasHrdNationality.EncryptedId)));
                if (fBasHrdNationality != null)
                {
                    fBasHrdNationalityViewModel.FBasHrdNationality.NATIONID = fBasHrdNationality.NATIONID;
                    fBasHrdNationalityViewModel.FBasHrdNationality.NATION_DESC = fBasHrdNationality.NATION_DESC;
                    fBasHrdNationalityViewModel.FBasHrdNationality.NATION_DESC_BN = fBasHrdNationality.NATION_DESC_BN;
                    fBasHrdNationalityViewModel.FBasHrdNationality.COUNTRY_NAME = fBasHrdNationality.COUNTRY_NAME;
                    fBasHrdNationalityViewModel.FBasHrdNationality.COUNTRY_NAME_BN = fBasHrdNationality.COUNTRY_NAME_BN;
                    fBasHrdNationalityViewModel.FBasHrdNationality.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdNationalityViewModel.FBasHrdNationality.UPDATED_AT = DateTime.Now;
                    fBasHrdNationalityViewModel.FBasHrdNationality.CREATED_AT = fBasHrdNationality.CREATED_AT;
                    fBasHrdNationalityViewModel.FBasHrdNationality.CREATED_BY = fBasHrdNationality.CREATED_BY;

                    if (await _fBasHrdNationality.Update(fBasHrdNationalityViewModel.FBasHrdNationality))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdNationality));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdNationality));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdNationality));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdNationalityViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdNationality(string id)
        {
            var fBasHrdNationality = await _fBasHrdNationality.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdNationality != null)
            {
                if (await _fBasHrdNationality.Delete(fBasHrdNationality))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdNationality));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdNationality));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdNationality));
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
                var data = await _fBasHrdNationality.GetAllFBasHrdNationalityAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.NATION_DESC != null && m.NATION_DESC.ToUpper().Contains(searchValue))
                                           || (m.NATION_DESC_BN != null && m.NATION_DESC_BN.ToUpper().Contains(searchValue))
                                           || (m.COUNTRY_NAME != null && m.COUNTRY_NAME.ToUpper().Contains(searchValue))
                                           || (m.COUNTRY_NAME_BN != null && m.COUNTRY_NAME_BN.ToUpper().Contains(searchValue))
                                           || (m.SHORT_NAME != null && m.SHORT_NAME.ToUpper().Contains(searchValue))
                                           || (m.CUR != null && m.CUR.NAME.ToUpper().Contains(searchValue))
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
        [Route("NationalityAlreadyInUse")]
        public async Task<IActionResult> IsNationalityInUse(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            var nationality = fBasHrdNationalityViewModel.FBasHrdNationality.NATION_DESC;
            return await _fBasHrdNationality.FindByNationalityAsync(nationality) ? Json(true) : Json($"Nationality {nationality} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CountryAlreadyInUse")]
        public async Task<IActionResult> IsCountryInUse(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            var country = fBasHrdNationalityViewModel.FBasHrdNationality.COUNTRY_NAME;
            return await _fBasHrdNationality.FindByCountryAsync(country) ? Json(true) : Json($"Country {country} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("NationalityAlreadyInUseBn")]
        public async Task<IActionResult> IsNationalityBnInUse(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            var nationality = fBasHrdNationalityViewModel.FBasHrdNationality.NATION_DESC_BN;
            return await _fBasHrdNationality.FindByNationalityAsync(nationality, true) ? Json(true) : Json($"ইতিমধ্যে {nationality} নামের জাতীয়তা আছে");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CountryAlreadyInUseBn")]
        public async Task<IActionResult> IsCountryBnInUse(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            var country = fBasHrdNationalityViewModel.FBasHrdNationality.COUNTRY_NAME_BN;
            return await _fBasHrdNationality.FindByCountryAsync(country, true) ? Json(true) : Json($"ইতিমধ্যে {country} নামের দেশ আছে");
        }
    }
}