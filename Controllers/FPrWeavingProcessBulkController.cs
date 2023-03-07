using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FPrWeavingProcessBulkController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_WEAVING_PROCESS_MASTER_B _fPrWeavingProcessMasterB;
        private readonly IF_PR_WEAVING_PROCESS_DETAILS_B _fPrWeavingProcessDetailsB;
        private readonly IF_PR_WEAVING_PROCESS_BEAM_DETAILS_B _fPrWeavingProcessBeamDetailsB;
        private readonly IF_PR_SIZING_PROCESS_ROPE_DETAILS _fPrSizingProcessRopeDetails;
        private readonly IF_PR_SLASHER_DYEING_DETAILS _fPrSlasherDyeingDetails;
        private readonly IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS _fPrWeavingWeftYarnConsumDetails;

        public FPrWeavingProcessBulkController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_WEAVING_PROCESS_MASTER_B fPrWeavingProcessMasterB,
            IF_PR_WEAVING_PROCESS_DETAILS_B fPrWeavingProcessDetailsB,
            IF_PR_WEAVING_PROCESS_BEAM_DETAILS_B fPrWeavingProcessBeamDetailsB,
            IF_PR_SIZING_PROCESS_ROPE_DETAILS fPrSizingProcessRopeDetails,
            IF_PR_SLASHER_DYEING_DETAILS fPrSlasherDyeingDetails,
            IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS fPrWeavingWeftYarnConsumDetails
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrWeavingProcessMasterB = fPrWeavingProcessMasterB;
            _fPrWeavingProcessDetailsB = fPrWeavingProcessDetailsB;
            _fPrWeavingProcessBeamDetailsB = fPrWeavingProcessBeamDetailsB;
            _fPrSizingProcessRopeDetails = fPrSizingProcessRopeDetails;
            _fPrSlasherDyeingDetails = fPrSlasherDyeingDetails;
            _fPrWeavingWeftYarnConsumDetails = fPrWeavingWeftYarnConsumDetails;
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var recordsTotal = 0;

            var data = await _fPrWeavingProcessMasterB.GetAllAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data
                    .Where(m => m.WV_PROCESSID.ToString().Contains(searchValue)
                                || m.SET.PROG_.PROG_NO != null && m.SET.PROG_.PROG_NO.ToString().Contains(searchValue)
                                || m.WV_PROCESS_DATE != null && m.WV_PROCESS_DATE.ToString().Contains(searchValue)
                                || m.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO != null && m.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO.ToUpper().Contains(searchValue)
                                || m.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.STYLENAME !=null && m.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.STYLENAME.ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)
                                ).ToList();
            }

           
            var cosStandardConses = data.ToList();
            recordsTotal = cosStandardConses.Count();
            var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
            return Json(jsonData);
        }

        [HttpGet]
        public IActionResult GetWeavingProcessBulkList()
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

        [HttpGet]
        public async Task<IActionResult> CreateWeavingProcessBulk()
        {
            try
            {
                var prWeavingProcessBulkViewModel = await GetInfo(new PrWeavingProcessBulkViewModel());
                prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB = new F_PR_WEAVING_PROCESS_MASTER_B()
                {
                    WV_PROCESS_DATE = DateTime.Now
                };
                return View(prWeavingProcessBulkViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeavingProcessBulk(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.CREATED_BY = user.Id;
                    prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.UPDATED_BY = user.Id;
                    prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.CREATED_AT = DateTime.Now;
                    prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.UPDATED_AT = DateTime.Now;
                    var processId = await _fPrWeavingProcessMasterB.InsertAndGetIdAsync(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB);

                    if (processId != 0)
                    {
                        foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList)
                        {
                            var wvBeamId = 0;
                            if (item.WV_BEAMID == 0)
                            {
                                item.WV_PROCESSID = processId;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                wvBeamId = await _fPrWeavingProcessBeamDetailsB.InsertAndGetIdAsync(item);
                            }
                            else
                            {
                                wvBeamId = item.WV_BEAMID;
                            }
                            foreach (var i in item.FPrWeavingProcessDetailsBList.Where(c => c.TRNSID == 0))
                            {
                                i.WV_BEAMID = wvBeamId;
                                i.CREATED_AT = DateTime.Now;
                                i.CREATED_BY = user.Id;
                                i.UPDATED_AT = DateTime.Now;
                                i.UPDATED_BY = user.Id;
                                await _fPrWeavingProcessDetailsB.InsertByAsync(i);
                            }
                        }
                        foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList.Where(c => c.TRNSID == 0))
                        {
                            item.WV_PROCESSID = processId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            item.WASTE ??= "0";
                            item.CONSUMP = ((item.YARN_RECEIVE ?? 0) + (item.YARN_RETURN ?? 0) + float.Parse(item.WASTE)).ToString();
                            item.WASTE_PERCENTAGE = item.YARN_RECEIVE != null ? Math.Round((double)((float.Parse(item.WASTE) * 100) / item.YARN_RECEIVE), 3) : 0;

                            await _fPrWeavingWeftYarnConsumDetails.InsertByAsync(item);
                        }
                        TempData["message"] = "Successfully Weaving Process Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetWeavingProcessBulkList", $"FPrWeavingProcessBulk");
                    }

                    TempData["message"] = "Failed to Create Weaving Process.";
                    TempData["type"] = "error";
                    return View(await GetInfo(prWeavingProcessBulkViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(prWeavingProcessBulkViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Weaving Process.";
                TempData["type"] = "error";
                return View(await GetInfo(prWeavingProcessBulkViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditWeavingProcessBulk(string id)
        {
            try
            {
                var sId = int.Parse(_protector.Unprotect(id));
                var prWeavingProcessBulkViewModel = await _fPrWeavingProcessMasterB.FindAllByIdAsync(sId);

                if (prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB != null)
                {
                    prWeavingProcessBulkViewModel = await GetInfo(prWeavingProcessBulkViewModel);
                    prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.EncryptedId = _protector.Protect(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.WV_PROCESSID.ToString());
                    return User.IsInRole("Clearance") ? View($"EditWeavingProcessBulkClearance", prWeavingProcessBulkViewModel) : View(prWeavingProcessBulkViewModel);
                }

                TempData["message"] = "Weaving Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetWeavingProcessBulkList), $"FPrWeavingProcessBulk");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWeavingProcessBulk(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wvId = int.Parse(_protector.Unprotect(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.EncryptedId));
                    if (prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.WV_PROCESSID == wvId)
                    {

                        var user = await _userManager.GetUserAsync(User);

                        //if (User.IsInRole("Clearance"))
                        //{

                        foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Where(c => !c.WV_BEAMID.Equals(0)))
                        {
                            var beamDetails = await _fPrWeavingProcessBeamDetailsB.FindByIdAsync(item.WV_BEAMID);

                            if (beamDetails != null)
                            {
                                foreach (var i in item.FPrWeavingProcessDetailsBList.Where(c => !c.TRNSID.Equals(0)))
                                {
                                    var doff = await _fPrWeavingProcessDetailsB.FindByIdAsync(i.TRNSID);
                                    i.WV_BEAMID = item.WV_BEAMID;
                                    i.CREATED_AT = doff.CREATED_AT;
                                    i.CREATED_BY = doff.CREATED_BY;
                                    i.UPDATED_AT = DateTime.Now;
                                    i.UPDATED_BY = user.Id;
                                    await _fPrWeavingProcessDetailsB.Update(i);
                                }
                            }

                        }
                        //}

                        var weavingDetails = await _fPrWeavingProcessMasterB.FindByIdAsync(wvId);

                        prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.UPDATED_BY = user.Id;
                        prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.UPDATED_AT = DateTime.Now;
                        prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.CREATED_AT = weavingDetails.CREATED_AT;
                        prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.CREATED_BY = weavingDetails.CREATED_BY;

                        var isUpdated = await _fPrWeavingProcessMasterB.Update(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB);
                        if (isUpdated)
                        {
                            if (weavingDetails.WV_PROCESSID != 0)
                            {
                                prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);

                                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Where(c => !c.WV_BEAMID.Equals(0)))
                                {
                                    var beamDetails = await _fPrWeavingProcessBeamDetailsB.FindByIdAsync(item.WV_BEAMID);

                                    beamDetails.UPDATED_AT = DateTime.Now;
                                    beamDetails.UPDATED_BY = user.Id;
                                    beamDetails.CRIMP = item.CRIMP;
                                    beamDetails.STATUS = item.STATUS;
                                    var isUpdate = await _fPrWeavingProcessBeamDetailsB.Update(beamDetails);

                                    if (isUpdate)
                                    {
                                        foreach (var i in item.FPrWeavingProcessDetailsBList.Where(c => c.TRNSID.Equals(0)))
                                        {
                                            i.WV_BEAMID = item.WV_BEAMID;
                                            i.CREATED_AT = DateTime.Now;
                                            i.CREATED_BY = user.Id;
                                            i.UPDATED_AT = DateTime.Now;
                                            i.UPDATED_BY = user.Id;
                                            await _fPrWeavingProcessDetailsB.InsertByAsync(i);
                                        }
                                    }

                                }

                                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Where(c => c.WV_BEAMID.Equals(0)))
                                {
                                    item.WV_PROCESSID = weavingDetails.WV_PROCESSID;
                                    item.CREATED_AT = DateTime.Now;
                                    item.CREATED_BY = user.Id;
                                    item.UPDATED_AT = DateTime.Now;
                                    item.UPDATED_BY = user.Id;
                                    var wvBeamId = await _fPrWeavingProcessBeamDetailsB.InsertAndGetIdAsync(item);
                                    foreach (var i in item.FPrWeavingProcessDetailsBList.Where(c => c.TRNSID.Equals(0)))
                                    {
                                        i.WV_BEAMID = wvBeamId;
                                        i.CREATED_AT = DateTime.Now;
                                        i.CREATED_BY = user.Id;
                                        i.UPDATED_AT = DateTime.Now;
                                        i.UPDATED_BY = user.Id;
                                        await _fPrWeavingProcessDetailsB.InsertByAsync(i);
                                    }
                                }


                                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList.Where(c => c.TRNSID == 0))
                                {
                                    item.WV_PROCESSID = weavingDetails.WV_PROCESSID;
                                    item.CREATED_AT = DateTime.Now;
                                    item.CREATED_BY = user.Id;
                                    item.UPDATED_AT = DateTime.Now;
                                    item.UPDATED_BY = user.Id;

                                    item.CONSUMP = ((item.YARN_RECEIVE ?? 0) + (item.YARN_RETURN ?? 0) + float.Parse(item.WASTE ?? "0")).ToString();
                                    item.WASTE_PERCENTAGE = item.YARN_RECEIVE != null ? Math.Round((double)((float.Parse(item.WASTE ?? "0") * 100) / item.YARN_RECEIVE), 3) : 0;

                                    await _fPrWeavingWeftYarnConsumDetails.InsertByAsync(item);
                                }

                                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList.Where(c => c.TRNSID > 0))
                                {
                                    var consumDetails = await _fPrWeavingWeftYarnConsumDetails.FindByIdAsync(item.TRNSID);

                                    item.WV_PROCESSID = weavingDetails.WV_PROCESSID;
                                    item.CREATED_AT = consumDetails.CREATED_AT;
                                    item.CREATED_BY = consumDetails.CREATED_BY;
                                    item.UPDATED_AT = DateTime.Now;
                                    item.UPDATED_BY = user.Id;

                                    item.CONSUMP = ((item.YARN_RECEIVE ?? 0) + (item.YARN_RETURN ?? 0) + float.Parse(item.WASTE ?? "0")).ToString();
                                    item.WASTE_PERCENTAGE = item.YARN_RECEIVE != null ? Math.Round((double)((float.Parse(item.WASTE ?? "0") * 100) / item.YARN_RECEIVE), 3) : 0;

                                    await _fPrWeavingWeftYarnConsumDetails.Update(item);
                                }

                                TempData["message"] = "Successfully Updated Weaving Details.";
                                TempData["type"] = "success";
                                return RedirectToAction("GetWeavingProcessBulkList", $"FPrWeavingProcessBulk");
                            }
                        }
                        TempData["message"] = "Failed to Update Weaving Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetWeavingProcessBulkList", $"FPrWeavingProcessBulk");
                    }
                    TempData["message"] = "Invalid Weaving Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetWeavingProcessBulkList", $"FPrWeavingProcessBulk");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                prWeavingProcessBulkViewModel = new PrWeavingProcessBulkViewModel
                {
                    FPrWeavingProcessMasterB = await _fPrWeavingProcessMasterB.FindByIdAsync(int.Parse(_protector.Unprotect(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.EncryptedId)))
                };
                prWeavingProcessBulkViewModel = await GetInfo(prWeavingProcessBulkViewModel);
                return View(prWeavingProcessBulkViewModel);
            }
            catch (Exception e)
            {
                TempData["message"] = "Failed to Update Weaving Details.";
                TempData["type"] = "error";
                prWeavingProcessBulkViewModel = new PrWeavingProcessBulkViewModel
                {
                    FPrWeavingProcessMasterB = await _fPrWeavingProcessMasterB.FindByIdAsync(int.Parse(_protector.Unprotect(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.EncryptedId)))
                };
                prWeavingProcessBulkViewModel = await GetInfo(prWeavingProcessBulkViewModel);
                return View(prWeavingProcessBulkViewModel);
            }
        }

        public async Task<PrWeavingProcessBulkViewModel> GetInfo(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            prWeavingProcessBulkViewModel = await _fPrWeavingProcessMasterB.GetInitObjects(prWeavingProcessBulkViewModel);
            return prWeavingProcessBulkViewModel;
        }

        [HttpGet]
        public async Task<IEnumerable<F_LOOM_MACHINE_NO>> GetLoomMachine(string loomid)
        {
            var result = await _fPrWeavingProcessBeamDetailsB.GetLoomMachines(int.Parse(loomid));
            return result.ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBeamList(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                if (prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.BEAMID == null)
                {

                    var resultS = prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Any(c =>
                        c.SBEAMID == prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.SBEAMID);
                    if (resultS)
                    {
                        var item = prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.FirstOrDefault(c => c.SBEAMID == prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.SBEAMID);

                        var flag = item.FPrWeavingProcessDetailsBList.Any(c => c.LOOM_NO.Equals(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB.LOOM_NO) && c.DOFF_TIME.Equals(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB.DOFF_TIME));

                        Response.Headers["Status"] = "Failed";
                        if (!flag)
                        {
                            item.FPrWeavingProcessDetailsBList.Add(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB);
                            Response.Headers["Status"] = "Success";
                        }

                        prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                        prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                        return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
                    }
                    prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.FPrWeavingProcessDetailsBList.Add(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB);
                    prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Add(prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB);
                    Response.Headers["Status"] = "Success";

                    prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                    prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                    return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
                }
                else
                {

                    var result = prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Any(c =>
                        c.BEAMID == prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.BEAMID);
                    if (result)
                    {
                        var item = prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.FirstOrDefault(c => c.BEAMID == prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.BEAMID);

                        var flag = item.FPrWeavingProcessDetailsBList.Any(c => c.LOOM_NO.Equals(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB.LOOM_NO) && c.DOFF_TIME.Equals(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB.DOFF_TIME));

                        Response.Headers["Status"] = "Failed";
                        if (!flag)
                        {
                            item.FPrWeavingProcessDetailsBList.Add(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB);
                            Response.Headers["Status"] = "Success";
                        }

                        prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                        prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                        return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
                    }
                    prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.FPrWeavingProcessDetailsBList.Add(prWeavingProcessBulkViewModel.FPrWeavingProcessDetailsB);
                    prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.Add(prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB);
                    Response.Headers["Status"] = "Success";

                    prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                    prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                    return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

                prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                Response.Headers["Status"] = "Failed";
                return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBeamFromList(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.RemoveAt(int.Parse(removeIndexValue));

            prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
            prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
            return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLoomFromBeamList(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel, string removeIndexValue, string setRemoveIndex)
        {
            ModelState.Clear();
            prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList[int.Parse(removeIndexValue)]
                .FPrWeavingProcessDetailsBList.RemoveAt(int.Parse(setRemoveIndex));

            prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
            prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
            return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
        }

        private static PrWeavingProcessBulkViewModel CalculateCrimp(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList)
                {
                    double? productionLength = 0;
                    foreach (var i in item.FPrWeavingProcessDetailsBList)
                    {
                        productionLength += i.LENGTH_BULK;
                    }
                    item.CRIMP = ((item.BEAM_LENGTH - productionLength) / productionLength) * 100;
                    if (item.CRIMP != null) item.CRIMP = Math.Round((double)item.CRIMP, 2);

                    if (item.BEAMID.Equals(prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.BEAMID))
                    {
                        item.STATUS = prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsB.STATUS;
                    }
                }
                return prWeavingProcessBulkViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PrWeavingProcessBulkViewModel> GetNamesAsync(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            prWeavingProcessBulkViewModel = await _fPrWeavingProcessBeamDetailsB.GetInitData(prWeavingProcessBulkViewModel);
            return prWeavingProcessBulkViewModel;
        }

        [HttpGet]
        public async Task<double> GetBeamDetails(int sdId)
        {
            try
            {
                var result = await _fPrSizingProcessRopeDetails.FindByIdAsync(sdId);
                var length = result?.LENGTH_PER_BEAM;

                if (result == null)
                {
                    var res = await _fPrSlasherDyeingDetails.FindByIdAsync(sdId);
                    length = res?.LENGTH_PER_BEAM;
                }

                return length ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDeliverable(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel, int doffId)
        {
            try
            {

                ModelState.Clear();
                if (doffId != 0)
                {
                    var doffDetails = await _fPrWeavingProcessDetailsB.FindByIdAsync(doffId);
                    doffDetails.IS_DELIVERABLE = true;
                    var isUpdate = await _fPrWeavingProcessDetailsB.Update(doffDetails);
                    if (isUpdate)
                    {
                        foreach (var i in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList.SelectMany(item => item.FPrWeavingProcessDetailsBList.Where(c => c.TRNSID.Equals(doffDetails))))
                        {
                            i.IS_DELIVERABLE = true;
                        }
                    }
                }
                Response.Headers["Status"] = "Success";

                prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Response.Headers["Status"] = "Failed";

                prWeavingProcessBulkViewModel = CalculateCrimp(prWeavingProcessBulkViewModel);
                prWeavingProcessBulkViewModel = await GetNamesAsync(prWeavingProcessBulkViewModel);
                return PartialView($"AddBeamList", prWeavingProcessBulkViewModel);
                throw;
            }
        }

        [HttpGet]
        public async Task<dynamic> GetSetDetails(int setId)
        {
            try
            {
                var result = await _fPrWeavingProcessMasterB.GetSetDetails(setId);

                var yarnLength = 0.0;

                if (result.DYEING_TYPE == "Rope" || result.DYEING_TYPE == "Sizing" || result.DYEING_TYPE == "Sectional")
                {
                    foreach (var item in result.SIZING_BEAM)
                    {
                        foreach (var i in item.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                        {
                            yarnLength += i.LENGTH_PER_BEAM ?? 0;
                        }
                    }
                }
                else if (result.DYEING_TYPE == "Slasher")
                {
                    foreach (var item in result.SLASHER_BEAM)
                    {
                        foreach (var i in item.F_PR_SLASHER_DYEING_DETAILS)
                        {
                            yarnLength += i.LENGTH_PER_BEAM ?? 0;
                        }
                    }
                }

                var crimp = result.CRIMP_PERCENTAGE ?? 12;
                var greyLength = yarnLength * ((100 - crimp) / 100);

                Response.Headers["GreyLength"] = greyLength.ToString();
                Response.Headers["SizingLength"] = yarnLength.ToString();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YarnBudgetList(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                var result = await _fPrWeavingProcessMasterB.GetSetDetails(prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB.SETID ?? 0);

                if (result == null)
                {
                    return PartialView($"YarnBudgetList", prWeavingProcessBulkViewModel);
                }

                //prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList = new List<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
                var yarnLength = 0.0;

                if (result.DYEING_TYPE == "Rope" || result.DYEING_TYPE == "Sizing" || result.DYEING_TYPE == "Sectional")
                {
                    foreach (var item in result.SIZING_BEAM)
                    {
                        foreach (var i in item.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                        {
                            yarnLength += i.LENGTH_PER_BEAM ?? 0;
                        }
                    }
                }
                else if (result.DYEING_TYPE == "Slasher")
                {
                    foreach (var item in result.SLASHER_BEAM)
                    {
                        foreach (var i in item.F_PR_SLASHER_DYEING_DETAILS)
                        {
                            yarnLength += i.LENGTH_PER_BEAM ?? 0;
                        }
                    }
                }

                var crimp = result.CRIMP_PERCENTAGE ?? 12;
                var greyLength = yarnLength * ((100 - crimp) / 100);
                var totalRatioWeft = 0.0;

                Response.Headers["GreyLength"] = greyLength.ToString();
                Response.Headers["SizingLength"] = yarnLength.ToString();

                //var warpSetLength = data.plProductionSetDistribution.proG_.seT_QTY;
                //var greySetLength = warpLength * 0.92;

                foreach (var item in result.RndFabricCountInfoViewModels)
                {
                    if (item.RndFabricCountinfo.YARNFOR == 2)
                    {
                        totalRatioWeft += (double)item.RndFabricCountinfo.RATIO;
                    }
                }
                var yarnData = "";

                if (prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList.Any(c => c.TRNSID > 0))
                {

                    prWeavingProcessBulkViewModel =
                        await _fPrWeavingProcessMasterB.GetConsumpDetails(prWeavingProcessBulkViewModel);

                    return PartialView($"YarnBudgetList", prWeavingProcessBulkViewModel);
                }

                foreach (var item in result.RndFabricCountInfoViewModels)
                {
                    if (item.RndFabricCountinfo.YARNFOR == 2)
                    {
                        var reqSetKgs = 0.0;


                        if (result.LOOMID == 1)
                        {
                            reqSetKgs = (greyLength * (result.REED_SPACE ?? 0 + 3) * (double)result.GRPPI ??
                                         0 * (double)item.RndFabricCountinfo.RATIO) / ((double)item.RndFabricCountinfo.NE * totalRatioWeft * 768 * 2.2046);
                        }
                        else if (result.LOOMID == 2)
                        {
                            reqSetKgs = (greyLength * (result.REED_SPACE ?? 0 + 6) * (double)result.GRPPI ??
                                         0 * (double)item.RndFabricCountinfo.RATIO) / ((double)item.RndFabricCountinfo.NE * totalRatioWeft * 768 * 2.2046);
                        }

                        var consumpDetails = new F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS();

                        if (item.FPrWeavingWeftYarnConsumDetails != null)
                        {
                            consumpDetails.TRNSID = item.FPrWeavingWeftYarnConsumDetails.TRNSID;
                            consumpDetails.TRNSDATE = item.FPrWeavingWeftYarnConsumDetails.TRNSDATE;
                            consumpDetails.WV_PROCESSID = item.FPrWeavingWeftYarnConsumDetails.WV_PROCESSID;
                            consumpDetails.YARN_RETURN = item.FPrWeavingWeftYarnConsumDetails.YARN_RETURN;
                            consumpDetails.CONSUMP = item.FPrWeavingWeftYarnConsumDetails.CONSUMP;
                            consumpDetails.WASTE = item.FPrWeavingWeftYarnConsumDetails.WASTE;
                            consumpDetails.WASTE_PERCENTAGE = item.FPrWeavingWeftYarnConsumDetails.WASTE_PERCENTAGE;
                            consumpDetails.REMARKS = item.FPrWeavingWeftYarnConsumDetails.REMARKS;
                        }

                        consumpDetails.COUNTID = item.RndFabricCountinfo.TRNSID;
                        consumpDetails.COUNT = item.RndFabricCountinfo;
                        consumpDetails.LOTID = item.RndFabricCountinfo.LOTID;
                        consumpDetails.LOT = item.RndFabricCountinfo.LOT;
                        consumpDetails.SUPPID = item.RndFabricCountinfo.SUPPID;
                        consumpDetails.SUPP = item.RndFabricCountinfo.SUPP;
                        consumpDetails.RATIO = item.RndFabricCountinfo.RATIO.ToString();
                        consumpDetails.SET_BGT = Math.Round(reqSetKgs, 0).ToString(CultureInfo.InvariantCulture);
                        consumpDetails.ACT_BGT = Math.Round(reqSetKgs, 0).ToString(CultureInfo.InvariantCulture);

                        prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList.Add(consumpDetails);
                    }
                }


                return PartialView($"YarnBudgetList", prWeavingProcessBulkViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Response.Headers["Status"] = "Failed";
                return null;
            }
        }


        [HttpGet]
        public async Task<string> GetDoffLength(int doffId)
        {
            var result = await _fPrWeavingProcessDetailsB.FindByIdAsync(doffId);
            return result.LENGTH_BULK.ToString();
        }



        [HttpGet]
        public IActionResult RWeavingDeliveryReport(string progNo)
        {
            return View(model: progNo);
        }


    }
}