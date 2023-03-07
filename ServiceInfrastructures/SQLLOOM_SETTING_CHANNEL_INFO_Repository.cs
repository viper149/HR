using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.LoomSetting;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLLOOM_SETTING_CHANNEL_INFO_Repository:BaseRepository<LOOM_SETTING_CHANNEL_INFO>, ILOOM_SETTING_CHANNEL_INFO
    {
        public SQLLOOM_SETTING_CHANNEL_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<LoomSettingStyleWiseViewModel> GetInitChannelData(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel)
        {
            try
            {
                foreach (var item in loomSettingStyleWiseViewModel.LoomSettingChannelInfoList)
                {
                    item.COUNTNavigation = await DenimDbContext.RND_FABRIC_COUNTINFO
                            .Include(c=>c.COUNT)
                            .Where(c => c.TRNSID.Equals(item.COUNT)).FirstOrDefaultAsync();
                    item.LOTNavigation = await DenimDbContext.BAS_YARN_LOTINFO.Where(c => c.LOTID.Equals(item.LOT))
                        .FirstOrDefaultAsync();
                    item.SUPPLIERNavigation = await DenimDbContext.BAS_SUPPLIERINFO.Where(c => c.SUPPID.Equals(item.SUPPLIER))
                        .FirstOrDefaultAsync();
                }

                return loomSettingStyleWiseViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
