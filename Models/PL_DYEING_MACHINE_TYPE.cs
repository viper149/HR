using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public class PL_DYEING_MACHINE_TYPE : BaseEntity
    {
        public int MC_TYPE_ID { get; set; }
        public string MACHINE_NO { get; set; }
        public string PROCESS_TYPE { get; set; }
        public string WARP_TYPE { get; set; }
        public string TYPE { get; set; }
        public string REMARKS { get; set; }
    }
}
