using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FBasAssetListViewModel
    {
        public FBasAssetListViewModel()
        {
            SectionList = new List<F_BAS_SECTION>();
        }

        public List<F_BAS_SECTION> SectionList { get; set; }
        public F_BAS_ASSET_LIST FBasAssetList { get; set; }
       

    }

   
}
