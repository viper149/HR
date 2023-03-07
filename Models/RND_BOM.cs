using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_BOM : BaseEntity
    {
        public RND_BOM()
        {
            RND_BOM_MATERIALS_DETAILS = new HashSet<RND_BOM_MATERIALS_DETAILS>();
        }

        public int BOMID { get; set; }
        [DisplayName("Trans Date")]
        public DateTime? TRNSDATE { get; set; }
        [DisplayName("Style Name")]
        public int? FABCODE { get; set; }
        [DisplayName("Finish Type")]
        public int? FINISH_TYPE { get; set; }
        [DisplayName("Color")]
        public int? COLOR { get; set; }
        [DisplayName("Set No.")]
        public int? SETNO { get; set; }
        [DisplayName("Prog No.")]
        public string PROG_NO { get; set; }
        [DisplayName("Construction")]
        public string CONSTRUCTION { get; set; }
        [DisplayName("Finish Weight")]
        public string FINISH_WEIGHT { get; set; }
        [DisplayName("Total Ends")]
        public int? TOTAL_ENDS { get; set; }
        [DisplayName("Lot Ratio")]
        public string LOT_RATIO { get; set; }
        [DisplayName("Width")]
        public string WIDTH { get; set; }
        [DisplayName("Indigo GPL")]
        public string INDIGO_GPL { get; set; }
        [DisplayName("Indigo Box")]
        public string INDIGO_BOX { get; set; }
        [DisplayName("Sulphure GPL")]
        public string SULPHURE_GPL { get; set; }
        [DisplayName("Sulphure Box")]
        public string SULPHURE_BOX { get; set; }
        [DisplayName("Others GPL")]
        public string OTHERS_GPL { get; set; }
        [DisplayName("Others Box")]
        public string OTHERS_BOX { get; set; }
        [DisplayName("Others Remarks")]
        public string OTHERS_REMARKS { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public string OPT8 { get; set; }
        public string OPT7 { get; set; }
        public string OPT6 { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        [NotMapped]
        public String EncryptedId { get; set; }
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }


        public BAS_COLOR COLORNavigation { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        public RND_FINISHTYPE FINISH_TYPENavigation { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SETNONavigation { get; set; }
        public ICollection<RND_BOM_MATERIALS_DETAILS> RND_BOM_MATERIALS_DETAILS { get; set; }
    }
}
