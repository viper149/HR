using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    //[Authorize]
    //[Route("ComExGspInfo")]
    public class ComExGspInfoController : Controller
    {
        private readonly ICOM_EX_GSPINFO _comExGspInfo;
        private readonly ICOM_EX_INVOICEMASTER _comExInvoiceMaster;
        private readonly ICOM_EX_LCINFO _comExLcinfo;
        private readonly ICOM_EX_INVDETAILS _comExInvDetails;
        private readonly ICOM_EX_FABSTYLE _comExFabstyle;
        private readonly IDataProtector _protector;

        public ComExGspInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_EX_GSPINFO comExGspInfo,
            ICOM_EX_INVOICEMASTER comExInvoiceMaster,
            ICOM_EX_LCINFO comExLcinfo,
            ICOM_EX_INVDETAILS comExInvDetails,
            ICOM_EX_FABSTYLE comExFabstyle
        )
        {
            _comExGspInfo = comExGspInfo;
            _comExInvoiceMaster = comExInvoiceMaster;
            _comExLcinfo = comExLcinfo;
            _comExInvDetails = comExInvDetails;
            _comExFabstyle = comExFabstyle;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("CommercialExport/GSP/GetForGSPInformation")]
        public async Task<IActionResult> GetForGSPInformation(ComExGspInfoViewModel comExGspInfoViewModel)
        {
            return Ok(await _comExGspInfo.GetForGSPInformationByAsync(comExGspInfoViewModel.ComExGspInfo.INVID));
        }
        [Route("CommercialExport/GSP/GetGSPNo")]
        public async Task<IActionResult> GetGSPNo(ComExGspInfoViewModel comExGspInfoViewModel)
        {
            return Ok(await _comExGspInfo.GetGSPNo(comExGspInfoViewModel.ComExGspInfo.GSPNO));
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
                var data = await _comExGspInfo.GetAllForDataTableByAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.GSPNO != null && m.GSPNO.ToUpper().Contains(searchValue)
                               || (m.INV.INVNO != null && m.INV.INVNO.ToUpper().Contains(searchValue))
                               || (m.INV.BUYER.BUYER_NAME != null && m.INV.BUYER.BUYER_NAME.Contains(searchValue))
                               || (m.INV.LC.FILENO != null && m.INV.LC.FILENO.Contains(searchValue))
                               || (m.ISSUEDATE != null && m.ISSUEDATE.ToString().Contains(searchValue))
                               || (m.SUBDATE != null && m.SUBDATE.ToString().Contains(searchValue))
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
        [Route("CommercialExport/GSP/GetAll")]
        public IActionResult GetGspInfoWithPaged()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateGspInfo()
        {
            return View(await GetInfo(new ComExGspInfoViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGspInfo(ComExGspInfoViewModel comExGspInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    comExGspInfoViewModel.ComExGspInfo.ISSUEDATE = DateTime.Now;
                    var isGspInfoInsert = await _comExGspInfo.InsertByAsync(comExGspInfoViewModel.ComExGspInfo);
                    if (isGspInfoInsert)
                    {
                        TempData["message"] = "Successfully added GSP Information.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add GSP Information";
                        TempData["type"] = "error";
                        return View(await GetInfo(comExGspInfoViewModel));
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input. Please Try Again.";
                    TempData["type"] = "error";
                    return View(await GetInfo(comExGspInfoViewModel));
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add GSP Information";
                TempData["type"] = "error";
                return View(await GetInfo(comExGspInfoViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsGspInfo(string gspId)
        {
            try
            {
                int decryptedId = Int32.Parse(_protector.Unprotect(gspId));

                LcInfoViewModel lcInfoViewModel = new LcInfoViewModel();

                var gspInfo = await _comExGspInfo.FindByIdAsync(decryptedId);

                if (gspInfo != null)
                {
                    gspInfo.EncryptedId = _protector.Protect(gspInfo.GSPID.ToString());
                    lcInfoViewModel.ComExGspInfo = gspInfo;
                    lcInfoViewModel.ComExInvoiceMaster = await _comExInvoiceMaster.FindByIdIncludeAllNotRealizedAsync(gspInfo.INVID);
                    lcInfoViewModel.ComExLcInfo = await _comExLcinfo.FindByIdAsync(lcInfoViewModel.ComExInvoiceMaster.LCID ?? 0);
                    return View(lcInfoViewModel);
                }
                else
                {
                    TempData["message"] = "GSP Information Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve GSP Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditGspInfo(string gspId)
        {
            try
            {
                int decryptedId = int.Parse(_protector.Unprotect(gspId));
                var gspInfo = await _comExGspInfo.FindByIdAsync(decryptedId);

                if (gspInfo != null)
                {
                    gspInfo.EncryptedId = _protector.Protect(gspInfo.GSPID.ToString());
                    ComExGspInfoViewModel comExGspInfoViewModel = new ComExGspInfoViewModel();
                    comExGspInfoViewModel.ComExGspInfo = gspInfo;
                    return View(await GetInfo(comExGspInfoViewModel));
                }
                else
                {
                    TempData["message"] = "GSP Information Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve GSP Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditGspInfo(ComExGspInfoViewModel comExGspInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (comExGspInfoViewModel.ComExGspInfo.GSPID == Int32.Parse(_protector.Unprotect(comExGspInfoViewModel.ComExGspInfo.EncryptedId)))
                    {
                        comExGspInfoViewModel.ComExGspInfo.GSPID = Int32.Parse(_protector.Unprotect(comExGspInfoViewModel.ComExGspInfo.EncryptedId));

                        var isGspInfoUpdated = await _comExGspInfo.Update(comExGspInfoViewModel.ComExGspInfo);

                        if (isGspInfoUpdated)
                        {
                            TempData["message"] = "Successfully Updated GSP Information.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                        }
                        TempData["message"] = "Failed to Update GSP Information.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                    }
                    TempData["message"] = "Invalid GSP No";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(await GetInfo(comExGspInfoViewModel)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Update GSP Information.";
                TempData["type"] = "error";
                return View(await GetInfo(await GetInfo(comExGspInfoViewModel)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteGspInfo(string gspId)
        {
            try
            {
                string decryptedId = _protector.Unprotect(gspId);
                var decryptedIntId = Convert.ToInt32(decryptedId);
                var gspInfo = await _comExGspInfo.FindByIdAsync(decryptedIntId);
                if (gspInfo != null)
                {
                    var result = await _comExGspInfo.Delete(gspInfo);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted GSP.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Delete GSP.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                    }
                }
                else
                {
                    TempData["message"] = "GSP Information Not Found!.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete GSP.";
                TempData["type"] = "error";
                return RedirectToAction($"GetGspInfoWithPaged", $"ComExGspInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceInfo(int invId)
        {
            try
            {
                var lcInfoViewModel = new LcInfoViewModel
                {
                    ComExInvoiceMaster = await _comExInvoiceMaster.FindByIdIncludeAllNotRealizedAsync(invId)
                };

                return Ok(lcInfoViewModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<ComExGspInfoViewModel> GetInfo(ComExGspInfoViewModel comExGspInfoViewModel)
        {
            var comExInvoiceMasters = await _comExInvoiceMaster.GetInvoiceList();
            //var comExInvoiceMasters = await _comExInvoiceMaster.GetAll();
            comExGspInfoViewModel.ComExInvoiceMasters = comExInvoiceMasters.OrderByDescending(c => c.INVID).ToList();
            return comExGspInfoViewModel;
        }
        
        [HttpGet]
        [Route("ComExGspInfo/GSPReport/{invid?}")]
        public async Task<IActionResult> GSPReport(string invid)
        {
            var invno = string.Empty;

            if (invid != null)
            {
                invno = await Task.Run(() => _comExInvoiceMaster.FindByIdAsync(int.Parse(_protector.Unprotect(invid))).Result.INVNO);
            }

            return View(model: invno);
        }
        
    }
}