using System;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_HRD_PROMOTION : BaseEntity
    {
        public int PROMID { get; set; }
        public DateTime? PROM_DATE { get; set; }
        public int? EMPID { get; set; }
        public int? OLD_DEGID { get; set; }
        public int? NEW_DEGID { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_BAS_HRD_DESIGNATION NEW_DEG { get; set; }
        public F_BAS_HRD_DESIGNATION OLD_DEG { get; set; }
    }
}
