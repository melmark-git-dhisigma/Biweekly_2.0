using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class Administration_AddStudentNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsSession sess = (clsSession)Session["UserSession"];

        if (sess.SchoolId == 1)
        {
            pageFrame.Attributes["src"] = "Facesheet.aspx";
        }
        else if (sess.SchoolId == 2)
        {

            pageFrame.Attributes["src"] = "Facesheetview.aspx";

        }
    }
}

