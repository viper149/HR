using System;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class BaDifference
    {
        public DateTime? BnkAccDate { get; set; }
        public DateTime? MatuDate { get; set; }
        public string InvId { get; set; }
        public double? _BaDifference { get; set; }
    }
}
