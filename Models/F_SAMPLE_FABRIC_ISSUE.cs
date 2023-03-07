using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_ISSUE : BaseEntity
    {
        public F_SAMPLE_FABRIC_ISSUE()
        {
            F_SAMPLE_FABRIC_ISSUE_DETAILS = new HashSet<F_SAMPLE_FABRIC_ISSUE_DETAILS>();
        }

        public int SFIID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Req. Date")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime REQ_DATE { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ISSUE_DATE { get; set; }
        [Display(Name = "Sample Requisition No.")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Remote(action: "IsSrNoInUse", controller: "SampleFabric", AdditionalFields = "MKT_TEAMID")]
        [MinLength(4, ErrorMessage = "The field {0} must greater or equal length of {1}")]
        public string SRNO { get; set; }
        [Display(Name = "Merchandiser Name")]
        public string MARCHANDISER_NAME { get; set; }
        [Display(Name = "Buyer Name")]
        public int? BUYERID { get; set; }
        [Display(Name = "Brand Name")]
        public int? BRANDID { get; set; }
        [Display(Name = "MKT Team ID")]
        public int? MKT_TEAMID { get; set; }
        [Display(Name = "Issue Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Cancel Issue")]
        public bool HasRemoved { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_BUYERINFO BUYER { get; set; }
        public MKT_TEAM MKT_TEAM { get; set; }
        public BAS_BRANDINFO BRAND { get; set; }

        public ICollection<F_SAMPLE_FABRIC_ISSUE_DETAILS> F_SAMPLE_FABRIC_ISSUE_DETAILS { get; set; }
    }
}
