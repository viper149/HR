using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLMAILBOX_Repository : BaseRepository<MAILBOX>, IMAILBOX
    {
        private readonly IDataProtector _protector;
        public SQLMAILBOX_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
            : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


    }
}
