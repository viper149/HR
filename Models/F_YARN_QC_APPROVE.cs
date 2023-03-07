using System;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YARN_QC_APPROVE : BaseEntity
    {
        public int YQCA { get; set; }
        public int? YRDID { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime? YQCADATE { get; set; }

        public F_YS_YARN_RECEIVE_DETAILS TRNS { get; set; }
    }
}
