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
    [Authorize]
    [Route("AdvanceDeliverySchedule")]
    public class ComExAdvDeliverySchController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICOM_EX_ADV_DELIVERY_SCH_MASTER _comExAdvDeliverySchMaster;
        private readonly ICOM_EX_ADV_DELIVERY_SCH_DETAILS _comExAdvDeliverySchDetails;
        private readonly IBAS_BUYERINFO _basBuyerinfo;
        private readonly ICOM_EX_PIMASTER _comExPimaster;
        private readonly ICOM_EX_PI_DETAILS _comExPiDetails;
        private readonly IDataProtector _protector;
        private readonly string title = "Advance Delivery Schedule Information";

        public ComExAdvDeliverySchController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            ICOM_EX_ADV_DELIVERY_SCH_MASTER comExAdvDeliverySchMaster,
            ICOM_EX_ADV_DELIVERY_SCH_DETAILS comExAdvDeliverySchDetails,
            IBAS_BUYERINFO basBuyerinfo,
            ICOM_EX_PIMASTER comExPimaster,
            ICOM_EX_PI_DETAILS comExPiDetails)
        {
            _userManager = userManager;
            _comExAdvDeliverySchMaster = comExAdvDeliverySchMaster;
            _comExAdvDeliverySchDetails = comExAdvDeliverySchDetails;
            _basBuyerinfo = basBuyerinfo;
            _comExPimaster = comExPimaster;
            _comExPiDetails = comExPiDetails;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetComExAdvDeliverySch()
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
        [Route("GetList")]
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

            var data = await _comExAdvDeliverySchMaster.GetAllAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.DSNO != null && m.DSNO.ToUpper().Contains(searchValue)
                                       || m.DSDATE != null && m.DSDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.DSTYPE != null && m.DSTYPE.ToUpper().Contains(searchValue)
                                       || m.BUYER_ID != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
        [Route("Create")]
        public async Task<IActionResult> CreateComExAdvDeliverySch()
        {
            try
            {
                return View(await _comExAdvDeliverySchMaster.GetInitObjByAsync(new ComExAdvDeliverySchViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateComExAdvDeliverySch(ComExAdvDeliverySchViewModel fComExAdvDeliverySchViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fComExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.DSNO = await _comExAdvDeliverySchMaster.GetLastDSNoAsync();
                fComExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.CREATED_BY = fComExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.UPDATED_BY = user.Id;
                fComExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.CREATED_AT = fComExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.UPDATED_AT = DateTime.Now;

                var atLeastOneInsert = false;

                var comExAdvDeliverySchMaster = await _comExAdvDeliverySchMaster.GetInsertedObjByAsync(fComExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster);

                if (comExAdvDeliverySchMaster.DSID != 0)
                {
                    foreach (var item in fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetailsList)
                    {
                        item.DSID = comExAdvDeliverySchMaster.DSID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;

                        if (await _comExAdvDeliverySchDetails.InsertByAsync(item))
                        {
                            atLeastOneInsert = true;
                        }
                    }
                    if (!atLeastOneInsert)
                    {
                        await _comExAdvDeliverySchMaster.Delete(comExAdvDeliverySchMaster);
                        TempData["message"] = $"Failed to Add {title}.";
                        TempData["type"] = "error";
                        return View(nameof(CreateComExAdvDeliverySch), await _comExAdvDeliverySchMaster.GetInitObjByAsync(fComExAdvDeliverySchViewModel));
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetComExAdvDeliverySch), $"ComExAdvDeliverySch");
                }

                await _comExAdvDeliverySchMaster.Delete(comExAdvDeliverySchMaster);
                TempData["message"] = $"Failed to Add {title}.";
                TempData["type"] = "error";
                return View(nameof(CreateComExAdvDeliverySch), await _comExAdvDeliverySchMaster.GetInitObjByAsync(fComExAdvDeliverySchViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("Edit/{dsId?}")]
        public async Task<IActionResult> EditComExAdvDeliverySch(string dsId)
        {
            return View(await _comExAdvDeliverySchMaster.GetInitObjByAsync(await _comExAdvDeliverySchMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(dsId)))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditComExAdvDeliverySch(ComExAdvDeliverySchViewModel comExAdvDeliverySchViewModel)
        {
            if (ModelState.IsValid)
            {
                var comExAdvDeliverySchMaster = await _comExAdvDeliverySchMaster.FindByIdAsync(int.Parse(_protector.Unprotect(comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.EncryptedId)));

                if (comExAdvDeliverySchMaster != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.DSNO = comExAdvDeliverySchMaster.DSNO;
                    comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.DSID = comExAdvDeliverySchMaster.DSID;
                    comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.CREATED_AT = comExAdvDeliverySchMaster.CREATED_AT;
                    comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.CREATED_BY = comExAdvDeliverySchMaster.CREATED_BY;
                    comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.UPDATED_AT = DateTime.Now;
                    comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster.UPDATED_BY = user.Id;

                    if (await _comExAdvDeliverySchMaster.Update(comExAdvDeliverySchViewModel.ComExAdvDeliverySchMaster))
                    {
                        var comExAdvDeliverySchDetailses = comExAdvDeliverySchViewModel.ComExAdvDeliverySchDetailsList.Where(d => d.TRNSID <= 0).ToList();

                        foreach (var item in comExAdvDeliverySchDetailses)
                        {
                            item.DSID = comExAdvDeliverySchMaster.DSID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                        }

                        if (await _comExAdvDeliverySchDetails.InsertRangeByAsync(comExAdvDeliverySchDetailses))
                        {
                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetComExAdvDeliverySch), $"ComExAdvDeliverySch");
                        }
                    }
                }

                ModelState.AddModelError("", "We can not process your request. Please try again later.");
                return View(nameof(EditComExAdvDeliverySch), await _comExAdvDeliverySchMaster.GetInitObjByAsync(comExAdvDeliverySchViewModel));
            }

            return View(nameof(EditComExAdvDeliverySch), await _comExAdvDeliverySchMaster.GetInitObjByAsync(comExAdvDeliverySchViewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddOrRemoveFromList")]
        public async Task<IActionResult> AddToList(ComExAdvDeliverySchViewModel fComExAdvDeliverySchViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fComExAdvDeliverySchViewModel.IsDelete)
                {
                    var comExAdvDeliverySchDetails = fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetailsList[fComExAdvDeliverySchViewModel.RemoveIndex];

                    if (comExAdvDeliverySchDetails.TRNSID > 0)
                    {
                        await _comExAdvDeliverySchDetails.Delete(comExAdvDeliverySchDetails);
                    }

                    fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetailsList.RemoveAt(fComExAdvDeliverySchViewModel.RemoveIndex);
                }
                else
                {
                    if (TryValidateModel(fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetails))
                    {
                        fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetailsList.Add(fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetails);
                    }
                }

                return PartialView($"ComExAdvDeliverySchPartialView", await _comExAdvDeliverySchMaster.GetInitDetailsObjByAsync(fComExAdvDeliverySchViewModel));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetAddress/{buyerId?}")]
        public async Task<IActionResult> GetBuyerAddressById(int buyerId)
        {
            try
            {
                return Ok(await _basBuyerinfo.GetBuyerAddressByIdAsync(buyerId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GetStyle/{piId?}")]
        public async Task<IActionResult> GetStyleByPi(int piId)
        {
            try
            {
                return Ok(await _comExPiDetails.GetStyleByPiAsync(piId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GetPI/{buyerId?}")]
        public async Task<IActionResult> GetPiByBuyer(int buyerId)
        {
            try
            {
                return Ok(await _comExPimaster.GetPiByBuyerAsync(buyerId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetUnit/{id?}")]
        public async Task<IActionResult> GetUnitByPi(int id)
        {
            try
            {
                return Ok(await _comExPiDetails.GetUnitByPiAsync(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
