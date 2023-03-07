using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class DISTRICTS
    {
        public DISTRICTS()
        {
            UPOZILAS = new HashSet<UPOZILAS>();
            //F_HRD_EMPLOYEEPRE_DISTRICTNavigation = new HashSet<F_HRD_EMPLOYEE>();
            //F_HRD_EMPLOYEEPRE_DISTRICT_BNGNavigation = new HashSet<F_HRD_EMPLOYEE>();
            //F_HRD_EMPLOYEEPER_DISTRICTNavigation = new HashSet<F_HRD_EMPLOYEE>();
            //F_HRD_EMPLOYEEPER_DISTRICT_BNGNavigation = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int ID { get; set; }
        public int DIVISION_ID { get; set; }
        public string NAME { get; set; }
        public string BN_NAME { get; set; }
        public decimal? LAT { get; set; }
        public decimal? LON { get; set; }
        public string WEBSITE { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public DIVISIONS DIVISION_ { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPRE_DISTRICTNavigation { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPRE_DISTRICT_BNGNavigation { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPER_DISTRICTNavigation { get; set; }
        //public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEEPER_DISTRICT_BNGNavigation { get; set; }
        public ICollection<UPOZILAS> UPOZILAS { get; set; }
    }
}
