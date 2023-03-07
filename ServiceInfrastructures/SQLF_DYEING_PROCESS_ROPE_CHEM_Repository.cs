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
    public class SQLF_DYEING_PROCESS_ROPE_CHEM_Repository:BaseRepository<F_DYEING_PROCESS_ROPE_CHEM>, IF_DYEING_PROCESS_ROPE_CHEM
    {
        public SQLF_DYEING_PROCESS_ROPE_CHEM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_DYEING_PROCESS_ROPE_CHEM>> GetInitChemData(List<F_DYEING_PROCESS_ROPE_CHEM> fDyeingProcessRopeChems)
        {
            try
            {
                foreach (var item in fDyeingProcessRopeChems)
                {
                    item.CHEM_PROD_ = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                        .Include(c=>c.F_DYEING_PROCESS_ROPE_CHEM)
                        .Where(c => c.PRODUCTID.Equals(item.CHEM_PROD_ID)).FirstOrDefaultAsync();
                }
                return fDyeingProcessRopeChems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
