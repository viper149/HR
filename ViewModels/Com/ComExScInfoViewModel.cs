using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComExScInfoViewModel
    {
        public COM_EX_SCINFO cOM_EX_SCINFO { get; set; }
        public COM_EX_SCDETAILS cOM_EX_SCDETAILS { get; set; }
        public List<COM_EX_FABSTYLE> cOM_EX_FABSTYLEs { get; set; }
        public List<BAS_BUYERINFO> bAS_BUYERINFOs { get; set; }
        public List<BAS_BUYER_BANK_MASTER> bAS_BUYER_BANK_MASTERs { get; set; }
        public List<COM_EX_SCDETAILS> cOM_EX_SCDETAILSList { get; set; }
        public List<COM_EX_SCDETAILS> cOM_EX_SCDETAILs { get; set; }

    }
}
