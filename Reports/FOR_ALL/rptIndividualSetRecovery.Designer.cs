//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DenimERP.Reports.FOR_ALL {
    
    public partial class rptIndividualSetRecovery : DevExpress.XtraReports.UI.XtraReport {
        private void InitializeComponent() {
            DevExpress.XtraReports.ReportInitializer reportInitializer = new DevExpress.XtraReports.ReportInitializer(this, "DenimERP.Reports.FOR_ALL.rptIndividualSetRecovery.repx");

            // Controls
            this.TopMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.TopMarginBand>("TopMargin");
            this.BottomMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.BottomMarginBand>("BottomMargin");
            this.Detail = reportInitializer.GetControl<DevExpress.XtraReports.UI.DetailBand>("Detail");
            this.GroupHeader1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupHeaderBand>("GroupHeader1");
            this.ReportHeader = reportInitializer.GetControl<DevExpress.XtraReports.UI.ReportHeaderBand>("ReportHeader");
            this.ReportFooter = reportInitializer.GetControl<DevExpress.XtraReports.UI.ReportFooterBand>("ReportFooter");
            this.PageHeader = reportInitializer.GetControl<DevExpress.XtraReports.UI.PageHeaderBand>("PageHeader");
            this.PageFooter = reportInitializer.GetControl<DevExpress.XtraReports.UI.PageFooterBand>("PageFooter");
            this.GroupFooter1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupFooterBand>("GroupFooter1");
            this.subreport6 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRSubreport>("subreport6");
            this.subreport6.ReportSource = new DenimERP.Reports.FOR_ALL.rptSetwise_SlasherConsumption();
            this.subreport5 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRSubreport>("subreport5");
            this.subreport5.ReportSource = new DenimERP.Reports.FOR_ALL.rptSetWiseFinishing_Consumption();
            this.subreport4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRSubreport>("subreport4");
            this.subreport4.ReportSource = new DenimERP.Reports.FOR_ALL.rptSetWiseSizing_Consumption();
            this.subreport3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRSubreport>("subreport3");
            this.subreport3.ReportSource = new DenimERP.Reports.FOR_ALL.setwise_RopeConsumption();
            this.subreport2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRSubreport>("subreport2");
            this.subreport2.ReportSource = new DenimERP.Reports.FOR_ALL.rptSetWiseWeft_Consumption();
            this.subreport1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRSubreport>("subreport1");
            this.subreport1.ReportSource = new DenimERP.Reports.FOR_ALL.rptSetWiseWarp_Consumption();
            this.label1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label1");
            this.table1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("table1");
            this.tableRow1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow1");
            this.tableRow2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow2");
            this.tableRow3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow3");
            this.tableRow4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow4");
            this.tableRow5 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow5");
            this.tableRow6 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow6");
            this.tableRow7 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow7");
            this.tableRow8 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow8");
            this.tableRow9 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow9");
            this.tableRow10 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow10");
            this.tableRow11 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow11");
            this.tableRow12 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow12");
            this.tableRow13 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow13");
            this.tableRow14 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow14");
            this.tableRow15 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow15");
            this.tableCell1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell1");
            this.tableCell2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell2");
            this.tableCell3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell3");
            this.tableCell4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell4");
            this.tableCell5 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell5");
            this.tableCell6 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell6");
            this.tableCell7 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell7");
            this.tableCell8 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell8");
            this.tableCell9 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell9");
            this.tableCell10 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell10");
            this.tableCell11 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell11");
            this.tableCell12 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell12");
            this.tableCell13 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell13");
            this.tableCell14 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell14");
            this.tableCell15 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell15");
            this.tableCell16 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell16");
            this.tableCell17 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell17");
            this.tableCell18 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell18");
            this.tableCell19 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell19");
            this.tableCell20 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell20");
            this.tableCell21 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell21");
            this.tableCell22 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell22");
            this.tableCell23 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell23");
            this.tableCell24 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell24");
            this.tableCell25 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell25");
            this.tableCell26 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell26");
            this.tableCell27 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell27");
            this.tableCell34 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell34");
            this.tableCell31 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell31");
            this.tableCell35 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell35");
            this.tableCell33 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell33");
            this.tableCell36 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell36");
            this.tableCell37 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell37");
            this.tableCell39 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell39");
            this.tableCell43 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell43");
            this.tableCell44 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell44");
            this.tableCell45 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell45");
            this.tableCell46 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell46");
            this.tableCell48 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell48");
            this.tableCell52 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell52");
            this.tableCell53 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell53");
            this.tableCell54 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell54");
            this.tableCell55 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell55");
            this.tableCell57 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell57");
            this.tableCell61 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell61");
            this.tableCell62 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell62");
            this.tableCell63 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell63");
            this.tableCell64 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell64");
            this.tableCell66 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell66");
            this.tableCell70 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell70");
            this.tableCell71 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell71");
            this.tableCell72 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell72");
            this.tableCell73 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell73");
            this.tableCell75 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell75");
            this.tableCell79 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell79");
            this.tableCell80 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell80");
            this.tableCell81 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell81");
            this.tableCell82 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell82");
            this.tableCell84 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell84");
            this.tableCell88 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell88");
            this.tableCell89 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell89");
            this.tableCell90 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell90");
            this.tableCell91 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell91");
            this.tableCell93 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell93");
            this.tableCell97 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell97");
            this.tableCell98 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell98");
            this.tableCell99 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell99");
            this.tableCell100 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell100");
            this.tableCell102 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell102");
            this.tableCell103 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell103");
            this.tableCell104 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell104");
            this.tableCell105 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell105");
            this.tableCell106 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell106");
            this.tableCell107 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell107");

            // Parameters
            this.SETNO = reportInitializer.GetParameter("SETNO");

            // Data Sources
            this.sqlDataSource1 = reportInitializer.GetDataSource<DevExpress.DataAccess.Sql.SqlDataSource>("sqlDataSource1");
        }
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRSubreport subreport6;
        private DevExpress.XtraReports.UI.XRSubreport subreport5;
        private DevExpress.XtraReports.UI.XRSubreport subreport4;
        private DevExpress.XtraReports.UI.XRSubreport subreport3;
        private DevExpress.XtraReports.UI.XRSubreport subreport2;
        private DevExpress.XtraReports.UI.XRSubreport subreport1;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow4;
        private DevExpress.XtraReports.UI.XRTableRow tableRow5;
        private DevExpress.XtraReports.UI.XRTableRow tableRow6;
        private DevExpress.XtraReports.UI.XRTableRow tableRow7;
        private DevExpress.XtraReports.UI.XRTableRow tableRow8;
        private DevExpress.XtraReports.UI.XRTableRow tableRow9;
        private DevExpress.XtraReports.UI.XRTableRow tableRow10;
        private DevExpress.XtraReports.UI.XRTableRow tableRow11;
        private DevExpress.XtraReports.UI.XRTableRow tableRow12;
        private DevExpress.XtraReports.UI.XRTableRow tableRow13;
        private DevExpress.XtraReports.UI.XRTableRow tableRow14;
        private DevExpress.XtraReports.UI.XRTableRow tableRow15;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell6;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.XRTableCell tableCell10;
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell12;
        private DevExpress.XtraReports.UI.XRTableCell tableCell13;
        private DevExpress.XtraReports.UI.XRTableCell tableCell14;
        private DevExpress.XtraReports.UI.XRTableCell tableCell15;
        private DevExpress.XtraReports.UI.XRTableCell tableCell16;
        private DevExpress.XtraReports.UI.XRTableCell tableCell17;
        private DevExpress.XtraReports.UI.XRTableCell tableCell18;
        private DevExpress.XtraReports.UI.XRTableCell tableCell19;
        private DevExpress.XtraReports.UI.XRTableCell tableCell20;
        private DevExpress.XtraReports.UI.XRTableCell tableCell21;
        private DevExpress.XtraReports.UI.XRTableCell tableCell22;
        private DevExpress.XtraReports.UI.XRTableCell tableCell23;
        private DevExpress.XtraReports.UI.XRTableCell tableCell24;
        private DevExpress.XtraReports.UI.XRTableCell tableCell25;
        private DevExpress.XtraReports.UI.XRTableCell tableCell26;
        private DevExpress.XtraReports.UI.XRTableCell tableCell27;
        private DevExpress.XtraReports.UI.XRTableCell tableCell34;
        private DevExpress.XtraReports.UI.XRTableCell tableCell31;
        private DevExpress.XtraReports.UI.XRTableCell tableCell35;
        private DevExpress.XtraReports.UI.XRTableCell tableCell33;
        private DevExpress.XtraReports.UI.XRTableCell tableCell36;
        private DevExpress.XtraReports.UI.XRTableCell tableCell37;
        private DevExpress.XtraReports.UI.XRTableCell tableCell39;
        private DevExpress.XtraReports.UI.XRTableCell tableCell43;
        private DevExpress.XtraReports.UI.XRTableCell tableCell44;
        private DevExpress.XtraReports.UI.XRTableCell tableCell45;
        private DevExpress.XtraReports.UI.XRTableCell tableCell46;
        private DevExpress.XtraReports.UI.XRTableCell tableCell48;
        private DevExpress.XtraReports.UI.XRTableCell tableCell52;
        private DevExpress.XtraReports.UI.XRTableCell tableCell53;
        private DevExpress.XtraReports.UI.XRTableCell tableCell54;
        private DevExpress.XtraReports.UI.XRTableCell tableCell55;
        private DevExpress.XtraReports.UI.XRTableCell tableCell57;
        private DevExpress.XtraReports.UI.XRTableCell tableCell61;
        private DevExpress.XtraReports.UI.XRTableCell tableCell62;
        private DevExpress.XtraReports.UI.XRTableCell tableCell63;
        private DevExpress.XtraReports.UI.XRTableCell tableCell64;
        private DevExpress.XtraReports.UI.XRTableCell tableCell66;
        private DevExpress.XtraReports.UI.XRTableCell tableCell70;
        private DevExpress.XtraReports.UI.XRTableCell tableCell71;
        private DevExpress.XtraReports.UI.XRTableCell tableCell72;
        private DevExpress.XtraReports.UI.XRTableCell tableCell73;
        private DevExpress.XtraReports.UI.XRTableCell tableCell75;
        private DevExpress.XtraReports.UI.XRTableCell tableCell79;
        private DevExpress.XtraReports.UI.XRTableCell tableCell80;
        private DevExpress.XtraReports.UI.XRTableCell tableCell81;
        private DevExpress.XtraReports.UI.XRTableCell tableCell82;
        private DevExpress.XtraReports.UI.XRTableCell tableCell84;
        private DevExpress.XtraReports.UI.XRTableCell tableCell88;
        private DevExpress.XtraReports.UI.XRTableCell tableCell89;
        private DevExpress.XtraReports.UI.XRTableCell tableCell90;
        private DevExpress.XtraReports.UI.XRTableCell tableCell91;
        private DevExpress.XtraReports.UI.XRTableCell tableCell93;
        private DevExpress.XtraReports.UI.XRTableCell tableCell97;
        private DevExpress.XtraReports.UI.XRTableCell tableCell98;
        private DevExpress.XtraReports.UI.XRTableCell tableCell99;
        private DevExpress.XtraReports.UI.XRTableCell tableCell100;
        private DevExpress.XtraReports.UI.XRTableCell tableCell102;
        private DevExpress.XtraReports.UI.XRTableCell tableCell103;
        private DevExpress.XtraReports.UI.XRTableCell tableCell104;
        private DevExpress.XtraReports.UI.XRTableCell tableCell105;
        private DevExpress.XtraReports.UI.XRTableCell tableCell106;
        private DevExpress.XtraReports.UI.XRTableCell tableCell107;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter SETNO;
    }
}
