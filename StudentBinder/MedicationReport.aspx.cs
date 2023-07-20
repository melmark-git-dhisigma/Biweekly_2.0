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
using System.Data.SqlClient;
using System.Data;

public partial class StudentBinder_MedicationReport : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            sess = (clsSession)Session["UserSession"];
            if (sess == null)
            {
                Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
            }
            else
            {
                //bool flag = clsGeneral.PageIdentification(sess.perPage);
                //if (flag == false)
                //{
                //    Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
                //}
            }
            if (!IsPostBack)
            {
                sess = (clsSession)Session["UserSession"];
                ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];

                if (Request.QueryString["studid"] != null)
                {
                    int pageid = Convert.ToInt32(Request.QueryString["pageid"].ToString());
                    int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                    sess.StudentId = studid;
                    if (pageid == 0)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('.loading').fadeIn('fast');", true);
                        //chkbxevents.Checked = true;
                        //chkbxIOA.Checked = true;
                        txtEdate.Text = DateTime.Now.Date.AddDays(1).ToString("MM-dd-yyyy");
                        txtSdate.Text = DateTime.Now.Date.AddDays(-91).ToString("MM-dd-yyyy");                    
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "loadWait();", true);
                        GenerateReport();
                    }
                    else
                    {
                        ObjTempSess.TemplateId = pageid;
                        RV_MedReport.Visible = false;
                    }

                }

            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
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
    private bool Validate()
    {
        bool result = true;
        if (txtSdate.Text == "")
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Start Date");
            return result;
        }
        else if (txtEdate.Text == "")
        {
            result = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter End Date");
            return result;
        }
        else if (txtSdate.Text != "" && txtEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > dted)
            {
                result = false;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
            }
            return result;
        }
        return result;

    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateReport();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void GenerateReport()
    {
        if (Validate() == true)
        {
            ObjData = new clsData();
            tdMsg.InnerHtml = "";
            RV_MedReport.Visible = true;
            sess = (clsSession)Session["UserSession"];
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            
                string StrQuery = "SELECT COUNT(*) FROM StdtSessEvent WHERE (StdtSessEventType = 'Medication') AND " +
                                  " StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND CONVERT(DATE,[EvntTs]) >= CONVERT(DATE,'" + StartDate + "') AND CONVERT(DATE,[EvntTs]) <= CONVERT(DATE,'" + enddate + "')";
                
            
            if (Convert.ToInt32(ObjData.FetchValue(StrQuery)) == 0)
            {
                RV_MedReport.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
                return;
            }



            RV_MedReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            RV_MedReport.ServerReport.ReportPath = "/Biweekly Reports/Medication";
            RV_MedReport.ShowParameterPrompts = false;

            ReportParameter[] parm = new ReportParameter[4];
            parm[0] = new ReportParameter("StartDate", StartDate.ToString());
            parm[1] = new ReportParameter("EndDate", enddate.ToString());
            parm[2] = new ReportParameter("StudentId", sess.StudentId.ToString());            
            parm[3] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
           
            this.RV_MedReport.ServerReport.SetParameters(parm);

            RV_MedReport.ServerReport.Refresh();
        }
        else
            RV_MedReport.Visible = false;
    }
}