using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_WASTAGE_RECEIVE_M : BaseEntity
    {
        public F_FS_WASTAGE_RECEIVE_M()
        {
            F_FS_WASTAGE_RECEIVE_D = new HashSet<F_FS_WASTAGE_RECEIVE_D>();
        }

        public int WRID { get; set; }
        [Display(Name = "Wastage Receive Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? WRDATE { get; set; }
        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Vehicle No.")]
        public string WTRNO { get; set; }
        [Display(Name = "WTR Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? WTRDATE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

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

        public F_BAS_SECTION SEC { get; set; }
        public ICollection<F_FS_WASTAGE_RECEIVE_D> F_FS_WASTAGE_RECEIVE_D { get; set; }
    }
}
