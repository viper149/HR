using System;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS : BaseEntity
    {
        public int TRNSID { get; set; }
        public DateTime? TRNSDATE { get; set; }
        public int? WV_PROCESSID { get; set; }
        public int? COUNTID { get; set; }
        public int? LOTID { get; set; }
        public int? SUPPID { get; set; }
        public string RATIO { get; set; }
        public string SET_BGT { get; set; }
        public string ACT_BGT { get; set; }
        public string CONSUMP { get; set; }
        public double? YARN_RECEIVE { get; set; }
        public double? YARN_RETURN { get; set; }
        public string WASTE { get; set; }
        public double? WASTE_PERCENTAGE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_PR_WEAVING_PROCESS_MASTER_B WEAVING { get; set; }
        public RND_FABRIC_COUNTINFO COUNT { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
    }
}
