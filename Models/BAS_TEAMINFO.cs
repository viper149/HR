using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_TEAMINFO
    {
        public BAS_TEAMINFO()
        {
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
            MKT_SDRF_INFO_M = new HashSet<MKT_SDRF_INFO>();
            MKT_SDRF_INFO_R = new HashSet<MKT_SDRF_INFO>();
            H_SAMPLE_TEAM_DETAILS = new HashSet<H_SAMPLE_TEAM_DETAILS>();
            MKT_SWATCH_CARD = new HashSet<MKT_SWATCH_CARD>();
            MKT_TEAM = new HashSet<MKT_TEAM>();
            H_SAMPLE_DESPATCH_M = new HashSet<H_SAMPLE_DESPATCH_M>();
        }

        public int TEAMID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Team Name")]
        [Remote(action: "IsTeamNameInUse", controller: "BasicTeamInfo")]
        public string TEAM_NAME { get; set; }
        [Display(Name = "Department")]
        public int DEPTID { get; set; }
        public string REMARKS { get; set; }

        public ADM_DEPARTMENT DEPT { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO_M { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO_R { get; set; }
        public ICollection<H_SAMPLE_TEAM_DETAILS> H_SAMPLE_TEAM_DETAILS { get; set; }
        public ICollection<MKT_SWATCH_CARD> MKT_SWATCH_CARD { get; set; }
        public ICollection<MKT_TEAM> MKT_TEAM { get; set; }
        public ICollection<H_SAMPLE_DESPATCH_M> H_SAMPLE_DESPATCH_M { get; set; }
    }
}
