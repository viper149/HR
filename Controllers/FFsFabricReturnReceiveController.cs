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
    public class FFsFabricReturnReceiveController : Controller
    {
        private readonly IF_FS_FABRIC_RETURN_RECEIVE _fabricReturnReceive;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " FABRIC RETURN RECEIVE ";

        public FFsFabricReturnReceiveController(IDataProtectionProvider dataProtectionProvider,
            IF_FS_FABRIC_RETURN_RECEIVE fabricReturnReceive,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fabricReturnReceive = fabricReturnReceive;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [HttpGet]
        public IActionResult GetFFsFabricReturnReceive()
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

            var data = (List<F_FS_FABRIC_RETURN_RECEIVE>)await _fabricReturnReceive.GetFFsFabricReturnReceive();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.RCVID.ToString().ToUpper().Contains(searchValue)
                                       || m.RCVDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.QTY_YDS.ToString().ToUpper().Contains(searchValue)
                                       || m.DC_NO.ToString().ToUpper().Contains(searchValue)
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

        public async Task<IActionResult> CreateFFsFabricReturnReceive()
        {

            try
            {
                var ffsfabricReturnReceive = await _fabricReturnReceive.GetInitObjByAsync(new FFsFabricReturnReceiveViewModel());


                return View(ffsfabricReturnReceive);
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateFFsFabricReturnReceive(FFsFabricReturnReceiveViewModel ffsfabricReturnReceiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.CREATED_AT = DateTime.Now;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.CREATED_BY = user.Id;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.UPDATED_AT = DateTime.Now;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.UPDATED_BY = user.Id;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.RCVDATE = DateTime.Now;

                    var result = await _fabricReturnReceive.InsertByAsync(ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsFabricReturnReceive), $"FFsFabricReturnReceive");

                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(ffsfabricReturnReceiveViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(ffsfabricReturnReceiveViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(ffsfabricReturnReceiveViewModel);
            }
        }





        [HttpGet]
        public async Task<IActionResult> EditFFsFabricReturnReceive(string lbId)
        {
            var ffsfabricReturnReceive = await _fabricReturnReceive.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

            if (ffsfabricReturnReceive != null)
            {
                var ffsfabricReturnReceiveViewModel = await _fabricReturnReceive.GetInitObjByAsync(new FFsFabricReturnReceiveViewModel());
                ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE = ffsfabricReturnReceive;

                ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.EncryptedId = _protector.Protect(ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.RCVID.ToString());
                return View(ffsfabricReturnReceiveViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFFsFabricReturnReceive), $"FFsFabricReturnReceive");
        }

        [HttpPost]
        public async Task<IActionResult> EditFFsFabricReturnReceive(FFsFabricReturnReceiveViewModel ffsfabricReturnReceiveViewModel)
        {
            if (ModelState.IsValid)
            {

                ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.RCVID = int.Parse(_protector.Unprotect(ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.EncryptedId));

                var fHrdLeave = await _fabricReturnReceive.FindByIdAsync(ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.RCVID);
                if (fHrdLeave != null)
                {
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.UPDATED_AT = DateTime.Now;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.CREATED_AT = fHrdLeave.CREATED_AT;
                    ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _fabricReturnReceive.Update(ffsfabricReturnReceiveViewModel.f_FS_FABRIC_RETURN_RECEIVE))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsFabricReturnReceive), $"FFsFabricReturnReceive");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(ffsfabricReturnReceiveViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(ffsfabricReturnReceiveViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(ffsfabricReturnReceiveViewModel);
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetStyleByPi(int pi)
        {
            try
            {
                return Ok(await _fabricReturnReceive.GetStyleByPi(pi));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


    }
}
