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
    public class SQLRND_SAMPLE_INFO_DETAILS_Repository : BaseRepository<RND_SAMPLE_INFO_DETAILS>, IRND_SAMPLE_INFO_DETAILS
    {
        public SQLRND_SAMPLE_INFO_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<RND_SAMPLE_INFO_DETAILS>> GetAllBySdIdAsync(int sdId)
        {
            try
            {
                var result = await DenimDbContext.RND_SAMPLE_INFO_DETAILS.Where(c => c.SDID.Equals(sdId)).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<RND_SAMPLE_INFO_DETAILS>> FindBySdIdAsync(int sdId)
        {
            return await DenimDbContext.RND_SAMPLE_INFO_DETAILS.Where(e => e.SDID.Equals(sdId)).ToListAsync();
        }
    }
}
