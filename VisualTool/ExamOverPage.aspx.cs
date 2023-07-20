using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class VisualTool_ExamOverPage : System.Web.UI.Page
{
    clsSession sess = null;
    clsData objData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillData();
        }
        
    }

    protected void FillData()
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        string studentName = "";
        string className = "";
        string selQuerry = "";
        if (sess != null)
        {
            try
            {
                selQuerry = "SELECT StudentId,StudentLname + ' ' + StudentFname AS Name FROM Student WHERE StudentId = " + sess.StudentId;
                DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
                selQuerry = "SELECT ClassId,ClassCd FROM Class WHERE ClassId = " + sess.Classid;
                DataTable dtList = objData.ReturnDataTable(selQuerry, false);
                if (dtNew != null)
                {
                    if (dtNew.Rows.Count > 0)
                    {
                        studentName = dtNew.Rows[0]["Name"].ToString();
                        if (dtList != null)
                        {
                            if (dtList.Rows.Count > 0)
                            {
                                className = dtList.Rows[0]["ClassCd"].ToString();
                            }
                        }
                    }
                }                

                lblStudentName.Text = studentName.ToString();
                lblClassName.Text = className.ToString();

            }
            catch (Exception Ex)
            {
            }
        }
    }
}