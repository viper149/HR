using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Receive
{
    public class SQLF_SAMPLE_GARMENT_RCV_D_Repository : BaseRepository<F_SAMPLE_GARMENT_RCV_D>, IF_SAMPLE_GARMENT_RCV_D
    {
        public SQLF_SAMPLE_GARMENT_RCV_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_SAMPLE_GARMENT_RCV_D>> FindBySrIdAsync(int srId)
        {
            return await DenimDbContext.F_SAMPLE_GARMENT_RCV_D.Where(e => e.SGRID.Equals(srId)).ToListAsync();
        }
    }
}
