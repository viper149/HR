using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces;

namespace HRMS.Security
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
