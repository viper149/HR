using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class ACC_EXPORT_DOMASTER
    {
        public ACC_EXPORT_DOMASTER()
        {
            ACC_EXPORT_DODETAILS = new HashSet<ACC_EXPORT_DODETAILS>();
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
            F_FS_FABRIC_RETURN_RECEIVE = new HashSet<F_FS_FABRIC_RETURN_RECEIVE>();
        }
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime TRNSDATE { get; set; }
        [Remote(action: "IsDoNoInUse",controller: "AccExportDoMaster")]
        [Display(Name = "DO No")]
        public string DONO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "DO Date")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public DateTime DODATE { get; set; }
        [Display(Name = "L/C No")]
        public int? LCID { get; set; }
        [NotMapped]
        [Display(Name = "L/C No")]
        public string LCNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "DO Exp. Date")]
        public DateTime? DOEX { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Audit By")]
        public string AUDITBY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Audit On")]
        public DateTime? AUDITON { get; set; }
        [Display(Name = "Comments")]
        public string COMMENTS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [Display(Name = "Cancel?")]
        public bool IS_CANCELLED { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public COM_EX_LCINFO ComExLcInfo { get; set; }

        public ICollection<ACC_EXPORT_DODETAILS> ACC_EXPORT_DODETAILS { get; set; }
        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_RETURN_RECEIVE> F_FS_FABRIC_RETURN_RECEIVE { get; set; }

    }
}
