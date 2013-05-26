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
using KarateGeek.databaseConnection;

namespace KarateGeek.Reports
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            loadAthleteForm();
            loadJudgeForm();
            loadEventForm();
            loadTournamentForm();
            loadClubForm();
            loadLotteryForm();
            loadScoreSheetForm();
        }

        private void loadAthleteForm()
        {
            AthleteCrystalReport objRpt =  new AthleteCrystalReport();
            DataSet ds = new DataSet();
            string sql = "SELECT * FROM athletes_total_details"; 

            Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

            adapter.Fill(ds, "athlete_dt");

            if (ds.Tables[0].Rows.Count == 0)
            {
                //MessageBox.Show("No data Found", "Athletes Forms");
                return;
            }

            // Setting data source of our report object
            objRpt.SetDataSource(ds);
            // Binding the crystalReportViewer with our report object. 
            crystalReportViewer1.ReportSource = objRpt;
        }

        private void loadJudgeForm()
        {
           JudgeCrystalRport objRpt = new JudgeCrystalRport();
            DataSet ds = new DataSet();
            string sql = "SELECT * FROM judges_total_details";

            Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

            adapter.Fill(ds, "judge_dt");
            /*
            if (ds.Tables[0].Rows.Count == 0)
            {
                //MessageBox.Show("No data Found", "Judges Forms");
                return;
            }
            */
            // Setting data source of our report object
            objRpt.SetDataSource(ds);
            // Binding the crystalReportViewer with our report object. 
            crystalReportViewer2.ReportSource = objRpt;
        }
        
private void loadEventForm()
{
    EventCrystalReport objRpt = new EventCrystalReport();
    DataSet ds = new DataSet();
    string sql = "SELECT * FROM events_total_details";

    Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

    adapter.Fill(ds, "event_dt");
    /*
    if (ds.Tables[0].Rows.Count == 0)
    {
        //MessageBox.Show("No data Found", "Events Forms");
        return;
    }
    */
    int rowNum = 0;
    foreach(DataRow row in ds.Tables[0].Rows){
                
        string gameType = ds.Tables[0].Rows[rowNum][16].ToString();
        switch(gameType){
            case Strings.indKata:
                ds.Tables[0].Rows[rowNum][16] = "Individual / Kata";
                rowNum++;
                break;
            case Strings.teamKata:
                ds.Tables[0].Rows[rowNum][16] = "Team / Kata";
                rowNum++;
                break;
            case Strings.syncKata:
                ds.Tables[0].Rows[rowNum][16] = "Synchronized / Kata";
                rowNum++;
                break;
            case Strings.indKumite:
                ds.Tables[0].Rows[rowNum][16] = "Individual / Kumite";
                rowNum++;
                break;
            case Strings.teamKumite:
                ds.Tables[0].Rows[rowNum][16] = "Team / Kumite";
                rowNum++;
                break;
            case Strings.enbu:
                ds.Tables[0].Rows[rowNum][16] = "Enbu";
                rowNum++;
                break;
            case Strings.fugugo:
                ds.Tables[0].Rows[rowNum][16] = "Fukugo";
                rowNum++;
                break;
        }
    }
    // Setting data source of our report object
    objRpt.SetDataSource(ds);
    // Binding the crystalReportViewer with our report object. 
    crystalReportViewer3.ReportSource = objRpt;
}
       
private void loadTournamentForm()
{
    TournamentCrystalReport objRpt = new TournamentCrystalReport();
    DataSet ds = new DataSet();
    string sql = "SELECT * FROM tournaments_total_details";

    Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

    adapter.Fill(ds, "tournament_dt");
    /*
    if (ds.Tables[0].Rows.Count == 0)
    {
        //MessageBox.Show("No data Found", "Events Forms");
        return;
    }
    */
    int rowNum = 0;
    foreach (DataRow row in ds.Tables[0].Rows)
    {

        string gameType = ds.Tables[0].Rows[rowNum][10].ToString();
        switch (gameType)
        {
            case Strings.indKata:
                ds.Tables[0].Rows[rowNum][10] = "Individual / Kata";
                rowNum++;
                break;
            case Strings.teamKata:
                ds.Tables[0].Rows[rowNum][10] = "Team / Kata";
                rowNum++;
                break;
            case Strings.syncKata:
                ds.Tables[0].Rows[rowNum][10] = "Synchronized / Kata";
                rowNum++;
                break;
            case Strings.indKumite:
                ds.Tables[0].Rows[rowNum][10] = "Individual / Kumite";
                rowNum++;
                break;
            case Strings.teamKumite:
                ds.Tables[0].Rows[rowNum][10] = "Team / Kumite";
                rowNum++;
                break;
            case Strings.enbu:
                ds.Tables[0].Rows[rowNum][10] = "Enbu";
                rowNum++;
                break;
            case Strings.fugugo:
                ds.Tables[0].Rows[rowNum][10] = "Fukugo";
                rowNum++;
                break;
        }
    }
    // Setting data source of our report object
    objRpt.SetDataSource(ds);
    // Binding the crystalReportViewer with our report object. 
   crystalReportViewer4.ReportSource = objRpt;
}
        
