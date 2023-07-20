using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE13 : System.Web.UI.Page
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

            strQuery = "SELECT IEP13RegularEdu,IEP13GeneralEdu FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    TextBoxExplanationregularClass.Text = Dt.Rows[0]["IEP13RegularEdu"].ToString().Trim();
                    TextBoxExplanationGeneralCurriculam.Text = Dt.Rows[0]["IEP13GeneralEdu"].ToString().Trim();
                   
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {        
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {


            strQuery = "UPDATE IEP_PE_Details SET IEP13RegularEdu='" + TextBoxExplanationregularClass.Text + "',IEP13GeneralEdu='" + TextBoxExplanationGeneralCurriculam.Text + "' WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();
                //fillBasicDetails();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {


            strQuery = "UPDATE IEP_PE_Details SET IEP13RegularEdu='" + TextBoxExplanationregularClass.Text + "',IEP13GeneralEdu='" + TextBoxExplanationGeneralCurriculam.Text + "' WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();
                //fillBasicDetails();
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
            objData.Execute("update StdtIEP_PEUpdateStatus set Page13='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page13) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(14);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page13='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page13) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP14('saved');", true);
    }
}