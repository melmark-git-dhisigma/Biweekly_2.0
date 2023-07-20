using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE15 : System.Web.UI.Page
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
        int IEP15RegularCls80 = 0;
        int IEP15Regular79 = 0;
        int IEP15Regular40 = 0;
        int IEP15ApprovePrivateSchool = 0;
        int IEP15OtherPublic = 0;
        int IEP15ApproveResidential = 0;
        int IEP15Hospital = 0;
        int IEP15PrivateFacility = 0;
        int IEP15CorrectionalFacility = 0;
        int IEP15PrivateResi = 0;
        int IEP15ChkoutState = 0;
        int IEP15PublicFacility = 0;
        int IEP15InstructionConducted = 0;
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

            strQuery = "SELECT IEP15RegularCls80,IEP15Regular79,IEP15Regular40,IEP15ApprovePrivateSchool,IEP15ApprovePrivateText,IEP15OtherPublic,IEP15OtherPublicText," +
                       "IEP15ApproveResidential,IEP15ApproveResidentialText, " +
                          "IEP15Hospital,IEP15HospitalText,IEP15PrivateFacility,IEP15PrivateFacilityText,IEP15CorrectionalFacility,IEP15CorrectionText," +
                           " IEP15PrivateResi,IEP15PrivateResText,IEP15ChkoutState,IEP15ChkoutStateText,IEP15PublicFacility,IEP15PublicFacilityText,IEP15InstructionConducted,IEP15InstructionText FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["IEP15RegularCls80"].ToString() != "")
                    IEP15RegularCls80 = Convert.ToInt32(Dt.Rows[0]["IEP15RegularCls80"]);
                    if (Dt.Rows[0]["IEP15Regular79"].ToString() != "")
                    IEP15Regular79 = Convert.ToInt32(Dt.Rows[0]["IEP15Regular79"]);
                    if (Dt.Rows[0]["IEP15Regular40"].ToString() != "")
                    IEP15Regular40 = Convert.ToInt32(Dt.Rows[0]["IEP15Regular40"]);
                    if (Dt.Rows[0]["IEP15ApprovePrivateSchool"].ToString() != "")
                    IEP15ApprovePrivateSchool = Convert.ToInt32(Dt.Rows[0]["IEP15ApprovePrivateSchool"]);
                    if (Dt.Rows[0]["IEP15OtherPublic"].ToString() != "")
                    IEP15OtherPublic = Convert.ToInt32(Dt.Rows[0]["IEP15OtherPublic"]);
                    if (Dt.Rows[0]["IEP15ApproveResidential"].ToString() != "")
                    IEP15ApproveResidential = Convert.ToInt32(Dt.Rows[0]["IEP15ApproveResidential"]);
                    if (Dt.Rows[0]["IEP15Hospital"].ToString() != "")
                    IEP15Hospital = Convert.ToInt32(Dt.Rows[0]["IEP15Hospital"]);
                    if (Dt.Rows[0]["IEP15PrivateFacility"].ToString() != "")
                    IEP15PrivateFacility = Convert.ToInt32(Dt.Rows[0]["IEP15PrivateFacility"]);
                    if (Dt.Rows[0]["IEP15CorrectionalFacility"].ToString() != "")
                    IEP15CorrectionalFacility = Convert.ToInt32(Dt.Rows[0]["IEP15CorrectionalFacility"]);
                    if (Dt.Rows[0]["IEP15PrivateResi"].ToString() != "")
                    IEP15PrivateResi = Convert.ToInt32(Dt.Rows[0]["IEP15PrivateResi"]);
                    if (Dt.Rows[0]["IEP15ChkoutState"].ToString() != "")
                    IEP15ChkoutState = Convert.ToInt32(Dt.Rows[0]["IEP15ChkoutState"]);
                    if (Dt.Rows[0]["IEP15PublicFacility"].ToString() != "")
                    IEP15PublicFacility = Convert.ToInt32(Dt.Rows[0]["IEP15PublicFacility"]);
                    if (Dt.Rows[0]["IEP15InstructionConducted"].ToString() != "")
                    IEP15InstructionConducted = Convert.ToInt32(Dt.Rows[0]["IEP15InstructionConducted"]);


                    txtApprovePrivate.Text = Dt.Rows[0]["IEP15ApprovePrivateText"].ToString().Trim();
                    txtotherpublic.Text = Dt.Rows[0]["IEP15OtherPublicText"].ToString().Trim();
                    txtapproveresidential.Text = Dt.Rows[0]["IEP15ApproveResidentialText"].ToString().Trim();
                    txthospital.Text = Dt.Rows[0]["IEP15HospitalText"].ToString().Trim();
                    txtprivatefacility.Text = Dt.Rows[0]["IEP15PrivateFacilityText"].ToString().Trim();
                    txtcorrectfacility.Text = Dt.Rows[0]["IEP15CorrectionText"].ToString().Trim();
                    txtprivateresidential.Text = Dt.Rows[0]["IEP15PrivateResText"].ToString().Trim();
                    txtoutofstate.Text = Dt.Rows[0]["IEP15ChkoutStateText"].ToString().Trim();
                    txtpublicfacility.Text = Dt.Rows[0]["IEP15PublicFacilityText"].ToString().Trim();
                    txtinstructionalconduct.Text = Dt.Rows[0]["IEP15InstructionText"].ToString().Trim();



                    if (IEP15RegularCls80 == 1) chkregularcls80.Checked = true; else chkregularcls80.Checked = false;
                    if (IEP15Regular79 == 1) chkregularcls79.Checked = true; else chkregularcls79.Checked = false;
                    if (IEP15Regular40 == 1) chkregularcls40.Checked = true; else chkregularcls40.Checked = false;
                    if (IEP15ApprovePrivateSchool == 1) chkApproveprivateschool.Checked = true; else chkApproveprivateschool.Checked = false;
                    if (IEP15OtherPublic == 1) chkotherpublic.Checked = true; else chkotherpublic.Checked = false;
                    if (IEP15ApproveResidential == 1) chkapproveresidential.Checked = true; else chkapproveresidential.Checked = false;
                    if (IEP15Hospital == 1) chkhospital.Checked = true; else chkhospital.Checked = false;
                    if (IEP15PrivateFacility == 1) chkprivatefacility.Checked = true; else chkprivatefacility.Checked = false;
                    if (IEP15CorrectionalFacility == 1) chkcorrectionalfacility.Checked = true; else chkcorrectionalfacility.Checked = false;
                    if (IEP15PrivateResi == 1) chkprivateresidential.Checked = true; else chkprivateresidential.Checked = false;
                    if (IEP15ChkoutState == 1) chkoutofstate.Checked = true; else chkoutofstate.Checked = false;
                    if (IEP15PublicFacility == 1) chkpublicfacility.Checked = true; else chkpublicfacility.Checked = false;
                    if (IEP15InstructionConducted == 1) chkInstructionconducted.Checked = true; else chkInstructionconducted.Checked = false;

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
        int IEP15RegularCls80 = 0;
        int IEP15Regular79 = 0;
        int IEP15Regular40 = 0;
        int IEP15ApprovePrivateSchool = 0;
        int IEP15OtherPublic = 0;
        int IEP15ApproveResidential = 0;
        int IEP15Hospital = 0;
        int IEP15PrivateFacility = 0;
        int IEP15CorrectionalFacility = 0;
        int IEP15PrivateResi = 0;
        int IEP15ChkoutState = 0;
        int IEP15PublicFacility = 0;
        int IEP15InstructionConducted = 0;
        sess = (clsSession)Session["UserSession"];
        oData = new DataClass();
        Dt = new DataTable();
        try
        {
            if (chkregularcls80.Checked == true) IEP15RegularCls80 = 1; else IEP15RegularCls80 = 0;
            if (chkregularcls79.Checked == true) IEP15Regular79 = 1; else IEP15Regular79 = 0;

            if (chkregularcls40.Checked == true) IEP15Regular40 = 1; else IEP15Regular40 = 0;
            if (chkApproveprivateschool.Checked == true) IEP15ApprovePrivateSchool = 1; else IEP15ApprovePrivateSchool = 0;
            if (chkotherpublic.Checked == true) IEP15OtherPublic = 1; else IEP15OtherPublic = 0;
            if (chkapproveresidential.Checked == true) IEP15ApproveResidential = 1; else IEP15ApproveResidential = 0;
            if (chkhospital.Checked == true) IEP15Hospital = 1; else IEP15Hospital = 0;
            if (chkprivatefacility.Checked == true) IEP15PrivateFacility = 1; else IEP15PrivateFacility = 0;
            if (chkcorrectionalfacility.Checked == true) IEP15CorrectionalFacility = 1; else IEP15CorrectionalFacility = 0;
            if (chkprivateresidential.Checked == true) IEP15PrivateResi = 1; else IEP15PrivateResi = 0;
            if (chkoutofstate.Checked == true) IEP15ChkoutState = 1; else IEP15ChkoutState = 0;
            if (chkpublicfacility.Checked == true) IEP15PublicFacility = 1; else IEP15PublicFacility = 0;
            if (chkInstructionconducted.Checked == true) IEP15InstructionConducted = 1; else IEP15InstructionConducted = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP15RegularCls80=" + IEP15RegularCls80 + ",IEP15Regular79=" + IEP15Regular79 + ",IEP15Regular40=" + IEP15Regular40 + ",IEP15ApprovePrivateSchool = " + IEP15ApprovePrivateSchool + "," +
                        "IEP15OtherPublic = " + IEP15OtherPublic + ",IEP15ApproveResidential = " + IEP15ApproveResidential + ",IEP15Hospital = " + IEP15Hospital + ",IEP15PrivateFacility = " + IEP15PrivateFacility + ",IEP15CorrectionalFacility = " + IEP15CorrectionalFacility + ",IEP15PrivateResi = " + IEP15PrivateResi + ",IEP15ChkoutState = " + IEP15ChkoutState + ",IEP15PublicFacility = " + IEP15PublicFacility + ",IEP15InstructionConducted = " + IEP15InstructionConducted + "," +
                        "IEP15ApprovePrivateText = '" + txtApprovePrivate.Text + "',IEP15OtherPublicText = '" + txtotherpublic.Text + "',IEP15ApproveResidentialText = '" + txtapproveresidential.Text + "',IEP15HospitalText = '" + txthospital.Text + "' ,IEP15PrivateFacilityText = '" + txtprivatefacility.Text + "'," +
                        "IEP15CorrectionText = '" + txtcorrectfacility.Text + "',IEP15PrivateResText = '" + txtprivateresidential.Text + "',IEP15ChkoutStateText = '" + txtoutofstate.Text + "',IEP15PublicFacilityText = '" + txtpublicfacility.Text + "',IEP15InstructionText = '" + txtinstructionalconduct.Text + "'" +
                        " WHERE StdtIEP_PEId=" + sess.IEPId;
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
        int IEP15RegularCls80 = 0;
        int IEP15Regular79 = 0;
        int IEP15Regular40 = 0;
        int IEP15ApprovePrivateSchool = 0;
        int IEP15OtherPublic = 0;
        int IEP15ApproveResidential = 0;
        int IEP15Hospital = 0;
        int IEP15PrivateFacility = 0;
        int IEP15CorrectionalFacility = 0;
        int IEP15PrivateResi = 0;
        int IEP15ChkoutState = 0;
        int IEP15PublicFacility = 0;
        int IEP15InstructionConducted = 0;
        sess = (clsSession)Session["UserSession"];
        oData = new DataClass();
        Dt = new DataTable();
        try
        {
            if (chkregularcls80.Checked == true) IEP15RegularCls80 = 1; else IEP15RegularCls80 = 0;
            if (chkregularcls79.Checked == true) IEP15Regular79 = 1; else IEP15Regular79 = 0;

            if (chkregularcls40.Checked == true) IEP15Regular40 = 1; else IEP15Regular40 = 0;
            if (chkApproveprivateschool.Checked == true) IEP15ApprovePrivateSchool = 1; else IEP15ApprovePrivateSchool = 0;
            if (chkotherpublic.Checked == true) IEP15OtherPublic = 1; else IEP15OtherPublic = 0;
            if (chkapproveresidential.Checked == true) IEP15ApproveResidential = 1; else IEP15ApproveResidential = 0;
            if (chkhospital.Checked == true) IEP15Hospital = 1; else IEP15Hospital = 0;
            if (chkprivatefacility.Checked == true) IEP15PrivateFacility = 1; else IEP15PrivateFacility = 0;
            if (chkcorrectionalfacility.Checked == true) IEP15CorrectionalFacility = 1; else IEP15CorrectionalFacility = 0;
            if (chkprivateresidential.Checked == true) IEP15PrivateResi = 1; else IEP15PrivateResi = 0;
            if (chkoutofstate.Checked == true) IEP15ChkoutState = 1; else IEP15ChkoutState = 0;
            if (chkpublicfacility.Checked == true) IEP15PublicFacility = 1; else IEP15PublicFacility = 0;
            if (chkInstructionconducted.Checked == true) IEP15InstructionConducted = 1; else IEP15InstructionConducted = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP15RegularCls80=" + IEP15RegularCls80 + ",IEP15Regular79=" + IEP15Regular79 + ",IEP15Regular40=" + IEP15Regular40 + ",IEP15ApprovePrivateSchool = " + IEP15ApprovePrivateSchool + "," +
                        "IEP15OtherPublic = " + IEP15OtherPublic + ",IEP15ApproveResidential = " + IEP15ApproveResidential + ",IEP15Hospital = " + IEP15Hospital + ",IEP15PrivateFacility = " + IEP15PrivateFacility + ",IEP15CorrectionalFacility = " + IEP15CorrectionalFacility + ",IEP15PrivateResi = " + IEP15PrivateResi + ",IEP15ChkoutState = " + IEP15ChkoutState + ",IEP15PublicFacility = " + IEP15PublicFacility + ",IEP15InstructionConducted = " + IEP15InstructionConducted + "," +
                        "IEP15ApprovePrivateText = '" + txtApprovePrivate.Text + "',IEP15OtherPublicText = '" + txtotherpublic.Text + "',IEP15ApproveResidentialText = '" + txtapproveresidential.Text + "',IEP15HospitalText = '" + txthospital.Text + "' ,IEP15PrivateFacilityText = '" + txtprivatefacility.Text + "'," +
                        "IEP15CorrectionText = '" + txtcorrectfacility.Text + "',IEP15PrivateResText = '" + txtprivateresidential.Text + "',IEP15ChkoutStateText = '" + txtoutofstate.Text + "',IEP15PublicFacilityText = '" + txtpublicfacility.Text + "',IEP15InstructionText = '" + txtinstructionalconduct.Text + "'" +
                        " WHERE StdtIEP_PEId=" + sess.IEPId;
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
            objData.Execute("update StdtIEP_PEUpdateStatus set Page15='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page15) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(15);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page15='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page15) values(" + sess.IEPId + ",'true')");
        }

       // ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP15('saved');", true);
    }
}