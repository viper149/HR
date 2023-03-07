using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels.SampleFabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Route("SampleFabric/HO/Dispatch")]
    public class HSampleFabricDispatchMasterController : Controller
    {
        private readonly IH_SAMPLE_FABRIC_DISPATCH_MASTER _hSampleFabricDispatchMaster;
        private readonly IH_SAMPLE_FABRIC_DISPATCH_DETAILS _hSampleFabricDispatchDetails;
        private readonly IDataProtector _protector;
        private const string Title = "Sample Fabric Dispatch";

        public HSampleFabricDispatchMasterController(IH_SAMPLE_FABRIC_DISPATCH_MASTER hSampleFabricDispatchMaster,
            IH_SAMPLE_FABRIC_DISPATCH_DETAILS hSampleFabricDispatchDetails,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _hSampleFabricDispatchMaster = hSampleFabricDispatchMaster;
            _hSampleFabricDispatchDetails = hSampleFabricDispatchDetails;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Post")]
        [Route("AddOrRemoveDetailsList")]
        public async Task<IActionResult> AddOrRemoveDetailsList(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            if (createHSampleFabricDispatchMaster.IsDelete)
            {
                var hSampleFabricDispatchDetails = createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses[createHSampleFabricDispatchMaster.RemoveIndex];

                if (hSampleFabricDispatchDetails.SFDID > 0)
                {
                    await _hSampleFabricDispatchDetails.Delete(hSampleFabricDispatchDetails);
                }

                createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses.RemoveAt(createHSampleFabricDispatchMaster.RemoveIndex);
            }
            else
            {
                createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses.Add(createHSampleFabricDispatchMaster.HSampleFabricDispatchDetails);
            }

            return PartialView("CreateHSampleFabricDispatchMasterTable", await _hSampleFabricDispatchMaster.GetInitObjForDetailsTableByAsync(createHSampleFabricDispatchMaster));
        }

        [HttpPost]
        [Route("GetTableData")]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _hSampleFabricDispatchMaster.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        [Route("GetAll")]
        public IActionResult GetHSampleFabricDispatchMaster()
        {
            return View();
        }

        [AcceptVerbs("Post")]
        [Route("GetOtherInfo")]
        public async Task<IActionResult> GetOtherInfo(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            return Ok(await _hSampleFabricDispatchMaster.GetOtherInfoByAsync(createHSampleFabricDispatchMaster));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetAvailableItems/{search?}/{page?}")]
        public async Task<IActionResult> GetAvailableItems(string search, int page)
        {
            return Ok(await _hSampleFabricDispatchMaster.GetAvailableItemsByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetMerchandisers/{search?}/{page?}")]
        public async Task<IActionResult> GetMerchandisers(string search, int page)
        {
            return Ok(await _hSampleFabricDispatchMaster.GetMerchandisersByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetBuyers/{search?}/{page?}")]
        public async Task<IActionResult> GetBuyers(string search, int page)
        {
            return Ok(await _hSampleFabricDispatchMaster.GetBuyersByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetBrands/{search?}/{page?}")]
        public async Task<IActionResult> GetBrands(string search, int page)
        {
            return Ok(await _hSampleFabricDispatchMaster.GetBrandsByAsync(search, page));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditHSampleFabricDispatchMaster(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    {
                        var hSampleFabricDispatchMaster = await _hSampleFabricDispatchMaster.FindByIdAsync(int.Parse(_protector.Unprotect(createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster.EncryptedId)));

                        if (hSampleFabricDispatchMaster != null)
                        {
                            createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster.SFDID = hSampleFabricDispatchMaster.SFDID;
                            createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster.GPNO = hSampleFabricDispatchMaster.GPNO;

                            if (await _hSampleFabricDispatchMaster.Update(createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster))
                            {
                                var hSampleFabricDispatchDetailses = createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses.Where(e => e.SFDDID <= 0).ToList();
                                var sampleFabricDispatchDetailses = createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses.Where(e => e.SFDDID > 0).ToList();

                                foreach (var item in createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses)
                                {
                                    item.SFDID = hSampleFabricDispatchMaster.SFDID;
                                }

                                await _hSampleFabricDispatchDetails.InsertRangeByAsync(hSampleFabricDispatchDetailses);
                                await _hSampleFabricDispatchDetails.UpdateRangeByAsync(sampleFabricDispatchDetailses);

                                TempData["message"] = $"Successfully Updated {Title}. With {hSampleFabricDispatchDetailses.Count} new rows & {sampleFabricDispatchDetailses.Count} old rows.";
                                TempData["type"] = "success";
                                return RedirectToAction(nameof(GetHSampleFabricDispatchMaster), $"HSampleFabricDispatchMaster");
                            }
                        }

                        break;
                    }
            }

            return View(nameof(EditHSampleFabricDispatchMaster), await _hSampleFabricDispatchMaster.GetInitObjByAsync(createHSampleFabricDispatchMaster));
        }

        [HttpGet]
        [Route("Edit/{sfdId?}")]
        public async Task<IActionResult> EditHSampleFabricDispatchMaster(string sfdId)
        {
            var hSampleFabricDispatchMaster = await _hSampleFabricDispatchMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(sfdId)));

            if (hSampleFabricDispatchMaster.HSampleFabricDispatchMaster != null)
            {
                return View(await _hSampleFabricDispatchMaster.GetInitObjByAsync(hSampleFabricDispatchMaster));
            }

            return RedirectToAction(nameof(GetHSampleFabricDispatchMaster), $"HSampleFabricDispatchMaster");
        }

        [AcceptVerbs("Post")]
        [Route("Delete/{sfdId?}")]
        public async Task<IActionResult> DeleteHSampleFabricDispatchMaster(string sfdId)
        {
            var hSampleFabricDispatchMaster = await _hSampleFabricDispatchMaster.GetForSafeDeleteByAsync(int.Parse(_protector.Unprotect(sfdId)));

            if (hSampleFabricDispatchMaster != null)
            {
                await _hSampleFabricDispatchDetails.DeleteRange(hSampleFabricDispatchMaster.H_SAMPLE_FABRIC_DISPATCH_DETAILS);
                await _hSampleFabricDispatchMaster.Delete(hSampleFabricDispatchMaster);

                TempData["message"] = $"Successfully Deleted {Title}";
                TempData["type"] = "success";
            }
            else
            {
                TempData["message"] = $"{Title} Could Not Be Found";
                TempData["type"] = "success";
            }

            return RedirectToAction(nameof(GetHSampleFabricDispatchMaster), $"HSampleFabricDispatchMaster");
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateHSampleFabricDispatchMaster()
        {
            var createHSampleFabricDispatchMaster = new CreateHSampleFabricDispatchMaster
            {
                HSampleFabricDispatchMaster = new H_SAMPLE_FABRIC_DISPATCH_MASTER
                {
                    GPNO = await _hSampleFabricDispatchMaster.GetGatePassNoByAsync(),
                    ISSUE_DATE = DateTime.Now,
                    PURPOSE = "Sample Fabric"
                }
            };

            return View(createHSampleFabricDispatchMaster);
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateHSampleFabricDispatchMaster(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    {
                        var hSampleFabricDispatchMaster = await _hSampleFabricDispatchMaster.GetInsertedObjByAsync(createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster);

                        switch (hSampleFabricDispatchMaster.SFDID)
                        {
                            case > 0:
                                {
                                    foreach (var item in createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses)
                                    {
                                        item.SFDID = hSampleFabricDispatchMaster.SFDID;
                                    }

                                    await _hSampleFabricDispatchDetails.InsertRangeByAsync(createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses);
                                    TempData["message"] = $"Successfully Created {Title}";
                                    TempData["type"] = "success";
                                    return RedirectToAction(nameof(GetHSampleFabricDispatchMaster), $"HSampleFabricDispatchMaster");
                                }
                        }

                        break;
                    }
            }

            TempData["message"] = $"Failed To Create {Title}";
            TempData["type"] = "success";
            return View(nameof(CreateHSampleFabricDispatchMaster), createHSampleFabricDispatchMaster);
        }
    }
}
