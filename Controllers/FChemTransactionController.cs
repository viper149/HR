using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    [Authorize]
    [Route("ChemicalTransaction")]
    public class FChemTransactionController : Controller
    {
        private readonly IF_CHEM_TRANSECTION _fChemTransection;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public FChemTransactionController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_CHEM_TRANSECTION fChemTransection,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fChemTransection = fChemTransection;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [Route("GetAll")]
        public IActionResult GetChemTransaction()
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

        [HttpPost]
        [Route("GetTableData")]
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

                var data = (List<F_CHEM_TRANSECTION>)await _fChemTransection.GetAllTransactions();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = sortColumnDirection == "asc" ? data.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty)?.GetValue(c)).ToList() : data.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c)).ToList();
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.CTRID.ToString().ToUpper().Contains(searchValue)
                                           || m.PRODUCT.PRODUCTNAME != null && m.PRODUCT.PRODUCTNAME.ToUpper().Contains(searchValue)
                                           || m.CTRDATE != null && m.CTRDATE.ToString().Contains(searchValue)
                                           || m.CRCVID != null && m.CRCVID.ToString().Contains(searchValue)
                                           || m.RCVT.RCVTYPE != null && m.RCVT.RCVTYPE.ToUpper().Contains(searchValue)
                                           || m.CISSUE is {CISSUE: { }} && m.CISSUE.CISSUE.CISSUEID.ToString().ToUpper().Contains(searchValue)
                                           || m.ISSUE.ISSUTYPE != null && m.ISSUE.ISSUTYPE.ToString().Contains(searchValue)
                                           || m.OP_BALANCE != null && m.OP_BALANCE.ToString().Contains(searchValue)
                                           || m.RCV_QTY.ToString().ToUpper().Contains(searchValue)
                                           || m.ISSUE_QTY != null && m.ISSUE_QTY.ToString().Contains(searchValue)
                                           || m.BALANCE != null && m.BALANCE.ToString().Contains(searchValue)
                                           || m.CRCV.BATCHNO != null && m.CRCV.BATCHNO.ToString().Contains(searchValue)
                                           || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)).ToList();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}