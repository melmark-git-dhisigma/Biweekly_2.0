﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


public class clsGeneral
{
    public static DateTime dateGeneral = Convert.ToDateTime("1900/01/01");
    static clsData objSData = null;
    private static bool IsTrue = false;
    static string strQuery = "";
    DateTime now = DateTime.Now;
    DateTime today = DateTime.Today;

    public static bool IsExit(string fieldName, string TableName, string Condition)
    {
        bool retVal = false;
        objSData = new clsData();
        strQuery = "Select " + fieldName + " from [" + TableName + "] Where " + fieldName + "='" + Condition.Trim() + "'";// AND ActiveInd = 'A'";
        retVal = objSData.IFExists(strQuery);
        return retVal;
    }
    public static bool IsExitGroup(string fieldName, string TableName, string Condition)
    {
        bool retVal = false;
        objSData = new clsData();
        strQuery = "Select " + fieldName + " from [" + TableName + "] Where " + fieldName + "='" + Condition.Trim() + "'";
        retVal = objSData.IFExists(strQuery);
        return retVal;
    }

    public static string popUp()
    {
        return "$(document).ready(function(){$('#overlay').fadeIn('slow', function () { $('#dialog').animate({ top: '20%' },{duration: 'slow',easing: 'linear'}) }); $('#close_x').click(function () {$('#dialog').animate({ top: '-300%' }, function () {$('#overlay').fadeOut('slow');});});});";
    }
    public static string popUp1()
    {
        return "$(document).ready(function(){$('#overlay1').fadeIn('slow', function () { $('#dialog1').animate({ top: '20%' },{duration: 'slow',easing: 'linear'}) });});";
    }

    public static string popUpfrIframe()
    {
        return "$(document).ready(function(){$('#overlay').fadeIn('slow', function () { $('#dialog').animate({ top: '8%' },{duration: 'slow',easing: 'linear'}) }); $('#close_x').click(function () {$('#dialog').animate({ top: '-300%' }, function () {$('#overlay').fadeOut('slow');});});});";
    }
    public clsGeneral()
    {

    }

    public static bool IsPhoneNumber(string Number)
    {
        string rgx = @"[\(\d]{3}\)-[\d]{3}-[\d]{4}$";
        //string rgx = @"^[2-9]\d{2}-\d{3}-\d{4}$";
        if (System.Text.RegularExpressions.Regex.IsMatch(Number, rgx))
        {
            IsTrue = true;
        }
        return IsTrue;
    }

    public static string getPageName()
    {
        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string[] words = PageUrl.Split('/');
        int len = words.Length;
        return words[len - 1].ToString();
    }
    public static string getPageURL()
    {
        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        return PageUrl;
    }

