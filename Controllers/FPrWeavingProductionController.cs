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
    public class FPrWeavingProductionController : Controller
    {

        private readonly IF_PR_WEAVING_PRODUCTION _fPrWeavingProduction;
        private readonly IRND_FABRICINFO _rndFabricinfo;
        private readonly ICOM_EX_PI_DETAILS _comExPiDetails;
        private readonly IF_HRD_EMPLOYEE _fHrEmployee;
        private readonly ILOOM_TYPE _loomType;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Weaving Production Information";

        public FPrWeavingProductionController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_PR_WEAVING_PRODUCTION fPrWeavingProduction,
            IRND_FABRICINFO rndFabricinfo,
            ICOM_EX_PI_DETAILS comExPiDetails,
            IF_HRD_EMPLOYEE fHrEmployee,
            ILOOM_TYPE loomType,
            UserManager<ApplicationUser> userManager
        )
        {
            _fPrWeavingProduction = fPrWeavingProduction;
            _rndFabricinfo = rndFabricinfo;
            _comExPiDetails = comExPiDetails;
            _fHrEmployee = fHrEmployee;
            _loomType = loomType;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        //[Route("GetAll")]
        public IActionResult GetFPrWeavingProduction()
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
        //[Route("GetTableData")]
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

            var data = (List<F_PR_WEAVING_PRODUCTION>)await _fPrWeavingProduction.GetAllFPrWeavingProductionAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.OPT2.ToString().ToUpper().Contains(searchValue)
                                       || m.WV_PRODID.ToString().ToUpper().Contains(searchValue)
                                       || m.OPT1 != null && m.OPT1.ToString().ToUpper().Contains(searchValue)
                                       || m.OPT3 != null && m.OPT3.ToString().ToUpper().Contains(searchValue)
                                       || m.EMP is { FIRST_NAME: { } } && m.EMP.FIRST_NAME.ToString().ToUpper().Contains(searchValue)
                                       || m.LOOM is { LOOM_TYPE_NAME: { } } && m.LOOM.LOOM_TYPE_NAME.ToString().ToUpper().Contains(searchValue)
                                       || m.PO is { SO: { SO_NO: { } } } && m.PO.SO.SO_NO.ToString().ToUpper().Contains(searchValue)
                                       || m.FABCODENavigation is { STYLE_NAME: { } } && m.FABCODENavigation.STYLE_NAME.ToString().ToUpper().Contains(searchValue)
                                       || m.TOTAL_PROD != null && m.TOTAL_PROD.ToString().ToUpper().Contains(searchValue)).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            foreach (var item in finalData)
            {
                item.EncryptedId = _protector.Protect(item.WV_PRODID.ToString());
            }
            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        //[Route("Create")]
        public async Task<IActionResult> CreateFPrWeavingProduction()
        {
            try
            {
                return View(await _fPrWeavingProduction.GetInitObjByAsync(new FPrWeavingProductionViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFPrWeavingProduction(FPrWeavingProductionViewModel fPrWeavingProductionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fPrWeavingProduction.InsertRangeByAsync(fPrWeavingProductionViewModel.FPrWeavingProductionList);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction("GetFPrWeavingProduction", $"FPrWeavingProduction");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fPrWeavingProductionViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fPrWeavingProductionViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fPrWeavingProductionViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFPrWeavingProduction(string WpId)
        {
            var fPrWeavingProduction = await _fPrWeavingProduction.FindByIdAsync(int.Parse(_protector.Unprotect(WpId)));

            if (fPrWeavingProduction != null)
            {
                var fPrWeavingProductionViewModel = await _fPrWeavingProduction.GetInitObjByAsync(new FPrWeavingProductionViewModel());
                fPrWeavingProductionViewModel.FPrWeavingProduction = fPrWeavingProduction;

                fPrWeavingProductionViewModel.FPrWeavingProduction.EncryptedId = _protector.Protect(fPrWeavingProductionViewModel.FPrWeavingProduction.WV_PRODID.ToString());
                return View(fPrWeavingProductionViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFPrWeavingProduction), $"FPrWeavingProduction");
        }


        [HttpPost]
        public async Task<IActionResult> EditFPrWeavingProduction(
            FPrWeavingProductionViewModel fPrWeavingProductionViewModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    fPrWeavingProductionViewModel.FPrWeavingProduction.WV_PRODID = int.Parse(
                        _protector.Unprotect(fPrWeavingProductionViewModel.FPrWeavingProduction.EncryptedId));
                    var fPrWeavingProduction =
                        await _fPrWeavingProduction.FindByIdAsync(fPrWeavingProductionViewModel.FPrWeavingProduction
                            .WV_PRODID);
                    if (fPrWeavingProduction != null)
                    {
                        fPrWeavingProductionViewModel.FPrWeavingProduction.UPDATED_BY =
                            (await _userManager.GetUserAsync(User)).Id;
                        fPrWeavingProductionViewModel.FPrWeavingProduction.UPDATED_AT = DateTime.Now;
                        fPrWeavingProductionViewModel.FPrWeavingProduction.CREATED_AT = fPrWeavingProduction.CREATED_AT;
                        fPrWeavingProductionViewModel.FPrWeavingProduction.CREATED_BY = fPrWeavingProduction.CREATED_BY;

                        if (await _fPrWeavingProduction.Update(fPrWeavingProductionViewModel.FPrWeavingProduction))
                        {
                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFPrWeavingProduction), $"FPrWeavingProduction");
                        }

                        TempData["message"] = $"Failed to Update {title}.";
                        TempData["type"] = "error";
                        return RedirectToAction(nameof(GetFPrWeavingProduction), $"FPrWeavingProduction");
                    }

                    TempData["message"] = $"{title} Not Found";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFPrWeavingProduction), $"FPrWeavingProduction");
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";

                return View(await _fPrWeavingProduction.GetInitObjByAsync(new FPrWeavingProductionViewModel()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        [HttpGet]
        public async Task<RND_PRODUCTION_ORDER> GetStyleInfoBySo(int id)
        {
            try
            {
                var result = await _fPrWeavingProduction.GetStyleInfoBySo(id);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductionList(FPrWeavingProductionViewModel fPrWeavingProductionViewModel)
        {
            try
            {
                ModelState.Clear();
                var flag = fPrWeavingProductionViewModel.FPrWeavingProductionList.Where(c => c.POID.Equals(fPrWeavingProductionViewModel.FPrWeavingProduction.POID));

                if (!flag.Any())
                {
                    fPrWeavingProductionViewModel.FPrWeavingProductionList.Add(fPrWeavingProductionViewModel.FPrWeavingProduction);
                }

                fPrWeavingProductionViewModel = await GetProductionDetailsAsync(fPrWeavingProductionViewModel);
                return PartialView($"AddProductionList", fPrWeavingProductionViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fPrWeavingProductionViewModel = await GetProductionDetailsAsync(fPrWeavingProductionViewModel);
                return PartialView($"AddProductionList", fPrWeavingProductionViewModel);
            }
        }


        public async Task<FPrWeavingProductionViewModel> GetProductionDetailsAsync(FPrWeavingProductionViewModel fPrWeavingProductionViewModel)
        {
            try
            {
                fPrWeavingProductionViewModel = await _fPrWeavingProduction.GetProductionDetailsAsync(fPrWeavingProductionViewModel);
                return fPrWeavingProductionViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductionDetailsList(FPrWeavingProductionViewModel fPrWeavingProductionViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (fPrWeavingProductionViewModel.FPrWeavingProductionList[int.Parse(removeIndexValue)].WV_PRODID != 0)
            {
                await _fPrWeavingProduction.Delete(fPrWeavingProductionViewModel.FPrWeavingProductionList[int.Parse(removeIndexValue)]);
            }
            fPrWeavingProductionViewModel.FPrWeavingProductionList.RemoveAt(int.Parse(removeIndexValue));
            fPrWeavingProductionViewModel = await GetProductionDetailsAsync(fPrWeavingProductionViewModel);
            return PartialView($"AddProductionList", fPrWeavingProductionViewModel);
        }
    }
}
