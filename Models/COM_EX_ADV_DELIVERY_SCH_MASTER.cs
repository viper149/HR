using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_EX_ADV_DELIVERY_SCH_MASTER : BaseEntity
    {
        public COM_EX_ADV_DELIVERY_SCH_MASTER()
        {
            COM_EX_ADV_DELIVERY_SCH_DETAILS = new HashSet<COM_EX_ADV_DELIVERY_SCH_DETAILS>();
        }

        public int DSID { get; set; }
        [Display(Name = "Ref No")]
        public string DSNO { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DSDATE { get; set; }
        [Display(Name = "Type")]
        public string DSTYPE { get; set; }
        [Display(Name = "Buyer Name")]
        public int? BUYER_ID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }

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

        public BAS_BUYERINFO BUYER { get; set; }

        public ICollection<COM_EX_ADV_DELIVERY_SCH_DETAILS> COM_EX_ADV_DELIVERY_SCH_DETAILS { get; set; }
    }
}
