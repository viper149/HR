using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_DYEING_PROCESS_ROPE_DETAILS
    {
        public F_DYEING_PROCESS_ROPE_DETAILS()
        {
            F_LCB_PRODUCTION_ROPE_DETAILS = new HashSet<F_LCB_PRODUCTION_ROPE_DETAILS>();
        }
        public int ROPEID { get; set; }
        [Display(Name = "Program/Set No.")]
        public int? SUBGROUPID { get; set; }
        public int? ROPE_DID { get; set; }
        [Display(Name = "Ball No.")]
        public int? BALLID { get; set; }
        [Display(Name = "Rope No.")]
        public int? ROPE_NO { get; set; }
        [Display(Name = "Rope Machine No.")]
        public int? R_MACHINE_NO { get; set; }
        [Display(Name = "Can No.")]
        public int? CAN_NO { get; set; }
        [Display(Name = "Shift")]
        public string SHIFT { get; set; }
        [Display(Name = "Dyeing Length")]
        public double? DYEING_LENGTH { get; set; }
        [Display(Name = "Rejection")]
        public double? REJECTION { get; set; }
        [Display(Name = "Stop Mark")]
        public double? STOP_MARK { get; set; }
        [Display(Name = "Ball Length")]
        public double? BALL_LENGTH { get; set; }

        [Display(Name = "Is Set Close?")]
        public bool CLOSE_STATUS { get; set; }

        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        public F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS BALL { get; set; }
        //public PL_PRODUCTION_SETDISTRIBUTION SETNO { get; set; }
        public PL_PRODUCTION_PLAN_DETAILS SUBGROUP { get; set; }
        public F_DYEING_PROCESS_ROPE_MASTER ROPE_D { get; set; }
        public F_PR_TUBE_INFO CAN_NONavigation { get; set; }
        public F_PR_ROPE_INFO ROPE_NONavigation { get; set; }
        public F_PR_ROPE_MACHINE_INFO R_MACHINE_NONavigation { get; set; }
        public ICollection<F_LCB_PRODUCTION_ROPE_DETAILS> F_LCB_PRODUCTION_ROPE_DETAILS { get; set; }
    }
}
