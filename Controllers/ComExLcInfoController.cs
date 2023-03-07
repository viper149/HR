using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DenimERP.ViewModels.Com;
using DenimERP.ViewModels.Com.Export;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ComExLcInfoController : Controller
    {
        private readonly ICOM_EX_LCINFO _cOmExLcinfo;
        private readonly ICOM_EX_PIMASTER _cOmExPimaster;
        private readonly ICOM_EX_PI_DETAILS _cOmExPiDetails;
        private readonly ICOM_EX_LCDETAILS _cOmExLcdetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProcessUploadFile _processUploadFile;
        private readonly IDeleteFileFromFolder _deleteFileFromFolder;
        private readonly IBAS_BEN_BANK_MASTER _basBenBankMaster;
        private readonly IBAS_BUYER_BANK_MASTER _basBuyerBankMaster;
        private readonly IDataProtector _protector;

        public ComExLcInfoController(ICOM_EX_LCINFO cOM_EX_LCINFO,
            ICOM_EX_PIMASTER cOM_EX_PIMASTER,
            ICOM_EX_PI_DETAILS cOM_EX_PI_DETAILS,
            ICOM_EX_LCDETAILS cOM_EX_LCDETAILS,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IProcessUploadFile processUploadFile,
            IDeleteFileFromFolder deleteFileFromFolder,
            IBAS_BEN_BANK_MASTER basBenBankMaster,
            IBAS_BUYER_BANK_MASTER basBuyerBankMaster)
        {
            _cOmExLcinfo = cOM_EX_LCINFO;
            _cOmExPimaster = cOM_EX_PIMASTER;
            _cOmExPiDetails = cOM_EX_PI_DETAILS;
            _cOmExLcdetails = cOM_EX_LCDETAILS;
            _userManager = userManager;
            _processUploadFile = processUploadFile;
            _deleteFileFromFolder = deleteFileFromFolder;
            _basBenBankMaster = basBenBankMaster;
            _basBuyerBankMaster = basBuyerBankMaster;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExportLC/GetPiList")]
        public async Task<IActionResult> GetPiInformation(CreateComExLcInfoViewModel comExLcInfoViewModel, string search, int page)
        {
            return Ok(await _cOmExPimaster.GetPiInformationByAsync(comExLcInfoViewModel, search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExportLC/GetFileNo")]
        public async Task<IActionResult> GetFileNo(CreateComExLcInfoViewModel comExLcInfoViewModel)
        {
            return Ok(await _cOmExLcinfo.GetNextLcFileNoByAsync(comExLcInfoViewModel.ComExLcinfo.LCDATE, comExLcInfoViewModel.PREV_YEAR));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsFileNoInUse(CreateComExLcInfoViewModel comExLcInfoViewModel)
        {
            return await _cOmExLcinfo.IsFileNoInUseByAsync(comExLcInfoViewModel.ComExLcinfo.FILENO) ? Json($"File NO: [{comExLcInfoViewModel.ComExLcinfo.FILENO}] already in use.") : Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> GetPiOtherDetails(int piId)
        {
            return PartialView($"GetPIOtherDetailsTable", await _cOmExLcinfo.FindByPiIdIncludeAllAsync(piId));
        }

        [HttpPost]
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

            var data = await _cOmExLcinfo.GetForDataTableByAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.LCNO.ToString().ToUpper().Contains(searchValue)
                                       || m.FILENO != null && m.FILENO.ToUpper().Contains(searchValue)
                                       || m.LCDATE != null && m.LCDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.NTFYBANK.PARTY_BANK != null && m.NTFYBANK.PARTY_BANK.ToUpper().Contains(searchValue)
                                       || m.BUYER.BUYER_NAME != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                       || m.AMENTNO != null && m.AMENTNO.ToUpper().Contains(searchValue)
                                       || m.AMENTDATE != null && m.AMENTDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.SHIP_DATE != null && m.SHIP_DATE.ToString().ToUpper().Contains(searchValue)
                                       || m.UP_DATE != null && m.UP_DATE.ToString().ToUpper().Contains(searchValue)
                                       || m.UDSUBDATE != null && m.UDSUBDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.MLCSUBDATE != null && m.MLCSUBDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.VALUE != null && m.VALUE.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));
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

        [HttpPost]
        public async Task<IActionResult> RemoveFromExLcInfoList(string x, string y)
        {
            try
            {
                var lcid = int.Parse(_protector.Unprotect(x));
                var user = await _userManager.GetUserAsync(User);
                var isExistLcDetails = await _cOmExLcdetails.FindByIdAsync(int.Parse(y));
                var comExPiViewModels = new List<ComExPIViewModel>();

                if (isExistLcDetails != null)
                {
                    isExistLcDetails.ISDELETE = true;
                    isExistLcDetails.USRID = user.Id;
                    await _cOmExLcdetails.Update(isExistLcDetails);

                    var comExLcDetailsList = await _cOmExLcdetails.FindByLcIdIsDeleteAsync(lcid);

                    foreach (var item in comExLcDetailsList)
                    {
                        comExPiViewModels.Add(new ComExPIViewModel
                        {
                            EN_TRNSID = item.TRNSID.ToString(),
                            ComExPimaster = await _cOmExPimaster.FindByIdAsync(item.PIID ?? -1),
                            PIID = item.PIID ?? -1,
                            EncryptedId = _protector.Protect(item.PIID.ToString()),
                            EN_LCNO = _protector.Protect(item.LCID.ToString()),
                            TOTAL = await GetTotalFromPiDetails(item.PIID ?? -1),
                            REMARKS = item.REMARKS,
                            PIFILEUPLOADTEXT = item.PIFILE
                        });
                    }
                    return PartialView($"RemoveFromExLcInfoListTable", comExPiViewModels);
                }
                else
                {
                    var comExLcDetailsList = await _cOmExLcdetails.FindByLcIdIsDeleteAsync(lcid);

                    foreach (var item in comExLcDetailsList)
                    {
                        comExPiViewModels.Add(new ComExPIViewModel
                        {
                            EN_TRNSID = item.TRNSID.ToString(),
                            ComExPimaster = await _cOmExPimaster.FindByIdAsync(item.PIID ?? -1),
                            PIID = item.PIID ?? -1,
                            EncryptedId = _protector.Protect(item.PIID.ToString()),
                            EN_LCNO = _protector.Protect(item.LCID.ToString()),
                            TOTAL = await GetTotalFromPiDetails(item.PIID ?? -1),
                            REMARKS = item.REMARKS,
                            PIFILEUPLOADTEXT = item.PIFILE
                        });
                    }
                    return PartialView($"RemoveFromExLcInfoListTable", comExPiViewModels);
                }
            }
            catch (Exception)
            {
                return Json("Invalid operation.");
            }
        }
        
        [HttpPost]
        [Route("CommercialExportLC/PostEdit")]
        public async Task<IActionResult> PostEditComExLcInfo(CreateComExLcInfoViewModel createComExLcInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var comExLcinfo = await _cOmExLcinfo.FindByIdAsync(int.Parse(_protector.Unprotect(createComExLcInfoViewModel.ComExLcinfo.EncryptedId)));

                    createComExLcInfoViewModel.ComExLcinfo.LCID = comExLcinfo.LCID;
                    //createComExLcInfoViewModel.ComExLcinfo.LCNO = comExLcinfo.LCNO;
                    //createComExLcInfoViewModel.ComExLcinfo.FILENO = comExLcinfo.FILENO;
                    createComExLcInfoViewModel.ComExLcinfo.UDFILEUPLOAD = createComExLcInfoViewModel.UDFILEUPLOAD != null ? _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.UDFILEUPLOAD, "lc_files") : comExLcinfo.UDFILEUPLOAD;
                    createComExLcInfoViewModel.ComExLcinfo.UPFILEUPLOAD = createComExLcInfoViewModel.UPFILEUPLOAD != null ? _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.UPFILEUPLOAD, "lc_files") : comExLcinfo.UPFILEUPLOAD;
                    createComExLcInfoViewModel.ComExLcinfo.COSTSHEETFILEUPLOAD = createComExLcInfoViewModel.COSTSHEETFILEUPLOAD != null ? _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.COSTSHEETFILEUPLOAD, "lc_files") : comExLcinfo.COSTSHEETFILEUPLOAD;
                    createComExLcInfoViewModel.ComExLcinfo.MLCFILE = createComExLcInfoViewModel.MLCFILEUPLOAD != null ? _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.MLCFILEUPLOAD, "lc_files") : comExLcinfo.MLCFILE;
                    createComExLcInfoViewModel.ComExLcinfo.LCFILE = createComExLcInfoViewModel.LCFILEUPLOAD != null ? _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.LCFILEUPLOAD, "lc_files") : comExLcinfo.LCFILE;
                    createComExLcInfoViewModel.ComExLcinfo.USRID = user.Id;
                    createComExLcInfoViewModel.ComExLcinfo.ISDELETE = false;
                    createComExLcInfoViewModel.ComExLcinfo.CREATED_AT = comExLcinfo.CREATED_AT;
                    createComExLcInfoViewModel.ComExLcinfo.CREATED_BY = comExLcinfo.CREATED_BY;
                    createComExLcInfoViewModel.ComExLcinfo.UPDATED_AT = DateTime.Now;
                    createComExLcInfoViewModel.ComExLcinfo.UPDATED_BY = user.Id;

                    if (await _cOmExLcinfo.Update(createComExLcInfoViewModel.ComExLcinfo))
                    {
                        var comExLcdetailses = createComExLcInfoViewModel.ComExLcdetailses.Where(e => e.TRNSID <= 0).ToList();

                        foreach (var item in comExLcdetailses)
                        {
                            item.LCID = comExLcinfo.LCID;
                            item.LCNO = comExLcinfo.LCNO;
                            item.USRID = user.Id;

                            var pi = await _cOmExPimaster.FindByIdAsync(item.PIID ?? 0);
                            if (pi != null && await _cOmExLcdetails.InsertByAsync(item))
                            {
                                pi.LCNO = comExLcinfo.LCNO;
                                await _cOmExPimaster.Update(pi);
                            }
                        }

                        TempData["message"] = "Successfully Updated LC Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
                    }

                    TempData["message"] = "Failed to update LC Information.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
                }

                TempData["message"] = "Failed to update LC Information.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("CommercialExportLC/Edit/{lcId?}")]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> EditComExLcInfo(string lcId)
        {
            var createComExLcInfoViewModel = await _cOmExLcinfo.FindBy_IdIncludeAllAsync(int.Parse(_protector.Unprotect(lcId)));

            if (createComExLcInfoViewModel.ComExLcinfo != null)
            {
                return View(nameof(EditComExLcInfo), await _cOmExLcinfo.GetInitObjByAsync(createComExLcInfoViewModel));
            }

            return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
        }

        [HttpGet]
        [Route("CommercialExportLC/Details/{lcId?}")]
        public async Task<IActionResult> DetailsComExLcInfo(string lcId)
        {
            var findByLcIdWithDetailsIsDeleteAsync = await _cOmExLcinfo.FindByLcIdWithDetailsIsDeleteAsync(int.Parse(_protector.Unprotect(lcId)));

            if (findByLcIdWithDetailsIsDeleteAsync != null)
            {
                return View(findByLcIdWithDetailsIsDeleteAsync);
            }

            TempData["message"] = "Not found LC Info.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
        }

        [HttpGet]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> DeleteComExLcInfo(string lcId)
        {
            var createComExLcInfoViewModel = await _cOmExLcinfo.FindByLcIdForDeleteAsync(int.Parse(_protector.Unprotect(lcId)));

            if (createComExLcInfoViewModel.ComExLcinfo != null)
            {
                if (createComExLcInfoViewModel.ComExLcdetailses.Any())
                {
                    await _cOmExLcdetails.DeleteRange(createComExLcInfoViewModel.ComExLcdetailses);
                }

                await _cOmExLcinfo.Delete(createComExLcInfoViewModel.ComExLcinfo);

                TempData["message"] = "Successfully Deleted LC Information.";
                TempData["type"] = "success";
            }
            else
            {
                TempData["message"] = "Failed to delete LC information.";
                TempData["type"] = "error";
            }

            return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
        }
        
        [HttpGet]
        [Route("CommercialExportLC/Amendment/{lcId?}")]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> AmendmentComExLcInfo(string lcId)
        {
            try
            {
                var createComExLcInfoViewModel = await _cOmExLcinfo.FindBy_IdIncludeAllAsync(int.Parse(_protector.Unprotect(lcId)));

                createComExLcInfoViewModel.ComExLcinfo.LCDATE = DateTime.Now;
                createComExLcInfoViewModel.ComExLcinfo.FILENO = await _cOmExLcinfo.GetNextLcFileNoByAsync();
                createComExLcInfoViewModel.ComExLcinfo.TEAMID = 41;
                createComExLcInfoViewModel.ComExLcinfo.LC_STATUS = true;
                createComExLcInfoViewModel.ComExLcinfo.VALUE = null;

                return View($"CreateComExLcInfo", await _cOmExLcinfo.GetInitObjByAsync(createComExLcInfoViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Amendment LC information.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
            }
        }

        [HttpGet]
        [Route("CommercialExportLC")]
        [Route("CommercialExportLC/GetAll")]
        public IActionResult GetComExLcInfoList()
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
        public async Task<IActionResult> RemoveFromList(ComExLcInfoViewModel comExLcInfoViewModel, int removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (!string.IsNullOrEmpty(comExLcInfoViewModel.comExPIViewModels[removeIndexValue].PIFILEUPLOADTEXT))
                {
                    _deleteFileFromFolder.DeleteFileFromContentRootPath(comExLcInfoViewModel.comExPIViewModels[removeIndexValue].PIFILEUPLOADTEXT, "lc_files/pi_files");
                }

                comExLcInfoViewModel.comExPIViewModels.RemoveAt(removeIndexValue);

                foreach (var item in comExLcInfoViewModel.comExPIViewModels)
                {
                    item.ComExPimaster = await _cOmExPimaster.FindByIdAsync(item.PIID);
                    item.BasBenBankMaster = await _basBenBankMaster.FindByIdAsync(item.BANKID ?? 0);
                    item.BasBenBankMaster = await _basBenBankMaster.FindByIdAsync(item.BANK_ID ?? 0);
                }

                return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
            }
            catch (Exception)
            {
                return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
            }
        }

        [HttpPost]
        public async Task<double?> GetTotalFromPiDetails(int piId)
        {
            return await _cOmExPiDetails.GetTotalSumByPiNoAsync(piId);
        }

        [HttpPost]
        public async Task<IActionResult> AddComExLcInfo(CreateComExLcInfoViewModel createComExLcInfoViewModel)
        {
            try
            {
                ModelState.Clear();
                if (createComExLcInfoViewModel.IsDelete)
                {
                    var comExLcdetails = createComExLcInfoViewModel.ComExLcdetailses[createComExLcInfoViewModel.RemoveIndex];

                    if (comExLcdetails.TRNSID > 0)
                    {
                        await _cOmExLcdetails.Delete(comExLcdetails);
                    }

                    createComExLcInfoViewModel.ComExLcdetailses.RemoveAt(createComExLcInfoViewModel.RemoveIndex);
                }
                else
                {
                    if (!createComExLcInfoViewModel.ComExLcdetailses.Any(e => e.PIID.Equals(createComExLcInfoViewModel.ComExLcdetails.PIID))
                        && _cOmExLcdetails.IsAdvisingBankMatched(createComExLcInfoViewModel.ComExLcdetails.PIID, createComExLcInfoViewModel.ComExLcdetails.BANKID).Result
                        && createComExLcInfoViewModel.ComExLcdetails.BANKID != null
                        && createComExLcInfoViewModel.ComExLcdetails.PIID != null)
                    {
                        createComExLcInfoViewModel.ComExLcdetailses.Add(new COM_EX_LCDETAILS
                        {
                            PIID = createComExLcInfoViewModel.ComExLcdetails.PIID,
                            BANKID = createComExLcInfoViewModel.ComExLcdetails.BANKID,
                            BANK_ID = createComExLcInfoViewModel.ComExLcdetails.BANK_ID,
                            REMARKS = createComExLcInfoViewModel.ComExLcdetails.REMARKS,
                            PIFILE = _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.PiFile, "lc_files/pi_files")
                        });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bank name mismatched. Can not add PI details.");
                        ModelState.AddModelError("", "Or duplicates PI occurred.");
                    }
                }

                return PartialView("CreateComExLcInfoTable", await _cOmExLcinfo.GetInitObjForDetailsTable(createComExLcInfoViewModel));

                #region MyRegion

                //if (!string.IsNullOrEmpty(createComExLcInfoViewModel.cOM_EX_LCINFO.EncryptedId))
                //{
                //    var findByIdIncludeAllAsync = await _cOmExLcinfo.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(comExLcInfoViewModel.cOM_EX_LCINFO.EncryptedId)));

                //    if (!findByIdIncludeAllAsync.COM_EX_LCDETAILS.Any(e => e.PIID.Equals(comExLcInfoViewModel.comExPIViewModel.PIID) && e.ISDELETE.Equals(false)) &&
                //        !comExLcInfoViewModel.comExPIViewModels.Any(e => e.PIID.Equals(comExLcInfoViewModel.comExPIViewModel.PIID)))
                //    {
                //        comExLcInfoViewModel.comExPIViewModel.ComExPimaster = await _cOmExPimaster.FindByIdAsync(comExLcInfoViewModel.comExPIViewModel.PIID);
                //        comExLcInfoViewModel.comExPIViewModel.BasBenBankMaster = await _basBenBankMaster.FindByIdAsync(comExLcInfoViewModel.comExPIViewModel.BANKID ?? 0);
                //        comExLcInfoViewModel.comExPIViewModel.BasBenBankMaster_Nego = await _basBenBankMaster.FindByIdAsync(comExLcInfoViewModel.comExPIViewModel.BANK_ID ?? 0);
                //        comExLcInfoViewModel.comExPIViewModel.PIFILEUPLOADTEXT = _processUploadFile.ProcessUploadFileToContentRootPath(comExLcInfoViewModel.PiFile, "lc_files/pi_files");
                //        comExLcInfoViewModel.comExPIViewModels.Add(comExLcInfoViewModel.comExPIViewModel);
                //    }
                //}
                //else
                //{
                //    if (!comExLcInfoViewModel.comExPIViewModels.Any(e => e.PIID.Equals(comExLcInfoViewModel.comExPIViewModel.PIID)))
                //    {
                //        comExLcInfoViewModel.comExPIViewModel.PIFILEUPLOADTEXT = _processUploadFile.ProcessUploadFileToContentRootPath(comExLcInfoViewModel.PiFile, "lc_files/pi_files");
                //        comExLcInfoViewModel.comExPIViewModels.Add(comExLcInfoViewModel.comExPIViewModel);

                //    }
                //}

                //return PartialView($"CreateComExLcInfoTable", await _cOmExLcinfo.GetInitObjForDetailsTable(comExLcInfoViewModel));

                #endregion

                #region Old Reference

                //if (!string.IsNullOrEmpty(comExLcInfoViewModel.cOM_EX_LCINFO.EncryptedId))
                //{
                //    #region Obsolete Reference
                //    var comExLcinfo = await _cOmExLcinfo.FindByIdAsync(int.Parse(_protector.Unprotect(comExLcInfoViewModel.cOM_EX_LCINFO.EncryptedId)));
                //    var findByLcNoAndPiIdAsync = await _cOmExLcdetails.FindByLcNoAndPiIdAsync(comExLcInfoViewModel.comExPIViewModel.PIID, comExLcinfo.LCID);

                //    if (findByLcNoAndPiIdAsync != null)
                //    {
                //        var isExistButDeletedComExPiDetails = await _cOmExLcdetails.FindByLcNoAndPiIdIsDeleteAsync(comExLcInfoViewModel.comExPIViewModel.PIID, comExLcinfo.LCID);
                //        if (isExistButDeletedComExPiDetails != null)
                //        {
                //            var piInfo = await _cOmExPimaster.FindByIdAsync(comExLcInfoViewModel.comExPIViewModel.PIID);
                //            var piNoList = comExLcInfoViewModel.comExPIViewModels.Where(e => e.PIID.Equals(comExLcInfoViewModel.comExPIViewModel.PIID));

                //            if (!piNoList.Any())
                //            {
                //                if (piInfo != null)
                //                {
                //                    comExLcInfoViewModel.comExPIViewModel.PINO = piInfo.PINO;
                //                }

                //                comExLcInfoViewModel.comExPIViewModel.PIFILEUPLOADTEXT = _processUploadFile.ProcessUploadFileToContentRootPath(comExLcInfoViewModel.PiFile, "lc_files/pi_files");
                //                comExLcInfoViewModel.comExPIViewModels.Add(comExLcInfoViewModel.comExPIViewModel);
                //                return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //            }

                //            return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //        }
                //        else
                //        {
                //            if (comExLcInfoViewModel.comExPIViewModel.PIID <= 0)
                //                return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //            var piInfo = await _cOmExPimaster.FindByIdAsync(comExLcInfoViewModel.comExPIViewModel.PIID);
                //            var piNoList = comExLcInfoViewModel.comExPIViewModels.Where(e => e.PIID == comExLcInfoViewModel.comExPIViewModel.PIID);

                //            if (piNoList.Count() != 0) return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //            if (piInfo != null)
                //            {
                //                comExLcInfoViewModel.comExPIViewModel.PINO = piInfo.PINO;
                //            }

                //            comExLcInfoViewModel.comExPIViewModel.PIFILEUPLOADTEXT = _processUploadFile.ProcessUploadFileToContentRootPath(comExLcInfoViewModel.PiFile, "lc_files/pi_files");
                //            comExLcInfoViewModel.comExPIViewModels.Add(comExLcInfoViewModel.comExPIViewModel);
                //            return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //        }
                //    }
                //    #endregion
                //}
                //else
                //{
                //    if (comExLcInfoViewModel.comExPIViewModel.PIID < 0)
                //        return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //    var piInfo = await _cOmExPimaster.FindByIdAsync(comExLcInfoViewModel.comExPIViewModel.PIID);
                //    var piNoList = comExLcInfoViewModel.comExPIViewModels.Where(e => e.PIID.Equals(comExLcInfoViewModel.comExPIViewModel.PIID));

                //    if (piNoList.Any()) return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //    if (piInfo != null)
                //    {
                //        comExLcInfoViewModel.comExPIViewModel.PINO = piInfo.PINO;
                //    }

                //    comExLcInfoViewModel.comExPIViewModel.PIFILEUPLOADTEXT = _processUploadFile.ProcessUploadFileToContentRootPath(comExLcInfoViewModel.PiFile, "lc_files/pi_files");
                //    comExLcInfoViewModel.comExPIViewModels.Add(comExLcInfoViewModel.comExPIViewModel);
                //    return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);
                //}

                //return PartialView($"CreateComExLcInfoTable", comExLcInfoViewModel);

                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> CreateComExLcInfo()
        {
            var createComExLcInfoViewModel = new CreateComExLcInfoViewModel
            {
                ComExLcinfo = new COM_EX_LCINFO
                {
                    LCRCVDATE = DateTime.Now,
                    FILENO = await _cOmExLcinfo.GetNextLcFileNoByAsync(),
                    TEAMID = 41,
                    LC_STATUS = true,
                    ITEM = "%(+/-)"
                }
            };

            return View(await _cOmExLcinfo.GetInitObjByAsync(createComExLcInfoViewModel));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateComExLcInfo(CreateComExLcInfoViewModel createComExLcInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    createComExLcInfoViewModel.ComExLcinfo.USRID = user.Id;
                    createComExLcInfoViewModel.ComExLcinfo.FILENO = await _cOmExLcinfo.GetNextLcFileNoByAsync(createComExLcInfoViewModel.ComExLcinfo.LCDATE, createComExLcInfoViewModel.PREV_YEAR);
                    createComExLcInfoViewModel.ComExLcinfo.UDFILEUPLOAD = _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.UDFILEUPLOAD, "lc_files");
                    createComExLcInfoViewModel.ComExLcinfo.UPFILEUPLOAD = _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.UPFILEUPLOAD, "lc_files");
                    createComExLcInfoViewModel.ComExLcinfo.COSTSHEETFILEUPLOAD = _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.COSTSHEETFILEUPLOAD, "lc_files");
                    createComExLcInfoViewModel.ComExLcinfo.MLCFILE = _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.MLCFILEUPLOAD, "lc_files");
                    createComExLcInfoViewModel.ComExLcinfo.LCFILE = _processUploadFile.ProcessUploadFileToContentRootPath(createComExLcInfoViewModel.LCFILEUPLOAD, "lc_files");
                    createComExLcInfoViewModel.ComExLcinfo.ISDELETE = false;
                    createComExLcInfoViewModel.ComExLcinfo.CREATED_AT = createComExLcInfoViewModel.ComExLcinfo.UPDATED_AT = DateTime.Now;
                    createComExLcInfoViewModel.ComExLcinfo.CREATED_BY = createComExLcInfoViewModel.ComExLcinfo.UPDATED_BY = user.Id;

                    var comExLcinfo = await _cOmExLcinfo.GetInsertedObjByAsync(createComExLcInfoViewModel.ComExLcinfo);

                    if (comExLcinfo != null)
                    {
                        var existingLc = await _cOmExLcinfo.GetAllLcByLcNo(comExLcinfo.LCNO);

                        foreach (var item in existingLc)
                        {
                            item.EX_DATE = comExLcinfo.EX_DATE;
                            item.SHIP_DATE = comExLcinfo.SHIP_DATE;
                            await _cOmExLcinfo.Update(item);
                        }

                        foreach (var item in createComExLcInfoViewModel.ComExLcdetailses)
                        {
                            item.LCID = comExLcinfo.LCID;
                            item.LCNO = comExLcinfo.LCNO;
                            item.USRID = user.Id;
                            item.ISDELETE = false;
                            var pi = await _cOmExPimaster.FindByIdAsync(item.PIID??0);
                            if (pi != null && await _cOmExLcdetails.InsertByAsync(item))
                            {
                                pi.LCNO = comExLcinfo.LCNO;
                                await _cOmExPimaster.Update(pi);
                            }
                        }
                        TempData["message"] = "Successfully Added LC Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetComExLcInfoList), $"ComExLcInfo");
                    }

                    TempData["message"] = "Failed to Add LC Information";
                    TempData["type"] = "error";
                    return View(await _cOmExLcinfo.GetInitObjByAsync(createComExLcInfoViewModel));
                }

                return View(await _cOmExLcinfo.GetInitObjByAsync(createComExLcInfoViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> IsLcNoInUse(ComExLcInfoViewModel comExLcInfoViewModel)
        //{
        //    try
        //    {
        //        comExLcInfoViewModel.cOM_EX_LCINFO = await _cOmExLcinfo.FindByLcNoIsDeleteAsync(comExLcInfoViewModel.cOM_EX_LCINFO.LCNO);
        //        if (comExLcInfoViewModel.cOM_EX_LCINFO == null) return Json(true);

        //        comExLcInfoViewModel = await _cOmExLcinfo.InitComExLcInfoViewModel(comExLcInfoViewModel);
        //        comExLcInfoViewModel.cOM_EX_LCINFO.FILENO = await _cOmExLcinfo.GetNextLcFileNoByAsync();
        //        return RedirectToAction($"CreateComExLcInfo",comExLcInfoViewModel);
        //    }
        //    catch (Exception)
        //    {
        //        return Json("An error occurred during your submission. Please Try again later.");
        //    }

        //}

        //[AcceptVerbs("Get", "Post")]
        //[Route("CommercialExportLC/PRC-Register")]
        //public async Task<IActionResult> RPRC_Register()
        //{
        //    return View();
        //}
    }
}