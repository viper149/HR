using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_DEFECTINFO
    {
        public F_PR_INSPECTION_DEFECTINFO()
        {
            F_PR_INSPECTION_DEFECT_POINT = new HashSet<F_PR_INSPECTION_DEFECT_POINT>();
            F_PR_INSPECTION_REJECTION_B = new HashSet<F_PR_INSPECTION_REJECTION_B>();
        }

        public int DEF_TYPEID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CODE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_PR_INSPECTION_DEFECT_POINT> F_PR_INSPECTION_DEFECT_POINT { get; set; }
        public ICollection<F_PR_INSPECTION_REJECTION_B> F_PR_INSPECTION_REJECTION_B { get; set; }
    }
}
