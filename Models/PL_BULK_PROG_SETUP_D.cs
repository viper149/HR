using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class PL_BULK_PROG_SETUP_D
    {
        public PL_BULK_PROG_SETUP_D()
        {
            PL_BULK_PROG_YARN_D = new HashSet<PL_BULK_PROG_YARN_D>();
            PlBulkProgYarnDList = new List<PL_BULK_PROG_YARN_D>();
            PL_PRODUCTION_SETDISTRIBUTION = new HashSet<PL_PRODUCTION_SETDISTRIBUTION>();
        }

        public int PROG_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Set Date")]
        public DateTime? SET_DATE { get; set; }
        public int? BLK_PROG_ID { get; set; }
        //[Remote(action: "IsProgNoInUse", controller: "PlBulkProgSetup")]
        [Display(Name = "Program/Set No.")]
        public string PROG_NO { get; set; }
        [Display(Name = "Set Quantity")]
        public double SET_QTY { get; set; }
        [Display(Name = "Program Type")]
        public string PROGRAM_TYPE { get; set; }
        [Display(Name = "Process Type")]
        public string PROCESS_TYPE { get; set; }
        [Display(Name = "Warp Type")]
        public string WARP_TYPE { get; set; }
        [Display(Name = "Yarn Type")]
        public int? YARN_TYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [NotMapped]
        public string OPT5 { get; set; }
        [Display(Name="Auto Create Plan?")]
        public bool IS_AUTO_CREATE_PLAN { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public PL_BULK_PROG_SETUP_M BLK_PROG_ { get; set; }
        public YARNFOR YARNFOR { get; set; }
        [NotMapped]
        public List<PL_BULK_PROG_YARN_D> PlBulkProgYarnDList { get; set; }
        public ICollection<PL_BULK_PROG_YARN_D> PL_BULK_PROG_YARN_D { get; set; }
        public ICollection<PL_PRODUCTION_SETDISTRIBUTION> PL_PRODUCTION_SETDISTRIBUTION { get; set; }
    }
}
