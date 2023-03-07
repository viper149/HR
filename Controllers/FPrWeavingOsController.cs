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
    public class FPrWeavingOsController : Controller
    {
        private readonly IF_PR_WEAVING_OS _fPrWeavingOs;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public FPrWeavingOsController(IDataProtectionProvider dataProtectionProvider,
            IF_PR_WEAVING_OS fPrWeavingOs,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fPrWeavingOs = fPrWeavingOs;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);

        }

        [HttpGet]
        public IActionResult GetFPrWeavingOs()
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

            var data = await _fPrWeavingOs.GetAllFPrWeavingOs();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m =>  m.OSID.ToString().ToUpper().Contains(searchValue)
                                       || m.OSDATE != null && m.OSDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.OS != null && m.OS.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)
                                       || m.LOT.LOTNO != null && m.LOT.LOTNO.ToString().ToUpper().Contains(searchValue)
                                       || m.COUNT.RND_COUNTNAME != null && m.COUNT.RND_COUNTNAME.ToUpper().Contains(searchValue)
                                      
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
        public async Task<IActionResult> CreateFPrWeavingOs()
        {

            try
            {
                var fPrWeavingOs =await _fPrWeavingOs.GetInitObjByAsync(new FPrWeavingOsViewModel());

                return View(fPrWeavingOs);
            }
            catch (Exception e)
            {
                return View($"Error");
            }

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> CreateFPrWeavingOs(FPrWeavingOsViewModel fPrWeavingOsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.CREATED_AT = DateTime.Now;
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.CREATED_BY = user.Id;
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.UPDATED_AT = DateTime.Now;
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.UPDATED_BY = user.Id;
                   
                    var result = await _fPrWeavingOs.InsertByAsync(fPrWeavingOsViewModel.f_PR_WEAVING_OS);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(CreateFPrWeavingOs), $"FPrWeavingOs");

                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fPrWeavingOsViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fPrWeavingOsViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fPrWeavingOsViewModel);
            }
        }



        [HttpGet]

        public async Task<IActionResult> EditFPrWeavingOs(string lbId)
        {
           int decrypted = int.Parse(_protector.Unprotect(lbId));

            var fPrWeavingOs = await _fPrWeavingOs.FindByIdAsync(decrypted);

            if(fPrWeavingOs != null)
            {
                var fPrWeavingOsViewModel = await _fPrWeavingOs.GetInitObjByAsync(new FPrWeavingOsViewModel());

                fPrWeavingOsViewModel.f_PR_WEAVING_OS = fPrWeavingOs;
                fPrWeavingOsViewModel.f_PR_WEAVING_OS.EncryptedId = _protector.Protect(fPrWeavingOsViewModel.f_PR_WEAVING_OS.OSID.ToString());

                return View(fPrWeavingOsViewModel);
            }

            TempData["message"] = $"Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFPrWeavingOs), $"FPrWeavingOs");


        }

        [HttpPost]
        public async Task<IActionResult> EditFPrWeavingOs(FPrWeavingOsViewModel fPrWeavingOsViewModel)
        {
            if (ModelState.IsValid)
            {

                fPrWeavingOsViewModel.f_PR_WEAVING_OS.OSID = int.Parse(_protector.Unprotect(fPrWeavingOsViewModel.f_PR_WEAVING_OS.EncryptedId));

                var fHrdLeave = await _fPrWeavingOs.FindByIdAsync(fPrWeavingOsViewModel.f_PR_WEAVING_OS.OSID);
                if (fHrdLeave != null)
                {
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.UPDATED_AT = DateTime.Now;
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.CREATED_AT = fHrdLeave.CREATED_AT;
                    fPrWeavingOsViewModel.f_PR_WEAVING_OS.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _fPrWeavingOs.Update(fPrWeavingOsViewModel.f_PR_WEAVING_OS))
                    {
                        TempData["message"] = $"Successfully Updated.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFPrWeavingOs), $"FPrWeavingOs");
                    }
                    TempData["message"] = $"Failed to Update ";
                    TempData["type"] = "error";
                    return View(fPrWeavingOsViewModel);
                }
                TempData["message"] = $"Not Found.";
                TempData["type"] = "error";
                return View(fPrWeavingOsViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(fPrWeavingOsViewModel);
        }




    }
}
