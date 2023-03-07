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
    public class SQLRND_MSTR_ROLL_Repository: BaseRepository<RND_MSTR_ROLL>, IRND_MSTR_ROLL
    {
        public SQLRND_MSTR_ROLL_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<RND_MSTR_ROLL> FindByIdAllAsync(int mid)
        {
            try
            {
                var rollDetails = await DenimDbContext.RND_MSTR_ROLL
                    .Include(c => c.SUPP)
                    .Include(c => c.LOT)
                    .Where(c => c.MID.Equals(mid))
                    .FirstOrDefaultAsync();
                return rollDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
