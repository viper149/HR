using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_HR_BLOOD_GROUP
    {
        public F_HR_BLOOD_GROUP()
        {
            //F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int BGID { get; set; }
        public string BLOOD_GROUP { get; set; }
        public string REMARKS { get; set; }

        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
