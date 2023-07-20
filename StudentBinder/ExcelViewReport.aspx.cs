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

public partial class StudentBinder_ExcelViewReport : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnExport.Visible = false;
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rbtnLsnClassType.Visible = false;
            if (Validation() == true)
            {
                dlLesson.Visible = false;
                btnExport.Visible = false;
                GenerateReport();
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
        else if (txtRepStart.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid Start date");
            }
            return result;
        }
        else if (txtrepEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid End date");
            }
            return result;
        }
        else if (txtRepStart.Text != "" && txtrepEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > dted)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
            }
            return result;
        }
        else if(chkBehavior.Checked==false&&chkLP.Checked==false)
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
}