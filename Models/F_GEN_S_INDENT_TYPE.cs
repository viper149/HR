using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_GEN_S_INDENT_TYPE
    {
        public F_GEN_S_INDENT_TYPE()
        {
            F_GEN_S_INDENTMASTER = new HashSet<F_GEN_S_INDENTMASTER>();
        }

        public int INDENTID { get; set; }
        public string INDENTTYPE { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_GEN_S_INDENTMASTER> F_GEN_S_INDENTMASTER { get; set; }
    }
}
