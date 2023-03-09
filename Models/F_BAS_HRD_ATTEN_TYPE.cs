using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_ATTEN_TYPE : BaseEntity
    {
        public int ATTYPID { get; set; }
        public string ATTYPID_NAME { get; set; }
        public string ATTYPID_DESC { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
    }
}
