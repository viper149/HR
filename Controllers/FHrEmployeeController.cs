using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FHrEmployeeController : Controller
    {
        //private readonly IF_HRD_EMPLOYEE _fHrEmployee;
        //private readonly IF_BAS_DEPARTMENT _fBasDepartment;
        //private readonly IF_BAS_SECTION _fBasSection;
        //private readonly IUPOZILAS _fHrUpozilas;
        //private readonly IF_HR_EMP_EDUCATION _fHrEmpEducation;
        //private readonly IF_HR_EMP_OFFICIALINFO _fHrOfficialInfo;
        //private readonly IF_HR_EMP_FAMILYDETAILS _fHrFamilyDetails;
        //private readonly IF_HR_EMP_SALARYSETUP _fHrSalarySetup;
        //private readonly IProcessUploadFile _processUploadFile;
        //private readonly IDataProtector _protector;

        //public FHrEmployeeController(
        //    IDataProtectionProvider dataProtectionProvider,
        //    DataProtectionPurposeStrings dataProtectionPurposeStrings,
        //    IF_HRD_EMPLOYEE fHrEmployee,
        //    IF_BAS_DEPARTMENT fBasDepartment,
        //    IF_BAS_SECTION fBasSection,
        //    IF_HR_EMP_EDUCATION fHrEmpEducation,
        //    IUPOZILAS fHrUpozilas,
        //    IF_HR_EMP_OFFICIALINFO fHrOfficialInfo,
        //    IF_HR_EMP_FAMILYDETAILS fHrFamilyDetails,
        //    IF_HR_EMP_SALARYSETUP fHrSalarySetup,
        //    IProcessUploadFile processUploadFile)
        //{
        //    _fHrEmployee = fHrEmployee;
        //    _fBasDepartment = fBasDepartment;
        //    _fBasSection = fBasSection;
        //    _fHrEmpEducation = fHrEmpEducation;
        //    _fHrUpozilas = fHrUpozilas;
        //    _fHrOfficialInfo = fHrOfficialInfo;
        //    _fHrFamilyDetails = fHrFamilyDetails;
        //    _fHrSalarySetup = fHrSalarySetup;
        //    _processUploadFile = processUploadFile;
        //    _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        //}

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> IsEmpNoInUse(FHrEmployeeViewModel fHrEmployeeViewModel)
        //{
        //    var findByEmpNoInUseAsync = await _fHrEmployee.FindByEmpNoInUseAsync(fHrEmployeeViewModel.FHrdEmployee.EMPNO);
        //    return !findByEmpNoInUseAsync ? Json(true) : Json($"Employee Number [ {fHrEmployeeViewModel.FHrdEmployee.EMPNO} ] is already in use.");
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="employeeId"> Belongs to EMPID. Primary key. Must not to be null. <see cref="F_HRD_EMPLOYEE"/></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> DeleteFHrEmployee(string employeeId)
        //{
        //    try
        //    {
        //        var fHrEmployee = await _fHrEmployee.FindByIdIncludeAllForDeleteAsync(int.Parse(_protector.Unprotect(employeeId)));

        //        if (fHrEmployee != null)
        //        {
        //            // OFFICIAL INFO
        //            if (fHrEmployee.F_HR_EMP_OFFICIALINFO.Any())
        //            {
        //                await _fHrOfficialInfo.DeleteRange(fHrEmployee.F_HR_EMP_OFFICIALINFO);
        //            }

        //            // SALARY SETUP
        //            if (fHrEmployee.F_HR_EMP_SALARYSETUP.Any())
        //            {
        //                await _fHrSalarySetup.DeleteRange(fHrEmployee.F_HR_EMP_SALARYSETUP);
        //            }

        //            // EDUCATION SUMMARY
        //            if (fHrEmployee.F_HR_EMP_EDUCATION.Any())
        //            {
        //                await _fHrEmpEducation.DeleteRange(fHrEmployee.F_HR_EMP_EDUCATION);
        //            }

        //            // FAMILY DETAILS
        //            if (fHrEmployee.F_HR_EMP_FAMILYDETAILS.Any())
        //            {
        //                await _fHrFamilyDetails.DeleteRange(fHrEmployee.F_HR_EMP_FAMILYDETAILS);
        //            }

        //            // EMPLOYEE INFO
        //            await _fHrEmployee.Delete(fHrEmployee);

        //            TempData["message"] = "Successfully Deleted Employee Information.";
        //            TempData["type"] = "success";
        //        }
        //        else
        //        {
        //            TempData["message"] = "Failed To Delete Employee Information.";
        //            TempData["type"] = "success";
        //        }

        //        return RedirectToAction("GetEmployee", $"FHrdEmployee");
        //    }
        //    catch (Exception)
        //    {
        //        return View($"Error");
        //    }
        //}

        //[HttpGet]
        //[Route("Employees")]
        //[Route("Employees/GetAll")]
        //public IActionResult GetEmployee()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception)
        //    {
        //        return View($"Error");
        //    }
        //}

        //[HttpPost]
        //public async Task<JsonResult> GetTableData()
        //{
        //    try
        //    {
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault();
        //        var length = Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
        //        var pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        var skip = start != null ? Convert.ToInt32(start) : 0;

        //        var data = (List<GetFHrEmployeeViewModel>)await _fHrEmployee.GetAllEmployeesAsync();

        //        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        //        {
        //            data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
        //        }

        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            data = data.Where(m => !string.IsNullOrEmpty(m.EMPNO) && m.EMPNO.ToUpper().Contains(searchValue)
        //                                   || !string.IsNullOrEmpty(m.FULL_NAME) && m.FULL_NAME.ToUpper().Contains(searchValue)
        //                                   || !string.IsNullOrEmpty(m.DEPARTMENT) && m.DEPARTMENT.ToUpper().Contains(searchValue)
        //                                   || !string.IsNullOrEmpty(m.DESIGNATION) && m.DESIGNATION.ToUpper().Contains(searchValue)
        //                                   || !string.IsNullOrEmpty(m.SECTION) && m.SECTION.ToUpper().Contains(searchValue)
        //                                   || !string.IsNullOrEmpty(m.EMP_TYPE) && m.EMP_TYPE.ToUpper().Contains(searchValue)).ToList();
        //        }

        //        var recordsTotal = data.Count();
        //        var finalData = data.Skip(skip).Take(pageSize).ToList();

        //        return Json(new
        //        {
        //            draw,
        //            recordsFiltered = recordsTotal,
        //            recordsTotal,
        //            data = finalData
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> CreateEmployee()
        //{
        //    return View(await _fHrEmployee.GetInitObjects(new FHrEmployeeViewModel()));
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateEmployee(FHrEmployeeViewModel fHrEmployeeViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            #region PROCESS FILES

        //            fHrEmployeeViewModel.FHrdEmployee.PHOTO = _processUploadFile.ProceessFormFile(fHrEmployeeViewModel.FHrdEmployee.IMAGE);
        //            fHrEmployeeViewModel.FHrdEmployee.NID_UP = _processUploadFile.ProcessUploadFileToContentRootPath(fHrEmployeeViewModel.FHrdEmployee.NidFormFile, "/hr_assets/employees_files/nid");
        //            fHrEmployeeViewModel.FHrdEmployee.PASS_UP = _processUploadFile.ProcessUploadFileToContentRootPath(fHrEmployeeViewModel.FHrdEmployee.NidFormFile, "/hr_assets/employees_files/passport");
        //            fHrEmployeeViewModel.FHrdEmployee.D_LICENSE_UP = _processUploadFile.ProcessUploadFileToContentRootPath(fHrEmployeeViewModel.FHrdEmployee.NidFormFile, "/hr_assets/employees_files/driving_license");

        //            #endregion

        //            var isEmpInfoInsert = await _fHrEmployee.InsertByAsync(fHrEmployeeViewModel.FHrdEmployee);

        //            if (isEmpInfoInsert)
        //            {
        //                var fHrEmployeesQueryable = await _fHrEmployee.All();
        //                var fHrEmployee = fHrEmployeesQueryable.FirstOrDefault(e => e.EMPID.Equals(fHrEmployeeViewModel.FHrdEmployee.EMPID));

        //                if (fHrEmployee != null)
        //                {
        //                    fHrEmployeeViewModel.FHrEmpOfficialInfo.EMPID = fHrEmployee.EMPID;
        //                    fHrEmployeeViewModel.FHrEmpSalarySetup.EMPID = fHrEmployee.EMPID;

        //                    foreach (var item in fHrEmployeeViewModel.FHrEmpEducations)
        //                    {
        //                        item.EMPID = fHrEmployee.EMPID;
        //                    }

        //                    // OFFICIAL INFO
        //                    await _fHrOfficialInfo.InsertByAsync(fHrEmployeeViewModel.FHrEmpOfficialInfo);
        //                    // SALARY SETUP
        //                    await _fHrSalarySetup.InsertByAsync(fHrEmployeeViewModel.FHrEmpSalarySetup);
        //                    // EDUCATION SUMMARY
        //                    await _fHrEmpEducation.InsertRangeByAsync(fHrEmployeeViewModel.FHrEmpEducations);
        //                    // FAMILY DETAILS
        //                    await _fHrFamilyDetails.InsertRangeByAsync(fHrEmployeeViewModel.FHrEmpFamilyDetailsList.Select(e => { e.EMPID = fHrEmployee.EMPID; return e; }));

        //                    TempData["message"] = "Successfully added New Employee Information.";
        //                    TempData["type"] = "success";
        //                    return RedirectToAction("GetEmployee", $"FHrdEmployee");
        //                }

        //                TempData["message"] = "Failed to Add New Employee Information";
        //                TempData["type"] = "error";
        //                return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //            }

        //            TempData["message"] = "Failed to Add New Employee Information";
        //            TempData["type"] = "error";
        //            return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //        }

        //        TempData["message"] = "Invalid Input. Please Try Again.";
        //        TempData["type"] = "error";
        //        return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //    }
        //    catch (Exception)
        //    {
        //        TempData["message"] = "Failed to Add New Employee Information";
        //        TempData["type"] = "error";
        //        return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="employeeId"> Belongs to EMPID. Primary key. Must not to be null. <see cref="F_HRD_EMPLOYEE"/></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> EditEmployee(string employeeId)
        //{
        //    try
        //    {
        //        var fHrEmployee = await _fHrEmployee.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(employeeId)));

        //        if (fHrEmployee != null)
        //        {
        //            fHrEmployee.EncryptedId = _protector.Protect(fHrEmployee.EMPID.ToString());

        //            var fHrEmployeeViewModel = new FHrEmployeeViewModel
        //            {
        //                FHrdEmployee = fHrEmployee,
        //                FHrEmpOfficialInfo = fHrEmployee.F_HR_EMP_OFFICIALINFO.FirstOrDefault(),
        //                FHrEmpSalarySetup = fHrEmployee.F_HR_EMP_SALARYSETUP.FirstOrDefault(),
        //                FHrEmpEducations = fHrEmployee.F_HR_EMP_EDUCATION.ToList(),
        //                FHrEmpFamilyDetailsList = fHrEmployee.F_HR_EMP_FAMILYDETAILS.ToList()
        //            };

        //            return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //        }

        //        TempData["message"] = "Failed to Retrieve Employee Details.";
        //        TempData["type"] = "error";
        //        return RedirectToAction("GetEmployee", $"FHrdEmployee");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["message"] = "Failed to Retrieve Employee Details.";
        //        TempData["type"] = "error";
        //        return RedirectToAction("GetEmployee", $"FHrdEmployee");
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="employeeId"> Belongs to EMPID. Primary key. Must not to be null. <see cref="F_HRD_EMPLOYEE"/></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> DetailsEmpInfo(string employeeId)
        //{
        //    try
        //    {
        //        var result = await _fHrEmployee.FindByIdIncludeAllAsync(int.Parse(_protector.Unprotect(employeeId)));

        //        if (result != null)
        //        {
        //            var fHrEmployeeViewModel = new FHrEmployeeViewModel();
        //            result.EncryptedId = _protector.Protect(result.EMPID.ToString());

        //            fHrEmployeeViewModel.FHrdEmployee = result;

        //            return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //        }
        //        TempData["message"] = "Failed to Retrieve Employee Information.";
        //        TempData["type"] = "error";
        //        return RedirectToAction($"GetEmployee", $"FHrdEmployee");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        TempData["message"] = "Failed to Retrieve Employee Information.";
        //        TempData["type"] = "error";
        //        return RedirectToAction($"GetEmployee", $"FHrdEmployee");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditEmployee(FHrEmployeeViewModel fHrEmployeeViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var fHrEmployee = await _fHrEmployee.FindByIdAsync(int.Parse(_protector.Unprotect(fHrEmployeeViewModel.FHrdEmployee.EncryptedId)));

        //            #region PROCESS FILES

        //            fHrEmployeeViewModel.FHrdEmployee.EMPID = fHrEmployee.EMPID;
        //            fHrEmployeeViewModel.FHrdEmployee.PHOTO = _processUploadFile.ProceessFormFile(fHrEmployeeViewModel.FHrdEmployee.IMAGE) ?? fHrEmployee.PHOTO;
        //            fHrEmployeeViewModel.FHrdEmployee.NID_UP = fHrEmployeeViewModel.FHrdEmployee.NidFormFile != null ? _processUploadFile.ProcessUploadFileToContentRootPath(fHrEmployeeViewModel.FHrdEmployee.NidFormFile, "/hr_assets/employees_files/nid") : fHrEmployee.NID_UP;
        //            fHrEmployeeViewModel.FHrdEmployee.PASS_UP = fHrEmployeeViewModel.FHrdEmployee.PassportFormFile != null ? _processUploadFile.ProcessUploadFileToContentRootPath(fHrEmployeeViewModel.FHrdEmployee.PassportFormFile, "/hr_assets/employees_files/passport") : fHrEmployee.PASS_UP;
        //            fHrEmployeeViewModel.FHrdEmployee.D_LICENSE_UP = fHrEmployeeViewModel.FHrdEmployee.DrivingLicenseFormFile != null ? _processUploadFile.ProcessUploadFileToContentRootPath(fHrEmployeeViewModel.FHrdEmployee.DrivingLicenseFormFile, "/hr_assets/employees_files/driving_license") : fHrEmployee.D_LICENSE_UP;

        //            #endregion

        //            if (fHrEmployeeViewModel.FHrdEmployee.SAME_AS_PRESENT_ADDRESS)
        //            {
        //                fHrEmployeeViewModel.FHrdEmployee.PER_DISTRICT = fHrEmployeeViewModel.FHrdEmployee.PRE_DISTRICT;
        //                fHrEmployeeViewModel.FHrdEmployee.PER_DISTRICT_BNG = fHrEmployeeViewModel.FHrdEmployee.PRE_DISTRICT_BNG;
        //                fHrEmployeeViewModel.FHrdEmployee.PER_THANA = fHrEmployeeViewModel.FHrdEmployee.PRE_THANA;
        //                fHrEmployeeViewModel.FHrdEmployee.PER_THANA_BNG = fHrEmployeeViewModel.FHrdEmployee.PRE_THANA_BNG;
        //            }

        //            var isUpdated = await _fHrEmployee.Update(fHrEmployeeViewModel.FHrdEmployee);

        //            if (isUpdated)
        //            {
        //                fHrEmployeeViewModel.FHrEmpOfficialInfo.EMPID = fHrEmployee.EMPID;
        //                fHrEmployeeViewModel.FHrEmpSalarySetup.EMPID = fHrEmployee.EMPID;

        //                #region EDUCATION SUMMARY

        //                await _fHrEmpEducation.DeleteRange(fHrEmployeeViewModel.FHrEmpEducations.Where(e => e.EEID > 0));

        //                foreach (var item in fHrEmployeeViewModel.FHrEmpEducations)
        //                {
        //                    item.EMPID = fHrEmployee.EMPID;
        //                    item.EEID = 0;
        //                }

        //                await _fHrEmpEducation.InsertRangeByAsync(fHrEmployeeViewModel.FHrEmpEducations);

        //                #endregion

        //                #region FAMILY DETAILS

        //                await _fHrFamilyDetails.DeleteRange(fHrEmployeeViewModel.FHrEmpFamilyDetailsList.Where(e => e.EFID > 0));

        //                foreach (var item in fHrEmployeeViewModel.FHrEmpFamilyDetailsList)
        //                {
        //                    item.EMPID = fHrEmployee.EMPID;
        //                    item.EFID = 0;
        //                }

        //                await _fHrFamilyDetails.InsertRangeByAsync(fHrEmployeeViewModel.FHrEmpFamilyDetailsList);

        //                #endregion

        //                #region OFFICIAL INFO

        //                var fHrEmpOfficialinfo = await _fHrOfficialInfo.FindByEmpIdAsync(fHrEmployee.EMPID);

        //                if (fHrEmpOfficialinfo != null)
        //                {
        //                    fHrEmployeeViewModel.FHrEmpOfficialInfo.EOID = fHrEmpOfficialinfo.EOID;
        //                    fHrEmployeeViewModel.FHrEmpOfficialInfo.EMPID = fHrEmployee.EMPID;
        //                    await _fHrOfficialInfo.Update(fHrEmployeeViewModel.FHrEmpOfficialInfo);
        //                }
        //                else
        //                {
        //                    fHrEmployeeViewModel.FHrEmpOfficialInfo.EMPID = fHrEmployee.EMPID;
        //                    await _fHrOfficialInfo.InsertByAsync(fHrEmployeeViewModel.FHrEmpOfficialInfo);
        //                }

        //                #endregion

        //                #region SALARY SETUP

        //                var fHrEmpSalarysetup = await _fHrSalarySetup.FindByEmpIdAsync(fHrEmployee.EMPID);

        //                if (fHrEmpSalarysetup != null)
        //                {
        //                    fHrEmployeeViewModel.FHrEmpSalarySetup.ESID = fHrEmpSalarysetup.ESID;
        //                    fHrEmployeeViewModel.FHrEmpSalarySetup.EMPID = fHrEmployee.EMPID;
        //                    await _fHrSalarySetup.Update(fHrEmployeeViewModel.FHrEmpSalarySetup);
        //                }
        //                else
        //                {
        //                    if (fHrEmployeeViewModel.FHrEmpSalarySetup != null)
        //                    {
        //                        fHrEmployeeViewModel.FHrEmpSalarySetup.EMPID = fHrEmployee.EMPID;
        //                        await _fHrSalarySetup.InsertByAsync(fHrEmployeeViewModel.FHrEmpSalarySetup);
        //                    }
        //                }

        //                #endregion

        //                TempData["message"] = "Successfully Update Employee Information.";
        //                TempData["type"] = "success";
        //                return RedirectToAction("GetEmployee", $"FHrdEmployee");
        //            }

        //            TempData["message"] = "Failed to Update Information";
        //            TempData["type"] = "error";
        //            return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //        }

        //        TempData["message"] = "Invalid Input. Please Try Again.";
        //        TempData["type"] = "error";
        //        return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["message"] = ex.GetType();
        //        TempData["type"] = "error";
        //        return View(await _fHrEmployee.GetInitObjects(fHrEmployeeViewModel));
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddOrRemoveFamilyDetails(FHrEmployeeViewModel fHrEmployeeViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        if (fHrEmployeeViewModel.IsDelete && fHrEmployeeViewModel.RemoveIndex >= 0)
        //        {
        //            var item = fHrEmployeeViewModel.FHrEmpFamilyDetailsList[fHrEmployeeViewModel.RemoveIndex];
        //            if (item.EFID > 0)
        //            {
        //                await _fHrFamilyDetails.Delete(item);
        //            }

        //            fHrEmployeeViewModel.FHrEmpFamilyDetailsList.Remove(item);
        //        }
        //        else
        //        {
        //            var result = fHrEmployeeViewModel.FHrEmpFamilyDetailsList.Any(e => e.NAME.Equals(fHrEmployeeViewModel.FHrEmpFamilyDetails.NAME));

        //            if (!result)
        //            {
        //                fHrEmployeeViewModel.FHrEmpFamilyDetailsList.Add(fHrEmployeeViewModel.FHrEmpFamilyDetails);
        //            }
        //        }

        //        return PartialView($"FamilyDetailsTable", fHrEmployeeViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        return PartialView($"FamilyDetailsTable", fHrEmployeeViewModel);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddOrRemoveEduDetails(FHrEmployeeViewModel fHrEmployeeViewModel)
        //{
        //    try
        //    {
        //        ModelState.Clear();
        //        if (fHrEmployeeViewModel.IsDelete && fHrEmployeeViewModel.RemoveIndex >= 0)
        //        {
        //            var item = fHrEmployeeViewModel.FHrEmpEducations[fHrEmployeeViewModel.RemoveIndex];

        //            if (item.EEID > 0)
        //            {
        //                await _fHrEmpEducation.Delete(item);
        //            }

        //            fHrEmployeeViewModel.FHrEmpEducations.Remove(item);
        //        }
        //        else
        //        {
        //            var result = fHrEmployeeViewModel.FHrEmpEducations.Any(e => e.EXAM == fHrEmployeeViewModel.FHrEmpEducation.EXAM);
        //            if (!result)
        //            {
        //                fHrEmployeeViewModel.FHrEmpEducations.Add(fHrEmployeeViewModel.FHrEmpEducation);
        //            }
        //        }

        //        return PartialView($"EduDetailsTable", fHrEmployeeViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        return PartialView($"EduDetailsTable", fHrEmployeeViewModel);
        //    }
        //}

        //[HttpGet]
        //public async Task<List<UPOZILAS>> GetThanaByDistrictId(int id)
        //{
        //    try
        //    {
        //        var upozilas = await _fHrUpozilas.GetThanaByDistrictIdAsync(id);
        //        return upozilas.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        throw;
        //    }
        //}

        //[HttpGet]
        //public async Task<F_BAS_DEPARTMENT> GetSectionByDepartmentId(int id)
        //{
        //    try
        //    {
        //        var department = await _fBasDepartment.GetSectionByDepartmentIdAsync(id);
        //        return department;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        throw;
        //    }
        //}

        //[HttpGet]
        //public async Task<F_BAS_SECTION> GetSubSectionBySectionId(int id)
        //{
        //    try
        //    {
        //        var section = await _fBasSection.GetSubSectionBySectionId(id);
        //        return section;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Write(e.Message);
        //        throw;
        //    }
        //}
    }
}