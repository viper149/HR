using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class COM_IMP_DEL_STATUS
    {
        public COM_IMP_DEL_STATUS()
        {
            COM_IMP_INVOICEINFO = new HashSet<COM_IMP_INVOICEINFO>();
        }
        public int ID { get; set; }
        [Display(Name = "Delivery Status Type")]
        public string NAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public virtual ICollection<COM_IMP_INVOICEINFO> COM_IMP_INVOICEINFO { get; set; }
    }
}
