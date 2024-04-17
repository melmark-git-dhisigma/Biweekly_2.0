using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using Microsoft.Office.Interop.Word;
using System.Xml;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

public partial class StudentBinder_ProgressReport : System.Web.UI.Page
{
    public static clsData objData = null;
    public static clsSession sess = null;
    static string[] columns;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            sess = (clsSession)Session["UserSession"];
            objData = new clsData();
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

            string evTar = "";
            string evArg = "";
            if (Request.Form["__EVENTTARGET"] != null)
            {
                evTar = Request.Form["__EVENTTARGET"];
            }
            if (Request.Form["__EVENTARGUMENT"] != null)
            {
                evArg = Request.Form["__EVENTARGUMENT"];
            }


            if (evTar == "ExportAll")
            {
                int curIEPID = 0;
                if (evArg != null)
                    curIEPID = Convert.ToInt32(evArg);
                if (curIEPID > 0)
                {
                    //DataTable DTGoal = objData.ReturnDataTable("SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId,LP.LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3 FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND SLP.LessonPlanId=GLPR.LessonPlanId INNER JOIN LessonPlan LP ON LP.LessonPlanId=SLP.LessonPlanId WHERE SLP.StdtIEPId='" + curIEPID + "' AND SLP.ActiveInd='A' AND SLP.IncludeIEP=1", false);
                    DataTable DTGoal = objData.ReturnDataTable("SELECT * FROM (SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId, " +
                              "LP.LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3,(SELECT TOP 1 IEPGoalNo FROM StdtGoal WHERE StdtIEPId=SLP.StdtIEPId AND GoalId=SLP.GoalId AND StudentId=SLP.StudentId " +
                              "ORDER BY StdtGoalId DESC ) IEPGoalNo,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE StudentId=SLP.StudentId AND DSTempHdr.LessonPlanId=SLP.LessonPlanId) LessonOrder FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND " +
                              "SLP.LessonPlanId=GLPR.LessonPlanId INNER JOIN LessonPlan LP ON LP.LessonPlanId=SLP.LessonPlanId WHERE SLP.StdtIEPId=" + curIEPID + " AND SLP.ActiveInd='A' AND SLP.IncludeIEP=1) LESSON ORDER BY IEPGoalNo,LessonOrder", false);
            
                    if (DTGoal != null)
                    {
                        if (DTGoal.Rows.Count > 0)
                        {
                            ExportAll(curIEPID);
                            return;
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(GetType(), "No IEP", "NoIEPToExport();", true);
                            return;
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "No IEP", "NoIEPToExport();", true);
                    return;
                }
            }
            if (!IsPostBack)
            {
                int IepId = 0;
                string IepExist = "SELECT StdtIEPId FROM StdtIEP WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved')";
                if (objData.IFExists(IepExist))
                {
                    IepId = Convert.ToInt32(objData.FetchValue("SELECT StdtIEPId FROM StdtIEP WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved') ORDER BY StdtIEPId DESC"));
                }
                GetData(IepId,"Load");
                GetProgressList();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void GetProgressList()
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        string IepExist = "SELECT StdtIEPId,CONVERT(VARCHAR(10),EffStartDate,101) AS EffStartDate,CONVERT(VARCHAR(10),EffEndDate,101) AS EffEndDate,(SELECT LookupName FROM LookUp WHERE LookupId=StatusId) AS Status FROM StdtIEP WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName IN ('Approved','Expired')) ORDER BY StdtIEPId DESC";
        DataTable DTProgressList = objData.ReturnDataTable(IepExist, false);
        GrdProgressList.DataSource = DTProgressList;
        GrdProgressList.DataBind();
    }
    private void GetData(int CurrentIepId,string displayType)
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();

        string studName ="";
        string studNbr = "";
        string studDOB = "";
        string DistrictName = "";
        string DistrictAddress = "";
        string DistrictContact = "";
        string ProgressReport = "";
        ProgressRpt.InnerHtml = "";
        string submitType = "";
        DataTable dtStudDtl = objData.ReturnDataTable("select LastName,FirstName,StudentPersonalId,FORMAT(BirthDate,'MM/dd/yyyy') as BirthDate from StudentPersonal where studentpersonalid='" + sess.StudentId + "'", false);
        DataTable dtSchDtl = objData.ReturnDataTable("select DistrictName,AddressLine1+', '+AddressLine2+', '+City+', '+(SELECT LookupName FROM LookUp WHERE LookupId=State)+', '+Zip AS DistrictAddress,DistContact+'/'+DistPhone AS DistrictContact from School sch inner join Address addr ON sch.AddressId=addr.AddressId where SchoolId='" + sess.SchoolId + "'", false);
        if (dtStudDtl != null)
        {
            studName = dtStudDtl.Rows[0]["LastName"].ToString() + ", " + dtStudDtl.Rows[0]["FirstName"].ToString();
            studNbr = dtStudDtl.Rows[0]["StudentPersonalId"].ToString();
            studDOB = dtStudDtl.Rows[0]["BirthDate"].ToString();
        }
        if (dtSchDtl != null)
        {
            DistrictName = dtSchDtl.Rows[0]["DistrictName"].ToString();
            DistrictAddress = dtSchDtl.Rows[0]["DistrictAddress"].ToString();
            DistrictContact = dtSchDtl.Rows[0]["DistrictContact"].ToString();
        }

        if (CurrentIepId != 0)
        {
            
            DataTable DTSchoolDetails = objData.ReturnDataTable("SELECT DistrictName,AddressLine1+','+AddressLine2+','+City+','+(SELECT LookupName FROM LookUp WHERE LookupId=State)+','+Zip AS DistrictAddress,DistContact+'/'+DistPhone AS DistrictContact FROM School SCL INNER JOIN Address ADR ON SCL.AddressId=ADR.AddressId WHERE SchoolId='" + sess.SchoolId + "'", false);
            string GoalIds = Convert.ToString(objData.FetchValue("SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),GoalLPRelId) FROM (SELECT SLP.GoalId,GLPR.GoalLPRelId FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND SLP.LessonPlanId=GLPR.LessonPlanId WHERE SLP.StdtIEPId='"+CurrentIepId+"') LP FOR XML PATH('')),1,1,'')"));
            string IEPGoalid = "saveProgressReport('" + GoalIds + "')";
            if (displayType == "Load") submitType = "<input id='btnsubmit' type='button'  class='NFButton' value='Submit' onclick=" + IEPGoalid + " />";
            
            DTSchoolDetails = objData.ReturnDataTable(" SELECT CONVERT(VARCHAR,EffStartDate,101) AS EffStartDate,CONVERT(VARCHAR,EffEndDate,101) AS EffEndDate,StudentLname+','+StudentFname AS StudentName,StudentNbr,CONVERT(VARCHAR,DOB,101) AS DOB FROM StdtIEP IEP INNER JOIN Student STU ON IEP.StudentId=STU.StudentId WHERE StdtIEPId='" + CurrentIepId + "'", false);
            //DataTable DTGoal = objData.ReturnDataTable("SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId,(SELECT CASE WHEN DSTemplateName IS NULL THEN LP.LessonPlanName ELSE DSTemplateName END DSTemplateName FROM DSTempHdr HDR INNER JOIN LessonPlan LP ON HDR.LessonPlanId=LP.LessonPlanId WHERE StdtLessonPlanId=SLP.StdtLessonPlanId) AS LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3 FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND SLP.LessonPlanId=GLPR.LessonPlanId WHERE SLP.StdtIEPId='" + CurrentIepId + "'", false);
            DataTable DTGoal = objData.ReturnDataTable("SELECT * FROM (SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId, "+
                               "LP.LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3,(SELECT TOP 1 IEPGoalNo FROM StdtGoal WHERE StdtIEPId=SLP.StdtIEPId AND GoalId=SLP.GoalId AND StudentId=SLP.StudentId "+
                               "ORDER BY StdtGoalId DESC ) IEPGoalNo,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE StudentId=SLP.StudentId AND DSTempHdr.LessonPlanId=SLP.LessonPlanId) LessonOrder FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND "+
                               "SLP.LessonPlanId=GLPR.LessonPlanId INNER JOIN LessonPlan LP ON LP.LessonPlanId=SLP.LessonPlanId WHERE SLP.StdtIEPId=" + CurrentIepId + " AND SLP.ActiveInd='A' AND SLP.IncludeIEP=1) LESSON ORDER BY IEPGoalNo,LessonOrder", false);
            
            //DataTable DTGoal = objData.ReturnDataTable("SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId,LP.LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3 FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND SLP.LessonPlanId=GLPR.LessonPlanId INNER JOIN LessonPlan LP ON LP.LessonPlanId=SLP.LessonPlanId WHERE SLP.StdtIEPId='" + CurrentIepId + "' AND SLP.ActiveInd='A' AND SLP.IncludeIEP=1", false);


            if (DTGoal != null)
            {
                if (DTGoal.Rows.Count > 0)
                {
                    //ProgressReport = "<div id='Sample' style='width: 100%'><table id='tblSchooldetails' style='width: 100%'><tr><td class='tdLabel' style='width:22%'>School District Name:</td><td colspan='2'>" + DTSchoolDetails.Rows[0]["DistrictName"] + "" +
                    //    "</td><td>" + submitType + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='btnExp1' type='button' value='Export' class='NFButton' onclick='ExportAll()' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='Button1' type='button' class='NFButton' value='Progress List' onclick='ProgressReport()' />" +
                    //    "</td></tr><tr><td class='tdLabel'>School District Address:</td><td colspan='2'> " + DTSchoolDetails.Rows[0]["DistrictAddress"] + "</td><td></td></tr><tr><td class='tdLabel'>School District Contact Person/Phone #:</td><td colspan='2'>" + DTSchoolDetails.Rows[0]["DistrictContact"] + "</td>" +
                    //    "</tr></table><br /></div>";
                    ProgressReport = "<div id='Sample' style='width: 100%'><table id='tblSchooldetails' style='width: 100%'><tr><td class='tdLabel' style='width:22%'>School District Name:</td><td colspan='2'>" + DistrictName + "" +
                        "</td><td>" + submitType + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='btnExp2' type='button' value='Export' class='NFButton' onclick='ExportAll(" + CurrentIepId + ")' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='Button1' type='button' class='NFButton' value='Progress List' onclick='ProgressReport()' />" +
                        "</td></tr><tr><td class='tdLabel'>School District Address:</td><td colspan='2'> " + DistrictAddress + "</td><td></td></tr><tr><td class='tdLabel'>School District Contact Person/Phone #:</td><td colspan='2'>" + DistrictContact + "</td>" +
                        "</tr></table><br /></div>";
                    ProgressRpt.InnerHtml = ProgressReport;
                    foreach (DataRow row in DTGoal.Rows)
                    {
                        int cnt = 1;
                        ProgressReport = "<div  style='width: 100%'><hr style='width: 100%; border: 5px solid;' /><br /><table style='width: 100%'><tr><td>Progress Report</td><td>on IEP Dated: from &nbsp;" + DTSchoolDetails.Rows[0]["EffStartDate"] + "</td>" +
                                         "<td>to &nbsp; " + DTSchoolDetails.Rows[0]["EffEndDate"] + "</td></tr></table><hr  style='width:100%' border:'solid' /><table style='width: 100%'><tr><td class='tdLabel'>Student Name:&nbsp;&nbsp;" + DTSchoolDetails.Rows[0]["StudentName"] + "</td>" +
                                         "<td class='tdLabel'>DOB:&nbsp;&nbsp;" + DTSchoolDetails.Rows[0]["DOB"] + "</td><td class='tdLabel'>ID #:&nbsp;&nbsp;" + DTSchoolDetails.Rows[0]["StudentNbr"] + "</td></tr></table><div style='width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center'>INFORMATION FROM CURRENT IEP</div>" +
                                         "<table style='width: 100%; border: thick'><tr><td>Goal #:&nbsp;&nbsp;" + row["IEPGoalNo"] + "</td><td>Specific Goal Focus:&nbsp;" + row["LessonPlanName"] + "</td></tr></table><table style='width:100%'><tr><td>Current Performance Level:&nbsp; What can the student currently do?" +
                                         "</td></tr><tr><td><ul><li>" + row["Objective1"] + "</li></ul></td></tr></table><hr  style='width:100%' border:'solid' /><table style='width:100%'><tr><td style='width:60%'>Measurable Annual Goal:What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?" +
                                         "</td><td style='width:40%'>How will we know that the student has reached this goal?</td></tr><tr><td><ul><li>" + row["Objective2"] + "</li></ul></td></tr></table><hr  style='width:100%' border:'solid' /><table style='width:100%'><tr><td>Benchmark/Objectives: What will the student need to do to complete this goal?" +
                                         "</td></tr><tr><td><ul><li>" + row["Objective3"] + "</li></ul></td></tr></table>";
                        ProgressRpt.InnerHtml += ProgressReport;
                        DataTable DtReport = objData.ReturnDataTable("SELECT CONVERT(VARCHAR,Report_Date,101) AS Report_Date,Report_Info,Report_Id FROM Progress_Report WHERE GoalId='" + row["GoalLPRelId"] + "' AND StdtIEPId='" + CurrentIepId + "'", false);
                        if (DtReport != null)
                        {
                            if (DtReport.Rows.Count > 0)
                            {
                                ProgressReport = "<table id='tbl_" + row["GoalLPRelId"] + "' style='width: 100%; border: thick'><tr><td colspan='3' style='color:white;background-color:black;text-align:center'>PROGRESS REPORT INFORMATION</td></tr>";
                                int Cnt = 1;
                                foreach (DataRow rpt in DtReport.Rows)
                                {

                                    ProgressReport += "<tr><td>Progress Report Date:&nbsp;&nbsp;<input type='text' class='datepicker' id='date_" + Cnt + "_" + row["GoalLPRelId"] + "' value='" + rpt["Report_Date"] + "'  onkeydown='allowBackSpace(event,this)' onpaste='return false' onkeypress='return false' /></td><td></td><td>Progress Report # " + Cnt + " of  4</td></tr>" +
                                    "<tr><td colspan='3'>Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.</td></tr>" +
                                    "<tr><td colspan='3'><input id='info_" + Cnt + "_" + row["GoalLPRelId"] + "' type='text' style='width: 90%; height: 60px; border: 1px solid #ccc' value='" + (rpt["Report_Info"]).ToString().Replace("'", "&#39;") + "' /></td></tr>" +
                                    "<tr><td colspan='3'><hr  style='width:100%' border:'solid' /></td></tr>";
                                    Cnt++;
                                }
                                ProgressReport += "<tr><td colspan='3' ><input id='btn_" + row["GoalLPRelId"] + "'  type='button'  class='NFButton' value='ADD HERE' onclick='    AddBtn_Click(this.id)' /></td></tr></table></div>";
                                ProgressRpt.InnerHtml += ProgressReport;
                            }
                            else
                            {
                                ProgressReport = "<table id='tbl_" + row["GoalLPRelId"] + "' style='width: 100%; border: thick'><tr><td colspan='3' style='color:white;background-color:black;text-align:center'>PROGRESS REPORT INFORMATION</td></tr><tr><td>Progress Report Date:&nbsp;&nbsp;<input type='text' id='date_" + cnt + "_" + row["GoalLPRelId"] + "'  class='datepicker' value='' onkeydown='allowBackSpace(event,this)' onpaste='return false' onkeypress='return false' /></td><td></td><td>Progress Report # " + cnt + " of  4</td></tr><tr><td colspan='3'>" +
                                                 "Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal." +
                                                 "</td></tr><tr><td colspan='3'><input id='info_" + cnt + "_" + row["GoalLPRelId"] + "' type='text' style='width: 90%; height: 60px; border: 1px solid #ccc' /></td></tr><tr><td colspan='3'  ><input id='btn_" + row["GoalLPRelId"] + "'  type='button'   class='NFButton' value='ADD HERE' onclick='    AddBtn_Click(this.id)' />" +
                                                 "</td></tr></table></div>";
                                ProgressRpt.InnerHtml += ProgressReport;
                            }
                        }

                    }


                }
                else
                {
                    //ProgressRpt.InnerHtml = "<div>No Goals and Lesson Plans Found in IEP For Progress Report....</div>";
                    submitType = "";
                    ProgressReport = "<div id='Sample' style='width: 100%'><table id='tblSchooldetails' style='width: 100%'><tr><td class='tdLabel' style='width:22%'>School District Name:</td><td colspan='2'>" + DistrictName + "" +
                        "</td><td>" + submitType + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='Button1' type='button' class='NFButton' value='Progress List' onclick='ProgressReport()' />" +
                        "</td></tr><tr><td class='tdLabel'>School District Address:</td><td colspan='2'> " + DistrictAddress + "</td><td></td></tr><tr><td class='tdLabel'>School District Contact Person/Phone #:</td><td colspan='2'>" + DistrictContact + "</td>" +
                        "</tr></table><br /></div>";
                    ProgressRpt.InnerHtml += ProgressReport;
                    ProgressReport = "<div  style='width: 100%'><hr style='width: 100%; border: 5px solid;' /><br /><table style='width: 100%'><tr><td>Progress Report</td><td>on IEP Dated: from &nbsp;" + DTSchoolDetails.Rows[0]["EffStartDate"] + "</td>" +
                                         "<td>to &nbsp; " + DTSchoolDetails.Rows[0]["EffEndDate"] + "</td></tr></table><hr  style='width:100%' border:'solid' /><table style='width: 100%'><tr><td class='tdLabel'>Student Name:&nbsp;&nbsp;" + DTSchoolDetails.Rows[0]["StudentName"] + "</td>" +
                                         "<td class='tdLabel'>DOB:&nbsp;&nbsp;" + DTSchoolDetails.Rows[0]["DOB"] + "</td><td class='tdLabel'>ID #:&nbsp;&nbsp;" + DTSchoolDetails.Rows[0]["StudentNbr"] + "</td></tr></table><div style='width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center'>INFORMATION FROM CURRENT IEP</div>" +
                                         "<table style='width: 100%; border: thick'><tr><td>Goal #:&nbsp;&nbsp;" + " " + "</td><td>Specific Goal Focus:&nbsp;" + " " + "</td></tr></table><table style='width:100%'><tr><td>Current Performance Level:&nbsp; What can the student currently do?" +
                                         "</td></tr><tr><td><ul><li><b>" + " " + "</b></li></ul></td></tr></table><hr  style='width:100%' border:'solid' /><table style='width:100%'><tr><td style='width:60%'>Measurable Annual Goal:What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?" +
                                         "</td><td  style='width:40%'>How will we know that the student has reached this goal?</td></tr><tr><td><ul><li><b>" + " " + "</b></li></ul></td></tr></table><hr  style='width:100%' border:'solid' /><table style='width:100%'><tr><td>Benchmark/Objectives: What will the student need to do to complete this goal?" +
                                         "</td></tr><tr><td><ul><li><b>" + " " + "</b></li></ul></td></tr></table>";
                    ProgressRpt.InnerHtml += ProgressReport;
                    ProgressReport = "<table id='tbl_GoalLPRelId' style='width: 100%; border: thick'><tr><td colspan='3' style='color:white;background-color:black;text-align:center'>PROGRESS REPORT INFORMATION</td></tr><tr><td>Progress Report Date:&nbsp;&nbsp;<input type='text' id='date_GoalLPRelId'  class='datepicker' value='' onkeydown='allowBackSpace(event,this)' onpaste='return false' onkeypress='return false' /></td><td></td><td>Progress Report # 0 of  4</td></tr><tr><td colspan='3'>" +
                                                 "Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal." +
                                                 "</td></tr><tr><td colspan='3'><input id='info_GoalLPRelId' type='text' style='width: 90%; height: 60px; border: 1px solid #ccc' /></td></tr><tr><td colspan='3'  ><input id='btn_GoalLPRelId'  type='button'   class='NFButton' value='ADD HERE' onclick='    AddBtn_Click(this.id)' />" +
                                                 "</td></tr></table></div>";
                    ProgressRpt.InnerHtml += ProgressReport;
                }
            }
        }
        else
        {
            //ProgressRpt.InnerHtml = "<div>No IEP data found for progress report....</div>";
            ProgressReport = "<div id='Sample' style='width: 100%'><table id='tblSchooldetails' style='width: 100%'><tr><td class='tdLabel' style='width:22%'>School District Name:</td><td colspan='2'>" + DistrictName + "" +
                        "</td><td>" + submitType + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='Button1' type='button' class='NFButton' value='Progress List' onclick='ProgressReport()' />" +
                        "</td></tr><tr><td class='tdLabel'>School District Address:</td><td colspan='2'> " + DistrictAddress + "</td><td></td></tr><tr><td class='tdLabel'>School District Contact Person/Phone #:</td><td colspan='2'>" + DistrictContact + "</td>" +
                        "</tr></table><br /></div>";
            ProgressRpt.InnerHtml += ProgressReport;
            ProgressReport = "<div  style='width: 100%'><hr style='width: 100%; border: 5px solid;' /><br /><table style='width: 100%'><tr><td>Progress Report</td><td>on IEP Dated: from &nbsp;" + " " + "</td>" +
                                         "<td>to &nbsp; " + " " + "</td></tr></table><hr  style='width:100%' border:'solid' /><table style='width: 100%'><tr><td class='tdLabel'>Student Name:&nbsp;&nbsp;" + studName + "</td>" +
                                         "<td class='tdLabel'>DOB:&nbsp;&nbsp;" + studDOB + "</td><td class='tdLabel'>ID #:&nbsp;&nbsp;" + studNbr + "</td></tr></table><div style='width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center'>INFORMATION FROM CURRENT IEP</div>" +
                                         "<table style='width: 100%; border: thick'><tr><td>Goal #:&nbsp;&nbsp;" + " " + "</td><td>Specific Goal Focus:&nbsp;" + " " + "</td></tr></table><table style='width:100%'><tr><td>Current Performance Level:&nbsp; What can the student currently do?" +
                                         "</td></tr><tr><td><ul><li><b>" + " " + "</b></li></ul></td></tr></table><hr  style='width:100%' border:'solid' /><table style='width:100%'><tr><td style='width:60%'>Measurable Annual Goal:What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?" +
                                         "</td><td style='width:40%'>How will we know that the student has reached this goal?</td></tr><tr><td><ul><li><b>" + " " + "</b></li></ul></td></tr></table><hr  style='width:100%' border:'solid' /><table style='width:100%'><tr><td>Benchmark/Objectives: What will the student need to do to complete this goal?" +
                                         "</td></tr><tr><td><ul><li><b>" + " " + "</b></li></ul></td></tr></table>";
            ProgressRpt.InnerHtml += ProgressReport;
            ProgressReport = "<table id='tbl_GoalLPRelId' style='width: 100%; border: thick'><tr><td colspan='3' style='color:white;background-color:black;text-align:center'>PROGRESS REPORT INFORMATION</td></tr><tr><td>Progress Report Date:&nbsp;&nbsp;<input type='text' id='date_GoalLPRelId'  class='datepicker' value='' onkeydown='allowBackSpace(event,this)' onpaste='return false' onkeypress='return false' /></td><td></td><td>Progress Report # 0 of  4</td></tr><tr><td colspan='3'>" +
                                         "Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal." +
                                         "</td></tr><tr><td colspan='3'><input id='info_GoalLPRelId' type='text' style='width: 90%; height: 60px; border: 1px solid #ccc' /></td></tr><tr><td colspan='3'  ><input id='btn_GoalLPRelId'  type='button'   class='NFButton' value='ADD HERE' onclick='    AddBtn_Click(this.id)' />" +
                                         "</td></tr></table></div>";
            ProgressRpt.InnerHtml += ProgressReport;
        }
    }


