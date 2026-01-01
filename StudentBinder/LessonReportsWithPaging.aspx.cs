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
using System.Drawing.Printing;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Text;



public partial class StudentBinder_LessonReportsWithPaging : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    static clsData objData = null;
    ArrayList arraylist1 = new ArrayList();
    ArrayList arraylist2 = new ArrayList();
    DataTable Dt = null;
    DataTable Dts = null;
  static  DataTable Dtexp = null;
    clsLessons oLessons;
    public bool reptype=false;
    public bool inctype = false;
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
    public bool medno=false;
    public string stname = "";
    protected void Page_Load(object sender, EventArgs e)
    {
      //  lbgraph.Visible = false;
        mednodata.Visible = false;
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
                ClassDatatable objdatacls = new ClassDatatable();
                hfPopUpValue.Value = "false";
                if (Request.QueryString["studid"] != null)
                {
                    //if (CheckUrlExists())
                    //{
                    int pageid = Convert.ToInt32(Request.QueryString["pageid"].ToString());
                    int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                    sess.StudentId = studid;

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "loadWait();", true);
                    ObjTempSess.TemplateId = pageid;
                    if (pageid == 0)
                    {
                        chkbxevents.Checked = true;
                        chkbxIOA.Checked = true;
                        chktrend.Checked = true;
                        chkmedication.Checked = false;
                        txtEdate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy").Replace("-", "/");
                        txtSdate.Text = DateTime.Now.Date.AddDays(-91).ToString("MM/dd/yyyy").Replace("-", "/");
                        //rbtnClassTypeall.SelectedValue = objdatacls.GetClassType(sess.Classid);
                        hdnallLesson.Value = "AllLessons";
                        LessonDiv.Visible = false;

                    }
                    else
                    {
                        chkrepevents.Checked = true;
                        chkrepioa.Checked = true;
                        chkreptrend.Checked = true;
                        chkrepmedi.Checked = false;
                        btnPrevious.Visible = false;
                        ddlLessonplan.Visible = false;
                        btnNext.Visible = false;

                        txtrepEdate.Text = DateTime.Now.Date.AddDays(1).ToString("MM/dd/yyyy").Replace("-", "/");
                        txtRepStart.Text = DateTime.Now.Date.AddDays(-91).ToString("MM/dd/yyyy").Replace("-", "/");
                        //rbtnClassType.SelectedValue = objdatacls.GetClassType(sess.Classid);
                        fillSet(pageid);
                        fillColumnCalc(pageid);
                        hdnallLesson.Value = "";
                        GenerateLessonPlanReport();
                        LessonDiv.Visible = true;
                    }
                    // generateHighchartReport();
                    loadLessonPlan();
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Report Server is not Available');", true);
                    //    RV_LPReport.Visible = false;
                    //    return;
                    //}


                   
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



    protected bool CheckUrlExists()
    {
        string ReportUrl = ConfigurationManager.AppSettings["ReportPath"].ToString();
        try
        {
            var request = WebRequest.Create(ReportUrl) as HttpWebRequest;
            request.Method = "HEAD";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
        catch
        {
            return false;
        }
    }
    private void fillColumnCalc(int templateId)
    {
        ObjData = new clsData();
        Dt = new DataTable();
        string strQry = "";
        string strQuery = "SELECT hdr.DSTempHdrId As ID FROM DSTempHdr hdr WHERE hdr.StudentId=" + sess.StudentId + " AND hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId + ") AND hdr.StatusId IN ((Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved'),(Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive'),(Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance')) ";

        strQry = "SELECT CALC.DSTempSetColCalcId AS Id,DCOL.ColName+' - '+CALC.CalcType AS Name FROM DSTempSetCol DCOL INNER JOIN DSTempSetColCalc CALC " +
        " ON DCOL.DSTempSetColId=CALC.DSTempSetColId WHERE IncludeInGraph=1 AND DCOL.ActiveInd='A' AND CALC.ActiveInd='A' AND DCOL.DSTempHdrId IN (" + strQuery + ")";
        Dt = ObjData.ReturnDataTable(strQry, false);
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
            foreach (DataRow row in result)
            {
                resultId += row[0] + ",";
            }
            dtColCalc.Rows.Add(resultId.TrimEnd(','), resultcalc);
            //tblTest.Rows.Add(tRow);
        }

        if (dtColCalc != null)
        {
            drpColumn.DataSource = dtColCalc;
            drpColumn.DataTextField = "Name";
            drpColumn.DataValueField = "Id";
            drpColumn.DataBind();
        }
        if (dtColCalc.Rows.Count >= 1)
        {
            foreach (ListItem item in (drpColumn as ListControl).Items)
            {
                item.Selected = true;
            }
        }
    }
    private void fillSet(int templateId)
    {
        ObjData = new clsData();
        Dt = new DataTable();
        string strQry = "";
        string strQuery = "SELECT hdr.DSTempHdrId As ID FROM DSTempHdr hdr WHERE hdr.StudentId=" + sess.StudentId + " AND hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateId + ") AND hdr.StatusId IN ((Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved'),(Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive'),(Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance')) ";

        strQry = "SELECT DSTempSetId AS Id,SetCd AS Name FROM DSTempSet WHERE ActiveInd='A' AND DSTempHdrId IN (" + strQuery + ")";
        Dt = ObjData.ReturnDataTable(strQry, false);
        DataRow row = Dt.NewRow();
        row["Name"] = "Normal Graph View";
        row["Id"] = "0";
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
            //tblTest.Rows.Add(tRow);
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

    private void loadLessonPlan()
    {
        ObjData = new clsData();
        sess = (clsSession)Session["UserSession"];
        oLessons = new clsLessons();
        Dts = new DataTable();
        Dt = new DataTable();
        string StatusCheck = "";

        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    StatusCheck += "'Approved',";
                }
                else if (item.Text == "Maintenance")
                {
                    StatusCheck += "'Maintenance',";
                }
                else if (item.Text == "Inactive")
                {
                    StatusCheck += "'Inactive',";
                }
            }
        }
        if (StatusCheck != "")
        {
        StatusCheck = StatusCheck.Substring(0, (StatusCheck.Length - 1));
        }
        else
        {
            StatusCheck = "''";
        }
        //strSql = " SELECT  distinct DTmp.DSTemplateName AS Name,DTmp.VerNbr,DTmp.LessonOrder,DTmp.DSTemplateName +' ( '+CASE WHEN LU.LookupName='Approved' THEN 'Active' ELSE LU.LookupName END+' )' AS LessonPlanName,DTmp.DSTempHdrId as LessonPlanId  FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)   ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId)    INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId + " AND   StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND (LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='Inactive' OR DTmp.DSMode='')  ORDER BY DTmp.LessonOrder ";

        //Dt=ObjData.ReturnDataTable("SELECT  distinct DTmp.DSTemplateName +' ( '+CASE WHEN LU.LookupName='Approved' THEN 'Active' ELSE LU.LookupName END+' )' as  LessonPlanName,DTmp.DSTempHdrId as LessonPlanId,DTmp.LessonOrder  FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN  LookUp LU ON LU.LookupId=DTmp.StatusId)   ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId + " AND   StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND (LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR  DTmp.DSMode='Inactive') ORDER BY  DTmp.LessonOrder",false);
        //Dt = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "DTmp.DSTemplateName +' ( '+CASE WHEN LU.LookupName='Approved' THEN 'Active' ELSE LU.LookupName END+' )' as LessonPlanName,DTmp.DSTempHdrId as LessonPlanId", "(LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='Inactive')");
        //Dt = ObjData.ReturnDataTable(strSql, false);

        //Dt = ObjData.ReturnDataTable("SELECT DISTINCT DTmp.LessonPlanId,DTmp.LessonOrder,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=DTmp.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " ORDER BY DSTempHdrId DESC)+' ( '+CASE WHEN LookupName='Approved' THEN 'Active' ELSE LookupName END+' )' AS LessonPlanName FROM DSTempHdr DTmp INNER JOIN LookUp ON DTmp.StatusId=LookUp.LookupId WHERE StudentId=" + sess.StudentId + " AND (" + StatusCheck + ") ORDER BY  DTmp.LessonOrder", false);

        string selLPs = "SELECT *, (SELECT CASE WHEN EXISTS (SELECT StatusId  FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + sess.StudentId + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Approved' AND L.LookupName IN ( " + StatusCheck + ")) "+
            "THEN (SELECT TOP 1 DSTemplateName +' ( Active )' FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + sess.StudentId + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Approved' AND L.LookupName IN ( " + StatusCheck + ")) "+
            "ELSE CASE WHEN EXISTS (SELECT StatusId  FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + sess.StudentId + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Maintenance' AND L.LookupName IN ( " + StatusCheck + ")) "+
            "THEN (SELECT TOP 1 DSTemplateName +' ( Maintenance )' FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + sess.StudentId + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Maintenance' AND L.LookupName IN ( " + StatusCheck + ")) "+
            "ELSE CASE WHEN EXISTS (SELECT TOP 1 StatusId  FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + sess.StudentId + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND LookupName = 'Inactive' AND L.LookupName IN ( " + StatusCheck + ")) "+
            "THEN (SELECT TOP 1 DSTemplateName +' ( Inactive )' FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + sess.StudentId + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND LookupName = 'Inactive' AND L.LookupName IN ( " + StatusCheck + ") ORDER BY DSTempHdrId DESC) " +
            "END END END ) AS LessonPlanName FROM (SELECT D.LessonPlanId, D.LessonOrder FROM DSTempHdr D INNER JOIN LookUp L ON D.StatusId=L.LookupId "+
            "WHERE StudentId=" + sess.StudentId + " AND L.LookupType = 'TemplateStatus' AND L.LookupName IN ( " + StatusCheck + ") GROUP BY D.LessonPlanId, D.LessonOrder) LSN ORDER BY  LessonOrder";

        Dt = ObjData.ReturnDataTable(selLPs, false);

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

    private void saveLessonPlans()
    {
        string strSql = "";
        sess = (clsSession)Session["UserSession"];
        if (ListBox2.Items.Count > 0)
        {

            ObjData = new clsData();
            strSql = "DELETE FROM lessonPlanGraphRelation WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND PageType='BiweeklyGraph' AND ClassId=" + sess.Classid;
            ObjData.Execute(strSql);
            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                strSql = "INSERT INTO lessonPlanGraphRelation (LessonPlanId,SchoolId,StudentId,PageType,ClassId,CreatedBy,CreatedOn)"
                        + "VALUES(" + ListBox2.Items[i].Value + "," + sess.SchoolId + "," + sess.StudentId + ",'BiweeklyGraph'," + sess.Classid + "," + sess.LoginId + ",GETDATE())";
                ObjData.Execute(strSql);
            }
            //
        }
        if (ListBox2.Items.Count == 0)
        {
            strSql = "DELETE FROM lessonPlanGraphRelation WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND PageType='BiweeklyGraph' AND ClassId=" + sess.Classid;
            ObjData.Execute(strSql);
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
    private bool Validation()
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
                tdMsg.InnerHtml = clsGeneral.warningMsg("Start Date must be before the End Date.");
                return result;
            }
           
        }
        return result;

    }

    private bool LessonValidation()
    {
        bool result = true;
        if (txtRepStart.Text == "")
        {
            result = false;
            tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Enter Start Date");
            return result;
        }
        else if (txtrepEdate.Text == "")
        {
            result = false;
            tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Enter End Date");
            return result;
        }
        if (txtRepStart.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Invalid Start date");
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
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Invalid End date");
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
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Start Date must be before the End Date");
                return result;
            }
            
        }
        return result;

    }
    
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ListBox2.Items.Count > 0)
            {
                if (Validation() == true)
                {
                    hfPopUpValue.Value = "true";
                    // saveLessonPlans();
                    btnPrevious.Visible = true;
                    ddlLessonplan.Visible = true;
                    btnNext.Visible = true;
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
                    btnPrevious.Visible = false;
                    ddlLessonplan.Visible = false;
                    btnNext.Visible = false;
                    graphPopup.Visible = true;
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

        ObjData = new clsData();
        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        //ObjData.ExecuteSp();
        tdMsg.InnerHtml = "";
        int LessonPlanId = 0;
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
        hdnType.Value = "MultipleLessonPlan";
            Session["hdnType"] = hdnType.Value;
        string AllLesson = "";

        if (Convert.ToInt32(Request.QueryString["pageid"].ToString()) == 0)
        {
            hdnallLesson.Value = "AllLessons";
        }
        else
        {
            LessonPlanId = Convert.ToInt32(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + ObjTempSess.TemplateId));
        }
        if (hdnallLesson.Value.Trim() == "AllLessons")
        {
            string StrSelected = "";
            for (int i = 0; i < ListBox2.Items.Count; i++)
            {
                StrSelected += ListBox2.Items[i].Value + ",";
            }
            StrSelected = StrSelected.Substring(0, (StrSelected.Length - 1));
            //string StrNewQry = "SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),LessonPlanId) FROM (SELECT DISTINCT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId IN (" + StrSelected + ")) DSTEMP FOR XML PATH('')) ,1,1,'')";
            AllLesson = Convert.ToString(StrSelected);
        }
        else
        {
            AllLesson = LessonPlanId.ToString();
        }

        if (AllLesson == "")
        {
            if (highcheck.Checked == false)
            {
            RV_LPReport.Visible = false;
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
            return;
        }
        
        Session["AcademicLessons"] = AllLesson;

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

        string result = "";
        string[] listVal = LPStatus.Split(',');
        if (listVal.Length >= 1)
        {
            for (int i = 0; i < listVal.Length; i++)
            {
                result +="'" +listVal[i] + "',";
            }
        }
        result = result.Substring(0, (result.Length - 1));

        string strLess = "SELECT *, (SELECT CASE WHEN EXISTS (SELECT StatusId FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + studid + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Approved' AND L.LookupName IN (" + result + ")) THEN " +
            "(SELECT TOP 1 DSTemplateName FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + studid + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName='Approved' AND L.LookupName IN (" + result + ")) "+
            "ELSE CASE WHEN EXISTS (SELECT StatusId FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + studid + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Maintenance' AND L.LookupName IN (" + result + ")) " +
            "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + studid + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND L.LookupName = 'Maintenance' AND L.LookupName IN (" + result + ")) "+
            "ELSE CASE WHEN EXISTS (SELECT TOP 1 StatusId FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + studid + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND LookupName = 'Inactive' AND L.LookupName IN (" + result + ")) " +
            "THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr DH LEFT JOIN LookUp L ON DH.StatusId = L.LookupId WHERE DH.StudentId = " + studid + " AND DH.LessonPlanId = LSN.LessonPlanId AND L.LookupType = 'TemplateStatus' AND LookupName = 'Inactive' AND L.LookupName IN (" + result + ") "+
            "ORDER BY DSTempHdrId DESC) END END END ) AS LessonPlanName FROM (SELECT D.LessonPlanId, D.LessonOrder FROM DSTempHdr D INNER JOIN LookUp L ON D.StatusId=L.LookupId WHERE StudentId=" + studid + " and "+
            "LessonPlanId IN (" + AllLesson + ") AND L.LookupType = 'TemplateStatus' AND L.LookupName IN (" + result + ") GROUP BY D.LessonPlanId, D.LessonOrder) LSN ORDER BY  LessonOrder";
        DataTable DTLesson = ObjData.ReturnDataTable(strLess, false);

        DataTable dtLP = new DataTable();
            binddropdown(dtLP, DTLesson);
            ViewState["LessonData"] = dtLP;
            AllLesson = DTLesson.Rows[0].ItemArray[0].ToString();
            if (highcheck.Checked == true)
            {
                fillGraphhighchart(AllLesson);

            }
            else {
                fillGraph(AllLesson);
            }
    }

    private void binddropdown(DataTable dtLP, DataTable DTLesson)

    {
        dtLP.Columns.Add("Id", typeof(string));
        dtLP.Columns.Add("Name", typeof(string));

        if (DTLesson != null && DTLesson.Rows.Count > 0)
        {
                foreach (DataRow drLessn in DTLesson.Rows)
                {
                    DataRow drr = dtLP.NewRow();
                    drr["Id"] = drLessn.ItemArray[0];
                    drr["Name"] = drLessn.ItemArray[2];
                    dtLP.Rows.Add(drr);
                }

            }

        ddlLessonplan.DataSource = dtLP;
        ddlLessonplan.DataTextField = "Name";
        ddlLessonplan.DataValueField = "Id";
        ddlLessonplan.DataBind();
    }

    private void fillGraph(string AllLesson)
    {
        ObjData = new clsData();
        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chktrend.Checked))
        {
            TrendType = "Quarter";
        }
        string Events = "None,";
        if (chkbxevents.Checked == true)
        {
            Events = "Major,Minor,Arrow";
        }
        else
        {
            if (chkbxmajor.Checked == true)
            {
                Events += "Major,";
            }
            if (chkbxminor.Checked == true)
            {
                Events += "Minor,";
            }
            if (chkbxarrow.Checked == true)
            {
                Events += "Arrow";
            }
        }
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

        string strLpStat = "SELECT STUFF((SELECT ','+CONVERT(VARCHAR(25), LookupId) FROM (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")) LookupId FOR XML PATH('')),1,1,'')";
        LPStatus = ObjData.FetchValue(strLpStat).ToString();

        RV_LPReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            reptype = true;
            if (rbtnIncidentalRegularall.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DupAcademic"];
            }
            else
            {
                inctype = true;
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalDupAcademic"];
            }
        }
        else
        {
            reptype = false;
            if (rbtnIncidentalRegularall.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["Academic"];
            }
            else
            {
                inctype = true;
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalAcademic"];
            }

        }

        RV_LPReport.ShowParameterPrompts = false;
        if (highcheck.Checked == false)
        {
        ReportParameter[] parm = new ReportParameter[11];
        parm[0] = new ReportParameter("SDate", StartDate.ToString());
        parm[1] = new ReportParameter("EDate", enddate.ToString());
        parm[2] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
        parm[3] = new ReportParameter("StudentId", studid.ToString());
        parm[4] = new ReportParameter("AllLesson", AllLesson.ToString());
        parm[5] = new ReportParameter("Events", Events);
        parm[6] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkbxIOA.Checked).ToString());
        parm[7] = new ReportParameter("Trendtype", TrendType);
        parm[8] = new ReportParameter("IncludeMedication", Convert.ToBoolean(chkmedication.Checked).ToString());
        parm[9] = new ReportParameter("ClsType", rbtnClassTypeall.SelectedValue);
        parm[10] = new ReportParameter("LPStatus", LPStatus);

        this.RV_LPReport.ServerReport.SetParameters(parm);
        RV_LPReport.ServerReport.Refresh();
    }
        else
        {
                sDate = StartDate.ToString();
                eDate = enddate.ToString();
                sid = studid;
                lid = AllLesson.ToString();
                scid = sess.SchoolId;
                evnt = Events;
                trend = TrendType;
                ioa = Convert.ToBoolean(chkbxIOA.Checked).ToString();
                cls = rbtnClassTypeall.SelectedValue;
    med = Convert.ToBoolean(chkmedication.Checked);
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
    mel.Visible = true;
    deftxt.Visible = true;
                Session["lid"] = lid;
              //  string script = "loadchart('" + sDate + "', '" + eDate + "','" + sid + "','" + lid + "','" + scid + "','" + evnt + "','" + trend + "','" + ioa + "','" + cls + "','" + med + "','" + lpstatus + "','" + medno + "','" + reptype + "','" + inctype + "');";
                string script = @"setTimeout(function() {loadchart('" + sDate + "', '" + eDate + "','" + sid + "','" + lid + "','" + scid + "','" + evnt + "','" + trend + "','" + ioa + "','" + cls + "','" + med + "','" + lpstatus + "','" + medno + "','" + reptype + "','" + inctype + "');}, 500);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript", script, true);
        }
    }
    private void fillGraphhighchart(string AllLesson)
    {
        ObjData = new clsData();
        int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");

        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chktrend.Checked))
        {
            TrendType = "Quarter";
        }
        string Events = "None,";
        if (chkbxevents.Checked == true)
        {
            Events = "Major,Minor,Arrow";
        }
        else
        {
            if (chkbxmajor.Checked == true)
            {
                Events += "Major,";
            }
            if (chkbxminor.Checked == true)
            {
                Events += "Minor,";
            }
            if (chkbxarrow.Checked == true)
            {
                Events += "Arrow";
            }
        }
        string LPStatus = "";
        foreach (System.Web.UI.WebControls.ListItem item in chkStatus.Items)
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

        string strLpStat = "SELECT STUFF((SELECT ','+CONVERT(VARCHAR(25), LookupId) FROM (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")) LookupId FOR XML PATH('')),1,1,'')";
        LPStatus = ObjData.FetchValue(strLpStat).ToString();
        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            reptype = true;
            if (rbtnIncidentalRegularall.SelectedValue == "Regular")
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
            if (rbtnIncidentalRegularall.SelectedValue == "Regular")
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
                lid = AllLesson.ToString();
                scid = sess.SchoolId;
                evnt = Events;
                trend = TrendType;
                ioa = Convert.ToBoolean(chkbxIOA.Checked).ToString();
                cls = rbtnClassTypeall.SelectedValue;
                med = Convert.ToBoolean(chkmedication.Checked);
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
                mel.Visible = true;
                deftxt.Visible = true;
                Session["lid"] = lid;
                //string script = "loadchart('" + sDate + "', '" + eDate + "','"+sid+"','"+lid+"','"+scid+"','"+evnt+"','"+trend+"','"+ioa+"','"+cls+"','"+med+"','"+lpstatus+"','"+medno+"','"+reptype+"','"+inctype+"');";
                string script = @"setTimeout(function() {loadchart('" + sDate + "', '" + eDate + "','" + sid + "','" + lid + "','" + scid + "','" + evnt + "','" + trend + "','" + ioa + "','" + cls + "','" + med + "','" + lpstatus + "','" + medno + "','" + reptype + "','" + inctype + "');}, 500);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript", script, true);
    }
  
    private void GenerateLessonPlanReport()
    {

        ObjData = new clsData();
        //ObjData.ExecuteSp();
        tdMsg1.InnerHtml = "";
        RV_LPReport.Visible = true;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        hdnType.Value = "SingleLessonPlan";
        Session["hdnType"] = hdnType.Value;
        dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string StartDate = dtst.ToString("yyyy-MM-dd");
        string enddate = dted.ToString("yyyy-MM-dd");
        string AllLesson = "";

        AllLesson = Convert.ToString(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + ObjTempSess.TemplateId));
        Session["AcademicLessons"] = AllLesson;
        if (AllLesson == "")
        {
            RV_LPReport.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
            return;
        }
        string TrendType = "NotNeed";
        if (Convert.ToBoolean(chkreptrend.Checked))
        {
            TrendType = "Quarter";
        }
        string Events = "None,";
        if (chkrepevents.Checked == true)
        {
            Events = "Major,Minor,Arrow";
        }
        else
        {
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
        }
        string SetId = "";
        string ColCalcId = "";

        string IsMaintanance = "";

        foreach (ListItem item in (drpSetname as ListControl).Items)
        {
            if (item.Selected)
            {
                SetId += item.Value + ",";
            }
        }


        if (SetId.TrimEnd(',') == "0")
        {
            foreach (ListItem item in (drpSetname as ListControl).Items)
            {
                if (item.Value != "0")
                {
                    SetId += item.Value + ",";
                }
            }
        }
        if (rbtnmainonly.Checked == false)
        {
            IsMaintanance = "0,1";
        }
        else
        {
            IsMaintanance = "1";
        }
        foreach (ListItem item in (drpColumn as ListControl).Items)
        {
            if (item.Selected)
            {
                ColCalcId += item.Value + ",";
            }
        }

        string LPStatus = ObjData.FetchValue("SELECT LookupName FROM LOOKUP LU INNER JOIN DSTempHdr HD ON LU.LookupId=HD.StatusId WHERE LookupType='TemplateStatus' AND DSTempHdrId=" + ObjTempSess.TemplateId).ToString();

        RV_LPReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
        {
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DupAcademicSet"];
            }
            else
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalDupAcademicSet"];
            }
        }
        else
        {
            if (rbtnIncidentalRegular.SelectedValue == "Regular")
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["AcademicSet"];
            }
            else
            {
                RV_LPReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalAcademicSet"];
            }
        }
        RV_LPReport.ShowParameterPrompts = false;

        ReportParameter[] parm = new ReportParameter[14];
        parm[0] = new ReportParameter("SDate", StartDate.ToString());
        parm[1] = new ReportParameter("EDate", enddate.ToString());
        parm[2] = new ReportParameter("StudentId", sess.StudentId.ToString());
        parm[3] = new ReportParameter("AllLesson", AllLesson.ToString());
        parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
        parm[5] = new ReportParameter("Events", Events);
        parm[6] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkrepioa.Checked).ToString());
        parm[7] = new ReportParameter("Trendtype", TrendType);
        parm[8] = new ReportParameter("IncludeMedication", Convert.ToBoolean(chkrepmedi.Checked).ToString());
        parm[9] = new ReportParameter("ClsType", rbtnClassType.SelectedValue);
        parm[10] = new ReportParameter("SetId", ((SetId == "") ? "0" : SetId.TrimEnd(',')));
        parm[11] = new ReportParameter("DsTempSetColCalcId", ((ColCalcId == "") ? "0" : ColCalcId.TrimEnd(',')));
        parm[12] = new ReportParameter("IsMaintanance", IsMaintanance);
        parm[13] = new ReportParameter("LPStatus", LPStatus);
        this.RV_LPReport.ServerReport.SetParameters(parm);
        RV_LPReport.ServerReport.Refresh();

    }


    protected void Button11_Click(object sender, EventArgs e)
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
            
            if (ListBox2.Items.Count == 1)
                ListBox2.SelectedIndex = 0;
            else
                ListBox2.SelectedIndex = -1;

            if (ListBox1.Items.Count == 1)
                ListBox1.SelectedIndex = 0;
            else
                ListBox1.SelectedIndex = -1;
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
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);
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
        ListBox2.SelectedIndex = -1;

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

            if (ListBox2.Items.Count == 1)
                ListBox2.SelectedIndex = 0;
            else
                ListBox2.SelectedIndex = -1;

            if (ListBox1.Items.Count == 1)
                ListBox1.SelectedIndex = 0;
            else
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
        ListBox1.SelectedIndex = -1;

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "disp();", true);
    }


    protected void btnLessonSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (LessonValidation() == true)
            {
                GenerateLessonPlanReport();
            }
            else
            {
                RV_LPReport.Visible = false;
            }
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
            if (highcheck.Checked == true)
            {
                
                string sourcePdfPath = HttpContext.Current.Server.MapPath("~/StudentBinder/Exported/TempAcademic/");
                string outputFileName = Session["StudName"].ToString()+"AcademicReport.pdf";
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
                outputDocument.Close();
                HttpContext.Current.Response.AddHeader("FileDownloadStatus", "Complete");
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
                btnsubmit_Click(sender, e);
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
           // HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validation() == true)
            {
                if (highcheck.Checked == true)
                {
                    string scripts = "showPopups();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Showpop", scripts, true);
                }
                hdnType.Value = Session["hdnType"].ToString();
                string LPStatus = "";
                tdMsgExport.InnerHtml = "";
                int LessonPlanId = 0;
                ObjData = new clsData();
                int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
                //string ReportXML = "SELECT [Name],CAST(CAST([Content] AS VARBINARY(MAX)) AS XML) AS reportXML FROM [Catalog] WHERE type = 2";
                //DataTable dt = ObjData.ReturnDataTable(ReportXML, false);
                sess = (clsSession)Session["UserSession"];
                string TrendType = "NotNeed";
                string Events = "None,";
                string IOA = "";
                string Medication = "";
                if (Convert.ToBoolean(chktrend.Checked) || Convert.ToBoolean(chkreptrend.Checked))
                {
                    TrendType = "Quarter";
                }
                ReportViewer AcademicReport = new ReportViewer();
                string AllLesson = "";

                if (Convert.ToInt32(Request.QueryString["pageid"].ToString()) == 0)
                {
                    hdnallLesson.Value = "AllLessons";
                }
                else
                {
                    LessonPlanId = Convert.ToInt32(ObjData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + ObjTempSess.TemplateId));
                }
                if (hdnallLesson.Value.Trim() == "AllLessons")
                {
                    string StrSelected = "";
                    for (int i = 0; i < ListBox2.Items.Count; i++)
                    {
                        StrSelected += ListBox2.Items[i].Value + ",";
                    }
                    StrSelected = StrSelected.Substring(0, (StrSelected.Length - 1));
                    //string StrNewQry = "SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),LessonPlanId) FROM (SELECT DISTINCT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId IN (" + StrSelected + ")) DSTEMP FOR XML PATH('')) ,1,1,'')";
                    AllLesson = Convert.ToString(StrSelected);
                }
                else
                {
                    AllLesson = LessonPlanId.ToString();
                }
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                string ClassType = "";
                string GraphType = "";
                if (hdnType.Value == "MultipleLessonPlan")
                {
                    dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //Events = Convert.ToBoolean(chkbxevents.Checked).ToString();
                    if (chkbxevents.Checked == true)
                    {
                        Events = "Major,Minor,Arrow";
                    }
                    else
                    {
                        if (chkbxmajor.Checked == true)
                        {
                            Events += "Major,";
                        }
                        if (chkbxminor.Checked == true)
                        {
                            Events += "Minor,";
                        }
                        if (chkbxarrow.Checked == true)
                        {
                            Events += "Arrow";
                        }
                    }
                    IOA = Convert.ToBoolean(chkbxIOA.Checked).ToString();
                    Medication = Convert.ToBoolean(chkmedication.Checked).ToString();
                    ClassType = rbtnClassTypeall.SelectedValue;
                    GraphType = rbtnIncidentalRegularall.SelectedValue;

                    ///checks the lesson status
                    ///                
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

                    string strLpStat = "SELECT STUFF((SELECT ','+CONVERT(VARCHAR(25), LookupId) FROM (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName IN(" + LPStatus + ")) LookupId FOR XML PATH('')),1,1,'')";
                    LPStatus = ObjData.FetchValue(strLpStat).ToString();
                    ///end
                    ///
                }
                else
                {
                    dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dted = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //Events = Convert.ToBoolean(chkrepevents.Checked).ToString();
                    if (chkrepevents.Checked == true)
                    {
                        Events = "Major,Minor,Arrow";
                    }
                    else
                    {
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
                    }
                    IOA = Convert.ToBoolean(chkrepioa.Checked).ToString();
                    Medication = Convert.ToBoolean(chkrepmedi.Checked).ToString();
                    ClassType = rbtnClassType.SelectedValue;
                    GraphType = rbtnIncidentalRegular.SelectedValue;
                }
                string StartDate = dtst.ToString("yyyy-MM-dd");
                string enddate = dted.ToString("yyyy-MM-dd");
                AcademicReport.ProcessingMode = ProcessingMode.Remote;
                AcademicReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                if (GraphType == "Regular")
                {
                    if (hdnType.Value == "MultipleLessonPlan")
                    {
                        AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ExportAcademic"];
                    }
                    else
                    {
                        AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ExportAcademicSet"];
                    }
                }
                else
                {
                    if (hdnType.Value == "MultipleLessonPlan")
                    {
                        AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalExportAcademic"];
                    }
                    else
                    {
                        AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["IncidentalExportAcademicSet"];
                    }
                }
                if (highcheck.Checked == true)
                {
                    if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
                    {
                        reptype = true;
                        if (rbtnIncidentalRegularall.SelectedValue == "Regular")
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
                        if (rbtnIncidentalRegularall.SelectedValue == "Regular")
                        {
                        }
                        else
                        {
                            inctype = true;
                        }

                    }
                    Session["StudName"] = sess.StudentName;
                    sDate = StartDate.ToString();
                    stname = sess.StudentName;
                    eDate = enddate.ToString();
                    sid = studid;
                    lid = AllLesson.ToString();
                    scid = sess.SchoolId;
                    evnt = Events;
                    trend = TrendType;
                    ioa = Convert.ToBoolean(chkbxIOA.Checked).ToString();
                    cls = rbtnClassTypeall.SelectedValue;
                    med = Convert.ToBoolean(chkmedication.Checked);
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
                    mel.Visible = true;
                    deftxt.Visible = true;
                    //ClientScript.RegisterStartupScript(GetType(), "", "exportChart();", true);
                   // string script = "exportChart();";
                  getAcademicReportExport(sDate, eDate, sid, lid, scid, Events, evnt, ioa, cls);

                    string script = @"setTimeout(function() {exportChart();}, 500);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "newone", script, true);
                    tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Export Successfully Created...");
                    hdnExport.Value = "true";
                    //    ClientScript.RegisterStartupScript(GetType(), "", "DownloadPopup();", true);
                }
                else
                {
                    AcademicReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportUrl"]);
                    if (hdnType.Value == "MultipleLessonPlan")
                    {
                        ReportParameter[] parm = new ReportParameter[11];
                        parm[0] = new ReportParameter("SDate", StartDate.ToString());
                        parm[1] = new ReportParameter("EDate", enddate.ToString());
                        parm[2] = new ReportParameter("StudentId", studid.ToString());
                        parm[3] = new ReportParameter("AllLesson", AllLesson.ToString());
                        parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
                        parm[5] = new ReportParameter("Events", Events);
                        parm[6] = new ReportParameter("IncludeIOA", IOA);
                        parm[7] = new ReportParameter("Trendtype", TrendType);
                        parm[8] = new ReportParameter("IncludeMedication", Medication);                                         //Parameter Value
                        parm[9] = new ReportParameter("ClsType", ClassType);
                        parm[10] = new ReportParameter("LPStatus", LPStatus);
                        AcademicReport.ServerReport.SetParameters(parm);
                    }
                    else
                    {
                        string SetId = "";
                        string ColCalcId = "";
                        string IsMaintanance = "";
                        foreach (ListItem item in (drpSetname as ListControl).Items)
                        {
                            if (item.Selected)
                            {
                                SetId += item.Value + ",";
                            }
                        }
                        if (SetId.TrimEnd(',') == "0")
                        {

                            foreach (ListItem item in (drpSetname as ListControl).Items)
                            {
                                if (item.Value != "0")
                                {
                                    SetId += item.Value + ",";
                                }
                            }
                        }
                        if (rbtnmainonly.Checked == false)
                        {
                            IsMaintanance = "0,1";
                        }
                        else
                        {
                            IsMaintanance = "1";
                        }
                        foreach (ListItem item in (drpColumn as ListControl).Items)
                        {
                            if (item.Selected)
                            {
                                ColCalcId += item.Value + ",";
                            }
                        }
                        LPStatus = ObjData.FetchValue("SELECT LookupName FROM LOOKUP LU INNER JOIN DSTempHdr HD ON LU.LookupId=HD.StatusId WHERE LookupType='TemplateStatus' AND DSTempHdrId=" + ObjTempSess.TemplateId).ToString();

                        ReportParameter[] parm = new ReportParameter[14];
                        parm[0] = new ReportParameter("SDate", StartDate.ToString());
                        parm[1] = new ReportParameter("EDate", enddate.ToString());
                        parm[2] = new ReportParameter("StudentId", sess.StudentId.ToString());
                        parm[3] = new ReportParameter("AllLesson", AllLesson.ToString());
                        parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
                        parm[5] = new ReportParameter("Events", Events);
                        parm[6] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkrepioa.Checked).ToString());
                        parm[7] = new ReportParameter("Trendtype", TrendType);
                        parm[8] = new ReportParameter("IncludeMedication", Convert.ToBoolean(chkrepmedi.Checked).ToString());
                        parm[9] = new ReportParameter("ClsType", rbtnClassType.SelectedValue);
                        parm[10] = new ReportParameter("SetId", ((SetId == "") ? "0" : SetId.TrimEnd(',')));
                        parm[11] = new ReportParameter("DsTempSetColCalcId", ((ColCalcId == "") ? "0" : ColCalcId.TrimEnd(',')));
                        parm[12] = new ReportParameter("IsMaintanance", IsMaintanance);
                        parm[13] = new ReportParameter("LPStatus", LPStatus);
                        AcademicReport.ServerReport.SetParameters(parm);
                    }
                    AcademicReport.ServerReport.Refresh();
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType, encoding, extension, deviceInfo;
                    if (Medication.ToLower() == "true")
                    {
                        deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth></DeviceInfo>";
                    }
                    else
                    {
                        deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth><MarginTop>1.5in</MarginTop></DeviceInfo>";
                    }
                    //deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth><MarginTop>1.5in</MarginTop></DeviceInfo>";

                    byte[] bytes = AcademicReport.ServerReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);


                    string outputPath = "";
                    if (hdnType.Value == "MultipleLessonPlan")
                    {
                        outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + sess.StudentName + "_AcademicReport.pdf");
                    }
                    else
                    {
                        string LessonName = Convert.ToString(ObjData.FetchValue("SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId='" + ObjTempSess.TemplateId + "'"));
                        string LPName = sess.StudentName.Replace(' ', '_') + "_" + LessonName.Replace(' ', '_');
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
                }
                //string PdfPath = "";
                //if (hdnType.Value == "MultipleLessonPlan")
                //{
                //    PdfPath = sess.StudentName + "_AcademicReport.pdf";
                //}
                //else
                //{
                //    string LessonName = Convert.ToString(ObjData.FetchValue("SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId='" + ObjTempSess.TemplateId + "'"));
                //    PdfPath = sess.StudentName + "_" + LessonName + ".pdf";
                //}

                //Response.Write(string.Format("<script> window.open('{0}','_blank');</script>", "PrintReport.aspx?file=" + PdfPath));


                //Response.Buffer = true;
                //Response.Clear();
                //Response.ContentType = mimeType;
                //if (hdnType.Value == "MultipleLessonPlan")
                //{
                //    Response.AddHeader("content-disposition", "attachment; filename=" + sess.StudentName + "_AcademicReport.pdf");
                //}
                //else
                //{
                //    string LessonName = Convert.ToString(ObjData.FetchValue("SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId='" + ObjTempSess.TemplateId + "'"));
                //    Response.AddHeader("content-disposition", "attachment; filename=" + sess.StudentName + "_" + LessonName + ".pdf");
                //}            
                //Response.BinaryWrite(bytes); // create the file
                //Response.End(); // send it to the client to download
                //Response.Clear();
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
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            //string ReportXML = "SELECT [Name],CAST(CAST([Content] AS VARBINARY(MAX)) AS XML) AS reportXML FROM [Catalog] WHERE type = 2";
            //DataTable dt = ObjData.ReturnDataTable(ReportXML, false);
            sess = (clsSession)Session["UserSession"];
            string TrendType = "NotNeed";
            string Events = "None,";
            string IOA = "";
            string Medication = "";
            if (Convert.ToBoolean(chktrend.Checked) || Convert.ToBoolean(chkreptrend.Checked))
            {
                TrendType = "Quarter";
            }
            ReportViewer AcademicReport = new ReportViewer();
            string AllLesson = Convert.ToString(Session["AcademicLessons"]);
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            if (hdnType.Value == "MultipleLessonPlan")
            {
                dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Events = Convert.ToBoolean(chkbxevents.Checked).ToString();
                IOA = Convert.ToBoolean(chkbxIOA.Checked).ToString();
                Medication = Convert.ToBoolean(chkmedication.Checked).ToString();
            }
            else
            {
                dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Events = Convert.ToBoolean(chkrepevents.Checked).ToString();
                IOA = Convert.ToBoolean(chkrepioa.Checked).ToString();
                Medication = Convert.ToBoolean(chkrepmedi.Checked).ToString();
            }
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            AcademicReport.ProcessingMode = ProcessingMode.Remote;
            AcademicReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            AcademicReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DupAcademic"];
            AcademicReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportUrl"]);
            AcademicReport.ServerReport.Refresh();

            ReportParameter[] parm = new ReportParameter[9];
            parm[0] = new ReportParameter("SDate", StartDate.ToString());
            parm[1] = new ReportParameter("EDate", enddate.ToString());
            parm[2] = new ReportParameter("StudentId", sess.StudentId.ToString());
            parm[3] = new ReportParameter("AllLesson", AllLesson.ToString());
            parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
            parm[5] = new ReportParameter("Events", Events);
            parm[6] = new ReportParameter("IncludeIOA", IOA);
            parm[7] = new ReportParameter("Trendtype", TrendType);
            parm[8] = new ReportParameter("IncludeMedication", Medication);                                         //Parameter Value
            AcademicReport.ServerReport.SetParameters(parm);

            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension, deviceInfo;

            deviceInfo = "<DeviceInfo><PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth></DeviceInfo>";

            byte[] bytes = AcademicReport.ServerReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            string outputPath = "";
            if (hdnType.Value == "MultipleLessonPlan")
            {
                outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + sess.StudentName + "_AcademicReport.pdf");
            }
            else
            {
                string LessonName = Convert.ToString(ObjData.FetchValue("SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId='" + ObjTempSess.TemplateId + "'"));
                outputPath = Server.MapPath("~\\StudentBinder\\Reports\\" + sess.StudentName + "_" + LessonName + ".pdf");
            }

            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

            string PdfPath = "";
            if (hdnType.Value == "MultipleLessonPlan")
            {
                PdfPath = sess.StudentName + "_AcademicReport.pdf";
            }
            else
            {
                string LessonName = Convert.ToString(ObjData.FetchValue("SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId='" + ObjTempSess.TemplateId + "'"));
                PdfPath = sess.StudentName + "_" + LessonName + ".pdf";
            }

            Response.Write(string.Format("<script> window.open('{0}','_blank');</script>", "PrintReport.aspx?file=" + PdfPath));

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void chkStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadLessonPlan();
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        int id = ddlLessonplan.SelectedIndex;   
        if (id >= 0)
        {
            if (id > 0)
            {
                ddlLessonplan.SelectedIndex = id - 1;
            }
            string LessonId = ddlLessonplan.SelectedValue.ToString();

            if (highcheck.Checked == true)
            {
                fillGraphhighchart(LessonId);
            }
            else
            {
                string script = "closePopup();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop2", script, true);
            fillGraph(LessonId);
        }
    }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        int id = ddlLessonplan.SelectedIndex;
        int count = ddlLessonplan.Items.Count-1;
        if (id <= count)
        {
            if (id < count)
            {
                ddlLessonplan.SelectedIndex = id + 1;
            }
            string LessonId = ddlLessonplan.SelectedValue.ToString();
            if (highcheck.Checked == true)
            {
                fillGraphhighchart(LessonId);
                //graphPopup.Visible = false;
            }
            else
            {
                string script = "closePopup();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop", script, true);
            fillGraph(LessonId);
        }
    }
    }
    protected void ddlLessonplan_SelectedIndexChanged(object sender, EventArgs e)
    {
        string LessonId = ddlLessonplan.SelectedValue.ToString();
        if (highcheck.Checked == true)
        {
            fillGraphhighchart(LessonId);
        }
        else
        {
            string script = "closePopup();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop3", script, true);
        fillGraph(LessonId);
    }
    }

   
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAcademicReport(string StartDate, string enddate, int studid, string AllLesson, int SchoolId, string Events, string Trendtype, string IncludeIOA, string Clstype)
    {
        objData = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[BiweeklyReport_Trendline]";
        DataTable dt = objData.ReturnAcademicTable(proc, StartDate, enddate, studid, AllLesson, SchoolId, Events, Trendtype, IncludeIOA, Clstype);
        foreach (DataRow row1 in dt.Rows)
        {
            if (row1["AggredatedDate"] != DBNull.Value)
            {
                DateTime dtime = Convert.ToDateTime(row1["AggredatedDate"]);
                row1["AggredatedDate"] = dtime.ToString("MM/dd/yyyy");
            }
        }
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                object value = dr[dc];
                if (value == DBNull.Value)
                {
                    row.Add(dc.ColumnName, null);
                }
                else
                {
                    row.Add(dc.ColumnName, value);
                }
            }
            rows.Add(row);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        string val = json.Serialize(rows);
        return json.Serialize(rows);
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAcademicReportOther(string lplan, int studid,string lpstatus)
    {
        objData = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[BiweeklyAcademicReport_Trendline]";
        DataTable dt = objData.ReturnAcademicTableNext(proc, lplan, studid,lpstatus);
        int i = dt.Rows.Count;
       
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
                string pdfFilePath = currentContext.Server.MapPath("~/StudentBinder/Exported/TempAcademic/stud" + chartId + ".pdf");

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
        if (highcheck.Checked == true) {
            string script = "showPopup();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "show", script, true);
        }
        string sourcePdfPath = HttpContext.Current.Server.MapPath("~/StudentBinder/Exported/TempAcademic/");
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
    public static string getAcademicReportNext(string lplan, int studid)
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
    protected void EvntCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "showEvents();", true);
    }

    
    public void  getAcademicReportExport (string StartDate, string enddate, int studid, string AllLesson, int SchoolId, string Events, string Trendtype, string IncludeIOA, string Clstype)
    {
        objData = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[BiweeklyReport_Trendline]";
        Dtexp = objData.ReturnAcademicTable(proc, StartDate, enddate, studid, AllLesson, SchoolId, Events, Trendtype, IncludeIOA, Clstype);
        foreach (DataRow row1 in Dtexp.Rows)
        {
            if (row1["AggredatedDate"] != DBNull.Value)
            {
                DateTime dtime = Convert.ToDateTime(row1["AggredatedDate"]);
                row1["AggredatedDate"] = dtime.ToString("MM/dd/yyyy");
            }
        }
        
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAcademicReportBysid(string AllLesson)
    {
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;

            DataTable dt = Dtexp.Clone(); 
        string filter = "LessonPlanId = " + AllLesson;

        DataRow[] rowsToCopy = Dtexp.Select(filter);

            foreach (DataRow ro in rowsToCopy)
            {
                dt.ImportRow(ro);
            }


        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                object value = dr[dc];
                if (value == DBNull.Value)
                {
                    row.Add(dc.ColumnName, null);
                }
                else
                {
                    row.Add(dc.ColumnName, value);
                }
            }
            rows.Add(row);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        string val = json.Serialize(rows);
        return json.Serialize(rows);
    }
  

}

