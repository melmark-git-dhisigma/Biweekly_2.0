using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dopostback_tesing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["__EVENTTARGET"] == "Button2")
        {
            //fire event
            Button2_Click(this, new EventArgs());
        }


       
    }
    public void click(object sender, EventArgs e)
    {
        Response.Write("hek");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Write("hek");
    }
}