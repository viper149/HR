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
    public class RndFinishMcController : Controller
    {
        private readonly IRND_FINISHMC _rndFinishmc;
        private readonly IDataProtector _protector;

        public RndFinishMcController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_FINISHMC rndFinishmc
        )
        {
            _rndFinishmc = rndFinishmc;
            this._protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsFinishMcInUse(RND_FINISHMC rndFinishmc)
        {
            var type = await _rndFinishmc.FindByTypeName(rndFinishmc.NAME);
            return !type ? Json(true) : Json($"Finish MC Name [ {rndFinishmc.NAME} ] is already in use");
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
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = await _rndFinishmc.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.MCID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.NAME.ToUpper().Contains(searchValue)
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
        public IActionResult GetRndFinishMcWithPaged()
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
        public async Task<IActionResult> DeleteFinishMc(string mcId)
        {
            try
            {
                string decryptedId = _protector.Unprotect(mcId);
                var decryptedIntId = Convert.ToInt32(decryptedId);
                var finishMc = await _rndFinishmc.FindByIdAsync(decryptedIntId);
                if (finishMc != null)
                {
                    var result = await _rndFinishmc.Delete(finishMc);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Finish MC.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
                    }
                    TempData["message"] = "Failed to Delete Finish MC.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
                }
                TempData["message"] = "Finish MC Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Finish MC.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
            }
        }


        [HttpGet]
        public IActionResult CreateFinishMc()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFinishMc(RND_FINISHMC rndFinishMc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rndFinishmc.InsertByAsync(rndFinishMc);

                    if (result == true)
                    {
                        TempData["message"] = "Successfully Added Finish MC.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Finish MC.";
                        TempData["type"] = "error";
                        return View(rndFinishMc);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(rndFinishMc);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Finish Type.";
                TempData["type"] = "error";
                return View(rndFinishMc);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFinishMc(string mcId)
        {
            try
            {
                string decryptedId = _protector.Unprotect(mcId);
                var decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await _rndFinishmc.FindByIdAsync(decryptedIntId);
                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.MCID.ToString());
                    ModelState.Clear();
                    return View(result);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Finish MC.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Finish MC.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditFinishMc(RND_FINISHMC rndFinishMc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string decryptedId = _protector.Unprotect(rndFinishMc.EncryptedId);
                    var decryptedIntId = Convert.ToInt32(decryptedId);

                    var type = await _rndFinishmc.FindByIdAsync(decryptedIntId);
                    if (type != null)
                    {
                        var result = await _rndFinishmc.Update(rndFinishMc);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Finish MC.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetRndFinishMcWithPaged", "RndFinishMc");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Finish MC.";
                            TempData["type"] = "error";
                            return View(rndFinishMc);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Update Finish MC.";
                        TempData["type"] = "error";
                        return View(rndFinishMc);
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input, Please Try Again.";
                    TempData["type"] = "error";
                    return View(rndFinishMc);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Finish MC.";
                TempData["type"] = "error";
                return View(rndFinishMc);
            }
        }
        // RND FINISH MC ENDED HERE
    }
}