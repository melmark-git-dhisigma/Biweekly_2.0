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
using System.Net;
using System.IO;

public partial class Home : System.Web.UI.Page
{
    static string _studID = "";
    public static bool frequency;
    public static string frequencycnt;
    public static bool status;
    public static clsData objData = null;
    public static clsSession sess = null;
    public static clsSession Prevsess = null;
    public static clsRoles role = null;
    public static DataClass objdataClass = null;
    public static ClsTemplateSession ObjTempSess = null;
    public static clsStatus objStatus = null;
    public clsRoles objRole = null;
    public static Dictionary<int, int> Remainder = null;

    clsGeneral clsGeneralSchk = new clsGeneral();
    public static DateTime now = DateTime.Now;
    public static DateTime today = DateTime.Today;
    string curesesid = "";
    int preid = 0;
    string preuser = "";
    string Sessstname = "";
    int SessStudentid = 0;
    int PreClassid = 0;
    string Pagepath = "";
    string HostName = "";
    string userAgent = "";
    string ip = "";
    int classchk = 0;


    bool Datasheet = false;
    bool Edit = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Remainder = new Dictionary<int,int>();
            sess = (clsSession)Session["UserSession"];
            if (sess == null)
            {
                Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
            }
            else
            {
                bool flag = clsGeneral.PageIdentification(sess.perPage);
                if (flag == false)
                {
                    Response.Redirect("~/Administration/Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
                }
            }


            if (!IsPostBack)
            {
                sess = (clsSession)Session["UserSession"];
                Prevsess = sess;
                Session["PreSession"] = Prevsess;
                Session["Spreid"] = Prevsess.LoginId;
                Session["Spreuser"] = Prevsess.UserName;
                Session["Sprestid"] = Prevsess.StudentId;
                Session["Sprestname"] = Prevsess.StudentName;
                Session["SpreClsid"] = Prevsess.Classid;
                Prevsess = (clsSession)Session["PreSession"];
                preid = Convert.ToInt16(Session["Spreid"]);
                preuser=Session["Spreuser"].ToString();
                SessStudentid = Convert.ToInt16(Session["Sprestid"]);
                Sessstname = Session["Sprestname"].ToString();
                PreClassid = Convert.ToInt16(Session["SpreClsid"]);
                curesesid = this.Session.SessionID.ToString();
                
                ClsListLog SesList = new ClsListLog();
                Pagepath = "Home:PageloadFillBef";
                SesList.WriteToLog(DateTime.Now.ToString() + ',' + sess.LoginTime.ToString() + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + classchk + ',' + PreClassid);

                ip = clsGeneral.GetIPAddress();
                int count = 3;
                classchk = 0;
                FillStudentList();
                while(count>0)
                {
                    if (classchk != PreClassid)
                    {
                        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);              
                        FillStudentList();
                        count--;
                    }
                    else
                        break;
                }
                if (classchk != PreClassid)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('There is an issue related to the session Data Please logout and Sign in Again...');", true);
                   
                }
                _studID = "";
                sess.AdminView = 2;
                setTitle();
                Loadfillclass();
                objRole = new clsRoles();

                objRole.DatasheetAndEdit(sess.LoginId, sess.SchoolId, out Datasheet, out Edit);
                menu.InnerHtml = "<ul>" + clsRoles.BinderPageLister(sess.perPageBinder, Datasheet, Edit) + "</ul>";

