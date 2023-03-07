using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COM_IMP_INVOICEINFO
    {
        public COM_IMP_INVOICEINFO()
        {
            F_YS_YARN_RECEIVE_MASTER = new HashSet<F_YS_YARN_RECEIVE_MASTER>();
            ComImpInvdetailses = new HashSet<COM_IMP_INVDETAILS>();
            FChemStoreReceiveMasters = new HashSet<F_CHEM_STORE_RECEIVE_MASTER>();
            ACC_LOAN_MANAGEMENT_D = new HashSet<ACC_LOAN_MANAGEMENT_D>();
            F_YS_YARN_RECEIVE_MASTER_S = new HashSet<F_YS_YARN_RECEIVE_MASTER_S>();
        }

        [Display(Name = "Invoice Id")]
        public int INVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "IsInvNoInUse", controller: "ComImpInvoiceInfo")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "Invoice No")]
        public string INVNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Please choose a specific date.")]
        [Display(Name = "Invoice Date")]
        public DateTime INVDATE { get; set; }
        [Display(Name = "Invoice File")]
        public string INVPATH { get; set; }
        [Display(Name = "L/C No.")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public int? LC_ID { get; set; }
        [Display(Name = "Loading Port")]
        public string LPORT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Doc Hand Over Date")]
        public DateTime? DOCHANDSON { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "ETA Date")]
        public DateTime? ETADATE { get; set; }
        [Display(Name = "Delivery Status")]
        public int? DEL_STATUS { get; set; }
        [Display(Name = "Delivery Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DEL_DATE { get; set; }
        [Display(Name = "Container Qty.")]
        public decimal? CONTQTY { get; set; }
        [Display(Name = "Container Size")]
        public int? CONTSIZE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Ship Board Date")]
        public DateTime? SBDATE { get; set; }
        [Display(Name = "B/L No")]
        public string BLNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "B/L Date")]
        public DateTime? BLDATE { get; set; }
        [Display(Name = "B/L File")]
        public string BLPATH { get; set; }
        [Display(Name = "Bill Entry Value")]
        public decimal? BENTRYVALUE { get; set; }
        [Display(Name = "Bill Entry No")]
        public string BENTRYNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Bill Entry Date")]
        public DateTime? BENTRYDATE { get; set; }

        [Display(Name = "Marine Policy No")]
        public string MPNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Marine Policy Date")]
        public DateTime? MPDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Marine Policy Submit Date")]
        public DateTime? MPSUB_DATE { get; set; }
        [Display(Name = "Marine Policy Bill")]
        public string MPBILL { get; set; }

        [Display(Name = "Rcv. Status")]
        public string RCVSTATUS { get; set; }
        [Display(Name = "MRR No.")]
        public string MRRNO { get; set; }
        [Display(Name = "MRR Date")]
        public string MRRDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Payment Date")]
        public DateTime? PAYMENTDATE { get; set; }

        [Display(Name = "CnF Name")]
        public int? CnF { get; set; }

        [Display(Name = "C&F Bill No")]
        public string CnFBILL { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "C&F Bill Date")]
        public DateTime? CnFBILLDATE { get; set; }
        [Display(Name = "Transport Name")]
        public int? TRNSPID { get; set; }
        [Display(Name = "Truck Qty.")]
        public decimal? TRUCKQTY { get; set; }
        [Display(Name = "Transport Bill No.")]
        public string TRNSPBILL { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Transport Bill Date")]
        public DateTime? TRNSPBILLDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Transport Bill Submit Date")]
        public DateTime? TRNS_BILL_SUB_DATE { get; set; }
        [Display(Name = "Ship By")]
        public string SHIPBY { get; set; }
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [NotMapped]
        public bool IsLocked { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_TRANSPORTINFO TRNSP { get; set; }
        public COM_IMP_DEL_STATUS DelStatus { get; set; }
        public COM_IMP_LCINFORMATION LC { get; set; }
        public COM_CONTAINER COM_CONTAINER { get; set; }
        public COM_IMP_CNFINFO CNF { get; set; }

        public ICollection<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public ICollection<COM_IMP_INVDETAILS> ComImpInvdetailses { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> FChemStoreReceiveMasters { get; set; }
        public ICollection<ACC_LOAN_MANAGEMENT_D> ACC_LOAN_MANAGEMENT_D { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER_S> F_YS_YARN_RECEIVE_MASTER_S { get; set; }
    }
}
