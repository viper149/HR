using System;
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
    public class FPrFinishingBeamReceiveController : Controller
    {

        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_FINISHING_BEAM_RECEIVE _fPrFinishingBeamReceive;
        private readonly IF_PR_WEAVING_PROCESS_DETAILS_B _fPrWeavingProcessDetailsB;

        public FPrFinishingBeamReceiveController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_FINISHING_BEAM_RECEIVE fPrFinishingBeamReceive,
            IF_PR_WEAVING_PROCESS_DETAILS_B fPrWeavingProcessDetailsB
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrFinishingBeamReceive = fPrFinishingBeamReceive;
            _fPrWeavingProcessDetailsB = fPrWeavingProcessDetailsB;
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

                var data = await _fPrFinishingBeamReceive.GetAllAsync();

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
                                           || (m.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO != null && m.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO.Contains(searchValue))
                                           || (m.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO != null && m.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO.Contains(searchValue))
                                           || (m.FABCODENavigation.STYLE_NAME != null && m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue))
                                           || (m.RCVBYNavigation.FIRST_NAME != null && m.RCVBYNavigation.FIRST_NAME.Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.FDRID.ToString());
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
        public IActionResult GetFinishingBeamReceiveList()
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
        public async Task<IActionResult> CreateFinishingBeamReceive()
        {
            var fPrFinishingBeamReceiveViewModel = await GetInfo(new FPrFinishingBeamReceiveViewModel());
            return View(fPrFinishingBeamReceiveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFinishingBeamReceive(FPrFinishingBeamReceiveViewModel fPrFinishingBeamReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.CREATED_BY = user.Id;
                    fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.UPDATED_BY = user.Id;
                    fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.CREATED_AT = DateTime.Now;
                    fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.UPDATED_AT = DateTime.Now;
                    var isInsertred = await _fPrFinishingBeamReceive.InsertByAsync(fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive);

                    if (isInsertred)
                    {
                        TempData["message"] = "Successfully Finishing Beam Received.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetFinishingBeamReceiveList", $"FPrFinishingBeamReceive");
                    }
                    TempData["message"] = "Failed to Received Finishing Beam.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fPrFinishingBeamReceiveViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrFinishingBeamReceiveViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Received Finishing Beam.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrFinishingBeamReceiveViewModel));
            }
        }

        public async Task<FPrFinishingBeamReceiveViewModel> GetInfo(FPrFinishingBeamReceiveViewModel fPrFinishingBeamReceiveViewModel)
        {
            fPrFinishingBeamReceiveViewModel = await _fPrFinishingBeamReceive.GetInitObjects(fPrFinishingBeamReceiveViewModel);
            return fPrFinishingBeamReceiveViewModel;
        }

        [HttpGet]
        public async Task<IActionResult> EditFinishingBeamReceive(string id)
        {
            try
            {
                var rcvId = int.Parse(_protector.Unprotect(id));
                var fPrFinishingBeamReceiveViewModel = new FPrFinishingBeamReceiveViewModel()
                {
                    FPrFinishingBeamReceive = await _fPrFinishingBeamReceive.FindByIdAsync(rcvId)
                };

                if (fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive != null)
                {
                    fPrFinishingBeamReceiveViewModel = await GetInfo(fPrFinishingBeamReceiveViewModel);
                    fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.EncryptedId = _protector.Protect(fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.FDRID.ToString());
                    
                    return View(fPrFinishingBeamReceiveViewModel);
                }

                TempData["message"] = "Beam Details Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFinishingBeamReceiveList", $"FPrFinishingBeamReceive");
            }
            catch (Exception)
            {
                TempData["message"] = "Beam Details Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFinishingBeamReceiveList", $"FPrFinishingBeamReceive");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFinishingBeamReceive(FPrFinishingBeamReceiveViewModel fPrFinishingBeamReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rcvId = int.Parse(_protector.Unprotect(fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.EncryptedId));
                    if (fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.FDRID == rcvId)
                    {
                        var rcvDetails = await _fPrFinishingBeamReceive.FindByIdAsync(rcvId);

                        var user = await _userManager.GetUserAsync(User);
                        fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.UPDATED_BY = user.Id;
                        fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.UPDATED_AT = DateTime.Now;
                        fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.CREATED_AT = rcvDetails.CREATED_AT;
                        fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.CREATED_BY = rcvDetails.CREATED_BY;

                        var isUpdated = await _fPrFinishingBeamReceive.Update(fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive);
                        if (isUpdated)
                        {
                            TempData["message"] = "Successfully Updated Finishing Beam Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFinishingBeamReceiveList", $"FPrFinishingBeamReceive");
                        }
                        TempData["message"] = "Failed to Update Finishing Beam Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetFinishingBeamReceiveList", $"FPrFinishingBeamReceive");
                    }
                    TempData["message"] = "Invalid Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetFinishingBeamReceiveList", $"FPrFinishingBeamReceive");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                fPrFinishingBeamReceiveViewModel = new FPrFinishingBeamReceiveViewModel
                {
                    FPrFinishingBeamReceive = await _fPrFinishingBeamReceive.FindByIdAsync(int.Parse(_protector.Unprotect(fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.EncryptedId)))
                };
                fPrFinishingBeamReceiveViewModel = await GetInfo(fPrFinishingBeamReceiveViewModel);
                return View(fPrFinishingBeamReceiveViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Finishing Details.";
                TempData["type"] = "error";
                fPrFinishingBeamReceiveViewModel = new FPrFinishingBeamReceiveViewModel
                {
                    FPrFinishingBeamReceive = await _fPrFinishingBeamReceive.FindByIdAsync(int.Parse(_protector.Unprotect(fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive.EncryptedId)))
                };
                fPrFinishingBeamReceiveViewModel = await GetInfo(fPrFinishingBeamReceiveViewModel);
                return View(fPrFinishingBeamReceiveViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoApprove(int doffId)
        {
            try
            {
                var beamDetails = await _fPrWeavingProcessDetailsB.FindByIdAsync(doffId);
                if (beamDetails != null)
                {
                    beamDetails.IS_RECEIVED_BY_FINISHING = true;
                    return Json(await _fPrWeavingProcessDetailsB.Update(beamDetails));
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpGet]
        public async Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> GetBeamDetails(int beamId)
        {
            try
            {
                var result = await _fPrWeavingProcessDetailsB.FindByIdAllAsync(beamId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}