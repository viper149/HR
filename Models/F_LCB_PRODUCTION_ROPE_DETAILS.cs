using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_LCB_PRODUCTION_ROPE_DETAILS : BaseEntity
    {
        public F_LCB_PRODUCTION_ROPE_DETAILS()
        {
            F_LCB_PRODUCTION_ROPE_PROCESS_INFO = new HashSet<F_LCB_PRODUCTION_ROPE_PROCESS_INFO>();
            FLcbProductionRopeProcessInfoList = new List<F_LCB_PRODUCTION_ROPE_PROCESS_INFO>();
        }

        public int LCB_D_ID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRANSDATE { get; set; }
        public int? LCBPROID { get; set; }
        [Display(Name = "Can/Tube No.")]
        public int? CANID { get; set; }
        [Display(Name = "LCB Ends")]
        public int? ENDS { get; set; }
        [Display(Name = "Shift")]
        public string SHIFT { get; set; }
        [Display(Name = "Time")]
        public int? TIME { get; set; }
        [Display(Name = "Employee Name")]
        public int? EMPLOYEEID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        
        [NotMapped]
        public List<F_LCB_PRODUCTION_ROPE_PROCESS_INFO> FLcbProductionRopeProcessInfoList { get; set; }

        public F_DYEING_PROCESS_ROPE_DETAILS CAN { get; set; } 
        public F_HRD_EMPLOYEE EMPLOYEE { get; set; }
        public F_LCB_PRODUCTION_ROPE_MASTER F_LCB_PRODUCTION_ROPE_MASTER { get; set; }

        public ICollection<F_LCB_PRODUCTION_ROPE_PROCESS_INFO> F_LCB_PRODUCTION_ROPE_PROCESS_INFO { get; set; }
    }
}
