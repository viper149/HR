using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COM_EX_PIMASTER : BaseEntity
    {
        public COM_EX_PIMASTER()
        {
            ACC_EXPORT_DODETAILS = new HashSet<ACC_EXPORT_DODETAILS>();
            COM_EX_PI_DETAILS = new HashSet<COM_EX_PI_DETAILS>();
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
            F_FS_FABRIC_RCV_DETAILS = new HashSet<F_FS_FABRIC_RCV_DETAILS>();
            COM_EX_LCDETAILS = new HashSet<COM_EX_LCDETAILS>();
            F_PR_INSPECTION_FABRIC_D_DETAILS = new HashSet<F_PR_INSPECTION_FABRIC_D_DETAILS>();
            COM_EX_ADV_DELIVERY_SCH_DETAILS = new HashSet<COM_EX_ADV_DELIVERY_SCH_DETAILS>();
            F_FS_FABRIC_RETURN_RECEIVE = new HashSet<F_FS_FABRIC_RETURN_RECEIVE>();
        }

        [Display(Name = "PI Id")]
        public int PIID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "IsPINoInUse", controller: "ComExPiMaster")]
        [Display(Name = "PI No")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public string PINO { get; set; }
        [Display(Name = "PI Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public DateTime PIDATE { get; set; }
        [Display(Name = "Duration")]
        public int? DURATION { get; set; }
        [Display(Name = "Validity")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? VALIDITY { get; set; }
        [Display(Name = "Party Name")]
        public int? BUYERID { get; set; }
        [Display(Name = "Tenor")]
        public int? TENOR { get; set; }
        [Display(Name = "Currency")]
        public int? CURRENCY { get; set; }
        [Display(Name = "Advising Bank")]
        public int? BANKID { get; set; }
        [Display(Name = "Delivery Period")]
        public string DEL_PERIOD { get; set; }
        [Display(Name = "Tolerance(%)")]
        public string TOLERANCE { get; set; }
        [Display(Name = "Negotiation")]
        public string NEGOTIATION { get; set; }
        [Display(Name = "Trade Terms")]
        public string INCOTERMS { get; set; }
        [Display(Name = "Inspection")]
        public string INSPECTION { get; set; }
        [Display(Name = "Team Name")]
        public int? TEAMID { get; set; }
        [Display(Name = "Team Person")]
        public int? TEAM_PERSONID { get; set; }
        [Display(Name = "Order Ref.")]
        public string ORDER_REF { get; set; }
        [Display(Name = "Nego. Bank")]
        public int? BANK_ID { get; set; }
        [Display(Name = "Port of Loading")]
        public string POL { get; set; }
        [Display(Name = "Port of Discharge")]
        public string POD { get; set; }
        [Display(Name = "Gross Weight")]
        public string GRS_WEIGHT { get; set; }
        [Display(Name = "Net Weight")]
        public string NET_WEIGHT { get; set; }
        [Display(Name = "Country of Origin")]
        public string COO { get; set; }
        [Display(Name = "Payment")]
        public string PAYMENT { get; set; }
        [Display(Name = "L/C No")]
        public string LCNO { get; set; }
        [Display(Name = "Shipment Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public string SHIPDATE { get; set; }
        [Display(Name = "Freight")]
        public string FREIGHT { get; set; }
        [Display(Name = "Insurance Coverage")]
        public string INSURANCE_COVERAGE { get; set; }
        [Display(Name = "Note of PO")]
        public string PONOTE { get; set; }
        public string PREVIOUS_DELIVERY_NOTE { get; set; }
        [Display(Name = "Export Status")]
        public int? EXP_STATUS { get; set; }
        [Display(Name = "Active Status")]
        public bool STATUS { get; set; }
        [Display(Name = "Delivery Start")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime? DEL_START { get; set; }
        [Display(Name = "Delivery Close")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime? DEL_CLOSE { get; set; }
        [Display(Name = "Brand Name")]
        public int? BRANDID { get; set; }
        public int? SID { get; set; }
        [Display(Name = "Followed By")]
        public string FLWBY { get; set; }
        public string ISDELETE { get; set; }
        public bool NON_EDITABLE { get; set; }

        [Display(Name = "SO No")]
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public double? PI_QTY { get; set; }
        public double? PI_TOTAL_VALUE { get; set; }

        [NotMapped]
        [Display(Name = "Lc No")]
        public string LcNoPi { get; set; }
        [NotMapped]
        [Display(Name = "File No (Lc)")]
        public string FileNo { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_BEN_BANK_MASTER BANK { get; set; }
        public BAS_BEN_BANK_MASTER BANK_ { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }
        public COM_TENOR TENORNavigation { get; set; }
        public BAS_BRANDINFO BRAND { get; set; }
        public BAS_TEAMINFO TEAM { get; set; }
        public MKT_TEAM PersonMktTeam { get; set; }
        public EXPORTSTATUS EXPORTSTATUS { get; set; }
        public CURRENCY CURRENCYS { get; set; }

        public BAS_SEASON S { get; set; }

        public ICollection<ACC_EXPORT_DODETAILS> ACC_EXPORT_DODETAILS { get; set; }
        public ICollection<COM_EX_PI_DETAILS> COM_EX_PI_DETAILS { get; set; }
        public ICollection<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
        public ICollection<COM_EX_LCDETAILS> COM_EX_LCDETAILS { get; set; }
        public ICollection<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }
        public ICollection<COM_EX_ADV_DELIVERY_SCH_DETAILS> COM_EX_ADV_DELIVERY_SCH_DETAILS { get; set; }
        public ICollection<F_FS_FABRIC_RETURN_RECEIVE> F_FS_FABRIC_RETURN_RECEIVE { get; set; }
    }
}
