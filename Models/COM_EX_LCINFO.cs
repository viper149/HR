using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COM_EX_LCINFO : BaseEntity
    {
        public COM_EX_LCINFO()
        {
            COM_EX_INVOICEMASTER = new HashSet<COM_EX_INVOICEMASTER>();
            AccExportDoMaster = new HashSet<ACC_EXPORT_DOMASTER>();
            COM_EX_LCDETAILS = new HashSet<COM_EX_LCDETAILS>();
            COM_EX_CASHINFO = new HashSet<COM_EX_CASHINFO>();
            ACC_LOCAL_DOMASTER = new HashSet<ACC_LOCAL_DOMASTER>();
            F_FS_UP_DETAILS = new HashSet<F_FS_UP_DETAILS>();
        }

        [Display(Name = "L/C Id")]
        public int LCID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }

        [Display(Name = "File No")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Remote(action: "IsFileNoInUse", controller: "ComExLcInfo")]
        public string FILENO { get; set; }
        [Required]
        //[Remote(action: "IsLcNoInUse", controller: "ComExLcInfo")]
        [Display(Name = "L/C No")]
        public string LCNO { get; set; }
        [Display(Name = "Active/Inactive")]
        public bool LC_STATUS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Master L/C Date")]
        public DateTime? MLCDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "L/C Date")]
        public DateTime? LCDATE { get; set; }
        [Display(Name = "Ament. No")]
        public string AMENTNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Ament. Date")]
        public DateTime? AMENTDATE { get; set; }
        [Display(Name = "Ament. Value")]
        public double? AMENTVALUE { get; set; }
        [Display(Name = "L/C Value")]
        public double? VALUE { get; set; }
        [Display(Name = "Currency")]
        public string CURRENCY { get; set; }
        [Display(Name = "Buyer")]
        public int? BUYERID { get; set; }
        [Display(Name = "L/C Op. Bank-Party")]
        public int? BANK_ID { get; set; }
        [Display(Name = "Marketing Person")]
        public int? TEAMID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Expiry Date")]
        public DateTime? EX_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Ship. Date")]
        public DateTime? SHIP_DATE { get; set; }
        [Display(Name = "Trade Terms")]
        public int? TTERMS { get; set; }
        [Display(Name = "Tenor")]
        public int? TID { get; set; }
        [Display(Name = "Trade Terms")]
        public string TRADETERMS { get; set; }
        [Display(Name = "Tenor")]
        public string TENOR { get; set; }
        [Display(Name = "Over Due Interest")]
        public string ODUEINTEREST { get; set; }
        [Display(Name = "Exp. & Fab. L/C")]
        public string EXP { get; set; }
        [Display(Name = "Master LC/Con. No.")]
        public string MLCNO { get; set; }
        [Display(Name = "IRC No.")]
        public string IRC { get; set; }
        [Display(Name = "ERC No.")]
        public string ERC { get; set; }
        [Display(Name = "Garments Oty.")]
        public decimal? GARMENT_QTY { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Item Name")]
        public string ITEM { get; set; }
        [Display(Name = "App. VAT/BIN Reg.")]
        public string VAT_REG { get; set; }
        [Display(Name = "App. Bank VAT/BIN No.")]
        public string VAT_REG_BANK { get; set; }
        [Display(Name = "App. VAT/BIN Area")]
        public string AREA { get; set; }
        [Display(Name = "App. TIN")]
        public string TIN { get; set; }
        [Display(Name = "Others")]
        public string OTHERS { get; set; }
        [Display(Name = "Ben. VAT/BIN")]
        public string BVAT_REG { get; set; }
        [Display(Name = "Ben. Area")]
        public string BAREA { get; set; }
        [Display(Name = "Ben. TIN")]
        public string BTIN { get; set; }
        [Display(Name = "H.S. Code")]
        public string HSCODE { get; set; }
        [Display(Name = "AD Ref. No.")]
        public string ADREF { get; set; }
        [Display(Name = "Advising Bank")]
        public int? BANKID { get; set; }
        [Display(Name = "Doc. Neg Bank")]
        public int? NEGOBANKID { get; set; }
        [Display(Name = "Notify Bank")]
        public int? NTFYBANKID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "File Entry Date")]
        public DateTime? LCRCVDATE { get; set; }
        [Display(Name = "Agree. Dis.(%)")]
        public string DISCOUNT { get; set; }
        [Display(Name = "Ship. Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "UD No.")]
        public string UDNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "UD Date")]
        public DateTime? UDDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "UD R&C Sub-Date")]
        public DateTime? UDSUBDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "M.L/C Rcv. Sub-Date")]
        public DateTime? MLCSUBDATE { get; set; }
        [Display(Name = "UP No")]
        public string UPNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Up Date")]
        public DateTime? UP_DATE { get; set; }
        [Display(Name = "Port of Landing")]
        public string PORTLOADING { get; set; }
        [Display(Name = "Port of Discharge")]
        public string PORTDISCHARGE { get; set; }
        [Display(Name = "Vessel Name")]
        public string VESSEL { get; set; }
        [Display(Name = "Marks & No.")]
        public string MARKS { get; set; }
        [Display(Name = "Sailing on or about")]
        public string SAILING { get; set; }
        [Display(Name = "DO Status")]
        public string DOSTATUS { get; set; }
        [Display(Name = "Contract Status")]
        public string CONTRACTSTATUS { get; set; }
        [Display(Name = "Export")]
        public string EXPORTSTATUS { get; set; }
        public bool? ISDELETE { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [Display(Name = "UD File")]
        public string UDFILEUPLOAD { get; set; }
        [Display(Name = "UP File")]
        public string UPFILEUPLOAD { get; set; }
        [Display(Name = "Cost Sheet File")]
        public string COSTSHEETFILEUPLOAD { get; set; }
        [Display(Name = "Master L/C File")]
        public string MLCFILE { get; set; }
        [Display(Name = "L/C File")]
        public string LCFILE { get; set; }
        [Display(Name = "L/C Cancellation Cause(If Any)")]
        public string LC_CANCEL_REMARKS { get; set; }
        [NotMapped]
        public bool IsInDo { get; set; }
        [NotMapped]
        public string DoNo { get; set; }
        [NotMapped]
        public bool ReadOnly { get; set; }
        [Display(Name = "L/C Qty")]
        [NotMapped] public double? LC_QTY { get; set; }
        
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
        public BAS_BUYER_BANK_MASTER NTFYBANK { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }
        public MKT_TEAM TEAM { get; set; }
        public COM_TENOR COM_TENOR { get; set; }
        public COM_TRADE_TERMS COM_TRADE_TERMS { get; set; }

        public ICollection<COM_EX_INVOICEMASTER> COM_EX_INVOICEMASTER { get; set; }
        public ICollection<ACC_EXPORT_DOMASTER> AccExportDoMaster { get; set; }
        public ICollection<COM_EX_LCDETAILS> COM_EX_LCDETAILS { get; set; }
        public ICollection<COM_EX_CASHINFO> COM_EX_CASHINFO { get; set; }
        public ICollection<ACC_LOCAL_DOMASTER> ACC_LOCAL_DOMASTER { get; set; }
        public ICollection<F_FS_UP_DETAILS> F_FS_UP_DETAILS { get; set; }
    }
}
