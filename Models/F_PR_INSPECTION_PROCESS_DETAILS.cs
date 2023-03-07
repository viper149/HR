using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_PROCESS_DETAILS : BaseEntity
    {
        public F_PR_INSPECTION_PROCESS_DETAILS()
        {
            F_PR_INSPECTION_DEFECT_POINT = new HashSet<F_PR_INSPECTION_DEFECT_POINT>();
            FPrInspectionDefectPointsList = new List<F_PR_INSPECTION_DEFECT_POINT>();
            F_FS_FABRIC_RCV_DETAILS = new HashSet<F_FS_FABRIC_RCV_DETAILS>();
            F_FS_FABRIC_CLEARANCE_DETAILS = new HashSet<F_FS_FABRIC_CLEARANCE_DETAILS>();
            F_FS_CLEARANCE_MASTER_SAMPLE_ROLL = new HashSet<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>();
            F_PR_INSPECTION_FABRIC_D_DETAILS = new HashSet<F_PR_INSPECTION_FABRIC_D_DETAILS>();
        }

        public int ROLL_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? INSPID { get; set; }
        [Display(Name = "Roll Ins. Date")]
        [DataType(DataType.Date)]
        public DateTime? ROLL_INSPDATE { get; set; }
        [Display(Name = "Trolley")]
        public int? TROLLEYNO { get; set; }
        [Display(Name = "Is Complete?")]
        public bool TROLLY_STATUS { get; set; }
        [Display(Name = "Operator")]
        public int? OPERATOR_ID { get; set; }
        [Display(Name = "Machine")]
        public int? MACHINE_ID { get; set; }
        [Display(Name = "Batch No")]
        public string BATCH { get; set; }
        [Display(Name = "Roll No")]
        //[Remote(action: "IsRollNoInUse", controller: "FPrInspectionProcess")]
        public string ROLLNO { get; set; }
        [Display(Name = "Length 1")]
        public double? LENGTH_1 { get; set; }
        [Display(Name = "Length 2")]
        public double? LENGTH_2 { get; set; }
        [Display(Name = "Act. Const.")]
        public string ACT_CONS { get; set; }
        [Display(Name = "Shift")]
        public string SHIFT { get; set; }
        [Display(Name = "No of Pics")]
        public int? PICES { get; set; }
        [Display(Name = "Total Defect Point")]
        public int? TOTAL_DEFECT { get; set; }
        [Display(Name = "Point/100sq")]
        public double? POINT_100SQ { get; set; }
        [Display(Name = "Meter")]
        public double? LENGTH_MTR { get; set; }
        [Display(Name = "Total Yds")]
        public double? LENGTH_YDS { get; set; }
        [Display(Name = "Act. Width")]
        public double? ACT_WIDTH_INCH { get; set; }
        [Display(Name = "Cut. Width")]
        public double? CUT_WIDTH_INCH { get; set; }
        [Display(Name = "Fabric + Tube(Kg)")]
        public double? GR_WEIGHT_KG { get; set; }
        [Display(Name = "Weight Deduct")]
        public double? WEIGHT_DEDUCT { get; set; }
        [Display(Name = "Net Wt.(Kg)")]
        public double? NET_WEIGHT_KG { get; set; }
        [Display(Name = "Fabric Grade")]
        public string FAB_GRADE { get; set; }
        [Display(Name = "Act. Oz/Yds")]
        public double? ACT_OZ_YDS { get; set; }
        [Display(Name = "Cut Pcs(Yds)")]
        public double? CUTPCS_YDS { get; set; }
        [Display(Name = "Defect Name")]
        public int? DEFECT_NAME { get; set; }
        [Display(Name = "Defect Fabric Pics")]
        public string DEF_PCS { get; set; }
        [DisplayName("Process Type")]
        public int? PROCESS_TYPE { get; set; }
        [Display(Name = "Fault Status")]
        public string DEFECT_FAULT_STATUS { get; set; }
        [Display(Name = "Cut Pcs Section")]
        public int? CUT_PCS_SECTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Gross Weight(kg)")]
        public string OPT1 { get; set; }
        [Display(Name = "Paper Tube(gm)")]
        public string OPT2 { get; set; }
        [Display(Name = "Poly(gm)")]
        public string OPT3 { get; set; }
        [DisplayName("Sample Style")]
        public string OPT4 { get; set; }

        //public F_PR_INSPECTION_BATCH BATCHNavigation { get; set; }
        public F_BAS_SECTION CUT_PCS_SECTIONNavigation { get; set; }
        public F_PR_INSPECTION_PROCESS_MASTER INSP { get; set; }
        public F_PR_INSPECTION_MACHINE MACHINE_ { get; set; }
        public F_PR_FINISHING_FNPROCESS TROLLEYNONavigation { get; set; }
        public F_PR_INSPECTION_PROCESS PROCESS_TYPENavigation { get; set; }
        public F_HRD_EMPLOYEE Operator { get; set; }

        [NotMapped]
        public List<F_PR_INSPECTION_DEFECT_POINT> FPrInspectionDefectPointsList { get; set; }

        public ICollection<F_PR_INSPECTION_DEFECT_POINT> F_PR_INSPECTION_DEFECT_POINT { get; set; }
        public ICollection<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
        public ICollection<F_FS_FABRIC_CLEARANCE_DETAILS> F_FS_FABRIC_CLEARANCE_DETAILS { get; set; }
        public ICollection<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL> F_FS_CLEARANCE_MASTER_SAMPLE_ROLL { get; set; }
          
        public ICollection<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }

    }
}
