using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_SLASHER_CHEM_CONSM_Repository:BaseRepository<F_PR_SLASHER_CHEM_CONSM>, IF_PR_SLASHER_CHEM_CONSM
    {
        public SQLF_PR_SLASHER_CHEM_CONSM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<IEnumerable<F_PR_SLASHER_CHEM_CONSM>> GetInitChemData(List<F_PR_SLASHER_CHEM_CONSM> fPrSlasherChemConsms)
        {
            try
            {
                foreach (var item in fPrSlasherChemConsms)
                {
                    item.CHEM_PROD = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                        .Include(c => c.F_PR_SIZING_PROCESS_ROPE_CHEM)
                        .Where(c => c.PRODUCTID.Equals(item.CHEM_PRODID)).FirstOrDefaultAsync();

                    item.UNITNavigation = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(c => c.UID.Equals(item.UNIT));
                }
                return fPrSlasherChemConsms;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
