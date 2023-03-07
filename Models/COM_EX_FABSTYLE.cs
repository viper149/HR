using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_EX_FABSTYLE: BaseEntity
    {
        public COM_EX_FABSTYLE()
        {
            ACC_EXPORT_DODETAILS = new HashSet<ACC_EXPORT_DODETAILS>();
            COM_EX_PI_DETAILS = new HashSet<COM_EX_PI_DETAILS>();
            COM_EX_SCDETAILS = new HashSet<COM_EX_SCDETAILS>();
            ComExInvdetailses = new HashSet<COM_EX_INVDETAILS>();
        }

        [Display(Name = "Style Id")]
        public int STYLEID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Style")]
        public string STYLENAME { get; set; }
        [Display(Name = "Brand Name")]
        public int BRANDID { get; set; }
        [Display(Name = "Fabric Code")]
        public int FABCODE { get; set; }
        [Display(Name = "Comm. Constration")]
        public string COMM_CONST { get; set; }
        [Display(Name = "RND Constration")]
        public string RND_CONST { get; set; }
        [Display(Name = "Com. Composition")]
        public string COM_FABRIC_COMPOSITION { get; set; }
        [Display(Name = "H.S Code")]
        public string HSCODE { get; set; }
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [Display(Name = "BCI(%)")]
        public string BCI { get; set; }
        [Display(Name = "Organic GOTS(%)")]
        public string ORG_GOTS { get; set; }
        [Display(Name = "Organic OCS(%)")]
        public string ORG_OCS { get; set; }
        [Display(Name = "RCS(%)")]
        public string RCS { get; set; }
        [Display(Name = "GRS(%)")]
        public string GRS { get; set; }
        [Display(Name = "CMIA(%)")]
        public string CMIA { get; set; }
        [Display(Name = "Option 1")]
        public string OPTION1 { get; set; }
        [Display(Name = "Option 2")]
        public string OPTION2 { get; set; }
        [Display(Name = "Option 2")]
        public string OPTION3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }

        public BAS_BRANDINFO BRAND { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }

        public ICollection<COM_EX_PI_DETAILS> COM_EX_PI_DETAILS { get; set; }
        public ICollection<COM_EX_SCDETAILS> COM_EX_SCDETAILS { get; set; }
        public ICollection<COM_EX_INVDETAILS> ComExInvdetailses { get; set; }
        public ICollection<ACC_EXPORT_DODETAILS> ACC_EXPORT_DODETAILS { get; set; }
    }
}
