using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FYarnQCAppproveSViewModel
    {
        public ICollection<F_YARN_QC_APPROVE_S> FYarnQcApproves { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_REPORT_S> FYsYarnReceiveReports { get; set; }
    }
}
