using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.Marketing;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class RndPurchaseRequisitionMasterController : Controller
    {
        private readonly IRND_PURCHASE_REQUISITION_MASTER _rndRequisition;
        private readonly IF_BAS_SECTION _fBasSection;
        private readonly IMKT_SDRF_INFO _mktSdrfInfo;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountInfo;
        private readonly IRND_SAMPLE_INFO_DYEING _rndSampleInfoDyeing;
        private readonly ICOM_EX_PI_DETAILS _comExPiDetails;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IYARNFOR _yarnfor;
        private readonly IYARNFROM _yarnfrom;
        private readonly IF_BAS_UNITS _fBasUnits;
        private readonly IBAS_YARN_LOTINFO _basYarnLotinfo;
        private readonly IF_YS_SLUB_CODE _fYsSlubCode;
        private readonly IF_YS_RAW_PER _fYsRawPer;
        private readonly IF_YS_INDENT_DETAILS _indentDetails;
        private readonly IDataProtector _protector;
        private readonly string title = "Yarn Store Purchase Requisition Information";

        public RndPurchaseRequisitionMasterController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_PURCHASE_REQUISITION_MASTER rndRequisition,
            IF_BAS_SECTION fBasSection,
            IMKT_SDRF_INFO mktSdrfInfo,
            IBAS_YARN_COUNTINFO basYarnCountInfo,
            IRND_SAMPLE_INFO_DYEING rndSampleInfoDyeing,
            IF_YS_INDENT_DETAILS indentDetails,
            ICOM_EX_PI_DETAILS comExPiDetails,
            UserManager<ApplicationUser> userManager,
            IYARNFOR yarnfor,
            IYARNFROM yarnfrom,
            IF_BAS_UNITS fBasUnits,
            IBAS_YARN_LOTINFO basYarnLotinfo,
            IF_YS_SLUB_CODE fYsSlubCode,
            IF_YS_RAW_PER fYsRawPer
        )
        {
            _rndRequisition = rndRequisition;
            _fBasSection = fBasSection;
            _basYarnCountInfo = basYarnCountInfo;
            _rndSampleInfoDyeing = rndSampleInfoDyeing;
            _comExPiDetails = comExPiDetails;
            _userManager = userManager;
            _yarnfor = yarnfor;
            _yarnfrom = yarnfrom;
            _fBasUnits = fBasUnits;
            _basYarnLotinfo = basYarnLotinfo;
            _fYsSlubCode = fYsSlubCode;
            _fYsRawPer = fYsRawPer;
            _mktSdrfInfo = mktSdrfInfo;
            _indentDetails = indentDetails;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpPost]
        [Route("/Requisition/GetCountName")]
        public async Task<IActionResult> GetCountName(RndYarnRequisitionViewModel rndYarnRequisition)
        {
            return Ok(await _rndRequisition.GetCountNameByOrderNoAsync(rndYarnRequisition));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indslId">Belongs to INDSLID. Primary key. Must not be null.<see cref="RND_PURCHASE_REQUISITION_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Requisition/Details/{indslId}")]
        public async Task<IActionResult> DetailsRndPurchaseRequisitionMaster(string indslId)
        {
            try
            {
                return View(await _rndRequisition.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indslId))));
            }
            catch (Exception)
            {

                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEditRndPurchaseRequisitionMaster(RndYarnRequisitionViewModel requisitionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rndPurchaseRequisitionMaster = await _rndRequisition.FindByIdAsync(int.Parse(_protector.Unprotect(requisitionViewModel.RndPurchaseRequisitionMaster.EncryptedId)));

                    if (rndPurchaseRequisitionMaster != null)
                    {
                        var currentUser = await _userManager.GetUserAsync(User);

                        requisitionViewModel.RndPurchaseRequisitionMaster.STATUS = (requisitionViewModel.RndPurchaseRequisitionMaster.IS_REVISED) ? "0" : rndPurchaseRequisitionMaster.STATUS;
                        requisitionViewModel.RndPurchaseRequisitionMaster.REVISE_DATE = (requisitionViewModel.RndPurchaseRequisitionMaster.IS_REVISED) ? DateTime.Now : null;
                        requisitionViewModel.RndPurchaseRequisitionMaster.CREATED_AT = rndPurchaseRequisitionMaster.CREATED_AT;
                        requisitionViewModel.RndPurchaseRequisitionMaster.CREATED_BY = rndPurchaseRequisitionMaster.CREATED_BY;
                        requisitionViewModel.RndPurchaseRequisitionMaster.UPDATED_AT = DateTime.Now;
                        requisitionViewModel.RndPurchaseRequisitionMaster.UPDATED_BY = currentUser.Id;

                        var isUpdated = await _rndRequisition.Update(requisitionViewModel.RndPurchaseRequisitionMaster);

                        if (isUpdated)
                        {
                            foreach (var item in requisitionViewModel.FysIndentDetailList.Where(c => c.TRNSID.Equals(0)))
                            {
                                item.INDSLID = rndPurchaseRequisitionMaster.INDSLID;
                                item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                                item.CREATED_BY = item.UPDATED_BY = currentUser.Id;
                                item.TRNSID = 0;
                                await _indentDetails.InsertByAsync(item);
                            }

                            foreach (var item in requisitionViewModel.FysIndentDetailList.Where(c => c.TRNSID > 0))
                            {
                                var oldData = await _indentDetails.FindByIdAsync(item.TRNSID);
                                item.CREATED_AT = oldData.CREATED_AT;
                                item.UPDATED_AT = DateTime.Now;
                                item.CREATED_BY = oldData.CREATED_BY;
                                item.UPDATED_BY = currentUser.Id;
                                item.CNSMP_AMOUNT = item.ORDER_QTY;
                                item.BASCOUNTINFO = null;
                                await _indentDetails.Update(item);
                            }
                            TempData["message"] = $"Successfully Updated {title}";
                            TempData["type"] = "success";
                        }
                        else
                        {
                            TempData["message"] = $"Failed To Update {title}";
                            TempData["type"] = "error";
                            return View(nameof(EditRndPurchaseRequisitionMaster), await _rndRequisition.GetInitObjectsByAsync(requisitionViewModel));
                        }
                    }

                    return RedirectToAction(nameof(GetPurchaseRequisition), $"RndPurchaseRequisitionMaster");
                }
                else
                {
                    TempData["message"] = "Invalid Form Inputs.";
                    TempData["type"] = "error";
                    return View(nameof(EditRndPurchaseRequisitionMaster), await _rndRequisition.GetInitObjectsByAsync(requisitionViewModel));
                }
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indslId">Belongs to INDSLID. Primary key. Must not be null.<see cref="RND_PURCHASE_REQUISITION_MASTER"/></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Requisition/Edit/{indslId}")]
        [Authorize(Roles = "Planning(F),RND,Yarn Store,Super Admin")]
        public async Task<IActionResult> EditRndPurchaseRequisitionMaster(string indslId)
        {
            try
            {
                return View(await _rndRequisition.GetInitObjectsByAsync(await _rndRequisition.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(indslId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        [Route("/Requisition/GetAll")]
        public IActionResult GetPurchaseRequisition()
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

                var data = (List<RND_PURCHASE_REQUISITION_MASTER>)await _rndRequisition.GetAllPurchaseRequisationAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.INDSLID.ToString().ToUpper().Contains(searchValue)
                                    || m.RESEMP.FIRST_NAME != null && m.RESEMP.FIRST_NAME.ToUpper().Contains(searchValue)
                                    || m.INDENT_SL_NO != null && m.INDENT_SL_NO.ToUpper().Contains(searchValue)
                                    || m.EMP.FIRST_NAME != null && m.EMP.FIRST_NAME.ToUpper().Contains(searchValue)
                                    || m.INDSLDATE != null && m.INDSLDATE.ToString().ToUpper().Contains(searchValue)
                                    || m.YARN_FOR != null && m.YARN_FOR.ToUpper().Contains(searchValue)
                                    || m.INDSLNO != null && m.INDSLNO.ToUpper().Contains(searchValue)
                                    || m.ORDER_NO.ToString().ToUpper().Contains(searchValue)
                                    || m.OPT2 != null && m.OPT2.ToUpper().Contains(searchValue)
                                    || m.STATUS != null && m.STATUS.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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

        [AcceptVerbs("Get", "Post")]
        //[Route("/YarnRequirement/GetCountList/{poId?}")]
        public async Task<IActionResult> GetCountList(int poId)
        {
            try
            {
                return Ok(await _indentDetails.GetCountIdList(poId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/Requisition/Create")]
        [Authorize(Roles = "Planning(F),RND,Yarn Store,Super Admin")]
        public async Task<IActionResult> CreatePurchaseRequisition()
        {
            return View(await _rndRequisition.GetInitObjectsByAsync(new RndYarnRequisitionViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> PostCreatePurchaseRequisition(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string indentNo;
                    var currentUser = await _userManager.GetUserAsync(User);
                    rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.CREATED_AT = rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.UPDATED_AT = DateTime.Now;
                    rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.CREATED_BY = rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.UPDATED_BY = currentUser.Id;

                    if (rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.YARN_FOR.ToLower().Contains("sample"))
                    {
                        indentNo = await _rndRequisition.GetLastIndentNoAsync("SYW");
                    }

                    else if (rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.YARN_FOR.ToLower().Contains("lc/work order"))
                    {
                        indentNo = await _rndRequisition.GetLastIndentNoAsync("YLC");
                    }

                    else
                    {
                        indentNo = await _rndRequisition.GetLastIndentNoAsync("YPR");
                    }

                    rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster.INDSLNO = indentNo;

                    var rndPurchaseRequisitionMaster = await _rndRequisition.GetInsertedObjByAsync(rndYarnRequisitionViewModel.RndPurchaseRequisitionMaster);

                    if (rndPurchaseRequisitionMaster != null)
                    {
                        foreach (var item in rndYarnRequisitionViewModel.FysIndentDetailList)
                        {
                            item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                            item.CREATED_BY = item.CREATED_BY = currentUser.Id;
                            item.INDSLID = rndPurchaseRequisitionMaster.INDSLID;
                        }

                        await _indentDetails.InsertRangeByAsync(rndYarnRequisitionViewModel.FysIndentDetailList);

                        TempData["message"] = $"Successfully added {title}";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetPurchaseRequisition), "RndPurchaseRequisitionMaster");
                    }

                    TempData["message"] = $"Failed to Add {title}";
                    TempData["type"] = "error";
                    return View(nameof(CreatePurchaseRequisition), await _rndRequisition.GetInitObjectsByAsync(rndYarnRequisitionViewModel));
                }

                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(nameof(CreatePurchaseRequisition), await _rndRequisition.GetInitObjectsByAsync(rndYarnRequisitionViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = $"Failed to Add {title}";
                TempData["type"] = "error";
                return View(nameof(CreatePurchaseRequisition), await _rndRequisition.GetInitObjectsByAsync(rndYarnRequisitionViewModel));
            }
        }

        [HttpGet]
        public async Task<List<RND_SAMPLE_INFO_DYEING>> GetRSList()
        {
            try
            {
                var rsList = await _rndSampleInfoDyeing.GetAll();
                return rsList.ToList();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("/Requisition/GetListOfSDRF")]
        public async Task<List<MKT_SDRF_INFO>> GetSdrfList()
        {
            try
            {
                var sdrfList = await _mktSdrfInfo.All();
                var mktSdrfInfos = sdrfList.Where(e => !string.IsNullOrEmpty(e.SDRF_NO))
                    .OrderByDescending(e => e.SDRF_NO).ToList();
                return mktSdrfInfos;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("/Requisition/GetListOfSO")]
        public async Task<IActionResult> GetSoList()
        {
            try
            {
                return Ok(await _comExPiDetails.GetSoListWithProductionOrder());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("/Requisition/GetYarnDetails")]
        [HttpGet]
        public async Task<F_YS_INDENT_DETAILS> GetYarnDetails(int trnsId)
        {
            try
            {
                return await _comExPiDetails.GetYarnDetailsbyId(trnsId);
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIndentDetails(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            try
            {
                ModelState.Clear();
                if (rndYarnRequisitionViewModel.IsDeletable)
                {
                    var fYsIndentDetails = rndYarnRequisitionViewModel.FysIndentDetailList[rndYarnRequisitionViewModel.RemoveIndex];

                    if (fYsIndentDetails.TRNSID > 0)
                    {
                        var ysIndentDetails = await _indentDetails.FindByIdAsync(fYsIndentDetails.TRNSID);
                        if (ysIndentDetails != null)
                        {
                            await _indentDetails.Delete(ysIndentDetails);
                        }
                    }

                    rndYarnRequisitionViewModel.FysIndentDetailList.RemoveAt(rndYarnRequisitionViewModel.RemoveIndex);
                }

                else
                {
                    if (rndYarnRequisitionViewModel.FysIndentDetailList.Any(e => e.TRNSID > 0 && e.TRNSID.Equals(rndYarnRequisitionViewModel.FysIndentDetails.TRNSID)))
                    {
                        var fysIndentDetailList = rndYarnRequisitionViewModel.FysIndentDetailList.Where(e => e.TRNSID.Equals(rndYarnRequisitionViewModel.FysIndentDetails.TRNSID)).ToList();


                        for (var i = 0; i < fysIndentDetailList.Count; i++)
                        {
                            if (TryValidateModel(rndYarnRequisitionViewModel.FysIndentDetails))
                            {
                                fysIndentDetailList[i].TRNSID = rndYarnRequisitionViewModel.FysIndentDetails.TRNSID;
                                fysIndentDetailList[i].PRODID = rndYarnRequisitionViewModel.FysIndentDetails.PRODID;
                                fysIndentDetailList[i].YARN_FOR = rndYarnRequisitionViewModel.FysIndentDetails.YARN_FOR;
                                fysIndentDetailList[i].YARN_FROM = rndYarnRequisitionViewModel.FysIndentDetails.YARN_FROM;
                                fysIndentDetailList[i].PREV_LOTID = rndYarnRequisitionViewModel.FysIndentDetails.PREV_LOTID;
                                fysIndentDetailList[i].RAW = rndYarnRequisitionViewModel.FysIndentDetails.RAW;
                                fysIndentDetailList[i].SLUB_CODE = rndYarnRequisitionViewModel.FysIndentDetails.SLUB_CODE;
                                fysIndentDetailList[i].ORDER_QTY = rndYarnRequisitionViewModel.FysIndentDetails.ORDER_QTY;
                                fysIndentDetailList[i].REMARKS = rndYarnRequisitionViewModel.FysIndentDetails.REMARKS;
                                fysIndentDetailList[i].NO_OF_CONE = rndYarnRequisitionViewModel.FysIndentDetails.NO_OF_CONE;
                                fysIndentDetailList[i].ETR = rndYarnRequisitionViewModel.FysIndentDetails.ETR;
                            }

                            else
                            {
                                ModelState.AddModelError("", $"There is an error with your data. Please check the row number {i + 1}");
                            }
                        }
                    }


                    else
                    {
                        if (!rndYarnRequisitionViewModel.FysIndentDetailList.Any(e => e.PRODID.Equals(rndYarnRequisitionViewModel.FysIndentDetails.PRODID) && e.YARN_FOR.Equals(rndYarnRequisitionViewModel.FysIndentDetails.YARN_FOR) && e.PREV_LOTID.Equals(rndYarnRequisitionViewModel.FysIndentDetails.PREV_LOTID) && e.RAW.Equals(rndYarnRequisitionViewModel.FysIndentDetails.RAW)))
                        {
                            rndYarnRequisitionViewModel.FysIndentDetailList.Add(rndYarnRequisitionViewModel.FysIndentDetails);
                        }
                    }
                }

                

                foreach (var item in rndYarnRequisitionViewModel.FysIndentDetailList)
                {
                    item.SEC = await _fBasSection.FindByIdAsync(item.SECID ?? 0);
                    item.BASCOUNTINFO = await _basYarnCountInfo.FindByIdAsync(item.PRODID ?? 0);
                    item.YARN_FORNavigation = await _yarnfor.FindByIdAsync(item.YARN_FOR ?? 0);
                    item.YARN_FROMNavigation = await _yarnfrom.FindByIdAsync(item.YARN_FROM ?? 0);
                    item.FBasUnits = await _fBasUnits.FindByIdAsync(item.UNIT ?? 0);
                    item.LOT = await _basYarnLotinfo.FindByIdAsync(item.PREV_LOTID ?? 0);
                    item.RAWNavigation = await _fYsRawPer.FindByIdAsync(item.RAW ?? 0);
                    item.SLUB_CODENavigation = await _fYsSlubCode.FindByIdAsync(item.SLUB_CODE ?? 0);
                    item.FBasUnits = await _fBasUnits.FindByIdAsync(item.UNIT ?? 0);
                }

                return PartialView($"IndentDetailsPartialView", rndYarnRequisitionViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return PartialView($"IndentDetailsPartialView", rndYarnRequisitionViewModel);
            }
        }
    }
}