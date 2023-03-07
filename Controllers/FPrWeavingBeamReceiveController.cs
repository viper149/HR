using System;
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
    public class FPrWeavingBeamReceiveController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_WEAVING_BEAM_RECEIVING _fPrWeavingBeamReceiving;
        private readonly IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS _fPrWeavingWeftYarnConsumDetails;
        private readonly IF_PR_SIZING_PROCESS_ROPE_DETAILS _fPrSizingProcessRopeDetails;
        private readonly IF_PR_SLASHER_DYEING_DETAILS _fPrSlasherDyeingDetails;

        public FPrWeavingBeamReceiveController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_WEAVING_BEAM_RECEIVING fPrWeavingBeamReceiving,
            IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS fPrWeavingWeftYarnConsumDetails,
            IF_PR_SIZING_PROCESS_ROPE_DETAILS fPrSizingProcessRopeDetails,
            IF_PR_SLASHER_DYEING_DETAILS fPrSlasherDyeingDetails
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrWeavingBeamReceiving = fPrWeavingBeamReceiving;
            _fPrWeavingWeftYarnConsumDetails = fPrWeavingWeftYarnConsumDetails;
            _fPrSizingProcessRopeDetails = fPrSizingProcessRopeDetails;
            _fPrSlasherDyeingDetails = fPrSlasherDyeingDetails;
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

                var data = await _fPrWeavingBeamReceiving.GetAllAsync();

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
                                           || (m.RCVDATE != null && m.RCVDATE.ToString().Contains(searchValue))
                                           || (m.RCVDBYNavigation.FIRST_NAME!=null && m.RCVDBYNavigation.FIRST_NAME.Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.RCVID.ToString());
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
        public IActionResult GetWeavingBeamReceiveList()
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
        public async Task<IActionResult> CreateWeavingBeamReceive()
        {
            var prWeavingProcessViewModel = await GetInfo(new PrWeavingProcessViewModel());
            prWeavingProcessViewModel.FPrWeavingBeamReceiving = new F_PR_WEAVING_BEAM_RECEIVING()
            {
                RCVDATE = DateTime.Now
            };
            return View(prWeavingProcessViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateWeavingBeamReceive(PrWeavingProcessViewModel prWeavingProcessViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    prWeavingProcessViewModel.FPrWeavingBeamReceiving.CREATED_BY = user.Id;
                    prWeavingProcessViewModel.FPrWeavingBeamReceiving.UPDATED_BY = user.Id;
                    prWeavingProcessViewModel.FPrWeavingBeamReceiving.CREATED_AT = DateTime.Now;
                    prWeavingProcessViewModel.FPrWeavingBeamReceiving.UPDATED_AT = DateTime.Now;
                    var isInsertred = await _fPrWeavingBeamReceiving.InsertByAsync(prWeavingProcessViewModel.FPrWeavingBeamReceiving);

                    if (isInsertred)
                    {
                        TempData["message"] = "Successfully Weaving Beam Received.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetWeavingBeamReceiveList", $"FPrWeavingBeamReceive");
                    }
                    TempData["message"] = "Failed to Received Weaving Beam.";
                    TempData["type"] = "error";
                    return View(await GetInfo(prWeavingProcessViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(prWeavingProcessViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Received Weaving Beam.";
                TempData["type"] = "error";
                return View(await GetInfo(prWeavingProcessViewModel));
            }
        }
        
        public async Task<PrWeavingProcessViewModel> GetInfo(PrWeavingProcessViewModel prWeavingProcessViewModel)
        {
            prWeavingProcessViewModel = await _fPrWeavingBeamReceiving.GetInitObjects(prWeavingProcessViewModel);
            return prWeavingProcessViewModel;
        }
        
        [HttpGet]
        public async Task<IActionResult> EditWeavingBeamReceive(string id)
        {
            try
            {
                var rcvId = int.Parse(_protector.Unprotect(id));
                var prWeavingProcessViewModel = new PrWeavingProcessViewModel
                {
                    FPrWeavingBeamReceiving = await _fPrWeavingBeamReceiving.FindByIdAsync(rcvId)
                };

                if (prWeavingProcessViewModel.FPrWeavingBeamReceiving != null)
                {
                    prWeavingProcessViewModel = await GetInfo(prWeavingProcessViewModel);
                    prWeavingProcessViewModel.FPrWeavingBeamReceiving.EncryptedId = _protector.Protect(prWeavingProcessViewModel.FPrWeavingBeamReceiving.RCVID.ToString());

                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails =
                        new F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS
                        {
                            WASTE = "0",
                            YARN_RECEIVE = 0,
                            YARN_RETURN = 0,
                            CONSUMP = "0"
                        };

                    return View(prWeavingProcessViewModel);
                }

                TempData["message"] = "Weaving Set Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetWeavingBeamReceiveList", $"FPrWeavingBeamReceive");
            }
            catch (Exception)
            {
                TempData["message"] = "Weaving Set Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetWeavingBeamReceiveList", $"FPrWeavingBeamReceive");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWeavingBeamReceive(PrWeavingProcessViewModel prWeavingProcessViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rcvId = int.Parse(_protector.Unprotect(prWeavingProcessViewModel.FPrWeavingBeamReceiving.EncryptedId));
                    if (prWeavingProcessViewModel.FPrWeavingBeamReceiving.RCVID == rcvId)
                    {
                        var rcvDetails = await _fPrWeavingBeamReceiving.FindByIdAsync(rcvId);

                        var user = await _userManager.GetUserAsync(User);
                        prWeavingProcessViewModel.FPrWeavingBeamReceiving.UPDATED_BY = user.Id;
                        prWeavingProcessViewModel.FPrWeavingBeamReceiving.UPDATED_AT = DateTime.Now;
                        prWeavingProcessViewModel.FPrWeavingBeamReceiving.CREATED_AT = rcvDetails.CREATED_AT;
                        prWeavingProcessViewModel.FPrWeavingBeamReceiving.CREATED_BY = rcvDetails.CREATED_BY;

                        var isUpdated = await _fPrWeavingBeamReceiving.Update(prWeavingProcessViewModel.FPrWeavingBeamReceiving);
                        if (isUpdated)
                        {
                            TempData["message"] = "Successfully Updated Weaving Beam Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetWeavingBeamReceiveList", $"FPrWeavingBeamReceive");
                        }
                        TempData["message"] = "Failed to Update Weaving Beam Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetWeavingBeamReceiveList", $"FPrWeavingBeamReceive");
                    }
                    TempData["message"] = "Invalid Set Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetWeavingBeamReceiveList", $"FPrWeavingBeamReceive");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                prWeavingProcessViewModel = new PrWeavingProcessViewModel
                {
                    FPrWeavingBeamReceiving = await _fPrWeavingBeamReceiving.FindByIdAsync(int.Parse(_protector.Unprotect(prWeavingProcessViewModel.FPrWeavingBeamReceiving.EncryptedId)))
                };
                prWeavingProcessViewModel = await GetInfo(prWeavingProcessViewModel);
                return View(prWeavingProcessViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Weaving Details.";
                TempData["type"] = "error";
                prWeavingProcessViewModel = new PrWeavingProcessViewModel
                {
                    FPrWeavingBeamReceiving = await _fPrWeavingBeamReceiving.FindByIdAsync(int.Parse(_protector.Unprotect(prWeavingProcessViewModel.FPrWeavingBeamReceiving.EncryptedId)))
                };
                prWeavingProcessViewModel = await GetInfo(prWeavingProcessViewModel);
                return View(prWeavingProcessViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoApprove(int sdid)
        {
            try
            {
                var sizingDetails = await _fPrSizingProcessRopeDetails.FindByIdAsync(sdid);
                if (sizingDetails != null)
                {
                    sizingDetails.IS_WEAVING_RECEIVED = true;
                    await _fPrSizingProcessRopeDetails.Update(sizingDetails);
                    return Json(true);
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
        public async Task<IActionResult> DoApproveSlasherBeam(int sdid)
        {
            try
            {
                var slasherDetails = await _fPrSlasherDyeingDetails.FindByIdAsync(sdid);
                if (slasherDetails != null)
                {
                    slasherDetails.IS_WEAVING_RECEIVED = true;
                    await _fPrSlasherDyeingDetails.Update(slasherDetails);
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddYarnToDb(PrWeavingProcessViewModel prWeavingProcessViewModel)
        {
            try
            {
                var isExists = await _fPrWeavingWeftYarnConsumDetails.FindByIdAndCountAsync(prWeavingProcessViewModel.FPrWeavingBeamReceiving.RCVID, prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.COUNTID);
                if (isExists==null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    //prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.RCVID = prWeavingProcessViewModel.FPrWeavingBeamReceiving.RCVID;
                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.TRNSDATE = DateTime.Now;
                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.CREATED_AT = DateTime.Now;
                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.CREATED_BY = user.Id;
                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.UPDATED_BY = user.Id;
                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.UPDATED_AT = DateTime.Now;
                    var isInsertred = await _fPrWeavingWeftYarnConsumDetails.InsertByAsync(prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails);
                    if (isInsertred)
                    {
                        return Json(true);
                    }
                }
                else
                {
                    var user = await _userManager.GetUserAsync(User);

                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.UPDATED_BY = user.Id;
                    prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.UPDATED_AT = DateTime.Now;
                    isExists.CONSUMP = prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.CONSUMP;
                    isExists.WASTE = prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.WASTE;
                    isExists.WASTE_PERCENTAGE = prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.WASTE_PERCENTAGE;
                    isExists.REMARKS = prWeavingProcessViewModel.FPrWeavingWeftYarnConsumDetails.REMARKS;

                    var isUpdated = await _fPrWeavingWeftYarnConsumDetails.Update(isExists);
                    if (isUpdated)
                    {
                        return Json(true);
                    }
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}