using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using HRMS.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class F_HRD_EMPLOYEE: BaseEntity
    {
        public F_HRD_EMPLOYEE()
        {
            F_HRD_EDUCATION = new HashSet<F_HRD_EDUCATION>();
            F_HRD_EMERGENCY = new HashSet<F_HRD_EMERGENCY>();
            F_HRD_EMP_CHILDREN = new HashSet<F_HRD_EMP_CHILDREN>();
            F_HRD_EMP_SPOUSE = new HashSet<F_HRD_EMP_SPOUSE>();
            F_HRD_EXPERIENCE = new HashSet<F_HRD_EXPERIENCE>();
            F_HRD_INCREMENT = new HashSet<F_HRD_INCREMENT>();
            F_HRD_PROMOTION = new HashSet<F_HRD_PROMOTION>();
            F_HRD_SKILL = new HashSet<F_HRD_SKILL>();
        }

        public int EMPID { get; set; }
        [Display(Name = "Employee No. ")]
        [Remote(action: "EmpNoAlreadyInUse", controller: "Employee")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string EMPNO { get; set; }
        [Display(Name = "Proximity Card No.")]
        [Remote(action: "ProxNoAlreadyInUse", controller: "Employee")]
        [Required]
        public string PROX_CARD { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FIRST_NAME { get; set; }
        [Display(Name = "Last Name")]
        public string LAST_NAME { get; set; }
        [Display(Name = "Image")]
        public byte[] IMAGE_PP { get; set; }
        [Display(Name = "Company")]
        [Required]
        public int? COMPANYID { get; set; }
        [Display(Name = "Designation")]
        [Required]
        public int? DESIGID { get; set; }
        [Display(Name = "Sub-Section")]
        [Required]
        public int? SUBSECID { get; set; }
        [Display(Name = "Section")]
        [Required]
        public int? SECID { get; set; }
        [Display(Name = "Department")]
        [Required]
        public int? DEPTID { get; set; }
        [Display(Name = "Shift")]
        [Required]
        public int? SHIFTID { get; set; }
        [Display(Name = "Weekend")]
        [Required]
        public int? ODID { get; set; }
        [Display(Name = "Sex")]
        [Required]
        public int? GENDERID { get; set; }
        [Display(Name = "Date of Birth")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DATE_BIRTH { get; set; }
        [Display(Name = "Contact No.")]
        [Required]
        public string MOBILE { get; set; }
        [Display(Name = "Contact No (Father)")]
        public string MOBILE_F { get; set; }
        [Display(Name = "Contact No (Mother)")]
        public string MOBILE_M { get; set; }
        [Display(Name = "Contact No (Office)")]
        public string MOBILE_G { get; set; }
        [Display(Name = "Blood Group")]
        [Required]
        public int? BLDGRPID { get; set; }
        [Display(Name = "Father's Name")]
        [Required]
        public string F_NAME { get; set; }
        [Display(Name = "Mother's Name")]
        [Required]
        public string M_NAME { get; set; }
        [Display(Name = "Address")]
        public string ADD_PRE { get; set; }
        [Display(Name = "Village")]
        [Required]
        public string VILLAGE_PRE { get; set; }
        [Display(Name = "Post Office")]
        [Required]
        public string PO_PRE { get; set; }
        [Display(Name = "ঠিকানা")]
        public string ADD_PRE_BN { get; set; }
        [Display(Name = "ঠিকানা")]
        public string ADD_PER_BN { get; set; }
        [Display(Name = "গ্রাম")]
        public string VILLAGE_PRE_BN { get; set; }
        [Display(Name = "পোস্ট অফিস")]
        public string PO_PRE_BN { get; set; }
        [Display(Name = "পিতার নাম")]
        [Required]
        public string F_NAME_BN { get; set; }
        [Display(Name = "মাতার নাম")]
        [Required]
        public string M_NAME_BN { get; set; }
        [Display(Name = "Union/Municipality - ইউনিয়ন/পৌরসভা")]
        [Required]
        public int? UNIONID_PRE { get; set; }
        [Display(Name = "Ward No.")]
        [Required]
        public int? WORDNO_PRE { get; set; }
        [Display(Name = "Nationality")]
        [Required]
        public int? NATIONALITY_ID { get; set; }
        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required]
        public DateTime? DATE_JOINING { get; set; }
        [Display(Name = "Joining Salary")]
        [Required]
        public double? SALARY_JOINING { get; set; }
        [Display(Name = "Present Salary")]
        [Required]
        public double? SALARY_PRE { get; set; }
        [Display(Name = "Bank Salary")]
        [Required]
        public double? SALARY_BANK { get; set; }
        [Display(Name = "Cash Salary")]
        [Required]
        public double? SALARY_CASH { get; set; }
        [Display(Name = "Tax Amount")]
        public double? AMT_TAX { get; set; }
        [Display(Name = "Mess Amount")]
        public double? AMT_MESS { get; set; }
        [Display(Name = "Utility Charge")]
        public double? AMT_UTILITY { get; set; }
        [Display(Name = "Employee Type")]
        [Required]
        public int? EMPTYPEID { get; set; }
        [Display(Name = "Out Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? OUT_DATE { get; set; }
        [Display(Name = "Bank Nme (Beneficiary)")]
        public int? BANKID { get; set; }
        [Display(Name = "Bank Acc. No.")]
        public string BANK_ACC_NO { get; set; }
        [Display(Name = "Tin No.")]
        public string TIN_NO { get; set; }
        [Display(Name = "National Identity No.")]
        [Remote(action: "NIDAlreadyInUse", controller: "Employee")]
        public string NID_NO { get; set; }
        [Display(Name = "Birth Certificate No.")]
        [Remote(action: "BIDAlreadyInUse", controller: "Employee")]
        public string BID_NO { get; set; }
        [Display(Name = "Passport No.")]
        [Remote(action: "PassportAlreadyInUse", controller: "Employee")]
        public string PASSPORT { get; set; }
        [Display(Name = "Religion")]
        [Required]
        public int? RELIGIONID { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "Email Address")]
        public string EMAIL { get; set; }
        [Display(Name = "Job")]
        public bool STATUS_JOB { get; set; }
        [Display(Name = "Tax")]
        public bool STATUS_TAX { get; set; }
        [Display(Name = "Over Time")]
        public bool STATUS_OT { get; set; }
        [Display(Name = "Married")]
        public bool STATUS_MARIED { get; set; }
        [Display(Name = "Special Bonus")]
        public bool SP_BONUS { get; set; }
        [Display(Name = "Eid Bonus (Half)")]
        public bool STATUS_EB_25 { get; set; }
        [Display(Name = "Eid Bonus (Full)")]
        public bool STATUS_EB_50 { get; set; }
        [Display(Name = "MFL")]
        public bool STATUS_MFL { get; set; }
        [Display(Name = "ML")]
        public bool STATUS_ML { get; set; }
        [Display(Name = "Transport")]
        public bool STATUS_TRANSPORT { get; set; }
        [Display(Name = "UTC")]
        public bool STATUS_UTC { get; set; }
        [Display(Name = "One Punch")]
        public bool STATUS_ONE_PUNCH { get; set; }
        [Display(Name = "Proximity Card Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DATE_EMPSID_ISSUE { get; set; }
        [Display(Name = "Date of Transfer")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DATE_TRANSFER { get; set; }
        [Display(Name = "Out reason")]
        public int? OUT_RESASON_ID { get; set; }
        [Display(Name = "নামের প্রথম অংশ")]
        public string FIRST_NAME_BN { get; set; }
        [Display(Name = "নামের শেষ অংশ")]
        public string LAST_NAME_BN { get; set; }
        [Display(Name = "Address")]
        public string ADD_PER { get; set; }
        [Display(Name = "Post Office")]
        public string PO_PER { get; set; }
        [Display(Name = "Village")]
        public string VILLAGE_PER { get; set; }
        [Display(Name = "গ্রাম")]
        public string VILLAGE_PER_BN { get; set; }
        [Display(Name = "পোস্ট অফিস")]
        public string PO_PER_BN { get; set; }
        [Display(Name = "Union/Municipality - ইউনিয়ন/পৌরসভা")]
        public int? UNIONID_PER { get; set; }
        [Display(Name = "Ward No.")]
        public int? WORDNO_PER { get; set; }
        [Display(Name = ("Division - বিভাগ"))]
        public int? DIVID_PRE { get; set; }
        [Display(Name = "Division - বিভাগ")]
        public int? DIVID_PER { get; set; }
        [Display(Name = "District - জেলা")]
        public int? DISTID_PRE { get; set; }
        [Display(Name = "District - জেলা")]
        public int? DISTID_PER { get; set; }
        [Display(Name = "Upazila - উপজেলা")]
        public int? THANAID_PRE { get; set; }
        [Display(Name = "Upazila - উপজেলা")]
        public int? THANAID_PER { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        public IFormFile IMAGE { get; set; }
        [NotMapped]
        [Display(Name = "Name")]
        public string NAME { get; set; }
        [NotMapped]
        [Display(Name = "নাম")]
        public string NAME_BN { get; set; }
        [NotMapped]
        [Display(Name = "Same as Present Address")]
        public bool SAME_AS_PRESENT_ADDRESS { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_BAS_HRD_BLOOD_GROUP BLDGRP { get; set; }
        public F_BAS_HRD_DEPARTMENT DEPT { get; set; }
        public F_BAS_HRD_DESIGNATION DESIG { get; set; }
        public F_BAS_HRD_EMP_TYPE EMPTYPE { get; set; }
        public F_BAS_HRD_NATIONALITY NATIONALITY_ { get; set; }
        public F_BAS_HRD_WEEKEND OD { get; set; }
        public F_BAS_HRD_OUT_REASON OUT_RESASON_ { get; set; }
        public F_BAS_HRD_RELIGION RELIGION { get; set; }
        public F_BAS_HRD_SECTION SEC { get; set; }
        public F_BAS_HRD_SHIFT SHIFT { get; set; }
        public F_BAS_HRD_SUB_SECTION SUBSEC { get; set; }
        public F_BAS_HRD_UNION UNION_PER { get; set; }
        public F_BAS_HRD_UNION UNION_PRE { get; set; }
        public COMPANY_INFO COMPANY { get; set; }
        public BAS_GENDER GENDER { get; set; }
        public F_BAS_HRD_DIVISION DIV_PER { get; set; }
        public F_BAS_HRD_DIVISION DIV_PRE { get; set; }
        public F_BAS_HRD_DISTRICT DIST_PER { get; set; }
        public F_BAS_HRD_DISTRICT DIST_PRE { get; set; }
        public F_BAS_HRD_THANA THANA_PER { get; set; }
        public F_BAS_HRD_THANA THANA_PRE { get; set; }

        public ICollection<F_HRD_EDUCATION> F_HRD_EDUCATION { get; set; }
        public ICollection<F_HRD_EMERGENCY> F_HRD_EMERGENCY { get; set; }
        public ICollection<F_HRD_EMP_CHILDREN> F_HRD_EMP_CHILDREN { get; set; }
        public ICollection<F_HRD_EMP_SPOUSE> F_HRD_EMP_SPOUSE { get; set; }
        public ICollection<F_HRD_EXPERIENCE> F_HRD_EXPERIENCE { get; set; }
        public ICollection<F_HRD_INCREMENT> F_HRD_INCREMENT { get; set; }
        public ICollection<F_HRD_PROMOTION> F_HRD_PROMOTION { get; set; }
        public ICollection<F_HRD_SKILL> F_HRD_SKILL { get; set; }
    }
}
