using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Receive
{
    public class SQLF_SAMPLE_LOCATION_Repository : BaseRepository<F_SAMPLE_LOCATION>, IF_SAMPLE_LOCATION
    {
        private readonly IDataProtector _protector;

        public SQLF_SAMPLE_LOCATION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<DataTableObject<F_SAMPLE_LOCATION>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "" };
            var fSampleLocations = DenimDbContext.F_SAMPLE_LOCATION
                .Select(e=> new F_SAMPLE_LOCATION
                {
                    LOCID = e.LOCID,
                    EncryptedId = _protector.Protect(e.LOCID.ToString()),
                    NAME = e.NAME,
                    DESCRIPTION = e.DESCRIPTION,
                    REMARKS = e.REMARKS
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                fSampleLocations = OrderedResult<F_SAMPLE_LOCATION>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleLocations);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                fSampleLocations = fSampleLocations
                    .Where(m => m.NAME.ToUpper().Contains(searchValue)
                                || !string.IsNullOrEmpty(m.DESCRIPTION) && m.DESCRIPTION.ToUpper().Contains(searchValue)
                                || !string.IsNullOrEmpty(m.REMARKS) && m.REMARKS.ToUpper().Contains(searchValue));

                fSampleLocations = OrderedResult<F_SAMPLE_LOCATION>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleLocations);
            }

            var recordsTotal = await fSampleLocations.CountAsync();

            return new DataTableObject<F_SAMPLE_LOCATION>(draw, recordsTotal, recordsTotal, await fSampleLocations.Skip(skip).Take(pageSize).ToListAsync());
        }
    }
}
