using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com.CnfInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    [Route("Cnf")]
    public class ComImpCnfInfoController : Controller
    {
        private readonly ICOM_IMP_CNFINFO _comImpCnfinfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public ComImpCnfInfoController(ICOM_IMP_CNFINFO comImpCnfinfo,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _comImpCnfinfo = comImpCnfinfo;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{cnfId?}")]
        public async Task<IActionResult> DetailsComImpCnfInfo(string cnfId)
        {
            return View(await _comImpCnfinfo.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(cnfId))));
        }

        [HttpGet]
        [Route("Edit/{cnfId?}")]
        public async Task<IActionResult> EditComImpCnfInfo(string cnfId)
        {
            var findByIdIncludeAllAsync = await _comImpCnfinfo.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(cnfId)));

            if (findByIdIncludeAllAsync != null)
            {
                return View(findByIdIncludeAllAsync);
            }
            else
            {
                TempData["message"] = "CNF Information Could Not Be Found. Please Try Again Later.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetComImpCnfInfo), $"ComImpCnfInfo");
            }
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditComImpCnfInfo(ComImpCnfInfoViewModel cnfInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var comImpCnfinfo = await _comImpCnfinfo.FindByIdAsync(int.Parse(_protector.Unprotect(cnfInfoViewModel.ComImpCnfinfo.EncryptedId)));

                if (comImpCnfinfo != null)
                {
                    cnfInfoViewModel.ComImpCnfinfo.CNFID = comImpCnfinfo.CNFID;
                    cnfInfoViewModel.ComImpCnfinfo.CREATED_AT = comImpCnfinfo.CREATED_AT ?? DateTime.Now;
                    cnfInfoViewModel.ComImpCnfinfo.CREATED_BY = comImpCnfinfo.CREATED_BY ?? user.Id;
                    cnfInfoViewModel.ComImpCnfinfo.UPDATED_AT = DateTime.Now;
                    cnfInfoViewModel.ComImpCnfinfo.UPDATED_BY = user.Id;

                    if (await _comImpCnfinfo.Update(cnfInfoViewModel.ComImpCnfinfo))
                    {
                        TempData["message"] = "CNF Information Successfully Updated.";
                        TempData["type"] = "success";
                    }
                }
                else
                {
                    TempData["message"] = "Failed To Update CNF Information. Please Try Again Later.";
                    TempData["type"] = "error";
                    return View(nameof(EditComImpCnfInfo), cnfInfoViewModel);
                }
            }

            return RedirectToAction(nameof(GetComImpCnfInfo), $"ComImpCnfInfo");
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetComImpCnfInfo()
        {
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateComImpCnfInfo()
        {
            return View();
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateComImpCnfInfo(ComImpCnfInfoViewModel cnfInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                cnfInfoViewModel.ComImpCnfinfo.CREATED_BY = cnfInfoViewModel.ComImpCnfinfo.UPDATED_BY = user.Id;
                cnfInfoViewModel.ComImpCnfinfo.CREATED_AT = cnfInfoViewModel.ComImpCnfinfo.UPDATED_AT = DateTime.Now;
                await _comImpCnfinfo.InsertByAsync(cnfInfoViewModel.ComImpCnfinfo);

                return RedirectToAction(nameof(GetComImpCnfInfo), "ComImpCnfInfo");
            }
            else
            {
                return View(nameof(CreateComImpCnfInfo), cnfInfoViewModel);
            }
        }

        [HttpPost]
        [Route("GetTableData")]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;

                var data = await _comImpCnfinfo.GetAllForDataTables();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CNFNAME.ToString().ToUpper().Contains(searchValue)
                                        || m.C_PERSON != null && m.C_PERSON.ToString().Contains(searchValue)
                                        || m.ADDRESS != null && m.ADDRESS.ToString().ToUpper().Contains(searchValue)
                                        || m.CELL_NO != null && m.CELL_NO.ToUpper().Contains(searchValue)).ToList();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
