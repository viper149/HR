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
    public class SQLRND_ANALYSIS_SHEET_DETAILS_Repository : BaseRepository<RND_ANALYSIS_SHEET_DETAILS>, IRND_ANALYSIS_SHEET_DETAILS
    {
        public SQLRND_ANALYSIS_SHEET_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<RND_ANALYSIS_SHEET_DETAILS>> GetAnalysisYarnDetailsList(int aid)
        {
            try
            {
                return await DenimDbContext.RND_ANALYSIS_SHEET_DETAILS
                    .Include(e => e.BasYarnCountinfo)
                    .Where(c => c.AID.Equals(aid)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}
