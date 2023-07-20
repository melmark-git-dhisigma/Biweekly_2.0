using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
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
using System.Xml.Linq;
using System.Text;
using System.Web.Services;
using Microsoft.Office.Interop.Word;
using System.Threading;


public partial class StudentBinder_IEPView : System.Web.UI.Page
{
    clsData objData = null;
    ClsTemplateSession ObjTempSess = null;
    clsSession Sess = null;

    clsSession sess = null;
    static int intStdtId = 0;
    static int schoolId = 0;
    int intStdtIEPId = 0;
    System.Data.DataTable Dt = null;

    System.Data.DataTable Dt1 = null;
    System.Data.DataTable Dt2 = null;
    System.Data.DataTable Dt3 = null;

    static string[] columnsText;
    static string[] placeHolders;

    Hashtable htChkbxColumn;
    Hashtable htChkbcPlacHldr;

    static string[] plcTextHolders;
    static string[] plcChkHolders;

    static string[] columnsChk;
    static string[] placeHoldersAll = new string[0];

    static string[] columnsCheck;
    static string[] placeHoldersCheck;


    static string[] chkAllColumns;
    static string[] chkAllPlaceHolders;


    static string[] columnsP4;
    static string[] placeHoldersP4;

    static int checkCount = 0;
    static int P4TotalCount = 1;
    // static string strQuery = "";
    //  bool Disable = false;
    static System.Data.DataTable dtIEP4 = null;

    int RCount = 0;
    int PCount = 0;

    string StrQuery3 = "";
    //DataTable Dt3 = new DataTable();

    static string[] columns;
    static string[] columnsAll = new string[0];

    protected void Page_Load(object sender, EventArgs e)
    {

        Sess = (clsSession)Session["UserSession"];
        if (Sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(Sess.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        if (!IsPostBack)
        {
            //GetStdtIEPId();

            LoadData();
        }
    }

    private void LoadData()
    {
        try
        {
            if (Request.QueryString["studid"] != null)
            {
                int pageid = Convert.ToInt32(Request.QueryString["pageid"].ToString());
                int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());

                Sess.StudentId = studid;
                Sess.IEPId = pageid;

            }
            FillIEPPage1();
            FillIEPPage2();
            FillIEPPage3();
            FillIEPPage4();
            FillIEPPage5();
            FillIEPPage6();
            FillIEPPage7();
            FillIEPPage8();
            FillIEPPage9();
            FillIEPPage10();
            FillIEPPage11();
            FillIEPPage12();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }







    protected void GetStdtIEPId()
    {
        objData = new clsData();
        string strQuery = "Select StdtIEPId from StdtIEP where StudentId='" + intStdtId + "'";
        intStdtIEPId = objData.ExecuteWithScope(strQuery);
    }

    protected void FillIEPPage1()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        try
        {
            string strQuery = "Select CONVERT(varchar(10), StdtIEP.EffStartDate, 101) as EffStartDate, CONVERT(varchar(10), StdtIEP.EffEndDate, 101) as EffEndDate, "
                             + "StdtIEP.Concerns, StdtIEP.Strength, StdtIEP.Vision, Student.SchoolId, Student.StudentId, Student.StudentNbr, "
                             + "Student.StudentFName+' '+StudentLName as StudentName,CONVERT(varchar(10), Student.DOB, 101) as DOB, Student.GradeLevel, School.DistrictName, "
                             + "School.DistAddrId, School.DistContact+'/'+DistPhone as DistContact, Address.AddressLine1+' '+Address.AddressLine2+' '+Address.AddressLine3 as Address from StdtIEP LEFT JOIN Student ON StdtIEP.StudentId = Student.StudentId "
                             + "LEFT JOIN School ON School.SchoolId= Student.SchoolId LEFT JOIN Address ON School.AddressId=Address.AddressId where StdtIEP.StdtIEPId='" + Sess.IEPId + "'";
            // objData.Execute(strQuery);
            Dt = objData.ReturnDataTable(strQuery, false);

            //  if (Dt != null) return;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblSclDistName.Text = Dt.Rows[0]["DistrictName"].ToString().Trim();
                    lblSclDistContPerson.Text = Dt.Rows[0]["DistContact"].ToString().Trim();
                    lblSclDistAdd.Text = Dt.Rows[0]["Address"].ToString().Trim();
                    lblDateFrom1.Text = Dt.Rows[0]["EffStartDate"].ToString().Trim();
                    lblDateTo1.Text = Dt.Rows[0]["EffEndDate"].ToString().Trim();
                    lblParConcerns1.Text = Dt.Rows[0]["Concerns"].ToString().Trim();
                    lblStStrength1.Text = Dt.Rows[0]["Strength"].ToString().Trim();
                    lblVision1.Text = Dt.Rows[0]["Vision"].ToString().Trim();
                    lblStName1.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    lblDOB1.Text = Dt.Rows[0]["DOB"].ToString().Trim();
                    lblID1.Text = Dt.Rows[0]["StudentNbr"].ToString().Trim();
                    lblGrade1.Text = Dt.Rows[0]["GradeLevel"].ToString().Trim();


                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillIEPPage2()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        string strQuery = "SELECT IEP1.* FROM StdtIEP IEP INNER JOIN StdtIEPExt1 IEP1 ON IEP.StdtIEPId = IEP1.StdtIEPId WHERE IEP.StdtIEPId=" + Sess.IEPId + "";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt.Rows.Count > 0)
        {
            lblSpec.Text = Dt.Rows[0]["OtherDesc"].ToString().Trim();
            lblDisabilities2.Text = Dt.Rows[0]["AffectDesc"].ToString().Trim();
            lblAccomodation2.Text = Dt.Rows[0]["AccomDesc"].ToString().Trim();
            lblContent2.Text = Dt.Rows[0]["ContentModDesc"].ToString().Trim();
            lblMethodology2.Text = Dt.Rows[0]["MethodModDesc"].ToString().Trim();
            lblPerformance2.Text = Dt.Rows[0]["PerfModDesc"].ToString().Trim();
            chkEngLangArts2.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngLangInd"].ToString());
            chkHistAndSS.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistInd"].ToString());
            chkScAndTech2.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechInd"].ToString());
            chkMaths2.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathInd"].ToString());
            chkOtherCurr2.Checked = clsGeneral.getChecked(Dt.Rows[0]["OtherInd"].ToString());
            chkContent2.Checked = clsGeneral.getChecked(Dt.Rows[0]["ContentModInd"].ToString());
            chkMethodology2.Checked = clsGeneral.getChecked(Dt.Rows[0]["MethodModInd"].ToString());
            chkPerformance2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PerfModInd"].ToString());
        }
        chkEngLangArts2.Attributes.Add("onclick", "return false");
        chkHistAndSS.Attributes.Add("onclick", "return false");
        chkScAndTech2.Attributes.Add("onclick", "return false");
        chkMaths2.Attributes.Add("onclick", "return false");
        chkOtherCurr2.Attributes.Add("onclick", "return false");
        chkContent2.Attributes.Add("onclick", "return false");
        chkMethodology2.Attributes.Add("onclick", "return false");
        chkPerformance2.Attributes.Add("onclick", "return false");

    }

    protected void FillIEPPage3()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        string strQuery = "SELECT IEP1.* FROM  StdtIEP IEP INNER JOIN StdtIEPExt2 IEP1 ON IEP.StdtIEPId = IEP1.StdtIEPId WHERE IEP.StdtIEPId='" + Sess.IEPId + "' ";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt.Rows.Count > 0)
        {
            lblDisabilities3.Text = Dt.Rows[0]["AffectDesc"].ToString().Trim();
            lblAccomodation3.Text = Dt.Rows[0]["AccomDesc"].ToString().Trim();
            lblContent3.Text = Dt.Rows[0]["ContentModDesc"].ToString().Trim();
            lblMethodology3.Text = Dt.Rows[0]["MethodModDesc"].ToString().Trim();
            lblPerformance3.Text = Dt.Rows[0]["PerfModDesc"].ToString().Trim();
            lblOther3.Text = Dt.Rows[0]["OtherDesc"].ToString().Trim();

            chkAdPhyEdu3.Checked = clsGeneral.getChecked(Dt.Rows[0]["PEInd"].ToString());
            chkAssTechDevice3.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechDevicesInd"].ToString());
            chkBehavior3.Checked = clsGeneral.getChecked(Dt.Rows[0]["BehaviorInd"].ToString());
            chkBrailleNeeds3.Checked = clsGeneral.getChecked(Dt.Rows[0]["BrailleInd"].ToString());

            chkCommunicationAll3.Checked = clsGeneral.getChecked(Dt.Rows[0]["CommInd"].ToString());
            chkCommunicationDeaf3.Checked = clsGeneral.getChecked(Dt.Rows[0]["CommDfInd"].ToString());
            chkExtraCurrAct3.Checked = clsGeneral.getChecked(Dt.Rows[0]["ExtCurInd"].ToString());
            chkLangNeeds3.Checked = clsGeneral.getChecked(Dt.Rows[0]["LEPInd"].ToString());

            chkNonAcadActivities3.Checked = clsGeneral.getChecked(Dt.Rows[0]["NonAcdInd"].ToString());
            chkSocial3.Checked = clsGeneral.getChecked(Dt.Rows[0]["SocialInd"].ToString());
            chkTravelTraining3.Checked = clsGeneral.getChecked(Dt.Rows[0]["TravelInd"].ToString());
            chkVocatEducation3.Checked = clsGeneral.getChecked(Dt.Rows[0]["VocInd"].ToString());

            chkOther3.Checked = clsGeneral.getChecked(Dt.Rows[0]["OtherInd"].ToString());
            chkChild3to5.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand1Ind"].ToString());
            chkChild14.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand2Ind"].ToString());
            chkChild16.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand3Ind"].ToString());

            chkContent3.Checked = clsGeneral.getChecked(Dt.Rows[0]["ContentModInd"].ToString());
            chkMethodology3.Checked = clsGeneral.getChecked(Dt.Rows[0]["MethodModInd"].ToString());
            chkPerformance3.Checked = clsGeneral.getChecked(Dt.Rows[0]["PerfModInd"].ToString());
        }
        lblDisabilities3.Attributes.Add("onclick", "return false");
        lblAccomodation3.Attributes.Add("onclick", "return false");
        lblContent3.Attributes.Add("onclick", "return false");
        lblMethodology3.Attributes.Add("onclick", "return false");
        lblPerformance3.Attributes.Add("onclick", "return false");
        lblOther3.Attributes.Add("onclick", "return false");

        chkAdPhyEdu3.Attributes.Add("onclick", "return false");
        chkAssTechDevice3.Attributes.Add("onclick", "return false");
        chkBehavior3.Attributes.Add("onclick", "return false");
        chkBrailleNeeds3.Attributes.Add("onclick", "return false");

        chkCommunicationAll3.Attributes.Add("onclick", "return false");
        chkCommunicationDeaf3.Attributes.Add("onclick", "return false");
        chkExtraCurrAct3.Attributes.Add("onclick", "return false");
        chkLangNeeds3.Attributes.Add("onclick", "return false");

        chkNonAcadActivities3.Attributes.Add("onclick", "return false");
        chkSocial3.Attributes.Add("onclick", "return false");
        chkTravelTraining3.Attributes.Add("onclick", "return false");
        chkVocatEducation3.Attributes.Add("onclick", "return false");

