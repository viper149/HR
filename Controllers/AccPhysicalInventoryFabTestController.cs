using System;
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
    public class AccPhysicalInventoryFabTestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IACC_PHYSICAL_INVENTORY_FAB _accPhysicalInventoryFab;
        private readonly IDataProtector _protector;
        private readonly string title = "Physical Inventory Test (Fabric)";

        public AccPhysicalInventoryFabTestController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IACC_PHYSICAL_INVENTORY_FAB accPhysicalInventoryFab)
        {
            _userManager = userManager;
            _accPhysicalInventoryFab = accPhysicalInventoryFab;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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

            var data = await _accPhysicalInventoryFab.GetAllAsync((await _userManager.GetUserAsync(User)).Id);

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.ROLL_.ROLL_ != null && m.ROLL_.ROLL_.ROLLNO.ToUpper().Contains(searchValue)
                                       || (m.ROLL_.FABCODENavigation != null && m.ROLL_.FABCODENavigation.STYLE_NAME.Contains(searchValue))
                                       || (m.FPI_DATE != null && m.FPI_DATE.ToString().ToUpper().Contains(searchValue))
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
        public IActionResult CreateAccPhysicalInventoryFabTest()
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
        public async Task<IActionResult> CreateAccPhysicalInventoryFabTest(AccPhysicalInventoryFabViewModel accPhysicalInventoryFabViewModel)
        {
            if (ModelState.IsValid)
            {
                accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.FPI_DATE = accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.CREATED_AT = accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.UPDATED_AT = DateTime.Now;
                accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.CREATED_BY = accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;

                accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.ROLL_ID =
                    await _accPhysicalInventoryFab.GetRcvIdByRollNoAsync(accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab.ROLL_NO);

                if (await _accPhysicalInventoryFab.InsertByAsync(accPhysicalInventoryFabViewModel.AccPhysicalInventoryFab))
                {
                    TempData["message"] = $"Successfully Added {title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(CreateAccPhysicalInventoryFabTest), "AccPhysicalInventoryFabTest");

                }

                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(nameof(CreateAccPhysicalInventoryFabTest));
            }

            TempData["message"] = $"Please Enter Valid {title}";
            TempData["type"] = "error";
            return View(nameof(CreateAccPhysicalInventoryFabTest));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAccPhysicalInventoryFab(string id)
        {
            var redirectToActionResult = RedirectToAction(nameof(CreateAccPhysicalInventoryFabTest), $"AccPhysicalInventoryFabTest");
            var accPhysicalInventory = await _accPhysicalInventoryFab.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (accPhysicalInventory != null)
            {
                if (await _accPhysicalInventoryFab.Delete(accPhysicalInventory))
                {
                    TempData["message"] = $"Successfully Deleted {title}.";
                    TempData["type"] = "success";
                    return redirectToActionResult;
                }

                TempData["message"] = $"Failed to Delete {title}.";
                TempData["type"] = "error";
                return redirectToActionResult;
            }

            TempData["message"] = $"{title} Not Found.";
            TempData["type"] = "error";
            return redirectToActionResult;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsReceivedOrDuplicate(string rollNo)
        {
            return await _accPhysicalInventoryFab.FindReceivedByRoll(rollNo) ? await _accPhysicalInventoryFab.FindDuplicateByRoll(rollNo) ? Json(true) : Json($"Roll has already been scanned") : Json($"Roll has not been received yet");
        }
    }
}
