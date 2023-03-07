using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ComExPIMasterViewModel
    {
        public ComExPIMasterViewModel()
        {
            cOM_EX_PI_DETAILs = new List<COM_EX_PI_DETAILS>();
            Exportstatuses = new List<EXPORTSTATUS>();
            BasSeasonList = new List<BAS_SEASON>();
            FBasUnitses = new List<F_BAS_UNITS>();
            Currencies = new List<CURRENCY>();
            NET_WEIGHT = new double();
            GROSS_WEIGHT = new double();
        }

        public COM_EX_PIMASTER cOM_EX_PIMASTER { get; set; }
        public COM_EX_PI_DETAILS cOM_EX_PI_DETAILS { get; set; }
        public COM_EX_FABSTYLE ComExFabStyle { get; set; }

        public BAS_SEASON bAS_SEASON { get; set; }

        public List<BAS_SEASON> BasSeasonList { get; set; }


        public List<COM_EX_PI_DETAILS> cOM_EX_PI_DETAILs { get; set; }
        public List<BAS_BUYERINFO> bAS_BUYERINFOs { get; set; }
        public List<RND_FABRICINFO> rND_FABRICINFOs { get; set; }
        public List<COM_EX_FABSTYLE> cOM_EX_FABSTYLEs { get; set; }
        public List<BAS_BUYER_BANK_MASTER> bAS_BUYER_BANK_MASTERs { get; set; }
        public List<BAS_COLOR> bAS_COLORs{ get; set; }
        public List<BAS_TEAMINFO> bAS_TEAMINFOs { get; set; }
        public List<MKT_TEAM> MktTeams { get; set; }
        public List<BAS_BEN_BANK_MASTER> bAS_BEN_BANK_MASTERs { get; set; }
        public List<BAS_BRANDINFO> bAS_BRANDINFOs { get; set; }
        public List<COM_TENOR> ComTenors { get; set; }
        public List<COS_PRECOSTING_MASTER> CosPreCostingMasters { get; set; }
        public List<EXPORTSTATUS> Exportstatuses { get; set; }
        public List<F_BAS_UNITS> FBasUnitses { get; set; }
        public List<CURRENCY> Currencies { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
        public double? NET_WEIGHT { get; set; }
        public double? GROSS_WEIGHT { get; set; }
    }
}
