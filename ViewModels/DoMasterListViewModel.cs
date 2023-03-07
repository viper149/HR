using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.ViewModels
{
    public class DoMasterListViewModel
    {
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Date")]
        public DateTime TRNSDATE { get; set; }
        [Display(Name = "DO No")]
        public string DONO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "DO Date")]
        public DateTime DODATE { get; set; }
        [Display(Name = "L/C No")]
        public int? LCID { get; set; }
        [Display(Name = "L/C No")]
        public string LCNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "DO Exp. Date")]
        public DateTime? DOEX { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Audit By")]
        public string AUDITBY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Audit On")]
        public DateTime? AUDITON { get; set; }
        [Display(Name = "Comments")]
        public string COMMENTS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }
    }
}
