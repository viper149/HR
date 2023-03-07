using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGsWastageReceiveViewModel
    {
        public FGsWastageReceiveViewModel()
        {
            FGsWastageReceiveM = new F_GS_WASTAGE_RECEIVE_M
            {
                WRDATE = DateTime.Now,
            };

            SectionList = new List<F_BAS_SECTION>();
            WasteProductList = new List<F_WASTE_PRODUCTINFO>();
            FGsWastageReceiveDList = new List<F_GS_WASTAGE_RECEIVE_D>();
        }

        public F_GS_WASTAGE_RECEIVE_M FGsWastageReceiveM { get; set; }
        public F_GS_WASTAGE_RECEIVE_D FGsWastageReceiveD { get; set; }

        public List<F_BAS_SECTION> SectionList { get; set; }
        public List<F_WASTE_PRODUCTINFO> WasteProductList { get; set; }
        public List<F_GS_WASTAGE_RECEIVE_D> FGsWastageReceiveDList { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }


    }


}
