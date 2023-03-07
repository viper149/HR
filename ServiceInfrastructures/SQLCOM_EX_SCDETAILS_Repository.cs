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
    public class SQLCOM_EX_SCDETAILS_Repository : BaseRepository<COM_EX_SCDETAILS>, ICOM_EX_SCDETAILS
    {
        public SQLCOM_EX_SCDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<COM_EX_SCDETAILS>> FindScDetailsByScNoAndStyleIdAsync(int? scNo, int styleId)
        {
            var comExScdetails = await DenimDbContext.COM_EX_SCDETAILS.Where(c => c.SCNO == scNo && c.STYLEID == styleId).ToListAsync();

            return comExScdetails;
        }
        public async Task<IEnumerable<COM_EX_SCDETAILS>> GetComExScDetailsList(int? scNo)
        {
            var comExScdetails = await DenimDbContext.COM_EX_SCDETAILS.Where(c => c.SCNO == scNo).ToListAsync();
            return comExScdetails;
        }
    }
}
