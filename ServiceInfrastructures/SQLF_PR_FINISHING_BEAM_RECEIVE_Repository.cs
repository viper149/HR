using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_FINISHING_BEAM_RECEIVE_Repository : BaseRepository<F_PR_FINISHING_BEAM_RECEIVE>, IF_PR_FINISHING_BEAM_RECEIVE
    {
        public SQLF_PR_FINISHING_BEAM_RECEIVE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FPrFinishingBeamReceiveViewModel> GetInitObjects(
            FPrFinishingBeamReceiveViewModel fPrFinishingBeamReceiveViewModel)
        {
            try
            {


                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(163) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                fPrFinishingBeamReceiveViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                if (fPrFinishingBeamReceiveViewModel.FPrFinishingBeamReceive == null)
                {
                    fPrFinishingBeamReceiveViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Where(c => DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID))
                        //&& !_denimDbContext.F_PR_FINISHING_BEAM_RECEIVE.All(e => e.SETID.Equals(c.SETID))
                        )
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }
                else
                {
                    fPrFinishingBeamReceiveViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Where(c => DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID)) && DenimDbContext.F_PR_FINISHING_BEAM_RECEIVE.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }

                fPrFinishingBeamReceiveViewModel.FPrWeavingProcessBeamDetailsBs = await DenimDbContext
                    .F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c=>c.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Select(c=>new TypeTableViewModel
                    {
                        ID = c.WV_BEAMID,
                        Name = c.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO??c.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO
                    })
                    .ToListAsync();

                fPrFinishingBeamReceiveViewModel.RndFabricInfos = await DenimDbContext.RND_FABRICINFO
                    .Include(c => c.WV)
                    .ToListAsync();

                return fPrFinishingBeamReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_FINISHING_BEAM_RECEIVE>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_BEAM_RECEIVE
                    .Include(c => c.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c=>c.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.FABCODENavigation.WV)
                    .Include(c => c.RCVBYNavigation)
                    .Include(c => c.SET.PROG_)
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
