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
    public class FGsGatepassReturnRcvController : Controller
    {
        private readonly string title = "General Store Gatepass Return Receive Information";
        private readonly IF_GS_GATEPASS_RETURN_RCV_MASTER _fGsGatepassReturnRcvMaster;
        private readonly IF_GS_GATEPASS_RETURN_RCV_DETAILS _fGsGatepassReturnRcvDetails;
        private readonly IF_GS_GATEPASS_INFORMATION_M _fGsGatepassInformationM;
        private readonly IF_GS_PRODUCT_INFORMATION _fGsProductInformation;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public FGsGatepassReturnRcvController(IF_GS_GATEPASS_RETURN_RCV_MASTER fGsGatepassReturnRcvMaster,
            IF_GS_GATEPASS_RETURN_RCV_DETAILS fGsGatepassReturnRcvDetails,
            IF_GS_GATEPASS_INFORMATION_M fGsGatepassInformationM,
            IF_GS_PRODUCT_INFORMATION fGsProductInformation,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _fGsGatepassReturnRcvMaster = fGsGatepassReturnRcvMaster;
            _fGsGatepassReturnRcvDetails = fGsGatepassReturnRcvDetails;
            _fGsGatepassInformationM = fGsGatepassInformationM;
            _fGsProductInformation = fGsProductInformation;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult GetFGsGatepassReturnRcv()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

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

            var data = (List<F_GS_GATEPASS_RETURN_RCV_MASTER>)await _fGsGatepassReturnRcvMaster.GetAllFGenSRequirementAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.RCVDATE != null && m.RCVDATE.ToString().ToUpper().Contains(searchValue)
                                                 || m.GP.GPNO != null && m.GP.GPNO.ToUpper().Contains(searchValue)
                                                 || m.RCVD_BYNavigation.FIRST_NAME != null && m.RCVD_BYNavigation.FIRST_NAME.ToUpper().Contains(searchValue)
                                                 || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
        public async Task<IActionResult> CreateFGsGatepassReturnRcv()
        {
            try
            {
                return View(await _fGsGatepassReturnRcvMaster.GetInitObjByAsync(new FGsGatepassReturnRcvViewModel()));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsGatepassReturnRcv(FGsGatepassReturnRcvViewModel fGsGatepassReturnRcvViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvMaster.CREATED_BY = fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvMaster.UPDATED_BY = user.Id;
                fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvMaster.CREATED_AT = fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvMaster.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var fGsGatepassReturnRcvMaster = await _fGsGatepassReturnRcvMaster.GetInsertedObjByAsync(fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvMaster);

                if (fGsGatepassReturnRcvMaster.RCVID != 0)
                {
                    foreach (var item in fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetailsList)
                    {
                        item.RCVID = fGsGatepassReturnRcvMaster.RCVID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        if (await _fGsGatepassReturnRcvDetails.InsertByAsync(item))
                        {
                            atLeastOneInsert = true;
                        }
                    }
                    if (!atLeastOneInsert)
                    {
                        await _fGsGatepassReturnRcvMaster.Delete(fGsGatepassReturnRcvMaster);
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(CreateFGsGatepassReturnRcv), await _fGsGatepassReturnRcvMaster.GetInitObjByAsync(fGsGatepassReturnRcvViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFGsGatepassReturnRcv), $"FGsGatepassReturnRcv");
                }

                await _fGsGatepassReturnRcvMaster.Delete(fGsGatepassReturnRcvMaster);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateFGsGatepassReturnRcv), await _fGsGatepassReturnRcvMaster.GetInitObjByAsync(fGsGatepassReturnRcvViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFGsGatepassReturnRcv(string rcvId)
        {
            return View(await _fGsGatepassReturnRcvMaster.GetInitObjByAsync(await _fGsGatepassReturnRcvMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(rcvId)))));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetProductList(int gpId)
        {
            try
            {
                return Ok(await _fGsGatepassInformationM.GetProductListByGpIdAsync(gpId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetProductDetails(int prodId)
        {
            try
            {
                return Ok(await _fGsProductInformation.GetSingleProductByProductId(prodId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToList(FGsGatepassReturnRcvViewModel fGsGatepassReturnRcvViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fGsGatepassReturnRcvViewModel.IsDelete)
                {
                    var fGsGatepassReturnRcvDetails = fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetailsList[fGsGatepassReturnRcvViewModel.RemoveIndex];

                    if (fGsGatepassReturnRcvDetails.TRNSID > 0)
                    {
                        await _fGsGatepassReturnRcvDetails.Delete(fGsGatepassReturnRcvDetails);
                    }

                    fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetailsList.RemoveAt(fGsGatepassReturnRcvViewModel.RemoveIndex);
                }
                else if (!fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetailsList.Any(e => e.PRODID.Equals(fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetails.PRODID)))
                {
                    if (TryValidateModel(fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetails))
                    {
                        fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetailsList.Add(fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetails);
                    }
                }

                return PartialView($"GsGatepassReturnRcvDetailsPartialView", await _fGsGatepassReturnRcvMaster.GetInitDetailsObjByAsync(fGsGatepassReturnRcvViewModel));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
