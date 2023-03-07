using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_PROCESS_MASTER : BaseEntity
    {
        public F_PR_INSPECTION_PROCESS_MASTER()
        {
            F_PR_INSPECTION_PROCESS_DETAILS = new HashSet<F_PR_INSPECTION_PROCESS_DETAILS>();
        }

        public int INSPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Trns Date")]
        public DateTime? INSPDATE { get; set; }
        [Required]
        [DisplayName("Program No")]
        public int? SETID { get; set; }
        public int? FABCODE { get; set; }
        public int? TROLLEYNO { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public double? TotalProduction { get; set; }


        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public RND_FABRICINFO FabricInfo { get; set; }
        public F_PR_FINISHING_FNPROCESS TROLLEYNONavigation { get; set; }
        public ICollection<F_PR_INSPECTION_PROCESS_DETAILS> F_PR_INSPECTION_PROCESS_DETAILS { get; set; }
    }
}
