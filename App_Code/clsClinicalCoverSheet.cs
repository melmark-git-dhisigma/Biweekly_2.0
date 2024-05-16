using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;



/// <summary>
/// Summary description for clsClinicalCoverSheet
/// </summary>
public class clsClinicalCoverSheet
{

    clsData objData = null;
    DataTable Dt = null;
    string strQuery = "";
    string[] IEPC = null;
    string[] IEPP = null;

    string[] Common = null;
    string[] aC = null;
    string[] aP = null;
    int Count = 0, objcnt = 0, RCount = 0;

    public clsClinicalCoverSheet()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /*   DateTime todayDate = DateTime.ParseExact(drpSelectDate.SelectedItem.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime lastDayDate=todayDate.AddDays(-90);

        dt = objData.ReturnDataTable("select distinct BehaviourDetails.Behaviour from StdtAggScores inner join BehaviourDetails on StdtAggScores.MeasurementId=BehaviourDetails.MeasurementId where CAST( AggredatedDate as date) between   cast( '" + lastDayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and CAST( '" + todayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and StdtAggScores.MeasurementId is not null and BehaviourDetails.ActiveInd='A'", true);*/


    public DataTable getHeaderValz(int StudentId, int SchoolId, int Clvid)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            // strQuery = "  Select CONVERT(VARCHAR(10),IE.EffStartDate, 103) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 103) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 103) AS DOB,S.StudentNbr,GL.GoalName,GL.GoalDesc,SG.Objective1+','+SG.Objective2+','+SG.Objective3 as BenchMarks,IE.Version FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " ";

            //strQuery = "select Student.StudentFname+' '+Student.StudentLname as StdName,(SELECT ClassName FROM Class WHERE ClassId=Location) Location,(SELECT Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId=Program) Program,IepYear,PeriodOfAssmt from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + ""; // should change the values to sess
            //Old single location and program query//strQuery = "select Student.StudentFname+' '+Student.StudentLname as StdName,(SELECT ClassName FROM Class WHERE ClassId=Location) Location,(SELECT Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId=Program) Program,CONCAT((SELECT TOP 1 convert(varchar,FORMAT (BehaviorIEPSDate,'MM/dd/yyyy'), 1) FROM BehaviourDetails WHERE StudentId = Student.StudentPersonalId AND BehaviorIEPSDate IS NOT NULL),' - '+(SELECT TOP 1 convert(varchar,FORMAT (BehaviorIEPEDate,'MM/dd/yyyy'), 1) FROM BehaviourDetails WHERE StudentId = Student.StudentPersonalId AND BehaviorIEPEDate IS NOT NULL)) AS IepYear,CONCAT((SELECT convert(varchar,StartDate, 1)),' - '+(SELECT convert(varchar,EndDate, 1))) AS PeriodOfAssmt from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";
            strQuery = "select Student.FirstName+' '+Student.LastName as StdName,CONCAT((SELECT TOP 1 convert(varchar,FORMAT (BehaviorIEPSDate,'MM/dd/yyyy'), 1) FROM BehaviourDetails WHERE StudentId = Student.StudentPersonalId AND BehaviorIEPSDate IS NOT NULL),' - '+(SELECT TOP 1 convert(varchar,FORMAT (BehaviorIEPEDate,'MM/dd/yyyy'), 1) FROM BehaviourDetails WHERE StudentId = Student.StudentPersonalId AND BehaviorIEPEDate IS NOT NULL)) AS IepYear,CONCAT((SELECT convert(varchar,StartDate, 1)),' - '+(SELECT convert(varchar,EndDate, 1))) AS PeriodOfAssmt from StdtClinicalCoverSheet inner join StudentPersonal Student  on Student.StudentPersonalId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";

