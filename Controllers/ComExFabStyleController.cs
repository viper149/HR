using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ComExFabStyleController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ICOM_EX_FABSTYLE _cOmExFabstyle;
        private readonly IBAS_BRANDINFO _bAsBrandinfo;
        private readonly IRND_FABRICINFO _rNdFabricinfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComExFabStyleController(
        IDataProtectionProvider dataProtectionProvider,
        DataProtectionPurposeStrings dataProtectionPurposeStrings,
        ICOM_EX_FABSTYLE cOM_EX_FABSTYLE,
        IBAS_BRANDINFO bAS_BRANDINFO,
        IRND_FABRICINFO rND_FABRICINFO,
        UserManager<ApplicationUser> userManager)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _cOmExFabstyle = cOM_EX_FABSTYLE;
            _bAsBrandinfo = bAS_BRANDINFO;
            _rNdFabricinfo = rND_FABRICINFO;
            _userManager = userManager;
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
                
                var forDataTableByAsync = await _cOmExFabstyle.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);
                
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
        [Route("CommercialExportFabricStyle/GetAll")]
        public IActionResult GetComEXFabStyleWithPaged()
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
        public async Task<List<COM_EX_PIMASTER>> GetPIByStyleIdList(int styleId)
        {
            try
            {
                var fabStyleInfo = await _cOmExFabstyle.FindByIdAsync(styleId);
                if (fabStyleInfo != null)
                {
                    var result = await _cOmExFabstyle.FindPIListByStyleIdAsync(styleId);

                    if (result != null)
                    {
                        return result.ToList();
                        //return View(result);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateComEXFabStyle()
        {
            return View(await _cOmExFabstyle.GetInitObjects(new ComExFabStyleViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComEXFabStyle(ComExFabStyleViewModel fabStyle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    fabStyle.cOM_EX_FABSTYLE.USRID = user.Id;
                    var result = await _cOmExFabstyle.InsertByAsync(fabStyle.cOM_EX_FABSTYLE);

                    if (result == true)
                    {
                        TempData["message"] = "Successfully Added Fabric Style Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                    }
                    else
                    {
                        TempData["message"] = "Failed to Add Fabric Style Information.";
                        TempData["type"] = "error";
                        return View(fabStyle);
                    }
                }
                TempData["message"] = "Please Enter Valid Fabric Style Information.";
                TempData["type"] = "error";
                return View(fabStyle);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Fabric Style Information.";
                TempData["type"] = "error";
                return View(fabStyle);
            }
        }

        [HttpGet]
        public async Task<ComExFabStyleViewModel> GetFabricInfo(int fabCode)
        {
            try
            {
                var result = await _cOmExFabstyle.GetFabricInfoAsync(fabCode);
                return result ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComFabStyleInfo(string styleId)
        {
            try
            {
                var result = await _cOmExFabstyle.DeleteFabStyle(int.Parse(_protector.Unprotect(styleId)));

                if (result)
                {
                    TempData["message"] = "Successfully Deleted Fabric Style Information.";
                    TempData["type"] = "success";
                    return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                }
                else
                {
                    TempData["message"] = "Failed to Delete Fabric Style Information.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Fabric Style Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsComFabStyleInfo(string styleId)
        {
            try
            {
                var result = await getResult(int.Parse(_protector.Unprotect(styleId)));
                if (result == null) return RedirectToAction($"GetComEXFabStyleWithPaged", $"ComExFabStyle");
                result.cOM_EX_FABSTYLE.EncryptedId = _protector.Protect(result.cOM_EX_FABSTYLE.STYLEID.ToString());
                return View(result);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Fabric Style Information.";
                TempData["type"] = "error";
                return RedirectToAction($"GetComEXFabStyleWithPaged", $"ComExFabStyle");
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditComFabStyleInfo(string styleId)
        {
            try
            {
                var result = await getResult(int.Parse(_protector.Unprotect(styleId)));
                    result = await _cOmExFabstyle.GetInitObjects(result);
                if (result == null) return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                result.cOM_EX_FABSTYLE.EncryptedId = _protector.Protect(result.cOM_EX_FABSTYLE.STYLEID.ToString());
                return View(result);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Fabric Style Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditComFabStyleInfo(ComExFabStyleViewModel fabStyle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var decryptedId = _protector.Unprotect(fabStyle.cOM_EX_FABSTYLE.EncryptedId);
                    var decryptedIntId = Convert.ToInt32(decryptedId);

                    var supplierInfoPrevious = await _cOmExFabstyle.FindByIdAsync(decryptedIntId);

                    if (supplierInfoPrevious != null)
                    {
                        var isUpdated = await _cOmExFabstyle.Update(fabStyle.cOM_EX_FABSTYLE);
                        if (isUpdated)
                        {
                            TempData["message"] = "Successfully Updated Style Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                        }
                        else
                        {
                            TempData["message"] = "Failed to Updated Style Information.";
                            TempData["type"] = "error";

                            var result = await getResult(decryptedIntId);

                            if (result != null)
                            {
                                result.cOM_EX_FABSTYLE.EncryptedId = _protector.Protect(result.cOM_EX_FABSTYLE.STYLEID.ToString());
                                return View(result);
                            }
                            return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Failed to Updated Style Information.";
                        TempData["type"] = "error";

                        var result = await getResult(decryptedIntId);
                        if (result != null)
                        {
                            result.cOM_EX_FABSTYLE.EncryptedId = _protector.Protect(result.cOM_EX_FABSTYLE.STYLEID.ToString());
                            return View(result);
                        }
                        return RedirectToAction("GetComEXFabStyleWithPaged", $"ComExFabStyle");
                    }
                }
                else
                {
                    TempData["message"] = "Failed to Updated Style Information.";
                    TempData["type"] = "error";

                    var basBrandInfos = await _bAsBrandinfo.GetAll();
                    fabStyle.bAS_BRANDINFOs = basBrandInfos.ToList();

                    var rndFabricInfos = await _rNdFabricinfo.GetAll();
                    fabStyle.rND_FABRICINFOs = rndFabricInfos.ToList();

                    return View(fabStyle);
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Updated Style Information.";
                TempData["type"] = "error";

                var basBrandInfos = await _bAsBrandinfo.GetAll();
                fabStyle.bAS_BRANDINFOs = basBrandInfos.ToList();

                var rndFabricInfos = await _rNdFabricinfo.GetAll();
                fabStyle.rND_FABRICINFOs = rndFabricInfos.ToList();

                return View(fabStyle);
            }
        }

        public async Task<ComExFabStyleViewModel> getResult(int decryptedIntId)
        {
            try
            {
                var comFabricStyleInfo = await _cOmExFabstyle.GetComExFabricInfoAsync(decryptedIntId);
                var result = await _cOmExFabstyle.GetFabricInfoAsync(comFabricStyleInfo.FABCODE);
                var basBrandInfos = await _bAsBrandinfo.GetAll();

                result.bAS_BRANDINFOs = basBrandInfos.ToList();

                var rndFabricInfos = await _rNdFabricinfo.GetAll();
                result.rND_FABRICINFOs = rndFabricInfos.ToList();
                result.cOM_EX_FABSTYLE = comFabricStyleInfo;

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}