using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory
{
    public class FBasSubSectionViewModel
    {
        public F_BAS_SUBSECTION FBasSubSection { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
    }
}
