using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class LoomSettingsSampleViewModel
    {
        public LoomSettingsSampleViewModel()
        {
            RndSampleInfoWeavingsList = new List<RND_SAMPLE_INFO_WEAVING>();
        }

        public LOOM_SETTINGS_SAMPLE LoomSettingsSample { get; set; }

        public List<RND_SAMPLE_INFO_WEAVING> RndSampleInfoWeavingsList { get; set; }
    }
}
