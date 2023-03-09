using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.BaseModels
{
    public abstract class BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Created Date")]
        public virtual DateTime? CREATED_AT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Updated Date")]
        public virtual DateTime? UPDATED_AT { get; set; }
        [Display(Name = "Created By")]
        public virtual string CREATED_BY { get; set; }
        [Display(Name = "Updated By")]
        public virtual string UPDATED_BY { get; set; }
    }
}
