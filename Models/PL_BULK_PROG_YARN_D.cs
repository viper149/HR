using System;

namespace DenimERP.Models
{
    public partial class PL_BULK_PROG_YARN_D
    {
        public int YARN_ID { get; set; }
        public int? PROG_ID { get; set; }
        public int? COUNTID { get; set; }
        public int? SCOUNTID { get; set; }
        public int? LOTID { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public RND_FABRIC_COUNTINFO COUNT { get; set; }
        public RND_SAMPLE_INFO_DETAILS SCOUNT { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public PL_BULK_PROG_SETUP_D PROG_ { get; set; }
    }
}