           // strQuery = "select TOP 1 Student.StudentFname+' '+Student.StudentLname as StdName,stdiep.EffStartDate as IepYear,Location,Program,PeriodOfAssmt from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId left join StdtIEP stdiep on Student.StudentId = stdiep.StudentId   where  StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";

            Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }

    public DataTable getClinicalBehavior(int StudentId, int SchoolId, string dateVal)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            DateTime todayDate = DateTime.ParseExact(dateVal, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime lastDayDate = todayDate.AddDays(-90);

            Dt = objData.ReturnDataTable("select distinct BehaviourDetails.Behaviour as Behaviour from StdtAggScores inner join BehaviourDetails on StdtAggScores.MeasurementId=BehaviourDetails.MeasurementId where CAST( AggredatedDate as date) between   cast( '" + lastDayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and CAST( '" + todayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and StdtAggScores.MeasurementId is not null and BehaviourDetails.ActiveInd='A' AND BehaviourDetails.StudentId = " + StudentId, true);

            // Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return Dt;
    }


    public DataTable getClinicalCvSheet(int StudentId, int SchoolId, int Clvid)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            // strQuery = "  Select CONVERT(VARCHAR(10),IE.EffStartDate, 103) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 103) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 103) AS DOB,S.StudentNbr,GL.GoalName,GL.GoalDesc,SG.Objective1+','+SG.Objective2+','+SG.Objective3 as BenchMarks,IE.Version FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " ";

            //strQuery = "select ClinicalCvId,SchoolId,ClassId,StartDate,EndDate,FollowUp,Proposals,PgmCord,CONVERT(varchar(10), PgmCordDate,101) as PgmCordDate ,EduCord,CONVERT(varchar(10), EduCordDate,101) as EduCordDate,BCBA, CONVERT(varchar(10), BCBADate,101) as BCBADate from StdtClinicalCoverSheet where StudentId=" + StudentId + " and SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + ""; // should change the values to sess
            strQuery = "select ClinicalCvId,SchoolId,ClassId,StartDate,EndDate,FollowUp,Proposals,PgmCord,Case when PgmCordDate='1900-01-01' Then ' ' else  CONVERT(varchar(10), PgmCordDate,101)  END as PgmCordDate,EduCord,Case when EduCordDate='1900-01-01' Then ' ' else  CONVERT(varchar(10), EduCordDate,101)  END as EduCordDate,BCBA, Case when BCBADate='1900-01-01' Then ' ' else  CONVERT(varchar(10), BCBADate,101)  END as BCBADate from StdtClinicalCoverSheet where StudentId=" + StudentId + " and SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";//10-May-2021 List 5 task #18 // should change the values to sess

            Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }


    public DataTable SettingEventsandProgramChanges(int StudentId, int SchoolId, string date)
    {

        try
        {
            string stDate = "";
            string endDate = "";
            if (date != null)
            {
                stDate = date.Split('-')[0];
                endDate = date.Split('-')[1];
            }

            DateTime dtst1 = new DateTime();
            DateTime dted1 = new DateTime();
            string dtst = "";
            string dted = "";
            if (date != null)
            {
                string[] dateParts = date.Split('-');
                if (dateParts.Length >= 2)
                {
                DateTime.TryParse(dateParts[0], out dtst1);
                DateTime.TryParse(dateParts[1], out dted1);
                dtst = dtst1.ToString("MM/dd/yyyy");
                dted = dted1.ToString("MM/dd/yyyy");
                }
            }
            
            objData = new clsData();
            Dt = new DataTable();
            string strQuery2 = "";
            //strQuery = "SELECT TOP 1 ISNULL((SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,103)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE, DATEADD(DAY,-90,'" + date + "')) AND CONVERT(DATE,'" + date + "') AND " +
            //            " (StdtSessEventType='Major') AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')),'-') Phaseline, ISNULL((SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,103)+')' FROM StdtSessEvent WHERE " +
            //            " CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE, DATEADD(DAY,-90,'" + date + "')) AND CONVERT(DATE,'" + date + "') AND (StdtSessEventType='Minor') AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')),'-') Conditionline, " +
            //            " ISNULL((SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,103)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE, DATEADD(DAY,-90,'" + date + "')) AND CONVERT(DATE,'" + date + "') AND " +
            //            " (StdtSessEventType='Arrow notes') AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')),'-') Arrownote FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE, DATEADD(DAY,-90,'" + date + "')) AND CONVERT(DATE,'" + date + "') AND " +
            //            " (StdtSessEventType='Major' OR StdtSessEventType='Minor' OR StdtSessEventType='Arrow notes')  AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + "";

            //Dt = objData.ReturnDataTable(strQuery, false);
            if (SchoolId == 1)
            {
                //strQuery2 = "SELECT TOP 1" +
                //" ISNULL((SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + stDate + "') AND CONVERT(DATE,'" + endDate + "') AND  (StdtSessEventType='Major') AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')),'-') Phaseline," +
                //" ISNULL((SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE  CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + stDate + "') AND CONVERT(DATE,'" + endDate + "') AND (StdtSessEventType='Minor') AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')),'-') Conditionline," +
                //" ISNULL((SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + stDate + "') AND CONVERT(DATE,'" + endDate + "') AND  (StdtSessEventType='Arrow notes') AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')),'-') Arrownote" +
                //" FROM StdtSessEvent INNER JOIN StdtClinicalCoverSheet ON StdtSessEvent.StudentId=StdtClinicalCoverSheet.StudentId AND StdtSessEvent.SchoolId=StdtClinicalCoverSheet.SchoolId" +
                //" WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + stDate + "') AND CONVERT(DATE,'" + endDate + "') AND (StdtSessEventType='Major' OR StdtSessEventType='Minor' OR StdtSessEventType='Arrow notes') AND StdtSessEvent.StudentId=" + StudentId + " AND StdtSessEvent.SchoolId=" + SchoolId + " AND CAST( StdtClinicalCoverSheet.StartDate as date)= cast('" + stDate + "' as date) AND CAST( StdtClinicalCoverSheet.EndDate as date)= cast('" + endDate + "' as date)";

               // System.Data.DataTable dtNew = new System.Data.DataTable();

     

                String Mesrmntid = "select MeasurementId from BehaviourDetails b where b.StudentId= " + StudentId + " and activeind = 'A' ";
                System.Data.DataTable dtNew4 = objData.ReturnDataTable(Mesrmntid, false);
                System.Data.DataRow dr4 = Dt.NewRow();
                System.Data.DataTable dtNew5;
                System.Data.DataTable dtNew6;
                System.Data.DataTable dtNew1;
                System.Data.DataTable dtNew2;

                String Beh = "", Evnt = "", concatStrng = "" ,concatStrngMj="" ,concatStrngMn="";
                foreach (DataRow row in dtNew4.Rows)
                {
                    if (row != null)
                    {
                        String Behav = "SELECT Behaviour  from BehaviourDetails where MeasurementId =" + row["MeasurementId"];
                        dtNew5 = objData.ReturnDataTable(Behav, false);
                        Beh = Convert.ToString(dtNew5.Rows[0]["Behaviour"]);

                        string queryfilter = " SELECT STUFF((select '; ' + CONVERT(VARCHAR(50), outr.EvntTs,101)+','+ outr.eventname from( SELECT * FROM (SELECT * FROM  ((SELECT  SE.MeasurementId, SE.StdtSessEventId,  SE.EventName, " +
                      " SE.StdtSessEventType, CONVERT(CHAR(10), SE.EvntTs,101) AS EvntTs, " +
                      "  B.Behaviour FROM  [StdtSessEvent] SE LEFT JOIN LessonPlan L ON SE.LessonPlanId = L.LessonPlanId " +
                      "LEFT JOIN BehaviourDetails B ON B.MeasurementId=SE.MeasurementId WHERE EventType='EV' AND SE.StudentId=" + StudentId + " AND SE.StdtSessEventType<>'Medication') " +
                      //"UNION ALL (SELECT NULL AS MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
                      //"+ ( " +
                      //"        (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
                      //"        FROM   [user] US" +
                      //"       WHERE  US.userid = (SELECT" +
                      //"             createdby" +
                      //"                         FROM" +
                      //"           stdtsessionhdr Hdr" +
                      //"                       WHERE" +
                      //"         Hdr.stdtsessionhdrid = SH.ioasessionhdrid" +
                      //"        AND SH.ioaind = 'Y'))" +
                      //" + '/'" +
                      //" + (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
                      //"   FROM   [user] US" +
                      //"  WHERE  SH.ioauserid = US.userid) ) AS EventName," +
                      //" 'Arrow notes'                         AS" +
                      //" StdtSessEventType," +
                      //"CONVERT(CHAR(10), SH.endts, 101)      AS EvntTs," +
                      //"NULL                                  AS Behaviour" +
                      //" FROM   stdtsessionhdr SH" +
                      //"       LEFT JOIN lessonplan" +
                      //"             ON SH.lessonplanid = lessonplan.lessonplanid" +
                      //" WHERE  SH.ioaperc IS NOT NULL" +
                      //"      AND SH.ioaind = 'Y'" +
                      //"     AND SH.sessionstatuscd = 'S'" +
                      //"    AND SH.studentid =" + StudentId + ")" +
                      "UNION ALL (SELECT  BIOA.MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
                      "+ CASE WHEN BIOA.normalbehaviorid IS NULL THEN ((SELECT" +
                      "      TOP 1" +
                      "     Rtrim(" +
                      "    Ltrim(Upper(" +
                      "   US.userinitial))) FROM behaviour BH INNER JOIN [user] US" +
                      "  ON" +
                      " BH.createdby" +
                      " =" +
                      " US.userid WHERE BH.createdon BETWEEN" +
                        " Dateadd(minute, -5, BIOA.createdon)" +
                        " AND" +
                        " BIOA.createdon ORDER BY BH.createdon DESC)+'/'+ (SELECT" +
                        " TOP 1" +
                        " Rtrim(Ltrim(Upper(US.userinitial))) FROM" +
                        " behaviorioadetails BI" +
                        " INNER" +
                        " JOIN [user]" +
                        " US ON BI.createdby =" +
                        " US.userid WHERE BI.createdon=BIOA.createdon ORDER BY" +
                        " BI.createdon DESC)" +
                        " )" +
                        " ELSE ((" +
                        " SELECT" +
                        " Rtrim(Ltrim(Upper(US.userinitial))) FROM behaviour BH" +
                        " INNER" +
                        " JOIN [user]" +
                        " US ON" +
                        " BH.createdby = US.userid WHERE" +
                        " BIOA.normalbehaviorid=BH.behaviourid)+'/'+ (" +
                        " SELECT Rtrim(" +
                        " Ltrim(Upper(US.userinitial))) FROM behaviorioadetails BI" +
                        " INNER" +
                        " JOIN" +
                        " [user] US ON" +
                        " BI.createdby = US.userid INNER JOIN behaviour BH ON" +
                        " BH.behaviourid=BI.normalbehaviorid WHERE" +
                        " BIOA.normalbehaviorid=BH.behaviourid))" +
                        " END " +
                      " AS EventName, " +
                      "'Arrow notes' AS StdtSessEventType,  CONVERT(CHAR(10), BIOA.CreatedOn,101) AS EvntTs,  " +
                      " BHD.Behaviour FROM BehaviorIOADetails BIOA LEFT JOIN BehaviourDetails BHD ON BIOA.MeasurementId=BHD.MeasurementId " +
                      "WHERE BIOA.StudentId=" + StudentId + " AND IOAPerc IS NOT NULL AND BIOA.ActiveInd='A') )IOA ) " +
                      "  ad " +
                      " WHERE  ( ( ad.behaviour IS NULL " +
                      " AND ad.measurementid = 0 )" +
                      "OR ad.behaviour = (SELECT TOP 1 behaviour " +
                      "FROM   behaviourdetails " +
                      "WHERE  measurementid = " + row["MeasurementId"] + ") ) " +
                      " AND ad.stdtsesseventtype IN( 'Arrow notes' ) " +
                      " AND CONVERT(DATE, ad.evntts) >=  cast('" + stDate + "' as date) " +
                      " AND CONVERT(DATE, ad.evntts) <=  cast('" + endDate + "' as date) " +
                      " )outr " +
                       " FOR XML PATH('')),1,1,'') eventname ";
                        dtNew6 = objData.ReturnDataTable(queryfilter, false);
                        if (Convert.ToString(dtNew6.Rows[0]["eventname"]) != "" && Convert.ToString(dtNew6.Rows[0]["eventname"]) != null)
                        concatStrng += "&lt;b&gt;" + Beh + " : " + "&lt;/b&gt;" + Convert.ToString(dtNew6.Rows[0]["eventname"]) +System.Environment.NewLine;


                        string Phaseline = "SELECT 'Major Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst+ "') AND CONVERT(DATE,'" + dted + "') AND " +
                                                         " (StdtSessEventType='Major' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + row["MeasurementId"] + ")) AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
                        dtNew1 = objData.ReturnDataTable(Phaseline, false);
                        if (Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != null)
                            concatStrngMj += "<b>" + Beh + " : " + "</b>" + Convert.ToString(dtNew1.Rows[0]["Eventdata"]) + "<br></br>";

                        string Conditionline = "SELECT 'Minor Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst+ "') AND CONVERT(DATE,'" + dted + "') AND " +
                         " (StdtSessEventType='Minor' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + row["MeasurementId"] + ")) AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
                        dtNew2 = objData.ReturnDataTable(Conditionline, false);
                        if (Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != null)
                            concatStrngMn += "<b>" + Beh + " : " + "</b>" + Convert.ToString(dtNew2.Rows[0]["Eventdata"]) + "<br></br>";
            

                    }

                }
               
                Dt.Columns.Add("Phaseline", typeof(string));
                Dt.Columns.Add("Conditionline", typeof(string));
                Dt.Columns.Add("Arrownote", typeof(string));
                System.Data.DataRow dr5 = Dt.NewRow();
                if (concatStrngMj != "")
                {
                    dr5["Phaseline"] = concatStrngMj;
                }
                else
                {
                dr5["Phaseline"] = "No Results.";
                }
                if (concatStrngMj == "")
                {
                dr5["Conditionline"] = "No Results.";
                }
                else
                {
                    dr5["Conditionline"] = concatStrngMn;
                }

                if (concatStrng != "")
                {
                    dr5["Arrownote"] = concatStrng;
                }
                else
                {
                    dr5["Arrownote"] = "No Results.";
                }
                Dt.Rows.Add(dr5);
       

            }
            else
            {


                strQuery2 = "SELECT TOP 1" +                   
                    " Academic,Clinical,Community,Other FROM StdtSessEvent INNER JOIN StdtClinicalCoverSheet ON StdtSessEvent.StudentId=StdtClinicalCoverSheet.StudentId AND StdtSessEvent.SchoolId=StdtClinicalCoverSheet.SchoolId" +
                    " WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + stDate + "') AND CONVERT(DATE,'" + endDate + "') AND (StdtSessEventType='Major' OR StdtSessEventType='Minor' OR StdtSessEventType='Arrow notes') AND StdtSessEvent.StudentId=" + StudentId + " AND StdtSessEvent.SchoolId=" + SchoolId + " AND CAST( StdtClinicalCoverSheet.StartDate as date)= cast('" + stDate + "' as date) AND CAST( StdtClinicalCoverSheet.EndDate as date)= cast('" + endDate + "' as date)";
                System.Data.DataTable dtNew1 = objData.ReturnDataTable(strQuery2, false);
                Dt.Columns.Add("Academic", typeof(string));
                Dt.Columns.Add("Clinical", typeof(string));
                Dt.Columns.Add("Community", typeof(string));
                Dt.Columns.Add("Other", typeof(string));
                System.Data.DataRow dr5 = Dt.NewRow();
                if (dtNew1.Rows.Count > 0) {        
                    
                    dr5["Academic"] = Convert.ToString(dtNew1.Rows[0]["Academic"]);
                    dr5["Clinical"] = Convert.ToString(dtNew1.Rows[0]["Clinical"]);
                    dr5["Community"] = Convert.ToString(dtNew1.Rows[0]["Community"]);
                    dr5["Other"] = Convert.ToString(dtNew1.Rows[0]["Other"]);
                }
                else
                {
                    dr5["Academic"] = "";
                    dr5["Clinical"] = "";
                    dr5["Community"] = "";
                    dr5["Other"] = "";
                }
                
               
                String Mesrmntid = "select MeasurementId from BehaviourDetails b where b.StudentId= " + StudentId + " and activeind = 'A' ";
                System.Data.DataTable dtNew4 = objData.ReturnDataTable(Mesrmntid, false);
                System.Data.DataRow dr4 = Dt.NewRow();
                System.Data.DataTable dtNew5;
                System.Data.DataTable dtNew6;
                System.Data.DataTable dtNew01;
                System.Data.DataTable dtNew2;

                String Beh = "", Evnt = "", concatStrng = "", concatStrngMj = "", concatStrngMn = "";
                foreach (DataRow row in dtNew4.Rows)
                {
                    if (row != null)
                    {
                        String Behav = "SELECT Behaviour  from BehaviourDetails where MeasurementId =" + row["MeasurementId"];
                        dtNew5 = objData.ReturnDataTable(Behav, false);
                        Beh = Convert.ToString(dtNew5.Rows[0]["Behaviour"]);

                        string queryfilter = " SELECT STUFF((select '; ' + CONVERT(VARCHAR(50), outr.EvntTs,101)+','+ outr.eventname from( SELECT * FROM (SELECT * FROM  ((SELECT  SE.MeasurementId, SE.StdtSessEventId,  SE.EventName, " +
                      " SE.StdtSessEventType, CONVERT(CHAR(10), SE.EvntTs,101) AS EvntTs, " +
                      "  B.Behaviour FROM  [StdtSessEvent] SE LEFT JOIN LessonPlan L ON SE.LessonPlanId = L.LessonPlanId " +
                      "LEFT JOIN BehaviourDetails B ON B.MeasurementId=SE.MeasurementId WHERE EventType='EV' AND SE.StudentId=" + StudentId + " AND SE.StdtSessEventType<>'Medication') " +
                      //"UNION ALL (SELECT NULL AS MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
                      //"+ ( " +
                      //"        (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
                      //"        FROM   [user] US" +
                      //"       WHERE  US.userid = (SELECT" +
                      //"             createdby" +
                      //"                         FROM" +
                      //"           stdtsessionhdr Hdr" +
                      //"                       WHERE" +
                      //"         Hdr.stdtsessionhdrid = SH.ioasessionhdrid" +
                      //"        AND SH.ioaind = 'Y'))" +
                      //" + '/'" +
                      //" + (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
                      //"   FROM   [user] US" +
                      //"  WHERE  SH.ioauserid = US.userid) ) AS EventName," +
                      //" 'Arrow notes'                         AS" +
                      //" StdtSessEventType," +
                      //"CONVERT(CHAR(10), SH.endts, 101)      AS EvntTs," +
                      //"NULL                                  AS Behaviour" +
                      //" FROM   stdtsessionhdr SH" +
                      //"       LEFT JOIN lessonplan" +
                      //"             ON SH.lessonplanid = lessonplan.lessonplanid" +
                      //" WHERE  SH.ioaperc IS NOT NULL" +
                      //"      AND SH.ioaind = 'Y'" +
                      //"     AND SH.sessionstatuscd = 'S'" +
                      //"    AND SH.studentid =" + StudentId + ")" +
                      "UNION ALL (SELECT  BIOA.MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
                      "+ CASE WHEN BIOA.normalbehaviorid IS NULL THEN ((SELECT" +
                      "      TOP 1" +
                      "     Rtrim(" +
                      "    Ltrim(Upper(" +
                      "   US.userinitial))) FROM behaviour BH INNER JOIN [user] US" +
                      "  ON" +
                      " BH.createdby" +
                      " =" +
                      " US.userid WHERE BH.createdon BETWEEN" +
                        " Dateadd(minute, -5, BIOA.createdon)" +
                        " AND" +
                        " BIOA.createdon ORDER BY BH.createdon DESC)+'/'+ (SELECT" +
                        " TOP 1" +
                        " Rtrim(Ltrim(Upper(US.userinitial))) FROM" +
                        " behaviorioadetails BI" +
                        " INNER" +
                        " JOIN [user]" +
                        " US ON BI.createdby =" +
                        " US.userid WHERE BI.createdon=BIOA.createdon ORDER BY" +
                        " BI.createdon DESC)" +
                        " )" +
                        " ELSE ((" +
                        " SELECT" +
                        " Rtrim(Ltrim(Upper(US.userinitial))) FROM behaviour BH" +
                        " INNER" +
                        " JOIN [user]" +
                        " US ON" +
                        " BH.createdby = US.userid WHERE" +
                        " BIOA.normalbehaviorid=BH.behaviourid)+'/'+ (" +
                        " SELECT Rtrim(" +
                        " Ltrim(Upper(US.userinitial))) FROM behaviorioadetails BI" +
                        " INNER" +
                        " JOIN" +
                        " [user] US ON" +
                        " BI.createdby = US.userid INNER JOIN behaviour BH ON" +
                        " BH.behaviourid=BI.normalbehaviorid WHERE" +
                        " BIOA.normalbehaviorid=BH.behaviourid))" +
                        " END " +
                      " AS EventName, " +
                      "'Arrow notes' AS StdtSessEventType,  CONVERT(CHAR(10), BIOA.CreatedOn,101) AS EvntTs,  " +
                      " BHD.Behaviour FROM BehaviorIOADetails BIOA LEFT JOIN BehaviourDetails BHD ON BIOA.MeasurementId=BHD.MeasurementId " +
                      "WHERE BIOA.StudentId=" + StudentId + " AND IOAPerc IS NOT NULL AND BIOA.ActiveInd='A') )IOA ) " +
                      "  ad " +
                      " WHERE  ( ( ad.behaviour IS  NULL " +
                      " AND ad.measurementid = 0 )" +
                      "OR ad.behaviour = (SELECT TOP 1 behaviour " +
                      "FROM   behaviourdetails " +
                      "WHERE  measurementid = " + row["MeasurementId"] + ") ) " +
                      " AND ad.stdtsesseventtype IN( 'Arrow notes' ) " +
                      " AND CONVERT(DATE, ad.evntts) >=  cast('" + stDate + "' as date) " +
                      " AND CONVERT(DATE, ad.evntts) <=  cast('" + endDate + "' as date) " +
                      " )outr " +
                       " FOR XML PATH('')),1,1,'') eventname ";
                        dtNew6 = objData.ReturnDataTable(queryfilter, false);
                        if(Convert.ToString(dtNew6.Rows[0]["eventname"])!="" && Convert.ToString(dtNew6.Rows[0]["eventname"])!=null)
                         concatStrng += "&lt;b&gt;" + Beh + " : " + "&lt;/b&gt;" + Convert.ToString(dtNew6.Rows[0]["eventname"]) + System.Environment.NewLine;

                        string Phaseline = "SELECT 'Major Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst + "') AND CONVERT(DATE,'" + dted + "') AND " +
                                                       " (StdtSessEventType='Major' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + row["MeasurementId"] + ")) AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
                        dtNew1 = objData.ReturnDataTable(Phaseline, false);
                        if (Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != null)
                            concatStrngMj += "<b>" + Beh + " : " + "</b>" + Convert.ToString(dtNew1.Rows[0]["Eventdata"]) + "<br></br>";

                        string Conditionline = "SELECT 'Minor Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst + "') AND CONVERT(DATE,'" + dted + "') AND " +
                         " (StdtSessEventType='Minor' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + row["MeasurementId"] + ")) AND StudentId=" + StudentId + " AND SchoolId=" + SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
                        dtNew2 = objData.ReturnDataTable(Conditionline, false);
                        if (Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != null)
                            concatStrngMn += "<b>" + Beh + " : " + "</b>" + Convert.ToString(dtNew2.Rows[0]["Eventdata"]) + "<br></br>";

                    }

                }

                Dt.Columns.Add("Phaseline", typeof(string));
                Dt.Columns.Add("Conditionline", typeof(string));
                Dt.Columns.Add("Arrownote", typeof(string));
                //System.Data.DataRow dr5 = Dt.NewRow();
                if (concatStrngMj == "")
                {
                dr5["Phaseline"] = "No Results.";
                }
                else
                {
                    dr5["Phaseline"] = concatStrngMj;
                }
                if (concatStrngMn == "")
                {
                dr5["Conditionline"] = "No Results.";
                }
                else
                {
                    dr5["Conditionline"] = concatStrngMn;
                }
                if (concatStrng == "")
                {
                    dr5["Arrownote"] = "No Results.";
                }
                else
                {
                dr5["Arrownote"] = concatStrng;
                }
                Dt.Rows.Add(dr5);
       
            }

           // Dt = objData.ReturnDataTable(strQuery2, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }


    public DataTable getClinicalCvSheetAsmtTool(int StudentId, int SchoolId, int Clvid)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            // strQuery = "  Select CONVERT(VARCHAR(10),IE.EffStartDate, 103) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 103) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 103) AS DOB,S.StudentNbr,GL.GoalName,GL.GoalDesc,SG.Objective1+','+SG.Objective2+','+SG.Objective3 as BenchMarks,IE.Version FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " ";

            strQuery = "select TrgetBehav,Functions,AnalysisTool from clvAsmtTool where StdtCoverSheetId=" + Clvid + ""; // should change the values to sess

            Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }
    public DataTable getClinicalCvReinfo(int StudentId, int SchoolId, int Clvid)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            // strQuery = "  Select CONVERT(VARCHAR(10),IE.EffStartDate, 103) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 103) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 103) AS DOB,S.StudentNbr,GL.GoalName,GL.GoalDesc,SG.Objective1+','+SG.Objective2+','+SG.Objective3 as BenchMarks,IE.Version FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " ";

            strQuery = "select CONVERT(varchar(10),ReinfoDate,101) as ReinfoDate,ToolUtilizd from clvReinfoSur where StdtCoverSheetId=" + Clvid + " "; // should change the values to sess

            Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }
    public DataTable getClinicalCvRecChange(int StudentId, int SchoolId, int Clvid)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            // strQuery = "  Select CONVERT(VARCHAR(10),IE.EffStartDate, 103) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 103) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 103) AS DOB,S.StudentNbr,GL.GoalName,GL.GoalDesc,SG.Objective1+','+SG.Objective2+','+SG.Objective3 as BenchMarks,IE.Version FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " ";

            strQuery = "select Recomendation,TimeLine,PersonResponsible from clvRecChange where StdtCoverSheetId=" + Clvid + ""; // should change the values to sess

            Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }

    public DataTable getGridHeaderValz(int StudentId, int SchoolId, int Clvid)
    {

        try
        {

            objData = new clsData();
            Dt = new DataTable();

            // strQuery = "  Select CONVERT(VARCHAR(10),IE.EffStartDate, 103) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 103) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 103) AS DOB,S.StudentNbr,GL.GoalName,GL.GoalDesc,SG.Objective1+','+SG.Objective2+','+SG.Objective3 as BenchMarks,IE.Version FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " ";

            //---strQuery = "select Student.StudentFname+' '+Student.StudentLname as StdName,(SELECT ClassName FROM Class WHERE ClassId=Location) Location,(SELECT Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId=Program) Program,IepYear,PeriodOfAssmt from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + ""; // should change the values to sess
            //IEP dates fetched from behaviourdetails table//strQuery = "select Student.StudentPersonalId as StdtId,Student.StudentFname+' '+Student.StudentLname as StdName,(SELECT ClassName FROM Class WHERE ClassId=Location) Location,(SELECT Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId=Program) Program,IepYear,(SELECT TOP 1 convert(varchar,FORMAT (BehaviorIEPSDate,'MM/dd/yyyy'), 1) FROM BehaviourDetails WHERE StudentId = Student.StudentPersonalId AND BehaviorIEPSDate IS NOT NULL) AS IepStDate,(SELECT TOP 1 convert(varchar,FORMAT (BehaviorIEPEDate,'MM/dd/yyyy'), 1) FROM BehaviourDetails WHERE StudentId = Student.StudentPersonalId AND BehaviorIEPEDate IS NOT NULL) AS IepEnDate,PeriodOfAssmt,(SELECT convert(varchar,AsmntYearStartDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdstdate,(SELECT convert(varchar,AsmntYearEndDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdendate,LEFT(IepYear,CHARINDEX('-',IepYear)-1) as IepYearStart,LTRIM(RIGHT(IepYear,LEN(IepYear) - CHARINDEX('-',IepYear) )) AS IepYearend,convert(varchar,StdtClinicalCoverSheet.StartDate, 1) AS AssmntStdate,convert(varchar,StdtClinicalCoverSheet.EndDate, 1) AS AssmntEnddate from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";
            //Only For One location and one program//strQuery = "select Student.StudentPersonalId as StdtId,Student.StudentFname+' '+Student.StudentLname as StdName,(SELECT ClassName FROM Class WHERE ClassId=Location) Location,(SELECT Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId=Program) Program,IepYear,convert(varchar,FORMAT (StdtClinicalCoverSheet.ClinicalBehIEPSDate,'MM/dd/yyyy'), 1) AS IepStDate,convert(varchar,FORMAT (StdtClinicalCoverSheet.ClinicalBehIEPEDate,'MM/dd/yyyy'), 1) AS IepEnDate,PeriodOfAssmt,(SELECT convert(varchar,AsmntYearStartDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdstdate,(SELECT convert(varchar,AsmntYearEndDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdendate,LEFT(IepYear,CHARINDEX('-',IepYear)-1) as IepYearStart,LTRIM(RIGHT(IepYear,LEN(IepYear) - CHARINDEX('-',IepYear) )) AS IepYearend,convert(varchar,StdtClinicalCoverSheet.StartDate, 1) AS AssmntStdate,convert(varchar,StdtClinicalCoverSheet.EndDate, 1) AS AssmntEnddate from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";

            //strQuery = "select Student.StudentPersonalId as StdtId,Student.StudentFname+' '+Student.StudentLname as StdName,IepYear,convert(varchar,FORMAT (StdtClinicalCoverSheet.ClinicalBehIEPSDate,'MM/dd/yyyy'), 1) AS IepStDate,convert(varchar,FORMAT (StdtClinicalCoverSheet.ClinicalBehIEPEDate,'MM/dd/yyyy'), 1) AS IepEnDate,PeriodOfAssmt,(SELECT convert(varchar,AsmntYearStartDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdstdate,(SELECT convert(varchar,AsmntYearEndDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdendate,LEFT(IepYear,CHARINDEX('-',IepYear)-1) as IepYearStart,LTRIM(RIGHT(IepYear,LEN(IepYear) - CHARINDEX('-',IepYear) )) AS IepYearend,convert(varchar,StdtClinicalCoverSheet.StartDate, 1) AS AssmntStdate,convert(varchar,StdtClinicalCoverSheet.EndDate, 1) AS AssmntEnddate from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";
            strQuery = "select Student.StudentPersonalId as StdtId,Student.FirstName+' '+Student.LastName as StdName,IepYear,convert(varchar,FORMAT (StdtClinicalCoverSheet.ClinicalBehIEPSDate,'MM/dd/yyyy'), 1) AS IepStDate,convert(varchar,FORMAT (StdtClinicalCoverSheet.ClinicalBehIEPEDate,'MM/dd/yyyy'), 1) AS IepEnDate,PeriodOfAssmt,(SELECT convert(varchar,AsmntYearStartDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdstdate,(SELECT convert(varchar,AsmntYearEndDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode = PeriodOfAssmt) as Prdendate,convert(varchar,StdtClinicalCoverSheet.StartDate, 1) AS AssmntStdate,convert(varchar,StdtClinicalCoverSheet.EndDate, 1) AS AssmntEnddate from StdtClinicalCoverSheet inner join StudentPersonal Student on Student.StudentPersonalId=StdtClinicalCoverSheet.StudentId where StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";
            // strQuery = "select TOP 1 Student.StudentFname+' '+Student.StudentLname as StdName,stdiep.EffStartDate as IepYear,Location,Program,PeriodOfAssmt from StdtClinicalCoverSheet inner join Student on Student.StudentId=StdtClinicalCoverSheet.StudentId left join StdtIEP stdiep on Student.StudentId = stdiep.StudentId   where  StdtClinicalCoverSheet.StudentId=" + StudentId + " and StdtClinicalCoverSheet.SchoolId=" + SchoolId + " and ClinicalCvId=" + Clvid + "";

            Dt = objData.ReturnDataTable(strQuery, false);

        }
        catch (Exception Ex)
        {

        }
        return Dt;
    }
}