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

    protected void Page_Load(object sender, EventArgs e)
    {
        btnExport.Visible = false;
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
                RV_ExcelReport.Visible = true;
                GenerateReport();
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

    protected DataSet LoadData()
    {
        sess = (clsSession)Session["UserSession"];
        oData = new clsData();

        String sqlStr = "SELECT MeasurementId,Behaviour FROM BehaviourDetails WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " AND ActiveInd IN('A','N') "+
        "ORDER BY Behaviour,MeasurementId";
        DataSet ds = oData.ReturnDataSet(sqlStr, false);
        return ds;
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