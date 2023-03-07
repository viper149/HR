using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_IMP_CSITEM_DETAILS
    {
        public COM_IMP_CSITEM_DETAILS()
        {
            COM_IMP_CSRAT_DETAILS = new HashSet<COM_IMP_CSRAT_DETAILS>();
            ComImpCsRatDetailsList = new List<COM_IMP_CSRAT_DETAILS>();
        }
        public int CSITEMID { get; set; }
        public int CSID { get; set; }
        [Display(Name = "Name of Item")]
        public int? ITEMID { get; set; }
        [NotMapped]
        public string ITEMNAME { get; set; }
        [Display(Name = "MU")]
        public string UNIT { get; set; }
        [Display(Name = "Req. Qty.")]
        public double? QTY { get; set; }
        [Display(Name = "Previous Rate")]
        public double? PREVIOUS_RATE { get; set; }
        public DateTime CREATED_AT { get; set; }

        public COM_IMP_CSINFO CS { get; set; }
        [NotMapped]
        public List<COM_IMP_CSRAT_DETAILS> ComImpCsRatDetailsList { get; set; }
        public ICollection<COM_IMP_CSRAT_DETAILS> COM_IMP_CSRAT_DETAILS { get; set; }
    }
}
