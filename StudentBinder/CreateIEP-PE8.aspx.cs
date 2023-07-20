using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE8 : System.Web.UI.Page
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
    protected void btnSave_Click(object sender, EventArgs e)
    {

        int videotape = 0;
        int writtenPattern = 0;
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            int chk1 = 0; int chk2 = 0; int chk3 = 0; int chk4 = 0; int chk5 = 0; int chk6 = 0; int chk7 = 0;
            if (IEP8SciencePartcptPSSAWithoutAcmdtn.Checked == true) chk1 = 1; else chk1 = 0;
            if (IEP8SciencePartcptPSSAWithFollowingAcmdtn.Checked == true) chk2 = 1; else chk2 = 0;
            if (IEP8SciencePartcptPSSAModiWithoutAcmdtn.Checked == true) chk3 = 1; else chk3 = 0;
            if (IEP8SciencePartcptPSSAModiWithFollowingAcmdtn.Checked == true) chk4 = 1; else chk4 = 0;
            if (IEP8WritePartcptPSSAWithoutAcmdtn.Checked == true) chk5 = 1; else chk5 = 0;
            if (IEP8WritePartcptPSSAWithFollowingAcmdtn.Checked == true) chk6 = 1; else chk6 = 0;
            if (IEP8PSSAParticipate.Checked == true) chk7 = 1; else chk7 = 0;
            
            if (chkVideoTape.Checked == true) videotape = 1; else videotape = 0;
            if (chkwrittenNative.Checked == true) writtenPattern = 1; else writtenPattern = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP8PSSAReading='" + TextBox1.Text + "',IEP8PSSAappropriate='" + TextBox2.Text + "',IEP8Videotape=" + videotape + ",IEP8WrittenNarrative = " + writtenPattern + ","
            + "IEP8SciencePartcptPSSAWithoutAcmdtn=" + chk1 + ",IEP8SciencePartcptPSSAWithFollowingAcmdtn=" + chk2 + ","
            + "IEP8SciencePartcptPSSAModiWithoutAcmdtn=" + chk3 + ",IEP8SciencePartcptPSSAModiWithFollowingAcmdtn=" + chk4 + ","
            + "IEP8WritePartcptPSSAWithoutAcmdtn=" + chk5 + ",IEP8WritePartcptPSSAWithFollowingAcmdtn=" + chk6 + ","
            + "IEP8PSSAParticipate=" + chk7 + " WHERE StdtIEP_PEId=" + sess.IEPId;
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

        int videotape = 0;
        int writtenPattern = 0;
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            int chk1 = 0; int chk2 = 0; int chk3 = 0; int chk4 = 0; int chk5 = 0; int chk6 = 0; int chk7 = 0;
            if (IEP8SciencePartcptPSSAWithoutAcmdtn.Checked == true) chk1 = 1; else chk1 = 0;
            if (IEP8SciencePartcptPSSAWithFollowingAcmdtn.Checked == true) chk2 = 1; else chk2 = 0;
            if (IEP8SciencePartcptPSSAModiWithoutAcmdtn.Checked == true) chk3 = 1; else chk3 = 0;
            if (IEP8SciencePartcptPSSAModiWithFollowingAcmdtn.Checked == true) chk4 = 1; else chk4 = 0;
            if (IEP8WritePartcptPSSAWithoutAcmdtn.Checked == true) chk5 = 1; else chk5 = 0;
            if (IEP8WritePartcptPSSAWithFollowingAcmdtn.Checked == true) chk6 = 1; else chk6 = 0;
            if (IEP8PSSAParticipate.Checked == true) chk7 = 1; else chk7 = 0;

            if (chkVideoTape.Checked == true) videotape = 1; else videotape = 0;
            if (chkwrittenNative.Checked == true) writtenPattern = 1; else writtenPattern = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP8PSSAReading='" + TextBox1.Text + "',IEP8PSSAappropriate='" + TextBox2.Text + "',IEP8Videotape=" + videotape + ",IEP8WrittenNarrative = " + writtenPattern + ","
            + "IEP8SciencePartcptPSSAWithoutAcmdtn=" + chk1 + ",IEP8SciencePartcptPSSAWithFollowingAcmdtn=" + chk2 + ","
            + "IEP8SciencePartcptPSSAModiWithoutAcmdtn=" + chk3 + ",IEP8SciencePartcptPSSAModiWithFollowingAcmdtn=" + chk4 + ","
            + "IEP8WritePartcptPSSAWithoutAcmdtn=" + chk5 + ",IEP8WritePartcptPSSAWithFollowingAcmdtn=" + chk6 + ","
            + "IEP8PSSAParticipate=" + chk7 + " WHERE StdtIEP_PEId=" + sess.IEPId;
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
            objData.Execute("update StdtIEP_PEUpdateStatus set Page8='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page8) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(9);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page8='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page8) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP9('saved');", true);
    }

    private void fillBasicDetails()
    {
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        int videoTape = 0;
        int writtenParam = 0;
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

            strQuery = "SELECT IEP8PSSAReading,IEP8PSSAappropriate,IEP8Videotape,IEP8WrittenNarrative,IEP8SciencePartcptPSSAWithoutAcmdtn,IEP8SciencePartcptPSSAWithFollowingAcmdtn,"
            + "IEP8SciencePartcptPSSAModiWithoutAcmdtn,IEP8SciencePartcptPSSAModiWithFollowingAcmdtn,IEP8WritePartcptPSSAWithoutAcmdtn,IEP8WritePartcptPSSAWithFollowingAcmdtn,IEP8PSSAParticipate"
            +" FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    TextBox1.Text = Dt.Rows[0]["IEP8PSSAReading"].ToString().Trim();
                    TextBox2.Text = Dt.Rows[0]["IEP8PSSAappropriate"].ToString().Trim();
                    if(Dt.Rows[0]["IEP8Videotape"].ToString() != "")
                    videoTape = Convert.ToInt32(Dt.Rows[0]["IEP8Videotape"]);
                    if (Dt.Rows[0]["IEP8WrittenNarrative"].ToString() != "")
                    writtenParam = Convert.ToInt32(Dt.Rows[0]["IEP8WrittenNarrative"]);

                    if (videoTape == 1) chkVideoTape.Checked = true; else chkVideoTape.Checked = false;
                    if (writtenParam == 1) chkwrittenNative.Checked = true; else chkwrittenNative.Checked = false;

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAWithoutAcmdtn"]);

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithoutAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAModiWithoutAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithoutAcmdtn"]);

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithFollowingAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAModiWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithFollowingAcmdtn"]);

                    if (Dt.Rows[0]["IEP8WritePartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8WritePartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8WritePartcptPSSAWithoutAcmdtn"]);

                    if (Dt.Rows[0]["IEP8WritePartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8WritePartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8WritePartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt.Rows[0]["IEP8PSSAParticipate"].ToString() != "")
                        IEP8PSSAParticipate.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8PSSAParticipate"]);
                }
            }
        }
       catch (Exception Ex)
        {
            throw (Ex);
        }
    }

}