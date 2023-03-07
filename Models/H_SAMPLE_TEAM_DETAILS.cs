using System;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_TEAM_DETAILS
    {
        public int HSTID { get; set; }
        public int? TEAMID { get; set; }
        public string TEAMMEMBER { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public BAS_TEAMINFO TEAM { get; set; }
    }
}
