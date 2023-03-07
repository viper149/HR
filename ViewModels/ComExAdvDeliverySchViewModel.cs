using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ComExAdvDeliverySchViewModel
    {
        public ComExAdvDeliverySchViewModel()
        {
            ComExAdvDeliverySchMaster = new COM_EX_ADV_DELIVERY_SCH_MASTER
            {
                DSDATE = DateTime.Now
            };

            BasBuyerList = new List<BAS_BUYERINFO>();
            ComExPimasterList = new List<COM_EX_PIMASTER>();
            ComExAdvDeliverySchDetailsList = new List<COM_EX_ADV_DELIVERY_SCH_DETAILS>();
        }

        public COM_EX_ADV_DELIVERY_SCH_MASTER ComExAdvDeliverySchMaster { get; set; }
        public COM_EX_ADV_DELIVERY_SCH_DETAILS ComExAdvDeliverySchDetails { get; set; }

        public List<BAS_BUYERINFO> BasBuyerList { get; set; }
        public List<COM_EX_PIMASTER> ComExPimasterList { get; set; }
        public List<COM_EX_ADV_DELIVERY_SCH_DETAILS> ComExAdvDeliverySchDetailsList { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
