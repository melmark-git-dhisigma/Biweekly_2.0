using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Graph : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    public string JsonSData;
    public string JsonTData;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack) PlotGraph("S"); PlotGraph("T");
    }
    private void PlotGraph(string Type)
    {
        sess = (clsSession)Session["UserSession"];
        List<JsonModel> JsonObj = new List<JsonModel>();

        List<Valuescl> val1 = new List<Valuescl>();
        List<Valuescl> val2 = new List<Valuescl>();

        DataTable Dt = new DataTable();

        if (Type == "S") { Dt = FillStudent(sess.Classid); }
        else { Dt = FillStaff(sess.Classid); }

        int count = 0;
        try
        {

            if (Dt != null)
            {
                count = Dt.Rows.Count;
                if (count > 0)
                {

                    object objLess = null, objBeh = null;
                    int LessP = 0, BehP = 0;
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {

                        objLess = Dt.Rows[i]["Lessons"]; if (objLess != null && objLess != "") { LessP = Convert.ToInt32(Dt.Rows[i]["Lessons"]); }
                        val1.Add(new Valuescl
                        {
                            label = (Dt.Rows[i]["Name"].ToString().Length > 20) ? Dt.Rows[i]["Name"].ToString().Substring(0, 20) + "..." : Dt.Rows[i]["Name"].ToString(),
                            value = LessP,

                        });
                        objBeh = (Type == "S") ? Dt.Rows[i]["TotalBehaviorCount"] : Dt.Rows[i]["Behavior"];
                        if (objBeh != null && objBeh != "") { BehP = Convert.ToInt32(objBeh); }
                        val2.Add(new Valuescl
                        {
                            label = (Dt.Rows[i]["Name"].ToString().Length > 20) ? Dt.Rows[i]["Name"].ToString().Substring(0, 20) + "..." : Dt.Rows[i]["Name"].ToString(),
                            value = BehP,
                        });

                        LessP = 0; BehP = 0;
                    }



                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        count = count * 62;
        if (Type == "S") { Student.Style.Add("Height", count.ToString() + "px"); }
        else { Teacher.Style.Add("Height", count.ToString() + "px"); }



        string Beh = "", Less = "";

        if (Type == "S") { Beh = "Behavior"; }
        else { Beh = "Behavior Count"; }

        if (Type == "S") { Less = "Lesson Plan"; }
        else { Less = "Lesson Plan Count"; }


        JsonObj.Add(new JsonModel
        {
            key = "Lesson Plan",
            color = "#1F77B4",
            values = val1
        });

        JsonObj.Add(new JsonModel
        {
            key = "Behavior",
            color = "#D62728",
            values = val2
        });

        JavaScriptSerializer JScriptObj = new JavaScriptSerializer();
        if (Type == "S")
        {
            JsonSData = JScriptObj.Serialize(JsonObj);
        }
        else
        {
            JsonTData = JScriptObj.Serialize(JsonObj);
        }

    }

    private DataTable FillStudent(int classid)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string ClassIds = Convert.ToString(objData.FetchValue("SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),ClassId) FROM (SELECT ClassId FROM UserClass WHERE UserId='" + sess.LoginId + "' AND ActiveInd='A') CLS FOR XML PATH('')),1,1,'')"));

        string StudentQuery = "SELECT *,CASE WHEN LessonCount>=LessonsCompleted THEN ISNULL(CASE WHEN STUDDATA.LessonCount<>0 THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT,STUDDATA.LessonsCompleted)/CONVERT(FLOAT,STUDDATA.LessonCount))*100,0))  END,0) ELSE 0 END AS Lessons ,ISNULL(CASE WHEN STUDDATA.BehaviorCount<>0 " +
  "THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT, STUDDATA.BehaviorCompleted)/CONVERT(FLOAT,STUDDATA.BehaviorCount))*100,0)) END,0) AS Behavior,(SELECT COUNT(*) FROM Behaviour BHR WHERE BHR.StudentId=STUDDATA.StudentId AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())) TotalBehaviorCount FROM (SELECT STUD.StudentId,STUD.Name,(SELECT COUNT(DISTINCT LessonPlanId) FROM StdtSessionHdr WHERE StudentId=STUD.StudentId AND CONVERT(VARCHAR(10), " +
"CreatedOn, 120)=CONVERT(VARCHAR(10), GETDATE(), 120)  AND [SessionStatusCd]='S') LessonsCompleted,(SELECT COUNT(DISTINCT LPId) FROM StdtLPSched SCH WHERE Day=CONVERT(VARCHAR(10), GETDATE(), 120) AND SCH.StdtId=STUD.StudentId) LessonCount, (SELECT COUNT(MeasurementId) FROM BehaviourDetails WHERE StudentId=STUD.StudentId AND ActiveInd='A') BehaviorCount," +
 "(SELECT Count(DISTINCT b.measurementid) FROM   behaviour b  join behaviourdetails bd on bd.MeasurementId=b.MeasurementId  WHERE b.studentid = STUD.StudentId AND b.activeind = 'A' And bd.ActiveInd='A' AND CONVERT(DATE,b.CreatedOn)=CONVERT(DATE,GETDATE())) BehaviorCompleted FROM (SELECT DISTINCT SDT.StudentId," +
 "SDT.StudentLname+','+SDT.StudentFname AS Name FROM Student SDT INNER JOIN StdtClass SCLS    ON SDT.StudentId=SCLS.StdtId WHERE SDT.ActiveInd='A' AND SCLS.ActiveInd='A' AND SCLS.ClassId IN (" + ClassIds + ")) STUD) STUDDATA";

        //string StudentQuery = "  SELECT distinct *,ISNULL(CASE WHEN STUD.LessonCount<>0 THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT, STUD.LessonsCompleted)/CONVERT(FLOAT,STUD.LessonCount))*100,0))  END,'')  " +
        //                        " AS Lessons ,ISNULL(CASE WHEN STUD.BehaviorCount<>0 THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT, STUD.BehaviorCompleted)/CONVERT(FLOAT,STUD.BehaviorCount))*100,0)) END,'')  " +
        //                        " AS Behavior FROM (SELECT SDT.StudentLname+','+SDT.StudentFname AS Name,(SELECT COUNT(DISTINCT LessonPlanId) FROM StdtSessionHdr WHERE StudentId=SDT.StudentId And  " +
        //                        "  convert(varchar(10), CreatedOn, 120)=convert(varchar(10), GETDATE(), 120)) LessonsCompleted,(Select COUNT(LPId) from StdtLPSched SCH where Day=convert(varchar(10), GETDATE(), 120)  " +
        //                        "  And SCH.StdtId=SDT.StudentId) LessonCount, (SELECT COUNT(MeasurementId) FROM BehaviourDetails WHERE StudentId=SDT.StudentId AND ActiveInd='A') BehaviorCount,  " +
        //                       " (SELECT Count(DISTINCT b.measurementid) FROM   behaviour b "
        //                       + " join behaviourdetails bd on bd.MeasurementId=b.MeasurementId "
        //                        + " WHERE  b.studentid = SDT.studentid AND b.activeind = 'A' And bd.ActiveInd='A') BehaviorCompleted"
        //                        +" FROM Student SDT INNER JOIN StdtClass SCLS  " +
        //                        "  ON SDT.StudentId=SCLS.StdtId WHERE SDT.ActiveInd='A' AND SCLS.ActiveInd='A' AND SCLS.ClassId IN ("+ClassIds+") ) STUD ";


        DataTable dtStudent = objData.ReturnDataTable(StudentQuery, false);




        return dtStudent;

    }
    private DataTable FillStaff(int classid)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string ClassIds = Convert.ToString(objData.FetchValue("SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),ClassId) FROM (SELECT ClassId FROM UserClass WHERE UserId='" + sess.LoginId + "' AND ActiveInd='A') CLS FOR XML PATH('')),1,1,'')"));

        string StudentQuery = "SELECT distinct USR.UserLName+ ',' +USR.UserFName AS Name,(SELECT COUNT(DISTINCT LessonPlanId) FROM (SELECT LessonPlanId,COUNT(1) Cnt,DSTempHdrId,(SELECT CASE WHEN DSMode='INACTIVE' THEN 0 ELSE 1 END FROM DSTempHdr DSH WHERE DSH.DSTempHdrId= "+
                              "SHDR.DSTempHdrId ) DSMode FROM StdtSessionHdr SHDR WHERE CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()) AND SHDR.CreatedBy=USR.UserId GROUP BY LessonPlanId,DSTempHdrId ) LP WHERE DSMode=1)  Lessons, " +
                              "(SELECT COUNT(DISTINCT MeasurementId) FROM Behaviour   WHERE CreatedBy=USR.UserId AND ActiveInd='A' And  " +
                              "convert(varchar(10), CreatedOn, 120)=convert(varchar(10), GETDATE(), 120)) Behavior FROM [User]  USR  " +
                              "INNER JOIN UserClass UCLS ON USR.UserId=UCLS.UserId WHERE USR.ActiveInd='A' AND UCLS.ActiveInd='A' AND UCLS.ClassId IN (" + ClassIds + ") ";

        DataTable dtStaff = objData.ReturnDataTable(StudentQuery, false);

        return dtStaff;
    }

}