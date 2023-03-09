using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_WEEKEND : BaseEntity
    {
        public F_BAS_HRD_WEEKEND()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int ODID { get; set; }
        public string OD_NAME { get; set; }
        public string OD_FULL_NAME { get; set; }
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
