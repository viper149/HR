using System.Collections.Generic;
using System.ComponentModel;

namespace DenimERP.Models
{
    public partial class COM_IMP_LCTYPE
    {
        public COM_IMP_LCTYPE()
        {
            COM_IMP_LCINFORMATION = new HashSet<COM_IMP_LCINFORMATION>();
        }

        public int LTID { get; set; }
        [DisplayName("LC Type")]
        public string TYPENAME { get; set; }
        public string REMARKS { get; set; }

        public virtual ICollection<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
    }
}
