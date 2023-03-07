using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PROCESS_BEAM_DETAILS_B : BaseEntity
    {
        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_B()
        {
            F_PR_WEAVING_PROCESS_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_B>();
            FPrWeavingProcessDetailsBList = new List<F_PR_WEAVING_PROCESS_DETAILS_B>();
            F_PR_FINISHING_BEAM_RECEIVE = new HashSet<F_PR_FINISHING_BEAM_RECEIVE>();
            F_QA_FIRST_MTR_ANALYSIS_M = new HashSet<F_QA_FIRST_MTR_ANALYSIS_M>();
        }

        public int WV_BEAMID { get; set; }
        public int? WV_PROCESSID { get; set; }
        [DisplayName("Beam No.")]
        public int? BEAMID { get; set; }
        [DisplayName("Beam No.")]
        public int? SBEAMID { get; set; }
        [DisplayName("Loom No.")]
        public int? LOOM_ID { get; set; }
        [DisplayName("Beam Length")]
        public double? BEAM_LENGTH { get; set; }
        [DisplayName("Mount Time")]
        public DateTime? MOUNT_TIME { get; set; }
        [DisplayName("Crimp(%)")]
        public double? CRIMP { get; set; }
        [DisplayName("Is Complete?")]
        public bool STATUS { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_PR_SIZING_PROCESS_ROPE_DETAILS F_PR_SIZING_PROCESS_ROPE_DETAILS { get; set; }
        public F_PR_SLASHER_DYEING_DETAILS F_PR_SLASHER_DYEING_DETAILS { get; set; }
        public F_PR_WEAVING_PROCESS_MASTER_B WV_PROCESS { get; set; }
        public F_LOOM_MACHINE_NO LoomMachine { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_B> F_PR_WEAVING_PROCESS_DETAILS_B { get; set; }
        [NotMapped]
        public List<F_PR_WEAVING_PROCESS_DETAILS_B> FPrWeavingProcessDetailsBList { get; set; }
        public ICollection<F_PR_FINISHING_BEAM_RECEIVE> F_PR_FINISHING_BEAM_RECEIVE { get; set; }
        public ICollection<F_QA_FIRST_MTR_ANALYSIS_M> F_QA_FIRST_MTR_ANALYSIS_M { get; set; }
    }
}
