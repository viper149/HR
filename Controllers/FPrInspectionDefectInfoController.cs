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
    public class FPrInspectionDefectInfoController : Controller
        {
            private readonly IDataProtector _protector;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IF_PR_INSPECTION_DEFECTINFO _fPrInspectionDefectinfo;

           


            public FPrInspectionDefectInfoController(IDataProtectionProvider dataProtectionProvider,
                DataProtectionPurposeStrings dataProtectionPurposeStrings,
                UserManager<ApplicationUser> userManager,
                IF_PR_INSPECTION_DEFECTINFO fPrInspectionDefectinfo

                )
            {
                _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
                _userManager = userManager;
                _fPrInspectionDefectinfo = fPrInspectionDefectinfo;
               
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

                    var data = await _fPrInspectionDefectinfo.GetAll();

                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    {
                        data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        data = data.Where(m =>
                                                    m.NAME != null && m.NAME.ToString().ToUpper().Contains(searchValue)
                                                    || (m.CODE != null && m.CODE.ToUpper().Contains(searchValue))
                                                || (m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue))
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
            public IActionResult GetFPrInspectionDefectInfoList()
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
            public async Task<IActionResult> CreateFPrInspectionDefectInfo()
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
            public async Task<IActionResult> CreateFPrInspectionDefectInfo(F_PR_INSPECTION_DEFECTINFO fPrInspectionDefectinfo)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var result = await _fPrInspectionDefectinfo.InsertByAsync(fPrInspectionDefectinfo);

                        if (result)
                        {
                            TempData["message"] = "Successfully Added YS RAW Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetFPrInspectionDefectInfoList", $"FPrInspectionDefectInfo");
                        }

                        TempData["message"] = "Failed to Add ";
                        TempData["type"] = "error";
                        return View(fPrInspectionDefectinfo);
                    }

                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(fPrInspectionDefectinfo);
                }
                catch (Exception)
                {
                    TempData["message"] = "Failed to Add ";
                    TempData["type"] = "error";
                    return View(fPrInspectionDefectinfo);
                }
            }


        }
    }
