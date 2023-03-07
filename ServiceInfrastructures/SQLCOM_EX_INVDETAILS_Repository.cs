using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com.InvoiceExport;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_INVDETAILS_Repository : BaseRepository<COM_EX_INVDETAILS>, ICOM_EX_INVDETAILS
    {
        public SQLCOM_EX_INVDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<COM_EX_INVDETAILS>> FindByInvNoAsync(string invNo)
        {
            try
            {
                var comExInvdetailses = await DenimDbContext.COM_EX_INVDETAILS
                    .Include(e => e.ComExFabstyle)
                    .ThenInclude(e => e.FABCODENavigation)
                    .AsNoTracking()
                    .Where(e => e.INVNO.Equals(invNo))
                    .ToListAsync();
                return comExInvdetailses;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> FindByInvNoStyleIdAsync(string invNo, int styleId)
        {
            try
            {
                return await DenimDbContext.COM_EX_INVDETAILS.AsNoTracking().AnyAsync(e => e.INVNO.Equals(invNo) && e.PIIDD_TRNSID.Equals(styleId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ComExInvoiceMasterCreateViewModel> GetInitObjByAsync(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel)
        {
            foreach (var item in comExInvoiceMasterCreateViewModel.ComExInvdetailses)
            {
                if (item.IS_OLD)
                {
                    item.PiDetails = await DenimDbContext.COM_EX_PI_DETAILS
                        .Include(e => e.STYLE.FABCODENavigation)
                        .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.PIIDD_TRNSID));
                }
                else
                {
                    item.ComExFabstyle = await DenimDbContext.COM_EX_FABSTYLE
                        .Include(c => c.FABCODENavigation)
                        .FirstOrDefaultAsync(c => c.STYLEID.Equals(item.STYLEID));
                }
            }

            if (comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC != null)
            {
                var comExLcinfo = await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.COM_EX_LCDETAILS)
                    .FirstOrDefaultAsync(e => e.LCID.Equals(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID));

                comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC = comExLcinfo;
                comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC.COM_EX_LCDETAILS = comExLcinfo.COM_EX_LCDETAILS;

                //comExInvoiceMasterCreateViewModel.PreviousValue = (double?) await _denimDbContext.COM_EX_INVOICEMASTER
                //    .Include(e => e.ComExInvdetailses)
                //    .Where(e => e.LCID.Equals(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID))
                //    .SumAsync(e => e.ComExInvdetailses.Sum(f => f.QTY));
            }

            return comExInvoiceMasterCreateViewModel;
        }

        public async Task<int> FindByTrnsIdAsync(int trnsId)
        {
            var comExPiDetails = await DenimDbContext.COM_EX_PI_DETAILS.Include(e => e.STYLE.FABCODENavigation)
                .FirstOrDefaultAsync(e => e.TRNSID.Equals(trnsId));

            return comExPiDetails.STYLE.STYLEID;
        }

        public async Task<COM_EX_INVDETAILS> GetSingleInvDetails(int trnsId)
        {
            return await DenimDbContext.COM_EX_INVDETAILS
                .Include(d=>d.PiDetails)
                .Where(d => d.TRNSID.Equals(trnsId))
                .Select(d=>new COM_EX_INVDETAILS
                {
                    TRNSID = d.TRNSID,
                    INVID = d.INVID,
                    INVNO = d.INVNO,
                    QTY = d.QTY,
                    RATE = d.RATE,
                    STYLEID = d.STYLEID,
                    AMOUNT = d.AMOUNT,
                    ROLL = d.ROLL,
                    REMARKS = d.REMARKS,
                    PiDetails = new COM_EX_PI_DETAILS()
                    {
                        TRNSID = d.PiDetails.TRNSID
                    }
                }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<COM_EX_LCINFO> GetInvoicAmountByLcno(string id)
        {

            try
            {
                //    var a = await _denimDbContext.COM_EX_LCINFO
                //    .Where(d => d.LCNO.Equals(id))
                //    .Include(d => d.COM_EX_INVOICEMASTER)


                //    .Select(e =>
                //    {
                //        var b = e.Sum(COM_EX_INVOICEMASTER.INV_AMOUNT;
                //    });

                var a = await DenimDbContext.COM_EX_LCINFO
                 
                 .Include(d => d.COM_EX_INVOICEMASTER)
                 .Where(d => d.FILENO.Equals(id))
                 .FirstOrDefaultAsync();

                return a;
            }
            catch
            {

                return null;

            }
        
        }
    }
}
