using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class StudentBinder_DSTempHistory : System.Web.UI.Page
{
    clsSession sess = null;
    DataClass oData = new DataClass();
    ClsTemplateSession Objtempsess = null;
    clsData oDta = null;

    protected void Page_Load(object sender, EventArgs e)
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
            FillLessonPlan();
            if (Request.QueryString["LPid"] != null)
            {
                ViewState["LPid"] = Request.QueryString["LPid"].ToString();
                Search(Convert.ToInt32(Request.QueryString["LPid"].ToString()));                
            }

            ddlLessonPlans.SelectedValue = Convert.ToString(ViewState["LPid"]);
            viewTab1();
            //FillScore();
        }
    }

    private void FillLessonPlan()
    {
        DataTable dtLP = new DataTable();
        dtLP.Columns.Add("Id", typeof(string));
        dtLP.Columns.Add("Name", typeof(string));
        DataRow dr0 = dtLP.NewRow();
        dr0["Id"] = -1;
        dr0["Name"] = "----------Select Lesson Plan----------";
        dtLP.Rows.Add(dr0);
        clsLessons oLessons = new clsLessons();
        HttpContext.Current.Session["datasheet"] = "ViewPriorSession";
        DataTable DTLesson = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "LP.LessonPlanId AS Id, DTmp.DSTemplateName AS Name", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
        if (DTLesson != null)
        {
            if (DTLesson.Rows.Count > 0)
            {

                foreach (DataRow drLessn in DTLesson.Rows)
                {
                    DataRow drr = dtLP.NewRow();
                    drr["Id"] = drLessn.ItemArray[0];
                    drr["Name"] = drLessn.ItemArray[1];
                    dtLP.Rows.Add(drr);
                }

            }
        }
        ddlLessonPlans.DataSource = dtLP;
        ddlLessonPlans.DataTextField = "Name";
        ddlLessonPlans.DataValueField = "Id";
        ddlLessonPlans.DataBind();
    }


    protected void gvView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (e.CommandName == "View")
        {
            try
            {
                //e.CommandArgument
                Objtempsess = (ClsTemplateSession)Session["BiweeklySession"];
                Objtempsess.bOpenMode = true;
                Objtempsess.iStdtSessHdr = Convert.ToInt32(e.CommandArgument);
                string isMaintanance = "";
                int CurrentSetId = 0;
                if (Request.QueryString["LPid"] == null)
                {
                    oDta = new clsData();
                    DataTable tempid = oDta.ReturnDataTable("SELECT DSTempHdrId,CurrentSetId,IsMaintanace FROM StdtSessionHdr WHERE StdtSessionHdrId=" + e.CommandArgument, false);
                    if (tempid != null)
                    {
                        if (tempid.Rows.Count > 0)
                        {
                            Objtempsess.TemplateId = Convert.ToInt32(tempid.Rows[0]["DSTempHdrId"].ToString());

                            if (tempid.Rows[0]["IsMaintanace"].ToString() == "True")
                            {
                                isMaintanance = "true";
                            }
                            else
                            {
                                isMaintanance = "false";
                            }
                            CurrentSetId = Convert.ToInt32(tempid.Rows[0]["CurrentSetId"]);
                        }
                    }
                }
                else 
                {
                    oDta = new clsData();
                    DataTable tempid = oDta.ReturnDataTable("SELECT DSTempHdrId,CurrentSetId,IsMaintanace FROM StdtSessionHdr WHERE StdtSessionHdrId=" + e.CommandArgument, false);
                    if (tempid != null)
                    {
                        if (tempid.Rows.Count > 0)
                        {
                            //Objtempsess.TemplateId = Convert.ToInt32(tempid.Rows[0]["DSTempHdrId"].ToString());

                            if (tempid.Rows[0]["IsMaintanace"].ToString() == "True")
                            {
                                isMaintanance = "true";
                            }
                            else
                            {
                                isMaintanance = "false";
                            }
                            CurrentSetId = Convert.ToInt32(tempid.Rows[0]["CurrentSetId"]);
                        }
                    }
                }
                Response.Redirect("Datasheet.aspx?SessHdrID=" + e.CommandArgument + "&isMaint=" + isMaintanance + "&currSetId=" + CurrentSetId);
            }
            catch (Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!!!");
                throw Ex;
            }
        }
    }

    protected void Search(int LPid)
    {
        Objtempsess = (ClsTemplateSession)Session["BiweeklySession"];
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            if (inputstart.Text == "" && inputend.Text == "")
            {
                try
                {
                    oData = new DataClass();

                    this.gvView.DataSource = oData.fillData("SELECT  StdtSessionHdr.IsMaintanace as IsMaintanance,DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId ,StdtSessionHdr.StdtSessionHdrId, "
        + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27),"
        + " StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
        + " DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
        + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
        + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId where StdtSessionHdr.LessonPlanId=" + LPid + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");
                    this.gvView.DataBind();
                }
                catch (Exception Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!!");
                    throw Ex;
                }
            }



            else if (inputstart.Text != "" && inputend.Text != "")
            {
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                dtst = DateTime.ParseExact(inputstart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(inputend.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string Stdate = dtst.ToString("yyyy-MM-dd");
                string EndDate = dted.ToString("yyyy-MM-dd");

                this.gvView.DataSource = oData.fillData("SELECT StdtSessionHdr.IsMaintanace as IsMaintanance, DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId,StdtSessionHdr.StdtSessionHdrId, "
     + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27), StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
     + "DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
    + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
    + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId WHERE CONVERT(DATE, StartTs) >= '" + Stdate + "' and CONVERT(DATE, StartTs)<= '" + EndDate + " 11:59PM' "
    + " AND StdtSessionHdr.LessonPlanId=" + LPid + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");
                this.gvView.DataBind();
            }
            else if (inputstart.Text != "" && inputend.Text == "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(inputstart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string Stdate = dtst.ToString("yyyy-MM-dd");
                this.gvView.DataSource = oData.fillData("SELECT  StdtSessionHdr.IsMaintanace as IsMaintanance,DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId,StdtSessionHdr.StdtSessionHdrId, "
     + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27), StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
     + "DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
    + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
    + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId WHERE CONVERT(DATE, StartTs) >= '" + Stdate + "'"

    + " AND StdtSessionHdr.LessonPlanId=" + LPid + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");

                this.gvView.DataBind();

            }
            else
            {
                DateTime dted = new DateTime();
                dted = DateTime.ParseExact(inputend.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string EndDate = dted.ToString("yyyy-MM-dd");
                this.gvView.DataSource = oData.fillData("SELECT  StdtSessionHdr.IsMaintanace as IsMaintanance,DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId,StdtSessionHdr.StdtSessionHdrId, "
     + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27), StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
     + "DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
    + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
    + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId WHERE CONVERT(DATE, StartTs) <= '" + EndDate + " 11:59PM'"
    + " AND StdtSessionHdr.LessonPlanId=" + LPid + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");
                this.gvView.DataBind();
            }
        }
    }

    protected void FillAllLPs(string LPcond)
    {
        Objtempsess = (ClsTemplateSession)Session["BiweeklySession"];
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            if (inputstart.Text == "" && inputend.Text == "")
            {
                try
                {
                    oData = new DataClass();

                    this.gvView.DataSource = oData.fillData("SELECT StdtSessionHdr.IsMaintanace as IsMaintanance, DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId ,StdtSessionHdr.StdtSessionHdrId, "
        + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27),"
        + " StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
        + " DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
        + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
        + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId where " + LPcond + " StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");
                    this.gvView.DataBind();
                }
                catch (Exception Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!!");
                    throw Ex;
                }
            }



            else if (inputstart.Text != "" && inputend.Text != "")
            {
                DateTime dtst = new DateTime();
                DateTime dted = new DateTime();
                dtst = DateTime.ParseExact(inputstart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dted = DateTime.ParseExact(inputend.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string Stdate = dtst.ToString("yyyy-MM-dd");
                string EndDate = dted.ToString("yyyy-MM-dd");

                this.gvView.DataSource = oData.fillData("SELECT  StdtSessionHdr.IsMaintanace as IsMaintanance,DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId,StdtSessionHdr.StdtSessionHdrId, "
     + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27), StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
     + "DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
    + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
    + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId WHERE " + LPcond + " CONVERT(DATE, StartTs) >= '" + Stdate + "' and CONVERT(DATE, StartTs)<= '" + EndDate + "' "
    + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");
                this.gvView.DataBind();
            }
            else if (inputstart.Text != "" && inputend.Text == "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(inputstart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string Stdate = dtst.ToString("yyyy-MM-dd");
                this.gvView.DataSource = oData.fillData("SELECT  StdtSessionHdr.IsMaintanace as IsMaintanance,DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId,StdtSessionHdr.StdtSessionHdrId, "
     + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27), StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
     + "DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
    + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
    + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId WHERE " + LPcond + " CONVERT(DATE, StartTs) >= '" + Stdate + "'"

    + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");

                this.gvView.DataBind();

            }
            else
            {
                DateTime dted = new DateTime();
                dted = DateTime.ParseExact(inputend.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                string EndDate = dted.ToString("yyyy-MM-dd");
                this.gvView.DataSource = oData.fillData("SELECT  StdtSessionHdr.IsMaintanace as IsMaintanance,DSTempHdr.DSTempHdrId, DSTempHdr.SchoolId, DSTempHdr.StudentId,StdtSessionHdr.StdtSessionHdrId, "
     + "StdtSessionHdr.CurrentPromptId, StdtSessionHdr.CurrentSetId, StdtSessionHdr.DSTempHdrId, StdtSessionHdr.SessionNbr,StdtSessionHdr.SessionStatusCd,DSTempHdr.DSTemplateName AS LessonPlanName,CONVERT(varchar(27), StdtSessionHdr.StartTs, 100) as StartTs, CONVERT(varchar(27), StdtSessionHdr.EndTs, 100) as EndTs, "
     + "DSTempSet.SetCd as SetName, [LookUp].LookupName as CurrPrompt,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM DSTempHdr LEFT JOIN LessonPlan LP ON LP.LessonPlanId=DSTempHdr.LessonPlanId LEFT JOIN StdtSessionHdr ON DSTempHdr.DSTempHdrId= StdtSessionHdr.DSTempHdrId LEFT JOIN "
    + "Student ON DSTempHdr.StudentId= Student.StudentId LEFT JOIN DSTempSet ON StdtSessionHdr.CurrentSetId= DSTempSet.DSTempSetId "
    + "LEFT JOIN [LookUp] ON [LookUp].LookupId=StdtSessionHdr.CurrentPromptId WHERE " + LPcond + " CONVERT(DATE, StartTs) <= '" + EndDate + "'"
    + " AND StdtSessionHdr.StudentId=" + sess.StudentId + " AND "
        + "StdtSessionHdr.SchoolId=" + sess.SchoolId + "  AND StdtSessionHdr.SessionStatusCd<>'P' ORDER BY StdtSessionHdr.SessionNbr DESC");
                this.gvView.DataBind();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if(ddlLessonPlans.SelectedIndex!=0)
        {
        ViewState["LPid"] = ddlLessonPlans.SelectedItem.Value;
        if (Request.QueryString["LPid"] != null)
        {
            Search(Convert.ToInt32(ViewState["LPid"].ToString()));
            FillScore();
        }
        else
        {
            LessonPlanSearch();
        }
        }
    }

    protected void gvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["LPid"] != null)
        {
            gvView.PageIndex = e.NewPageIndex;
            Search(Convert.ToInt32(ViewState["LPid"].ToString()));
        }
        else
        {
            gvView.PageIndex = e.NewPageIndex;
            LessonPlanSearch();
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        inputstart.Text = "";
    }
    protected void btnClearEnd_Click(object sender, EventArgs e)
    {
        inputend.Text = "";
    }
    protected void Tab1_Click(object sender, EventArgs e)
    {
        viewTab1();
    }
    protected void Tab2_Click(object sender, EventArgs e)
    {
        viewtab2();
    }
    private void viewTab1()
    {
        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
        if(ddlLessonPlans.SelectedIndex>0)
        ViewState["LPid"] = ddlLessonPlans.SelectedItem.Value;
        Search(Convert.ToInt32(ViewState["LPid"]));

    }
    private void viewtab2()
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Clicked";
        MainView.ActiveViewIndex = 1;
        if (ddlLessonPlans.SelectedIndex > 0)
        {
            ViewState["LPid"] = ddlLessonPlans.SelectedItem.Value;
            FillScore();
        }
    }
    public void FillScore()
    {
        sess = (clsSession)Session["UserSession"];
        clsData ObjData = new clsData();
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();        
        string SDATE = "";
        string EDATE = "";
        string WHEREQuery = "";
        if (inputstart.Text != "" && inputend.Text != "")
        {
            dtst = DateTime.ParseExact(inputstart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(inputend.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            SDATE = dtst.ToString("yyyy-MM-dd");
            EDATE = dted.ToString("yyyy-MM-dd");
            WHEREQuery = " AND CONVERT(DATE, StartTs) >= '" + SDATE + "' AND CONVERT(DATE, StartTs)<= '" + EDATE + "'";
            //WHEREQuery = " AND StartTs BETWEEN CONVERT(DATE,'"+SDATE+"') AND CONVERT(DATE,'"+EDATE+"')";
        }
        else if (inputstart.Text != "" && inputend.Text == "")
        {
            dtst = DateTime.ParseExact(inputstart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            SDATE = dtst.ToString("yyyy-MM-dd");
            WHEREQuery = " AND CONVERT(DATE, StartTs) >= '" + SDATE + "'";
            //WHEREQuery = " AND CONVERT(DATE,StartTs) >= CONVERT(DATE,'" + SDATE + "') ";
        }
        else if (inputstart.Text == "" && inputend.Text != "")
        {
            dted = DateTime.ParseExact(inputend.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            EDATE = dted.ToString("yyyy-MM-dd");
            WHEREQuery = " AND CONVERT(DATE, StartTs) <= '" + EDATE + "'";
            //WHEREQuery = " AND CONVERT(DATE,StartTs) <= CONVERT(DATE,'" + EDATE + "') ";
        }

        string StrQuery = "SELECT StdtSessionHdrId,SessionNbr,SessionStatusCd,CONVERT(varchar(27), StartTs, 100) as StartTs ,CONVERT(varchar(27), EndTs, 100) as EndTs ,IOAInd AS IOA,CASE WHEN StdtSessionHdr.ModifiedBy IS NULL THEN (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.CreatedBy) ELSE (SELECT CONCAT(UserFName,' ',UserLName) FROM [User] WHERE UserId=StdtSessionHdr.ModifiedBy) END as [User] FROM StdtSessionHdr WHERE " +
            " LessonPlanId=" + ViewState["LPid"] + "  AND StdtSessionHdr.SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + WHEREQuery + " AND StdtSessionHdr.SessionStatusCd<>'P'  ORDER BY SessionNbr DESC";
        DataTable dt = ObjData.ReturnDataTable(StrQuery, false);
        if (dt != null)
        {
            GrdScore.DataSource = dt;
            GrdScore.DataBind();
        }
    }
    protected void GrdScore_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsData ObjData = new clsData();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnid = (HiddenField)e.Row.FindControl("hdnsesshdr");
            GridView gridcalc = (GridView)e.Row.FindControl("grdCalcscore");
            string Query = "SELECT CALC.CalcType,CASE WHEN CSR.Score=-1 THEN 'N/A' ELSE CASE WHEN HDR.SessionStatusCd='D' THEN 'N/A' ELSE CONVERT(VARCHAR,CSR.Score) END END Score FROM StdtSessColScore CSR INNER JOIN DSTempSetColCalc CALC ON CSR.DSTempSetColCalcId=CALC.DSTempSetColCalcId INNER JOIN StdtSessionHdr HDR ON CSR.StdtSessionHdrId=HDR.StdtSessionHdrId WHERE HDR.StdtSessionHdrId=" + hdnid.Value + "";
            DataTable DT = ObjData.ReturnDataTable(Query, false);
            if (DT != null)
            {
                gridcalc.DataSource = DT;
                gridcalc.DataBind();
            }

        }
    }
    protected void GrdScore_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdScore.PageIndex = e.NewPageIndex;
        FillScore();
    }
    protected void LessonPlanSearch()
    {
        if (ddlLessonPlans.SelectedIndex > 0)
        {
            ViewState["LPid"] = ddlLessonPlans.SelectedValue;
            FillAllLPs("StdtSessionHdr.LessonPlanId=" + ddlLessonPlans.SelectedValue + " AND ");
            FillScore();
        }
        else
        {
            FillAllLPs("");
        }
    }
   
}