using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_RELIGION : BaseEntity
    {
        public F_BAS_HRD_RELIGION()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int RELIGIONID { get; set; }
        public string RELEGION_NAME { get; set; }
        public string RELEGION_NAME_BNG { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
