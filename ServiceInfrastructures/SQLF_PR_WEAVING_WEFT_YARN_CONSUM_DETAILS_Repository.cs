using System;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_Repository:BaseRepository<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>, IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS
    {
        public SQLF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> FindByIdAndCountAsync(int rcvId, int? countId)
        {
            try
            {
                //var isExists = await _denimDbContext.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS.FirstOrDefaultAsync(c => c.RCVID.Equals(rcvId) && c.COUNTID.Equals(countId));
                //return isExists;
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
