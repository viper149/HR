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
    public class FYsGpController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_YS_GP_MASTER  _fYsGpMaster;
        private readonly IF_YS_GP_DETAILS _fYsGpDetails;
        private readonly IDataProtector _protector;

        public FYsGpController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_YS_GP_MASTER fYsGpMaster,
            IF_YS_GP_DETAILS fYsGpDetails)
      
        {
            _userManager = userManager;
            _fYsGpMaster = fYsGpMaster;
            _fYsGpDetails = fYsGpDetails;
  
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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

            var data = await _fYsGpMaster.GetAllAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    //data = data.Where(m => m.DSNO != null && m.DSNO.ToUpper().Contains(searchValue)
            //    //                       || m.DSDATE != null && m.DSDATE.ToString().ToUpper().Contains(searchValue)
            //    //                       || m.DSTYPE != null && m.DSTYPE.ToUpper().Contains(searchValue)
            //    //                       || m.BUYER_ID != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
            //    //                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
            //}

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


        public IActionResult GetFYsGp()
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


        public async Task<IActionResult> CreateFYsGp()
        {
            try
            {
                return View(await _fYsGpMaster.GetInitObjByAsync(new FYsGpViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateFYsGp(FYsGpViewModel fysgpviewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fysgpviewModel.f_YS_GP_MASTER.CREATED_BY = fysgpviewModel.f_YS_GP_MASTER.UPDATED_BY = user.Id;
                fysgpviewModel.f_YS_GP_MASTER.CREATED_AT = fysgpviewModel.f_YS_GP_MASTER.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var fysgpMASTER = await _fYsGpMaster.GetInsertedObjByAsync(fysgpviewModel.f_YS_GP_MASTER);

                if (fysgpMASTER.GPID != 0)
                {
                    foreach (var item in fysgpviewModel.fysgpdetailsList)
                    {
                        item.GPID = fysgpMASTER.GPID;
                        item.TRNSDATE = fysgpviewModel.f_YS_GP_DETAILS.TRNSDATE;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        if (await _fYsGpDetails.InsertByAsync(item))
                        {
                            atLeastOneInsert = true;
                        }
                    }
                    if (!atLeastOneInsert)
                    {
                        await _fYsGpMaster.Delete(fysgpMASTER);
                        TempData["message"] = $"Failed to Add";
                        TempData["type"] = "error";
                        return View(nameof(CreateFYsGp), await _fYsGpMaster.GetInitObjByAsync(fysgpviewModel));
                    }

                    TempData["message"] = $"Successfully added";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFYsGp), $"FYsGp");
                }

                await _fYsGpMaster.Delete(fysgpMASTER);
                TempData["message"] = $"Failed to Add";
                TempData["type"] = "error";
                return View(nameof(CreateFYsGp), await _fYsGpMaster.GetInitObjByAsync(fysgpviewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }


        public async Task<IActionResult> EditFYsGp(string dsId)
        {
            return View(await _fYsGpMaster.GetInitObjByAsync(await _fYsGpMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(dsId)))));
        }



        [HttpPost]
        public async Task<IActionResult> EditFYsGp(FYsGpViewModel fysgpviewModel)
        {
            if (ModelState.IsValid)
            {
                var fysgpMASTER = await _fYsGpMaster.FindByIdAsync(int.Parse(_protector.Unprotect(fysgpviewModel.f_YS_GP_MASTER.EncryptedId)));

                if (fysgpMASTER != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fysgpviewModel.f_YS_GP_MASTER.GPID = fysgpMASTER.GPID;
                    //fysgpviewModel.f_YS_GP_MASTER.GPDATE = fysgpMASTER.GPDATE;
                    fysgpviewModel.f_YS_GP_MASTER.CREATED_AT = fysgpMASTER.CREATED_AT;
                    fysgpviewModel.f_YS_GP_MASTER.CREATED_BY = fysgpMASTER.CREATED_BY;
                    fysgpviewModel.f_YS_GP_MASTER.UPDATED_AT = DateTime.Now;
                    fysgpviewModel.f_YS_GP_MASTER.UPDATED_BY = user.Id;

                    if (await _fYsGpMaster.Update(fysgpviewModel.f_YS_GP_MASTER))
                    {
                        var fysgpDetailses = fysgpviewModel.fysgpdetailsList.Where(d => d.TRNSID <= 0).ToList();

                        foreach (var item in fysgpDetailses)
                        {
                            item.GPID = fysgpMASTER.GPID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        if (await _fYsGpDetails.InsertRangeByAsync(fysgpDetailses))
                        {
                            TempData["message"] = $"Successfully Updated";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFYsGp), $"FYsGp");
                        }
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditFYsGp), await _fYsGpMaster.GetInitObjByAsync(fysgpviewModel));
            }

            return View(nameof(EditFYsGp), await _fYsGpMaster.GetInitObjByAsync(fysgpviewModel));
        }


        [HttpGet]
        public async Task<IActionResult> DeleteFYsGp(string dsId)
        {
            var gpMaster = await _fYsGpMaster.FindByIdForDeleteAsync(int.Parse(_protector.Unprotect(dsId)));

            if (gpMaster != null)
            {

                if (gpMaster.F_YS_GP_DETAILS.Any())
                {
                    await _fYsGpDetails.DeleteRange(gpMaster.F_YS_GP_DETAILS.ToList());
                }

                await _fYsGpMaster.Delete(gpMaster);

                TempData["message"] = "Successfully Deleted";
                TempData["type"] = "success";

                return RedirectToAction(nameof(GetFYsGp), $"FYsGp");
            }

            TempData["message"] = "Failed To Delete";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFYsGp), $"FYsGp");
        }


        public async Task<IActionResult> AddToList(FYsGpViewModel fysgpViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fysgpViewModel.IsDelete)
                {
                    var fysgpdetails = fysgpViewModel.fysgpdetailsList[fysgpViewModel.RemoveIndex];

                    if (fysgpdetails.TRNSID > 0)
                    {
                        await _fYsGpDetails.Delete(fysgpdetails);
                    }

                    fysgpViewModel.fysgpdetailsList.RemoveAt(fysgpViewModel.RemoveIndex);
                }
                else
                {
                    if (TryValidateModel(fysgpViewModel.f_YS_GP_DETAILS))
                    {
                        fysgpViewModel.fysgpdetailsList.Add(fysgpViewModel.f_YS_GP_DETAILS);
                    }
                }

                  return PartialView($"FYsGpPartialView", await _fYsGpMaster.GetInitDetailsObjByAsync(fysgpViewModel));

                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetLcInfo(int id)
        {
            try
            {
                return Ok(await _fYsGpMaster.GetLcInfoAsync(id));

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetYarnIndentDetails(int countId)
        {
            try
            {
                return Ok(await _fYsGpMaster.GetYarnIndentDetails(countId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetLotId(int lotid)
        {
            try
            {
                return Ok(await _fYsGpMaster.GetLotId(lotid));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
