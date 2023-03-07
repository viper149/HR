using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_WASTAGE_TRANSFER : BaseEntity
    {
        public int TRANSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRANSDATE { get; set; }
        [Display(Name = "Cut Pices Yds ")]
        public double? CUT_PIECE_Y { get; set; }
        [Display(Name = "Cut Pices Roll ")]
        public int? CUT_PIECE_R { get; set; }
        [Display(Name = "Cut Pices Kg ")]
        public double? CUT_PIECE_KG { get; set; }
        [Display(Name = "Jute Moni Kg ")]
        public double? JUTE_MONI { get; set; }
        [Display(Name = "Paper Tube Kg ")]
        public double? PAPER_TUBE { get; set; }
        [Display(Name = "Poly Kg ")]
        public double? POLY { get; set; }
        [Display(Name = "Cutton Kg ")]
        public double? CUTTON { get; set; }
        [Display(Name = "Lead Line Yds ")]
        public double? LEAD_LINE_Y { get; set; }
        [Display(Name = " Lead Line Kg")]
        public double? LEAD_LINE_KG { get; set; }
        [Display(Name = "Clearance Swach Kg")]
        public double? CLEARANCE_SWACH { get; set; }
        [Display(Name = "Clearance Headers Kg")]
        public double? CLEARANCE_HEADERS { get; set; }
        [Display(Name = "Remarks ")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }
        [NotMapped]
        public string EncryptedId { get; set; }
    }
}