        chkOther3.Attributes.Add("onclick", "return false");
        chkChild3to5.Attributes.Add("onclick", "return false");
        chkChild14.Attributes.Add("onclick", "return false");
        chkChild16.Attributes.Add("onclick", "return false");

        chkContent3.Attributes.Add("onclick", "return false");
        chkMethodology3.Attributes.Add("onclick", "return false");
        chkPerformance3.Attributes.Add("onclick", "return false");
    }

    protected void FillIEPPage4()
    {
        sess = (clsSession)Session["UserSession"];
        System.Data.DataTable dtGoal = new System.Data.DataTable();
        objData = new clsData();
        Dt = new System.Data.DataTable();
        string strQuery = "select distinct Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId=" + sess.IEPId + " and StdtLessonPlan.IncludeIEP=1";
        Dt = objData.ReturnDataTable(strQuery, false);
        string GoalIdZ = "";
        int countForGoalId = 0;
        foreach (DataRow dr in Dt.Rows)
        {
            if (countForGoalId == 0)
            {
                GoalIdZ += dr["Id"].ToString();
            }
            else if (countForGoalId > 0)
            {
                GoalIdZ += "," + dr["Id"].ToString();
            }
            countForGoalId++;
        }

        if (Dt.Rows.Count == 0)
        {
            GoalIdZ = "0";
        }




        strQuery = "SELECT CASE A.Row WHEN 1 THEN '<div class=goalTitle>Goal No : '+CONVERT(varchar(10),IEPGoalNo)+' - '+A.GoalCode+'</div>' " +
"ELSE CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END  END AS Title," +
"CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END " +
"LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row,A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1, " +
"A.Objective2, A.Objective3,IEPGoalNo,GoalIEPNote,LessonOrder" +
" FROM (SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode," +
"dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber,ROW_NUMBER() " +
"OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row, dbo.StdtLessonPlan.LessonPlanId, " +
"dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, " +
"dbo.StdtLessonPlan.Objective3, (SELECT TOP 1 DSTemplateName FROM DSTempHdr " +
"WHERE StdtLessonplanId= dbo.StdtLessonPlan.StdtLessonPlanId ORDER BY DSTempHdrId DESC) DSTemplateName," +
"(SELECT TOP 1 LessonOrder  FROM DSTempHdr" +
" WHERE LessonplanId= dbo.StdtLessonPlan.LessonPlanId ORDER BY DSTempHdrId DESC) LessonOrder" +
" FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan " +
 "ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + GoalIdZ + ") " +
 "AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A' AND dbo.StdtLessonPlan.IncludeIEP=1)A " +
