namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_DISPATCH_TRANSACTION
    {
        public int TID { get; set; }
        public int? DPDID { get; set; }
        public double? REQ_QTY { get; set; }
        public double? DEL_QTY { get; set; }
        public double? OP_BALANCE { get; set; }
        public double? BALANCE { get; set; }

        public F_SAMPLE_FABRIC_DISPATCH_DETAILS DPD { get; set; }
    }
}
