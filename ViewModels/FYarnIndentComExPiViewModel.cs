using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FYarnIndentComExPiViewModel
    {

        public FYarnIndentComExPiViewModel()
        {
            ComExPiDetailseList = new List<COM_EX_PI_DETAILS>();
        }

        public F_YS_INDENT_DETAILS FYsIndentDetails { get; set; }

        public List<F_YS_INDENT_DETAILS> FYsIndentDetailsList { get; set; }
        public List<COM_EX_PI_DETAILS> ComExPiDetailseList { get; set; }
    }
}
