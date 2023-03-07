using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGsWastageIssueViewModel
    {
        public FGsWastageIssueViewModel()
        {
            FGsWastagePartyList = new List<F_GS_WASTAGE_PARTY>();

            FWasteProductList = new List<F_WASTE_PRODUCTINFO>();
            FGsWastageIssueDList = new List<F_GS_WASTAGE_ISSUE_D>();

            FGsWastageIssueM = new F_GS_WASTAGE_ISSUE_M
            {
                WIDATE = DateTime.Now
            };
        }

        public List<F_GS_WASTAGE_PARTY> FGsWastagePartyList { get; set; }
        public List<F_WASTE_PRODUCTINFO> FWasteProductList { get; set; }
        public List<F_GS_WASTAGE_ISSUE_D> FGsWastageIssueDList { get; set; }


        public F_GS_WASTAGE_ISSUE_M FGsWastageIssueM { get; set; }
        public F_GS_WASTAGE_ISSUE_D FGsWastageIssueD { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