                clsData oData = new clsData();
                string selRole = "SELECT Role.DashBrdType FROM Role " +
                            "INNER JOIN (RoleGroup " +
                            "INNER JOIN (UserRoleGroup UGrp " +
                            "INNER JOIN [User] " +
                            "ON UGrp.UserId=[User].UserId) " +
                            "ON UGrp.RoleGroupId=RoleGroup.RoleGroupId) " +
                            "ON Role.RoleId = RoleGroup.RoleId " +
                            "WHERE UGrp.UserId = " + sess.LoginId;
                object dash = oData.FetchValue(selRole);
                if (dash != null)
                {
                    hfDashType.Value = dash.ToString();
                }
                LBLClassnotfound.Text = "";
                updateAlerts();
            }

            if (sess != null)
            {
                lblLoginName.Text = sess.UserName;
            }
           
            if (sess.Classid == classIdCheck())
            {
                btnDischarge.Visible = true;
            }
            else
            {
                btnDischarge.Visible = false;
            }
            preid = Convert.ToInt16(Session["Spreid"]);
            preuser = Session["Spreuser"].ToString();
            SessStudentid = Convert.ToInt16(Session["Sprestid"]);
            Sessstname = Session["Sprestname"].ToString();
            curesesid = this.Session.SessionID.ToString();

            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "Home:Page_Load";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
                    
            if (preuser != null)
            {
                lblLoginName.Text = preuser;
            }
        }
        catch (Exception Ex)
        {
            Response.Redirect("~/Administration/Error.aspx?Error=" + Ex.Message.ToString() + "");
        }
    }
    public int Index()
    {
        if (Session["UpdateAlerts"] == null)
        {
            Session["UpdateAlerts"] = DateTime.Now;
            return 1;
        }
        return 0;
    }
    private void updateAlerts()
    {
        int alertdelete = Index();
        if (alertdelete.Equals(1))
        {
            clsData oData = new clsData();
            string selQry = "SELECT distinct StudentId as Id FROM StdtClass stdtcls Inner Join Student stdt ON stdt.StudentId=stdtcls.StdtId"+
                " INNER JOIN placement plc ON stdt.studentid = plc.studentpersonalid AND plc.location = stdtcls.classid WHERE stdtcls.ClassId='" +
                sess.Classid + "' AND stdt.ActiveInd='A' And stdtcls.ActiveInd='A' AND ( plc.enddate IS NULL OR CONVERT(DATE, plc.enddate) >= CONVERT(DATE, Getdate()))";
            DataTable dt = new DataTable();
            dt = oData.ReturnDataTable(selQry, false);
            string studentIds = "";

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string StudID = row["Id"].ToString();
                    studentIds += "'" + StudID + "',";
                }
            }
            if (studentIds != null && studentIds.Length > 0)
            {
                studentIds = studentIds.Substring(0, studentIds.Length - 1);
                string deRemider = " DELETE FROM BehaviorReminder WHERE StudentId IN(" + studentIds + ")";  // UserId=" + sess.LoginId + " and
                oData.Execute(deRemider);
            }
            alertdelete = 2;
        }
    }
    private void setTitle()
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        string ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "Home:setTitle";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        if (sess != null)
        {
            object obj = objData.FetchValue("Select SchoolDesc from School Where SchoolId=" + sess.SchoolId + "");
            if (obj != null)
            {
                TitleName.Text = obj.ToString();
            }
        }
    }
    private void Loadfillclass()
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        string ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "Home:Loadfillclass";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        if (sess == null) return;
        string LblClassName = "";

        if (sess.Classid != 0)
        {
            if (objData.FetchValue("Select ClassName from Class Where ClassId='" + sess.Classid + "'") != null)
            {
                LblClassName = objData.FetchValue("Select ClassName from Class Where ClassId='" + sess.Classid + "'").ToString();
            }
        }

        if (LblClassName.Length > 10)
            LblClass.Text = LblClassName.Substring(0, 8) + "..";
        else
            LblClass.Text = LblClassName;
    }
    private void TemplateSession()
    {
        ObjTempSess = new ClsTemplateSession();
        if (_studID.ToString() != "")
        {
            setTemplateSess(_studID);
            Session["BiweeklySession"] = ObjTempSess;
        }

    }
    private void FillStudentList()
    {
        clsData oData = new clsData();
        studentContainer.InnerHtml = " ";
        newBehStdList.InnerHtml = " ";

        sess = (clsSession)Session["UserSession"];

        ClsListLog SesList = new ClsListLog();
        Pagepath = "Home:FillstudFillBef";
        SesList.WriteToLog(DateTime.Now.ToString() + ',' + sess.LoginTime.ToString() + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid  + ',' + PreClassid);
                


        string ip = clsGeneral.GetIPAddress();
        curesesid = this.Session.SessionID.ToString();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "Home:FillStudentList";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        if (sess != null)
        {
            string name = "";
            string selQry = "";
            string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
            classchk = sess.Classid;

            if (buildName == "Local")
            {
                if (sess.Classid == classIdCheck())
                {
                    selQry = "SELECT DISTINCT StudentPersonalId as Id, (FirstName + ' ' + LastName) as Name, ImageUrl FROM StudentPersonal where StudentPersonalId = '" + sess.StudentId + "'";
                }
                else
                {
                    selQry = "SELECT distinct StudentId as Id,(StudentFname+' '+StudentLname) as Name,ImageURL FROM StdtClass stdtcls left Join Student stdt ON stdt.StudentId=stdtcls.StdtId left join Placement plc on stdt.StudentId=plc.StudentPersonalId WHERE stdtcls.ClassId='" + sess.Classid + "' AND stdt.ActiveInd='A' And  stdt.SchoolId='" + sess.SchoolId + "' And stdtcls.ActiveInd='A' AND plc.Location='" + sess.Classid + "' AND plc.status='1' AND (plc.EndDate is null or convert(DATE,plc.EndDate) >= convert(DATE,getdate()))";
                }
            }
            else
            {
                if (sess.Classid == classIdCheck())
                {
                    selQry = "SELECT DISTINCT StudentPersonalId as Id, (FirstName + ' ' + LastName) as Name, ImageUrl FROM StudentPersonal where StudentPersonalId ='" + sess.StudentId + "'";
                }
                else
                {
                    selQry = "SELECT distinct StudentId as Id,(StudentFname+' '+StudentLname) as Name,ImageURL FROM StdtClass stdtcls left Join Student stdt ON stdt.StudentId=stdtcls.StdtId left join Placement plc on stdt.StudentId=plc.StudentPersonalId WHERE stdtcls.ClassId='" + sess.Classid + "' AND stdt.ActiveInd='A' And  stdt.SchoolId='" + sess.SchoolId + "' And stdtcls.ActiveInd='A' AND plc.Location='" + sess.Classid + "' AND plc.status='1' AND (plc.EndDate is null or convert(DATE,plc.EndDate) >= convert(DATE,getdate()))";
                }

            }
            
            Pagepath = "Home:StudFillqury";
            SesList.WriteToLog(DateTime.Now.ToString() + ',' + sess.LoginTime.ToString() + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + "Sel qry:" + selQry);
              

            DataTable dt = new DataTable();
            dt = oData.ReturnDataTable(selQry, false);
            string stdentchk = " " ;
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    stdentchk = stdentchk +','+ row["Name"].ToString();
                }
            }
            Pagepath = "Home:studFillqury";
            SesList.WriteToLog(DateTime.Now.ToString() + ',' + sess.LoginTime.ToString() + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + "dt:" + stdentchk);
              

            var behName = "";
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Name"].ToString().Length > 11)
                    {
                        name = row["Name"].ToString().Substring(0, 11) + "..";
                        if (row["Name"].ToString().Length > 25)
                        {
                            behName = row["Name"].ToString().Substring(0, 25) + "..";
                        }
                        else
                        {
                            behName = row["Name"].ToString();
                        }
                        // name = row["Name"].ToString();
                    }
                    else
                    {
                        name = row["Name"].ToString();
                        behName = row["Name"].ToString();
                    }

                    if (row["ImageUrl"].ToString() == "defaultStudent.png")
                    {
                        studentContainer.InnerHtml += "<div id=stud-" + row["Id"].ToString() + " class='students' onclick='selStudent(this,0);' ><img class='imgStudPhoto' src='../Administration/StudentsPhoto/" + row["ImageURL"].ToString() + "' class='png' width='16' height='45'/><a  href='#' onmouseover='StudentMouseOver(this.parentNode.id);' onmouseout='StudentMouseOut(this.parentNode.id);' >" + name + "</a> <div id='HoverDiv" + row["Id"].ToString() + "' style='visibility:hidden'>" + row["Name"].ToString() + "</div> </div>";
                        newBehStdList.InnerHtml += "<div class='students2' id=" + row["Id"].ToString() + " onclick='selStudent2(this,1);' ><img style='height:25px; width:25px;' class='imgStudPhoto' src='../Administration/StudentsPhoto/" + row["ImageURL"].ToString() + "'/><a style='color: white;' href='#'  >" + behName + "</a></div>";
                    }
                    else
                    {
                        if (buildName == "Local")
                        {
                            studentContainer.InnerHtml += "<div id=stud-" + row["Id"].ToString() + " class='students' onclick='selStudent(this,0);' ><img class='imgStudPhoto' src='../Administration/StudentsPhoto/" + row["ImageURL"].ToString() + "' class='png' width='16' height='45'/><a  href='#' onmouseover='StudentMouseOver(this.parentNode.id);' onmouseout='StudentMouseOut(this.parentNode.id);' >" + name + "</a> <div id='HoverDiv" + row["Id"].ToString() + "' style='visibility:hidden'>" + row["Name"].ToString() + "</div> </div>";
                            newBehStdList.InnerHtml += "<div class='students2' id=" + row["Id"].ToString() + " onclick='selStudent2(this,1);' ><img style='height:25px; width:25px;' class='imgStudPhoto' src='../Administration/StudentsPhoto/" + row["ImageURL"].ToString() + "'/><a style='color: white;' href='#'  >" + behName + "</a></div>";
                        }
                        else
                        {
                            studentContainer.InnerHtml += "<div id=stud-" + row["Id"].ToString() + " class='students' onclick='selStudent(this,0);' ><img class='imgStudPhoto' height='45px' width='16px' src=data:image/gif;base64," + row["ImageUrl"] + "><a  href='#' onmouseover='StudentMouseOver(this.parentNode.id);' onmouseout='StudentMouseOut(this.parentNode.id);' >" + name + "</a> <div id='HoverDiv" + row["Id"].ToString() + "' style='visibility:hidden'>" + row["Name"].ToString() + "</div> </div>";
                            newBehStdList.InnerHtml += "<div class='students2' id=" + row["Id"].ToString() + " onclick='selStudent2(this,1);' ><img style='height:25px; width:25px;' class='imgStudPhoto' src=data:image/gif;base64," + row["ImageUrl"] + "><a style='color: white;' href='#'  >" + behName + "</a></div>";
                        }
                    }

                }
            }
            else
            {
                Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
            }
        }




    }


    ///-------function FOR ENTERING 0 INTO THE BEHAVIOR------
    ///
    [WebMethod]
    public static string Setllabelname(string lblLoginName)
    {
        string preuser = HttpContext.Current.Session["Spreuser"].ToString();
        return preuser;
    }
    [WebMethod]
    public static void SaveBehaviorForStudent(string StudentId)
    {
        int studentId = Convert.ToInt32(StudentId);
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];

        HttpContext.Current.Session["Sprestid"] = Convert.ToInt32(_studID);
        string HostName = Dns.GetHostName();
        string userAgent = HttpContext.Current.Request.UserAgent.ToString();
        string curesesid = HttpContext.Current.Session.SessionID.ToString();
        int preid = Convert.ToInt16(HttpContext.Current.Session["Spreid"]);
        string preuser = HttpContext.Current.Session["Spreuser"].ToString();
        int SessStudentid = Convert.ToInt16(HttpContext.Current.Session["Sprestid"]);
        int PreClassid = Convert.ToInt16(HttpContext.Current.Session["SpreClsid"]);
        string Sessstname = HttpContext.Current.Session["Sprestname"].ToString();
        string ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        string Pagepath = "Home:SaveBehaviorForStudent";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        oSession = clsGeneral.sessionchecks(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);

        /// ZERO OUT FOR ALL BEHAVIOR EXCEPT INTERVAL BEHAVIOR
        /// 
        /// FOR FREQUENCY
        string InsertBehaviorFrequency = "SELECT MeasurementId FROM BehaviourDetails WHERE StudentId=" + studentId + " AND ActiveInd='A' and Frequency = 1 and PartialInterval = 0 and MeasurementId NOT IN (SELECT DISTINCT MeasurementId FROM Behaviour WHERE StudentId=" + studentId + "  AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()) and FrequencyCount is not null AND YesOrNo is null)";
        DataTable DTBehaviorFrequency = objData.ReturnDataTable(InsertBehaviorFrequency, false);

        /// FOR DURATION
        string InsertBehaviorDuration = "SELECT MeasurementId FROM BehaviourDetails WHERE StudentId=" + studentId + " AND ActiveInd='A' and Duration = 1 and PartialInterval = 0 and MeasurementId NOT IN (SELECT DISTINCT MeasurementId FROM Behaviour WHERE StudentId=" + studentId + "  AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()) and Duration is not null)";
        DataTable DTBehaviorDuration = objData.ReturnDataTable(InsertBehaviorDuration, false);

        /// FOR YES NO
        string InsertBehaviorYesNo = "SELECT MeasurementId FROM BehaviourDetails WHERE StudentId=" + studentId + " AND ActiveInd='A' and YesOrNo = 1 and PartialInterval = 0 and MeasurementId NOT IN (SELECT DISTINCT MeasurementId FROM Behaviour WHERE StudentId=" + studentId + "  AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()) and FrequencyCount is not null  AND YesOrNo is not null)";
        DataTable DTBehaviorYesNo = objData.ReturnDataTable(InsertBehaviorYesNo, false);

        string InsertQuery = "";
        int insertresult = 0;
        foreach (DataRow Behaviour in DTBehaviorFrequency.Rows)
        {
            InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,TimeOfEvent,ObserverId) Values('" + Behaviour["MeasurementId"] + "','" + studentId + "','" + 0 + "','A','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),getdate(),'"+ sess.LoginId + "')";
            insertresult = objData.Execute(InsertQuery);
        }
        foreach (DataRow Behaviour in DTBehaviorDuration.Rows)
        {
            InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,ActiveInd,Duration,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,TimeOfEvent,ObserverId) Values('" + Behaviour["MeasurementId"] + "','" + studentId + "','A','" + 0 + "','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),getdate(),'" + sess.LoginId + "')";
            insertresult = objData.Execute(InsertQuery);
        }

        foreach (DataRow Behaviour in DTBehaviorYesNo.Rows)
        {
            InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,TimeOfEvent,ObserverId,YesOrNo) Values('" + Behaviour["MeasurementId"] + "','" + studentId + "','" + 0 + "','A','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),getdate(),'" + sess.LoginId + "',0)";
            insertresult = objData.Execute(InsertQuery);
        }
    }



    protected void DlClass_ItemCommand(object source, DataListCommandEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        string ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "Home:DlClass_ItemCommand";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        sess.Classid = Convert.ToInt32(e.CommandArgument);
        Response.Redirect("~/StudentBinder/Home.aspx");
    }
    protected void lbname_Click(object sender, EventArgs e)
    {
        LinkButton lbStud = (LinkButton)sender;
        _studID = lbStud.CommandArgument;
        DataListItem dli = (DataListItem)lbStud.NamingContainer;
    }



    [WebMethod]
    public static string SetStudentID(string stdID)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession == null)
        {
            HttpContext.Current.Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (stdID.Contains("-"))
        {
            string[] ar = stdID.Split('-');
            stdID = ar[1];
        }
        _studID = stdID;

        if (_studID != "")
        {
            oSession.StudentId = Convert.ToInt32(_studID);
            Prevsess.StudentId = Convert.ToInt32(_studID);
        }
        else
        {
            oSession.StudentId = 0;
        }
        try
        {
            if (_studID != "")
            {
                HttpContext.Current.Session["Sprestid"] = Convert.ToInt32(_studID);
                string HostName = Dns.GetHostName();
                string userAgent = HttpContext.Current.Request.UserAgent.ToString();
                string curesesid = HttpContext.Current.Session.SessionID.ToString();
                int preid = Convert.ToInt16(HttpContext.Current.Session["Spreid"]);
                string preuser = HttpContext.Current.Session["Spreuser"].ToString();
                int SessStudentid = Convert.ToInt16(HttpContext.Current.Session["Sprestid"]);
                int PreClassid = Convert.ToInt16(HttpContext.Current.Session["SpreClsid"]);
                string Sessstname = HttpContext.Current.Session["Sprestname"].ToString();
                string ip = clsGeneral.GetIPAddress();
                ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
                string Pagepath = "Home:SetStudentID";
                if (Prevsess != null)
                {
                    sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
                }
                else
                {
                    sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
                }
                
                preid = Convert.ToInt16(HttpContext.Current.Session["Spreid"]);
                preuser = HttpContext.Current.Session["Spreuser"].ToString();
                SessStudentid = Convert.ToInt16(HttpContext.Current.Session["Sprestid"]);
                Sessstname = HttpContext.Current.Session["Sprestname"].ToString();
                curesesid = HttpContext.Current.Session.SessionID.ToString();
                PreClassid = Convert.ToInt16(HttpContext.Current.Session["SpreClsid"]);
                oSession = clsGeneral.sessionchecks(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);

            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return oSession.StudentId.ToString();
    }

    [WebMethod]
    public static void setCheckInn(string studId)
    {
        try
        {
            clsSession objSess = (clsSession)HttpContext.Current.Session["UserSession"];
            if (objSess == null)
            {
                HttpContext.Current.Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
            }
            int studentId = Convert.ToInt32(studId.Split('-')[1]);
            bool blExist = objData.IFExists("SELECT StudentId FROM StdtSessEvent WHERE StudentId=" + studentId + " and ClassId=" + objSess.Classid + "and EventType='CH' AND SchoolId=" + objSess.SchoolId + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
            if (blExist == true)
            {
                objData.Execute("Update [StdtSessEvent] SET CheckStatus='True' Where StudentId=" + studentId + "  And ClassId=" + objSess.Classid + "and EventType='CH' AND SchoolId=" + objSess.SchoolId + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");

            }
            else
            {
                string strQuery = "insert into StdtSessEvent (SchoolId,StudentId,EvntTs,CheckStatus,ClassId,CreatedBy,CreatedOn,ModifiedOn,EventType,CheckinTime)values(" + objSess.SchoolId + "," + studentId + ",getdate(),'True'," + objSess.Classid + "," + objSess.LoginId + ",getdate(),getdate(),'CH',GETDATE()) ";
                objData.Execute(strQuery);
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    [WebMethod]
    public static clsSubmenu[] FillSubMenu(string Menu)
    {
        clsData oData = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        clsSubmenu oSubmenu = new clsSubmenu();
        List<clsSubmenu> list = new List<clsSubmenu>();
        bool IsChecked = false;
        DataTable dt = new DataTable();

        int preid = Convert.ToInt16(HttpContext.Current.Session["Spreid"]);
        string preuser = HttpContext.Current.Session["Spreuser"].ToString();
        int SessStudentid = Convert.ToInt16(HttpContext.Current.Session["Sprestid"]);
        int PreClassid = Convert.ToInt16(HttpContext.Current.Session["SpreClsid"]);
        string Sessstname = HttpContext.Current.Session["Sprestname"].ToString();
        string curesesid = HttpContext.Current.Session.SessionID.ToString();
        string HostName = Dns.GetHostName();
        string userAgent = HttpContext.Current.Request.UserAgent.ToString();
        string ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        string Pagepath = "Home:FillSubMenu";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        oSession = clsGeneral.sessionchecks(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);

         if (_studID == "")
        {           
            _studID = oSession.StudentId.ToString();
        }

        if (_studID != "")
        {

            bool blExist = oData.IFExists("SELECT StudentId FROM StdtSessEvent WHERE StudentId=" + oSession.StudentId + " and ClassId=" + oSession.Classid + "and EventType='CH' AND SchoolId=" + oSession.SchoolId + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
            if (blExist == true)
            {
                bool objCheckIn = Convert.ToBoolean(objData.FetchValue("Select CheckStatus from  [StdtSessEvent] Where StudentId=" + oSession.StudentId + "  And ClassId=" + oSession.Classid + "and EventType='CH' AND SchoolId=" + oSession.SchoolId + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())"));
                if (objCheckIn)
                {
                    IsChecked = true;
                }
            }
            if (Menu.Trim() == "DATASHEETS")
            {
                if (IsChecked == true)
                {
                    dt = oSubmenu.getSubmenuList(Menu.Replace("\n", "").Trim(), oSession.StudentId.ToString(), oSession.SchoolId);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                dt = oSubmenu.getSubmenuList(Menu.Replace("\n", "").Trim(), oSession.StudentId.ToString(), oSession.SchoolId);
            }
        }
        if (dt != null)
            foreach (DataRow row in dt.Rows)
            {
                clsSubmenu _names = new clsSubmenu();
                _names.Submenu = row["Name"].ToString();
                _names.Url = row["Url"].ToString();
                _names.ID = row["ID"].ToString();
                list.Add(_names);
            }
        return list.ToArray();
    }
    [WebMethod]
    public static clsSubmenu[] FillStudnt()
    {
        clsData oData = new clsData();
        List<clsSubmenu> list = new List<clsSubmenu>();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        if (sess != null)
        {
            string selQry = "SELECT distinct StudentId as Id,(StudentLname+' - '+StudentNbr) as Name,ImageURL FROM StdtClass stdtcls left Join Student stdt ON stdt.StudentId=stdtcls.StdtId WHERE stdtcls.ClassId='" + sess.Classid + "' AND stdt.ActiveInd='A' And stdtcls.ActiveInd='A'";

            DataTable dt = new DataTable();
            dt = oData.ReturnDataTable(selQry, false);


            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    clsSubmenu _names = new clsSubmenu();
                    _names.StudName = row["Name"].ToString();
                    _names.Img = row["ImageURL"].ToString();
                    _names.StudID = Convert.ToInt32(row["Id"].ToString());
                    list.Add(_names);
                }
            }
            else
            {

            }

        }
        return list.ToArray();
    }




    /// <summary>
    /// /////////for fill side slide////////////////////////
    /// </summary>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getbehaviourdata(string studentid)
    {
        if (studentid.Contains("-"))
        {
            string[] ar = studentid.Split('-');
            studentid = ar[1];
        }
        objData = new clsData();
        List<BehaviourInfo> columns = new List<BehaviourInfo>();
        string strQuery = "SELECT Mt.Behaviour,Mt.MeasurementId,Mt.Frequency from [BehaviourDetails] Mt where Mt.StudentId='" + studentid + "' and Mt.ActiveInd='A'";

        SqlDataReader reader = objData.ReturnDataReader(strQuery, false);

        try
        {
            if (reader != null)
            {
                while (reader.Read())
                {
                    BehaviourInfo behaviour = new BehaviourInfo();
                    behaviour.MeasurementId = Convert.ToInt32(reader["MeasurementId"].ToString());
                    behaviour.Behaviour = reader["Behaviour"].ToString();
                    behaviour.Frequency = reader["Frequency"].ToString();
                    columns.Add(behaviour);
                }
            }
            reader.Close();
        }
        catch (Exception exp)
        {
            reader.Close();
        }
        return new JavaScriptSerializer().Serialize(columns);
    }

    [Serializable()]
    public class BehaviourInfo
    {
        public string Behaviour;
        public int MeasurementId;
        public string Frequency;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static void getResultFrequency(string MeasurementId, string StudentId)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];

        //string a = "Select FrequencyCount from Behaviour where MeasurementId='" + Convert.ToInt32(MeasurementId) + "' and StudentId='" + sess.StudentId + "' and Convert(date,CreatedOn)=convert(date,GETDATE())";
        //string b = "Select FrequencyCount from Behaviour where MeasurementId='" + Convert.ToInt32(MeasurementId) + "' and StudentId='" + sess.StudentId + "' and Convert(date,CreatedOn)=convert(date,GETDATE())";
        //if (objData.IFExists(a))
        //{
        //    string FrequencyString = objData.FetchValue(b).ToString();

        //    frequencycount = Convert.ToInt32(FrequencyString);
        //    string updatequery = "Update Behaviour SET FrequencyCount='" + (frequencycount + 1) + "',ActiveInd='A',ModifiedBy='" + sess.LoginId + "',ModifiedOn=getdate() where MeasurementId='" + Convert.ToInt32(MeasurementId) + "' and StudentId='" + sess.StudentId + "' and convert(date,CreatedOn)=convert(date,GETDATE())";
        //    int updateresult = objData.Execute(updatequery);
        //}
        //else
        //{
        string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId) Values('" + MeasurementId + "','" + StudentId + "','" + 1 + "','A','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),'" + sess.LoginId + "')";
        int insertresult = objData.Execute(InsertQuery);
        //}
        //}


    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static void selectDischargedStudent(int stdId)
    {
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        sess.StudentId = stdId;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static void ChangeClassId(int ClassId)
    {
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        sess.Classid = ClassId;
        sess.StudentId = -9;
        int preid = Convert.ToInt16(HttpContext.Current.Session["Spreid"]);
        string preuser = HttpContext.Current.Session["Spreuser"].ToString();
        int SessStudentid = Convert.ToInt16(HttpContext.Current.Session["Sprestid"]);
        int PreClassid = Convert.ToInt16(HttpContext.Current.Session["SpreClsid"]);
        string Sessstname = HttpContext.Current.Session["Sprestname"].ToString();
        string curesesid = HttpContext.Current.Session.SessionID.ToString();
        string HostName = Dns.GetHostName();
        string userAgent = HttpContext.Current.Request.UserAgent.ToString();
        string ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        string Pagepath = "Home:ChangeClassId";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneral.sessionchecks(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string fillclass()
    {
        string DlClass = "";
        clsData objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["uSEERsESSION"];
        if (sess == null) return "Failed";
        if (objData.IFExists("SELECT ClassId from UserClass where UserId='" + sess.LoginId + "'") == true)
        {
            //string classdetail = "SELECT Cls.ClassId AS Id,Cls.ClassName AS Name FROM Class Cls WHERE Cls.ActiveInd='A'";
            string classdetail = "SELECT UsrCls.ClassId AS Id,Cls.ClassName AS Name FROM UserClass UsrCls INNER JOIN Class Cls ON Cls.ClassId=UsrCls.ClassId WHERE UsrCls.UserId=" + sess.LoginId + " AND Cls.ActiveInd='A' and UsrCls.ClassId<>'" + sess.Classid + "' AND UsrCls.ActiveInd='A' ORDER BY Name ASC";

            DataTable dt = objData.ReturnDataTable(classdetail, false);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string functn = "ChangeClassId(" + row["Id"] + ");";
                    DlClass += "<div class='grmb' id=" + row["Id"] + " onclick=" + functn + ">" + row["Name"] + "</div>";
                }
            }

        }
        return DlClass;
    }
    protected void rbtnClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        //sess = (clsSession)Session["UserSession"];
        //if (rbtnClass.SelectedItem.Text != "")
        //{

        //    if (sess.Classid != Convert.ToInt32(rbtnClass.SelectedValue))
        //    {
        //        sess.Classid = Convert.ToInt32(rbtnClass.SelectedValue);
        //        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "open_window", "window.document.forms[0].target = '_blank'; setTimeout(function () {window.document.forms[0].target = '';}, 500);$('.classDrop').fadeOut('slow');", true);
        //        Response.Redirect("~/StudentBinder/Home.aspx");
        //    }
        //}
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getResultDuration(string BehaviourId, string Durationid, string DurationTime)
    {
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int DurationId = Convert.ToInt32(Durationid);
        int durTime = Convert.ToInt32(DurationTime);
        //THIS CODE IS TO CONVERT SECONDS TO HH:MM:SS FORMAT //UNCOMMENT IF NEEDED.

        //string mt = "00";
        //string hr = "00";

        //mt = ((durTime / 60) < 10) ? "0" + (durTime / 60).ToString() : (durTime / 60).ToString();
        //hr = ((Convert.ToInt32(mt) / 60) < 10) ? "0" + (Convert.ToInt32(mt) / 60).ToString() : (Convert.ToInt32(mt) / 60).ToString();

        //DurationTime = hr + ":" + mt;


        string updatequery = "Update Behaviour SET EndTime=CONVERT (time, GETDATE()),ModifiedOn=getdate(), Duration='" + DurationTime + "' where BehaviourId='" + BehaviourId + "'";
        int updateresult = objData.Execute(updatequery);
        DurationId = 0;

        return DurationId.ToString();

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string saveDurationStartTime(string MeasurementId, string StudentId)
    {
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,StartTime,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId) Values('" + MeasurementId + "','" + StudentId + "',CONVERT (time, GETDATE()),'A','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),'" + sess.LoginId + "')";
        int insertresult = objData.ExecuteWithScope(InsertQuery);

        return insertresult.ToString();

    }


    [WebMethod]
    public static void setTemplateSess(string StudIDs)
    {
        int StudID = 0;
        if (StudIDs.Contains("-"))
        {
            string[] ar = StudIDs.Split('-');
            StudID = Convert.ToInt32(ar[1]);
        }

        if (StudID != 0)
        {

            objData = new clsData();
            ObjTempSess = new ClsTemplateSession();

            try
            {
                sess = (clsSession)HttpContext.Current.Session["UserSession"];
                if (sess == null) return;
                sess.StudentId = StudID;
                StudID = 0;
                string strQuery = "SELECT StudentLname+','+StudentFname AS StudentName FROM Student WHERE StudentId=" + sess.StudentId;
                DataTable dt = objData.ReturnDataTable(strQuery, false);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        sess.StudentName = dt.Rows[0]["StudentName"].ToString();
                        HttpContext.Current.Session["Sprestname"] = dt.Rows[0]["StudentName"].ToString();
                        Prevsess.StudentName = dt.Rows[0]["StudentName"].ToString();
                    }
                }
                HttpContext.Current.Session["UserSession"] = sess;
                HttpContext.Current.Session["BiweeklySession"] = ObjTempSess;

                int preid = Convert.ToInt16(HttpContext.Current.Session["Spreid"]);
                string preuser = HttpContext.Current.Session["Spreuser"].ToString();
                int SessStudentid = Convert.ToInt16(HttpContext.Current.Session["Sprestid"]);
                int PreClassid = Convert.ToInt16(HttpContext.Current.Session["SpreClsid"]);
                string Sessstname = HttpContext.Current.Session["Sprestname"].ToString();
                string curesesid = HttpContext.Current.Session.SessionID.ToString();
                string HostName = Dns.GetHostName();
                string userAgent = HttpContext.Current.Request.UserAgent.ToString();
                string ip = clsGeneral.GetIPAddress();
                ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
                string Pagepath = "Home:setTemplateSess";
                if (Prevsess != null)
                {
                    sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
                }
                else
                {
                    sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
                }
                sess = clsGeneral.sessionchecks(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
                    
            }
            catch (Exception Ex)
            {
                HttpContext.Current.Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
            }
        }
    }


    [WebMethod]
    public static string setOverAllStatus(string StudIDs)
    {
        int StudID = 0;
        string status = "";
        if (StudIDs.Contains("-"))
        {
            string[] ar = StudIDs.Split('-');
            StudID = Convert.ToInt32(ar[1]);
        }

        if (StudID != 0)
        {
            objStatus = new clsStatus();
            status = objStatus.biWeeklyStatus(StudID);


        }

        return status;

    }


    [WebMethod]
    public static string SearchLessonPlanList(string Tab, string SearchCondition, int option)
    {
        HttpContext.Current.Session["type"] = "next";
        HttpContext.Current.Session["page"] = Tab.ToString();
        HttpContext.Current.Session["SearchCondition"] = SearchCondition.ToString();
        HttpContext.Current.Session["option"] = option.ToString();
        string LessonList = "";
        string opt = "";
        objData = new clsData();
        clsLessons oLessons = new clsLessons();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        DataTable dtSubmenu = new DataTable();
        dtSubmenu.Columns.Add("Name", typeof(string));
        dtSubmenu.Columns.Add("ID", typeof(string));
        dtSubmenu.Columns.Add("StatusID", typeof(string));
        string Url = "";
        string optString = "";

        if (option == 0)
        {
            optString = " AND (StdtLP.LessonPlanTypeDay = 1 OR StdtLP.LessonPlanTypeResi = 1)";
        }
        if (option == 1)
        {
            optString = " AND StdtLP.LessonPlanTypeDay = 1";
        }
        if (option == 2)
        {
            optString = " AND StdtLP.LessonPlanTypeResi = 1";
        }
        if (option == 3)
        {
            optString = " AND ((StdtLP.LessonPlanTypeDay IS NULL AND StdtLP.LessonPlanTypeResi IS NULL) OR (StdtLP.LessonPlanTypeDay = 0 OR StdtLP.LessonPlanTypeResi = 0))";
        }

        string dayOrResi = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) as DayFlag," +
                "(SELECT TOP 1 lessonplantyperesi FROM stdtlessonplan STLP WHERE STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag";

        HttpContext.Current.Session["optString"] = optString.ToString();
        if (Tab == "LessonPlanTab_approve")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "LessonPlanAttributes.aspx";
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.DSTemplateName as Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved') " + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "LessonPlanTab_inactive")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "LessonPlanAttributes.aspx";
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Inactive' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.DSTemplateName as Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Inactive')" + optString + "  AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='' OR DTmp.DSMode='Inactive') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "LessonPlanTab_maintenance")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "LessonPlanAttributes.aspx";
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Maintenance' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.DSTemplateName as Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "( LU.LookupName='Maintenance')" + optString + "  AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "LessonPlanTab_rd")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "LessonPlanAttributes.aspx";
            string Result = " SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder FROM (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR LookupName='Maintenance' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%')LP) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.DSTemplateName as Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='Inactive') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "LessonPlanTab")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "LessonPlanAttributes.aspx";
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Maintenance' " +
" )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.DSTemplateName as Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance' OR DTmp.DSMode='') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='Inactive') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "DatasheetTab")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "Datasheet.aspx";
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "DatasheetTab_rd")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "Datasheet.aspx";
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            // dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "DatasheetTab_approved")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "Datasheet.aspx";
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "DatasheetTab_maintenance")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "Datasheet.aspx";
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Maintenance' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Maintenance' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "Schedule_View")
        {
            opt = "scheduleview";
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "Datasheet.aspx";
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId.ToString() + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId.ToString() + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'Revised ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),003)) ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "GraphTab")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "AcademicSessionReport.aspx";
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN 
            //StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in 
            //(" + StatusIdAppr + "," + StatusIdIna + "," + StatusIdMai + ") AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance'))) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "GraphTab_rd")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "AcademicSessionReport.aspx";
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.StatusId as StatusID,
            //hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND 
            //hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId 
            //in (" + StatusIdAppr + "," + StatusIdIna + "," + StatusIdMai + ")" + optString + "  AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR " +
 " LookupName='Maintenance')) " + optString + ")LP) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "GraphTab_inactive")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "AcademicSessionReport.aspx";
            int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.StatusId as StatusID,
            //hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=
            //StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdIna + ")"
            //+ optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Inactive' " +
" )) " + optString + ") DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "GraphTab_approved")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "AcademicSessionReport.aspx";
            int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
 " )) " + optString + ") DATA ORDER BY  DATA.LessonOrder";
            //string strQuery = "SELECT DISTINCT top(50)  StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdAppr + ")" + optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ORDER BY hdr.LessonOrder ";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "GraphTab_maintenance")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "AcademicSessionReport.aspx";
            int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT  top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdMai + ")" + optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( " +
 " LookupName='Maintenance')) " + optString + ") DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "ChainGraphTab")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "ChainedBarGraphReport.aspx";
            //int StatusId = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag, hdr.StatusId as StatusID,
            //hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=
            //StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdAppr + ",
            //" + StatusIdIna + "," + StatusIdMai + ") AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND hdr.SkillType='Chained' AND hdr.ChainType<>'Total Task'
            //ORDER BY hdr.LessonOrder ";

            ///select the chain graph with DSTempHdr.ChainType='Total Task' or 'Forword chain' or 'Backword chain'
            ///
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,LessonOrder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained'" +
 //AND DSTempHdr.ChainType<>'Total Task' 
 "AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance'))) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "ChainGraphTab_rd")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "ChainedBarGraphReport.aspx";
            //int StatusId = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag, hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdAppr + "," + StatusIdIna + "," + StatusIdMai + ")" + optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND hdr.SkillType='Chained' AND hdr.ChainType<>'Total Task' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 //AND DSTempHdr.ChainType<>'Total Task' 
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR " +
 " LookupName='Maintenance')) " + optString + ")LP) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "ChainGraphTab_inactive")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "ChainedBarGraphReport.aspx";
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' "+
 //AND DSTempHdr.ChainType<>'Total Task' 
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( LookupName='Inactive' " +
 " )) " + optString + ") DATA ORDER BY  DATA.LessonOrder";
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag, hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and (hdr.StatusId =" + StatusIdIna + ")" + optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND hdr.SkillType='Chained' AND hdr.ChainType<>'Total Task' ORDER BY hdr.LessonOrder ";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "ChainGraphTab_approved")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "ChainedBarGraphReport.aspx";
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag, hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and (hdr.StatusId =" + StatusIdAppr + ")" + optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND hdr.SkillType='Chained' AND hdr.ChainType<>'Total Task' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' "+
//AND DSTempHdr.ChainType<>'Total Task' 
" AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
" )) " + optString + ") DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "ChainGraphTab_maintenance")
        {
            HttpContext.Current.Session["current"] = 1;
            HttpContext.Current.Session["Tab1"] = Tab.ToString();
            Url = "ChainedBarGraphReport.aspx";
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50)  StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.StatusId as StatusID,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and (hdr.StatusId =" + StatusIdMai + ")" + optString + " AND hdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND hdr.SkillType='Chained' AND hdr.ChainType<>'Total Task' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' "+
 //AND DSTempHdr.ChainType<>'Total Task'
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( " +
 " LookupName='Maintenance')) " + optString + ") DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }

        // new
        else if (Tab == "Previous")
        {
            Home h1 = new Home(); 
            dtSubmenu = h1.LoadPreviousPage(HttpContext.Current.Session["Tab1"].ToString(), HttpContext.Current.Session["SearchCondition"].ToString(), Convert.ToInt32(HttpContext.Current.Session["option"]));
        }
        else if (Tab == "Next")
        {
            Home h1 = new Home();
            dtSubmenu = h1.LoadNextPage(HttpContext.Current.Session["Tab1"].ToString(), HttpContext.Current.Session["SearchCondition"].ToString(),Convert.ToInt32(HttpContext.Current.Session["option"]));
        }
        int count = dtSubmenu.Rows.Count;
        if (Url == "")
        {
            Url = HttpContext.Current.Session["Url"].ToString();
        }
        LessonList = FillLessonPlanList(LessonList, dtSubmenu, Url,opt);
        if (dtSubmenu != null)
        {
            if (dtSubmenu.Rows.Count == 0)
            {
                LessonList = "<div>No Data Available</div>";
            }


        }
        else
            LessonList = "<div>No Data Available</div>";
        return LessonList;
    }

    [WebMethod]
    public static string SelectLessonPlanList(string Tab)
    {
        HttpContext.Current.Session["type"] = "next";
        HttpContext.Current.Session["Tab1"] = Tab.ToString();
        HttpContext.Current.Session["lastitem"] = 0;
        HttpContext.Current.Session["current"] = 1;
        string LessonList = "";
        string opt = "";
        objData = new clsData();
        clsLessons oLessons = new clsLessons();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        DataTable dtSubmenu = new DataTable();
        dtSubmenu.Columns.Add("Name", typeof(string));
        dtSubmenu.Columns.Add("ID", typeof(string));
        dtSubmenu.Columns.Add("StatusID", typeof(string));
        string Url = "";
        string dayOrResi = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) as DayFlag, " +
                "(SELECT TOP 1 lessonplantyperesi FROM stdtlessonplan STLP WHERE STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag";

        if (Tab == "LessonPlanTab")
        {
            Url = "LessonPlanAttributes.aspx";

            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +  //OR LookupName='Inactive'
 " LookupName='Maintenance'))) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
            int count = dtSubmenu.Rows.Count;
            if (count == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 1]["lessonorder"].ToString();
            }
            else if (count > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 2]["lessonorder"].ToString();
            }
            if (count != 0)
            {
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "LP.LessonPlanName as Name, DTmp.DSTempHdrId As ID", "LU.LookupName='Approved'");StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,StdtLP.LessonPlanTypeResi as ResFlag,DTmp.DSTemplateName as Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID", "(LU.LookupName='Approved' OR LU.LookupName='Inactive' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='' OR DTmp.DSMode='Inactive')");

        }
        else if (Tab == "DatasheetTab")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["datasheet"] = Tab.ToString();
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            if (count1 != 0)
            {
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "Schedule_View")
        {
            Url = "Datasheet.aspx";
            opt = "Scheduleview";
            HttpContext.Current.Session["datasheet"] = Tab.ToString();
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "StdtLP.LessonPlanTypeDay as DayFlag,CASE WHEN StdtLP.LessonPlanTypeResi IS NULL THEN 0 ELSE StdtLP.LessonPlanTypeResi END as ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId.ToString() + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId.ToString() + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,' Revised ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),003)) ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
            //dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP inner join StdtLPSched stst on LessonPlanId=stst.LPId AND StudentId=stst.StdtId where stst.Day=convert(date,GETDATE()) and  STLP.lessonplanid = stst.LPId AND STLP.studentid = " + sess.StudentId.ToString() + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP inner join StdtLPSched stst on LessonPlanId=stst.LPId AND StudentId=stst.StdtId where stst.Day=convert(date,GETDATE()) and  STLP.lessonplanid = stst.LPId AND STLP.studentid = " + sess.StudentId.ToString() + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*)  FROM DSTempHdr inner join StdtLPSched stst on LessonPlanId=stst.LPId AND StudentId=stst.StdtId where stst.Day=convert(date,GETDATE()) AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
            string getothSch = "SELECT distinct top(51) NULL AS DayFlag,NULL AS ResFlag,NULL AS IsMT_IOA,SLP.SchOtherDesc AS Name, LPId AS ID,NULL AS StatusID,NULL AS ModifiedDate,NULL AS ModifiedCnt,NULL AS lessonorder,FORMAT(CAST(SLP.starttime AS DATETIME),'hh:mmtt') starttime, CONVERT(VARCHAR(4),SLP.starttime,108) as starttime1, CONVERT(VARCHAR(15),CAST(SLP.EndTime AS TIME), 100)  as EndTime,NULL AS Draft,NULL AS Counter,NULL AS modi,NULL AS ioa FROM StdtLPSched SLP WHERE SLP.StdtId = " + sess.StudentId.ToString() + " AND SLP.LPId = 0 AND SLP.SchOtherDesc IS NOT NULL AND CONVERT(DATE,GETDATE()) = SLP.Day ORDER BY starttime1 ASC";
            DataTable test = objData.ReturnDataTable(getothSch, true);
            dtSubmenu.Merge(test, true, MissingSchemaAction.Ignore);
            dtSubmenu.DefaultView.Sort = "starttime1 asc";
            dtSubmenu = dtSubmenu.DefaultView.ToTable();

            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            if (count1 != 0)
            {
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "GraphTab")
        {
            Url = "AcademicSessionReport.aspx";
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID,hdr.StatusId FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdAppr + "," + StatusIdIna + "," + StatusIdMai + ") ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance'))) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 == 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            else if (count1 > 1)
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
            }
            if (count1 != 0)
            {
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["Tab"] = Tab.ToString();
            HttpContext.Current.Session["count"] = 50;
        }
        else if (Tab == "ChainGraphTab")
        {
            Url = "ChainedBarGraphReport.aspx";
            //int StatusId = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            //int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            //int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            //string strQuery = "SELECT DISTINCT LP.LessonPlanName as Name,hdr.DSTempHdrId As ID FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId in (" + StatusIdAppr + "," + StatusIdIna + "," + StatusIdMai + ")  AND hdr.SkillType='Chained' ORDER BY LP.LessonPlanName ";
            //string strQuery = "SELECT DISTINCT top(50) StdtLP.LessonPlanTypeDay as DayFlag,hdr.LessonOrder,StdtLP.LessonPlanTypeResi as ResFlag,hdr.DSTemplateName as Name,hdr.DSTempHdrId As ID,hdr.StatusId FROM DSTempHdr hdr INNER JOIN StdtLessonPlan StdtLP ON hdr.LessonPlanId=StdtLP.LessonPlanId AND hdr.studentid=StdtLP.studentid Inner Join LessonPlan LP On LP.LessonPlanId=hdr.LessonPlanId WHERE hdr.StudentId=" + sess.StudentId + " and hdr.StatusId in (" + StatusIdAppr + "," + StatusIdIna + "," + StatusIdMai + ")  AND hdr.SkillType='Chained' AND hdr.ChainType<>'Total Task' ORDER BY hdr.LessonOrder ";
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.SkillType='Chained'"+
 //AND DSTempHdr.ChainType<>'Total Task' 
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance'))) DATA ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
            int count1 = dtSubmenu.Rows.Count;
            if (count1 != 0)
            {
                if (count1 == 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
                }
                else if (count1 > 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
                }
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
                HttpContext.Current.Session["Tab"] = Tab.ToString();
                HttpContext.Current.Session["count"] = 50;
            }
        }
        LessonList = FillLessonPlanList(LessonList, dtSubmenu, Url,opt);
        if (dtSubmenu != null)
        {
            if (dtSubmenu.Rows.Count == 0)
            {
                LessonList = "<div>No Data Available</div>";
            }
        }
        else
            LessonList = "<div>No Data Available</div>";

        return LessonList;
    }

    private static string FillLessonPlanList(string LessonList, DataTable dtSubmenu, string Url, string opt)
    {
        string StatusIdApp = Convert.ToString(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
        string StatusIdIn = Convert.ToString(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
        string value = "";
        string ioa = "IOA:0";
        string sch = "";
        if (dtSubmenu != null)
        {
            if (dtSubmenu.Rows.Count >= 1)
            {
                int count = dtSubmenu.Rows.Count;
                LessonList += " <table id='lessonplanlist' style='width: 100%'> <tr style='height: 25px;'><td>";
                if(Convert.ToInt32(HttpContext.Current.Session["current"]) == 1 && count < 51)
                {
                    if (HttpContext.Current.Session["type"] != "previous")
                    {
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                    }
                    else
                    {
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                    }
                }
                else if (Convert.ToInt32(HttpContext.Current.Session["current"]) == 1)
                {
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                }
                else if (count < 51)
                {
                    HttpContext.Current.Session["last"] = Convert.ToInt32(HttpContext.Current.Session["current"]);
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Previous\");'>PREVIOUS</span> |";
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                }
                else if (Convert.ToInt32(HttpContext.Current.Session["current"]) < Convert.ToInt32(HttpContext.Current.Session["last"]))
                {
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Previous\");'>PREVIOUS</span> |";
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                }

                else
                {
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Previous\");'>PREVIOUS</span> |";
                    LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                }
                if (opt != "Scheduleview")
                {
                    LessonList += "</td>";
                    LessonList += "<td>";
                    LessonList += "<div style='padding-left: 480px;font-style: italic; color: blue;'> <label>Page " + Convert.ToInt32(HttpContext.Current.Session["current"]) + "</label></div>";
                    LessonList += "</table>";
                    LessonList += "</td>";
                    LessonList += "</tr>";
                }
                LessonList += " <table id='lessonplanlist' style='width: 100%'> <tr style='height: 50px;'><td>";
                if (dtSubmenu.Rows.Count > 50)
                {
                    if (HttpContext.Current.Session["type"] != "previous")
                    {
                        dtSubmenu.Rows.RemoveAt(dtSubmenu.Rows.Count - 1);
                    }
                    else
                    {
                        if (Convert.ToInt32(HttpContext.Current.Session["current"]) != 1)
                        {
                            dtSubmenu.Rows.RemoveAt(dtSubmenu.Rows.Count - 1);
                        }
                        else
                        {
                            dtSubmenu.Rows.RemoveAt(dtSubmenu.Rows.Count - count);
                        }
                    }
                }
                int LoopCnt = 1;
                string LessonPlanName = "";
                string str = "";
                foreach (DataRow row in dtSubmenu.Rows)
                {
                    if (row["Name"].ToString().Count() > 97)
                    {
                        LessonPlanName = row["Name"].ToString().Substring(0, 96) + "...";
                        //LessonPlanName = row["Name"].ToString();
                    }
                    else
                    {
                        LessonPlanName = row["Name"].ToString();
                    }

                    if (LoopCnt == 5 && opt != "Scheduleview")
                    {
                        LoopCnt = 1;
                        if (Url == "Datasheet.aspx")
                        {
                            string ioaDivs = "";
                            string Counter = "";
                            if (Convert.ToInt16(row["Counter"].ToString()) != 0)
                            {
                                Counter = "#" + Convert.ToInt16(row["Counter"].ToString());
                            }
                            if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 1)
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span><div class='ioaNeed' style='float:right' title='Require IOA to proceed.'>IOA</div></div>";
                            }
                            else if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 2)
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span><div class='mtNeed' style='float:right' title='Require Multi Teacher to proceed.'>MT</div></div>";
                            }
                            else if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 3)
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span><div class='ioaNeed' style='float:right' title='Require IOA to proceed.'>IOA</div><div class='mtNeed' style='float:right' title='Require Multi Teacher to proceed.'>MT</div></div>";
                            }
                            else
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span></div>";
                            }

                            if (Convert.ToInt32(row["ModifiedCnt"]) == 0)
                            {
                                row["ModifiedDate"] = "";
                            }
                            if (row["StatusID"].ToString() == StatusIdApp)
                                LessonList += "</td></tr><tr style='height: 50px;'><td><div></div> <div style='background-color:#6bc963; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList' onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0);CloseOverlayOnSelect();'> <div class='lpName'>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 70px;position: absolute;'>" + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Active</div>  </div>" + ioaDivs + " </div> ";
                            else
                                LessonList += "</td></tr><tr style='height: 50px;'><td><div></div> <div style='background-color:#E8E80C; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList' onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0);CloseOverlayOnSelect();'> <div class='lpName'>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 70px;position: absolute;'>" + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Maintenance</div>  </div>" + ioaDivs + " </div> ";
                        }
                        else
                        {
                            if (row["StatusID"].ToString() == StatusIdApp)
                                LessonList += "<div style='background-color:#6bc963;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '> <div class='lpName'>" + LessonPlanName + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Active</div>  </div> </div> ";
                            else if (row["StatusID"].ToString() == StatusIdIn)
                                LessonList += "<div style='background-color:#A6A6A6;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '> <div class='lpName'>" + LessonPlanName + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Inactive</div>  </div> </div> ";
                            else
                                LessonList += "<div style='background-color:#E8E80C;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '> <div class='lpName'>" + LessonPlanName + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Maintenance</div>  </div> </div> ";
                        }
                        //else
                        //{
                        //    LessonList += "</td></tr><tr style='height: 50px;'><td> <div id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList' onclick='selSubmenu(this," + row["ID"] + ",0);CloseOverlayOnSelect();'><br clear='all'> " + LessonPlanName + "   </div>";
                        //}
                    }
                    else
                    {
                        if (Url == "Datasheet.aspx" && opt == "Scheduleview")
                        {
                            string ioaDivs = "";
                            string Counter = "";
                            //if (Convert.ToInt16(row["ioastatus"].ToString()) != 0)
                            //{
                            //    ioa = "IOA:" + row["ioa"].ToString();
                            //}
                            //else
                            //{
                            //  ioa = "IOA:0";
                            //}
                            if (Convert.ToInt32(row["ID"].ToString()) > 0)
                            {
                                if (Convert.ToInt16(row["Counter"].ToString()) != 0)
                                {
                                    Counter = "Runs: " + Convert.ToInt16(row["Counter"].ToString());
                                }
                                if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 1)
                                {
                                    ioaDivs = "<div class='leftDiv'><span style='color:black;font-size:10px;margin-left:63px;'>" + Counter + "  " + ioa + "</span><div class='ioaNeed' style='float:right' title='Require IOA to proceed.'>IOA</div></div>";
                                }
                                else if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 2)
                                {
                                    ioaDivs = "<div class='leftDiv'><span style='color:black;font-size:10px;margin-left:63px;'>" + Counter + "  " + ioa + "</span><div class='mtNeed' style='float:right' title='Require Multi Teacher to proceed.'>MT</div></div>";
                                }
                                else if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 3)
                                {
                                    ioaDivs = "<div class='leftDiv'><span style='color:black;font-size:10px;margin-left:63px;'>" + Counter + "  " + ioa + "</span><div class='ioaNeed' style='float:right' title='Require IOA to proceed.'>IOA</div><div class='mtNeed' style='float:right' title='Require Multi Teacher to proceed.'>MT</div></div>";
                                }
                                else
                                {
                                    ioaDivs = "<div class='leftDiv'><span style='color:black;font-size:10px;margin-left:63px;'>" + Counter + "  " + ioa + "</span></div>";
                                }

                                if (Convert.ToInt32(row["ModifiedCnt"]) == 0)
                                {
                                    row["ModifiedDate"] = "";
                                }
                                value = "";
                                if (Convert.ToInt16(row["draft"].ToString()) != 0)
                                {
                                    value = " 🕐 ";
                                }
                                //if (row["starttime"] != 0)
                                //{
                                //row["starttime"] = row["starttime"]+"-" + row["starttime"];
                                //}
                            }
                            if (row["StatusID"].ToString() == StatusIdApp)
                            {
                                if (Convert.ToInt16(row["modi"].ToString()) != 0)
                                {
                                    LessonList += "<div style='background-color:grey; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList1' onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName1' style=height:64px;background:orange;>" + row["starttime"] + " - " + row["endtime"] + "</div> <div class='lpName2' style=margin-left: 50px;>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 135px;position: absolute;font-size:11px;'>" + "   " + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div><div class='lpStatus'>" + value + "Active</div>  </div>" + ioaDivs + " </div>";
                                }
                                else
                                {

                                    LessonList += "<div style='background-color:#6bc963; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList1' onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName1' style=height:64px;background:orange;>" + row["starttime"] + " - " + row["endtime"] + "</div> <div class='lpName2' style=margin-left: 50px;>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 135px;position: absolute;font-size:11px;'>" + "   " + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div><div class='lpStatus'>" + value + "Active</div>  </div>" + ioaDivs + " </div>";
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(row["ID"].ToString()) > 0)
                                {
                                    LessonList += "<div style='background-color:#E8E80C; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList1' onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName1' style=height:64px;background:orange;>" + row["starttime"] + " - " + row["endtime"] + "</div> <div class='lpName2' style=margin-left: 50px;>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 135px;position: absolute;font-size:11px;'>" + "   " + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div><div class='lpStatus'>" + value + "Active</div>  </div>" + ioaDivs + " </div>";
                                }
                                else
                                {
                                    LessonList += "<div style='position:relative;cursor:default !important' title='" + row["Name"] + "' class='ClsLessonPlanList1' ><div class='lpName1' style=height:64px;background:white;>" + row["starttime"] + " - " + row["endtime"] + "</div> <div class='lpName2' style=margin-left: 50px;>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 135px;position: absolute;font-size:11px;'></div><div class='rightBox'> <div class='rdBox'><div class='d'></div><div class='r'></div></div><div class='lpStatus'>Notes</div>  </div> </div>";
                                }
                            }

                        }
                        else if (Url == "Datasheet.aspx" && opt != "Scheduleview" || Url == "Datasheet_listview.aspx")
                        {
                            string ioaDivs = "";
                            string Counter = "";
                            if (Convert.ToInt16(row["Counter"].ToString()) != 0)
                            {
                                Counter = "#" + Convert.ToInt16(row["Counter"].ToString());
                            }
                            if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 1)
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span><div class='ioaNeed' style='float:right' title='Require IOA to proceed.'>IOA</div></div>";
                            }
                            else if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 2)
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span><div class='mtNeed' style='float:right' title='Require Multi Teacher to proceed.'>MT</div></div>";
                            }
                            else if (Convert.ToInt16(row["IsMT_IOA"].ToString()) == 3)
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span><div class='ioaNeed' style='float:right' title='Require IOA to proceed.'>IOA</div><div class='mtNeed' style='float:right' title='Require Multi Teacher to proceed.'>MT</div></div>";
                            }
                            else
                            {
                                ioaDivs = "<div class='leftDiv'><span style='color:blue'>" + Counter + "</span></div>";
                            }

                            if (Convert.ToInt32(row["ModifiedCnt"]) == 0)
                            {
                                row["ModifiedDate"] = "";
                            }

                            if (row["StatusID"].ToString() == StatusIdApp)
                                LessonList += "<div style='background-color:#6bc963; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName'>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 70px;position: absolute;'>" + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Active</div>  </div>" + ioaDivs + " </div> ";
                            else
                                LessonList += "<div style='background-color:#E8E80C; position:relative;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='addBehaviour();selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName'>" + LessonPlanName + "</div><div style='float:left;font-weight:bold;color:red;margin:50px 0 0 70px;position: absolute;'>" + row["ModifiedDate"] + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Maintenance</div>  </div>" + ioaDivs + " </div> ";
                        }
                        else
                        {
                            //string d_Flag = (row["DayFlag"].ToString() == "true") ? "None" : "Block"; style='background-color:#CBED91;style='background-color:#F6B8B0;style='background-color:#FFBB8E;
                            // string r_Flag = (row["DayFlag"].ToString() == "true") ? "None" : "Block";

                            //<span class='RD_R' style='dispay:" + r_Flag + ";'>R</span><span class='RD_D' style='dispay:" + d_Flag + ";'>D</span>
                            if (row["StatusID"].ToString() == StatusIdApp)
                                LessonList += "<div style='background-color:#6bc963;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName'>" + LessonPlanName + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Active</div>  </div> </div> ";
                            else if (row["StatusID"].ToString() == StatusIdIn)
                                LessonList += "<div style='background-color:#A6A6A6;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName'>" + LessonPlanName + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Inactive</div>  </div> </div> ";
                            else
                                LessonList += "<div style='background-color:#E8E80C;' id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><div class='lpName'>" + LessonPlanName + "</div><div class='rightBox'> <div class='rdBox'><div class='d'>" + ((row["DayFlag"].ToString() == "1") ? "D" : "") + "</div><div class='r'>" + ((row["ResFlag"].ToString() == "1") ? "R" : "") + "</div></div> <div class='lpStatus'>Maintenance</div></div> </div> ";
                        }
                        //else
                        //    LessonList += "<div id='" + Url + "' title='" + row["Name"] + "' class='ClsLessonPlanList'  onclick='selSubmenu(this," + row["ID"] + ",0); CloseOverlayOnSelect(); '><br clear='all'> " + LessonPlanName + "     </div>";
                    }
                    LoopCnt++;
                }
                    LessonList += "</tr></table>";
                    if (Convert.ToInt32(HttpContext.Current.Session["current"]) == 1 && count < 51)
                    {
                        if (HttpContext.Current.Session["type"] != "previous")
                        {
                            LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                            LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                        }
                        else
                        {
                            LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                            LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                        }
                    }
                    else if (Convert.ToInt32(HttpContext.Current.Session["current"]) == 1)
                    {
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                    }
                    else if (count < 51)
                    {
                        HttpContext.Current.Session["last"] = Convert.ToInt32(HttpContext.Current.Session["current"]);
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Previous\");'>PREVIOUS</span> |";
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'></span> ";
                    }
                    else if (Convert.ToInt32(HttpContext.Current.Session["current"]) < Convert.ToInt32(HttpContext.Current.Session["last"]))
                    {
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Previous\");'>PREVIOUS</span> |";
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                    }

                    else
                    {
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Previous\");'>PREVIOUS</span> |";
                        LessonList += "<span style='cursor: pointer; font-style: italic; color: blue; text-decoration: underline;' onclick='ListLessonPopup(\"Next\");'>NEXT</span> ";
                    }
            }
        }
        return LessonList;
    }

    public DataTable LoadPreviousPage(string Tab, string SearchCondition, int option)
    {
        HttpContext.Current.Session["current"] = Convert.ToInt32(HttpContext.Current.Session["current"]) - 1;
        int fetch = Convert.ToInt32(HttpContext.Current.Session["current"]);
        if (fetch == 1)
        {
            fetch = 0;
        }
        else
        {
            fetch = fetch * 50;
        }
        HttpContext.Current.Session["type"] = "previous";
        string LessonList = "";
        int lastitem = 0;
        int lastitem1 = 0;
        int count = 0;
        objData = new clsData();
        clsLessons oLessons = new clsLessons();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        DataTable dtSubmenu = new DataTable();
        dtSubmenu.Columns.Add("Name", typeof(string));
        dtSubmenu.Columns.Add("ID", typeof(string));
        dtSubmenu.Columns.Add("StatusID", typeof(string));
        string Url = "";
        string optString = HttpContext.Current.Session["optString"].ToString();
        if (Convert.ToInt32(HttpContext.Current.Session["lastitem1"]) != 0)
        {
           lastitem1 = Convert.ToInt32(HttpContext.Current.Session["lastitem1"]);
        }
        if (Convert.ToInt32(HttpContext.Current.Session["lastitem"]) != 0)
        {
           lastitem = Convert.ToInt32(HttpContext.Current.Session["lastitem"]);
        }
        if (Convert.ToInt32(HttpContext.Current.Session["count"]) != 0)
        {
            int y = Convert.ToInt32(HttpContext.Current.Session["count"]);
            int x = Convert.ToInt32(HttpContext.Current.Session["count1"]);

            if (x == 0 && y < 50)
            {
                count = y;
            }
            else
            {
                count = 50;
            }
        }
        HttpContext.Current.Session["optString"] = optString.ToString();

        string dayOrResi = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) as DayFlag, " +
                "(SELECT TOP 1 lessonplantyperesi FROM stdtlessonplan STLP WHERE STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag";

        #region LessonPlanTab
        if (Tab == "LessonPlanTab")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = "SELECT * FROM ( SELECT  distinct top(" + (count + 1) + ") * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
    " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
    " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +  //OR LookupName='Inactive'
    " LookupName='Maintenance'))) DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_maintenance")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = "SELECT * FROM ( SELECT  distinct top(" + (count + 1) + ") * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Maintenance' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_approve")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = "SELECT * FROM ( SELECT  distinct top(" + (count + 1) + ") * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_inactive")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = "SELECT * FROM ( SELECT  distinct * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Inactive' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder OFFSET " + fetch + " ROWS FETCH NEXT 51 ROWS ONLY) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_rd")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = "SELECT * FROM ( SELECT  distinct top(" + (count + 1) + ") * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(lessonorder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR LookupName='Maintenance' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%')LP) DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        #endregion
        #region Datasheet
        else if (Tab == "DatasheetTab")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            HttpContext.Current.Session["datasheet"] = Tab.ToString();
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')";
            string Result = "SELECT * FROM ( SELECT  distinct top(" + (count + 1) + ")" + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder <" + lastitem1 + " ORDER BY  LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "DatasheetTab_maintenance")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Maintenance' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Maintenance' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            string Result = " SELECT * FROM ( SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder <" + lastitem1 + " ORDER BY  LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "DatasheetTab_approved")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Approved' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            string Result = "SELECT * FROM ( SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder <" + lastitem1 + " ORDER BY  LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "DatasheetTab_rd")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            string Result = "SELECT * FROM ( SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder <" + lastitem1 + " ORDER BY  LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        #endregion
        #region GraphTab
        else if (Tab == "GraphTab")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM ( SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance')))  DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_rd")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM ( SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR " +
 " LookupName='Maintenance')) " + optString + ")LP)  DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_inactive")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            string strQuery = "SELECT * FROM ( SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Inactive' " +
