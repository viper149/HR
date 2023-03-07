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
    public class FPrFinishingMachinePreperationController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_PR_FINISHING_MACHINE_PREPARATION _fPrFinishingMachinePreparation;
        private readonly IF_PR_FN_CHEMICAL_CONSUMPTION _fPrFnChemicalConsumption;
        private readonly IF_PR_FINIGHING_DOFF_FOR_MACHINE _fPrFinighingDoffForMachine;

        public FPrFinishingMachinePreperationController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_PR_FINISHING_MACHINE_PREPARATION fPrFinishingMachinePreparation,
            IF_PR_FN_CHEMICAL_CONSUMPTION fPrFnChemicalConsumption,
            IF_PR_FINIGHING_DOFF_FOR_MACHINE fPrFinighingDoffForMachine
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fPrFinishingMachinePreparation = fPrFinishingMachinePreparation;
            _fPrFnChemicalConsumption = fPrFnChemicalConsumption;
            _fPrFinighingDoffForMachine = fPrFinighingDoffForMachine;
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request
                    .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()
                    .ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var recordsTotal = 0;

                var data = await _fPrFinishingMachinePreparation.GetAllAsync();

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
                        m.FABCODENavigation.STYLE_NAME != null &&
                        m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue)
                        || (m.MACHINE_NONavigation.NAME != null && m.MACHINE_NONavigation.NAME.Contains(searchValue))
                        || (m.FINISH_ROUTE != null && m.FINISH_ROUTE.Contains(searchValue))
                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                    ).ToList();
                }

                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.FPMID.ToString());
                }

                var jsonData = new
                    { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetFinishingMachinePreparationList()
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
        public async Task<IActionResult> CreateFinishingMachinePreparation()
        {
            var fPrFinishingMachineCreatePreparationViewModel =
                await GetInfo(new FPrFinishingMachineCreatePreparationViewModel());
            return View(fPrFinishingMachineCreatePreparationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFinishingMachinePreparation(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.CREATED_BY = user.Id;
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.UPDATED_BY = user.Id;
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.CREATED_AT =
                        DateTime.Now;
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.UPDATED_AT =
                        DateTime.Now;
                    var isInsertred =
                        await _fPrFinishingMachinePreparation.GetInsertedObjByAsync(
                            fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation);

                    if (isInsertred != null)
                    {
                        foreach (var item in fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions
                            .Where(c => c.TRNSID.Equals(0)))
                        {
                            item.FPMID = isInsertred.FPMID;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;

                            await _fPrFnChemicalConsumption.InsertByAsync(item);
                        }

                        foreach (var item in fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines
                            .Where(c => c.DID.Equals(0)))
                        {
                            item.FPMID = isInsertred.FPMID;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;

                            await _fPrFinighingDoffForMachine.InsertByAsync(item);
                        }

                        TempData["message"] = "Successfully Finishing Machine Prepared.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetFinishingMachinePreparationList",
                            $"FPrFinishingMachinePreperation");
                    }

                    TempData["message"] = "Failed to Prepared Finishing Machine.";
                    TempData["type"] = "error";
                    return View(await GetInfo(fPrFinishingMachineCreatePreparationViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrFinishingMachineCreatePreparationViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Prepared Finishing Machine.";
                TempData["type"] = "error";
                return View(await GetInfo(fPrFinishingMachineCreatePreparationViewModel));
            }
        }

        public async Task<FPrFinishingMachineCreatePreparationViewModel> GetInfo(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            fPrFinishingMachineCreatePreparationViewModel =
                await _fPrFinishingMachinePreparation.GetInitObjects(fPrFinishingMachineCreatePreparationViewModel);
            return fPrFinishingMachineCreatePreparationViewModel;
        }

        [HttpGet]
        public async Task<IActionResult> EditFinishingMachinePreparation(string id)
        {
            try
            {
                var machineId = int.Parse(_protector.Unprotect(id));
                var fPrFinishingMachineCreatePreparationViewModel =
                    await _fPrFinishingMachinePreparation.GetEditData(machineId);

                if (fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation != null)
                {
                    fPrFinishingMachineCreatePreparationViewModel =
                        await GetInfo(fPrFinishingMachineCreatePreparationViewModel);
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.EncryptedId =
                        _protector.Protect(fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation
                            .FPMID.ToString());

                    return View(fPrFinishingMachineCreatePreparationViewModel);
                }

                TempData["message"] = "Machine Preparation Details Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFinishingMachinePreparationList", $"FPrFinishingMachinePreperation");
            }
            catch (Exception)
            {
                TempData["message"] = "Machine Preparation Details Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetFinishingMachinePreparationList", $"FPrFinishingMachinePreperation");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFinishingMachinePreparation(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rcvId = int.Parse(_protector.Unprotect(fPrFinishingMachineCreatePreparationViewModel
                        .FPrFinishingMachinePreparation.EncryptedId));
                    if (fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.FPMID == rcvId)
                    {
                        var machineDetails = await _fPrFinishingMachinePreparation.FindByIdAsync(rcvId);

                        var user = await _userManager.GetUserAsync(User);
                        fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.UPDATED_BY =
                            user.Id;
                        fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.UPDATED_AT =
                            DateTime.Now;
                        fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.CREATED_AT =
                            machineDetails.CREATED_AT;
                        fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.CREATED_BY =
                            machineDetails.CREATED_BY;

                        var isUpdated = await _fPrFinishingMachinePreparation.Update(
                            fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation);
                        if (isUpdated)
                        {
                            foreach (var item in fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions
                                .Where(c => c.TRNSID.Equals(0)))
                            {
                                item.FPMID = machineDetails.FPMID;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_BY = user.Id;
                                item.CREATED_AT = DateTime.Now;
                                item.UPDATED_AT = DateTime.Now;

                                await _fPrFnChemicalConsumption.InsertByAsync(item);
                            }

                            foreach (var item in fPrFinishingMachineCreatePreparationViewModel
                                .FPrFinighingDoffForMachines.Where(c => c.DID.Equals(0)))
                            {
                                item.FPMID = machineDetails.FPMID;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_BY = user.Id;
                                item.CREATED_AT = DateTime.Now;
                                item.UPDATED_AT = DateTime.Now;

                                await _fPrFinighingDoffForMachine.InsertByAsync(item);
                            }

                            TempData["message"] = "Successfully Updated Finishing Machine Preparation.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFinishingMachinePreparationList",
                                $"FPrFinishingMachinePreperation");
                        }

                        TempData["message"] = "Failed to Update Finishing Beam Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetFinishingMachinePreparationList",
                            $"FPrFinishingMachinePreperation");
                    }

                    TempData["message"] = "Invalid Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetFinishingMachinePreparationList", $"FPrFinishingMachinePreperation");
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                fPrFinishingMachineCreatePreparationViewModel =
                    await GetInfo(fPrFinishingMachineCreatePreparationViewModel);
                return View(fPrFinishingMachineCreatePreparationViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Finishing Details.";
                TempData["type"] = "error";
                fPrFinishingMachineCreatePreparationViewModel =
                    await GetInfo(fPrFinishingMachineCreatePreparationViewModel);
                return View(fPrFinishingMachineCreatePreparationViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChemList(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                ModelState.Clear();
                var flag = fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions.Where(c =>
                    c.CHEM_PROD_ID.Equals(fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumption
                        .CHEM_PROD_ID));

                if (!flag.Any())
                {
                    fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions.Add(
                        fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumption);
                }
                else
                {
                    foreach (var item in fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions.Where(
                        item => item.CHEM_PROD_ID == fPrFinishingMachineCreatePreparationViewModel
                            .FPrFnChemicalConsumption.CHEM_PROD_ID))
                    {
                        item.QTY += fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumption.QTY;
                    }
                }

                fPrFinishingMachineCreatePreparationViewModel =
                    await GetChemDetailsAsync(fPrFinishingMachineCreatePreparationViewModel);
                return PartialView($"AddChemList", fPrFinishingMachineCreatePreparationViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrFinishingMachineCreatePreparationViewModel =
                    await GetChemDetailsAsync(fPrFinishingMachineCreatePreparationViewModel);
                return PartialView($"AddChemList", fPrFinishingMachineCreatePreparationViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveChemFromList(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel,
            string removeIndexValue)
        {
            ModelState.Clear();
            if (fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions[int.Parse(removeIndexValue)]
                .TRNSID != 0)
            {
                await _fPrFnChemicalConsumption.Delete(
                    fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions
                        [int.Parse(removeIndexValue)]);
            }

            fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions.RemoveAt(
                int.Parse(removeIndexValue));
            fPrFinishingMachineCreatePreparationViewModel =
                await GetChemDetailsAsync(fPrFinishingMachineCreatePreparationViewModel);
            return PartialView($"AddChemList", fPrFinishingMachineCreatePreparationViewModel);
        }

        public async Task<FPrFinishingMachineCreatePreparationViewModel> GetChemDetailsAsync(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions =
                (List<F_PR_FN_CHEMICAL_CONSUMPTION>)await _fPrFnChemicalConsumption.GetInitChemData(
                    fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumptions);
            return fPrFinishingMachineCreatePreparationViewModel;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoffList(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                ModelState.Clear();
                var flag = fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines.Where(c =>
                    c.FN_PROCESSID.Equals(fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachine
                        .FN_PROCESSID));

                if (!flag.Any())
                {
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines.Add(
                        fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachine);
                }

                fPrFinishingMachineCreatePreparationViewModel =
                    await GetDoffDetailsAsync(fPrFinishingMachineCreatePreparationViewModel);
                return PartialView($"AddDoffList", fPrFinishingMachineCreatePreparationViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrFinishingMachineCreatePreparationViewModel =
                    await GetDoffDetailsAsync(fPrFinishingMachineCreatePreparationViewModel);
                return PartialView($"AddDoffList", fPrFinishingMachineCreatePreparationViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDoffFromList(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel,
            string removeIndexValue)
        {
            ModelState.Clear();
            if (fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines[int.Parse(removeIndexValue)]
                .DID != 0)
            {
                await _fPrFinighingDoffForMachine.Delete(
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines[
                        int.Parse(removeIndexValue)]);
            }

            fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines.RemoveAt(
                int.Parse(removeIndexValue));
            fPrFinishingMachineCreatePreparationViewModel =
                await GetDoffDetailsAsync(fPrFinishingMachineCreatePreparationViewModel);
            return PartialView($"AddDoffList", fPrFinishingMachineCreatePreparationViewModel);
        }

        public async Task<FPrFinishingMachineCreatePreparationViewModel> GetDoffDetailsAsync(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines =
                (List<F_PR_FINIGHING_DOFF_FOR_MACHINE>)await _fPrFinighingDoffForMachine.GetInitDoffData(
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines,
                    fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.FIN_PRO_TYPEID ?? 0);
            return fPrFinishingMachineCreatePreparationViewModel;
        }


        public async Task<IEnumerable<dynamic>> GetStyleDetails(int fabcode)
        {
            try
            {
                var result = await _fPrFinishingMachinePreparation.GetStyleDetailsAsync(fabcode);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetDoffDetails(FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                var result = await _fPrFinishingMachinePreparation.GetDoffDetails(fPrFinishingMachineCreatePreparationViewModel);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}