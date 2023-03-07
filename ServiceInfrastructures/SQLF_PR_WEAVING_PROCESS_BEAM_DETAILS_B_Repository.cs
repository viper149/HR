using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_PROCESS_BEAM_DETAILS_B_Repository:BaseRepository<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>, IF_PR_WEAVING_PROCESS_BEAM_DETAILS_B
    {
        public SQLF_PR_WEAVING_PROCESS_BEAM_DETAILS_B_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_LOOM_MACHINE_NO>> GetLoomMachines(int loomId)
        {
            try
            {
                var result = await DenimDbContext.F_LOOM_MACHINE_NO.Where(c => c.LOOM_TYPE.Equals(loomId)).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PrWeavingProcessBulkViewModel> GetInitData(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList)
                {
                    item.LoomMachine = await DenimDbContext.F_LOOM_MACHINE_NO
                        .FirstOrDefaultAsync(c =>
                            c.ID.Equals(item.LOOM_ID));

                    item.F_PR_SIZING_PROCESS_ROPE_DETAILS = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_DETAILS
                            .Include(c=>c.W_BEAM)
                            .FirstOrDefaultAsync(c =>
                            c.SDID.Equals(item.BEAMID));

                    item.F_PR_SLASHER_DYEING_DETAILS = await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                            .Include(c=>c.W_BEAM)
                            .FirstOrDefaultAsync(c =>
                            c.SLDID.Equals(item.SBEAMID));


                    foreach (var i in item.FPrWeavingProcessDetailsBList)
                    {
                        i.LOOM_NONavigation = await DenimDbContext.F_LOOM_MACHINE_NO.FirstOrDefaultAsync(c =>
                            c.ID.Equals(i.LOOM_NO));
                        i.LOOM_TYPENavigation = await DenimDbContext.LOOM_TYPE.FirstOrDefaultAsync(c =>
                            c.LOOMID.Equals(i.LOOM_TYPE));
                        i.DOFFER_NAMENavigation = await DenimDbContext.F_HRD_EMPLOYEE.FirstOrDefaultAsync(c =>
                            c.EMPID.Equals(i.DOFFER_NAME));
                    }
                }

                prWeavingProcessBulkViewModel.OtherDoffs = await DenimDbContext.F_PR_WEAVING_OTHER_DOFF.Select(c=>new F_PR_WEAVING_OTHER_DOFF
                {
                    ID = c.ID,
                    NAME = c.NAME
                }).ToListAsync();
                prWeavingProcessBulkViewModel.Weavings = await DenimDbContext.RND_SAMPLE_INFO_WEAVING.Select(c=>new RND_SAMPLE_INFO_WEAVING
                {
                    WVID = c.WVID,
                    FABCODE = c.FABCODE
                }).ToListAsync();

                return prWeavingProcessBulkViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_WEAVING_PROCESS_BEAM_DETAILS_B fPrWeavingProcessBeamDetails)
        {
            try
            {
                await DenimDbContext.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.AddAsync(fPrWeavingProcessBeamDetails);
                await SaveChanges();
                return fPrWeavingProcessBeamDetails.WV_BEAMID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}
