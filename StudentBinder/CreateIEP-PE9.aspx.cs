using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class StudentBinder_CreateIEP_PE9 : System.Web.UI.Page
{
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    public clsSession sess = null;
    static string args1 = "", args2 = "", args3 = "";
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

    [WebMethod]
    public static void submitIepPE9(string arg1, string arg2, string arg3)
    {
        args1 = arg1;
        args2 = arg2;
        args3 = arg3;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string TrainingGoal =System.Uri.UnescapeDataString(TextBoxDetailsA_hdn.Text);
        string TrainingCourse =System.Uri.UnescapeDataString(TextBoxDetailsB_hdn.Text);
        string EmploymentGoal = System.Uri.UnescapeDataString(TextBoxDetailsC_hdn.Text);

        bool chkbxAssment1 = false;
        bool chkbxAssment2 = false;
        bool chkbxAssment3 = false;
        bool chkbxAssment4 = false;

        List<string> selectedValues = CheckBoxListLocalAsssesment.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();
        foreach (var item in selectedValues)
        {
            if (item.ToString() == "A")
            {
                chkbxAssment1 = true;
            }
            if (item.ToString() == "B")
            {
                chkbxAssment2 = true;
            }
            if (item.ToString() == "C")
            {
                chkbxAssment3 = true;
            }
        }
        if (CheckBox1.Checked)
        {
            chkbxAssment4 = true;
        }

        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];


            sess = (clsSession)Session["UserSession"];
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId + " ").ToString();
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
            }
            else
            {
                strQuery = "UPDATE IEP_PE_Details SET IEP8AsmtStdtGrade='" + chkbxAssment1 + "',IEP8AsmtWithoutAcc='" + chkbxAssment2 + "',IEP8AsmtWithAcc='" + chkbxAssment3 + "',"
                    + "IEP8AssmtAcc='" + clsGeneral.convertQuotes(TrainingGoal) + "',IEP9AlernativeAsmt='" + chkbxAssment4 + "',IEP9NoRegAsmt='" + clsGeneral.convertQuotes(TrainingCourse) + "',IEP9AssmtAppropriate='" + clsGeneral.convertQuotes(EmploymentGoal) + "'"
                    + " WHERE StdtIEP_PEId=" + sess.IEPId;
                int id = oData.ExecuteNonQuery(strQuery);
                if (id > 0)
                {
                     tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                     setIEPPEupdateStatus();
                     //fillBasicDetails();
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        string TrainingGoal = System.Uri.UnescapeDataString(TextBoxDetailsA_hdn.Text);
        string TrainingCourse = System.Uri.UnescapeDataString(TextBoxDetailsB_hdn.Text);
        string EmploymentGoal = System.Uri.UnescapeDataString(TextBoxDetailsC_hdn.Text);
        bool chkbxAssment1 = false;
        bool chkbxAssment2 = false;
        bool chkbxAssment3 = false;
        bool chkbxAssment4 = false;

        List<string> selectedValues = CheckBoxListLocalAsssesment.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();
        foreach (var item in selectedValues)
        {
            if (item.ToString() == "A")
            {
                chkbxAssment1 = true;
            }
            if (item.ToString() == "B")
            {
                chkbxAssment2 = true;
            }
            if (item.ToString() == "C")
            {
                chkbxAssment3 = true;
            }
        }
        if (CheckBox1.Checked)
        {
            chkbxAssment4 = true;
        }

        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];


            sess = (clsSession)Session["UserSession"];
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId + " ").ToString();
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
            }
            else
            {
                strQuery = "UPDATE IEP_PE_Details SET IEP8AsmtStdtGrade='" + chkbxAssment1 + "',IEP8AsmtWithoutAcc='" + chkbxAssment2 + "',IEP8AsmtWithAcc='" + chkbxAssment3 + "',"
                    + "IEP8AssmtAcc='" + TrainingGoal + "',IEP9AlernativeAsmt='" + chkbxAssment4 + "',IEP9NoRegAsmt='" + TrainingCourse + "',IEP9AssmtAppropriate='" + EmploymentGoal + "'"
                    + " WHERE StdtIEP_PEId=" + sess.IEPId;
                int id = oData.ExecuteNonQuery(strQuery);
                if (id > 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    setIEPPEupdateStatus1();
                    //fillBasicDetails();
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page9='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page9) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(10);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page9='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page9) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP10('saved');", true);
    }

    private void fillBasicDetails()
    {

        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
         int alternateAssesment = 0;
        foreach (ListItem item in CheckBoxListLocalAsssesment.Items)
        {
            item.Selected = false;
        }
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

            strQuery = "SELECT IEP8AsmtStdtGrade,IEP8AsmtWithoutAcc,IEP8AssmtAcc,IEP8AsmtWithAcc,IEP9AlernativeAsmt,IEP9NoRegAsmt,IEP9AssmtAppropriate" +
                " from dbo.IEP_PE_Details where StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                     if(Dt.Rows[0]["IEP9AlernativeAsmt"].ToString() != "")
                     alternateAssesment = Convert.ToInt32(Dt.Rows[0]["IEP9AlernativeAsmt"]);
                     if (alternateAssesment == 1) CheckBox1.Checked = true; else CheckBox1.Checked = false;
                     foreach (DataRow dr in Dt.Rows)
                     {
                        TextBoxDetailsA.InnerHtml = Dt.Rows[0]["IEP8AssmtAcc"].ToString().Trim();
                        TextBoxDetailsA.InnerHtml = TextBoxDetailsA.InnerHtml.Replace("##", "'");
                        TextBoxDetailsA.InnerHtml = TextBoxDetailsA.InnerHtml.Replace("?bs;", "\\");

                        TextBoxDetailsA_hdn.Text = System.Uri.EscapeDataString(TextBoxDetailsA.InnerHtml);

                        TextBoxDetailsB.InnerHtml = Dt.Rows[0]["IEP9NoRegAsmt"].ToString().Trim();
                        TextBoxDetailsB.InnerHtml = TextBoxDetailsB.InnerHtml.Replace("##", "'");
                        TextBoxDetailsB.InnerHtml = TextBoxDetailsB.InnerHtml.Replace("?bs;", "\\");

                        TextBoxDetailsB_hdn.Text = System.Uri.EscapeDataString(TextBoxDetailsB.InnerHtml);

                        TextBoxDetailsC.InnerHtml = Dt.Rows[0]["IEP9AssmtAppropriate"].ToString().Trim();
                        TextBoxDetailsC.InnerHtml = TextBoxDetailsC.InnerHtml.Replace("##", "'");
                        TextBoxDetailsC.InnerHtml = TextBoxDetailsC.InnerHtml.Replace("?bs;", "\\");

                        TextBoxDetailsC_hdn.Text = System.Uri.EscapeDataString(TextBoxDetailsC.InnerHtml);

                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtStdtGrade"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtStdtGrade"].ToString())))
                        {
                            foreach (ListItem item in CheckBoxListLocalAsssesment.Items)
                            {
                                if (item.Value == "A")
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithoutAcc"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithoutAcc"].ToString())))
                        {
                            foreach (ListItem item in CheckBoxListLocalAsssesment.Items)
                            {
                                if (item.Value == "B")
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithAcc"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithAcc"].ToString())))
                        {
                            foreach (ListItem item in CheckBoxListLocalAsssesment.Items)
                            {
                                if (item.Value == "C")
                                {
                                    item.Selected = true;
                                }
                            }
                        }

                    }


                }
            }

        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
}

