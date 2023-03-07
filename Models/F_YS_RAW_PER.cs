using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_YS_RAW_PER
    {
        public F_YS_RAW_PER()
        {
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
            FYsYarnReceiveDetailses = new HashSet<F_YS_YARN_RECEIVE_DETAILS>();
            F_YS_YARN_RECEIVE_DETAILS_S = new HashSet<F_YS_YARN_RECEIVE_DETAILS_S>();
            F_YS_YARN_RECEIVE_DETAILS2 = new HashSet<F_YS_YARN_RECEIVE_DETAILS2>();
        }

        public int RAWID { get; set; }
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "RAW (%)")]
        public string RAWPER { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public int? OLD_CODE { get; set; }

        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS> FYsYarnReceiveDetailses { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS_S> F_YS_YARN_RECEIVE_DETAILS_S { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS2> F_YS_YARN_RECEIVE_DETAILS2 { get; set; }
    }
}
