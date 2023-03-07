using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.GeneralStore
{
    public class FGsProductInformationViewModel
    {
        public F_GS_PRODUCT_INFORMATION FGsProductInformation { get; set; }
        public BAS_PRODUCTINFO BasProductinfo { get; set; }

        public List<F_GS_ITEMSUB_CATEGORY> FGsItemsubCategoriesList { get; set; }
        public List<F_GS_ITEMCATEGORY> FGsItemcategoriesList { get; set; }
        public List<F_BAS_UNITS> FBasUnitsList { get; set; }
    }
}
