using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_MASTER2
    {
        public F_YS_YARN_RECEIVE_MASTER2()
        {
            F_YS_YARN_RECEIVE_DETAILS2 = new HashSet<F_YS_YARN_RECEIVE_DETAILS2>();
        }

        public int YRCVID { get; set; }
        [Display(Name = "Receive Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? YRCVDATE { get; set; }
        [Display(Name = "Receive Type")]
        public int? RCVTID { get; set; }
        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Sub Section")]
        public int? SUBSECID { get; set; }
        [Display(Name = "Challan No")]
        public string CHALLANNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Challan Date")]
        public DateTime? CHALLANDATE { get; set; }
        public bool? ISRETURNABLE { get; set; }
        [Display(Name = "Indent Type")]
        public string INDENT_TYPE { get; set; }
        [Display(Name = "Supplier")]
        public int? RCVFROM { get; set; }
        [Display(Name = "Gate Entry No")]
        public string G_ENTRY_NO { get; set; }
        [Display(Name = "Gate Entry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? G_ENTRY_DATE { get; set; }
        [Display(Name = "Order No")]
        public int? SO_NO { get; set; }
        [Display(Name = "Truck No")]
        public string TRUCKNO { get; set; }
        public int? OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        [NotMapped]
        public string EncryptedId { get; set; }
        public BAS_SUPPLIERINFO RCVFROMNavigation { get; set; }
        public F_BAS_SECTION SEC { get; set; }
        public F_BAS_SUBSECTION SUBSEC { get; set; }
        public RND_PRODUCTION_ORDER SO { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS2> F_YS_YARN_RECEIVE_DETAILS2 { get; set; }
    }
}
