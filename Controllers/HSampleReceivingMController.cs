using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.SampleGarments.HReceive;
using DenimERP.ViewModels.SampleGarments.HReceive;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize(Policy = "HOSample")]
    public class HSampleReceivingMController : Controller
    {
        private readonly IH_SAMPLE_RECEIVING_M _hSampleReceivingM;
        private readonly IH_SAMPLE_RECEIVING_D _hSampleReceivingD;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public HSampleReceivingMController(IH_SAMPLE_RECEIVING_M hSampleReceivingM,
            IH_SAMPLE_RECEIVING_D hSampleReceivingD,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _hSampleReceivingM = hSampleReceivingM;
            _hSampleReceivingD = hSampleReceivingD;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RCVID"> Belongs to . Primary key. Must not be null. <see cref="H_SAMPLE_RECEIVING_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteHSampleReceivingM(string hsrId)
        {
            try
            {
                var createHSampleReceivingMViewModel = await _hSampleReceivingM.FindByHsrIdIncludeAllAsync(int.Parse(_protector.Unprotect(hsrId)));

                if (createHSampleReceivingMViewModel.HSampleReceivingM.H_SAMPLE_RECEIVING_D.Any())
                {
                    await _hSampleReceivingD.DeleteRange(createHSampleReceivingMViewModel.HSampleReceivingM.H_SAMPLE_RECEIVING_D);
                }

                if (await _hSampleReceivingM.Delete(createHSampleReceivingMViewModel.HSampleReceivingM))
                {
                    TempData["message"] = "Successfully Deleted HO, Sample Garments Receive Information.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Failed To Delete HO, Sample Garments Receive Information.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetHSampleReceivingM", $"HSampleReceivingM");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RCVID"> Belongs to . Primary key. Must not be null. <see cref="H_SAMPLE_RECEIVING_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailsHSampleReceivingM(string hsrId)
        {
            try
            {
                return View(await _hSampleReceivingM.FindByHsrIdIncludeAllAsync(int.Parse(_protector.Unprotect(hsrId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createHSampleReceivingMViewModel"> View model. <see cref="CreateHSampleReceivingMViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditHSampleReceivingM(CreateHSampleReceivingMViewModel createHSampleReceivingMViewModel)
        {
            try
            {
                var hSampleReceivingM = await _hSampleReceivingM.FindByIdAsync(int.Parse(_protector.Unprotect(createHSampleReceivingMViewModel.HSampleReceivingM.EncryptedId)));
                var currentUser = await _userManager.GetUserAsync(User);

                if (hSampleReceivingM != null)
                {
                    hSampleReceivingM.RCVDATE = createHSampleReceivingMViewModel.HSampleReceivingM.RCVDATE;
                    hSampleReceivingM.REMARKS = createHSampleReceivingMViewModel.HSampleReceivingM.REMARKS;
                    hSampleReceivingM.UPDATED_AT = DateTime.Now;
                    hSampleReceivingM.UPDATED_BY = currentUser.Id;

                    if (await _hSampleReceivingM.Update(hSampleReceivingM))
                    {
                        TempData["message"] = "Successfully Updated HO, Sample Garments Receive Information.";
                        TempData["type"] = "success";
                    }
                    else
                    {
                        TempData["message"] = "Failed To Update HO, Sample Garments Receive Information.";
                        TempData["type"] = "error";
                    }
                }
                else
                {
                    TempData["message"] = "Not found! HO, Sample Garments Receive Information.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetHSampleReceivingM", $"HSampleReceivingM");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RCVID"> Belongs to . Primary key. Must not be null. <see cref="H_SAMPLE_RECEIVING_M"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditHSampleReceivingM(string hsrId)
        {
            try
            {
                return View(await _hSampleReceivingM.FindByHsrIdIncludeAllAsync(int.Parse(_protector.Unprotect(hsrId))));
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _hSampleReceivingM.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                return Json(new
                {
                    draw = forDataTableByAsync.Draw,
                    recordsFiltered = forDataTableByAsync.RecordsTotal,
                    recordsTotal = forDataTableByAsync.RecordsTotal,
                    data = forDataTableByAsync.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetHSampleReceivingM()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createHSampleReceivingMViewModel"> View model. <see cref="CreateHSampleReceivingMViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateHSampleReceivingM(CreateHSampleReceivingMViewModel createHSampleReceivingMViewModel)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var hSampleReceivingMViewModel = await _hSampleReceivingM.GetFactoryGatePassReceiveDetails(createHSampleReceivingMViewModel.HSampleReceivingM.DPID ?? 0);
                var hSampleReceivingM = await _hSampleReceivingM.GetInsertedObjByAsync(new H_SAMPLE_RECEIVING_M
                {
                    RCVDATE = createHSampleReceivingMViewModel.HSampleReceivingM.RCVDATE,
                    DPID = hSampleReceivingMViewModel.FSampleDespatchMaster.DPID,
                    GPNO = hSampleReceivingMViewModel.FSampleDespatchMaster.GPNO.ToString(),
                    GPDATE = hSampleReceivingMViewModel.FSampleDespatchMaster.GPDATE,
                    DRID = hSampleReceivingMViewModel.FSampleDespatchMaster.DRID,
                    VID = hSampleReceivingMViewModel.FSampleDespatchMaster.VID,
                    REMARKS = hSampleReceivingMViewModel.FSampleDespatchMaster.REMARKS,
                    CREATED_AT = DateTime.Now,
                    UPDATED_AT = DateTime.Now,
                    CREATED_BY = currentUser.Id,
                    UPDATED_BY = currentUser.Id
                });

                var hSampleReceivingDs = hSampleReceivingMViewModel.FSampleDespatchMaster.F_SAMPLE_DESPATCH_DETAILS.Select(item => new H_SAMPLE_RECEIVING_D
                {
                    RCVID = hSampleReceivingM.RCVID,
                    TRNSID = item.TRNSID,
                    BUYERID = item.BYERID,
                    UID = item.UID,
                    QTY = item.DEL_QTY,
                    CREATED_AT = DateTime.Now,
                    UPDATED_AT = DateTime.Now,
                    CREATED_BY = currentUser.Id,
                    UPDATED_BY = currentUser.Id
                }).ToList();

                await _hSampleReceivingD.InsertRangeByAsync(hSampleReceivingDs);

                if (hSampleReceivingM != null)
                {
                    TempData["message"] = "Successfully Added HO, Sample Garments Receive Information.";
                    TempData["type"] = "success";
                }
                else
                {
                    TempData["message"] = "Failed To Add HO, Sample Garments Receive Information.";
                    TempData["type"] = "success";
                }

                return RedirectToAction("GetHSampleReceivingM", $"HSampleReceivingM");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dpId"> Belongs to DPID. Primary key. Must not be null. <see cref="F_SAMPLE_DESPATCH_MASTER"/></param>
        /// <returns> Partial view.</returns>
        [HttpPost]
        public async Task<IActionResult> GetFactoryGatePassReceiveDetails(int dpId)
        {
            var createHSampleReceivingMViewModel = await _hSampleReceivingM.GetFactoryGatePassReceiveDetails(dpId);
            return PartialView($"GetFactoryGatePassReceiveDetailsTable", createHSampleReceivingMViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateHSampleReceivingM()
        {
            return View(await _hSampleReceivingM.GetInitObjects(new CreateHSampleReceivingMViewModel()));
        }
    }
}