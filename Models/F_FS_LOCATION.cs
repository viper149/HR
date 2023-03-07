using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_FS_LOCATION
    {
        public F_FS_LOCATION()
        {
            F_FS_FABRIC_RCV_DETAILS = new HashSet<F_FS_FABRIC_RCV_DETAILS>();
            F_PR_INSPECTION_FABRIC_D_DETAILS = new HashSet<F_PR_INSPECTION_FABRIC_D_DETAILS>();
        }

        public int ID { get; set; }
        public string LOCATION { get; set; }
        public int? LOC_CODE { get; set; }
        public string LOC_NO { get; set; }
        public string RACK_NO { get; set; }
        public int? RACK_CODE { get; set; }
        public string TYPE { get; set; }
        public string REMARKS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATETD_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        public ICollection<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
        public ICollection<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }
    }
}
