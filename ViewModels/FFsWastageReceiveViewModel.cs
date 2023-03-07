using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{

    public class FFsWastageReceiveViewModel
    {
        public FFsWastageReceiveViewModel()
        {
            FFsWastageReceiveM = new F_FS_WASTAGE_RECEIVE_M
            {
                WRDATE = DateTime.Now,
            };

            SectionList = new List<F_BAS_SECTION>();
            WasteProductList = new List<F_WASTE_PRODUCTINFO>();
            FFsWastageReceiveDList = new List<F_FS_WASTAGE_RECEIVE_D>();
        }

        public F_FS_WASTAGE_RECEIVE_M FFsWastageReceiveM { get; set; }
        public F_FS_WASTAGE_RECEIVE_D FFsWastageReceiveD { get; set; }

        public List<F_BAS_SECTION> SectionList { get; set; }
        public List<F_WASTE_PRODUCTINFO> WasteProductList { get; set; }
        public List<F_FS_WASTAGE_RECEIVE_D> FFsWastageReceiveDList { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }


    }
}
