using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE7 : System.Web.UI.Page
{
    clsSession sess = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    public clsData objData = null;

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


            FillData();
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
        oData = new DataClass();
        Dt = new DataTable();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {
            int chk1 = 0; int chk2 = 0; int chk3 = 0; int chk4 = 0; int chk5 = 0; int chk6 = 0; int chk7 = 0; int chk8 = 0; int chk9 = 0;
            if (IEP8AsmtNotAdministred.Checked == true) chk1 = 1; else chk1 = 0;
            if (IEP8ReadPartcptPSSAWithoutAcmdtn.Checked == true) chk2 = 1; else chk2 = 0;
            if (IEP8ReadPartcptPSSAWithFollowingAcmdtn.Checked == true) chk3 = 1; else chk3 = 0;
            if (IEP8ReadPartcptPSSAModiWithoutAcmdtn.Checked == true) chk4 = 1; else chk4 = 0;
            if (IEP8ReadPartcptPSSAModiWithFollowingAcmdtn.Checked == true) chk5 = 1; else chk5 = 0;
            if (IEP8MathPartcptPSSAWithoutAcmdtn.Checked == true) chk6 = 1; else chk6 = 0;
            if (IEP8MathPartcptPSSAWithFollowingAcmdtn.Checked == true) chk7 = 1; else chk7 = 0;
            if (IEP8MathPartcptPSSAModiWithoutAcmdtn.Checked == true) chk8 = 1; else chk8 = 0;
            if (IEP8MathPartcptPSSAModiWithFollowingAcmdtn.Checked == true) chk9 = 1; else chk9 = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP8AsmtNotAdministred=" + chk1 + ",IEP8ReadPartcptPSSAWithoutAcmdtn=" + chk2 + ",IEP8ReadPartcptPSSAWithFollowingAcmdtn=" + chk3 + ",IEP8ReadPartcptPSSAModiWithoutAcmdtn = " + chk4 + ",IEP8ReadPartcptPSSAModiWithFollowingAcmdtn = " + chk5 + ",IEP8MathPartcptPSSAWithoutAcmdtn = " + chk6 + ",IEP8MathPartcptPSSAWithFollowingAcmdtn = " + chk7 + ",IEP8MathPartcptPSSAModiWithoutAcmdtn = " + chk8 + ",IEP8MathPartcptPSSAModiWithFollowingAcmdtn = " + chk9 + " WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();
                //FillData();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
        //tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        oData = new DataClass();
        Dt = new DataTable();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {
            int chk1 = 0; int chk2 = 0; int chk3 = 0; int chk4 = 0; int chk5 = 0; int chk6 = 0; int chk7 = 0; int chk8 = 0; int chk9 = 0;
            if (IEP8AsmtNotAdministred.Checked == true) chk1 = 1; else chk1 = 0;
            if (IEP8ReadPartcptPSSAWithoutAcmdtn.Checked == true) chk2 = 1; else chk2 = 0;
            if (IEP8ReadPartcptPSSAWithFollowingAcmdtn.Checked == true) chk3 = 1; else chk3 = 0;
            if (IEP8ReadPartcptPSSAModiWithoutAcmdtn.Checked == true) chk4 = 1; else chk4 = 0;
            if (IEP8ReadPartcptPSSAModiWithFollowingAcmdtn.Checked == true) chk5 = 1; else chk5 = 0;
            if (IEP8MathPartcptPSSAWithoutAcmdtn.Checked == true) chk6 = 1; else chk6 = 0;
            if (IEP8MathPartcptPSSAWithFollowingAcmdtn.Checked == true) chk7 = 1; else chk7 = 0;
            if (IEP8MathPartcptPSSAModiWithoutAcmdtn.Checked == true) chk8 = 1; else chk8 = 0;
            if (IEP8MathPartcptPSSAModiWithFollowingAcmdtn.Checked == true) chk9 = 1; else chk9 = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP8AsmtNotAdministred=" + chk1 + ",IEP8ReadPartcptPSSAWithoutAcmdtn=" + chk2 + ",IEP8ReadPartcptPSSAWithFollowingAcmdtn=" + chk3 + ",IEP8ReadPartcptPSSAModiWithoutAcmdtn = " + chk4 + ",IEP8ReadPartcptPSSAModiWithFollowingAcmdtn = " + chk5 + ",IEP8MathPartcptPSSAWithoutAcmdtn = " + chk6 + ",IEP8MathPartcptPSSAWithFollowingAcmdtn = " + chk7 + ",IEP8MathPartcptPSSAModiWithoutAcmdtn = " + chk8 + ",IEP8MathPartcptPSSAModiWithFollowingAcmdtn = " + chk9 + " WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();
                //FillData();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
        //tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
    }

    private void setIEPPEupdateStatus()
    {

        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page7='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page7) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(8);", true);
    }
    private void setIEPPEupdateStatus1()
    {

        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page7='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page7) values(" + sess.IEPId + ",'true')");
        }

       // ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP8('saved');", true);
    }

    protected void FillData()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string strQuery1 = "";
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();

        try
        {
            strQuery1 = "select ST.StudentLname+','+ST.StudentFname as StudentName from Student ST  where StudentId=" + sess.StudentId + ""
                        + "and SchoolId=" + sess.SchoolId;

            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblStudentName.InnerText = Dt.Rows[0]["StudentName"].ToString().Trim();
                }
            }

            string strQuery2 = "SELECT IEP8AsmtNotAdministred,IEP8ReadPartcptPSSAWithoutAcmdtn,IEP8ReadPartcptPSSAWithFollowingAcmdtn,IEP8ReadPartcptPSSAModiWithoutAcmdtn,IEP8ReadPartcptPSSAModiWithFollowingAcmdtn,"
                               + "IEP8MathPartcptPSSAWithoutAcmdtn,IEP8MathPartcptPSSAWithFollowingAcmdtn,IEP8MathPartcptPSSAModiWithoutAcmdtn,IEP8MathPartcptPSSAModiWithFollowingAcmdtn FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;

            Dt1 = objData.ReturnDataTable(strQuery2, false);
            if (Dt1 != null)
            {
                if (Dt1.Rows.Count > 0)
                {
                    if (Dt1.Rows[0]["IEP8AsmtNotAdministred"].ToString() != "")
                        IEP8AsmtNotAdministred.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8AsmtNotAdministred"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithoutAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAModiWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithFollowingAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAModiWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithFollowingAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithoutAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAModiWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithFollowingAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAModiWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithFollowingAcmdtn"]);


                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
}