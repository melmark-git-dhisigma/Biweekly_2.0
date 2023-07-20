using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentBinder_Dashboard : System.Web.UI.Page
{
    clsSession oSession = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
    }
    /// <summary>
    /// function to get the IEP Expiry Report....
    /// This graph shows the count of IEPs which will be expire in 1,2,3 and >3 weeks....
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static List<string[]> getIEPExpGraph()
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        var Duration = new List<string[]>();
        if (oSession != null)
        {

            string qry = "  SELECT DATEDIFF(dd,CONVERT(VARCHAR(10), GETDATE(), 120),IEP.EffEndDate) as Count," +
                    "CASE WHEN DATEDIFF(dd,CONVERT(VARCHAR(10), GETDATE(), 120),IEP.EffEndDate)>21 THEN '>3 Weeks' " +
                    "WHEN DATEDIFF(dd,CONVERT(VARCHAR(10), GETDATE(), 120),IEP.EffEndDate)>14 THEN '2-3 Weeks' " +
                    "WHEN DATEDIFF(dd,CONVERT(VARCHAR(10), GETDATE(), 120),IEP.EffEndDate)>7 THEN '1-2 Weeks' " +
                    "ELSE '0-1 Weeks' END as Categry " +
                    "FROM StdtIEP IEP INNER JOIN StdtClass StdCls ON StdCls.StdtId=IEP.StudentId " +
                    "WHERE StdCls.ClassId=" + oSession.Classid + " AND IEP.EffEndDate>(SELECT CONVERT(VARCHAR(10), GETDATE(), 120))";

            DataTable dtIEP = oData.ReturnDataTable(qry, false);
           

            if (dtIEP != null)
            {
                DataRow[] dr_3wks = dtIEP.Select("Categry = '>3 Weeks'");
                DataRow[] dr_2wks = dtIEP.Select("Categry = '2-3 Weeks'");
                DataRow[] dr_1wks = dtIEP.Select("Categry = '1-2 Weeks'");
                DataRow[] dr_0wks = dtIEP.Select("Categry = '0-1 Weeks'");

                Duration.Add(new string[] { "0-1 Weeks", dr_0wks.Length.ToString() });
                Duration.Add(new string[] { "1-2 Weeks", dr_1wks.Length.ToString() });
                Duration.Add(new string[] { "2-3 Weeks", dr_2wks.Length.ToString() });
                Duration.Add(new string[] { ">3 Weeks", dr_3wks.Length.ToString() });

            }
        }
        return Duration;
    }
    /// <summary>
    /// function to get the LessonPlan Status....
    /// This graph shows the status(ie, In Progress,Approved,Pending Approval) of all LP in a class
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static List<object[]> getLPStatus()
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        string qry = "  SELECT LU.LookupName,COUNT(DTmp.DSTempHdrId) as Count " +
                "FROM ((StdtLessonPlan StdtLP INNER JOIN StdtClass StdCls ON StdCls.StdtId=StdtLP.StudentId) " +
                "INNER JOIN (DSTempHdr DTmp INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId) " +
                "ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId " +
                "LEFT OUTER JOIN (StdtIEP IEP INNER JOIN LookUp LUp ON LUp.LookupId=IEP.StatusId) " +
                "ON IEP.StdtIEPId=StdtLP.StdtIEPId) INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId " +
                "WHERE StdCls.ClassId=" + oSession.Classid + " AND StdtLP.ActiveInd='A' AND " +
                "((LUp.LookupName='Approved' AND StdtLP.IncludeIEP=1) OR LUp.LookupName IS NULL) " +
                "AND LU.LookupName<>'Expired'  GROUP BY LU.LookupName";

        DataTable dtLPStat = oData.ReturnDataTable(qry, false);
        var LPStat = new List<object[]>();
        if (dtLPStat != null)
        {
            foreach (DataRow dr in dtLPStat.Rows)
            {
                LPStat.Add(new object[] { dr["LookupName"].ToString(), Convert.ToInt32(dr["Count"].ToString()) });
            }

        }
        return LPStat;
    }
    /// <summary>
    /// function to get the IEP Status...
    /// This graph shows the status(ie, In Progress,Approved,Pending Approval) of all IEP in a class
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static List<object[]> getIEPStatus()
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        string qry = "  SELECT LU.LookupName,COUNT(StdtIEPId) as Count " +
                "FROM (StdtIEP IEP " +
                "INNER JOIN LookUp LU ON LU.LookUpId=IEP.StatusId) " +
                "INNER JOIN StdtClass StdCls ON StdCls.StdtId=IEP.StudentId " +
                "WHERE StdCls.ClassId=" + oSession.Classid + " AND LU.LookupName<>'Expired' " +
                "GROUP BY LU.LookupName";

        DataTable dtIEPStat = oData.ReturnDataTable(qry, false);
        var IEPStat = new List<object[]>();
        if (dtIEPStat != null)
        {
            foreach (DataRow dr in dtIEPStat.Rows)
            {
                IEPStat.Add(new object[] { dr["LookupName"].ToString(), Convert.ToInt32(dr["Count"].ToString()) });
            }

        }
        return IEPStat;
    }
    /// <summary>
    /// function to get the LessonPlan Scheduling Report....
    /// This graph shown the Completed LP against Total assigned LP of all students in a class
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static List<object[]> getLPSchedule()
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        string qry = "  SELECT Std.StudentLname,COUNT(LP.LPId) as TotLP,COUNT(Hdr.LessonPlanId) as ActualCount " +
                "FROM (Student Std " +
                "INNER JOIN StdtClass StdCls ON StdCls.StdtId=Std.StudentId) " +
                "LEFT OUTER JOIN (StdtLPSched LP LEFT OUTER JOIN StdtSessionHdr Hdr " +
                "ON Hdr.StudentId=LP.StdtId AND LP.LPId=Hdr.LessonPlanId) " +
                "ON LP.StdtId=Std.StudentId " +
                "WHERE StdCls.ClassId=" + oSession.Classid + " " +
                "GROUP BY Std.StudentLname " +
                "ORDER BY Std.StudentLname";

        DataTable dtLPSched = oData.ReturnDataTable(qry, false);
        var LPSched = new List<object[]>();
        if (dtLPSched != null)
        {
            object[] stdTs = new object[dtLPSched.Rows.Count];
            object[] totLPs = new object[dtLPSched.Rows.Count];
            object[] actLPs = new object[dtLPSched.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dtLPSched.Rows)
            {
                stdTs[i] = dr["StudentLname"].ToString();
                totLPs[i] = dr["TotLP"].ToString();
                actLPs[i] = dr["ActualCount"].ToString();
                i++;
            }
            LPSched.Add(stdTs); LPSched.Add(totLPs); LPSched.Add(actLPs);
        }
        return LPSched;
    }
}