using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using DenimERP.ViewModels.SampleGarments.Receive;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize(Policy = "FSample")]
    public class FSampleGarmentRcvMController : Controller
    {
        private readonly IF_SAMPLE_GARMENT_RCV_M _fSampleGarmentRcvM;
        private readonly IF_SAMPLE_GARMENT_RCV_D _fSampleGarmentRcvD;
        private readonly IDataProtector _protector;

        public FSampleGarmentRcvMController(IF_SAMPLE_GARMENT_RCV_M fSampleGarmentRcvM,
            IF_SAMPLE_GARMENT_RCV_D fSampleGarmentRcvD,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _fSampleGarmentRcvM = fSampleGarmentRcvM;
            _fSampleGarmentRcvD = fSampleGarmentRcvD;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Post")]
        [Route("SampleGarments/GetDetailsFormInspection")]
        public async Task<IActionResult> GetDetailsFormInspection(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            return PartialView($"AddOrDeleteFSampleGarmentRcvMDetailsTable", await _fSampleGarmentRcvM.GetInitObjectsByAsync(await _fSampleGarmentRcvM.GetInitObjectsOfSelectedItems(await _fSampleGarmentRcvM.GetDetailsFormInspectionByAsync(createFSampleGarmentRcvMViewModel))));
        }
        
        [AcceptVerbs("Get", "Post")]
        [Route("SampleGarments/GetRndFabrics/{search?}/{page?}")]
        public async Task<IActionResult> GetRndFabrics(string search, int page)
        {
            return Ok(await _fSampleGarmentRcvM.GetRndFabricsByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("SampleGarments/GetSampleItems/{search?}/{page?}")]
        public async Task<IActionResult> GetSampleItems(string search, int page)
        {
            return Ok(await _fSampleGarmentRcvM.GetSampleItemsByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("SampleGarments/GetEmployees/{search?}/{page?}")]
        public async Task<IActionResult> GetEmployees(string search, int page)
        {
            return Ok(await _fSampleGarmentRcvM.GetEmployeesByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("SampleGarments/GetSections/{search?}/{page?}")]
        public async Task<IActionResult> GetSections(string search, int page)
        {
            return Ok(await _fSampleGarmentRcvM.GetSectionsByAsync(search, page));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"> Belongs to BARCODE. <see cref="F_SAMPLE_GARMENT_RCV_D"/></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RFSampleGarmentRcvMHandTag(string barcode)
        {
            return View(model: barcode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"> Belongs to BARCODE. <see cref="F_SAMPLE_GARMENT_RCV_D"/></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RFSampleGarmentRcvMSticker(string barcode)
        {
            return View(model: barcode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fabCode"> Belongs to FABCODE. Primary key. Must not to null. <see cref="RND_FABRICINFO"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetStyleInfo(int fabCode)
        {
            return PartialView($"GetStyleInfoTable", await _fSampleGarmentRcvM.FindByFabCodeAsync(fabCode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srId"> Belongs to SGRID. Primary key. Must not be null. <see cref="F_SAMPLE_GARMENT_RCV_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteComImpInvoiceInfo(string srId)
        {
            try
            {
                var fSampleGarmentRcvM = await _fSampleGarmentRcvM.FindByIdAsync(int.Parse(_protector.Unprotect(srId)));

                if (fSampleGarmentRcvM != null)
                {
                    await _fSampleGarmentRcvD.DeleteRange(await _fSampleGarmentRcvD.FindBySrIdAsync(fSampleGarmentRcvM.SGRID));
                    await _fSampleGarmentRcvM.Delete(fSampleGarmentRcvM);

                    TempData["message"] = "Successfully Deleted Sample Garments Receive Information.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Not Found, Sample Garments Receive Information.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetFSampleGarmentRcvM", $"FSampleGarmentRcvM");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srId"> Belongs to SGRID. Primary key. Must not be null. <see cref="F_SAMPLE_GARMENT_RCV_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailsFSampleGarmentRcvM(string srId)
        {
            try
            {
                return View(await _fSampleGarmentRcvM.FindBySrIdIncludeAllAsync(int.Parse(_protector.Unprotect(srId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFSampleGarmentRcvM(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return View(await _fSampleGarmentRcvM.GetInitObjectsByAsync(createFSampleGarmentRcvMViewModel));

                var fSampleGarmentRcvM = await _fSampleGarmentRcvM.Update(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM);

                if (fSampleGarmentRcvM)
                {
                    //var count = 10000000;
                    foreach (var item in createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs)
                    {
                        var fSampleGarmentRcvDs = await _fSampleGarmentRcvD.All();
                        item.SGRID = int.Parse(_protector.Unprotect(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM.EncryptedId));

                        if (item.TRNSID > 0)
                        {
                            if (string.IsNullOrEmpty(item.BARCODE))
                            {
                                var itemBarcode = fSampleGarmentRcvDs.Any(e => e.FABCODE.Equals(item.FABCODE) && e.SITEMID.Equals(item.SITEMID))
                                    ? (int.Parse((fSampleGarmentRcvDs.Where(e => e.FABCODE.Equals(item.FABCODE) && e.SITEMID.Equals(item.SITEMID))
                                        .OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE ?? (fSampleGarmentRcvDs.OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE)) ?? "0") + 1)
                                    : (int.Parse(fSampleGarmentRcvDs.OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE ?? "0") + 1);

                                item.BARCODE = $"{itemBarcode}".PadLeft(8, '0');
                            }

                            await _fSampleGarmentRcvD.Update(item);
                        }
                        else
                        {
                            var itemBarcode = fSampleGarmentRcvDs.Any(e => e.FABCODE.Equals(item.FABCODE) && e.SITEMID.Equals(item.SITEMID))
                                ? int.Parse(fSampleGarmentRcvDs.Where(e => e.FABCODE.Equals(item.FABCODE) && e.SITEMID.Equals(item.SITEMID)).OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE ?? "0") + 1
                                : (int.Parse(fSampleGarmentRcvDs.OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE ?? "0")) + 1;

                            item.BARCODE = $"{itemBarcode}".PadLeft(8, '0');
                            await _fSampleGarmentRcvD.InsertByAsync(item);
                        }
                    }

                    TempData["message"] = "Successfully Updated Sample Garments Receive Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetFSampleGarmentRcvM", $"FSampleGarmentRcvM");
                }

                TempData["message"] = "Failed To Update Sample Garments Receive Information.";
                TempData["type"] = "error";
                return RedirectToAction("EditFSampleGarmentRcvM", $"FSampleGarmentRcvM");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srId"> Belongs to SGRID. Primary key. Must not be null. <see cref="F_SAMPLE_GARMENT_RCV_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditFSampleGarmentRcvM(string srId)
        {
            try
            {
                return View(await _fSampleGarmentRcvM.GetInitObjectsByAsync(await _fSampleGarmentRcvM.FindBySrIdIncludeAllAsync(int.Parse(_protector.Unprotect(srId)))));
            }
            catch (Exception)
            {
                return View($"Error");
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _fSampleGarmentRcvM.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

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
        [Route("SampleGarments")]
        [Route("SampleGarments/GetAll")]
        public IActionResult GetFSampleGarmentRcvM()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createFSampleGarmentRcvMViewModel"> View model. <see cref="CreateFSampleGarmentRcvMViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SampleGarments/Create")]
        public async Task<IActionResult> CreateFSampleGarmentRcvM(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fSampleGarmentRcvM = await _fSampleGarmentRcvM.GetInsertedObjByAsync(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM);

                    if (fSampleGarmentRcvM != null)
                    {
                        //const int count = 10000000;
                        foreach (var item in createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs)
                        {
                            var fSampleGarmentRcvDs = await _fSampleGarmentRcvD.All();

                            var itemBarcode = fSampleGarmentRcvDs.Any(e => e.FABCODE.Equals(item.FABCODE) && e.SITEMID.Equals(item.SITEMID))
                                ? (int.Parse((fSampleGarmentRcvDs.Where(e => e.FABCODE.Equals(item.FABCODE) && e.SITEMID.Equals(item.SITEMID))
                                    .OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE ?? (fSampleGarmentRcvDs.OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE)) ?? "0") + 1)
                                : (int.Parse(fSampleGarmentRcvDs.OrderByDescending(e => e.BARCODE).FirstOrDefault()?.BARCODE ?? "0") + 1);

                            item.SGRID = fSampleGarmentRcvM.SGRID;
                            item.BARCODE = $"{itemBarcode}".PadLeft(8, '0');

                            await _fSampleGarmentRcvD.InsertByAsync(item);
                        }
                    }

                    TempData["message"] = "Successfully Added Sample Garments Receive Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetFSampleGarmentRcvM", $"FSampleGarmentRcvM");
                }

                TempData["message"] = "Failed To Add Sample Garments Receive Information.";
                TempData["type"] = "error";
                return View(await _fSampleGarmentRcvM.GetInitObjectsByAsync(createFSampleGarmentRcvMViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrDeleteFSampleGarmentRcvMDetailsTable(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            try
            {
                ModelState.Clear();
                if (createFSampleGarmentRcvMViewModel.IsDeletable)
                {
                    var fSampleGarmentRcvD = createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs[createFSampleGarmentRcvMViewModel.RemoveIndex];

                    if (fSampleGarmentRcvD.TRNSID > 0)
                    {
                        await _fSampleGarmentRcvD.Delete(fSampleGarmentRcvD);
                    }

                    createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs.RemoveAt(createFSampleGarmentRcvMViewModel.RemoveIndex);
                }
                else
                {
                    if (!createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs.Any(e =>
                        e.SITEMID.Equals(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvD.SITEMID) &&
                        e.FABCODE.Equals(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvD.FABCODE)))
                    {
                        createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs.Add(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvD);
                    }
                }

                return PartialView($"AddOrDeleteFSampleGarmentRcvMDetailsTable", await _fSampleGarmentRcvM.GetInitObjectsByAsync(await _fSampleGarmentRcvM.GetInitObjectsOfSelectedItems(createFSampleGarmentRcvMViewModel)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("SampleGarments/Create")]
        public async Task<IActionResult> CreateFSampleGarmentRcvM()
        {
            var createFSampleGarmentRcvMViewModel = new CreateFSampleGarmentRcvMViewModel
            {
                FSampleGarmentRcvM = new F_SAMPLE_GARMENT_RCV_M { SGRDATE = DateTime.Now }
            };
            
            return View(await _fSampleGarmentRcvM.GetInitObjectsByAsync(createFSampleGarmentRcvMViewModel));
        }
    }
}