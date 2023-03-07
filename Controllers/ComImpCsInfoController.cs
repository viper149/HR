using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ComImpCsInfoController : Controller
    {
        private readonly ICOM_IMP_CSINFO _comImpCsInfo;    
        private readonly IBAS_PRODUCTINFO _basProductInfo;
        private readonly IBAS_SUPPLIERINFO _basSupplierInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICOM_IMP_CSITEM_DETAILS _comImpCsItemDetails;
        private readonly ICOM_IMP_CSRAT_DETAILS _comImpCsRatDetails;
        private readonly IF_YS_INDENT_MASTER _fYsIndentMaster;
        private readonly IF_YS_INDENT_DETAILS _fYsIndentDetails;
        private readonly IBAS_YARN_COUNTINFO _basYarnCountInfo;
        private readonly IDataProtector _protector;

        public ComImpCsInfoController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ICOM_IMP_CSINFO comImpCsInfo,
            IBAS_PRODUCTINFO basProductInfo,
            IBAS_SUPPLIERINFO basSupplierInfo,
            UserManager<ApplicationUser> userManager,
            ICOM_IMP_CSITEM_DETAILS comImpCsItemDetails,
            ICOM_IMP_CSRAT_DETAILS comImpCsRatDetails,
            IF_YS_INDENT_MASTER fYsIndentMaster,
            IF_YS_INDENT_DETAILS fYsIndentDetails,
            IBAS_YARN_COUNTINFO basYarnCountInfo
        )
        {
            _comImpCsInfo = comImpCsInfo;
            _basProductInfo = basProductInfo;
            _basSupplierInfo = basSupplierInfo;
            _userManager = userManager;
            _comImpCsItemDetails = comImpCsItemDetails;
            _comImpCsRatDetails = comImpCsRatDetails;
            _fYsIndentMaster = fYsIndentMaster;
            _fYsIndentDetails = fYsIndentDetails;
            _basYarnCountInfo = basYarnCountInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
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
                var data = (List<COM_IMP_CSINFO>)await _comImpCsInfo.GetAll();

                foreach (var item in data)
                {
                    item.EncryptedId = _protector.Protect(item.CSID.ToString());
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {

                    switch (sortColumnDirection)
                    {
                        case "asc" when sortColumn != null && sortColumn.Contains("."):
                            {
                                var subStrings = sortColumn.Split(".");
                                data = data.OrderBy(c =>
                                    c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType().GetProperty(subStrings[1])
                                        ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                break;
                            }
                        case "asc":
                            data = data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c))
                                .ToList();
                            break;
                        default:
                            {
                                if (sortColumn != null && sortColumn.Contains("."))
                                {
                                    var subStrings = sortColumn.Split(".");
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(subStrings[0])?.GetValue(c).GetType()
                                            .GetProperty(subStrings[1])
                                            ?.GetValue(c.GetType().GetProperty(subStrings[0])?.GetValue(c))).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(c =>
                                        c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList();
                                }

                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CSNO.ToUpper().Contains(searchValue)
                                           || (m.CSDATE.ToString().ToUpper().Contains(searchValue))
                                           || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var totalRecords = data.Count();
                var finalData = data.Skip(skip).Take(pageSize);

                foreach (var item in finalData)
                {
                    var r = await _fYsIndentMaster.FindByIdAsync((int)item.INDID);
                    item.IND = r;
                }

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
        public IActionResult GetCsInfo()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> CreateCsInfo()
        {
            var result = await GetInfo(new ComImpCsInfoViewModel());
            return View(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCsInfo(ComImpCsInfoViewModel comImpCsInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    comImpCsInfoViewModel.ComImpCsInfo.CREATED_BY = user.Id;
                    var csId = await _comImpCsInfo.InsertAndGetIdAsync(comImpCsInfoViewModel.ComImpCsInfo);

                    if (csId != 0)
                    {
                        foreach (var item in comImpCsInfoViewModel.ComImpCsItemDetailsList)
                        {
                            item.CSID = csId;
                            var csItemId = await _comImpCsItemDetails.InsertAndGetIdAsync(item);
                            foreach (var i in item.ComImpCsRatDetailsList)
                            {
                                i.SUPP = null;
                                i.CSITEMID = csItemId;
                                await _comImpCsRatDetails.InsertByAsync(i);
                            }
                        }
                        TempData["message"] = "Successfully added CS Info.";
                        TempData["type"] = "success";
                        return RedirectToAction($"GetCsInfo", $"ComImpCsInfo");
                    }
                    TempData["message"] = "Failed to Add CS Info.";
                    TempData["type"] = "error";
                    return View(await GetInfo(comImpCsInfoViewModel));
                }
                TempData["message"] = "Invalid Input. Please Try Again.";
                TempData["type"] = "error";
                return View(await GetInfo(comImpCsInfoViewModel));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                TempData["message"] = "Failed to Add CS Info.";
                TempData["type"] = "error";
                return View(await GetInfo(comImpCsInfoViewModel));
            }
        }
        
        public async Task<ComImpCsInfoViewModel> GetInfo(ComImpCsInfoViewModel comImpCsInfoViewModel)
        {
            var itemList = await _basProductInfo.GetAll();
            var supplierList = await _basSupplierInfo.GetAll();
            var indentList = await _fYsIndentMaster.GetAll();

            comImpCsInfoViewModel.ProductInfos = itemList.ToList();
            comImpCsInfoViewModel.SupplierInfos = supplierList.ToList();
            comImpCsInfoViewModel.YsIndentMasters = indentList.ToList();
            return comImpCsInfoViewModel;
        }
        
        public async Task<ComImpCsInfoViewModel> GetNamesAsync(ComImpCsInfoViewModel comImpCsInfoViewModel)
        {
            foreach (var item in comImpCsInfoViewModel.ComImpCsItemDetailsList)
            {
                item.ITEMNAME = await _basYarnCountInfo.FindCountNameByIdAsync(item.ITEMID);
                foreach (var i in item.ComImpCsRatDetailsList)
                {
                    i.SUPP = new BAS_SUPPLIERINFO
                    {
                        SUPPNAME = await _basSupplierInfo.FindSupplierNameByIdAsync((int) i.SUPPID),
                        COM_IMP_CSRAT_DETAILS = null
                    };
                }
            }
            return comImpCsInfoViewModel;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCsDetails(ComImpCsInfoViewModel comImpCsInfoViewModel)
        {
            try
            {
                var result = comImpCsInfoViewModel.ComImpCsItemDetailsList.Any(c =>
                    c.ITEMID == comImpCsInfoViewModel.ComImpCsItemDetails.ITEMID);
                if (result)
                {
                    var item = comImpCsInfoViewModel.ComImpCsItemDetailsList.FirstOrDefault(c => c.ITEMID == comImpCsInfoViewModel.ComImpCsItemDetails.ITEMID);

                    var flag = item.ComImpCsRatDetailsList.Any(c => c.SUPPID.Equals(comImpCsInfoViewModel.ComImpCsRatDetails.SUPPID));

                    if (!flag)
                    {
                        item?.ComImpCsRatDetailsList.Add(comImpCsInfoViewModel.ComImpCsRatDetails);
                    }
                    comImpCsInfoViewModel = await GetNamesAsync(comImpCsInfoViewModel);
                    return PartialView($"RateList", comImpCsInfoViewModel);
                }
                comImpCsInfoViewModel.ComImpCsItemDetails.ComImpCsRatDetailsList.Add(comImpCsInfoViewModel.ComImpCsRatDetails);
                comImpCsInfoViewModel.ComImpCsItemDetailsList.Add(comImpCsInfoViewModel.ComImpCsItemDetails);

                comImpCsInfoViewModel = await GetNamesAsync(comImpCsInfoViewModel);

                return PartialView($"RateList", comImpCsInfoViewModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                comImpCsInfoViewModel = await GetNamesAsync(comImpCsInfoViewModel);
                return PartialView($"RateList", comImpCsInfoViewModel);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveCsFromList(ComImpCsInfoViewModel comImpCsInfoViewModel, string removeIndexValue)
        {
            ModelState.Clear();
            comImpCsInfoViewModel.ComImpCsItemDetailsList.RemoveAt(int.Parse(removeIndexValue));
            return PartialView($"RateList", comImpCsInfoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveCsRateDetails(ComImpCsInfoViewModel comImpCsInfoViewModel, string removeIndexValue,string rateRemoveIndex)
        {
            ModelState.Clear();
            comImpCsInfoViewModel.ComImpCsItemDetailsList[int.Parse(removeIndexValue)]
                .ComImpCsRatDetailsList.RemoveAt(int.Parse(rateRemoveIndex));
            return PartialView($"RateList", comImpCsInfoViewModel);
        }

        [HttpGet]
        public async Task<IEnumerable<BAS_YARN_COUNTINFO>> GetIndentYarnList(int indId)
        {
            var yarnList = await _fYsIndentDetails.GetIndentYarnListByIndidAsync(indId);
            return yarnList;
        }

        [HttpGet]
        public async Task<F_YS_INDENT_DETAILS> GetIndentQty(int prodId, int indId)
        {
            var indentQty = await _fYsIndentDetails.GetIndentQtyAsync(prodId, indId);
            return indentQty;
        }

    }
}