using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public class COS_PRECOSTING_DETAILS
    {
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int CSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        public int FABCODE { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Yarn For")]
        public int? YARN_FOR { get; set; }
        [Display(Name = "YPB")]
        public string YPB { get; set; }
        //[NotMapped]
        //public string COUNTNAME { get; set; }
        [Display(Name = "R&D Consumption")]
        public double? RND_CONSUMP { get; set; }
        [Display(Name = "Cost Consumption")]
        public double? COS_CONSUMP { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Value")]
        public double? VALUE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CREATED_AT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? UPDATED_AT { get; set; }
        public string USERID { get; set; }
        [Display(Name = "Author")]
        [NotMapped]
        public string USERNAME { get; set; }
        [NotMapped]
        public string Lot { get; set; }

        public COS_PRECOSTING_MASTER CosPrecostingMaster { get; set; }
        public BAS_YARN_COUNTINFO BasYarnCountInfo { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        //public BAS_SUPPLIERINFO YPBNavigation { get; set; }
        public YARNFOR Yarnfor { get; set; }
    }
}