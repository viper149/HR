using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ComExCashInfoController : Controller
    {
        private readonly ICOM_EX_CASHINFO _comExCashInfo;
        private readonly IDataProtector _protector;

        public ComExCashInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_EX_CASHINFO comExCashInfo
        )
        {
            _comExCashInfo = comExCashInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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

                var data = await _comExCashInfo.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CASHNO.ToUpper().Contains(searchValue)
                                           || (m.CASHID.ToString().Contains(searchValue))
                                           || (m.LC.LCNO != null && m.LC.LCNO.Contains(searchValue))
                                           || (m.ITEMQTY_YDS != null && m.ITEMQTY_YDS.Contains(searchValue))
                                           || (m.VCNO != null && m.VCNO.Contains(searchValue))
                                           || (m.RATE != null && m.RATE.Contains(searchValue))
                                           || (m.BACKTOBACK_LCTYPE != null && m.BACKTOBACK_LCTYPE.Contains(searchValue))
                                           || (m.RCVDDATE != null && m.RCVDDATE.ToString().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
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
        [Route("CommercialExport/BTMA/GetAll")]
        public IActionResult GetCashInfoWithPaged()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateCashInfo()
        {
            return View(await _comExCashInfo.GetInitObjects(new ComExCashInfoViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCashInfo(ComExCashInfoViewModel comExCashInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isCashInfoInsert = await _comExCashInfo.InsertByAsync(comExCashInfoViewModel.ComExCashInfo);
                    if (isCashInfoInsert)
                    {
                        TempData["message"] = "Successfully added Cash Information.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
                    }

                    TempData["message"] = "Failed to Add Cash Information";
                    TempData["type"] = "error";
                    return View(await _comExCashInfo.GetInitObjects(comExCashInfoViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _comExCashInfo.GetInitObjects(comExCashInfoViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Cash Information";
                TempData["type"] = "error";
                return View(await _comExCashInfo.GetInitObjects(comExCashInfoViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCashInfo(string id)
        {
            try
            {
                var cashInfo = await _comExCashInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (cashInfo != null)
                {
                    cashInfo.EncryptedId = _protector.Protect(cashInfo.CASHID.ToString());
                    var comExCashInfoViewModel = new ComExCashInfoViewModel
                    {
                        ComExCashInfo = cashInfo
                    };
                    return View(await _comExCashInfo.GetInitObjects(comExCashInfoViewModel));
                }

                TempData["message"] = "Cash Information Not Found.";
                TempData["type"] = "error";
                return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCashInfo(ComExCashInfoViewModel comExCashInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (comExCashInfoViewModel.ComExCashInfo.CASHID == int.Parse(_protector.Unprotect(comExCashInfoViewModel.ComExCashInfo.EncryptedId)))
                    {
                        comExCashInfoViewModel.ComExCashInfo.CASHID = int.Parse(_protector.Unprotect(comExCashInfoViewModel.ComExCashInfo.EncryptedId));

                        var isGspInfoUpdated = await _comExCashInfo.Update(comExCashInfoViewModel.ComExCashInfo);

                        if (isGspInfoUpdated)
                        {
                            TempData["message"] = "Successfully Updated Cash Information.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
                        }
                        TempData["message"] = "Failed to Update Cash Information.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
                    }
                    TempData["message"] = "Invalid Cash No";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _comExCashInfo.GetInitObjects(comExCashInfoViewModel));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Update Cash Information.";
                TempData["type"] = "error";
                return View(await _comExCashInfo.GetInitObjects(comExCashInfoViewModel));
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteCashInfo(string id)
        {
            try
            {
                var gspInfo = await _comExCashInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (gspInfo != null)
                {
                    if (await _comExCashInfo.Delete(gspInfo))
                    {
                        TempData["message"] = "Successfully Deleted Cash.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
                    }
                    TempData["message"] = "Failed to Delete Cash.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
                }
                TempData["message"] = "Cash Information Not Found!.";
                TempData["type"] = "error";
                return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
            }
            catch (Exception e)
            {
                TempData["message"] = e.Message;
                TempData["type"] = "error";
                return RedirectToAction($"GetCashInfoWithPaged", $"ComExCashInfo");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetLcInfo(int lcId)
        {
            try
            {
                return Ok(await _comExCashInfo.FindLCByIdAsync(lcId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Reports/CommercialExport/Cash/{certificateNo:int?}")]
        public IActionResult RComExCashInfo(int certificateNo)
        {
            return View(certificateNo);
        }

        [HttpGet]
        [Route("Reports/CommercialExport/Cash/Deemed/{certificateNo:int?}")]
        public IActionResult RComExCashInfoDeemed(int certificateNo)
        {
            return View(certificateNo);
        }

    }
}