" INNER JOIN StdtGoal  on StdtGoal.GoalId =A.GoalId " +
"WHERE StdtGoal.StdtIEPId='" + sess.IEPId + "' and StdtGoal.IncludeIEP=1 ORDER BY IEPGoalNo,LessonOrder";
        Dt = objData.ReturnDataTable(strQuery, false);


        if (Dt != null)
        {



            dtGoal.Columns.Add(new DataColumn("Title", typeof(string)));
            dtGoal.Columns.Add(new DataColumn("StdtLessonPlanId", typeof(Int32)));
            dtGoal.Columns.Add(new DataColumn("LessonPlanId", typeof(Int32)));
            dtGoal.Columns.Add(new DataColumn("Objective2", typeof(string)));
            dtGoal.Columns.Add(new DataColumn("Objective3", typeof(string)));
            dtGoal.Columns.Add(new DataColumn("GoalId", typeof(Int32)));
            dtGoal.Columns.Add(new DataColumn("GoalIEPNote", typeof(string)));


            foreach (DataRow row in Dt.Rows)
            {
                if (row["Title"].ToString().Contains("</div>"))
                {
                    dtGoal.Rows.Add(row["Title"].ToString(), row["StdtLessonPlanId"], row["LessonPlanId"], row["Objective2"], row["Objective3"], row["GoalId"], row["GoalIEPNote"]);
                }


                //            string strQuery2 = "SELECT CASE A.Row WHEN 1 THEN '<div class=goalTitle>'+A.GoalCode+'</div>' ELSE A.LessonPlanName END AS Title,* FROM(" +
                //"SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As IEPGoalNo," +
                //"ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row," +
                //            " dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, " +
                //            " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3  " +
                //                    " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
                //                         "dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.LessonPlanId = " + Convert.ToInt32(row["LessonPlanId"]) + " AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A')A ORDER BY A.GoalId";

                //            DataTable Dt2 = objData.ReturnDataTable(strQuery2, false);

            }

            RepPage4.DataSource = dtGoal;
            ViewState["RepData"] = dtGoal;
            RepPage4.DataBind();

            //if (dtGoal != null)
            //{
            //    if (dtGoal.Rows.Count > 0)
            //    {
            //        //lblObj2.Text = Dt.Rows[0]["Objective2"].ToString().Trim();
            //    }
            //}
        }
    }

    protected void FillIEPPage5()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();

        string strQuery = "SELECT SGS.StdtGoalId as IEPGoalNo,SGS.StdtGoalSvcId, SGS.SvcDelTyp, SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), " +
                          "SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS " +
                          "ON IEP.StdtIEPId = SGS.StdtIEPId  WHERE     IEP.StdtIEPId = " + Sess.IEPId + " AND IEP.SchoolId = " + Sess.SchoolId + " AND IEP.StudentId = " + Sess.StudentId + " and SGS.SvcDelTyp='A' ";

        //strQuery = "SELECT StdtGoalSvc.StdtGoalId  as IEPGoalNo,StdtGoalSvc.SvcDelTyp,StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc,StdtGoalSvc.FreqDurDesc, "
        //                + "CONVERT(varchar(10), StdtGoalSvc.StartDate, 101) as StartDate, CONVERT(varchar(10), StdtGoalSvc.EndDate, 101) as EndDate, "
        //                + "StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,StdtIEPExt2.OtherDesc1, Goal.GoalId, "
        //                + "Goal.GoalName FROM StdtGoalSvc LEFT JOIN StdtIEPExt2 ON StdtGoalSvc.StdtIEPId=StdtIEPExt2.StdtIEPId Inner Join StdtGoal SG On "
        //                + "SG.StdtIEPId=StdtIEPExt2.StdtIEPId Left join Goal ON SG.GoalId=Goal.GoalId   "
        //               + " WHERE StdtGoalSvc.StdtIEPId= " + Sess.IEPId + " AND StdtGoalSvc.SvcDelTyp='A' order by SG.IEPGoalNo";

        //string strQuery = "SELECT SG.IEPGoalNo as IEPGoalNo,StdtGoalSvc.SvcDelTyp,StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc,StdtGoalSvc.FreqDurDesc, "
        //                 + "CONVERT(varchar(10), StdtGoalSvc.StartDate, 101) as StartDate, CONVERT(varchar(10), StdtGoalSvc.EndDate, 101) as EndDate, "
        //                 + "StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,StdtIEPExt2.OtherDesc1, Goal.GoalId, "
        //                 + "Goal.GoalName FROM StdtGoalSvc LEFT JOIN StdtIEPExt2 ON StdtGoalSvc.StdtIEPId=StdtIEPExt2.StdtIEPId Inner Join StdtGoal SG On "
        //                 + "SG.StdtGoalId=StdtGoalSvc.StdtGoalId Left join Goal ON SG.GoalId=Goal.GoalId   "
        //                 + " WHERE StdtGoalSvc.StdtIEPId= " + Sess.IEPId + " AND StdtGoalSvc.SvcDelTyp='A' order by SG.IEPGoalNo";


        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);
        Rep1.DataSource = Dt;
        Rep1.DataBind();

        //string strQuery1 = "SELECT StdtGoalSvc.StdtGoalId  as IEPGoalNo,StdtGoalSvc.SvcDelTyp,StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc,StdtGoalSvc.FreqDurDesc, "
        //                + "CONVERT(varchar(10), StdtGoalSvc.StartDate, 101) as StartDate, CONVERT(varchar(10), StdtGoalSvc.EndDate, 101) as EndDate, "
        //                + "StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,StdtIEPExt2.OtherDesc1, Goal.GoalId, "
        //                + "Goal.GoalName FROM StdtGoalSvc LEFT JOIN StdtIEPExt2 ON StdtGoalSvc.StdtIEPId=StdtIEPExt2.StdtIEPId Inner Join StdtGoal SG On "
        //                + "SG.StdtIEPId=StdtIEPExt2.StdtIEPId Left join Goal ON SG.GoalId=Goal.GoalId   "
        //               + " WHERE StdtGoalSvc.StdtIEPId= " + Sess.IEPId + " AND StdtGoalSvc.SvcDelTyp='B' order by SG.IEPGoalNo";

        string strQuery1 = "SELECT SGS.StdtGoalId as IEPGoalNo,SGS.StdtGoalSvcId, SGS.SvcDelTyp, SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), " +
                          "SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS " +
                          "ON IEP.StdtIEPId = SGS.StdtIEPId  WHERE     IEP.StdtIEPId = " + Sess.IEPId + " AND IEP.SchoolId = " + Sess.SchoolId + " AND IEP.StudentId = " + Sess.StudentId + " and SGS.SvcDelTyp='B' ";

        //string strQuery1 = "SELECT SG.IEPGoalNo as IEPGoalNo,StdtGoalSvc.SvcDelTyp,StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc,StdtGoalSvc.FreqDurDesc, "
        //                 + "CONVERT(varchar(10), StdtGoalSvc.StartDate, 101) as StartDate, CONVERT(varchar(10), StdtGoalSvc.EndDate, 101) as EndDate, "
        //                 + "StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,StdtIEPExt2.OtherDesc1, Goal.GoalId, "
        //                 + "Goal.GoalName FROM StdtGoalSvc LEFT JOIN StdtIEPExt2 ON StdtGoalSvc.StdtIEPId=StdtIEPExt2.StdtIEPId Inner Join StdtGoal SG On "
        //                 + "SG.StdtGoalId=StdtGoalSvc.StdtGoalId Left join Goal ON SG.GoalId=Goal.GoalId   "
        //                + " WHERE StdtGoalSvc.StdtIEPId= " + Sess.IEPId + " AND StdtGoalSvc.SvcDelTyp='B'";


        objData.Execute(strQuery1);
        Dt = objData.ReturnDataTable(strQuery1, false);
        Rep2.DataSource = Dt;
        Rep2.DataBind();


        //string strQuery2 = "SELECT StdtGoalSvc.StdtGoalId  as IEPGoalNo,StdtGoalSvc.SvcDelTyp,StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc,StdtGoalSvc.FreqDurDesc, "
        //                 + "CONVERT(varchar(10), StdtGoalSvc.StartDate, 101) as StartDate, CONVERT(varchar(10), StdtGoalSvc.EndDate, 101) as EndDate, "
        //                 + "StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,StdtIEPExt2.OtherDesc1, Goal.GoalId, "
        //                 + "Goal.GoalName FROM StdtGoalSvc LEFT JOIN StdtIEPExt2 ON StdtGoalSvc.StdtIEPId=StdtIEPExt2.StdtIEPId Inner Join StdtGoal SG On "
        //                 + "SG.StdtIEPId=StdtIEPExt2.StdtIEPId Left join Goal ON SG.GoalId=Goal.GoalId   "
        //                + " WHERE StdtGoalSvc.StdtIEPId= " + Sess.IEPId + " AND StdtGoalSvc.SvcDelTyp='C' order by SG.IEPGoalNo";
        string strQuery2 = "SELECT SGS.StdtGoalId as IEPGoalNo,SGS.StdtGoalSvcId, SGS.SvcDelTyp, SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), " +
                          "SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate, StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,StdtIEPExt2.OtherDesc1 FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS " +
                          "ON IEP.StdtIEPId = SGS.StdtIEPId inner join StdtIEPExt2 on SGS.StdtIEPId=StdtIEPExt2.StdtIEPId  WHERE     IEP.StdtIEPId = " + Sess.IEPId + " AND IEP.SchoolId = " + Sess.SchoolId + " AND IEP.StudentId = " + Sess.StudentId + " and SGS.SvcDelTyp='C' ";


        objData.Execute(strQuery2);
        Dt = objData.ReturnDataTable(strQuery2, false);
        Rep3.DataSource = Dt;
        Rep3.DataBind();

        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                lblOther5.Text = Dt.Rows[0]["OtherDesc1"].ToString().Trim();

                chk5DayCycle5.Checked = clsGeneral.getChecked(Dt.Rows[0]["5DayInd"].ToString());
                chk6DayCycle5.Checked = clsGeneral.getChecked(Dt.Rows[0]["6DayInd"].ToString());
                chk10DayCycle5.Checked = clsGeneral.getChecked(Dt.Rows[0]["10DayInd"].ToString());
                chkOther5.Checked = clsGeneral.getChecked(Dt.Rows[0]["DayOther"].ToString());

                chk5DayCycle5.Attributes.Add("onclick", "return false");
                chk6DayCycle5.Attributes.Add("onclick", "return false");
                chk10DayCycle5.Attributes.Add("onclick", "return false");
                chkOther5.Attributes.Add("onclick", "return false");
            }
        }
    }

    protected void FillIEPPage6()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        string strQuery = "Select IEP3.StdtIEPId,IEP3.RemovedInd1,IEP3.RemovedInd2,IEP3.RemovedDesc,IEP3.ShorterCd1,IEP3.ShorterCd2,IEP3.ShorterCd3,IEP3.LongerCd1,IEP3.LongerCd2,"
                         + "IEP3.LongerCd3,IEP3.SchedModDesc,IEP3.TransportInd1,IEP3.TransportInd2,IEP3.RegTransInd,IEP3.RegTransDesc,IEP3.SpTransInd ,IEP3.SpTransDesc "
                         + "from StdtIEPExt3 IEP3  INNER JOIN StdtIEP IEP  ON IEP.StdtIEPId  =IEP3.StdtIEPId where IEP.StdtIEPId='" + Sess.IEPId + "' ";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt.Rows.Count > 0)
        {
            lblRemove6.Text = Dt.Rows[0]["RemovedDesc"].ToString().Trim();
            lblSchedule6.Text = Dt.Rows[0]["SchedModDesc"].ToString().Trim();
            lblTransportationYesRegular6.Text = Dt.Rows[0]["RegTransDesc"].ToString().Trim();
            lblTransportationYesSpecial6.Text = Dt.Rows[0]["SpTransDesc"].ToString().Trim();

            chkRemoveNo6.Checked = clsGeneral.getChecked(Dt.Rows[0]["RemovedInd1"].ToString());
            chkRemoveYes6.Checked = clsGeneral.getChecked(Dt.Rows[0]["RemovedInd2"].ToString());

            chkShorterNo6.Checked = clsGeneral.getChecked(Dt.Rows[0]["ShorterCd1"].ToString());
            chkShorterYesDay6.Checked = clsGeneral.getChecked(Dt.Rows[0]["ShorterCd2"].ToString());
            chkShorterYesYear6.Checked = clsGeneral.getChecked(Dt.Rows[0]["ShorterCd3"].ToString());

            chkLongerNo6.Checked = clsGeneral.getChecked(Dt.Rows[0]["LongerCd1"].ToString());
            chkLongerYesShDay6.Checked = clsGeneral.getChecked(Dt.Rows[0]["LongerCd2"].ToString());
            chkLongerYesShYear6.Checked = clsGeneral.getChecked(Dt.Rows[0]["LongerCd3"].ToString());

            chkTransportationNo6.Checked = clsGeneral.getChecked(Dt.Rows[0]["TransportInd1"].ToString());
            chkTransportationYes6.Checked = clsGeneral.getChecked(Dt.Rows[0]["TransportInd2"].ToString());

            chkTransportationYesRegular6.Checked = clsGeneral.getChecked(Dt.Rows[0]["RegTransInd"].ToString());
            chkTransportationYesSpecial6.Checked = clsGeneral.getChecked(Dt.Rows[0]["SpTransInd"].ToString());

            chkRemoveNo6.Attributes.Add("onclick", "return false");
            chkRemoveYes6.Attributes.Add("onclick", "return false");

            chkShorterNo6.Attributes.Add("onclick", "return false");
            chkShorterYesDay6.Attributes.Add("onclick", "return false");
            chkShorterYesYear6.Attributes.Add("onclick", "return false");

            chkLongerNo6.Attributes.Add("onclick", "return false");
            chkLongerYesShDay6.Attributes.Add("onclick", "return false");
            chkLongerYesShYear6.Attributes.Add("onclick", "return false");

            chkTransportationNo6.Attributes.Add("onclick", "return false");
            chkTransportationYes6.Attributes.Add("onclick", "return false");

            chkTransportationYesRegular6.Attributes.Add("onclick", "return false");
            chkTransportationYesSpecial6.Attributes.Add("onclick", "return false");
        }
    }

    protected void FillIEPPage7()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        string strQuery = "SELECT   * FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt3 IEP3 ON IEP1.StdtIEPId = IEP3.StdtIEPId WHERE  IEP1.StdtIEPId = '" + Sess.IEPId + "' ";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt.Rows.Count > 0)
        {
            lblAsmntPlanned7.Text = Dt.Rows[0]["AsmntPlanned"].ToString().Trim();
            lblInfoi.Text = Dt.Rows[0]["InfoCol2"].ToString().Trim();
            lblInfoii.Text = Dt.Rows[0]["InfoCol3"].ToString().Trim();

            chkEngLangArt1.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngCol1"].ToString());
            chkEngLangArt2.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngCol2"].ToString());
            chkEngLangArt3.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngCol3"].ToString());

            chkHistAndSS1.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistCol1"].ToString());
            chkHistAndSS2.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistCol2"].ToString());
            chkHistAndSS3.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistCol3"].ToString());

            chkMathematics1.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathCol1"].ToString());
            chkMathematics2.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathCol2"].ToString());
            chkMathematics3.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathCol3"].ToString());

            chkScienceAndTech1.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechCol1"].ToString());
            chkScienceAndTech2.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechCol2"].ToString());
            chkScienceAndTech3.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechCol3"].ToString());

            chkReading1.Checked = clsGeneral.getChecked(Dt.Rows[0]["ReadCol1"].ToString());
            chkReading2.Checked = clsGeneral.getChecked(Dt.Rows[0]["ReadCol2"].ToString());
            chkReading3.Checked = clsGeneral.getChecked(Dt.Rows[0]["ReadCol3"].ToString());

            chkEngLangArt1.Attributes.Add("onclick", "return false");
            chkEngLangArt2.Attributes.Add("onclick", "return false");
            chkEngLangArt3.Attributes.Add("onclick", "return false");

            chkHistAndSS1.Attributes.Add("onclick", "return false");
            chkHistAndSS2.Attributes.Add("onclick", "return false");
            chkHistAndSS3.Attributes.Add("onclick", "return false");

            chkMathematics1.Attributes.Add("onclick", "return false");
            chkMathematics2.Attributes.Add("onclick", "return false");
            chkMathematics3.Attributes.Add("onclick", "return false");

            chkScienceAndTech1.Attributes.Add("onclick", "return false");
            chkScienceAndTech2.Attributes.Add("onclick", "return false");
            chkScienceAndTech3.Attributes.Add("onclick", "return false");

            chkReading1.Attributes.Add("onclick", "return false");
            chkReading2.Attributes.Add("onclick", "return false");
            chkReading3.Attributes.Add("onclick", "return false");
        }
    }

    protected void FillIEPPage8()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        string strQuery = "SELECT   IEP3.StdtIEPId,  IEP3.AddInfoCol1, IEP3.AddInfoCol2, IEP3.AddInfoCol3, IEP3.AddInfoCol3Desc FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt3 IEP3 "
                         + "ON IEP1.StdtIEPId = IEP3.StdtIEPId WHERE  IEP1.StdtIEPId = '" + Sess.IEPId + "' ";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt.Rows.Count > 0)
        {
            lblAddInfo8.Text = Dt.Rows[0]["AddInfoCol3Desc"].ToString().Trim();

            chkAddInfo1.Checked = clsGeneral.getChecked(Dt.Rows[0]["AddInfoCol1"].ToString());
            chkAddInfo2.Checked = clsGeneral.getChecked(Dt.Rows[0]["AddInfoCol2"].ToString());
            chkAddInfo3.Checked = clsGeneral.getChecked(Dt.Rows[0]["AddInfoCol3"].ToString());

            chkAddInfo1.Attributes.Add("onclick", "return false");
            chkAddInfo2.Attributes.Add("onclick", "return false");
            chkAddInfo3.Attributes.Add("onclick", "return false");
        }
    }

    private void FillIEPPage9()
    {
        int IEPId = 0;
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        string strQuery = "SELECT * FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + "";
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt.Rows.Count > 0)
        {
            IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);
            lblStateOrDistrict.Text = Dt.Rows[0]["PlOneHoursWkPgm"].ToString().Trim();
            lblSignParentOne.Text = Dt.Rows[0]["PlOneSignParent"].ToString().Trim();
            lblSignParentTwo.Text = Dt.Rows[0]["PlTwoSignParent"].ToString().Trim();
            lblLocationService.Text = Dt.Rows[0]["PlOneServiceLocation"].ToString().Trim();
            lblLocationService2.Text = Dt.Rows[0]["PlOneServiceLocation2"].ToString().Trim();
            lblOtherDesc.Text = Dt.Rows[0]["PlTwoOtherDesc"].ToString().Trim();
            if (Dt.Rows[0]["PlOneDate"].ToString().Trim() != null && Dt.Rows[0]["PlOneDate"].ToString().Trim() != "")
                lblDateOne.Text = (Dt.Rows[0]["PlOneDate"].ToString().Trim());
            else
                lblDateOne.Text = "";

            if (Dt.Rows[0]["PlTwoDate"].ToString().Trim() != null && Dt.Rows[0]["PlTwoDate"].ToString().Trim() != "")
                lblDateTwo.Text = (Dt.Rows[0]["PlTwoDate"].ToString().Trim());
            else
                lblDateTwo.Text = "";
            chkEarlyChild.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneEarlyPgm"].ToString()); ;
            chkSeparate.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparatePgm"].ToString()); ;
            chkBoth.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneBothPgm"].ToString()); ;

            chkEnrolled.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneEnrolledPrnt"].ToString()); ;
            chkPlaced.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOnePlcdTeam"].ToString()); ;
            chkTimeMore.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneTimeMore"].ToString()); ;

            chkOfTime.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneTimeTwo"].ToString()); ;
            chk39Time.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneTimeThree"].ToString()); ;
            chkSubstancialy.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparateClass"].ToString()); ;

            chkDaySchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparateDayScl"].ToString()); ;
            chkPublic.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparatePublic"].ToString()); ;
            chkPrivate.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparatePvt"].ToString()); ;

            chkResidential.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneResidentialFacility"].ToString()); ;
            chkHome.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneHome"].ToString()); ;
            chkService.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneServiceLctn"].ToString()); ;

            chkDepartment.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOnePsychiatric"].ToString()); ;
            chkMassachusetts.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneMassachusetts"].ToString()); ;
            chkDay.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneMassachusettsDay"].ToString()); ;

            chkRes.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneMassachusettsRes"].ToString()); ;
            chkHmeBasedPgm.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneDoctorHme"].ToString()); ;
            chkHospital.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneDoctorHsptl"].ToString()); ;

            chkConcent.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneConsent"].ToString()); ;
            chkPlacement.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneRefuse"].ToString()); ;
            chkRefused.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOnePlacement"].ToString()); ;



            chkFullPgm.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoFullInclusionPgm"].ToString()); ;
            chkPartialPgm.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPartialPgm"].ToString()); ;
            chkSepClass.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoSubstantially"].ToString()); ;

            chkSepDaySchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoSeparateScl"].ToString()); ;
            chkPublicSep.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPublicScl"].ToString()); ;
            chkPrivateSep.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPrivateScl"].ToString()); ;

            chkResSchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoResScl"].ToString()); ;
            chkOtherSep.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoOther"].ToString()); ;
            chkDetained.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoYouth"].ToString()); ;

            chkTreatment.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPsychiatric"].ToString()); ;
            chkHospitalSchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoMassachusetts"].ToString()); ;
            chkDayPlace.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoMassachusettsDay"].ToString()); ;
            chkResPlace.Checked = clsGeneral.getChecked(Dt.Rows[0]["PltwoMassachusettsRes"].ToString()); ;


            chkFacility.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoCorrectionFacility"].ToString()); ;
            chkHomeDoctor.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoDoctorHome"].ToString()); ;
            chkHospitalDoctor.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoDoctorHsptl"].ToString()); ;
            chkConsent2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoConsent"].ToString()); ;

            chkRefuse2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPlacement"].ToString()); ;
            chkPlacement2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoFullPgm"].ToString()); ;
        }

        chkPlacement2.Attributes.Add("onclick", "return false");
        chkRefuse2.Attributes.Add("onclick", "return false");
        chkConsent2.Attributes.Add("onclick", "return false");
        chkHospitalDoctor.Attributes.Add("onclick", "return false");

        chkHomeDoctor.Attributes.Add("onclick", "return false");
        chkFacility.Attributes.Add("onclick", "return false");
        chkResPlace.Attributes.Add("onclick", "return false");
        chkDayPlace.Attributes.Add("onclick", "return false");

        chkHospitalSchool.Attributes.Add("onclick", "return false");
        chkTreatment.Attributes.Add("onclick", "return false");
        chkDetained.Attributes.Add("onclick", "return false");
        chkOtherSep.Attributes.Add("onclick", "return false");

        chkResSchool.Attributes.Add("onclick", "return false");
        chkPrivateSep.Attributes.Add("onclick", "return false");
        chkPublicSep.Attributes.Add("onclick", "return false");
        chkSepDaySchool.Attributes.Add("onclick", "return false");

        chkSepClass.Attributes.Add("onclick", "return false");
        chkPartialPgm.Attributes.Add("onclick", "return false");
        chkFullPgm.Attributes.Add("onclick", "return false");
        chkRefused.Attributes.Add("onclick", "return false");

        chkPlacement.Attributes.Add("onclick", "return false");
        chkConcent.Attributes.Add("onclick", "return false");
        chkHospital.Attributes.Add("onclick", "return false");
        chkHmeBasedPgm.Attributes.Add("onclick", "return false");

        chkRes.Attributes.Add("onclick", "return false");
        chkDay.Attributes.Add("onclick", "return false");
        chkMassachusetts.Attributes.Add("onclick", "return false");
        chkDepartment.Attributes.Add("onclick", "return false");

        chkService.Attributes.Add("onclick", "return false");
        chkHome.Attributes.Add("onclick", "return false");
        chkResidential.Attributes.Add("onclick", "return false");
        chkPrivate.Attributes.Add("onclick", "return false");

        chkPublic.Attributes.Add("onclick", "return false");
        chkDaySchool.Attributes.Add("onclick", "return false");
        chkSubstancialy.Attributes.Add("onclick", "return false");
        chk39Time.Attributes.Add("onclick", "return false");

        chkOfTime.Attributes.Add("onclick", "return false");
        chkTimeMore.Attributes.Add("onclick", "return false");
        chkPlaced.Attributes.Add("onclick", "return false");
        chkEnrolled.Attributes.Add("onclick", "return false");

        chkBoth.Attributes.Add("onclick", "return false");
        chkSeparate.Attributes.Add("onclick", "return false");
        chkEarlyChild.Attributes.Add("onclick", "return false");
        lblDateTwo.Attributes.Add("onclick", "return false");

        lblDateTwo.Attributes.Add("onclick", "return false");
        lblStateOrDistrict.Attributes.Add("onclick", "return false");
        lblSignParentOne.Attributes.Add("onclick", "return false");
        lblSignParentTwo.Attributes.Add("onclick", "return false");
        lblOtherDesc.Attributes.Add("onclick", "return false");

    }

    private void FillIEPPage10()
    {
        int IEPId = 0;
        Sess = (clsSession)Session["UserSession"];
        System.Data.DataTable Dt1 = null;
        System.Data.DataTable Dt2 = null;
        System.Data.DataTable Dt3 = null;
        System.Data.DataTable Dt4 = null;
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        TimeSpan tempDatetime;

        string strQuery = "SELECT   IEP4.StdtIEPId,IEP4.LanguageofInst,IEP4.RoleDesc,IEP4.ActonOwnBehalfCk, IEP4.CourtAppGrdCk, IEP4.SharedDecMakingCk,IEP4.DelegateDeciMakCk, IEP4.CourtAppGuardian, IEP4.PrLanguageGrd1, IEP4.PrLanguageGrd2, IEP4.DateOfMeeting, IEP4.TypeOfMeeting,IEP4.AnnualReviewMeeting, IEP4.ReevaluationMeeting, IEP4.CostSharedPnt, IEP4.SpecifyAgency,IEP4.SchoolName,IEP4.SchAddress,IEP4.SchContact,IEP4.SchoolPhone,IEP4.SchTelephone FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + " ";
        Dt = objData.ReturnDataTable(strQuery, false);
        string strQuery1 = "Select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS, ST.DOB,ST.GradeLevel,ADR.ApartmentType,ADR.StreetName,ADR.City, ST.SchoolId,ST.SASID,ST.Gender,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) AS Phone,"
            + "(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=2)) AS Mobile from Student ST Inner Join StudentAddresRel SAR ON ST.StudentId=SAR.StudentPersonalId "
            + "Inner Join AddressList ADR ON SAR.AddressId=ADR.AddressId Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + " And SAR.ContactSequence=0";
        Dt1 = objData.ReturnDataTable(strQuery1, false);
        string sQuery = "SELECT sch.SchoolId,sch.SchoolName As SName,sch.SchoolDesc,sch.DistrictName,sch.DistContact As cont,sch.DistPhone As Phone ,Adr.AddressLine1,Adr.AddressLine2,Adr.AddressLine3 As ScAdd,sch.DistContact +','+ sch.DistPhone As DistCon " +
                              " from School Sch " +
                                "   LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State) " +
                                           "  ON Sch.DistAddrId = Adr.AddressId  WHERE sch.ActiveInd = 'A'  And sch.SchoolId=" + sess.SchoolId + " ";
        Dt2 = objData.ReturnDataTable(sQuery, false);
        if (Dt.Rows.Count > 0)
        {
            IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);
            //txtCourtAppointed.InnerHtml = Dt.Rows[0]["CourtAppGuardian"].ToString().Trim();
            lblInstruction.Text = Dt.Rows[0]["LanguageofInst"].ToString().Trim();
            chkActingon.Checked = clsGeneral.getChecked(Dt.Rows[0]["ActonOwnBehalfCk"].ToString());
            chkCourtAppointed.Checked = clsGeneral.getChecked(Dt.Rows[0]["CourtAppGrdCk"].ToString());
            chkDecisionMaking.Checked = clsGeneral.getChecked(Dt.Rows[0]["SharedDecMakingCk"].ToString());
            ChkDelegateDecision.Checked = clsGeneral.getChecked(Dt.Rows[0]["DelegateDeciMakCk"].ToString());
            lblCourtAppointed.Text = Dt.Rows[0]["CourtAppGuardian"].ToString().Trim();
            lblPrLanguageParent.Text = Dt.Rows[0]["PrLanguageGrd1"].ToString().Trim();
            lblPrLanguageParent2.Text = Dt.Rows[0]["PrLanguageGrd2"].ToString().Trim();
            if (Dt.Rows[0]["DateOfMeeting"].ToString().Trim() != null && Dt.Rows[0]["DateOfMeeting"].ToString().Trim() != "")
                lblMeetingDate.Text = (Dt.Rows[0]["DateOfMeeting"].ToString().Trim());
            else
                lblMeetingDate.Text = "";
            lblMeetingType.Text = Dt.Rows[0]["TypeOfMeeting"].ToString().Trim();
            lblAnnualReview.Text = Dt.Rows[0]["AnnualReviewMeeting"].ToString().Trim();
            lblReevaluation.Text = Dt.Rows[0]["ReevaluationMeeting"].ToString().Trim();
            //   RadioButtonList1.Text = Dt.Rows[0]["CostSharedPnt"].ToString().Trim();
            lblAgency.Text = Dt.Rows[0]["SpecifyAgency"].ToString().Trim();
            //string role = Dt.Rows[0]["Role"].ToString().Trim();
            lblRole.Text = Dt.Rows[0]["RoleDesc"].ToString().Trim();

            //lblSchoolName.Text = Dt.Rows[0]["SchoolName"].ToString().Trim();

            //lblSchoolPhone.Text = Dt.Rows[0]["SchoolPhone"].ToString().Trim();
            //lblSchAddress.Text = Dt.Rows[0]["SchAddress"].ToString().Trim();
            //lblSchContact.Text = Dt.Rows[0]["SchContact"].ToString().Trim();
            //lblSchTelephone.Text = Dt.Rows[0]["SchTelephone"].ToString().Trim();
            string Query = "SELECT IEP4.SchoolName,IEP4.SchoolPhone,IEP4.SchAddress,IEP4.SchContact,IEP4.SchTelephone  FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE IEP4.SchoolName IS NOT NULL and IEP1.StdtIEPId = " + sess.IEPId + " ";
            Dt3 = objData.ReturnDataTable(Query, false);
            string stQuery = "SELECT sch.SchoolName As SName,sch.DistContact As cont,sch.DistPhone As Phone  ,Adr.AddressLine1,Adr.AddressLine2,Adr.AddressLine3 As ScAdd,sch.DistPhone As DistCon  from School Sch  LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State)  ON Sch.DistAddrId = Adr.AddressId  WHERE sch.ActiveInd = 'A'  And sch.SchoolId=" + sess.SchoolId + " ";
            Dt4 = objData.ReturnDataTable(stQuery, false);
            if (Dt3 != null)
            {
                if (Dt3.Rows.Count > 0)
                {
                    lblSchoolName.Text = Dt3.Rows[0]["SchoolName"].ToString().Trim();
                    lblSchoolPhone.Text = Dt3.Rows[0]["SchoolPhone"].ToString().Trim();
                    lblSchAddress.Text = Dt3.Rows[0]["SchAddress"].ToString().Trim();
                    lblSchContact.Text = Dt3.Rows[0]["SchContact"].ToString().Trim();
                    lblSchTelephone.Text = Dt3.Rows[0]["SchTelephone"].ToString().Trim();
                }
                else
                {
                    lblSchoolName.Text = Dt4.Rows[0]["SName"].ToString().Trim();
                    lblSchoolPhone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                    lblSchAddress.Text = Dt4.Rows[0]["ScAdd"].ToString().Trim();
                    lblSchContact.Text = Dt4.Rows[0]["cont"].ToString().Trim();
                    lblSchTelephone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                }
            }
            else
            {
                lblSchoolName.Text = Dt4.Rows[0]["SName"].ToString().Trim();
                lblSchoolPhone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                lblSchAddress.Text = Dt4.Rows[0]["ScAdd"].ToString().Trim();
                lblSchContact.Text = Dt4.Rows[0]["cont"].ToString().Trim();
                lblSchTelephone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
            }
            //string Qry = "SELECT RL.RoleCode AS RoleCode FROM Role RL WHERE RL.RoleId=" + role + "";
            //Dt2 = objData.ReturnDataTable(Qry, false);
            // if (Dt2 != null)
            //{
            // if (Dt2.Rows.Count > 0)
            // {
            // lblRole.Text = Dt2.Rows[0]["RoleCode"].ToString().Trim();
            // }
            //}
            //Dt2 = null;


            if (Convert.ToString(Dt.Rows[0]["CostSharedPnt"]) == "True")
            {
                chkYes.Checked = clsGeneral.getChecked(Dt.Rows[0]["CostSharedPnt"].ToString());
            }
            else if (Convert.ToString(Dt.Rows[0]["CostSharedPnt"]) == "False")
            {
                chkNo.Checked = clsGeneral.getChecked(Dt.Rows[0]["CostSharedPnt"].ToString());
            }




        }
        if (Dt1.Rows.Count > 0)
        {
            lblPhoneHome.Text = Dt1.Rows[0]["Phone"].ToString().Trim();
            lblStudentSchIep10.Text = Dt1.Rows[0]["SchoolId"].ToString().Trim();
            lblStudentNameIep10.Text = Dt1.Rows[0]["StudentName"].ToString().Trim();
            lblsasidIep10.Text = Dt1.Rows[0]["SASID"].ToString().Trim();
            string gender = Dt1.Rows[0]["Gender"].ToString().Trim();
            if (gender == "0")
                lblSex.Text = " ";
            else if (gender == "1")
                lblSex.Text = "Male";
            else if (gender == "2")
                lblSex.Text = "Female";

            lblBirthDate.Text = Dt1.Rows[0]["DOBS"].ToString().Trim();
            tempDatetime = DateTime.Now - Convert.ToDateTime(Dt1.Rows[0]["DOB"].ToString().Trim());
            double dats = tempDatetime.TotalDays;
            int age = Convert.ToInt32(dats / 360);
            if (age > 0)
            {
                lblAge.Text = age.ToString();
            }
            else lblAge.Text = "";
            lblGrade.Text = Dt1.Rows[0]["GradeLevel"].ToString().Trim();
            lblAddress.Text = Dt1.Rows[0]["ApartmentType"].ToString().Trim() + "</br>" + Dt1.Rows[0]["StreetName"].ToString().Trim() + "</br>" + Dt1.Rows[0]["City"].ToString().Trim();
        }
        string strQuery2 = "Select distinct AL.AddressLine1+','+AL.AddressLine2+','+AL.AddressLine3 AS PrimaryContactAddress,CP.LastName+','+CP.FirstName As GuardianName,SP.PlaceOfBirth As PB,SP.PrimaryLanguage As PrLang,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone,AL.Mobile AS CellPhone,AL.PrimaryEmail AS Email from AddressList AL"
                        + " Inner Join StudentAddresRel ADR on AL.AddressId=ADR.AddressId"
                        + " Inner Join StudentPersonal SP on SP.StudentPersonalId=ADR.StudentPersonalId"
                        + " Inner join ContactPersonal CP on CP.StudentPersonalId=SP.StudentPersonalId"
                        + " Inner join StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
                        + " Inner join LookUp LK on LK.LookupId=SCR.RelationshipId"
                       + " where ADR.ContactSequence=1  AND SP.StudentPersonalId=" + sess.StudentId + " And SP.StudentType='Client' "
                        + " And SP.SchoolId=" + sess.SchoolId + "  And LK.LookupName='Legal Guardian 1' AND CP.Status=1";
        Dt2 = objData.ReturnDataTable(strQuery2, false);
        if (Dt2 != null)
        {
            if (Dt2.Rows.Count > 0)
            {
                lblPlace.Text = Dt2.Rows[0]["PB"].ToString().Trim();
                lblPrLanguageParent.Text = Dt2.Rows[0]["PrLang"].ToString().Trim();
                lblName.Text = Dt2.Rows[0]["GuardianName"].ToString().Trim();
                lblRelationship.Text = "Legal Guardian 1";
                lblstudAddress.Text = Dt2.Rows[0]["PrimaryContactAddress"].ToString().Trim();
                lblstudPhoneHome.Text = Dt2.Rows[0]["HomePhone"].ToString().Trim();
                lblstudPhoneOther.Text = Dt2.Rows[0]["WorkPhone"].ToString().Trim();

            }

        }

        strQuery2 = "Select distinct AL.AddressLine1+','+AL.AddressLine2+','+AL.AddressLine3 AS PrimaryContactAddress,CP.LastName+','+CP.FirstName As GuardianName,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone,AL.Mobile AS CellPhone,AL.PrimaryEmail AS Email from AddressList AL"
                        + " Inner Join StudentAddresRel ADR on AL.AddressId=ADR.AddressId"
                        + " Inner Join StudentPersonal SP on SP.StudentPersonalId=ADR.StudentPersonalId"
                        + " Inner join ContactPersonal CP on CP.StudentPersonalId=SP.StudentPersonalId"
                        + " Inner join StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
                        + " Inner join LookUp LK on LK.LookupId=SCR.RelationshipId"
                       + "  where ADR.ContactSequence=1  AND SP.StudentPersonalId=" + sess.StudentId + " And SP.StudentType='Client' "
                        + " And SP.SchoolId=" + sess.SchoolId + "  And LK.LookupName='Legal Guardian 2' AND CP.Status=1";
        Dt3 = objData.ReturnDataTable(strQuery2, false);
        if (Dt3 != null)
        {
            if (Dt3.Rows.Count > 0)
            {
                lblName2.Text = Dt3.Rows[0]["GuardianName"].ToString().Trim();
                lblRelationship2.Text = "Legal Guardian 2";
                lblstudAddress2.Text = Dt3.Rows[0]["PrimaryContactAddress"].ToString().Trim();
                lblstudPhoneHome2.Text = Dt3.Rows[0]["HomePhone"].ToString().Trim();
                lblstudPhoneOther2.Text = Dt3.Rows[0]["WorkPhone"].ToString().Trim();

            }

        }

        string strQuery4 = "SELECT sch.SchoolId,sch.SchoolName As SName,sch.SchoolDesc,sch.DistrictName,sch.DistContact As cont,sch.DistPhone As Phone ,Adr.AddressLine1,Adr.AddressLine2,Adr.AddressLine3 As ScAdd,sch.DistContact +','+ sch.DistPhone As DistCon " +
                               " from School Sch " +
                                 "   LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State) " +
                                            "  ON Sch.DistAddrId = Adr.AddressId  WHERE sch.ActiveInd = 'A'  And sch.SchoolId=" + sess.SchoolId + " ";
        Dt4 = objData.ReturnDataTable(strQuery4, false);
        if (Dt4 != null)
        {
            if (Dt4.Rows.Count > 0)
            {
                // lblSchName.Text = Dt.Rows[0]["DistrictName"].ToString().Trim();
                //lblSchAdd.Text = Dt.Rows[0]["ScAdd"].ToString().Trim();
                // lblSchoolPhone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                //lblSchCon.Text = Dt.Rows[0]["DistCon"].ToString().Trim();
                // lblSchoolName.Text = Dt4.Rows[0]["SName"].ToString().Trim();
                // lblSchContact.Text = Dt4.Rows[0]["cont"].ToString().Trim();
                // lblSchTelephone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                // lblSchAddress.Text = Dt4.Rows[0]["ScAdd"].ToString().Trim();

            }

        }
        lblInstruction.Attributes.Add("onclick", "return false");
        chkActingon.Attributes.Add("onclick", "return false");
        chkCourtAppointed.Attributes.Add("onclick", "return false");
        chkDecisionMaking.Attributes.Add("onclick", "return false");
        ChkDelegateDecision.Attributes.Add("onclick", "return false");

        lblCourtAppointed.Attributes.Add("onclick", "return false");
        lblPrLanguageParent.Attributes.Add("onclick", "return false");
        lblPrLanguageParent2.Attributes.Add("onclick", "return false");
        lblMeetingDate.Attributes.Add("onclick", "return false");

        lblMeetingType.Attributes.Add("onclick", "return false");
        lblAnnualReview.Attributes.Add("onclick", "return false");
        lblReevaluation.Attributes.Add("onclick", "return false");
        lblAgency.Attributes.Add("onclick", "return false");

        lblRole.Attributes.Add("onclick", "return false");
        chkYes.Attributes.Add("onclick", "return false");
        chkNo.Attributes.Add("onclick", "return false");


    }
    private void FillIEPPage11()
    {
        sess = (clsSession)Session["UserSession"];
        System.Data.DataTable Dt1 = null;
        System.Data.DataTable Dt2 = null;
        string strQuery1 = "";
        string strQuery = "";
        string strQuery2 = "";

        if (sess != null)
        {
            strQuery1 = "SELECT IEP5.TMName,IEP5.TMRole,IEP5.InitialIfInAttn  FROM StdtIEPExt5 IEP5 WHERE IEP5.StdtIEPId=" + sess.IEPId + "";
            strQuery = "SELECT StudentPersonal.StudentPersonalId, StudentPersonal.LastName, StudentPersonal.BirthDate, ";
            strQuery += "StdtIEPExt4.PoMEliDeter, StdtIEPExt4.PoMIEPDev, StdtIEPExt4.PoMPlacement, StdtIEPExt4.PoMInitEval, StdtIEPExt4.PoMInit, StdtIEPExt4.PoMReeval, StdtIEPExt4.PoMAnnRev, StdtIEPExt4.PoMOtherCheck, StdtIEPExt4.PoMOtherText, StdtIEPExt4.AtndDate ";
            strQuery += "FROM StudentPersonal JOIN StdtIEP ON StudentPersonal.StudentPersonalId=StdtIEP.StudentId JOIN StdtIEPExt4 ON StdtIEP.StdtIEPId=StdtIEPExt4.StdtIEPId WHERE StdtIEP.SchoolId=" + sess.SchoolId + " AND StdtIEP.StudentId=" + sess.StudentId + " AND StdtIEP.StdtIEPId=" + sess.IEPId;

            strQuery2 = "SELECT StudentPersonal.StudentPersonalId, StudentPersonal.LastName, StudentPersonal.FirstName, StudentPersonal.BirthDate, StudentPersonal.AdmissionDate FROM StudentPersonal JOIN StdtIEP ON StudentPersonal.StudentPersonalId=StdtIEP.StudentId WHERE StdtIEP.SchoolId=" + sess.SchoolId + " AND StdtIEP.StudentId=" + sess.StudentId + " AND StdtIEP.StdtIEPId=" + sess.IEPId;
        }
        objData.Execute(strQuery1);
        Dt1 = objData.ReturnDataTable(strQuery1, false);
        Dt = objData.ReturnDataTable(strQuery, false);
        Dt2 = objData.ReturnDataTable(strQuery2, false);
        RepIep10.DataSource = Dt1;
        RepIep10.DataBind();
        if (Dt2.Rows.Count > 0)
        {
            //if (Dt2.Rows[0]["AdmissionDate"].ToString().Trim() != null && Dt2.Rows[0]["AdmissionDate"].ToString().Trim() != "")
            //  lblDate.Text = DateTime.Parse(Dt2.Rows[0]["AdmissionDate"].ToString().Trim()).ToString("MM/dd/yyyy");
            //  else
            //   lblDate.Text = "";
            if (Dt2.Rows[0]["BirthDate"].ToString().Trim() != null && Dt2.Rows[0]["BirthDate"].ToString().Trim() != "")
                lblDOB.Text = DateTime.Parse(Dt2.Rows[0]["BirthDate"].ToString().Trim()).ToString("MM/dd/yyyy");
            else
                lblDOB.Text = "";
            lblStudentName.Text = Dt2.Rows[0]["FirstName"].ToString().Trim() + " " + Dt2.Rows[0]["LastName"].ToString().Trim();
            lblID.Text = Dt2.Rows[0]["StudentPersonalId"].ToString().Trim();
        }
        if (Dt.Rows.Count > 0)
        {
            chkEliDeter.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMEliDeter"].ToString());
            chkIEPDev.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMIEPDev"].ToString());
            chkPlacementData.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMPlacement"].ToString());
            chkInitEval.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMInitEval"].ToString());
            chkInit.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMInit"].ToString());
            chkReeval.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMReeval"].ToString());
            chkAnnRev.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMAnnRev"].ToString());
            chkOther.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMOtherCheck"].ToString());
            lblOther.Text = Dt.Rows[0]["PoMOtherText"].ToString().Trim();
            //lblDate.Text = DateTime.Parse(Dt.Rows[0]["AtndDate"].ToString().Trim());

            //lblDate.Text = DateTime.Parse(Dt.Rows[0]["AtndDate"].ToString().Trim());
        }
        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["AtndDate"].ToString().Trim() != "" && Dt.Rows[0]["AtndDate"].ToString().Trim() != null)
            {
                lblDate.Text = DateTime.Parse(Dt.Rows[0]["AtndDate"].ToString().Trim()).ToString("MM/dd/yyyy");
            }
            else
                lblDate.Text = " ";
        }
        chkEliDeter.Attributes.Add("onclick", "return false");
        chkIEPDev.Attributes.Add("onclick", "return false");
        chkPlacementData.Attributes.Add("onclick", "return false");
        chkInitEval.Attributes.Add("onclick", "return false");
        chkInit.Attributes.Add("onclick", "return false");
        chkReeval.Attributes.Add("onclick", "return false");
        chkAnnRev.Attributes.Add("onclick", "return false");
        chkOther.Attributes.Add("onclick", "return false");
    }
    private void FillIEPPage12()
    {
        objData = new clsData();
        string strQuery = "SELECT   IEP4.StdtIEPId,  IEP4.SigRoleLEARep, IEP4.SigRep_date, IEP4.ParntAccptIEP, IEP4.ParntRejctIEP,IEP4.ParntDontRejctIEP, IEP4.ParntDontRejctDesc, IEP4.ParntReqMeetig, IEP4.SigParnt,IEP4.SigParnt_date,IEP4.ParntComnt FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + " ";
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt.Rows.Count > 0)
        {
            int IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);


            lblSigRep.Text = Dt.Rows[0]["SigRoleLEARep"].ToString().Trim();
            if (Dt.Rows[0]["SigRep_date"].ToString().Trim() != null && Dt.Rows[0]["SigRep_date"].ToString().Trim() != "")
                lblSigRepDate.Text = (Dt.Rows[0]["SigRep_date"].ToString().Trim());
            else
                lblSigRepDate.Text = "";
            acceptIepDeveloped.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntAccptIEP"].ToString());
            rejectIepDeveloped.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntRejctIEP"].ToString());
            deleteFollowingPortions.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntDontRejctIEP"].ToString());
            lblRejectedPortions.Text = Dt.Rows[0]["ParntDontRejctDesc"].ToString().Trim();
            RejectionMeeting.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntReqMeetig"].ToString());
            lblSigPrnt.Text = Dt.Rows[0]["SigParnt"].ToString().Trim();
            if (Dt.Rows[0]["SigParnt_date"].ToString().Trim() != null && Dt.Rows[0]["SigParnt_date"].ToString().Trim() != "")
                lbldteSigPrnt.Text = (Dt.Rows[0]["SigParnt_date"].ToString().Trim());
            else
                lbldteSigPrnt.Text = "";

            lblParentComment.Text = Dt.Rows[0]["ParntComnt"].ToString().Trim();
        }
        lblSigRep.Attributes.Add("onclick", "return false");
        lblSigRepDate.Attributes.Add("onclick", "return false");
        acceptIepDeveloped.Attributes.Add("onclick", "return false");
        rejectIepDeveloped.Attributes.Add("onclick", "return false");

        deleteFollowingPortions.Attributes.Add("onclick", "return false");
        lblRejectedPortions.Attributes.Add("onclick", "return false");
        RejectionMeeting.Attributes.Add("onclick", "return false");
        lblSigPrnt.Attributes.Add("onclick", "return false");
        lbldteSigPrnt.Attributes.Add("onclick", "return false");

    }

    protected void btn_edit_Click(object sender, ImageClickEventArgs e)
    {
        //Sess = (clsSession)Session["UserSession"];
        Response.Redirect("~/Administration/IEPDocumentaion.aspx");
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }

    protected System.Data.DataTable getGoalData4()
    {
        //Page Number 4 
        System.Data.DataTable Dt = null;
        objData = new clsData();

        Dt = new System.Data.DataTable();
        string strQuery = "select distinct  Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId=" + sess.IEPId + " and StdtLessonPlan.IncludeIEP=1";
        Dt = objData.ReturnDataTable(strQuery, false);
        string GoalIdZ = "";
        int countForGoalId = 0;
        foreach (DataRow dr in Dt.Rows)
        {
            if (countForGoalId == 0)
            {
                GoalIdZ += dr["Id"].ToString();
            }
            else if (countForGoalId > 0)
            {
                GoalIdZ += "," + dr["Id"].ToString();
            }
            countForGoalId++;
        }

        if (Dt.Rows.Count == 0)
        {
            GoalIdZ = "0";
        }

        strQuery = "SELECT    dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName,(SELECT StdtGoalId FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + sess.IEPId + ") StdtGoalId, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT IEPGoalNo FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + sess.IEPId + ") IEPGoalNo,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, dbo.Goal.GoalName   FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'    ORDER BY dbo.StdtLessonPlan.GoalId ";


        Dt = objData.ReturnDataTable(strQuery, false);


        System.Data.DataTable dtRep = Dt;
        if (dtRep != null)
        {
            dtRep.Columns.Remove("StudentId");
            dtRep.Columns.Remove("StdtGoalId");
            dtRep.Columns.Remove("AsmntYearId");
        }
        //  dtRep.Columns.Remove("GoalId");
        return dtRep;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

        btnExport.Enabled = false;
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        try
        {

            if (objData.IFExists("Select * from binaryFiles  Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And IEPId=" + sess.IEPId + " And Type='BW' And ModuleName='IEP'") == false)
            {
                ExportAll();
            }
            string fileName = "", contentType = "";
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            string strQuery = "Select Data,ContentType,DocumentName from binaryFiles Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And IEPId=" + sess.IEPId + " And Type='BW'  And ModuleName='IEP' ORDER BY BinaryId DESC";
            byte[] bytes = objBinary.getDocument(strQuery, out contentType, out fileName);
            objBinary.ShowDocument(fileName, bytes, contentType);


        }
        catch (Exception Ex)
        {
            // throw Ex;
        }
        finally
        {
        }

        btnExport.Enabled = true;
    }

    private void AllInOne()
    {
    }


    [STAThread]
    public static void ConvertHTMLTOWORD(string url)
    {

        var file = new FileInfo(url);
        Microsoft.Office.Interop.Word.Application app
            = new Microsoft.Office.Interop.Word.Application();
        try
        {
            app.Visible = true;
            object missing = Missing.Value;
            object visible = true;
            _Document doc = app.Documents.Add(ref missing,
                     ref missing,
                     ref missing,
                     ref visible);
            var bookMark = doc.Words.First.Bookmarks.Add("entry");
            bookMark.Range.InsertFile(file.FullName);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            app.Quit();
        }
    }



    public void downloadfile()
    {
        try
        {
            string FileName = ViewState["FileName"].ToString();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");
            byte[] data = req.DownloadData(FileName);
            response.BinaryWrite(data);
            ClientScript.RegisterStartupScript(GetType(), "", "HideWait();", true);
            response.End();
        }
        catch (Exception ex)
        {

        }
        ViewState["FileName"] = "";
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

            string path = Server.MapPath("~\\Administration") + "\\IEPTemp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid g;

            g = Guid.NewGuid();

            sess = (clsSession)Session["UserSession"];
            string newpath = path + "\\";
            string ids = g.ToString();
            ids = ids.Replace("-", "");

            string newFileName = "IEPTemporyTemplate" + ids.ToString();
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
            tdMsgExport.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
        }
    }

    static int xmlcolumn = 0;

    private void CreateQuery(string StateName, string Path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Path);

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");
        checkCount = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                columns = new string[xmlListColumns.Count];
                placeHolders = new string[xmlListColumns.Count];
                xmlcolumn += xmlListColumns.Count;



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

    private void ExportToWord(string htmlData, string newPath)
    {
        try
        {
            StringBuilder strBody = new StringBuilder();
            strBody.Append(htmlData);
            Response.AppendHeader("Content-Type", "application/msword");
            Response.AppendHeader("Content-disposition", "attachment; filename=IEPDocument.doc");
            Response.Write(strBody);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string replaceWithTexts(string HtmlData, string[] plcT, string[] TextT)
    {
        int count = plcT.Count();

        for (int i = 0; i < count; i++)
        {
            if (TextT[i] != null)
            {
                HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
            }
            else
            {
                HtmlData = HtmlData.Replace(plcT[i], "");
            }
        }
        return HtmlData;
    }

    public static void PageConvert(string input, string output, WdSaveFormat format)
    {
        try
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object bConfirmDialog = false;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = format;
            object oFileShare = true;

            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref bConfirmDialog, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();

            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

            using (var fs = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Close();
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }




    }

    public static String URLTOHTML(string Url)
    {
        string result = "";
        try
        {

            using (StreamReader reader = new StreamReader(Url))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }
        return result;

    }

    public void ExportAll()
    {
        Hashtable ht = new Hashtable();
        clsDocumentasBinary objBinary = new clsDocumentasBinary();
        clsData objData = new clsData();
        DataClass oData = new DataClass();

        ht = bindData();
        string[] plcT, TextT, plcC, chkC;

        string[] totChkBox = new string[61];

        string Path = Server.MapPath("~\\Administration\\IEPTemplates\\NAIEPTemplate.docx");
        string TemporyPath = Server.MapPath("~\\Administration\\IEPTemp\\");

        string NewPath = CopyTemplate(Path, "0");
        System.Threading.Thread.Sleep(3000);

        int x = 0, count = 0, lastCount = 0;

        for (int k = 1; k < 9; k++)
        {
            CreateQuery1("NE", "..\\Administration\\XMlIEPS\\IEPCreations" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
            chkC.CopyTo(totChkBox, count);
            count += lastCount;
        }

        setCheckBox(NewPath, ht, totChkBox);

        sess = (clsSession)Session["UserSession"];

        string HtmlFileName = "";
        string HtmlData = objBinary.ConvertToHtml(NewPath, TemporyPath, out HtmlFileName);

        System.Threading.Thread.Sleep(3000);

        string Temp = Server.MapPath("~\\Administration") + "\\XMlIEPS\\";

        string[] filePaths = Directory.GetFiles(Temp);
        string[] totCkeckColums = new string[0];

        for (int k = 1; k < 9; k++)
        {
            if (k != 4)
            {
                CreateQuery1("NE", "..\\Administration\\XMlIEPS\\IEPCreations" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                HtmlData = replaceWithTexts(HtmlData, plcT, TextT);
            }
        }

        System.Data.DataTable DTS = getGoalData4();
        clsGetGoalData objGoals = new clsGetGoalData();
        string goalHtml4 = "";

        if (DTS != null)
        {
            goalHtml4 = objGoals.getGoals(DTS, sess.IEPId, sess.StudentId);
            HtmlData = HtmlData.Replace("plcGoalSession", goalHtml4);
        }
        else
        {
            HtmlData = HtmlData.Replace("plcGoalSession", "");
        }
        sess = (clsSession)Session["UserSession"];

        string strquery = "";
        string fileName = "";

        strquery = "Select 'IEP Doc :'+ S.StudentLname +'-'+AY.AsmntYearDesc+'-V'+IE.Version from StdtIEP IE Inner Join AsmntYear AY on IE.AsmntYearId=AY.AsmntYearId INNER JOIN Student S ON IE.StudentId =S.StudentId  Where IE.StdtIEPId=" + sess.IEPId + " And IE.StudentId=" + sess.StudentId + " And IE.SchoolId=" + sess.SchoolId + " ";
        fileName = Convert.ToString(objData.FetchValue(strquery));
        fileName = fileName + ".doc";

        strquery = "select version from stdtiep where  schoolid=" + sess.SchoolId + " and studentid=" + sess.StudentId + " and stdtiepid=" + sess.IEPId + " ";
        object ver = objData.FetchValue(strquery);
        string ver1 = "";
        if (ver != null) ver1 = Convert.ToString(ver);
        if (ver1 == "") ver1 = "0";
        byte[] contents = objBinary.ConvertToByte(HtmlData, TemporyPath, HtmlFileName);
        System.Threading.Thread.Sleep(3000);
        strquery = "select lookupid from LookUp where LookupName='IEP' ";
        object lookupId = objData.FetchValue(strquery);
        int lookupId1 = Convert.ToInt32(lookupId);
        int docId = objBinary.saveDoc(sess.SchoolId, sess.StudentId, sess.LoginId, lookupId1);
        //     int iepId = oData.ExecuteScalar("SELECT StdtIEPId FROM StdtIEP WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " AND StatusId=(SELECT LOOKUPID FROM LookUp WHERE LookupName='Approved' AND LookupType='IEP Status')");
        int binaryid = objBinary.saveDocument(contents, fileName, ver1, "BW", docId, sess.IEPId, "IEP", sess.SchoolId, sess.StudentId, sess.LoginId);
        // objBinary.ShowDocument(fileName, contents, "application/msword");


    }


    public void WorkThreadFunction()
    {
        try
        {

            Thread.Sleep(5000);
        }
        catch (Exception ex)
        {
            // log errors
        }
    }

    private Hashtable bindData()
    {
        Hashtable htColumns = new Hashtable();
        sess = (clsSession)Session["UserSession"];
        clsExport objExport = new clsExport();
        string[] data;
        string[] data2;

        objExport.getIEP1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(1))
        {
            htColumns.Add(1, data2);
            data2 = null;
        }
        objExport.getIEP2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(2))
        {
            htColumns.Add(2, data2);
            data2 = null;
        }
        objExport.getIEP3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (!htColumns.ContainsKey(3))
        {
            htColumns.Add(3, data2);
            data2 = null;
        }

        System.Data.DataTable dtIEP4 = new System.Data.DataTable();
        bool Odd = false;
        dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);

        if (dtIEP4 != null)
        {
            int RowsCount = dtIEP4.Rows.Count;
            dtIEP4.TableName = "Table";

            for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
            {
                System.Data.DataTable Dt4 = new System.Data.DataTable();
                Dt4 = objExport.ReturnRows(Round, dtIEP4);
                objExport.getIEP4(out data, Dt4);
            }
        }
        if (!htColumns.ContainsKey(4))
        {
            htColumns.Add(4, data2);
            data2 = null;
        }
        objExport.getIEP5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(5))
        {
            htColumns.Add(5, data2);
            data2 = null;
        }

        objExport.getIEP6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (!htColumns.ContainsKey(6))
        {
            htColumns.Add(6, data2);
            data2 = null;
        }

        objExport.getIEP7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (!htColumns.ContainsKey(7))
        {
            htColumns.Add(7, data2);
            data2 = null;
        }
        objExport.getIEP8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(8))
        {
            htColumns.Add(8, data2);
            data2 = null;
        }
        return htColumns;
    }

    private string[] getCheckBoxes(string StateName, string Path, int PageNo, string[] chkData)
    {
        string[] chkC = new string[0];

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");

        checkCount = 0;

        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                int chkCount = 0;

                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        chkCount++;
                    }
                }

                chkC = new string[chkCount];

                columns = getColumns(PageNo, chkCount + 1);

                int j = 0, l = 0;


                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        checkCount++;

                        chkC[j] = columns[l];
                        j++;
                        //l++;
                    }
                    else
                    {
                        j++;
                        //l++;
                    }
                }
            }
        }
        Array.Copy(chkC, chkData, chkC.Length);
        //return chkData.Join<Array>(chkC[j], chkData);

        return chkData;
    }

    private void CreateQuery1(string StateName, string Path, int PageNo, out string[] plcT, out string[] TextT, out string[] plcC, out string[] chkC, bool check, out int lastValue)
    {



        lastValue = 0;
        chkC = new string[0];
        plcC = new string[0];

        TextT = new string[0];
        plcT = new string[0];

        if (PageNo == 4) return;
        string[] textValues;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");
        checkCount = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                int chkCount = 0, textCount = 0;

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



                chkC = new string[chkCount];
                plcC = new string[chkCount];

                TextT = new string[textCount];
                plcT = new string[textCount];

                columns = getColumns(PageNo, textCount);


                int j = 0, k = 0, l = 0;

                if (check == true)
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                            chkC[j] = columns[l];
                            plcC[j] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            j++;
                            lastValue++;
                        }
                        else
                        {
                            TextT[k] = columns[l];
                            plcT[k] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            k++;
                        }
                        l++;
                    }
                }
                else
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                            chkC[j] = columns[l];
                            plcC[j] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            j++;
                        }
                        else
                        {
                            j++;
                        }


                    }
                }
                columns = null;
            }
        }

    }

    private string[] getColumns(int PageNo, int Count)
    {
        sess = (clsSession)Session["UserSession"];
        clsExport objExport = new clsExport();
        string[] data = new string[Count];
        string[] data2 = new string[2];
        string temp = "";
        int counter = 0;
        if (PageNo == 1) objExport.getIEP1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 2) objExport.getIEP2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 3) objExport.getIEP3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (PageNo == 4)
        {
            System.Data.DataTable dtIEP4 = new System.Data.DataTable();
            bool Odd = false;

            dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);

            if (dtIEP4 != null)
            {
                int RowsCount = dtIEP4.Rows.Count;
                dtIEP4.TableName = "Table";

                for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
                {
                    System.Data.DataTable Dt4 = new System.Data.DataTable();
                    Dt4 = objExport.ReturnRows(Round, dtIEP4);
                    objExport.getIEP4(out data, Dt4);

                }
            }

        }

        if (PageNo == 5) objExport.getIEP5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 6) objExport.getIEP6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (PageNo == 7) objExport.getIEP7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 8) objExport.getIEP8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        return data;
    }

    public void SearchAndReplace(string document)
    {
        int m = 0;


        string col = "";
        string plc = "";

        columnsCheck = new string[checkCount];

        for (int i = 0; i < columns.Length; i++)
        {
            plc = placeHolders[i].ToString().Trim();
            col = columns[i].ToString().Trim();


            if (plc == "abcdefgh")
            {
                columnsCheck[m] = col;
                m++;
            }
            else
            {

                if (col == null) col = "";

                replaceWithHtml(document, plc, col);
            }
        }
        if (document.Contains("IEP1") == false)
        {
            if (document.Contains("IEP5") == false)
            {
                if (document.Contains("IEP8") == false)
                {

                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
                    {
                        MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
                        Body body = mainPart.Document.Body;

                        DocumentFormat.OpenXml.Wordprocessing.Paragraph para = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run((new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = DocumentFormat.OpenXml.Wordprocessing.BreakValues.Page })));


                        mainPart.Document.Body.InsertAfter(para, mainPart.Document.Body.LastChild);
                        mainPart.Document.Save();
                    }
                }
            }
        }



    }

    private void setCheckBox(string document, Hashtable ht, string[] columnsChks)
    {

        bool IsCheck = false;
        int i = 0;
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        {

            foreach (DocumentFormat.OpenXml.Wordprocessing.CheckBox cb in wordDoc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.CheckBox>())
            {
                if (cb != null)
                {

                    FormFieldName cbName = cb.Parent.ChildElements.First<FormFieldName>();

                    try
                    {
                        DefaultCheckBoxFormFieldState defaultState = cb.GetFirstChild<DefaultCheckBoxFormFieldState>();
                        if (i < columnsChks.Length)
                        {
                            defaultState.Val = false;
                            if (columnsChks[i] == "") IsCheck = false;
                            if (columnsChks[i] != "") IsCheck = Convert.ToBoolean(columnsChks[i]);
                            if (IsCheck == true) defaultState.Val = true;
                        }
                        i++;
                    }
                    catch
                    {

                    }
                }
            }
        }
    }

    public void replaceWithHtml(string fileName, string replace, string replaceTest)
    {

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            ParagraphProperties paragraphProperties = new ParagraphProperties();

            DocumentFormat.OpenXml.Wordprocessing.Paragraph par = wordDoc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0); //hardcoded a paragraph index for testing

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();
            //      paragraphProperties.TextAlignment.

            if (replaceTest == "") replaceTest = "&nbsp;";
            if (replaceTest != "") replaceTest = replaceTest.Trim();
            try
            {

                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();
                string[] ParaList = replaceTest.Split(new string[] { "\n\n" }, StringSplitOptions.None);
                string orginal = "";
                IList<OpenXmlCompositeElement> element = new List<OpenXmlCompositeElement>();
                foreach (var item in ParaList)
                {
                    var paragraphs = converter.Parse(item);
                    Run[] runArr = par.Descendants<Run>().ToArray();

                    // foreach each run

                    foreach (var eachel in paragraphs)
                    {
                        //if (eachel.InnerXml.Contains("table"))
                        //{
                        //}

                        element.Add(eachel);
                    }
                }
                bool flag = false;

                string txt = "";
                char vt = (char)13;

                //         OpenXmlElement elm;
                //     Microsoft.Office.Interop.Word.Paragraph para;
                string modifiedString = "";
                var parent = placeholder.Parent;
                for (int i = 0; i < element.Count; i++)
                {
                    if (element.Count == 1)
                    {
                        parent.ReplaceChild(element[i], placeholder);
                    }
                    else if (element[i].InnerText.Trim() != "")
                    {
                        Run text = (Run)placeholder.Descendants();
                        //foreach (Run run in text)
                        //{
                        string innerText = text.InnerText;
                        modifiedString = text.InnerText.Replace(innerText, "My New Text");
                        // if the InnerText doesn't modify
                        if (modifiedString != text.InnerText)
                        {
                            Text t = new Text(modifiedString);
                            text.RemoveAllChildren<Text>();
                            text.AppendChild<Text>(t);
                        }
                        // }
                        txt = txt + element[i].InnerText + vt;

                        //     elm.InnerText.Concat(elm.InnerText, element[i].InnerText);
                    }
                    // element[i].Append(paragraphProperties);

                }


                // get all runs under the paragraph and convert to be array






                if (element.Count > 1)
                {

                    // element[0].InnerText = txt;
                    placeholder.Text = txt;
                    flag = true;

                    //parent.ReplaceChild(element[i], placeholder);
                }
                //if (!flag)
                //{
                //    parent.ReplaceChild(element[i], placeholder);
                //}
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                tdMsgExport.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
            }
            /**/


            //  byte[] byteArray = File.ReadAllBytes(DocxFilePath);


            /**/



        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        tdMsgExport.InnerHtml = "";
        string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
        Array.ForEach(Directory.GetFiles(path), File.Delete);
        string Temp = Server.MapPath("~\\Administration") + "\\Temp\\";

        if (Directory.Exists(Temp))
        {
            Directory.Delete(Temp, true);
        }
        ClientScript.RegisterStartupScript(GetType(), "", "DownloadDone();", true);

    }

    public string MergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\Administration") + "\\Temp\\";
            string Temp1 = Server.MapPath("~\\Administration") + "\\IEPMerged\\";
            const string DOC_URL = "/word/document.xml";

            string FolderName = "\\IEP_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FolderName;
            string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string OUTPUT_FILE = path + "\\IEP_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.docx";
            string FIRST_PAGE = Server.MapPath("~\\Administration\\IEPTemplates\\Dummy.docx");

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            File.Copy(FIRST_PAGE, fileName);

            string[] filePaths = Directory.GetFiles(Temp);

            int i = 1;
            for (int j = filePaths.Length - 1; j >= 0; j--)
            {
                makeWord(filePaths[j], fileName, i);
                i++;
            }

            ViewState["FileName"] = fileName;
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }


            return FolderName;

        }
        catch (Exception Ex)
        {
            return "";
        }
    }
    public void makeWord(string filenamePass, string fileName1, int i)
    {

        using (WordprocessingDocument myDoc =
            WordprocessingDocument.Open(fileName1, true))
        {
            string altChunkId = "AltChunkId" + i.ToString();
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
    }


    protected void btnSign_Click(object sender, EventArgs e)
    {
        sess = sess = (clsSession)Session["UserSession"];
        clsSignature objSign = new clsSignature();
        objSign.SignDocument(sess.StudentId, sess.IEPId, sess.SchoolId, sess.LoginId);
    }
    protected void btnSignDetails_Click(object sender, EventArgs e)
    {
        sess = sess = (clsSession)Session["UserSession"];
        clsSignature objSign = new clsSignature();
        string users = objSign.getSignedUsers(sess.StudentId, sess.IEPId, sess.SchoolId);
        string[] temp = users.Split('|');
        string[] signeduser = new string[temp.Length - 1];
        int i = 0;

        foreach (var item in temp)
        {

            if (item != "")
            {
                string[] username = item.Split('=');
                signeduser[i] = username[1];
            }
            i++;
        }


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/StudentBinder/XMLSign/signXML.xml"));
        string[] userXML = null;
        string[] certificateXMl = null;
        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("Document");
        checkCount = 0;
        int index = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == "NEIEP")
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;
                userXML = new string[xmlListColumns.Count];
                certificateXMl = new string[xmlListColumns.Count];
                foreach (XmlNode stMs in xmlListColumns)
                {
                    userXML[index] = stMs.Attributes["Role"].Value;
                    certificateXMl[index] = stMs.Attributes["signature"].Value;
                    index++;
                }
            }
        }

        bool[] signed = new bool[certificateXMl.Length];


        string table = "<table style='width:100%'><tr><th>User Roles</th><th>Sign Status</th></tr>";
        for (int indexi = 0; indexi < certificateXMl.Length; indexi++)
        {
            for (int indexj = 0; indexj < signeduser.Length; indexj++)
            {
                if (certificateXMl[indexi].ToString() == signeduser[indexj].ToString())
                {
                    signed[indexi] = true;

                    break;
                }

            }

        }

        for (int isign = 0; isign < signed.Length; isign++)
        {
            if (signed[isign] == true)

                table = table + "<tr><td style='text-align:center'>" + userXML[isign].ToString() + "</td><td style='text-align:center'>Signed</td></tr>";

            else
                table = table + "<tr><td style='text-align:center'>" + userXML[isign].ToString() + "</td><td style='text-align:center'>Unsigned</td></tr>";
        }
        table = table + "</table>";
        tdUSers.InnerHtml = table;
        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "listUsers();", true);
    }


    protected void RepPage4_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        HiddenField hf_LPId = (HiddenField)e.Item.FindControl("hfLessonPlanId");
        Repeater rp4Inside = e.Item.FindControl("RepPage4Inside") as Repeater;
        HiddenField hf_GoalId = (HiddenField)e.Item.FindControl("hfGoalId");

        StrQuery3 = "SELECT  * FROM(SELECT dbo.LessonPlan.LessonPlanName,dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber,ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row, dbo.StdtLessonPlan.LessonPlanId,dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, " +
            " (SELECT TOP 1 hdr.LessonOrder from DSTempHdr hdr WHERE hdr.LessonPlanId = StdtLessonPlan.LessonPlanId AND hdr.StudentId = StdtLessonPlan.StudentId) As LessonOrder  FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON " +
            "dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId = '" + hf_GoalId.Value + "' AND dbo.StdtLessonPlan.StdtIEPId =    '" + sess.IEPId + "'  AND dbo.StdtLessonPlan.ActiveInd = 'A' AND dbo.StdtLessonPlan.IncludeIEP=1)A ORDER BY LessonOrder";

        Dt3 = objData.ReturnDataTable(StrQuery3, false);

        //DataBinder dtbnd_Obj2 = e.Item.FindControl("Objective2") as DataBinder;
        //DataBinder dtbnd_Obj3 = e.Item.FindControl("Objective3") as DataBinder;

        rp4Inside.DataSource = Dt3;
        rp4Inside.DataBind();
    }
    protected void RepPage4Inside_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        HiddenField hf_GoalId2 = (HiddenField)e.Item.FindControl("hfGoalId2");
        HiddenField hf_StdtLPId = (HiddenField)e.Item.FindControl("hfStdtLessonPlanId");

        //DataBinder dtbnd_Obj1 = e.Item.FindControl("Objective1") as DataBinder;

        //string StrQuery3 = "SELECT  * FROM(SELECT dbo.LessonPlan.LessonPlanName,dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber,ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row, dbo.StdtLessonPlan.LessonPlanId,dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3   FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON " +
        //    "dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId = '" + hf_GoalId2.Value + "' AND dbo.StdtLessonPlan.StdtIEPId =    '" + sess.IEPId + "'  AND dbo.StdtLessonPlan.ActiveInd = 'A')A ORDER BY A.GoalId";

        //DataTable Dt3 = objData.ReturnDataTable(StrQuery3, false);
    }



    #region BSP Form View 05-05-2015 Hari

    protected void btnBSP_Click(object sender, EventArgs e)
    {
        try
        {
            Sess = (clsSession)Session["UserSession"];
            if (Sess != null)
            {
                int headerId = Sess.IEPId;
                FillDoc(headerId);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
            }
        }
        catch (Exception EX)
        {

            throw EX;
        }

    }

    private void FillDoc(int stdtIEPID)
    {
        try
        {
            divMessage.InnerHtml = "";
            objData = new clsData();
            string strQuery = "";
            strQuery = "Select ROW_NUMBER() OVER (ORDER BY BSPDoc) AS No,BSPDocUrl as Document, BSPDoc FROM BSPDoc Where BSPDocUrl<>'' And StdtIEPId = " + stdtIEPID + "";
            System.Data.DataTable Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Dt.Columns.Add("Name", typeof(string));


                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        string name = Dt.Rows[i]["Document"].ToString();
                        string[] ext = name.Split('.');
                        string ext_name = ext[1];
                        if (name != "")
                        {
                            if (name.Length > 50)
                            {
                                name = name.Substring(0, 30) + "....";
                                name += ext_name;
                            }
                        }
                        Dt.Rows[i]["Name"] = name;
                    }
                    grdFile.DataSource = Dt;
                    grdFile.DataBind();
                }
                else
                    divMessage.InnerHtml = clsGeneral.warningMsg("No Data Found");
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    protected void grdFile_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void grdFile_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        string file = Convert.ToString(e.CommandArgument);
        clsData objData = new clsData();
        if (e.CommandName == "download")
        {
            if (Sess != null)
            {
                try
                {
                    HttpResponse Response = HttpContext.Current.Response;
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Byte[] data = (Byte[])objData.FetchValue("SELECT Data FROM BSPDoc WHERE BSPDoc='" + file + "'");
                    string docURL = Convert.ToString(objData.FetchValue("SELECT BSPDocUrl FROM BSPDoc WHERE BSPDoc='" + file + "'"));
                    string contentType = GetContentType(Path.GetExtension(docURL).ToLower().ToString());
                    Response.AddHeader("Content-type", contentType);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + docURL);
                    Response.BinaryWrite(data);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception Ex)
                {

                    throw Ex;
                }
            }
        }

    }

    private string GetContentType(string extension)
    {
        try
        {
            string ContentType = "";
            switch (extension)
            {
                case ".txt":
                    ContentType = "text/plain";
                    break;
                case ".doc":
                    ContentType = "application/msword";
                    break;
                case ".docx":
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".pdf":
                    ContentType = "application/pdf";
                    break;
                case ".xls":
                    ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    ContentType = "application/vnd.ms-excel";
                    break;
            }
            return ContentType;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void grdFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdFile.PageIndex = e.NewPageIndex;
        int headerId = sess.IEPId;
        if (headerId != 0)
        {
            FillDoc(headerId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
        }

    }
    protected void grdFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnUpdateandExport_Click(object sender, EventArgs e)
    {

    }
}
    #endregion
