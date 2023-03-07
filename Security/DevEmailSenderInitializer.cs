using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces;

namespace DenimERP.Security
{
    public class DevEmailSenderInitializer
    {
        private readonly IPDL_EMAIL_SENDER _pdlEmailSender;

        public DevEmailSenderInitializer(IPDL_EMAIL_SENDER pdlEmailSender)
        {
            _pdlEmailSender = pdlEmailSender;
        }

        public DevEmailSenderInitializer()
        {
            
        }

        public async Task<PDL_EMAIL_SENDER> GetEmailSenderCredentials()
        {
            return await _pdlEmailSender.GetTop1Async();
        }
    }
}
