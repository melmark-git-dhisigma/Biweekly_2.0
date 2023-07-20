using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.IO.Packaging;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Net;

public partial class StudentBinder_LessonPlanNew : System.Web.UI.Page
{
    #region Objects / Datamembers
    clsData objData = null;
    clsSession sess = null;
    ClsTemplateSession ObjTempSess = null;
    DataTable Dt = null;
    string strQuery = "";
    string strBinder = "";
    object objVal = null;
    int pageid = 0;
    int studid = 0;
    int delLesonPlanId = 0;
    int lessonPlanID = 0;
    int goalId = 0;
    string advanceSet = "";
    string advanceStep = "";
    string advancePrompt = "";
    string MoveBackSet = "";
    string MoveBackStep = "";
    string MoveBackPrompt = "";
    string ModificationSet = "";
    string ModificationStep = "";
    string ModificationPrompt = "";
    string typeOfInstr = "";
    static string[] columns;
    static string[] placeHolders;
    static string[] columnsCheck;
    static int checkCount = 0;

    #endregion

    #region Methods / Functions

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
            if (Request.QueryString["ViewPopUp"] != null)
            {
                btnExportWord.Visible = false;
                btnRefresh.Visible = false;
            }
            
            if (Request.QueryString["export"] == "true")
            {
                btnExportWord_Click(sender, e);
                btnDownload_Click(sender, e);
            }
        }
    }

    private void LoadData()
    {
        int value = 0;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];


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


        string url = HttpContext.Current.Request.Url.AbsoluteUri;

        if (Request.QueryString["pageid"] != null) pageid = Convert.ToInt32(Request.QueryString["pageid"].ToString());
        if (Request.QueryString["studid"] != null) studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
        if (Request.QueryString["lessonId"] != null) lessonPlanID = Convert.ToInt32(Request.QueryString["lessonId"].ToString());
        if (Request.QueryString["goalId"] != null) goalId = Convert.ToInt32(Request.QueryString["goalId"].ToString());
        if (Request.QueryString["delLpID"] != null) delLesonPlanId = Convert.ToInt32(Request.QueryString["delLpID"].ToString());
        //delLpID
        if (lessonPlanID != 0)
        {
            ObjTempSess.LessonPlanId = lessonPlanID;
            if (ObjTempSess.TemplateId == 0)
            {
            ObjTempSess.TemplateId = Convert.ToInt32(objData.FetchValue("SELECT DSTempHdrId FROM DSTempHdr WHERE StudentId IS NULL AND LessonPlanId = " + lessonPlanID + ""));
            }
            
            FillDataLP(goalId);
            ClientScript.RegisterStartupScript(GetType(), "", "hideButton();", true);
            //sess.StudentId = studid;
        }
        else if (delLesonPlanId != 0)
        {
            sess.StudentId = studid;
            if (ObjTempSess != null)
            {
                string selLPCount = "select DSTempHdrId from DSTempHdr where StudentId=" + sess.StudentId + " AND LessonPlanId=(select LessonPlanId from StdtLessonPlan where StdtLessonPlanId=" + delLesonPlanId +
                       " ) and StatusId=(select LookUpId from [LookUp] where LookupType='TemplateStatus' and LookupName='In Progress')";
                ObjTempSess.TemplateId = Convert.ToInt32(objData.FetchValue(selLPCount));
                string LesonPlanId = "select distinct SLP.LessonPlanId from DSTempHdr DH inner join StdtLessonPlan SLP on DH.LessonPlanId=SLP.LessonPlanId where DSTempHdrId=" + ObjTempSess.TemplateId;
                ObjTempSess.LessonPlanId = Convert.ToInt32(objData.FetchValue(LesonPlanId));
                FillDelData();
            }
            btnDownload.Visible = false;
            btnExport.Visible = false;
            btnExportWord.Visible = false;
            ClientScript.RegisterStartupScript(GetType(), "", "hidePrint();", true);
        }
        else
        {
            sess.StudentId = studid;
            if (ObjTempSess != null)
            {
                ObjTempSess.TemplateId = pageid;
                ObjTempSess.LessonPlanId = Convert.ToInt32(objData.FetchValue("SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId = " + pageid + ""));
                FillData();
            }

        }

    }

    private void FillDelData()
    {
        string LessonId = "0";
        string TemplateId = "0";
        string status = "D";
        string tempStatus = "In Progress";
        try
        {
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            if (sess != null) LessonId = ObjTempSess.LessonPlanId.ToString();
            if (ObjTempSess != null) TemplateId = ObjTempSess.TemplateId.ToString();

            FillStudent();
            FillIEPDate(LessonId);
            FillGoal(TemplateId, status, tempStatus);
            FillLessonDetails(LessonId, TemplateId);
            FillMeasurement(TemplateId);
            fillStepAndSet(TemplateId);
            FillCriteria(TemplateId);
            //FillObjective(LessonId, status);
            FillTypeOfInstrutions(TemplateId);
            FilActivitySimuli(TemplateId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void FillObjective(string LessonId, string status)
    {
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        objData = new clsData();
        if (sess != null && ObjTempSess != null)
        {
            Dt = new DataTable();
            if (sess != null && ObjTempSess != null)
            {
                strQuery = "select Objective3 from StdtLessonPlan where SchoolId = " + sess.SchoolId + " and StudentId=" + sess.StudentId + " and LessonPlanId=" + LessonId + " and ActiveInd= '" + status + "'";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null) tdObjective.InnerHtml = "<b>Objective: </b>" + objVal.ToString();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
    }

    private void FillGoal(string TemplateId, string status, string tempStatus)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            sess = (clsSession)Session["UserSession"];

            if (sess != null)
            {
                //strQuery = "Select CONVERT(VARCHAR, StDtIEP.EffStartDate, 103) +' to '+   CONVERT(VARCHAR,  StDtIEP.EffEndDate, 103)   from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.LessonPlanId = " + LessonId + " and StDtLessonPlan.StudentId = " + sess.StudentId + " order by StDtIEP.StdtIEPId Desc ";


                strQuery = " SELECT LP.LessonPlanName as Name,StdtLP.GoalId,Gl.GoalName, DTmp.DSTempHdrId As ID," +
                                                  "  DTmp.StatusId,LU.LookupName  FROM ((StdtLessonPlan StdtLP INNER JOIN Goal Gl ON Gl.GoalId=StdtLP.GoalId " +
                                                  "    INNER JOIN DSTempHdr DTmp  " +
                                                  "      INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND  " +
                                                         "    DTmp.StudentId=StdtLP.StudentId) )   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId  " +
                                                                 "     WHERE StdtLP.StudentId=" + sess.StudentId + " AND   StdtLP.ActiveInd='" + status + "' AND LU.LookupName = '" + tempStatus + "'  " +
                                                                                 "          AND DTmp.DSTempHdrId = " + TemplateId;

                Dt = objData.ReturnDataTable(strQuery, false);
                if (Dt != null)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        string goal = Dr["GoalName"].ToString() + " . ";
                        strBinder += goal;
                    }
                }
                tdGoal.InnerHtml = "<b>Goal : </b>" + strBinder.TrimEnd(',');
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }

    private void FillData()
    {
        string LessonId = "0";
        string TemplateId = "0";
        try
        {
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            if (sess != null) LessonId = ObjTempSess.LessonPlanId.ToString();
            if (ObjTempSess != null) TemplateId = ObjTempSess.TemplateId.ToString();

            FillStudent();
            FillIEPDate(LessonId);
            FillGoal(TemplateId);
            FillLessonDetails(LessonId, TemplateId);
            FillMeasurement(TemplateId);
            fillStepAndSet(TemplateId);
            FillCriteria(TemplateId);
            //FillObjective(LessonId);
            FillTypeOfInstrutions(TemplateId);
            FilActivitySimuli(TemplateId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void FillDataLP(int GoalID)
    {
        string LessonId = "0";
        string TemplateId = "0";
        try
        {
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            if (sess != null) LessonId = ObjTempSess.LessonPlanId.ToString();
            if (ObjTempSess != null) TemplateId = ObjTempSess.TemplateId.ToString();

            FillStudent();
            FillIEPDate(LessonId);
            FillGoalWithLessonPlanID(LessonId, GoalID);
            FillLessonDetails(LessonId, TemplateId);
            FillMeasurement(TemplateId);
            fillStepAndSet(TemplateId);
            FillCriteria(TemplateId);
            //FillObjective(LessonId);
            FilActivitySimuli(TemplateId);
            FillTypeOfInstrutions(TemplateId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void FillStudent()
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                strQuery = "Select StudentLname +','+StudentFname from  dbo.Student where StudentId =" + sess.StudentId + "";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null) tdStudent.InnerHtml = "<b>Individual Name : </b>" + objVal.ToString();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }

    private void FilActivitySimuli(string templateID)
    {
        try
        {
            int val = 0;
            Double versionNum = 0.0;
            string reason = "";
            string createVal = "";
            string modifiedVal = "";
            objData = new clsData();
            Dt = new DataTable();
            DataTable Dt2 = new DataTable();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
               // strQuery = "select StimulyActivityId,case when(ActivitiType='STARTED') then ('STARTED') ELSE (case when(ActivitiType='SET') then (Select 'SET - '+SetCd from DSTempSet where  DSTempSetId= Act.ActivityId)"
               //+ "ELSE (case when(ActivitiType='STEP') then (Select 'STEP - '+StepCd from DSTempStep where  DSTempStepId= Act.ActivityId)"
               //+ "ELSE (case when(ActivitiType='MASTERED') then (Select 'SET - '+SetCd+'' from DSTempSet where  DSTempSetId= Act.ActivityId)"
               //+ "ELSE(Select 'PROMPT - '+ LookupName from LookUp where  LookupId= Act.ActivityId)END)END)END)END NAME,"
               //+ "(Convert(VARCHAR(50), StartTime,110))StartDate,case when(ActivitiType='MASTERED') then (Convert(VARCHAR(50), ACT.CreatedOn,110))"
               //+ "ELSE NULL END MasteredDate,Hdr.Reason_New,Hdr.CreatedOn,Hdr.ModifiedOn,Hdr.VerNbr,Hdr.DSTempHdrId,(Convert(VARCHAR(50), DateClosed,110))DateClosed,(Convert(VARCHAR(50), DateMastered,110))DateMastered from StdtSessStimuliActivity ACT INNER JOIN DSTempHdr Hdr ON ACT.DSTempHdrId=Hdr.DSTempHdrId where ACT.StudentId=" + sess.StudentId + " "
               //+ "AND Hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE  DSTempHdrId=" + Convert.ToInt32(templateID) + ")  ORDER BY ACT.DSTempHdrId" ;
               strQuery=   " (SELECT Distinct ( CASE WHEN( StimulyActivityId is not null ) THEN (StimulyActivityId ) ELSE NULL  END  ) StimulyActivityId,"
         					+ " CASE WHEN( activititype = 'STARTED' ) THEN ( 'STARTED' )"
            				+ " ELSE ( CASE WHEN( activititype = 'SET' ) THEN (SELECT 'SET - ' + setcd FROM   dstempset WHERE dstempsetid = Act.activityid)"
              				+ "     ELSE ( CASE WHEN( activititype = 'STEP' ) THEN (SELECT 'STEP - ' + stepcd FROM dstempstep WHERE  dstempstepid = Act.activityid)"
               			    + "         ELSE ( CASE  WHEN( activititype = 'MASTERED' ) THEN ( SELECT 'SET - ' + setcd + '' FROM dstempset WHERE dstempsetid = Act.activityid)"
                  			+ "            ELSE (  CASE   WHEN( activititype = 'PROMPT' ) THEN (SELECT 'PROMPT - ' + lookupname FROM   lookup WHERE  lookupid = Act.activityid )"
                    		+ "            ELSE( concat('SET - ',SetCd))"
                        	+ "		END ) END ) END ) END ) END NAME,"
        					+ " ISNULL(Convert(Varchar(50), ACT.createdon, 110), ' ') AS StartDate,"
         					+ " CASE"
          					+ " WHEN( activititype = 'MASTERED' ) THEN ("
          					+ " Convert(Varchar(50), ACT.createdon, 110) )"
          					+ " ELSE 'Not Mastered'"
					        + " END MasteredDate,Hdr.reason_new,Hdr.createdon,Hdr.modifiedon,Hdr.vernbr,Hdr.dstemphdrid,      "
					        + " ISNULL(Convert(Varchar(50), ACT.dateclosed, 110), ' ') AS DateClosed,"
					        + " ISNULL(Convert(Varchar(50), ACT.datemastered, 110), ' ') AS DateMastered        "
					        + " FROM   dstemphdr Hdr left JOIN stdtsessstimuliactivity ACT ON ACT.dstemphdrid = Hdr.dstemphdrid"
					        + " JOIN dstempset dstset ON dstset.dstemphdrid = Hdr.dstemphdrid       "
					        + " WHERE  hdr.studentid = " + sess.StudentId + "  AND Hdr.lessonplanid = (SELECT lessonplanid FROM   dstemphdr   WHERE  dstemphdrid = " + Convert.ToInt32(templateID) + ") "
					        + " AND  Hdr.StatusId not in(select LookupId from LookUp lk where lk.LookupName='In Progress' )        )"
					        + " ORDER  BY hdr.dstemphdrid; ";
		             String reasonnewqry = "(SELECT Hdr.reason_new,Hdr.createdon,Hdr.modifiedon,Hdr.vernbr,Hdr.dstemphdrid "
		             + " FROM   dstemphdr Hdr     WHERE  hdr.studentid =  " + sess.StudentId + "   AND "
		             + " Hdr.lessonplanid = (SELECT lessonplanid FROM   dstemphdr   WHERE  dstemphdrid = " + Convert.ToInt32(templateID) + ") "
		             + " AND  Hdr.StatusId not in(select LookupId from LookUp lk where lk.LookupName='In Progress' )        ) ORDER  BY hdr.dstemphdrid; ";
		            DataTable Dtreason = new DataTable();
		            Dtreason = objData.ReturnDataTable(reasonnewqry, false);

                Dt = objData.ReturnDataTable(strQuery, false);
                val = (Dt.Rows.Count) - 1;
                if (Dt != null)
                {
                    if (Dt.Rows.Count > 0)
                    {

                        DataRow[] dtRw = Dt.Select("NAME<>'STARTED' AND DSTempHdrId =" + templateID);

                        if (dtRw != null)
                        {
                            if (dtRw.Length > 0)
                            {
                                if ((dtRw[0]["VerNbr"]) != DBNull.Value)
                                {
                                    versionNum = Convert.ToDouble(dtRw[0]["VerNbr"]);
                                    reason = "(Reason:" + dtRw[0]["Reason_New"].ToString() + ")";
                                    DataRow[] dtRw2 = Dt.Select("VerNbr <='" + versionNum + "' OR VerNbr is null ");
                                    DataTable dt3 = dtRw2.CopyToDataTable();


                                    //if (reason == "" || reason == "()")
                                    //{
                                    //    reason = "(Reason:No Reason to display )";
                                    //    tdReasonNew.InnerHtml = reason;
                                    //}
                                    //else
                                    //{
                                    //    tdReasonNew.InnerHtml = reason;
                                    //}
                                    //createVal = Dt.Rows[val]["CreatedOn"].ToString();
                                    //string[] splitVal = createVal.Split(' ');
                                    //createVal = splitVal[0];
                                    //tdDate.InnerHtml = DateTime.Parse(createVal).ToString("MM/dd/yyyy").Replace('-', '/');
                                    DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                    DataTable dt4Reason = dtRwReason.CopyToDataTable();
 									dt4Reason.DefaultView.Sort = "dstemphdrid";
                                   // dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                    dt4Reason = dt4Reason.DefaultView.ToTable();
                                    tdReasonNew.InnerHtml += "<table style='margin: 0px !important;'>";
                                    foreach (DataRow dr in Dtreason.Rows)
                                    {
                                       // if (dr["NAME"].ToString() == "STARTED" && dr["Reason_New"].ToString() != "")
                                        //{
                                           // reason = dr["Reason_New"].ToString();
                                            //if (reason == "" || reason == "()")
                                            //{
                                           //     reason = "(No Reason to display)";
                                           // }


                                           // createVal = dr["CreatedOn"].ToString();
                                            //string[] splitVal = createVal.Split(' ');
                                            //createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                            //tdReasonNew.InnerHtml += "<tr><td>" + createVal + "</td>";
                                            //tdReasonNew.InnerHtml += "<td>(Reason: " + reason + ")</td></tr>";
                                        //}

									 	if (dr["vernbr"].ToString() != "" && Convert.ToDouble(dr["vernbr"]) == versionNum)
                                       	 	{
                                                createVal = "";
                                                modifiedVal="";
                                                reason = "";
                                          		//no reason                                           
                                        	}                                        
                                        else if( dr["Reason_New"].ToString() != "")
                                        {
                                            reason = dr["Reason_New"].ToString();
                                            if (reason == "" || reason == "()")
                                            {
                                                reason = "(No Reason to display)";
                                            }

                                            createVal = dr["CreatedOn"].ToString();
                                            modifiedVal = dr["ModifiedOn"].ToString();

                                            string[] splitVal = createVal.Split(' ');
                                            createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');

                                            string[] splitmodifidonVal = modifiedVal.Split(' ');
                                            modifiedVal = DateTime.Parse(splitmodifidonVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                            tdReasonNew.InnerHtml += "<tr><td>" + modifiedVal + "</td>";
                                            tdReasonNew.InnerHtml += "<td>(Reason: " + reason + ")</td></tr>";
                                        }
                                        else if( dr["Reason_New"].ToString() == "")
                                        {
                                            reason = dr["Reason_New"].ToString();
                                            if (reason == "" || reason == "()")
                                            {
                                                reason = "(No Reason to display)";
                                            }


                                            createVal = dr["CreatedOn"].ToString();
                                            modifiedVal = dr["ModifiedOn"].ToString();

                                            string[] splitVal = createVal.Split(' ');
                                            createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');

                                            string[] splitmodifidonVal = modifiedVal.Split(' ');
                                            modifiedVal = DateTime.Parse(splitmodifidonVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                            tdReasonNew.InnerHtml += "<tr><td>" + modifiedVal + "</td>";
                                            tdReasonNew.InnerHtml += "<td>(Reason: " + reason + ")</td></tr>";
                                        }
                                    }
                                    tdReasonNew.InnerHtml += "</table>";
                                    dvStimuliActivity.DataSource = dt4Reason;
                                    dvStimuliActivity.DataBind();
                                }
                                else
                                {
                                    tdReasonNew.InnerHtml += "<table style='margin: 0px !important;'>";
                                    DataTable dt3 = dtRw.CopyToDataTable();
                                    DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                    DataTable dt4Reason = dtRwReason.CopyToDataTable();
                                    dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                    dt4Reason = dt4Reason.DefaultView.ToTable();
                                    dvStimuliActivity.DataSource = dt4Reason;
                                    dvStimuliActivity.DataBind();
                                    createVal = Dt.Rows[val]["CreatedOn"].ToString();
                                    modifiedVal = Dt.Rows[val]["ModifiedOn"].ToString();
                                    string[] splitVal = createVal.Split(' ');
                                    createVal = splitVal[0];
                                    string[] splitmodifidonVal = modifiedVal.Split(' ');
                                    modifiedVal = splitVal[0];
                                    //tdDate.InnerHtml = DateTime.Parse(createVal).ToString("MM/dd/yyyy").Replace('-', '/');
                                    if (reason == "")
                                    {
                                        reason = "(Reason: Initial version. No reason to display )";
                                        tdReasonNew.InnerHtml = "<tr><td>" + reason + "</td></tr>";
                                    }
                                    tdReasonNew.InnerHtml += "</table>";
                                }
                            }
                        }
                    }
                    else
                    {
                        String Qrygrid = "select  Hdr.LessonPlanId,  " +
                                 " concat('SET - ',SetCd)Name, " +
                                 "'Not Started' StartDate, 'Not Started' MasteredDate, " +
                                 " Hdr.Reason_New,Hdr.CreatedOn,Hdr.ModifiedOn,Hdr.VerNbr,Hdr.DSTempHdrId, " +
                                 "'Not Started' DateClosed, " +
                                 " 'Not Started' DateMastered from DSTempHdr Hdr " +
                                 " join DSTempSet dstset on dstset.DSTempHdrId=Hdr.DSTempHdrId where " +
                                 " hdr.StudentId=" + sess.StudentId + "  AND"+
                                 " Hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE  DSTempHdrId=" + Convert.ToInt32(templateID) + ")  ORDER BY hdr.DSTempHdrId;";



                        Dt = objData.ReturnDataTable(Qrygrid, false);
                        val = (Dt.Rows.Count) - 1;
                        if (Dt != null)
                        {
                            if (Dt.Rows.Count > 0)
                            {

                                DataRow[] dtRw = Dt.Select("NAME<>'STARTED' AND DSTempHdrId =" + templateID);

                                if (dtRw != null)
                                {
                                    if (dtRw.Length > 0)
                                    {
                                        if ((dtRw[0]["VerNbr"]) != DBNull.Value)
                                        {
                                            versionNum = Convert.ToDouble(dtRw[0]["VerNbr"]);
                                            reason = "(Reason:" + dtRw[0]["Reason_New"].ToString() + ")";
                                            DataRow[] dtRw2 = Dt.Select("VerNbr ='" + versionNum + "' OR VerNbr is null ");
                                            DataTable dt3 = dtRw2.CopyToDataTable();
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            DataTable dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "DSTempHdrId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                    tdReasonNew.InnerHtml += "<table style='margin: 0px !important;'>";
                                    foreach (DataRow dr in dt3.Rows)
                                    {
                                                if ( dr["Reason_New"].ToString() != "")
                                        {
                                            reason = dr["Reason_New"].ToString();
                                            if (reason == "" || reason == "()")
                                            {
                                                reason = "(No Reason to display)";
                                            }


                                            createVal = dr["CreatedOn"].ToString();
                                                    modifiedVal = dr["ModifiedOn"].ToString();

                                            string[] splitVal = createVal.Split(' ');
                                            createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');

                                                    string[] splitmodifidonVal = modifiedVal.Split(' ');
                                                    modifiedVal = DateTime.Parse(splitmodifidonVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                                    tdReasonNew.InnerHtml += "<tr><td>" + modifiedVal + "</td>";
                                            tdReasonNew.InnerHtml += "<td>(Reason: " + reason + ")</td></tr>";
                                        }
                                    }
                                    tdReasonNew.InnerHtml += "</table>";
                                    dvStimuliActivity.DataSource = dt4Reason;
                                    dvStimuliActivity.DataBind();
                                }
                                else
                                {
                                    tdReasonNew.InnerHtml += "<table style='margin: 0px !important;'>";
                                    DataTable dt3 = dtRw.CopyToDataTable();
                                    DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                    DataTable dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "LessonPlanId";
                                    dt4Reason = dt4Reason.DefaultView.ToTable();
                                    dvStimuliActivity.DataSource = dt4Reason;
                                    dvStimuliActivity.DataBind();
                                    createVal = Dt.Rows[val]["CreatedOn"].ToString();
                                    string[] splitVal = createVal.Split(' ');
                                    createVal = splitVal[0];
                                    //tdDate.InnerHtml = DateTime.Parse(createVal).ToString("MM/dd/yyyy").Replace('-', '/');
                                    if (reason == "")
                                    {
                                        reason = "(Reason: Initial version. No reason to display )";
                                        tdReasonNew.InnerHtml = "<tr><td>" + reason + "</td></tr>";
                                    }
                                    tdReasonNew.InnerHtml += "</table>";
                                }
                            }
                        }
                    }
                }
                    }// else
            }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }

    private void FillIEPDate(string LessonId)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            if (sess != null)
            {
                if (sess.SchoolId == 1)
                {

                    strQuery = "Select TOP 1 CONVERT(VARCHAR, LessonSDate, 101) +' to '+ CONVERT(VARCHAR,LessonEDate,101) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId = " + LessonId + " and StatusId=(select Lookupid from Lookup where LookupType='TemplateStatus' and LookupName='Approved')";
                        objVal = objData.FetchValue(strQuery);
                        tdIEPDate.InnerHtml = "<b>IEP Dates: </b>" + objVal;
                   
                }
                else if (sess.SchoolId == 2)
                {
                    strQuery = "Select TOP 1 CONVERT(VARCHAR, LessonSDate, 101) +' to '+ CONVERT(VARCHAR,LessonEDate,101) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId = " + LessonId + " and StatusId=(select Lookupid from Lookup where LookupType='TemplateStatus' and LookupName='Approved')";
                    objVal = objData.FetchValue(strQuery);
                    tdIEPDate.InnerHtml = "<b>IEP Dates: </b>" + objVal;
                }

                //if (objVal != null)
                //{
                //    string[] splitDate = objVal.ToString().Split(' ');
                //    string startD = splitDate[0];
                //    string endD = splitDate[2];
                //    tdIEPDate.InnerHtml = "<b>IEP Dates: </b>" + Convert.ToDateTime(startD).ToString("MM/dd/yyyy").Replace('-', '/') + " to " + Convert.ToDateTime(endD).ToString("MM/dd/yyyy").Replace('-', '/');
                //}
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }

    private void FillGoal(string TemplateID)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            sess = (clsSession)Session["UserSession"];

            if (sess != null)
            {
                //strQuery = "Select CONVERT(VARCHAR, StDtIEP.EffStartDate, 103) +' to '+   CONVERT(VARCHAR,  StDtIEP.EffEndDate, 103)   from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.LessonPlanId = " + LessonId + " and StDtLessonPlan.StudentId = " + sess.StudentId + " order by StDtIEP.StdtIEPId Desc ";

                //GoalCode is used to display the Edited goals
                strQuery = " SELECT distinct LP.LessonPlanName as Name,StdtLP.GoalId,Gl.GoalCode, DTmp.DSTempHdrId As ID," +
                                                  "  DTmp.StatusId,LU.LookupName  FROM ((StdtLessonPlan StdtLP INNER JOIN Goal Gl ON Gl.GoalId=StdtLP.GoalId " +
                                                  "    INNER JOIN DSTempHdr DTmp  " +
                                                  "      INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND  " +
                                                         "    DTmp.StudentId=StdtLP.StudentId) )   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId  " +
                                                                 "     WHERE StdtLP.StudentId=" + sess.StudentId + " AND   StdtLP.ActiveInd='A'  " +
                                                                                 "          AND DTmp.DSTempHdrId = " + TemplateID;

                Dt = objData.ReturnDataTable(strQuery, false);
                if (Dt != null)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        string goal = Dr["GoalCode"].ToString() + " . ";
                        strBinder += goal;
                    }
                }
                tdGoal.InnerHtml = "<b>Goal : </b>" + strBinder.TrimEnd(',');
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }

    private void FillGoalWithLessonPlanID(string LessonPlanId, int GoalID)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            sess = (clsSession)Session["UserSession"];

            if (sess != null)
            {
                //strQuery = "Select CONVERT(VARCHAR, StDtIEP.EffStartDate, 103) +' to '+   CONVERT(VARCHAR,  StDtIEP.EffEndDate, 103)   from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.LessonPlanId = " + LessonId + " and StDtLessonPlan.StudentId = " + sess.StudentId + " order by StDtIEP.StdtIEPId Desc ";


                strQuery = "SELECT GoalName FROM GoalLPRel glLpRel LEFT JOIN Goal gol ON glLpRel.GoalId = gol.GoalId WHERE  " +
                                                        " glLpRel.LessonPlanId = " + LessonPlanId + " AND glLpRel.ActiveInd = 'A' AND glLpRel.GoalId=" + GoalID;

                Dt = objData.ReturnDataTable(strQuery, false);
                if (Dt != null)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        string goal = Dr["GoalName"].ToString() + " , ";
                        strBinder += goal;
                    }
                }
                tdGoal.InnerHtml = "<b>Goal : </b>" + strBinder.TrimEnd(',');
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }

    private void FillLessonDetails(string LessonId, string TempId)
    {
        strQuery = "select DSTemplateName AS LessonPlanName,LessonPlanGoal,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials from DSTempHdr where DSTempHdrId=" + TempId + " ";
        Dt = new DataTable();
        objData = new clsData();
        string correctRes = "";
        string inCorrecResp = "";
        sess = (clsSession)Session["UserSession"];

        if (sess != null)
        {
            Dt = objData.ReturnDataTable(strQuery, true);
            if (Dt.Rows.Count > 0)
            {
                tdLesson.InnerHtml = "<b>Lesson Title: </b>" + Dt.Rows[0]["LessonPlanName"].ToString();
                tdLessonPlanGoal.InnerHtml = "<b>Lesson Plan Goal: </b>" + Dt.Rows[0]["LessonPlanGoal"].ToString();
                tdFrameWork.InnerHtml = "<b>Framework and Strand: </b>" + Dt.Rows[0]["FrameandStrand"].ToString();
                tdSpecificStandard.InnerHtml = "<b>Specific Standard:</b>" + Dt.Rows[0]["SpecStandard"].ToString();
                tdSpecificEntry.InnerHtml = "<b>Specific Entry Point:</b>" + Dt.Rows[0]["SpecEntryPoint"].ToString();
                tdPreReqSkills.InnerHtml = "<b>Pre-requisite Skills:</b>" + Dt.Rows[0]["PreReq"].ToString();
                tdMaterials.InnerHtml = "<b>Materials: </b>" + Dt.Rows[0]["Materials"].ToString();
            }

            strQuery = "select TeachingProcId,SkillType,NbrOfTrials,ChainType,PromptTypeId,Baseline,Objective,GeneralProcedure,CorrRespDef,IncorrRespDef,LessonDefInst,"
                + " StudentReadCrita,TeacherRespReadness,ReinforcementProc,CorrectionProc,MajorSetting,MinorSetting,StudIncorrRespDef,StudCorrRespDef,Mistrial,MistrialResponse,TeacherPrepare,StudentPrepare,StudResponse"
                + " from DSTempHdr where DSTempHdrId = " + TempId + "";

            Dt = objData.ReturnDataTable(strQuery, true);
            if (Dt.Rows.Count > 0)
            {
                //  tdNCorrect.InnerHtml = tdCorrect.InnerHtml = "<b>Correct Response: </b>" + Dt.Rows[0]["CorrRespDef"].ToString();
                //  tdNICorrect.InnerHtml = tdIncorrect.InnerHtml = "<b>Incorrect Response:</b>" + Dt.Rows[0]["IncorrRespDef"].ToString();
                tdLessonDelivary.InnerHtml = "<b>Lesson Delivery Instructions:</b>" + Dt.Rows[0]["LessonDefInst"].ToString();
                // tdReadiness.InnerHtml = "<b>Student Readiness Criteria:</b>" + Dt.Rows[0]["StudentReadCrita"].ToString();
                // tdRestoReadiness.InnerHtml = "<b>Teacher Response to Readiness:</b>" + Dt.Rows[0]["TeacherRespReadness"].ToString();
                tdReInfo.InnerHtml = "<b>Reinforcement Procedure:</b>" + Dt.Rows[0]["ReinforcementProc"].ToString();
                tdCorrProc.InnerHtml = "<b>Correction Procedure:</b>" + Dt.Rows[0]["CorrectionProc"].ToString();
                tdMajorSet.InnerHtml = "<b>Major Setting:</b>" + Dt.Rows[0]["MajorSetting"].ToString();
                tdMinorSet.InnerHtml = "<b>Minor Setting:</b>" + Dt.Rows[0]["MinorSetting"].ToString();
                tdMistrialProcedure.InnerHtml = "<b>Mistrial Procedure:</b>" + Dt.Rows[0]["MistrialResponse"].ToString();
                tdMistrialResponse.InnerHtml = "<b>Mistrial Response:</b>" + Dt.Rows[0]["Mistrial"].ToString();
                tdNCorrect.InnerHtml = "<b>Correct Response:</b>" + Dt.Rows[0]["StudCorrRespDef"].ToString();
                tdNICorrect.InnerHtml = "<b>Incorrect Response:</b>" + Dt.Rows[0]["StudIncorrRespDef"].ToString();
                tdBaseLine.InnerHtml = Dt.Rows[0]["Baseline"].ToString();
                tdGenProcedure.InnerHtml = Dt.Rows[0]["GeneralProcedure"].ToString();
                tdObjective.InnerHtml = "<b>Objective:</b>" + " " + Dt.Rows[0]["Objective"].ToString();
                q1.InnerHtml = Dt.Rows[0]["TeacherPrepare"].ToString();
                q2.InnerHtml = Dt.Rows[0]["StudentPrepare"].ToString();
                q3.InnerHtml = Dt.Rows[0]["StudResponse"].ToString();

            }


            strQuery = "SELECT CorrRespDesc,InCorrRespDesc FROM DSTempSetCol WHERE DSTempHdrId = " + TempId + " AND ActiveInd = 'A'";
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Dt.Rows[i]["CorrRespDesc"].ToString() != "")
                        {
                            correctRes += Dt.Rows[i]["CorrRespDesc"].ToString() + ",";
                        }
                        if (Dt.Rows[i]["InCorrRespDesc"].ToString() != "")
                        {
                            inCorrecResp += Dt.Rows[i]["InCorrRespDesc"].ToString() + ",";
                        }
                    }
                    correctRes = correctRes.TrimEnd(',');
                    inCorrecResp = inCorrecResp.TrimEnd(',');

                    tdCorrect.InnerHtml = tdCorrect.InnerHtml = "<b>Correct Response: </b>" + correctRes.ToString();
                    tdIncorrect.InnerHtml = tdIncorrect.InnerHtml = "<b>Incorrect Response:</b>" + inCorrecResp.ToString();

                }
                else
                {
                    tdCorrect.InnerHtml = tdCorrect.InnerHtml = "<b>Correct Response: </b> No input Data ";
                    tdIncorrect.InnerHtml = tdIncorrect.InnerHtml = "<b>Incorrect Response:</b> No input Data ";
                }
            }
            else
            {
                tdCorrect.InnerHtml = tdCorrect.InnerHtml = "<b>Correct Response: </b> No input Data ";
                tdIncorrect.InnerHtml = tdIncorrect.InnerHtml = "<b>Incorrect Response:</b> No input Data ";
            }


        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
        }
    }

    protected string SplitText(string text)
    {
        int limit = text.Length / 160;
        int j = 0;
        for (int i = 1; i <= limit; i++)
        {
            text = text.Insert((i * 160) + j, "<br/>");
            j += 4;
        }
        return text;
    }

    private void FillMeasurement(string TempId)
    {
        sess = (clsSession)Session["UserSession"];

        objData = new clsData();

        if (sess != null)
        {
            strQuery = "select ColTypeCd from DSTempSetCol where DSTempHdrId = " + TempId + " and ActiveInd ='A'";
            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, false);

            strBinder = "<ul>";
            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<li>" + Dr["ColTypeCd"].ToString() + "</li>";
                }
            }
            tdMeasurement.InnerHtml = "<b>Measurement System: </b> </br>" + strBinder.ToString() + "</ul>";
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
        }

    }

    private void fillStepAndSet(string TempId)
    {
        sess = (clsSession)Session["UserSession"];

        objData = new clsData();
        if (sess != null && ObjTempSess != null)
        {
            strQuery = "SELECT  DSTempParentStepId,SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames FROM DSTempParentStep"
                        + " WHERE DSTempHdrId = " + TempId + " And ActiveInd = 'A' ORDER BY DSTempSetId,SortOrder";


            strBinder = "<ul>";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<li>" + Dr["StepCd"].ToString() + "-" + Dr["StepName"].ToString() + "</li>";
                }
            }
            tdStep.InnerHtml = "<b>Step Description(s):</b>" + strBinder.ToString() + "</ul>";


            strBinder = "<ul>";
            strQuery = "select SetName,SetCd from dbo.DSTempSet where DSTempHdrId = " + TempId + " AND ActiveInd = 'A' order by SortOrder";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<li>" + Dr["SetCd"].ToString() + "-" + Dr["SetName"].ToString() + "</li>";
                }
            }

            tdSet.InnerHtml = "<b>Set Description(s):</b>" + strBinder.ToString() + "</ul>";
        }

    }

    private void FillCriteria(string TempId)
    {
        Session["Criteria"] = "";
        string caltype = string.Empty;
        int HdrId = 0;
        strQuery = "select DSTempSetColId,ColName from DSTempSetCol where DSTempHdrId = " + TempId + " and ActiveInd ='A'";
        objData = new clsData();
        DataTable DtSet = new DataTable();
        DtSet = objData.ReturnDataTable(strQuery, false);
        if (DtSet.Rows.Count > 0)
        {
            for (int i = 0; i < DtSet.Rows.Count; i++)
            {
                if (DtSet.Rows[0]["DSTempSetColId"] != null)
                {
                    strQuery = " SELECT  DR.RuleType, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DST.IncMisTrialInd, DST.ColName,DR.CriteriaDetails," +
                               "DC.CalcType,DC.CalcLabel,Dt.DSTempHdrId, DR.ScoreReq,DR.ConsequetiveInd FROM DSTempHdr Dt " +
                               "INNER JOIN DSTempSetCol DST ON Dt.DSTempHdrId = DST.DSTempHdrId " +
                               "INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId " +
                               "INNER JOIN DSTempRule DR ON DR.DSTempSetColCalcId = DC.DSTempSetColCalcId " +
                               "WHERE DR.DSTempSetColId=" + DtSet.Rows[i]["DSTempSetColId"].ToString() + " AND DR.ActiveInd = 'A'";
                    Dt = new DataTable();
                    Dt = objData.ReturnDataTable(strQuery, false);

                    if (Dt != null)
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            if (HdrId == 0)
                            {
                                HdrId = Convert.ToInt32(Dt.Rows[0]["DSTempHdrId"]);
                            }

                            for (int j = 0; j < Dt.Rows.Count; j++)
                            {

                                if (Dt.Rows[j]["CalcLabel"].ToString() == "")
                                {
                                    caltype = Dt.Rows[j]["CalcType"].ToString();
                                }
                                else
                                {
                                    caltype = Dt.Rows[j]["CalcLabel"].ToString();
                                }

                                if (Dt.Rows[j]["CriteriaType"].ToString() == "MOVE UP")
                                {
                                    if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            tdAdvPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            advancePrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                            //advancePrompt += "Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) ;
                                        }
                                        else
                                        {
                                            tdAdvPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            advancePrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                            //advancePrompt += "Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) ;
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            tdAdvSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            advanceSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            tdAdvSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            advanceSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            tdAdvStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            advanceStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            tdAdvStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            advanceStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                }
                                else if (Dt.Rows[j]["CriteriaType"].ToString() == "MOVE DOWN")
                                {
                                    if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            tdMovePrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            tdMovePrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            tdMoveSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            MoveBackSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            tdMoveSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            tdMoveStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            MoveBackStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            tdMoveStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                }

                                //else if (Dt.Rows[j]["CriteriaType"].ToString() == "MODIFICATION")
                                //{
                                //    if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                                //    {
                                //        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                //        {
                                //            tdModPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                //            ModificationPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                //        }
                                //        else
                                //        {
                                //            tdModPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                //            ModificationPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                //        }
                                //    }
                                //    else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                                //    {
                                //        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                //        {
                                //            tdModSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                //            ModificationSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                //        }
                                //        else
                                //        {
                                //            tdModSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                //            ModificationSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                //        }
                                //    }
                                //    else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                                //    {
                                //        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                //        {
                                //            tdModStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                //            ModificationStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                //        }
                                //        else
                                //        {
                                //            tdModStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                //            ModificationStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                //        }
                                //    }



                                //}
                            }
                        }
                    }
                }
            }




            strQuery = " Select DR.RuleType, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DR.CriteriaDetails, " +
        " DR.ScoreReq,DR.ConsequetiveInd,Dt.DSTempHdrId  FROM DSTempHdr Dt INNER JOIN  DSTempRule DR On DR.DSTempHdrId  = Dt.DSTempHdrId " +
        " Where   DR.ActiveInd = 'A' AND DR.CriteriaType='MODIFICATION'  And DR.DSTempHdrId  =" + TempId + "  ";

            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    for (int j = 0; j < Dt.Rows.Count; j++)
                    {
                        if (Dt.Rows[j]["CriteriaType"].ToString() == "MODIFICATION")
                        {
                            if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                            {
                                if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                {
                                    tdModPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                    ModificationPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                                else
                                {
                                    tdModPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                            }
                            else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                            {
                                if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                {
                                    tdModSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                                else
                                {
                                    tdModSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                    ModificationSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                            }
                            else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                            {
                                if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                {
                                    tdModStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                                else
                                {
                                    tdModStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                            }


                        }
                    }
                }
            }
        }



















        Session["Criteria"] = advanceSet + "|" + advanceStep + "|" + advancePrompt + "|" + MoveBackSet + "|" + MoveBackStep + "|" + MoveBackPrompt + "|" + ModificationSet + "|" + ModificationStep + "|" + ModificationPrompt;

    }

    private void FillObjective(string LessonId)
    {
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        objData = new clsData();
        if (sess != null && ObjTempSess != null)
        {
            Dt = new DataTable();
            if (sess != null && ObjTempSess != null)
            {
                strQuery = "select Objective3 from StdtLessonPlan where SchoolId = " + sess.SchoolId + " and StudentId=" + sess.StudentId + " and LessonPlanId=" + LessonId + " and ActiveInd= 'A'";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null) tdObjective.InnerHtml = "<b>Objective: </b>" + objVal.ToString();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
    }

    private void FillTypeOfInstrutions(string TemplateId)
    {
        try
        {
            string strPtBinder = "";
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            Dt = new DataTable();
            if (sess != null && ObjTempSess != null)
            {
                strBinder = "";
                //strQuery = "Select LU.LookupName from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.TeachingProcId  Where DSTempHdrId=" + TemplateId + " And LU.LookupType='Datasheet-Teaching Procedures' ";
                //objVal = objData.FetchValue(strQuery);
                //if (objVal != null) strBinder += objVal.ToString() + " - ";
                string strQuery1 = "Select LU.LookUpName,LU.LookUpCode from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.TeachingProcId  Where DSTempHdrId=" + TemplateId + " And LU.LookupType='Datasheet-Teaching Procedures'";
                Dt = objData.ReturnDataTable(strQuery1, false);
                if (Dt.Rows[0]["LookUpName"].ToString() == "Other" && Dt.Rows[0]["LookUpCode"].ToString() == "Other")
                {
                    strBinder = "Other-Other - ";
                }
                else
                {
                    strQuery = "DECLARE @CHAINTYPE VARCHAR(20) SET @CHAINTYPE = (Select SkillType from DSTempHdr Where DSTempHdrId=" + TemplateId + ") SELECT 	CASE @CHAINTYPE WHEN 'Discrete' THEN 'Discrete Trial'	ELSE 'Task Analysis' END ";
                    objVal = objData.FetchValue(strQuery);

                    if (objVal != null) strBinder += objVal.ToString() + " - ";
                    if (objVal.ToString() == "Task Analysis")
                    {
                        // strQuery = "DECLARE @VAL int SET @VAL = (Select ChainType from DSTempHdr Where DSTempHdrId=" + TemplateId + ") SELECT 	CASE @VAL 		WHEN 1 THEN 'Forward chain' 		WHEN 2 THEN 'Backward chain' WHEN 3 THEN 'Total Task' 	ELSE '' END ";
                        strQuery = "Select ChainType from DSTempHdr Where DSTempHdrId=" + TemplateId + " ";
                        objVal = objData.FetchValue(strQuery);
                        if (objVal.ToString() != null) strBinder += objVal.ToString() + " - ";
                    }
                }
                strQuery = "Select LU.LookupName from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.PromptTypeId  Where DSTempHdrId=" + TemplateId + " And LU.LookupType='Datasheet-Prompt Procedures'";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null)
                {
                    strBinder += objVal.ToString();


                    if ((objVal.ToString() == "Least-to-Most")||(objVal.ToString() == "Graduated Guidance"))
                    {
                        strQuery = "SELECT LU.LookupId as Id,LU.LookupName as Name FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                                                " DS.ActiveInd='A' AND DS.DSTempHdrId=" + TemplateId + " ORDER BY PromptOrder";
                        Dt = objData.ReturnDataTable(strQuery, false);
                    }
                    else
                    {
                        strQuery = "SELECT LU.LookupId as Id,LU.LookupName as Name FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                                    " DS.ActiveInd='A' AND DS.DSTempHdrId=" + TemplateId + " ORDER BY PromptOrder DESC";
                        Dt = objData.ReturnDataTable(strQuery, false);
                    }






                    if (Dt != null)
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < Dt.Rows.Count; j++)
                            {
                                strQuery = "select LookUp.LookupName from LookUp where LookupId=" + Convert.ToInt32(Dt.Rows[j]["id"]) + " ";
                                objVal = objData.FetchValue(strQuery);
                                if (objVal != null) strPtBinder += objVal.ToString() + "<br>";
                            }
                        }
                    }
                }
   
                if (strBinder != null) tdTypeIns.InnerHtml = "<b>Type of Instruction: </b>" + strBinder.ToString() + "<br>" + strPtBinder;
                typeOfInstr = strBinder.ToString();
                Session["typOfInstr"] = typeOfInstr;

            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
            throw Ex;
        }
    }



    protected void btnExport_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=LessonPlanSummary(" + sess.StudentName + ").pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        lessonPrintDiv.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 100f, 0f);
        iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }

    protected void btnExportWord_Click(object sender, EventArgs e)
    {
        ClsErrorLog err = new ClsErrorLog();
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        try
        {
            exportToWord(ObjTempSess.LessonPlanId, ObjTempSess.TemplateId);
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }

    }

    private void exportToWord(int LessonPlanId, int TempId)
    {
        /*----------------------------------------------------------------------------*/

        string Path = "";
        string NewPath = "";

        try
        {
            CreateQuery("NE", "..\\Administration\\LesonPlanTemplate\\LessonPlanXML.xml");
            Path = Server.MapPath("~\\Administration\\LesonPlanTemplate\\LessonPlanSampleTemplate.docx");
            string Path2 = Server.MapPath("~\\Administration\\LessonPlanMerg\\Dummy.docx");

            string path = Server.MapPath("~\\Administration") + "\\TempLessonPlan";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int fileCount = Directory.GetFiles(path).Length + 1;
            string newpathz = Server.MapPath("~\\Administration\\LessonPlanMerg\\");
            string subname = DateTime.Now.Date.ToShortDateString();
            if (subname.Contains('/'))
            {
                subname = subname.Replace('/', '-');
            }
            string newFileName = "Dummy" + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
            ViewState["FileName2"] = newFileName;
            FileInfo f1 = new FileInfo(Path2);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpathz))
                {
                    Directory.CreateDirectory(newpathz);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpathz, newFileName, f1.Extension));
            }

            NewPath = CopyTemplate(Path, "1");
            if (NewPath != "")
            {
                //fillDataLEssonPlan(LessonPlanId, TempId);
                //SearchAndReplace(NewPath);
                Dt = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt4Reason = new DataTable();
                dt4.Columns.Add("Date");
                dt4.Columns.Add("Reason");
                string reason = "";
                string createVal = "";
                string reasonStr = "";
                string createValStr = "";
                try
                {
                    objData = new clsData();
                    int val = 0;
                    Double versionNum = 0.0;
                    DataTable Dt2 = new DataTable();

                    sess = (clsSession)Session["UserSession"];
                    if (sess != null)
                    {

                        strQuery = "select StimulyActivityId,case when(ActivitiType='STARTED') then ('STARTED') ELSE (case when(ActivitiType='SET') then (Select 'SET - '+SetCd from DSTempSet where  DSTempSetId= Act.ActivityId)"
                             + "ELSE (case when(ActivitiType='STEP') then (Select 'STEP - '+StepCd from DSTempStep where  DSTempStepId= Act.ActivityId)"
                             + "ELSE (case when(ActivitiType='MASTERED') then (Select 'SET - '+SetCd+'' from DSTempSet where  DSTempSetId= Act.ActivityId)"
                             + "ELSE(Select 'PROMPT - '+ LookupName from LookUp where  LookupId= Act.ActivityId)END)END)END)END NAME,"
                             + "(Convert(VARCHAR(50), StartTime,110))StartDate,(Convert(VARCHAR(50), DateMastered,110))DateMastered,(Convert(VARCHAR(50), DateClosed,110))DateClosed"
                             + ",Hdr.Reason_New,Hdr.CreatedOn,Hdr.VerNbr,Hdr.DSTempHdrId from StdtSessStimuliActivity ACT INNER JOIN DSTempHdr Hdr ON ACT.DSTempHdrId=Hdr.DSTempHdrId where ACT.StudentId=" + sess.StudentId + " "
                             + "AND Hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE  DSTempHdrId=" + Convert.ToInt32(ObjTempSess.TemplateId) + ")  ORDER BY ACT.DSTempHdrId";
                        Dt = objData.ReturnDataTable(strQuery, false);

                        val = (Dt.Rows.Count) - 1;
                        if (Dt != null)
                        {
                            if (Dt.Rows.Count > 0)
                            {
                                DataRow[] dtRw = Dt.Select("DSTempHdrId =" + ObjTempSess.TemplateId);
                                if (dtRw != null)
                                {
                                    if (dtRw.Length > 0)
                                    {
                                        if ((dtRw[0]["VerNbr"]) != DBNull.Value)
                                        {
                                            versionNum = Convert.ToDouble(dtRw[0]["VerNbr"]);
                                            DataRow[] dtRw2 = Dt.Select("VerNbr <= '" + versionNum + "'OR VerNbr is null "); //May-29-2020 Fix done
                                            //DataRow[] dtRw2 = Dt.Select("VerNbr <=" + versionNum + "OR VerNbr is null "); //May-29-2020 Above Fix for Cannot perform '<=' operation on System.String and System.Int32. in this string
                                            dt3 = dtRw2.CopyToDataTable();
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                            dt4Reason.Columns.Remove("StimulyActivityId");
                                            dt4Reason.Columns.Remove("CreatedOn");
                                            dt4Reason.Columns.Remove("VerNbr");
                                            dt4Reason.Columns.Remove("DSTempHdrId");
                                            dt4Reason.Columns.Remove("Reason_New");
                                            foreach (DataRow dr in dt3.Rows)
                                            {
                                                if (dr["NAME"].ToString() == "STARTED" && dr["Reason_New"].ToString() != "")
                                                {
                                                    reason = "(Reason:" + dr["Reason_New"].ToString() + ")";
                                                    if (reason == "" || reason == "()")
                                                    {
                                                        reason = "(No Reason to display)";
                                                    }
                                                    createVal = dr["CreatedOn"].ToString();
                                                    string[] splitVal = createVal.Split(' ');
                                                    createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                                    DataRow dr4 = dt4.NewRow();
                                                    dr4["Date"] = createVal;
                                                    dr4["Reason"] = reason;
                                                    dt4.Rows.Add(dr4);
                                                }
                                            }
                                           
                                        }
                                        else
                                        {
                                            dt3 = dtRw.CopyToDataTable();
                                            createVal = Dt.Rows[val]["CreatedOn"].ToString();
                                            string[] splitVal = createVal.Split(' ');
                                            createVal = splitVal[0];
                                            if (reason == "")
                                            {
                                                reason = "(Reason: Initial version. No reason to display)";
                                            }
                                            DataRow dr4 = dt4.NewRow();
                                            dr4["Date"] = "";
                                            dr4["Reason"] = reason;
                                            dt4.Rows.Add(dr4);
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                            dt4Reason.Columns.Remove("StimulyActivityId");
                                            dt4Reason.Columns.Remove("CreatedOn");
                                            dt4Reason.Columns.Remove("VerNbr");
                                            dt4Reason.Columns.Remove("DSTempHdrId");
                                            dt4Reason.Columns.Remove("Reason_New");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
                    }
                }
                catch (Exception Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error !!!");
                    throw Ex;
                }
                fillDataLEssonPlan(LessonPlanId, TempId);
                SearchAndReplace(NewPath);

                AppndTableAssmtTool(NewPath, dt4Reason);
                AppendDateAndReason(NewPath, dt4);



                makeWord(NewPath, newpathz);
            }

            tdMsgExport.InnerHtml = "<div class='valid_box' style='width:76%'>Lesson Plan Export Successfully.. .</div>";
            string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '15%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);

        }
        catch (Exception ex)
        {

        }

    }

    public void AppndTableAssmtTool(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tablecounter == 1)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text())));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[3].ToString()))));
                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);
                        trXml.Append(tblCell4);
                        trXml.Append(tblCell5);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tablecounter++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppendDateAndReason(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            Body bod = mainPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {
                if (tablecounter == 2)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        t.Append(trXml);
                    }
                }
                tablecounter++;
            }
            mainPart.Document.Save();
        }
    }

    private void fillDataLEssonPlan(int LessonPlanId, int TempId)
    {
        Dt = new DataTable();
        objData = new clsData();
        string correctRes = "";
        string inCorrecResp = "";
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {

            strQuery = "Select StudentLname +','+StudentFname from  dbo.Student where StudentId =" + sess.StudentId + "";
            objVal = objData.FetchValue(strQuery);
            if (objVal != null) columns[1] = objVal.ToString(); else columns[2] = "";


            if (sess.SchoolId == 1)
            {
                strQuery = "Select TOP 1 CONVERT(VARCHAR, LessonSDate, 101) +' to '+ CONVERT(VARCHAR,LessonEDate,101) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId = " + LessonPlanId + " ORDER BY DSTempHdrId DESC";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null)
                {
                    columns[2] = objVal.ToString();
                }
                else
                {
                    columns[2] = "";
                }
            }
            else if (sess.SchoolId == 2)
            {
                strQuery = "Select TOP 1 CONVERT(VARCHAR, LessonSDate, 101) +' to '+ CONVERT(VARCHAR,LessonEDate,101) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId = " + LessonPlanId + " ORDER BY DSTempHdrId DESC";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null)
                {
                    columns[2] = objVal.ToString();
                }
                else
                {
                    columns[2] = "";
                }
            }
            //if (objVal != null)
            //{
            //    string[] splitDate = objVal.ToString().Split(' ');
            //    string startD = splitDate[0];
            //    string endD = splitDate[2];
            //    columns[2] = Convert.ToDateTime(startD).ToString("MM/dd/yyyy").Replace('-', '/') + " to " + Convert.ToDateTime(endD).ToString("MM/dd/yyyy").Replace('-', '/');
            //}
            //else columns[2] = "";

            strQuery = "SELECT GoalName FROM GoalLPRel glLpRel LEFT JOIN Goal gol ON glLpRel.GoalId = gol.GoalId WHERE  " +
                                                       " glLpRel.LessonPlanId = " + LessonPlanId + " AND glLpRel.ActiveInd = 'A'";
            strBinder = "";
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    string goal = Dr["GoalName"].ToString() + " , ";
                    strBinder += goal;
                }
            }
            columns[0] = strBinder.TrimEnd(',');

                string strQuerychk = "Select LU.LookupName from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.PromptTypeId  Where DSTempHdrId=" + TempId + " And LU.LookupType='Datasheet-Prompt Procedures'";
                objVal = objData.FetchValue(strQuerychk);
                if (objVal != null)
                {
                    if ((objVal.ToString() == "Least-to-Most") || (objVal.ToString() == "Graduated Guidance"))
                    {
                        strBinder = "";
                        strQuery = "select PromptId id from DSTempPrompt where DSTempHdrId=" + TempId + " ORDER BY PromptOrder";            //export selected prompt
                        Dt = objData.ReturnDataTable(strQuery, false);
                    }
                    else
                    {
                        strBinder = "";
                        strQuery = "select PromptId id from DSTempPrompt where DSTempHdrId=" + TempId + " ORDER BY PromptOrder DESC";            //export selected prompt
                        Dt = objData.ReturnDataTable(strQuery, false);
                    }
                }
                else
                {
                    strBinder = "";
                    strQuery = "select PromptId id from DSTempPrompt where DSTempHdrId=" + TempId + "";            //export selected prompt
                    Dt = objData.ReturnDataTable(strQuery, false);
                }

            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    for (int j = 0; j < Dt.Rows.Count; j++)
                    {
                        strQuery = "select LookUp.LookupName from LookUp where LookupId=" + Convert.ToInt32(Dt.Rows[j]["id"]) + " ";
                        objVal = objData.FetchValue(strQuery);
                        if (objVal != null) strBinder += objVal.ToString() + "<w:br/>";
                    }
                }
            }


            columns[7] = Session["typOfInstr"].ToString() + "<w:br/>" + strBinder;

            strQuery = "select DSTemplateName AS LessonPlanName,LessonPlanGoal,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials from DSTempHdr where DSTempHdrId=" + TempId + " ";
            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, true);
            if (Dt.Rows.Count > 0)
            {
                columns[5] = Dt.Rows[0]["FrameandStrand"].ToString();
                columns[6] = Dt.Rows[0]["SpecStandard"].ToString();
                columns[4] = Dt.Rows[0]["SpecEntryPoint"].ToString();
                columns[9] = Dt.Rows[0]["PreReq"].ToString();
                columns[10] = Dt.Rows[0]["Materials"].ToString();
                columns[27] = Dt.Rows[0]["LessonPlanName"].ToString();
                columns[32] = Dt.Rows[0]["LessonPlanGoal"].ToString();


            }
            strQuery = "select TeachingProcId,SkillType,NbrOfTrials,ChainType,PromptTypeId,Baseline,Objective,GeneralProcedure,CorrRespDef,IncorrRespDef,LessonDefInst,"
                + " StudentReadCrita,TeacherRespReadness,ReinforcementProc,CorrectionProc,MajorSetting,MinorSetting,StudIncorrRespDef,StudCorrRespDef,Mistrial,MistrialResponse,TeacherPrepare,StudentPrepare,StudResponse"
                + " from DSTempHdr where DSTempHdrId = " + TempId + "";

            Dt = objData.ReturnDataTable(strQuery, true);
            if (Dt.Rows.Count > 0)
            {
                columns[13] = Dt.Rows[0]["LessonDefInst"].ToString();
                columns[16] = Dt.Rows[0]["ReinforcementProc"].ToString();
                columns[18] = Dt.Rows[0]["CorrectionProc"].ToString();
                columns[8] = Dt.Rows[0]["MajorSetting"].ToString() + " " + Dt.Rows[0]["MinorSetting"].ToString();
                columns[20] = Dt.Rows[0]["MistrialResponse"].ToString();
                columns[19] = Dt.Rows[0]["Mistrial"].ToString();
                columns[14] = Dt.Rows[0]["StudCorrRespDef"].ToString();
                columns[15] = Dt.Rows[0]["StudIncorrRespDef"].ToString();
                columns[26] = Dt.Rows[0]["Baseline"].ToString();
                columns[3] = Dt.Rows[0]["Objective"].ToString();
                columns[28] = Dt.Rows[0]["TeacherPrepare"].ToString();
                columns[29] = Dt.Rows[0]["StudentPrepare"].ToString();
                columns[30] = Dt.Rows[0]["StudResponse"].ToString();
                columns[31] = Dt.Rows[0]["GeneralProcedure"].ToString();

            }


            strQuery = "SELECT CorrRespDesc,InCorrRespDesc FROM DSTempSetCol WHERE DSTempHdrId = " + TempId + " AND ActiveInd = 'A'";
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Dt.Rows[i]["CorrRespDesc"].ToString() != "")
                        {
                            correctRes += Dt.Rows[i]["CorrRespDesc"].ToString() + ",";
                        }
                        if (Dt.Rows[i]["InCorrRespDesc"].ToString() != "")
                        {
                            inCorrecResp += Dt.Rows[i]["InCorrRespDesc"].ToString() + ",";
                        }

                    }
                    correctRes = correctRes.TrimEnd(',');
                    inCorrecResp = inCorrecResp.TrimEnd(',');

                    columns[12] = correctRes.ToString();
                    columns[17] = inCorrecResp.ToString();


                }
                else
                {
                    columns[12] = "No input Data";
                    columns[17] = "No input Data";
                }
            }
            else
            {
                columns[12] = "No input Data";
                columns[17] = "No input Data";
            }

            strBinder = "";
            //strQuery = "select StepName,StepCd from dbo.DSTempStep where DSTempHdrId = " + TempId + " AND ActiveInd = 'A'";
            strQuery = "SELECT StepName,StepCd FROM DSTempParentStep"
                        + " WHERE DSTempHdrId = " + TempId + " And ActiveInd = 'A' ORDER BY DSTempSetId,SortOrder";

            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<w:br/>" + Dr["StepCd"].ToString() + "-" + Dr["StepName"].ToString() + "<w:br/>";
                }
            }
            columns[22] = strBinder.ToString();
            strBinder = "";
            strQuery = "select SetName,SetCd from dbo.DSTempSet where DSTempHdrId = " + TempId + " AND ActiveInd = 'A' order by SortOrder";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<w:br/>" + Dr["SetCd"].ToString() + "-" + Dr["SetName"].ToString() + "<w:br/>";
                }
            }
            columns[21] = strBinder.ToString();


            strBinder = "";
            strQuery = "select ColTypeCd from DSTempSetCol where DSTempHdrId = " + TempId + " and ActiveInd ='A'";
            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, false);

            strBinder = "";
            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<w:br/>" + Dr["ColTypeCd"].ToString() + "<w:br/>";
                }
            }
            columns[11] = strBinder.ToString();

            //strQuery = "select Objective3 from StdtLessonPlan where SchoolId = " + sess.SchoolId + " and StudentId=" + sess.StudentId + " and LessonPlanId=" + LessonPlanId + " and ActiveInd= 'A'";
            //objVal = objData.FetchValue(strQuery);
            //if (objVal != null) columns[3] = objVal.ToString();
            string[] criteria = Session["Criteria"].ToString().Split('|');
            columns[23] = criteria[0].ToString() + "<w:br/>" + criteria[1].ToString() + "<w:br/>" + criteria[2].ToString();
            columns[24] = criteria[3].ToString() + "<w:br/>" + criteria[4].ToString() + "<w:br/>" + criteria[5].ToString();
            columns[25] = criteria[6].ToString() + "<w:br/>" + criteria[7].ToString() + "<w:br/>" + criteria[8].ToString();

            //---[For removing ambersand start (12-May-2020)]---
            int colcount = Convert.ToInt32(columns.Length.ToString());
            for (int icol = 0; icol < colcount; icol++)
            {
                if (columns[icol].Contains("&"))
                {
                    columns[icol] = columns[icol].ToString().Replace("&", " and "); //<==== Ampersand is not readable in xml (12-May-2020) Developer 2
                }
                if (columns[icol].Contains("<"))
                {
                    columns[icol] = columns[icol].ToString().Replace("<", " Less than ");
                }
                if (columns[icol].Contains(">"))
                {
                    columns[icol] = columns[icol].ToString().Replace(">", " Greater than ");
                }
                if (columns[icol].Contains("Less than w:br/ Greater than "))
                {
                    columns[icol] = columns[icol].ToString().Replace("Less than w:br/ Greater than ", " <w:br/> ");
                }
            }
            //---[For removing ambersand end (12-May-2020)]---
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
        }
    }

    private string CopyTemplate(string oldPath, string PageNo)
    {
        try
        {
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            sess = (clsSession)Session["UserSession"];
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\Administration") + "\\TempLessonPlan";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int fileCount = Directory.GetFiles(path).Length + 1;
            string newpath = path + "\\";
            string subname = DateTime.Now.Date.ToShortDateString();
            if (subname.Contains('/'))
            {
                subname = subname.Replace('/', '-');
            }
            string newFileName = "LessonPlan" + "_" + sess.StudentId + "-" + subname + "_" + fileCount;

            if (Request.QueryString["export"] == "true")
            {
                if (Request.QueryString["typeview"] == "false")
                {
                    sess.StudentId = 0;
                    newFileName = "LessonPlan" + "_" + subname + "_" + fileCount;
                }
                else if (Request.QueryString["typeview"] == "true")
                {
                    newFileName = "LessonPlan" + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
                }
            }

            ViewState["FileName"] = newFileName;
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
            }

            //  ViewState["FileName"] = newpath + newFileName + f1.Extension;
            return newpath + newFileName + f1.Extension;
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
        }
    }

    public void downloadfile()
    {
        ClsErrorLog err = new ClsErrorLog();
        try
        {
            string FileName = ViewState["FileName5"].ToString();
            string FileHeader = ViewState["FileName"].ToString() + ".doc";
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileHeader + "\"");
            byte[] data = req.DownloadData(FileName);
            Response.AddHeader("Content-Length", data.Length.ToString());
            response.BinaryWrite(data);
            //response.End();           
            HttpContext.Current.Response.Flush(); 
            HttpContext.Current.Response.SuppressContent = true; 
            HttpContext.Current.ApplicationInstance.CompleteRequest(); 
            ClientScript.RegisterStartupScript(GetType(), "", "HideWait();", true);            
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();


    }

    protected void btnDone_Click(object sender, EventArgs e)
    {
        tdMsgExport.InnerHtml = "";

        string Temp = Server.MapPath("~\\Administration") + "\\TempLessonPlan\\" + ViewState["FileName"] + ".docx";
        FileInfo f1 = new FileInfo(Temp);
        if (f1.Exists)
        {
            f1.Delete();
        }
        Temp = Server.MapPath("~\\Administration\\LessonPlanMerg\\") + ViewState["FileName2"] + ".docx";
        f1 = new FileInfo(Temp);
        if (f1.Exists)
        {
            f1.Delete();
        }

        ClientScript.RegisterStartupScript(GetType(), "", "DownloadDone();", true);

    }

    public void makeWord(string filenamePass, string fileName1)
    {
        ClsErrorLog err = new ClsErrorLog();
        fileName1 += ViewState["FileName2"].ToString() + ".docx";
        try
        {
            using (WordprocessingDocument myDoc =
                WordprocessingDocument.Open(fileName1, true))
            {
                string altChunkId = "AltChunkId1";
                MainDocumentPart mainPart = myDoc.MainDocumentPart;
                AlternativeFormatImportPart chunk =
                    mainPart.AddAlternativeFormatImportPart(
                    AlternativeFormatImportPartType.WordprocessingML, altChunkId);


                using (FileStream fileStream = File.Open(filenamePass, FileMode.Open))
                    chunk.FeedData(fileStream);


                AltChunk altChunk = new AltChunk();
                altChunk.Id = altChunkId;
                mainPart.Document
                    .Body
                    .InsertAfter(altChunk, mainPart.Document.Body
                    .Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last());
                mainPart.Document.Save();

            }
            ViewState["FileName5"] = fileName1;
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
    }

    private void CreateQuery(string StateName, string Path)
    {
        ClsErrorLog err = new ClsErrorLog();
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath(Path));

            XmlNodeList xmlList = null;
            xmlList = xmlDoc.GetElementsByTagName("State");

            foreach (XmlNode st in xmlList)
            {
                if (st.Attributes["Name"].Value == StateName)
                {
                    XmlNodeList xmlListColumns = null;
                    xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                    columns = new string[xmlListColumns.Count];
                    placeHolders = new string[xmlListColumns.Count];




                    int i = 0, j = 0;
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        columns[i] = stMs.Attributes["Column"].Value;
                        i++;
                    }
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        placeHolders[j] = stMs.Attributes["PlaceHolder"].Value;

                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                        }
                        j++;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
    }

    public void SearchAndReplace(string document)
    {
        ClsErrorLog err = new ClsErrorLog();
        int m = 0;

        try
        {
            WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true);



            string docText = null;
            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();

                ;
            }

            string col = "";
            string plc = "";

            columnsCheck = new string[checkCount];


            for (int i = 0; i < columns.Length; i++)
            {
                plc = placeHolders[i].ToString().Trim();
                col = columns[i].ToString().Trim();


                Regex regexText = new Regex(plc);
                docText = regexText.Replace(docText, col);



            }
            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(docText);
            }
            wordDoc.Close();
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }


    }

    public void replaceWithHtml(string fileName, string replace, string replaceTest)
    {
        ClsErrorLog err = new ClsErrorLog();
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            //     converter.ConsiderDivAsParagraph = false;

            //     SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            //paragraphProperties.Append(spacing);

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();





            if (replaceTest == "") replaceTest = "&nbsp;";
            if (replaceTest != "") replaceTest = replaceTest.Trim();
            try
            {
                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();

                var paragraphs = converter.Parse(replaceTest);



                for (int i = 0; i < paragraphs.Count; i++)
                {
                    var parent = placeholder.Parent;
                    paragraphs[i].Append(paragraphProperties);
                    parent.ReplaceChild(paragraphs[i], placeholder);
                }
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + eX.ToString());
                tdMsg.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
            }


        }


    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {

    }

    #endregion
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadData();
    }
}

