using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class StudentBinder_CreateIEP_PE5 : System.Web.UI.Page
{
    DataTable DTRoles = null;
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    static string x = "", y = "";
    public clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (!IsPostBack)
        {
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

            ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

            
            fillBasicDetails();
            
        }
      
    }
    private void ViewAccReject()
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        int reject = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Rejected'");
        int approve = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        if (sess.IEPStatus == approve || sess.IEPStatus == reject)
        {
            btnSave.Visible = false;
        }

    }

    private void fillBasicDetails()
    {
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            //display student name
            DataTable dataStud = new DataTable();
            string studentName = "select ST.StudentLname+','+ST.StudentFname as StudentName from Student ST  where StudentId=" + sess.StudentId + ""
                        + "and SchoolId=" + sess.SchoolId;
            dataStud = objData.ReturnDataTable(studentName, false);
            if (dataStud != null)
            {
                if (dataStud.Rows.Count > 0)
                {
                    lblStudentName.Text = dataStud.Rows[0]["StudentName"].ToString().Trim();
                }
            }

            strQuery = "SELECT IEP5OtherSpecify,IEP5CIPCode FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        TextBoxCIPCode.InnerHtml = Dt.Rows[0]["IEP5CIPCode"].ToString().Trim();
                        TextBoxCIPCode.InnerHtml = TextBoxCIPCode.InnerHtml.Replace("##", "'");
                        TextBoxCIPCode.InnerHtml = TextBoxCIPCode.InnerHtml.Replace("?bs;", "\\");

                        TextBoxCIPCode_hdn.Text = System.Uri.EscapeDataString(TextBoxCIPCode.InnerHtml);

                        TextBoxOtherDetails.InnerHtml = Dt.Rows[0]["IEP5OtherSpecify"].ToString().Trim();
                        TextBoxOtherDetails.InnerHtml = TextBoxOtherDetails.InnerHtml.Replace("##", "'");
                        TextBoxOtherDetails.InnerHtml = TextBoxOtherDetails.InnerHtml.Replace("?bs;", "\\");

                        TextBoxOtherDetails_hdn.Text = System.Uri.EscapeDataString(TextBoxOtherDetails.InnerHtml);
                    }


                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    [WebMethod]
    public static void submitIepPE5(string arg1, string arg2)
    {
        x = arg1;
        y = arg2;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string other = System.Uri.UnescapeDataString(TextBoxOtherDetails_hdn.Text);
        string CIPcode = System.Uri.UnescapeDataString(TextBoxCIPCode_hdn.Text);
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            strQuery = "UPDATE IEP_PE_Details SET IEP5OtherSpecify='" + clsGeneral.convertQuotes(other) + "',IEP5CIPCode='" + clsGeneral.convertQuotes(CIPcode) + "' WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();
               // fillBasicDetails();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        string other = System.Uri.UnescapeDataString(TextBoxOtherDetails_hdn.Text);
        string CIPcode = System.Uri.UnescapeDataString(TextBoxCIPCode_hdn.Text);
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            strQuery = "UPDATE IEP_PE_Details SET IEP5OtherSpecify='" + other + "',IEP5CIPCode='" + CIPcode + "' WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();
                // fillBasicDetails();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(6);", true);
    }


    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
        }

       // ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP6('saved');", true);
    }
}