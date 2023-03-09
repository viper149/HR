using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_UNION : BaseEntity
    {
        public F_BAS_HRD_UNION()
        {
            F_HRD_EMPLOYEE_PER = new HashSet<F_HRD_EMPLOYEE>();
            F_HRD_EMPLOYEE_PRE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int UNIONID { get; set; }
        public int? THANAID { get; set; }
        public string UNION_NAME { get; set; }
        public string UNION_NAME_BN { get; set; }
        public string WEBSITE { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_BAS_HRD_THANA THANA { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE_PER { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE_PRE { get; set; }
    }
}
