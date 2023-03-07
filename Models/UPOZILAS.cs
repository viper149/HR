using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class UPOZILAS
    {
        public UPOZILAS()
        {
            //F_HRD_EMPLOYEEPER_THANANavigation = new HashSet<F_HRD_EMPLOYEE>();
            //F_HRD_EMPLOYEEPER_THANA_BNGNavigation = new HashSet<F_HRD_EMPLOYEE>();
            //F_HRD_EMPLOYEEPRE_THANANavigation = new HashSet<F_HRD_EMPLOYEE>();
            //F_HRD_EMPLOYEEPRE_THANA_BNGNavigation = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int ID { get; set; }
        public int? DISTRICT_ID { get; set; }
        public string NAME { get; set; }
        public string BN_NAME { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public DISTRICTS DISTRICT_ { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPER_THANANavigation { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPER_THANA_BNGNavigation { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPRE_THANANavigation { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPRE_THANA_BNGNavigation { get; set; }
    }
}
