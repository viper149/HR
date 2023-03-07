using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class RND_SAMPLEINFO_FINISHING
    {
        public RND_SAMPLEINFO_FINISHING()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            RND_FABTEST_SAMPLE = new HashSet<RND_FABTEST_SAMPLE>();
            RND_FABTEST_SAMPLE_BULK = new HashSet<RND_FABTEST_SAMPLE_BULK>();
        }
        public int SFINID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Finish Date")]
        [Required]
        public DateTime FINISHDATE { get; set; }
        [Required]
        [Display(Name = "Lab Test No")]
        public int? LTGID { get; set; }
        [Display(Name = "Color")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Approved Style No.")]
        public string STYLE_NAME { get; set; }
        [Display(Name = "Dev. No.")]
        public string DEV_NO { get; set; }
        [Display(Name = "Finish Route")]
        public string FINISH_ROUTE { get; set; }
        [Display(Name = "Processed Length")]
        public double? PROCESSED_LENGTH { get; set; }
        [Display(Name = "Wash Pick")]
        public string WASHPICK { get; set; }
        [Display(Name = "Grey Const")]
        public string GRCONST { get; set; }
        [Display(Name = "Weight Before Wash")]
        public string BWGBW { get; set; }
        [Display(Name = "Weight After Wash")]
        public string BWGAW { get; set; }
        [Display(Name = "Width Before Wash")]
        public string BWIBW { get; set; }
        [Display(Name = "Width After Wash")]
        public string BWIAW { get; set; }
        [Display(Name = "Shrink Warp")]
        public string BSRWARP { get; set; }
        [Display(Name = "Shrink Weft")]
        public string BSRWEFT { get; set; }
        [Display(Name = "Stretch Warp")]
        public string BSTWARP { get; set; }
        [Display(Name = "Stretch Weft")]
        public string BSTWEFT { get; set; }
        [Display(Name = "Finish EPI")]
        public string BFNEPI { get; set; }
        [Display(Name = "Finish PPI")]
        public string BFNPPI { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Finish Const")]
        public string FNCONST { get; set; }
        public string CREATED_BY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? UPDATED_AT { get; set; }
        public string OPTION5 { get; set; }
        public string OPTION4 { get; set; }
        public string OPTION3 { get; set; }
        public string OPTION2 { get; set; }
        public string OPTION1 { get; set; }
        [DisplayName("Is OK?")]
        public bool STATUS { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public RND_FABTEST_GREY LTG { get; set; }
        public BAS_COLOR COLOR { get; set; }
        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }
        public ICollection<RND_FABTEST_SAMPLE_BULK> RND_FABTEST_SAMPLE_BULK { get; set; }
    }
}
