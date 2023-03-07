using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_GS_PRODUCT_INFORMATION : BaseEntity
    {
        public F_GS_PRODUCT_INFORMATION()
        {
            F_GS_GATEPASS_INFORMATION_D = new HashSet<F_GS_GATEPASS_INFORMATION_D>();
            F_GS_RETURNABLE_GP_RCV_D = new HashSet<F_GS_RETURNABLE_GP_RCV_D>();

            F_GEN_S_INDENTDETAILS = new HashSet<F_GEN_S_INDENTDETAILS>();
            F_GEN_S_ISSUE_DETAILSADJ_PRO_AGNSTNavigation = new HashSet<F_GEN_S_ISSUE_DETAILS>();
            F_GEN_S_ISSUE_DETAILSPRODUCT = new HashSet<F_GEN_S_ISSUE_DETAILS>();
            F_GEN_S_RECEIVE_DETAILS = new HashSet<F_GEN_S_RECEIVE_DETAILS>();
            F_GEN_S_REQ_DETAILS = new HashSet<F_GEN_S_REQ_DETAILS>();
            F_GS_GATEPASS_RETURN_RCV_DETAILS = new HashSet<F_GS_GATEPASS_RETURN_RCV_DETAILS>();
            PROC_BILL_DETAILS = new HashSet<PROC_BILL_DETAILS>();
            PROC_WORKORDER_DETAILS = new HashSet<PROC_WORKORDER_DETAILS>();
            BAS_PRODUCTINFO = new HashSet<BAS_PRODUCTINFO>();
        }

        [Display(Name="Product Code")]
        public int PRODID { get; set; }
        [Display(Name = "Sub-Category")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? SCATID { get; set; }
        [Display(Name = "Product")]
        [Remote(action: "IsProdNameInUse", controller: "FGsProductInformation")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string PRODNAME { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? UNIT { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Part No")]
        public string PARTNO { get; set; }
        public int? OLDCODE { get; set; }
        [Display(Name = "Location")]
        public string PROD_LOC { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        [Display(Name = "Balance")]
        public double? Balance { get; set; }
        [NotMapped]
        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int CATID { get; set; }

        public F_GS_ITEMSUB_CATEGORY SCAT { get; set; }
        public F_BAS_UNITS UNITNavigation { get; set; }

        public ICollection<F_GS_GATEPASS_INFORMATION_D> F_GS_GATEPASS_INFORMATION_D { get; set; }
        public ICollection<F_GS_RETURNABLE_GP_RCV_D> F_GS_RETURNABLE_GP_RCV_D { get; set; }
        public ICollection<F_GEN_S_INDENTDETAILS> F_GEN_S_INDENTDETAILS { get; set; }
        public ICollection<F_GEN_S_ISSUE_DETAILS> F_GEN_S_ISSUE_DETAILSADJ_PRO_AGNSTNavigation { get; set; }
        public ICollection<F_GEN_S_ISSUE_DETAILS> F_GEN_S_ISSUE_DETAILSPRODUCT { get; set; }
        public ICollection<F_GEN_S_RECEIVE_DETAILS> F_GEN_S_RECEIVE_DETAILS { get; set; }
        public ICollection<F_GEN_S_REQ_DETAILS> F_GEN_S_REQ_DETAILS { get; set; }
        public ICollection<F_GS_GATEPASS_RETURN_RCV_DETAILS> F_GS_GATEPASS_RETURN_RCV_DETAILS { get; set; }
        public ICollection<PROC_BILL_DETAILS> PROC_BILL_DETAILS { get; set; }
        public ICollection<PROC_WORKORDER_DETAILS> PROC_WORKORDER_DETAILS { get; set; }
        public ICollection<BAS_PRODUCTINFO> BAS_PRODUCTINFO { get; set; }
    }
}
