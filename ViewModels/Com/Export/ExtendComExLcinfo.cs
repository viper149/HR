using DenimERP.Models;

namespace DenimERP.ViewModels.Com.Export
{
    public class ExtendComExLcinfo : COM_EX_LCINFO
    {
        public bool DeleteOnly { get; set; }
        public bool EditOnly { get; set; }
        public bool CreateOnly { get; set; }
    }
}
