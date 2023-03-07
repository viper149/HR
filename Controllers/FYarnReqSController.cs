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
    [Route("YarnRequirementS")]
    public class FYarnReqSController : Controller
    {
        private readonly IF_YARN_REQ_MASTER_S _fYarnReqMasterS;
        private readonly IRND_FABRIC_COUNTINFO _rndFabricCountinfo;
        private readonly IF_YARN_REQ_DETAILS_S _fYarnReqDetailsS;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        public readonly string title = "Yarn Requirement (Sample) Information";

        public FYarnReqSController(IF_YARN_REQ_MASTER_S fYarnReqMasterS,
            IRND_FABRIC_COUNTINFO rndFabricCountinfo,
            IF_YARN_REQ_DETAILS_S fYarnReqDetailsS,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _fYarnReqMasterS = fYarnReqMasterS;
            _rndFabricCountinfo = rndFabricCountinfo;
            _fYarnReqDetailsS = fYarnReqDetailsS;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("Details/{ysrId}")]
        public async Task<IActionResult> DetailsFYarnReqS(string ysrId)
        {
            try
            {
                return View(await _fYarnReqMasterS.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(ysrId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("Edit/{ysrId}")]
        public async Task<IActionResult> EditFYarnReqS(string ysrId)
        {
            try
            {
                return View(await _fYarnReqMasterS.GetInitObjects(await _fYarnReqMasterS.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(ysrId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEditFYarnReqS(FYarnReqSViewModel fYarnReqSViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fYarnReqMaster = await _fYarnReqMasterS.FindByIdAsync(int.Parse(_protector.Unprotect(fYarnReqSViewModel.FYarnRequirementMasterS.EncryptedId)));

                    if (fYarnReqMaster != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        fYarnReqMaster.YSRNO = fYarnReqSViewModel.FYarnRequirementMasterS.YSRNO;
                        fYarnReqMaster.YSRDATE = fYarnReqSViewModel.FYarnRequirementMasterS.YSRDATE;
                        fYarnReqMaster.OPT1 = fYarnReqSViewModel.FYarnRequirementMasterS.OPT1;
                        fYarnReqMaster.OPT2 = fYarnReqSViewModel.FYarnRequirementMasterS.OPT2;
                        fYarnReqMaster.OPT3 = fYarnReqSViewModel.FYarnRequirementMasterS.OPT3;
                        fYarnReqMaster.REMARKS = fYarnReqSViewModel.FYarnRequirementMasterS.REMARKS;
                        fYarnReqMaster.UPDATED_BY = user.Id;
                        fYarnReqMaster.UPDATED_AT = DateTime.Now;

                        if (await _fYarnReqMasterS.Update(fYarnReqMaster))
                        {
                            var fYarnReqDetailses = fYarnReqSViewModel.FYarnRequirementDetailsSList.Where(e => e.TRNSID <= 0).ToList();

                            foreach (var item in fYarnReqDetailses)
                            {
                                item.YSRID = fYarnReqMaster.YSRID;
                                item.CREATED_BY = item.UPDATED_BY = user.Id;
                                item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            }

                            await _fYarnReqDetailsS.InsertRangeByAsync(fYarnReqDetailses);

                            TempData["message"] = $"Successfully Updated {title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetFYarnReqS), $"FYarnReqS");
                        }
                        else
                        {

                            TempData["message"] = $"Failed To Add {title}.";
                            TempData["type"] = "error";
                        }
                    }
                    else
                    {
                        TempData["message"] = $"Failed To Add {title}.";
                        TempData["type"] = "error";
                    }

                    return View($"CreateFYarnReqS", await _fYarnReqMasterS.GetInitObjects(fYarnReqSViewModel));
                }

                return RedirectToAction(nameof(GetFYarnReqS), $"FYarnReqS");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFYarnReqS()
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
        [Route("GetTableData")]
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
            var data = (List<F_YARN_REQ_MASTER_S>)await _fYarnReqMasterS.GetAllYarnRequirementAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data
                    .Where(m => m.YSRNO.ToString().ToUpper().Contains(searchValue)
                                || m.YSRDATE != null && m.YSRDATE.ToString().ToUpper().Contains(searchValue)
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

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFYarnReqS()
        {
            try
            {
                var fYarnReqSViewModel = await _fYarnReqMasterS.GetInitObjects(new FYarnReqSViewModel());

                var fYarnReqMasters = await _fYarnReqMasterS.All();
                var lastReqNo = fYarnReqMasters.OrderByDescending(c => c.YSRID).Select(c => c.YSRID).FirstOrDefault();

                if (lastReqNo == 0)
                {
                    lastReqNo = 99999999;
                }

                fYarnReqSViewModel.FYarnRequirementMasterS = new F_YARN_REQ_MASTER_S
                {
                    YSRNO = (lastReqNo + 1).ToString(),
                    YSRDATE = DateTime.Now
                };

                return View(fYarnReqSViewModel);
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        [Route("PostCreate")]
        public async Task<IActionResult> PostCreateFYarnReqS(FYarnReqSViewModel fYarnReqSViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                fYarnReqSViewModel.FYarnRequirementMasterS.CREATED_BY = fYarnReqSViewModel.FYarnRequirementMasterS.UPDATED_BY = user.Id;
                fYarnReqSViewModel.FYarnRequirementMasterS.CREATED_AT = fYarnReqSViewModel.FYarnRequirementMasterS.UPDATED_AT = DateTime.Now;

                var fYarnReqMaster = await _fYarnReqMasterS.GetInsertedObjByAsync(fYarnReqSViewModel.FYarnRequirementMasterS);

                if (fYarnReqMaster.YSRID != 0)
                {
                    fYarnReqMaster.YSRNO = fYarnReqMaster.YSRID.ToString();
                    await _fYarnReqMasterS.Update(fYarnReqMaster);

                    foreach (var item in fYarnReqSViewModel.FYarnRequirementDetailsSList)
                    {
                        item.CREATED_BY = user.Id;
                        item.UPDATED_BY = user.Id;
                        item.CREATED_AT = DateTime.Now;
                        item.UPDATED_AT = DateTime.Now;
                        item.YSRID = fYarnReqMaster.YSRID;
                    }

                    if (fYarnReqSViewModel.FYarnRequirementDetailsSList.Any())
                    {
                        await _fYarnReqDetailsS.InsertRangeByAsync(fYarnReqSViewModel.FYarnRequirementDetailsSList);
                    }

                    TempData["message"] = $"Successfully added {title}.";
                    TempData["type"] = "success";
                    return RedirectToAction(nameof(GetFYarnReqS), $"FYarnReqS");
                }
                else
                {
                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View($"CreateFYarnReqS", await _fYarnReqMasterS.GetInitObjects(fYarnReqSViewModel));
                }
            }

            catch (Exception)
            {
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View($"CreateFYarnReqS", await _fYarnReqMasterS.GetInitObjects(fYarnReqSViewModel));
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetCountList/{poId?}")]
        public async Task<IActionResult> GetCountList(string poId)
        {
            try
            {
                if (User.IsInRole("Warping"))
                {
                    return Ok(await _fYarnReqDetailsS.GetCountIdList(int.Parse(poId), 1));
                }
                else if (User.IsInRole("Weaving Airjet") || User.IsInRole("Weaving Rappier"))
                {
                    return Ok(await _fYarnReqDetailsS.GetCountIdList(int.Parse(poId), 2));
                }
                return Ok(await _fYarnReqDetailsS.GetCountIdList(int.Parse(poId), 1));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetCountConsumpList/{setId?}")]
        public async Task<IActionResult> GetCountConsumpList(int setId)
        {
            try
            {
                if (User.IsInRole("Warping"))
                {
                    return Ok(await Task.Run(() => _fYarnReqDetailsS.GetCountConsumpList(setId, 1)));
                }
                else if (User.IsInRole("Weaving Airjet") || User.IsInRole("Weaving Rappier"))
                {
                    return Ok(await Task.Run(() => _fYarnReqDetailsS.GetCountConsumpList(setId, 2)));
                }
                return Ok(await Task.Run(() => _fYarnReqDetailsS.GetCountConsumpList(setId, 1)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetRequiredAmountWithLot/{poId?}/{countId?}")]
        public async Task<IActionResult> GetRequiredAmountWithLot(int poId, int countId)
        {
            try
            {
                return Ok(await _fYarnReqMasterS.GetRequiredKgsWithLotdAsync(poId, countId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetSetList")]
        public dynamic GetSetList(int poId)
        {
            try
            {
                return _fYarnReqDetailsS.GetSetList(poId);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetLotFromRndFci/{count?}")]
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
        [Route("AddToList")]
        public async Task<IActionResult> AddOrDeleteFYarnRequirementDetailsTable(FYarnReqSViewModel fYarnReqSViewModel)
        {
            try
            {
                ModelState.Clear();
                if (fYarnReqSViewModel.IsDeletable)
                {
                    var fYarnReqDetails = fYarnReqSViewModel.FYarnRequirementDetailsSList[fYarnReqSViewModel.RemoveIndex];

                    if (fYarnReqDetails.TRNSID > 0)
                    {
                        await _fYarnReqDetailsS.Delete(fYarnReqDetails);
                    }

                    fYarnReqSViewModel.FYarnRequirementDetailsSList.RemoveAt(fYarnReqSViewModel.RemoveIndex);
                }
                else
                {
                    if (!fYarnReqSViewModel.FYarnRequirementDetailsSList.Any(e => e.COUNTID.Equals(fYarnReqSViewModel.FYarnRequirementDetailsS.COUNTID) && e.LOTID.Equals(fYarnReqSViewModel.FYarnRequirementDetailsS.LOTID) && e.ORDERNO.Equals(fYarnReqSViewModel.FYarnRequirementDetailsS.ORDERNO)))
                    {
                        fYarnReqSViewModel.FYarnRequirementDetailsSList.Add(fYarnReqSViewModel.FYarnRequirementDetailsS);
                    }
                }

                return PartialView($"YarnRequirementDetailsPartialView", await _fYarnReqDetailsS.GetInitObjectsOfSelectedItems(fYarnReqSViewModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
