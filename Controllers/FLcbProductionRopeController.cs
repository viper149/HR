using System;
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
    public class FLcbProductionRopeController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_LCB_PRODUCTION_ROPE_MASTER _fLcbProductionRopeMaster;
        private readonly IF_LCB_PRODUCTION_ROPE_DETAILS _fLcbProductionRopeDetails;
        private readonly IF_LCB_PRODUCTION_ROPE_PROCESS_INFO _fLcbProductionRopeProcessInfo;

        public FLcbProductionRopeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster,
            IF_LCB_PRODUCTION_ROPE_DETAILS fLcbProductionRopeDetails,
            IF_LCB_PRODUCTION_ROPE_PROCESS_INFO fLcbProductionRopeProcessInfo

        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fLcbProductionRopeMaster = fLcbProductionRopeMaster;
            _fLcbProductionRopeDetails = fLcbProductionRopeDetails;
            _fLcbProductionRopeProcessInfo = fLcbProductionRopeProcessInfo;
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

                var data = await _fLcbProductionRopeMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault()?.PROG_.PROG_NO != null && m.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.PROG_NO.ToUpper().Contains(searchValue)
                                           || (m.TRANSDATE != null && m.TRANSDATE.ToString().Contains(searchValue))
                                           || (m.PER_SET_LENGTH.ToString() != null && m.PER_SET_LENGTH.ToString().Contains(searchValue))
                                           || (m.LCB_LENGTH.ToString() != null && m.LCB_LENGTH.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.LCBPROID.ToString());
                }

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
        public IActionResult GetLcbProductionRope()
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
        public async Task<IActionResult> CreateLcbProductionRope()
        {
            var fLcbProductionRopeViewModel = await GetInfo(new FLcbProductionRopeViewModel());
            fLcbProductionRopeViewModel.FLcbProductionRopeMaster = new F_LCB_PRODUCTION_ROPE_MASTER
            {
                TRANSDATE = DateTime.Now
            };
            fLcbProductionRopeViewModel.FLcbProductionRopeDetails = new F_LCB_PRODUCTION_ROPE_DETAILS()
            {
                TRANSDATE = DateTime.Now
            };
            return View(fLcbProductionRopeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLcbProductionRope(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fLcbProductionRopeViewModel.FLcbProductionRopeMaster.CREATED_BY = user.Id;
                    fLcbProductionRopeViewModel.FLcbProductionRopeMaster.UPDATED_BY = user.Id;
                    fLcbProductionRopeViewModel.FLcbProductionRopeMaster.CREATED_AT = DateTime.Now;
                    fLcbProductionRopeViewModel.FLcbProductionRopeMaster.UPDATED_AT = DateTime.Now;
                    var lcbProId = await _fLcbProductionRopeMaster.InsertAndGetIdAsync(fLcbProductionRopeViewModel.FLcbProductionRopeMaster);

                    if (lcbProId != 0)
                    {
                        foreach (var item in fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList)
                        {
                            item.LCBPROID = lcbProId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var lcbDId = await _fLcbProductionRopeDetails.InsertAndGetIdAsync(item);
                            foreach (var i in item.FLcbProductionRopeProcessInfoList)
                            {
                                i.LCB_D_ID = lcbDId;
                                await _fLcbProductionRopeProcessInfo.InsertByAsync(i);
                            }
                        }
                        TempData["message"] = "Successfully LCB Process Rope Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetLcbProductionRope", $"FLcbProductionRope");
                    }

                    TempData["message"] = "Failed to Create LCB Process Rope.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fLcbProductionRopeViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fLcbProductionRopeViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create LCB Process Rope.";
                TempData["type"] = "error";
                return View(await GetInfo(fLcbProductionRopeViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditLcbProductionRope(string id)
        {
            try
            {
                var lcbId = int.Parse(_protector.Unprotect(id));
                var fLcbProductionRopeViewModel = await _fLcbProductionRopeMaster.FindAllByIdAsync(lcbId);

                if (fLcbProductionRopeViewModel.FLcbProductionRopeMaster != null)
                {
                    fLcbProductionRopeViewModel = await GetInfo(fLcbProductionRopeViewModel);
                    fLcbProductionRopeViewModel.FLcbProductionRopeMaster.EncryptedId = _protector.Protect(fLcbProductionRopeViewModel.FLcbProductionRopeMaster.LCBPROID.ToString());

                    return View(fLcbProductionRopeViewModel);
                }

                TempData["message"] = "LCB Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetLcbProductionRope", $"FLcbProductionRope");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetLcbProductionRope", $"FLcbProductionRope");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditLcbProductionRope(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lcbId = int.Parse(_protector.Unprotect(fLcbProductionRopeViewModel.FLcbProductionRopeMaster.EncryptedId));
                    if (fLcbProductionRopeViewModel.FLcbProductionRopeMaster.LCBPROID == lcbId)
                    {
                        var lcbDetails = await _fLcbProductionRopeMaster.FindByIdAsync(lcbId);

                        var user = await _userManager.GetUserAsync(User);
                        fLcbProductionRopeViewModel.FLcbProductionRopeMaster.UPDATED_BY = user.Id;
                        fLcbProductionRopeViewModel.FLcbProductionRopeMaster.UPDATED_AT = DateTime.Now;
                        fLcbProductionRopeViewModel.FLcbProductionRopeMaster.CREATED_AT = lcbDetails.CREATED_AT;
                        fLcbProductionRopeViewModel.FLcbProductionRopeMaster.CREATED_BY = lcbDetails.CREATED_BY;

                        var isUpdated = await _fLcbProductionRopeMaster.Update(fLcbProductionRopeViewModel.FLcbProductionRopeMaster);
                        if (isUpdated)
                        {
                            foreach (var item in fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList.Where(c => c.LCB_D_ID.Equals(0)))
                            {
                                item.LCBPROID = lcbDetails.LCBPROID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                var lcbDId = await _fLcbProductionRopeDetails.InsertAndGetIdAsync(item);
                                foreach (var i in item.FLcbProductionRopeProcessInfoList)
                                {
                                    i.LCB_D_ID = lcbDId;
                                    await _fLcbProductionRopeProcessInfo.InsertByAsync(i);
                                }
                            }
                            TempData["message"] = "Successfully Updated LCB Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetLcbProductionRope", $"FLcbProductionRope");
                        }
                        TempData["message"] = "Failed to Update LCB Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetLcbProductionRope", $"FLcbProductionRope");
                    }
                    TempData["message"] = "Invalid LCB Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetLcbProductionRope", $"FLcbProductionRope");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                fLcbProductionRopeViewModel = await GetInfo(fLcbProductionRopeViewModel);
                return View(fLcbProductionRopeViewModel);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                fLcbProductionRopeViewModel = await GetInfo(fLcbProductionRopeViewModel);
                return View(fLcbProductionRopeViewModel);
            }
        }

        public async Task<FLcbProductionRopeViewModel> GetInfo(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            fLcbProductionRopeViewModel = await _fLcbProductionRopeMaster.GetInitObjects(fLcbProductionRopeViewModel);
            return fLcbProductionRopeViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCanToList(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            try
            {
                var result = fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList.Any(c =>
                    c.CANID.Equals(fLcbProductionRopeViewModel.FLcbProductionRopeDetails.CANID));
                if (result)
                {
                    var item = fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList.FirstOrDefault(c => c.CANID.Equals(fLcbProductionRopeViewModel.FLcbProductionRopeDetails.CANID));

                    var flag = item.FLcbProductionRopeProcessInfoList.Any(c => c.BEAMID.Equals(fLcbProductionRopeViewModel.FLcbProductionRopeProcessInfo.BEAMID));

                    if (!flag)
                    {
                        item.FLcbProductionRopeProcessInfoList.Add(fLcbProductionRopeViewModel.FLcbProductionRopeProcessInfo);
                    }
                    fLcbProductionRopeViewModel = await GetNamesAsync(fLcbProductionRopeViewModel);
                    return PartialView($"AddCanToList", fLcbProductionRopeViewModel);
                }
                fLcbProductionRopeViewModel.FLcbProductionRopeDetails.FLcbProductionRopeProcessInfoList.Add(fLcbProductionRopeViewModel.FLcbProductionRopeProcessInfo);
                fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList.Add(fLcbProductionRopeViewModel.FLcbProductionRopeDetails);
                fLcbProductionRopeViewModel = await GetNamesAsync(fLcbProductionRopeViewModel);

                return PartialView($"AddCanToList", fLcbProductionRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fLcbProductionRopeViewModel = await GetNamesAsync(fLcbProductionRopeViewModel);
                return PartialView($"AddCanToList", fLcbProductionRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCanFromList(FLcbProductionRopeViewModel fLcbProductionRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList.RemoveAt(int.Parse(removeIndexValue));
            fLcbProductionRopeViewModel = await GetNamesAsync(fLcbProductionRopeViewModel);
            return PartialView($"AddCanToList", fLcbProductionRopeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProcessFromCanList(FLcbProductionRopeViewModel fLcbProductionRopeViewModel, string removeIndexValue, string setRemoveIndex)
        {
            ModelState.Clear();
            fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList[int.Parse(removeIndexValue)]
                .FLcbProductionRopeProcessInfoList.RemoveAt(int.Parse(setRemoveIndex));
            fLcbProductionRopeViewModel = await GetNamesAsync(fLcbProductionRopeViewModel);
            return PartialView($"AddCanToList", fLcbProductionRopeViewModel);
        }

        public async Task<FLcbProductionRopeViewModel> GetNamesAsync(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            fLcbProductionRopeViewModel = await _fLcbProductionRopeDetails.GetInitData(fLcbProductionRopeViewModel);
            return fLcbProductionRopeViewModel;
        }

        [HttpGet]
        public async Task<RndProductionOrderDetailViewModel> GetSetDetails(string setId)
        {
            var result = await _fLcbProductionRopeMaster.GetSetDetails(int.Parse(setId));
            return result;
        }

        [AcceptVerbs("Get", "Post")]
        [Route("ProductionRope/LCB/GetSubGroupDetails/{subGroupId?}")]
        public async Task<IActionResult> GetSubGroupDetails(int subGroupId)
        {
            try
            {
                return Ok(await _fLcbProductionRopeMaster.GetSubGroupDetails(subGroupId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<F_DYEING_PROCESS_ROPE_DETAILS> GetCanDetails(int canId)
        {
            var result = await _fLcbProductionRopeDetails.GetCanDetails(canId);
            return result;
        }
        
        [HttpGet]
        public IActionResult RLCBDeliveryReport(string subGroupNo)
        {
            return View(model: subGroupNo);
        }
    }
}