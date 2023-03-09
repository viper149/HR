using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class COUNTRIES
    {
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
    }
}
