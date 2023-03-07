using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class BAS_FABRICINFO
    {
        public BAS_FABRICINFO()
        {
            BAS_STYLEINFO = new HashSet<BAS_STYLEINFO>();
        }

        public int FABID { get; set; }
        public string FABCODE { get; set; }
        public string COMPOSITION { get; set; }
        public int? COLORCODE { get; set; }
        public string WEIGHT { get; set; }
        public string WIDTH { get; set; }
        public string WEAVE { get; set; }
        public string CONSTRACTION { get; set; }
        public string SHRINKAGE { get; set; }
        public string FINISH_TYPE { get; set; }
        public string HSCODE { get; set; }
        public string TOTALENDS { get; set; }
        public string REED_COUNT { get; set; }
        public string REED_SPACE { get; set; }
        public string FINISH_ROUTE { get; set; }
        public string SHADE { get; set; }
        public string SELVEDGE { get; set; }
        public string PROGNO { get; set; }
        public string DEVNO { get; set; }
        public string EPIxPPI { get; set; }
        public string RATIO_WRP { get; set; }
        public string RATIO_WFT { get; set; }
        public string USRID { get; set; }

        public virtual BAS_COLOR COLORCODENavigation { get; set; }
        public virtual ICollection<BAS_STYLEINFO> BAS_STYLEINFO { get; set; }
    }
}
