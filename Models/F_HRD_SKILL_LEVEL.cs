using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class F_HRD_SKILL_LEVEL
    {
        public F_HRD_SKILL_LEVEL()
        {
            F_HRD_SKILL = new HashSet<F_HRD_SKILL>();
        }

        public int LEVELID { get; set; }
        public string LEVEL_NAME { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_HRD_SKILL> F_HRD_SKILL { get; set; }
    }
}