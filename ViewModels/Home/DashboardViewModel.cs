namespace DenimERP.ViewModels.Home
{
    public class DashboardViewModel
    {
        public int TotalNumberOfComImpLcInformation { get; set; }
        public int TotalNumberOfComExLcInformation { get; set; }
        public int TotalNumberOfRndFabricInformation { get; set; }
        public int TotalNumberOfComExPiInformation { get; set; }

        public double TotalPercentageOfComImpLcInformationList { get; set; }
        public double TotalPercentageOfComExLcInfoList { get; set; }
        public double TotalPercentageOfRndFabricInformation { get; set; }
        public double TotalPercentageOfComExPiInformation { get; set; }
        public PiChartDataViewModel PiChartDataViewModel { get; set; }
        public LcChartDataViewModel LcChartDataViewModel { get; set; }
        public DyeingChartDataViewModel DyeingChartDataViewModel { get; set; }
        public InspectionChartDataViewModel InspectionChartDataViewModel { get; set; }
        public ChartViewModel ChartViewModel { get; set; }
        public SizingChartDataViewModel SizingChartDataViewModel { get; set; }
        public FabricDeliveryChallanViewModel FabricDeliveryChallanViewModel { get; set; }
        public RealizationViewModel RealizationViewModel { get; set; }
        public WeavingChartDataViewModel WeavingChartDataViewModel { get; set; }
        public TopStyleProductionViewModel TopStyleProductionViewModel { get; set; }
        public FYsYarnIssueViewModel FYsYarnIssueViewModel { get; set; }
        public FChemIssueViewModel FChemIssueViewModel { get; set; }
        public WarpingChartDataViewModel WarpingChartDataViewModel { get; set; }
    }
}
