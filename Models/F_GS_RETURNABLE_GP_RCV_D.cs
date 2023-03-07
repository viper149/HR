using System;

namespace DenimERP.Models
{
    public partial class F_GS_RETURNABLE_GP_RCV_D
    {
        public int TRNSID { get; set; }
        public int? RCVID { get; set; }
        public int? PRODID { get; set; }
        public double? RCV_QTY { get; set; }
        public int? RCV_BAG { get; set; }
        public string GATE_ENTRYNO { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_GS_PRODUCT_INFORMATION PROD { get; set; }
        public F_GS_RETURNABLE_GP_RCV_M RCV { get; set; }
    }
}
