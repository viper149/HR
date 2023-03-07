using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class RND_FABRICINFO : BaseEntity
    {
        public RND_FABRICINFO()
        {
            COM_EX_FABSTYLE = new HashSet<COM_EX_FABSTYLE>();
            COS_PRECOSTING_DETAILS = new HashSet<COS_PRECOSTING_DETAILS>();
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
            RND_FABRIC_COUNTINFO = new HashSet<RND_FABRIC_COUNTINFO>();
            RND_YARNCONSUMPTION = new HashSet<RND_YARNCONSUMPTION>();
            RND_FABRICINFO_APPROVAL_DETAILS = new HashSet<RND_FABRICINFO_APPROVAL_DETAILS>();
            F_SAMPLE_GARMENT_RCV_D = new HashSet<F_SAMPLE_GARMENT_RCV_D>();
            F_FS_FABRIC_RCV_DETAILS = new HashSet<F_FS_FABRIC_RCV_DETAILS>();
            LOOM_SETTING_STYLE_WISE_M = new HashSet<LOOM_SETTING_STYLE_WISE_M>();
            F_PR_FINISHING_BEAM_RECEIVE = new HashSet<F_PR_FINISHING_BEAM_RECEIVE>();
            F_PR_FINISHING_MACHINE_PREPARATION = new HashSet<F_PR_FINISHING_MACHINE_PREPARATION>();
            F_PR_FINISHING_PROCESS_MASTER = new HashSet<F_PR_FINISHING_PROCESS_MASTER>();
            F_FS_FABRIC_CLEARANCE_MASTER = new HashSet<F_FS_FABRIC_CLEARANCE_MASTER>();
            F_SAMPLE_FABRIC_ISSUE_DETAILS = new HashSet<F_SAMPLE_FABRIC_ISSUE_DETAILS>();
            RND_PURCHASE_REQUISITION_MASTER = new HashSet<RND_PURCHASE_REQUISITION_MASTER>();
            F_PR_WEAVING_PRODUCTION = new HashSet<F_PR_WEAVING_PRODUCTION>();
            FSampleFabricRcvDs = new HashSet<F_SAMPLE_FABRIC_RCV_D>();
            RND_BOM = new HashSet<RND_BOM>();
            PL_BULK_PROG_SETUP_M = new HashSet<PL_BULK_PROG_SETUP_M>();
            F_PR_INSPECTION_PROCESS_MASTER = new HashSet<F_PR_INSPECTION_PROCESS_MASTER>();
            F_PR_INSPECTION_FABRIC_D_DETAILS = new HashSet<F_PR_INSPECTION_FABRIC_D_DETAILS>();
            F_FS_FABRIC_RETURN_RECEIVE = new HashSet<F_FS_FABRIC_RETURN_RECEIVE>();
        }

        [Display(Name = "Dev. No")]
        public string DEVID { get; set; }
        [Remote(action: "IsRndFabricInfoFabCodeInUse", controller: "RndFabricInfo")]
        [Display(Name = "Fabric Code")]
        public int FABCODE { get; set; }
        [Display(Name = "Style Name")]
        public string STYLE_NAME { get; set; }
        [Display(Name = "Weave Id")]
        public int? WVID { get; set; }
        [Display(Name = "Finish Id")]
        public int? SFINID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Program No.")]
        public string PROGNO { get; set; }
        [Display(Name = "Dyeing Type")]
        public int? DID { get; set; }
        [Display(Name = "Dyeing Code")]
        public string DYCODE { get; set; }
        [Display(Name = "Reed Count")]
        public double? REED_COUNT { get; set; }
        [Display(Name = "Reed Space")]
        public double? REED_SPACE { get; set; }
        [Display(Name = "No. of Dent")]
        public double? DENT { get; set; }
        //[Required(ErrorMessage = "Color cannot be empty.")]
        [Display(Name = "Shade / Color")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Loom Type")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? LOOMID { get; set; }
        [Display(Name = "Loom Speed")]
        public double? LOOMSPEED { get; set; }
        [Display(Name = "Efficiency")]
        public double? EFFICIENCY { get; set; }
        [Display(Name = "Weave")]
        public int? WID { get; set; }
        [Display(Name = "Dobby Code")]
        public string DOBBY { get; set; }
        [Display(Name = "Total Ends(D)")]
        public double? TOTALENDS { get; set; }
        [Display(Name = "Total Weft")]
        public double? TOTALWEFT { get; set; }
        [Display(Name = "Total Rope")]
        public double? TOTALROPE { get; set; }
        [Display(Name = "Pick Length")]
        public double? PICKLENGHT { get; set; }
        [Display(Name = "Fabric Type")]
        public string FABRICTYPE { get; set; }
        [Display(Name = "Composition Act.")]
        public string COMPOSITION { get; set; }
        [Display(Name = "Composition Pro.")]
        public string COMPOSITION_PRO { get; set; }
        [Display(Name = "Finish Route")]
        public string FINISH_ROUTE { get; set; }
        [Display(Name = "Concept")]
        public string CONCEPT { get; set; }
        [Display(Name = "Finish Type")]
        public int? FINID { get; set; }
        [Display(Name = "Finish M/C")]
        public int? MCID { get; set; }
        [Display(Name = "Meeting Remarks")]
        public string MEETING { get; set; }
        [Display(Name = "Buyer")]
        public int? BUYERID { get; set; }
        [Display(Name = "Master Roll Set No.")]
        public string MASTERROLL { get; set; }
        [Display(Name = "Buyer Ref.")]
        public string BUYERREF { get; set; }
        [Display(Name = "Revise No.")]
        public string REVISENO { get; set; }
        [Display(Name = "Lab Test(F) No")]
        public string LSBTESTNO { get; set; }
        [Display(Name = "Lab Test(F) No.")]
        public int? LTSID { get; set; }
        [Display(Name = "EPI(Gr.)")]
        public double? GREPI { get; set; }
        [Display(Name = "PPI(Gr.)")]
        public double? GRPPI { get; set; }                       
        [Display(Name = "EPI(Fn.)")]
        public double? FNEPI { get; set; }                       
        [Display(Name = "PPI(Fn.)")]
        public double? FNPPI { get; set; }                       
        [Display(Name = "EPI(AW)")]
        public double? AWEPI { get; set; }                       
        [Display(Name = "PPI(AW)")]
        public double? AWPPI { get; set; }                       
        [Display(Name = "Gr. BW")]
        public double? WGGRBW { get; set; }                      
        [Display(Name = "Gr. AW")]
        public double? WGGRAW { get; set; }                      
        [Display(Name = "Finish BW")]
        public double? WGFNBW { get; set; }                      
        [Display(Name = "Finish AW")]
        public double? WGFNAW { get; set; }                      
        [Display(Name = "Declaration")]
        public string WGDEC { get; set; }
        [Display(Name = "Greige BW")]
        public double? WIGRBW { get; set; }
        [Display(Name = "Greige AW")]
        public double? WIGRAW { get; set; }
        [Display(Name = "Finish BW")]
        public double? WIFNBW { get; set; }                                            
        [Display(Name = "Finish Cutable")]
        public double? WIFNCUT { get; set; }
        [Display(Name = "Finish AW")]
        public double? WIFNAW { get; set; }                                            
        [Display(Name = "Declaration")]
        public string WIDEC { get; set; }                                              
        [Display(Name = "Greige Warp")]
        public double? SRGRWARP { get; set; }                                          
        [Display(Name = "Grieige Weft")]
        public double? SRGRWEFT { get; set; }                                          
        [Display(Name = "Finish Warp")]
        public double? SRFNWARP { get; set; }                                          
        [Display(Name = "Finish Weft")]
        public double? SRFNWEFT { get; set; }                                          
        [Display(Name = "Declr. Warp")]
        public string SRDECWARP { get; set; }                                          
        [Display(Name = "Declr. Weft")]
        public string SRDECWEFT { get; set; }
        [Display(Name = "Greige Warp")]
        public double? STGRWARP { get; set; }
        [Display(Name = "Grieige Weft")]
        public double? STGRWEFT { get; set; }
        [Display(Name = "Finish Warp")]
        public double? STFNWARP { get; set; }
        [Display(Name = "Finish Weft")]
        public double? STFNWEFT { get; set; }
        [Display(Name = "Declr. Warp")]
        public string STDECWARP { get; set; }
        [Display(Name = "Declr. Weft")]
        public string STDECWEFT { get; set; }
        [Display(Name = "Finish Warp")]
        public double? GRFNWARP { get; set; }
        [Display(Name = "Finish Weft")]
        public double? GRFNWEFT { get; set; }
        [Display(Name = "Declr. Warp")]
        public string GRDECWARP { get; set; }
        [Display(Name = "Declr. Weft")]
        public string GRDECWEFT { get; set; }
        [Display(Name = "Crimp%")]
        public double? CRIMP_PERCENTAGE { get; set; }
        [Display(Name = "Skew Move.")]
        public string SKEWMOVE { get; set; }                                           
        [Display(Name = "Slippage Warp")]
        public double? SLPWARP { get; set; }                                           
        [Display(Name = "Slippage Weft")]
        public double? SLPWEFT { get; set; }                                           
        [Display(Name = "Tensile Warp")]
        public double? TNWARPFN { get; set; }                                            
        [Display(Name = "Tensile Weft")]
        public double? TNWEFTFN { get; set; }                                            
        [Display(Name = "Tear Warp")]
        public double? TRWARP { get; set; }                                            
        [Display(Name = "Tear Weft")]
        public double? TRWEFT { get; set; }                                            
        [Display(Name = "Color Fat. Dry")]
        public string CFATDRY { get; set; }                                            
        [Display(Name = "Color Fat. Wet")]
        public string CFATWET { get; set; }
        [Display(Name = "PH")]
        public string PH { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [Display(Name = "Lab Test(G) No")]
        public string LSGTESTNO { get; set; }
        [Display(Name = "Archive No")]
        public string ARCHIVE_NO { get; set; }
        [Display(Name = "Lab Test(Protocol) No")]
        public string PROTOCOL_NO { get; set; }
        [Display(Name = "Composition Dec.")]
        public string COMPOSITION_DEC { get; set; }
        [Display(Name = "Total Ends(S)")]
        public int? TOTAL_ENDS_SAMPLE { get; set; }
        [Display(Name = "EPI(BW)")]
        public double? BWEPI { get; set; }
        [Display(Name = "PPI(BW)")]
        public double? BWPPI { get; set; }
        [DisplayName("Is Declare?")]
        public bool APPROVED { get; set; }
        [DisplayName("Is Delete?")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "Method")]
        public string METHOD { get; set; }
        public int? SDRF { get; set; }
        public string RS { get; set; }
        [Display(Name = "Updated By")]
        public int? UPD_BY { get; set; }
        [Display(Name = "Contraction")]
        public double? CONTRACTION { get; set; }
        [Display(Name = "Ends / Rope")]
        public double? ENDS { get; set; }
        [Display(Name = "Weft Ratio")]
        public string OPT3 { get; set; }
        [Display(Name = "Warp Ratio")]
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public string FINISH_METHOD { get; set; }
        public string PROTOCOL_METHOD { get; set; }
        public string SPR_A_FIN { get; set; }
        public string SPR_B_FIN { get; set; }
        public string SPR_A_DEC { get; set; }
        public string SPR_B_DEC { get; set; }
        public string SKEW_FN { get; set; }
        public string TNWARP { get; set; }
        public string TNWEFT { get; set; }
        public string SLPWARPFN { get; set; }
        public string SLPWEFTFN { get; set; }
        public string RUBDRYFN { get; set; }
        public string RUBDRYDEC { get; set; }
        public string RUBWETFN { get; set; }



        public string PROBWEPI { get; set; }
        public string PROBWPPI { get; set; }
        public string WGBWPRO { get; set; }
        public string WGAWPRO { get; set; }
        public string WIBWPRO { get; set; }
        public string WICUTPRO { get; set; }
        public string WIAWPRO { get; set; }
        public string SRWARPPRO { get; set; }
        public string SRWEFTPRO { get; set; }
        public string STWARPPRO { get; set; }
        public string STWEFTPRO { get; set; }
        public string GRWARPPRO { get; set; }
        public string GRWEFTPRO { get; set; }
        public string SPR_A_PRO { get; set; }
        public string SPR_B_PRO { get; set; }
        public string SKEW_PRO { get; set; }
        public string TNWARPPRO { get; set; }
        public string TNWEFTPRO { get; set; }
        public string TEARWARPPRO { get; set; }
        public string TEARWEFTPRO { get; set; }
        public string SLIWARPPRO { get; set; }
        public string SLIWEFTPRO { get; set; }
        public string RUBDRYPRO { get; set; }
        public string RUBWET { get; set; }
        public string RUBWETPRO { get; set; }
        public string PHPRO { get; set; }
        public string PHFN { get; set; }
        public string COMPOSITIONPRO { get; set; }
        public string PROAWEPI { get; set; }
        public string PROAWPPI { get; set; }
        public string TRWARPFN { get; set; }
        public string TRWEFTFN { get; set; }

        public BAS_BUYERINFO BUYER { get; set; }
        public BAS_COLOR COLORCODENavigation { get; set; }
        public RND_DYEING_TYPE D { get; set; }
        public LOOM_TYPE LOOM { get; set; }
        public RND_FINISHTYPE RND_FINISHTYPE { get; set; }
        public RND_WEAVE RND_WEAVE { get; set; }
        public RND_FINISHMC RND_FINISHMC { get; set; }
        public RND_SAMPLE_INFO_WEAVING WV { get; set; }
        public RND_SAMPLEINFO_FINISHING FIN { get; set; }
        public RND_FABTEST_SAMPLE FABTEST { get; set; }
        public F_HRD_EMPLOYEE UPD_BYNavigation { get; set; }

        public ICollection<RND_FABRICINFO_APPROVAL_DETAILS> RND_FABRICINFO_APPROVAL_DETAILS { get; set; }
        public ICollection<COM_EX_FABSTYLE> COM_EX_FABSTYLE { get; set; }
        public ICollection<COS_PRECOSTING_DETAILS> COS_PRECOSTING_DETAILS { get; set; }
        public ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
        public ICollection<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public ICollection<RND_YARNCONSUMPTION> RND_YARNCONSUMPTION { get; set; }
        public ICollection<F_SAMPLE_GARMENT_RCV_D> F_SAMPLE_GARMENT_RCV_D { get; set; }
        public ICollection<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
        public ICollection<LOOM_SETTING_STYLE_WISE_M> LOOM_SETTING_STYLE_WISE_M { get; set; }
        public ICollection<F_PR_FINISHING_BEAM_RECEIVE> F_PR_FINISHING_BEAM_RECEIVE { get; set; }
        public ICollection<F_PR_FINISHING_MACHINE_PREPARATION> F_PR_FINISHING_MACHINE_PREPARATION { get; set; }
        public ICollection<F_PR_FINISHING_PROCESS_MASTER> F_PR_FINISHING_PROCESS_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_CLEARANCE_MASTER> F_FS_FABRIC_CLEARANCE_MASTER { get; set; }
        public ICollection<F_SAMPLE_FABRIC_ISSUE_DETAILS> F_SAMPLE_FABRIC_ISSUE_DETAILS { get; set; }
        public ICollection<RND_PURCHASE_REQUISITION_MASTER> RND_PURCHASE_REQUISITION_MASTER { get; set; }
        public ICollection<F_PR_WEAVING_PRODUCTION> F_PR_WEAVING_PRODUCTION { get; set; }
        public ICollection<F_SAMPLE_FABRIC_RCV_D> FSampleFabricRcvDs { get; set; }
        public ICollection<RND_BOM> RND_BOM { get; set; }
        public ICollection<PL_BULK_PROG_SETUP_M> PL_BULK_PROG_SETUP_M { get; set; }
        public ICollection<F_PR_INSPECTION_PROCESS_MASTER> F_PR_INSPECTION_PROCESS_MASTER { get; set; }
        public ICollection<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }
        public ICollection<F_FS_FABRIC_RETURN_RECEIVE> F_FS_FABRIC_RETURN_RECEIVE { get; set; }

    }
}
