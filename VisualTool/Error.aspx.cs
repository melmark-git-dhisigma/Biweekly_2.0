using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Error : System.Web.UI.Page
{
    private static clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            //Response.Redirect("Login.aspx");
            pError.InnerHtml = Request.QueryString["Error"].ToString();
        }
        if (Request.QueryString["Error"] != null)
        {
            pError.InnerHtml = Request.QueryString["Error"].ToString();
        }
    }
}