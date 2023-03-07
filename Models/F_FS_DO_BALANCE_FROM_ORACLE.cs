using System;

namespace DenimERP.Models
{
    public partial class F_FS_DO_BALANCE_FROM_ORACLE
    {
        public int TRNSID { get; set; }
        public string BUYER_NAME { get; set; }
        public string LC_NO { get; set; }
        public DateTime? DO_DATE { get; set; }
        public DateTime? LC_DATE { get; set; }
        public string FABRIC_ID { get; set; }
        public double? DC_TO { get; set; }
        public string DO_UNIT { get; set; }
        public string UNIT { get; set; }
        public string RET_QTY { get; set; }
        public double? STATUS { get; set; }
        public string FABRIC_CODE { get; set; }
        public string DO_REF { get; set; }
        public double? DO_QTY { get; set; }
        public double? ORACLE_DELIVERY { get; set; }
        public string DOT_NET_DELIVERY { get; set; }
        public string DEL_QTY_TOTAL { get; set; }
    }
}
