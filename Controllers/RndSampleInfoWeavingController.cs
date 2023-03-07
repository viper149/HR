using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.Rnd.Weaving;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndSampleInfoWeavingController : Controller
    {
        private readonly IRND_SAMPLE_INFO_WEAVING _rndSampleInfoWeaving;
        private readonly IRND_SAMPLE_INFO_WEAVING_DETAILS _rndSampleInfoWeavingDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPL_SAMPLE_PROG_SETUP _plSampleProgSetup;
        private readonly IDataProtector _protector;

        public RndSampleInfoWeavingController(IRND_SAMPLE_INFO_WEAVING rndSampleInfoWeaving,
            IRND_SAMPLE_INFO_WEAVING_DETAILS rndSampleInfoWeavingDetails,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IPL_SAMPLE_PROG_SETUP plSampleProgSetup)
        {
            _rndSampleInfoWeaving = rndSampleInfoWeaving;
            _rndSampleInfoWeavingDetails = rndSampleInfoWeavingDetails;
            _userManager = userManager;
            _plSampleProgSetup = plSampleProgSetup;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRndSampleInfoWeaving(string wvId)
        {
            try
            {
                var rndSampleInfoWeavingWithDetailsViewModel = await _rndSampleInfoWeaving.FindByWvIdAsync(int.Parse(_protector.Unprotect(wvId)));

                if (await _rndSampleInfoWeaving.Delete(rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeaving))
                {
                    if (rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeavingDetailses.Any())
                    {
                        await _rndSampleInfoWeavingDetails.DeleteRange(rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeavingDetailses);
                    }

                    TempData["message"] = "Successfully Deleted RnD Sample Information Weaving.";
                    TempData["type"] = "success";
                }

                return RedirectToAction("GetRndSampleInfoWeaving", $"RndSampleInfoWeaving");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsRndSampleInfoWeaving(string wvId)
        {
            var rndSampleInfoWeavingWithDetailsViewModel = await _rndSampleInfoWeaving.FindByWvIdAsync(int.Parse(_protector.Unprotect(wvId)));
            rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeaving.EncryptedId = _protector.Protect(rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeaving.WVID.ToString());

            return View(rndSampleInfoWeavingWithDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRndSampleInfoWeaving(EditRndSampleInfoWeavingViewModel editRndSampleInfoWeavingViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.GetUserAsync(User).Result;

                    editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving.UPDATED_AT = DateTime.Now;
                    editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving.UPDATED_BY = user.Id;

                    if (await _rndSampleInfoWeaving.Update(editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving))
                    {
                        if (editRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses.Any())
                        {
                            foreach (var item in editRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses)
                            {
                                item.WVID_PARENT = editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving.WVID;
                                item.SDID = editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving.SDID;

                                if (item.WVID > 0)
                                {
                                    await _rndSampleInfoWeavingDetails.Update(item);
                                }
                                else
                                {
                                    await _rndSampleInfoWeavingDetails.InsertByAsync(item);
                                }
                            }
                        }

                        TempData["message"] = "Successfully Updated RnD Weaving Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRndSampleInfoWeaving", $"RndSampleInfoWeaving");
                    }

                    TempData["message"] = "Failed to Update RnD Weaving Information.";
                    TempData["type"] = "error";

                    var createRndSampleInfoWeavingViewModel = await _rndSampleInfoWeaving.GetInitObjects(new RndSampleInfoWeavingViewModel());
                    var rndSampleInfoWeavingWithDetailsViewModel = await _rndSampleInfoWeaving.FindByWvIdAsync(editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving.WVID);

                    editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving = rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeaving;
                    editRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses = rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeavingDetailses;
                    editRndSampleInfoWeavingViewModel.BasYarnCountinfos = createRndSampleInfoWeavingViewModel.BasYarnCountinfos;
                    editRndSampleInfoWeavingViewModel.BasColors = createRndSampleInfoWeavingViewModel.BasColors;
                    editRndSampleInfoWeavingViewModel.BasYarnLotinfos = createRndSampleInfoWeavingViewModel.BasYarnLotinfos;
                    editRndSampleInfoWeavingViewModel.BasSupplierinfos = createRndSampleInfoWeavingViewModel.BasSupplierinfos;
                    editRndSampleInfoWeavingViewModel.RndSampleInfoDyeings = createRndSampleInfoWeavingViewModel.RndSampleInfoDyeings;
                    editRndSampleInfoWeavingViewModel.PlSampleProgSetups = createRndSampleInfoWeavingViewModel.PlSampleProgSetups;
                    editRndSampleInfoWeavingViewModel.RndWeaves = createRndSampleInfoWeavingViewModel.RndWeaves;
                    editRndSampleInfoWeavingViewModel.Yarnfors = createRndSampleInfoWeavingViewModel.Yarnfors;
                    editRndSampleInfoWeavingViewModel.LoomTypes = createRndSampleInfoWeavingViewModel.LoomTypes;

                    return View(editRndSampleInfoWeavingViewModel);
                }
                else
                {
                    TempData["message"] = "Failed to Update RnD Weaving Information.";
                    TempData["type"] = "error";

                    var createRndSampleInfoWeavingViewModel = await _rndSampleInfoWeaving.GetInitObjects(new RndSampleInfoWeavingViewModel());
                    var rndSampleInfoWeavingWithDetailsViewModel = await _rndSampleInfoWeaving.FindByWvIdAsync(editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving.WVID);

                    editRndSampleInfoWeavingViewModel.RndSampleInfoWeaving = rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeaving;
                    editRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses = rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeavingDetailses;
                    editRndSampleInfoWeavingViewModel.BasYarnCountinfos = createRndSampleInfoWeavingViewModel.BasYarnCountinfos;
                    editRndSampleInfoWeavingViewModel.BasColors = createRndSampleInfoWeavingViewModel.BasColors;
                    editRndSampleInfoWeavingViewModel.BasYarnLotinfos = createRndSampleInfoWeavingViewModel.BasYarnLotinfos;
                    editRndSampleInfoWeavingViewModel.BasSupplierinfos = createRndSampleInfoWeavingViewModel.BasSupplierinfos;
                    editRndSampleInfoWeavingViewModel.RndSampleInfoDyeings = createRndSampleInfoWeavingViewModel.RndSampleInfoDyeings;
                    editRndSampleInfoWeavingViewModel.PlSampleProgSetups = createRndSampleInfoWeavingViewModel.PlSampleProgSetups;
                    editRndSampleInfoWeavingViewModel.RndWeaves = createRndSampleInfoWeavingViewModel.RndWeaves;
                    editRndSampleInfoWeavingViewModel.Yarnfors = createRndSampleInfoWeavingViewModel.Yarnfors;
                    editRndSampleInfoWeavingViewModel.LoomTypes = createRndSampleInfoWeavingViewModel.LoomTypes;

                    return View(editRndSampleInfoWeavingViewModel);
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRndSampleInfoWeaving(string wvId)
        {
            var decryptedWvId = int.Parse(_protector.Unprotect(wvId));
            var createRndSampleInfoWeavingViewModel = await _rndSampleInfoWeaving.GetInitObjects(new RndSampleInfoWeavingViewModel());
            var rndSampleInfoWeavingWithDetailsViewModel = await _rndSampleInfoWeaving.FindByWvIdAsync(decryptedWvId);

            var editRndSampleInfoWeavingViewModel = new EditRndSampleInfoWeavingViewModel
            {
                RndSampleInfoWeaving = rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeaving,
                RndSampleInfoWeavingDetailses = rndSampleInfoWeavingWithDetailsViewModel.RndSampleInfoWeavingDetailses,
                BasYarnCountinfos = createRndSampleInfoWeavingViewModel.BasYarnCountinfos,
                BasColors = createRndSampleInfoWeavingViewModel.BasColors,
                BasYarnLotinfos = createRndSampleInfoWeavingViewModel.BasYarnLotinfos,
                BasSupplierinfos = createRndSampleInfoWeavingViewModel.BasSupplierinfos,
                RndSampleInfoDyeings = createRndSampleInfoWeavingViewModel.RndSampleInfoDyeings,
                PlSampleProgSetups = createRndSampleInfoWeavingViewModel.PlSampleProgSetups,
                RndWeaves = createRndSampleInfoWeavingViewModel.RndWeaves,
                Yarnfors = createRndSampleInfoWeavingViewModel.Yarnfors,
                LoomTypes = createRndSampleInfoWeavingViewModel.LoomTypes,
                BuyerInfos = createRndSampleInfoWeavingViewModel.BuyerInfos,
                MktTeams = createRndSampleInfoWeavingViewModel.MktTeams,
                RndConcerns = createRndSampleInfoWeavingViewModel.RndConcerns,
                PlProductionSetDistributions = createRndSampleInfoWeavingViewModel.PlProductionSetDistributions
            };

            return View(editRndSampleInfoWeavingViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadDetails(int sdId)
        {
            try
            {
                return PartialView($"LoadDetailsTable", await _rndSampleInfoWeaving.FindBySdIdWithSetAsync(sdId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet]
        public async Task<PL_PRODUCTION_SETDISTRIBUTION> LoadSetDetails(int setId)
        {
            try
            {
                return await _rndSampleInfoWeaving.FindSetWithSetIdAsync(setId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadDetailsWarping(int sdId)
        {
            try
            {
                return PartialView($"AddRndSampleInfoDetailsTable.cshtml", await _rndSampleInfoWeaving.FindBySdIdWithSetAsync(sdId));
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _rndSampleInfoWeaving.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        public IActionResult GetRndSampleInfoWeaving()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateRndSampleInfoWeaving()
        {
            return View(await _rndSampleInfoWeaving.GetInitObjects(new RndSampleInfoWeavingViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRndSampleInfoWeaving(RndSampleInfoWeavingViewModel rndSampleInfoWeavingViewModel)
        {
            if (ModelState.IsValid)
            {

                var user = _userManager.GetUserAsync(User).Result;

                rndSampleInfoWeavingViewModel.RndSampleInfoWeaving.CREATED_AT =
                    rndSampleInfoWeavingViewModel.RndSampleInfoWeaving.UPDATED_AT = DateTime.Now;

                rndSampleInfoWeavingViewModel.RndSampleInfoWeaving.CREATED_BY =
                    rndSampleInfoWeavingViewModel.RndSampleInfoWeaving.UPDATED_BY = user.Id;

                var plSampleProgSetup = await _plSampleProgSetup.FindByProgIdAsync(rndSampleInfoWeavingViewModel.RndSampleInfoWeaving.PROG_ID ?? 0);
                rndSampleInfoWeavingViewModel.RndSampleInfoWeaving.SDID = plSampleProgSetup.SDID ?? 0;

                var parentWvIdInsertByAsync = await _rndSampleInfoWeaving.GetParentWvIdInsertByAsync(rndSampleInfoWeavingViewModel.RndSampleInfoWeaving);

                if (parentWvIdInsertByAsync > 0 && rndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses.Any())
                {

                    await _rndSampleInfoWeavingDetails.InsertRangeByAsync(rndSampleInfoWeavingViewModel
                        .RndSampleInfoWeavingDetailses.Select(e =>
                        {
                            e.SDID = plSampleProgSetup.SDID ?? 0;
                            e.WVID_PARENT = parentWvIdInsertByAsync;
                            return e;
                        }));
                }

                TempData["message"] = "Successfully Added Weaving Information.";
                TempData["type"] = "success";
                return RedirectToAction("GetRndSampleInfoWeaving", $"RndSampleInfoWeaving");
            }
            else
            {
                TempData["message"] = "Failed To Add Weaving Information.";
                TempData["type"] = "error";
                return View(await _rndSampleInfoWeaving.GetInitObjects(rndSampleInfoWeavingViewModel));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRndSampleInfoWeavingTable(RndSampleInfoWeavingViewModel createRndSampleInfoWeavingViewModel)
        {
            ModelState.Clear();
            if (createRndSampleInfoWeavingViewModel.IsDelete)
            {
                if (createRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses[createRndSampleInfoWeavingViewModel.RemoveIndex].WVID > 0)
                {
                    await _rndSampleInfoWeavingDetails.Delete(createRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses[createRndSampleInfoWeavingViewModel.RemoveIndex]);
                }

                createRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses.RemoveAt(createRndSampleInfoWeavingViewModel.RemoveIndex);
            }
            else
            {
                createRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses.Add(createRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetails);
            }

            return PartialView($"AddRndSampleInfoWeavingTable", GetCalculatedAmount(await _rndSampleInfoWeaving.GetInitObjectsWithDetails(createRndSampleInfoWeavingViewModel)));
        }

        // 2 (WEFT)
        private RndSampleInfoWeavingViewModel GetCalculatedAmount(RndSampleInfoWeavingViewModel modifyRndFabricInfoViewModel)
        {
            try
            {
                // YARN FOR
                switch (modifyRndFabricInfoViewModel.RndSampleInfoWeavingDetails.YARNID)
                {
                    case 2:
                        var sumOfRatio = modifyRndFabricInfoViewModel.RndSampleInfoWeavingDetailses.Where(e => e.YARN.YARNNAME.ToLower() == "weft").Sum(e => e.RATIO);
                        switch (modifyRndFabricInfoViewModel.RndSampleInfoWeaving.LOOMID)
                        {
                            case 1:

                                modifyRndFabricInfoViewModel.RndSampleInfoWeavingDetailses.Where(e => e.YARN.YARNNAME.ToLower() == "weft").Select(e =>
                                {
                                    if (modifyRndFabricInfoViewModel.RndSampleInfoWeaving.REED_SPACE != null && modifyRndFabricInfoViewModel.RndSampleInfoWeaving.GR_PPI != null)
                                        e.BGT = (double?)Math.Round((decimal)((((modifyRndFabricInfoViewModel.RndSampleInfoWeaving.REED_SPACE + 3) * (e.RATIO / sumOfRatio) * modifyRndFabricInfoViewModel.RndSampleInfoWeaving.GR_PPI) * 1.0936) / (840 * 2.2046 * e.NE ?? 0.0)), 4);
                                    return e;
                                }).ToList();
                                break;
                            case 2:
                                modifyRndFabricInfoViewModel.RndSampleInfoWeavingDetailses.Where(e => e.YARN.YARNNAME.ToLower() == "weft").Select(e =>
                                {
                                    if (modifyRndFabricInfoViewModel.RndSampleInfoWeaving.REED_SPACE != null && modifyRndFabricInfoViewModel.RndSampleInfoWeaving.GR_PPI != null)
                                        e.BGT = (double?)Math.Round((decimal)((((modifyRndFabricInfoViewModel.RndSampleInfoWeaving.REED_SPACE + 6) * (e.RATIO / sumOfRatio) * modifyRndFabricInfoViewModel.RndSampleInfoWeaving.GR_PPI) * 1.09) / (840 * 2.2046 * e.NE ?? 0.0)), 4);
                                    return e;
                                }).ToList();
                                break;
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
    }
}