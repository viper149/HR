using System;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_HRD_INCREMENT : BaseEntity
    {
        public int INCID { get; set; }
        public DateTime? INC_DATE { get; set; }
        public int? EMPID { get; set; }
        public double? OLD_SAL { get; set; }
        public double? INC_AMOUNT { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
