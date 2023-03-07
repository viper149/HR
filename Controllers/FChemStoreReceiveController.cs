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
    [Route("ChemicalReceive")]
    public class FChemStoreReceiveController : Controller
    {
        private readonly ICOM_IMP_INVDETAILS _comImpInvdetails;
        private readonly IF_CHEM_STORE_PRODUCTINFO _fChemStoreProductinfo;
        private readonly IF_CHEM_TRANSECTION _fChemTransection;
        private readonly IF_CHEM_QC_APPROVE _fChemQcApprove;
        private readonly IF_CS_CHEM_RECEIVE_REPORT _fCsChemReceiveReport;
        private readonly IF_CHEM_STORE_INDENTMASTER _fChemStoreIndentmaster;
        private readonly IF_CHEM_STORE_RECEIVE_MASTER _fChemStoreReceiveMaster;
        private readonly IF_CHEM_STORE_RECEIVE_DETAILS _fChemStoreReceiveDetails;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public FChemStoreReceiveController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_IMP_INVDETAILS comImpInvdetails,
            IF_CHEM_TRANSECTION fChemTransection,
            IF_CS_CHEM_RECEIVE_REPORT fCsChemReceiveReport,
            IF_CHEM_STORE_RECEIVE_MASTER fChemStoreReceiveMaster,
            IF_CHEM_STORE_RECEIVE_DETAILS fChemStoreReceiveDetails,
            IF_CHEM_STORE_INDENTMASTER fChemStoreIndentmaster,
            IF_CHEM_QC_APPROVE fChemQcApprove,
            IF_CHEM_STORE_PRODUCTINFO fChemStoreProductinfo,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fChemTransection = fChemTransection;
            _fChemStoreIndentmaster = fChemStoreIndentmaster;
            _fChemStoreProductinfo = fChemStoreProductinfo;
            _fChemQcApprove = fChemQcApprove;
            _fCsChemReceiveReport = fCsChemReceiveReport;
            _comImpInvdetails = comImpInvdetails;
            _fChemStoreReceiveMaster = fChemStoreReceiveMaster;
            _fChemStoreReceiveDetails = fChemStoreReceiveDetails;
            _userManager = userManager;
        }

        [HttpGet("Details/{id?}")]
        public IActionResult DetailsFChemStoreReceive(string id)
        {
            return View();
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetChemicalReceiveMaster()
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

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateChemicalReceive()
        {
            return View(await _fChemStoreReceiveMaster.GetInitObjsByAsync(new FChemStoreReceiveViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateChemicalReceive(FChemStoreReceiveViewModel fChemStoreReceiveViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fChemStoreReceiveViewModel.FChemStoreReceiveMaster.CREATED_BY = fChemStoreReceiveViewModel.FChemStoreReceiveMaster.UPDATED_BY = user.Id;
                fChemStoreReceiveViewModel.FChemStoreReceiveMaster.CREATED_AT = fChemStoreReceiveViewModel.FChemStoreReceiveMaster.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;
                var fChemStoreReceiveMaster = await _fChemStoreReceiveMaster.GetInsertedObjByAsync(fChemStoreReceiveViewModel.FChemStoreReceiveMaster);

                if (fChemStoreReceiveMaster.CHEMRCVID > 0)
                {
                    foreach (var item in fChemStoreReceiveViewModel.FChemStoreReceiveDetailsList)
                    {
                        item.CHEMRCVID = fChemStoreReceiveMaster.CHEMRCVID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        var fChemStoreReceiveDetails = await _fChemStoreReceiveDetails.GetInsertedObjByAsync(item);
                        var lastBalance = await _fChemTransection.GetLastBalanceByProductIdAsync(item.PRODUCTID, fChemStoreReceiveDetails.TRNSID);

                        var fChemTransection = new F_CHEM_TRANSECTION
                        {
                            CTRDATE = fChemStoreReceiveViewModel.FChemStoreReceiveMaster.RCVDATE,
                            PRODUCTID = item.PRODUCTID,
                            CRCVID = fChemStoreReceiveDetails.TRNSID,
                            RCVTID = fChemStoreReceiveViewModel.FChemStoreReceiveMaster.RCVTID,
                            RCV_QTY = item.FRESH_QTY,
                            REMARKS = item.REMARKS,
                            BALANCE = lastBalance + item.FRESH_QTY,
                            OP_BALANCE = lastBalance
                            //BALANCE = lastBalance + item.FRESH_QTY - item.REJ_QTY,
                        };

                        if (await _fChemTransection.InsertByAsync(fChemTransection))
                        {
                            atLeastOneInsert = true;
                        }
                        else
                        {
                            await _fChemStoreReceiveDetails.Delete(fChemStoreReceiveDetails);

                            TempData["message"] = "Failed to Add Chemical Receive Information";
                            TempData["type"] = "error";
                            return View(await _fChemStoreReceiveMaster.GetInitObjsByAsync(fChemStoreReceiveViewModel));
                        }
                    }

                    if (!atLeastOneInsert)
                    {
                        await _fChemStoreReceiveMaster.Delete(fChemStoreReceiveMaster);
                    }

                    TempData["message"] = "Successfully added Chemical Receive Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetChemicalReceiveMaster), $"FChemStoreReceive");
                }
                else
                {
                    TempData["message"] = "Failed to Add Chemical Receive Information";
                    TempData["type"] = "error";
                    return View(await _fChemStoreReceiveMaster.GetInitObjsByAsync(fChemStoreReceiveViewModel));
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditChemicalReceive(FChemStoreReceiveViewModel fChemStoreReceiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var fChemStoreReceiveMaster = await _fChemStoreReceiveMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fChemStoreReceiveViewModel.FChemStoreReceiveMaster.EncryptedId)));

                if (fChemStoreReceiveMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fChemStoreReceiveViewModel.FChemStoreReceiveMaster.CHEMRCVID = fChemStoreReceiveMaster.CHEMRCVID;
                    fChemStoreReceiveViewModel.FChemStoreReceiveMaster.CREATED_AT = fChemStoreReceiveMaster.CREATED_AT;
                    fChemStoreReceiveViewModel.FChemStoreReceiveMaster.CREATED_BY = fChemStoreReceiveMaster.CREATED_BY;
                    fChemStoreReceiveViewModel.FChemStoreReceiveMaster.UPDATED_AT = DateTime.Now;
                    fChemStoreReceiveViewModel.FChemStoreReceiveMaster.UPDATED_BY = user.Id;

                    if (await _fChemStoreReceiveMaster.Update(fChemStoreReceiveViewModel.FChemStoreReceiveMaster))
                    {
                        var fChemStoreReceiveDetailses = fChemStoreReceiveViewModel.FChemStoreReceiveDetailsList.Where(e => e.TRNSID <= 0);

                        foreach (var item in fChemStoreReceiveDetailses)
                        {
                            item.CHEMRCVID = fChemStoreReceiveMaster.CHEMRCVID;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;

                            var fChemStoreReceiveDetails = await _fChemStoreReceiveDetails.GetInsertedObjByAsync(item);
                            var lastBalance = await _fChemTransection.GetLastBalanceByProductIdAsync(item.PRODUCTID, fChemStoreReceiveDetails.TRNSID);

                            var fChemTransection = new F_CHEM_TRANSECTION
                            {
                                CTRDATE = fChemStoreReceiveViewModel.FChemStoreReceiveMaster.RCVDATE,
                                PRODUCTID = item.PRODUCTID,
                                CRCVID = fChemStoreReceiveDetails.TRNSID,
                                RCVTID = fChemStoreReceiveViewModel.FChemStoreReceiveMaster.RCVTID,
                                RCV_QTY = item.FRESH_QTY,
                                REMARKS = item.REMARKS,
                                BALANCE = lastBalance + item.FRESH_QTY,
                                OP_BALANCE = lastBalance
                            };

                            await _fChemTransection.InsertByAsync(fChemTransection);
                        }

                        TempData["message"] = "Successfully Updated Chemical Receive Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetChemicalReceiveMaster), $"FChemStoreReceive");
                    }
                    else
                    {
                        TempData["message"] = "Failed To Update Chemical Receive Information.";
                        TempData["type"] = "error";
                    }
                }
                else
                {
                    TempData["message"] = "Failed To Update Chemical Receive Information.";
                    TempData["type"] = "error";
                }
            }
            else
            {
                TempData["message"] = "Failed To Update Chemical Receive Information.";
                TempData["type"] = "error";
            }

            return View(nameof(EditChemicalReceive), await _fChemStoreReceiveMaster.GetInitObjsByAsync(fChemStoreReceiveViewModel));
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditChemicalReceive(string id)
        {
            var fChemStoreReceiveViewModel = await _fChemStoreReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id)));

            if (fChemStoreReceiveViewModel.FChemStoreReceiveMaster != null)
            {
                return View(await _fChemStoreReceiveMaster.GetInitObjsByAsync(fChemStoreReceiveViewModel));
            }

            TempData["message"] = "Failed to Retrieve Chemical Receive Details.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetChemicalReceiveMaster), $"FChemStoreReceive");
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

                var forDataTableByAsync = await _fChemStoreReceiveMaster.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                return Json(new
                {
                    draw = forDataTableByAsync.Draw,
                    recordsFiltered = forDataTableByAsync.RecordsTotal,
                    recordsTotal = forDataTableByAsync.RecordsTotal,
                    data = forDataTableByAsync.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult GetPreviousChemReceiveDetailsList(int id)
        {
            try
            {
                var detailsList = _fChemStoreReceiveDetails.FindAllChemByReceiveIdAsync(id);
                FChemStoreReceiveViewModel fChemStoreReceiveViewModel = new FChemStoreReceiveViewModel
                {
                    FChemStoreReceiveDetailsList = detailsList.ToList()
                };
                return PartialView("GetPreviousChemReceiveDetailsList", fChemStoreReceiveViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CreateMrr(FChemStoreReceiveViewModel fChemStoreReceiveViewModel)
        {
            try
            {
                fChemStoreReceiveViewModel.FCsChemReceiveReport.CMRRNO = await _fCsChemReceiveReport.GetLastMrrNo();
                return Ok(await _fCsChemReceiveReport.InsertByAsync(fChemStoreReceiveViewModel.FCsChemReceiveReport));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetInvoiceDetails/{id?}")]
        public async Task<IActionResult> GetInvoiceDetails(int id)
        {
            try
            {
                return PartialView($"DetailsComImpInvoiceInfoTable", await _comImpInvdetails.GetSingleInvoiceDetails(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetIndentMaster/{id?}")]
        public async Task<IActionResult> GetIndentMaster(int id)
        {
            try
            {
                return Ok(await _fChemStoreIndentmaster.GetIndentByCindid(id));
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
                return Ok(await _fChemStoreProductinfo.GetSingleProductDetailsByPID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> QcApprove(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                return Ok(await _fChemQcApprove.InsertByAsync(new F_CHEM_QC_APPROVE
                {
                    CRDID = id,
                    CQCADATE = DateTime.Now,
                    APPROVED_BY = user.Id
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddOrRemoveFromReceiveDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToList(FChemStoreReceiveViewModel fChemStoreReceiveViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fChemStoreReceiveViewModel.IsDelete)
                {
                    var fChemStoreReceiveDetails = fChemStoreReceiveViewModel.FChemStoreReceiveDetailsList[fChemStoreReceiveViewModel.RemoveIndex];

                    if (fChemStoreReceiveDetails.TRNSID > 0)
                    {
                        await _fChemStoreReceiveDetails.Delete(fChemStoreReceiveDetails);
                    }

                    fChemStoreReceiveViewModel.FChemStoreReceiveDetailsList.RemoveAt(fChemStoreReceiveViewModel.RemoveIndex);
                }
                else
                {
                    if (!fChemStoreReceiveViewModel.FChemStoreReceiveDetailsList.Any(e => e.PRODUCTID.Equals(fChemStoreReceiveViewModel.FChemStoreReceiveDetails.PRODUCTID) && e.BATCHNO.Equals(fChemStoreReceiveViewModel.FChemStoreReceiveDetails.BATCHNO)))
                    {
                        fChemStoreReceiveViewModel.FChemStoreReceiveDetailsList.Add(fChemStoreReceiveViewModel.FChemStoreReceiveDetails);
                    }
                }

                return PartialView($"ChemicalReceiveDetailsPartialView", await _fChemStoreReceiveDetails.GetInitObjForDetails(fChemStoreReceiveViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}