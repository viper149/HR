using DenimERP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.Data
{
    public partial class DenimDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DenimDbContext(DbContextOptions<DenimDbContext> options)
            : base(options)
        {
        }

        #region F_HR
        public virtual DbSet<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
        public virtual DbSet<F_BAS_HRD_DEPARTMENT> F_BAS_HRD_DEPARTMENT { get; set; }
        public virtual DbSet<F_BAS_HRD_DESIGNATION> F_BAS_HRD_DESIGNATION { get; set; }
        public virtual DbSet<F_BAS_HRD_SECTION> F_BAS_HRD_SECTION { get; set; }
        public virtual DbSet<F_BAS_HRD_SUB_SECTION> F_BAS_HRD_SUB_SECTION { get; set; }
        public virtual DbSet<F_BAS_HRD_GRADE> F_BAS_HRD_GRADE { get; set; }
        public virtual DbSet<F_BAS_HRD_LOCATION> F_BAS_HRD_LOCATION { get; set; }
        public virtual DbSet<F_BAS_HRD_ATTEN_TYPE> F_BAS_HRD_ATTEN_TYPE { get; set; }
        public virtual DbSet<F_BAS_HRD_BLOOD_GROUP> F_BAS_HRD_BLOOD_GROUP { get; set; }
        public virtual DbSet<F_BAS_HRD_DISTRICT> F_BAS_HRD_DISTRICT { get; set; }
        public virtual DbSet<F_BAS_HRD_DIVISION> F_BAS_HRD_DIVISION { get; set; }
        public virtual DbSet<F_BAS_HRD_EMP_RELATION> F_BAS_HRD_EMP_RELATION { get; set; }
        public virtual DbSet<F_BAS_HRD_EMP_TYPE> F_BAS_HRD_EMP_TYPE { get; set; }
        public virtual DbSet<F_BAS_HRD_NATIONALITY> F_BAS_HRD_NATIONALITY { get; set; }
        public virtual DbSet<F_BAS_HRD_OUT_REASON> F_BAS_HRD_OUT_REASON { get; set; }
        public virtual DbSet<F_BAS_HRD_RELIGION> F_BAS_HRD_RELIGION { get; set; }
        public virtual DbSet<F_BAS_HRD_SHIFT> F_BAS_HRD_SHIFT { get; set; }
        public virtual DbSet<F_BAS_HRD_THANA> F_BAS_HRD_THANA { get; set; }
        public virtual DbSet<F_BAS_HRD_UNION> F_BAS_HRD_UNION { get; set; }
        public virtual DbSet<F_BAS_HRD_WEEKEND> F_BAS_HRD_WEEKEND { get; set; }
        public virtual DbSet<F_HRD_EDUCATION> F_HRD_EDUCATION { get; set; }
        public virtual DbSet<F_HRD_EMERGENCY> F_HRD_EMERGENCY { get; set; }
        public virtual DbSet<F_HRD_EMP_CHILDREN> F_HRD_EMP_CHILDREN { get; set; }
        public virtual DbSet<F_HRD_EMP_EDU_DEGREE> F_HRD_EMP_EDU_DEGREE { get; set; }
        public virtual DbSet<F_HRD_EMP_SPOUSE> F_HRD_EMP_SPOUSE { get; set; }
        public virtual DbSet<F_HRD_EXPERIENCE> F_HRD_EXPERIENCE { get; set; }
        public virtual DbSet<F_HRD_INCREMENT> F_HRD_INCREMENT { get; set; }
        public virtual DbSet<F_HRD_PROMOTION> F_HRD_PROMOTION { get; set; }
        public virtual DbSet<F_HRD_SKILL> F_HRD_SKILL { get; set; }
        public virtual DbSet<F_HRD_SKILL_LEVEL> F_HRD_SKILL_LEVEL { get; set; }
        public virtual DbSet<BAS_GENDER> BAS_GENDER { get; set; }
        #endregion

        public virtual DbSet<F_FS_FABRIC_LOADING_BILL> F_FS_FABRIC_LOADING_BILL { get; set; }
        public virtual DbSet<ACC_EXPORT_DODETAILS> ACC_EXPORT_DODETAILS { get; set; }
        public virtual DbSet<ACC_EXPORT_DOMASTER> ACC_EXPORT_DOMASTER { get; set; }
        public virtual DbSet<ACC_EXPORT_REALIZATION> ACC_EXPORT_REALIZATION { get; set; }
        public virtual DbSet<ACC_LOCAL_DODETAILS> ACC_LOCAL_DODETAILS { get; set; }
        public virtual DbSet<ACC_LOCAL_DOMASTER> ACC_LOCAL_DOMASTER { get; set; }
        public virtual DbSet<ACC_LOAN_MANAGEMENT_M> ACC_LOAN_MANAGEMENT_M { get; set; }
        public virtual DbSet<ACC_LOAN_MANAGEMENT_D> ACC_LOAN_MANAGEMENT_D { get; set; }
        public virtual DbSet<ADM_DEPARTMENT> ADM_DEPARTMENT { get; set; }
        public virtual DbSet<ADM_DESIGNATION> ADM_DESIGNATION { get; set; }
        public virtual DbSet<AGEGROUP> AGEGROUP { get; set; }
        public virtual DbSet<AGEGROUPRNDFABRICS> AGEGROUPRNDFABRICS { get; set; }

        //F_BAS_ASSET_LIST
        public virtual DbSet<F_BAS_ASSET_LIST> F_BAS_ASSET_LIST { get; set; }

        //POST COSTING
        public virtual DbSet<COS_POSTCOSTING_CHEMDETAILS> COS_POSTCOSTING_CHEMDETAILS { get; set; }
        public virtual DbSet<COS_POSTCOSTING_MASTER> COS_POSTCOSTING_MASTER { get; set; }
        public virtual DbSet<COS_POSTCOSTING_YARNDETAILS> COS_POSTCOSTING_YARNDETAILS { get; set; }

        //RND BOM
        public virtual DbSet<RND_BOM> RND_BOM { get; set; }
        public virtual DbSet<RND_BOM_MATERIALS_DETAILS> RND_BOM_MATERIALS_DETAILS { get; set; }

        //F_PR_RECONE_MASTER, F_PR_RECONE_YARN_CONSUMPTION, F_PR_RECONE_YARN_DETAILS
        public virtual DbSet<F_PR_RECONE_MASTER> F_PR_RECONE_MASTER { get; set; }
        public virtual DbSet<F_PR_RECONE_YARN_CONSUMPTION> F_PR_RECONE_YARN_CONSUMPTION { get; set; }
        public virtual DbSet<F_PR_RECONE_YARN_DETAILS> F_PR_RECONE_YARN_DETAILS { get; set; }

        //Proc Bill Master & Details
        public virtual DbSet<PROC_BILL_DETAILS> PROC_BILL_DETAILS { get; set; }
        public virtual DbSet<PROC_BILL_MASTER> PROC_BILL_MASTER { get; set; }

        //Proc WorkOrder Master & Details
        public virtual DbSet<PROC_WORKORDER_DETAILS> PROC_WORKORDER_DETAILS { get; set; }
        public virtual DbSet<PROC_WORKORDER_MASTER> PROC_WORKORDER_MASTER { get; set; }



        public virtual DbSet<VEHICLE_TYPE> VEHICLE_TYPE { get; set; }


        #region KEEP THIS COMMENTED
        //public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        //public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        #endregion

        public virtual DbSet<AspNetUserTypes> AspNetUserTypes { get; set; }

        public virtual DbSet<BAS_BEN_BANK_MASTER> BAS_BEN_BANK_MASTER { get; set; }
        public virtual DbSet<BAS_BRANDINFO> BAS_BRANDINFO { get; set; }
        public virtual DbSet<BAS_BUYER_BANK_MASTER> BAS_BUYER_BANK_MASTER { get; set; }
        public virtual DbSet<BAS_BUYERINFO> BAS_BUYERINFO { get; set; }
        public virtual DbSet<BAS_COLOR> BAS_COLOR { get; set; }
        public virtual DbSet<BAS_INSURANCEINFO> BAS_INSURANCEINFO { get; set; }
        public virtual DbSet<BAS_YARN_CATEGORY> BAS_YARN_CATEGORY { get; set; }
        public virtual DbSet<BAS_PRODCATEGORY> BAS_PRODCATEGORY { get; set; }
        public virtual DbSet<BAS_PRODUCTINFO> BAS_PRODUCTINFO { get; set; }
        public virtual DbSet<BAS_PRODUCTINFO1> BAS_PRODUCTINFO1 { get; set; }
        public virtual DbSet<BAS_SUPP_CATEGORY> BAS_SUPP_CATEGORY { get; set; }
        public virtual DbSet<BAS_SUPPLIERINFO> BAS_SUPPLIERINFO { get; set; }
        public virtual DbSet<BAS_TEAMINFO> BAS_TEAMINFO { get; set; }
        public virtual DbSet<BAS_TRANSPORTINFO> BAS_TRANSPORTINFO { get; set; }
        public virtual DbSet<BAS_YARN_COUNTINFO> BAS_YARN_COUNTINFO { get; set; }
        public virtual DbSet<BAS_YARN_COUNT_LOT_INFO> BAS_YARN_COUNT_LOT_INFO { get; set; }
        public virtual DbSet<BAS_YARN_LOTINFO> BAS_YARN_LOTINFO { get; set; }
        public virtual DbSet<COM_EX_BOEXOPTION> COM_EX_BOEXOPTION { get; set; }
        public virtual DbSet<COM_EX_FABSTYLE> COM_EX_FABSTYLE { get; set; }
        public virtual DbSet<COM_EX_INVDETAILS> COM_EX_INVDETAILS { get; set; }
        public virtual DbSet<COM_EX_INVOICEMASTER> COM_EX_INVOICEMASTER { get; set; }
        public virtual DbSet<COM_EX_LCDETAILS> COM_EX_LCDETAILS { get; set; }
        public virtual DbSet<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
        public virtual DbSet<COM_EX_PI_DETAILS> COM_EX_PI_DETAILS { get; set; }
        public virtual DbSet<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public virtual DbSet<COM_EX_SCDETAILS> COM_EX_SCDETAILS { get; set; }
        public virtual DbSet<COM_EX_SCINFO> COM_EX_SCINFO { get; set; }
        public virtual DbSet<COM_IMP_INVDETAILS> COM_IMP_INVDETAILS { get; set; }
        public virtual DbSet<COM_IMP_INVOICEINFO> COM_IMP_INVOICEINFO { get; set; }
        public virtual DbSet<COM_IMP_LCDETAILS> COM_IMP_LCDETAILS { get; set; }
        public virtual DbSet<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
        public virtual DbSet<COM_IMP_DEL_STATUS> COM_IMP_DEL_STATUS { get; set; }
        public virtual DbSet<COM_TENOR> COM_TENOR { get; set; }
        public virtual DbSet<COM_TRADE_TERMS> COM_TRADE_TERMS { get; set; }
        public virtual DbSet<RND_DYEING_TYPE> RND_DYEING_TYPE { get; set; }
        public virtual DbSet<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public virtual DbSet<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public virtual DbSet<RND_FABRICINFO_APPROVAL_DETAILS> RND_FABRICINFO_APPROVAL_DETAILS { get; set; }
        public virtual DbSet<RND_FINISHTYPE> RND_FINISHTYPE { get; set; }
        public virtual DbSet<RND_SAMPLE_INFO_DYEING> RND_SAMPLE_INFO_DYEING { get; set; }
        public virtual DbSet<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public virtual DbSet<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
        public virtual DbSet<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public virtual DbSet<RND_FABTEST_GREY> RND_FABTEST_GREY { get; set; }
        public virtual DbSet<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }

        //COM_EX_CERTIFICATE

        public virtual DbSet<COM_EX_CERTIFICATE_MANAGEMENT> COM_EX_CERTIFICATE_MANAGEMENT { get; set; }

        public virtual DbSet<RND_SAMPLEINFO_FINISHING> RND_SAMPLEINFO_FINISHING { get; set; }
        public virtual DbSet<RND_YARNCONSUMPTION> RND_YARNCONSUMPTION { get; set; }
        public virtual DbSet<RND_WEAVE> RND_WEAVE { get; set; }
        public virtual DbSet<RND_FINISHMC> RND_FINISHMC { get; set; }
        public virtual DbSet<COM_EX_CASHINFO> COM_EX_CASHINFO { get; set; }
        public virtual DbSet<COM_EX_GSPINFO> COM_EX_GSPINFO { get; set; }
        public virtual DbSet<COM_IMP_LCTYPE> COM_IMP_LCTYPE { get; set; }
        public virtual DbSet<COS_CERTIFICATION_COST> COS_CERTIFICATION_COST { get; set; }
        public virtual DbSet<COS_FIXEDCOST> COS_FIXEDCOST { get; set; }
        public virtual DbSet<COS_STANDARD_CONS> COS_STANDARD_CONS { get; set; }
        public virtual DbSet<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
        public virtual DbSet<COS_PRECOSTING_DETAILS> COS_PRECOSTING_DETAILS { get; set; }
        public virtual DbSet<F_BAS_DEPARTMENT> F_BAS_DEPARTMENT { get; set; }
        public virtual DbSet<F_BAS_DESIGNATION> F_BAS_DESIGNATION { get; set; }
        public virtual DbSet<F_BAS_SECTION> F_BAS_SECTION { get; set; }
        public virtual DbSet<F_BAS_SUBSECTION> F_BAS_SUBSECTION { get; set; }
        public virtual DbSet<F_HR_BLOOD_GROUP> F_HR_BLOOD_GROUP { get; set; }
        public virtual DbSet<F_HR_EMP_EDUCATION> F_HR_EMP_EDUCATION { get; set; }
        public virtual DbSet<F_HR_EMP_FAMILYDETAILS> F_HR_EMP_FAMILYDETAILS { get; set; }
        public virtual DbSet<F_HR_EMP_OFFICIALINFO> F_HR_EMP_OFFICIALINFO { get; set; }
        public virtual DbSet<F_HR_EMP_SALARYSETUP> F_HR_EMP_SALARYSETUP { get; set; }
        public virtual DbSet<DISTRICTS> DISTRICTS { get; set; }
        public virtual DbSet<DIVISIONS> DIVISIONS { get; set; }
        public virtual DbSet<UPOZILAS> UPOZILAS { get; set; }
        public virtual DbSet<MKT_DEV_TYPE> MKT_DEV_TYPE { get; set; }
        public virtual DbSet<MKT_FACTORY> MKT_FACTORY { get; set; }
        public virtual DbSet<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
        public virtual DbSet<MKT_SUPPLIER> MKT_SUPPLIER { get; set; }
        public virtual DbSet<MKT_SWATCH_CARD> MKT_SWATCH_CARD { get; set; }
        public virtual DbSet<MKT_TEAM> MKT_TEAM { get; set; }
        public virtual DbSet<PDL_EMAIL_SENDER> PDL_EMAIL_SENDER { get; set; }
        public virtual DbSet<YARNFOR> YARNFOR { get; set; }
        public virtual DbSet<YARNFROM> YARNFROM { get; set; }
        public virtual DbSet<COM_IMP_CSINFO> COM_IMP_CSINFO { get; set; }
        public virtual DbSet<COM_IMP_CSITEM_DETAILS> COM_IMP_CSITEM_DETAILS { get; set; }
        public virtual DbSet<COM_IMP_CSRAT_DETAILS> COM_IMP_CSRAT_DETAILS { get; set; }
        public virtual DbSet<PL_SAMPLE_PROG_SETUP> PL_SAMPLE_PROG_SETUP { get; set; }
        public virtual DbSet<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
        public virtual DbSet<RND_ANALYSIS_SHEET_DETAILS> RND_ANALYSIS_SHEET_DETAILS { get; set; }
        public virtual DbSet<RND_PURCHASE_REQUISITION_MASTER> RND_PURCHASE_REQUISITION_MASTER { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_DETAILS> F_YS_YARN_RECEIVE_DETAILS { get; set; }
        public virtual DbSet<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public virtual DbSet<F_YS_INDENT_MASTER> F_YS_INDENT_MASTER { get; set; }
        public virtual DbSet<F_YS_RAW_PER> F_YS_RAW_PER { get; set; }
        public virtual DbSet<F_YS_SLUB_CODE> F_YS_SLUB_CODE { get; set; }
        public virtual DbSet<MESSAGE> MESSAGE { get; set; }
        public virtual DbSet<MESSAGE_INDIVIDUAL> MESSAGE_INDIVIDUAL { get; set; }
        public virtual DbSet<LOOM_TYPE> LOOM_TYPE { get; set; }
        public virtual DbSet<PL_ORDERWISE_LOTINFO> PL_ORDERWISE_LOTINFO { get; set; }
        public virtual DbSet<RND_MSTR_ROLL> RND_MSTR_ROLL { get; set; }
        public virtual DbSet<RND_ORDER_REPEAT> RND_ORDER_REPEAT { get; set; }
        public virtual DbSet<RND_ORDER_TYPE> RND_ORDER_TYPE { get; set; }
        public virtual DbSet<RND_PRODUCTION_ORDER> RND_PRODUCTION_ORDER { get; set; }
        public virtual DbSet<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
        public virtual DbSet<F_YARN_REQ_MASTER> F_YARN_REQ_MASTER { get; set; }
        public virtual DbSet<F_BAS_ISSUE_TYPE> F_BAS_ISSUE_TYPE { get; set; }
        public virtual DbSet<F_BAS_RECEIVE_TYPE> F_BAS_RECEIVE_TYPE { get; set; }
        public virtual DbSet<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
        public virtual DbSet<F_YS_YARN_ISSUE_DETAILS> F_YS_YARN_ISSUE_DETAILS { get; set; }
        public virtual DbSet<F_YS_YARN_ISSUE_MASTER> F_YS_YARN_ISSUE_MASTER { get; set; }
        public virtual DbSet<PL_BULK_PROG_SETUP_D> PL_BULK_PROG_SETUP_D { get; set; }
        public virtual DbSet<PL_BULK_PROG_SETUP_M> PL_BULK_PROG_SETUP_M { get; set; }
        public virtual DbSet<PL_BULK_PROG_YARN_D> PL_BULK_PROG_YARN_D { get; set; }
        public virtual DbSet<PL_PRODUCTION_PLAN_DETAILS> PL_PRODUCTION_PLAN_DETAILS { get; set; }
        public virtual DbSet<PL_PRODUCTION_PLAN_MASTER> PL_PRODUCTION_PLAN_MASTER { get; set; }
        public virtual DbSet<PL_PRODUCTION_SETDISTRIBUTION> PL_PRODUCTION_SETDISTRIBUTION { get; set; }
        public virtual DbSet<PL_PRODUCTION_SO_DETAILS> PL_PRODUCTION_SO_DETAILS { get; set; }
        public virtual DbSet<PL_DYEING_MACHINE_TYPE> PL_DYEING_MACHINE_TYPE { get; set; }
        public virtual DbSet<SEGMENTOTHERSIMILARNAME> SEGMENTOTHERSIMILARNAME { get; set; }
        public virtual DbSet<SEGMENTOTHERSIMILARRNDFABRICS> SEGMENTOTHERSIMILARRNDFABRICS { get; set; }
        public virtual DbSet<SEGMENTSEASON> SEGMENTSEASON { get; set; }
        public virtual DbSet<SEGMENTSEASONRNDFABRICS> SEGMENTSEASONRNDFABRICS { get; set; }
        public virtual DbSet<TARGETCHARACTER> TARGETCHARACTER { get; set; }
        public virtual DbSet<TARGETCHARACTERRNDFABRICS> TARGETCHARACTERRNDFABRICS { get; set; }
        public virtual DbSet<TARGETFITSTYLE> TARGETFITSTYLE { get; set; }
        public virtual DbSet<TARGETFITSTYLERNDFABRICS> TARGETFITSTYLERNDFABRICS { get; set; }
        public virtual DbSet<TARGETGENDER> TARGETGENDER { get; set; }
        public virtual DbSet<TARGETGENDERRNDFABRICS> TARGETGENDERRNDFABRICS { get; set; }
        public virtual DbSet<TARGETPRICESEGMENT> TARGETPRICESEGMENT { get; set; }
        public virtual DbSet<TARGETPRICESEGMENTRNDFABRICS> TARGETPRICESEGMENTRNDFABRICS { get; set; }
        public virtual DbSet<SEGMENTCOMSEGMENT> SEGMENTCOMSEGMENT { get; set; }
        public virtual DbSet<SEGMENTCOMSEGMENTRNDFABRICS> SEGMENTCOMSEGMENTRNDFABRICS { get; set; }
        public virtual DbSet<F_YARN_QC_APPROVE> F_YARN_QC_APPROVE { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_REPORT> F_YS_YARN_RECEIVE_REPORT { get; set; }
        public virtual DbSet<F_YS_LOCATION> F_YS_LOCATION { get; set; }
        public virtual DbSet<F_YS_LEDGER> F_YS_LEDGER { get; set; }

        public virtual DbSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_ROPE_DETAILS> F_PR_WARPING_PROCESS_ROPE_DETAILS { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_ROPE_MASTER> F_PR_WARPING_PROCESS_ROPE_MASTER { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS { get; set; }
        public virtual DbSet<F_PR_WARPING_MACHINE> F_PR_WARPING_MACHINE { get; set; }
        //F_PR_WEAVING_WORKLOAD_EFFECIENCYLOOS
        public virtual DbSet<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS> F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS { get; set; }

        //F_PR_WEAVING_PRODUCTION
        public virtual DbSet<F_PR_WEAVING_PRODUCTION> F_PR_WEAVING_PRODUCTION { get; set; }

        //F_QA_FIRST_MTR_ANALYSIS_M & B
        public virtual DbSet<F_QA_FIRST_MTR_ANALYSIS_D> F_QA_FIRST_MTR_ANALYSIS_D { get; set; }
        public virtual DbSet<F_QA_FIRST_MTR_ANALYSIS_M> F_QA_FIRST_MTR_ANALYSIS_M { get; set; }

        //SW MASTER, DETAILS, CONSUM_DETAILS
        public virtual DbSet<F_PR_WARPING_PROCESS_SW_DETAILS> F_PR_WARPING_PROCESS_SW_DETAILS { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_SW_MASTER> F_PR_WARPING_PROCESS_SW_MASTER { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS { get; set; }

        //ECRU MASTER, DETAILS, CONSUM_DETAILS
        public virtual DbSet<F_PR_WARPING_PROCESS_ECRU_DETAILS> F_PR_WARPING_PROCESS_ECRU_DETAILS { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_ECRU_MASTER> F_PR_WARPING_PROCESS_ECRU_MASTER { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS { get; set; }

        //Direct Warping
        public virtual DbSet<F_PR_WARPING_PROCESS_DW_DETAILS> F_PR_WARPING_PROCESS_DW_DETAILS { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_DW_MASTER> F_PR_WARPING_PROCESS_DW_MASTER { get; set; }
        public virtual DbSet<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS { get; set; }

        public virtual DbSet<F_SAMPLE_GARMENT_RCV_D> F_SAMPLE_GARMENT_RCV_D { get; set; }
        public virtual DbSet<F_SAMPLE_GARMENT_RCV_M> F_SAMPLE_GARMENT_RCV_M { get; set; }
        public virtual DbSet<F_SAMPLE_ITEM_DETAILS> F_SAMPLE_ITEM_DETAILS { get; set; }
        public virtual DbSet<F_SAMPLE_LOCATION> F_SAMPLE_LOCATION { get; set; }
        public virtual DbSet<F_CHEM_PURCHASE_REQUISITION_MASTER> F_CHEM_PURCHASE_REQUISITION_MASTER { get; set; }
        public virtual DbSet<F_CHEM_STORE_INDENTDETAILS> F_CHEM_STORE_INDENTDETAILS { get; set; }
        public virtual DbSet<F_CHEM_STORE_INDENTMASTER> F_CHEM_STORE_INDENTMASTER { get; set; }
        public virtual DbSet<F_CHEM_STORE_PRODUCTINFO> F_CHEM_STORE_PRODUCTINFO { get; set; }
        public virtual DbSet<F_BAS_BALL_INFO> F_BAS_BALL_INFO { get; set; }
        public virtual DbSet<COM_IMP_CNFINFO> COM_IMP_CNFINFO { get; set; }
        public virtual DbSet<F_CHEM_STORE_RECEIVE_DETAILS> F_CHEM_STORE_RECEIVE_DETAILS { get; set; }
        public virtual DbSet<F_CHEM_STORE_RECEIVE_MASTER> F_CHEM_STORE_RECEIVE_MASTER { get; set; }
        public virtual DbSet<F_CHEM_TRANSECTION> F_CHEM_TRANSECTION { get; set; }

        //DYEING(ROPE)
        public virtual DbSet<F_DYEING_PROCESS_ROPE_CHEM> F_DYEING_PROCESS_ROPE_CHEM { get; set; }
        public virtual DbSet<F_DYEING_PROCESS_ROPE_DETAILS> F_DYEING_PROCESS_ROPE_DETAILS { get; set; }
        public virtual DbSet<F_DYEING_PROCESS_ROPE_MASTER> F_DYEING_PROCESS_ROPE_MASTER { get; set; }

        //DYEING(SLASHER)
        public virtual DbSet<F_PR_SLASHER_CHEM_CONSM> F_PR_SLASHER_CHEM_CONSM { get; set; }
        public virtual DbSet<F_PR_SLASHER_DYEING_DETAILS> F_PR_SLASHER_DYEING_DETAILS { get; set; }
        public virtual DbSet<F_PR_SLASHER_DYEING_MASTER> F_PR_SLASHER_DYEING_MASTER { get; set; }
        public virtual DbSet<F_PR_SLASHER_MACHINE_INFO> F_PR_SLASHER_MACHINE_INFO { get; set; }



        public virtual DbSet<F_CS_CHEM_RECEIVE_REPORT> F_CS_CHEM_RECEIVE_REPORT { get; set; }
        public virtual DbSet<F_CHEM_QC_APPROVE> F_CHEM_QC_APPROVE { get; set; }
        public virtual DbSet<F_CHEM_REQ_DETAILS> F_CHEM_REQ_DETAILS { get; set; }
        public virtual DbSet<F_CHEM_REQ_MASTER> F_CHEM_REQ_MASTER { get; set; }
        public virtual DbSet<F_CHEM_ISSUE_DETAILS> F_CHEM_ISSUE_DETAILS { get; set; }
        public virtual DbSet<F_CHEM_ISSUE_MASTER> F_CHEM_ISSUE_MASTER { get; set; }

        public virtual DbSet<F_PR_ROPE_INFO> F_PR_ROPE_INFO { get; set; }
        public virtual DbSet<F_PR_ROPE_MACHINE_INFO> F_PR_ROPE_MACHINE_INFO { get; set; }
        public virtual DbSet<F_PR_TUBE_INFO> F_PR_TUBE_INFO { get; set; }

        public virtual DbSet<F_LCB_BEAM> F_LCB_BEAM { get; set; }
        public virtual DbSet<F_LCB_MACHINE> F_LCB_MACHINE { get; set; }
        public virtual DbSet<F_LCB_PRODUCTION_ROPE_DETAILS> F_LCB_PRODUCTION_ROPE_DETAILS { get; set; }
        public virtual DbSet<F_LCB_PRODUCTION_ROPE_MASTER> F_LCB_PRODUCTION_ROPE_MASTER { get; set; }
        public virtual DbSet<F_LCB_PRODUCTION_ROPE_PROCESS_INFO> F_LCB_PRODUCTION_ROPE_PROCESS_INFO { get; set; }
        public virtual DbSet<F_PR_INSPECTION_CONSTRUCTION> F_PR_INSPECTION_CONSTRUCTION { get; set; }
        public virtual DbSet<F_PR_INSPECTION_REMARKS> F_PR_INSPECTION_REMARKS { get; set; }


        public virtual DbSet<F_PR_SIZING_PROCESS_ROPE_CHEM> F_PR_SIZING_PROCESS_ROPE_CHEM { get; set; }
        public virtual DbSet<F_PR_SIZING_PROCESS_ROPE_DETAILS> F_PR_SIZING_PROCESS_ROPE_DETAILS { get; set; }
        public virtual DbSet<F_PR_SIZING_PROCESS_ROPE_MASTER> F_PR_SIZING_PROCESS_ROPE_MASTER { get; set; }
        public virtual DbSet<F_SIZING_MACHINE> F_SIZING_MACHINE { get; set; }
        public virtual DbSet<F_WEAVING_BEAM> F_WEAVING_BEAM { get; set; }
        public virtual DbSet<F_BAS_UNITS> F_BAS_UNITS { get; set; }
        public virtual DbSet<COUNTRIES> COUNTRIES { get; set; }

        public virtual DbSet<F_LOOM_MACHINE_NO> F_LOOM_MACHINE_NO { get; set; }
        public virtual DbSet<F_PR_WEAVING_BEAM_RECEIVING> F_PR_WEAVING_BEAM_RECEIVING { get; set; }
        public virtual DbSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> F_PR_WEAVING_PROCESS_BEAM_DETAILS_B { get; set; }
        public virtual DbSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_S> F_PR_WEAVING_PROCESS_BEAM_DETAILS_S { get; set; }
        public virtual DbSet<F_PR_WEAVING_PROCESS_DETAILS_B> F_PR_WEAVING_PROCESS_DETAILS_B { get; set; }
        public virtual DbSet<F_PR_WEAVING_PROCESS_DETAILS_S> F_PR_WEAVING_PROCESS_DETAILS_S { get; set; }
        public virtual DbSet<F_PR_WEAVING_PROCESS_MASTER_B> F_PR_WEAVING_PROCESS_MASTER_B { get; set; }
        public virtual DbSet<F_PR_WEAVING_PROCESS_MASTER_S> F_PR_WEAVING_PROCESS_MASTER_S { get; set; }
        public virtual DbSet<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS { get; set; }
        public virtual DbSet<F_PR_WEAVING_OTHER_DOFF> F_PR_WEAVING_OTHER_DOFF { get; set; }
        public virtual DbSet<F_CHEM_TYPE> F_CHEM_TYPE { get; set; }


        public virtual DbSet<F_PR_FIN_TROLLY> F_PR_FIN_TROLLY { get; set; }
        public virtual DbSet<F_PR_FINISHING_BEAM_RECEIVE> F_PR_FINISHING_BEAM_RECEIVE { get; set; }
        public virtual DbSet<F_PR_FINISHING_FAB_PROCESS> F_PR_FINISHING_FAB_PROCESS { get; set; }
        public virtual DbSet<F_PR_FINISHING_FNPROCESS> F_PR_FINISHING_FNPROCESS { get; set; }
        public virtual DbSet<F_PR_FINISHING_PROCESS_MASTER> F_PR_FINISHING_PROCESS_MASTER { get; set; }
        public virtual DbSet<F_PR_FN_CHEMICAL_CONSUMPTION> F_PR_FN_CHEMICAL_CONSUMPTION { get; set; }
        public virtual DbSet<F_PR_FN_MACHINE_INFO> F_PR_FN_MACHINE_INFO { get; set; }
        public virtual DbSet<F_PR_FN_PROCESS_TYPEINFO> F_PR_FN_PROCESS_TYPEINFO { get; set; }
        public virtual DbSet<F_PR_PROCESS_MACHINEINFO> F_PR_PROCESS_MACHINEINFO { get; set; }
        public virtual DbSet<F_PR_PROCESS_TYPE_INFO> F_PR_PROCESS_TYPE_INFO { get; set; }
        public virtual DbSet<BAS_YARN_PARTNO> BAS_YARN_PARTNO { get; set; }
        public virtual DbSet<F_PR_FINIGHING_DOFF_FOR_MACHINE> F_PR_FINIGHING_DOFF_FOR_MACHINE { get; set; }
        public virtual DbSet<F_PR_FINISHING_MACHINE_PREPARATION> F_PR_FINISHING_MACHINE_PREPARATION { get; set; }

        public virtual DbSet<BAS_SEASON> BAS_SEASON { get; set; }

        // Sample Garments
        public virtual DbSet<F_SAMPLE_DESPATCH_MASTER> F_SAMPLE_DESPATCH_MASTER { get; set; }
        public virtual DbSet<F_SAMPLE_DESPATCH_DETAILS> F_SAMPLE_DESPATCH_DETAILS { get; set; }
        public virtual DbSet<GATEPASS_TYPE> GATEPASS_TYPE { get; set; }
        public virtual DbSet<F_BAS_DRIVERINFO> F_BAS_DRIVERINFO { get; set; }
        public virtual DbSet<F_BAS_VEHICLE_INFO> F_BAS_VEHICLE_INFO { get; set; }

        public virtual DbSet<H_SAMPLE_RECEIVING_D> H_SAMPLE_RECEIVING_D { get; set; }
        public virtual DbSet<H_SAMPLE_RECEIVING_M> H_SAMPLE_RECEIVING_M { get; set; }

        //General Store
        public virtual DbSet<F_GS_ITEMCATEGORY> F_GS_ITEMCATEGORY { get; set; }
        public virtual DbSet<F_GS_ITEMSUB_CATEGORY> F_GS_ITEMSUB_CATEGORY { get; set; }
        public virtual DbSet<F_GS_PRODUCT_INFORMATION> F_GS_PRODUCT_INFORMATION { get; set; }
        public virtual DbSet<F_GATEPASS_TYPE> F_GATEPASS_TYPE { get; set; }
        public virtual DbSet<F_GS_GATEPASS_INFORMATION_D> F_GS_GATEPASS_INFORMATION_D { get; set; }
        public virtual DbSet<F_GS_GATEPASS_INFORMATION_M> F_GS_GATEPASS_INFORMATION_M { get; set; }
        public virtual DbSet<F_GS_RETURNABLE_GP_RCV_D> F_GS_RETURNABLE_GP_RCV_D { get; set; }
        public virtual DbSet<F_GS_RETURNABLE_GP_RCV_M> F_GS_RETURNABLE_GP_RCV_M { get; set; }

        //Inspection
        public virtual DbSet<F_PR_INSPECTION_BATCH> F_PR_INSPECTION_BATCH { get; set; }
        public virtual DbSet<F_PR_INSPECTION_DEFECT_POINT> F_PR_INSPECTION_DEFECT_POINT { get; set; }
        public virtual DbSet<F_PR_INSPECTION_DEFECTINFO> F_PR_INSPECTION_DEFECTINFO { get; set; }
        public virtual DbSet<F_PR_INSPECTION_MACHINE> F_PR_INSPECTION_MACHINE { get; set; }
        public virtual DbSet<F_PR_INSPECTION_PROCESS_DETAILS> F_PR_INSPECTION_PROCESS_DETAILS { get; set; }
        public virtual DbSet<F_PR_INSPECTION_PROCESS_MASTER> F_PR_INSPECTION_PROCESS_MASTER { get; set; }
        public virtual DbSet<F_PR_INSPECTION_PROCESS> F_PR_INSPECTION_PROCESS { get; set; }
        public virtual DbSet<F_PR_INSPECTION_WASTAGE_TRANSFER> F_PR_INSPECTION_WASTAGE_TRANSFER { get; set; }
        public virtual DbSet<F_PR_INSPECTION_REJECTION_B> F_PR_INSPECTION_REJECTION_B { get; set; }

        //F_PR_INSPECTION_CUTPCS_TRANSFER
        public virtual DbSet<F_HR_SHIFT_INFO> F_HR_SHIFT_INFO { get; set; }
        public virtual DbSet<F_PR_INSPECTION_CUTPCS_TRANSFER> F_PR_INSPECTION_CUTPCS_TRANSFER { get; set; }


        //F_GS_WASTAGE
        public virtual DbSet<F_GS_WASTAGE_PARTY> F_GS_WASTAGE_PARTY { get; set; }
        public virtual DbSet<F_WASTE_PRODUCTINFO> F_WASTE_PRODUCTINFO { get; set; }
        public virtual DbSet<F_GS_WASTAGE_RECEIVE_D> F_GS_WASTAGE_RECEIVE_D { get; set; }
        public virtual DbSet<F_GS_WASTAGE_RECEIVE_M> F_GS_WASTAGE_RECEIVE_M { get; set; }
        public virtual DbSet<F_GS_WASTAGE_ISSUE_D> F_GS_WASTAGE_ISSUE_D { get; set; }
        public virtual DbSet<F_GS_WASTAGE_ISSUE_M> F_GS_WASTAGE_ISSUE_M { get; set; }


        //Fabric Store

        public virtual DbSet<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
        public virtual DbSet<F_FS_FABRIC_RCV_MASTER> F_FS_FABRIC_RCV_MASTER { get; set; }
        public virtual DbSet<F_FS_LOCATION> F_FS_LOCATION { get; set; }
        public virtual DbSet<F_FS_ISSUE_TYPE> F_FS_ISSUE_TYPE { get; set; }
        public virtual DbSet<F_FS_DO_BALANCE_FROM_ORACLE> F_FS_DO_BALANCE_FROM_ORACLE { get; set; }
        public virtual DbSet<F_FS_UP_DETAILS> F_FS_UP_DETAILS { get; set; }
        public virtual DbSet<F_FS_UP_MASTER> F_FS_UP_MASTER { get; set; }
        //Fabric Wastage
        public virtual DbSet<F_FS_WASTAGE_RECEIVE_D> F_FS_WASTAGE_RECEIVE_D { get; set; }
        public virtual DbSet<F_FS_WASTAGE_RECEIVE_M> F_FS_WASTAGE_RECEIVE_M { get; set; }
        public virtual DbSet<F_FS_WASTAGE_ISSUE_D> F_FS_WASTAGE_ISSUE_D { get; set; }
        public virtual DbSet<F_FS_WASTAGE_ISSUE_M> F_FS_WASTAGE_ISSUE_M { get; set; }
        public virtual DbSet<F_FS_WASTAGE_PARTY> F_FS_WASTAGE_PARTY { get; set; }

        //Fabric Delivery Challan & Packing List
        public virtual DbSet<F_FS_DELIVERYCHALLAN_PACK_DETAILS> F_FS_DELIVERYCHALLAN_PACK_DETAILS { get; set; }
        public virtual DbSet<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
        public virtual DbSet<F_BAS_DELIVERY_TYPE> F_BAS_DELIVERY_TYPE { get; set; }

        //Fabric Clearance
        public virtual DbSet<F_FS_FABRIC_CLEARANCE_DETAILS> F_FS_FABRIC_CLEARANCE_DETAILS { get; set; }
        public virtual DbSet<F_FS_FABRIC_CLEARANCE_MASTER> F_FS_FABRIC_CLEARANCE_MASTER { get; set; }

        #region DO NOT UNCOMMENT
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Server=192.168.46.101;Database=PDLERP;User Id=sa;Password=p!0n33r@23#");
        //    }
        //}
        #endregion

        public virtual DbSet<H_SAMPLE_DESPATCH_D> H_SAMPLE_DESPATCH_D { get; set; }
        public virtual DbSet<H_SAMPLE_DESPATCH_M> H_SAMPLE_DESPATCH_M { get; set; }
        public virtual DbSet<H_SAMPLE_PARTY> H_SAMPLE_PARTY { get; set; }
        public virtual DbSet<H_SAMPLE_TEAM_DETAILS> H_SAMPLE_TEAM_DETAILS { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<MenuMasterRoles> MenuMasterRoles { get; set; }
        public virtual DbSet<COMPANY_INFO> COMPANY_INFO { get; set; }
        public virtual DbSet<UPAS> UPAS { get; set; }

        //LOOM SETTING STYLE WISE
        public virtual DbSet<LOOM_SETTING_CHANNEL_INFO> LOOM_SETTING_CHANNEL_INFO { get; set; }
        public virtual DbSet<LOOM_SETTING_STYLE_WISE_M> LOOM_SETTING_STYLE_WISE_M { get; set; }
        public virtual DbSet<LOOM_SETTINGS_FILTER_VALUE> LOOM_SETTINGS_FILTER_VALUE { get; set; }
        public virtual DbSet<F_SAMPLE_DESPATCH_MASTER_TYPE> F_SAMPLE_DESPATCH_MASTER_TYPE { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }

        //Loom settings Sample Weaving
        public virtual DbSet<LOOM_SETTINGS_SAMPLE> LOOM_SETTINGS_SAMPLE { get; set; }

        public virtual DbSet<F_QA_YARN_TEST_INFORMATION_COTTON> F_QA_YARN_TEST_INFORMATION_COTTON { get; set; }
        public virtual DbSet<F_QA_YARN_TEST_INFORMATION_POLYESTER> F_QA_YARN_TEST_INFORMATION_POLYESTER { get; set; }
        public virtual DbSet<F_CHEM_STORE_INDENT_TYPE> F_CHEM_STORE_INDENT_TYPE { get; set; }
        public virtual DbSet<COS_WASTAGE_PERCENTAGE> COS_WASTAGE_PERCENTAGE { get; set; }
        public virtual DbSet<RND_FABTEST_BULK> RND_FABTEST_BULK { get; set; }
        public virtual DbSet<F_BAS_TESTMETHOD> F_BAS_TESTMETHOD { get; set; }

        //public virtual DbSet<F_HR_SHIFT_INFO> F_HR_SHIFT_INFO { get; set; }
        public virtual DbSet<RND_FABTEST_SAMPLE_BULK> RND_FABTEST_SAMPLE_BULK { get; set; }

        public virtual DbSet<COM_CONTAINER> COM_CONTAINER { get; set; }
        public virtual DbSet<EXPORTSTATUS> EXPORTSTATUS { get; set; }
        public virtual DbSet<CURRENCY> CURRENCY { get; set; }

        //General Store New
        public virtual DbSet<F_GEN_S_INDENT_TYPE> F_GEN_S_INDENT_TYPE { get; set; }
        public virtual DbSet<F_GEN_S_INDENTDETAILS> F_GEN_S_INDENTDETAILS { get; set; }
        public virtual DbSet<F_GEN_S_INDENTMASTER> F_GEN_S_INDENTMASTER { get; set; }
        public virtual DbSet<F_GEN_S_ISSUE_DETAILS> F_GEN_S_ISSUE_DETAILS { get; set; }
        public virtual DbSet<F_GEN_S_ISSUE_MASTER> F_GEN_S_ISSUE_MASTER { get; set; }
        public virtual DbSet<F_GEN_S_PURCHASE_REQUISITION_MASTER> F_GEN_S_PURCHASE_REQUISITION_MASTER { get; set; }
        public virtual DbSet<F_GEN_S_QC_APPROVE> F_GEN_S_QC_APPROVE { get; set; }
        public virtual DbSet<F_GEN_S_RECEIVE_DETAILS> F_GEN_S_RECEIVE_DETAILS { get; set; }
        public virtual DbSet<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
        public virtual DbSet<F_GEN_S_REQ_DETAILS> F_GEN_S_REQ_DETAILS { get; set; }
        public virtual DbSet<F_GEN_S_REQ_MASTER> F_GEN_S_REQ_MASTER { get; set; }
        public virtual DbSet<F_GS_GATEPASS_RETURN_RCV_DETAILS> F_GS_GATEPASS_RETURN_RCV_DETAILS { get; set; }
        public virtual DbSet<F_GS_GATEPASS_RETURN_RCV_MASTER> F_GS_GATEPASS_RETURN_RCV_MASTER { get; set; }
        public virtual DbSet<F_GEN_S_MRR> F_GEN_S_MRR { get; set; }

        //FS CLEARANCE
        public virtual DbSet<F_FS_CLEARANCE_WASTAGE_ITEM> F_FS_CLEARANCE_WASTAGE_ITEM { get; set; }
        public virtual DbSet<F_FS_CLEARANCE_WASTAGE_TRANSFER> F_FS_CLEARANCE_WASTAGE_TRANSFER { get; set; }
        public virtual DbSet<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL> F_FS_CLEARANCE_MASTER_SAMPLE_ROLL { get; set; }
        public virtual DbSet<F_FS_CLEARANCE_ROLL_TYPE> F_FS_CLEARANCE_ROLL_TYPE { get; set; }
        public virtual DbSet<F_FS_CLEARANCE_WASH_TYPE> F_FS_CLEARANCE_WASH_TYPE { get; set; }

        //F_FS_FABRIC_CLEARENCE_2ND_BEAM
        public virtual DbSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
        public virtual DbSet<F_FS_FABRIC_TYPE> F_FS_FABRIC_TYPE { get; set; }

        //COM IMPORT WORK ORDER
        public virtual DbSet<COM_IMP_WORK_ORDER_DETAILS> COM_IMP_WORK_ORDER_DETAILS { get; set; }
        public virtual DbSet<COM_IMP_WORK_ORDER_MASTER> COM_IMP_WORK_ORDER_MASTER { get; set; }

        //Yarn Store Sample
        public virtual DbSet<F_YARN_QC_APPROVE_S> F_YARN_QC_APPROVE_S { get; set; }
        public virtual DbSet<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }
        public virtual DbSet<F_YARN_REQ_MASTER_S> F_YARN_REQ_MASTER_S { get; set; }
        public virtual DbSet<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
        public virtual DbSet<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S { get; set; }
        public virtual DbSet<F_YS_YARN_ISSUE_MASTER_S> F_YS_YARN_ISSUE_MASTER_S { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_DETAILS_S> F_YS_YARN_RECEIVE_DETAILS_S { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_MASTER_S> F_YS_YARN_RECEIVE_MASTER_S { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_REPORT_S> F_YS_YARN_RECEIVE_REPORT_S { get; set; }
        //H_GS_IT

        public virtual DbSet<H_GS_ITEM_CATEGORY> H_GS_ITEM_CATEGORY { get; set; }
        public virtual DbSet<H_GS_ITEM_SUBCATEGORY> H_GS_ITEM_SUBCATEGORY { get; set; }
        public virtual DbSet<H_GS_PRODUCT> H_GS_PRODUCT { get; set; }

        public virtual DbSet<F_SAMPLE_FABRIC_ISSUE> F_SAMPLE_FABRIC_ISSUE { get; set; }
        public virtual DbSet<F_SAMPLE_FABRIC_ISSUE_DETAILS> F_SAMPLE_FABRIC_ISSUE_DETAILS { get; set; }

        public virtual DbSet<F_SAMPLE_FABRIC_RCV_D> F_SAMPLE_FABRIC_RCV_D { get; set; }
        public virtual DbSet<F_SAMPLE_FABRIC_RCV_M> F_SAMPLE_FABRIC_RCV_M { get; set; }

        public virtual DbSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public virtual DbSet<F_SAMPLE_FABRIC_DISPATCH_MASTER> F_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
        public virtual DbSet<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION> F_SAMPLE_FABRIC_DISPATCH_TRANSACTION { get; set; }
        public virtual DbSet<F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE> F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE { get; set; }

        public virtual DbSet<H_SAMPLE_FABRIC_RECEIVING_D> H_SAMPLE_FABRIC_RECEIVING_D { get; set; }
        public virtual DbSet<H_SAMPLE_FABRIC_RECEIVING_M> H_SAMPLE_FABRIC_RECEIVING_M { get; set; }

        public virtual DbSet<H_SAMPLE_FABRIC_DISPATCH_DETAILS> H_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public virtual DbSet<H_SAMPLE_FABRIC_DISPATCH_MASTER> H_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
        public virtual DbSet<MERCHANDISER> MERCHANDISER { get; set; }

        //F_PR_INSPECTION_FABRIC_Dispatch
        public virtual DbSet<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }
        public virtual DbSet<F_PR_INSPECTION_FABRIC_D_MASTER> F_PR_INSPECTION_FABRIC_D_MASTER { get; set; }

        //adv delivery
        public virtual DbSet<COM_EX_ADV_DELIVERY_SCH_DETAILS> COM_EX_ADV_DELIVERY_SCH_DETAILS { get; set; }
        public virtual DbSet<COM_EX_ADV_DELIVERY_SCH_MASTER> COM_EX_ADV_DELIVERY_SCH_MASTER { get; set; }

        //MAIL BOX
        public virtual DbSet<MAILBOX> MAILBOX { get; set; }

        //Physical Inventory
        public virtual DbSet<ACC_PHYSICAL_INVENTORY_FAB> ACC_PHYSICAL_INVENTORY_FAB { get; set; }

        public virtual DbSet<F_YS_PARTY_INFO> F_YS_PARTY_INFO { get; set; }
        public virtual DbSet<F_YS_GP_MASTER> F_YS_GP_MASTER { get; set; }

        public virtual DbSet<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }

        public virtual DbSet<F_FS_FABRIC_RETURN_RECEIVE> F_FS_FABRIC_RETURN_RECEIVE { get; set; }

        public virtual DbSet<F_PR_WEAVING_OS> F_PR_WEAVING_OS { get; set; }

        public virtual DbSet<F_YARN_TRANSACTION_TYPE> F_YARN_TRANSACTION_TYPE { get; set; }

        public virtual DbSet<F_YS_YARN_RECEIVE_DETAILS2> F_YS_YARN_RECEIVE_DETAILS2 { get; set; }
        public virtual DbSet<F_YS_YARN_RECEIVE_MASTER2> F_YS_YARN_RECEIVE_MASTER2 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ACC_LOAN_MANAGEMENT_M>(entity =>
            {
                entity.HasKey(e => e.LOANID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.EXP_DATE).HasColumnType("datetime");

                entity.Property(e => e.LOANDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.PAID_DATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BANK)
                    .WithMany(p => p.ACC_LOAN_MANAGEMENT_M)
                    .HasForeignKey(d => d.BANKID)
                    .HasConstraintName("FK_ACC_LOAN_MANAGEMENT_M_BAS_BEN_BANK_MASTER");
            });


            //Fabric Wastage Issue
            modelBuilder.Entity<F_FS_WASTAGE_ISSUE_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.WI)
                    .WithMany(p => p.F_FS_WASTAGE_ISSUE_D)
                    .HasForeignKey(d => d.WIID)
                    .HasConstraintName("FK_F_FS_WASTAGE_ISSUE_D_F_FS_WASTAGE_ISSUE_M");

                entity.HasOne(d => d.WP)
                    .WithMany(p => p.F_FS_WASTAGE_ISSUE_D)
                    .HasForeignKey(d => d.WPID)
                    .HasConstraintName("FK_F_FS_WASTAGE_ISSUE_D_F_WASTE_PRODUCTINFO");
            });

            modelBuilder.Entity<F_FS_WASTAGE_ISSUE_M>(entity =>
            {
                entity.HasKey(e => e.WIID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.THROUGH).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WIDATE).HasColumnType("datetime");

                entity.HasOne(d => d.FFsWastageParty)
                    .WithMany(p => p.F_FS_WASTAGE_ISSUE_M)
                    .HasForeignKey(d => d.PID)
                    .HasConstraintName("FK_F_FS_WASTAGE_ISSUE_M_F_FS_WASTAGE_PARTY");
            });
            modelBuilder.Entity<F_FS_WASTAGE_PARTY>(entity =>
            {
                entity.HasKey(e => e.PID);

                entity.Property(e => e.ADDRESS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PHONE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            //F_FS_WASTAGE_RECEIVE_D

            modelBuilder.Entity<F_FS_WASTAGE_RECEIVE_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.WP)
                    .WithMany(p => p.F_FS_WASTAGE_RECEIVE_D)
                    .HasForeignKey(d => d.WPID)
                    .HasConstraintName("FK_F_FS_WASTAGE_RECEIVE_D_F_WASTE_PRODUCTINFO");

                entity.HasOne(d => d.WR)
                    .WithMany(p => p.F_FS_WASTAGE_RECEIVE_D)
                    .HasForeignKey(d => d.WRID)
                    .HasConstraintName("FK_F_FS_WASTAGE_RECEIVE_D_F_FS_WASTAGE_RECEIVE_M");
            });


            // F_FS_WASTAGE_RECEIVE_M
            modelBuilder.Entity<F_FS_WASTAGE_RECEIVE_M>(entity =>
            {
                entity.HasKey(e => e.WRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WRDATE).HasColumnType("datetime");

                entity.Property(e => e.WTRDATE).HasColumnType("datetime");

                entity.Property(e => e.WTRNO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_FS_WASTAGE_RECEIVE_M)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_FS_WASTAGE_RECEIVE_M_F_BAS_SECTION");
            });

            //F_FS_UP
            modelBuilder.Entity<F_FS_UP_DETAILS>(entity =>
            {
                entity.HasKey(e => e.UP_DID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.ComExLcInfo)
                    .WithMany(p => p.F_FS_UP_DETAILS)
                    .HasForeignKey(d => d.LC_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_FS_UP_DETAILS_COM_EX_LCINFO");

                entity.HasOne(d => d.UPMaster)
                    .WithMany(p => p.F_FS_UP_DETAILS)
                    .HasForeignKey(d => d.UP_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_FS_UP_DETAILS_F_FS_UP_MASTER");
            });

            modelBuilder.Entity<F_FS_UP_MASTER>(entity =>
            {
                entity.HasKey(e => e.UP_ID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TYPE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.UP_DATE).HasColumnType("datetime");

                entity.Property(e => e.UP_NO).HasMaxLength(50);
            });

            //F_FS_FABRIC_LOADING_BILL
            modelBuilder.Entity<F_FS_FABRIC_LOADING_BILL>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.END_TIME).HasColumnType("datetime");

                entity.Property(e => e.BILL_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.START_TIME).HasColumnType("datetime");

                entity.Property(e => e.TOTAL_TIME).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.VEHICLE_)
                    .WithMany(p => p.F_FS_FABRIC_LOADING_BILL)
                    .HasForeignKey(d => d.VEHICLE_ID)
                    .HasConstraintName("FK_F_FS_FABRIC_LOADING_BILL_F_BAS_VEHICLE_INFO");
            });

            //Post Costing

            modelBuilder.Entity<COS_POSTCOSTING_CHEMDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CHEM_PROD)
                    .WithMany(p => p.COS_POSTCOSTING_CHEMDETAILS)
                    .HasForeignKey(d => d.CHEM_PRODID)
                    .HasConstraintName("FK_POST_COSTING_CHEMDETAILS_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.PCOST)
                    .WithMany(p => p.COS_POSTCOSTING_CHEMDETAILS)
                    .HasForeignKey(d => d.PCOSTID)
                    .HasConstraintName("FK_PST_COSTING_CHEMDETAILS_COS_POSTCOSTING_MASTER");

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.COS_POSTCOSTING_CHEMDETAILS)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_POST_COSTING_CHEMDETAILS_F_BAS_UNITS");
            });

            modelBuilder.Entity<COS_POSTCOSTING_MASTER>(entity =>
            {
                entity.HasKey(e => e.PCOSTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS).IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.RndProductionOrder)
                    .WithMany(p => p.COS_POSTCOSTING_MASTER)
                    .HasForeignKey(d => d.SO_NO)
                    .HasConstraintName("FK_POST_COSTING_MASTER_RND_PRODUCTION_ORDER");
            });

            modelBuilder.Entity<COS_POSTCOSTING_YARNDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.COS_POSTCOSTING_YARNDETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_POST_COSTING_YARNDETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.COS_POSTCOSTING_YARNDETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_POST_COSTING_YARNDETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.YarnFor)
                    .WithMany(p => p.COS_POSTCOSTING_YARNDETAILS)
                    .HasForeignKey(d => d.YARNFOR)
                    .HasConstraintName("FK_COS_POSTCOSTING_YARNDETAILS_YARNFOR");

                entity.HasOne(d => d.PCOST)
                    .WithMany(p => p.COS_POSTCOSTING_YARNDETAILS)
                    .HasForeignKey(d => d.PCOSTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PST_COSTING_YARNDETAILS_COS_POSTCOSTING_MASTER");
            });



            //Proc WorkOrder Master & Details
            modelBuilder.Entity<PROC_WORKORDER_DETAILS>(entity =>
            {
                entity.HasKey(e => e.WODID);

                entity.Property(e => e.BSUNIT).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PRODNAME).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GsProductInfo)
                    .WithMany(p => p.PROC_WORKORDER_DETAILS)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_PROC_WORKORDER_DETAILS_F_GS_PRODUCT_INFORMATION");

                entity.HasOne(d => d.WorkOrderMaster)
                    .WithMany(p => p.PROC_WORKORDER_DETAILS)
                    .HasForeignKey(d => d.WOID)
                    .HasConstraintName("FK_PROC_WORKORDER_DETAILS_PROC_WORKORDER_MASTER");

                entity.HasOne(d => d.YsIndentMaster)
                    .WithMany(p => p.PROC_WORKORDER_DETAILS)
                    .HasForeignKey(d => d.INDENTNO)
                    .HasConstraintName("FK_PROC_WORKORDER_DETAILS_F_YS_INDENT_MASTER");
            });

            modelBuilder.Entity<PROC_WORKORDER_MASTER>(entity =>
            {
                entity.HasKey(e => e.WOID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CURRENCY).HasMaxLength(50);

                entity.Property(e => e.FREIGHT).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PAYMODE).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WODATE).HasColumnType("datetime");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.PROC_WORKORDER_MASTER)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_PROC_WORKORDER_MASTER_BAS_SUPPLIERINFO");
            });

            //Vehicle Type
            modelBuilder.Entity<VEHICLE_TYPE>(entity =>
            {
                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TYPE_NAME).HasMaxLength(50);
            });

            //F_PR_WEAVING_PRODUCTION
            modelBuilder.Entity<F_PR_WEAVING_PRODUCTION>(entity =>
            {
                entity.HasKey(e => e.WV_PRODID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50).HasColumnType("datetime");

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_PR_WEAVING_PRODUCTION)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_PR_WEAVING_PRODUCTION_RND_FABRICINFO");

                entity.HasOne(d => d.LOOM)
                    .WithMany(p => p.F_PR_WEAVING_PRODUCTION)
                    .HasForeignKey(d => d.LOOMID)
                    .HasConstraintName("FK_F_PR_WEAVING_PRODUCTION_LOOM_TYPE");

                entity.HasOne(d => d.PO)
                    .WithMany(p => p.F_PR_WEAVING_PRODUCTION)
                    .HasForeignKey(d => d.POID)
                    .HasConstraintName("FK_F_PR_WEAVING_PRODUCTION_RND_PRODUCTION_ORDER");
            });

            modelBuilder.Entity<F_YS_INDENT_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.TRNSID).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.ETR).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YARN_TYPE).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.LAST_INDENT_NO).HasMaxLength(50);

                entity.Property(e => e.LAST_INDENT_DATE).HasMaxLength(50);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_F_BAS_SECTION");

                entity.HasOne(d => d.BASCOUNTINFO)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.IND)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.INDID)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_F_YS_INDENT_MASTER");

                entity.HasOne(d => d.RND_PURCHASE_REQUISITION_MASTER)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_RND_PURCHASE_REQUISITION_MASTER");

                entity.HasOne(d => d.YARN_FORNavigation)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.YARN_FOR)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_YARNFOR");

                entity.HasOne(d => d.FBasUnits)
                    .WithMany(p => p.FYsIndentDetailses)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.YARN_FROMNavigation)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.YARN_FROM)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_YARNFROM");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.PREV_LOTID)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.RAWNavigation)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.RAW)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_F_YS_RAW_PER");

                entity.HasOne(d => d.SLUB_CODENavigation)
                    .WithMany(p => p.F_YS_INDENT_DETAILS)
                    .HasForeignKey(d => d.SLUB_CODE)
                    .HasConstraintName("FK_F_YS_INDENT_DETAILS_F_YS_SLUB_CODE");
            });

            //RND BOM
            modelBuilder.Entity<RND_BOM>(entity =>
            {
                entity.HasKey(e => e.BOMID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FINISH_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.INDIGO_BOX).HasMaxLength(50);

                entity.Property(e => e.INDIGO_GPL).HasMaxLength(50);

                entity.Property(e => e.LOT_RATIO);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.OPT7).HasMaxLength(50);

                entity.Property(e => e.OPT8).HasMaxLength(50);

                entity.Property(e => e.OTHERS_BOX).HasMaxLength(50);

                entity.Property(e => e.OTHERS_GPL).HasMaxLength(50);

                entity.Property(e => e.OTHERS_REMARKS).HasMaxLength(50);

                entity.Property(e => e.PROG_NO).HasMaxLength(50);

                entity.Property(e => e.SULPHURE_BOX).HasMaxLength(50);

                entity.Property(e => e.SULPHURE_GPL).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WIDTH).HasMaxLength(50);

                entity.HasOne(d => d.COLORNavigation)
                    .WithMany(p => p.RND_BOM)
                    .HasForeignKey(d => d.COLOR)
                    .HasConstraintName("FK_RND_BOM_BAS_COLOR");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.RND_BOM)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_RND_BOM_RND_FABRICINFO");

                entity.HasOne(d => d.FINISH_TYPENavigation)
                    .WithMany(p => p.RND_BOM)
                    .HasForeignKey(d => d.FINISH_TYPE)
                    .HasConstraintName("FK_RND_BOM_RND_FINISHTYPE");

                entity.HasOne(d => d.SETNONavigation)
                    .WithMany(p => p.RND_BOM)
                    .HasForeignKey(d => d.SETNO)
                    .HasConstraintName("FK_RND_BOM_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<RND_BOM_MATERIALS_DETAILS>(entity =>
            {
                entity.HasKey(e => e.BOM_D_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT10).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.OPT7).HasMaxLength(50);

                entity.Property(e => e.OPT8).HasMaxLength(50);

                entity.Property(e => e.OPT9).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BOM)
                    .WithMany(p => p.RND_BOM_MATERIALS_DETAILS)
                    .HasForeignKey(d => d.BOMID)
                    .HasConstraintName("FK_RND_BOM_MATERIALS_DETAILS_RND_BOM");

                entity.HasOne(d => d.CHEM_PROD_)
                    .WithMany(p => p.RND_BOM_MATERIALS_DETAILS)
                    .HasForeignKey(d => d.CHEM_PROD_ID)
                    .HasConstraintName("FK_RND_BOM_MATERIALS_DETAILS_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.SECTIONNavigation)
                    .WithMany(p => p.RND_BOM_MATERIALS_DETAILS)
                    .HasForeignKey(d => d.SECTION)
                    .HasConstraintName("FK_RND_BOM_MATERIALS_DETAILS_F_BAS_SECTION");
            });

            //Proc Bill Master & Details
            modelBuilder.Entity<PROC_BILL_DETAILS>(entity =>
            {
                entity.HasKey(e => e.BDID);

                entity.Property(e => e.BDDATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BILL)
                    .WithMany(p => p.PROC_BILL_DETAILS)
                    .HasForeignKey(d => d.BILLID)
                    .HasConstraintName("FK_PROC_BILL_DETAILS_PROC_BILL_MASTER");

                //entity.HasOne(d => d.IND)
                //    .WithMany(p => p.PROC_BILL_DETAILS)
                //    .HasForeignKey(d => d.INDID)
                //    .HasConstraintName("FK_PROC_BILL_DETAILS_F_GS_INDENT_MASTER");

                entity.HasOne(d => d.PROD)
                    .WithMany(p => p.PROC_BILL_DETAILS)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_PROC_BILL_DETAILS_F_GS_PRODUCT_INFORMATION");

                //entity.HasOne(d => d.TRANS)
                //    .WithMany(p => p.PROC_BILL_DETAILS)
                //    .HasForeignKey(d => d.TRANSID)
                //    .HasConstraintName("FK_PROC_BILL_DETAILS_F_GS_PURCHASE_REQUISITION_DETAILS");
            });

            modelBuilder.Entity<PROC_BILL_MASTER>(entity =>
            {
                entity.HasKey(e => e.BILLID);

                entity.Property(e => e.BILLDATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PAYMODE).HasMaxLength(50);

                entity.Property(e => e.SOURCE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CHALLAN)
                    .WithMany(p => p.PROC_BILL_MASTER)
                    .HasForeignKey(d => d.CHALLANID)
                    .HasConstraintName("FK_PROC_BILL_MASTER_F_GEN_S_RECEIVE_MASTER");
            });

            modelBuilder.Entity<YARNFROM>(entity =>
            {
                entity.HasKey(e => e.YFID);
            });

            modelBuilder.Entity<F_YS_RAW_PER>(entity =>
            {
                entity.HasKey(e => e.RAWID);

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CREATED_DATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_DATE).HasColumnType("datetime");
            });

            modelBuilder.Entity<F_YS_SLUB_CODE>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });
            modelBuilder.Entity<F_YS_INDENT_MASTER>(entity =>
            {
                entity.HasKey(e => e.INDID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INDDATE).HasColumnType("datetime");

                entity.Property(e => e.INDNO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.INDSL)
                    .WithMany(p => p.F_YS_INDENT_MASTER)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_YS_INDENT_MASTER_RND_PURCHASE_REQUISITION_MASTER");
            });

            modelBuilder.Entity<ACC_EXPORT_DODETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.QTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.REMARKS).HasMaxLength(150);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.PI)
                    .WithMany(p => p.ACC_EXPORT_DODETAILS)
                    .HasForeignKey(d => d.PIID)
                    .HasConstraintName("FK_ACC_EXPORT_DODETAILS_COM_EX_PIMASTER");

                entity.HasOne(d => d.DO)
                    .WithMany(p => p.ACC_EXPORT_DODETAILS)
                    .HasForeignKey(d => d.DONO)
                    .HasConstraintName("FK_ACC_EXPORT_DODETAILS_ACC_EXPORT_DOMASTER");

                entity.HasOne(d => d.STYLE)
                    .WithMany(p => p.ACC_EXPORT_DODETAILS)
                    .HasForeignKey(d => d.STYLEID)
                    .HasConstraintName("FK_ACC_EXPORT_DODETAILS_COM_EX_FABSTYLE");

                entity.HasOne(d => d.SO)
                    .WithMany(p => p.ACC_EXPORT_DODETAILS)
                    .HasForeignKey(d => d.SOID)
                    .HasConstraintName("FK_ACC_EXPORT_DODETAILS_COM_EX_PI_DETAILS");
            });

            modelBuilder.Entity<ACC_EXPORT_DOMASTER>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.AUDITBY).HasMaxLength(50);

                entity.Property(e => e.AUDITON).HasColumnType("datetime");

                entity.Property(e => e.COMMENTS).HasMaxLength(50);

                entity.Property(e => e.DODATE).HasColumnType("datetime");

                entity.Property(e => e.DOEX).HasColumnType("datetime");

                entity.Property(e => e.DONO).IsRequired().HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(100);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.ComExLcInfo)
                    .WithMany(p => p.AccExportDoMaster)
                    .HasForeignKey(d => d.LCID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_EXPORT_DOMASTER_COM_EX_LCINFO");
            });

            modelBuilder.Entity<ACC_EXPORT_REALIZATION>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.DISVOU).HasMaxLength(50);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NOTES).HasMaxLength(50);

                entity.Property(e => e.REALVOU).HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.REZDATE).HasColumnType("datetime");

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.AUDITDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.INVOICE)
                    .WithMany(p => p.ACC_EXPORT_REALIZATION)
                    .HasForeignKey(d => d.INVID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_EXPORT_REALIZATION_COM_EX_INVOICEMASTER");
            });

            modelBuilder.Entity<ACC_LOCAL_DODETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.QTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.DO)
                    .WithMany(p => p.ACC_LOCAL_DODETAILS)
                    .HasForeignKey(d => d.DONO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_LOCAL_DODETAILS_ACC_LOCAL_DOMASTER");

                entity.HasOne(d => d.STYLE)
                    .WithMany(p => p.ACC_LOCAL_DODETAILS)
                    .HasForeignKey(d => d.STYLEID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_LOCAL_DODETAILS_COM_EX_SCDETAILS");

                entity.HasOne(d => d.ComExPiDetails)
                    .WithMany(p => p.ACC_LOCAL_DODETAILS)
                    .HasForeignKey(d => d.PI_TRNSID)
                    .HasConstraintName("FK_ACC_LOCAL_DODETAILS_COM_EX_PI_DETAILS");
            });

            modelBuilder.Entity<ACC_LOCAL_DOMASTER>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.AUDITBY).HasMaxLength(50);

                entity.Property(e => e.AUDITON).HasColumnType("datetime");

                entity.Property(e => e.COMMENTS).HasMaxLength(50);

                entity.Property(e => e.DODATE).HasColumnType("datetime");

                entity.Property(e => e.DOEX).HasColumnType("datetime");

                entity.Property(e => e.DONO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.LC)
                    .WithMany(p => p.ACC_LOCAL_DOMASTER)
                    .HasForeignKey(d => d.LCID)
                    .HasConstraintName("FK_ACC_LOCAL_DOMASTER_COM_EX_LCINFO");

                entity.HasOne(d => d.ComExScinfo)
                    .WithMany(p => p.ACC_LOCAL_DOMASTER)
                    .HasForeignKey(d => d.SCID)
                    .HasConstraintName("FK_ACC_LOCAL_DOMASTER_COM_EX_SCINFO");
            });

            modelBuilder.Entity<ADM_DEPARTMENT>(entity =>
            {
                entity.HasKey(e => e.DEPTID);

                entity.Property(e => e.DEPTNAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<ADM_DESIGNATION>(entity =>
            {
                entity.HasKey(e => e.DESID);

                entity.Property(e => e.DESNAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.DEP)
                    .WithMany(p => p.ADM_DESIGNATION)
                    .HasForeignKey(d => d.DEPID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ADM_DESIGNATION_ADM_DEPARTMENT");
            });

            #region IndentityUser

            //modelBuilder.Entity<AspNetRoleClaims>(entity =>
            //{
            //    entity.HasIndex(e => e.RoleId);

            //    entity.Property(e => e.RoleId).IsRequired();

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetRoleClaims)
            //        .HasForeignKey(d => d.RoleId);
            //});

            //modelBuilder.Entity<AspNetRoles>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedName)
            //        .HasName("RoleNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedName] IS NOT NULL)");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.Name).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetUserClaims>(entity =>
            //{
            //    entity.HasIndex(e => e.UserId);

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserClaims)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserLogins>(entity =>
            //{
            //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            //    entity.HasIndex(e => e.UserId);

            //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

            //    entity.Property(e => e.ProviderKey).HasMaxLength(128);

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserLogins)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserRoles>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.RoleId });

            //    entity.HasIndex(e => e.RoleId);

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.RoleId);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUsers>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedEmail)
            //        .HasName("EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName)
            //        .HasName("UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            //    entity.Property(e => e.UserName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetUserTokens>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

            //    entity.Property(e => e.Name).HasMaxLength(128);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserTokens)
            //        .HasForeignKey(d => d.UserId);
            //});

            #endregion

            modelBuilder.Entity<BAS_BEN_BANK_MASTER>(entity =>
            {
                entity.HasKey(e => e.BANKID);

                entity.Property(e => e.ADDRESS).HasMaxLength(100);

                entity.Property(e => e.BEN_BANK)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BRANCH).HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_BRANDINFO>(entity =>
            {
                entity.HasKey(e => e.BRANDID);

                entity.Property(e => e.BRANDCODE).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.BRANDNAME)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_BUYER_BANK_MASTER>(entity =>
            {
                entity.HasKey(e => e.BANK_ID);

                entity.Property(e => e.ADDRESS).HasMaxLength(250);

                entity.Property(e => e.BRANCH).HasMaxLength(100);

                entity.Property(e => e.PARTY_BANK)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_BUYERINFO>(entity =>
            {
                entity.HasKey(e => e.BUYERID);

                entity.Property(e => e.ADDRESS).HasMaxLength(250);

                entity.Property(e => e.BIN_NO).HasMaxLength(50);

                entity.Property(e => e.BUYER_NAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DEL_ADDRESS).HasMaxLength(250);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_COLOR>(entity =>
            {
                entity.HasKey(e => e.COLORCODE);

                entity.Property(e => e.COLOR)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_INSURANCEINFO>(entity =>
            {
                entity.HasKey(e => e.INSID);

                entity.Property(e => e.ADDRESS).HasMaxLength(50);

                entity.Property(e => e.CPERSON).HasMaxLength(50);

                entity.Property(e => e.EMAIL).HasMaxLength(50);

                entity.Property(e => e.INSNAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PHONE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_PRODCATEGORY>(entity =>
            {
                entity.HasKey(e => e.CATID);

                entity.Property(e => e.CATEGORY)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_PRODUCTINFO>(entity =>
            {
                entity.HasKey(e => e.PRODID);

                entity.Property(e => e.DESCRIPTION).IsUnicode(false);

                entity.Property(e => e.PRODNAME).IsUnicode(false);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.CAT)
                    .WithMany(p => p.BAS_PRODUCTINFO)
                    .HasForeignKey(d => d.CATID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAS_PRODUCTINFO_BAS_PRODCATEGORY");

                entity.HasOne(d => d.CS)
                    .WithMany(p => p.BAS_PRODUCTINFO)
                    .HasForeignKey(d => d.CSID)
                    .HasConstraintName("FK_BAS_PRODUCTINFO_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.GS)
                    .WithMany(p => p.BAS_PRODUCTINFO)
                    .HasForeignKey(d => d.GSID)
                    .HasConstraintName("FK_BAS_PRODUCTINFO_F_GS_PRODUCT_INFORMATION");

                entity.HasOne(d => d.YS)
                    .WithMany(p => p.BAS_PRODUCTINFO)
                    .HasForeignKey(d => d.YSID)
                    .HasConstraintName("FK_BAS_PRODUCTINFO_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.BAS_PRODUCTINFO)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_BAS_PRODUCTINFO_F_BAS_UNITS");
            });

            modelBuilder.Entity<BAS_PRODUCTINFO1>(entity =>
            {
                entity.HasKey(e => e.PRODID);

                entity.Property(e => e.CATID).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.PRODNAME).IsRequired();

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_SUPP_CATEGORY>(entity =>
            {
                entity.HasKey(e => e.SCATID);

                entity.Property(e => e.CATNAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_SUPPLIERINFO>(entity =>
            {
                entity.HasKey(e => e.SUPPID);

                entity.Property(e => e.ADDRESS).HasMaxLength(150);

                entity.Property(e => e.CPERSON).HasMaxLength(50);

                entity.Property(e => e.EMAIL).HasMaxLength(50);

                entity.Property(e => e.PHONE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SUPPNAME)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.SCAT)
                    .WithMany(p => p.BAS_SUPPLIERINFO)
                    .HasForeignKey(d => d.SCATID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAS_SUPPLIERINFO_BAS_SUPP_CATEGORY");
            });

            modelBuilder.Entity<BAS_TEAMINFO>(entity =>
            {
                entity.HasKey(e => e.TEAMID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TEAM_NAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DEPT)
                    .WithMany(p => p.BAS_TEAMINFO)
                    .HasForeignKey(d => d.DEPTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAS_TEAMINFO_ADM_DEPARTMENT");
            });

            modelBuilder.Entity<BAS_TRANSPORTINFO>(entity =>
            {
                entity.HasKey(e => e.TRNSPID);

                entity.Property(e => e.ADDRESS).HasMaxLength(50);

                entity.Property(e => e.CPERSON).HasMaxLength(50);

                entity.Property(e => e.EMAIL).HasMaxLength(50);

                entity.Property(e => e.PHONE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSPNAME)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_YARN_COUNTINFO>(entity =>
            {
                entity.HasKey(e => e.COUNTID);

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.YARN_CAT_)
                    .WithMany(p => p.BAS_YARN_COUNTINFO)
                    .HasForeignKey(d => d.YARN_CAT_ID)
                    .HasConstraintName("FK_BAS_YARN_COUNTINFO_BAS_YARN_CATEGORY");

                entity.HasOne(d => d.BAS_COLOR)
                    .WithMany(p => p.BAS_YARN_COUNTINFO)
                    .HasForeignKey(d => d.COLOR)
                    .HasConstraintName("FK_BAS_YARN_COUNTINFO_BAS_COLOR");

                entity.HasOne(d => d.PART_)
                    .WithMany(p => p.BAS_YARN_COUNTINFO)
                    .HasForeignKey(d => d.PART_ID)
                    .HasConstraintName("FK_BAS_YARN_COUNTINFO_BAS_YARN_PARTNO");
            });

            modelBuilder.Entity<BAS_YARN_COUNT_LOT_INFO>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.BAS_YARN_COUNT_LOT_INFO)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_BAS_YARN_COUNT_LOT_INFO_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.BAS_YARN_COUNT_LOT_INFO)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_BAS_YARN_COUNT_LOT_INFO_BAS_YARN_LOTINFO");
            });

            modelBuilder.Entity<BAS_YARN_CATEGORY>(entity =>
            {
                entity.HasKey(e => e.YARN_CAT_ID);

                entity.Property(e => e.CATEGORY_NAME).HasMaxLength(150);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_YARN_LOTINFO>(entity =>
            {
                entity.HasKey(e => e.LOTID);

                entity.Property(e => e.BRAND).HasMaxLength(50);

                entity.Property(e => e.LOTNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SLABCODE).HasMaxLength(50);
            });

            modelBuilder.Entity<COM_EX_BOEXOPTION>(entity =>
            {
                entity.HasKey(e => e.OPTIONID);

                entity.Property(e => e.OPTIONID).ValueGeneratedNever();

                entity.Property(e => e.OPTIONNAME)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<COM_EX_FABSTYLE>(entity =>
            {
                entity.HasKey(e => e.STYLEID);

                entity.Property(e => e.BCI).HasMaxLength(50);

                entity.Property(e => e.CMIA).HasMaxLength(50);

                entity.Property(e => e.COMM_CONST).HasMaxLength(250);

                entity.Property(e => e.FABCODE)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GRS).HasMaxLength(50);

                entity.Property(e => e.HSCODE).HasMaxLength(50);

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.ORG_GOTS).HasMaxLength(50);

                entity.Property(e => e.ORG_OCS).HasMaxLength(50);

                entity.Property(e => e.RCS).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.RND_CONST).HasMaxLength(250);

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.STYLENAME)
                    .IsRequired();


                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.BRAND)
                    .WithMany(p => p.COM_EX_FABSTYLE)
                    .HasForeignKey(d => d.BRANDID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAS_FABSTYLE_BAS_brandInfo");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.COM_EX_FABSTYLE)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_COM_EX_FABSTYLE_RND_FABRICINFO");
            });

            modelBuilder.Entity<COM_EX_INVDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.Property(e => e.QTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.ROLL).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.ComExFabstyle)
                    .WithMany(p => p.ComExInvdetailses)
                    .HasForeignKey(d => d.STYLEID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_INVDETAILS_COM_EX_FABSTYLE");

                entity.HasOne(d => d.InvoiceMaster)
                    .WithMany(p => p.ComExInvdetailses)
                    .HasForeignKey(d => d.INVID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_INVDETAILS_COM_EX_INVOICEMASTER");

                entity.HasOne(d => d.PiDetails)
                    .WithMany(p => p.COM_EX_INVDETAILS)
                    .HasForeignKey(d => d.PIIDD_TRNSID)
                    .HasConstraintName("FK_COM_EX_INVDETAILS_COM_EX_PI_DETAILS");
            });

            modelBuilder.Entity<COM_EX_INVOICEMASTER>(entity =>
            {
                entity.HasKey(e => e.INVID);

                entity.Property(e => e.BANK_REF).HasMaxLength(50);

                entity.Property(e => e.BILL_DATE).HasColumnType("datetime");

                entity.Property(e => e.BNK_ACC_DATE).HasColumnType("datetime");

                entity.Property(e => e.BNK_ACC_POSTING).HasColumnType("datetime");

                entity.Property(e => e.BNK_SUB_DATE).HasColumnType("datetime");

                entity.Property(e => e.ODRCVDATE).HasColumnType("datetime");

                entity.Property(e => e.DELDATE).HasColumnType("datetime");

                entity.Property(e => e.DISCREPANCY).HasMaxLength(50);

                entity.Property(e => e.DOC_NOTES).HasMaxLength(50);

                entity.Property(e => e.DOC_RCV_DATE).HasColumnType("datetime");

                entity.Property(e => e.DOC_SUB_DATE).HasColumnType("datetime");

                entity.Property(e => e.EXDATE).HasColumnType("datetime");

                entity.Property(e => e.INVDATE).HasColumnType("datetime");

                entity.Property(e => e.INVDURATION).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.INVNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.INVREF).HasMaxLength(50);

                entity.Property(e => e.ISACTIVE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.Property(e => e.MATUDATE).HasColumnType("datetime");

                entity.Property(e => e.NEGODATE).HasColumnType("datetime");

                entity.Property(e => e.PDOCNO).HasMaxLength(50);

                entity.Property(e => e.PRCDATE).HasColumnType("datetime");

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.COM_EX_INVOICEMASTER)
                    .HasForeignKey(d => d.BUYERID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_INVOICEMASTER_BAS_BUYERINFO");

                entity.HasOne(d => d.LC)
                    .WithMany(p => p.COM_EX_INVOICEMASTER)
                    .HasForeignKey(d => d.LCID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_INVOICEMASTER_COM_EX_LCINFO");
            });

            modelBuilder.Entity<COM_EX_LCDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LCNO)
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.LC)
                    .WithMany(p => p.COM_EX_LCDETAILS)
                    .HasForeignKey(d => d.LCID)
                    .HasConstraintName("FK_COM_EX_LCDETAILS_COM_EX_LCINFO");

                entity.HasOne(d => d.BANK)
                    .WithMany(p => p.COM_EX_LCDETAILS)
                    .HasForeignKey(d => d.BANKID)
                    .HasConstraintName("FK_COM_EX_LCDETAILS_BAS_BEN_BANK_MASTER");

                entity.HasOne(d => d.BANK_)
                    .WithMany(p => p.COM_EX_LCDETAILS_NEGO)
                    .HasForeignKey(d => d.BANK_ID)
                    .HasConstraintName("FK_COM_EX_LCDETAILS_BAS_BEN_BANK_MASTER_NEGO");

                entity.HasOne(d => d.PI)
                    .WithMany(p => p.COM_EX_LCDETAILS)
                    .HasForeignKey(d => d.PIID)
                    .HasConstraintName("FK_COM_EX_LCDETAILS_COM_EX_PIMASTER");
            });

            modelBuilder.Entity<COM_EX_LCINFO>(entity =>
            {
                entity.HasKey(e => e.LCID);

                entity.Property(e => e.ADREF).HasMaxLength(100);

                entity.Property(e => e.AMENTDATE).HasColumnType("datetime");

                entity.Property(e => e.AREA).HasMaxLength(100);

                entity.Property(e => e.BAREA).HasMaxLength(100);

                entity.Property(e => e.BTIN).HasMaxLength(100);

                entity.Property(e => e.BVAT_REG).HasMaxLength(100);

                entity.Property(e => e.CONTRACTSTATUS).HasMaxLength(100);

                entity.Property(e => e.CURRENCY).HasMaxLength(100);

                entity.Property(e => e.DISCOUNT).HasMaxLength(100);

                entity.Property(e => e.DOSTATUS).HasMaxLength(100);

                entity.Property(e => e.ERC).HasMaxLength(100);

                entity.Property(e => e.EXPORTSTATUS).HasMaxLength(100);

                entity.Property(e => e.EX_DATE).HasColumnType("datetime");

                entity.Property(e => e.FILENO)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GARMENT_QTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.HSCODE).HasMaxLength(100);

                entity.Property(e => e.IRC).HasMaxLength(100);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LCDATE).HasColumnType("datetime");

                entity.Property(e => e.LCNO)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LCRCVDATE).HasColumnType("datetime");

                entity.Property(e => e.MARKS).HasMaxLength(150);

                entity.Property(e => e.MLCSUBDATE).HasColumnType("datetime");

                entity.Property(e => e.ODUEINTEREST).HasMaxLength(100);

                entity.Property(e => e.OTHERS).HasMaxLength(250);

                entity.Property(e => e.PORTDISCHARGE).HasMaxLength(100);

                entity.Property(e => e.PORTLOADING).HasMaxLength(100);

                entity.Property(e => e.SAILING).HasMaxLength(100);

                entity.Property(e => e.SHIP_DATE).HasColumnType("datetime");

                entity.Property(e => e.TIN).HasMaxLength(100);

                entity.Property(e => e.UDDATE).HasColumnType("datetime");

                entity.Property(e => e.UDNO).HasMaxLength(100);

                entity.Property(e => e.UDSUBDATE).HasColumnType("datetime");

                entity.Property(e => e.UNIT).HasMaxLength(100);

                entity.Property(e => e.UPNO).HasMaxLength(100);

                entity.Property(e => e.UP_DATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(100);

                entity.Property(e => e.VAT_REG).HasMaxLength(100);

                entity.Property(e => e.VESSEL).HasMaxLength(100);

                entity.HasOne(d => d.BANK_)
                    .WithMany(p => p.COM_EX_LCINFO)
                    .HasForeignKey(d => d.BANK_ID)
                    .HasConstraintName("FK_COM_EX_LCINFO_BAS_BUYER_BANK_MASTER");

                entity.HasOne(d => d.NTFYBANK)
                    .WithMany(p => p.COM_EX_LCINFONTFYBANK)
                    .HasForeignKey(d => d.NTFYBANKID)
                    .HasConstraintName("FK_COM_EX_LCINFO_BAS_BUYER_BANK_MASTER_NTFY");

                entity.HasOne(d => d.BANK)
                    .WithMany(p => p.COM_EX_LCINFO)
                    .HasForeignKey(d => d.BANKID)
                    .HasConstraintName("FK_COM_EX_LCINFO_BAS_BEN_BANK_MASTER");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.COM_EX_LCINFO)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_COM_EX_LCINFO_BAS_BUYERINFO");

                entity.HasOne(d => d.TEAM)
                    .WithMany(p => p.COM_EX_LCINFO)
                    .HasForeignKey(d => d.TEAMID)
                    .HasConstraintName("FK_COM_EX_LCINFO_MKT_TEAM");

                entity.HasOne(d => d.COM_TENOR)
                    .WithMany(p => p.COM_EX_LCINFO)
                    .HasForeignKey(d => d.TID)
                    .HasConstraintName("FK_COM_EX_LCINFO_COM_TENOR");

                entity.HasOne(d => d.COM_TRADE_TERMS)
                    .WithMany(p => p.COM_EX_LCINFO)
                    .HasForeignKey(d => d.TTERMS)
                    .HasConstraintName("FK_COM_EX_LCINFO_COM_TRADE_TERMS");
            });

            modelBuilder.Entity<COM_EX_PI_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PINO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SO_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(100);

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.HasOne(d => d.STYLE)
                    .WithMany(p => p.COM_EX_PI_DETAILS)
                    .HasForeignKey(d => d.STYLEID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_PI_DETAILS_COM_EX_FABSTYLE");

                entity.HasOne(d => d.PIMASTER)
                    .WithMany(p => p.COM_EX_PI_DETAILS)
                    .HasForeignKey(d => d.PIID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_PI_DETAILS_COM_EX_PIMASTER");

                entity.HasOne(d => d.PRECOSTING)
                    .WithMany(p => p.COM_EX_PI_DETAILS)
                    .HasForeignKey(d => d.COSTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_PI_DETAILS_COS_PRECOSTING_MASTER");

                entity.HasOne(d => d.F_BAS_UNITS)
                    .WithMany(p => p.COM_EX_PI_DETAILS)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_COM_EX_PI_DETAILS_F_BAS_UNITS");

            });


            modelBuilder.Entity<BAS_SEASON>(entity =>
            {
                entity.HasKey(e => e.SID);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<COM_EX_PIMASTER>(entity =>
            {
                entity.HasKey(e => e.PIID);

                entity.Property(e => e.COO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(100);

                entity.Property(e => e.DEL_CLOSE).HasColumnType("datetime");

                entity.Property(e => e.DEL_PERIOD).HasMaxLength(150);

                entity.Property(e => e.DEL_START).HasColumnType("datetime");

                entity.Property(e => e.FLWBY).HasMaxLength(50);

                entity.Property(e => e.FREIGHT).HasMaxLength(50);

                entity.Property(e => e.GRS_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.INCOTERMS).HasMaxLength(150);

                entity.Property(e => e.INSPECTION).HasMaxLength(250);

                entity.Property(e => e.INSURANCE_COVERAGE).HasMaxLength(100);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LCNO).HasMaxLength(50);

                entity.Property(e => e.NEGOTIATION).HasMaxLength(50);

                entity.Property(e => e.NET_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.ORDER_REF).HasMaxLength(150);

                entity.Property(e => e.PAYMENT).HasMaxLength(150);

                entity.Property(e => e.PIDATE).HasColumnType("datetime");

                entity.Property(e => e.PINO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.POD).HasMaxLength(50);

                entity.Property(e => e.POL).HasMaxLength(50);

                entity.Property(e => e.PREVIOUS_DELIVERY_NOTE).HasMaxLength(100);

                entity.Property(e => e.SHIPDATE).HasMaxLength(50);

                entity.Property(e => e.TOLERANCE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(100);

                entity.Property(e => e.VALIDITY).HasColumnType("datetime");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.SID)
                    .HasConstraintName("FK_COM_EX_PIMASTER_BAS_SEASON");
            });

            modelBuilder.Entity<COM_EX_PIMASTER>(entity =>
            {
                entity.HasKey(e => e.PIID);

                entity.Property(e => e.COO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(100);

                entity.Property(e => e.DEL_CLOSE).HasColumnType("datetime");

                entity.Property(e => e.DEL_PERIOD).HasMaxLength(150);

                entity.Property(e => e.DEL_START).HasColumnType("datetime");

                entity.Property(e => e.FLWBY).HasMaxLength(50);

                entity.Property(e => e.FREIGHT).HasMaxLength(50);

                entity.Property(e => e.GRS_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.INCOTERMS).HasMaxLength(150);

                entity.Property(e => e.INSPECTION).HasMaxLength(250);

                entity.Property(e => e.INSURANCE_COVERAGE).HasMaxLength(100);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LCNO).HasMaxLength(50);

                entity.Property(e => e.NEGOTIATION).HasMaxLength(50);

                entity.Property(e => e.NET_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.ORDER_REF).HasMaxLength(150);

                entity.Property(e => e.PAYMENT).HasMaxLength(150);

                entity.Property(e => e.PIDATE).HasColumnType("datetime");

                entity.Property(e => e.PINO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.POD).HasMaxLength(50);

                entity.Property(e => e.POL).HasMaxLength(50);

                entity.Property(e => e.PREVIOUS_DELIVERY_NOTE).HasMaxLength(100);

                entity.Property(e => e.SHIPDATE).HasMaxLength(50);

                entity.Property(e => e.TOLERANCE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(100);

                entity.Property(e => e.VALIDITY).HasColumnType("datetime");

                entity.HasOne(d => d.BANK)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.BANKID)
                    .HasConstraintName("FK_COM_PIMASTER_BAS_BEN_BANK_MASTER");

                entity.HasOne(d => d.BANK_)
                    .WithMany(p => p.COM_EX_PIMASTER_)
                    .HasForeignKey(d => d.BANK_ID)
                    .HasConstraintName("FK_COM_PIMASTER_BAS_BEN_BANK_MASTER_");

                entity.HasOne(d => d.BRAND)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.BRANDID)
                    .HasConstraintName("FK_COM_PIMASTER_BAS_BRANDINFO");

                entity.HasOne(d => d.TEAM)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.TEAMID)
                    .HasConstraintName("FK_COM_PIMASTER_BAS_TEAMINFO");

                entity.HasOne(d => d.PersonMktTeam)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.TEAM_PERSONID)
                    .HasConstraintName("FK_COM_EX_PIMASTER_MKT_TEAM");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_COM_EX_PIMASTER_BAS_BUYERINFO");

                entity.HasOne(d => d.TENORNavigation)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.TENOR)
                    .HasConstraintName("FK_COM_EX_PIMASTER_COM_TENOR");

                entity.HasOne(d => d.EXPORTSTATUS)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.EXP_STATUS)
                    .HasConstraintName("FK_COM_EX_PIMASTER_EXPORTSTATUS");

                entity.HasOne(d => d.CURRENCYS)
                    .WithMany(p => p.COM_EX_PIMASTER)
                    .HasForeignKey(d => d.CURRENCY)
                    .HasConstraintName("FK_COM_EX_PIMASTER_CURRENCY");
            });

            modelBuilder.Entity<COM_EX_SCDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.QTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.HasOne(d => d.STYLE)
                    .WithMany(p => p.COM_EX_SCDETAILS)
                    .HasForeignKey(d => d.STYLEID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_SCDETAILS_COM_EX_FABSTYLE");

                entity.HasOne(d => d.SC)
                    .WithMany(p => p.COM_EX_SCDETAILS)
                    .HasForeignKey(d => d.SCNO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_SCDETAILS_COM_EX_SCINFO");
            });

            modelBuilder.Entity<COM_EX_SCINFO>(entity =>
            {
                entity.HasKey(e => e.SCID);

                entity.Property(e => e.BCEMAIL).HasMaxLength(50);

                entity.Property(e => e.BCPERSON).HasMaxLength(50);

                entity.Property(e => e.BCPHONE).HasMaxLength(50);

                entity.Property(e => e.INIDEPOSIT).HasMaxLength(50);

                entity.Property(e => e.CHKNO).HasMaxLength(50);

                entity.Property(e => e.CONTRACTNOTE).HasMaxLength(50);

                entity.Property(e => e.CON_VAL).HasMaxLength(50);

                entity.Property(e => e.CURRENCY).HasMaxLength(50);

                entity.Property(e => e.DELDATE).HasColumnType("datetime");

                entity.Property(e => e.DEL_CLOSEDATE).HasColumnType("datetime");

                entity.Property(e => e.DEL_STARTDATE).HasColumnType("datetime");

                entity.Property(e => e.DEV_CODE).HasMaxLength(50);

                entity.Property(e => e.EXTRA_PROCESS).HasMaxLength(50);

                entity.Property(e => e.INSPECTION).HasMaxLength(50);

                entity.Property(e => e.ISDELETE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISDELMODE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISDYEING)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISFINISHING)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISLCB)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISOVERDYEING)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISPROVE_FARE)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISPROV_CHEM)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISPROV_TRNS)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISSIZING)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ISWARPING)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PAYDATE).HasColumnType("datetime");

                entity.Property(e => e.PAYMODE).HasMaxLength(50);

                entity.Property(e => e.SCDATE).HasColumnType("datetime");

                entity.Property(e => e.SCEMAIL).HasMaxLength(50);

                entity.Property(e => e.SCNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SCPERSON).HasMaxLength(50);

                entity.Property(e => e.SCPHONE).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.Property(e => e.WARP_PROVBY).HasMaxLength(50);

                entity.Property(e => e.WEFT_PROVBY).HasMaxLength(50);

                entity.HasOne(d => d.BANK_)
                    .WithMany(p => p.COM_EX_SCINFO)
                    .HasForeignKey(d => d.BANK_ID)
                    .HasConstraintName("FK_COM_EX_SCINFO_BAS_BUYER_BANK_MASTER");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.COM_EX_SCINFO)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_COM_EX_SCINFO_BAS_BUYERINFO");
            });

            modelBuilder.Entity<COM_IMP_INVDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.INVNO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.ComImpInvoiceinfo)
                    .WithMany(p => p.ComImpInvdetailses)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_COM_IMP_INVDETAILS_COM_IMP_INVOICEINFO");

                entity.HasOne(d => d.BasProductinfo)
                    .WithMany(p => p.ComImpInvdetailses)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_COM_IMP_INVDETAILS_BAS_PRODUCTINFO");

                entity.HasOne(d => d.F_BAS_UNITS)
                    .WithMany(p => p.COM_IMP_INVDETAILS)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_COM_IMP_INVDETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.F_CHEM_STORE_PRODUCTINFOS)
                    .WithMany(p => p.CHEM_COM_IMP_INVDETAILS)
                    .HasForeignKey(d => d.CHEMPRODID)
                    .HasConstraintName("FK_COM_IMP_INVDETAILS_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.BAS_YARN_LOTINFOS)
                    .WithMany(p => p.YARNLOTCOM_IMP_INVDETAILS)
                    .HasForeignKey(d => d.YARNLOTID)
                    .HasConstraintName("FK_COM_IMP_INVDETAILS_BAS_YARN_LOTINFO");
            });

            modelBuilder.Entity<COM_IMP_INVOICEINFO>(entity =>
            {
                entity.HasKey(e => e.INVID);

                entity.Property(e => e.BENTRYDATE).HasColumnType("datetime");

                entity.Property(e => e.BLDATE).HasMaxLength(50);

                entity.Property(e => e.MPNO).HasMaxLength(50);

                entity.Property(e => e.MPBILL).HasMaxLength(50);

                entity.Property(e => e.MPDATE).HasColumnType("datetime");

                entity.Property(e => e.MPSUB_DATE).HasColumnType("datetime");

                entity.Property(e => e.BLPATH).HasMaxLength(50);

                entity.Property(e => e.CONTQTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.CONTSIZE).HasMaxLength(50);

                entity.Property(e => e.CnFBILL).HasMaxLength(50);

                entity.Property(e => e.CnFBILLDATE).HasColumnType("datetime");

                entity.Property(e => e.DOCHANDSON).HasColumnType("datetime");

                entity.Property(e => e.ETADATE).HasColumnType("datetime");

                entity.Property(e => e.DEL_DATE).HasColumnType("datetime");

                entity.Property(e => e.INVDATE).HasColumnType("datetime");

                entity.Property(e => e.INVNO)
                    .IsRequired();

                entity.Property(e => e.INVPATH).HasMaxLength(50);

                entity.Property(e => e.LPORT).HasMaxLength(50);

                entity.Property(e => e.MRRDATE).HasMaxLength(100);

                entity.Property(e => e.TRNS_BILL_SUB_DATE).HasColumnType("datetime");

                entity.Property(e => e.PAYMENTDATE).HasColumnType("datetime");

                entity.Property(e => e.RCVSTATUS).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(100);

                entity.Property(e => e.SBDATE).HasColumnType("datetime");

                entity.Property(e => e.SHIPBY).HasMaxLength(50);

                entity.Property(e => e.TRUCKQTY).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.BENTRYVALUE).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.TRNSPBILL).HasMaxLength(50);

                entity.Property(e => e.TRNSPBILLDATE).HasColumnType("datetime");

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.TRNSP)
                    .WithMany(p => p.COM_IMP_INVOICEINFO)
                    .HasForeignKey(d => d.TRNSPID)
                    .HasConstraintName("FK_COM_IMP_INVOICEINFO_BAS_TRANSPORTINFO");

                entity.HasOne(d => d.DelStatus)
                    .WithMany(p => p.COM_IMP_INVOICEINFO)
                    .HasForeignKey(d => d.DEL_STATUS)
                    .HasConstraintName("FK_COM_IMP_INVOICEINFO_COM_IMP_DEL_STATUS");

                entity.HasOne(d => d.LC)
                    .WithMany(p => p.COM_IMP_INVOICEINFO)
                    .HasForeignKey(d => d.LC_ID)
                    .HasConstraintName("FK_COM_IMP_INVOICEINFO_COM_IMP_LCINFORMATION");

                entity.HasOne(d => d.COM_CONTAINER)
                    .WithMany(p => p.COM_IMP_INVOICEINFOS)
                    .HasForeignKey(d => d.CONTSIZE)
                    .HasConstraintName("FK_COM_IMP_INVOICEINFO_COM_CONTAINER");


                entity.HasOne(d => d.CNF)
                    .WithMany(p => p.COM_IMP_INVOICEINFO)
                    .HasForeignKey(d => d.CnF)
                    .HasConstraintName("FK_COM_IMP_INVOICEINFO_COM_IMP_CNFINFO");

            });

            modelBuilder.Entity<COM_IMP_LCDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.LCNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PIDATE).HasColumnType("datetime");

                entity.Property(e => e.PINO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PIPATH).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.LC)
                    .WithMany(p => p.COM_IMP_LCDETAILS)
                    .HasForeignKey(d => d.LC_ID)
                    .HasConstraintName("FK_COM_IMP_LCDETAILS_COM_IMP_LCINFORMATION");

                entity.HasOne(d => d.F_BAS_UNITS)
                    .WithMany(p => p.COM_IMP_LCDETAILS)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_COM_IMP_LCDETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.BAS_PRODUCTINFO)
                    .WithMany(p => p.COM_IMP_LCDETAILS)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_COM_IMP_LCDETAILS_BAS_PRODUCTINFO");
            });

            modelBuilder.Entity<COM_IMP_LCINFORMATION>(entity =>
            {
                entity.HasKey(e => e.LC_ID);

                entity.Property(e => e.CNOTEDATE).HasMaxLength(50);

                entity.Property(e => e.CURRENCY).HasMaxLength(50);

                entity.Property(e => e.DESPORT).HasMaxLength(50);

                entity.Property(e => e.EXPDATE).HasColumnType("datetime");

                entity.Property(e => e.LCDATE).HasColumnType("datetime");

                entity.Property(e => e.LCNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LCPATH).HasMaxLength(50);

                entity.Property(e => e.ORIGIN).HasMaxLength(50);

                entity.Property(e => e.SHIPDATE).HasColumnType("datetime");

                entity.Property(e => e.TOLLERANCE).HasMaxLength(50);

                entity.Property(e => e.FILENO).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.HasOne(d => d.BANK)
                    .WithMany(p => p.COM_IMP_LCINFORMATION)
                    .HasForeignKey(d => d.BANKID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_BAS_BEN_BANK_MASTER");

                entity.HasOne(d => d.BANK_)
                    .WithMany(p => p.COM_IMP_LCINFORMATION)
                    .HasForeignKey(d => d.BANK_ID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_BAS_BUYER_BANK_MASTER");

                entity.HasOne(d => d.CAT)
                    .WithMany(p => p.cOM_IMP_LCINFORMATIONs)
                    .HasForeignKey(d => d.CATID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_BAS_PRODCATEGORY");

                entity.HasOne(d => d.INS)
                    .WithMany(p => p.COM_IMP_LCINFORMATION)
                    .HasForeignKey(d => d.INSID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_BAS_INSURANCEINFO");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.COM_IMP_LCINFORMATION)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.COM_TENOR)
                    .WithMany(p => p.COM_IMP_LCINFORMATION)
                    .HasForeignKey(d => d.TID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_COM_TENOR");

                entity.HasOne(d => d.COM_IMP_LCTYPE)
                    .WithMany(p => p.COM_IMP_LCINFORMATION)
                    .HasForeignKey(d => d.LTID)
                    .HasConstraintName("FK_COM_IMP_LCINFORMATION_COM_IMP_LCTYPE");

                //entity.HasOne(d => d.ComExLcinfo)
                //    .WithMany(p => p.ComImpLcinformations)
                //    .HasForeignKey(d => d.LCID)
                //    .HasConstraintName("PK_COM_IMP_LCINFORMATION");
            });

            modelBuilder.Entity<COM_IMP_LCTYPE>(entity =>
            {
                entity.HasKey(e => e.LTID);

                entity.Property(e => e.TYPENAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<COM_IMP_DEL_STATUS>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_DYEING_TYPE>(entity =>
            {
                entity.HasKey(e => e.DID);

                entity.Property(e => e.DTYPE)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_FABRIC_COUNTINFO>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.FABCODE)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.YARNTYPE).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.RND_FABRIC_COUNTINFO)
                    .HasForeignKey(d => d.COUNTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABRIC_COUNTINFO_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.RND_FABRIC_COUNTINFO)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_RND_FABRIC_COUNTINFO_RND_FABRICINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.RND_FABRIC_COUNTINFO)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_RND_FABRIC_COUNTINFO_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.RND_FABRIC_COUNTINFO)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_RND_FABRIC_COUNTINFO_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.RND_FABRIC_COUNTINFO)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_RND_FABRIC_COUNTINFO_BAS_COLOR");

                entity.HasOne(d => d.YarnFor)
                    .WithMany(p => p.RND_FABRIC_COUNTINFO)
                    .HasForeignKey(d => d.YARNFOR)
                    .HasConstraintName("FK_RND_FABRIC_COUNTINFO_YARNFOR");
            });

            modelBuilder.Entity<RND_FABRICINFO>(entity =>
            {
                entity.HasKey(e => e.FABCODE);

                entity.Property(e => e.DEVID).HasMaxLength(50);

                entity.Property(e => e.BUYERREF).HasMaxLength(50);

                entity.Property(e => e.CFATDRY).HasMaxLength(50);

                entity.Property(e => e.CFATWET).HasMaxLength(50);

                entity.Property(e => e.COMPOSITION).HasMaxLength(250);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEVID).HasMaxLength(50);

                entity.Property(e => e.DOBBY).HasMaxLength(50);

                entity.Property(e => e.DYCODE).HasMaxLength(50);

                entity.Property(e => e.FABRICTYPE).HasMaxLength(50);

                entity.Property(e => e.FINISH_ROUTE).HasMaxLength(100);

                entity.Property(e => e.GRDECWARP).HasMaxLength(50);

                entity.Property(e => e.GRDECWEFT).HasMaxLength(50);

                entity.Property(e => e.LSBTESTNO).HasMaxLength(50);

                entity.Property(e => e.MASTERROLL).HasMaxLength(100);

                entity.Property(e => e.PH).HasMaxLength(50);

                entity.Property(e => e.PROGNO).HasMaxLength(50);

                entity.Property(e => e.REVISENO).HasMaxLength(50);

                entity.Property(e => e.SKEWMOVE).HasMaxLength(50);

                entity.Property(e => e.SRDECWARP).HasMaxLength(50);

                entity.Property(e => e.SRDECWEFT).HasMaxLength(50);

                entity.Property(e => e.STDECWARP).HasMaxLength(50);

                entity.Property(e => e.STDECWEFT).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);

                entity.Property(e => e.LSGTESTNO).HasMaxLength(50);

                entity.Property(e => e.ARCHIVE_NO).HasMaxLength(50);

                entity.Property(e => e.PROTOCOL_NO).HasMaxLength(50);

                entity.Property(e => e.COMPOSITION_DEC).HasMaxLength(50);

                entity.Property(e => e.WGDEC).HasMaxLength(50);

                entity.Property(e => e.WIDEC).HasMaxLength(50);

                entity.Property(e => e.RS).HasMaxLength(50);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_BAS_FABRICINFO_BAS_BUYERINFO");

                entity.HasOne(d => d.COLORCODENavigation)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_BAS_FABRICINFO_BAS_COLOR");

                entity.HasOne(d => d.D)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.DID)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_DYEING_TYPE");

                entity.HasOne(d => d.RND_FINISHTYPE)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.FINID)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_FINISHTYPE");

                entity.HasOne(d => d.RND_WEAVE)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.WID)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_WEAVE");

                entity.HasOne(d => d.RND_FINISHMC)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.MCID)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_FINISHMC");

                entity.HasOne(d => d.WV)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.WVID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_SAMPLE_INFO_WEAVING");

                entity.HasOne(d => d.FIN)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.SFINID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_SAMPLEINFO_FINISHING");

                entity.HasOne(d => d.LOOM)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.LOOMID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABRICINFO_LOOM_TYPE");

                entity.HasOne(d => d.FABTEST)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.LTSID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABRICINFO_RND_FABTEST_SAMPLE");

                entity.HasOne(d => d.UPD_BYNavigation)
                    .WithMany(p => p.RND_FABRICINFO)
                    .HasForeignKey(d => d.UPD_BY)
                    .HasConstraintName("FK_RND_FABRICINFO_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<RND_FABRICINFO_APPROVAL_DETAILS>(entity =>
            {
                entity.HasKey(e => e.RFAID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_BY).IsRequired();

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.RND_FABRICINFO_APPROVAL_DETAILS)
                    .HasForeignKey(d => d.FABCODE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABRICINFO_APPROVAL_DETAILS_RND_FABRICINFO");
            });

            modelBuilder.Entity<RND_FINISHTYPE>(entity =>
            {
                entity.HasKey(e => e.FINID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TYPENAME)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RND_SAMPLE_INFO_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.HasOne(d => d.COLORCODENavigation)
                    .WithMany(p => p.RND_SAMPLE_INFO_DETAILS)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DETAILS_BAS_COLOR");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.RND_SAMPLE_INFO_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.RND_SAMPLE_INFO_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.RND_SAMPLE_INFO_DETAILS)
                    .HasForeignKey(d => d.SUPPID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DETAILS_BAS_SUPPLIERINFO1");

                entity.HasOne(d => d.YARN)
                    .WithMany(p => p.RND_SAMPLE_INFO_DETAILS)
                    .HasForeignKey(d => d.YARNID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DETAILS_YARNFOR");

                entity.HasOne(d => d.SD)
                    .WithMany(p => p.RND_SAMPLE_INFO_DETAILS)
                    .HasForeignKey(d => d.SDID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DETAILS_RND_SAMPLE_INFO_DYEING");
            });

            modelBuilder.Entity<RND_SAMPLE_INFO_DYEING>(entity =>
            {
                entity.HasKey(e => e.SDID);

                entity.HasIndex(e => e.PROG_NO)
                    .HasName("IX_RND_SAMPLE_INFO_DYEING");

                entity.Property(e => e.PROG_NO)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.WARP_PROG_DATE).HasColumnType("datetime");

                entity.Property(e => e.COMMITED_DEL_DATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.SIDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.RSOrder).HasMaxLength(50);
                entity.Property(e => e.RNDTEAM).HasMaxLength(5);

                entity.HasOne(d => d.COLOR)
                    .WithMany(p => p.RND_SAMPLE_INFO_DYEING)
                    .HasForeignKey(d => d.COLORCODE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DYEING_BAS_COLOR");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.RND_SAMPLE_INFO_DYEING)
                    .HasForeignKey(d => d.COLORCODE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DYEING_BAS_BUYERINFO");

                entity.HasOne(d => d.D)
                    .WithMany(p => p.RND_SAMPLE_INFO_DYEING)
                    .HasForeignKey(d => d.DID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DYEING_RND_DYEING_TYPE");

                entity.HasOne(d => d.LOOM)
                    .WithMany(p => p.RND_SAMPLE_INFO_DYEING)
                    .HasForeignKey(d => d.LOOMID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DYEING_LOOM_TYPE");

                entity.HasOne(d => d.SDRF)
                    .WithMany(p => p.RND_SAMPLE_INFO_DYEING)
                    .HasForeignKey(d => d.SDRFID)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DYEING_MKT_SDRF_INFO");

                entity.HasOne(d => d.RNDPERSONNavigation)
                    .WithMany(p => p.RND_SAMPLE_INFO_DYEING)
                    .HasForeignKey(d => d.RNDPERSON)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_DYEING_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<RND_WEAVE>(entity =>
            {
                entity.HasKey(e => e.WID);

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_YARNCONSUMPTION>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.FABCODE)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.RND_YARNCONSUMPTION)
                    .HasForeignKey(d => d.COUNTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_YARNCONSUMPTION_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.RND_YARNCONSUMPTION)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_RND_YARNCONSUMPTION_RND_FABRICINFO");
            });

            modelBuilder.Entity<RND_SAMPLE_INFO_WEAVING>(entity =>
            {
                entity.HasKey(e => e.WVID);

                entity.Property(e => e.CLOSE_DATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.FABCODE)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DEV_NO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).IsRequired();

                entity.HasOne(d => d.LOOM)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.LOOMID)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_LOOM_TYPE");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_BAS_BUYERINFO");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.RND_CONCERN)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.MKTPerson)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.MKT_PERSON)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_MKT_TEAM");

                entity.HasOne(d => d.PROG_)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.PROG_ID)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_PL_SAMPLE_PROG_SETUP");

                entity.HasOne(d => d.WEAVENavigation)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.WEAVE)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_RND_WEAVE");

                entity.HasOne(d => d.SizingBeam)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_F_PR_SIZING_PROCESS_ROPE_DETAILS");

                entity.HasOne(d => d.SlasherBeamDetails)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.SBEAMID)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_F_PR_SLASHER_DYEING_DETAILS");

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING)
                    .HasForeignKey(d => d.SETNO)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<RND_SAMPLE_INFO_WEAVING_DETAILS>(entity =>
            {
                entity.HasKey(e => e.WVID);

                entity.Property(e => e.WVID).ValueGeneratedOnAdd();

                entity.HasOne(d => d.COLORCODENavigation)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .HasForeignKey(d => d.COLORCODE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_DETAILS_BAS_COLOR");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .HasForeignKey(d => d.SUPPID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_DETAILS_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.YARN)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .HasForeignKey(d => d.YARNID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_DETAILS_YARNFOR");

                entity.HasOne(d => d.WEAVING)
                    .WithMany(p => p.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .HasForeignKey(d => d.WVID_PARENT)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_SAMPLE_INFO_WEAVING_DETAILS_RND_SAMPLE_INFO_WEAVING");
            });

            modelBuilder.Entity<RND_FABTEST_GREY>(entity =>
            {
                entity.HasKey(e => e.LTGID);

                entity.Property(e => e.LAB_NO).HasMaxLength(50);

                entity.Property(e => e.AWEPI).HasMaxLength(50);

                entity.Property(e => e.AWPPI).HasMaxLength(50);

                entity.Property(e => e.BATCHNO).HasMaxLength(50);

                entity.Property(e => e.BWEPI).HasMaxLength(50);

                entity.Property(e => e.BWPPI).HasMaxLength(50);

                entity.Property(e => e.GRWRAP).HasMaxLength(50);

                entity.Property(e => e.GRWEFT).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEVELOPMENTNO).HasMaxLength(50);

                entity.Property(e => e.GREPI).HasMaxLength(50);

                entity.Property(e => e.GRPPI).HasMaxLength(50);

                entity.Property(e => e.LTGDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.SKEWMOVE).HasMaxLength(50);

                entity.Property(e => e.SKEWWASH).HasMaxLength(50);

                entity.Property(e => e.SRGRWEFT).HasMaxLength(50);

                entity.Property(e => e.SRGRWRAP).HasMaxLength(50);

                entity.Property(e => e.UNWASHEDBY).HasDefaultValueSql("((0))");

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WASHEDBY).HasDefaultValueSql("((0))");

                entity.Property(e => e.WGGRAW).HasMaxLength(50);

                entity.Property(e => e.WGGRBW).HasMaxLength(50);

                entity.Property(e => e.WIGRAW).HasMaxLength(50);

                entity.Property(e => e.WIGRBW).HasMaxLength(50);

                entity.HasOne(d => d.EMP_UNWASHEDBY)
                    .WithMany(p => p.RND_FABTEST_GREY_UNWASHEDBY)
                    .HasForeignKey(d => d.UNWASHEDBY)
                    .HasConstraintName("FK_RND_FABTEST_GREY_F_HRD_EMPLOYEE_BW");

                entity.HasOne(d => d.EMP_WASHEDBY)
                    .WithMany(p => p.RND_FABTEST_GREY_WASHEDBY)
                    .HasForeignKey(d => d.WASHEDBY)
                    .HasConstraintName("FK_RND_FABTEST_GREY_F_HRD_EMPLOYEE_AW");

                entity.HasOne(d => d.PROG)
                    .WithMany(p => p.RND_FABTEST_GREY)
                    .HasForeignKey(d => d.PROGID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABTEST_GREY_PL_SAMPLE_PROG_SETUP");

                entity.HasOne(d => d.DOFF)
                    .WithMany(p => p.RND_FABTEST_GREY)
                    .HasForeignKey(d => d.DOFF_ROLL_NO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABTEST_GREY_F_PR_WEAVING_PROCESS_DETAILS_B");

                entity.HasOne(d => d.ORDER_RPT)
                    .WithMany(p => p.RND_FABTEST_GREY)
                    .HasForeignKey(d => d.ORDER_REPEAT)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABTEST_GREY_RND_ORDER_REPEAT");

                entity.HasOne(d => d.SHIFTINFO)
                    .WithMany(p => p.RND_FABTEST_GREY)
                    .HasForeignKey(d => d.SHIFT)
                    .HasConstraintName("FK_RND_FABTEST_GREY_F_HR_SHIFT_INFO");

                entity.HasOne(d => d.PROGN)
                    .WithMany(p => p.RND_FABTEST_GREY)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_RND_FABTEST_GREY_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<RND_FABTEST_SAMPLE>(entity =>
            {
                entity.HasKey(e => e.LTSID);

                entity.Property(e => e.CFATDRY).HasMaxLength(50);

                entity.Property(e => e.CFATNET).HasMaxLength(50);

                entity.Property(e => e.COLORCNG_ACID).HasMaxLength(50);

                entity.Property(e => e.COLORCNG_ALKA).HasMaxLength(50);

                entity.Property(e => e.COLORSTN).HasMaxLength(50);

                entity.Property(e => e.COLORSTN_ACID).HasMaxLength(50);

                entity.Property(e => e.COLORSTN_ALKA).HasMaxLength(50);

                entity.Property(e => e.COMMENTS).HasMaxLength(100);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FABCOMP).HasMaxLength(100);

                entity.Property(e => e.FNEPI).HasMaxLength(50);

                entity.Property(e => e.FNPPI).HasMaxLength(50);

                entity.Property(e => e.FORMALDEHYDE).HasMaxLength(50);

                entity.Property(e => e.GRFNWARP).HasMaxLength(50);

                entity.Property(e => e.GRFNWEFT).HasMaxLength(50);

                entity.Property(e => e.ISSUE).HasMaxLength(50);

                entity.Property(e => e.LTSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MCNAME).HasMaxLength(50);

                entity.Property(e => e.METHOD).HasMaxLength(50);

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.PILLGRADE).HasMaxLength(50);

                entity.Property(e => e.PILLRUBS).HasMaxLength(50);

                entity.Property(e => e.PROCESSROUTE).HasMaxLength(50);

                entity.Property(e => e.PROGNO).HasMaxLength(50);

                entity.Property(e => e.SHADECNG).HasMaxLength(50);

                entity.Property(e => e.SHIFTNAME).HasMaxLength(50);

                entity.Property(e => e.SKEWMOVE).HasMaxLength(50);

                entity.Property(e => e.SLPWARP).HasMaxLength(50);

                entity.Property(e => e.SLPWEFT).HasMaxLength(50);

                entity.Property(e => e.SPIRALIRTY_B).HasMaxLength(50);

                entity.Property(e => e.SPIRALITY_A).HasMaxLength(50);

                entity.Property(e => e.SRFNWARP).HasMaxLength(50);

                entity.Property(e => e.SRFNWEFT).HasMaxLength(50);

                entity.Property(e => e.STFNWARP).HasMaxLength(50);

                entity.Property(e => e.STFNWEFT).HasMaxLength(50);

                entity.Property(e => e.TESTMETHOD).HasMaxLength(50);

                entity.Property(e => e.TNWARP).HasMaxLength(50);

                entity.Property(e => e.TNWEFT).HasMaxLength(50);

                entity.Property(e => e.TRWARP).HasMaxLength(50);

                entity.Property(e => e.TRWEFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WGDEAD).HasMaxLength(50);

                entity.Property(e => e.WGFNAW).HasMaxLength(50);

                entity.Property(e => e.WGFNBW).HasMaxLength(50);

                entity.Property(e => e.WIFNAW).HasMaxLength(50);

                entity.Property(e => e.WIFNBW).HasMaxLength(50);

                entity.Property(e => e.WIFNCUT).HasMaxLength(50);

                entity.HasOne(d => d.UNWASHEDBYNavigation)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_UNWASHEDBY)
                    .HasForeignKey(d => d.UNWASHEDBY)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_F_HRD_EMPLOYEE_U");

                entity.HasOne(d => d.WASHEDBYNavigation)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_WASHEDBY)
                    .HasForeignKey(d => d.WASHEDBY)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_F_HRD_EMPLOYEE_W");

                entity.HasOne(d => d.SFIN)
                    .WithMany(p => p.RND_FABTEST_SAMPLE)
                    .HasForeignKey(d => d.SFINID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_RND_SAMPLE_INFO_FINISHING");

                entity.HasOne(d => d.TROLLY)
                    .WithMany(p => p.RND_FABTEST_SAMPLE)
                    .HasForeignKey(d => d.TROLLEYNO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_F_PR_FIN_TROLLY");

                entity.HasOne(d => d.SHIFTINFO)
                    .WithMany(p => p.RND_FABTEST_SAMPLE)
                    .HasForeignKey(d => d.SHIFTNAME)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_F_HR_SHIFT_INFO");

                entity.HasOne(d => d.TEST)
                    .WithMany(p => p.RND_FABTEST_SAMPLE)
                    .HasForeignKey(d => d.TESTMETHOD)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_F_BAS_TESTMETHOD");

                entity.HasOne(d => d.LOOMINFO)
                    .WithMany(p => p.RND_FABTEST_SAMPLE)
                    .HasForeignKey(d => d.LOOM)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_F_LOOM_MACHINE_NO");

                entity.HasOne(d => d.PROGNONavigation)
                    .WithMany(p => p.RND_FABTEST_SAMPLE)
                    .HasForeignKey(d => d.PROGNO)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_PL_PRODUCTION_SETDISTRIBUTION");
            });

            //F_PR_WEAVING_WORKLOAD_EFFICIENCYLOOS

            modelBuilder.Entity<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>(entity =>
            {
                entity.HasKey(e => e.WWEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OP1).HasMaxLength(50);

                entity.Property(e => e.OP2).HasMaxLength(50);

                entity.Property(e => e.OP3).HasMaxLength(50);

                entity.Property(e => e.OP4).HasMaxLength(50);

                entity.Property(e => e.OP5).HasMaxLength(50);

                entity.Property(e => e.SHIFT_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.LOOM)
                    .WithMany(p => p.F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS)
                    .HasForeignKey(d => d.LOOMID)
                    .HasConstraintName("FK_F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS_LOOM_TYPE");

                entity.HasOne(d => d.SHIFT)
                    .WithMany(p => p.F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS)
                    .HasForeignKey(d => d.SHIFTID)
                    .HasConstraintName("FK_F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS_F_HR_SHIFT_INFO");

                entity.HasOne(d => d.SIEMP)
                    .WithMany(p => p.F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS)
                    .HasForeignKey(d => d.SIEMPID)
                    .HasConstraintName("FK_F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS_F_HRD_EMPLOYEE");
            });


            modelBuilder.Entity<RND_SAMPLEINFO_FINISHING>(entity =>
            {
                entity.HasKey(e => e.SFINID);

                entity.Property(e => e.BFNEPI).HasMaxLength(50);

                entity.Property(e => e.BFNPPI).HasMaxLength(50);

                entity.Property(e => e.BSRWARP).HasMaxLength(50);

                entity.Property(e => e.BSRWEFT).HasMaxLength(50);

                entity.Property(e => e.BSTWARP).HasMaxLength(50);

                entity.Property(e => e.BSTWEFT).HasMaxLength(50);

                entity.Property(e => e.BWGAW).HasMaxLength(50);

                entity.Property(e => e.BWGBW).HasMaxLength(50);

                entity.Property(e => e.BWIAW).HasMaxLength(50);

                entity.Property(e => e.BWIBW).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FINISHDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(100);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.STYLE_NAME).HasMaxLength(100);

                entity.Property(e => e.DEV_NO).HasMaxLength(100);

                entity.Property(e => e.FINISH_ROUTE).HasMaxLength(100);

                entity.Property(e => e.WASHPICK).HasMaxLength(50);

                entity.HasOne(d => d.LTG)
                    .WithMany(p => p.RND_SAMPLEINFO_FINISHING)
                    .HasForeignKey(d => d.LTGID)
                    .HasConstraintName("FK_RND_SAMPLEINFO_FINISHING_RND_FABTEST_GREY");

                entity.HasOne(d => d.COLOR)
                    .WithMany(p => p.RND_SAMPLEINFO_FINISHING)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_RND_SAMPLEINFO_FINISHING_BAS_COLOR");
            });


            modelBuilder.Entity<RND_FINISHMC>(entity =>
            {
                entity.HasKey(e => e.MCID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<COM_TENOR>(entity =>
            {
                entity.HasKey(e => e.TID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.NAME)
                    .IsRequired();
            });

            modelBuilder.Entity<COM_TRADE_TERMS>(entity =>
            {
                entity.HasKey(e => e.TTID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRADE_TERMS)
                    .IsRequired();
            });

            modelBuilder.Entity<COM_EX_CASHINFO>(entity =>
            {
                entity.HasKey(e => e.CASHID);

                entity.Property(e => e.CASHNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.ISSUEDATE).HasColumnType("datetime");

                entity.Property(e => e.LCDATE).HasColumnType("datetime");

                entity.Property(e => e.LCEXPIRY).HasColumnType("datetime");

                entity.Property(e => e.ITEMQTY_MTS).HasMaxLength(50);

                entity.Property(e => e.GARMENT_QTY).HasMaxLength(50);

                entity.Property(e => e.LCVALUE).HasMaxLength(50);

                entity.Property(e => e.BACKTOBACK_LCTYPE).HasMaxLength(50);

                entity.Property(e => e.ITEMQTY_YDS).HasMaxLength(50);

                entity.Property(e => e.OTHERS).HasMaxLength(250);

                entity.Property(e => e.RCVDDATE).HasColumnType("datetime");

                entity.Property(e => e.SUBDATE).HasColumnType("datetime");

                entity.Property(e => e.YARN_CASH_BTMA_DATE).HasMaxLength(50);

                entity.Property(e => e.YARN_CASH_BTMA_NO).HasMaxLength(50);

                entity.HasOne(d => d.LC)
                    .WithMany(p => p.COM_EX_CASHINFO)
                    .HasForeignKey(d => d.LCID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_EX_CASHINFO_COM_EX_LCINFO");

                entity.HasOne(d => d.CURRENCYNavigation)
                    .WithMany(p => p.COM_EX_CASHINFO)
                    .HasForeignKey(d => d.CURRENCY_ID)
                    .HasConstraintName("FK_COM_EX_CASHINFO_CURRENCY");
            });


            modelBuilder.Entity<COM_EX_GSPINFO>(entity =>
            {
                entity.HasKey(e => e.GSPID);

                entity.Property(e => e.DATE_OF_DELIVERY).HasColumnType("datetime");

                entity.Property(e => e.DELIVERY_DATE).HasColumnType("datetime");

                entity.Property(e => e.EXPITEMS).HasMaxLength(250);

                entity.Property(e => e.EXPLCDATE).HasMaxLength(100);

                entity.Property(e => e.EXPLCNO).HasMaxLength(50);

                entity.Property(e => e.GSPNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ISSUEDATE).HasColumnType("datetime");

                entity.Property(e => e.ITEMQTY_MTS).HasMaxLength(50);

                entity.Property(e => e.ITEMQTY_YDS).HasMaxLength(50);

                //entity.Property(e => e.LEFT_EXT).HasMaxLength(50);

                entity.Property(e => e.OTHERS).HasMaxLength(250);

                entity.Property(e => e.RCVDDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(100);

                entity.Property(e => e.RIGHT_EXT).HasMaxLength(50);
                entity.Property(e => e.SUBMITTED_TO).HasMaxLength(50);
                entity.Property(e => e.SUB_BNK_PRTY_DATE).HasColumnType("datetime");
                entity.Property(e => e.SUBDATE).HasColumnType("datetime");

                entity.Property(e => e.VCDATE).HasMaxLength(100);

                entity.Property(e => e.VCNO).HasMaxLength(50);



                entity.HasOne(d => d.INV)
                    .WithMany(p => p.ComExGspInfos)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_COM_EX_GSPINFO_COM_EX_INVOICEMASTER");
            });

            modelBuilder.Entity<COS_CERTIFICATION_COST>(entity =>
            {
                entity.HasKey(e => e.CID);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(150);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<COS_FIXEDCOST>(entity =>
            {
                entity.HasKey(e => e.FCID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<COS_PRECOSTING_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.FABCODE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.BasYarnCountInfo)
                    .WithMany(p => p.COS_PRECOSTING_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_COS_PRECOSTING_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.COS_PRECOSTING_DETAILS)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_COS_PRECOSTING_DETAILS_RND_FABRICINFO");

                entity.HasOne(d => d.CosPrecostingMaster)
                    .WithMany(p => p.COS_PRECOSTING_DETAILS)
                    .HasForeignKey(d => d.CSID)
                    .HasConstraintName("FK_COS_PRECOSTING_DETAILS_COS_PRECOSTING_MASTER");

                //entity.HasOne(d => d.YPBNavigation)
                //    .WithMany(p => p.COS_PRECOSTING_DETAILS)
                //    .HasForeignKey(d => d.YPB)
                //    .HasConstraintName("FK_COS_PRECOSTING_DETAILS_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.Yarnfor)
                    .WithMany(p => p.COS_PRECOSTING_DETAILS)
                    .HasForeignKey(d => d.YARN_FOR)
                    .HasConstraintName("FK_COS_PRECOSTING_DETAILS_YARNFOR");
            });

            modelBuilder.Entity<COS_PRECOSTING_MASTER>(entity =>
            {
                entity.HasKey(e => e.CSID);

                entity.Property(e => e.COMPOSITION).HasMaxLength(250);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CSDATE).HasColumnType("datetime");

                entity.Property(e => e.FABCODE).HasMaxLength(50);

                entity.Property(e => e.LOOMTYPE).HasMaxLength(50);

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.USERID).HasMaxLength(50);

                entity.Property(e => e.WGDEC).HasMaxLength(50);

                entity.Property(e => e.WIDEC).HasMaxLength(50);

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_BAS_COLOR");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_RND_FABRICINFO");

                entity.HasOne(d => d.CosFixedCost)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.FCID)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_COS_FIXEDCOST");

                entity.HasOne(d => d.FinishType)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.FINID)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_RND_FINISHTYPE");

                entity.HasOne(d => d.CosStandardCons)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.SCID)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_COS_STANDARD_CONS");

                entity.HasOne(d => d.Weave)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.WID)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_RND_WEAVE");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.CID)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_COS_CERTIFICATION_COST");

                entity.HasOne(d => d.T)
                    .WithMany(p => p.COS_PRECOSTING_MASTER)
                    .HasForeignKey(d => d.TID)
                    .HasConstraintName("FK_COS_PRECOSTING_MASTER_COM_TENOR");
            });

            modelBuilder.Entity<COS_STANDARD_CONS>(entity =>
            {
                entity.HasKey(e => e.SCID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<F_BAS_DEPARTMENT>(entity =>
            {
                entity.HasKey(e => e.DEPTID);

                entity.Property(e => e.DEPTNAME).HasMaxLength(50);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            //F_BAS_ASSET_LIST
            modelBuilder.Entity<F_BAS_ASSET_LIST>(entity =>
            {
                entity.HasKey(e => e.ASSET_ID);

                entity.Property(e => e.ASSET_NAME).HasMaxLength(150);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(100);

                entity.Property(e => e.OPT2).HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_BAS_ASSET_LIST)
                    .HasForeignKey(d => d.SEC_CODE)
                    .HasConstraintName("FK_F_BAS_ASSET_LIST_F_BAS_SECTION");
            });

            modelBuilder.Entity<F_BAS_DESIGNATION>(entity =>
            {
                entity.HasKey(e => e.DESID);

                entity.Property(e => e.DESNAME).HasMaxLength(50);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_SECTION>(entity =>
            {
                entity.HasKey(e => e.SECID);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SECNAME).HasMaxLength(50);

                entity.HasOne(d => d.DEPT)
                    .WithMany(p => p.F_BAS_SECTION)
                    .HasForeignKey(d => d.DEPTID)
                    .HasConstraintName("FK_F_BAS_SECTION_F_BAS_DEPARTMENT");
            });

            modelBuilder.Entity<F_BAS_SUBSECTION>(entity =>
            {
                entity.HasKey(e => e.SSECID);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SSECNAME).HasMaxLength(50);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_BAS_SUBSECTION)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_BAS_SUBSECTION_F_BAS_SECTION");
            });

            modelBuilder.Entity<F_HR_BLOOD_GROUP>(entity =>
            {
                entity.HasKey(e => e.BGID);

                entity.Property(e => e.BLOOD_GROUP)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_HR_EMP_EDUCATION>(entity =>
            {
                entity.HasKey(e => e.EEID);

                entity.Property(e => e.EXAM).HasMaxLength(50);

                entity.Property(e => e.INSTITUTE).HasMaxLength(50);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.OPN3).HasMaxLength(50);

                entity.Property(e => e.PASSING_YEAR).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.RESULT_CGPA).HasMaxLength(50);

                //entity.HasOne(d => d.EMP)
                //    .WithMany(p => p.F_HR_EMP_EDUCATION)
                //    .HasForeignKey(d => d.EMPID)
                //    .HasConstraintName("FK_F_HR_EMP_EDUCATION_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HR_EMP_FAMILYDETAILS>(entity =>
            {
                entity.HasKey(e => e.EFID);

                entity.Property(e => e.DOB).HasColumnType("datetime");

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.OPN3).HasMaxLength(50);

                entity.Property(e => e.PROFESSION).HasMaxLength(50);

                entity.Property(e => e.RELATION).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                //entity.HasOne(d => d.EMP)
                //    .WithMany(p => p.F_HR_EMP_FAMILYDETAILS)
                //    .HasForeignKey(d => d.EMPID)
                //    .HasConstraintName("FK_F_HR_EMP_FAMILYDETAILS_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HR_EMP_OFFICIALINFO>(entity =>
            {
                entity.HasKey(e => e.EOID);

                entity.Property(e => e.EMP_TYPE).HasMaxLength(50);

                entity.Property(e => e.IMEI01).HasMaxLength(50);

                entity.Property(e => e.IMEI02).HasMaxLength(50);

                entity.Property(e => e.INTERNET_PACK).HasMaxLength(50);

                entity.Property(e => e.JOINING_DATE).HasColumnType("datetime");

                entity.Property(e => e.OFF_CELLNO).HasMaxLength(50);

                entity.Property(e => e.MFS_DETAILS).HasMaxLength(50);

                entity.Property(e => e.MFS_TYPE).HasMaxLength(50);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.DEPT)
                    .WithMany(p => p.F_HR_EMP_OFFICIALINFO)
                    .HasForeignKey(d => d.DEPTID)
                    .HasConstraintName("FK_F_HR_EMP_OFFICIALINFO_F_BAS_DEPARTMENT");

                entity.HasOne(d => d.DES)
                    .WithMany(p => p.F_HR_EMP_OFFICIALINFO)
                    .HasForeignKey(d => d.DESID)
                    .HasConstraintName("FK_F_HR_EMP_OFFICIALINFO_F_BAS_DESIGNATION");

                //entity.HasOne(d => d.EMP)
                //    .WithMany(p => p.F_HR_EMP_OFFICIALINFO)
                //    .HasForeignKey(d => d.EMPID)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_F_HR_EMP_OFFICIALINFO_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_HR_EMP_OFFICIALINFO)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_HR_EMP_OFFICIALINFO_F_BAS_SECTION");

                entity.HasOne(d => d.SSEC)
                    .WithMany(p => p.F_HR_EMP_OFFICIALINFO)
                    .HasForeignKey(d => d.SSECID)
                    .HasConstraintName("FK_F_HR_EMP_OFFICIALINFO_F_BAS_SUBSECTION");
            });

            modelBuilder.Entity<F_HR_EMP_SALARYSETUP>(entity =>
            {
                entity.HasKey(e => e.ESID);

                entity.Property(e => e.ALW_NOTES).HasMaxLength(50);

                entity.Property(e => e.DED_NOTES).HasMaxLength(50);

                entity.Property(e => e.OPN1).HasMaxLength(50);

                entity.Property(e => e.OPN2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HR_EMP_SALARYSETUP)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HR_EMP_SALARYSETUP_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<DISTRICTS>(entity =>
            {
                entity.Property(e => e.BN_NAME).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.LAT).HasColumnType("numeric(18, 10)");

                entity.Property(e => e.LON).HasColumnType("numeric(18, 10)");

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.WEBSITE).HasMaxLength(100);

                entity.HasOne(d => d.DIVISION_)
                    .WithMany(p => p.DISTRICTS)
                    .HasForeignKey(d => d.DIVISION_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DISTRICTS_DIVISIONS");
            });

            modelBuilder.Entity<DIVISIONS>(entity =>
            {
                entity.Property(e => e.BN_NAME).HasMaxLength(50);

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UPOZILAS>(entity =>
            {
                entity.Property(e => e.BN_NAME).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.DISTRICT_)
                    .WithMany(p => p.UPOZILAS)
                    .HasForeignKey(d => d.DISTRICT_ID)
                    .HasConstraintName("FK_UPOZILAS_DISTRICTS");
            });

            modelBuilder.Entity<YARNFOR>(entity =>
            {
                entity.HasKey(e => e.YARNID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.YARNNAME).HasColumnName("YARNNAME");
            });

            modelBuilder.Entity<LOOM_TYPE>(entity =>
            {
                entity.HasKey(e => e.LOOMID);

                entity.Property(e => e.LOOM_TYPE_NAME)
                    .HasColumnName("LOOM_TYPE_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<MESSAGE>(entity =>
            {
                entity.Property(e => e.ReceiverId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.ReceiverName).IsRequired();
                entity.Property(e => e.SendAt).HasColumnType("datetime");
                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.Text).IsRequired();
                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_AspNetUsers");
            });

            modelBuilder.Entity<MESSAGE_INDIVIDUAL>(entity =>
            {
                entity.Property(e => e.ReceiverId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.ReceiverName).IsRequired();
                entity.Property(e => e.SendAt).HasColumnType("datetime");
                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.Text).IsRequired();
            });

            modelBuilder.Entity<MKT_DEV_TYPE>(entity =>
            {
                entity.HasKey(e => e.DEV_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.DEV_TYPE)
                    .IsRequired()
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<MKT_FACTORY>(entity =>
            {
                entity.HasKey(e => e.FID);

                entity.Property(e => e.ADDRESS).HasMaxLength(250);

                entity.Property(e => e.BIN_NO).HasMaxLength(50);

                entity.Property(e => e.DEL_ADDRESS).HasMaxLength(250);

                entity.Property(e => e.FACTORY_NAME)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<MKT_SDRF_INFO>(entity =>
            {
                entity.HasKey(e => e.SDRFID);

                entity.Property(e => e.PRIORITY).HasMaxLength(50);

                entity.Property(e => e.BUYER_REF).HasMaxLength(100);

                entity.Property(e => e.CONSTRUCTION).HasMaxLength(250);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FORTYPE).HasMaxLength(50);

                entity.Property(e => e.GSM_AW).HasMaxLength(50);

                entity.Property(e => e.GSM_BW).HasMaxLength(50);

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.ORDER_QTY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(250);

                entity.Property(e => e.REQUIRED_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPTIONAL_DATE).HasColumnType("datetime");

                entity.Property(e => e.ACTUAL_DATE).HasColumnType("datetime");

                entity.Property(e => e.SUBMIT_DATE_FACTORY).HasColumnType("datetime");

                entity.Property(e => e.REWORK_REASON).HasMaxLength(50);

                entity.Property(e => e.SDRF_NO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SEASON).HasMaxLength(50);

                entity.Property(e => e.TARGET_PRICE).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEIGHT_AW).HasMaxLength(50);

                entity.Property(e => e.WEIGHT_BW).HasMaxLength(50);

                entity.Property(e => e.WIDTH).HasMaxLength(50);

                entity.Property(e => e.PLANNING_REMARKS).HasMaxLength(100);

                entity.Property(e => e.Marketing_DGM_REMARKS).HasMaxLength(100);

                entity.Property(e => e.RND_REMARKS).HasMaxLength(100);

                entity.Property(e => e.PLANT_HEAD_REMARKS).HasMaxLength(100);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_BAS_BUYERINFO");

                entity.HasOne(d => d.DEV_)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.DEV_ID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_MKT_DEV_TYPE");

                entity.HasOne(d => d.BasBrandinfo)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.BRANDID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_BAS_BRANDINFO");

                entity.HasOne(d => d.MKT_PERSON)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.MKT_PERSON_ID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_MKT_TEAM");

                entity.HasOne(d => d.TEAM_M)
                    .WithMany(p => p.MKT_SDRF_INFO_M)
                    .HasForeignKey(d => d.TEAMID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_BAS_TEAMINFO_M");

                entity.HasOne(d => d.TEAM_R)
                    .WithMany(p => p.MKT_SDRF_INFO_R)
                    .HasForeignKey(d => d.RND_TEAM_ID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_BAS_TEAMINFO_R");

                entity.HasOne(d => d.COUNTRIES)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.BUYER_ORIGIN)
                    .HasConstraintName("FK_MKT_SDRF_INFO_COUNTRIES");

                entity.HasOne(d => d.FINISHTYPE)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.FINID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_RND_FINISHTYPE");

                entity.HasOne(d => d.RND_ANALYSIS_SHEET)
                    .WithMany(p => p.MKT_SDRF_INFO)
                    .HasForeignKey(d => d.AID)
                    .HasConstraintName("FK_MKT_SDRF_INFO_RND_ANALYSIS_SHEET");
            });

            modelBuilder.Entity<MKT_SUPPLIER>(entity =>
            {
                entity.HasKey(e => e.SUPP_ID);

                entity.Property(e => e.ADDRESS).HasMaxLength(250);

                entity.Property(e => e.BIN_NO).HasMaxLength(50);

                entity.Property(e => e.DEL_ADDRESS).HasMaxLength(250);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SUPP_NAME)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<MKT_TEAM>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESID).HasMaxLength(50);

                entity.Property(e => e.EMAIL).HasMaxLength(50);

                entity.Property(e => e.OLD_ID).HasMaxLength(50);

                entity.Property(e => e.PERSON_FULL_NAME).HasMaxLength(50);

                entity.Property(e => e.PERSON_NAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PHONE).HasMaxLength(20);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BasTeamInfo)
                    .WithMany(p => p.MKT_TEAM)
                    .HasForeignKey(d => d.TEAMID)
                    .HasConstraintName("FK_MKT_SWATCH_CARD_BAS_TEAMINFO");
            });

            modelBuilder.Entity<PDL_EMAIL_SENDER>(entity =>
            {
                entity.Property(e => e.FROM_EMAIL)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NETWORK_CREDENTIAL_PASSWORD).IsRequired();

                entity.Property(e => e.NETWORK_CREDENTIAL_USERNAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SMTP_CLIENT)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<COUNTRIES>(entity =>
            {
                entity.Property(e => e.CONTINENT_CODE).HasMaxLength(2);

                entity.Property(e => e.CONTINENT_NAME).HasMaxLength(50);

                entity.Property(e => e.NATIONALITY).HasMaxLength(50);

                entity.Property(e => e.ISO).HasMaxLength(2);

                entity.Property(e => e.ISO3).HasMaxLength(3);

                entity.Property(e => e.COUNTRY_NAME)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<COM_IMP_CSINFO>(entity =>
            {
                entity.HasKey(e => e.CSID);

                entity.Property(e => e.AITSTATUS)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'As Per Govt. Rules')");

                entity.Property(e => e.VAT)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'As Per Govt. Rules')");

                entity.Property(e => e.APPROVED_BY).HasMaxLength(50);

                entity.Property(e => e.AVAILABLE)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CRFACILITIES)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CSNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ENCLOSED).HasMaxLength(50);

                entity.Property(e => e.LISTED)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LOWESTPRICE)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.PAYMODE)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Cash/Cheque')");

                entity.Property(e => e.QUALITY)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SUBJECT).HasMaxLength(50);

                entity.Property(e => e.REVISEDNO).HasMaxLength(50);

                entity.Property(e => e.NEXT_REVIEW_DATE).HasColumnType("datetime");

                entity.Property(e => e.RECOMMENDATION).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.URECOM)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.WARRANTEE)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'As Per Govt. Rules')");

                entity.HasOne(d => d.IND)
                    .WithMany(p => p.COM_IMP_CSINFO)
                    .HasForeignKey(d => d.INDID)
                    .HasConstraintName("FK_COM_IMP_CSINFO_F_YS_INDENT_MASTER");
            });

            modelBuilder.Entity<COM_IMP_CSITEM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CSITEMID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UNIT).HasMaxLength(5);

                entity.HasOne(d => d.CS)
                    .WithMany(p => p.COM_IMP_CSITEM_DETAILS)
                    .HasForeignKey(d => d.CSID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COM_IMP_CSITEM_DETAILS_COM_IMP_CSINFO");
            });

            modelBuilder.Entity<COM_IMP_CSRAT_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CSRID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ORIGIN).HasMaxLength(50);

                entity.HasOne(d => d.CSITEM)
                    .WithMany(p => p.COM_IMP_CSRAT_DETAILS)
                    .HasForeignKey(d => d.CSITEMID)
                    .HasConstraintName("FK_COM_IMP_CSRAT_DETAILS_COM_IMP_CSITEM_DETAILS");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.COM_IMP_CSRAT_DETAILS)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_COM_IMP_CSRAT_DETAILS_BAS_SUPPLIERINFO");
            });

            modelBuilder.Entity<PL_SAMPLE_PROG_SETUP>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.PROCESS_TYPE).HasMaxLength(50);

                entity.Property(e => e.PROGRAM_TYPE).HasMaxLength(50);

                entity.Property(e => e.PROG_DATE).HasColumnType("datetime");

                entity.Property(e => e.PROG_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.STYLE_NAME).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.TYPE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARP_TYPE).HasMaxLength(50);

                entity.HasOne(d => d.SD)
                    .WithMany(p => p.PL_SAMPLE_PROG_SETUP)
                    .HasForeignKey(d => d.SDID)
                    .HasConstraintName("FK_PL_SAMPLE_PROG_SETUP_RND_SAMPLE_INFO_DYEING");
            });

            modelBuilder.Entity<RND_ANALYSIS_SHEET>(entity =>
            {
                entity.HasKey(e => e.AID);

                entity.Property(e => e.ADATE).HasColumnType("datetime");

                entity.Property(e => e.APPROVE_DATE).HasColumnType("datetime");

                entity.Property(e => e.BUYER_REF).HasMaxLength(50);

                entity.Property(e => e.CHECK_BY).HasMaxLength(50);

                entity.Property(e => e.CHECK_DATE).HasColumnType("datetime");

                entity.Property(e => e.COLORCODE).HasMaxLength(50);

                entity.Property(e => e.CONSTRUCTION).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FAB_CONTENT).HasMaxLength(50);

                entity.Property(e => e.FAB_TYPE).HasMaxLength(50);

                entity.Property(e => e.FN_EPI_PPI).HasMaxLength(50);

                entity.Property(e => e.FN_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.FN_WIDTH).HasMaxLength(50);

                entity.Property(e => e.LOOM_EPI_PPI).HasMaxLength(50);

                entity.Property(e => e.MKT_QUERY_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.RND_QUERY_NO).HasMaxLength(50);

                entity.Property(e => e.STRETCH_ABILITY).HasMaxLength(50);

                entity.Property(e => e.TOTAL_ENDS).HasMaxLength(50);

                entity.Property(e => e.REED_SPACE).HasMaxLength(50);

                entity.Property(e => e.FINISH_ROUTE).HasMaxLength(50);

                entity.Property(e => e.CHECKED_COMMENTS).HasMaxLength(50);

                entity.Property(e => e.HEAD_COMMENTS).HasMaxLength(100);

                entity.Property(e => e.SUBMITTED_DATE).HasColumnType("datetime");

                entity.Property(e => e.SUBMITTED_STYLE).HasMaxLength(50);

                entity.Property(e => e.SWATCH_RCV_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARP_CRIMP).HasMaxLength(50);

                entity.Property(e => e.WARP_FAB_LENGTH).HasMaxLength(50);

                entity.Property(e => e.WARP_RATIO).HasMaxLength(50);

                entity.Property(e => e.WARP_SHRINKAGE).HasMaxLength(50);

                entity.Property(e => e.WASH_EPI_PPI).HasMaxLength(50);

                entity.Property(e => e.WA_WEIGHT).HasMaxLength(50);

                entity.Property(e => e.WEFT_CRIMP).HasMaxLength(50);

                entity.Property(e => e.WEFT_FAB_LENGTH).HasMaxLength(50);

                entity.Property(e => e.WEFT_RATIO).HasMaxLength(50);

                entity.Property(e => e.WEFT_SHRINKAGE).HasMaxLength(50);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.RND_ANALYSIS_SHEET)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_BAS_BUYERINFO");

                entity.HasOne(d => d.Swatch)
                    .WithMany(p => p.RND_ANALYSIS_SHEET)
                    .HasForeignKey(d => d.SWATCH_ID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_MKT_SWATCH_CARD");

                entity.HasOne(d => d.FIN)
                    .WithMany(p => p.RND_ANALYSIS_SHEET)
                    .HasForeignKey(d => d.FINID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_RND_FINISHTYPE");

                entity.HasOne(d => d.MKT_PERSON_)
                    .WithMany(p => p.RND_ANALYSIS_SHEET)
                    .HasForeignKey(d => d.MKT_PERSON_ID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_MKT_TEAM");

                entity.HasOne(d => d.W)
                    .WithMany(p => p.RND_ANALYSIS_SHEET)
                    .HasForeignKey(d => d.WID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_RND_WEAVE");

                entity.HasOne(d => d.BasBrandinfo)
                    .WithMany(p => p.RND_ANALYSIS_SHEET)
                    .HasForeignKey(d => d.BRANDID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_BAS_BRANDINFO");
            });

            modelBuilder.Entity<RND_ANALYSIS_SHEET_DETAILS>(entity =>
            {
                entity.HasKey(e => e.ADID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.YARN_FOR).HasMaxLength(50);

                entity.Property(e => e.YARN_LENGTH).HasMaxLength(50);

                entity.Property(e => e.YARN_WEIGHT).HasMaxLength(50);

                entity.HasOne(d => d.A)
                    .WithMany(p => p.RND_ANALYSIS_SHEET_DETAILS)
                    .HasForeignKey(d => d.AID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_DETAILS_RND_ANALYSIS_SHEET");

                entity.HasOne(d => d.BasYarnCountinfo)
                    .WithMany(p => p.RndAnalysisSheetDetailses)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_RND_ANALYSIS_SHEET_DETAILS_BAS_YARN_COUNTINFO");
            });

            modelBuilder.Entity<RND_PURCHASE_REQUISITION_MASTER>(entity =>
            {
                entity.HasKey(e => e.INDSLID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.RESEMPID).HasMaxLength(50);

                entity.Property(e => e.INDSLDATE).HasColumnType("datetime");

                entity.Property(e => e.INDENT_SL_NO).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REVISE_NO).HasMaxLength(50);

                entity.Property(e => e.REVISE_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.INDSLNO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YARN_FOR).HasMaxLength(50);

                entity.Property(e => e.STATUS)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.RND_PURCHASE_REQUISITION_MASTER)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.RESEMP)
                    .WithMany(p => p.RndPurchaseRequisitionMasters)
                    .HasForeignKey(d => d.RESEMPID)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_F_HRD_EMPLOYEE1");

                entity.HasOne(d => d.ORDERNO_SNavigation)
                    .WithMany(p => p.RND_PURCHASE_REQUISITION_MASTER)
                    .HasForeignKey(d => d.ORDERNO_S)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_MKT_SDRF_INFO");

                entity.HasOne(d => d.ORDER_NONavigation)
                    .WithMany(p => p.RND_PURCHASE_REQUISITION_MASTER)
                    .HasForeignKey(d => d.ORDER_NO)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.COSTREF)
                    .WithMany(p => p.RND_PURCHASE_REQUISITION_MASTER)
                    .HasForeignKey(d => d.COSTREFID)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_COS_PRECOSTING_MASTER");

                entity.HasOne(d => d.BasBuyerinfo)
                    .WithMany(p => p.RndPurchaseRequisitionMasters)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_BAS_BUYERINFO");

                entity.HasOne(d => d.RndFabricinfo)
                    .WithMany(p => p.RND_PURCHASE_REQUISITION_MASTER)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_RND_PURCHASE_REQUISITION_MASTER_RND_FABRICINFO");
            });

            modelBuilder.Entity<PL_ORDERWISE_LOTINFO>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.PL_ORDERWISE_LOTINFO)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_PL_ORDERWISE_LOTINFO_BAS_YARN_LOTINFO");

                //entity.HasOne(d => d.PO)
                //    .WithMany(p => p.PL_ORDERWISE_LOTINFO)
                //    .HasForeignKey(d => d.POID)
                //    .HasConstraintName("FK_PL_ORDERWISE_LOTINFO_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.PL_ORDERWISE_LOTINFO)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_PL_ORDERWISE_LOTINFO_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.YARNFOR)
                    .WithMany(p => p.PL_ORDERWISE_LOTINFO)
                    .HasForeignKey(d => d.YARNTYPE)
                    .HasConstraintName("FK_PL_ORDERWISE_LOTINFO_YARNFOR");
            });

            modelBuilder.Entity<RND_MSTR_ROLL>(entity =>
            {
                entity.HasKey(e => e.MID);

                entity.Property(e => e.MASTER_ROLL)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.RND_MSTR_ROLL)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_RND_MSTR_ROLL_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.RND_MSTR_ROLL)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_RND_MSTR_ROLL_BAS_SUPPLIERINFO");
            });

            modelBuilder.Entity<RND_ORDER_REPEAT>(entity =>
            {
                entity.HasKey(e => e.ORPTID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.ORPTNAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_ORDER_TYPE>(entity =>
            {
                entity.HasKey(e => e.OTYPEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OTYPENAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_PRODUCTION_ORDER>(entity =>
            {
                entity.HasKey(e => e.POID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DYENG_TYPE).HasMaxLength(50);

                entity.Property(e => e.LOOM_TYPE).HasMaxLength(50);

                entity.Property(e => e.NO_OF_BALL).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.PIRCVDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.M)
                    .WithMany(p => p.RND_PRODUCTION_ORDER)
                    .HasForeignKey(d => d.MID)
                    .HasConstraintName("FK_RND_PRODUCTION_ORDER_RND_MSTR_ROLL");

                entity.HasOne(d => d.ORPT)
                    .WithMany(p => p.RND_PRODUCTION_ORDER)
                    .HasForeignKey(d => d.ORPTID)
                    .HasConstraintName("FK_RND_PRODUCTION_ORDER_RND_ORDER_REPEAT");

                entity.HasOne(d => d.OTYPE)
                    .WithMany(p => p.RND_PRODUCTION_ORDER)
                    .HasForeignKey(d => d.OTYPEID)
                    .HasConstraintName("FK_RND_PRODUCTION_ORDER_RND_ORDER_TYPE");

                entity.HasOne(d => d.SO)
                    .WithMany(p => p.RND_PRODUCTION_ORDER)
                    .HasForeignKey(d => d.ORDERNO)
                    .HasConstraintName("FK_RND_PRODUCTION_ORDER_RND_SAMPLE_INFO_DYEING");

                entity.HasOne(d => d.RS)
                    .WithMany(p => p.RND_PRODUCTION_ORDER)
                    .HasForeignKey(d => d.RSNO)
                    .HasConstraintName("FK_RND_PRODUCTION_ORDER_COM_EX_PI_DETAILS");
            });

            modelBuilder.Entity<F_BAS_RECEIVE_TYPE>(entity =>
            {
                entity.HasKey(e => e.RCVTID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.RCVTYPE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_YS_LEDGER>(entity =>
            {
                entity.HasKey(e => e.LEDID);

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LEDNAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<F_YS_LOCATION>(entity =>
            {
                entity.HasKey(e => e.LOCID);

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LOCNAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.LOT).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FYarnFor)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.YARN_TYPE)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_YARNFOR");

                entity.HasOne(d => d.LEDGER)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.LEDGERID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_F_YS_LEDGER");

                entity.HasOne(d => d.LOCATION)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_F_YS_LOCATION");

                entity.HasOne(d => d.PROD)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.YRCVID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_F_YS_YARN_RECEIVE_MASTER");

                entity.HasOne(d => d.BasYarnLotinfo)
                    .WithMany(p => p.FYsYarnReceiveDetailses)
                    .HasForeignKey(d => d.LOT)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.FYsRawPer)
                    .WithMany(p => p.FYsYarnReceiveDetailses)
                    .HasForeignKey(d => d.RAW)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_F_YS_RAW_PER");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_MASTER>(entity =>
            {
                entity.HasKey(e => e.YRCVID);

                entity.Property(e => e.CHALLANDATE).HasColumnType("datetime");

                entity.Property(e => e.CHALLANNO).HasMaxLength(50);

                entity.Property(e => e.COMMENTS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.G_ENTRY_DATE).HasColumnType("datetime");

                entity.Property(e => e.G_ENTRY_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RCVFROM).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TUCK_NO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YRCVDATE).HasColumnType("datetime");

                entity.HasOne(d => d.INV)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_COM_IMP_INVOICEINFO");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_F_BAS_RECEIVE_TYPE");

                entity.HasOne(d => d.IND)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER)
                    .HasForeignKey(d => d.INDID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_F_YS_INDENT_MASTER");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_F_BAS_SECTION");


                entity.HasOne(d => d.SUBSEC)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER)
                    .HasForeignKey(d => d.SUBSECID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_F_BAS_SUBSECTION");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.FYsYarnReceiveMasters)
                    .HasForeignKey(d => d.RCVFROM)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_BAS_SUPPLIERINFO");
            });

            modelBuilder.Entity<F_YARN_REQ_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);
                entity.Property(e => e.ORDER_TYPE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.REQ_QTY).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.YSR)
                    .WithMany(p => p.F_YARN_REQ_DETAILS)
                    .HasForeignKey(d => d.YSRID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_F_YARN_REQ_MASTER");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_YARN_REQ_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.PO)
                    .WithMany(p => p.F_YARN_REQ_DETAILS)
                    .HasForeignKey(d => d.ORDERNO)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_YARN_REQ_DETAILS)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YARN_REQ_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.FBasUnits)
                    .WithMany(p => p.FYarnReqDetailses)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.RS)
                   .WithMany(p => p.F_YARN_REQ_DETAILS)
                   .HasForeignKey(d => d.RSID)
                   .HasConstraintName("FK_F_YARN_REQ_DETAILS_RND_SAMPLE_INFO_DYEING");

                entity.HasOne(d => d.RSCOUNT)
                    .WithMany(p => p.F_YARN_REQ_DETAILS)
                    .HasForeignKey(d => d.COUNTID_RS)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_BAS_YARN_COUNTINFO");
            });



            modelBuilder.Entity<F_YARN_REQ_MASTER>(entity =>
            {
                entity.HasKey(e => e.YSRID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.YSRNO).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YSRDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.YSRNO).HasMaxLength(50);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.F_YARN_REQ_MASTER)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_YARN_REQ_MASTER_F_BAS_SECTION");

                entity.HasOne(d => d.SubSection)
                    .WithMany(p => p.F_YARN_REQ_MASTER)
                    .HasForeignKey(d => d.SUBSECID)
                    .HasConstraintName("FK_F_YARN_REQ_MASTER_F_BAS_SUBSECTION");
            });

            // modelBuilder.Entity<BAS_YARN_COUNTINFO>(entity =>
            // {
            //     entity.HasKey(e => e.COUNTID);
            //
            //     entity.Property(e => e.COLOR).HasMaxLength(50);
            //
            //     entity.Property(e => e.COUNTNAME).IsRequired();
            //
            //     entity.Property(e => e.UNIT).HasMaxLength(50);
            // });

            modelBuilder.Entity<F_BAS_ISSUE_TYPE>(entity =>
            {
                entity.HasKey(e => e.ISSUID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ISSUTYPE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });


            modelBuilder.Entity<F_YARN_TRANSACTION>(entity =>
            {
                entity.HasKey(e => e.YTRNID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.YTRNDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_YARN_TRANSACTION)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.ISSUE)
                    .WithMany(p => p.F_YARN_TRANSACTION)
                    .HasForeignKey(d => d.ISSUEID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_YARN_TRANSACTION)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_F_BAS_RECEIVE_TYPE");

                entity.HasOne(d => d.YISSUE)
                    .WithMany(p => p.F_YARN_TRANSACTION)
                    .HasForeignKey(d => d.YISSUEID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_F_YS_YARN_ISSUE_DETAILS");

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_YARN_TRANSACTION)
                    .HasForeignKey(d => d.YRCVID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_F_YS_YARN_RECEIVE_DETAILS");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YARN_TRANSACTION)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_BAS_YARN_LOTINFO");
            });

            modelBuilder.Entity<F_YARN_QC_APPROVE>(entity =>
            {
                entity.HasKey(e => e.YQCA);

                entity.Property(e => e.APPROVED_BY).HasMaxLength(200);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YQCADATE).HasColumnType("datetime");

                entity.HasOne(d => d.TRNS)
                    .WithMany(p => p.F_YARN_QC_APPROVE)
                    .HasForeignKey(d => d.YRDID)
                    .HasConstraintName("FK_F_YARN_QC_APPROVE_F_YS_YARN_RECEIVE_DETAILS");
            });

            modelBuilder.Entity<F_YS_YARN_ISSUE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.TRANSID).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ISSUE_QTY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.TRANS)
                    .WithOne(p => p.F_YS_YARN_ISSUE_DETAILS)
                    .HasForeignKey<F_YS_YARN_ISSUE_DETAILS>(d => d.REQ_DET_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_F_YARN_REQ_DETAILS_S");

                entity.HasOne(d => d.YISSUE)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS)
                    .HasForeignKey(d => d.YISSUEID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_F_YS_YARN_ISSUE_MASTER");

                entity.HasOne(d => d.FBasUnits)
                    .WithMany(p => p.FYsYarnIssueDetailses)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.BasYarnCountinfo)
                    .WithMany(p => p.FYsYarnIssueDetailses)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.FYsLocation)
                    .WithMany(p => p.FYsYarnIssueDetailses)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_F_YS_LOCATION");

                entity.HasOne(d => d.RefBasYarnCountinfo)
                    .WithMany(p => p.RefFYsYarnIssueDetailses)
                    .HasForeignKey(d => d.MAIN_COUNTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_F_YS_YARN_ISSUE_DETAILS");

                entity.HasOne(d => d.RCVD)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS)
                    .HasForeignKey(d => d.RCVDID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_F_YS_YARN_RECEIVE_DETAILS");

                entity.HasOne(d => d.PO_EXTRA)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS)
                    .HasForeignKey(d => d.SO_NO)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_RND_PRODUCTION_ORDER_EXTRA");
            });

            modelBuilder.Entity<F_YS_YARN_ISSUE_MASTER>(entity =>
            {
                entity.HasKey(e => e.YISSUEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ISSUETO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.PURPOSE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YISSUEDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ISSUE)
                    .WithMany(p => p.F_YS_YARN_ISSUE_MASTER)
                    .HasForeignKey(d => d.ISSUEID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_MASTER_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.YSR)
                    .WithMany(p => p.F_YS_YARN_ISSUE_MASTER)
                    .HasForeignKey(d => d.YSRID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_MASTER_F_YARN_REQ_MASTER");
            });

            modelBuilder.Entity<MKT_SWATCH_CARD>(entity =>
            {
                entity.HasKey(e => e.SWCDID);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.MKT_SWATCH_CARD)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_MKT_SWATCH_CARD_BAS_BUYERINFO");

                entity.HasOne(d => d.TEAM)
                    .WithMany(p => p.MKT_SWATCH_CARD)
                    .HasForeignKey(d => d.MKTPERSON)
                    .HasConstraintName("FK_MKT_SWATCH_CARD_MKT_TEAM");

                entity.HasOne(d => d.BasTeamInfo)
                    .WithMany(p => p.MKT_SWATCH_CARD)
                    .HasForeignKey(d => d.MKTTEAM)
                    .HasConstraintName("FK_MKT_SWATCH_CARD_BAS_TEAMINFO");

                entity.HasOne(d => d.FN)
                    .WithMany(p => p.MKT_SWATCH_CARD)
                    .HasForeignKey(d => d.FNID)
                    .HasConstraintName("FK_MKT_SWATCH_CARD_RND_FINISHTYPE");

                entity.HasOne(d => d.BasColor)
                    .WithMany(p => p.MKT_SWATCH_CARD)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_MKT_SWATCH_CARD_BAS_COLOR");
            });

            modelBuilder.Entity<PL_BULK_PROG_SETUP_D>(entity =>
            {
                entity.HasKey(e => e.PROG_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.PROCESS_TYPE).HasMaxLength(50);

                entity.Property(e => e.PROGRAM_TYPE).HasMaxLength(50);

                entity.Property(e => e.PROG_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SET_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARP_TYPE).HasMaxLength(50);

                entity.HasOne(d => d.BLK_PROG_)
                    .WithMany(p => p.PL_BULK_PROG_SETUP_D)
                    .HasForeignKey(d => d.BLK_PROG_ID)
                    .HasConstraintName("FK_PL_BULK_PROG_SETUP_D_PL_BULK_PROG_SETUP_M");

                entity.HasOne(d => d.YARNFOR)
                    .WithMany(p => p.PL_BULK_PROG_SETUP_D)
                    .HasForeignKey(d => d.YARN_TYPE)
                    .HasConstraintName("FK_PL_BULK_PROG_SETUP_D_YARNFOR");
            });

            modelBuilder.Entity<PL_BULK_PROG_SETUP_M>(entity =>
            {
                entity.HasKey(e => e.BLK_PROGID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.RndProductionOrder)
                    .WithMany(p => p.PL_BULK_PROG_SETUP_M)
                    .HasForeignKey(d => d.ORDERNO)
                    .HasConstraintName("FK_PL_BULK_PROG_SETUP_M_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.PL_BULK_PROG_SETUP_M)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_PL_BULK_PROG_SETUP_M_RND_FABRICINFO");
            });

            modelBuilder.Entity<PL_BULK_PROG_YARN_D>(entity =>
            {
                entity.HasKey(e => e.YARN_ID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.PL_BULK_PROG_YARN_D)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_PL_BULK_PROG_YARN_D_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.SCOUNT)
                    .WithMany(p => p.PL_BULK_PROG_YARN_D)
                    .HasForeignKey(d => d.SCOUNTID)
                    .HasConstraintName("FK_PL_BULK_PROG_YARN_D_RND_SAMPLE_INFO_DETAILS");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.PL_BULK_PROG_YARN_D)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_PL_BULK_PROG_YARN_D_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.PROG_)
                    .WithMany(p => p.PL_BULK_PROG_YARN_D)
                    .HasForeignKey(d => d.PROG_ID)
                    .HasConstraintName("FK_PL_BULK_PROG_YARN_D_PL_BULK_PROG_SETUP_D");
            });

            modelBuilder.Entity<PL_PRODUCTION_PLAN_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SUBGROUPID);

                entity.Property(e => e.BEAM_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REED).HasMaxLength(50);

                entity.Property(e => e.RATIO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SUBGROUPDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GROUP)
                    .WithMany(p => p.PL_PRODUCTION_PLAN_DETAILS)
                    .HasForeignKey(d => d.GROUPID)
                    .HasConstraintName("FK_PL_PRODUCTION_PLAN_DETAILS_PL_PRODUCTION_PLAN_MASTER");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.PL_PRODUCTION_PLAN_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_PL_PRODUCTION_PLAN_DETAILS_BAS_YARN_LOTINFO");
            });

            modelBuilder.Entity<PL_PRODUCTION_PLAN_MASTER>(entity =>
            {
                entity.HasKey(e => e.GROUPID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DYEING_REFERANCE).HasMaxLength(50);

                entity.Property(e => e.GROUPDATE).HasColumnType("datetime");

                entity.Property(e => e.PRODUCTION_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.RND_DYEING_TYPE)
                    .WithMany(p => p.PL_PRODUCTION_PLAN_MASTER)
                    .HasForeignKey(d => d.DYEING_TYPE)
                    .HasConstraintName("FK_PL_PRODUCTION_PLAN_MASTER_RND_DYEING_TYPE");
            });

            modelBuilder.Entity<PL_PRODUCTION_SETDISTRIBUTION>(entity =>
            {
                entity.HasKey(e => e.SETID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.PROG_)
                    .WithMany(p => p.PL_PRODUCTION_SETDISTRIBUTION)
                    .HasForeignKey(d => d.PROG_ID)
                    .HasConstraintName("FK_PL_PRODUCTION_SETDISTRIBUTION_PL_BULK_PROG_SETUP_D");

                entity.HasOne(d => d.SUBGROUP)
                    .WithMany(p => p.PL_PRODUCTION_SETDISTRIBUTION)
                    .HasForeignKey(d => d.SUBGROUPID)
                    .HasConstraintName("FK_PL_PRODUCTION_SETDISTRIBUTION_PL_PRODUCTION_PLAN_DETAILS");
            });

            modelBuilder.Entity<PL_DYEING_MACHINE_TYPE>(entity =>
            {
                entity.HasKey(e => e.MC_TYPE_ID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.MACHINE_NO).HasMaxLength(50);

                entity.Property(e => e.PROCESS_TYPE).HasMaxLength(50);

                entity.Property(e => e.WARP_TYPE).HasMaxLength(50);

                entity.Property(e => e.TYPE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

            });

            modelBuilder.Entity<PL_PRODUCTION_SO_DETAILS>(entity =>
            {
                entity.HasKey(e => e.PP_SO_ID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GROUP)
                    .WithMany(p => p.PL_PRODUCTION_SO_DETAILS)
                    .HasForeignKey(d => d.GROUPID)
                    .HasConstraintName("FK_PL_PRODUCTION_SO_DETAILS_PL_PRODUCTION_PLAN_MASTER");

                entity.HasOne(d => d.PO)
                    .WithMany(p => p.PL_PRODUCTION_SO_DETAILS)
                    .HasForeignKey(d => d.POID)
                    .HasConstraintName("FK_PL_PRODUCTION_SO_DETAILS_RND_PRODUCTION_ORDER");
            });

            modelBuilder.Entity<AGEGROUP>(entity =>
            {
                entity.Property(e => e.AGEGROUPNAME).HasMaxLength(50);

                entity.Property(e => e.AGEGROUPNORMALIZEDNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<SEGMENTOTHERSIMILARNAME>(entity =>
            {
                entity.Property(e => e.NORMALIZEDOTHERSIMILARNAME).HasMaxLength(50);

                entity.Property(e => e.OTHERSIMILARNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<SEGMENTOTHERSIMILARRNDFABRICS>(entity =>
            {
                entity.Property(e => e.INPUT).HasMaxLength(150);
            });

            modelBuilder.Entity<SEGMENTSEASON>(entity =>
            {
                entity.Property(e => e.NORMALIZEDSEASONNAME).HasMaxLength(50);

                entity.Property(e => e.SEASONNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<TARGETCHARACTER>(entity =>
            {
                entity.Property(e => e.CHARACTERNAME).HasMaxLength(50);

                entity.Property(e => e.NORMALIZEDCHARACTERNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<TARGETFITSTYLE>(entity =>
            {
                entity.Property(e => e.FITSTYLENAME).HasMaxLength(50);

                entity.Property(e => e.NORMALIZEDFITSTYLENAME).HasMaxLength(50);
            });

            modelBuilder.Entity<TARGETGENDER>(entity =>
            {
                entity.Property(e => e.GENDERNAME).HasMaxLength(50);

                entity.Property(e => e.NORMALIZEDGENDER).HasMaxLength(50);
            });

            modelBuilder.Entity<TARGETPRICESEGMENT>(entity =>
            {
                entity.Property(e => e.NORMALIZEDPRICESEGMENTTYPE).HasMaxLength(50);

                entity.Property(e => e.PRICESEGMENTTYPE).HasMaxLength(50);
            });

            modelBuilder.Entity<SEGMENTCOMSEGMENT>(entity =>
            {
                entity.Property(e => e.COMSEGMENTNAME).HasMaxLength(50);

                entity.Property(e => e.NORMALIZEDCOMSEGMENTNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_REPORT>(entity =>
            {
                entity.HasKey(e => e.MRRID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.MRRDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.YRD)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_REPORT)
                    .HasForeignKey(d => d.YRDID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_REPORT_F_YS_YARN_RECEIVE_DETAILS");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>(entity =>
            {
                entity.HasKey(e => e.BALLID);

                entity.Property(e => e.BREAKS_ENDS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ENDS_ROPE).HasMaxLength(50);

                entity.Property(e => e.LINK_BALL_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .HasForeignKey(d => d.COUNT_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.WARP_PROG)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .HasForeignKey(d => d.WARPID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_F_PR_WARPING_PROCESS_ROPE_MASTER");

                entity.HasOne(d => d.BALL_ID_FKNavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .HasForeignKey(d => d.BALL_ID_FK)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_F_BAS_BALL_INFO");

                entity.HasOne(d => d.BALL_ID_FK_Link)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_LINK)
                    .HasForeignKey(d => d.LINK_BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_F_BAS_BALL_INFO_LINK");

                entity.HasOne(d => d.MACHINE_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .HasForeignKey(d => d.MACHINE_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_F_PR_WARPING_MACHINE");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .HasForeignKey(d => d.OPERATOR)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_F_HRD_EMPLOYEE");

            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_ROPE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.WARP_PROG_ID);

                entity.Property(e => e.BALL_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.PL_PRODUCTION_SETDISTRIBUTION)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_DETAILS_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.WARP)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.WARPID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_DETAILS_F_PR_WARPING_PROCESS_ROPE_MASTER");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_ROPE_MASTER>(entity =>
            {
                entity.HasKey(e => e.WARPID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DELIVERY_DATE).HasColumnType("datetime");

                entity.HasOne(d => d.SUBGROUP)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_MASTER)
                    .HasForeignKey(d => d.SUBGROUPID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_MASTER_PL_PRODUCTION_PLAN_DETAILS");
            });

            //F_PR_RECONE_MASTER, F_PR_RECONE_YARN_CONSUMPTION, F_PR_RECONE_YARN_DETAILS
            modelBuilder.Entity<F_PR_RECONE_MASTER>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CONVERTED).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_RECONE_YARN_CONSUMPTION>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.RECONE_)
                    .WithMany(p => p.F_PR_RECONE_YARN_CONSUMPTION)
                    .HasForeignKey(d => d.RECONE_ID)
                    .HasConstraintName("FK_F_PR_RECONE_YARN_CONSUMPTION_F_PR_RECONE_MASTER");
            });

            modelBuilder.Entity<F_PR_RECONE_YARN_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.BREAK_ENDS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ENDS_ROPE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.RECONE_)
                    .WithMany(p => p.F_PR_RECONE_YARN_DETAILS)
                    .HasForeignKey(d => d.RECONE_ID)
                    .HasConstraintName("FK_F_PR_RECONE_YARN_DETAILS_F_PR_RECONE_MASTER");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CONSM_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.COUNT_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.COUNT_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.WARP)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.WARPID)
                    .HasConstraintName(
                        "FK_F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS_F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS");
            });

            modelBuilder.Entity<F_PR_WARPING_MACHINE>(entity =>
            {
                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MACHINE_NAME).HasMaxLength(50);

                entity.Property(e => e.TYPE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });



            modelBuilder.Entity<F_PR_WARPING_PROCESS_DW_DETAILS>(entity =>
            {
                entity.HasKey(e => e.WARP_D_ID);

                entity.Property(e => e.BALL_LENGTH).HasMaxLength(50);

                entity.Property(e => e.BREAKS_ENDS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ENDS_ROPE).HasMaxLength(50);

                entity.Property(e => e.LEADLINE).HasMaxLength(50);

                entity.Property(e => e.LINK_BALL_LENGTH).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFTNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BALL_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_DETAILSBALL_NONavigation)
                    .HasForeignKey(d => d.BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_DETAILS_F_BAS_BALL_INFO");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.LINK_BALL_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_DETAILSLINK_BALL_NONavigation)
                    .HasForeignKey(d => d.LINK_BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_DETAILS_F_BAS_BALL_INFO_LINK");

                entity.HasOne(d => d.MACHINE_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_DETAILS)
                    .HasForeignKey(d => d.MACHINE_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_DETAILS_F_PR_WARPING_MACHINE");

                entity.HasOne(d => d.WARP_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_DETAILS)
                    .HasForeignKey(d => d.WARP_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_DETAILS_F_PR_WARPING_PROCESS_DW_MASTER");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_DW_MASTER>(entity =>
            {
                entity.HasKey(e => e.WARPID);

                entity.Property(e => e.BALL_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEL_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PRODDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TIME_END).HasColumnType("datetime");

                entity.Property(e => e.TIME_START).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARPLENGTH).HasMaxLength(50);

                entity.Property(e => e.WARPRATIO).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_MASTER_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CONSM_ID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.COUNT_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.WARP_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.WARP_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS_F_PR_WARPING_PROCESS_DW_MASTER");
            });

            modelBuilder.Entity<F_SAMPLE_GARMENT_RCV_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.DEV_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.QTY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.COLOR)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_D)
                    .HasForeignKey(d => d.COLORID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_D_BAS_COLOR");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_D)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_D_RND_FABRICINFO");

                entity.HasOne(d => d.LOC)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_D)
                    .HasForeignKey(d => d.LOCID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_D_F_SAMPLE_LOCATION");

                entity.HasOne(d => d.SGR)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_D)
                    .HasForeignKey(d => d.SGRID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_D_F_SAMPLE_GARMENT_RCV_M");

                entity.HasOne(d => d.SITEM)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_D)
                    .HasForeignKey(d => d.SITEMID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_D_F_SAMPLE_ITEM_DETAILS");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_D)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_D_BAS_BUYERINFO");
            });

            modelBuilder.Entity<F_SAMPLE_GARMENT_RCV_M>(entity =>
            {
                entity.HasKey(e => e.SGRID);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SGRDATE).HasColumnType("datetime");

                entity.Property(e => e.SFTRDATE).HasColumnType("datetime");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_M)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_M_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_SAMPLE_GARMENT_RCV_M)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_SAMPLE_GARMENT_RCV_M_F_BAS_SECTION");
            });

            modelBuilder.Entity<F_SAMPLE_ITEM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SITEMID);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_SAMPLE_LOCATION>(entity =>
            {
                entity.HasKey(e => e.LOCID);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_CHEM_STORE_INDENTMASTER>(entity =>
            {
                entity.HasKey(e => e.CINDID);

                entity.Property(e => e.CINDDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CINDNO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INDTYPE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.INDSL)
                    .WithMany(p => p.F_CHEM_STORE_INDENTMASTER)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_CHEM_STORE_INDENTMASTER_F_CHEM_PURCHASE_REQUISITION_MASTER");

                entity.HasOne(d => d.FChemStoreIndentType)
                    .WithMany(p => p.FChemStoreIndentmasters)
                    .HasForeignKey(d => d.INDTYPE)
                    .HasConstraintName("FK_F_CHEM_STORE_INDENTMASTER_F_CHEM_STORE_INDENT_TYPE");
            });

            modelBuilder.Entity<F_CHEM_STORE_PRODUCTINFO>(entity =>
            {
                entity.HasKey(e => e.PRODUCTID);

                entity.Property(e => e.OPT1).HasMaxLength(100);

                entity.Property(e => e.OLD_CODE).HasMaxLength(100);

                entity.Property(e => e.PROD_CODE).HasMaxLength(100);

                entity.Property(e => e.OPT2).HasMaxLength(100);

                entity.Property(e => e.OPT3).HasMaxLength(100);

                entity.Property(e => e.SIZE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(100);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.TYPENAVIGATION)
                    .WithMany(p => p.F_CHEM_STORE_PRODUCTINFO)
                    .HasForeignKey(d => d.TYPE)
                    .HasConstraintName("FK_F_CHEM_STORE_PRODUCTINFO_F_CHEM_TYPE");

                entity.HasOne(d => d.UNITNAVIGATION)
                    .WithMany(p => p.F_CHEM_STORE_PRODUCTINFO)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_CHEM_STORE_PRODUCTINFO_F_BAS_UNITS"); ;

                entity.HasOne(d => d.COUNTRIES)
                    .WithMany(p => p.FChemStoreProductinfos)
                    .HasForeignKey(d => d.ORIGIN)
                    .HasConstraintName("FK_F_CHEM_STORE_PRODUCTINFO_COUNTRIES");
            });

            modelBuilder.Entity<F_CHEM_STORE_INDENTDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);
                entity.Property(e => e.TRNSID).ValueGeneratedOnAdd();

                entity.Property(e => e.ADD_QTY).HasMaxLength(50);

                entity.Property(e => e.BAL_QTY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FULL_QTY).HasMaxLength(50);

                entity.Property(e => e.LOCATION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_CHEM_STORE_INDENTDETAILS)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_CHEM_STORE_INDENTDETAILS_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.TRNS)
                    .WithMany(p => p.F_CHEM_STORE_INDENTDETAILS)
                    .HasForeignKey(d => d.CINDID)
                    .HasConstraintName("FK_F_CHEM_STORE_INDENTDETAILS_F_CHEM_STORE_INDENTMASTER");

                entity.HasOne(d => d.INDSL)
                    .WithMany(p => p.F_CHEM_STORE_INDENTDETAILS)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_CHEM_STORE_INDENTDETAILS_F_CHEM_PURCHASE_REQUISITION_MASTER");

                entity.HasOne(d => d.FBasUnits)
                    .WithMany(p => p.FChemStoreIndentdetailses)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_CHEM_STORE_INDENTDETAILS_F_BAS_UNITS");
            });

            modelBuilder.Entity<F_CHEM_PURCHASE_REQUISITION_MASTER>(entity =>
            {
                entity.HasKey(e => e.INDSLID);

                entity.Property(e => e.CN_PERSON).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INDSLDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FBasDepartment)
                    .WithMany(p => p.FChemPurchaseRequisitionMasters)
                    .HasForeignKey(d => d.DEPTID)
                    .HasConstraintName("FK_F_CHEM_PURCHASE_REQUISITION_MASTER_F_BAS_DEPARTMENT");

                entity.HasOne(d => d.FBasSection)
                    .WithMany(p => p.FChemPurchaseRequisitionMasters)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_CHEM_PURCHASE_REQUISITION_MASTER_F_BAS_SECTION");

                entity.HasOne(d => d.FBasSubsection)
                    .WithMany(p => p.FChemPurchaseRequisitionMasters)
                    .HasForeignKey(d => d.SSECID)
                    .HasConstraintName("FK_F_CHEM_PURCHASE_REQUISITION_MASTER_F_BAS_SUBSECTION");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.FChemPurchaseRequisitionMasters)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_CHEM_PURCHASE_REQUISITION_MASTER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.ConcernEmployee)
                    .WithMany(p => p.FChemPurchaseRequisitionMastersForConcernEmployees)
                    .HasForeignKey(d => d.CN_PERSON)
                    .HasConstraintName("FK_F_CHEM_PURCHASE_REQUISITION_MASTER_F_HRD_EMPLOYEE1");
            });

            modelBuilder.Entity<F_BAS_BALL_INFO>(entity =>
            {
                entity.HasKey(e => e.BALLID);

                entity.Property(e => e.BALL_NO).HasMaxLength(50);

                entity.Property(e => e.FOR_).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<COM_IMP_CNFINFO>(entity =>
            {
                entity.HasKey(e => e.CNFID);

                entity.Property(e => e.ADDRESS).HasMaxLength(50);

                entity.Property(e => e.CELL_NO).HasMaxLength(50);

                entity.Property(e => e.CNFNAME).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.C_PERSON).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_CHEM_STORE_RECEIVE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.BATCHNO).HasMaxLength(50);

                entity.Property(e => e.CINDDATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CURRENCY).HasMaxLength(50);

                entity.Property(e => e.EXDATE).HasColumnType("datetime");

                entity.Property(e => e.MNGDATE).HasColumnType("datetime");

                entity.Property(e => e.MRRDATE).HasColumnType("datetime");

                entity.Property(e => e.REJ_QTY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UNIT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CHEMRCV)
                    .WithMany(p => p.F_CHEM_STORE_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.CHEMRCVID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_DETAILS_F_CHEM_STORE_RECEIVE_MASTER");

                entity.HasOne(d => d.FBasUnits)
                    .WithMany(p => p.FChemStoreReceiveDetailses)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_DETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.FChemStoreProductinfo)
                    .WithMany(p => p.FChemStoreReceiveDetailsesFromProductInfo)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_DETAILS_F_CHEM_STORE_PRODUCTINFO");
            });

            modelBuilder.Entity<F_CHEM_STORE_RECEIVE_MASTER>(entity =>
            {
                entity.HasKey(e => e.CHEMRCVID);

                entity.Property(e => e.CHALLAN_DATE).HasColumnType("datetime");

                entity.Property(e => e.CNF_CHALLAN_DATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GEDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CNF)
                    .WithMany(p => p.F_CHEM_STORE_RECEIVE_MASTER)
                    .HasForeignKey(d => d.CNFID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_COM_IMP_CNFINFO");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_CHEM_STORE_RECEIVE_MASTER)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_F_BAS_RECEIVE_TYPE");

                entity.HasOne(d => d.ComImpInvoiceinfo)
                    .WithMany(p => p.FChemStoreReceiveMasters)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_COM_IMP_INVOICEINFO");

                entity.HasOne(d => d.BasSupplierinfo)
                    .WithMany(p => p.FChemStoreReceiveMasters)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.ComImpLcinformation)
                    .WithMany(p => p.FChemStoreReceiveMasters)
                    .HasForeignKey(d => d.LC_ID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_COM_IMP_LCINFORMATION");

                entity.HasOne(d => d.Countries)
                    .WithMany(p => p.FChemStoreReceiveMasters)
                    .HasForeignKey(d => d.ORIGIN)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_COUNTRIES");

                entity.HasOne(d => d.RcvEmployee)
                    .WithMany(p => p.RcvFChemStoreReceiveMasters)
                    .HasForeignKey(d => d.RCVBY)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.CheckEmployee)
                    .WithMany(p => p.CheckFChemStoreReceiveMasters)
                    .HasForeignKey(d => d.CHECKBY)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_F_HRD_EMPLOYEE1");

                entity.HasOne(d => d.BasTransportinfo)
                    .WithMany(p => p.FChemStoreReceiveMasters)
                    .HasForeignKey(d => d.TRANSPID)
                    .HasConstraintName("FK_F_CHEM_STORE_RECEIVE_MASTER_BAS_TRANSPORTINFO");
            });

            modelBuilder.Entity<F_CHEM_TRANSECTION>(entity =>
            {
                entity.HasKey(e => e.CTRID);

                entity.Property(e => e.CTRDATE).HasColumnType("datetime");

                entity.Property(e => e.OP1).HasMaxLength(50);

                entity.Property(e => e.OP2).HasMaxLength(50);

                entity.Property(e => e.OP3).HasMaxLength(50);

                entity.HasOne(d => d.CRCV)
                    .WithMany(p => p.F_CHEM_TRANSECTION)
                    .HasForeignKey(d => d.CRCVID)
                    .HasConstraintName("FK_F_CHEM_TRANSECTION_F_CHEM_STORE_RECEIVE_DETAILS");

                entity.HasOne(d => d.ISSUE)
                    .WithMany(p => p.F_CHEM_TRANSECTION)
                    .HasForeignKey(d => d.ISSUEID)
                    .HasConstraintName("FK_F_CHEM_TRANSECTION_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_CHEM_TRANSECTION)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_CHEM_TRANSECTION_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_CHEM_TRANSECTION)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_CHEM_TRANSECTION_F_BAS_RECEIVE_TYPE");

                entity.HasOne(d => d.CISSUE)
                    .WithMany(p => p.F_CHEM_TRANSECTION)
                    .HasForeignKey(d => d.CISSUEID)
                    .HasConstraintName("FK_F_CHEM_TRANSECTION_F_CHEM_ISSUE_DETAILS");

                //entity.HasOne(d => d.GSISSUE)
                //    .WithMany(p => p.F_CHEM_TRANSECTION)
                //    .HasForeignKey(d => d.GSISSUETRNSID)
                //    .HasConstraintName("FK_F_CHEM_TRANSECTION_F_GS_ISSUE_DETAILS");
            });

            modelBuilder.Entity<F_DYEING_PROCESS_ROPE_CHEM>(entity =>
            {
                entity.HasKey(e => e.CHEM_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(5);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CHEM_PROD_)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_CHEM)
                    .HasForeignKey(d => d.CHEM_PROD_ID)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_CHEM_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.ROPE_D)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_CHEM)
                    .HasForeignKey(d => d.ROPE_DID)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_CHEM_F_DYEING_PROCESS_ROPE_MASTER");
            });

            modelBuilder.Entity<F_DYEING_PROCESS_ROPE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.ROPEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(5);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BALL)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.BALLID)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_DETAILS_F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS");

                entity.HasOne(d => d.ROPE_D)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.ROPE_DID)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_DETAILS_F_DYEING_PROCESS_ROPE_MASTER");

                entity.HasOne(d => d.SUBGROUP)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.SUBGROUPID)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_DETAILS_PL_PRODUCTION_PLAN_DETAILS");

                entity.HasOne(d => d.CAN_NONavigation)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.CAN_NO)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_DETAILS_F_PR_TUBE_INFO");

                entity.HasOne(d => d.ROPE_NONavigation)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.ROPE_NO)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_DETAILS_F_PR_ROPE_INFO");

                entity.HasOne(d => d.R_MACHINE_NONavigation)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.R_MACHINE_NO)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_DETAILS_F_PR_ROPE_MACHINE_INFO");
            });

            modelBuilder.Entity<F_DYEING_PROCESS_ROPE_MASTER>(entity =>
            {
                entity.HasKey(e => e.ROPE_DID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DYEING_CODE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GROUP)
                    .WithMany(p => p.F_DYEING_PROCESS_ROPE_MASTER)
                    .HasForeignKey(d => d.GROUPID)
                    .HasConstraintName("FK_F_DYEING_PROCESS_ROPE_MASTER_PL_PRODUCTION_PLAN_MASTER");
            });




            modelBuilder.Entity<F_PR_SLASHER_CHEM_CONSM>(entity =>
            {
                entity.HasKey(e => e.SL_CHEMID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.QTY).HasMaxLength(50);

                entity.Property(e => e.FOR_).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TYPE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CHEM_PROD)
                    .WithMany(p => p.F_PR_SLASHER_CHEM_CONSM)
                    .HasForeignKey(d => d.CHEM_PRODID)
                    .HasConstraintName("FK_F_PR_SLASHER_CHEM_CONSM_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.SL)
                    .WithMany(p => p.F_PR_SLASHER_CHEM_CONSM)
                    .HasForeignKey(d => d.SLID)
                    .HasConstraintName("FK_F_PR_SLASHER_CHEM_CONSM_F_PR_SLASHER_DYEING_MASTER");

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.F_PR_SLASHER_CHEM_CONSM)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_PR_SLASHER_CHEM_CONSM_F_BAS_UNITS");
            });

            modelBuilder.Entity<F_PR_SLASHER_DYEING_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SLDID);

                entity.Property(e => e.BEAM_TYPE).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_PR_SLASHER_DYEING_DETAILS)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_PR_SLASHER_DYEING_DETAILS_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.OFFICER)
                    .WithMany(p => p.F_PR_SLASHER_DYEING_DETAILS_OFFICER)
                    .HasForeignKey(d => d.OFFICERID)
                    .HasConstraintName("FK_F_PR_SLASHER_DYEING_DETAILS_F_HRD_EMPLOYEE_OFFICER");

                entity.HasOne(d => d.SL)
                    .WithMany(p => p.F_PR_SLASHER_DYEING_DETAILS)
                    .HasForeignKey(d => d.SLID)
                    .HasConstraintName("FK_F_PR_SLASHER_DYEING_DETAILS_F_PR_SLASHER_DYEING_MASTER");

                entity.HasOne(d => d.SL_M)
                    .WithMany(p => p.F_PR_SLASHER_DYEING_DETAILS)
                    .HasForeignKey(d => d.SL_MID)
                    .HasConstraintName("FK_F_PR_SLASHER_DYEING_DETAILS_F_PR_SLASHER_MACHINE_INFO");

                entity.HasOne(d => d.W_BEAM)
                    .WithMany(p => p.F_PR_SLASHER_DYEING_DETAILS)
                    .HasForeignKey(d => d.W_BEAMID)
                    .HasConstraintName("FK_F_PR_SLASHER_DYEING_DETAILS_F_WEAVING_BEAM");
            });

            modelBuilder.Entity<F_PR_SLASHER_DYEING_MASTER>(entity =>
            {
                entity.HasKey(e => e.SLID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(250);

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_SLASHER_DYEING_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_SLASHER_DYEING_MASTER_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_SLASHER_MACHINE_INFO>(entity =>
            {
                entity.HasKey(e => e.SL_MID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DETAILS).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SL_MNO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });


            modelBuilder.Entity<F_CS_CHEM_RECEIVE_REPORT>(entity =>
            {
                entity.HasKey(e => e.CMRRID);

                entity.Property(e => e.CMRDATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OTP1).HasMaxLength(50);

                entity.Property(e => e.OTP2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CMRR)
                    .WithMany(p => p.F_CS_CHEM_RECEIVE_REPORT)
                    .HasForeignKey(d => d.CRDID)
                    .HasConstraintName("FK_F_CS_CHEM_RECEIVE_REPORT_F_CHEM_STORE_RECEIVE_DETAILS");
            });

            modelBuilder.Entity<F_CHEM_QC_APPROVE>(entity =>
            {
                entity.HasKey(e => e.CQCA);

                entity.Property(e => e.APPROVED_BY).HasMaxLength(50);

                entity.Property(e => e.CQCADATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CRD)
                    .WithMany(p => p.F_CHEM_QC_APPROVE)
                    .HasForeignKey(d => d.CRDID)
                    .HasConstraintName("FK_F_CHEM_QC_APPROVE_F_CHEM_STORE_RECEIVE_DETAILS");
            });
            modelBuilder.Entity<F_CHEM_REQ_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CRQID);

                entity.Property(e => e.CREATED_AT).HasMaxLength(50);

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CSR)
                    .WithMany(p => p.F_CHEM_REQ_DETAILS)
                    .HasForeignKey(d => d.CSRID)
                    .HasConstraintName("FK_F_CHEM_REQ_DETAILS_F_CHEM_REQ_MASTER");

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_CHEM_REQ_DETAILS)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_CHEM_REQ_DETAILS_F_CHEM_STORE_PRODUCTINFO");
            });

            modelBuilder.Entity<F_CHEM_REQ_MASTER>(entity =>
            {
                entity.HasKey(e => e.CSRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CSRDATE).HasColumnType("datetime");

                entity.Property(e => e.CSRNO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DEPT)
                    .WithMany(p => p.F_CHEM_REQ_MASTER)
                    .HasForeignKey(d => d.DEPTID)
                    .HasConstraintName("FK_F_CHEM_REQ_MASTER_F_BAS_DEPARTMENT");

                entity.HasOne(d => d.FBasSection)
                    .WithMany(p => p.FChemReqMasters)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_CHEM_REQ_MASTER_F_BAS_SECTION");

                entity.HasOne(d => d.FBasSubsection)
                    .WithMany(p => p.FChemReqMasters)
                    .HasForeignKey(d => d.SSECID)
                    .HasConstraintName("FK_F_CHEM_REQ_MASTER_F_BAS_SUBSECTION");

                entity.HasOne(d => d.RequisitionEmployee)
                    .WithMany(p => p.FChemReqMasters)
                    .HasForeignKey(d => d.REQUISITIONBY)
                    .HasConstraintName("FK_F_CHEM_REQ_MASTER_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_PR_ROPE_INFO>(entity =>
            {
                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.ROPE_NO).HasMaxLength(5);
            });

            modelBuilder.Entity<F_PR_ROPE_MACHINE_INFO>(entity =>
            {
                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.ROPE_MC_NO).HasMaxLength(5);
            });

            modelBuilder.Entity<F_PR_TUBE_INFO>(entity =>
            {
                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TUBE_NO).HasMaxLength(5);
            });

            modelBuilder.Entity<F_CHEM_ISSUE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CISSDID);

                entity.Property(e => e.CISSDDATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OTP1).HasMaxLength(50);

                entity.Property(e => e.OTP2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CISSUE)
                    .WithMany(p => p.F_CHEM_ISSUE_DETAILS)
                    .HasForeignKey(d => d.CISSUEID)
                    .HasConstraintName("FK_F_CHEM_ISSUE_DETAILS_F_CHEM_ISSUE_MASTER");

                entity.HasOne(d => d.CREQ_DET_)
                    .WithMany(p => p.F_CHEM_ISSUE_DETAILS)
                    .HasForeignKey(d => d.CREQ_DET_ID)
                    .HasConstraintName("FK_F_CHEM_ISSUE_DETAILS_F_CHEM_REQ_DETAILS");

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_CHEM_ISSUE_DETAILS)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_CHEM_ISSUE_DETAILS_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.PRODUCTACCLIMATIZE)
                    .WithMany(p => p.F_CHEM_ISSUE_DETAILS_ACCLIMATIZE)
                    .HasForeignKey(d => d.ADJ_PRO_AGNST)
                    .HasConstraintName("FK_F_CHEM_ISSUE_DETAILS_F_CHEM_STORE_PRODUCTINFO1");

                entity.HasOne(d => d.CRCVIDDNavigation)
                    .WithMany(p => p.F_CHEM_ISSUE_DETAILS)
                    .HasForeignKey(d => d.CRCVIDD)
                    .HasConstraintName("FK_F_CHEM_ISSUE_DETAILS_F_CHEM_STORE_RECEIVE_DETAILS");
            });

            modelBuilder.Entity<F_CHEM_ISSUE_MASTER>(entity =>
            {
                entity.HasKey(e => e.CISSUEID);

                entity.Property(e => e.CISSUEID).ValueGeneratedOnAdd();

                entity.Property(e => e.CISSUEDATE).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ISSUETO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.PURPOSE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CISSUE)
                    .WithMany(p => p.F_CHEM_ISSUE_MASTER)
                    .HasForeignKey(d => d.ISSUEID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_CHEM_ISSUE_MASTER_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.CSR)
                    .WithMany(p => p.F_CHEM_ISSUE_MASTER)
                    .HasForeignKey(d => d.CSRID)
                    .HasConstraintName("FK_F_CHEM_ISSUE_MASTER_F_CHEM_REQ_MASTER");

                entity.HasOne(d => d.IssueFHrdEmployee)
                    .WithMany(p => p.IssueFChemIssueMasters)
                    .HasForeignKey(d => d.ISSUEBY)
                    .HasConstraintName("FK_F_CHEM_ISSUE_MASTER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.ReceiveFHrdEmployee)
                    .WithMany(p => p.ReceiveFChemIssueMasters)
                    .HasForeignKey(d => d.RECEIVEBY)
                    .HasConstraintName("FK_F_CHEM_ISSUE_MASTER_F_HRD_EMPLOYEE1");
            });


            modelBuilder.Entity<F_LCB_BEAM>(entity =>
            {
                entity.Property(e => e.BEAM_NO).HasMaxLength(50);

                entity.Property(e => e.FOR_).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_LCB_MACHINE>(entity =>
            {
                entity.Property(e => e.MACHINE_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_LCB_PRODUCTION_ROPE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.LCB_D_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(5);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CAN)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .HasForeignKey(d => d.CANID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_DETAILS_F_DYEING_PROCESS_ROPE_DETAILS");

                entity.HasOne(d => d.EMPLOYEE)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .HasForeignKey(d => d.EMPLOYEEID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_DETAILS_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.F_LCB_PRODUCTION_ROPE_MASTER)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .HasForeignKey(d => d.LCBPROID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_DETAILS_F_LCB_PRODUCTION_ROPE_MASTER");
            });

            modelBuilder.Entity<F_PR_INSPECTION_REMARKS>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<F_PR_INSPECTION_REJECTION_B>(entity =>
            {
                entity.HasKey(e => e.IBR_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DOFFING_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.TRANS_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DEFECT_)
                    .WithMany(p => p.F_PR_INSPECTION_REJECTION_B)
                    .HasForeignKey(d => d.DEFECT_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_REJECTION_B_F_PR_INSPECTION_DEFECTINFO");

                entity.HasOne(d => d.DOFF_)
                    .WithMany(p => p.F_PR_INSPECTION_REJECTION_B)
                    .HasForeignKey(d => d.DOFF_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_REJECTION_B_F_PR_WEAVING_PROCESS_DETAILS_B1");

                entity.HasOne(d => d.SECTION_)
                    .WithMany(p => p.F_PR_INSPECTION_REJECTION_B)
                    .HasForeignKey(d => d.SECTION_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_REJECTION_B_F_BAS_SECTION");

                entity.HasOne(d => d.SHIFTNavigation)
                    .WithMany(p => p.F_PR_INSPECTION_REJECTION_B)
                    .HasForeignKey(d => d.SHIFT)
                    .HasConstraintName("FK_F_PR_INSPECTION_REJECTION_B_F_HR_SHIFT_INFO");
            });

            modelBuilder.Entity<F_PR_INSPECTION_CONSTRUCTION>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<F_LCB_PRODUCTION_ROPE_PROCESS_INFO>(entity =>
            {
                entity.HasKey(e => e.LCB_P_ID);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.HasOne(d => d.BEAM)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_PROCESS_INFO)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_PROCESS_INFO_F_LCB_BEAM");

                entity.HasOne(d => d.LCB_D_)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_PROCESS_INFO)
                    .HasForeignKey(d => d.LCB_D_ID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_PROCESS_INFO_F_LCB_PRODUCTION_ROPE_DETAILS");

                entity.HasOne(d => d.MACHINE)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_PROCESS_INFO)
                    .HasForeignKey(d => d.MACHINEID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_PROCESS_INFO_F_LCB_MACHINE");
            });

            modelBuilder.Entity<F_LCB_PRODUCTION_ROPE_MASTER>(entity =>
            {
                entity.HasKey(e => e.LCBPROID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);


                //entity.HasOne(d => d.SET)
                //    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_MASTER)
                //    .HasForeignKey(d => d.SETID)
                //    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_MASTER_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.SUBGROUP)
                    .WithMany(p => p.F_LCB_PRODUCTION_ROPE_MASTER)
                    .HasForeignKey(d => d.SUBGROUPID)
                    .HasConstraintName("FK_F_LCB_PRODUCTION_ROPE_MASTER_PL_PRODUCTION_PLAN_DETAILS");
            });



            modelBuilder.Entity<F_PR_SIZING_PROCESS_ROPE_CHEM>(entity =>
            {
                entity.HasKey(e => e.S_CHEMID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.UNIT).HasMaxLength(10);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CHEM_PROD)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_CHEM)
                    .HasForeignKey(d => d.CHEM_PRODID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_CHEM_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_CHEM)
                    .HasForeignKey(d => d.SID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_CHEM_F_PR_SIZING_PROCESS_ROPE_MASTER");
            });

            modelBuilder.Entity<F_PR_SIZING_PROCESS_ROPE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SDID);

                entity.Property(e => e.BEAM_TYPE).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(2);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_DETAILS_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.SID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_DETAILS_F_PR_SIZING_PROCESS_ROPE_MASTER");

                entity.HasOne(d => d.S_M)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.S_MID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_DETAILS_F_SIZING_MACHINE");

                entity.HasOne(d => d.W_BEAM)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .HasForeignKey(d => d.W_BEAMID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_DETAILS_F_WEAVING_BEAM");
            });

            modelBuilder.Entity<F_PR_SIZING_PROCESS_ROPE_MASTER>(entity =>
            {
                entity.HasKey(e => e.SID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_SIZING_PROCESS_ROPE_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_SIZING_PROCESS_ROPE_MASTER_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_SIZING_MACHINE>(entity =>
            {
                entity.Property(e => e.MACHINE_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_WEAVING_BEAM>(entity =>
            {
                entity.Property(e => e.BEAM_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_UNITS>(entity =>
            {
                entity.HasKey(e => e.UID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_LOOM_MACHINE_NO>(entity =>
            {
                entity.Property(e => e.LOOM_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_WEAVING_BEAM_RECEIVING>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.RCVDBYNavigation)
                    .WithMany(p => p.F_PR_WEAVING_BEAM_RECEIVING)
                    .HasForeignKey(d => d.RCVDBY)
                    .HasConstraintName("FK_F_PR_WEAVING_BEAM_RECEIVING_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WEAVING_BEAM_RECEIVING)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WEAVING_BEAM_RECEIVING_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>(entity =>
            {
                entity.HasKey(e => e.WV_BEAMID);

                entity.Property(e => e.WV_BEAMID).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.MOUNT_TIME).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.WV_PROCESS)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .HasForeignKey(d => d.WV_PROCESSID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B_F_PR_WEAVING_PROCESS_MASTER_B");

                entity.HasOne(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B_F_PR_SIZING_PROCESS_ROPE_DETAILS");

                entity.HasOne(d => d.F_PR_SLASHER_DYEING_DETAILS)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .HasForeignKey(d => d.SBEAMID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B_F_PR_SLASHER_DYEING_DETAILS");

                entity.HasOne(d => d.LoomMachine)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .HasForeignKey(d => d.LOOM_ID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B_F_LOOM_MACHINE_NO");
            });

            modelBuilder.Entity<F_PR_WEAVING_PROCESS_BEAM_DETAILS_S>(entity =>
            {
                entity.HasKey(e => e.SWV_BEAMID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.MOUNT_TIME).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_BEAM_DETAILS_S)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_BEAM_DETAILS_S_F_PR_SIZING_PROCESS_ROPE_DETAILS");

                entity.HasOne(d => d.SW_PPROCESS)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_BEAM_DETAILS_S)
                    .HasForeignKey(d => d.SW_PPROCESSID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_BEAM_DETAILS_S_F_PR_WEAVING_PROCESS_MASTER_S");
            });

            modelBuilder.Entity<F_PR_WEAVING_PROCESS_DETAILS_B>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DOFF_TIME).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);
                entity.Property(e => e.OPT4).HasMaxLength(50);
                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DOFFER_NAMENavigation)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .HasForeignKey(d => d.DOFFER_NAME)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_B_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.LOOM_NONavigation)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .HasForeignKey(d => d.LOOM_NO)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_B_F_LOOM_MACHINE_NO");

                entity.HasOne(d => d.LOOM_TYPENavigation)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .HasForeignKey(d => d.LOOM_TYPE)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_B_LOOM_TYPE");

                entity.HasOne(d => d.WV_BEAM)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .HasForeignKey(d => d.WV_BEAMID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_B_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B");

                entity.HasOne(d => d.OTHER_DOFF)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .HasForeignKey(d => d.OTHERS_DOFF)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_B_F_PR_WEAVING_OTHER_DOFF");

                entity.HasOne(d => d.WV)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .HasForeignKey(d => d.SAMPLE_FABCODE)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_B_RND_SAMPLE_INFO_WEAVING");
            });

            modelBuilder.Entity<F_PR_WEAVING_PROCESS_DETAILS_S>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DOFFER).HasMaxLength(50);

                entity.Property(e => e.DOFF_TIME).HasColumnType("datetime");

                entity.Property(e => e.FABCODE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.LOOM_NONavigation)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_S)
                    .HasForeignKey(d => d.LOOM_NO)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_S_F_LOOM_MACHINE_NO");

                entity.HasOne(d => d.LOOM_TYPENavigation)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_S)
                    .HasForeignKey(d => d.LOOM_TYPE)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_S_LOOM_TYPE");

                entity.HasOne(d => d.SWV_BEAM)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_DETAILS_S)
                    .HasForeignKey(d => d.SWV_BEAMID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_DETAILS_S_F_PR_WEAVING_PROCESS_BEAM_DETAILS_S");
            });

            modelBuilder.Entity<F_PR_WEAVING_PROCESS_MASTER_B>(entity =>
            {
                entity.HasKey(e => e.WV_PROCESSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WV_PROCESS_DATE).HasColumnType("datetime");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_MASTER_B)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_MASTER_B_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_WEAVING_PROCESS_MASTER_S>(entity =>
            {
                entity.HasKey(e => e.SW_PPROCESSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SWP_PROCESS_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WEAVING_PROCESS_MASTER_S)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WEAVING_PROCESS_MASTER_S_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.ACT_BGT).HasMaxLength(50);

                entity.Property(e => e.CONSUMP).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RATIO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SET_BGT).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WASTE).HasMaxLength(50);

                entity.HasOne(d => d.WEAVING)
                    .WithMany(p => p.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.WV_PROCESSID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_F_PR_WEAVING_PROCESS_MASTER_B");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.SUPPID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_BAS_SUPPLIERINFO");
            });


            modelBuilder.Entity<F_PR_WEAVING_OTHER_DOFF>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });


            modelBuilder.Entity<F_FS_WASTAGE_PARTY>(entity =>
            {
                entity.HasKey(e => e.PID);

                entity.Property(e => e.ADDRESS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PHONE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });
            modelBuilder.Entity<F_GS_WASTAGE_ISSUE_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.WI)
                    .WithMany(p => p.F_GS_WASTAGE_ISSUE_D)
                    .HasForeignKey(d => d.WIID)
                    .HasConstraintName("FK_F_GS_WASTAGE_ISSUE_D_F_GS_WASTAGE_ISSUE_M");
            });

            modelBuilder.Entity<F_GS_WASTAGE_ISSUE_M>(entity =>
            {
                entity.HasKey(e => e.WIID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.THROUGH).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WIDATE).HasColumnType("datetime");

                entity.HasOne(d => d.FGsWastageParty)
                    .WithMany(p => p.F_GS_WASTAGE_ISSUE_M)
                    .HasForeignKey(d => d.PID)
                    .HasConstraintName("FK_F_GS_WASTAGE_ISSUE_M_F_GS_WASTAGE_PARTY");
            });

            modelBuilder.Entity<F_CHEM_TYPE>(entity =>
            {
                entity.HasKey(e => e.CTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CTYPE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });



            modelBuilder.Entity<F_PR_FIN_TROLLY>(entity =>
            {
                entity.HasKey(e => e.FIN_TORLLY_ID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<F_PR_FINISHING_BEAM_RECEIVE>(entity =>
            {
                entity.HasKey(e => e.FDRID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BEAM)
                    .WithMany(p => p.F_PR_FINISHING_BEAM_RECEIVE)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_F_PR_FINISHING_BEAM_RECEIVE_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_PR_FINISHING_BEAM_RECEIVE)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_PR_FINISHING_BEAM_RECEIVE_RND_FABRICINFO");

                entity.HasOne(d => d.RCVBYNavigation)
                    .WithMany(p => p.F_PR_FINISHING_BEAM_RECEIVE)
                    .HasForeignKey(d => d.RCVBY)
                    .HasConstraintName("FK_F_PR_FINISHING_BEAM_RECEIVE_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_FINISHING_BEAM_RECEIVE)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_FINISHING_BEAM_RECEIVE_PL_PRODUCTION_SETDISTRIBUTION");
            });


            modelBuilder.Entity<F_PR_FINISHING_FAB_PROCESS>(entity =>
            {
                entity.HasKey(e => e.FAB_PROCESSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FAB_PROCESSDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(5);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FAB_MACHINE)
                    .WithMany(p => p.F_PR_FINISHING_FAB_PROCESS)
                    .HasForeignKey(d => d.FAB_MACHINEID)
                    .HasConstraintName("FK_F_PR_FINISHING_FAB_PROCESS_F_PR_PROCESS_MACHINEINFO");

                entity.HasOne(d => d.FAB_PRO_TYPE)
                    .WithMany(p => p.F_PR_FINISHING_FAB_PROCESS)
                    .HasForeignKey(d => d.FAB_PRO_TYPEID)
                    .HasConstraintName("FK_F_PR_FINISHING_FAB_PROCESS_F_PR_PROCESS_TYPE_INFO");

                //entity.HasOne(d => d.FN_PROCESS)
                //    .WithMany(p => p.F_PR_FINISHING_FAB_PROCESS)
                //    .HasForeignKey(d => d.FN_PROCESSID)
                //    .HasConstraintName("FK_F_PR_FINISHING_FAB_PROCESS_F_PR_FINISHING_PROCESS_MASTER");

                entity.HasOne(d => d.PROCESS_BYNavigation)
                    .WithMany(p => p.F_PR_FINISHING_FAB_PROCESS)
                    .HasForeignKey(d => d.PROCESS_BY)
                    .HasConstraintName("FK_F_PR_FINISHING_FAB_PROCESS_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.TROLLYNONavigation)
                    .WithMany(p => p.F_PR_FINISHING_FAB_PROCESS)
                    .HasForeignKey(d => d.TROLLYNO)
                    .HasConstraintName("FK_F_PR_FINISHING_FAB_PROCESS_F_PR_FIN_TROLLY");
            });

            modelBuilder.Entity<F_PR_FINISHING_FNPROCESS>(entity =>
            {
                entity.HasKey(e => e.FIN_PROCESSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FIN_PROCESSDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OTHERS_DOFF).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(5);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FIN_PRO_TYPE)
                    .WithMany(p => p.F_PR_FINISHING_FNPROCESS)
                    .HasForeignKey(d => d.FIN_PRO_TYPEID)
                    .HasConstraintName("FK_F_PR_FINISHING_FNPROCESS_F_PR_FN_PROCESS_TYPEINFO");

                entity.HasOne(d => d.FN_MACHINE)
                    .WithMany(p => p.F_PR_FINISHING_FNPROCESS)
                    .HasForeignKey(d => d.FN_MACHINEID)
                    .HasConstraintName("FK_F_PR_FINISHING_FNPROCESS_F_PR_FN_MACHINE_INFO");

                entity.HasOne(d => d.FN_PROCESS)
                    .WithMany(p => p.F_PR_FINISHING_FNPROCESS)
                    .HasForeignKey(d => d.FN_PROCESSID)
                    .HasConstraintName("FK_F_PR_FINISHING_FNPROCESS_F_PR_FINISHING_PROCESS_MASTER");

                entity.HasOne(d => d.PROCESS_BYNavigation)
                    .WithMany(p => p.F_PR_FINISHING_FNPROCESS)
                    .HasForeignKey(d => d.PROCESS_BY)
                    .HasConstraintName("FK_F_PR_FINISHING_FNPROCESS_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_PR_FINISHING_FNPROCESS)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_PR_FINISHING_FNPROCESS_F_BAS_SECTION");

                entity.HasOne(d => d.TROLLNONavigation)
                    .WithMany(p => p.F_PR_FINISHING_FNPROCESS)
                    .HasForeignKey(d => d.TROLLNO)
                    .HasConstraintName("FK_F_PR_FINISHING_FNPROCESS_F_PR_FIN_TROLLY");
            });

            modelBuilder.Entity<F_PR_FINISHING_PROCESS_MASTER>(entity =>
            {
                entity.HasKey(e => e.FN_PROCESSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.FN_PROCESSDATE).HasColumnType("datetime");

                //entity.HasOne(d => d.MACHINE_PREPARATION)
                //    .WithMany(p => p.F_PR_FINISHING_PROCESS_MASTER)
                //    .HasForeignKey(d => d.FPMID)
                //    .HasConstraintName("FK_F_PR_FINISHING_PROCESS_MASTER_F_PR_FINISHING_MACHINE_PREPARATION");

                entity.HasOne(d => d.FABRICINFO)
                    .WithMany(p => p.F_PR_FINISHING_PROCESS_MASTER)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_PR_FINISHING_PROCESS_MASTER_RND_FABRICINFO");

                entity.HasOne(d => d.DOFF)
                    .WithMany(p => p.F_PR_FINISHING_PROCESS_MASTER)
                    .HasForeignKey(d => d.DOFF_ID)
                    .HasConstraintName("FK_F_PR_FINISHING_PROCESS_MASTER_F_PR_WEAVING_PROCESS_DETAILS_B");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_FINISHING_PROCESS_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_FINISHING_PROCESS_MASTER_PL_PRODUCTION_SETDISTRIBUTION");
            });


            modelBuilder.Entity<F_PR_FN_CHEMICAL_CONSUMPTION>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.RECIPE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CHEM_PROD_)
                    .WithMany(p => p.F_PR_FN_CHEMICAL_CONSUMPTION)
                    .HasForeignKey(d => d.CHEM_PROD_ID)
                    .HasConstraintName("FK_F_PR_FN_CHEMICAL_CONSUMPTION_F_CHEM_STORE_PRODUCTINFO");

                entity.HasOne(d => d.FPM)
                    .WithMany(p => p.F_PR_FN_CHEMICAL_CONSUMPTION)
                    .HasForeignKey(d => d.FPMID)
                    .HasConstraintName("FK_F_PR_FN_CHEMICAL_CONSUMPTION_F_PR_FINISHING_MACHINE_PREPARATION");
            });

            modelBuilder.Entity<F_PR_FN_MACHINE_INFO>(entity =>
            {
                entity.HasKey(e => e.FN_MACHINEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_FN_PROCESS_TYPEINFO>(entity =>
            {
                entity.HasKey(e => e.FIN_PRO_TYPEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<F_PR_PROCESS_MACHINEINFO>(entity =>
            {
                entity.HasKey(e => e.FBMACHINEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_PROCESS_TYPE_INFO>(entity =>
            {
                entity.HasKey(e => e.FBPRTYPEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);
            });

            modelBuilder.Entity<F_SAMPLE_DESPATCH_DETAILS>(entity =>
            {
                entity.HasKey(e => e.DPDID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEL_QTY).HasMaxLength(50);

                entity.Property(e => e.ISSUE_PERSON).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.REQ_QTY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BYER)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_DETAILS)
                    .HasForeignKey(d => d.BYERID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_DETAILS_BAS_BUYERINFO");

                entity.HasOne(d => d.DP)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_DETAILS)
                    .HasForeignKey(d => d.DPID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_DETAILS_F_SAMPLE_DESPATCH_MASTER");

                entity.HasOne(d => d.TRNS)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_DETAILS)
                    .HasForeignKey(d => d.TRNSID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_DETAILS_F_SAMPLE_GARMENT_RCV_D");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_DETAILS)
                    .HasForeignKey(d => d.UID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_DETAILS_F_BAS_UNITS");
            });

            modelBuilder.Entity<F_SAMPLE_DESPATCH_MASTER>(entity =>
            {
                entity.HasKey(e => e.DPID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DR)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_MASTER)
                    .HasForeignKey(d => d.DRID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_MASTER_F_BAS_DRIVERINFO");

                entity.HasOne(d => d.GPTYPE)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_MASTER)
                    .HasForeignKey(d => d.GPTYPEID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_MASTER_GATEPASS_TYPE");

                entity.HasOne(d => d.V)
                    .WithMany(p => p.F_SAMPLE_DESPATCH_MASTER)
                    .HasForeignKey(d => d.VID)
                    .HasConstraintName("FK_F_SAMPLE_DESPATCH_MASTER_F_BAS_VEHICLE_INFO");
            });

            modelBuilder.Entity<GATEPASS_TYPE>(entity =>
            {
                entity.HasKey(e => e.GPTYPEID);

                entity.Property(e => e.GPTYPENAME).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_DRIVERINFO>(entity =>
            {
                entity.HasKey(e => e.DRID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DETAILS).HasMaxLength(50);

                entity.Property(e => e.DRIVER_NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_VEHICLE_INFO>(entity =>
            {
                entity.HasKey(e => e.VID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.VNUMBER).HasMaxLength(50);

                entity.HasOne(d => d.VD)
                    .WithMany(p => p.F_BAS_VEHICLE_INFO)
                    .HasForeignKey(d => d.VDID)
                    .HasConstraintName("FK_F_BAS_VEHICLE_INFO_F_BAS_DRIVERINFO");

                entity.HasOne(d => d.VEHICLE_TYPENavigation)
                   .WithMany(p => p.F_BAS_VEHICLE_INFO)
                   .HasForeignKey(d => d.VEHICLE_TYPE)
                   .HasConstraintName("FK_F_BAS_VEHICLE_INFO_VEHICLE_TYPE");

            });

            modelBuilder.Entity<F_GS_ITEMCATEGORY>(entity =>
            {
                entity.HasKey(e => e.CATID);

                entity.Property(e => e.CATNAME).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_GS_ITEMSUB_CATEGORY>(entity =>
            {
                entity.HasKey(e => e.SCATID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SCATNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CAT)
                    .WithMany(p => p.F_GS_ITEMSUB_CATEGORY)
                    .HasForeignKey(d => d.CATID)
                    .HasConstraintName("FK_F_GS_ITEMSUB_CATEGORY_F_GS_ITEMCATEGORY");
            });

            modelBuilder.Entity<F_GS_PRODUCT_INFORMATION>(entity =>
            {
                entity.HasKey(e => e.PRODID);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.PROD_LOC).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PRODNAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.F_GS_PRODUCT_INFORMATION)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_GS_PRODUCT_INFORMATION_F_BAS_UNITS");
                entity.HasOne(d => d.SCAT)
                    .WithMany(p => p.F_GS_PRODUCT_INFORMATION)
                    .HasForeignKey(d => d.SCATID)
                    .HasConstraintName("FK_F_GS_PRODUCT_INFORMATION_F_GS_ITEMSUB_CATEGORY");
            });

            //F_PR_INSPECTION_CUTPCS_TRANSFER
            modelBuilder.Entity<F_PR_INSPECTION_CUTPCS_TRANSFER>(entity =>
            {
                entity.HasKey(e => e.CPID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.TRNS_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SHIFTNavigation)
                    .WithMany(p => p.F_PR_INSPECTION_CUTPCS_TRANSFER)
                    .HasForeignKey(d => d.SHIFT)
                    .HasConstraintName("FK_F_PR_INSPECTION_CUTPCS_TRANSFER_F_HR_SHIFT_INFO1");
            });

            //F_GS_WASTAGE_PARTY
            modelBuilder.Entity<F_GS_WASTAGE_PARTY>(entity =>
            {
                entity.HasKey(e => e.PID);

                entity.Property(e => e.ADDRESS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PHONE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_INSPECTION_BATCH>(entity =>
            {
                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_INSPECTION_DEFECT_POINT>(entity =>
            {
                entity.HasKey(e => e.DPID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DEF_TYPE)
                    .WithMany(p => p.F_PR_INSPECTION_DEFECT_POINT)
                    .HasForeignKey(d => d.DEF_TYPEID)
                    .HasConstraintName("FK_F_PR_INSPECTION_DEFECT_POINT_F_PR_INSPECTION_DEFECTINFO");

                entity.HasOne(d => d.ROLL_)
                    .WithMany(p => p.F_PR_INSPECTION_DEFECT_POINT)
                    .HasForeignKey(d => d.ROLL_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_DEFECT_POINT_F_PR_INSPECTION_PROCESS_DETAILS");
            });

            modelBuilder.Entity<F_PR_INSPECTION_DEFECTINFO>(entity =>
            {
                entity.HasKey(e => e.DEF_TYPEID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.CODE).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_INSPECTION_MACHINE>(entity =>
            {
                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_INSPECTION_PROCESS_DETAILS>(entity =>
            {
                entity.HasKey(e => e.ROLL_ID);

                entity.Property(e => e.ACT_CONS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEF_PCS).HasMaxLength(50);

                entity.Property(e => e.FAB_GRADE).HasMaxLength(50);

                entity.Property(e => e.DEFECT_FAULT_STATUS).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.ROLLNO).HasMaxLength(50);

                entity.Property(e => e.ROLL_INSPDATE).HasColumnType("datetime");

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                //entity.HasOne(d => d.BATCHNavigation)
                //    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                //    .HasForeignKey(d => d.BATCH)
                //    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_PR_INSPECTION_BATCH");

                entity.HasOne(d => d.CUT_PCS_SECTIONNavigation)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                    .HasForeignKey(d => d.CUT_PCS_SECTION)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_BAS_SECTION");

                entity.HasOne(d => d.INSP)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                    .HasForeignKey(d => d.INSPID)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_PR_INSPECTION_PROCESS_MASTER");

                entity.HasOne(d => d.MACHINE_)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                    .HasForeignKey(d => d.MACHINE_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_PR_INSPECTION_MACHINE");

                entity.HasOne(d => d.TROLLEYNONavigation)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                    .HasForeignKey(d => d.TROLLEYNO)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_PR_FINISHING_FNPROCESS");

                entity.HasOne(d => d.Operator)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                    .HasForeignKey(d => d.OPERATOR_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_HRD_EMPLOYEE");


                entity.HasOne(d => d.PROCESS_TYPENavigation)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_DETAILS)
                    .HasForeignKey(d => d.PROCESS_TYPE)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_DETAILS_F_PR_INSPECTION_PROCESS");
            });

            modelBuilder.Entity<F_PR_INSPECTION_PROCESS_MASTER>(entity =>
            {
                entity.HasKey(e => e.INSPID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INSPDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_MASTER_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.FabricInfo)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_MASTER)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_MASTER_RND_FABRICINFO");

                entity.HasOne(d => d.TROLLEYNONavigation)
                    .WithMany(p => p.F_PR_INSPECTION_PROCESS_MASTER)
                    .HasForeignKey(d => d.TROLLEYNO)
                    .HasConstraintName("FK_F_PR_INSPECTION_PROCESS_MASTER_F_PR_FINISHING_FNPROCESS");
            });

            modelBuilder.Entity<F_PR_INSPECTION_WASTAGE_TRANSFER>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });


            modelBuilder.Entity<F_FS_FABRIC_RCV_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.QC_APPROVE_DATE).HasColumnType("datetime");

                entity.Property(e => e.QC_REJECT_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_FS_FABRIC_RCV_DETAILS)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_DETAILS_RND_FABRICINFO");

                entity.HasOne(d => d.PO_NONavigation)
                    .WithMany(p => p.F_FS_FABRIC_RCV_DETAILS)
                    .HasForeignKey(d => d.PO_NO)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_DETAILS_COM_EX_PIMASTER");

                entity.HasOne(d => d.RCV)
                    .WithMany(p => p.F_FS_FABRIC_RCV_DETAILS)
                    .HasForeignKey(d => d.RCVID)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_DETAILS_F_FS_FABRIC_RCV_MASTER");

                entity.HasOne(d => d.ROLL_)
                    .WithMany(p => p.F_FS_FABRIC_RCV_DETAILS)
                    .HasForeignKey(d => d.ROLL_ID)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_DETAILS_F_PR_INSPECTION_PROCESS_DETAILS");

                entity.HasOne(d => d.SO_NONavigation)
                    .WithMany(p => p.F_FS_FABRIC_RCV_DETAILS)
                    .HasForeignKey(d => d.SO_NO)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_DETAILS_COM_EX_PI_DETAILS");

                entity.HasOne(d => d.LOCATIONNavigation)
                    .WithMany(p => p.F_FS_FABRIC_RCV_DETAILS)
                    .HasForeignKey(d => d.LOCATION)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_DETAILS_F_FS_LOCATION");
            });

            modelBuilder.Entity<F_FS_FABRIC_RCV_MASTER>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FFTR_NO).HasMaxLength(50);
                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_FS_FABRIC_RCV_MASTER)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_FS_FABRIC_RCV_MASTER_F_BAS_SECTION");
            });


            modelBuilder.Entity<F_FS_DO_BALANCE_FROM_ORACLE>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.BUYER_NAME).HasMaxLength(255);

                entity.Property(e => e.DEL_QTY_TOTAL).HasMaxLength(255);

                entity.Property(e => e.DOT_NET_DELIVERY).HasMaxLength(255);

                entity.Property(e => e.DO_DATE).HasColumnType("datetime");

                entity.Property(e => e.DO_REF).HasMaxLength(50);

                entity.Property(e => e.DO_UNIT).HasMaxLength(255);

                entity.Property(e => e.FABRIC_CODE).HasMaxLength(255);

                entity.Property(e => e.FABRIC_ID).HasMaxLength(255);

                entity.Property(e => e.LC_DATE).HasColumnType("datetime");

                entity.Property(e => e.LC_NO).HasMaxLength(255);

                entity.Property(e => e.RET_QTY).HasMaxLength(255);

                entity.Property(e => e.UNIT).HasMaxLength(255);
            });

            modelBuilder.Entity<F_FS_LOCATION>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATETD_BY).HasMaxLength(50);

                entity.Property(e => e.LOCATION).HasMaxLength(50);

                entity.Property(e => e.LOC_NO).HasMaxLength(50);

                entity.Property(e => e.RACK_NO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TYPE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });



            modelBuilder.Entity<F_FS_FABRIC_CLEARANCE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CL_D_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PICK_AW).HasMaxLength(50);

                entity.Property(e => e.PICK_BW).HasMaxLength(50);

                entity.Property(e => e.PROD_DATE).HasColumnType("datetime");

                entity.Property(e => e.SHADE_GROUP).HasMaxLength(50);

                entity.Property(e => e.SHRINKAGE_WARP).HasMaxLength(50);

                entity.Property(e => e.SHRINKAGE_WEFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WGAW).HasMaxLength(50);

                entity.Property(e => e.WGBW).HasMaxLength(50);

                entity.HasOne(d => d.CL)
                    .WithMany(p => p.F_FS_FABRIC_CLEARANCE_DETAILS)
                    .HasForeignKey(d => d.CLID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARANCE_DETAILS_F_FS_FABRIC_CLEARANCE_MASTER");

                entity.HasOne(d => d.ROLL_)
                    .WithMany(p => p.F_FS_FABRIC_CLEARANCE_DETAILS)
                    .HasForeignKey(d => d.ROLL_ID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARANCE_DETAILS_F_PR_INSPECTION_PROCESS_DETAILS");
            });

            modelBuilder.Entity<F_PR_INSPECTION_PROCESS>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });


            modelBuilder.Entity<F_FS_FABRIC_CLEARANCE_MASTER>(entity =>
            {
                entity.HasKey(e => e.CLID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DATE_FROM).HasColumnType("datetime");

                entity.Property(e => e.DATE_TO).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PACKING_LIST_DATE).HasColumnType("datetime");

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.ROLE_FROM).HasMaxLength(50);

                entity.Property(e => e.ROLE_TO).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_FS_FABRIC_CLEARANCE_MASTER)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARANCE_MASTER_RND_FABRICINFO");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.F_FS_FABRIC_CLEARANCE_MASTER)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARANCE_MASTER_BAS_BUYERINFO");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.F_FS_FABRIC_CLEARANCE_MASTER_FACTORY)
                    .HasForeignKey(d => d.FACTORYID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARANCE_MASTER_BAS_BUYERINFO_FACTORY");

                entity.HasOne(d => d.PO)
                    .WithMany(p => p.F_FS_FABRIC_CLEARANCE_MASTER)
                    .HasForeignKey(d => d.ORDER_NO)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARANCE_MASTER_RND_PRODUCTION_ORDER");
            });

            modelBuilder.Entity<H_SAMPLE_RECEIVING_D>(entity =>
            {
                entity.HasKey(e => e.RCVDID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.QTY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_D)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_D_BAS_BUYERINFO");

                entity.HasOne(d => d.RCV)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_D)
                    .HasForeignKey(d => d.RCVID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_D_H_SAMPLE_RECEIVING_M");

                entity.HasOne(d => d.TRNS)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_D)
                    .HasForeignKey(d => d.TRNSID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_D_F_SAMPLE_GARMENT_RCV_D");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_D)
                    .HasForeignKey(d => d.UID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_D_F_BAS_UNITS");
            });

            modelBuilder.Entity<H_SAMPLE_RECEIVING_M>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.GPNO).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(250);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DP)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_M)
                    .HasForeignKey(d => d.DPID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_M_F_SAMPLE_DESPATCH_MASTER");

                entity.HasOne(d => d.DR)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_M)
                    .HasForeignKey(d => d.DRID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_M_F_BAS_DRIVERINFO");

                entity.HasOne(d => d.V)
                    .WithMany(p => p.H_SAMPLE_RECEIVING_M)
                    .HasForeignKey(d => d.VID)
                    .HasConstraintName("FK_H_SAMPLE_RECEIVING_M_F_BAS_VEHICLE_INFO");
            });

            modelBuilder.Entity<H_SAMPLE_DESPATCH_D>(entity =>
            {
                entity.HasKey(e => e.SDDID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CS_PRICE).HasMaxLength(50);

                entity.Property(e => e.NEGO_PRICE).HasMaxLength(50);

                entity.Property(e => e.QTY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SD)
                    .WithMany(p => p.H_SAMPLE_DESPATCH_D)
                    .HasForeignKey(d => d.SDID)
                    .HasConstraintName("FK_H_SAMPLE_DESPATCH_D_H_SAMPLE_DESPATCH_M");
            });

            modelBuilder.Entity<H_SAMPLE_DESPATCH_M>(entity =>
            {
                entity.HasKey(e => e.SDID);

                entity.Property(e => e.COST_STATUS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.RTNDATE).HasColumnType("datetime");

                entity.Property(e => e.SDDATE).HasColumnType("datetime");

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.THROUGH).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BRAND)
                    .WithMany(p => p.H_SAMPLE_DESPATCH_M)
                    .HasForeignKey(d => d.BRANDID)
                    .HasConstraintName("FK_H_SAMPLE_DESPATCH_M_BAS_BRANDINFO");

                entity.HasOne(d => d.PURPOSENavigation)
                    .WithMany(p => p.H_SAMPLE_DESPATCH_M)
                    .HasForeignKey(d => d.PURPOSE)
                    .HasConstraintName("FK_H_SAMPLE_DESPATCH_M_F_BAS_UNITS");

                entity.HasOne(d => d.HSP)
                    .WithMany(p => p.H_SAMPLE_DESPATCH_M)
                    .HasForeignKey(d => d.HSPID)
                    .HasConstraintName("FK_H_SAMPLE_DESPATCH_M_H_SAMPLE_PARTY");

                entity.HasOne(d => d.HST)
                    .WithMany(p => p.H_SAMPLE_DESPATCH_M)
                    .HasForeignKey(d => d.HSTID)
                    .HasConstraintName("FK_H_SAMPLE_DESPATCH_M_H_SAMPLE_TEAM_DETAILS");
            });

            modelBuilder.Entity<H_SAMPLE_PARTY>(entity =>
            {
                entity.HasKey(e => e.HSPID);

                entity.Property(e => e.ADDRESS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.EMAIL).HasMaxLength(50);

                entity.Property(e => e.HSPDISIGNATION).HasMaxLength(50);

                entity.Property(e => e.HSPNAME).HasMaxLength(50);

                entity.Property(e => e.PHONE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<H_SAMPLE_TEAM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.HSTID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TEAMMEMBER).HasMaxLength(50);

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.TEAM)
                    .WithMany(p => p.H_SAMPLE_TEAM_DETAILS)
                    .HasForeignKey(d => d.TEAMID)
                    .HasConstraintName("FK_H_SAMPLE_TEAM_DETAILS_BAS_TEAMINFO");
            });

            modelBuilder.Entity<F_FS_DELIVERYCHALLAN_PACK_DETAILS>(entity =>
            {
                entity.HasKey(e => e.D_CHALLAN_D_ID);

                entity.Property(e => e.D_CHALLAN_D_ID).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHADE_GROUP).HasMaxLength(50);

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.D_CHALLAN)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .HasForeignKey(d => d.D_CHALLANID)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_DETAILS_F_FS_DELIVERYCHALLAN_PACK_MASTER");

                entity.HasOne(d => d.ROLL)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .HasForeignKey(d => d.ROLL_NO)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_DETAILS_F_FS_FABRIC_RCV_DETAILS");
            });

            modelBuilder.Entity<F_FS_DELIVERYCHALLAN_PACK_MASTER>(entity =>
            {
                entity.HasKey(e => e.D_CHALLANID);

                entity.Property(e => e.AUDIT_COMMENTS).HasMaxLength(150);

                entity.Property(e => e.AUDIT_ON).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DC_NO).HasMaxLength(50);

                entity.Property(e => e.GP_NO).HasMaxLength(50);

                entity.Property(e => e.DRIVER).HasMaxLength(50);

                entity.Property(e => e.D_CHALLANDATE).HasColumnType("datetime");

                entity.Property(e => e.LOCKNO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.AUDITBYNavigation)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.AUDITBY)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.DELIVERY_TYPENavigation)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.DELIVERY_TYPE)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_F_BAS_DELIVERY_TYPE");

                entity.HasOne(d => d.DO)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.DOID)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_ACC_EXPORT_DOMASTER");

                entity.HasOne(d => d.PI)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.PIID)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_COM_EX_PIMASTER");

                entity.HasOne(d => d.SO_NONavigation)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.SO_NO)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_COM_EX_PI_DETAILS");

                entity.HasOne(d => d.VEHICLENONavigation)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.VEHICLENO)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_F_BAS_VEHICLE_INFO");

                entity.HasOne(d => d.ISSUE_TYPENavigation)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.ISSUE_TYPE)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_F_FS_ISSUE_TYPE");

                entity.HasOne(d => d.DO_LOCAL)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.DOID_LOCAL)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_ACC_LOCAL_DOMASTER");

                entity.HasOne(d => d.SC)
                    .WithMany(p => p.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .HasForeignKey(d => d.SCID)
                    .HasConstraintName("FK_F_FS_DELIVERYCHALLAN_PACK_MASTER_COM_EX_SCINFO");
            });


            modelBuilder.Entity<F_BAS_DELIVERY_TYPE>(entity =>
            {
                entity.Property(e => e.DEL_TYPE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<BAS_YARN_PARTNO>(entity =>
            {
                entity.HasKey(e => e.PART_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_PR_FINIGHING_DOFF_FOR_MACHINE>(entity =>
            {
                entity.HasKey(e => e.DID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FPM)
                    .WithMany(p => p.F_PR_FINIGHING_DOFF_FOR_MACHINE)
                    .HasForeignKey(d => d.FPMID)
                    .HasConstraintName("FK_F_PR_FINIGHING_DOFF_FOR_MACHINE_F_PR_FINISHING_MACHINE_PREPARATION");

                entity.HasOne(d => d.DOFF)
                    .WithMany(p => p.F_PR_FINIGHING_DOFF_FOR_MACHINE)
                    .HasForeignKey(d => d.FN_PROCESSID)
                    .HasConstraintName("FK_F_PR_FINIGHING_DOFF_FOR_MACHINE_F_PR_FINISHING_PROCESS_MASTER");
            });

            modelBuilder.Entity<F_PR_FINISHING_MACHINE_PREPARATION>(entity =>
            {
                entity.HasKey(e => e.FPMID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TRNSDATE)
                    .HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FINISH_ROUTE).HasMaxLength(100);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_PR_FINISHING_MACHINE_PREPARATION)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_PR_FINISHING_MACHINE_PREPARATION_RND_FABRICINFO");

                entity.HasOne(d => d.MACHINE_NONavigation)
                    .WithMany(p => p.F_PR_FINISHING_MACHINE_PREPARATION)
                    .HasForeignKey(d => d.MACHINE_NO)
                    .HasConstraintName("FK_F_PR_FINISHING_MACHINE_PREPARATION_F_PR_FN_MACHINE_INFO");

                entity.HasOne(d => d.ProcessType)
                    .WithMany(p => p.F_PR_FINISHING_MACHINE_PREPARATION)
                    .HasForeignKey(d => d.FIN_PRO_TYPEID)
                    .HasConstraintName("FK_F_PR_FINISHING_MACHINE_PREPARATION_F_PR_FN_PROCESS_TYPEINFO");
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasKey(e => new { e.MenuIdentity, e.MenuID, e.MenuName });

                entity.Property(e => e.MenuIdentity).ValueGeneratedOnAdd();

                entity.Property(e => e.MenuID)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuFileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuURL)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuIcon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parent_MenuID)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.USE_YN).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<MenuMasterRoles>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<COMPANY_INFO>(entity =>
            {
                entity.Property(e => e.BIN_NO)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.COMPANY_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION).IsUnicode(false);

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ETIN_NO)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FACTORY_ADDRESS).IsUnicode(false);

                entity.Property(e => e.HEADOFFICE_ADDRESS).IsUnicode(false);

                entity.Property(e => e.PHONE1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE3)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TAGLINE).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WEB_ADDRESS).IsUnicode(false);
            });

            modelBuilder.Entity<F_GATEPASS_TYPE>(entity =>
            {
                entity.HasKey(e => e.GPTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_GS_GATEPASS_INFORMATION_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ETR).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GP)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_D)
                    .HasForeignKey(d => d.GPID)
                    .HasConstraintName("FK_F_GS_GATEPASS_INFORMATION_D_F_GS_GATEPASS_INFORMATION_M");

                entity.HasOne(d => d.PROD)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_D)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_F_GS_GATEPASS_INFORMATION_D_F_GS_PRODUCT_INFORMATION");
            });

            modelBuilder.Entity<F_GS_GATEPASS_INFORMATION_M>(entity =>
            {
                entity.HasKey(e => e.GPID);

                entity.Property(e => e.ADDRESS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.GPNO).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SENDTO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_MEMP)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_GS_GATEPASS_INFORMATION_M_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.EMP_REQBYNavigation)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_MREQ_BYNavigation)
                    .HasForeignKey(d => d.REQ_BY);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_M)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_GS_GATEPASS_INFORMATION_M_F_BAS_SECTION");

                entity.HasOne(d => d.V)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_M)
                    .HasForeignKey(d => d.VID)
                    .HasConstraintName("FK_F_GS_GATEPASS_INFORMATION_M_F_BAS_VEHICLE_INFO");

                entity.HasOne(d => d.GPT)
                    .WithMany(p => p.F_GS_GATEPASS_INFORMATION_M)
                    .HasForeignKey(d => d.GPTID)
                    .HasConstraintName("FK_F_GS_GATEPASS_INFORMATION_M_F_GATEPASS_TYPE");
            });

            modelBuilder.Entity<F_GS_RETURNABLE_GP_RCV_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GATE_ENTRYNO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.PROD)
                    .WithMany(p => p.F_GS_RETURNABLE_GP_RCV_D)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_D_F_GS_PRODUCT_INFORMATION");

                entity.HasOne(d => d.RCV)
                    .WithMany(p => p.F_GS_RETURNABLE_GP_RCV_D)
                    .HasForeignKey(d => d.RCVID)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_D_F_GS_RETURNABLE_GP_RCV_M");
            });

            modelBuilder.Entity<F_GS_RETURNABLE_GP_RCV_M>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GP)
                    .WithMany(p => p.F_GS_RETURNABLE_GP_RCV_M)
                    .HasForeignKey(d => d.GPID)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_M_F_GS_GATEPASS_INFORMATION_M");

                entity.HasOne(d => d.RCVD_BYNavigation)
                    .WithMany(p => p.F_GS_RETURNABLE_GP_RCV_M)
                    .HasForeignKey(d => d.RCVD_BY)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_M_F_HRD_EMPLOYEE");
            });




            modelBuilder.Entity<LOOM_SETTING_CHANNEL_INFO>(entity =>
            {
                entity.HasKey(e => e.CHANNEL_ID);

                entity.Property(e => e.BREAK_FORCE).HasMaxLength(50);

                entity.Property(e => e.BREAK_TIME).HasMaxLength(50);

                entity.Property(e => e.CHANNEL_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FIXED_ELCA_START).HasMaxLength(50);

                entity.Property(e => e.FIXED_ELCA_STOP).HasMaxLength(50);

                entity.Property(e => e.MAIN_NOZZLE).HasMaxLength(50);

                entity.Property(e => e.MAIN_VALVE).HasMaxLength(50);

                entity.Property(e => e.MODE).HasMaxLength(50);

                entity.Property(e => e.MOV_ELCA_START).HasMaxLength(50);

                entity.Property(e => e.MOV_ELCA_STOP).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PAEXP).HasMaxLength(50);

                entity.Property(e => e.PS).HasMaxLength(50);

                entity.Property(e => e.RATIO).HasMaxLength(50);

                entity.Property(e => e.RELAY_NOZZLE_LR).HasMaxLength(50);

                entity.Property(e => e.RELAY_NOZZLE_M).HasMaxLength(50);

                entity.Property(e => e.RELAY_VALVE).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEFT_CUTTER_ANGLE).HasMaxLength(50);

                entity.HasOne(d => d.COUNTNavigation)
                    .WithMany(p => p.LOOM_SETTING_CHANNEL_INFO)
                    .HasForeignKey(d => d.COUNT)
                    .HasConstraintName("FK_LOOM_SETTING_CHANNEL_INFO_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.LOTNavigation)
                    .WithMany(p => p.LOOM_SETTING_CHANNEL_INFO)
                    .HasForeignKey(d => d.LOT)
                    .HasConstraintName("FK_LOOM_SETTING_CHANNEL_INFO_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SETTING_)
                    .WithMany(p => p.LOOM_SETTING_CHANNEL_INFO)
                    .HasForeignKey(d => d.SETTING_ID)
                    .HasConstraintName("FK_LOOM_SETTING_CHANNEL_INFO_LOOM_SETTING_STYLE_WISE_M");

                entity.HasOne(d => d.SUPPLIERNavigation)
                    .WithMany(p => p.LOOM_SETTING_CHANNEL_INFO)
                    .HasForeignKey(d => d.SUPPLIER)
                    .HasConstraintName("FK_LOOM_SETTING_CHANNEL_INFO_BAS_SUPPLIERINFO");
            });

            modelBuilder.Entity<LOOM_SETTING_STYLE_WISE_M>(entity =>
            {
                entity.HasKey(e => e.SETTING_ID);

                entity.Property(e => e.AMP_FILLING).HasMaxLength(50);

                entity.Property(e => e.AMP_OTHER).HasMaxLength(50);

                entity.Property(e => e.ASO_FILLING).HasMaxLength(50);

                entity.Property(e => e.ASO_OTHER).HasMaxLength(50);

                entity.Property(e => e.ASP_FILLING).HasMaxLength(50);

                entity.Property(e => e.ASP_OTHER).HasMaxLength(50);

                entity.Property(e => e.BACKREST_DEPTH).HasMaxLength(50);

                entity.Property(e => e.BACKREST_HEIGHT).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DBOX_DEPTH).HasMaxLength(50);

                entity.Property(e => e.DBOX_HEIGHT).HasMaxLength(50);

                entity.Property(e => e.FRAME_HEIGHT).HasMaxLength(50);

                entity.Property(e => e.MODE_FILLING).HasMaxLength(50);

                entity.Property(e => e.MODE_FILLING_START).HasMaxLength(50);

                entity.Property(e => e.MODE_OTHER).HasMaxLength(50);

                entity.Property(e => e.MODE_OTHER_START).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.PFR_FILLING).HasMaxLength(50);

                entity.Property(e => e.SECOND_ROLL_CLAMP_USE).HasMaxLength(5);

                entity.Property(e => e.SECOND_ROLL_TYPE).HasMaxLength(10);

                entity.Property(e => e.SHED_ANGLE).HasMaxLength(50);

                entity.Property(e => e.SHED_CROSSING).HasMaxLength(50);

                entity.Property(e => e.STOP_FILLING).HasMaxLength(50);

                entity.Property(e => e.STOP_OTHER).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARP_TENSION).HasMaxLength(50);

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.LOOM_SETTING_STYLE_WISE_M)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_LOOM_SETTING_STYLE_WISE_M_RND_FABRICINFO");

                entity.HasOne(d => d.FILTER_VALUENavigation)
                    .WithMany(p => p.LOOM_SETTING_STYLE_WISE_M)
                    .HasForeignKey(d => d.FILTER_VALUE)
                    .HasConstraintName("FK_LOOM_SETTING_STYLE_WISE_M_LOOM_SETTINGS_FILTER_VALUE");

                entity.HasOne(d => d.LOOM_TYPENavigation)
                    .WithMany(p => p.LOOM_SETTING_STYLE_WISE_M)
                    .HasForeignKey(d => d.LOOM_TYPE)
                    .HasConstraintName("FK_LOOM_SETTING_STYLE_WISE_M_LOOM_TYPE");
            });

            modelBuilder.Entity<LOOM_SETTINGS_FILTER_VALUE>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_SAMPLE_DESPATCH_MASTER_TYPE>(entity =>
            {
                entity.HasKey(e => e.TYPEID);
            });

            modelBuilder.Entity<AspNetUserTypes>(entity =>
            {
                entity.HasKey(e => e.TYPEID);
            });


            modelBuilder.Entity<F_QA_YARN_TEST_INFORMATION_COTTON>(entity =>
            {
                entity.HasKey(e => e.TESTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("date");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TESTDATE).HasColumnType("date");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_QA_YARN_TEST_INFORMATION_COTTON)
                    .HasForeignKey(d => d.YRCVID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_QA_YARN_TEST_INFORMATION_F_YS_YARN_RECEIVE_MASTER");

                entity.HasOne(d => d.YRDT)
                    .WithMany(p => p.F_QA_YARN_TEST_INFORMATION_COTTON)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_QA_YARN_TEST_INFORMATION_F_YS_YARN_RECEIVE_DETAILS");
            });

            modelBuilder.Entity<F_QA_YARN_TEST_INFORMATION_POLYESTER>(entity =>
            {
                entity.HasKey(e => e.TESTID);

                entity.Property(e => e.TESTID).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_AT).HasColumnType("date");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.NIP_RETENTION_STRENGTH).HasMaxLength(50);

                entity.Property(e => e.LIMHIM).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TESTDATE).HasColumnType("date");

                entity.Property(e => e.UPDATED_AT).HasColumnType("date");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COLOR)
                    .WithMany(p => p.F_QA_YARN_TEST_INFORMATION_POLYESTER)
                    .HasForeignKey(d => d.COLORCODE)
                    .HasConstraintName("FK_F_QA_YARN_TEST_INFORMATION_POLYESTER_BAS_COLOR");

                entity.HasOne(d => d.YRDT)
                    .WithMany(p => p.F_QA_YARN_TEST_INFORMATION_POLYESTERS)
                    .HasForeignKey(d => d.TESTID)
                    .HasConstraintName("FK_F_QA_YARN_TEST_INFORMATION_POLYESTER_F_YS_YARN_RECEIVE_DETAILS");

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_QA_YARN_TEST_INFORMATION_POLYESTER)
                    .HasForeignKey(d => d.YRCVID)
                    .HasConstraintName("FK_F_QA_YARN_TEST_INFORMATION_POLYESTER_F_YS_YARN_RECEIVE_MASTER");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_MASTER>(entity =>
            {
                entity.HasKey(e => e.YRCVID);

                entity.Property(e => e.CHALLANDATE).HasColumnType("datetime");

                entity.Property(e => e.CHALLANNO).HasMaxLength(50);

                entity.Property(e => e.COMMENTS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.G_ENTRY_DATE).HasColumnType("datetime");

                entity.Property(e => e.G_ENTRY_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RCVFROM).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TUCK_NO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YRCVDATE).HasColumnType("datetime");
            });

            modelBuilder.Entity<F_CHEM_STORE_INDENT_TYPE>(entity =>
            {
                entity.HasKey(e => e.INDENTID);
            });

            modelBuilder.Entity<COM_CONTAINER>(entity =>
            {
                entity.HasKey(e => e.CONTAINERID);
            });

            modelBuilder.Entity<COS_WASTAGE_PERCENTAGE>(entity =>
            {
                entity.HasKey(e => e.WESTAGE_ID);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(150);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_FABTEST_BULK>(entity =>
            {
                entity.HasKey(e => e.LTBID);

                entity.Property(e => e.ACUW_EPI).HasMaxLength(50);

                entity.Property(e => e.ACUW_PPI).HasMaxLength(50);

                entity.Property(e => e.ACWASH_EPI).HasMaxLength(50);

                entity.Property(e => e.ACWASH_PPI).HasMaxLength(50);

                entity.Property(e => e.BOWING).HasMaxLength(50);

                entity.Property(e => e.CCA).HasMaxLength(50);

                entity.Property(e => e.CCALK).HasMaxLength(50);

                entity.Property(e => e.CLR_CHANGE).HasMaxLength(50);

                entity.Property(e => e.CLR_DRY).HasMaxLength(50);

                entity.Property(e => e.CLR_STAINING).HasMaxLength(50);

                entity.Property(e => e.CLR_WET).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CSA).HasMaxLength(50);

                entity.Property(e => e.CSALK).HasMaxLength(50);

                entity.Property(e => e.FAB_COMP).HasMaxLength(50);

                entity.Property(e => e.FORMALD).HasMaxLength(50);

                entity.Property(e => e.GROWTH_WARP).HasMaxLength(50);

                entity.Property(e => e.GROWTH_WEFT).HasMaxLength(50);

                entity.Property(e => e.ISSUE).HasMaxLength(50);

                entity.Property(e => e.LAB_NO).HasMaxLength(50);

                entity.Property(e => e.LTB_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.REC_WARP).HasMaxLength(50);

                entity.Property(e => e.REC_WEFT).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);

                entity.Property(e => e.SHRINK_WARP).HasMaxLength(50);

                entity.Property(e => e.SHRINK_WEFT).HasMaxLength(50);

                entity.Property(e => e.SKEW_MOVE).HasMaxLength(50);

                entity.Property(e => e.SKEW_UW).HasMaxLength(50);

                entity.Property(e => e.SKEW_WASH).HasMaxLength(50);

                entity.Property(e => e.SLIP_WARP).HasMaxLength(50);

                entity.Property(e => e.SLIP_WEFT).HasMaxLength(50);

                entity.Property(e => e.SPIR_A).HasMaxLength(50);

                entity.Property(e => e.SPIR_B).HasMaxLength(50);

                entity.Property(e => e.STIFFNESS).HasMaxLength(50);

                entity.Property(e => e.STRWARP_QA).HasMaxLength(50);

                entity.Property(e => e.STRWEFT_QA).HasMaxLength(50);

                entity.Property(e => e.TEAR_WARP).HasMaxLength(50);

                entity.Property(e => e.TEAR_WEFT).HasMaxLength(50);

                entity.Property(e => e.TENSILE_WARPT).HasMaxLength(50);

                entity.Property(e => e.TENSILE_WEFTT).HasMaxLength(50);

                entity.Property(e => e.TEST_METHOD).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEIGHT_DEAD).HasMaxLength(50);

                entity.Property(e => e.WEIGHT_UW).HasMaxLength(50);

                entity.Property(e => e.WEIGHT_WASH).HasMaxLength(50);

                entity.Property(e => e.WIDTH_CUT).HasMaxLength(50);

                entity.Property(e => e.WIDTH_OVR).HasMaxLength(50);

                entity.Property(e => e.WIDTH_WASH).HasMaxLength(50);

                entity.HasOne(d => d.PROG)
                    .WithMany(p => p.RND_FABTEST_BULK)
                    .HasForeignKey(d => d.PROG_NO)
                    .HasConstraintName("FK_RND_FABTEST_BULK_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.FINPROC)
                    .WithMany(p => p.RND_FABTEST_BULK)
                    .HasForeignKey(d => d.TROLLEY_NO)
                    .HasConstraintName("FK_RND_FABTEST_BULK_F_PR_FINISHING_FNPROCESS");

                entity.HasOne(d => d.EMP_WASHEDBY)
                    .WithMany(p => p.RND_FABTEST_BULK_WASHEDBY)
                    .HasForeignKey(d => d.WASHED_BY)
                    .HasConstraintName("FK_RND_FABTEST_BULK_F_HRD_EMPLOYEE_WB");

                entity.HasOne(d => d.EMP_UNWASHEDBY)
                    .WithMany(p => p.RND_FABTEST_BULK_UNWASHEDBY)
                    .HasForeignKey(d => d.UNWASHED_BY)
                    .HasConstraintName("FK_RND_FABTEST_BULK_F_HRD_EMPLOYEE_UB");

                entity.HasOne(d => d.SHIFTINFO)
                    .WithMany(p => p.RND_FABTEST_BULK)
                    .HasForeignKey(d => d.SHIFT)
                    .HasConstraintName("FK_RND_FABTEST_BULK_F_HR_SHIFT_INFO");

                entity.HasOne(d => d.TEST)
                    .WithMany(p => p.RND_FABTEST_BULK)
                    .HasForeignKey(d => d.TEST_METHOD)
                    .HasConstraintName("FK_RND_FABTEST_BULK_F_BAS_TESTMETHOD");
            });

            modelBuilder.Entity<F_BAS_TESTMETHOD>(entity =>
            {
                entity.HasKey(e => e.TMID);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TMNAME).HasMaxLength(50);
            });
            modelBuilder.Entity<F_HR_SHIFT_INFO>(entity =>
            {
                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFT).HasMaxLength(50);
            });

            modelBuilder.Entity<RND_FABTEST_SAMPLE_BULK>(entity =>
            {
                entity.HasKey(e => e.LTSGID);

                entity.Property(e => e.CFATDRY).HasMaxLength(50);

                entity.Property(e => e.CFATNET).HasMaxLength(50);

                entity.Property(e => e.COLORCNG_ACID).HasMaxLength(50);

                entity.Property(e => e.COLORCNG_ALKA).HasMaxLength(50);

                entity.Property(e => e.COLORSTN).HasMaxLength(50);

                entity.Property(e => e.COLORSTN_ACID).HasMaxLength(50);

                entity.Property(e => e.COLORSTN_ALKA).HasMaxLength(50);

                entity.Property(e => e.COMMENTS).HasMaxLength(100);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FABCOMP).HasMaxLength(100);

                entity.Property(e => e.FNEPI).HasMaxLength(50);

                entity.Property(e => e.FNPPI).HasMaxLength(50);

                entity.Property(e => e.FORMALDEHYDE).HasMaxLength(50);

                entity.Property(e => e.GRFNWARP).HasMaxLength(50);

                entity.Property(e => e.GRFNWEFT).HasMaxLength(50);

                entity.Property(e => e.ISSUE).HasMaxLength(50);

                entity.Property(e => e.LABNO).HasMaxLength(50);

                entity.Property(e => e.LTSGDATE).HasColumnType("datetime");

                entity.Property(e => e.MCNAME).HasMaxLength(50);

                entity.Property(e => e.METHOD).HasMaxLength(50);

                entity.Property(e => e.OPTION1).HasMaxLength(50);

                entity.Property(e => e.OPTION2).HasMaxLength(50);

                entity.Property(e => e.OPTION3).HasMaxLength(50);

                entity.Property(e => e.OPTION4).HasMaxLength(50);

                entity.Property(e => e.OPTION5).HasMaxLength(50);

                entity.Property(e => e.PILLGRADE).HasMaxLength(50);

                entity.Property(e => e.PILLRUBS).HasMaxLength(50);

                entity.Property(e => e.PROCESSROUTE).HasMaxLength(100);

                entity.Property(e => e.PROGNO).HasMaxLength(50);

                entity.Property(e => e.RECWARP).HasMaxLength(50);

                entity.Property(e => e.RECWEFT).HasMaxLength(50);

                entity.Property(e => e.SHADECNG).HasMaxLength(50);

                entity.Property(e => e.SKEWMOVE).HasMaxLength(50);

                entity.Property(e => e.SLPWARP).HasMaxLength(50);

                entity.Property(e => e.SLPWEFT).HasMaxLength(50);

                entity.Property(e => e.SPIRALIRTY_B).HasMaxLength(50);

                entity.Property(e => e.SPIRALITY_A).HasMaxLength(50);

                entity.Property(e => e.SRFNWARP).HasMaxLength(50);

                entity.Property(e => e.SRFNWEFT).HasMaxLength(50);

                entity.Property(e => e.STFNWARP).HasMaxLength(50);

                entity.Property(e => e.STFNWEFT).HasMaxLength(50);

                entity.Property(e => e.TNWARP).HasMaxLength(50);

                entity.Property(e => e.TNWEFT).HasMaxLength(50);

                entity.Property(e => e.TRWARP).HasMaxLength(50);

                entity.Property(e => e.TRWEFT).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WASHEPI).HasMaxLength(50);

                entity.Property(e => e.WASHPPI).HasMaxLength(50);

                entity.Property(e => e.WGDEAD).HasMaxLength(50);

                entity.Property(e => e.WGFNAW).HasMaxLength(50);

                entity.Property(e => e.WGFNBW).HasMaxLength(50);

                entity.Property(e => e.WIFNAW).HasMaxLength(50);

                entity.Property(e => e.WIFNBW).HasMaxLength(50);

                entity.Property(e => e.WIFNCUT).HasMaxLength(50);

                entity.HasOne(d => d.TESTINFO)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_BULK)
                    .HasForeignKey(d => d.TESTMETHOD)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_BULK_F_BAS_TESTMETHOD");
                entity.HasOne(d => d.EMPWASHBY)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_BULK_WASHEDBY)
                    .HasForeignKey(d => d.WASHEDBY)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_BULK_F_HRD_EMPLOYEEW");
                entity.HasOne(d => d.EMPUNWASHBY)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_BULK_UNWASHEDBY)
                    .HasForeignKey(d => d.UNWASHEDBY)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_BULK_F_HRD_EMPLOYEEUW");
                entity.HasOne(d => d.LOOMINFO)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_BULK)
                    .HasForeignKey(d => d.LOOM)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_BULK_F_LOOM_MACHINE_NO");
                entity.HasOne(d => d.SHIFTINFO)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_BULK)
                    .HasForeignKey(d => d.SHIFT)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_BULK_F_HR_SHIFT_INFO");
                entity.HasOne(d => d.SFIN)
                    .WithMany(p => p.RND_FABTEST_SAMPLE_BULK)
                    .HasForeignKey(d => d.SFINID)
                    .HasConstraintName("FK_RND_FABTEST_SAMPLE_BULK_RND_SAMPLEINFO_FINISHING");
            });

            modelBuilder.Entity<EXPORTSTATUS>(entity =>
            {
                entity.HasKey(e => e.EXTYPEID);

                entity.Property(e => e.EXPPORTTYPE).IsRequired();
            });

            modelBuilder.Entity<CURRENCY>(entity =>
            {
                entity.Property(e => e.CODE).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.SYMBOL).HasMaxLength(50);
            });
            //Acc_Loan_Management_M
            modelBuilder.Entity<ACC_LOAN_MANAGEMENT_M>(entity =>
          {
              entity.HasKey(e => e.LOANID);

              entity.Property(e => e.CREATED_AT)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

              entity.Property(e => e.CREATED_BY).HasMaxLength(50);

              entity.Property(e => e.EXP_DATE).HasColumnType("datetime");

              entity.Property(e => e.LOANDATE).HasColumnType("datetime");

              entity.Property(e => e.OPT2).HasMaxLength(50);

              entity.Property(e => e.OPT3).HasMaxLength(50);

              entity.Property(e => e.OPT4).HasMaxLength(50);

              entity.Property(e => e.OPT5).HasMaxLength(50);

              entity.Property(e => e.OPT6).HasMaxLength(50);

              entity.Property(e => e.PAID_DATE).HasColumnType("datetime");

              entity.Property(e => e.REMARKS).HasMaxLength(50);

              entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

              entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

              entity.HasOne(d => d.LC)
                  .WithMany(p => p.ACC_LOAN_MANAGEMENT_M)
                  .HasForeignKey(d => d.LCID)
                  .HasConstraintName("FK_ACC_LOAN_MANAGEMENT_M_COM_IMP_LCINFORMATION");
          });

            //F_QA_FIRST_MTR_ANALYSIS_M & D
            modelBuilder.Entity<F_QA_FIRST_MTR_ANALYSIS_D>(entity =>
            {
                entity.HasKey(e => e.FM_D_ID);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.HasOne(d => d.FM)
                    .WithMany(p => p.F_QA_FIRST_MTR_ANALYSIS_D)
                    .HasForeignKey(d => d.FMID)
                    .HasConstraintName("FK_F_QA_FIRST_MTR_ANALYSIS_D_F_QA_FIRST_MTR_ANALYSIS_M");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_QA_FIRST_MTR_ANALYSIS_D)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_QA_FIRST_MTR_ANALYSIS_D_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.SUPPLIER)
                    .WithMany(p => p.F_QA_FIRST_MTR_ANALYSIS_D)
                    .HasForeignKey(d => d.SUPPLIERID)
                    .HasConstraintName("FK_F_QA_FIRST_MTR_ANALYSIS_D_BAS_SUPPLIERINFO");
            });

            modelBuilder.Entity<F_QA_FIRST_MTR_ANALYSIS_M>(entity =>
            {
                entity.HasKey(e => e.FMID);

                entity.Property(e => e.BYPASS_YARN).HasMaxLength(50);

                entity.Property(e => e.ACT_RATIO).HasMaxLength(50);

                entity.Property(e => e.RPTNO).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.TRANS_DATE).HasColumnType("datetime");

                entity.HasOne(d => d.BEAM)
                    .WithMany(p => p.F_QA_FIRST_MTR_ANALYSIS_M)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_F_QA_FIRST_MTR_ANALYSIS_M_F_PR_WEAVING_PROCESS_BEAM_DETAILS_B");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_QA_FIRST_MTR_ANALYSIS_M)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_QA_FIRST_MTR_ANALYSIS_M_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_QA_FIRST_MTR_ANALYSIS_M)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_QA_FIRST_MTR_ANALYSIS_M_PL_PRODUCTION_SETDISTRIBUTION");
            });


            //SW MASTER, DETAILS, CONSUM_DETAILS
            modelBuilder.Entity<F_PR_WARPING_PROCESS_SW_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SW_D_ID);

                entity.Property(e => e.BALL_LENGTH).HasMaxLength(50);

                entity.Property(e => e.BREAKS_ENDS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ENDS_ROPE).HasMaxLength(50);

                entity.Property(e => e.LEADLINE).HasMaxLength(50);

                entity.Property(e => e.LINK_BALL_LENGTH).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFTNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BALL_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_DETAILSBALL_NONavigation)
                    .HasForeignKey(d => d.BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_DETAILS_F_BAS_BALL_INFO");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.LINK_BALL_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_DETAILSLINK_BALL_NONavigation)
                    .HasForeignKey(d => d.LINK_BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_DETAILS_F_BAS_BALL_INFO1");

                entity.HasOne(d => d.MACHINE_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_DETAILS)
                    .HasForeignKey(d => d.MACHINE_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_DETAILS_F_PR_WARPING_MACHINE");

                entity.HasOne(d => d.SW_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_DETAILS)
                    .HasForeignKey(d => d.SW_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_DETAILS_F_PR_WARPING_PROCESS_SW_MASTER");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_SW_MASTER>(entity =>
            {
                entity.HasKey(e => e.SWID);

                entity.Property(e => e.BALL_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEL_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PRODDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TIME_END).HasColumnType("datetime");

                entity.Property(e => e.TIME_START).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARPLENGTH).HasMaxLength(50);

                entity.Property(e => e.WARPRATIO).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_MASTER_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CONSM_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.COUNT_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.SW_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.SW_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS_F_PR_WARPING_PROCESS_SW_MASTER");
            });

            //Acc_Loan_Management_D
            modelBuilder.Entity<ACC_LOAN_MANAGEMENT_D>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.INV)
                    .WithMany(p => p.ACC_LOAN_MANAGEMENT_D)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_ACC_LOAN_MANAGEMENT_D_COM_IMP_INVOICEINFO");

                entity.HasOne(d => d.LOAN)
                    .WithMany(p => p.ACC_LOAN_MANAGEMENT_D)
                    .HasForeignKey(d => d.LOANID)
                    .HasConstraintName("FK_ACC_LOAN_MANAGEMENT_D_ACC_LOAN_MANAGEMENT_M");
            });

            //General Store New

            modelBuilder.Entity<F_GEN_S_INDENT_TYPE>(entity =>
            {
                entity.HasKey(e => e.INDENTID);
            });

            modelBuilder.Entity<F_GEN_S_INDENTDETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.ADD_QTY).HasMaxLength(50);

                entity.Property(e => e.BAL_QTY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.FULL_QTY).HasMaxLength(50);

                entity.Property(e => e.LOCATION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.VALIDITY).HasColumnType("datetime");

                entity.HasOne(d => d.GIND)
                    .WithMany(p => p.F_GEN_S_INDENTDETAILS)
                    .HasForeignKey(d => d.GINDID)
                    .HasConstraintName("FK_F_GEN_S_INDENTDETAILS_F_GEN_S_INDENTMASTER");

                entity.HasOne(d => d.INDSL)
                    .WithMany(p => p.F_GEN_S_INDENTDETAILS)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_GEN_S_INDENTDETAILS_F_GEN_S_PURCHASE_REQUISITION_MASTER");

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_GEN_S_INDENTDETAILS)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_GEN_S_INDENTDETAILS_F_GS_PRODUCT_INFORMATION");
            });

            modelBuilder.Entity<F_GEN_S_INDENTMASTER>(entity =>
            {
                entity.HasKey(e => e.GINDID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GINDDATE).HasColumnType("datetime");

                entity.Property(e => e.GINDNO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.INDSL)
                    .WithMany(p => p.F_GEN_S_INDENTMASTER)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_GEN_S_INDENTMASTER_F_GEN_S_PURCHASE_REQUISITION_MASTER");

                entity.HasOne(d => d.INDTYPENavigation)
                    .WithMany(p => p.F_GEN_S_INDENTMASTER)
                    .HasForeignKey(d => d.INDTYPE)
                    .HasConstraintName("FK_F_GEN_S_INDENTMASTER_F_GEN_S_INDENT_TYPE");
            });

            modelBuilder.Entity<F_GEN_S_ISSUE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.GISSDID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GISSDDATE).HasColumnType("datetime");

                entity.Property(e => e.OTP1).HasMaxLength(50);

                entity.Property(e => e.OTP2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GRCVIDDNavigation)
                    .WithMany(p => p.F_GEN_S_ISSUE_DETAILS)
                    .HasForeignKey(d => d.GRCVIDD)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_DETAILS_F_GEN_S_RECEIVE_DETAILS");

                entity.HasOne(d => d.GREQ_DET_)
                    .WithMany(p => p.F_GEN_S_ISSUE_DETAILS)
                    .HasForeignKey(d => d.GREQ_DET_ID)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_DETAILS_F_GEN_S_REQ_DETAILS");

                entity.HasOne(d => d.ADJ_PRO_AGNSTNavigation)
                    .WithMany(p => p.F_GEN_S_ISSUE_DETAILSADJ_PRO_AGNSTNavigation)
                    .HasForeignKey(d => d.ADJ_PRO_AGNST)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_DETAILS_F_GS_PRODUCT_INFORMATION1");

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_GEN_S_ISSUE_DETAILSPRODUCT)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_DETAILS_F_GS_PRODUCT_INFORMATION");

                entity.HasOne(d => d.GISSUE)
                    .WithMany(p => p.F_GEN_S_ISSUE_DETAILS)
                    .HasForeignKey(d => d.GISSUEID)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_DETAILS_F_GEN_S_ISSUE_MASTER");
            });

            modelBuilder.Entity<F_GEN_S_ISSUE_MASTER>(entity =>
            {
                entity.HasKey(e => e.GISSUEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GISSUEDATE).HasColumnType("datetime");

                entity.Property(e => e.ISSUETO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.PURPOSE).HasMaxLength(50);

                entity.Property(e => e.SRDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GSR)
                    .WithMany(p => p.F_GEN_S_ISSUE_MASTER)
                    .HasForeignKey(d => d.GSRID)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_MASTER_F_GEN_S_REQ_MASTER");

                entity.HasOne(d => d.ISSUE)
                    .WithMany(p => p.F_GEN_S_ISSUE_MASTER)
                    .HasForeignKey(d => d.ISSUEID)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_MASTER_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.ISSUEBYNavigation)
                    .WithMany(p => p.F_GEN_S_ISSUE_MASTERISSUEBYNavigation)
                    .HasForeignKey(d => d.ISSUEBY)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_MASTER_F_HRD_EMPLOYEE_ISSUE");

                entity.HasOne(d => d.RECEIVEBYNavigation)
                    .WithMany(p => p.F_GEN_S_ISSUE_MASTERRECEIVEBYNavigation)
                    .HasForeignKey(d => d.RECEIVEBY)
                    .HasConstraintName("FK_F_GEN_S_ISSUE_MASTER_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_GEN_S_PURCHASE_REQUISITION_MASTER>(entity =>
            {
                entity.HasKey(e => e.INDSLID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INDSLDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CN_PERSONNavigation)
                    .WithMany(p => p.F_GEN_S_PURCHASE_REQUISITION_MASTERCN_PERSONNavigation)
                    .HasForeignKey(d => d.CN_PERSON)
                    .HasConstraintName("FK_F_GEN_S_PURCHASE_REQUISITION_MASTER_F_HRD_EMPLOYEE_CN_PERSON");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_GEN_S_PURCHASE_REQUISITION_MASTEREMP)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_GEN_S_PURCHASE_REQUISITION_MASTER_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_GEN_S_QC_APPROVE>(entity =>
            {
                entity.HasKey(e => e.GSQCAID);

                entity.Property(e => e.APPROVED_BY).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GQCADATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_GEN_S_MRR>(entity =>
            {
                entity.HasKey(e => e.GSMRRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GSMRRDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_GEN_S_RECEIVE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.BATCHNO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CURRENCY).HasMaxLength(50);

                entity.Property(e => e.EXDATE).HasColumnType("datetime");

                entity.Property(e => e.MNGDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GIND)
                    .WithMany(p => p.F_GEN_S_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.GINDID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_DETAILS_F_GEN_S_INDENTMASTER");

                entity.HasOne(d => d.GRCV)
                    .WithMany(p => p.F_GEN_S_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.GRCVID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_DETAILS_F_GEN_S_RECEIVE_MASTER");

                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_GEN_S_RECEIVE_DETAILS)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_DETAILS_F_GS_PRODUCT_INFORMATION");
            });

            modelBuilder.Entity<F_GEN_S_RECEIVE_MASTER>(entity =>
            {
                entity.HasKey(e => e.GRCVID);

                entity.Property(e => e.CHALLAN_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHALLAN_NO).HasMaxLength(50);

                entity.Property(e => e.CNF_CHALLAN_DATE).HasColumnType("datetime");

                entity.Property(e => e.CNF_CHALLAN_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GEDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.VEHICAL_NO).HasMaxLength(50);

                entity.HasOne(d => d.GINV)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.GINVID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_F_GEN_S_INDENTMASTER");

                entity.HasOne(d => d.CHECKBYNavigation)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTERCHECKBYNavigation)
                    .HasForeignKey(d => d.CHECKBY)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_F_HRD_EMPLOYEE_CHECK");

                entity.HasOne(d => d.CNF)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.CNFID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_COM_IMP_CNFINFO");

                entity.HasOne(d => d.LCD)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.LC_ID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_COM_IMP_LCDETAILS");

                entity.HasOne(d => d.ORIGINNavigation)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.ORIGIN)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_COUNTRIES");

                entity.HasOne(d => d.RCVBYNavigation)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTERRCVBYNavigation)
                    .HasForeignKey(d => d.RCVBY)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_F_BAS_RECEIVE_TYPE");

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.TRANSP)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.TRANSPID)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_BAS_TRANSPORTINFO");

                entity.HasOne(d => d.MRRNavigation)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.MRR)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_F_GEN_S_MRR");

                entity.HasOne(d => d.QCPASSNavigation)
                    .WithMany(p => p.F_GEN_S_RECEIVE_MASTER)
                    .HasForeignKey(d => d.QCPASS)
                    .HasConstraintName("FK_F_GEN_S_RECEIVE_MASTER_F_GEN_S_QC_APPROVE");
            });

            modelBuilder.Entity<F_GEN_S_REQ_DETAILS>(entity =>
            {
                entity.HasKey(e => e.GRQID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.GSR)
                    .WithMany(p => p.F_GEN_S_REQ_DETAILS)
                    .HasForeignKey(d => d.GSRID)
                    .HasConstraintName("FK_F_GEN_S_REQ_DETAILS_F_GEN_S_REQ_MASTER");
                entity.HasOne(d => d.PRODUCT)
                    .WithMany(p => p.F_GEN_S_REQ_DETAILS)
                    .HasForeignKey(d => d.PRODUCTID)
                    .HasConstraintName("FK_F_GEN_S_REQ_DETAILS_F_GS_PRODUCT_INFORMATION");
            });

            modelBuilder.Entity<F_GEN_S_REQ_MASTER>(entity =>
            {
                entity.HasKey(e => e.GSRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GSRDATE).HasColumnType("datetime");

                entity.Property(e => e.GSRNO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_GEN_S_REQ_MASTER)
                    .HasForeignKey(d => d.REQUISITIONBY)
                    .HasConstraintName("FK_F_GEN_S_REQ_MASTER_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_GS_GATEPASS_RETURN_RCV_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GATE_ENTRYNO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.PROD)
                    .WithMany(p => p.F_GS_GATEPASS_RETURN_RCV_DETAILS)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_D_F_GS_PRODUCT_INFORMATION");

                entity.HasOne(d => d.RCV)
                    .WithMany(p => p.F_GS_GATEPASS_RETURN_RCV_DETAILS)
                    .HasForeignKey(d => d.RCVID)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_D_F_GS_RETURNABLE_GP_RCV_M");
            });

            modelBuilder.Entity<F_GS_GATEPASS_RETURN_RCV_MASTER>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.GP)
                    .WithMany(p => p.F_GS_GATEPASS_RETURN_RCV_MASTER)
                    .HasForeignKey(d => d.GPID)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_M_F_GS_GATEPASS_INFORMATION_M");

                entity.HasOne(d => d.RCVD_BYNavigation)
                    .WithMany(p => p.F_GS_GATEPASS_RETURN_RCV_MASTER)
                    .HasForeignKey(d => d.RCVD_BY)
                    .HasConstraintName("FK_F_GS_RETURNABLE_GP_RCV_M_F_HRD_EMPLOYEE");
            });

            //ECRU MASTER, DETAILS, CONSUM_DETAILS
            modelBuilder.Entity<F_PR_WARPING_PROCESS_ECRU_DETAILS>(entity =>
            {
                entity.HasKey(e => e.ECRU_D_ID);

                entity.Property(e => e.BALL_LENGTH).HasMaxLength(50);

                entity.Property(e => e.BREAKS_ENDS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ENDS_ROPE).HasMaxLength(50);

                entity.Property(e => e.LEADLINE).HasMaxLength(50);

                entity.Property(e => e.LINK_BALL_LENGTH).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SHIFTNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BALL_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_DETAILSBALL_NONavigation)
                    .HasForeignKey(d => d.BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_DETAILS_F_BAS_BALL_INFO");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.ECRU_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_DETAILS)
                    .HasForeignKey(d => d.ECRU_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_DETAILS_F_PR_WARPING_PROCESS_ECRU_MASTER");

                entity.HasOne(d => d.LINK_BALL_NONavigation)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_DETAILSLINK_BALL_NONavigation)
                    .HasForeignKey(d => d.LINK_BALL_NO)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_DETAILS_F_BAS_BALL_INFO_LINK_BALL");

                entity.HasOne(d => d.MACHINE_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_DETAILS)
                    .HasForeignKey(d => d.MACHINE_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_DETAILS_F_PR_WARPING_MACHINE");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_ECRU_MASTER>(entity =>
            {
                entity.HasKey(e => e.ECRUID);

                entity.Property(e => e.BALL_NO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEL_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PRODDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TIME_END).HasColumnType("datetime");

                entity.Property(e => e.TIME_START).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WARPRATIO).HasMaxLength(50);

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_MASTER)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_MASTER_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>(entity =>
            {
                entity.HasKey(e => e.CONSM_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.COUNT_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.ECRU_)
                    .WithMany(p => p.F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS)
                    .HasForeignKey(d => d.ECRU_ID)
                    .HasConstraintName("FK_F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS_F_PR_WARPING_PROCESS_ECRU_MASTER");
            });

            //F_FS_FABRIC_CLEARATION_2ND_BEAM
            modelBuilder.Entity<F_FS_FABRIC_CLEARENCE_2ND_BEAM>(entity =>
            {
                entity.HasKey(e => e.AID);

                entity.Property(e => e.ADATE).HasColumnType("datetime");

                entity.Property(e => e.APPEARANCE).HasMaxLength(50);

                entity.Property(e => e.BULK_BS).HasMaxLength(15);

                entity.Property(e => e.BULK_CUT).HasMaxLength(15);

                entity.Property(e => e.BULK_OV).HasMaxLength(15);

                entity.Property(e => e.BULK_SR_WARP).HasMaxLength(15);

                entity.Property(e => e.BULK_SR_WEFT).HasMaxLength(15);

                entity.Property(e => e.BULK_ST_WARP).HasMaxLength(15);

                entity.Property(e => e.BULK_ST_WEFT).HasMaxLength(15);

                entity.Property(e => e.BULK_U_EPI).HasMaxLength(15);

                entity.Property(e => e.BULK_U_PPI).HasMaxLength(15);

                entity.Property(e => e.BULK_WG_AW).HasMaxLength(15);

                entity.Property(e => e.BULK_WG_UW).HasMaxLength(15);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CSV).HasMaxLength(15);

                entity.Property(e => e.CSV_GR).HasMaxLength(15);

                entity.Property(e => e.CURLING).HasMaxLength(15);

                entity.Property(e => e.DYEING_COMMENTS).HasMaxLength(150);

                entity.Property(e => e.FINISHDATE).HasColumnType("datetime");

                entity.Property(e => e.FN_COMMENTS).HasMaxLength(150);

                entity.Property(e => e.FN_MC_FI).HasMaxLength(50);

                entity.Property(e => e.FN_MC_NAME).HasMaxLength(50);

                entity.Property(e => e.GM_COMMENTS).HasMaxLength(150);

                entity.Property(e => e.HAND_FEEL).HasMaxLength(15);

                entity.Property(e => e.HP_COMMENTS).HasMaxLength(150);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.QUALITY_COMMENTS).HasMaxLength(150);

                entity.Property(e => e.SAMPLE_BS).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_CUT).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_OV).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_SR_WARP).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_SR_WEFT).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_ST_WARP).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_ST_WEFT).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_U_EPI).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_U_PPI).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_WG_AW).HasMaxLength(15);

                entity.Property(e => e.SAMPLE_WG_UW).HasMaxLength(15);

                entity.Property(e => e.TRIAL_NO).HasMaxLength(15);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BEAM)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.BEAMID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_F_PR_WEAVING_PROCESS_DETAILS_B");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.LBTEST_)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.LBTEST_ID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_RND_FABTEST_BULK");

                entity.HasOne(d => d.LGTEST_)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.LGTEST_ID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_RND_FABTEST_GREY");

                entity.HasOne(d => d.ORDERNONavigation)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.ORDERNO)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.TROLLY_NONavigation)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.TROLLY_NO)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_F_PR_FIN_TROLLY");

                entity.HasOne(d => d.TT)
                    .WithMany(p => p.F_FS_FABRIC_CLEARENCE_2ND_BEAM)
                    .HasForeignKey(d => d.TTID)
                    .HasConstraintName("FK_F_FS_FABRIC_CLEARENCE_2ND_BEAM_F_FS_FABRIC_TYPE");
            });

            //F_FS_FABRIC_TYPE
            modelBuilder.Entity<F_FS_FABRIC_TYPE>(entity =>
            {
                entity.HasKey(e => e.TTID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT6).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });


            modelBuilder.Entity<F_FS_ISSUE_TYPE>(entity =>
            {
                entity.Property(e => e.ISSUE_TYPE).HasMaxLength(100);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });
            //H_GS_IT
            modelBuilder.Entity<H_GS_ITEM_CATEGORY>(entity =>
            {
                entity.HasKey(e => e.CATID);

                entity.Property(e => e.CATID).ValueGeneratedOnAdd();

                entity.Property(e => e.CATNAME).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<H_GS_ITEM_SUBCATEGORY>(entity =>
            {
                entity.HasKey(e => e.SCATID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SCATNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.CAT)
                    .WithMany(p => p.H_GS_ITEM_SUBCATEGORY)
                    .HasForeignKey(d => d.CATID)
                    .HasConstraintName("FK_H_GS_ITEM_SUBCATEGORY_H_GS_ITEM_CATEGORY");
            });

            modelBuilder.Entity<H_GS_PRODUCT>(entity =>
            {
                entity.HasKey(e => e.PRODID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PROD_LOC).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SCAT)
                    .WithMany(p => p.H_GS_PRODUCT)
                    .HasForeignKey(d => d.SCATID)
                    .HasConstraintName("FK_H_GS_PRODUCT_H_GS_ITEM_SUBCATEGORY");

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.H_GS_PRODUCT)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_H_GS_PRODUCT_F_BAS_UNITS");
            });

            modelBuilder.Entity<LOOM_SETTINGS_SAMPLE>(entity =>
            {
                entity.HasKey(e => e.SETTINGS_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DEV)
                    .WithMany(p => p.LOOM_SETTINGS_SAMPLE)
                    .HasForeignKey(d => d.DEV_ID)
                    .HasConstraintName("FK_LOOM_SETTINGS_SAMPLE_RND_SAMPLE_INFO_WEAVING");
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_ISSUE>(entity =>
            {
                entity.HasKey(e => e.SFIID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.REQ_DATE).HasColumnType("datetime");

                entity.Property(e => e.ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.SRNO).IsRequired();

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.F_SAMPLE_FABRIC_ISSUE)
                    .HasForeignKey(d => d.BUYERID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_ISSUE_BAS_BUYERINFO");

                entity.HasOne(d => d.MKT_TEAM)
                    .WithMany(p => p.F_SAMPLE_FABRIC_ISSUE)
                    .HasForeignKey(d => d.MKT_TEAMID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_ISSUE_MKT_TEAM");

                entity.HasOne(d => d.BRAND)
                    .WithMany(p => p.F_SAMPLE_FABRIC_ISSUE)
                    .HasForeignKey(d => d.BRANDID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_ISSUE_BAS_BRANDINFO");
            });

            modelBuilder.Entity<F_FS_CLEARANCE_WASTAGE_ITEM>(entity =>
            {
                entity.HasKey(e => e.IID);

                entity.Property(e => e.INAME).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_FS_CLEARANCE_WASTAGE_TRANSFER>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.TRANSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CLRBYNavigation)
                    .WithMany(p => p.F_FS_CLEARANCE_WASTAGE_TRANSFER)
                    .HasForeignKey(d => d.CLRBY)
                    .HasConstraintName("FK_F_FS_CLEARANCE_WASTAGE_TRANSFER_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.ITEMNavigation)
                    .WithMany(p => p.F_FS_CLEARANCE_WASTAGE_TRANSFER)
                    .HasForeignKey(d => d.ITEM)
                    .HasConstraintName("FK_F_FS_CLEARANCE_WASTAGE_TRANSFER_F_FS_CLEARANCE_WASTAGE_ITEM");
            });

            modelBuilder.Entity<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>(entity =>
            {
                entity.HasKey(e => e.MSRID);

                entity.Property(e => e.APPEAR_B).HasMaxLength(10);

                entity.Property(e => e.APPEAR_S).HasMaxLength(10);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.CSV_B).HasMaxLength(10);

                entity.Property(e => e.CSV_S).HasMaxLength(10);

                entity.Property(e => e.MAILDATE).HasColumnType("datetime");

                entity.Property(e => e.MSRDATE).HasColumnType("datetime");

                entity.Property(e => e.C_GM).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.ROLL)
                    .WithMany(p => p.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL)
                    .HasForeignKey(d => d.ROLLID)
                    .HasConstraintName("FK_F_FS_CLEARANCE_MASTER_SAMPLE_ROLL_F_PR_INSPECTION_PROCESS_DETAILS");

                entity.HasOne(d => d.RT)
                    .WithMany(p => p.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL)
                    .HasForeignKey(d => d.RTID)
                    .HasConstraintName("FK_F_FS_CLEARANCE_MASTER_SAMPLE_ROLL_F_FS_CLEARANCE_ROLL_TYPE");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_FS_CLEARANCE_MASTER_SAMPLE_ROLL_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.WT)
                    .WithMany(p => p.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL)
                    .HasForeignKey(d => d.WTID)
                    .HasConstraintName("FK_F_FS_CLEARANCE_MASTER_SAMPLE_ROLL_F_FS_CLEARANCE_WASH_TYPE");
            });

            modelBuilder.Entity<F_FS_CLEARANCE_ROLL_TYPE>(entity =>
            {
                entity.HasKey(e => e.RTID);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.RTNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<F_FS_CLEARANCE_WASH_TYPE>(entity =>
            {
                entity.HasKey(e => e.WTID);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.WTNAME).HasMaxLength(50);
            });

            modelBuilder.Entity<COM_IMP_WORK_ORDER_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.INDD)
                    .WithMany(p => p.COM_IMP_WORK_ORDER_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_COM_IMP_WORK_ORDER_DETAILS_F_YS_INDENT_DETAILS");

                entity.HasOne(d => d.WO)
                    .WithMany(p => p.COM_IMP_WORK_ORDER_DETAILS)
                    .HasForeignKey(d => d.WOID)
                    .HasConstraintName("FK_COM_IMP_WORK_ORDER_DETAILS_COM_IMP_WORK_ORDER_MASTER");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.COM_IMP_WORK_ORDER_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_COM_IMP_WORK_ORDER_DETAILS_BAS_YARN_LOTINFO");
            });

            modelBuilder.Entity<COM_IMP_WORK_ORDER_MASTER>(entity =>
            {
                entity.HasKey(e => e.WOID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WODATE).HasColumnType("datetime");

                entity.Property(e => e.VALDATE).HasColumnType("datetime");

                entity.Property(e => e.CONTNO).HasMaxLength(50);

                entity.HasOne(d => d.SUPP)
                    .WithMany(p => p.COM_IMP_WORK_ORDER_MASTER)
                    .HasForeignKey(d => d.SUPPID)
                    .HasConstraintName("FK_COM_IMP_WORK_ORDER_MASTER_BAS_SUPPLIERINFO");
                entity.HasOne(d => d.IND)
                    .WithMany(p => p.COM_IMP_WORK_ORDER_MASTER)
                    .HasForeignKey(d => d.INDID)
                    .HasConstraintName("FK_COM_IMP_WORK_ORDER_MASTER_F_YS_INDENT_MASTER");
            });

            modelBuilder.Entity<F_YARN_QC_APPROVE_S>(entity =>
            {
                entity.HasKey(e => e.YQCA);

                entity.Property(e => e.APPROVED_BY).HasMaxLength(200);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YQCADATE).HasColumnType("datetime");

                entity.HasOne(d => d.YRD)
                    .WithMany(p => p.F_YARN_QC_APPROVE_S)
                    .HasForeignKey(d => d.YRDID)
                    .HasConstraintName("FK_F_YARN_QC_APPROVE_S_F_YS_YARN_RECEIVE_DETAILS_S");
            });

            modelBuilder.Entity<F_YARN_REQ_DETAILS_S>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_YARN_REQ_DETAILS_S)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_S_RND_FABRIC_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YARN_REQ_DETAILS_S)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_S_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.ORDERNONavigation)
                    .WithMany(p => p.F_YARN_REQ_DETAILS_S)
                    .HasForeignKey(d => d.ORDERNO)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_S_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_YARN_REQ_DETAILS_S)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_S_PL_PRODUCTION_SETDISTRIBUTION");

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.F_YARN_REQ_DETAILS_S)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_S_F_BAS_UNITS");

                entity.HasOne(d => d.YSR)
                    .WithMany(p => p.F_YARN_REQ_DETAILS_S)
                    .HasForeignKey(d => d.YSRID)
                    .HasConstraintName("FK_F_YARN_REQ_DETAILS_S_F_YARN_REQ_MASTER_S");
            });

            modelBuilder.Entity<F_YARN_REQ_MASTER_S>(entity =>
            {
                entity.HasKey(e => e.YSRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YSRDATE).HasColumnType("datetime");

                entity.Property(e => e.YSRNO).HasMaxLength(50);
            });

            modelBuilder.Entity<F_YARN_TRANSACTION_S>(entity =>
            {
                entity.HasKey(e => e.YTRNID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YTRNDATE).HasColumnType("datetime");

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_YARN_TRANSACTION_S)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_S_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.ISSUE)
                    .WithMany(p => p.F_YARN_TRANSACTION_S)
                    .HasForeignKey(d => d.ISSUEID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_S_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YARN_TRANSACTION_S)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_S_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_YARN_TRANSACTION_S)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_S_F_BAS_RECEIVE_TYPE");

                entity.HasOne(d => d.YISSUE)
                    .WithMany(p => p.F_YARN_TRANSACTION_S)
                    .HasForeignKey(d => d.YISSUEID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_S_F_YS_YARN_ISSUE_DETAILS_S");

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_YARN_TRANSACTION_S)
                    .HasForeignKey(d => d.YRCVID)
                    .HasConstraintName("FK_F_YARN_TRANSACTION_S_F_YS_YARN_RECEIVE_DETAILS_S");
            });

            modelBuilder.Entity<F_YS_YARN_ISSUE_DETAILS_S>(entity =>
            {
                entity.HasKey(e => e.TRANSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.RefBasYarnCountinfo)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S_MAIN)
                    .HasForeignKey(d => d.MAIN_COUNTID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_BAS_YARN_COUNTINFO_MAIN");

                entity.HasOne(d => d.LOCATION)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_F_YS_LOCATION");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.UNITNavigation)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S)
                    .HasForeignKey(d => d.UNIT)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_F_BAS_UNITS");

                entity.HasOne(d => d.YISSUE)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S)
                    .HasForeignKey(d => d.YISSUEID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_F_YS_YARN_ISSUE_MASTER_S");

                entity.HasOne(d => d.RCVD)
                    .WithMany(p => p.F_YS_YARN_ISSUE_DETAILS_S)
                    .HasForeignKey(d => d.RCVDID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_DETAILS_S_F_YS_YARN_RECEIVE_DETAILS_S");
            });

            modelBuilder.Entity<F_YS_YARN_ISSUE_MASTER_S>(entity =>
            {
                entity.HasKey(e => e.YISSUEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.ISSUETO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.PURPOSE).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YISSUEDATE).HasColumnType("datetime");

                entity.HasOne(d => d.ISSUE)
                    .WithMany(p => p.F_YS_YARN_ISSUE_MASTER_S)
                    .HasForeignKey(d => d.ISSUEID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_MASTER_S_F_BAS_ISSUE_TYPE");

                entity.HasOne(d => d.YSR)
                    .WithMany(p => p.F_YS_YARN_ISSUE_MASTER_S)
                    .HasForeignKey(d => d.YSRID)
                    .HasConstraintName("FK_F_YS_YARN_ISSUE_MASTER_S_F_YARN_REQ_MASTER_S");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_DETAILS_S>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.LEDGER)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS_S)
                    .HasForeignKey(d => d.LEDGERID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_S_F_YS_LEDGER");

                entity.HasOne(d => d.LOCATION)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS_S)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_S_F_YS_LOCATION");

                entity.HasOne(d => d.LOTNavigation)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS_S)
                    .HasForeignKey(d => d.LOT)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_S_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.PROD)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS_S)
                    .HasForeignKey(d => d.PRODID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_S_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.RAWNavigation)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS_S)
                    .HasForeignKey(d => d.RAW)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_S_F_YS_RAW_PER");

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS_S)
                    .HasForeignKey(d => d.YRCVID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS_S_F_YS_YARN_RECEIVE_MASTER_S");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_MASTER_S>(entity =>
            {
                entity.HasKey(e => e.YRCVID);

                entity.Property(e => e.CHALLANDATE).HasColumnType("datetime");

                entity.Property(e => e.CHALLANNO).HasMaxLength(50);

                entity.Property(e => e.COMMENTS).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.G_ENTRY_DATE).HasColumnType("datetime");

                entity.Property(e => e.G_ENTRY_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.TUCK_NO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YRCVDATE).HasColumnType("datetime");

                entity.HasOne(d => d.IND)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER_S)
                    .HasForeignKey(d => d.INDID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_S_F_YS_INDENT_MASTER");

                entity.HasOne(d => d.INV)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER_S)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_S_COM_IMP_INVOICEINFO");

                entity.HasOne(d => d.RCVFROMNavigation)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER_S)
                    .HasForeignKey(d => d.RCVFROM)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_S_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.RCVT)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER_S)
                    .HasForeignKey(d => d.RCVTID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER_S_F_BAS_RECEIVE_TYPE");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_REPORT_S>(entity =>
            {
                entity.HasKey(e => e.MRRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.MRRDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_ISSUE_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SFIDID);

                entity.HasOne(d => d.F_SAMPLE_FABRIC_ISSUE)
                    .WithMany(p => p.F_SAMPLE_FABRIC_ISSUE_DETAILS)
                    .HasForeignKey(d => d.SFIID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_ISSUE_DETAILS_F_SAMPLE_FABRIC_ISSUE");

                entity.HasOne(d => d.Fabricinfo)
                    .WithMany(p => p.F_SAMPLE_FABRIC_ISSUE_DETAILS)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_ISSUE_DETAILS_RND_FABRICINFO");
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_RCV_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.HasOne(d => d.SFR)
                    .WithMany(p => p.F_SAMPLE_FABRIC_RCV_D)
                    .HasForeignKey(d => d.SFRID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_RCV_D_F_SAMPLE_FABRIC_RCV_M");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.FSampleFabricRcvDs)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_RCV_D_RND_FABRICINFO");

                entity.HasOne(d => d.SITEM)
                    .WithMany(p => p.F_SAMPLE_FABRIC_RCV_D)
                    .HasForeignKey(d => d.SITEMID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_RCV_D_F_SAMPLE_ITEM_DETAILS");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_SAMPLE_FABRIC_RCV_D)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_RCV_D_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_RCV_M>(entity =>
            {
                entity.HasKey(e => e.SFRID);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.SFRDATE).HasColumnType("datetime");

                entity.Property(e => e.SFTRDATE).HasColumnType("datetime");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_SAMPLE_FABRIC_RCV_M)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_RCV_M_F_BAS_SECTION");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_SAMPLE_FABRIC_RCV_M)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_RCV_M_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_DISPATCH_DETAILS>(entity =>
            {
                entity.HasKey(e => e.DPDID);

                entity.HasOne(d => d.DP)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.DPID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_F_SAMPLE_FABRIC_DISPATCH_MASTER");

                entity.HasOne(d => d.FSampleFabricRcvD)
                    .WithMany(p => p.FSampleFabricDispatchDetailses)
                    .HasForeignKey(d => d.TRNSID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_F_SAMPLE_FABRIC_RCV_D");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.BYERID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_BAS_BUYERINFO");

                entity.HasOne(d => d.TEAM)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.MKT_TEAMID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_MKT_TEAM");

                entity.HasOne(d => d.UNIT)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.UID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_F_BAS_UNITS");

                entity.HasOne(d => d.STYPE)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.STYPEID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE");

                entity.HasOne(d => d.BRAND)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.BRANDID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_DETAILS_BAS_BRANDINFO");
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE>(entity =>
            {
                entity.HasKey(e => e.STYPEID);
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_DISPATCH_MASTER>(entity =>
            {
                entity.HasKey(e => e.DPID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.DR)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.DRID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_MASTER_F_BAS_DRIVERINFO");

                entity.HasOne(d => d.GPTYPE)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.GPTYPEID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_MASTER_GATEPASS_TYPE");

                entity.HasOne(d => d.V)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.VID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_MASTER_F_BAS_VEHICLE_INFO");
            });

            modelBuilder.Entity<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION>(entity =>
            {
                entity.HasKey(e => e.TID);

                entity.HasOne(d => d.DPD)
                    .WithMany(p => p.F_SAMPLE_FABRIC_DISPATCH_TRANSACTION)
                    .HasForeignKey(d => d.DPDID)
                    .HasConstraintName("FK_F_SAMPLE_FABRIC_DISPATCH_TRANSACTION_F_SAMPLE_FABRIC_DISPATCH_DETAILS");
            });

            modelBuilder.Entity<H_SAMPLE_FABRIC_RECEIVING_D>(entity =>
            {
                entity.HasKey(e => e.RCVDID);

                entity.HasOne(d => d.DPD)
                    .WithMany(p => p.H_SAMPLE_FABRIC_RECEIVING_D)
                    .HasForeignKey(d => d.DPDID)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_RECEIVING_D_F_SAMPLE_FABRIC_DISPATCH_DETAILS");

                entity.HasOne(d => d.RCV)
                    .WithMany(p => p.H_SAMPLE_FABRIC_RECEIVING_D)
                    .HasForeignKey(d => d.RCVID)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_RECEIVING_D_H_SAMPLE_FABRIC_RECEIVING_M");
            });

            modelBuilder.Entity<H_SAMPLE_FABRIC_RECEIVING_M>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.DPID).IsRequired();

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.HasOne(d => d.FSampleFabricDispatchMaster)
                    .WithMany(p => p.H_SAMPLE_FABRIC_RECEIVING_M)
                    .HasForeignKey(d => d.DPID)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_RECEIVING_M_F_SAMPLE_FABRIC_DISPATCH_MASTER");
            });

            modelBuilder.Entity<H_SAMPLE_FABRIC_DISPATCH_MASTER>(entity =>
            {
                entity.HasKey(e => e.SFDID);

                entity.Property(e => e.ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.RETURN_DATE).HasColumnType("datetime");

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.H_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.BUYERID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_DISPATCH_MASTER_BAS_BUYERINFO");

                entity.HasOne(d => d.BRAND)
                    .WithMany(p => p.H_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.BRANDID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_DISPATCH_MASTER_BAS_BRANDINFO");

                entity.HasOne(d => d.MERCH)
                    .WithMany(p => p.H_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.MERCID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_DISPATCH_MASTER_MERCHANDISER");

                entity.HasOne(d => d.MktTeam)
                    .WithMany(p => p.H_SAMPLE_FABRIC_DISPATCH_MASTER)
                    .HasForeignKey(d => d.MERCID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_DISPATCH_MASTER_MKT_TEAM");
            });

            modelBuilder.Entity<H_SAMPLE_FABRIC_DISPATCH_DETAILS>(entity =>
            {
                entity.HasKey(e => e.SFDDID);

                entity.HasOne(d => d.SFD)
                    .WithMany(p => p.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.SFDID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_DISPATCH_DETAILS_H_SAMPLE_FABRIC_DISPATCH_MASTER");

                entity.HasOne(d => d.RCVD)
                    .WithMany(p => p.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .HasForeignKey(d => d.RCVDID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_SAMPLE_FABRIC_DISPATCH_DETAILS_H_SAMPLE_FABRIC_RECEIVING_D");
            });

            modelBuilder.Entity<MERCHANDISER>(entity =>
            {
                entity.HasKey(e => e.MERCID);
            });

            modelBuilder.Entity<F_BAS_UNITS>(entity =>
            {
                entity.HasKey(e => e.UID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UNAME).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            //F_WASTAGE

            modelBuilder.Entity<F_WASTE_PRODUCTINFO>(entity =>
            {
                entity.HasKey(e => e.WPID);

                entity.Property(e => e.WPID).ValueGeneratedOnAdd();

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PRODUCT_NAME).IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UID)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WPTYPE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.WP)
                    .WithOne(p => p.F_WASTE_PRODUCTINFO)
                    .HasForeignKey<F_WASTE_PRODUCTINFO>(d => d.UID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_F_WASTE_PRODUCTINFO_F_BAS_UNITS");
            });
            modelBuilder.Entity<F_GS_WASTAGE_RECEIVE_D>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.WP)
                    .WithMany(p => p.F_GS_WASTAGE_RECEIVE_D)
                    .HasForeignKey(d => d.WPID)
                    .HasConstraintName("FK_F_GS_WASTAGE_RECEIVE_D_F_WASTE_PRODUCTINFO");
            });

            modelBuilder.Entity<F_GS_WASTAGE_RECEIVE_M>(entity =>
            {
                entity.HasKey(e => e.WRID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WRDATE).HasColumnType("datetime");

                entity.Property(e => e.WTRDATE).HasColumnType("datetime");

                entity.Property(e => e.WTRNO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_GS_WASTAGE_RECEIVE_M)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_GS_WASTAGE_RECEIVE_M_F_BAS_SECTION");
            });


            //COM_EX_CERTIFICATE

            modelBuilder.Entity<COM_EX_CERTIFICATE_MANAGEMENT>(entity =>
            {
                entity.HasKey(e => e.CMID);

                entity.Property(e => e.BCI_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.BCI_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.BCI_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BCI_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CMIA_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.CMIA_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CMIA_REP_DATE).HasColumnType("datetime");

                entity.Property(e => e.CMIA_RETAILER)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GRS_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.GRS_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.GRS_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GRS_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IC_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.IC_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IC_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IC_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IC_TYPE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.ORGANIC_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.ORGANIC_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.ORGANIC_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ORGANIC_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ORGANIC_TYPE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PSCP_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.PSCP_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.PSCP_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PSCP_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RCS_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.RCS_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.RCS_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RCS_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TENCEL_APP_DATE).HasColumnType("datetime");

                entity.Property(e => e.TENCEL_ISSUE_DATE).HasColumnType("datetime");

                entity.Property(e => e.TENCEL_REF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TENCEL_REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.INV)
                    .WithMany(p => p.COM_EX_CERTIFICATE_MANAGEMENT)
                    .HasForeignKey(d => d.INVID)
                    .HasConstraintName("FK_COM_EX_CERTIFICATE_MANAGEMENT_COM_EX_INVOICEMASTER");
            });

            modelBuilder.Entity<COM_EX_INVOICEMASTER>(entity =>
            {
                entity.HasKey(e => e.INVID);

                entity.Property(e => e.BANK_REF).HasMaxLength(50);

                entity.Property(e => e.BILL_DATE).HasColumnType("datetime");

                entity.Property(e => e.BNK_ACC_DATE).HasColumnType("datetime");

                entity.Property(e => e.BNK_ACC_POSTING).HasColumnType("datetime");

                entity.Property(e => e.BNK_SUB_DATE).HasColumnType("datetime");

                entity.Property(e => e.DELDATE).HasColumnType("datetime");

                entity.Property(e => e.DISCREPANCY).HasMaxLength(50);

                entity.Property(e => e.DOC_NOTES).HasMaxLength(50);

                entity.Property(e => e.DOC_RCV_DATE).HasColumnType("datetime");

                entity.Property(e => e.DOC_SUB_DATE).HasColumnType("datetime");

                entity.Property(e => e.EXDATE).HasColumnType("datetime");

                entity.Property(e => e.INVDATE).HasColumnType("datetime");

                entity.Property(e => e.INVDURATION).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.INVNO)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.INVREF).HasMaxLength(50);

                entity.Property(e => e.MATUDATE).HasColumnType("datetime");

                entity.Property(e => e.NEGODATE).HasColumnType("datetime");

                entity.Property(e => e.ODRCVDATE).HasColumnType("datetime");

                entity.Property(e => e.PDOCNO).HasMaxLength(50);

                entity.Property(e => e.PRCDATE).HasColumnType("datetime");

                entity.Property(e => e.STATUS).HasMaxLength(50);

                entity.Property(e => e.USRID).HasMaxLength(50);
            });

            //F_PR_INSPECTION_FABRIC_Dispatch Model Binder

            modelBuilder.Entity<F_PR_INSPECTION_FABRIC_D_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OPT7).HasMaxLength(50);

                entity.Property(e => e.OPT8).HasMaxLength(50);

                entity.Property(e => e.QC_APPROVE_DATE).HasColumnType("datetime");

                entity.Property(e => e.QC_REJECT_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.D)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .HasForeignKey(d => d.DID)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_DETAILS_F_PR_INSPECTION_FABRIC_D_MASTER");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_DETAILS_RND_FABRICINFO");

                entity.HasOne(d => d.LOCATIONNavigation)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .HasForeignKey(d => d.LOCATION)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_DETAILS_F_FS_LOCATION");

                entity.HasOne(d => d.PO_NONavigation)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .HasForeignKey(d => d.PO_NO)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_DETAILS_COM_EX_PIMASTER");

                entity.HasOne(d => d.ROLL_)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .HasForeignKey(d => d.ROLL_ID)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_DETAILS_F_PR_INSPECTION_PROCESS_DETAILS");

                entity.HasOne(d => d.SO_NONavigation)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .HasForeignKey(d => d.SO_NO)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_DETAILS_COM_EX_PI_DETAILS");
            });

            modelBuilder.Entity<F_PR_INSPECTION_FABRIC_D_MASTER>(entity =>
            {
                entity.HasKey(e => e.DID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DDATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_PR_INSPECTION_FABRIC_D_MASTER)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_PR_INSPECTION_FABRIC_D_MASTER_F_BAS_SECTION");
            });

            modelBuilder.Entity<COM_EX_ADV_DELIVERY_SCH_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT5)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.DS)
                    .WithMany(p => p.COM_EX_ADV_DELIVERY_SCH_DETAILS)
                    .HasForeignKey(d => d.DSID)
                    .HasConstraintName("FK_COM_EX_ADV_DELIVERY_SCH_DETAILS_COM_EX_ADV_DELIVERY_SCH_MASTER");

                entity.HasOne(d => d.PI)
                    .WithMany(p => p.COM_EX_ADV_DELIVERY_SCH_DETAILS)
                    .HasForeignKey(d => d.PIID)
                    .HasConstraintName("FK_COM_EX_ADV_DELIVERY_SCH_DETAILS_COM_EX_PIMASTER");

                entity.HasOne(d => d.STYLE)
                    .WithMany(p => p.COM_EX_ADV_DELIVERY_SCH_DETAILS)
                    .HasForeignKey(d => d.STYLE_ID)
                    .HasConstraintName("FK_COM_EX_ADV_DELIVERY_SCH_DETAILS_COM_EX_PI_DETAILS");
            });

            modelBuilder.Entity<COM_EX_ADV_DELIVERY_SCH_MASTER>(entity =>
            {
                entity.HasKey(e => e.DSID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DSDATE).HasColumnType("datetime");

                entity.Property(e => e.DSNO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DSTYPE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BUYER)
                    .WithMany(p => p.COM_EX_ADV_DELIVERY_SCH_MASTER)
                    .HasForeignKey(d => d.BUYER_ID)
                    .HasConstraintName("FK_COM_EX_ADV_DELIVERY_SCH_MASTER_BAS_BUYERINFO");
            });

            modelBuilder.Entity<MAILBOX>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(450);

                entity.Property(e => e.IP_PHONE).HasMaxLength(50);

                entity.Property(e => e.MAIL_ATTACHMENT).HasMaxLength(50);

                entity.Property(e => e.MAIL_FROM).HasMaxLength(50);

                entity.Property(e => e.MOBILE_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);
            });

            modelBuilder.Entity<ACC_PHYSICAL_INVENTORY_FAB>(entity =>
            {
                entity.HasKey(e => e.FPI_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FPI_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.ROLL_)
                    .WithMany(p => p.ACC_PHYSICAL_INVENTORY_FAB)
                    .HasForeignKey(d => d.ROLL_ID)
                    .HasConstraintName("FK_ACC_PHYSICAL_INVENTORY_FAB_F_FS_FABRIC_RCV_DETAILS");
            });

            modelBuilder.Entity<F_YS_PARTY_INFO>(entity =>
            {
                entity.HasKey(e => e.PARTY_ID);

                entity.Property(e => e.ADDRESS);

                entity.Property(e => e.CELL_NO).HasMaxLength(50);

                entity.Property(e => e.CONTRACT_PERSON).HasMaxLength(50);

                entity.Property(e => e.PARTY_NAME).HasMaxLength(50);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<F_YS_GP_MASTER>(entity =>
            {
                entity.HasKey(e => e.GPID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.GPDATE).HasColumnType("datetime");

                entity.Property(e => e.GPNO).HasMaxLength(50);

                entity.Property(e => e.GPTYPE).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.LC_)
                    .WithMany(p => p.F_YS_GP_MASTER)
                    .HasForeignKey(d => d.LC_ID)
                    .HasConstraintName("FK_F_YS_GP_MASTER_COM_IMP_LCINFORMATION");

                entity.HasOne(d => d.PARTY_)
                    .WithMany(p => p.F_YS_GP_MASTER)
                    .HasForeignKey(d => d.PARTY_ID)
                    .HasConstraintName("FK_F_YS_GP_MASTER_F_YS_PARTY_INFO");
            });

            modelBuilder.Entity<F_YS_GP_DETAILS>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.PAGENO).HasMaxLength(50);

                entity.Property(e => e.QTY_BAGS).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.QTY_KGS).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.GP)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.GPID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_F_YS_GP_MASTER");

                entity.HasOne(d => d.LEDGER_)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.LEDGER_ID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_F_YS_LEDGER");

                entity.HasOne(d => d.LOCATION_)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.LOCATION_ID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_F_YS_LOCATION");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.RCV)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.INDSLID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_F_YS_YARN_RECEIVE_DETAILS");

                entity.HasOne(d => d.STOCK)
                    .WithMany(p => p.F_YS_GP_DETAILS)
                    .HasForeignKey(d => d.STOCKID)
                    .HasConstraintName("FK_F_YS_GP_DETAILS_F_YARN_TRANSACTION_TYPE");

            });

            modelBuilder.Entity<F_FS_FABRIC_RETURN_RECEIVE>(entity =>
            {
                entity.HasKey(e => e.RCVID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DC_NO).IsUnicode(false);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OPT4).HasMaxLength(50);

                entity.Property(e => e.RCVDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.BUYER_)
                    .WithMany(p => p.F_FS_FABRIC_RETURN_RECEIVE)
                    .HasForeignKey(d => d.BUYER_ID)
                    .HasConstraintName("FK_F_FS_FABRIC_RETURN_RECEIVE_BAS_BUYERINFO");

                entity.HasOne(d => d.DO_NONavigation)
                    .WithMany(p => p.F_FS_FABRIC_RETURN_RECEIVE)
                    .HasForeignKey(d => d.DO_NO)
                    .HasConstraintName("FK_F_FS_FABRIC_RETURN_RECEIVE_ACC_EXPORT_DOMASTER");

                entity.HasOne(d => d.FABCODENavigation)
                    .WithMany(p => p.F_FS_FABRIC_RETURN_RECEIVE)
                    .HasForeignKey(d => d.FABCODE)
                    .HasConstraintName("FK_F_FS_FABRIC_RETURN_RECEIVE_RND_FABRICINFO");

                entity.HasOne(d => d.PI)
                    .WithMany(p => p.F_FS_FABRIC_RETURN_RECEIVE)
                    .HasForeignKey(d => d.PI_NO)
                    .HasConstraintName("FK_F_FS_FABRIC_RETURN_RECEIVE_COM_EX_PIMASTER");
            });

            modelBuilder.Entity<F_PR_WEAVING_OS>(entity =>
            {
                entity.HasKey(e => e.OSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3).HasMaxLength(50);

                entity.Property(e => e.OS)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OSDATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.COUNT)
                    .WithMany(p => p.F_PR_WEAVING_OS)
                    .HasForeignKey(d => d.COUNTID)
                    .HasConstraintName("FK_F_PR_WEAVING_OS_BAS_YARN_COUNTINFO");

                entity.HasOne(d => d.LOT)
                    .WithMany(p => p.F_PR_WEAVING_OS)
                    .HasForeignKey(d => d.LOTID)
                    .HasConstraintName("FK_F_PR_WEAVING_OS_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.PO)
                    .WithMany(p => p.F_PR_WEAVING_OS)
                    .HasForeignKey(d => d.POID)
                    .HasConstraintName("FK_F_PR_WEAVING_OS_RND_PRODUCTION_ORDER");

                entity.HasOne(d => d.SET)
                    .WithMany(p => p.F_PR_WEAVING_OS)
                    .HasForeignKey(d => d.SETID)
                    .HasConstraintName("FK_F_PR_WEAVING_OS_PL_PRODUCTION_SETDISTRIBUTION");
            });

            modelBuilder.Entity<F_YARN_TRANSACTION_TYPE>(entity =>
            {
                entity.HasKey(e => e.STOCKID);

                entity.Property(e => e.NAME).HasMaxLength(50);

            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_MASTER2>(entity =>
            {
                entity.HasKey(e => e.YRCVID);

                entity.Property(e => e.CHALLANDATE).HasColumnType("datetime");

                entity.Property(e => e.CHALLANNO).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.G_ENTRY_DATE).HasColumnType("datetime");

                entity.Property(e => e.G_ENTRY_NO).HasMaxLength(50);

                entity.Property(e => e.INDENT_TYPE).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.TRUCKNO).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.YRCVDATE).HasColumnType("datetime");

                entity.HasOne(d => d.RCVFROMNavigation)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER2)
                    .HasForeignKey(d => d.RCVFROM)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER2_BAS_SUPPLIERINFO");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER2)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER2_F_BAS_SECTION");

                entity.HasOne(d => d.SUBSEC)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER2)
                    .HasForeignKey(d => d.SUBSECID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER2_F_BAS_SUBSECTION");

                entity.HasOne(d => d.SO)
                   .WithMany(p => p.F_YS_YARN_RECEIVE_MASTER2)
                   .HasForeignKey(d => d.SO_NO)
                   .HasConstraintName("FK_F_YS_YARN_RECEIVE_MASTER2_RND_PRODUCTION_ORDER");
            });

            modelBuilder.Entity<F_YS_YARN_RECEIVE_DETAILS2>(entity =>
            {
                entity.HasKey(e => e.TRNSID);

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.REAMRKS).HasMaxLength(50);

                entity.Property(e => e.TRNSDATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.LEDGER)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS2)
                    .HasForeignKey(d => d.LEDGERID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS2_F_YS_LEDGER");

                entity.HasOne(d => d.LOCATION)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS2)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS2_F_YS_LOCATION");

                entity.HasOne(d => d.LOTNavigation)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS2)
                    .HasForeignKey(d => d.LOT)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS2_BAS_YARN_LOTINFO");

                entity.HasOne(d => d.RAWNavigation)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS2)
                    .HasForeignKey(d => d.RAW)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS2_F_YS_RAW_PER");

                entity.HasOne(d => d.YRCV)
                    .WithMany(p => p.F_YS_YARN_RECEIVE_DETAILS2)
                    .HasForeignKey(d => d.YRCVID)
                    .HasConstraintName("FK_F_YS_YARN_RECEIVE_DETAILS2_F_YS_YARN_RECEIVE_MASTER2");
            });

            modelBuilder.Entity<F_BAS_HRD_DEPARTMENT>(entity =>
            {
                entity.HasKey(e => e.DEPTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DEPTNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DEPTNAME_BN).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.LOCATION)
                    .WithMany(p => p.F_BAS_HRD_DEPARTMENT)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_BAS_HRD_DEPARTMENT_F_BAS_HRD_LOCATION");
            });

            modelBuilder.Entity<F_BAS_HRD_DESIGNATION>(entity =>
            {
                entity.HasKey(e => e.DESID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DES_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DES_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.GRADE)
                    .WithMany(p => p.F_BAS_HRD_DESIGNATION)
                    .HasForeignKey(d => d.GRADEID)
                    .HasConstraintName("FK_F_BAS_HRD_DESIGNATION_F_BAS_HRD_GRADE");
            });

            modelBuilder.Entity<F_BAS_HRD_SECTION>(entity =>
            {
                entity.HasKey(e => e.SECID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SEC_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SEC_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_SUB_SECTION>(entity =>
            {
                entity.HasKey(e => e.SUBSECID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SUBSEC_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SUBSEC_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.SUBSEC_SNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SUBSEC_SNAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_GRADE>(entity =>
            {
                entity.HasKey(e => e.GRADEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GRADE_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_LOCATION>(entity =>
            {
                entity.HasKey(e => e.LOCID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LOC_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_ATTEN_TYPE>(entity =>
            {
                entity.HasKey(e => e.ATTYPID);

                entity.Property(e => e.ATTYPID_DESC).IsUnicode(false);

                entity.Property(e => e.ATTYPID_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_BLOOD_GROUP>(entity =>
            {
                entity.HasKey(e => e.BLDGRPID);

                entity.Property(e => e.BLDGRP_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_DISTRICT>(entity =>
            {
                entity.HasKey(e => e.DISTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DIST_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DIST_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEBSITE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DIV)
                    .WithMany(p => p.F_BAS_HRD_DISTRICT)
                    .HasForeignKey(d => d.DIVID)
                    .HasConstraintName("FK_F_BAS_HRD_DISTRICT_F_BAS_HRD_DIVISION");
            });

            modelBuilder.Entity<F_BAS_HRD_DIVISION>(entity =>
            {
                entity.HasKey(e => e.DIVID);

                entity.Property(e => e.DIVID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DIV_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DIV_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEBSITE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.COUNTRY)
                    .WithMany(p => p.F_BAS_HRD_DIVISION)
                    .HasForeignKey(d => d.COUNTRYID)
                    .HasConstraintName("FK_F_BAS_HRD_DIVISION_F_BAS_HRD_NATIONALITY");
            });

            modelBuilder.Entity<F_BAS_HRD_EMP_RELATION>(entity =>
            {
                entity.HasKey(e => e.RELATIONID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_EMP_TYPE>(entity =>
            {
                entity.HasKey(e => e.TYPEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TYPE_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_NATIONALITY>(entity =>
            {
                entity.HasKey(e => e.NATIONID);

                entity.Property(e => e.COUNTRY_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.COUNTRY_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);
                
                entity.Property(e => e.NATION_DESC)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NATION_DESC_BN).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CUR)
                    .WithMany(p => p.F_BAS_HRD_NATIONALITY)
                    .HasForeignKey(d => d.CURRENCYID)
                    .HasConstraintName("FK_F_BAS_HRD_NATIONALITY_CURRENCY");
            });

            modelBuilder.Entity<F_BAS_HRD_OUT_REASON>(entity =>
            {
                entity.HasKey(e => e.RESASON_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RESASON_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_RELIGION>(entity =>
            {
                entity.HasKey(e => e.RELIGIONID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RELEGION_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RELEGION_NAME_BNG).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_SHIFT>(entity =>
            {
                entity.HasKey(e => e.SHIFTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHIFT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_THANA>(entity =>
            {
                entity.HasKey(e => e.THANAID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.THANA_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.THANA_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEBSITE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DIST)
                    .WithMany(p => p.F_BAS_HRD_THANA)
                    .HasForeignKey(d => d.DISTID)
                    .HasConstraintName("FK_F_BAS_HRD_THANA_F_BAS_HRD_DISTRICT");
            });

            modelBuilder.Entity<F_BAS_HRD_UNION>(entity =>
            {
                entity.HasKey(e => e.UNIONID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.THANA)
                    .WithMany(p => p.F_BAS_HRD_UNION)
                    .HasForeignKey(d => d.THANAID)
                    .HasConstraintName("FK_F_BAS_HRD_UNION_F_BAS_HRD_THANA");
            });

            modelBuilder.Entity<F_BAS_HRD_WEEKEND>(entity =>
            {
                entity.HasKey(e => e.ODID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OD_FULL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OD_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_HRD_EDUCATION>(entity =>
            {
                entity.HasKey(e => e.EDUID);

                entity.Property(e => e.BOARD_UNI)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CGPA)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MAJOR)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OUTOF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PASS_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DEG)
                    .WithMany(p => p.F_HRD_EDUCATION)
                    .HasForeignKey(d => d.DEGID)
                    .HasConstraintName("FK_F_HRD_EDUCATION_F_HRD_EMP_EDU_DEGREE");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EDUCATION)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EDUCATION_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_EMERGENCY>(entity =>
            {
                entity.HasKey(e => e.CONTID);

                entity.Property(e => e.ADDRESS).IsUnicode(false);

                entity.Property(e => e.CONT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EMERGENCY)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EMERGENCY_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.RELATION)
                    .WithMany(p => p.F_HRD_EMERGENCY)
                    .HasForeignKey(d => d.RELATIONID)
                    .HasConstraintName("FK_F_HRD_EMERGENCY_F_BAS_HRD_EMP_RELATION");
            });

            modelBuilder.Entity<F_HRD_EMP_CHILDREN>(entity =>
            {
                entity.HasKey(e => e.CHID);

                entity.Property(e => e.CH_BID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_DOB).HasColumnType("datetime");

                entity.Property(e => e.CH_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.CH_NID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_PASSPORT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_PROFESSION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EMP_CHILDREN)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EMP_CHILDREN_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_EMP_EDU_DEGREE>(entity =>
            {
                entity.HasKey(e => e.DEGID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEGNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_HRD_EMP_SPOUSE>(entity =>
            {
                entity.HasKey(e => e.SPID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SPNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SPNAME_BN).HasMaxLength(50);

                entity.Property(e => e.SP_BID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SP_DOB).HasColumnType("datetime");

                entity.Property(e => e.SP_NID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SP_PASSPORT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SP_PROFESSION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EMP_SPOUSE)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EMP_SPOUSE_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_EXPERIENCE>(entity =>
            {
                entity.HasKey(e => e.EXPID);

                entity.Property(e => e.COMPANY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).IsUnicode(false);

                entity.Property(e => e.DESIGNATION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.END_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.START_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EXPERIENCE)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EXPERIENCE_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_INCREMENT>(entity =>
            {
                entity.HasKey(e => e.INCID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INC_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_INCREMENT)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_INCREMENT_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_PROMOTION>(entity =>
            {
                entity.HasKey(e => e.PROMID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PROM_DATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_PROMOTION)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_PROMOTION_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.NEW_DEG)
                    .WithMany(p => p.F_HRD_PROMOTIONNEW_DEG)
                    .HasForeignKey(d => d.NEW_DEGID)
                    .HasConstraintName("FK_F_HRD_PROMOTION_F_BAS_HRD_DESIGNATION_NEW");

                entity.HasOne(d => d.OLD_DEG)
                    .WithMany(p => p.F_HRD_PROMOTIONOLD_DEG)
                    .HasForeignKey(d => d.OLD_DEGID)
                    .HasConstraintName("FK_F_HRD_PROMOTION_F_BAS_HRD_DESIGNATION_OLD");
            });

            modelBuilder.Entity<F_HRD_SKILL>(entity =>
            {
                entity.HasKey(e => e.SKILLID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SKILL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.LEVEL)
                    .WithMany(p => p.F_HRD_SKILL)
                    .HasForeignKey(d => d.LEVELID)
                    .HasConstraintName("FK_F_HRD_SKILL_F_HRD_SKILL_LEVEL");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_SKILL)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_SKILL_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_SKILL_LEVEL>(entity =>
            {
                entity.HasKey(e => e.LEVELID);

                entity.Property(e => e.LEVEL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BAS_GENDER>(entity =>
            {
                entity.HasKey(e => e.GENID);

                entity.Property(e => e.GENNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_HRD_EMPLOYEE>(entity =>
            {
                entity.HasKey(e => e.EMPID);

                entity.Property(e => e.ADD_PER).IsUnicode(false);

                entity.Property(e => e.ADD_PRE).IsUnicode(false);

                entity.Property(e => e.BANK_ACC_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BID_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DATE_BIRTH).HasColumnType("datetime");

                entity.Property(e => e.DATE_EMPSID_ISSUE).HasColumnType("datetime");

                entity.Property(e => e.DATE_JOINING).HasColumnType("datetime");

                entity.Property(e => e.DATE_TRANSFER).HasColumnType("datetime");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EMPNO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FIRST_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FIRST_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.F_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IMAGE_PP).HasColumnType("image");

                entity.Property(e => e.LAST_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LAST_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.MOBILE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_F)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_G)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_M)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.M_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NID_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OUT_DATE).HasColumnType("datetime");

                entity.Property(e => e.PASSPORT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PO_PER)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PO_PER_BN).HasMaxLength(50);

                entity.Property(e => e.PO_PRE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PROX_CARD)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TIN_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.VILLAGE_PER)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VILLAGE_PER_BN).HasMaxLength(50);

                entity.Property(e => e.VILLAGE_PRE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BLDGRP)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.BLDGRPID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_BLOOD_GROUP");

                entity.HasOne(d => d.DEPT)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.DEPTID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DEPARTMENT");

                entity.HasOne(d => d.DESIG)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.DESIGID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DESIGNATION");

                entity.HasOne(d => d.EMPTYPE)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.EMPTYPEID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_EMP_TYPE");

                entity.HasOne(d => d.NATIONALITY_)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.NATIONALITY_ID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_NATIONALITY");

                entity.HasOne(d => d.OD)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.ODID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_WEEKEND");

                entity.HasOne(d => d.OUT_RESASON_)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.OUT_RESASON_ID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_OUT_REASON");

                entity.HasOne(d => d.RELIGION)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.RELIGIONID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_RELIGION");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_SECTION");

                entity.HasOne(d => d.SHIFT)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.SHIFTID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_SHIFT");

                entity.HasOne(d => d.SUBSEC)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.SUBSECID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_SUB_SECTION");

                entity.HasOne(d => d.UNION_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.UNIONID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_UNION_PER");

                entity.HasOne(d => d.UNION_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.UNIONID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_UNION_PRE");

                entity.HasOne(d => d.COMPANY)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.COMPANYID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_COMPANY_INFO");

                entity.HasOne(d => d.GENDER)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.GENDERID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_BAS_GENDER");

                entity.HasOne(d => d.DIST_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.DISTID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DISTRICT_PER");

                entity.HasOne(d => d.DIST_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.DISTID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DISTRICT_PRE");

                entity.HasOne(d => d.DIV_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.DIVID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DIVISION_PER");

                entity.HasOne(d => d.DIV_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.DIVID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DIVISION_PRE");

                entity.HasOne(d => d.THANA_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.THANAID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_THANA_PER");

                entity.HasOne(d => d.THANA_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.THANAID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_THANA_PRE");
            });
        }
    }
}
