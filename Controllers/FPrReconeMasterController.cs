using System;
using System.Collections.Generic;
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
    public class FPrReconeMasterController : Controller
    {

      
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly IF_PR_RECONE_MASTER _fPrReconeMaster;
        private readonly IF_PR_RECONE_YARN_DETAILS _fPrReconeYarnDetails;
        private readonly IF_PR_RECONE_YARN_CONSUMPTION _fPrReconeYarnConsumption;
        private readonly string title = "Recone Production Information";

        public FPrReconeMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_PR_RECONE_MASTER fPrReconeMaster,
            IF_PR_RECONE_YARN_DETAILS fPrReconeYarnDetails,
            IF_PR_RECONE_YARN_CONSUMPTION fPrReconeYarnConsumption,
            UserManager<ApplicationUser> userManager
        )
        {
            _fPrReconeMaster = fPrReconeMaster;
            _fPrReconeYarnDetails = fPrReconeYarnDetails;
            _fPrReconeYarnConsumption = fPrReconeYarnConsumption;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        //[Route("GetAll")]
        public IActionResult GetFPrReconeMasterInfo()
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

            var data = (List<F_PR_RECONE_MASTER>)await _fPrReconeMaster.GetFPrReconeMasterInfoAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.TRANSDATE != null && m.TRANSDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.SET_NO != null && m.SET_NO.ToString().ToUpper().Contains(searchValue)
                                       || m.NO_OF_BALL != null && m.NO_OF_BALL.ToString().ToUpper().Contains(searchValue)
                                       || m.WARP_LENGTH != null && m.WARP_LENGTH.ToString().ToUpper().Contains(searchValue)
                                       || m.WARP_RATIO != null && m.WARP_RATIO.ToString().ToUpper().Contains(searchValue)
                                       || m.CONVERTED != null && m.CONVERTED.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
            }

            var recordsTotal = data.Count();
            var finalData = data.Skip(skip).Take(pageSize).ToList();

            //foreach (var item in finalData)
            //{
            //    item.EncryptedId = _protector.Protect(item.WV_PRODID.ToString());
            //}
            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = finalData
            });
        }

        [HttpGet]
        //[Route("Create")]
        public async Task<IActionResult> CreateFPrReconeMasterInfo()
        {
            try
            {
                return View(await _fPrReconeMaster.GetInitObjByAsync(new FPrReconeMasterViewModel()));
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateFPrReconeMasterInfo(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var result = await _fPrReconeMaster.InsertByAsync(fPrReconeMasterViewModel.F_PR_RECONE_MASTER);

        //            if (result)
        //            {
        //                TempData["message"] = "Successfully Added ";
        //                TempData["type"] = "success";
        //                return RedirectToAction("GetFPrReconeMasterInfo", $"FPrReconeMaster");
        //            }

        //            TempData["message"] = "Failed to Add .";
        //            TempData["type"] = "error";
        //            return View(fPrReconeMasterViewModel);
        //        }

        //        TempData["message"] = "Please Fill All The Fields with Valid Data.";
        //        TempData["type"] = "error";
        //        return View(fPrReconeMasterViewModel);
        //    }
        //    catch (Exception)
        //    {
        //        TempData["message"] = "Failed to Add .";
        //        TempData["type"] = "error";
        //        return View(fPrReconeMasterViewModel);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateFPrReconeMasterInfo(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fPrReconeMasterViewModel.F_PR_RECONE_MASTER.CREATED_BY = user.Id;
                    fPrReconeMasterViewModel.F_PR_RECONE_MASTER.UPDATED_BY = user.Id;
                    fPrReconeMasterViewModel.F_PR_RECONE_MASTER.CREATED_AT = DateTime.Now;
                    fPrReconeMasterViewModel.F_PR_RECONE_MASTER.UPDATED_AT = DateTime.Now;

                    var result = await _fPrReconeMaster.GetInsertedObjByAsync(fPrReconeMasterViewModel.F_PR_RECONE_MASTER);

                    if (result != null)
                    {
                        foreach (var item in fPrReconeMasterViewModel.ReconeYarnDetailsList)
                        {
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.TRANSID = result.TRANSID;
                            await _fPrReconeYarnDetails.InsertByAsync(item);
                        }

                        TempData["message"] = $"Successfully Added {title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFPrReconeMasterInfo), $"FPrReconeMaster");
                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(fPrReconeMasterViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fPrReconeMasterViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(fPrReconeMasterViewModel);
            }
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> GetAllBysetId(int wpId)
        //{
        //    try
        //    {
        //        return Ok(await _fPrReconeYarnDetails.GetAllBywpIdAsync(wpId));
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        //[Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddFPrReconeDetails(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fPrReconeMasterViewModel.IsDelete)
                {
                    var fPrReconeDetails = fPrReconeMasterViewModel.ReconeYarnDetailsList
                        [fPrReconeMasterViewModel.RemoveIndex];

                    if (fPrReconeDetails.TRANSID > 0)
                    {
                        await _fPrReconeYarnDetails.Delete(fPrReconeDetails);
                    }

                    fPrReconeMasterViewModel.ReconeYarnDetailsList.RemoveAt(fPrReconeMasterViewModel.RemoveIndex);
                }
                else if (!fPrReconeMasterViewModel.ReconeYarnDetailsList.Any(e => e.TRANSID.Equals(fPrReconeMasterViewModel.F_PR_RECONE_YARN_DETAILS.TRANSID)))
                {
                    if (TryValidateModel(fPrReconeMasterViewModel.F_PR_RECONE_YARN_DETAILS))
                    {
                        fPrReconeMasterViewModel.ReconeYarnDetailsList.Add(fPrReconeMasterViewModel.F_PR_RECONE_YARN_DETAILS);
                    }
                }

                Response.Headers["HasItems"] = $"{fPrReconeMasterViewModel.ReconeYarnDetailsList.Any()}";

                return PartialView($"FPrReconeDetailsPartialView", await _fPrReconeYarnDetails.GetInitObjForDetailsByAsync(fPrReconeMasterViewModel));
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        //[Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddFPrConsumptionDetails(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fPrReconeMasterViewModel.IsDelete)
                {
                    var fPrReconeConsumptionDetails = fPrReconeMasterViewModel.ReconeYarnConsumptionList
                        [fPrReconeMasterViewModel.RemoveIndex];

                    if (fPrReconeConsumptionDetails.TRANSID > 0)
                    {
                        await _fPrReconeYarnConsumption.Delete(fPrReconeConsumptionDetails);
                    }

                    fPrReconeMasterViewModel.ReconeYarnConsumptionList.RemoveAt(fPrReconeMasterViewModel.RemoveIndex);
                }
                else if (!fPrReconeMasterViewModel.ReconeYarnConsumptionList.Any(e => e.TRANSID.Equals(fPrReconeMasterViewModel.F_PR_RECONE_YARN_CONSUMPTION.TRANSID)))
                {
                    if (TryValidateModel(fPrReconeMasterViewModel.F_PR_RECONE_YARN_CONSUMPTION))
                    {
                        fPrReconeMasterViewModel.ReconeYarnConsumptionList.Add(fPrReconeMasterViewModel.F_PR_RECONE_YARN_CONSUMPTION);
                    }
                }

                Response.Headers["HasItems"] = $"{fPrReconeMasterViewModel.ReconeYarnConsumptionList.Any()}";

                return PartialView($"FPrReconeConsumptionPartialView", await _fPrReconeYarnConsumption.GetInitObjForDetailsByAsync(fPrReconeMasterViewModel));
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }

    }
} 
