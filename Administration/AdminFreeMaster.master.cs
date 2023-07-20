using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Script.Services;

public partial class AdminMasterFree : System.Web.UI.MasterPage
{
    public static clsSession sess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            sess.AdmStudentId = 0;
        }

        if (!IsPostBack)
        {
            if (sess != null)
            {
                lblLoginName.Text = sess.UserName;
                if (sess.Gender == "Male")
                {
                    imgUserIcon.ImageUrl = "~/Administration/images/boyIcon.png";
                }
                else
                {
                    imgUserIcon.ImageUrl = "~/Administration/images/GirlIcon.png";
                }
                string studInfo = clsGeneral.PageStudentInfo();
                if (studInfo != "Student Info")
                {
                    string Title = clsGeneral.PageTitle(sess.perPageName);
                    Page.Title = "Melmark :: " + Title;
                    if (Title == "Welcome") Title = "Welcome " + sess.UserName;
                    HeadingDiv.InnerText = Title;
                }
                else
                {
                    Page.Title = "Melmark :: " + studInfo;
                    HeadingDiv.InnerText = studInfo;
                }
                fillclass();

            }

        }

       

    }

    public void fillclass()
    {
        sess = (clsSession)Session["UserSession"];
        clsData objData = new clsData();
        if (objData.IFExists("SELECT ClassId from UserClass where UserId='" + sess.LoginId + "'") == true)
        {
            string classdetail = "SELECT UsrCls.ClassId AS Id,Cls.ClassName AS Name FROM UserClass UsrCls INNER JOIN Class Cls ON Cls.ClassId=UsrCls.ClassId WHERE UsrCls.UserId=" + sess.LoginId + " AND Cls.ActiveInd='A'";

            DataTable dt = objData.ReturnDataTable(classdetail, false);
            if (dt.Rows.Count > 0)
            {
                Lblnoclass.Visible = false;
                DlClass.Visible = true;
                lblchoose.Visible = true;
                DlClass.DataSource = dt;
                DlClass.DataBind();
            }
            else
            {
                Lblnoclass.Visible = true;
                DlClass.Visible = false;
                lblchoose.Visible = false;
                Lblnoclass.Text = "Class Not Assigned...";
            }
        }
    }





    protected void lnk_logout_Click(object sender, EventArgs e)
    {

    }




    protected void DlClass_ItemCommand(object source, DataListCommandEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        sess.Classid = Convert.ToInt32(e.CommandArgument);
        Response.Redirect("~/StudentBinder/Home.aspx");
    }
   
}
