using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace KarateGeek.Reports
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            //gia to athlete
            ReportDocument cryRpt = new ReportDocument();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables ;
            
            cryRpt.Load("C:\\Users\\michael\\Documents\\GitHub\\KarateGeekCsharpEdition\\KarateGeek\\KarateGeek\\Reports\\AthleteCrystalReport.rpt");

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "karategeek";
            crConnectionInfo.UserID = "postgres";
            crConnectionInfo.Password = "admin";

            CrTables = cryRpt.Database.Tables ;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();

            //gia to judge
            ReportDocument cryRpt2 = new ReportDocument();
            TableLogOnInfos crtableLogoninfos2 = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo2 = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo2 = new ConnectionInfo();
            Tables CrTables2;

            cryRpt2.Load("C:\\Users\\michael\\Documents\\GitHub\\KarateGeekCsharpEdition\\KarateGeek\\KarateGeek\\Reports\\JudgeCrystalReport.rpt");

            crConnectionInfo2.ServerName = "localhost";
            crConnectionInfo2.DatabaseName = "karategeek";
            crConnectionInfo2.UserID = "postgres";
            crConnectionInfo2.Password = "admin";

            CrTables2 = cryRpt2.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables2)
            {
                crtableLogoninfo2 = CrTable.LogOnInfo;
                crtableLogoninfo2.ConnectionInfo = crConnectionInfo2;
                CrTable.ApplyLogOnInfo(crtableLogoninfo2);
            }

            crystalReportViewer5.ReportSource = cryRpt2;
            crystalReportViewer5.Refresh(); 
        
            //gia to events
            
            ReportDocument cryRpt3 = new ReportDocument();
            TableLogOnInfos crtableLogoninfos3 = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo3 = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo3 = new ConnectionInfo();
            Tables CrTables3;

            cryRpt3.Load("C:\\Users\\michael\\Documents\\GitHub\\KarateGeekCsharpEdition\\KarateGeek\\KarateGeek\\Reports\\EventCrystalReport.rpt");

            crConnectionInfo3.ServerName = "localhost";
            crConnectionInfo3.DatabaseName = "karategeek";
            crConnectionInfo3.UserID = "postgres";
            crConnectionInfo3.Password = "admin";

            CrTables3 = cryRpt3.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables3)
            {
                crtableLogoninfo3 = CrTable.LogOnInfo;
                crtableLogoninfo3.ConnectionInfo = crConnectionInfo3;
                CrTable.ApplyLogOnInfo(crtableLogoninfo3);
            }

            crystalReportViewer4.ReportSource = cryRpt3;
            crystalReportViewer4.Refresh(); 


            //gia to tournaments

            ReportDocument cryRpt4 = new ReportDocument();
            TableLogOnInfos crtableLogoninfos4 = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo4 = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo4 = new ConnectionInfo();
            Tables CrTables4;

            cryRpt4.Load("C:\\Users\\michael\\Documents\\GitHub\\KarateGeekCsharpEdition\\KarateGeek\\KarateGeek\\Reports\\TournamentCrystalReport.rpt");

            crConnectionInfo4.ServerName = "localhost";
            crConnectionInfo4.DatabaseName = "karategeek";
            crConnectionInfo4.UserID = "postgres";
            crConnectionInfo4.Password = "admin";

            CrTables4 = cryRpt4.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables4)
            {
                crtableLogoninfo4 = CrTable.LogOnInfo;
                crtableLogoninfo4.ConnectionInfo = crConnectionInfo4;
                CrTable.ApplyLogOnInfo(crtableLogoninfo4);
            }

            crystalReportViewer2.ReportSource = cryRpt4;
            crystalReportViewer2.Refresh();

            //gia to club

            ReportDocument cryRpt5 = new ReportDocument();
            TableLogOnInfos crtableLogoninfos5 = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo5 = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo5 = new ConnectionInfo();
            Tables CrTables5;

            cryRpt5.Load("C:\\Users\\michael\\Documents\\GitHub\\KarateGeekCsharpEdition\\KarateGeek\\KarateGeek\\Reports\\ClubCrystalReport.rpt");

            crConnectionInfo5.ServerName = "localhost";
            crConnectionInfo5.DatabaseName = "karategeek";
            crConnectionInfo5.UserID = "postgres";
            crConnectionInfo5.Password = "admin";

            CrTables5 = cryRpt5.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables5)
            {
                crtableLogoninfo5 = CrTable.LogOnInfo;
                crtableLogoninfo5.ConnectionInfo = crConnectionInfo5;
                CrTable.ApplyLogOnInfo(crtableLogoninfo5);
            }

            crystalReportViewer6.ReportSource = cryRpt5;
            crystalReportViewer6.Refresh(); 
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
        private void crystalReportViewer2_Load(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer5_Load(object sender, EventArgs e)
        {

        }

        

        private void crystalReportViewer4_Load(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer6_Load(object sender, EventArgs e)
        {

        }
    }
}
