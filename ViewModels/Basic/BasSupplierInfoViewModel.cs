using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Basic
{
    public class BasSupplierInfoViewModel
    {
        public BAS_SUPPLIERINFO BAS_SUPPLIERINFO { get; set; }
        public IEnumerable<BAS_SUPP_CATEGORY> bAS_SUPP_CATEGORies { get; set; }
    }
}
