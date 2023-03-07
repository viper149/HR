using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class FYsIndentMasterController : Controller
    {
        private readonly IYARNFOR _yarnfor;
        private readonly IF_YS_INDENT_MASTER _fYsIndentMaster;
        private readonly IF_YS_INDENT_DETAILS _fYsIndentDetails;
        private readonly IRND_PURCHASE_REQUISITION_MASTER _rndPurchaseRequisitionMaster;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public FYsIndentMasterController(
            IF_YS_INDENT_MASTER fYsIndentMaster,
            IYARNFOR yarnfor,
            IF_YS_INDENT_DETAILS fYsIndentDetails,
            IRND_PURCHASE_REQUISITION_MASTER rndPurchaseRequisitionMaster,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
                UserManager<ApplicationUser> userManager
            )
        {
            _yarnfor = yarnfor;
            _fYsIndentDetails = fYsIndentDetails;
            _fYsIndentMaster = fYsIndentMaster;
            _userManager = userManager;
            _rndPurchaseRequisitionMaster = rndPurchaseRequisitionMaster;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("/Indent/Details/{indId}")]
        public async Task<IActionResult> DetailsFYsIndentMaster(string indId)
        {
            try
            {
                return View(await _fYsIndentMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("/Indent/Edit")]
        public async Task<IActionResult> PostEditFYsIndentMaster(RndYarnRequisitionViewModel requisitionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fYsIndentMaster = await _fYsIndentMaster.FindByIdAsync(int.Parse(_protector.Unprotect(requisitionViewModel.FysIndentMaster.EncryptedId)));

                    if (fYsIndentMaster != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        var rndPurchaseRequisitionMaster = await _rndPurchaseRequisitionMaster.GetSinglePurchaseRequisitionByIdAsync(fYsIndentMaster.INDSLID ?? 0);
                        rndPurchaseRequisitionMaster.STATUS = "1";
                        await _rndPurchaseRequisitionMaster.Update(rndPurchaseRequisitionMaster);

                        foreach (var item in requisitionViewModel.FysIndentDetailList.Where(item => item.TRNSID > 0))
                        {
                            var fYsIndentDetails = await _fYsIndentDetails.FindByIdAsync(item.TRNSID);

                            if (fYsIndentDetails != null)
                            {
                                fYsIndentDetails.INDID = fYsIndentMaster.INDID;
                                fYsIndentDetails.ORDER_QTY = item.ORDER_QTY;
                                fYsIndentDetails.CNSMP_AMOUNT = item.CNSMP_AMOUNT;
                                fYsIndentDetails.UPDATED_BY = user.Id;
                                fYsIndentDetails.UPDATED_AT = DateTime.Now;

                                await _fYsIndentDetails.Update(fYsIndentDetails);
                            }
                        }
                    }
                    TempData["message"] = "Successfully Updated Indent Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetIndentMaster", $"FYsIndentMaster");
                }
                else
                {
                    return View(nameof(EditFYsIndentMaster), await _fYsIndentMaster.GetInitObjectsForYarnDetails(requisitionViewModel));
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("/Indent/Edit/{indId}")]
        public async Task<IActionResult> EditFYsIndentMaster(string indId)
        {
            try
            {
                return View(await _fYsIndentMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOtherDetails(string id)
        {
            return PartialView($"OtherDetailsViewModel", await _fYsIndentMaster.GetOtherDetails(id));
        }

        [HttpPost]
        public IActionResult GetProcessedData(List<F_YS_INDENT_DETAILS> fYsIndentDetailse, int selectedTrnsId)
        {
            return Json(fYsIndentDetailse.FirstOrDefault(e => e.TRNSID.Equals(selectedTrnsId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveFromDetailsList(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                if (rndYarnRequisitionViewModel.IsDeletable)
                {
                    //var fYsIndentDetails = rndYarnRequisitionViewModel.FysIndentDetailList[rndYarnRequisitionViewModel.RemoveIndex];

                    // IMPLEMENT LATER
                    //if (fYsIndentDetails.TRNSID > 0)
                    //{
                    //    await _fYsIndentDetails.Delete(fYsIndentDetails);
                    //}

                    rndYarnRequisitionViewModel.FysIndentDetailList.RemoveAt(rndYarnRequisitionViewModel.RemoveIndex);
                }
                else
                {
                    if (rndYarnRequisitionViewModel.FysIndentDetailList.All(e => e.PRODID != rndYarnRequisitionViewModel.FysIndentDetails.PRODID))
                    {
                        rndYarnRequisitionViewModel.FysIndentDetailList.Add(rndYarnRequisitionViewModel.FysIndentDetails);
                    }
                }

                return PartialView($"GetPreviousIndentDetailsPartialView", await _fYsIndentMaster.GetInitObjectsForYarnDetails(rndYarnRequisitionViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/Indent/Create")]
        public async Task<IActionResult> CreateIndentMaster()
        {
            try
            {
                return View(await _fYsIndentMaster.GetInitObjects(new RndYarnRequisitionViewModel()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCreateIndentMaster(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            try
            {
                if (await _fYsIndentMaster.GetIndentByINDSLID(rndYarnRequisitionViewModel.FysIndentMaster.INDSLID ?? 0) == null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    rndYarnRequisitionViewModel.FysIndentMaster.CREATED_AT = rndYarnRequisitionViewModel.FysIndentMaster.UPDATED_AT = DateTime.Now;
                    rndYarnRequisitionViewModel.FysIndentMaster.CREATED_BY = rndYarnRequisitionViewModel.FysIndentMaster.UPDATED_BY = user.Id;

                    var fYsIndentMaster = await _fYsIndentMaster.GetInsertedObjByAsync(rndYarnRequisitionViewModel.FysIndentMaster);

                    if (fYsIndentMaster != null)
                    {
                        var rndPurchaseRequisitionMaster = await _rndPurchaseRequisitionMaster.GetSinglePurchaseRequisitionByIdAsync(fYsIndentMaster.INDSLID ?? 0);
                        rndPurchaseRequisitionMaster.STATUS = "1";
                        await _rndPurchaseRequisitionMaster.Update(rndPurchaseRequisitionMaster);

                        // OLD DATA
                        foreach (var item in rndYarnRequisitionViewModel.FysIndentDetailList.Where(e => e.TRNSID > 0))
                        {
                            var existingData = await _fYsIndentDetails.FindByIdAsync(item.TRNSID);
                            if (existingData != null)
                            {
                                existingData.INDSLID = rndYarnRequisitionViewModel.FysIndentMaster.INDSLID;
                                existingData.TRNSDATE = item.TRNSDATE;
                                existingData.INDID = fYsIndentMaster.INDID;
                                existingData.SECID = item.SECID;
                                existingData.PRODID = item.PRODID;
                                existingData.SLUB_CODE = item.SLUB_CODE;
                                existingData.RAW = item.RAW;
                                existingData.PREV_LOTID = item.PREV_LOTID;
                                existingData.STOCK_AMOUNT = item.STOCK_AMOUNT;
                                existingData.ORDER_QTY = item.ORDER_QTY;
                                existingData.CNSMP_AMOUNT = item.CNSMP_AMOUNT;
                                existingData.YARN_FOR = item.YARN_FOR;
                                existingData.YARN_FROM = item.YARN_FROM;
                                existingData.YARN_TYPE = item.YARN_TYPE;
                                existingData.REMARKS = item.REMARKS;
                                existingData.UPDATED_BY = user.Id;
                                existingData.ETR = item.ETR;
                                existingData.UNIT = item.UNIT;
                                existingData.LAST_INDENT_NO = item.LAST_INDENT_NO;
                                existingData.LAST_INDENT_DATE = item.LAST_INDENT_DATE;
                                existingData.UPDATED_AT = DateTime.Now;
                                existingData.UPDATED_BY = user.Id;

                                await _fYsIndentDetails.Update(existingData);
                            }

                            TempData["message"] = "Successfully added New Indent Master Information.";
                            TempData["type"] = "success";

                            //return RedirectToAction("GetIndentMaster", $"FYsIndentMaster");
                            //return View(nameof(GetIndentMaster), await _fYsIndentMaster.GetInitObjects(rndYarnRequisitionViewModel));
                        }

                        // NOT EXIST, PROCESS NEW DATA
                        var fYsIndentDetailses = rndYarnRequisitionViewModel.FysIndentDetailList.Where(e => e.TRNSID.Equals(0)).ToList();

                        foreach (var item in fYsIndentDetailses)
                        {
                            item.INDID = fYsIndentMaster.INDID;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                        }

                        if (fYsIndentDetailses.Any())
                        {
                            await _fYsIndentDetails.InsertRangeByAsync(fYsIndentDetailses);
                        }


                        TempData["message"] = "Successfully added New Indent Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetIndentMaster), $"FYsIndentMaster");
                    }

                    TempData["message"] = "Failed to Add Indent Information";
                    TempData["type"] = "error";
                    return View(nameof(CreateIndentMaster), await _fYsIndentMaster.GetInitObjects(rndYarnRequisitionViewModel));
                }

                TempData["message"] = "Indent Already Exists";
                TempData["type"] = "error";
                return View(nameof(CreateIndentMaster), await _fYsIndentMaster.GetInitObjects(rndYarnRequisitionViewModel));
            }
            catch (Exception ex)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("/Indent/GetAll")]
        public IActionResult GetIndentMaster()
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
                var data = (List<F_YS_INDENT_MASTER>)await _fYsIndentMaster.GetAllIndentMasterAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.INDNO.ToUpper().Contains(searchValue)
                                        || (m.INDDATE != null && m.INDDATE.ToString().ToUpper().Contains(searchValue))
                                        || (m.INDSLID != null && m.INDSLID.ToString().ToUpper().Contains(searchValue))
                                        || (m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue))
                                        || (m.OPT2 != null && m.OPT2.ToUpper().Contains(searchValue))
                                        || (m.OPT3 != null && m.OPT3.ToUpper().Contains(searchValue))
                                        || (m.OPT4 != null && m.OPT4.ToUpper().Contains(searchValue))
                                        || (m.IsRevised != null && m.IsRevised.ToUpper().Contains(searchValue))
                                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        || (m.INDSL.REMARKS != null && m.INDSL.REMARKS.ToUpper().Contains(searchValue))
                    ).ToList();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<RndYarnRequisitionViewModel> GetPurchaseRequisitionById(int id)
        {
            try
            {
                return await _rndPurchaseRequisitionMaster.GetPurchaseRequisitionById(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPreviousIndentDetailsById(int id, int prdId)
        {
            try
            {
                return PartialView($"DetailsFYsIndentMasterPartialViewTable", new RndYarnRequisitionViewModel
                {
                    FysIndentDetails = await _fYsIndentDetails.FindIndentListByIdAsync(id, prdId)
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<FYarnIndentComExPiViewModel> GetCountNameListById(int id)
        {
            try
            {
                return await _fYsIndentDetails.FindAllpRODUCTListAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIndentCountList(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                rndYarnRequisitionViewModel = await _fYsIndentMaster.GetIndentCountListByINDSLID(rndYarnRequisitionViewModel);

                return PartialView($"GetPreviousIndentDetailsPartialView", await _fYsIndentMaster.GetInitObjectsForYarnDetails(rndYarnRequisitionViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}