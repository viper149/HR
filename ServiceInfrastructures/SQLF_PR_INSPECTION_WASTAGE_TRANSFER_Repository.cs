using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_WASTAGE_TRANSFER_Repository : BaseRepository<F_PR_INSPECTION_WASTAGE_TRANSFER>, IF_PR_INSPECTION_WASTAGE_TRANSFER
    {
        public SQLF_PR_INSPECTION_WASTAGE_TRANSFER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_INSPECTION_WASTAGE_TRANSFER>> GetAllInspectionWastageTransferAsync()
        {
            return await DenimDbContext.F_PR_INSPECTION_WASTAGE_TRANSFER.ToListAsync();
        }
    }
}
