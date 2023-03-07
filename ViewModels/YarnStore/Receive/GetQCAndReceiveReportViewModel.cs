using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.YarnStore.Receive
{
    public class GetQcAndReceiveReportViewModel
    {
        public ICollection<F_YARN_QC_APPROVE> FYarnQcApproves { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_REPORT> FYsYarnReceiveReports { get; set; }
    }
}
