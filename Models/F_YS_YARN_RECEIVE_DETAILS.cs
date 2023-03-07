using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_DETAILS : BaseEntity
    {
        public F_YS_YARN_RECEIVE_DETAILS()
        {
            F_YARN_QC_APPROVE = new HashSet<F_YARN_QC_APPROVE>();
            F_YS_YARN_RECEIVE_REPORT = new HashSet<F_YS_YARN_RECEIVE_REPORT>();
            F_YARN_TRANSACTION = new HashSet<F_YARN_TRANSACTION>();
            F_QA_YARN_TEST_INFORMATION_COTTON = new HashSet<F_QA_YARN_TEST_INFORMATION_COTTON>();
            F_QA_YARN_TEST_INFORMATION_POLYESTERS = new HashSet<F_QA_YARN_TEST_INFORMATION_POLYESTER>();
            F_YS_YARN_ISSUE_DETAILS = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            F_YS_GP_DETAILS = new HashSet<F_YS_GP_DETAILS>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Yarn Receive No.")]
        public int? YRCVID { get; set; }
        [Display(Name = "Raw(%)")]
        public int? RAW { get; set; }
        [Display(Name = "Count Name")]
        public int? PRODID { get; set; }
        [Display(Name = "Challan Quantity")]
        public double? INV_QTY { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOT { get; set; }
        [Display(Name = "Bag Quantity")]
        public double? BAG_QTY { get; set; }
        [Display(Name = "Kg Quantity")]
        public double? RCV_QTY { get; set; }
        [Display(Name = "Reject Quantity")]
        public double? REJ_QTY { get; set; }
        [Display(Name = "Location")]
        public int? LOCATIONID { get; set; }
        [Display(Name = "Ledger")]
        public int? LEDGERID { get; set; }
        [Display(Name = "Page No.")]
        public int? PAGENO { get; set; }
        [Display(Name = "Warp/Weft")]
        public int? YARN_TYPE { get; set; }
        [Display(Name = "Import Type")]
        public string IMPORT_TYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public int? INDENT_TYPE { get; set; }
        public int? POID { get; set; }
        public int? SDRFID { get; set; }
        [Display(Name = "Bag Quantity")]
        public int? BAG { get; set; }


        public F_YS_LEDGER LEDGER { get; set; }
        public F_YS_LOCATION LOCATION { get; set; }
        public BAS_YARN_COUNTINFO PROD { get; set; }
        public F_YS_YARN_RECEIVE_MASTER YRCV { get; set; }
        public BAS_YARN_LOTINFO BasYarnLotinfo { get; set; }
        public F_YS_RAW_PER FYsRawPer { get; set; }
        public YARNFOR FYarnFor { get; set; }

        public ICollection<F_YARN_QC_APPROVE> F_YARN_QC_APPROVE { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_REPORT> F_YS_YARN_RECEIVE_REPORT { get; set; }
        public ICollection<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
        public ICollection<F_QA_YARN_TEST_INFORMATION_COTTON> F_QA_YARN_TEST_INFORMATION_COTTON { get; set; }
        public ICollection<F_QA_YARN_TEST_INFORMATION_POLYESTER> F_QA_YARN_TEST_INFORMATION_POLYESTERS { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS> F_YS_YARN_ISSUE_DETAILS { get; set; }
        public ICollection<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }
    }
}
