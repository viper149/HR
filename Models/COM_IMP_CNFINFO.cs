using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_IMP_CNFINFO : BaseEntity
    {
        public COM_IMP_CNFINFO()
        {
            F_CHEM_STORE_RECEIVE_MASTER = new HashSet<F_CHEM_STORE_RECEIVE_MASTER>();
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
            COM_IMP_INVOICEINFO = new HashSet<COM_IMP_INVOICEINFO>();
        }

        public int CNFID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "CNF Name")]
        [Required(ErrorMessage = "The filed {0} can not be empty.")]
        public string CNFNAME { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "CNF Person")]
        public string C_PERSON { get; set; }
        [Display(Name = "Phone Number")]
        public string CELL_NO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> F_CHEM_STORE_RECEIVE_MASTER { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
        public ICollection<COM_IMP_INVOICEINFO> COM_IMP_INVOICEINFO { get; set; }
    }
}
