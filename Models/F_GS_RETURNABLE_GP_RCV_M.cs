using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_GS_RETURNABLE_GP_RCV_M
    {
        public F_GS_RETURNABLE_GP_RCV_M()
        {
            F_GS_RETURNABLE_GP_RCV_D = new HashSet<F_GS_RETURNABLE_GP_RCV_D>();
        }

        public int RCVID { get; set; }
        public DateTime? RCVDATE { get; set; }
        public int? GPID { get; set; }
        public int? RCVD_BY { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_GS_GATEPASS_INFORMATION_M GP { get; set; }
        public F_HRD_EMPLOYEE RCVD_BYNavigation { get; set; }
        public ICollection<F_GS_RETURNABLE_GP_RCV_D> F_GS_RETURNABLE_GP_RCV_D { get; set; }
    }
}
