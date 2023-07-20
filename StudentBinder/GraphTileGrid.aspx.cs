using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Calculate;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web.Services;

public partial class StudentBinder_GraphTileGrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    public void loadgraphMenu(object sender, EventArgs e)
    {
        int studid = Convert.ToInt32(Request.QueryString["studid"]);
        int pageid = Convert.ToInt32(Request.QueryString["pageid"]);
        LinkButton btn = (LinkButton)sender;
        String s= btn.CommandName.ToString();
        if (s == "1") ClientScript.RegisterStartupScript(this.GetType(), "", "PopupAcademic();", true);// Response.Redirect("LessonReportsWithPaging.aspx?studid=" + studid + "&pageid=" + pageid);
        if (s == "2") ClientScript.RegisterStartupScript(this.GetType(), "", "PopupClinical();", true);//.Redirect("BiweeklyBehaviorGraph.aspx?studid=" + studid + "&pageid=" + pageid);
        if (s == "3") ClientScript.RegisterStartupScript(this.GetType(), "", "PopupSessionBased();", true); //Response.Redirect("AcademicSessionReport.aspx?studid=" + studid + "&pageid=" + pageid);
        if (s == "5") ClientScript.RegisterStartupScript(this.GetType(), "", "PopupExcelView();", true); //Response.Redirect("ExcelViewReport.aspx?studid=" + studid + "&pageid=" + pageid);
        if (s == "6") ClientScript.RegisterStartupScript(this.GetType(), "", "PopupAcademicPGSummary();", true);// Response.Redirect("ProgressSummaryReport.aspx?studid=" + studid + "&pageid=" + pageid);
        if (s == "7") ClientScript.RegisterStartupScript(this.GetType(), "", "PopupClinicalPGSummary();", true); //Response.Redirect("ProgressSummaryReportClinical.aspx?studid=" + studid + "&pageid=" + pageid);

            
        
    }
}