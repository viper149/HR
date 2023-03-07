using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_STORE_RECEIVE_MASTER : BaseEntity
    {
        public F_CHEM_STORE_RECEIVE_MASTER()
        {
            F_CHEM_STORE_RECEIVE_DETAILS = new HashSet<F_CHEM_STORE_RECEIVE_DETAILS>();
        }

        [Display(Name = "Chemical Receive Number")]
        public int CHEMRCVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Receive Date")]
        public DateTime? RCVDATE { get; set; }
        [Display(Name = "Receive Type")]
        public int? RCVTID { get; set; }
        [Display(Name = "Received By Id")]
        public int? RCVBY { get; set; }
        [Display(Name = "Checked By Id")]
        public int? CHECKBY { get; set; }
        [Display(Name = "Invoice No.")]
        public int? INVID { get; set; }
        [Display(Name = "LC ID")]
        public int? LC_ID { get; set; }
        [Display(Name = "Supplier Id")]
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
        [Display(Name = "Gate Pass No.")]
        public string GE_ID { get; set; }
        [Display(Name = "Gate Pass Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GEDATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
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
        public bool IsLocked { get; set; }

        public COM_IMP_CNFINFO CNF { get; set; }
        public F_BAS_RECEIVE_TYPE RCVT { get; set; }
        public COM_IMP_INVOICEINFO ComImpInvoiceinfo { get; set; }
        public BAS_SUPPLIERINFO BasSupplierinfo { get; set; }
        public COM_IMP_LCINFORMATION ComImpLcinformation { get; set; }
        public COUNTRIES Countries { get; set; }
        public F_HRD_EMPLOYEE RcvEmployee { get; set; }
        public F_HRD_EMPLOYEE CheckEmployee { get; set; }
        public BAS_TRANSPORTINFO BasTransportinfo { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_DETAILS> F_CHEM_STORE_RECEIVE_DETAILS { get; set; }
    }
}
