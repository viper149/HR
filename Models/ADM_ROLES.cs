using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class ADM_ROLES
    {
        public ADM_ROLES()
        {
            ADM_USERS = new HashSet<ADM_USERS>();
        }

        public int ROLEID { get; set; }
        public string ROLENAME { get; set; }
        public string STATUS { get; set; }

        public virtual ICollection<ADM_USERS> ADM_USERS { get; set; }
    }
}
