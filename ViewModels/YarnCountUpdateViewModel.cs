using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class YarnCountUpdateViewModel
    {
        public List<COM_EX_PI_DETAILS> PiDetailsList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfosWithoutRnd { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfos { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfosJustRnd { get; set; }
        [Display(Name = "Order No.")]
        public int So_No { get; set; }
        [Display(Name = "Old Count Name")]
        public int Old_Count { get; set; }
        [Display(Name = "New Count Name")]
        public int New_Count { get; set; }
    }
}
