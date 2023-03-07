using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_CERTIFICATE_MANAGEMENT_Repository : BaseRepository<COM_EX_CERTIFICATE_MANAGEMENT>,
        ICOM_EX_CERTIFICATE_MANAGEMENT
    {

        private readonly IDataProtector _protector;

        public SQLCOM_EX_CERTIFICATE_MANAGEMENT_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }



        public async Task<IEnumerable<COM_EX_CERTIFICATE_MANAGEMENT>> GetAllComExCertificateManagement()
        {
            return await DenimDbContext.COM_EX_CERTIFICATE_MANAGEMENT
                .Include(e => e.INV)
                .Select(d => new COM_EX_CERTIFICATE_MANAGEMENT()
                {
                    CMID = d.CMID,
                    EncryptedId = _protector.Protect(d.CMID.ToString()),
                    ORGANIC_TYPE = d.ORGANIC_TYPE,
                    BCI_TRACER_ID = d.BCI_TRACER_ID,
                    BCI_REMARKS = d.BCI_REMARKS,
                    CMIA_REMARKS = d.CMIA_REMARKS,
                    RCS_REF = d.RCS_REF,
                    RCS_REMARKS = d.RCS_REMARKS,
                    GRS_REF = d.GRS_REF,
                    GRS_REMARKS = d.GRS_REMARKS,
                    CREATED_BY = d.CREATED_BY,
                    CREATED_AT = d.CREATED_AT,
                    UPDATED_BY = d.UPDATED_BY,
                    INV = new COM_EX_INVOICEMASTER()
                    {
                        INVNO = d.INV.INVNO
                    }

                }).ToListAsync();
        }



        public async Task<ComExCertificateManagementViewModel>GetInitObjByAsync(ComExCertificateManagementViewModel comExCertificateManagementViewModel, bool filter)

        {
            if (filter)
            {

                comExCertificateManagementViewModel.ComExInvoicemasterList = await DenimDbContext.COM_EX_INVOICEMASTER
               .Select(d => new COM_EX_INVOICEMASTER
               {
                   INVID = d.INVID,
                   INVNO = d.INVNO,
               }).ToListAsync();

            }

            else
            {

                comExCertificateManagementViewModel.ComExInvoicemasterList = await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(d => d.COM_EX_CERTIFICATE_MANAGEMENT)
                .Where(d => !d.COM_EX_CERTIFICATE_MANAGEMENT.Any(f => f.INVID.Equals(d.INVID)))
                .Select(d => new COM_EX_INVOICEMASTER
                {
                    INVID = d.INVID,
                    INVNO = d.INVNO,
                }).ToListAsync();

                

            }

            return comExCertificateManagementViewModel;

        }



        public async Task<COM_EX_INVOICEMASTER> GetAllByIdAsync(int id)
        {
            return await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(d => d.LC)
                .Include(d => d.BUYER)
                .Include(d => d.ComExInvdetailses)
                .ThenInclude(d=> d.ComExFabstyle.BRAND)
                .Include(d=>d.ComExInvdetailses)
                .ThenInclude(d => d.ComExFabstyle.FABCODENavigation)
                .Where(d => d.INVID.Equals(id))
                .FirstOrDefaultAsync();
        }

    }
}
