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
    public class SQLPL_SAMPLE_PROG_SETUP_Repository : BaseRepository<PL_SAMPLE_PROG_SETUP>, IPL_SAMPLE_PROG_SETUP
    {
        public SQLPL_SAMPLE_PROG_SETUP_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<PL_SAMPLE_PROG_SETUP>> GetAllBySdIdAsync(int sdId)
        {
            try
            {
                var result = await DenimDbContext.PL_SAMPLE_PROG_SETUP.Where(c => c.SDID.Equals(sdId)).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PL_SAMPLE_PROG_SETUP> FindByProgIdAsync(int progId)
        {
            return await DenimDbContext.PL_SAMPLE_PROG_SETUP.Where(e => e.TRNSID.Equals(progId)).FirstOrDefaultAsync();
        }
    }
}
