using System;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_REPORT : BaseEntity
    {
        public int MRRID { get; set; }
        [Display(Name = "MRR No.")]
        public int? MRRNO { get; set; }
        [Display(Name = "Yarn Receive No.")]
        public int? YRDID { get; set; }
        [Display(Name = "MRR Date")]
        public DateTime? MRRDATE { get; set; }
        [Display(Name = "MRR Quantity")]
        public double? MRR_QTY { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_YS_YARN_RECEIVE_DETAILS YRD { get; set; }
    }
}
