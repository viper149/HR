using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    [Route("GeneralStoreRequirement")]
    public class FGenSRequirementController : Controller
    {
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IF_BAS_SUBSECTION _fBasSubsection;
        private readonly IF_GEN_S_REQ_DETAILS _fGenSReqDetails;
        private readonly IF_GEN_S_REQ_MASTER _fGenSReqMaster;
        private readonly IF_HR_EMP_OFFICIALINFO _fHrEmpOfficialinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Requirement Information";

        public FGenSRequirementController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            IF_BAS_SECTION fBasSection,
            IF_BAS_SUBSECTION fBasSubsection,
            IF_GEN_S_REQ_DETAILS fGenSReqDetails,
            IF_GEN_S_REQ_MASTER fGenSReqMaster,
            IF_HR_EMP_OFFICIALINFO fHrEmpOfficialinfo,
            UserManager<ApplicationUser> userManager
            )
        {
            _fGsProductInformation = fGsProductInformation;
            _fBasSection = fBasSection;
            _fBasSubsection = fBasSubsection;
            _fGenSReqDetails = fGenSReqDetails;
            _fGenSReqMaster = fGenSReqMaster;
            _fHrEmpOfficialinfo = fHrEmpOfficialinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{gsrId?}")]
        public async Task<IActionResult> DetailsFGenSRequirement(string gsrId)
        {
            return View(await _fGenSReqMaster.GetInitObjByAsync(await _fGenSReqMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gsrId)))));
        }

        [HttpGet]
        [Route("Edit/{gsrId?}")]
        public async Task<IActionResult> EditFGenSRequirement(string gsrId)
        {
            return View(await _fGenSReqMaster.GetInitObjByAsync(await _fGenSReqMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gsrId)), true)));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFGenSRequirement(FGenSRequirementViewModel fGenSRequirementViewModel)
        {
            if (ModelState.IsValid)
            {
                var fGenSReqMaster = await _fGenSReqMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fGenSRequirementViewModel.FGenSReqMaster.EncryptedId)));

                if (fGenSReqMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fGenSRequirementViewModel.FGenSReqMaster.GSRNO = fGenSReqMaster.GSRNO;
                    fGenSRequirementViewModel.FGenSReqMaster.CREATED_AT = fGenSReqMaster.CREATED_AT;
                    fGenSRequirementViewModel.FGenSReqMaster.CREATED_BY = fGenSReqMaster.CREATED_BY;
                    fGenSRequirementViewModel.FGenSReqMaster.UPDATED_AT = DateTime.Now;
                    fGenSRequirementViewModel.FGenSReqMaster.UPDATED_BY = user.Id;

                    if (await _fGenSReqMaster.Update(fGenSRequirementViewModel.FGenSReqMaster))
                    {
                        var fGenSReqDetailses = fGenSRequirementViewModel.FGenSReqDetailsesList.Where(d => d.GRQID <= 0).ToList();

                        foreach (var item in fGenSReqDetailses)
                        {
                            item.GSRID = fGenSReqMaster.GSRID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        await _fGenSReqDetails.InsertRangeByAsync(fGenSReqDetailses);

                        return RedirectToAction(nameof(GetFGenSRequirement), $"FGenSRequirement");
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditFGenSRequirement), await _fGenSReqMaster.GetInitObjByAsync(fGenSRequirementViewModel));
            }

            return View(nameof(EditFGenSRequirement), await _fGenSReqMaster.GetInitObjByAsync(fGenSRequirementViewModel));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFGenSRequirement()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("GetTableData")]
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

            var data = await _fGenSReqMaster.GetAllFGenSRequirementAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.GSRNO != null && m.GSRNO.ToUpper().Contains(searchValue)
                                       || m.GSRDATE != null && m.GSRDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.EMP != null && m.EMP.FIRST_NAME.ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
        [Route("Create")]
        public async Task<IActionResult> CreateFGenSRequirement()
        {
            try
            {
                return View(await _fGenSReqMaster.GetInitObjByAsync(new FGenSRequirementViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFGenSRequirement(FGenSRequirementViewModel fFGenSRequirementViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fFGenSRequirementViewModel.FGenSReqMaster.CREATED_BY = fFGenSRequirementViewModel.FGenSReqMaster.UPDATED_BY = user.Id;
                fFGenSRequirementViewModel.FGenSReqMaster.CREATED_AT = fFGenSRequirementViewModel.FGenSReqMaster.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var fGenSReqMaster = await _fGenSReqMaster.GetInsertedObjByAsync(fFGenSRequirementViewModel.FGenSReqMaster);
                fGenSReqMaster.GSRNO = fGenSReqMaster.GSRID.ToString();
                await _fGenSReqMaster.Update(fGenSReqMaster);

                if (fGenSReqMaster.GSRID != 0)
                {
                    foreach (var item in fFGenSRequirementViewModel.FGenSReqDetailsesList)
                    {
                        item.GSRID = fGenSReqMaster.GSRID;
                        item.STATUS = "REQUIRED";
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        if (await _fGenSReqDetails.InsertByAsync(item))
                        {
                            atLeastOneInsert = true;
                        }
                    }
                    if (!atLeastOneInsert)
                    {
                        await _fGenSReqMaster.Delete(fGenSReqMaster);
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(CreateFGenSRequirement), await _fGenSReqMaster.GetInitObjByAsync(fFGenSRequirementViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFGenSRequirement), $"FGenSRequirement");
                }

                await _fGenSReqMaster.Delete(fGenSReqMaster);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateFGenSRequirement), await _fGenSReqMaster.GetInitObjByAsync(fFGenSRequirementViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetSubSections/{sectionId?}")]
        public async Task<IActionResult> GetSubSections(int sectionId)
        {
            try
            {
                return Ok(await _fBasSubsection.GetSubSectionsBySectionIdAsync(sectionId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetSections")]
        public async Task<IActionResult> GetSections(int id)
        {
            try
            {
                return Ok(await _fBasSection.GetSectionsByDeptIdAsync(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddToList(FGenSRequirementViewModel fGenSRequirementViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGenSRequirementViewModel.IsDelete)
                {
                    var fGenSReqDetails = fGenSRequirementViewModel.FGenSReqDetailsesList[fGenSRequirementViewModel.RemoveIndex];

                    if (fGenSReqDetails.GRQID > 0)
                    {
                        await _fGenSReqDetails.Delete(fGenSReqDetails);
                    }

                    fGenSRequirementViewModel.FGenSReqDetailsesList.RemoveAt(fGenSRequirementViewModel.RemoveIndex);
                }
                else if (!fGenSRequirementViewModel.FGenSReqDetailsesList.Any(e => e.PRODUCTID.Equals(fGenSRequirementViewModel.FGenSReqDetails.PRODUCTID)))
                {
                    if (TryValidateModel(fGenSRequirementViewModel.FGenSReqDetails))
                    {
                        fGenSRequirementViewModel.FGenSReqDetailsesList.Add(fGenSRequirementViewModel.FGenSReqDetails);
                    }
                }

                return PartialView($"GenSRequirementDetailsPartialView", await _fGenSReqMaster.GetInitDetailsObjByAsync(fGenSRequirementViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetProductDetails/{id?}")]
        public async Task<IActionResult> GetSingleProductDetailsByPid(int id)
        {
            try
            {
                return Ok(await _fGsProductInformation.GetSingleProductByProductId(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetEmpInfo/{id?}")]
        public async Task<F_HR_EMP_OFFICIALINFO> GetEmpById(int id)
        {
            return await _fHrEmpOfficialinfo.GetSingleEmployeeDetails(id);
        }

        [HttpGet]
        [Route("GSRequirementReport/{gsrId?}")]
        [HttpGet]
        public async Task<IActionResult> RGenSReqReport(string gsrId)
        {
            var gsrNo = string.Empty;

            if (gsrId != null)
                gsrNo = await Task.Run(() => _fGenSReqMaster.FindByIdAsync(int.Parse(_protector.Unprotect(gsrId))).Result.GSRNO);
            return View(model: gsrNo);
        }
    }
}
