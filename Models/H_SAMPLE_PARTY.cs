using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_PARTY : BaseEntity
    {
        public int HSPID { get; set; }
        [Display(Name = "Party Name")]
        public string HSPNAME { get; set; }
        [Display(Name = "Designation")]
        public string HSPDISIGNATION { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Phone Number")]
        public string PHONE { get; set; }
        [Display(Name = "Email")]
        public string EMAIL { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}
