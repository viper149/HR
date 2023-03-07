using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic.YarnCountInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    public class BasYarnCountinfoController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly IBAS_YARN_COUNTINFO _bAsYarnCountinfo;
        private readonly IBAS_YARN_COUNT_LOT_INFO _basYarnCountLotInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBAS_PRODUCTINFO _basProductinfo;
        private readonly string _title = "Yarn Count Information";

        public BasYarnCountinfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IBAS_YARN_COUNTINFO basYarnCountinfo,
            IBAS_YARN_COUNT_LOT_INFO basYarnCountLotInfo,
            UserManager<ApplicationUser> userManager,
            IBAS_PRODUCTINFO basProductinfo)
        {
            _bAsYarnCountinfo = basYarnCountinfo;
            _basYarnCountLotInfo = basYarnCountLotInfo;
            _userManager = userManager;
            _basProductinfo = basProductinfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsBasYarnCountinfoInUse(CreateBasYarnCountInfoViewModel countInfoViewModel)
        {
            var findByCountnameByAsync = await _bAsYarnCountinfo.FindByCountnameByAsync(countInfoViewModel.BasYarnCountinfo.COUNTNAME);
            return findByCountnameByAsync ? Json($"Yarn Count '{countInfoViewModel.BasYarnCountinfo.COUNTNAME}' already in use.") : Json(true);
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

                var data = await _bAsYarnCountinfo.GetCountListByAsync();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.COUNTNAME.ToUpper().Contains(searchValue)
                                           || m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue)
                                           || m.UNIT != null && m.UNIT.ToUpper().Contains(searchValue)
                                           || m.RND_COUNTNAME != null && m.RND_COUNTNAME.ToUpper().Contains(searchValue)
                                           || m.BAS_COLOR.COLOR != null && m.BAS_COLOR.COLOR.ToUpper().Contains(searchValue)
                                           || m.PART_.PART_NO != null && m.PART_.PART_NO.ToUpper().Contains(searchValue)
                                           || m.YARN_CAT_.CATEGORY_NAME != null && m.YARN_CAT_.CATEGORY_NAME.ToUpper().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                        .ToList();
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
            catch (Exception)
            {
                return Json(BadRequest());
            }
        }

        [HttpGet]
        public IActionResult GetBasYarnCountinfoList()
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

        [HttpPost]
        public async Task<IActionResult> CreateBasYarnCountinfo(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    createBasYarnCountInfoViewModel.BasYarnCountinfo.RND_COUNTNAME = createBasYarnCountInfoViewModel.BasYarnCountinfo.COUNTNAME;
                    createBasYarnCountInfoViewModel.BasYarnCountinfo.CREATED_AT = DateTime.Now;
                    createBasYarnCountInfoViewModel.BasYarnCountinfo.UPDATED_AT = DateTime.Now;
                    createBasYarnCountInfoViewModel.BasYarnCountLotInfo.CREATED_BY = user.Id;
                    createBasYarnCountInfoViewModel.BasYarnCountLotInfo.UPDATED_BY = user.Id;

                    var countId = await _bAsYarnCountinfo.InsertByAndGetIdAsync(createBasYarnCountInfoViewModel.BasYarnCountinfo);

                    if (countId != 0)
                    {
                        createBasYarnCountInfoViewModel.BasProductinfo = new BAS_PRODUCTINFO
                        {
                            PRODNAME = createBasYarnCountInfoViewModel.BasYarnCountinfo.COUNTNAME,
                            CATID = 11,                                    //From BasProductCategory
                            UNIT = createBasYarnCountInfoViewModel.BasYarnCountinfo.UNIT == "KG" ? 1 : 41,
                            REMARKS = createBasYarnCountInfoViewModel.BasYarnCountinfo.REMARKS,
                            DESCRIPTION = createBasYarnCountInfoViewModel.BasYarnCountinfo.DESCRIPTION,
                            YSID = countId
                        };

                        if (await _basProductinfo.InsertByAsync(createBasYarnCountInfoViewModel.BasProductinfo))
                        {
                            foreach (var item in createBasYarnCountInfoViewModel.BasYarnCountLotInfoList)
                            {
                                item.COUNTID = countId;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_BY = user.Id;
                                item.CREATED_AT = DateTime.Now;
                                item.UPDATED_AT = DateTime.Now;
                            }

                            await _basYarnCountLotInfo.InsertRangeByAsync(createBasYarnCountInfoViewModel.BasYarnCountLotInfoList);
                            TempData["message"] = $"Successfully Created {_title}.";
                            TempData["type"] = "success";
                            return RedirectToAction(nameof(GetBasYarnCountinfoList));
                        }

                        await _bAsYarnCountinfo.Delete(createBasYarnCountInfoViewModel.BasYarnCountinfo);
                    }

                    TempData["message"] = $"Failed to Create {_title}.";
                    TempData["type"] = "error";
                    return View(await _bAsYarnCountinfo.GetInitObjects(createBasYarnCountInfoViewModel));
                }
                catch (Exception)
                {
                    TempData["message"] = $"Failed to Create {_title}";
                    TempData["type"] = "error";
                    return View(await _bAsYarnCountinfo.GetInitObjects(createBasYarnCountInfoViewModel));
                }
            }

            return View(await _bAsYarnCountinfo.GetInitObjects(createBasYarnCountInfoViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> CreateBasYarnCountinfo()
        {
            try
            {
                return View(await _bAsYarnCountinfo.GetInitObjects(new CreateBasYarnCountInfoViewModel()));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBasYarnCountinfo(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bAsYarnCountinfo.FindByIdAsync(int.Parse(_protector.Unprotect(createBasYarnCountInfoViewModel.BasYarnCountinfo.EncryptedId)));

                    if (result != null)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        result.RND_COUNTNAME = createBasYarnCountInfoViewModel.BasYarnCountinfo.RND_COUNTNAME;
                        result.COUNTNAME = createBasYarnCountInfoViewModel.BasYarnCountinfo.COUNTNAME;
                        result.DESCRIPTION = createBasYarnCountInfoViewModel.BasYarnCountinfo.DESCRIPTION;
                        result.UNIT = createBasYarnCountInfoViewModel.BasYarnCountinfo.UNIT;
                        result.COLOR = createBasYarnCountInfoViewModel.BasYarnCountinfo.COLOR;
                        result.REMARKS = createBasYarnCountInfoViewModel.BasYarnCountinfo.REMARKS;
                        result.YARN_CAT_ID = createBasYarnCountInfoViewModel.BasYarnCountinfo.YARN_CAT_ID;
                        result.UPDATED_BY = user.Id;
                        result.UPDATED_AT = DateTime.Now;
                        
                        if (await _bAsYarnCountinfo.Update(result))
                        {
                            foreach (var item in createBasYarnCountInfoViewModel.BasYarnCountLotInfoList.Where(c => c.ID.Equals(0)))
                            {
                                item.COUNTID = result.COUNTID;
                                item.CREATED_BY = user.Id;
                                item.UPDATED_BY = user.Id;
                                item.UPDATED_AT = DateTime.Now;
                                item.CREATED_AT = DateTime.Now;
                            }

                            if (await _basYarnCountLotInfo.InsertRangeByAsync(
                                    createBasYarnCountInfoViewModel.BasYarnCountLotInfoList.Where(c => c.ID.Equals(0))))
                            {
                                TempData["message"] = $"Successfully Updated {_title}.";
                                TempData["type"] = "success";
                                return RedirectToAction(nameof(GetBasYarnCountinfoList));
                            }
                        }
                    }
                }

                TempData["message"] = $"Failed to update {_title}.";
                TempData["type"] = "error";
                return View(await _bAsYarnCountinfo.GetInitObjects(createBasYarnCountInfoViewModel));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                TempData["type"] = "error";
                return View(await _bAsYarnCountinfo.GetInitObjects(createBasYarnCountInfoViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBasYarnCountinfo(string basYarnCountinfoId)
        {
            try
            {
                var result = await _bAsYarnCountinfo.FindByCountIdAsync(int.Parse(_protector.Unprotect(basYarnCountinfoId)));
                return result != null ? View(await _bAsYarnCountinfo.GetInitObjects(result)) : View("Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = "Failed to Open Edit Page.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasYarnCountinfoList", "BasYarnCountinfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBasYarnCountinfo(string basYarnCountinfoId)
        {
            try
            {
                var result = await _bAsYarnCountinfo.FindByIdAsync(int.Parse(_protector.Unprotect(basYarnCountinfoId)));

                if (result != null)
                {
                    var flag = await _basYarnCountLotInfo.DeleteCountLotAsync(result.COUNTID);

                    if (flag)
                    {
                        var f = await _bAsYarnCountinfo.Delete(result);

                        if (f)
                        {
                            TempData["message"] = "Successfully Deleted Basic Yarn Count Info.";
                            TempData["type"] = "success";
                            return RedirectToAction("GetBasYarnCountinfoList", "BasYarnCountinfo");
                        }

                        TempData["message"] = "Failed to Delete Count Info.";
                        TempData["type"] = "error";
                        return RedirectToAction("GetBasYarnCountinfoList", "BasYarnCountinfo");
                    }

                    TempData["message"] = "Failed to Delete Count Info.";
                    TempData["type"] = "error";
                    return RedirectToAction("GetBasYarnCountinfoList", "BasYarnCountinfo");
                }

                TempData["message"] = "Failed to Delete Count Info.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasYarnCountinfoList", "BasYarnCountinfo");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["message"] = "Failed to Delete Count Info.";
                TempData["type"] = "error";
                return RedirectToAction("GetBasYarnCountinfoList", "BasYarnCountinfo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsBasYarnCountinfo(string basYarnCountinfoId)
        {
            try
            {
                var result = await _bAsYarnCountinfo.FindByIdAsync(int.Parse(_protector.Unprotect(basYarnCountinfoId)));

                if (result != null)
                {
                    result.EncryptedId = _protector.Protect(result.COUNTID.ToString());
                    return View(result);
                }

                ViewBag.ErrorMessage = $"Inavalid input [ {basYarnCountinfoId} ]. Please find the correct id to get the result";
                return View("NotFound");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"Invalid input [ {basYarnCountinfoId} ], not found!";
                return View("NotFound");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLotToList(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel)
        {
            try
            {
                ModelState.Clear();
                var flag = createBasYarnCountInfoViewModel.BasYarnCountLotInfoList.Where(c => c.LOTID.Equals(createBasYarnCountInfoViewModel.BasYarnCountLotInfo.LOTID));

                if (!flag.Any())
                {
                    createBasYarnCountInfoViewModel.BasYarnCountLotInfoList.Add(createBasYarnCountInfoViewModel.BasYarnCountLotInfo);
                }
                createBasYarnCountInfoViewModel = await GetLotDetailsAsync(createBasYarnCountInfoViewModel);
                return PartialView("AddLotToList", createBasYarnCountInfoViewModel);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                createBasYarnCountInfoViewModel = await GetLotDetailsAsync(createBasYarnCountInfoViewModel);
                return PartialView("AddLotToList", createBasYarnCountInfoViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLotFromList(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            if (createBasYarnCountInfoViewModel.BasYarnCountLotInfoList[int.Parse(removeIndexValue)].ID != 0)
            {
                await _basYarnCountLotInfo.Delete(createBasYarnCountInfoViewModel.BasYarnCountLotInfoList[int.Parse(removeIndexValue)]);
            }
            createBasYarnCountInfoViewModel.BasYarnCountLotInfoList.RemoveAt(int.Parse(removeIndexValue));
            createBasYarnCountInfoViewModel = await GetLotDetailsAsync(createBasYarnCountInfoViewModel);
            return PartialView("AddLotToList", createBasYarnCountInfoViewModel);
        }

        private async Task<CreateBasYarnCountInfoViewModel> GetLotDetailsAsync(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel)
        {
            try
            {
                createBasYarnCountInfoViewModel = await _basYarnCountLotInfo.GetLotDetailsAsync(createBasYarnCountInfoViewModel);

                return createBasYarnCountInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLotList(int countId)
        {
            try
            {
                ModelState.Clear();

                var createBasYarnCountInfoViewModel = new CreateBasYarnCountInfoViewModel
                {
                    BasYarnCountLotInfoList = (List<BAS_YARN_COUNT_LOT_INFO>)await _bAsYarnCountinfo.GetLotList(countId)
                };

                createBasYarnCountInfoViewModel = await GetLotDetailsAsync(createBasYarnCountInfoViewModel);
                return PartialView("AddLotToList", createBasYarnCountInfoViewModel);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                var createBasYarnCountInfoViewModel = new CreateBasYarnCountInfoViewModel();
                createBasYarnCountInfoViewModel = await GetLotDetailsAsync(createBasYarnCountInfoViewModel);
                return PartialView("AddLotToList", createBasYarnCountInfoViewModel);
            }
        }
    }
}