" )) " + optString + ")  DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_approved")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            string strQuery = "SELECT * FROM ( SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
 " )) " + optString + ")  DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_maintenance")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            string strQuery = "SELECT * FROM ( SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( " +
 " LookupName='Maintenance')) " + optString + ")  DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        #endregion
        #region ChainGraphTab
        else if (Tab == "ChainGraphTab")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM (SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained'" +
 "AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance'))) DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_rd")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM (SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR " +
 " LookupName='Maintenance')) " + optString + ")LP) DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_inactive")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM (SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( LookupName='Inactive' " +
 " )) " + optString + ") DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_approved")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM (SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
" AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
" )) " + optString + ") DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_maintenance")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = "SELECT * FROM (SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( " +
 " LookupName='Maintenance')) " + optString + ") DATA WHERE LessonOrder <" + lastitem1 + " ORDER BY  DATA.LessonOrder DESC ) DATA ORDER BY lessonorder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        #endregion
        int count1 = dtSubmenu.Rows.Count;
        if (count1 > 0)
        {
            if (HttpContext.Current.Session["type"] != "previous")
            {
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[1]["lessonorder"].ToString();
                if (count1 == 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
                }
                else if (count1 > 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 2]["lessonorder"].ToString();
                }
            }
            else
            {
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[1]["lessonorder"].ToString();
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count1 - 1]["lessonorder"].ToString();
            }
            int a = Convert.ToInt32(HttpContext.Current.Session["lastitem1"]);
            int b = Convert.ToInt32(HttpContext.Current.Session["lastitem"]);
        }
        else
        {
            HttpContext.Current.Session["lastitem1"] = lastitem1;
            HttpContext.Current.Session["lastitem"] = lastitem;
        }
        if (count1 < 50 || count ==0)
        {
            HttpContext.Current.Session["count"] = 51;
        }
        else
        {
            HttpContext.Current.Session["count"] = count1;
        }
        return dtSubmenu;
    }

    public DataTable LoadNextPage(string Tab, string SearchCondition, int option)
    {
        int fetch = Convert.ToInt32(HttpContext.Current.Session["current"]) * 50;
        HttpContext.Current.Session["current"] = Convert.ToInt32(HttpContext.Current.Session["current"]) + 1;
        HttpContext.Current.Session["type"] = "next";
        string LessonList = "";
        int lastitem = 0;
        int lastitem1 = 0;
        objData = new clsData();
        clsLessons oLessons = new clsLessons();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        DataTable dtSubmenu = new DataTable();
        dtSubmenu.Columns.Add("Name", typeof(string));
        dtSubmenu.Columns.Add("ID", typeof(string));
        dtSubmenu.Columns.Add("StatusID", typeof(string));
        string Url = "";
        if (Convert.ToInt32(HttpContext.Current.Session["lastitem1"]) != 0)
        {
            lastitem1 = Convert.ToInt32(HttpContext.Current.Session["lastitem1"]);
        }
        if (Convert.ToInt32(HttpContext.Current.Session["lastitem"]) != 0)
        {
            lastitem = Convert.ToInt32(HttpContext.Current.Session["lastitem"]);
        }
        HttpContext.Current.Session["Tab"] = Tab.ToString();
        string Tab1 = HttpContext.Current.Session["Tab"].ToString();
        string page=HttpContext.Current.Session["page"].ToString();
        string optString = HttpContext.Current.Session["optString"].ToString();

        string dayOrResi = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) as DayFlag, " +
                "(SELECT TOP 1 lessonplantyperesi FROM stdtlessonplan STLP WHERE STLP.lessonplanid = DSTempHdr.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag";

        Url = "LessonPlanAttributes.aspx";
        #region LessonPlanTab
        if (Tab == "LessonPlanTab_approve")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_inactive")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = " SELECT  distinct * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Inactive' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder" +
 " OFFSET " + fetch + " ROWS FETCH NEXT 51 ROWS ONLY ";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_maintenance")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = " SELECT  distinct * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Maintenance'" +
 ")) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA ORDER BY  DATA.LessonOrder"+
 " OFFSET " + fetch + " ROWS FETCH NEXT 51 ROWS ONLY ";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab_rd")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = " SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR LookupName='Maintenance' " +
 " )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%')LP) DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "LessonPlanTab")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Maintenance' " +
