using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FYsRawPerController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IF_YS_RAW_PER _fYsRawPer;
       

        public FYsRawPerController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_YS_RAW_PER fYsRawPer
            
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fYsRawPer = fYsRawPer;
            
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var recordsTotal = 0;

                var data = await _fYsRawPer.GetAll();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                                m.RAWPER != null && m.RAWPER.ToString().ToUpper().Contains(searchValue)
                                            || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                //foreach (var item in finalData)
                //{
                //    item.EncryptedId = _protector.Protect(item.FN_PROCESSID.ToString());
                //}
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = finalData };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult GetYsRawPerList()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateYsRawPer()
        {
            try
            {
               

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateYsRawPer(F_YS_RAW_PER fYsRawPer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fYsRawPer.InsertByAsync(fYsRawPer);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added YS RAW Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetYsRawPerList", $"FYsRawPer");
                    }

                    TempData["message"] = "Failed to Add YS RAW   Information.";
                    TempData["type"] = "error";
                    return View(fYsRawPer);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fYsRawPer);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add YS RAW  Information.";
                TempData["type"] = "error";
                return View(fYsRawPer);
            }
        }

        


    }
}
