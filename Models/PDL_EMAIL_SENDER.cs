using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class PDL_EMAIL_SENDER : BaseEntity
    {
        public int ID { get; set; }
        [Display(Name = "From Email", Prompt = "Sender Email. [example@domain.com]"),
        Required,
        EmailAddress]
        public string FROM_EMAIL { get; set; }
        [Display(Name = "Smtp Client Host", Prompt = "Host Email. [example@domain.com]"),
        Required,
        EmailAddress]
        public string SMTP_CLIENT { get; set; }
        [Display(Name = "Port No", Prompt = "Type Port Number"),
        Required]
        public int PORT_NO { get; set; }
        [Display(Name = "Use Default Credentials", Prompt = "Default Credentials")]
        public bool USE_DEFAULT_CREDENTIALS { get; set; }
        [Display(Name = "Enable SSL", Prompt = "SSL")]
        public bool ENABLE_SSL { get; set; }
        [Display(Name = "Network Credentials User Name", Prompt = "Network Credentials User Name [example@domain.com]"),
        Required,
        EmailAddress]
        public string NETWORK_CREDENTIAL_USERNAME { get; set; }
        [Display(Name = "Network Credentials Password", Prompt = "Network Credentials Password"),
         DataType(DataType.Password)]
        public string NETWORK_CREDENTIAL_PASSWORD { get; set; }
    }
}
