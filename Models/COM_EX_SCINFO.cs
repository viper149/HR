using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COM_EX_SCINFO
    {
        public COM_EX_SCINFO()
        {
            COM_EX_SCDETAILS = new HashSet<COM_EX_SCDETAILS>();
            ACC_LOCAL_DOMASTER = new HashSet<ACC_LOCAL_DOMASTER>();
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
        }
        public int SCID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Contact No")]
        [Remote(action: "IsScNoInUse", controller: "ComExScInfo")]
        public string SCNO { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Contact Date")]
        public DateTime? SCDATE { get; set; }
        [Display(Name = "Seller Name")]
        public string SCPERSON { get; set; }
        [Display(Name = "Seller Phone")]
        public string SCPHONE { get; set; }
        [Display(Name = "Seller Email")]
        public string SCEMAIL { get; set; }
        [Display(Name = "Buyer")]
        public int? BUYERID { get; set; }
        [Display(Name = "Buyer Name")]
        public string BCPERSON { get; set; }
        [Display(Name = "Buyer Phone")]
        public string BCPHONE { get; set; }
        [Display(Name = "Buyer Email")]
        public string BCEMAIL { get; set; }

        [Display(Name = "Currency")]
        public string CURRENCY { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        public DateTime? DELDATE { get; set; }
        [Display(Name = "Con Validity")]
        public string CON_VAL { get; set; }
        [Display(Name = "Initial Deposit")]
        public string INIDEPOSIT { get; set; }
        [Display(Name = "Transport Fare")]
        public double? TRNSFARE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Start Date")]
        public DateTime? DEL_STARTDATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Close Date")]
        public DateTime? DEL_CLOSEDATE { get; set; }
        [Display(Name = "Dev. Code")]
        public string DEV_CODE { get; set; }
        [Display(Name = "Fabric Process")]
        public string FAB_PROCESS { get; set; }

        [Display(Name = "Extra Process")]
        public string EXTRA_PROCESS { get; set; }
        [Display(Name = "Inspection")]
        public string INSPECTION { get; set; }
        [Display(Name = "LPO Note")]
        public string LPONOTE { get; set; }
        [Display(Name = "Contract Note")]
        public string CONTRACTNOTE { get; set; }
        [Display(Name = "Warp Yarn")]
        public string WARP_PROVBY { get; set; }
        [Display(Name = "Weft Yarn")]
        public string WEFT_PROVBY { get; set; }
        [Display(Name = "Dyes & Chem.")]
        public bool ISPROV_CHEM { get; set; }
        [Display(Name = "Warping")]
        public bool ISWARPING { get; set; }
        [Display(Name = "Dyeing")]
        public bool ISDYEING { get; set; }
        [Display(Name = "LCB")]
        public bool ISLCB { get; set; }
        [Display(Name = "Sizing")]
        public bool ISSIZING { get; set; }
        [Display(Name = "Weaving")]
        public bool ISWEAVING { get; set; }
        [Display(Name = "Over Dyeing")]
        public bool ISOVERDYEING { get; set; }
        [Display(Name = "Finishing")]
        public bool ISFINISHING { get; set; }
        [Display(Name = "Delivery Mode")]
        public bool ISDELMODE { get; set; }
        [Display(Name = "Tran. Provided By")]
        public bool ISPROV_TRNS { get; set; }
        [Display(Name = "Fair Provided By")]
        public bool ISPROVE_FARE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Payment Date")]
        public DateTime? PAYDATE { get; set; }
        [Display(Name = "Payment Mode")]
        public string PAYMODE { get; set; }
        [Display(Name = "Deposit Bank")]
        public int? BANK_ID { get; set; }
        [Display(Name = "Check No")]
        public string CHKNO { get; set; }
        [Display(Name = "Cash Amount")]
        public double? CASHAMOUNT { get; set; }
        [Display(Name = "Bank Amount")]
        public double? CHKAMOUNT { get; set; }
        public string USRID { get; set; }
        public string ISDELETE { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_BUYER_BANK_MASTER BANK_ { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }

        public ICollection<COM_EX_SCDETAILS> COM_EX_SCDETAILS { get; set; }
        public ICollection<ACC_LOCAL_DOMASTER> ACC_LOCAL_DOMASTER { get; set; }
        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
    }
}
