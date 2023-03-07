using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.GatePass
{
    public class SQLF_SAMPLE_DESPATCH_DETAILS_Repository : BaseRepository<F_SAMPLE_DESPATCH_DETAILS>, IF_SAMPLE_DESPATCH_DETAILS
    {
        public SQLF_SAMPLE_DESPATCH_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_SAMPLE_DESPATCH_DETAILS>> FindByDispatchIdAsync(int dispatchId)
        {
            return await DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Where(e => e.DPID.Equals(dispatchId)).ToListAsync();
        }
    }
}
