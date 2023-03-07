using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_STORE_INDENTMASTER : BaseEntity
    {
        public F_CHEM_STORE_INDENTMASTER()
        {
            F_CHEM_STORE_INDENTDETAILS = new HashSet<F_CHEM_STORE_INDENTDETAILS>();
        }

        public int CINDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Indent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CINDDATE { get; set; }
        [Display(Name = "Indent No.")]
        public string CINDNO { get; set; }
        [Display(Name = "Indent Sl. No")]
        public int? INDSLID { get; set; }
        [Display(Name = "Indent Type")]
        public int? INDTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [Display(Name = "Status")]
        public bool STATUS { get; set; }

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
        public bool IsLocked { get; set; }

        public F_CHEM_PURCHASE_REQUISITION_MASTER INDSL { get; set; }
        public F_CHEM_STORE_INDENT_TYPE FChemStoreIndentType { get; set; }
        public ICollection<F_CHEM_STORE_INDENTDETAILS> F_CHEM_STORE_INDENTDETAILS { get; set; }
    }
}
