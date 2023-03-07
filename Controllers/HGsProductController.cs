using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class HGsProductController : Controller
    {
        private readonly IH_GS_PRODUCT _hGsProduct;
        private readonly IH_GS_ITEM_SUBCATEGORY _h_GS_ITEM_SUBCATEGORY;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public HGsProductController(IH_GS_PRODUCT hGsProduct,
            IH_GS_ITEM_SUBCATEGORY h_GS_ITEM_SUBCATEGORY,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager)
        {
            _hGsProduct = hGsProduct;
            _h_GS_ITEM_SUBCATEGORY = h_GS_ITEM_SUBCATEGORY;
            _dataProtectionProvider = dataProtectionProvider;
            _userManager = userManager;
        }
        public IActionResult GetHGsProduct()
        {
            return View();
        }

        [HttpPost]
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

            var data = (List<H_GS_PRODUCT>)await _hGsProduct.GetAllProductInformationAsync();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(m => (m.PRODNAME != null && m.PRODNAME.ToUpper().Contains(searchValue))
                                       || (m.PARTNO != null && m.PARTNO.ToUpper().Contains(searchValue))
                                       || (m.PRODID.ToString().ToUpper().Contains(searchValue))
                                       || (m.PROD_LOC != null && m.PROD_LOC.ToUpper().Contains(searchValue))
                                       || (m.SCAT.SCATNAME != null && m.SCAT.SCATNAME.ToUpper().Contains(searchValue))
                                       || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))).ToList();
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
    }
}
