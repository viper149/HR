using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_RAW_PER_Repository:BaseRepository<F_YS_RAW_PER>, IF_YS_RAW_PER
    {
        public SQLF_YS_RAW_PER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<List<F_YS_RAW_PER>> GetAllAsync()
        {

            try
            {
                var result = await DenimDbContext.F_YS_RAW_PER
                    .ToListAsync();
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
