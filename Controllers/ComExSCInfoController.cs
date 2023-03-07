using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ComExScInfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ICOM_EX_SCINFO _comExScinfo;
        private readonly ICOM_EX_SCDETAILS _comExScdetails;
        private readonly ICOM_EX_FABSTYLE _comExFabstyle;
        private readonly IBAS_BUYERINFO _basBuyerInfo;
        private readonly IBAS_BUYER_BANK_MASTER _basBuyerBankMaster;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComExScInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_EX_SCINFO comExScinfo,
            ICOM_EX_SCDETAILS comExScdetails,
            ICOM_EX_FABSTYLE comExFabstyle,
            IBAS_BUYERINFO basBuyerInfo,
            IBAS_BUYER_BANK_MASTER basBuyerBankMaster,
            UserManager<ApplicationUser> userManager)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _comExScinfo = comExScinfo;
            _comExScdetails = comExScdetails;
            _comExFabstyle = comExFabstyle;
            _basBuyerInfo = basBuyerInfo;
            _basBuyerBankMaster = basBuyerBankMaster;
            _userManager = userManager;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsScNoInUse(ComExScInfoViewModel comExScInfoViewModel)
        {
            var isScNoExists = await _comExScinfo.FindByScNoInUseAsync(comExScInfoViewModel.cOM_EX_SCINFO.SCNO);
            return !isScNoExists ? Json(true) : Json($"Sales Contact No [ {comExScInfoViewModel.cOM_EX_SCINFO.SCNO} ] is already in use");
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
                var data = (List<COM_EX_SCINFO>)await _comExScinfo.GetComExScInfoAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SCNO.ToUpper().Contains(searchValue)
                                           || (m.SCDATE != null && m.SCDATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.SCPERSON != null && m.SCPERSON.ToUpper().Contains(searchValue))
                                           || (m.BCPERSON != null && m.BCPERSON.ToUpper().Contains(searchValue))
                                           || (m.DELDATE != null && m.DELDATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.PAYDATE != null && m.PAYDATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.PAYMODE != null && m.PAYMODE.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize);

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
        public IActionResult GetComExScInfoWithPaged()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateComScInfo()
        {


            var comExScInfoViewModel = new ComExScInfoViewModel
            {
                cOM_EX_SCDETAILS = new COM_EX_SCDETAILS
                {
                    QTY = 0,
                    RATE = 0,
                    AMOUNT = 0
                },
                cOM_EX_SCINFO = new COM_EX_SCINFO
                {
                    SCNO = await _comExScinfo.GetLastSCNoAsync(),
                    SCDATE = DateTime.Now
                }
            };

            return View(await GetInfo(comExScInfoViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComScInfo(ComExScInfoViewModel comExScInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isComExInfo = await _comExScinfo.GetInsertedObjByAsync(comExScInfoViewModel.cOM_EX_SCINFO);

                    if (isComExInfo != null)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        foreach (var item in comExScInfoViewModel.cOM_EX_SCDETAILs)
                        {
                            var sc = new COM_EX_SCDETAILS();
                            sc.SCNO = isComExInfo.SCID;
                            sc.TRNSDATE = DateTime.Now;
                            sc.STYLEID = item.STYLEID;
                            sc.UNIT = item.UNIT;
                            sc.QTY = item.QTY;
                            sc.RATE = item.RATE;
                            sc.AMOUNT = item.AMOUNT;
                            sc.REMARKS = item.REMARKS;
                            await _comExScdetails.InsertByAsync(sc);
                        }
                        TempData["message"] = "Successfully added Sales Contact.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Sales Contact.";
                        TempData["type"] = "error";
                        ComExScInfoViewModel result = await GetInfo(comExScInfoViewModel);
                        return View(result);
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input. Please Try Again.";
                    TempData["type"] = "error";
                    ComExScInfoViewModel result = await GetInfo(comExScInfoViewModel);
                    return View(result);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Sales Contact.";
                TempData["type"] = "error";
                ComExScInfoViewModel result = await GetInfo(comExScInfoViewModel);
                return View(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteScInfo(string scNo)
        {
            try
            {
                var scInfo = await _comExScinfo.GetComExScInfoList(int.Parse(_protector.Unprotect(scNo)));

                if (scInfo != null)
                {
                    var scDetailsList = await _comExScdetails.GetComExScDetailsList(scInfo.SCID);
                    var comExScDetails = scDetailsList.ToList();

                    if (comExScDetails.Count() != 0)
                    {
                        await _comExScdetails.DeleteRange(comExScDetails);
                    }

                    var result = await _comExScinfo.Delete(scInfo);

                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Sales Contact.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Delete Sales Contact.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                    }
                }
                else
                {
                    TempData["message"] = "Sales Contact Not Found!.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Sales Contact.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsScInfo(string scNo)
        {
            try
            {
                var decryptedId = _protector.Unprotect(scNo);
                var comExScInfoViewModel = new ComExScInfoViewModel();
                var scInfo = await _comExScinfo.GetComExScInfoList(int.Parse(decryptedId));

                if (scInfo != null)
                {
                    var accExDoDetailsList = await _comExScdetails.GetComExScDetailsList(int.Parse(decryptedId));

                    comExScInfoViewModel.cOM_EX_SCINFO = scInfo;
                    comExScInfoViewModel.cOM_EX_SCDETAILs = accExDoDetailsList.ToList();

                    foreach (var item in comExScInfoViewModel.cOM_EX_SCDETAILs)
                    {
                        item.STYLE = await _comExFabstyle.FindByIdForStyleNameAsync(item.STYLEID);
                    }

                    comExScInfoViewModel.cOM_EX_SCINFO.EncryptedId = _protector.Protect(decryptedId);

                    return View(comExScInfoViewModel);
                }

                TempData["message"] = "Sales Contact Information Not Found.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Sales Contact.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditScInfo(string scNo)
        {
            try
            {
                var decryptedId = _protector.Unprotect(scNo);

                var comExScInfoViewModel = new ComExScInfoViewModel();

                var scInfo = await _comExScinfo.GetComExScInfoByScNo(int.Parse(decryptedId));

                if (scInfo != null)
                {
                    comExScInfoViewModel = await GetInfo(comExScInfoViewModel);
                    var accExScDetailsList = await _comExScdetails.GetComExScDetailsList(int.Parse(decryptedId));

                    scInfo.EncryptedId = _protector.Protect(scInfo.SCNO);

                    foreach (var item in accExScDetailsList)
                    {
                        item.STYLE = await _comExFabstyle.FindByIdForStyleNameAsync(item.STYLEID);
                    }

                    comExScInfoViewModel.cOM_EX_SCINFO = scInfo;
                    comExScInfoViewModel.cOM_EX_SCDETAILSList = accExScDetailsList.ToList();

                    return View(comExScInfoViewModel);
                }
                else
                {
                    TempData["message"] = "Sales Contact Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Sales Contact.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditScInfo(ComExScInfoViewModel comExScInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (comExScInfoViewModel.cOM_EX_SCINFO.SCNO == _protector.Unprotect(comExScInfoViewModel.cOM_EX_SCINFO.EncryptedId))
                    {
                        comExScInfoViewModel.cOM_EX_SCINFO.SCNO = _protector.Unprotect(comExScInfoViewModel.cOM_EX_SCINFO.EncryptedId);

                        var isScInfoUpdated = await _comExScinfo.Update(comExScInfoViewModel.cOM_EX_SCINFO);

                        if (isScInfoUpdated)
                        {
                            foreach (var item in comExScInfoViewModel.cOM_EX_SCDETAILs)
                            {
                                var isExistsScDetails = await _comExScdetails.FindScDetailsByScNoAndStyleIdAsync(comExScInfoViewModel.cOM_EX_SCINFO.SCID, item.STYLEID);
                                if (!isExistsScDetails.Any())
                                {
                                    var scInfo = new COM_EX_SCDETAILS()
                                    {
                                        SCNO = comExScInfoViewModel.cOM_EX_SCINFO.SCID,
                                        TRNSDATE = DateTime.Now,
                                        UNIT = item.UNIT,
                                        STYLEID = item.STYLEID,
                                        QTY = item.QTY,
                                        RATE = item.RATE,
                                        AMOUNT = item.AMOUNT,
                                        REMARKS = item.REMARKS
                                    };
                                    await _comExScdetails.InsertByAsync(scInfo);
                                }
                            }

                            TempData["message"] = "Successfully Updated Sales Contact.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Sales Contact.";
                            TempData["type"] = "error";
                            return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Invalid Contact No.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetComExScInfoWithPaged", $"ComExScInfo");
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input. Please Try Again.";
                    TempData["type"] = "error";
                    var accExScDetailsList = await _comExScdetails.GetComExScDetailsList(comExScInfoViewModel.cOM_EX_SCINFO.SCID);
                    comExScInfoViewModel.cOM_EX_SCDETAILSList = accExScDetailsList.ToList();
                    var result = await GetInfo(comExScInfoViewModel);
                    return View(result);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Sales Contact.";
                TempData["type"] = "error";
                var accExScDetailsList = await _comExScdetails.GetComExScDetailsList(comExScInfoViewModel.cOM_EX_SCINFO.SCID);
                comExScInfoViewModel.cOM_EX_SCDETAILSList = accExScDetailsList.ToList();
                var result = await GetInfo(comExScInfoViewModel);
                return View(result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddScDetails(ComExScInfoViewModel comExScInfoViewModel)
        {
            try
            {
                var isExistsScDetails = await _comExScdetails.FindScDetailsByScNoAndStyleIdAsync(comExScInfoViewModel.cOM_EX_SCINFO.SCID, comExScInfoViewModel.cOM_EX_SCDETAILS.STYLEID);
                if (!isExistsScDetails.Any())
                {
                    var result = comExScInfoViewModel.cOM_EX_SCDETAILs.Where(e => e.STYLEID == comExScInfoViewModel.cOM_EX_SCDETAILS.STYLEID && e.SCNO == comExScInfoViewModel.cOM_EX_SCINFO.SCID).Select(e => e.STYLEID);
                    if (!result.Any())
                    {
                        comExScInfoViewModel.cOM_EX_SCDETAILS.STYLE = await _comExFabstyle.FindByIdForStyleNameAsync(comExScInfoViewModel.cOM_EX_SCDETAILS.STYLEID);
                        comExScInfoViewModel.cOM_EX_SCDETAILS.SCNO = comExScInfoViewModel.cOM_EX_SCINFO.SCID;
                        comExScInfoViewModel.cOM_EX_SCDETAILs.Add(comExScInfoViewModel.cOM_EX_SCDETAILS);
                    }
                    return PartialView($"ComScDetailsTable", comExScInfoViewModel);
                }
                else
                {
                    return PartialView($"ComScDetailsTable", comExScInfoViewModel);
                }

            }
            catch (Exception)
            {
                return PartialView($"ComScDetailsTable", comExScInfoViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveScFromList(ComExScInfoViewModel comExScInfoViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            comExScInfoViewModel.cOM_EX_SCDETAILs.RemoveAt(Int32.Parse(removeIndexValue));
            return PartialView($"ComScDetailsTable", comExScInfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetScDetailsList(int scNo)
        {
            try
            {
                var comExScDetailsList = await _comExScdetails.GetComExScDetailsList(scNo);
                var comExDetails = comExScDetailsList.ToList();
                foreach (var item in comExDetails)
                {
                    item.STYLE = await _comExFabstyle.FindByIdForStyleNameAsync(item.STYLEID);
                }
                return PartialView($"ScDetailsPreviousList", comExDetails);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveExistingScDetails(int scNo, int styleId)
        {
            try
            {
                var ifExistScinfo = await _comExScdetails.FindScDetailsByScNoAndStyleIdAsync(scNo, styleId);

                if (ifExistScinfo != null)
                {
                    await _comExScdetails.DeleteRange(ifExistScinfo);
                }

                var comExScDetailsList = await _comExScdetails.GetComExScDetailsList(scNo);
                var comExDetails = comExScDetailsList.ToList();

                foreach (var item in comExDetails)
                {
                    item.STYLE = await _comExFabstyle.FindByIdForStyleNameAsync(item.STYLEID);
                }

                return PartialView($"ScDetailsPreviousList", comExDetails);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<ComExScInfoViewModel> GetInfo(ComExScInfoViewModel comExScInfoViewModel)
        {
            var buyerList = await _basBuyerInfo.GetAll();
            var buyerBankList = await _basBuyerBankMaster.GetAll();
            var fabricStyleList = await _comExFabstyle.GetAll();

            comExScInfoViewModel.bAS_BUYERINFOs = buyerList.OrderBy(e => e.BUYER_NAME).ToList();
            comExScInfoViewModel.bAS_BUYER_BANK_MASTERs = buyerBankList.OrderBy(e => e.PARTY_BANK).ToList();
            comExScInfoViewModel.cOM_EX_FABSTYLEs = fabricStyleList.OrderBy(e => e.STYLENAME).ToList();

            return comExScInfoViewModel;
        }

    }
}