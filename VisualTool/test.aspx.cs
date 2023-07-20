using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Phase002_1_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
        //Session["setValue"] = 2052;
        //Session["stepValue"] = 2053;
        //Response.Redirect("TeachPage_timeEditor.aspx");

        Session["setValue"] = 2058;
        Session["stepValue"] = 2059;
        Response.Redirect("TeachPage_mouseEditor.aspx");

    }
}