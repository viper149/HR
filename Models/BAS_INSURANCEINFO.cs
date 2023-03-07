using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class BAS_INSURANCEINFO
    {
        public BAS_INSURANCEINFO()
        {
            COM_IMP_LCINFORMATION = new HashSet<COM_IMP_LCINFORMATION>();
        }

        public int INSID { get; set; }
        public string INSNAME { get; set; }
        public string ADDRESS { get; set; }
        public string CPERSON { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string REMARKS { get; set; }

        public virtual ICollection<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
    }
}
