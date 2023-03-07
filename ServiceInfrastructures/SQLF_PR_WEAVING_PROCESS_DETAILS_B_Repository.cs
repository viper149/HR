using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_PROCESS_DETAILS_B_Repository:BaseRepository<F_PR_WEAVING_PROCESS_DETAILS_B>, IF_PR_WEAVING_PROCESS_DETAILS_B
    {
        public SQLF_PR_WEAVING_PROCESS_DETAILS_B_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }


        public async Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> FindByIdAllAsync(int beamId)
        {
            try
            {
                var result =
                    await DenimDbContext.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                        .Include(c=>c.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)

                        .Include(c=>c.F_PR_WEAVING_PROCESS_DETAILS_B)
                        .ThenInclude(c=>c.DOFFER_NAMENavigation)

                        .Include(c=>c.F_PR_WEAVING_PROCESS_DETAILS_B)
                        .ThenInclude(c=>c.LOOM_NONavigation)

                        .Include(c=>c.F_PR_WEAVING_PROCESS_DETAILS_B)
                        .ThenInclude(c=>c.LOOM_TYPENavigation)

                        .Include(c=>c.F_PR_WEAVING_PROCESS_DETAILS_B)
                        .ThenInclude(c=>c.OTHER_DOFF)

                        .FirstOrDefaultAsync(c => c.WV_BEAMID.Equals(beamId));

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetBeamDetailsByDoffIdAsync(int doffId)
        {
            try
            {
                var finishProcessList = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Where(c => c.DOFF_ID.Equals(doffId))
                    .ToListAsync();

                var doffDetails = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Where(c => c.TRNSID.Equals(doffId)).FirstOrDefaultAsync();

                if (finishProcessList.Any())
                {
                    var doffSum = finishProcessList.Sum(c => c.LENGTH_BEAM);
                    
                    var length = doffDetails.LENGTH_BULK - (doffSum ?? 0);

                    return length.ToString();
                }

                return doffDetails.LENGTH_BULK.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
