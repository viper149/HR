using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class FDyeingProcessRopeMasterController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_DYEING_PROCESS_ROPE_MASTER _fDyeingProcessRopeMaster;
        private readonly IF_DYEING_PROCESS_ROPE_DETAILS _fDyeingProcessRopeDetails;
        private readonly IF_DYEING_PROCESS_ROPE_CHEM _fDyeingProcessRopeChem;

        public FDyeingProcessRopeMasterController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_DYEING_PROCESS_ROPE_MASTER fDyeingProcessRopeMaster,
            IF_DYEING_PROCESS_ROPE_DETAILS fDyeingProcessRopeDetails,
            IF_DYEING_PROCESS_ROPE_CHEM fDyeingProcessRopeChem
            )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fDyeingProcessRopeMaster = fDyeingProcessRopeMaster;
            _fDyeingProcessRopeDetails = fDyeingProcessRopeDetails;
            _fDyeingProcessRopeChem = fDyeingProcessRopeChem;
        }

        [AcceptVerbs("Post")]
        [Route("RopeDyeing/GetGroupNumbers")]
        public async Task<IActionResult> GetGroupNumbers(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel, string search, int page)
        {
            return Ok(await _fDyeingProcessRopeMaster.GetGroupNumbersByAsync(fDyeingProcessRopeViewModel, search, page));
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

                var data = await _fDyeingProcessRopeMaster.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                data = data.OrderBy(c =>
                                    c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])
                                        ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c))
                                .ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType()
                                            .GetProperty(subStrings[1])
                                            ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }
                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.GROUP.GROUP_NO.ToString().Contains(searchValue)
                                           || (m.GROUP != null && m.GROUP.OPTION1.ToString().ToUpper().Contains(searchValue))
                                           || (m.TRNSDATE != null && m.TRNSDATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.DYEING_CODE != null && m.DYEING_CODE.ToUpper().Contains(searchValue))
                                           || (m.GROUP_LENGTH.ToString() != null && m.GROUP_LENGTH.ToString().ToUpper().Contains(searchValue))
                                           || (m.DYEING_LENGTH.ToString() != null && m.DYEING_LENGTH.ToString().ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var totalRecords = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                return Json(new
                {
                    draw,
                    recordsFiltered = totalRecords,
                    recordsTotal = totalRecords,
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
        [Route("RopeDyeing")]
        [Route("RopeDyeing/GetAll")]
        public IActionResult GetDyeingProcessRopeList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateDyeingProcessRope()
        {
            var dyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(new FDyeingProcessRopeViewModel());
            
            dyeingProcessRopeViewModel.FDyeingProcessRopeDetails = new F_DYEING_PROCESS_ROPE_DETAILS
            {
                R_MACHINE_NO = 1
            };

            dyeingProcessRopeViewModel.FDyeingProcessRopeMaster = new F_DYEING_PROCESS_ROPE_MASTER
            {
                TRNSDATE = DateTime.Now
            };

            return View(dyeingProcessRopeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDyeingProcessRope(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.GROUPID == null)
                    {
                        TempData["message"] = "Please Select Group No";
                        TempData["type"] = "error";
                        return View(await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel));
                    }

                    var user = await _userManager.GetUserAsync(User);
                    fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.CREATED_BY = user.Id;
                    fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.UPDATED_BY = user.Id;
                    fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.CREATED_AT = DateTime.Now;
                    fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.UPDATED_AT = DateTime.Now;
                    var ropeDid = await _fDyeingProcessRopeMaster.InsertAndGetIdAsync(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster);

                    if (ropeDid != 0)
                    {
                        foreach (var item in fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList)
                        {
                            item.ROPE_DID = ropeDid;
                            item.CREATED_AT = DateTime.Now;
                            item.CREATED_BY = user.Id;
                            item.UPDATED_AT = DateTime.Now;
                            item.UPDATED_BY = user.Id;
                            var warpProgId = await _fDyeingProcessRopeChem.InsertByAsync(item);
                        }
                        foreach (var i in fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList)
                        {
                            i.ROPE_DID = ropeDid;
                            i.CREATED_AT = DateTime.Now;
                            i.CREATED_BY = user.Id;
                            i.UPDATED_AT = DateTime.Now;
                            i.UPDATED_BY = user.Id;

                            await _fDyeingProcessRopeDetails.InsertByAsync(i);
                        }
                        TempData["message"] = "Successfully Dyeing Process(Rope) Created.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetDyeingProcessRopeList", $"FDyeingProcessRopeMaster");
                    }

                    TempData["message"] = "Failed to Create Dyeing Process(Rope).";
                    TempData["type"] = "error";
                    return View(await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Create Dyeing Process(Rope).";
                TempData["type"] = "error";
                return View(await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditDyeingProcessRope(string id)
        {
            var sId = int.Parse(_protector.Unprotect(id));
            var fDyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.FindAllByIdAsync(sId);

            fDyeingProcessRopeViewModel = await GetChemDetailsAsync(fDyeingProcessRopeViewModel);
            if (fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster != null)
            {
                fDyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel);
                fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId = _protector.Protect(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.ROPE_DID.ToString());

                return View(fDyeingProcessRopeViewModel);
            }

            TempData["message"] = "Rope Dyeing Data Not Found";
            TempData["type"] = "error";
            return RedirectToAction("GetDyeingProcessRopeList", $"FDyeingProcessRopeMaster");
        }

        [HttpPost]
        public async Task<IActionResult> EditDyeingProcessRope(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.GROUPID == null)
                    {
                        TempData["message"] = "Please Select Group No";
                        TempData["type"] = "error";
                        return View(await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel));
                    }
                    var sId = int.Parse(_protector.Unprotect(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId));
                    if (fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.ROPE_DID == sId)
                    {
                        var ropeDetails = await _fDyeingProcessRopeMaster.FindByIdAsync(sId);

                        var user = await _userManager.GetUserAsync(User);
                        fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.UPDATED_BY = user.Id;
                        fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.UPDATED_AT = DateTime.Now;
                        fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.CREATED_AT = ropeDetails.CREATED_AT;
                        fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.CREATED_BY = ropeDetails.CREATED_BY;

                        var isUpdated = await _fDyeingProcessRopeMaster.Update(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster);
                        if (isUpdated)
                        {
                            foreach (var item in fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList.Where(c => c.ROPEID.Equals(0)))
                            {
                                item.ROPE_DID = fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.ROPE_DID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fDyeingProcessRopeDetails.InsertByAsync(item);
                            }
                            foreach (var item in fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList.Where(c => c.ROPEID > 0))
                            {
                                var ballDetails = await _fDyeingProcessRopeDetails.FindByIdAsync(item.ROPEID);

                                item.CREATED_AT = ballDetails.CREATED_AT;
                                item.CREATED_BY = ballDetails.CREATED_BY;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;

                                await _fDyeingProcessRopeDetails.Update(item);
                            }

                            foreach (var item in fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList.Where(c => c.CHEM_ID.Equals(0)))
                            {
                                item.ROPE_DID = fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.ROPE_DID;
                                item.CREATED_AT = DateTime.Now;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fDyeingProcessRopeChem.InsertByAsync(item);
                            }

                            foreach (var item in fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList.Where(c => c.CHEM_ID > 0))
                            {
                                var chemDetails = await _fDyeingProcessRopeChem.FindByIdAsync(item.CHEM_ID);

                                item.CREATED_AT = chemDetails.CREATED_AT;
                                item.CREATED_BY = chemDetails.CREATED_BY;
                                item.UPDATED_AT = DateTime.Now;
                                item.UPDATED_BY = user.Id;
                                await _fDyeingProcessRopeChem.Update(item);
                            }
                            TempData["message"] = "Successfully Updated Rope Dyeing Details.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetDyeingProcessRopeList", $"FDyeingProcessRopeMaster");
                        }
                        TempData["message"] = "Failed to Update Rope Dyeing Details.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetDyeingProcessRopeList", $"FDyeingProcessRopeMaster");
                    }
                    TempData["message"] = "Invalid Rope Dyeing Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetDyeingProcessRopeList", $"FDyeingProcessRopeMaster");
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                fDyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.FindAllByIdAsync(int.Parse(_protector.Unprotect(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId)));
                fDyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel);
                return View(fDyeingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                fDyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.FindAllByIdAsync(int.Parse(_protector.Unprotect(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId)));
                fDyeingProcessRopeViewModel = await _fDyeingProcessRopeMaster.GetInitObjectsByAsync(fDyeingProcessRopeViewModel);
                return View(fDyeingProcessRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChemList(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            try
            {
                var flag = fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList.Where(c => c.CHEM_PROD_ID.Equals(fDyeingProcessRopeViewModel.FDyeingProcessRopeChem.CHEM_PROD_ID));

                if (!flag.Any())
                {
                    fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList.Add(fDyeingProcessRopeViewModel.FDyeingProcessRopeChem);
                    fDyeingProcessRopeViewModel = await GetChemDetailsAsync(fDyeingProcessRopeViewModel);
                }
                fDyeingProcessRopeViewModel = await GetChemDetailsAsync(fDyeingProcessRopeViewModel);
                return PartialView($"AddChemList", fDyeingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fDyeingProcessRopeViewModel = await GetChemDetailsAsync(fDyeingProcessRopeViewModel);
                return PartialView($"AddChemList", fDyeingProcessRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveChemFromList(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();


            if (fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList[int.Parse(removeIndexValue)].CHEM_ID != 0)
            {
                await _fDyeingProcessRopeChem.Delete(fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList[int.Parse(removeIndexValue)]);
            }

            fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList.RemoveAt(int.Parse(removeIndexValue));
            fDyeingProcessRopeViewModel = await GetChemDetailsAsync(fDyeingProcessRopeViewModel);
            return PartialView($"AddChemList", fDyeingProcessRopeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBallList(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            try
            {
                Response.Headers["Status"] = "";
                var flag = fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList.Where(c => c.BALLID.Equals(fDyeingProcessRopeViewModel.FDyeingProcessRopeDetails.BALLID) || c.CAN_NO.Equals(fDyeingProcessRopeViewModel.FDyeingProcessRopeDetails.CAN_NO) || c.ROPE_NO.Equals(fDyeingProcessRopeViewModel.FDyeingProcessRopeDetails.ROPE_NO));

                if (!flag.Any())
                {
                    fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList.Add(fDyeingProcessRopeViewModel.FDyeingProcessRopeDetails);
                }
                else
                {
                    Response.Headers["Status"] = "Duplicate Ball No. or Can No. or Rope No.";
                }
                fDyeingProcessRopeViewModel = await GetBallDetailsAsync(fDyeingProcessRopeViewModel);
                return PartialView($"AddBallList", fDyeingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fDyeingProcessRopeViewModel = await GetBallDetailsAsync(fDyeingProcessRopeViewModel);
                return PartialView($"AddBallList", fDyeingProcessRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSetStatus(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            try
            {
                ModelState.Clear();
                foreach (var item in fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList.Where(item => item.SUBGROUPID == fDyeingProcessRopeViewModel.FDyeingProcessRopeDetails.SUBGROUPID))
                {
                    item.CLOSE_STATUS = fDyeingProcessRopeViewModel.FDyeingProcessRopeDetails.CLOSE_STATUS;
                }

                fDyeingProcessRopeViewModel = await GetBallDetailsAsync(fDyeingProcessRopeViewModel);
                return PartialView($"AddBallList", fDyeingProcessRopeViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                fDyeingProcessRopeViewModel = await GetBallDetailsAsync(fDyeingProcessRopeViewModel);
                return PartialView($"AddBallList", fDyeingProcessRopeViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBallFromList(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel, string removeIndexValue)
        {
            ModelState.Clear();

            if (fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList[int.Parse(removeIndexValue)].ROPEID != 0)
            {
                await _fDyeingProcessRopeDetails.Delete(fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList[int.Parse(removeIndexValue)]);
            }

            fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList.RemoveAt(int.Parse(removeIndexValue));
            fDyeingProcessRopeViewModel = await GetBallDetailsAsync(fDyeingProcessRopeViewModel);
            return PartialView($"AddBallList", fDyeingProcessRopeViewModel);
        }
        
        public async Task<FDyeingProcessRopeViewModel> GetChemDetailsAsync(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList = (List<F_DYEING_PROCESS_ROPE_CHEM>)await _fDyeingProcessRopeChem.GetInitChemData(fDyeingProcessRopeViewModel.FDyeingProcessRopeChemList);
            return fDyeingProcessRopeViewModel;
        }

        public async Task<FDyeingProcessRopeViewModel> GetBallDetailsAsync(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel)
        {
            fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList = (List<F_DYEING_PROCESS_ROPE_DETAILS>)await _fDyeingProcessRopeDetails.GetInitBallData(fDyeingProcessRopeViewModel.FDyeingProcessRopeDetailsList);
            return fDyeingProcessRopeViewModel;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetGroupDetails(int groupId)
        {
            try
            {
                return Ok(await _fDyeingProcessRopeMaster.GetGroupDetails(groupId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetProgramNoDetails(int subGroupId)
        {
            try
            {
                return Ok(await _fDyeingProcessRopeMaster.GetProgramNoDetails(subGroupId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<float> GetBallNoDetails(int ballId)
        {
            var result = await _fDyeingProcessRopeMaster.GetBallNoDetails(ballId);
            return result;
        }

        [HttpGet]
        public IActionResult RDyeingDeliveryReport(string groupNo)
        {
            return View(model: groupNo);
        }

        [HttpGet]
        public IActionResult RDyeingStickerReport(string groupNo)
        {
            return View(model: groupNo);
        }
    }
}