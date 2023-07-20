using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentBinder_ChainedBarGraphReport : System.Web.UI.Page
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
                bool flag = clsGeneral.PageIdentification(sess.perPage);
                if (flag == false)
                {
                    Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
                }
            }
            if (!IsPostBack)
            {
                sess = (clsSession)Session["UserSession"];
                ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
                ClassDatatable objClassData = new ClassDatatable();
                hfPopUpValue.Value = "false";
                if (Request.QueryString["studid"] != null)
                {
                    int pageid = Convert.ToInt32(Request.QueryString["pageid"].ToString());
                    int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                    sess.StudentId = studid;

                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "loadWait();", true);
                    ObjTempSess.TemplateId = pageid;
                    txtEdate.Text = DateTime.Now.Date.AddDays(1).ToString("MM/dd/yyyy").Replace("-", "/");
                    txtSdate.Text = DateTime.Now.Date.AddDays(-10).ToString("MM/dd/yyyy").Replace("-", "/");
                    //rbtnClassType.SelectedValue = objClassData.GetClassType(sess.Classid);
                    
                    //GenerateReport();
                }

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
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            hfPopUpValue.Value = "true";
            GenerateReport();

        }
        catch (Exception Ex)
        {
            throw Ex;
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

    private void GenerateReport()
    {
        if (Validate() == true)
        {
            ObjData = new clsData();
            tdMsg.InnerHtml = "";
            RV_LPReport.Visible = true;
            sess = (clsSession)Session["UserSession"];
            int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
            int templateId = Convert.ToInt32(Request.QueryString["pageid"].ToString());
            string StudName = "";
            string strQuery = "SELECT StudentLname+','+StudentFname AS StudentName FROM Student WHERE StudentId=" + studid;
            StudName = Convert.ToString(ObjData.FetchValue(strQuery));
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            string AllLesson = "";
            AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId));
            string LPStatus = ObjData.FetchValue("SELECT LookupName FROM LOOKUP LU INNER JOIN DSTempHdr HD ON LU.LookupId=HD.StatusId WHERE LookupType='TemplateStatus' AND DSTempHdrId=" + templateId).ToString();
            RV_LPReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ChainedDupBar"];
            }
            else
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ChainedBar"];
            }

            if (AllLesson == "")
            {
                RV_LPReport.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
                return;
            }

            RV_LPReport.ShowParameterPrompts = false;

            ReportParameter[] parm = new ReportParameter[8];
            parm[0] = new ReportParameter("LessonPlanId", AllLesson);
            parm[1] = new ReportParameter("StudentId",studid.ToString());
            parm[2] = new ReportParameter("SDate", StartDate.ToString());
            parm[3] = new ReportParameter("EDate", enddate.ToString());    
            parm[4] = new ReportParameter("ClassType", rbtnClassType.SelectedValue);
            parm[5] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
            parm[6] = new ReportParameter("PromptType", RbtnPloatType.SelectedValue);
            //parm[7] = new ReportParameter("LPStatus", LPStatus);
            parm[7] = new ReportParameter("DSTempHdrId", templateId.ToString());


            this.RV_LPReport.ServerReport.SetParameters(parm);

            RV_LPReport.ServerReport.Refresh();

        }
        else
            RV_LPReport.Visible = false;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {

            ObjData = new clsData();
            int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
            int templateId = Convert.ToInt32(Request.QueryString["pageid"].ToString());
            string StudName = "";
            string strQuery = "SELECT StudentLname+','+StudentFname AS StudentName FROM Student WHERE StudentId=" + studid;
            StudName = Convert.ToString(ObjData.FetchValue(strQuery));

            //string ReportXML = "SELECT [Name],CAST(CAST([Content] AS VARBINARY(MAX)) AS XML) AS reportXML FROM [Catalog] WHERE type = 2";
            //DataTable dt = ObjData.ReturnDataTable(ReportXML, false);
            sess = (clsSession)Session["UserSession"];
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            ReportViewer ChainedBarReport = new ReportViewer();
            string AllLesson = "";
            AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId));
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            ChainedBarReport.ProcessingMode = ProcessingMode.Remote;
            ChainedBarReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            ChainedBarReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ChainedExportBar"];
            ChainedBarReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportUrl"]);
            ChainedBarReport.ServerReport.Refresh();
            string LPStatus = ObjData.FetchValue("SELECT LookupName FROM LOOKUP LU INNER JOIN DSTempHdr HD ON LU.LookupId=HD.StatusId WHERE LookupType='TemplateStatus' AND DSTempHdrId=" + templateId).ToString();

            ReportParameter[] parm = new ReportParameter[8];
            parm[0] = new ReportParameter("LessonPlanId", AllLesson);
            parm[1] = new ReportParameter("StudentId", studid.ToString());
            parm[2] = new ReportParameter("SDate", StartDate.ToString());
            parm[3] = new ReportParameter("EDate", enddate.ToString());
            parm[4] = new ReportParameter("ClassType", rbtnClassType.SelectedValue);
            parm[5] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
            parm[6] = new ReportParameter("PromptType", RbtnPloatType.SelectedValue);
            //parm[7] = new ReportParameter("LPStatus", LPStatus);
            parm[7] = new ReportParameter("DSTempHdrId", templateId.ToString());
            ChainedBarReport.ServerReport.SetParameters(parm);

            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension, deviceInfo;
            
            deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth><MarginTop>1in</MarginTop></DeviceInfo>";
            
            //deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth></DeviceInfo>";

            byte[] bytes = ChainedBarReport.ServerReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            string outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + StudName + "_Chained-Bar-Graph-Report.pdf");
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            Session["PdfPath"] = outputPath;
            tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Export Successfully Created...");
            hdnExport.Value = "true";
            ClientScript.RegisterStartupScript(GetType(), "", "DownloadPopup();", true);
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", "attachment; filename=" + sess.StudentName + "_Chained-Bar-Graph-Report.pdf");

            //Response.BinaryWrite(bytes); // create the file
            //Response.Flush(); // send it to the client to download

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        try
        {

            ObjData = new clsData();
            //string ReportXML = "SELECT [Name],CAST(CAST([Content] AS VARBINARY(MAX)) AS XML) AS reportXML FROM [Catalog] WHERE type = 2";
            //DataTable dt = ObjData.ReturnDataTable(ReportXML, false);
            sess = (clsSession)Session["UserSession"];
            ReportViewer AcademicReport = new ReportViewer();
            string AllLesson = Convert.ToString(Session["AcademicSessions"]);
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            AcademicReport.ProcessingMode = ProcessingMode.Remote;
            AcademicReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DupSessionBased"];
            AcademicReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportUrl"]);
            AcademicReport.ServerReport.Refresh();

            ReportParameter[] parm = new ReportParameter[5];
            parm[0] = new ReportParameter("StartDate", StartDate.ToString());
            parm[1] = new ReportParameter("EndDate", enddate.ToString());
            parm[2] = new ReportParameter("StudentId", sess.StudentId.ToString());
            parm[3] = new ReportParameter("LessonPlan", AllLesson.ToString());
            parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());                                        //Parameter Value
            AcademicReport.ServerReport.SetParameters(parm);

            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension, deviceInfo;

            deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth></DeviceInfo>";

            byte[] bytes = AcademicReport.ServerReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            string outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + sess.StudentName + "_Session-BasedReport.pdf");
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

            string PdfPath = sess.StudentName + "_Session-BasedReport.pdf";
            Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintReport.aspx?file=" + PdfPath));

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string FileName = Session["PdfPath"].ToString();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");
            byte[] data = req.DownloadData(FileName);
            response.BinaryWrite(data);
            ClientScript.RegisterStartupScript(GetType(), "", "HideWait();", true);
            response.End();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}