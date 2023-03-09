using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_DIVISION : BaseEntity
    {
        public F_BAS_HRD_DIVISION()
        {
            F_BAS_HRD_DISTRICT = new HashSet<F_BAS_HRD_DISTRICT>();
            F_HRD_EMPLOYEE_PER = new HashSet<F_HRD_EMPLOYEE>();
            F_HRD_EMPLOYEE_PRE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int DIVID { get; set; }
        public int? COUNTRYID { get; set; }
        public string DIV_NAME { get; set; }
        public string DIV_NAME_BN { get; set; }
        public string WEBSITE { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_BAS_HRD_NATIONALITY COUNTRY { get; set; }

        public ICollection<F_BAS_HRD_DISTRICT> F_BAS_HRD_DISTRICT { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE_PER { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE_PRE { get; set; }
    }
}
