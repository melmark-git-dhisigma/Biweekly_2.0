using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Data;
using System.DirectoryServices;
using System.Collections;

public partial class Admin_Login : System.Web.UI.Page
{

    DataClass oClass = new DataClass();
    public Int32 UserId = 0;
    clsSession objSession = null;
    clsData objData = null;
    ClsTemplateSession ObjTempSess = null;
    bool IsLogined = false;
    clsLoginValues objLogins = null;
    clsAciveDirectory objActiveLogin = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objSession = (clsSession)Session["UserSession"];

        ////Redirectting to Login in Master - Start - Uncomment below section in this page and take the build
        //if (objSession == null)
        //{

        //    //Response.Redirect("Login.aspx");
        //    string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
        //    Response.Redirect(baseUrl);
        //}
        ////Redirectting to Login in Master - End
            
        if (IsPostBack == false)
        {
            txtPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + lnkLogin.UniqueID + "').click();return false;}} else {return true}; ");
            txtUserName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + lnkLogin.UniqueID + "').click();return false;}} else {return true}; ");
            txtUserName.Focus();
        }
    }


    public void fillclass()
    {

        clsData objData = new clsData();
        if (objData.IFExists("SELECT ClassId from UserClass where UserId='" + objSession.LoginId + "'") == true)
        {
            DataTable dt = objData.ReturnDataTable("SELECT UsrCls.ClassId AS Id,Cls.ClassName AS Name FROM UserClass UsrCls INNER JOIN Class Cls ON Cls.ClassId=UsrCls.ClassId WHERE UsrCls.UserId=" + objSession.LoginId + " AND Cls.SchoolId=" + objSession.SchoolId + "  AND Cls.ActiveInd='A' And UsrCls.ActiveInd='A' ORDER BY Name ASC ", false);
            DdlClass.DataSource = dt;
            DdlClass.DataTextField = "Name";
            DdlClass.DataValueField = "Id";
            DdlClass.DataBind();
            DdlClass.Items.Insert(0, new ListItem("------------Select Class------------", "0"));
        }
    }
    private void SetUserSession(int UserID)
    {
        if (UserID != 0)
        {
            Session["BiweeklySession"] = ObjTempSess;
            objData = new clsData();
            string strQuery = "SELECT Role.RoleId, Role.RoleDesc as RoleName, [User].UserId, [User].UserLName +', '+ [User].UserFName  AS LoginName,[User].Gender ," +
                " [User].SchoolId FROM Role INNER JOIN RoleGroup ON Role.RoleId = RoleGroup.RoleId CROSS JOIN [User] WHERE        [User].UserId = " + UserId + "";
            DataTable Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt == null) return;
            if (Dt.Rows.Count > 0)
            {
                ArrayList ar = new ArrayList();
                ArrayList arBinder = new ArrayList();
                Hashtable hs = new Hashtable();
                objSession = new clsSession();
                objSession.IsLogin = true;
                objSession.LoginTime = (DateTime.Now.ToShortTimeString()).ToString();
                objSession.SchoolId = Convert.ToInt32(Dt.Rows[0]["SchoolId"]);
                objSession.LoginId = Convert.ToInt32(Dt.Rows[0]["UserId"]);
                objSession.UserName = Convert.ToString(Dt.Rows[0]["LoginName"]);
                objSession.RoleId = Convert.ToInt32(Dt.Rows[0]["RoleId"]);
                objSession.Gender = Convert.ToString(Dt.Rows[0]["Gender"]);
                objSession.RoleName = Convert.ToString(Dt.Rows[0]["RoleName"]);
                objSession.SessionID = Session.SessionID.ToString();
                objSession.YearId = Convert.ToInt32(objData.FetchValue("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'"));
                objSession.AdminView = 1;
                clsGeneral.PageLister(objSession.LoginId, objSession.SchoolId, out ar, out hs);
                clsGeneral.PageBinderLister(objSession.LoginId, objSession.SchoolId, out arBinder);
                objSession.perPage = ar;
                objSession.perPageName = hs;
                objSession.perPageBinder = arBinder;
                Session["UserSession"] = objSession;
            }
        }
        else
        {
            Session["UserSession"] = null;
        }
    }
    private int SQLServerLogined()
    {
        string strQry = "";
        if (objLogins.IsActiveLogin == "Y")
        {
            strQry = "SELECT UserId,Login,Password FROM [User] WHERE Login = CONVERT(varbinary(180),'" + txtUserName.Text.Trim() + "')  AND ActiveInd<>'D'";
        }
        else
        {
            strQry = "SELECT UserId,Login,Password FROM [User] WHERE Login = CONVERT(varbinary(180),'" + txtUserName.Text.Trim() + "')   AND Password=CONVERT(varbinary(180),'" + txtPassword.Text.Trim() + "') AND ActiveInd<>'D'";
        }
        try
        {
            objData.Dispose();
            UserId = Convert.ToInt32(objData.FetchValue(strQry));
        }
        catch (Exception exp)
        {
            tdMessage.InnerText = "You seem to have entered wrong credentials ,Please Try Again !";
            throw exp;
        }
        return UserId;

    }


    private bool ActiveLogin()
    {
        objLogins = (clsLoginValues)Session["ActiveLogin"];

        if (objLogins != null && objLogins.IsActiveLogin == "Y")
        {
            try
            {
                if (objLogins != null)
                {
                    objActiveLogin = new clsAciveDirectory();
                    if (objActiveLogin.IsActiveDirectoryLogin(objLogins.LoginVals, objLogins.DomainName, txtUserName.Text.Trim(), txtPassword.Text.Trim()) == true)
                    {
                        IsLogined = true;
                    }
                    else
                    {
                        IsLogined = false;
                    }
                }
            }
            catch
            {

            }
        }
        else
        {
            IsLogined = true;
        }
        return IsLogined;
    }
    protected void lnkLogin_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtUserName.Text == "")
            {
                tdMessage.InnerText = "You seem to have entered wrong credentials !";
                txtUserName.Focus();
                tdMessage.Visible = true;
                return;
            }
            else if (txtPassword.Text == "")
            {
                tdMessage.InnerText = "You seem to have entered wrong credentials !";
                txtPassword.Focus();
                tdMessage.Visible = true;
                return;
            }
            objData = new clsData();
            if (txtPassword.Text == "") return;

            if (ActiveLogin() == true)
            {
                UserId = SQLServerLogined();

                if (UserId >= 1)
                {
                    SetUserSession(UserId);
                }
                objSession = (clsSession)Session["UserSession"];
                if (objSession != null && UserId >= 1)
                {
                    fillclass();
                    if (DdlClass.Items.Count > 0)
                    {
                        if (DdlClass.Items.Count == 2)
                        {
                            objSession.Classid = Convert.ToInt32(DdlClass.Items[1].Value);

                            Response.Clear();
                            Response.Redirect("~/StudentBinder/Home.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else if (DdlClass.Items.Count == 1)
                        {
                            Response.Clear();
                            Response.Redirect("~/Administration/AdminHome.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            Classdiv.Visible = true;
                            Logindiv.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Clear();
                        Response.Redirect("~/Administration/AdminHome.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }


                }
                else
                {
                    tdMessage.Visible = true;
                    tdMessage.InnerText = "You seem to have entered wrong credentials ,Please Try Again !";
                }
            }
            else
            {
                tdMessage.Visible = true;
                tdMessage.InnerText = "You seem to have entered wrong credentials ,Please Try Again !";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }





    protected void lnkBtnAdmin_Click1(object sender, EventArgs e)
    {
        try
        {

            objSession = (clsSession)Session["UserSession"];
            if (objSession != null)
            {
                objSession.Classid = Convert.ToInt32(DdlClass.SelectedValue);
                Response.Clear();
                Response.Redirect("~/Administration/AdminHome.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    protected void imgLoginSel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (DdlClass.SelectedIndex > 0)
            {
                objSession = (clsSession)Session["UserSession"];
                if (objSession != null)
                {
                    objSession.Classid = Convert.ToInt32(DdlClass.SelectedValue);
                    Response.Clear();
                    Response.Redirect("~/StudentBinder/Home.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
            else
                LblError.Text = "Please Select Class";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}