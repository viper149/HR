using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class BAS_GENDER
    {
        public BAS_GENDER()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int GENID { get; set; }
        public string GENNAME { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
