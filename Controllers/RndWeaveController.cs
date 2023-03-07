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
    public class RndWeaveController : Controller
    {
        private readonly IRND_WEAVE _rndWeave;
        private readonly IDataProtector _protector;

        public RndWeaveController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_WEAVE rndWeave
            )
        {
            _rndWeave = rndWeave;
            this._protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        // RND WEAVE STARTED HERE

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsWeaveInUse(RND_WEAVE rndWeave)
        {
            var type = await _rndWeave.FindByTypeName(rndWeave.NAME);

            if (!type)
            {
                return Json(true);
            }
            else
            {
                return Json($"Weave Name [ {rndWeave.NAME} ] is already in use");
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault().ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = await _rndWeave.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.WID.ToString());
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
        public IActionResult GetRndWeaveWithPaged()
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
        public async Task<IActionResult> DeleteWeave(string wId)
        {
            try
            {
                string decryptedId = _protector.Unprotect(wId);
                var decryptedIntId = Convert.ToInt32(decryptedId);
                var weave = await _rndWeave.FindByIdAsync(decryptedIntId);
                if (weave != null)
                {
                    var result = await _rndWeave.Delete(weave);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Weave.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
                    }
                    TempData["message"] = "Failed to Delete Weave.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
                }
                TempData["message"] = "Weave Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Weave.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
            }
        }

        [HttpGet]
        public IActionResult CreateWeave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeave(RND_WEAVE rndWeave)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rndWeave.InsertByAsync(rndWeave);

                    if (result == true)
                    {
                        TempData["message"] = "Successfully Added Weave.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Weave.";
                        TempData["type"] = "error";
                        return View(rndWeave);
                    }
                }
                else
                {
                    TempData["message"] = "Please Fill All The Fields with Valid Data.";
                    TempData["type"] = "error";
                    return View(rndWeave);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Weave.";
                TempData["type"] = "error";
                return View(rndWeave);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditWeave(string wId)
        {
            try
            {
                string decryptedId = _protector.Unprotect(wId);
                var decryptedIntId = Convert.ToInt32(decryptedId);

                var result = await _rndWeave.FindByIdAsync(decryptedIntId);
                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.WID.ToString());
                    ModelState.Clear();
                    return View(result);
                }
                else
                {
                    TempData["message"] = "Failed to Retrieve Weave.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Weave.";
                TempData["type"] = "error";
                return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditWeave(RND_WEAVE rndWeave)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string decryptedId = _protector.Unprotect(rndWeave.EncryptedId);
                    var decryptedIntId = Convert.ToInt32(decryptedId);

                    var type = await _rndWeave.FindByIdAsync(decryptedIntId);
                    if (type != null)
                    {
                        var result = await _rndWeave.Update(rndWeave);
                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Weave.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetRndWeaveWithPaged", "RndWeave");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Update Weave.";
                            TempData["type"] = "error";
                            return View(rndWeave);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Update Weave.";
                        TempData["type"] = "error";
                        return View(rndWeave);
                    }
                }
                else
                {
                    TempData["message"] = "Invalid Input, Please Try Again.";
                    TempData["type"] = "error";
                    return View(rndWeave);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Weave.";
                TempData["type"] = "error";
                return View(rndWeave);
            }
        }
        // RND WEAVE ENDED HERE

    }
}