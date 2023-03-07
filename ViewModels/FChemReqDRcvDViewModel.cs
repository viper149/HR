using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FChemReqDRcvDViewModel
    {
        public FChemReqDRcvDViewModel()
        {
            FChemReqDetailsList = new List<F_CHEM_REQ_DETAILS>();
            FChemStoreReceiveDetailsList = new List<F_CHEM_STORE_RECEIVE_DETAILS>();
        }

        public List<F_CHEM_REQ_DETAILS> FChemReqDetailsList { get; set; }
        public F_CHEM_REQ_DETAILS FChemReqDetails { get; set; }
        public List<F_CHEM_STORE_RECEIVE_DETAILS> FChemStoreReceiveDetailsList { get; set; }
        public F_CHEM_STORE_RECEIVE_DETAILS FChemStoreReceiveDetails { get; set; }
    }
}
