using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_LCDETAILS_Repository : BaseRepository<COM_IMP_LCDETAILS>, ICOM_IMP_LCDETAILS
    {
        public SQLCOM_IMP_LCDETAILS_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<COM_IMP_LCDETAILS>> FindByLcNoAsync(string lcNo)
        {
            return await DenimDbContext.COM_IMP_LCDETAILS.Where(e => e.LCNO.Equals(lcNo)).ToListAsync();
        }

        public async Task<COM_IMP_LCDETAILS> FindByLcNoAndProdIdAsync(string lcNo, int? prodId, string piNo)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_LCDETAILS.Where(e => e.LCNO.Equals(lcNo) && e.PRODID.Equals(prodId) && e.PINO.Equals(piNo)).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ComImpLcInformationForCreateViewModel> GetInitObjForDetailsTable(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            foreach (var item in comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILs)
            {
                item.BAS_PRODUCTINFO = await DenimDbContext.BAS_PRODUCTINFO.FirstOrDefaultAsync(e => e.PRODID.Equals(item.PRODID));
                item.F_BAS_UNITS = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));
            }

            return comImpLcInformationForCreateViewModel;
        }

        public async Task<COM_IMP_LCDETAILS> GetLcNoByTransId(int id)
        {
            return await DenimDbContext.COM_IMP_LCDETAILS
                .Include(d => d.LC)
                .Where(d => d.TRNSID.Equals(id))
                .Select(d => new COM_IMP_LCDETAILS
                {
                    LC_ID = d.LC_ID,
                    LC = new COM_IMP_LCINFORMATION
                    {
                        LC_ID = d.LC.LC_ID,
                        LCNO = d.LC.LCNO
                    }
                }).FirstOrDefaultAsync();
        }
    }
}
