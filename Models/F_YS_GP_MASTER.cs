using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_YS_GP_MASTER
    {
        public F_YS_GP_MASTER()
        {
            F_YS_GP_DETAILS = new HashSet<F_YS_GP_DETAILS>();
        }
        public int GPID { get; set; }
        [Display(Name = "Gp No")]
        public string GPNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GPDATE { get; set; }
        public int? PARTY_ID { get; set; }
        public int? LC_ID { get; set; }
        [Display(Name = "Gp Type")]
        public string GPTYPE { get; set; }
        [Display(Name = "Truck No")]
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
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

        public COM_IMP_LCINFORMATION LC_ { get; set; }
        public F_YS_PARTY_INFO PARTY_ { get; set; }

        public ICollection<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }
    }
}
