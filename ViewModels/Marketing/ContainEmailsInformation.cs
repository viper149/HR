using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Marketing
{
    public class ContainEmailsInformation
    {
        public ContainEmailsInformation()
        {
            ToEmailObj = new List<MKT_TEAM>();
            CcEmailObj = new List<MKT_TEAM>();
            BccEmailObj = new List<MKT_TEAM>();
        }

        public string BaseUrl { get; set; }
        public List<MKT_TEAM> ToEmailObj { get; set; }
        public List<MKT_TEAM> CcEmailObj { get; set; }
        public List<MKT_TEAM> BccEmailObj { get; set; }
    }
}
