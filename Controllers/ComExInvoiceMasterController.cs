using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DenimERP.ViewModels.Com.InvoiceExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class ComExInvoiceMasterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICOM_EX_INVOICEMASTER _comExInvoicemaster;
        private readonly ICOM_EX_FABSTYLE _comExFabstyle;
        private readonly ICOM_EX_INVDETAILS _comExInvdetails;
        private readonly ICOM_EX_LCINFO _comExLcinfo;
        private readonly IProcessUploadFile _processUploadFile;
        private readonly IDataProtector _protector;
        private readonly string title = "Commercial Export Invoice";

        public ComExInvoiceMasterController(UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_EX_INVOICEMASTER comExInvoicemaster,
            ICOM_EX_FABSTYLE comExFabstyle,
            ICOM_EX_INVDETAILS comExInvdetails,
            ICOM_EX_LCINFO comExLcinfo,
            IProcessUploadFile processUploadFile)
        {
            _userManager = userManager;
            _comExInvoicemaster = comExInvoicemaster;
            _comExFabstyle = comExFabstyle;
            _comExInvdetails = comExInvdetails;
            _comExLcinfo = comExLcinfo;
            _processUploadFile = processUploadFile;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExportInvoice/GetFabricStyles/{lcId?}")]
        public async Task<IActionResult> GetFabricStyles(string lcId)
        {
            return Ok(await _comExInvoicemaster.GetFabricStylesByAsync(int.Parse(lcId)));
        }

        [HttpPost]
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
                var data = await _comExInvoicemaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.LC != null && (!string.IsNullOrEmpty(m.INVNO) && m.INVNO.ToUpper().Contains(searchValue)
                                                            || m.INVDATE != null && m.INVDATE.ToShortDateString().ToUpper().Contains(searchValue)
                                                            || m.LC != null && !string.IsNullOrEmpty(m.LC.LCNO) && m.LC.LCNO.ToUpper().Contains(searchValue)
                                                            || m.LC != null && !string.IsNullOrEmpty(m.LC.FILENO) && m.LC.FILENO.ToUpper().Contains(searchValue)
                                                            || m.INV_QTY != null && m.INV_QTY.ToString().ToUpper().Contains(searchValue)
                                                            || m.INV_AMOUNT != null && m.INV_AMOUNT.ToString().ToUpper().Contains(searchValue)
                                                            || m.BANK_REF != null && m.BANK_REF.ToUpper().Contains(searchValue)
                                                            || m.BUYER != null && !string.IsNullOrEmpty(m.BUYER.BUYER_NAME) && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                                            || m.LC.COM_EX_LCDETAILS.FirstOrDefault()?.PI?.BRAND?.BRANDNAME != null && m.LC.COM_EX_LCDETAILS.FirstOrDefault()!.PI.BRAND.BRANDNAME.ToUpper().Contains(searchValue)
                                                            || !string.IsNullOrEmpty(m.STATUS) && m.STATUS.ToUpper().Contains(searchValue)));
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize);

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
        [Route("CommercialExportInvoice/Details/{invId?}")]
        public async Task<IActionResult> DetailsComExInvoiceMaster(string invId)
        {
            var createComExInvoiceMasterViewModel = await _comExInvoicemaster.FindByInvIdIncludeAllAsync(int.Parse(_protector.Unprotect(invId)));

            if (createComExInvoiceMasterViewModel.ComExInvoicemaster != null)
            {
                return View(createComExInvoiceMasterViewModel);
            }

            return RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
        }

        [HttpGet]
        public async Task<IActionResult> GetGetComExInvoiceMasterBySearch(string searchBy)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var comExInvoiceMasterSearchBy = await _comExInvoicemaster.GetComExInvoiceMasterBy(searchBy);
                    return PartialView($"GetComExInvoiceMasterTable", comExInvoiceMasterSearchBy);
                }

                return Json(new EmptyResult());
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Create")]
        public async Task<IActionResult> CreateComExInvoiceMaster()
        {
            var comExInvoiceMasterCreateViewModel = await _comExInvoicemaster.GetInitObjByAsync(new ComExInvoiceMasterCreateViewModel());

            comExInvoiceMasterCreateViewModel.ComExInvoicemaster = new COM_EX_INVOICEMASTER
            {
                ISACTIVE = true,
                STATUS = "New Invoice",
                INVDATE = DateTime.Now,
                INVNO = await _comExInvoicemaster.GetLastInvoiceNoAsync()
            };

            return View(comExInvoiceMasterCreateViewModel);
        }

        [HttpPost]
        [Route("CommercialExportInvoice/PostCreate")]
        public async Task<IActionResult> PostCreateComExInvoiceMaster(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var findByInvNoAsync = await _comExInvoicemaster.FindByInvNoAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVNO);

                    if (findByInvNoAsync)
                    {
                        TempData["message"] = "Failed To Insert Export Invoice.";
                        TempData["type"] = "error";
                        return View(nameof(CreateComExInvoiceMaster), await _comExInvoicemaster.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));
                    }

                    var user = await _userManager.GetUserAsync(User);

                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INV_QTY = (double?)comExInvoiceMasterCreateViewModel.ComExInvdetailses.Sum(e => e.QTY);
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INV_AMOUNT = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Sum(e => e.AMOUNT);
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.STATUS = "Prepared";
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.USRID = user.Id;

                    var comExInvoicemaster = await _comExInvoicemaster.GetInsertedObjByAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster);

                    if (comExInvoicemaster.INVID > 0)
                    {
                        foreach (var item in comExInvoiceMasterCreateViewModel.ComExInvdetailses)
                        {
                            item.INVNO = comExInvoicemaster.INVNO;
                            item.INVID = comExInvoicemaster.INVID;
                            item.USRID = user.Id;
                        }

                        await _comExInvdetails.InsertRangeByAsync(comExInvoiceMasterCreateViewModel.ComExInvdetailses);
                    }

                    TempData["message"] = "Successfully Inserted Export Invoice.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(InvokeUserChoices), $"ComExInvoiceMaster", new { @invId = _protector.Protect(comExInvoicemaster.INVID.ToString()) });
                }

                TempData["message"] = "Failed To Insert Export Invoice.";
                TempData["type"] = "error";
                return View(nameof(CreateComExInvoiceMaster), await _comExInvoicemaster.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Edit/{invId?}")]
        public async Task<IActionResult> EditComExInvoiceMaster(string invId)
        {
            try
            {
                var comExInvoiceMasterCreateViewModel = await _comExInvoicemaster.FindByinvIdIncludeAllAsync(int.Parse(_protector.Unprotect(invId)));

                if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster != null)
                {

                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.EncryptedId = _protector.Protect(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVID.ToString());

                    comExInvoiceMasterCreateViewModel.PENDINGDIFFERENCEDAYS = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_RCV_DATE.HasValue && comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_SUB_DATE.HasValue ? (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_RCV_DATE - comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_SUB_DATE).Value.TotalDays : 0;

                    #region Obsolete

                    //if (double.TryParse(comExInvoicemaster.LC.COM_TENOR!=null? comExInvoicemaster.LC.COM_TENOR.NAME : comExInvoicemaster.LC.TENOR, out var result))
                    //{
                    //    comExInvoiceMasterCreateViewModel.BADEFFEREDDAYS = result.ToString(CultureInfo.InvariantCulture);
                    //    comExInvoiceMasterCreateViewModel.BADIFFERENCE = result -
                    //                                                     (comExInvoicemaster.MATUDATE.HasValue &&
                    //                                                      comExInvoicemaster.BNK_ACC_DATE.HasValue
                    //                                                         ? (comExInvoicemaster.MATUDATE -
                    //                                                             comExInvoicemaster.BNK_ACC_DATE).Value
                    //                                                         .TotalDays
                    //                                                         : 0);
                    //}
                    //else
                    //{
                    //    comExInvoiceMasterCreateViewModel.BADEFFEREDDAYS = comExInvoicemaster.LC.COM_TENOR.NAME;
                    //}

                    #endregion

                    return View(await _comExInvoicemaster.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));
                }

                TempData["message"] = "Failed to Load Export Invoice.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
            }
        }

        [HttpPost]
        [Route("CommercialExportInvoice/PostEdit")]
        public async Task<IActionResult> PostEditComExInvoiceMaster(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel)
        {
            try
            {
                var redirectToActionResult = RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
                var user = await _userManager.GetUserAsync(User);
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.EncryptedId)));

                comExInvoicemaster.INV_QTY = (double?)comExInvoiceMasterCreateViewModel.ComExInvdetailses.Sum(e => e.QTY);
                comExInvoicemaster.INV_AMOUNT = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Sum(e => e.AMOUNT);
                comExInvoicemaster.USRID = user.Id;
                comExInvoicemaster.INVREF = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVREF;
                comExInvoicemaster.INVDURATION = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVDURATION;
                comExInvoicemaster.DOC_NOTES = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_NOTES;
                comExInvoicemaster.NEGODATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.NEGODATE;
                comExInvoicemaster.TRUCKNO = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.TRUCKNO;
                comExInvoicemaster.TOTAL_CBM = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.TOTAL_CBM;
                comExInvoicemaster.ISACTIVE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.ISACTIVE;
                comExInvoicemaster.DELDATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DELDATE;
                comExInvoicemaster.DOC_SUB_DATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_SUB_DATE;
                comExInvoicemaster.DOC_RCV_DATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_RCV_DATE;
                comExInvoicemaster.BNK_SUB_DATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BNK_SUB_DATE;
                comExInvoicemaster.BANK_REF = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BANK_REF;
                comExInvoicemaster.BILL_DATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BILL_DATE;
                comExInvoicemaster.DISCREPANCY = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DISCREPANCY;
                comExInvoicemaster.BNK_ACC_DATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BNK_ACC_DATE;
                comExInvoicemaster.MATUDATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.MATUDATE;
                comExInvoicemaster.BNK_ACC_POSTING = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BNK_ACC_POSTING;
                comExInvoicemaster.EXDATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.EXDATE;
                comExInvoicemaster.ODAMOUNT = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.ODAMOUNT;
                comExInvoicemaster.ODRCVDATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.ODRCVDATE;
                comExInvoicemaster.PRCAMOUNT = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.PRCAMOUNT;
                comExInvoicemaster.PRCDATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.PRCDATE;
                comExInvoicemaster.PRCAMNTTK = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.PRCAMNTTK;
                comExInvoicemaster.AMOUNT_EURO = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.AMOUNT_EURO;
                comExInvoicemaster.AMOUNT_BDT = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.AMOUNT_BDT;
                comExInvoicemaster.DOC_VALUE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.DOC_VALUE;
                comExInvoicemaster.IS_FINAL = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.IS_FINAL;
                comExInvoicemaster.PDOCNO = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.PDOCNO;
                comExInvoicemaster.INVDATE = comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVDATE;

                if (comExInvoiceMasterCreateViewModel.BANKREFPATH != null)
                {
                    comExInvoicemaster.BANKREFPATH =
                        _processUploadFile.ProcessUploadFileToContentRootPath(comExInvoiceMasterCreateViewModel.BANKREFPATH, "com_ex_invoice_files");
                }

                if (comExInvoiceMasterCreateViewModel.BANKACCEPTPATH != null)
                {
                    comExInvoicemaster.BANKACCEPTPATH =
                        _processUploadFile.ProcessUploadFileToContentRootPath(comExInvoiceMasterCreateViewModel.BANKACCEPTPATH, "com_ex_invoice_files");
                }

                if (comExInvoiceMasterCreateViewModel.DISCREPANCYPATH != null)
                {
                    comExInvoicemaster.DISCREPANCYPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comExInvoiceMasterCreateViewModel.DISCREPANCYPATH, "com_ex_invoice_files");
                }

                if (comExInvoiceMasterCreateViewModel.PAYMENTPATH != null)
                {
                    comExInvoicemaster.PAYMENTPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comExInvoiceMasterCreateViewModel.PAYMENTPATH, "com_ex_invoice_files");
                }

                if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BANK_REF != null &&
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.PRCDATE != null)
                {
                    comExInvoicemaster.STATUS = "Realized";
                }
                else if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.BANK_REF != null &&
                         comExInvoiceMasterCreateViewModel.ComExInvoicemaster.PRCDATE == null)
                {
                    comExInvoicemaster.STATUS = "Submitted";
                }
                else
                {
                    comExInvoicemaster.STATUS = "Prepared";
                }

                if (await _comExInvoicemaster.Update(comExInvoicemaster))
                {
                    //var comExInvdetailsesNew = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Where(d => d.TRNSID <= 0).ToList();
                    //var comExInvdetailsesOld = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Where(d => d.TRNSID > 0).ToList();
                    bool insert = false, update = false;
                    foreach (var item in comExInvoiceMasterCreateViewModel.ComExInvdetailses.Where(d => d.TRNSID <= 0))
                    {
                        item.INVID = comExInvoicemaster.INVID;
                        item.INVNO = comExInvoicemaster.INVNO;
                        item.USRID = user.Id;
                        if (await _comExInvdetails.InsertByAsync(item))
                            insert = true;
                    }

                    foreach (var item in comExInvoiceMasterCreateViewModel.ComExInvdetailses.Where(d => d.TRNSID > 0))
                    {
                        var comExInvdetails = await _comExInvdetails.FindByIdAsync(item.TRNSID);
                        comExInvdetails.STYLEID = item.STYLEID;
                        comExInvdetails.PIIDD_TRNSID = item.PIIDD_TRNSID;
                        comExInvdetails.AMOUNT = item.AMOUNT;
                        comExInvdetails.RATE = item.RATE;
                        comExInvdetails.QTY = item.QTY;
                        comExInvdetails.ROLL = item.ROLL;
                        comExInvdetails.REMARKS = item.REMARKS;
                        comExInvdetails.USRID = user.Id;
                        if (await _comExInvdetails.Update(comExInvdetails))
                            update = true;
                    }

                    if (insert || update)
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }

                }

                TempData["message"] = $"Failed to Update {title}";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [AcceptVerbs("Post")]
        [Route("CommercialExportInvoice/Delete/{invId?}")]
        public async Task<IActionResult> DeleteComExInvoiceMaster(string invId)
        {
            var comExInvoicemaster = await _comExInvoicemaster.FindByIdForDeleteAsync(int.Parse(_protector.Unprotect(invId)));

            if (comExInvoicemaster != null)
            {
                if (!comExInvoicemaster.ACC_EXPORT_REALIZATION.Any())
                {
                    if (comExInvoicemaster.ComExInvdetailses.Any())
                    {
                        await _comExInvdetails.DeleteRange(comExInvoicemaster.ComExInvdetailses);
                    }

                    await _comExInvoicemaster.Delete(comExInvoicemaster);

                    TempData["message"] = "Successfully Deleted Export Invoice.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "This Invoice Used In A/C Export Realization. Delete Not Possible.";
                    TempData["type"] = "error";
                }

                return RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
            }

            TempData["message"] = "Failed To Delete Export Invoice.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsInvNoInUse(COM_EX_INVOICEMASTER comExInvoicemaster)
        {
            try
            {
                var findByInvNoAsync = await _comExInvoicemaster.FindByInvNoAsync(comExInvoicemaster.INVNO);
                return findByInvNoAsync ? Json($"Invoice Number {comExInvoicemaster.INVNO} already in use.") : Json(true);
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        [HttpGet]
        [Route("CommercialExportInvoice")]
        [Route("CommercialExportInvoice/GetAll")]
        public IActionResult GetComExInvoiceMaster()
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
        public async Task<IActionResult> GetBuyerAndPDocNo(int lcId)
        {
            try
            {
                var result = await _comExInvoicemaster.GetBuyerAndPDocNumber(lcId);
                return Json(result);
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        [HttpGet]
        public IActionResult GetFormatForInvNo(string lastPart)
        {
            try
            {
                return !string.IsNullOrEmpty(lastPart) ? Json(lastPart.StartsWith($"PDL/{DateTime.Now.Year % 100}/") ? lastPart : $"PDL/{DateTime.Now.Year % 100}/{lastPart}") : Json(new EmptyResult());
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveExportInvoiceDetails(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel)
        {
            try
            {
                ModelState.Clear();
                if (comExInvoiceMasterCreateViewModel.RemoveIndex >= 0)
                {
                    var comExInvdetails = comExInvoiceMasterCreateViewModel.ComExInvdetailses[comExInvoiceMasterCreateViewModel.RemoveIndex];

                    if (comExInvdetails.TRNSID > 0)
                    {
                        await _comExInvdetails.Delete(comExInvdetails);
                    }

                    comExInvoiceMasterCreateViewModel.ComExInvdetailses.RemoveAt(comExInvoiceMasterCreateViewModel.RemoveIndex);
                    return PartialView($"AddComExpInvDetailsTable", await _comExInvdetails.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));
                }

                if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID > 0)
                {
                    var findByIdIncludeAllAsync = await _comExLcinfo.FindByIdIncludeAllAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID ?? 0);
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC = findByIdIncludeAllAsync;
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC.COM_EX_LCDETAILS = findByIdIncludeAllAsync.COM_EX_LCDETAILS.ToList();
                }

                return PartialView($"AddComExpInvDetailsTable", await _comExInvdetails.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComExpInvDetails(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel)
        {
            try
            {
                ModelState.Clear();
                if (comExInvoiceMasterCreateViewModel.IsDelete)
                {
                    var cOmExInvDetaiL = comExInvoiceMasterCreateViewModel.ComExInvdetailses[comExInvoiceMasterCreateViewModel.RemoveIndex];

                    if (cOmExInvDetaiL.TRNSID > 0)
                    {
                        await _comExInvdetails.Delete(cOmExInvDetaiL);
                    }

                    comExInvoiceMasterCreateViewModel.ComExInvdetailses.RemoveAt(comExInvoiceMasterCreateViewModel.RemoveIndex);
                }
                else
                {
                    comExInvoiceMasterCreateViewModel.ComExInvdetails.STYLEID = await _comExInvdetails.FindByTrnsIdAsync(comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID ?? 0);
                    var findByInvNoStyleIdAsync = await _comExInvdetails.FindByInvNoStyleIdAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVNO, comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID ?? 0);
                    var any = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Any(e => e.PIIDD_TRNSID.Equals(comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID));

                    if (comExInvoiceMasterCreateViewModel.ComExInvdetailses.Any(e => e.TRNSID > 0 && e.TRNSID.Equals(comExInvoiceMasterCreateViewModel.ComExInvdetails.TRNSID)))
                    {
                        var comExInvdetailses = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Where(e => e.TRNSID.Equals(comExInvoiceMasterCreateViewModel.ComExInvdetails.TRNSID)).ToList();


                        for (var i = 0; i < comExInvdetailses.Count; i++)
                        {
                            if (TryValidateModel(comExInvoiceMasterCreateViewModel.ComExInvdetails))
                            {

                                comExInvdetailses[i].TRNSID = comExInvoiceMasterCreateViewModel.ComExInvdetails.TRNSID;
                                comExInvdetailses[i].STYLEID = comExInvoiceMasterCreateViewModel.ComExInvdetails.STYLEID;
                                comExInvdetailses[i].PIIDD_TRNSID = comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID;
                                comExInvdetailses[i].QTY = comExInvoiceMasterCreateViewModel.ComExInvdetails.QTY;
                                comExInvdetailses[i].RATE = comExInvoiceMasterCreateViewModel.ComExInvdetails.RATE;
                                comExInvdetailses[i].AMOUNT = comExInvoiceMasterCreateViewModel.ComExInvdetails.AMOUNT;
                                comExInvdetailses[i].ROLL = comExInvoiceMasterCreateViewModel.ComExInvdetails.ROLL;
                                comExInvdetailses[i].REMARKS = comExInvoiceMasterCreateViewModel.ComExInvdetails.REMARKS;
                            }
                            else
                            {
                                ModelState.AddModelError("", $"There is an error with your data. Please check the row number {i + 1}");
                            }
                        }
                    }
                    else
                    {
                        if (!findByInvNoStyleIdAsync && !any && await _comExInvoicemaster.HasBalanceByAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID ?? 0, comExInvoiceMasterCreateViewModel.ComExInvdetails.AMOUNT ?? 0))
                        {
                            comExInvoiceMasterCreateViewModel.ComExInvdetailses.Add(comExInvoiceMasterCreateViewModel.ComExInvdetails);
                        }
                    }
                }

                if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID > 0)
                {
                    var findByIdIncludeAllAsync = await _comExLcinfo.FindByIdIncludeAllAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID ?? 0);
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC = findByIdIncludeAllAsync;
                    comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC.COM_EX_LCDETAILS = findByIdIncludeAllAsync.COM_EX_LCDETAILS.ToList();
                }

                return PartialView($"AddComExpInvDetailsTable", await _comExInvdetails.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));

            }
            catch (Exception)
            {
                return BadRequest();
            }

            #region Obsolete

            //try
            //{
            //    ModelState.Clear();
            //    if (comExInvoiceMasterCreateViewModel.IsDelete)
            //    {
            //        var comExInvdetails = comExInvoiceMasterCreateViewModel.ComExInvdetailses[comExInvoiceMasterCreateViewModel.RemoveIndex];

            //        if (comExInvdetails.TRNSID > 0)
            //        {
            //            await _comExInvdetails.Delete(comExInvdetails);
            //        }

            //        comExInvoiceMasterCreateViewModel.ComExInvdetailses.RemoveAt(comExInvoiceMasterCreateViewModel.RemoveIndex);
            //    }
            //    else
            //    {
            //        comExInvoiceMasterCreateViewModel.ComExInvdetails.STYLEID = await _comExInvdetails.FindByTrnsIdAsync(comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID ?? 0);
            //        var findByInvNoStyleIdAsync = await _comExInvdetails.FindByInvNoStyleIdAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.INVNO, comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID ?? 0);
            //        var any = comExInvoiceMasterCreateViewModel.ComExInvdetailses.Any(e => e.PIIDD_TRNSID.Equals(comExInvoiceMasterCreateViewModel.ComExInvdetails.PIIDD_TRNSID));

            //        if (!findByInvNoStyleIdAsync && !any && await _comExInvoicemaster.HasBalanceByAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID ?? 0, comExInvoiceMasterCreateViewModel.ComExInvdetails.AMOUNT ?? 0))
            //        {
            //            comExInvoiceMasterCreateViewModel.ComExInvdetailses.Add(comExInvoiceMasterCreateViewModel.ComExInvdetails);
            //        }
            //    }

            //    if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID > 0)
            //    {
            //        var findByIdIncludeAllAsync = await _comExLcinfo.FindByIdIncludeAllAsync(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID ?? 0);
            //        comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC = findByIdIncludeAllAsync;
            //        comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC.COM_EX_LCDETAILS = findByIdIncludeAllAsync.COM_EX_LCDETAILS.ToList();
            //    }

            //    return PartialView($"AddComExpInvDetailsTable", await _comExInvdetails.GetInitObjByAsync(comExInvoiceMasterCreateViewModel));
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}

            #endregion
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExport/Invoice/ShowOptions/{invId?}")]
        public async Task<IActionResult> InvokeUserChoices(string invId)
        {
            try
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                comExInvoicemaster.EncryptedId = _protector.Protect(comExInvoicemaster.INVID.ToString());
                return View(comExInvoicemaster);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(GetComExInvoiceMaster), $"ComExInvoiceMaster");
            }
        }

        [HttpGet]
        public IActionResult RSubmissionOfShipping()
        {
            return View();
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Invoice/Weight-Measurement/{invId?}")]
        public async Task<IActionResult> RWeightMeasurement(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Invoice/Pre-Shipment-Inspection-Beni/{invId?}")]
        public async Task<IActionResult> RPreShipmentInspectionBeni(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Forwarding/{invId?}")]
        public async Task<IActionResult> RForwarding(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Certificate-of-Origin/{invId?}")]
        public async Task<IActionResult> RCertificateOfOrigin(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Beneficiary-Certificate/{invId?}")]
        public async Task<IActionResult> RBeneficiaryCertificate(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/GetBillOfExe-2/{invId?}")]
        public async Task<IActionResult> RBillOfExchange2(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/GetBillOfExe-1/{invId?}")]
        public async Task<IActionResult> RBillOfExchange1(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/GetInvoice/{invId?}")]
        public async Task<IActionResult> RComExInvoice(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Foreign/Packing-List/{invId?}")]
        public async Task<IActionResult> RForeignComExInvoiceMasterPackingList(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        public IActionResult RForeignComExInvoice()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RComExInvoiceMasterInvoice()
        {
            return View();
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Truck-Challan/{invId?}")]
        public async Task<IActionResult> RComExInvoiceMasterTruckReceipt(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Delivery-Challan/{invId?}")]
        public async Task<IActionResult> RComExInvoiceMasterDeliveryChallan(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [HttpGet]
        [Route("CommercialExportInvoice/Local/Packing-List/{invId?}")]
        public async Task<IActionResult> RComExInvoiceMasterPackingList(string invId)
        {
            var invNo = string.Empty;

            if (!string.IsNullOrEmpty(invId))
            {
                var comExInvoicemaster = await _comExInvoicemaster.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));
                invNo = comExInvoicemaster.INVNO;
            }

            return View(model: invNo);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetInvoiceList(int lcId)
        {
            try
            {
                return PartialView($"DetailsOfPreviousInvoicesAndPITable", await _comExLcinfo.GetLcDetailsByIdAsync(lcId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExportInvoice/GetInvBalance/{trnsId?}/{lcId?}")]
        public async Task<IActionResult> GetInvBalance(int trnsId, int lcId)
        {
            return Ok(await _comExFabstyle.GetGetInvBalanceAsync(trnsId, lcId));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExportInvoice/GetStyleInfo/{trnsId?}/{lcId?}")]
        public async Task<IActionResult> GetStyleInfo(int trnsId, int lcId)
        {
            return Ok(await _comExFabstyle.GetComExFabricInfoAsync(trnsId, lcId));
        }

        [HttpGet]
        public async Task<IActionResult> GetBaDifference(BaDifference baDifference)
        {
            try
            {
                var comExInvoiceMaster = await _comExInvoicemaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(baDifference.InvId)));

                if (comExInvoiceMaster == null) return Json(new EmptyResult());
                var byIdIncludeAllAsync = await _comExLcinfo.FindByIdIncludeAllAsync(comExInvoiceMaster.LCID ?? 0);
                if (byIdIncludeAllAsync == null) return Json(new EmptyResult());
                if (byIdIncludeAllAsync.TENOR == null)
                {
                    if (double.TryParse(byIdIncludeAllAsync.COM_TENOR.NAME, out var result))
                    {
                        baDifference._BaDifference = result - (baDifference.MatuDate.HasValue && baDifference.BnkAccDate.HasValue ? (baDifference.MatuDate - baDifference.BnkAccDate).Value.TotalDays : 0);
                        return Json(baDifference);
                    }

                    baDifference._BaDifference = 0;
                    return Json(baDifference);
                }

                baDifference._BaDifference = 0;
                return Json(baDifference);

                //return Json(new EmptyResult());
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        [HttpGet]
        public async Task<COM_EX_INVDETAILS> GetSingleInvDetails(int trnsId)
        {
            try
            {
                return await _comExInvdetails.GetSingleInvDetails(trnsId);
            }
            catch (Exception)
            {
                return null;
            }
        }



        [Route("CommercialExportInvoice/GetInvoicAmount")]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetInvoicAmount(string id)
        {
            try
            {
                return Ok(await _comExInvdetails.GetInvoicAmountByLcno(id));

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }




    }
}