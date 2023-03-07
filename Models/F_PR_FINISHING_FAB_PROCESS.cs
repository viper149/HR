using System;

namespace DenimERP.Models
{
    public partial class F_PR_FINISHING_FAB_PROCESS
    {
        public int FAB_PROCESSID { get; set; }
        public DateTime? FAB_PROCESSDATE { get; set; }
        public int? FN_PROCESSID { get; set; }
        public int? FAB_MACHINEID { get; set; }
        public int? FAB_PRO_TYPEID { get; set; }
        public double? LENGTH_RCV { get; set; }
        public double? LENGTH_ACT { get; set; }
        public double? LENGTH_PROCESS { get; set; }
        public double? LENGTH_DIFF { get; set; }
        public int? PROCESS_BY { get; set; }
        public string SHIFT { get; set; }
        public int? TROLLYNO { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_PR_PROCESS_MACHINEINFO FAB_MACHINE { get; set; }
        public F_PR_PROCESS_TYPE_INFO FAB_PRO_TYPE { get; set; }
        //public F_PR_FINISHING_PROCESS_MASTER FN_PROCESS { get; set; }
        public F_HRD_EMPLOYEE PROCESS_BYNavigation { get; set; }
        public F_PR_FIN_TROLLY TROLLYNONavigation { get; set; }
    }
}
