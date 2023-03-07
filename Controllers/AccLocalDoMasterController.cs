using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class AccLocalDoMasterController : Controller
    {
        private readonly IACC_LOCAL_DOMASTER _accLocalDomaster;
        private readonly IACC_LOCAL_DODETAILS _accLocalDodetails;
        private readonly ICOM_EX_SCINFO _comExScinfo;
        private readonly ICOM_EX_FABSTYLE _comExFabstyle;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public AccLocalDoMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IACC_LOCAL_DOMASTER accLocalDomaster,
            IACC_LOCAL_DODETAILS accLocalDodetails,
            ICOM_EX_SCINFO comExScinfo,
            ICOM_EX_FABSTYLE comExFabstyle,
            UserManager<ApplicationUser> userManager
        )
        {
            _accLocalDomaster = accLocalDomaster;
            _accLocalDodetails = accLocalDodetails;
            _comExScinfo = comExScinfo;
            _comExFabstyle = comExFabstyle;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
        [Route("AccountDO/Local/GetStyles/{lcId?}")]
        public async Task<IActionResult> GetStyles(int lcId)
        {
            return Ok(await _accLocalDomaster.GetGetStylesLcWiseByAsync(lcId));
        }

        [AcceptVerbs("Post")]
        [Route("AccountDO/Local/GetOtherInfo")]
        public async Task<IActionResult> GetOtherInfo(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            return Ok(await _accLocalDomaster.GetOtherInfoByAsync(accLocalDoMasterViewModel));
        }


        [AcceptVerbs("Get", "Post")]
        [Route("AccountDO/Local/GetCommercialExportLC/{search?}/{page?}")]
        public async Task<IActionResult> GetCommercialExportLC(AccLocalDoMasterViewModel accLocalDoMasterViewModel, string search, int page)
        {
            return Ok(await _accLocalDomaster.GetCommercialExportLCByAsync(accLocalDoMasterViewModel, search, page));
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AccountDO/Local/GetLocalSaleOrders/{search?}/{page?}")]
        public async Task<IActionResult> GetLocalSaleOrders(string search, int page)
        {
            return Ok(await _accLocalDomaster.GetLocalSaleOrdersByAsync(search, page));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsDoNoInUse(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            return await _accLocalDomaster.FindByDoNoInUseAsync(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.DONO) ? Json($"DO No [ {accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.DONO} ] is already in use") : Json(true);
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

                var data = await _accLocalDomaster.GetAccDoMasterAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => !string.IsNullOrEmpty(m.DONO) && m.DONO.ToUpper().Contains(searchValue)
                                           || m.DODATE != null && m.DODATE.ToString().Contains(searchValue)
                                           || m.DOEX != null && m.DOEX.ToString().Contains(searchValue)
                                           || m.ComExScinfo != null && !string.IsNullOrEmpty(m.ComExScinfo.SCNO) && m.ComExScinfo.SCNO.ToString().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.AUDITBY) && m.AUDITBY.ToUpper().Contains(searchValue)
                                           || m.AUDITON != null && m.AUDITON.ToString().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.COMMENTS) && m.COMMENTS.ToUpper().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.REMARKS) && m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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

        [HttpGet]
        [Route("AccountDO/Local/GetAll")]
        public IActionResult GetAccDoMasterWithPaged()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccDoMaster()
        {
            var accLocalDoMasterViewModel = new AccLocalDoMasterViewModel
            {
                ACC_LOCAL_DOMASTER = new ACC_LOCAL_DOMASTER { DONO = await _accLocalDomaster.GetLastDoNoAsync(), DODATE = DateTime.Now }
            };

            return View(await _accLocalDomaster.GetInitObjectsByAsync(accLocalDoMasterViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccDoMaster(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.TRNSDATE = DateTime.Now;
                accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.USRID = user.Id;

                if (!string.IsNullOrEmpty(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.COMMENTS))
                {
                    accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.AUDITBY = user.Id;
                    accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.AUDITON = DateTime.Now;
                }

                var accLocalDomaster = await _accLocalDomaster.GetInsertedObjByAsync(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER);

                if (accLocalDomaster.TRNSID > 0)
                {
                    foreach (var item in accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs)
                    {
                        item.DONO = accLocalDomaster.TRNSID;
                        item.TRNSDATE = DateTime.Now;
                        item.USRID = user.Id;

                        switch (accLocalDomaster.IS_COMPENSATION)
                        {
                            case true:
                                item.PI_TRNSID = item.STYLEID;
                                item.STYLEID = null;
                                break;
                        }
                    }

                    await _accLocalDodetails.InsertRangeByAsync(accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs);

                    TempData["message"] = "Successfully added DO Information.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetAccDoMasterWithPaged), $"AccLocalDoMaster");
                }
                else
                {
                    TempData["message"] = "Failed to Add DO Information";
                    TempData["type"] = "error";
                    return View(await _accLocalDomaster.GetInitObjectsByAsync(accLocalDoMasterViewModel));
                }
            }
            else
            {
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _accLocalDomaster.GetInitObjectsByAsync(accLocalDoMasterViewModel));
            }
        }

        [HttpGet]
        [Route("AccountDO/Local/Details/{transId?}")]
        public async Task<IActionResult> DetailsAccDoMaster(string transId)
        {
            var findByIdIncludeAllByAsync = await _accLocalDomaster.FindByIdIncludeAllByAsync(int.Parse(_protector.Unprotect(transId)));

            if (findByIdIncludeAllByAsync.ACC_LOCAL_DOMASTER.TRNSID > 0)
            {
                return View(await _accLocalDomaster.GetInitObjectsByAsync(findByIdIncludeAllByAsync));
            }

            TempData["message"] = "DO Information Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetAccDoMasterWithPaged), $"AccLocalDoMaster");
        }

        [HttpGet]
        public async Task<IActionResult> EditAccDoMaster(string transId)
        {
            var findByIdIncludeAllByAsync = await _accLocalDomaster.FindByIdIncludeAllByAsync(int.Parse(_protector.Unprotect(transId)));

            if (findByIdIncludeAllByAsync.ACC_LOCAL_DOMASTER != null)
            {
                return View(await _accLocalDomaster.GetInitObjectsByAsync(findByIdIncludeAllByAsync));
            }

            TempData["message"] = "DO Information Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetAccDoMasterWithPaged), $"AccLocalDoMaster");
        }

        [HttpPost]
        public async Task<IActionResult> EditAccDoMaster(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                var accLocalDomaster = await _accLocalDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.EncryptedId)));

                if (accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.COMMENTS != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.AUDITBY = user.Id;
                    accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.AUDITON = DateTime.Now;
                }

                if (await _accLocalDomaster.Update(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER))
                {
                    foreach (var item in accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs)
                    {
                        var isExistsDoDetails = await _accLocalDodetails.FindDoDetailsBydoNoAndStyleIdAsync(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.TRNSID, (int)item.STYLEID);
                        if (!isExistsDoDetails.Any())
                        {
                            var doInfo = new ACC_LOCAL_DODETAILS()
                            {
                                DONO = accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.TRNSID,
                                TRNSDATE = item.TRNSDATE,
                                STYLEID = item.STYLEID,
                                QTY = item.QTY,
                                RATE = item.RATE,
                                AMOUNT = item.AMOUNT,
                                REMARKS = item.REMARKS
                            };

                            await _accLocalDodetails.InsertByAsync(doInfo);
                        }
                    }

                    TempData["message"] = "Successfully Updated DO Information.";
                    TempData["type"] = "success";
                    return RedirectToAction($"GetAccDoMasterWithPaged", $"AccLocalDoMaster");
                }

                TempData["message"] = "Failed to Update DO Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetAccDoMasterWithPaged", $"AccLocalDoMaster");
            }

            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            var accLocalDoDetailsList = await _accLocalDodetails.FindDoDetailsListByDoNoAsync(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.TRNSID);
            accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs = accLocalDoDetailsList.ToList();
            return View(await _accLocalDomaster.GetInitObjectsByAsync(accLocalDoMasterViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> GetDoDetailsList(int doNo)
        {
            try
            {
                return PartialView($"DoDetailsPreviousList", await _accLocalDodetails.FindDoDetailsListByDoNoAsync(doNo));
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveExistingDoDetails(int doNo, int styleId)
        {
            try
            {
                var ifExistDOinfo = await _accLocalDodetails.FindDoDetailsBydoNoAndStyleIdAsync(doNo, styleId);

                if (ifExistDOinfo != null)
                {
                    await _accLocalDodetails.DeleteRange(ifExistDOinfo);
                }

                return PartialView($"DoDetailsPreviousList", await _accLocalDodetails.FindDoDetailsListByDoNoAsync(doNo));

            }
            catch (Exception)
            {
                return null;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetScInfo(int scId)
        {
            try
            {
                return Ok(await _comExScinfo.GetComExScInfoList(scId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoDetails(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            try
            {
                ModelState.Clear();

                var isExistsDoDetails = await _accLocalDodetails.FindDoDetailsBydoNoAndStyleIdAsync(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.TRNSID, accLocalDoMasterViewModel.ACC_LOCAL_DODETAILS.STYLEID ?? 0);

                if (!isExistsDoDetails.Any())
                {
                    if (accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs.All(e => e.STYLEID != accLocalDoMasterViewModel.ACC_LOCAL_DODETAILS.STYLEID))
                    {
                        accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs.Add(accLocalDoMasterViewModel.ACC_LOCAL_DODETAILS);
                    }

                    return PartialView($"AccDoDetailsTable", await _accLocalDomaster.GetInitObjectsForDetailsTable(accLocalDoMasterViewModel));
                }

                return PartialView($"AccDoDetailsTable", await _accLocalDomaster.GetInitObjectsForDetailsTable(accLocalDoMasterViewModel));

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveDoFromList(AccLocalDoMasterViewModel accLocalDoMasterViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs.RemoveAt(int.Parse(removeIndexValue));
                return PartialView($"AccDoDetailsTable", accLocalDoMasterViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PartialView($"AccDoDetailsTable", accLocalDoMasterViewModel);
            }
        }

        public async Task<AccLocalDoMasterViewModel> GetCreateInfo(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            try
            {
                accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER = new ACC_LOCAL_DOMASTER
                {
                    DONO = await _accLocalDomaster.GetLastDoNoAsync(),
                    DODATE = DateTime.Now
                };
                return accLocalDoMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("Account-Finance/LocalDO/After-Audit/GetReports/{transId?}")]
        public async Task<IActionResult> RptDoAudit(string transId)
        {
            var doNo = 0;

            if (transId != null)
                doNo = await Task.Run(() => _accLocalDomaster.FindByIdAsync(int.Parse(_protector.Unprotect(transId))).Result.TRNSID);
            return View(model: doNo);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("AccountDO/Local/Delete/{doId?}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteAccLocalDoMaster(string doId)
        {
            var findByIdIncludeAllForDeleteAsync = await _accLocalDomaster.FindByIdIncludeAllForDeleteAsync(int.Parse(_protector.Unprotect(doId)));

            if (findByIdIncludeAllForDeleteAsync != null)
            {
                await _accLocalDodetails.DeleteRange(findByIdIncludeAllForDeleteAsync.ACC_LOCAL_DODETAILS);
                await _accLocalDomaster.Delete(findByIdIncludeAllForDeleteAsync);
            }

            return RedirectToAction(nameof(GetAccDoMasterWithPaged), "AccLocalDoMaster");
        } 

    }
}