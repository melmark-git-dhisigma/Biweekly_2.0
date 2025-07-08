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
using System.Web.Services;
using System.Web.Script.Services;



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
        //if (!IsPostBack)
        //{
        //    PlotGraphload();
        //    tdMsg.Visible = true;
        //    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please choose a Location(s) and/or Client(s) above to begin.");
        //}


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
        
        if (!IsPostBack)
        {
            PlotGraphload();
            sess = (clsSession)Session["UserSession"];
            int userId = sess.LoginId;
            string classId = sess.Classid.ToString();
            string getStudidlistquery = "SELECT STUFF((SELECT distinct ','+Convert(varchar(200),StudentId)" +
                                            " FROM StdtClass SC left Join Student STD ON STD.StudentId = SC.StdtId left join Placement PLC on STD.StudentId = PLC.StudentPersonalId" +
                                            " WHERE SC.ClassId IN (" + classId + ") AND STD.ActiveInd='A' AND SC.ActiveInd='A' AND PLC.Location IN(" + classId + ") AND PLC.Status = 1 AND (PLC.EndDate is null or convert(DATE,PLC.EndDate) >= convert(DATE,getdate()))FOR XML PATH('')), 1, 1, '')";
            DataTable dtLP = new DataTable();
            dtLP.Columns.Add("LpId", typeof(string));
            dtLP.Columns.Add("LessonName", typeof(string));
            DataTable DTLesson = new DataTable();
            string selLessons = "select cl.classid,cl.classname from class cl inner join userclass uc on cl.ClassId = uc.ClassId where cl.ActiveInd = 'A' AND uc.UserId = " + sess.LoginId + " order by classname";
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

            ddlClassrooms.DataSource = dtLP;
            ddlClassrooms.DataTextField = "LessonName";
            ddlClassrooms.DataValueField = "LpId";
            ddlClassrooms.DataBind();
            ddlClassrooms.SelectedValue = classId;


            string getStudidlist = Convert.ToString(objData.FetchValue(getStudidlistquery));
            BtnSwitchTableChart.Text = "Table View";
            graphContainerAcademic.Visible = true;
            graphContainerAcademicStaff.Visible = true;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = true;
            highcheck.Checked = true;
            LoadDashBoardStaffAcademicGraph(classId, getStudidlist, userId);
            LoadDashBoardClientAcademicGraph(classId, getStudidlist, userId);
            LoadDashBoardStaffClinicalGraph(classId, getStudidlist);
            LoadDashBoardClientClinicalGraph(classId, getStudidlist);
        }
        else
        {
            if (BtnSwitchTableChart.Text == "Chart View")
            {
                RV_DBReport.Visible = false;
                highcheck.Visible = false;
                BtnRefresh.Visible = false;
                ButtonGo.Visible = true;
                btnExportToExcel.Visible = true;
                string script1 = "TableView();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Table", script1, true);
            }
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
        if (BtnSwitchTableChart.Text == "Table View")
        {
            if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0); }
            else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardStaffAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0); }
            else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardStaffClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        }
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

        if (BtnSwitchTableChart.Text == "Table View")
        {
            if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0); }
            else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardStaffAcademicGraph(Txt_Clasid.Text, Txt_Studid.Text, chkbx_Mistrial.Checked ? 1 : 0); }
            else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_leson_deliverd.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardStaffClinicalGraph(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "1")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "2")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "3")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
            else if ((chkbx_block_sch.Checked == true) && (Txt_graphid.Text == "4")) { LoadDashBoardClientAcademicPercentage(Txt_Clasid.Text, Txt_Studid.Text); }
        }
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
                                  " WHERE SC.ClassId IN(" + getClassid + ") AND STD.ActiveInd='A' AND SC.ActiveInd='A' AND PLC.Location IN(" + getClassid + ") AND PLC.Status = '1' AND (PLC.EndDate is null or convert(DATE,PLC.EndDate) >= convert(DATE,getdate())) ORDER BY StudentName";
                //STD.SchoolId=1 AND

                //string getStudidlistquery = "SELECT STUFF((select distinct ','+Convert(varchar(200),sp.StudentPersonalId) from studentpersonal sp inner join stdtclass sc on sp.StudentPersonalId = sc.StdtId  where sc.ClassId IN(" + getClassid + ") and sc.ActiveInd = 'A' FOR XML PATH('')), 1, 1, '')  ";
                string getStudidlistquery = "SELECT STUFF((SELECT distinct ','+Convert(varchar(200),StudentId)" +
                                            " FROM StdtClass SC left Join Student STD ON STD.StudentId = SC.StdtId left join Placement PLC on STD.StudentId = PLC.StudentPersonalId" +
                                            " WHERE SC.ClassId IN (" + getClassid + ") AND STD.ActiveInd='A' AND SC.ActiveInd='A' AND PLC.Location IN(" + getClassid + ") AND PLC.Status = '1' AND (PLC.EndDate is null or convert(DATE,PLC.EndDate) >= convert(DATE,getdate()))FOR XML PATH('')), 1, 1, '')";
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
        if (highcheck.Checked == false)
        {
        RV_DBReport.Visible = false; //Shoudld be True
        graphContainerAcademic.Visible = false;
        graphContainerAcademicStaff.Visible = false;
        graphContainerClinical.Visible = false;
        graphContainerClinicalStaff.Visible = false;
        //graphcontainer.Visible = false;
        //lblNoData.Text = "";
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
        else
        {
            RV_DBReport.Visible = false;
            graphContainerAcademic.Visible = true;
            graphContainerAcademicStaff.Visible = true;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = true;
            //graphcontainer.Visible = true;
            //lblNoData.Text = "";
            string script1 = "loadWait();";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "show", script1, true);
            string script = @"setTimeout(function() {loadAcbyClient('" + CAgraphClassid + "', '" + CAgraphStudid + "','" + CAgraphMistrial + "');}, 500);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript", script, true);

    }
     
    }

    private void LoadDashBoardClientAcademicPercentage(string CAgraphClassid, string CAgraphStudid)
    {
        if (highcheck.Checked == false)
        {
            graphContainerAcademic.Visible = false;
            graphContainerAcademicStaff.Visible = false;
            graphContainerClinical.Visible = false;
            graphContainerClinicalStaff.Visible = false;
            //graphcontainer.Visible = false;
         //lblNoData.Text = "";
         RV_DBReport.Visible = false; //Shoudld be True
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
        else
        {
            CAgraphClassid = ddlClassrooms.SelectedValue;
            RV_DBReport.Visible = false;
            graphContainerAcademic.Visible = false;
            graphContainerAcademicStaff.Visible = false;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = false;
            //graphcontainer.Visible = true;
            //lblNoData.Text = "";
            
            string script1 = "loadWait();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "show3", script1, true);
            string script = @"setTimeout(function() {loadAcbyClientPerc('" + CAgraphClassid + "', '" + CAgraphStudid + "');}, 500);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript3", script, true);
        }
    }

    private void LoadDashBoardStaffAcademicGraph(string SAgraphClassid, string SAgraphStudid, int CAgraphMistrial)
    {
        if (highcheck.Checked == false)
        {
            graphContainerAcademic.Visible = false;
            graphContainerAcademicStaff.Visible = false;
            graphContainerClinical.Visible = false;
            graphContainerClinicalStaff.Visible = false;
            //graphcontainer.Visible = false;
            //lblNoData.Text = "";
            RV_DBReport.Visible = false; //Shoudld be True
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
        else
        {
            string script1 = "loadWait();";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showload", script1, true);
            RV_DBReport.Visible = false;
            graphContainerAcademic.Visible = true;
            graphContainerAcademicStaff.Visible = true;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = true;
            //graphcontainer.Visible = true;
            String Classids = Convert.ToString(SAgraphClassid);
            String Studids = Convert.ToString(SAgraphStudid);
            String Mistrial = Convert.ToString(CAgraphMistrial);
            String getUseridsquery = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),Modifiedby) from stdtsessionhdr where stdtclassid IN(" + Classids + ") and studentid IN(" + Studids + ") and CONVERT(VARCHAR(10),ModifiedOn, 120) = CONVERT(VARCHAR(10),getdate(), 120)FOR XML PATH('')), 1, 1, '')";
            String Userids = Convert.ToString(objData.FetchValue(getUseridsquery));
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            SqlCommand cmd = null;
            DataTable Dt = new DataTable();
            clsData ObjData = new clsData();
            SqlConnection con = ObjData.Open();
            if (Userids == "")
                Userids = null;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("DashboardStaffAcademic", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@ParamClassid", Classids);
                cmd.Parameters.AddWithValue("@ParamStudid", Studids);
                cmd.Parameters.AddWithValue("@ParamUserid", Userids);
                cmd.Parameters.AddWithValue("@ParamMistrial", Mistrial);
                da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);

                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + Studids + "\n" + ex.ToString());
            }
            finally
            {
                ObjData.Close(con);
            }
            foreach (DataRow dr in Dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn dc in Dt.Columns)
                {
                    row.Add(dc.ColumnName, dr[dc]);
                }
                rows.Add(row);

            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            //string script = @"setTimeout(function() {loadAcademicbyStaff('" + json.Serialize(rows) + "');}, 300);";
            string jsonString = HttpUtility.JavaScriptStringEncode(json.Serialize(rows));
            string script = string.Format("setTimeout(function() {{ loadAcademicbyStaff('{0}'); }}, 300);", jsonString);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadAcademicbyStaff", script, true);
        }
    }

    private void LoadDashBoardClientClinicalGraph(string CCgraphClassid, string CCgraphStudid)
    {
        if (highcheck.Checked == false)
        {
            RV_DBReport.Visible = false; //Shoudld be True
            graphContainerAcademic.Visible = false;
            graphContainerAcademicStaff.Visible = false;
            graphContainerClinical.Visible = false;
            graphContainerClinicalStaff.Visible = false;
            //graphcontainer.Visible = false;
        //lblNoData.Text = "";
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
        else {
            RV_DBReport.Visible = false;
            graphContainerAcademic.Visible = true;
            graphContainerAcademicStaff.Visible = true;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = true;
            //graphcontainer.Visible = true;
            //lblNoData.Text = "";
            string script1 = "loadWait();";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "show1", script1, true);
            string script = @"setTimeout(function() {loadClinicbyClient('" + CCgraphClassid + "', '" + CCgraphStudid + "');}, 500);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessageWithParamsScript2", script, true);
    }
        
    }

    private void LoadDashBoardStaffClinicalGraph(string SCgraphClassid, string SCgraphStudid)
    {
        if (highcheck.Checked == false)
        {
            RV_DBReport.Visible = false; //Shoudld be True
            RV_DBReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            RV_DBReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DashBoardStaffClinical"];
            String Classids = Convert.ToString(SCgraphClassid);
            String Studids = Convert.ToString(SCgraphStudid);
            String getUseridsquery = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),CreatedBy) from Behaviour where Classid IN(" + Classids + ")  and Studentid IN(" + Studids + ") and CONVERT(VARCHAR(10),ModifiedOn, 120) = CONVERT(VARCHAR(10),getdate(), 120)FOR XML PATH('')), 1, 1, '') ";
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
        else
        {
            RV_DBReport.Visible = false;
            graphContainerAcademic.Visible = true;
            graphContainerAcademicStaff.Visible = true;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = true;
            //graphcontainer.Visible = true;
            String Classids = Convert.ToString(SCgraphClassid);
            String Studids = Convert.ToString(SCgraphStudid);
            String getUseridsquery = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),CreatedBy) from Behaviour where Classid IN(" + Classids + ")  and Studentid IN(" + Studids + ") and CONVERT(VARCHAR(10),ModifiedOn, 120) = CONVERT(VARCHAR(10),getdate(), 120)FOR XML PATH('')), 1, 1, '') ";
            String Userids = Convert.ToString(objData.FetchValue(getUseridsquery));
            if (Userids == "")
            {
                Userids = null;
            }
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            SqlCommand cmd = null;
            DataTable Dt = new DataTable();
            clsData ObjData = new clsData();
            SqlConnection con = ObjData.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("DashboardStaffClinicalNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@ParamClassid", SCgraphClassid);
                cmd.Parameters.AddWithValue("@ParamStudid", SCgraphStudid);
                cmd.Parameters.AddWithValue("@ParamUserid", Userids);
                da = new SqlDataAdapter(cmd);
                da.Fill(Dt);

                var grouped = Dt.AsEnumerable()
                    .GroupBy(row1 => new
                    {
                        StaffId = row1.Field<int>("StaffId"),
                        StudentId = row1.Field<int>("StudentId"),
                        MeasurementId = row1.Field<int>("MeasurementId"),
                        ClassId = row1.Field<int>("ClassId")
                    })
                    .Select(g =>
                    {
                        var first = g.First();
                        var newRow = Dt.NewRow();

                        // Copy ID 
                        newRow["StaffId"] = g.Key.StaffId;
                        newRow["StudentId"] = g.Key.StudentId;
                        newRow["MeasurementId"] = g.Key.MeasurementId;
                        newRow["ClassId"] = g.Key.ClassId;

                        // Sum MeasurementCount
                        newRow["MeasurementCount"] = g.Sum(r => r.Field<int>("MeasurementCount"));

                        // Copy other columns from the first row in the group
                        foreach (DataColumn col in Dt.Columns)
                        {
                            if (!newRow.IsNull(col.ColumnName) ||
                                col.ColumnName == "StaffId" ||
                                col.ColumnName == "StudentId" ||
                                col.ColumnName == "MeasurementId" ||
                                col.ColumnName == "ClassId" ||
                                col.ColumnName == "MeasurementCount")
                                continue;

                            newRow[col.ColumnName] = first[col.ColumnName];
                        }

                        return newRow;
                    });

                DataTable groupedDt = Dt.Clone(); // Clone the schema
                foreach (var row1 in grouped)
                    groupedDt.Rows.Add(row1.ItemArray);

                Dt = groupedDt;
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    int maxCount = Dt.AsEnumerable()
                                     .Max(row2 => row2.Field<int>("MeasurementCount"));

                    if (!Dt.Columns.Contains("MaxCount"))
                        Dt.Columns.Add("MaxCount", typeof(int));

                    foreach (DataRow row2 in Dt.Rows)
                    {
                        row2["MaxCount"] = maxCount;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);

                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + Studids + "\n" + ex.ToString());
            }
            finally
            {
                ObjData.Close(con);
            }
            foreach (DataRow dr in Dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn dc in Dt.Columns)
                {
                    row.Add(dc.ColumnName, dr[dc]);
                }
                rows.Add(row);

            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            //string script = @"setTimeout(function() {loadClinicalbyStaff('" + json.Serialize(rows) + "');}, 300);";
            string jsonString = HttpUtility.JavaScriptStringEncode(json.Serialize(rows));
            string script = string.Format("setTimeout(function() {{ loadClinicalbyStaff('{0}'); }}, 300);", jsonString);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadClinicalbyStaff", script, true);
         
        }
    }

    private void TestLoad()
    {
        RV_DBReport.Visible = false; //Shoudld be True
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
        sess = (clsSession)Session["UserSession"];
        int userId = sess.LoginId;
        string classId = ddlClassrooms.SelectedValue;
        string getStudidlistquery = "SELECT STUFF((SELECT distinct ','+Convert(varchar(200),StudentId)" +
                                        " FROM StdtClass SC left Join Student STD ON STD.StudentId = SC.StdtId left join Placement PLC on STD.StudentId = PLC.StudentPersonalId" +
                                        " WHERE SC.ClassId IN (" + classId + ") AND STD.ActiveInd='A' AND SC.ActiveInd='A' AND PLC.Location IN(" + classId + ") AND (PLC.EndDate is null or convert(DATE,PLC.EndDate) >= convert(DATE,getdate()))FOR XML PATH('')), 1, 1, '')";
        string getStudidlist = Convert.ToString(objData.FetchValue(getStudidlistquery));
        if (chkbx_block_sch.Checked == true)
        {
            graphContainerClinical.Visible = true;
            graphContainerAcademic.Visible = false;
            graphContainerAcademicStaff.Visible = false;
            graphContainerClinicalStaff.Visible = false;
            BtnClientAcademic_Click(sender, e); 
        }
        else if (chkbx_leson_deliverd.Checked == true)
        {
            //BtnSwitchTableChart.Text = "Table View";
            graphContainerAcademic.Visible = true;
            graphContainerAcademicStaff.Visible = true;
            graphContainerClinical.Visible = true;
            graphContainerClinicalStaff.Visible = true;
            highcheck.Checked = true;
            LoadDashBoardStaffAcademicGraph(classId, getStudidlist, userId);
            LoadDashBoardClientAcademicGraph(classId, getStudidlist, userId);
            LoadDashBoardStaffClinicalGraph(classId, getStudidlist);
            LoadDashBoardClientClinicalGraph(classId, getStudidlist);
        }
    }
    protected void BtnRefreshOld_Click(object sender, EventArgs e)
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
        string script1 = "disableLoader();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", script1, true);
    }

    protected void chkbx_leson_deliverd_CheckedChanged(object sender, EventArgs e)
    {
        if(chkbx_leson_deliverd.Checked == true)
        {
            chkbx_leson_deliverd.Enabled = false;
            chkbx_block_sch.Checked = false;
            chkbx_block_sch.Enabled = true;
            BtnRefresh_Click(sender, e);
        }
    }
    protected void chkbx_block_sch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbx_block_sch.Checked == true)
        {
            chkbx_block_sch.Enabled = false;            
            chkbx_leson_deliverd.Checked = false;
            chkbx_leson_deliverd.Enabled = true;
            BtnRefresh_Click(sender, e);
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
 
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAClient(string classid, string studentid, string mistrial)
    {
        clsData objdata = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        //String proc = "[dbo].[DashboardClientAcademic]";

        //DataTable dt = objdata.ReturnNewAcademicTable(proc, classid, studentid, mistrial);

        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        SqlConnection con = objdata.Open();
        SqlDataAdapter da = new SqlDataAdapter();
        cmd = new SqlCommand("DashboardClientAcademic", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 600;
        cmd.Parameters.AddWithValue("@ParamClassid", classid);
        cmd.Parameters.AddWithValue("@ParamStudid", studentid);
        cmd.Parameters.AddWithValue("@ParamMistrial", mistrial);
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);


        foreach (DataRow dr in dt.Rows)
            {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
                {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

                }

        JavaScriptSerializer json = new JavaScriptSerializer();
        string dat = json.Serialize(rows);
        return json.Serialize(rows);
            }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAClientPerc(string classid, string studentid)
        {
        clsData objdata = new clsData();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        //String proc = "[dbo].[DashboardClientAcademicPercentage]";

        //DataTable dt = objdata.ReturnNewAcademicTablePerc(proc, classid, studentid);

        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        SqlConnection con = objdata.Open();
        SqlDataAdapter da = new SqlDataAdapter();
        cmd = new SqlCommand("DashboardClientAcademicPercentage", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 600;
        cmd.Parameters.AddWithValue("@ParamClassid", classid);
        cmd.Parameters.AddWithValue("@ParamStudid", studentid);
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        foreach (DataRow dr in dt.Rows)
            {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
                {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

                }

        JavaScriptSerializer json = new JavaScriptSerializer();
        string dat = json.Serialize(rows);
        return json.Serialize(rows);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getClientClinic(string cid, string sid)
            {
        clsData objData = new clsData();
        string str = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        SqlConnection cn = new SqlConnection(str);
        cn.Open();

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        //String proc = "[dbo].[DashboardClientClinicalNew]";

        //DataTable dt = objData.ReturnNewTableClinicClient(proc, cid, sid);

        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        SqlConnection con = objData.Open();
        SqlDataAdapter da = new SqlDataAdapter();
        cmd = new SqlCommand("DashboardClientClinicalNew", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 600;
        cmd.Parameters.AddWithValue("@ParamClassid", cid);
        cmd.Parameters.AddWithValue("@ParamStudid", sid);
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        if (dt != null && dt.Rows.Count > 0)
        {
            var grouped = dt.AsEnumerable()
                .GroupBy(row1 => new
                {
                    StudentId = row1.Field<int>("StudentId"),
                    MeasurementId = row1.Field<int>("MeasurementId"),
                    ClassId = row1.Field<int>("ClassId")
                })
                .Select(g =>
                {
                    var first = g.First();
                    var newRow = dt.NewRow();

                    newRow["StudentId"] = g.Key.StudentId;
                    newRow["MeasurementId"] = g.Key.MeasurementId;
                    newRow["ClassId"] = g.Key.ClassId;
                    newRow["BehaviourSession"] = g.Sum(r => r.Field<int>("BehaviourSession"));

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName == "StudentId" ||
                            col.ColumnName == "MeasurementId" ||
                            col.ColumnName == "ClassId" ||
                            col.ColumnName == "BehaviourSession")
                            continue;

                        newRow[col.ColumnName] = first[col.ColumnName];
                    }

                    return newRow;
                });

            DataTable groupedDt = dt.Clone(); // Clone the schema
            foreach (var row1 in grouped)
                groupedDt.Rows.Add(row1.ItemArray);

            dt = groupedDt;

            // Add MaxCount column
            int maxCount = dt.AsEnumerable()
                             .Max(row2 => row2.Field<int>("BehaviourSession"));

            if (!dt.Columns.Contains("MaxCount"))
                dt.Columns.Add("MaxCount", typeof(int));

            foreach (DataRow row2 in dt.Rows)
            {
                row2["MaxCount"] = maxCount;
            }
        }
        else
        {
            return "[]"; // or return an error object
        }

        // Convert DataTable to JSON
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);
        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        string dat = json.Serialize(rows);
        Debug.WriteLine(dat);
        return json.Serialize(rows);
    }


    protected void ButtonGo_Click(object sender, EventArgs e)
    {
        RV_DBReport.Visible = false;
        String Classids = Convert.ToString(Txt_Clasid.Text);
        String Studids = Convert.ToString(Txt_Studid.Text);
        string startDate = txtstartDate.Text.ToString();
        string endDate = txtendDate.Text.ToString();
        String Mistrial = Convert.ToString(chkbx_Mistrial.Checked ? 1 : 0);
        String getUserIdAcademic = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),Modifiedby) from stdtsessionhdr where stdtclassid IN(" + Classids + ") and studentid IN(" + Studids + ") and Convert(DATE,ModifiedOn) BETWEEN CONVERT(DATE,'" + startDate + "') AND CONVERT(DATE,'" + endDate + "')FOR XML PATH('')), 1, 1, '')";
        String getUserIdClinical = "SELECT STUFF((SELECT Distinct ','+Convert(varchar(200),Modifiedby) from Behaviour where ClassId IN(" + Classids + ") and studentid IN(" + Studids + ") and Convert(DATE,CreatedOn) BETWEEN CONVERT(DATE,'" + startDate + "') AND CONVERT(DATE,'" + endDate + "')FOR XML PATH('')), 1, 1, '')";
        
        String UserIdsAcademic = Convert.ToString(objData.FetchValue(getUserIdAcademic));
        String UserIdClinical = Convert.ToString(objData.FetchValue(getUserIdClinical));
        String Userids = string.IsNullOrEmpty(UserIdsAcademic)
                 ? UserIdClinical
                 : string.IsNullOrEmpty(UserIdClinical)
                   ? UserIdsAcademic
                   : UserIdsAcademic + "," + UserIdClinical;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        DataTable Dt = new DataTable();
        clsData ObjData = new clsData();
        SqlConnection con = ObjData.Open();
        
        try
        {
            gvProgramsByClient.Columns.Clear();
            gvProgramsByClient.DataSource = null;
            gvProgramsByClient.DataBind();

            gvProgramsByStaff.Columns.Clear();
            gvProgramsByStaff.DataSource = null;
            gvProgramsByStaff.DataBind();

            gvClinicalByClient.Columns.Clear();
            gvClinicalByClient.DataSource = null;
            gvClinicalByClient.DataBind();

            gvClinicalByStaff.Columns.Clear();
            gvClinicalByStaff.DataSource = null;
            gvClinicalByStaff.DataBind();


            DataTable DtAcademic = new DataTable();
            SqlCommand cmdAcademic = new SqlCommand("DashboardAcademicTable", con);
            cmdAcademic.CommandType = CommandType.StoredProcedure;
            cmdAcademic.CommandTimeout = 600;
            cmdAcademic.Parameters.AddWithValue("@ParamStartDate", startDate);
            cmdAcademic.Parameters.AddWithValue("@ParamEndDate", endDate);
            cmdAcademic.Parameters.AddWithValue("@StudentIds", Studids);
            cmdAcademic.Parameters.AddWithValue("@UserIds", Userids);
            cmdAcademic.Parameters.AddWithValue("@ParamMistrial", Mistrial);
            cmdAcademic.Parameters.AddWithValue("@ClassIds", Classids);
            SqlDataAdapter daAcademic = new SqlDataAdapter(cmdAcademic);
            daAcademic.Fill(DtAcademic);
            
            DataTable dtAcademicStaff = PivotDataTableStaff(DtAcademic);
            DtAcademic = PivotDataTableClient(DtAcademic);

            DtAcademic = fillDates(DtAcademic, startDate, endDate);
            dtAcademicStaff = fillDates(dtAcademicStaff, startDate, endDate);

            AddDynamicColumnsClientAcademic(DtAcademic);
            AddDynamicColumnsStaffAcademic(dtAcademicStaff);
            
            gvProgramsByClient.DataSource = DtAcademic;
            gvProgramsByClient.DataBind();

            gvProgramsByStaff.DataSource = dtAcademicStaff;
            gvProgramsByStaff.DataBind();

            DataTable DtClinical = new DataTable();
            SqlCommand cmdClinical = new SqlCommand("DashboardClinicalTable", con);
            cmdClinical.CommandType = CommandType.StoredProcedure;
            cmdClinical.CommandTimeout = 600;
            cmdClinical.Parameters.AddWithValue("@ParamStartDate", startDate);
            cmdClinical.Parameters.AddWithValue("@ParamEndDate", endDate);
            cmdClinical.Parameters.AddWithValue("@StudentIds", Studids);
            cmdClinical.Parameters.AddWithValue("@UserIds", Userids);
            cmdClinical.Parameters.AddWithValue("@ClassIds", Classids);
            SqlDataAdapter daClinical = new SqlDataAdapter(cmdClinical);
            daClinical.Fill(DtClinical);
            DtClinical = addCountPerDate(DtClinical);
            DataTable dtClinicalStaff = PivotDataTableStaff(DtClinical);
            DtClinical = PivotDataTableClient(DtClinical);

            DtClinical = fillDates(DtClinical, startDate, endDate);
            dtClinicalStaff = fillDates(dtClinicalStaff, startDate, endDate);


            AddDynamicColumnsClientClinical(DtClinical);
            AddDynamicColumnsStaffClinical(dtClinicalStaff);

            gvClinicalByClient.DataSource = DtClinical;
            gvClinicalByClient.DataBind();

            gvClinicalByStaff.DataSource = dtClinicalStaff;
            gvClinicalByStaff.DataBind();

            //ddcb_clas.SelectedIndex = 0;
            highcheck.Visible = false;
            BtnRefresh.Visible = false;
            ButtonGo.Visible = true;
            btnExportToExcel.Visible = true;
            //BtnChartView.Visible = true;
            //BtnTableView.Visible = false;
            startDate = startDate.Replace('/', '-');
            endDate = endDate.Replace('/', '-');
            string script2 = "setDateFields('" + startDate + "', '" + endDate + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setDate", script2, true);
            string script1 = "TableView();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Table", script1, true);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);

            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n StudentId ID = " + Studids + "\n" + ex.ToString());
        }
        finally
        {
            ObjData.Close(con);
            string script1 = "disableLoader();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", script1, true);
        }

    }
    static DataTable addCountPerDate(DataTable dt)
    {
        var groupedData = dt.AsEnumerable()
             .GroupBy(row => new
             {
                 CreatedDate = row.Field<DateTime>("CreatedDate"),
                 StudentName = row.Field<string>("StudentName"),
                 ClassName = row.Field<string>("ClassName"),
                 StaffName = row.Field<string>("StaffName")
             })
             .Select(g => new
             {
                 CreatedDate = g.Key.CreatedDate,
                 StudentName = g.Key.StudentName,
                 ClassName = g.Key.ClassName,
                 StaffName = g.Key.StaffName,
                 TotalCount = g.Sum(r => r.Field<int>("Count"))
             })
             .ToList();

        // Step 2: Clear the original DataTable and repopulate it
        dt.Clear();

        foreach (var row in groupedData)
        {
            dt.Rows.Add(row.StudentName, row.ClassName, row.StaffName, row.TotalCount, row.CreatedDate);
        }
        return dt;

    }
    static DataTable PivotDataTableClient(DataTable dt)
    {
        dt.DefaultView.Sort = dt.Columns["ClassName"].ColumnName + " ASC";
        dt = dt.DefaultView.ToTable();
        DataTable result = new DataTable();
        result.Columns.Add("ClassName", typeof(string));
        result.Columns.Add("StudentName", typeof(string));

        // Get unique CreatedDate values as column headers
        List<string> dateColumns = dt.AsEnumerable()
            .Select(row => Convert.ToDateTime(row["CreatedDate"]).ToString("MM-dd-yyyy"))
            .Distinct()
            .OrderBy(date => date)
            .ToList();

        foreach (string date in dateColumns)
        {
            result.Columns.Add(date, typeof(int));
        }

        result.Columns.Add("Grand Total", typeof(int)); // Add Grand Total column

        Dictionary<string, DataRow> classAggregates = new Dictionary<string, DataRow>();
        Dictionary<string, Dictionary<string, int>> classSums = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, DataRow> rowLookup = new Dictionary<string, DataRow>();

        Dictionary<string, int> grandTotalValues = new Dictionary<string, int>(); // Store grand total for dates
        int overallGrandTotal = 0;

        foreach (DataRow row in dt.Rows)
        {
            string className = row["ClassName"].ToString();
            string studentName = row["StudentName"].ToString();
            string createdDate = Convert.ToDateTime(row["CreatedDate"]).ToString("MM-dd-yyyy");
            int countValue = Convert.ToInt32(row["Count"]);

            string studentKey = className + "_" + studentName;

            // Handle class-level aggregation
            if (!classAggregates.ContainsKey(className))
            {
                DataRow classRow = result.NewRow();
                classRow["ClassName"] = className;
                classRow["StudentName"] = "Total";
                classAggregates[className] = classRow;
                result.Rows.Add(classRow);
                classSums[className] = new Dictionary<string, int>();
            }

            if (!classSums[className].ContainsKey(createdDate))
            {
                classSums[className][createdDate] = 0;
            }
            classSums[className][createdDate] += countValue;

            // Handle individual student rows
            if (!rowLookup.ContainsKey(studentKey))
            {
                DataRow studentRow = result.NewRow();
                studentRow["ClassName"] = className;
                studentRow["StudentName"] = studentName;
                rowLookup[studentKey] = studentRow;
                result.Rows.Add(studentRow);
            }


            if (rowLookup[studentKey][createdDate] == DBNull.Value)
            {
                rowLookup[studentKey][createdDate] = countValue;
            }
            else
            {
                rowLookup[studentKey][createdDate] = Convert.ToInt32(rowLookup[studentKey][createdDate]) + countValue;
            }

            // Track grand total values per date (excluding class summary rows)
            if (!grandTotalValues.ContainsKey(createdDate))
            {
                grandTotalValues[createdDate] = 0;
            }
            grandTotalValues[createdDate] += countValue;
            overallGrandTotal += countValue; // Overall sum
        }

        // Populate class summary rows
        foreach (var classEntry in classSums)
        {
            string className = classEntry.Key;
            DataRow classRow = classAggregates[className];
            int classGrandTotal = 0;

            foreach (var dateEntry in classEntry.Value)
            {
                classRow[dateEntry.Key] = dateEntry.Value;
                classGrandTotal += dateEntry.Value;
            }

            classRow["Grand Total"] = classGrandTotal;
        }

        // Compute row-wise Grand Total for each student
        foreach (var studentRow in rowLookup.Values)
        {
            int studentGrandTotal = 0;
            foreach (string date in dateColumns)
            {
                if (studentRow[date] != DBNull.Value)
                {
                    studentGrandTotal += Convert.ToInt32(studentRow[date]);
                }
            }
            studentRow["Grand Total"] = studentGrandTotal;
        }

        // Add final Grand Total row
        DataRow grandTotalRow = result.NewRow();
        grandTotalRow["ClassName"] = "";
        grandTotalRow["StudentName"] = "Grand Total";

        foreach (string date in dateColumns)
        {
            grandTotalRow[date] = grandTotalValues.ContainsKey(date) ? grandTotalValues[date] : 0;
        }

        grandTotalRow["Grand Total"] = overallGrandTotal;
        result.Rows.Add(grandTotalRow);

        return result;
    }




    static DataTable PivotDataTableStaff(DataTable dt)
    {
        dt.DefaultView.Sort = dt.Columns["ClassName"].ColumnName + " ASC";
        dt = dt.DefaultView.ToTable();
        DataTable result = new DataTable();
        result.Columns.Add("ClassName", typeof(string));
        result.Columns.Add("StaffName", typeof(string));

        // Get unique CreatedDate values as column headers
        List<string> dateColumns = dt.AsEnumerable()
            .Select(row => Convert.ToDateTime(row["CreatedDate"]).ToString("MM-dd-yyyy"))
            .Distinct()
            .OrderBy(date => date)
            .ToList();

        foreach (string date in dateColumns)
        {
            result.Columns.Add(date, typeof(int));
        }

        result.Columns.Add("Grand Total", typeof(int)); // Add Grand Total column

        Dictionary<string, DataRow> classAggregates = new Dictionary<string, DataRow>();
        Dictionary<string, Dictionary<string, int>> classSums = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, DataRow> rowLookup = new Dictionary<string, DataRow>();

        Dictionary<string, int> grandTotalValues = new Dictionary<string, int>(); // Store grand total for dates
        int overallGrandTotal = 0;

        foreach (DataRow row in dt.Rows)
        {
            string className = row["ClassName"].ToString();
            string staffName = row["StaffName"].ToString();
            string createdDate = Convert.ToDateTime(row["CreatedDate"]).ToString("MM-dd-yyyy");
            int countValue = Convert.ToInt32(row["Count"]);

            string staffKey = className + "_" + staffName;

            // Handle class-level aggregation
            if (!classAggregates.ContainsKey(className))
            {
                DataRow classRow = result.NewRow();
                classRow["ClassName"] = className;
                classRow["StaffName"] = "Total";
                classAggregates[className] = classRow;
                result.Rows.Add(classRow);
                classSums[className] = new Dictionary<string, int>();
            }

            if (!classSums[className].ContainsKey(createdDate))
            {
                classSums[className][createdDate] = 0;
            }
            classSums[className][createdDate] += countValue;

            // Handle individual staff rows
            if (!rowLookup.ContainsKey(staffKey))
            {
                DataRow staffRow = result.NewRow();
                staffRow["ClassName"] = className;
                staffRow["StaffName"] = staffName;
                rowLookup[staffKey] = staffRow;
                result.Rows.Add(staffRow);
            }

            if (rowLookup[staffKey][createdDate] == DBNull.Value)
            {
                rowLookup[staffKey][createdDate] = countValue;
            }
            else
            {
                rowLookup[staffKey][createdDate] = Convert.ToInt32(rowLookup[staffKey][createdDate]) + countValue;
            }

            // Track grand total values per date (excluding class summary rows)
            if (!grandTotalValues.ContainsKey(createdDate))
            {
                grandTotalValues[createdDate] = 0;
            }
            grandTotalValues[createdDate] += countValue;
            overallGrandTotal += countValue; // Overall sum
        }

        // Populate class summary rows
        foreach (var classEntry in classSums)
        {
            string className = classEntry.Key;
            DataRow classRow = classAggregates[className];
            int classGrandTotal = 0;

            foreach (var dateEntry in classEntry.Value)
            {
                classRow[dateEntry.Key] = dateEntry.Value;
                classGrandTotal += dateEntry.Value;
            }

            classRow["Grand Total"] = classGrandTotal;
        }

        // Compute row-wise Grand Total for each staff member
        foreach (var staffRow in rowLookup.Values)
        {
            int staffGrandTotal = 0;
            foreach (string date in dateColumns)
            {
                if (staffRow[date] != DBNull.Value)
                {
                    staffGrandTotal += Convert.ToInt32(staffRow[date]);
                }
            }
            staffRow["Grand Total"] = staffGrandTotal;
        }

        // Add final Grand Total row
        DataRow grandTotalRow = result.NewRow();
        grandTotalRow["ClassName"] = "";
        grandTotalRow["StaffName"] = "Grand Total";

        foreach (string date in dateColumns)
        {
            grandTotalRow[date] = grandTotalValues.ContainsKey(date) ? grandTotalValues[date] : 0;
        }

        grandTotalRow["Grand Total"] = overallGrandTotal;
        result.Rows.Add(grandTotalRow);

        return result;
    }
    protected void gvProgramsByClient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string rawClassName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ClassName"));
            string className = rawClassName.Replace(" ", "_");
            string studentName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StudentName"));

            e.Row.Attributes["class"] = "class-" + className;
            e.Row.Attributes["data-class"] = className;
            e.Row.Attributes["data-raw-class"] = rawClassName;

            bool isSummaryRow = studentName == "Total";

            if (isSummaryRow)
            {
                e.Row.Attributes["class"] += " summary-row";
                string buttonHtml = "<a href='javascript:void(0);' class='collapse-button' onclick=\"toggleRows('" + className + "')\" style='font-weight:bold; text-decoration:none; color:black;'>&#9660; " + className.Replace("_", " ") + "</a>";
                e.Row.Cells[0].Text = buttonHtml;
                e.Row.Font.Bold = true;
            }
            else
            {
                if (studentName == "Grand Total")
                    e.Row.Font.Bold = true;
                e.Row.Cells[0].Text = "";
            }
        }
    }

    protected void gvProgramsByStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string rawClassName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ClassName"));
            string className = rawClassName.Replace(" ", "_");
            string staffName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StaffName"));

            e.Row.Attributes["class"] = "class-" + className;
            e.Row.Attributes["data-class"] = className;
            e.Row.Attributes["data-raw-class"] = rawClassName;

            bool isSummaryRow = staffName == "Total";

            if (isSummaryRow)
            {
                e.Row.Attributes["class"] += " summary-row";
                string buttonHtml = "<a href='javascript:void(0);' class='collapse-button' onclick=\"toggleRows('" + className + "')\" style='font-weight:bold; text-decoration:none; color:black;'>&#9660; " + className.Replace("_", " ") + "</a>";
                e.Row.Cells[0].Text = buttonHtml;
                e.Row.Font.Bold = true;
            }
            else
            {
                if (staffName == "Grand Total")
                    e.Row.Font.Bold = true;
                e.Row.Cells[0].Text = "";
            }
        }
    }

    protected void gvClinicalByClient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string rawClassName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ClassName"));
            string className = rawClassName.Replace(" ", "_");
            string studentName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StudentName"));

            e.Row.Attributes["class"] = "class-" + className;
            e.Row.Attributes["data-class"] = className;
            e.Row.Attributes["data-raw-class"] = rawClassName;

            bool isSummaryRow = studentName == "Total";

            if (isSummaryRow)
            {
                e.Row.Attributes["class"] += " summary-row";
                string buttonHtml = "<a href='javascript:void(0);' class='collapse-button' onclick=\"toggleRows('" + className + "')\" style='font-weight:bold; text-decoration:none; color:black;'>&#9660; " + className.Replace("_", " ") + "</a>";
                e.Row.Cells[0].Text = buttonHtml;
                e.Row.Font.Bold = true;
            }
            else
            {
                if (studentName == "Grand Total")
                    e.Row.Font.Bold = true;
                e.Row.Cells[0].Text = "";
            }
        }
    }

    protected void gvClinicalByStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string rawClassName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ClassName"));
            string className = rawClassName.Replace(" ", "_");
            string staffName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StaffName"));

            e.Row.Attributes["class"] = "class-" + className;
            e.Row.Attributes["data-class"] = className;
            e.Row.Attributes["data-raw-class"] = rawClassName;

            bool isSummaryRow = staffName == "Total";

            if (isSummaryRow)
            {
                e.Row.Attributes["class"] += " summary-row";
                string buttonHtml = "<a href='javascript:void(0);' class='collapse-button' onclick=\"toggleRows('" + className + "')\" style='font-weight:bold; text-decoration:none; color:black;'>&#9660; " + className.Replace("_", " ") + "</a>";
                e.Row.Cells[0].Text = buttonHtml;
                e.Row.Font.Bold = true;
            }
            else
            {
                if (staffName == "Grand Total")
                    e.Row.Font.Bold = true;
                e.Row.Cells[0].Text = "";
            }
        }
    }



    private void AddDynamicColumnsClientAcademic(DataTable dt)
    {
        foreach (DataColumn col in dt.Columns)
        {
            if (col.ColumnName == "StudentName")
            {
                BoundField StudentName = new BoundField();
                StudentName.DataField = col.ColumnName;
                StudentName.HeaderText = "Student Name";
                gvProgramsByClient.Columns.Add(StudentName);
            }
            else if (col.ColumnName == "ClassName")
            {
                BoundField ClassName = new BoundField();
                ClassName.DataField = col.ColumnName;
                ClassName.HeaderText = "Class Name";
                gvProgramsByClient.Columns.Add(ClassName);
            }
            else
            {
                BoundField dateColumn = new BoundField();
                dateColumn.DataField = col.ColumnName;
                dateColumn.HeaderText = col.ColumnName;
                dateColumn.HeaderStyle.CssClass = "rotated-header";
                gvProgramsByClient.Columns.Add(dateColumn);
            }
        }
    }
    private void AddDynamicColumnsClientClinical(DataTable dt)
    {
        foreach (DataColumn col in dt.Columns)
        {
            if (col.ColumnName == "StudentName")
            {
                BoundField StudentName = new BoundField();
                StudentName.DataField = col.ColumnName;
                StudentName.HeaderText = "Student Name";
                gvClinicalByClient.Columns.Add(StudentName);
            }
            else if (col.ColumnName == "ClassName")
            {
                BoundField ClassName = new BoundField();
                ClassName.DataField = col.ColumnName;
                ClassName.HeaderText = "Class Name";
                gvClinicalByClient.Columns.Add(ClassName);
            }
            else
            {
                BoundField dateColumn = new BoundField();
                dateColumn.DataField = col.ColumnName;
                dateColumn.HeaderText = col.ColumnName;
                dateColumn.HeaderStyle.CssClass = "rotated-header";
                gvClinicalByClient.Columns.Add(dateColumn);
            }
        }
    }
    private void AddDynamicColumnsStaffAcademic(DataTable dt)
    {
        foreach (DataColumn col in dt.Columns)
        {
            if (col.ColumnName == "StaffName")
            {
                BoundField StaffName = new BoundField();
                StaffName.DataField = col.ColumnName;
                StaffName.HeaderText = "Staff Name";
                gvProgramsByStaff.Columns.Add(StaffName);
            }
            else if (col.ColumnName == "ClassName")
            {
                BoundField ClassName = new BoundField();
                ClassName.DataField = col.ColumnName;
                ClassName.HeaderText = "Class Name";
                gvProgramsByStaff.Columns.Add(ClassName);
            }
            else
            {
                BoundField dateColumn = new BoundField();
                dateColumn.DataField = col.ColumnName;
                dateColumn.HeaderText = col.ColumnName;
                dateColumn.HeaderStyle.CssClass = "rotated-header";
                gvProgramsByStaff.Columns.Add(dateColumn);
            }
        }
    }
    private void AddDynamicColumnsStaffClinical(DataTable dt)
    {
        foreach (DataColumn col in dt.Columns)
        {
            if (col.ColumnName == "StaffName")
            {
                BoundField StaffName = new BoundField();
                StaffName.DataField = col.ColumnName;
                StaffName.HeaderText = "Staff Name";
                gvClinicalByStaff.Columns.Add(StaffName);
            }
            else if (col.ColumnName == "ClassName")
            {
                BoundField ClassName = new BoundField();
                ClassName.DataField = col.ColumnName;
                ClassName.HeaderText = "Class Name";
                gvClinicalByStaff.Columns.Add(ClassName);
            }
            else
            {
                BoundField dateColumn = new BoundField();
                dateColumn.DataField = col.ColumnName;
                dateColumn.HeaderText = col.ColumnName;
                dateColumn.HeaderStyle.CssClass = "rotated-header";
                gvClinicalByStaff.Columns.Add(dateColumn);
            }
        }
    }
    //protected void BtnSwitchTableChart_Click(object sender, EventArgs e)
    //{
    //    highcheck.Visible = true;
    //    BtnRefresh.Visible = true;
    //    ButtonGo.Visible = false;
    //    BtnChartView.Visible = false;
    //    BtnTableView.Visible = true;
    //    string script1 = "ChartView();";
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "chart", script1, true);
    //}
    protected void BtnSwitchTableChart_Click(object sender, EventArgs e)
    {
        if (BtnSwitchTableChart.Text == "Table View")
        {
            sess = (clsSession)Session["UserSession"];
            string selectedClasses = sess.Classid.ToString();

            foreach (System.Web.UI.WebControls.ListItem item in ddcb_clas.Items)
            {
                if (selectedClasses.Contains(item.Value))
                {
                    item.Selected = true;
                }
            }
            Txt_Clasid.Text = selectedClasses;
            FillStudentDropdown(selectedClasses);
            BtnSwitchTableChart.Text = "Chart View";

            txtstartDate.Text = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
            txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");


            ButtonGo_Click(sender, e);
        }
        else
        {
            BtnRefresh.Visible = true;
            ButtonGo.Visible = false;
            btnExportToExcel.Visible = false;
            BtnSwitchTableChart.Text = "Table View";
            string script1 = "ChartView();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "chart", script1, true);
            BtnRefresh_Click(sender, e);
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=DashboardExport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        Response.ContentEncoding = System.Text.Encoding.UTF8;

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                sw.WriteLine("<b>Programs by Client</b><br/>");
                gvProgramsByClient.RenderControl(hw);

                sw.WriteLine("<br/><b>Programs by Staff</b><br/>");
                gvProgramsByStaff.RenderControl(hw);

                sw.WriteLine("<br/><b>Clinical by Client</b><br/>");
                gvClinicalByClient.RenderControl(hw);

                sw.WriteLine("<br/><b>Clinical by Staff</b><br/>");
                gvClinicalByStaff.RenderControl(hw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }

    static DataTable fillDates(DataTable originalDt, string startDateStr, string endDateStr)
    {
        DateTime startDate = DateTime.ParseExact(startDateStr.Replace("/","-"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(endDateStr.Replace("/", "-"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
        
        DataTable dtFullDates = new DataTable();
        dtFullDates.Columns.Add(originalDt.Columns[0].ColumnName, originalDt.Columns[0].DataType);
        dtFullDates.Columns.Add(originalDt.Columns[1].ColumnName, originalDt.Columns[1].DataType);

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            dtFullDates.Columns.Add(date.ToString("MM-dd-yyyy"), typeof(string));
        }

        int lastColIndex = originalDt.Columns.Count - 1;
        dtFullDates.Columns.Add(originalDt.Columns[lastColIndex].ColumnName, originalDt.Columns[lastColIndex].DataType);
        foreach (DataRow row in originalDt.Rows)
        {
            DataRow newRow = dtFullDates.NewRow();

            newRow[0] = row[0];
            newRow[1] = row[1];

            foreach (DataColumn dateColumn in dtFullDates.Columns)
            {
                string dateColumnName = dateColumn.ColumnName;

                if (originalDt.Columns.Contains(dateColumnName))
                {
                    newRow[dateColumnName] = row[dateColumnName];
                }
                else
                {
                    newRow[dateColumnName] = ""; 
                }
            }

            newRow[dtFullDates.Columns.Count - 1] = row[lastColIndex];

            dtFullDates.Rows.Add(newRow);
        }

        return dtFullDates;
 
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }

}


