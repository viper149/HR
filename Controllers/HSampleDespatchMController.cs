using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.HDispatch;
using DenimERP.ServiceInterfaces.SampleGarments.HReceive;
using DenimERP.ViewModels.SampleGarments.HDispatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize(Policy = "HOSample")]
    public class HSampleDespatchMController : Controller
    {
        private readonly IH_SAMPLE_DESPATCH_M _hSampleDespatchM;
        private readonly IH_SAMPLE_RECEIVING_D _hSampleReceivingD;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IH_SAMPLE_DESPATCH_D _hSampleDespatchD;
        private readonly IDataProtector _protector;

        public HSampleDespatchMController(IH_SAMPLE_DESPATCH_M hSampleDespatchM,
            IH_SAMPLE_RECEIVING_D hSampleReceivingD,
            UserManager<ApplicationUser> userManager,
            IH_SAMPLE_DESPATCH_D hSampleDespatchD,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _hSampleDespatchM = hSampleDespatchM;
            _hSampleReceivingD = hSampleReceivingD;
            _userManager = userManager;
            _hSampleDespatchD = hSampleDespatchD;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsdId"> Belongs to SDID. Primary key. Must not be null. <see cref="H_SAMPLE_DESPATCH_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailsHSampleDespatchM(string hsdId)
        {
            try
            {
                return View(await _hSampleDespatchM.FindByHsdIdIncludeAllAsync(int.Parse(_protector.Unprotect(hsdId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsdId"> Belongs to SDID. Primary key. Must not be null. <see cref="H_SAMPLE_DESPATCH_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteHSampleDespatchM(string hsdId)
        {
            try
            {
                var createHSampleDespatchMViewModel = await _hSampleDespatchM.FindByHsdIdIncludeAllAsync(int.Parse(_protector.Unprotect(hsdId)));

                if (createHSampleDespatchMViewModel.HSampleDespatchM != null)
                {
                    await _hSampleDespatchD.DeleteRange(createHSampleDespatchMViewModel.HSampleDespatchDs);
                    await _hSampleDespatchM.Delete(createHSampleDespatchMViewModel.HSampleDespatchM);

                    TempData["message"] = "Successfully Deleted HO, Sample Dispatch Information.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Failed To Delete HO, Sample Dispatch Information.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetHSampleDespatchM", $"HSampleDespatchM");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createHSampleDespatchMViewModel"> View model. <see cref="CreateHSampleDespatchMViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditHSampleDespatchM(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var hSampleDespatchM = await _hSampleDespatchM.FindByIdAsync(int.Parse(_protector.Unprotect(createHSampleDespatchMViewModel.HSampleDespatchM.EncryptedId)));

                    if (hSampleDespatchM != null)
                    {
                        hSampleDespatchM.SDDATE = createHSampleDespatchMViewModel.HSampleDespatchM.SDDATE;
                        hSampleDespatchM.GPDATE = createHSampleDespatchMViewModel.HSampleDespatchM.GPDATE;
                        hSampleDespatchM.BRANDID = createHSampleDespatchMViewModel.HSampleDespatchM.BRANDID;
                        hSampleDespatchM.PURPOSE = createHSampleDespatchMViewModel.HSampleDespatchM.PURPOSE;
                        hSampleDespatchM.STATUS = createHSampleDespatchMViewModel.HSampleDespatchM.STATUS;
                        hSampleDespatchM.RTNDATE = createHSampleDespatchMViewModel.HSampleDespatchM.RTNDATE;
                        hSampleDespatchM.HSPID = createHSampleDespatchMViewModel.HSampleDespatchM.HSPID;
                        hSampleDespatchM.THROUGH = createHSampleDespatchMViewModel.HSampleDespatchM.THROUGH;
                        hSampleDespatchM.COST_STATUS = createHSampleDespatchMViewModel.HSampleDespatchM.COST_STATUS;
                        hSampleDespatchM.REMARKS = createHSampleDespatchMViewModel.HSampleDespatchM.REMARKS;
                        hSampleDespatchM.UPDATED_BY = currentUser.Id;
                        hSampleDespatchM.UPDATED_AT = DateTime.Now;

                        if (await _hSampleDespatchM.Update(hSampleDespatchM))
                        {
                            foreach (var item in createHSampleDespatchMViewModel.HSampleDespatchDs.Where(e => e.SDDID > 0))
                            {
                                // FIND THE EXISTING FROM DATABASE
                                var hSampleDespatchD = await _hSampleDespatchD.FindByIdAsync(item.SDDID);

                                hSampleDespatchD.SDID = hSampleDespatchM.SDID;
                                hSampleDespatchD.UPDATED_AT = DateTime.Now;
                                hSampleDespatchD.UPDATED_BY = currentUser.Id;
                                hSampleDespatchD.BARCODE = string.IsNullOrEmpty(hSampleDespatchD.BARCODE) ? await _hSampleDespatchD.GetBarcodeByAsync(item.RCVDID) : hSampleDespatchD.BARCODE;

                                // THEN UPDATE
                                await _hSampleDespatchD.Update(hSampleDespatchD);
                            }

                            foreach (var item in createHSampleDespatchMViewModel.HSampleDespatchDs.Where(e => e.SDDID == 0 || e.SDDID < 0))
                            {
                                item.SDID = hSampleDespatchM.SDID;
                                item.CREATED_BY = item.UPDATED_BY = currentUser.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.BARCODE = await _hSampleDespatchD.GetBarcodeByAsync(item.RCVDID);
                            }

                            await _hSampleDespatchD.InsertRangeByAsync(createHSampleDespatchMViewModel.HSampleDespatchDs.Where(e => e.SDDID == 0 || e.SDDID < 0));

                            TempData["message"] = "Successfully Updated HO, Sample Dispatch Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetHSampleDespatchM", $"HSampleDespatchM");
                        }

                        TempData["message"] = "Failed To Update HO, Sample Dispatch Information.";
                        TempData["type"] = "error";
                        return View(await _hSampleDespatchM.GetInitObjects(createHSampleDespatchMViewModel));
                    }

                    TempData["message"] = "Failed To Update HO, Sample Dispatch Information.";
                    TempData["type"] = "error";
                    return View(await _hSampleDespatchM.GetInitObjects(createHSampleDespatchMViewModel));
                }

                TempData["message"] = "Failed To Update HO, Sample Dispatch Information.";
                TempData["type"] = "error";
                return View(await _hSampleDespatchM.GetInitObjects(createHSampleDespatchMViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsdId"> Belongs to SDID. Primary key. Must not be null. <see cref="H_SAMPLE_DESPATCH_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditHSampleDespatchM(string hsdId)
        {
            try
            {
                return View(await _hSampleDespatchM.GetInitObjects(await _hSampleDespatchM.FindByHsdIdIncludeAllAsync(int.Parse(_protector.Unprotect(hsdId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
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

                var forDataTableByAsync = await _hSampleDespatchM.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        public IActionResult GetHSampleDespatchM()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createHSampleDespatchMViewModel"> View model. <see cref="CreateHSampleDespatchMViewModel"/></param>
        /// <returns> Partial view.</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveDispatchDetails(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel)
        {
            ModelState.Clear();
            if (createHSampleDespatchMViewModel.IsDeletable)
            {
                var hSampleDespatchD = createHSampleDespatchMViewModel.HSampleDespatchDs[createHSampleDespatchMViewModel.RemoveIndex];

                if (hSampleDespatchD.SDDID > 0)
                {
                    await _hSampleDespatchD.Delete(hSampleDespatchD);
                }

                createHSampleDespatchMViewModel.HSampleDespatchDs.RemoveAt(createHSampleDespatchMViewModel.RemoveIndex);
            }
            else
            {
                if (await _hSampleDespatchM.IsAddableToHSampleDispatchAsync(createHSampleDespatchMViewModel))
                {
                    createHSampleDespatchMViewModel.HSampleDespatchD.BARCODE = await _hSampleDespatchD.GetBarcodeByAsync(createHSampleDespatchMViewModel.HSampleDespatchD.RCVDID);
                    createHSampleDespatchMViewModel.HSampleDespatchDs.Add(createHSampleDespatchMViewModel.HSampleDespatchD);
                }
            }

            return PartialView($"AddOrRemoveDispatchDetailsTable", createHSampleDespatchMViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rcvdId"> Belongs to RCVDID. Primary key. Must not be null. <see cref="H_SAMPLE_RECEIVING_D"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetHoSampleReceiveDetails(int rcvdId)
        {
            return Json(await _hSampleReceivingD.GetAvailableQty(rcvdId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createHSampleDespatchMViewModel"> View model. <see cref="CreateHSampleDespatchMViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateHSampleDespatchM(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                    createHSampleDespatchMViewModel.HSampleDespatchM.CREATED_BY =
                        createHSampleDespatchMViewModel.HSampleDespatchM.UPDATED_BY = currentUser.Id;
                    createHSampleDespatchMViewModel.HSampleDespatchM.UPDATED_AT = DateTime.Now;

                    var hSampleDespatchM = await _hSampleDespatchM.GetInsertedObjByAsync(createHSampleDespatchMViewModel.HSampleDespatchM);

                    if (hSampleDespatchM != null)
                    {
                        foreach (var item in createHSampleDespatchMViewModel.HSampleDespatchDs)
                        {
                            item.SDID = hSampleDespatchM.SDID;
                            item.UPDATED_BY = item.CREATED_BY = currentUser.Id;
                            item.UPDATED_AT = DateTime.Now;
                        }

                        await _hSampleDespatchD.InsertRangeByAsync(createHSampleDespatchMViewModel.HSampleDespatchDs);

                        TempData["message"] = "Successfully Added HO, Sample Dispatch Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetHSampleDespatchM", $"HSampleDespatchM");
                    }

                    TempData["message"] = "Failed To Add HO, Sample Dispatch Information.";
                    TempData["type"] = "error";
                    return View(await _hSampleDespatchM.GetInitObjects(createHSampleDespatchMViewModel));
                }

                TempData["message"] = "Failed To Add HO, Sample Dispatch Information.";
                TempData["type"] = "error";
                return View(await _hSampleDespatchM.GetInitObjects(createHSampleDespatchMViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateHSampleDespatchM()
        {
            try
            {
                var createHSampleDespatchMViewModel = await _hSampleDespatchM.GetInitObjects(new CreateHSampleDespatchMViewModel());
                createHSampleDespatchMViewModel.HSampleDespatchM.SDDATE = createHSampleDespatchMViewModel.HSampleDespatchM.GPDATE = DateTime.Now;
                createHSampleDespatchMViewModel.HSampleDespatchM.GPNO = await _hSampleDespatchM.GetGatePassNumber();

                return View(createHSampleDespatchMViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
    }
}