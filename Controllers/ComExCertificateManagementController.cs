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
    public class ComExCertificateManagementController : Controller
    {

        private readonly ICOM_EX_CERTIFICATE_MANAGEMENT _comExCertificateManagement;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly string title = " ComExCertificateManagement ";

        public ComExCertificateManagementController(IDataProtectionProvider dataProtectionProvider,
            ICOM_EX_CERTIFICATE_MANAGEMENT comExCertificateManagement,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager
        )
        {
            _comExCertificateManagement = comExCertificateManagement;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        [HttpGet]
        public IActionResult GetComExCertificateManagement()
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

            var data = (List<COM_EX_CERTIFICATE_MANAGEMENT>)await _comExCertificateManagement.GetAllComExCertificateManagement();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => m.INV.INVNO.ToString().ToUpper().Contains(searchValue)
                                       //|| m.ORGANIC_TYPE.ToString().ToUpper().Contains(searchValue)
                                       //|| m.ORGANIC_REF.ToString().ToUpper().Contains(searchValue)
                                       //|| m.IC_TYPE.ToString().ToUpper().Contains(searchValue)
                                       //|| m.IC_REF.ToString().ToUpper().Contains(searchValue)
                                       //|| m.RCS_REF.ToString().ToUpper().Contains(searchValue)
                                       //|| m.GRS_REF.ToString().ToUpper().Contains(searchValue)
                                       //|| m.PSCP_REF.ToString().ToUpper().Contains(searchValue)
                                       ).ToList();
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

        public async Task<IActionResult> CreateComExCertificateManagement()
        {
            


            try
            {
                var comExCertificateManagement = await _comExCertificateManagement.GetInitObjByAsync(new ComExCertificateManagementViewModel(), false);

                comExCertificateManagement.ComExCertificateManagement = new COM_EX_CERTIFICATE_MANAGEMENT
                {
                    TRNSDATE = DateTime.Now
                };

                return View(comExCertificateManagement);
            }
            catch (Exception e)
            {
                return View($"Error");
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateComExCertificateManagement(ComExCertificateManagementViewModel comExCertificateManagementViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    comExCertificateManagementViewModel.ComExCertificateManagement.CREATED_AT = DateTime.Now;
                    comExCertificateManagementViewModel.ComExCertificateManagement.CREATED_BY = user.Id;
                    comExCertificateManagementViewModel.ComExCertificateManagement.UPDATED_AT = DateTime.Now;
                    comExCertificateManagementViewModel.ComExCertificateManagement.UPDATED_BY = user.Id;
                    comExCertificateManagementViewModel.ComExCertificateManagement.TRNSDATE = DateTime.Now;

                    var result = await _comExCertificateManagement.InsertByAsync(comExCertificateManagementViewModel.ComExCertificateManagement);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added ";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(CreateComExCertificateManagement), $"ComExCertificateManagement");

                    }

                    TempData["message"] = "Failed to Add .";
                    TempData["type"] = "error";
                    return View(comExCertificateManagementViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(comExCertificateManagementViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add .";
                TempData["type"] = "error";
                return View(comExCertificateManagementViewModel);
            }
        }





        [HttpGet]
        public async Task<IActionResult> EditComExCertificateManagement(string lbId)
        {
            var comExCertificateManagement = await _comExCertificateManagement.FindByIdAsync(int.Parse(_protector.Unprotect(lbId)));

            if (comExCertificateManagement != null)
            {
                var comExCertificateManagementViewModel = await _comExCertificateManagement.GetInitObjByAsync(new ComExCertificateManagementViewModel(), true);
                comExCertificateManagementViewModel.ComExCertificateManagement = comExCertificateManagement;

                comExCertificateManagementViewModel.ComExCertificateManagement.EncryptedId = _protector.Protect(comExCertificateManagementViewModel.ComExCertificateManagement.CMID.ToString());
                return View(comExCertificateManagementViewModel);
            }

            TempData["message"] = $"{title} Not Found";
            TempData["type"] = "error";
            return RedirectToAction(nameof(GetComExCertificateManagement), $"ComExCertificateManagement");
        }

        [HttpPost]
        public async Task<IActionResult> EditComExCertificateManagement(ComExCertificateManagementViewModel comExCertificateManagementViewModel)
        {
            if (ModelState.IsValid)
            {

                comExCertificateManagementViewModel.ComExCertificateManagement.CMID = int.Parse(_protector.Unprotect(comExCertificateManagementViewModel.ComExCertificateManagement.EncryptedId));

                var fHrdLeave = await _comExCertificateManagement.FindByIdAsync(comExCertificateManagementViewModel.ComExCertificateManagement.CMID);
                if (fHrdLeave != null)
                {
                    comExCertificateManagementViewModel.ComExCertificateManagement.UPDATED_BY = (await _userManager.GetUserAsync(User)).Id;
                    comExCertificateManagementViewModel.ComExCertificateManagement.UPDATED_AT = DateTime.Now;
                    comExCertificateManagementViewModel.ComExCertificateManagement.CREATED_AT = fHrdLeave.CREATED_AT;
                    comExCertificateManagementViewModel.ComExCertificateManagement.CREATED_BY = fHrdLeave.CREATED_BY;

                    if (await _comExCertificateManagement.Update(comExCertificateManagementViewModel.ComExCertificateManagement))
                    {
                        TempData["message"] = $"Successfully Updated {title}.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(GetComExCertificateManagement), $"ComExCertificateManagement");
                    }
                    TempData["message"] = $"Failed to Update {title}.";
                    TempData["type"] = "error";
                    return View(comExCertificateManagementViewModel);
                }
                TempData["message"] = $"{title} Not Found.";
                TempData["type"] = "error";
                return View(comExCertificateManagementViewModel);
            }
            TempData["message"] = "Invalid Input. Please Try Again.";
            TempData["type"] = "error";
            return View(comExCertificateManagementViewModel);
        }




        //[AcceptVerbs("Get", "Post")]
        //public string GetAllByINVNO(int id)
        //{
        //    return "PIONEER DENIM";
        //} 
        
        
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> GetAllByINVNO(int id)
        {
            try
            {
                return Ok(await _comExCertificateManagement.GetAllByIdAsync(id));

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }






    }
}
