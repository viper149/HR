using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class MKT_DEV_TYPE
    {
        public MKT_DEV_TYPE()
        {
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
        }

        public int DEV_ID { get; set; }
        public string DEV_TYPE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public int UPADTED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public int OLD_ID { get; set; }

        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
    }
}
