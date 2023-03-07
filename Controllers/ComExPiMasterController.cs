using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DenimERP.Controllers
{
    [Authorize]
    public class ComExPiMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ICOM_EX_PIMASTER _cOmExPimaster;
        private readonly ICOM_EX_PI_DETAILS _cOmExPiDetails;
        private readonly IRND_PRODUCTION_ORDER _rndProductionOrder;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<ComExPiMasterController> _localizer;
        private readonly string _title = "Commercial Export PI";

        public ComExPiMasterController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_EX_PIMASTER cOM_EX_PIMASTER,
            ICOM_EX_PI_DETAILS cOM_EX_PI_DETAILS,
            IRND_PRODUCTION_ORDER rndProductionOrder,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<ComExPiMasterController> localizer)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _cOmExPimaster = cOM_EX_PIMASTER;
            _cOmExPiDetails = cOM_EX_PI_DETAILS;
            _rndProductionOrder = rndProductionOrder;
            _userManager = userManager;
            _localizer = localizer;
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExport/AuditSummary/{searchValue?}")]
        public async Task<IActionResult> RAuditSummary(string searchValue)
        {
            var dictionary = new Dictionary<int, string>() { { 0, searchValue } };

            DateTime startDate = default;
            DateTime endDate = default;

            var dateRangeStrings = searchValue?.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (dateRangeStrings is { Length: > 1 })
            {
                DateTime.TryParse(dateRangeStrings[0], out startDate);
                DateTime.TryParse(dateRangeStrings[1], out endDate);

                dictionary.Add(1, startDate.ToString());
                dictionary.Add(2, endDate.ToString());
            }


            return View(model: dictionary);
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPiNoInUse(ComExPIMasterViewModel piInfo)
        {
            var isPiNoExists = await _cOmExPimaster.FindByPINoInUseAsync(piInfo.cOM_EX_PIMASTER.PINO);
            return !isPiNoExists ? Json(true) : Json($"PI No [ {piInfo.cOM_EX_PIMASTER.PINO} ] is already in use");
        }

        [HttpPost]
        [Route("CommercialExport/GetTableData")]
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

            var data = await _cOmExPimaster.GetForDataTableByAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.PINO != null && m.PINO.ToUpper().Contains(searchValue))
                                       //|| m.PIDATE.ToString(CultureInfo.CurrentCulture).ToUpper().Contains(searchValue)
                                       || m.LcNoPi != null && m.LcNoPi.ToUpper().Contains(searchValue)
                                       || m.FileNo != null && m.FileNo.ToUpper().Contains(searchValue)
                                       || m.PI_QTY != null && m.PI_QTY.ToString().ToUpper().Contains(searchValue)
                                       || m.PI_TOTAL_VALUE != null && m.PI_TOTAL_VALUE.ToString().ToUpper().Contains(searchValue)
                                       || m.BUYER.BUYER_NAME != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                       //|| m.BANK.BEN_BANK != null && m.BANK.BEN_BANK.ToUpper().Contains(searchValue)
                                       || m.COM_EX_PI_DETAILS.Any() && m.COM_EX_PI_DETAILS.Any(d => d.SO_NO.ToUpper().Contains(searchValue))
                                       || m.COM_EX_PI_DETAILS.Any() && m.COM_EX_PI_DETAILS.Any(d => d.STYLE.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue))
                                       || m.BANK.BEN_BANK != null && m.BANK.BEN_BANK.ToUpper().Contains(searchValue)
                                       || m.PersonMktTeam.PERSON_NAME != null && m.PersonMktTeam.PERSON_NAME.ToUpper().Contains(searchValue));
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
        [Route("CommercialExport/GetAll")]
        public IActionResult GetComExpiInfoWithPaged()
        {
            try
            {
                ViewData["Actions"] = _localizer["Actions"];
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<COM_EX_PI_DETAILS> GetPiDetails(int piId, int styleId)
        {
            try
            {
                var result = await _cOmExPiDetails.FindPiListByPiIdAndStyleIdAsync(piId, styleId);
                var r = result?.FirstOrDefault();
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<COM_EX_PI_DETAILS> GetSinglePiDetails(int trnsId)
        {
            try
            {
                var result = await _cOmExPiDetails.FindByIdAsync(trnsId);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPiDetailsExist(int piId, int styleId)
        {
            try
            {
                var result = await _cOmExPiDetails.FindPiListByPiIdAndStyleIdAsync(piId, styleId);
                return Ok(result.Any());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<List<COM_EX_FABSTYLE>> GetFabStyleByPiIdList(int piId)
        {
            try
            {
                var piInfo = await _cOmExPimaster.FindByIdAsync(piId);
                if (piInfo != null)
                {
                    var result = await _cOmExPimaster.FindFabStyleByPiIdAsync(piId);
                    return result?.ToList();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("CommercialExport/Create")]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> CreateComEXPIMaster()
        {
            var comExPiMasterViewModel = new ComExPIMasterViewModel
            {
                cOM_EX_PIMASTER = new COM_EX_PIMASTER
                {
                    PINO = await _cOmExPimaster.GetLastPINoAsync(),
                    PIDATE = DateTime.Today,
                    STATUS = true,
                    DURATION = 10,
                    VALIDITY = DateTime.Today.AddDays(10),
                    CURRENCY = 2,
                    DEL_PERIOD = "30 Days from the date of the acceptable L/C",
                    NEGOTIATION = "Within 21 days from delivery challan date",
                    INCOTERMS = "FCA (Incoterms 2020)",
                    INSPECTION = "Quality & quantity may be inspected",
                    EXP_STATUS = 1,
                    TOLERANCE = "5",
                    PAYMENT = "Reimbursement of full invoice value to be paid by the L/C opening Bank to the negotiating Bank as per instruction of the negotiation Bank."
                },
                cOM_EX_PI_DETAILS = new COM_EX_PI_DETAILS { QTY = 0, UNITPRICE = 0, TOTAL = 0 },

            };

            return View(await _cOmExPimaster.GetInitObjects(comExPiMasterViewModel));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExport/ShowOptions/{piId?}")]
        public async Task<IActionResult> InvokeUserChoices(string piId)
        {
            try
            {
                var comExPimaster = await _cOmExPimaster.FindByIdAsync(int.Parse(_protector.Unprotect(piId)));
                comExPimaster.EncryptedId = _protector.Protect(comExPimaster.PIID.ToString());
                return View(comExPimaster);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
            }
        }

        [HttpPost]
        [Route("CommercialExport/Create")]
        public async Task<IActionResult> CreateComEXPIMaster(ComExPIMasterViewModel comExPIMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    comExPIMasterViewModel.cOM_EX_PIMASTER.CREATED_AT = DateTime.Now;
                    comExPIMasterViewModel.cOM_EX_PIMASTER.CREATED_BY = user.Id;
                    comExPIMasterViewModel.cOM_EX_PIMASTER.PI_QTY = comExPIMasterViewModel.cOM_EX_PI_DETAILs.Sum(e => e.QTY) ?? 0;
                    comExPIMasterViewModel.cOM_EX_PIMASTER.PI_TOTAL_VALUE = comExPIMasterViewModel.cOM_EX_PI_DETAILs.Sum(e => e.TOTAL) ?? 0;

                    var isPiNoExists = await _cOmExPimaster.FindByPINoInUseAsync(comExPIMasterViewModel.cOM_EX_PIMASTER.PINO);

                    if (isPiNoExists)
                    {
                        var piNo = await _cOmExPimaster.GetLastPINoAsync();
                        comExPIMasterViewModel.cOM_EX_PIMASTER.PINO = piNo;
                    }

                    var comExPimaster = await _cOmExPimaster.GetInsertedObjByAsync(comExPIMasterViewModel.cOM_EX_PIMASTER);

                    if (comExPimaster.PIID != 0)
                    {
                        foreach (var item in comExPIMasterViewModel.cOM_EX_PI_DETAILs)
                        {
                            item.PIID = comExPimaster.PIID;
                            item.INITIAL_QTY = item.QTY;
                            item.SO_NO = await _cOmExPiDetails.GetLastSoNoAsync();
                            item.SO_STATUS = true;

                            await _cOmExPiDetails.InsertByAsync(item);
                        }

                        comExPimaster.EncryptedId = _protector.Protect(comExPimaster.PIID.ToString());

                        TempData["message"] = $"Successfully Added PI Information. PI NO: {comExPimaster.PINO}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(InvokeUserChoices), new { @piId = comExPimaster.EncryptedId });
                    }
                    else
                    {
                        TempData["message"] = "Failed To Add  PI Information";
                        TempData["type"] = "error";
                        return View(await _cOmExPimaster.GetInitObjects(comExPIMasterViewModel));
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Submission. Please Try Again.";
                    TempData["type"] = "error";
                    return View(await _cOmExPimaster.GetInitObjects(comExPIMasterViewModel));
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = $"{ex.Message}";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> DeletePIInfo(string piId)
        {

            try
            {
                var piInfo = await _cOmExPimaster.FindByPiIdAsync(int.Parse(_protector.Unprotect(piId)));

                if (piInfo != null)
                {
                    var comExPiDetailsList = (List<COM_EX_PI_DETAILS>)await _cOmExPiDetails.FindPIListByPINoAsync(piInfo.PIID);

                    if (comExPiDetailsList.Any())
                    {
                        await _cOmExPiDetails.DeleteRange(comExPiDetailsList);
                    }

                    await _cOmExPimaster.Delete(piInfo);

                    TempData["message"] = "Successfully Deleted PI Information.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "PI Information Not Found!";
                    TempData["type"] = "error";
                }

                return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("CommercialExport/Details/{piId?}")]
        public async Task<IActionResult> DetailsPIInfo(string piId)
        {
            var findByPiIdIncludeAllAsync = await _cOmExPimaster.FindByPiIdIncludeAllAsync(int.Parse(_protector.Unprotect(piId)));

            if (findByPiIdIncludeAllAsync.cOM_EX_PIMASTER != null)
            {
                return View(findByPiIdIncludeAllAsync);
            }

            TempData["message"] = $"{_title} Could Not Be Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
        }

        [HttpGet]
        [Route("CommercialExport/Edit/{piId?}")]
        [Authorize(Roles = "Com Exp, Super Admin")]
        public async Task<IActionResult> EditPIInfo(string piId)
        {
            try
            {
                var findByPiIdIncludeAllAsync = await _cOmExPimaster.FindByPiIdIncludeAllAsync(int.Parse(_protector.Unprotect(piId)));

                if (findByPiIdIncludeAllAsync.cOM_EX_PIMASTER != null)
                {
                    return View(await _cOmExPimaster.GetInitObjects(findByPiIdIncludeAllAsync));
                }

                TempData["message"] = "PI Details not found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve PI Information.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
            }
        }

        [HttpPost]
        [Route("CommercialExport/PostEdit")]
        public async Task<IActionResult> PostEditPIInfo(ComExPIMasterViewModel comExPiMasterViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var comExPimaster = await _cOmExPimaster.FindByIdAsync(int.Parse(_protector.Unprotect(comExPiMasterViewModel.cOM_EX_PIMASTER.EncryptedId)));

                comExPiMasterViewModel.cOM_EX_PIMASTER.CREATED_AT = comExPimaster.CREATED_AT;
                comExPiMasterViewModel.cOM_EX_PIMASTER.CREATED_BY = comExPimaster.CREATED_BY;
                comExPiMasterViewModel.cOM_EX_PIMASTER.UPDATED_AT = DateTime.Now;
                comExPiMasterViewModel.cOM_EX_PIMASTER.UPDATED_BY = user.Id;
                comExPiMasterViewModel.cOM_EX_PIMASTER.PI_QTY = comExPiMasterViewModel.cOM_EX_PI_DETAILs.Sum(e => e.QTY) ?? 0;
                comExPiMasterViewModel.cOM_EX_PIMASTER.PI_TOTAL_VALUE = comExPiMasterViewModel.cOM_EX_PI_DETAILs.Sum(e => e.TOTAL) ?? 0;

                if (await _cOmExPimaster.Update(comExPiMasterViewModel.cOM_EX_PIMASTER))
                {
                    foreach (var item in comExPiMasterViewModel.cOM_EX_PI_DETAILs)
                    {
                        if (item.TRNSID <= 0)
                        {
                            var soNo = await _cOmExPiDetails.GetLastSoNoAsync();

                            item.PIID = comExPimaster.PIID;
                            item.INITIAL_QTY = item.QTY;
                            item.SO_NO = soNo;
                            item.SO_STATUS = true;

                            await _cOmExPiDetails.InsertByAsync(item);
                        }
                        else
                        {
                            var comExPiDetails = await _cOmExPiDetails.FindByIdAsync(item.TRNSID);
                            var rndProductionOrder = await _rndProductionOrder.FindByIdAsync(await _cOmExPiDetails.GetPoIdBySO(item.TRNSID));

                            if (comExPiMasterViewModel.cOM_EX_PIMASTER.PINO.Contains("REV-"))
                            {
                                var revNo = comExPiMasterViewModel.cOM_EX_PIMASTER.PINO.Split("V-")[1];
                                revNo = revNo.Split(")")[0];

                                comExPiDetails.SO_NO = comExPiDetails?.SO_NO != null && !comExPiDetails.SO_NO.Contains("R" + revNo) ?
                                    comExPiDetails.SO_NO.Split('R').Length > 1 ?
                                        $"{comExPiDetails.SO_NO.TrimEnd(comExPiDetails.SO_NO.Split('R')[1].ToCharArray())}{revNo}" : $"{comExPiDetails?.SO_NO}R{revNo}"
                                    : comExPiDetails?.SO_NO;

                                if (comExPiDetails.REV_TRACK != null)
                                {
                                    if (int.Parse(revNo) > 1 && (int.Parse(revNo) > comExPiDetails.REV_TRACK))
                                    {
                                        comExPiDetails.INTIAL_QTY_2S = comExPiDetails.QTY;
                                        comExPiDetails.REV_TRACK = int.Parse(revNo);
                                    }
                                }
                                else
                                {
                                    comExPiDetails.INTIAL_QTY_2S = item.QTY;
                                    comExPiDetails.REV_TRACK ??= int.Parse(revNo);
                                }

                                if (rndProductionOrder != null)
                                {
                                    rndProductionOrder.ORDER_QTY_YDS = item.UNIT == 7 ? item.QTY : item.QTY * 1.09361;
                                    rndProductionOrder.ORDER_QTY_MTR = item.UNIT == 6 ? item.QTY : item.QTY * 0.9144;
                                }
                            }

                            comExPiDetails.STYLEID = item.STYLEID;
                            comExPiDetails.UNIT = item.UNIT;
                            comExPiDetails.COSTID = item.COSTID;
                            comExPiDetails.INITIAL_QTY = comExPiDetails.SO_NO != null && comExPiDetails.SO_NO.Contains("R") ? comExPiDetails.INITIAL_QTY : item.QTY;
                            comExPiDetails.QTY = item.QTY;
                            comExPiDetails.UNITPRICE = item.UNITPRICE;
                            comExPiDetails.TOTAL = item.TOTAL;
                            comExPiDetails.REMARKS = item.REMARKS;


                            await _cOmExPiDetails.Update(comExPiDetails);
                            if (rndProductionOrder != null)
                                await _rndProductionOrder.Update(rndProductionOrder);
                        }
                    }

                    TempData["message"] = "Successfully Updated PI Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
                }

                TempData["message"] = "Failed to Update PI Information.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComExpiInfoWithPaged), $"ComExPiMaster");
            }
            catch (Exception ex)
            {
                TempData["message"] = "Failed to Update PI Information.";
                TempData["type"] = "error";
                return View(nameof(EditPIInfo), await _cOmExPimaster.GetInitObjects(comExPiMasterViewModel));
            }
        }

        [AcceptVerbs("Post")]
        [Route("CommercialExport/GetNetGrossWeight")]
        public async Task<IActionResult> GetNetGrossWeight(ComExPIMasterViewModel comExPiMasterViewModel)
        {
            return Ok(await _cOmExPimaster.GetInitObjForDetailsTable(comExPiMasterViewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CommercialExport/AddOrRemovePIDetails")]
        public async Task<IActionResult> AddPIDetails(ComExPIMasterViewModel comExPiMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (comExPiMasterViewModel.IsDelete)
                {
                    var cOmExPiDetaiL = comExPiMasterViewModel.cOM_EX_PI_DETAILs[comExPiMasterViewModel.RemoveIndex];

                    if (cOmExPiDetaiL.TRNSID > 0)
                    {
                        await _cOmExPiDetails.Delete(cOmExPiDetaiL);
                    }

                    comExPiMasterViewModel.cOM_EX_PI_DETAILs.RemoveAt(comExPiMasterViewModel.RemoveIndex);
                }
                else
                {
                    if (comExPiMasterViewModel.cOM_EX_PI_DETAILs.Any(e => e.TRNSID > 0 && e.TRNSID.Equals(comExPiMasterViewModel.cOM_EX_PI_DETAILS.TRNSID)))
                    {
                        var comExPiDetailses = comExPiMasterViewModel.cOM_EX_PI_DETAILs.Where(e => e.TRNSID.Equals(comExPiMasterViewModel.cOM_EX_PI_DETAILS.TRNSID)).ToList();

                        for (var i = 0; i < comExPiDetailses.Count; i++)
                        {
                            if (TryValidateModel(comExPiMasterViewModel.cOM_EX_PI_DETAILS))
                            {
                                comExPiDetailses[i].STYLEID = comExPiMasterViewModel.cOM_EX_PI_DETAILS.STYLEID;
                                comExPiDetailses[i].SO_NO = comExPiMasterViewModel.cOM_EX_PI_DETAILS.SO_NO;
                                comExPiDetailses[i].COSTID = comExPiMasterViewModel.cOM_EX_PI_DETAILS.COSTID;
                                comExPiDetailses[i].COSTREF = comExPiMasterViewModel.cOM_EX_PI_DETAILS.COSTREF;
                                comExPiDetailses[i].UNIT = comExPiMasterViewModel.cOM_EX_PI_DETAILS.UNIT;
                                comExPiDetailses[i].QTY = comExPiMasterViewModel.cOM_EX_PI_DETAILS.QTY;
                                comExPiDetailses[i].UNITPRICE = comExPiMasterViewModel.cOM_EX_PI_DETAILS.UNITPRICE;
                                comExPiDetailses[i].TOTAL = comExPiMasterViewModel.cOM_EX_PI_DETAILS.TOTAL;
                                comExPiDetailses[i].REMARKS = comExPiMasterViewModel.cOM_EX_PI_DETAILS.REMARKS;
                            }
                            else
                            {
                                ModelState.AddModelError("", $"There is an error with your data. Please check the row number {i + 1}");
                            }
                        }
                    }
                    else
                    {
                        if (TryValidateModel(comExPiMasterViewModel.cOM_EX_PI_DETAILS))
                        {
                            comExPiMasterViewModel.cOM_EX_PI_DETAILS.INITIAL_QTY = comExPiMasterViewModel.cOM_EX_PI_DETAILS.QTY;
                            comExPiMasterViewModel.cOM_EX_PI_DETAILs.Add(comExPiMasterViewModel.cOM_EX_PI_DETAILS);
                        }
                    }
                }

                return PartialView($"ComExPIDetailsTable", await _cOmExPimaster.GetInitObjForDetailsTable(comExPiMasterViewModel));

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveExistingPiDetails(int trnsId, int piId)
        {
            try
            {
                var result = await _cOmExPiDetails.FindSoInPOTableAsync(trnsId);
                if (result != null)
                {
                    result.SO_STATUS = false;
                    result.RND_PRODUCTION_ORDER = null;
                    await _cOmExPiDetails.Update(result);
                    var comExPiDetailsLista = await _cOmExPiDetails.FindPIListByPINoAsync(piId);
                    return PartialView($"RemovePIDetailsList", comExPiDetailsLista.ToList());
                }

                var ifExistPIinfo = await _cOmExPiDetails.FindPIListByPINoAndTransIDAsync(piId, trnsId);
                if (ifExistPIinfo != null)
                {
                    await _cOmExPiDetails.DeleteRange(ifExistPIinfo);
                }
                var comExPiDetailsList = await _cOmExPiDetails.FindPIListByPINoAsync(piId);
                return PartialView($"RemovePIDetailsList", comExPiDetailsList.ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPIDetailsList(int piId)
        {
            try
            {
                var comExPiDetailsList = await _cOmExPiDetails.FindPIListByPINoAsync(piId);

                return PartialView($"RemovePIDetailsList", comExPiDetailsList.ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IEnumerable<COM_EX_PI_DETAILS>> GetPIDetailsListData(int piId)
        {
            try
            {
                var comExPiDetailsList = await _cOmExPiDetails.FindPIListByPINoAsync(piId);
                return comExPiDetailsList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult AddDays(ComExPIMasterViewModel comExPiMasterViewModel)
        {
            return Json(comExPiMasterViewModel.cOM_EX_PIMASTER.PIDATE.AddDays(int.Parse(comExPiMasterViewModel.cOM_EX_PIMASTER.DURATION.ToString())).ToString("yyyy-MM-dd"));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetCostRefNo(int styleId)
        {
            try
            {
                return Ok(await _cOmExPimaster.GetCostRefNo(styleId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="piId"> Belongs to PIID. Primary key. Must not be null. <see cref="COM_EX_PIMASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CommercialExport/ProductionOrder/{piId?}")]
        public async Task<IActionResult> RProductionOrder(string piId)
        {
            try
            {
                var piNo = string.Empty;

                if (piId != null)
                    piNo = await Task.Run(() => _cOmExPimaster.FindByIdAsync(int.Parse(_protector.Unprotect(piId))).Result.PINO);
                return View(model: piNo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="piId"> Belongs to PIID. Primary key. Must not be null. <see cref="COM_EX_PIMASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CommercialExport/ProformaInvoice/{piId?}")]
        public async Task<IActionResult> RProformaInvoice(string piId)
        {
            try
            {
                var piNo = string.Empty;

                if (piId != null)
                    piNo = await Task.Run(() => _cOmExPimaster.FindByIdAsync(int.Parse(_protector.Unprotect(piId))).Result.PINO);
                return View(model: piNo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("CommercialExport/FrnProformaInvoice/{piId?}")]
        public async Task<IActionResult> FrnProformaInvoice(string piId)
        {
            try
            {
                var piNo = string.Empty;

                if (piId != null)
                    piNo = await Task.Run(() => _cOmExPimaster.FindByIdAsync(int.Parse(_protector.Unprotect(piId))).Result.PINO);
                return View(model: piNo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}