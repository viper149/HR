using System;
using System.Collections.Generic;
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
    [Route("ChemicalRequirement")]
    public class FChemRequirementController : Controller
    {
        private readonly IF_CHEM_STORE_PRODUCTINFO _fChemStoreProductinfo;
        private readonly IF_CHEM_REQ_MASTER _fChemReqMaster;
        private readonly IF_CHEM_REQ_DETAILS _fChemReqDetails;
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public FChemRequirementController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_CHEM_STORE_PRODUCTINFO fChemStoreProductinfo,
            IF_BAS_SECTION fBasSection,
            IF_CHEM_REQ_DETAILS fChemReqDetails,
            IF_CHEM_REQ_MASTER fChemReqMaster,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fChemStoreProductinfo = fChemStoreProductinfo;
            _fBasSection = fBasSection;
            _fChemReqDetails = fChemReqDetails;
            _fChemReqMaster = fChemReqMaster;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Details/{csrId?}")]
        public async Task<IActionResult> DetailsFChemRequirement(string csrId)
        {
            return View(await _fChemReqMaster.GetInitObjByAsync(await _fChemReqMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(csrId)))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFChemRequirement(FChemRequirementViewModel fChemRequirementViewModel)
        {
            if (ModelState.IsValid)
            {
                var fChemReqMaster = await _fChemReqMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fChemRequirementViewModel.FChemReqMaster.EncryptedId)));

                if (fChemReqMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fChemRequirementViewModel.FChemReqMaster.CSRID = fChemReqMaster.CSRID;
                    fChemRequirementViewModel.FChemReqMaster.CSRNO = fChemReqMaster.CSRNO;
                    fChemRequirementViewModel.FChemReqMaster.CREATED_AT = fChemReqMaster.CREATED_AT;
                    fChemRequirementViewModel.FChemReqMaster.CREATED_BY = fChemReqMaster.CREATED_BY;
                    fChemRequirementViewModel.FChemReqMaster.UPDATED_AT = DateTime.Now;
                    fChemRequirementViewModel.FChemReqMaster.UPDATED_BY = user.Id;

                    if (await _fChemReqMaster.Update(fChemRequirementViewModel.FChemReqMaster))
                    {
                        var fChemReqDetailses = fChemRequirementViewModel.FChemReqDetailsList.Where(e => e.CRQID <= 0).ToList();

                        foreach (var item in fChemReqDetailses)
                        {
                            item.CSRID = fChemReqMaster.CSRID;
                            item.STATUS = "REQUIRED";
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                        }

                        if (fChemReqDetailses.Any())
                        {
                            await _fChemReqDetails.InsertRangeByAsync(fChemReqDetailses);
                        }

                        TempData["message"] = "Successfully Updated Chemical Requirement Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetChemicalRequirement), $"FChemRequirement");
                    }

                    TempData["message"] = "Failed To Update Chemical Requirement Information.";
                    TempData["type"] = "error";
                }
                else
                {
                    TempData["message"] = "Failed To Update Chemical Requirement Information.";
                    TempData["type"] = "error";
                }
            }
            else
            {
                TempData["message"] = "Failed To Update Chemical Requirement Information.";
                TempData["type"] = "error";
            }

            return View(nameof(EditFChemRequirement), await _fChemReqMaster.GetInitObjByAsync(fChemRequirementViewModel));
        }

        [HttpGet]
        [Route("Edit/{csrId?}")]
        public async Task<IActionResult> EditFChemRequirement(string csrId)
        {
            return View(await _fChemReqMaster.GetInitObjByAsync(
                await _fChemReqMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(csrId)))));
        }

        [HttpGet]
        [Route("")]
        [Route("GetAll")]
        public IActionResult GetChemicalRequirement()
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

                var data = (List<F_CHEM_REQ_MASTER>)await _fChemReqMaster.GetAllChemicalRequirementAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CSRNO.ToString().ToUpper().Contains(searchValue)
                                           || m.CSRDATE != null && m.CSRDATE.ToString().ToUpper().Contains(searchValue)
                                           || m.DEPT.DEPTNAME != null && m.DEPT.DEPTNAME.ToString().ToUpper().Contains(searchValue)
                                           || m.FBasSection.SECNAME != null && m.FBasSection.SECNAME.ToString().ToUpper().Contains(searchValue)
                                           || m.FBasSubsection.SSECNAME != null && m.FBasSubsection.SSECNAME.ToString().ToUpper().Contains(searchValue)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateChemRequirement()
        {
            try
            {
                return View(await _fChemReqMaster.GetInitObjByAsync(new FChemRequirementViewModel()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateChemRequirement(FChemRequirementViewModel fChemRequirementViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fChemRequirementViewModel.FChemReqMaster.CREATED_BY = fChemRequirementViewModel.FChemReqMaster.UPDATED_BY = user.Id;
                fChemRequirementViewModel.FChemReqMaster.CREATED_AT = fChemRequirementViewModel.FChemReqMaster.UPDATED_AT = DateTime.Now;

                var fChemReqMaster = await _fChemReqMaster.GetInsertedObjByAsync(fChemRequirementViewModel.FChemReqMaster);
                fChemReqMaster.CSRNO = fChemReqMaster.CSRID.ToString();
                await _fChemReqMaster.Update(fChemReqMaster);

                if (fChemReqMaster.CSRID != 0)
                {
                    foreach (var item in fChemRequirementViewModel.FChemReqDetailsList)
                    {
                        item.CSRID = fChemReqMaster.CSRID;
                        item.STATUS = "REQUIRED";
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        await _fChemReqDetails.InsertByAsync(item);
                    }

                    TempData["message"] = "Successfully added Chemical Requirement Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetChemicalRequirement), $"FChemRequirement");
                }
                else
                {
                    TempData["message"] = "Failed to add Chemical Requirement Information.";
                    TempData["type"] = "error";
                    return View(nameof(CreateChemRequirement), await _fChemReqMaster.GetInitObjByAsync(fChemRequirementViewModel));
                }
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
                return Ok(await _fChemReqMaster.GetSubSectionsBySectionIdAsync(sectionId));
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

        [AcceptVerbs("Get", "Post")]
        [Route("GetUnitWithBalance")]
        public async Task<IActionResult> GetUnitWithBalance(int id)
        {
            try
            {
                return Ok(await _fChemStoreProductinfo.GetSingleProductDetailsByPID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddToList(FChemRequirementViewModel fChemRequirementViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fChemRequirementViewModel.IsDelete)
                {
                    var fChemReqDetails = fChemRequirementViewModel.FChemReqDetailsList[fChemRequirementViewModel.RemoveIndex];

                    if (fChemReqDetails.CRQID > 0)
                    {
                        await _fChemReqDetails.Delete(fChemReqDetails);
                    }

                    fChemRequirementViewModel.FChemReqDetailsList.RemoveAt(fChemRequirementViewModel.RemoveIndex);
                }
                else if (!fChemRequirementViewModel.FChemReqDetailsList.Any(e => e.PRODUCTID.Equals(fChemRequirementViewModel.FChemReqDetails.PRODUCTID)))
                {
                    if (TryValidateModel(fChemRequirementViewModel.FChemReqDetails))
                    {
                        fChemRequirementViewModel.FChemReqDetailsList.Add(fChemRequirementViewModel.FChemReqDetails);
                    }
                }

                return PartialView($"ChemicalRequirementDetailsPartialView", await _fChemReqMaster.GetInitDetailsObjByAsync(fChemRequirementViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}