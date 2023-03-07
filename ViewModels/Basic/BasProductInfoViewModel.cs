using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Basic
{
    public class BasProductInfoViewModel
    {
        public BAS_PRODUCTINFO BAS_PRODUCTINFO { get; set; }
        public IEnumerable<BAS_PRODCATEGORY> BAS_PRODCATEGORies { get; set; }
    }
}
