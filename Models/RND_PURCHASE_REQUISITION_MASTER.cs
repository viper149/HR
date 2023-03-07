using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_PURCHASE_REQUISITION_MASTER : BaseEntity
    {
        public RND_PURCHASE_REQUISITION_MASTER()
        {
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
            F_YS_INDENT_MASTER = new HashSet<F_YS_INDENT_MASTER>();
        }

        public int INDSLID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Indent Date")]
        public DateTime? INDSLDATE { get; set; }
        [Display(Name = "Responsible Employee")]
        public int? RESEMPID { get; set; }
        [Display(Name = "Ref. Person")]
        public int? EMPID { get; set; }
        [Display(Name = "Indent Sl. No(Khata)")]
        public string INDENT_SL_NO { get; set; }
        [Display(Name = "Yarn For")]
        public string YARN_FOR { get; set; }
        [Display(Name = "Order No. (Bulk)")]
        public int? ORDER_NO { get; set; }
        [Display(Name = "Order No. (Sample)")]
        public int? ORDERNO_S { get; set; }
        [Display(Name = "Buyer Name")]
        public int? BUYERID { get; set; }
        [Display(Name = "Fabric Code")]
        public int? FABCODE { get; set; }
        [Display(Name = "Sample Length (Yds)")]
        public double? SAMPLE_L { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [Display(Name = "Revise No.")]
        public string REVISE_NO { get; set; }
        [Display(Name = "Revise Date")]
        [DataType(DataType.Date)]
        public DateTime? REVISE_DATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Requisition No")]
        public string INDSLNO { get; set; }
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [Display(Name = "Cost Ref.")]
        public int? COSTREFID { get; set; }
        public bool IS_REVISED { get; set; }
        [NotMapped]
        public bool FLAG { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_HRD_EMPLOYEE RESEMP { get; set; }
        public COS_PRECOSTING_MASTER COSTREF { get; set; }
        public MKT_SDRF_INFO ORDERNO_SNavigation { get; set; }
        public RND_PRODUCTION_ORDER ORDER_NONavigation { get; set; }
        public BAS_BUYERINFO BasBuyerinfo { get; set; }
        public RND_FABRICINFO RndFabricinfo { get; set; }

        public ICollection<F_YS_INDENT_MASTER> F_YS_INDENT_MASTER { get; set; }
        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
    }
}
