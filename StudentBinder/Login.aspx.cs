using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentBinder_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Redirect("~/Administration/Error.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
}