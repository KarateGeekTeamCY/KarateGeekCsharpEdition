using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports ;
using CrystalDecisions.CrystalReports.Engine;
using KarateGeek.databaseConnection;

namespace KarateGeek.reports
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        CrystalDecisions.CrystalReports.Engine.ReportDocument myReportDocument; 
        private void Form1_Load(object sender, EventArgs e)
        {
            AthleteConnection myBaseDAO = new AthleteConnection();
            ReportDocument myReportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            DataTable myDT = myBaseDAO.GetAthletes();

            myReportDocument.Load("karategeek.reports.CrystalReport4.rpt");

            myReportDocument.SetDataSource(myDT);
            this.athinaViewer.ReportSource = myReportDocument;


            this.athinaViewer.RefreshReport();
        }


        public void shit()
        {
            AthleteConnection myBaseDAO = new AthleteConnection();
            ReportDocument myReportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            DataTable myDT = myBaseDAO.GetAthletes();

            myReportDocument.Load("C:\\Users\\Athina\\Documents\\GitHub\\KarateGeekCsharpEdition\\KarateGeek\\KarateGeek\\reports\\CrystalReport6.rpt");

            myReportDocument.SetDataSource(myDT);
            this.athinaViewer.ReportSource = myReportDocument;


            this.athinaViewer.RefreshReport();
        
        
        
        
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.shit();
        }
    }
}
