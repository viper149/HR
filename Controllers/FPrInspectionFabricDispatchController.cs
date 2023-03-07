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
    public class FPrInspectionFabricDispatchController : Controller
    {

        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_INSPECTION_FABRIC_D_MASTER _fPrInspectionFabricDMaster;
        private readonly IF_PR_INSPECTION_FABRIC_D_DETAILS _fPrInspectionFabricDDetails;
        

        public FPrInspectionFabricDispatchController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_INSPECTION_FABRIC_D_MASTER fPrInspectionFabricDMaster,
            IF_PR_INSPECTION_FABRIC_D_DETAILS fPrInspectionFabricDDetails
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrInspectionFabricDMaster = fPrInspectionFabricDMaster;
            _fPrInspectionFabricDDetails = fPrInspectionFabricDDetails;
            
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
                var recordsTotal = 0;

                var data = await _fPrInspectionFabricDMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                               m.SEC.SECNAME != null && m.SEC.SECNAME.ToUpper().Contains(searchValue)
                                           || (m.DDATE != null && m.DDATE.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.DID.ToString());
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
        public IActionResult GetRollReceiveList()
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
        public async Task<IActionResult> CreateRollReceive()
        {
            try
            {
                var fPrInspectionFabricDispatchViewModel = await GetInfo(new FPrInspectionFabricDispatchViewModel());
                fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster = new F_PR_INSPECTION_FABRIC_D_MASTER()
                {
                    DDATE = DateTime.Now,
                    SECID = 167
                };
                return View(fPrInspectionFabricDispatchViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRollReceive(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rollDetails = await _fPrInspectionFabricDMaster.GetRollDetailsByDate(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.DDATE);
                    var user = await _userManager.GetUserAsync(User);

                    int dId;

                    if (rollDetails == null)
                    {
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.CREATED_BY = user.Id;
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.UPDATED_BY = user.Id;
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.CREATED_AT = DateTime.Now;
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.UPDATED_AT = DateTime.Now;
                        dId = await _fPrInspectionFabricDMaster.InsertAndGetIdAsync(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster);
                    }
                    else
                    {
                        dId = rollDetails.DID;
                    }

                    if (dId != 0)
                    {
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList = fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList.GroupBy(c => c.ROLL_ID)
                            .Select(c => c.First())
                            .ToList();
                        foreach (var item in fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList)
                        {
                            item.DID = dId;
                            item.BALANCE_QTY = item.QTY_YARDS;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var x = await _fPrInspectionFabricDDetails.GetRollIDetails(item.ROLL_ID ?? 0);
                            if (x == null)
                            {
                                await _fPrInspectionFabricDDetails.InsertByAsync(item);
                            }
                        }

                        TempData["message"] = "Successfully Roll Received.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRollReceiveList", $"FPrInspectionFabricDispatch");
                    }

                    TempData["message"] = "Failed to Receive Roll.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fPrInspectionFabricDispatchViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrInspectionFabricDispatchViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Receive Roll.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrInspectionFabricDispatchViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> CreateRollReceiveByDate()
        {
            try
            {
                var fPrInspectionFabricDispatchViewModel = await GetInfo(new FPrInspectionFabricDispatchViewModel());
                fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster = new F_PR_INSPECTION_FABRIC_D_MASTER()
                {
                    DDATE = DateTime.Now,
                    SECID = 167
                };
                return View(fPrInspectionFabricDispatchViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRollReceiveByDate(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rollDetails = await _fPrInspectionFabricDMaster.GetRollDetailsByDate(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.DDATE);
                    var user = await _userManager.GetUserAsync(User);

                    int dId;

                    if (rollDetails == null)
                    {
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.CREATED_BY = user.Id;
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.UPDATED_BY = user.Id;
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.CREATED_AT = DateTime.Now;
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.UPDATED_AT = DateTime.Now;
                        dId = await _fPrInspectionFabricDMaster.InsertAndGetIdAsync(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster);
                    }
                    else
                    {
                        dId = rollDetails.DID;
                    }

                    if (dId != 0)
                    {
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList = fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList.GroupBy(c => c.ROLL_ID)
                            .Select(c => c.First())
                            .ToList();
                        foreach (var item in fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList)
                        {
                            item.DID = dId;
                            item.BALANCE_QTY = item.QTY_YARDS;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var x = await _fPrInspectionFabricDDetails.GetRollIDetails(item.ROLL_ID ?? 0);
                            if (x == null)
                            {
                                await _fPrInspectionFabricDDetails.InsertByAsync(item);
                            }
                        }

                        TempData["message"] = "Successfully Roll Received.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRollReceiveList", $"FPrInspectionFabricDispatch");
                    }

                    TempData["message"] = "Failed to Receive Roll.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fPrInspectionFabricDispatchViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrInspectionFabricDispatchViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Receive Roll.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrInspectionFabricDispatchViewModel));
            }
        }

        public async Task<FPrInspectionFabricDispatchViewModel> GetInfo(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            fPrInspectionFabricDispatchViewModel = await _fPrInspectionFabricDMaster.GetInitObjects(fPrInspectionFabricDispatchViewModel);
            return fPrInspectionFabricDispatchViewModel;
        }


        [HttpGet]
        public async Task<IActionResult> DetailsRollReceive(string id)
        {
            return View(await _fPrInspectionFabricDMaster.FindByRollRcvIdAsync(int.Parse(_protector.Unprotect(id))));
        }

        [HttpPost]
        public async Task<IActionResult> RollDetailsListByScan(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                var rollId = await _fPrInspectionFabricDDetails.GetRollIdByRollNo(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.ROLL_NO);

                fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.ROLL_ID = rollId.ROLL_ID;

                var result = await _fPrInspectionFabricDDetails.FindRollDetails(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.ROLL_ID ?? 0, (DateTime)fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.DDATE);
                var roll = await _fPrInspectionFabricDDetails.GetRollIDetails(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails
                    .ROLL_ID ?? 0);

                if (roll != null)
                {
                    Response.Headers["Status"] = "DB";
                }
                else if (result == null)
                {
                    Response.Headers["Status"] = "Null";
                }
                else if (!fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList.Any(c =>
                    c.ROLL_ID.Equals(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.ROLL_ID)))
                {
                    fPrInspectionFabricDispatchViewModel = await _fPrInspectionFabricDDetails.GetRollsByScanAsync(fPrInspectionFabricDispatchViewModel);
                    Response.Headers["Status"] = "Success";
                }
                else
                {
                    Response.Headers["Status"] = "Error";
                }

                fPrInspectionFabricDispatchViewModel = await _fPrInspectionFabricDDetails.GetRollDetailsList(fPrInspectionFabricDispatchViewModel);
                return PartialView($"RollDetailsList", fPrInspectionFabricDispatchViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> RollDetailsList(DateTime dDate)
        {
            try
            {
                var fPrInspectionFabricDispatchViewModel = await _fPrInspectionFabricDDetails.GetRollsAsync(dDate);
                return PartialView($"RollDetailsList", fPrInspectionFabricDispatchViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetRollTableData()
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
                var recordsTotal = 0;

                var data = await _fPrInspectionFabricDDetails.GetRollListAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                               m.FABCODENavigation.STYLE_NAME != null && m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue)
                                           || (m.D.DDATE != null && m.D.DDATE.ToString().Contains(searchValue))
                                           || (m.PO_NONavigation.PINO != null && m.PO_NONavigation.PINO.ToUpper().Contains(searchValue))
                                           || (m.SO_NONavigation.SO_NO != null && m.SO_NONavigation.SO_NO.ToUpper().Contains(searchValue))
                                           || (m.ROLL_.ROLLNO != null && m.ROLL_.ROLLNO.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.TRNSID.ToString());
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
        public IActionResult GetRollReceiveListForQc()
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
        public async Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRollIDetails(int rollId)
        {
            try
            {
                var rollDetails = await _fPrInspectionFabricDDetails.GetRollIDetails(rollId);
                return rollDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoApprove(string id)
        {
            try
            {
                var rollDetails = await _fPrInspectionFabricDDetails.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (rollDetails != null)
                {
                    rollDetails.IS_QC_APPROVE = true;
                    rollDetails.QC_APPROVE_DATE = DateTime.Now;
                    rollDetails.IS_QC_REJECT = false;
                    await _fPrInspectionFabricDDetails.Update(rollDetails);
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
        public async Task<IActionResult> DoReject(string id)
        {
            try
            {
                var rollDetails = await _fPrInspectionFabricDDetails.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (rollDetails != null)
                {
                    rollDetails.IS_QC_APPROVE = false;
                    rollDetails.IS_QC_REJECT = true;
                    rollDetails.QC_REJECT_DATE = DateTime.Now;
                    await _fPrInspectionFabricDDetails.Update(rollDetails);
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
        public async Task<IActionResult> RemoveDespatchDetailsFromList(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                ModelState.Clear();
                var fPrInspectionFabricDDetails = fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList[fPrInspectionFabricDispatchViewModel.RemoveIndex];

                    if (fPrInspectionFabricDDetails.TRNSID > 0)
                    {
                        await _fPrInspectionFabricDDetails.Delete(fPrInspectionFabricDDetails);
                    }

                    fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList.RemoveAt(fPrInspectionFabricDispatchViewModel.RemoveIndex);

                    return PartialView($"RollDetailsList", await _fPrInspectionFabricDDetails.GetRollDetailsList(fPrInspectionFabricDispatchViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
