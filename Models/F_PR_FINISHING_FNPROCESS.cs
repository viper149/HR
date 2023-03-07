using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_FINISHING_FNPROCESS : BaseEntity
    {
        public F_PR_FINISHING_FNPROCESS()
        {
            F_PR_INSPECTION_PROCESS_DETAILS = new HashSet<F_PR_INSPECTION_PROCESS_DETAILS>();
            F_PR_INSPECTION_PROCESS_MASTER = new HashSet<F_PR_INSPECTION_PROCESS_MASTER>();
            RND_FABTEST_BULK = new HashSet<RND_FABTEST_BULK>();
        }
        public int FIN_PROCESSID { get; set; }
        [DisplayName("Process Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? FIN_PROCESSDATE { get; set; }
        public int? FN_PROCESSID { get; set; }
        [DisplayName("Machine Name")]
        public int? FN_MACHINEID { get; set; }
        [DisplayName("Finish Type")]
        public int? FIN_PRO_TYPEID { get; set; }
        [DisplayName("Length Receive")]
        public double? LENGTH_RCV { get; set; }
        [DisplayName("Length In")]
        public double? LENGTH_IN { get; set; }
        [DisplayName("Length Out")]
        public double? LENGTH_OUT { get; set; }
        [DisplayName("Shrinkage(%)")]
        public double? SHRINKAGE { get; set; }
        [DisplayName("Process By")]
        public int? PROCESS_BY { get; set; }
        [DisplayName("Shift")]
        public string SHIFT { get; set; }
        [DisplayName("Trolly No")]
        public int? TROLLNO { get; set; }
        [DisplayName("Grey Width")]
        public double? GREY_WIDTH { get; set; }
        [DisplayName("Finish Width")]
        public double? FINISH_WIDTH { get; set; }
        [DisplayName("Other Doff")]
        public string OTHERS_DOFF { get; set; }
        [DisplayName("Section")]
        public int? SECID { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_PR_FN_PROCESS_TYPEINFO FIN_PRO_TYPE { get; set; }
        public F_PR_FN_MACHINE_INFO FN_MACHINE { get; set; }
        public F_PR_FINISHING_PROCESS_MASTER FN_PROCESS { get; set; }
        public F_HRD_EMPLOYEE PROCESS_BYNavigation { get; set; }
        public F_BAS_SECTION SEC { get; set; }
        public F_PR_FIN_TROLLY TROLLNONavigation { get; set; }

        public ICollection<F_PR_INSPECTION_PROCESS_DETAILS> F_PR_INSPECTION_PROCESS_DETAILS { get; set; }
        public ICollection<F_PR_INSPECTION_PROCESS_MASTER> F_PR_INSPECTION_PROCESS_MASTER { get; set; }
        public ICollection<RND_FABTEST_BULK> RND_FABTEST_BULK { get; set; }
    }
}
