using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_BAS_DESIGNATION_Reporsitory : BaseRepository<F_BAS_DESIGNATION>, IF_BAS_DESIGNATION
    {
        public SQLF_BAS_DESIGNATION_Reporsitory(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<bool> FindByDesignationNameAsync(string designationName)
        {
            return await DenimDbContext.F_BAS_DESIGNATION.AnyAsync(e => e.DESNAME.Equals(designationName));
        }
    }
}
