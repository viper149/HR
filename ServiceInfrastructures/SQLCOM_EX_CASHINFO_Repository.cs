using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_CASHINFO_Repository : BaseRepository<COM_EX_CASHINFO>, ICOM_EX_CASHINFO
    {
        private readonly IDataProtector _protector;

        public SQLCOM_EX_CASHINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_EX_CASHINFO>> GetAllAsync()
        {
            try
            {
                return await DenimDbContext.COM_EX_CASHINFO
                    .Include(c => c.LC)
                    .Select(e => new COM_EX_CASHINFO
                    {
                        CASHID = e.CASHID,
                        EncryptedId = _protector.Protect(e.CASHID.ToString()),
                        CASHNO = e.CASHNO,
                        ITEMQTY_YDS = e.ITEMQTY_YDS,
                        VCNO = e.VCNO,
                        RATE = e.RATE,
                        BACKTOBACK_LCTYPE = e.BACKTOBACK_LCTYPE,
                        SUBDATE = e.SUBDATE,
                        RCVDDATE = e.RCVDDATE,
                        DELIVERY_DATE = e.DELIVERY_DATE,
                        REMARKS = e.REMARKS,
                        LC = new COM_EX_LCINFO
                        {
                            LCNO = e.LC.LCNO
                        }
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ComExCashInfoViewModel> GetInitObjects(ComExCashInfoViewModel comExCashInfoViewModel)
        {
            var currency = new Dictionary<int, string>
            {
                {2, "USD$"},
                {11, "Euro€"}
            };

            comExCashInfoViewModel.CurrenciesList = await DenimDbContext.CURRENCY
                .Where(d => currency.ContainsKey(d.Id) || currency.ContainsValue(d.CODE))
                .Select(d => new CURRENCY
                {
                    Id = d.Id,
                    CODE = d.CODE
                }).OrderBy(d=>d.Id).ToListAsync();

            comExCashInfoViewModel.ComExLcInfos = await DenimDbContext.COM_EX_LCINFO
                .Select(e=> new COM_EX_LCINFO
                {
                    LCID = e.LCID,
                    LCNO = $"{e.LCNO} ({e.FILENO})"
                }).OrderByDescending(c => c.LCID).ToListAsync();
            return comExCashInfoViewModel;
        }

        public async Task<IEnumerable<COM_EX_LCINFO>> FindLCByIdAsync(int lcId)
        {
            try
            {
                //var lcNo = await _denimDbContext.COM_EX_LCINFO.Where(c => c.LCID.Equals(lcId)).Select(c => c.LCNO).FirstOrDefaultAsync();

                var comExLcInfo = await DenimDbContext.COM_EX_LCINFO
                    .Include(c => c.COM_EX_INVOICEMASTER)
                    .ThenInclude(c => c.ComExInvdetailses)
                    .Include(c => c.COM_EX_LCDETAILS)
                    .ThenInclude(c => c.PI)
                    .ThenInclude(c => c.COM_EX_PI_DETAILS)
                    .ThenInclude(c => c.STYLE.FABCODENavigation.WV)
                    .Include(c => c.COM_EX_LCDETAILS)
                    .ThenInclude(c => c.PI)
                    .ThenInclude(c => c.COM_EX_PI_DETAILS)
                    .ThenInclude(c => c.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Where(c => c.LCID.Equals(lcId)).ToListAsync();

                return comExLcInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
