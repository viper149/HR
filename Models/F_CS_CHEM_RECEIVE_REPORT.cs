using System;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CS_CHEM_RECEIVE_REPORT : BaseEntity
    {
        public int CMRRID { get; set; }
        [Display(Name = "MRR No")]
        public int? CMRRNO { get; set; }
        public int? CRDID { get; set; }
        [Display(Name = "MRR Date")]
        public DateTime? CMRDATE { get; set; }
        [Display(Name = "MRR Quantity")]
        public double? MRR_QTY { get; set; }
        public string OTP1 { get; set; }
        public string OTP2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_CHEM_STORE_RECEIVE_DETAILS CMRR { get; set; }
    }
}
