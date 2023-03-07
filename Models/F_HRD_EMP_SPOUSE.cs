using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_HRD_EMP_SPOUSE : BaseEntity
    {
        public int SPID { get; set; }
        public int? EMPID { get; set; }
        [Display(Name = "Name")]
        public string SPNAME { get; set; }
        [Display(Name = "নাম")]
        public string SPNAME_BN { get; set; }
        [Display(Name = "Profession")]
        public string SP_PROFESSION { get; set; }
        [Display(Name = "National Identity No.")]
        //[Remote(action: "NIDAlreadyInUse", controller: "Employee")]
        public string SP_NID { get; set; }
        [Display(Name = "Birth Certificate No.")]
        //[Remote(action: "BIDAlreadyInUse", controller: "Employee")]
        public string SP_BID { get; set; }
        [Display(Name = "Passport No.")]
        //[Remote(action: "PassportAlreadyInUse", controller: "Employee")]
        public string SP_PASSPORT { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SP_DOB { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
