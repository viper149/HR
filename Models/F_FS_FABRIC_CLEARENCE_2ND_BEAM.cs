using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_CLEARENCE_2ND_BEAM
    {
        public int AID { get; set; }
        [DisplayName("Trns Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ADATE { get; set; }
        [DisplayName("Trial No")]
        public string TRIAL_NO { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DisplayName("Is Ok?")]
        public bool IS_OK { get; set; }
        [DisplayName("Emp Name")]
        public int? EMPID { get; set; }
        [DisplayName("Trial Type")]
        public int? TTID { get; set; }
        [DisplayName("Order No.")]
        public int? ORDERNO { get; set; }
        [DisplayName("Set No")]
        public int? SETID { get; set; }
        [DisplayName("Beam No")]
        public int? BEAMID { get; set; }
        [DisplayName("Lab Test(G)")]
        public int? LGTEST_ID { get; set; }
        [DisplayName("CSV")]
        public string CSV_GR { get; set; }
        [DisplayName("Lab Test(B)")]
        public int? LBTEST_ID { get; set; }
        [DisplayName("Finish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? FINISHDATE { get; set; }
        [DisplayName("Trolly No")]
        public int? TROLLY_NO { get; set; }
        [DisplayName("Appearance")]
        public string APPEARANCE { get; set; }
        [DisplayName("CSV")]
        public string CSV { get; set; }
        [DisplayName("Curling Porten.")]
        public string CURLING { get; set; }
        [DisplayName("Hand Feel")]
        public string HAND_FEEL { get; set; }
        [DisplayName("Pass Fail")]
        public bool IS_PASS { get; set; }
        [DisplayName("Pass Fail Comments")]
        public string PASS_FAIL_COMMENTS { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_OV { get; set; }
        [DisplayName("Bulk")]
        public string BULK_OV { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_OV_AW { get; set; }
        [DisplayName("Bulk")]
        public string BULK_OV_AW { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_CUT { get; set; }
        [DisplayName("Bulk")]
        public string BULK_CUT { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_WG_UW { get; set; }
        [DisplayName("Bulk")]
        public string BULK_WG_UW { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_WG_AW { get; set; }
        [DisplayName("Bulk")]
        public string BULK_WG_AW { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_U_EPI { get; set; }
        [DisplayName("Bulk")]
        public string BULK_U_EPI { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_A_EPI { get; set; }
        [DisplayName("Bulk")]
        public string BULK_A_EPI { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_U_PPI { get; set; }
        [DisplayName("Bulk")]
        public string BULK_U_PPI { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_A_PPI { get; set; }
        [DisplayName("Bulk")]
        public string BULK_A_PPI { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_SR_WARP { get; set; }
        [DisplayName("Bulk")]
        public string BULK_SR_WARP { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_SR_WEFT { get; set; }
        [DisplayName("Bulk")]
        public string BULK_SR_WEFT { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_BS { get; set; }
        [DisplayName("Bulk")]
        public string BULK_BS { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_ST_WARP { get; set; }
        [DisplayName("Bulk")]
        public string BULK_ST_WARP { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_ST_WEFT { get; set; }
        [DisplayName("Bulk")]
        public string BULK_ST_WEFT { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_GR_WARP { get; set; }
        [DisplayName("Bulk")]
        public string BULK_GR_WARP { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_GR_WEFT { get; set; }
        [DisplayName("Bulk")]
        public string BULK_GR_WEFT { get; set; }


        [DisplayName("Sample")]
        public string SAMPLE_U_SKEW { get; set; }
        [DisplayName("Bulk")]
        public string BULK_U_SKEW { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_A_SKEW { get; set; }
        [DisplayName("Bulk")]
        public string BULK_A_SKEW { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_SKEW { get; set; }
        [DisplayName("Bulk")]
        public string BULK_SKEW { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_SPIRALITY_A { get; set; }
        [DisplayName("Bulk")]
        public string BULK_SPIRALITY_A { get; set; }
        [DisplayName("Sample")]
        public string SAMPLE_SPIRALITY_B { get; set; }
        [DisplayName("Bulk")]
        public string BULK_SPIRALITY_B { get; set; }

        [DisplayName("Finish M/C")]
        public string FN_MC_FI { get; set; }
        [DisplayName("Fin M/C Name")]
        public string FN_MC_NAME { get; set; }
        [DisplayName("Follow Route")]
        public string FOLLOW_ROUTE { get; set; }
        [DisplayName("Finishing")]
        public string FN_COMMENTS { get; set; }
        [DisplayName("Dyeing")]
        public string DYEING_COMMENTS { get; set; }
        [DisplayName("Quality")]
        public string QUALITY_COMMENTS { get; set; }
        [DisplayName("GM")]
        public string GM_COMMENTS { get; set; }
        [DisplayName("Head Of Denim")]
        public string HP_COMMENTS { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        [DisplayName("Beam Serial")]
        public string OPT1 { get; set; }
        [DisplayName("Dyeing Serial")]
        public string OPT2 { get; set; }
        [DisplayName("Roll Report Test No.")]
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string OPT6 { get; set; }
        [NotMapped]
        public string OPT7 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string FN_COMMENTS_FN_PARA { get; set; }
        public string FN_COMMENTS_FN_OTHERS { get; set; }
        public string FN_COMMENTS_FN_SIGNATURE { get; set; }
        public string DYEING_COMMENTS_FN_PARA { get; set; }
        public string DYEING_COMMENTS_FN_OTHERS { get; set; }
        public bool DYEING_COMMENTS_FN_SIGNATURE { get; set; }
        public string QUALITY_COMMENTS_FN_PARA { get; set; }
        public string QUALITY_COMMENTS_FN_OTHERS { get; set; }
        public bool QUALITY_COMMENTS_FN_SIGNATURE { get; set; }
        public string GM_COMMENTS_FN_PARA { get; set; }
        public string GM_COMMENTS_FN_OTHERS { get; set; }
        public bool GM_COMMENTS_FN_SIGNATURE { get; set; }
        public string HP_COMMENTS_FN_PARA { get; set; }
        public string HP_COMMENTS_FN_OTHERS { get; set; }
        public bool HP_COMMENTS_FN_SIGNATURE { get; set; }

        public F_PR_WEAVING_PROCESS_DETAILS_B BEAM { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }
        public RND_FABTEST_BULK LBTEST_ { get; set; }
        public RND_FABTEST_GREY LGTEST_ { get; set; }
        public RND_PRODUCTION_ORDER ORDERNONavigation { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public F_PR_FIN_TROLLY TROLLY_NONavigation { get; set; }
        public F_FS_FABRIC_TYPE TT { get; set; }
    }
}
