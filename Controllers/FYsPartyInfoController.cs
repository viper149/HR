using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FYsPartyInfoController : Controller
    {
        private readonly IF_YS_PARTY_INFO _f_YS_PARTY_INFO;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "F Ys Party Info";

        public FYsPartyInfoController(IDataProtectionProvider dataProtectionProvider,
            IF_YS_PARTY_INFO f_YS_PARTY_INFO,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _f_YS_PARTY_INFO = f_YS_PARTY_INFO;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> GetFYsPartyInfo()
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

            var data = await _f_YS_PARTY_INFO.GetAllFYsPartyInfoAsync();  // get all data with protector id


            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.ADDRESS.ToUpper().Contains(searchValue)
                                 
                                    || m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
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
        public async Task<IActionResult> CreateFYsPartyInfo()
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
        public async Task<IActionResult> CreateFYsPartyInfo(F_YS_PARTY_INFO f_YS_PARTY_INFO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _f_YS_PARTY_INFO.InsertByAsync(f_YS_PARTY_INFO);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFYsPartyInfo), $"FYsPartyInfo");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(f_YS_PARTY_INFO);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(f_YS_PARTY_INFO);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(f_YS_PARTY_INFO);
            }
        }



        // edit data


        [HttpGet]
        public async Task<IActionResult> EditFYsPartyInfo(string lbId)
        {
            try
            {
                var f_YS_PARTY_INFO = await _f_YS_PARTY_INFO.GetInitObjByAsync(new F_YS_PARTY_INFO());

                f_YS_PARTY_INFO = await _f_YS_PARTY_INFO.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

                f_YS_PARTY_INFO.EncryptedId = lbId;

                //f_YS_PARTY_INFO = await _f_YS_PARTY_INFO.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

                //f_YS_PARTY_INFO.EncryptedId = lbId;



                return View(f_YS_PARTY_INFO);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFYsPartyInfo(F_YS_PARTY_INFO f_YS_PARTY_INFO)
        {
            if (ModelState.IsValid)
            {

                f_YS_PARTY_INFO.PARTY_ID = int.Parse(_protector.Unprotect(f_YS_PARTY_INFO.EncryptedId));

                var fHrdLeave = await _f_YS_PARTY_INFO.FindByIdAsync(f_YS_PARTY_INFO.PARTY_ID);
                if (fHrdLeave != null)
                {
                    if (await _f_YS_PARTY_INFO.Update(f_YS_PARTY_INFO))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFYsPartyInfo), $"FYsPartyInfo");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(f_YS_PARTY_INFO);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(f_YS_PARTY_INFO);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(f_YS_PARTY_INFO);
        }



        [HttpGet]
        public async Task<IActionResult> DeleteFYsPartyInfo(string id)
        {
            try
            {
                var f_YS_PARTY_INFO = await _f_YS_PARTY_INFO.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (f_YS_PARTY_INFO != null)
                {
                    var result = await _f_YS_PARTY_INFO.Delete(f_YS_PARTY_INFO);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFYsPartyInfo), $"FYsPartyInfo");
                    }
                    TempData["message"] = "Failed to Delete";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFYsPartyInfo), $"FYsPartyInfo");
                }
                TempData["message"] = "Sample Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFYsPartyInfo), $"FYsPartyInfo");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Delete";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFYsPartyInfo), $"FYsPartyInfo");
            }
        }
    }
}
