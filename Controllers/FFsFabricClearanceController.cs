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
    public class FFsFabricClearanceController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_FS_FABRIC_CLEARANCE_MASTER _fFsFabricClearanceMaster;
        private readonly IF_FS_FABRIC_CLEARANCE_DETAILS _fFsFabricClearanceDetails;
        private readonly IF_PR_INSPECTION_PROCESS_DETAILS _fPrInspectionProcessDetails;

        public FFsFabricClearanceController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_FS_FABRIC_CLEARANCE_MASTER fFsFabricClearanceMaster,
            IF_FS_FABRIC_CLEARANCE_DETAILS fFsFabricClearanceDetails,
            IF_PR_INSPECTION_PROCESS_DETAILS fPrInspectionProcessDetails
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fFsFabricClearanceMaster = fFsFabricClearanceMaster;
            _fFsFabricClearanceDetails = fFsFabricClearanceDetails;
            _fPrInspectionProcessDetails = fPrInspectionProcessDetails;
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

                var data = await _fFsFabricClearanceMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.FABCODENavigation != null && m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue) 
                                           || m.WASH_CODE != null && m.WASH_CODE.ToString().Contains(searchValue)
                                           || m.PO != null && m.PO.SO.SO_NO.ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = finalData
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetFabricClearanceList()
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
        public async Task<IActionResult> CreateFabricClearance()
        {
            var fFsFabricClearanceViewModel = await GetInfo(new FFsFabricClearanceViewModel());
            fFsFabricClearanceViewModel.FFsFabricClearanceMaster = new F_FS_FABRIC_CLEARANCE_MASTER()
            {
                TRNSDATE = DateTime.Now,
                PACKING_LIST_DATE = DateTime.Now
            };

            return View(fFsFabricClearanceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFabricClearance(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var flag = await _fFsFabricClearanceMaster.WashcodeAnyAsync(fFsFabricClearanceViewModel);
                    if (!flag)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CREATED_BY = user.Id;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.UPDATED_BY = user.Id;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CREATED_AT = DateTime.Now;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.UPDATED_AT = DateTime.Now;
                        var clearance = await _fFsFabricClearanceMaster.GetInsertedObjByAsync(fFsFabricClearanceViewModel.FFsFabricClearanceMaster);

                        if (clearance != null)
                        {
                            foreach (var item in fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList)
                            {
                                item.CLID = clearance.CLID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;

                                if (item.STATUS == 0)
                                {
                                    item.STATUS = 3;
                                }
                            }
                            await _fFsFabricClearanceDetails.InsertRangeByAsync(fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList);
                            TempData["message"] = "Successfully Clearance Created.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFabricClearanceList", $"FFsFabricClearance");
                        }
                        TempData["message"] = "Failed to Create Clearance";
                        TempData["type"] = "error";
                        return View(await GetInfo(fFsFabricClearanceViewModel));
                    }
                    else
                    {
                        TempData["message"] = "Duplicate Wash Code";
                        TempData["type"] = "warning";

                        ModelState.Clear();
                        fFsFabricClearanceViewModel = await _fFsFabricClearanceDetails.SetRollStatus(fFsFabricClearanceViewModel);
                        ModelState.Clear();
                        return View(await GetInfo(fFsFabricClearanceViewModel));
                    }

                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fFsFabricClearanceViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Clearance";
                TempData["type"] = "error";
                return View(await GetInfo(fFsFabricClearanceViewModel));
            }
        }

        public async Task<FFsFabricClearanceViewModel> GetInfo(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            fFsFabricClearanceViewModel = await _fFsFabricClearanceMaster.GetInitObjects(fFsFabricClearanceViewModel);
            return fFsFabricClearanceViewModel;
        }

        public async Task<IActionResult> AddRollList(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                ModelState.Clear();
                fFsFabricClearanceViewModel = await _fFsFabricClearanceDetails.GetRollDetailsAsync(fFsFabricClearanceViewModel);
                ModelState.Clear();
                return PartialView($"AddRollList", fFsFabricClearanceViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IActionResult> SetRollStatus(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                ModelState.Clear();
                fFsFabricClearanceViewModel = await _fFsFabricClearanceDetails.SetRollStatus(fFsFabricClearanceViewModel);
                ModelState.Clear();
                return PartialView($"AddRollList", fFsFabricClearanceViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IActionResult> AddRollListEdit(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                ModelState.Clear();
                fFsFabricClearanceViewModel = await _fFsFabricClearanceDetails.GetRollDetailsEditAsync(fFsFabricClearanceViewModel);
                ModelState.Clear();
                return PartialView($"AddRollList", fFsFabricClearanceViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRollFromList(FFsFabricClearanceViewModel fFsFabricClearanceViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();

                if (fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList[int.Parse(removeIndexValue)]
                    .CL_D_ID > 0)
                {
                    await _fFsFabricClearanceDetails.Delete(fFsFabricClearanceViewModel
                        .FFsFabricClearanceDetailsList[int.Parse(removeIndexValue)]);
                }

                fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.RemoveAt(int.Parse(removeIndexValue));
                fFsFabricClearanceViewModel = await _fFsFabricClearanceDetails.GetDetailsAsync(fFsFabricClearanceViewModel);
                return PartialView($"AddRollList", fFsFabricClearanceViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpGet]
        public async Task<dynamic> GetOrderDetails(int id)
        {
            try
            {
                var result = await _fFsFabricClearanceMaster.GetOrderDetaiils(id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditFabricClearance(string id)
        {
            try
            {
                var sId = int.Parse(_protector.Unprotect(id));
                var fFsFabricClearanceViewModel = await _fFsFabricClearanceMaster.FindAllByIdAsync(sId);

                if (fFsFabricClearanceViewModel.FFsFabricClearanceMaster != null)
                {
                    fFsFabricClearanceViewModel = await GetInfo(fFsFabricClearanceViewModel);
                    fFsFabricClearanceViewModel.FFsFabricClearanceMaster.EncryptedId = _protector.Protect(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CLID.ToString());

                    return View(fFsFabricClearanceViewModel);
                }

                TempData["message"] = "Clearance Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFabricClearanceList", $"FFsFabricClearance");
            }
            catch (Exception)
            {
                TempData["message"] = "Clearance Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFabricClearanceList", $"FFsFabricClearance");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFabricClearance(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var clId = int.Parse(_protector.Unprotect(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.EncryptedId));
                    if (fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CLID == clId)
                    {
                        var sizingDetails = await _fFsFabricClearanceMaster.FindByIdAsync(clId);

                        var user = await _userManager.GetUserAsync(User);

                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.FABCODE = sizingDetails.FABCODE;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.UPDATED_BY = user.Id;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.UPDATED_AT = DateTime.Now;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CREATED_AT = sizingDetails.CREATED_AT;
                        fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CREATED_BY = sizingDetails.CREATED_BY;

                        var isUpdated = await _fFsFabricClearanceMaster.Update(fFsFabricClearanceViewModel.FFsFabricClearanceMaster);
                        if (isUpdated)
                        {
                            foreach (var item in fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Where(c => c.CL_D_ID.Equals(0)))
                            {
                                item.CLID = fFsFabricClearanceViewModel.FFsFabricClearanceMaster.CLID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fFsFabricClearanceDetails.InsertByAsync(item);
                            }

                            foreach (var item in fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Where(c => !c.CL_D_ID.Equals(0)))
                            {
                                var details = await _fFsFabricClearanceDetails.FindByIdAsync(item.CL_D_ID);
                                item.CREATED_AT = details.CREATED_AT;
                                item.CREATED_BY = details.CREATED_BY;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fFsFabricClearanceDetails.Update(item);
                            }
                            TempData["message"] = "Successfully Updated Clearance Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFabricClearanceList", $"FFsFabricClearance");
                        }
                        TempData["message"] = "Failed to Update Clearance Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetFabricClearanceList", $"FFsFabricClearance");
                    }
                    TempData["message"] = "Invalid Clearance Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetFabricClearanceList", $"FFsFabricClearance");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                fFsFabricClearanceViewModel = await _fFsFabricClearanceMaster.FindAllByIdAsync(
                    int.Parse(_protector.Unprotect(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.EncryptedId)));
                fFsFabricClearanceViewModel = await GetInfo(fFsFabricClearanceViewModel);
                return View(fFsFabricClearanceViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Clearance Details.";
                TempData["type"] = "error";
                fFsFabricClearanceViewModel = await _fFsFabricClearanceMaster.FindAllByIdAsync(
                    int.Parse(_protector.Unprotect(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.EncryptedId)));
                fFsFabricClearanceViewModel = await GetInfo(fFsFabricClearanceViewModel);
                return View(fFsFabricClearanceViewModel);
            }
        }

    }
}