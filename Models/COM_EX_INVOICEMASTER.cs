using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COM_EX_INVOICEMASTER
    {
        public COM_EX_INVOICEMASTER()
        {
            ComExInvdetailses = new HashSet<COM_EX_INVDETAILS>();
            ComExGspInfos = new List<COM_EX_GSPINFO>();
            ACC_EXPORT_REALIZATION = new List<ACC_EXPORT_REALIZATION>();
        }

        public int INVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Invoice Reference")]
        public string INVREF { get; set; }
        [Required(ErrorMessage = "Please insert an invoice number.")]
        [Remote(action: "IsInvNoInUse", controller: "ComExInvoiceMaster")]
        [Display(Name = "Invoice No")]
        public string INVNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "Invoice Date")]
        public DateTime INVDATE { get; set; }
        [Display(Name = "Duration")]
        public decimal? INVDURATION { get; set; }
        [Display(Name = "Invoice Qty.")]
        public double? INV_QTY { get; set; }
        [Display(Name = "Invoice Total Amount.")]
        public double? INV_AMOUNT { get; set; }
        //[Required(ErrorMessage = "Please select a LC.")]
        [Display(Name = "L/C No")]
        public int? LCID { get; set; }
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "Buyer")]
        public int BUYERID { get; set; }
        [Display(Name = "P.Doc No.")]
        public string PDOCNO { get; set; }
        [Display(Name = "Doc Notes")]
        public string DOC_NOTES { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Doc Neg. Date")]
        public DateTime? NEGODATE { get; set; }
        [Display(Name = "EXP No.")]
        public string TRUCKNO { get; set; }
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [Required(ErrorMessage = "Active status is not allowed to modify.")]
        [DisplayName("Is Active")]
        [Display(Name = "Active Status")]
        public bool ISACTIVE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Delivery Date")]
        public DateTime? DELDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Doc Sub. Date")]
        public DateTime? DOC_SUB_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Doc Rcv. Date")]
        public DateTime? DOC_RCV_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Bank Sub. Date")]
        public DateTime? BNK_SUB_DATE { get; set; }
        [Display(Name = "Bank Ref.")]
        public string BANK_REF { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Bill Date")]
        public DateTime? BILL_DATE { get; set; }
        [Display(Name = "Discrepancy")]
        public string DISCREPANCY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Bank Acceptance Date")]
        public DateTime? BNK_ACC_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Maturity Date")]
        public DateTime? MATUDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Bank Acceptance Posting")]
        public DateTime? BNK_ACC_POSTING { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "M.Ext. Date")]
        public DateTime? EXDATE { get; set; }
        [Display(Name = "OverDue Amount")]
        public double? ODAMOUNT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "OverDue Days")]
        public DateTime? ODRCVDATE { get; set; }
        [Display(Name = "PRC Amount (USD)")]
        public double? PRCAMOUNT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "PRC Date")]
        public DateTime? PRCDATE { get; set; }
        [Display(Name = "PRC Amount (BDT)")]
        public double? PRCAMNTTK { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [Display(Name = "Bank File")]
        public string BANKREFPATH { get; set; }
        [Display(Name = "Discrepancy File")]
        public string DISCREPANCYPATH { get; set; }
        [Display(Name = "Payment File")]
        public string PAYMENTPATH { get; set; }
        [Display(Name = "PRC Amount (EURO)")]
        public double? AMOUNT_EURO { get; set; }
        [Display(Name = "PRC Amount (BDT)")]
        public double? AMOUNT_BDT { get; set; }
        [Display(Name = "DOC. Value")]
        public double? DOC_VALUE { get; set; }
        [Display(Name = "Acceptance File")]
        public string BANKACCEPTPATH { get; set; }
        public bool IS_FINAL { get; set; }
        [Display(Name = "Total CBM")]
        public double? TOTAL_CBM { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped] 
        public string EncryptedInvId { get; set; }

        [NotMapped]
        public string DateFormat {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_BUYERINFO BUYER { get; set; }
        public COM_EX_LCINFO LC { get; set; }

        public ICollection<COM_EX_INVDETAILS> ComExInvdetailses { get; set; }
        public ICollection<COM_EX_GSPINFO> ComExGspInfos { get; set; }
        public ICollection<ACC_EXPORT_REALIZATION> ACC_EXPORT_REALIZATION { get; set; }

        public ICollection<COM_EX_CERTIFICATE_MANAGEMENT> COM_EX_CERTIFICATE_MANAGEMENT { get; set; }
    }
}
