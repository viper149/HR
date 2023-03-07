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
    public class SQLBAS_COLOR_Repository : BaseRepository<BAS_COLOR>, IBAS_COLOR
    {
        public SQLBAS_COLOR_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {

        }

        public async Task<IEnumerable<BAS_COLOR>> GetColorsWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var excludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_COLOR
                     .OrderByDescending(c => c.COLORCODE)
                    .Skip(excludeResult)
                    .Take(pageSize)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<BAS_COLOR>> GetForSelectItemsByAsync()
        {
            return await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR
            {
                COLORCODE = e.COLORCODE,
                COLOR = e.COLOR
            }).ToListAsync();
        }
    }
}
