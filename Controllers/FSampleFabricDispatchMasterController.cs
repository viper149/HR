using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels.SampleGarments.Fabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Route("SampleFabricDispatch")]
    public class FSampleFabricDispatchMasterController : Controller
    {
        private readonly IF_SAMPLE_FABRIC_DISPATCH_MASTER _fSampleFabricDispatchMaster;
        private readonly IF_SAMPLE_FABRIC_DISPATCH_DETAILS _fSampleFabricDispatchDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_SAMPLE_FABRIC_DISPATCH_TRANSACTION _fSampleFabricDispatchTransaction;
        private readonly IF_SAMPLE_FABRIC_RCV_D _fSampleFabricRcvD;
        private readonly IDataProtector _protector;

        public FSampleFabricDispatchMasterController(IF_SAMPLE_FABRIC_DISPATCH_MASTER fSampleFabricDispatchMaster,
            IF_SAMPLE_FABRIC_DISPATCH_DETAILS fSampleFabricDispatchDetails,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_SAMPLE_FABRIC_DISPATCH_TRANSACTION fSampleFabricDispatchTransaction,
            IF_SAMPLE_FABRIC_RCV_D fSampleFabricRcvD)
        {
            _fSampleFabricDispatchMaster = fSampleFabricDispatchMaster;
            _fSampleFabricDispatchDetails = fSampleFabricDispatchDetails;
            _userManager = userManager;
            _fSampleFabricDispatchTransaction = fSampleFabricDispatchTransaction;
            _fSampleFabricRcvD = fSampleFabricRcvD;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("IsValidForUse")]
        public async Task<IActionResult> IsValidForUse(F_SAMPLE_FABRIC_DISPATCH_DETAILS fSampleFabricDispatchDetails)
        {
            return fSampleFabricDispatchDetails.REQ_QTY >= fSampleFabricDispatchDetails.DEL_QTY
                ? Json(true)
                : Json($"Delivery Qty [{fSampleFabricDispatchDetails.DEL_QTY}] must less than or equal to Requested Qty [{fSampleFabricDispatchDetails.REQ_QTY}]");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetGatePassFor/{search?}/{page?}")]
        public async Task<IActionResult> GetGatePassFor(string search, int page)
        {
            return Ok(await _fSampleFabricDispatchMaster.GetGatePassFor(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetDriverInfo/{search?}/{page?}")]
        public async Task<IActionResult> GetDriverInfo(string search, int page)
        {
            return Ok(await _fSampleFabricDispatchMaster.GetDriverInfo(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetVehicleInfo/{search?}/{page?}")]
        public async Task<IActionResult> GetVehicleInfo(string search, int page)
        {
            return Ok(await _fSampleFabricDispatchMaster.GetVehicleInfo(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetGatePassType/{search?}/{page?}")]
        public async Task<IActionResult> GetGatePassType(string search, int page)
        {
            return Ok(await _fSampleFabricDispatchMaster.GetGatePassType(search, page));
        }

        [AcceptVerbs("Post")]
        [Route("GetQty")]
        public async Task<IActionResult> GetQty(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel)
        {
            return Ok(await _fSampleFabricDispatchMaster.GetQtyByAsync(createFSampleFabricDispatchMasterViewModel));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("Delete/{dpId}")]
        public async Task<IActionResult> DeleteFSampleFabricDispatchMaster(string dpId)
        {
            var fabricDispatch = await _fSampleFabricDispatchMaster.FindByIdForDeleteAsync(int.Parse(_protector.Unprotect(dpId)));
            if (fabricDispatch != null)
            {
                if (fabricDispatch.F_SAMPLE_FABRIC_DISPATCH_DETAILS.Any())
                {
                    foreach (var item in fabricDispatch.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    {
                        if(item.F_SAMPLE_FABRIC_DISPATCH_TRANSACTION.Any())
                        {
                            await _fSampleFabricDispatchTransaction.DeleteRange(item.F_SAMPLE_FABRIC_DISPATCH_TRANSACTION);
                        }
                    }

                    await _fSampleFabricDispatchDetails.DeleteRange(fabricDispatch.F_SAMPLE_FABRIC_DISPATCH_DETAILS);
                }

                await _fSampleFabricDispatchMaster.Delete(fabricDispatch);
            }
            TempData["message"] = $"Successfully Deleted";
            TempData["type"] = "success";
            return RedirectToAction(nameof(GetFSampleFabricDispatchMaster), $"FSampleFabricDispatchMaster");
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFSampleFabricDispatchMasterViewModel(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel)
        {
            switch (ModelState.IsValid)
            {
                case true:
                {
                    var fSampleFabricDispatchMaster = await _fSampleFabricDispatchMaster.FindByIdAsync(int.Parse(
                        _protector.Unprotect(createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster
                            .EncryptedId)));

                    if (fSampleFabricDispatchMaster != null)
                    {
                        createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster.DPID = fSampleFabricDispatchMaster.DPID;
                        createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster.GPNO = fSampleFabricDispatchMaster.GPNO;

                        if (await _fSampleFabricDispatchMaster.Update(createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster))
                        {
                            var fSampleFabricDispatchDetailses = createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses
                                .Where(e => e.TRNSID <= 0).ToList();
                            var sampleFabricDispatchDetailses = createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses
                                .Where(e => e.TRNSID > 0).ToList();

                            foreach (var item in createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses)
                            {
                                item.DPID = fSampleFabricDispatchMaster.DPID;
                            }

                            await _fSampleFabricDispatchDetails.InsertRangeByAsync(fSampleFabricDispatchDetailses);
                            await _fSampleFabricDispatchDetails.UpdateRangeByAsync(sampleFabricDispatchDetailses);

                            return RedirectToAction(nameof(GetFSampleFabricDispatchMaster), $"FSampleFabricDispatchMaster");
                        }
                    }

                    break;
                }
            }

            return View(nameof(EditFSampleFabricDispatchMaster), await _fSampleFabricDispatchMaster.GetInitObjectsByAsync(createFSampleFabricDispatchMasterViewModel));
        }

        [HttpGet]
        [Route("Edit/{dpId?}")]
        public async Task<IActionResult> EditFSampleFabricDispatchMaster(string dpId)
        {
            return View(await _fSampleFabricDispatchMaster.GetInitObjectsByAsync(await _fSampleFabricDispatchMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(dpId)))));
        }

        [HttpGet]
        [Route("Details/{dpId}")]
        public async Task<IActionResult> DetailsFSampleFabricDispatchMaster(string dpId)
        {
            return View(await _fSampleFabricDispatchMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(dpId))));
        }

        [HttpGet]
        [Route("BayDispatchReport/{dpId?}")]
        public async Task<IActionResult> RSampleFabricBayDispatch(string dpId)
        {
            return View(model: int.Parse(_protector.Unprotect(dpId)));
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _fSampleFabricDispatchMaster.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFSampleFabricDispatchMaster(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    {
                        var applicationUser = await _userManager.GetUserAsync(User);

                        createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster.CREATED_BY = createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster.UPDATED_BY = applicationUser.Id;
                        createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster.CREATED_AT = createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster.UPDATED_AT = DateTime.Now;

                        var fSampleFabricDispatchMaster = await _fSampleFabricDispatchMaster.GetInsertedObjByAsync(createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster);

                        switch (fSampleFabricDispatchMaster.DPID)
                        {
                            case > 0:
                                {
                                    foreach (var item in createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses)
                                    {
                                        item.DPID = fSampleFabricDispatchMaster.DPID;

                                        var fSampleFabricDispatchDetails = await _fSampleFabricDispatchDetails.GetInsertedObjByAsync(item);
                                        var openingBalance = await _fSampleFabricDispatchTransaction.FindByDpIdAsync(item.DPDID, item.TRNSID ?? -1);

                                        var fSampleFabricDispatchTransaction = new F_SAMPLE_FABRIC_DISPATCH_TRANSACTION
                                        {
                                            DPDID = fSampleFabricDispatchDetails.DPDID,
                                            REQ_QTY = item.REQ_QTY,
                                            DEL_QTY = item.DEL_QTY,
                                            OP_BALANCE = openingBalance,
                                            BALANCE = openingBalance - item.DEL_QTY
                                        };

                                        if (!await _fSampleFabricDispatchTransaction.InsertByAsync(fSampleFabricDispatchTransaction))
                                        {
                                            await _fSampleFabricDispatchDetails.Delete(fSampleFabricDispatchDetails);
                                        }
                                    }

                                    break;
                                }
                        }

                        break;
                    }
            }
            TempData["message"] = $"Successfully Created";
            TempData["type"] = "success";
            return RedirectToAction(nameof(GetFSampleFabricDispatchMaster), $"FSampleFabricDispatchMaster");
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFSampleFabricDispatchMaster()
        {
            var createFSampleFabricDispatchMasterViewModel = new CreateFSampleFabricDispatchMasterViewModel
            {
                FSampleFabricDispatchMaster = new F_SAMPLE_FABRIC_DISPATCH_MASTER
                {
                    GPNO = await _fSampleFabricDispatchMaster.GetGetPassNo(),
                    GPDATE = DateTime.Now
                }
            };

            return View(await _fSampleFabricDispatchMaster.GetInitObjectsByAsync(createFSampleFabricDispatchMasterViewModel));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetFSampleFabricDispatchMaster()
        {
            return View();
        }

        [AcceptVerbs("Post")]
        [Route("AddOrRemoveDetailsList")]
        public async Task<IActionResult> AddOrRemoveDetailsList(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (createFSampleFabricDispatchMasterViewModel.IsDeletable)
                {
                    var fSampleFabricDispatchDetails = createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses[createFSampleFabricDispatchMasterViewModel.RemoveIndex];

                    if (fSampleFabricDispatchDetails.DPDID > 0)
                    {
                        await _fSampleFabricDispatchDetails.Delete(fSampleFabricDispatchDetails);
                    }

                    createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses.RemoveAt(createFSampleFabricDispatchMasterViewModel.RemoveIndex);
                }
                else
                {
                    //if (!createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses.Any(e =>
                    //    e.TRNSID.Equals(createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetails.TRNSID)))
                    //{
                    if (TryValidateModel(createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetails)
                    && createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetails.REQ_QTY >= createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetails.DEL_QTY)
                    {
                        createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses.Add(createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetails);
                    }
                    //}
                }

                return PartialView($"CreateFSampleFabricDispatchMasterTable", await _fSampleFabricDispatchMaster.GetInitObjectsForDetailsTableByAsync(createFSampleFabricDispatchMasterViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
