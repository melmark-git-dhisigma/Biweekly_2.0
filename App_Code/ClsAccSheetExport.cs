using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for ClsAccSheetExport
/// </summary>
public class ClsAccSheetExport
{
    clsData objData = null;
    DataTable Dt = null;
    string strQuery = "";
    string[] IEPC = null;
    string[] IEPP = null;
    clsSession sess = null;

    string[] Common = null;
    string[] aC = null;
    string[] aP = null;
    int Count = 0, objcnt = 0, RCount = 0;

	public ClsAccSheetExport()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataTable getAccSheet(int StudentId, int SchoolId,string dateToLoad) //Query to Export the Academic Coversheet
    {
        string strtDate = "";
        string endDate = "";
        try
        {

            if (dateToLoad != null && dateToLoad != "")
            {
                strtDate = dateToLoad.Split('-')[0];
                endDate = dateToLoad.Split('-')[1];
            }
            string StdtIEPTable = "StdtIEP";
            if (SchoolId == 2)
            {
                StdtIEPTable = "StdtIEP_PE";
            }
            objData = new clsData();
            Dt = new DataTable();
            strQuery = "select Student.StudentFname+' '+Student.StudentLname as 'Name'," +
            "(Select TOP 1 'From:'+CONVERT(VARCHAR, SDIEP.EffStartDate,101)+' To :'+CONVERT(VARCHAR,SDIEP.EffEndDate,101)  from " + StdtIEPTable + " SDIEP where StudentId=" + StudentId + "  AND StatusId In (select LookupId from LookUp where LookupName!='Not Started'  And  LookupName!='Pending Approval'   And  LookupName!='Rejected'  AND LookupType='IEP Status')) AS  'IepDates'," +
            "StdtAcdSheet.DateOfMeeting," +
            "StdtAcdSheet.GoalArea,StdtAcdSheet.Goal,StdtAcdSheet.Benchmarks,StdtAcdSheet.MetObjective, StdtAcdSheet.MetGoal,StdtAcdSheet.NotMaintaining, '' as  Curr,StdtAcdSheet.Period1,StdtAcdSheet.Period2,StdtAcdSheet.Period3,StdtAcdSheet.Period4," +
            "StdtAcdSheet.Period5,StdtAcdSheet.Period6,StdtAcdSheet.Period7,StdtAcdSheet.Progressing1,StdtAcdSheet.Progressing2,StdtAcdSheet.Progressing3,StdtAcdSheet.Progressing4,StdtAcdSheet.Progressing5,StdtAcdSheet.Progressing6,StdtAcdSheet.Progressing7,StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.TypeOfInstruction," +
            "StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.Set1," +
            "StdtAcdSheet.Set2,StdtAcdSheet.Set3,StdtAcdSheet.Set4,StdtAcdSheet.Set5,StdtAcdSheet.Set6,StdtAcdSheet.Set7,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step1)AS Step1,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step2)AS Step2,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step3)AS Step3,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step4)AS Step4,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step5)AS Step5,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step6)AS Step6,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step7)AS Step7,StdtAcdSheet.Prompt1,StdtAcdSheet.Prompt2," +
            "StdtAcdSheet.Prompt3,StdtAcdSheet.Prompt4,StdtAcdSheet.Prompt5,StdtAcdSheet.Prompt6,StdtAcdSheet.Prompt7,StdtAcdSheet.IOA1,StdtAcdSheet.IOA2," +
            "StdtAcdSheet.IOA3,StdtAcdSheet.IOA4,StdtAcdSheet.IOA5,StdtAcdSheet.IOA6,StdtAcdSheet.IOA7,StdtAcdSheet.FeedBack,StdtAcdSheet.PreposalDiss," +
            "StdtAcdSheet.PersonResNdDeadline,StdtAcdSheet.NoOfTimes1,StdtAcdSheet.NoOfTimes2,StdtAcdSheet.NoOfTimes3,StdtAcdSheet.NoOfTimes4,StdtAcdSheet.NoOfTimes5,StdtAcdSheet.NoOfTimes6,StdtAcdSheet.NoOfTimes7,StdtAcdSheet.LessonPlanId,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtAcdSheet.LessonPlanId AND DSTempHdr.StudentId="+StudentId+") LessonOrder " +
            "from StdtAcdSheet inner join Student on StdtAcdSheet.StudentId=Student.StudentId where StdtAcdSheet.StudentId=" + StudentId + "" +
            "and CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + strtDate + "') and CONVERT(datetime,EndDate)=CONVERT(datetime,'" + endDate + "') ORDER BY LessonOrder";
            Dt = objData.ReturnDataTable(strQuery, false);
            Dt.Columns.Add("Major", typeof(string));
            Dt.Columns.Add("Minor", typeof(string));
            Dt.Columns.Add("Arrow", typeof(string));

            Dt.Columns.Add("FeedBackP", typeof(string));
           
            Dt.Columns.Add("FeedBackC", typeof(string));

            foreach (DataRow dtR in Dt.Rows)
            {
                string cprog = "";
                if (Convert.ToInt32(dtR["MetObjective"]) == 1)
                {
                    cprog = cprog + "Met Objective,";
                }
                if (Convert.ToInt32(dtR["MetGoal"]) == 1)
                {
                    cprog = cprog + "Met Goal,";
                }
                if (Convert.ToInt32(dtR["NotMaintaining"]) == 1)
                {
                    cprog = cprog + "Not Maintaining,";
                }
                cprog = cprog.Trim(',');
                dtR["Curr"] = cprog;
            }
            if (Dt.Columns.Contains("MetObjective"))
            {
                Dt.Columns.Remove("MetObjective");
            }
            if (Dt.Columns.Contains("MetGoal"))
            {
                Dt.Columns.Remove("MetGoal");
            }
            if (Dt.Columns.Contains("NotMaintaining"))
            {
                Dt.Columns.Remove("NotMaintaining");
            }

            foreach (DataRow dtRow in Dt.Rows)
            {
                
                int Lessonplanid = Convert.ToInt32(dtRow["LessonPlanId"]);

                string Phaseline = "SELECT (SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + strtDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                " (StdtSessEventType='Major') AND StudentId=" + StudentId + " and LessonPlanId in(" + Lessonplanid + ",0)  FOR XML PATH('')),1,1,'')) Eventdata";
                string dtNew1 = Convert.ToString(objData.FetchValue(Phaseline));
                dtRow[68] = dtNew1;
                if (dtNew1 == "")
                {
                    dtRow[68] = "No Results";
                }
                string Conditionline = "SELECT (SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + strtDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                    " (StdtSessEventType='Minor') AND StudentId=" + StudentId + "and LessonPlanId in(" + Lessonplanid + ",0)  FOR XML PATH('')),1,1,'')) Eventdata";
                string dtNew2 = Convert.ToString(objData.FetchValue(Conditionline));
                dtRow[69] = dtNew2;
                if (dtNew2 == "")
                {
                    dtRow[69] = "No Results";
                }
                string Arrownotes = "SELECT (SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + strtDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                    " (StdtSessEventType='Arrow notes') AND StudentId=" + StudentId + " and LessonPlanId in(" + Lessonplanid + ",0) FOR XML PATH('')),1,1,'')) Eventdata";
                string dtNew3 = Convert.ToString(objData.FetchValue(Arrownotes));
                dtRow[70] = dtNew3;
                if (dtNew3 == "")
                {
                    dtRow[70] = "No Results";
                }
               
            }

            foreach (DataRow dtRow in Dt.Rows)
            {
                int Lessonplanid = Convert.ToInt32(dtRow["LessonPlanId"]);
                string followup = "SELECT (SELECT STUFF((SELECT CHAR(10)+  'Follow Up: ' + CASE WHEN PropAndDisc != ''  THEN PropAndDisc ELSE 'Nil' END    + '        Person Responsible: ' + CASE  WHEN (SELECT UserFName + ' ' + UserLName A FROM [User] WHERE UserId = PersonResp) IS NOT NULL THEN (SELECT UserFName +' '+ UserLName A FROM [User] WHERE UserId = PersonResp) ELSE 'Nil' END +'       Deadlines:'+  case when convert(varchar,Deadlines)  is null then 'Nil' else convert(varchar,convert(date,Deadlines)) end   from AcdSheetMtng where NOT (Deadlines IS NULL AND PersonResp=0 AND PropAndDisc='') and lessonplanid=" + Lessonplanid + " and followstatus = 'P' and ActiveInd='A'  and AccSheetId in (select AccSheetId from StdtAcdSheet where  EndDate = '" + endDate + "' and LessonPlanId='" + Lessonplanid + "') FOR XML PATH('')),1,1,''))";
                string dtNewP = Convert.ToString(objData.FetchValue(followup));
                string resultP = dtNewP.Replace("Follow Up:", "<w:br/>Follow Up:");
                dtRow[71] = resultP;

                string followuc = "SELECT (SELECT STUFF((SELECT CHAR(10)+  'Follow Up: ' + CASE WHEN PropAndDisc != ''  THEN PropAndDisc ELSE 'Nil' END    + '        Person Responsible: ' + CASE  WHEN (SELECT UserFName + ' ' + UserLName A FROM [User] WHERE UserId = PersonResp) IS NOT NULL THEN (SELECT UserFName +' '+ UserLName A FROM [User] WHERE UserId = PersonResp) ELSE 'Nil' END +'       Deadlines:'+  case when convert(varchar,Deadlines)  is null then 'Nil' else convert(varchar,convert(date,Deadlines)) end   from AcdSheetMtng where NOT (Deadlines IS NULL AND PersonResp=0 AND PropAndDisc='') and lessonplanid=" + Lessonplanid + " and followstatus = 'C' and ActiveInd='A'  and AccSheetId in (select AccSheetId from StdtAcdSheet where  EndDate = '" + endDate + "' and LessonPlanId='" + Lessonplanid + "') FOR XML PATH('')),1,1,''))";
                string dtNewC = Convert.ToString(objData.FetchValue(followuc));
                string resultC = dtNewC.Replace("Follow Up:", "<w:br/>Follow Up:");
                dtRow[72] = resultC;
                
            }

            //foreach (DataRow dr in Dt.Rows)      /// To Convert DB string to Html
            //{
            //    if (dr["Benchmarks"] != null && dr["Benchmarks"] != "")
            //    {
            //        dr["Benchmarks"] = StripHTML(dr["Benchmarks"].ToString()).Replace("&nbsp;"," ");  //Remove all html tags
            //    }
            //}
           
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return Dt;
    }
    public static string StripHTML(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
  

}