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
    public class YarnReceiveForOthersController : Controller
    {
        private readonly IF_YS_YARN_RECEIVE_MASTER2 _fYsYarnReceiveMaster;
        private readonly IF_YS_YARN_RECEIVE_DETAILS2 _fYsYarnReceiveDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public YarnReceiveForOthersController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
           
            IF_YS_YARN_RECEIVE_MASTER2 fYsYarnReceiveMaster,
            IF_YS_YARN_RECEIVE_DETAILS2 fYsYarnReceiveDetails,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fYsYarnReceiveMaster = fYsYarnReceiveMaster;
            _fYsYarnReceiveDetails = fYsYarnReceiveDetails;
            _userManager = userManager;
        }
  
        public IActionResult GetReceiveMaster()
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

                var data = (List<F_YS_YARN_RECEIVE_MASTER2>)await _fYsYarnReceiveMaster.GetAllYarnReceiveAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CHALLANNO != null && m.CHALLANNO.ToUpper().Contains(searchValue)).ToList();
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


        public async Task<IActionResult> CreateYarnReceiveForOthers()
        {
            try
            {
                return View(await _fYsYarnReceiveMaster.GetInitObjectsByAsync(new YarnReceiveForOthersViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }


        public async Task<IActionResult> AddToList(YarnReceiveForOthersViewModel yarnReceiveForOthersViewModel)
        {
            try
            {
                ModelState.Clear();
                if (yarnReceiveForOthersViewModel.IsDelete)
                {
                    var fysgpdetails = yarnReceiveForOthersViewModel.FYsYarnReceiveDetailList[yarnReceiveForOthersViewModel.RemoveIndex];

                    if (fysgpdetails.TRNSID > 0)
                    {
                        await _fYsYarnReceiveDetails.Delete(fysgpdetails);
                    }

                    yarnReceiveForOthersViewModel.FYsYarnReceiveDetailList.RemoveAt(yarnReceiveForOthersViewModel.RemoveIndex);
                }
                else
                {
                    if (TryValidateModel(yarnReceiveForOthersViewModel.FYsYarnReceiveDetail))
                    {
                        yarnReceiveForOthersViewModel.FYsYarnReceiveDetailList.Add(yarnReceiveForOthersViewModel.FYsYarnReceiveDetail);
                    }
                }

                return PartialView($"PartialViewForYarnOtherReceive", await _fYsYarnReceiveMaster.GetInitDetailsObjByAsync(yarnReceiveForOthersViewModel));


            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateYarnReceiveForOthers(YarnReceiveForOthersViewModel yarnReceiveForOthersViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.CREATED_BY = yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.UPDATED_BY = user.Id;
                yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.CREATED_AT = yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var MASTER = await _fYsYarnReceiveMaster.GetInsertedObjByAsync(yarnReceiveForOthersViewModel.FYsYarnReceiveMaster);

                if (MASTER.YRCVID != 0)
                {
                    foreach (var item in yarnReceiveForOthersViewModel.FYsYarnReceiveDetailList)
                    {
                        item.YRCVID = MASTER.YRCVID;
                        item.TRNSDATE = yarnReceiveForOthersViewModel.FYsYarnReceiveDetail.TRNSDATE;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        if (await _fYsYarnReceiveDetails.InsertByAsync(item))
                        {
                            atLeastOneInsert = true;
                        }
                    }
                    if (!atLeastOneInsert)
                    {
                        await _fYsYarnReceiveMaster.Delete(MASTER);
                        TempData["message"] = $"Failed to Add";
                        TempData["type"] = "error";
                        return View(nameof(CreateYarnReceiveForOthers), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveForOthersViewModel));
                    }

                    TempData["message"] = $"Successfully added";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetReceiveMaster), $"YarnReceiveForOthers");
                }

                await _fYsYarnReceiveMaster.Delete(MASTER);
                TempData["message"] = $"Failed to Add";
                TempData["type"] = "error";
                return View(nameof(CreateYarnReceiveForOthers), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveForOthersViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }


        public async Task<IActionResult> EditYarnReceiveForOthers(string id)
        {
            return View(await _fYsYarnReceiveMaster.GetInitObjectsByAsync(await _fYsYarnReceiveMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(id)))));
        }

        [HttpPost]
        public async Task<IActionResult> EditYarnReceiveForOthers(YarnReceiveForOthersViewModel yarnReceiveForOthersViewModel)
        {
            if (ModelState.IsValid)
            {
                var MASTER = await _fYsYarnReceiveMaster.FindByIdAsync(int.Parse(_protector.Unprotect(yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.EncryptedId)));

                if (MASTER != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.YRCVID = MASTER.YRCVID;
                    //fysgpviewModel.f_YS_GP_MASTER.GPDATE = fysgpMASTER.GPDATE;
                    yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.CREATED_AT = MASTER.CREATED_AT;
                    yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.CREATED_BY = MASTER.CREATED_BY;
                    yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.UPDATED_AT = DateTime.Now;
                    yarnReceiveForOthersViewModel.FYsYarnReceiveMaster.UPDATED_BY = user.Id;

                    if (await _fYsYarnReceiveMaster.Update(yarnReceiveForOthersViewModel.FYsYarnReceiveMaster))
                    {
                        var Detailses = yarnReceiveForOthersViewModel.FYsYarnReceiveDetailList.Where(d => d.TRNSID <= 0).ToList();

                        foreach (var item in Detailses)
                        {
                            item.YRCVID = MASTER.YRCVID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        if (await _fYsYarnReceiveDetails.InsertRangeByAsync(Detailses))
                        {
                            TempData["message"] = $"Successfully Updated";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetReceiveMaster), $"YarnReceiveForOthers");
                        }
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditYarnReceiveForOthers), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveForOthersViewModel));
            }

            return View(nameof(EditYarnReceiveForOthers), await _fYsYarnReceiveMaster.GetInitObjectsByAsync(yarnReceiveForOthersViewModel));
        }



    }

   

}
