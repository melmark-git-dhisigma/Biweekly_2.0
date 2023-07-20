using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class testing_static : System.Web.UI.Page
{
    static int x = 0;
    int y = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label1.Text = x.ToString();
            Label2.Text = y.ToString();
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("testing_static.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        x = x + 1;
        Label1.Text = x.ToString();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        y = y + 1;
        Label2.Text = y.ToString();
    }
}