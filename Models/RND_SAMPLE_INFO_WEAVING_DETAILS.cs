﻿using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class RND_SAMPLE_INFO_WEAVING_DETAILS
    {
        public int WVID { get; set; }
        public int? WVID_PARENT { get; set; }
        public int? SDID { get; set; }
        public int? COUNTID { get; set; }
        [Display(Name = "Type")]
        public string TYPE { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        public int? COLORCODE { get; set; }
        public int? LOTID { get; set; }
        public int? SUPPID { get; set; }
        [Display(Name = "Ratio")]
        public double? RATIO { get; set; }
        public double? NE { get; set; }
        [Display(Name = "Budget")]
        public double? BGT { get; set; }
        [Display(Name = "Yarn Type")]
        public string YARNTYPE { get; set; }
        public int? YARNID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public BAS_COLOR COLORCODENavigation { get; set; }
        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public YARNFOR YARN { get; set; }
        public RND_SAMPLE_INFO_WEAVING WEAVING { get; set; }
    }
}
