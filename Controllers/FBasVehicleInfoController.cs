using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FBasVehicleInfoController : Controller
    {
        private readonly IF_BAS_VEHICLE_INFO _fBasVehicleInfo;
        private readonly IF_BAS_DRIVERINFO _fBasDriverinfo;
        private readonly IVEHICLE_TYPE _vehicleType;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "Vehicle Information";


        public FBasVehicleInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_VEHICLE_INFO fBasVehicleInfo,
            IF_BAS_DRIVERINFO fBasDriverinfo,
            IVEHICLE_TYPE vehicleType,
            UserManager<ApplicationUser> userManager
        )
        {
            _fBasVehicleInfo = fBasVehicleInfo;
            _fBasDriverinfo = fBasDriverinfo;
            _vehicleType = vehicleType;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        //[Route("GetAll")]
        public IActionResult GetFBasVehicleInfo()
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

            var data = (List<F_BAS_VEHICLE_INFO>)await _fBasVehicleInfo.GetAllFBasVehicleInfoAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.VNUMBER != null && m.VNUMBER.ToString().ToUpper().Contains(searchValue)
                                       || m.VEHICLE_TYPE != null && m.VEHICLE_TYPE.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            foreach (var item in finalData)
            {
                item.EncryptedId = _protector.Protect(item.VID.ToString());
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
        public async Task<IActionResult> CreateFBasVehicleInfo()
        {
            try
            {
                return View(await _fBasVehicleInfo.GetInitObjByAsync(new FBasVehicleInfoViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFBasVehicleInfo(FBasVehicleInfoViewModel fBasVehicleInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fBasVehicleInfo.InsertByAsync(fBasVehicleInfoViewModel.VehicleInfo);

                    if (result)
                    {
                        TempData["message"] = $"Successfully Added {title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFBasVehicleInfo), $"FBasVehicleInfo");
                    }

                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(fBasVehicleInfoViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fBasVehicleInfoViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fBasVehicleInfoViewModel);
            }
        }

    }
}