    public clsSession sessioncheck(string curesesid, int preid, string ip, string preuser, clsSession sess, clsSession Prevsess, int SessStudentid, int PreClassid, string Sessstname, string Pagepath)
    {
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        ClsSessionSwpLog SessSwpLog = new ClsSessionSwpLog();
        if (curesesid != "" && Prevsess != null)
        {
            if ((curesesid != sess.SessionID || preid != sess.LoginId || preuser != sess.UserName || SessStudentid != sess.StudentId || Sessstname != sess.StudentName || PreClassid != sess.Classid) && Prevsess != sess)
            {
                if (curesesid == Prevsess.SessionID)
                {

                    sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "SessCheck" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + "SessionFix" + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + "" + ',' + "");
                    SessSwpLog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "SessCheck" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + "SessionFix" + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + "" + ',' + "");
                    sess = Prevsess;
                    sess.LoginId = preid;
                    sess.UserName = preuser;
                    sess.StudentId = SessStudentid;
                    sess.StudentName = Sessstname;
                    sess.Classid = PreClassid;
                }
                else
                {
                    SessSwpLog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "SessCheck" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + "Cuid_Not_match_with_PrevessId" + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + "" + ',' + "");
                }
            }            
        }
        
        return sess;
    }

    public static clsSession sessionchecks(string curesesid, int preid, string ip, string preuser, clsSession sess, clsSession Prevsess, int SessStudentid, int PreClassid, string Sessstname, string Pagepath)
    {        
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        ClsSessionSwpLog SessSwpLog = new ClsSessionSwpLog();
        DateTime nows = DateTime.Now;
        DateTime todays = DateTime.Today;
        if (curesesid != "" && Prevsess != null)
        {
            if ((curesesid != sess.SessionID || preid != sess.LoginId || preuser != sess.UserName || SessStudentid != sess.StudentId || Sessstname != sess.StudentName || PreClassid != sess.Classid) && Prevsess != sess)
            {
                if (curesesid == Prevsess.SessionID)
                {

                    sesserrlog.WriteToLog(todays.ToString("yyyy-MM-dd") + ',' + nows.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "SessCheck" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + "SessionFix" + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + "" + ',' + "");
                    SessSwpLog.WriteToLog(todays.ToString("yyyy-MM-dd") + ',' + nows.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "SessCheck" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + "SessionFix" + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + "" + ',' + "");
                    sess = Prevsess;
                    sess.LoginId = preid;
                    sess.UserName = preuser;
                    sess.StudentId = SessStudentid;
                    sess.StudentName = Sessstname;
                    sess.Classid = PreClassid;
                }
                else
                {
                    SessSwpLog.WriteToLog(todays.ToString("yyyy-MM-dd") + ',' + nows.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "SessCheck" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + "Cuid_Not_match_with_PrevessId" + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + "" + ',' + "");
                }
            }
        }
        return sess;
    }

    public static string convertQuotes(string str)
    {
        return str.Replace("'", "''");
    }
    public static string convertDoubleQuotes(string str)
    {
        return str.Replace("\"", "'");
    }

    public static string EditImage()
    {
        return "~/Administration/images/user_edit.png";
    }
    public static string sucessMsg(string Msg)
    {
        return "<div class='valid_box'>" + Msg + ".</div>";
    }
    public static string failedMsg(string Msg)
    {
        return "<div class='error_box'>" + Msg + ".</div>";
    }
    public static string warningMsg(string Msg)
    {
        return "<div class='warning_box'>" + Msg + ".</div>";
    }



    public static bool setPermissionPage(string Page)
    {
        return false;
    }
    public static string PageStudentInfo()
    {
        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string[] words = PageUrl.Split('/');
        int len = words.Length;
        string PageName = words[len - 1].ToString();
        if (PageName == "StudentInfo.aspx") PageName = "Student Info";
        return PageName;
    }
    public static string PageTitle(Hashtable ar)
    {
        string PageName = "";
        string title = "";
        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string[] words = PageUrl.Split('/');
        int len = words.Length;
        PageName = words[len - 1].ToString();


        foreach (DictionaryEntry url in ar)
        {
            string v = url.Key.ToString();
            if (url.Key.ToString().Contains(PageName))
            {
                title = url.Value.ToString();
                break;
            }
        }
        if (title == "") title = "Welcome";
        if (PageName == "AddStudent.aspx") title = "Facesheet";
        return title;
    }

    public static bool PageIdentification(ArrayList ar)
    {
        string PageName = "";
        bool IsExit = false;
        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string[] words = PageUrl.Split('/');
        int len = words.Length;
        PageName = words[len - 1].ToString();
        if (PageName.Contains('?') == true)
        {
            string[] pagesname = PageName.Split('?');
            PageName = pagesname[0].ToString();
        }

        if (PageName == "UserCreate.aspx?MyProfile=View") PageName = "UserCreate.aspx";

        if (PageName == "AdminHome.aspx" || PageName == "AddSchool.aspx") //Added OR clause to allow addSchool.aspx
        {
            IsExit = true;
        }
        else
        {
            foreach (string Item in ar)
            {
                if (PageName.Contains(Item))
                {
                    if (PageName == "CustomizeTemplateEditor.aspx" && Item == "Edit")
                    { }
                    else
                    {
                        IsExit = true;
                        break;
                    }
                }
            }
        }


        return IsExit;
    }



    public static void PageLister(int UserId, int SchoolId, out ArrayList arr, out Hashtable hashArr)
    {
        string PageName = "";
        arr = new ArrayList();
        hashArr = new Hashtable();
        string strQuery = "";


        try
        {
            clsData objData = new clsData();

            strQuery = "Select DISTINCT O.ObjectName as Name,O.ObjectURL as URL,SortOrder from Object O Inner Join RoleGroupPerm RGP On O.ObjectId=RGP.ObjectId ";
            strQuery += "Inner Join UserRoleGroup URG On RGP.RoleGroupId=URG.RoleGroupId Where URG.UserId=" + UserId + "   ";
            strQuery += " And (RGP.ReadInd=1 Or RGP.WriteInd=1) AND URG.ActiveInd = 'A' And O.ObjectUrl is Not Null and ObjectType='P' order By SortOrder Asc ";


            DataTable Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    for (int index = 0; index < Dt.Rows.Count; index++)
                    {
                        arr.Add(Dt.Rows[index]["URL"]);
                        hashArr.Add(Dt.Rows[index]["URL"], Dt.Rows[index]["Name"]);
                    }


                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


    }
    public static void PageBinderLister(int UserId, int SchoolId, out ArrayList arr)
    {

        arr = new ArrayList();
        string strQuery = "";

        try
        {
            clsData objData = new clsData();

            strQuery = "Select DISTINCT O.ObjectName as Name,O.ObjectURL as URL,SortOrder from Object O Inner Join RoleGroupPerm RGP On O.ObjectId=RGP.ObjectId ";
            strQuery += "Inner Join UserRoleGroup URG On RGP.RoleGroupId=URG.RoleGroupId Where URG.UserId=" + UserId + "";
            strQuery += " And RGP.ReadInd=1 AND URG.ActiveInd = 'A' And O.ObjectUrl is Not Null and ObjectType='P' And O.ObjectName like '%menu%' order By SortOrder Asc ";


            DataTable Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt.Rows.Count > 0)
            {
                for (int index = 0; index < Dt.Rows.Count; index++)
                {
                    arr.Add(Dt.Rows[index]["Name"]);
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


    }
    public static void ApprovePermissions(int UserId, int SchoolId, out bool Approve)
    {
        string PageName = "";
        bool APerm = false;
        Approve = false;
        clsData objData = new clsData();
        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string[] words = PageUrl.Split('/');
        int len = words.Length;
        PageName = words[len - 1].ToString();

        if (PageName.Contains("?"))
        {
            string[] pages = PageName.Split('?');
            PageName = pages[0].ToString();
        }
        if (PageName == "LessonPlanAttributes.aspx")
        {
            PageName = "TemplateEditor.aspx";
        }




        string strSql = " SELECT RGP.ApproveInd FROM RoleGroupPerm RGP  INNER JOIN UserRoleGroup URG ON RGP.RoleGroupId = URG.RoleGroupId INNER JOIN  [Object] O ";
        strSql += "ON RGP.ObjectId = O.ObjectId WHERE    URG.SchoolId =  '" + SchoolId + "' AND  URG.UserId = '" + UserId + "'  and O.ObjectUrl='" + PageName + "'  ";


        DataTable Dt = objData.ReturnDataTable(strSql, false);
        try
        {
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        APerm = Convert.ToBoolean(Dr["ApproveInd"]);
                        if (APerm == true) break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        if (APerm == true)
        {
            Approve = false;
        }
        else
        {
            Approve = true;
        }
    }

    public static void PageReadAndWrite(int UserId, int SchoolId, out bool Disable)
    {
        clsData objData = new clsData();


        string PageName = "";
        Disable = false;
        bool Read = false;
        bool Write = false;

        string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string[] words = PageUrl.Split('/');
        int len = words.Length;
        PageName = words[len - 1].ToString();

        if (PageName.Contains("?"))
        {
            string[] pages = PageName.Split('?');
            PageName = pages[0].ToString();
        }
        if (PageName == "AddLessonPlan.aspx")
        {
            PageName = "TemplateLevel.aspx";
        }

        if (PageName != "")
        {


            string strSql = " SELECT RGP.ReadInd,RGP.WriteInd FROM RoleGroupPerm RGP  INNER JOIN UserRoleGroup URG ON RGP.RoleGroupId = URG.RoleGroupId INNER JOIN  [Object] O ";
            strSql += "ON RGP.ObjectId = O.ObjectId WHERE    URG.SchoolId =  '" + SchoolId + "' AND  URG.UserId = '" + UserId + "'  and O.ObjectUrl='" + PageName + "'";
            DataTable Dt = objData.ReturnDataTable(strSql, false);
            try
            {
                if (Dt != null)
                {
                    if (Dt.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt.Rows)
                        {
                            Read = Convert.ToBoolean(Dr["ReadInd"]);
                            Write = Convert.ToBoolean(Dr["WriteInd"]);

                            if (Write == true) break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            }

            if (Write == true)
            {
                Disable = false;
            }
            else
            {
                Disable = true;
            }
        }
    }

    public static void Btn_Write(int UserId, int SchoolId, out bool Disable)
    {
        clsData objData = new clsData();


        Disable = false;
        bool Read = false;
        bool Write = false;
        string PageName = "Override";

        string strSql = " SELECT RGP.ReadInd,RGP.WriteInd FROM RoleGroupPerm RGP  INNER JOIN UserRoleGroup URG ON RGP.RoleGroupId = URG.RoleGroupId INNER JOIN  [Object] O ";
        strSql += "ON RGP.ObjectId = O.ObjectId WHERE    URG.SchoolId =  '" + SchoolId + "' AND  URG.UserId = '" + UserId + "'  and O.ObjectUrl='" + PageName + "'";
        DataTable Dt = objData.ReturnDataTable(strSql, false);
        try
        {
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        Read = Convert.ToBoolean(Dr["ReadInd"]);
                        Write = Convert.ToBoolean(Dr["WriteInd"]);

                        if (Write == true) break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }

        if (Write == true)
        {
            Disable = false;
        }
        else
        {
            Disable = true;
        }

    }
    public void CheckStatus(DataListItem di, CheckBox chkReadAll, int val, string type)
    {
        string id = chkReadAll.ClientID;

        if (type == "R")
        {
            id = id.Replace("chkReadAll", "");
        }
        else if (type == "W")
        {
            id = id.Replace("chkWriteAll", "");
        }
        else if (type == "A")
        {
            id = id.Replace("chkApproveAll", "");
        }

        Panel p = (Panel)di.FindControl("panelsubcat");
        DataList dl2 = (DataList)di.FindControl("subcat");
        string dsId = dl2.ClientID;
        if (dsId.Contains(id))
        {
            foreach (DataListItem di2 in dl2.Items)
            {
                string idc = "";
                if (type == "R")
                {
                    CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                    idc = chkRead.ClientID;

                    if (idc.Contains(id))
                    {
                        chkRead.Checked = Convert.ToBoolean(val);
                    }
                }
                if (type == "W")
                {

                    CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");
                    CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                    idc = chkWrite.ClientID;

                    if (idc.Contains(id))
                    {
                        chkWrite.Checked = Convert.ToBoolean(val);
                        chkRead.Checked = Convert.ToBoolean(val);

                    }
                }

                if (type == "A")
                {

                    CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");
                    idc = chkApprove.ClientID;

                    if (idc.Contains(id))
                    {

                        chkApprove.Checked = Convert.ToBoolean(val);
                    }
                }
            }
        }
        if (p.ClientID.Contains(id))
        {
            p.Visible = true;
            //p.Attributes.Add("display", "none");
        }
    }


    public static void getValues(string content, out string[] arSave, out  string[] arDelete)
    {
        int i = 0, j = 0;
        string sv = "";
        string[] ar = content.Split(',');

        arSave = new string[ar.Length];
        arDelete = new string[ar.Length];

        foreach (string s in ar)
        {
            if (s.Contains("#"))
            {
                sv = s;
                sv = sv.Replace("#", "");
                arSave[i] = sv;
                i = i + 1;
            }

            if (s.Contains("&"))
            {
                sv = s;
                sv = sv.Replace("&", "");
                arDelete[j] = sv;
                j = j + 1;
            }


        }



    }


    public static bool getChecked(string x)
    {
        return x == "" ? false : Convert.ToBoolean(x);
    }







    public static bool IsItZip(string inputvalue)
    {

        if (IsItNumber(inputvalue) == true)
        {
            if (inputvalue.Length > 5)
            { return false; }
            else { return true; }
        }
        else
            return false;



        //Regex isnumber = new Regex("^([0-9]{5})$");

        //Match match = isnumber.Match(inputvalue);
        //if (match.Success)
        //    return true;
        //else
        //    return false;

    }
    public static bool IsItNumber(string inputvalue)
    {
        Regex isnumber = new Regex("^([0-9])");
        Match match = isnumber.Match(inputvalue);
        if (match.Success)
            return true;
        else
            return false;

    }
    public static bool IsItValidPhone(string inputvalue)
    {
        Regex rePhoneNumber = new Regex("^\\([0-9]{3}\\)[0-9]{3}-[0-9]{4}$");
        Match match = rePhoneNumber.Match(inputvalue);
        if (match.Success)
            return true;
        else
            return false;
    }
    public static bool IsItValidPassword(string pass)
    {
        if (pass.Length > 5 && pass.Length < 13)
        {
            return true;
        }
        else
        {
            return false;
        }

    }



    public static bool IsItEmail(string Email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(Email);
        if (match.Success)
            return true;
        else
            return false;
    }

    public static int GetTemplateHeaderId(int lessionId, string studentId, int schoolId)
    {
        clsData objData = new clsData();
        if (studentId == null || studentId == "" || studentId == "NULL" || studentId == "0")
        {
            studentId = " is null";
        }
        else
        {
            studentId = "= " + studentId;
        }
        int temphr = 0;
        object ret = null;

        strQuery = "select DSTempHdrId from dbo.DSTempHdr where LessonPlanId =" + lessionId + " and StudentId" + studentId + " and SchoolId=" + schoolId + " and statusid in(select lookupId from lookup  where LookupType='TemplateStatus' And LookupName='In Progress')";
        ret = objData.FetchValue(strQuery);

        try
        {
            temphr = Convert.ToInt32(ret);
        }
        catch (Exception Ex)
        {
            temphr = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        return temphr;
    }

    public static string HtmlToString(string convertString)
    {
        string ResultString = "";
        ResultString = convertString.Replace("&gt;", ">");
        ResultString = ResultString.Replace("&lt;", "<");
        return ResultString;
    }

    public static string StringToHtml(string convertString)
    {
        string ResultString = "";
        ResultString = convertString.Replace(">", "&gt;");
        ResultString = ResultString.Replace("<", "&lt;");
        return ResultString;
    }

    public static bool getSchoolCount()
    {
        int ret = 0;
        bool status = false;
        clsData objData = new clsData();
        strQuery = "select count(SchoolName) from School";
        ret = Convert.ToInt32(objData.FetchValue(strQuery));
        if (ret > 0)
            status = true;
        else status = false;
        return status;
    }

    public static string GetIPAddress()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
            {
                return addresses[0];
            }
        }

        return context.Request.ServerVariables["REMOTE_ADDR"];
    }
}