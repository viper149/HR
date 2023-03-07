using DenimERP.Models;

namespace DenimERP.ViewModels.Com.Export
{
    public class ExtendComExPiMaster : COM_EX_PIMASTER
    {
        public bool ReadOnly { get; set; }
        public bool DeleteOnly { get; set; }
        public bool EditOnly { get; set; }
        public bool CreateOnly { get; set; }
    }
}
