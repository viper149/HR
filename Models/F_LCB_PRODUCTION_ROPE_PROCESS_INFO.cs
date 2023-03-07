using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_LCB_PRODUCTION_ROPE_PROCESS_INFO
    {
        public int LCB_P_ID { get; set; }
        public int? LCB_D_ID { get; set; }
        [Display(Name = "Machine No.")]
        public int? MACHINEID { get; set; }
        [Display(Name = "Beam No.")]
        public int? BEAMID { get; set; }
        [Display(Name = "Length")]
        public double? LENGTH { get; set; }
        [Display(Name = "Tens.")]
        public int? TENS { get; set; }
        [Display(Name = "Break")]
        public int? BREAK { get; set; }
        [Display(Name = "Knot")]
        public int? KNOT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_LCB_BEAM BEAM { get; set; }
        public F_LCB_PRODUCTION_ROPE_DETAILS LCB_D_ { get; set; }
        public F_LCB_MACHINE MACHINE { get; set; }
    }
}
