using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_FS_UP_MASTER
    {
        public F_FS_UP_MASTER()
        {
            F_FS_UP_DETAILS = new HashSet<F_FS_UP_DETAILS>();
        }

        public int UP_ID { get; set; }
        [Display(Name = "UP NO")]
        public string UP_NO { get; set; }
        [Display(Name = "UP Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? UP_DATE { get; set; }
        [Display(Name = "Type")]
        public string TYPE { get; set; }
        [Display(Name = "UP Qty")]
        public double? UP_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public int? OPT5 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public ICollection<F_FS_UP_DETAILS> F_FS_UP_DETAILS { get; set; }
    }
}
