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
    public class SQLF_BAS_HRD_DESIGNATION_Repository: BaseRepository<F_BAS_HRD_DESIGNATION>, IF_BAS_HRD_DESIGNATION
    {
        private readonly IF_BAS_HRD_GRADE _fBasHrdGrade;
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_DESIGNATION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_HRD_GRADE fBasHrdGrade) : base(denimDbContext)
        {
            _fBasHrdGrade = fBasHrdGrade;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_BAS_HRD_DESIGNATION>> GetAllFBasHrdDesignationAsync()
        {
            return await DenimDbContext.F_BAS_HRD_DESIGNATION
                .Include(d => d.GRADE)
                .Select(d => new F_BAS_HRD_DESIGNATION
                {
                    DESID = d.DESID,
                    EncryptedId = _protector.Protect(d.DESID.ToString()),
                    DES_NAME = d.DES_NAME,
                    DES_NAME_BN = d.DES_NAME_BN,
                    SHORT_NAME = d.SHORT_NAME,
                    SHORT_NAME_BN = d.SHORT_NAME_BN,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS,
                    GRADE = new F_BAS_HRD_GRADE
                    {
                        GRADE_NAME = d.GRADE.GRADE_NAME
                    }
                }).ToListAsync();
        }

        public async Task<bool> FindByDesNameAsync(string desName, bool isBn)
        {
            return !isBn ? !await DenimDbContext.F_BAS_HRD_DESIGNATION.AnyAsync(d => d.DES_NAME.Equals(desName))
                : !await DenimDbContext.F_BAS_HRD_DESIGNATION.AnyAsync(d => d.DES_NAME_BN.Equals(desName));
        }

        public async Task<FBasHrdDesignationViewModel> GetInitObjByAsync(FBasHrdDesignationViewModel fBasHrdDesignationViewModel)
        {
            fBasHrdDesignationViewModel.FBasHrdGradeList = await _fBasHrdGrade.GetAllGradesAsync();

            return fBasHrdDesignationViewModel;
        }

        public async Task<List<F_BAS_HRD_DESIGNATION>> GetAllDesignationsAsync()
        {
            return await DenimDbContext.F_BAS_HRD_DESIGNATION
                .Select(d => new F_BAS_HRD_DESIGNATION
                {
                    DESID = d.DESID,
                    DES_NAME = d.DES_NAME
                }).ToListAsync();
        }
    }
}
