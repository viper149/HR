using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class AccExportRealizationController : Controller
    {
        private readonly IACC_EXPORT_REALIZATION _accExportRealization;
        private readonly ICOM_EX_INVOICEMASTER _comExInvoiceMaster;
        private readonly ICOM_EX_LCINFO _comExLcInfo;
        private readonly ICOM_EX_INVDETAILS _comExInvDetails;
        private readonly ICOM_EX_FABSTYLE _comExFabstyle;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public AccExportRealizationController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IACC_EXPORT_REALIZATION accExportRealization,
            ICOM_EX_INVOICEMASTER comExInvoiceMaster,
            ICOM_EX_LCINFO comExLcInfo,
            ICOM_EX_INVDETAILS comExInvDetails,
            ICOM_EX_FABSTYLE comExFabstyle,
            UserManager<ApplicationUser> userManager
        )
        {
            _accExportRealization = accExportRealization;
            _comExInvoiceMaster = comExInvoiceMaster;
            _comExLcInfo = comExLcInfo;
            _comExInvDetails = comExInvDetails;
            _comExFabstyle = comExFabstyle;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
        [Route("AccountExportRealization/PostAuditControl")]
        public async Task<IActionResult> PostAuditRealization(AccExRealizationViewModel accExRealizationViewModel)
        {
            var accExportRealization = await _accExportRealization.FindByIdAsync(int.Parse(_protector.Unprotect(accExRealizationViewModel.AccExportRealization.EncryptedId)));

            if (accExportRealization != null)
            {
                var user = await _userManager.GetUserAsync(User);

                accExportRealization.REMARKS = accExRealizationViewModel.AccExportRealization.REMARKS;
                accExportRealization.AUDITBY ??= user.Id;
                accExportRealization.AUDITDATE ??= (accExRealizationViewModel.AccExportRealization.AUDITDATE ?? DateTime.Now);

                if (await _accExportRealization.Update(accExportRealization))
                {
                    TempData["message"] = "Successfully Added Audit Note.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Failed To Add Audit Note.";
                    TempData["type"] = "error";
                }
            }
            else
            {
                TempData["message"] = "Failed To Add Audit Note.";
                TempData["type"] = "error";
            }

            return RedirectToAction(nameof(GetAccExRealizationWithPaged), $"AccExportRealization");
        }

        [HttpGet]
        [Route("AccountExportRealization/AuditControl/{trnsId?}")]
        public async Task<IActionResult> AuditRealization(string trnsId)
        {
            return View(await _accExportRealization.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(trnsId))));
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

                var data = await _accExportRealization.GetAccExRealizationAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.INVID.ToString().ToUpper().Contains(searchValue)
                                           || m.INVOICE != null && !string.IsNullOrEmpty(m.INVOICE.INVNO) && m.INVOICE.INVNO.ToUpper().Contains(searchValue)
                                           || m.REZDATE != null && m.REZDATE.ToString().Contains(searchValue)
                                           || m.PRC_USD != null && m.PRC_USD.ToString().Contains(searchValue)
                                           || m.ERQ != null && m.ERQ.ToString().Contains(searchValue)
                                           || m.ORQ != null && m.ORQ.ToString().Contains(searchValue)
                                           || m.CD != null && m.CD.ToString().Contains(searchValue)
                                           || m.OD != null && m.OD.ToString().Contains(searchValue)
                                           || m.RATE != null && m.RATE.ToString().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.REMARKS) && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
        public IActionResult GetAccExRealizationWithPaged()
        {
            try
            {
                //var gspInfo = await _accExportRealization.GetAccExRealizationWithPaged(pi, ps);
                //var totalItems = await _accExportRealization.GetAll();

                //var finalResult = new PagedResult<ACC_EXPORT_REALIZATION>
                //{
                //    Data = gspInfo.Select(e => { e.EncryptedId = _protector.Protect(e.TRNSID.ToString()); return e; }).ToList(),
                //    TotalItems = totalItems.Count(),
                //    PageNumber = pi,
                //    PageSize = ps
                //};

                //ViewData["controller"] = "AccExportRealization";
                //ViewData["action"] = "GetAccExRealizationWithPaged";

                //return View(finalResult);
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccExRealization()
        {
            AccExRealizationViewModel accExRealizationViewModel = new AccExRealizationViewModel();
            accExRealizationViewModel.AccExportRealization = new ACC_EXPORT_REALIZATION
            {
                REZDATE = DateTime.Now,
                PRC_EURO = 0
            };
            return View(await GetInfo(accExRealizationViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccExRealization(AccExRealizationViewModel accExRealizationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    accExRealizationViewModel.AccExportRealization.TRNSDATE = DateTime.Now;
                    var isGspInfoInsert = await _accExportRealization.InsertByAsync(accExRealizationViewModel.AccExportRealization);
                    if (isGspInfoInsert)
                    {
                        TempData["message"] = "Successfully added Realization.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Realization";
                        TempData["type"] = "error";
                        return View(await GetInfo(accExRealizationViewModel));
                    }
                }
                else
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage + " " + v.Exception).ToList();
                    TempData["message"] = "Invalid Input. Please Try Again.";
                    TempData["type"] = "error";
                    return View(await GetInfo(accExRealizationViewModel));
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Add Realization";
                TempData["type"] = "error";
                return View(await GetInfo(accExRealizationViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAccExRealization(string trnsId)
        {
            try
            {
                string decryptedId = _protector.Unprotect(trnsId);
                var decryptedIntId = Convert.ToInt32(decryptedId);
                var realization = await _accExportRealization.FindByIdAsync(decryptedIntId);
                if (realization != null)
                {
                    var result = await _accExportRealization.Delete(realization);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Realization.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Delete Realization.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                    }
                }
                else
                {
                    TempData["message"] = "Realization Information Not Found!.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Realization.";
                TempData["type"] = "error";
                return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAccExRealization(string trnsId)
        {
            try
            {
                var accExRealizationViewModel = new AccExRealizationViewModel();
                var realization = await _accExportRealization.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId)));

                if (realization != null)
                {
                    realization.EncryptedId = _protector.Protect(realization.TRNSID.ToString());
                    accExRealizationViewModel.AccExportRealization = realization;
                    accExRealizationViewModel.LcInfoViewModel.ComExInvoiceMaster = await _comExInvoiceMaster.FindByIdIncludeAllNotRealizedAsync(realization.INVID);
                    accExRealizationViewModel.LcInfoViewModel.ComExLcInfo = await _comExLcInfo.FindByIdIncludeAllAsync(accExRealizationViewModel.LcInfoViewModel.ComExInvoiceMaster.LCID ?? 0);
                    var lcDetails = await _comExInvDetails.FindByInvNoAsync(accExRealizationViewModel.LcInfoViewModel
                            .ComExInvoiceMaster.INVNO);
                    accExRealizationViewModel.LcInfoViewModel.ComExInvDetails = lcDetails.ToList();

                    return View(accExRealizationViewModel);
                }
                TempData["message"] = "Realization Information Not Found.";
                TempData["type"] = "error";
                return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Retrieve Realization Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAccExRealization(string trnsId)
        {
            try
            {
                var realization = await _accExportRealization.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId)));

                if (realization != null)
                {
                    var accExRealizationViewModel = new AccExRealizationViewModel();

                    realization.EncryptedId = _protector.Protect(realization.TRNSID.ToString());
                    accExRealizationViewModel.AccExportRealization = realization;
                    return View(await GetInfo(accExRealizationViewModel));
                }
                else
                {
                    TempData["message"] = "Realization Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "Failed to Retrieve Realization.";
                TempData["type"] = "error";
                return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAccExRealization(AccExRealizationViewModel accExRealizationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (accExRealizationViewModel.AccExportRealization.TRNSID == Int32.Parse(_protector.Unprotect(accExRealizationViewModel.AccExportRealization.EncryptedId)))
                    {
                        accExRealizationViewModel.AccExportRealization.TRNSID = Int32.Parse(_protector.Unprotect(accExRealizationViewModel.AccExportRealization.EncryptedId));

                        var isRealizationUpdated = await _accExportRealization.Update(accExRealizationViewModel.AccExportRealization);

                        if (isRealizationUpdated)
                        {
                            TempData["message"] = "Successfully Updated Realization.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                        }
                        TempData["message"] = "Failed to Update Realization.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                    }
                    TempData["message"] = "Invalid Realization";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetAccExRealizationWithPaged", $"AccExportRealization");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(await GetInfo(accExRealizationViewModel)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Update Realization. Please Contact With Developer.";
                TempData["type"] = "error";
                return View(await GetInfo(await GetInfo(accExRealizationViewModel)));
            }
        }

        public async Task<AccExRealizationViewModel> GetInfo(AccExRealizationViewModel accExRealizationViewModel)
        {
            var comExInvoiceMasters = await _comExInvoiceMaster.GetAll();
            accExRealizationViewModel.ComExInvoiceMasters = comExInvoiceMasters.OrderByDescending(c => c.INVDATE).ToList();
            return accExRealizationViewModel;
        }

        [HttpGet]
        public async Task<LcInfoViewModel> GetInvoiceInfo(int invId)
        {
            try
            {
                var lcInfoViewModel = new LcInfoViewModel
                {
                    ComExInvoiceMaster = await _comExInvoiceMaster.FindByIdIncludeAllNotRealizedAsync(invId)
                };

                return lcInfoViewModel;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}