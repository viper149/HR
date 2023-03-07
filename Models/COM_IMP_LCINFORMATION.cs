using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COM_IMP_LCINFORMATION
    {
        public COM_IMP_LCINFORMATION()
        {
            COM_IMP_INVOICEINFO = new HashSet<COM_IMP_INVOICEINFO>();
            FChemStoreReceiveMasters = new HashSet<F_CHEM_STORE_RECEIVE_MASTER>();
            COM_IMP_LCDETAILS = new HashSet<COM_IMP_LCDETAILS>();
            ACC_LOAN_MANAGEMENT_M = new HashSet<ACC_LOAN_MANAGEMENT_M>();
            F_YS_GP_MASTER = new HashSet<F_YS_GP_MASTER>();
        }

        [Display(Name = "L/C Id")]
        public int LC_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Required(ErrorMessage = "LC no can not be empty."), Remote(controller: "ComImpLcInformation", action: "IsLcNoInUse"), Display(Name = "L/C No")]
        public string LCNO { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd MMM yyyy}"), Required(ErrorMessage = "LC date can not be empty."), Display(Name = "L/C Date")]
        public DateTime LCDATE { get; set; }
        [Display(Name = "L/C Path")]
        public string LCPATH { get; set; }
        [Display(Name = "L/C Type")]
        public int LTID { get; set; }
        [Display(Name = "Currency")]
        public string CURRENCY { get; set; }
        [Display(Name = "FTT")]
        public bool ISFTT { get; set; }
        [Required(ErrorMessage = "Please select an item.")]
        [Display(Name = "Item Type")]
        public int CATID { get; set; }
        [Display(Name = "Supplier")]
        public int? SUPPID { get; set; }
        [Display(Name = "L/C Op. Bank")]
        public int? BANKID { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd MMM yyyy}"), Display(Name = "Shipment Date")]
        public DateTime? SHIPDATE { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd MMM yyyy}"), Display(Name = "Expire Date")]
        public DateTime? EXPDATE { get; set; }
        [Display(Name = "Tenor")]
        public int? TID { get; set; }
        [Display(Name = "Export L/C")]
        public int? LCID { get; set; }
        [Display(Name = "Tollerance(%)")]
        public string TOLLERANCE { get; set; }
        [Display(Name = "Country Origin")]
        public string ORIGIN { get; set; }
        [Display(Name = "Dest. Port BD")]
        public string DESPORT { get; set; }
        [Display(Name = "Insurance")]
        public int? INSID { get; set; }
        [Display(Name = "Insurance Path")]
        public string INSPATH { get; set; }
        [Display(Name = "Cover Note Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CNOTEDATE { get; set; }
        [Display(Name = "Cover Note No")]
        public string CNOTENO { get; set; }
        [Display(Name = "M.L/C Op. Bank")]
        public int? BANK_ID { get; set; }
        [Display(Name = "Lien Value %")]
        public double? LIENVAL { get; set; }
        [Display(Name = "Lien Balance")]
        public double? BALANCE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [Required(ErrorMessage = "Must have a file number.")]
        [Display(Name = "File No.")]
        [Remote(action: "IsFileNoInUse", controller: "CommercialImportLC")]
        public string FILENO { get; set; }
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

        public BAS_BEN_BANK_MASTER BANK { get; set; }
        public BAS_BUYER_BANK_MASTER BANK_ { get; set; }
        public BAS_PRODCATEGORY CAT { get; set; }
        public BAS_INSURANCEINFO INS { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public COM_TENOR COM_TENOR { get; set; }
        public COM_IMP_LCTYPE COM_IMP_LCTYPE { get; set; }
        public COM_EX_LCINFO ComExLcinfo { get; set; }

        public ICollection<COM_IMP_INVOICEINFO> COM_IMP_INVOICEINFO { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> FChemStoreReceiveMasters { get; set; }
        public ICollection<COM_IMP_LCDETAILS> COM_IMP_LCDETAILS { get; set; }
        public ICollection<ACC_LOAN_MANAGEMENT_M> ACC_LOAN_MANAGEMENT_M { get; set; }
        public ICollection<F_YS_GP_MASTER> F_YS_GP_MASTER { get; set; }
    }
}
