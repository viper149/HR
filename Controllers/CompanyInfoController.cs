using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInterfaces.CompanyInfo;
using HRMS.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("Company")]
    public class CompanyInfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICOMPANY_INFO _companyInfo;
        private const string Title = "Company Information";

        public CompanyInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            ICOMPANY_INFO companyInfo)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _companyInfo = companyInfo;
        }
        
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetCompanyInfo()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateCompanyInfo()
        {
            ViewData["Title"] = Title;
            return View(new CompanyInfoViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCompanyInfo(CompanyInfoViewModel companyInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                var memoryStream = new MemoryStream();
                if (companyInfoViewModel.Logo == null)
                {
                    companyInfoViewModel.CompanyInfo.LOGO = null;
                }
                else
                {
                    await companyInfoViewModel.Logo.CopyToAsync(memoryStream);
                    companyInfoViewModel.CompanyInfo.LOGO = memoryStream.ToArray();
                }

                companyInfoViewModel.CompanyInfo.CREATED_AT =
                    companyInfoViewModel.CompanyInfo.UPDATED_AT = DateTime.Now;
                companyInfoViewModel.CompanyInfo.CREATED_BY = companyInfoViewModel.CompanyInfo.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _companyInfo.InsertByAsync(companyInfoViewModel.CompanyInfo))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetCompanyInfo));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(companyInfoViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(companyInfoViewModel);
        }
        
        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditCompanyInfo(string id)
        {
            var companyInfoViewModel = new CompanyInfoViewModel
            {
                CompanyInfo = await _companyInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
            };

            companyInfoViewModel.CompanyInfo.EncryptedId = id;
            ViewData["Title"] = Title;
            return View(companyInfoViewModel);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditCompanyInfo(CompanyInfoViewModel companyInfoViewModel, IFormFile LOGO)
        {
            if (ModelState.IsValid)
            {
                var companyInfo = await _companyInfo.FindByIdAsync(int.Parse(_protector.Unprotect(companyInfoViewModel.CompanyInfo.EncryptedId)));

                var memoryStream = new MemoryStream();
                if (companyInfoViewModel.Logo == null)
                {
                    companyInfoViewModel.CompanyInfo.LOGO = companyInfo.LOGO;
                }
                else
                {
                    await companyInfoViewModel.Logo.CopyToAsync(memoryStream);
                    companyInfoViewModel.CompanyInfo.LOGO = memoryStream.ToArray();
                }

                if (companyInfo != null)
                {
                    companyInfoViewModel.CompanyInfo.ID = companyInfo.ID;
                    companyInfoViewModel.CompanyInfo.COMPANY_NAME = companyInfo.COMPANY_NAME;
                    companyInfoViewModel.CompanyInfo.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    companyInfoViewModel.CompanyInfo.UPDATED_AT = DateTime.Now;
                    companyInfoViewModel.CompanyInfo.CREATED_AT = companyInfo.CREATED_AT;
                    companyInfoViewModel.CompanyInfo.CREATED_BY = companyInfo.CREATED_BY;

                    if (await _companyInfo.Update(companyInfoViewModel.CompanyInfo))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetCompanyInfo));
                    }

                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetCompanyInfo));
                }

                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetCompanyInfo));
            }

            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(companyInfoViewModel);
        }

        [HttpGet]
        [Route("Details/{id?}")]
        public async Task<IActionResult> DetailsCompanyInfo(string id)
        {
            var companyInfo = await _companyInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));
            
            companyInfo.EncryptedId = id;
            companyInfo.LogoBase64 = "data:image/" +
                                     Common.Common.GetFileExtension(Convert.ToBase64String(companyInfo.LOGO)) +
                                     ";base64," + Convert.ToBase64String(companyInfo.LOGO);
            ViewData["Title"] = Title;
            return View(companyInfo);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteCompanyInfo(string id)
        {
            var companyInfo = await _companyInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (companyInfo != null)
            {
                if (await _companyInfo.Delete(companyInfo))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetCompanyInfo));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetCompanyInfo));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetCompanyInfo));
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
                var sortColumn = Request
                    .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()
                    ?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var data = await _companyInfo.GetAllCompanyInfoAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc"
                        ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList()
                        : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.COMPANY_NAME != null && m.COMPANY_NAME.ToUpper().Contains(searchValue))
                                           //|| (m.TAGLINE != null && m.TAGLINE.ToUpper().Contains(searchValue))
                                           //|| (m.HEADOFFICE_ADDRESS != null && m.HEADOFFICE_ADDRESS.ToUpper().Contains(searchValue))
                                           //|| (m.FACTORY_ADDRESS != null && m.FACTORY_ADDRESS.ToUpper().Contains(searchValue))
                                           //|| (m.WEB_ADDRESS != null && m.WEB_ADDRESS.ToUpper().Contains(searchValue))
                                           || (m.EMAIL != null && m.EMAIL.ToUpper().Contains(searchValue))
                                           || (m.PHONE1 != null && m.PHONE1.ToUpper().Contains(searchValue))
                                           //|| (m.PHONE2 != null && m.PHONE2.ToUpper().Contains(searchValue))
                                           //|| (m.PHONE3 != null && m.PHONE3.ToUpper().Contains(searchValue))
                                           //|| (m.BIN_NO != null && m.BIN_NO.ToUpper().Contains(searchValue))
                                           //|| (m.ETIN_NO != null && m.ETIN_NO.ToUpper().Contains(searchValue))
                                           //|| (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)))
                        .ToList();
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
        public async Task<IActionResult> IsEmpTypeInUse(CompanyInfoViewModel companyInfoViewModel)
        {
            var companyName = companyInfoViewModel.CompanyInfo.COMPANY_NAME;
            return await _companyInfo.FindByCompanyName(companyName) ? Json(true) : Json($"Type {companyName} already exists");
        }
    }
}
