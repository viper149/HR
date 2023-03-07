using System;
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
    public class FFsFabricClearence2ndBeamController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_FS_FABRIC_CLEARENCE_2ND_BEAM _fFsFabricClearence2NdBeam;
        private readonly IRND_FABTEST_BULK _rndFabtestBulk;
        private readonly IRND_FABTEST_GREY _rndFabtestGrey;

        public FFsFabricClearence2ndBeamController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_FS_FABRIC_CLEARENCE_2ND_BEAM fFsFabricClearence2NdBeam,
            IRND_FABTEST_BULK rndFabtestBulk,
            IRND_FABTEST_GREY rndFabtestGrey
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fFsFabricClearence2NdBeam = fFsFabricClearence2NdBeam;
            _rndFabtestBulk = rndFabtestBulk;
            _rndFabtestGrey = rndFabtestGrey;
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

                var data = await _fFsFabricClearence2NdBeam.GetAllAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m =>
                                                m.TRIAL_NO != null && m.TRIAL_NO.ToString().ToUpper().Contains(searchValue)
                                            || (m.OPT1 != null && m.OPT1.ToString().Contains(searchValue))
                                            || (m.OPT2 != null && m.OPT2.ToString().Contains(searchValue))
                                            || (m.OPT3 != null && m.OPT3.ToString().Contains(searchValue))
                                            || (m.OPT4 != null && m.OPT4.ToString().Contains(searchValue))
                                            || (m.OPT5 != null && m.OPT5.ToString().Contains(searchValue))
                                            || (m.QUALITY_COMMENTS != null && m.QUALITY_COMMENTS.ToString().Contains(searchValue))
                                            || (m.OPT7 != null && m.OPT7.ToString().Contains(searchValue))
                                            || (m.OPT6 != null && m.OPT6.ToUpper().Contains(searchValue))
                                        ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.AID.ToString());
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
        public IActionResult Get2ndBeamList()
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
        public async Task<IActionResult> Create2ndBeam()
        {
            try
            {
                var fFsFabricClearance2NdBeamViewModel = await GetInfo(new FFsFabricClearance2ndBeamViewModel());
                var user = await _userManager.GetUserAsync(User);

                fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam = new F_FS_FABRIC_CLEARENCE_2ND_BEAM
                {
                    ADATE = DateTime.Now,
                    EMPID = user.EMPID
                };

                return View(fFsFabricClearance2NdBeamViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create2ndBeam(FFsFabricClearance2ndBeamViewModel fFsFabricClearance2NdBeamViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fFsFabricClearence2NdBeam.InsertByAsync(fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam);

                    if (result)
                    {
                        TempData["message"] = "Successfully Added 2nd Beam Information.";
                        TempData["type"] = "success";
                        return RedirectToAction("Get2ndBeamList", $"FFsFabricClearence2ndBeam");
                    }

                    TempData["message"] = "Failed to Add 2nd Beam  Information.";
                    TempData["type"] = "error";
                    return View(fFsFabricClearance2NdBeamViewModel);
                }

                TempData["message"] = "Please Fill All The Fields with Valid Data.";
                TempData["type"] = "error";
                return View(fFsFabricClearance2NdBeamViewModel);
            }
            catch (Exception)
            {
                TempData["message"] = "Failed to Add 2nd Beam  Information.";
                TempData["type"] = "error";
                return View(fFsFabricClearance2NdBeamViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit2ndBeam(string id)
        {

            var fFsFabricClearence2NdBeam = await _fFsFabricClearence2NdBeam.FindByIdAsync(int.Parse(_protector.Unprotect(id)));
            fFsFabricClearence2NdBeam.EncryptedId = id;
            var fFsFabricClearance2NdBeamViewModel = new FFsFabricClearance2ndBeamViewModel
            {
                FFsFabricClearence2NdBeam = fFsFabricClearence2NdBeam
            };
            return View(await GetInfo(fFsFabricClearance2NdBeamViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> Edit2ndBeam(FFsFabricClearance2ndBeamViewModel fFsFabricClearance2NdBeamViewModel)
        {
            if (ModelState.IsValid)
            {
                var fFsFabricClearence2NdBeam = await _fFsFabricClearence2NdBeam.FindByIdAsync(int.Parse(_protector.Unprotect(fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam.EncryptedId)));

                if (fFsFabricClearence2NdBeam != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam.UPDATED_BY = user.Id;
                    fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam.AID = fFsFabricClearence2NdBeam.AID;
                    fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam.UPDATED_AT = DateTime.Now;
                    fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam.CREATED_AT = fFsFabricClearence2NdBeam.CREATED_AT;
                    fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam.CREATED_BY = fFsFabricClearence2NdBeam.CREATED_BY;

                    if (await _fFsFabricClearence2NdBeam.Update(fFsFabricClearance2NdBeamViewModel.FFsFabricClearence2NdBeam))
                    {
                        TempData["message"] = "Successfully Updated Chemical Information.";
                        TempData["type"] = "success";
                        return RedirectToAction(nameof(Get2ndBeamList), $"FFsFabricClearence2ndBeam");
                    }
                }
                else
                {
                    TempData["message"] = "Failed To Update Chemical Information.";
                    TempData["type"] = "error";
                }
            }
            else
            {
                TempData["message"] = "Invalid Form Submission.";
                TempData["type"] = "error";
            }

            return View(nameof(Edit2ndBeam), await GetInfo(fFsFabricClearance2NdBeamViewModel));
        }

        private async Task<FFsFabricClearance2ndBeamViewModel> GetInfo(FFsFabricClearance2ndBeamViewModel fFsFabricClearance2NdBeamViewModel)
        {
            try
            {
                fFsFabricClearance2NdBeamViewModel = await _fFsFabricClearence2NdBeam.GetInitData(fFsFabricClearance2NdBeamViewModel);
                return fFsFabricClearance2NdBeamViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSetDetails(string setId)
        {
            return Ok(await _fFsFabricClearence2NdBeam.GetSetDetails(int.Parse(setId)));
        }

        [HttpGet]
        public async Task<RND_FABTEST_GREY> GetLabGDetails(int labGId)
        {
            try
            {
                return await _rndFabtestGrey.FindByIdAsync(labGId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<RND_FABTEST_BULK> GetLabBDetails(int labBId)
        {
            try
            {
                return await _rndFabtestBulk.FindByIdAsync(labBId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}