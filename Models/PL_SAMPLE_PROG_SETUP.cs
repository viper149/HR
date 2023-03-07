using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class PL_SAMPLE_PROG_SETUP : BaseEntity
    {
        public PL_SAMPLE_PROG_SETUP()
        {
            RND_FABTEST_GREY = new HashSet<RND_FABTEST_GREY>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
        }

        public int TRNSID { get; set; }
        public DateTime? TRNSDATE { get; set; }
        public int? SDID { get; set; }
        [Display(Name = "Style Name")]
        public string STYLE_NAME { get; set; }
        [Display(Name = "Program/Set No.")]
        public string PROG_NO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Program Set Date")]
        public DateTime? PROG_DATE { get; set; }
        [Display(Name = "Set Qty[Mtr]")]
        public double? QTY_MTR { get; set; }
        [Display(Name = "Program Type")]
        public string PROGRAM_TYPE { get; set; }
        [Display(Name = "Process Type")]
        public string PROCESS_TYPE { get; set; }
        [Display(Name = "Warp Type")]
        public string WARP_TYPE { get; set; }
        [Display(Name = "Type")]
        public string TYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }

        public RND_SAMPLE_INFO_DYEING SD { get; set; }
        public ICollection<RND_FABTEST_GREY> RND_FABTEST_GREY { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
    }
}
