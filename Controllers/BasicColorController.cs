using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class BasicColorController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_COLOR _bAsColor;

        public BasicColorController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              IBAS_COLOR bAS_COLOR)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsColor = bAS_COLOR;
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

                var data = await _bAsColor.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.COLORCODE.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.COLOR.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)
                                    ).ToList();
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

        [HttpGet]
        public IActionResult BasicColors()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateColor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateColor(ColorViewModel colorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (colorModel != null)
                    {
                        var color = new BAS_COLOR
                        {
                            COLOR = colorModel.COLOR,
                            REMARKS = colorModel.REMARKS
                        };

                        var result = await _bAsColor.InsertByAsync(color);

                        if (result)
                        {
                            TempData["message"] = "Successfully Added Color.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(BasicColors));
                        }

                        TempData["message"] = "Failed to Add Color.";
                        TempData["type"] = "error";
                        return View(colorModel);
                    }

                    TempData["message"] = "Failed to Add Color.";
                    TempData["type"] = "error";
                    return View(colorModel);
                }
                TempData["message"] = "Invalid Input.";
                TempData["type"] = "error";
                return View(colorModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["message"] = "Failed to Add Color.";
                TempData["type"] = "error";
                return View(colorModel);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditColor(string id)
        {
            try
            {
                var result = await _bAsColor.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.COLORCODE.ToString());
                    return View(result);
                }
                TempData["message"] = "Failed to Retrieve Color.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(BasicColors));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Color.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(BasicColors));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditColor(BAS_COLOR basColor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var color = await _bAsColor.FindByIdAsync(int.Parse(_protector.Unprotect(basColor.EncryptedId)));

                    if (color != null)
                    {
                        var result = await _bAsColor.Update(basColor);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Color.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(BasicColors));
                        }

                        TempData["message"] = "Failed to Update Color.";
                        TempData["type"] = "error";
                        return View(basColor);
                    }

                    TempData["message"] = "Failed to Update Color.";
                    TempData["type"] = "error";
                    return View(basColor);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(basColor);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Color.";
                TempData["type"] = "error";
                return View(basColor);
            }
        }
        
        //[HttpGet]
        //public async Task<IActionResult> Find(string id)
        //{
        //    var decryptedIntId = 0;
        //    try
        //    {
        //        decryptedIntId = int.Parse(_protector.Unprotect(id));
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.ErrorMessage = $"Invalid input [ {id} ], not found!";
        //        return View($"NotFound");
        //    }

        //    var result = await _bAsColor.FindByIdAsync(decryptedIntId);

        //    if (result != null)
        //    {
        //        result.EncryptedId = _protector.Protect(result.COLORCODE.ToString());
        //        return View(result);
        //    }

        //    return View("Error");
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var color = await _bAsColor.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (color != null)
                {
                    var result = await _bAsColor.Delete(color);

                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Color.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(BasicColors));
                    }

                    TempData["message"] = "Failed to Delete Color.";
                    TempData["type"] = "error";
                    return RedirectToAction(nameof(BasicColors));
                }

                TempData["message"] = "Color Not Found.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(BasicColors));
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Color.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(BasicColors));
            }
        }
    }
}