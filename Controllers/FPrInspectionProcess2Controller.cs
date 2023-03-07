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
    public class FPrInspectionProcess2Controller : Controller
    {

        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_INSPECTION_PROCESS_MASTER _fPrInspectionProcessMaster;
        private readonly IF_PR_INSPECTION_PROCESS_DETAILS _fPrInspectionProcessDetails;
        private readonly IF_PR_INSPECTION_DEFECT_POINT _fPrInspectionDefectPoint;
        private readonly IF_PR_FINISHING_FNPROCESS _fPrFinishingFnProcess;
        private readonly IPL_PRODUCTION_SETDISTRIBUTION _plProductionSetDistribution;

        public FPrInspectionProcess2Controller(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_INSPECTION_PROCESS_MASTER fPrInspectionProcessMaster,
            IF_PR_INSPECTION_PROCESS_DETAILS fPrInspectionProcessDetails,
            IF_PR_INSPECTION_DEFECT_POINT fPrInspectionDefectPoint,
            IF_PR_FINISHING_FNPROCESS fPrFinishingFnProcess,
            IPL_PRODUCTION_SETDISTRIBUTION plProductionSetDistribution
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrInspectionProcessMaster = fPrInspectionProcessMaster;
            _fPrInspectionProcessDetails = fPrInspectionProcessDetails;
            _fPrInspectionDefectPoint = fPrInspectionDefectPoint;
            _fPrFinishingFnProcess = fPrFinishingFnProcess;
            _plProductionSetDistribution = plProductionSetDistribution;
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsRollNoInUse(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                var isRollNoExists = await _fPrInspectionProcessDetails.IsRollNoInUseAsync(prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO);
                return Json(isRollNoExists);
                //return !isRollNoExists ? Json(true) : Json($"Roll No [ {prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO} ] is already in use");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var recordsTotal = 0;

                var data = await _fPrInspectionProcessMaster.GetAllAsync();

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
                                               m.SET != null && m.SET.PROG_.PROG_NO.ToUpper().Contains(searchValue)
                                           || (m.INSPDATE != null && m.INSPDATE.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.INSPID.ToString());
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
        
        [HttpPost]
        public async Task<JsonResult> GetTableFindRollData()
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

                var data = await _fPrInspectionProcessDetails.GetAll();

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
                                               m.ROLLNO != null && m.ROLLNO.ToUpper().Contains(searchValue)
                                           || (m.ROLL_INSPDATE != null && m.ROLL_INSPDATE.ToString().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.ROLL_ID.ToString());
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

        //[HttpGet]
        //public IActionResult GetInspectionProcessList()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> CreateInspectionProcess()
        {
            try
            {
                var prInspectionProcessViewModel = await GetInfo(new FPrInspectionProcessViewModel());
                prInspectionProcessViewModel.FPrInspectionProcessMaster = new F_PR_INSPECTION_PROCESS_MASTER()
                {
                    INSPDATE = DateTime.Now.AddHours(-7),
                    CREATED_AT = DateTime.Now,
                    UPDATED_AT = DateTime.Now
                };
                prInspectionProcessViewModel.FPrInspectionProcessDetails = new F_PR_INSPECTION_PROCESS_DETAILS()
                {
                    ROLL_INSPDATE = DateTime.Now.AddHours(-7),
                    PROCESS_TYPE = 1,
                    REMARKS = "Shade & CSV Ok"
                };
                prInspectionProcessViewModel.FPrInspectionDefectPoint = new F_PR_INSPECTION_DEFECT_POINT()
                {
                    FindDate = DateTime.Now.AddHours(-7)
                };
                return View(prInspectionProcessViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [AcceptVerbs("Get", "Post")]
        [Route("FPrInspectionProcess/GetRollList/{search?}/{page?}/{fabcode}")]
        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollList(string search, int page, int fabcode)
        {
            try
            {
                var result = await _fPrInspectionProcessDetails.GetRollListByStyleDynamic(search, page, fabcode);
                return result;
            }
            catch (Exception)
            {
                TempData["message"] = "Inspection Process Not Found (Ex)";
                TempData["type"] = "error";
                return null;
            }
        }
        
        [HttpGet]
        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByStyle(int fabcode)
        {
            try
            {
                var result = await _fPrInspectionProcessDetails.GetRollListByStyle(fabcode);
                return result;
            }
            catch (Exception)
            {
                TempData["message"] = "Inspection Process Not Found (Ex)";
                TempData["type"] = "error";
                return null;
            }
        }

        [HttpGet]
        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByDate(DateTime? date)
        {
            try
            {
                var result = await _fPrInspectionProcessDetails.GetRollListByDate(date);
                return result;
            }
            catch (Exception)
            {
                TempData["message"] = "Inspection Process Not Found (Ex)";
                TempData["type"] = "error";
                return null;
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateInspectionProcess(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = await _userManager.GetUserAsync(User);
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.CREATED_BY = user.Id;
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.UPDATED_BY = user.Id;
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.CREATED_AT = DateTime.Now;
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.UPDATED_AT = DateTime.Now;
        //            var inspId = await _fPrInspectionProcessMaster.InsertAndGetIdAsync(prInspectionProcessViewModel.FPrInspectionProcessMaster);

        //            if (inspId != 0)
        //            {

        //                prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = inspId;
        //                prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_BY = user.Id;
        //                prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_BY = user.Id;
        //                prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_AT = DateTime.Now.Date;
        //                prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_AT = DateTime.Now.Date;
        //                var rollId = await _fPrInspectionProcessDetails.InsertAndGetIdAsync(prInspectionProcessViewModel.FPrInspectionProcessDetails);

        //                prInspectionProcessViewModel.FPrInspectionDefectPoint.ROLL_ID = rollId;
        //                prInspectionProcessViewModel.FPrInspectionDefectPoint.CREATED_BY = user.Id;
        //                prInspectionProcessViewModel.FPrInspectionDefectPoint.UPDATED_BY = user.Id;
        //                await _fPrInspectionDefectPoint.InsertByAsync(prInspectionProcessViewModel.FPrInspectionDefectPoint);

        //                TempData["message"] = "Successfully Inspection Rolls Created.";
        //                TempData["type"] = "success";

        //                var encryptedId = _protector.Protect(inspId.ToString());
        //                return RedirectToAction($"EditInspectionProcess", $"FPrInspectionProcess", new { id = encryptedId });
        //            }

        //            TempData["message"] = "Failed to Create Inspection Rolls.";
        //            TempData["type"] = "error";
        //            return View(await GetInfo(prInspectionProcessViewModel));
        //        }
        //        TempData["message"] = "Invalid Input. Please Try Again.";
        //        TempData["type"] = "error";
        //        return View(await GetInfo(prInspectionProcessViewModel));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        TempData["message"] = "Failed to Create Inspection Rolls.";
        //        TempData["type"] = "error";
        //        return View(await GetInfo(prInspectionProcessViewModel));
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateInspectionProcess(FPrInspectionProcessViewModel prInspectionProcessViewModel, bool status=true)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = await _userManager.GetUserAsync(User);
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.CREATED_BY = user.Id;
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.UPDATED_BY = user.Id;
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.CREATED_AT = DateTime.Now;
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.UPDATED_AT = DateTime.Now;
        //            var inspId = await _fPrInspectionProcessMaster.InsertAndGetIdAsync(prInspectionProcessViewModel.FPrInspectionProcessMaster);

        //            if (inspId != 0)
        //            {
        //                foreach (var item in prInspectionProcessViewModel.FPrInspectionProcessDetailsList)
        //                {
        //                    item.INSPID = inspId;
        //                    item.CREATED_BY = user.Id;
        //                    item.UPDATED_BY = user.Id;
        //                    item.CREATED_AT = DateTime.Now.Date;
        //                    item.UPDATED_AT = DateTime.Now.Date;
        //                    var rollId = await _fPrInspectionProcessDetails.InsertAndGetIdAsync(item);
        //                    foreach (var i in item.FPrInspectionDefectPointsList)
        //                    {
        //                        i.ROLL_ID = rollId;
        //                        i.CREATED_BY = user.Id;
        //                        i.UPDATED_BY = user.Id;
        //                        await _fPrInspectionDefectPoint.InsertByAsync(i);
        //                    }
        //                }
        //                TempData["message"] = "Successfully Inspection Rolls Created.";
        //                TempData["type"] = "success";
        //                if (status)
        //                {
        //                    return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
        //                }

        //                //var encryptedId = _protector.Protect(inspId.ToString());
        //                //return RedirectToAction(nameof(EditInspectionProcess), $"FPrInspectionProcess", new { id = encryptedId });
        //            }

        //            TempData["message"] = "Failed to Create Inspection Rolls.";
        //            TempData["type"] = "error";
        //            return View(await GetInfo(prInspectionProcessViewModel));
        //        }
        //        TempData["message"] = "Invalid Input. Please Try Again.";
        //        TempData["type"] = "error";
        //        return View(await GetInfo(prInspectionProcessViewModel));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        TempData["message"] = "Failed to Create Inspection Rolls.";
        //        TempData["type"] = "error";
        //        return View(await GetInfo(prInspectionProcessViewModel));
        //    }
        //}
        
        [HttpGet]
        public async Task<IActionResult> RollFindProcess(int id)
        {
            try
            {
                var prInspectionProcessViewModel = new FPrInspectionProcessViewModel
                {
                    FPrInspectionProcessMaster = await _fPrInspectionProcessMaster.FindByIdAllAsync(id)
                };

                if (prInspectionProcessViewModel.FPrInspectionProcessMaster != null)
                {
                    prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
                    prInspectionProcessViewModel.FPrInspectionProcessMaster.EncryptedId = _protector.Protect(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID.ToString());

                    //prInspectionProcessViewModel.FPrInspectionProcessDetails = new F_PR_INSPECTION_PROCESS_DETAILS()
                    //{
                    //    ROLL_INSPDATE = DateTime.Now.AddHours(-7),
                    //    LENGTH_1 = 0,
                    //    LENGTH_2 = 0,
                    //    PROCESS_TYPE = 1,
                    //    CUT_PCS_SECTION = 175
                    //};

                    //prInspectionProcessViewModel.FPrInspectionProcessDetails = await _fPrInspectionProcessDetails.GetRollDetailsByInsIdAsync(id);

                    var details = await _fPrInspectionProcessDetails.GetRollDetailsByInsIdAsync(id);
                    prInspectionProcessViewModel.FPrInspectionProcessDetails = details ?? new F_PR_INSPECTION_PROCESS_DETAILS();
                    prInspectionProcessViewModel.FPrInspectionDefectPoint = new F_PR_INSPECTION_DEFECT_POINT
                    {
                         StyleName = details != null
                             ? details.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE
                             : null
                    };
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList = await _fPrInspectionProcessDetails.GetDefectListByInsIdAsync(prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO);


                    prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);

                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.PROCESS_TYPE = 1;
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_INSPDATE = DateTime.Now.AddHours(-7);
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_1 = null;
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_2 = null;
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.REMARKS ="Shade & CSV Ok";

                    return View($"EditInspectionProcess", prInspectionProcessViewModel);
                }

                TempData["message"] = "Inspection Process Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
            }
            catch (Exception)
            {
                TempData["message"] = "Inspection Process Not Found (Ex)";
                TempData["type"] = "error";
                return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> RollFindCopyProcess(int id)
        {
            try
            {
                var prInspectionProcessViewModel = new FPrInspectionProcessViewModel
                {
                    FPrInspectionProcessMaster = await _fPrInspectionProcessMaster.FindByIdAllAsync(id)
                };

                if (prInspectionProcessViewModel.FPrInspectionProcessMaster != null)
                {
                    prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
                    prInspectionProcessViewModel.FPrInspectionProcessMaster.EncryptedId = _protector.Protect(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID.ToString());

                    //prInspectionProcessViewModel.FPrInspectionProcessDetails = new F_PR_INSPECTION_PROCESS_DETAILS()
                    //{
                    //    ROLL_INSPDATE = DateTime.Now.AddHours(-7),
                    //    LENGTH_1 = 0,
                    //    LENGTH_2 = 0,
                    //    PROCESS_TYPE = 1,
                    //    CUT_PCS_SECTION = 175
                    //};

                    //prInspectionProcessViewModel.FPrInspectionProcessDetails = await _fPrInspectionProcessDetails.GetRollDetailsByInsIdAsync(id);

                    var details = await _fPrInspectionProcessDetails.GetRollDetailsByInsIdAsync(id);
                    prInspectionProcessViewModel.FPrInspectionProcessDetails = details ?? new F_PR_INSPECTION_PROCESS_DETAILS();
                    prInspectionProcessViewModel.FPrInspectionDefectPoint = new F_PR_INSPECTION_DEFECT_POINT
                    {
                         StyleName = details != null
                             ? details.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE
                             : null
                    };
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList = await _fPrInspectionProcessDetails.GetDefectListByInsIdAsync(prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO);


                    prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);

                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.PROCESS_TYPE = 1;
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_INSPDATE = DateTime.Now.AddHours(-7);
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_1 = null;
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_2 = null;
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.REMARKS ="Shade & CSV Ok";

                    return View($"EditInspectionProcess", prInspectionProcessViewModel);
                }

                TempData["message"] = "Inspection Process Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
            }
            catch (Exception)
            {
                TempData["message"] = "Inspection Process Not Found (Ex)";
                TempData["type"] = "error";
                return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> EditInspectionProcess(string id)
        //{
        //    try
        //    {
        //        var insId = int.Parse(_protector.Unprotect(id));
        //        var prInspectionProcessViewModel = new FPrInspectionProcessViewModel
        //        {
        //            FPrInspectionProcessMaster = await _fPrInspectionProcessMaster.FindByIdAsync(insId)
        //        };

        //        if (prInspectionProcessViewModel.FPrInspectionProcessMaster != null)
        //        {
        //            prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
        //            prInspectionProcessViewModel.FPrInspectionProcessMaster.EncryptedId = _protector.Protect(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID.ToString());

        //            //prInspectionProcessViewModel.FPrInspectionProcessDetails = new F_PR_INSPECTION_PROCESS_DETAILS()
        //            //{
        //            //    ROLL_INSPDATE = DateTime.Now.AddHours(-7),
        //            //    LENGTH_1 = 0,
        //            //    LENGTH_2 = 0,
        //            //    PROCESS_TYPE = 1,
        //            //    CUT_PCS_SECTION = 175
        //            //};

        //            prInspectionProcessViewModel.FPrInspectionProcessDetailsList = await _fPrInspectionProcessDetails.GetRollListByInsIdAsync(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID);

        //            var details =
        //                await _fPrInspectionProcessDetails.GetDefectDetailsByInsIdAsync(prInspectionProcessViewModel
        //                    .FPrInspectionProcessMaster.INSPID);
        //            prInspectionProcessViewModel.FPrInspectionProcessDetails = details ?? new F_PR_INSPECTION_PROCESS_DETAILS();

        //            prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList = await _fPrInspectionProcessDetails.GetDefectListByInsIdAsync(prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO);


        //            prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);

        //            //prInspectionProcessViewModel.FPrInspectionProcessDetails.PROCESS_TYPE = 1;
        //            //prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_INSPDATE = DateTime.Now.AddHours(-7);
        //            prInspectionProcessViewModel.FPrInspectionDefectPoint = new F_PR_INSPECTION_DEFECT_POINT()
        //            {
        //                FindDate = DateTime.Now.AddHours(-7)
        //            };
        //            //prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_1 = null;
        //            //prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_2 = null;
        //            //prInspectionProcessViewModel.FPrInspectionProcessDetails.REMARKS ="Shade & CSV Ok";

        //            return View(prInspectionProcessViewModel);
        //        }

        //        TempData["message"] = "Inspection Process Not Found";
        //        TempData["type"] = "error";
        //        return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["message"] = "Inspection Process Not Found (Ex)";
        //        TempData["type"] = "error";
        //        return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditInspectionProcess(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var insId = int.Parse(_protector.Unprotect(prInspectionProcessViewModel.FPrInspectionProcessMaster.EncryptedId));
        //            if (prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID == insId)
        //            {
        //                var inspectionDetails = await _fPrInspectionProcessMaster.FindByIdAsync(insId);

        //                var user = await _userManager.GetUserAsync(User);
        //                prInspectionProcessViewModel.FPrInspectionProcessMaster.UPDATED_BY = user.Id;
        //                prInspectionProcessViewModel.FPrInspectionProcessMaster.UPDATED_AT = DateTime.Now;
        //                prInspectionProcessViewModel.FPrInspectionProcessMaster.CREATED_AT = inspectionDetails.CREATED_AT;
        //                prInspectionProcessViewModel.FPrInspectionProcessMaster.CREATED_BY = inspectionDetails.CREATED_BY;

        //                var isUpdated = await _fPrInspectionProcessMaster.Update(prInspectionProcessViewModel.FPrInspectionProcessMaster);
        //                if (isUpdated)
        //                {
        //                    foreach (var item in prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Where(e => e.ROLL_ID == 0))
        //                    {
        //                        item.INSPID = insId;
        //                        item.CREATED_BY = user.Id;
        //                        item.UPDATED_BY = user.Id;
        //                        item.CREATED_AT = DateTime.Now.Date;
        //                        item.UPDATED_AT = DateTime.Now.Date;
        //                        var rollId = await _fPrInspectionProcessDetails.InsertAndGetIdAsync(item);
        //                        foreach (var i in item.FPrInspectionDefectPointsList.Where(c => c.DPID == 0))
        //                        {
        //                            i.ROLL_ID = rollId;
        //                            i.CREATED_AT = DateTime.Now;
        //                            i.CREATED_BY = user.Id;
        //                            i.UPDATED_AT = DateTime.Now;
        //                            i.UPDATED_BY = user.Id;
        //                            await _fPrInspectionDefectPoint.InsertByAsync(i);
        //                        }
        //                    }
        //                    TempData["message"] = "Successfully Updated Inspection Details.";
        //                    TempData["type"] = "success";
        //                    return RedirectToAction("GetInspectionProcessList", $"FPrInspectionProcess");
        //                }
        //                TempData["message"] = "Failed to Update Inspection Details.";
        //                TempData["type"] = "error";
        //                prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
        //                return View(prInspectionProcessViewModel);
        //            }
        //            TempData["message"] = "Invalid Inspection Info.";
        //            TempData["type"] = "error";
        //            prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
        //            return View(prInspectionProcessViewModel);
        //        }
        //        TempData["message"] = "Invalid Input. Please Try Again.";
        //        TempData["type"] = "error";
        //        prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
        //        return View(prInspectionProcessViewModel);
        //    }
        //    catch (Exception)
        //    {
        //        TempData["message"] = "Failed to Update Inspection.";
        //        TempData["type"] = "error";
        //        prInspectionProcessViewModel = await GetInfo(prInspectionProcessViewModel);
        //        return View(prInspectionProcessViewModel);
        //    }
        //}RemoveDefectFromRollListA

        public async Task<FPrInspectionProcessViewModel> GetInfo(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            prInspectionProcessViewModel = await _fPrInspectionProcessMaster.GetInitObjects(prInspectionProcessViewModel);
            return prInspectionProcessViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetailsList(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var roll = await _fPrInspectionProcessDetails.IsRollNoExists(prInspectionProcessViewModel
                    .FPrInspectionProcessDetails.ROLLNO);

                //if (!roll)
                //{
                //    prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList =
                //        await _fPrInspectionProcessDetails.GetDefectListByInsIdAsync(prInspectionProcessViewModel
                //            .FPrInspectionProcessDetails.ROLLNO);
                //    prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                //    return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
                //}

                var set = await _fPrInspectionProcessDetails.IsSetNoExists(prInspectionProcessViewModel.FPrInspectionProcessMaster.SETID ?? 0);
                if (roll)
                {
                    var item = await _fPrInspectionProcessDetails.FindByRollNoAsync(prInspectionProcessViewModel
                        .FPrInspectionProcessDetails.ROLLNO);
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_ID = item.ROLL_ID;

                    var flag = prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Any(c => c.DEF_TYPEID.Equals(prInspectionProcessViewModel.FPrInspectionDefectPoint.DEF_TYPEID));

                    if (!flag && prInspectionProcessViewModel.FPrInspectionDefectPoint.DEF_TYPEID!=null)
                    {
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.ROLL_ID = item.ROLL_ID;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.CREATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.UPDATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.DPID = 0;

                        prInspectionProcessViewModel.FPrInspectionDefectPoint =
                            await _fPrInspectionDefectPoint.GetInsertedObjByAsync(prInspectionProcessViewModel
                                .FPrInspectionDefectPoint);

                        prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Add(prInspectionProcessViewModel.FPrInspectionDefectPoint);
                    }

                    prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;

                    var inspDetails = await _fPrInspectionProcessDetails.FindByIdAsync(prInspectionProcessViewModel
                        .FPrInspectionProcessDetails.ROLL_ID);

                    prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_AT = inspDetails.CREATED_AT;
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_BY = inspDetails.CREATED_BY;
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_AT = DateTime.Now;
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_BY = user.Id;
                    //prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_INSPDATE = prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_INSPDATE;

                    await _fPrInspectionProcessDetails.Update(prInspectionProcessViewModel
                        .FPrInspectionProcessDetails);
                    return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
                }
                else
                {
                    if (set!=null)
                    {
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_AT = DateTime.Now.Date;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_AT = DateTime.Now.Date;
                        if (!await _fPrInspectionProcessDetails.IsRollNoExists(prInspectionProcessViewModel
                            .FPrInspectionProcessDetails.ROLLNO))
                        {
                            prInspectionProcessViewModel.FPrInspectionProcessDetails =
                                await _fPrInspectionProcessDetails.GetInsertedObjByAsync(prInspectionProcessViewModel
                                    .FPrInspectionProcessDetails);
                        }
                        

                        prInspectionProcessViewModel.FPrInspectionDefectPoint.ROLL_ID = prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_ID;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.CREATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.UPDATED_BY = user.Id;

                        prInspectionProcessViewModel.FPrInspectionDefectPoint =
                            await _fPrInspectionDefectPoint.GetInsertedObjByAsync(prInspectionProcessViewModel
                                .FPrInspectionDefectPoint);


                        prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Add(prInspectionProcessViewModel.FPrInspectionDefectPoint);
                        prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Add(prInspectionProcessViewModel.FPrInspectionProcessDetails);

                        prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.Operator = null;
                        await _fPrInspectionProcessDetails.Update(prInspectionProcessViewModel
                            .FPrInspectionProcessDetails);
                        return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
                    }
                    else
                    {
                        prInspectionProcessViewModel.FPrInspectionProcessMaster =
                            await _fPrInspectionProcessMaster.GetInsertedObjByAsync(prInspectionProcessViewModel
                                .FPrInspectionProcessMaster);

                        Response.Headers["Id"] =
                            _protector.Protect(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID.ToString());


                        prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_AT = DateTime.Now.Date;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_AT = DateTime.Now.Date;
                        prInspectionProcessViewModel.FPrInspectionProcessDetails =
                            await _fPrInspectionProcessDetails.GetInsertedObjByAsync(prInspectionProcessViewModel
                                .FPrInspectionProcessDetails);
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.ROLL_ID = prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_ID;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.CREATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.UPDATED_BY = user.Id;

                        prInspectionProcessViewModel.FPrInspectionDefectPoint =
                            await _fPrInspectionDefectPoint.GetInsertedObjByAsync(prInspectionProcessViewModel
                                .FPrInspectionDefectPoint);

                        
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Add(prInspectionProcessViewModel.FPrInspectionDefectPoint);
                        prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Add(prInspectionProcessViewModel.FPrInspectionProcessDetails);


                        prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;
                        await _fPrInspectionProcessDetails.Update(prInspectionProcessViewModel
                            .FPrInspectionProcessDetails);
                        return PartialView($"AddDefectPoint", prInspectionProcessViewModel);

                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRollList(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                var result = prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Any(c =>
                    c.ROLLNO.Equals(prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO));

                var user = await _userManager.GetUserAsync(User);

                if (result)
                {
                    var item = prInspectionProcessViewModel.FPrInspectionProcessDetailsList.FirstOrDefault(c => c.ROLLNO.Equals(prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLLNO));

                    var flag = item.FPrInspectionDefectPointsList.Any(c => c.DEF_TYPEID.Equals(prInspectionProcessViewModel.FPrInspectionDefectPoint.DEF_TYPEID));

                    if (!flag)
                    {
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.ROLL_ID = item.ROLL_ID;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.CREATED_BY = user.Id;
                        prInspectionProcessViewModel.FPrInspectionDefectPoint.UPDATED_BY = user.Id;

                        prInspectionProcessViewModel.FPrInspectionDefectPoint =
                            await _fPrInspectionDefectPoint.GetInsertedObjByAsync(prInspectionProcessViewModel
                                .FPrInspectionDefectPoint);

                        item.FPrInspectionDefectPointsList.Add(prInspectionProcessViewModel.FPrInspectionDefectPoint);
                    }

                    prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                    await _fPrInspectionProcessDetails.UpdateRangeByAsync(prInspectionProcessViewModel
                        .FPrInspectionProcessDetailsList);
                    await _fPrInspectionProcessMaster.Update(prInspectionProcessViewModel.FPrInspectionProcessMaster);
                    return PartialView($"AddRollList", prInspectionProcessViewModel);
                }

                if (prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID == 0)
                {
                    prInspectionProcessViewModel.FPrInspectionProcessMaster =
                        await _fPrInspectionProcessMaster.GetInsertedObjByAsync(prInspectionProcessViewModel
                            .FPrInspectionProcessMaster);

                    Response.Headers["Id"] =
                        _protector.Protect(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID.ToString());
                }
                prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;
                prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_BY = user.Id;
                prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_BY = user.Id;
                prInspectionProcessViewModel.FPrInspectionProcessDetails.CREATED_AT = DateTime.Now.Date;
                prInspectionProcessViewModel.FPrInspectionProcessDetails.UPDATED_AT = DateTime.Now.Date;
                prInspectionProcessViewModel.FPrInspectionProcessDetails =
                    await _fPrInspectionProcessDetails.GetInsertedObjByAsync(prInspectionProcessViewModel
                        .FPrInspectionProcessDetails);
                prInspectionProcessViewModel.FPrInspectionDefectPoint.ROLL_ID = prInspectionProcessViewModel.FPrInspectionProcessDetails.ROLL_ID;
                prInspectionProcessViewModel.FPrInspectionDefectPoint.CREATED_BY = user.Id;
                prInspectionProcessViewModel.FPrInspectionDefectPoint.UPDATED_BY = user.Id;

                prInspectionProcessViewModel.FPrInspectionDefectPoint =
                    await _fPrInspectionDefectPoint.GetInsertedObjByAsync(prInspectionProcessViewModel
                        .FPrInspectionDefectPoint);


                prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Add(prInspectionProcessViewModel.FPrInspectionDefectPoint);
                prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Add(prInspectionProcessViewModel.FPrInspectionProcessDetails);

                //if (prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Count == 1 &&
                //    prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Select(c =>
                //        c.FPrInspectionDefectPointsList).Count() == 1)
                //{
                //    await CreateInspectionProcess(prInspectionProcessViewModel,false);
                //}

                prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                await _fPrInspectionProcessDetails.UpdateRangeByAsync(prInspectionProcessViewModel
                    .FPrInspectionProcessDetailsList);
                return PartialView($"AddRollList", prInspectionProcessViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                return PartialView($"AddRollList", prInspectionProcessViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetDetailsList(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList = null;

                return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList = null;
                return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRollFromList(FPrInspectionProcessViewModel prInspectionProcessViewModel, string removeIndexValue)
        {
            ModelState.Clear();


            if (prInspectionProcessViewModel.FPrInspectionProcessDetailsList[int.Parse(removeIndexValue)]
                .ROLL_ID > 0)
            {
                await _fPrInspectionDefectPoint.DeleteRange(prInspectionProcessViewModel
                    .FPrInspectionProcessDetailsList[int.Parse(removeIndexValue)].FPrInspectionDefectPointsList.Where(c => c.DPID > 0));

                await _fPrInspectionProcessDetails.Delete(prInspectionProcessViewModel
                    .FPrInspectionProcessDetailsList[int.Parse(removeIndexValue)]);
            }

            prInspectionProcessViewModel.FPrInspectionProcessDetailsList.RemoveAt(int.Parse(removeIndexValue));
            prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
            await _fPrInspectionProcessDetails.UpdateRangeByAsync(prInspectionProcessViewModel
                .FPrInspectionProcessDetailsList);
            return PartialView($"AddRollList", prInspectionProcessViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDefectFromRollList(FPrInspectionProcessViewModel prInspectionProcessViewModel, string removeIndexValue, string setRemoveIndex)
        {
            ModelState.Clear();

            if (prInspectionProcessViewModel.FPrInspectionProcessDetailsList[int.Parse(removeIndexValue)]
                .FPrInspectionDefectPointsList[int.Parse(setRemoveIndex)].DPID > 0)
            {
                await _fPrInspectionDefectPoint.Delete(prInspectionProcessViewModel
                    .FPrInspectionProcessDetailsList[int.Parse(removeIndexValue)]
                    .FPrInspectionDefectPointsList[int.Parse(setRemoveIndex)]);
            }

            prInspectionProcessViewModel.FPrInspectionProcessDetailsList[int.Parse(removeIndexValue)]
                .FPrInspectionDefectPointsList.RemoveAt(int.Parse(setRemoveIndex));
            prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
            await _fPrInspectionProcessDetails.UpdateRangeByAsync(prInspectionProcessViewModel
                .FPrInspectionProcessDetailsList);
            return PartialView($"AddRollList", prInspectionProcessViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDefectFromRollListA(FPrInspectionProcessViewModel prInspectionProcessViewModel, string setRemoveIndex)
        {
            ModelState.Clear();

            if (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList[int.Parse(setRemoveIndex)].DPID > 0)
            {
                await _fPrInspectionDefectPoint.Delete(prInspectionProcessViewModel.FPrInspectionProcessDetails
                    .FPrInspectionDefectPointsList[int.Parse(setRemoveIndex)]);
            }

            prInspectionProcessViewModel.FPrInspectionProcessDetails
                .FPrInspectionDefectPointsList.RemoveAt(int.Parse(setRemoveIndex));
            prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
            prInspectionProcessViewModel.FPrInspectionProcessDetails.INSPID = prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID;
            await _fPrInspectionProcessDetails.Update(prInspectionProcessViewModel
                .FPrInspectionProcessDetails);
            return PartialView($"AddDefectPoint", prInspectionProcessViewModel);
        }
        [HttpPost]
        public async Task<string> GetRollNoBySetId(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                var rollNo = "";
                if (prInspectionProcessViewModel.FPrInspectionProcessDetailsList.Any())
                {
                    rollNo = prInspectionProcessViewModel.FPrInspectionProcessDetailsList
                        .Max(c => (long.Parse(c.ROLLNO) + 1)).ToString();

                    rollNo = rollNo.Substring(0, 8);

                }
                else
                {
                    rollNo = await _fPrInspectionProcessMaster.GetRollNoBySetId(prInspectionProcessViewModel
                        .FPrInspectionProcessMaster.SETID);
                }
                return rollNo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetRollList(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                ModelState.Clear();
                prInspectionProcessViewModel.FPrInspectionProcessDetailsList = await _fPrInspectionProcessDetails.GetRollListByInsIdAsync(prInspectionProcessViewModel.FPrInspectionProcessMaster.INSPID);
                prInspectionProcessViewModel = await GetNamesAsync(prInspectionProcessViewModel);
                return PartialView($"AddRollList", prInspectionProcessViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionProcessViewModel> GetNamesAsync(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                prInspectionProcessViewModel = await _fPrInspectionProcessDetails.GetInitData(prInspectionProcessViewModel);
                var totalDefect = 0;
                //foreach (var item in prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList)
                //{
                if (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Any())
                {
                    totalDefect =
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT1 ?? 0) * 1) +
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT2 ?? 0) * 2) +
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT3 ?? 0) * 3) +
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT4 ?? 0) * 4);

                    prInspectionProcessViewModel.FPrInspectionProcessDetails.TOTAL_DEFECT = totalDefect;

                    if (prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH != null)
                    {
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.POINT_100SQ = Math.Round((double)(((totalDefect * 3600) / prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH) / prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_YDS), 2);
                    }
                }
                //}

                Response.Headers["TotalDefect"] = totalDefect.ToString();
                //Response.Headers["TotalDefect"] = totalDefect.ToString();


                return prInspectionProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public FPrInspectionProcessViewModel GetPointInfo(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            var totalDefect = 0;

            if (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Count()!=0)
            {
                //foreach (var item in prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList)
                //{
                    totalDefect =
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT1 ?? 0) * 1) +
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT2 ?? 0) * 2) +
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT3 ?? 0) * 3) +
                        (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList.Sum(c => c.POINT4 ?? 0) * 4);
                    totalDefect = totalDefect != 0 ? totalDefect : prInspectionProcessViewModel.FPrInspectionProcessDetails.TOTAL_DEFECT ?? 0;
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.TOTAL_DEFECT = totalDefect;
                    
                    if (prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH != 0 &&
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH != null)
                    {
                        prInspectionProcessViewModel.FPrInspectionProcessDetails.POINT_100SQ = Math.Round(
                            (double)(((totalDefect * 3600) /
                                      prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH) /
                                     prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_YDS), 2);
                    }
                    //}
            }
            else
            {
                totalDefect =
                    ((prInspectionProcessViewModel.FPrInspectionDefectPoint.POINT1 ?? 0) * 1) +
                    ((prInspectionProcessViewModel.FPrInspectionDefectPoint.POINT2 ?? 0) * 2) +
                    ((prInspectionProcessViewModel.FPrInspectionDefectPoint.POINT3 ?? 0) * 3) +
                    ((prInspectionProcessViewModel.FPrInspectionDefectPoint.POINT4 ?? 0) * 4);
                prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH ??= 0;

                totalDefect = totalDefect != 0 ? totalDefect : prInspectionProcessViewModel.FPrInspectionProcessDetails.TOTAL_DEFECT ?? 0;

                prInspectionProcessViewModel.FPrInspectionProcessDetails.TOTAL_DEFECT = totalDefect;
                if (prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH != 0 && prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH!=null)
                {
                    prInspectionProcessViewModel.FPrInspectionProcessDetails.POINT_100SQ = Math.Round((double)(((totalDefect * 3600) / prInspectionProcessViewModel.FPrInspectionProcessDetails.ACT_WIDTH_INCH) / prInspectionProcessViewModel.FPrInspectionProcessDetails.LENGTH_YDS), 2);
                }

            }



            //Response.Headers["TotalDefect"] = totalDefect.ToString();


            return prInspectionProcessViewModel;
        }

        public async Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetTrollyListBySetId(int setId)
        {
            try
            {
                var result = await _fPrInspectionProcessDetails.GetTrollyListBySetId(setId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetTrollyListBySetIdEdit(int setId)
        {
            try
            {
                var result = await _fPrInspectionProcessDetails.GetTrollyListBySetIdEdit(setId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_FINISHING_FNPROCESS> GetTrollyDetails(int trollyId, int setId)
        {
            try
            {
                var result = await _fPrInspectionProcessDetails.GetTrollyDetails(trollyId, setId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<RndProductionOrderDetailViewModel> GetSetDetails(string setId)
        {
            var result = await _fPrInspectionProcessMaster.GetSetDetails(int.Parse(setId));
            return result;
        }


        [HttpGet]
        public async Task<bool> GetRollConfirm(string roll)
        {
            var result = await _fPrInspectionProcessMaster.GetRollConfirm(roll);
            return result;
        }

        //[HttpGet]
        //public IActionResult RRollStickerReport(string rollId)
        //{
        //    return View(model: rollId);
        //}

        //[HttpGet]
        //public IActionResult RRollStickerLoReport(string rollId)
        //{
        //    return View(model: rollId);
        //}

        [HttpGet]
        public async Task<dynamic> GetRemarks()
        {
            try
            {
                return await _fPrInspectionProcessMaster.GetRemarks();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<dynamic> GetConstruction()
        {
            try
            {
                return await _fPrInspectionProcessMaster.GetConstruction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet]
        public async Task<string> DeleteRoll(string rollNo)
        {
            ModelState.Clear();

            var fPrInspectionProcessDetails = await _fPrInspectionProcessDetails.FindByRollNoAsync(rollNo);

            if (fPrInspectionProcessDetails.F_FS_FABRIC_RCV_DETAILS.Count==0 && fPrInspectionProcessDetails.F_FS_FABRIC_CLEARANCE_DETAILS.Count==0 //&& fPrInspectionProcessDetails.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL.Count==0
            )
            {
                if (await _fPrInspectionDefectPoint.DeleteRange(
                    fPrInspectionProcessDetails.F_PR_INSPECTION_DEFECT_POINT))
                {
                    await _fPrInspectionProcessDetails.Delete(fPrInspectionProcessDetails);
                    return "Success";
                }
                else
                {
                    return "Can't Delete Point!";
                }
            }

            return "Roll Already engaged with Fabric store or Clearance or Master Roll!!";
        }


    }
}