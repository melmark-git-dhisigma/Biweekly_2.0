using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;
using System.Net;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class Administration_AAa : System.Web.UI.Page
{
    clsSession sess=null;    
    clsData objData = null;
    DataClass oData = null;
    static clsData Dataobj = null;
    public static ClsTemplateSession ObjTempSess = null;

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

        if (!IsPostBack)
        {
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
                ddlTeachingMethod.Visible=true;
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
        if (DtLCount != null )
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
            foreach (TableCell headerCell in grdDatabankView.HeaderRow.Cells)
            {
                if (count < 3)
                {
                    //Font font = new Font();
                    //PdfPCell pdfCell = new PdfPCell(new Phrase(headerCell.Text, font));
                    //pdfTable.AddCell(pdfCell);

                    Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
                    pdfTable.AddCell(new PdfPCell(new Phrase(headerCell.Text, fontH1)));
                }
                count++;
            }
            foreach (GridViewRow gridViewRow in grdDatabankView.Rows)
            {
                int countCol = 0;   
                foreach (TableCell tableCell in gridViewRow.Cells)
                {
                    if (countCol < 3)
                    {
                        //Font font = new Font();
                        //PdfPCell pdfCell = new PdfPCell(new Phrase(tableCell.Text));
                        //pdfTable.AddCell(pdfCell);

                        string DatabankCopy = Server.HtmlDecode(tableCell.Text);
                        Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL);
                        pdfTable.AddCell(new PdfPCell(new Phrase(DatabankCopy, fontH1)));                        
                    }
                    countCol++;
                }
            }
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
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
            foreach (TableCell headerCell in grdClientView.HeaderRow.Cells)
            {
                if (count < 6)
                {
                    //Font font = new Font();
                    //PdfPCell pdfCell = new PdfPCell(new Phrase(headerCell.Text, font));
                    //pdfTable.AddCell(pdfCell);

                    Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
                    pdfTable.AddCell(new PdfPCell(new Phrase(headerCell.Text, fontH1)));
                }
                count++;
            }
            foreach (GridViewRow gridViewRow in grdClientView.Rows)
            {
                int countCol = 0;
                foreach (TableCell tableCell in gridViewRow.Cells)
                {
                    if (countCol < 6)
                    {
                        //Font font = new Font();
                        //PdfPCell pdfCell = new PdfPCell(new Phrase(tableCell.Text));
                        //pdfTable.AddCell(pdfCell);

                        string ClientCopy = Server.HtmlDecode(tableCell.Text);
                        Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL);
                        pdfTable.AddCell(new PdfPCell(new Phrase(ClientCopy, fontH1)));                         

                    }
                    countCol++;
                }
            }
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
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

    public override void VerifyRenderingInServerForm(Control control)
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "LoadLessonView(" + LessonId +", "+ GoalId + ");", true);
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
            DataTable dtdoc=null;
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
                        strQuery += "SELECT  SchoolId," + TId + ",PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd," + loginid + ",getdate() FROM DSTempSet WHERE DSTempSetId = " + Convert.ToInt32(row["DSTempSetId"]) + " ";
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
                dtstep = objData.ReturnDataTable("SELECT DSTempStepId,DSTempSetId FROM DSTempStep WHERE DSTempHdrId=" + templateid + " AND IsDynamic=0", Con, Trans, false);
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
                        strQuery += "SELECT SchoolId," + TId + "," + parentSetId + ",PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()	FROM DSTempStep WHERE DSTempStepId = " + Convert.ToInt32(row["DSTempStepId"]) + " AND IsDynamic=0 ";
                        int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            else
            {
                int oldParentSetId = 0;
                DataTable dtParentStep = new DataTable();
                strQuery = "SELECT  DSTempParentStepId,SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn"
                    + " FROM DSTempParentStep WHERE DSTempHdrId = " + templateid;
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
                                        + " FROM DSTempParentStep WHERE DSTempHdrId = " + templateid + " AND DSTempParentStepId=" + oldParentSetId;
                            parentSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                            DataTable dtstep = new DataTable();

                            strQuery = "SELECT  SchoolId,PrevStepId,SortOrder,PreDefinedInd,CustomById,VTStepId,DSTempSetId,StepCd,StepName,ActiveInd,"
                                    + "DSTempParentStepId FROM DSTempStep WHERE DSTempParentStepId=" + oldParentSetId + " AND IsDynamic=0 AND DSTempHdrId = " + templateid;
                            dtstep = objData.ReturnDataTable(strQuery, Con, Trans, false);
                            if (dtstep.Rows.Count > 0)
                            {
                                foreach (DataRow rows in dtstep.Rows)
                                {
                                    oldSetId = Convert.ToInt32(rows["DSTempSetId"]);

                                    strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                                    strQuery += "SELECT SchoolId," + TId + ",DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()"
                                        + "	FROM DSTempStep WHERE DSTempSetId = " + oldSetId + " AND IsDynamic=0 AND DSTempParentStepId=" + oldParentSetId + " AND DSTempHdrId = " + templateid;
                                    int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    strQuery = "SELECT DSTempSetId FROM DSTempStep WHERE DSTempStepId=" + StepId + " AND IsDynamic=0";
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
                        strQuery += "SELECT SchoolId, " + TId + ",ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd," + loginid + ",CreatedOn FROM DSTempSetCol WHERE DSTempSetColId = " + Convert.ToInt32(row["DSTempSetColId"]) + " ";
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
}
