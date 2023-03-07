using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class COUNTRIES
    {
        public COUNTRIES()
        {
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
            FChemStoreReceiveMasters = new HashSet<F_CHEM_STORE_RECEIVE_MASTER>();
            FChemStoreProductinfos = new HashSet<F_CHEM_STORE_PRODUCTINFO>();
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
        }

        public int ID { get; set; }
        [Display(Name = "ISO")]
        public string ISO { get; set; }
        [Display(Name = "Origin/Country Name")]
        public string COUNTRY_NAME { get; set; }
        public string ISO3 { get; set; }
        public int COUNTRY_CODE { get; set; }
        [Display(Name = "Continent Code")]
        public string CONTINENT_CODE { get; set; }
        [Display(Name = "Continent Name")]
        public string CONTINENT_NAME { get; set; }
        [Display(Name = "Nationality")]
        public string NATIONALITY { get; set; }

        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> FChemStoreReceiveMasters { get; set; }
        public ICollection<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductinfos { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
    }
}
