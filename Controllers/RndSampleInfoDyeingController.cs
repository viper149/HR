using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Dyeing;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndSampleInfoDyeingController : Controller
    {
        private readonly IRND_SAMPLE_INFO_DYEING _rndSampleInfoDyeing;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRND_SAMPLE_INFO_DETAILS _rndSampleInfoDetails;
        private readonly IPL_SAMPLE_PROG_SETUP _plSampleProgSetup;
        private readonly IPL_DYEING_MACHINE_TYPE _plDyeingMachineType;
        private readonly IDataProtector _protector;

        public RndSampleInfoDyeingController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_SAMPLE_INFO_DYEING rndSampleInfoDyeing,
            UserManager<ApplicationUser> userManager,
            IRND_SAMPLE_INFO_DETAILS rndSampleInfoDetails,
            IPL_SAMPLE_PROG_SETUP plSampleProgSetup,
            IPL_DYEING_MACHINE_TYPE plDyeingMachineType
            )
        {
            _rndSampleInfoDyeing = rndSampleInfoDyeing;
            _userManager = userManager;
            _rndSampleInfoDetails = rndSampleInfoDetails;
            _plSampleProgSetup = plSampleProgSetup;
            _plDyeingMachineType = plDyeingMachineType;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamInfo(string sdrfId)
        {
            try
            {
                return Json(await _rndSampleInfoDyeing.GetTeamInfo(int.Parse(sdrfId)));
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRndSampleInfoDyeing(string id)
        {
            var rndSampleInfoDyeing = await _rndSampleInfoDyeing.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (rndSampleInfoDyeing != null)
            {
                var rndSampleInfoDetailses = (List<RND_SAMPLE_INFO_DETAILS>)await _rndSampleInfoDetails.FindBySdIdAsync(rndSampleInfoDyeing.SDID);

                if (rndSampleInfoDetailses.Any())
                {
                    await _rndSampleInfoDetails.DeleteRange(rndSampleInfoDetailses);
                }

                var isDeleted = await _rndSampleInfoDyeing.Delete(rndSampleInfoDyeing);

                if (isDeleted)
                {
                    TempData["message"] = "Successfully Deleted RnD Sample Info Dyeing";
                    TempData["type"] = "success";
                }
            }
            else
            {
                TempData["message"] = "Failed To Delete RnD Sample Info Dyeing";
                TempData["type"] = "warning";
            }

            return RedirectToAction("GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsRndSampleInfoDyeing(string id)
        {
            return View(await _rndSampleInfoDyeing.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id))));
        }

        private static CreateRndSampleInfoDyeingAndDetailsViewModel GetCalculatedAmount(CreateRndSampleInfoDyeingAndDetailsViewModel modifyRndFabricInfoViewModel)
        {
            try
            {
                switch (modifyRndFabricInfoViewModel.RndSampleInfoDetails.YARNID)
                {
                    case 1:
                        var sumOfRatio = modifyRndFabricInfoViewModel.RndSampleInfoDetailses.Where(e => e.YARN.YARNNAME.ToLower() == "warp").Sum(e => e.RATIO);
                        foreach (var item in modifyRndFabricInfoViewModel.RndSampleInfoDetailses)
                        {
                            if (item.YARN.YARNNAME.ToLower().Equals("warp") && modifyRndFabricInfoViewModel.RndSampleInfoDyeing.TOTAL_ENDS != null && sumOfRatio != null && item.RATIO != null && item.NE != null)
                            {
                                item.BGT = (double?)Math.Round((decimal)((modifyRndFabricInfoViewModel.RndSampleInfoDyeing.TOTAL_ENDS * (item.RATIO / sumOfRatio) * 1.4) / (840 * 2.2046 * item.NE)), 4);
                            }
                        }
                        break;
                }
                return modifyRndFabricInfoViewModel;
            }
            catch (Exception)
            {
                return modifyRndFabricInfoViewModel;
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _rndSampleInfoDyeing.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                return Json(new
                {
                    draw = forDataTableByAsync.Draw,
                    recordsFiltered = forDataTableByAsync.RecordsTotal,
                    recordsTotal = forDataTableByAsync.RecordsTotal,
                    data = forDataTableByAsync.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetRndSampleInfoDyeing()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateRndSampleInfoDyeing()
        {
            return View(await _rndSampleInfoDyeing.GetInitObjects(new CreateRndSampleInfoDyeingAndDetailsViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndSampleInfoDyeing(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User);
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.USERID =
                    createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.UPDATED_BY = user.Result.Id;
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.CREATED_AT = DateTime.Now;
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.CREATED_BY = user.Result.Id;
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.UPDATED_AT = DateTime.Now;
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.RSOrder = _rndSampleInfoDyeing.GetLastRSNo(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.DYEINGCODE);
                var isInserted = await _rndSampleInfoDyeing.InsertByAsync(createRndSampleInfoDyeingAndDetailsViewModel
                    .RndSampleInfoDyeing);

                if (createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses.Any() && isInserted)
                {
                    await _rndSampleInfoDetails.InsertRangeByAsync(createRndSampleInfoDyeingAndDetailsViewModel
                        .RndSampleInfoDetailses.Select(e =>
                        {
                            e.SDID = createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.SDID;
                            return e;
                        }).ToList());
                }
                return RedirectToAction("GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
            }
            else
            {
                return View(await _rndSampleInfoDyeing.GetInitObjects(createRndSampleInfoDyeingAndDetailsViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRndSampleInfoDyeing(string id)
        {
            try
            {
                var rndSampleInfoDyeing = await _rndSampleInfoDyeing.FindByIdIncludeAssociatesAsync(int.Parse(_protector.Unprotect(id)));

                if (rndSampleInfoDyeing != null)
                {


                    var createRndSampleInfoDyeingAndDetailsViewModel = new CreateRndSampleInfoDyeingAndDetailsViewModel
                    {
                        RndSampleInfoDyeing = rndSampleInfoDyeing
                    };

                    createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.EncryptedId = _protector.Protect(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.SDID.ToString());
                    return View(await _rndSampleInfoDyeing.GetInitObjects(createRndSampleInfoDyeingAndDetailsViewModel));
                }
                TempData["message"] = "Failed to retrieve Data.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return RedirectToAction("GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRndSampleInfoDyeing(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sdId = Int32.Parse(_protector.Unprotect(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.EncryptedId));
                    if (createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.SDID == sdId)
                    {
                        var dyeingDetails = await _rndSampleInfoDyeing.FindByIdAsync(sdId);

                        if (dyeingDetails != null && User.IsInRole("Planning"))
                        {
                            foreach (var item in createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList.Where(e => e.TRNSID == 0))
                            {
                                item.SDID = sdId;
                                await _plSampleProgSetup.InsertByAsync(item);
                            }
                            TempData["message"] = "Successfully Set Program/Set No.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
                        }

                        var user = await _userManager.GetUserAsync(User);
                        createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.UPDATED_BY = user.Id;
                        createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.UPDATED_AT = DateTime.Now;
                        createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.CREATED_AT = dyeingDetails.CREATED_AT;
                        createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.CREATED_BY = dyeingDetails.CREATED_BY;

                        var isDyeingInfoUpdated = await _rndSampleInfoDyeing.Update(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing);
                        if (isDyeingInfoUpdated)
                        {
                            foreach (var item in createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses.Where(e => e.TRNSID == 0))
                            {
                                item.SDID = sdId;
                                await _rndSampleInfoDetails.InsertByAsync(item);
                            }

                            foreach (var item in createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList.Where(e => e.TRNSID == 0))
                            {
                                item.SDID = sdId;
                                await _plSampleProgSetup.InsertByAsync(item);
                            }
                            TempData["message"] = "Successfully Updated Warping & Dyeing";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
                        }
                        TempData["message"] = "Failed to Update Warping & Dyeing";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
                    }
                    TempData["message"] = "Invalid Warping & Dyeing";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetRndSampleInfoDyeing", $"RndSampleInfoDyeing");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                var requiredInfo = await _rndSampleInfoDyeing.GetInitObjects(createRndSampleInfoDyeingAndDetailsViewModel);
                return View(requiredInfo);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Analysis Sheet.";
                TempData["type"] = "error";
                var requiredInfo = await _rndSampleInfoDyeing.GetInitObjects(createRndSampleInfoDyeingAndDetailsViewModel);
                return View(requiredInfo);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveRndSampleInfoDetailsTable(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            ModelState.Clear();
            if (createRndSampleInfoDyeingAndDetailsViewModel.IsDelete)
            {
                var removeIndexValue = createRndSampleInfoDyeingAndDetailsViewModel.RemoveIndex;
                if (createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses[removeIndexValue]
                    .TRNSID != 0)
                {
                    await _rndSampleInfoDetails.Delete(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses[removeIndexValue]);
                }
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses.RemoveAt(removeIndexValue);
            }
            else
            {
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses.Add(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetails);
            }

            return PartialView($"AddRndSampleInfoDetailsTable", GetCalculatedAmount(await _rndSampleInfoDyeing.GetInitObjectsWithDetails(createRndSampleInfoDyeingAndDetailsViewModel)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProgNoDetails(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            try
            {
                ModelState.Clear();
                var isExist = createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList.Any(c =>
                    c.PROG_NO.Equals(createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetup.PROG_NO));

                if (!isExist)
                {
                    createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList.Add(createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetup);
                }

                return PartialView($"AddProgNoDetailsTable", createRndSampleInfoDyeingAndDetailsViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProgNoDetails(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList[int.Parse(removeIndexValue)]
                    .TRNSID != 0)
                {
                    await _plSampleProgSetup.Delete(createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList[int.Parse(removeIndexValue)]);
                }

                createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList.RemoveAt(int.Parse(removeIndexValue));
                return PartialView($"AddProgNoDetailsTable", createRndSampleInfoDyeingAndDetailsViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetRndSampleInfoDetailsTable(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            try
            {
                ModelState.Clear();
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses = (List<RND_SAMPLE_INFO_DETAILS>)await _rndSampleInfoDetails.GetAllBySdIdAsync(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.SDID);
                return PartialView($"AddRndSampleInfoDetailsTable", await _rndSampleInfoDyeing.GetInitObjectsWithDetails(createRndSampleInfoDyeingAndDetailsViewModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetProgDetailsTable(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            try
            {
                ModelState.Clear();
                createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetupList = (List<PL_SAMPLE_PROG_SETUP>)await _plSampleProgSetup.GetAllBySdIdAsync(createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.SDID);
                return PartialView($"AddProgNoDetailsTable", createRndSampleInfoDyeingAndDetailsViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<ProgramNoDetailsViewModel> GetProgramDetails(string programNo)
        {
            try
            {
                if (programNo.Length < 4)
                {
                    return null;
                }
                var result =await GetProgSet(new List<ProgramNoDetailsViewModel>());

                if (programNo.ToUpper() == "LEADER")
                {
                    var program = result.FirstOrDefault(c => c.MACHINE_NO.Contains(programNo.ToUpper()));
                    return program;
                }
                return result.FirstOrDefault(c => c.MACHINE_NO.Contains(programNo.Split("/")[0]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ProgramNoDetailsViewModel>> GetProgSet(List<ProgramNoDetailsViewModel> prog)
        {
            var dyeingMachine = await _plDyeingMachineType.GetAll();

            prog.AddRange(dyeingMachine.Select(item => new ProgramNoDetailsViewModel {MACHINE_NO = item.MACHINE_NO, PROCESS_TYPE = item.PROCESS_TYPE, WARP_TYPE = item.WARP_TYPE, TYPE = item.TYPE}));

            return prog;
        }
    }
}