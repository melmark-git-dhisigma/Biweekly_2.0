using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Drawing;

public partial class Graph : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    public string JsonSData;
    public string JsonTData;
    public static string grphclasid = null;
    public static string grphstudid = null;  

    protected void Page_Load(object sender, EventArgs e)
    {
        objData = new clsData();
        BtnClientAcademic.Text = "Academic by \nClient";
        BtnStaffAcademic.Text = "Academic by \nStaff";
        BtnClientClinical.Text = "Clinical by \nClient";
        BtnStaffClinical.Text = "Clinical by   \nStaff ";
        BtnClientAcademic.Attributes.CssStyle.Add("white-space", "normal");
        BtnStaffAcademic.Attributes.CssStyle.Add("white-space", "normal");
        BtnClientClinical.Attributes.CssStyle.Add("white-space", "normal");
        BtnStaffClinical.Attributes.CssStyle.Add("white-space", "normal");
        BtnClientAcademic.Attributes.CssStyle.Add("outline", "none");
        BtnStaffAcademic.Attributes.CssStyle.Add("outline", "none");
        BtnClientClinical.Attributes.CssStyle.Add("outline", "none");
        BtnStaffClinical.Attributes.CssStyle.Add("outline", "none");
        tdMsg.InnerHtml = "";
        tdMsg.Visible = false;
        if (!IsPostBack)
        {
            PlotGraphload();
            tdMsg.Visible = true;
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Please choose a Location(s) and/or Client(s) above to begin.");
        }


        if (Label_location.Text == "" && Txt_All.Text == "2")
        {
            Label_location.Text = "All Location";
        }
        else if(Label_location.Text == "")
        {
            Label_location.Text = "No Location Selected";
        }

        if (Label_Client.Text == "" && Txt_All.Text == "2")
        {
            Label_Client.Text = "All Clients";
        }
        else if (Label_Client.Text == "")
        {
            Label_Client.Text = "No Client Selected";
        }        
    }

    private void RefreshinitialLoad() 
    {
        FillClassRoomDropdown("2");
        Txt_All.Text ="2";
        chkbx_leson_deliverd.Checked = true;
        chkbx_leson_deliverd.Enabled = false;
        chkbx_block_sch.Checked = false;
        chkbx_block_sch.Enabled = true;
        Txt_graphid.Text = "1";
        
        if (Txt_All.Text == "2")
        {
           getAllStudents();
        }
        else if (Txt_Clasid.Text != null && Txt_Clasid.Text != "2" && Txt_All.Text == "CLASS")
        {
            FillStudentDropdown(Txt_Clasid.Text);
            LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, null,chkbx_Mistrial.Checked ? 1 : 0);
        }
        else
        {
            Txt_Clasid.Text = sess.Classid.ToString();
            LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, null,chkbx_Mistrial.Checked ? 1 : 0);
        }
    }

    private void getAllStudents() 
    {
        string getallDayClasses = "SELECT STUFF((SELECT DISTINCT ','+Convert(varchar(200),SC.ClassId) FROM StudentPersonal SP INNER JOIN StdtClass SC ON SP.StudentPersonalId = SC.StdtId WHERE SC.ActiveInd = 'A' AND SC.ClassId IN(Select ClassId From class where ResidenceInd IN (0,1) and Activeind = 'A') FOR XML PATH('')), 1, 1, '')";
        string getStudidDaylist = Convert.ToString(objData.FetchValue(getallDayClasses));
        Txt_Clasid.Text = getStudidDaylist;        
        FillAllStudentDropdown();
        if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text,chkbx_Mistrial.Checked ? 1 : 0); }
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardStaffAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0); }
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardStaffClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
    }

    private void getAllDayResStudents(string clastype)
    {
        string getallDayClasses = "";
        string getallResClasses = "";
        if (clastype == "0")
        {
            getallDayClasses = "SELECT STUFF((SELECT DISTINCT ','+Convert(varchar(200),SC.ClassId) FROM StudentPersonal SP INNER JOIN StdtClass SC ON SP.StudentPersonalId = SC.StdtId WHERE SC.ActiveInd = 'A' AND SC.ClassId IN(Select ClassId From class where ResidenceInd = 0 and Activeind = 'A') FOR XML PATH('')), 1, 1, '')";
            string getClassidDaylist = Convert.ToString(objData.FetchValue(getallDayClasses));
            Txt_Clasid.Text = getClassidDaylist;
            FillStudentDayResDropdown("0");
        }
        else if (clastype == "1")
        {
            getallResClasses = "SELECT STUFF((SELECT DISTINCT ','+Convert(varchar(200),SC.ClassId) FROM StudentPersonal SP INNER JOIN StdtClass SC ON SP.StudentPersonalId = SC.StdtId WHERE SC.ActiveInd = 'A' AND SC.ClassId IN(Select ClassId From class where ResidenceInd = 1 and Activeind = 'A') FOR XML PATH('')), 1, 1, '')";
            string getClassidReslist = Convert.ToString(objData.FetchValue(getallResClasses));
            Txt_Clasid.Text = getClassidReslist;
            FillStudentDayResDropdown("1");
        }
        
        
        if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text,chkbx_Mistrial.Checked ? 1 : 0); }
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardStaffAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0); }
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardStaffClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
    }
    private void PlotGraphload()
    {
        sess = (clsSession)Session["UserSession"];
        DataTable Dt = new DataTable();
       
        FillClassRoomDropdown("2");
        Txt_All.Text ="2";
        chkbx_leson_deliverd.Checked = true;
        chkbx_leson_deliverd.Enabled = false;
        chkbx_block_sch.Checked = false;
        chkbx_block_sch.Enabled = true;
        Txt_graphid.Text = "1";

        //Txt_StudSelcted.Text = "";
        //Txt_All.Text = "";
        //Txt_Clasid.Text = "";
        //Txt_Studid.Text = "";
        //Txt_Userid.Text = "";
        //Txt_clstype.Text = "";
        //Txt_graphid.Text = "";
        
        if (Txt_All.Text == "2")
        {
           //getAllStudents();
            string getallDayClasses = "SELECT STUFF((SELECT DISTINCT ','+Convert(varchar(200),SC.ClassId) FROM StudentPersonal SP INNER JOIN StdtClass SC ON SP.StudentPersonalId = SC.StdtId WHERE SC.ActiveInd = 'A' AND SC.ClassId IN(Select ClassId From class where ResidenceInd IN (0,1) and Activeind = 'A') FOR XML PATH('')), 1, 1, '')";
            string getStudidDaylist = Convert.ToString(objData.FetchValue(getallDayClasses));
            Txt_Clasid.Text = getStudidDaylist;
            FillAllStudentDropdown();
        }
        else if (Txt_Clasid.Text != null && Txt_Clasid.Text != "2" && Txt_All.Text == "CLASS")
        {
            FillStudentDropdown(Txt_Clasid.Text);
        }
        else
        {
            Txt_Clasid.Text = sess.Classid.ToString();
        }
    

            if (grphstudid != null && grphclasid == null)
            {
                Dt = FillStudentIDbased(sess.Classid);
            }
            else if (grphclasid != null && grphstudid == null)
            {
                Dt = FillStudentClassbased(sess.Classid.ToString());
            }
            else
            {
                Dt = FillStudent(sess.Classid);
            }
        
    }
    private void PlotGraph(string Type)
    {
        sess = (clsSession)Session["UserSession"];
        DataTable Dt = new DataTable();

        if (Type == "DB")
        {
            RefreshinitialLoad();

            if (grphstudid != null && grphclasid == null)
            {
                Dt = FillStudentIDbased(sess.Classid);
            }
            else if (grphclasid != null && grphstudid == null)
            {
                Dt = FillStudentClassbased(sess.Classid.ToString());
            }
            else
            {
                Dt = FillStudent(sess.Classid);
            }
        }        
    }
    [Serializable]
    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        // From: http://community.discountasp.net/default.aspx?f=14&m=15967
        // local variable for network credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user,
            out string password, out string authority)
        {
            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
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


    private DataTable FillStudentClassbased(string classid)
    {
        objData = new clsData();

        string ClassIds = "";
        if (grphclasid != null)
        {
            ClassIds = grphclasid.ToString();
        }

        string StudentQuery = "SELECT *,CASE WHEN LessonCount>=LessonsCompleted THEN ISNULL(CASE WHEN STUDDATA.LessonCount<>0 THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT,STUDDATA.LessonsCompleted)/CONVERT(FLOAT,STUDDATA.LessonCount))*100,0))  END,0) ELSE 0 END AS Lessons ,ISNULL(CASE WHEN STUDDATA.BehaviorCount<>0 " +
                               "THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT, STUDDATA.BehaviorCompleted)/CONVERT(FLOAT,STUDDATA.BehaviorCount))*100,0)) END,0) AS Behavior,(SELECT COUNT(*) FROM Behaviour BHR WHERE BHR.StudentId=STUDDATA.StudentId AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())) TotalBehaviorCount FROM (SELECT STUD.StudentId,STUD.Name,(SELECT COUNT(DISTINCT LessonPlanId) FROM StdtSessionHdr WHERE StudentId=STUD.StudentId AND CONVERT(VARCHAR(10), " +
                               "CreatedOn, 120)=CONVERT(VARCHAR(10), GETDATE(), 120)  AND [SessionStatusCd]='S') LessonsCompleted,(SELECT COUNT(DISTINCT LPId) FROM StdtLPSched SCH WHERE Day=CONVERT(VARCHAR(10), GETDATE(), 120) AND SCH.StdtId=STUD.StudentId) LessonCount, (SELECT COUNT(MeasurementId) FROM BehaviourDetails WHERE StudentId=STUD.StudentId AND ActiveInd='A') BehaviorCount," +
                               "(SELECT Count(DISTINCT b.measurementid) FROM   behaviour b  join behaviourdetails bd on bd.MeasurementId=b.MeasurementId  WHERE b.studentid = STUD.StudentId AND b.activeind = 'A' And bd.ActiveInd='A' AND CONVERT(DATE,b.CreatedOn)=CONVERT(DATE,GETDATE())) BehaviorCompleted FROM (SELECT DISTINCT SDT.StudentId," +
                               "SDT.StudentLname+','+SDT.StudentFname AS Name FROM Student SDT INNER JOIN StdtClass SCLS    ON SDT.StudentId=SCLS.StdtId WHERE SDT.ActiveInd='A' AND SCLS.ActiveInd='A' AND SCLS.ClassId IN (" + ClassIds + ")) STUD) STUDDATA";

        DataTable dtStudent = objData.ReturnDataTable(StudentQuery, false);
        return dtStudent;
    }

    private DataTable FillStudentIDbased(int classid)
    {
        objData = new clsData();

        string StudIds = "";
        if (grphstudid != null)
        {
            StudIds = grphstudid.ToString();
        }

        string studQry = "SELECT *,CASE WHEN LessonCount>=LessonsCompleted THEN ISNULL(CASE WHEN STUDDATA.LessonCount<>0 THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT,STUDDATA.LessonsCompleted)/CONVERT(FLOAT,STUDDATA.LessonCount))*100,0))  END,0) ELSE 0 END AS Lessons ,ISNULL(CASE WHEN STUDDATA.BehaviorCount<>0 " +
                         "THEN CONVERT(VARCHAR(50), ROUND((CONVERT(FLOAT, STUDDATA.BehaviorCompleted)/CONVERT(FLOAT,STUDDATA.BehaviorCount))*100,0)) END,0) AS Behavior,(SELECT COUNT(*) FROM Behaviour BHR WHERE BHR.StudentId=STUDDATA.StudentId AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())) TotalBehaviorCount FROM (SELECT STUD.StudentId,STUD.Name,(SELECT COUNT(DISTINCT LessonPlanId) FROM StdtSessionHdr WHERE StudentId=STUD.StudentId AND CONVERT(VARCHAR(10), " +
                         "CreatedOn, 120)=CONVERT(VARCHAR(10), GETDATE(), 120)  AND [SessionStatusCd]='S') LessonsCompleted,(SELECT COUNT(DISTINCT LPId) FROM StdtLPSched SCH WHERE Day=CONVERT(VARCHAR(10), GETDATE(), 120) AND SCH.StdtId=STUD.StudentId) LessonCount, (SELECT COUNT(MeasurementId) FROM BehaviourDetails WHERE StudentId=STUD.StudentId AND ActiveInd='A') BehaviorCount," +
                         "(SELECT Count(DISTINCT b.measurementid) FROM   behaviour b  join behaviourdetails bd on bd.MeasurementId=b.MeasurementId  WHERE b.studentid = STUD.StudentId AND b.activeind = 'A' And bd.ActiveInd='A' AND CONVERT(DATE,b.CreatedOn)=CONVERT(DATE,GETDATE())) BehaviorCompleted FROM (SELECT DISTINCT SDT.StudentId," +
                         "SDT.StudentLname+','+SDT.StudentFname AS Name FROM Student SDT INNER JOIN StdtClass SCLS    ON SDT.StudentId=SCLS.StdtId WHERE SDT.ActiveInd='A' AND SCLS.ActiveInd='A' AND SDT.StudentId IN(" + StudIds + ")) STUD )  STUDDATA";

        DataTable dtStudent = objData.ReturnDataTable(studQry, false);
        return dtStudent;
    }

    private void FillClassRoomDropdown(string clastype)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (grphclasid != null)
        {
            string[] classarry = grphclasid.Split(',');
            foreach (string getclassid in classarry)
            {
                foreach (System.Web.UI.WebControls.ListItem item in ddcb_clas.Items)
                {
                    if (item.Value == getclassid)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        if (sess != null)
        {
            try
            {
                DataTable dtLP = new DataTable();
                dtLP.Columns.Add("LpId", typeof(string));
                dtLP.Columns.Add("LessonName", typeof(string));

                DataTable DTLesson = new DataTable();                
                string selLessons="";
                if (clastype == "0" || clastype == "1")
                {
                    selLessons = "select cl.classid,cl.classname from class cl inner join userclass uc on cl.ClassId = uc.ClassId where cl.ActiveInd = 'A' AND uc.UserId = " + sess.LoginId + " AND cl.ResidenceInd = " + Convert.ToInt32(clastype) + " order by classname";
                }
                else if (clastype == "2") 
                {
                    selLessons = "select cl.classid,cl.classname from class cl inner join userclass uc on cl.ClassId = uc.ClassId where cl.ActiveInd = 'A' AND uc.UserId = " + sess.LoginId + " AND cl.ResidenceInd IN(0,1) order by classname"; 
                }
                DTLesson = objData.ReturnDataTable(selLessons, false);

                if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {
                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtLP.NewRow();
                            drr["LpId"] = drLessn.ItemArray[0];
                            drr["LessonName"] = drLessn.ItemArray[1];
                            dtLP.Rows.Add(drr);
                        }
                    }
                }

                ddcb_clas.DataSource = dtLP;
                ddcb_clas.DataTextField = "LessonName";
                ddcb_clas.DataValueField = "LpId";
                ddcb_clas.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void FillStudentDropdown(string getClassid)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (grphstudid != null)
        {
            string[] studarry = grphstudid.Split(',');
            foreach (string getstudid in studarry)
            {
                foreach (System.Web.UI.WebControls.ListItem item in ddcb_stud.Items)
                {
                    if (item.Value == getstudid)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        if (sess != null)
        {
            try
            {
                DataTable dtST = new DataTable();
                dtST.Columns.Add("StdId", typeof(string));
                dtST.Columns.Add("StdName", typeof(string));

                DataTable DTLesson = new DataTable();
                //string selQuery = "select distinct sp.StudentPersonalId,sp.LastName,sp.FirstName,sp.Middlename,sc.ClassId from studentpersonal sp inner join stdtclass sc on sp.StudentPersonalId = sc.StdtId  where sc.ClassId IN(" + getClassid + ") and sc.ActiveInd = 'A' order by sc.ClassId,sp.Lastname";
                string selQuery = "SELECT distinct StudentId,(SELECT CONCAT(lastname,+', '+FirstName) FROM StudentPersonal WHERE StudentPersonalID = SC.stdtid) AS StudentName" +
                                  " FROM StdtClass SC left Join Student STD ON STD.StudentId = SC.StdtId left join Placement PLC on STD.StudentId = PLC.StudentPersonalId" +
                                  " WHERE SC.ClassId IN(" + getClassid + ") AND STD.ActiveInd='A' AND SC.ActiveInd='A' AND PLC.Location IN(" + getClassid + ") AND (PLC.EndDate is null or convert(DATE,PLC.EndDate) >= convert(DATE,getdate())) ORDER BY StudentName";
                //STD.SchoolId=1 AND

                //string getStudidlistquery = "SELECT STUFF((select distinct ','+Convert(varchar(200),sp.StudentPersonalId) from studentpersonal sp inner join stdtclass sc on sp.StudentPersonalId = sc.StdtId  where sc.ClassId IN(" + getClassid + ") and sc.ActiveInd = 'A' FOR XML PATH('')), 1, 1, '')  ";
                string getStudidlistquery = "SELECT STUFF((SELECT distinct ','+Convert(varchar(200),StudentId)" +
                                            " FROM StdtClass SC left Join Student STD ON STD.StudentId = SC.StdtId left join Placement PLC on STD.StudentId = PLC.StudentPersonalId" +
                                            " WHERE SC.ClassId IN (" + getClassid + ") AND STD.ActiveInd='A' AND SC.ActiveInd='A' AND PLC.Location IN(" + getClassid + ") AND (PLC.EndDate is null or convert(DATE,PLC.EndDate) >= convert(DATE,getdate()))FOR XML PATH('')), 1, 1, '')";
                 //STD.SchoolId=1 AND

                string getStudidlist = Convert.ToString(objData.FetchValue(getStudidlistquery));
                Txt_Studid.Text = getStudidlist;
                DTLesson = objData.ReturnDataTable(selQuery, false);

                if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {
                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtST.NewRow();
                            drr["StdId"] = drLessn.ItemArray[0];
                            drr["StdName"] = drLessn.ItemArray[1];// +"," + drLessn.ItemArray[2];
                            dtST.Rows.Add(drr);
                        }
                    }
                }

                ddcb_stud.DataSource = dtST;
                ddcb_stud.DataTextField = "StdName";
                ddcb_stud.DataValueField = "StdId";
                ddcb_stud.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void FillStudentDayResDropdown(string DayResId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (grphstudid != null)
        {
            string[] studarry = grphstudid.Split(',');
            foreach (string getstudid in studarry)
            {
                foreach (System.Web.UI.WebControls.ListItem item in ddcb_stud.Items)
                {
                    if (item.Value == getstudid)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        if (sess != null)
        {
            try
            {
                DataTable dtST = new DataTable();
                dtST.Columns.Add("StdId", typeof(string));
                dtST.Columns.Add("StdName", typeof(string));

                DataTable DTLesson = new DataTable();
                //string selQuery = "select distinct sp.StudentPersonalId,sp.LastName,sp.FirstName,sp.Middlename,sc.ClassId from studentpersonal sp inner join stdtclass sc on sp.StudentPersonalId = sc.StdtId  where sc.ClassId IN(" + getClassid + ") and sc.ActiveInd = 'A' order by sc.ClassId,sp.Lastname";
                string selQuery = "SELECT distinct sp.StudentPersonalId,(CONCAT(sp.lastname,+', '+sp.FirstName)) AS StudentName,cl.ResidenceInd" +
                                  " from Stdtclass sc INNER JOIN class cl on sc.ClassId = cl.ClassId Inner join StudentPersonal sp on sc.StdtId = sp.StudentPersonalId" +
                                  " where sc.ActiveInd ='A' and cl.ResidenceInd =" + Convert.ToInt32(DayResId)  + " AND sp.LastName IS NOT NULL ORDER BY StudentName";


                //string getStudidlistquery = "SELECT STUFF((select distinct ','+Convert(varchar(200),sp.StudentPersonalId) from studentpersonal sp inner join stdtclass sc on sp.StudentPersonalId = sc.StdtId  where sc.ClassId IN(" + getClassid + ") and sc.ActiveInd = 'A' FOR XML PATH('')), 1, 1, '')  ";
                string getStudidlistquery = "SELECT STUFF((SELECT distinct ','+Convert(varchar(200),sp.StudentPersonalId)" +
                                            " from Stdtclass sc INNER JOIN class cl on sc.ClassId = cl.ClassId Inner join StudentPersonal sp on sc.StdtId = sp.StudentPersonalId" +
                                            " where sc.ActiveInd ='A' and cl.ResidenceInd = "+ Convert.ToInt32(DayResId)  + " AND sp.LastName IS NOT NULL FOR XML PATH('')), 1, 1, '')";

                string getStudidlist = Convert.ToString(objData.FetchValue(getStudidlistquery));
                Txt_Studid.Text = getStudidlist;
                DTLesson = objData.ReturnDataTable(selQuery, false);

                if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {
                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtST.NewRow();
                            drr["StdId"] = drLessn.ItemArray[0];
                            drr["StdName"] = drLessn.ItemArray[1];// +"," + drLessn.ItemArray[2];
                            dtST.Rows.Add(drr);
                        }
                    }
                }

                ddcb_stud.DataSource = dtST;
                ddcb_stud.DataTextField = "StdName";
                ddcb_stud.DataValueField = "StdId";
                ddcb_stud.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void FillAllStudentDropdown()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (grphstudid != null)
        {
            string[] studarry = grphstudid.Split(',');
            foreach (string getstudid in studarry)
            {
                foreach (System.Web.UI.WebControls.ListItem item in ddcb_stud.Items)
                {
                    if (item.Value == getstudid)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        if (sess != null)
        {
            try
            {
                DataTable dtST = new DataTable();
                dtST.Columns.Add("StdId", typeof(string));
                dtST.Columns.Add("StdName", typeof(string));

                DataTable DTLesson = new DataTable();
                //string selQuery = "SELECT DISTINCT StudentPersonalid,CONCAT(lastname,+', '+FirstName) AS StudentName FROM studentpersonal ORDER By StudentName";
                string selQuery = "SELECT DISTINCT StudentPersonalid,CONCAT(lastname,+', '+FirstName) AS StudentName FROM studentpersonal sp INNER JOIN stdtclass sc on sp.StudentPersonalId = sc.StdtId ORDER By StudentName";
                string getStudidlistquery = "SELECT STUFF((SELECT DISTINCT ','+Convert(Varchar(200),StudentPersonalid) FROM studentpersonal sp INNER JOIN stdtclass sc on sp.StudentPersonalId = sc.StdtId FOR XML PATH('')), 1, 1, '')";
                string getStudidlist = Convert.ToString(objData.FetchValue(getStudidlistquery));
                Txt_Studid.Text = getStudidlist;
                DTLesson = objData.ReturnDataTable(selQuery, false);
                Label_location.Text = "All Location";
                Label_Client.Text = "All Clients";
                if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {
                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtST.NewRow();
                            drr["StdId"] = drLessn.ItemArray[0];
                            drr["StdName"] = drLessn.ItemArray[1];
                            dtST.Rows.Add(drr);
                        }
                    }
                }

                ddcb_stud.DataSource = dtST;
                ddcb_stud.DataTextField = "StdName";
                ddcb_stud.DataValueField = "StdId";
                ddcb_stud.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }   

    private void Classadd()
    {
        string ClassId = "";
        string Classlabel = "";
        if (Label_Client.Text == "")
        {
            //Label_Client.Text = "No Client Selected";
        }
        foreach (System.Web.UI.WebControls.ListItem item in ddcb_clas.Items)
        {
            if (item.Selected == true)
            {
                ClassId += item.Value + ",";
                Classlabel += item.Text + ";";
            }
        }
        if (ClassId.Length > 0)
        {
            ClassId = ClassId.Substring(0, (ClassId.Length - 1));
            Txt_Clasid.Text = ClassId;
            Classlabel = Classlabel.Substring(0, (Classlabel.Length - 1));
            Label_location.Text = Classlabel;
            grphclasid = ClassId;
            FillStudentDropdown(Txt_Clasid.Text);
            //Label_Client.Text = "All Clients";
            Txt_All.Text = "CLASS";
        }
        else 
        {
            FillClassRoomDropdown("2");
            Label_location.Text = "All Location";
            Label_Client.Text = "All Clients";
            Label1_CrntSelctn.Text = "Current Selection:";
            Txt_Studid.Text = "";
            Txt_Clasid.Text = "";
            Txt_All.Text = "2";
            if (Txt_All.Text == "2") { rbtnClassType.SelectedIndex = 2;}
            chkbx_leson_deliverd.Checked = true;
            chkbx_leson_deliverd.Enabled = false;
            chkbx_block_sch.Checked = false;
            chkbx_block_sch.Enabled = true;
            Txt_StudSelcted.Text = "";
            getAllStudents();
            grphstudid = null;   
            
        }
    }

    protected void ddcb_clas_DataBound(object sender, EventArgs e)
    {
        foreach (System.Web.UI.WebControls.ListItem item in ddcb_clas.Items)
        {
            item.Selected = true;
        }
    }

    public void ddcb_stud_DataBound(object sender, EventArgs e)
    {
        string s = "";
        if (Txt_StudSelcted.Text != null) { s = Txt_StudSelcted.Text;  }
        else { s = Txt_Studid.Text; }
        if (Txt_Studid.Text.Contains(Txt_StudSelcted.Text))
        {
            foreach (System.Web.UI.WebControls.ListItem item in ddcb_stud.Items)
            {
                if (s != null)
                {
                    string[] values = s.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        if (item.Value == values[i])
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }
        else
        {
            string prevStudselction = Txt_StudSelcted.Text;
            Txt_StudSelcted.Text = Txt_Studid.Text.Contains(prevStudselction).ToString();
            Label_Client.Text = "All Clients";
        }
    }


    private void Studentadd()
    {
        string StudentId = "";
        string Studentname = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddcb_stud.Items) 
        {
            if (item.Selected == true)
            {
                StudentId += item.Value + ",";
                Studentname += item.Text + ";";
            }
        }
        if (StudentId.Length > 0)
        {
            StudentId = StudentId.Substring(0, (StudentId.Length - 1));
            Txt_Studid.Text = StudentId;
            Studentname = Studentname.Substring(0, (Studentname.Length - 1));
            Label_Client.Text = Studentname;
            grphstudid = StudentId;
            Txt_StudSelcted.Text = grphstudid;
        }
        else 
        {
            Label_Client.Text = "All Clients";
            Txt_Studid.Text = "";
            Txt_StudSelcted.Text = "";
            FillStudentDropdown(Txt_Clasid.Text);            
            grphstudid = Txt_Studid.Text;
        }
    }

    private void LoadDashBoardClientAcademicGraph(string CAgraphClassid, string CAgraphStudid, int CAgraphMistrial)
    {
        RV_DBReport.Visible = true;
        RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DashBoardClientAcademic"];        
        //Classids = Convert.ToString(objData.FetchValue("select classid from class where classid = 2011"));
        String Classids = Convert.ToString(CAgraphClassid);
        String Studids = Convert.ToString(CAgraphStudid);
        String Mistrial = Convert.ToString(CAgraphMistrial);
        RV_DBReport.ShowParameterPrompts = false;
        ReportParameter[] parm = new ReportParameter[3];        
        parm[0] = new ReportParameter("ParamClassid", Classids);
        parm[1] = new ReportParameter("ParamStudid", Studids);
        parm[2] = new ReportParameter("ParamMistrial", Mistrial);
        this.RV_DBReport.ServerReport.SetParameters(parm);
        RV_DBReport.ServerReport.Refresh();
    }

    private void LoadDashBoardClientAcademicPercentage(string CAgraphClassid, string CAgraphStudid)
    {
        RV_DBReport.Visible = true;
        RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DashBoardClientAcademicPercentage"];
        //Classids = Convert.ToString(objData.FetchValue("select classid from class where classid = 2011"));
        String Classids = Convert.ToString(CAgraphClassid);
        String Studids = Convert.ToString(CAgraphStudid);
        RV_DBReport.ShowParameterPrompts = false;
        ReportParameter[] parm = new ReportParameter[2];
        parm[0] = new ReportParameter("ParamClassid", Classids);
        parm[1] = new ReportParameter("ParamStudid", Studids);
        this.RV_DBReport.ServerReport.SetParameters(parm);
        RV_DBReport.ServerReport.Refresh();
    }

    private void LoadDashBoardStaffAcademicGraph(string SAgraphClassid, string SAgraphStudid, int CAgraphMistrial)
    {
        RV_DBReport.Visible = true;
        RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DashBoardStaffAcademic"];
        String Classids = Convert.ToString(SAgraphClassid);
        String Studids = Convert.ToString(SAgraphStudid);
        String Mistrial = Convert.ToString(CAgraphMistrial);
        String getUseridsquery = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),Modifiedby) from stdtsessionhdr where stdtclassid IN(" + Classids + ") and studentid IN(" + Studids + ") and CONVERT(VARCHAR(10),ModifiedOn, 120) = CONVERT(VARCHAR(10),getdate(), 120)FOR XML PATH('')), 1, 1, '')";
        String Userids = Convert.ToString(objData.FetchValue(getUseridsquery));
        if (Userids == "")
        {
            Userids = null;
        }
        Txt_Userid.Text = Userids;        
        RV_DBReport.ShowParameterPrompts = false;
        ReportParameter[] parm = new ReportParameter[4];
        parm[0] = new ReportParameter("ParamClassid", Classids);
        parm[1] = new ReportParameter("ParamStudid", Studids);
        parm[2] = new ReportParameter("ParamUserid", Userids);
        parm[3] = new ReportParameter("ParamMistrial", Mistrial);
        this.RV_DBReport.ServerReport.SetParameters(parm);
        RV_DBReport.ServerReport.Refresh();
    }

    private void LoadDashBoardClientClinicalGraph(string CCgraphClassid, string CCgraphStudid)
    {
        RV_DBReport.Visible = true;
        RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DashBoardClientClinical"];        
        String Classids = Convert.ToString(CCgraphClassid);
        String Studids = Convert.ToString(CCgraphStudid);
        RV_DBReport.ShowParameterPrompts = false;
        ReportParameter[] parm = new ReportParameter[2];
        parm[0] = new ReportParameter("ParamClassid", Classids);
        parm[1] = new ReportParameter("ParamStudid", Studids);
        this.RV_DBReport.ServerReport.SetParameters(parm);
        RV_DBReport.ServerReport.Refresh();
    }

    private void LoadDashBoardStaffClinicalGraph(string SCgraphClassid, string SCgraphStudid)
    {
        RV_DBReport.Visible = true;
        RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DashBoardStaffClinical"];
        String Classids = Convert.ToString(SCgraphClassid);
        String Studids = Convert.ToString(SCgraphStudid);
        String getUseridsquery = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),CreatedBy) from behaviour where Classid IN(" + Classids + ")  and Studentid IN(" + Studids + ") and CONVERT(VARCHAR(10),ModifiedOn, 120) = CONVERT(VARCHAR(10),getdate(), 120)FOR XML PATH('')), 1, 1, '') ";
        String Userids = Convert.ToString(objData.FetchValue(getUseridsquery));
        if (Userids == "") 
        {
            Userids = null;
        }
        Txt_Userid.Text = Userids;        
        RV_DBReport.ShowParameterPrompts = false;
        ReportParameter[] parm = new ReportParameter[3];
        parm[0] = new ReportParameter("ParamClassid", Classids);
        parm[1] = new ReportParameter("ParamStudid", Studids);
        parm[2] = new ReportParameter("ParamUserid", Userids);
        this.RV_DBReport.ServerReport.SetParameters(parm);
        RV_DBReport.ServerReport.Refresh();
    }

    private void TestLoad()
    {
        RV_DBReport.Visible = true;
        RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
        RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["TESTDashBoardClientClinical"];
        RV_DBReport.ServerReport.Refresh();
    }

    protected void ddcb_clas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Classadd();
        //if (Txt_Clasid.Text == "0")
        //{
        //    //FillStudentDropdown(sess.Classid.ToString());
        //    getAllStudents();
        //}
        //else
        //{
        //    FillStudentDropdown(Txt_Clasid.Text);
        //}        
    }
    protected void ddcb_stud_SelectedIndexChanged(object sender, EventArgs e)
    {
        Studentadd();
    }
    protected void BtnClientAcademic_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        string studid = Txt_Studid.Text;
        BtnClientAcademic.BackColor = System.Drawing.Color.FromArgb(88, 163, 163);
        BtnClientClinical.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnStaffAcademic.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnStaffClinical.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        if (Txt_Clasid.Text != null && studid != null)
        {
            //FillStudentDropdown(Txt_Clasid.Text);
            if (chkbx_leson_deliverd.Checked == true)
            {
                Txt_graphid.Text = "1";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else 
                {
                    if (grphstudid != null)
                    {
                        ddcb_stud_DataBound(sender, e);
                        LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, grphstudid, chkbx_Mistrial.Checked ? 1 : 0);
                    }
                    else
                    {
                        ddcb_stud_DataBound(sender, e);
                        LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0);
                    }
                }  
            }
            else if (chkbx_block_sch.Checked == true)
            {
                Txt_graphid.Text = "1";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else { ddcb_stud_DataBound(sender, e); LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, grphstudid); }
            }
        }
        else
        {
            ddcb_stud_DataBound(sender, e);
            FillStudentDropdown(sess.Classid.ToString());            
            LoadDashBoardClientAcademicGraph(sess.Classid.ToString(), null,chkbx_Mistrial.Checked ? 1 : 0);
        }
    }
    protected void BtnStaffAcademic_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        string studid = Txt_Studid.Text; 
        BtnStaffAcademic.BackColor = System.Drawing.Color.FromArgb(88, 163, 163);
        BtnClientClinical.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnClientAcademic.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnStaffClinical.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        if (Txt_Clasid.Text != null && studid != null)
        {
            //FillStudentDropdown(Txt_Clasid.Text);
            if (chkbx_leson_deliverd.Checked == true)
            {
                Txt_graphid.Text = "2";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else 
                {
                    if (grphstudid != null)
                    {
                        ddcb_stud_DataBound(sender, e); LoadDashBoardStaffAcademicGraph(Txt_Clasid.Text, grphstudid, chkbx_Mistrial.Checked ? 1 : 0);
                    }
                    else
                    {
                        ddcb_stud_DataBound(sender, e); LoadDashBoardStaffAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0);
                    }
                 }
            }
            else if (chkbx_block_sch.Checked == true)
            {
                Txt_graphid.Text = "2";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else 
                { 
                    ddcb_stud_DataBound(sender, e); LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, grphstudid); 
                }
            }
        }
        else
        {
            ddcb_stud_DataBound(sender, e);
            FillStudentDropdown(sess.Classid.ToString());
            LoadDashBoardStaffAcademicGraph(sess.Classid.ToString(), null,chkbx_Mistrial.Checked ? 1 : 0);
        }
    }
    protected void BtnClientClinical_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        string studid = Txt_Studid.Text;          
        BtnClientClinical.BackColor = System.Drawing.Color.FromArgb(88, 163, 163);
        BtnStaffAcademic.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnClientAcademic.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnStaffClinical.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        if (Txt_Clasid.Text != null && studid !=null)
        {
            //FillStudentDropdown(Txt_Clasid.Text);
            if (chkbx_leson_deliverd.Checked == true)
            {
                Txt_graphid.Text = "3";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else 
                {
                    if (grphstudid != null)
                    {
                        ddcb_stud_DataBound(sender, e); LoadDashBoardClientClinicalGraph(Txt_Clasid.Text, grphstudid);
                    }
                    else
                    {
                        ddcb_stud_DataBound(sender, e); LoadDashBoardClientClinicalGraph(Txt_Clasid.Text,Txt_Studid.Text);
                    }
                }
            }
            else if (chkbx_block_sch.Checked == true)
            {
                Txt_graphid.Text = "3";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else { ddcb_stud_DataBound(sender, e); LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, grphstudid); }
            }
        }
        else
        {
            ddcb_stud_DataBound(sender, e);
            FillStudentDropdown(sess.Classid.ToString());
            LoadDashBoardClientClinicalGraph(sess.Classid.ToString(), null);
        }
    }
    protected void BtnStaffClinical_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        string studid = Txt_Studid.Text;                
        BtnStaffClinical.BackColor = System.Drawing.Color.FromArgb(88, 163, 163);
        BtnStaffAcademic.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnClientAcademic.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        BtnClientClinical.BackColor = System.Drawing.Color.FromArgb(0, 84, 159);
        if (Txt_Clasid.Text != null && studid != null)
        {
            //FillStudentDropdown(Txt_Clasid.Text);
            if (chkbx_leson_deliverd.Checked == true)
            {
                Txt_graphid.Text = "4";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else 
                {
                    if (grphstudid != null)
                    {
                        ddcb_stud_DataBound(sender, e); LoadDashBoardStaffClinicalGraph(Txt_Clasid.Text, grphstudid);
                    }
                    else
                    {
                        ddcb_stud_DataBound(sender, e); LoadDashBoardStaffClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text);
                    }
                }
            }
            else if (chkbx_block_sch.Checked == true)
            {
                Txt_graphid.Text = "4";
                if ((Txt_All.Text == "2" && grphstudid == null)) { getAllStudents(); }
                else if ((Txt_All.Text == "0" && grphstudid == null)) { getAllDayResStudents("0"); }
                else if ((Txt_All.Text == "1" && grphstudid == null)) { getAllDayResStudents("1"); }
                else { ddcb_stud_DataBound(sender, e); LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, grphstudid); }
            }
        }
        else
        {
            ddcb_stud_DataBound(sender, e);
            FillStudentDropdown(sess.Classid.ToString());
            LoadDashBoardStaffClinicalGraph(sess.Classid.ToString(), null);
        }        
    }

    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "1")) {BtnClientAcademic_Click(sender, e);}
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "2")) { BtnStaffAcademic_Click(sender,e);}
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "3")) { BtnClientClinical_Click(sender,e);}
        else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "4")) { BtnStaffClinical_Click(sender,e);}
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "1")) { BtnClientAcademic_Click(sender, e); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "2")) { BtnStaffAcademic_Click(sender, e); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "3")) { BtnClientClinical_Click(sender, e); }
        else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "4")) { BtnStaffClinical_Click(sender, e); }
    }

    protected void rbtnClassType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Txt_clstype.Text == "DAY")
        {
            FillClassRoomDropdown("0");
            Label_location.Text = "All Location";
            Label_Client.Text = "All Clients";
            Label1_CrntSelctn.Text = "Current Selection (All Day):";
            Txt_Studid.Text = "";
            Txt_Clasid.Text = "";
            Txt_All.Text = "0";
            Txt_StudSelcted.Text = "";
            getAllDayResStudents("0");
            grphstudid = null;
        }
        else if (Txt_clstype.Text == "RES")
        {
            FillClassRoomDropdown("1");
            Label_location.Text = "All Location";
            Label_Client.Text = "All Clients";
            Label1_CrntSelctn.Text = "Current Selection (All Res):";
            Txt_Studid.Text = "";
            Txt_Clasid.Text = "";
            Txt_All.Text = "1";
            Txt_StudSelcted.Text = "";
            getAllDayResStudents("1");
            grphstudid = null;
        }
        else if (Txt_clstype.Text == "BOTH") 
        {
            FillClassRoomDropdown("2");
            Label_location.Text = "All Location";
            Label_Client.Text = "All Clients";
            Label1_CrntSelctn.Text = "Current Selection:";
            Txt_Studid.Text = "";
            Txt_Clasid.Text = "";
            Txt_All.Text = "2";
            Txt_StudSelcted.Text = "";
            getAllStudents();
            grphstudid = null;
        }
    }

    protected void chkbx_leson_deliverd_CheckedChanged(object sender, EventArgs e)
    {
        if(chkbx_leson_deliverd.Checked == true)
        {
            chkbx_leson_deliverd.Enabled = false;
            chkbx_block_sch.Checked = false;
            chkbx_block_sch.Enabled = true;
        }
    }
    protected void chkbx_block_sch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbx_block_sch.Checked == true)
        {
            chkbx_block_sch.Enabled = false;            
            chkbx_leson_deliverd.Checked = false;
            chkbx_leson_deliverd.Enabled = true;
        }
    }
    protected void chkbx_Mistrial_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbx_Mistrial.Checked == true)
        {
            //chkbx_Mistrial.Enabled = false;
            //chkbx_leson_deliverd.Checked = false;
            //chkbx_leson_deliverd.Enabled = true;
        }
    }
}