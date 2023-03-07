using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.PostCosting;
using DenimERP.ViewModels.PostCostingMaster;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class CosPostCostingMasterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICOS_POSTCOSTING_CHEMDETAILS _cosPostCostingChemDetails;
        private readonly ICOS_POSTCOSTING_MASTER _cosPostCostingMaster;
        private readonly ICOS_POSTCOSTING_YARNDETAILS _cosPostCostingYarnDetails;
        private readonly IRND_PRODUCTION_ORDER _rndProductionOrder;
        private readonly IDataProtector _protector;

        public CosPostCostingMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            ICOS_POSTCOSTING_CHEMDETAILS cosPostCostingChemDetails,
            ICOS_POSTCOSTING_MASTER cosPostCostingMaster,
            ICOS_POSTCOSTING_YARNDETAILS cosPostCostingYarnDetails,
            IRND_PRODUCTION_ORDER rndProductionOrder
        )
        {
            _userManager = userManager;
            _cosPostCostingChemDetails = cosPostCostingChemDetails;
            _cosPostCostingMaster = cosPostCostingMaster;
            _cosPostCostingYarnDetails = cosPostCostingYarnDetails;
            _rndProductionOrder = rndProductionOrder;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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

                var data = await _cosPostCostingMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                                m.RndProductionOrder != null && m.RndProductionOrder.SO.SO_NO.ToString().ToUpper().Contains(searchValue)
                                            || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                                || (m.TRNSDATE != null && m.TRNSDATE.ToString().Contains(searchValue))
                                                || (m.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME != null && m.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.PCOSTID.ToString());
                }
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
        //[Route("GetAll")]
        public IActionResult GetPostCostingMasterList()
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

        [HttpGet]
        public async Task<IActionResult> CreatePostCostingMaster()
        {
            try
            {
                return View(await _cosPostCostingMaster.GetInitObj(new PostCostingViewModel()));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostCostingMaster(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    postCostingViewModel.CosPostcostingMaster.CREATED_BY = postCostingViewModel.CosPostcostingMaster.UPDATED_BY = user.Id;
                    postCostingViewModel.CosPostcostingMaster.CREATED_AT = postCostingViewModel.CosPostcostingMaster.UPDATED_AT = DateTime.Now;

                    var result = await _cosPostCostingMaster.GetInsertedObjByAsync(postCostingViewModel.CosPostcostingMaster);

                    if (result != null)
                    {

                        foreach (var item in postCostingViewModel.CosPostCostingYarnDetailsList)
                        {
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;

                            item.PCOSTID = result.PCOSTID;
                            await _cosPostCostingYarnDetails.InsertByAsync(item);
                        }

                        foreach (var item in postCostingViewModel.CosPostCostingChemDetailsList)
                        {
                            item.CREATED_BY = item.UPDATED_BY = user.Id;
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;

                            item.PCOSTID = result.PCOSTID;
                            await _cosPostCostingChemDetails.InsertByAsync(item);
                        }

                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
                    }

                    TempData["message"] = "Failed to Add";
                    TempData["type"] = "error";
                    postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                    return View(postCostingViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                return View(postCostingViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                return View(postCostingViewModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditPostCosting(string postid)
        {
            try
            {
                var postId = int.Parse(_protector.Unprotect(postid));
                var postCostingViewModel = await _cosPostCostingMaster.FindAllByIdAsync(postId);

                if (postCostingViewModel.CosPostcostingMaster != null)
                {
                    postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                    postCostingViewModel.CosPostcostingMaster.EncryptedId = _protector.Protect(postCostingViewModel.CosPostcostingMaster.PCOSTID.ToString());

                    postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                    return View(postCostingViewModel);
                }

                TempData["message"] = "Post Costing Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
            }
        }



        [HttpGet]
        public async Task<IActionResult> DeletePostCosting(string postid)
        {
            try
            {
                var postId = int.Parse(_protector.Unprotect(postid));
                var postCostingViewModel = await _cosPostCostingMaster.FindAllByIdAsync(postId);

                if (postCostingViewModel != null)
                {
                    if (await _cosPostCostingChemDetails.DeleteRange(postCostingViewModel
                            .CosPostCostingChemDetailsList) &&
                        await _cosPostCostingYarnDetails.DeleteRange(postCostingViewModel
                            .CosPostCostingYarnDetailsList))
                    {
                        if (await _cosPostCostingMaster.Delete(postCostingViewModel.CosPostcostingMaster))
                        {
                            TempData["message"] = "Post Costing Data Not Found";
                            TempData["type"] = "success";
                            return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
                        }
                    }
                    TempData["message"] = "Delete Not Complete";
                    TempData["type"] = "error";
                    return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
                }

                TempData["message"] = "Post Costing Data Not Found";
                TempData["type"] = "error";
                return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditPostCosting(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    postCostingViewModel.CosPostcostingMaster.UPDATED_BY = user.Id;
                    postCostingViewModel.CosPostcostingMaster.UPDATED_AT = DateTime.Now;

                    var result = await _cosPostCostingMaster.Update(postCostingViewModel.CosPostcostingMaster);

                    if (result)
                    {

                        foreach (var item in postCostingViewModel.CosPostCostingYarnDetailsList.Where(c => c.TRNSID > 0))
                        {
                            var yds = await _cosPostCostingYarnDetails.FindByIdAsync(item.TRNSID);
                            yds.UPDATED_BY = user.Id;
                            yds.UPDATED_AT = DateTime.Now;
                            yds.CONSUMPTION = item.CONSUMPTION;
                            yds.RATE = item.RATE;
                            yds.REMARKS = item.REMARKS;

                            await _cosPostCostingYarnDetails.Update(yds);
                        }

                        foreach (var item in postCostingViewModel.CosPostCostingChemDetailsList.Where(c => c.TRANSID > 0))
                        {
                            var yds = await _cosPostCostingChemDetails.FindByIdAsync(item.TRANSID);
                            yds.UPDATED_BY = user.Id;
                            yds.UPDATED_AT = DateTime.Now;
                            yds.CONSUMPTION = item.CONSUMPTION;
                            yds.RATE = item.RATE;
                            yds.REMARKS = item.REMARKS;
                            await _cosPostCostingChemDetails.Update(yds);
                        }

                        TempData["message"] = "Successfully Updated ";
                        TempData["type"] = "success";
                        return RedirectToAction("GetPostCostingMasterList", $"CosPostCostingMaster");
                    }

                    TempData["message"] = "Failed to Updated";
                    TempData["type"] = "error";
                    postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                    postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                    return View(postCostingViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                return View(postCostingViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update";
                TempData["type"] = "error";
                postCostingViewModel = await _cosPostCostingMaster.GetInitObj(postCostingViewModel);
                postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                return View(postCostingViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountList(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                postCostingViewModel = await _cosPostCostingYarnDetails.GetYarnDetailsBySo(postCostingViewModel);
                postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                return PartialView($"AddPostCostingYarnDetails", postCostingViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                /*postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);*/
                return PartialView($"AddPostCostingYarnDetails", postCostingViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChemicalList(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                postCostingViewModel = await _cosPostCostingChemDetails.GetChemicalDetailsBySo(postCostingViewModel);
                postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                return PartialView($"AddPostCostingChemDetails", postCostingViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //postCostingViewModel = await _cosPostCostingMaster.GetInfoAsync(postCostingViewModel);
                return PartialView($"AddPostCostingChemDetails", postCostingViewModel);
            }
        }

        [HttpGet]
        public RndProductionOrderDetailViewModel GetSoDetails(string soId)
        {
            try
            {
                return _rndProductionOrder.GetPoDetailsByPoIdAsync(soId);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<double> GetSoEliteQty(int soId)
        {
            try
            {
                return await _cosPostCostingMaster.GetSoEliteQty(soId);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("CosPostCostingMaster/PostCostingReport/{pcostid?}")]
        public async Task<IActionResult> PostCostingReport(int pcostid)
        {
            return View(model: pcostid);
        }

    }
    
}
