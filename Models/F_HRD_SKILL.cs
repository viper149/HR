using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_HRD_SKILL : BaseEntity
    {
        public int SKILLID { get; set; }
        public string SKILL_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int? EMPID { get; set; }
        public int? LEVELID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_HRD_SKILL_LEVEL LEVEL { get; set; }
    }
}
