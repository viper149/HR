using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_EMP_RELATION : BaseEntity
    {
        public F_BAS_HRD_EMP_RELATION()
        {
            F_HRD_EMERGENCY = new HashSet<F_HRD_EMERGENCY>();
        }

        public int RELATIONID { get; set; }
        public string REL_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMERGENCY> F_HRD_EMERGENCY { get; set; }
    }
}
