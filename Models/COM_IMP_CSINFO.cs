using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_IMP_CSINFO
    {
        public COM_IMP_CSINFO()
        {
            COM_IMP_CSITEM_DETAILS = new HashSet<COM_IMP_CSITEM_DETAILS>();
        }

        public int CSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "CS Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CSDATE { get; set; }
        [Display(Name = "CS NO")]
        public string CSNO { get; set; }
        [Display(Name = "Indent NO")]
        public int? INDID { get; set; }
        [Display(Name = "Subject")]
        public string SUBJECT { get; set; }
        [Display(Name = "Revised No")]
        public string REVISEDNO { get; set; }
        [Display(Name = "Next Review Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? NEXT_REVIEW_DATE { get; set; }
        [Display(Name = "Lowest Price")]
        public bool LOWESTPRICE { get; set; }
        [Display(Name = "Quality")]
        public bool QUALITY { get; set; }
        [Display(Name = "User Recommendation")]
        public bool URECOM { get; set; }
        [Display(Name = "Credit Facilities")]
        public bool CRFACILITIES { get; set; }
        [Display(Name = "Available Stock")]
        public bool AVAILABLE { get; set; }
        [Display(Name = "Other Unit Used or Not")]
        public bool USEDBY_OTHERUNIT { get; set; }
        [Display(Name = "Sole Agent")]
        public bool SOLE_AGENT { get; set; }
        [Display(Name = "Listed")]
        public bool LISTED { get; set; }
        [Display(Name = "Over Phone")]
        public bool OVER_PHONE { get; set; }
        [Display(Name = "Over Email")]
        public bool OVER_EMAIL { get; set; }
        [Display(Name = "Physically")]
        public bool PHYSICALLY { get; set; }
        [Display(Name = "Mode of Payment")]
        public string PAYMODE { get; set; }
        [Display(Name = "AIT Status")]
        public string AITSTATUS { get; set; }
        [Display(Name = "VAT")]
        public string VAT { get; set; }
        [Display(Name = "Warrantee")]
        public string WARRANTEE { get; set; }
        [Display(Name = "Comments")]
        public string REMARKS { get; set; }
        public string OPTION1 { get; set; }
        public string OPTION2 { get; set; }
        public string OPTION3 { get; set; }
        public string OPTION4 { get; set; }
        public string OPTION5 { get; set; }
        [Display(Name = "Recommendation")]
        public string RECOMMENDATION { get; set; }
        [Display(Name = "Approved By")]
        public string APPROVED_BY { get; set; }
        [Display(Name = "Approve")]
        public bool IS_APPROVE { get; set; }
        [Display(Name = "Enclosed")]
        public string ENCLOSED { get; set; }
        [Display(Name = "Created By")]
        public string CREATED_BY { get; set; }
        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CREATED_AT { get; set; }
        [Display(Name = "Updated By")]
        public string UPDATED_BY { get; set; }
        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime UPDATED_AT { get; set; }

        public ICollection<COM_IMP_CSITEM_DETAILS> COM_IMP_CSITEM_DETAILS { get; set; }
        public F_YS_INDENT_MASTER IND { get; set; }
    }
}