" )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
 //       else if (Tab == "LessonPlanTab_rd")
 //       {
 //           Url = "LessonPlanAttributes.aspx";
 //           HttpContext.Current.Session["Url"] = Url.ToString();
 //           string Result = " SELECT  distinct top(51) * FROM (SELECT LessonPlanTypeDay as DayFlag,LessonPlanTypeResi as ResFlag,DSTemplateName as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 //" AND HDR.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR LookupName='Maintenance')) ORDER BY DSTempHdrId DESC) ID, " +
 //" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 //" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR LookupName='Maintenance' " +
 //" )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
 //           dtSubmenu = objData.ReturnDataTable(Result, false);
 //       }
        else if (Tab == "LessonPlanTab_next")
        {
            Url = "LessonPlanAttributes.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string Result = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Maintenance' " +
" )) " + optString + " AND (DSTempHdr.DSMode IS NULL OR DSTempHdr.DSMode='MAINTENANCE' OR DSTempHdr.DSMode='' OR DSTempHdr.DSMode='Inactive') AND DSTempHdr.DSTemplateName like +'%'+'" + SearchCondition + "'+'%') DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        #endregion
        #region DatasheetTab
        else if (Tab == "DatasheetTab")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            HttpContext.Current.Session["datasheet"] = Tab.ToString();
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')";
            string Result = " SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder ," +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder >" + lastitem + " ORDER BY  LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "DatasheetTab_maintenance")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Maintenance' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            string Result = " SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder >" + lastitem + " ORDER BY  LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "DatasheetTab_approved")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Approved' )" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            string Result = " SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder >" + lastitem + " ORDER BY  LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        else if (Tab == "DatasheetTab_rd")
        {
            Url = "Datasheet.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            dtSubmenu = oLessons.getDSLessonPlans(sess.StudentId.ToString(), "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = "+ sess.StudentId.ToString() +" ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'");
            string columns = "(SELECT TOP 1 lessonplantypeday FROM stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) DayFlag,(SELECT TOP 1 lessonplantyperesi FROM   stdtlessonplan STLP WHERE  STLP.lessonplanid = LP.lessonplanid AND STLP.studentid = " + sess.StudentId + " ORDER  BY STLP.stdtlessonplanid DESC) AS ResFlag,DTmp.IsMT_IOA,DTmp.DSTemplateName AS Name,DTmp.DSTempHdrId as ID,DTmp.StatusId as StatusID,'! Updated ' +CONVERT(VARCHAR(50),CONVERT(VARCHAR(50),(CASE WHEN DTmp.ModifiedOn IS NULL THEN CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.CreatedOn THEN DTmp.CreatedOn END ELSE CASE WHEN DATEADD(DAY,-3,GETDATE())<= DTmp.ModifiedOn THEN DTmp.ModifiedOn END END),101))+' !' ModifiedDate,(SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=StdtLP.LessonPlanId AND StudentId=StdtLP.StudentId AND StatusId NOT IN (SELECT LookupId FROM LookUp WHERE (LookupName='Deleted' OR LookupName='In Progress' OR LookupName='Pending Approval') AND LookupType='TemplateStatus') AND VerNbr IS NOT NULL) ModifiedCnt";
            string status = "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')" + optString + " AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='') AND DTmp.DSTemplateName like +'%'+'" + SearchCondition + "'+'%'";
            string Result = " SELECT  distinct top(51) " + columns + ",ISNULL(DTmp.LessonOrder, 0 ) AS lessonorder, " +
                "(SELECT ISNULL((SELECT COUNT(SessionNbr) FROM StdtSessionHdr WHERE LessonPlanId = StdtLP.lessonplanid AND StudentId = StdtLP.studentid AND SessionStatusCd = 'S' AND IOAInd <> 'Y' AND CONVERT(char(10),ModifiedOn,101) = CONVERT(char(10),GETDATE(),101)),0))AS Counter" +
                         " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                           " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                               "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE (StdtLP.LessonPlanTypeDay IS NOT NULL OR StdtLP.LessonPlanTypeResi IS NOT NULL) AND StdtLP.StudentId=" + sess.StudentId.ToString() + " AND " +
                                     "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND " + status + " AND DTmp.LessonOrder >" + lastitem + " ORDER BY  LessonOrder";
            dtSubmenu = objData.ReturnDataTable(Result, false);
        }
        #endregion
        #region GraphTab
        else if (Tab == "GraphTab")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan ON DSTempHdr.StdtLessonplanId=StdtLessonPlan.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance')))  DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_rd")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR " +
 " LookupName='Maintenance')) " + optString + ")LP) DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_inactive")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            int StatusIdIna = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Inactive' "));
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Inactive' " +
" )) " + optString + ")  DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_approved")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            int StatusIdAppr = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Approved' "));
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
 " )) " + optString + ")  DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "GraphTab_maintenance")
        {
            Url = "AcademicSessionReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            int StatusIdMai = Convert.ToInt16(objData.FetchValue("Select  LookupId from LookUp where LookupType='TemplateStatus' And LookupName='Maintenance' "));
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( " +
 " LookupName='Maintenance')) " + optString + ")  DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        #endregion
        #region ChainGraphTab
        else if (Tab == "ChainGraphTab")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTempHdr.SkillType='Chained'" +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR " +
 " LookupName='Maintenance'))) DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_rd")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT LP.DayFlag,LP.ResFlag,(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE LP.LessonPlanId=HDR.LessonPlanId and HDR.StatusId = LP.StatusID " +
                "AND HDR.StudentId=" + sess.StudentId + " ORDER BY HDR.DSTempHdrId DESC) as ID,LP.StatusID,LP.lessonorder from (SELECT " + dayOrResi + ",DSTempHdr.LessonPlanId, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' OR LookupName='Inactive' OR " +
 " LookupName='Maintenance')) " + optString + ")LP) DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_inactive")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",(SELECT TOP 1 DSTemplateName FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) as Name,(SELECT TOP 1 DSTempHdrId FROM DSTempHdr HDR WHERE HDR.LessonPlanId=DSTempHdr.LessonPlanId " +
 " AND HDR.StudentId=" + sess.StudentId + " AND HDR.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='Inactive') ORDER BY HDR.DSTempHdrId DESC) ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( LookupName='Inactive' " +
 " )) " + optString + ") DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_approved")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
" DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
" INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
" AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND (LookupName='Approved' " +
" )) " + optString + ") DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        else if (Tab == "ChainGraphTab_maintenance")
        {
            Url = "ChainedBarGraphReport.aspx";
            HttpContext.Current.Session["Url"] = Url.ToString();
            string strQuery = " SELECT  distinct top(51) * FROM (SELECT " + dayOrResi + ",DSTemplateName as Name,DSTempHdr.DSTempHdrId as ID, " +
 " DSTempHdr.StatusId as StatusID,ISNULL(LessonOrder, 0 ) AS lessonorder FROM DSTempHdr INNER JOIN StdtLessonPlan StdtLP ON DSTempHdr.StdtLessonplanId=StdtLP.StdtLessonPlanId " +
 " INNER JOIN  DSTempPrompt P ON DSTempHdr.DSTempHdrId = P.DSTempHdrId AND P.ActiveInd = 'A' WHERE DSTempHdr.StudentId=" + sess.StudentId + " AND DSTemplateName like +'%'+'" + SearchCondition + "'+'%' AND DSTempHdr.SkillType='Chained' " +
 " AND  DSTempHdr.StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND ( " +
 " LookupName='Maintenance')) " + optString + ") DATA WHERE LessonOrder >" + lastitem + " ORDER BY  DATA.LessonOrder";
            dtSubmenu = objData.ReturnDataTable(strQuery, false);
        }
        #endregion
        int count1 = Convert.ToInt32(HttpContext.Current.Session["count"]);
        int count = dtSubmenu.Rows.Count;
        if (count == 51)
        {
            if (HttpContext.Current.Session["type"] != "previous")
            {
                if (count > 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 2]["lessonorder"].ToString();
                }
                else if (count1 > 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 1]["lessonorder"].ToString();
                }
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            else
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 1]["lessonorder"].ToString();
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["count"] = count1.ToString();
        }
        else if (count < 51 && count > 0)
        {
            if (HttpContext.Current.Session["type"] != "previous")
            {
                if (count > 1)
                {
                    HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 2]["lessonorder"].ToString();
                }
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            else
            {
                HttpContext.Current.Session["lastitem"] = dtSubmenu.Rows[count - 1]["lessonorder"].ToString();
                HttpContext.Current.Session["lastitem1"] = dtSubmenu.Rows[0]["lessonorder"].ToString();
            }
            HttpContext.Current.Session["count"] = count.ToString();
            HttpContext.Current.Session["count1"] = count1.ToString();
        }
        else if (count == 0)
        {
            HttpContext.Current.Session["lastitem"] = lastitem;
            HttpContext.Current.Session["lastitem1"] = lastitem + 1;
            HttpContext.Current.Session["count"] = count1.ToString();
            HttpContext.Current.Session["count1"] = count.ToString();
        }
        return dtSubmenu;
    }



    [WebMethod]
    public static bool setActiveStudents(int ID)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        if (sess == null) return false;
        string strSql = "";
        bool retVal = false;
        bool alredyIn = objData.IFExists("Select StudentId from StdtCheckedIn where StudentId=" + sess.StudentId + " And CheckInDate=(Select CONVERT(DATE,GETDATE(),101) AS DateOnly) And CheckIn Is Not Null ");

        if (alredyIn == false)
        {
            strSql = "Insert into StdtCheckedIn (StudentId,UserId,CheckInDate,CheckIn,CreatedBy,CreatedOn) values(" + sess.StudentId + "," + sess.LoginId + ",getdate(), CONVERT(TIME,GETDATE())," + sess.LoginId + ",getdate())";
            retVal = Convert.ToBoolean(objData.ExecuteWithScope(strSql));
        }
        else
        {
            // checkOut = DateTime.Now.ToString("HH:mm:ss");
        }

        return true;

    }
    [WebMethod]
    public static bool setIDSess(int ID)
    {
        object Year = null;
        object stat = null;

        status = false;
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        if (sess == null) return false;
        ObjTempSess = (ClsTemplateSession)HttpContext.Current.Session["BiweeklySession"];
        if (ObjTempSess == null) return false;
        ObjTempSess.bOpenMode = false;
        string menu = Convert.ToString(HttpContext.Current.Session["MenuItem"]);
        if (menu == "IEPS")
        {
            status = true;
            ObjTempSess.StdtIEPId = ID;
            sess.IEPId = ID;
            try
            {
                if (ID != 0)
                {
                    Year = objData.FetchValue("Select AsmntYearId from StdtIEP where StdtIEPId=" + ID + "");
                    if (Year != null) sess.YearId = Convert.ToInt32(Year);
                    stat = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + ID + "");
                    if (stat != null) sess.IEPStatus = Convert.ToInt32(stat);

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        else if (menu == "DATASHEETS")
        {
            ObjTempSess.TemplateId = ID;
        }
        else
        {
            status = false;
            HttpContext.Current.Session["MenuItem"] = null;
        }
        return status;

    }

    [WebMethod]
    public static void setMenuSess(string menu)
    {
        string Menu = menu.Replace("\n", "").Trim();
        HttpContext.Current.Session["MenuItem"] = Menu;
    }
    /// <summary>
    /// 
    /// Below code is a web method which is used to make communication with client side scripting using Ajax Call. 
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// 

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    /// Function that called by the script function in client side and return the values to the clent page.
    /// 
    public static string checkTemplateHeader(int parameter)
    {
        int tempId = parameter;
        int nextSet = 0;
        int LesPlanID = 0;
        string setName = "";
        string lPlanName = "";
        int status = 0;
        List<LessonPlanInfo> LessonPlans = new List<LessonPlanInfo>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString1"].ToString()))
        {


            string commandString1 = "select LessonPlanId,NextSetId from StdtDSStat where DSTempHdrId=" + tempId;

            SqlDataReader reader;

            SqlCommand command = new SqlCommand(commandString1, con);

            con.Open();

            reader = command.ExecuteReader();

            if (reader.Read())
            {

                LesPlanID = Convert.ToInt32(reader["LessonPlanId"]);

                nextSet = Convert.ToInt32(reader["NextSetId"]);

                reader.Close();

                con.Close();

            }



            else
            {

                reader.Close();

                string sql = "INSERT INTO StdtDSStat (SchoolId,StudentId,DSTempHdrId,LessonPlanId,NextSetId,NextStepId,NextPromptId,NextSessionNbr" +

                    ",CreatedBy,CreatedOn) VALUES(1,8," + tempId + ",0,'1','0','0','0',1,GETDATE())";

                command = new SqlCommand(sql, con);

                command.ExecuteNonQuery();

                string commandString = "select SetName,SortOrder from DSTempSet where SortOrder in (Select min(SortOrder) as CurrentSet from DSTempSet) and DSTempHdrId=" + tempId;

                command = new SqlCommand(commandString, con);

                reader = command.ExecuteReader();

                while (reader.Read())
                {

                    setName = reader["SetName"].ToString();
                    nextSet = Convert.ToInt32(reader["SortOrder"]);

                }

                reader.Close();
            }

            con.Open();

            string sql1 = "SELECT SetName FROM DSTempSet WHERE SortOrder=" + nextSet + " AND DSTempHdrId=" + tempId + "";

            command = new SqlCommand(sql1, con);

            setName = command.ExecuteScalar().ToString();


            con.Close();


            LessonPlanInfo Lesson = new LessonPlanInfo();

            Lesson.lesPlanName = lPlanName.ToString();

            Lesson.NextSetId = nextSet;

            Lesson.NextSetName = setName;

            LessonPlans.Add(Lesson);

        }
        return new JavaScriptSerializer().Serialize(LessonPlans);

    }



    public class LessonPlanInfo
    {
        public string lesPlanName;
        public int NextSetId;
        public string NextSetName;
    }




    //////////// alarm count/////////////////
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getReminder(string TimeNow, string StudId)
    {
        string result = "";
        var newAlert = 0;
        try
        {
            //HttpContext.Current.Session
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            if (sess != null)
            {

                string s = StudId;
                if (sess.LoginId > 0)
                {
                    DateTime alarmtime = DateTime.Parse(TimeNow);
                    clsData obj = new clsData();
                    DataTable dt = new DataTable();
                    string temp = "";
                    int counter = 0;
                    string sqlStr = "select ST.StudentLname+' '+ST.StudentFname AS Name,LP.LessonPlanName,EVT.Set_MoveUp,EVT.Set_MoveDown,EVT.Step_MoveUp,EVT.Step_MoveDown,EVT.Prompt_MoveUp," +
                        "EVT.Prompt_MoveDown,EVT.IOAEvntStatus,EVT.MultiTchrEvntStatus from StdtSessEvent EVT inner join LessonPlan LP on EVT.LessonPlanId=LP.LessonPlanId inner join Student ST on EVT.StudentId=ST.StudentId";
                    dt = obj.ReturnDataTable(sqlStr, true);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Set_MoveUp"].ToString() == "true")
                        {
                            if (dr["IOAEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",IOA Required,Set MoveUp" + ",,";
                                counter++;
                            }
                            if (dr["MultiTchrEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",Multi-Teacher Required,Set MoveUp" + ",,";
                                counter++;
                            }
                        }
                        if (dr["Set_MoveDown"].ToString() == "true")
                        {
                            if (dr["IOAEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",IOA Required,Set MoveDown" + ",,";
                                counter++;
                            }
                            if (dr["MultiTchrEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",Multi-Teacher Required,Set MoveDown" + ",,";
                                counter++;
                            }
                        }
                        if (dr["Step_MoveUp"].ToString() == "true")
                        {
                            if (dr["IOAEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",IOA Required,Step MoveUp" + ",,";
                                counter++;
                            }
                            if (dr["MultiTchrEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",Multi-Teacher Required,Step MoveUp" + ",,";
                                counter++;
                            }
                        }
                        if (dr["Step_MoveDown"].ToString() == "true")
                        {
                            if (dr["IOAEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",IOA Required,Step MoveDown" + ",,";
                                counter++;
                            }
                            if (dr["MultiTchrEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",Multi-Teacher Required,Step MoveDown" + ",,";
                                counter++;
                            }
                        }
                        if (dr["Prompt_MoveUp"].ToString() == "true")
                        {
                            if (dr["IOAEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",IOA Required,Prompt MoveUp" + ",,";
                                counter++;
                            }
                            if (dr["MultiTchrEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",Multi-Teacher Required,Prompt MoveUp" + ",,";
                                counter++;
                            }
                        }
                        if (dr["Prompt_MoveDown"].ToString() == "true")
                        {
                            if (dr["IOAEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",IOA Required,Prompt MoveDown" + ",,";
                                counter++;
                            }
                            if (dr["MultiTchrEvntStatus"].ToString() == "true")
                            {
                                temp += dr["Name"].ToString() + "," + dr["LessonPlanName"].ToString() + ",Multi-Teacher Required,Prompt MoveDown" + ",,";
                                counter++;
                            }
                        }
                    }
                    string alarmQry = "select BehaviorReminder.BehaviourReminderId,BehaviorReminder.BehaviourCalcId,BehaviourDetails.Behaviour,BehaviourCalc.Date,BehaviourCalc.StartTime," +
                        "Student.StudentFname+' '+Student.StudentLname as 'StudentName',BehaviourCalc.IOAFlag,[User].UserLName,BehaviourCalc.EndTime  from BehaviorReminder inner join BehaviourCalc on " +
                        "BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId inner join BehaviourDetails on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId" +
                        " inner join Student on Student.StudentId=BehaviourDetails.StudentId left join [User] on [USER].UserId = BehaviourCalc.IOAUser where" +
                        " BehaviorReminder.UserId=" + sess.LoginId + " and BehaviourDetails.ActiveInd='A' and DateDiff(day, BehaviourCalc.Date, getdate()) = 0 and " +
                        //to include alerts after 10 minutes
                        //"BehaviourCalc.StartTime < '" + alarmtime.AddMinutes(10).TimeOfDay + "'  and BehaviorReminder.DismissStatus='true'" +
                        "BehaviourCalc.StartTime = '" + alarmtime.TimeOfDay.Hours +":"+ alarmtime.TimeOfDay.Minutes + ":00' and BehaviorReminder.DismissStatus='true'" +
                        //for removing expired alerts
                        //" and BehaviourCalc.EndTime > '" + alarmtime.AddMinutes(0).TimeOfDay + "'" +
                        " and DateDiff(day, BehaviourCalc.Date, getdate()) = 0 order by StartTime desc";
                    DataTable alarmDetails = obj.ReturnDataTable(alarmQry, true);
                    string alarmCountQry = "select COUNT(BehaviorReminder.BehaviourReminderId) from BehaviorReminder inner join BehaviourCalc on" +
                        " BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId inner join BehaviourDetails on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId" +
                        " inner join Student on Student.StudentId=BehaviourDetails.StudentId where BehaviorReminder.UserId=" + sess.LoginId + " and BehaviourDetails.ActiveInd='A'" +
                        //to include alerts after 10 minutes
                        //" and DateDiff(day, BehaviourCalc.Date, getdate()) = 0 and BehaviourCalc.StartTime < '" + alarmtime.AddMinutes(10).TimeOfDay + "'" +
                        " and DateDiff(day, BehaviourCalc.Date, getdate()) = 0 and BehaviourCalc.StartTime = '" + alarmtime.TimeOfDay.Hours +":"+ alarmtime.TimeOfDay.Minutes + ":00'" +
                        //for removing expired alerts
                        " and BehaviourCalc.EndTime > '" + alarmtime.AddMinutes(0).TimeOfDay + "'" +
                        "  and BehaviorReminder.DismissStatus='true'";
                    object ret = obj.FetchValue(alarmCountQry);
                    int alarmcout = int.Parse(ret.ToString());
                    int row = alarmDetails.Rows.Count;
                    int col = alarmDetails.Columns.Count;
                    result = result + alarmcout.ToString() + ",," + alarmDetails.Rows.Count.ToString() + ",," + alarmDetails.Columns.Count;
                    foreach (DataRow dr in alarmDetails.Rows)
                    {
                        for (int i = 0; i < alarmDetails.Columns.Count; i++)
                        {
                            result = result + "," + dr[i].ToString();
                        }
                        result = result + ",,";
                        if (!Remainder.ContainsKey(Convert.ToInt32(dr[0])))
                        {
                            Remainder.Add(Convert.ToInt32(dr[0]), 0);
                            newAlert = 1;
                    }
                    }

                    //result = result + "$" + counter+",,2,,2,,1,msg00,msg01,,2,msg10,msg11,,";
                    //result = result + "$" + counter + ",," + temp;
                    result = result + "$" + counter + ",," + temp + "$" + newAlert;
                    //  return value to client
                }
                else
                {
                    //result = "0,,0,,$0,,";
                    result = "0,,0,,$0,,$0";
                }
            }


        }
        catch (Exception Ex)
        {
            return result;
            throw Ex;
        }
        return result;
    }
    //AlarmStatusUpdate

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string AlarmStatusUpdate(string ReminderId)
    {
        try
        {
            //HttpContext.Current.Session

            clsData obj = new clsData();


            string alarmUpdateQry = "update BehaviorReminder set DismissStatus='false' where BehaviourReminderId=" + int.Parse(ReminderId) + "";
            int rowAffcdCount = obj.Execute(alarmUpdateQry);

            return rowAffcdCount.ToString();
        }
        catch (Exception Ex)
        {
            return "error";
            throw Ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string AlarmSnooZUpdate(string ReminderId, string snoozTime, string conditionTest)
    {
        try
        {
            //HttpContext.Current.Session


            clsData obj = new clsData();
            if ((obj.IFExists("select BehaviourCalc.BehaviourCalcId from BehaviourDetails inner join BehaviourCalc on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId inner join BehaviorReminder on BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId where BehaviorReminder.UserId=" + 1 + " and DateDiff(day, BehaviourCalc.Date, getdate()) = 0 and BehaviourCalc.StartTime='" + DateTime.Parse(snoozTime).TimeOfDay + "'") == false) || (conditionTest == "updateIt"))
            {
                //  string alarmUpdateQry = "update BehaviorReminder set DismissStatus='false' where BehaviourReminderId=" + int.Parse(ReminderId) + "";
                string getmesureIdQry = "select BehaviourCalc.BehaviourCalcId  from  BehaviorReminder inner join BehaviourCalc  on BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId where BehaviourReminderId=" + int.Parse(ReminderId) + "";
                object BhCalcId = obj.FetchValue(getmesureIdQry);
                int ChkUpdate = 0;
                int BhaviourCalcid = Convert.ToInt32(BhCalcId);
                if (BhaviourCalcid > 0)
                {
                    string alarmSnooZUpdateQry = "update BehaviourCalc set StartTime='" + DateTime.Parse(snoozTime).TimeOfDay + "' where BehaviourCalcId=" + BhaviourCalcid + "";
                    ChkUpdate = obj.Execute(alarmSnooZUpdateQry);

                    string periodString = "select BehaviourDetails.Period from BehaviourDetails inner join BehaviourCalc on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId where BehaviourCalc.BehaviourCalcId=" + BhaviourCalcid + "";
                    object EndObj = obj.FetchValue(periodString);


                    string PeriodStr = EndObj.ToString();
                    int period = int.Parse(PeriodStr);
                    DateTime endTime = DateTime.Parse(snoozTime);

                    string upDateEnd = "update BehaviourCalc set EndTime='" + endTime.AddMinutes(period).TimeOfDay + "' where BehaviourCalcId=" + BhaviourCalcid + "";
                    obj.Execute(upDateEnd);


                }
                return ChkUpdate.ToString();
            }
            else
            {
                return "existing";
            }




        }
        catch (Exception Ex)
        {
            return "error";
            throw Ex;
        }
    }
    //////////////////////////////////////



    //protected void btn_edit_Click(object sender, ImageClickEventArgs e)
    //{
    //    //sess = (clsSession)Session["UserSession"];
    //    //sess.StudentId = Convert.ToInt32(_studID);
    //  //  Response.Redirect("~/Administration/IEPDocumentaion.aspx");
    //}
    protected void lnk_home_Click1(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        sess.AdminView = 1;
        Response.Redirect("~/Administration/AdminHome.aspx");
    }

    protected void lnk_logout_Click1(object sender, EventArgs e)
    {
        Response.Redirect("../Logout.aspx");
    }
    protected void imgsearch_Click(object sender, ImageClickEventArgs e)
    {
        LBLClassnotfound.Text = "";
        sess = (clsSession)Session["UserSession"];
        string DlClass = "";
        if (txtSname.Text.Trim() != "Student Name")
        {
            objData = new clsData();
            //if (objData.IFExists("SELECT SCLS.ClassId FROM StdtClass SCLS INNER JOIN Student ST ON ST.StudentId=SCLS.StdtId WHERE ST.ActiveInd='A' AND (ST.StudentFname='" + txtSname.Text.Trim() + "' OR ST.StudentLname='" + txtSname.Text.Trim() + "' OR ST.StudentFname+' '+ST.StudentLname='" + txtSname.Text.Trim() + "' OR ST.StudentLname+' '+ST.StudentFname='" + txtSname.Text.Trim() + "' OR ST.StudentLname LIKE +'%'+'" + txtSname.Text.Trim() + "'+'%'  OR ST.StudentFname LIKE +'%'+'" + txtSname.Text.Trim() + "'+'%') and SCLS.ActiveInd='A' ") == true)
            if (objData.IFExists("SELECT SCLS.ClassId FROM StdtClass SCLS INNER JOIN Student ST ON ST.StudentId=SCLS.StdtId WHERE ST.ActiveInd='A' AND (ST.StudentFname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentFname+' '+ST.StudentLname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname+' '+ST.StudentFname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname LIKE +'%'+'" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "'+'%'  OR ST.StudentFname LIKE +'%'+'" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "'+'%') and SCLS.ActiveInd='A' ") == true)
            {

                //string classdetail = "SELECT DISTINCT Id,Name FROM (SELECT SCLS.ClassId AS Id,Cls.ClassName AS Name FROM StdtClass SCLS INNER JOIN Student ST ON ST.StudentId=SCLS.StdtId INNER JOIN " +
                //                     "Class Cls ON Cls.ClassId=SCLS.ClassId INNER JOIN Placement plc on ST.StudentId=plc.StudentPersonalId AND plc.Location=SCLS.ClassId WHERE ST.ActiveInd='A' AND (plc.EndDate is null or convert(DATE,plc.EndDate) >= convert(DATE,getdate())) AND (ST.StudentFname='" + txtSname.Text.Trim() + "' OR ST.StudentLname='" + txtSname.Text.Trim() + "' OR ST.StudentFname+' '+ST.StudentLname='" + txtSname.Text.Trim() + "' OR ST.StudentLname+' '+ST.StudentFname='" + txtSname.Text.Trim() + "' OR ST.StudentLname LIKE +'%'+'" + txtSname.Text.Trim() + "'+'%'  OR ST.StudentFname LIKE +'%'+'" + txtSname.Text.Trim() + "'+'%') AND Cls.ActiveInd='A' " +
                //                     "AND SCLS.ActiveInd='A' ) AS STDTCLASS LEFT JOIN UserClass UCLS ON Id=UCLS.ClassId WHERE UCLS.ActiveInd='A'";

                string classdetail = "SELECT DISTINCT Id,Name FROM (SELECT SCLS.ClassId AS Id,Cls.ClassName AS Name FROM StdtClass SCLS INNER JOIN Student ST ON ST.StudentId=SCLS.StdtId INNER JOIN " +
                                     "Class Cls ON Cls.ClassId=SCLS.ClassId INNER JOIN Placement plc on ST.StudentId=plc.StudentPersonalId AND plc.Location=SCLS.ClassId WHERE ST.ActiveInd='A' AND (plc.EndDate is null or convert(DATE,plc.EndDate) >= convert(DATE,getdate())) AND (ST.StudentFname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentFname+' '+ST.StudentLname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname+' '+ST.StudentFname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname LIKE +'%'+'" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "'+'%'  OR ST.StudentFname LIKE +'%'+'" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "'+'%') AND Cls.ActiveInd='A' " + 
                                     "AND SCLS.ActiveInd='A' ) AS STDTCLASS LEFT JOIN UserClass UCLS ON Id=UCLS.ClassId WHERE UCLS.ActiveInd='A'";

                string classactivedetail = "SELECT DISTINCT Id,Name FROM (SELECT SCLS.ClassId AS Id,Cls.ClassName AS Name FROM StdtClass SCLS INNER JOIN Student ST ON ST.StudentId=SCLS.StdtId INNER JOIN " +
                                     "Class Cls ON Cls.ClassId=SCLS.ClassId INNER JOIN Placement plc on ST.StudentId=plc.StudentPersonalId AND plc.Location=SCLS.ClassId AND plc.status=1 WHERE ST.ActiveInd='A' AND (plc.EndDate is null or convert(DATE,plc.EndDate) >= convert(DATE,getdate())) AND (ST.StudentFname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentFname+' '+ST.StudentLname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname+' '+ST.StudentFname='" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "' OR ST.StudentLname LIKE +'%'+'" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "'+'%'  OR ST.StudentFname LIKE +'%'+'" + clsGeneral.convertQuotes(txtSname.Text.Trim()) + "'+'%') AND Cls.ActiveInd='A' " +
                                     "AND SCLS.ActiveInd='A' ) AS STDTCLASS LEFT JOIN UserClass UCLS ON Id=UCLS.ClassId WHERE UCLS.ActiveInd='A'";

                DataTable dt = objData.ReturnDataTable(classdetail, false);
                DataTable activedt = objData.ReturnDataTable(classactivedetail, false);
                if (dt == null) return;
                if (dt.Rows.Count > 0)
                {
                    if (activedt != null)
                        if (activedt.Rows.Count > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClass').empty();", true);
                            foreach (DataRow row in activedt.Rows)
                            {
                                string functn = "ChangeClassId(" + row["Id"] + ");";
                                DlClass += "<div class=\"grmb\" id=" + row["Id"] + " onclick=" + functn + " >" + row["Name"] + "</div>";
                            }
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClass').append('" + DlClass + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClass').empty();", true);
                            LBLClassnotfound.Text = "Student not found";
                        }

                }
                else if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClass').empty();", true);
                    LBLClassnotfound.ForeColor = System.Drawing.Color.Red;
                    LBLClassnotfound.Text = "You are not autherized to Access this Student's Class";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClass').empty();", true);

                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClass').empty();", true);
                LBLClassnotfound.Text = "Student not found";
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "classBind();", true);

        }

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static Boolean checkAlertCount(int studId)
    {
        clsData oData = new clsData();
        //string checkRemider = "SELECT COUNT(*) FROM BehaviorReminder WHERE UserId=" + sess.LoginId + " AND  StudentId=" + sess.StudentId + " AND BehaviourCalcId=" + remainder;  // UserId=" + sess.LoginId + " and
        string checkRemider = "SELECT COUNT(*) FROM BehaviorReminder br join BehaviourCalc bc on br.BehaviourCalcId=bc.BehaviourCalcId WHERE br.StudentId=" + studId + " AND Date=CONVERT(DATE,GETDATE())";  // UserId=" + sess.LoginId + " and
            object ret = oData.FetchValue(checkRemider);
            int alarmcout = int.Parse(ret.ToString());
            if (!(alarmcout > 0))
            {
                return false;
            }
            return true;
    }
    protected void imgSearchDsch_Click(object sender, ImageClickEventArgs e)
    {
        LblStudentNotFound.Text = "";
        sess = (clsSession)Session["UserSession"];
        string DlClass = "";
        string studentDetail;
        objData = new clsData();
        if (txtSnameDsch.Text.Trim() == "Choose Discharged Client")
        {
            studentDetail = "SELECT Distinct StudentPersonalId AS Id, FirstName+' ' +LastName AS Name FROM StudentPersonal WHERE StudentType='client' and clientid>0 "+//and StudentPersonalId not in" +
                             //"(SELECT Distinct ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId WHERE (PLC.EndDate is null or PLC.EndDate>=cast (GETDATE() as DATE)) and PLC.Status=1 )"+
                             "AND PlacementStatus='D'";
        }
        else
        {
            studentDetail = "SELECT Id, Name FROM (SELECT distinct StudentPersonalId AS Id, FirstName+' ' +LastName AS Name FROM StudentPersonal WHERE StudentType='client' and clientid>0 "+//and StudentPersonalId not in" +
                             //"(SELECT Distinct ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId WHERE (PLC.EndDate is null or PLC.EndDate>=cast (GETDATE() as DATE)) and PLC.Status=1 )"+
                            "AND (FirstName='" + clsGeneral.convertQuotes(txtSnameDsch.Text.Trim()) + "' " +
                            "OR LastName='" + clsGeneral.convertQuotes(txtSnameDsch.Text.Trim()) + "' OR FirstName+LastName='" + clsGeneral.convertQuotes(txtSnameDsch.Text.Trim()) + "' OR LastName+FirstName='" + clsGeneral.convertQuotes(txtSnameDsch.Text.Trim()) + "' " +
                            "OR LastName LIKE '%" + clsGeneral.convertQuotes(txtSnameDsch.Text.Trim()) + "%' OR FirstName LIKE '%" + clsGeneral.convertQuotes(txtSnameDsch.Text.Trim()) + "%') AND PlacementStatus='D') AS STDTCLASS";
        }
        DataTable dt = objData.ReturnDataTable(studentDetail, false);
        if (dt == null) return;
        if (dt.Rows.Count > 0)
        {
            int stdId = 0;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClassDsch').empty();", true);
            foreach (DataRow row in dt.Rows)
            {
                stdId = Convert.ToInt32(row["Id"]);
                string functn = "FillDischargedStudents(" + stdId + ");";
                DlClass += "<div class=\"grmb\" id=" + row["Id"] + " onclick=" + functn + " >" + row["Name"] + "</div>";
            }
            DlClass = DlClass.Replace('\'', ' ');
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClassDsch').append('" + DlClass + "');", true);


        }
        else if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "$('#DlClassDsch').empty();", true);
            LblStudentNotFound.Text = "Student not found";
        }
    }
    protected void btnOpenDischargeDiv(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "openDischargeDiv();", true);
    }
    private int classIdCheck()
    {
        string cs = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("SELECT ClassId FROM Class WHERE ClassName='Discharged'", con);
            con.Open();
            return (Convert.ToInt32(cmd.ExecuteScalar()));
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string enableAllAlert()
    {
        clsData oData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int studId = sess.StudentId;
        DataTable DTable = new DataTable();
        if (!studId.Equals(0))
        {
            var alertCheck = checkAlertCount(studId);
            if (!alertCheck)
            {
                string Behaviorid = "SELECT MeasurementId,Behaviour FROM BehaviourDetails WHERE MeasurementId IN (SELECT DISTINCT MeasurmentId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE())) AND ActiveInd='A'";
                DTable = oData.ReturnDataTable(Behaviorid, false);
                int behLength = 0;
                foreach (DataRow dr in DTable.Rows)
                {
                    string insertRemider = "INSERT INTO BehaviorReminder(UserId,StudentId,BehaviourCalcId,DismissStatus) SELECT " + sess.LoginId + "," + sess.StudentId + "," + "behaviourcalcid,'true' FROM BehaviourCalc WHERE MeasurmentId=" + dr["MeasurementId"].ToString() + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE()) AND StudentId=" + sess.StudentId + " AND IsPartial='True' ORDER BY StartTime";
                    oData.Execute(insertRemider);
                    behLength++;
                }
                if (!(behLength > 0))
                {
                    behLength = 0;
                    return "2";
                }
                var CurrtDateTime = DateTime.Now;
                var CurTime = CurrtDateTime.ToString().Substring(0, 8);
                //getReminder(CurTime,"2");
            }
            return "0";
        }
        return "1";
    }

}
