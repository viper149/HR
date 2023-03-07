using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_CHEM_STORE_INDENT_TYPE
    {
        public F_CHEM_STORE_INDENT_TYPE()
        {
            FChemStoreIndentmasters = new HashSet<F_CHEM_STORE_INDENTMASTER>();
        }

        [Display(Name = "Indent Type Number")]
        public int INDENTID { get; set; }
        [Display(Name = "Indent Type")]
        public string INDENTTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_CHEM_STORE_INDENTMASTER> FChemStoreIndentmasters { get; set; }
    }
}
