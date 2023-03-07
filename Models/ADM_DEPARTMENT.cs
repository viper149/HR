using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class ADM_DEPARTMENT
    {
        public ADM_DEPARTMENT()
        {
            ADM_DESIGNATION = new HashSet<ADM_DESIGNATION>();
            BAS_TEAMINFO = new HashSet<BAS_TEAMINFO>();
        }

        public int DEPTID { get; set; }

        public string DEPTNAME { get; set; }
        public string REMARKS { get; set; }

        public ICollection<ADM_DESIGNATION> ADM_DESIGNATION { get; set; }
        public ICollection<BAS_TEAMINFO> BAS_TEAMINFO { get; set; }
    }
}
