using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COS_POSTCOSTING_MASTER : BaseEntity
    {
        public COS_POSTCOSTING_MASTER()
        {
            COS_POSTCOSTING_CHEMDETAILS = new HashSet<COS_POSTCOSTING_CHEMDETAILS>();
            COS_POSTCOSTING_YARNDETAILS = new HashSet<COS_POSTCOSTING_YARNDETAILS>();
        }

        [DisplayName(" Post Cost ID")]
        public int PCOSTID { get; set; }
        [Display(Name = "Production Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? TRNSDATE { get; set; }
        public int? SO_NO { get; set; }
        public string REMARKS { get; set; }
        [Display(Name="Elite Qty")]
        public double? PRODUCTION_QTY { get; set; }
        [Display(Name="Overhead")]
        public double? OVERHEAD { get; set; }
        [Display(Name="This Month Production")]
        public double? MONTH_PRODUCTION { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public RND_PRODUCTION_ORDER RndProductionOrder { get; set; }
        public ICollection<COS_POSTCOSTING_CHEMDETAILS> COS_POSTCOSTING_CHEMDETAILS { get; set; }
        public ICollection<COS_POSTCOSTING_YARNDETAILS> COS_POSTCOSTING_YARNDETAILS { get; set; }
    }
}
