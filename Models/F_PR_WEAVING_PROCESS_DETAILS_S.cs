using System;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PROCESS_DETAILS_S
    {
        public int TRNSID { get; set; }
        public int? SWV_BEAMID { get; set; }
        public int? LOOM_NO { get; set; }
        public int? LOOM_TYPE { get; set; }
        public double? LENGTH_SAMPLE { get; set; }
        public string FABCODE { get; set; }
        public double? RS_REF { get; set; }
        public double? RS_ACT { get; set; }
        public double? WIDTH { get; set; }
        public int? RPM { get; set; }
        public double? GSM { get; set; }
        public int? COUNT { get; set; }
        public int? DENT { get; set; }
        public DateTime? DOFF_TIME { get; set; }
        public string SHIFT { get; set; }
        public string DOFFER { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_LOOM_MACHINE_NO LOOM_NONavigation { get; set; }
        public LOOM_TYPE LOOM_TYPENavigation { get; set; }
        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_S SWV_BEAM { get; set; }
    }
}
