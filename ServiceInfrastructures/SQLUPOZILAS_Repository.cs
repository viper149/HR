using System;
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
    public class SQLUPOZILAS_Repository:BaseRepository<UPOZILAS>,IUPOZILAS
    {
        public SQLUPOZILAS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<UPOZILAS>> GetThanaByDistrictIdAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.UPOZILAS.Where(c => c.DISTRICT_ID == id).ToListAsync();
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
