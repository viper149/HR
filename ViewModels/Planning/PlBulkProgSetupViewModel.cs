using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Planning
{
    public class PlBulkProgSetupViewModel
    {
        public PlBulkProgSetupViewModel()
        {
            PlBulkProgSetupDList = new List<PL_BULK_PROG_SETUP_D>();
            Yarnfors = new List<YARNFOR>();
        }

        public PL_BULK_PROG_SETUP_M PlBulkProgSetupM { get; set; }
        public PL_BULK_PROG_SETUP_D PlBulkProgSetupD { get; set; }
        public List<PL_BULK_PROG_SETUP_D> PlBulkProgSetupDList { get; set; }
        public PL_BULK_PROG_YARN_D PlBulkProgYarnD { get; set; }
        public List<COM_EX_PI_DETAILS> PiDetailsList { get; set; }
        public List<TypeTableViewModel> ProductionOrderList { get; set; }
        public List<RND_FABRICINFO> RndFabricInfos { get; set; }
        public List<RND_FABRIC_COUNTINFO> RndFabricCountInfos { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotInfos { get; set; }
        public List<YARNFOR> Yarnfors { get; set; }
    }
}
