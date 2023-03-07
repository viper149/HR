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
    [Route("ChemicalRequisition")]
    public class FChemPurchaseRequisitionController : Controller
    {
        private readonly IF_CHEM_PURCHASE_REQUISITION_MASTER _fChemPurchaseRequisitionMaster;
        private readonly IDataProtector _protector;
        private readonly IF_CHEM_STORE_PRODUCTINFO _fChemStoreProductinfo;
        private readonly IF_CHEM_STORE_INDENTDETAILS _fChemStoreIndentdetails;
        private readonly UserManager<ApplicationUser> _userManager;

        public FChemPurchaseRequisitionController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_CHEM_PURCHASE_REQUISITION_MASTER fChemPurchaseRequisitionMaster,
            IF_CHEM_STORE_PRODUCTINFO fChemStoreProductinfo,
            IF_CHEM_STORE_INDENTDETAILS fChemStoreIndentdetails,
            UserManager<ApplicationUser> userManager
        )
        {
            _fChemPurchaseRequisitionMaster = fChemPurchaseRequisitionMaster;
            _fChemStoreProductinfo = fChemStoreProductinfo;
            _fChemStoreIndentdetails = fChemStoreIndentdetails;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{indslId?}")]
        public async Task<IActionResult> DetailsFChemPurchaseRequisition(string indslId)
        {
            return View(await _fChemPurchaseRequisitionMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indslId))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFChemPurchaseRequisition(FChemicalRequisitionViewModel fChemicalRequisitionViewModel)
        {
            if (ModelState.IsValid)
            {
                var fChemPurchaseRequisitionMaster = await _fChemPurchaseRequisitionMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.EncryptedId)));

                if (fChemPurchaseRequisitionMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.CREATED_AT = fChemPurchaseRequisitionMaster.CREATED_AT;
                    fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.CREATED_BY = fChemPurchaseRequisitionMaster.CREATED_BY;
                    fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.UPDATED_AT = DateTime.Now;
                    fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.UPDATED_BY = user.Id;

                    if (await _fChemPurchaseRequisitionMaster.Update(fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster))
                    {
                        var fChemStoreIndentdetailses = fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Where(e => e.TRNSID <= 0).ToList();

                        foreach (var item in fChemStoreIndentdetailses)
                        {
                            item.INDSLID = fChemPurchaseRequisitionMaster.INDSLID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        TempData["message"] = "Successfully Updated Purchase Requisition Information.";
                        TempData["type"] = "success";
                        await _fChemStoreIndentdetails.InsertRangeByAsync(fChemStoreIndentdetailses);

                        return RedirectToAction(nameof(GetPurchaseRequisition), $"FChemPurchaseRequisition");
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditFChemPurchaseRequisition), await _fChemPurchaseRequisitionMaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
            }

            return View(nameof(EditFChemPurchaseRequisition), await _fChemPurchaseRequisitionMaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
        }

        [HttpGet]
        [Route("Edit/{indslId?}")]
        public async Task<IActionResult> EditFChemPurchaseRequisition(string indslId)
        {
            try
            {
                return View(await _fChemPurchaseRequisitionMaster.GetInitObjByAsync(await _fChemPurchaseRequisitionMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indslId)), edit: true)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("")]
        [Route("GetAll")]
        public IActionResult GetPurchaseRequisition()
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

                var data = (List<F_CHEM_PURCHASE_REQUISITION_MASTER>)await _fChemPurchaseRequisitionMaster.GetAllChemicalPurchaseRequisitionAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.INDSLID.ToString().ToUpper().Contains(searchValue)
                                           || m.Employee.FIRST_NAME != null && m.Employee.FIRST_NAME.ToUpper().Contains(searchValue)
                                           || m.FBasDepartment.DEPTNAME != null && m.FBasDepartment.DEPTNAME.ToUpper().Contains(searchValue)
                                           || m.FBasSection.SECNAME != null && m.FBasSection.SECNAME.ToUpper().Contains(searchValue)
                                           || m.FBasSubsection.SSECNAME != null && m.FBasSubsection.SSECNAME.ToUpper().Contains(searchValue)
                                           || m.INDSLDATE != null && m.INDSLDATE.ToString().ToUpper().Contains(searchValue)
                                           || m.ConcernEmployee.FIRST_NAME != null && m.ConcernEmployee.FIRST_NAME.ToUpper().Contains(searchValue)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateChemicalPurchaseRequisition()
        {
            try
            {
                return View(await _fChemPurchaseRequisitionMaster.GetInitObjByAsync(new FChemicalRequisitionViewModel()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateChemicalPurchaseRequisition(FChemicalRequisitionViewModel fChemicalRequisitionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.CREATED_AT =
                        fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.UPDATED_AT = DateTime.Now;

                    fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.CREATED_BY =
                        fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster.UPDATED_BY = user.Id;

                    var fChemPurchaseRequisitionMaster = await _fChemPurchaseRequisitionMaster.GetInsertedObjByAsync(fChemicalRequisitionViewModel.FChemPurchaseRequisitionMaster);

                    if (fChemPurchaseRequisitionMaster.INDSLID != 0)
                    {
                        foreach (var item in fChemicalRequisitionViewModel.FChemStoreIndentdetailsList)
                        {
                            item.INDSLID = fChemPurchaseRequisitionMaster.INDSLID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        if (fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Any())
                        {
                            await _fChemStoreIndentdetails.InsertRangeByAsync(fChemicalRequisitionViewModel.FChemStoreIndentdetailsList);
                        }

                        TempData["message"] = "Successfully added Indent and Rnd Purchase Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetPurchaseRequisition), $"FChemPurchaseRequisition");
                    }

                    TempData["message"] = "Failed to Add Indent and Chemical Purchase Information";
                    TempData["type"] = "error";
                    return View(nameof(CreateChemicalPurchaseRequisition), await _fChemPurchaseRequisitionMaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(nameof(CreateChemicalPurchaseRequisition), await _fChemPurchaseRequisitionMaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
            }
            catch (Exception ex)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddIndentDetails(FChemicalRequisitionViewModel fChemicalRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fChemicalRequisitionViewModel.IsDelete)
                {
                    var fChemStoreIndentDetails = fChemicalRequisitionViewModel.FChemStoreIndentdetailsList
                        [fChemicalRequisitionViewModel.RemoveIndex];

                    if (fChemStoreIndentDetails.TRNSID > 0)
                    {
                        await _fChemStoreIndentdetails.Delete(fChemStoreIndentDetails);
                    }

                    fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.RemoveAt(fChemicalRequisitionViewModel.RemoveIndex);
                }
                else if (!fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Any(e => e.PRODUCTID.Equals(fChemicalRequisitionViewModel.FChemStoreIndentdetails.PRODUCTID)))
                {
                    if (TryValidateModel(fChemicalRequisitionViewModel.FChemStoreIndentdetails))
                    {
                        fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Add(fChemicalRequisitionViewModel.FChemStoreIndentdetails);
                    }
                }

                Response.Headers["HasItems"] = $"{fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Any()}";

                return PartialView($"ChemIndentDetailsPartialView", await _fChemStoreProductinfo.GetInitObjForDetailsByAsync(fChemicalRequisitionViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}