using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using MathNet.Numerics;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Administration_AAa : System.Web.UI.Page
{
    clsSession sess = null;
    clsData objData = null;
    DataClass oData = null;
    static clsData Dataobj = null;
    public static ClsTemplateSession ObjTempSess = null;

    Dictionary<int, int> lessonPlanMap = new Dictionary<int, int>();
    Dictionary<int, int> stdtLessonPlanMap = new Dictionary<int, int>();
    Dictionary<int, int> hdrMap = new Dictionary<int, int>();
    Dictionary<int, int> setMap = new Dictionary<int, int>();
    Dictionary<int, int> parentStepMap = new Dictionary<int, int>();
    Dictionary<int, int> stepMap = new Dictionary<int, int>();
    Dictionary<int, int> setColMap = new Dictionary<int, int>();
    Dictionary<int, int> setColCalcMap = new Dictionary<int, int>();
    Dictionary<int, int> docMap = new Dictionary<int, int>();


    public class LessonExport
    {
        public DataTable Header { get; set; }
        public DataTable Prompts { get; set; }
        public DataTable Sets { get; set; }
        public DataTable Steps { get; set; }
        public DataTable ParentSteps { get; set; }
        public DataTable SetCols { get; set; }
        public DataTable SetColCalcs { get; set; }
        public DataTable Rules { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = new ClsTemplateSession();
        HttpContext.Current.Session["BiweeklySession"] = ObjTempSess;

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
        string eventTarget = Request["__EVENTTARGET"];

        if (eventTarget == "ContinueProcess")
        {
            hdnPopupValue.Value = "duplicatevalidation";
            btnUploadJson_Click(sender, e);
        }

        if (!IsPostBack)
        {
            btnexp.Visible = (RbtnLessonView.SelectedValue == "ClientView");
            btnimpMEDS.Visible = (RbtnLessonView.SelectedValue == "ClientView");
            tdMsg.InnerHtml = "";
            txtLessonName.Text = "";
            fillGoal();
            fillLessonName();
            fillYear();

            btnShowOrHide_Click(sender, e);
            if (RbtnLessonView.SelectedValue == "DatabankView")
            {
                ddlClientName.Visible = false;
                ddlIepYear.Visible = false;
                iepPtag.Visible = false;
                ddlLessonStatus.Visible = false;
                grdClientView.Visible = false;
                btnAdd.Visible = true;
                ddlTeachingMethod.Visible = true;
                grdDatabankView.Visible = true;
                fillTeachingMethod();
                BindDatabankView();
            }
            else
            {
                btnAdd.Visible = false;
                ddlIepYear.Visible = true;
                iepPtag.Visible = true;
                ddlTeachingMethod.Visible = false;
                grdDatabankView.Visible = false;
                ddlLessonStatus.Visible = true;
                ddlClientName.Visible = true;
                grdClientView.Visible = true;
                fillClientName();
                BindClientView();
            }
        }
    }

    protected void btnShowOrHide_Click(object sender, EventArgs e)
    {
        if (btnShowOrHide.Text == "Show")
        {
            GrdOverview.Visible = true;
            BindLessonCount();
            btnShowOrHide.Text = "Hide";
        }
        else
        {
            GrdOverview.Visible = false;
            btnShowOrHide.Text = "Show";
        }
    }

    private void BindLessonCount()
    {
        objData = new clsData();
        DataTable DtLCount;
        string strLessonCount = "SELECT T1.GoalName AS GoalName, ClientLessonCount, DatabankLessonCount FROM(SELECT COUNT(LS.LessonPlanId) ClientLessonCount, GoalName FROM (SELECT DISTINCT G.GoalId, G.GoalName, " +
            "DSINFO.LessonPlanId, DS.StudentId FROM (SELECT DISTINCT LessonPlanId,StudentId FROM DSTempHdr DS LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE DS.StatusId IN " +
            "(SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
            "AND DS.TeachingProcId IN (SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A'))DSINFO " +
            "INNER JOIN DSTempHdr DS ON DS.LessonPlanId=DSINFO.LessonPlanId AND DS.StudentId= DSINFO.StudentId INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId " +
            "INNER JOIN Goal G ON GLP.GoalId=G.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE GLP.ActiveInd='A' AND G.ActiveInd='A' AND LU.LookupType='Datasheet-Teaching Procedures' " +
            "AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' ) LS GROUP BY GoalName,GoalId ) AS T1 LEFT JOIN ( SELECT COUNT(LS.LessonPlanId) DatabankLessonCount, LS.GoalName FROM " +
            "(SELECT DISTINCT G.GoalId, G.GoalName, DSINFO.LessonPlanId, DS.StudentId FROM (SELECT DISTINCT LessonPlanId,StudentId FROM DSTempHdr DS LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
            "WHERE DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
            "AND DS.TeachingProcId IN (SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A'))DSINFO " +
            "INNER JOIN DSTempHdr DS ON DS.LessonPlanId=DSINFO.LessonPlanId AND DS.StudentId= DSINFO.StudentId INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId " +
            "INNER JOIN Goal G ON GLP.GoalId=G.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE GLP.ActiveInd='A' AND G.ActiveInd='A' AND LU.LookupType='Datasheet-Teaching Procedures' " +
            "AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' AND DS.StudentId IS NULL UNION SELECT G.GoalId, G.GoalName, DS.LessonPlanId, DS.StudentId FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId " +
            "INNER JOIN Goal G ON G.GoalId=GLP.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE DS.isDynamic=0 AND DS.StudentId IS NULL AND GLP.ActiveInd='A' AND G.ActiveInd='A' " +
            "AND DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
            "AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A'  ) LS GROUP BY GoalName,GoalId ) AS T2 ON T1.GoalName=T2.GoalName ";
        DtLCount = objData.ReturnDataTable(strLessonCount, false);
        if (DtLCount != null)
        {
            DataTable dt = new DataTable();
            DataRow drow;
            DataColumn dc1 = new DataColumn("GoalName", typeof(string));
            DataColumn dc2 = new DataColumn("ClientLessonCount", typeof(Int32));
            DataColumn dc3 = new DataColumn("DatabankLessonCount", typeof(Int32));
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            if (DtLCount.Rows.Count > 0)
            {
                int clCount = 0, dblCount = 0;
                foreach (DataRow dr in DtLCount.Rows)
                {
                    object CLcnt = dr["ClientLessonCount"];
                    object DBcnt = dr["DatabankLessonCount"];
                    if (CLcnt != DBNull.Value)
                    {
                        clCount = clCount + Convert.ToInt32(dr["ClientLessonCount"]);
                    }
                    else
                    {
                        clCount = clCount + 0;
                    }
                    if (DBcnt != DBNull.Value)
                    {
                        dblCount = dblCount + Convert.ToInt32(dr["DatabankLessonCount"]);
                    }
                    else
                    {
                        dblCount = dblCount + 0;
                    }
                    drow = dt.NewRow();
                    drow["GoalName"] = dr.ItemArray[0];
                    drow["ClientLessonCount"] = dr.ItemArray[1];
                    drow["DatabankLessonCount"] = dr.ItemArray[2];
                    dt.Rows.Add(drow);
                }
                drow = dt.NewRow();
                drow["GoalName"] = "Grand Total";
                drow["ClientLessonCount"] = clCount;
                drow["DatabankLessonCount"] = dblCount;
                dt.Rows.Add(drow);

                GrdOverview.DataSource = dt;
                GrdOverview.DataBind();
            }
        }

        //Organizational Statistics

        DataTable dt2 = new DataTable();
        DataRow drowOrg;
        DataColumn dco1 = new DataColumn("Category", typeof(string));
        DataColumn dco2 = new DataColumn("Count", typeof(Int32));
        dt2.Columns.Add(dco1);
        dt2.Columns.Add(dco2);

        drowOrg = dt2.NewRow();
        drowOrg["Category"] = "Total Active Clients";
        string sqlCount = "SELECT COUNT(DISTINCT StudentPersonalId) FROM Placement WHERE EndDate IS NULL AND Status=1";
        int totalActClients = Convert.ToInt32(objData.FetchValue(sqlCount));
        drowOrg["Count"] = totalActClients;
        dt2.Rows.Add(drowOrg);

        drowOrg = dt2.NewRow();
        drowOrg["Category"] = "Total Active Clients with Active / Maintenance Lessons";
        sqlCount = "SELECT COUNT(DISTINCT StudentPersonalId) FROM Placement INNER JOIN DSTempHdr ON Placement.StudentPersonalId = DSTempHdr.StudentId WHERE Placement.EndDate IS NULL AND Placement.Status = 1  AND (DSTempHdr.StatusId = (SELECT LookupId FROM LookUp WHERE lookupType='TemplateStatus' AND LookupName='Approved') OR DSTempHdr.StatusId = (SELECT LookupId FROM LookUp WHERE lookupType='TemplateStatus' AND LookupName='Maintenance'))";
        int clientsActMnt = Convert.ToInt32(objData.FetchValue(sqlCount));
        drowOrg["Count"] = clientsActMnt;
        dt2.Rows.Add(drowOrg);

        drowOrg = dt2.NewRow();
        drowOrg["Category"] = "Total Active / Maintenance Lessons for all Clients";
        sqlCount = "SELECT COUNT(DISTINCT LessonPlanId) FROM DSTempHdr INNER JOIN Placement ON DSTempHdr.StudentId = Placement.StudentPersonalId WHERE (DSTempHdr.StatusId = (SELECT LookupId FROM LookUp WHERE lookupType='TemplateStatus' AND LookupName='Approved') OR DSTempHdr.StatusId = (SELECT LookupId FROM LookUp WHERE lookupType='TemplateStatus' AND LookupName='Maintenance')) AND Placement.EndDate IS NULL AND Placement.Status = 1";
        int lessonActMnt = Convert.ToInt32(objData.FetchValue(sqlCount));
        drowOrg["Count"] = lessonActMnt;
        dt2.Rows.Add(drowOrg);

        drowOrg = dt2.NewRow();
        drowOrg["Category"] = "Total number of mastered (Maintenance) Lessons for all Clients";
        sqlCount = "SELECT COUNT(DISTINCT StdtDSStat.LessonPlanId) FROM StdtDSStat INNER JOIN Placement ON Placement.StudentPersonalId = StdtDSStat.StudentId INNER JOIN DSTempHdr ON DSTempHdr.StudentId = StdtDSStat.StudentId WHERE StdtDSStat.statusMessage = 'COMPLETED' AND DSTempHdr.StatusId = (SELECT LookupId FROM LookUp WHERE lookupType='TemplateStatus' AND LookupName='Maintenance') AND Placement.EndDate IS NULL AND Placement.Status = 1";
        int lessonMstd = Convert.ToInt32(objData.FetchValue(sqlCount));
        drowOrg["Count"] = lessonMstd;
        dt2.Rows.Add(drowOrg);

        drowOrg = dt2.NewRow();
        drowOrg["Category"] = "Average number of all lessons (Active and Maintenance) per Client";
        drowOrg["Count"] = lessonActMnt / totalActClients;
        dt2.Rows.Add(drowOrg);

        drowOrg = dt2.NewRow();
        drowOrg["Category"] = "Average number of Active lessons per Student";
        sqlCount = "SELECT COUNT(DISTINCT LessonPlanId) FROM DSTempHdr INNER JOIN Placement ON DSTempHdr.StudentId = Placement.StudentPersonalId WHERE DSTempHdr.StatusId = (SELECT LookupId FROM LookUp WHERE lookupType='TemplateStatus' AND LookupName='Approved') AND Placement.EndDate IS NULL AND Placement.Status = 1";
        int totalActiveLessons = Convert.ToInt32(objData.FetchValue(sqlCount));
        drowOrg["Count"] = totalActiveLessons / totalActClients;
        dt2.Rows.Add(drowOrg);

        GrdOrgStats.DataSource = dt2;
        GrdOrgStats.DataBind();
    }

    protected void RbtnLessonView_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGoal();
        fillYear();
        fillLessonName();
        txtLessonName.Text = "";

        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            btnexp.Visible = false;
            btnimpMEDS.Visible = false;
            ddlIepYear.Visible = false;
            iepPtag.Visible = false;
            ddlClientName.Visible = false;
            ddlLessonStatus.Visible = false;
            grdClientView.Visible = false;
            btnAdd.Visible = true;
            ddlTeachingMethod.Visible = true;
            grdDatabankView.Visible = true;
            fillTeachingMethod();
            BindDatabankView();
        }
        else
        {
            btnexp.Visible = true;
            btnimpMEDS.Visible = true;
            ddlIepYear.Visible = true;
            iepPtag.Visible = true;
            btnAdd.Visible = false;
            ddlTeachingMethod.Visible = false;
            grdDatabankView.Visible = false;
            ddlLessonStatus.Visible = true;
            ddlClientName.Visible = true;
            grdClientView.Visible = true;
            fillClientName();
            BindClientView();
        }
    }

    private void fillGoal()
    {
        objData = new clsData();
        DataTable DTLesson;
        string strGoal = "SELECT GoalId, GoalName FROM Goal WHERE ActiveInd='A'";
        DTLesson = objData.ReturnDataTable(strGoal, false);
        if (DTLesson != null)
        {
            ddlGoal.DataSource = DTLesson;
            ddlGoal.DataTextField = "GoalName";
            ddlGoal.DataValueField = "GoalId";
            ddlGoal.DataBind();
            ddlGoal.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Goal", "0"));
        }
    }

    private void fillYear()
    {
        ddlIepYear.Items.Clear();
        int year = DateTime.Now.Year;
        for (int i = 2010; i <= year + 5; i++)
        {
            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
            ddlIepYear.Items.Add(li);
        }
        //ddlIepYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
        //ddlIepYear.Items.FindByText("All").Selected = true;
        //ddlIepYear.Items.FindByText(year.ToString()).Selected = true;
    }

    protected void ddlGoal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.PageIndex = 0;
            BindDatabankView();
        }
        else
        {
            grdClientView.PageIndex = 0;
            BindClientView();
        }
        fillLessonName();
    }

    private void fillLessonName()
    {
        objData = new clsData();
        DataTable DTLessonName;
        string strLessonName = "";
        string strCondition = "";

        int goalId = Convert.ToInt32(ddlGoal.SelectedValue);
        if (goalId > 0)
        {
            strCondition += " AND G.GoalId = " + goalId + " ";
        }
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            //// This also related with lesson count more than 5000+
            //strLessonName = "SELECT DISTINCT G.GoalId,DSINFO.LessonPlanId, DS.DSTemplateName AS LessonName FROM (SELECT *,CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) THEN (SELECT TOP 1 DSTempHdrId  FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='In Progress' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='In Progress' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Expired' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
            //    "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Expired' " +
            //    "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) END END END END END END AS DSTempHdrId " +
            //    "FROM(SELECT DISTINCT LessonPlanId,StudentId FROM DSTempHdr DS LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE DS.StatusId IN (SELECT LookupId FROM LookUp Look " +
            //    "WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) AND DS.TeachingProcId IN " +
            //    "(SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A')) LSN)DSINFO INNER JOIN DSTempHdr DS " +
            //    "ON DS.DSTempHdrId=DSINFO.DSTempHdrId INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G ON GLP.GoalId=G.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
            //    "WHERE GLP.ActiveInd='A' AND G.ActiveInd='A' AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' " + strCondition +
            //    "UNION SELECT G.GoalId,DS.LessonPlanId, DS.DSTemplateName AS LessonName FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G " +
            //    "ON G.GoalId=GLP.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE DS.isDynamic=0 AND DS.StudentId IS NULL AND GLP.ActiveInd='A' AND G.ActiveInd='A' " +
            //    "AND DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
            //    "AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' " + strCondition + " ORDER BY GoalId,LessonPlanId";


            strLessonName = "SELECT DISTINCT G.GoalId,DSINFO.LessonPlanId, DS.DSTemplateName AS LessonName FROM (SELECT *,CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) THEN (SELECT TOP 1 DSTempHdrId  FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='In Progress' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='In Progress' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Maintenance' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Inactive' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Expired' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS " +
                "LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Expired' " +
                "AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) END END END END END END AS DSTempHdrId " +
                "FROM(SELECT DISTINCT LessonPlanId,StudentId FROM DSTempHdr DS LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE DS.StatusId IN (SELECT LookupId FROM LookUp Look " +
                "WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) AND DS.TeachingProcId IN " +
                "(SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A')) LSN)DSINFO INNER JOIN DSTempHdr DS " +
                "ON DS.DSTempHdrId=DSINFO.DSTempHdrId INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G ON GLP.GoalId=G.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
                "WHERE GLP.ActiveInd='A' AND G.ActiveInd='A' AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' " + strCondition +
                "UNION SELECT G.GoalId,DS.LessonPlanId, DS.DSTemplateName AS LessonName FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G " +
                "ON G.GoalId=GLP.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE DS.isDynamic=0 AND DS.StudentId IS NULL AND GLP.ActiveInd='A' AND G.ActiveInd='A' " +
                "AND DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
                "AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' " + strCondition + " ORDER BY GoalId,LessonPlanId";

            DTLessonName = objData.ReturnDataTable(strLessonName, false);
            if (DTLessonName != null)
            {
                ddlLesson.Items.Clear();
                ddlLesson.DataSource = DTLessonName;
                ddlLesson.DataTextField = "LessonName";
                ddlLesson.DataValueField = "LessonPlanId";
                ddlLesson.DataBind();
                ddlLesson.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Lesson Name", "0"));
            }
        }
        else
        {
            strLessonName = "SELECT ROW_NUMBER() OVER ( ORDER BY LessonName ASC) AS RowNumber,LessonName  FROM (SELECT DISTINCT DS.DSTemplateName AS LessonName " +
                "FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON GLP.LessonPlanId = DS.LessonPlanId INNER JOIN Goal G ON G.GoalId= GLP.GoalId INNER JOIN LookUp LU " +
                "ON DS.StatusId = LU.LookupId  WHERE DS.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
                "AND DS.TeachingProcId IN (SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A') " +
                strCondition + " ) LN ORDER BY LessonName";
            DTLessonName = objData.ReturnDataTable(strLessonName, false);
            if (DTLessonName != null)
            {
                ddlLesson.Items.Clear();
                ddlLesson.DataSource = DTLessonName;
                ddlLesson.DataTextField = "LessonName";
                ddlLesson.DataValueField = "RowNumber";
                ddlLesson.DataBind();
                ddlLesson.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Lesson Name", "0"));
            }
        }
    }

    private void fillTeachingMethod()
    {
        objData = new clsData();
        DataTable DTTeachingMethod;
        string strTMethod = "SELECT LookupId, LookupName FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ActiveInd='A' AND ParentLookupId IS NOT NULL";
        DTTeachingMethod = objData.ReturnDataTable(strTMethod, false);
        if (DTTeachingMethod != null)
        {
            ddlTeachingMethod.DataSource = DTTeachingMethod;
            ddlTeachingMethod.DataTextField = "LookupName";
            ddlTeachingMethod.DataValueField = "LookupId";
            ddlTeachingMethod.DataBind();
            ddlTeachingMethod.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Teaching Method", "0"));
        }
    }

    protected void grdDatabankView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDatabankView.PageIndex = e.NewPageIndex;
        string SearchCondition = txtLessonName.Text.Trim(); ;

        if (SearchCondition == "")
        {
            this.BindDatabankView();
        }
        else
        {
            this.btnGo_Click(sender, e);
        }
    }

    protected void ddlLesson_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.PageIndex = 0;
            BindDatabankView();
        }
        else
        {
            grdClientView.PageIndex = 0;
            BindClientView();
        }
    }

    protected void ddlTeachingMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdDatabankView.PageIndex = 0;
        BindDatabankView();
    }

    protected void ddlIepYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdClientView.PageIndex = 0;
        BindClientView();
    }

    private void BindDatabankView()
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        DataTable DtDatabank;
        string strData = "";
        string strCondition = "";
        int goalId = Convert.ToInt32(ddlGoal.SelectedValue);
        int LessonId = Convert.ToInt32(ddlLesson.SelectedValue);
        int TeachingId = Convert.ToInt32(ddlTeachingMethod.SelectedValue);
        string LName = ddlLesson.SelectedItem.Text;
        string TeachingMethod = ddlTeachingMethod.SelectedItem.Text;
        string SearchCondition = txtLessonName.Text.Trim();
        ddlGoal.Width = 200;
        ddlLesson.Width = 200;
        ddlTeachingMethod.Width = 200;
        txtLessonName.Width = 275;
        if (goalId > 0)
        {
            strCondition += " AND G.GoalId = " + goalId;
        }
        if (TeachingId > 0)
        {
            strCondition += " AND LU.LookupName='" + TeachingMethod + "' ";
        }
        if (LessonId > 0)
        {
            strCondition += " AND DS.DSTemplateName= '" + clsGeneral.convertQuotes(LName) + "'";
        }
        if (SearchCondition != "")
        {
            strCondition += " AND DS.DSTemplateName like +'%'+'" + clsGeneral.convertQuotes(SearchCondition) + "'+'%'";
        }

        ////== This query returned 5000+ data which wrong

        //strData = "SELECT DISTINCT G.GoalId, G.GoalName, DSINFO.LessonPlanId, DSINFO.DSTempHdrId, DS.DSTemplateName AS LessonPlanName, DS.TeachingProcId, LU.LookupName AS TeachingMethod, DS.StudentId " +
        //    "FROM ( SELECT *,CASE WHEN EXISTS( SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId  WHERE DS.LessonPlanId=LSN.LessonPlanId " +
        //    "AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "THEN (SELECT TOP 1 DSTempHdrId  FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='In Progress' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='In Progress' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) " +
        //    "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) " +
        //    "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Expired' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) " +
        //    "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
        //    "AND LookUpType='TemplateStatus' AND LookupName='Expired' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') ORDER BY DS.DSTempHdrId DESC) " +
        //    "END END END END END END AS DSTempHdrId FROM(SELECT DISTINCT LessonPlanId,StudentId FROM DSTempHdr DS LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
        //    "WHERE DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) " +
        //    "AND DS.TeachingProcId IN (SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A')) LSN)DSINFO " +
        //    "INNER JOIN DSTempHdr DS ON DS.DSTempHdrId=DSINFO.DSTempHdrId INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G ON GLP.GoalId=G.GoalId " +
        //    "LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE GLP.ActiveInd='A' AND G.ActiveInd='A' AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' " +
        //    strCondition + " UNION SELECT G.GoalId, G.GoalName, DS.LessonPlanId, DS.DSTempHdrId, DS.DSTemplateName, DS.TeachingProcId, LU.LookupName AS TeachingMethod, DS.StudentId " +
        //    "FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G ON G.GoalId=GLP.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
        //    "WHERE DS.isDynamic=0 AND DS.StudentId IS NULL AND GLP.ActiveInd='A' AND G.ActiveInd='A' AND DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' " +
        //    "AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')) AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL " +
        //    "AND LU.ActiveInd='A' " + strCondition + " ORDER BY GoalId,LessonPlanId,TeachingProcId ";

        strData = "SELECT DISTINCT G.GoalId, G.GoalName, DSINFO.LessonPlanId, DSINFO.DSTempHdrId, DS.DSTemplateName AS LessonPlanName, DS.TeachingProcId, LU.LookupName AS TeachingMethod, DS.StudentId " +
            "FROM ( SELECT *,CASE WHEN EXISTS( SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId  WHERE DS.LessonPlanId=LSN.LessonPlanId " +
           "AND DS.StudentId=LSN.StudentId AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired')  AND DS.studentid IS NULL ) " +
            "THEN (SELECT TOP 1 DSTempHdrId  FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Approved' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Pending Approval' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='In Progress' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='In Progress' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "ELSE CASE WHEN EXISTS(SELECT DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Maintenance' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) " +
            "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Inactive' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) " +
            "ELSE CASE WHEN EXISTS(SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Expired' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) " +
            "THEN (SELECT TOP 1 DSTempHdrId FROM DSTempHdr DS LEFT JOIN LookUp L on DS.StatusId=L.LookupId WHERE DS.LessonPlanId=LSN.LessonPlanId AND DS.StudentId=LSN.StudentId " +
           "AND LookUpType='TemplateStatus' AND LookupName='Expired' AND LookupName IN('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ORDER BY DS.DSTempHdrId DESC) " +
            "END END END END END END AS DSTempHdrId FROM(SELECT DISTINCT LessonPlanId,StudentId FROM DSTempHdr DS LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
           "WHERE DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) " +
            "AND DS.TeachingProcId IN (SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A')) LSN)DSINFO " +
            "INNER JOIN DSTempHdr DS ON DS.DSTempHdrId=DSINFO.DSTempHdrId INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G ON GLP.GoalId=G.GoalId " +
            "LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId WHERE GLP.ActiveInd='A' AND G.ActiveInd='A' AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL AND LU.ActiveInd='A' " +
            strCondition + " UNION SELECT G.GoalId, G.GoalName, DS.LessonPlanId, DS.DSTempHdrId, DS.DSTemplateName, DS.TeachingProcId, LU.LookupName AS TeachingMethod, DS.StudentId " +
            "FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON DS.LessonPlanId=GLP.LessonPlanId INNER JOIN Goal G ON G.GoalId=GLP.GoalId LEFT JOIN LookUp LU ON LU.LookupId=DS.TeachingProcId " +
            "WHERE DS.isDynamic=0 AND DS.StudentId IS NULL AND GLP.ActiveInd='A' AND G.ActiveInd='A' AND DS.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' " +
           "AND Look.LookupName IN ('Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired') AND DS.studentid IS NULL ) AND LU.LookupType='Datasheet-Teaching Procedures' AND LU.ParentLookupId IS NOT NULL " +
            "AND LU.ActiveInd='A' " + strCondition + " ORDER BY GoalId,LessonPlanId,TeachingProcId ";


        DtDatabank = objData.ReturnDataTable(strData, false);

        if (DtDatabank != null)
        {
            grdDatabankView.DataSource = DtDatabank;
            int DBRowCount = DtDatabank.Rows.Count;
            if (DBRowCount < 10)
            {
                paginationBtns.Visible = false;
            }
            else
            {
                paginationBtns.Visible = true;
            }
            grdDatabankView.DataBind();
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        //tdMsg.InnerHtml = "";
        //string SearchCondition = txtLessonName.Text.Trim();
        //if (SearchCondition == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter any search condition");
        //    txtLessonName.Focus();
        //}
        //else
        //{
            if (RbtnLessonView.SelectedValue == "DatabankView")
            {
                grdDatabankView.PageIndex = 0;
                BindDatabankView();                
            }
            else
            {
                grdClientView.PageIndex = 0;
                BindClientView();                
            }
        //}
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.HeaderRow.Cells[3].Visible = false;
            grdDatabankView.HeaderRow.Cells[4].Visible = false;
            grdDatabankView.HeaderRow.Cells[5].Visible = false;
            grdDatabankView.HeaderRow.Cells[6].Visible = false;
            grdDatabankView.AllowPaging = false;
            BindDatabankView();

            PdfPTable pdfTable = new PdfPTable(3);
            int count = 0;
            foreach (System.Web.UI.WebControls.TableCell headerCell in grdDatabankView.HeaderRow.Cells)
            {
                if (count < 3)
                {
                    //Font font = new Font();
                    //PdfPCell pdfCell = new PdfPCell(new Phrase(headerCell.Text, font));
                    //pdfTable.AddCell(pdfCell);

                    iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
                    pdfTable.AddCell(new PdfPCell(new Phrase(headerCell.Text, fontH1)));
                }
                count++;
            }
            foreach (GridViewRow gridViewRow in grdDatabankView.Rows)
            {
                int countCol = 0;
                foreach (System.Web.UI.WebControls.TableCell tableCell in gridViewRow.Cells)
                {
                    if (countCol < 3)
                    {
                        //Font font = new Font();
                        //PdfPCell pdfCell = new PdfPCell(new Phrase(tableCell.Text));
                        //pdfTable.AddCell(pdfCell);

                        string DatabankCopy = Server.HtmlDecode(tableCell.Text);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);
                        pdfTable.AddCell(new PdfPCell(new Phrase(DatabankCopy, fontH1)));
                    }
                    countCol++;
                }
            }
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            pdfDoc.Add(pdfTable);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Databank_View.pdf");
            Response.Write(pdfDoc);
            Response.Flush();
            Response.End();
        }
        else
        {
            grdClientView.HeaderRow.Cells[6].Visible = false;
            grdClientView.HeaderRow.Cells[7].Visible = false;
            grdClientView.HeaderRow.Cells[8].Visible = false;
            grdClientView.AllowPaging = false;
            BindClientView();

            PdfPTable pdfTable = new PdfPTable(6);
            int count = 0;
            foreach (System.Web.UI.WebControls.TableCell headerCell in grdClientView.HeaderRow.Cells)
            {
                if (count < 6)
                {
                    //Font font = new Font();
                    //PdfPCell pdfCell = new PdfPCell(new Phrase(headerCell.Text, font));
                    //pdfTable.AddCell(pdfCell);

                    iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
                    pdfTable.AddCell(new PdfPCell(new Phrase(headerCell.Text, fontH1)));
                }
                count++;
            }
            foreach (GridViewRow gridViewRow in grdClientView.Rows)
            {
                int countCol = 0;
                foreach (System.Web.UI.WebControls.TableCell tableCell in gridViewRow.Cells)
                {
                    if (countCol < 6)
                    {
                        //Font font = new Font();
                        //PdfPCell pdfCell = new PdfPCell(new Phrase(tableCell.Text));
                        //pdfTable.AddCell(pdfCell);

                        string ClientCopy = Server.HtmlDecode(tableCell.Text);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);
                        pdfTable.AddCell(new PdfPCell(new Phrase(ClientCopy, fontH1)));

                    }
                    countCol++;
                }
            }
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            pdfDoc.Add(pdfTable);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Client_View.pdf");
            Response.Write(pdfDoc);
            Response.Flush();
            Response.End();
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DatabankView.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdDatabankView.AllowPaging = false;
                grdDatabankView.Columns[3].Visible = false;
                grdDatabankView.Columns[4].Visible = false;
                grdDatabankView.Columns[5].Visible = false;
                grdDatabankView.Columns[6].Visible = false;
                grdDatabankView.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                this.BindDatabankView();
                grdDatabankView.RenderControl(hw);
                //string style = @"<style> .textmode { } </style>";
                //Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        else
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ClientView.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdClientView.AllowPaging = false;
                grdClientView.Columns[6].Visible = false;
                grdClientView.Columns[7].Visible = false;
                grdClientView.Columns[8].Visible = false;
                grdClientView.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                grdClientView.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grdClientView.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                this.BindClientView();
                grdClientView.RenderControl(hw);
                //string style = @"<style> .textmode { } </style>";
                //Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        return;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "LoadAdminLPs();", true);
    }

    protected void grdDatabankView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "preview")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string LessonId = Id[0].ToString();
            string GoalId = Id[1].ToString();
            string DSTempHdrId = Id[2].ToString();

            ObjTempSess.TemplateId = Convert.ToInt32(DSTempHdrId);
            ClientScript.RegisterStartupScript(this.GetType(), "", "LoadLessonView(" + LessonId + ", " + GoalId + ");", true);
        }
        else if (e.CommandName == "Delete")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string DSTempHdrId = Id[0].ToString();
            string StudentId = Id[1].ToString();
            string LessonId = Id[2].ToString();

            objData = new clsData();
            SqlTransaction Transs = null;
            SqlConnection con = objData.Open();
            string DelQuery = "";
            string UpdateQuery = "";
            DataTable dtdoc = null;
            clsData.blnTrans = true;
            Transs = con.BeginTransaction();

            if (StudentId == "")
            {
                string strDynamic = "SELECT isDynamic FROM DSTempHdr WHERE DSTempHdrId ='" + DSTempHdrId + "'";
                int isDynamic = Convert.ToInt32(objData.FetchValueTrans(strDynamic, Transs, con));
                if (isDynamic == 0)
                {
                    UpdateQuery = "UPDATE DSTempHdr SET StatusId=(SELECT LookupId FROM LookUp WHERE LookupName='SoftDelete') WHERE DSTempHdrId='" + DSTempHdrId + "'";
                    objData.ExecuteWithTrans(UpdateQuery, con, Transs);
                    dtdoc = objData.ReturnDataTableWithTransaction("SELECT LPDoc FROM LPDoc WHERE DSTempHdrId IN (SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId='" + LessonId + "')", con, Transs, false);
                }
            }
            else
            {
                UpdateQuery = "UPDATE DSTempHdr SET StatusId=(SELECT LookupId FROM LookUp WHERE LookupName='SoftDelete') WHERE LessonPlanId='" + LessonId + "' AND StudentId='" + StudentId + "'";
                objData.ExecuteWithTrans(UpdateQuery, con, Transs);
                dtdoc = objData.ReturnDataTableWithTransaction("SELECT LPDoc FROM LPDoc WHERE DSTempHdrId IN (SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId='" + LessonId + "' AND StudentId='" + StudentId + "')", con, Transs, false);
            }
            if (dtdoc != null)
            {
                if (dtdoc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtdoc.Rows)
                    {
                        DelQuery = "DELETE FROM binaryFiles WHERE DocId=" + row["LPDoc"].ToString() + " AND type='LP_DOC' ";
                        objData.ExecuteWithTrans(DelQuery, con, Transs);
                    }
                }
            }
            objData.CommitTransation(Transs, con);
        }
        else if (e.CommandName == "OpenOrEdit")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string DSTempHdrId = Id[0].ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "", "LoadUpdateLesson(" + DSTempHdrId + ");", true);
        }
        else if (e.CommandName == "Export")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string LessonId = Id[0].ToString();
            string DSTempHdrId = Id[1].ToString();
            string export = "true";
            string viewmethod = "false";
            ObjTempSess.TemplateId = Convert.ToInt32(DSTempHdrId);
            //ClientScript.RegisterStartupScript(this.GetType(), "", "LessonExport(" + export + "," + LessonId + ", " + DSTempHdrId + ");", true);
            ClientScript.RegisterStartupScript(this.GetType(), "", "LessonExportNew(" + export + "," + viewmethod + "," + LessonId + ", " + DSTempHdrId + ");", true);
        }
    }

    protected void grdDatabankView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grdDatabankView.EditIndex = -1;
        grdDatabankView.DataBind();
    }

    private void fillClientName()
    {
        objData = new clsData();
        DataTable DTStudent;
        string strStudent = "SELECT StudentPersonalId AS StudentId, FirstName+' '+LastName  AS StudentName FROM StudentPersonal WHERE StudentType ='Client' ORDER BY StudentName";
        DTStudent = objData.ReturnDataTable(strStudent, false);
        if (DTStudent != null)
        {
            ddlClientName.DataSource = DTStudent;
            ddlClientName.DataTextField = "StudentName";
            ddlClientName.DataValueField = "StudentId";
            ddlClientName.DataBind();
            ddlClientName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Client Name", "0"));
        }
    }

    protected void ddlClientName_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdClientView.PageIndex = 0;
        BindClientView();
    }

    private void BindClientView()
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        DataTable DtClient;
        string strData = "";
        string strCondition1 = "";
        string strCondition2 = "";
        int goalId = Convert.ToInt32(ddlGoal.SelectedValue);
        int LessonId = Convert.ToInt32(ddlLesson.SelectedValue);
        int StudentId = Convert.ToInt32(ddlClientName.SelectedValue);
        string LName = ddlLesson.SelectedItem.Text;
        string SearchCondition = txtLessonName.Text.Trim();
        //string IepYear = ddlIepYear.SelectedValue;
        string IepYear = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddlIepYear.Items)
        {
            if (item.Selected == true)
            {
                //if (item.Text.Equals("All"))
                //{
                //    if(ddlIepYear.Items.Count > 1)
                //    item.Selected = false;
                //    continue;
                //}
                IepYear += "'" + item.Text + "',";
            }
        }
        string LPStatus = "";
        ddlGoal.Width = 150;
        ddlLesson.Width = 150;
        ddlTeachingMethod.Width = 150;
        txtLessonName.Width = 200;
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Approved")
                {
                    LPStatus += "'Approved',";
                }
                else if (item.Text == "Pending Approval")
                {
                    LPStatus += "'Pending Approval',";
                }
                else if (item.Text == "In Progress")
                {
                    LPStatus += "'In Progress',";
                }
                else if (item.Text == "Maintenance")
                {
                    LPStatus += "'Maintenance',";
                }
                else if (item.Text == "Inactive")
                {
                    LPStatus += "'Inactive',";
                }
                else if (item.Text == "Rejected")
                {
                    LPStatus += "'Expired',";
                }
            }
        }
        if (LPStatus == "")
        {
            LPStatus = " 'Approved', 'Pending Approval', 'In Progress', 'Maintenance', 'Inactive', 'Expired' ";
        }
        LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
        strCondition1 = " AND LookupName IN (" + LPStatus + ") ";

        if (goalId > 0)
        {
            strCondition2 += " AND G.GoalId = " + goalId;
        }
        if (StudentId > 0)
        {
            strCondition2 += " AND DS.StudentId = " + StudentId;
        }
        if (LessonId != 0)
        {
            strCondition2 += " AND DS.DSTemplateName= '" + LName + "'";
        }
        if (SearchCondition != "")
        {
            strCondition2 += " AND DS.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
        }

        if (IepYear != null)
        {
            //int getIepYear = Convert.ToInt32(IepYear);
            //if (getIepYear > 0)
            //{
            //    //strCondition2 += " AND DS.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            //    strCondition2 += "AND YEAR(DS.LessonSDate) = " + getIepYear + "";
            //}
            if (IepYear != "")
            {
                IepYear = IepYear.Substring(0, IepYear.Length - 1);
                //if (IepYear != "'All'")
                //{
                strCondition2 += "AND YEAR(DS.LessonSDate) IN (" + IepYear + ")";
                //}
            }
        }

        strData = "SELECT StudentId, StudentName, GoalId, GoalName, LessonPlanId, DSTempHdrId, LessonName, StatusId ,IEPSDate,IEPEDate,CASE WHEN LessonStatus = 'Expired' THEN 'Rejected' ELSE LessonStatus END AS LessonStatus " +
            "FROM (SELECT DISTINCT DS.StudentId, SP.FirstName+' '+SP.LastName AS StudentName, G.GoalId, G.GoalName, DS.LessonPlanId, DSTempHdrId, DS.DSTemplateName AS LessonName,(Select CONVERT(VARCHAR, DS.LessonSDate , 101)) AS IEPSDate,(Select CONVERT(VARCHAR, DS.LessonEDate , 101)) AS IEPEDate, " +
            "DS.StatusId ,LU.LookupName  AS LessonStatus FROM DSTempHdr DS INNER JOIN GoalLPRel GLP ON GLP.LessonPlanId = DS.LessonPlanId INNER JOIN Goal G ON G.GoalId= GLP.GoalId " +
            "INNER JOIN LookUp LU ON DS.StatusId = LU.LookupId INNER JOIN StudentPersonal SP ON SP.StudentPersonalId=DS.StudentId WHERE DS.StatusId IN " +
            "(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' " + strCondition1 + " ) AND DS.TeachingProcId IN (SELECT LookupId FROM LookUp WHERE " +
            "LookupType='Datasheet-Teaching Procedures' AND ParentLookupId IS NOT NULL AND ActiveInd='A') " + strCondition2 + " ) LSN ORDER BY StudentId, GoalId, LessonPlanId, StatusId ";
        DtClient = objData.ReturnDataTable(strData, false);

        if (DtClient != null)
        {
            grdClientView.DataSource = DtClient;
            int ClRowCount = DtClient.Rows.Count;
            if (ClRowCount < 10)
            {
                paginationBtns.Visible = false;
            }
            else
            {
                paginationBtns.Visible = true;
            }
            grdClientView.DataBind();
        }
    }

    protected void grdClientView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdClientView.PageIndex = e.NewPageIndex;
        BindClientView();
    }


    protected void ddlLessonStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdClientView.PageIndex = 0;
        BindClientView();
    }

    protected void grdClientView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "preview")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string LessonId = Id[0].ToString();
            string DSTempHdrId = Id[1].ToString();
            string GoalId = Id[2].ToString();
            string studentId = Id[3].ToString();

            sess.StudentId = Convert.ToInt32(studentId);
            ObjTempSess.TemplateId = Convert.ToInt32(DSTempHdrId);
            ClientScript.RegisterStartupScript(this.GetType(), "", "LoadClientLessonView(" + LessonId + ", " + GoalId + ", " + studentId + ");", true);
        }
        if (e.CommandName == "copyToBank")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string DSTempHdrId = Id[0].ToString();
            int GoalId = Convert.ToInt32(Id[1].ToString());            
            string studentId = Id[2].ToString();            
            string LessonPlanName = Id[3].ToString();

            ObjTempSess.TemplateId = Convert.ToInt32(DSTempHdrId);
            ViewState["HeaderId"] = Convert.ToInt32(DSTempHdrId);
            sess.StudentId = Convert.ToInt32(studentId);
            ViewState["GoalId"] = GoalId;
            hdnLessonName.Value = LessonPlanName;
            ClientScript.RegisterStartupScript(this.GetType(), "", "LoadCopyToDatabank();", true);
        }
        else if (e.CommandName == "Export")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string LessonId = Id[0].ToString();
            string DSTempHdrId = Id[1].ToString();
            string studid = Id[2].ToString();
            sess.StudentId = Convert.ToInt32(studid);
            string export = "true";
            string viewmethod = "true";
            ObjTempSess.TemplateId = Convert.ToInt32(DSTempHdrId);
            //ClientScript.RegisterStartupScript(this.GetType(), "", "LessonExport(" + export + "," + LessonId + ", " + DSTempHdrId + ");", true);         
            ClientScript.RegisterStartupScript(this.GetType(), "", "LessonExportNew(" + export + "," + viewmethod + "," + LessonId + ", " + DSTempHdrId + ");", true);
        }
        if (e.CommandName == "MEDS Export")
        {
            string newval = e.CommandArgument.ToString();
            string[] Id = newval.Split(',');
            string LessonId = Id[0].ToString();
            string DSTempHdrId = Id[1].ToString();
            string studid = Id[2].ToString();
            sess.StudentId = Convert.ToInt32(studid);
            exportlessondata(studid, DSTempHdrId, LessonId, 0);
        }

    }
    protected void grdClientView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string lessonStatus = DataBinder.Eval(e.Row.DataItem, "LessonStatus").ToString();

            if (lessonStatus == "In Progress" || lessonStatus == "Pending Approval")
            {
              System.Web.UI.WebControls.ImageButton btnMedsExport = (System.Web.UI.WebControls.ImageButton)e.Row.FindControl("lb_clnt_exprtmds");

                if (btnMedsExport != null)
                {
                    btnMedsExport.Visible = false;
                }
            }
        }
    }
    public void exportlessondata(string studid, string DSTempHdrId, string LessonId, int num)
    {
        objData = new clsData();
        DataSet exportSet = new DataSet();

        String lessplanandgoal = "SELECT  d.DSTemplateName, g.GoalName FROM DSTempHdr d INNER JOIN StdtLessonPlan s ON d.StdtLessonPlanId = s.StdtLessonPlanId INNER JOIN Goal g  ON s.GoalId = g.GoalId WHERE d.DSTempHdrId in (" + DSTempHdrId + ")";


        string lookupqrymap = "SELECT lookupId AS lid, LookupName AS lname,LookupCode AS lcode,LookupDesc AS ldes, NULL AS paid FROM lookup WHERE LookupType = 'Datasheet-Teaching Procedures' AND ActiveInd = 'A'";
        String lookupproc = "SELECT  LookupId as lid,LookupName as lname,LookupCode as lcode,LookupDesc as ldes,null as paid FROM    lookup WHERE    LookupType = 'Datasheet-Prompt Procedures'    AND ActiveInd = 'A'";
        String lookupprompt = "SELECT    LookupId as lid,LookupName as lname,LookupCode as lcode,LookupDesc as ldes,null as paid FROM   lookup WHERE     LookupType = 'DSTempPrompt'    AND ActiveInd = 'A'";
        string stdtGoalQry = "SELECT SchoolId,StudentId,GoalId,NULL AS AsmntYearId,0 AS IncludeIEP,'A' AS ActiveInd, CreatedBy,GETDATE() AS CreatedOn FROM StdtGoal WHERE SchoolId = " + sess.SchoolId + " AND StudentId in( " + studid + ")";
        //string tempLessPlanQry = "SELECT LessonPlanId,SchoolId,LessonPlanName,LessonPlanDesc,PreReq,TeacherSD, TeacherInst,Consequence,BaselineProc,PostCheckProc, ImageURL,Materials,FrameandStrand,SpecStandard,SpecEntryPoint," +
        //                       "ActiveInd,CreatedBy,GETDATE() AS CreatedOn,ModifiedBy,ModifiedOn,Baseline,Objective,LessonSDate,LessonEDate,NULL AS Newless FROM LessonPlan WHERE SchoolId = " + sess.SchoolId + " AND LessonPlanId IN (SELECT LessonPlanId FROM DSTempHdr " +
        //                       "WHERE StudentId in( " + studid + ") AND LessonPlanId in (" + LessonId + ") AND  StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType = 'TemplateStatus' AND LookupName IN ('Approved','Maintenance','Inactive')))";

        string tempLessPlanQry ="SELECT lp.LessonPlanId, lp.SchoolId, hdr.DSTemplateName AS LessonPlanName, " +
                                   "hdr.PreReq, hdr.BaselineProc, hdr.Materials, hdr.FrameandStrand, " +
                                   "hdr.SpecStandard, hdr.SpecEntryPoint, " +
                                   "'A' AS ActiveInd, lp.CreatedBy, GETDATE() AS CreatedOn, " +
                                   "NULL AS ModifiedBy, NULL AS ModifiedOn, " +
                                   "hdr.Baseline, hdr.Objective, hdr.LessonSDate, hdr.LessonEDate, " +
                                   "NULL AS Newless " +
                                   "FROM LessonPlan lp " +
                                   "INNER JOIN DSTempHdr hdr ON lp.LessonPlanId = hdr.LessonPlanId " +
                                   "WHERE lp.SchoolId = " + sess.SchoolId +
                                   " AND hdr.DSTempHdrId IN (" + DSTempHdrId + ")" +
                                   " AND hdr.StudentId IN (" + studid + ")" +
                                   " AND hdr.StatusId IN " +
                                   "(SELECT LookupId FROM LookUp " +
                                   " WHERE LookupType = 'TemplateStatus' " +
                                   " AND LookupName IN ('Approved','Maintenance','Inactive'))";

        string goalLPRelQry = "SELECT GoalId,LessonPlanId,ActiveInd,CreatedBy,GETDATE() AS CreatedOn,NULL AS ModifiedBy,NULL AS ModifiedOn FROM GoalLPRel WHERE LessonPlanId IN(SELECT DISTINCT LessonPlanId FROM LessonPlan WHERE SchoolId = " + sess.SchoolId +
                              " AND LessonPlanId IN(SELECT LessonPlanId FROM DSTempHdr WHERE StudentId in( " + studid + ") AND LessonPlanId in (" + LessonId + ") AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType = 'TemplateStatus' AND LookupName IN ('Approved','Maintenance','Inactive'))))";

        string tempStdtLessPlanQry = "SELECT StdtLessonPlanId,SchoolId,StudentId,LessonPlanId,GoalId,'false' AS IncludeIEP,'A' AS ActiveInd,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,GETDATE() AS CreatedOn, isDynamic,NULL AS newstdtless FROM StdtLessonPlan WHERE StudentId in( " + studid + ")" +
                                    "AND SchoolId = " + sess.SchoolId + "AND LessonPlanId IN( SELECT DISTINCT LessonPlanId FROM LessonPlan WHERE SchoolId = " + sess.SchoolId + " AND LessonPlanId IN (SELECT LessonPlanId FROM DSTempHdr WHERE StudentId in( " + studid + ") AND LessonPlanId in (" + LessonId + ") AND  StatusId IN" +
                                     " (SELECT LookupId FROM LookUp WHERE LookupType = 'TemplateStatus' AND LookupName IN ('Approved','Maintenance','Inactive') ) ))";


        string tempDSTempHdrQry = "SELECT [DSTempHdrId],ds.SchoolId, StudentId, ds.LessonPlanId,[TeachingProcId],[ModificationInd],[DSTemplateName],[DSTemplateDesc],NULL AS VerNbr,[VerBeginDate],[VerEndDate],[CurrVerInd],[MultiSetsInd]," +
                                   " [MultiStepInd], [SkillType],[NbrOfTrials], [ChainType], [PromptTypeId], [TotNbrOfSessions], [SessionFreq], [NbrOfSession], [CompCurrInd], StatusId, [IsVisualTool], [VTLessonId], ds.[BaselineProc]," +
                                   "[BaselineStart], [BaselineEnd], [CorrRespDef], [CorrectResponse], [StudCorrRespDef], [IncorrRespDef], [StudIncorrRespDef], [CorrectionProc], [ReinforcementProc], [TeacherRespReadness], [StudentReadCrita], [MajorSetting]," +
                                    "[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse],[TeacherPrepare],[StudentPrepare], [StudResponse], NULL AS DSMode, [RejectedReason], [PrevStatus], ds.[CreatedBy], GETDATE() AS CreatedOn, NULL AS ModifiedBy," +
                                   "NULL AS ModifiedOn,StdtLessonplanId,ds.[isDynamic],ds.[Baseline],ds.[Objective],[TotalTaskType],[TaskOther],[GeneralProcedure],[MatchToSampleType],ds.[Materials],ds.[PreReq],ds.[SpecEntryPoint],ds.[SpecStandard]," +
                                    "[ApprNoteLessonProc], [ApprNoteMeasurement], [ApprNotePrompt],[ApprNoteSet],[ApprNoteStep],[ApprNoteTypeInstruction],ds.[FrameandStrand],[TotalTaskFormat],[ApprNoteLessonInfo],[LessonPlanGoal],[MatchToSampleRecOrExp]," +
                                    "[IsMT_IOA],[Reason_New],[NoofTimesTried], [LessonOrder], [deletessn], [CrntSet], [CrntStep], [CrntPrompt], [NextSetNo], ds.[LessonSDate], ds.[LessonEDate], [LessonStatusforBanner],[Bannerstatus], [NoofTimesTriedPer],NULL AS dsthdrkey FROM DSTempHdr ds " +
                                    "INNER JOIN LessonPlan pln ON ds.LessonPlanId = pln.LessonPlanId WHERE StudentId in( " + studid + ") AND ds.SchoolId = " + sess.SchoolId + " AND StatusId IN ( SELECT LookupId  FROM LookUp  WHERE LookupType='TemplateStatus'  AND LookupName IN ('Approved','Maintenance','Inactive')) AND ds.DSTempHdrId in (" + DSTempHdrId + ") AND ds.LessonPlanId in (" + LessonId + ")";
        string tempDSTempPromptQry = "SELECT dp.*, NULL AS ModifiedByExport FROM DSTempPrompt dp INNER JOIN DSTempHdr hdr ON dp.DSTempHdrId = hdr.DSTempHdrId  WHERE hdr.StudentId in( " + studid + ") AND hdr.SchoolId = " + sess.SchoolId + " AND hdr.LessonPlanId in( " + LessonId + ") AND dp.DSTempHdrId in (" + DSTempHdrId + ")";
        string tempDSTempSetQry = "SELECT ds.*, NULL AS newsetid " +
                                    " FROM DSTempSet ds " +
                                    "INNER JOIN DSTempHdr hdr ON ds.DSTempHdrId = hdr.DSTempHdrId " +
                                    " WHERE hdr.StudentId in( " + studid +
                                    ")  AND hdr.SchoolId = " + sess.SchoolId +
                                    "  AND hdr.LessonPlanId in( " + LessonId +
                                    ") AND ds.ActiveInd = 'A' AND ds.DSTempHdrId in(" + DSTempHdrId + ")";

        string tempParentStepQry = "SELECT p.*, NULL AS newparentstepid " +
                                    " FROM DSTempParentStep p " +
                                    "INNER JOIN DSTempHdr hdr ON p.DSTempHdrId = hdr.DSTempHdrId " +
                                    " WHERE hdr.StudentId in( " + studid +
                                    ") AND hdr.SchoolId = " + sess.SchoolId +
                                    " AND hdr.LessonPlanId in( " + LessonId +
                                    ") AND p.ActiveInd = 'A' AND p.DSTempHdrId in (" + DSTempHdrId + ")";

        string tempStepQry = "SELECT stp.*, NULL AS newstep " +
                            "FROM DSTempStep stp " +
                            "INNER JOIN DSTempHdr hdr ON stp.DSTempHdrId = hdr.DSTempHdrId " +
                            "WHERE hdr.StudentId in( " + studid +
                            ") AND hdr.SchoolId = " + sess.SchoolId +
                            " AND hdr.LessonPlanId in( " + LessonId + ") AND hdr.DSTempHdrId in(" + DSTempHdrId + ")";

        string tempSetColQry = "SELECT col.*, NULL AS newsetcolid " +
                                "FROM DSTempSetCol col " +
                                "INNER JOIN DSTempHdr hdr ON col.DSTempHdrId = hdr.DSTempHdrId " +
                                "WHERE hdr.StudentId in( " + studid +
                                ") AND hdr.SchoolId = " + sess.SchoolId +
                                " AND hdr.LessonPlanId in ( " + LessonId + ") AND hdr.DSTempHdrId in  (" + DSTempHdrId + ") AND ActiveInd='A'";

        string tempSetColCalcQry = "SELECT calc.*, NULL AS newcolcalcid " +
                                    "FROM DSTempSetColCalc calc " +
                                    "WHERE calc.SchoolId = " + sess.SchoolId +
                                    " AND calc.DSTempSetColId IN (SELECT DSTempSetColId FROM DSTempSetCol where ActiveInd='A' AND DSTempHdrId in  (" + DSTempHdrId + "))";


        string tempRuleQry =
                            "SELECT r.* " +
                            "FROM DSTempRule r " +
                            "INNER JOIN DSTempHdr hdr ON hdr.DSTempHdrId = r.DSTempHdrId " +
                            "WHERE hdr.StudentId in( " + studid +
                            ") AND hdr.SchoolId = " + sess.SchoolId +
                            " AND hdr.LessonPlanId in( " + LessonId + ") AND hdr.DSTempHdrId in( " + DSTempHdrId + ") AND r.ActiveInd = 'A'";


        string tempLPDocQry =
                                "SELECT lp.*, NULL AS newdocid " +
                                "FROM LPDoc lp " +
                                "INNER JOIN DSTempHdr hdr ON hdr.DSTempHdrId = lp.DSTempHdrId " +
                                "WHERE hdr.StudentId in( " + studid +
                                ") AND hdr.SchoolId = " + sess.SchoolId +
                                " AND hdr.LessonPlanId in( " + LessonId + ") AND hdr.DSTempHdrId in (" + DSTempHdrId + ")";


        string tempBinaryFilesQry =
                                "SELECT bin.* " +
                                "FROM binaryFiles bin " +
                                "WHERE bin.SchoolId = " + sess.SchoolId +
                                " AND bin.DocId IN (SELECT LPDoc FROM LPDoc WHERE DSTempHdrId in( " + DSTempHdrId + "))";


        DataTable lessandgoaldt = objData.ReturnDataTable(lessplanandgoal, false);
        DataTable lookupmapdt = objData.ReturnDataTable(lookupqrymap, false);
        DataTable lookupprocdt = objData.ReturnDataTable(lookupproc, false);
        DataTable lookuppromptdt = objData.ReturnDataTable(lookupprompt, false);
        DataTable stdtGoalDt = objData.ReturnDataTable(stdtGoalQry, false);
        DataTable tempLessPlanDt = objData.ReturnDataTable(tempLessPlanQry, false);
        DataTable goalLPRelDt = objData.ReturnDataTable(goalLPRelQry, false);
        DataTable tempStdtLessPlanDt = objData.ReturnDataTable(tempStdtLessPlanQry, false);
        DataTable tempDSTempHdrDt = objData.ReturnDataTable(tempDSTempHdrQry, false);
        DataTable tempDSTempPromptDt = objData.ReturnDataTable(tempDSTempPromptQry, false);
        DataTable tempDSTempSetDt = objData.ReturnDataTable(tempDSTempSetQry, false);
        DataTable tempParentStepDt = objData.ReturnDataTable(tempParentStepQry, false);
        DataTable tempStepDt = objData.ReturnDataTable(tempStepQry, false);
        DataTable tempSetColDt = objData.ReturnDataTable(tempSetColQry, false);
        DataTable tempSetColCalcDt = objData.ReturnDataTable(tempSetColCalcQry, false);
        DataTable tempRuleDt = objData.ReturnDataTable(tempRuleQry, false);
        DataTable tempLPDocDt = objData.ReturnDataTable(tempLPDocQry, false);
        DataTable tempBinaryFilesDt = objData.ReturnDataTable(tempBinaryFilesQry, false);

  


        lessandgoaldt.TableName = "LessonNameAndGoal";
        lookupmapdt.TableName = "LookupIdMapping";
        lookupprocdt.TableName = "LookupProcIdMapping";
        lookuppromptdt.TableName = "LookupPrompt";
        stdtGoalDt.TableName = "StdtGoal";
        tempLessPlanDt.TableName = "Temp_lessplan";
        goalLPRelDt.TableName = "GoalLPRel";
        tempStdtLessPlanDt.TableName = "Temp_stdtlessplan";
        tempDSTempHdrDt.TableName = "Temp_DSTempHdr";
        tempDSTempPromptDt.TableName = "Temp_DSTempPrompt";
        tempDSTempSetDt.TableName = "Temp_DSTempSet";
        tempParentStepDt.TableName = "Temp_DSTempParentStep";
        tempStepDt.TableName = "Temp_DSTempStep";
        tempSetColDt.TableName = "Temp_DSTempSetCol";
        tempSetColCalcDt.TableName = "Temp_DSTempSetColCalc";
        tempRuleDt.TableName = "Temp_DSTempRule";
        tempLPDocDt.TableName = "Temp_LPDoc";
        tempBinaryFilesDt.TableName = "Temp_binaryFiles";

        exportSet.Tables.Add(lessandgoaldt);
        exportSet.Tables.Add(lookupmapdt);
        exportSet.Tables.Add(lookupprocdt);
        exportSet.Tables.Add(lookuppromptdt);
        exportSet.Tables.Add(stdtGoalDt);
        exportSet.Tables.Add(tempLessPlanDt);
        exportSet.Tables.Add(goalLPRelDt);
        exportSet.Tables.Add(tempStdtLessPlanDt);
        exportSet.Tables.Add(tempDSTempHdrDt);
        exportSet.Tables.Add(tempDSTempPromptDt);
        exportSet.Tables.Add(tempDSTempSetDt);
        exportSet.Tables.Add(tempParentStepDt);
        exportSet.Tables.Add(tempStepDt);
        exportSet.Tables.Add(tempSetColDt);
        exportSet.Tables.Add(tempSetColCalcDt);
        exportSet.Tables.Add(tempRuleDt);
        exportSet.Tables.Add(tempLPDocDt);
        exportSet.Tables.Add(tempBinaryFilesDt);
     

        string json =
Newtonsoft.Json.JsonConvert.SerializeObject(
exportSet,
Newtonsoft.Json.Formatting.Indented
);
        //        exportstatus.Value = "1";

        //        Response.Clear();
        //        Response.ContentType = "application/json";
        //        Response.AddHeader(
        //    "content-disposition",
        //    "attachment; filename=lessonexport.json"
        //);
        //        Response.Write(json);
        //        Response.End();

        string script = @"
        downloadJsonFile(" + Newtonsoft.Json.JsonConvert.SerializeObject(json) + @");
    ";

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "downloadFile",
            script,
            true
        );



    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        txtLessonName.Text = "";
        tdMsg.InnerHtml = "";

        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            ddlLessonStatus.Visible = false;
            btnAdd.Visible = true;
            grdClientView.Visible = false;
            ddlTeachingMethod.Visible = true;
            grdDatabankView.Visible = true;
            ddlIepYear.Visible = false;
            iepPtag.Visible = false;
            ddlClientName.Visible = false;
            fillGoal();
            fillLessonName();
            fillTeachingMethod();
            grdDatabankView.AllowPaging = true;
            grdDatabankView.PageSize = 10;
            grdDatabankView.PageIndex = 0;
            paginationBtns.Visible = true;
            BindDatabankView();

        }
        else
        {
            ddlLessonStatus.Visible = true;
            foreach (System.Web.UI.WebControls.ListItem item in ddlLessonStatus.Items)
            {
                if (item.Selected == true)
                {
                    item.Selected = false;
                }
            }
            btnAdd.Visible = false;
            ddlTeachingMethod.Visible = false;
            ddlIepYear.Visible = true;
            iepPtag.Visible = true;
            grdDatabankView.Visible = false;
            ddlClientName.Visible = true;
            grdClientView.Visible = true;
            fillGoal();
            fillYear();
            fillLessonName();
            fillClientName();
            grdClientView.AllowPaging = true;
            grdClientView.PageSize = 10;
            grdClientView.PageIndex = 0;
            paginationBtns.Visible = true;
            BindClientView();
        }
    }

    protected void btn10_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.AllowPaging = true;
            grdDatabankView.PageSize = 10;
            grdDatabankView.PageIndex = 0;
            BindDatabankView();
        }
        else
        {
            grdClientView.AllowPaging = true;
            grdClientView.PageSize = 10;
            grdClientView.PageIndex = 0;
            BindClientView();
        }
    }

    protected void btn20_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.AllowPaging = true;
            grdDatabankView.PageSize = 20;
            grdDatabankView.PageIndex = 0;
            BindDatabankView();
        }
        else
        {
            grdClientView.AllowPaging = true;
            grdClientView.PageSize = 20;
            grdClientView.PageIndex = 0;
            BindClientView();
        }
    }

    protected void btn50_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.AllowPaging = true;
            grdDatabankView.PageSize = 50;
            grdDatabankView.PageIndex = 0;
            BindDatabankView();
        }
        else
        {
            grdClientView.AllowPaging = true;
            grdClientView.PageSize = 50;
            grdClientView.PageIndex = 0;
            BindClientView();
        }
    }

    protected void btn100_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.AllowPaging = true;
            grdDatabankView.PageSize = 100;
            grdDatabankView.PageIndex = 0;
            BindDatabankView();
        }
        else
        {
            grdClientView.AllowPaging = true;
            grdClientView.PageSize = 100;
            grdClientView.PageIndex = 0;
            BindClientView();
        }
    }

    protected void btnAll_Click(object sender, EventArgs e)
    {
        if (RbtnLessonView.SelectedValue == "DatabankView")
        {
            grdDatabankView.AllowPaging = false;
            BindDatabankView();
        }
        else
        {
            grdClientView.AllowPaging = false;
            BindClientView();
        }
    }

    [WebMethod]
    public static string SearchLessonPlanList(string Name)
    {
        object GoalApproved = HttpContext.Current.Session["GoalID_Approved"];
        Dataobj = new clsData();
        int LPcount = 0;
        LPcount = Convert.ToInt32(Dataobj.FetchValue("SELECT COUNT(*) FROM DSTempHdr HD inner join GoalLPRel GR on HD.lessonplanid=GR.lessonplanid WHERE  RTRIM(LTRIM(LOWER(DSTemplateName)))= RTRIM(LTRIM(LOWER('" + Name.Trim() + "'))) and StatusId IN (select LookupId from LookUp where LookupType='TemplateStatus' and LookupName IN ('Approved', 'In Progress', 'Pending Approval', 'Maintenance', 'Inactive', 'Expired')) AND StudentId IS NULL AND isDynamic=0 AND GR.GOALID='" + GoalApproved + "' AND GR.ACTIVEIND='A'"));
        if (LPcount > 0)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }

    protected void btnCopyToDataBank_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int apprvdLessonId = 0;
        int visualLessonId = 0;
        int NewLpid = 0;
        apprvdLessonId = Convert.ToInt32(ObjTempSess.TemplateId);
        tdMsg.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        int GoalId = Convert.ToInt32(ViewState["GoalId"]);
        Session["GoalID_Approved"] = GoalId;
        if (ViewState["HeaderId"] != null)
        {
            apprvdLessonId = Convert.ToInt32(ViewState["HeaderId"]);
        }

        visualLessonId = ReturnNewVLessonId(apprvdLessonId);
        int OldLpId = Convert.ToInt32(objData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + apprvdLessonId));
        try
        {
            NewLpid = AddLessonPlan(hdnLessonName.Value, GoalId, OldLpId, apprvdLessonId);
            if (NewLpid > 0)
            {
                int tempid = CopyCustomtemplate(apprvdLessonId, sess.LoginId, visualLessonId);
                if (tempid > 0)
                {
                    CreateDocument(apprvdLessonId, tempid);
                    string UpdateLessonName = "UPDATE DSTempHdr SET DSTemplateName='" + hdnLessonName.Value + "',LessonPlanId='" + NewLpid + "',isDynamic=0 WHERE DSTempHdrId=" + tempid;
                    objData.Execute(UpdateLessonName);
                    string NewName = "";
                    if (hdnLessonName.Value != "")
                    {
                        NewName = "to <h3>" + hdnLessonName.Value + "</h3>";
                    }
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Template Successfully Copied " + NewName);
                    BindLessonCount();
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected int ReturnNewVLessonId(int templateId)
    {
        objData = new clsData();
        oData = new DataClass();
        int studId = sess.StudentId;
        int newVisualLessonId = 0;
        string selctQuerry = "";
        selctQuerry = "SELECT VTLessonId FROM DSTempHdr WHERE DSTempHdrId = " + templateId;
        object objVt = objData.FetchValue(selctQuerry);
        if (objVt != null)
        {
            if (objVt.ToString() != "")
            {
                int vtId = Convert.ToInt32(objVt);
                if (vtId > 0)
                {
                    try
                    {
                        int isStEdit = 1;
                        int isCcEdit = 0;
                        string selctSpQuerry = "sp_copyLessonPlan";     // Stored Procedure call for duplicate Lessonplan
                        int newLessonId = oData.Execute_SpCopyLesson(selctSpQuerry, vtId, isStEdit, isCcEdit);
                        if (newLessonId > 0)
                        {
                            string selctLp = "SELECT MAX(LessonId) FROM LE_Lesson";
                            newVisualLessonId = Convert.ToInt32(objData.FetchValue(selctLp));
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }
                }
            }
        }
        return newVisualLessonId;
    }
    protected int AddLessonPlan(string LpName, int GoalId, int oldLp, int DSTempId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        SqlConnection con = new SqlConnection();
        con = objData.Open();
        SqlTransaction trans = con.BeginTransaction();
        int LPid = 0;
        try
        {
            if (sess != null)
            {
                string insLP = "";
                insLP = "insert into LessonPlan(SchoolId,[PreReq],[BaselineProc],[Materials],[FrameandStrand],[SpecStandard],[SpecEntryPoint],ActiveInd,LessonPlanName,CreatedBy,CreatedOn,[Baseline],[Objective],LessonSDate,LessonEDate) " +
                    "SELECT " + sess.SchoolId + ",[PreReq],[BaselineProc],[Materials],[FrameandStrand],[SpecStandard],[SpecEntryPoint],'A','" + LpName + "'," + sess.LoginId + ",GETDATE(),[Baseline],[Objective],LessonSDate,LessonEDate FROM DSTempHdr WHERE DSTempHdrId=" + DSTempId;
                LPid = objData.ExecuteWithScopeandConnection(insLP, con, trans);
                if (LPid > 0)
                {
                    string strQuery = "INSERT into GoalLPRel(GoalId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn) " +
                    "Values('" + GoalId.ToString() + "'," + LPid.ToString() + ",'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                    objData.ExecuteWithScopeandConnection(strQuery, con, trans);
                    objData.CommitTransation(trans, con);
                }
            }
            return LPid;
        }
        catch (Exception ex)
        {
            objData.RollBackTransation(trans, con);
            con.Close();
            return 0;
            throw ex;
        }
    }
    public int CopyCustomtemplate(int templateid, int loginid, int visualLessonId, int studentid = 0, int stdtLpId = 0)
    {
        SqlTransaction Trans = null;
        SqlConnection Con = null;
        objData = new clsData();
        string strQuery = "";
        int oldSetId = 0;
        int parentSetId = 0;
        clsAssignLessonPlan AssignLP = new clsAssignLessonPlan();
        try
        {
            Con = objData.Open();
            Trans = Con.BeginTransaction();
            strQuery = "SELECT LessonPlanId,StudentId,SchoolId from DSTempHdr WHERE DSTempHdrId=" + templateid;
            DataTable dt = new DataTable();
            dt = objData.ReturnDataTable(strQuery, Con, Trans, false);
            string stid = "", stval = "";
            int schoolid = Convert.ToInt32(dt.Rows[0]["SchoolId"]);

            if (studentid == 0 && stdtLpId == 0)
            {
                stid = ",";
                stval = ",";
            }
            else
            {
                stid = ",[StudentId],[StdtLessonplanId],";
                stval = ",'" + studentid + "','" + stdtLpId + "',";
            }
            if (schoolid == 1)
                strQuery = "INSERT INTO DSTempHdr ([SchoolId]" + stid + "[LessonPlanId],[TeachingProcId],[DSTemplateName]," +
               "[DSTemplateDesc],[VerBeginDate],[NoofTimesTried],[NoofTimesTriedPer],[VerEndDate],[CurrVerInd],[MultiSetsInd],[MultiStepInd],[SkillType],[MatchToSampleType],[NbrOfTrials]," +
               "[ChainType],[TotalTaskFormat],[TotalTaskType],[TaskOther],[MatchToSampleRecOrExp],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd],[StatusId],[IsVisualTool]," +
               "[VTLessonId],[Baseline],[Objective],[GeneralProcedure],[BaselineProc],[BaselineStart],[BaselineEnd],[CorrRespDef]," +
               "[CorrectResponse],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef],[CorrectionProc],[ReinforcementProc]," +
               "[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse]," +
               "[TeacherPrepare],[StudentPrepare],[StudResponse],[DSMode],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]," +
               "[FrameandStrand],[LessonPlanGoal],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[ApprNoteLessonInfo],[ApprNoteTypeInstruction],[ApprNoteMeasurement],[ApprNoteSet],[ApprNoteStep],[ApprNotePrompt],[ApprNoteLessonProc],[deletessn],[LessonOrder],[LessonSDate],[LessonEDate]) SELECT [SchoolId]" + stval + "[LessonPlanId]," +
               "[TeachingProcId],[DSTemplateName],[DSTemplateDesc],[VerBeginDate],[NoofTimesTried],[NoofTimesTriedPer],[VerEndDate],[CurrVerInd],[MultiSetsInd],[MultiStepInd]," +
               "[SkillType],[MatchToSampleType],[NbrOfTrials],[ChainType],[TotalTaskFormat],[TotalTaskType],[TaskOther],[MatchToSampleRecOrExp],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd]," +
               "(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress'),[IsVisualTool]," +
               "'" + visualLessonId + "',[Baseline],[Objective],[GeneralProcedure],[BaselineProc],[BaselineStart],[BaselineEnd]," +
               "[CorrRespDef],[CorrectResponse],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef],[CorrectionProc],[ReinforcementProc]," +
               "[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse]," +
               "[TeacherPrepare],[StudentPrepare],[StudResponse],[DSMode]," + loginid + ",GETDATE()," + loginid + ",GETDATE(),[FrameandStrand],[LessonPlanGoal]," +
               "[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[ApprNoteLessonInfo],[ApprNoteTypeInstruction],[ApprNoteMeasurement],[ApprNoteSet],[ApprNoteStep],[ApprNotePrompt],[ApprNoteLessonProc],[deletessn],(select isnull( max(LessonOrder)+1,1) from dstemphdr where studentid=" + studentid + ")," +
               "(Select DISTINCT EffStartDate  from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.StudentId = " + studentid + " AND StdtIEP.StatusId=65) ," +
                "(Select DISTINCT EffEndDate  from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.StudentId = " + studentid + " AND StdtIEP.StatusId=65) FROM DSTempHdr WHERE DSTempHdrId='" + templateid + "'";
            if (schoolid == 2)
                strQuery = "INSERT INTO DSTempHdr ([SchoolId]" + stid + "[LessonPlanId],[TeachingProcId],[DSTemplateName]," +
                  "[DSTemplateDesc],[VerBeginDate],[NoofTimesTried],[NoofTimesTriedPer],[VerEndDate],[CurrVerInd],[MultiSetsInd],[MultiStepInd],[SkillType],[MatchToSampleType],[NbrOfTrials]," +
                  "[ChainType],[TotalTaskFormat],[TotalTaskType],[TaskOther],[MatchToSampleRecOrExp],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd],[StatusId],[IsVisualTool]," +
                  "[VTLessonId],[Baseline],[Objective],[GeneralProcedure],[BaselineProc],[BaselineStart],[BaselineEnd],[CorrRespDef]," +
                  "[CorrectResponse],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef],[CorrectionProc],[ReinforcementProc]," +
                  "[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse]," +
                  "[TeacherPrepare],[StudentPrepare],[StudResponse],[DSMode],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]," +
                  "[FrameandStrand],[LessonPlanGoal],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[ApprNoteLessonInfo],[ApprNoteTypeInstruction],[ApprNoteMeasurement],[ApprNoteSet],[ApprNoteStep],[ApprNotePrompt],[ApprNoteLessonProc],[deletessn],[LessonOrder],[LessonSDate],[LessonEDate]) SELECT [SchoolId]" + stval + "[LessonPlanId]," +
                  "[TeachingProcId],[DSTemplateName],[DSTemplateDesc],[VerBeginDate],[NoofTimesTried],[NoofTimesTriedPer],[VerEndDate],[CurrVerInd],[MultiSetsInd],[MultiStepInd]," +
                  "[SkillType],[MatchToSampleType],[NbrOfTrials],[ChainType],[TotalTaskFormat],[TotalTaskType],[TaskOther],[MatchToSampleRecOrExp],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd]," +
                  "(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress'),[IsVisualTool]," +
                  "'" + visualLessonId + "',[Baseline],[Objective],[GeneralProcedure],[BaselineProc],[BaselineStart],[BaselineEnd]," +
                  "[CorrRespDef],[CorrectResponse],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef],[CorrectionProc],[ReinforcementProc]," +
                  "[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse]," +
                  "[TeacherPrepare],[StudentPrepare],[StudResponse],[DSMode]," + loginid + ",GETDATE()," + loginid + ",GETDATE(),[FrameandStrand],[LessonPlanGoal]," +
                  "[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[ApprNoteLessonInfo],[ApprNoteTypeInstruction],[ApprNoteMeasurement],[ApprNoteSet],[ApprNoteStep],[ApprNotePrompt],[ApprNoteLessonProc],[deletessn],(select isnull( max(LessonOrder)+1,1) from dstemphdr where studentid=" + studentid + ")," +
                  "(Select DISTINCT EffStartDate  from StDtLessonPlan inner join StDtIEP_PE on StDtLessonPlan.StDtIEPId=StDtIEP_PE.StDtIEP_PEId where StDtLessonPlan.StudentId = " + studentid + " AND StdtIEP_PE.StatusId=65) ," +
                   "(Select DISTINCT EffEndDate  from StDtLessonPlan inner join StDtIEP_PE on StDtLessonPlan.StDtIEPId=StDtIEP_PE.StDtIEP_PEId where StDtLessonPlan.StudentId = " + studentid + " AND StdtIEP_PE.StatusId=65) FROM DSTempHdr WHERE DSTempHdrId='" + templateid + "'";
            int TId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

            DataTable dtpromt = new DataTable();
            dtpromt = objData.ReturnDataTable("SELECT DSTempPromptId FROM DSTempPrompt WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtpromt != null)
            {
                if (dtpromt.Rows.Count > 0)
                {
                    foreach (DataRow row in dtpromt.Rows)
                    {
                        strQuery = "INSERT INTO DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT " + TId + ",PromptId,PromptOrder,ActiveInd," + loginid + ",CreatedOn FROM DSTempPrompt WHERE DSTempPromptId=" + Convert.ToInt32(row["DSTempPromptId"]) + "";
                        int PromptId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            DataTable dtset = new DataTable();
            Hashtable ht = new Hashtable();
            dtset = objData.ReturnDataTable("SELECT DSTempSetId FROM DSTempSet WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtset != null)
            {
                if (dtset.Rows.Count > 0)
                {
                    foreach (DataRow row in dtset.Rows)
                    {
                        strQuery = "INSERT INTO DSTempSet(SchoolId,DSTempHdrId,PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT  SchoolId," + TId + ",PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd," + loginid + ",getdate() FROM DSTempSet WHERE ActiveInd='A' AND DSTempSetId = " + Convert.ToInt32(row["DSTempSetId"]) + " ";
                        int SetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        if (!ht.ContainsKey(row["DSTempSetId"]))
                        {
                            ht.Add(row["DSTempSetId"], SetId);
                        }
                    }
                }
            }
            string teachingProc = "";
            string sqlStr = "";
            sqlStr = "SELECT DH.LessonPlanId,ISNULL(LU.LookupName,'') AS TeachingProc,ISNULL(LU.LookupDesc,'') AS TeachProc,ISNULL(LUp.LookupName,'') as PromptProc ,SkillType,ISNULL(NbrOfTrials,0) as NbrOfTrials," +
                    "LP.LessonPlanName,ISNULL(LP.Materials,'') as Mat,ISNULL(ChainType,'') AS ChainType,DH.IsVisualTool,ISNULL(DH.VTLessonId,0) as VTLessonId,ISNULL(ModificationInd,0) as ModificationInd FROM DSTempHdr DH JOIN LessonPlan LP ON LP.LessonPlanId=DH.LessonPlanId LEFT " +
                    "JOIN LookUp LU ON TeachingProcId=LU.LookUpId INNER JOIN Lookup LUp ON LUp.LookupId=PromptTypeId WHERE DSTempHdrId=" + templateid;
            DataTable dtTmpHdrDtls = objData.ReturnDataTable(sqlStr, false);
            if (dtTmpHdrDtls != null)
            {
                if (dtTmpHdrDtls.Rows.Count > 0)
                {
                    teachingProc = dtTmpHdrDtls.Rows[0]["TeachProc"].ToString();
                }
            }
            if (teachingProc == "Match-to-Sample")
            {
                DataTable dtstep = new DataTable();
                dtstep = objData.ReturnDataTable("SELECT DSTempStepId,DSTempSetId FROM DSTempStep WHERE DSTempHdrId=" + templateid + " AND ActiveInd='A' AND IsDynamic=0", Con, Trans, false);
                if (dtstep.Rows.Count > 0)
                {
                    foreach (DataRow row in dtstep.Rows)
                    {
                        oldSetId = Convert.ToInt32(row["DSTempSetId"]);
                        if (oldSetId != 0)
                        {
                            parentSetId = AssignLP.SetUpdateCopy(oldSetId, TId, Trans, Con);
                        }
                        strQuery =
                        strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                        strQuery += "SELECT SchoolId," + TId + "," + parentSetId + ",PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()	FROM DSTempStep WHERE DSTempStepId = " + Convert.ToInt32(row["DSTempStepId"]) + " AND ActiveInd='A' AND IsDynamic=0 ";
                        int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            else
            {
                int oldParentSetId = 0;
                DataTable dtParentStep = new DataTable();
                strQuery = "SELECT  DSTempParentStepId,SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn"
                    + " FROM DSTempParentStep WHERE ActiveInd='A' AND DSTempHdrId = " + templateid;
                dtParentStep = objData.ReturnDataTable(strQuery, Con, Trans, false);
                if (dtParentStep != null)
                {
                    if (dtParentStep.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtParentStep.Rows)
                        {
                            string newsetids = "";
                            foreach (string setid in row["SetIds"].ToString().Split(','))
                            {
                                if (setid != "")
                                {
                                    if (ht.ContainsKey(Convert.ToInt32(setid)))
                                    {
                                        newsetids += ht[Convert.ToInt32(setid)] + ",";
                                    }
                                }
                            }
                            oldParentSetId = Convert.ToInt32(row["DSTempParentStepId"]);
                            strQuery = "INSERT INTO DSTempParentStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn) "
                                        + "SELECT  SchoolId," + TId + ",StepCd,StepName,DSTempSetId,SortOrder,'" + newsetids + "',SetNames,ActiveInd," + loginid + ",getdate()"
                                        + " FROM DSTempParentStep WHERE ActiveInd='A' AND DSTempHdrId = " + templateid + " AND DSTempParentStepId=" + oldParentSetId;
                            parentSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                            DataTable dtstep = new DataTable();

                            strQuery = "SELECT  SchoolId,PrevStepId,SortOrder,PreDefinedInd,CustomById,VTStepId,DSTempSetId,StepCd,StepName,ActiveInd,"
                                    + "DSTempParentStepId FROM DSTempStep WHERE DSTempParentStepId=" + oldParentSetId + " AND IsDynamic=0 AND ActiveInd='A' AND DSTempHdrId = " + templateid;
                            dtstep = objData.ReturnDataTable(strQuery, Con, Trans, false);
                            if (dtstep.Rows.Count > 0)
                            {
                                foreach (DataRow rows in dtstep.Rows)
                                {
                                    oldSetId = Convert.ToInt32(rows["DSTempSetId"]);

                                    strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                                    strQuery += "SELECT SchoolId," + TId + ",DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()"
                                        + "	FROM DSTempStep WHERE DSTempSetId = " + oldSetId + " AND IsDynamic=0 AND DSTempParentStepId=" + oldParentSetId + " AND ActiveInd='A' AND DSTempHdrId = " + templateid;
                                    int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    strQuery = "SELECT DSTempSetId FROM DSTempStep WHERE DSTempStepId=" + StepId + " AND ActiveInd='A' AND IsDynamic=0";
                                    int NewSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    if (ht.ContainsKey(Convert.ToInt32(NewSetId)))
                                    {
                                        newsetids = ht[Convert.ToInt32(NewSetId)].ToString();
                                        strQuery = "UPDATE DSTempStep SET DSTempSetId=" + Convert.ToInt32(ht[Convert.ToInt32(NewSetId)]) + ",DSTempParentStepId=" + parentSetId + " "
                                            + " WHERE DSTempStepId=" + StepId + " AND IsDynamic=0";
                                        int updateId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            DataTable dtsetcol = new DataTable();
            dtsetcol = objData.ReturnDataTable("SELECT DSTempSetColId FROM DSTempSetCol WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtsetcol != null)
            {
                if (dtsetcol.Rows.Count > 0)
                {
                    foreach (DataRow row in dtsetcol.Rows)
                    {
                        strQuery = "INSERT INTO DSTempSetCol(SchoolId, DSTempHdrId,ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp	,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT SchoolId, " + TId + ",ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd," + loginid + ",CreatedOn FROM DSTempSetCol WHERE ActiveInd='A' AND DSTempSetColId = " + Convert.ToInt32(row["DSTempSetColId"]) + " ";
                        int setColNewId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        DataTable dtsetcolcalc = new DataTable();
                        dtsetcolcalc = objData.ReturnDataTable("SELECT DSTempSetColCalcId FROM DSTempSetColCalc WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + "", Con, Trans, false);
                        if (dtsetcolcalc.Rows.Count > 0)
                        {
                            foreach (DataRow rowc in dtsetcolcalc.Rows)
                            {
                                strQuery = "INSERT INTO DSTempSetColCalc(SchoolId,DSTempSetColId,CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd,CreatedBy,CreatedOn,IncludeInGraph) " +
                                            "SELECT SchoolId," + setColNewId + ",CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd," + loginid + ",getdate(),IncludeInGraph FROM DSTempSetColCalc WHERE DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + "";
                                int setColCalId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                                strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) ";
                                strQuery += "SELECT  " + TId + ",SchoolId," + setColNewId + "," + setColCalId + ",RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + " And DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + " ";
                                int lastId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                            }
                        }
                    }
                    strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) ";
                    strQuery += "SELECT  " + TId + ",SchoolId,0,0,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,"
                        + "LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE" +
                        " DSTempSetColId=0 And DSTempSetColCalcId=0 AND DSTempHdrId=" + templateid;
                    int lastModRuleId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                }
            }
            objData.CommitTransation(Trans, Con);
            return TId;
        }
        catch (Exception Ex)
        {
            objData.RollBackTransation(Trans, Con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            return 0;
        }
    }
    protected void CreateDocument(int tempid, int newtempId)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataTable dtdoc = new DataTable();
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            dtdoc = objData.ReturnDataTable("SELECT LPDoc FROM LPDoc WHERE DSTempHdrId=" + tempid + "", false);
            if (dtdoc != null)
            {
                if (dtdoc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtdoc.Rows)
                    {
                        string strquerry = "INSERT INTO LPDoc(SchoolId,DSTempHdrId,DocURL,CreatedBy,CreatedOn) SELECT SchoolId," + newtempId + ",DocURL," + sess.LoginId + ",GETDATE() FROM LPDoc WHERE LPDoc='" + row["LPDoc"].ToString() + "'";
                        int docid = objData.ExecuteWithScope(strquerry);
                        string binarydata = "SELECT Data,DocumentName FROM binaryFiles WHERE DocId=" + row["LPDoc"].ToString() + " AND type='LP_DOC' ";
                        DataTable dtbinary = objData.ReturnDataTable(binarydata, false);
                        byte[] myData = (byte[])dtbinary.Rows[0]["Data"];
                        string filename = Convert.ToString(dtbinary.Rows[0]["DocumentName"]);
                        int binaryid = objBinary.saveDocument(myData, filename, "", "LP_DOC", docid, "LessonPlanDoc", sess.SchoolId, 0, sess.LoginId);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GridView_PreRender(object sender, EventArgs e)
    {
        try
        {
            GridViewRow LastRow = GrdOverview.Rows[GrdOverview.Rows.Count - 1];
            LastRow.Font.Bold = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnUploadJson_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        SqlTransaction Trans = null;
        string jsonstore = "";
        if (!chkTemplate.Checked)
        {
            if (txtIndividualName.Value.ToString() == "" || hfSelectedStudentId.Value.ToString()=="")
            {
                ScriptManager.RegisterStartupScript(
    this,
    this.GetType(),
    "studentvalidation",
    "alert('Please selelct student');",
    true);
                return;
            }
        }
        if (hdnPopupValue.Value == "duplicatevalidation")
        {
            jsonstore = Session["JsonPath"] as string;

        }
        else
        {
            if (!fileJsonUpload.HasFile)
            {
                ScriptManager.RegisterStartupScript(
    this,
    this.GetType(),
    "select",
    "alert('Please selectt a JSON file');",
    true);
                return;
            }
        }

        try
        {
            string jsonString = "";
            if (hdnPopupValue.Value == "duplicatevalidation")
            {
                jsonString = jsonstore;
            }
            else
            {
                using (StreamReader reader =
                   new StreamReader(fileJsonUpload.FileContent))
                {
                    jsonString = reader.ReadToEnd();
                }
            }


            DataSet importSet =
                Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(jsonString);

            if (importSet == null)
            {
                lblUploadStatus.Text = "Invalid JSON file.";
                ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "popup",
            "showExportPopup();",
            true
        );
                return;
            }
            DataTable lookupmapdt =
       GetTableOrNull(importSet, "LookupIdMapping");

            string lidmatch = "SELECT LookupId,LookupName,LookupCode,LookupDesc from Lookup where LookupType = 'Datasheet-Teaching Procedures' AND ActiveInd = 'A'";
            DataTable lookupidmatch = objData.ReturnDataTable(lidmatch, false);

            Dictionary<string, int> lookupDictionary =
new Dictionary<string, int>();


            foreach (DataRow row in lookupidmatch.Rows)
            {
                string key =
                    row["LookupName"].ToString().Trim() + "|" +
                    row["LookupCode"].ToString().Trim() + "|" +
                    row["LookupDesc"].ToString().Trim();


                if (!lookupDictionary.ContainsKey(key))
                {
                    lookupDictionary.Add(
                        key,
                        Convert.ToInt32(row["LookupId"])
                    );
                }
            }



            foreach (DataRow mapRow in lookupmapdt.Rows)
            {
                string key =
                    mapRow["lname"].ToString().Trim() + "|" +
                    mapRow["lcode"].ToString().Trim() + "|" +
                    mapRow["ldes"].ToString().Trim();


                if (lookupDictionary.ContainsKey(key))
                {
                    mapRow["paid"] =
                    lookupDictionary[key];
                }
            }


            DataTable lookupprocdt =
                GetTableOrNull(importSet, "LookupProcIdMapping");



            string lidprocmatch = "SELECT LookupId,LookupName,LookupCode,LookupDesc from Lookup where LookupType = 'Datasheet-Prompt Procedures' AND ActiveInd = 'A'";
            DataTable lookupidprocmatch = objData.ReturnDataTable(lidprocmatch, false);

            Dictionary<string, int> lookupprocDictionary =
new Dictionary<string, int>();


            foreach (DataRow row in lookupidprocmatch.Rows)
            {
                string key =
                    row["LookupName"].ToString().Trim() + "|" +
                    row["LookupCode"].ToString().Trim() + "|" +
                    row["LookupDesc"].ToString().Trim();


                if (!lookupprocDictionary.ContainsKey(key))
                {
                    lookupprocDictionary.Add(
                        key,
                        Convert.ToInt32(row["LookupId"])
                    );
                }
            }



            foreach (DataRow mapRow in lookupprocdt.Rows)
            {
                string key =
                    mapRow["lname"].ToString().Trim() + "|" +
                    mapRow["lcode"].ToString().Trim() + "|" +
                    mapRow["ldes"].ToString().Trim();


                if (lookupprocDictionary.ContainsKey(key))
                {
                    mapRow["paid"] =
                    lookupprocDictionary[key];
                }
            }

            DataTable lookuppromptdt =
                GetTableOrNull(importSet, "LookupPrompt");


            string lidprompmatch = "SELECT LookupId,LookupName,LookupCode,LookupDesc from Lookup where LookupType = 'DSTempPrompt' AND ActiveInd = 'A'";
            DataTable lookupidprompmatch = objData.ReturnDataTable(lidprompmatch, false);

            Dictionary<string, int> lookupprompDictionary =
new Dictionary<string, int>();


            foreach (DataRow row in lookupidprompmatch.Rows)
            {
                string key =
                    row["LookupName"].ToString().Trim() + "|" +
                    row["LookupCode"].ToString().Trim() + "|" +
                    row["LookupDesc"].ToString().Trim();


                if (!lookupprompDictionary.ContainsKey(key))
                {
                    lookupprompDictionary.Add(
                        key,
                        Convert.ToInt32(row["LookupId"])
                    );
                }
            }



            foreach (DataRow mapRow in lookuppromptdt.Rows)
            {
                string key =
                    mapRow["lname"].ToString().Trim() + "|" +
                    mapRow["lcode"].ToString().Trim() + "|" +
                    mapRow["ldes"].ToString().Trim();


                if (lookupprompDictionary.ContainsKey(key))
                {
                    mapRow["paid"] =
                    lookupprompDictionary[key];
                }
            }


            DataTable lessgoal =
               GetTableOrNull(importSet, "LessonNameAndGoal");

            DataTable stdtGoalDt =
                GetTableOrNull(importSet, "StdtGoal");

            DataTable tempLessPlanDt =
                GetTableOrNull(importSet, "Temp_lessplan");

            DataTable goalLPRelDt =
                GetTableOrNull(importSet, "GoalLPRel");

            DataTable tempStdtLessPlanDt =
                GetTableOrNull(importSet, "Temp_stdtlessplan");

            DataTable tempDSTempHdrDt =
                GetTableOrNull(importSet, "Temp_DSTempHdr");

            DataTable tempDSTempPromptDt =
                GetTableOrNull(importSet, "Temp_DSTempPrompt");

            DataTable tempDSTempSetDt =
                GetTableOrNull(importSet, "Temp_DSTempSet");

            DataTable tempParentStepDt =
                GetTableOrNull(importSet, "Temp_DSTempParentStep");

            DataTable tempStepDt =
                GetTableOrNull(importSet, "Temp_DSTempStep");

            DataTable tempSetColDt =
                GetTableOrNull(importSet, "Temp_DSTempSetCol");

            DataTable tempSetColCalcDt =
                GetTableOrNull(importSet, "Temp_DSTempSetColCalc");

            DataTable tempRuleDt =
                GetTableOrNull(importSet, "Temp_DSTempRule");

            DataTable tempLPDocDt =
                GetTableOrNull(importSet, "Temp_LPDoc");

            DataTable tempBinaryFilesDt =
                GetTableOrNull(importSet, "Temp_binaryFiles");

            string lessname = "";
            if(chkTemplate.Checked)
            {
                hfSelectedStudentId.Value = "0";
            }

            if (hdnPopupValue.Value != "duplicatevalidation")
            {

                string result = string.Empty;

                string templateNames = "";

                if (!string.IsNullOrWhiteSpace(txtlessname.Value))
                {
                    // Single template name from textbox
                    templateNames = "'" + txtlessname.Value.Replace("'", "''") + "'";
                }
                else if (lessgoal != null)
                {
                    // Template names from DataTable
                    templateNames = string.Join(",",
                        lessgoal.AsEnumerable()
                            .Select(row => row.Field<string>("DSTemplateName"))
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => "'" + x.Replace("'", "''") + "'"));
                }

                string lessplanandgoal = "";
                if (hfSelectedStudentId.Value == "0")
                {
                     //lessplanandgoal = "SELECT  d.DSTemplateName, g.GoalName FROM DSTempHdr d INNER JOIN StdtLessonPlan s ON d.StdtLessonPlanId = s.StdtLessonPlanId INNER JOIN Goal g  ON s.GoalId = g.GoalId WHERE d.DSTemplateName in (" + templateNames + ") AND d.StudentId in (NULL) AND d.StatusId IN (Select LookupId from LookUp WHERE LookupType='TemplateStatus' And LookupName NOT IN('deleted'))";
                     lessplanandgoal= "SELECT DISTINCT HD.DSTemplateName, G.GoalName FROM DSTempHdr HD INNER JOIN GoalLPRel GR ON HD.LessonPlanId = GR.LessonPlanId INNER JOIN Goal G ON GR.GoalId = G.GoalId WHERE HD.DSTemplateName IN (" + templateNames + ") AND HD.StudentId IS NULL AND HD.StatusId <> (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Deleted') AND HD.IsDynamic=0 AND GR.ActiveInd='A'";
                }
                else
                {
                     lessplanandgoal = "SELECT  d.DSTemplateName, g.GoalName FROM DSTempHdr d INNER JOIN StdtLessonPlan s ON d.StdtLessonPlanId = s.StdtLessonPlanId INNER JOIN Goal g  ON s.GoalId = g.GoalId WHERE d.DSTemplateName in (" + templateNames + ") AND d.StudentId in (" + hfSelectedStudentId.Value + ") AND d.StatusId IN (Select LookupId from LookUp WHERE LookupType='TemplateStatus' And LookupName NOT IN('deleted'))";
                }
                    DataTable lessandgoaldt = objData.ReturnDataTable(lessplanandgoal, false);
                if (lessandgoaldt != null && lessandgoaldt.Rows.Count>0)
                {
                    DataTable lessgoal2 = lessgoal.Copy();
                    lessgoal.Merge(lessandgoaldt);
                    DataTable duplicateTable = lessgoal.Clone();

                    var duplicates = lessgoal.AsEnumerable()
                        .GroupBy(row => new
                        {
                            DSTemplateName = row["DSTemplateName"],
                            GoalName = row["GoalName"]
                        })
                        .Where(g => g.Count() > 1)
                        .SelectMany(g => g);

                    foreach (var row in duplicates)
                    {
                        duplicateTable.ImportRow(row);
                    }

                    if (lessgoal2 != null && lessgoal2.Rows.Count == 1)
                    {
                        String findlessgoal = "";
                        if (hfSelectedStudentId.Value == "0")
                        {
                            //findlessgoal = "SELECT  d.DSTemplateName, g.GoalName FROM DSTempHdr d INNER JOIN StdtLessonPlan s ON d.StdtLessonPlanId = s.StdtLessonPlanId INNER JOIN Goal g  ON s.GoalId = g.GoalId WHERE d.DSTemplateName in (" + templateNames + ") AND d.StudentId in ( NULL)  AND d.StatusId IN (Select LookupId from LookUp WHERE LookupType='TemplateStatus' And LookupName NOT IN('deleted'))";
                            findlessgoal = "SELECT DISTINCT HD.DSTemplateName, G.GoalName FROM DSTempHdr HD INNER JOIN GoalLPRel GR ON HD.LessonPlanId = GR.LessonPlanId INNER JOIN Goal G ON GR.GoalId = G.GoalId WHERE HD.DSTemplateName IN (" + templateNames + ") AND HD.StudentId IS NULL AND HD.StatusId <> (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Deleted') AND HD.IsDynamic=0 AND GR.ActiveInd='A'";

                        }
                        else
                        {
                            findlessgoal = "SELECT  d.DSTemplateName, g.GoalName FROM DSTempHdr d INNER JOIN StdtLessonPlan s ON d.StdtLessonPlanId = s.StdtLessonPlanId INNER JOIN Goal g  ON s.GoalId = g.GoalId WHERE d.DSTemplateName in (" + templateNames + ") AND d.StudentId in (" + hfSelectedStudentId.Value + ")  AND d.StatusId IN (Select LookupId from LookUp WHERE LookupType='TemplateStatus' And LookupName NOT IN('deleted'))";

                        }

                        DataTable findlessandgoaldt = objData.ReturnDataTable(findlessgoal, false);
                        if (findlessandgoaldt != null && findlessandgoaldt.Rows.Count>0)
                        {
                            lessgoal2.Merge(findlessandgoaldt);
                            DataTable duplicateTable2 = lessgoal.Clone();

                            var duplicates2 = lessgoal2.AsEnumerable()
                                .GroupBy(row => new
                                {
                                    DSTemplateName = row["DSTemplateName"],
                                    GoalName = row["GoalName"]
                                })
                                .Where(g => g.Count() > 1)
                                .SelectMany(g => g);


                            foreach (var row in duplicates2)
                            {
                                duplicateTable2.ImportRow(row);
                            }
                            if (duplicateTable2 != null && duplicateTable2.Rows.Count > 0)
                            {
                                //lblUploadStatus.Text = "Lesson Name Already Exist";
                                ScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "popup",
                                "alert('Lesson Name Already Exist');",
                                true);
                                hfSelectedStudentId.Value = "0";
                                return;

                            }
                            else
                            {
                                lessname = txtlessname.Value.ToString();

                            }
                        }

                    }

                    if (duplicateTable != null && duplicateTable.Rows.Count > 0)

                    {
                        StringBuilder sb = new StringBuilder();

                        var uniqueNames = duplicateTable.AsEnumerable()
                                        .Select(r => r["DSTemplateName"].ToString())
                                        .Distinct();

                        foreach (string name in uniqueNames)
                        {
                            sb.Append(name + "<br/>");
                        }

                        lesslist.InnerHtml = sb.ToString();

                        Session["JsonPath"] = jsonString;

                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "dupPopup", "showDuplicatePopup();", true);


                    }
                    else
                    {
                        if (chkTemplate.Checked)
                        {
                            hfSelectedStudentId.Value = "0";
                        }
                        if (txtlessname.Value.ToString() != "")
                        {
                            lessname = txtlessname.Value.ToString();
                        }
                        SqlConnection con = objData.Open();
                        clsData.blnTrans = true;
                        Trans = con.BeginTransaction();
                        if (!chkTemplate.Checked)
                        {

                            InsertStudentGoalsFromDataTable(stdtGoalDt, con, Trans);
                        }
                        lessonPlanMap = InsertLessonPlanTable(tempLessPlanDt, sess.SchoolId, lessname, con, Trans);
                        InsertGoalLPRelTable(goalLPRelDt, lessonPlanMap, con, Trans);

                        if (!chkTemplate.Checked)
                        {
                            stdtLessonPlanMap = InsertStdtLessonPlanTable(tempStdtLessPlanDt, lessonPlanMap, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), con, Trans);
                        }
                        hdrMap = InsertDSTempHdrTable(tempDSTempHdrDt, lessonPlanMap, stdtLessonPlanMap, lookupmapdt, lookupprocdt, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), lessname, con, Trans);

                        InsertDSTempPromptTable(tempDSTempPromptDt, hdrMap, lookuppromptdt, con, Trans);
                        setMap = InsertDSTempSetTable(tempDSTempSetDt, hdrMap, sess.SchoolId, con, Trans);
                        parentStepMap = InsertDSTempParentStepTable(tempParentStepDt, hdrMap, setMap, sess.SchoolId, con, Trans);
                        stepMap = InsertDSTempStepTable(tempStepDt, hdrMap, setMap, parentStepMap, sess.SchoolId, con, Trans);
                        setColMap = InsertDSTempSetColTable(tempSetColDt, hdrMap, sess.SchoolId, con, Trans);
                        setColCalcMap = InsertDSTempSetColCalcTable(tempSetColCalcDt, setColMap, sess.SchoolId, sess.SchoolId, con, Trans);
                        Dictionary<int, int> ruleMap = InsertDSTempRuleTable(tempRuleDt, hdrMap, setColMap, setColCalcMap, sess.SchoolId, con, Trans);
                        docMap = InsertLPDocTable(tempLPDocDt, hdrMap, sess.LoginId, sess.LoginId, con, Trans);
                        InsertBinaryFilesTable(tempBinaryFilesDt, docMap, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), con, Trans);
                        objData.CommitTransation(Trans, con);
                        ScriptManager.RegisterStartupScript(
       this,
       this.GetType(),
       "success2",
       "alert('Lessons imported successfully.');",
       true);
                        hfSelectedStudentId.Value = "0";
                    }
                }
                else
                {
                    if (chkTemplate.Checked)
                    {
                        hfSelectedStudentId.Value = "0";
                    }
                    if (txtlessname.Value.ToString() != "")
                    {
                        lessname = txtlessname.Value.ToString();
                    }
                    SqlConnection con = objData.Open();
                    clsData.blnTrans = true;
                    Trans = con.BeginTransaction();
                    if (!chkTemplate.Checked)
                    {

                        InsertStudentGoalsFromDataTable(stdtGoalDt, con, Trans);
                    }
                    lessonPlanMap = InsertLessonPlanTable(tempLessPlanDt, sess.SchoolId, lessname, con, Trans);
                    InsertGoalLPRelTable(goalLPRelDt, lessonPlanMap, con, Trans);

                    if (!chkTemplate.Checked)
                    {
                        stdtLessonPlanMap = InsertStdtLessonPlanTable(tempStdtLessPlanDt, lessonPlanMap, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), con, Trans);
                    }
                    hdrMap = InsertDSTempHdrTable(tempDSTempHdrDt, lessonPlanMap, stdtLessonPlanMap, lookupmapdt, lookupprocdt, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), lessname, con, Trans);
                    
                    InsertDSTempPromptTable(tempDSTempPromptDt, hdrMap, lookuppromptdt, con, Trans);
                    setMap = InsertDSTempSetTable(tempDSTempSetDt, hdrMap, sess.SchoolId, con, Trans);
                    parentStepMap = InsertDSTempParentStepTable(tempParentStepDt, hdrMap, setMap, sess.SchoolId, con, Trans);
                    stepMap = InsertDSTempStepTable(tempStepDt, hdrMap, setMap, parentStepMap, sess.SchoolId, con, Trans);
                    setColMap = InsertDSTempSetColTable(tempSetColDt, hdrMap, sess.SchoolId, con, Trans);
                    setColCalcMap = InsertDSTempSetColCalcTable(tempSetColCalcDt, setColMap, sess.SchoolId, sess.SchoolId, con, Trans);
                    Dictionary<int, int> ruleMap = InsertDSTempRuleTable(tempRuleDt, hdrMap, setColMap, setColCalcMap, sess.SchoolId, con, Trans);
                    docMap = InsertLPDocTable(tempLPDocDt, hdrMap, sess.LoginId, sess.LoginId, con, Trans);
                    InsertBinaryFilesTable(tempBinaryFilesDt, docMap, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), con, Trans);
                    objData.CommitTransation(Trans, con);
                    ScriptManager.RegisterStartupScript(
   this,
   this.GetType(),
   "success2",
   "alert('Lessons imported successfully.');",
   true);
                    hfSelectedStudentId.Value = "0";
                }
            }
            else
            {
                if (chkTemplate.Checked)
                {
                    hfSelectedStudentId.Value = "0";
                }
                SqlConnection con = objData.Open();
                clsData.blnTrans = true;
                Trans = con.BeginTransaction();
                if (!chkTemplate.Checked)
                {

                    InsertStudentGoalsFromDataTable(stdtGoalDt, con, Trans);
                }
                lessonPlanMap = InsertLessonPlanTable(tempLessPlanDt, sess.SchoolId,lessname, con, Trans);

                InsertGoalLPRelTable(goalLPRelDt, lessonPlanMap, con, Trans);

                if (!chkTemplate.Checked)
                {
                    stdtLessonPlanMap = InsertStdtLessonPlanTable(tempStdtLessPlanDt, lessonPlanMap, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), con, Trans);
                }
                hdrMap = InsertDSTempHdrTable(tempDSTempHdrDt, lessonPlanMap, stdtLessonPlanMap, lookupmapdt, lookupprocdt, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), lessname, con, Trans);
                InsertDSTempPromptTable(tempDSTempPromptDt, hdrMap, lookuppromptdt, con, Trans);
                setMap = InsertDSTempSetTable(tempDSTempSetDt, hdrMap, sess.SchoolId, con, Trans);
                parentStepMap = InsertDSTempParentStepTable(tempParentStepDt, hdrMap, setMap, sess.SchoolId, con, Trans);
                stepMap = InsertDSTempStepTable(tempStepDt, hdrMap, setMap, parentStepMap, sess.SchoolId, con, Trans);
                setColMap = InsertDSTempSetColTable(tempSetColDt, hdrMap, sess.SchoolId, con, Trans);
                setColCalcMap = InsertDSTempSetColCalcTable(tempSetColCalcDt, setColMap, sess.SchoolId, sess.SchoolId, con, Trans);
                Dictionary<int, int> ruleMap = InsertDSTempRuleTable(tempRuleDt, hdrMap, setColMap, setColCalcMap, sess.SchoolId, con, Trans);
                docMap = InsertLPDocTable(tempLPDocDt, hdrMap, sess.LoginId, sess.LoginId, con, Trans);
                InsertBinaryFilesTable(tempBinaryFilesDt, docMap, sess.SchoolId, Convert.ToInt32(hfSelectedStudentId.Value), con, Trans);
                objData.CommitTransation(Trans, con);
                            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "success",
                "alert('Lessons imported successfully.');",
                true);
                            hfSelectedStudentId.Value = "0";
                        }
                        ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "HideLoader2",
                "hideLoader();",
                true
                                    );


        }
        catch (Exception ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());

            Trans.Rollback();
            ScriptManager.RegisterStartupScript(
    this,
    this.GetType(),
    "popup",
    "alert('Import failed. Please try again.');",
    true);
            hfSelectedStudentId.Value = "0";
            return;
        }
    }


    public DataTable GetTableOrNull(DataSet ds, string tableName)
    {
        if (ds.Tables.Contains(tableName))
        {
            if (ds.Tables[tableName].Rows.Count > 0)
                return ds.Tables[tableName];
        }

        return null;
    }

    [System.Web.Services.WebMethod]
    public static List<object> GetIndividualNames(string searchText)
    {
        List<object> students = new List<object>();

        clsData objData = new clsData();

        string safeText =
            clsGeneral.convertQuotes(searchText.Trim());

        string query = @"
        SELECT TOP 50
            StudentPersonalId,
            StudentFname + ' ' + StudentLname AS FullName
        FROM Student
        WHERE
            StudentFname LIKE '%" + safeText + @"%'
            OR StudentLname LIKE '%" + safeText + @"%'
            OR (StudentFname + ' ' + StudentLname)
                LIKE '%" + safeText + @"%'
            OR (StudentLname + ' ' + StudentFname)
                LIKE '%" + safeText + @"%'";

        DataTable dt =
            objData.ReturnDataTable(query, false);

        if (dt == null || dt.Rows.Count == 0)
            return students;

        foreach (DataRow row in dt.Rows)
        {
            students.Add(new
            {
                Id = row["StudentPersonalId"].ToString(),
                Name = row["FullName"].ToString()
            });
        }

        return students;
    }

    public void InsertStudentGoalsFromDataTable(DataTable dtGoals, SqlConnection con, SqlTransaction trans)
    {
        try
        {
            if (dtGoals == null ||
                dtGoals.Rows.Count == 0)
                return;

            int value = Convert.ToInt32(hfSelectedStudentId.Value);

            int? studid = value == 0 ? (int?)null : value;


            SqlCommand asmntCmd =
            new SqlCommand(@"
        SELECT AsmntYearId
        FROM AsmntYear
        WHERE CurrentInd='A'",
            con, trans);

            int asmntYearId =
            Convert.ToInt32(
            asmntCmd.ExecuteScalar());



            SqlCommand statusCmd =
            new SqlCommand(@"
        SELECT LookupId
        FROM Lookup
        WHERE LookupType='Goal Status'
        AND LookupName='In Progress'",
            con, trans);

            int statusId =
            Convert.ToInt32(
            statusCmd.ExecuteScalar());



            SqlCommand iepCmd =
            new SqlCommand(@"
        SELECT ISNULL(MAX(IEPGoalNo),0)
        FROM StdtGoal
        WHERE StudentId=@studentId",
            con, trans);

            iepCmd.Parameters.Add("@studentId", SqlDbType.Int).Value =
    studid ?? (object)DBNull.Value;

            int nextIEPGoalNo =
            Convert.ToInt32(
            iepCmd.ExecuteScalar());



            foreach (DataRow row in dtGoals.Rows)
            {
                nextIEPGoalNo++;


                SqlCommand insertCmd =
                new SqlCommand(@"

            INSERT INTO StdtGoal
            (
                SchoolId,
                StudentId,
                GoalId,
                AsmntYearId,
                IncludeIEP,
                StatusId,
                ActiveInd,
                CreatedBy,
                CreatedOn,
                IEPGoalNo
            )

            VALUES
            (
                @SchoolId,
                @StudentId,
                @GoalId,
                @AsmntYearId,
                0,
                @StatusId,
                'A',
                @CreatedBy,
                GETDATE(),
                @IEPGoalNo
            )

            ", con, trans);


                insertCmd.Parameters.AddWithValue(
                "@SchoolId",
                sess.SchoolId);


                insertCmd.Parameters.Add("@StudentId", SqlDbType.Int).Value =
    studid ?? (object)DBNull.Value;

                insertCmd.Parameters.AddWithValue(
                "@GoalId",
                row["GoalId"]);


                insertCmd.Parameters.AddWithValue(
                "@AsmntYearId",
                asmntYearId);


                insertCmd.Parameters.AddWithValue(
                "@StatusId",
                statusId);


                insertCmd.Parameters.AddWithValue(
                "@CreatedBy",
                row["CreatedBy"]);


                insertCmd.Parameters.AddWithValue(
                "@IEPGoalNo",
                nextIEPGoalNo);


                insertCmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(
            "Error inserting Student Goals: " +
            ex.Message);
        }
    }



    public Dictionary<int, int> InsertLessonPlanTable(
  DataTable tempLessonPlanTable,
  int targetSchoolId, string lessname,
  SqlConnection con,
  SqlTransaction trans)
    {
        Dictionary<int, int> lessonPlanMap =
        new Dictionary<int, int>();


        if (tempLessonPlanTable == null ||
            tempLessonPlanTable.Rows.Count == 0)
            return lessonPlanMap;


        foreach (DataRow row in tempLessonPlanTable.Rows)
        {

            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO LessonPlan
        (
            SchoolId,
            LessonPlanName,
            PreReq,
            BaselineProc,
            Materials,
            FrameandStrand,
            SpecStandard,
            SpecEntryPoint,
            ActiveInd,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            Baseline,
            Objective,
            LessonSDate,
            LessonEDate
        )

        OUTPUT INSERTED.LessonPlanId

        VALUES
        (
            @SchoolId,
            @LessonPlanName,
            @PreReq,
            @BaselineProc,
            @Materials,
            @FrameandStrand,
            @SpecStandard,
            @SpecEntryPoint,
            @ActiveInd,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @Baseline,
            @Objective,
            @LessonSDate,
            @LessonEDate
        )",
            con,
            trans);


            cmd.Parameters.AddWithValue("@SchoolId", targetSchoolId);
            if(lessname=="")
            {
                cmd.Parameters.AddWithValue("@LessonPlanName", GetValue(row, "LessonPlanName"));

            }
            else
            {
                cmd.Parameters.AddWithValue("@LessonPlanName", lessname.Trim());
            }

            cmd.Parameters.AddWithValue("@PreReq", GetValue(row, "PreReq"));
            cmd.Parameters.AddWithValue("@BaselineProc", GetValue(row, "BaselineProc"));
            cmd.Parameters.AddWithValue("@Materials", GetValue(row, "Materials"));
            cmd.Parameters.AddWithValue("@FrameandStrand", GetValue(row, "FrameandStrand"));
            cmd.Parameters.AddWithValue("@SpecStandard", GetValue(row, "SpecStandard"));
            cmd.Parameters.AddWithValue("@SpecEntryPoint", GetValue(row, "SpecEntryPoint"));
            cmd.Parameters.AddWithValue("@ActiveInd", GetValue(row, "ActiveInd"));
            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);
            cmd.Parameters.AddWithValue("@Baseline", GetValue(row, "Baseline"));
            cmd.Parameters.AddWithValue("@Objective", GetValue(row, "Objective"));
            cmd.Parameters.AddWithValue("@LessonSDate", GetValue(row, "LessonSDate"));
            cmd.Parameters.AddWithValue("@LessonEDate", GetValue(row, "LessonEDate"));


            int newLessonPlanId =
            Convert.ToInt32(cmd.ExecuteScalar());

            int oldLessonPlanId =
            Convert.ToInt32(row["LessonPlanId"]);

            lessonPlanMap.Add(
            oldLessonPlanId,
            newLessonPlanId);
        }


        return lessonPlanMap;
    }
    private object GetValue(DataRow row, string columnName)
    {
        if (!row.Table.Columns.Contains(columnName))
            return DBNull.Value;

        if (row[columnName] == DBNull.Value)
            return DBNull.Value;

        return row[columnName];
    }
    public void InsertGoalLPRelTable(DataTable goalLPRelTable, Dictionary<int, int> lessonPlanMap, SqlConnection con, SqlTransaction trans)
    {

        if (goalLPRelTable == null ||
            goalLPRelTable.Rows.Count == 0)
            return;


        foreach (DataRow row in goalLPRelTable.Rows)
        {

            int oldLessonPlanId =
            Convert.ToInt32(row["LessonPlanId"]);

            if (!lessonPlanMap.ContainsKey(oldLessonPlanId))
                continue;


            int newLessonPlanId =
            lessonPlanMap[oldLessonPlanId];


            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO GoalLPRel
        (
            GoalId,
            LessonPlanId,
            ActiveInd,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn
        )

        VALUES
        (
            @GoalId,
            @LessonPlanId,
            @ActiveInd,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL
        )

        ",
            con,
            trans);


            cmd.Parameters.AddWithValue(
            "@GoalId",
            GetValue(row, "GoalId"));


            cmd.Parameters.AddWithValue(
            "@LessonPlanId",
            newLessonPlanId);


            cmd.Parameters.AddWithValue(
            "@ActiveInd",
            GetValue(row, "ActiveInd"));


            cmd.Parameters.AddWithValue(
            "@CreatedBy",
            sess.LoginId);



            cmd.ExecuteNonQuery();
        }
    }
    public Dictionary<int, int> InsertStdtLessonPlanTable(DataTable stdtLessonPlanTable, Dictionary<int, int> lessonPlanMap, int targetSchoolId, int StudentId, SqlConnection con, SqlTransaction trans)
    {
        int? targetStudentId = StudentId == 0 ? (int?)null : StudentId;
        Dictionary<int, int> stdtLessonPlanMap =
        new Dictionary<int, int>();

        if (stdtLessonPlanTable == null ||
            stdtLessonPlanTable.Rows.Count == 0)
            return stdtLessonPlanMap;



        SqlCommand asmntCmd =
        new SqlCommand(@"

        SELECT AsmntYearId
        FROM AsmntYear
        WHERE CurrentInd='A'

    ", con, trans);


        int asmntYearId =
        Convert.ToInt32(
        asmntCmd.ExecuteScalar());

        foreach (DataRow row
        in stdtLessonPlanTable.Rows)
        {

            int oldLessonPlanId =
            Convert.ToInt32(row["LessonPlanId"]);


            if (!lessonPlanMap.ContainsKey(oldLessonPlanId))
                continue;


            int newLessonPlanId =
            lessonPlanMap[oldLessonPlanId];


            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO StdtLessonPlan
        (
            SchoolId,
            StudentId,
            LessonPlanId,
            GoalId,
            AsmntYearId,
            IncludeIEP,
            ActiveInd,
            StatusId,
            LessonPlanTypeDay,
            LessonPlanTypeResi,
            CreatedBy,
            CreatedOn,
            isDynamic
        )

        OUTPUT INSERTED.StdtLessonPlanId

        VALUES
        (
            @SchoolId,
            @StudentId,
            @LessonPlanId,
            @GoalId,
            @AsmntYearId,
            @IncludeIEP,
            @ActiveInd,
            @StatusId,
            @LessonPlanTypeDay,
            @LessonPlanTypeResi,
            @CreatedBy,
            GETDATE(),
            @isDynamic
        )

        ", con, trans);


            cmd.Parameters.AddWithValue(
            "@SchoolId",
            targetSchoolId);


            cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value =
    targetStudentId ?? (object)DBNull.Value;


            cmd.Parameters.AddWithValue(
            "@LessonPlanId",
            newLessonPlanId);


            cmd.Parameters.AddWithValue(
            "@GoalId",
            GetValue(row, "GoalId"));


            cmd.Parameters.AddWithValue(
            "@AsmntYearId",
            asmntYearId);


            cmd.Parameters.AddWithValue(
            "@IncludeIEP",
            GetValue(row, "IncludeIEP"));


            cmd.Parameters.AddWithValue(
            "@ActiveInd",
            GetValue(row, "ActiveInd"));


            cmd.Parameters.AddWithValue(
            "@StatusId",
            GetValue(row, "StatusId"));


            cmd.Parameters.AddWithValue(
            "@LessonPlanTypeDay",
            GetValue(row, "LessonPlanTypeDay"));


            cmd.Parameters.AddWithValue(
            "@LessonPlanTypeResi",
            GetValue(row, "LessonPlanTypeResi"));


            cmd.Parameters.AddWithValue(
            "@CreatedBy",
            sess.LoginId);


            cmd.Parameters.AddWithValue(
            "@isDynamic",
            GetValue(row, "isDynamic"));


            int newStdtLessonPlanId =
            Convert.ToInt32(
            cmd.ExecuteScalar());


            int oldStdtLessonPlanId =
            Convert.ToInt32(
            row["StdtLessonPlanId"]);


            stdtLessonPlanMap.Add(
            oldStdtLessonPlanId,
            newStdtLessonPlanId);
        }


        return stdtLessonPlanMap;
    }
    public Dictionary<int, int> InsertDSTempHdrTable(DataTable dstHdrTable, Dictionary<int, int> lessonPlanMap, Dictionary<int, int> stdtLessonPlanMap, DataTable teachingProcMapTable, DataTable promptTypeMapTable, int targetSchoolId, int StudentId, string tempname,
     SqlConnection con,
     SqlTransaction trans)
    {
        int? targetStudentId = StudentId == 0 ? (int?)null : StudentId;

        Dictionary<int, int> dstHdrMap =
        new Dictionary<int, int>();


        if (dstHdrTable == null ||
            dstHdrTable.Rows.Count == 0)
            return dstHdrMap;




        SqlCommand statusCmd =
        new SqlCommand(@"
        SELECT LookupId
        FROM Lookup
        WHERE LookupType='TemplateStatus'
        AND LookupName='In Progress'
    ", con, trans);


        int statusId =
        Convert.ToInt32(statusCmd.ExecuteScalar());



        foreach (DataRow row in dstHdrTable.Rows)
        {

            int oldLessonPlanId =
            Convert.ToInt32(row["LessonPlanId"]);


            if (!lessonPlanMap.ContainsKey(oldLessonPlanId))
                continue;


            int newLessonPlanId =
            lessonPlanMap[oldLessonPlanId];



            object teachingProcId =
            GetMappedLookupValue(
            row,
            "TeachingProcId",
            teachingProcMapTable);



            object promptTypeId =
            GetMappedLookupValue(
            row,
            "PromptTypeId",
            promptTypeMapTable);



            object stdtLessonPlanId =
            GetMappedFKValue(
            row,
            "StdtLessonplanId",
            stdtLessonPlanMap);




            SqlCommand lessonOrderCmd =
            new SqlCommand(@"
            SELECT ISNULL(MAX(LessonOrder)+1,1)
            FROM DSTempHdr
            WHERE StudentId=@StudentId
        ", con, trans);


            lessonOrderCmd.Parameters.Add("@StudentId", SqlDbType.Int).Value =
     targetStudentId ?? (object)DBNull.Value;


            int lessonOrder =
            Convert.ToInt32(
            lessonOrderCmd.ExecuteScalar());




            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempHdr
        (
            SchoolId,
            StudentId,
            LessonPlanId,
            TeachingProcId,
            DSTemplateName,
            DSTemplateDesc,
            VerBeginDate,
            VerEndDate,
            CurrVerInd,
            MultiSetsInd,
            MultiStepInd,
            SkillType,
            NbrOfTrials,
            ChainType,
            PromptTypeId,
            TotNbrOfSessions,
            SessionFreq,
            NbrOfSession,
            CompCurrInd,
            StatusId,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            StdtLessonplanId,
            LessonOrder,
            IsVisualTool,
            VTLessonId,
            BaselineProc,
            BaselineStart,
            BaselineEnd,
            CorrRespDef,
            CorrectResponse,
            StudCorrRespDef,
            IncorrRespDef,
            StudIncorrRespDef,
            CorrectionProc,
            ReinforcementProc,
            TeacherRespReadness,
            StudentReadCrita,
            MajorSetting,
            MinorSetting,
            LessonDefInst,
            Mistrial,
            MistrialResponse,
            TeacherPrepare,
            StudentPrepare,
            StudResponse,
            Baseline,
            Objective,
            TotalTaskType,
            TaskOther,
            GeneralProcedure,
            MatchToSampleType,
            Materials,
            PreReq,
            SpecEntryPoint,
            SpecStandard,
            ApprNoteLessonProc,
            ApprNoteMeasurement,
            ApprNotePrompt,
            ApprNoteSet,
            ApprNoteStep,
            ApprNoteTypeInstruction,
            FrameandStrand,
            TotalTaskFormat,
            ApprNoteLessonInfo,
            LessonPlanGoal,
            MatchToSampleRecOrExp,
            NoofTimesTried,
            deletessn,
            LessonSDate,
            LessonEDate,
            NoofTimesTriedPer
        )

        OUTPUT INSERTED.DSTempHdrId

        VALUES
        (
            @SchoolId,
            @StudentId,
            @LessonPlanId,
            @TeachingProcId,
            @DSTemplateName,
            @DSTemplateDesc,
            @VerBeginDate,
            @VerEndDate,
            @CurrVerInd,
            @MultiSetsInd,
            @MultiStepInd,
            @SkillType,
            @NbrOfTrials,
            @ChainType,
            @PromptTypeId,
            @TotNbrOfSessions,
            @SessionFreq,
            @NbrOfSession,
            @CompCurrInd,
            @StatusId,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @StdtLessonplanId,
            @LessonOrder,
           @IsVisualTool,
@VTLessonId,
@BaselineProc,
@BaselineStart,
@BaselineEnd,
@CorrRespDef,
@CorrectResponse,
@StudCorrRespDef,
@IncorrRespDef,
@StudIncorrRespDef,
@CorrectionProc,
@ReinforcementProc,
@TeacherRespReadness,
@StudentReadCrita,
@MajorSetting,
@MinorSetting,
@LessonDefInst,
@Mistrial,
@MistrialResponse,
@TeacherPrepare,
@StudentPrepare,
@StudResponse,
@Baseline,
@Objective,
@TotalTaskType,
@TaskOther,
@GeneralProcedure,
@MatchToSampleType,
@Materials,
@PreReq,
@SpecEntryPoint,
@SpecStandard,
@ApprNoteLessonProc,
@ApprNoteMeasurement,
@ApprNotePrompt,
@ApprNoteSet,
@ApprNoteStep,
@ApprNoteTypeInstruction,
@FrameandStrand,
@TotalTaskFormat,
@ApprNoteLessonInfo,
@LessonPlanGoal,
@MatchToSampleRecOrExp,
@NoofTimesTried,
@deletessn,
@LessonSDate,
@LessonEDate,
@NoofTimesTriedPer
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId", targetSchoolId);
            cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value =
                targetStudentId ?? (object)DBNull.Value; cmd.Parameters.AddWithValue("@LessonPlanId", newLessonPlanId);
            cmd.Parameters.AddWithValue("@TeachingProcId", teachingProcId);
            if(tempname!="")
            {
                cmd.Parameters.AddWithValue("@DSTemplateName",
                            tempname.Trim());
            }
            else
            {
                cmd.Parameters.AddWithValue("@DSTemplateName",
                GetValue(row, "DSTemplateName"));

            }


            cmd.Parameters.AddWithValue("@DSTemplateDesc",
            GetValue(row, "DSTemplateDesc"));

            cmd.Parameters.AddWithValue("@VerBeginDate",
            GetValue(row, "VerBeginDate"));

            cmd.Parameters.AddWithValue("@VerEndDate",
            GetValue(row, "VerEndDate"));

            cmd.Parameters.AddWithValue("@CurrVerInd",
            GetValue(row, "CurrVerInd"));

            cmd.Parameters.AddWithValue("@MultiSetsInd",
            GetValue(row, "MultiSetsInd"));

            cmd.Parameters.AddWithValue("@MultiStepInd",
            GetValue(row, "MultiStepInd"));

            cmd.Parameters.AddWithValue("@SkillType",
            GetValue(row, "SkillType"));

            cmd.Parameters.AddWithValue("@NbrOfTrials",
            GetValue(row, "NbrOfTrials"));

            cmd.Parameters.AddWithValue("@ChainType",
            GetValue(row, "ChainType"));

            cmd.Parameters.AddWithValue("@PromptTypeId",
            promptTypeId);

            cmd.Parameters.AddWithValue("@TotNbrOfSessions",
            GetValue(row, "TotNbrOfSessions"));

            cmd.Parameters.AddWithValue("@SessionFreq",
            GetValue(row, "SessionFreq"));

            cmd.Parameters.AddWithValue("@NbrOfSession",
            GetValue(row, "NbrOfSession"));

            cmd.Parameters.AddWithValue("@CompCurrInd",
            GetValue(row, "CompCurrInd"));
            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);

            cmd.Parameters.AddWithValue("@StatusId",
            statusId);



            cmd.Parameters.AddWithValue("@StdtLessonplanId",
            stdtLessonPlanId);

            cmd.Parameters.AddWithValue("@LessonOrder",
            lessonOrder);

            cmd.Parameters.Add("@IsVisualTool", SqlDbType.Bit).Value = 0;
            cmd.Parameters.Add("@VTLessonId", SqlDbType.Bit).Value = 0;
            cmd.Parameters.AddWithValue("@BaselineProc", GetValue(row, "BaselineProc"));
            cmd.Parameters.AddWithValue("@BaselineStart", GetValue(row, "BaselineStart"));
            cmd.Parameters.AddWithValue("@BaselineEnd", GetValue(row, "BaselineEnd"));
            cmd.Parameters.AddWithValue("@CorrRespDef", GetValue(row, "CorrRespDef"));
            cmd.Parameters.AddWithValue("@CorrectResponse", GetValue(row, "CorrectResponse"));
            cmd.Parameters.AddWithValue("@StudCorrRespDef", GetValue(row, "StudCorrRespDef"));
            cmd.Parameters.AddWithValue("@IncorrRespDef", GetValue(row, "IncorrRespDef"));
            cmd.Parameters.AddWithValue("@StudIncorrRespDef", GetValue(row, "StudIncorrRespDef"));
            cmd.Parameters.AddWithValue("@CorrectionProc", GetValue(row, "CorrectionProc"));
            cmd.Parameters.AddWithValue("@ReinforcementProc", GetValue(row, "ReinforcementProc"));
            cmd.Parameters.AddWithValue("@TeacherRespReadness", GetValue(row, "TeacherRespReadness"));
            cmd.Parameters.AddWithValue("@StudentReadCrita", GetValue(row, "StudentReadCrita"));
            cmd.Parameters.AddWithValue("@MajorSetting", GetValue(row, "MajorSetting"));
            cmd.Parameters.AddWithValue("@MinorSetting", GetValue(row, "MinorSetting"));
            cmd.Parameters.AddWithValue("@LessonDefInst", GetValue(row, "LessonDefInst"));
            cmd.Parameters.AddWithValue("@Mistrial", GetValue(row, "Mistrial"));
            cmd.Parameters.AddWithValue("@MistrialResponse", GetValue(row, "MistrialResponse"));
            cmd.Parameters.AddWithValue("@TeacherPrepare", GetValue(row, "TeacherPrepare"));
            cmd.Parameters.AddWithValue("@StudentPrepare", GetValue(row, "StudentPrepare"));
            cmd.Parameters.AddWithValue("@StudResponse", GetValue(row, "StudResponse"));
            cmd.Parameters.AddWithValue("@Baseline", GetValue(row, "Baseline"));
            cmd.Parameters.AddWithValue("@Objective", GetValue(row, "Objective"));
            cmd.Parameters.AddWithValue("@TotalTaskType", GetValue(row, "TotalTaskType"));
            cmd.Parameters.AddWithValue("@TaskOther", GetValue(row, "TaskOther"));
            cmd.Parameters.AddWithValue("@GeneralProcedure", GetValue(row, "GeneralProcedure"));
            cmd.Parameters.AddWithValue("@MatchToSampleType", GetValue(row, "MatchToSampleType"));
            cmd.Parameters.AddWithValue("@Materials", GetValue(row, "Materials"));
            cmd.Parameters.AddWithValue("@PreReq", GetValue(row, "PreReq"));
            cmd.Parameters.AddWithValue("@SpecEntryPoint", GetValue(row, "SpecEntryPoint"));
            cmd.Parameters.AddWithValue("@SpecStandard", GetValue(row, "SpecStandard"));
            cmd.Parameters.AddWithValue("@ApprNoteLessonProc", GetValue(row, "ApprNoteLessonProc"));
            cmd.Parameters.AddWithValue("@ApprNoteMeasurement", GetValue(row, "ApprNoteMeasurement"));
            cmd.Parameters.AddWithValue("@ApprNotePrompt", GetValue(row, "ApprNotePrompt"));
            cmd.Parameters.AddWithValue("@ApprNoteSet", GetValue(row, "ApprNoteSet"));
            cmd.Parameters.AddWithValue("@ApprNoteStep", GetValue(row, "ApprNoteStep"));
            cmd.Parameters.AddWithValue("@ApprNoteTypeInstruction", GetValue(row, "ApprNoteTypeInstruction"));
            cmd.Parameters.AddWithValue("@FrameandStrand", GetValue(row, "FrameandStrand"));
            cmd.Parameters.AddWithValue("@TotalTaskFormat", GetValue(row, "TotalTaskFormat"));
            cmd.Parameters.AddWithValue("@ApprNoteLessonInfo", GetValue(row, "ApprNoteLessonInfo"));
            cmd.Parameters.AddWithValue("@LessonPlanGoal", GetValue(row, "LessonPlanGoal"));
            cmd.Parameters.AddWithValue("@MatchToSampleRecOrExp", GetValue(row, "MatchToSampleRecOrExp"));
            cmd.Parameters.AddWithValue("@NoofTimesTried", GetValue(row, "NoofTimesTried"));
            cmd.Parameters.AddWithValue("@deletessn", GetValue(row, "deletessn"));
            cmd.Parameters.AddWithValue("@LessonSDate", GetValue(row, "LessonSDate"));
            cmd.Parameters.AddWithValue("@LessonEDate", GetValue(row, "LessonEDate"));
            cmd.Parameters.AddWithValue("@NoofTimesTriedPer", GetValue(row, "NoofTimesTriedPer"));

            int newDstHdrId =
            Convert.ToInt32(
            cmd.ExecuteScalar());


            int oldDstHdrId =
            Convert.ToInt32(
            row["DSTempHdrId"]);



            dstHdrMap.Add(
            oldDstHdrId,
            newDstHdrId);
        }


        return dstHdrMap;
    }
    private object GetMappedLookupValue(DataRow row, string columnName, DataTable lookupTable)
    {
        if (!row.Table.Columns.Contains(columnName))
            return DBNull.Value;

        if (row[columnName] == DBNull.Value)
            return DBNull.Value;

        int oldId =
        Convert.ToInt32(row[columnName]);

        DataRow[] foundRows =
        lookupTable.Select("lid=" + oldId);

        if (foundRows.Length > 0)
            return foundRows[0]["paid"];

        return oldId;
    }
    private object GetMappedFKValue(DataRow row, string columnName, Dictionary<int, int> map)
    {
        if (!row.Table.Columns.Contains(columnName))
            return DBNull.Value;

        if (row[columnName] == DBNull.Value)
            return DBNull.Value;

        int oldId =
        Convert.ToInt32(row[columnName]);

        if (map.ContainsKey(oldId))
            return map[oldId];

        return DBNull.Value;
    }
   
    public void InsertDSTempPromptTable(DataTable dstPromptTable, Dictionary<int, int> dstHdrMap, DataTable promptTypeLookupTable, SqlConnection con, SqlTransaction trans)
    {

        if (dstPromptTable == null ||
            dstPromptTable.Rows.Count == 0)
            return;



        foreach (DataRow row in dstPromptTable.Rows)
        {

            int oldDstHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);



            if (!dstHdrMap.ContainsKey(oldDstHdrId))
                continue;



            int newDstHdrId =
            dstHdrMap[oldDstHdrId];



            object newPromptId =
            GetMappedLookupValue(
            row,
            "PromptId",
            promptTypeLookupTable);



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempPrompt
        (
            DSTempHdrId,
            PromptId,
            PromptOrder,
            ActiveInd,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn
        )

        VALUES
        (
            @DSTempHdrId,
            @PromptId,
            @PromptOrder,
            @ActiveInd,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL
        )

        ", con, trans);



            cmd.Parameters.AddWithValue(
            "@DSTempHdrId",
            newDstHdrId);

            cmd.Parameters.AddWithValue(
            "@CreatedBy",
            sess.LoginId);

            cmd.Parameters.AddWithValue(
            "@PromptId",
            newPromptId);



            cmd.Parameters.AddWithValue(
            "@PromptOrder",
            GetValue(row, "PromptOrder"));



            cmd.Parameters.AddWithValue(
            "@ActiveInd",
            GetValue(row, "ActiveInd"));

            cmd.ExecuteNonQuery();
        }
    }
    public Dictionary<int, int> InsertDSTempSetTable(DataTable dstSetTable, Dictionary<int, int> dstHdrMap, int targetSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> dstSetMap =
        new Dictionary<int, int>();


        if (dstSetTable == null ||
            dstSetTable.Rows.Count == 0)
            return dstSetMap;



        foreach (DataRow row in dstSetTable.Rows)
        {

            int oldHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);



            if (!dstHdrMap.ContainsKey(oldHdrId))
                continue;



            int newHdrId =
            dstHdrMap[oldHdrId];



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempSet
        (
            SchoolId,
            DSTempHdrId,
            PrevSetId,
            SetCd,
            SetName,
            Samples,
            SortOrder,
            ActiveInd,
            VTSetId,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            DistractorSamples,
            DistractorSamplesCount
        )

        OUTPUT INSERTED.DSTempSetId

        VALUES
        (
            @SchoolId,
            @DSTempHdrId,
            @PrevSetId,
            @SetCd,
            @SetName,
            @Samples,
            @SortOrder,
            @ActiveInd,
            @VTSetId,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @DistractorSamples,
            @DistractorSamplesCount
        )

        ", con, trans);



            cmd.Parameters.AddWithValue(
            "@SchoolId",
            targetSchoolId);



            cmd.Parameters.AddWithValue(
            "@DSTempHdrId",
            newHdrId);



            cmd.Parameters.AddWithValue(
            "@PrevSetId",
            GetValue(row, "PrevSetId"));



            cmd.Parameters.AddWithValue(
            "@SetCd",
            GetValue(row, "SetCd"));



            cmd.Parameters.AddWithValue(
            "@SetName",
            GetValue(row, "SetName"));



            cmd.Parameters.AddWithValue(
            "@Samples",
            GetValue(row, "Samples"));



            cmd.Parameters.AddWithValue(
            "@SortOrder",
            GetValue(row, "SortOrder"));



            cmd.Parameters.AddWithValue(
            "@ActiveInd",
            GetValue(row, "ActiveInd"));



            cmd.Parameters.AddWithValue(
            "@VTSetId",
            GetValue(row, "VTSetId"));




            cmd.Parameters.AddWithValue(
            "@CreatedBy", sess.LoginId);


            cmd.Parameters.AddWithValue(
            "@DistractorSamples",
            GetValue(row, "DistractorSamples"));



            cmd.Parameters.AddWithValue(
            "@DistractorSamplesCount",
            GetValue(row, "DistractorSamplesCount"));



            int newSetId =
            Convert.ToInt32(
            cmd.ExecuteScalar());



            int oldSetId =
            Convert.ToInt32(
            row["DSTempSetId"]);



            dstSetMap.Add(
            oldSetId,
            newSetId);
        }



        return dstSetMap;
    }
    public Dictionary<int, int> InsertDSTempParentStepTable(DataTable parentStepTable, Dictionary<int, int> dstHdrMap, Dictionary<int, int> dstSetMap, int targetSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> parentStepMap =
        new Dictionary<int, int>();


        if (parentStepTable == null ||
            parentStepTable.Rows.Count == 0)
            return parentStepMap;



        foreach (DataRow row in parentStepTable.Rows)
        {

            int oldHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);



            if (!dstHdrMap.ContainsKey(oldHdrId))
                continue;



            int newHdrId =
            dstHdrMap[oldHdrId];




            string mappedSetIds =
            ConvertSetIds(
            GetValue(row, "SetIds"),
            dstSetMap);



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempParentStep
        (
            SchoolId,
            DSTempHdrId,
            ActiveInd,
            StepCd,
            StepName,
            DSTempSetId,
            SortOrder,
            SetNames,
            SetIds,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn
        )

        OUTPUT INSERTED.DSTempParentStepId

        VALUES
        (
            @SchoolId,
            @DSTempHdrId,
            @ActiveInd,
            @StepCd,
            @StepName,
            @DSTempSetId,
            @SortOrder,
            @SetNames,
            @SetIds,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId", targetSchoolId);

            cmd.Parameters.AddWithValue("@DSTempHdrId", newHdrId);

            cmd.Parameters.AddWithValue("@ActiveInd",
            GetValue(row, "ActiveInd"));

            cmd.Parameters.AddWithValue("@StepCd",
            GetValue(row, "StepCd"));

            cmd.Parameters.AddWithValue("@StepName",
            GetValue(row, "StepName"));

            cmd.Parameters.AddWithValue("@DSTempSetId",
            GetValue(row, "DSTempSetId"));

            cmd.Parameters.AddWithValue("@SortOrder",
            GetValue(row, "SortOrder"));

            cmd.Parameters.AddWithValue("@SetNames",
            GetValue(row, "SetNames"));

            cmd.Parameters.AddWithValue("@SetIds",
            mappedSetIds);

            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);


            int newParentId =
            Convert.ToInt32(cmd.ExecuteScalar());



            int oldParentId =
            Convert.ToInt32(
            row["DSTempParentStepId"]);



            parentStepMap.Add(
            oldParentId,
            newParentId);
        }



        return parentStepMap;
    }
    private string ConvertSetIds(
object setIdsObj,
Dictionary<int, int> dstSetMap)
    {

        if (setIdsObj == DBNull.Value ||
            setIdsObj == null)
            return "";


        string setIds =
        setIdsObj.ToString().Trim();


        if (string.IsNullOrEmpty(setIds))
            return "";


        string[] parts =
        setIds.Split(',');


        List<string> mapped =
        new List<string>();


        foreach (string part in parts)
        {

            int oldId;

            if (int.TryParse(part, out oldId))
            {

                if (dstSetMap.ContainsKey(oldId))
                    mapped.Add(
                    dstSetMap[oldId].ToString());
            }
        }


        if (mapped.Count == 0)
            return "";


        return string.Join(",", mapped) + ",";
    }
    public Dictionary<int, int> InsertDSTempStepTable(DataTable stepTable, Dictionary<int, int> dstHdrMap, Dictionary<int, int> dstSetMap, Dictionary<int, int> parentStepMap, int targetSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> stepMap =
        new Dictionary<int, int>();


        if (stepTable == null ||
            stepTable.Rows.Count == 0)
            return stepMap;



        foreach (DataRow row in stepTable.Rows)
        {

            int oldHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);



            if (!dstHdrMap.ContainsKey(oldHdrId))
                continue;



            int newHdrId =
            dstHdrMap[oldHdrId];



            // MAP DSTempSetId

            object newSetId =
            MapSetId(
            row,
            "DSTempSetId",
            dstSetMap);



            // MAP ParentStepId

            object newParentStepId =
            MapParentStepId(
            row,
            "DSTempParentStepId",
            parentStepMap);



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempStep
        (
            SchoolId,
            DSTempHdrId,
            DSTempSetId,
            PrevStepId,
            StepCd,
            StepName,
            SortOrder,
            PreDefinedInd,
            DSTempParentStepId,
            CustomById,
            ActiveInd,
            VTStepId,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            IsDynamic
        )

        OUTPUT INSERTED.DSTempStepId

        VALUES
        (
            @SchoolId,
            @DSTempHdrId,
            @DSTempSetId,
            @PrevStepId,
            @StepCd,
            @StepName,
            @SortOrder,
            @PreDefinedInd,
            @DSTempParentStepId,
            @CustomById,
            @ActiveInd,
            @VTStepId,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @IsDynamic
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId", targetSchoolId);

            cmd.Parameters.AddWithValue("@DSTempHdrId", newHdrId);

            cmd.Parameters.AddWithValue("@DSTempSetId", newSetId);

            cmd.Parameters.AddWithValue("@PrevStepId",
            GetValue(row, "PrevStepId"));

            cmd.Parameters.AddWithValue("@StepCd",
            GetValue(row, "StepCd"));

            cmd.Parameters.AddWithValue("@StepName",
            GetValue(row, "StepName"));

            cmd.Parameters.AddWithValue("@SortOrder",
            GetValue(row, "SortOrder"));

            cmd.Parameters.AddWithValue("@PreDefinedInd",
            GetValue(row, "PreDefinedInd"));

            cmd.Parameters.AddWithValue("@DSTempParentStepId",
            newParentStepId);

            cmd.Parameters.AddWithValue("@CustomById",
            GetValue(row, "CustomById"));

            cmd.Parameters.AddWithValue("@ActiveInd",
            GetValue(row, "ActiveInd"));

            cmd.Parameters.AddWithValue("@VTStepId",
            GetValue(row, "VTStepId"));

            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);


            cmd.Parameters.AddWithValue("@IsDynamic",
            GetValue(row, "IsDynamic"));



            int newStepId =
            Convert.ToInt32(
            cmd.ExecuteScalar());



            int oldStepId =
            Convert.ToInt32(
            row["DSTempStepId"]);



            stepMap.Add(
            oldStepId,
            newStepId);
        }



        return stepMap;
    }
    private object MapSetId(DataRow row, string columnName, Dictionary<int, int> setMap)
    {

        if (!row.Table.Columns.Contains(columnName))
            return DBNull.Value;


        if (row[columnName] == DBNull.Value)
            return DBNull.Value;


        int oldId =
        Convert.ToInt32(row[columnName]);


        if (oldId == 0)
            return 0;


        if (setMap.ContainsKey(oldId))
            return setMap[oldId];


        return DBNull.Value;
    }
    private object MapParentStepId(DataRow row, string columnName, Dictionary<int, int> parentMap)
    {

        if (!row.Table.Columns.Contains(columnName))
            return DBNull.Value;


        if (row[columnName] == DBNull.Value)
            return DBNull.Value;


        int oldId =
        Convert.ToInt32(row[columnName]);


        if (parentMap.ContainsKey(oldId))
            return parentMap[oldId];


        return DBNull.Value;
    }
    public Dictionary<int, int> InsertDSTempSetColTable(DataTable setColTable, Dictionary<int, int> dstHdrMap, int targetSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> setColMap =
        new Dictionary<int, int>();


        if (setColTable == null ||
            setColTable.Rows.Count == 0)
            return setColMap;



        foreach (DataRow row in setColTable.Rows)
        {

            int oldHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);



            if (!dstHdrMap.ContainsKey(oldHdrId))
                continue;



            int newHdrId =
            dstHdrMap[oldHdrId];



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempSetCol
        (
            SchoolId,
            DSTempHdrId,
            ColName,
            ColTypeCd,
            CorrRespType,
            CorrResp,
            CorrRespDesc,
            InCorrRespDesc,
            CorrStdtResp,
            InCorrStdResp,
            IncPromptCriteria,
            IncMisTrialInd,
            MisTrialDesc,
            ActiveInd,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            CalcuData,
            CalcuType,
            MoveUpstat
        )

        OUTPUT INSERTED.DSTempSetColId

        VALUES
        (
            @SchoolId,
            @DSTempHdrId,
            @ColName,
            @ColTypeCd,
            @CorrRespType,
            @CorrResp,
            @CorrRespDesc,
            @InCorrRespDesc,
            @CorrStdtResp,
            @InCorrStdResp,
            @IncPromptCriteria,
            @IncMisTrialInd,
            @MisTrialDesc,
            @ActiveInd,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @CalcuData,
            @CalcuType,
            @MoveUpstat
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId", targetSchoolId);

            cmd.Parameters.AddWithValue("@DSTempHdrId", newHdrId);

            cmd.Parameters.AddWithValue("@ColName",
            GetValue(row, "ColName"));

            cmd.Parameters.AddWithValue("@ColTypeCd",
            GetValue(row, "ColTypeCd"));

            cmd.Parameters.AddWithValue("@CorrRespType",
            GetValue(row, "CorrRespType"));

            cmd.Parameters.AddWithValue("@CorrResp",
            GetValue(row, "CorrResp"));

            cmd.Parameters.AddWithValue("@CorrRespDesc",
            GetValue(row, "CorrRespDesc"));

            cmd.Parameters.AddWithValue("@InCorrRespDesc",
            GetValue(row, "InCorrRespDesc"));

            cmd.Parameters.AddWithValue("@CorrStdtResp",
            GetValue(row, "CorrStdtResp"));

            cmd.Parameters.AddWithValue("@InCorrStdResp",
            GetValue(row, "InCorrStdResp"));

            cmd.Parameters.AddWithValue("@IncPromptCriteria",
            GetValue(row, "IncPromptCriteria"));

            cmd.Parameters.AddWithValue("@IncMisTrialInd",
            GetValue(row, "IncMisTrialInd"));

            cmd.Parameters.AddWithValue("@MisTrialDesc",
            GetValue(row, "MisTrialDesc"));

            cmd.Parameters.AddWithValue("@ActiveInd",
            GetValue(row, "ActiveInd"));

            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);


            cmd.Parameters.AddWithValue("@CalcuData",
            GetValue(row, "CalcuData"));

            cmd.Parameters.AddWithValue("@CalcuType",
            GetValue(row, "CalcuType"));

            cmd.Parameters.AddWithValue("@MoveUpstat",
            GetValue(row, "MoveUpstat"));



            int newSetColId =
            Convert.ToInt32(
            cmd.ExecuteScalar());



            int oldSetColId =
            Convert.ToInt32(
            row["DSTempSetColId"]);



            setColMap.Add(
            oldSetColId,
            newSetColId);
        }



        return setColMap;
    }
    public Dictionary<int, int> InsertDSTempSetColCalcTable(DataTable tempSetColCalc, Dictionary<int, int> setColMap, int targetSchoolId, int currentSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> colCalcMap =
        new Dictionary<int, int>();


        if (tempSetColCalc == null ||
            tempSetColCalc.Rows.Count == 0)
            return colCalcMap;



        foreach (DataRow row in tempSetColCalc.Rows)
        {

           


            int oldSetColId =
            Convert.ToInt32(row["DSTempSetColId"]);



            int newSetColId = 0;


            if (oldSetColId != 0)
            {
                if (!setColMap.ContainsKey(oldSetColId))
                    continue;

                newSetColId =
                setColMap[oldSetColId];
            }



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempSetColCalc
        (
            SchoolId,
            DSTempSetColId,
            CalcType,
            CalcLabel,
            CalcFormula,
            CalcRptLabel,
            MaxLen,
            MaxVal,
            MinVal,
            ValText,
            ActiveInd,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            CalcuType,
            IncludeInGraph
        )

        OUTPUT INSERTED.DSTempSetColCalcId

        VALUES
        (
            @SchoolId,
            @DSTempSetColId,
            @CalcType,
            @CalcLabel,
            @CalcFormula,
            @CalcRptLabel,
            @MaxLen,
            @MaxVal,
            @MinVal,
            @ValText,
            @ActiveInd,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @CalcuType,
            @IncludeInGraph
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId",
            targetSchoolId);

            cmd.Parameters.AddWithValue("@DSTempSetColId",
            newSetColId);

            cmd.Parameters.AddWithValue("@CalcType",
            GetValue(row, "CalcType"));

            cmd.Parameters.AddWithValue("@CalcLabel",
            GetValue(row, "CalcLabel"));

            cmd.Parameters.AddWithValue("@CalcFormula",
            GetValue(row, "CalcFormula"));

            cmd.Parameters.AddWithValue("@CalcRptLabel",
            GetValue(row, "CalcRptLabel"));

            cmd.Parameters.AddWithValue("@MaxLen",
            GetValue(row, "MaxLen"));

            cmd.Parameters.AddWithValue("@MaxVal",
            GetValue(row, "MaxVal"));

            cmd.Parameters.AddWithValue("@MinVal",
            GetValue(row, "MinVal"));

            cmd.Parameters.AddWithValue("@ValText",
            GetValue(row, "ValText"));

            cmd.Parameters.AddWithValue("@ActiveInd",
            GetValue(row, "ActiveInd"));

            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);



            cmd.Parameters.AddWithValue("@CalcuType",
            GetValue(row, "CalcuType"));

            cmd.Parameters.AddWithValue("@IncludeInGraph",
            GetValue(row, "IncludeInGraph"));



            int newColCalcId =
            Convert.ToInt32(
            cmd.ExecuteScalar());



            int oldColCalcId =
            Convert.ToInt32(
            row["DSTempSetColCalcId"]);



            colCalcMap.Add(
            oldColCalcId,
            newColCalcId);

        }



        return colCalcMap;

    }
    public Dictionary<int, int> InsertDSTempRuleTable(DataTable tempRuleTable, Dictionary<int, int> dstHdrMap, Dictionary<int, int> setColMap, Dictionary<int, int> setColCalcMap, int targetSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> ruleMap =
        new Dictionary<int, int>();


        if (tempRuleTable == null ||
            tempRuleTable.Rows.Count == 0)
            return ruleMap;



        foreach (DataRow row in tempRuleTable.Rows)
        {

            if (row["ActiveInd"].ToString() != "A")
                continue;



            int oldHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);


            if (!dstHdrMap.ContainsKey(oldHdrId))
                continue;



            int newHdrId =
            dstHdrMap[oldHdrId];



            int oldSetColId =
            Convert.ToInt32(row["DSTempSetColId"]);


            int newSetColId = 0;


            if (oldSetColId != 0)
            {
                if (!setColMap.ContainsKey(oldSetColId))
                    continue;

                newSetColId =
                setColMap[oldSetColId];
            }



            int oldSetColCalcId =
            Convert.ToInt32(row["DSTempSetColCalcId"]);


            int newSetColCalcId = 0;


            if (oldSetColCalcId != 0)
            {
                if (!setColCalcMap.ContainsKey(oldSetColCalcId))
                    continue;

                newSetColCalcId =
                setColCalcMap[oldSetColCalcId];
            }



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO DSTempRule
        (
            SchoolId,
            DSTempHdrId,
            DSTempSetColId,
            DSTempSetColCalcId,
            RuleType,
            CriteriaType,
            ScoreReq,
            TotalInstance,
            TotCorrInstance,
            ConsequetiveInd,
            MultiTeacherReqInd,
            IOAReqInd,
            LogicalCombType,
            CriteriaDetails,
            ActiveInd,
            IsComment,
            ModificationComment,
            ModificationRule,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            IsNA,
            ConsequetiveAvgInd
        )

        OUTPUT INSERTED.DSTempRuleId

        VALUES
        (
            @SchoolId,
            @DSTempHdrId,
            @DSTempSetColId,
            @DSTempSetColCalcId,
            @RuleType,
            @CriteriaType,
            @ScoreReq,
            @TotalInstance,
            @TotCorrInstance,
            @ConsequetiveInd,
            @MultiTeacherReqInd,
            @IOAReqInd,
            @LogicalCombType,
            @CriteriaDetails,
            @ActiveInd,
            @IsComment,
            @ModificationComment,
            @ModificationRule,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL,
            @IsNA,
            @ConsequetiveAvgInd
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId",
            targetSchoolId);

            cmd.Parameters.AddWithValue("@DSTempHdrId",
            newHdrId);

            cmd.Parameters.AddWithValue("@DSTempSetColId",
            newSetColId);

            cmd.Parameters.AddWithValue("@DSTempSetColCalcId",
            newSetColCalcId);

            cmd.Parameters.AddWithValue("@RuleType",
            GetValue(row, "RuleType"));

            cmd.Parameters.AddWithValue("@CriteriaType",
            GetValue(row, "CriteriaType"));

            cmd.Parameters.AddWithValue("@ScoreReq",
            GetValue(row, "ScoreReq"));

            cmd.Parameters.AddWithValue("@TotalInstance",
            GetValue(row, "TotalInstance"));

            cmd.Parameters.AddWithValue("@TotCorrInstance",
            GetValue(row, "TotCorrInstance"));

            cmd.Parameters.AddWithValue("@ConsequetiveInd",
            GetValue(row, "ConsequetiveInd"));

            cmd.Parameters.AddWithValue("@MultiTeacherReqInd",
            GetValue(row, "MultiTeacherReqInd"));

            cmd.Parameters.AddWithValue("@IOAReqInd",
            GetValue(row, "IOAReqInd"));

            cmd.Parameters.AddWithValue("@LogicalCombType",
            GetValue(row, "LogicalCombType"));

            cmd.Parameters.AddWithValue("@CriteriaDetails",
            GetValue(row, "CriteriaDetails"));

            cmd.Parameters.AddWithValue("@ActiveInd",
            GetValue(row, "ActiveInd"));

            cmd.Parameters.AddWithValue("@IsComment",
            GetValue(row, "IsComment"));

            cmd.Parameters.AddWithValue("@ModificationComment",
            GetValue(row, "ModificationComment"));

            cmd.Parameters.AddWithValue("@ModificationRule",
            GetValue(row, "ModificationRule"));

            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);

            cmd.Parameters.AddWithValue("@IsNA",
            GetValue(row, "IsNA"));

            cmd.Parameters.AddWithValue("@ConsequetiveAvgInd",
            GetValue(row, "ConsequetiveAvgInd"));



            int newRuleId =
            Convert.ToInt32(
            cmd.ExecuteScalar());



            int oldRuleId =
            Convert.ToInt32(
            row["DSTempRuleId"]);



            ruleMap.Add(
            oldRuleId,
            newRuleId);

        }



        return ruleMap;

    }

    public Dictionary<int, int> InsertLPDocTable(DataTable tempLPDocTable, Dictionary<int, int> dstHdrMap, int targetSchoolId, int currentSchoolId, SqlConnection con, SqlTransaction trans)
    {

        Dictionary<int, int> lpDocMap =
        new Dictionary<int, int>();


        if (tempLPDocTable == null ||
            tempLPDocTable.Rows.Count == 0)
            return lpDocMap;



        foreach (DataRow row in tempLPDocTable.Rows)
        {



            if (Convert.ToInt32(row["SchoolId"])
            != currentSchoolId)
                continue;



            int oldHdrId =
            Convert.ToInt32(row["DSTempHdrId"]);




            if (!dstHdrMap.ContainsKey(oldHdrId))
                continue;



            int newHdrId =
            dstHdrMap[oldHdrId];



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO LPDoc
        (
            SchoolId,
            DSTempHdrId,
            DocURL,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn
        )

        OUTPUT INSERTED.LPDoc

        VALUES
        (
            @SchoolId,
            @DSTempHdrId,
            @DocURL,
            @CreatedBy,
            GETDATE(),
            NULL,
            NULL
        )

        ", con, trans);



            cmd.Parameters.AddWithValue("@SchoolId",
            targetSchoolId);

            cmd.Parameters.AddWithValue("@DSTempHdrId",
            newHdrId);

            cmd.Parameters.AddWithValue("@DocURL",
            GetValue(row, "DocURL"));

            cmd.Parameters.AddWithValue("@CreatedBy", sess.LoginId);


            int newDocId =
            Convert.ToInt32(
            cmd.ExecuteScalar());



            int oldDocId =
            Convert.ToInt32(
            row["LPDoc"]);



            lpDocMap.Add(
            oldDocId,
            newDocId);

        }

        return lpDocMap;

    }
    public void InsertBinaryFilesTable(DataTable binaryFilesTable, Dictionary<int, int> lpDocMap, int targetSchoolId, int StudentId, SqlConnection con, SqlTransaction trans)
    {
        int? targetStudentId = StudentId == 0 ? (int?)null : StudentId;


        if (binaryFilesTable == null ||
            binaryFilesTable.Rows.Count == 0)
            return;
        foreach (DataRow row in binaryFilesTable.Rows)
        {

            int oldDocId =
            Convert.ToInt32(row["DocId"]);



            if (!lpDocMap.ContainsKey(oldDocId))
                continue;

            int newDocId =
            lpDocMap[oldDocId];



            SqlCommand cmd =
            new SqlCommand(@"

        INSERT INTO binaryFiles
        (
            SchoolId,
            StudentId,
            DocId,
            IEPId,
            AllowParent,
            DocumentName,
            ContentType,
            Data,
            type,
            ModuleName,
            VersionNo,
            Varified,
            CreatedBy,
            CreatedOn,
            Active
        )

        VALUES
        (
            @SchoolId,
            @StudentId,
            @DocId,
            @IEPId,
            @AllowParent,
            @DocumentName,
            @ContentType,
            @Data,
            @type,
            @ModuleName,
            @VersionNo,
            @Varified,
            @CreatedBy,
            GETDATE(),
            @Active
        )

        ", con, trans);


           

            cmd.Parameters.AddWithValue("@SchoolId", targetSchoolId);

            cmd.Parameters.AddWithValue("@StudentId", StudentId);

            cmd.Parameters.AddWithValue("@DocId", newDocId);

            cmd.Parameters.AddWithValue("@IEPId",
                GetValue(row, "IEPId"));

            cmd.Parameters.AddWithValue("@AllowParent",
                GetValue(row, "AllowParent"));

            cmd.Parameters.AddWithValue("@DocumentName",
                GetValue(row, "DocumentName"));

            /* ContentType is varchar(MAX) */
            cmd.Parameters.AddWithValue("@ContentType",
                GetValue(row, "ContentType"));

            /* Data is varbinary(MAX) */
            if (row["Data"] != DBNull.Value &&
                !string.IsNullOrWhiteSpace(row["Data"].ToString()))
            {
                // if stored as base64 string
                byte[] fileBytes =
                    Convert.FromBase64String(row["Data"].ToString());

                cmd.Parameters.Add("@Data", SqlDbType.VarBinary).Value =
                    fileBytes;
            }
            else
            {
                cmd.Parameters.Add("@Data", SqlDbType.VarBinary).Value =
                    DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@type",
                GetValue(row, "type"));

            cmd.Parameters.AddWithValue("@ModuleName",
                GetValue(row, "ModuleName"));

            cmd.Parameters.AddWithValue("@VersionNo",
                GetValue(row, "VersionNo"));

            cmd.Parameters.AddWithValue("@Varified",
                GetValue(row, "Varified"));

            cmd.Parameters.AddWithValue("@CreatedBy",
                sess.LoginId);

            cmd.Parameters.AddWithValue("@Active",
                GetValue(row, "Active"));

            cmd.ExecuteNonQuery();


        }

    }

    protected void buttonexp_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        DataTable DtClient;
        string strData = "";
        string strCondition1 = "";
        string strCondition2 = "";
        int goalId = Convert.ToInt32(ddlGoal.SelectedValue);
        int LessonId = Convert.ToInt32(ddlLesson.SelectedValue);
        int StudentId = Convert.ToInt32(ddlClientName.SelectedValue);
        string LName = ddlLesson.SelectedItem.Text;
        string SearchCondition = txtLessonName.Text.Trim();
        //string IepYear = ddlIepYear.SelectedValue;
        string IepYear = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddlIepYear.Items)
        {
            if (item.Selected == true)
            {
                //if (item.Text.Equals("All"))
                //{
                //    if(ddlIepYear.Items.Count > 1)
                //    item.Selected = false;
                //    continue;
                //}
                IepYear += "'" + item.Text + "',";
            }
        }
        string LPStatus = "";
        ddlGoal.Width = 150;
        ddlLesson.Width = 150;
        ddlTeachingMethod.Width = 150;
        txtLessonName.Width = 200;
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Approved")
                {
                    LPStatus += "'Approved',";
                }
                else if (item.Text == "Pending Approval")
                {
                    LPStatus += "'Pending Approval',";
                }
                else if (item.Text == "In Progress")
                {
                    LPStatus += "'In Progress',";
                }
                else if (item.Text == "Maintenance")
                {
                    LPStatus += "'Maintenance',";
                }
                else if (item.Text == "Inactive")
                {
                    LPStatus += "'Inactive',";
                }
                else if (item.Text == "Rejected")
                {
                    LPStatus += "'Expired',";
                }

            }
        }
        if (LPStatus == "")
        {
            LPStatus = " 'Approved', 'Maintenance', 'Inactive' ";
        }
        List<string> removeStatuses = new List<string>
{
    "'Expired'",
    "'Pending Approval'",
    "'In Progress'"
};

        var statuses = LPStatus.TrimEnd(',')
                               .Split(',')
                               .Where(x => !removeStatuses.Contains(x.Trim()))
                               .ToList();

        LPStatus = string.Join(",", statuses);

        if (!string.IsNullOrEmpty(LPStatus))
        {
            LPStatus += ",";
        }
        if (LPStatus.Length > 1)
        {
            LPStatus = LPStatus.Substring(0, (LPStatus.Length - 1));
        }
        //strCondition1 = " AND LookupName IN (" + LPStatus + ") ";
        LPStatus = LPStatus.Trim(',');
        if (goalId > 0)
        {
            strCondition2 += " AND G.GoalId = " + goalId;
        }
        if (StudentId > 0)
        {
            strCondition2 += " AND DS.StudentId = " + StudentId;
        }
        if (LessonId != 0)
        {
            strCondition2 += " AND DS.DSTemplateName= '" + LName + "'";
        }
        if (SearchCondition != "")
        {
            strCondition2 += " AND DS.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
        }

        if (IepYear != null)
        {

            if (IepYear != "")
            {
                IepYear = IepYear.Substring(0, IepYear.Length - 1);

                strCondition2 += "AND YEAR(DS.LessonSDate) IN (" + IepYear + ")";
            }
        }
       string checkver = "SELECT DS.StudentId, SP.FirstName + ' ' + SP.LastName AS StudentName, " +
          "G.GoalId, G.GoalName, DS.LessonPlanId, " +
          "COUNT(DISTINCT DS.DSTempHdrId) AS DSTempHdrCount " +
          "FROM DSTempHdr DS " +
          "INNER JOIN GoalLPRel GLP ON GLP.LessonPlanId = DS.LessonPlanId " +
          "INNER JOIN Goal G ON G.GoalId = GLP.GoalId " +
          "INNER JOIN LookUp LU ON DS.StatusId = LU.LookupId " +
          "INNER JOIN StudentPersonal SP ON SP.StudentPersonalId = DS.StudentId " +
          "WHERE DS.StatusId IN ( " +
          "SELECT LookupId FROM LookUp " +
          "WHERE LookupType = 'TemplateStatus' " +
          "AND LookupName IN (" + LPStatus + ") ) " +
          "AND DS.TeachingProcId IN ( " +
          "SELECT LookupId FROM LookUp " +
          "WHERE LookupType = 'Datasheet-Teaching Procedures' " +
          "AND ParentLookupId IS NOT NULL " +
          "AND ActiveInd = 'A' ) " +
          strCondition2 + " " +
          "GROUP BY DS.StudentId, SP.FirstName, SP.LastName, " +
          "G.GoalId, G.GoalName, DS.LessonPlanId " +
          "HAVING COUNT(DISTINCT DS.DSTempHdrId) > 1 " +
          "ORDER BY DS.StudentId, DS.LessonPlanId";
        DataTable chkver = objData.ReturnDataTable(checkver, false);

        strData = "SELECT StudentId, LessonPlanId, DSTempHdrId, LessonName, StatusId, IEPSDate, IEPEDate, " +
                        "CASE WHEN LessonStatus = 'Expired' THEN 'Rejected' ELSE LessonStatus END AS LessonStatus " +
                        "FROM ( " +
                        "SELECT *, ROW_NUMBER() OVER (PARTITION BY StudentId, LessonPlanId ORDER BY CreatedOn DESC) AS RN " +
                        "FROM ( " +

                        "SELECT DISTINCT DS.StudentId, SP.FirstName+' '+SP.LastName AS StudentName, G.GoalId, G.GoalName, " +
                        "DS.LessonPlanId, DS.DSTempHdrId, DS.DSTemplateName AS LessonName, " +
                        "CONVERT(VARCHAR, DS.LessonSDate , 101) AS IEPSDate, " +
                        "CONVERT(VARCHAR, DS.LessonEDate , 101) AS IEPEDate, " +
                        "DS.StatusId, LU.LookupName AS LessonStatus, DS.CreatedOn " +

                        "FROM DSTempHdr DS " +
                        "INNER JOIN GoalLPRel GLP ON GLP.LessonPlanId = DS.LessonPlanId " +
                        "INNER JOIN Goal G ON G.GoalId= GLP.GoalId " +
                        "INNER JOIN LookUp LU ON DS.StatusId = LU.LookupId " +
                        "INNER JOIN StudentPersonal SP ON SP.StudentPersonalId=DS.StudentId " +

                        "WHERE DS.StatusId IN ( " +
                        "SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' " +
                        "AND LookupName IN (" + LPStatus + ") ) " +

                        "AND DS.TeachingProcId IN ( " +
                        "SELECT LookupId FROM LookUp WHERE LookupType='Datasheet-Teaching Procedures' " +
                        "AND ParentLookupId IS NOT NULL AND ActiveInd='A') " +

                        strCondition2 +

                        ") X ) LSN WHERE RN = 1 ORDER BY StudentId, LessonPlanId";
        DtClient = objData.ReturnDataTable(strData, false);

        if (DtClient != null && DtClient.Rows.Count > 0)
        {
            var finalList = new List<object>();

            string studentid = ""; string lessonplanid = ""; string dstemphdrid = "";

            foreach (DataRow row in DtClient.Rows)
            {
                studentid = studentid + "," + row["StudentId"].ToString();
                lessonplanid = lessonplanid + "," + row["LessonPlanId"].ToString();
                dstemphdrid = dstemphdrid + "," + row["DSTempHdrId"].ToString();
            }
            studentid = string.Join(",", studentid
     .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
     .Select(x => x.Trim())
     .Distinct());

            lessonplanid = string.Join(",", lessonplanid
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct());
            var ids = dstemphdrid
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            int count = ids.Count;

            dstemphdrid = string.Join(",", ids);
            exportparameter1.Value = studentid;
            exportparameter2.Value = dstemphdrid;
            exportparameter3.Value = lessonplanid;


            if (chkver != null && chkver.Rows.Count > 0)
            {
                string message = @"
                            if(confirm('You are about to export " + count + @" lessons. Some lessons have multiple versions, and only the latest version of each lesson will be exported. Do you want to continue?')) {
                                showLoader();
                                return true;
                            }
                            return false;";

                btnExport.OnClientClick = message;

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "clickExport",
                    "document.getElementById('" + btnExport.ClientID + "').click();",
                    true);

            }
            else
            {

                string message = @"
                            if(confirm('You are about to export " + count + @" lessons. Do you want to continue?')) {
                                showLoader();
                                return true;
                            }
                            return false;";

                btnExport.OnClientClick = message;

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "clickExport",
                    "document.getElementById('" + btnExport.ClientID + "').click();",
                    true);
            }
            //exportlessondata(studentid, dstemphdrid, lessonplanid, 1);


        }
        else
        {
            ScriptManager.RegisterStartupScript(
   this,
   this.GetType(),
   "nodata",
   "alert('No data available');",
   true);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "", "showLoader();", true);

        exportlessondata(exportparameter1.Value.ToString(), exportparameter2.Value.ToString(), exportparameter3.Value.ToString(), 1);
    }

}
