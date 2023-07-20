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
public partial class StudentBinder_BiweeklySessionReport : System.Web.UI.Page
{
    clsSession sess = null;
    ClsTemplateSession ObjTempSess;
    clsData ObjData = null;
    ArrayList arraylist1 = new ArrayList();
    ArrayList arraylist2 = new ArrayList();
    DataTable Dt = null;
    DataTable Dts = null;
    clsLessons oLessons;
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
                        txtEdate.Text = DateTime.Now.Date.AddDays(1).ToString("MM/dd/yyyy").Replace("-", "/");
                        txtSdate.Text = DateTime.Now.Date.AddDays(-91).ToString("MM/dd/yyyy").Replace("-", "/");
                        hdnallLesson.Value = "AllLessons";
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "loadWait();", true);
                        loadLessonPlan();
                       // GenerateReport();
                    }
                    else
                    {
                        ObjTempSess.TemplateId = pageid;
                        RV_LPReport.Visible = false;
                    }

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
            hfPopUpValue.Value = "true";
            saveLessonPlans();
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
            int LessonPlanId = 0;
            RV_LPReport.Visible = true;
            sess = (clsSession)Session["UserSession"];
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
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
                string StrQuery = "SELECT STUFF((SELECT ','+CONVERT(VARCHAR, LessonPlanId)  FROM (SELECT DISTINCT LessonPlanId FROM [dbo].[StdtSessionHdr] WHERE"+
                                  " StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND CONVERT(DATE,StartTs) BETWEEN CONVERT(DATE,'" + StartDate + "') AND CONVERT(DATE,'" + enddate + "')) LP FOR XML PATH('')),1,1,'')";
                    AllLesson = Convert.ToString(ObjData.FetchValue(StrQuery));
            }
            else
            {
                AllLesson = LessonPlanId.ToString();
            }

            if (AllLesson == "")
            {
                RV_LPReport.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('No Data Available For Graph Plotting');", true);
                return;
            }



            RV_LPReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            RV_LPReport.ServerReport.ReportPath = "/Biweekly Reports/BiweeklySession";
            RV_LPReport.ShowParameterPrompts = false;

            ReportParameter[] parm = new ReportParameter[5];
            parm[0] = new ReportParameter("StartDate", StartDate.ToString());
            parm[1] = new ReportParameter("EndDate", enddate.ToString());
            parm[2] = new ReportParameter("StudentId", sess.StudentId.ToString());
            parm[3] = new ReportParameter("LessonPlan", AllLesson.ToString());
            parm[4] = new ReportParameter("SchoolId", sess.SchoolId.ToString());
            //parm[5] = new ReportParameter("Events", Convert.ToBoolean(chkbxevents.Checked).ToString());
            //parm[6] = new ReportParameter("IncludeIOA", Convert.ToBoolean(chkbxIOA.Checked).ToString());
            //parm[7] = new ReportParameter("IncludeTrend", "Quarter");
            

            this.RV_LPReport.ServerReport.SetParameters(parm);

            RV_LPReport.ServerReport.Refresh();
        }
        else
            RV_LPReport.Visible = false;
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    ObjData = new clsData();
    //    ObjData.ExecuteSp();
    //}



    private void loadLessonPlan()
    {
        ObjData = new clsData();
        sess = (clsSession)Session["UserSession"];
        oLessons = new clsLessons();
        Dts = new DataTable();
        Dt = new DataTable();
        string strSql = "";
        strSql = "SELECT LPG.StdtLessonPlanId,LP.LessonPlanName FROM LessonPlanGraphRelation LPG INNER JOIN StdtLessonPlan SLP ON SLP.StdtLessonPlanId =LPG.StdtLessonPlanId"
                + " INNER JOIN LessonPlan LP on SLP.LessonPlanId=LP.LessonPlanId WHERE LPG.StudentId=" + sess.StudentId + ""
                + " AND LPG.SchoolId=" + sess.SchoolId + " AND LPG.ClassId=" + sess.Classid + " AND PageType='BiweeklySessionGraph'";
        Dts = ObjData.ReturnDataTable(strSql, true);
        if (Dts != null)
        {
            ListBox2.DataSource = Dts;
            ListBox2.DataTextField = "LessonPlanName";
            ListBox2.DataValueField = "StdtLessonPlanId";
            ListBox2.DataBind();
        }
        Dt = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.StdtLessonPlanId as LessonPlanId,LP.LessonPlanName as LessonPlan", "LU.LookupName='Approved' AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE')");
        if (Dt != null)
        {
            ListBox1.DataSource = Dt;
            ListBox1.DataTextField = "lessonplan";
            ListBox1.DataValueField = "lessonplanid";
            ListBox1.DataBind();
            for (int j = 0; j < Dt.Rows.Count; j++)
            {
                arraylist2.Add(Dt.Rows[j]["LessonPlanId"].ToString());
            }
            foreach (ListItem item in ListBox2.Items)
            {
                if (arraylist2.Contains(item.Value))
                {
                    ListBox1.Items.Remove(item);
                }
            }
        }


    }

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
                strSql = "INSERT INTO lessonPlanGraphRelation (StdtLessonPlanId,SchoolId,StudentId,PageType,ClassId,CreatedBy,CreatedOn)"
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
    }
   
}