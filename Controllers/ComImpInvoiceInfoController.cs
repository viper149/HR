using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DenimERP.ViewModels.Com.InvoiceImport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class ComImpInvoiceInfoController : Controller
    {
        private readonly IBAS_PRODUCTINFO _basProductinfo;
        private readonly IProcessUploadFile _processUploadFile;
        private readonly ICOM_IMP_INVOICEINFO _comImpInvoiceinfo;
        private readonly ICOM_IMP_INVDETAILS _comImpInvdetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public ComImpInvoiceInfoController(IBAS_PRODUCTINFO basProductinfo,
            IProcessUploadFile processUploadFile,
            ICOM_IMP_INVOICEINFO comImpInvoiceinfo,
            ICOM_IMP_INVDETAILS comImpInvdetails,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _basProductinfo = basProductinfo;
            _processUploadFile = processUploadFile;
            _comImpInvoiceinfo = comImpInvoiceinfo;
            _comImpInvdetails = comImpInvdetails;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialImport/GetProducts/{search?}/{lcId?}/{page?}")]
        public async Task<IActionResult> GetProducts(string search, int lcId, int page)
        {
            return Ok(await _comImpInvdetails.GetProductsByAsync(search, lcId, page));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetProductInfo(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel)
        {
            return Ok(await _comImpInvoiceinfo.GetProductInfoByAsync(comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.LC_ID ?? 0));
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
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _comImpInvoiceinfo.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);
                
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
        public async Task<IActionResult> EditComImpInvoiceInfo(ComImpInvoiceInfoEditViewModel comImpInvoiceInfoEditViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    if (comImpInvoiceInfoEditViewModel.INVPATH != null)
                    {
                        comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo.INVPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comImpInvoiceInfoEditViewModel.INVPATH, "imp_invoice_files");
                    }

                    if (comImpInvoiceInfoEditViewModel.BLPATH != null)
                    {
                        comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo.BLPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comImpInvoiceInfoEditViewModel.BLPATH, "imp_invoice_files");
                    }

                    comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo.USRID = user.Id;

                    if (await _comImpInvoiceinfo.Update(comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo))
                    {
                        var comImpInvdetailses = comImpInvoiceInfoEditViewModel.ComImpInvdetailses.Where(e => e.TRNSID <= 0).ToList();

                        foreach (var item in comImpInvdetailses)
                        {
                            item.INVID = comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo.INVID;
                            item.INVNO = comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo.INVNO;
                            item.USRID = user.Id;
                            item.TRNSDATE = DateTime.Now;
                        }

                        await _comImpInvdetails.InsertRangeByAsync(comImpInvdetailses);

                        TempData["message"] = "Successfully Updated Import Invoice Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetComImpInvoiceInfo), $"ComImpInvoiceInfo");
                    }

                    TempData["message"] = "Failed to Update Import Invoice Information.";
                    TempData["type"] = "error";
                    return View(comImpInvoiceInfoEditViewModel);
                }

                TempData["message"] = "Failed to Update Import Invoice Information.";
                TempData["type"] = "error";
                return View(comImpInvoiceInfoEditViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImportInvoiceFromPreviousList(string X, string Y)
        {
            try
            {
                // COMMERCIAL INVOICE INFORMATION (IMPORT)
                var comImpInvoice = await _comImpInvoiceinfo.FindByIdAsync(int.Parse(_protector.Unprotect(Y)));
                comImpInvoice.EncryptedId = Y;

                // COMMERCIAL INVOICE DETAILS (IMPORT)
                var comImpDetails = await _comImpInvdetails.FindByIdAsync(int.Parse(_protector.Unprotect(X)));

                // PRODUCT INFORMATION (BASIC)
                var basProductinfos = await _basProductinfo.GetAll();

                // CREATE AN OBJECT OF ComImpInvoiceInfoEditViewModel TO STORE THE RESULT
                var comImpInvoiceInfoEditViewModel = new ComImpInvoiceInfoEditViewModel();
                comImpInvoiceInfoEditViewModel.BasProductinfos = basProductinfos.ToList();

                // CHECK POINT
                if (comImpInvoice != null && comImpDetails != null)
                {
                    comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo = comImpInvoice;

                    if (comImpInvoice.INVNO == comImpDetails.INVNO)
                    {
                        await _comImpInvdetails.Delete(comImpDetails);
                        var comImpDetainees = await _comImpInvdetails.FindByInvNoAsync(comImpInvoice.INVNO);
                        comImpInvoiceInfoEditViewModel.ComImpInvdetailsesForExistingList = comImpDetainees.Select(e =>
                        {
                            e.EncryptedId = _protector.Protect(e.TRNSID.ToString());
                            return e;
                        }).ToList();
                    }
                    else
                    {
                        var comImpDetainees = await _comImpInvdetails.FindByInvNoAsync(comImpInvoice.INVNO);
                        comImpInvoiceInfoEditViewModel.ComImpInvdetailsesForExistingList = comImpDetainees.Select(e =>
                        {
                            e.EncryptedId = _protector.Protect(e.TRNSID.ToString());
                            return e;
                        }).ToList();
                    }

                    return PartialView($"RemoveImportInvoiceFromPreviousListTable", comImpInvoiceInfoEditViewModel);
                }

                var comImpInvdetailses = await _comImpInvdetails.FindByInvNoAsync(comImpInvoice.INVNO);
                comImpInvoiceInfoEditViewModel.ComImpInvdetailsesForExistingList = comImpInvdetailses.Select(e =>
                {
                    e.EncryptedId = _protector.Protect(e.TRNSID.ToString());
                    return e;
                }).ToList();
                return PartialView($"RemoveImportInvoiceFromPreviousListTable", comImpInvoiceInfoEditViewModel);
            }
            catch (Exception)
            {
                return Json("Invalid Operation. Please reload your webpage.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditComImpInvoiceInfo(string invId)
        {
            try
            {
                var comImpInvoiceInfoEditViewModel = await _comImpInvoiceinfo.FindByInvIdAsync(int.Parse(_protector.Unprotect(invId)));

                if (comImpInvoiceInfoEditViewModel != null)
                {
                    return View(comImpInvoiceInfoEditViewModel);
                }

                return RedirectToAction(nameof(GetComImpInvoiceInfo), $"ComImpInvoiceInfo");
            }
            catch (Exception ex)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("CommercialImportInvoice/Details/{invId?}")]
        public async Task<IActionResult> DetailsComImpInvoiceInfo(string invId)
        {
            try
            {
                var findBInvIdIncludeAllAsync = await _comImpInvoiceinfo.FindBInvIdIncludeAllAsync(int.Parse(_protector.Unprotect(invId)));

                if (findBInvIdIncludeAllAsync != null)
                {
                    return View(findBInvIdIncludeAllAsync);
                }

                return RedirectToAction(nameof(GetComImpInvoiceInfo), $"ComImpInvoiceInfo");

            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComImpInvoiceInfo(string invId)
        {
            try
            {
                var comImpInvoiceinfo = await _comImpInvoiceinfo.FindByIdAsync(int.Parse(_protector.Unprotect(invId)));

                if (comImpInvoiceinfo == null)
                {
                    TempData["message"] = "Invalid Operation";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetComImpInvoiceInfo), $"ComImpInvoiceInfo");
                }
                else
                {
                    var comImpInvDetailsList = await _comImpInvdetails.FindByInvNoAsync(comImpInvoiceinfo.INVNO);
                    var comImpDetainees = comImpInvDetailsList.ToList();

                    if (comImpDetainees.Any())
                    {
                        await _comImpInvdetails.DeleteRange(comImpDetainees);
                    }

                    await _comImpInvoiceinfo.Delete(comImpInvoiceinfo);

                    TempData["message"] = "Successfully Deleted Import Invoice Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetComImpInvoiceInfo), $"ComImpInvoiceInfo");
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("CommercialImportInvoice")]
        [Route("CommercialImportInvoice/GetAll")]
        public IActionResult GetComImpInvoiceInfo()
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

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsInvNoInUse(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel)
        {
            try
            {
                var isAlreadyExistComImpInvoiceInfo = await _comImpInvoiceinfo.FindByInvNoAsync(comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.INVNO);
                return isAlreadyExistComImpInvoiceInfo ? Json($"Invoice Number [ {comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.INVNO} ] already in use.") : Json(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImportInvoiceDetails(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel, int x)
        {
            try
            {
                ModelState.Clear();
                if (x < 0) return PartialView($"AddComImpInvDetailsTable", await _comImpInvoiceinfo.GetInitObjForDetailsTableByAsync(comImpInvoiceInfoCreateViewModel));
                comImpInvoiceInfoCreateViewModel.ComImpInvdetailses.RemoveAt(x);

                return PartialView($"AddComImpInvDetailsTable", await _comImpInvoiceinfo.GetInitObjForDetailsTableByAsync(comImpInvoiceInfoCreateViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComImpInvDetails(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel)
        {
            try
            {
                ModelState.Clear();
                if (comImpInvoiceInfoCreateViewModel.IsDelete)
                {
                    var comImpInvdetails = comImpInvoiceInfoCreateViewModel.ComImpInvdetailses[comImpInvoiceInfoCreateViewModel.RemoveIndex];

                    if (comImpInvdetails.TRNSID > 0)
                    {
                        await _comImpInvdetails.Delete(comImpInvdetails);
                    }

                    comImpInvoiceInfoCreateViewModel.ComImpInvdetailses.RemoveAt(comImpInvoiceInfoCreateViewModel.RemoveIndex);

                    return PartialView($"AddComImpInvDetailsTable", await _comImpInvoiceinfo.GetInitObjForDetailsTableByAsync(comImpInvoiceInfoCreateViewModel));
                }

                //if (comImpInvoiceInfoCreateViewModel.ComImpInvdetailses.Any(e =>
                //        (e.PRODID != null && e.PRODID == comImpInvoiceInfoCreateViewModel.ComImpInvdetails.PRODID) ||
                //        (e.CHEMPRODID != null &&
                //         e.CHEMPRODID == comImpInvoiceInfoCreateViewModel.ComImpInvdetails.CHEMPRODID) ||
                //        (e.YARNLOTID != null &&
                //         e.YARNLOTID == comImpInvoiceInfoCreateViewModel.ComImpInvdetails.YARNLOTID)))
                //{
                //    return PartialView($"AddComImpInvDetailsTable", await _comImpInvoiceinfo.GetInitObjForDetailsTableByAsync(comImpInvoiceInfoCreateViewModel));
                //}

                comImpInvoiceInfoCreateViewModel.ComImpInvdetails.AMOUNT = comImpInvoiceInfoCreateViewModel.ComImpInvdetails.QTY * comImpInvoiceInfoCreateViewModel.ComImpInvdetails.RATE;
                comImpInvoiceInfoCreateViewModel.ComImpInvdetailses.Add(comImpInvoiceInfoCreateViewModel.ComImpInvdetails);
                return PartialView($"AddComImpInvDetailsTable", await _comImpInvoiceinfo.GetInitObjForDetailsTableByAsync(comImpInvoiceInfoCreateViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateComImpInvoiceInfo(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Invalid Operation";
                    TempData["type"] = "error";
                    return View(await _comImpInvoiceinfo.GetInitObjByAsync(comImpInvoiceInfoCreateViewModel));
                }

                if (await _comImpInvoiceinfo.FindByInvNoAsync(comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.INVNO))
                {
                    TempData["message"] = "Already Exist With The Same Invoice Number. Try With The Different Number.";
                    TempData["type"] = "error";
                    return View(await _comImpInvoiceinfo.GetInitObjByAsync(comImpInvoiceInfoCreateViewModel));
                }

                var user = await _userManager.GetUserAsync(User);

                if (comImpInvoiceInfoCreateViewModel.INVPATH != null)
                {
                    comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.INVPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comImpInvoiceInfoCreateViewModel.INVPATH, "imp_invoice_files");
                }

                if (comImpInvoiceInfoCreateViewModel.BLPATH != null)
                {
                    comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.BLPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comImpInvoiceInfoCreateViewModel.BLPATH, "imp_invoice_files");
                }

                comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.USRID = user.Id;
                var comImpInvoiceinfo = await _comImpInvoiceinfo.GetInsertedObjByAsync(comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo);

                if (comImpInvoiceinfo == null)
                {
                    TempData["message"] = "Invalid Operation";
                    TempData["type"] = "error";
                    return View(await _comImpInvoiceinfo.GetInitObjByAsync(comImpInvoiceInfoCreateViewModel));
                }

                if (comImpInvoiceInfoCreateViewModel.ComImpInvdetailses.Any())
                {
                    await _comImpInvdetails.InsertRangeByAsync(comImpInvoiceInfoCreateViewModel.ComImpInvdetailses.Select(e =>
                    {
                        e.INVID = comImpInvoiceinfo.INVID;
                        e.INVNO = comImpInvoiceinfo.INVNO;
                        e.USRID = user.Id;
                        e.TRNSDATE = DateTime.Now;
                        return e;
                    }));
                }

                TempData["message"] = "Successfully Added Import Invoice Information";
                TempData["type"] = "success";
                return RedirectToAction(nameof(GetComImpInvoiceInfo), $"ComImpInvoiceInfo");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateComImpInvoiceInfo()
        {
            return View(await _comImpInvoiceinfo.GetInitObjByAsync(new ComImpInvoiceInfoCreateViewModel()));
        }
    }
}