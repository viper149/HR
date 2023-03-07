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
    [Route("GeneralStoreReceive")]
    public class FGenSReceiveController : Controller
    {
        private readonly IF_GEN_S_RECEIVE_MASTER _fGenSReceiveMaster;
        private readonly IF_GEN_S_RECEIVE_DETAILS _fGenSReceiveDetails;
        private readonly IF_GEN_S_INDENTMASTER _fGenSIndentmaster;
        private readonly IF_GEN_S_QC_APPROVE _fGenSQcApprove;
        private readonly IF_GEN_S_MRR _fGenSMrr;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly ICOM_IMP_LCDETAILS _comImpLcdetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Receive Information";

        public FGenSReceiveController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            //IF_CS_GEN_S_RECEIVE_REPORT fCsGenSReceiveReport,
            IF_GEN_S_RECEIVE_MASTER fGenSReceiveMaster,
            IF_GEN_S_RECEIVE_DETAILS fGenSReceiveDetails,
            IF_GEN_S_INDENTMASTER fGenSIndentmaster,
            IF_GEN_S_QC_APPROVE fGenSQcApprove,
            IF_GEN_S_MRR fGenSMrr,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            ICOM_IMP_LCDETAILS comImpLcdetails,
            UserManager<ApplicationUser> userManager
            )
        {
            _fGenSReceiveMaster = fGenSReceiveMaster;
            _fGenSReceiveDetails = fGenSReceiveDetails;
            _fGenSIndentmaster = fGenSIndentmaster;
            _fGenSQcApprove = fGenSQcApprove;
            _fGenSMrr = fGenSMrr;
            _fGsProductInformation = fGsProductInformation;
            _comImpLcdetails = comImpLcdetails;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet("Details/{id?}")]
        public async Task<IActionResult> DetailsFGenSReceive(string id)
        {
            return View(await _fGenSReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id))));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFGenSReceive()
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
        public async Task<IActionResult> CreateFGenSReceive()
        {
            return View(await _fGenSReceiveMaster.GetInitObjsByAsync(new FGenSReceiveViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFGenSReceive(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fGenSReceiveViewModel.FGenSReceiveMaster.CREATED_BY = fGenSReceiveViewModel.FGenSReceiveMaster.UPDATED_BY = user.Id;
                fGenSReceiveViewModel.FGenSReceiveMaster.CREATED_AT = fGenSReceiveViewModel.FGenSReceiveMaster.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var fGenSReceiveMaster = await _fGenSReceiveMaster.GetInsertedObjByAsync(fGenSReceiveViewModel.FGenSReceiveMaster);

                if (fGenSReceiveMaster.RCVTID > 0)
                {
                    foreach (var item in fGenSReceiveViewModel.FGenSReceiveDetailsesList)
                    {
                        item.GRCVID = fGenSReceiveMaster.GRCVID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                        var fGenSReceiveDetails = await _fGenSReceiveDetails.GetInsertedObjByAsync(item);
                        atLeastOneInsert = true;
                        var fGenSIndentmaster = await _fGenSIndentmaster.FindByIdAsync(fGenSReceiveDetails.GINDID ?? 0);
                            fGenSIndentmaster.STATUS = true;
                            await _fGenSIndentmaster.Update(fGenSIndentmaster);
                    }

                    if (!atLeastOneInsert)
                    {
                        await _fGenSReceiveMaster.Delete(fGenSReceiveMaster);
                        TempData["message"] = $"Failed to Add {title}";
                        TempData["type"] = "error";
                        return View(await _fGenSReceiveMaster.GetInitObjsByAsync(fGenSReceiveViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFGenSReceive), $"FGenSReceive");
                }

                await _fGenSReceiveMaster.Delete(fGenSReceiveMaster);
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(await _fGenSReceiveMaster.GetInitObjsByAsync(fGenSReceiveViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFGenSReceive(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var fGenSReceiveMaster = await _fGenSReceiveMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fGenSReceiveViewModel.FGenSReceiveMaster.EncryptedId)));

                if (fGenSReceiveMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fGenSReceiveViewModel.FGenSReceiveMaster.GRCVID = fGenSReceiveMaster.GRCVID;
                    fGenSReceiveViewModel.FGenSReceiveMaster.CREATED_AT = fGenSReceiveMaster.CREATED_AT;
                    fGenSReceiveViewModel.FGenSReceiveMaster.CREATED_BY = fGenSReceiveMaster.CREATED_BY;
                    fGenSReceiveViewModel.FGenSReceiveMaster.UPDATED_AT = DateTime.Now;
                    fGenSReceiveViewModel.FGenSReceiveMaster.UPDATED_BY = user.Id;

                    if (await _fGenSReceiveMaster.Update(fGenSReceiveViewModel.FGenSReceiveMaster))
                    {
                        var fGenSReceiveDetailses = fGenSReceiveViewModel.FGenSReceiveDetailsesList.Where(e => e.TRNSID <= 0);

                        foreach (var item in fGenSReceiveDetailses)
                        {
                            item.GRCVID = fGenSReceiveMaster.GRCVID;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                        }

                        if (await _fGenSReceiveDetails.InsertRangeByAsync(fGenSReceiveDetailses))
                        {
                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFGenSReceive), $"FGenSReceive");
                        }
                    }
                }
            }
            TempData["message"] = $"Failed to update {title}";
            TempData["type"] = "error";
            return View(nameof(EditFGenSReceive), await _fGenSReceiveMaster.GetInitObjsByAsync(fGenSReceiveViewModel));
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditFGenSReceive(string id)
        {
            var fGenSReceiveViewModel = await _fGenSReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id)),true);

            if (fGenSReceiveViewModel.FGenSReceiveMaster != null)
            {
                return View(await _fGenSReceiveMaster.GetInitObjsByAsync(fGenSReceiveViewModel));
            }

            TempData["message"] = $"Failed to Retrieve {title} Details.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFGenSReceive), $"FGenSReceive");
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

            var data = await _fGenSReceiveMaster.GetAllFGenSReceiveAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.GRCVID.ToString().ToUpper().Contains(searchValue)
                                        || m.RCVDATE != null && m.RCVDATE.ToString().ToUpper().Contains(searchValue)
                                        || m.RCVT.RCVTYPE != null && m.RCVT.RCVTYPE.ToUpper().Contains(searchValue)
                                        || m.CHALLAN_NO != null && m.CHALLAN_NO.ToUpper().Contains(searchValue)
                                        || m.CHALLAN_DATE != null && m.CHALLAN_DATE.ToString().Contains(searchValue)
                                        || m.VEHICAL_NO != null && m.VEHICAL_NO.ToUpper().Contains(searchValue)).ToList();
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

        //[HttpPost]
        //public IActionResult GetPreviousGenSReceiveDetailsList(int id)
        //{
        //    try
        //    {
        //        var detailsList = _fGenSReceiveDetails.FindAllGenSByReceiveIdAsync(id);
        //        FGenSReceiveViewModel fGenSReceiveViewModel = new FGenSReceiveViewModel
        //        {
        //            FGenSReceiveDetailsesList = detailsList.ToList()
        //        };
        //        return PartialView("GetPreviousGenSReceiveDetailsList", fGenSReceiveViewModel);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> CreateMrr(FGenSReceiveViewModel fGenSReceiveViewModel)
        //{
        //    try
        //    {
        //        fGenSReceiveViewModel.FCsChemReceiveReport.CMRRNO = await _fCsChemReceiveReport.GetLastMrrNo();
        //        return Ok(await _fCsChemReceiveReport.InsertByAsync(fGenSReceiveViewModel.FCsChemReceiveReport));
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet]
        //[Route("GetInvoiceDetails/{id?}")]
        //public async Task<IActionResult> GetInvoiceDetails(int id)
        //{
        //    try
        //    {
        //        return PartialView($"DetailsComImpInvoiceInfoTable", await _comImpInvdetails.GetSingleInvoiceDetails(id));
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        [AcceptVerbs("Get", "Post")]
        [Route("GetIndentMaster/{id?}")]
        public async Task<IActionResult> GetIndentMaster(int id)
        {
            try
            {
                return Ok(await _fGenSIndentmaster.GetIndentByIndId(id));
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

        //[HttpPost]
        //public async Task<IActionResult> QcApprove(int id)
        //{
        //    try
        //    {
        //        var user = await _userManager.GetUserAsync(User);

        //        return Ok(await _fGenSQcApprove.InsertByAsync(new F_GEN_S_QC_APPROVE
        //        {
        //            GRDID = id,
        //            GQCADATE = DateTime.Now,
        //            APPROVED_BY = user.Id
        //        }));
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [Route("AddOrRemoveFromReceiveDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToList(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGenSReceiveViewModel.IsDelete)
                {
                    var fGenSReceiveDetails = fGenSReceiveViewModel.FGenSReceiveDetailsesList[fGenSReceiveViewModel.RemoveIndex];

                    if (fGenSReceiveDetails.TRNSID > 0)
                    {
                        await _fGenSReceiveDetails.Delete(fGenSReceiveDetails);
                    }

                    fGenSReceiveViewModel.FGenSReceiveDetailsesList.RemoveAt(fGenSReceiveViewModel.RemoveIndex);
                }
                else
                {
                    if (!fGenSReceiveViewModel.FGenSReceiveDetailsesList.Any(e => e.PRODUCTID.Equals(fGenSReceiveViewModel.FGenSReceiveDetails.PRODUCTID)))
                    {
                        if (TryValidateModel(fGenSReceiveViewModel.FGenSReceiveDetails))
                        {
                            fGenSReceiveViewModel.FGenSReceiveDetailsesList.Add(fGenSReceiveViewModel.FGenSReceiveDetails);
                        }
                    }
                }

                return PartialView($"FGenSReceiveDetailsPartialView", await _fGenSReceiveDetails.GetInitObjForDetails(fGenSReceiveViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetLC/{id?}")]
        public async Task<IActionResult> GetLC(int id)
        {
            try
            {
                return Ok(await _comImpLcdetails.GetLcNoByTransId(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetIndentByProduct/{productId?}")]
        public async Task<IActionResult> GetIndentListByPId(int productId)
        {
            try
            {
                return Ok(await _fGsProductInformation.GetIndentListByPId(productId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("QcApprove")]
        public async Task<IActionResult> QcApprove(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            try
            {
                //bool exist = false;
                //int? qcNo;

                //if (fGenSReceiveViewModel.FGenSReceiveMaster.F_GEN_S_RECEIVE_DETAILS.Count == 1)
                //{
                //    var fGenSReceiveDetailses = fGenSReceiveViewModel.FGenSReceiveDetailsesList.Where(e => e.TRNSID <= 0);
                //    foreach (var item in fGenSReceiveDetailses)
                //    {
                //        if (item.PRODUCTID == fGenSReceiveViewModel.FGenSReceiveDetails.PRODUCTID && item.GRCV.SUPPID == fGenSReceiveViewModel.FGenSReceiveMaster.SUPPID && item.GRCV.RCVDATE == fGenSReceiveViewModel.FGenSReceiveMaster.RCVDATE)
                //        {
                //            exist = true;
                //            qcNo = fGenSReceiveViewModel.FGenSReceiveMaster.QCPASS;
                //        }
                //    }
                //}

                //if (exist)
                //{
                //    //return qcNo;
                //}

                ModelState.Clear();
                var user = await _userManager.GetUserAsync(User);
                var lastQcNo = await _fGenSQcApprove.GetLastQCANo();
                var userId = user.Id;
                var insertTime = DateTime.Now;

                fGenSReceiveViewModel.FGenSQcApprove = new F_GEN_S_QC_APPROVE
                {
                    CREATED_BY = userId,
                    UPDATED_BY = userId,
                    APPROVED_BY = userId,
                    CREATED_AT = insertTime,
                    UPDATED_AT = insertTime,
                    GQCADATE = insertTime,
                    GSQCANO = lastQcNo + 1
                };

                return Ok(await _fGenSQcApprove.InsertByAsync(fGenSReceiveViewModel.FGenSQcApprove));

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GetQc")]
        public async Task<IActionResult> GetQcNo()
        {
            try
            {
                return Ok(await _fGenSQcApprove.GetQcDetails());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CreateMrr")]
        public async Task<IActionResult> CreateMrr(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            try
            {
                ModelState.Clear();

                var user = await _userManager.GetUserAsync(User);
                var lastMrrNo = await _fGenSMrr.GetLastMrrNo();
                var userId = user.Id;
                var insertTime = DateTime.Now;

                fGenSReceiveViewModel.FGenSMrr = new F_GEN_S_MRR
                {
                    CREATED_BY = userId,
                    UPDATED_BY = userId,
                    GSMRRDATE = insertTime,
                    CREATED_AT = insertTime,
                    UPDATED_AT = insertTime,
                    GSMRRNO = lastMrrNo + 1
                };

                return Ok(await _fGenSMrr.InsertByAsync(fGenSReceiveViewModel.FGenSMrr));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GetMrr")]
        public async Task<IActionResult> GetMrr()
        {
            try
            {
                return Ok(await _fGenSMrr.GetMrrDetails());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
