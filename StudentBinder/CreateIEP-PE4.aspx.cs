using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class StudentBinder_CreateIEP_PE4 : System.Web.UI.Page
{
    public clsData objData = null;
    DataClass oData = null;
    string strQuery = "";
    DataTable Dt = null;
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

            fillCheckBoxDetails();

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

    private void fillCheckBoxDetails()
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

            strQuery = "SELECT IEP4IsBlind,IEP4Isdeaf,IEP4CommNeeded,IEP4AssistiveTechNeeded,IEP4EnglishProficiency,IEP4ImpedeLearning FROM IEP_PE_Details"
                + " WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        if (dr["IEP4IsBlind"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4IsBlind"].ToString()))
                            {
                                CheckBoxBlindYes.Checked = true;
                                HiddenFieldBlind.Value = dr["IEP4IsBlind"].ToString();
                            }
                            else
                            {
                                CheckBoxBlindNo.Checked = true;
                                HiddenFieldBlind.Value = dr["IEP4IsBlind"].ToString();
                            }
                        }
                        else
                        {
                            CheckBoxBlindYes.Checked = false;
                            CheckBoxBlindNo.Checked = false;
                        }
                        if (dr["IEP4Isdeaf"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4Isdeaf"].ToString()))
                            {
                                CheckBoxDeafYes.Checked = true;
                                HiddenFieldDeaf.Value = dr["IEP4Isdeaf"].ToString();
                            }
                            else
                            {
                                CheckBoxDeafNo.Checked = true;
                                HiddenFieldDeaf.Value = dr["IEP4Isdeaf"].ToString();
                            }
                        }
                        else
                        {
                            CheckBoxDeafYes.Checked = false;
                            CheckBoxDeafNo.Checked = false;
                        }
                        if (dr["IEP4CommNeeded"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4CommNeeded"].ToString()))
                            {
                                CheckBoxCommunicationYes.Checked = true;
                                CheckBoxCommunicationYes.Value = dr["IEP4CommNeeded"].ToString();
                            }
                            else
                            {
                                CheckBoxCommunicationNo.Checked = true;
                                CheckBoxCommunicationYes.Value = dr["IEP4CommNeeded"].ToString();
                            }
                        }
                        else
                        {
                            CheckBoxCommunicationYes.Checked = false;
                            CheckBoxCommunicationNo.Checked = false;
                        }
                        if (dr["IEP4AssistiveTechNeeded"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4AssistiveTechNeeded"].ToString()))
                            {
                                CheckBoxAssistiveServicesYes.Checked = true;
                                HiddenFieldAssistiveServices.Value = dr["IEP4AssistiveTechNeeded"].ToString();
                            }
                            else
                            {
                                CheckBoxAssistiveServicesNo.Checked = true;
                                HiddenFieldAssistiveServices.Value = dr["IEP4AssistiveTechNeeded"].ToString();
                            }
                        }
                        else
                        {
                            CheckBoxAssistiveServicesYes.Checked = false;
                            CheckBoxAssistiveServicesNo.Checked = false;
                        }
                        if (dr["IEP4EnglishProficiency"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4EnglishProficiency"].ToString()))
                            {
                                CheckBoxEnglishproficiencyYes.Checked = true;
                                HiddenFieldEnglishproficiency.Value = dr["IEP4EnglishProficiency"].ToString();
                            }
                            else
                            {
                                CheckBoxEnglishproficiencyNo.Checked = true;
                                HiddenFieldEnglishproficiency.Value = dr["IEP4EnglishProficiency"].ToString();
                            }
                        }
                        else
                        {
                            CheckBoxEnglishproficiencyYes.Checked = false;
                            CheckBoxEnglishproficiencyNo.Checked = false;
                        }
                        if (dr["IEP4ImpedeLearning"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4ImpedeLearning"].ToString()))
                            {
                                CheckBoxBehaviorsYes.Checked = true;
                                HiddenFieldBehaviors.Value = dr["IEP4ImpedeLearning"].ToString();
                            }
                            else
                            {
                                CheckBoxBehaviorsNo.Checked = true;
                                HiddenFieldBehaviors.Value = dr["IEP4ImpedeLearning"].ToString();
                            }
                        }
                        else
                        {
                            CheckBoxBehaviorsYes.Checked = false;
                            CheckBoxBehaviorsNo.Checked = false;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        checkBoxValue();
        string blind = HiddenFieldBlind.Value;
        string deaf = HiddenFieldDeaf.Value;
        string communication = HiddenFieldCommunication.Value;
        string assistiveService = HiddenFieldAssistiveServices.Value;
        string EnglishProficiency = HiddenFieldEnglishproficiency.Value;
        string Behaviors = HiddenFieldBehaviors.Value;
        saveToDb();
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        checkBoxValue();
        string blind = HiddenFieldBlind.Value;
        string deaf = HiddenFieldDeaf.Value;
        string communication = HiddenFieldCommunication.Value;
        string assistiveService = HiddenFieldAssistiveServices.Value;
        string EnglishProficiency = HiddenFieldEnglishproficiency.Value;
        string Behaviors = HiddenFieldBehaviors.Value;
        saveToDb1();
    }

    private void saveToDb()
    {
        objData = new clsData();
        oData = new DataClass();
        sess = (clsSession)Session["UserSession"];
        try
        {
            strQuery = "UPDATE IEP_PE_Details SET IEP4IsBlind=" + HiddenFieldBlind.Value + ",IEP4Isdeaf=" + HiddenFieldDeaf.Value + ",IEP4CommNeeded=" + HiddenFieldCommunication.Value + ""
                + ",IEP4AssistiveTechNeeded=" + HiddenFieldAssistiveServices.Value + ",IEP4EnglishProficiency=" + HiddenFieldEnglishproficiency.Value + ","
                + "IEP4ImpedeLearning=" + HiddenFieldBehaviors.Value + " WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();
            }

            //fillCheckBoxDetails();
        }
        catch (Exception ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw (ex);
        }
    }
    private void saveToDb1()
    {
        objData = new clsData();
        oData = new DataClass();
        sess = (clsSession)Session["UserSession"];
        try
        {
            strQuery = "UPDATE IEP_PE_Details SET IEP4IsBlind=" + HiddenFieldBlind.Value + ",IEP4Isdeaf=" + HiddenFieldDeaf.Value + ",IEP4CommNeeded=" + HiddenFieldCommunication.Value + ""
                + ",IEP4AssistiveTechNeeded=" + HiddenFieldAssistiveServices.Value + ",IEP4EnglishProficiency=" + HiddenFieldEnglishproficiency.Value + ","
                + "IEP4ImpedeLearning=" + HiddenFieldBehaviors.Value + " WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();
            }

            //fillCheckBoxDetails();
        }
        catch (Exception ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw (ex);
        }
    }

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page4='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page4) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(5);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page4='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page4) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP5('saved');", true);
    }
    public void checkBoxValue()
    {
        if (CheckBoxBlindYes.Checked == true)
            HiddenFieldBlind.Value = "1";
        else if (CheckBoxBlindNo.Checked == true)
            HiddenFieldBlind.Value = "0";
        else
            HiddenFieldBlind.Value = "NULL";

        if (CheckBoxDeafYes.Checked == true)
            HiddenFieldDeaf.Value = "1";
        else if (CheckBoxDeafNo.Checked == true)
            HiddenFieldDeaf.Value = "0";
        else
            HiddenFieldDeaf.Value = "NULL";

        if (CheckBoxCommunicationYes.Checked == true)
            HiddenFieldCommunication.Value = "1";
        else if (CheckBoxCommunicationNo.Checked == true)
            HiddenFieldCommunication.Value = "0";
        else
            HiddenFieldCommunication.Value = "NULL";

        if (CheckBoxAssistiveServicesYes.Checked == true)
            HiddenFieldAssistiveServices.Value = "1";
        else if (CheckBoxAssistiveServicesNo.Checked == true)
            HiddenFieldAssistiveServices.Value = "0";
        else
            HiddenFieldAssistiveServices.Value = "NULL";

        if (CheckBoxEnglishproficiencyYes.Checked == true)
            HiddenFieldEnglishproficiency.Value = "1";
        else if (CheckBoxEnglishproficiencyNo.Checked == true)
            HiddenFieldEnglishproficiency.Value = "0";
        else
            HiddenFieldEnglishproficiency.Value = "NULL";

        if (CheckBoxBehaviorsYes.Checked == true)
            HiddenFieldBehaviors.Value = "1";
        else if (CheckBoxBehaviorsNo.Checked == true)
            HiddenFieldBehaviors.Value = "0";
        else
            HiddenFieldBehaviors.Value = "NULL";
    }
}