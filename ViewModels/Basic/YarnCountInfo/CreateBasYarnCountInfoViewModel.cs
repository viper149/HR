using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Basic.YarnCountInfo
{
    public class CreateBasYarnCountInfoViewModel
    {
        public CreateBasYarnCountInfoViewModel()
        {
            BasYarnCountLotInfoList = new List<BAS_YARN_COUNT_LOT_INFO>();
        }

        public ExtendBasYarnCountInfo BasYarnCountinfo { get; set; }
        public BAS_YARN_COUNT_LOT_INFO BasYarnCountLotInfo { get; set; }
        public BAS_PRODUCTINFO BasProductinfo { get; set; }

        public List<BAS_YARN_COUNT_LOT_INFO> BasYarnCountLotInfoList { get; set; }
        public List<BAS_YARN_CATEGORY> BasYarnCategories { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotInfos { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
        public List<BAS_YARN_PARTNO> BasYarnPartNo { get; set; }
    }
}
