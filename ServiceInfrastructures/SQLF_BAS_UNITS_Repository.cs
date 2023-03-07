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
    public class SQLF_BAS_UNITS_Repository : BaseRepository<F_BAS_UNITS>, IF_BAS_UNITS
    {
        public SQLF_BAS_UNITS_Repository(DenimDbContext denimDbContext) : base(denimDbContext) {}
        public async Task<F_BAS_UNITS> GetSingleDetails(int? unitid)
        {
            try
            {
                var result = await DenimDbContext.F_BAS_UNITS
                    .Where(e => e.UID.Equals(unitid))
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
