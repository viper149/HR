using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.LoomSetting
{
    public class LoomSettingStyleWiseViewModel
    {

        public LoomSettingStyleWiseViewModel()
        {
            LoomSettingChannelInfoList = new List<LOOM_SETTING_CHANNEL_INFO>();
        }

        public LOOM_SETTING_STYLE_WISE_M LoomSettingStyleWiseM { get; set; }
        public LOOM_SETTING_CHANNEL_INFO LoomSettingChannelInfo { get; set; }


        public List<LOOM_SETTING_CHANNEL_INFO> LoomSettingChannelInfoList { get; set; }
        public List<RND_FABRICINFO> RndFabricInfoList { get; set; }
        public List<LOOM_TYPE> LoomTypeList { get; set; }
        public List<LOOM_SETTINGS_FILTER_VALUE> LoomSettingsFilterValues { get; set; }
        public List<RND_FABRIC_COUNTINFO> RndFabricCountInfos { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotInfos { get; set; }
        public List<BAS_SUPPLIERINFO> SupplierInfos { get; set; }
    }
}
