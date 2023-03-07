using System;
using System.Collections.Generic;
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
    public class AccLoanManagementMController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IACC_LOAN_MANAGEMENT_M _accLoanManagementM;
        private readonly IBAS_BEN_BANK_MASTER _basBenBankMaster;
        private readonly ICOM_IMP_INVOICEINFO _comImpInvoiceInfo;
        private readonly ICOM_IMP_LCINFORMATION _comImpLcInformation;

        public AccLoanManagementMController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IACC_LOAN_MANAGEMENT_M accLoanManagementM,
            IBAS_BEN_BANK_MASTER basBenBankMaster,
            ICOM_IMP_INVOICEINFO comImpInvoiceInfo,
            ICOM_IMP_LCINFORMATION comImpLcInformation
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _accLoanManagementM = accLoanManagementM;
            _basBenBankMaster = basBenBankMaster;
            _comImpInvoiceInfo = comImpInvoiceInfo;
            _comImpLcInformation = comImpLcInformation;
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

                var data = await _accLoanManagementM.GetForDataTableByAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.LOANID.ToString().ToUpper().Contains(searchValue)
                                           || (m.LOANDATE != null && m.LOANDATE.ToString().Contains(searchValue))
                                           || (m.LOAN_AMT != null && m.LOAN_AMT.ToString().Contains(searchValue))
                                           || (m.EXP_DATE != null && m.EXP_DATE.ToString().Contains(searchValue))
                                           || (m.INTEREST_RATE != null && m.INTEREST_RATE.ToString().Contains(searchValue))
                                           || (m.PAID_AMT != null && m.PAID_AMT.ToString().Contains(searchValue))
                                           || (m.PAID_DATE != null && m.PAID_DATE.ToString().Contains(searchValue))
                                           || (m.BANK!=null && m.BANK.BEN_BANK != null && m.BANK.BEN_BANK.ToString().Contains(searchValue))
                                           || (m.LC!=null && m.LC.LCNO != null && m.LC.LCNO.ToString().Contains(searchValue))
                                           || (m.BANKID != null && m.BANKID.ToString().ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
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
        public IActionResult GetAccLoanManagementMList()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccLoanManagementInfo()
        {
            try
            {
                var accLoanManagementMViewModel = new AccLoanManagementMViewModel()
                {
                    BankList = (List<BAS_BEN_BANK_MASTER>)await _basBenBankMaster.GetAll(),
                    LcList = (List<COM_IMP_LCINFORMATION>)await _comImpLcInformation.GetAll(),
                    InvoiceList = (List<COM_IMP_INVOICEINFO>)await _comImpInvoiceInfo.GetAll()
                };

                return View(accLoanManagementMViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccLoanManagementInfo(AccLoanManagementMViewModel accLoanManagementMViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accLoanManagementM.InsertByAsync(accLoanManagementMViewModel.ACC_LOAN_MANAGEMENT_M);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Account Loan Management Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetAccLoanManagementMList", $"AccLoanManagementM");
                    }

                    TempData["message"] = "Failed to Add Account Loan Management Information..";
                    TempData["type"] = "error";
                    return View(accLoanManagementMViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(accLoanManagementMViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Account Loan Management Information..";
                TempData["type"] = "error";
                return View(accLoanManagementMViewModel);
            }
        }
    }

}
