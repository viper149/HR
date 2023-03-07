using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_DEFECT_POINT
    {
        public int DPID { get; set; }
        public int? ROLL_ID { get; set; }
        [DisplayName("Defect Point")]
        public int? DEF_TYPEID { get; set; }
        [DisplayName("Point 1")]
        public int? POINT1 { get; set; }
        [DisplayName("Point 2")]
        public int? POINT2 { get; set; }
        [DisplayName("Point 3")]
        public int? POINT3 { get; set; }
        [DisplayName("Point 4")]
        public int? POINT4 { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        [NotMapped]
        public int? RollFId { get; set; }
        [NotMapped]
        public int? RollFCId { get; set; }
        [NotMapped]
        public int? StyleName { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]

        public DateTime? FindDate { get; set; }

        public F_PR_INSPECTION_DEFECTINFO DEF_TYPE { get; set; }
        public F_PR_INSPECTION_PROCESS_DETAILS ROLL_ { get; set; }
    }
}
