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
    [Route("Sub-Section")]
    public class FBasHrdSubSectionController: Controller
    {
        private readonly IF_BAS_HRD_SUB_SECTION _fBasHrdSubSection;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private const string Title = "Sub-Section Information";

        public FBasHrdSubSectionController (IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_BAS_HRD_SUB_SECTION fBasHrdSubSection)
        {
            _fBasHrdSubSection=fBasHrdSubSection;
            _userManager=userManager;
            _protector=dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFBasHrdSubSection ()
        {
            ViewData["Title"]=Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateFBasHrdSubSection ()
        {
            ViewData["Title"]=Title;
            return View(new FBasHrdSubSectionViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFBasHrdSubSection (FBasHrdSubSectionViewModel fBasHrdSubSectionViewModel)
        {
            if (ModelState.IsValid)
            {
                fBasHrdSubSectionViewModel.FBasHrdSubSection.CREATED_AT=fBasHrdSubSectionViewModel.FBasHrdSubSection.UPDATED_AT=DateTime.Now;
                fBasHrdSubSectionViewModel.FBasHrdSubSection.CREATED_BY=fBasHrdSubSectionViewModel.FBasHrdSubSection.UPDATED_BY=
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _fBasHrdSubSection.InsertByAsync(fBasHrdSubSectionViewModel.FBasHrdSubSection))
                {
                    TempData["message"]=$"Successfully Added {Title}";
                    TempData["type"]="success";
                    return RedirectToAction(nameof(GetFBasHrdSubSection));
                }

                TempData["message"]=$"Failed to Add {Title}";
                TempData["type"]="error";
                return View(fBasHrdSubSectionViewModel);
            }

            TempData["message"]=$"Please Enter Valid {Title}";
            TempData["type"]="error";
            return View(fBasHrdSubSectionViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFBasHrdSubSection (string id)
        {

            try
            {
                var fBasHrdSubSectionViewModel = new FBasHrdSubSectionViewModel
                {
                    FBasHrdSubSection=await _fBasHrdSubSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
                };

                fBasHrdSubSectionViewModel.FBasHrdSubSection.EncryptedId=id;
                ViewData["Title"]=Title;
                return View(fBasHrdSubSectionViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditFBasHrdSubSection (FBasHrdSubSectionViewModel fBasHrdSubSectionViewModel)
        {
            if (ModelState.IsValid)
            {
                var fBasHrdSubSection = await _fBasHrdSubSection.FindByIdAsync(int.Parse(_protector.Unprotect(fBasHrdSubSectionViewModel.FBasHrdSubSection.EncryptedId)));
                if (fBasHrdSubSection!=null)
                {
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.SUBSECID=fBasHrdSubSection.SUBSECID;
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.SUBSEC_NAME=fBasHrdSubSection.SUBSEC_NAME;
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.SUBSEC_NAME_BN=fBasHrdSubSection.SUBSEC_NAME_BN;
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.UPDATED_BY=(await _userManager.GetUserAsync(User)).Id;
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.UPDATED_AT=DateTime.Now;
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.CREATED_AT=fBasHrdSubSection.CREATED_AT;
                    fBasHrdSubSectionViewModel.FBasHrdSubSection.CREATED_BY=fBasHrdSubSection.CREATED_BY;

                    if (await _fBasHrdSubSection.Update(fBasHrdSubSectionViewModel.FBasHrdSubSection))
                    {
                        TempData["message"]=$"Successfully Updated {Title}.";
                        TempData["type"]="success";
                        return RedirectToAction(nameof(GetFBasHrdSubSection));
                    }
                    TempData["message"]=$"Failed to Update {Title}.";
                    TempData["type"]="error";
                    return RedirectToAction(nameof(GetFBasHrdSubSection));
                }
                TempData["message"]=$"{Title} Not Found.";
                TempData["type"]="error";
                return RedirectToAction(nameof(GetFBasHrdSubSection));
            }
            TempData["message"]="Invalid Input. Please Try Again.";
            TempData["type"]="error";
            return View(fBasHrdSubSectionViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFBasHrdSubSection (string id)
        {
            var fBasHrdSubSection = await _fBasHrdSubSection.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdSubSection!=null)
            {
                if (await _fBasHrdSubSection.Delete(fBasHrdSubSection))
                {
                    TempData["message"]=$"Successfully Deleted {Title}.";
                    TempData["type"]="success";
                    return RedirectToAction(nameof(GetFBasHrdSubSection));
                }

                TempData["message"]=$"Failed to Delete {Title}.";
                TempData["type"]="error";
                return RedirectToAction(nameof(GetFBasHrdSubSection));
            }

            TempData["message"]=$"{Title} Not Found.";
            TempData["type"]="error";
            return RedirectToAction(nameof(GetFBasHrdSubSection));
        }

        [HttpPost]
        [Route("GetData")]
        public async Task<JsonResult> GetTableData ()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns["+Request.Form["order[0][column]"].FirstOrDefault()+"][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                var pageSize = length!=null ? Convert.ToInt32(length) : 0;
                var skip = start!=null ? Convert.ToInt32(start) : 0;
                var data = await _fBasHrdSubSection.GetAllFBasHrdSubSectionAsync();

                if (!(string.IsNullOrEmpty(sortColumn)&&string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data=sortColumnDirection=="asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn??string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data=data.Where(m => (m.SUBSEC_NAME!=null&&m.SUBSEC_NAME.ToUpper().Contains(searchValue))
                                         ||(m.SUBSEC_NAME_BN!=null&&m.SUBSEC_NAME_BN.ToUpper().Contains(searchValue))
                                         ||(m.SUBSEC_SNAME!=null&&m.SUBSEC_SNAME.ToUpper().Contains(searchValue))
                                         ||(m.SUBSEC_SNAME_BN!=null&&m.SUBSEC_SNAME_BN.ToUpper().Contains(searchValue))
                                         ||(m.DESCRIPTION!=null&&m.DESCRIPTION.ToUpper().Contains(searchValue))
                                         ||(m.REMARKS!=null&&m.REMARKS.ToUpper().Contains(searchValue))).ToList();
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
        public async Task<IActionResult> IsSubSecNameInUse (FBasHrdSubSectionViewModel fBasHrdSubSectionViewModel)
        {
            var subSecName = fBasHrdSubSectionViewModel.FBasHrdSubSection.SUBSEC_NAME;
            return await _fBasHrdSubSection.FindBySubSecNameAsync(subSecName) ? Json(true) : Json($"Section {subSecName} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUseBn")]
        public async Task<IActionResult> IsIsSubSecNameBnInUse (FBasHrdSubSectionViewModel fBasHrdSubSectionViewModel)
        {
            var subsecName = fBasHrdSubSectionViewModel.FBasHrdSubSection.SUBSEC_NAME_BN;
            return await _fBasHrdSubSection.FindBySubSecNameAsync(subsecName, true) ? Json(true) : Json($"ইতিমধ্যে {subsecName} নামের সাব-সেকশন আছে");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetSubSections")]
        public async Task<IActionResult> GetAllSubSections ()
        {
            try
            {
                return Ok(await _fBasHrdSubSection.GetAllSubSectionsAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}