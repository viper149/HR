using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComImpCsInfoViewModel
    {
        public ComImpCsInfoViewModel()
        {
            ComImpCsItemDetailsList = new List<COM_IMP_CSITEM_DETAILS>();
        }

        public COM_IMP_CSINFO ComImpCsInfo { get; set; }
        public COM_IMP_CSITEM_DETAILS ComImpCsItemDetails { get; set; }
        public List<COM_IMP_CSITEM_DETAILS> ComImpCsItemDetailsList { get; set; }
        public COM_IMP_CSRAT_DETAILS ComImpCsRatDetails { get; set; }
        public List<BAS_PRODUCTINFO> ProductInfos { get; set; }
        public List<BAS_SUPPLIERINFO> SupplierInfos { get; set; }
        public List<F_YS_INDENT_MASTER> YsIndentMasters { get; set; }
    }
}
