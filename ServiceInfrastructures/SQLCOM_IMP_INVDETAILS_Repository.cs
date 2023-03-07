using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com.InvoiceImport;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_INVDETAILS_Repository : BaseRepository<COM_IMP_INVDETAILS>, ICOM_IMP_INVDETAILS
    {
        public SQLCOM_IMP_INVDETAILS_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<COM_IMP_INVDETAILS>> FindByInvNoAsync(string invNo)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_INVDETAILS.Where(e => e.INVNO.Equals(invNo)).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<COM_IMP_INVOICEINFO> GetSingleInvoiceDetails(int id)
        {
            return await DenimDbContext.COM_IMP_INVOICEINFO
                .Include(e => e.LC)
                .Include(e => e.ComImpInvdetailses)
                .ThenInclude(e => e.BasProductinfo)
                .FirstOrDefaultAsync(e => e.INVID.Equals(id));
        }

        public async Task<bool> FindByInvNoProdIdAsync(string invNo, int? prodId)
        {
            try
            {
                return await DenimDbContext.COM_IMP_INVDETAILS.AnyAsync(e => e.INVNO.Equals(invNo) && e.PRODID.Equals(prodId));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ComImpInvoiceInfoCreateViewModel> GetProductsByAsync(string search, int lcId, int page)
        {
            var comImpInvoiceInfoCreateViewModel = new ComImpInvoiceInfoCreateViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                comImpInvoiceInfoCreateViewModel.BasProductinfos = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Include(e => e.COM_IMP_LCDETAILS)
                    .ThenInclude(e => e.BAS_PRODUCTINFO)
                    .Where(e => e.LC_ID.Equals(lcId))
                    .SelectMany(e => e.COM_IMP_LCDETAILS.Select(f => new BAS_PRODUCTINFO
                    {
                        PRODID = f.BAS_PRODUCTINFO.PRODID,
                        PRODNAME = f.BAS_PRODUCTINFO.PRODNAME
                    }).Where(f => f.PRODNAME.ToLower().Contains(search.ToLower()))).ToListAsync();
            }
            else
            {
                comImpInvoiceInfoCreateViewModel.BasProductinfos = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Include(e => e.COM_IMP_LCDETAILS)
                    .ThenInclude(e => e.BAS_PRODUCTINFO)
                    .Where(e => e.LC_ID.Equals(lcId))
                    .SelectMany(e => e.COM_IMP_LCDETAILS.Select(f => new BAS_PRODUCTINFO
                    {
                        PRODID = f.BAS_PRODUCTINFO.PRODID,
                        PRODNAME = f.BAS_PRODUCTINFO.PRODNAME
                    })).ToListAsync();
            }

            return comImpInvoiceInfoCreateViewModel;
        }
    }
}
