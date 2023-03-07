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
    public class SQLF_PR_FINIGHING_DOFF_FOR_MACHINE_Repository:BaseRepository<F_PR_FINIGHING_DOFF_FOR_MACHINE>, IF_PR_FINIGHING_DOFF_FOR_MACHINE
    {
        public SQLF_PR_FINIGHING_DOFF_FOR_MACHINE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }


        public async Task<IEnumerable<F_PR_FINIGHING_DOFF_FOR_MACHINE>> GetInitDoffData(
            List<F_PR_FINIGHING_DOFF_FOR_MACHINE> fPrFinighingDoffForMachines, int processType)
        {
            try
            {
                foreach (var item in fPrFinighingDoffForMachines)
                {
                    item.DOFF = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER
                        .Include(c=>c.F_PR_FINISHING_FNPROCESS)
                        .Include(c=>c.DOFF.LOOM_NONavigation)
                        .Include(c => c.DOFF.LOOM_TYPENavigation)
                        .Include(c => c.DOFF.OTHER_DOFF)
                        .Include(c => c.DOFF.WV_BEAM.F_PR_FINISHING_BEAM_RECEIVE)
                        .ThenInclude(c => c.SET.PROG_)
                        .Include(c => c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                        .Include(c => c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                        .Where(c => c.FN_PROCESSID.Equals(item.FN_PROCESSID))
                        .Select(c=>new F_PR_FINISHING_PROCESS_MASTER
                        {
                            OPT1 = c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}- {c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})" : $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}-{c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})",
                            OPT2 = c.F_PR_FINISHING_FNPROCESS.Where(e=>e.FIN_PRO_TYPEID.Equals(processType) && e.FN_PROCESSID.Equals(c.FN_PROCESSID)).Select(d=>d.LENGTH_OUT).FirstOrDefault().ToString()
                        })
                        .FirstOrDefaultAsync();
                }

                return fPrFinighingDoffForMachines;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
