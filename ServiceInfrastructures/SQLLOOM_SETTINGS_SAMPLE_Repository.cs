using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using static System.String;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLLOOM_SETTINGS_SAMPLE_Repository : BaseRepository<LOOM_SETTINGS_SAMPLE>, ILOOM_SETTINGS_SAMPLE
    {
        private readonly IDataProtector _protector;

        public SQLLOOM_SETTINGS_SAMPLE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<LOOM_SETTINGS_SAMPLE>> GetAllLoomSettingsSampleAsync()
        {
            return await DenimDbContext.LOOM_SETTINGS_SAMPLE
                .Include(d => d.DEV.PROG_)
                .Select(d => new LOOM_SETTINGS_SAMPLE
                {
                    SETTINGS_ID = d.SETTINGS_ID,
                    EncryptedId = _protector.Protect(d.SETTINGS_ID.ToString()),
                    REMARKS = d.REMARKS,
                    DEV = new RND_SAMPLE_INFO_WEAVING
                    {
                        FABCODE = d.DEV.FABCODE,
                        PROG_ = new PL_SAMPLE_PROG_SETUP
                        {
                            PROG_NO = d.DEV.PROG_.PROG_NO
                        }
                    }
                }).ToListAsync();
        }

        public async Task<LoomSettingsSampleViewModel> GetInitObjByAsync(LoomSettingsSampleViewModel loomSettingsSampleViewModel)
        {
            loomSettingsSampleViewModel.RndSampleInfoWeavingsList = await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                .Select(d => new RND_SAMPLE_INFO_WEAVING
                {
                    WVID = d.WVID,
                    FABCODE = d.FABCODE
                }).ToListAsync();

            return loomSettingsSampleViewModel;
        }

        public async Task<dynamic> GetAllByDevIdAsync(int devId)
        {
            try
            {
                var result = await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                    .Include(c => c.PROG_.SD.COLOR)
                    .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c=>c.COUNT)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(c => c.LOT)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(c => c.SUPP)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(c => c.COLORCODENavigation)
                    .Include(c => c.LOOM)
                    .Include(c => c.WEAVENavigation)
                    .Where(c=>c.WVID.Equals(devId))
                    .Select(d => new
                    {
                        setNo = d.PROG_.PROG_NO,
                        color = d.PROG_.SD.COLOR.COLOR,
                        warpCount = Join(" + ", d.PROG_.SD.RND_SAMPLE_INFO_DETAILS.Where(c => c.YARNID.Equals(1)).Select(p => p.COUNT.RND_COUNTNAME)),
                        weftCount = Join(" + ", d.RND_SAMPLE_INFO_WEAVING_DETAILS.Where(c => c.YARNID.Equals(2)).Select(p => p.COUNT.RND_COUNTNAME)),
                        lot = Join(" + ", d.RND_SAMPLE_INFO_WEAVING_DETAILS.Where(c => c.YARNID.Equals(2)).Select(p => p.LOT.LOTNO)),
                        ratio = Join(" : ", d.RND_SAMPLE_INFO_WEAVING_DETAILS.Where(c => c.YARNID.Equals(2)).Select(p => p.RATIO)),
                        supplier = Join(" + ", d.RND_SAMPLE_INFO_WEAVING_DETAILS.Where(c => c.YARNID.Equals(2)).Select(p => p.SUPP.SUPPNAME)),
                        loomType = d.LOOM.LOOM_TYPE_NAME,
                        grepi = d.GR_EPI,
                        grppi = d.GR_PPI,
                        weave = d.WEAVENavigation.NAME,
                        totalEnds = d.TOTAL_ENDS,
                        rs = d.REED_SPACE,
                        reed = $"{d.REED_COUNT} / {d.REED_DENT}"
                    })
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
