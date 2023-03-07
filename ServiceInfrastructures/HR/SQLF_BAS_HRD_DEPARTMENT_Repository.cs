using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.HR;
using DenimERP.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.HR
{
    public class SQLF_BAS_HRD_DEPARTMENT_Repository : BaseRepository<F_BAS_HRD_DEPARTMENT>, IF_BAS_HRD_DEPARTMENT
    {
        private readonly IF_BAS_HRD_LOCATION _fBasHrdLocation;
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_DEPARTMENT_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_HRD_LOCATION fBasHrdLocation) : base(denimDbContext)
        {
            _fBasHrdLocation = fBasHrdLocation;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByDeptNameAsync(string deptName, bool isBn)
        {
            return !isBn ? !await DenimDbContext.F_BAS_HRD_DEPARTMENT.AnyAsync(d => d.DEPTNAME.Equals(deptName))
                : !await DenimDbContext.F_BAS_HRD_DEPARTMENT.AnyAsync(d => d.DEPTNAME_BN.Equals(deptName));
        }

        public async Task<IEnumerable<F_BAS_HRD_DEPARTMENT>> GetAllFBasHrdDepartmentAsync()
        {
            return await DenimDbContext.F_BAS_HRD_DEPARTMENT
                .Include(d=>d.LOCATION)
                .Select(d => new F_BAS_HRD_DEPARTMENT
                {
                    DEPTID = d.DEPTID,
                    EncryptedId = _protector.Protect(d.DEPTID.ToString()),
                    DEPTNAME = d.DEPTNAME,
                    DEPTNAME_BN = d.DEPTNAME_BN,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS,
                    LOCATION = new F_BAS_HRD_LOCATION
                    {
                        LOC_NAME = d.LOCATION.LOC_NAME
                    }
                }).ToListAsync();
        }

        public async Task<FBasHrdDepartmentViewModel> GetInitObjByAsync(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel)
        {
            fBasHrdDepartmentViewModel.FBasHrdLocationList = await _fBasHrdLocation.GetAllLocationsAsync();

            return fBasHrdDepartmentViewModel;
        }

        public async Task<List<F_BAS_HRD_DEPARTMENT>> GetAllDepartmentsAsync()
        {
            return await DenimDbContext.F_BAS_HRD_DEPARTMENT
                .Select(d => new F_BAS_HRD_DEPARTMENT
                {
                    DEPTID=d.DEPTID,
                    DEPTNAME=d.DEPTNAME
                }).ToListAsync();
        }
    }
}