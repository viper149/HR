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
    [Route("SampleFabric/HO/Receive")]
    public class HSampleFabricReceivingMController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IH_SAMPLE_FABRIC_RECEIVING_M _hSampleFabricReceivingM;
        private readonly IH_SAMPLE_FABRIC_RECEIVING_D _hSampleFabricReceivingD;
        private const string Title = "HO Sample Fabric Receive";

        public HSampleFabricReceivingMController(IH_SAMPLE_FABRIC_RECEIVING_M hSampleFabricReceivingM,
            IH_SAMPLE_FABRIC_RECEIVING_D hSampleFabricReceivingD,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _hSampleFabricReceivingM = hSampleFabricReceivingM;
            _hSampleFabricReceivingD = hSampleFabricReceivingD;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Post")]
        [Route("AddOrRemoveDispatchDetails")]
        public async Task<IActionResult> AddOrRemoveDispatchDetails(CreateHSampleFabricReceivingM createHSampleFabricReceivingM)
        {
            ModelState.Clear();
            switch (createHSampleFabricReceivingM.IsDelete)
            {
                case true:
                {
                    var hSampleFabricReceivingD = createHSampleFabricReceivingM.HSampleFabricReceivingDs[createHSampleFabricReceivingM.RemoveIndex];

                    if (hSampleFabricReceivingD.RCVDID > 0)
                    {
                        await _hSampleFabricReceivingD.Delete(hSampleFabricReceivingD);
                    }

                    createHSampleFabricReceivingM.HSampleFabricReceivingDs.RemoveAt(createHSampleFabricReceivingM.RemoveIndex);
                    break;
                }
            }

            return PartialView($"CreateHSampleFabricReceivingMTable", await _hSampleFabricReceivingM.GetInitObjsForDetailsTableByAsync(createHSampleFabricReceivingM));
        }

        [AcceptVerbs("Post")]
        [Route("GetHSampleFabricReceiveDetails")]
        public async Task<IActionResult> GetHSampleFabricReceiveDetails(CreateHSampleFabricReceivingM createHSampleFabricReceivingM)
        {
            ModelState.Clear();
            return PartialView($"CreateHSampleFabricReceivingMTable", await _hSampleFabricReceivingM.GetInitObjsForDetailsTableByAsync(await _hSampleFabricReceivingM.GetHSampleFabricReceiveDetailsByAsync(createHSampleFabricReceivingM)));
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

                var forDataTableByAsync = await _hSampleFabricReceivingM.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        public IActionResult GetHSampleFabricReceivingM()
        {
            return View();
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditHSampleFabricReceivingM(CreateHSampleFabricReceivingM createHSampleFabricReceivingM)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    {
                        var hSampleFabricReceivingM = await _hSampleFabricReceivingM.FindByIdAsync(int.Parse(_protector.Unprotect(createHSampleFabricReceivingM.HSampleFabricReceivingM.EncryptedId)));

                        if (hSampleFabricReceivingM is { } && await _hSampleFabricReceivingM.Update(createHSampleFabricReceivingM.HSampleFabricReceivingM))
                        {
                            var hSampleFabricReceivingDs = createHSampleFabricReceivingM.HSampleFabricReceivingDs.Where(e => e.RCVDID <= 0).ToList();
                            var sampleFabricReceivingDs = createHSampleFabricReceivingM.HSampleFabricReceivingDs.Where(e => e.RCVDID > 0).ToList();

                            foreach (var item in createHSampleFabricReceivingM.HSampleFabricReceivingDs)
                            {
                                item.RCVID = hSampleFabricReceivingM.RCVID;
                            }

                            await _hSampleFabricReceivingD.InsertRangeByAsync(hSampleFabricReceivingDs);
                            await _hSampleFabricReceivingD.UpdateRangeByAsync(sampleFabricReceivingDs);

                            TempData["message"] = $"Successfully Updated {Title}";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetHSampleFabricReceivingM), $"HSampleFabricReceivingM");
                        }

                        break;
                    }
            }

            TempData["message"] = $"Failed To Update {Title}";
            TempData["type"] = "error";
            return View(nameof(EditHSampleFabricReceivingM), await _hSampleFabricReceivingM.GetInitObjsByAsync(createHSampleFabricReceivingM));
        }

        [HttpGet]
        [Route("Edit/{rcvId?}")]
        public async Task<IActionResult> EditHSampleFabricReceivingM(string rcvId)
        {
            return View(await _hSampleFabricReceivingM.GetInitObjsByAsync(await _hSampleFabricReceivingM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(rcvId)))));
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateHSampleFabricReceivingM()
        {
            var createHSampleFabricReceivingM = new CreateHSampleFabricReceivingM
            {
                HSampleFabricReceivingM = new H_SAMPLE_FABRIC_RECEIVING_M { RCVDATE = DateTime.Now }
            };

            return View(await _hSampleFabricReceivingM.GetInitObjsByAsync(createHSampleFabricReceivingM));
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateHSampleFabricReceivingM(CreateHSampleFabricReceivingM createHSampleFabricReceiving)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    {
                        var hSampleFabricReceivingM = await _hSampleFabricReceivingM.GetInsertedObjByAsync(createHSampleFabricReceiving
                            .HSampleFabricReceivingM);

                        switch (hSampleFabricReceivingM.RCVID)
                        {
                            case > 0:
                                {
                                    foreach (var item in createHSampleFabricReceiving.HSampleFabricReceivingDs)
                                    {
                                        item.RCVID = hSampleFabricReceivingM.RCVID;
                                    }

                                    await _hSampleFabricReceivingD.InsertRangeByAsync(createHSampleFabricReceiving.HSampleFabricReceivingDs);
                                    return RedirectToAction(nameof(GetHSampleFabricReceivingM), $"HSampleFabricReceivingM");
                                }
                            default:
                                return View(nameof(CreateHSampleFabricReceivingM), await _hSampleFabricReceivingM.GetInitObjsByAsync(createHSampleFabricReceiving));
                        }
                    }
                default:
                    return View(nameof(CreateHSampleFabricReceivingM), await _hSampleFabricReceivingM.GetInitObjsByAsync(createHSampleFabricReceiving));
            }
        }
    }
}
