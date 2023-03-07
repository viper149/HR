using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class RND_FABTEST_GREY : BaseEntity
    {
        public RND_FABTEST_GREY()
        {
            RND_SAMPLEINFO_FINISHING = new HashSet<RND_SAMPLEINFO_FINISHING>();
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
        }

        public int LTGID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Lab Test Date")]
        public DateTime LTGDATE { get; set; }
        [Display(Name = "Lab Test No")]
        [Remote(action: "IsLabNoInUse", controller: "RndFabTestGrey")]
        public string LAB_NO { get; set; }

        [Display(Name = "Prog./Set No.")]
        public int? PROGID { get; set; }

        [Display(Name = "Prog./Set No.")]
        public int? SETID { get; set; }
        [Display(Name = "Length Test")]
        public double? LENGTHTEST { get; set; }
        [Display(Name = "Washed By")]
        public int? WASHEDBY { get; set; }
        [Display(Name = "Unwashed By")]
        public int? UNWASHEDBY { get; set; }
        [Display(Name = "Shift Name")]
        public int? SHIFT { get; set; }
        [Display(Name = "Batch No")]
        public string BATCHNO { get; set; }
        [Display(Name = "Doff/Roll No")]
        public int? DOFF_ROLL_NO { get; set; }
        [Display(Name = "Development No")]
        public string DEVELOPMENTNO { get; set; }
        [Display(Name = "Order Repeat")]
        public int? ORDER_REPEAT { get; set; }
        [Display(Name = "Grey EPI")]
        public string GREPI { get; set; }
        [Display(Name = "Grey PPI")]
        public string GRPPI { get; set; }
        [Display(Name = "Unwash EPI")]
        public string BWEPI { get; set; }
        [Display(Name = "Unwash PPI")]
        public string BWPPI { get; set; }
        [Display(Name = "Wash EPI")]
        public string AWEPI { get; set; }
        [Display(Name = "Wash PPI")]
        public string AWPPI { get; set; }
        [Display(Name = "Unwash Weight(oz/yd")]
        public string WGGRBW { get; set; }
        [Display(Name = "Wash Weight(oz/yd")]
        public string WGGRAW { get; set; }
        [Display(Name = "Unwash Width (Inch)")]
        public string WIGRBW { get; set; }
        [Display(Name = "Wash Width(Inch) ")]
        public string WIGRAW { get; set; }
        [Display(Name = "Stretch % (Warp)")]
        public double? SGWARP { get; set; }
        [Display(Name = "Stretch % (Weft)")]
        public double? SGWEFT { get; set; }

        [Display(Name = "Growth % (Warp)")]
        public string GRWRAP { get; set; }
        [Display(Name = "Growth % (Weft)")]
        public string GRWEFT { get; set; }

        [Display(Name = "Shrinkage % (Warp)")]
        public string SRGRWRAP { get; set; }
        [Display(Name = "Shrinkage % (Weft)")]
        public string SRGRWEFT { get; set; }
        [Display(Name = "Skew Wash %")]
        public string SKEWWASH { get; set; }
        [Display(Name = "Skew Movement%")]
        public string SKEWMOVE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Style Name")]
        public string OPTION1 { get; set; }
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

        public F_HRD_EMPLOYEE EMP_UNWASHEDBY { get; set; }
        public F_HRD_EMPLOYEE EMP_WASHEDBY { get; set; }
        public F_PR_WEAVING_PROCESS_DETAILS_B DOFF { get; set; }
        public RND_ORDER_REPEAT ORDER_RPT { get; set; }
        public F_HR_SHIFT_INFO SHIFTINFO { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION PROGN { get; set; }
        public PL_SAMPLE_PROG_SETUP PROG { get; set; }

        public ICollection<RND_SAMPLEINFO_FINISHING> RND_SAMPLEINFO_FINISHING { get; set; }
        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
    }
}
