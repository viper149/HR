using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class BAS_YARN_PARTNO : BaseEntity
    {
        public BAS_YARN_PARTNO()
        {
            BAS_YARN_COUNTINFO = new HashSet<BAS_YARN_COUNTINFO>();
        }

        public int PART_ID { get; set; }
        [Display(Name = "Part Number")]
        public string PART_NO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<BAS_YARN_COUNTINFO> BAS_YARN_COUNTINFO { get; set; }
    }
}
