using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_DISTRICT : BaseEntity
    {
        public F_BAS_HRD_DISTRICT()
        {
            F_BAS_HRD_THANA = new HashSet<F_BAS_HRD_THANA>();
            F_HRD_EMPLOYEE_PER = new HashSet<F_HRD_EMPLOYEE>();
            F_HRD_EMPLOYEE_PRE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int DISTID { get; set; }
        public int? DIVID { get; set; }
        public string DIST_NAME { get; set; }
        public string DIST_NAME_BN { get; set; }
        public double? LATITUDE { get; set; }
        public double? LONGITUDE { get; set; }
        public string WEBSITE { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_BAS_HRD_DIVISION DIV { get; set; }

        public ICollection<F_BAS_HRD_THANA> F_BAS_HRD_THANA { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE_PER { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE_PRE { get; set; }
    }
}
