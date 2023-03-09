using System;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInterfaces;
using HRMS.ServiceInterfaces.HR;
using HRMS.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("Employee")]
    public class FHrdEmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_HRD_EMPLOYEE _fHrdEmployee;
        private readonly IF_HRD_EDUCATION _fHrdEducation;
        private readonly IDataProtector _protector;
        private const string Title = "Employee Information";

        public FHrdEmployeeController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_HRD_EMPLOYEE fHrdEmployee,
            IF_HRD_EDUCATION fHrdEducation)
        {
            _userManager = userManager;
            _fHrdEmployee = fHrdEmployee;
            _fHrdEducation = fHrdEducation;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetFHrdEmployee()
        {
            ViewData["Title"] = Title;
            return View();
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateFHrdEmployee()
        {
            ViewData["Title"] = Title;
            return View(await _fHrdEmployee.GetInitObjByAsync(new FHrdEmployeeViewModel()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateFHrdEmployee(FHrdEmployeeViewModel fHrdEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                fHrdEmployeeViewModel.FHrdEmployee.CREATED_BY = fHrdEmployeeViewModel.FHrdEmployee.UPDATED_BY = user.Id;
                fHrdEmployeeViewModel.FHrdEmployee.CREATED_AT = fHrdEmployeeViewModel.FHrdEmployee.UPDATED_AT = DateTime.Now;

                var fHrdEmployee = await _fHrdEmployee.GetInsertedObjByAsync(fHrdEmployeeViewModel.FHrdEmployee);

                if (fHrdEmployee.EMPID > 0)
                {
                    foreach (var item in fHrdEmployeeViewModel.FHrdEducationList)
                    {
                        item.EMPID = fHrdEmployee.EMPID;
                        item.CREATED_AT = item.UPDATED_AT = DateTime.Now;
                        item.CREATED_BY = item.UPDATED_BY = user.Id;
                    }

                    if (await _fHrdEducation.InsertRangeByAsync(fHrdEmployeeViewModel.FHrdEducationList))
                    {
                        TempData["message"] = $"Successfully added {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFHrdEmployee));
                    }

                    await _fHrdEmployee.Delete(fHrdEmployee);
                    TempData["message"] = $"Failed to Add {Title}.";
                    TempData["type"] = "error";
                    ViewData["Title"] = Title;
                    return View(fHrdEmployeeViewModel);
                }
            }

            TempData["message"] = "Invalid Input.";
            TempData["type"] = "error";
            ViewData["Title"] = Title;
            return View(fHrdEmployeeViewModel);
        }

        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> DeleteFHrdEmployee(string id)
        {
            var fHrdEmployee = await _fHrdEmployee.FindByIdAsync(int.Parse(_protector.Unprotect(id)));

            if (fHrdEmployee != null)
            {
                var fHrdEmpEducationList = await _fHrdEducation.GetAllEducationByEmpIdAsync(fHrdEmployee.EMPID);
                if (fHrdEmpEducationList.Any())
                {
                    if (await _fHrdEducation.DeleteRange(fHrdEmpEducationList))
                    {
                        await _fHrdEmployee.Delete(fHrdEmployee);
                        TempData["message"] = $"Successfully Deleted {Title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetFHrdEmployee));
                    }
                }

                TempData["message"] = $"Failed to Delete {Title}.";
                TempData["type"] = "error";
                return RedirectToAction(nameof(GetFHrdEmployee));
            }

            TempData["message"] = $"{Title} Not Found.";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetFHrdEmployee));
        }

        [HttpPost]
        [Route("GetData")]
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
                var data = await _fHrdEmployee.GetAllFHrdEmployeeAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.EMPNO != null && m.EMPNO.ToUpper().Contains(searchValue))
                                           || (m.PROX_CARD != null && m.PROX_CARD.ToUpper().Contains(searchValue))
                                           || (m.NAME != null && m.NAME.ToUpper().Contains(searchValue))
                                           || (m.NAME_BN != null && m.NAME_BN.ToUpper().Contains(searchValue))
                                           || (m.NID_NO != null && m.NID_NO.ToUpper().Contains(searchValue))
                                           || (m.GENDER != null && m.GENDER.GENNAME.ToUpper().Contains(searchValue))
                                           || (m.RELIGION != null && m.RELIGION.RELEGION_NAME.ToUpper().Contains(searchValue))
                                           || (m.BLDGRP != null && m.BLDGRP.BLDGRP_NAME.ToUpper().Contains(searchValue))
                                           //|| (m.COMPANY != null && m.COMPANY.COMPANY_NAME.ToUpper().Contains(searchValue))
                                           || (m.DESIG != null && m.DESIG.DES_NAME.ToUpper().Contains(searchValue))
                                           || (m.DEPT != null && m.DEPT.DEPTNAME.ToUpper().Contains(searchValue))
                                           || (m.SEC != null && m.SEC.SEC_NAME.ToUpper().Contains(searchValue))
                                           || (m.SUBSEC != null && m.SUBSEC.SUBSEC_NAME.ToUpper().Contains(searchValue))
                                           || (m.OD != null && m.OD.OD_FULL_NAME.ToUpper().Contains(searchValue))).ToList();
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
            catch (Exception e)
            {
                return Json(BadRequest(e));
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetDivisions")]
        public async Task<IActionResult> GetDivByNation (int nationId)
        {
            try
            {
                return Ok(await _fHrdEmployee.GetDivByNationIdAsync(nationId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetDistricts")]
        public async Task<IActionResult> GetDistByDiv (int divId)
        {
            try
            {
                return Ok(await _fHrdEmployee.GetDistByDivIdAsync(divId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetThana")]
        public async Task<IActionResult> GetThanaByDist (int distId)
        {
            try
            {
                return Ok(await _fHrdEmployee.GetThanaByDistIdAsync(distId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AcceptVerbs("Get", "Post")]
        [Route("GetUnion")]
        public async Task<IActionResult> GetUnionByThana (int thanaId)
        {
            try
            {
                return Ok(await _fHrdEmployee.GetUnionByThanaIdAsync(thanaId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("AddOrRemoveEducation")]
        public async Task<IActionResult> AddOrRemoveEduDetails(FHrdEmployeeViewModel fHrdEmployeeViewModel)
        {
            ModelState.Clear();
            if (fHrdEmployeeViewModel.IsDelete && fHrdEmployeeViewModel.RemoveIndex >= 0)
            {
                var item = fHrdEmployeeViewModel.FHrdEducationList[fHrdEmployeeViewModel.RemoveIndex];

                //if (item.EDUID > 0)
                //{
                //    await _fHrEmpEducation.Delete(item);
                //}

                fHrdEmployeeViewModel.FHrdEducationList.Remove(item);
            }
            else
            {
                if (fHrdEmployeeViewModel.FHrdEducationList.All(e => e.DEGID != fHrdEmployeeViewModel.FHrdEducation.DEGID))
                {
                    fHrdEmployeeViewModel.FHrdEducationList.Add(fHrdEmployeeViewModel.FHrdEducation);
                }
                else
                {
                    TempData["Duplicate"] = "Please Select Different Degree";
                    TempData["type"] = "error";
                }
            }

            return PartialView("EduDetailsTable", await _fHrdEmployee.GetInitDetailsObjEduByAsync(fHrdEmployeeViewModel));
        }

        [HttpPost]
        [Route("AddOrRemoveSpouse")]
        public async Task<IActionResult> AddOrRemoveSpouseDetails(FHrdEmployeeViewModel fHrdEmployeeViewModel)
        {
            ModelState.Clear();
            if (fHrdEmployeeViewModel.IsDelete && fHrdEmployeeViewModel.RemoveIndex >= 0)
            {
                var item = fHrdEmployeeViewModel.FHrdEmpSpouseList[fHrdEmployeeViewModel.RemoveIndex];

                //if (item.SPID > 0)
                //{
                //    await _fHrEmpEducation.Delete(item);
                //}

                fHrdEmployeeViewModel.FHrdEmpSpouseList.Remove(item);
            }
            else
            {
                fHrdEmployeeViewModel.FHrdEmpSpouseList.Add(fHrdEmployeeViewModel.FHrdEmpSpouse);
            }

            return PartialView("SpouseDetailsTable", fHrdEmployeeViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("EmpNoAlreadyInUse")]
        public async Task<IActionResult> IsEmpNoInUse(FHrdEmployeeViewModel fHrEmployeeViewModel)
        {
            var value = fHrEmployeeViewModel.FHrdEmployee.EMPNO;
            return await _fHrdEmployee.FindByValueAsync(value,"EmpNo") ? Json(true) : Json($"Employee No. {value} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("ProxNoAlreadyInUse")]
        public async Task<IActionResult> IsProximityInUse(FHrdEmployeeViewModel fHrEmployeeViewModel)
        {
            var value = fHrEmployeeViewModel.FHrdEmployee.PROX_CARD;
            return await _fHrdEmployee.FindByValueAsync(value,"Proximity") ? Json(true) : Json("This Proximity card already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("NIDAlreadyInUse")]
        public async Task<IActionResult> IsNidInUse(FHrdEmployeeViewModel fHrEmployeeViewModel)
        {
            var value = fHrEmployeeViewModel.FHrdEmployee.NID_NO;
            return await _fHrdEmployee.FindByValueAsync(value,"NID") ? Json(true) : Json($"National Identity No. {value} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("BIDAlreadyInUse")]
        public async Task<IActionResult> IsBidInUse(FHrdEmployeeViewModel fHrEmployeeViewModel)
        {
            var value = fHrEmployeeViewModel.FHrdEmployee.BID_NO;
            return await _fHrdEmployee.FindByValueAsync(value,"BID") ? Json(true) : Json($"Birth Certificate No. {value} already exists");
        }

        [AcceptVerbs("Get", "Post")]
        [Route("PassportAlreadyInUse")]
        public async Task<IActionResult> IsPassportInUse(FHrdEmployeeViewModel fHrEmployeeViewModel)
        {
            var value = fHrEmployeeViewModel.FHrdEmployee.PASSPORT;
            return await _fHrdEmployee.FindByValueAsync(value,"Passport") ? Json(true) : Json($"Passport No. {value} already exists");
        }
    }
}
