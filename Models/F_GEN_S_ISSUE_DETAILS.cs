using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_ISSUE_DETAILS : BaseEntity
    {

        public int GISSDID { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GISSDDATE { get; set; }
        public int? GRCVIDD { get; set; }
        public int? GISSUEID { get; set; }
        [Display(Name = "Requirement No")]
        public int? GREQ_DET_ID { get; set; }
        [Display(Name = "Product")]
        public int? PRODUCTID { get; set; }
        public int? ADJ_PRO_AGNST { get; set; }
        [Display(Name = "Issue Quantity")]
        [Range(1, double.MaxValue, ErrorMessage = "The filed {0} can not be less than {1}")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OTP1 { get; set; }
        public string OTP2 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_GS_PRODUCT_INFORMATION ADJ_PRO_AGNSTNavigation { get; set; }
        public F_GS_PRODUCT_INFORMATION PRODUCT { get; set; }
        public F_GEN_S_RECEIVE_DETAILS GRCVIDDNavigation { get; set; }
        public F_GEN_S_REQ_DETAILS GREQ_DET_ { get; set; }
        public F_GEN_S_ISSUE_MASTER GISSUE { get; set; }
    }
}
