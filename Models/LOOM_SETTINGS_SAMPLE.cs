using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class LOOM_SETTINGS_SAMPLE : BaseEntity
    {
        public int SETTINGS_ID { get; set; }
        [Display(Name = "Development No.")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? DEV_ID { get; set; }
        [Display (Name = "Warp Crimp %" )]
        public double? WARP_CRIMP { get; set; }
        [Display(Name = "Weft Crimp %")]
        public double? WEFT_CRIMP { get; set; }
        [Display(Name = "Fabric Length")]
        public double? FABRIC_LENGTH { get; set; }
        [Display(Name = "Loom RPM")]
        public double? RPM { get; set; }
        [Display(Name = "Selvedge Weave")]
        public double? SEL_WEAVE { get; set; }
        [Display(Name = "Selvedge Width")]
        public double? SEL_WIDTH { get; set; }
        [Display(Name = "Loom Efficiency")]
        public double? EFFICIENCE { get; set; }
        [Display(Name = "Warp")]
        public double? BR_WARP { get; set; }
        [Display(Name = "Weft")]
        public double? BR_WEFT { get; set; }
        [Display(Name = "Catch Cord")]
        public double? BR_CATCH_CORD { get; set; }
        [Display(Name = "Others Breaks")]
        public double? BR_OTHER_BREAK { get; set; }
        [Display(Name = "Backrest Height(cm)")]
        public double? BACK_HEIGHT { get; set; }
        [Display(Name = "Backrest Depth(cm)")]
        public double? BACK_DEPTH { get; set; }
        [Display(Name = "D-Box Height(cm)")]
        public double? D_HEIGHT { get; set; }
        [Display(Name = "D-Box Depth(cm)")]
        public double? D_DEPTH { get; set; }
        [Display(Name = "Warp Tension")]
        public double? WARP_TENSN { get; set; }
        [Display(Name = "Frame Height(mm)")]
        public double? FR_HEIGHT { get; set; }
        [Display(Name = "Shade Angle(°)")]
        public double? SHADE_ANGLE { get; set; }
        [Display(Name = "Shade Crossing(°)")]
        public double? SHADE_CROSSING { get; set; }
        [Display(Name = "Comments")]
        public string REMARKS { get; set; }
        [Display(Name = "Shift")]
        public string OPT1 { get; set; }
        [Display(Name = "Suggested TE")]
        public string OPT2 { get; set; }
        [Display(Name = "Suggested Reed")]
        public string OPT3 { get; set; }
        [Display(Name = "Suggested Reed Space")]
        public string OPT4 { get; set; }
        [Display(Name = "Greige Construction")]
        public string GRCONST { get; set; }
        [Display(Name = "Weft Count")]
        public string WEFT_COUNT { get; set; }
        [Display(Name = "Weft Ratio")]
        public string WEFT_RATIO { get; set; }
        [Display(Name = "Weft Lot")]
        public string WEFT_LOT { get; set; }
        [Display(Name = "Weft Supply")]
        public string WEFT_SUPP { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public RND_SAMPLE_INFO_WEAVING DEV { get; set; }
    }
}
