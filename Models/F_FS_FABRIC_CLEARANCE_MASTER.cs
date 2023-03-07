using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_CLEARANCE_MASTER
    {
        public F_FS_FABRIC_CLEARANCE_MASTER()
        {
            F_FS_FABRIC_CLEARANCE_DETAILS = new HashSet<F_FS_FABRIC_CLEARANCE_DETAILS>();
        }

        public int CLID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name="Trns. Date")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name="Style")]
        public int? FABCODE { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name="Wash Code")]
        public int? WASH_CODE { get; set; }
        [Display(Name="Packing List Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PACKING_LIST_DATE { get; set; }
        [Display(Name="Order No")]
        public int? ORDER_NO { get; set; }
        [Display(Name="Buyer")]
        public int? BUYERID { get; set; }
        [Display(Name="Factory")]
        public int? FACTORYID { get; set; }
        [Display(Name="From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DATE_FROM { get; set; }
        [Display(Name="To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DATE_TO { get; set; }
        [Display(Name="Roll Qty")]
        public int? ROLE_QTY { get; set; }
        [Display(Name="Role From")]
        public string ROLE_FROM { get; set; }
        [Display(Name="Roll To")]
        public string ROLE_TO { get; set; }
        [Display(Name="Shift")]
        public string SHIFT { get; set; }
        [Display(Name="Remarks")]
        public string REMARKS { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }

        [Display(Name = "Roll From")]
        public string OPT2 { get; set; }
        [Display(Name = "Roll To")]
        public string OPT1 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public RND_FABRICINFO FABCODENavigation { get; set; }
        public RND_PRODUCTION_ORDER PO { get; set; }
        public BAS_BUYERINFO Buyer { get; set; }
        public BAS_BUYERINFO Factory { get; set; }

        public ICollection<F_FS_FABRIC_CLEARANCE_DETAILS> F_FS_FABRIC_CLEARANCE_DETAILS { get; set; }


    }
}
