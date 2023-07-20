using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsLessons
/// </summary>
public class clsLessons
{
    clsData oData = null;
    public clsLessons()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="studId"></param>
    /// <returns></returns>
    public DataTable getDSLessonPlans(string studId, string columns, string status)
    {
        DataTable dt = new DataTable();
        oData = new clsData();
        //string qry = "SELECT " + columns + " " +
        //        "FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId) " +
        //        "ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId " +
        //        "LEFT OUTER JOIN (StdtIEP IEP INNER JOIN LookUp LUp ON LUp.LookupId=IEP.StatusId) " +
        //        "ON IEP.StdtIEPId=StdtLP.StdtIEPId) " +
        //        "INNER JOIN LessonPlan LP " +
        //        "ON StdtLP.LessonPlanId=LP.LessonPlanId " +
        //        "WHERE StdtLP.StudentId=" + studId + " " +
        //        "AND StdtLP.ActiveInd='A' " +
        //        "AND ((LUp.LookupName='Approved' AND StdtLP.IncludeIEP=1) OR LUp.LookupName IS NULL) " +
        //        "AND " + status;
        string qry = "";
        string str = "";
        if (HttpContext.Current.Session["datasheet"] != null)
        {
            str = HttpContext.Current.Session["datasheet"].ToString();
        }
        if (str == "DatasheetTab")
        {
            qry = " SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND SessMissTrailStus <> 'Y' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + studId + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " ORDER BY  LessonOrder";
        }
        else if (str == "Schedule_View")
        {
            qry = " SELECT distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder,FORMAT(CAST(st.starttime AS DATETIME),'hh:mmtt') starttime ,convert(VARCHAR(4),st.starttime,108) as starttime1,CONVERT(varchar(15),  CAST(st.EndTime AS TIME), 100)  as EndTime," +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE	LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'D' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Draft, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter," +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE	LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 's' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101) AND CONVERT(time,ModifiedOn)>=convert(time,st.starttime) and CONVERT(time,ModifiedOn)<=convert(time,st.EndTime)),0))AS modi," +
                //"(select ISNULL((select top 1 IOAPerc from StdtSessionHdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid and convert(date,ModifiedOn)= convert(date,GETDATE()) order by 1 desc ),0)) as ioastatus," +
                "('IOA '+(select ISNULL((select top 1 IOAPerc from StdtSessionHdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid order by 1 desc ),0)) +" +
                "(SELECT  RTRIM(LTRIM(UPPER(UserInitial))) From [User] US WHERE US.UserId=(SELECT top 1 IOAUserId FROM StdtSessionHdr Hdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid and  IOAPerc is not null AND ModifiedOn=GETDATE()order by 1 desc ))+'/'+" +
                "(SELECT  RTRIM(LTRIM(UPPER(UserInitial))) From [User] US WHERE US.UserId=(SELECT top 1 ModifiedBy FROM StdtSessionHdr Hdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid and  IOAPerc is not null AND ModifiedOn=GETDATE()order by 1 desc ))) ioa" +

                //"('IOA '+(select ISNULL((select top 1 IOAPerc from StdtSessionHdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid order by 1 desc ),0)) "+
                //"(SELECT  RTRIM(LTRIM(UPPER(UserInitial))) From [User] US WHERE US.UserId=(SELECT top 1 IOAUserId FROM StdtSessionHdr Hdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid and Hdr.IOAInd='n' order by 1 desc ))"+		
                //"(SELECT  RTRIM(LTRIM(UPPER(UserInitial))) From [User] US WHERE US.UserId=(SELECT top 1 IOAUserId FROM StdtSessionHdr Hdr where LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid and Hdr.IOAInd='y'order by 1 desc ))) ioa,"+
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId inner join StdtLPSched st on LP.LessonPlanId=st.LPId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + studId + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " and convert(date,GETDATE())=st.Day order by starttime1 asc";
            //ORDER BY st.starttime";
        }
        else
        {
            qry = " SELECT  distinct " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder " +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + studId + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " ORDER BY  LessonOrder";
        }
        dt = oData.ReturnDataTable(qry, false);
        return dt;
    }
    public DataTable getDSLessonPlans1(string studId, string columns, string status)
    {
        DataTable dt = new DataTable();
        oData = new clsData();       
        string qry = "";

            qry = " SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + studId + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " ORDER BY  LessonOrder";
       
        dt = oData.ReturnDataTable(qry, false);
        return dt;
    }
    public DataTable getLessonPlans(string studId, string columns, string status)
    {
        DataTable dt = new DataTable();
        oData = new clsData();
        string qry = "";



        qry = " SELECT DISTINCT " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder,Dtmp.CreatedOn AS CreatedOn " +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + studId + " AND " +
                                     "  StdtLP.ActiveInd='A' AND " + status + " ORDER BY  CreatedOn DESC";

        dt = oData.ReturnDataTable(qry, false);
        return dt;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="studId"></param>
    /// <param name="columns"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public DataTable getLPwithRank(string studId, string columns)
    {
        DataTable dt = new DataTable();
        oData = new clsData();

        string qry = "SELECT " + columns + "," +
            "RANK() OVER(PARTITION BY DTmp.LessonPlanId " +
            "ORDER BY (CASE(LU.LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' THEN 2 WHEN 'Approved' THEN 3 END)) as Rank " +
            "FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId) " +
            "ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId " +
            "LEFT OUTER JOIN (StdtIEP IEP INNER JOIN LookUp LUp ON LUp.LookupId=IEP.StatusId) " +
            "ON IEP.StdtIEPId=StdtLP.StdtIEPId) INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId " +
            "WHERE StdtLP.StudentId=" + studId + " AND StdtLP.ActiveInd='A' AND " +
            "((LUp.LookupName='Approved' ) OR LUp.LookupName IS NULL) " +
            "AND LU.LookupName<>'Expired'";

        dt = oData.ReturnDataTable(qry, false);
        return dt;
    }

}