using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_HRD_EMERGENCY : BaseEntity
    {
        public int CONTID { get; set; }
        public string CONT_NAME { get; set; }
        public int? EMPID { get; set; }
        public int? RELATIONID { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string ADDRESS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_BAS_HRD_EMP_RELATION RELATION { get; set; }
    }
}
