using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_EX_ADV_DELIVERY_SCH_DETAILS : BaseEntity
    {
        public int TRNSID { get; set; }
        public int? DSID { get; set; }
        [Display(Name = "PI No")]
        public int? PIID { get; set; }
        [Display(Name = "Style Name")]
        public int? STYLE_ID { get; set; }
        [Display(Name = "Previous TTL Qty")]
        public double? PREV_QTY { get; set; }
        [Display(Name = "Qty")]
        public double? QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

        public COM_EX_ADV_DELIVERY_SCH_MASTER DS { get; set; }
        public COM_EX_PIMASTER PI { get; set; }
        public COM_EX_PI_DETAILS STYLE { get; set; }
    }
}
