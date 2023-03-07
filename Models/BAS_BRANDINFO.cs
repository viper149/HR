using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_BRANDINFO
    {
        public BAS_BRANDINFO()
        {
            COM_EX_FABSTYLE = new HashSet<COM_EX_FABSTYLE>();
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
            H_SAMPLE_DESPATCH_M = new HashSet<H_SAMPLE_DESPATCH_M>();
            RND_ANALYSIS_SHEET = new HashSet<RND_ANALYSIS_SHEET>();
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
            F_SAMPLE_FABRIC_ISSUE = new HashSet<F_SAMPLE_FABRIC_ISSUE>();
            F_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            H_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<H_SAMPLE_FABRIC_DISPATCH_MASTER>();
        }

        public int BRANDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Brand Name")]
        [Remote(action: "IsBrandNameInUse", controller: "BasicBrandInfo")]
        public string BRANDNAME { get; set; }
        [Display(Name = "Brand Code")]
        public decimal? BRANDCODE { get; set; }
        public string REMARKS { get; set; }

        public ICollection<COM_EX_FABSTYLE> COM_EX_FABSTYLE { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public ICollection<H_SAMPLE_DESPATCH_M> H_SAMPLE_DESPATCH_M { get; set; }
        public ICollection<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
        public ICollection<F_SAMPLE_FABRIC_ISSUE> F_SAMPLE_FABRIC_ISSUE { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_FABRIC_DISPATCH_MASTER> H_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
    }
}
