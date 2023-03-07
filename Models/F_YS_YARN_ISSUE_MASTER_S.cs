﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_ISSUE_MASTER_S : BaseEntity
    {
        public F_YS_YARN_ISSUE_MASTER_S()
        {
            F_YS_YARN_ISSUE_DETAILS_S = new HashSet<F_YS_YARN_ISSUE_DETAILS_S>();
        }

        public int YISSUEID { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? YISSUEDATE { get; set; }
        [Display(Name = "Issue Type")]
        public int? ISSUEID { get; set; }
        [Display(Name = "Yarn Req. No.")]
        public int? YSRID { get; set; }
        [Display(Name = "Issue To.")]
        public string ISSUETO { get; set; }
        [Display(Name = "Purpose")]
        public string PURPOSE { get; set; }
        [Display(Name = "Is Returnable")]
        public bool? ISREMARKABLE { get; set; }
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

        public F_BAS_ISSUE_TYPE ISSUE { get; set; }
        public F_YARN_REQ_MASTER_S YSR { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S { get; set; }
    }
}