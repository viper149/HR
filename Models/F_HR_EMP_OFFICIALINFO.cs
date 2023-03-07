using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_HR_EMP_OFFICIALINFO
    {
        public int EOID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        public int? EMPID { get; set; }
        [Display(Name = "Department")]
        public int? DEPTID { get; set; }
        [Display(Name ="Section")]
        public int? SECID { get; set; }
        [Display(Name ="Sub-Section")]
        public int? SSECID { get; set; }
        [Display(Name ="Designation")]
        public int? DESID { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd MMM yyyy}"), Display(Name ="Joining Date")]
        public DateTime? JOINING_DATE { get; set; }
        [Display(Name ="Employee Type")]
        public string EMP_TYPE { get; set; }
        [Display(Name ="Official Cell No.")]
        public string OFF_CELLNO { get; set; }
        [Display(Name ="MFS Type")]
        public string MFS_TYPE { get; set; }
        [Display(Name ="MFS Details")]
        public string MFS_DETAILS { get; set; }
        [Display(Name ="Mobile Set Available")]
        public bool? MOBILE_SET_AVAIL { get; set; }
        [Display(Name ="Credit Limit")]
        public double? CREDIT_LIMIT { get; set; }
        [Display(Name ="Internet Pack")]
        public string INTERNET_PACK { get; set; }
        public string IMEI01 { get; set; }
        public string IMEI02 { get; set; }
        [Display(Name ="Option 1")]
        public string OPN1 { get; set; }
        [Display(Name ="Option 2")]
        public string OPN2 { get; set; }
        [Display(Name ="Remarks")]
        public string REMARKS { get; set; }

        public F_BAS_DEPARTMENT DEPT { get; set; }
        public F_BAS_DESIGNATION DES { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_BAS_SECTION SEC { get; set; }
        public F_BAS_SUBSECTION SSEC { get; set; }
    }
}
