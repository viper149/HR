using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.ProcWorkOrder;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ProcWorkOrderController : Controller
    {
        private readonly IPROC_WORKORDER_MASTER _procWorkorderMaster;
        private readonly IPROC_WORKORDER_DETAILS _procWorkorderDetails;
        private readonly IBAS_SUPPLIERINFO _basSupplierinfo;
        private readonly IF_GS_PRODUCT_INFORMATION _gsProductInformation;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IDataProtector _protector;
        //private readonly string title = "Weaving Production Information";

        public ProcWorkOrderController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IPROC_WORKORDER_MASTER procWorkorderMaster,
            IPROC_WORKORDER_DETAILS procWorkorderDetails,
            IBAS_SUPPLIERINFO basSupplierinfo,
            IF_GS_PRODUCT_INFORMATION gsProductInformation,
            UserManager<ApplicationUser> userManager
        )
        {
            _procWorkorderMaster = procWorkorderMaster;
            _procWorkorderDetails = procWorkorderDetails;
            _basSupplierinfo = basSupplierinfo;
            _gsProductInformation = gsProductInformation;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Post")]
        [Route("ProcWorkOrder/GetIndentProductInfo")]
        public async Task<IActionResult> GetIndentProductInfo(ProcWorkOrderViewModel procWorkOrderViewModel)
        {
            return Ok(await _procWorkorderMaster.GetIndentProductInfoByAsync(procWorkOrderViewModel));
        }

        [HttpGet]
        //[Route("GetAll")]
        public IActionResult GetProcWorkOrderList()
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

            var data = (List<PROC_WORKORDER_MASTER>)await _procWorkorderMaster.GetAllProcWorkOrder();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.WODATE != null && m.WODATE.ToString().ToUpper().Contains(searchValue)
                                       || m.CURRENCY != null && m.CURRENCY.ToString().ToUpper().Contains(searchValue)
                                       || m.PAYMODE != null && m.PAYMODE.ToString().ToUpper().Contains(searchValue)
                                       || m.UNIT != null && m.UNIT.ToString().ToUpper().Contains(searchValue)
                                       || m.CARRING_AMT != null && m.CARRING_AMT.ToString().ToUpper().Contains(searchValue)
                                       || m.DISC_AMT != null && m.DISC_AMT.ToString().ToUpper().Contains(searchValue)
                                       || m.PAY_AMT != null && m.PAY_AMT.ToString().ToUpper().Contains(searchValue)).ToList();
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
        public async Task<IActionResult> CreateProcWorkOrderInfo()
        {
            try
            {
                return View(await _procWorkorderMaster.GetInitObjByAsync(new ProcWorkOrderViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProcWorkOrderInfo(ProcWorkOrderViewModel ProcWorkOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _procWorkorderMaster.InsertByAsync(ProcWorkOrderViewModel.ProcWorkOrderMaster);
                   
                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction("GetProcWorkOrderList", $"ProcWorkOrder");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(ProcWorkOrderViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(ProcWorkOrderViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(ProcWorkOrderViewModel);
            }
        }
    }

}
