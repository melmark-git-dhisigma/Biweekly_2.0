using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Error1 : System.Web.UI.Page
{
    private static clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            //Response.Redirect("../Login.aspx");
            //Response.Redirect("Login.aspx");
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            Response.Redirect(baseUrl);
        }
        if (Request.QueryString["Error"] != null)
        {
            //pError.InnerHtml = Request.QueryString["Error"].ToString();
        }
    }
}