    [WebMethod]
    public static string SaveProgressReport(string[] Data2)
    {
        string Result = "";
        try
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            objData = new clsData();
            string dataArray = Data2.ToString();
            //string[] arrayval = dataArray.Split(',');
            string[] arrayval = Data2;
            int IepId = Convert.ToInt32(objData.FetchValue("SELECT StdtIEPId FROM StdtIEP WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved') ORDER BY StdtIEPId DESC"));
            string IepExist = "SELECT StdtIEPId FROM Progress_Report WHERE StdtIEPId='" + IepId + "' ";
            if (objData.IFExists(IepExist))
            {
                int DeletePrptData = objData.Execute("DELETE FROM Progress_Report WHERE StdtIEPId='" + IepId + "'");
            }
            for (int i = 0; i < arrayval.Length; i++)
            {
                string[] goal_date_info = Convert.ToString(arrayval[i]).Split('_');
                if (goal_date_info[1] != "") goal_date_info[1] = ReturnDate(goal_date_info[1]);
                if (goal_date_info[1] != "" || goal_date_info[2] != "")
                {
                    int InsertPropData = objData.Execute("INSERT INTO Progress_Report(Report_Date,Report_Info,GoalId,CreatedBy,CreatedOn,StdtIEPId,ModifiedBy,ModifiedOn) VALUES ('" + goal_date_info[1] + "','" + clsGeneral.convertQuotes(goal_date_info[2]) + "','" + goal_date_info[0] + "','" + sess.LoginId + "',GETDATE(),'" + IepId + "','" + sess.LoginId + "',GETDATE())");
                    Result = "Saved Successfully...";
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

        return Result;
    }
    public static string ReturnDate(string Rptdate)
    {
        try
        {
            if (Rptdate.Contains("/"))
            {
                string[] dateVal = Rptdate.Split('/');
                Rptdate = dateVal[2] + "-" + dateVal[0] + "-" + dateVal[1];
            }
            else if (Rptdate.Contains("-"))
            {
                string[] dateVal = Rptdate.Split('-');
                Rptdate = dateVal[1] + "/" + dateVal[2] + "/" + dateVal[0];
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return Rptdate;
    }
    protected void GrdProgressList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            objData = new clsData();
            int IepId = Convert.ToInt32(e.CommandArgument);
            int ApproveIEPId = Convert.ToInt32(objData.FetchValue("SELECT StdtIEPId FROM StdtIEP WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved') ORDER BY StdtIEPId DESC"));
            string Type = "";
            if (IepId == ApproveIEPId) Type = "Load";
            GetData(IepId, Type);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void GrdProgressList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CleanDoc(string DocName)
    {
        using (WordprocessingDocument doc =
            WordprocessingDocument.Open(DocName, true))
        {
            SimplifyMarkupSettings settings = new SimplifyMarkupSettings
            {
                RemoveComments = true,
                RemoveContentControls = true,
                RemoveEndAndFootNotes = true,
                RemoveFieldCodes = false,
                RemoveLastRenderedPageBreak = true,
                RemovePermissions = true,
                RemoveProof = true,
                RemoveRsidInfo = true,
                RemoveSmartTags = true,
                RemoveSoftHyphens = true,
                ReplaceTabsWithSpaces = true,
            };
            MarkupSimplifier.SimplifyMarkup(doc, settings);
        }
    }

    private string CopyTemplate(string oldPath, string PageNo)
    {
        try
        {
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\Administration") + "\\TempPR1";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Guid g;
            g = Guid.NewGuid();
            sess = (clsSession)Session["UserSessionClient"];
            string newpath = path + "\\";
            string ids = g.ToString();
            ids = ids.Replace("-", "");
            string newFileName = "PRTemporyTemplate" + ids.ToString();
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
            }
            return newpath + newFileName + f1.Extension;
        }
        catch (Exception Ex)
        {
            return "";
        }
    }

    private void CreateQuery1(string StateName, string Path, out string[] plcT, out string[] TextT, bool check, clsSession sess)
    {
        TextT = new string[0];
        plcT = new string[0];
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
                int textCount = 0, chkCount = 0;
                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        chkCount++;
                    }
                    else
                    {
                        textCount++;
                    }
                }
                TextT = new string[textCount];
                plcT = new string[textCount];
                columns = getColumns(textCount, sess);
                int j = 0, k = 0, l = 0;
                if (check == true)
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        TextT[k] = columns[l];
                        plcT[k] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                        k++;
                        l++;
                    }
                }
                columns = null;
            }
        }
    }

    public string getHtmlStringProgress(clsSession sess, int curIEPID)
    {
        try
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            int SchoolID = sess.SchoolId;
            int StudentID = sess.StudentId;
            string HTML3 = "";
            int count = 0;

            int CurrentIepId = curIEPID;

            string studName = "";
            string studNbr = "";
            string studDOB = "";
            string DistrictName = "";
            string DistrictAddress = "";
            string DistrictContact = "";
            DataTable dtStudDtl = objData.ReturnDataTable("select LastName,FirstName,StudentPersonalId,FORMAT(BirthDate,'MM/dd/yyyy') as BirthDate from StudentPersonal where studentpersonalid='" + sess.StudentId + "'", false);
            DataTable dtSchDtl = objData.ReturnDataTable("select DistrictName,AddressLine1+', '+AddressLine2+', '+City+', '+(SELECT LookupName FROM LookUp WHERE LookupId=State)+', '+Zip AS DistrictAddress,DistContact+'/'+DistPhone AS DistrictContact from School sch inner join Address addr ON sch.AddressId=addr.AddressId where SchoolId='" + sess.SchoolId + "'", false);
            if (dtStudDtl != null)
            {
                studName = dtStudDtl.Rows[0]["LastName"].ToString() + ", " + dtStudDtl.Rows[0]["FirstName"].ToString();
                studNbr = dtStudDtl.Rows[0]["StudentPersonalId"].ToString();
                studDOB = dtStudDtl.Rows[0]["BirthDate"].ToString();
            }
            if (dtSchDtl != null)
            {
                DistrictName = dtSchDtl.Rows[0]["DistrictName"].ToString();
                DistrictAddress = dtSchDtl.Rows[0]["DistrictAddress"].ToString();
                DistrictContact = dtSchDtl.Rows[0]["DistrictContact"].ToString();
            }
            if (CurrentIepId != 0)
            {
                DataTable DTSchoolDetails = objData.ReturnDataTable("SELECT DistrictName,AddressLine1+','+AddressLine2+','+City+','+(SELECT LookupName FROM LookUp WHERE LookupId=State)+','+Zip AS DistrictAddress,DistContact+'/'+DistPhone AS DistrictContact FROM School SCL INNER JOIN Address ADR ON SCL.AddressId=ADR.AddressId WHERE SchoolId='" + sess.SchoolId + "'", false);
                string GoalIds = Convert.ToString(objData.FetchValue("SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),GoalLPRelId) FROM (SELECT SLP.GoalId,GLPR.GoalLPRelId FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND SLP.LessonPlanId=GLPR.LessonPlanId WHERE SLP.StdtIEPId='" + CurrentIepId + "') LP FOR XML PATH('')),1,1,'')"));
                DTSchoolDetails = objData.ReturnDataTable(" SELECT CONVERT(VARCHAR,EffStartDate,101) AS EffStartDate,CONVERT(VARCHAR,EffEndDate,101) AS EffEndDate,StudentLname+','+StudentFname AS StudentName,StudentNbr,CONVERT(VARCHAR,DOB,101) AS DOB FROM StdtIEP IEP INNER JOIN Student STU ON IEP.StudentId=STU.StudentId WHERE StdtIEPId='" + CurrentIepId + "'", false);
                //DataTable DTGoal = objData.ReturnDataTable("SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId,LP.LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3 FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND SLP.LessonPlanId=GLPR.LessonPlanId INNER JOIN LessonPlan LP ON LP.LessonPlanId=SLP.LessonPlanId WHERE SLP.StdtIEPId='" + CurrentIepId + "' AND SLP.ActiveInd='A' AND SLP.IncludeIEP=1", false);
                DataTable DTGoal = objData.ReturnDataTable("SELECT * FROM (SELECT GLPR.GoalLPRelId,(SELECT GoalName FROM Goal WHERE GoalId=SLP.GoalId) AS GoalName,SLP.GoalId,SLP.LessonPlanId, " +
                              "LP.LessonPlanName,SLP.Objective1,SLP.Objective2,SLP.Objective3,(SELECT TOP 1 IEPGoalNo FROM StdtGoal WHERE StdtIEPId=SLP.StdtIEPId AND GoalId=SLP.GoalId AND StudentId=SLP.StudentId " +
                              "ORDER BY StdtGoalId DESC ) IEPGoalNo,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE StudentId=SLP.StudentId AND DSTempHdr.LessonPlanId=SLP.LessonPlanId) LessonOrder FROM StdtLessonPlan SLP INNER JOIN GoalLPRel GLPR ON SLP.GoalId=GLPR.GoalId AND " +
                              "SLP.LessonPlanId=GLPR.LessonPlanId INNER JOIN LessonPlan LP ON LP.LessonPlanId=SLP.LessonPlanId WHERE SLP.StdtIEPId=" + CurrentIepId + " AND SLP.ActiveInd='A' AND SLP.IncludeIEP=1) LESSON ORDER BY IEPGoalNo,LessonOrder", false);
            

                HTML3 += "<div style='width:100%'>";

                if (DTGoal != null)
                {
                    if (DTGoal.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in DTGoal.Rows)
                        {
                            int tmp = count;

                            HTML3 += "<table style='width:100%; border:1px solid black; border-collapse:collapse;'>" +
                            "<tr><td colspan='2' style='border:1px solid black; background-color:black; color:white; text-align:center'><font face='Arial' size='11px'><b>INFORMATION FROM CURRENT IEP</b></font></td></tr>" +
                            "<tr><td style='border:1px solid black;'><font face='Arial' size='10px'><b>Goal #: " + dr1["IEPGoalNo"] + "</b></font></td><td style='border:1px solid black;'><font face='Arial' size='10px'><b>Specific Goal Focus:</b></font><font face='Helvetica' size='8px'> " + dr1["LessonPlanName"] + "</font></td></tr>" +
                            "</table>";

                            HTML3 += "<font face='Arial' size='8px'><b>Current Performance Level:</b> <i>What can the student currently do?</i></font><hr>" + dr1["Objective1"] + "<br><hr>" +
                                "<font face='Arial' size='8px'><b>Measurable Annual Goal:</b> What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?<br />How will we know that the student has reached this goal?</font><hr>" + dr1["Objective2"] + "<br><hr>" +
                                "<font face='Arial' size='8px'><b>Benchmark/Objectives:</b> What will the student need to do to complete this goal?</font><hr>" + dr1["Objective3"] + "";

                            int index = 0;
                            int qu = 1;
                            DataTable DtReport = objData.ReturnDataTable("SELECT CONVERT(VARCHAR,Report_Date,101) AS Report_Date,Report_Info,Report_Id FROM Progress_Report WHERE GoalId='" + dr1["GoalLPRelId"] + "' AND StdtIEPId='" + CurrentIepId + "'", false);
                            if (DtReport != null)
                            {
                                if (DtReport.Rows.Count > 0)
                                {
                                    HTML3 += "<table style='width:100%; border:1px solid black; border-collapse:collapse;'><tr>" +
                                    "<td style='border:1px solid black; background-color:black; color:white; text-align:center'><font face='Arial' size='12px'><b>PROGRESS REPORT INFORMATION</b></font></td>" +
                                    "</tr></table>";

                                    foreach (DataRow rpt in DtReport.Rows)
                                    {
                                        HTML3 += "<table style='width:100%;'>" +
                                        "<tr>" +
                                        "<td><font face='Arial' size='9px'>Progress Report Date: <b>" + rpt["Report_Date"] + "</b></font></td>" +
                                        "<td></td>" +
                                        "<td><font face='Arial' size='9px'>Progress Report #<b>" + qu + "</b> of <b>4</b></font></td>" +
                                        "</tr></table><br><hr>";

                                        if (qu == 1)
                                        {
                                            HTML3 += "<table style='width:100%;'><tr><td>";
                                            HTML3 += "<font face='Arial' size='3px'>Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.</font>";
                                            HTML3 += "</td></tr></table><br><hr>";
                                        }

                                        HTML3 += "<table style='width:100%;'><tr><td colspan='3'>" + rpt["Report_Info"] + "</td></tr></table><br><hr>";
                                        index++; qu++;
                                    }
                                }
                            }
                        }
                    }
                }

                HTML3 += "</div>";
            }
            return HTML3;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string[] getColumns(int Count, clsSession sess)
    {
        int cIepId = Convert.ToInt32(ViewState["curIEPID"]);
        string[] colum = new string[13];
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int SchoolID = sess.SchoolId;
        int StudentID = sess.StudentId;

        string DistrictName = "";
        string DistrictAddress = "";
        string DistrictContact = "";
        string EffStartDate = "";
        string EffEndDate = "";
        string StudentName = "";
        string StudentNbr = "";
        string DOB = "";

        DataTable dtSchDtl = objData.ReturnDataTable("select DistrictName,AddressLine1+', '+AddressLine2+', '+City+', '+(SELECT LookupName FROM LookUp WHERE LookupId=State)+', '+Zip AS DistrictAddress,DistContact+'/'+DistPhone AS DistrictContact from School sch inner join Address addr ON sch.AddressId=addr.AddressId where SchoolId='" + sess.SchoolId + "'", false);
        if (dtSchDtl != null)
        {
            DistrictName = dtSchDtl.Rows[0]["DistrictName"].ToString();
            DistrictAddress = dtSchDtl.Rows[0]["DistrictAddress"].ToString();
            DistrictContact = dtSchDtl.Rows[0]["DistrictContact"].ToString();
        }

        DataTable DTHdrDtl = objData.ReturnDataTable(" SELECT CONVERT(VARCHAR,EffStartDate,101) AS EffStartDate,CONVERT(VARCHAR,EffEndDate,101) AS EffEndDate,StudentLname+','+StudentFname AS StudentName,StudentNbr,CONVERT(VARCHAR,DOB,101) AS DOB FROM StdtIEP IEP INNER JOIN Student STU ON IEP.StudentId=STU.StudentId WHERE StdtIEPId='" + cIepId + "'", false);
        if (DTHdrDtl != null)
        {
            EffStartDate = DTHdrDtl.Rows[0]["EffStartDate"].ToString();
            EffEndDate = DTHdrDtl.Rows[0]["EffEndDate"].ToString();
            StudentName = DTHdrDtl.Rows[0]["StudentName"].ToString();
            StudentNbr = DTHdrDtl.Rows[0]["StudentNbr"].ToString();
            DOB = DTHdrDtl.Rows[0]["DOB"].ToString();
        }

        colum[0] = DistrictName;
        colum[1] = DistrictAddress;
        colum[2] = DistrictContact;
        colum[3] = EffStartDate;
        colum[4] = EffEndDate;
        colum[5] = StudentName;
        colum[6] = StudentNbr;
        colum[7] = DOB;
        colum[8] = StudentName;
        colum[9] = StudentNbr;
        colum[10] = DOB;
        colum[11] = EffStartDate;
        colum[12] = EffEndDate;
        return colum;
    }

    public void ExportAll(int curIEPID)
    {
        try
        {
            ViewState["curIEPID"] = curIEPID;
            clsData objData = new clsData();
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            string[] plcT, TextT;
            string Path = Server.MapPath("~\\Administration\\TempPR\\IEP1.docx");
            string NewPath = CopyTemplate(Path, "0");

            Guid g = Guid.NewGuid();
            string ids = g.ToString();
            string hPath = Server.MapPath("~\\Administration") + "\\TempPR1\\HTML" + ids + ".html";
            CleanDoc(NewPath);

            string sName = "";
            DataTable DTHdrDtls = objData.ReturnDataTable(" SELECT CONVERT(VARCHAR,EffStartDate,101) AS EffStartDate,CONVERT(VARCHAR,EffEndDate,101) AS EffEndDate,StudentLname+','+StudentFname AS StudentName,StudentNbr,CONVERT(VARCHAR,DOB,101) AS DOB FROM StdtIEP IEP INNER JOIN Student STU ON IEP.StudentId=STU.StudentId WHERE StdtIEPId='" + curIEPID + "'", false);
            if (DTHdrDtls != null)
            {
                if (DTHdrDtls.Rows.Count > 0)
                {
                    sName = DTHdrDtls.Rows[0]["StudentName"].ToString();
                    string sDate = DTHdrDtls.Rows[0]["EffStartDate"].ToString();
                    string eDate = DTHdrDtls.Rows[0]["EffEndDate"].ToString();
                    string sID = DTHdrDtls.Rows[0]["StudentNbr"].ToString();
                    string sDOB = DTHdrDtls.Rows[0]["DOB"].ToString();

                    using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
                    {
                        MainDocumentPart mainPart = theDoc.MainDocumentPart;
                        string content = null;

                        using (StreamReader reader = new StreamReader(theDoc.MainDocumentPart.HeaderParts.First().GetStream()))
                        {
                            content = reader.ReadToEnd();
                        }
                        Regex exSName = new Regex("plcStdName");
                        Regex exSDate = new Regex("plcIEPFrmDate");
                        Regex exEDate = new Regex("plcIEPToDate");
                        Regex exSID = new Regex("plcStdID");
                        Regex exSDOB = new Regex("plcStdDOB");

                        content = exSName.Replace(content, sName);
                        content = exSDate.Replace(content, sDate);
                        content = exEDate.Replace(content, eDate);
                        content = exSID.Replace(content, sID);
                        content = exSDOB.Replace(content, sDOB);

                        using (StreamWriter writer = new StreamWriter(theDoc.MainDocumentPart.HeaderParts.First().GetStream(FileMode.Create)))
                        {
                            writer.Write(content);
                        }
                        mainPart.Document.Save();
                    }
                }
            }

            sess = (clsSession)Session["UserSession"];
            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
            {
                CreateQuery1("NE", "~\\Administration\\XMLProgressReport\\ProgressReport.xml", out plcT, out TextT, true, sess);
                replaceWithTexts(theDoc.MainDocumentPart, plcT, TextT);
            }

            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
            {
                string htmlProgress = getHtmlStringProgress(sess, curIEPID);
                replaceWithTextsSingle(theDoc.MainDocumentPart, "plcPRI", htmlProgress);
            }

            sess = (clsSession)Session["UserSession"];

            string fileName = "ProgressReport" + sName + ".doc";
            string enCodeFileName = Server.UrlEncode(fileName); 
            byte[] contents = System.IO.File.ReadAllBytes(NewPath);
            string contentType = "application/msword";
            objBinary.ShowDocument(enCodeFileName, contents, contentType);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void replaceWithTexts(MainDocumentPart mainPart, string[] plcT, string[] TextT)
    {
        int count = plcT.Count();
        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
        for (int i = 0; i < count; i++)
        {
            string textData = "";
            if (TextT[i] != null)
            {
                textData = TextT[i];
            }
            else
            {
                textData = "";
            }

            var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);

            string textDataNoSpace = textData.Replace(" ", "");

            foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
            {
                var paragraphs = converter.Parse(textData);
                if (paragraphs.Count == 0)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                    para.Parent.Append(tempPara);
                    para.Remove();
                }
                else
                {
                    for (int k = 0; k < paragraphs.Count; k++)
                    {
                        bool isBullet = false;
                        if (textDataNoSpace.Contains("<li>" + paragraphs[k].InnerText.Trim()))
                            isBullet = true;
                        if (isBullet)
                        {
                            ParagraphProperties paraProp = new ParagraphProperties();
                            ParagraphStyleId paraStyleid = new ParagraphStyleId() { Val = "BulletPara" };
                            NumberingProperties numProp = new NumberingProperties();
                            NumberingLevelReference numLvlRef = new NumberingLevelReference() { Val = 0 };
                            NumberingId numID = new NumberingId() { Val = 1 };
                            numProp.Append(numLvlRef);
                            numProp.Append(numID);
                            paraProp.Append(paraStyleid);
                            paraProp.Append(numProp);

                            if (((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties != null)
                            {
                                //Assign Bullet point property to paragraph
                                ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties.Append(paraProp);
                            }
                        }
                        para.Parent.Append(paragraphs[k]);

                    }
                    para.Remove();
                }
            }
        }
    }

    public void replaceWithTextsSingle(MainDocumentPart mainPart, string plcT, string TextT)
    {
        try
        {
            NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);

            string textData = "";

            if (TextT != null)
            {
                textData = TextT;
            }
            else
            {
                textData = "";
            }

            var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);
            string textDataNoSpace = textData.Replace(" ", "");

            foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
            {
                var paragraphs = converter.Parse(textData);

                if (paragraphs.Count == 0)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                    para.Parent.Append(tempPara);
                }
                else
                {
                    for (int k = 0; k < paragraphs.Count; k++)
                    {
                        bool isBullet = false;

                        if (textDataNoSpace.Contains("<li>" + paragraphs[k].InnerText.Trim()))
                            isBullet = true;

                        if (isBullet)
                        {
                            ParagraphProperties paraProp = new ParagraphProperties();
                            ParagraphStyleId paraStyleid = new ParagraphStyleId() { Val = "BulletPara" };
                            NumberingProperties numProp = new NumberingProperties();
                            NumberingLevelReference numLvlRef = new NumberingLevelReference() { Val = 0 };
                            NumberingId numID = new NumberingId() { Val = 1 };
                            numProp.Append(numLvlRef);
                            numProp.Append(numID);
                            paraProp.Append(paraStyleid);
                            paraProp.Append(numProp);

                            if (((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties != null)
                            {
                                //Assign Bullet point property to paragraph
                                ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties.Append(paraProp);
                            }
                        }

                        para.Parent.Append(paragraphs[k]);
                    }
                }
                //para.RemoveAllChildren<Run>();
            }

            paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);

            foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
            {
                para.RemoveAllChildren<Run>();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}