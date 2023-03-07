using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.HR;
using DenimERP.ViewModels.Factory;
using DenimERP.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_HRD_EMPLOYEE_Repository: BaseRepository<F_HRD_EMPLOYEE>, IF_HRD_EMPLOYEE
    {
        private readonly IF_BAS_HRD_DEPARTMENT _fBasHrdDepartment;
        private readonly IF_BAS_HRD_SECTION _fBasHrdSection;
        private readonly IF_BAS_HRD_SUB_SECTION _fBasHrdSubSection;
        private readonly IF_BAS_HRD_DESIGNATION _fBasHrdDesignation;
        private readonly IBAS_BEN_BANK_MASTER _basBenBank;
        private readonly IF_BAS_HRD_EMP_TYPE _fBasHrdEmpType;
        private readonly IF_HRD_EMP_EDU_DEGREE _fHrdEmpEduDegree;
        private readonly IDataProtector _protector;

        public SQLF_HRD_EMPLOYEE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_BAS_HRD_DEPARTMENT fBasHrdDepartment,
            IF_BAS_HRD_SECTION fBasHrdSection,
            IF_BAS_HRD_SUB_SECTION fBasHrdSubSection,
            IF_BAS_HRD_DESIGNATION fBasHrdDesignation,
            IBAS_BEN_BANK_MASTER basBenBank,
            IF_BAS_HRD_EMP_TYPE fBasHrdEmpType,
            IF_HRD_EMP_EDU_DEGREE fHrdEmpEduDegree) : base(denimDbContext)
        {
            _fBasHrdDepartment = fBasHrdDepartment;
            _fBasHrdSection = fBasHrdSection;
            _fBasHrdSubSection = fBasHrdSubSection;
            _fBasHrdDesignation = fBasHrdDesignation;
            _basBenBank = basBenBank;
            _fBasHrdEmpType = fBasHrdEmpType;
            _fHrdEmpEduDegree = fHrdEmpEduDegree;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<GetFHrEmployeeViewModel>> GetAllEmployeesAsync()
        {
            return await DenimDbContext.F_HRD_EMPLOYEE
                .Include(c => c.DEPT)
                .Include(c => c.SEC)
                .Include(e => e.DESIG)
                .Include(e => e.EMPTYPE)
                .Select(c => new GetFHrEmployeeViewModel
                {
                    EncryptedId = _protector.Protect(c.EMPID.ToString()),
                    EMPID = c.EMPID.ToString(),
                    EMPNO = c.EMPNO,
                    FULL_NAME = $"{c.FIRST_NAME} {c.LAST_NAME}",
                    DEPARTMENT = c.DEPT.DEPTNAME,
                    DESIGNATION = c.DESIG.DES_NAME,
                    SECTION = c.SEC.SEC_NAME,
                    EMP_TYPE = c.EMPTYPE.TYPE_NAME,

                }).ToListAsync();
        }

        public async Task<IEnumerable<F_HRD_EMPLOYEE>> GetAllFHrdEmployeeAsync()
        {
            var x = await DenimDbContext.F_HRD_EMPLOYEE
                .Include(d => d.GENDER)
                .Include(d => d.DESIG)
                .Include(d => d.DEPT)
                .Include(d => d.SEC)
                .Include(d => d.SUBSEC)
                .Include(d => d.OD)
                .Include(d => d.BLDGRP)
                .Include(d => d.RELIGION)
                //.Include(d=>d.COMPANY)
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    EncryptedId = _protector.Protect(d.EMPID.ToString()),
                    EMPNO = d.EMPNO,
                    PROX_CARD = d.PROX_CARD,
                    NAME = $"{d.FIRST_NAME} {d.LAST_NAME}",
                    NAME_BN = $"{d.FIRST_NAME_BN} {d.LAST_NAME_BN}",
                    NID_NO = d.NID_NO,
                    GENDER = new BAS_GENDER
                    {
                        GENNAME = d.GENDER.GENNAME
                    },
                    //COMPANY = new COMPANY_INFO
                    //{
                    //    COMPANY_NAME = d.COMPANY.COMPANY_NAME
                    //},
                    DESIG = new F_BAS_HRD_DESIGNATION
                    {
                        DES_NAME = d.DESIG.DES_NAME
                    },
                    DEPT = new F_BAS_HRD_DEPARTMENT
                    {
                        DEPTNAME = d.DEPT.DEPTNAME
                    },
                    SEC = new F_BAS_HRD_SECTION
                    {
                        SEC_NAME = d.SEC.SEC_NAME
                    },
                    SUBSEC = new F_BAS_HRD_SUB_SECTION
                    {
                        SUBSEC_NAME = d.SUBSEC.SUBSEC_NAME
                    },
                    OD = new F_BAS_HRD_WEEKEND
                    {
                        OD_FULL_NAME = d.OD.OD_FULL_NAME
                    },
                    BLDGRP = new F_BAS_HRD_BLOOD_GROUP
                    {
                        BLDGRP_NAME = d.BLDGRP.BLDGRP_NAME
                    },
                    RELIGION = new F_BAS_HRD_RELIGION
                    {
                        RELEGION_NAME = d.RELIGION.RELEGION_NAME
                    }
                }).ToListAsync();
            return x;
        }

        public async Task<FHrdEmployeeViewModel> GetInitObjByAsync(FHrdEmployeeViewModel fHrdEmployeeViewModel)
        {
            fHrdEmployeeViewModel.BasGenderList = await DenimDbContext.BAS_GENDER
                .Select(d => new BAS_GENDER
                {
                    GENID = d.GENID,
                    GENNAME = d.GENNAME
                }).ToListAsync();

            fHrdEmployeeViewModel.FBasHrdBloodGroupList = await DenimDbContext.F_BAS_HRD_BLOOD_GROUP
                .Select(d => new F_BAS_HRD_BLOOD_GROUP
                {
                    BLDGRPID = d.BLDGRPID,
                    BLDGRP_NAME = d.BLDGRP_NAME
                }).ToListAsync();

            fHrdEmployeeViewModel.FBasHrdReligionList = await DenimDbContext.F_BAS_HRD_RELIGION
                .Select(d => new F_BAS_HRD_RELIGION
                {
                    RELIGIONID = d.RELIGIONID,
                    RELEGION_NAME = $"{d.RELEGION_NAME} - {d.RELEGION_NAME_BNG}"
                }).ToListAsync();

            fHrdEmployeeViewModel.FBasHrdNationalityList = await DenimDbContext.F_BAS_HRD_NATIONALITY
                .Select(d => new F_BAS_HRD_NATIONALITY
                {
                    NATIONID = d.NATIONID,
                    NATION_DESC = d.NATION_DESC
                }).ToListAsync();

            fHrdEmployeeViewModel.FBasHrdShiftList = await DenimDbContext.F_BAS_HRD_SHIFT
                .Select(d => new F_BAS_HRD_SHIFT
                {
                    SHIFTID = d.SHIFTID,
                    SHIFT_NAME = d.SHIFT_NAME
                }).ToListAsync();

            fHrdEmployeeViewModel.FBasHrdWeekendList = await DenimDbContext.F_BAS_HRD_WEEKEND
                .Select(d => new F_BAS_HRD_WEEKEND
                {
                    ODID = d.ODID,
                    OD_FULL_NAME = d.OD_FULL_NAME
                }).ToListAsync();

            fHrdEmployeeViewModel.BasBenBankList = await _basBenBank.GetAllBenBanksAsync();
            fHrdEmployeeViewModel.FBasHrdEmpTypeList = await _fBasHrdEmpType.GetAllEmpTypesAsync();
            fHrdEmployeeViewModel.FBasHrdDepartmentList = await _fBasHrdDepartment.GetAllDepartmentsAsync();
            fHrdEmployeeViewModel.FBasHrdSectionList = await _fBasHrdSection.GetAllSectionsAsync();
            fHrdEmployeeViewModel.FBasHrdSubSectionList = await _fBasHrdSubSection.GetAllSubSectionsAsync();
            fHrdEmployeeViewModel.FBasHrdDesignationList = await _fBasHrdDesignation.GetAllDesignationsAsync();
            fHrdEmployeeViewModel.FHrdEmpEduDegreeList = await _fHrdEmpEduDegree.GetAllEduDegreesAsync();

            return fHrdEmployeeViewModel;
        }

        public async Task<IEnumerable<F_BAS_HRD_DIVISION>> GetDivByNationIdAsync(int nationId)
        {
            return await DenimDbContext.F_BAS_HRD_DIVISION
                .Where(d => d.COUNTRYID.Equals(nationId))
                .Select(d => new F_BAS_HRD_DIVISION
                {
                    DIVID = d.DIVID,
                    DIV_NAME = $"{d.DIV_NAME} - {d.DIV_NAME_BN}"
                }).ToListAsync();
        }

        public async Task<IEnumerable<F_BAS_HRD_DISTRICT>> GetDistByDivIdAsync(int divId)
        {
            return await DenimDbContext.F_BAS_HRD_DISTRICT
                .Where(d => d.DIVID.Equals(divId))
                .Select(d => new F_BAS_HRD_DISTRICT
                {
                    DISTID = d.DISTID,
                    DIST_NAME = $"{d.DIST_NAME} - {d.DIST_NAME_BN}"
                }).ToListAsync();
        }

        public async Task<IEnumerable<F_BAS_HRD_THANA>> GetThanaByDistIdAsync(int distId)
        {
            return await DenimDbContext.F_BAS_HRD_THANA
                .Where(d => d.DISTID.Equals(distId))
                .Select(d => new F_BAS_HRD_THANA
                {
                    THANAID = d.THANAID,
                    THANA_NAME = $"{d.THANA_NAME} - {d.THANA_NAME_BN}"
                }).ToListAsync();
        }

        public async Task<IEnumerable<F_BAS_HRD_UNION>> GetUnionByThanaIdAsync(int thanaId)
        {
            return await DenimDbContext.F_BAS_HRD_UNION
                .Where(d => d.THANAID.Equals(thanaId))
                .Select(d => new F_BAS_HRD_UNION
                {
                    UNIONID = d.UNIONID,
                    UNION_NAME = $"{d.UNION_NAME} - {d.UNION_NAME_BN}"
                }).ToListAsync();
        }

        public async Task<FHrdEmployeeViewModel> GetInitDetailsObjEduByAsync(FHrdEmployeeViewModel fHrEmployeeViewModel)
        {
            foreach (var item in fHrEmployeeViewModel.FHrdEducationList)
            {
                item.DEG = await DenimDbContext.F_HRD_EMP_EDU_DEGREE
                    .Select(d => new F_HRD_EMP_EDU_DEGREE
                    {
                        DEGID = d.DEGID,
                        DEGNAME = d.DEGNAME
                    }).FirstOrDefaultAsync(e => e.DEGID.Equals(item.DEGID));
            }

            return fHrEmployeeViewModel;
        }

        public async Task<bool> FindByEmpNoAsync(string empNo)
        {
            return !await DenimDbContext.F_HRD_EMPLOYEE.AnyAsync(d => d.EMPNO.Equals(empNo));
        }

        public async Task<bool> FindByValueAsync(string value, string type)
        {
            return type switch
            {
                "EmpNo" => !await DenimDbContext.F_HRD_EMPLOYEE.AnyAsync(d => d.EMPNO.Equals(value)),
                "Proximity" => !await DenimDbContext.F_HRD_EMPLOYEE.AnyAsync(d => d.PROX_CARD.Equals(value)),
                "NID" => !await DenimDbContext.F_HRD_EMPLOYEE.AnyAsync(d => d.NID_NO.Equals(value)),
                "BID" => !await DenimDbContext.F_HRD_EMPLOYEE.AnyAsync(d => d.BID_NO.Equals(value)),
                "Passport" => !await DenimDbContext.F_HRD_EMPLOYEE.AnyAsync(d => d.PASSPORT.Equals(value)),
                _ => true
            };
        }
    }
}
