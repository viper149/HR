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
    [Route("ChemicalIndent")]
    public class FChemStoreIndentController : Controller
    {
        private readonly IF_CHEM_PURCHASE_REQUISITION_MASTER _fChemPurchaseRequisitionMaster;
        private readonly IDataProtector _protector;
        private readonly IF_CHEM_STORE_INDENTDETAILS _fChemStoreIndentdetails;
        private readonly IF_CHEM_STORE_PRODUCTINFO _fChemStoreProductinfo;
        private readonly IF_CHEM_STORE_INDENTMASTER _fChemStoreIndentmaster;
        private readonly UserManager<ApplicationUser> _userManager;

        public FChemStoreIndentController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_CHEM_PURCHASE_REQUISITION_MASTER fChemPurchaseRequisitionMaster,
            IF_CHEM_STORE_PRODUCTINFO fChemStoreProductinfo,
            IF_CHEM_STORE_INDENTMASTER fChemStoreIndentmaster,
            IF_CHEM_STORE_INDENTDETAILS fChemStoreIndentdetails,
            UserManager<ApplicationUser> userManager
        )
        {
            _fChemPurchaseRequisitionMaster = fChemPurchaseRequisitionMaster;
            _fChemStoreIndentdetails = fChemStoreIndentdetails;
            _fChemStoreIndentmaster = fChemStoreIndentmaster;
            _fChemStoreProductinfo = fChemStoreProductinfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Details/{cindId?}")]
        public IActionResult DetailsFChemStoreIndent(string cindId)
        {
            return View();
        }

        [HttpGet]
        [Route("Edit/{cindId?}")]
        public async Task<IActionResult> EditFChemStoreIndent(string cindId)
        {
            return View(await _fChemStoreIndentmaster.GetInitObjByAsync(await _fChemStoreIndentmaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(cindId)))));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetIndentMaster()
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

                var data = (List<F_CHEM_STORE_INDENTMASTER>)await _fChemStoreIndentmaster.GetAllChemicalIndentAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.INDSLID.ToString().ToUpper().Contains(searchValue)
                                           || m.CINDDATE != null && m.CINDDATE.ToString().ToUpper().Contains(searchValue)
                                           || m.INDTYPE != null && m.INDTYPE.ToString().ToUpper().Contains(searchValue)
                                           || m.CINDNO != null && m.CINDNO.ToUpper().Contains(searchValue)
                                           || m.STATUS.ToString().ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.CINDID.ToString());
                }

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
        public async Task<IActionResult> CreateChemicalIndent()
        {
            var fChemicalRequisitionViewModel = new FChemicalRequisitionViewModel();
            fChemicalRequisitionViewModel.FChemStoreIndentmaster.CINDNO = $"{await _fChemStoreIndentmaster.GetLastChmIndNo()}";
            return View(await _fChemStoreIndentmaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateChemicalIndent(FChemicalRequisitionViewModel fChemicalRequisitionViewModel)
        {
            try
            {
                if (fChemicalRequisitionViewModel.FChemStoreIndentmaster.INDSLID != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fChemicalRequisitionViewModel.FChemStoreIndentmaster.CREATED_BY = fChemicalRequisitionViewModel.FChemStoreIndentmaster.UPDATED_BY = user.Id;
                    fChemicalRequisitionViewModel.FChemStoreIndentmaster.CREATED_AT = fChemicalRequisitionViewModel.FChemStoreIndentmaster.UPDATED_AT = DateTime.Now;

                    var fChemStoreIndentmaster = await _fChemStoreIndentmaster.GetInsertedObjByAsync(fChemicalRequisitionViewModel.FChemStoreIndentmaster);

                    if (fChemStoreIndentmaster.INDSLID > 0)
                    {
                        var fChemPurchaseRequisitionMaster = await _fChemPurchaseRequisitionMaster.FindByIdAsync(fChemStoreIndentmaster.INDSLID ?? 0);
                        fChemPurchaseRequisitionMaster.STATUS = true;
                        await _fChemPurchaseRequisitionMaster.Update(fChemPurchaseRequisitionMaster);

                        foreach (var item in fChemicalRequisitionViewModel.FChemStoreIndentdetailsList)
                        {
                            if (item.TRNSID > 0)
                            {
                                var fChemStoreIndentdetails = await _fChemStoreIndentdetails.FindByIdAsync(item.TRNSID);

                                if (fChemStoreIndentdetails != null)
                                {
                                    fChemStoreIndentdetails.INDSLID = fChemicalRequisitionViewModel.FChemStoreIndentmaster.INDSLID;
                                    fChemStoreIndentdetails.TRNSDATE = item.TRNSDATE;
                                    fChemStoreIndentdetails.CINDID = fChemStoreIndentmaster.CINDID;
                                    fChemStoreIndentdetails.PRODUCTID = item.PRODUCTID;
                                    fChemStoreIndentdetails.UNIT = item.UNIT;
                                    fChemStoreIndentdetails.QTY = item.QTY;
                                    fChemStoreIndentdetails.VALIDITY = item.VALIDITY;
                                    fChemStoreIndentdetails.FULL_QTY = item.FULL_QTY;
                                    fChemStoreIndentdetails.ADD_QTY = item.ADD_QTY;
                                    fChemStoreIndentdetails.BAL_QTY = item.BAL_QTY;
                                    fChemStoreIndentdetails.LOCATION = item.LOCATION;
                                    fChemStoreIndentdetails.REMARKS = item.REMARKS;
  
                                    fChemStoreIndentdetails.UPDATED_BY = user.Id;
                                    fChemStoreIndentdetails.UPDATED_AT = DateTime.Now;

                                    await _fChemStoreIndentdetails.Update(fChemStoreIndentdetails);
                                }
                                else
                                {
                                    TempData["message"] = "Invalid Data";
                                    TempData["type"] = "error";
                                    return View(nameof(CreateChemicalIndent), await _fChemStoreIndentmaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
                                }
                            }
                            else
                            {
                                TempData["message"] = "Failed to Edit Chemical Indent Details";
                                TempData["type"] = "error";
                                return View(nameof(CreateChemicalIndent), await _fChemStoreIndentmaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
                            }
                        }

                        TempData["message"] = "Successfully added New Chemical Indent Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetIndentMaster), $"FChemStoreIndent");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Chemical Indent Information";
                        TempData["type"] = "error";
                        return View(nameof(CreateChemicalIndent), await _fChemStoreIndentmaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
                    }
                }
                else
                {
                    TempData["message"] = "Failed to Add Indent Master Information";
                    TempData["type"] = "error";
                    return View(nameof(CreateChemicalIndent), await _fChemStoreIndentmaster.GetInitObjByAsync(fChemicalRequisitionViewModel));
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("AddOrRemoveChemIndentDetailsList")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToChemIndentDetailsList(FChemicalRequisitionViewModel fChemicalRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fChemicalRequisitionViewModel.IsDelete)
                {
                    var fChemStoreIndentdetails = fChemicalRequisitionViewModel.FChemStoreIndentdetailsList[fChemicalRequisitionViewModel.RemoveIndex];

                    if (fChemStoreIndentdetails.TRNSID > 0)
                    {
                        //await _fChemStoreIndentdetails.Delete(fChemStoreIndentdetails);
                    }

                    fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.RemoveAt(fChemicalRequisitionViewModel.RemoveIndex);
                }
                else
                {
                    if (!fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Any(e => e.PRODUCTID.Equals(fChemicalRequisitionViewModel.FChemStoreIndentdetails.PRODUCTID)))
                    {
                        fChemicalRequisitionViewModel.FChemStoreIndentdetailsList.Add(fChemicalRequisitionViewModel.FChemStoreIndentdetails);
                    }
                }

                foreach (var item in fChemicalRequisitionViewModel.FChemStoreIndentdetailsList)
                {
                    item.PRODUCT = await _fChemStoreProductinfo.FindByIdAsync(item.PRODUCTID ?? 0);
                }

                return PartialView($"GetIndentDetailsPartialView", fChemicalRequisitionViewModel);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetChemicalRequirements/{id?}")]
        public async Task<IActionResult> GetChemPurReqMasterById(int id)
        {
            try
            {
                return Ok(await _fChemPurchaseRequisitionMaster.GetChemPurReqMasterById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetChemicalIndentDetails/{id?}/{prdId?}")]
        public async Task<IActionResult> GetChemIndentDetailsById(int id, int prdId)
        {
            try
            {
                return Ok(await _fChemStoreIndentdetails.FindChemIndentListByIdAsync(id, prdId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}