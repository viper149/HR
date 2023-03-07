using System;

namespace DenimERP.Models
{
    public partial class MAILBOX
    {
        public int ID { get; set; }
        public string MAIL_FROM { get; set; }
        public string MAIL_SUBJECT { get; set; }
        public string MAIL_BODY { get; set; }
        public string MAIL_ATTACHMENT { get; set; }
        public string IP_PHONE { get; set; }
        public string MOBILE_NO { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
    }
}
