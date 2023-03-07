using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class BasicTeamInfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_TEAMINFO _bAsTeaminfo;
        public readonly IADM_DEPARTMENT ADmDepartment;

        public BasicTeamInfoController(IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              IBAS_TEAMINFO bAsTeaminfo,
                              IADM_DEPARTMENT aDmDepartment)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _bAsTeaminfo = bAsTeaminfo;
            ADmDepartment = aDmDepartment;
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult IsTeamNameInUse(BasTeamInfoViewModel t)
        {
            var team = _bAsTeaminfo.FindByTeamName(t.BAS_TEAMINFO.TEAM_NAME);
            return team ? Json(true) : Json($"Team Name [ {t.BAS_TEAMINFO.TEAM_NAME} ] is already in use");
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

                var data = await _bAsTeaminfo.GetBasTeamsAllAsync();
                
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderBy(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            data = data.OrderByDescending(c => c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                        }
                        else
                        {
                            data = data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data
                        .Where(m => m.TEAM_NAME.ToUpper().Contains(searchValue)
                                    || m.DEPT.DEPTNAME != null && m.DEPT.DEPTNAME.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
                }

                var recordsTotal = data.Count();
                var finalData = data.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.TEAMID.ToString());
                }
                
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

        [HttpGet]
        public IActionResult GetBasTeamsWithPaged()
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
        public async Task<IActionResult> DeleteBasTeamInfo(string teamId)
        {
            try
            {
                var team = await _bAsTeaminfo.FindByIdAsync(int.Parse(_protector.Unprotect(teamId)));

                if (team != null)
                {
                    var result = await _bAsTeaminfo.Delete(team);
                    if (result)
                    {
                        TempData["message"] = "Successfully Deleted Team.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
                    }

                    TempData["message"] = "Failed to Delete Team.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
                }

                TempData["message"] = "Team Not Found!.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Delete Team.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateBasTeamInfo()
        {
            var departments = await ADmDepartment.GetAll();

            return View(new BasTeamInfoViewModel
            {
                aDM_DEPARTMENTs = departments.ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasTeamInfo(BasTeamInfoViewModel team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bAsTeaminfo.InsertByAsync(team.BAS_TEAMINFO);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added Team.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
                    }

                    TempData["message"] = "Failed to Add Team.";
                    TempData["type"] = "error";
                    return View(team);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(team);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add Team.";
                TempData["type"] = "error";
                return View(team);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasTeamInfo(string teamId)
        {

            try
            {
                var result = await _bAsTeaminfo.FindByIdAsync(int.Parse(_protector.Unprotect(teamId)));
                var departments = await ADmDepartment.GetAll();

                var basTeamInfoViewModel = new BasTeamInfoViewModel()
                {
                    aDM_DEPARTMENTs = departments.ToList(),
                    BAS_TEAMINFO = result
                };

                if (result != null && departments.Any())
                {
                    result.EncryptedId = _protector.Protect(result.TEAMID.ToString());
                    return View(basTeamInfoViewModel);
                }

                TempData["message"] = "Team Information Not Found.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Retrieve Team Information.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditBasTeamInfo(BasTeamInfoViewModel team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var teamResult = await _bAsTeaminfo.FindByIdAsync(int.Parse(_protector.Unprotect(team.BAS_TEAMINFO.EncryptedId)));

                    if (teamResult != null)
                    {
                        var result = await _bAsTeaminfo.Update(team.BAS_TEAMINFO);

                        if (result)
                        {
                            TempData["message"] = "Successfully Updated Team Information.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasTeamsWithPaged", $"BasicTeamInfo");
                        }

                        TempData["message"] = "Failed to Update Team Information.";
                        TempData["type"] = "error";
                        return View(team);
                    }

                    TempData["message"] = "Team Information Not Found.";
                    TempData["type"] = "error";
                    return View(team);
                }

                TempData["message"] = "Invalid Input, Please Try Again.";
                TempData["type"] = "error";
                return View(team);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Update Team Information.";
                TempData["type"] = "error";
                return View(team);
            }
        }
    }
}