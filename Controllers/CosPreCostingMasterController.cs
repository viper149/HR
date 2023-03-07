using System;
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

namespace DenimERP.Controllers
{
    [Authorize]
    public class CosPreCostingMasterController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICOS_PRECOSTING_MASTER _cosPreCostingMaster;
        private readonly IRND_FINISHTYPE _rndFinishType;
        private readonly IBAS_YARN_COUNTINFO _yarnCountInfo;
        private readonly ICOS_PRECOSTING_DETAILS _cosPreCostingDetails;
        private readonly ICOS_CERTIFICATION_COST _cosCertificationCost;
        private readonly IAuthorizationService _authorizationService;
        private readonly IDataProtector _protector;

        public CosPreCostingMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            SignInManager<ApplicationUser> signInManager,
            ICOS_PRECOSTING_MASTER cosPreCostingMaster,
            IRND_FINISHTYPE rndFinishType,
            IBAS_YARN_COUNTINFO yarnCountInfo,
            ICOS_PRECOSTING_DETAILS cosPreCostingDetails,
            ICOS_CERTIFICATION_COST cosCertificationCost,
            IAuthorizationService authorizationService
        )
        {
            _signInManager = signInManager;
            _cosPreCostingMaster = cosPreCostingMaster;
            _rndFinishType = rndFinishType;
            _yarnCountInfo = yarnCountInfo;
            _cosPreCostingDetails = cosPreCostingDetails;
            _cosCertificationCost = cosCertificationCost;
            _authorizationService = authorizationService;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;

                var data = await _cosPreCostingMaster.GetAllAsync();

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, "CostingPDLCost");

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.CSID.ToString());
                    item.USERNAME = item.USERID != null ? _signInManager.UserManager.FindByIdAsync(item.USERID).Result.UserName : "N/A";
                    item.OPTION1 = item.COM_EX_PI_DETAILS.Any(c => c.COSTID.Equals(item.CSID)) ? "1" : "0";
                    item.CSDATESTRING = item.CSDATE.ToString("dd MMM yyyy");
                    item.HasAccess = authorizationResult.Succeeded;
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CSID.ToString().Contains(searchValue)
                                           || m.CSDATESTRING.ToUpper().Contains(searchValue)
                                           || m.ADJ_MONTHLYCOST.ToString().Contains(searchValue)
                                           || m.ADJ_PRODUCTION.ToString().Contains(searchValue)
                                           || m.ADJ_OVERHEAD_USD.ToString().Contains(searchValue)
                                           || m.USERNAME.ToUpper().Contains(searchValue)
                                           || m.FABCODENavigation.STYLE_NAME != null && m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize);

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
        [Route("CostingPDLCost")]
        [Route("CostingPDLCost/GetAll")]
        public IActionResult GetPreCostingMaster()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Authorize(Policy = "CostingPDLCost")]
        public async Task<IActionResult> CreatePreCostingMaster()
        {
            try
            {
                var cosPreCostingMasterViewModel = await _cosPreCostingMaster.GetInitObjects(new CosPreCostingMasterViewModel());

                cosPreCostingMasterViewModel.CosPreCostingMaster = new COS_PRECOSTING_MASTER
                {
                    CSDATE = DateTime.Now,
                    SCID = cosPreCostingMasterViewModel.StandardCons.SCID,
                    MONTHLY_COST = cosPreCostingMasterViewModel.StandardCons.MONTHLY_COST,
                    PROD = cosPreCostingMasterViewModel.StandardCons.PROD,
                    OVERHEAD_BDT = cosPreCostingMasterViewModel.StandardCons.OVERHEAD_BDT,
                    CONV_RATE = cosPreCostingMasterViewModel.StandardCons.CONV_RATE,
                    OVERHEAD_USD = cosPreCostingMasterViewModel.StandardCons.OVERHEAD_USD,
                    SLOOMSPEED = cosPreCostingMasterViewModel.StandardCons.LOOMSPEED,
                    SEFFICIENCY = cosPreCostingMasterViewModel.StandardCons.EFFICIENCY,
                    SGRPPI = cosPreCostingMasterViewModel.StandardCons.GRPPI,
                    CONTRACTION = cosPreCostingMasterViewModel.StandardCons.CONTRACTION,
                    FCID = cosPreCostingMasterViewModel.FixedCost.FCID,
                    DYEINGCOST = cosPreCostingMasterViewModel.FixedCost.DYEINGCOST,
                    UPCHARGE = cosPreCostingMasterViewModel.FixedCost.UPCHARGE,
                    PRINTINGCOST = cosPreCostingMasterViewModel.FixedCost.PRINTINGCOST,
                    OVERHEADCOST = cosPreCostingMasterViewModel.FixedCost.OVERHEADCOST,
                    SAMPLECOST = cosPreCostingMasterViewModel.FixedCost.SAMPLECOST,
                    SIZINGCOST = cosPreCostingMasterViewModel.FixedCost.SIZINGCOST,
                    ORDER_QTY = 10000,
                    CID = 1,
                    TID = 47,
                    CERTIFICATE_COST = 0.00
                };

                cosPreCostingMasterViewModel.CosPreCostingDetails = new COS_PRECOSTING_DETAILS
                {
                    TRNSDATE = DateTime.Now,
                    COS_CONSUMP = 0,
                    RATE = 0,
                    VALUE = 0,
                    REMARKS = "N/A"
                };

                return View(cosPreCostingMasterViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = "CostingPDLCost")]
        public async Task<IActionResult> CreatePreCostingMaster(CosPreCostingMasterViewModel cosPreCostingMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _signInManager.UserManager.GetUserAsync(User);
                    cosPreCostingMasterViewModel.CosPreCostingMaster.USERID = user.Id;
                    cosPreCostingMasterViewModel.CosPreCostingMaster.CREATED_AT = DateTime.Now;
                    cosPreCostingMasterViewModel.CosPreCostingMaster.UPDATED_AT = DateTime.Now;
                    var result = await _cosPreCostingMaster.GetInsertedObjByAsync(cosPreCostingMasterViewModel.CosPreCostingMaster);
                    if (result!=null)
                    {
                        foreach (var item in cosPreCostingMasterViewModel.CosPreCostingDetailsList)
                        {
                            item.USERID = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CSID = result.CSID;
                            item.BasYarnCountInfo = null;

                            item.VALUE = Math.Round(item.RATE * item.COS_CONSUMP??0,4);

                            await _cosPreCostingDetails.InsertByAsync(item);
                        }
                        TempData["message"] = "Successfully Added Pre-Costing.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
                    }
                    TempData["message"] = "Failed to Add Pre-Costing.";
                        TempData["type"] = "error";
                        return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
                }

                //var err = ModelState.Values.SelectMany(v => v.Errors).ToList();
                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
            }
        }

        [HttpGet]
        [Authorize(Policy = "CostingPDLCost")]
        public async Task<IActionResult> EditPreCostingMaster(string id)
        {
            try
            {
                var cosPreCostingMasterViewModel = await _cosPreCostingMaster.GetInitObjects(new CosPreCostingMasterViewModel());

                cosPreCostingMasterViewModel.CosPreCostingMaster = await _cosPreCostingMaster.FindByIdAllAsync(int.Parse(_protector.Unprotect(id)));
                var detailsList = await _cosPreCostingDetails.GetAllDetailsAsync(cosPreCostingMasterViewModel.CosPreCostingMaster.CSID);
                cosPreCostingMasterViewModel.CosPreCostingDetailsList = detailsList.ToList();
                cosPreCostingMasterViewModel.CosPreCostingMaster.EncryptedId = _protector.Protect(cosPreCostingMasterViewModel.CosPreCostingMaster.CSID.ToString());


                cosPreCostingMasterViewModel.CosPreCostingMaster.SCID =
                    cosPreCostingMasterViewModel.StandardCons.SCID;
                cosPreCostingMasterViewModel.CosPreCostingMaster.FCID =
                    cosPreCostingMasterViewModel.FixedCost.FCID;
                cosPreCostingMasterViewModel.CosPreCostingMaster.MONTHLY_COST =
                    cosPreCostingMasterViewModel.StandardCons.MONTHLY_COST;
                cosPreCostingMasterViewModel.CosPreCostingMaster.PROD = cosPreCostingMasterViewModel.StandardCons.PROD;
                cosPreCostingMasterViewModel.CosPreCostingMaster.OVERHEAD_BDT =
                    cosPreCostingMasterViewModel.StandardCons.OVERHEAD_BDT;
                cosPreCostingMasterViewModel.CosPreCostingMaster.CONV_RATE =
                    cosPreCostingMasterViewModel.StandardCons.CONV_RATE;
                cosPreCostingMasterViewModel.CosPreCostingMaster.OVERHEAD_USD =
                    cosPreCostingMasterViewModel.StandardCons.OVERHEAD_USD;
                cosPreCostingMasterViewModel.CosPreCostingMaster.SLOOMSPEED = cosPreCostingMasterViewModel.StandardCons.LOOMSPEED;
                cosPreCostingMasterViewModel.CosPreCostingMaster.SEFFICIENCY =
                    cosPreCostingMasterViewModel.StandardCons.EFFICIENCY;
                cosPreCostingMasterViewModel.CosPreCostingMaster.SGRPPI =
                    cosPreCostingMasterViewModel.StandardCons.GRPPI;
                cosPreCostingMasterViewModel.CosPreCostingMaster.CONTRACTION =
                    cosPreCostingMasterViewModel.StandardCons.CONTRACTION;
                cosPreCostingMasterViewModel.CosPreCostingMaster.FCID = cosPreCostingMasterViewModel.FixedCost.FCID;
                cosPreCostingMasterViewModel.CosPreCostingMaster.DYEINGCOST =
                    cosPreCostingMasterViewModel.FixedCost.DYEINGCOST;
                cosPreCostingMasterViewModel.CosPreCostingMaster.UPCHARGE =
                    cosPreCostingMasterViewModel.FixedCost.UPCHARGE;
                cosPreCostingMasterViewModel.CosPreCostingMaster.PRINTINGCOST =
                    cosPreCostingMasterViewModel.FixedCost.PRINTINGCOST;
                cosPreCostingMasterViewModel.CosPreCostingMaster.OVERHEADCOST =
                    cosPreCostingMasterViewModel.FixedCost.OVERHEADCOST;
                cosPreCostingMasterViewModel.CosPreCostingMaster.SAMPLECOST =
                    cosPreCostingMasterViewModel.FixedCost.SAMPLECOST;
                cosPreCostingMasterViewModel.CosPreCostingMaster.SIZINGCOST =
                    cosPreCostingMasterViewModel.FixedCost.SIZINGCOST;

                cosPreCostingMasterViewModel.CosPreCostingDetails = new COS_PRECOSTING_DETAILS
                {
                    TRNSDATE = DateTime.Now,
                    COS_CONSUMP = 0,
                    RATE = 0,
                    VALUE = 0
                };

                return View(cosPreCostingMasterViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = "CostingPDLCost")]
        public async Task<IActionResult> EditPreCostingMaster(CosPreCostingMasterViewModel cosPreCostingMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var costId = int.Parse(_protector.Unprotect(cosPreCostingMasterViewModel.CosPreCostingMaster.EncryptedId));
                    if (cosPreCostingMasterViewModel.CosPreCostingMaster.CSID == costId)
                    {
                        var costDetails = await _cosPreCostingMaster.FindByIdAsync(costId);

                        cosPreCostingMasterViewModel.CosPreCostingMaster.CREATED_AT = costDetails.CREATED_AT;
                        cosPreCostingMasterViewModel.CosPreCostingMaster.UPDATED_AT = DateTime.Now;
                        var user = await _signInManager.UserManager.GetUserAsync(User);
                        cosPreCostingMasterViewModel.CosPreCostingMaster.USERID = user.Id;

                        var isUpdated = await _cosPreCostingMaster.Update(cosPreCostingMasterViewModel.CosPreCostingMaster);
                        if (isUpdated)
                        {
                            foreach (var item in cosPreCostingMasterViewModel.CosPreCostingDetailsList.Where(e => e.TRNSID == 0))
                            {
                                item.USERID = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.CREATED_AT = DateTime.Now;
                                item.CSID = costDetails.CSID;
                                item.BasYarnCountInfo = null;
                                item.VALUE = Math.Round(item.RATE * item.COS_CONSUMP ?? 0, 4);
                                await _cosPreCostingDetails.InsertByAsync(item);
                            }

                            foreach (var item in cosPreCostingMasterViewModel.CosPreCostingDetailsList.Where(e => e.TRNSID > 0))
                            {
                                var preCostingDetails = await _cosPreCostingDetails.FindByIdAsync(item.TRNSID);

                                item.USERID = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.CREATED_AT = preCostingDetails.CREATED_AT;
                                item.CSID = costDetails.CSID;
                                item.BasYarnCountInfo = null;
                                item.VALUE = Math.Round(item.RATE * item.COS_CONSUMP ?? 0, 4);
                                await _cosPreCostingDetails.Update(item);
                            }
                            TempData["message"] = "Successfully Updated Costing Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetPreCostingMaster", $"CosPreCostingMaster");
                        }
                        TempData["message"] = "Failed to Update Costing Details.";
                        TempData["type"] = "error";


                        return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
                    }
                    TempData["message"] = "Invalid Costing Info.";
                    TempData["type"] = "error";

                    return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";

                return View(await _cosPreCostingMaster.GetInitObjects(cosPreCostingMasterViewModel));
            }
        }
        
        [HttpGet]
        [Authorize(Policy = "CostingPDLCost")]
        public async Task<IActionResult> DetailsPreCostingMaster(string csId)
        {
            try
            {
                var costingInfo = await _cosPreCostingMaster.FindByIdAllAsync(int.Parse(_protector.Unprotect(csId)));

                if (costingInfo != null)
                {
                    var costingMasterViewModel = new CosPreCostingMasterViewModel();

                    costingInfo.EncryptedId = _protector.Protect(costingInfo.CSID.ToString());
                    costingInfo.USERNAME = costingInfo.USERID != null ? _signInManager.UserManager.FindByIdAsync(costingInfo.USERID).Result.UserName : "N/A";

                    var costDetails = await _cosPreCostingDetails.GetAllDetailsAsync(costingInfo.CSID);

                    costingMasterViewModel.CosPreCostingMaster = costingInfo;
                    costingMasterViewModel.CosPreCostingDetailsList = costDetails.ToList();
                    return View(costingMasterViewModel);
                }
                else
                {
                    TempData["message"] = "Pre-Costing Information Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Retrieve Pre-Costing Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> RePreCostingMaster(string csId)
        {
            try
            {
                var cosPrecostingMaster = await _cosPreCostingMaster.FindByIdAllAsync(int.Parse(_protector.Unprotect(csId)));
                var cosPreCostingMasterViewModel = await _cosPreCostingMaster.GetInitObjects(new CosPreCostingMasterViewModel());
                cosPreCostingMasterViewModel.CosPreCostingMaster = cosPrecostingMaster;

                var detailsList = await _cosPreCostingDetails.GetAllDetailsAsync(cosPreCostingMasterViewModel.CosPreCostingMaster.CSID);

                var cosPreCostingDetails = detailsList.ToList();
                foreach (var item in cosPreCostingDetails)
                {
                    item.CSID = 0;
                    item.TRNSID = 0;
                }

                cosPreCostingMasterViewModel.CosPreCostingDetailsList = cosPreCostingDetails.ToList();

                cosPreCostingMasterViewModel.CosPreCostingDetails = new COS_PRECOSTING_DETAILS
                {
                    TRNSDATE = DateTime.Now,
                    COS_CONSUMP = 0,
                    RATE = 0,
                    VALUE = 0
                };
                return View($"CreatePreCostingMaster", cosPreCostingMasterViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Find Pre-Costing information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> GetPreCostingDetails(CosPreCostingMasterViewModel cosPreCostingMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                cosPreCostingMasterViewModel = await _cosPreCostingDetails.GetCountList(cosPreCostingMasterViewModel);

                cosPreCostingMasterViewModel.CosPreCostingDetailsList = await _yarnCountInfo.GetDetailsByIdAsync(cosPreCostingMasterViewModel.CosPreCostingDetailsList);

                //cosPreCostingMasterViewModel.CosPreCostingDetailsList = await _yarnCountInfo.GetCountDetailsByIdAsync(cosPreCostingMasterViewModel.CosPreCostingDetailsList);
                return PartialView($"AccPreCostDetailsTable", cosPreCostingMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                cosPreCostingMasterViewModel.CosPreCostingDetailsList = await _yarnCountInfo.GetDetailsByIdAsync(cosPreCostingMasterViewModel.CosPreCostingDetailsList);
                return PartialView($"AccPreCostDetailsTable", cosPreCostingMasterViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPreCostingDetails(CosPreCostingMasterViewModel cosPreCostingMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (ModelState.IsValid)
                {
                    var result = cosPreCostingMasterViewModel.CosPreCostingDetailsList.Where(c => c.COUNTID == cosPreCostingMasterViewModel.CosPreCostingDetails.COUNTID && c.FABCODE == cosPreCostingMasterViewModel.CosPreCostingMaster.FABCODE && c.YARN_FOR == cosPreCostingMasterViewModel.CosPreCostingDetails.YARN_FOR);
                    if (!result.Any())
                    {
                        cosPreCostingMasterViewModel.CosPreCostingDetails.FABCODE = cosPreCostingMasterViewModel.CosPreCostingMaster.FABCODE;
                        cosPreCostingMasterViewModel.CosPreCostingDetailsList.Add(cosPreCostingMasterViewModel.CosPreCostingDetails);
                    }

                    cosPreCostingMasterViewModel.CosPreCostingDetailsList =
                        await _yarnCountInfo.GetCountDetailsByIdAsync(cosPreCostingMasterViewModel.CosPreCostingDetailsList);
                    return PartialView($"AccPreCostDetailsTable", cosPreCostingMasterViewModel);
                }
                var err = ModelState.Values.SelectMany(v => v.Errors).ToList();
                cosPreCostingMasterViewModel.CosPreCostingDetailsList =
                    await _yarnCountInfo.GetCountDetailsByIdAsync(cosPreCostingMasterViewModel.CosPreCostingDetailsList);
                ModelState.Clear();
                return PartialView($"AccPreCostDetailsTable", cosPreCostingMasterViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                cosPreCostingMasterViewModel.CosPreCostingDetailsList =
                    await _yarnCountInfo.GetCountDetailsByIdAsync(cosPreCostingMasterViewModel.CosPreCostingDetailsList);
                return PartialView($"AccPreCostDetailsTable", cosPreCostingMasterViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePreCostDetailsFromList(CosPreCostingMasterViewModel cosPreCostingMasterViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (cosPreCostingMasterViewModel.CosPreCostingDetailsList[int.Parse(removeIndexValue)].TRNSID != 0)
            {
                await _cosPreCostingDetails.Delete(cosPreCostingMasterViewModel.CosPreCostingDetailsList[int.Parse(removeIndexValue)]);
            }
            cosPreCostingMasterViewModel.CosPreCostingDetailsList.RemoveAt(int.Parse(removeIndexValue));
            return PartialView($"AccPreCostDetailsTable", cosPreCostingMasterViewModel);
        }

        public async Task<double> GetFinishCost(int finId)
        {
            try
            {
                var data = await _rndFinishType.FindByIdAsync(finId);
                return data.COST;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double> GetCertificationCost(int cId)
        {
            try
            {
                var data = await _cosCertificationCost.FindByIdAsync(cId);
                return data.VALUE ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult RPreCostingReport(int csId)
        {
            return View(csId);
        }
        
        [HttpGet]
        [Authorize(Policy = "CostingPDLCost")]
        public async Task<IActionResult> DeletePreCostingMaster(string id)
        {
            try
            {
                var cosPreCostingMasterViewModel = await _cosPreCostingMaster.GetInitObjects(new CosPreCostingMasterViewModel());

                cosPreCostingMasterViewModel.CosPreCostingMaster = await _cosPreCostingMaster.FindByIdAllAsync(int.Parse(_protector.Unprotect(id)));
                var detailsList = await _cosPreCostingDetails.GetAllDetailsAsync(cosPreCostingMasterViewModel.CosPreCostingMaster.CSID);
                cosPreCostingMasterViewModel.CosPreCostingDetailsList = detailsList.ToList();
                cosPreCostingMasterViewModel.CosPreCostingMaster.EncryptedId = _protector.Protect(cosPreCostingMasterViewModel.CosPreCostingMaster.CSID.ToString());

                if (cosPreCostingMasterViewModel.CosPreCostingMaster!= null)
                {
                    if (await _cosPreCostingDetails.DeleteRange(cosPreCostingMasterViewModel.CosPreCostingDetailsList))
                    {
                        if (await _cosPreCostingMaster.Delete(cosPreCostingMasterViewModel.CosPreCostingMaster))
                        {
                            TempData["message"] = "Successfully Deleted Pre-Costing";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Delete Pre-Costing";
                            TempData["type"] = "error";
                            return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
                        }
                    }
                    TempData["message"] = "Failed to Delete Pre-Costing";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");
                }
                TempData["message"] = "Failed to Delete Pre-Costing";
                TempData["type"] = "error";
                return RedirectToAction($"GetPreCostingMaster", $"CosPreCostingMaster");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region NextUse
        /*
        [HttpGet]
        public async Task<IActionResult> EditPreCostingMaster(string csId)
        {
            try
            {
                int decryptedId = Int32.Parse(_protector.Unprotect(csId));
                var costingInfo = await _cosPreCostingMaster.FindByIdAllAsync(decryptedId);

                if (costingInfo != null)
                {
                    var costDetails = await _cosPreCostingDetails.GetAllDetailsAsync(decryptedId);

                    CosPreCostingMasterViewModel costingMasterViewModel = new CosPreCostingMasterViewModel();

                    costingMasterViewModel.CosPreCostingMaster = costingInfo;
                    costingMasterViewModel.CosPreCostingDetailsList = costDetails.ToList();
                    costingMasterViewModel.StandardCons = await _standardCons.FindByIdAsync(costingMasterViewModel.CosPreCostingMaster.SCID);
                    costingMasterViewModel.FixedCost = await _fixedCost.FindByIdAsync(costingMasterViewModel.CosPreCostingMaster.FCID);

                    await GetInitObjectsAsync(costingMasterViewModel);

                    return View(costingMasterViewModel);
                }
                else
                {
                    TempData["message"] = "Fabric details not found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Invalid Operation.";
                TempData["type"] = "error";
                return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRndFabricInfo(ModifyRndFabricInfoViewModel modifyRndFabricInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (modifyRndFabricInfoViewModel.rND_FABRICINFO.FABCODE == _protector.Unprotect(modifyRndFabricInfoViewModel.rND_FABRICINFO.EncryptedId))
                    {
                        var user = await userManager.GetUserAsync(User);
                        modifyRndFabricInfoViewModel.rND_FABRICINFO.USRID = user.Id;
                        var isRndFabricInfoUpdated = await rND_FABRICINFO.Update(modifyRndFabricInfoViewModel.rND_FABRICINFO);
                        var fabCode = _protector.Unprotect(modifyRndFabricInfoViewModel.rND_FABRICINFO.EncryptedId);

                        if (isRndFabricInfoUpdated)
                        {
                            foreach (var item in modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_FABRIC_COUNTINFO.TRNSID == 0))
                            {
                                var xTo = new RND_FABRIC_COUNTINFO()
                                {
                                    FABCODE = fabCode,
                                    COUNTID = item.rND_FABRIC_COUNTINFO.COUNTID,
                                    YARNTYPE = item.rND_FABRIC_COUNTINFO.YARNTYPE,
                                    LOTID = item.rND_FABRIC_COUNTINFO.LOTID,
                                    SUPPID = item.rND_FABRIC_COUNTINFO.SUPPID,
                                    RATIO = item.rND_FABRIC_COUNTINFO.RATIO,
                                    NE = item.rND_FABRIC_COUNTINFO.NE,
                                    YARNFOR = item.rND_FABRIC_COUNTINFO.YARNFOR
                                };

                                var yTo = new RND_YARNCONSUMPTION()
                                {
                                    FABCODE = fabCode,
                                    COUNTID = item.rND_YARNCONSUMPTION.COUNTID,
                                    YARNFOR = item.rND_YARNCONSUMPTION.YARNFOR,
                                    AMOUNT = item.rND_YARNCONSUMPTION.AMOUNT
                                };

                                var x = await rND_FABRIC_COUNTINFO.InsertByAsync(xTo);
                                var y = await rND_YARNCONSUMPTION.InsertByAsync(yTo);
                            }

                            foreach (var item in modifyRndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels.Where(e => e.rND_FABRIC_COUNTINFO.TRNSID > 0))
                            {
                                var a = await rND_FABRIC_COUNTINFO.Update(item.rND_FABRIC_COUNTINFO);
                                var b = await rND_YARNCONSUMPTION.Update(item.rND_YARNCONSUMPTION);
                            }

                            TempData["message"] = "Fabric details successfully updated.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
                        }
                    }
                    TempData["message"] = "Fabric details not found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetRndFabricInfo", $"RndFabricInfo");
                }
                else
                {
                    return View(modifyRndFabricInfoViewModel);
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
        */
        #endregion
    }
}