using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;

using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Net;


public partial class StudentBinder_AsmntReview : System.Web.UI.Page
{
    clsData oData;
    clsSession oSession;
    DataClass objData = null;
    clsSession Prevsess;

    string curesesid = "";
    int preid = 0;
    string preuser = "";
    int SessStudentid = 0;
    string Sessstname = "";
    string Pagepath = "";
    string ip = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        oSession = (clsSession)Session["UserSession"];
        if (oSession == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(oSession.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        if (!IsPostBack)
        {
            ShowAllLPs("");
            LoadData();
            
        }
        Prevsess = (clsSession)Session["PreSession"];
        preid = Convert.ToInt16(Session["Spreid"]);
        preuser = Session["Spreuser"].ToString();
        SessStudentid = Convert.ToInt16(Session["Sprestid"]);
        Sessstname = Session["Sprestname"].ToString();
        curesesid = this.Session.SessionID.ToString();
        string HostName = Dns.GetHostName();
        string userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "AsmntReview:Page_Load";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + "" + ',' + "");
        }
        oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);
       
    }
    
  
    private void LoadData()
    {
        ul_selGoals.InnerHtml = "";
        LoadSource();
        LoadAssignLPs();
        loadLessonplan();
        oData = new clsData();
        oData.ReturnDropDown("SELECT GoalId as Id,GoalCode as Name FROM Goal Gl INNER JOIN GoalType GT ON GT.GoalTypeId=Gl.GoalTypeId WHERE GoalTypeName='Academic Goals' and Gl.ActiveInd='A'", ddlGoals);
    }


    private void loadLessonplan()
    {

        try
        {
            clsData oData = new clsData();
            oSession = (clsSession)Session["UserSession"];
            string strqry = "SELECT SLP.StdtLessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,SLP.CreatedOn,SLP.ModifiedOn,GL.GoalName from LessonPlan LP " +
                            "inner join StdtLessonPlan SLP on LP.LessonPlanId=SLP.LessonPlanId INNER JOIN Goal GL ON SLP.goalid = GL.goalid where SLP.ActiveInd='D' AND SLP.StudentId =" + oSession.StudentId;

            DataTable dt = oData.ReturnDataTable(strqry, false);
            grdLessonPlanDelete.DataSource = dt;
            grdLessonPlanDelete.DataBind();



        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    /// <summary>
    /// function which finds all the assessmnts associated with the student and populate in the source section.....
    /// </summary>
    protected void LoadSource()
    {
        oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
        try
        {
            if (oSession != null)
            {
                //query to find all the latest assessments of the selected student....
                string selAsmnt = "SELECT * FROM(" +
                            "SELECT Asmnt.AsmntId,Asmnt.AsmntName,Asmnt.AsmntStartTs,Usr.UserLName+','+Usr.UserFName as name," +
                            "CASE WHEN DATEDIFF(dd,Asmnt.AsmntStartTs,CONVERT(VARCHAR(10), GETDATE(), 120))>180 THEN 'Expired' " +
                            "WHEN Asmnt.AsmntStartTs IS NULL THEN 'Expired' ELSE 'Not Expired' END as Status," +
                            "RANK() OVER (PARTITION BY Asmnt.AsmntName ORDER BY Asmnt.AsmntStartTs DESC) AS Rank " +
                            "FROM (Assessment Asmnt INNER JOIN [User] Usr ON Usr.UserId=Asmnt.CreatedBy) " +
                            "INNER JOIN LookUp LU " +
                            "ON LU.LookupId=Asmnt.AsmntStatusId " +
                            "WHERE (Asmnt.StudentId=" + oSession.StudentId + ") AND Asmnt.SchoolId=" + oSession.SchoolId + " " +
                            "AND (Asmnt.AsmntTyp='By Assessment') " +
                            "AND (LU.LookupName='Complete') " +
                            "AND Asmnt.ActiveInd='A' " +
                            "GROUP BY Asmnt.AsmntName,Asmnt.AsmntId,Asmnt.AsmntStartTs,UserLName,UserFName) A " +
                            "WHERE Rank=1";
                DataTable dtAsmnts = oData.ReturnDataTable(selAsmnt, false);

                dlAsmnts.DataSource = dtAsmnts;
                dlAsmnts.DataBind();


                //query to find all the assessments of the selected student skillswise....
                string selSkills = "SELECT * FROM(" +
                            "SELECT Asmnt.AsmntId,Asmnt.AsmntName as GoalName,Asmnt.AsmntStartTs,Usr.UserLName+','+Usr.UserFName as name," +
                            "RANK() OVER (PARTITION BY Asmnt.AsmntName ORDER BY Asmnt.AsmntStartTs DESC) AS Rank " +
                            "FROM (Assessment Asmnt INNER JOIN [User] Usr ON Usr.UserId=Asmnt.CreatedBy) " +
                            "INNER JOIN LookUp LU " +
                            "ON LU.LookupId=Asmnt.AsmntStatusId " +
                            "WHERE (Asmnt.StudentId=" + oSession.StudentId + ") AND Asmnt.SchoolId=" + oSession.SchoolId + " " +
                            "AND (Asmnt.AsmntTyp='By Skill') " +
                            "AND (LU.LookupName='Complete') " +
                            "AND Asmnt.ActiveInd='A'" +
                            "GROUP BY Asmnt.AsmntName,Asmnt.AsmntId,Asmnt.AsmntStartTs,UserLName,UserFName) A " +
                            "WHERE Rank=1";

                dlSkills.DataSource = oData.ReturnDataTable(selSkills, false);
                dlSkills.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    /// <summary>
    /// function to generate goals and LPs and populate in the Choose Goals and LPs section.....
    /// </summary>
    /// <param name="asmntIDs">all the selected assessments and skills id</param>
    /// <param name="min">min percent value</param>
    /// <param name="max">max percent value</param>
    /// <param name="keywrd">search keyword for LP</param>
    protected void LoadGoalAndLPs(string asmntIDs, double min, double max, string keywrd)  //GoalCode is used to display the Edited goals
    {
        try
        {
            oSession = (clsSession)Session["UserSession"];

            curesesid = this.Session.SessionID.ToString();
            string HostName = Dns.GetHostName();
            string userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AsmntReview:LoadGoalAndLPs";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
            }
            oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);
           
            ul_Goals.InnerHtml = "";
            if (oSession != null)
            {
                string stdtAssmntIDs = asmntIDs;
                string selQry = "SELECT Stg.GoalName,Stg.GoalCode, " +
                                "COUNT(CASE WHEN AsmntScr <> TotScr THEN NULL ELSE Stg.AsmntQId END) as LPCount, " +
                                "COUNT(Stg.AsmntQId) as TotalLP," +
                                "(COUNT(CASE WHEN AsmntScr <> TotScr THEN NULL ELSE Stg.AsmntQId END)*100/COUNT(Stg.AsmntQId)) as Percentage " +
                                "FROM StdtLPStg Stg INNER JOIN " +
                                "(select AssessmentId," +
                                "AsmntName," +
                                "GoalName," +
                                "MAX(CreatedOn)CreatedOn," +
                                "ROW_NUMBER() over (partition by AsmntName,GoalName order by MAX(CreatedOn) DESC) frow " +
                                "FROM StdtLPStg " +
                                "WHERE (" + stdtAssmntIDs + ") " +
                                "AND StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " " +
                                "group by AssessmentId,AsmntName,GoalName ) AS TStg " +
                                "ON TStg.AssessmentId = Stg.AssessmentId " +
                                "AND TStg.AsmntName=Stg.AsmntName " +
                                "AND TStg.GoalName = Stg.GoalName " +
                                "WHERE frow = 1 " +
                                "GROUP BY Stg.GoalName,Stg.GoalCode " +
                                "ORDER BY Percentage DESC ";            // query which finds all the goals and its percentage of score from the given assessments.....
                oData = new clsData();
                DataTable dtGoals = oData.ReturnDataTable(selQry, false);
                foreach (DataRow drGoal in dtGoals.Rows)
                {
                    object objGoalId = oData.FetchValue("SELECT GoalId FROM Goal WHERE GoalName='" + drGoal["GoalName"].ToString() + "'");
                    string goalid = "";
                    if (objGoalId != null)
                        goalid = objGoalId.ToString();
                    string goalname = drGoal["GoalName"].ToString();
                    //generate li tags for goals.....
                    //ul_Goals.InnerHtml += "<li style='position: static;' class='accordion'>" +
                    //                                "<h2 class='BG' onclick='h2click(this);'><a id='a_" + goalid + "' class='kk' href='#' onclick='AssignGoal(this.id,this.innerHTML,event);'>" + goalname + "</a></h2>" +
                    //                                "<div style='display: none;' class='wrapper nomar'>" +
                    //                                    "<div class='nobdrrcontainer'>";
                    string conditn = "WHERE ROW_NUM = 1 AND RStg.GoalName='" + goalname + "' ";
                    if (keywrd.Length > 0)
                        conditn = "WHERE ROW_NUM = 1 AND RStg.GoalName='" + goalname + "' AND Lp.LessonPlanName LIKE '%" + keywrd + "%' ";
                    //query which finds all the LPs and its percentage of score from the given assessments....
                    string selLPs = "SELECT Stg.LessonPlanId,Lp.LessonPlanName,LP.LessonPlanDesc, COUNT(CASE WHEN AsmntScr <> TotScr THEN NULL ELSE AsmntQId END) as QCount,COUNT(AsmntQId) as TotalQues, " +
                                    "(COUNT(CASE WHEN AsmntScr <> TotScr THEN NULL ELSE AsmntQId END)*100/COUNT(AsmntQId)) as Percentage " +
                                    "FROM StdtLPStg Stg INNER JOIN (select AssessmentId,AsmntName,GoalName," +
                                    "ROW_NUMBER() over (partition by AsmntName,GoalName order by MAX(CreatedOn) DESC) AS ROW_NUM " +
                                    "FROM stdtlpstg " +
                                    "WHERE (" + stdtAssmntIDs + ") AND StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " " +
                                    "group by AssessmentId,AsmntName,GoalName) AS RStg " +
                                    "ON RStg.AssessmentId = Stg.AssessmentId " +
                                    "AND RStg.AsmntName = Stg.AsmntName " +
                                    "AND RStg.GoalName = Stg.GoalName " +
                                    "INNER JOIN LessonPlan LP " +
                                    "ON Stg.LessonPlanId = LP.LessonPlanId " + conditn +
                                    "GROUP BY Lp.LessonPlanName,Lp.LessonPlanDesc,Stg.LessonPlanId " +
                                    "ORDER BY Lp.LessonPlanName,Percentage ASC";

                    DataTable dtLPs = oData.ReturnDataTable(selLPs, false);

                    if (dtLPs != null)
                    {
                        if (dtLPs.Rows.Count > 0)
                        {
                            ul_Goals.InnerHtml += "<li style='position: static;' class='accordion'>" +
                                              "<h2 class='BG' onclick='h2click(this);'><a id='a_" + goalid + "' class='kk' href='#' onclick='AssignGoal(this.id,this.innerHTML,event);'>" + goalname + "</a></h2>" +
                                              "<div style='display: none;' class='wrapper nomar'>" +
                                                  "<div class='nobdrrcontainer'>";
                        }
                    }
                    //generate html tags for the LPs.....
                    foreach (DataRow drLP in dtLPs.Rows)
                    {
                        if ((Convert.ToDouble(drLP["Percentage"]) >= min) && (Convert.ToDouble(drLP["Percentage"]) <= max))
                        {
                            //this loop is to find the position for the image based on the percentage of LP.....
                            int pos = 147;
                            double[] limits = { 0, 12.51, 25.01, 37.51, 50.01, 62.51, 75.01, 87.51, 100.01 };
                            for (int count = 0; count < 8; count++)
                            {
                                if ((Convert.ToDouble(drLP["Percentage"]) >= limits[count]) && (Convert.ToDouble(drLP["Percentage"]) < limits[count + 1]))
                                {
                                    pos = 1 + (pos - (21 * count));
                                    break;
                                }
                            }//
                            string glname = "&apos;" + goalname + "&apos;";
                            string LPdesc = "&apos;" + drLP["LessonPlanDesc"].ToString() + "&apos;";
                            //ul_Goals.InnerHtml += "<a href='#' id='" + drLP["LessonPlanId"].ToString() + "' class='grmb' onclick='AssignGoalAndLPs(this.id,this.firstChild.textContent," + goalid + "," + glname + "," + LPdesc + ");'>" +
                            //    "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis; overflow: hidden; width:56%;' title='" + drLP["LessonPlanName"].ToString() + "'>" + drLP["LessonPlanName"].ToString() +
                            //    "</div><div class='grapgContainer'><span style='background-position:-" + pos + "px;'></span><p>" + drLP["Percentage"].ToString() + "%</p></div></a>";

                            ul_Goals.InnerHtml += "<div class='grmb'><a style='width:100px;' href='#' id='" + drLP["LessonPlanId"].ToString() + "'  onclick='AssignGoalAndLPs(this.id,this.firstChild.textContent," + goalid + "," + glname + "," + LPdesc + ");'>" +
                               "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis; overflow: hidden;' title='" + drLP["LessonPlanName"].ToString() + "'>" + drLP["LessonPlanName"].ToString() +
                               "</div></a><img id='" + drLP["LessonPlanId"].ToString() + "' onclick='LoadLessonView(this.id," + goalid + ");' src='images/transbtn.png' height='15px' width='15px' style='float:right;margin:0 10px 0 0;' /><div class='grapgContainer'>" +
                               "<span style='background-position:-" + pos + "px;'></span><p>" + drLP["Percentage"].ToString() + "%</p></div></div>";

                        }
                    }
                    //closing the li tag....
                    ul_Goals.InnerHtml += "<div class='clear'></div>" +
                                         "</div>" +
                                         "<div class='clear'></div>" +
                                        "</div>" +
                                       "</li>";

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    /// <summary>
    /// function to load and fill all the goals and LPs of the current student from the database.....
    /// </summary>
    protected void LoadAssignLPs()
    {
        try
        {
            oData = new clsData();
            oSession = (clsSession)Session["UserSession"];
            bool isDay = false;
            string selQry = "";
            if (oSession != null)
            {

                //query to check the current IEP status of the student.....
                if (oSession.SchoolId == 2)

                    selQry = "SELECT StdtIEP_PEId FROM(SELECT IEP.StdtIEP_PEId,LookupName,IEP.EffStartDate,RANK() OVER " +
                                   "(PARTITION BY StudentId " +
                                   "ORDER BY CASE(LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' " +
                                   "THEN 2 WHEN 'Approved' THEN 3 END,IEP.EffStartDate DESC) as rank " +
                                   "FROM (StdtIEP_PE IEP INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                                   "INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId " +
                                   "WHERE StudentId=" + oSession.StudentId + " AND IEP.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' AND LookupName<>'Expired') A WHERE rank=1";
                else
                    selQry = "SELECT StdtIEPId FROM(SELECT IEP.StdtIEPId,LookupName,IEP.EffStartDate,RANK() OVER " +
                                   "(PARTITION BY StudentId " +
                                   "ORDER BY CASE(LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' " +
                                   "THEN 2 WHEN 'Approved' THEN 3 END,IEP.EffStartDate DESC) as rank " +
                                   "FROM (StdtIEP IEP INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                                   "INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId " +
                                   "WHERE StudentId=" + oSession.StudentId + " AND IEP.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' AND LookupName<>'Expired') A WHERE rank=1";

                object obj_IEP = oData.FetchValue(selQry);
                if (obj_IEP == null) obj_IEP = "0";
                string nonIEPGoals = "";
                //if (obj_IEP != null)updateLessonPlan
                {
                    //query to find all the goals of the student, only if iep not null.....
                    //string selGoals = "SELECT StdtGoalId,StdGl.GoalId,GoalCode,StdGl.IncludeIEP FROM StdtGoal StdGl INNER JOIN Goal Gl ON Gl.GoalId=StdGl.GoalId " +
                    //                "WHERE StdGl.StudentId=" + oSession.StudentId + " AND StdGl.ActiveInd='A' AND StdGl.SchoolId=" + oSession.SchoolId + " " +
                    //                "AND (StdGl.IncludeIEP=0 OR (StdGl.IncludeIEP=1 AND StdGl.StdtIEPId=" + obj_IEP.ToString() + "))";

                    //string selGoals = "SELECT StdtGoalId,StdGl.GoalId,GoalCode,StdGl.IncludeIEP FROM StdtGoal StdGl INNER JOIN Goal Gl ON Gl.GoalId=StdGl.GoalId WHERE " +
                    //                " StdGl.StudentId=" + oSession.StudentId + " AND StdGl.ActiveInd='A' AND StdGl.SchoolId=" + oSession.SchoolId + " ORDER BY GoalId";
                    //" SELECT StdtGoalId,StdGl.GoalId,GoalCode,StdGl.IncludeIEP FROM StdtGoal StdGl INNER JOIN Goal Gl ON Gl.GoalId=StdGl.GoalId WHERE " +
                    //" StdGl.StudentId="+oSession.StudentId+" AND StdGl.ActiveInd='A' AND StdGl.SchoolId="+oSession.SchoolId+" AND StdGl.IncludeIEP=0 AND StdGl.GoalId NOT IN (SELECT StdGl.GoalId " +
                    //" FROM StdtGoal StdGl INNER JOIN Goal Gl ON Gl.GoalId=StdGl.GoalId WHERE StdGl.StudentId="+oSession.StudentId+" AND StdGl.ActiveInd='A' AND StdGl.SchoolId="+oSession.SchoolId+" " +
                    //" AND ((StdGl.IncludeIEP=1 AND StdGl.StdtIEPId=" + obj_IEP.ToString() + "))) ORDER BY GoalId";

                    string selGoals = "select DISTINCT (SELECT GoalCode FROM Goal GL WHERE GL.GoalId=GOL.GoalId) GoalCode,GoalId,(SELECT TOP 1 StdtGoalId FROM StdtGoal SGOL WHERE SGOL.StudentId=GOL.StudentId AND SGOL.GoalId=GOL.GoalId " +
                                        "AND SGOL.ActiveInd='A' ORDER BY SGOL.StdtGoalId DESC) StdtGoalId,(SELECT TOP 1 IncludeIEP FROM StdtGoal SGOL WHERE SGOL.StudentId=GOL.StudentId AND SGOL.GoalId=GOL.GoalId " +
                                        "AND SGOL.ActiveInd='A' ORDER BY SGOL.StdtGoalId DESC) IncludeIEP from stdtgoal GOL where GOL.ActiveInd='A' and GOL.StudentId=" + oSession.StudentId + "  AND GOL.SchoolId=" + oSession.SchoolId + " ORDER BY GoalId";


                    DataTable dtGoals = oData.ReturnDataTable(selGoals, false);
                    //loop to find all the LPs associated with the goal and fill it under the goal......
                    foreach (DataRow drGl in dtGoals.Rows)
                    {
                        HtmlGenericControl cntrl_li = (HtmlGenericControl)ul_selGoals.FindControl("li_" + drGl["GoalId"].ToString());
                        if (cntrl_li == null)
                        {
                            nonIEPGoals += "AND StdLP.GoalId<>" + drGl["GoalId"].ToString();
                            string chkbox = "<input name='' class='rdo' type='checkbox' value='' onclick='updateIEP(this,this.parentNode.parentNode.id,0,&apos;Goal&apos;,event);' />";
                            if (drGl["IncludeIEP"].ToString() == "True")    //if IncludeIEp is true make the checkbox's checked to true....
                            {
                                chkbox = "<input name='' class='rdo' checked='true' type='checkbox' value='' onclick='updateIEP(this,this.parentNode.parentNode.id,0,&apos;Goal&apos;,event);' />";
                            }
                            string tags = "<li id='li_" + drGl["GoalId"].ToString() + "' style='position: static;' class='accordion'>" +
                                 "<h2 class='' onclick='h2click(this);'><span class='dd'></span><a href='#our-name'>" + drGl["GoalCode"].ToString() + "</a><div id=&apos;" + drGl["StdtGoalId"].ToString() + "&apos; class='container'>" +
                                 "<span><img width='15px' height='7px' style='position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;' src='images/load.gif'>" + chkbox + "Included in IEP</span><a href='#' class='dlt' onclick='deleteLPandGoal(this.parentNode,this.parentNode.id,&apos;Goal&apos;,event);'></a></div></h2>" +
                                 "<div style='display: none;background-color:#E1E1E1;border-radius:0px 0px 7px 7px;' class='wrapper'>";
                            //"<img width='16px' height='15px' title='Refresh' style='padding-left: 97%;' src='images/arrows_refresh.png' onclick='refreshGoal(this);'>";
                            ul_selGoals.InnerHtml += tags;
                            //query to find all the LPs associated with the goal.....
                            //string selLPs = "SELECT StdtLessonPlanId,StdLP.LessonPlanId,LessonPlanName,LessonPlanDesc,StdLP.GoalId,GoalName,IncludeIEP,StdLP.LessonPlanTypeDay,StdLP.LessonPlanTypeResi " +
                            //        "FROM (StdtLessonPlan StdLP INNER JOIN Goal Gl ON Gl.GoalId=StdLP.GoalId) " +
                            //        "INNER JOIN LessonPlan LP ON LP.LessonPlanId=StdLP.LessonPlanId " +
                            //        "WHERE StudentId=" + oSession.StudentId + " AND StdLP.ActiveInd='A' AND StdLP.SchoolId=" + oSession.SchoolId + " " +
                            //        "AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + obj_IEP.ToString() + ")) AND StdLP.GoalId=" + drGl["GoalId"].ToString() + " ORDER BY LessonPlanName";


                            //string selLPs = "SELECT StdtLessonPlanId,StdLP.LessonPlanId,LessonPlanName,LessonPlanDesc,StdLP.GoalId,GoalName,IncludeIEP,StdLP.LessonPlanTypeDay, "+
                            //                " StdLP.LessonPlanTypeResi FROM (StdtLessonPlan StdLP INNER JOIN Goal Gl ON Gl.GoalId=StdLP.GoalId) INNER JOIN LessonPlan LP ON "+
                            //                " LP.LessonPlanId=StdLP.LessonPlanId WHERE StudentId="+oSession.StudentId+" AND StdLP.ActiveInd='A' AND StdLP.SchoolId="+oSession.SchoolId+" AND ( (IncludeIEP=1 "+
                            //                " AND StdtIEPId=" + obj_IEP.ToString() + ")) AND StdLP.GoalId=" + drGl["GoalId"].ToString() + " UNION SELECT StdtLessonPlanId,StdLP.LessonPlanId,LessonPlanName,LessonPlanDesc, " +
                            //                " StdLP.GoalId,GoalName,IncludeIEP,StdLP.LessonPlanTypeDay,StdLP.LessonPlanTypeResi FROM (StdtLessonPlan StdLP INNER JOIN Goal "+ 
                            //                " Gl ON Gl.GoalId=StdLP.GoalId) INNER JOIN LessonPlan LP ON LP.LessonPlanId=StdLP.LessonPlanId WHERE StudentId="+oSession.StudentId+" AND StdLP.ActiveInd='A' "+
                            //                " AND StdLP.SchoolId=" + oSession.SchoolId + " AND IncludeIEP=0 AND StdLP.GoalId=" + drGl["GoalId"].ToString() + " AND StdLP.LessonPlanId NOT IN (SELECT StdLP.LessonPlanId FROM (StdtLessonPlan " +
                            //                " StdLP INNER JOIN Goal Gl ON Gl.GoalId=StdLP.GoalId) INNER JOIN LessonPlan LP ON LP.LessonPlanId=StdLP.LessonPlanId WHERE StudentId="+oSession.StudentId+" AND "+
                            //                " StdLP.ActiveInd='A' AND StdLP.SchoolId=" + oSession.SchoolId + " AND ( (IncludeIEP=1 AND StdtIEPId=" + obj_IEP.ToString() + ")) AND StdLP.GoalId=" + drGl["GoalId"].ToString() + ") ORDER BY LessonPlanName ";

                            /// -------------LP name from DSTempHdr------------------------------

                            //string selLPs = "SELECT DISTINCT SLP.LessonPlanId,SLP.GoalId,LP.LessonPlanName,LP.LessonPlanDesc,GL.GoalName," +
                            //                "(SELECT TOP 1 StdtLessonPlanId FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                            //                   "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) StdtLessonPlanId," +
                            //                    "(SELECT TOP 1 IncludeIEP FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                            //                    "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) IncludeIEP," +
                            //                    "(SELECT TOP 1 LessonPlanTypeDay FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                            //                    "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) LessonPlanTypeDay," +
                            //                    "(SELECT TOP 1 LessonPlanTypeResi FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                            //                     "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) LessonPlanTypeResi, " +
                            //                     "(SELECT TOP 1 LessonOrder FROM DSTempHdr HDR where HDR.LessonPlanId=SLP.LessonPlanId AND HDR.StudentId=" + oSession.StudentId + ") LessonOrder " +
                            //                     "FROM StdtLessonPlan SLP INNER JOIN LessonPlan LP ON " +
                            //                       "SLP.LessonPlanId=LP.LessonPlanId INNER JOIN Goal GL ON GL.GoalId=SLP.GoalId WHERE SLP.StudentId=" + oSession.StudentId + " AND SLP.ActiveInd='A' AND SLP.GoalId=" + drGl["GoalId"].ToString() + " ORDER BY LessonOrder";


                            string strReplace = "StatusId FROM DSTempHdr DSTH WHERE DSTH.StudentId=" + oSession.StudentId + " AND DSTH.LessonPlanId=SLP.LessonPlanId AND DSTH.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' ";
                            string strReplace2 = "DSTemplateName FROM DSTempHdr DSTH WHERE DSTH.StudentId=" + oSession.StudentId + "  AND DSTH.LessonPlanId=SLP.LessonPlanId AND DSTH.StatusId = (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus'";

                            string selLPs = "SELECT DISTINCT SLP.LessonPlanId,SLP.GoalId," +
                                "(SELECT CASE WHEN (SELECT " + strReplace + " AND Look.LookupName='Approved')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND Look.LookupName='Approved')) " +
                                "ELSE CASE WHEN (SELECT " + strReplace + " AND Look.LookupName='Pending Approval')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND Look.LookupName='Pending Approval')) " +
                                "ELSE CASE WHEN (SELECT " + strReplace + " AND Look.LookupName='In Progress')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND Look.LookupName='In Progress')) " +
                                "ELSE CASE WHEN (SELECT " + strReplace + " AND Look.LookupName='Maintenance')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND Look.LookupName='Maintenance')) " +
                                //"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Expired')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Expired') ORDER BY DSTempHdrId DESC) " +
                                //"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Inactive')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Inactive') ORDER BY DSTempHdrId DESC) END END "+
                                "END END END END) AS LessonPlanName, LP.LessonPlanDesc,GL.GoalName," +
                                "(SELECT TOP 1 StdtLessonPlanId FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                                "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) StdtLessonPlanId," +
                                "(SELECT TOP 1 IncludeIEP FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                                "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) IncludeIEP," +
                                "(SELECT TOP 1 LessonPlanTypeDay FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                                "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) LessonPlanTypeDay," +
                                "(SELECT TOP 1 LessonPlanTypeResi FROM StdtLessonPlan STLP WHERE STLP.LessonPlanId=SLP.LessonPlanId AND STLP.ActiveInd='A'" +
                                "AND STLP.GoalId=SLP.GoalId AND STLP.StudentId=" + oSession.StudentId + " ORDER BY STLP.StdtLessonPlanId DESC) LessonPlanTypeResi, " +
                                "(SELECT TOP 1 LessonOrder FROM DSTempHdr HDR where HDR.LessonPlanId=SLP.LessonPlanId AND HDR.StudentId=" + oSession.StudentId + ") LessonOrder " +
                                "FROM StdtLessonPlan SLP INNER JOIN LessonPlan LP ON SLP.LessonPlanId=LP.LessonPlanId INNER JOIN Goal GL ON GL.GoalId=SLP.GoalId " +
                                "WHERE SLP.StudentId=" + oSession.StudentId + " AND SLP.ActiveInd='A' AND SLP.GoalId=" + drGl["GoalId"].ToString() + " AND " +
                                //"((select COUNT(LessonPlanId) FROM StdtLessonPlan where ActiveInd='D' AND LessonplanId=SLP.lessonplanid AND StudentId=" + oSession.StudentId + ")=0 OR (select COUNT(StatusId) " +
                                "((select COUNT(StatusId) " +
                                "FROM DSTempHdr DSTH WHERE DSTH.LessonPlanId = SLP.lessonplanid AND DSTH.StudentId=" + oSession.StudentId + " AND DSTH.StatusId IN (SELECT LookupId FROM LookUp Look WHERE Look.LookupType='TemplateStatus' " +
                                "AND Look.LookupName IN ('In Progress', 'Pending Approval', 'Approved', 'Maintenance')))>0) ORDER BY LessonOrder";
                            ///end
                            ///
                            DataTable dtLPs = oData.ReturnDataTable(selLPs, false);
                            if (dtLPs != null)
                            {
                                foreach (DataRow drLP in dtLPs.Rows)
                                {
                                    string LPchkbx = "<input name='' class='rdo' type='checkbox' value='' onclick='updateIEP(this,this.parentNode.parentNode.id," + drLP["LessonPlanId"].ToString() + ",&apos;LP&apos;,event);' />";
                                    string LPchkbxDay = "<input name='Day' class='rdoDay' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + drLP["LessonPlanId"].ToString() + ",&apos;LP&apos;,event);' />";
                                    string LPchkbxResi = "<input name='Resi' class='rdoResi' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + drLP["LessonPlanId"].ToString() + ",&apos;LP&apos;,event);' />";

                                    if (drLP["IncludeIEP"].ToString() == "True")    //if IncludeIEp is true make the checkbox's checked to true....
                                    {
                                        LPchkbx = "<input name='' class='rdo' checked='true' type='checkbox' value='' onclick='updateIEP(this,this.parentNode.parentNode.id," + drLP["LessonPlanId"].ToString() + ",&apos;LP&apos;,event);' />";
                                    }

                                    if (drLP["LessonPlanTypeDay"].ToString() == "1")    //if LessonPlanTypeDay is 1 make the checkbox's checked to true....
                                    {
                                        LPchkbxDay = "<input name='Day' class='rdoDay' checked='true' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + drLP["LessonPlanId"].ToString() + ",&apos;LP&apos;,event);' />";
                                    }
                                    if (drLP["LessonPlanTypeResi"].ToString() == "1")    //if LessonPlanTypeResi is 1 make the checkbox's checked to true....
                                    {
                                        LPchkbxResi = "<input name='Resi' class='rdoResi' checked='true' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + drLP["LessonPlanId"].ToString() + ",&apos;LP&apos;,event);' />";
                                    }

                                    string LPtags = "<div id='div_" + drLP["LessonPlanId"].ToString() + "' class='ingrContainer'><h3>" + drLP["LessonPlanName"].ToString() + "</h3><div id=&apos;" + drLP["StdtLessonPlanId"].ToString() + "&apos; class='container'>" +
                                             "<span style='float:left;'><img width='15px' height='7px' style='position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;' src='images/load.gif'>" + LPchkbxDay + "Day </span>" +
                                             "<span style='float:left;'><img width='15px' height='7px' style='position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;' src='images/load.gif'> " + LPchkbxResi + "Residential </span>" +
                                             "<span><img width='15px' height='7px' style='position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;' src='images/load.gif'>" + LPchkbx + "Included in IEP</span></div><br clear='all' />" +//<a href='#' class='dlt' onclick='deleteLPandGoal(this,this.parentNode.id,&apos;LP&apos;,event);'></a>
                                             "<p>" + drLP["LessonPlanDesc"].ToString() + " </p>" +
                                             "</div>";
                                    ul_selGoals.InnerHtml += LPtags;
                                }
                            }
                            ul_selGoals.InnerHtml += "</div></li>";
                        }

                    }


                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    /// <summary>
    /// Function which fills the 'Choose Goals and Lessons' section based on the selected asmnts and skills.....
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string asmntIDs = "";
            foreach (DataListItem dli in dlAsmnts.Items)
            {
                HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                if (chk.Checked == true)    //check whether the assessment is selected or not
                {
                    asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                }
            }
            foreach (DataListItem dli in dlSkills.Items)
            {
                HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                if (chk.Checked == true)    //check whether the skill is selected or not
                {
                    asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                }
            }
            if (asmntIDs.Length > 4)
            {
                asmntIDs = asmntIDs.Substring(0, asmntIDs.Length - 4);
                LoadGoalAndLPs(asmntIDs, 0, 100, "");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void dlAsmnts_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //HiddenField hfStat = (HiddenField)e.Item.FindControl("hfStat");
        //if (hfStat.Value == "Expired")
        //{
        //    LinkButton lbAsmnt = (LinkButton)e.Item.FindControl("lbasmnt");
        //    lbAsmnt.CssClass = "grb";
        //    HtmlInputCheckBox chk = (HtmlInputCheckBox)e.Item.FindControl("chkAsmnt");
        //    chk.Disabled = true;
        //}
    }
    /// <summary>
    /// Search button click..
    /// This function find all the LPs that matches with the search keyword from the selected assessments
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string asmntIDs = "";
            foreach (DataListItem dli in dlAsmnts.Items)
            {
                HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                if (chk.Checked == true)
                {
                    asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                }
            }
            foreach (DataListItem dli in dlSkills.Items)
            {
                HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                if (chk.Checked == true)
                {
                    asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                }
            }
            if (asmntIDs.Length > 4)
            {
                asmntIDs = asmntIDs.Substring(0, asmntIDs.Length - 4);
                LoadGoalAndLPs(asmntIDs, 0, 100, clsGeneral.convertQuotes(txtSearch.Value.Trim()));
            }
            else
            {
                ul_Goals.InnerHtml = "";
                ShowAllLPs(clsGeneral.convertQuotes(txtSearch.Value.Trim()));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Percent Button click...
    /// function which finds all the LPs with Percent in between the minPercent and maxPercent from the selected assessments.....
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPercnt_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkSearch.Checked == true)
            {
                string asmntIDs = "";
                foreach (DataListItem dli in dlAsmnts.Items)
                {
                    HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                    if (chk.Checked == true)
                    {
                        asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                    }
                }
                foreach (DataListItem dli in dlSkills.Items)
                {
                    HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                    if (chk.Checked == true)
                    {
                        asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                    }
                }
                if (asmntIDs.Length > 4)
                {
                    asmntIDs = asmntIDs.Substring(0, asmntIDs.Length - 4);
                    if ((minPercnt.Value != "") && (maxPercnt.Value != ""))
                        LoadGoalAndLPs(asmntIDs, Convert.ToDouble(minPercnt.Value), Convert.ToDouble(maxPercnt.Value), "");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Please select any Assessment');", true);
                    ul_Goals.InnerHtml = "";
                }
            }
            else
            {
                try
                {
                    string asmntIDs = "";
                    foreach (DataListItem dli in dlAsmnts.Items)
                    {
                        HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                        HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                        if (chk.Checked == true)    //check whether the assessment is selected or not
                        {
                            asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                        }
                    }
                    foreach (DataListItem dli in dlSkills.Items)
                    {
                        HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                        HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
                        if (chk.Checked == true)    //check whether the skill is selected or not
                        {
                            asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
                        }
                    }
                    if (asmntIDs.Length > 4)
                    {
                        asmntIDs = asmntIDs.Substring(0, asmntIDs.Length - 4);
                        LoadGoalAndLPs(asmntIDs, 0, 100, "");
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Please select any Assessment');", true);
                        ul_Goals.InnerHtml = "";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// All Assessments Button click event....
    /// Function which finds all the LPs and Goals from All the Assessments and skills without considering the selection.....
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbAllAsmnts_Click(object sender, EventArgs e)
    {
        try
        {
            string asmntIDs = "";
            foreach (DataListItem dli in dlAsmnts.Items)
            {
                HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
            }
            foreach (DataListItem dli in dlSkills.Items)
            {
                HiddenField hfId = (HiddenField)dli.FindControl("hfId");
                asmntIDs += "AssessmentId=" + hfId.Value + " OR ";
            }
            if (asmntIDs.Length > 4)
            {
                asmntIDs = asmntIDs.Substring(0, asmntIDs.Length - 4);
                LoadGoalAndLPs(asmntIDs, 0, 100, "");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// All Lessons inbank button click....
    /// Function which finds all the LPs and Goals from the Database without considering the student or the Assessmnts.....
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbLPBank_Click(object sender, EventArgs e)
    {

        //Clear Filter Criterias...
        //foreach (DataListItem dli in dlAsmnts.Items)
        //{
        //    HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
        //    chk.Checked = false;
        //}
        //foreach (DataListItem dli in dlSkills.Items)
        //{
        //    HtmlInputCheckBox chk = (HtmlInputCheckBox)dli.FindControl("chkAsmnt");
        //    chk.Checked = false;
        //}
        //minPercnt.Value = ""; maxPercnt.Value = "";


        ShowAllLPs("");

    }


    public void ShowAllLPs(string keywrd)
    {
        try
        {
            oSession = (clsSession)Session["UserSession"];

            curesesid = this.Session.SessionID.ToString();
            string HostName = Dns.GetHostName();
            string userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AsmntReview:ShowAllLPs";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
            }
            oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);

            ul_Goals.InnerHtml = "";
            if (oSession != null)
            {
                //query to find all the goals .....
                string selgoal = "SELECT DISTINCT Rel.GoalId,Gl.GoalName,Gl.GoalCode " +
                                "FROM GoalLPRel Rel " +
                                "INNER JOIN (Goal Gl " +
                                "INNER JOIN GoalType Typ " +
                                "ON Typ.GoalTypeId=Gl.GoalTypeId) " +
                                "ON Gl.GoalId=Rel.GoalId " +
                                "WHERE Typ.GoalTypeName='Academic Goals' " +
                                "AND Rel.ActiveInd='A' AND Gl.ActiveInd='A' " +
                                "AND Gl.SchoolId=" + oSession.SchoolId + " " +
                                "ORDER BY GoalId";

                oData = new clsData();
                DataTable dtGoal = oData.ReturnDataTable(selgoal, false);
                foreach (DataRow drGl in dtGoal.Rows)
                {
                    string goalid = drGl["GoalId"].ToString();
                    string goalname = drGl["GoalCode"].ToString();
                    //generate li tags for goals.....
                    //ul_Goals.InnerHtml += "<li style='position: static;' class='accordion'>" +
                    //                                "<h2 class='BG' onclick='h2click(this);'><a id='a_" + goalid + "' class='kk' href='#' onclick='AssignGoal(this.id,this.innerHTML,event);'>" + goalname + "</a></h2>" +
                    //                                "<div style='display: none;' class='wrapper nomar'>" +
                    //                                    "<div class='nobdrrcontainer'>";
                    //condition
                    string conditn = "WHERE GoalTypeName='Academic Goals' AND GoalCode='" + goalname + "' " +
                            "AND LP.ActiveInd='A' AND Rel.ActiveInd='A' AND Gl.ActiveInd='A' " +
                            "AND LP.SchoolId=" + oSession.SchoolId + " AND Gl.SchoolId=" + oSession.SchoolId;
                    if (keywrd.Length > 0)//if keyword not null..select LPs which matches the keywrd only..
                        conditn = "WHERE GoalTypeName='Academic Goals' AND GoalCode='" + goalname + "' AND LP.LessonPlanName LIKE '%" + keywrd + "%' " +
                            "AND LP.ActiveInd='A' AND Rel.ActiveInd='A' AND Gl.ActiveInd='A' " +
                            "AND LP.SchoolId=" + oSession.SchoolId + " AND Gl.SchoolId=" + oSession.SchoolId;
                    //query to find the LPs....
                    //string selLP = "SELECT * FROM (SELECT Rel.GoalId,GoalName,Rel.LessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,(SELECT COUNT(*) FROM [dbo].[StdtLessonPlan] SLP WHERE Rel.LessonPlanId=SLP.LessonPlanId AND Rel.GoalId=SLP.GoalId AND isDynamic='True' and SLP.StudentId=" + oSession.StudentId + ") LessonExist " +
                    //        "FROM (GoalLPRel Rel " +
                    //        "INNER JOIN LessonPlan LP " +
                    //        "ON LP.LessonPlanId=Rel.LessonPlanId) " +
                    //        "INNER JOIN (Goal Gl " +
                    //        "INNER JOIN GoalType Typ " +
                    //        "ON Typ.GoalTypeId=Gl.GoalTypeId) " +
                    //        "ON Gl.GoalId=Rel.GoalId " + conditn + ") GLP WHERE GLP.LessonExist=0 ORDER BY GLP.LessonPlanName,GoalId,GLP.LessonPlanId";


                    ///New select query for select the template. The goal should not disappear
                    ///
                    //string selLP = "SELECT * FROM (SELECT Rel.GoalId,GoalName,Rel.LessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,(SELECT COUNT(*) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                    //                " LP.LessonPlanId=SLP.LessonPlanId AND Rel.GoalId=SLP.GoalId and SLP.StudentId=" + oSession.StudentId + " AND ActiveInd='A') LessonExist,(SELECT COUNT(*) FROM DSTempHdr HDR WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL) DSTMPID " +
                    //                " ,(SELECT COUNT(*) FROM DSTempHdr HDR WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL AND isDynamic=0) Status FROM (GoalLPRel Rel INNER JOIN LessonPlan LP ON LP.LessonPlanId=Rel.LessonPlanId) INNER JOIN (Goal Gl INNER JOIN GoalType Typ ON Typ.GoalTypeId=Gl.GoalTypeId) ON Gl.GoalId=Rel.GoalId " + conditn + ") GLP " +
                    //                " WHERE GLP.DSTMPID=1 AND Status=1 AND GLP.LessonExist=0  UNION SELECT * FROM (SELECT Rel.GoalId,GoalName,Rel.LessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,(SELECT COUNT(*) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                    //                " LP.LessonPlanId=SLP.LessonPlanId ) LessonExist,(SELECT COUNT(*) FROM DSTempHdr HDR WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL) DSTMPID " +
                    //                " ,(SELECT COUNT(*) FROM DSTempHdr HDR WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL AND isDynamic=0) Status FROM (GoalLPRel Rel INNER JOIN LessonPlan LP " +
                    //                " ON LP.LessonPlanId=Rel.LessonPlanId) INNER JOIN (Goal Gl INNER JOIN GoalType Typ ON Typ.GoalTypeId=Gl.GoalTypeId) ON Gl.GoalId=Rel.GoalId " + conditn + ") GLP " +
                    //                " WHERE GLP.DSTMPID=0 AND Status=0 AND GLP.LessonExist=0  ORDER BY GLP.LessonPlanName,GoalId,GLP.LessonPlanId";
                    //string selLP = "SELECT * FROM (SELECT Rel.GoalId, Gl.GoalName, HDR.LessonPlanId,HDR.DSTemplateName as LessonPlanName, LP.LessonPlanDesc, (SELECT COUNT(*) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                    //    "LP.LessonPlanId=SLP.LessonPlanId AND Rel.GoalId=SLP.GoalId and SLP.StudentId=" + oSession.StudentId + " AND ActiveInd='A') LessonExist, " +
                    //    "(SELECT COUNT(*) FROM DSTempHdr HDR WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL) DSTMPID, " +
                    //    "(SELECT COUNT(*) FROM DSTempHdr HDR WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL AND isDynamic=0) Status " +
                    //    "FROM DSTempHdr HDR INNER JOIN GoalLPRel Rel ON Rel.LessonPlanId=HDR.LessonPlanId INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId " +
                    //    "INNER JOIN LessonPlan LP ON LP.LessonPlanId=HDR.LessonPlanId INNER JOIN GoalType GT ON GT.GoalTypeId=Gl.GoalTypeId  " + conditn + " AND HDR.isDynamic=0 AND StudentId IS NULL)GLP " +
                    //    "UNION SELECT * FROM (SELECT Rel.GoalId,GoalName,Rel.LessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,(SELECT COUNT(*) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                    //    "LP.LessonPlanId=SLP.LessonPlanId ) LessonExist,(SELECT COUNT(*) FROM DSTempHdr HDR WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL) DSTMPID  , " +
                    //    "(SELECT COUNT(*) FROM DSTempHdr HDR WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL AND isDynamic=0) Status FROM (GoalLPRel Rel INNER JOIN LessonPlan LP " +
                    //    "ON LP.LessonPlanId=Rel.LessonPlanId) INNER JOIN (Goal Gl INNER JOIN GoalType Typ ON Typ.GoalTypeId=Gl.GoalTypeId) ON Gl.GoalId=Rel.GoalId " + conditn + ") GLP " +
                    //    "WHERE GLP.DSTMPID=0 AND Status=0 AND GLP.LessonExist=0  ORDER BY GLP.LessonPlanName,GoalId,GLP.LessonPlanId ";
                    //string lookupQuery = "SELECT LookupId FROM LookUp WHERE LookupName='SoftDelete'";
                    string lookupQuery = "'SoftDelete'";
                    string selLP="";
                    if (hdLessonId.Value != null && hdLessonId.Value != "")
                    {
                        if (Convert.ToInt32(hdLessonId.Value) > 0)
                        {
                            selLP = "SELECT * FROM (SELECT Rel.GoalId, Gl.GoalName, HDR.LessonPlanId,HDR.DSTemplateName as LessonPlanName, LP.LessonPlanDesc, (SELECT COUNT(StdtLessonPlanId) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                                "LP.LessonPlanId=SLP.LessonPlanId AND Rel.GoalId=SLP.GoalId and SLP.StudentId=" + oSession.StudentId + " AND ActiveInd='A') LessonExist, " +
                                "(SELECT COUNT(DSTempHdrId) FROM DSTempHdr HDR INNER JOIN lookup ON HDR.statusid = lookup.lookupid WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL AND lookupname<>(" + lookupQuery + ")) DSTMPID, " +
                                "(SELECT COUNT(DSTempHdrId) FROM DSTempHdr HDR INNER JOIN lookup ON HDR.statusid = lookup.lookupid WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL AND HDR.isDynamic=0 AND lookupname<>(" + lookupQuery + ")) Status " +
                                "FROM DSTempHdr HDR INNER JOIN GoalLPRel Rel ON Rel.LessonPlanId=HDR.LessonPlanId INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId " +
                                "INNER JOIN LessonPlan LP ON LP.LessonPlanId=HDR.LessonPlanId INNER JOIN GoalType GT ON GT.GoalTypeId=Gl.GoalTypeId INNER JOIN lookup ON HDR.statusid = lookup.lookupid " + conditn + " AND HDR.isDynamic=0 AND lookupname<>(" + lookupQuery + ") AND StudentId IS NULL)GLP " +
                                "UNION ALL SELECT * FROM (SELECT Rel.GoalId,GoalName,Rel.LessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,(SELECT COUNT(StdtLessonPlanId) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                                "LP.LessonPlanId=SLP.LessonPlanId ) LessonExist,(SELECT COUNT(DSTempHdrId) FROM DSTempHdr HDR WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL) DSTMPID  , " +
                                "(SELECT COUNT(DSTempHdrId) FROM DSTempHdr HDR INNER JOIN lookup ON HDR.statusid = lookup.lookupid WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL AND HDR.isDynamic=0 AND lookupname<>(" + lookupQuery + ")) Status FROM (GoalLPRel Rel INNER JOIN LessonPlan LP " +
                                "ON LP.LessonPlanId=Rel.LessonPlanId) INNER JOIN (Goal Gl INNER JOIN GoalType Typ ON Typ.GoalTypeId=Gl.GoalTypeId) ON Gl.GoalId=Rel.GoalId " + conditn + " AND LP.LessonPlanId = " + Convert.ToInt32(hdLessonId.Value) + " ) GLP " +
                                "WHERE GLP.DSTMPID=0 AND Status=0 AND GLP.LessonExist=0 ORDER BY GLP.LessonPlanName,GoalId,GLP.LessonPlanId ";
                        }
                    }
                    else
                        selLP = "SELECT * FROM (SELECT Rel.GoalId, Gl.GoalName, HDR.LessonPlanId,HDR.DSTemplateName as LessonPlanName, LP.LessonPlanDesc, (SELECT COUNT(StdtLessonPlanId) FROM [dbo].[StdtLessonPlan] SLP WHERE " +
                                "LP.LessonPlanId=SLP.LessonPlanId AND Rel.GoalId=SLP.GoalId and SLP.StudentId=" + oSession.StudentId + " AND ActiveInd='A') LessonExist, " +
                                "(SELECT COUNT(DSTempHdrId) FROM DSTempHdr HDR INNER JOIN lookup ON HDR.statusid = lookup.lookupid WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL AND lookupname<>(" + lookupQuery + ")) DSTMPID, " +
                                "(SELECT COUNT(DSTempHdrId) FROM DSTempHdr HDR INNER JOIN lookup ON HDR.statusid = lookup.lookupid WHERE Rel.LessonPlanId=HDR.LessonPlanId AND StudentId IS NULL AND HDR.isDynamic=0 AND lookupname<>(" + lookupQuery + ")) Status " +
                                "FROM DSTempHdr HDR INNER JOIN GoalLPRel Rel ON Rel.LessonPlanId=HDR.LessonPlanId INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId " +
                                "INNER JOIN LessonPlan LP ON LP.LessonPlanId=HDR.LessonPlanId INNER JOIN GoalType GT ON GT.GoalTypeId=Gl.GoalTypeId INNER JOIN lookup ON HDR.statusid = lookup.lookupid " + conditn + " AND HDR.isDynamic=0 AND lookupname<>(" + lookupQuery + ") AND StudentId IS NULL)GLP " +
                                "ORDER BY GLP.LessonPlanName,GoalId,GLP.LessonPlanId ";                   
                    ///end
                    ///
                    
                    //UNION SELECT * FROM (SELECT Rel.GoalId,GoalName,Rel.LessonPlanId,LP.LessonPlanName,LP.LessonPlanDesc,(SELECT COUNT(*) FROM "+
                    //                " [dbo].[StdtLessonPlan] SLP WHERE  LP.LessonPlanId=SLP.LessonPlanId AND ActiveInd='D' and SLP.StudentId="+oSession.StudentId+") LessonExist,(SELECT COUNT(*) FROM DSTempHdr HDR "+
                    //                " WHERE HDR.LessonPlanId=LP.LessonPlanId AND StudentId ="+oSession.StudentId+" AND StatusId=(select LookupId from LookUp where LookupName='Deleted' and LookupType='TemplateStatus')) DSTMPID  ,(SELECT COUNT(*) FROM DSTempHdr HDR WHERE "+
                    //                " HDR.LessonPlanId=LP.LessonPlanId AND StudentId IS NULL AND isDynamic=0 ) Status FROM (GoalLPRel Rel INNER JOIN LessonPlan LP ON LP.LessonPlanId=Rel.LessonPlanId) INNER JOIN (Goal Gl INNER JOIN GoalType Typ ON Typ.GoalTypeId=Gl.GoalTypeId) ON "+
                    //                " Gl.GoalId=Rel.GoalId "+conditn+") GLP  WHERE GLP.DSTMPID=1 AND Status=0 AND GLP.LessonExist=1   ORDER BY GLP.LessonPlanName,GoalId,GLP.LessonPlanId";

                    

                    DataTable dtLP = oData.ReturnDataTable(selLP, false);

                    if (dtLP != null)
                    {
                        if (dtLP.Rows.Count > 0)
                        {
                            ul_Goals.InnerHtml += "<li style='position: static;' class='accordion'>" +
                                                  "<h2 class='BG' onclick='h2click(this);'><a id='a_" + goalid + "' class='kk' href='#' onclick='AssignGoal(this.id,this.innerHTML,event);'>" + goalname + "</a></h2>" +
                                                  "<div style='display: none;' class='wrapper nomar'>" +
                                                      "<div class='nobdrrcontainer'>";
                        }
                    }

                    foreach (DataRow drLP in dtLP.Rows)
                    {
                        //HtmlAnchor atag = new HtmlAnchor();
                        //atag.ID = drLP["LessonPlanId"].ToString();
                        //atag.Attributes.Add("class", "grmb");
                        //atag.Attributes.Add("onclick", "AssignGoalAndLPs(this.id,this.firstChild.textContent," + goalid + ",'" + goalname + "','" + drLP["LessonPlanDesc"].ToString() + "');");
                        //atag.InnerHtml = "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis; overflow: hidden; width:56%;' title='" + drLP["LessonPlanName"].ToString() + "'>" + drLP["LessonPlanName"].ToString() + "</div>";

                        //ul_Goals.InnerHtml += "<a href='#' id='" + drLP["LessonPlanId"].ToString() + "' class='grmb' onclick='AssignGoalAndLPs(this.id,this.firstChild.textContent," + goalid + ",&apos;" + goalname + "&apos;,&apos;" + drLP["LessonPlanDesc"].ToString() + "&apos;);'>" +
                        //        "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis; overflow: hidden; width:56%;' title='" + drLP["LessonPlanName"].ToString() + "'>" + drLP["LessonPlanName"].ToString() +
                        //        "</div></a>";


                        ul_Goals.InnerHtml += "<div class='grmb'><a style='width:137px;' href='#' id='" + drLP["LessonPlanId"].ToString() + "'  onclick='AssignGoalAndLPs(this.id,this.firstChild.textContent," + goalid + ",&apos;" + goalname + "&apos;,&apos;" + drLP["LessonPlanDesc"].ToString() + "&apos;);'>" +
                               "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis; overflow: hidden; width:110%;' title='" + drLP["LessonPlanName"].ToString() + "'>" + drLP["LessonPlanName"].ToString() +
                               "</div></a><img id='" + drLP["LessonPlanId"].ToString() + "' onclick='LoadLessonView(this.id," + goalid + ");' src='images/transbtn.png' height='15px' width='15px' style='float:right;margin:0 10px 0 0;' /></div>";

                    }
                    if (dtLP != null)
                    {
                        if (dtLP.Rows.Count == 0)
                        {
                        }
                        //closing the li tag....
                        else
                            ul_Goals.InnerHtml += "<div class='clear'></div>" +
                                            "</div>" +
                                            "<div class='clear'></div>" +
                                           "</div>" +
                                          "</li>";
                    }

                }
                hdLessonId.Value = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// WebMethod which used to save the Goal which the user added to the 'Assign Goals and Lessons' Section.....
    /// </summary>
    /// <param name="goalid">Goalid</param>
    /// <returns>>It returns 'exists' if the goal is already exists or returns the newly created id if suceesfully inserted or returns '0' if insertion failed</returns>
    [WebMethod]
    public static string SaveGoal(string goalid, bool flag)
    {
        try
        {
            clsData oData = new clsData();
            clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            object objIEP = "";
            if (oSession != null)
            {

                //string selQry = "SELECT StdtIEPId FROM(SELECT IEP.StdtIEPId,LookupName,IEP.EffStartDate,RANK() OVER " +
                //                "(PARTITION BY StudentId " +
                //                "ORDER BY CASE(LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' " +
                //                "THEN 2 WHEN 'Approved' THEN 3 END,IEP.EffStartDate DESC) as rank " +
                //                "FROM (StdtIEP IEP INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                //                "INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId " +
                //                "WHERE StudentId=" + oSession.StudentId + " AND IEP.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' AND LookupName<>'Expired') A WHERE rank=1";
                //
                if (oSession.SchoolId == 1)
                {
                    string selQry = "select isnull(StdtIEPId,0) StdtIEPId from StdtIEP I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                       + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                       + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A'  ";
                    objIEP = oData.FetchValue(selQry);
                }
                else if (oSession.SchoolId == 2)
                {
                    string selQry = "select isnull(StdtIEP_PEId,0) StdtIEP_PEId from StdtIEP_PE I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                       + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                       + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A'  ";
                    objIEP = oData.FetchValue(selQry);
                }
                bool glExists = false;
                if (objIEP == null)
                    objIEP = 0;
                if (Convert.ToInt32(objIEP) == 0)
                {
                    glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A'");
                }
                else
                {
                    glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");

                }
                //  glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");
                if (!glExists)  //check whether the goal is already existed or not.....
                {
                    object objYearId = oData.FetchValue("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
                    if (objYearId != null)
                    {
                        object objStat = oData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='Goal Status' AND LookupName='In Progress'");
                        if (objStat != null)
                        {
                            string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                                            "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + objYearId.ToString() + ",0," +
                                            "" + objStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                            if (flag)
                            {
                                if (Convert.ToInt32(objIEP) == 0)
                                {
                                    insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                                            "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + objYearId.ToString() + ",0," +
                                            "" + objStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                                else
                                {
                                    insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StdtIEPId,StatusId,ActiveInd,IEPGoalNo," +
                                                "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + objYearId.ToString() + ",1," + objIEP.ToString() + "," +
                                                "" + objStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                            }

                            int stdtglID = oData.ExecuteWithScope(insGoal);
                            if (!flag)
                            {
                                objIEP = 0;
                            }
                            //returns the id..
                            return stdtglID.ToString() + "*" + objIEP.ToString();
                        }
                        else
                        {
                            return "0";
                        }

                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    if (flag)
                    {
                        object objGlId = oData.FetchValue("SELECT StdtGoalId FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + ")");
                        if (objGlId == null)
                        {
                            objGlId = oData.FetchValue("SELECT StdtGoalId FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND IncludeIEP=0");
                        }
                        if (objGlId != null)
                        {
                            if (objIEP != null)
                                if (Convert.ToInt32(objIEP) != 0)
                                {
                                    string updGoal = "UPDATE StdtGoal SET IncludeIEP=1,StdtIEPId=" + objIEP.ToString() + ",ModifiedBy=" + oSession.LoginId + ",ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StdtGoalId=" + objGlId.ToString();
                                    oData.Execute(updGoal);
                                }
                        }
                    }
                    //returns 'exists' if already exists....
                    return "exists*" + objIEP.ToString();
                }

            }
            else
            {
                return "0";
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]

    public static string SaveLessons(string LPid, string goalid)
    {
        SqlTransaction Trans = null;
        SqlConnection Con = new SqlConnection();
        clsData oData = new clsData();
        try
        {

            Con = oData.Open();
            Trans = Con.BeginTransaction();
            object objIEP = "";
            clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (oSession != null)
            {
                if (oSession.SchoolId == 2)
                {
                    string selQry = "select isnull(StdtIEP_PEId,0) StdtIEP_PEId from StdtIEP_PE I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                        + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                        + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' ";
                    objIEP = oData.FetchValueTrans(selQry, Trans, Con);
                }
                else if (oSession.SchoolId == 1)
                {
                    string selQry = "select isnull(StdtIEPId,0) StdtIEPId from StdtIEP I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                        + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                        + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' ";
                    objIEP = oData.FetchValueTrans(selQry, Trans, Con);
                }

                if (objIEP == null)
                    objIEP = 0;
                //query to check current class status;
                string qryclass = "select ResidenceInd from [dbo].[Class] where classid=" + oSession.Classid;
                bool isDay = Convert.ToBoolean(oData.FetchValueTrans(qryclass, Trans, Con));
                string LPname = oData.FetchValueTrans("SELECT LessonPlanName FROM LessonPlan WHERE LessonPlanId=" + LPid, Trans, Con).ToString();
                bool lpNameExists = oData.IFExistsWithTranss("SELECT * FROM DSTempHdr inner join LookUp lu on lu.LookupId=DSTempHdr.StatusId WHERE  RTRIM(LTRIM(LOWER(DSTemplateName)))= RTRIM(LTRIM(LOWER('" + clsGeneral.convertQuotes(LPname.Trim()) + "'))) AND StudentId='" + oSession.StudentId + "' AND (SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId=DSTempHdr.StdtLessonplanId)='" + goalid + "' and lu.LookupName <> 'Deleted'", Trans, Con);
                bool lpDupExists = oData.IFExistsWithTranss("SELECT * FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND ActiveInd='A' AND GoalId=" + goalid + " ", Trans, Con);            
                bool lpDelExists = oData.IFExistsWithTranss("SELECT * FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND ActiveInd='D' AND GoalId=" + goalid + " ", Trans, Con);
                // bool lpExists = oData.IFExists("SELECT * FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND LessonPlanId=" + LPid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");

                if (!lpDupExists && !lpDelExists && !lpNameExists)  //chech whether the LP is exists or not.....
                {
                    object objYearId = oData.FetchValueTrans("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'", Trans, Con);
                    if (objYearId != null)
                    {
                        string strQuery = "SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + LPid + " and StudentId IS NULL ";
                        object flag = oData.FetchValueTrans(strQuery, Trans, Con);
                        object objStat = oData.FetchValueTrans("SELECT LookupId FROM LookUp WHERE LookupType='LP Status' AND LookupName='In Progress'", Trans, Con);
                        object objLPname = oData.FetchValueTrans("SELECT LessonPlanName FROM LessonPlan WHERE LessonPlanId=" + LPid + "", Trans, Con);
                        if ((objStat != null) && (objLPname != null))
                        {
                            string insLP = "";
                            if (Convert.ToInt32(objIEP) == 0)
                            {

                                if (Convert.ToInt32(flag) > 0)
                                {
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + ",0," +
                                                                       "" + objStat.ToString() + ",'A',0," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                                else
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + ",0," +
                                                                       "" + objStat.ToString() + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";

                            }
                            else
                            {
                                if (Convert.ToInt32(flag) > 0)
                                {
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,StdtIEPId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + "," + objIEP.ToString() + ",1," +
                                                                       "" + objStat.ToString() + ",'A',0," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                                else
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,StdtIEPId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + "," + objIEP.ToString() + ",1," +
                                                                       "" + objStat.ToString() + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                            }

                            int stdtLPID = oData.ExecuteWithScopeandConnection(insLP, Con, Trans);

                            string condtn = "";// 

                            if (!isDay)
                                condtn = "SET LessonPlanTypeDay=1";
                            else
                                condtn = "SET LessonPlanTypeResi=1";

                            oData.ExecuteWithScopeandConnection("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + stdtLPID + "", Con, Trans);

                            //bool glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");
                            //if (!glExists)  //check whether the goal is already existed or not.....
                            //{
                            //    object obj_YearId = oData.FetchValue("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
                            //    if (obj_YearId != null)
                            //    {
                            //        object obj_Stat = oData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='Goal Status' AND LookupName='In Progress'");
                            //        if (obj_Stat != null)
                            //        {
                            //            string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                            //                            "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + obj_YearId.ToString() + ",0," +
                            //                            "" + obj_Stat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";

                            //            int stdtglID = oData.ExecuteWithScope(insGoal);
                            //            //returns the id..
                            //            return stdtglID.ToString();
                            //        }
                            //}


                            clsAssignLessonPlan objTemplateAssign = new clsAssignLessonPlan();
                            int TempId = objTemplateAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(LPid), clsGeneral.convertQuotes(objLPname.ToString()), oSession.LoginId, stdtLPID, Con, Trans);
                            oData.ExecuteWithScopeandConnection("UPDATE DSTempHdr SET PrevStatus='IEP-'+'" + objIEP + "' WHERE DSTempHdrId='" + TempId + "'", Con, Trans);
                            clsDocumentasBinary objBinary = new clsDocumentasBinary();
                            int templateId = Convert.ToInt32(oData.FetchValueTrans("SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + LPid + " and StudentId IS NULL", Trans, Con));
                            DataTable dtdoc = oData.ReturnDataTableWithTransaction("SELECT LPDoc FROM LPDoc WHERE DSTempHdrId=" + templateId + "", Con, Trans, false);
                            if (dtdoc != null)
                            {
                                if (dtdoc.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtdoc.Rows)
                                    {
                                        string strquerry = "INSERT INTO LPDoc(SchoolId,DSTempHdrId,DocURL,CreatedBy,CreatedOn) SELECT SchoolId," + TempId + ",DocURL," + oSession.LoginId + ",GETDATE() FROM LPDoc WHERE LPDoc='" + row["LPDoc"].ToString() + "'";
                                        int docid = oData.ExecuteWithScopeandConnection(strquerry, Con, Trans);
                                        string binarydata = "SELECT Data,DocumentName FROM binaryFiles WHERE DocId=" + row["LPDoc"].ToString() + " AND type='LP_DOC'";
                                        DataTable dtbinary = oData.ReturnDataTableWithTransaction(binarydata, Con, Trans, false);
                                        byte[] myData = (byte[])dtbinary.Rows[0]["Data"];
                                        string filename = Convert.ToString(dtbinary.Rows[0]["DocumentName"]);
                                        int binaryid = objBinary.saveDocument(myData, filename, "", "LP_DOC", docid, "LessonPlanDoc", oSession.SchoolId, 0, oSession.LoginId);
                                    }
                                }
                            }
                            //string insDs = "INSERT INTO DSTempHdr(DSTemplateName,SchoolId,StudentId,LessonPlanId,CreatedBy,CreatedOn,StatusId) " +
                            //               "VALUES ('" + objLPname.ToString().Trim() + "'," + oSession.SchoolId + "," + oSession.StudentId + "," + LPid.ToString() + "," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," +
                            //               "(select LookupId from LookUp where LookupName = 'In Progress' and LookupType = 'TemplateStatus'))";
                            //oData.ExecuteWithScope(insDs);
                            ////returns the id ...
                            int isItDay = 0;
                            if (isDay)
                                isItDay = 0;
                            else
                                isItDay = 1;
                            oData.CommitTransation(Trans, Con);
                            return stdtLPID.ToString() + "*" + objIEP.ToString() + "*" + isItDay.ToString();

                        }
                        else
                        {
                            //oData.RollBackTransation(Trans, Con);
                            return "0";
                        }

                    }
                    else
                    {
                        return "0";
                    }

                }
                else
                {
                    if (lpDupExists || lpNameExists)
                    {
                        return "exists";
                    }
                    if (lpDelExists)
                    {
                        clsAssignLessonPlan objTemplateAssign = new clsAssignLessonPlan();
                        object objLPname = oData.FetchValueTrans("SELECT LessonPlanName FROM LessonPlan WHERE LessonPlanId=" + LPid, Trans, Con);

                        int result = oData.ExecuteWithTrans("UPDATE StdtLessonPlan SET ActiveInd='A' WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND GoalId=" + goalid + " ", Con, Trans);
                        int stdtLessonPID = Convert.ToInt32(oData.FetchValueTrans("SELECT StdtLessonPlanId FROM  StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND ActiveInd='A' AND GoalId=" + goalid + " ", Trans, Con));
                        int TempId = objTemplateAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(LPid), objLPname.ToString(), oSession.LoginId, stdtLessonPID, Con, Trans);
                        int isItDay = 0;
                        if (isDay)
                            isItDay = 0;
                        else
                            isItDay = 1;
                        oData.CommitTransation(Trans, Con);
                        return stdtLessonPID.ToString() + "*" + objIEP.ToString() + "*" + isItDay.ToString();

                    }
                    //returns 'exists' if already exists....
                    return "exists";
                }
            }
            else
            {
                return "0";
            }

        }
        catch (Exception ex)
        {
            oData.RollBackTransation(Trans, Con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
            throw ex;
        }


    }

    [WebMethod]
    public static string DeleteLPandGoals(string id, string type)
    {
        string retrn = "";
        try
        {
            clsData oData = new clsData();
            clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (type == "LP")
            {
                //query to find current iep..
                string selQry = "SELECT StdtIEPId FROM(SELECT IEP.StdtIEPId,LookupName,IEP.EffStartDate,RANK() OVER " +
                                "(PARTITION BY StudentId " +
                                "ORDER BY CASE(LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' " +
                                "THEN 2 WHEN 'Approved' THEN 3 END,IEP.EffStartDate DESC) as rank " +
                                "FROM (StdtIEP IEP INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                                "INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId " +
                                "WHERE StudentId=" + oSession.StudentId + " AND IEP.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' AND LookupName<>'Expired') A WHERE rank=1";
                object objIEP = oData.FetchValue(selQry);
                //query to check whether if any lessoplan are mapped to the goal....
                string selLPCount = "select IncludeIEP from StdtLessonPlan where StdtLessonPlanId=" + id;
                int LPsExists = Convert.ToInt32(oData.FetchValue(selLPCount));
                if (LPsExists == 0) //if not...
                {
                    selLPCount = "select DSTempHdrId from DSTempHdr where LessonPlanId=(select LessonPlanId from StdtLessonPlan where StdtLessonPlanId=" + id +
                        " ) and StatusId=(select LookUpId from [LookUp] where LookupType='TemplateStatus' and LookupName='Approved')";
                    bool LPsExist = oData.IFExists(selLPCount);
                    if (!LPsExist) //if not...
                    {
                        //string delLP = "DELETE FROM StdtLessonPlan WHERE StdtLessonPlanId=" + id;
                        string updateLP = "UPDATE StdtLessonPlan SET ActiveInd='D',ModifiedOn=getdate() WHERE StdtLessonPlanId=" + id; ;
                        oData.Execute(updateLP);
                        retrn = "1";
                    }
                    else
                    {
                        retrn = "Deletion Not Possible : Lessonplan is using in Datasheet";
                    }
                }
                else
                {
                    retrn = "Deletion Not Possible : Lessonplan is Included in IEP";
                }
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "", true);
                //     Page.Redirect("default.aspx", false);
            }
            else if (type == "Goal")
            {
                if (oSession != null)
                {
                    //query to find current iep..
                    string selQry = "SELECT StdtIEPId FROM(SELECT IEP.StdtIEPId,LookupName,IEP.EffStartDate,RANK() OVER " +
                                    "(PARTITION BY StudentId " +
                                    "ORDER BY CASE(LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' " +
                                    "THEN 2 WHEN 'Approved' THEN 3 END,IEP.EffStartDate DESC) as rank " +
                                    "FROM (StdtIEP IEP INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                                    "INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId " +
                                    "WHERE StudentId=" + oSession.StudentId + " AND IEP.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' AND LookupName<>'Expired') A WHERE rank=1";
                    object objIEP = oData.FetchValue(selQry);
                    if (objIEP == null)
                        objIEP = 0;
                    //query to check whether if any lessoplan are mapped to the goal....
                    string selLPCount = "SELECT StdtLessonPlanId " +
                                    "FROM StdtLessonPlan StdLP INNER JOIN StdtGoal StdGl ON StdGl.StudentId=StdLP.StudentId AND " +
                                    "StdLP.SchoolId=StdGl.SchoolId AND StdLP.GoalId=StdGl.GoalId " +
                                    "WHERE StdLP.StudentId=" + oSession.StudentId + " AND StdLP.ActiveInd='A' AND StdGl.ActiveInd='A' " +
                                    "AND StdLP.SchoolId=" + oSession.SchoolId + " AND (StdLP.IncludeIEP=0 OR (StdLP.IncludeIEP=1 AND StdLP.StdtIEPId=" + objIEP.ToString() + ")) " +
                                    "AND StdGl.StdtGoalId=" + id;
                    bool LPsExists = oData.IFExists(selLPCount);
                    if (!LPsExists) //if not...
                    {
                        string delGoal = "DELETE FROM StdtGoal WHERE StdtGoalId=" + id;
                        oData.Execute(delGoal);
                        retrn = "1";
                    }
                    else
                        retrn = "Delete Not Possible";
                }
                else
                    retrn = "Delete Failed";
            }

        }
        catch (Exception ex)
        {
            retrn = ex.Message;
            throw ex;
        }
        return retrn;
    }




    private static void bindgrid(DataTable dt)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Webmethod which used to update the IEPStatus of the stdtGoal or stdtLP
    /// </summary>
    /// <param name="id">LessonPlan Id or Goal Id</param>
    /// <param name="type">this param indicates whether it is goal or LP</param>
    /// <param name="crntStat">it indicates whether the goal or LP is mapped to the current IEP or not, 
    /// if goal or LP already is mapped to current iep then this param value is '1'
    /// if goal or LP not mapped to any iep then this param value is '0'</param>
    /// <returns>it returns 'There is not any IEP in Progress. Updation not Possible' if there is not any IEP in Progress or return 'success' if the insertion is success
    /// or it returns 'Updation failed !!' if insertion failed...</returns>
    [WebMethod]
    public static string UpdateIEPStat(string id, string type, string crntStat)
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        int stat = 0;
        object objIEP = null;
        try
        {
            if (oSession != null)
            {
                if (oSession.SchoolId == 2)
                {
                    //objIEP = oData.FetchValue("SELECT StdtIEP_PEId FROM ((StdtIEP_PE IEP INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId)" +
                    //                               "INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                    //                               "WHERE IEP.SchoolId=" + oSession.SchoolId + " AND StudentId=" + oSession.StudentId + " AND Yr.CurrentInd='A' AND LookupName='In Progress'");
                    objIEP = oData.FetchValue("SELECT StdtIEP_PEId FROM ((StdtIEP_PE IEP INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId)" +
                                                   "INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                                                   "WHERE IEP.SchoolId=" + oSession.SchoolId + " AND StudentId=" + oSession.StudentId + " AND LookupName='In Progress'");
                }
                else
                {
                    //objIEP = oData.FetchValue("SELECT StdtIEPId FROM ((StdtIEP IEP INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId)" +
                    //                               "INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                    //                               "WHERE IEP.SchoolId=" + oSession.SchoolId + " AND StudentId=" + oSession.StudentId + " AND Yr.CurrentInd='A' AND LookupName='In Progress'");
                    objIEP = oData.FetchValue("SELECT StdtIEPId FROM ((StdtIEP IEP INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId)" +
                                                   "INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                                                   "WHERE IEP.SchoolId=" + oSession.SchoolId + " AND StudentId=" + oSession.StudentId + " AND LookupName='In Progress'");

                }
                string condtn = "";

                if (objIEP == null)    //if iep is null, returns message that mapping not possibile...
                {
                    return "There is not any IEP in Progress. Not Possible to update";
                }
                else if (objIEP != null)
                {
                    if (crntStat == "0")    //if iep is not null, then map the goal or LP to the iep..
                    {
                        condtn = "SET IncludeIEP=1, StdtIEPId=" + objIEP.ToString();

                        if (type == "LP")
                        {
                            if (oData.IFExists("select * from StdtGoal where GoalId=(select GoalId from  StdtLessonPlan where StdtLessonPlanId= " + id + ") and StudentId=" + oSession.StudentId + " and ActiveInd='A' and includeIEP=1 and StdtIEPId=" + objIEP.ToString()))
                            {
                                stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                            }
                            else
                            {
                                //string stdtGoalId = oData.FetchValue("select * from StdtGoal where GoalId=(select GoalId from  StdtLessonPlan where StdtLessonPlanId= " + id + ") and StudentId=" + oSession.StudentId + " and ActiveInd='A' and StdtIEPId=" + objIEP.ToString()).ToString();
                                stat = oData.Execute("UPDATE StdtGoal " + condtn + " WHERE GoalId=(select GoalId from  StdtLessonPlan where StdtLessonPlanId= " + id + ") and StdtIEPId=" + objIEP.ToString());
                                stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                            }

                        }
                        if (type == "Goal")
                        {
                            stat = oData.Execute("UPDATE StdtGoal " + condtn + " WHERE StdtGoalId=" + id);
                        }
                    }
                    else if (crntStat == "1")    //to unmap the LP or goal from the current iep...
                    {
                        condtn = "SET IncludeIEP=0";
                        if (type == "LP")
                        {
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }
                        if (type == "Goal")
                        {

                            if (oData.IFExists("select * from StdtLessonPlan where GoalId=(select GoalId from StdtGoal where StdtGoalId= " + id + ") and StudentId=" + oSession.StudentId + " and ActiveInd='A' and includeIEP=1 AND StdtIEPId='" + objIEP + "'") == false)
                            {
                                stat = oData.Execute("UPDATE StdtGoal " + condtn + " WHERE StdtGoalId=" + id);
                            }
                            else
                            {
                                return "Goal Canot be removed With out removing Lesson plans under the goal";
                            }
                        }
                    }
                }




                if (stat > 0)   //if insertion success
                    return "success";
                else if (stat == -10)
                    return "";
                else
                    return "Updation failed !!";
            }
            else
                return "Session Expired.";
        }
        catch (Exception ex)
        {
            return ex.Message;
            throw ex;
        }

    }



    /// <summary>
    /// Webmethod which used to update the LessonPlan Status of the StdtLessonPlan
    /// </summary>

    [WebMethod]
    public static string UpdateLessonPlanStat(string id, string type, string name, string crntStat)
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        int stat = 0;
        try
        {
            if (oSession != null)
            {

                string condtn = "";
                int val = 0;
                if (name == "Day")
                {

                    if (crntStat == "0")    //if iep is not null, then map the goal or LP to the iep..
                    {

                        condtn = "SET LessonPlanTypeDay=" + val;

                        if (type == "LP")
                        {
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }

                    }
                    else if (crntStat == "1")    //to unmap the LP or goal from the current iep...
                    {
                        val = 1;
                        condtn = "SET LessonPlanTypeDay=" + val;
                        if (type == "LP")
                        {
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }

                    }
                }
                else
                {

                    if (crntStat == "0")    //if iep is not null, then map the goal or LP to the iep..
                    {
                        condtn = "SET LessonPlanTypeResi=" + val;
                        if (type == "LP")
                        {
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }

                    }
                    else if (crntStat == "1")    //to unmap the LP or goal from the current iep...
                    {
                        val = 1;
                        condtn = "SET LessonPlanTypeResi=" + val;
                        if (type == "LP")
                        {
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }

                    }
                }
                // bool Present = true;
                DataTable dt = oData.ReturnDataTable("StdtLessonPlan WHERE StdtLessonPlanId=" + id);
                foreach (DataRow drGoal in dt.Rows)
                {
                    if ((drGoal["LessonPlanTypeDay"].ToString() == "0" || drGoal["LessonPlanTypeDay"].ToString() == "" || drGoal["LessonPlanTypeDay"].ToString() == null) && (drGoal["LessonPlanTypeResi"].ToString() == "0" || drGoal["LessonPlanTypeResi"].ToString() == "" || drGoal["LessonPlanTypeResi"].ToString() == null))
                    {
                        string qryclass = "select ResidenceInd from [dbo].[Class] where classid=" + oSession.Classid;
                        bool isDay = Convert.ToBoolean(oData.FetchValue(qryclass));
                        if (!isDay)
                        {
                            condtn = "SET LessonPlanTypeDay=1";
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }
                        else
                        {
                            condtn = "SET LessonPlanTypeResi=1";
                            stat = oData.Execute("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + id);
                        }
                    }
                }




                if (stat > 0)   //if insertion success
                    return "success";
                else
                    return "Updation failed !!";

            }
            else
                return "Session Expired.";

        }

        catch (Exception ex)
        {
            return ex.Message;
            throw ex;
        }

    }



    /// <summary>
    /// funtion to add a new Lessonplan....
    /// </summary>
    protected void AddLessonPlan()
    {
        oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        string HostName = Dns.GetHostName();
        string userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "AsmntReview:AddLessonPlan";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
        }
        
        oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);

        object objYearId = oData.FetchValue("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
        object objStat = oData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='LP Status' AND LookupName='In Progress'");

        //if (oData.IFExists("select LessonPlanId from LessonPlan where LessonPlanName='" + txtLPname.Text + "'") == false)
        //{
        SqlConnection con = new SqlConnection();
        con = oData.Open();

        SqlTransaction trans = con.BeginTransaction();
        int LPid = 0;
        try
        {
            if (oSession != null)
            {
                string insLP = "insert into LessonPlan(SchoolId,ActiveInd,LessonPlanName,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials,CreatedBy,CreatedOn) " +
                    "values(" + oSession.SchoolId + ",'A','" + clsGeneral.convertQuotes(txtLPname.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtFrameworkAndStandard.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSpecStandard.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSpecEntryPoint.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPreskills.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtMaterials.Text.Trim()) + "'," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100))) ";
                LPid = oData.ExecuteWithScopeandConnection(insLP, con, trans);
                if (LPid > 0)
                {
                    string strQuery = "INSERT into GoalLPRel(GoalId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn) " +
                        "Values(" + ddlGoals.SelectedValue + "," + LPid.ToString() + ",'A'," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                    oData.ExecuteWithScopeandConnection(strQuery, con, trans);



                    //string stdtinsLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,IncludeIEP,StatusId," +
                    //                                            "ActiveInd,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + ddlGoals.SelectedValue + "," + objYearId.ToString() + ",0," +
                    //                                            "" + objStat.ToString() + ",'A'," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";

                    //oData.ExecuteWithScopeandConnection(stdtinsLP, con, trans);

                    //string insDs = "insert into DSTempHdr(DSTemplateName,SchoolId,StudentId,LessonPlanId,CreatedBy,CreatedOn,StatusId) " +
                    //    "values ('" + txtLPname.Text.Trim() + "'," + oSession.SchoolId + "," + oSession.StudentId + "," + LPid.ToString() + "," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," +
                    //    "(select LookupId from LookUp where LookupName = 'In Progress' and LookupType = 'TemplateStatus'))";
                    //oData.ExecuteWithScopeandConnection(insDs, con, trans);

                    //bool glExists = oData.IFExistsWithTranss("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + ddlGoals.SelectedValue + " AND ActiveInd='A'", trans, con);
                    //if (!glExists)  //check whether the goal is already existed or not.....
                    //{
                    //    object objgolStat = oData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='Goal Status' AND LookupName='In Progress'");
                    //    if (objStat != null)
                    //    {
                    //        string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                    //                        "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + ddlGoals.SelectedValue + "," + objYearId.ToString() + ",0," +
                    //                        "" + objgolStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";

                    //        int stdtglID = oData.ExecuteWithScopeandConnection(insGoal, con, trans);
                    //    }
                    //}
                    oData.CommitTransation(trans, con);
                    HttpContext.Current.Session["Baseline"] = txtBaseline.Text;
                    HttpContext.Current.Session["Objective"] = txtObjective.Text;
                    hdLessonId.Value = LPid.ToString();
                }

            }
        }
        catch (Exception ex)
        {
            oData.RollBackTransation(trans, con);
            con.Close();
            throw ex;
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "closePOP(" + ddlGoals.SelectedValue + "," + LPid + ");", true);
        //}
        //else
        //{
        //    tdMsgForLp.InnerHtml = clsGeneral.failedMsg("LessonPlan Already Exist");

        //}
    }
    /// <summary>
    /// Create new Lesson Plan button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddLP_Click(object sender, EventArgs e)
    {
        if ((txtLPname.Text.Trim().Length == 0) || (ddlGoals.SelectedIndex == 0))
        {
            if (txtLPname.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('LessonPlan name cannot be null');", true);
            }
            else if (ddlGoals.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Select any Goal');", true);
            }
        }
        else
        {
            oData = new clsData();
            oSession = (clsSession)Session["UserSession"];
            curesesid = this.Session.SessionID.ToString();
            string HostName = Dns.GetHostName();
            string userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AsmntReview:btnAddLP_Click";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
            }
            oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);

            //int Template = 0;
            int StudentLp = 0;

            //Template = Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM DSTempHdr WHERE  RTRIM(LTRIM(LOWER(DSTemplateName)))= RTRIM(LTRIM(LOWER('" + txtLPname.Text.Trim() + "'))) AND StudentId IS NULL AND isDynamic=0 "));
            StudentLp = Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM DSTempHdr WHERE  RTRIM(LTRIM(LOWER(DSTemplateName)))= RTRIM(LTRIM(LOWER('" + clsGeneral.convertQuotes(txtLPname.Text.Trim()) + "'))) AND StudentId='" + oSession.StudentId + "' AND (SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId=DSTempHdr.StdtLessonplanId)='" + ddlGoals.SelectedItem.Value + "'"));
            if (StudentLp > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Lesson plan name already exist. Please enter another name...');", true);
            }
            else
            {
                AddLessonPlan();
                lbLPBank_Click(sender, e);  //call the all lessonplan in bank button click to refill the choose goal and LP section.....
                clearAddNewLesson();
            }



        }
    }

    protected void clearAddNewLesson()
    {
        txtLPname.Text = "";
        ddlGoals.SelectedIndex = 0;
        txtFrameworkAndStandard.Text = "";
        txtSpecStandard.Text = "";
        txtSpecEntryPoint.Text = "";
        txtPreskills.Text = "";
        txtMaterials.Text = "";
        txtBaseline.Text = "";
        txtObjective.Text = "";
    }

    protected void grdLessonPlanDelete_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdLessonPlanDelete.PageIndex = e.NewPageIndex;

    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }
    protected void btnListDelLessonPlan_Click(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "popPrompts();", true);
    }

    protected void grdLessonPlanDelete_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        oData = new clsData();
        oSession = (clsSession)Session["UserSession"];

        curesesid = this.Session.SessionID.ToString();
        string HostName = Dns.GetHostName();
        string userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "AsmntReview:grdLessonPlanDelete_RowCommand";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log " + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
        }
        
        oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);

        if (e.CommandName == "View")
        {
            int lpId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("LessonPlanAttributes.aspx?delLpID=" + lpId + "&studid=" + oSession.StudentId + "");
        }

        else if (e.CommandName == "Activate")
        {
            int lpId = Convert.ToInt32(e.CommandArgument);
            string updateLP = "UPDATE StdtLessonPlan SET ActiveInd='A',ModifiedOn=getdate() WHERE StdtLessonPlanId=" + lpId;
            oData.Execute(updateLP);
            //     loadLessonplan();
            LoadData();
        }

    }
    protected void btnCopyLP_Click(object sender, EventArgs e)
    {
        SqlTransaction Trans = null;
        SqlConnection Con = null;
        oData = new clsData();
        objData = new DataClass();
        int tempHdrId = 0;
        int tempLpId = 0;
        int NewLpid = 0;
        try
        {
            Con = oData.Open();
            Trans = Con.BeginTransaction();

            object AsmntYr = oData.FetchValueTrans("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'", Trans, Con);
            tempLpId = Convert.ToInt32(hdLessonId.Value);
            tempHdrId = Convert.ToInt32(oData.FetchValueTrans("SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId=" + tempLpId + " AND StudentId IS NULL AND isDynamic=0", Trans, Con));
            oSession = (clsSession)Session["UserSession"];

            curesesid = this.Session.SessionID.ToString();
            string HostName = Dns.GetHostName();
            string userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AsmntReview:btnCopyLP_Click";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
            }
            
            oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);

            int gid = Convert.ToInt32(hdGoalId.Value);
            string strQuery = "";
            object objIEP = "";
            if (oSession != null)
            {
                if (oSession.SchoolId == 2)
                {
                    string selQry = "select isnull(StdtIEP_PEId,0) StdtIEP_PEId from StdtIEP_PE I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                        + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                        + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' ";
                    objIEP = oData.FetchValueTrans(selQry, Trans, Con);
                }
                else if (oSession.SchoolId == 1)
                {
                    string selQry = "select isnull(StdtIEPId,0) StdtIEPId from StdtIEP I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                        + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                        + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' ";
                    objIEP = oData.FetchValueTrans(selQry, Trans, Con);
                }
                if (objIEP == null)
                    objIEP = 0;

                string insLP = "insert into LessonPlan(SchoolId,[PreReq],[TeacherSD],[TeacherInst],[Consequence],[BaselineProc],[PostCheckProc],[ImageURL],[Materials],[FrameandStrand],[SpecStandard],[SpecEntryPoint],ActiveInd,LessonPlanName,CreatedBy,CreatedOn,[Baseline],[Objective]) " +
                    "SELECT " + oSession.SchoolId + ",[PreReq],[TeacherSD],[TeacherInst],[Consequence],[BaselineProc],[PostCheckProc],[ImageURL],[Materials],[FrameandStrand],[SpecStandard],[SpecEntryPoint],'A','" + clsGeneral.convertQuotes(hdLessonName.Value) + "'," + oSession.LoginId + ",GETDATE(),[Baseline],[Objective] FROM [dbo].[LessonPlan] WHERE LessonPlanId=" + tempLpId;
                NewLpid = oData.ExecuteWithScopeandConnection(insLP, Con, Trans);

                string qryclass = "select ResidenceInd from [dbo].[Class] where classid=" + oSession.Classid;
                bool isDay = Convert.ToBoolean(oData.FetchValueTrans(qryclass, Trans, Con));
                string condtn = "";

                if (NewLpid > 0)
                {
                    strQuery = "INSERT into GoalLPRel(GoalId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn) " +
                        "Values('" + gid + "'," + NewLpid + ",'A'," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                    oData.ExecuteWithScopeandConnection(strQuery, Con, Trans);

                    string insHDR = "insert into DSTempHdr(SchoolId,LessonPlanId,DSTemplateName,isDynamic,CreatedBy,CreatedOn) " +
                       "values(" + oSession.SchoolId + "," + NewLpid + ",'" + clsGeneral.convertQuotes(hdLessonName.Value) + "'," + 1 + "," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100))) ";
                    int Hid=oData.ExecuteWithScopeandConnection(insHDR, Con, Trans);

                    int StdtLpid = oData.ExecuteWithScopeandConnection("INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,IncludeIEP,ActiveInd,StatusId,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,CreatedOn,isDynamic) " +
                        " VALUES('" + oSession.SchoolId + "','" + oSession.StudentId + "'," + NewLpid + ",'" + gid + "','" + AsmntYr + "','false','A', " +
                        "(SELECT LookupId FROM LookUp WHERE LookupType='LP Status' AND LookupName='In Progress'),(SELECT LessonPlanTypeDay FROM StdtLessonPlan WHERE StdtLessonPlanId=(SELECT StdtLessonPlanId FROM DSTempHdr WHERE " +
                        "DSTempHdrId='" + tempHdrId + "')),(SELECT LessonPlanTypeResi FROM StdtLessonPlan WHERE StdtLessonPlanId=(SELECT StdtLessonPlanId FROM DSTempHdr WHERE DSTempHdrId='" + tempHdrId + "')),'" + oSession.LoginId + "',GETDATE(),1)", Con,Trans);
                    if (!isDay)
                        condtn = "SET LessonPlanTypeDay=1";
                    else
                        condtn = "SET LessonPlanTypeResi=1";

                    if (Convert.ToInt32(objIEP) == 0)
                        condtn += ", IncludeIEP='false'";
                    else
                        condtn += ", IncludeIEP='true'";

                    oData.ExecuteWithScopeandConnection("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + StdtLpid, Con, Trans);

                    int tempStudHdrId = Convert.ToInt32(oData.FetchValueTrans("SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId=" + tempLpId + " AND StudentId=" + oSession.StudentId, Trans, Con));
                    if (tempStudHdrId == 0)   //When LP and Template have same name
                        tempStudHdrId = tempHdrId;
                    if (tempStudHdrId == 0)   ///LP name conflict with a student LP and a LP which is not assigned to a student
                        tempStudHdrId = Hid;
                    int tempid = CopyCustomtemplate(tempStudHdrId, tempLpId, NewLpid, clsGeneral.convertQuotes( hdLessonName.Value), Convert.ToInt32(StdtLpid), Con, Trans);
                    if (tempid > 0)
                    {
                        CreateDocument(tempStudHdrId, tempid);
                        string UpdateLessonName = "UPDATE DSTempHdr SET DSTemplateName='" + clsGeneral.convertQuotes(hdLessonName.Value) + "',LessonPlanId='" + NewLpid + "',isDynamic=1 WHERE DSTempHdrId=" + tempid;
                        oData.ExecuteWithScopeandConnection(UpdateLessonName, Con, Trans);
                    }
                }                              
            }
            oData.CommitTransation(Trans, Con);
            LoadData();
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){showdivMessage();});", true);
            
        }
        catch (Exception Ex)
        {
            oData.RollBackTransation(Trans, Con);
            Con.Close();
            throw Ex;
        }
    }

    public int CopyCustomtemplate(int tempHdrId, int tempLpId, int NewLpid, string LessonName, int stdtLpId, SqlConnection Con, SqlTransaction Trans)
    {
        oData = new clsData();
        string strQuery = "";
        int oldSetId = 0;
        int parentSetId = 0;
        clsAssignLessonPlan AssignLP = new clsAssignLessonPlan();
        try
        {
            strQuery = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,StdtLessonplanId,DSTemplateName,BaselineStart,BaselineEnd,VTLessonId,[CompCurrInd],MultiStepInd," +
                "MultiSetsInd,CurrVerInd,VerBeginDate,VerEndDate,DSTemplateDesc,StatusId,TeachingProcId,SkillType,MatchToSampleType,MatchToSampleRecOrExp,NbrOfTrials,ChainType," +
                "TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,GeneralProcedure,BaselineProc,CorrRespDef,StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc," +
                "ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,Mistrial,MistrialResponse,TeacherPrepare,StudentPrepare,StudResponse," +
                "CreatedBy,CreatedOn,LessonPlanGoal,[FrameandStrand],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[LessonOrder]) SELECT " + oSession.SchoolId + "," + oSession.StudentId + "," + NewLpid + "," +
                "'" + stdtLpId + "','" + LessonName + "',BaselineStart,BaselineEnd,VTLessonId,[CompCurrInd],MultiStepInd,MultiSetsInd,CurrVerInd,VerBeginDate,VerEndDate," +
                "DSTemplateDesc,(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress'),TeachingProcId,SkillType,MatchToSampleType,MatchToSampleRecOrExp," +
                "NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,GeneralProcedure,BaselineProc,CorrRespDef,StudCorrRespDef,IncorrRespDef," +
                "StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,Mistrial,MistrialResponse,TeacherPrepare," +
                "StudentPrepare,StudResponse,CreatedBy,getdate(),LessonPlanGoal,(SELECT [FrameandStrand] FROM LessonPlan WHERE LessonPlanId=" + tempLpId + ") FrameandStrand," +
                "(SELECT [SpecStandard] FROM LessonPlan WHERE LessonPlanId=" + tempLpId + ") SpecStandard,(SELECT [SpecEntryPoint]  FROM LessonPlan WHERE LessonPlanId=" + tempLpId + ") " +
                "SpecEntryPoint,(SELECT [PreReq] FROM LessonPlan WHERE LessonPlanId=" + tempLpId + ") PreReq,(SELECT [Materials] FROM LessonPlan WHERE LessonPlanId=" + tempLpId + ") Materials," +
                "(select isnull( max(LessonOrder)+1,1) from dstemphdr where studentid=" + oSession.StudentId + ") LessonOrder FROM DSTempHdr WHERE DSTempHdrId = " + tempHdrId;

            int TId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

            DataTable dtpromt = new DataTable();
            dtpromt = oData.ReturnDataTable("SELECT DSTempPromptId FROM DSTempPrompt WHERE DSTempHdrId=" + tempHdrId + "", Con, Trans, false);
            if (dtpromt != null)
            {
                if (dtpromt.Rows.Count > 0)
                {
                    foreach (DataRow row in dtpromt.Rows)
                    {
                        strQuery = "INSERT INTO DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT " + TId + ",PromptId,PromptOrder,ActiveInd," + oSession.LoginId + ",CreatedOn FROM DSTempPrompt WHERE DSTempPromptId=" + Convert.ToInt32(row["DSTempPromptId"]) + "";
                        int PromptId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            DataTable dtset = new DataTable();
            Hashtable ht = new Hashtable();
            dtset = oData.ReturnDataTable("SELECT DSTempSetId FROM DSTempSet WHERE DSTempHdrId=" + tempHdrId + "", Con, Trans, false);
            if (dtset != null)
            {
                if (dtset.Rows.Count > 0)
                {
                    foreach (DataRow row in dtset.Rows)
                    {
                        strQuery = "INSERT INTO DSTempSet(SchoolId,DSTempHdrId,PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT  SchoolId," + TId + ",PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd," + oSession.LoginId + ",getdate() FROM DSTempSet WHERE DSTempSetId = " + Convert.ToInt32(row["DSTempSetId"]) + " ";
                        int SetId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
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
                    "JOIN LookUp LU ON TeachingProcId=LU.LookUpId INNER JOIN Lookup LUp ON LUp.LookupId=PromptTypeId WHERE DSTempHdrId=" + tempHdrId;
            DataTable dtTmpHdrDtls = oData.ReturnDataTable(sqlStr, false);
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
                dtstep = oData.ReturnDataTable("SELECT DSTempStepId,DSTempSetId FROM DSTempStep WHERE DSTempHdrId=" + tempHdrId + " AND IsDynamic=0", Con, Trans, false);
                if (dtstep.Rows.Count > 0)
                {
                    foreach (DataRow row in dtstep.Rows)
                    {
                        oldSetId = Convert.ToInt32(row["DSTempSetId"]);
                        if (oldSetId != 0)
                        {
                            parentSetId = AssignLP.SetUpdateCopy(oldSetId, TId, Trans, Con);
                        }
                        strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                        strQuery += "SELECT SchoolId," + TId + "," + parentSetId + ",PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + oSession.LoginId + ",ActiveInd,GETDATE() FROM DSTempStep WHERE DSTempStepId = " + Convert.ToInt32(row["DSTempStepId"]) + " AND IsDynamic=0 ";
                        int StepId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            else
            {
                int oldParentSetId = 0;
                DataTable dtParentStep = new DataTable();
                strQuery = "SELECT  DSTempParentStepId,SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn"
                    + " FROM DSTempParentStep WHERE DSTempHdrId = " + tempHdrId;
                dtParentStep = oData.ReturnDataTable(strQuery, Con, Trans, false);
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
                                        + "SELECT  SchoolId," + TId + ",StepCd,StepName,DSTempSetId,SortOrder,'" + newsetids + "',SetNames,ActiveInd," + oSession.LoginId + ",getdate()"
                                        + " FROM DSTempParentStep WHERE DSTempHdrId = " + tempHdrId + " AND DSTempParentStepId=" + oldParentSetId;
                            parentSetId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                            DataTable dtstep = new DataTable();
                            strQuery = "SELECT  SchoolId,PrevStepId,SortOrder,PreDefinedInd,CustomById,VTStepId,DSTempSetId,StepCd,StepName,ActiveInd,"
                                    + "DSTempParentStepId FROM DSTempStep WHERE DSTempParentStepId=" + oldParentSetId + " AND IsDynamic=0 AND DSTempHdrId = " + tempHdrId;
                            dtstep = oData.ReturnDataTable(strQuery, Con, Trans, false);
                            if (dtstep.Rows.Count > 0)
                            {
                                foreach (DataRow rows in dtstep.Rows)
                                {
                                    oldSetId = Convert.ToInt32(rows["DSTempSetId"]);
                                    strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                                    strQuery += "SELECT SchoolId," + TId + ",DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + oSession.LoginId + ",ActiveInd,GETDATE()"
                                        + "	FROM DSTempStep WHERE DSTempSetId = " + oldSetId + " AND IsDynamic=0 AND DSTempParentStepId=" + oldParentSetId;
                                    int StepId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    strQuery = "SELECT DSTempSetId FROM DSTempStep WHERE DSTempStepId=" + StepId + " AND IsDynamic=0";
                                    int NewSetId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    if (ht.ContainsKey(Convert.ToInt32(NewSetId)))
                                    {
                                        newsetids = ht[Convert.ToInt32(NewSetId)].ToString();
                                        strQuery = "UPDATE DSTempStep SET DSTempSetId=" + Convert.ToInt32(ht[Convert.ToInt32(NewSetId)]) + ",DSTempParentStepId=" + parentSetId + " "
                                            + " WHERE DSTempStepId=" + StepId + " AND IsDynamic=0";
                                        int updateId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            DataTable dtsetcol = new DataTable();
            dtsetcol = oData.ReturnDataTable("SELECT DSTempSetColId FROM DSTempSetCol WHERE DSTempHdrId=" + tempHdrId + "", Con, Trans, false);
            if (dtsetcol != null)
            {
                if (dtsetcol.Rows.Count > 0)
                {
                    foreach (DataRow row in dtsetcol.Rows)
                    {
                        strQuery = "INSERT INTO DSTempSetCol(SchoolId, DSTempHdrId,ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp	,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT SchoolId, " + TId + ",ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd," + oSession.LoginId + ",CreatedOn FROM DSTempSetCol WHERE DSTempSetColId = " + Convert.ToInt32(row["DSTempSetColId"]) + " ";
                        int setColNewId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        DataTable dtsetcolcalc = new DataTable();
                        dtsetcolcalc = oData.ReturnDataTable("SELECT DSTempSetColCalcId FROM DSTempSetColCalc WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + "", Con, Trans, false);
                        if (dtsetcolcalc.Rows.Count > 0)
                        {
                            foreach (DataRow rowc in dtsetcolcalc.Rows)
                            {
                                strQuery = "INSERT INTO DSTempSetColCalc(SchoolId,DSTempSetColId,CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd,CreatedBy,CreatedOn,IncludeInGraph) " +
                                            "SELECT SchoolId," + setColNewId + ",CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd," + oSession.LoginId + ",getdate(),IncludeInGraph FROM DSTempSetColCalc WHERE DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + "";
                                int setColCalId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                                strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) ";
                                strQuery += "SELECT  " + TId + ",SchoolId," + setColNewId + "," + setColCalId + ",RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + " And DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + " ";
                                int lastId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                            }
                        }
                    }
                    strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) ";
                    strQuery += "SELECT  " + TId + ",SchoolId,0,0,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,MultiTeacherReqInd,IOAReqInd,"
                        + "LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE" +
                        " DSTempSetColId=0 And DSTempSetColCalcId=0 AND DSTempHdrId=" + tempHdrId;
                    int lastModRuleId = Convert.ToInt32(oData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                }
            }
            return TId;
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            return 0;
        }
    }

    protected void CreateDocument(int tempid, int newtempId)
    {
        try
        {
            oData = new clsData();
            oSession = (clsSession)Session["UserSession"];

            curesesid = this.Session.SessionID.ToString();
            string HostName = Dns.GetHostName();
            string userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AsmntReview:CreateDocument";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ":" + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(DateTime.Now.ToString() + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + HostName + ',' + userAgent);
            }
            
            oSession = clsGeneral.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, Sessstname, Pagepath);

            DataTable dtdoc = new DataTable();
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            dtdoc = oData.ReturnDataTable("SELECT LPDoc FROM LPDoc WHERE DSTempHdrId=" + tempid + "", false);
            if (dtdoc != null)
            {
                if (dtdoc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtdoc.Rows)
                    {
                        string strquerry = "INSERT INTO LPDoc(SchoolId,DSTempHdrId,DocURL,CreatedBy,CreatedOn) SELECT SchoolId," + newtempId + ",DocURL," + oSession.LoginId + ",GETDATE() FROM LPDoc WHERE LPDoc='" + row["LPDoc"].ToString() + "'";
                        int docid = oData.ExecuteWithScope(strquerry);
                        string binarydata = "SELECT Data,DocumentName FROM binaryFiles WHERE DocId=" + row["LPDoc"].ToString() + " AND type='LP_DOC' ";
                        DataTable dtbinary = oData.ReturnDataTable(binarydata, false);
                        byte[] myData = (byte[])dtbinary.Rows[0]["Data"];
                        string filename = Convert.ToString(dtbinary.Rows[0]["DocumentName"]);
                        int binaryid = objBinary.saveDocument(myData, filename, "", "LP_DOC", docid, "LessonPlanDoc", oSession.SchoolId, 0, oSession.LoginId);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string SearchLessonPlanList(string Name)
    {
        try
        {
            object GoalApproved = HttpContext.Current.Session["GoalID_Approved"];
            clsData oData = new clsData();
            clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            int StudentLp = 0;
            //Template = Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM DSTempHdr WHERE  RTRIM(LTRIM(LOWER(DSTemplateName)))= RTRIM(LTRIM(LOWER('" + Name.Trim() + "'))) AND StudentId IS NULL AND isDynamic=0"));
            StudentLp = Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM DSTempHdr inner join LookUp lu on lu.LookupId=DSTempHdr.StatusId WHERE  RTRIM(LTRIM(LOWER(DSTemplateName)))= RTRIM(LTRIM(LOWER('" + Name.Trim() + "'))) AND StudentId='" + oSession.StudentId + "' AND (SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId=DSTempHdr.StdtLessonplanId)='" + GoalApproved + "' AND lu.LookupName <> 'Deleted' AND lu.LookupType='TemplateStatus'"));
            if (StudentLp > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
}