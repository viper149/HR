using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.GeneralStore
{
    public class FGsItemSubCategoryViewModel
    {
        public F_GS_ITEMSUB_CATEGORY FGsItemsubCategory { get; set; }

        public List<F_GS_ITEMCATEGORY> FGsItemcategoriesList { get; set; }
    }
}
