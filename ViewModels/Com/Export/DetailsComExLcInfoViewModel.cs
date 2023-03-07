using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com.Export
{
    public class DetailsComExLcInfoViewModel
    {
        public COM_EX_LCINFO ComExLcinfo { get; set; }
        public List<COM_EX_LCDETAILS> ComExLcdetailses { get; set; }
    }
}
