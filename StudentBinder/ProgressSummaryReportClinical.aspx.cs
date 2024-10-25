using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;

public partial class StudentBinder_ProgressSummaryReportClinical : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    clsData oData = null;
    DataTable allclsviewdata = new DataTable();
    DataTable dateandcount = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnExport.Visible = false;
        clsview.Visible = false;
        Gvclsbehadate.Visible = false;
    }
   
    [Serializable]
    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        // local variable for network credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user,
            out string password, out string authority)
        {

            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
    private bool Validation()
    {
        bool result = true;
        if (txtRepStart.Text == "")
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Start Date");
            return result;
        }
        if (txtrepEdate.Text == "")
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter End Date");
            return result;
        }
        if (txtRepStart.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid Start date");
                return result;
            }            
        }
        if (txtrepEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid End date");
                return result;
            }            
        }
        if (txtRepStart.Text != "" && txtrepEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > dted)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
                return result;
            }            
        }
        return result;
    }

    private void GenerateReport()
    {
        ObjData = new clsData();
        tdMsg.InnerHtml = "";
        RV_ExcelReport.Visible = true;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");
        RV_ExcelReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_ExcelReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ProgressSummaryReportClinical"];
        RV_ExcelReport.ShowParameterPrompts = false;
        ReportParameter[] parm = new ReportParameter[4];
        parm[0] = new ReportParameter("StudentId", sess.StudentId.ToString());
        parm[1] = new ReportParameter("StartDate", StartDate.ToString());
        parm[2] = new ReportParameter("EndDate", enddate.ToString());
        parm[3] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
        this.RV_ExcelReport.ServerReport.SetParameters(parm);
        RV_ExcelReport.ServerReport.Refresh();
    }
    private void GenerateReportHighchart()
    {
        ObjData = new clsData();
        tdMsg.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");
        //CreateDataTable();
        getAllclassicViewData(StartDate, enddate,sess.StudentId,sess.SchoolId);
        ClassicLoadBehaviors();
        clsview.Visible = true;
        Gvclsbehadate.Visible = true;
    }

    public void getAllclassicViewData(String Sdate,String Edate,int studid, int schoolid)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("PSR_Data_Clinical", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", Sdate);
            cmd.Parameters.AddWithValue("@EndDate", Edate);
            cmd.Parameters.AddWithValue("@StudentId", studid);
            cmd.Parameters.AddWithValue("@SchoolId", schoolid);
            da = new SqlDataAdapter(cmd);
            da.Fill(allclsviewdata);
            countforrowsize();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
           
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + studid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }
    }

    private void countforrowsize()
    {
        DataTable groupedTable = new DataTable();
        groupedTable.Columns.Add("date", typeof(String));
        groupedTable.Columns.Add("id", typeof(int));
        groupedTable.Columns.Add("count", typeof(int));
        var groupedData = allclsviewdata.AsEnumerable()
           .GroupBy(row => new
           {
               Date = row.Field<String>("EvntDate"),
               Id = row.Field<int>("MeasurementId")
                   
           })
           .Select(g => new
           {
               Date = g.Key.Date,
               Id = g.Key.Id,
               Count = g.Count()
           });
        foreach (var group in groupedData)
        {
            groupedTable.Rows.Add(group.Date, group.Id, group.Count);
        }


        dateandcount.Columns.Add("Date", typeof(DateTime));
        dateandcount.Columns.Add("Count", typeof(int));

        var maxCounts = new Dictionary<String, int>();

        foreach (DataRow row in groupedTable.Rows)
        {
            string date = row.Field<string>("date");
            int count = row.Field<int>("count");

            if (maxCounts.ContainsKey(date))
            {
                maxCounts[date] = Math.Max(maxCounts[date], count);
            }
            else
            {
                maxCounts[date] = count;
            }
        }

        foreach (var kvp in maxCounts)
        {
            dateandcount.Rows.Add(kvp.Key, kvp.Value);
        }


    }

    public void ClassicLoadBehaviors()
    {
        DataSet ds = LoadDataHighchart();
        if (ds != null)
        {

            clslist.DataSource = ds;
            clslist.DataBind();
        }
        else
        {
            td1.Visible = true;
            td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
        }
    }

   
 
   
    protected void btnSessView_Click(object sender, EventArgs e)
    {
        try
        {
            RV_ExcelReport.Visible = false;
            rbtnClassType.Visible = true;
            if (Validation() == true)
            {
                tdMsg.Visible = false;
                td1.Visible = false;
                btnExport.Visible = true;
                LoadBehaviors();
                dlBehavior.Visible = true;
            }
            else
            {
                dlBehavior.Visible = false;
                tdMsg.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnClassicView_Click(object sender, EventArgs e)
    {
        try
        {
            dlBehavior.Visible = false;
            rbtnClassType.Visible = false;
            if (Validation() == true)
            {
                tdMsg.Visible = false;
                td1.Visible = false;
                btnExport.Visible = false;
                if (highcheck.Checked == false)
                {
                RV_ExcelReport.Visible = true;
                GenerateReport();
            }
            else
            {
                RV_ExcelReport.Visible = false;
                    GenerateReportHighchart();
                }
            }
            else
            {
                RV_ExcelReport.Visible = false;
                tdMsg.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        Response.Clear();
        string filename = sess.StudentName + "_ClinicalProgressSummaryReport.xls";
        string enCodeFileName = Server.UrlEncode(filename);  
        Response.AddHeader("content-disposition", "attachment;filename=" + enCodeFileName);
        Response.ContentType = "application/ms-excel";
        Response.ContentEncoding = System.Text.Encoding.Unicode;
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
        dlBehavior.RenderControl(hw);
        Response.Write(sw.ToString());
        Response.End();  
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        rbtnClassType.Visible = false;
        rbtnClassType.SelectedIndex = 2; 
        txtRepStart.Text = string.Empty;
        txtrepEdate.Text = string.Empty;
        RV_ExcelReport.Visible = false;
        td1.Visible = false;
        tdMsg.Visible = false;
        dlBehavior.Visible = false;
        btnSessView.Visible = true;
        btnClassicView.Visible = true;
        btnExport.Visible = false;
    }

    public void LoadBehaviors()
    {
        DataSet ds = LoadData();
        if (ds != null)
        {
            dlBehavior.DataSource = ds;
            dlBehavior.DataBind();
        }
        else
        {
            td1.Visible = true;
            td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
        }
    }


    protected DataSet LoadDataHighchart()
    {
        sess = (clsSession)Session["UserSession"];
        oData = new clsData();

        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        String sqlStr = "SELECT MeasurementId,Behaviour,IsFrequency,IsDuration,IsYesOrNo FROM ("+
    "SELECT   B.StudentId,  B.MeasurementId,BD.Behaviour,BD.Frequency AS IsFrequency, BD.Duration AS IsDuration,BD.YesOrNo AS IsYesOrNo FROM "+
      "  BehaviourDetails BD INNER JOIN Behaviour B ON "+
       " B.MeasurementId = BD.MeasurementId  WHERE  B.StudentId = " + sess.StudentId + "  AND BD.ActiveInd IN ('A', 'N')  AND BD.SchoolId = " + sess.SchoolId + ") AS StdCalcs " +
	"WHERE  StdCalcs.StudentId = "+sess.StudentId+" GROUP BY  MeasurementId,  Behaviour, IsFrequency, IsDuration, IsYesOrNo ORDER BY  Behaviour";
        DataSet ds = oData.ReturnDataSet(sqlStr, false);
        return ds;
    }

    protected DataSet LoadData()
    {
        sess = (clsSession)Session["UserSession"];
        oData = new clsData();

        String sqlStr = "SELECT MeasurementId,Behaviour FROM BehaviourDetails WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " AND ActiveInd IN('A','N') "+
        "ORDER BY Behaviour,MeasurementId";
        DataSet ds = oData.ReturnDataSet(sqlStr, false);
        return ds;
    }

 
    protected void clslist_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        Gvclsbehadate.DataSource = GetDataForDate(); // Method to get data for the first GridView
        Gvclsbehadate.DataBind();

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            oData = new clsData();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            //enddate = enddate + " 23:59:59.998";


            GridView Gvclsbeha = (e.Item.FindControl("Gvclsbeha") as GridView);
            string BehaviorId = (e.Item.FindControl("bhid") as Label).Text;
            int frq = 0, dur = 0, yesNo = 0;
            string sqlStr = "SELECT ISNULL(Frequency,0) AS IsFrequency, ISNULL(Duration,0) AS IsDuration, ISNULL(YesOrNo,0) AS IsYesOrNo FROM BehaviourDetails WHERE MeasurementId=" + BehaviorId;
            DataTable DT = oData.ReturnDataTable(sqlStr, false);
            if (DT != null && DT.Rows.Count == 1)
            {
                frq = Convert.ToInt32(DT.Rows[0]["IsFrequency"]);
                dur = Convert.ToInt32(DT.Rows[0]["IsDuration"]);
                yesNo = Convert.ToInt32(DT.Rows[0]["IsYesOrNo"]);
            }
            if (frq == 0)
            {
                ((DataControlField)Gvclsbeha.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Frequency"))
                    .SingleOrDefault()).Visible = false;
            }
            if (dur == 0)
            {
                ((DataControlField)Gvclsbeha.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Duration"))
                    .SingleOrDefault()).Visible = false;
            }
            if (yesNo == 0)
            {
                ((DataControlField)Gvclsbeha.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Yes/No"))
                    .SingleOrDefault()).Visible = false;
            }

                ((DataControlField)Gvclsbeha.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Date"))
                    .SingleOrDefault()).Visible = false;
            

            DataTable dt = getClsDataByMeasurementId(BehaviorId,StartDate,enddate);

            Gvclsbeha.DataSource = dt;
            Gvclsbeha.DataBind();

            foreach (GridViewRow row in Gvclsbeha.Rows)
            {
                Label datelabel = (Label)row.FindControl("dateid");
                foreach (DataRow rows in dateandcount.Rows)
                 {
                     DateTime dateTime = DateTime.Parse((rows["date"]).ToString());
                     string dateOnly = dateTime.ToString("MM/dd/yyyy");
                     if (datelabel.Text == dateOnly)
                     {
                         if (Convert.ToInt32(rows["count"]) > 1)
                         {
                             int count = 30 + (Convert.ToInt32(rows["count"]) * 15);
                             row.Style["height"] = count + "px";

                         }
                         else
                         {
                             
                                 row.Style["height"] = "40px";
                             
                         }
                     }
                
                 }
            }
            foreach (GridViewRow row in Gvclsbehadate.Rows)
            {
                Label datelabel = (Label)row.FindControl("dateid2");
                foreach (DataRow rows in dateandcount.Rows)
                {
                    DateTime dateTime = DateTime.Parse((rows["date"]).ToString());
                    string dateOnly = dateTime.ToString("MM/dd/yyyy");
                    if (datelabel.Text == dateOnly)
                    {
                        if (Convert.ToInt32(rows["count"]) > 1)
                        {
                            int count = 30 + (Convert.ToInt32(rows["count"]) * 15);
                            row.Style["height"] = count + "px";

                        }
                        else
                        {
                            row.Style["height"] = "40px";
                        }
                    }

                }
            }
            
        }
    
    }
    private DataTable GetDataForDate()
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd"); 

        DataTable table = new DataTable();
        table.Columns.Add("Date", typeof(DateTime));

        DateTime startDate = DateTime.Parse(StartDate);
        DateTime endDate = DateTime.Parse(enddate);

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            DataRow row = table.NewRow();
            row["Date"] = date;
            table.Rows.Add(row);
        }

        

        return table;
    }
    protected DataTable getClsDataByMeasurementId(string BehaviorId, String StartDate,string enddate)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("EvntDate", typeof(DateTime));
        dt.Columns.Add("Time", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("Duration", typeof(string));
        dt.Columns.Add("YesOrNo", typeof(string));
        dt.Columns.Add("EventName", typeof(string));
        dt.Columns.Add("StdtSessEventType", typeof(string));

        DateTime startDate = DateTime.Parse(StartDate);
        DateTime endDate = DateTime.Parse(enddate);

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            string totDate = date.ToString("MM/dd/yyyy");
            string time = "";
            string user = "";
            string fre="";
            string yorn="";
            string dur="";
            string evname="";
            string type="";
            DataRow row = dt.NewRow();
            row["EvntDate"] = date;
            if (allclsviewdata != null && allclsviewdata.Rows.Count > 0)
            {
                for (int r = 0; r < allclsviewdata.Rows.Count;r++ )
                {
                    if (allclsviewdata.Rows[r]["EvntDate"].ToString() == totDate && Convert.ToInt32(allclsviewdata.Rows[r]["MeasurementId"]) == Convert.ToInt32(BehaviorId))
                    {
                        
                            time = time + "<br/>" + allclsviewdata.Rows[r]["Time"].ToString();
                        
                            user = user + "<br/>" + allclsviewdata.Rows[r]["Name"].ToString();
                       
                            fre = fre + "<br/>" + allclsviewdata.Rows[r]["Frequency"].ToString();
                        
                            dur = dur + "<br/>" + allclsviewdata.Rows[r]["Duration"].ToString();
                        
                            yorn = yorn + "<br/>" + allclsviewdata.Rows[r]["YesOrNo"].ToString();
                        
                            evname = evname + "<br/>" + allclsviewdata.Rows[r]["EventName"].ToString();
                        
                            type = type + "<br/>" + allclsviewdata.Rows[r]["StdtSessEventType"].ToString();
                        

                    }
                }
                row["Time"] = time;
                row["Name"] = user;
                row["Frequency"] = fre;
                row["Duration"] = dur;
                row["YesOrNo"] = yorn;
                row["EventName"] = evname;
                row["StdtSessEventType"] = type;
            }
          
            dt.Rows.Add(row);
        }

        return dt;
    }

    protected void dlBehavior_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            oData = new clsData();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            //enddate = enddate + " 23:59:59.998";

            GridView gVBehvior = (e.Item.FindControl("gVBehvior") as GridView);
            string BehaviorId = (e.Item.FindControl("BehaviorId") as Label).Text;
            int frq = 0, dur = 0, yesNo = 0;
            string sqlStr = "SELECT ISNULL(Frequency,0) AS IsFrequency, ISNULL(Duration,0) AS IsDuration, ISNULL(YesOrNo,0) AS IsYesOrNo FROM BehaviourDetails WHERE MeasurementId=" + BehaviorId;
            DataTable DT = oData.ReturnDataTable(sqlStr, false);
            if (DT != null && DT.Rows.Count == 1)
            {
                frq = Convert.ToInt32(DT.Rows[0]["IsFrequency"]);
                dur = Convert.ToInt32(DT.Rows[0]["IsDuration"]);
                yesNo = Convert.ToInt32(DT.Rows[0]["IsYesOrNo"]);
            }
            if (frq == 0)
            {
                ((DataControlField)gVBehvior.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Frequency"))
                    .SingleOrDefault()).Visible = false;
            }
            if (dur == 0)
            {
                ((DataControlField)gVBehvior.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Duration"))
                    .SingleOrDefault()).Visible = false;
            }
            if (yesNo == 0)
            {
                ((DataControlField)gVBehvior.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Yes/No"))
                    .SingleOrDefault()).Visible = false;
            }
            
            SqlCommand cmd = null;
            DataTable Dt = new DataTable();
            SqlConnection con = ObjData.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("PSR_GridData_Clinical", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", enddate);
                cmd.Parameters.AddWithValue("@Studentid", sess.StudentId);
                cmd.Parameters.AddWithValue("@MeasurementId", BehaviorId);
                cmd.Parameters.AddWithValue("@TypeofClass", rbtnClassType.SelectedValue);
                da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                tdMsg.InnerHtml = clsGeneral.warningMsg("Some of the behaviours may not be displayed due to larger amount of data. Please select a shorter date range.");
                tdMsg.Visible = true;
                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\nBehaviour ID = " + BehaviorId + "\n" + ex.ToString());
            }
            finally
            {
                ObjData.Close(con);
            }
            gVBehvior.DataSource = Dt;
            gVBehvior.DataBind();
        }
    }
}