using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
   
    public partial class RND_FABTEST_BULK:BaseEntity
    {
        public RND_FABTEST_BULK()
        {
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
        }
        public int LTBID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Lab Test No")]
        [Remote(action: "IsLabNoInUse", controller: "RndFabTestBulk")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public string LAB_NO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Lab Test Date")]
        public DateTime? LTB_DATE { get; set; }
        [Display(Name = "Set No")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? PROG_NO { get; set; }
        [Display(Name = "Trolley No.")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? TROLLEY_NO { get; set; }
        [Display(Name = "Length Test")]
        public double? LENGTH_TEST { get; set; }
        [Display(Name = "Washed By")]
        public int? WASHED_BY { get; set; }
        [Display(Name = "Unwashed By")]
        public int? UNWASHED_BY { get; set; }
        [Display(Name = "Shift Name")]
        public int? SHIFT { get; set; }
        [Display(Name = "Stiffness (Kg)")]
        public string STIFFNESS { get; set; }
        [Display(Name = "Bowing %")]
        public string BOWING { get; set; }
        [Display(Name = "Issue")]
        public string ISSUE { get; set; }
        [Display(Name = "Test Method")]
        public int? TEST_METHOD { get; set; }
        [Display(Name = "Overall Width(Inch)")]
        public string WIDTH_OVR { get; set; }
        [Display(Name = "Cuttable Width(Inch) ")]
        public string WIDTH_CUT { get; set; }
        [Display(Name = "Wash Width(Inch)")]
        public string WIDTH_WASH { get; set; }
        [Display(Name = "Unwash Weight(oz/yd")]
        public string WEIGHT_UW { get; set; }
        [Display(Name = "Wash Weight(oz/yd")]
        public string WEIGHT_WASH { get; set; }
        [Display(Name = "Actual Unwash EPI")]
        public string ACUW_EPI { get; set; }
        [Display(Name = "Actual Unwash PPI")]
        public string ACUW_PPI { get; set; }
        [Display(Name = "Actual Wash EPI")]
        public string ACWASH_EPI { get; set; }
        [Display(Name = "Actual Wash PPI")]
        public string ACWASH_PPI { get; set; }
        [Display(Name = "Shrinkage % (Warp)")]
        public string SHRINK_WARP { get; set; }
        [Display(Name = "Shrinkage % (Weft)")]
        public string SHRINK_WEFT { get; set; }
        [Display(Name = "Skew Unwash %")]
        public string SKEW_UW { get; set; }
        [Display(Name = "Skew Wash %")]
        public string SKEW_WASH { get; set; }
        [Display(Name = "Skew Move %")]
        public string SKEW_MOVE { get; set; }
        [Display(Name = "Spirality % (A)")]
        public string SPIR_A { get; set; }
        [Display(Name = "Spirality % (B)")]
        public string SPIR_B { get; set; }
        [Display(Name = "Weight Dead")]
        public string WEIGHT_DEAD { get; set; }
        [Display(Name = "Stretch % (Weft)")]
        public string STRWEFT_QA { get; set; }
        [Display(Name = "Stretch % (Warp)")]
        public string STRWARP_QA { get; set; }
        [Display(Name = "Growth % (Weft)")]
        public string GROWTH_WEFT { get; set; }
        [Display(Name = "Recovery % (Weft)")]
        public string REC_WEFT { get; set; }
        [Display(Name = "Growth % (Warp)")]
        public string GROWTH_WARP { get; set; }
        [Display(Name = "Recovery % (Warp)")]
        public string REC_WARP { get; set; }
        [Display(Name = "Tensile Warp (Kg)")]
        public string TENSILE_WARPT { get; set; }
        [Display(Name = "Tensile Weft (Kg)")]
        public string TENSILE_WEFTT { get; set; }
        [Display(Name = "Tear Warp (Kg)")]
        public string TEAR_WARP { get; set; }
        [Display(Name = "Tear Weft (Kg)")]
        public string TEAR_WEFT { get; set; }
        [Display(Name = "Seam Slippage Warp (Kg)")]
        public string SLIP_WARP { get; set; }
        [Display(Name = "Seam Slippage Weft (Kg)")]
        public string SLIP_WEFT { get; set; }
        [Display(Name = "Fabric Composition")]
        public string FAB_COMP { get; set; }
        [Display(Name = "Rubbing Dry")]
        public string CLR_DRY { get; set; }
        [Display(Name = "Rubbing Wet")]
        public string CLR_WET { get; set; }
        [Display(Name = "Color Change (Washing)")]
        public string CLR_CHANGE { get; set; }
        [Display(Name = "Color Staining (Washing)")]
        public string CLR_STAINING { get; set; }
        [Display(Name = "Color Change (Perspiration)")]
        public string CCA { get; set; }
        [Display(Name = "Color Staining (Perspiration)")]
        public string CSA { get; set; }
        [Display(Name = "Color Change (Water)")]
        public string CCALK { get; set; }
        [Display(Name = "Color Staining (Water)")]
        public string CSALK { get; set; }
        public double? PH { get; set; }
        [Display(Name = "Formaldehyde")]
        public string FORMALD { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Color Change (Saliva)")]
        public string OPTION1 { get; set; }
        [Display(Name = "Color Staining (Saliva)")]
        public string OPTION2 { get; set; }
        public string OPTION3 { get; set; }
        public string OPTION4 { get; set; }
        public string OPTION5 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public PL_PRODUCTION_SETDISTRIBUTION PROG { get; set; }
        public F_PR_FINISHING_FNPROCESS FINPROC { get; set; }
        public F_HRD_EMPLOYEE EMP_UNWASHEDBY { get; set; }
        public F_HRD_EMPLOYEE EMP_WASHEDBY { get; set; }
        public F_HR_SHIFT_INFO SHIFTINFO { get; set; }
        public F_BAS_TESTMETHOD TEST { get; set; }
        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
    }
}
