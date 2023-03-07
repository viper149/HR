using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_YARN_CATEGORY : BaseEntity
    {
        public BAS_YARN_CATEGORY()
        {
            BAS_YARN_COUNTINFO = new HashSet<BAS_YARN_COUNTINFO>();
        }

        public int YARN_CAT_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }

        [Display(Name = "Yarn Code")]
        [Remote("FindByYarnCode", "BasicYarnCategory")]
        public int? YARN_CODE { get; set; }

        [Display(Name = "Yarn Category Name")]
        [Remote("FindByYarnCategoryName", "BasicYarnCategory")]
        public string CATEGORY_NAME { get; set; }

        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<BAS_YARN_COUNTINFO> BAS_YARN_COUNTINFO { get; set; }
    }
}
