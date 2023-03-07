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
    public class FGsWastagePartyController : Controller
    {
        private readonly IF_GS_WASTAGE_PARTY _fGsWastageParty;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "General Store Wastage Party";

        public FGsWastagePartyController(IDataProtectionProvider dataProtectionProvider,
            IF_GS_WASTAGE_PARTY fGsWastageParty,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fGsWastageParty = fGsWastageParty;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> GetFGsWastageParty()
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

            //var data = (List<F_GS_WASTAGE_PARTY>)await _fGsWastageParty.GetAllFGsWastagePartyAsync();
            
            var data = await _fGsWastageParty.GetAllFGsWastagePartyAsync();
            

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.PNAME.ToUpper().Contains(searchValue)
                                    || m.PNAME.ToUpper().Contains(searchValue)
                                    || m.ADDRESS.ToUpper().Contains(searchValue)
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
        public async Task<IActionResult> CreateFGsWastageParty()
        {
            try
            {
                //return View(await _fGsWastageParty.GetInitObjByAsync(new F_GS_WASTAGE_PARTY()));
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateFGsWastageParty(F_GS_WASTAGE_PARTY fGsWastageParty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fGsWastageParty.InsertByAsync(fGsWastageParty);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsWastageParty), $"FGsWastageParty");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fGsWastageParty);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fGsWastageParty);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fGsWastageParty);
            }
        }

        [HttpGet]
        public async Task<IActionResult>EditFGsWastageParty(string wpId){

            try
            {
                var fGsWastageParty = await _fGsWastageParty.GetInitObjByAsync(new F_GS_WASTAGE_PARTY());

                fGsWastageParty = await _fGsWastageParty.FindByIdAsync(int.Parse(_protector.Unprotect(wpId)));

                fGsWastageParty.EncryptedId = wpId;

                fGsWastageParty = await _fGsWastageParty.FindByIdAsync(int.Parse(_protector.Unprotect(wpId)));

                fGsWastageParty.EncryptedId = wpId;



                return View(fGsWastageParty);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFGsWastageParty(F_GS_WASTAGE_PARTY fGsWastageParty)
        {
            if (ModelState.IsValid)
            {

                fGsWastageParty.PID = int.Parse(_protector.Unprotect(fGsWastageParty.EncryptedId));

                var fHrdLeave = await _fGsWastageParty.FindByIdAsync(fGsWastageParty.PID);
                if (fHrdLeave != null)
                {
                    fGsWastageParty.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fGsWastageParty.UPDATED_AT = DateTime.Now;
                    fGsWastageParty.CREATED_AT = fHrdLeave.CREATED_AT;
                    fGsWastageParty.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _fGsWastageParty.Update(fGsWastageParty))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFGsWastageParty), $"FGsWastageParty");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(fGsWastageParty);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(fGsWastageParty);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fGsWastageParty);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProductNameInUse(F_FS_WASTAGE_PARTY fFsWastageParty)
        {
            var pName = fFsWastageParty.PNAME;
            return await _fGsWastageParty.FindByProductName(pName) ? Json(true) : Json($"Product {pName} is already in use");
        }
    }
}
