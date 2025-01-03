using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

public partial class StudentBinder_ExcelViewReport : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    DataTable alldata = new DataTable();
    Dictionary<string, string> columntypesdict = new Dictionary<string, string>();
    Dictionary<string, string> coltypedict = new Dictionary<string, string>();
    string[] columntypes = null;
    DataTable allless = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnExport.Visible = false;
        clsview.Visible = false;
        Gvclsdate.Visible = false;
        lbl.Visible = false;
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rbtnLsnClassType.Visible = false;
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
                dlLesson.Visible = false;
                btnExport.Visible = false;   
            }
            else
            {
                RV_ExcelReport.Visible = false;
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
        RV_ExcelReport.Visible = false;
        tdMsg.InnerHtml = "";
        lbl.Visible = false;
        lbl.Text = "";
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        string DisplayType = "";
        if (Convert.ToBoolean(chkLP.Checked) && Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "1,0";
        }
        else if (Convert.ToBoolean(chkLP.Checked))
        {
            DisplayType = "1";
        }
        else if (Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "0";
        }

        int FilterColumnIndex = 0;
        if (chkFiltrColumn.Checked == true)
        { FilterColumnIndex = 1; }
        else
        { FilterColumnIndex = 0; }

        getAllData(sess.StudentId.ToString(), StartDate.ToString(), enddate.ToString(), DisplayType, sess.SchoolId.ToString(), FilterColumnIndex.ToString());
        clsLoadRptLesson();
        clsview.Visible = true;
        Gvclsdate.Visible = true;
        divLesson.Visible = false;

    }

    public void clsLoadRptLesson()
    {
        if (alldata.Rows.Count > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LessonPlanId", typeof(int));
            dt.Columns.Add("LessonName", typeof(string));

            var idandname = alldata.AsEnumerable()
                                    .GroupBy(row => new { LessonPlanId = row.Field<int>("LessonPlanId"), LessonName = row.Field<string>("LessonName") })
                                    .Select(group => group.First());

            foreach (var row in idandname)
            {
                dt.ImportRow(row);
            }
            if (dt != null)
            {
                clslist.DataSource = dt;
                clslist.DataBind();
            }
        }
        else
        {
            lbl.Visible = true;
            lbl.Text = "No data available";
            Gvclsdate.Visible = false;
            clsview.Visible = false;
        }
    }

    public DataTable clsLoadRptLessonexport()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("LessonPlanId", typeof(int));
        dt.Columns.Add("LessonName", typeof(string));
        dt.Columns.Add("IsLp", typeof(string));
        dt.Columns.Add("IsDuration", typeof(bool));
        dt.Columns.Add("IsFrequency", typeof(bool));
        dt.Columns.Add("IsYesNo", typeof(bool));
        dt.Columns.Add("Interval", typeof(bool));
        if (alldata.Rows.Count > 0)
        {
            //var idandname = alldata.AsEnumerable()
            //                        .GroupBy(row => new { LessonPlanId = row.Field<int>("LessonPlanId"), LessonName = row.Field<string>("LessonName"), IsLp = row.Field<string>("IsLp"), IsDuration = row.Field<bool?>("IsDuration") ?? false, IsFrequency = row.Field<bool?>("IsFrequency") ?? false, IsYesNo = row.Field<bool?>("IsYesNo") ?? false, Interval = row.Field<string>("CalcType") == "%Interval" })
            //                        .Select(group => group.First());

            //foreach (var row in idandname)
            //{
            //    dt.ImportRow(row);
            //}
            var idandname = alldata.AsEnumerable()
       .GroupBy(row => new
       {
           LessonPlanId = row.Field<int>("LessonPlanId"),
           LessonName = row.Field<string>("LessonName"),
           IsLp = row.Field<string>("IsLp"),
           IsDuration = row.Field<bool?>("IsDuration") ?? false,
           IsFrequency = row.Field<bool?>("IsFrequency") ?? false,
           IsYesNo = row.Field<bool?>("IsYesNo") ?? false,
       })
       .Select(group => new
       {
           group.Key.LessonPlanId,
           group.Key.LessonName,
           group.Key.IsLp,
           group.Key.IsDuration,
           group.Key.IsFrequency,
           group.Key.IsYesNo,
           Interval = group.Any(row => row.Field<string>("CalcType") == "%Interval") // Check if any CalcType is %Interval
       })
       .ToList();
            foreach (var row in idandname)
            {
                DataRow newRow = dt.NewRow();
                newRow["LessonPlanId"] = row.LessonPlanId;
                newRow["LessonName"] = row.LessonName;
                newRow["IsLp"] = row.IsLp;
                newRow["IsDuration"] = row.IsDuration;
                newRow["IsFrequency"] = row.IsFrequency;
                newRow["IsYesNo"] = row.IsYesNo;
                newRow["Interval"] = row.Interval;
                dt.Rows.Add(newRow);
            }

        }
        return dt;

    }

    public void getAllData(string sid, string sdate, string edate, string DisplayType, string scid, string FilterColumnIndex)
    {
        SqlCommand cmd = null;
        SqlConnection con = ObjData.Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("BiweeklyExcelView", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", sdate);
            cmd.Parameters.AddWithValue("@ENDDate", edate);
            cmd.Parameters.AddWithValue("@StudentId", sid);
            cmd.Parameters.AddWithValue("@SchoolId", scid);
            cmd.Parameters.AddWithValue("@ShowLessonBehavior", DisplayType);
            cmd.Parameters.AddWithValue("@FilterColumn", FilterColumnIndex);
            da = new SqlDataAdapter(cmd);
            da.Fill(alldata);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + scid + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
        }
    }

    private void GenerateReport()
    {
        clsview.Visible = false;
        Gvclsdate.Visible = false;
        ObjData = new clsData();
        tdMsg.InnerHtml = "";
        lbl.Visible = false;
        lbl.Text = "";
        RV_ExcelReport.Visible = true;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        string DisplayType = "";
        if (Convert.ToBoolean(chkLP.Checked) && Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "1,0";
        }
        else if (Convert.ToBoolean(chkLP.Checked))
        {
            DisplayType = "1";
        }
        else if (Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "0";
        }


        RV_ExcelReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        //{

        //}
        //else
        //{

        RV_ExcelReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ExcelViewReport"];


        //}
        RV_ExcelReport.ShowParameterPrompts = false;

        //Excel View Not Show Blank Column [29-Jun-2020]
        int FilterColumnIndex = 0;
        if (chkFiltrColumn.Checked == true)
        { FilterColumnIndex = 1; }
        else
        { FilterColumnIndex = 0; }
        //Excel View Not Show Blank Column [29-Jun-2020]

        ReportParameter[] parm = new ReportParameter[6];
        parm[0] = new ReportParameter("StudentId", sess.StudentId.ToString());
        parm[1] = new ReportParameter("StartDate", StartDate.ToString());
        parm[2] = new ReportParameter("EndDate", enddate.ToString());
        parm[3] = new ReportParameter("ShowLessonBehavior", DisplayType);
        parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
        parm[5] = new ReportParameter("FilterColumn", FilterColumnIndex.ToString()); //Excel View Not Show Blank Column [29-Jun-2020]


        this.RV_ExcelReport.ServerReport.SetParameters(parm);
        RV_ExcelReport.ServerReport.Refresh();

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
        else if (txtrepEdate.Text == "")
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
        if (chkBehavior.Checked == false && chkLP.Checked == false)
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select display type");
            return result;
        }
        return result;

    }
    protected void btnSession_Click(object sender, EventArgs e)
    {
        try
        {

            if (Validation() == true)
            {
                RV_ExcelReport.Visible = false;
                rbtnLsnClassType.Visible = true;
                btnExport.Visible = true;
                dlLesson.Visible = true;
                LoadRptLesson();
            }
            else
            {
                
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    public void LoadRptLesson()
    {
        DataSet ds = null;
        sess = (clsSession)Session["UserSession"];
        ObjData = new clsData();
        System.Data.DataTable Dt = new System.Data.DataTable();
        string DisplayType = "";
        if (Convert.ToBoolean(chkLP.Checked) && Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "1,0";
        }
        else if (Convert.ToBoolean(chkLP.Checked))
        {
            DisplayType = "1";
        }
        else if (Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "0";
        }
        if (DisplayType.Contains("1"))
        {
            string sqlStr = "SELECT *,CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Approved') THEN (SELECT DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Approved') " +
        "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Maintenance') THEN (SELECT DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Maintenance')" +
        "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Inactive' ORDER BY DSTempHdr.DSTempHdrId DESC) END END END AS LessonName " +
            ",'1' AS IsLP FROM (SELECT LessonPlanId,LessonOrder FROM DSTempHdr DS INNER JOIN LookUp LU ON DS.StatusId=LU.LookupId WHERE DS.StudentId=" + sess.StudentId + " AND DS.SchoolId=" + sess.SchoolId +" AND " +
            "LU.LookupName IN ('Approved','Inactive','Maintenance') GROUP BY DS.LessonPlanId,DS.LessonOrder) LSN ORDER BY LessonOrder";
            ds = ObjData.ReturnDataSet(sqlStr, false);
        }
        if (DisplayType.Contains("0"))
        {
            DataSet ds2 = null;
            string sqlStr = "SELECT BDS.MeasurementId as LessonPlanId,BDS.Behaviour AS LessonName, '0' as IsLP FROM BehaviourDetails BDS WHERE BDS.ActiveInd IN ('A', 'N') AND BDS.StudentId=" + sess.StudentId + " AND BDS.SchoolId=" + sess.SchoolId;
            ds2 = ObjData.ReturnDataSet(sqlStr, false);
            if (ds != null)
            {
                ds.Merge(ds2);
            }
            else
            {
                ds = ds2;
            }
        }
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
    protected void dlLesson_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Session["e"] = e;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            GridView gVSession = (e.Item.FindControl("gVSession") as GridView);
            string LessonId = (e.Item.FindControl("LessonId") as Label).Text;
            string IsLP = (e.Item.FindControl("IsLP") as Label).Text;
            string rbtnClassType = rbtnLsnClassType.SelectedValue;            
            DataTable Dt = new DataTable();
            SqlCommand cmd = null;
            SqlConnection con = ObjData.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("BiweeklyExcelViewSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@ENDDate", enddate);
                cmd.Parameters.AddWithValue("@Studentid", sess.StudentId);
                cmd.Parameters.AddWithValue("@SchoolId", sess.SchoolId);
                cmd.Parameters.AddWithValue("@ShowLessonBehavior", IsLP);
                cmd.Parameters.AddWithValue("@LessonId", LessonId);
                cmd.Parameters.AddWithValue("@ClsType", rbtnLsnClassType.SelectedValue);
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

            BoundField boundField = null;
            //iterate through the columns of the datatable and add them to the gridview
            foreach (DataColumn col in Dt.Columns)
            {
                //initialize the bound field
                boundField = new BoundField();
                //set the DataField.
                boundField.DataField = col.ColumnName;

                //set the HeaderText
                boundField.HeaderText = col.ColumnName;

                //Add the field to the GridView columns.
                gVSession.Columns.Add(boundField);

            }
            //bind the gridview the DataSource



            gVSession.DataSource = Dt;
            gVSession.DataBind();
                
        }
    }
    public void checkLessonBehav(DataListItemEventArgs e)
    {
        ObjData = new clsData();
        GridView gVSession = (e.Item.FindControl("gVSession") as GridView);
        string LessonId = (e.Item.FindControl("LessonId") as Label).Text;
        string IsLP = (e.Item.FindControl("IsLP") as Label).Text;
        if (IsLP == "1")
        {
            ((DataControlField)gVSession.Columns.Cast<DataControlField>().Where(fld => (fld.HeaderText == "Duration (In minutes)")).SingleOrDefault()).Visible = false;
            ((DataControlField)gVSession.Columns.Cast<DataControlField>().Where(fld => (fld.HeaderText == "Frequency")).SingleOrDefault()).Visible = false;
            ((DataControlField)gVSession.Columns.Cast<DataControlField>().Where(fld => (fld.HeaderText == "%Interval")).SingleOrDefault()).Visible = false;
        }
        if (IsLP == "0")
        {
            ((DataControlField)gVSession.Columns.Cast<DataControlField>().Where(fld => (fld.HeaderText == "Column Measure")).SingleOrDefault()).Visible = false;
            ((DataControlField)gVSession.Columns.Cast<DataControlField>().Where(fld => (fld.HeaderText == "Score")).SingleOrDefault()).Visible = false;
            int frq = 0, dur = 0, yesNo = 0, Interval = 0;
            string sqlStr = "SELECT ISNULL(Frequency,0) AS IsFrequency, ISNULL(Duration,0) AS IsDuration, ISNULL(YesOrNo,0) AS IsYesOrNo, ISNULL(IfPerInterval,0) AS IsInterval FROM BehaviourDetails WHERE MeasurementId=" + LessonId;
            DataTable DT = ObjData.ReturnDataTable(sqlStr, false);
            if (DT != null && DT.Rows.Count == 1)
            {
                frq = Convert.ToInt32(DT.Rows[0]["IsFrequency"]);
                dur = Convert.ToInt32(DT.Rows[0]["IsDuration"]);
                yesNo = Convert.ToInt32(DT.Rows[0]["IsYesOrNo"]);
                Interval = Convert.ToInt32(DT.Rows[0]["IsInterval"]);
            }
            if ((frq == 0 && yesNo == 0) || (Interval == 1))
            {
                ((DataControlField)gVSession.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Frequency"))
                    .SingleOrDefault()).Visible = false;
            }
            if (dur == 0)
            {
                ((DataControlField)gVSession.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "Duration (In minutes)"))
                    .SingleOrDefault()).Visible = false;
            }
            if (Interval == 0)
            {
                ((DataControlField)gVSession.Columns.Cast<DataControlField>()
                    .Where(fld => (fld.HeaderText == "%Interval"))
                    .SingleOrDefault()).Visible = false;
            }
        }
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        txtRepStart.Text = string.Empty;
        txtrepEdate.Text = string.Empty;
        chkLP.Checked = true;
        chkBehavior.Checked = true;
        chkFiltrColumn.Checked = true;
        RV_ExcelReport.Visible = false;
        btnExport.Visible = false;
        dlLesson.Visible = false;
        rbtnLsnClassType.Visible = false;
    }
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=ExcelviewReport.xls");
        Response.ContentType = "application/ms-excel";
        Response.ContentEncoding = System.Text.Encoding.Unicode;
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
        //dlExport.RenderControl(hw);
        dlLesson.RenderControl(hw);
        Response.Write(sw.ToString());
        Response.End();  
    }
    public override void
   VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void clslist_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Gvclsdate.DataSource = GetDataForDate();
        Gvclsdate.DataBind();
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            //enddate = enddate + " 23:59:59.998";


            GridView less = (e.Item.FindControl("Gvclsless") as GridView);
            string lessid = (e.Item.FindControl("lessid") as Label).Text;
            selectcolumntype(lessid);
            int frq = 0, dur = 0, yesno = 0; int isLp = 0; int intrvl = 0;

            foreach (DataRow row in alldata.Rows)
            {
                if (row.Field<int>("LessonPlanId") == Convert.ToInt32(lessid))
                {

                    isLp = Convert.ToInt32(row.Field<string>("isLp"));
                    if (isLp == 0)
                    {
                        frq = Convert.ToInt32(row.Field<bool>("IsFrequency"));
                        dur = Convert.ToInt32(row.Field<bool>("IsDuration"));
                        yesno = Convert.ToInt32(row.Field<bool>("IsYesNo"));
                        if (row.Field<string>("CalcType") == "%Interval")
                        {
                            intrvl = 1;
                        }
                        else
                        {
                            intrvl = 0;
                        }
                    }

                    break;
                }
            }

            less.Columns.Clear();
            less.AutoGenerateColumns = false;

            if (isLp == 1)
            {
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
                    templateField.HeaderText = textval;
                    templateField.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                    templateField.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#4498c2");
                    templateField.HeaderStyle.CssClass = "nowrapText";
                    //templateField.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                    templateField.HeaderStyle.VerticalAlign = VerticalAlign.Top;
                    templateField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    templateField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    templateField.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, item);
                    less.Columns.Add(templateField);
                }
            }
            else
            {

                TemplateField templateFielddur = new TemplateField();
                templateFielddur.HeaderText = "Duration </br> (In minutes)";
                templateFielddur.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFielddur.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#4498c2");
                templateFielddur.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFielddur.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //templateFielddur.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFielddur.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFielddur.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Duration");
                templateFielddur.ItemStyle.CssClass = "nowrapText";
                if (dur == 0)
                {
                    templateFielddur.Visible = false;
                }
                less.Columns.Add(templateFielddur);

                TemplateField templateFieldfrq = new TemplateField();
                templateFieldfrq.HeaderText = "Frequency <br><br>";
                templateFieldfrq.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldfrq.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#4498c2");
                templateFieldfrq.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldfrq.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //templateFieldfrq.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldfrq.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldfrq.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Frequency");
                templateFieldfrq.ItemStyle.CssClass = "nowrapText";
                if (frq == 0 && yesno == 0)
                {
                    templateFieldfrq.Visible = false;
                }
                less.Columns.Add(templateFieldfrq);

                TemplateField templateFieldint = new TemplateField();
                templateFieldint.HeaderText = "%Interval <br><br> ";
                templateFieldint.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                templateFieldint.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#4498c2");
                templateFieldint.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                templateFieldint.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //templateFieldint.HeaderStyle.BorderColor = System.Drawing.Color.Black;
                templateFieldint.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                templateFieldint.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "Interval");
                templateFieldint.ItemStyle.CssClass = "nowrapText";
                if (intrvl == 0)
                {
                    templateFieldint.Visible = false;
                }
                less.Columns.Add(templateFieldint);

            }
            TemplateField templateFielddate = new TemplateField();
            templateFielddate.HeaderText = "Date </br> ";
            templateFielddate.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            templateFielddate.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");
            templateFielddate.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            templateFielddate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //templateFielddate.HeaderStyle.BorderColor = System.Drawing.Color.Black;
            templateFielddate.ItemTemplate = new AddGridViewTemplate(ListItemType.Item, "PeriodDate");
            templateFielddate.ItemStyle.CssClass = "nowrapText";
            templateFielddate.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            templateFielddate.Visible = false;
            less.Columns.Add(templateFielddate);

            DataRowView rowData = (DataRowView)e.Item.DataItem;
            DataTable dt = getClsDataBylessId(lessid, StartDate, enddate, isLp);
            less.DataSource = dt;
            less.DataBind();

        }

    }

    protected DataTable getClsDataBylessId(string lessid, String StartDate, string enddate, int islp)
    {
        DataTable dt = new DataTable();
        if (islp == 1)
        {
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
            DateTime startDate = DateTime.Parse(StartDate);
            DateTime endDate = DateTime.Parse(enddate);


            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                Dictionary<string, string> scoresdict = new Dictionary<string, string>();
                DataRow row = dt.NewRow();
                string currdate = date.ToString("MM/dd/yyyy");
                foreach (var type in columntypes)
                {
                    int count = 0;
                    double score = 0;
                    string cal = "";
                    if (alldata != null && alldata.Rows.Count > 0)
                    {
                        for (int r1 = 0; r1 < alldata.Rows.Count; r1++)
                        {
                            string coltype = alldata.Rows[r1]["ColName"].ToString() + "-" + alldata.Rows[r1]["CalcType"].ToString();
                            if (alldata.Rows[r1]["PeriodDate"].ToString() == currdate && alldata.Rows[r1]["LessonPlanId"].ToString() == lessid && type == coltype)
                            {
                                cal = alldata.Rows[r1]["CalcType"].ToString();
                                if (alldata.Rows[r1]["Score"].ToString() != "")
                                {
                                    score = score + Convert.ToDouble(alldata.Rows[r1]["Score"].ToString());
                                    count++;
                                }
                            }
                        }
                    }

                    string final = "";
                    if (count == 0 && score == 0)
                    {
                        final = "";
                    }
                    else
                    {
                        if (cal.Contains("%"))
                        {
                            score = score / count;
                        }
                        final = score.ToString();
                    }
                    if (!scoresdict.ContainsKey(type))
                    {
                        scoresdict.Add(type, final);
                    }
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
                dt.Rows.Add(row);
            }

        }
        else
        {
            dt.Columns.Add("PeriodDate", typeof(DateTime));

            dt.Columns.Add("Duration", typeof(string));
            dt.Columns.Add("Frequency", typeof(string));
            dt.Columns.Add("Interval", typeof(string));

            DateTime startDate = DateTime.Parse(StartDate);
            DateTime endDate = DateTime.Parse(enddate);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                DataRow row = dt.NewRow();
                string currdate = date.ToString("MM/dd/yyyy");
                string freq = "";
                string dur = "";
                string intvl = "";
                if (alldata != null && alldata.Rows.Count > 0)
                {
                    for (int r1 = 0; r1 < alldata.Rows.Count; r1++)
                    {
                        if (alldata.Rows[r1]["PeriodDate"].ToString() == currdate && alldata.Rows[r1]["LessonPlanId"].ToString() == lessid)
                        {
                            if (alldata.Rows[r1]["Frequency"].ToString() != null)
                            {
                                freq = alldata.Rows[r1]["Frequency"].ToString();
                            }
                            if (alldata.Rows[r1]["Duration"].ToString() != null)
                            {
                                dur = alldata.Rows[r1]["Duration"].ToString();
                            }
                            if (alldata.Rows[r1]["Interval"].ToString() != null)
                            {
                                intvl = alldata.Rows[r1]["Interval"].ToString();
                            }
                        }

                    }

                }
                row["PeriodDate"] = currdate;
                row["Frequency"] = freq;
                row["Duration"] = dur;
                row["Interval"] = intvl;
                dt.Rows.Add(row);
            }
        }

        return dt;

    }



    private void selectcolumntype(string id)
    {
        columntypes = alldata.AsEnumerable()
                                       .Where(row => row.Field<int>("LessonPlanId") == Convert.ToInt32(id) && row.Field<string>("isLp") == "1")
                                       .Select(row => row.Field<string>("ColName") + "-" + row.Field<string>("CalcType"))
                                       .Distinct()
                                       .ToArray();
        coltypedict = alldata.AsEnumerable()
   .Where(row => row.Field<int>("LessonPlanId") == Convert.ToInt32(id) && row.Field<string>("isLp") == "1")
   .Select(row => new
   {
       Key = row.Field<string>("ColName") + "-" + row.Field<string>("CalcType"),
       Value = row.Field<string>("ColName") + "-<br/>" + row.Field<string>("CalcType")
   })
   .Distinct()
   .ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < columntypes.Length; i++)
        {
            columntypesdict[columntypes[i]] = "Subcolumn" + i + 1;
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

    protected void btnExport_Clickhigh(object sender, ImageClickEventArgs e)
    {
        ObjData = new clsData();
        RV_ExcelReport.Visible = false;
        tdMsg.InnerHtml = "";
        lbl.Visible = false;
        lbl.Text = "";
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        string DisplayType = "";
        if (Convert.ToBoolean(chkLP.Checked) && Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "1,0";
        }
        else if (Convert.ToBoolean(chkLP.Checked))
        {
            DisplayType = "1";
        }
        else if (Convert.ToBoolean(chkBehavior.Checked))
        {
            DisplayType = "0";
        }

        int FilterColumnIndex = 0;
        if (chkFiltrColumn.Checked == true)
        { FilterColumnIndex = 1; }
        else
        { FilterColumnIndex = 0; }

        getAllData(sess.StudentId.ToString(), StartDate.ToString(), enddate.ToString(), DisplayType, sess.SchoolId.ToString(), FilterColumnIndex.ToString());

        allless = clsLoadRptLessonexport();
        if (allless.Rows.Count > 0)
        {
            ViewState["allless"] = allless;
            ViewState["alldata"] = alldata;
            tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Export Successfully Created...");
            hdnExport.Value = "true";
            string script = "hideOverlay();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "show", script, true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available');", true);


        }

    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        this.BindGrid();
        sess = (clsSession)Session["UserSession"];
        string sname = sess.StudentName.ToString().Replace(",", "");
        string Filename = sname + DateTime.Now + ".xls";
        Response.ClearContent();
        Response.Clear();
        Response.Buffer = true;
        Response.ClearHeaders();
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;Filename=" + Filename);
        StringWriter str = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(str);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        exportgrid.AllowPaging = false;
        exportgrid.GridLines = GridLines.Both;
        exportgrid.HeaderStyle.Font.Bold = true;
        //exportgrid.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2EFF");
        for (int i = 0; i < exportgrid.Columns.Count; i++)
        {
            exportgrid.Columns[i].HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0080FF");
        }
        foreach (GridViewRow row in exportgrid.Rows)
        {
            row.Cells[0].BackColor = System.Drawing.ColorTranslator.FromHtml("#0080FF");  // First column in each row
        }

        exportgrid.RenderControl(htw);
        string fileContent = str.ToString();
        Response.Write(str.ToString());
        Response.Flush();
        //Response.Close();
        Response.End();


    }


    public void SummaryBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

        TableHeaderCell tec = new TableHeaderCell();
        tec = new TableHeaderCell();
        tec.ColumnSpan = 1;
        tec.Text = "";
        tec.BackColor = ColorTranslator.FromHtml("#0080FF");
        row.Controls.Add(tec);
        string lessid = "";
        foreach (DataRow rows in allless.Rows)
        {
            lessid = rows["LessonPlanId"].ToString();
            if (rows["IsLp"].ToString() == "1")
            {
                selectcolumntype(lessid);
                int columnleng = columntypes.Length;

                tec = new TableHeaderCell();
                tec.ColumnSpan = columnleng;
                tec.Text = rows["LessonName"].ToString();
                tec.BackColor = ColorTranslator.FromHtml("#0080FF");
                row.Controls.Add(tec);
            }
            else
            {
                int len = 0;
                int frq = Convert.ToInt32(rows.Field<bool>("IsFrequency"));
                int dur = Convert.ToInt32(rows.Field<bool>("IsDuration"));
                int yesno = Convert.ToInt32(rows.Field<bool>("IsYesNo"));
                int Interval = Convert.ToInt32(rows.Field<bool?>("Interval") ?? false);
                if (frq == 1 || yesno == 1)
                {
                    len++;
                }
                if (dur == 1)
                {
                    len++;
                }
                if (Interval == 1)
                {
                    len++;
                }

                tec = new TableHeaderCell();
                tec.ColumnSpan = len;
                tec.Text = rows["LessonName"].ToString();
                tec.BackColor = ColorTranslator.FromHtml("#0080FF");
                row.Controls.Add(tec);
            }
        }

        exportgrid.HeaderRow.Parent.Controls.AddAt(0, row);
    }

    private void BindGrid()
    {
        int subcolumn = 0;
        allless = (DataTable)ViewState["allless"];
        alldata = (DataTable)ViewState["alldata"];
        DataTable allcolumn = new DataTable();
        allcolumn.Columns.Add("lessid", typeof(string));
        allcolumn.Columns.Add("colname", typeof(string));
        allcolumn.Columns.Add("subcol", typeof(int));
        foreach (DataRow rows in allless.Rows)
        {
            int frq = 0, dur = 0, yesno = 0; int isLp = 0; int intrvl = 0;
            isLp = Convert.ToInt32(rows.Field<string>("isLp"));
            if (isLp == 0)
            {
                frq = Convert.ToInt32(rows.Field<bool>("IsFrequency"));
                dur = Convert.ToInt32(rows.Field<bool>("IsDuration"));
                yesno = Convert.ToInt32(rows.Field<bool>("IsYesNo"));
                intrvl = Convert.ToInt32(rows.Field<bool>("Interval"));
            }
            string lessid = rows["LessonPlanId"].ToString();
            if (isLp == 1)
            {
                selectcolumntype(lessid);
                for (int i = 0; i < columntypes.Length; i++)
                {
                    subcolumn = subcolumn + 1;
                    allcolumn.Rows.Add(lessid, columntypes[i].ToString(), subcolumn);
                }
            }
            else
            {
                if (dur == 1)
                {
                    subcolumn = subcolumn + 1;
                    allcolumn.Rows.Add(lessid, "Duration(In minutes)", subcolumn);

                }

                if (frq == 1 || yesno == 1)
                {
                    subcolumn = subcolumn + 1;
                    allcolumn.Rows.Add(lessid, "Frequency", subcolumn);
                }
                if (intrvl == 1)
                {
                    subcolumn = subcolumn + 1;
                    allcolumn.Rows.Add(lessid, "%Interval", subcolumn);
                }

            }
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Date", typeof(string));
        BoundField boundField1 = new BoundField();
        boundField1.DataField = "Date";
        boundField1.HeaderText = "Date";
        exportgrid.Columns.Add(boundField1);
        foreach (DataRow rows in allcolumn.Rows)
        {
            BoundField boundField = new BoundField();
            boundField.DataField = "Subcol" + rows["subcol"].ToString();
            boundField.HeaderText = rows["colname"].ToString();
            exportgrid.Columns.Add(boundField);
            string colname = "Subcol" + rows["subcol"].ToString();
            dt.Columns.Add(colname, typeof(string));

        }
        dt = getAllData(allcolumn, dt);

        exportgrid.DataSource = dt;
        exportgrid.DataBind();
    }
    private DataTable getAllData(DataTable dt, DataTable findt)
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");
        DateTime startDate = DateTime.Parse(StartDate);
        DateTime endDate = DateTime.Parse(enddate);
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            DataRow newRow = findt.NewRow();
            string currdate = date.ToString("MM/dd/yyyy");
            newRow["Date"] = currdate;
            foreach (DataRow rows in dt.Rows)
            {

                string subcol = "Subcol" + rows["subcol"].ToString();
                string score = findscore(rows["lessid"].ToString(), rows["colname"].ToString(), currdate);
                newRow[subcol] = score;

            }
            findt.Rows.Add(newRow);

        }

        return findt;
    }
    private string findscore(string lessid, string colname, string currdate)
    {
        string[] parts = colname.Split('-');
        int count = 0; string score = ""; double score1 = 0;
        bool isless = false;
        bool avg = false;

        if (alldata != null && alldata.Rows.Count > 0)
        {
            foreach (DataRow rows in alldata.Rows)
            {
                if (lessid == rows["LessonPlanId"].ToString() && rows["PeriodDate"].ToString() == currdate)
                {

                    if (rows["IsLp"].ToString() == "1")
                    {
                        isless = true;
                        if ((rows["CalcType"].ToString()).Contains("%"))
                        {
                            avg = true;
                        }
                        string coltype = rows["ColName"].ToString() + "-" + rows["CalcType"].ToString();
                        if (colname == coltype && lessid == rows["LessonPlanId"].ToString() && rows["PeriodDate"].ToString() == currdate)
                        {
                            if (rows["Score"].ToString() != "")
                            {
                                score1 = score1 + Convert.ToDouble(rows["Score"].ToString());
                                count++;
                            }

                        }
                    }
                    else
                    {
                        if (lessid == rows["LessonPlanId"].ToString() && rows["PeriodDate"].ToString() == currdate)
                        {
                            if (colname == "Frequency")
                            {

                                score = rows["Frequency"].ToString();

                            }
                            if (colname == "Duration(In minutes)")
                            {

                                score = rows["Duration"].ToString();

                            }
                            if (colname == "%Interval")
                            {

                                score = rows["Interval"].ToString();

                            }
                        }
                    }
                }
            }
        }

        if (isless)
        {
            if (avg)
            {
                if (count == 0 && score1 == 0)
                {
                    return "";
                }
                else
                {
                    score1 = score1 / count;
                    return score1.ToString();
                }
            }
            else
            {
                if (count == 0 && score1 == 0)
                {
                    return "";
                }
                else
                {
                    return score1.ToString();
                }
            }

        }
        else
        {
            return score;
        }


    }

}