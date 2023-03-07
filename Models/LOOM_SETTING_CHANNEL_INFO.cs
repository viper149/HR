using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class LOOM_SETTING_CHANNEL_INFO
    {
        public int CHANNEL_ID { get; set; }
        [Display(Name = "Setting Name")]
        public int? SETTING_ID { get; set; }
        [Display(Name = "Channel No")]
        public string CHANNEL_NO { get; set; }
        [Display(Name = "Mode")]
        public string MODE { get; set; }
        [Display(Name = "Brake force")]
        public string BREAK_FORCE { get; set; }
        [Display(Name = "Break Time")]
        public string BREAK_TIME { get; set; }
        [Display(Name = "Start % (Fixed)")]
        public string FIXED_ELCA_START { get; set; }
        [Display(Name = "Stop % (Fixed)")]
        public string FIXED_ELCA_STOP { get; set; }
        [Display(Name = "Start % (Movable)")]
        public string MOV_ELCA_START { get; set; }
        [Display(Name = "Stop % (Movable)")]
        public string MOV_ELCA_STOP { get; set; }
        [Display(Name = "Ps(degree)")]
        public string PS { get; set; }
        [Display(Name = "Paexp(degree)")]
        public string PAEXP { get; set; }
        [Display(Name = "Main Valve(degree)")]
        public string MAIN_VALVE { get; set; }
        [Display(Name = "Relay Valve(degree)")]
        public string RELAY_VALVE { get; set; }
        [Display(Name = "Weft Cutter angle(degree)")]
        public string WEFT_CUTTER_ANGLE { get; set; }
        [Display(Name = "Main Nozzle(bar)")]
        public string MAIN_NOZZLE { get; set; }
        [Display(Name = "Relay Nozzle Middle(bar)")]
        public string RELAY_NOZZLE_M { get; set; }
        [Display(Name = "Relay Nozzle Left & Right(bar)")]
        public string RELAY_NOZZLE_LR { get; set; }
        [Display(Name = "Supplier")]
        public int? SUPPLIER { get; set; }
        [Display(Name = "Lot")]
        public int? LOT { get; set; }
        [Display(Name = "Count")]
        public int? COUNT { get; set; }
        [Display(Name = "Ratio")]
        public string RATIO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; } public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public string OPT5 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public RND_FABRIC_COUNTINFO COUNTNavigation { get; set; }
        public BAS_YARN_LOTINFO LOTNavigation { get; set; }
        public LOOM_SETTING_STYLE_WISE_M SETTING_ { get; set; }
        public BAS_SUPPLIERINFO SUPPLIERNavigation { get; set; }

    }
}
