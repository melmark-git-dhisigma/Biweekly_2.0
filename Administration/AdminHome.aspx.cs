using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Net;

public partial class Admin_AdminHome : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    public static clsData objclsData = null;
    string strQuery = "";
    ClsTemplateSession ObjTempSess = null;
    clsGeneral clsGeneralSchk = new clsGeneral();
    clsSession Prevsess = null;
    int preid = 0;
    string preuser = "";
    int SessStudentid = 0;
    int PreClassid = 0;
    string Sessstname = "";
    string curesesid = "";
    string Pagepath = "";
    string ip = "";
    string HostName = "";
    string userAgent ="";
    DateTime now = DateTime.Now;
    DateTime today = DateTime.Today;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
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
                preuser = Session["Spreuser"].ToString();
                SessStudentid = Convert.ToInt16(Session["Sprestid"]);
                Sessstname = Session["Sprestname"].ToString();
                PreClassid = Convert.ToInt16(Session["SpreClsid"]);
                curesesid = this.Session.SessionID.ToString();
            }

            curesesid = this.Session.SessionID.ToString();
            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AdminHome_PageLoad";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);


            fillMenu();
            sess.AdmStudentId = 0;
        }

        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];      
        Session["BiweeklySession"] = null;
        sess.StudentId = -9;
       
    }


    private void fillMenu()
    {
        string MenuId = "";
        MenuId = "M";// for Administation
        Session["MenuId"] = MenuId;
        dlAdmin.DataSource = Heading(MenuId);
        dlAdmin.DataBind();

        MenuId = "MS";// for App Configuration
        Session["MenuId"] = MenuId;
        dlAppConfig.DataSource = Heading(MenuId);
        dlAppConfig.DataBind();
       
        if (sess.CurrentMenuId != 0)
        {
            MenuId = sess.CurrentMenuId.ToString();
            dlAdmin.DataSource = Heading(MenuId);
            dlAdmin.DataBind();
        }

    }

    private DataTable Heading(string MenuId)
    {
        string ImagePath = Server.MapPath("~/Administration/Images/");
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        curesesid = this.Session.SessionID.ToString();
        HostName = Dns.GetHostName();
        userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "AdminHome_Heading";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);


        strQuery = "Select Top 6 O.ObjectId as Id,O.ObjectDispName as Heading,O.ObjectImageUrl as ImagePath from Object O ";
        strQuery += "Where  O.ParntObjectId=1 And ObjectType='" + MenuId + "' And  O.ObjectId IN ( Select O.ParntObjectId  from Object O Inner Join RoleGroupPerm RGP ON O.ObjectId=RGP.ObjectId Inner Join  ";
        strQuery += "UserRoleGroup URG ON RGP.RoleGroupId= URG.RoleGroupId where URG.UserId=" + sess.LoginId + " And URG.ActiveInd='A'  And (RGP.ReadInd=1 Or RGP.WriteInd=1) And O.ParntObjectId<>0 ) ";


        DataTable DtHeading = objData.ReturnDataTable(strQuery, false);

        if (DtHeading != null)
        {
            if (DtHeading.Rows.Count != 0)
            {
                tdMenu.Visible = true;
                tdMenu1.Visible = true;
                if (MenuId == "M")  // for Administation Label
                    lblAdministration.Text = "Administration";
                else if (MenuId == "MS")  // for Appconfig Label
                    LblAppConfig.Text = "App Configuration";
            }
        }
        return DtHeading;

    }
    protected void dlMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HiddenField hdValue = (HiddenField)e.Item.FindControl("hidValue");
        if (hdValue.Value != "")
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            curesesid = this.Session.SessionID.ToString();
            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AdminHome_dlMenu_ItemDataBound";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);


            strQuery = "Select DISTINCT O.ObjectDispName as SubHead,O.ObjectURL as Url,SortOrder from Object O Inner Join RoleGroupPerm RGP On O.ObjectId=RGP.ObjectId ";
            strQuery += "Inner Join UserRoleGroup URG On RGP.RoleGroupId=URG.RoleGroupId Where URG.UserId=" + sess.LoginId + "" ;
            strQuery += " And (RGP.ReadInd=1 Or RGP.WriteInd=1) And ParntObjectId=" + hdValue.Value + " AND URG.ActiveInd = 'A' order By SortOrder Asc ";

            DataTable DtSub = objData.ReturnDataTable(strQuery, false);
            DataList dlSub = (DataList)e.Item.FindControl("dlSub");
            dlSub.DataSource = DtSub;
            dlSub.DataBind();
        }
    }





    protected void dlAppConfig_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HiddenField hdValue = (HiddenField)e.Item.FindControl("hidValue");
        if (hdValue.Value != "")
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            curesesid = this.Session.SessionID.ToString();
            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "AdminHome_dlAppConfig_ItemDataBound";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);


            strQuery = "Select DISTINCT O.ObjectDispName as SubHead,O.ObjectURL as Url,SortOrder from Object O Inner Join RoleGroupPerm RGP On O.ObjectId=RGP.ObjectId ";
            strQuery += "Inner Join UserRoleGroup URG On RGP.RoleGroupId=URG.RoleGroupId Where URG.UserId=" + sess.LoginId + "";
            strQuery += " And (RGP.ReadInd=1 Or RGP.WriteInd=1) And ParntObjectId=" + hdValue.Value + " AND URG.ActiveInd = 'A'  AND ObjectType='P' order By SortOrder Asc ";

            DataTable DtSub = objData.ReturnDataTable(strQuery, false);
            DataList dlSub = (DataList)e.Item.FindControl("dlSub");
            dlSub.DataSource = DtSub;
            dlSub.DataBind();
        }
    }



    [System.Web.Services.WebMethod]
    public static string GetBatchResult()
    {
        try
        {
            objclsData = new clsData();
            object ResultSet = objclsData.ExecuteSetSp();
            object Result = objclsData.ExecuteSp();
            string FinalResult = "";
            if (Result.ToString() == "SUCCESS" && ResultSet.ToString() == "SUCCESS")
            {
                object obj = objclsData.FetchValue("SELECT CONVERT(VARCHAR(20),Last_run_date,101) FROM StdtLoadDates");
                FinalResult = "Execute batch[" + obj + "]";
                //lnkExecute.ToolTip = "Last executed in: " + obj;
            }
            else
            {
                FinalResult = "Execute batch[Failed]";
                //lnkExecute.ToolTip = "Failed";
            }

            return FinalResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}