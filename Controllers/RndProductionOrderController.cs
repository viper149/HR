using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndProductionOrderController : Controller
    {
        private readonly IRND_PRODUCTION_ORDER _rndProductionOrder;
        private readonly IRND_MSTR_ROLL _rndMstrRoll;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPL_BULK_PROG_SETUP_M _plBulkProgSetupM;
        private readonly IDataProtector _protector;

        public RndProductionOrderController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_PRODUCTION_ORDER rndProductionOrder,
            IRND_MSTR_ROLL rndMstrRoll,
            UserManager<ApplicationUser> userManager,
            IPL_BULK_PROG_SETUP_M plBulkProgSetupM
            )
        {
            _rndProductionOrder = rndProductionOrder;
            _rndMstrRoll = rndMstrRoll;
            _userManager = userManager;
            _plBulkProgSetupM = plBulkProgSetupM;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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
                var data = (List<RND_PRODUCTION_ORDER>)await _rndProductionOrder.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                data = data.OrderBy(c =>
                                    c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])
                                        ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c))
                                .ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType()
                                            .GetProperty(subStrings[1])
                                            ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }
                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.OPT1.Contains(searchValue)
                                           || m.ORPTID.ToString().ToUpper().Contains(searchValue)
                                           || m.ORDER_QTY_YDS.ToString().ToUpper().Contains(searchValue)
                                           || m.ORDER_QTY_MTR.ToString().ToUpper().Contains(searchValue)
                                           || m.SO != null && m.OPT2.ToUpper().Contains(searchValue)
                                           || m.SO != null && m.OPT3.ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var totalRecords = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                return Json(new
                {
                    draw,
                    recordsFiltered = totalRecords,
                    recordsTotal = totalRecords,
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
        public IActionResult GetProductionOrder()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductionOrder()
        {
            try
            {
                var result = await _rndProductionOrder.GetInitObjects(new RndProductionOrderViewModel());
                result.RndProductionOrder = new RND_PRODUCTION_ORDER { 
                    OTYPEID = 401,
                    ORPTID = 2000090
                };
                return View(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductionOrder(RndProductionOrderViewModel rndProductionOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    rndProductionOrderViewModel.RndProductionOrder.CREATED_BY = user.Id;

                    if (rndProductionOrderViewModel.RndProductionOrder.OTYPEID == 401 || 
                        rndProductionOrderViewModel.RndProductionOrder.OTYPEID == 402 || 
                        rndProductionOrderViewModel.RndProductionOrder.OTYPEID == 419 ||
                        rndProductionOrderViewModel.RndProductionOrder.OTYPEID == 422)
                    {
                        rndProductionOrderViewModel.RndProductionOrder.RSNO = null;
                    }
                    else
                    {
                        rndProductionOrderViewModel.RndProductionOrder.RSNO = rndProductionOrderViewModel.RndProductionOrder.ORDERNO;
                        rndProductionOrderViewModel.RndProductionOrder.ORDERNO = null;
                    }

                    if (rndProductionOrderViewModel.RndProductionOrder.ORPTID == 0)
                    {
                        rndProductionOrderViewModel.RndProductionOrder.ORPTID = 2000090;
                    }
                    
                    var poId = await _rndProductionOrder.InsertAndGetIdAsync(rndProductionOrderViewModel.RndProductionOrder);

                    if (poId != 0)
                    {
                        var bulkSetup = new PL_BULK_PROG_SETUP_M
                        {
                            ORDERNO = poId,
                            WARP_QTY = rndProductionOrderViewModel.RndProductionOrder.WARP_LENGTH_MTR,
                            REMARKS = "",
                            CREATED_AT = DateTime.Now,
                            UPDATED_AT = DateTime.Now,
                            CREATED_BY = user.Id,
                            UPDATED_BY = user.Id,
                            OPT1 = rndProductionOrderViewModel.RndProductionOrder.OTYPEID.ToString(),
                            OPT2 = "",
                            OPT3 = "",
                            OPT4 = "",
                        };

                        await _plBulkProgSetupM.InsertAndGetIdAsync(bulkSetup);

                        //foreach (var item in rndProductionOrderViewModel.PlOrderwiseLotInfoList)
                        //{
                        //    item.POID = poId;
                        //    await _plOrderwiseLotInfo.InsertByAsync(item);
                        //}
                        TempData["message"] = "Successfully added Production Order(P.O).";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
                    }
                    TempData["message"] = "Failed to Add Production Order(P.O).";
                    TempData["type"] = "error";
                    return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProductionOrder(string id)
        {
            try
            {
                var exData = await _rndProductionOrder.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (exData != null)
                {
                    var rndProductionOrderViewModel = new RndProductionOrderViewModel
                    {
                        RndProductionOrder = exData

                    };
                    if (User.IsInRole("Planning(F)"))
                    {
                        ViewBag.SetupContent = "setup-content";
                        ViewBag.Step = "step";
                    }
                    rndProductionOrderViewModel.RndProductionOrder.EncryptedId = _protector.Protect(rndProductionOrderViewModel.RndProductionOrder.POID.ToString());
                    var requiredInfo = await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel);
                    return View(requiredInfo);
                }
                TempData["message"] = "Failed to retrieve Data.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProductionOrder(RndProductionOrderViewModel rndProductionOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);


                    var exData = await _rndProductionOrder.FindByIdAsync(int.Parse(_protector.Unprotect(rndProductionOrderViewModel.RndProductionOrder.EncryptedId)));

                    if (exData != null)
                    {
                        rndProductionOrderViewModel.RndProductionOrder.UPDATED_BY = user.Id;
                        rndProductionOrderViewModel.RndProductionOrder.UPDATED_AT = DateTime.Now;
                        rndProductionOrderViewModel.RndProductionOrder.CREATED_BY = exData.CREATED_BY;
                        rndProductionOrderViewModel.RndProductionOrder.CREATED_AT = exData.CREATED_AT;

                        if (rndProductionOrderViewModel.RndProductionOrder.ORPTID == 0)
                        {
                            rndProductionOrderViewModel.RndProductionOrder.ORPTID = 2000090;
                        }

                        var poId = await _rndProductionOrder.Update(rndProductionOrderViewModel.RndProductionOrder);

                        if (poId)
                        {
                            //foreach (var item in rndProductionOrderViewModel.PlOrderwiseLotInfoList.Where(item => item.TRNSID == 0))
                            //{
                            //    item.CREATED_BY = user.Id;
                            //    item.CREATED_AT = DateTime.Now;
                            //    item.POID = rndProductionOrderViewModel.RndProductionOrder.POID;
                            //    await _plOrderwiseLotInfo.InsertByAsync(item);
                            //}
                            TempData["message"] = "Successfully updated Production Order(P.O).";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
                        }
                        TempData["message"] = "Failed to update Production Order(P.O).";
                        TempData["type"] = "error";
                        return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
                    }
                    TempData["message"] = "Production Order(P.O) not available.";
                    TempData["type"] = "error";
                    return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return View(await _rndProductionOrder.GetInitObjects(rndProductionOrderViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProductionOrder(string id)
        {
            try
            {
                var decryptedId = _protector.Unprotect(id);
                var decryptedIntId = Convert.ToInt32(decryptedId);
                var po = await _rndProductionOrder.FindByIdAsync(decryptedIntId);
                if (po != null)
                {

                    //var lotList = await _plOrderwiseLotInfo.FindByPoIdAsync(po.POID);

                    //await _plOrderwiseLotInfo.DeleteRange(lotList);
                    var result = await _rndProductionOrder.Delete(po);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted P.O";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
                    }
                    TempData["message"] = "Failed to Delete PO. This entity may be used already";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
                }
                TempData["message"] = "P.O Not Found.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete P.O.";
                TempData["type"] = "error";
                return RedirectToAction($"GetProductionOrder", $"RndProductionOrder");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddLotNoDetailsTable(RndProductionOrderViewModel rndProductionOrderViewModel)
        //{
        //    try
        //    {
        //        var flag = rndProductionOrderViewModel.PlOrderwiseLotInfoList.Any(c => c.LOTID.Equals(rndProductionOrderViewModel.PlOrderwiseLotInfo.LOTID) && c.YARNTYPE.Equals(rndProductionOrderViewModel.PlOrderwiseLotInfo.YARNTYPE));

        //        ModelState.Clear();
        //        if (rndProductionOrderViewModel.PlOrderwiseLotInfo.LOTID != 0 && rndProductionOrderViewModel.PlOrderwiseLotInfo.YARNTYPE != 0 && rndProductionOrderViewModel.PlOrderwiseLotInfo.SUPPID != 0 && !flag)
        //        {
        //            rndProductionOrderViewModel.PlOrderwiseLotInfoList.Add(rndProductionOrderViewModel
        //                .PlOrderwiseLotInfo);
        //        }
        //        rndProductionOrderViewModel.PlOrderwiseLotInfoList = (List<PL_ORDERWISE_LOTINFO>)await _plOrderwiseLotInfo.GetInitObjectsAsync(rndProductionOrderViewModel.PlOrderwiseLotInfoList);
        //        return PartialView($"AddLotNoDetailsTable", rndProductionOrderViewModel);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return PartialView($"AddLotNoDetailsTable", rndProductionOrderViewModel);
        //        //throw;
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> GetLotNoDetailsTable(RndProductionOrderViewModel rndProductionOrderViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        var poid = int.Parse(_protector.Unprotect(rndProductionOrderViewModel.RndProductionOrder.EncryptedId));
        //        rndProductionOrderViewModel.PlOrderwiseLotInfoList = (List<PL_ORDERWISE_LOTINFO>)await _plOrderwiseLotInfo.FindByPoIdAsync(poid);

        //        return PartialView($"AddLotNoDetailsTable", rndProductionOrderViewModel);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return PartialView($"AddLotNoDetailsTable", rndProductionOrderViewModel);
        //        //throw;
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveLotNoDetailsTable(RndProductionOrderViewModel rndProductionOrderViewModel, string removeIndexValue)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        if (rndProductionOrderViewModel.PlOrderwiseLotInfoList[int.Parse(removeIndexValue)].TRNSID != 0)
        //        {
        //            await _plOrderwiseLotInfo.Delete(rndProductionOrderViewModel.PlOrderwiseLotInfoList[int.Parse(removeIndexValue)]);
        //        }
        //        rndProductionOrderViewModel.PlOrderwiseLotInfoList.RemoveAt(int.Parse(removeIndexValue));

        //        rndProductionOrderViewModel.PlOrderwiseLotInfoList = (List<PL_ORDERWISE_LOTINFO>)await _plOrderwiseLotInfo.GetInitObjectsAsync(rndProductionOrderViewModel.PlOrderwiseLotInfoList);

        //        return PartialView($"AddLotNoDetailsTable", rndProductionOrderViewModel);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return PartialView($"AddLotNoDetailsTable", rndProductionOrderViewModel);
        //        //throw;
        //    }
        //}

        [AcceptVerbs("Get", "Post")]
        public IActionResult GetPoDetails(string orderNo)
        {
            try
            {
                return Ok(_rndProductionOrder.GetPoDetailsAsync(orderNo));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public double GetPoDetailsForCountBudget(string orderNo, string countId, string warpLength)
        {
            try
            {
                var result = _rndProductionOrder.GetPoDetailsForCountBudgetAsync(orderNo, countId, warpLength);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public RndRsProductionOrderDetailsViewModel GetRsDetails(string orderNo)
        {
            try
            {
                var result = _rndProductionOrder.GetRsDetailsAsync(orderNo);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<RND_MSTR_ROLL> GetMasterRollDetails(string mid)
        {
            try
            {
                var result = await _rndMstrRoll.FindByIdAllAsync(int.Parse(mid));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<TypeTableViewModel>> GetOrderNoData(string orderType)
        {
            try
            {
                var result = await _rndProductionOrder.GetOrderNoDataAsync(orderType);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //[HttpPost]
        //public async Task<bool> AddRndCountName(RndProductionOrderViewModel rndProductionOrderViewModel)
        //{
        //    try
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        var exData = await _rndFabricCountInfo.FindByIdAsync(rndProductionOrderViewModel.UpdateCountInfoViewModel.OLD_COUNTID);

        //        if (exData == null) return false;
        //        var consumptionId = await _rndYarnConsumption.GetPrimaryKeyByCountIdAndFabCodeAsync((int)exData.COUNTID, exData.FABCODE);
        //        //Count Info Data
        //        exData.COUNTID = rndProductionOrderViewModel.UpdateCountInfoViewModel.NEW_COUNTID;
        //        exData.UPDATED_AT = DateTime.Now;
        //        exData.UPDATED_BY = user.Id;
        //        //Consumption Data
        //        var consumptionDetails = await _rndYarnConsumption.FindByIdAsync(consumptionId);

        //        if (consumptionDetails == null)
        //        {
        //            return false;
        //        }
        //        consumptionDetails.COUNTID = rndProductionOrderViewModel.UpdateCountInfoViewModel.NEW_COUNTID;

        //        return await _rndFabricCountInfo.Update(exData) && await _rndYarnConsumption.Update(consumptionDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //}


    }
}