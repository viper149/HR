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
    public class FGsGatePassController : Controller
    {
        private readonly IF_GS_GATEPASS_INFORMATION_M _fGsGatepassInformationM;
        private readonly IF_GS_GATEPASS_INFORMATION_D _fGsGatepassInformationD;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string title = "General Store Gate Pass Issue Information.";

        public FGsGatePassController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_SECTION fBasSection,
            IF_GS_GATEPASS_INFORMATION_M fGsGatepassInformationM,
            IF_GS_GATEPASS_INFORMATION_D fGsGatepassInformationD,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fBasSection = fBasSection;
            _fGsProductInformation = fGsProductInformation;
            _fGsGatepassInformationM = fGsGatepassInformationM;
            _fGsGatepassInformationD = fGsGatepassInformationD;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetFGsGatePass()
        {
            return View();
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
            var data = (List<F_GS_GATEPASS_INFORMATION_M>)await _fGsGatepassInformationM.GetAllGsGatePassAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.GPNO.ToUpper().Contains(searchValue)
                                       || (m.GPDATE != null && m.GPDATE.ToString().ToUpper().Contains(searchValue))
                                       || (m.DEPT.DEPTNAME != null && m.DEPT.DEPTNAME.ToUpper().Contains(searchValue))
                                       || (m.SEC.SECNAME != null && m.SEC.SECNAME.ToUpper().Contains(searchValue))
                                       || (m.EMP.FIRST_NAME != null && m.EMP.FIRST_NAME.ToUpper().Contains(searchValue))
                                       || (m.SENDTO != null && m.SENDTO.ToUpper().Contains(searchValue))
                                       || (m.ADDRESS != null && m.ADDRESS.ToUpper().Contains(searchValue))
                                       || (m.EMP_REQBYNavigation.FIRST_NAME != null && m.EMP_REQBYNavigation.FIRST_NAME.ToUpper().Contains(searchValue))
                                       || (m.V.VNUMBER != null && m.V.VNUMBER.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                    ).ToList();
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
        public async Task<IActionResult> CreateFGsGatePass()
        {
            return View(await _fGsGatepassInformationM.GetInitObjByAsync(new FGsGatePassViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsGatePass(FGsGatePassViewModel fGsGatePassViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            fGsGatePassViewModel.FGsGatepassInformationM.CREATED_BY = fGsGatePassViewModel.FGsGatepassInformationM.UPDATED_BY = user.Id;
            fGsGatePassViewModel.FGsGatepassInformationM.CREATED_AT = fGsGatePassViewModel.FGsGatepassInformationM.UPDATED_AT = DateTime.Now;

            var fGsGatepassInformationM = await _fGsGatepassInformationM.GetInsertedObjByAsync(fGsGatePassViewModel.FGsGatepassInformationM);
            fGsGatepassInformationM.GPNO = $"{fGsGatepassInformationM.GPID + 1}";
            await _fGsGatepassInformationM.Update(fGsGatepassInformationM);


            if (fGsGatepassInformationM.GPID > 0)
            {
                foreach (var item in fGsGatePassViewModel.FGsGatepassInformationDList)
                {
                    item.CREATED_BY = item.UPDATED_BY = user.Id;
                    item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                    item.GPID = fGsGatepassInformationM.GPID;
                }

                if (fGsGatePassViewModel.FGsGatepassInformationDList.Any())
                {
                    if (await _fGsGatepassInformationD.InsertRangeByAsync(fGsGatePassViewModel.FGsGatepassInformationDList))
                    {
                        TempData["message"] = $"Successfully added {title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsGatePass), $"FGsGatePass");
                    }
                }
            }

            TempData["message"] = $"Failed to add {title}. Please Add First";
            TempData["type"] = "error";
            return View(await _fGsGatepassInformationM.GetInitObjByAsync(fGsGatePassViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsGatePass(string gpId)
        {
            var fGsGatepassInformationM = await _fGsGatepassInformationM.FindByIdAsync(int.Parse(_protector.Unprotect(gpId)));

            if (fGsGatepassInformationM != null)
            {
                var fGsGatePassViewModel = await _fGsGatepassInformationM.GetInitObjByAsync(
                    await _fGsGatepassInformationM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gpId))));
                fGsGatePassViewModel.FGsGatepassInformationM = fGsGatepassInformationM;

                fGsGatePassViewModel.FGsGatepassInformationM.EncryptedId = _protector.Protect(fGsGatePassViewModel.FGsGatepassInformationM.GPID.ToString());
                return View(fGsGatePassViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFGsGatePass), $"FGsGatePass");
        }

        [HttpPost]
        public async Task<IActionResult> EditFGsGatePass(FGsGatePassViewModel fGsGatePassViewModel)
        {
            if (ModelState.IsValid)
            {
                var redirectToActionResult = RedirectToAction(nameof(GetFGsGatePass), $"FGsGatePass");
                fGsGatePassViewModel.FGsGatepassInformationM.GPID =
                    int.Parse(_protector.Unprotect(fGsGatePassViewModel.FGsGatepassInformationM.EncryptedId));
                var fGsGatepassInformationM = await _fGsGatepassInformationM.FindByIdAsync(fGsGatePassViewModel.FGsGatepassInformationM.GPID);

                if (fGsGatepassInformationM != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fGsGatePassViewModel.FGsGatepassInformationM.CREATED_AT = fGsGatepassInformationM.CREATED_AT;
                    fGsGatePassViewModel.FGsGatepassInformationM.CREATED_BY = fGsGatepassInformationM.CREATED_BY;
                    fGsGatePassViewModel.FGsGatepassInformationM.UPDATED_BY = user.Id;
                    fGsGatePassViewModel.FGsGatepassInformationM.UPDATED_AT = DateTime.Now;

                    if (await _fGsGatepassInformationM.Update(fGsGatePassViewModel.FGsGatepassInformationM))
                    {
                        var fGsGatepassInformationDs = fGsGatePassViewModel.FGsGatepassInformationDList.Where(e => e.TRNSID <= 0).ToList();

                        foreach (var item in fGsGatepassInformationDs)
                        {
                            item.GPID = fGsGatepassInformationM.GPID;
                            item.UPDATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                        }

                        if (await _fGsGatepassInformationD.InsertRangeByAsync(fGsGatepassInformationDs))
                        {
                            TempData["message"] = $"Successfully Updated {title}";
                            TempData["type"] = "success";
                            return redirectToActionResult;
                        }
                    }
                    TempData["message"] = $"Failed To Add {title}";
                    TempData["type"] = "error";
                    return redirectToActionResult;
                }
                TempData["message"] = $"{title} Not Found";
                TempData["type"] = "error";
                return redirectToActionResult;
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(await _fGsGatepassInformationM.GetInitObjByAsync(fGsGatePassViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> DetailsFGsGatePass(string gpId)
        {
            var fGsGatepassInformationMViewModel = await _fGsGatepassInformationM.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(gpId)));

            if (fGsGatepassInformationMViewModel != null)
            {
                fGsGatepassInformationMViewModel.FGsGatepassInformationM.EncryptedId =
                    _protector.Protect(fGsGatepassInformationMViewModel.FGsGatepassInformationM.GPID.ToString());
                return View(fGsGatepassInformationMViewModel);
            }
            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFGsGatePass), $"FGsGatePass");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetSectionByDeptId(int id)
        {
            try
            {
                return Ok(await _fBasSection.GetSectionsByDeptIdAsync(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetSingleProductByProductId(int id)
        {
            try
            {
                return Ok(await _fGsProductInformation.GetSingleProductByProductId(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToList(FGsGatePassViewModel fGsGatePassViewModel)
        {
            ModelState.Clear();
            if (fGsGatePassViewModel.IsDelete)
            {
                var fGsGatepassInformationD = fGsGatePassViewModel.FGsGatepassInformationDList[fGsGatePassViewModel.RemoveIndex];

                if (fGsGatepassInformationD.TRNSID > 0)
                {
                    await _fGsGatepassInformationD.Delete(fGsGatepassInformationD);
                }

                fGsGatePassViewModel.FGsGatepassInformationDList.RemoveAt(fGsGatePassViewModel.RemoveIndex);
            }
            else if (fGsGatePassViewModel.FGsGatepassInformationD.PRODID > 0)
            {
                if (!fGsGatePassViewModel.FGsGatepassInformationDList.Any(d => d.PRODID.Equals(fGsGatePassViewModel.FGsGatepassInformationD.PRODID)))
                {
                    if (TryValidateModel(fGsGatePassViewModel.FGsGatepassInformationD))
                    {

                        fGsGatePassViewModel.FGsGatepassInformationDList.Add(fGsGatePassViewModel
                            .FGsGatepassInformationD);
                    }
                }
            }

            return PartialView($"FGsGatepassInformationDPartialView", await _fGsGatepassInformationM.GetInitDetailsObjByAsync(fGsGatePassViewModel));
        }
    }
}