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
    public class SQLF_CHEM_STORE_INDENTDETAILS_Repository : BaseRepository<F_CHEM_STORE_INDENTDETAILS>, IF_CHEM_STORE_INDENTDETAILS
    {
        public SQLF_CHEM_STORE_INDENTDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public Task<F_CHEM_STORE_INDENTDETAILS> FindChemIndentListByIdAsync(int id, int prdId)
        {
            try
            {
                var result = DenimDbContext.F_CHEM_STORE_INDENTDETAILS
                    .Include(e => e.FBasUnits)
                    .Include(e => e.PRODUCT)
                    .Where(e => e.INDSLID.Equals(id) && e.PRODUCTID.Equals(prdId))
                    .FirstOrDefaultAsync();

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
