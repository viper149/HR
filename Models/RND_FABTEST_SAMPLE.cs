using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class RND_FABTEST_SAMPLE : BaseEntity
    {
        public RND_FABTEST_SAMPLE()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
        }
        public int LTSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Lab Test Date")]
        public DateTime LTSDATE { get; set; }
        [Display(Name = "Program/Set No")]
        public int? PROGNO { get; set; }
        [Display(Name = "Style Name")]
        public int? SFINID { get; set; }
        [Display(Name = "Shift Name")]
        public int? SHIFTNAME { get; set; }
        [Display(Name = "Trolley No")]
        public int? TROLLEYNO { get; set; }
        [Display(Name = "Washed By")]
        public int? WASHEDBY { get; set; }
        [Display(Name = "Unwashed By")]
        public int? UNWASHEDBY { get; set; }
        [Display(Name = "Process Route")]
        public string PROCESSROUTE { get; set; }
        [Display(Name = "M/C Name 01")]
        public string MCNAME { get; set; }
        [Display(Name = "Issue")]
        public string ISSUE { get; set; }
        [Display(Name = "Test Method")]
        public int? TESTMETHOD { get; set; }
        [Display(Name = "Actual Unwash EPI")]
        public string FNEPI { get; set; }
        [Display(Name = "Actual Unwash  PPI")]
        public string FNPPI { get; set; }
        [Display(Name = "Unwashed Weight(oz/yd")]
        public string WGFNBW { get; set; }
        [Display(Name = "Wash Weight(oz/yd")]
        public string WGFNAW { get; set; }
        [Display(Name = "Unwashed Width(Inch)")]
        public string WIFNBW { get; set; }
        [Display(Name = "Cuttable Width(Inch)")]
        public string WIFNCUT { get; set; }
        [Display(Name = "Wash Width(Inch)")]
        public string WIFNAW { get; set; }
        [Display(Name = "Shrinkage % (Warp)")]
        public string SRFNWARP { get; set; }
        [Display(Name = "Shrinkage % (Weft)")]
        public string SRFNWEFT { get; set; }
        [Display(Name = "Skew Move %")]
        public string SKEWMOVE { get; set; }
        [Display(Name = "Spirality % (A)")]
        public string SPIRALITY_A { get; set; }
        [Display(Name = "Spirality % (B)")]
        public string SPIRALIRTY_B { get; set; }
        [Display(Name = "Method")]
        public string METHOD { get; set; }
        [Display(Name = "Weight Dead")]
        public string WGDEAD { get; set; }
        [Display(Name = "Stretch % (Warp)")]
        public string STFNWARP { get; set; }
        [Display(Name = "Stretch % (Weft)")]
        public string STFNWEFT { get; set; }
        [Display(Name = "Growth % (Warp)")]
        public string GRFNWARP { get; set; }
        [Display(Name = "Growth % (Weft)")]
        public string GRFNWEFT { get; set; }
        [Display(Name = "Tensile Warp (Kg)")]
        public string TNWARP { get; set; }
        [Display(Name = "Tensile Weft (Kg)")]
        public string TNWEFT { get; set; }
        [Display(Name = "Tear Warp (Kg)")]
        public string TRWARP { get; set; }
        [Display(Name = "Tear Weft (Kg)")]
        public string TRWEFT { get; set; }
        [Display(Name = "Seam Slipage Warp (Kg)")]
        public string SLPWARP { get; set; }
        [Display(Name = "Seam Slipage Weft (Kg)")]
        public string SLPWEFT { get; set; }
        [Display(Name = "Pilling Rubs")]
        public string PILLRUBS { get; set; }
        [Display(Name = "Pilling Grade")]
        public string PILLGRADE { get; set; }
        [Display(Name = "Rubbing Dry")]
        public string CFATDRY { get; set; }
        [Display(Name = "Rubbing Wet")]
        public string CFATNET { get; set; }
        [Display(Name = "Color Change(Washing)")]
        public string SHADECNG { get; set; }
        [Display(Name = "Color Staning(Washing)")]
        public string COLORSTN { get; set; }
        [Display(Name = "Color Change (Perspiration)")]
        public string COLORCNG_ACID { get; set; }
        [Display(Name = "Color Staning (Perspiration)")]
        public string COLORSTN_ACID { get; set; }
        [Display(Name = "Color Change (Water)")]
        public string COLORCNG_ALKA { get; set; }
        [Display(Name = "Color Staning (Water)")]
        public string COLORSTN_ALKA { get; set; }
        [Display(Name = "PH")]
        public double? PH { get; set; }
        [Display(Name = "Formaldehyde")]
        public string FORMALDEHYDE { get; set; }
        [Display(Name = "Fabric Composition")]
        public string FABCOMP { get; set; }
        [Display(Name = "Lab No")]
        [Remote(action: "IsLabNoInUse", controller: "RndFabTestSample")]
        public string LABNO { get; set; }
        [Display(Name = "Loom No")]
        public int? LOOM { get; set; }
        [Display(Name = "Actual Wash EPI")]
        public string WASHEPI { get; set; }
        [Display(Name = "Actual Wash PPI")]
        public string WASHPPI { get; set; }
        [Display(Name = "Recovery % (Warp)")]
        public string RECWARP { get; set; }
        [Display(Name = "Recovery % (Weft)")]
        public string RECWEFT { get; set; }
        [Display(Name = "Remarks")]
        public string COMMENTS { get; set; }
        [Display(Name = "Color Change (Saliva)")]
        public string OPTION5 { get; set; }
        [Display(Name = "Color Staining (Saliva)")]
        public string OPTION4 { get; set; }
        public string OPTION3 { get; set; }
        public string OPTION2 { get; set; }
        public string OPTION1 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        //public RND_FABTEST_SAMPLE LTS { get; set; }
        public F_HRD_EMPLOYEE UNWASHEDBYNavigation { get; set; }
        public F_HRD_EMPLOYEE WASHEDBYNavigation { get; set; }
        public RND_SAMPLEINFO_FINISHING SFIN { get; set; }
        public F_PR_FIN_TROLLY TROLLY { get; set; }
        public F_HR_SHIFT_INFO SHIFTINFO { get; set; }
        public F_BAS_TESTMETHOD TEST { get; set; }
        public F_LOOM_MACHINE_NO LOOMINFO { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION PROGNONavigation { get; set; }

        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
    }
}
