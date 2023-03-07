using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Marketing;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLMKT_TEAM_Repository : BaseRepository<MKT_TEAM>, IMKT_TEAM
    {
        public SQLMKT_TEAM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<MKT_TEAM>> FindTeamMembersAsync(int teamid)
        {
            try
            {
                var mktTeams = await DenimDbContext.MKT_TEAM.Where(c => c.TEAMID.Equals(teamid)).ToListAsync();
                return mktTeams;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="mktPersonId"><see cref="MKT_TEAM"/></param>
        /// <returns></returns>
        public async Task<ContainEmailsInformation> GetEmailsByTeamIdMktPersonIdAsync(int teamId, int mktPersonId)
        {
            var containEmailsInformation = new ContainEmailsInformation();
            var listAsync = await DenimDbContext.MKT_TEAM.Where(e => e.TEAMID.Equals(teamId))
                .Select(e => new
                {
                    ToEmailObj = e.PERSON_NAME.ToLower().Contains("dgm") ? e : null,
                    CcEmailObj = e.MKT_TEAMID.Equals(mktPersonId) ? e : null
                }).ToListAsync();

            foreach (var item in listAsync)
            {
                if (item.ToEmailObj != null)
                    containEmailsInformation.ToEmailObj.Add(item.ToEmailObj);

                if (item.CcEmailObj != null)
                    containEmailsInformation.CcEmailObj.Add(item.CcEmailObj);
            }

            return containEmailsInformation;
        }
    }
}