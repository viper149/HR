using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ServiceInterfaces.Marketing;
using DenimERP.ViewModels.Marketing;
using DenimERP.ViewModels.Security;
using DenimERP.ViewModels.StaticData;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class MktSdrfInfoController : Controller
    {
        private readonly IMKT_SDRF_INFO _mktSdrfInfo;
        private readonly IMKT_TEAM _mktTeam;
        private readonly IRND_ANALYSIS_SHEET _rndAnalysisSheet;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPDL_EMAIL_SENDER<bool> _emailSender;
        private readonly IDataProtector _protector;

        public MktSdrfInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IMKT_SDRF_INFO mktSdrfInfo,
            IMKT_TEAM mktTeam,
            IRND_ANALYSIS_SHEET rndAnalysisSheet,
            UserManager<ApplicationUser> userManager,
            IPDL_EMAIL_SENDER<bool> emailSender
        )
        {
            _mktSdrfInfo = mktSdrfInfo;
            _mktTeam = mktTeam;
            _rndAnalysisSheet = rndAnalysisSheet;
            _userManager = userManager;
            _emailSender = emailSender;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mktSdrfInfoViewModel"> View model. <see cref="MktSdrfInfoViewModel"/></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsSdrfNoInUse(MktSdrfInfoViewModel mktSdrfInfoViewModel)
        {
            var isSdrfNoExists = await _mktSdrfInfo.FindBySdrfNoInUseAsync(mktSdrfInfoViewModel.MktSdrfInfo.SDRF_NO);
            return !isSdrfNoExists ? Json(true) : Json($"SDRF No [ {mktSdrfInfoViewModel.MktSdrfInfo.SDRF_NO} ] is already in use");
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
                var data = (List<MKT_SDRF_INFO>)await _mktSdrfInfo.GetMktSdrfAllAsync();

                if (!User.IsInRole("Super Admin"))
                {
                    if (User.IsInRole("Planning(HO)"))
                    {
                        data = data.Where(c => c.MKT_DGM_APPROVE.Equals(true))
                            //.OrderBy(c=>c.PLN_APPROVE)
                            .ToList();
                    }
                    if (User.IsInRole("RND"))
                    {
                        data = data.Where(c => c.PLN_APPROVE.Equals(true))
                            //.OrderBy(c => c.RND_APPROVE)
                            .ToList();
                    }
                    if (User.IsInRole("Plant Head(F)"))
                    {
                        data = data.Where(c => c.RND_APPROVE.Equals(true))
                            //.OrderBy(c => c.PLANT_HEAD_APPROVE)
                            .ToList();
                    }
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }

                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.SDRF_NO!=null && m.SDRF_NO.Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.TRANSDATE.ToString()) && m.TRANSDATE.ToString().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.RND_ANALYSIS_SHEET.MKT_QUERY_NO) && m.RND_ANALYSIS_SHEET.MKT_QUERY_NO.ToUpper().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.CONSTRUCTION) && m.CONSTRUCTION.ToUpper().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.TEAM_M.TEAM_NAME) && m.TEAM_M.TEAM_NAME.ToUpper().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.MKT_PERSON.PERSON_NAME) && m.MKT_PERSON.PERSON_NAME.ToUpper().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.REMARKS) && m.REMARKS.ToUpper().Contains(searchValue)
                                           || !string.IsNullOrEmpty(m.SEASON) && m.SEASON.Contains(searchValue)
                                        ).ToList();
                }

                var finalData = data.Skip(skip).Take(pageSize).ToList();
                var totalRecords = data.Count();

                return Json(new
                {
                    draw = draw,
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
        public IActionResult GetSdrfList()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateSdrf()
        {
            try
            {
                ViewBag.Seasons = StaticData.GetSeasons();

                return View(await _mktSdrfInfo.GetInitObjects(new MktSdrfInfoViewModel
                {
                    MktSdrfInfo = new MKT_SDRF_INFO
                    {
                        TRANSDATE = DateTime.Now,
                        PRIORITY = "REGULAR"
                    }
                }));
            }
            catch (Exception e)
            {
                TempData["message"] = "404! not found. Please Contact with the Developers.";
                TempData["type"] = "error";
                return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mktSdrfInfoViewModel"> View model. <see cref="MktSdrfInfoViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSdrf(MktSdrfInfoViewModel mktSdrfInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (mktSdrfInfoViewModel.MktSdrfInfo != null)
                    {
                        var result = await _mktSdrfInfo.InsertByAsync(mktSdrfInfoViewModel.MktSdrfInfo);

                        if (result)
                        {
                            var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
                            var ip = HttpContext.Connection.LocalIpAddress.ToString();

                            if (ip == "127.0.0.1" || ip == "::1")
                            {
                                baseUrl = "https://pioneerdenim.com";
                            }
                            else if (!baseUrl.Split('.').Any(e => e.Contains("www") || e.Contains("com")))
                            {
                                baseUrl = "https://pioneerdenim.com";
                            }

                            var emailsByTeamIdMktPersonIdAsync = await _mktTeam.GetEmailsByTeamIdMktPersonIdAsync(mktSdrfInfoViewModel.MktSdrfInfo.TEAMID ?? 0, mktSdrfInfoViewModel.MktSdrfInfo.MKT_PERSON_ID ?? 0);
                            emailsByTeamIdMktPersonIdAsync.BaseUrl = baseUrl;
                            var emailBody = EmailBodies.GetMarketingSdrfInfoCreateEmailBody(emailsByTeamIdMktPersonIdAsync);

                            if (emailsByTeamIdMktPersonIdAsync.ToEmailObj.Any())
                            {
                                var to = string.Join(";", emailsByTeamIdMktPersonIdAsync.ToEmailObj.Select(e => e.EMAIL).Where(e => e != null && !string.IsNullOrWhiteSpace(e)));
                                var cc = string.Empty;
                                var bcc = string.Empty;

                                if (emailsByTeamIdMktPersonIdAsync.CcEmailObj.Any())
                                    cc = string.Join(";", emailsByTeamIdMktPersonIdAsync.CcEmailObj.Select(e => e.EMAIL).Where(e => e != null && !string.IsNullOrWhiteSpace(e)));

                                if (emailsByTeamIdMktPersonIdAsync.BccEmailObj.Any())
                                    bcc = string.Join(";", emailsByTeamIdMktPersonIdAsync.BccEmailObj.Select(e => e.EMAIL).Where(e => e != null && !string.IsNullOrWhiteSpace(e)));

                                _emailSender.SendMultiEmailAsync(to, cc, bcc, "Test SDRF Created", emailBody, true);
                            }

                            TempData["message"] = "Successfully Added Sample Development Requisition.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
                        }

                        TempData["message"] = "Failed to Add Sample Development Requisition.";
                        TempData["type"] = "error";
                        return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
                    }

                    TempData["message"] = "Failed to Add Sample Development Requisition.";
                    TempData["type"] = "error";
                    return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
                }

                TempData["message"] = "Invalid Input for Sample Development Requisition.";
                TempData["type"] = "error";
                return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["message"] = "Failed to Add Sample Development Requisition.";
                TempData["type"] = "error";
                return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> Belongs to SDRFID. Primary key. Must not be null. <see cref="MKT_SDRF_INFO"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditSdrf(string id)
        {
            try
            {
                var mktSdrfInfo = await _mktSdrfInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (mktSdrfInfo != null)
                {
                    mktSdrfInfo.EncryptedId = _protector.Protect(mktSdrfInfo.SDRFID.ToString());

                    ViewBag.Seasons = StaticData.GetSeasons();

                    return View(await _mktSdrfInfo.GetInitObjects(new MktSdrfInfoViewModel
                    {
                        MktSdrfInfo = mktSdrfInfo,
                    }));
                }

                TempData["message"] = "Failed to retrieve Data.";
                TempData["type"] = "error";
                return RedirectToAction($"GetSdrfList", $"MktSdrfInfo");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mktSdrfInfoViewModel"> View model. <see cref="MktSdrfInfoViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditSdrf(MktSdrfInfoViewModel mktSdrfInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sdrf = await _mktSdrfInfo.FindByIdAsync(int.Parse(_protector.Unprotect(mktSdrfInfoViewModel.MktSdrfInfo.EncryptedId)));

                    if (sdrf != null)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        mktSdrfInfoViewModel.MktSdrfInfo.UPDATED_AT = DateTime.Now;
                        mktSdrfInfoViewModel.MktSdrfInfo.UPDATED_BY = user.Id;
                        mktSdrfInfoViewModel.MktSdrfInfo.CREATED_AT = sdrf.CREATED_AT;
                        mktSdrfInfoViewModel.MktSdrfInfo.CREATED_BY = sdrf.CREATED_BY;
                        mktSdrfInfoViewModel.MktSdrfInfo.SDRF_NO = sdrf.SDRF_NO;

                        if (User.IsInRole("Marketing(DGM)"))
                        {
                            mktSdrfInfoViewModel.MktSdrfInfo.RND_APPROVE = sdrf.RND_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.SUBMIT_DATE_FACTORY = sdrf.SUBMIT_DATE_FACTORY;
                            mktSdrfInfoViewModel.MktSdrfInfo.RND_REMARKS = sdrf.RND_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.PLANT_HEAD_APPROVE = sdrf.PLANT_HEAD_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANT_HEAD_REMARKS = sdrf.PLANT_HEAD_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.PLN_APPROVE = sdrf.PLN_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.ACTUAL_DATE = sdrf.ACTUAL_DATE;
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANNING_REMARKS = sdrf.PLANNING_REMARKS;
                            mktSdrfInfoViewModel.MktSdrfInfo.MATERIAL_AVAILABLE = sdrf.MATERIAL_AVAILABLE;
                        }

                        if (User.IsInRole("Planning(F)"))
                        {
                            mktSdrfInfoViewModel.MktSdrfInfo.RND_APPROVE = sdrf.RND_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.SUBMIT_DATE_FACTORY = sdrf.SUBMIT_DATE_FACTORY;
                            mktSdrfInfoViewModel.MktSdrfInfo.RND_REMARKS = sdrf.RND_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.PLANT_HEAD_APPROVE = sdrf.PLANT_HEAD_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANT_HEAD_REMARKS = sdrf.PLANT_HEAD_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.MKT_DGM_APPROVE = sdrf.MKT_DGM_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.Marketing_DGM_REMARKS = sdrf.Marketing_DGM_REMARKS;
                            mktSdrfInfoViewModel.MktSdrfInfo.MATERIAL_AVAILABLE = sdrf.MATERIAL_AVAILABLE;
                        }

                        if (User.IsInRole("RND"))
                        {
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANT_HEAD_APPROVE = sdrf.PLANT_HEAD_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANT_HEAD_REMARKS = sdrf.PLANT_HEAD_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.MKT_DGM_APPROVE = sdrf.MKT_DGM_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.Marketing_DGM_REMARKS = sdrf.Marketing_DGM_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.PLN_APPROVE = sdrf.PLN_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.ACTUAL_DATE = sdrf.ACTUAL_DATE;
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANNING_REMARKS = sdrf.PLANNING_REMARKS;
                        }

                        if (User.IsInRole("Plant Head(F)"))
                        {
                            mktSdrfInfoViewModel.MktSdrfInfo.RND_APPROVE = sdrf.RND_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.SUBMIT_DATE_FACTORY = sdrf.SUBMIT_DATE_FACTORY;
                            mktSdrfInfoViewModel.MktSdrfInfo.RND_REMARKS = sdrf.RND_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.MKT_DGM_APPROVE = sdrf.MKT_DGM_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.Marketing_DGM_REMARKS = sdrf.Marketing_DGM_REMARKS;

                            mktSdrfInfoViewModel.MktSdrfInfo.PLN_APPROVE = sdrf.PLN_APPROVE;
                            mktSdrfInfoViewModel.MktSdrfInfo.ACTUAL_DATE = sdrf.ACTUAL_DATE;
                            mktSdrfInfoViewModel.MktSdrfInfo.PLANNING_REMARKS = sdrf.PLANNING_REMARKS;
                            mktSdrfInfoViewModel.MktSdrfInfo.MATERIAL_AVAILABLE = sdrf.MATERIAL_AVAILABLE;
                        }

                        var result = await _mktSdrfInfo.Update(mktSdrfInfoViewModel.MktSdrfInfo);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Sample Development Requisition.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
                        }

                        TempData["message"] = "Failed to Update Sample Development Requisition.";
                        TempData["type"] = "error";
                        return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
                    }

                    TempData["message"] = "Failed to Update Sample Development Requisition.";
                    TempData["type"] = "error";
                    return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
            }
            catch (Exception)
            {
                TempData["message"] = "Error Occurred During Your Submission. Please Try Again.";
                TempData["type"] = "error";
                return View(await _mktSdrfInfo.GetInitObjects(mktSdrfInfoViewModel));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> Belongs to SDRFID. Primary key. Must not be null. <see cref="MKT_SDRF_INFO"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteSdrf(string id)
        {
            try
            {
                var sdrf = await _mktSdrfInfo.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

                if (sdrf != null)
                {
                    var result = await _mktSdrfInfo.Delete(sdrf);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Sample Development Requisition.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
                    }
                    TempData["message"] = "Failed to Delete Sample Development Requisition.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
                }
                TempData["message"] = "Sample Development Requisition Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Delete Sample Development Requisition.";
                TempData["type"] = "error";
                return RedirectToAction("GetSdrfList", $"MktSdrfInfo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamid"> Belongs to MKT_TEAMID. Primary key. Must not be null. <see cref="MKT_TEAM"/></param>
        /// <returns> List of items. </returns>
        [HttpGet]
        public async Task<List<MKT_TEAM>> GetTeamMembers(int teamid)
        {
            try
            {
                var result = await _mktTeam.FindTeamMembersAsync(teamid);
                return result?.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aid"> Belongs to AID. Primary key. Must not be null. <see cref="RND_ANALYSIS_SHEET"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RND_ANALYSIS_SHEET> GetAnalysisDetails(int aid)
        {
            try
            {
                var result = await _rndAnalysisSheet.FindByIdAsync(aid);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<int> GetSdrfNumber()
        {
            try
            {
                var result = await _mktSdrfInfo.GetSdrfNumberAsync();
                return result + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"> Belongs to DateTime. Must not be null. </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> GetAvailableDate(DateTime date)
        {
            try
            {
                return await _mktSdrfInfo.GetAvailableDate(date);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}