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
    [Route("BeneficiaryBank")]
    public class BasBenBankController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBAS_BEN_BANK_MASTER _bAsBenBankMaster;
        private readonly IDataProtector _protector;
        private const string Title = "Beneficiary Bank Information";

        public BasBenBankController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IBAS_BEN_BANK_MASTER bAsBenBankMaster)
        {
            _userManager = userManager;
            _bAsBenBankMaster = bAsBenBankMaster;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetBasBenBank()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateBasBenBank()
        {
            ViewData["Title"] = Title;
            return View(new BasBenBankViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateBasBenBank(BasBenBankViewModel basBenBankViewModel)
        {
            if (ModelState.IsValid)
            {
                basBenBankViewModel.BasBenBankMaster.CREATED_AT = basBenBankViewModel.BasBenBankMaster.UPDATED_AT = DateTime.Now;
                basBenBankViewModel.BasBenBankMaster.CREATED_BY = basBenBankViewModel.BasBenBankMaster.UPDATED_BY =
                    (await _userManager.GetUserAsync(User)).Id;

                if (await _bAsBenBankMaster.InsertByAsync(basBenBankViewModel.BasBenBankMaster))
                {
                    TempData["message"] = $"Successfully Added {Title}";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetBasBenBank));
                }

                TempData["message"] = $"Failed to Add {Title}";
                TempData["type"] = "error";
                return View(basBenBankViewModel);
            }

            TempData["message"] = $"Please Enter Valid {Title}";
            TempData["type"] = "error";
            return View(basBenBankViewModel);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> EditBasBenBank(string id)
        {
            var fBasHrdOutReasonViewModel = new BasBenBankViewModel
            {
                BasBenBankMaster = await _bAsBenBankMaster.FindByIdAsync(int.Parse(_protector.Unprotect(id)))
            };

            fBasHrdOutReasonViewModel.BasBenBankMaster.EncryptedId = id;
            ViewData["Title"] = Title;
            return View(fBasHrdOutReasonViewModel);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditBasBenBank(BasBenBankViewModel basBenBankViewModel)
        {
            if (ModelState.IsValid)
            {
                var basBenBankMaster = await _bAsBenBankMaster.FindByIdAsync(int.Parse(_protector.Unprotect(basBenBankViewModel.BasBenBankMaster.EncryptedId)));
                if (basBenBankMaster != null)
                {
                    basBenBankViewModel.BasBenBankMaster.BANKID = basBenBankMaster.BANKID;
                    basBenBankViewModel.BasBenBankMaster.BEN_BANK = basBenBankMaster.BEN_BANK;
                    basBenBankViewModel.BasBenBankMaster.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    basBenBankViewModel.BasBenBankMaster.UPDATED_AT = DateTime.Now;
                    basBenBankViewModel.BasBenBankMaster.CREATED_AT = basBenBankMaster.CREATED_AT;
                    basBenBankViewModel.BasBenBankMaster.CREATED_BY = basBenBankMaster.CREATED_BY;

                    if (await _bAsBenBankMaster.Update(basBenBankViewModel.BasBenBankMaster))
                    {
                        TempData["message"] = $"Successfully Updated {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetBasBenBank));
                    }
                    TempData["message"] = $"Failed to Update {Title}.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(GetBasBenBank));
                }
                TempData["message"] = $"{Title} Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetBasBenBank));
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(basBenBankViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteBasBenBank(string id)
        {
            var fBasHrdOutReason = await _bAsBenBankMaster.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fBasHrdOutReason != null)
            {
                if (await _bAsBenBankMaster.Delete(fBasHrdOutReason))
                {
                    TempData["message"] = $"Successfully Deleted {Title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetBasBenBank));
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetBasBenBank));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetBasBenBank));
        }

        [HttpPost]
        [Route("GetData")]
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
                var data = await _bAsBenBankMaster.GetAllBasBenBankAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.BEN_BANK != null && m.BEN_BANK.ToUpper().Contains(searchValue))
                                           || (m.ADDRESS != null && m.ADDRESS.ToUpper().Contains(searchValue))
                                           || (m.BRANCH != null && m.BRANCH.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();
                return Json(new
                {
                    draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal,
                    data = finalData
                });
            }
            catch (Exception e)
            {
                return Json(BadRequest(e));
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AlreadyInUse")]
        public async Task<IActionResult> IsBenBankInUse(BasBenBankViewModel basBenBankViewModel)
        {
            var bank = basBenBankViewModel.BasBenBankMaster.BEN_BANK;
            return await _bAsBenBankMaster.FindByBenBankAsync(bank) ? Json(true) : Json($"Beneficiary Bank {bank} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetBenBanks")]
        public async Task<IActionResult> GetAllBenBanks()
        {
            try
            {
                return Ok(await _bAsBenBankMaster.GetAllBenBanksAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}