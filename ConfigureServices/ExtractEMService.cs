using System;
using System.Collections.Generic;
using System.Globalization;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures;
using DenimERP.ServiceInfrastructures.CompanyInfo;
using DenimERP.ServiceInfrastructures.Emp;
using DenimERP.ServiceInfrastructures.HR;
using DenimERP.ServiceInfrastructures.Hubs;
using DenimERP.ServiceInfrastructures.IdentityUser;
using DenimERP.ServiceInfrastructures.Marketing;
using DenimERP.ServiceInfrastructures.MenuMaster;
using DenimERP.ServiceInfrastructures.OtherInfrastructures;
using DenimERP.ServiceInfrastructures.PostCosting;
using DenimERP.ServiceInfrastructures.ProcWorkOrder;
using DenimERP.ServiceInfrastructures.SampleGarments.Fabric;
using DenimERP.ServiceInfrastructures.SampleGarments.GatePass;
using DenimERP.ServiceInfrastructures.SampleGarments.HDispatch;
using DenimERP.ServiceInfrastructures.SampleGarments.HReceive;
using DenimERP.ServiceInfrastructures.SampleGarments.Receive;
using DenimERP.ServiceInfrastructures.Security;
using DenimERP.ServiceInfrastructures.TargetSegment;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ServiceInterfaces.CompanyInfo;
using DenimERP.ServiceInterfaces.Emp;
using DenimERP.ServiceInterfaces.Factory;
using DenimERP.ServiceInterfaces.HR;
using DenimERP.ServiceInterfaces.Hubs;
using DenimERP.ServiceInterfaces.IdentityUser;
using DenimERP.ServiceInterfaces.Marketing;
using DenimERP.ServiceInterfaces.MenuMaster;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DenimERP.ServiceInterfaces.PostCosting;
using DenimERP.ServiceInterfaces.ProcWorkOrder;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using DenimERP.ServiceInterfaces.SampleGarments.HDispatch;
using DenimERP.ServiceInterfaces.SampleGarments.HReceive;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using DenimERP.ServiceInterfaces.TargetSegment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace DenimERP.ConfigureServices
{
    public static class ExtractEmService
    {
        public static void ExtractEmRegisterService(IServiceCollection services)
        {
            services.AddScoped<IH_SAMPLE_FABRIC_DISPATCH_DETAILS, SQLH_SAMPLE_FABRIC_DISPATCH_DETAILS_Repository>();
            services.AddScoped<IH_SAMPLE_FABRIC_DISPATCH_MASTER, SQLH_SAMPLE_FABRIC_DISPATCH_MASTER_Repository>();

            services.AddScoped<IH_SAMPLE_FABRIC_RECEIVING_D, SQLH_SAMPLE_FABRIC_RECEIVING_D_Repository>();
            services.AddScoped<IH_SAMPLE_FABRIC_RECEIVING_M, SQLH_SAMPLE_FABRIC_RECEIVING_M_Repository>();

            services.AddScoped<IF_SAMPLE_FABRIC_DISPATCH_TRANSACTION, SQLF_SAMPLE_FABRIC_DISPATCH_TRANSACTION_Repository>();
            services.AddScoped<IF_SAMPLE_FABRIC_DISPATCH_DETAILS, SQLF_SAMPLE_FABRIC_DISPATCH_DETAILS_Repository>();
            services.AddScoped<IF_SAMPLE_FABRIC_DISPATCH_MASTER, SQLF_SAMPLE_FABRIC_DISPATCH_MASTER_Repository>();

            services.AddScoped<IF_SAMPLE_FABRIC_RCV_D, SQLF_SAMPLE_FABRIC_RCV_D_Repository>();
            services.AddScoped<IF_SAMPLE_FABRIC_RCV_M, SQLF_SAMPLE_FABRIC_RCV_M_Repository>();

            services.AddScoped<IH_GS_ITEM_SUBCATEGORY, SQLH_GS_ITEM_SUBCATEGORY_Repository>();
            services.AddScoped<IH_GS_ITEM_CATEGORY, SQLH_GS_ITEM_CATEGORY_Repository>();

            services.AddScoped<IF_SAMPLE_FABRIC_ISSUE, SQLF_SAMPLE_FABRIC_ISSUE_Repository>();
            services.AddScoped<IF_SAMPLE_FABRIC_ISSUE_DETAILS, SQLF_SAMPLE_FABRIC_ISSUE_DETAILS_Repository>();

            services.AddScoped<IH_GS_PRODUCT, SQLH_GS_PRODUCT_Repository>();
            services.AddScoped<IH_GS_ITEM_SUBCATEGORY, SQLH_GS_ITEM_SUBCATEGORY_Repository>();

            services.AddScoped<ICOM_EX_CERTIFICATE_MANAGEMENT, SQLCOM_EX_CERTIFICATE_MANAGEMENT_Repository>();

            services.AddScoped<IBAS_COLOR, SQLBAS_COLOR_Repository>();
            services.AddScoped<IBAS_PRODCATEGORY, SQLBAS_PRODCATEGORY_Repository>();
            services.AddScoped<IBAS_PRODUCTINFO, SQLBAS_PRODUCTINFO_Repository>();
            services.AddScoped<IBAS_YARN_LOTINFO, SQLBAS_YARN_LOTINFO_Repository>();
            services.AddScoped<IBAS_SUPP_CATEGORY, SQLBAS_SUPP_CATEGORY_Repository>();
            services.AddScoped<IBAS_SUPPLIERINFO, SQLBAS_SUPPLIERINFO_Repository>();
            services.AddScoped<IBAS_YARN_COUNTINFO, SQLBAS_YARN_COUNTINFO_Repository>();
            services.AddScoped<IRND_DYEING_TYPE, SQLBAS_RND_DYEING_TYPE_Repository>();
            services.AddScoped<IBAS_BUYERINFO, SQLBAS_BUYERINFO_Repository>();
            services.AddScoped<IRND_FABRICINFO, SQLRND_FABRICINFO_Repository>();
            services.AddScoped<IRND_FABRIC_COUNTINFO, SQLRND_FABRIC_COUNTINFO_Repository>();
            services.AddScoped<IRND_YARNCONSUMPTION, SQLRND_YARNCONSUMPTION_Repository>();
            services.AddScoped<IBAS_BRANDINFO, SQLBAS_BRANDINFO_Repository>();
            services.AddScoped<ICOM_EX_FABSTYLE, SQLCOM_EX_FABSTYLE_Repository>();
            services.AddScoped<ICOM_EX_PIMASTER, SQLCOM_EX_PIMASTER_Repository>();
            services.AddScoped<IBAS_BUYER_BANK_MASTER, SQLBAS_BUYER_BANK_MASTER_Repository>();
            services.AddScoped<IBAS_TEAMINFO, SQLBAS_TEAMINFO_Repository>();
            services.AddScoped<IADM_DEPARTMENT, SQLADM_DEPARTMENT_Repository>();
            services.AddScoped<IBAS_BEN_BANK_MASTER, SQLBAS_BEN_BANK_MASTER_Repository>();
            services.AddScoped<ICOM_EX_LCINFO, SQLCOM_EX_LCINFO_Repository>();
            services.AddScoped<ICOM_EX_BOEXOPTION, SQLCOM_EX_BOEXOPTION_Repository>();
            services.AddScoped<IACC_EXPORT_DODETAILS, SQLACC_EXPORT_DODETAILS_Repository>();
            services.AddScoped<ICOM_EX_PI_DETAILS, SQLCOM_EX_PI_DETAILS_Repository>();
            services.AddScoped<ICOM_EX_LCDETAILS, SQLCOM_EX_LCDETAILS_Repository>();
            services.AddScoped<IACC_EXPORT_DOMASTER, SQLACC_EXPORT_DOMASTER_Repository>();
            services.AddScoped<ICOM_IMP_LCINFORMATION, SQLCOM_IMP_LCINFORMATION_Repository>();
            services.AddScoped<ICOM_IMP_LCDETAILS, SQLCOM_IMP_LCDETAILS_Repository>();
            services.AddScoped<IBAS_INSURANCEINFO, SQLBAS_INSURANCEINFO_Repository>();
            services.AddScoped<ICOM_EX_SCINFO, SQLCOM_EX_SCINFO_Repository>();
            services.AddScoped<ICOM_EX_SCDETAILS, SQLCOM_EX_SCDETAILS_Repository>();
            services.AddScoped<IACC_LOCAL_DOMASTER, SQLACC_LOCAL_DOMASTER_Repository>();
            services.AddScoped<IACC_LOCAL_DODETAILS, SQLACC_LOCAL_DODETAILS_Repository>();
            services.AddScoped<IBAS_TRANSPORTINFO, SQLBAS_TRANSPORTINFO_Repository>();
            services.AddScoped<ICOM_IMP_INVOICEINFO, SQLCOM_IMP_INVOICEINFO_Repository>();
            services.AddScoped<ICOM_IMP_INVDETAILS, SQLCOM_IMP_INVDETAILS_Repository>();
            services.AddScoped<IRND_FINISHTYPE, SQLRND_FINISHTYPE_Repository>();
            services.AddScoped<ICOM_EX_INVOICEMASTER, SQLCOM_EX_INVOICEMASTER_Repository>();
            services.AddScoped<IRND_WEAVE, SQLRND_WEAVE_Repository>();
            services.AddScoped<IRND_FINISHMC, SQLRND_FINISHMC_Repository>();
            services.AddScoped<ICOM_TENOR, SQLCOM_TENOR_Repository>();
            services.AddScoped<ICOM_TRADE_TERMS, SQLCOM_TRADE_TERMS_Repository>();
            services.AddScoped<ICOM_EX_INVDETAILS, SQLCOM_EX_INVDETAILS_Repository>();
            services.AddScoped<ICOM_EX_CASHINFO, SQLCOM_EX_CASHINFO_Repository>();
            services.AddScoped<ICOM_EX_GSPINFO, SQLCOM_EX_GSPINFO_Repository>();
            services.AddScoped<IACC_EXPORT_REALIZATION, SQLACC_EXPORT_REALIZATION_Repository>();
            services.AddScoped<ICOM_IMP_LCTYPE, SQLCOM_IMP_LCTYPE_Repository>();
            services.AddScoped<ICOS_FIXEDCOST, SQLCOS_FIXEDCOST_Repository>();
            services.AddScoped<ICOS_STANDARD_CONS, SQLCOS_STANDARD_CONS_Repository>();
            services.AddScoped<ICOS_PRECOSTING_MASTER, SQLCOS_PRECOSTING_MASTER_Repository>();
            services.AddScoped<ICOS_PRECOSTING_DETAILS, SQLCOS_PRECOSTING_DETAILS_Repository>();
            services.AddScoped<ICOS_CERTIFICATION_COST, SQLCOS_CERTIFICATION_COST_Repository>();
            services.AddScoped<IRequestFiles, RequestFilesRepository>();
            services.AddScoped<IF_BAS_DESIGNATION, SQLF_BAS_DESIGNATION_Reporsitory>();
            services.AddScoped<IF_BAS_DEPARTMENT, SQLF_BAS_DEPARTMENT_Repository>();
            services.AddScoped<IF_BAS_SECTION, SQLF_BAS_SECTION_Repository>();
            services.AddScoped<IF_BAS_SUBSECTION, SQLF_BAS_SUBSECTION_Repository>();
            services.AddScoped<IF_HR_EMP_FAMILYDETAILS, SQLF_HR_EMP_FAMILYDETAILS_Repository>();
            services.AddScoped<IF_HR_EMP_OFFICIALINFO, SQLF_HR_EMP_OFFICIALINFO_Repository>();
            services.AddScoped<IF_HR_EMP_EDUCATION, SQLF_HR_EMP_EDUCATION_Repository>();
            services.AddScoped<IF_HR_EMP_SALARYSETUP, SQLF_HR_EMP_SALARYSETUP_Repository>();
            services.AddScoped<IF_HRD_EMPLOYEE, SQLF_HRD_EMPLOYEE_Repository>();
            services.AddScoped<IF_HR_BLOOD_GROUP, SQLF_HR_BLOOD_GROUP_Repository>();
            services.AddScoped<IDISTRICTS, SQLDISTRICTS_Repository>();
            services.AddScoped<IUPOZILAS, SQLUPOZILAS_Repository>();
            services.AddScoped<IMKT_SDRF_INFO, SQLMKT_SDRF_INFO_Repository>();
            services.AddScoped<IMKT_TEAM, SQLMKT_TEAM_Repository>();
            services.AddScoped<IMKT_SUPPLIER, SQLMKT_SUPPLIER_Repostiory>();
            services.AddScoped<IMKT_FACTORY, SQLMKT_FACTORY_Repository>();
            services.AddScoped<IMKT_DEV_TYPE, SQLMKT_DEV_TYPE_Repository>();
            services.AddScoped<IYARNFOR, SQLYARNFOR_Repository>();
            services.AddScoped<IRND_SAMPLE_INFO_DYEING, SQLRND_SAMPLE_INFO_DYEING_Repository>();
            services.AddScoped<ICOUNTRIES, SQLCOUNTRIES_Repository>();
            services.AddScoped<IRND_SAMPLE_INFO_DETAILS, SQLRND_SAMPLE_INFO_DETAILS_Repository>();
            services.AddScoped<ICOM_IMP_CSINFO, SQLCOM_IMP_CSINFO_Repository>();
            services.AddScoped<ICOM_IMP_CSITEM_DETAILS, SQLCOM_IMP_CSITEM_DETAILS_Repository>();
            services.AddScoped<ICOM_IMP_CSRAT_DETAILS, SQLCOM_IMP_CSRAT_DETAILS_Repository>();
            services.AddScoped<ICOM_IMP_DEL_STATUS, SQLCOM_IMP_DEL_STATUS_Repository>();
            services.AddScoped<IRND_FABTEST_GREY, SQLRND_FABTEST_GREY_Repository>();
            services.AddScoped<IRND_FABTEST_SAMPLE, SQLRND_FABTEST_SAMPLE_Repository>();
            services.AddScoped<IRND_FABTEST_BULK, SQLRND_FABTEST_BULK_Repository>();
            services.AddScoped<IRND_FABTEST_SAMPLE_BULK, SQLRND_FABTEST_SAMPLE_BULK_Repository>();
            services.AddScoped<IRND_SAMPLEINFO_FINISHING, SQLRND_SAMPLEINFO_FINISHING_Repository>();
            services.AddScoped<IRND_SAMPLE_INFO_WEAVING, SQLRND_SAMPLE_INFO_WEAVING_Repository>();
            services.AddScoped<IRND_SAMPLE_INFO_WEAVING_DETAILS, SQLRND_SAMPLE_INFO_WEAVING_DETAILS_Repository>();
            services.AddScoped<IRND_ANALYSIS_SHEET, SQLRND_ANALYSIS_SHEET_Repository>();
            services.AddScoped<IRND_ANALYSIS_SHEET_DETAILS, SQLRND_ANALYSIS_SHEET_DETAILS_Repository>();
            services.AddScoped<IPL_SAMPLE_PROG_SETUP, SQLPL_SAMPLE_PROG_SETUP_Repository>();
            services.AddScoped<IRND_PURCHASE_REQUISITION_MASTER, SQLRND_PURCHASE_REQUISITION_MASTER>();
            services.AddScoped<IF_YS_INDENT_DETAILS, SQLF_YS_INDENT_DETAILS_Repository>();
            services.AddScoped<IF_YS_INDENT_MASTER, SQLF_YS_INDENT_MASTER_Repository>();
            services.AddScoped<IRND_PRODUCTION_ORDER, SQLRND_PRODUCTION_ORDER_Repository>();
            services.AddScoped<IRND_MSTR_ROLL, SQLRND_MSTR_ROLL_Repository>();
            services.AddScoped<IPL_ORDERWISE_LOTINFO, SQLPL_ORDERWISE_LOTINFO_Repository>();
            services.AddScoped<IPDL_EMAIL_SENDER, SQLPDL_EMAIL_SENDER_Repository>();
            services.AddScoped<IF_YS_YARN_RECEIVE_MASTER, SQLF_YS_YARN_RECEIVE_MASTER_Repository>();
            services.AddScoped<IF_YS_YARN_RECEIVE_MASTER_S, SQLF_YS_YARN_RECEIVE_MASTER_S_Repository>();
            services.AddScoped<IF_YS_YARN_RECEIVE_DETAILS, SQLF_YS_YARN_RECEIVE_DETAILS_Repository>();
            services.AddScoped<IF_YS_YARN_RECEIVE_DETAILS_S, SQLF_YS_YARN_RECEIVE_DETAILS_S_Repository>();
            services.AddScoped<IF_BAS_RECEIVE_TYPE, SQLF_BAS_RECEIVE_TYPE_Repository>();
            services.AddScoped<IF_YS_LOCATION, SQLF_YS_LOCATION_Repository>();
            services.AddScoped<IF_YS_LEDGER, SQLF_YS_LEDGER_Repository>();

            services.AddScoped<IRND_FABRICINFO_APPROVAL_DETAILS, SQLRND_FABRICINFO_APPROVAL_DETAILS_Repository>();

            services.AddScoped<IF_YARN_REQ_MASTER, SQLF_YARN_REQ_MASTER_Repository>();
            services.AddScoped<IF_YARN_REQ_DETAILS, SQLF_YARN_REQ_DETAILS_Repository>();
            services.AddScoped<IF_YARN_REQ_MASTER_S, SQLF_YARN_REQ_MASTER_S_Repository>();
            services.AddScoped<IF_YARN_REQ_DETAILS_S, SQLF_YARN_REQ_DETAILS_S_Repository>();
            services.AddScoped<IF_YS_SLUB_CODE, SQLF_YS_SLUB_CODE_Repository>();
            services.AddScoped<IF_YS_RAW_PER, SQLF_YS_RAW_PER_Repository>();

            //F_BAS_ASSET_LIST
            services.AddScoped<IF_BAS_ASSET_LIST, SQLF_BAS_ASSET_LIST_Repository>();

            //HR
            services.AddScoped<IF_BAS_HRD_DEPARTMENT, SQLF_BAS_HRD_DEPARTMENT_Repository>();
            services.AddScoped<IF_BAS_HRD_SECTION, SQLF_BAS_HRD_SECTION_Repository>();
            services.AddScoped<IF_BAS_HRD_SUB_SECTION, SQLF_BAS_HRD_SUB_SECTION_Repository>();
            services.AddScoped<IF_BAS_HRD_DESIGNATION, SQLF_BAS_HRD_DESIGNATION_Repository>();
            services.AddScoped<IF_BAS_HRD_LOCATION, SQLF_BAS_HRD_LOCATION_Repository>();
            services.AddScoped<IF_BAS_HRD_GRADE, SQLF_BAS_HRD_GRADE_Repository>();
            services.AddScoped<IF_BAS_HRD_EMP_TYPE, SQLF_BAS_HRD_EMP_TYPE_Repository>();
            services.AddScoped<IF_BAS_HRD_NATIONALITY, SQLF_BAS_HRD_NATIONALITY_Repository>();
            services.AddScoped<IF_BAS_HRD_OUT_REASON, SQLF_BAS_HRD_OUT_REASON_Repository>();
            services.AddScoped<IF_BAS_HRD_SHIFT, SQLF_BAS_HRD_SHIFT_Repository>();
            services.AddScoped<IF_HRD_EMP_EDU_DEGREE, SQLF_HRD_EMP_EDU_DEGREE_Repository>();
            services.AddScoped<IF_HRD_EDUCATION, SQLF_HRD_EDUCATION_Repository>();
            services.AddScoped<IF_HRD_EMP_SPOUSE, SQLF_HRD_EMP_SPOUSE_Repository>();

            //RND BOM
            services.AddScoped<IRND_BOM, SQLRND_BOM_Repository>();
            services.AddScoped<IRND_BOM_MATERIALS_DETAILS, SQLRND_BOM_MATERIALS_DETAILS_Repository>();

            //F_PR_INSPECTION_CUTPCS_TRANSFER
            services.AddScoped<IF_PR_INSPECTION_CUTPCS_TRANSFER, SQLF_PR_INSPECTION_CUTPCS_TRANSFER_Repository>();

            //F_PR_RECONE_MASTER, F_PR_RECONE_YARN_CONSUMPTION, F_PR_RECONE_YARN_DETAILS
            services.AddScoped<IF_PR_RECONE_MASTER, SQLF_PR_RECONE_MASTER_Repository>();
            services.AddScoped<IF_PR_RECONE_YARN_CONSUMPTION, SQLF_PR_RECONE_YARN_CONSUMPTION_Repository>();
            services.AddScoped<IF_PR_RECONE_YARN_DETAILS, SQLF_PR_RECONE_YARN_DETAILS_Repository>();

            //F_FS_FABRIC_LOADING_BILL
            services.AddScoped<IF_FS_FABRIC_LOADING_BILL, SQLF_FS_FABRIC_LOADING_BILL_Repository>();

            //Post Costing
            services.AddScoped<ICOS_POSTCOSTING_MASTER, SQLCOS_POSTCOSTING_MASTER_Repository>();
            services.AddScoped<ICOS_POSTCOSTING_YARNDETAILS, SQLCOS_POSTCOSTING_YARNDETAILS_Repository>();
            services.AddScoped<ICOS_POSTCOSTING_CHEMDETAILS, SQLCOS_POSTCOSTING_CHEMDETAILS_Repository>();

            //Proc Master & Detaills
            services.AddScoped<IPROC_WORKORDER_MASTER, SQLPROC_WORKORDER_MASTER_Repository>();
            services.AddScoped<IPROC_WORKORDER_DETAILS, SQLPROC_WORKORDER_DETAILS_Repository>();

            //F_PR_INSPECTION_FABRIC_D_MASTER, F_PR_INSPECTION_FABRIC_D_DETAILS
            services.AddScoped<IF_PR_INSPECTION_FABRIC_D_MASTER, SQLF_PR_INSPECTION_FABRIC_D_MASTER_Repository>();
            services.AddScoped<IF_PR_INSPECTION_FABRIC_D_DETAILS, SQLF_PR_INSPECTION_FABRIC_D_DETAILS_Repository>();

            services.AddScoped<IF_YS_YARN_ISSUE_MASTER, SQLF_YS_YARN_ISSUE_MASTER_Repository>();
            services.AddScoped<IF_YS_YARN_ISSUE_MASTER_S, SQLF_YS_YARN_ISSUE_MASTER_S_Repository>();
            services.AddScoped<IF_YS_YARN_ISSUE_DETAILS, SQLF_YS_YARN_ISSUE_DETAILS_Repository>();
            services.AddScoped<IF_YS_YARN_ISSUE_DETAILS_S, SQLF_YS_YARN_ISSUE_DETAILS_S_Repository>();
            services.AddScoped<IF_BAS_ISSUE_TYPE, SQLF_BAS_ISSUE_TYPE_Repository>();
            services.AddScoped<IF_YARN_TRANSACTION, SQLF_YARN_TRANSACTION_Repository>();
            services.AddScoped<IF_YARN_TRANSACTION_S, SQLF_YARN_TRANSACTION_S_Repository>();
            services.AddScoped<IF_YS_YARN_RECEIVE_REPORT_S, SQLF_YS_YARN_RECEIVE_REPORT_S_Repository>();
            services.AddScoped<IF_YARN_QC_APPROVE_S, SQLF_YARN_QC_APPROVE_S_Repository>();
            services.AddScoped<IPL_BULK_PROG_SETUP_M, SQLPL_BULK_PROG_SETUP_M_Repository>();
            services.AddScoped<IPL_BULK_PROG_SETUP_D, SQLPL_BULK_PROG_SETUP_D_Repository>();
            services.AddScoped<IPL_SAMPLE_PROG_SETUP_M, SQLPL_SAMPLE_PROG_SETUP_M_Repository>();
            services.AddScoped<IPL_SAMPLE_PROG_SETUP_D, SQLPL_SAMPLE_PROG_SETUP_D_Repository>();

            services.AddScoped<IPL_BULK_PROG_YARN_D, SQLPL_BULK_PROG_YARN_D_Repository>();
            services.AddScoped<IPL_PRODUCTION_PLAN_MASTER, SQLPL_PRODUCTION_PLAN_MASTER_Repository>();
            services.AddScoped<IPL_PRODUCTION_PLAN_DETAILS, SQLPL_PRODUCTION_PLAN_DETAILS_Repository>();
            services.AddScoped<IPL_PRODUCTION_SETDISTRIBUTION, SQLPL_PRODUCTION_SETDISTRIBUTION_Repository>();
            services.AddScoped<IPL_PRODUCTION_SO_DETAILS, SQLPL_PRODUCTION_SO_DETAILS_Repository>();
            services.AddScoped<IPL_DYEING_MACHINE_TYPE, SQLPL_DYEING_MACHINE_TYPE_Repository>();

            services.AddScoped<IAGEGROUP, SQLAGEGROUP_Repository>();
            services.AddScoped<IAGEGROUPRNDFABRICS, SQLAGEGROUPRNDFABRICS_Repository>();
            services.AddScoped<ISEGMENTOTHERSIMILARNAME, SQLSEGMENTOTHERSIMILARNAME_Repository>();
            services.AddScoped<ISEGMENTOTHERSIMILARRNDFABRICS, SQLSEGMENTOTHERSIMILARRNDFABRICS_Repository>();
            services.AddScoped<ISEGMENTSEASON, SQLSEGMENTSEASON_Repository>();
            services.AddScoped<ISEGMENTSEASONRNDFABRICS, SQLSEGMENTSEASONRNDFABRICS_Repository>();
            services.AddScoped<ITARGETCHARACTER, SQLTARGETCHARACTER_Repository>();
            services.AddScoped<ITARGETCHARACTERRNDFABRICS, SQLTARGETCHARACTERRNDFABRICS_Repository>();
            services.AddScoped<ITARGETFITSTYLE, SQLTARGETFITSTYLE_Repository>();
            services.AddScoped<ITARGETFITSTYLERNDFABRICS, SQLTARGETFITSTYLERNDFABRICS_Repository>();
            services.AddScoped<ITARGETGENDER, SQLTARGETGENDER_Repository>();
            services.AddScoped<ITARGETGENDERRNDFABRICS, SQLTARGETGENDERRNDFABRICS_Repository>();
            services.AddScoped<ITARGETPRICESEGMENT, SQLTARGETPRICESEGMENT_Repository>();
            services.AddScoped<ITARGETPRICESEGMENTRNDFABRICS, SQLTARGETPRICESEGMENTRNDFABRICS_Repository>();
            services.AddScoped<ISEGMENTCOMSEGMENT, SQLSEGMENTCOMSEGMENT_Repository>();
            services.AddScoped<ISEGMENTCOMSEGMENTRNDFABRICS, SQLSEGMENTCOMSEGMENTRNDFABRICS_Repository>();
            services.AddScoped<IF_YS_YARN_RECEIVE_REPORT, SQLF_YS_YARN_RECEIVE_REPORT_Repository>();

            services.AddScoped<IF_CHEM_PURCHASE_REQUISITION_MASTER, SQLF_CHEM_PURCHASE_REQUISITION_MASTER_Repository>();
            services.AddScoped<IF_CHEM_STORE_INDENTMASTER, SQLF_CHEM_STORE_INDENTMASTER_Repositiry>();
            services.AddScoped<IF_CHEM_STORE_INDENTDETAILS, SQLF_CHEM_STORE_INDENTDETAILS_Repository>();
            services.AddScoped<IF_CHEM_STORE_PRODUCTINFO, SQLF_CHEM_STORE_PRODUCTINFO_Repository>();

            services.AddScoped<IF_YARN_QC_APPROVE, SQLF_YARN_QC_APPROVE_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_ROPE_MASTER, SQLF_PR_WARPING_PROCESS_ROPE_MASTER_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_ROPE_DETAILS, SQLF_PR_WARPING_PROCESS_ROPE_DETAILS_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS, SQLF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS, SQLF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS_Repository>();
            services.AddScoped<IF_PR_WARPING_MACHINE, SQLF_PR_WARPING_MACHINE_Repository>();

            services.AddScoped<IF_PR_WARPING_PROCESS_DW_MASTER, SQLF_PR_WARPING_PROCESS_DW_MASTER_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_DW_DETAILS, SQLF_PR_WARPING_PROCESS_DW_DETAILS_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS, SQLF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS_Repository>();

            services.AddScoped<IF_CHEM_STORE_RECEIVE_MASTER, SQLF_CHEM_STORE_RECEIVE_MASTER_Repository>();
            services.AddScoped<IF_CHEM_STORE_RECEIVE_DETAILS, SQLF_CHEM_STORE_RECEIVE_DETAILS_Repository>();
            services.AddScoped<ICOM_IMP_CNFINFO, SQLCOM_IMP_CNFINFO_Repository>();
            services.AddScoped<IF_CHEM_TRANSECTION, SQLF_CHEM_TRANSECTION_Repository>();

            //F_PR_WEAVING_WORKLOAD_EFFICIENCYLOSS
            services.AddScoped<IF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS, SQLF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS_Repository>();
            services.AddScoped<IF_HR_SHIFT_INFO, SQLF_HR_SHIFT_INFO_Repository>();
            //F_PR_WEAVING_PRODUCTION
            services.AddScoped<IF_PR_WEAVING_PRODUCTION, SQLF_PR_WEAVING_PRODUCTION_Repository>();
            services.AddScoped<ILOOM_TYPE, SQLLOOM_TYPE_Repository>();

            //ECRU
            services.AddScoped<IF_PR_WARPING_PROCESS_ECRU_MASTER, SQLF_PR_WARPING_PROCESS_ECRU_MASTER_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_ECRU_DETAILS, SQLF_PR_WARPING_PROCESS_ECRU_DETAILS_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS,
                SQLF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS__Repository>();

            //SW
            services.AddScoped<IF_PR_WARPING_PROCESS_SW_MASTER, SQLF_PR_WARPING_PROCESS_SW_MASTER_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_SW_DETAILS, SQLF_PR_WARPING_PROCESS_SW_DETAILS_Repository>();
            services.AddScoped<IF_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS, SQLF_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS__Repository>();

            //Dyeing(Rope)
            services.AddScoped<IF_DYEING_PROCESS_ROPE_MASTER, SQlF_DYEING_PROCESS_ROPE_MASTER_Repository>();
            services.AddScoped<IF_DYEING_PROCESS_ROPE_DETAILS, SQLF_DYEING_PROCESS_ROPE_DETAILS_Repository>();
            services.AddScoped<IF_DYEING_PROCESS_ROPE_CHEM, SQLF_DYEING_PROCESS_ROPE_CHEM_Repository>();
            //Dyeing(Slasher)

            services.AddScoped<IF_PR_SLASHER_DYEING_MASTER, SQLF_PR_SLASHER_DYEING_MASTER_Repository>();
            services.AddScoped<IF_PR_SLASHER_DYEING_DETAILS, SQLF_PR_SLASHER_DYEING_DETAILS_Repository>();
            services.AddScoped<IF_PR_SLASHER_CHEM_CONSM, SQLF_PR_SLASHER_CHEM_CONSM_Repository>();
            services.AddScoped<IF_PR_SLASHER_MACHINE_INFO, SQLF_PR_SLASHER_MACHINE_INFO_Repository>();

            services.AddScoped<IF_CS_CHEM_RECEIVE_REPORT, SQLF_CS_CHEM_RECEIVE_REPORT_Repository>();
            services.AddScoped<IF_CHEM_QC_APPROVE, SQLF_CHEM_QC_APPROVE_Repository>();
            services.AddScoped<IMKT_SWATCH_CARD, SQLMKT_SWATCH_CARD_Repository>();
            services.AddScoped<IF_CHEM_REQ_DETAILS, SQLF_CHEM_REQ_DETAILS_Repository>();
            services.AddScoped<IF_CHEM_REQ_MASTER, SQLF_CHEM_REQ_MASTER_Repository>();
            services.AddScoped<IF_PR_ROPE_INFO, SQLF_PR_ROPE_INFO_Repository>();
            services.AddScoped<IF_PR_ROPE_MACHINE_INFO, SQLF_PR_ROPE_MACHINE_INFO_Repository>();
            services.AddScoped<IF_PR_TUBE_INFO, SQLF_PR_TUBE_INFO_Repository>();
            services.AddScoped<IF_CHEM_ISSUE_MASTER, SQLF_CHEM_ISSUE_MASTER_Repository>();
            services.AddScoped<IF_CHEM_ISSUE_DETAILS, SQLF_CHEM_ISSUE_DETAILS_Repository>();
            services.AddScoped<IF_BAS_UNITS, SQLF_BAS_UNITS_Repository>();
            services.AddScoped<IF_LCB_PRODUCTION_ROPE_MASTER, SQLF_LCB_PRODUCTION_ROPE_MASTER_Repository>();
            services.AddScoped<IF_LCB_PRODUCTION_ROPE_DETAILS, SQLF_LCB_PRODUCTION_ROPE_DETAILS_Repository>();
            services.AddScoped<IF_LCB_MACHINE, SQLF_LCB_MACHINE_Repository>();
            services.AddScoped<IF_LCB_BEAM, SQLF_LCB_BEAM_Repository>();
            services.AddScoped<IF_LCB_PRODUCTION_ROPE_PROCESS_INFO, SQLF_LCB_PRODUCTION_ROPE_PROCESS_INFO_Repository>();
            services.AddScoped<IF_PR_SIZING_PROCESS_ROPE_MASTER, SQLF_PR_SIZING_PROCESS_ROPE_MASTER_Repository>();
            services.AddScoped<IF_PR_SIZING_PROCESS_ROPE_DETAILS, SQLF_PR_SIZING_PROCESS_ROPE_DETAILS_Repository>();
            services.AddScoped<IF_PR_SIZING_PROCESS_ROPE_CHEM, SQLF_PR_SIZING_PROCESS_ROPE_CHEM_Repository>();
            services.AddScoped<IF_SIZING_MACHINE, SQLF_SIZING_MACHINE_Repository>();
            services.AddScoped<IF_WEAVING_BEAM, SQLF_WEAVING_BEAM_Repository>();
            services.AddScoped<IMESSAGE, SQLMESSAGE_Repository>();
            services.AddScoped<IF_CHEM_TYPE, SQLF_CHEM_TYPE_Repository>();
            services.AddScoped<IBAS_YARN_CATEGORY, SQLBAS_YARN_CATEGORY_Repository>();
            services.AddScoped<IBAS_YARN_COUNT_LOT_INFO, SQLBAS_YARN_COUNT_LOT_INFO_Repository>();

            //F_QA_FIRST_MTR_ANALYSIS_M, F_QA_FIRST_MTR_ANALYSIS_D
            services.AddScoped<IF_QA_FIRST_MTR_ANALYSIS_M, SQLF_QA_FIRST_MTR_ANALYSIS_M_Repository>();
            services.AddScoped<IF_QA_FIRST_MTR_ANALYSIS_D, SQLF_QA_FIRST_MTR_ANALYSIS_D_Repository>();


            // Weaving Production

            // Receive
            services.AddScoped<IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS, SQLF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS_Repository>();
            services.AddScoped<IF_PR_WEAVING_BEAM_RECEIVING, SQLF_PR_WEAVING_BEAM_RECEIVING_Repository>();

            // Bulk
            services.AddScoped<IF_PR_WEAVING_PROCESS_MASTER_B, SQLF_PR_WEAVING_PROCESS_MASTER_B_Repository>();
            services.AddScoped<IF_PR_WEAVING_PROCESS_BEAM_DETAILS_B, SQLF_PR_WEAVING_PROCESS_BEAM_DETAILS_B_Repository>();
            services.AddScoped<IF_PR_WEAVING_PROCESS_DETAILS_B, SQLF_PR_WEAVING_PROCESS_DETAILS_B_Repository>();
            services.AddScoped<IF_PR_WEAVING_PROCESS_DETAILS_B, SQLF_PR_WEAVING_PROCESS_DETAILS_B_Repository>();

            // Sample
            services.AddScoped<IF_PR_WEAVING_PROCESS_MASTER_S, SQLF_PR_WEAVING_PROCESS_MASTER_S_Repository>();
            services.AddScoped<IF_PR_WEAVING_PROCESS_BEAM_DETAILS_S, SQLF_PR_WEAVING_PROCESS_BEAM_DETAILS_S_Repository>();
            services.AddScoped<IF_PR_WEAVING_PROCESS_DETAILS_S, SQLF_PR_WEAVING_PROCESS_DETAILS_S_Repository>();

            services.AddScoped<IMESSAGE_INDIVIDUAL, SQLMESSAGE_INDIVIDUAL_Repository>();

            // Factory Sample Garments Receive
            services.AddScoped<IF_SAMPLE_GARMENT_RCV_D, SQLF_SAMPLE_GARMENT_RCV_D_Repository>();
            services.AddScoped<IF_SAMPLE_GARMENT_RCV_M, SQLF_SAMPLE_GARMENT_RCV_M_Repository>();
            services.AddScoped<IF_SAMPLE_ITEM_DETAILS, SQLF_SAMPLE_ITEM_DETAILS_Repository>();
            services.AddScoped<IF_SAMPLE_LOCATION, SQLF_SAMPLE_LOCATION_Repository>();

            // Factory Sample Garments Gate Pass
            services.AddScoped<IF_SAMPLE_DESPATCH_MASTER, SQLF_SAMPLE_DESPATCH_MASTER_Repository>();
            services.AddScoped<IF_SAMPLE_DESPATCH_DETAILS, SQLF_SAMPLE_DESPATCH_DETAILS_Repository>();
            services.AddScoped<IGATEPASS_TYPE, SQLGATEPASS_TYPE_Repository>();
            services.AddScoped<IF_BAS_DRIVERINFO, SQLF_BAS_DRIVERINFO_Repository>();
            services.AddScoped<IF_BAS_VEHICLE_INFO, SQLF_BAS_VEHICLE_INFO_Repository>();
            services.AddScoped<IVEHICLE_TYPE, SQLVEHICLE_TYPE_Repository>();


            // Head Office Sample Garments Receive
            services.AddScoped<IH_SAMPLE_RECEIVING_M, SQLH_SAMPLE_RECEIVING_M_Repository>();
            services.AddScoped<IH_SAMPLE_RECEIVING_D, SQLH_SAMPLE_RECEIVING_D_Repository>();

            // Head Office Sample Garments Dispatch
            services.AddScoped<IH_SAMPLE_DESPATCH_M, SQLH_SAMPLE_DESPATCH_M_Repository>();
            services.AddScoped<IH_SAMPLE_DESPATCH_D, SQLH_SAMPLE_DESPATCH_D_Repository>();
            services.AddScoped<IH_SAMPLE_PARTY, SQLH_SAMPLE_PARTY_Repository>();
            services.AddScoped<IH_SAMPLE_TEAM_DETAILS, SQLH_SAMPLE_TEAM_DETAILS_Repository>();

            // F_FS_WASTAGE
            services.AddScoped<IF_FS_WASTAGE_PARTY, SQLF_FS_WASTAGE_PARTY_Repository>();
            services.AddScoped<IF_FS_WASTAGE_ISSUE_M, SQLF_FS_WASTAGE_ISSUE_M_Repository>();
            services.AddScoped<IF_FS_WASTAGE_ISSUE_D, SQLF_FS_WASTAGE_ISSUE_D_Repository>();
            services.AddScoped<IF_FS_WASTAGE_RECEIVE_M, SQLF_FS_WASTAGE_RECEIVE_M_Repository>();
            services.AddScoped<IF_FS_WASTAGE_RECEIVE_D, SQLF_FS_WASTAGE_RECEIVE_D_Repository>();

            //Finishing Process
            services.AddScoped<IF_PR_FINISHING_PROCESS_MASTER, SQLF_PR_FINISHING_PROCESS_MASTER_Repository>();
            services.AddScoped<IF_PR_FIN_TROLLY, SQLF_PR_FIN_TROLLY_Repository>();
            services.AddScoped<IF_PR_FINISHING_FAB_PROCESS, SQLF_PR_FINISHING_FAB_PROCESS_Repository>();
            services.AddScoped<IF_PR_PROCESS_MACHINEINFO, SQLF_PR_PROCESS_MACHINEINFO_Repository>();
            services.AddScoped<IF_PR_PROCESS_TYPE_INFO, SQLF_PR_PROCESS_TYPE_INFO_Repository>();
            services.AddScoped<IF_PR_FINISHING_FNPROCESS, SQLF_PR_FINISHING_FNPROCESS_Repository>();
            services.AddScoped<IF_PR_FINISHING_BEAM_RECEIVE, SQLF_PR_FINISHING_BEAM_RECEIVE_Repository>();
            services.AddScoped<IF_PR_FN_PROCESS_TYPEINFO, SQLF_PR_FN_PROCESS_TYPEINFO_Repository>();
            services.AddScoped<IF_PR_FN_CHEMICAL_CONSUMPTION, SQLF_PR_FN_CHEMICAL_CONSUMPTION_Repository>();
            services.AddScoped<IF_PR_FN_MACHINE_INFO, SQLF_PR_FN_MACHINE_INFO_Repository>();
            services.AddScoped<IF_PR_FINISHING_MACHINE_PREPARATION, SQLF_PR_FINISHING_MACHINE_PREPARATION_Repository>();
            services.AddScoped<IF_PR_FINIGHING_DOFF_FOR_MACHINE, SQLF_PR_FINIGHING_DOFF_FOR_MACHINE_Repository>();

            //General Store Old
            services.AddScoped<IF_GS_ITEMCATEGORY, SQLF_GS_ITEMCATEGORY_Repository>();
            services.AddScoped<IF_GS_ITEMSUB_CATEGORY, SQLF_GS_ITEMSUB_CATEGORY_Repository>();
            services.AddScoped<IF_GS_PRODUCT_INFORMATION, SQLF_GS_PRODUCT_INFORMATION_Repository>();
            services.AddScoped<IF_GS_GATEPASS_INFORMATION_D, SQLF_GS_GATEPASS_INFORMATION_D_Repository>();
            services.AddScoped<IF_GS_GATEPASS_INFORMATION_M, SQLF_GS_GATEPASS_INFORMATION_M_Repository>();
            services.AddScoped<IF_GATEPASS_TYPE, SQLF_GATEPASS_TYPE_Repository>();

            services.AddScoped<IF_GS_RETURNABLE_GP_RCV_M, SQLF_GS_RETURNABLE_GP_RCV_M_Repository>();
            services.AddScoped<IF_GS_RETURNABLE_GP_RCV_D, SQLF_GS_RETURNABLE_GP_RCV_D_Repository>();

            //Inspection
            services.AddScoped<IF_PR_INSPECTION_BATCH, SQLF_PR_INSPECTION_BATCH_Repository>();
            services.AddScoped<IF_PR_INSPECTION_DEFECT_POINT, SQLF_PR_INSPECTION_DEFECT_POINT_Repository>();
            services.AddScoped<IF_PR_INSPECTION_DEFECTINFO, SQLF_PR_INSPECTION_DEFECTINFO_Repository>();
            services.AddScoped<IF_PR_INSPECTION_MACHINE, SQLF_PR_INSPECTION_MACHINE_Repository>();
            services.AddScoped<IF_PR_INSPECTION_PROCESS_DETAILS, SQLF_PR_INSPECTION_PROCESS_DETAILS_Repository>();
            services.AddScoped<IF_PR_INSPECTION_PROCESS_MASTER, SQLF_PR_INSPECTION_PROCESS_MASTER_Repository>();
            services.AddScoped<IF_PR_INSPECTION_WASTAGE_TRANSFER, SQLF_PR_INSPECTION_WASTAGE_TRANSFER_Repository>();
            services.AddScoped<IF_PR_INSPECTION_REJECTION_B, SQLF_PR_INSPECTION_REJECTION_B_Repository>();

            //Fabric Store
            services.AddScoped<IF_FS_FABRIC_RCV_MASTER, SQLF_FS_FABRIC_RCV_MASTER_Repository>();
            services.AddScoped<IF_FS_FABRIC_RCV_DETAILS, SQLF_FS_FABRIC_RCV_DETAILS_Repository>();
            services.AddScoped<IF_FS_LOCATION, SQLF_FS_LOCATION_Repository>();
            services.AddScoped<IF_FS_UP_DETAILS, SQLF_FS_UP_DETAILS_Repository>();
            services.AddScoped<IF_FS_UP_MASTER, SQLF_FS_UP_MASTER_Repository>();

            //Fabric Delivery Challan & Packing List
            services.AddScoped<IF_FS_DELIVERYCHALLAN_PACK_MASTER, SQLF_FS_DELIVERYCHALLAN_PACK_MASTER_Repository>();
            services.AddScoped<IF_FS_DELIVERYCHALLAN_PACK_DETAILS, SQLF_FS_DELIVERYCHALLAN_PACK_DETAILS_Repository>();

            //Fabric Clearance
            services.AddScoped<IF_FS_FABRIC_CLEARANCE_MASTER, SQLF_FS_FABRIC_CLEARANCE_MASTER_Repository>();
            services.AddScoped<IF_FS_FABRIC_CLEARANCE_DETAILS, SQLF_FS_FABRIC_CLEARANCE_DETAILS_Repository>();
            services.AddScoped<IF_FS_FABRIC_TYPE, SQLF_FS_FABRIC_TYPE_Repository>();
            services.AddScoped<IF_FS_FABRIC_CLEARENCE_2ND_BEAM, SQLF_FS_FABRIC_CLEARENCE_2ND_BEAM_Repository>();
            services.AddScoped<IF_FS_CLEARANCE_WASTAGE_TRANSFER, SQLF_FS_CLEARANCE_WASTAGE_TRANSFER_Repository>();
            services.AddScoped<IF_FS_CLEARANCE_MASTER_SAMPLE_ROLL, SQLF_FS_CLEARANCE_MASTER_SAMPLE_ROLL_Repository>();

            //Loom Setting Style Wise
            services.AddScoped<ILOOM_SETTING_STYLE_WISE_M, SQLLOOM_SETTING_STYLE_WISE_M_Repository>();
            services.AddScoped<ILOOM_SETTING_CHANNEL_INFO, SQLLOOM_SETTING_CHANNEL_INFO_Repository>();
            services.AddScoped<ILOOM_SETTINGS_FILTER_VALUE, SQLLOOM_SETTINGS_FILTER_VALUE_Repository>();

            //Loom Settings Sample Weaving
            services.AddScoped<ILOOM_SETTINGS_SAMPLE, SQLLOOM_SETTINGS_SAMPLE_Repository>();

            //General Store New
            services.AddScoped<IF_GEN_S_REQ_MASTER, SQLF_GEN_S_REQ_MASTER_Repository>();
            services.AddScoped<IF_GEN_S_REQ_DETAILS, SQLF_GEN_S_REQ_DETAILS_Repository>();
            services.AddScoped<IF_GEN_S_INDENTDETAILS, SQLF_GEN_S_INDENTDETAILS_Repository>();
            services.AddScoped<IF_GEN_S_INDENTMASTER, SQLF_GEN_S_INDENTMASTER_Repository>();
            services.AddScoped<IF_GEN_S_ISSUE_DETAILS, SQLF_GEN_S_ISSUE_DETAILS_Repository>();
            services.AddScoped<IF_GEN_S_ISSUE_MASTER, SQLF_GEN_S_ISSUE_MASTER_Repository>();
            services.AddScoped<IF_GEN_S_PURCHASE_REQUISITION_MASTER, SQLF_GEN_S_PURCHASE_REQUISITION_MASTER_Repository>();
            services.AddScoped<IF_GEN_S_RECEIVE_MASTER, SQLF_GEN_S_RECEIVE_MASTER_Repository>();
            services.AddScoped<IF_GEN_S_RECEIVE_DETAILS, SQLF_GEN_S_RECEIVE_DETAILS_Repository>();
            services.AddScoped<IF_GEN_S_QC_APPROVE, SQLF_GEN_S_QC_APPROVE_Repository>();
            services.AddScoped<IF_GEN_S_MRR, SQLF_GEN_S_MRR_Repository>();
            services.AddScoped<IF_GS_GATEPASS_RETURN_RCV_MASTER, SQLF_GS_GATEPASS_RETURN_RCV_MASTER_Repository>();
            services.AddScoped<IF_GS_GATEPASS_RETURN_RCV_DETAILS, SQLF_GS_GATEPASS_RETURN_RCV_DETAILS_Repository>();
            
            //FGS Wastage
            services.AddScoped<IF_GS_WASTAGE_PARTY, SQLF_GS_WASTAGE_PARTY_Repository>();
            services.AddScoped<IF_GS_WASTAGE_RECEIVE_M, SQLF_GS_WASTAGE_RECEIVE_M_Repository>();
            services.AddScoped<IF_GS_WASTAGE_RECEIVE_D, SQLF_GS_WASTAGE_RECEIVE_D_Repository>();
            services.AddScoped<IF_GS_WASTAGE_ISSUE_M, SQLF_GS_WASTAGE_ISSUE_M_Repository>();
            services.AddScoped<IF_GS_WASTAGE_ISSUE_D, SQLF_GS_WASTAGE_ISSUE_D_Repository>();

            //AdvDelivery Schedule

            services.AddScoped<ICOM_EX_ADV_DELIVERY_SCH_MASTER, SQLCOM_EX_ADV_DELIVERY_SCH_MASTER_Repository>();
            services.AddScoped<ICOM_EX_ADV_DELIVERY_SCH_DETAILS, SQLCOM_EX_ADV_DELIVERY_SCH_DETAILS_Repository>();


            //Account Loan Management
            services.AddScoped<IACC_LOAN_MANAGEMENT_M, SQLACC_LOAN_MANAGEMENT_M_Repository>();
            // Menu Master
            services.AddTransient<IMenuMaster, SQLMenuMaster_Repository>();
            services.AddTransient<IMenuMasterRoles, SQLMenuMasterRoles_Repository>();

            // Company Info
            services.AddScoped<ICOMPANY_INFO, SQLCOMPANY_INFO_Repository>();
            services.AddScoped<IUPAS, SQLUPAS_Repository>();

            // Identity User
            services.AddScoped<IAspNetUserTypes, SQLAspNetUserTypes_Repository>();

            // Yarn ~ From
            services.AddScoped<IYARNFROM, SQLYARNFROM_Repository>();

            // API
            services.AddScoped<IEmployee, SQLEmployee_Repository>();

            //MAILBOX
            services.AddScoped<IMAILBOX, SQLMAILBOX_Repository>();

            //wastage

            services.AddScoped<IF_WASTE_PRODUCTINFO, SQLF_WASTE_PRODUCTINFO_Repository>();

            //Yarn test information
            services.AddScoped<IF_QA_YARN_TEST_INFORMATION_COTTON, SQLF_QA_YARN_TEST_INFORMATION_COTTON_Repository>();
            services.AddScoped<IF_QA_YARN_TEST_INFORMATION_POLYESTER, SQLF_QA_YARN_TEST_INFORMATION_POLYESTER_Repository>();

            //Com Import Work Order
            services.AddScoped<ICOM_IMP_WORK_ORDER_MASTER, SQLCOM_IMP_WORK_ORDER_MASTER_Repository>();
            services.AddScoped<ICOM_IMP_WORK_ORDER_DETAILS, SQLCOM_IMP_WORK_ORDER_DETAILS_Repository>();

            //Wastage %
            services.AddScoped<ICOS_WASTAGE_PERCENTAGE, SQLCOS_WASTAGE_PERCENTAGE_Repository>();

            services.AddScoped<IBAS_SEASON, SQLBAS_SEASON_Repository>();
            services.AddScoped<IACC_PHYSICAL_INVENTORY_FAB, SQLACC_PHYSICAL_INVENTORY_FAB_Repository>();


            services.AddScoped<IF_YS_PARTY_INFO, SQLF_YS_PARTY_INFO_Repository>();

            services.AddScoped<IF_YS_GP_MASTER, SQLF_YS_GP_MASTER_Repository>();

            services.AddScoped<IF_YS_GP_DETAILS, SQLF_YS_GP_DETAILS_Repository>();

            services.AddScoped<IF_FS_FABRIC_RETURN_RECEIVE, SQLF_FS_FABRIC_RETURN_RECEIVE_Repository>();

            services.AddScoped<IF_PR_WEAVING_OS, SQLF_PR_WEAVING_OS_Repository>();

            services.AddScoped<IF_YS_YARN_RECEIVE_MASTER2, SQLF_YS_YARN_RECEIVE_MASTER2_Repository>();

            services.AddScoped<IF_YS_YARN_RECEIVE_DETAILS2, SQLF_YS_YARN_RECEIVE_DETAILS2_Repository>();


            // SignalR
            services.AddSignalR();

            // Implement Later core v2.1+
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(typeof(StoreActionFilter));
            //});

            // Resources
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("bn-BD"),
                        new CultureInfo("en-GB"),
                        new CultureInfo("ja-JP"),
                        //new CultureInfo("en-US")
                    };
                    options.DefaultRequestCulture = new RequestCulture("en-GB");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            // Other
            services.AddScoped<IDeleteFileFromFolder, DeleteFileFromFolderRepository>();
            services.AddScoped<IProcessUploadFile, ProcessUploadFileToFolderRepository>();

            // Pagination
            services.AddCloudscribePagination();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "117021812063-c75uer39qi8f7ru2o2tj2jisraorvhqd.apps.googleusercontent.com";
                    options.ClientSecret = "lo9H8Ej9ZiYCWMzWDhmQm3KP";
                });

            // Changes token lifespan of all token types
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                    o.TokenLifespan = TimeSpan.FromHours(5));

            // Changes token lifespan of just the Email Confirmation Token type
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                    o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddSingleton<DataProtectionPurposeStrings>();

            // Bind the email
            services.AddSingleton<IPDL_EMAIL_SENDER<bool>, DevEmailSender>();
        }
    }
}
