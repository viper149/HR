using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FPrSizingProductionRopeController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_SIZING_PROCESS_ROPE_MASTER _fPrSizingProcessRopeMaster;
        private readonly IF_PR_SIZING_PROCESS_ROPE_DETAILS _fPrSizingProcessRopeDetails;
        private readonly IF_PR_SIZING_PROCESS_ROPE_CHEM _fPrSizingProcessRopeChem;
        private readonly IF_SIZING_MACHINE _fSizingMachine;
        private readonly IF_WEAVING_BEAM _fWeavingBeam;

        public FPrSizingProductionRopeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_SIZING_PROCESS_ROPE_MASTER fPrSizingProcessRopeMaster,
            IF_PR_SIZING_PROCESS_ROPE_DETAILS fPrSizingProcessRopeDetails,
            IF_PR_SIZING_PROCESS_ROPE_CHEM fPrSizingProcessRopeChem,
            IF_SIZING_MACHINE fSizingMachine,
            IF_WEAVING_BEAM fWeavingBeam,
            IF_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrSizingProcessRopeMaster = fPrSizingProcessRopeMaster;
            _fPrSizingProcessRopeDetails = fPrSizingProcessRopeDetails;
            _fPrSizingProcessRopeChem = fPrSizingProcessRopeChem;
            _fSizingMachine = fSizingMachine;
            _fWeavingBeam = fWeavingBeam;
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

                var data = await _fPrSizingProcessRopeMaster.GetAllAsync();

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
                    item.EncryptedId = _protector.Protect(item.SID.ToString());
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
        public IActionResult GetSizingProductionRope()
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
        public async Task<IActionResult> CreateSizingProductionRope()
        {
            var fSizingProductionRopeViewModel = await GetInfo(new FSizingProductionRopeViewModel());
            fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster = new F_PR_SIZING_PROCESS_ROPE_MASTER
            {
                TRNSDATE = DateTime.Now
            };

            return View(fSizingProductionRopeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSizingProductionRope(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.CREATED_BY = user.Id;
                    fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.UPDATED_BY = user.Id;
                    fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.CREATED_AT = DateTime.Now;
                    fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.UPDATED_AT = DateTime.Now;
                    var sizingId = await _fPrSizingProcessRopeMaster.InsertAndGetIdAsync(fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster);

                    if (sizingId != 0)
                    {
                        foreach (var item in fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList)
                        {
                            item.SID = sizingId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrSizingProcessRopeChem.InsertByAsync(item);
                        }

                        foreach (var item in fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList)
                        {
                            item.SID = sizingId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrSizingProcessRopeDetails.InsertByAsync(item);
                        }
                        TempData["message"] = "Successfully Sizing Process Rope Created.";
                        TempData["type"] = "success";
                        return RedirectToAction($"EditSizingProductionRope",new {id= _protector.Protect(sizingId.ToString()) });

                        //rsizingIdsizingIdeturn RedirectToAction("GetSizingProductionRope", $"FPrSizingProductionRope");
                    }
                    TempData["message"] = "Failed to Create Sizing Process Rope.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fSizingProductionRopeViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fSizingProductionRopeViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Sizing Process Rope.";
                TempData["type"] = "error";
                return View(await GetInfo(fSizingProductionRopeViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditSizingProductionRope(string id)
        {
            try
            {
                var sId = int.Parse(_protector.Unprotect(id));
                var fSizingProductionRopeViewMode = await _fPrSizingProcessRopeMaster.FindAllByIdAsync(sId);

                if (fSizingProductionRopeViewMode.FPrSizingProcessRopeMaster != null)
                {
                    fSizingProductionRopeViewMode = await GetInfo(fSizingProductionRopeViewMode);
                    fSizingProductionRopeViewMode.FPrSizingProcessRopeMaster.EncryptedId = _protector.Protect(fSizingProductionRopeViewMode.FPrSizingProcessRopeMaster.SID.ToString());

                    return View(fSizingProductionRopeViewMode);
                }

                TempData["message"] = "Sizing Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetSizingProductionRope", $"FPrSizingProductionRope");
            }
            catch (Exception)
            {
                TempData["message"] = "Sizing Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetSizingProductionRope", $"FPrSizingProductionRope");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditSizingProductionRope(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sId = int.Parse(_protector.Unprotect(fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.EncryptedId));
                    if (fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.SID == sId)
                    {
                        var sizingDetails = await _fPrSizingProcessRopeMaster.FindByIdAsync(sId);

                        var user = await _userManager.GetUserAsync(User);
                        fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.UPDATED_BY = user.Id;
                        fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.UPDATED_AT = DateTime.Now;
                        fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.CREATED_AT = sizingDetails.CREATED_AT;
                        fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.CREATED_BY = sizingDetails.CREATED_BY;

                        var isUpdated = await _fPrSizingProcessRopeMaster.Update(fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster);
                        if (isUpdated)
                        {
                            foreach (var item in fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList.Where(c=>c.S_CHEMID.Equals(0)))
                            {
                                item.SID = fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.SID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrSizingProcessRopeChem.InsertByAsync(item);
                            }

                            foreach (var item in fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList.Where(c=>c.SDID.Equals(0)))
                            {
                                item.SID = fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.SID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fPrSizingProcessRopeDetails.InsertByAsync(item);
                            }
                            TempData["message"] = "Successfully Updated Sizing Details.";
                            TempData["type"] = "success";
                            
                            return RedirectToAction($"EditSizingProductionRope", new { id = _protector.Protect(fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.SID.ToString()) });

                            //return RedirectToAction("GetSizingProductionRope", $"FPrSizingProductionRope");
                        }
                        TempData["message"] = "Failed to Update Sizing Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetSizingProductionRope", $"FPrSizingProductionRope");
                    }
                    TempData["message"] = "Invalid Sizing Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetSizingProductionRope", $"FPrSizingProductionRope");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                fSizingProductionRopeViewModel = new FSizingProductionRopeViewModel
                {
                    FPrSizingProcessRopeMaster = await _fPrSizingProcessRopeMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.EncryptedId)))
                };
                fSizingProductionRopeViewModel = await GetInfo(fSizingProductionRopeViewModel);
                return View(fSizingProductionRopeViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Sizing Details.";
                TempData["type"] = "error";
                fSizingProductionRopeViewModel = new FSizingProductionRopeViewModel
                {
                    FPrSizingProcessRopeMaster = await _fPrSizingProcessRopeMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.EncryptedId)))
                };
                fSizingProductionRopeViewModel = await GetInfo(fSizingProductionRopeViewModel);
                return View(fSizingProductionRopeViewModel);
            }
        }
        
        public async Task<FSizingProductionRopeViewModel> GetInfo(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            fSizingProductionRopeViewModel = await _fPrSizingProcessRopeMaster.GetInitObjects(fSizingProductionRopeViewModel);
            return fSizingProductionRopeViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChemList(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            try
            {
                var flag = fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList.Where(c => c.CHEM_PRODID.Equals(fSizingProductionRopeViewModel.FPrSizingProcessRopeChem.CHEM_PRODID));

                if (!flag.Any())
                {
                    fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList.Add(fSizingProductionRopeViewModel.FPrSizingProcessRopeChem);
                }
                fSizingProductionRopeViewModel = await GetChemDetailsAsync(fSizingProductionRopeViewModel);
                return PartialView($"AddChemList", fSizingProductionRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fSizingProductionRopeViewModel = await GetChemDetailsAsync(fSizingProductionRopeViewModel);
                return PartialView($"AddChemList", fSizingProductionRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveChemFromList(FSizingProductionRopeViewModel fSizingProductionRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList[int.Parse(removeIndexValue)].S_CHEMID != 0)
            {
                await _fPrSizingProcessRopeChem.Delete(fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList[int.Parse(removeIndexValue)]);
            }
            fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList.RemoveAt(int.Parse(removeIndexValue));
            fSizingProductionRopeViewModel = await GetChemDetailsAsync(fSizingProductionRopeViewModel);
            return PartialView($"AddChemList", fSizingProductionRopeViewModel);
        }

        public async Task<FSizingProductionRopeViewModel> GetChemDetailsAsync(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList = (List<F_PR_SIZING_PROCESS_ROPE_CHEM>)await _fPrSizingProcessRopeMaster.GetInitChemData(fSizingProductionRopeViewModel.FPrSizingProcessRopeChemList);
            return fSizingProductionRopeViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBeamList(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            try
            {
                if (fSizingProductionRopeViewModel.FPrSizingProcessRopeDetails.W_BEAMID != null)
                {
                    var flag = fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList.Where(c => c.W_BEAMID.Equals(fSizingProductionRopeViewModel.FPrSizingProcessRopeDetails.W_BEAMID));

                    if (!flag.Any())
                    {
                        fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList.Add(fSizingProductionRopeViewModel.FPrSizingProcessRopeDetails);
                    }
                }
                GetSetQty(fSizingProductionRopeViewModel);
                fSizingProductionRopeViewModel = await GetBeamDetailsAsync(fSizingProductionRopeViewModel);
                return PartialView($"AddBeamList", fSizingProductionRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                GetSetQty(fSizingProductionRopeViewModel);
                fSizingProductionRopeViewModel = await GetBeamDetailsAsync(fSizingProductionRopeViewModel);
                return PartialView($"AddBeamList", fSizingProductionRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBeamFromList(FSizingProductionRopeViewModel fSizingProductionRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList[int.Parse(removeIndexValue)].SDID != 0)
            {
                await _fPrSizingProcessRopeDetails.Delete(fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList[int.Parse(removeIndexValue)]);
            }
            fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList.RemoveAt(int.Parse(removeIndexValue));
            GetSetQty(fSizingProductionRopeViewModel);
            fSizingProductionRopeViewModel = await GetBeamDetailsAsync(fSizingProductionRopeViewModel);
            return PartialView($"AddBeamList", fSizingProductionRopeViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDeliverable(FSizingProductionRopeViewModel fSizingProductionRopeViewModel, int sdId)
        {
            ModelState.Clear();
            if (sdId != 0)
            {
                var sizingDetails = await _fPrSizingProcessRopeDetails.FindByIdAsync(sdId);
                sizingDetails.IS_DELIVERABLE = true;
                var isUpdate = await _fPrSizingProcessRopeDetails.Update(sizingDetails);
                if (isUpdate)
                {
                    foreach (var item in fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList.Where(item => item.SDID.Equals(sdId)))
                    {
                        item.IS_DELIVERABLE = true;
                    }
                }
            }
             
            GetSetQty(fSizingProductionRopeViewModel);
            fSizingProductionRopeViewModel = await GetBeamDetailsAsync(fSizingProductionRopeViewModel);
            return PartialView($"AddBeamList", fSizingProductionRopeViewModel);
        }

        public async Task<FSizingProductionRopeViewModel> GetBeamDetailsAsync(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList = (List<F_PR_SIZING_PROCESS_ROPE_DETAILS>)await _fPrSizingProcessRopeMaster.GetInitBeamData(fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList);
            return fSizingProductionRopeViewModel;
        }

        private async void GetSetQty(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            if (fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.SETID == null) return;

            var production = fSizingProductionRopeViewModel.FPrSizingProcessRopeDetailsList.Sum(c => c.LENGTH_PER_BEAM);
            var setDetails = await _fPrSizingProcessRopeMaster.GetSetDetails((int)fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.SETID);

            var a = fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.LCB_ACT_LENGTH - production;
            var elongation = fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.LCB_ACT_LENGTH!=0?(((production - fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.LCB_ACT_LENGTH) / fSizingProductionRopeViewModel.FPrSizingProcessRopeMaster.LCB_ACT_LENGTH)*100):0;
            Response.Headers["Pending"] = a.ToString();
            Response.Headers["Production"] = production.ToString();
            Response.Headers["Elongation"] = $"{elongation:.##}";
        }
        
        [HttpGet]
        public async Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId)
        {
            var result = await _fPrSizingProcessRopeMaster.GetSetDetails(setId);
            return result;
        }

        [HttpGet]
        public IActionResult RSizingDeliveryReport(string setNo)
        {
            return View(model: setNo);
        }

        //[HttpGet]
        //public IActionResult RSizingStrickerReport()
        //{
        //    return View();
        //}
    }
}