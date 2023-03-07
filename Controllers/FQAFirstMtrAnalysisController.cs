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
    [Route("FirstMeterAnalysis")]
    public class FQAFirstMtrAnalysisController : Controller
    {
        private readonly IF_QA_FIRST_MTR_ANALYSIS_M _fQaFirstMtrAnalysisM;
        private readonly IF_QA_FIRST_MTR_ANALYSIS_D _fQaFirstMtrAnalysisD;
        //private readonly PL_PRODUCTION_SETDISTRIBUTION _plProductionSetdistribution;
        private readonly IF_PR_WEAVING_PROCESS_DETAILS_B _fPrWeavingProcessDetailsB;
        private readonly IBAS_YARN_LOTINFO _basYarnLotinfo;
        private readonly IBAS_SUPPLIERINFO _basSupplierinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "First Meter Analysis Information";

        public FQAFirstMtrAnalysisController(IF_QA_FIRST_MTR_ANALYSIS_M fQaFirstMtrAnalysisM,
            IF_QA_FIRST_MTR_ANALYSIS_D fQaFirstMtrAnalysisD,
            IF_HRD_EMPLOYEE hrEmployee,
            //IF_PL_PRODUCTION_SETDISTRIBUTION plProductionSetdistribution,
            IF_PR_WEAVING_PROCESS_DETAILS_B fPrWeavingProcessDetailsB,
            IBAS_YARN_LOTINFO basYarnLotinfo,
            IBAS_SUPPLIERINFO basSupplierinfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fQaFirstMtrAnalysisM = fQaFirstMtrAnalysisM;
            _fQaFirstMtrAnalysisD = fQaFirstMtrAnalysisD;
            //_plProductionSetdistribution = plProductionSetdistribution;
            _fPrWeavingProcessDetailsB = fPrWeavingProcessDetailsB;
            _basYarnLotinfo = basYarnLotinfo;
            _basSupplierinfo = basSupplierinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFirstMtrAnalysisList()
        {
            return View();
        }

        [HttpPost]
        [Route("GetTableData")]
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
            var data = (List<F_QA_FIRST_MTR_ANALYSIS_M>)await _fQaFirstMtrAnalysisM.GetAllFirstMeterAnalysisInformation();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.TRANS_DATE != null && m.TRANS_DATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.EMP.EMPNO != null && m.EMP.EMPNO.ToUpper().Contains(searchValue))
                                       || (m.SET.PROG_.PROG_NO != null && m.SET.PROG_.PROG_NO.ToUpper().Contains(searchValue))
                                       || (m.ACT_DENT != null && m.ACT_DENT.ToString().ToUpper().Contains(searchValue))
                                       || (m.ACT_REED != null && m.ACT_REED.ToString().ToUpper().Contains(searchValue))
                                       || (m.ACT_RS != null && m.ACT_RS.ToString().ToUpper().Contains(searchValue))
                                       || (m.ACT_RATIO != null && m.ACT_RATIO.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
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

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFQAFirstMtrAnalysis()
        {
            var fqaFirstMtrAnalysisMViewModel = new FQAFirstMtrAnalysisMViewModel();
            fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.RPTNO = await _fQaFirstMtrAnalysisM.GetLastReptNoAsync();
            return View(await _fQaFirstMtrAnalysisM.GetInitObjByAsync(fqaFirstMtrAnalysisMViewModel));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFQAFirstMtrAnalysis(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.CREATED_BY = fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.UPDATED_BY = user.Id;
                fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.CREATED_AT = fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var fQaFirstMtrAnalysisM = await _fQaFirstMtrAnalysisM.GetInsertedObjByAsync(fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM);

                if (fQaFirstMtrAnalysisM.FMID > 0)
                {
                    foreach (var item in fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList)
                    {
                        item.FMID = fQaFirstMtrAnalysisM.FMID;
                    }
                    if (fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList.Any())
                    {
                        if (await _fQaFirstMtrAnalysisD.InsertRangeByAsync(fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList))
                        {
                            atLeastOneInsert = true;
                        }
                    }

                    if (!atLeastOneInsert)
                    {
                        await _fQaFirstMtrAnalysisM.Delete(fQaFirstMtrAnalysisM);
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(CreateFQAFirstMtrAnalysis), await _fQaFirstMtrAnalysisM.GetInitObjByAsync(fqaFirstMtrAnalysisMViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFirstMtrAnalysisList), $"FQAFirstMtrAnalysis");
                }

                await _fQaFirstMtrAnalysisM.Delete(fQaFirstMtrAnalysisM);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateFQAFirstMtrAnalysis), await _fQaFirstMtrAnalysisM.GetInitObjByAsync(fqaFirstMtrAnalysisMViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("AddToList")]
        public async Task<IActionResult> AddToFQAFirstMtrAnalysisDetailsList(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fqaFirstMtrAnalysisMViewModel.IsDelete)
                {
                    var fGenSIndentdetails = fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList[fqaFirstMtrAnalysisMViewModel.RemoveIndex];

                    if (fGenSIndentdetails.FM_D_ID > 0)
                    {
                        await _fQaFirstMtrAnalysisD.Delete(fGenSIndentdetails);
                    }

                    fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList.RemoveAt(fqaFirstMtrAnalysisMViewModel.RemoveIndex);
                }
                else
                {
                    if (TryValidateModel(fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisD))
                    {
                        fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList.Add(fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisD);
                    }
                }

                return PartialView($"FQaFirstMtrAnalysisDetailsPartialView", await _fQaFirstMtrAnalysisD.GetInitObjForDetails(fqaFirstMtrAnalysisMViewModel));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetByAllSet/{setId?}")]
        public async Task<IActionResult> GetAllBySetId(int setId)
        {
            try
            {
                return Ok(await _fQaFirstMtrAnalysisM.GetBySetIdAsync(setId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetAllByBeam/{beamId?}")]
        public async Task<IActionResult> GetAllByBeamId(int beamId)
        {
            try
            {
                return Ok(await _fQaFirstMtrAnalysisM.GetByBeamIdAsync(beamId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Edit/{fmaId?}")]
        public async Task<IActionResult> EditFQAFirstMtrAnalysis(string fmaId)
        {
            return View(await _fQaFirstMtrAnalysisM.GetInitObjByAsync(await _fQaFirstMtrAnalysisM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(fmaId)))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditFQAFirstMtrAnalysis(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel)
        {
            if (ModelState.IsValid)
            {
                var fQaFirstMtrAnalysisM = await _fQaFirstMtrAnalysisM.FindByIdAsync(int.Parse(_protector.Unprotect(fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.EncryptedId)));

                if (fQaFirstMtrAnalysisM != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.CREATED_AT = fQaFirstMtrAnalysisM.CREATED_AT;
                    fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.CREATED_BY = fQaFirstMtrAnalysisM.CREATED_BY;
                    fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.UPDATED_AT = DateTime.Now;
                    fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM.UPDATED_BY = user.Id;

                    if (await _fQaFirstMtrAnalysisM.Update(fqaFirstMtrAnalysisMViewModel.FirstMtrAnalysisM))
                    {
                        var fqaFirstMtrAnalysisDList = fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList.Where(d => d.FM_D_ID <= 0).ToList();

                        foreach (var item in fqaFirstMtrAnalysisDList)
                        {
                            item.FMID = fQaFirstMtrAnalysisM.FMID;
                        }

                        if (await _fQaFirstMtrAnalysisD.UpdateRangeByAsync(fqaFirstMtrAnalysisDList))
                        {
                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFirstMtrAnalysisList), $"FQAFirstMtrAnalysis");
                        }
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(EditFQAFirstMtrAnalysis), await _fQaFirstMtrAnalysisM.GetInitObjByAsync(fqaFirstMtrAnalysisMViewModel));
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditFQAFirstMtrAnalysis), await _fQaFirstMtrAnalysisM.GetInitObjByAsync(fqaFirstMtrAnalysisMViewModel));
            }
            TempData["message"] = $"Failed to Add {title}.";
            TempData["type"] = "error";
            return View(nameof(EditFQAFirstMtrAnalysis), await _fQaFirstMtrAnalysisM.GetInitObjByAsync(fqaFirstMtrAnalysisMViewModel));
        }
    }
}
