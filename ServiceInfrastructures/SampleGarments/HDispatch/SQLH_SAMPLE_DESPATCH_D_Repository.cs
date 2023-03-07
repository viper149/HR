using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.HDispatch;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.HDispatch
{
    public class SQLH_SAMPLE_DESPATCH_D_Repository : BaseRepository<H_SAMPLE_DESPATCH_D>, IH_SAMPLE_DESPATCH_D
    {
        public SQLH_SAMPLE_DESPATCH_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<string> GetBarcodeByAsync(int rcvId)
        {
            var barcodeValue = await DenimDbContext.H_SAMPLE_RECEIVING_D
                .Include(e => e.RCV)
                .ThenInclude(e => e.DP)
                .ThenInclude(e => e.F_SAMPLE_DESPATCH_DETAILS)
                .ThenInclude(e => e.TRNS)
                .Select(e => new
                {
                    RCVID = e.RCVDID,
                    BARCODE = e.RCV.DP.F_SAMPLE_DESPATCH_DETAILS.FirstOrDefault().TRNS.BARCODE
                }).FirstOrDefaultAsync(e => e.RCVID.Equals(rcvId));

            return barcodeValue.BARCODE;
        }
    }
}
