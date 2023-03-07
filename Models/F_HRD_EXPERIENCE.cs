using System;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_HRD_EXPERIENCE : BaseEntity
    {
        public int EXPID { get; set; }
        public int? EMPID { get; set; }
        public string COMPANY { get; set; }
        public string DESIGNATION { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
