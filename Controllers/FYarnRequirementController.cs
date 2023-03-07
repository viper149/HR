using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class FYarnRequirementController : Controller
    {
        private readonly IF_YARN_REQ_MASTER _yarnReqMaster;
        private readonly IF_YARN_REQ_DETAILS _yarnReqDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRND_FABRIC_COUNTINFO _rndFabricCountinfo;
        private readonly IDataProtector _protector;

        public FYarnRequirementController(
            IF_YARN_REQ_MASTER yarnReqMaster,
            IRND_FABRIC_COUNTINFO rndFabricCountinfo,
            IF_YARN_REQ_DETAILS yarnReqDetails,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _yarnReqMaster = yarnReqMaster;
            _yarnReqDetails = yarnReqDetails;
            _userManager = userManager;
            _rndFabricCountinfo = rndFabricCountinfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ysrId"> Belongs to YSRID. Primary key. Must not to be null. <see cref="F_YARN_REQ_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Requirement/Details/{ysrId}")]
        public async Task<IActionResult> DetailsFYarnRequirement(string ysrId)
        {
            try
            {
                return View(await _yarnReqMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(ysrId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ysrId"> Belongs to YSRID. Primary key. Must not to be null. <see cref="F_YARN_REQ_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Requirement/Edit/{ysrId}")]
        public async Task<IActionResult> EditFYarnRequirement(string ysrId)
        {
            try
            {
                return View(await _yarnReqMaster.GetInitObjects(await _yarnReqMaster.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(ysrId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEditFYarnRequirement(YarnRequirementViewModel yarnRequirement)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fYarnReqMaster = await _yarnReqMaster.FindByIdAsync(
                        int.Parse(_protector.Unprotect(yarnRequirement.FYarnRequirementMaster.EncryptedId)));

                    if (fYarnReqMaster != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        fYarnReqMaster.YSRNO = yarnRequirement.FYarnRequirementMaster.YSRNO;
                        fYarnReqMaster.YSRDATE = yarnRequirement.FYarnRequirementMaster.YSRDATE;
                        fYarnReqMaster.OPT1 = yarnRequirement.FYarnRequirementMaster.OPT1;
                        fYarnReqMaster.OPT2 = yarnRequirement.FYarnRequirementMaster.OPT2;
                        fYarnReqMaster.OPT3 = yarnRequirement.FYarnRequirementMaster.OPT3;
                        fYarnReqMaster.REMARKS = yarnRequirement.FYarnRequirementMaster.REMARKS;
                        fYarnReqMaster.SECID = yarnRequirement.FYarnRequirementMaster.SECID;
                        fYarnReqMaster.SUBSECID = yarnRequirement.FYarnRequirementMaster.SUBSECID;
                        fYarnReqMaster.UPDATED_BY = user.Id;
                        fYarnReqMaster.UPDATED_AT = DateTime.Now;

                        if (await _yarnReqMaster.Update(fYarnReqMaster))
                        {
                            var fYarnReqDetailses = yarnRequirement.FYarnRequirementDetailsList
                                .Where(e => e.TRNSID <= 0).ToList();

                            foreach (var item in fYarnReqDetailses)
                            {
                                item.YSRID = fYarnReqMaster.YSRID;
                                item.CREATED_BY = item.UPDATED_BY = user.Id;
                                item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                                item.COUNT = null;
                            }

                            if (await _yarnReqDetails.InsertRangeByAsync(fYarnReqDetailses))
                            {
                                TempData["message"] = "Successfully Updated Yarn Requirement Information.";
                                TempData["type"] = "success";
                                return RedirectToAction("GetYarnRequirementMaster", $"FYarnRequirement");
                            }
                        }
                    }
                }
                TempData["message"] = "Failed To Add Yarn Requirement Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetYarnRequirementMaster", $"FYarnRequirement");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("/Requirement")]
        [Route("/Requirement/GetAll")]
        public IActionResult GetYarnRequirementMaster()
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
                var data = (List<F_YARN_REQ_MASTER>)await _yarnReqMaster.GetAllYarnRequirementAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.YSRNO != null && m.YSRNO.ToUpper().Contains(searchValue)
                                    || m.YSRDATE != null && m.YSRDATE.ToString().ToUpper().Contains(searchValue)
                                    || m.Section.SECNAME.ToUpper().Contains(searchValue)
                                    || m.SSECNAME != null && m.SSECNAME.ToUpper().Contains(searchValue)
                                    || m.ORD_TYPE != null && m.ORD_TYPE.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                return Json(new
                {
                    draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal,
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
        [Route("/Requirement/Create")]
        public async Task<IActionResult> CreateYarnRequirement()
        {
            try
            {
                var yarnRequirementViewModel = await _yarnReqMaster.GetInitObjects(new YarnRequirementViewModel());

                var fYarnReqMasters = await _yarnReqMaster.All();
                var lastReqNo = fYarnReqMasters.OrderByDescending(c => c.YSRID).Select(c => c.YSRID).FirstOrDefault();

                if (lastReqNo == 0)
                {
                    lastReqNo = 99999999;
                }

                yarnRequirementViewModel.FYarnRequirementMaster = new F_YARN_REQ_MASTER
                {
                    YSRNO = (lastReqNo + 1).ToString(),
                    YSRDATE = DateTime.Now
                };

                return View(yarnRequirementViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCreateYarnRequirement(YarnRequirementViewModel yarnRequirementViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                yarnRequirementViewModel.FYarnRequirementMaster.CREATED_BY = yarnRequirementViewModel.FYarnRequirementMaster.UPDATED_BY = user.Id;
                yarnRequirementViewModel.FYarnRequirementMaster.CREATED_AT = yarnRequirementViewModel.FYarnRequirementMaster.UPDATED_AT = DateTime.Now;

                var fYarnReqMaster = await _yarnReqMaster.GetInsertedObjByAsync(yarnRequirementViewModel.FYarnRequirementMaster);

                if (fYarnReqMaster.YSRID != 0)
                {
                    fYarnReqMaster.YSRNO = fYarnReqMaster.YSRID.ToString();
                    await _yarnReqMaster.Update(fYarnReqMaster);

                    foreach (var item in yarnRequirementViewModel.FYarnRequirementDetailsList)
                    {
                        item.CREATED_BY = user.Id;
                        item.UPDATED_BY = user.Id;
                        item.CREATED_AT = DateTime.Now;
                        item.UPDATED_AT = DateTime.Now;
                        item.YSRID = fYarnReqMaster.YSRID;
                    }

                    if (yarnRequirementViewModel.FYarnRequirementDetailsList.Any())
                    {
                        if (await _yarnReqDetails.InsertRangeByAsync(yarnRequirementViewModel
                                .FYarnRequirementDetailsList))
                        {
                            TempData["message"] = "Successfully added Yarn Requirement Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetYarnRequirementMaster", $"FYarnRequirement");
                        }

                        TempData["message"] = "Failed to Add Yarn Requirement Information";
                        TempData["type"] = "error";
                        return View($"CreateYarnRequirement", await _yarnReqMaster.GetInitObjects(yarnRequirementViewModel));
                    }
                    TempData["message"] = "Failed to Add Yarn Requirement Information";
                    TempData["type"] = "error";
                    return View($"CreateYarnRequirement", await _yarnReqMaster.GetInitObjects(yarnRequirementViewModel));
                }
                TempData["message"] = "Failed to Add Yarn Requirement Information";
                TempData["type"] = "error";
                return View($"CreateYarnRequirement", await _yarnReqMaster.GetInitObjects(yarnRequirementViewModel));
            }

            catch (Exception)
            {
                TempData["message"] = "Failed to Add Yarn Requirement Information";
                TempData["type"] = "error";
                return View($"CreateYarnRequirement", await _yarnReqMaster.GetInitObjects(yarnRequirementViewModel));
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnRequirement/GetCountList/{poId?}")]
        public async Task<IActionResult> GetCountList(string poId)
        {
            try
            {
                //if (User.IsInRole("Warping"))
                //{
                //    return Ok(await _yarnReqDetails.GetCountIdList(int.Parse(poId), 1));
                //}
                //else if(User.IsInRole("Weaving Airjet")||User.IsInRole("Weaving Rappier"))
                //{
                //    return Ok(await _yarnReqDetails.GetCountIdList(int.Parse(poId), 1));
                //}
                return Ok(await _yarnReqDetails.GetCountIdList(int.Parse(poId), 1));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [AcceptVerbs("Get", "Post")]
        [Route("/YarnRequirement/GetCountList2/")]
        public async Task<IActionResult> GetCountList2()
        {
            try
            {
                return Ok(await _yarnReqDetails.GetCountIdList2T());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnRequirement/GetStyleByRs/{rsId?}")]
        public async Task<IActionResult> GetStyleName(int rsId)
        {
            try
            {
                return Ok(await _yarnReqDetails.GetStyleByRSNO(rsId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [AcceptVerbs("Get", "Post")]
        [Route("/YarnRequirement/GetCountConsumpList/{setId?}")]
        public async Task<IActionResult> GetCountConsumpList(int setId)
        {
            try
            {
                if (User.IsInRole("Warping"))
                {
                    return Ok(await Task.Run(() => _yarnReqDetails.GetCountConsumpList(setId, 1)));
                }
                else if (User.IsInRole("Weaving Airjet") || User.IsInRole("Weaving Rappier"))
                {
                    return Ok(await Task.Run(() => _yarnReqDetails.GetCountConsumpList(setId, 2)));
                }
                return Ok(await Task.Run(() => _yarnReqDetails.GetCountConsumpList(setId, 1)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnRequirement/GetRequiredAmountWithLot/{poId?}/{countId?}")]
        public async Task<IActionResult> GetRequiredAmountWithLot(int poId, int countId)
        {
            try
            {
                return Ok(await _yarnReqMaster.GetRequiredKgsWithLotdAsync(poId, countId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public dynamic GetSetList(int poId)
        {
            try
            {
                return _yarnReqDetails.GetSetList(poId);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("/YarnRequirement/GetLotFromRndFci/{count?}")]
        public async Task<IActionResult> GetLotFromRndFci(int count)
        {
            try
            {
                return Ok(await _rndFabricCountinfo.GetLotFromRNDFCI(count));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrDeleteFYarnRequirementDetailsTable(YarnRequirementViewModel yarnRequirementViewModel)
        {
            try
            {
                ModelState.Clear();
                if (yarnRequirementViewModel.IsDeletable)
                {
                    var fYarnReqDetails = yarnRequirementViewModel.FYarnRequirementDetailsList[yarnRequirementViewModel.RemoveIndex];

                    if (fYarnReqDetails.TRNSID > 0)
                    {
                        await _yarnReqDetails.Delete(fYarnReqDetails);
                    }

                    yarnRequirementViewModel.FYarnRequirementDetailsList.RemoveAt(yarnRequirementViewModel.RemoveIndex);
                }
                else
                {
                    if (yarnRequirementViewModel.FYarnRequirementDetailsList.Any(d => !d.ORDER_TYPE.Equals("RSNO")))
                    {
                        if (!yarnRequirementViewModel.FYarnRequirementDetailsList.Any(e =>
                                e.COUNTID.Equals(yarnRequirementViewModel.FYarnRequirementDetails.COUNTID) &&
                                e.LOTID.Equals(yarnRequirementViewModel.FYarnRequirementDetails.LOTID) &&
                                e.ORDERNO.Equals(yarnRequirementViewModel.FYarnRequirementDetails.ORDERNO) &&
                                e.SETID.Equals(yarnRequirementViewModel.FYarnRequirementDetails.SETID)))
                        {
                            yarnRequirementViewModel.FYarnRequirementDetailsList.Add(yarnRequirementViewModel.FYarnRequirementDetails);
                        }
                    }
                    else
                    {
                        if (!yarnRequirementViewModel.FYarnRequirementDetailsList.Any(e =>
                                e.COUNTID_RS.Equals(yarnRequirementViewModel.FYarnRequirementDetails.COUNTID_RS) &&
                                e.LOTID.Equals(yarnRequirementViewModel.FYarnRequirementDetails.LOTID) &&
                                e.RS.SDID.Equals(yarnRequirementViewModel.FYarnRequirementDetails.RSID) &&
                                e.SETID.Equals(yarnRequirementViewModel.FYarnRequirementDetails.SETID)))
                        {
                            yarnRequirementViewModel.FYarnRequirementDetailsList.Add(yarnRequirementViewModel.FYarnRequirementDetails);
                        }
                    }
                }

                return PartialView($"YarnRequirementDetailsPartialView", await _yarnReqDetails.GetInitObjectsOfSelectedItems(yarnRequirementViewModel));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


    }
}