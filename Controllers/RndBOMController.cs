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
    public class RndBOMController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRND_BOM _rndBom;
        private readonly IRND_BOM_MATERIALS_DETAILS _rndBomMaterialsDetails;
        private readonly string title = "BOM Information";


        public RndBOMController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IRND_BOM rndBom,
            IRND_BOM_MATERIALS_DETAILS rndBomMaterialsDetails
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _rndBom = rndBom;
            _rndBomMaterialsDetails = rndBomMaterialsDetails;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetRndBomInfo()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        //[Route("GetTableData")]
        public async Task<JsonResult> GetTableData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;

            var data = (List<RND_BOM>)await _rndBom.GetAllRndBomInfoAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.TRNSDATE != null && m.TRNSDATE.ToString().ToUpper().Contains(searchValue)
                                       || m.OPT1 != null && m.OPT1 != null && m.OPT1.ToString().ToUpper().Contains(searchValue)
                                       || m.FINISH_TYPENavigation.TYPENAME != null && m.FINISH_TYPENavigation.TYPENAME.ToString().ToUpper().Contains(searchValue)
                                       || m.COLORNavigation.COLOR != null && m.COLORNavigation.COLOR.ToString().ToUpper().Contains(searchValue)
                                       || m.TOTAL_ENDS != null && m.TOTAL_ENDS.ToString().ToUpper().Contains(searchValue)
                                       || m.LOT_RATIO != null && m.LOT_RATIO.ToString().ToUpper().Contains(searchValue)
                                       || m.WIDTH != null && m.WIDTH.ToString().ToUpper().Contains(searchValue)
                                       || m.SETNO != null && m.SETNO.ToString().ToUpper().Contains(searchValue)
                                       || m.PROG_NO != null && m.PROG_NO.ToString().ToUpper().Contains(searchValue)
                                       || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)).ToList();
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


        public async Task<IActionResult> CreateRndBOM()
        {
            return View(await GetInfo(new RndBomViewModel()));
        }

        public async Task<RndBomViewModel> GetInfo(RndBomViewModel rndBomViewModel)
        {
            rndBomViewModel = await _rndBom.GetInitObjects(rndBomViewModel);
            rndBomViewModel.RndBom = new RND_BOM()
            {
                TRNSDATE = DateTime.Now,
            };
            return rndBomViewModel;
        }


        [HttpPost]
        public async Task<IActionResult> CreateRndBOM(RndBomViewModel rndBomViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    rndBomViewModel.RndBom.CREATED_BY = user.Id;
                    rndBomViewModel.RndBom.UPDATED_BY = user.Id;
                    rndBomViewModel.RndBom.CREATED_AT = DateTime.Now;
                    rndBomViewModel.RndBom.UPDATED_AT = DateTime.Now;
                    var bom = await _rndBom.GetInsertedObjByAsync(rndBomViewModel.RndBom);

                    if (bom != null)
                    {
                        foreach (var item in rndBomViewModel.RndBomMaterialsDetailsList)
                        {
                                item.BOMID = bom.BOMID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _rndBomMaterialsDetails.InsertByAsync(item);
                        }
                        
                        TempData["message"] = "Successfully BOM Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("CreateRndBOM", $"RndBOM");
                    }

                    TempData["message"] = "Failed to Create BOM.";
                    TempData["type"] = "error";
                    return View(await GetInfo(rndBomViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(rndBomViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create BOM.";
                TempData["type"] = "error";
                return View(await GetInfo(rndBomViewModel));
            }
        }

        [AcceptVerbs("Get", "Post")]
        //[Route("GetAllByEmp")]
        public async Task<IActionResult> GetAllByStyleId(int styleId)
        {
            try
            {
                return Ok(await _rndBom.GetAllByStyleIdAsync(styleId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMaterialList(RndBomViewModel rndBomViewModel)
        {
            try
            {
                var flag = rndBomViewModel.RndBomMaterialsDetailsList.Where(c => c.CHEM_PROD_ID.Equals(rndBomViewModel.RndBomMaterialsDetails.CHEM_PROD_ID) && c.SECTION.Equals(rndBomViewModel.RndBomMaterialsDetails.SECTION));

                if (!flag.Any())
                {
                    rndBomViewModel.RndBomMaterialsDetailsList.Add(rndBomViewModel.RndBomMaterialsDetails);
                }
                rndBomViewModel = await GetMaterialsNameAsync(rndBomViewModel);
                return PartialView($"AddMaterialList", rndBomViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                rndBomViewModel = await GetMaterialsNameAsync(rndBomViewModel);
                return PartialView($"AddMaterialList", rndBomViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMaterialFromList(RndBomViewModel rndBomViewModel, string removeIndexValue)
        {
            try
            {
                ModelState.Clear();
                if (rndBomViewModel.RndBomMaterialsDetailsList[int.Parse(removeIndexValue)].BOM_D_ID != 0)
                {
                    await _rndBomMaterialsDetails.Delete(rndBomViewModel.RndBomMaterialsDetailsList[int.Parse(removeIndexValue)]);
                }
                rndBomViewModel.RndBomMaterialsDetailsList.RemoveAt(int.Parse(removeIndexValue));
                rndBomViewModel = await GetMaterialsNameAsync(rndBomViewModel);
                return PartialView($"AddMaterialList", rndBomViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                rndBomViewModel = await GetMaterialsNameAsync(rndBomViewModel);
                return PartialView($"AddMaterialList", rndBomViewModel);
            }
        }

        public async Task<RndBomViewModel> GetMaterialsNameAsync(RndBomViewModel rndBomViewModel)
        {
            try
            {
                rndBomViewModel = await _rndBomMaterialsDetails.GetMaterialsNameAsync(rndBomViewModel);
                return rndBomViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRndBOM(string rbId)
        {
            return View(await _rndBom.GetInitObjects(await _rndBom.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(rbId)))));
        }

        [HttpPost]
        public async Task<IActionResult> EditRndBOM(RndBomViewModel rndBomViewModel)
        {
            if (ModelState.IsValid)
            {
                var rndBom = await _rndBom.FindByIdAsync(int.Parse(_protector.Unprotect(rndBomViewModel.RndBom.EncryptedId)));

                if (rndBom != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    rndBomViewModel.RndBom.BOMID = rndBom.BOMID;
                    rndBomViewModel.RndBom.CREATED_AT = rndBom.CREATED_AT;
                    rndBomViewModel.RndBom.CREATED_BY = rndBom.CREATED_BY;
                    rndBomViewModel.RndBom.UPDATED_AT = DateTime.Now;
                    rndBomViewModel.RndBom.UPDATED_BY = user.Id;

                    if (await _rndBom.Update(rndBomViewModel.RndBom))
                    {

                        foreach (var item in rndBomViewModel.RndBomMaterialsDetailsList.Where(d => d.BOM_D_ID == 0).ToList())
                        {
                            item.BOMID = rndBom.BOMID;
                            item.CREATED_AT = DateTime.Now;
                            item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_BY = user.Id;
                            await _rndBomMaterialsDetails.InsertByAsync(item);
                        }
                        
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetRndBomInfo), $"RndBOM");


                    }
                }
            }

            TempData["message"] = $"Failed to Add {title}.";
            TempData["type"] = "error";
            return View(await _rndBom.GetInitObjects((rndBomViewModel)));
        }
    }
}