private void loadClubForm()
{
    ClubCrystalReport objRpt = new ClubCrystalReport();
    DataSet ds = new DataSet();
    string sql = "SELECT * FROM clubs_total_details";

    Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

    adapter.Fill(ds, "club_dt");
    /*
    if (ds.Tables[0].Rows.Count == 0)
    {
        //MessageBox.Show("No data Found", "Clubs Forms");
        return;
    }*/
    // Setting data source of our report object
    objRpt.SetDataSource(ds);
    // Binding the crystalReportViewer with our report object. 
    crystalReportViewer5.ReportSource = objRpt;
}

private void loadLotteryForm()
{
    GraphCrystalReport objRpt = new GraphCrystalReport();
    DataSet ds = new DataSet();
    string sql = "SELECT * FROM graph_view";

    Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

    adapter.Fill(ds, "graph_dt");
    /*
    if (ds.Tables[0].Rows.Count == 0)
    {
        //MessageBox.Show("No data Found", "Lottery Forms");
        return;
    }*/
    //apo edw pernei to string apo to graph
    string graph;
    int i = 0;
    int maxLines = 93;
    int lines;
    int maxLineChars = 132;
    int lineChars;
    bool smallerFont = false;
    foreach (DataRow row in ds.Tables[0].Rows)
    {
        graph = ds.Tables[0].Rows[i][0].ToString();
        lines = graph.Count(f => f == '\n');
        if(lines > maxLines)
            smallerFont = true;
        lineChars = graph.IndexOf('\n');
        if (lineChars > maxLineChars)
            smallerFont = true;
        i++;
    }
    FieldObject fo = (FieldObject)objRpt.ReportDefinition.ReportObjects["graph1"];
    //edw kanei diaforous elegxous gia na dei poio einai to katalilo megethos
    //to default apo to crystal einai 10
    if(smallerFont)
        fo.ApplyFont(new Font("consolas", 10F));
    

    // Setting data source of our report object
    objRpt.SetDataSource(ds);
    // Binding the crystalReportViewer with our report object. 
    crystalReportViewer6.ReportSource = objRpt;
}

private void loadScoreSheetForm()
{
    ScoreSheetCrystalReport objRpt = new ScoreSheetCrystalReport();
    DataSet ds = new DataSet();
    string sql = "SELECT * FROM progress_graph_view";

    Npgsql.NpgsqlDataAdapter adapter = new CoreDatabaseConnection().AdapterForQuery(sql);

    adapter.Fill(ds, "scoresheet_dt");
    /*
    if (ds.Tables[0].Rows.Count == 0)
    {
        //MessageBox.Show("No data Found", "Lottery Forms");
        return;
    }*/
    //apo edw pernei to string apo to graph
    string graph;
    int i = 0;
    int maxLines = 93;
    int lines;
    int maxLineChars = 132;
    int lineChars;
    bool smallerFont = false;

    foreach (DataRow row in ds.Tables[0].Rows)
    {
        graph = ds.Tables[0].Rows[i][0].ToString();
        lines = graph.Count(f => f == '\n');
        if (lines > maxLines)
            smallerFont = true;
        lineChars = graph.IndexOf('\n');
        if (lineChars > maxLineChars)
            smallerFont = true;
        i++;

    }
    FieldObject fo = (FieldObject)objRpt.ReportDefinition.ReportObjects["graph1"];
    //edw kanei diaforous elegxous gia na dei poio einai to katalilo megethos
    //to default apo to crystal einai 10
    if (smallerFont)
        fo.ApplyFont(new Font("consolas", 10F));


    // Setting data source of our report object
    objRpt.SetDataSource(ds);
    // Binding the crystalReportViewer with our report object. 
    crystalReportViewer7.ReportSource = objRpt;
}
    }
}
