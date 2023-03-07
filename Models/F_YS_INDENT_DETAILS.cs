using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_INDENT_DETAILS : BaseEntity
    {
        public F_YS_INDENT_DETAILS()
        {
            COM_IMP_WORK_ORDER_DETAILS = new HashSet<COM_IMP_WORK_ORDER_DETAILS>();
        }

        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? INDSLID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Indent No.")]
        public int? INDID { get; set; }
        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Count Name")]
        public int? PRODID { get; set; }
        [Display(Name = "No of Cones")]
        public int? NO_OF_CONE { get; set; }
        [Display(Name = "SLUB Code")]
        public int? SLUB_CODE { get; set; }
        [Display(Name = "RAW")]
        public int? RAW { get; set; }
        [Display(Name = "Previous Lot")]
        public int? PREV_LOTID { get; set; }
        [Display(Name = "Consumption Amount")]
        public double? CNSMP_AMOUNT { get; set; }
        [Display(Name = "Stock Amount")]
        public double? STOCK_AMOUNT { get; set; }
        [Display(Name = "Order Quantity")]
        [Range(0.0, double.MaxValue, ErrorMessage = "{0} must be greater than {1}")]
        public double? ORDER_QTY { get; set; }
        [Display(Name = "Yarn From")]
        public int? YARN_FROM { get; set; }
        [Display(Name = "Yarn For")]
        public int? YARN_FOR { get; set; }
        [Display(Name = "Yarn Type")]
        public string YARN_TYPE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "ETR")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ETR { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Last Indent No.")]
        public string LAST_INDENT_NO { get; set; }
        [Display(Name = "Last Indent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public string LAST_INDENT_DATE { get; set; }

        public F_YS_RAW_PER RAWNavigation { get; set; }
        public F_YS_SLUB_CODE SLUB_CODENavigation { get; set; }
        public F_YS_INDENT_MASTER IND { get; set; }
        public F_BAS_SECTION SEC { get; set; }
        public BAS_YARN_COUNTINFO BASCOUNTINFO { get; set; }
        public F_BAS_UNITS FBasUnits { get; set; }
        public YARNFOR YARN_FORNavigation { get; set; }
        public YARNFROM YARN_FROMNavigation { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public RND_PURCHASE_REQUISITION_MASTER RND_PURCHASE_REQUISITION_MASTER { get; set; }

        public ICollection<COM_IMP_WORK_ORDER_DETAILS> COM_IMP_WORK_ORDER_DETAILS { get; set; }
    }
}
