using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_DELIVERYCHALLAN_PACK_MASTER : BaseEntity
    {
        public F_FS_DELIVERYCHALLAN_PACK_MASTER()
        {
            F_FS_DELIVERYCHALLAN_PACK_DETAILS = new HashSet<F_FS_DELIVERYCHALLAN_PACK_DETAILS>();
            FFsDeliveryChallanPackDetailsList = new List<F_FS_DELIVERYCHALLAN_PACK_DETAILS>();
        }

        public int D_CHALLANID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Delivery Challan Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? D_CHALLANDATE { get; set; }
        [Display(Name = "DO No.")]
        public int? DOID { get; set; }
        [Display(Name = "PI Id")]
        public int? PIID { get; set; }
        [Display(Name = "Order No.")]
        public int? SO_NO { get; set; }
        [Display(Name = "Vehicle Id")]
        public int? VEHICLENO { get; set; }
        [Display(Name = "Delivery Chalan No")]
        public string DC_NO { get; set; }
        [Display(Name = "Gate Pass No")]
        public string GP_NO { get; set; }
        [Display(Name = "Driver")]
        public string DRIVER { get; set; }
        [Display(Name = "Lock Number")]
        public string LOCKNO { get; set; }
        [Display(Name = "Delivery Type Id")]
        public int DELIVERY_TYPE { get; set; }
        [Display(Name = "Audit By")]
        public int? AUDITBY { get; set; }
        public DateTime? AUDIT_ON { get; set; }
        [Display(Name = "Audit Comments")]
        public string AUDIT_COMMENTS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Issue Type")]
        public int ISSUE_TYPE { get; set; }
        [Display(Name = "Buyer Name")]
        public int? BUYERID { get; set; }
        [Display(Name = "Sales Contact No.")]
        public int? SCID { get; set; }
        [Display(Name = "DO No.")]
        public int? DOID_LOCAL { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public int? OPT5 { get; set; }

        [NotMapped]
        public double? DO_BALANCE { get; set; }
        [NotMapped]
        public double? PI_BALANCE { get; set; }
        [NotMapped]
        public double? SO_BALANCE { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_HRD_EMPLOYEE AUDITBYNavigation { get; set; }
        public F_BAS_DELIVERY_TYPE DELIVERY_TYPENavigation { get; set; }
        public ACC_EXPORT_DOMASTER DO { get; set; }
        public COM_EX_PIMASTER PI { get; set; }
        public COM_EX_PI_DETAILS SO_NONavigation { get; set; }
        public F_BAS_VEHICLE_INFO VEHICLENONavigation { get; set; }
        public F_FS_ISSUE_TYPE ISSUE_TYPENavigation { get; set; }
        public ACC_LOCAL_DOMASTER DO_LOCAL { get; set; }
        public COM_EX_SCINFO SC { get; set; }

        public ICollection<F_FS_DELIVERYCHALLAN_PACK_DETAILS> F_FS_DELIVERYCHALLAN_PACK_DETAILS { get; set; }
        [NotMapped]
        public List<F_FS_DELIVERYCHALLAN_PACK_DETAILS> FFsDeliveryChallanPackDetailsList { get; set; }
    }
}
