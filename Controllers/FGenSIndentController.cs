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
    [Route("GeneralStoreIndent")]
    public class FGenSIndentController : Controller
    {
        private readonly IF_GEN_S_PURCHASE_REQUISITION_MASTER _fGenSPurchaseRequisitionMaster;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly IF_GEN_S_INDENTMASTER _fGenSIndentmaster;
        private readonly IF_GEN_S_INDENTDETAILS _fGenSIndentdetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Indent Information";

        public FGenSIndentController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_GEN_S_PURCHASE_REQUISITION_MASTER fGenSPurchaseRequisitionMaster,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            IF_GEN_S_INDENTMASTER fGenSIndentmaster,
            IF_GEN_S_INDENTDETAILS fGenSIndentdetails,
            UserManager<ApplicationUser> userManager
            )
        {
            _fGenSPurchaseRequisitionMaster = fGenSPurchaseRequisitionMaster;
            _fGsProductInformation = fGsProductInformation;
            _fGenSIndentmaster = fGenSIndentmaster;
            _fGenSIndentdetails = fGenSIndentdetails;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{id?}")]
        public async Task<IActionResult> DetailsFGenSIndent(string id)
        {
            return View(await _fGenSIndentmaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id))));
        }

        [HttpGet]
        [Route("Edit/{gindId?}")]
        public async Task<IActionResult> EditFGenSIndent(string gindId)
        {
            return View(await _fGenSIndentmaster.GetInitEditObjByAsync(await _fGenSIndentmaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gindId)))));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFGenSIndent()
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

            var data = await _fGenSIndentmaster.GetAllGenSIndentAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.INDSLID.ToString().ToUpper().Contains(searchValue)
                                       || m.GINDDATE != null && m.GINDDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.INDTYPE != null && m.INDTYPE.ToString().ToUpper().Contains(searchValue)
                                       || m.GINDNO != null && m.GINDNO.ToUpper().Contains(searchValue)
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
        public async Task<IActionResult> CreateFGenSIndent()
        {
            var fGenSRequisitionViewModel = new FGenSRequisitionViewModel();
            fGenSRequisitionViewModel.FGenSIndentmaster.GINDNO = $"{await _fGenSIndentmaster.GetLastGenSIndentNo() + 1}";
            return View(await _fGenSIndentmaster.GetInitObjByAsync(fGenSRequisitionViewModel));
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFGenSIndent(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            try
            {
                if (fGenSRequisitionViewModel.FGenSIndentmaster.INDSLID != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fGenSRequisitionViewModel.FGenSIndentmaster.CREATED_BY = fGenSRequisitionViewModel.FGenSIndentmaster.UPDATED_BY = user.Id;
                    fGenSRequisitionViewModel.FGenSIndentmaster.CREATED_AT = fGenSRequisitionViewModel.FGenSIndentmaster.UPDATED_AT = DateTime.Now;

                    var atLeastOneInsert = false;

                    var fGenSIndentmaster = await _fGenSIndentmaster.GetInsertedObjByAsync(fGenSRequisitionViewModel.FGenSIndentmaster);

                    if (fGenSIndentmaster.INDSLID > 0)
                    {
                        var fGenSPurchaseRequisitionMaster = await _fGenSPurchaseRequisitionMaster.FindByIdAsync(fGenSIndentmaster.INDSLID ?? 0);
                        fGenSPurchaseRequisitionMaster.STATUS = true;
                        await _fGenSPurchaseRequisitionMaster.Update(fGenSPurchaseRequisitionMaster);

                        foreach (var item in fGenSRequisitionViewModel.FGenSIndentdetailsesList)
                        {
                            if (item.TRNSID > 0)
                            {
                                var fGenSIndentdetails = await _fGenSIndentdetails.FindByIdAsync(item.TRNSID);

                                if (fGenSIndentdetails != null)
                                {
                                    fGenSIndentdetails.INDSLID = fGenSRequisitionViewModel.FGenSIndentmaster.INDSLID;
                                    fGenSIndentdetails.TRNSDATE = item.TRNSDATE;
                                    fGenSIndentdetails.GINDID = fGenSIndentmaster.GINDID;
                                    fGenSIndentdetails.PRODUCTID = item.PRODUCTID;
                                    fGenSIndentdetails.UNIT = item.UNIT;
                                    fGenSIndentdetails.QTY = item.QTY;
                                    fGenSIndentdetails.VALIDITY = item.VALIDITY;
                                    fGenSIndentdetails.FULL_QTY = item.FULL_QTY;
                                    fGenSIndentdetails.ADD_QTY = item.ADD_QTY;
                                    fGenSIndentdetails.BAL_QTY = item.BAL_QTY;
                                    fGenSIndentdetails.LOCATION = item.LOCATION;
                                    fGenSIndentdetails.REMARKS = item.REMARKS;
                                    fGenSIndentdetails.CREATED_BY = fGenSIndentdetails.UPDATED_BY = user.Id;
                                    fGenSIndentdetails.CREATED_AT = fGenSIndentdetails.UPDATED_AT = DateTime.Now;

                                    if (await _fGenSIndentdetails.Update(fGenSIndentdetails))
                                    {
                                        atLeastOneInsert = true;
                                    }
                                }
                            }
                        }

                        if (!atLeastOneInsert)
                        {
                            fGenSPurchaseRequisitionMaster.STATUS = false;
                            await _fGenSPurchaseRequisitionMaster.Update(fGenSPurchaseRequisitionMaster);
                            await _fGenSIndentmaster.Delete(fGenSIndentmaster);
                            TempData["message"] = $"Failed to Add {title}.";
                            TempData["type"] = "error";
                            return View(nameof(CreateFGenSIndent), await _fGenSIndentmaster.GetInitObjByAsync(fGenSRequisitionViewModel));
                        }

                        TempData["message"] = $"Successfully added {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGenSIndent), $"FGenSIndent");
                    }

                    await _fGenSIndentmaster.Delete(fGenSIndentmaster);
                    TempData["message"] = $"Failed to Add {title}.";
                    TempData["type"] = "error";
                    return View(nameof(CreateFGenSIndent), await _fGenSIndentmaster.GetInitObjByAsync(fGenSRequisitionViewModel));
                }

                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateFGenSIndent), await _fGenSIndentmaster.GetInitObjByAsync(fGenSRequisitionViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("AddOrRemoveFGenSIndentDetailsList")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFGenSIndentDetailsList(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGenSRequisitionViewModel.IsDelete)
                {
                    var fGenSIndentdetails = fGenSRequisitionViewModel.FGenSIndentdetailsesList[fGenSRequisitionViewModel.RemoveIndex];

                    if (fGenSIndentdetails.TRNSID > 0)
                    {
                        await _fGenSIndentdetails.Delete(fGenSIndentdetails);
                    }

                    fGenSRequisitionViewModel.FGenSIndentdetailsesList.RemoveAt(fGenSRequisitionViewModel.RemoveIndex);
                }
                else
                {
                    if (!fGenSRequisitionViewModel.FGenSIndentdetailsesList.Any(e => e.PRODUCTID.Equals(fGenSRequisitionViewModel.FGenSIndentdetails.PRODUCTID)))
                    {
                        if (TryValidateModel(fGenSRequisitionViewModel.FGenSIndentdetails))
                        {
                            fGenSRequisitionViewModel.FGenSIndentdetailsesList.Add(fGenSRequisitionViewModel.FGenSIndentdetails);
                        }
                    }
                }

                foreach (var item in fGenSRequisitionViewModel.FGenSIndentdetailsesList)
                {
                    item.PRODUCT = await _fGsProductInformation.FindByIdAsync(item.PRODUCTID ?? 0);
                }

                return PartialView($"FGenSDetailsPartialView", await _fGenSIndentdetails.GetInitObjForDetails(fGenSRequisitionViewModel));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetFGenSRequirements/{id?}")]
        public async Task<IActionResult> GetFGenSPurReqMasterById(int id)
        {
            try
            {
                return Ok(await _fGenSPurchaseRequisitionMaster.GetFGenSPurReqMasterById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetFGenSIndentDetails/{id?}/{prdId?}")]
        public async Task<IActionResult> GetFGenSIndentDetailsById(int id, int prdId)
        {
            try
            {
                return Ok(await _fGenSIndentdetails.FindFGenSIndentListByIdAsync(id, prdId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GSIndentReport/{gindId?}")]
        public async Task<IActionResult> RGenSIndentReport(string gindId)
        {
            var gindNo = string.Empty;

            if (gindId != null)
                gindNo = await Task.Run(() => _fGenSIndentmaster.FindByIdAsync(int.Parse(_protector.Unprotect(gindId))).Result.GINDNO);
            return View(model: gindNo);
        }
    }
}
