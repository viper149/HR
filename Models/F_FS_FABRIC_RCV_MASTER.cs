using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_RCV_MASTER : BaseEntity
    {
        public F_FS_FABRIC_RCV_MASTER()
        {
            F_FS_FABRIC_RCV_DETAILS = new HashSet<F_FS_FABRIC_RCV_DETAILS>();
        }

        public int RCVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Receive Date")]
        public DateTime? RCVDATE { get; set; }
        public int? SECID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "No of Rolls")]
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "FFTR No.")]
        public string FFTR_NO { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_BAS_SECTION SEC { get; set; }
        public ICollection<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
    }
}
