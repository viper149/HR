using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class BAS_TRANSPORTINFO
    {
        public BAS_TRANSPORTINFO()
        {
            COM_IMP_INVOICEINFO = new HashSet<COM_IMP_INVOICEINFO>();
            FChemStoreReceiveMasters = new List<F_CHEM_STORE_RECEIVE_MASTER>();
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
        }

        public int TRNSPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Transport Name")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public string TRNSPNAME { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Person Name")]
        public string CPERSON { get; set; }
        [Display(Name = "Phone")]
        public string PHONE { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string EMAIL { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<COM_IMP_INVOICEINFO> COM_IMP_INVOICEINFO { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> FChemStoreReceiveMasters { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
    }
}
