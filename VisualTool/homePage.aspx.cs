using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class homePage : System.Web.UI.Page
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
            }
            setTitle();
        }
    }


    private void setTitle()
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            object obj = objData.FetchValue("Select SchoolDesc from School Where SchoolId=" + sess.SchoolId + "");
            if (obj != null)
            {
                TitleName.InnerText = obj.ToString();

               
            }
        }
    }


    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Logout.aspx");
    }
}