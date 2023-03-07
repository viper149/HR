using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class LOOM_SETTINGS_FILTER_VALUE
    {
        public LOOM_SETTINGS_FILTER_VALUE()
        {
            LOOM_SETTING_STYLE_WISE_M = new HashSet<LOOM_SETTING_STYLE_WISE_M>();
        }

        public int ID { get; set; }
        public string NAME { get; set; }
        public string REMARKS { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public string OPT5 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<LOOM_SETTING_STYLE_WISE_M> LOOM_SETTING_STYLE_WISE_M { get; set; }
    }
}
