using System;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_REPORT_S : BaseEntity
    {
        public int MRRID { get; set; }
        public int? MRRNO { get; set; }
        public int? YRDID { get; set; }
        public DateTime? MRRDATE { get; set; }
        public double? MRR_QTY { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string REMARKS { get; set; }

        public F_YS_YARN_RECEIVE_DETAILS_S YRD { get; set; }
    }
}
