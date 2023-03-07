using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_CHEM_STORE_PRODUCTINFO : BaseEntity
    {
        public F_CHEM_STORE_PRODUCTINFO()
        {
            F_CHEM_STORE_INDENTDETAILS = new HashSet<F_CHEM_STORE_INDENTDETAILS>();
            F_CHEM_TRANSECTION = new HashSet<F_CHEM_TRANSECTION>();
            F_DYEING_PROCESS_ROPE_CHEM = new HashSet<F_DYEING_PROCESS_ROPE_CHEM>();
            F_CHEM_REQ_DETAILS = new HashSet<F_CHEM_REQ_DETAILS>();
            F_CHEM_ISSUE_DETAILS = new HashSet<F_CHEM_ISSUE_DETAILS>();
            F_PR_SIZING_PROCESS_ROPE_CHEM = new HashSet<F_PR_SIZING_PROCESS_ROPE_CHEM>();
            F_PR_FN_CHEMICAL_CONSUMPTION = new HashSet<F_PR_FN_CHEMICAL_CONSUMPTION>();
            F_PR_SLASHER_CHEM_CONSM = new HashSet<F_PR_SLASHER_CHEM_CONSM>();
            FChemStoreReceiveDetailsesFromProductInfo = new HashSet<F_CHEM_STORE_RECEIVE_DETAILS>();
            F_CHEM_ISSUE_DETAILS_ACCLIMATIZE = new HashSet<F_CHEM_ISSUE_DETAILS>();
            CHEM_COM_IMP_INVDETAILS = new HashSet<COM_IMP_INVDETAILS>();
            RND_BOM_MATERIALS_DETAILS = new HashSet<RND_BOM_MATERIALS_DETAILS>();
            COS_POSTCOSTING_CHEMDETAILS = new HashSet<COS_POSTCOSTING_CHEMDETAILS>();
            BAS_PRODUCTINFO = new HashSet<BAS_PRODUCTINFO>();
        }

        public int PRODUCTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Chemical Name")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        [Remote(action: "IsProdNameInUse", controller: "FChemStrProductinfo")]
        public string PRODUCTNAME { get; set; }
        public string OLD_CODE { get; set; }
        public string PROD_CODE { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? UNIT { get; set; }
        [Display(Name = "Origin")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? ORIGIN { get; set; }
        [Display(Name = "Chemical Type")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? TYPE { get; set; }
        [Display(Name = "Size")]
        public string SIZE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_CHEM_TYPE TYPENAVIGATION { get; set; }
        public F_BAS_UNITS UNITNAVIGATION { get; set; }
        public COUNTRIES COUNTRIES { get; set; }

        public ICollection<F_CHEM_STORE_INDENTDETAILS> F_CHEM_STORE_INDENTDETAILS { get; set; }
        public ICollection<F_CHEM_TRANSECTION> F_CHEM_TRANSECTION { get; set; }
        public ICollection<F_DYEING_PROCESS_ROPE_CHEM> F_DYEING_PROCESS_ROPE_CHEM { get; set; }
        public ICollection<F_CHEM_REQ_DETAILS> F_CHEM_REQ_DETAILS { get; set; }
        public ICollection<F_CHEM_ISSUE_DETAILS> F_CHEM_ISSUE_DETAILS { get; set; }
        public ICollection<F_PR_SIZING_PROCESS_ROPE_CHEM> F_PR_SIZING_PROCESS_ROPE_CHEM { get; set; }
        public ICollection<F_PR_FN_CHEMICAL_CONSUMPTION> F_PR_FN_CHEMICAL_CONSUMPTION { get; set; }
        public ICollection<F_PR_SLASHER_CHEM_CONSM> F_PR_SLASHER_CHEM_CONSM { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_DETAILS> FChemStoreReceiveDetailsesFromProductInfo { get; set; }
        public ICollection<F_CHEM_ISSUE_DETAILS> F_CHEM_ISSUE_DETAILS_ACCLIMATIZE { get; set; }
        public ICollection<COM_IMP_INVDETAILS> CHEM_COM_IMP_INVDETAILS { get; set; }
        public ICollection<RND_BOM_MATERIALS_DETAILS> RND_BOM_MATERIALS_DETAILS { get; set; }
        public ICollection<COS_POSTCOSTING_CHEMDETAILS> COS_POSTCOSTING_CHEMDETAILS { get; set; }
        public ICollection<BAS_PRODUCTINFO> BAS_PRODUCTINFO { get; set; }
    }
}