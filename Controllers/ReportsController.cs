using DenimERP.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IDataProtector _protector;

        public ReportsController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public IActionResult RDateWisePiInfoReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RWashCodewiseRollClearanceReport(int clid)
        {
            return View(model: clid);
        }

        [HttpGet]
        public IActionResult RFSGatePassReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RWorkOrderReport(string woId)
        {
            return View(model: woId);
        }

        [HttpGet]
        public IActionResult RWorkOrderPOReport(string woId)
        {
            return View(model: woId);
        }

        [HttpGet]
        public IActionResult RFSPackingListReport()
        {
            return View();
        }

        //Inspection Reports Starts
        [HttpGet]
        public IActionResult DailyInsProdReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RInspectionDailyProductionReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RDateWiseProductionInsReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RProductionSummaryInsReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ROrderWiseProductionInsReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RRollDispatchInsReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RShiftWiseProductionInsReport()
        {
            return View();
        }


        //Inspection Report End

        //Finishing Report Start

        [HttpGet]
        public IActionResult RFnChemicalConsumpReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RFnProductionReport()
        {
            return View();
        }

        //Clearance
        [HttpGet]
        public IActionResult RClearance2ndReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RClearanceMasterRollReport()
        {
            return View();
        }


        [HttpGet]
        public IActionResult RSetStatusReport()
        {
            return View();
        }

        //Sizing
        [HttpGet]
        public IActionResult RSizingDailyProductionReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RSizingMonthlyProductionReport()
        {
            return View();
        }

        //Warping
        [HttpGet]
        public IActionResult RDailyProduction_DirectWarping()
        {
            return View();
        }

        //Weaving
        [HttpGet]
        public IActionResult RDailyProduction_Weaving()
        {
            return View();
        }

        public IActionResult RLoomCard(int beamNo)
        {
            return View(model: beamNo);
        }
        public IActionResult RDoffCard(int doffId)
        {
            return View(model: doffId);
        }
        public IActionResult ROrderWiseReqIssue()
        {
            return View();
        }
        public IActionResult RLoomSettingsSample()
        {
            return View();
        }

        public IActionResult RWeavingMounting()
        {
            return View();
        }



        //QA
        [HttpGet]
        public IActionResult RQALabTestPolyester()
        {
            return View();
        }

        //Yarn
        [HttpGet]
        public IActionResult RYarnRcvReport()
        {
            return View();
        }

        [HttpGet]
        [Route("/Reports/RYarnIndentReport/")]
        [Route("/Reports/RYarnIndentReport/{indid}")]
        public IActionResult RYarnIndentReport(string indid)
        {
            return View(model: indid);
        }

        [HttpGet]
        public IActionResult RYarnStockReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RYarnIssueReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RDateWisePoInfoReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RPiInfoLcStatusReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RDateWiseLcRegisterReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RBankAccountPendingReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RBankAccountReceiveStatusReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RDateWiseDoSubNRcvReport()
        {
            return View();
        }

        //Fabric Store
        [HttpGet]
        public IActionResult RFabricDetailsReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RFabricStockReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RFabricDetailsMktReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RDeliveryChallan()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RGisReport(string style)
        {
            return View(model: style);
        }

        [HttpGet]
        public IActionResult RYarnRequisitionReport(string ysrid)
        {
            return View(model: ysrid);
        }

        [HttpGet]
        public IActionResult RYarnRequirmentReport(string ysrid)
        {
            return View(model: _protector.Unprotect(ysrid));
        }

        [HttpGet]
        public IActionResult RChemDataWiseStockReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RFDetailsReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RFDetailsMarketingReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RWeavingDeliveryShiftWiseReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RProductionOrderReport(string soNo)
        {
            return View(model: soNo);
        }

        [HttpGet]
        public IActionResult RChemRequirmentReport(string reqNo)
        {
            return View(model: reqNo);
        }

        [HttpGet]
        public IActionResult RFinishingProductionReport()
        {
            return View();
        }
        public IActionResult RWaitingForPartySubmissionReport()
        {
            return View();
        }
        public IActionResult RPendingPartyAcceptanceReport()
        {
            return View();
        }
        public IActionResult RPendingBankPaymentReport()
        {
            return View();
        }

        public IActionResult RPRC_Register()
        {
            return View();
        }
        public IActionResult QA_LabTest_Polyester()
        {
            return View();
        }
        public IActionResult QA_LabTest_Cotton()
        {
            return View();
        }

        public IActionResult Clearance_WastageTransferReport()
        {
            return View();
        }
        public IActionResult DailyInsPactionReport()
        {
            return View();
        }
        public IActionResult Ins_DailyWastageTranferReport()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/Commercial-Import-LC-Details")]
        public IActionResult Com_ImpLcDetailsReport()
        {
            return View();
        }

        public IActionResult RND_FabricInfoReport()
        {
            return View();
        }

        public IActionResult Ins_StyleWiseProductionReport()
        {
            return View();
        }
        public IActionResult RPT_DailyInspectionReport()
        {
            return View();
        }
        public IActionResult RPT_StyleWiseInsProdReport()
        {
            return View();
        }
        public IActionResult RPT_StyleWiseAnalysisReport()
        {
            return View();
        }

        public IActionResult RollStickerReportInsp()
        {
            return View();
        }

        public IActionResult StyleWiseAnalysisShiftReport_Ins()
        {
            return View();
        }
        public IActionResult DailyBulkRejectionreport()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/FabricReceive/Statement")]
        public IActionResult FabricReceiveStatement_Fab()
        {
            return View();
        }

        public IActionResult FabricTestReportBulk_QA()
        {
            return View();
        }
        public IActionResult FabricTestReportGrey_QA()
        {
            return View();
        }
        public IActionResult FabricTestReportSample_QA(int ltsId)
        {
            return View(model: ltsId);
        }

        public IActionResult BeamStockReport_Weav()
        {
            return View();
        }
        public IActionResult LoomSettingsReport_Weav(string Pstyle)
        {
            return View(model: Pstyle);
        }

        public IActionResult DailyFabricDeliveryReport_FS()
        {
            return View();
        }

        public IActionResult ChallanWiseDeliveryReport_FS(string P_Date)
        {
            return View(model: P_Date);
        }

        public IActionResult ShadeWiseSummeryReport_Clearance(string P_Date)
        {
            return View(model: P_Date);
        }

        public IActionResult SizingBeamCard(string Beam_No)
        {
            return View(model: Beam_No);
        }

        public IActionResult DateWiseDataReport_Clearance()
        {
            return View();
        }
        public IActionResult rptSizingDelivery(string ProgNo)
        {
            return View(model: ProgNo);
        }

        public IActionResult rptSlasherDelivery(string ProgNo)
        {
            return View(model: ProgNo);
        }

        public IActionResult rptDespatchCode_FS()
        {
            return View();
        }

        public IActionResult rptFabricRoll_Clearance()
        {
            return View();
        }

        public IActionResult OperatorWiseProductionRpt_Ins()
        {
            return View();
        }

        public IActionResult DailyLoomProductionRpt_Weav()
        {
            return View();
        }

        public IActionResult DateWiseFinishingConsumption()
        {
            return View();
        }

        //Ecru Delivery SHeet
        public IActionResult DailyProductionEcru(string setNo)
        {
            return View(setNo);
        }

        [HttpGet]
        [Route("Reports/Customs/GetReports")]
        public IActionResult RCustomsInvoiceDetails()
        {
            return View();
        }

        public IActionResult DateWiseCustomsReport()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/Planning/Set-Order-Wise")]
        public IActionResult SetRecoveryReportForAll()
        {
            return View();
        }

        public IActionResult YarnDateWiseIssue()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/Import/Register")]
        public IActionResult ImportregisterReport()
        {
            return View();
        }

        public IActionResult FabricRollRcvPending()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/FabricStore/PI-Wise-Balance")]
        public IActionResult FabricPIWiseBalance()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/FabricStore/DO-Wise-Balance")]
        public IActionResult FabricDOWiseBalance()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/FabricStore/PackingList")]
        public IActionResult FabricPackingListAll()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/FabricStore/PackingList-Summery")]
        public IActionResult FabricPackingListSummery()
        {
            return View();
        }

        public IActionResult SectionWiseIssue_Chemical()
        {
            return View();
        }

        public IActionResult SectionWiseIssueSummery_Chemical()
        {
            return View();
        }

        public IActionResult InternalDeliveryChallanRpt_FS()
        {
            return View();
        }

        public IActionResult DateWiseIssueRpt_GS()
        {
            return View();
        }

        public IActionResult IndentBalanceRpt_GS()
        {
            return View();
        }

        public IActionResult DateWiseReceivingRpt_GS()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/FabricStore/Date-Wise")]
        public IActionResult RptFabricStockDateWise()
        {
            return View();
        }

        [HttpGet]
        [Route("DyeingReport/DyeingReport")]
        public IActionResult DyeingReport()
        {
            return View();
        }

        [HttpGet]
        [Route("LCBReport/LCBReport")]
        public IActionResult LcbReport()
        {
            return View();
        }

        public IActionResult RollToRollRcvRpt_FS()
        {
            return View();
        }

        public IActionResult QCStatusReport_YS()
        {
            return View();
        }

        public IActionResult IndentHistoryReport_YS()
        {
            return View();
        }

        public IActionResult WorderWiseHistoryReport_YS()
        {
            return View();
        }

        public IActionResult FirstMeterAnalysisReport_QA()
        {
            return View();
        }

        public IActionResult ForeignPIReport_ComEx()
        {
            return View();
        }

        public IActionResult FabricLoadingBillRpt_FS()
        {
            return View();
        }

        public IActionResult ShadeGroupPendingRpt_Clearance()
        {
            return View();
        }

        public IActionResult AgingRpt_FS()
        {
            return View();
        }

        public IActionResult FabricTestRptSample_QA()
        {
            return View();
        }

        //[HttpGet]
        ////[Route("Reports/Accounts/Bill-of-Exe/Mushak")]
        //public IActionResult BillOfExeForAccounts()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult AccountMushakRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricDeliveryStatement_DO()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricRollStockRptExcelFormet()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricInternalIssueRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricgatePassLocalRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricRollStockAgeRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrderRecoveryRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AdvancedFabricDeliveryStatusRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ExpPiDetailsContactRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LcSearchingInPIRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult YarnReqWiseIssueRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricDetailsSheetlabRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult YarnCostingRpt()
        {
            return View();
        }

        [HttpGet]
        [Route("/Reports/ChemicalIndentRpt/{indNo?}")]
        public IActionResult ChemicalIndentRpt(string indNo)
        {
            return View(model: indNo);
        }

        [HttpGet]
        public IActionResult DailyStyleWiseProductionRpt()
        {
            return View();
        }

        [HttpGet]
        [Route("/Reports/ChemicalPurReqRpt/{indSlid?}")]
        public IActionResult ChemicalPurReqRpt(string indSlid)
        {
            return View(model: indSlid);
        }

        [HttpGet]
        public IActionResult ChemicalDateWiseRcvRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChemicalMrrRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FabricStockA2()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RndStickerPrint()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LocalDOInformation(int transId)
        {
            return View(model: transId);
        }


        [HttpGet]
        [Route("/Reports/SalesContactInfoRpt/{scId?}")]
        public IActionResult SalesContactInfoRpt(int scId)
        {
            return View(model: scId);
        }


        [HttpGet]
        public IActionResult FabricStoreBinCardRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OrderWiseProductionHistoryRpt_Planning()
        {
            return View();
        }

        [HttpGet] 
        public IActionResult SampleReceiveStatementRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult IndividualSetRecoveryRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DateWiseDyeingConsumptionRpt()
        {
            return View();
        }



        [HttpGet]
        public IActionResult DateWiseSizingConsumptionRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DateWiseFinishConsumptionSummeryRpt()
        {
            return View();
        }
        //Shipment Date Follow Up List
        [HttpGet]
        public IActionResult ShipmentFollowUpListRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult IndividualRollInfoRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DocumentsRegisterRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InvoiceWisePrcCheckingRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DailyDocumentsRcvSubListRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SWSizingChemicalConsumptionRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PIWiseSOListRptt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult FabricReceivedStatementExportRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ClearancePendingRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SectionWiseCutPcsRpt()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DepartmentWiseBulkRejectionSummeryRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InspectionCutPcsRollDespatchSheet()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InspectionCutPcsRollDespatchSummeryRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BTMAGSPPendingInvoiceListRpt()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult FabricDetailsCostingRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InspectionRollScanPendingRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InspRollToRollScanStatusRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RollDispatchSheetScanRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StyleWiseProductionScanRpt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InvoiceWiseRpt()
        {
            return View();
        }

        [HttpGet]
        [Route("Reports/AdvanceDeliveryReport")]
        public IActionResult RComExAdvDeliverySch()
        {
            return View();

        }

        [HttpGet]
        public IActionResult OrderWiseYarnPriceRpt()
        {
            return View();

        }

        [HttpGet]
        public IActionResult BuyerWiseStatusRpt()
        {
            return View();

        }

        [HttpGet]
        public IActionResult CommercialInvoiceRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult YearlyProductionSummery()
        {
            return View();

        }
        [HttpGet]
        public IActionResult RequiredRawMaterials()
        {
            return View();

        }

        [HttpGet]
        public IActionResult DeliveryWisePostCostingRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult ChallanWiseSearchingRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult DailySampleFabricStockRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult MktPersonWisePoRcvRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult FabricPhysicalInventoryRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult TeamWiseDeliveryRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult RollInformationDummyRpt()
        {
            return View();

        }
        [HttpGet]
        public IActionResult RptBOMvsProduction_Rope()
        {
            return View();

        }
        [HttpGet]
        public IActionResult RptBOMvsProduction_Sizing()
        {
            return View();

        }
        [HttpGet]
        public IActionResult RptMonthlyWarpingProduction()
        {
            return View();

        }

        [HttpGet]
        public IActionResult RptFabricPhysicalInventory()
        {
            return View();

        }
        [HttpGet]
        public IActionResult RptFDSvsPostCosting_Variance()
        {
            return View();

        }

        public IActionResult RptBomVsProduction()
        {
            return View();

        }

        public IActionResult InspectionAudit()
        {
            return View();

        }

        public IActionResult PartyWiseComImp()
        {
            return View();

        }

        public IActionResult BankWiseComImp()
        {
            return View();

        }
        public IActionResult PendingShipmentComImp()
        {
            return View();

        }

        public IActionResult BillOfEntryComImp()
        {
            return View();

        }

        public IActionResult BillSubmitComImp()
        {
            return View();

        }
        public IActionResult RptBTM_GSPCertificateRegister()
        {
            return View();

        }


    }
}
