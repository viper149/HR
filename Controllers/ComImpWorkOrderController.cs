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
    public class ComImpWorkOrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICOM_IMP_WORK_ORDER_MASTER _comImpWorkOrderMaster;
        private readonly ICOM_IMP_WORK_ORDER_DETAILS _comImpWorkOrderDetails;
        private readonly IRND_PURCHASE_REQUISITION_MASTER _rndPurchaseRequisitionMaster;
        private readonly IDataProtector _protector;
        private readonly string title = "Commercial Import Work Order";

        public ComImpWorkOrderController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            ICOM_IMP_WORK_ORDER_MASTER comImpWorkOrderMaster,
            ICOM_IMP_WORK_ORDER_DETAILS comImpWorkOrderDetails,
            IRND_PURCHASE_REQUISITION_MASTER rndPurchaseRequisitionMaster)
        {
            _userManager = userManager;
            _comImpWorkOrderMaster = comImpWorkOrderMaster;
            _comImpWorkOrderDetails = comImpWorkOrderDetails;
            _rndPurchaseRequisitionMaster = rndPurchaseRequisitionMaster;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetComImpWorkOrder()
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

            var data = (List<COM_IMP_WORK_ORDER_MASTER>)await _comImpWorkOrderMaster.GetAllComImpWorkOrderAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.CONTNO.ToUpper().Contains(searchValue)
                                       || m.WODATE != null && m.WODATE.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)
                                       || m.VALDATE != null && m.VALDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.INDID != null && m.IND.INDSL.INDSLNO.ToUpper().Contains(searchValue)
                                       || m.SUPPID != null && m.SUPP.SUPPNAME.ToUpper().Contains(searchValue)).ToList();
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
        public async Task<IActionResult> CreateComImpWorkOrder()
        {
            try
            {
                var comImpWorkOrderViewModel = new ComImpWorkOrderViewModel();
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.CONTNO = await _comImpWorkOrderMaster.GetLastContNoAsync();
                return View(await _comImpWorkOrderMaster.GetInitObjByAsync(comImpWorkOrderViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateComImpWorkOrder(ComImpWorkOrderViewModel comImpWorkOrderViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var rndPurchaseRequisitionMaster = await _rndPurchaseRequisitionMaster.FindByIdAsync(await _comImpWorkOrderMaster.GetIndslIdByInd(comImpWorkOrderViewModel.ComImpWorkOrderMaster.INDID ?? 0));
                var findPreviousWorkOrder = await _comImpWorkOrderMaster.FindPreviousWorkOrder(comImpWorkOrderViewModel.ComImpWorkOrderMaster
                    .INDID);

                comImpWorkOrderViewModel.ComImpWorkOrderMaster.CREATED_BY = comImpWorkOrderViewModel.ComImpWorkOrderMaster.UPDATED_BY = user.Id;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.CREATED_AT = comImpWorkOrderViewModel.ComImpWorkOrderMaster.UPDATED_AT = DateTime.Now;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.IND = null;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.SUPP = null;

                if (findPreviousWorkOrder != null)
                {
                    rndPurchaseRequisitionMaster.IS_REVISED = false;
                    findPreviousWorkOrder.IS_REVISED = true;
                    await _comImpWorkOrderMaster.Update(findPreviousWorkOrder);
                }
                
                await _rndPurchaseRequisitionMaster.Update(rndPurchaseRequisitionMaster);

                var atLeastOneInsert = false;

                var comImpWorkOrderMaster = await _comImpWorkOrderMaster.GetInsertedObjByAsync(comImpWorkOrderViewModel.ComImpWorkOrderMaster);

                if (comImpWorkOrderMaster.WOID != 0)
                {
                    foreach (var item in comImpWorkOrderViewModel.ComImpWorkOrderDetailsList)
                    {
                        item.WOID = comImpWorkOrderMaster.WOID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                        item.INDD = null;
                        item.LOTID = item.LOTID > 0 ? item.LOTID : null;
                    }

                    if (comImpWorkOrderViewModel.ComImpWorkOrderDetailsList.Any())
                    {
                        if (await _comImpWorkOrderDetails.InsertRangeByAsync(comImpWorkOrderViewModel.ComImpWorkOrderDetailsList))
                        {
                            atLeastOneInsert = true;
                        }
                    }

                    if (!atLeastOneInsert)
                    {
                        await _comImpWorkOrderMaster.Delete(comImpWorkOrderMaster);
                        TempData["message"] = $"Please Add Count to List First!";
                        TempData["type"] = "error";
                        return View(await _comImpWorkOrderMaster.GetInitObjByAsync(comImpWorkOrderViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetComImpWorkOrder), $"ComImpWorkOrder");
                }

                await _comImpWorkOrderMaster.Delete(comImpWorkOrderMaster);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(await _comImpWorkOrderMaster.GetInitObjByAsync(comImpWorkOrderViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Costing(Head), Marketing(Head), Super Admin")]
        public async Task<IActionResult> EditComImpWorkOrder(string woId)
        {
            return View(await _comImpWorkOrderMaster.GetInitObjByAsync(await _comImpWorkOrderMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(woId)))));
        }

        [HttpPost]
        public async Task<IActionResult> EditComImpWorkOrder(ComImpWorkOrderViewModel comImpWorkOrderViewModel)
        {
            var comImpWorkOrderMaster = await _comImpWorkOrderMaster.FindByIdAsync(int.Parse(_protector.Unprotect(comImpWorkOrderViewModel.ComImpWorkOrderMaster.EncryptedId)));

            if (comImpWorkOrderMaster != null)
            {
                var user = await _userManager.GetUserAsync(User);

                comImpWorkOrderViewModel.ComImpWorkOrderMaster.WOID = comImpWorkOrderMaster.WOID;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.CREATED_AT = comImpWorkOrderMaster.CREATED_AT;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.CREATED_BY = comImpWorkOrderMaster.CREATED_BY;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.UPDATED_AT = DateTime.Now;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.UPDATED_BY = user.Id;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.IND = null;
                comImpWorkOrderViewModel.ComImpWorkOrderMaster.SUPP = null;


                if (await _comImpWorkOrderMaster.Update(comImpWorkOrderViewModel.ComImpWorkOrderMaster))
                {
                    var comImpWorkOrderDetailses = comImpWorkOrderViewModel.ComImpWorkOrderDetailsList.Where(d => d.TRANSID > 0).ToList();

                    if (comImpWorkOrderDetailses.Any())
                    {
                        foreach (var item in comImpWorkOrderDetailses)
                        {
                            var comImpWorkOrderDetails = await _comImpWorkOrderDetails.FindByIdAsync(item.TRANSID);
                            item.INDD = null;
                            item.LOT = null;
                            //item.WOID = comImpWorkOrderMaster.WOID;
                            //item.UPDATED_AT = DateTime.Now;
                            //item.UPDATED_BY = user.Id;

                            comImpWorkOrderDetails.UPRICE = item.UPRICE;
                            comImpWorkOrderDetails.TOTAL = item.TOTAL;
                            comImpWorkOrderDetails.UPDATED_AT = DateTime.Now;
                            comImpWorkOrderDetails.UPDATED_BY = user.Id;

                            await _comImpWorkOrderDetails.Update(comImpWorkOrderDetails);
                        }
                    }
                    TempData["message"] = $"Successfully Updated {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetComImpWorkOrder), $"ComImpWorkOrder");
                }
            }

            ModelState.AddModelError("", "We can not process your request. Please try again later.");
            return View(await _comImpWorkOrderMaster.GetInitObjByAsync(comImpWorkOrderViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddToList(ComImpWorkOrderViewModel comImpWorkOrderViewModel)
        {
            try
            {
                ModelState.Clear();
                if (comImpWorkOrderViewModel.IsDelete)
                {
                    var comImpWorkOrderDetails = comImpWorkOrderViewModel.ComImpWorkOrderDetailsList[comImpWorkOrderViewModel.RemoveIndex];

                    if (comImpWorkOrderDetails.TRANSID > 0)
                    {
                        await _comImpWorkOrderDetails.Delete(comImpWorkOrderDetails);
                    }

                    comImpWorkOrderViewModel.ComImpWorkOrderDetailsList.RemoveAt(comImpWorkOrderViewModel.RemoveIndex);
                }
                else if (!comImpWorkOrderViewModel.ComImpWorkOrderDetailsList.Any(e => e.COUNTID.Equals(comImpWorkOrderViewModel.ComImpWorkOrderDetails.COUNTID)))
                {
                    if (TryValidateModel(comImpWorkOrderViewModel.ComImpWorkOrderDetails))
                    {
                        comImpWorkOrderViewModel.ComImpWorkOrderDetailsList.Add(comImpWorkOrderViewModel.ComImpWorkOrderDetails);
                    }
                }

                return PartialView($"ComImpWorkOrderDetailsPartialView", await _comImpWorkOrderMaster.GetInitDetailsObjByAsync(comImpWorkOrderViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetCountInfo(int indId)
        {
            try
            {
                return Ok(await _comImpWorkOrderDetails.GetCountInfoByIndentIdAsync(indId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetAllByCount(int transId)
        {
            try
            {
                return Ok(await _comImpWorkOrderDetails.GetAllByCountIdAsync(transId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetCalulatedFieldsValue(ComImpWorkOrderViewModel comImpWorkOrderViewModel)
        {
            try
            {
                var comImpWorkOrderDetails = comImpWorkOrderViewModel.ComImpWorkOrderDetailsList[comImpWorkOrderViewModel.RemoveIndex];
                comImpWorkOrderDetails.TOTAL = comImpWorkOrderDetails.QTY * comImpWorkOrderDetails.UPRICE;

                return Ok(comImpWorkOrderDetails);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
