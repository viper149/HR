using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FPrFinishingProcessController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_FINISHING_PROCESS_MASTER _fPrFinishingProcessMaster;
        private readonly IF_PR_FINISHING_FAB_PROCESS _fPrFinishingFabProcess;
        private readonly IF_PR_FINISHING_FNPROCESS _fPrFinishingFnProcess;
        private readonly IF_PR_FIN_TROLLY _fPrFinTrolly;
        private readonly IF_PR_FN_MACHINE_INFO _fPrFnMachineInfo;
        private readonly IF_PR_PROCESS_MACHINEINFO _fPrProcessMachineInfo;
        private readonly IF_PR_PROCESS_TYPE_INFO _fPrProcessTypeInfo;
        private readonly IF_PR_FN_PROCESS_TYPEINFO _fPrFnProcessTypeInfo;
        private readonly IF_PR_FN_CHEMICAL_CONSUMPTION _fPrFnChemicalConsumption;
        private readonly IF_PR_WEAVING_PROCESS_DETAILS_B _fPrWeavingProcessDetailsB;

        public FPrFinishingProcessController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_FINISHING_PROCESS_MASTER fPrFinishingProcessMaster,
            IF_PR_FINISHING_FAB_PROCESS fPrFinishingFabProcess,
            IF_PR_FINISHING_FNPROCESS fPrFinishingFnProcess,
            IF_PR_FIN_TROLLY fPrFinTrolly,
            IF_PR_FN_MACHINE_INFO fPrFnMachineInfo,
            IF_PR_PROCESS_MACHINEINFO fPrProcessMachineInfo,
            IF_PR_PROCESS_TYPE_INFO fPrProcessTypeInfo,
            IF_PR_FN_PROCESS_TYPEINFO fPrFnProcessTypeInfo,
            IF_PR_FN_CHEMICAL_CONSUMPTION fPrFnChemicalConsumption,
            IF_PR_WEAVING_PROCESS_DETAILS_B fPrWeavingProcessDetailsB
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrFinishingProcessMaster = fPrFinishingProcessMaster;
            _fPrFinishingFabProcess = fPrFinishingFabProcess;
            _fPrFinishingFnProcess = fPrFinishingFnProcess;
            _fPrFinTrolly = fPrFinTrolly;
            _fPrFnMachineInfo = fPrFnMachineInfo;
            _fPrProcessMachineInfo = fPrProcessMachineInfo;
            _fPrProcessTypeInfo = fPrProcessTypeInfo;
            _fPrFnProcessTypeInfo = fPrFnProcessTypeInfo;
            _fPrFnChemicalConsumption = fPrFnChemicalConsumption;
            _fPrWeavingProcessDetailsB = fPrWeavingProcessDetailsB;
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

                var data = await _fPrFinishingProcessMaster.GetAllAsync();

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
                    data = data.Where(m =>
                                                m.STYLE_NAME != null && m.STYLE_NAME.ToUpper().Contains(searchValue) 
                                            || (m.FN_PROCESSDATE != null && m.FN_PROCESSDATE.ToString().Contains(searchValue))
                                            || (m.BEAM_NO != null && m.BEAM_NO.ToUpper().Contains(searchValue))
                                            || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                //foreach (var item in finalData)
                //{
                //    item.EncryptedId = _protector.Protect(item.FN_PROCESSID.ToString());
                //}
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
        public IActionResult GetFinishingProcessList()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> CreateFinishingProcess()
        {
            try
            {
                var prFinishingProcessViewModel = await GetInfo(new PrFinishingProcessViewModel());
                prFinishingProcessViewModel.FPrFinishingProcessMaster = new F_PR_FINISHING_PROCESS_MASTER()
                {
                    FN_PROCESSDATE = DateTime.Now.AddHours(-6)
                };
                prFinishingProcessViewModel.FPrFinishingFnProcess = new F_PR_FINISHING_FNPROCESS()
                {
                    FIN_PROCESSDATE = DateTime.Now.AddHours(-6),
                    SECID = 167,
                    LENGTH_IN = 0,
                    LENGTH_OUT = 0,
                    LENGTH_RCV = 0
                };
                //prFinishingProcessViewModel.FPrFinishingFabProcess = new F_PR_FINISHING_FAB_PROCESS()
                //{
                //    FAB_PROCESSDATE = DateTime.Now
                //};
                //prFinishingProcessViewModel.FPrFnChemicalConsumptions = new F_PR_FN_CHEMICAL_CONSUMPTION()
                //{
                //    TRNSDATE = DateTime.Now
                //};

                return View(prFinishingProcessViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateFinishingProcess(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    prFinishingProcessViewModel.FPrFinishingProcessMaster.CREATED_BY = user.Id;
                    prFinishingProcessViewModel.FPrFinishingProcessMaster.UPDATED_BY = user.Id;
                    prFinishingProcessViewModel.FPrFinishingProcessMaster.CREATED_AT = DateTime.Now;
                    prFinishingProcessViewModel.FPrFinishingProcessMaster.UPDATED_AT = DateTime.Now;
                    var finishingId = await _fPrFinishingProcessMaster.InsertAndGetIdAsync(prFinishingProcessViewModel.FPrFinishingProcessMaster);

                    if (finishingId != 0)
                    {
                        foreach (var item in prFinishingProcessViewModel.FPrFinishingFnProcessList)
                        {
                            item.FN_PROCESSID = finishingId;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            await _fPrFinishingFnProcess.InsertByAsync(item);
                        }
                        TempData["message"] = "Successfully Finishing Process Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetFinishingProcessList", $"FPrFinishingProcess");
                    }
                    TempData["message"] = "Failed to Create Finishing Process.";
                    TempData["type"] = "error";
                    return View(await GetInfo(prFinishingProcessViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(prFinishingProcessViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Finishing Process.";
                TempData["type"] = "error";
                return View(await GetInfo(prFinishingProcessViewModel));
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditFinishingProcess(string id)
        {
            try
            {
                var finId = int.Parse(_protector.Unprotect(id));
                var prFinishingProcessViewModel = await GetEditInfo(finId);
                if (prFinishingProcessViewModel.FPrFinishingProcessMaster != null)
                {
                    prFinishingProcessViewModel = await GetInfo(prFinishingProcessViewModel);
                    prFinishingProcessViewModel.FPrFinishingProcessMaster.EncryptedId = _protector.Protect(prFinishingProcessViewModel.FPrFinishingProcessMaster.FN_PROCESSID.ToString());

                    prFinishingProcessViewModel.FPrFinishingFnProcess = new F_PR_FINISHING_FNPROCESS()
                    {
                        LENGTH_RCV = prFinishingProcessViewModel.FPrFinishingProcessMaster.LENGTH_BEAM,
                        LENGTH_IN = prFinishingProcessViewModel.FPrFinishingProcessMaster.LENGTH_BEAM,
                        LENGTH_OUT = prFinishingProcessViewModel.FPrFinishingProcessMaster.LENGTH_BEAM,
                        SECID = 167,
                        FIN_PROCESSDATE = DateTime.Now.AddHours(-6)
                    };
                    //prFinishingProcessViewModel.FPrFinishingFabProcess = new F_PR_FINISHING_FAB_PROCESS()
                    //{
                    //    FAB_PROCESSDATE = DateTime.Now
                    //};
                    //prFinishingProcessViewModel.FPrFnChemicalConsumptions = new F_PR_FN_CHEMICAL_CONSUMPTION()
                    //{
                    //    TRNSDATE = DateTime.Now
                    //};
                    return View(prFinishingProcessViewModel);
                }

                TempData["message"] = "Finishing Process Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFinishingProcessList", $"FPrFinishingProcess");
            }
            catch (Exception)
            {
                TempData["message"] = "Finishing Process Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFinishingProcessList", $"FPrFinishingProcess");
            }
        }

        public async Task<PrFinishingProcessViewModel> GetEditInfo(int finId)
        {
            try
            {
                var prFinishingProcessViewModel = await _fPrFinishingProcessMaster.GetFinishingDetails(finId);
                return prFinishingProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFinishingProcess(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var finId = int.Parse(_protector.Unprotect(prFinishingProcessViewModel.FPrFinishingProcessMaster.EncryptedId));
                    if (prFinishingProcessViewModel.FPrFinishingProcessMaster.FN_PROCESSID == finId)
                    {
                        var finishDetails = await _fPrFinishingProcessMaster.FindByIdAsync(finId);

                        var user = await _userManager.GetUserAsync(User);
                        prFinishingProcessViewModel.FPrFinishingProcessMaster.UPDATED_BY = user.Id;
                        prFinishingProcessViewModel.FPrFinishingProcessMaster.UPDATED_AT = DateTime.Now;
                        prFinishingProcessViewModel.FPrFinishingProcessMaster.CREATED_AT = finishDetails.CREATED_AT;
                        prFinishingProcessViewModel.FPrFinishingProcessMaster.CREATED_BY = finishDetails.CREATED_BY;

                        var isUpdated = await _fPrFinishingProcessMaster.Update(prFinishingProcessViewModel.FPrFinishingProcessMaster);
                        if (isUpdated)
                        {
                            foreach (var item in prFinishingProcessViewModel.FPrFinishingFnProcessList.Where(e => e.FIN_PROCESSID == 0))
                            {
                                item.FN_PROCESSID = finId;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;

                                await _fPrFinishingFnProcess.InsertByAsync(item);
                            }
                            TempData["message"] = "Successfully Updated Finishing Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFinishingProcessList", $"FPrFinishingProcess");
                        }
                        TempData["message"] = "Failed to Update Finishing Details.";
                        TempData["type"] = "error";
                        prFinishingProcessViewModel = await GetEditInfo(finId);
                        prFinishingProcessViewModel = await GetInfo(prFinishingProcessViewModel);
                        return View(prFinishingProcessViewModel);
                    }
                    TempData["message"] = "Invalid Finishing Info.";
                    TempData["type"] = "error";
                    prFinishingProcessViewModel = await GetEditInfo(finId);
                    prFinishingProcessViewModel = await GetInfo(prFinishingProcessViewModel);
                    return View(prFinishingProcessViewModel);
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                var fnId = int.Parse(_protector.Unprotect(prFinishingProcessViewModel.FPrFinishingProcessMaster.EncryptedId));
                prFinishingProcessViewModel = await GetEditInfo(fnId);
                prFinishingProcessViewModel = await GetInfo(prFinishingProcessViewModel);
                return View(prFinishingProcessViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Finishing.";
                TempData["type"] = "error";
                var fnId = int.Parse(_protector.Unprotect(prFinishingProcessViewModel.FPrFinishingProcessMaster.EncryptedId));
                prFinishingProcessViewModel = await GetEditInfo(fnId);
                prFinishingProcessViewModel = await GetInfo(prFinishingProcessViewModel);
                return View(prFinishingProcessViewModel);
            }
        }

        public async Task<PrFinishingProcessViewModel> GetInfo(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            prFinishingProcessViewModel = await _fPrFinishingProcessMaster.GetInitObjects(prFinishingProcessViewModel);
            return prFinishingProcessViewModel;
        }
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddFabricList(PrFinishingProcessViewModel prFinishingProcessViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        var flag = prFinishingProcessViewModel.FPrFinishingFabProcessList.Where(c => c.FAB_MACHINEID.Equals(prFinishingProcessViewModel.FPrFinishingFabProcess.FAB_MACHINEID));

        //        if (!flag.Any())
        //        {
        //            prFinishingProcessViewModel.FPrFinishingFabProcessList.Add(prFinishingProcessViewModel.FPrFinishingFabProcess);
        //        }
        //        prFinishingProcessViewModel = await GetFabricDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddFabricList", prFinishingProcessViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        prFinishingProcessViewModel = await GetFabricDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddFabricList", prFinishingProcessViewModel);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveFabricFromList(PrFinishingProcessViewModel prFinishingProcessViewModel, string removeIndexValue)
        //{
        //    ModelState.Clear();
        //    if (prFinishingProcessViewModel.FPrFinishingFabProcessList[int.Parse(removeIndexValue)].FAB_PROCESSID != 0)
        //    {
        //        await _fPrFinishingFabProcess.Delete(prFinishingProcessViewModel.FPrFinishingFabProcessList[int.Parse(removeIndexValue)]);
        //    }
        //    prFinishingProcessViewModel.FPrFinishingFabProcessList.RemoveAt(int.Parse(removeIndexValue));
        //    prFinishingProcessViewModel = await GetFabricDetailsAsync(prFinishingProcessViewModel);
        //    return PartialView($"AddFabricList", prFinishingProcessViewModel);
        //}
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> GetFabricList(PrFinishingProcessViewModel prFinishingProcessViewModel)
        //{
        //    try
        //    {
        //        prFinishingProcessViewModel.FPrFinishingFabProcessList =
        //            (List<F_PR_FINISHING_FAB_PROCESS>) await _fPrFinishingFabProcess.GetFabricList(prFinishingProcessViewModel.FPrFinishingProcessMaster
        //                .FN_PROCESSID);
        //        prFinishingProcessViewModel = await GetFabricDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddFabricList", prFinishingProcessViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        prFinishingProcessViewModel = await GetFabricDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddFabricList", prFinishingProcessViewModel);
        //    }
        //}
        
        //public async Task<PrFinishingProcessViewModel> GetFabricDetailsAsync(PrFinishingProcessViewModel prFinishingProcessViewModel)
        //{
        //    prFinishingProcessViewModel.FPrFinishingFabProcessList = (List<F_PR_FINISHING_FAB_PROCESS>) await _fPrFinishingFabProcess.GetInitFabricData(prFinishingProcessViewModel.FPrFinishingFabProcessList);
        //    return prFinishingProcessViewModel;
        //}
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFinishList(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            try
            {
                ModelState.Clear();
                var flag = prFinishingProcessViewModel.FPrFinishingFnProcessList.Where(c => c.FN_MACHINEID.Equals(prFinishingProcessViewModel.FPrFinishingFnProcess.FN_MACHINEID) && c.FIN_PROCESSDATE.Equals(prFinishingProcessViewModel.FPrFinishingFnProcess.FIN_PROCESSDATE) && c.TROLLNO.Equals(prFinishingProcessViewModel.FPrFinishingFnProcess.TROLLNO) && c.FIN_PRO_TYPEID.Equals(prFinishingProcessViewModel.FPrFinishingFnProcess.FIN_PRO_TYPEID) && c.PROCESS_BY.Equals(prFinishingProcessViewModel.FPrFinishingFnProcess.PROCESS_BY) && c.SHIFT.Equals(prFinishingProcessViewModel.FPrFinishingFnProcess.SHIFT));

                if (!flag.Any())
                {
                    prFinishingProcessViewModel.FPrFinishingFnProcessList.Add(prFinishingProcessViewModel.FPrFinishingFnProcess);
                }
                prFinishingProcessViewModel = await GetFinishDetailsAsync(prFinishingProcessViewModel);
                return PartialView($"AddFinishList", prFinishingProcessViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prFinishingProcessViewModel = await GetFinishDetailsAsync(prFinishingProcessViewModel);
                return PartialView($"AddFinishList", prFinishingProcessViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFinishFromList(PrFinishingProcessViewModel prFinishingProcessViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (prFinishingProcessViewModel.FPrFinishingFnProcessList[int.Parse(removeIndexValue)].FIN_PROCESSID != 0)
            {
                await _fPrFinishingFnProcess.Delete(prFinishingProcessViewModel.FPrFinishingFnProcessList[int.Parse(removeIndexValue)]);
            }
            prFinishingProcessViewModel.FPrFinishingFnProcessList.RemoveAt(int.Parse(removeIndexValue));
            prFinishingProcessViewModel = await GetFinishDetailsAsync(prFinishingProcessViewModel);
            return PartialView($"AddFinishList", prFinishingProcessViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetFinishList(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            try
            {
                prFinishingProcessViewModel.FPrFinishingFnProcessList =
                    (List<F_PR_FINISHING_FNPROCESS>) await _fPrFinishingFnProcess.GetFinishList(prFinishingProcessViewModel.FPrFinishingProcessMaster
                        .FN_PROCESSID);
                prFinishingProcessViewModel = await GetFinishDetailsAsync(prFinishingProcessViewModel);
                return PartialView($"AddFinishList", prFinishingProcessViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                prFinishingProcessViewModel = await GetFinishDetailsAsync(prFinishingProcessViewModel);
                return PartialView($"AddFinishList", prFinishingProcessViewModel);
            }
        }

        public async Task<PrFinishingProcessViewModel> GetFinishDetailsAsync(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            prFinishingProcessViewModel.FPrFinishingFnProcessList = (List<F_PR_FINISHING_FNPROCESS>)await _fPrFinishingFnProcess.GetInitFinishData(prFinishingProcessViewModel.FPrFinishingFnProcessList);
            return prFinishingProcessViewModel;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddChemList(PrFinishingProcessViewModel prFinishingProcessViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        var flag = prFinishingProcessViewModel.FPrFnChemicalConsumptionsList.Where(c => c.CHEM_PROD_ID.Equals(prFinishingProcessViewModel.FPrFnChemicalConsumptions.CHEM_PROD_ID));

        //        if (!flag.Any())
        //        {
        //            prFinishingProcessViewModel.FPrFnChemicalConsumptionsList.Add(prFinishingProcessViewModel.FPrFnChemicalConsumptions);
        //        }
        //        else
        //        {
        //            foreach (var item in prFinishingProcessViewModel.FPrFnChemicalConsumptionsList.Where(item => item.CHEM_PROD_ID == prFinishingProcessViewModel.FPrFnChemicalConsumptions.CHEM_PROD_ID))
        //            {
        //                item.QTY += prFinishingProcessViewModel.FPrFnChemicalConsumptions.QTY;
        //            }
        //        }
        //        prFinishingProcessViewModel = await GetChemDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddChemList", prFinishingProcessViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        prFinishingProcessViewModel = await GetChemDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddChemList", prFinishingProcessViewModel);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveChemFromList(PrFinishingProcessViewModel prFinishingProcessViewModel, string removeIndexValue)
        //{
        //    ModelState.Clear();
        //    if (prFinishingProcessViewModel.FPrFnChemicalConsumptionsList[int.Parse(removeIndexValue)].TRNSID != 0)
        //    {
        //        await _fPrFnChemicalConsumption.Delete(prFinishingProcessViewModel.FPrFnChemicalConsumptionsList[int.Parse(removeIndexValue)]);
        //    }
        //    prFinishingProcessViewModel.FPrFnChemicalConsumptionsList.RemoveAt(int.Parse(removeIndexValue));
        //    prFinishingProcessViewModel = await GetChemDetailsAsync(prFinishingProcessViewModel);
        //    return PartialView($"AddChemList", prFinishingProcessViewModel);
        //}
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> GetChemList(PrFinishingProcessViewModel prFinishingProcessViewModel)
        //{
        //    try
        //    {
        //        prFinishingProcessViewModel.FPrFnChemicalConsumptionsList =
        //            (List<F_PR_FN_CHEMICAL_CONSUMPTION>) await _fPrFnChemicalConsumption.GetChemList(prFinishingProcessViewModel.FPrFinishingProcessMaster
        //                .SETID);
        //        prFinishingProcessViewModel = await GetChemDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddChemList", prFinishingProcessViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        prFinishingProcessViewModel = await GetChemDetailsAsync(prFinishingProcessViewModel);
        //        return PartialView($"AddChemList", prFinishingProcessViewModel);
        //    }
        //}

        //public async Task<PrFinishingProcessViewModel> GetChemDetailsAsync(PrFinishingProcessViewModel prFinishingProcessViewModel)
        //{
        //    prFinishingProcessViewModel.FPrFnChemicalConsumptionsList = (List<F_PR_FN_CHEMICAL_CONSUMPTION>) await _fPrFnChemicalConsumption.GetInitChemData(prFinishingProcessViewModel.FPrFnChemicalConsumptionsList);
        //    return prFinishingProcessViewModel;
        //}

        [HttpGet]
        public async Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> GetBeamDetails(int beamId)
        {
            var result = await _fPrFinishingProcessMaster.GetBeamDetails(beamId);
            return result;
        }

        [HttpGet]
        public async Task<F_PR_WEAVING_PROCESS_DETAILS_B> GetLoomDetails(int loomId)
        {
            var result = await _fPrFinishingProcessMaster.GetLoomDetails(loomId);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MpId">Machine Preparation PK</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<F_PR_WEAVING_PROCESS_DETAILS_B>> GetStyleDetails(int fabcode,int setId)
        {
            var result = await _fPrFinishingProcessMaster.GetStyleDetails(fabcode, setId);
            return result.ToList();
        }

        [HttpGet]
        public async Task<IEnumerable<F_PR_WEAVING_PROCESS_DETAILS_B>> GetStyleDetailsEdit(int fabcode)
        {
            var result = await _fPrFinishingProcessMaster.GetStyleDetailsEdit(fabcode);
            return result.ToList();
        }


        [HttpGet]
        public async Task<dynamic> GetDoffLength(int doffId)
        {
            var result = await _fPrWeavingProcessDetailsB.GetBeamDetailsByDoffIdAsync(doffId);
            var doffLength = await _fPrWeavingProcessDetailsB.FindByIdAsync(doffId);
            var obj = new
            {
                Balance = result,
                DoffLength = doffLength.LENGTH_BULK
            };
            return obj;
        }


        [HttpGet]
        public async Task<dynamic> GetStyleDetailsBySetId(int setId)
        {
            var result = await _fPrFinishingProcessMaster.GetStyleDetailsBySetId(setId);
           
            return result;
        }

    }
}