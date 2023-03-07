using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_RECEIVE_MASTER : BaseEntity
    {
        public F_GEN_S_RECEIVE_MASTER()
        {
            F_GEN_S_RECEIVE_DETAILS = new HashSet<F_GEN_S_RECEIVE_DETAILS>();
            PROC_BILL_MASTER = new HashSet<PROC_BILL_MASTER>();
        }

        [Display(Name = "Receive No.")]
        public int GRCVID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Receive Date")]
        public DateTime? RCVDATE { get; set; }
        [Display(Name = "Receive Type")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? RCVTID { get; set; }
        [Display(Name = "Received By")]
        public int? RCVBY { get; set; }
        [Display(Name = "Checked By")]
        public int? CHECKBY { get; set; }
        [Display(Name = "PI No.")]
        public int? LC_ID { get; set; }
        [Display(Name = "Supplier")]
        public int? SUPPID { get; set; }
        [Display(Name = "Origin")]
        public int? ORIGIN { get; set; }
        [Display(Name = "Challan No.")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public string CHALLAN_NO { get; set; }
        [Display(Name = "Challan Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public DateTime? CHALLAN_DATE { get; set; }
        [Display(Name = "CNF Challan No.")]
        public string CNF_CHALLAN_NO { get; set; }
        [Display(Name = "CNF Challan Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CNF_CHALLAN_DATE { get; set; }
        [Display(Name = "CNF No.")]
        public int? CNFID { get; set; }
        [Display(Name = "Transport")]
        public int? TRANSPID { get; set; }
        [Display(Name = "Vehicle No")]
        public string VEHICAL_NO { get; set; }
        [Display(Name = "Gate Entry No.")]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public int? GE_ID { get; set; }
        [Display(Name = "Gate Entry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GEDATE { get; set; }
        [Display(Name = "QC Approve No.")]
        public int? QCPASS { get; set; }
        [Display(Name = "MRR No.")]
        public int? MRR { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public int? GINVID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

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
        public string EncryptedId { get; set; }
        [NotMapped]
        public bool IsLocked { get; set; }
        [NotMapped]
        public string QCINFO { get; set; }
        [NotMapped]
        public string MRRINFO { get; set; }

        public F_GEN_S_INDENTMASTER GINV { get; set; }
        public F_HRD_EMPLOYEE CHECKBYNavigation { get; set; }
        public COM_IMP_CNFINFO CNF { get; set; }
        public COM_IMP_LCDETAILS LCD { get; set; }
        public COUNTRIES ORIGINNavigation { get; set; }
        public F_HRD_EMPLOYEE RCVBYNavigation { get; set; }
        public F_BAS_RECEIVE_TYPE RCVT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public BAS_TRANSPORTINFO TRANSP { get; set; }
        public F_GEN_S_MRR MRRNavigation { get; set; }
        public F_GEN_S_QC_APPROVE QCPASSNavigation { get; set; }

        public ICollection<F_GEN_S_RECEIVE_DETAILS> F_GEN_S_RECEIVE_DETAILS { get; set; }
        public ICollection<PROC_BILL_MASTER> PROC_BILL_MASTER { get; set; }
    }
}
