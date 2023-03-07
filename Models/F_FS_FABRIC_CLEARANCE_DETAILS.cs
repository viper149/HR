using System;
using System.ComponentModel.DataAnnotations;
using DevExpress.Xpo;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_CLEARANCE_DETAILS
    {
        public int CL_D_ID { get; set; }
        public int? CLID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yy}")]
        [DataType(DataType.Date)]
        [DisplayName("Prodn. Date")]
        public DateTime? PROD_DATE { get; set; }
        [DisplayName("Roll")]
        public int? ROLL_ID { get; set; }
        [DisplayName("Shade")]
        [StringLength(3, MinimumLength = 0, ErrorMessage = "Should be between 1 and 3 characters")]
        public string SHADE_GROUP { get; set; }
        [DisplayName("Shrin. Warp")]
        public string SHRINKAGE_WARP { get; set; }
        [DisplayName("Shrin. Weft")]
        public string SHRINKAGE_WEFT { get; set; }
        [DisplayName("Weight Unwash")]
        public string WGBW { get; set; }
        [DisplayName("Weight Wash")]
        public string WGAW { get; set; }
        [DisplayName("Pick Unwash")]
        public string PICK_BW { get; set; }
        [DisplayName("Pick Wash")]
        public string PICK_AW { get; set; }
        [DisplayName("Status")]
        public int? STATUS { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        [DisplayName("Insp. Remarks")]
        public string INSPECTION_REMARKS { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        public F_FS_FABRIC_CLEARANCE_MASTER CL { get; set; }
        public F_PR_INSPECTION_PROCESS_DETAILS ROLL_ { get; set; }
    }
}
