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
    [Route("GeneralStoreRequisition")]
    public class FGenSPurchaseRequisitionController : Controller
    {
        private readonly IF_GEN_S_PURCHASE_REQUISITION_MASTER _fGenSPurchaseRequisitionMaster;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly IF_GEN_S_INDENTDETAILS _fGenSIndentdetails;
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IF_BAS_SUBSECTION _fBasSubsection;
        private readonly IF_HR_EMP_OFFICIALINFO _fHrEmpOfficialinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Purchase Requisition Information";

        public FGenSPurchaseRequisitionController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_GEN_S_PURCHASE_REQUISITION_MASTER fGenSPurchaseRequisitionMaster,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            IF_GEN_S_INDENTDETAILS fGenSIndentdetails,
            IF_BAS_SECTION fBasSection,
            IF_BAS_SUBSECTION fBasSubsection,
            IF_HR_EMP_OFFICIALINFO fHrEmpOfficialinfo,
            UserManager<ApplicationUser> userManager)
        {
            _fGenSPurchaseRequisitionMaster = fGenSPurchaseRequisitionMaster;
            _fGsProductInformation = fGsProductInformation;
            _fGenSIndentdetails = fGenSIndentdetails;
            _fBasSection = fBasSection;
            _fBasSubsection = fBasSubsection;
            _fHrEmpOfficialinfo = fHrEmpOfficialinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{indslId?}")]
        public async Task<IActionResult> DetailsFGenSPurchaseRequisition(string indslId)
        {
            return View(await _fGenSPurchaseRequisitionMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indslId))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFGenSPurchaseRequisition(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            if (ModelState.IsValid)
            {
                var fGenSPurchaseRequisitionMaster = await _fGenSPurchaseRequisitionMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.EncryptedId)));

                if (fGenSPurchaseRequisitionMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.CREATED_AT = fGenSPurchaseRequisitionMaster.CREATED_AT;
                    fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.CREATED_BY = fGenSPurchaseRequisitionMaster.CREATED_BY;
                    fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.UPDATED_AT = DateTime.Now;
                    fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.UPDATED_BY = user.Id;

                    if (await _fGenSPurchaseRequisitionMaster.Update(fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster))
                    {
                        var fGenSIndentdetailses = fGenSRequisitionViewModel.FGenSIndentdetailsesList.Where(d => d.TRNSID <= 0).ToList();

                        foreach (var item in fGenSIndentdetailses)
                        {
                            item.INDSLID = fGenSPurchaseRequisitionMaster.INDSLID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        await _fGenSIndentdetails.InsertRangeByAsync(fGenSIndentdetailses);

                        return RedirectToAction(nameof(GetFGenSPurchaseRequisition), $"FGenSPurchaseRequisition");
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditFGenSPurchaseRequisition), await _fGenSPurchaseRequisitionMaster.GetInitObjByAsync(fGenSRequisitionViewModel));
            }

            return View(nameof(EditFGenSPurchaseRequisition), await _fGenSPurchaseRequisitionMaster.GetInitObjByAsync(fGenSRequisitionViewModel));
        }

        [HttpGet]
        [Route("Edit/{indslId?}")]
        public async Task<IActionResult> EditFGenSPurchaseRequisition(string indslId)
        {
            try
            {
                return View(await _fGenSPurchaseRequisitionMaster.GetInitObjByAsync(await _fGenSPurchaseRequisitionMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indslId)), true)));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("")]
        [Route("GetAll")]
        public IActionResult GetFGenSPurchaseRequisition()
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

            var data = await _fGenSPurchaseRequisitionMaster.GetAllGenSPurchaseRequisitionAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.INDSLID.ToString().ToUpper().Contains(searchValue)
                                       || m.EMP.FIRST_NAME != null && m.EMP.FIRST_NAME.ToUpper().Contains(searchValue)
                                       || m.INDSLDATE != null && m.INDSLDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.CN_PERSONNavigation.FIRST_NAME != null && m.CN_PERSONNavigation.FIRST_NAME.ToUpper().Contains(searchValue)
                                       || m.STATUS.ToString().ToUpper().Contains(searchValue)
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
        public async Task<IActionResult> CreateFGenSPurchaseRequisition()
        {
            try
            {
                return View(await _fGenSPurchaseRequisitionMaster.GetInitObjByAsync(new FGenSRequisitionViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFGenSPurchaseRequisition(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.CREATED_AT =
                    fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.UPDATED_AT = DateTime.Now;

                fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.CREATED_BY =
                    fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster.UPDATED_BY = user.Id;

                var atLeastOneInsert = false;

                var fGenSPurchaseRequisitionMaster = await _fGenSPurchaseRequisitionMaster.GetInsertedObjByAsync(fGenSRequisitionViewModel.FGenSPurchaseRequisitionMaster);

                if (fGenSPurchaseRequisitionMaster.INDSLID != 0)
                {
                    foreach (var item in fGenSRequisitionViewModel.FGenSIndentdetailsesList)
                    {
                        item.INDSLID = fGenSPurchaseRequisitionMaster.INDSLID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                    }

                    if (fGenSRequisitionViewModel.FGenSIndentdetailsesList.Any())
                    {
                        if (await _fGenSIndentdetails.InsertRangeByAsync(fGenSRequisitionViewModel.FGenSIndentdetailsesList))
                        {
                            atLeastOneInsert = true;
                        }
                    }

                    if (!atLeastOneInsert)
                    {
                        await _fGenSPurchaseRequisitionMaster.Delete(fGenSPurchaseRequisitionMaster);
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(CreateFGenSPurchaseRequisition), await _fGenSPurchaseRequisitionMaster.GetInitObjByAsync(fGenSRequisitionViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFGenSPurchaseRequisition), $"FGenSPurchaseRequisition");
                }

                await _fGenSPurchaseRequisitionMaster.Delete(fGenSPurchaseRequisitionMaster);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateFGenSPurchaseRequisition), await _fGenSPurchaseRequisitionMaster.GetInitObjByAsync(fGenSRequisitionViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddIndentDetails(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGenSRequisitionViewModel.IsDelete)
                {
                    var fGenSIndentdetails = fGenSRequisitionViewModel.FGenSIndentdetailsesList
                        [fGenSRequisitionViewModel.RemoveIndex];

                    if (fGenSIndentdetails.TRNSID > 0)
                    {
                        await _fGenSIndentdetails.Delete(fGenSIndentdetails);
                    }

                    fGenSRequisitionViewModel.FGenSIndentdetailsesList.RemoveAt(fGenSRequisitionViewModel.RemoveIndex);
                }
                else if (!fGenSRequisitionViewModel.FGenSIndentdetailsesList.Any(e => e.PRODUCTID.Equals(fGenSRequisitionViewModel.FGenSIndentdetails.PRODUCTID)))
                {
                    if (TryValidateModel(fGenSRequisitionViewModel.FGenSIndentdetails))
                    {
                        fGenSRequisitionViewModel.FGenSIndentdetailsesList.Add(fGenSRequisitionViewModel.FGenSIndentdetails);
                    }

                    //var errors = ModelState.Select(x => x.Value.Errors)
                    //    .Where(y => y.Count > 0)
                    //    .ToList();
                }

                Response.Headers["HasItems"] = $"{fGenSRequisitionViewModel.FGenSIndentdetailsesList.Any()}";

                return PartialView($"FGenSIndentDetailsPartialView", await _fGsProductInformation.GetInitObjForDetailsByAsync(fGenSRequisitionViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
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
        [Route("GetSections/{id?}")]
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
    }
}
