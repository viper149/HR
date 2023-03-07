using System;
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
    public class FFabricStoreController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IF_FS_FABRIC_RCV_MASTER _fFsFabricRcvMaster;
        private readonly IF_FS_FABRIC_RCV_DETAILS _fFsFabricRcvDetails;

        public FFabricStoreController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager,
            IF_FS_FABRIC_RCV_MASTER fFsFabricRcvMaster,
            IF_FS_FABRIC_RCV_DETAILS fFsFabricRcvDetails
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _userManager = userManager;
            _fFsFabricRcvMaster = fFsFabricRcvMaster;
            _fFsFabricRcvDetails = fFsFabricRcvDetails;
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

                var data = await _fFsFabricRcvDetails.GetRollListAsync();

                if (User.IsInRole("Marketing"))
                {
                    data = data.Where(c => c.IS_QC_APPROVE);
                }

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
                    data = data.Where(m =>
                        m.FABCODENavigation.STYLE_NAME != null && m.FABCODENavigation.STYLE_NAME.ToUpper().Contains(searchValue)
                        || (m.RCV.RCVDATE != null && m.RCV.RCVDATE.ToString().Contains(searchValue))
                        || (m.PO_NONavigation.PINO != null && m.PO_NONavigation.PINO.ToUpper().Contains(searchValue))
                        || (m.SO_NONavigation.SO_NO != null && m.SO_NONavigation.SO_NO.ToUpper().Contains(searchValue))
                        || (m.ROLL_.ROLLNO != null && m.ROLL_.ROLLNO.ToUpper().Contains(searchValue))
                        || (m.QTY_YARDS != null && m.QTY_YARDS.ToString().Contains(searchValue))
                        || (m.BALANCE_QTY != null && m.BALANCE_QTY.ToString().Contains(searchValue))
                        || (m.OPT1 != null && m.OPT1.ToUpper().Contains(searchValue))
                        || (m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue))
                    ).ToList();
                }
                //data = data.ToList();
                var cosStandardConses = data.ToList();
                recordsTotal = cosStandardConses.Count();
                var finalData = cosStandardConses.Skip(skip).Take(pageSize).ToList();

                foreach (var item in finalData)
                {
                    item.EncryptedId = _protector.Protect(item.RCVID.ToString());
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
        public IActionResult GetApprovedRollList()
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
    }
}