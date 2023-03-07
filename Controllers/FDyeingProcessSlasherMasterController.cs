using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FDyeingProcessSlasherMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_SLASHER_DYEING_MASTER _fPrSlasherDyeingMaster;
        private readonly IF_PR_SLASHER_DYEING_DETAILS _fPrSlasherDyeingDetails;
        private readonly IF_PR_SLASHER_CHEM_CONSM _fPrSlasherChemConsm;
        private readonly IF_PR_SLASHER_MACHINE_INFO _fPrSlasherMachineInfo;
        private readonly IF_LCB_PRODUCTION_ROPE_MASTER _fLcbProductionRopeMaster;

        public FDyeingProcessSlasherMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_SLASHER_DYEING_MASTER fPrSlasherDyeingMaster,
            IF_PR_SLASHER_DYEING_DETAILS fPrSlasherDyeingDetails,
            IF_PR_SLASHER_CHEM_CONSM fPrSlasherChemConsm,
            IF_PR_SLASHER_MACHINE_INFO fPrSlasherMachineInfo,
            IF_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrSlasherDyeingMaster = fPrSlasherDyeingMaster;
            _fPrSlasherDyeingDetails = fPrSlasherDyeingDetails;
            _fPrSlasherChemConsm = fPrSlasherChemConsm;
            _fPrSlasherMachineInfo = fPrSlasherMachineInfo;
            _fLcbProductionRopeMaster = fLcbProductionRopeMaster;
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
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

                var data = await _fPrSlasherDyeingMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                               m.SET.PROG_.PROG_NO != null && m.SET.PROG_.PROG_NO.ToUpper().Contains(searchValue)
                                           || (m.TRNSDATE != null && m.TRNSDATE.ToString().Contains(searchValue))
                                           || (m.BEAM_SPACE != 0 && m.BEAM_SPACE.ToString().Contains(searchValue))
                                           || (m.PICK_UP.ToString() != null && m.PICK_UP.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.SLID.ToString());
                }
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetDyeingProcessSlasher()
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
        public async Task<IActionResult> CreateDyeingProcessSlasher()
        {
            var fDyeingProcessSlasherViewModel = await GetInfo(new FDyeingProcessSlasherViewModel());
            fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster = new F_PR_SLASHER_DYEING_MASTER
            {
                TRNSDATE = DateTime.Now
            };

            return View(fDyeingProcessSlasherViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDyeingProcessSlasher(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.CREATED_BY = user.Id;
                    fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.UPDATED_BY = user.Id;
                    fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.CREATED_AT = DateTime.Now;
                    fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.UPDATED_AT = DateTime.Now;
                    var insertedObj = await _fPrSlasherDyeingMaster.GetInsertedObjByAsync(fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster);

                    if (insertedObj!=null)
                    {
                        foreach (var item in fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList)
                        {
                            item.SLID = insertedObj.SLID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrSlasherDyeingDetails.InsertByAsync(item);
                        }

                        foreach (var item in fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList)        
                        {
                            item.SLID = insertedObj.SLID;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrSlasherChemConsm.InsertByAsync(item);
                        }
                        TempData["message"] = "Successfully Dyeing Process Slasher Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetDyeingProcessSlasher", $"FDyeingProcessSlasherMaster");
                    }
                    TempData["message"] = "Failed to Create Dyeing Process Slasher.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fDyeingProcessSlasherViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fDyeingProcessSlasherViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Dyeing Process Slasher.";
                TempData["type"] = "error";
                return View(await GetInfo(fDyeingProcessSlasherViewModel));
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> EditDyeingProcessSlasher(string id)
        {
            try
            {
                var sId = int.Parse(_protector.Unprotect(id));
                var fDyeingProcessSlasherViewModel = await _fPrSlasherDyeingMaster.FindAllByIdAsync(sId);

                if (fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster != null)
                {
                    fDyeingProcessSlasherViewModel = await GetInfo(fDyeingProcessSlasherViewModel);
                    fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.EncryptedId = _protector.Protect(fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.SLID.ToString());
                    
                    return View(fDyeingProcessSlasherViewModel);
                }

                TempData["message"] = "Slasher Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetDyeingProcessSlasher", $"FDyeingProcessSlasherMaster");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetDyeingProcessSlasher", $"FDyeingProcessSlasherMaster");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDyeingProcessSlasher(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sId = int.Parse(_protector.Unprotect(fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.EncryptedId));
                    if (fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.SLID == sId)
                    {
                        var slasherDetails = await _fPrSlasherDyeingMaster.FindByIdAsync(sId);

                        var user = await _userManager.GetUserAsync(User);
                        fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.UPDATED_BY = user.Id;
                        fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.UPDATED_AT = DateTime.Now;
                        fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.CREATED_AT = slasherDetails.CREATED_AT;
                        fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.CREATED_BY = slasherDetails.CREATED_BY;

                        var isUpdated = await _fPrSlasherDyeingMaster.Update(fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster);
                        if (isUpdated)
                        {
                            foreach (var item in fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.Where(c => c.SLDID.Equals(0)))
                            {
                                item.SLID = fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.SLID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrSlasherDyeingDetails.InsertByAsync(item);
                            }

                            foreach (var item in fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.Where(c => c.SLDID>0))
                            {
                                var beamDetails = await _fPrSlasherDyeingDetails.FindByIdAsync(item.SLDID);

                                item.CREATED_AT = beamDetails.CREATED_AT;
                                item.CREATED_BY = beamDetails.CREATED_BY;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrSlasherDyeingDetails.Update(item);
                            }

                            foreach (var item in fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList.Where(c => c.SL_CHEMID.Equals(0)))
                            {
                                item.SLID = fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.SLID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrSlasherChemConsm.InsertByAsync(item);
                            }
                            TempData["message"] = "Successfully Updated Slasher Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetDyeingProcessSlasher", $"FDyeingProcessSlasherMaster");
                        }
                        TempData["message"] = "Failed to Update Slasher Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetDyeingProcessSlasher", $"FDyeingProcessSlasherMaster");
                    }
                    TempData["message"] = "Invalid Slasher Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetDyeingProcessSlasher", $"FDyeingProcessSlasherMaster");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                fDyeingProcessSlasherViewModel = new FDyeingProcessSlasherViewModel()
                {
                    FPrSlasherDyeingMaster = await _fPrSlasherDyeingMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.EncryptedId)))
                };
                fDyeingProcessSlasherViewModel = await GetInfo(fDyeingProcessSlasherViewModel);
                return View(fDyeingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                fDyeingProcessSlasherViewModel = new FDyeingProcessSlasherViewModel
                {
                    FPrSlasherDyeingMaster = await _fPrSlasherDyeingMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.EncryptedId)))
                };
                fDyeingProcessSlasherViewModel = await GetInfo(fDyeingProcessSlasherViewModel);
                return View(fDyeingProcessSlasherViewModel);
            }
        }

        public async Task<FDyeingProcessSlasherViewModel> GetInfo(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            fDyeingProcessSlasherViewModel = await _fPrSlasherDyeingMaster.GetInitObjects(fDyeingProcessSlasherViewModel);
            return fDyeingProcessSlasherViewModel;
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChemList(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            try
            {
                var flag = fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList.Where(c => c.CHEM_PRODID.Equals(fDyeingProcessSlasherViewModel.FPrSlasherChemConsm.CHEM_PRODID));

                if (!flag.Any())
                {
                    fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList.Add(fDyeingProcessSlasherViewModel.FPrSlasherChemConsm);
                }
                fDyeingProcessSlasherViewModel = await GetChemDetailsAsync(fDyeingProcessSlasherViewModel);
                return PartialView($"AddChemList", fDyeingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fDyeingProcessSlasherViewModel = await GetChemDetailsAsync(fDyeingProcessSlasherViewModel);
                return PartialView($"AddChemList", fDyeingProcessSlasherViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveChemFromList(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList[int.Parse(removeIndexValue)].SL_CHEMID != 0)
            {
                await _fPrSlasherChemConsm.Delete(fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList[int.Parse(removeIndexValue)]);
            }

            fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList.RemoveAt(int.Parse(removeIndexValue));
            fDyeingProcessSlasherViewModel = await GetChemDetailsAsync(fDyeingProcessSlasherViewModel);
            return PartialView($"AddChemList", fDyeingProcessSlasherViewModel);
        }

        public async Task<FDyeingProcessSlasherViewModel> GetChemDetailsAsync(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList = (List<F_PR_SLASHER_CHEM_CONSM>)await _fPrSlasherChemConsm.GetInitChemData(fDyeingProcessSlasherViewModel.FPrSlasherChemConsmList);
            return fDyeingProcessSlasherViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBeamList(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            try
            {
                if (fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetails.W_BEAMID != null)
                {
                    var flag = fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.Where(c => c.W_BEAMID.Equals(fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetails.W_BEAMID) && c.BEAM_TYPE.Equals(fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetails.BEAM_TYPE));

                    if (!flag.Any())
                    {
                        fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.Add(fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetails);
                    }
                }
                GetSetQty(fDyeingProcessSlasherViewModel);
                fDyeingProcessSlasherViewModel = await GetBeamDetailsAsync(fDyeingProcessSlasherViewModel);
                return PartialView($"AddBeamList", fDyeingProcessSlasherViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                GetSetQty(fDyeingProcessSlasherViewModel);
                fDyeingProcessSlasherViewModel = await GetBeamDetailsAsync(fDyeingProcessSlasherViewModel);
                return PartialView($"AddBeamList", fDyeingProcessSlasherViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBeamFromList(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            
            if (fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList[int.Parse(removeIndexValue)].SLDID != 0)
            {
                await _fPrSlasherDyeingDetails.Delete(fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList[int.Parse(removeIndexValue)]);
            }

            fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.RemoveAt(int.Parse(removeIndexValue));
            GetSetQty(fDyeingProcessSlasherViewModel);
            fDyeingProcessSlasherViewModel = await GetBeamDetailsAsync(fDyeingProcessSlasherViewModel);
            return PartialView($"AddBeamList", fDyeingProcessSlasherViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDeliverable(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel, int sdId)
        {
            ModelState.Clear();
            if (sdId != 0)
            {
                var slasherDetails = await _fPrSlasherDyeingDetails.FindByIdAsync(sdId);
                slasherDetails.IS_DELIVERABLE = true;
                var isUpdate = await _fPrSlasherDyeingDetails.Update(slasherDetails);
                if (isUpdate)
                {
                    foreach (var item in fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.Where(item => item.SLDID.Equals(sdId)))
                    {
                        item.IS_DELIVERABLE = true;
                    }
                }
            }

            GetSetQty(fDyeingProcessSlasherViewModel);
            fDyeingProcessSlasherViewModel = await GetBeamDetailsAsync(fDyeingProcessSlasherViewModel);
            return PartialView($"AddBeamList", fDyeingProcessSlasherViewModel);
        }

        public async Task<FDyeingProcessSlasherViewModel> GetBeamDetailsAsync(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList = (List<F_PR_SLASHER_DYEING_DETAILS>)await _fPrSlasherDyeingDetails.GetInitBeamData(fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList);
            return fDyeingProcessSlasherViewModel;
        }

        private async void GetSetQty(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {
            if (fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.SETID == null) return;

            var production = fDyeingProcessSlasherViewModel.FPrSlasherDyeingDetailsList.Sum(c => c.LENGTH_PER_BEAM);
            var warpLength = await _fPrSlasherDyeingMaster.GetSetWarpLength((int)fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster.SETID);

            var a = int.Parse(warpLength) - production;
            Response.Headers["Pending"] = a.ToString();
            Response.Headers["Production"] = production.ToString();
        }
        
        [HttpGet]
        public async Task<dynamic> GetSetDetails(string setId)
        {
            var result = await _fPrSlasherDyeingMaster.GetSetDetails(int.Parse(setId));
            return result;
        }
        

        [HttpGet]
        public IActionResult RSlasherDeliveryReport(string PROG_NO)
        {
            return View(model: PROG_NO);
        }

        [HttpGet]
        public IActionResult RSlasherStickerReport(string PROG_NO)
        {
            return View(model: PROG_NO);
        }

    }
}