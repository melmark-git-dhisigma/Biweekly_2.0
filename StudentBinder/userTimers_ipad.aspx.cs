using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_userTimers_ipad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_addTimer_Click(object sender, EventArgs e)
    {
        clsSession sess = (clsSession)Session["UserSession"];
    }

    [WebMethod]
    public static string getStudentDetails()
    {
        clsSession sess = (clsSession)HttpContext.Current.Session["UserSession"];
        if (sess != null && sess.StudentId > 0)
        {
            string[] stdNameArray = sess.StudentName.Split(',');
            string studentNameCorrect = stdNameArray[1] + "," + stdNameArray[0];
            return sess.StudentId + "^" + studentNameCorrect;
        }
        else
        {
            return "0^ ";
        }
    }

}