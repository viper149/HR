using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsWastageIssueViewModel
    {
        public FFsWastageIssueViewModel()
        {
            FFsWastagePartyList = new List<F_FS_WASTAGE_PARTY>();

            FWasteProductList = new List<F_WASTE_PRODUCTINFO>();
            FFsWastageIssueDList = new List<F_FS_WASTAGE_ISSUE_D>();

            FFsWastageIssueM = new F_FS_WASTAGE_ISSUE_M
            {
                WIDATE = DateTime.Now
            };
        }

        public List<F_FS_WASTAGE_PARTY> FFsWastagePartyList { get; set; }
        public List<F_WASTE_PRODUCTINFO> FWasteProductList { get; set; }
        public List<F_FS_WASTAGE_ISSUE_D> FFsWastageIssueDList { get; set; }


        public F_FS_WASTAGE_ISSUE_M FFsWastageIssueM { get; set; }
        public F_FS_WASTAGE_ISSUE_D FFsWastageIssueD { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
