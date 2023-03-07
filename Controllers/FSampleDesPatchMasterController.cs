using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using DenimERP.ViewModels.SampleGarments.GatePass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize(Policy = "FSample")]
    public class FSampleDesPatchMasterController : Controller
    {
        private readonly IF_SAMPLE_DESPATCH_MASTER _fSampleDespatchMaster;
        private readonly IF_SAMPLE_DESPATCH_DETAILS _fSampleDespatchDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_SAMPLE_GARMENT_RCV_D _fSampleGarmentRcvD;
        private readonly IDataProtector _protector;

        public FSampleDesPatchMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_SAMPLE_DESPATCH_MASTER fSampleDespatchMaster,
            IF_SAMPLE_DESPATCH_DETAILS fSampleDespatchDetails,
            UserManager<ApplicationUser> userManager,
            IF_SAMPLE_GARMENT_RCV_D fSampleGarmentRcvD)
        {
            _fSampleDespatchMaster = fSampleDespatchMaster;
            _fSampleDespatchDetails = fSampleDespatchDetails;
            _userManager = userManager;
            _fSampleGarmentRcvD = fSampleGarmentRcvD;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createFSampleDesPatchMasterViewModel"> View Model. <see cref="CreateFSampleDesPatchMasterViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetObjectsByBarcode(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel)
        {
            try
            {
                return Json(await _fSampleDespatchMaster.FindByBarcodeAsync(createFSampleDesPatchMasterViewModel.FSampleDespatchDetails.TRNS.BARCODE));
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trnsId"> Belongs to TRNSID. Primary key. Must not be null. <see cref="F_SAMPLE_GARMENT_RCV_D"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetNumberOfTotalItems(int trnsId)
        {
            return PartialView("GetNumberOfTotalItemsTable", await _fSampleDespatchMaster.GetNumberOfTotalItems(trnsId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchId"> Belongs to DPID. Primary key. Must not be null. <see cref="F_SAMPLE_DESPATCH_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteFSampleDesPatchMaster(string dispatchId)
        {
            var fSampleDespatchMaster = await _fSampleDespatchMaster.FindByIdAsync(int.Parse(_protector.Unprotect(dispatchId)));

            if (fSampleDespatchMaster != null)
            {
                await _fSampleDespatchDetails.DeleteRange(await _fSampleDespatchDetails.FindByDispatchIdAsync(fSampleDespatchMaster.DPID));
                await _fSampleDespatchMaster.Delete(fSampleDespatchMaster);

                TempData["message"] = "Successfully Deleted Sample Garments Gate Pass Information.";
                TempData["type"] = "success";
            }
            else
            {
                TempData["message"] = "Not Found, Sample Garments Gate Pass Information.";
                TempData["type"] = "error";
            }

            return RedirectToAction("GetFSampleDesPatchMaster", "FSampleDesPatchMaster"); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchId"> Belongs to DPID. Primary key. Must not be null. <see cref="F_SAMPLE_DESPATCH_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailsFSampleDesPatchMaster(string dispatchId)
        {
            try
            {
                return View(await _fSampleDespatchMaster.FindByDispatchIdIncludeAllAsync(int.Parse(_protector.Unprotect(dispatchId))));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> SampleGatePassReport(string dpid)
        //{
        //    return View(model: int.Parse(_protector.Unprotect(dpId)));
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createFSampleDesPatchMasterViewModel"> View model. <see cref="CreateFSampleDesPatchMasterViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditFSampleDesPatchMaster(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return View(await _fSampleDespatchMaster.GetInitObjects(createFSampleDesPatchMasterViewModel));

                var currentUser = await _userManager.GetUserAsync(User);
                var fSampleDespatchMaster = await _fSampleDespatchMaster.FindByIdAsync(int.Parse(_protector.Unprotect(createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.EncryptedId)));

                if (fSampleDespatchMaster.CREATED_AT < DateTime.Now.AddDays(-2))
                {
                    TempData["message"] = "Failed To Update Sample Garments Gate Pass Information.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetFSampleDesPatchMaster", "FSampleDesPatchMaster");
                }

                fSampleDespatchMaster.GPDATE = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.GPDATE;
                fSampleDespatchMaster.GPTYPEID = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.GPTYPEID;
                fSampleDespatchMaster.DRID = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.DRID;
                fSampleDespatchMaster.VID = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.VID;
                fSampleDespatchMaster.REMARKS = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.REMARKS;
                fSampleDespatchMaster.TYPEID = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.TYPEID;
                fSampleDespatchMaster.UPDATED_AT = DateTime.Now;
                fSampleDespatchMaster.UPDATED_BY = currentUser.Id;

                var update = await _fSampleDespatchMaster.Update(fSampleDespatchMaster);

                if (update)
                {
                    foreach (var item in createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses)
                    {
                        item.DPID = fSampleDespatchMaster.DPID;

                        if (item.DPDID > 0)
                        {
                            var fSampleDespatchDetails = await _fSampleDespatchDetails.FindByIdAsync(item.DPDID);

                            fSampleDespatchDetails.UPDATED_BY = currentUser.Id;
                            fSampleDespatchDetails.UPDATED_AT = DateTime.Now;

                            await _fSampleDespatchDetails.Update(fSampleDespatchDetails);
                        }
                        else
                        {
                            item.CREATED_BY = item.UPDATED_BY = currentUser.Id;
                            item.UPDATED_AT = item.CREATED_AT = DateTime.Now;
                            await _fSampleDespatchDetails.InsertByAsync(item);
                        }
                    }

                    TempData["message"] = "Successfully Updated Sample Garments Gate Pass Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetFSampleDesPatchMaster", "FSampleDesPatchMaster");
                }

                TempData["message"] = "Failed To Update Sample Garments Gate Pass Information.";
                TempData["type"] = "error";
                return RedirectToAction("EditFSampleDesPatchMaster", "FSampleDesPatchMaster");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchId"> Belongs to DPID. Primary key. Must not be null. <see cref="F_SAMPLE_DESPATCH_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditFSampleDesPatchMaster(string dispatchId)
        {
            try
            {
                var createFSampleDesPatchMasterViewModel = await _fSampleDespatchMaster.FindByDispatchIdIncludeAllAsync(int.Parse(_protector.Unprotect(dispatchId)));

                if (createFSampleDesPatchMasterViewModel.IsLocked)
                {
                    TempData["message"] = "This item has been locked.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetFSampleDesPatchMaster", "FSampleDesPatchMaster");
                }

                return View(await _fSampleDespatchMaster.GetInitObjects(createFSampleDesPatchMasterViewModel));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }



        [HttpGet]
        //[Route("GatePassReport/{dpId?}")]
        public async Task<IActionResult> SampleGatePassReport(string dpId)
        {
            return View(model: int.Parse(_protector.Unprotect(dpId)));
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

                var forDataTableByAsync = await _fSampleDespatchMaster.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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

        [HttpGet]
        public IActionResult GetFSampleDesPatchMaster()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createFSampleDesPatchMasterViewModel"> View model. <see cref="CreateFSampleDesPatchMasterViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateFSampleDesPatchMaster(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.UPDATED_AT = DateTime.Now;
                    createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.CREATED_BY = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.UPDATED_BY = currentUser.Id;
                    var fSampleDespatchMaster = await _fSampleDespatchMaster.GetInsertedObjByAsync(createFSampleDesPatchMasterViewModel.FSampleDespatchMaster);

                    if (fSampleDespatchMaster != null)
                    {
                        foreach (var item in createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses)
                        {
                            item.CREATED_BY = item.UPDATED_BY = currentUser.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.DPID = fSampleDespatchMaster.DPID;
                        }

                        await _fSampleDespatchDetails.InsertRangeByAsync(createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses);
                    }

                    TempData["message"] = "Successfully Added Sample Garments Gate Pass Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetFSampleDesPatchMaster", "FSampleDesPatchMaster");
                }

                TempData["message"] = "Failed To Add Sample Garments Gate Pass Information.";
                TempData["type"] = "error";
                return View(await _fSampleDespatchMaster.GetInitObjects(createFSampleDesPatchMasterViewModel));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createFSampleDesPatchMasterViewModel"> View model. <see cref="CreateFSampleDesPatchMasterViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddOrDeleteFSampleDesPatchMasterDetailsTable(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (createFSampleDesPatchMasterViewModel.IsDeletable)
                {
                    var fSampleDespatchDetails = createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses[createFSampleDesPatchMasterViewModel.RemoveIndex];

                    if (fSampleDespatchDetails.DPDID > 0)
                    {
                        await _fSampleDespatchDetails.Delete(fSampleDespatchDetails);
                    }

                    createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses.RemoveAt(createFSampleDesPatchMasterViewModel.RemoveIndex);
                }
                else
                {
                    var fSampleGarmentRcvDs = await _fSampleGarmentRcvD.All();

                    if (fSampleGarmentRcvDs.Where(e => e.TRNSID.Equals(createFSampleDesPatchMasterViewModel.FSampleDespatchDetails.TRNSID))
                        .Any(f => f.QTY
                            - createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses.Where(g => g.TRNSID.Equals(createFSampleDesPatchMasterViewModel.FSampleDespatchDetails.TRNSID)).Sum(h => h.DEL_QTY)
                            - createFSampleDesPatchMasterViewModel.FSampleDespatchDetails.DEL_QTY >= 0))
                    {
                        if (!createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses.Any(e =>
                            e.TRNSID.Equals(createFSampleDesPatchMasterViewModel.FSampleDespatchDetails.TRNSID) &&
                            e.ISSUE_PERSON.Equals(createFSampleDesPatchMasterViewModel.FSampleDespatchDetails.ISSUE_PERSON)))
                        {
                            createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses.Add(createFSampleDesPatchMasterViewModel.FSampleDespatchDetails);
                        }
                    }
                }

                return PartialView("AddOrDeleteFSampleDesPatchMasterDetailsTable", await _fSampleDespatchMaster.GetInitObjectsOfSelectedItems(createFSampleDesPatchMasterViewModel));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateFSampleDesPatchMaster()
        {
            try
            {
                var createFSampleDesPatchMasterViewModel = await _fSampleDespatchMaster.GetInitObjects(new CreateFSampleDesPatchMasterViewModel());
                createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.GPDATE = DateTime.Now;
                createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.GPNO = await _fSampleDespatchMaster.GetGatePassNumber();
                return View(createFSampleDesPatchMasterViewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}