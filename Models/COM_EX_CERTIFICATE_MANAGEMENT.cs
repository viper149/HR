using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_EX_CERTIFICATE_MANAGEMENT : BaseEntity
    {
        public int CMID { get; set; }
        public DateTime? TRNSDATE { get; set; }
        public int? INVID { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? BCI_APP_DATE { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? BCI_ISSUE_DATE { get; set; }

        [Display(Name = "Ref")]
        public string BCI_REF { get; set; }

        [Display(Name = "BCI Tracer ID")]
        public int? BCI_TRACER_ID { get; set; }

        [Display(Name = "Remarks")]
        public string BCI_REMARKS { get; set; }

        [Display(Name = "Type")]
        public string ORGANIC_TYPE { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ORGANIC_APP_DATE { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ORGANIC_ISSUE_DATE { get; set; }

        [Display(Name = "TC Ref No")]
        public string ORGANIC_REF { get; set; }

        [Display(Name = "Remarks")]
        public string ORGANIC_REMARKS { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CMIA_APP_DATE { get; set; }

        [Display(Name = "Rep.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CMIA_REP_DATE { get; set; }

        [Display(Name = "Retailer")]
        public string CMIA_RETAILER { get; set; }

        [Display(Name = "Remarks")]
        public string CMIA_REMARKS { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GRS_APP_DATE { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GRS_ISSUE_DATE { get; set; }

        [Display(Name = "TC Ref No")]
        public string GRS_REF { get; set; }

        [Display(Name = "Remarks")]
        public string GRS_REMARKS { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RCS_APP_DATE { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RCS_ISSUE_DATE { get; set; }

        [Display(Name = "TC Ref No")]
        public string RCS_REF { get; set; }

        [Display(Name = "Remarks")]
        public string RCS_REMARKS { get; set; }

        [Display(Name = "Type")]
        public string IC_TYPE { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? IC_APP_DATE { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? IC_ISSUE_DATE { get; set; }
        [Display(Name = "TC No ")]
        public string IC_REF { get; set; }
        [Display(Name = "Remarks")]
        public string IC_REMARKS { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TENCEL_APP_DATE { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TENCEL_ISSUE_DATE { get; set; }
        [Display(Name = "Ref")]
        public string TENCEL_REF { get; set; }
        [Display(Name = "Remarks")]
        public string TENCEL_REMARKS { get; set; }

        [Display(Name = "App.Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PSCP_APP_DATE { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PSCP_ISSUE_DATE { get; set; }
        [Display(Name = "Ref")]
        public string PSCP_REF { get; set; }
        [Display(Name = "Remarks")]
        public string PSCP_REMARKS { get; set; }

        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

   
        public COM_EX_INVOICEMASTER INV { get; set; }
    }
}
