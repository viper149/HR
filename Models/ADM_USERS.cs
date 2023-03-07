namespace DenimERP.Models
{
    public partial class ADM_USERS
    {
        public int USRID { get; set; }
        public string USRNAME { get; set; }
        public string USRFULLNAME { get; set; }
        public string PASSWORD { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public int? DEPTID { get; set; }
        public int? ROLEID { get; set; }
        public byte? STATUS { get; set; }

        public virtual ADM_DEPARTMENT DEPT { get; set; }
        public virtual ADM_ROLES ROLE { get; set; }
    }
}
