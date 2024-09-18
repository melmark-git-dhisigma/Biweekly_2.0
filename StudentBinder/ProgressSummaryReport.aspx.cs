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

public partial class StudentBinder_ProgressSummaryReport : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    System.Data.DataTable Dt = null;
    clsData oData_ov = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLessonPlan();            
        }
        btnExport.Visible = false;
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rbtnClassType.Visible = false;
            tableDurFormat.Visible = false;
            if (Validation() == true)
            {
                GenerateReport();
                chkStatus.Visible = false;
                dlLesson.Visible = false;
                td1.Visible = false;
                tdMsg.Visible = false;
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void btnExport_Click1(object sender, ImageClickEventArgs e)
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

}