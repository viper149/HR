using System;
using System.Collections.Generic;
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
    public class FprWeavingWorkLoadEfficiencyLossController : Controller
    {
        private readonly IF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS _fPrWeavingWorkloadEfficienceloss;
        private readonly IF_HR_SHIFT_INFO _fHrShiftInfo;
        private readonly IF_HRD_EMPLOYEE _fHrEmployee;
        private readonly ILOOM_TYPE _loomType;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Work order & Load Efficiency Loss Information";
        

        public FprWeavingWorkLoadEfficiencyLossController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS fPrWeavingWorkloadEfficienceloss,
            IF_HR_SHIFT_INFO fHrShiftInfo,
            IF_HRD_EMPLOYEE fHrEmployee,
            ILOOM_TYPE loomType,
            UserManager<ApplicationUser> userManager
        )
        {
            _fPrWeavingWorkloadEfficienceloss = fPrWeavingWorkloadEfficienceloss;
            _fHrShiftInfo = fHrShiftInfo;
            _fHrEmployee = fHrEmployee;
            _loomType = loomType;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        //[Route("GetAll")]
        public IActionResult GetFPrWeavingWorkLoadEfficiencyLoss()
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

        [HttpPost]
        //[Route("GetTableData")]
        public async Task<JsonResult> GetTableData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;

            var data = (List<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>)await _fPrWeavingWorkloadEfficienceloss.GetAllFPrWeavingWorkLoadEfficiencyLossAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.KNOTTING != null && m.KNOTTING.ToString().ToUpper().Contains(searchValue)
                                       || m.NEWRUN != null && m.NEWRUN.ToString().ToUpper().Contains(searchValue)
                                       || m.PENDING != null && m.PENDING.ToString().ToUpper().Contains(searchValue)
                                       || m.CAM_CHANGE != null && m.CAM_CHANGE.ToString().ToUpper().Contains(searchValue)
                                       || m.BEAM_SHORT != null && m.BEAM_SHORT.ToString().ToUpper().Contains(searchValue)
                                       || m.YARN_SHORT != null && m.YARN_SHORT.ToString().ToUpper().Contains(searchValue)
                                       || m.MECHANICAL_WORK != null && m.MECHANICAL_WORK.ToString().ToUpper().Contains(searchValue)
                                       || m.ELECTRICAL_WORK != null && m.ELECTRICAL_WORK.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            foreach (var item in finalData)
            {
                item.EncryptedId = _protector.Protect(item.WWEID.ToString());
            }

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        //[Route("Create")]
        public async Task<IActionResult> CreateFPrWeavingWorkLoadEfficiencyLoss()
        {
            try
            {
                return View(await _fPrWeavingWorkloadEfficienceloss.GetInitObjByAsync(new FPrWeavingWorkLoadEfficiencyLossViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFPrWeavingWorkLoadEfficiencyLoss(FPrWeavingWorkLoadEfficiencyLossViewModel fPrWeavingWorkLoadEfficiencyLossViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fPrWeavingWorkloadEfficienceloss.InsertByAsync(fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction("GetFPrWeavingWorkLoadEfficiencyLoss", $"FprWeavingWorkLoadEfficiencyLoss");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fPrWeavingWorkLoadEfficiencyLossViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fPrWeavingWorkLoadEfficiencyLossViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fPrWeavingWorkLoadEfficiencyLossViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFPrWeavingWorkLoadEfficiencyLoss(string WId) 
        {
            var fPrWeavingWorkloadEfficienceloss = await _fPrWeavingWorkloadEfficienceloss.FindByIdAsync(int.Parse(_protector.Unprotect(WId)));

            if (fPrWeavingWorkloadEfficienceloss != null)
            {
                var fPrWeavingWorkLoadEfficiencyLossViewModel = await _fPrWeavingWorkloadEfficienceloss.GetInitObjByAsync(new FPrWeavingWorkLoadEfficiencyLossViewModel());
                fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss = fPrWeavingWorkloadEfficienceloss;

                fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.EncryptedId = _protector.Protect(fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.WWEID.ToString());
                return View(fPrWeavingWorkLoadEfficiencyLossViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFPrWeavingWorkLoadEfficiencyLoss), $"FprWeavingWorkLoadEfficiencyLoss");
        }

        [HttpPost]
        public async Task<IActionResult> EditFPrWeavingWorkLoadEfficiencyLoss(
            FPrWeavingWorkLoadEfficiencyLossViewModel fPrWeavingWorkLoadEfficiencyLossViewModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.WWEID = int.Parse(
                        _protector.Unprotect(fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.EncryptedId));
                    var fPrWeavingProduction =
                        await _fPrWeavingWorkloadEfficienceloss.FindByIdAsync(fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss
                            .WWEID);
                    if (fPrWeavingProduction != null)
                    {
                        fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.UPDATED_BY =
                            (await _userManager.GetUserAsync(User)).Id;
                        fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.UPDATED_AT = DateTime.Now;
                        fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.CREATED_AT = fPrWeavingProduction.CREATED_AT;
                        fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss.CREATED_BY = fPrWeavingProduction.CREATED_BY;

                        if (await _fPrWeavingWorkloadEfficienceloss.Update(fPrWeavingWorkLoadEfficiencyLossViewModel.FPrWeavingWorkloadEfficienceloss))
                        {
                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFPrWeavingWorkLoadEfficiencyLoss), $"FprWeavingWorkLoadEfficiencyLoss");
                        }

                        TempData["message"] = $"Failed to Update {title}.";
                        TempData["type"] = "error";
                        return RedirectToAction(nameof(GetFPrWeavingWorkLoadEfficiencyLoss), $"FprWeavingWorkLoadEfficiencyLoss");
                    }

                    TempData["message"] = $"{title} Not Found";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFPrWeavingWorkLoadEfficiencyLoss), $"FprWeavingWorkLoadEfficiencyLoss");
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                return View(await _fPrWeavingWorkloadEfficienceloss.GetInitObjByAsync(new FPrWeavingWorkLoadEfficiencyLossViewModel()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
