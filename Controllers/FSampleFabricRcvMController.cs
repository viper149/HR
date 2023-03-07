using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels.SampleGarments.Fabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Route("SampleFabricReceive")]
    public class FSampleFabricRcvMController : Controller
    {
        private readonly IF_SAMPLE_FABRIC_RCV_M _fSampleFabricRcvM;
        private readonly IF_SAMPLE_FABRIC_RCV_D _fSampleFabricRcvD;
        private readonly IDataProtector _protector;

        public FSampleFabricRcvMController(IF_SAMPLE_FABRIC_RCV_M fSampleFabricRcvM,
            IF_SAMPLE_FABRIC_RCV_D fSampleFabricRcvD,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _fSampleFabricRcvM = fSampleFabricRcvM;
            _fSampleFabricRcvD = fSampleFabricRcvD;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [AcceptVerbs("Get", "Post")]
        [Route("GetPrograms/{search?}/{page?}")]
        public async Task<IActionResult> GetPrograms(string search, int page)
        {
            return Ok(await _fSampleFabricRcvM.GetProgramsByAsync(search, page));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFSampleGarmentRcvM(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMView)
        {
            switch (ModelState.IsValid)
            {
                case true:
                {
                    var fSampleFabricRcvM = await _fSampleFabricRcvM.FindByIdAsync(int.Parse(_protector.Unprotect(createFSampleFabricRcvMView.FSampleFabricRcvM.EncryptedId)));

                    switch (fSampleFabricRcvM.SFRID)
                    {
                        case > 0:
                        {
                            if (await _fSampleFabricRcvM.Update(createFSampleFabricRcvMView.FSampleFabricRcvM))
                            {
                                var fSampleFabricRcvDs = createFSampleFabricRcvMView.FSampleFabricRcvDs.Where(e => e.TRNSID <= 0).ToList();
                                var sampleFabricRcvDs = createFSampleFabricRcvMView.FSampleFabricRcvDs.Where(e => e.TRNSID > 0).ToList();

                                foreach (var item in createFSampleFabricRcvMView.FSampleFabricRcvDs)
                                {
                                    item.SFRID = fSampleFabricRcvM.SFRID;
                                }

                                await _fSampleFabricRcvD.InsertRangeByAsync(fSampleFabricRcvDs);
                                await _fSampleFabricRcvD.UpdateRangeByAsync(sampleFabricRcvDs);
                            }

                            break;
                        }
                    }

                    break;
                }
            }

            return RedirectToAction(nameof(GetFSampleFabricRcvM), $"FSampleFabricRcvM");
        }

        [HttpGet]
        [Route("Edit/{sfrId?}")]
        public async Task<IActionResult> EditFSampleGarmentRcvM(string sfrId)
        {
            var createFSampleFabricRcvMViewModel = await _fSampleFabricRcvM.FindBySfrIdIncludeAllAsync(int.Parse(_protector.Unprotect(sfrId)));
            return View(await _fSampleFabricRcvM.GetInitObjsByAsync(createFSampleFabricRcvMViewModel));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("Delete/{sfrId}")]
        public async Task<IActionResult> DeleteFSampleGarmentRcvM(string sfrId)
        {
            var createFSampleFabricRcvMViewModel = await _fSampleFabricRcvM.FindBySfrIdForDeleteAsync(int.Parse(_protector.Unprotect(sfrId)));

            await _fSampleFabricRcvD.DeleteRange(createFSampleFabricRcvMViewModel.FSampleFabricRcvDs);
            await _fSampleFabricRcvM.Delete(createFSampleFabricRcvMViewModel.FSampleFabricRcvM);

            return RedirectToAction(nameof(GetFSampleFabricRcvM), $"FSampleFabricRcvM");
        }

        [HttpGet]
        [Route("Details/{sfrId}")]
        public async Task<IActionResult> DetailsFSampleGarmentRcvM(string sfrId)
        {
            return View(await _fSampleFabricRcvM.FindBySfrIdIncludeAllAsync(int.Parse(_protector.Unprotect(sfrId))));
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

                var forDataTableByAsync = await _fSampleFabricRcvM.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        public async Task<IActionResult> GetFSampleFabricRcvM()
        {
            return View();
        }

        [AcceptVerbs("Post")]
        [Route("GetDetailsFormInspection")]
        public async Task<IActionResult> GetDetailsFormInspection(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            ModelState.Clear();
            return PartialView($"CreateFSampleFabricRcvMTable", await _fSampleFabricRcvM.GetInitObjectsByAsync(await _fSampleFabricRcvM.GetDetailsFormInspectionByAsync(createFSampleFabricRcvMViewModel)));
        }

        [AcceptVerbs("Post")]
        [Route("GetDetailsFromClearance")]
        public async Task<IActionResult> GetDetailsFromClearance(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            ModelState.Clear();
            return PartialView($"CreateFSampleFabricRcvMTable", await _fSampleFabricRcvM.GetInitObjectsByAsync(await _fSampleFabricRcvM.GetDetailsFormClearanceByAsync(createFSampleFabricRcvMViewModel)));
        }

        [HttpPost]
        [Route("AddOrRemoveDetailsList")]
        public async Task<IActionResult> AddOrDeleteFSampleGarmentRcvMDetailsTable(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            try
            {
                ModelState.Clear();
                if (createFSampleFabricRcvMViewModel.IsDeletable)
                {
                    var fSampleFabricRcvD = createFSampleFabricRcvMViewModel.FSampleFabricRcvDs[createFSampleFabricRcvMViewModel.RemoveIndex];

                    if (fSampleFabricRcvD.TRNSID > 0)
                    {
                        await _fSampleFabricRcvD.Delete(fSampleFabricRcvD);
                    }

                    createFSampleFabricRcvMViewModel.FSampleFabricRcvDs.RemoveAt(createFSampleFabricRcvMViewModel.RemoveIndex);
                }
                else
                {
                    if (!createFSampleFabricRcvMViewModel.FSampleFabricRcvDs.Any(e =>
                        e.SITEMID.Equals(createFSampleFabricRcvMViewModel.FSampleFabricRcvD.SITEMID) &&
                        e.FABCODE.Equals(createFSampleFabricRcvMViewModel.FSampleFabricRcvD.FABCODE)))
                    {
                        createFSampleFabricRcvMViewModel.FSampleFabricRcvDs.Add(createFSampleFabricRcvMViewModel.FSampleFabricRcvD);
                    }
                }

                return PartialView($"CreateFSampleFabricRcvMTable", await _fSampleFabricRcvM.GetInitObjectsByAsync(createFSampleFabricRcvMViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFSampleFabricRcvM()
        {
            var createFSampleFabricRcvMViewModel = new CreateFSampleFabricRcvMViewModel
            {
                FSampleFabricRcvM = new F_SAMPLE_FABRIC_RCV_M { SFRDATE = DateTime.Now }
            };

            return View(await _fSampleFabricRcvM.GetInitObjsByAsync(createFSampleFabricRcvMViewModel));
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFSampleFabricRcvM(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            if (ModelState.IsValid)
            {
                var fSampleFabricRcvM = await _fSampleFabricRcvM.GetInsertedObjByAsync(createFSampleFabricRcvMViewModel.FSampleFabricRcvM);

                if (fSampleFabricRcvM.SFRID > 0)
                {
                    foreach (var item in createFSampleFabricRcvMViewModel.FSampleFabricRcvDs)
                    {
                        item.SFRID = fSampleFabricRcvM.SFRID;
                    }

                    await _fSampleFabricRcvD.InsertRangeByAsync(createFSampleFabricRcvMViewModel.FSampleFabricRcvDs);
                }

                return RedirectToAction(nameof(GetFSampleFabricRcvM), $"FSampleFabricRcvM");
            }
            else
            {
                return View($"CreateFSampleFabricRcvM", createFSampleFabricRcvMViewModel);
            }
        }

    }
}
