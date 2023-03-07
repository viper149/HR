using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_ISSUE_DETAILS : BaseEntity
    {
        public F_CHEM_ISSUE_DETAILS()
        {
            F_CHEM_TRANSECTION = new HashSet<F_CHEM_TRANSECTION>();
        }

        public int CISSDID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Issue Date")]
        public DateTime? CISSDDATE { get; set; }
        [Display(Name = "Batch Number")]
        //[Required(ErrorMessage = "The filed {0} can not be empty.")]
        public int? CRCVIDD { get; set; }
        public int? CISSUEID { get; set; }
        [Display(Name = "Requirement No")]
        public int? CREQ_DET_ID { get; set; }
        [Display(Name = "Chemical Name")]
        //[Required(ErrorMessage = "The filed {0} can not be empty.")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Adjust With")]
        public int? ADJ_PRO_AGNST { get; set; }
        [Display(Name = "Issue Quantity")]
        //[Range(1, double.MaxValue, ErrorMessage = "The filed {0} can not be less than {1}")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OTP1 { get; set; }
        public string OTP2 { get; set; }

        [NotMapped]
        [Display(Name = "Remaining Balance")]
        public double? REMAINING_AMOUNT { get; set; }

        public F_CHEM_ISSUE_MASTER CISSUE { get; set; }
        public F_CHEM_REQ_DETAILS CREQ_DET_ { get; set; }
        public F_CHEM_STORE_PRODUCTINFO PRODUCT { get; set; }
        public F_CHEM_STORE_PRODUCTINFO PRODUCTACCLIMATIZE { get; set; }
        public F_CHEM_STORE_RECEIVE_DETAILS CRCVIDDNavigation { get; set; }
        public ICollection<F_CHEM_TRANSECTION> F_CHEM_TRANSECTION { get; set; }
    }
}
