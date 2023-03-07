using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class BasicBuyerBankMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_BUYER_BANK_MASTER _bAsBuyerBankMaster;

        public BasicBuyerBankMasterController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              IBAS_BUYER_BANK_MASTER bAS_BUYER_BANK_MASTER)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsBuyerBankMaster = bAS_BUYER_BANK_MASTER;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsBuyerBankNameInUse(string bankName)
        {
            var bank = _bAsBuyerBankMaster.FindByBuyerBankName(bankName);
            return bank ? Json(true) : Json($"Bank Name [ {bankName} ] is already in use");
        }

        [HttpPost]
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

                var data = await _bAsBuyerBankMaster.GetAll();
                
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.PARTY_BANK.ToUpper().Contains(searchValue)
                                           || (m.ADDRESS != null && m.ADDRESS.ToUpper().Contains(searchValue))
                                           || (m.BRANCH != null && m.BRANCH.ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.BANK_ID.ToString());
                }

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

        [HttpGet]
        public IActionResult GetBasBuyerBanksWithPaged()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBasBuyerBank(string bankId)
        {
            try
            {
                var buyerBank = await _bAsBuyerBankMaster.FindByIdAsync(int.Parse(_protector.Unprotect(bankId)));

                if (buyerBank != null)
                {
                    var result = await _bAsBuyerBankMaster.Delete(buyerBank);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Buyer Bank.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
                    }

                    TempData["message"] = "Failed to Delete Buyer Bank.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
                }

                TempData["message"] = "Buyer Bank Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Buyer Bank.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
            }
        }
        
        [HttpGet]
        public IActionResult CreateBasBuyerBank()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasBuyerBank(BAS_BUYER_BANK_MASTER bank)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bAsBuyerBankMaster.InsertByAsync(bank);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Buyer Bank.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
                    }

                    TempData["message"] = "Failed to Add Buyer Bank.";
                    TempData["type"] = "error";
                    return View(bank);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(bank);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Buyer Bank.";
                TempData["type"] = "error";
                return View(bank);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasBuyerBank(string bankId)
        {
            try
            {
                var result = await _bAsBuyerBankMaster.FindByIdAsync(int.Parse(_protector.Unprotect(bankId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.BANK_ID.ToString());
                    return View(result);
                }

                TempData["message"] = "Failed to Retrieve Buyer Bank Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Buyer Bank Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditBasBuyerBank(BAS_BUYER_BANK_MASTER bank)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var bankResult = await _bAsBuyerBankMaster.FindByIdAsync(int.Parse(_protector.Unprotect(bank.EncryptedId)));

                    if (bankResult != null)
                    {
                        var result = await _bAsBuyerBankMaster.Update(bank);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Buyer Bank Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasBuyerBanksWithPaged", $"BasicBuyerBankMaster");
                        }

                        TempData["message"] = "Failed to Update Buyer Bank Information.";
                        TempData["type"] = "error";
                        return View(bank);
                    }

                    TempData["message"] = "Failed to Update Buyer Bank Information.";
                    TempData["type"] = "error";
                    return View(bank);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(bank);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Buyer Bank Information.";
                TempData["type"] = "error";
                return View(bank);
            }
        }
    }
}