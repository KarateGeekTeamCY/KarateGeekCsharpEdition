using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KarateGeek.databaseConnection;

namespace KarateGeek.Reports
{
    public partial class LotteryForm : Form
    {
        public LotteryForm()
        {
            InitializeComponent();
        }

        private void load_form(object sender, EventArgs e)
        {
            Graph objRpt;
            // Creating object of our report.
            objRpt = new Graph();

            DataSet ds = new DataSet();

            Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery("SELECT * FROM graph_view;");

            adapter.Fill(ds, "graph_dt");

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("No data Found", "CrystalReportWithOracle");
                return;
            }

            // Setting data source of our report object
            objRpt.SetDataSource(ds);
            // Binding the crystalReportViewer with our report object. 
            crystalReportViewer1.ReportSource = objRpt;
            CrystalDecisions.CrystalReports.Engine.TextObject root;
            root = (CrystalDecisions.CrystalReports.Engine.TextObject)
                 objRpt.ReportDefinition.ReportObjects["txt_header"];
            root.Text = "Sample Report By Using Data Table!!";

           
        }
    }
}
