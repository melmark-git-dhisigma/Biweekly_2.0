using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
public partial class StudentBinder_Event : System.Web.UI.Page
{
    clsData objData = null;
    string strQuery = "";
    static bool retVal = false;
    static int retValsave = 0;
    static bool Disable = false;
    clsSession sess = null;
    int srhck = 0;
    string query3 = "";
    

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
            LoadData();
            setVisible(false);

            Panel1.Visible = false;
            UpdatePanel1.Visible = true;
            btnSave.Visible = true;
        }

        lbledate.Visible = false;
        txtEdate.Visible = false;
    }
    private void setVisible(bool vis)
    {
        if (vis == true)
        {
            txtEdate.Attributes.Add("display", "block");
            lbledate.Attributes.Add("display", "block");
        }
        else
        {
            txtEdate.Attributes.Add("display", "none");
            lbledate.Attributes.Add("display", "none");
        }
      
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "disp();", true);
    }
    private void LoadData()
    {
        tdMsg.InnerHtml = "";
        bool Disable = false;
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            btnSave.Visible = false;
            if (grdGroup.Rows.Count > 0)
            {
                foreach (GridViewRow rows in grdGroup.Rows)
                {
                    ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                    lb_delete.Visible = false;
                    ImageButton lb_edit = ((ImageButton)rows.FindControl("lb_Edit"));
                    lb_edit.Visible = false;
                }
            }

        }
        else
        {
            btnSave.Visible = true;
            if (grdGroup.Rows.Count > 0)
            {
                foreach (GridViewRow rows in grdGroup.Rows)
                {
                    ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                    lb_delete.Visible = true;
                    ImageButton lb_edit = ((ImageButton)rows.FindControl("lb_Edit"));
                    lb_edit.Visible = true;
                }
            }
        }



        LoadGroup();
        LoadlessonPlan();
        loadBehavior();
        LoadMedication();
        viewTab1();
        txteventname.Text = string.Empty;
        txtComment.Text = string.Empty;
        txtSdate.Text = string.Empty;
    }

    private void LoadlessonPlan()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {

            //string qry = "SELECT LP.LessonPlanId AS Id, LP.LessonPlanName AS Name " +
            //    "FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId) " +
            //    "ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId " +
            //    "LEFT OUTER JOIN (StdtIEP IEP INNER JOIN LookUp LUp ON LUp.LookupId=IEP.StatusId) " +
            //    "ON IEP.StdtIEPId=StdtLP.StdtIEPId) " +
            //    "INNER JOIN LessonPlan LP " +
            //    "ON StdtLP.LessonPlanId=LP.LessonPlanId " +
            //    "WHERE StdtLP.StudentId=" + sess.StudentId + " " +
            //    "AND StdtLP.ActiveInd='A' " +
            //    "AND ((LUp.LookupName='Approved' AND StdtLP.IncludeIEP=1) OR LUp.LookupName IS NULL) " +
            //    "AND LU.LookupName<>'Expired'";
            //objData.ReturnDropDown_LessonPlan(qry, ddlLessonplan);
            try
            {
                DataTable dtLP = new DataTable();
                dtLP.Columns.Add("Id", typeof(string));
                dtLP.Columns.Add("Name", typeof(string));
                DataRow dr0 = dtLP.NewRow();
                dr0["Id"] = -1;
                dr0["Name"] = "----------Select Lesson Plan----------";
                dtLP.Rows.Add(dr0);
                DataRow dr = dtLP.NewRow();
                dr["Id"] = 0;
                dr["Name"] = "All Lesson Plan";
                dtLP.Rows.Add(dr);

                clsLessons oLessons = new clsLessons();
                DataTable DTLesson = oLessons.getLessonPlans(sess.StudentId.ToString(), "LP.LessonPlanId AS Id, DTmp.DSTemplateName AS Name,DTmp.VerNbr", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')");
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
                ddlLessonplan.DataSource = dtLP;
                ddlLessonplan.DataTextField = "Name";
                ddlLessonplan.DataValueField = "Id";
                ddlLessonplan.DataBind();

                ddlLessonplan0.DataSource = dtLP;
                ddlLessonplan0.DataTextField = "Name";
                ddlLessonplan0.DataValueField = "Id";
                ddlLessonplan0.DataBind();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public void loadBehavior()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            //objData.ReturnDropDown("SELECT MeasurementId as Id,Behaviour as Name FROM BehaviourDetails WHERE StudentId=" + sess.StudentId + " AND (Frequency <>0 OR Duration <>0) AND ActiveInd='A' AND SchoolId=" + sess.SchoolId, ddlBehavior);
            //if (ddlBehavior.Items.Count > 0)
            //{
            //    ddlBehavior.Items.RemoveAt(0);
            //    ListItem index0 = new ListItem("----------Select Behavior----------", "-1");
            //    ListItem index1 = new ListItem("All Behavior", "0");
            //    ddlBehavior.Items.Insert(0, index0);
            //    ddlBehavior.Items.Insert(1, index1);
            //}
            //else
            //    ddlBehavior.Items.Insert(0, new ListItem("----------Select Behavior----------", "0"));


            DataTable dtLP = new DataTable();
            dtLP.Columns.Add("Id", typeof(string));
            dtLP.Columns.Add("Name", typeof(string));
            DataRow dr0 = dtLP.NewRow();
            dr0["Id"] = -1;
            dr0["Name"] = "----------Select Behavior----------";
            dtLP.Rows.Add(dr0);
            DataRow dr = dtLP.NewRow();
            dr["Id"] = 0;
            dr["Name"] = "All Behavior";
            dtLP.Rows.Add(dr);

            clsLessons oLessons = new clsLessons();
            DataTable DTLesson = objData.ReturnDataTable("SELECT MeasurementId as Id,Behaviour as Name FROM BehaviourDetails WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND ActiveInd='A'", false);     //AND (Frequency <>0 OR Duration <>0) 
            //DataTable DTLesson = objData.ReturnDataTable("SELECT  Bdet.MeasurementId as Id,Bdet.Behaviour as Name FROM BehaviourDetails Bdet LEFT JOIN BehaviourLPRel BlpRel on Bdet.MeasurementId = BlpRel.MeasurementId WHERE Bdet.StudentId = " + sess.StudentId + " AND (Bdet.ActiveInd = 'A' OR Bdet.ActiveInd = 'N') order by Bdet.CreatedOn", false);
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
            ddlBehavior.DataSource = dtLP;
            ddlBehavior.DataTextField = "Name";
            ddlBehavior.DataValueField = "Id";
            ddlBehavior.DataBind();

            ddlBehavior1.DataSource = dtLP;
            ddlBehavior1.DataTextField = "Name";
            ddlBehavior1.DataValueField = "Id";
            ddlBehavior1.DataBind();
            

        }

    }

    private void LoadMedication()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (RadioButtonevent.SelectedValue == "Medication")
        {
        }
        if (sess != null)
        {
            DataTable Dt = null;
            if (RadioButtonevent.SelectedValue == "Medication" && (txtSdate0.Text != "" || txtEdate1.Text != ""))
            {
                Dt = objData.ReturnDataTable("select * from(Select StdtSessEventId,EventName,Comment,EvntTs,CreatedOn,ModifiedOn,CASE WHEN EndTime='1900-01-01 00:00:00.000' THEN NULL ELSE EndTime END AS EndTime from [StdtSessEvent] Where EventType='EV' And StudentId=" + sess.StudentId + " AND StdtSessEventType='Medication' )" + query3, false);//AND ClassId=" + sess.Classid + "
            
            }
            else
            Dt = objData.ReturnDataTable("Select StdtSessEventId,EventName,Comment,EvntTs,CreatedOn,ModifiedOn,CASE WHEN EndTime='1900-01-01 00:00:00.000' THEN NULL ELSE EndTime END AS EndTime from [StdtSessEvent] Where EventType='EV' And StudentId=" + sess.StudentId + " AND StdtSessEventType='Medication' ", false);//AND ClassId=" + sess.Classid + "
            GrdMedication.DataSource = Dt;
            GrdMedication.DataBind();
        }
    }

    private void LoadGroup()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            ///select query to include Academic IOA events AND Behavior IOA events to Event Tracker
            ///
            //DataTable Dt = objData.ReturnDataTable("Select StdtSessEvent.LessonPlanId,StdtSessEvent.MeasurementId,StdtSessEvent.StdtSessEventId,StdtSessEvent.EventName," +
            //    //"CASE WHEN EventName='SET MOVEUP' OR EventName='SET MOVEDOWN' THEN (SELECT SetCd FROM [DSTempSet] WHERE [DSTempSet].DSTempSetId=setid) ELSE CASE WHEN  EventName='STEP MOVEUP' OR EventName='STEP MOVEDOWN' THEN (SELECT StepCd FROM [DSTempStep] WHERE [DSTempStep].DSTempStepId=Stepid) ELSE CASE WHEN EventName='PROMPT MOVEUP' OR EventName='PROMPT MOVEDOWN' THEN ( SELECT LookUpName FROM LookUp WHERE lookuptype='DSTempPrompt' and LookUp.LookupId=PromptId) ELSE CASE WHEN EventName='COMPLETED' THEN LessonPlanName END END END END +'-'+ StdtSessEvent.EventName EventName," +
            //                "CASE WHEN (SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtSessEvent.LessonPlanId AND StudentId=StdtSessEvent.StudentId " +
            //               " AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus')) >0 THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE LessonPlanId=StdtSessEvent.LessonPlanId AND StudentId=StdtSessEvent.StudentId " +
            //               " AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus') ORDER BY DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE LessonPlanId=StdtSessEvent.LessonPlanId AND StudentId=StdtSessEvent.StudentId " +
            //               " ORDER BY DSTempHdrId DESC) END LessonPlanName,StdtSessEvent.StdtSessEventType,StdtSessEvent.Comment,convert(char(10), StdtSessEvent.EvntTs, 101) as EvntTs,StdtSessEvent.CreatedOn,StdtSessEvent.ModifiedOn,(select Behaviour from BehaviourDetails where " +
            //               " BehaviourDetails.MeasurementId=StdtSessEvent.MeasurementId) as Behaviour  from [StdtSessEvent] left join LessonPlan on StdtSessEvent.LessonPlanId = LessonPlan.LessonPlanId Where EventType='EV' And StdtSessEvent.StudentId=" + sess.StudentId + " AND StdtSessEvent.StdtSessEventType<>'Medication' Order by StdtSessEvent.StdtSessEventId DESC ", false);//AND StdtSessEvent.ClassId=" + sess.Classid + "
            string query1 = "((SELECT RTRIM(LTRIM(UPPER(UserInitial))) From [User] US WHERE US.UserId=(SELECT CreatedBy FROM StdtSessionHdr Hdr WHERE Hdr.StdtSessionHdrId=SH.IOASessionHdrId AND SH.IOAInd='Y'))+'/'+(SELECT RTRIM(LTRIM(UPPER(UserInitial))) From [User] US where SH.IOAUserId=US.UserId))";
            string query2 = "CASE WHEN BIOA.NormalBehaviorId IS NULL THEN ((SELECT TOP 1 RTRIM(LTRIM(UPPER(US.UserInitial))) FROM Behaviour BH INNER JOIN [USER] US ON BH.CreatedBy = US.UserId " +
	            " WHERE BH.CreatedOn between DATEADD(minute,-5,BIOA.CreatedOn) AND BIOA.CreatedOn ORDER BY BH.CreatedOn DESC)+'/'+ (SELECT TOP 1 RTRIM(LTRIM(UPPER(US.UserInitial))) FROM BehaviorIOADetails BI INNER JOIN [USER] US ON BI.CreatedBy = US.UserId " +
	            " WHERE BI.CreatedOn=BIOA.CreatedOn ORDER BY BI.CreatedOn DESC)) ELSE ((SELECT RTRIM(LTRIM(UPPER(US.UserInitial))) FROM Behaviour BH INNER JOIN [USER] US ON BH.CreatedBy = US.UserId " +
	            " WHERE BIOA.NormalBehaviorId=BH.BehaviourId)+'/'+ (SELECT RTRIM(LTRIM(UPPER(US.UserInitial))) FROM BehaviorIOADetails BI INNER JOIN [USER] US ON BI.CreatedBy = US.UserId " +
	            " INNER JOIN Behaviour BH ON BH.BehaviourId=BI.NormalBehaviorId WHERE BIOA.NormalBehaviorId=BH.BehaviourId)) END";
            if (srhck == 0&&Panel1.Visible == true && UpdatePanel1.Visible == false)
            {
                btnsearch();
                btnSave.Visible = false;
            }
            DataTable Dt = null;
            if (srhck == 1 && (Panel1.Visible == true && UpdatePanel1.Visible == false) && query3 != "")
            {
                string queryfilter = "SELECT * FROM (SELECT * FROM  ((SELECT SE.LessonPlanId, SE.MeasurementId, SE.StdtSessEventId, NULL AS StdtSessionHdrId, SE.EventName, " +
                "CASE WHEN (SELECT COUNT(DSTempHdrId) FROM  DSTempHdr DH LEFT JOIN LookUp LU ON DH.StatusId=LU.LookupId WHERE LessonPlanId=SE.LessonPlanId AND StudentId=SE.StudentId " +
                "AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus') >0 THEN (SELECT TOP 1 DSTemplateName FROM  DSTempHdr DH LEFT JOIN LookUp LU ON " +
                "DH.StatusId=LU.LookupId WHERE LessonPlanId=SE.LessonPlanId AND StudentId=SE.StudentId AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus' " +
                "ORDER BY DH.DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM  DSTempHdr WHERE LessonPlanId=SE.LessonPlanId AND  StudentId=SE.StudentId ORDER BY DSTempHdrId " +
                "DESC) END LessonPlanName, SE.StdtSessEventType, SE.Comment, CONVERT(CHAR(10), SE.EvntTs,101) AS EvntTs, CASE WHEN SE.CreatedOn IS NULL THEN SE.EvntTs ELSE " +
                "SE.CreatedOn END AS CreatedOn, SE.ModifiedOn, NULL AS BehaviorIOAId, B.Behaviour FROM  [StdtSessEvent] SE LEFT JOIN LessonPlan L ON SE.LessonPlanId = L.LessonPlanId " +
                "LEFT JOIN BehaviourDetails B ON B.MeasurementId=SE.MeasurementId WHERE EventType='EV' AND SE.StudentId=" + sess.StudentId + " AND SE.StdtSessEventType<>'Medication') " +
                "UNION ALL (SELECT SH.LessonPlanId, NULL AS MeasurementId, NULL AS StdtSessEventId, StdtSessionHdrId, 'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" + query1 + " AS EventName, " +
                "CASE WHEN (SELECT COUNT(DSTempHdrId) FROM  DSTempHdr DH LEFT JOIN LookUp LU ON DH.StatusId=LU.LookupId WHERE LessonPlanId=SH.LessonPlanId AND StudentId=SH.StudentId " +
                "AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus') >0 THEN (SELECT TOP 1 DSTemplateName FROM  DSTempHdr DH LEFT JOIN LookUp LU ON " +
                "DH.StatusId=LU.LookupId WHERE LessonPlanId=SH.LessonPlanId AND StudentId=SH.StudentId AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus' " +
                "ORDER BY DH.DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM  DSTempHdr WHERE LessonPlanId=SH.LessonPlanId AND StudentId=SH.StudentId ORDER BY DSTempHdrId " +
                "DESC) END LessonPlanName, 'Arrow notes' AS StdtSessEventType, SH.Comments AS Comment, CONVERT(CHAR(10),SH.EndTs,101) AS EvntTs, SH.CreatedOn, SH.ModifiedOn, " +
                "NULL AS BehaviorIOAId, NULL AS Behaviour FROM  StdtSessionHdr SH LEFT JOIN LessonPlan ON SH.LessonPlanId=LessonPlan.LessonPlanId WHERE SH.IOAPerc IS NOT NULL " +
                "AND SH.IOAInd='Y' AND SH.SessionStatusCd='S' AND SH.StudentId=" + sess.StudentId + ")" +
                "UNION ALL (SELECT NULL AS LessonPlanId, BIOA.MeasurementId, NULL AS StdtSessEventId, NULL AS StdtSessionHdrId, 'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" + query2 + " AS EventName, " +
                "NULL AS LessonPlanName, 'Arrow notes' AS StdtSessEventType, NULL AS Comment, CONVERT(CHAR(10), BIOA.CreatedOn,101) AS EvntTs, BIOA.CreatedOn, BIOA.ModifiedOn, " +
                "BIOA.BehaviorIOAId, BHD.Behaviour FROM BehaviorIOADetails BIOA LEFT JOIN BehaviourDetails BHD ON BIOA.MeasurementId=BHD.MeasurementId " +
                "WHERE BIOA.StudentId=" + sess.StudentId + " AND IOAPerc IS NOT NULL AND BIOA.ActiveInd='A') )IOA ) " + query3 +"order By ad.CreatedOn DESC";
                
                      Dt = objData.ReturnDataTable(queryfilter, false);
                 
            }
            else
            {
                Dt = objData.ReturnDataTable("SELECT * FROM  ((SELECT SE.LessonPlanId, SE.MeasurementId, SE.StdtSessEventId, NULL AS StdtSessionHdrId, SE.EventName, " +
               "CASE WHEN (SELECT COUNT(DSTempHdrId) FROM  DSTempHdr DH LEFT JOIN LookUp LU ON DH.StatusId=LU.LookupId WHERE LessonPlanId=SE.LessonPlanId AND StudentId=SE.StudentId " +
               "AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus') >0 THEN (SELECT TOP 1 DSTemplateName FROM  DSTempHdr DH LEFT JOIN LookUp LU ON " +
               "DH.StatusId=LU.LookupId WHERE LessonPlanId=SE.LessonPlanId AND StudentId=SE.StudentId AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus' " +
               "ORDER BY DH.DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM  DSTempHdr WHERE LessonPlanId=SE.LessonPlanId AND StudentId=SE.StudentId ORDER BY DSTempHdrId " +
               "DESC) END LessonPlanName, SE.StdtSessEventType, SE.Comment, CONVERT(CHAR(10), SE.EvntTs,101) AS EvntTs, CASE WHEN SE.CreatedOn IS NULL THEN SE.EvntTs ELSE " +
               "SE.CreatedOn END AS CreatedOn, SE.ModifiedOn, NULL AS BehaviorIOAId, B.Behaviour FROM  [StdtSessEvent] SE LEFT JOIN LessonPlan L ON SE.LessonPlanId = L.LessonPlanId " +
               "LEFT JOIN BehaviourDetails B ON B.MeasurementId=SE.MeasurementId WHERE EventType='EV' AND SE.StudentId=" + sess.StudentId + " AND SE.StdtSessEventType<>'Medication') " +
               "UNION ALL (SELECT SH.LessonPlanId, NULL AS MeasurementId, NULL AS StdtSessEventId, StdtSessionHdrId, 'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" + query1 + " AS EventName, " +
               "CASE WHEN (SELECT COUNT(DSTempHdrId) FROM  DSTempHdr DH LEFT JOIN LookUp LU ON DH.StatusId=LU.LookupId WHERE LessonPlanId=SH.LessonPlanId AND StudentId=SH.StudentId " +
               "AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus') >0 THEN (SELECT TOP 1 DSTemplateName FROM  DSTempHdr DH LEFT JOIN LookUp LU ON " +
               "DH.StatusId=LU.LookupId WHERE LessonPlanId=SH.LessonPlanId AND StudentId=SH.StudentId AND LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus' " +
               "ORDER BY DH.DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM  DSTempHdr WHERE LessonPlanId=SH.LessonPlanId AND StudentId=SH.StudentId ORDER BY DSTempHdrId " +
               "DESC) END LessonPlanName, 'Arrow notes' AS StdtSessEventType, SH.Comments AS Comment, CONVERT(CHAR(10),SH.EndTs,101) AS EvntTs, SH.CreatedOn, SH.ModifiedOn, " +
               "NULL AS BehaviorIOAId, NULL AS Behaviour FROM  StdtSessionHdr SH LEFT JOIN LessonPlan ON SH.LessonPlanId=LessonPlan.LessonPlanId WHERE SH.IOAPerc IS NOT NULL " +
               "AND SH.IOAInd='Y' AND SH.SessionStatusCd='S' AND SH.StudentId=" + sess.StudentId + ")" +
               "UNION ALL (SELECT NULL AS LessonPlanId, BIOA.MeasurementId, NULL AS StdtSessEventId, NULL AS StdtSessionHdrId, 'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" + query2 + " AS EventName, " +
               "NULL AS LessonPlanName, 'Arrow notes' AS StdtSessEventType, NULL AS Comment, CONVERT(CHAR(10), BIOA.CreatedOn,101) AS EvntTs, BIOA.CreatedOn, BIOA.ModifiedOn, " +
               "BIOA.BehaviorIOAId, BHD.Behaviour FROM BehaviorIOADetails BIOA LEFT JOIN BehaviourDetails BHD ON BIOA.MeasurementId=BHD.MeasurementId " +
               "WHERE BIOA.StudentId=" + sess.StudentId + " AND IOAPerc IS NOT NULL AND BIOA.ActiveInd='A') )IOA ORDER BY  IOA.CreatedOn  DESC", false);

              
            }
                ///end
            ///

            if (Dt != null)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (Dt.Rows[i]["LessonPlanId"].ToString() == "0")
                    {
                        Dt.Rows[i]["LessonPlanName"] = "All Lesson Plans";
                    }
                    if (Dt.Rows[i]["MeasurementId"].ToString() == "0" )
                    {
                        Dt.Rows[i]["Behaviour"] = "All Behavior";
                    }
                }
            }
            grdGroup.DataSource = Dt;
            grdGroup.DataBind();
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Validation() == true)
        {

            try
            {
                int result = 0;
                bool res = false;
                if (hdneventId.Value == "" && btnSave.Text == "Save")
                {
                    result = Save();
                    if (result > 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Saved Sucessfully");
                    }
                }
                else if (hdneventId.Value != "" && btnSave.Text == "Update")
                {
                    res = Update();
                    if (res == true)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Updated Sucessfully");
                    }
                    Eventvisible();
                }
                else if (hdneventId.Value != "" && btnSave.Text == "Delete")
                {
                    Delete();
                    Eventvisible();
                }
                clearFields();
            }
            catch (SqlException Ex)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Execution failed,Please Try Again");

            }

        }
    }

    private void clearFields()
    {
        txtComment.Text = "";
        txteventname.Text = "";
        txtSdate.Text = "";
        txtEdate.Text = "";
        hdneventId.Value = "";
        hdnStdtSessionHdrId.Value = "";
        hdnBehaviorIOAId.Value = "";
        btnSave.Text = "Save";
        ddlLessonplan.SelectedIndex = 0;
        ddlBehavior.SelectedIndex = 0;
        ddlBehavior.Enabled = true;
        ddlLessonplan.Enabled = true;
        LoadGroup();
        LoadMedication();
        ddlType.SelectedValue = "0";
        setVisible(false);
    }


    private bool Validation()
    {
        objData = new clsData();
        if (txteventname.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter event name");
            txteventname.Focus();
            return false;
        }
        else if (ddlType.SelectedValue == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select type");
            return false;
        }
        
        //else if (ddlType.SelectedValue == "3")
        //{
        //    if (txtSdate.Text == string.Empty)
        //    {
        //        tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter Start Date");
        //        txtSdate.Focus();
        //        return false;
        //    }
        //    else if (txtEdate.Text == string.Empty)
        //    {
        //        tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter End Date");
        //        txtEdate.Focus();
        //        return false;
        //    }
        //    else if (txtSdate.Text != string.Empty && txtEdate.Text != string.Empty)
        //    {
        //        string[] arDate = txtSdate.Text.Split('-');
        //        string strDate = "";
        //        strDate = arDate[0];
        //        arDate[0] = arDate[1];
        //        arDate[1] = strDate;
        //        strDate = arDate[0] + "/" + arDate[1] + "/" + arDate[2];
        //        DateTime sdate = Convert.ToDateTime(strDate);

        //        string[] erDate = txtEdate.Text.Split('-');
        //        string endDate = "";
        //        endDate = erDate[0];
        //        erDate[0] = erDate[1];
        //        erDate[1] = endDate;
        //        endDate = erDate[0] + "/" + erDate[1] + "/" + erDate[2];
        //        DateTime edate = Convert.ToDateTime(endDate);

        //        if (sdate > edate)
        //        {
        //            tdMsg.InnerHtml = clsGeneral.warningMsg("Start date should be less than end date");
        //            txtSdate.Focus();
        //            return false;
        //        }
        //    }
        //}
        else if (ddlType.SelectedValue != "3")
        {
            if ((ddlLessonplan.SelectedIndex == 0) && (ddlBehavior.SelectedIndex == 0))
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please select lesson plan or behavior");
                ddlLessonplan.Focus();
                return false;
            }
            else if (txtSdate.Text == string.Empty)
            {               

                tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter date");
                txtSdate.Focus();
                return false;
            }
            else if (txtSdate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //if (dtst < DateTime.Now.Date)
                //{
                //    tdMsg.InnerHtml = clsGeneral.warningMsg("Create ,edit and delete is not possible for previous date.");
                //    txtSdate.Focus();
                //    return false;
                //}
                //removed validation for the date
            }
        }
        else if (ddlType.SelectedValue == "3") // 3 for medication
        {
            if (txtSdate.Text == string.Empty)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter Start Date");
                txtSdate.Focus();
                return false;
            }
            else if (txtSdate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //if (dtst < DateTime.Now.Date)
                //{
                //    tdMsg.InnerHtml = clsGeneral.warningMsg("Incorrect Start Date");
                //    txtSdate.Focus();
                //    return false;
                //}
                //removed validation for the date
            }
            else if (txtEdate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst < DateTime.Now.Date)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Incorrect End Date");
                    txtSdate.Focus();
                    return false;
                }
            }
            else if (txtSdate.Text != "" && txtEdate.Text != "")
            {
                DateTime dtst = new DateTime();
                dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtet = new DateTime();
                dtet = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (dtst > dtet)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Start date should be less than end date");
                    txtSdate.Focus();
                    return false;
                }
            }
        }
        return true;

    }


    protected void grdGroup_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (Disable == true)
        {
            grdGroup.Columns[6].Visible = false;
            grdGroup.Columns[7].Visible = false;
        }
        else
        {
            grdGroup.Columns[6].Visible = true;
            grdGroup.Columns[7].Visible = true;
        }
    }


    protected void grdGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        txtSdate.Enabled = true;  
        string newval = e.CommandArgument.ToString();
        string[] eventId = newval.Split(',');
        hdneventId.Value = eventId[0].ToString();
        
        if (e.CommandName == "Edit")
        {
            hdnStdtSessionHdrId.Value = eventId[3].ToString();

            if (ddlLessonplan.Items.FindByValue(eventId[1].ToString()) != null && ddlBehavior.Items.FindByValue(eventId[2].ToString()) != null)
            {
                btnSave.Text = "Update";
                LoadGroupById();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Cannot Modify this event");
                return;
            }
        }
        else if (e.CommandName == "Delete")
        {
            //btnSave.Text = "Delete";
            //LoadGroupById();


            //DateTime dtst = new DateTime();
            //dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //if (dtst < DateTime.Now.Date)
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Create ,edit and delete is not possible for previous date.");
            //    txtSdate.Enabled = false;
            //}
            //else
            //{
            //    txtSdate.Enabled = true;  
            //}


        }


        tdMsg.InnerHtml = "";
        setVisible(false);

        if (e.CommandName == "Delete")
        {
            hdnStdtSessionHdrId.Value = eventId[1].ToString();
            hdnBehaviorIOAId.Value = eventId[2].ToString();
            Delete();
            clearFields();
        }
    }

    private void LoadGroupById()
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null && hdneventId.Value != "")
        {
            DataTable Dt = objData.ReturnDataTable("Select StdtSessEventId,EventName,LessonPlanId,MeasurementId,StdtSessEventType,Comment,EvntTs,CreatedOn,ModifiedOn from [StdtSessEvent] Where StdtSessEventId=" + hdneventId.Value + " and EventType='EV'", false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    txteventname.Text = Dt.Rows[0]["EventName"].ToString();
                    txtComment.Text = Dt.Rows[0]["Comment"].ToString();
                    string dt = Dt.Rows[0]["EvntTs"].ToString();
                    DateTime dt1 = Convert.ToDateTime(dt);
                    txtSdate.Text = dt1.ToString("MM/dd/yyyy").Replace("-", "/");
                    string EvntType = Dt.Rows[0]["StdtSessEventType"].ToString();

                    if (EvntType == "Major")
                    {
                        ddlType.SelectedIndex = 0;
                        setVisible(false);
                    }
                    else if (EvntType == "Minor")
                    {
                        ddlType.SelectedIndex = 1;
                        setVisible(false);
                    }
                    else if (EvntType == "Arrow notes")
                    {
                        ddlType.SelectedIndex = 2;
                        setVisible(false);
                    }
                    else if (EvntType == "Medication")
                    {
                        ddlType.SelectedIndex = 3;
                        setVisible(true);
                    }

                    try
                    {
                        if (Dt.Rows[0]["LessonPlanId"] != null) ddlLessonplan.SelectedValue = Convert.ToString(Dt.Rows[0]["LessonPlanId"]);
                        if (Dt.Rows[0]["MeasurementId"] != null) ddlBehavior.SelectedValue = Convert.ToString(Dt.Rows[0]["MeasurementId"]);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
    protected void grdGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grdGroup.EditIndex = -1;

    }


    protected void grdGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
    }
    protected void grdGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGroup.PageIndex = e.NewPageIndex;
        LoadGroup();
        clearFields();
    }


    protected void grdGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (Disable == true)
        {
            //imgBtnEdi.Visible = false;
            //imgBtnDel.Visible = false;
            grdGroup.Columns[6].Visible = false;
            grdGroup.Columns[7].Visible = false;
        }
        else
        {
            grdGroup.Columns[6].Visible = true;
            grdGroup.Columns[7].Visible = true;

        }

    }



    private int Save()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        DateTime dt = new DateTime();
        dt = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        if (sess != null)
        {

            if (ddlType.SelectedValue == "3") //Value 3 for Medication
            {

                if (txtSdate.Text != "")
                {
                    strQuery = "INSERT INTO [StdtSessEvent](SchoolId,StudentId,ClassId,EventName,StdtSessEventType,Comment,EvntTs,EndTime,CreatedOn,ModifiedOn,EventType) " +
                            "VALUES (" + sess.SchoolId + "," + sess.StudentId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txteventname.Text.Trim())) + "','" + ddlType.SelectedItem.Text.Trim() + "'," +
                            "'" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txtComment.Text.Trim())) + "',CONVERT(datetime,'" + txtSdate.Text + "'),CONVERT(datetime,'" + txtEdate.Text + "'),getdate(),getdate(),'EV')";

                }
                else
                {
                    strQuery = "INSERT INTO [StdtSessEvent](SchoolId,StudentId,ClassId,EventName,StdtSessEventType,Comment,EvntTs,CreatedOn,ModifiedOn,EventType) " +
                            "VALUES (" + sess.SchoolId + "," + sess.StudentId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txteventname.Text.Trim())) + "','" + ddlType.SelectedItem.Text.Trim() + "'," +
                            "'" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txtComment.Text.Trim())) + "',CONVERT(datetime,'" + txtSdate.Text + "'),getdate(),getdate(),'EV')";

                }

            }
            else
            {

                strQuery = "INSERT INTO [StdtSessEvent](SchoolId,StudentId,ClassId,EventName,StdtSessEventType,Comment,EvntTs,CreatedOn,ModifiedOn,EventType,LessonPlanId,MeasurementId,TimeStampForReport) " +
                            "VALUES (" + sess.SchoolId + "," + sess.StudentId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txteventname.Text.Trim())) + "','" + ddlType.SelectedItem.Text.Trim() + "'," +
                            "'" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txtComment.Text.Trim())) + "',CONVERT(datetime,'" + txtSdate.Text + "'),getdate(),getdate(),'EV'," + Convert.ToInt32(ddlLessonplan.SelectedValue) + "," + Convert.ToInt32(ddlBehavior.SelectedValue) + ",DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,'" + txtSdate.Text + "') AND " +
                            "SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " AND EventType='EV' AND LessonPlanId=" + Convert.ToInt32(ddlLessonplan.SelectedValue) + ")," +
                            "CONVERT(datetime,'" + txtSdate.Text + "')) )";
            }

            retValsave = objData.Execute(strQuery);
        }

        return retValsave;
    }


    private bool Update()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            if (ddlType.SelectedValue == "3") // 3 stands for medication
            {
                if (txtEdate.Text != "")
                {
                    strQuery = "Update [StdtSessEvent] SET EventName='" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txteventname.Text.Trim())) + "',StdtSessEventType='" + ddlType.SelectedItem.Text.Trim() + "',Comment='" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txtComment.Text.Trim())) + "',EvntTs= CONVERT(datetime,'" + txtSdate.Text + "'),CreatedOn=getdate(),ModifiedOn=getdate(),EndTime=CONVERT(datetime,'" + txtEdate.Text + "') Where StdtSessEventId=" + hdneventId.Value + " and EventType='EV'";
                }
                else
                {
                    string Edate=",EndTime=NULL";
                    strQuery = "Update [StdtSessEvent] SET EventName='" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txteventname.Text.Trim())) + "',StdtSessEventType='" + ddlType.SelectedItem.Text.Trim() + "',Comment='" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txtComment.Text.Trim())) + "',EvntTs= CONVERT(datetime,'" + txtSdate.Text + "'),CreatedOn=getdate(),ModifiedOn=getdate()" + Edate + " Where StdtSessEventId=" + hdneventId.Value + " and EventType='EV'";
                }
            }
            else
            {
                strQuery = "Update [StdtSessEvent] SET EventName='" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txteventname.Text.Trim())) + "',StdtSessEventType='" + ddlType.SelectedItem.Text.Trim() + "',Comment='" + clsGeneral.convertQuotes(clsGeneral.convertDoubleQuotes(txtComment.Text.Trim())) + "',EvntTs= CONVERT(datetime,'" + txtSdate.Text + "'),CreatedOn=getdate(),ModifiedOn=getdate(),LessonPlanId=" + Convert.ToInt32(ddlLessonplan.SelectedValue) + ",MeasurementId=" + Convert.ToInt32(ddlBehavior.SelectedValue) + " Where StdtSessEventId=" + hdneventId.Value + " and EventType='EV'";
            }
            retVal = Convert.ToBoolean(objData.Execute(strQuery));



        }
        return retVal;
    }
    private bool Delete()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            
            if (hdneventId.Value != "" && hdneventId.Value != null)
            {
                DataTable Dt = objData.ReturnDataTable("Select StdtSessEventId,EventName,StdtSessEventType,Comment,EvntTs,CreatedOn,ModifiedOn from StdtSessEvent where StdtSessEventId='" + hdneventId.Value + "' and EventType='EV' ", false);

                if (Dt.Rows.Count == 1)
                {
                    strQuery = "Delete from [StdtSessEvent] Where StdtSessEventId=" + hdneventId.Value + " and EventType='EV'";
                    retVal = Convert.ToBoolean(objData.Execute(strQuery));
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Deleted Sucessfully");
                }
                else
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Cannot Delete !!!");
            }
            ///Delete the Event when it is a LP IOA%
            ///
            else if (hdnStdtSessionHdrId.Value != "" && hdnStdtSessionHdrId.Value != null)
            {
                DataTable Dt = objData.ReturnDataTable("Select StdtSessionHdrId,IOAPerc,IOAInd,CreatedOn,ModifiedOn from StdtSessionHdr where StdtSessionHdrId='" + hdnStdtSessionHdrId.Value + "' ", false);
                if (Dt.Rows.Count == 1)
                {
                    strQuery = "UPDATE StdtSessionHdr SET IOAPerc=NULL WHERE StdtSessionHdrId=" + hdnStdtSessionHdrId.Value ;
                    retVal = Convert.ToBoolean(objData.Execute(strQuery));
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Deleted Sucessfully");
                }
                else
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Cannot Delete !!!");
            }

            ///Delete the Event when it is a Behavior IOA%
            ///
            else if (hdnBehaviorIOAId.Value != "" && hdnBehaviorIOAId.Value != null)
            {
                DataTable Dt = objData.ReturnDataTable("Select BehaviorIOAId,IOAPerc,CreatedOn,ModifiedOn FROM BehaviorIOADetails WHERE BehaviorIOAId='" + hdnBehaviorIOAId.Value + "' ", false);
                if (Dt.Rows.Count == 1)
                {
                    strQuery = "UPDATE BehaviorIOADetails SET IOAPerc=NULL, ActiveInd='D' WHERE BehaviorIOAId=" + hdnBehaviorIOAId.Value;
                    retVal = Convert.ToBoolean(objData.Execute(strQuery));
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Deleted Sucessfully");
                }
                else
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Cannot Delete !!!");
            }
            else
                tdMsg.InnerHtml = clsGeneral.warningMsg("Cannot Delete !!!");

        }
        return retVal;
    }

    private void Eventvisible()
    {
        ddlLessonplan.Enabled = true;
        ddlBehavior.Enabled = true;
        lblSdate.Text = "Date";
        lblcomment.Text = "Comment";
        lbledate.Visible = false;
        txtEdate.Visible = false;
    }

    private void medication()
    {
        ddlBehavior.SelectedIndex = 0;
        ddlLessonplan.SelectedIndex = 0;
        ddlLessonplan.Enabled = false;
        ddlBehavior.Enabled = false;
        lblSdate.Text = "Start Date";
        lblcomment.Text = "Dosage";
        lbledate.Visible = true;
        txtEdate.Visible = true;
    }
    protected void GrdMedication_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdMedication.PageIndex = e.NewPageIndex;
        LoadMedication();
        clearFields();
    }
    protected void GrdMedication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            btnSave.Text = "Update";
            LoadMedicationById(hdneventId.Value);
        }
        hdneventId.Value = e.CommandArgument.ToString();
        tdMsg.InnerHtml = "";
        setVisible(true);    
        if (e.CommandName == "Delete")
        {
            //btnSave.Text = "Delete";
            Delete();
            clearFields();
        }
            
    }
    protected void GrdMedication_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GrdMedication_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "disp();", true);
    }
    private void LoadMedicationById(string StdtSessEventId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null && StdtSessEventId != "")
        {
            DataTable Dt = objData.ReturnDataTable("SELECT EventName,StdtSessEventType,Comment,EvntTs,CASE WHEN EndTime='1900-01-01 00:00:00.000' THEN NULL ELSE EndTime END AS EndTime  FROM [StdtSessEvent] WHERE StdtSessEventId=" + StdtSessEventId + "", false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    medication();
                    txteventname.Text = Dt.Rows[0]["EventName"].ToString();
                    txtComment.Text = Dt.Rows[0]["Comment"].ToString();
                    string dt = Dt.Rows[0]["EvntTs"].ToString();
                    DateTime dt1 = Convert.ToDateTime(dt);
                    txtSdate.Text = dt1.ToString("MM/dd/yyyy").Replace("-", "/");
                    string dtt = Convert.ToString(Dt.Rows[0]["EndTime"]);
                    if (dtt != "")
                    {
                        DateTime dt2 = Convert.ToDateTime(dtt);
                        txtEdate.Text = dt2.ToString("MM/dd/yyyy").Replace("-", "/");
                    }
                    if (dtt == "")
                        txtEdate.Text = "";

                    string EvntType = Dt.Rows[0]["StdtSessEventType"].ToString();

                    if (EvntType == "Major")
                    {
                        ddlType.SelectedIndex = 0;
                    }
                    else if (EvntType == "Minor")
                    {
                        ddlType.SelectedIndex = 1;
                    }
                    else if (EvntType == "Arrow")
                    {
                        ddlType.SelectedIndex = 2;
                    }
                    else if (EvntType == "Medication")
                    {
                        ddlType.SelectedIndex = 3;
                    }


                }
            }
        }
    }
    protected void Tab1_Click(object sender, EventArgs e)
    {
        RadioButtonevent.SelectedValue = "All";
        viewTab1();
        clearFields();
    }

    private void viewTab1()
    {
        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
    }
    protected void Tab2_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Clicked";
        MainView.ActiveViewIndex = 1;
        clearFields();
        clearevent();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "3")
        {
            setVisible(true);
            medication();
        }
        else
        {
            setVisible(false);
            Eventvisible();
        }
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        if (Panel1.Visible == true)
        {
            clearevent();
        }
        LoadData();
       

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {

        if (RadioButtonevent.SelectedValue == "Medication" || Tab2.CssClass == "Clicked")
        {
            sess = (clsSession)Session["UserSession"];
            GrdMedication.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdMedication.HeaderRow.Cells[6].Visible = false;
            GrdMedication.HeaderRow.Cells[7].Visible = false;
            GrdMedication.Columns[6].Visible = false;
            GrdMedication.Columns[7].Visible = false;
            GrdMedication.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GrdMedication.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GrdMedication.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GrdMedication.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GrdMedication.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GrdMedication.AllowPaging = false;
            LoadGroup();
            Response.Clear();
            string filename = sess.StudentName + "_Events.xls";
            string enCodeFileName = Server.UrlEncode(filename);
            Response.AddHeader("content-disposition", "attachment;filename=" + enCodeFileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            GrdMedication.RenderControl(hw);
            Response.Write(sw.ToString());
            Response.End();
        }
        else
        {
            sess = (clsSession)Session["UserSession"];
            grdGroup.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            grdGroup.HeaderRow.Cells[8].Visible = false;
            grdGroup.HeaderRow.Cells[9].Visible = false;
            grdGroup.Columns[8].Visible = false;
            grdGroup.Columns[9].Visible = false;
            grdGroup.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdGroup.AllowPaging = false;
            LoadGroup();
            Response.Clear();
            string filename = sess.StudentName + "_Events.xls";
            string enCodeFileName = Server.UrlEncode(filename);
            Response.AddHeader("content-disposition", "attachment;filename=" + enCodeFileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdGroup.RenderControl(hw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
    protected void loadenterdata(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        tdMsg.Visible = true;
        Panel1.Visible = false;
        UpdatePanel1.Visible = true;
        btnSave.Visible = true;
   
        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
    }
    protected void btn(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        tdMsg.Visible = true;
        Panel1.Visible = false;
        UpdatePanel1.Visible = true;
        btnSave.Visible = true;
        clearFields();

        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
        LoadGroup();
    }
    protected void clearevent()
    {
        ddlLessonplan0.SelectedIndex = 0;
        ddlBehavior1.SelectedIndex = 0;
        ddlBehavior1.Enabled = true;
        ddlLessonplan0.Enabled = true;
       // RadioButtonevent.SelectedIndex = -1;
        RadioButtonevent.SelectedIndex = 0;
        txtEdate1.Text = "";
        txtSdate0.Text = "";
    }
    protected void btn1(object sender, EventArgs e)
    {
        tdMsg.Visible = false;
        RadioButtonevent.SelectedIndex = 0;
        UpdatePanel1.Visible = false;
        Panel1.Visible = true;
        btnSave.Visible = false;
        clearevent();
        
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        //LoadGroup();
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        btnsearch();
        LoadGroup();
    }
    protected void btnsearch()
    {
        int chk = 0;
        srhck = 1;
        if (Convert.ToInt32(ddlLessonplan0.SelectedValue) == -1 && Convert.ToInt32(ddlBehavior1.SelectedValue) >= 0)
            query3 = "ad where ((ad.Behaviour IS NULL and ad.MeasurementId=0) OR ";
        else if (Convert.ToInt32(ddlLessonplan0.SelectedValue) >= 0 && Convert.ToInt32(ddlBehavior1.SelectedValue) == -1)
            query3 = "ad where  ((ad.LessonPlanName IS NULL and ad.LessonPlanId=0) OR ";
        else
            query3 = "ad where  ((ad.LessonPlanName IS NULL and ad.LessonPlanId=0) OR  (ad.Behaviour IS NULL and ad.MeasurementId=0) OR";
        if (Convert.ToInt32(ddlLessonplan0.SelectedValue) != -1 && Convert.ToInt32(ddlLessonplan0.SelectedValue) != 0)
        {
            chk++;
            int Lid = Convert.ToInt32(ddlLessonplan0.SelectedValue);
           query3 = query3 + " ad.LessonPlanName IN (select DStemplateName from DSTempHdr where LessonPlanId=" + Lid + ")";
        }
        else if (Convert.ToInt32(ddlLessonplan0.SelectedValue) == 0)
        {
            chk++;
            int Lid = Convert.ToInt32(ddlLessonplan0.SelectedValue);
            query3 = query3 + " ad.LessonPlanName in(select DStemplateName from DSTempHdr where StudentId=" + sess.StudentId + " )";
        }

        if (Convert.ToInt32(ddlBehavior1.SelectedValue) != -1 && Convert.ToInt32(ddlBehavior1.SelectedValue) != 0)
        {

            int Mid = Convert.ToInt32(ddlBehavior1.SelectedValue);
            if (chk >= 1)
                query3 = query3 + " OR ad.Behaviour=(select TOP 1 Behaviour from BehaviourDetails where MeasurementId=" + Mid + ")";
            else
                query3 = query3 + " ad.Behaviour=(select TOP 1 Behaviour from BehaviourDetails where MeasurementId=" + Mid + ")";
            chk++;
        }
        else if (Convert.ToInt32(ddlBehavior1.SelectedValue) == 0)
        {

            int Mid = Convert.ToInt32(ddlBehavior1.SelectedValue);
            if (chk >= 1)
                query3 = query3 + " OR ad.Behaviour in(select Behaviour from BehaviourDetails where StudentId=" + sess.StudentId + " )";
            else
                query3 = query3 + " ad.Behaviour in(select Behaviour from BehaviourDetails where StudentId=" + sess.StudentId + " )";
            chk++;
        }
        if (RadioButtonevent.SelectedValue != "All")
        {
            if (RadioButtonevent.SelectedValue == "Major")
            {

                if (chk >= 1)
                    query3 = query3 + " )AND ad.StdtSessEventType in('Major')";
                else
                    query3 = query3 + "  ad.StdtSessEventType in('Major')";
                chk++;

            }
            if (RadioButtonevent.SelectedValue == "Minor")
            {

                if (chk >= 1)
                    query3 = query3 + ") AND ad.StdtSessEventType in('Minor')";
                else
                    query3 = query3 + "  ad.StdtSessEventType in('Minor')";
                chk++;
            }
            if (RadioButtonevent.SelectedValue == "Arrow Notes")
            {

                if (chk >= 1)
                    query3 = query3 + ") AND ad.StdtSessEventType in('Arrow notes')";
                else
                    query3 = query3 + " ad.StdtSessEventType in('Arrow notes')";
                chk++;
            }
            if (RadioButtonevent.SelectedValue == "LP Modified")
            {

                if (chk >= 1)
                    query3 = query3 + " )AND ad.EventName in('LP Modified')";
                else
                    query3 = query3 + " ad.EventName in('LP Modified')";
                chk++;
            }
        }
        if (RadioButtonevent.SelectedValue == "All" && txtSdate0.Text == "")
        {

            query3 = query3 + ")";
        }
        if (RadioButtonevent.SelectedValue != "Medication")
        {
            if (txtSdate0.Text != null && txtSdate0.Text != "")
            {
                if (chk >= 1 && RadioButtonevent.SelectedValue == "All")
                    query3 = query3 + " )AND Convert(date,ad.EvntTs)>=" + "'" + txtSdate0.Text + "'";
                else if (chk >= 1 && RadioButtonevent.SelectedValue != "All")
                    query3 = query3 + " AND Convert(date,ad.EvntTs)>=" + "'" + txtSdate0.Text + "'";
                else
                    query3 = query3 + " Convert(date,ad.EvntTs)>=" + "'" + txtSdate0.Text + "'";
                chk++;
            }

            if (txtEdate1.Text != null && txtEdate1.Text != "")
            {

                if (chk >= 1)
                    query3 = query3 + " AND Convert(date,ad.EvntTs)<=" + "'" + txtEdate1.Text + "'";
                else
                    query3 = query3 + " Convert(date,ad.EvntTs)<=" + "'" + txtEdate1.Text + "'";
                chk++;

            }
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        }

        if (RadioButtonevent.SelectedValue == "Medication" || Tab2.CssClass == "Clicked")
        {
            int chk2 = 0;
            if (txtSdate0.Text != null && txtSdate0.Text != "")
            {

                query3 = query3 + " Convert(date,ad.EvntTs)>=" + "'" + txtSdate0.Text + "'";
                chk2++;
            }

            if (txtEdate1.Text != null && txtEdate1.Text != "")
            {
                if (chk2 >= 1)
                    query3 = query3 + " )AND Convert(date,ad.EvntTs)<=" + "'" + txtEdate1.Text + "'";
                else
                    query3 = query3 + " Convert(date,ad.EvntTs)<=" + "'" + txtEdate1.Text + "'";

            }
            ddlLessonplan0.SelectedIndex = 0;
            ddlBehavior1.SelectedIndex = 0;
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            MainView.ActiveViewIndex = 1;
            clearFields();
        }
      

    }
}