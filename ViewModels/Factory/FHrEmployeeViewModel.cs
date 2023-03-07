using System.Collections.Generic;
using DenimERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels.Factory
{
    public class FHrEmployeeViewModel
    {
        public FHrEmployeeViewModel()
        {
            Upozilas = new List<UPOZILAS>();
            Districts = new List<DISTRICTS>();
            Countries = new List<COUNTRIES>();
            FBasSections = new List<F_BAS_SECTION>();
            FHrBloodGroups = new List<F_HR_BLOOD_GROUP>();
            FBasDepartments = new List<F_BAS_DEPARTMENT>();
            FBasSubsections = new List<F_BAS_SUBSECTION>();
            FBasDesignations = new List<F_BAS_DESIGNATION>();
            FHrEmpEducations = new List<F_HR_EMP_EDUCATION>();
            FHrEmpFamilyDetailsList = new List<F_HR_EMP_FAMILYDETAILS>();
        }

        public F_HRD_EMPLOYEE FHrdEmployee { get; set; }
        public List<UPOZILAS> Upozilas { get; set; }
        public List<DISTRICTS> Districts { get; set; }
        public List<COUNTRIES> Countries { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }        
        public List<F_HR_BLOOD_GROUP> FHrBloodGroups { get; set; }
        public List<F_BAS_DEPARTMENT> FBasDepartments { get; set; }
        public List<F_BAS_SUBSECTION> FBasSubsections { get; set; }
        public List<F_BAS_DESIGNATION> FBasDesignations { get; set; }
        public List<F_HR_EMP_EDUCATION> FHrEmpEducations { get; set; }
        public List<F_HR_EMP_FAMILYDETAILS> FHrEmpFamilyDetailsList { get; set; }
        
        // CHECK LATER
        public F_HR_EMP_OFFICIALINFO FHrEmpOfficialInfo { get; set; }
        public F_HR_EMP_EDUCATION FHrEmpEducation { get; set; }
        public F_HR_EMP_SALARYSETUP FHrEmpSalarySetup { get; set; }
        public F_HR_EMP_FAMILYDETAILS FHrEmpFamilyDetails { get; set; }
        public List<F_HR_EMP_SALARYSETUP> FHrEmpSalarySetups { get; set; }
        
        public SelectList SexualOrientation { get; set; }
        public SelectList MaritalStatus { get; set; }
        public SelectList EmployeeType { get; set; }
        public SelectList MFSType { get; set; }
        public SelectList ExamType { get; set; }
        public SelectList AdditionalRelation { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
