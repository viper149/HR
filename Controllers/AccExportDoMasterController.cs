using System;
using System.Globalization;
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
    public class AccExportDoMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IACC_EXPORT_DOMASTER _aCcExportDomaster;
        private readonly IACC_EXPORT_DODETAILS _aCcExportDodetails;
        private readonly ICOM_EX_LCINFO _cOmExLcinfo;
        private readonly ICOM_EX_FABSTYLE _cOmExFabstyle;
        private readonly ICOM_EX_PIMASTER _cOmExPimaster;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string title = "Do Information (Export)";

        public AccExportDoMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IACC_EXPORT_DOMASTER aCcExportDomaster,
            IACC_EXPORT_DODETAILS aCcExportDodetails,
            ICOM_EX_LCINFO cOmExLcinfo,
            ICOM_EX_FABSTYLE cOmExFabstyle,
            ICOM_EX_PIMASTER cOmExPimaster,
            UserManager<ApplicationUser> userManager)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _aCcExportDomaster = aCcExportDomaster;
            _aCcExportDodetails = aCcExportDodetails;
            _cOmExLcinfo = cOmExLcinfo;
            _cOmExFabstyle = cOmExFabstyle;
            _cOmExPimaster = cOmExPimaster;
            _userManager = userManager;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsDoNoInUse(AccExportDoMasterViewModel doInfo)
        {
            var isDoNoExists = await _aCcExportDomaster.FindByDoNoInUseAsync(doInfo.ACC_EXPORT_DOMASTER.DONO);
            return !isDoNoExists ? Json(true) : Json($"DO No [ {doInfo.ACC_EXPORT_DOMASTER.DONO} ] is already in use");
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var recordsTotal = 0;

                var data = await _aCcExportDomaster.GetForDataTableByAsync();
                
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.TRNSID.ToString().Contains(searchValue)
                                    || m.DOEX != null && m.DOEX.ToString().Contains(searchValue)
                                    || m.DODATE.ToString(CultureInfo.InvariantCulture).Contains(searchValue)
                                    || m.AUDITON != null && m.AUDITON.ToString().Contains(searchValue)
                                    || !string.IsNullOrEmpty(m.DONO) && m.DONO.ToUpper().Contains(searchValue)
                                    || !string.IsNullOrEmpty(m.LCNO) && m.LCNO.ToUpper().Contains(searchValue)
                                    || !string.IsNullOrEmpty(m.AUDITBY) && m.AUDITBY.ToUpper().Contains(searchValue)
                                    || !string.IsNullOrEmpty(m.COMMENTS) && m.COMMENTS.ToUpper().Contains(searchValue)
                                    || !string.IsNullOrEmpty(m.REMARKS) && m.REMARKS.ToUpper().Contains(searchValue)
                                    || m.ComExLcInfo != null && !string.IsNullOrEmpty(m.ComExLcInfo.LCNO) && m.ComExLcInfo.LCNO.ToUpper().Contains(searchValue)).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.TRNSID.ToString());
                }
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("Account-Finance/ExportDO/GetAll")]
        public IActionResult GetAccDoMasterWithPaged()
        {
            return View();
        }

        [HttpGet]
        [Route("Account-Finance/ExportDO/Details/{trnsId?}")]
        public async Task<IActionResult> DetailsAccDoMaster(string trnsId)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetAccDoMasterWithPaged), $"AccExportDoMaster");

            try
            {
                var accExportDoMasterViewModel = await _aCcExportDomaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(trnsId)));

                if (accExportDoMasterViewModel.ACC_EXPORT_DOMASTER != null)
                {
                    return View(accExportDoMasterViewModel);
                }

                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            catch (Exception)
            {
                TempData["message"] = $"Failed to Retrieve {title}";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccDoMaster()
        {
            return View(await GetInfo());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccDoMaster(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var atLeastOneInsert = false;

                accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.USRID = user.Id;
                accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.IS_CANCELLED = false;

                accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.DONO = $"{await Task.Run(() => _aCcExportDomaster.GetLastDoNoAsync().Result)}".PadLeft(5, '0');
                var accExportDomaster = await _aCcExportDomaster.GetInsertedObjByAsync(accExportDoMasterViewModel.ACC_EXPORT_DOMASTER);

                if (accExportDomaster.TRNSID != 0)
                {
                    foreach (var item in accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList)
                    {
                        item.SOID = item.STYLEID;
                        item.STYLEID = item.STYLE.STYLEID;
                        item.DONO = accExportDomaster.TRNSID;
                        item.USRID = user.Id;
                        item.STYLE = null;

                        var comExPimaster = await _cOmExPimaster.FindByIdAsync(item.PIID ?? 0);
                        comExPimaster.NON_EDITABLE = true;
                        await _cOmExPimaster.Update(comExPimaster);
                    }

                    if (accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList.Any())
                    {
                        if (await _aCcExportDodetails.InsertRangeByAsync(accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList))
                        {
                            atLeastOneInsert = true;
                        }
                    }

                    if (!atLeastOneInsert)
                    {
                        await _aCcExportDomaster.Delete(accExportDomaster);
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(CreateAccDoMaster), await GetInfo());
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetAccDoMasterWithPaged), "AccExportDoMaster");
                }

                await _aCcExportDomaster.Delete(accExportDomaster);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateAccDoMaster), await GetInfo());
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAccDoMaster(string trnsId)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetAccDoMasterWithPaged), $"AccExportDoMaster");

            var accExportDoMasterViewModel = await _aCcExportDomaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(trnsId)));

            if (accExportDoMasterViewModel.ACC_EXPORT_DOMASTER != null)
            {
                return View(await GetInfo(accExportDoMasterViewModel));
            }

            TempData["message"] = $"{title} Not Found.";
            TempData["type"] = "error";
            return redirectToActionResult;
        }

        [HttpPost]
        public async Task<IActionResult> EditAccDoMaster(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            var redirectToActionResult = RedirectToAction(nameof(GetAccDoMasterWithPaged), "AccExportDoMaster");

            if (ModelState.IsValid)
            {
                var accExportDomaster = await _aCcExportDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.EncryptedId)));

                if (accExportDomaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.TRNSDATE = DateTime.Now;

                    if (accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.COMMENTS != null || accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.IS_CANCELLED)
                    {
                        accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.AUDITBY = user.Id;
                        accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.USRID = user.Id;
                        accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.AUDITON = DateTime.Now;
                        accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.DODATE = DateTime.Now;
                    }

                    if (await _aCcExportDomaster.Update(accExportDoMasterViewModel.ACC_EXPORT_DOMASTER))
                    {
                        var accExportDodetailses = accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList.Where(d => d.TRNSID <= 0).ToList();

                        foreach (var item in accExportDodetailses)
                        {
                            item.SOID = item.STYLEID;
                            item.STYLEID = item.STYLE.STYLEID;
                            item.DONO = accExportDomaster.TRNSID;
                            item.USRID = user.Id;
                            item.STYLE = null;
                        }

                        await _aCcExportDodetails.InsertRangeByAsync(accExportDodetailses);

                        TempData["message"] = "Successfully Updated DO Information.";
                        TempData["type"] = "success";
                        return redirectToActionResult;
                    }
                }

                TempData["message"] = $"Failed to Update {title}";
                TempData["type"] = "error";
                return redirectToActionResult;
            }

            TempData["message"] = $"Invalid {title}";
            TempData["type"] = "error";
            return redirectToActionResult;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetLcInfo(int lcId)
        {
            try
            {
                return Ok(await _aCcExportDomaster.GetLCDetailsByLCNoAsync(lcId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoDetails(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (accExportDoMasterViewModel.IsDelete)
                {
                    var aCcExportDodetails = accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList[accExportDoMasterViewModel.RemoveIndex];

                    if (aCcExportDodetails.TRNSID > 0)
                    {
                        await _aCcExportDodetails.Delete(aCcExportDodetails);
                    }

                    accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList.RemoveAt(accExportDoMasterViewModel.RemoveIndex);
                }
                else if (!accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList.Any(e => e.STYLEID.Equals(accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLEID)))
                {
                    if (TryValidateModel(accExportDoMasterViewModel.ACC_EXPORT_DODETAILS))
                    {
                        accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList.Add(accExportDoMasterViewModel.ACC_EXPORT_DODETAILS);
                    }
                }

                Response.Headers["HasItems"] = $"{accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList.Any()}";

                return PartialView("AccDoDetailsTable", await _aCcExportDodetails.GetInitObjForDetailsByAsync(accExportDoMasterViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }

            #region Obsolete

            //try
            //{
            //    var isExistsDoDetails = await _aCcExportDodetails.FindDODetailsBYDONoAndStyleIdAndPIIDAsync(accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.TRNSID, accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PIID, (int)accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLEID);
            //    if (!isExistsDoDetails.Any())
            //    {
            //        var result = accExportDoMasterViewModel.aCC_EXPORT_DODETAILs.Where(e => e.STYLEID == accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLEID && e.PIID == accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PIID).Select(e => e.STYLEID);
            //        if (!result.Any())
            //        {
            //            accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLENAME = await _cOmExFabstyle.FindByIdForStyleNameAsync((int)accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLEID);
            //            if (accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PIID != 0)
            //            {
            //                var pIInfo = await _cOmExPimaster.FindByIdPIInfoAsync(accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PIID);
            //                accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PINO = pIInfo.PINO;
            //            }
            //            accExportDoMasterViewModel.aCC_EXPORT_DODETAILs.Add(new ACC_EXPORT_DODETAILS()
            //            {
            //                STYLEID = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLEID,
            //                STYLENAME = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLENAME,
            //                DONO = accExportDoMasterViewModel.ACC_EXPORT_DOMASTER.TRNSID,
            //                PIID = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PIID,
            //                PINO = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.PINO,
            //                QTY = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.QTY,
            //                RATE = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.RATE,
            //                AMOUNT = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.AMOUNT,
            //                REMARKS = accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.REMARKS,
            //            });
            //        }
            //        return PartialView($"AccDoDetailsTable", accExportDoMasterViewModel);
            //    }
            //    else
            //    {
            //        return PartialView($"AccDoDetailsTable", accExportDoMasterViewModel);
            //    }

            //}
            //catch (Exception)
            //{
            //    return PartialView($"AccDoDetailsTable", accExportDoMasterViewModel);
            //}

            #endregion
        }

        public async Task<AccExportDoMasterViewModel> GetInfo()
        {
            var comExLcDetails = await _cOmExLcinfo.GetAll();
            var comExFabStyleList = await _cOmExFabstyle.GetAll();
            var comExPiInfoList = await _cOmExPimaster.GetAll();

            var accExportDoMasterViewModel = new AccExportDoMasterViewModel
            {
                cOM_EX_LCINFOs = comExLcDetails.Select(c => new COM_EX_LCINFO
                {
                    LCNO = $"{c.LCNO} ({c.FILENO})",
                    LCID = c.LCID
                }).ToList(),
                cOM_EX_FABSTYLEs = comExFabStyleList.ToList(),
                cOM_EX_PIMASTERs = comExPiInfoList.ToList(),
                ACC_EXPORT_DOMASTER = new ACC_EXPORT_DOMASTER
                {
                    DONO = $"{await Task.Run(() => _aCcExportDomaster.GetLastDoNoAsync().Result)}".PadLeft(5, '0'),
                    DODATE = DateTime.Now
                }
            };

            return accExportDoMasterViewModel;
        }

        public async Task<AccExportDoMasterViewModel> GetInfo(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            try
            {
                var comExLcDetails = await _cOmExLcinfo.GetAll();
                var comExFabStyleList = await _cOmExFabstyle.GetAll();
                var comExPiInfoList = await _cOmExPimaster.GetAll();

                accExportDoMasterViewModel.cOM_EX_LCINFOs = comExLcDetails.Select(c => new COM_EX_LCINFO
                {
                    LCNO = $"{c.LCNO} ({c.FILENO})",
                    LCID = c.LCID
                }).ToList();
                accExportDoMasterViewModel.cOM_EX_FABSTYLEs = comExFabStyleList.ToList();
                accExportDoMasterViewModel.cOM_EX_PIMASTERs = comExPiInfoList.ToList();

                return accExportDoMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDoDetailsList(int doNo)
        {
            try
            {
                var comExDoDetailsList = await _aCcExportDodetails.FindDoDetailsListByDoNoAsync(doNo);
                return PartialView($"DoDetailsPreviousList", comExDoDetailsList.ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveDoFromList(AccExportDoMasterViewModel accExportDoMasterViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            accExportDoMasterViewModel.aCC_EXPORT_DODETAILs.RemoveAt(Int32.Parse(removeIndexValue));
            return PartialView($"AccDoDetailsTable", accExportDoMasterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveExistingDoDetails(int doNo, int piId, int styleId)
        {
            try
            {
                var ifExistDOinfo = await _aCcExportDodetails.FindDODetailsBYDONoAndStyleIdAndPIIDAsync(doNo, piId, styleId);

                if (ifExistDOinfo != null)
                {
                    await _aCcExportDodetails.DeleteRange(ifExistDOinfo);
                }

                var comExDoDetailsList = await _aCcExportDodetails.FindDoDetailsListByDoNoAsync(doNo);
                return PartialView($"DoDetailsPreviousList", comExDoDetailsList.ToList());

            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<ACC_EXPORT_DOMASTER> GetDoDetails(int doId)
        {
            try
            {
                var doDetails = await _aCcExportDomaster.GetDoDetails(doId);
                return doDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Reports
        [HttpGet]
        [Route("Account-Finance/ExportDO/After-Audit/GetReports/{trnsId?}")]
        public async Task<IActionResult> RDoAudit(string trnsId)
        {
            var doNo = 0;

            if (trnsId != null)
                doNo = await Task.Run(() => _aCcExportDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId))).Result.TRNSID);
            return View(model: doNo);
        }

        [HttpGet]
        [Route("Account-Finance/ExportDO/Small/GetReports/{trnsId?}")]
        public async Task<IActionResult> RDoReportS(string trnsId)
        {
            var doNo = 0;

            if (trnsId != null)
                doNo = await Task.Run(() => _aCcExportDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId))).Result.TRNSID);
            return View(model: doNo);
        }

        [HttpGet]
        [Route("Account-Finance/ExportDO/Medium/GetReports/{trnsId?}")]
        public async Task<IActionResult> RDoReportM(string trnsId)
        {
            var doNo = 0;

            if (trnsId != null)
                doNo = await Task.Run(() => _aCcExportDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId))).Result.TRNSID);
            return View(model: doNo);
        }

        [HttpGet]
        [Route("Account-Finance/ExportDO/XL/GetReports/{trnsId?}")]
        public async Task<IActionResult> RDoReportXl(string trnsId)
        {
            var doNo = 0;

            if (trnsId != null)
                doNo = await Task.Run(() => _aCcExportDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(trnsId))).Result.TRNSID);
            return View(model: doNo);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetFabStyleListByLcId(int lcId)
        {
            try
            {
                return Ok(await _aCcExportDomaster.GetFabStyleByLcIdAsync(lcId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetPiDetails(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            try
            {
                return Ok(await _aCcExportDodetails.GetPiDetailsByStyleIdAsync(accExportDoMasterViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #region Unused Delete Func
        /***
        [Obsolete]
        [HttpGet]
        public async Task<IActionResult> DeleteAccDoMaster(string doId)
        {
            try
            {
                var doInfo = await aCC_EXPORT_DOMASTER.FindByIdAsync(int.Parse(_protector.Unprotect(doId)));

                if (doInfo != null)
                {
                    var result = await bAS_TEAMINFO.Delete(doInfo);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted DO.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetAccDoMasterWithPaged", $"AccExportDoMaster");
                    }

                    TempData["message"] = "Failed to Delete DO.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetAccDoMasterWithPaged", $"AccExportDoMaster");
                }

                TempData["message"] = "DO Not Found!.";
                TempData["type"] = "error";
                return RedirectToAction("GetAccDoMasterWithPaged", $"AccExportDoMaster");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete DO.";
                TempData["type"] = "error";
                return RedirectToAction("GetAccDoMasterWithPaged", $"AccExportDoMaster");
            }
        }
        ***/
        #endregion
    }
}