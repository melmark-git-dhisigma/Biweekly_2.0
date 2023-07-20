using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Configuration;
using System.Data;

public partial class StudentBinder_userTimers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_addTimer_Click(object sender, EventArgs e)
    {
        clsSession sess = (clsSession)Session["UserSession"];
    }

    [WebMethod]
    public static string getStudentDetails1()
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

    [WebMethod]
    public static string getStudentDetails()
    {
        clsSession sess = (clsSession)HttpContext.Current.Session["UserSession"];
        clsData oData = new clsData();
        if (sess != null)
        {
            string name = "";
            string selQry = "";
            string stdId = "";
            string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
            if (buildName == "Local")
            {
                selQry = "SELECT distinct StudentId as Id,(StudentFname+', '+StudentLname) as Name,ImageURL FROM StdtClass stdtcls left Join Student stdt ON stdt.StudentId=stdtcls.StdtId WHERE stdtcls.ClassId='" + sess.Classid + "' AND stdt.ActiveInd='A' And  stdt.SchoolId='" + sess.SchoolId + "' And stdtcls.ActiveInd='A'";
            }
            else
            {
                selQry = "SELECT distinct StudentId as Id,(StudentFname+', '+StudentLname) as Name,ImageURL FROM StdtClass stdtcls left Join Student stdt ON stdt.StudentId=stdtcls.StdtId  WHERE stdtcls.ClassId='" + sess.Classid + "' AND stdt.ActiveInd='A' And  stdt.SchoolId='" + sess.SchoolId + "' And stdtcls.ActiveInd='A'";
            }
            DataTable dt = new DataTable();
            dt = oData.ReturnDataTable(selQry, false);

            //var behName = "";
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {

                    name += row["Name"].ToString() + ";";
                    stdId += row["Id"].ToString() + ";";
                }
            }

            return stdId + "^" + name;
        }
        else
        {
            return "0^ ";
        }
    }
}