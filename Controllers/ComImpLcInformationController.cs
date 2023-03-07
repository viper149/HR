using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace DenimERP.Controllers
{
    [Authorize]
    public class ComImpLcInformationController : Controller
    {
        private readonly ICOM_IMP_LCINFORMATION _cOmImpLcinformation;
        private readonly IBAS_PRODCATEGORY _bAsProdcategory;
        private readonly ICOM_IMP_LCDETAILS _cOmImpLcdetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDeleteFileFromFolder _iDeleteFileFromFolder;
        private readonly ICOM_EX_LCINFO _comExLcinfo;
        private readonly IProcessUploadFile _processUploadFile;
        private readonly IDataProtector _protector;

        public ComImpLcInformationController(ICOM_IMP_LCINFORMATION cOM_IMP_LCINFORMATION,
            IBAS_PRODCATEGORY bAS_PRODCATEGORY,
            ICOM_IMP_LCDETAILS cOM_IMP_LCDETAILS,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IDeleteFileFromFolder iDeleteFileFromFolder,
            ICOM_EX_LCINFO comExLcinfo,
            IProcessUploadFile processUploadFile)
        {
            _cOmImpLcinformation = cOM_IMP_LCINFORMATION;
            _bAsProdcategory = bAS_PRODCATEGORY;
            _cOmImpLcdetails = cOM_IMP_LCDETAILS;
            _userManager = userManager;
            _iDeleteFileFromFolder = iDeleteFileFromFolder;
            _comExLcinfo = comExLcinfo;
            _processUploadFile = processUploadFile;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialImportLC/IsFileNoInUse")]
        public async Task<IActionResult> IsFileNoInUse(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            return await _cOmImpLcinformation.FindFileNoByAsync(comImpLcInformationForCreateViewModel)
                ? Json($"File No [{comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.FILENO}] is already in use.")
                : Json(true);
        }

        [HttpGet]
        [Route("CommercialImportLC/GetReports")]
        public IActionResult RGetComImpLcInformationList()
        {
            return View();
        }

        private async Task<IEnumerable<COM_IMP_LCDETAILS>> LoadPreviousListA(int masterLcNo)
        {
            try
            {
                var findByExportLcIdAsync = await _cOmImpLcinformation.FindByExportLCIdAsync(masterLcNo);
                var comImpLcdetailses = new List<COM_IMP_LCDETAILS>();

                foreach (var item in findByExportLcIdAsync)
                {
                    comImpLcdetailses.AddRange((List<COM_IMP_LCDETAILS>)await _cOmImpLcdetails.FindByLcNoAsync(item.LCNO));
                }

                return comImpLcdetailses;
            }
            catch (Exception)
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<IActionResult> LoadPreviousList(int masterLcNo)
        {
            try
            {
                var loadPreviousList = await LoadPreviousListA(masterLcNo);
                return PartialView($"LoadPreviousListTable", loadPreviousList);
            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetExportLcZenbuJyuhou(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            try
            {
                // FIND BY LCID
                var findByIdIsDeleteFalseAsync = await _comExLcinfo.FindByIdIsDeleteFalseAsync(comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.LCID ?? 0);

                // IF IMPORT LC CREATE & EXPIRE DATE HAS VALUE 
                if (findByIdIsDeleteFalseAsync.LCDATE.HasValue && comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.EXPDATE.HasValue)
                {
                    // AMENTDATE NOT NULL
                    if (findByIdIsDeleteFalseAsync.AMENTDATE == null)
                    {
                        return Json(findByIdIsDeleteFalseAsync);
                    }
                    else
                    {
                        var comExLcInfoToExtendViewModel = new ComExLcInfoToExtendViewModel()
                        {
                            ComExLcInfo = findByIdIsDeleteFalseAsync,
                            Override = true
                        };

                        return Json(comExLcInfoToExtendViewModel);
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            catch (Exception)
            {
                return Json(new EmptyResult());
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> LoadStepper(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateView)
        //{
        //    try
        //    {
        //        comImpLcInformationForCreateView.ComExLcinfos = (List<COM_EX_LCINFO>)await _comExLcinfo.GetAllGreaterThan(comImpLcInformationForCreateView.cOM_IMP_LCINFORMATION.LCDATE);
        //        return PartialView($"LoadStepperTable", comImpLcInformationForCreateView);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetFileNumberByCategoryId(int categoryId)
        {
            try
            {
                var fileNumberByCategory = await _cOmImpLcinformation.GetFileNumberByCategory(categoryId);
                return Json(fileNumberByCategory);
            }
            catch (Exception)
            {
                return Json($"An error occurred during processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveComImpLcDetailsPreviousList(string x, string y)
        {
            try
            {
                var lcId = int.Parse(_protector.Unprotect(y));
                var comImpLcInformation = await _cOmImpLcinformation.FindByIdAsync(lcId);
                var comImpDetails = await _cOmImpLcdetails.FindByIdAsync(int.Parse(_protector.Unprotect(x)));
                var basProductCategoriesList = await _bAsProdcategory.GetAll();
                var comImpLcInformationEditViewModel = new ComImpLcInformationEditViewModel();

                if (comImpDetails != null)
                {
                    await _cOmImpLcdetails.Delete(comImpDetails);
                    var comImpLcDetailsList = await _cOmImpLcdetails.FindByLcNoAsync(comImpLcInformation.LCNO);


                    comImpLcInformationEditViewModel.PrevComImpLcdetailses = comImpLcDetailsList.Select(e =>
                    {
                        e.EncryptedId = _protector.Protect(e.TRNSID.ToString());
                        return e;

                    }).ToList();

                    comImpLcInformationEditViewModel.bAS_PRODCATEGORies = basProductCategoriesList.ToList();
                    comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.EncryptedId = y;

                    return PartialView($"RemoveComImpLcDetailsPreviousListTable", comImpLcInformationEditViewModel);
                }
                else
                {
                    var comImpLcDetailsList = await _cOmImpLcdetails.FindByLcNoAsync(comImpLcInformation.LCNO);

                    comImpLcInformationEditViewModel.PrevComImpLcdetailses = comImpLcDetailsList.Select(e =>
                    {
                        e.EncryptedId = _protector.Protect(e.TRNSID.ToString());
                        return e;

                    }).ToList();

                    comImpLcInformationEditViewModel.bAS_PRODCATEGORies = basProductCategoriesList.ToList();
                    comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.EncryptedId = y;

                    return PartialView($"RemoveComImpLcDetailsPreviousListTable", comImpLcInformationEditViewModel);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        
        [Route("CommercialImportLC/PostEdit")]
        public async Task<IActionResult> PostEditComImpLcInformation(ComImpLcInformationEditViewModel comImpLcInformationEditViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (comImpLcInformationEditViewModel.LCPATH != null)
                {
                    comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.LCPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comImpLcInformationEditViewModel.LCPATH, "lc_files");
                }

                comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.USRID = user.Id;
                comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.LC_ID = int.Parse(_protector.Unprotect(comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.EncryptedId));

                if (await _cOmImpLcinformation.Update(comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION))
                {
                    var comImpLcdetailses = comImpLcInformationEditViewModel.cOM_IMP_LCDETAILs.Where(e => e.TRNSID <= 0).ToList();

                    foreach (var item in comImpLcdetailses)
                    {
                        item.LC_ID = comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.LC_ID;
                        item.LCNO = comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.LCNO;
                        item.TOTAL = (item.QTY * item.RATE);
                        item.USRID = user.Id;
                    }

                    await _cOmImpLcdetails.InsertRangeByAsync(comImpLcdetailses);

                    TempData["message"] = "Successfully Updated LC Information.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Failed To Update LC Information.";
                    TempData["type"] = "error";
                }

                return RedirectToAction(nameof(GetComImpLcInformation), $"ComImpLcInformation");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed To Update LC Information.";
                TempData["type"] = "error";
                return View(nameof(EditComImpLcInformation), comImpLcInformationEditViewModel);
            }
        }

        [HttpGet]
        [Route("CommercialImportLC/Edit/{lcId?}")]
        public async Task<IActionResult> EditComImpLcInformation(string lcId)
        {
            try
            {
                var findByIdInlcudeOtherObjAsync = await _cOmImpLcinformation.FindByIdInlcudeOtherObjAsync(int.Parse(_protector.Unprotect(lcId)));

                if (findByIdInlcudeOtherObjAsync == null)
                    return RedirectToAction(nameof(GetComImpLcInformation), $"ComImpLcInformation");

                var comImpLcInformationEditViewModel = new ComImpLcInformationEditViewModel();

                await _cOmImpLcinformation.GetComImpLcInformationForCreateViewModelValue(comImpLcInformationEditViewModel);

                comImpLcInformationEditViewModel.cOM_IMP_LCDETAILs = findByIdInlcudeOtherObjAsync.COM_IMP_LCDETAILS.Select(e => new COM_IMP_LCDETAILS
                {
                    TRNSID = e.TRNSID,
                    TRNSDATE = e.TRNSDATE,
                    LCNO = e.LCNO,
                    LC_ID = e.LC_ID,
                    PINO = e.PINO,
                    PIDATE = e.PIDATE,
                    PIPATH = e.PIPATH,
                    PRODID = e.PRODID,
                    UNIT = e.UNIT,
                    QTY = e.QTY,
                    RATE = e.RATE,
                    TOTAL = e.TOTAL,
                    USRID = e.USRID,
                    HSCODE = e.HSCODE,
                    F_BAS_UNITS = new F_BAS_UNITS
                    {
                        UID = e.F_BAS_UNITS?.UID ?? 0,
                        UNAME = e.F_BAS_UNITS?.UNAME
                    },
                    BAS_PRODUCTINFO = new BAS_PRODUCTINFO
                    {
                        PRODID = e.BAS_PRODUCTINFO?.PRODID ?? 0,
                        PRODNAME = e.BAS_PRODUCTINFO?.PRODNAME
                    },
                    LC = new COM_IMP_LCINFORMATION
                    {
                        LCID = e.LC.LCID
                    }
                }).ToList();

                comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION = findByIdInlcudeOtherObjAsync;
                comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.EncryptedId = _protector.Protect(findByIdInlcudeOtherObjAsync.LC_ID.ToString());
                comImpLcInformationEditViewModel.cOM_IMP_LCDETAILS.LCNO = findByIdInlcudeOtherObjAsync.LCNO;
                comImpLcInformationEditViewModel.cOM_IMP_LCINFORMATION.LCID = findByIdInlcudeOtherObjAsync.LCID;

                return View(comImpLcInformationEditViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("CommercialImportLC/Details/{lcId?}")]
        public async Task<IActionResult> DetailsComImpLcInformation(string lcId)
        {
            return View(await _cOmImpLcinformation.FindByLcIdInlcudeAllAsync(int.Parse(_protector.Unprotect(lcId))));
        }

        [HttpGet]
        [Route("CommercialImportLC/Delete/{lcId?}")]
        public async Task<IActionResult> DeleteComImpLcInformation(string lcId)
        {
            try
            {
                var comImpLcInformation = await _cOmImpLcinformation.FindByIdAsync(int.Parse(_protector.Unprotect(lcId)));
                if (comImpLcInformation != null)
                {
                    var isExistComImpLcDetailsList = await _cOmImpLcdetails.FindByLcNoAsync(comImpLcInformation.LCNO);

                    if (isExistComImpLcDetailsList != null)
                    {
                        await _cOmImpLcdetails.DeleteRange(isExistComImpLcDetailsList);
                    }

                    await _cOmImpLcinformation.Delete(comImpLcInformation);

                    TempData["message"] = "Successfully Deleted LC Information.";
                    TempData["type"] = "success";
                    return RedirectToAction($"GetComImpLcInformation", $"ComImpLcInformation");
                }
                TempData["message"] = "Failed to Deleted LC Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComImpLcInformation", $"ComImpLcInformation");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("CommercialImportLC/GetTableData")]
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

                var comImpLcInformation = (List<COM_IMP_LCINFORMATION>)await _cOmImpLcinformation.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                comImpLcInformation = comImpLcInformation.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            comImpLcInformation = comImpLcInformation.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    comImpLcInformation = comImpLcInformation.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    comImpLcInformation = comImpLcInformation.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }

                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    comImpLcInformation = comImpLcInformation
                        .Where(m => m.LCNO.ToString().Contains(searchValue)
                                    || m.LC_ID.ToString().ToUpper().Contains(searchValue)
                                    || m.LCDATE.ToShortDateString().ToUpper().Contains(searchValue)
                                    || m.ComExLcinfo.LCDATE != null && m.ComExLcinfo.LCDATE.ToString().ToUpper().Contains(searchValue)
                                    || m.COM_IMP_LCTYPE != null && m.COM_IMP_LCTYPE.TYPENAME.ToUpper().Contains(searchValue)
                                    || m.CURRENCY != null && m.CURRENCY.ToUpper().Contains(searchValue)
                                    || m.LIENVAL != null && m.LIENVAL.ToString().ToUpper().Contains(searchValue)
                                    || m.ComExLcinfo is {VALUE: { }} && m.ComExLcinfo.VALUE.ToString().ToUpper().Contains(searchValue)
                                    || !IsNullOrEmpty(m.ComExLcinfo.LCNO) && m.LCDATE.ToShortDateString().ToUpper().Contains(searchValue)
                                    || !IsNullOrEmpty(m.LCDATE.ToShortDateString()) && m.LCDATE.ToShortDateString().ToUpper().Contains(searchValue)
                                    || !IsNullOrEmpty(m.SHIPDATE.ToString()) && m.SHIPDATE.ToString().ToUpper().Contains(searchValue)
                                    || !IsNullOrEmpty(m.EXPDATE.ToString()) && m.EXPDATE.ToString().ToUpper().Contains(searchValue)
                                    || m.SUPP != null && m.SUPP.SUPPNAME.ToUpper().Contains(searchValue)
                                    || m.CAT != null && m.CAT.CATEGORY.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = comImpLcInformation.Count();
                var finalData = comImpLcInformation.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.IsLocked = User.IsInRole("Super Admin") || User.IsInRole("Admin") || User.IsInRole("Com Imp");
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = finalData
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("CommercialImportLC")]
        [Route("CommercialImportLC/GetAll")]
        public IActionResult GetComImpLcInformation()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsLcNoInUse(ComImpLcInformationForCreateViewModel obj)
        {
            var result = await _cOmImpLcinformation.FindByLcNoAsync(obj.cOM_IMP_LCINFORMATION.LCNO);
            return result ? Json($"Lc no [ {obj.cOM_IMP_LCINFORMATION.LCNO} ] already in use.") : Json(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveComImpLcDetails(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            try
            {
                ModelState.Clear();
                var basProdcategories = await _bAsProdcategory.GetAll();
                comImpLcInformationForCreateViewModel.bAS_PRODCATEGORies = basProdcategories.ToList();

                if (comImpLcInformationForCreateViewModel.RemoveIndex < 0) return PartialView($"AddComImpLcDetailsTable", comImpLcInformationForCreateViewModel);
                _iDeleteFileFromFolder.DeleteFile(comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs[comImpLcInformationForCreateViewModel.RemoveIndex].PIPATH, "lc_files");
                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs.RemoveAt(comImpLcInformationForCreateViewModel.RemoveIndex);
                return PartialView($"AddComImpLcDetailsTable", comImpLcInformationForCreateViewModel);
            }
            catch (Exception)
            {
                return Json("An error occurred during your submission.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComImpLcDetails(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            ModelState.Clear();
            if (comImpLcInformationForCreateViewModel.IsDelete)
            {
                var cOmImpLcdetaiL = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs[comImpLcInformationForCreateViewModel.RemoveIndex];

                if (cOmImpLcdetaiL.TRNSID > 0)
                {
                    await _cOmImpLcdetails.Delete(cOmImpLcdetaiL);
                }

                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs.RemoveAt(comImpLcInformationForCreateViewModel.RemoveIndex);
            }
            else
            {
                // CAN ADD DUPLICATES PRODUCTS
                //if (!comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs.Any(e => e.PRODID.Equals(comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.PRODID)))
                //{
                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs.Add(new Models.COM_IMP_LCDETAILS
                {
                    TRNSDATE = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.TRNSDATE,
                    LCNO = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.LCNO,
                    PINO = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.PINO,
                    PIDATE = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.PIDATE,
                    PIPATH = comImpLcInformationForCreateViewModel.PIPATH != null ? _processUploadFile.ProcessUploadFileToContentRootPath(comImpLcInformationForCreateViewModel.PIPATH, "lc_files") : null,
                    PRODID = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.PRODID,
                    UNIT = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.UNIT,
                    QTY = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.QTY,
                    RATE = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.RATE,
                    TOTAL = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.TOTAL,
                    HSCODE = comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.HSCODE,
                });
                //}
            }

            return PartialView($"AddComImpLcDetailsTable", await _cOmImpLcdetails.GetInitObjForDetailsTable(comImpLcInformationForCreateViewModel));
        }

        [HttpGet]
        [Route("CommercialImportLC/Create")]
        public async Task<IActionResult> CreateComImpLcInformation()
        { 
            return View(await _cOmImpLcinformation.GetComImpLcInformationForCreateViewModelValue(new ComImpLcInformationForCreateViewModel()));
        }

        [HttpPost]
        [Route("CommercialImportLC/Create")]
        public async Task<IActionResult> CreateComImpLcInformation(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isExistComImpLcInformation = await _cOmImpLcinformation.FindByLcNoAsync(comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.LCNO);

                    if (isExistComImpLcInformation)
                    {
                        TempData["message"] = "Failed to insert LC Information. Already exist with the same Lc Number.";
                        TempData["type"] = "error";
                        return View(await _cOmImpLcinformation.GetComImpLcInformationForCreateViewModelValue(comImpLcInformationForCreateViewModel));
                    }
                    else
                    {
                        var user = await _userManager.GetUserAsync(User);
                        comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.USRID = user.Id;
                        comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.LCPATH = _processUploadFile.ProcessUploadFileToContentRootPath(comImpLcInformationForCreateViewModel.LCPATH, "lc_files");
                        var comImpLcinformation = await _cOmImpLcinformation.GetInsertedObjByAsync(comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION);

                        if (comImpLcinformation != null)
                        {
                            foreach (var entity in comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs.Select(
                                item => new COM_IMP_LCDETAILS
                                {
                                    LCNO = comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.LCNO,
                                    LC_ID = comImpLcinformation.LC_ID,
                                    PIDATE = item.PIDATE,
                                    PINO = item.PINO,
                                    HSCODE = item.HSCODE,
                                    PIPATH = item.PIPATH,
                                    PRODID = item.PRODID,
                                    UNIT = item.UNIT,
                                    QTY = item.QTY,
                                    RATE = item.RATE,
                                    TOTAL = (double?)item.QTY * item.RATE,
                                    TRNSDATE = item.TRNSDATE,
                                    USRID = user.Id
                                }))
                            {
                                await _cOmImpLcdetails.InsertByAsync(entity);
                            }
                            TempData["message"] = "Successfully inserted LC Information.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetComImpLcInformation), $"ComImpLcInformation");
                        }
                        TempData["message"] = "Failed to insert LC Information.";
                        TempData["type"] = "error";
                        return View(await _cOmImpLcinformation.GetComImpLcInformationForCreateViewModelValue(comImpLcInformationForCreateViewModel));
                    }
                }

                TempData["message"] = "Failed to insert LC Information.";
                TempData["type"] = "error";
                return View(await _cOmImpLcinformation.GetComImpLcInformationForCreateViewModelValue(comImpLcInformationForCreateViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
    }
}