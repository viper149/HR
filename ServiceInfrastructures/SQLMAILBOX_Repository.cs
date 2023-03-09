using HRMS.Data;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;

namespace HRMS.ServiceInfrastructures
{
    public class SQLMAILBOX_Repository : BaseRepository<MAILBOX>, IMAILBOX
    {
        private readonly IDataProtector _protector;
        public SQLMAILBOX_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
            : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


    }
}
