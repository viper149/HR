using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FFsRollReceiveController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_FS_FABRIC_RCV_MASTER _fFsFabricRcvMaster;
        private readonly IF_FS_FABRIC_RCV_DETAILS _fFsFabricRcvDetails;

        public FFsRollReceiveController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_FS_FABRIC_RCV_MASTER fFsFabricRcvMaster,
            IF_FS_FABRIC_RCV_DETAILS fFsFabricRcvDetails
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fFsFabricRcvMaster = fFsFabricRcvMaster;
            _fFsFabricRcvDetails = fFsFabricRcvDetails;
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

                var data = await _fFsFabricRcvMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                               m.SEC.SECNAME != null && m.SEC.SECNAME.ToUpper().Contains(searchValue)
                                           || (m.RCVDATE != null && m.RCVDATE.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

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
                var fFsRollReceiveViewModel = await GetInfo(new FFsRollReceiveViewModel());
                fFsRollReceiveViewModel.FFsFabricRcvMaster = new F_FS_FABRIC_RCV_MASTER()
                {
                    RCVDATE = DateTime.Now,
                    SECID = 167
                };
                return View(fFsRollReceiveViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRollReceive(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rollDetails = await _fFsFabricRcvMaster.GetRollDetailsByDate(fFsRollReceiveViewModel.FFsFabricRcvMaster.RCVDATE);
                    var user = await _userManager.GetUserAsync(User);

                    int rcvId;

                    if (rollDetails == null)
                    {
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.CREATED_BY = user.Id;
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.UPDATED_BY = user.Id;
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.CREATED_AT = DateTime.Now;
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.UPDATED_AT = DateTime.Now;
                        rcvId = await _fFsFabricRcvMaster.InsertAndGetIdAsync(fFsRollReceiveViewModel.FFsFabricRcvMaster);
                    }
                    else
                    {
                        rcvId = rollDetails.RCVID;
                    }

                    if (rcvId != 0)
                    {
                        fFsRollReceiveViewModel.FFsFabricRcvDetailsList = fFsRollReceiveViewModel.FFsFabricRcvDetailsList.GroupBy(c => c.ROLL_ID)
                            .Select(c => c.First())
                            .ToList();
                        foreach (var item in fFsRollReceiveViewModel.FFsFabricRcvDetailsList)
                        {
                            item.RCVID = rcvId;
                            item.BALANCE_QTY = item.QTY_YARDS;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var x = await _fFsFabricRcvDetails.GetRollIDetails(item.ROLL_ID??0);
                            if (x == null)
                            {
                                await _fFsFabricRcvDetails.InsertByAsync(item);
                            }
                        }

                        TempData["message"] = "Successfully Roll Received.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRollReceiveList", $"FFsRollReceive");
                    }

                    TempData["message"] = "Failed to Receive Roll.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fFsRollReceiveViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fFsRollReceiveViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Receive Roll.";
                TempData["type"] = "error";
                return View(await GetInfo(fFsRollReceiveViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> CreateRollReceiveByDate()
        {
            try
            {
                var fFsRollReceiveViewModel = await GetInfo(new FFsRollReceiveViewModel());
                fFsRollReceiveViewModel.FFsFabricRcvMaster = new F_FS_FABRIC_RCV_MASTER()
                {
                    RCVDATE = DateTime.Now,
                    SECID = 167
                };
                return View(fFsRollReceiveViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRollReceiveByDate(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rollDetails = await _fFsFabricRcvMaster.GetRollDetailsByDate(fFsRollReceiveViewModel.FFsFabricRcvMaster.RCVDATE);
                    var user = await _userManager.GetUserAsync(User);

                    int rcvId;

                    if (rollDetails == null)
                    {
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.CREATED_BY = user.Id;
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.UPDATED_BY = user.Id;
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.CREATED_AT = DateTime.Now;
                        fFsRollReceiveViewModel.FFsFabricRcvMaster.UPDATED_AT = DateTime.Now;
                        rcvId = await _fFsFabricRcvMaster.InsertAndGetIdAsync(fFsRollReceiveViewModel.FFsFabricRcvMaster);
                    }
                    else
                    {
                        rcvId = rollDetails.RCVID;
                    }

                    if (rcvId != 0)
                    {
                        fFsRollReceiveViewModel.FFsFabricRcvDetailsList = fFsRollReceiveViewModel.FFsFabricRcvDetailsList.GroupBy(c => c.ROLL_ID)
                            .Select(c => c.First())
                            .ToList();
                        foreach (var item in fFsRollReceiveViewModel.FFsFabricRcvDetailsList)
                        {
                            item.RCVID = rcvId;
                            item.BALANCE_QTY = item.QTY_YARDS;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var x = await _fFsFabricRcvDetails.GetRollIDetails(item.ROLL_ID??0);
                            if (x == null)
                            {
                                await _fFsFabricRcvDetails.InsertByAsync(item);
                            }
                        }

                        TempData["message"] = "Successfully Roll Received.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRollReceiveList", $"FFsRollReceive");
                    }

                    TempData["message"] = "Failed to Receive Roll.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fFsRollReceiveViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fFsRollReceiveViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Receive Roll.";
                TempData["type"] = "error";
                return View(await GetInfo(fFsRollReceiveViewModel));
            }
        }

        public async Task<FFsRollReceiveViewModel> GetInfo(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            fFsRollReceiveViewModel = await _fFsFabricRcvMaster.GetInitObjects(fFsRollReceiveViewModel);
            return fFsRollReceiveViewModel;
        }
        

        [HttpGet]
        public async Task<IActionResult> DetailsRollReceive(string id)
        {
            return View(await _fFsFabricRcvMaster.FindByRollRcvIdAsync(int.Parse(_protector.Unprotect(id))));
        }

        [HttpPost]
        public async Task<IActionResult> RollDetailsListByScan(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            try
            {
                var rollId = await _fFsFabricRcvDetails.GetRollIdByRollNo(fFsRollReceiveViewModel.FFsFabricRcvDetails.ROLL_NO);
                fFsRollReceiveViewModel.FFsFabricRcvDetails.ROLL_ID = rollId.ROLL_ID;

                var result = await _fFsFabricRcvDetails.FindRollDetails(fFsRollReceiveViewModel.FFsFabricRcvDetails.ROLL_ID ?? 0, (DateTime)fFsRollReceiveViewModel.FFsFabricRcvMaster.RCVDATE);
                var roll = await _fFsFabricRcvDetails.GetRollIDetails(fFsRollReceiveViewModel.FFsFabricRcvDetails
                    .ROLL_ID ?? 0);

                if (roll != null)
                {
                    Response.Headers["Status"] = "DB";
                }
                else if (result == null)
                {
                    Response.Headers["Status"] = "Null";
                }
                else if (!fFsRollReceiveViewModel.FFsFabricRcvDetailsList.Any(c =>
                    c.ROLL_ID.Equals(fFsRollReceiveViewModel.FFsFabricRcvDetails.ROLL_ID)))
                {
                    fFsRollReceiveViewModel = await _fFsFabricRcvDetails.GetRollsByScanAsync(fFsRollReceiveViewModel);
                    Response.Headers["Status"] = "Success";
                }
                else
                {
                    Response.Headers["Status"] = "Error";
                }

                fFsRollReceiveViewModel = await _fFsFabricRcvDetails.GetRollDetailsList(fFsRollReceiveViewModel);
                return PartialView($"RollDetailsList", fFsRollReceiveViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> RollDetailsList(DateTime rcvDate)
        {
            try
            {
                var fFsRollReceiveViewModel = await _fFsFabricRcvDetails.GetRollsAsync(rcvDate);
                return PartialView($"RollDetailsList", fFsRollReceiveViewModel);
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

                var data = await _fFsFabricRcvDetails.GetRollListAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                               m.FABCODENavigation.STYLE_NAME != null && m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue)
                                           || (m.RCV.RCVDATE != null && m.RCV.RCVDATE.ToString().Contains(searchValue))
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
        public async Task<F_FS_FABRIC_RCV_DETAILS> GetRollIDetails(int rollId)
        {
            try
            {
                var rollDetails = await _fFsFabricRcvDetails.GetRollIDetails(rollId);
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
                var rollDetails = await _fFsFabricRcvDetails.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (rollDetails != null)
                {
                    rollDetails.IS_QC_APPROVE = true;
                    rollDetails.QC_APPROVE_DATE = DateTime.Now;
                    rollDetails.IS_QC_REJECT = false;
                    await _fFsFabricRcvDetails.Update(rollDetails);
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
                var rollDetails = await _fFsFabricRcvDetails.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (rollDetails != null)
                {
                    rollDetails.IS_QC_APPROVE = false;
                    rollDetails.IS_QC_REJECT = true;
                    rollDetails.QC_REJECT_DATE = DateTime.Now;
                    await _fFsFabricRcvDetails.Update(rollDetails);
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
    }
}