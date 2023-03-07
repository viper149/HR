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
    [Route("Section")]
    public class FBasHrdSectionController : Controller
    {
        private readonly IF_BAS_HRD_SECTION _fBasHrdSection;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private const string Title = "Section Information";

        public FBasHrdSectionController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_SECTION fBasHrdSection)
        {
            _fBasHrdSection = fBasHrdSection;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdSection()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdSection()
        {
            ViewData["Title"] = Title;
            return View(new FBasHrdSectionViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdSection(FBasHrdSectionViewModel fBasHrdSectionViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdSectionViewModel.FBasHrdSection.CREATED_AT = fBasHrdSectionViewModel.FBasHrdSection.UPDATED_AT = DateTime.Now;
                fBasHrdSectionViewModel.FBasHrdSection.CREATED_BY = fBasHrdSectionViewModel.FBasHrdSection.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdSection.InsertByAsync(fBasHrdSectionViewModel.FBasHrdSection))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdSection));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(fBasHrdSectionViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(fBasHrdSectionViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdSection(string id)
        {

            try
            {
                var fBasHrdSectionViewModel = new FBasHrdSectionViewModel
                {
                    FBasHrdSection = await _fBasHrdSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdSectionViewModel.FBasHrdSection.EncryptedId = id;
                ViewData["Title"] = Title;
                return View(fBasHrdSectionViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdSection(FBasHrdSectionViewModel fBasHrdSectionViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdSection = await _fBasHrdSection.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdSectionViewModel.FBasHrdSection.EncryptedId)));
                if (fBasHrdSection != null)
                {
                    fBasHrdSectionViewModel.FBasHrdSection.SECID = fBasHrdSection.SECID;
                    fBasHrdSectionViewModel.FBasHrdSection.SEC_NAME = fBasHrdSection.SEC_NAME;
                    fBasHrdSectionViewModel.FBasHrdSection.SEC_NAME_BN = fBasHrdSection.SEC_NAME_BN;
                    fBasHrdSectionViewModel.FBasHrdSection.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fBasHrdSectionViewModel.FBasHrdSection.UPDATED_AT = DateTime.Now;
                    fBasHrdSectionViewModel.FBasHrdSection.CREATED_AT = fBasHrdSection.CREATED_AT;
                    fBasHrdSectionViewModel.FBasHrdSection.CREATED_BY = fBasHrdSection.CREATED_BY;

                    if (await _fBasHrdSection.Update(fBasHrdSectionViewModel.FBasHrdSection))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasHrdSection));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFBasHrdSection));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdSection));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fBasHrdSectionViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdSection(string id)
        {
            var fBasHrdSection = await _fBasHrdSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdSection != null)
            {
                if (await _fBasHrdSection.Delete(fBasHrdSection))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFBasHrdSection));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFBasHrdSection));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFBasHrdSection));
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
                var data = await _fBasHrdSection.GetAllFBasHrdSectionAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.SEC_NAME != null && m.SEC_NAME.ToUpper().Contains(searchValue))
                                           || (m.SEC_NAME_BN != null && m.SEC_NAME_BN.ToUpper().Contains(searchValue))
                                           || (m.SHORT_NAME != null && m.SHORT_NAME.ToUpper().Contains(searchValue))
                                           || (m.SHORT_NAME_BN != null && m.SHORT_NAME_BN.ToUpper().Contains(searchValue))
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
        public async Task<IActionResult> IsSubSecNameInUse(FBasHrdSectionViewModel fBasHrdSectionViewModel)
        {
            var deptName = fBasHrdSectionViewModel.FBasHrdSection.SEC_NAME;
            return await _fBasHrdSection.FindBySecNameAsync(deptName) ? Json(true) : Json($"Section {deptName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUseBn")]
        public async Task<IActionResult> IsSubSecNameBnInUse(FBasHrdSectionViewModel fBasHrdSectionViewModel)
        {
            var deptName = fBasHrdSectionViewModel.FBasHrdSection.SEC_NAME_BN;
            return await _fBasHrdSection.FindBySecNameAsync(deptName, true) ? Json(true) : Json($"ইতিমধ্যে {deptName} নামের সেকশন আছে");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetSections")]
        public async Task<IActionResult> GetAllSections (int thanaId)
        {
            try
            {
                return Ok(await _fBasHrdSection.GetAllSectionsAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
