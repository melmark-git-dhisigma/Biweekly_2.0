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
using System.Collections;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

public partial class StudentBinder_AcademicSessionReport : System.Web.UI.Page
{
    public string sDate;
    public String eDate;
    public int sid;
    public string lid;
    public int scid;
    public string evnt;
    public string trend;
    public string ioa;
    public string cls;
    public bool med;
    public string lname;
    public string lpstatus;
    public bool reptype = false;
    public bool inctype = false;
    public static clsData objData = null;
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    ArrayList arraylist1 = new ArrayList();
    ArrayList arraylist2 = new ArrayList();
    DataTable Dt = null;
    DataTable Dts = null;
    clsLessons oLessons;
    public bool medno = false;
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
                ClassDatatable objClassData = new ClassDatatable();
                hfPopUpValue.Value = "false";
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
                        //txtEdate.Text = DateTime.Now.Date.AddDays(1).ToString("MM/dd/yyyy").Replace("-", "/");
                        txtEdate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy").Replace("-", "/");
                        txtSdate.Text = DateTime.Now.Date.AddDays(-30).ToString("MM/dd/yyyy").Replace("-", "/");
                        //rbtnClassType.SelectedValue = objClassData.GetClassType(sess.Classid);
                        hdnallLesson.Value = "AllLessons";
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "loadWait();", true);
                        //chkrepevents.Checked = true;
                        chkrepmajor.Checked = true;
                        chkrepminor.Checked = true;
                        chkreparrow.Checked = true;
                        chkrepioa.Checked = true;
                        chkreptrend.Checked = true;
                        chkrepmedi.Checked = false;
                        loadLessonPlan();
                        hdnType.Value = "SessionGraph";
                        RefreshMaintenance.Visible = false;
                        //GenerateReport();
                    }
                    else
                    {
                        ObjTempSess.TemplateId = pageid;
                        hdnType.Value = "MaintenanceGraph";
                        hdnallLesson.Value = "SingleLesson";
                        FillData();
                    }                    
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);
        }
        catch (Exception Ex)
        {
            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(Ex.ToString());
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
    private bool Validate()
    {
        bool result = true;
        if (hdnType.Value == "SessionGraph")
        {            
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
            if (txtSdate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > DateTime.Now)
                {
                    result = false;
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid Start date");
                    return result;
                }

            }
            if (txtEdate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > DateTime.Now)
                {
                    result = false;
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid End date");
                    return result;
                }

            }
            if (txtSdate.Text != "" && txtEdate.Text != "")
            {
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > dted)
                {
                    result = false;
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Start Date must be before the End Date");
                    return result;
                }

            }
            return result;
        }
        else if (hdnType.Value == "MaintenanceGraph")
        {
            if (txtStartDate.Text == "")
            {
                result = false;
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Enter Start Date");
                return result;
            }
            else if (txtEndDate.Text == "")
            {
                result = false;
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Enter End Date");
                return result;
            }
            if (txtStartDate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtStartDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > DateTime.Now)
                {
                    result = false;
                    tdMsg1.InnerHtml = clsGeneral.warningMsg("Invalid Start date");
                    return result;
                }

            }
            if (txtEndDate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtEndDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > DateTime.Now)
                {
                    result = false;
                    tdMsg1.InnerHtml = clsGeneral.warningMsg("Invalid End date");
                    return result;
                }

            }
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                dtst = DateTime.ParseExact(txtStartDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(txtEndDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > dted)
                {
                    result = false;
                    tdMsg1.InnerHtml = clsGeneral.warningMsg("Start Date must be before the End Date");
                    return result;
                }

            }
            return result;
        }
        return result;
        
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ListBox2.Items.Count > 0)
            {
                if (Validate() == true)
                {
                    btnPrevious1.Visible = true;
                    ddlLessonplan1.Visible = true;
                    btnNext1.Visible = true;

                    hfPopUpValue.Value = "true";
                    //saveLessonPlans();
                    GenerateReport();
                }

                else
                {
                    if (highcheck.Checked == false)
                    {
                        RV_LPReport.Visible = false;
                    }
                    else
                    {
                        string script = "warningmsgpop();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepops", script, true);
                    }
                    hfPopUpValue.Value = "false";
                    btnPrevious1.Visible = false;
                    ddlLessonplan1.Visible = false;
                    btnNext1.Visible = false;
                    
                }
               
            }
            else
            {
                hfPopUpValue.Value = "false";
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select LessonPlan");
                if (highcheck.Checked == true)
                {
                    string script = "warningmsgpop();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepopss", script, true);
                }

            }
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
            int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
            tdMsg.InnerHtml = "";
            if (highcheck.Checked == false)
            {
            RV_LPReport.Visible = true;
            }
            else
            {
                RV_LPReport.Visible = false;
            }
            sess = (clsSession)Session["UserSession"];
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            hdnType.Value = "SessionGraph";            

            ///checks the lesson status
            ///
            string LPStatus = "";
            foreach (ListItem item in chkStatus.Items)
            {
                if (item.Selected == true)
                {
                    if (item.Text == "Active")
                    {
                        LPStatus += "Approved,";
                    }
                    else if (item.Text == "Maintenance")
                    {
                        LPStatus += "Maintenance,";
                    }
                    else if (item.Text == "Inactive")
                    {
                        LPStatus += "Inactive,";
                    }
                }
            }
            LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
            ///end
            ///

            string AllLesson = "";
            string result = "";

            string StrQuery = "";
            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                StrQuery += ListBox2.Items[i].Value + ",";
            }
            StrQuery = StrQuery.Substring(0, (StrQuery.Length - 1));
            AllLesson = Convert.ToString(StrQuery);
            Session["AcademicSessions"] = AllLesson;

            string[] listVal = LPStatus.Split(',');

            if (listVal.Length >= 1)
            {
                for (int i = 0; i < listVal.Length; i++)
                {
                    result += "'" + listVal[i] + "',";
                }
            }
            result = result.Substring(0, (result.Length - 1));

            string strLess = "SELECT *,CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + studid + " AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN(" + result + ")) " +
            "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + studid + " AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN(" + result + ")) " +
            "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + studid + " AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN(" + result + ")) " +
            "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + studid + " AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN(" + result + "))" +
            "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + studid + " AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN(" + result + ") ORDER BY DSTempHdr.DSTempHdrId DESC) " +
            "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + studid + " AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN(" + result + ") ORDER BY DSTempHdr.DSTempHdrId DESC) END END END AS LessonPlanName " +
            "FROM (SELECT DISTINCT DTmp.LessonPlanId, DTmp.LessonOrder FROM DSTempHdr DTmp INNER JOIN LookUp ON DTmp.StatusId=LookUp.LookupId WHERE StudentId=" + studid + " and LessonPlanId in (" + AllLesson + ") AND LookupName in (" + result + ")) LSN ORDER BY  LessonOrder";
            DataTable DTLesson = ObjData.ReturnDataTable(strLess, false);

            DataTable dtLP = new DataTable();
            dtLP.Columns.Add("Id", typeof(string));
            dtLP.Columns.Add("Name", typeof(string));

            if (DTLesson != null)
            {
                if (DTLesson.Rows.Count > 0)
                {
                    foreach (DataRow drLessn in DTLesson.Rows)
                    {
                        DataRow drr = dtLP.NewRow();
                        drr["Id"] = drLessn.ItemArray[0];
                        drr["Name"] = drLessn.ItemArray[2];
                        dtLP.Rows.Add(drr);
                    }
                }
            }
            ddlLessonplan1.DataSource = dtLP;
            ddlLessonplan1.DataTextField = "Name";
            ddlLessonplan1.DataValueField = "Id";
            ddlLessonplan1.DataBind();
            AllLesson = DTLesson.Rows[0].ItemArray[0].ToString();
            string LessonName = DTLesson.Rows[0].ItemArray[2].ToString();
            if (AllLesson == "")
            {
                if (highcheck.Checked == false)
                {
                RV_LPReport.Visible = false;
                }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available');", true);
                return;
            }
            else
            {
                if (highcheck.Checked == true)
                {
                    fillGraphhighchart(AllLesson, LessonName);
                    //graphPopup.Visible = false;
                }
                else
                {
                    string script = "closePopup();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop", script, true);
                fillGraph(AllLesson, LessonName);
            }
        }
    }
    }




    private void loadLessonPlan()
    {
        ObjData = new clsData();
        sess = (clsSession)Session["UserSession"];
        oLessons = new clsLessons();
        Dts = new DataTable();
        Dt = new DataTable();
        string StatusCheck="";
        foreach(ListItem item in chkStatus.Items)
        {
            if(item.Selected==true)
            {
                if(item.Text=="Active")
                {
                    StatusCheck+="'Approved' ";
                }
                else if(item.Text=="Maintenance")
                    {
                    if(StatusCheck!="")
                    {
                        StatusCheck+=",";
                    }
                    StatusCheck+="'Maintenance' ";
                
                }
                else if(item.Text=="Inactive")
                    {
                    if(StatusCheck!="")
                    {
                        StatusCheck+=",";
                    }
                    StatusCheck+="'Inactive' ";
                }
            }
        }
        
        
        //Dt = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "DTmp.DSTemplateName +' ( '+CASE WHEN LU.LookupName='Approved' THEN 'Active' ELSE LU.LookupName END+' )' as LessonPlanName,DTmp.DSTempHdrId as LessonPlanId", "(LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='Inactive')");
        //Dt = ObjData.ReturnDataTable("SELECT  distinct DTmp.DSTemplateName +' ( '+CASE WHEN LU.LookupName='Approved' THEN 'Active' ELSE LU.LookupName END+' )' as  LessonPlanName,DTmp.DSTempHdrId as LessonPlanId,DTmp.LessonOrder  FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN  LookUp LU ON LU.LookupId=DTmp.StatusId)   ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId + " AND   StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND (LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR  DTmp.DSMode='Inactive') ORDER BY  DTmp.LessonOrder", false);
        //Dt = ObjData.ReturnDataTable("SELECT DISTINCT DTmp.LessonPlanId,DTmp.LessonOrder,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=DTmp.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " ORDER BY DSTempHdrId DESC) LessonPlanName FROM DSTempHdr DTmp WHERE StudentId=" + sess.StudentId + " AND StatusId IN (SELECT LookupId FROM LookUp WHERE " + StatusCheck + ") ORDER BY  DTmp.LessonOrder", false);
        //Dt = ObjData.ReturnDataTable("SELECT DISTINCT DTmp.LessonPlanId,DTmp.LessonOrder,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=DTmp.LessonPlanId AND DSTempHdr.StudentId="+sess.StudentId+" ORDER BY DSTempHdrId DESC)+' ( '+CASE WHEN LookupName='Approved' THEN 'Active' ELSE LookupName END+' )' AS LessonPlanName FROM DSTempHdr DTmp INNER JOIN LookUp ON DTmp.StatusId=LookUp.LookupId WHERE StudentId="+sess.StudentId+" AND ("+StatusCheck+") ORDER BY  DTmp.LessonOrder", false);
        string StrQuery = "SELECT *,CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN(" + StatusCheck + ")) " +
       "THEN (SELECT TOP 1 DSTemplateName+' ( Active )' FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN(" + StatusCheck + ")) " +
        "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN(" + StatusCheck + ")) " +
        "THEN (SELECT TOP 1 DSTemplateName+' ( Maintenance )' FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN(" + StatusCheck + "))" +
        "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN(" + StatusCheck + ") ORDER BY DSTempHdr.DSTempHdrId DESC) " +
        "THEN (SELECT TOP 1 DSTemplateName+' ( Inactive )' FROM DSTempHdr LEFT JOIN LookUp on DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN(" + StatusCheck + ") ORDER BY DSTempHdr.DSTempHdrId DESC) END END END AS LessonPlanName " +
        "FROM (SELECT DISTINCT DTmp.LessonPlanId,DTmp.LessonOrder FROM DSTempHdr DTmp INNER JOIN LookUp ON DTmp.StatusId=LookUp.LookupId WHERE StudentId=" + sess.StudentId + "  AND LookUpType='TemplateStatus' AND LookupName IN( " + StatusCheck + " )) LSN ORDER BY  LessonOrder";
        if(StatusCheck!="")
        Dt = ObjData.ReturnDataTable(StrQuery, false);
        if (Dt.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
        }

        if (Dt != null && Dt.Rows.Count!=0)
        {
            ListBox1.DataSource = Dt;
            ListBox1.DataTextField = "LessonPlanName";
            ListBox1.DataValueField = "LessonPlanId";
            ListBox1.DataBind();
            for (int j = 0; j < Dt.Rows.Count; j++)
            {
                arraylist2.Add(Dt.Rows[j]["LessonPlanName"].ToString());
            }
            foreach (ListItem item in ListBox2.Items)
            {
                if (arraylist2.Contains(item.Text))
                {
                    ListBox1.Items.Remove(item);
                }
            }
            ListBox1.Rows = Dt.Rows.Count;
        }
        else
        {
            if (ListBox1.Items.Count > 0)
            {
                ListBox1.Items.Clear();
            }
            ListBox1.DataSource = null;
            ListBox1.DataBind();
        }


    }

    //private void loadLessonPlan()
    //{
    //    ObjData = new clsData();
    //    sess = (clsSession)Session["UserSession"];
    //    oLessons = new clsLessons();
    //    Dts = new DataTable();
    //    Dt = new DataTable();
    //    string strSql = "";
    //    //strSql = "SELECT LPG.LessonPlanId,LP.LessonPlanName FROM LessonPlanGraphRelation LPG INNER JOIN LessonPlan LP on LPG.LessonPlanId=LP.LessonPlanId WHERE LPG.StudentId="+sess.StudentId+" AND LPG.SchoolId="+sess.SchoolId+" AND LPG.ClassId="+sess.Classid+" "+
    //    //         "AND PageType='BiweeklySessionGraph'";
    //    //strSql = "SELECT distinct LPG.LessonPlanId,LP.LessonPlanName+' ( '+case when lk.LookupName='Approved' then 'Active' else lk.LookupName end+' )' as LessonPlanName,lk.LookupId FROM LessonPlanGraphRelation LPG " +
    //    //         "INNER JOIN LessonPlan LP on LPG.LessonPlanId=LP.LessonPlanId INNER JOIN StdtLessonPlan ST on ST.LessonPlanId=LPG.LessonPlanId " +
    //    //         "INNER JOIN DSTempHdr DT on DT.StdtLessonplanId=ST.StdtLessonPlanId INNER JOIN LookUp lk on lk.LookupId=DT.StatusId " +
    //    //         "WHERE LPG.StudentId=" + sess.StudentId + " AND LPG.SchoolId=" + sess.SchoolId + " AND LPG.ClassId=" + sess.Classid + " AND PageType='BiweeklySessionGraph' AND DT.StatusId in " +
    //    //         "((Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Approved'), " +
    //    //         "(Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Maintanance'), " +
    //    //         "(Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive')) order by LookupId";
    //    strSql = "SELECT DISTINCT LessonPlanId,LessonPlanGraphRelationId,LessonPlanName FROM (SELECT  LPG.LessonPlanGraphRelationId,LPG.LessonPlanId,CONVERT(VARCHAR,(LP.LessonPlanName+' ( '+(SELECT TOP 1 CASE WHEN LK.LookupName='Approved' THEN 'Active' ELSE LK.LookupName END FROM " +
    //                   " DSTempHdr DT INNER JOIN LookUp LK ON LK.LookupId=DT.StatusId  WHERE StudentId='" + sess.StudentId + "' AND LookupType='TemplateStatus' AND LessonPlanId=LPG.LessonPlanId ORDER BY DSTempHdrId DESC)+' )')) AS LessonPlanName " +
    //                   " FROM LessonPlanGraphRelation LPG INNER JOIN LessonPlan LP ON LPG.LessonPlanId=LP.LessonPlanId  WHERE StudentId='" + sess.StudentId + "' AND PageType='BiweeklySessionGraph' ) GRPH WHERE  (LessonPlanName LIKE '%( Active )' OR " +
    //                   " LessonPlanName LIKE '%( Inactive )' OR LessonPlanName LIKE '%( Maintenance )') ORDER BY LessonPlanGraphRelationId";


    //    Dts = ObjData.ReturnDataTable(strSql, true);
    //    if (Dts != null)
    //    {
    //        ListBox2.DataSource = Dts;
    //        ListBox2.DataTextField = "LessonPlanName";
    //        ListBox2.DataValueField = "LessonPlanId";
    //        ListBox2.DataBind();
    //    }
    //    //string sqlQuery = "SELECT distinct hdr.LessonPlanId AS LessonPlanId,LP.LessonPlanName AS LessonPlan FROM DSTempHdr hdr INNER JOIN StdtLessonPlan slp ON " +
    //    //                  "hdr.LessonPlanId=slp.LessonPlanId AND hdr.studentid=slp.studentid INNER JOIN LessonPlan LP ON LP.LessonPlanId=hdr.LessonPlanId WHERE " +
    //    //                  "hdr.StudentId="+sess.StudentId+" AND hdr.StatusId =(Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Approved') ";
    //    //string sqlQuery = "SELECT distinct hdr.LessonPlanId AS LessonPlanId,LP.LessonPlanName +' ( '+case when lkp.LookupName='Approved' then 'Active' else lkp.LookupName end+' )' AS LessonPlan,lkp.LookupId  FROM DSTempHdr hdr INNER JOIN StdtLessonPlan slp ON " +
    //    //                  "hdr.LessonPlanId=slp.LessonPlanId AND hdr.studentid=slp.studentid INNER JOIN LessonPlan LP ON LP.LessonPlanId=hdr.LessonPlanId inner join LookUp LKP on lkp.LookupId=hdr.StatusId WHERE " +
    //    //                  "hdr.StudentId=" + sess.StudentId + " AND hdr.StatusId in ((Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Approved'), " +
    //    //                  "(Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Maintanance'), " +
    //    //                  "(Select  LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive')) order by LookupId";
    //    Dt = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "DTmp.DSTemplateName AS Name,,DTmp.VerNbr,DTmp.DSTemplateName +' ( '+CASE WHEN LU.LookupName='Approved' THEN 'Active' ELSE LU.LookupName END+' )' AS LessonPlanName,LP.LessonPlanId as LessonPlanId", "(LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='Inactive' ) ");


    //    //Dt = ObjData.ReturnDataTable(sqlQuery, true);
    //    //Dt = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.StdtLessonPlanId as LessonPlanId,LP.LessonPlanName as LessonPlan", "LU.LookupName='Approved' AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE')");
    //    if (Dt != null)
    //    {
    //        ListBox1.DataSource = Dt;
    //        ListBox1.DataTextField = "LessonPlanName";
    //        ListBox1.DataValueField = "LessonPlanId";
    //        ListBox1.DataBind();
    //        for (int j = 0; j < Dt.Rows.Count; j++)
    //        {
    //            arraylist2.Add(Dt.Rows[j]["LessonPlanName"].ToString().Trim());
    //        }
    //        foreach (ListItem item in ListBox2.Items)
    //        {
    //            if (arraylist2.Contains(item.Text))
    //            {
    //                ListBox1.Items.Remove(item);
    //            }
    //        }
    //    }


    //}

    private void saveLessonPlans()
    {
        string strSql = "";
        sess = (clsSession)Session["UserSession"];
        if (ListBox2.Items.Count > 0)
        {

            ObjData = new clsData();
            strSql = "DELETE FROM lessonPlanGraphRelation WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + ""
                + " AND PageType='BiweeklySessionGraph' AND ClassId=" + sess.Classid;
            ObjData.Execute(strSql);
            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                strSql = "INSERT INTO lessonPlanGraphRelation (LessonPlanId,SchoolId,StudentId,PageType,ClassId,CreatedBy,CreatedOn)"
                        + "VALUES(" + ListBox2.Items[i].Value + "," + sess.SchoolId + "," + sess.StudentId + ",'BiweeklySessionGraph'," + sess.Classid + "," + sess.LoginId + ",GETDATE())";
                ObjData.Execute(strSql);
            }
            //
        }
        if (ListBox2.Items.Count == 0)
        {
            strSql = "DELETE FROM lessonPlanGraphRelation WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + "AND PageType='BiweeklySessionGraph' AND ClassId=" + sess.Classid;
            ObjData.Execute(strSql);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        

        lbltxt.Visible = false;
        if (ListBox1.SelectedIndex >= 0)
        {
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                if (ListBox1.Items[i].Selected)
                {
                    if (!arraylist1.Contains(ListBox1.Items[i]))
                    {
                        arraylist1.Add(ListBox1.Items[i]);
                    }
                }
            }
            for (int i = 0; i < arraylist1.Count; i++)
            {
                if (!ListBox2.Items.Contains(((ListItem)arraylist1[i])))
                {
                    ListBox2.Items.Add(((ListItem)arraylist1[i]));
                }
                ListBox1.Items.Remove(((ListItem)arraylist1[i]));
            }
            ListBox2.SelectedIndex = -1;
        }
        else
        {
            lbltxt.Visible = true;
            lbltxt.Text = "Please select atleast one in Listbox1 to move";
        }
        if (ListBox1.Items.Count == 0)
        {
            ListBox1.Rows = 4;
        }
        else
        {
            ListBox1.Rows = ListBox1.Items.Count;
        }
        if (ListBox2.Items.Count == 0)
        {
            ListBox2.Rows = 4;
        }
        else
        {
            ListBox2.Rows = ListBox2.Items.Count;
        }

        //------------------------------
        
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);
        //-----------------------------------




    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        lbltxt.Visible = false;
        while (ListBox1.Items.Count != 0)
        {
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                ListBox2.Items.Add(ListBox1.Items[i]);
                ListBox1.Items.Remove(ListBox1.Items[i]);
            }
        }
        if (ListBox1.Items.Count == 0)
        {
            ListBox1.Rows = 4;
        }
        else
        {
            ListBox1.Rows = ListBox1.Items.Count;
        }
        if (ListBox2.Items.Count == 0)
        {
            ListBox2.Rows = 4;
        }
        else
        {
            ListBox2.Rows = ListBox2.Items.Count;
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        lbltxt.Visible = false;
        if (ListBox2.SelectedIndex >= 0)
        {
            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                if (ListBox2.Items[i].Selected)
                {
                    if (!arraylist2.Contains(ListBox2.Items[i]))
                    {
                        arraylist2.Add(ListBox2.Items[i]);
                    }
                }
            }
            for (int i = 0; i < arraylist2.Count; i++)
            {
                if (!ListBox1.Items.Contains(((ListItem)arraylist2[i])))
                {
                    ListBox1.Items.Add(((ListItem)arraylist2[i]));
                }
                ListBox2.Items.Remove(((ListItem)arraylist2[i]));
            }
            ListBox1.SelectedIndex = -1;
        }
        else
        {
            lbltxt.Visible = true;
            lbltxt.Text = "Please select atleast one in Listbox2 to move";
        }
        if (ListBox1.Items.Count == 0)
        {
            ListBox1.Rows = 4;
        }
        else
        {
            ListBox1.Rows = ListBox1.Items.Count;
        }
        if (ListBox2.Items.Count == 0)
        {
            ListBox2.Rows = 4;
        }
        else
        {
            ListBox2.Rows = ListBox2.Items.Count;
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        lbltxt.Visible = false;
        while (ListBox2.Items.Count != 0)
        {
            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                ListBox1.Items.Add(ListBox2.Items[i]);
                ListBox2.Items.Remove(ListBox2.Items[i]);
            }
        }
        if (ListBox1.Items.Count == 0)
        {
            ListBox1.Rows = 4;
        }
        else
        {
            ListBox1.Rows = ListBox1.Items.Count;
        }
        if (ListBox2.Items.Count == 0)
        {
            ListBox2.Rows = 4;
        }
        else
        {
            ListBox2.Rows = ListBox2.Items.Count;
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validate() == true)
            {
                if (highcheck.Checked == true)
                {

                    string scripts = "showPopups();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Showpop", scripts, true);

                }

                ObjData = new clsData();
                //string ReportXML = "SELECT [Name],CAST(CAST([Content] AS VARBINARY(MAX)) AS XML) AS reportXML FROM [Catalog] WHERE type = 2";
                //DataTable dt = ObjData.ReturnDataTable(ReportXML, false);
                sess = (clsSession)Session["UserSession"];
                int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                int templateId = Convert.ToInt32(Request.QueryString["pageid"].ToString());
                string StudName = "";
                string strQuery = "SELECT StudentLname+','+StudentFname AS StudentName FROM Student WHERE StudentId=" + studid;
                DataTable dt = ObjData.ReturnDataTable(strQuery, false);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        StudName = dt.Rows[0]["StudentName"].ToString();
                    }
                }
                Session["StudName"] = StudName;
                string[] StudentName = StudName.Split(',');
                ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
                ReportViewer AcademicReport = new ReportViewer();
                string AllLesson = "";
                if (hdnType.Value != "MaintenanceGraph")
                {
                    string result = "";

                    string StrQuery = "";
                    for (int i = 0; i < ListBox2.Items.Count; i++)
                    {
                        StrQuery += ListBox2.Items[i].Value + ",";
                    }
                    StrQuery = StrQuery.Substring(0, (StrQuery.Length - 1));
                    AllLesson = Convert.ToString(StrQuery);
                }
                else
                {
                    AllLesson = "";
                    string SetId = drpSetname.SelectedValue;

                    AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId));
                    Session["AcademicLessons"] = AllLesson;
                    if (AllLesson == "")
                    {
                        RV_LPReport.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
                        return;
                    }
                }
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                if (hdnType.Value == "SessionGraph")
                {
                    dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string StartDate = dtst.ToString("yyyy-MM-dd");
                    string enddate = dted.ToString("yyyy-MM-dd");

                    string TrendType = "NotNeed";
                    if (Convert.ToBoolean(chkreptrend.Checked))
                    {
                        TrendType = "Quarter";
                    }

                    string Events = "None,";
                    if (chkrepmajor.Checked == true)
                    {
                        Events += "Major,";
                    }
                    if (chkrepminor.Checked == true)
                    {
                        Events += "Minor,";
                    }
                    if (chkreparrow.Checked == true)
                    {
                        Events += "Arrow";
                    }

                    string LPStatus = "";
                    foreach (ListItem item in chkStatus.Items)
                    {
                        if (item.Selected == true)
                        {
                            if (item.Text == "Active")
                            {
                                LPStatus += "'Approved'";
                            }
                            else if (item.Text == "Maintenance")
                            {
                                if (LPStatus != "")
                                {
                                    LPStatus += ",";
                                }
                                LPStatus += "'Maintenance'";
                            }
                            else if (item.Text == "Inactive")
                            {
                                if (LPStatus != "")
                                {
                                    LPStatus += ",";
                                }
                                LPStatus += "'Inactive'";
                            }
                        }
                    }
                    //foreach (ListItem item in chkStatus.Items)
                    //{
                    //    if (item.Selected == true)
                    //    {
                    //        if (item.Text == "Active")
                    //        {
                    //            LPStatus += "Approved,";
                    //        }
                    //        else if (item.Text == "Maintenance")
                    //        {
                    //            LPStatus += "Maintenance,";
                    //        }
                    //        else if (item.Text == "Inactive")
                    //        {
                    //            LPStatus += "Inactive,";
                    //        }
                    //    }
                    //}
                    string StrStat = "SELECT LookupId FROM Lookup WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")";
                    DataTable LPStat = ObjData.ReturnDataTable(StrStat, false);
                    string StatusId = "";
                    for (int i = 0; i < LPStat.Rows.Count; i++)
                    {
                        StatusId += LPStat.Rows[i]["LookupId"].ToString() + ",";
                    }
                    //string[] StudentName = sess.StudentName.Split(',');
                    if (highcheck.Checked == true)
                    {
                        sDate = StartDate.ToString();
                        eDate = enddate.ToString();
                        sid = studid;
                        lid = AllLesson;
                        scid = sess.SchoolId;
                        cls = rbtnClassType.SelectedValue;
                        evnt = Events;
                        ioa = Convert.ToBoolean(chkrepioa.Checked).ToString();
                        trend = TrendType;
                        med = Convert.ToBoolean(chkrepmedi.Checked);
                        string lessname = "";
                        foreach (ListItem item in ddlLessonplan1.Items)
                        {

                            lessname += item.Text + ",";

                        }

                        if (lessname.EndsWith(","))
                        {
                            lessname = lessname.Substring(0, lessname.Length - 1);
                        }
                        lname = lessname;
                        lpstatus = LPStatus;

                        if (med)
                        {
                            ObjData = new clsData();
                            string squery = "SELECT * FROM (SELECT        SchoolId, StudentId, EventName, StdtSessEventType, Comment, EvntTs,CASE WHEN ( CASE WHEN EndTime='1900-01-01 00:00:00.000'  THEN NULL ELSE EndTime END) IS NULL THEN DATEADD(DAY,1, '" + eDate + "') ELSE EndTime END AS EndTime, EventType FROM            StdtSessEvent WHERE        (StdtSessEventType = 'Medication') AND  SchoolId = " + scid + "   AND StudentId =" + sid + ") MEDICATION WHERE EvntTs BETWEEN  '" + sDate + "'  AND '" + eDate + "' OR EndTime BETWEEN '" + sDate + "' AND  '" + eDate + "' OR (EvntTs <= '" + sDate + "' AND EndTime >= '" + eDate + "')";
                            DataTable medtab = ObjData.ReturnDataTable(squery, false);
                            int i = medtab.Rows.Count;
                            if (i < 1)
                            {
                                medno = true;
                                mednodata.Text = "No Data Available";
                                mednodata.Visible = true;
                                medcont.Visible = false;
                            }
                            else
                            {
                                mednodata.Visible = false;
                                medcont.Visible = true;
                            }

                        }
                        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
                        {
                            reptype = true;
                            if (rbtnIncidentalRegular.SelectedValue == "Regular")
                            {
                            }
                            else
                            {
                                inctype = true;
                            }
                        }
                        else
                        {
                            reptype = false;
                            if (rbtnIncidentalRegular.SelectedValue == "Regular")
                            {
                            }
                            else
                            {
                                inctype = true;
                            }
                        }
                        lbgraph.Visible = false;
                        cont.Visible = true;
                        sname.Visible = true;
                        lnam.Visible = true;
                        daterang.Visible = true;
                        mel.Visible = true;
                        deftxt.Visible = true;
                        Session["StudName"] = StudName;
                        ClientScript.RegisterStartupScript(GetType(), "", @"setTimeout(function() {exportChart();}, 500);", true);
                        tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Export Successfully Created...");
                        hdnExport.Value = "true";
                    }
                    else
                    {
                        AcademicReport.ProcessingMode = ProcessingMode.Remote;
                        AcademicReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                        if (rbtnIncidentalRegular.SelectedValue == "Regular")
                        {
                            AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ExportSessionBased"];
                        }
                        else
                        {
                            AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalExportSessionBased"];
                        }
                        AcademicReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportUrl"]);
                        AcademicReport.ServerReport.Refresh();

                        ReportParameter[] parm = new ReportParameter[12];
                        parm[0] = new ReportParameter("StartDate", StartDate.ToString());
                        parm[1] = new ReportParameter("EndDate", enddate.ToString());
                        parm[2] = new ReportParameter("StudentId", studid.ToString());
                        parm[3] = new ReportParameter("LessonPlan", AllLesson.ToString());
                        parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
                        parm[5] = new ReportParameter("ClsType", rbtnClassType.SelectedValue);//Parameter Value
                        parm[6] = new ReportParameter("Events", Events);
                        parm[7] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkrepioa.Checked).ToString());
                        parm[8] = new ReportParameter("Trendtype", TrendType);
                        parm[9] = new ReportParameter("IncludeMedication", Convert.ToBoolean(chkrepmedi.Checked).ToString());
                        parm[10] = new ReportParameter("StudentName", StudentName[0] + ", " + StudentName[1]);
                        parm[11] = new ReportParameter("LPStatus", StatusId);
                        AcademicReport.ServerReport.SetParameters(parm);
                    }
                }
                else if (hdnType.Value == "MaintenanceGraph")
                {
                    dtst = DateTime.ParseExact(txtStartDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dted = DateTime.ParseExact(txtEndDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string StartDate = dtst.ToString("yyyy-MM-dd");
                    string enddate = dted.ToString("yyyy-MM-dd");
                    string SetId = drpSetname.SelectedValue;
                    AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId));
                    Session["AcademicLessons"] = AllLesson;
                    if (Mhighcheck.Checked == true)
                    {
                        RV_LPReport.Visible = false;
                    if (AllLesson == "")
                    {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
                            return;
                        }
                        string TrendType = "NotNeed";
                        if (Convert.ToBoolean(chktrend.Checked))
                        {
                            TrendType = "Quarter";
                        }

                        string Events = "NotNeed,";
                        if (Convert.ToInt32(SetId) == -1)
                        {
                            if (chkmajor.Checked == true)
                            {
                                Events += "Major,";
                            }
                            if (chkminor.Checked == true)
                            {
                                Events += "Minor,";
                            }
                        }
                        if (chkarrow.Checked == true)
                        {
                            Events += "Arrow";
                        }


                        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
                        {
                            reptype = true;
                            if (rbtnIncidentalRegular.SelectedValue == "Regular")
                            {
                            }
                            else
                            {
                                inctype = true;
                            }
                        }
                        else
                        {
                            reptype = false;
                            if (rbtnIncidentalRegular.SelectedValue == "Regular")
                            {
                            }
                            else
                            {
                                inctype = true;
                            }
                        }
                        string script = @"setTimeout(function() {exportMchart('" + StartDate.ToString() + "', '" + enddate.ToString() + "','" + studid.ToString() + "','" + templateId.ToString() + "','" + sess.SchoolId.ToString() + "','" + Events + "','" + TrendType + "','" + Convert.ToBoolean(chkioa.Checked).ToString() + "','" + rbtnLsnClassType.SelectedValue + "','" + SetId + "','" + reptype + "','" + inctype + "');}, 500);";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript3", script, true);
                        tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Export Successfully Created...");
                    }
                    else
                    {
                        if (AllLesson == "")
                        {
                        RV_LPReport.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
                        return;
                    }
                    string TrendType = "NotNeed";
                    if (Convert.ToBoolean(chktrend.Checked))
                    {
                        TrendType = "Quarter";
                    }

                    string Events = "NotNeed,";
                    if (Convert.ToInt32(SetId) == -1)
                    {
                        if (chkmajor.Checked == true)
                        {
                            Events += "Major,";
                        }
                        if (chkminor.Checked == true)
                        {
                            Events += "Minor,";
                        }
                    }
                    if (chkarrow.Checked == true)
                    {
                        Events += "Arrow";
                    }

                    AcademicReport.ProcessingMode = ProcessingMode.Remote;
                    AcademicReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);

                    if (rbtnLsnIncidentalRegular.SelectedValue == "Regular")
                    {
                        AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ExportMaintenance"];
                    }
                    else
                    {
                        AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalExportMaintenance"];
                    }
                    AcademicReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportUrl"]);
                    AcademicReport.ShowParameterPrompts = false;

                    ReportParameter[] parm = new ReportParameter[10];
                    parm[0] = new ReportParameter("StartDate", StartDate.ToString());
                    parm[1] = new ReportParameter("EndDate", enddate.ToString());
                    parm[2] = new ReportParameter("StudentId", studid.ToString());
                    parm[3] = new ReportParameter("LessonHdrId", templateId.ToString());
                    parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
                    parm[5] = new ReportParameter("TrendType", TrendType);
                    parm[6] = new ReportParameter("ClsType", rbtnLsnClassType.SelectedValue);
                    parm[7] = new ReportParameter("SetId", SetId);
                    parm[8] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkioa.Checked).ToString());
                    parm[9] = new ReportParameter("Events", Events);
                    AcademicReport.ServerReport.SetParameters(parm);
                }
                }

                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, extension, deviceInfo;
                    if (highcheck.Checked == false && Mhighcheck.Checked==false)
                {
                    deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth><MarginTop>.5cm</MarginTop></DeviceInfo>";

                    //deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth></DeviceInfo>";

                    byte[] bytes = AcademicReport.ServerReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                    string outputPath = "";
                    if (hdnType.Value == "SessionGraph")
                    {
                        outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + StudName + "_Session-BasedReport.pdf");
                    }
                    else
                    {
                        string LessonName = Convert.ToString(ObjData.FetchValue("SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId='" + templateId + "'"));
                        string LPName = StudName.Replace(' ', '_') + "_" + LessonName.Replace(' ', '_');
                        LPName = LPName.Replace('-', '_').Replace(',', '_').Replace(':', '_').Replace('.', '_').Replace('"', '_').Replace(';', '_');
                        outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + LPName + ".pdf");
                    }
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
                    //Response.AddHeader("content-disposition", "attachment; filename=" + sess.StudentName + "_Session-BasedReport.pdf");

                    //Response.BinaryWrite(bytes); // create the file
                    //Response.Flush(); // send it to the client to download
                }
            }

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
            if (highcheck.Checked == true || Mhighcheck.Checked==true)
            {

                string sourcePdfPath = HttpContext.Current.Server.MapPath("~/StudentBinder/Exported/TempSession/");
                string outputFileName = Session["StudName"].ToString() + "SessionReport.pdf";
                string outputPath = HttpContext.Current.Server.MapPath("~/StudentBinder/Exported/" + outputFileName);
                int count = 0;
                string originalFileName = outputFileName;
                while (System.IO.File.Exists(outputPath))
                {
                    count++;
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(originalFileName);
                    string fileExtension = System.IO.Path.GetExtension(originalFileName);
                    outputFileName = fileNameWithoutExtension + " (" + count + ")" + fileExtension;
                    outputPath = HttpContext.Current.Server.MapPath("~/StudentBinder/Exported/" + outputFileName);
                }
                PdfDocument outputDocument = new PdfDocument();
                string[] fileNames = System.IO.Directory.GetFiles(sourcePdfPath);
                foreach (string fileName in fileNames)
                {
                    PdfDocument inputDocument = PdfReader.Open(fileName, PdfDocumentOpenMode.Import);
                    foreach (PdfPage page in inputDocument.Pages)
                    {
                        outputDocument.AddPage(page);
                    }
                    inputDocument.Close();
                    inputDocument.Dispose();
                    File.Delete(fileName);
                }
                outputDocument.Save(outputPath);
                ClientScript.RegisterStartupScript(GetType(), "", "CloseDownload();", true);
                outputDocument.Close();
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + outputPath + "\"");
                byte[] data = req.DownloadData(outputPath);
                response.BinaryWrite(data);
                response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                if (highcheck.Checked == true)
                {
                btnsubmit_Click(sender, e);
                }
                

            }
            else{
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
            response.End(); //Code reviewed- used because there is an issue when opening the file in Adobe reader
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void chkStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadLessonPlan();
    }

    protected void btnPrevious1_Click(object sender, EventArgs e)
    {

        int id = ddlLessonplan1.SelectedIndex;
        if (id >= 0)
        {
            if (id > 0)
            {
                ddlLessonplan1.SelectedIndex = id - 1;
            }
            string LessonId = ddlLessonplan1.SelectedValue.ToString();
            string LessonName = ddlLessonplan1.SelectedItem.Text.ToString();
            if (highcheck.Checked == true)
            {
                fillGraphhighchart(LessonId, LessonName);
                //graphPopup.Visible = false;
            }
            else
            {
                string script = "closePopup();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop", script, true);
            fillGraph(LessonId, LessonName);
        }
        }

    }
    protected void ddlLessonplan1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string LessonId = ddlLessonplan1.SelectedValue.ToString();
        string LessonName = ddlLessonplan1.SelectedItem.Text.ToString();
        if (highcheck.Checked == true)
        {
            fillGraphhighchart(LessonId, LessonName);
            //graphPopup.Visible = false;
        }
        else
        {
            string script = "closePopup();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop", script, true);
        fillGraph(LessonId, LessonName);
    }
    }
    protected void btnNext1_Click(object sender, EventArgs e)
    {
        int id = ddlLessonplan1.SelectedIndex;
        int count = ddlLessonplan1.Items.Count - 1;
        if (id <= count)
        {
            if (id < count)
            {
                ddlLessonplan1.SelectedIndex = id + 1;
            }
            string LessonId = ddlLessonplan1.SelectedValue.ToString();
            string LessonName = ddlLessonplan1.SelectedItem.Text.ToString();
            if (highcheck.Checked == true)
            {
                fillGraphhighchart(LessonId, LessonName);
                //graphPopup.Visible = false;
            }
            else
            {
                string script = "closePopup();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop", script, true);
                fillGraph(LessonId, LessonName);
            }
        }
    }

    private void fillGraphhighchart(string AllLesson, string LessonName)
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chkreptrend.Checked))
        {
            TrendType = "Quarter";
        }
        string Events = "None,";
        //if (chkrepevents.Checked == true)
        //{
        //    Events = "Major,Minor,Arrow";
        //}
        //else
        //{
        if (chkrepmajor.Checked == true)
        {
            Events += "Major,";
        }
        if (chkrepminor.Checked == true)
        {
            Events += "Minor,";
        }
        if (chkreparrow.Checked == true)
        {
            Events += "Arrow";
        }
        //}

        ///checks the lesson status
        ///
        string LPStatus = "";
        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    LPStatus += "Approved,";
                }
                else if (item.Text == "Maintenance")
                {
                    LPStatus += "Maintenance,";
                }
                else if (item.Text == "Inactive")
                {
                    LPStatus += "Inactive,";
                }
            }
        }
        LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            reptype = true;
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
            }
            else
            {
                inctype = true;
            }
        }
        else
        {
            reptype = false;
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
            }
            else
            {
                inctype = true;
            }
        }
        
      
            sDate = StartDate.ToString();
            eDate = enddate.ToString();
            sid = studid;
            lid = AllLesson;
            scid = sess.SchoolId;
            cls = rbtnClassType.SelectedValue;
            evnt = Events;
            ioa = Convert.ToBoolean(chkrepioa.Checked).ToString();
            trend = TrendType;
            med = Convert.ToBoolean(chkrepmedi.Checked);
            lname = LessonName;
            lpstatus = LPStatus;

            if (med)
            {
                ObjData = new clsData();
                string squery = "SELECT * FROM (SELECT        SchoolId, StudentId, EventName, StdtSessEventType, Comment, EvntTs,CASE WHEN ( CASE WHEN EndTime='1900-01-01 00:00:00.000'  THEN NULL ELSE EndTime END) IS NULL THEN DATEADD(DAY,1, '" + eDate + "') ELSE EndTime END AS EndTime, EventType FROM            StdtSessEvent WHERE        (StdtSessEventType = 'Medication') AND  SchoolId = " + scid + "   AND StudentId =" + sid + ") MEDICATION WHERE EvntTs BETWEEN  '" + sDate + "'  AND '" + eDate + "' OR EndTime BETWEEN '" + sDate + "' AND  '" + eDate + "' OR (EvntTs <= '" + sDate + "' AND EndTime >= '" + eDate + "')";
                DataTable medtab = ObjData.ReturnDataTable(squery, false);
                int i = medtab.Rows.Count;
                if (i < 1)
                {
                    medno = true;
                    mednodata.Text = "No Data Available";
                    mednodata.Visible = true;
                    medcont.Visible = false;
                }
                else
                {
                    mednodata.Visible = false;
                    medcont.Visible = true;
                }

            }

            lbgraph.Visible = false;
            cont.Visible = true;
            sname.Visible = true;
            lnam.Visible = true;
            daterang.Visible = true;
            mel.Visible = true;
            deftxt.Visible = true;
            HighchartGraph.Visible = true;
            string script = @"setTimeout(function() {loadchart('" + sDate + "', '" + eDate + "','" + sid + "','" + lid + "','" + scid + "','" + evnt + "','" + trend + "','" + ioa + "','" + cls + "','" + med + "','" + lpstatus + "','" + medno + "','" + reptype + "','" + inctype + "','" + lname + "');}, 500);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript", script, true);
        

    }

    private void fillGraph(string AllLesson,string LessonName)
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chkreptrend.Checked))
        {
            TrendType = "Quarter";
        }
        string Events = "None,";
        //if (chkrepevents.Checked == true)
        //{
        //    Events = "Major,Minor,Arrow";
        //}
        //else
        //{
        if (chkrepmajor.Checked == true)
        {
            Events += "Major,";
        }
        if (chkrepminor.Checked == true)
        {
            Events += "Minor,";
        }
        if (chkreparrow.Checked == true)
        {
            Events += "Arrow";
        }
        //}

        ///checks the lesson status
        ///
        string LPStatus = "";
        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    LPStatus += "Approved,";
                }
                else if (item.Text == "Maintenance")
                {
                    LPStatus += "Maintenance,";
                }
                else if (item.Text == "Inactive")
                {
                    LPStatus += "Inactive,";
                }
            }
        }
        LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));


        RV_LPReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            reptype = true;
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DupSessionBased"];
            }
            else
            {
                inctype = true;
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalDupSessionBased"];
            }
        }
        else
        {
            reptype = false;
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["SessionBased"];
            }
            else
            {
                inctype = true;
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalSessionBased"];
            }
        }
        RV_LPReport.ShowParameterPrompts = false;
        if (highcheck.Checked == false)
        {
    ReportParameter[] parm = new ReportParameter[11];
            parm[0] = new ReportParameter("StartDate", StartDate.ToString());
            parm[1] = new ReportParameter("EndDate", enddate.ToString());
            parm[2] = new ReportParameter("StudentId", studid.ToString());
            parm[3] = new ReportParameter("LessonPlan", AllLesson.ToString());
            parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
            parm[5] = new ReportParameter("ClsType", rbtnClassType.SelectedValue);
            parm[6] = new ReportParameter("Events", Events);
            parm[7] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkrepioa.Checked).ToString());
            parm[8] = new ReportParameter("Trendtype", TrendType);
            parm[9] = new ReportParameter("IncludeMedication", Convert.ToBoolean(chkrepmedi.Checked).ToString());
            parm[10] = new ReportParameter("LessonPlanName", LessonName);

            this.RV_LPReport.ServerReport.SetParameters(parm);

            RV_LPReport.ServerReport.Refresh();
        }
        //}
        //else
        //    RV_LPReport.Visible = false;
        else {
            sDate = StartDate.ToString();
            eDate = enddate.ToString();
            sid = studid;
            lid = AllLesson;
            scid = sess.SchoolId;
            cls = rbtnClassType.SelectedValue;
            evnt = Events;
            ioa = Convert.ToBoolean(chkrepioa.Checked).ToString();
            trend = TrendType;
            med = Convert.ToBoolean(chkrepmedi.Checked);
            lname = LessonName;
            lpstatus = LPStatus;

            if (med)
            {
                ObjData = new clsData();
                string squery = "SELECT * FROM (SELECT        SchoolId, StudentId, EventName, StdtSessEventType, Comment, EvntTs,CASE WHEN ( CASE WHEN EndTime='1900-01-01 00:00:00.000'  THEN NULL ELSE EndTime END) IS NULL THEN DATEADD(DAY,1, '" + eDate + "') ELSE EndTime END AS EndTime, EventType FROM            StdtSessEvent WHERE        (StdtSessEventType = 'Medication') AND  SchoolId = " + scid + "   AND StudentId =" + sid + ") MEDICATION WHERE EvntTs BETWEEN  '" + sDate + "'  AND '" + eDate + "' OR EndTime BETWEEN '" + sDate + "' AND  '" + eDate + "' OR (EvntTs <= '" + sDate + "' AND EndTime >= '" + eDate + "')";
                DataTable medtab = ObjData.ReturnDataTable(squery, false);
                int i = medtab.Rows.Count;
                if (i < 1)
                {
                    medno = true;
                    mednodata.Text = "No Data Available";
                    mednodata.Visible = true;
                    medcont.Visible = false;
    }
                else
                {
                    mednodata.Visible = false;
                    medcont.Visible = true;
                }

            }
            lbgraph.Visible = false;
            cont.Visible = true;
            sname.Visible = true;
            lnam.Visible = true;
            daterang.Visible = true;
            mel.Visible = true;
            deftxt.Visible = true;
        }
    }

    private void fillSet(int templateId)
    {
        ObjData = new clsData();
        Dt = new DataTable();
       //int LessonPlanId = Convert.ToInt32(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId));
       // string strQry = "SELECT DSTempSetId AS Id,SetCd AS Name FROM DSTempSet DS INNER JOIN StdtDSStat DST ON DS.DSTempHdrId=DST.DSTempHdrId " +
       //     " WHERE DS.ActiveInd='A' AND DS.DSTempHdrId in (Select DSTempHdrId from dstemphdr where studentId=" + sess.StudentId + " AND LessonPlanId = " + LessonPlanId + ") AND DST.StudentId=" + sess.StudentId + " AND DS.SortOrder < DST.NextSetNmbr";

       int LessonPlanId = Convert.ToInt32(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId));
        string strQry = "SELECT distinct DSTempSetId AS Id,SetCd AS Name FROM DSTempSet DS INNER JOIN StdtsessionHdr hdr On DS.DSTempHdrId=hdr.DSTempHdrId" +
            " WHERE DS.ActiveInd='A' AND DS.DSTempHdrId in (Select DSTempHdrId from dstemphdr where studentId=" + sess.StudentId + " AND LessonPlanId = " + LessonPlanId + ") AND hdr.StudentId=" + sess.StudentId + " AND hdr.IsMaintanace=1 AND SessionStatusCd='S' AND DS.DstempsetId=hdr.CurrentSetId";
       

        Dt = ObjData.ReturnDataTable(strQry, false);
        DataRow row = Dt.NewRow();
        row["Name"] = "Current Progress";
        row["Id"] = "-1";
        Dt.Rows.InsertAt(row, 0);
        DataView view = new DataView(Dt);
        DataTable distinctValues = view.ToTable(true, "Name");
        DataTable dtColCalc = new DataTable();
        dtColCalc.Columns.Add("Id", typeof(string));
        dtColCalc.Columns.Add("Name", typeof(string));

        foreach (DataRow DRow in distinctValues.Rows)
        {
            string resultcalc = DRow["Name"].ToString();
            DataRow[] result = Dt.Select(string.Format("Name ='{0}'", resultcalc.Replace("'", "''")));
            string resultId = "";
            foreach (DataRow rowset in result)
            {
                resultId += rowset[0] + ",";
            }
            dtColCalc.Rows.Add(resultId.TrimEnd(','), resultcalc);
        }
        if (dtColCalc != null)
        {
            drpSetname.DataSource = dtColCalc;
            drpSetname.DataTextField = "Name";
            drpSetname.DataValueField = "Id";
            drpSetname.DataBind();
        }
        if (dtColCalc.Rows.Count > 1)
        {
            foreach (ListItem item in (drpSetname as ListControl).Items)
            {
                if (item.Value == "0")
                {
                    item.Selected = true;
                }
            }
        }
    }

    private void GenerateMaintenanceReport()
    {
        ObjData = new clsData();
        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        int templateId = Convert.ToInt32(Request.QueryString["pageid"].ToString());
        tdMsg1.InnerHtml = "";
        RV_LPReport.Visible = true;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        hdnType.Value = "MaintenanceGraph";
        DateTime dtst =  new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtStartDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEndDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");
        string AllLesson = "";
        string SetId = drpSetname.SelectedValue;

        AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + ObjTempSess.TemplateId));
        Session["AcademicLessons"] = AllLesson;
        if (AllLesson == "")
        {
            RV_LPReport.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
            return;
        }
        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chktrend.Checked))
        {
            TrendType = "Quarter";
        }

        string Events = "NotNeed,";
        if (chkmajor.Checked == true)
        {
            Events += "Major,";
        }
        if (chkminor.Checked == true)
        {
            Events += "Minor,";
        }
        if (chkarrow.Checked == true)
        {
            Events += "Arrow";
        }

        RV_LPReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            if (rbtnLsnIncidentalRegular.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DupMaintenance"];
            }
            else
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalDupMaintenance"];
            }
        }
        else
        {
            if (rbtnLsnIncidentalRegular.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["Maintenance"];
            }
            else
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalMaintenance"];
            }
        }
        RV_LPReport.ShowParameterPrompts = false;

        ReportParameter[] parm = new ReportParameter[10];
        parm[0] = new ReportParameter("StartDate", StartDate.ToString());
        parm[1] = new ReportParameter("EndDate", enddate.ToString());
        parm[2] = new ReportParameter("StudentId", studid.ToString());
        parm[3] = new ReportParameter("LessonHdrId", templateId.ToString());
        parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
        parm[5] = new ReportParameter("TrendType", TrendType);
        parm[6] = new ReportParameter("ClsType", rbtnLsnClassType.SelectedValue);
        parm[7] = new ReportParameter("SetId", SetId);
        parm[8] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkioa.Checked).ToString());
        parm[9] = new ReportParameter("Events", Events);
        this.RV_LPReport.ServerReport.SetParameters(parm);
        RV_LPReport.ServerReport.Refresh();


    }

    
        private void GenerateHighchartMaintenanceReport()
    {
        ObjData = new clsData();
        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        int templateId = Convert.ToInt32(Request.QueryString["pageid"].ToString());
        tdMsg1.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        hdnType.Value = "MaintenanceGraph";
        DateTime dtst =  new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtStartDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEndDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");
        string AllLesson = "";
        string SetId = drpSetname.SelectedValue;
        RV_LPReport.Visible = false;
        AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + ObjTempSess.TemplateId));
        Session["AcademicLessons"] = AllLesson;
        if (AllLesson == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
            return;
        }
        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chktrend.Checked))
        {
            TrendType = "Quarter";
        }

        string Events = "NotNeed,";
        if (chkmajor.Checked == true)
        {
            Events += "Major,";
        }
        if (chkminor.Checked == true)
        {
            Events += "Minor,";
        }
        if (chkarrow.Checked == true)
        {
            Events += "Arrow";
        }

        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            reptype = true;
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
            }
            else
            {
                inctype = true;
            }
        }
        else
        {
            reptype = false;
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
            }
            else
            {
                inctype = true;
            }
        }
            HighchartGraph.Visible = true;
            sname.Visible = true;
            lnam.Visible = true;
            daterang.Visible = true;
            mel.Visible = true;
            deftxt.Visible = true;
            string script = @"setTimeout(function() {loadMchart('" + StartDate.ToString() + "', '" + enddate.ToString() + "','" + studid.ToString() + "','" + templateId.ToString() + "','" + sess.SchoolId.ToString() + "','" + Events + "','" + TrendType + "','" + Convert.ToBoolean(chkioa.Checked).ToString() + "','" + rbtnLsnClassType.SelectedValue + "','" + SetId + "','" + reptype + "','" + inctype + "');}, 500);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript2", script, true);
        }

    protected void btnMaintenanceGraph_Click(object sender, EventArgs e)
    {
        if (Validate() == true)
        {
            if (Mhighcheck.Checked == false)
            {
            GenerateMaintenanceReport();
        }

        else
        {
                GenerateHighchartMaintenanceReport();

            }
           
        }
        else
            {
            if (Mhighcheck.Checked == false)
            {
                RV_LPReport.Visible = false;
            }
            else
            {

                string script = "warningmsgpop();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepops2", script, true);
            }
            btnPrevious1.Visible = false;
            ddlLessonplan1.Visible = false;
            btnNext1.Visible = false;
        }
    }

    protected void drpSetname_SelectedIndexChanged(object sender, EventArgs e)
    {
        RV_LPReport.Visible = false;
        string SetId = drpSetname.SelectedValue;
        //if (Convert.ToInt32(SetId) == -1)
        if (SetId == "-1")
        {
            chkioa.Visible = true;
        }
        else
        {
            chkioa.Visible = false;
        }
    }

    protected void RefreshMaintenance_Click(object sender, EventArgs e)
    {
        chkioa.Visible = true;
        rbtnLsnClassType.SelectedValue = "Day,Residence";
        rbtnLsnIncidentalRegular.SelectedValue = "Regular";
        tdMsg1.InnerHtml = "";
        FillData();
    }

    public void FillData()
    {
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        RV_LPReport.Visible = false;
        txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy").Replace("-", "/");
        txtStartDate.Text = DateTime.Now.Date.AddDays(-10).ToString("MM/dd/yyyy").Replace("-", "/");        
        chkmajor.Checked = true;
        chkminor.Checked = true;
        chkarrow.Checked = true;
        chkioa.Checked = true;
        chktrend.Checked = true;
        btnPrevious1.Visible = false;
        ddlLessonplan1.Visible = false;
        btnNext1.Visible = false;        
        fillSet(ObjTempSess.TemplateId);        
        LessonDiv.Visible = true;
        btnMaintenanceGraph.Visible = true;
        btnRefresh.Visible = false;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getSessionReport(string StartDate, string enddate, int studid, string AllLesson, int SchoolId, string Events, string Trendtype, string IncludeIOA, string Clstype)
    {
        objData = new clsData();
        string str = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        SqlConnection cn = new SqlConnection(str);
        cn.Open();

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[BiweeklySessionReport]";

        DataTable dt = objData.ReturnSessTable(proc, StartDate, enddate, studid, AllLesson, SchoolId, Events, Trendtype, IncludeIOA, Clstype);

        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getSessionReportNext(string lplan, int studid)
    {
        objData = new clsData();
        string str = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        SqlConnection cn = new SqlConnection(str);
        cn.Open();

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[SessionGraphReport]";

        DataTable dt = objData.ReturnSessTableNext(proc, lplan, studid);

        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getmedAcademicReport(string StartDate, string enddate, int studid, int SchoolId)
    {
        objData = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        string squery = "SELECT * FROM (SELECT        SchoolId, StudentId, EventName, StdtSessEventType, Comment, EvntTs,CASE WHEN ( CASE WHEN EndTime='1900-01-01 00:00:00.000'  THEN NULL ELSE EndTime END) IS NULL THEN DATEADD(DAY,1, '" + enddate + "') ELSE EndTime END AS EndTime, EventType FROM            StdtSessEvent WHERE        (StdtSessEventType = 'Medication') AND  SchoolId = " + SchoolId + "   AND StudentId =" + studid + ") MEDICATION WHERE EvntTs BETWEEN  '" + StartDate + "'  AND '" + enddate + "' OR EndTime BETWEEN '" + StartDate + "' AND  '" + enddate + "' OR (EvntTs <= '" + StartDate + "' AND EndTime >= '" + enddate + "')";
        DataTable dt = objData.ReturnDataTable(squery, false);
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);

    }
    [WebMethod]
    public static string[] getgraphs(string base64, string chartId)
    {
         try
        {
            HttpContext currentContext = HttpContext.Current;

            if (currentContext != null)
            {
                string base64Images = base64.Split(',')[1];
                string pdfFilePath = currentContext.Server.MapPath("~/StudentBinder/Exported/TempSession/stud" + chartId + ".pdf");

                PdfSharp.Pdf.PdfDocument pdfDocument = new PdfSharp.Pdf.PdfDocument();
                PdfSharp.Pdf.PdfPage page = pdfDocument.AddPage();
                page.Orientation = PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                byte[] bytes = Convert.FromBase64String(base64Images);
                using (MemoryStream imageStream = new MemoryStream(bytes))
                {
                    XImage image = XImage.FromStream(imageStream);

                    double pageWidth = page.Width;
                    double pageHeight = page.Height;
                    double imageWidth = image.PixelWidth;
                    double imageHeight = image.PixelHeight;

                    double scale = Math.Min(pageWidth / imageWidth, pageHeight / imageHeight);
                    double newWidth = imageWidth * scale;
                    double newHeight = imageHeight * scale;

                    double x = (pageWidth - newWidth) / 2;
                    double y = (pageHeight - newHeight) / 2;

                    gfx.DrawImage(image, x, y, newWidth, newHeight);
                }

                pdfDocument.Save(pdfFilePath);
                return new string[] { pdfFilePath };
            }
            else
            {
                return new string[] { "Error: HttpContext is null" };
            }
        }
        catch (Exception ex)
        {
            return new string[] { "Error: " + ex.Message };
        }
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        if (highcheck.Checked == true)
        {
            string script = "showPopup();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "show", script, true);
        }
        string sourcePdfPath = HttpContext.Current.Server.MapPath("~/StudentBinder/Exported/TempSession/");
        if (Directory.Exists(sourcePdfPath))
        {
            try
            {
                string[] pdfFiles = Directory.GetFiles(sourcePdfPath, "*.pdf");
                foreach (string pdfFile in pdfFiles)
                {
                    File.Delete(pdfFile);
                }
                btnsubmit_Click(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getMaintreport(string startdate, string enddate, int studid,string lessid,int schoolid,string events,string trend, string checkioa,string classtype,string setid)
    {
        objData = new clsData();

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[MaintenanceReport]";

        DataTable dt = objData.ReturnMainttable(proc, startdate, enddate, studid, lessid, schoolid, events, trend, checkioa, classtype, setid);

        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string NoMaintReport(int studid, string lessid)
    {
        objData = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        string sqlStr = "SELECT S.LastName + ', ' + S.FirstName AS StudentName,(SELECT TOP 1 ('Tx: ' + (SELECT LookupName FROM LookUp WHERE LookupId= [TeachingProcId]) + ';' + (SELECT LookupName FROM LookUp WHERE LookupId= [PromptTypeId])))Treatment,(SELECT TOP 1 'Correct Response: ' + DS.StudCorrRespDef WHERE DS.StudCorrRespDef <>'')Deftn,DSTemplateName as LessonPlanName FROM StudentPersonal S JOIN DSTempHdr DS ON S.StudentPersonalId=DS.StudentId WHERE S.StudentPersonalId=" + studid + " AND DSTempHdrId=" + lessid;
        DataTable dt = objData.ReturnDataTable(sqlStr, false);
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);

    }
}