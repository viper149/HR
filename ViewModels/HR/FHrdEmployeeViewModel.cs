using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.HR
{
    public class FHrdEmployeeViewModel
    {
        public FHrdEmployeeViewModel()
        {
            BasGenderList = new List<BAS_GENDER>();
            FBasHrdBloodGroupList = new List<F_BAS_HRD_BLOOD_GROUP>();
            FBasHrdReligionList = new List<F_BAS_HRD_RELIGION>();
            FBasHrdNationalityList = new List<F_BAS_HRD_NATIONALITY>();
            FBasHrdDepartmentList = new List<F_BAS_HRD_DEPARTMENT>();
            FBasHrdSectionList = new List<F_BAS_HRD_SECTION>();
            FBasHrdSubSectionList = new List<F_BAS_HRD_SUB_SECTION>();
            FBasHrdDesignationList = new List<F_BAS_HRD_DESIGNATION>();
            FBasHrdEmpTypeList = new List<F_BAS_HRD_EMP_TYPE>();
            BasBenBankList = new List<BAS_BEN_BANK_MASTER>();
            FHrdEmpEduDegreeList = new List<F_HRD_EMP_EDU_DEGREE>();
            FHrdEducationList = new List<F_HRD_EDUCATION>();
            FBasHrdShiftList = new List<F_BAS_HRD_SHIFT>();
            FBasHrdWeekendList = new List<F_BAS_HRD_WEEKEND>();
            FHrdEmpSpouseList = new List<F_HRD_EMP_SPOUSE>();
        }

        public F_HRD_EMPLOYEE FHrdEmployee { get; set; }
        public F_HRD_EDUCATION FHrdEducation { get; set; }
        public F_HRD_EMP_SPOUSE FHrdEmpSpouse { get; set; }

        public List<BAS_GENDER> BasGenderList { get; set; }
        public List<F_BAS_HRD_BLOOD_GROUP> FBasHrdBloodGroupList { get; set; }
        public List<F_BAS_HRD_RELIGION> FBasHrdReligionList { get; set; }
        public List<F_BAS_HRD_NATIONALITY> FBasHrdNationalityList { get; set; }
        public List<F_BAS_HRD_DEPARTMENT> FBasHrdDepartmentList { get; set; }
        public List<F_BAS_HRD_SECTION> FBasHrdSectionList { get; set; }
        public List<F_BAS_HRD_SUB_SECTION> FBasHrdSubSectionList { get; set; }
        public List<F_BAS_HRD_DESIGNATION> FBasHrdDesignationList { get; set; }
        public List<F_BAS_HRD_EMP_TYPE> FBasHrdEmpTypeList { get; set; }
        public List<BAS_BEN_BANK_MASTER> BasBenBankList { get; set; }
        public List<F_HRD_EMP_EDU_DEGREE> FHrdEmpEduDegreeList { get; set; }
        public List<F_HRD_EDUCATION> FHrdEducationList { get; set; }
        public List<F_BAS_HRD_SHIFT> FBasHrdShiftList { get; set; }
        public List<F_BAS_HRD_WEEKEND> FBasHrdWeekendList { get; set; }
        public List<F_HRD_EMP_SPOUSE> FHrdEmpSpouseList { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
