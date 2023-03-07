using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class FSizingProductionRopeViewModel
    {
        public FSizingProductionRopeViewModel()
        {
            FPrSizingProcessRopeChemList = new List<F_PR_SIZING_PROCESS_ROPE_CHEM>();
            FPrSizingProcessRopeDetailsList = new List<F_PR_SIZING_PROCESS_ROPE_DETAILS>();
        }


        public double ? TotalSizing { get; set; }
        public double ? TodaySizing { get; set; }
        public double ? MonthlySizing { get; set; }
        public F_PR_SIZING_PROCESS_ROPE_MASTER FPrSizingProcessRopeMaster { get; set; }
        public F_PR_SIZING_PROCESS_ROPE_DETAILS FPrSizingProcessRopeDetails { get; set; }
        public F_PR_SIZING_PROCESS_ROPE_CHEM FPrSizingProcessRopeChem { get; set; }
        public List<F_PR_SIZING_PROCESS_ROPE_DETAILS> FPrSizingProcessRopeDetailsList { get; set; }
        public List<F_PR_SIZING_PROCESS_ROPE_CHEM> FPrSizingProcessRopeChemList { get; set; }
        public List<PL_PRODUCTION_PLAN_MASTER> PlProductionPlanMasters { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributionsForEdit { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_WEAVING_BEAM> FWeavingBeams { get; set; }
        public List<F_SIZING_MACHINE> FSizingMachines { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductInfos { get; set; }
        public double? TotalProduction { get; set; }
        public double? MonthlyProduction { get; set; }
        public double? DailyProduction { get; set; }
        public double ? CompleteSets { get; set; }
        public double ? PendingSets { get; set; }
        public DateTime date { get; set; }
        public  double ? MonthlyCompleteSets { get; set; }
        public double ? MonthlyPendingSets { get; set; }
        public double ? MonthlyCompletePercent { get; set; }
        public double ? MonthlyPendingPercent { get; set; }
        public double ? ComparisonMonthlyProduction { get; set; }
    }
}
