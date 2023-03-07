using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.LoomSetting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLLOOM_SETTING_STYLE_WISE_M_Repository : BaseRepository<LOOM_SETTING_STYLE_WISE_M>, ILOOM_SETTING_STYLE_WISE_M
    {
        private readonly IDataProtector _protector;

        public SQLLOOM_SETTING_STYLE_WISE_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<LOOM_SETTING_STYLE_WISE_M>> GetAllLoomSettingStyleWiseAsync()
        {
            try
            {
                var loomSettingList = await DenimDbContext.LOOM_SETTING_STYLE_WISE_M
                    .Include(c => c.LOOM_TYPENavigation)
                    .Include(c => c.FILTER_VALUENavigation)
                    .Include(c => c.FABCODENavigation)
                    .Select(c => new LOOM_SETTING_STYLE_WISE_M
                    {
                        RPM = c.RPM,
                        REMARKS = c.REMARKS,
                        SETTING_ID = c.SETTING_ID,
                        EncryptedId = _protector.Protect(c.SETTING_ID.ToString()),
                        FABCODENavigation = new RND_FABRICINFO
                        {
                            STYLE_NAME = c.FABCODENavigation.STYLE_NAME
                        },
                        LOOM_TYPENavigation = new LOOM_TYPE
                        {
                            LOOM_TYPE_NAME = c.LOOM_TYPENavigation.LOOM_TYPE_NAME
                        },
                        FILTER_VALUENavigation = new LOOM_SETTINGS_FILTER_VALUE
                        {
                            NAME = c.FILTER_VALUENavigation.NAME
                        }
                    })
                    .ToListAsync();

                return loomSettingList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<LoomSettingStyleWiseViewModel> GetInitObjects(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel, bool edit)
        {
            try
            {
                var filterLoomSettings = new Dictionary<int, string> { { 1, "P1" }, { 2, "P2" }, { 7, "P7" }, { 8, "P8" } };

                if (!edit)
                {
                    loomSettingStyleWiseViewModel.RndFabricInfoList = await DenimDbContext.RND_FABRICINFO
                        .Include(c => c.WV)
                        //.Where(c => !_denimDbContext.LOOM_SETTING_STYLE_WISE_M.Any(e => e.FABCODE.Equals(c.FABCODE)))
                        .Select(c => new RND_FABRICINFO
                        {
                            FABCODE = c.FABCODE,
                            WV = new RND_SAMPLE_INFO_WEAVING
                            {
                                FABCODE = c.WV.FABCODE
                            },
                            STYLE_NAME = c.STYLE_NAME
                        })
                        .ToListAsync();
                }
                else
                {
                    loomSettingStyleWiseViewModel.RndFabricInfoList = await DenimDbContext.RND_FABRICINFO
                        .Include(c => c.WV)
                        .Select(c => new RND_FABRICINFO
                        {
                            FABCODE = c.FABCODE,
                            WV = new RND_SAMPLE_INFO_WEAVING
                            {
                                FABCODE = c.WV.FABCODE
                            },
                            STYLE_NAME = c.STYLE_NAME
                        })
                        .ToListAsync();
                }

                loomSettingStyleWiseViewModel.LoomTypeList = await DenimDbContext.LOOM_TYPE.ToListAsync();

                loomSettingStyleWiseViewModel.LoomSettingsFilterValues = await DenimDbContext.LOOM_SETTINGS_FILTER_VALUE
                    .Where(e => filterLoomSettings.All(f => !f.Key.Equals(e.ID)))
                    .OrderBy(e => e.NAME)
                    .ToListAsync();

                loomSettingStyleWiseViewModel.RndFabricCountInfos = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(c => c.COUNT)
                    .Include(c => c.SUPP)
                    .Include(c => c.LOT)
                    .Where(c => c.YARNFOR.Equals("2"))
                    .ToListAsync();
                loomSettingStyleWiseViewModel.BasYarnLotInfos = await DenimDbContext.BAS_YARN_LOTINFO.ToListAsync();
                loomSettingStyleWiseViewModel.SupplierInfos = await DenimDbContext.BAS_SUPPLIERINFO.ToListAsync();

                return loomSettingStyleWiseViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RND_FABRICINFO> GetStyleDetails(int fabcode)
        {
            try
            {
                var fabDetails = await DenimDbContext.RND_FABRICINFO
                    .Include(c => c.WV)
                    .Include(c => c.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.RND_WEAVE)
                    .Where(c => c.FABCODE.Equals(fabcode))
                    .FirstOrDefaultAsync();




                fabDetails.REMARKS = string.Join(" + ",
                    fabDetails.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1))
                        .Select(p => p.COUNT.RND_COUNTNAME));
                fabDetails.COMPOSITION_PRO = string.Join(" + ",
                    fabDetails.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                        .Select(p => p.COUNT.RND_COUNTNAME));
                fabDetails.REMARKS =
                    $"{fabDetails.REMARKS} X {fabDetails.REMARKS} / {fabDetails.FNEPI}X{fabDetails.FNPPI}";



                return fabDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RND_FABRIC_COUNTINFO> GetCountDetails(int countId)
        {
            try
            {
                var fabDetails = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(c => c.SUPP)
                    .Include(c => c.LOT)
                    .Where(c => c.COUNTID.Equals(countId))
                    .FirstOrDefaultAsync();

                return fabDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}
