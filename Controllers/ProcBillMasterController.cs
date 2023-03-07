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
    public class ProcBillMasterController : Controller
    {
        private readonly IPROC_BILL_MASTER _procBillMaster;
        private readonly IPROC_BILL_DETAILS _procBillDetails;
        //private readonly IF_GS_RECEIVE_MASTER _fGsReceiveMaster;
        //private readonly IF_GS_INDENT_MASTER _fGsIndentMaster;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        //private readonly IF_GS_PURCHASE_REQUISITION_DETAILS _fGsPurchaseRequisitionDetails;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IDataProtector _protector;
        //private readonly string title = "Weaving Production Information";

        public ProcBillMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IPROC_BILL_MASTER procBillMaster,
            IPROC_BILL_DETAILS procBillDetails,
            //IF_GS_RECEIVE_MASTER fGsReceiveMaster,
            //IF_GS_INDENT_MASTER fGsIndentMaster,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            //IF_GS_PURCHASE_REQUISITION_DETAILS fGsPurchaseRequisitionDetails,
            UserManager<ApplicationUser> userManager
        )
        {
            _procBillMaster = procBillMaster;
            _procBillDetails = procBillDetails;
            //_fGsReceiveMaster = fGsReceiveMaster;
            //_fGsIndentMaster = fGsIndentMaster;
            _fGsProductInformation = fGsProductInformation;
            //_fGsPurchaseRequisitionDetails = fGsPurchaseRequisitionDetails;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        //[Route("GetAll")]
        public IActionResult GetProcBillMasterList()
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
        //[Route("GetTableData")]
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

            var data = (List<PROC_BILL_MASTER>)await _procBillMaster.GetAllProcBillMaster();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.BILLDATE != null && m.BILLDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.CHALLANID != null && m.CHALLANID.ToString().ToUpper().Contains(searchValue)
                                       || m.SOURCE != null && m.SOURCE.ToString().ToUpper().Contains(searchValue)
                                       || m.PAYMODE != null && m.PAYMODE.ToString().ToUpper().Contains(searchValue)
                                       || m.BILLAMOUNT != null && m.BILLAMOUNT.ToString().ToUpper().Contains(searchValue)
                                       || m.ACTBILL != null && m.ACTBILL.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
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
        //[Route("Create")]
        public async Task<IActionResult> CreateProcBillMasterInfo()
        {
            try
            {
                return View(await _procBillMaster.GetInitObjByAsync(new ProcBillMasterViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProcBillMasterInfo(ProcBillMasterViewModel procBillMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _procBillMaster.InsertByAsync(procBillMasterViewModel.PROC_BILL_MASTER);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction("GetProcBillMasterList", $"ProcBillMaster");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(procBillMasterViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(procBillMasterViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(procBillMasterViewModel);
            }
        }
    }
}
