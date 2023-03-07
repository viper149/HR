using System;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_HRD_EMP_CHILDREN : BaseEntity
    {
        public int CHID { get; set; }
        public int? EMPID { get; set; }
        public string CH_NAME { get; set; }
        public string CH_NAME_BN { get; set; }
        public string CH_PROFESSION { get; set; }
        public string CH_NID { get; set; }
        public string CH_BID { get; set; }
        public string CH_PASSPORT { get; set; }
        public DateTime? CH_DOB { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
