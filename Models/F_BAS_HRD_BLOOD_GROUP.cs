using System.Collections.Generic;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_BLOOD_GROUP
    {
        public F_BAS_HRD_BLOOD_GROUP()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int BLDGRPID { get; set; }
        public string BLDGRP_NAME { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
