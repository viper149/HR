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
    public class SQLBAS_TEAMINFO_Repository : BaseRepository<BAS_TEAMINFO>, IBAS_TEAMINFO
    {
        public SQLBAS_TEAMINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }


        public bool FindByTeamName(string teamName)
        {
            try
            {
                var teamNames = DenimDbContext.BAS_TEAMINFO.Where(sc => sc.TEAM_NAME.Equals(teamName)).Select(e => e.TEAM_NAME);
                return teamName.Any();
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<BAS_TEAMINFO>> GetBasTeamsAllAsync()
        {
            try
            {
                var result = await DenimDbContext.BAS_TEAMINFO
                    .Include(t=>t.DEPT)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

    }
}
