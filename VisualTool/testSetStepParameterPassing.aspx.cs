using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class testSetStepParameterPassing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnParameter_Click(object sender, EventArgs e)
    {
         int stepValue = 0;
        int setValue = Convert.ToInt32(txtSets.Text);
        if (txtSteps.Text != "")
        {
             stepValue = Convert.ToInt32(txtSteps.Text);
        }
        else
        {
            stepValue = 0;
        }
        int noOfAttempts = Convert.ToInt32(txtNoOfAttempts.Text);
        Response.Redirect("TeachPage_timeEditor.aspx?stN="+ setValue +"&spN="+ stepValue +"&nofAttempts="+noOfAttempts);
    }
}