using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

public partial class StudentBinder_CreateIEP12 : System.Web.UI.Page
{
    public static clsData objData = null;
    public static clsSession sess = null;
    string strQuery = "";
    static int IEPId = 0;
    int retVal = 0;
    DataTable Dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }

        if (!IsPostBack)
        {
            //sess.IEPId = getIepIdFromStudentId();
            clsIEP IEPObj = new clsIEP();
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
            Fill();
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

    //[WebMethod]
    //public static string submitIEP12(string txtRejectedPortions, string txtParentComment)
    //{
    //    objData = new clsData();
    //    sess = (clsSession)HttpContext.Current.Session["UserSession"];

    //    string strQuery = "Update StdtIEPExt4 set ParntDontRejctDesc='" + clsGeneral.convertQuotes(txtRejectedPortions) + "',ParntComnt='" + clsGeneral.convertQuotes(txtParentComment) + "'  where StdtIEPId=" + IEPId + " ";
    //    string retVal = Convert.ToString(objData.Execute(strQuery));
    //    return retVal;
    //}
   
    public string submitIEP12(string txtRejectedPortions, string txtParentComment)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];

        string strQuery = "Update StdtIEPExt4 set ParntDontRejctDesc='" + clsGeneral.convertQuotes(txtRejectedPortions) + "',ParntComnt='" + clsGeneral.convertQuotes(txtParentComment) + "'  where StdtIEPId=" + IEPId + " ";
        string retVal = Convert.ToString(objData.Execute(strQuery));
        return retVal;
    }

    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            if (sess.IEPId <= 0) return;
            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }
            objData = new clsData();
            string strQuery = "SELECT   IEP4.StdtIEPId,  IEP4.SigRoleLEARep, IEP4.SigRep_date, IEP4.ParntAccptIEP, IEP4.ParntRejctIEP,IEP4.ParntDontRejctIEP, IEP4.ParntDontRejctDesc, IEP4.ParntReqMeetig, IEP4.SigParnt,IEP4.SigParnt_date,IEP4.ParntComnt FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + " ";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count > 0)
            {
                IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);

                txtSigRep.Text = Dt.Rows[0]["SigRoleLEARep"].ToString().Trim();
                if (Dt.Rows[0]["SigRep_date"].ToString().Trim() != null && Dt.Rows[0]["SigRep_date"].ToString().Trim() != "")

                    dteSigRep.Text = Convert.ToDateTime(Dt.Rows[0]["SigRep_date"]).ToString("MM/dd/yyyy").Replace('-', '/');
                else
                    dteSigRep.Text = "";
                acceptIepDeveloped.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntAccptIEP"].ToString());
                rejectIepDeveloped.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntRejctIEP"].ToString());
                deleteFollowingPortions.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntDontRejctIEP"].ToString());

                txtRejectedPortions.InnerHtml = Dt.Rows[0]["ParntDontRejctDesc"].ToString().Trim();
                txtRejectedPortions_hdn.Text = System.Uri.EscapeDataString(txtRejectedPortions.InnerHtml);

                RejectionMeeting.Checked = clsGeneral.getChecked(Dt.Rows[0]["ParntReqMeetig"].ToString());
                txtSigPrnt.Text = Dt.Rows[0]["SigParnt"].ToString().Trim();

                if (Dt.Rows[0]["SigParnt_date"].ToString().Trim() != null && Dt.Rows[0]["SigParnt_date"].ToString().Trim() != "")
                {
                    dteSigPrnt.Text = Convert.ToDateTime(Dt.Rows[0]["SigParnt_date"]).ToString("MM/dd/yyyy").Replace('-', '/');
                }
                else
                    dteSigPrnt.Text = "";

                txtParentComment.InnerHtml = Dt.Rows[0]["ParntComnt"].ToString().Trim();
                txtParentComment_hdn.Text = System.Uri.EscapeDataString(txtParentComment.InnerHtml);


                btnSave.Text = "Save and continue";
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            submitIEP12(System.Uri.UnescapeDataString(txtRejectedPortions_hdn.Text), System.Uri.UnescapeDataString(txtParentComment_hdn.Text));
            Update();
            Fill();
        }
        //if (btnSave.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}
        //Clear();
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            Update1();
            Fill();
        }
        //if (btnSave.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}
        //Clear();
    }
    private void Save()
    {
        try
        {
            if (Validation() == true)
            {
                sess = (clsSession)Session["UserSession"];
                objData = new clsData();
                IEPId = Convert.ToInt32(objData.FetchValue("Select [StdtIEPId] from [dbo].[StdtIEP] where SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + " and EffEndDate is Null"));
                if (IEPId > 0)
                {

                    strQuery = " Insert into StdtIEPExt3( StdtIEPId,SigRoleLEARep,SigRep_date,ParntAccptIEP,ParntRejctIEP,ParntDontRejctIEP,ParntDontRejctDesc,ParntReqMeetig,SigParnt,SigParnt_date,ParntComnt) Values  (" + IEPId + ",'" + dteSigRep.Text + "','" + dteSigRep.Text + "','" + dteSigRep.Text + "','" + acceptIepDeveloped.Checked + "','" + deleteFollowingPortions.Checked + "','" + clsGeneral.convertQuotes(txtRejectedPortions.InnerHtml) + "','" + RejectionMeeting.Checked + "','" + txtSigPrnt.Text + "''" + txtSigPrnt.Text + "','" + clsGeneral.convertQuotes(txtParentComment.InnerHtml) + "') ";
                    retVal = objData.Execute(strQuery);
                }

                else if (IEPId == 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please Save IEP Page 1 for " + sess.StudentName + "!");
                    return;
                }

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Saved Successfully");
                    //Clear();
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation failed...");
                }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                return;
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }
    private void Update()
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                 
                if (Validation() == true)
                {
                    ifExist();
                    IEPId = sess.IEPId;

                    strQuery = " Update StdtIEPExt4 set StdtIEPId=" + IEPId + ",SigRoleLEARep='" + clsGeneral.convertQuotes(txtSigRep.Text) + "',SigRep_date='" + clsGeneral.convertQuotes(dteSigRep.Text) + "',ParntAccptIEP='" + acceptIepDeveloped.Checked + "',ParntRejctIEP='" + rejectIepDeveloped.Checked + "',ParntDontRejctIEP='" + deleteFollowingPortions.Checked + "',ParntReqMeetig='" + RejectionMeeting.Checked + "',SigParnt='" + clsGeneral.convertQuotes(txtSigPrnt.Text) + "',SigParnt_date='" + clsGeneral.convertQuotes(dteSigPrnt.Text) + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + IEPId + " ";

                        retVal = objData.Execute(strQuery);
                    

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page12='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page12) values(" + sess.IEPId + ",'true')");
                        }
                        //Clear();

                        //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP12();", true);
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(12);", true);
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }
    private void Update1()
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {

                if (Validation() == true)
                {
                    ifExist();
                    IEPId = sess.IEPId;

                    strQuery = " Update StdtIEPExt4 set StdtIEPId=" + IEPId + ",SigRoleLEARep='" + clsGeneral.convertQuotes(txtSigRep.Text) + "',SigRep_date='" + clsGeneral.convertQuotes(dteSigRep.Text) + "',ParntAccptIEP='" + acceptIepDeveloped.Checked + "',ParntRejctIEP='" + rejectIepDeveloped.Checked + "',ParntDontRejctIEP='" + deleteFollowingPortions.Checked + "',ParntReqMeetig='" + RejectionMeeting.Checked + "',SigParnt='" + clsGeneral.convertQuotes(txtSigPrnt.Text) + "',SigParnt_date='" + clsGeneral.convertQuotes(dteSigPrnt.Text) + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);


                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page12='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page12) values(" + sess.IEPId + ",'true')");
                        }
                        //Clear();

                        //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP12();", true);

                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }
    private bool Validation()
    {
        //sess = (clsSession)Session["UserSession"];
        //if (sess.StudentId == 0)
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
        //    return false;
        //}
        //else if (txtAddInfoColDesc3.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Description");
        //    txtAddInfoColDesc3.Focus();
        //    return false;
        //}

        return true;
    }
    protected void ifExist()
    {

        DataClass oData = new DataClass();
        clsSession oSession = (clsSession)Session["UserSession"];
        string selIEPstatus = "SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'";
        int IEPstatus = oData.ExecuteScalar(selIEPstatus);
        string copyIEP = "SELECT MAX(StdtIEPId) AS StdtIEPId FROM StdtIEP WHERE StudentId=" + oSession.StudentId + " ";
        int newIEP = oData.ExecuteScalar(copyIEP);

        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt4 WHERE StdtIEPId=" + newIEP + " ") == false)
        {
            string insIEP4 = "INSERT INTO StdtIEPExt4(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + newIEP + "," + IEPstatus + "," + oSession.LoginId + "," +
           "(SELECT convert(varchar, getdate(), 100)))";
            int newVersion = oData.ExecuteNonQuery(insIEP4);
        }
        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt5 WHERE StdtIEPId=" + newIEP + " ") == false)
        {
            string insIEP5 = "INSERT INTO StdtIEPExt5(StdtIEPId) VALUES(" + newIEP + ")";
            oData.ExecuteNonQuery(insIEP5);
        }

    }


}