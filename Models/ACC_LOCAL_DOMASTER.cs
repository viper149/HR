using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class ACC_LOCAL_DOMASTER
    {
        public ACC_LOCAL_DOMASTER()
        {
            ACC_LOCAL_DODETAILS = new HashSet<ACC_LOCAL_DODETAILS>();
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "Is Compensation")]
        public bool IS_COMPENSATION { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime TRNSDATE { get; set; }
        [Remote(action: "IsDoNoInUse", controller: "AccLocalDoMaster")]
        [Display(Name = "DO No")]
        public string DONO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "DO Date")]
        public DateTime? DODATE { get; set; }
        [Display(Name = "LC/Order No")]
        public int? SCID { get; set; }
        [Display(Name = "LC Id")]
        public int? LCID { get; set; }
        [NotMapped]
        [Display(Name = "LC/Order No")]
        public string SCNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "DO Expire Date")]
        public DateTime? DOEX { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Audit By")]
        public string AUDITBY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Audition")]
        public DateTime? AUDITON { get; set; }
        [Display(Name = "Comments")]
        public string COMMENTS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }
        
        public COM_EX_LCINFO LC { get; set; }
        public COM_EX_SCINFO ComExScinfo { get; set; }

        public ICollection<ACC_LOCAL_DODETAILS> ACC_LOCAL_DODETAILS { get; set; }
        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
    }
}
