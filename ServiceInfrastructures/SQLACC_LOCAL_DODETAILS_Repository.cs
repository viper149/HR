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
    public class SQLACC_LOCAL_DODETAILS_Repository : BaseRepository<ACC_LOCAL_DODETAILS>, IACC_LOCAL_DODETAILS
    {
        public SQLACC_LOCAL_DODETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<ACC_LOCAL_DODETAILS>> FindDoDetailsBydoNoAndStyleIdAsync(int doNo,
            int styleId)
        {
            return await DenimDbContext.ACC_LOCAL_DODETAILS.Where(c => c.DONO == doNo && c.STYLEID == styleId).ToListAsync();
        }

        public async Task<IEnumerable<ACC_LOCAL_DODETAILS>> FindDoDetailsListByDoNoAsync(int doNo)
        {
            var result = await DenimDbContext.ACC_LOCAL_DODETAILS
                .Include(c=>c.STYLE)
                .ThenInclude(c=>c.STYLE)
                .Where(c => c.DONO == doNo).ToListAsync();
             
            //foreach (var item in result)
            //{
            //    item.STYLENAME = await _denimDbContext.COM_EX_FABSTYLE.Where(c => c.STYLEID == item.STYLEID).Select(c => c.STYLENAME).FirstOrDefaultAsync();
            //}

            return result;
        }
    }
}
