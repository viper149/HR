using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels.Security
{
    public class ExtendPdlEmailSenderViewModel : PDL_EMAIL_SENDER
    {
        [Display(Name = "Network Credentials Password", Prompt = "Network Credentials Password"), DataType(DataType.Password)]
        public string NEW_NETWORK_CREDENTIAL_PASSWORD { get; set; }
    }
}
