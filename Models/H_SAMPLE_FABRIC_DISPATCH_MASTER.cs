using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_FABRIC_DISPATCH_MASTER
    {
        public H_SAMPLE_FABRIC_DISPATCH_MASTER()
        {
            H_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<H_SAMPLE_FABRIC_DISPATCH_DETAILS>();
        }

        public int SFDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "GP No.")]
        public string GPNO { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ISSUE_DATE { get; set; }
        public int? BUYERID { get; set; }
        public int? BRANDID { get; set; }
        public int? MERCID { get; set; }
        [Display(Name = "Purpose")]
        public string PURPOSE { get; set; }
        [Display(Name = "Is Returnable")]
        public bool ISRETURNABLE { get; set; }
        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RETURN_DATE { get; set; }
        public int? MKT_TEAMID { get; set; }
        [Display(Name = "Through")]
        public string THROUGH { get; set; }

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
        public BAS_BRANDINFO BRAND { get; set; }
        public MERCHANDISER MERCH { get; set; }
        public MKT_TEAM MktTeam { get; set; }

        public ICollection<H_SAMPLE_FABRIC_DISPATCH_DETAILS> H_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
    }
}
