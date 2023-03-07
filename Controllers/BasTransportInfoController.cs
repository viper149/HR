using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic.TransportInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    [Route("Transport")]
    public class BasTransportInfoController : Controller
    {
        private readonly IBAS_TRANSPORTINFO _basTransportinfo;
        private readonly IDataProtector _protector;

        public BasTransportInfoController(IBAS_TRANSPORTINFO basTransportinfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _basTransportinfo = basTransportinfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{trnspId?}")]
        public async Task<IActionResult> DetailsBasTransportInfo(string trnspId)
        {
            return View(await _basTransportinfo.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(trnspId))));
        }

        [HttpPost]
        [Route("PostEdit")]
        public async Task<IActionResult> PostEditBasTransportInfo(BasTransportInfoViewModel basTransportInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                var findByIdAsync = await _basTransportinfo.FindByIdAsync(int.Parse(_protector.Unprotect(basTransportInfoViewModel.BasTransportinfo.EncryptedId)));

                if (findByIdAsync != null)
                {
                    basTransportInfoViewModel.BasTransportinfo.TRNSPID = findByIdAsync.TRNSPID;
                    
                    if (await _basTransportinfo.Update(basTransportInfoViewModel.BasTransportinfo))
                    {
                        TempData["message"] = "Transport Information Successfully Updated.";
                        TempData["type"] = "success";

                        return RedirectToAction(nameof(GetBasTransportInfo), $"BasTransportInfo");
                    }
                    else
                    {
                        TempData["message"] = "Transport Information Failed To Update.";
                        TempData["type"] = "error";

                        return RedirectToAction(nameof(EditBasTransportInfo), basTransportInfoViewModel);
                    }
                }
            }

            TempData["message"] = "Transport Information Failed To Update.";
            TempData["type"] = "error";

            return RedirectToAction(nameof(EditBasTransportInfo), basTransportInfoViewModel);
        }

        [HttpGet]
        [Route("Edit/{trnspId?}")]
        public async Task<IActionResult> EditBasTransportInfo(string trnspId)
        {
            return View(await _basTransportinfo.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(trnspId))));
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateBasTransportInfo()
        {
            return View();
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateBasTransportInfo(BasTransportInfoViewModel basTransportInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _basTransportinfo.InsertByAsync(basTransportInfoViewModel.BasTransportinfo))
                {

                    TempData["message"] = "Transport Information Successfully Inserted.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetBasTransportInfo), $"BasTransportInfo");
                }

                TempData["message"] = "Transport Information Failed To Insert. Please Try Again Later.";
                TempData["type"] = "error";
                return View(nameof(GetBasTransportInfo));
            }

            TempData["message"] = "Transport Information Failed To Insert. Please Try Again Later.";
            TempData["type"] = "error";
            return View(nameof(GetBasTransportInfo));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetBasTransportInfo()
        {
            return View();
        }

        [HttpPost]
        [Route("GetTableData")]
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

                var data = await _basTransportinfo.GetAllForDataTables();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.TRNSPNAME.ToString().ToUpper().Contains(searchValue)
                                        || m.CPERSON != null && m.CPERSON.ToString().Contains(searchValue)
                                        || m.ADDRESS != null && m.ADDRESS.ToString().ToUpper().Contains(searchValue)
                                        || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
    }
}
