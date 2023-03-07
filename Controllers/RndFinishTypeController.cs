using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndFinishTypeController : Controller
    {
        private readonly IRND_FINISHTYPE _rndFinishtype;
        private readonly IDataProtector _protector;

        public RndFinishTypeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_FINISHTYPE rndFinishtype)
        {
            _rndFinishtype = rndFinishtype;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
 
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsFinishTypeInUse(RND_FINISHTYPE rndFinishtype)
        {
            var type = await _rndFinishtype.FindByTypeName(rndFinishtype.TYPENAME);
            return !type ? Json(true) : Json($"Type Name [ {rndFinishtype.TYPENAME} ] is already in use");
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
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = await _rndFinishtype.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.FINID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.TYPENAME.ToUpper().Contains(searchValue)
                                           || m.COST.ToString().Contains(searchValue)
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                data = data.ToList();
                recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

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
        public IActionResult GetRndFinishTypeWithPaged()
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
        public async Task<IActionResult> DeleteFinishType(string fId)
        {
            try
            {
                var finishType= await _rndFinishtype.FindByIdAsync(int.Parse(_protector.Unprotect(fId)));

                if (finishType != null)
                {
                    var result = await _rndFinishtype.Delete(finishType);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Finish Type.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Delete Finish Type.";
                        TempData["type"] = "error";
                        return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
                    }
                }
                else
                {
                    TempData["message"] = "Finish Type Not Found.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Finish Type.";
                TempData["type"] = "error";
                return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
            }
        }
        
        [HttpGet]
        public IActionResult CreateFinishType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFinishType(RND_FINISHTYPE rndFinishtype)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rndFinishtype.InsertByAsync(rndFinishtype);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Finish Type.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Finish Type.";
                        TempData["type"] = "error";
                        return View(rndFinishtype);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(rndFinishtype);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Finish Type.";
                TempData["type"] = "error";
                return View(rndFinishtype);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFinishType(string fId)
        {
            try
            {
                var result = await _rndFinishtype.FindByIdAsync(int.Parse(_protector.Unprotect(fId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.FINID.ToString());
                    return View(result);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Finish Type.";
                    TempData["type"] = "error";
                    return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Finish Type.";
                TempData["type"] = "error";
                return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditFinishType(RND_FINISHTYPE finishtype)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var type = await _rndFinishtype.FindByIdAsync(int.Parse(_protector.Unprotect(finishtype.EncryptedId)));

                    if (type != null)
                    {
                        var result = await _rndFinishtype.Update(finishtype);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Finish Type.";
                            TempData["type"] = "success";
                            return RedirectToAction($"GetRndFinishTypeWithPaged", $"RndFinishType");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Finish Type.";
                            TempData["type"] = "error";
                            return View(finishtype);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Update Finish Type.";
                        TempData["type"] = "error";
                        return View(finishtype);
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input, Please Try Again.";
                    TempData["type"] = "error";
                    return View(finishtype);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Finish Type.";
                TempData["type"] = "error";
                return View(finishtype);
            }
        }
    }
}