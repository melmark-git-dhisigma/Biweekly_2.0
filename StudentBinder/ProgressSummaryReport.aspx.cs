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
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.Script.Serialization;
using System.IO.Compression;



public partial class StudentBinder_ProgressSummaryReport : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    System.Data.DataTable Dt = null;
    clsData oData_ov = null;
    DataTable allclsviewdata = new DataTable();
    DataTable DSSessionDataZerodt = new DataTable();
    DataTable DSSessionDatadt = new DataTable();
    DataTable DSEventSessionZerodt = new DataTable();
    DataTable DSScoredt = new DataTable();
    DataTable DSScoreZerodt = new DataTable();
    DataTable allless = new DataTable();
    Dictionary<string, int> dateandcount = new Dictionary<string, int>();
    string []columntypes=null;
    Dictionary<string, string> coltypedict = new Dictionary<string, string>();
    Dictionary<string, string> columntypesdict = new Dictionary<string, string>();
    int countlesson = 0;
    int countdatalist = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLessonPlan();            
        }
        btnExport.Visible = false;
        clsview.Visible = false;
        Gvclsdate.Visible = false;
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rbtnClassType.Visible = false;
            tableDurFormat.Visible = false;
            if (Validation() == true)
            {
                if (highcheck.Checked == false)
                {
                GenerateReport();
                }
                else
                {
                    GenerateReportHighchart();
                }
                chkStatus.Visible = false;
                dlLesson.Visible = false;
                td1.Visible = false;
                tdMsg.Visible = false;
                btnExport.Visible = true;

            }
            else
            {
                RV_ExcelReport.Visible = false;
                dlLesson.Visible = false;
                tdMsg.Visible = true;
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
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

    private void GenerateReportHighchart()
    {
        ObjData = new clsData();
        tdMsg.InnerHtml = "";
        RV_ExcelReport.Visible = false;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd") + " 23:59:59.999";

        string LessonId = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {
            if (item.Selected == true)
            {
                LessonId += item.Value + ",";
            }
        }
        LessonId = LessonId.Substring(0, (LessonId.Length - 1));

        string LPStatus = "";
        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    LPStatus += "'Approved',";
                }
                else if (item.Text == "Maintenance")
                {
                    LPStatus += "'Maintenance',";
                }
                else if (item.Text == "Inactive")
                {
                    LPStatus += "'Inactive',";
                }
            }
        }
        LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
        string StrStat = "SELECT LookupId FROM Lookup WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")";
        DataTable LPStat = ObjData.ReturnDataTable(StrStat, false);
        string StatusId = "";
        for (int i = 0; i < LPStat.Rows.Count; i++)
        {
            StatusId += LPStat.Rows[i]["LookupId"].ToString() + ",";
        }
        getAllclassicViewData(StartDate, enddate, sess.StudentId, LessonId, StatusId);
        getDSSessionData(sess.StudentId.ToString(), StartDate, enddate, LessonId);
        getDSEventSessionZero(sess.StudentId.ToString(), StartDate, enddate, LessonId);
        getDSScore(sess.StudentId.ToString(), StartDate, enddate, LessonId);
        getDSSessionDataZero(sess.StudentId.ToString(), StartDate, enddate);
        getDSScoreZero(sess.StudentId.ToString(), StartDate, enddate);
        //ViewState["alldata"] = allclsviewdata;
        //ViewState["SessionData"] = DSSessionDatadt;
        //ViewState["SessionDataZero"] = DSSessionDataZerodt;
        //ViewState["EventSessionZero"] = DSEventSessionZerodt;
        //ViewState["Score"] = DSScoredt;
        //ViewState["ScoreZero"] = DSScoreZerodt;

        ViewState["alldata"] = DataTableToJson(allclsviewdata);
         ViewState["SessionData"] = DataTableToJson(DSSessionDatadt);
        ViewState["SessionDataZero"]  = DataTableToJson(DSSessionDataZerodt);
        ViewState["EventSessionZero"] = DataTableToJson(DSEventSessionZerodt);
         ViewState["Score"] = DataTableToJson(DSScoredt);
         ViewState["ScoreZero"] = DataTableToJson(DSScoreZerodt);

        int[] lessonids = allclsviewdata.AsEnumerable()
                    .Select(row => row.Field<int>("LessonPlanId"))
                    .Distinct()
                    .ToArray();

        countlesson = lessonids.Length;
        clsLoadRptLesson(lessonids);
        clsview.Visible = true;
        Gvclsdate.Visible = true;
        

    }
    public void getAllclassicViewData(String Sdate, String Edate, int studid, string lessonid,string lpstatus)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("DSProgressRptSummary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", Sdate);
            cmd.Parameters.AddWithValue("@EndDate", Edate);
            cmd.Parameters.AddWithValue("@StudentId", studid);
            cmd.Parameters.AddWithValue("@LessonId", lessonid);
            cmd.Parameters.AddWithValue("@LPStatus", lpstatus);
            da = new SqlDataAdapter(cmd);
            da.Fill(allclsviewdata);
            
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
    private void selectcolumntype(string id)
    {
        columntypes = allclsviewdata.AsEnumerable()
                                       .Where(row => row.Field<int>("LessonPlanId") ==Convert.ToInt32(id))
                                       .Select(row => row.Field<string>("ColName") + "-" + row.Field<string>("CalcType"))
                                       .Distinct()
                                       .ToArray();
         coltypedict = allclsviewdata.AsEnumerable()
    .Where(row => row.Field<int>("LessonPlanId") == Convert.ToInt32(id))
    .Select(row => new
    {
        Key = row.Field<string>("ColName") + "-" + row.Field<string>("CalcType"),
        Value = row.Field<string>("ColName") + "-<br/>" + row.Field<string>("CalcType")
    })
    .Distinct()
    .ToDictionary(x => x.Key, x => x.Value);

         for (int i = 0; i < columntypes.Length; i++)
         {
             columntypesdict[columntypes[i]] = "Subcolumn" + i+1;
         }

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
        string enddate = dted.ToString("yyyy-MM-dd") + " 23:59:59.999";     


        RV_ExcelReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);


        RV_ExcelReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ProgressSummaryReport"];

        string LessonId = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {   
            if (item.Selected == true)
            {
                LessonId += item.Value + ",";
            }
        }
        LessonId = LessonId.Substring(0, (LessonId.Length - 1));

        string LPStatus = "";
        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    LPStatus += "'Approved',";
                }
                else if (item.Text == "Maintenance")
                {
                    LPStatus += "'Maintenance',";
                }
                else if (item.Text == "Inactive")
                {
                    LPStatus += "'Inactive',";
                }
            }
        }
        LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
        string StrStat = "SELECT LookupId FROM Lookup WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")";
        DataTable LPStat = ObjData.ReturnDataTable(StrStat, false);
        string StatusId = "";
        for (int i = 0; i < LPStat.Rows.Count; i++)
        {
            StatusId += LPStat.Rows[i]["LookupId"].ToString() + ",";
        }

        //}
        RV_ExcelReport.ShowParameterPrompts = false;

        ReportParameter[] parm = new ReportParameter[6];
        parm[0] = new ReportParameter("StudentId", sess.StudentId.ToString());
        parm[1] = new ReportParameter("StartDate", StartDate.ToString());
        parm[2] = new ReportParameter("EndDate", enddate.ToString());
        parm[3] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
        parm[4] = new ReportParameter("LessonId", LessonId.ToString());
        parm[5] = new ReportParameter("LPStatus", StatusId.ToString());

        this.RV_ExcelReport.ServerReport.SetParameters(parm);
        RV_ExcelReport.ServerReport.Refresh();

    }

    private bool Validation()
    {
        bool result = true;
        int count = 0;
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {
            if (item.Selected == true)
            {
                count++;
            }
        }
        if (txtRepStart.Text == "")
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Start Date");
            return result;
        }
        else if (txtrepEdate.Text == "")
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter End Date");
            return result;
        }
        else if (count == 0)
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select lesson plan");
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
    private void LoadLessonPlan()
    {
        ObjData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            try
            {
                DataTable dtLP = new DataTable();
                dtLP.Columns.Add("Id", typeof(string));
                dtLP.Columns.Add("Name", typeof(string));

                string StatusCheck = "";
                string APqry="";
                string ap_end="";

                foreach (ListItem item in chkStatus.Items)
                {
                    if (item.Selected == true)
                    {
                        td1.Visible = false;
                        if (item.Text == "Active")
                        {
                            StatusCheck += " LookupName='Approved' ";
                            APqry = "CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved') " + "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved')+' (active)' ";
                            ap_end=" END";
                        }
                        else if (item.Text == "Maintenance")
                        {
                            if (StatusCheck != "")
                            {
                                StatusCheck += " OR ";
                                APqry+= " ELSE ";
                                
                            }
                            StatusCheck += " LookupName='Maintenance' ";
                                APqry +="CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance') "+
                                 "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance') + '( Maintenance)'";
                             ap_end+=" END";
                        }
                        else if (item.Text == "Inactive")
                        {
                            if (StatusCheck != "")
                            {
                                StatusCheck += " OR ";
                                  APqry+= " ELSE ";
                            }
                            StatusCheck += " LookupName='Inactive' ";
                            APqry +="CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) "+ 
	                        "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) + ' (inactive)'";
                       ap_end +=" END";
                        }
                    }
                }
                DataTable DTLesson = new DataTable();
                if (StatusCheck != "")
                {
                    DTLesson = ObjData.ReturnDataTable("SELECT *," + APqry + ap_end +

        " AS LessonPlanName " +
     "FROM (SELECT DISTINCT hdr.LessonPlanId,hdr.LessonOrder,hdr.studentId FROM DSTempHdr hdr INNER JOIN LookUp ON hdr.StatusId=LookUp.LookupId WHERE StudentId=" + sess.StudentId + "  AND ( " + StatusCheck + "  )) LSN ORDER BY lsn.LessonOrder", false);
                }
                
            /*    DataTable DTLesson = ObjData.ReturnDataTable("SELECT *,(SELECT TOP 1 DSTemplateName +' ( '+CASE WHEN LookupName='Approved' THEN 'Active' ELSE LookupName END+' )' " +
                    "FROM DSTempHdr INNER JOIN LookUp ON DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND " +
                    "( " + StatusCheck + " ) ORDER BY DSTempHdrId DESC) AS LessonPlanName FROM (SELECT DISTINCT DTmp.LessonPlanId,DTmp.LessonOrder FROM DSTempHdr DTmp " +
                    "INNER JOIN LookUp ON DTmp.StatusId=LookUp.LookupId WHERE StudentId=" + sess.StudentId + " AND ( " + StatusCheck + " )) LSN ORDER BY  LessonOrder", false);
 */
 
                if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {
                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtLP.NewRow();
                            drr["Id"] = drLessn.ItemArray[0];
                            drr["Name"] = drLessn.ItemArray[3];
                            dtLP.Rows.Add(drr);
                        }
                    }
                }
                else
                {
                    td1.Visible = true;
                    td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
                }

                ddlLessonplan.DataSource = dtLP;
                ddlLessonplan.DataTextField = "Name";
                ddlLessonplan.DataValueField = "Id";
                ddlLessonplan.DataBind();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    protected void chkStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLessonPlan();
        RV_ExcelReport.Visible = false;
        dlLesson.Visible = false;
        rbtnClassType.Visible = false;
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        rbtnClassType.Visible = false;
        tableDurFormat.Visible = false;
        radBtnDurationFormat.SelectedValue = "0";
        rbtnClassType.SelectedIndex = 2; 
        txtRepStart.Text = string.Empty;
        txtrepEdate.Text = string.Empty;
        chkStatus.Visible = true;
        chkStatus.Items[0].Selected = true;
        chkStatus.Items[1].Selected = true;
        chkStatus.Items[2].Selected = false;
        LoadLessonPlan();
        RV_ExcelReport.Visible = false;
        dlLesson.Visible = false;
        td1.Visible = false;
        tdMsg.Visible = false;
        Button1.Visible = true;
        Button2.Visible = true;
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validation() == true)
            {
                RV_ExcelReport.Visible = false;
                rbtnClassType.Visible = true;
                dlLesson.Visible = true;
                LoadRptLesson();
                chkStatus.Visible = true;
                btnExport.Visible = true;
                tdMsg.Visible = false;
                td1.Visible = false;
                tableDurFormat.Visible = true;
            }
            else
            {
                RV_ExcelReport.Visible = false;
                dlLesson.Visible = false;
                tdMsg.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public void LoadRptLesson()
    {
        DataSet ds = LoadData();
        if (ds != null)
        {
            dlLesson.DataSource = ds;
            dlLesson.DataBind();
        }
        else
        {
            td1.Visible = true;
            td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
        }
    }
    public DataTable  clsLoadRptLessonexport(string[] lessoncount)
    {
        DataSet ds = LoadData();
        DataTable dt = ds.Tables[0];
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            DataRow row = dt.Rows[i];
            if (!lessoncount.Contains(row["LessonPlanId"].ToString()))
            {
                dt.Rows.RemoveAt(i);
            }
        }
        return dt;
    }

    public void clsLoadRptLesson(int[] lessoncount)
    {
        DataSet ds = LoadData();
        DataTable dt = ds.Tables[0];
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            DataRow row = dt.Rows[i];
            if (!lessoncount.Contains(Convert.ToInt32(row["LessonPlanId"])))
            {
                dt.Rows.RemoveAt(i);
            }
        }
        if (dt.Rows.Count == 0)
        {
            td1.Visible = true;
            td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
        }
        else
        {
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
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void btnExport_Click1(object sender, ImageClickEventArgs e)
    {
        if (highcheck.Checked == false)
        {
            sess = (clsSession)Session["UserSession"];
            Response.Clear();
            string filename = sess.StudentName + "_ProgressSummaryReport.xls";
            string enCodeFileName = Server.UrlEncode(filename);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + enCodeFileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            dlLesson.RenderControl(hw);
            Response.Write(sw.ToString());
            Response.End();
        }
        else
        {
            ObjData = new clsData();
            tdMsg.InnerHtml = "";
            RV_ExcelReport.Visible = false;
            sess = (clsSession)Session["UserSession"];
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd") + " 23:59:59.999";

            string LessonId = "";
            foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
            {
                if (item.Selected == true)
                {
                    LessonId += item.Value + ",";
                }
            }
            LessonId = LessonId.Substring(0, (LessonId.Length - 1));

            string LPStatus = "";
            foreach (ListItem item in chkStatus.Items)
            {
                if (item.Selected == true)
                {
                    if (item.Text == "Active")
                    {
                        LPStatus += "'Approved',";
                    }
                    else if (item.Text == "Maintenance")
                    {
                        LPStatus += "'Maintenance',";
                    }
                    else if (item.Text == "Inactive")
                    {
                        LPStatus += "'Inactive',";
                    }
                }
            }
            LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
            string StrStat = "SELECT LookupId FROM Lookup WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")";
            DataTable LPStat = ObjData.ReturnDataTable(StrStat, false);
            string StatusId = "";
            for (int i = 0; i < LPStat.Rows.Count; i++)
            {
                StatusId += LPStat.Rows[i]["LookupId"].ToString() + ",";
            }
            //getAllclassicViewData(StartDate, enddate, sess.StudentId, LessonId, StatusId);
            //getDSSessionData(sess.StudentId.ToString(), StartDate, enddate, LessonId);
            //getDSEventSessionZero(sess.StudentId.ToString(), StartDate, enddate, LessonId);
            //getDSScore(sess.StudentId.ToString(), StartDate, enddate, LessonId);
            //getDSSessionDataZero(sess.StudentId.ToString(), StartDate, enddate);
            //getDSScoreZero(sess.StudentId.ToString(), StartDate, enddate);
            //allclsviewdata = (DataTable)ViewState["alldata"];
            allclsviewdata = JsonToDataTable(ViewState["alldata"].ToString());
            string[] lessonids = allclsviewdata.AsEnumerable()
                        .Select(row => row.Field<string>("LessonPlanId"))
                        .Distinct()
                        .ToArray();

            countlesson = lessonids.Length;
            allless = clsLoadRptLessonexport(lessonids);
            if (allless.Rows.Count > 0)
            {
                ViewState["alless"] = DataTableToJson(allless);
                //ViewState["allless"] = allless;
                exportgrid.Visible = true;
                this.BindGrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available');", true);


            }
        }
    }
   
    protected void dlLesson_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Session["e"] = e;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            oData_ov = new clsData();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd") + " 23:59:59.997";
            GridView gVSession = (e.Item.FindControl("gVSession") as GridView);
            string LessonId = (e.Item.FindControl("LessonId") as Label).Text;
            
            DataSet ds1 = oData_ov.ReturnDataSet("select LookupName,lookupid  from Lookup WHERE LookupType='TemplateStatus' AND LookupName IN('Approved','Maintenance','Inactive') order by LookupName", false);
            string aprvd ="-33"; 
            string maintance = "-33"; 
            string inactive = "-33"; 
            string LPStatus = "";
            int timestatus = 0;
            foreach (ListItem item in chkStatus.Items)
            {
                if (item.Selected == true)
                {
                    if (item.Text == "Active")
                    {
                        LPStatus += "Approved";
                        aprvd = ds1.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();
                    }
                    else if (item.Text == "Maintenance")
                    {
                        if (LPStatus != "")
                        {
                            LPStatus += ",";
                        }
                        LPStatus += "Maintenance";
                        maintance = ds1.Tables[0].Rows[2].ItemArray.GetValue(1).ToString();

                    }
                    else if (item.Text == "Inactive")
                    {
                        if (LPStatus != "")
                        {
                            LPStatus += ",";
                        }
                        LPStatus += "Inactive";
                        inactive = ds1.Tables[0].Rows[1].ItemArray.GetValue(1).ToString();

                    }
                }
            }
          
            //Dt = ObjData.Execute_PSR_Data(LessonId, LPStatus, StartDate, enddate, Convert.ToInt32(sess.StudentId));
            SqlDataAdapter DAdap = null;
            SqlCommand cmd = null;
            DataTable Dt = new DataTable();
            timestatus = Convert.ToInt32(radBtnDurationFormat.SelectedValue);
            SqlConnection con = ObjData.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("PSR_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@ENDDate", enddate);
                cmd.Parameters.AddWithValue("@Studentid", sess.StudentId);
                cmd.Parameters.AddWithValue("@LessonPlanId", LessonId);
                cmd.Parameters.AddWithValue("@LPStatus", LPStatus);
                cmd.Parameters.AddWithValue("@LessonType", rbtnClassType.SelectedValue);
                cmd.Parameters.AddWithValue("@Timestatus", timestatus);
                da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            finally
            {
                ObjData.Close(con);
            }
            gVSession.DataSource = Dt;
            gVSession.DataBind();
        }
    }
    protected void DataListexport_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            oData_ov = new clsData();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd") + " 23:59:59.998";
            GridView gVexport = (e.Item.FindControl("gVSessionexport") as GridView);
            string LessonId = (e.Item.FindControl("LessonId") as Label).Text;

            string LPStatus = "";
            int timestatus = 0;
            foreach (ListItem item in chkStatus.Items)
            {
                if (item.Selected == true)
                {
                    if (item.Text == "Active")
                    {
                        LPStatus += "Approved";
                    }
                    else if (item.Text == "Maintenance")
                    {
                        if (LPStatus != "")
                        {
                            LPStatus += ",";
                        }
                        LPStatus += "Maintenance";
                    }
                    else if (item.Text == "Inactive")
                    {
                        if (LPStatus != "")
                        {
                            LPStatus += ",";
                        }
                        LPStatus += "Inactive";
                    }
                }
            }
            //Dt = ObjData.Execute_PSR_Data(LessonId, LPStatus, StartDate, enddate, Convert.ToInt32(sess.StudentId));

            SqlDataAdapter DAdap = null;
            SqlCommand cmd = null;
            DataTable Dt = new DataTable();
            timestatus = Convert.ToInt32(radBtnDurationFormat.SelectedValue);
            SqlConnection con = ObjData.Open();
         
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("PSR_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@ENDDate", enddate);
                cmd.Parameters.AddWithValue("@Studentid", sess.StudentId);
                cmd.Parameters.AddWithValue("@LessonPlanId", LessonId);
                cmd.Parameters.AddWithValue("@LPStatus", LPStatus);
                cmd.Parameters.AddWithValue("@LessonType", rbtnClassType.SelectedValue);
                cmd.Parameters.AddWithValue("@Timestatus", timestatus);
            //    String Studname = sess.StudentName.Split(',')[0]+ "  " + sess.StudentName.Split(',')[0];


                da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            finally
            {
                ObjData.Close(con);
            }
            gVexport.GridLines = GridLines.Both;
            gVexport.DataSource = Dt;
            gVexport.DataBind();
        }
    }

    protected DataSet LoadData()
    {
        sess = (clsSession)Session["UserSession"];
        oData_ov = new clsData();
        string LPStatus = "";
        string APqry = "";
        string ap_end = "";
        Dt = new System.Data.DataTable();
        ObjData = new clsData();

        ///////
        //foreach (ListItem item in chkStatus.Items)
        //{
        //    if (item.Selected == true)
        //    {
        //        if (item.Text == "Active")
        //        {
        //            StatusCheck += " LookupName='Approved' ";
        //            APqry = "CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved') " + "THEN (SELECT DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved')+' (active)' ";
        //            ap_end = " END";
        //        }
        //        else if (item.Text == "Maintenance")
        //        {
        //            if (StatusCheck != "")
        //            {
        //                StatusCheck += " OR ";
        //                APqry += " ELSE ";

        //            }
        //            StatusCheck += " LookupName='Maintenance' ";
        //            APqry += "CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance') " +
        //             "THEN (SELECT DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance') + '( Maintenance)'";
        //            ap_end += " END";
        //        }
        //        else if (item.Text == "Inactive")
        //        {
        //            if (StatusCheck != "")
        //            {
        //                StatusCheck += " OR ";
        //                APqry += " ELSE ";
        //            }
        //            StatusCheck += " LookupName='Inactive' ";
        //            APqry += "CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) " +
        //            "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) + ' (inactive)'";
        //            ap_end += " END";
        //        }
        //    }
        //}

        //DataTable DTLesson = ObjData.ReturnDataTable("SELECT *," + APqry + ap_end +

//" AS LessonPlanName " +
//"FROM (SELECT DISTINCT hdr.LessonPlanId,hdr.LessonOrder,hdr.studentId FROM DSTempHdr hdr INNER JOIN LookUp ON hdr.StatusId=LookUp.LookupId WHERE StudentId=" + sess.StudentId + "  AND ( " + StatusCheck + "  )) LSN ORDER BY lsn.LessonOrder", false);


        //////
        
        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    LPStatus += " 'Approved' ";
                    APqry = "CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved') " + "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved') ";
                    ap_end = " END";
                }
                else if (item.Text == "Maintenance")
                {
                    if (LPStatus != "")
                    {
                        LPStatus += ",";
                        APqry += " ELSE ";
                    }
                    LPStatus += " 'Maintenance' ";
                    APqry += "CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance') " +
                 "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance') ";
                    ap_end += " END";
                }
                else if (item.Text == "Inactive")
                {
                    if (LPStatus != "")
                    {
                        LPStatus += ",";
                        APqry += " ELSE ";
                    }
                    LPStatus += " 'Inactive' ";
                    APqry += "CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) " +
                 "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC)";
                    ap_end += " END";
                }
            }
        }



        string LessonId = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {
            if (item.Selected == true)
            {
                LessonId += item.Value + ",";
            }
        }
        LessonId = LessonId.Substring(0, (LessonId.Length - 1));

        string sqlStr = "SELECT *,"+ APqry + ap_end +" AS LessonName " +
            "FROM (SELECT LessonPlanId,LessonOrder,studentid FROM DSTempHdr DS INNER JOIN LookUp LU ON DS.StatusId=LU.LookupId WHERE DS.StudentId=" + sess.StudentId + " AND DS.SchoolId=" + sess.SchoolId + " AND " +
            "LU.LookupName IN (" + LPStatus + ") AND DS.LessonPlanId IN ( " + LessonId + ") GROUP BY   DS.StudentId,DS.LessonPlanId,DS.LessonOrder) LSN ORDER BY LessonOrder";

        //string sqlStr = "SELECT *,(SELECT TOP 1 DSTemplateName FROM DSTempHdr DH INNER JOIN LookUp L ON DH.StatusId=L.LookupId WHERE DH.LessonPlanId=LSN.LessonPlanId AND " +
        //         "DH.StudentId=" + sess.StudentId + " AND L.LookupName IN (" + LPStatus + ") ORDER BY DH.DSTempHdrId DESC) AS LessonName " +
        //         "FROM (SELECT LessonPlanId,LessonOrder FROM DSTempHdr DS INNER JOIN LookUp LU ON DS.StatusId=LU.LookupId WHERE DS.StudentId=" + sess.StudentId + " AND DS.SchoolId=" + sess.SchoolId + " AND " +
        //         "LU.LookupName IN (" + LPStatus + ") AND DS.LessonPlanId IN ( " + LessonId + ") GROUP BY DS.LessonPlanId,DS.LessonOrder) LSN ORDER BY LessonOrder";
   
        DataSet ds = oData_ov.ReturnDataSet(sqlStr, false);
        return ds;
    }

    protected void clslist_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {

            Gvclsdate.DataSource = GetDataForDate();
            Gvclsdate.DataBind();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                countdatalist++;
                ObjData = new clsData();
                sess = (clsSession)Session["UserSession"];
                DataSet ds = new DataSet();
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string StartDate = dtst.ToString("yyyy-MM-dd");
                string enddate = dted.ToString("yyyy-MM-dd");
                GridView less = (e.Item.FindControl("Gvclsless") as GridView);
                string lessid = (e.Item.FindControl("lessid") as Label).Text;
                selectcolumntype(lessid);
                DataRowView rowData = (DataRowView)e.Item.DataItem;
                DataTable dt = getClsDataBylessId(lessid, StartDate, enddate, rowData);
                less.Columns.Clear();
                less.AutoGenerateColumns = false;
                foreach (var type in columntypes)
                {
                    string item = "";
                    if (columntypesdict.ContainsKey(type))
                    {
                        item = columntypesdict[type];
                    }
                    TemplateField templateField = new TemplateField();
                    string textval = type;
                    if (coltypedict.ContainsKey(type))
                    {
                        textval = coltypedict[type];
                    }
                    templateField.HeaderText = "<div style='border-bottom: 1px solid black;'>Row Data</div>" + textval;
                    templateField.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                    templateField.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#4498c2");
                    templateField.HeaderStyle.CssClass = "nowrapText";
                    templateField.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                    templateField.HeaderStyle.VerticalAlign = VerticalAlign.Top;
                    templateField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    templateField.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, item);
                    less.Columns.Add(templateField);
                }
                TemplateField templateFieldtime = new TemplateField();
                templateFieldtime.HeaderText = "Time";
                templateFieldtime.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldtime.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");
                templateFieldtime.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldtime.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldtime.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldtime.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Time");
                templateFieldtime.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFieldtime);
                TemplateField templateFielduser = new TemplateField();
                templateFielduser.HeaderText = "User";
                templateFielduser.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFielduser.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFielduser.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");
                templateFielduser.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFielduser.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFielduser.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "User");
                templateFielduser.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFielduser);
                TemplateField templateFieldset = new TemplateField();
                templateFieldset.HeaderText = "Set";
                templateFieldset.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldset.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");
                templateFieldset.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldset.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldset.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldset.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Set");
                templateFieldset.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFieldset);
                TemplateField templateFieldprompt = new TemplateField();
                templateFieldprompt.HeaderText = "Prompt";
                templateFieldprompt.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldprompt.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");
                templateFieldprompt.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldprompt.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldprompt.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldprompt.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Prompt");
                templateFieldprompt.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFieldprompt);
                TemplateField templateFieldEventName = new TemplateField();
                templateFieldEventName.HeaderText = "EventName";
                templateFieldEventName.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldEventName.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#dbe4c0");
                templateFieldEventName.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldEventName.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldEventName.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldEventName.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "EventName");
                templateFieldEventName.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFieldEventName);
                TemplateField templateFieldStdtSessEventType = new TemplateField();
                templateFieldStdtSessEventType.HeaderText = "StdtSessEventType";
                templateFieldStdtSessEventType.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldStdtSessEventType.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#dbe4c0");
                templateFieldStdtSessEventType.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldStdtSessEventType.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldStdtSessEventType.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldStdtSessEventType.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "StdtSessEventType");
                templateFieldStdtSessEventType.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFieldStdtSessEventType);
                TemplateField templateFieldStdtdate = new TemplateField();
                templateFieldStdtdate.HeaderText = "Date";
                templateFieldStdtdate.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldStdtdate.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldStdtdate.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldStdtdate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldStdtdate.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "PeriodDate");
                templateFieldStdtdate.ItemStyle.CssClass = "nowrapText";
                less.Columns.Add(templateFieldStdtdate);
                if (columntypes.Length == 0)
                {
                    e.Item.Visible = false;
                    less.Visible = false;
                }
                less.DataSource = dt;
                less.DataBind();
                AddCustomHeader(less);
                ((DataControlField)less.Columns.Cast<DataControlField>()
                        .Where(fld => (fld.HeaderText == "Date"))
                        .SingleOrDefault()).Visible = false;
                if (countdatalist == countlesson)
                {
                    daterow(less);
                    setDatesize();
                }
            }
        }
        catch (Exception ex) {
            throw ex;
        
        }
    }

         private void setDatesize() 
         {
             foreach (DataListItem item in clslist.Items)
             {
                 if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                 {
                     GridView less = (GridView)item.FindControl("Gvclsless");
                     daterow(less);
                 }
             }
        }
         private void daterow(GridView less) {
             foreach (GridViewRow row in less.Rows)
             {
                 Label datelabel = (Label)row.FindControl("PeriodDate");
                 if (datelabel.Text != "")
                 {
                     DateTime datetime = DateTime.Parse(datelabel.Text);
                     string dateOnly = datetime.ToString("MM/dd/yyyy");
                     if (dateandcount.ContainsKey(dateOnly))
                     {
                         if (dateandcount[dateOnly] > 1)
                         {
                             int count = 30 + (dateandcount[dateOnly] * 15);
                             row.Style["height"] = count + "px";
                         }
                         else
                         {
                             row.Style["height"] = "40px";
                         }

                     }
                 }

             }
             foreach (GridViewRow row in Gvclsdate.Rows)
             {
                 Label datelabel = (Label)row.FindControl("dateid2");
                 if (datelabel.Text != "")
                 {
                     DateTime datetime = DateTime.Parse(datelabel.Text);
                     string dateOnly = datetime.ToString("MM/dd/yyyy");
                     if (dateandcount.ContainsKey(dateOnly))
                     {
                         if (dateandcount[dateOnly] > 1)
                         {
                             int count = 30 + (dateandcount[dateOnly] * 15);
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

    private void AddCustomHeader(GridView gv)
    {
        GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
       
        TableCell headerCell0 = new TableCell()
        {
            Text = "Datasheet Data",
            HorizontalAlign = HorizontalAlign.Center,
            ColumnSpan = columntypes.Length,
            BackColor = System.Drawing.ColorTranslator.FromHtml("#4498c2"),
            ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000"),
            CssClass = "nowrapText"
        };
        headerCell0.Font.Bold = true;
        TableCell headerCell1 = new TableCell()
        {
            Text = "Lesson Delivery - Sessions Data",
            HorizontalAlign = HorizontalAlign.Center,
            ColumnSpan = 4,
            BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD"),
            ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000"),
            CssClass = "nowrapText"

        };
       
        headerCell1.Font.Bold = true;
        TableCell headerCell2 = new TableCell()
        {
            Text = "Condition Lines/ Arrow Notes",
            HorizontalAlign = HorizontalAlign.Center,
            ColumnSpan = 2,
            BackColor = System.Drawing.ColorTranslator.FromHtml("#dbe4c0"),
            ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000"),
            CssClass = "nowrapText"

        };
        headerCell2.Font.Bold = true;
        TableCell headerCell3 = new TableCell()
        {
            Text = "date",
            HorizontalAlign = HorizontalAlign.Center,
            ColumnSpan = 1,
                        CssClass = "nowrapText",            Visible=false
        };
        headerCell3.Font.Bold = true;
        gvHeader.Cells.Add(headerCell0);
        gvHeader.Cells.Add(headerCell0);
        gvHeader.Cells.Add(headerCell1);
        gvHeader.Cells.Add(headerCell2);
        gvHeader.Cells.Add(headerCell3);
        gv.Controls[0].Controls.AddAt(0, gvHeader);
    }

    private void getDSEventSessionZero(string sid, string sdate, string enddate, string lessid)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("DSEventSessionZero", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", sdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            cmd.Parameters.AddWithValue("@StudentId", sid);
            cmd.Parameters.AddWithValue("@LessonId", lessid);
            da = new SqlDataAdapter(cmd);
            da.Fill(DSEventSessionZerodt);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + sid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }

    }


    private void getDSScore(string sid, string sdate, string enddate, string lessid)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("DSScore", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", sdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            cmd.Parameters.AddWithValue("@StudentId", sid);
            cmd.Parameters.AddWithValue("@LessonId", lessid);
            da = new SqlDataAdapter(cmd);
            da.Fill(DSScoredt);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + sid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }

    }

    private void getDSSessionData(string sid, string sdate, string enddate, string lessid)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("DSSessionData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", sdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            cmd.Parameters.AddWithValue("@StudentId", sid);
            cmd.Parameters.AddWithValue("@LessonId", lessid);
            da = new SqlDataAdapter(cmd);
            da.Fill(DSSessionDatadt);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + sid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }

    }

    private void getDSSessionDataZero(string sid, string sdate, string enddate)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("DSSessionDataZero", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", sdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            cmd.Parameters.AddWithValue("@StudentId", sid);
            da = new SqlDataAdapter(cmd);
            da.Fill(DSSessionDataZerodt);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + sid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }

    }


    private void getDSScoreZero(string sid, string sdate, string enddate)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("DSScoreZero", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", sdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            cmd.Parameters.AddWithValue("@StudentId", sid);
            da = new SqlDataAdapter(cmd);
            da.Fill(DSScoreZerodt);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + sid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }
    }
    protected DataTable getClsDataBylessId(string lessid, String StartDate, string enddate, DataRowView rowData)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("PeriodDate", typeof(DateTime));
        foreach (var type in columntypes)
        {
            string item = "";
            if (columntypesdict.ContainsKey(type))
            {
                item = columntypesdict[type];
            }
            dt.Columns.Add(item, typeof(string));
        }
        dt.Columns.Add("Time", typeof(string));
        dt.Columns.Add("User", typeof(string));
        dt.Columns.Add("Set", typeof(string));
        dt.Columns.Add("Prompt", typeof(string));
        dt.Columns.Add("EventName", typeof(string));
        dt.Columns.Add("StdtSessEventType", typeof(string));
             DateTime startDate = DateTime.Parse(StartDate);
        DateTime endDate = DateTime.Parse(enddate);
       
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            string time = "";
            string user = "";
            string set = "";
            string prompt = "";
            string evname = "";
            string evtype = "";
            string currdate = date.ToString("MM/dd/yyyy");
            DataRow row = dt.NewRow();
            Dictionary<string, string> scoresdict = new Dictionary<string, string>();
            if (allclsviewdata != null && allclsviewdata.Rows.Count > 0)
            {
                        for (int r1 = 0; r1 < DSSessionDataZerodt.Rows.Count; r1++)
                        {
                            if (DSSessionDataZerodt.Rows[r1]["SessPeriodDate"].ToString() == currdate)
                            {
                                time = time + "<br/>" + DSSessionDataZerodt.Rows[r1]["StartTs"].ToString();
                                user = user + "<br/>" + DSSessionDataZerodt.Rows[r1]["CurrentPrompt"].ToString();
                                set = set + "<br/>" + DSSessionDataZerodt.Rows[r1]["CurrentSet"].ToString();
                                prompt = prompt + "<br/>" + DSSessionDataZerodt.Rows[r1]["CurrentPrompt"].ToString();
                                evname = evname + "<br/>" + DSSessionDataZerodt.Rows[r1]["EventName"].ToString();
                                evtype = evtype + "<br/>" + DSSessionDataZerodt.Rows[r1]["StdtSessEventType"].ToString();
                            }
                        }

                        for (int r2 = 0; r2 < DSSessionDatadt.Rows.Count; r2++)
                        {
                            if (DSSessionDatadt.Rows[r2]["SessPeriodDate"].ToString() == currdate && DSSessionDatadt.Rows[r2]["LessonPlanId"].ToString() == lessid)
                            {
                                time = time + "<br/>" + DSSessionDatadt.Rows[r2]["StartTs"].ToString();
                                user = user + "<br/>" + DSSessionDatadt.Rows[r2]["UserName"].ToString();
                                set = set + "<br/>" + DSSessionDatadt.Rows[r2]["CurrentSet"].ToString();
                                prompt = prompt + "<br/>" + DSSessionDatadt.Rows[r2]["CurrentPrompt"].ToString();
                                evname = evname + "<br/>" + DSSessionDatadt.Rows[r2]["EventName"].ToString();
                                evtype = evtype + "<br/>" + DSSessionDatadt.Rows[r2]["StdtSessEventType"].ToString();
                            }
                        }
                        try
                        {
                            for (int i = 0; i < columntypes.Length; i++)
                            {
                                string score = "";
                                bool DSScoredtfound = false;
                                bool DSEventSessionZerodtfound = false;
                                for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                {
                                    if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate)
                                    {
                                        DSScoredtfound = true;
                                        break;
                                    }
                                }
                                if (DSScoredtfound)
                                {
                                    for (int j = 0; j < DSEventSessionZerodt.Rows.Count; j++)
                                    {
                                        if (DSEventSessionZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSEventSessionZerodt.Rows[j]["LessonPlanId"].ToString() == lessid)
                                        {
                                            DSEventSessionZerodtfound = true;
                                            break;
                                        }
                                    }
                                    if (DSEventSessionZerodtfound)
                                    {
                                        string[] parts = columntypes[i].Split('-');
                                        for (int j = 0; j < DSEventSessionZerodt.Rows.Count; j++)
                                        {
                                            if (DSEventSessionZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSEventSessionZerodt.Rows[j]["LessonPlanId"].ToString() == lessid)
                                            {
                                                score = score + "<br/>" + DSEventSessionZerodt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                        for (int j = 0; j < DSScoreZerodt.Rows.Count; j++)
                                        {
                                            if (DSScoreZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoreZerodt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoreZerodt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoreZerodt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoreZerodt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                        for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                        {
                                            if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoredt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoredt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoredt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoredt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] parts = columntypes[i].Split('-');
                                        for (int j = 0; j < DSScoreZerodt.Rows.Count; j++)
                                        {
                                            if (DSScoreZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoreZerodt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoreZerodt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoreZerodt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoreZerodt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                        for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                        {
                                            if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoredt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoredt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoredt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoredt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < DSEventSessionZerodt.Rows.Count; j++)
                                    {
                                        if (DSEventSessionZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSEventSessionZerodt.Rows[j]["LessonPlanId"].ToString() == lessid)
                                        {
                                            DSEventSessionZerodtfound = true;
                                            break;
                                        }
                                    }
                                    if (DSEventSessionZerodtfound)
                                    {
                                        string[] parts = columntypes[i].Split('-');
                                        for (int j = 0; j < DSEventSessionZerodt.Rows.Count; j++)
                                        {
                                            if (DSEventSessionZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSEventSessionZerodt.Rows[j]["LessonPlanId"].ToString() == lessid)
                                            {
                                                score = score + "<br/>" + DSEventSessionZerodt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                        for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                        {
                                            if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoredt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoredt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoredt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoredt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] parts = columntypes[i].Split('-');
                                        for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                        {
                                            if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoredt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoredt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoredt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoredt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                    }

                                }
                                if (DSScoredt.Rows.Count == 0)
                                {
                                    for (int j = 0; j < DSEventSessionZerodt.Rows.Count; j++)
                                    {
                                        if (DSEventSessionZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSEventSessionZerodt.Rows[j]["LessonPlanId"].ToString() == lessid)
                                        {
                                            DSEventSessionZerodtfound = true;
                                            break;
                                        }
                                    }
                                    if (DSEventSessionZerodtfound)
                                    {
                                        string[] parts = columntypes[i].Split('-');
                                        for (int j = 0; j < DSEventSessionZerodt.Rows.Count; j++)
                                        {
                                            if (DSEventSessionZerodt.Rows[j]["PeriodDate"].ToString() == currdate && DSEventSessionZerodt.Rows[j]["LessonPlanId"].ToString() == lessid)
                                            {
                                                score = score + "<br/>" + DSEventSessionZerodt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                        for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                        {
                                            if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoredt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoredt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoredt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoredt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] parts = columntypes[i].Split('-');
                                        for (int j = 0; j < DSScoredt.Rows.Count; j++)
                                        {
                                            if (DSScoredt.Rows[j]["PeriodDate"].ToString() == currdate && DSScoredt.Rows[j]["LessonPlanId"].ToString() == lessid && parts[1] == DSScoredt.Rows[j]["CalcType"].ToString() && parts[0] == DSScoredt.Rows[j]["ColName"].ToString())
                                            {
                                                score = score + "<br/>" + DSScoredt.Rows[j]["Score1"].ToString();
                                            }
                                        }
                                    }

                                }


                                if (!scoresdict.ContainsKey(columntypes[i]))
                                {
                                    scoresdict.Add(columntypes[i], score);
                                }
                            }
                        }
                        catch (Exception ex) {
                            throw ex;
                            Console.WriteLine("Error: " + ex);
                        }
                row["PeriodDate"] = currdate;
               for (int i = 0; i < scoresdict.Count; i++)
                {
                    string item = "";
                    if (columntypesdict.ContainsKey(scoresdict.ElementAt(i).Key))
                    {
                        item = columntypesdict[scoresdict.ElementAt(i).Key];
                    }
                    string name = item;
            string score = scoresdict.ElementAt(i).Value;
            row[name] = score;
                }
                row["Time"] = time;
                row["User"] = user;
                row["Set"] = set;
                row["Prompt"] = prompt;
                row["EventName"] = evname;
                row["StdtSessEventType"] = evtype;

            }
            dt.Rows.Add(row);
            string pattern = @"<br\s*/?>";
            int count = Regex.Matches(user, pattern, RegexOptions.IgnoreCase).Count;
            if (dateandcount.ContainsKey(currdate))
            {
                if (count > dateandcount[currdate])
                {
                    dateandcount[currdate] = count;
                }
            }
            else
            {
                dateandcount.Add(currdate, count);
            }
           
        }
            
        return dt;

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

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        string sname = sess.StudentName.ToString().Replace(",", "");
        string Filename = sname + "_ProgressSummaryReport.xls";
        Filename = Server.UrlEncode(Filename);
        string filePath = Server.MapPath("~/StudentBinder/ExportACPSR/" + Filename);
        if (File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Filename);
            Response.TransmitFile(filePath);
            Response.End();
            try
            {
                File.Delete(filePath);  
            }
            catch (Exception ex)
            {
              
            }
        }


    }
    private DataTable columntable(string lessid)
    {
        DataTable allcolumn = new DataTable();
        allcolumn.Columns.Add("lessid", typeof(string));
        allcolumn.Columns.Add("colname", typeof(string));
        allcolumn.Columns.Add("colid", typeof(string));

     
        var query = (from row in allclsviewdata.AsEnumerable()
                     where row.Field<string>("LessonPlanId") == lessid
                     select new
                     {
                         Id = row.Field<string>("LessonPlanId"),
                         Name = row.Field<string>("ColName") + "-</br> " + row.Field<string>("CalcType"),
                         colid = row.Field<string>("DstempSetColCalcId")
                     })
             .GroupBy(x => new { x.Id, x.Name })
             .Select(g => new
             {
                 g.Key.Id,
                 g.Key.Name,
                 colid = string.Join(", ", g.Select(x => x.colid).Distinct())
             })
             .ToList();

        foreach (var item in query)
        {
            allcolumn.Rows.Add(item.Id, item.Name, item.colid);
        }
        return allcolumn;
    }
    private void BindGrid()
    {
        exportgrid.Columns.Clear();
        int subcolumn = 0;
       DSSessionDatadt = JsonToDataTable(ViewState["SessionData"].ToString());
       DSSessionDataZerodt = JsonToDataTable(ViewState["SessionDataZero"].ToString());
       DSEventSessionZerodt = JsonToDataTable(ViewState["EventSessionZero"].ToString());
       DSScoredt = JsonToDataTable(ViewState["Score"].ToString());
       DSScoreZerodt = JsonToDataTable(ViewState["ScoreZero"].ToString());

       ViewState["allless"] = ""; ViewState["alldata"] = ""; ViewState["SessionData"] = "";
       ViewState["SessionDataZero"] = ""; ViewState["EventSessionZero"] = ""; ViewState["Score"] = "";
       ViewState["ScoreZero"] = "";
        DataTable allcolumn = new DataTable();
        allcolumn.Columns.Add("lessid", typeof(string));
        allcolumn.Columns.Add("colname", typeof(string));
        allcolumn.Columns.Add("subcol", typeof(int));
        allcolumn.Columns.Add("colid", typeof(string));
        foreach (DataRow rows in allless.Rows)
        {
            string lessid = rows["LessonPlanId"].ToString();
            DataTable colforless= columntable(lessid);
            foreach (DataRow rows1 in colforless.Rows)
            {
                subcolumn = subcolumn + 1;
               string col = "<div style='border-bottom: 1px solid black;'>Row Data</div>" + rows1["colname"].ToString();
               allcolumn.Rows.Add(lessid, col, subcolumn, rows1["colid"].ToString());
            }
            subcolumn = subcolumn + 1;
            allcolumn.Rows.Add(lessid, "Time", subcolumn,"1");
            subcolumn = subcolumn + 1;
            allcolumn.Rows.Add(lessid, "User", subcolumn,"2");
            subcolumn = subcolumn + 1;
            allcolumn.Rows.Add(lessid, "Set", subcolumn,"3");
            subcolumn = subcolumn + 1;
            allcolumn.Rows.Add(lessid, "Prompt", subcolumn,"4");
            
            subcolumn = subcolumn + 1;
            allcolumn.Rows.Add(lessid, "Name", subcolumn,"5");
            subcolumn = subcolumn + 1;
            allcolumn.Rows.Add(lessid, "Type", subcolumn,"6");

        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Date", typeof(string));
        //BoundField boundField1 = new BoundField();
        //boundField1.DataField = "Date";
        //boundField1.HeaderText = "Date";
        TemplateField templateField0 = new TemplateField();
        templateField0.HeaderText = "Date";
        templateField0.HeaderStyle.ForeColor = System.Drawing.Color.Black;
        //templateField.HeaderStyle.CssClass = "nowrapText";
        //templateField.HeaderStyle.BorderColor = System.Drawing.Color.Black;
        templateField0.HeaderStyle.VerticalAlign = VerticalAlign.Top;
        templateField0.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        templateField0.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        templateField0.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Date");
        templateField0.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0080FF");
        templateField0.ItemStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0080FF");
        exportgrid.Columns.Add(templateField0);
        foreach (DataRow rows in allcolumn.Rows)
        {

            TemplateField templateField = new TemplateField();
            templateField.HeaderText = rows["colname"].ToString();
            templateField.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            //templateField.HeaderStyle.CssClass = "nowrapText";
            //templateField.HeaderStyle.BorderColor = System.Drawing.Color.Black;
            templateField.HeaderStyle.VerticalAlign = VerticalAlign.Top;
            templateField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            templateField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            templateField.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Subcol" + rows["subcol"].ToString());
            if (templateField.HeaderText == "Time" || templateField.HeaderText == "User" || templateField.HeaderText == "Set" || templateField.HeaderText == "Prompt" || templateField.HeaderText == "Yes/No")
            {
                templateField.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");
            }
            else
            {
                if (templateField.HeaderText == "Name" || templateField.HeaderText == "Type")
                {
                    templateField.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#dbe4c0");
                    templateField.ItemStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#dbe4c0");
                }
                else
                {
                    templateField.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0080FF");
                }
            }

            exportgrid.Columns.Add(templateField);
            dt.Columns.Add("Subcol" + rows["subcol"].ToString(), typeof(string));
        }
        dt = getAllData(allcolumn, dt);
        exportgrid.DataBound += new EventHandler(SummaryBound);
        exportgrid.DataSource = dt;
        exportgrid.DataBind();


        ExportGridViewToExcel();
    }
    public void SummaryBound(object sender, EventArgs e)
    {

        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

        TableHeaderCell tec = new TableHeaderCell();
        tec.ColumnSpan = 1;
        tec.Text = "";
        tec.BackColor = ColorTranslator.FromHtml("#0080FF");
        row.Controls.Add(tec);
        GridViewRow subRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Normal);

        TableHeaderCell subTec0 = new TableHeaderCell();
        subTec0.ColumnSpan = 1;
        subTec0.Text = "";
        subTec0.BackColor = ColorTranslator.FromHtml("#0080FF");
        subRow.Controls.Add(subTec0);
        foreach (DataRow rows in allless.Rows)
        {
            string lessid = rows["LessonPlanId"].ToString();
            DataTable colforless = columntable(lessid);
            int len = colforless.Rows.Count;
            tec = new TableHeaderCell();
            tec.ColumnSpan = len + 6; 
            tec.Text = rows["LessonName"].ToString();
            tec.BackColor = ColorTranslator.FromHtml("#0080FF");
            row.Controls.Add(tec);

            TableHeaderCell subTec = new TableHeaderCell();
            subTec.ColumnSpan = len;
            subTec.Text = "Datasheet Data";
            subTec.BackColor = ColorTranslator.FromHtml("#0080FF");
            subRow.Controls.Add(subTec);

            TableHeaderCell subTec1 = new TableHeaderCell();
            subTec1.ColumnSpan = 4;
            subTec1.Text = "Lesson Delivery - Sessions Data	";
            subTec1.BackColor = ColorTranslator.FromHtml("#FFDEAD");
            subRow.Controls.Add(subTec1);

            TableHeaderCell subTec2 = new TableHeaderCell();
            subTec2.ColumnSpan = 2;
            subTec2.Text = "Condition Lines/ Arrow Notes";
            subTec2.BackColor = ColorTranslator.FromHtml("#dbe4c0");
            subRow.Controls.Add(subTec2);
        }

        exportgrid.HeaderRow.Parent.Controls.AddAt(0, row);
        exportgrid.HeaderRow.Parent.Controls.AddAt(1, subRow);


    }
  
    private DataTable getAllData(DataTable dt, DataTable findt)
    {
        DateTime dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string startDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        DateTime startDateTime = DateTime.Parse(startDate);
        DateTime endDateTime = DateTime.Parse(enddate);

        var sessionDataZeroDict = new Dictionary<string, DataRow[]>();
        foreach (DataRow row in DSSessionDataZerodt.Rows)
        {
            string date = row["SessPeriodDate"].ToString();
            if (!sessionDataZeroDict.ContainsKey(date))
            {
                sessionDataZeroDict[date] = new DataRow[0]; 
            }
            sessionDataZeroDict[date] = sessionDataZeroDict[date].Concat(new[] { row }).ToArray();
        }

        var sessionDataDict = new Dictionary<string, DataRow[]>();
        foreach (DataRow row in DSSessionDatadt.Rows)
        {
            string date = row["SessPeriodDate"].ToString();
            if (!sessionDataDict.ContainsKey(date))
            {
                sessionDataDict[date] = new DataRow[0]; 
            }
            sessionDataDict[date] = sessionDataDict[date].Concat(new[] { row }).ToArray();
        }

        var eventSessionZeroDict = new Dictionary<string, DataRow[]>();
        foreach (DataRow row in DSEventSessionZerodt.Rows)
        {
            string date = row["PeriodDate"].ToString();
            if (!eventSessionZeroDict.ContainsKey(date))
            {
                eventSessionZeroDict[date] = new DataRow[0]; 
            }
            eventSessionZeroDict[date] = eventSessionZeroDict[date].Concat(new[] { row }).ToArray();
        }

        var scorezeroDict = new Dictionary<string, DataRow[]>();
        foreach (DataRow row in DSScoreZerodt.Rows)
        {
            string date = row["PeriodDate"].ToString();
            if (!scorezeroDict.ContainsKey(date))
            {
                scorezeroDict[date] = new DataRow[0]; 
            }
            scorezeroDict[date] = scorezeroDict[date].Concat(new[] { row }).ToArray();
        }

        var scoreDict = new Dictionary<string, DataRow[]>();
        foreach (DataRow row in DSScoredt.Rows)
        {
            string date = row["PeriodDate"].ToString();
            if (!scoreDict.ContainsKey(date))
            {
                scoreDict[date] = new DataRow[0]; 
            }
            scoreDict[date] = scoreDict[date].Concat(new[] { row }).ToArray();
        }

        for (DateTime date = startDateTime; date <= endDateTime; date = date.AddDays(1))
        {
            DataRow newRow = findt.NewRow();
            string currDate = date.ToString("MM/dd/yyyy");
            newRow["Date"] = currDate;

            foreach (DataRow row in dt.Rows)
            {
                string subcol = "Subcol" + row["subcol"].ToString();
                string lessid = row["lessid"].ToString();
                StringBuilder valueBuilder = new StringBuilder();
                string colId = row["colid"].ToString();
                string[] colIds = row["colid"].ToString().Split(',');
                string fullcolname = row["colname"].ToString();
                string pattern = @"<div[^>]*>(.*?)<\/div>(.*?)<\/br>\s*(.*)";
                Regex regex = new Regex(pattern);
                string colname = ""; string calctype = "";
                Match match = regex.Match(fullcolname);
                if (match.Success)
                {
                    colname = match.Groups[2].Value.Split('-')[0].Trim();
                     calctype = match.Groups[3].Value.Trim();
                }
          
                    if (colId == "1") 
                    {
                        if (sessionDataZeroDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataZeroDict[currDate])
                            {
                                valueBuilder.Append("<br/>").Append(sessionRow["StartTs"].ToString());
                            }
                        }

                        if (sessionDataDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataDict[currDate])
                            {
                                if (sessionRow["LessonPlanId"].ToString() == lessid)
                                {
                                    valueBuilder.Append("<br/>").Append(sessionRow["StartTs"].ToString());
                                }
                            }
                        }
                    }
                    else if (colId == "2") 
                    {
                        if (sessionDataZeroDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataZeroDict[currDate])
                            {
                                valueBuilder.Append("<br/>").Append(sessionRow["CurrentPrompt"].ToString());
                            }
                        }

                        if (sessionDataDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataDict[currDate])
                            {
                                if (sessionRow["LessonPlanId"].ToString() == lessid)
                                {
                                    valueBuilder.Append("<br/>").Append(sessionRow["UserName"].ToString());
                                }
                            }
                        }
                    }
                    else if (colId == "3") 
                    {
                        if (sessionDataZeroDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataZeroDict[currDate])
                            {
                                valueBuilder.Append("<br/>").Append(sessionRow["CurrentSet"].ToString());
                            }
                        }

                        if (sessionDataDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataDict[currDate])
                            {
                                if (sessionRow["LessonPlanId"].ToString() == lessid)
                                {
                                    valueBuilder.Append("<br/>").Append(sessionRow["CurrentSet"].ToString());
                                }
                            }
                        }
                    }
                    else if (colId == "4") 
                    {
                        if (sessionDataZeroDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataZeroDict[currDate])
                            {
                                valueBuilder.Append("<br/>").Append(sessionRow["CurrentPrompt"].ToString());
                            }
                        }

                        if (sessionDataDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataDict[currDate])
                            {
                                if (sessionRow["LessonPlanId"].ToString() == lessid)
                                {
                                    valueBuilder.Append("<br/>").Append(sessionRow["CurrentPrompt"].ToString());
                                }
                            }
                        }
                    }
                    else if (colId == "5") 
                    {
                        if (sessionDataZeroDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataZeroDict[currDate])
                            {
                                valueBuilder.Append("<br/>").Append(sessionRow["EventName"].ToString());
                            }
                        }

                        if (sessionDataDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataDict[currDate])
                            {
                                if (sessionRow["LessonPlanId"].ToString() == lessid)
                                {
                                    valueBuilder.Append("<br/>").Append(sessionRow["EventName"].ToString());
                                }
                            }
                        }
                    }
                    else if (colId == "6")
                    {
                        if (sessionDataZeroDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataZeroDict[currDate])
                            {
                                valueBuilder.Append("<br/>").Append(sessionRow["StdtSessEventType"].ToString());
                            }
                        }

                        if (sessionDataDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in sessionDataDict[currDate])
                            {
                                if (sessionRow["LessonPlanId"].ToString() == lessid)
                                {
                                    valueBuilder.Append("<br/>").Append(sessionRow["StdtSessEventType"].ToString());
                                }
                            }
                        }
                    }
                    else
                    {

                        bool DSScoredtfound = false;
                        bool Dseventsessionzerodtfound = false;

                        if (scoreDict.ContainsKey(currDate))
                        {
                            foreach (DataRow sessionRow in scoreDict[currDate])
                            {
                                DSScoredtfound = true;
                                break;
                            }
                        }

                        if (DSScoredtfound)
                        {
                            if (eventSessionZeroDict.ContainsKey(currDate))
                            {
                                foreach (DataRow sessionRow in eventSessionZeroDict[currDate])
                                {
                                    if (sessionRow["LessonPlanId"].ToString() == lessid)
                                    {
                                        Dseventsessionzerodtfound = true;
                                        break;
                                    }
                                }
                            }
                            if (Dseventsessionzerodtfound)
                            {
                                if (eventSessionZeroDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in eventSessionZeroDict[currDate])
                                    {
                                        if (sessionRow["LessonPlanId"].ToString() == lessid)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                                if (scorezeroDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scorezeroDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                                if (scoreDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scoreDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (scorezeroDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scorezeroDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                                if (scoreDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scoreDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (eventSessionZeroDict.ContainsKey(currDate))
                            {
                                foreach (DataRow sessionRow in eventSessionZeroDict[currDate])
                                {
                                    if (sessionRow["LessonPlanId"].ToString() == lessid)
                                    {
                                        Dseventsessionzerodtfound = true;
                                        break;
                                    }
                                }
                            }
                            if (Dseventsessionzerodtfound)
                            {
                                if (eventSessionZeroDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in eventSessionZeroDict[currDate])
                                    {
                                        if (sessionRow["LessonPlanId"].ToString() == lessid)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }
                                if (scoreDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scoreDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (scoreDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scoreDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                            }
                        }

                        if (DSScoredt.Rows.Count == 0)
                        {
                            if (eventSessionZeroDict.ContainsKey(currDate))
                            {
                                foreach (DataRow sessionRow in eventSessionZeroDict[currDate])
                                {
                                    if (sessionRow["LessonPlanId"].ToString() == lessid)
                                    {
                                        Dseventsessionzerodtfound = true;
                                        break;
                                    }
                                }
                            }
                            if (Dseventsessionzerodtfound)
                            {
                                if (eventSessionZeroDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in eventSessionZeroDict[currDate])
                                    {
                                        if (sessionRow["LessonPlanId"].ToString() == lessid)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }
                                if (scoreDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scoreDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) &&  colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (scoreDict.ContainsKey(currDate))
                                {
                                    foreach (DataRow sessionRow in scoreDict[currDate])
                                    {
                                        String Colcalcid = sessionRow["DstempSetColCalcId"].ToString();
                                        colIds = colIds.Select(Id => Id.Trim()).ToArray();
                                        string cname = sessionRow["ColName"].ToString();
                                        string ctype = sessionRow["CalcType"].ToString();
                                        if (sessionRow["LessonPlanId"].ToString() == lessid && colIds.Contains(Colcalcid) && colname == cname && calctype == ctype)
                                        {
                                            valueBuilder.Append("<br/>").Append(sessionRow["Score1"].ToString());
                                        }
                                    }
                                }

                            }
                        }
                    }
   
                newRow[subcol] = valueBuilder.ToString();
            }
            findt.Rows.Add(newRow);
        }

        return findt;
    }

    private void ExportGridViewToExcel()
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        exportgrid.RenderControl(htw);
        sess = (clsSession)Session["UserSession"];
        string sname = sess.StudentName.ToString().Replace(",", "");
        string Filename = sname + "_ProgressSummaryReport.xls";
         Filename = Server.UrlEncode(Filename);
        string filePath = Server.MapPath("~/StudentBinder/ExportACPSR/" + Filename);
        string folderPath = Server.MapPath("~/StudentBinder/ExportACPSR/");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        foreach (string file in Directory.GetFiles(folderPath))
        {
            File.Delete(file);
        }
        File.WriteAllText(filePath, sw.ToString(), Encoding.UTF8);
        tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Export Successfully Created...");
        hdnExport.Value = "true";
        exportgrid.Visible = false;
        exportgrid.Columns.Clear();
        string script = "hideOverlay();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "show", script, true);
    }

    private string DataTableToJson(DataTable dt)
    {
        var rows = new List<Dictionary<string, object>>();
        foreach (DataRow row in dt.Rows)
        {
            var rowDict = new Dictionary<string, object>();
            foreach (DataColumn column in dt.Columns)
            {
                rowDict[column.ColumnName] = row[column];
            }
            rows.Add(rowDict);
        }

        var serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = 2147483647;
        return CompressString(serializer.Serialize(rows));
    }

    private DataTable JsonToDataTable(string jsonString)
    {
        jsonString=DecompressString(jsonString);
        var serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = 2147483647;
        var rows = serializer.Deserialize<List<Dictionary<string, object>>>(jsonString);
        DataTable dt = new DataTable();
        if (rows.Count > 0)
        {
            foreach (var column in rows[0].Keys)
            {
                dt.Columns.Add(column);
            }

            foreach (var rowDict in rows)
            {
                var row = dt.NewRow();
                foreach (var column in rowDict.Keys)
                {
                    row[column] = rowDict[column];
                }
                dt.Rows.Add(row);
            }
        }

        return dt;
    }

    public static string CompressString(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using (var ms = new MemoryStream())
        {
            using (var gzip = new GZipStream(ms, CompressionMode.Compress))
            {
                gzip.Write(bytes, 0, bytes.Length);
            }
            return Convert.ToBase64String(ms.ToArray());
        }
    }

    public static string DecompressString(string compressedStr)
    {
        var bytes = Convert.FromBase64String(compressedStr);
        using (var ms = new MemoryStream(bytes))
        using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
        using (var reader = new StreamReader(gzip, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }
}
