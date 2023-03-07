using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class LOOM_SETTING_STYLE_WISE_M
    {


        public LOOM_SETTING_STYLE_WISE_M()
        {
            LOOM_SETTING_CHANNEL_INFO = new HashSet<LOOM_SETTING_CHANNEL_INFO>();
        }

        [Display(Name = "Setting Name")]
        public int SETTING_ID { get; set; }
        [Display(Name = "Style")]
        public int? FABCODE { get; set; }
        [Display(Name = "RPM")]
        public int? RPM { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOM_TYPE { get; set; }
        [Display(Name = "Filter Value")]
        public int? FILTER_VALUE { get; set; }
        [Display(Name = "2nd Roll Position")]
        public string SECOND_ROLL_TYPE { get; set; }
        [Display(Name = "2nd Roll Clamp Use")]
        public string SECOND_ROLL_CLAMP_USE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [Display(Name = "Backrest Height (cm)")]
        public string BACKREST_HEIGHT { get; set; }
        [Display(Name = "Backrest Depth (cm)")]
        public string BACKREST_DEPTH { get; set; }
        [Display(Name = "D.Box Height (cm)")]
        public string DBOX_HEIGHT { get; set; }
        [Display(Name = "D.Box Depth (cm)")]
        public string DBOX_DEPTH { get; set; }
        [Display(Name = "Shed angle (degree)")]
        public string SHED_ANGLE { get; set; }
        [Display(Name = "Frame height (mm)")]
        public string FRAME_HEIGHT { get; set; }
        [Display(Name = "Shed crossing (degree)")]
        public string SHED_CROSSING { get; set; }
        [Display(Name = "Warp Tension (kn)")]
        public string WARP_TENSION { get; set; }
        [Display(Name = "Stop (Filling)")]
        public string STOP_FILLING { get; set; }
        [Display(Name = "Stop (Other)")]
        public string STOP_OTHER { get; set; }
        [Display(Name = "Mode (AB)")]
        public string MODE_FILLING { get; set; }
        [Display(Name = "Mode (AB)")]
        public string MODE_OTHER { get; set; }
        [Display(Name = "AMP (Filling)")]
        public string AMP_FILLING { get; set; }
        [Display(Name = "Amp (Other)")]
        public string AMP_OTHER { get; set; }
        [Display(Name = "Mode (BS)")]
        public string MODE_FILLING_START { get; set; }
        [Display(Name = "Mode (BS)")]
        public string MODE_OTHER_START { get; set; }
        [Display(Name = "ASP (Filling)")]
        public string ASP_FILLING { get; set; }
        [Display(Name = "ASP (Other)")]
        public string ASP_OTHER { get; set; }
        [Display(Name = "ASO (Filling)")]
        public string ASO_FILLING { get; set; }
        [Display(Name = "ASO (Other)")]
        public string ASO_OTHER { get; set; }
        [Display(Name = "PFR (Filling)")]
        public string PFR_FILLING { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Selvedge Weave")]
        public string OPT2 { get; set; }
        [Display(Name = "Selvedge Width")]
        public string OPT1 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        [Display(Name = "B/CMPX WARP")]
        public double? BREAKS_CMPX_WARP { get; set; }
        [Display(Name = "B/CMPX WEFT")]
        public double? BREAKS_CMPX_WEFT { get; set; }
        [Display(Name = "B/CMPX CATCH CORD")]
        public double? BREAKS_CMPX_CATCH_CORD { get; set; }
        [Display(Name = "B/CMPX OTHERS")]
        public double? BREAKS_CMPX_OTHERS_BREAKS { get; set; }
        [Display(Name = "B/CMPX TOTAL")]
        public double? BREAKS_CMPX_TOTAL { get; set; }
        [Display(Name = "Warp Crimp(%)")]
        public double? WARP_CRIMP { get; set; }
        [Display(Name = "Weft Crimp(%)")]
        public double? WEFT_CRIMP { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }


        public RND_FABRICINFO FABCODENavigation { get; set; }
        public LOOM_SETTINGS_FILTER_VALUE FILTER_VALUENavigation { get; set; }
        public LOOM_TYPE LOOM_TYPENavigation { get; set; }
        public ICollection<LOOM_SETTING_CHANNEL_INFO> LOOM_SETTING_CHANNEL_INFO { get; set; }








        //public LOOM_SETTING_STYLE_WISE_M()
        //{
        //    LOOM_SETTING_CHANNEL_INFO = new HashSet<LOOM_SETTING_CHANNEL_INFO>();
        //}

        //public string OPT5 { get; set; }
        //public string OPT4 { get; set; }
        //public string OPT3 { get; set; }
        //public string OPT2 { get; set; }
        //public string OPT1 { get; set; }
        //public string CREATED_BY { get; set; }
        //public DateTime? CREATED_AT { get; set; }
        //public string UPDATED_BY { get; set; }
        //public DateTime? UPDATED_AT { get; set; }

        //public RND_FABRICINFO FABCODENavigation { get; set; }
        //public LOOM_SETTINGS_FILTER_VALUE FILTER_VALUENavigation { get; set; }
        //public LOOM_TYPE LOOM_TYPENavigation { get; set; }
        //public ICollection<LOOM_SETTING_CHANNEL_INFO> LOOM_SETTING_CHANNEL_INFO { get; set; }
    }
}
