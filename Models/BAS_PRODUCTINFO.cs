using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class BAS_PRODUCTINFO
    {
        public BAS_PRODUCTINFO()
        {
            ComImpInvdetailses = new HashSet<COM_IMP_INVDETAILS>();
            COM_IMP_LCDETAILS = new HashSet<COM_IMP_LCDETAILS>();
        }

        public int PRODID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Must input a prodcut name")]
        public string PRODNAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Category")]
        public int? CATID { get; set; }
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} can not be empty")]
        public int? UNIT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public int? CSID { get; set; }
        public int? YSID { get; set; }
        public int? GSID { get; set; }

        public BAS_PRODCATEGORY CAT { get; set; }
        public F_CHEM_STORE_PRODUCTINFO CS { get; set; }
        public F_GS_PRODUCT_INFORMATION GS { get; set; }
        public BAS_YARN_COUNTINFO YS { get; set; }
        public F_BAS_UNITS UNITNavigation { get; set; }

        public ICollection<COM_IMP_INVDETAILS> ComImpInvdetailses { get; set; }
        public ICollection<COM_IMP_LCDETAILS> COM_IMP_LCDETAILS { get; set; }
    }
}
