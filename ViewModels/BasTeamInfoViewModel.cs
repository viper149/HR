using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class BasTeamInfoViewModel
    {
        public BAS_TEAMINFO BAS_TEAMINFO{ get; set; }
        public List<ADM_DEPARTMENT> aDM_DEPARTMENTs{ get; set; }
    }
}
