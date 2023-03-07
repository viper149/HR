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
    public class FFsWastagePartyController : Controller
    {

        private readonly IF_FS_WASTAGE_PARTY _fFsWastageParty;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = "FS Wastage Party";

        public FFsWastagePartyController(IDataProtectionProvider dataProtectionProvider,
            IF_FS_WASTAGE_PARTY fFsWastageParty,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _fFsWastageParty = fFsWastageParty;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> GetFFsWastageParty()
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

            var data = await _fFsWastageParty.GetAllFFsWastagePartyAsync();  // get all data with protector id


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
        public async Task<IActionResult> CreateFFsWastageParty()
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
        public async Task<IActionResult> CreateFFsWastageParty(F_FS_WASTAGE_PARTY fFsWastageParty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fFsWastageParty.InsertByAsync(fFsWastageParty);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageParty), $"FFsWastageParty");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fFsWastageParty);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fFsWastageParty);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fFsWastageParty);
            }
        }



        // edit data


        [HttpGet]
        public async Task<IActionResult> EditFsWastege(string lbId)
        {
            try
            {
                var f_WASTE = await _fFsWastageParty.GetInitObjByAsync(new F_FS_WASTAGE_PARTY());

                f_WASTE = await _fFsWastageParty.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

                f_WASTE.EncryptedId = lbId;

                f_WASTE = await _fFsWastageParty.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

                f_WASTE.EncryptedId = lbId;



                return View(f_WASTE);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFsWastege(F_FS_WASTAGE_PARTY f_FS_WASTAGE_PARTY)
        {
            if (ModelState.IsValid)
            {

                f_FS_WASTAGE_PARTY.PID = int.Parse(_protector.Unprotect(f_FS_WASTAGE_PARTY.EncryptedId));

                var fHrdLeave = await _fFsWastageParty.FindByIdAsync(f_FS_WASTAGE_PARTY.PID);
                if (fHrdLeave != null)
                {
                    f_FS_WASTAGE_PARTY.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    f_FS_WASTAGE_PARTY.UPDATED_AT = DateTime.Now;
                    f_FS_WASTAGE_PARTY.CREATED_AT = fHrdLeave.CREATED_AT;
                    f_FS_WASTAGE_PARTY.CREATED_BY = fHrdLeave.CREATED_BY;
                    

                    if (await _fFsWastageParty.Update(f_FS_WASTAGE_PARTY))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageParty), $"FFsWastageParty");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(f_FS_WASTAGE_PARTY);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(f_FS_WASTAGE_PARTY);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(f_FS_WASTAGE_PARTY);
        }



        [HttpGet]
        public async Task<IActionResult> DeleteWastageParty(string id)
        {
            try
            {
                var waste = await _fFsWastageParty.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (waste != null)
                {
                    var result = await _fFsWastageParty.Delete(waste);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFFsWastageParty), $"FFsWastageParty");
                    }
                    TempData["message"] = "Failed to Delete";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetFFsWastageParty), $"FFsWastageParty");
                }
                TempData["message"] = "Sample Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFFsWastageParty), $"FFsWastageParty");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Delete";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFFsWastageParty), $"FFsWastageParty");
            }
        }



        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProductNameInUse(F_FS_WASTAGE_PARTY f_FS_WASTAGE_PARTY)
        {
            var pName = f_FS_WASTAGE_PARTY.PNAME;
            return await _fFsWastageParty.FindByProductName(pName) ? Json(true) : Json($"Product {pName} is already in use");
        }

    }

}
