using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP6 : System.Web.UI.Page
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
            clsIEP IEPObj = new clsIEP();
            //sess.IEPId = getIepIdFromStudentId();
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
            if (Status.Trim() == "Approved" ||Status.Trim()== "Expired")
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }
        }
    }



    public int getIepIdFromStudentId()
    {

        object pendstatus = null;
        object IepStatus = null;
        object IepId = null;
        int IEP_id = 0;
        clsData oData = new clsData();
        sess = (clsSession)Session["UserSession"];
        sess.IEPId = 0;
        if (oData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
        {
            pendstatus = oData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                IepStatus = oData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
            if ((IepStatus.ToString() == "Approved") || (IepStatus.ToString() == "Expired"))
            {
                IEP_id = 0;
            }

            else
            {
                IepId = oData.FetchValue("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ");
                IEP_id = int.Parse(IepId.ToString());
            }


        }
        return IEP_id;
    }
    //[WebMethod]
    //public static string submitIEP6(string txtNonParticipation, string txtSceduleModification)
    //{
    //    objData = new clsData();
    //    sess = (clsSession)HttpContext.Current.Session["UserSession"];

    //    string strQuery = "Update StdtIEPExt3 Set RemovedDesc='" + clsGeneral.convertQuotes(txtNonParticipation) + "',SchedModDesc='" + clsGeneral.convertQuotes(txtSceduleModification) + "' where StdtIEPId=" + IEPId + " ";
    //    string retVal = Convert.ToString(objData.Execute(strQuery));
    //    return retVal;
    //}

    public string submitIEP6(string txtNonParticipation, string txtSceduleModification)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];

        string strQuery = "Update StdtIEPExt3 Set RemovedDesc='" + clsGeneral.convertQuotes(txtNonParticipation) + "',SchedModDesc='" + clsGeneral.convertQuotes(txtSceduleModification) + "' where StdtIEPId=" + IEPId + " ";
        string retVal = Convert.ToString(objData.Execute(strQuery));
        return retVal;
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
    //private void fillSudentsDetails(int studentId)
    //{
    //    objData = new clsData();
    //    string query = "select * from Student where StudentId=" + studentId;
    //    Dt = objData.ReturnDataTable(query, false);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        //lbliepDateFrom.Text=Dt.Rows[0]["
    //        lblDob.Text = Dt.Rows[0]["DOB"].ToString();
    //        lblGrade.Text = Dt.Rows[0]["GradeLevel"].ToString();
    //        lblStudentName.Text = Dt.Rows[0]["StudentLname"].ToString() + ", " + Dt.Rows[0]["StudentFname"].ToString();
    //        lblId.Text = Dt.Rows[0]["StudentNbr"].ToString();


    //    }
    //}
   
    
    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill();
    }

   
    private void SaveAndUpdate()
    {
        bool[] radioButtonlists = new bool[10]; // RemovedInd1,RemovedInd2,ShorterCd1,ShorterCd2,ShorterCd3,LongerCd1,LongerCd2,LongerCd3,,TransportInd1,TransportInd2

        for (int i = 0; i < radioButtonlists.Length; i++)
        {
            radioButtonlists[i] = false;
        }

       // Response.Write(rblNonParticipationNoYes.SelectedIndex.ToString());

        switch (rblNonParticipationNoYes.SelectedIndex)
        {
            case 0:
                radioButtonlists[0] = true;
                break;
            case 1:
                radioButtonlists[1] = true;
                break;
            default:
                break;
        }
        switch (rblScheduleModificationShort.SelectedIndex)
        {
            case 0:
                radioButtonlists[2] = true;
                break;
            case 1:
                radioButtonlists[3] = true;
                break;
            case 2:
                radioButtonlists[4] = true;
                break;
            default:
                break;
        }
        switch (rblScheduleModificationLong.SelectedIndex)
        {
            case 0:
                radioButtonlists[5] = true;
                break;
            case 1:
                radioButtonlists[6] = true;
                break;
            case 2:
                radioButtonlists[7] = true;
                break;
            default:
                break;
        }
        if(rbtnTransportno.Checked==true)
            radioButtonlists[8] = true;
        else if (rbtnTransportyes.Checked == true)
            radioButtonlists[9] = true;
        try
        {
            if (Validation() == true)
            {
                sess = (clsSession)Session["UserSession"];
                objData = new clsData();
               // IEPId = Convert.ToInt32(objData.FetchValue("Select [StdtIEPId] from [dbo].[StdtIEP] where SchoolId=" + sess.SchoolId + " and StudentId=" + ddlStudent.SelectedValue + ""));
               // IEPId = Convert.ToInt32(Session["IEPID"].ToString());




                IEPId = sess.IEPId;
                if (IEPId > 0)
                {
                    strQuery = "Update StdtIEPExt3 Set StdtIEPId=" + IEPId + ",RemovedInd1='" + radioButtonlists[0] + "',RemovedInd2='" + radioButtonlists[1] + "',"
                        +"ShorterCd1='" + radioButtonlists[2] + "',ShorterCd2='" + radioButtonlists[3] + "',ShorterCd3='" + radioButtonlists[4] + "',"
                        +"LongerCd1='" + radioButtonlists[5] + "',LongerCd2='" + radioButtonlists[6] + "',LongerCd3='" + radioButtonlists[7] + "',"
                        +"TransportInd1='" + radioButtonlists[8] + "',TransportInd2='" + radioButtonlists[9] + "',RegTransInd='" + chkTransportServicesRegular.Checked + "'"
                        + ",RegTransDesc='" + clsGeneral.convertQuotes(txtTransportServicesRegular.Text) + "',SpTransInd='" + chkTransportServicesSpecial.Checked + "'"
                        + ",SpTransDesc='" + clsGeneral.convertQuotes(txtTransportServicesSpecial.Text) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=getDate()"
                        +" where StdtIEPId=" + IEPId + " ";
                    retVal = objData.Execute(strQuery);
                }

                else if (IEPId == 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Saved Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page6='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page6) values(" + sess.IEPId + ",'true')");
                    }
                    return;
                }

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page6='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page6) values(" + sess.IEPId + ",'true')");
                    }
                    
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
    private bool Validation()
    {
        //if (ddlStudent.SelectedIndex == 0)
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
        //    ddlStudent.Focus();
        //    return false;
        //}
        //else 
        //if (txtNonParticipation.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Non Participation details");
        //    txtNonParticipation.Focus();
        //    return false;
        //}
        //else if (txtSceduleModification.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Shedule Modification details");
        //    txtSceduleModification.Focus();
        //    return false;
        //}
        
        return true;
    }
   
    private void Clear()
    {
        IEPId = 0;
        txtNonParticipation.InnerHtml = "";
        txtSceduleModification.InnerHtml = "";
        txtTransportServicesRegular.Text = "";
        txtTransportServicesSpecial.Text = "";
        chkTransportServicesRegular.Checked = false;
        chkTransportServicesSpecial.Checked = false;
        rblNonParticipationNoYes.SelectedIndex = 0;
        rblScheduleModificationLong.SelectedIndex = 0;
        rblScheduleModificationShort.SelectedIndex = 0;
    }
    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        if (sess != null)
        {
            string strQuery = "Select IEP3.StdtIEPId,IEP3.RemovedInd1,IEP3.RemovedInd2,IEP3.RemovedDesc,IEP3.ShorterCd1,IEP3.ShorterCd2,IEP3.ShorterCd3,IEP3.LongerCd1,IEP3.LongerCd2,IEP3.LongerCd3,IEP3.SchedModDesc,IEP3.TransportInd1,IEP3.TransportInd2,IEP3.RegTransInd,IEP3.RegTransDesc,IEP3.SpTransInd ,IEP3.SpTransDesc from StdtIEPExt3 IEP3  INNER JOIN StdtIEP IEP  ON IEP.StdtIEPId  =IEP3.StdtIEPId where IEP.StdtIEPId=" + sess.IEPId + " ";
            objData = new clsData();
            Dt = objData.ReturnDataTable(strQuery, false);
            //Clear();
            if (Dt.Rows.Count > 0)
            {
               // IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]); 
                IEPId = sess.IEPId;

                txtNonParticipation.InnerHtml = Dt.Rows[0]["RemovedDesc"].ToString().Trim();
                txtSceduleModification.InnerHtml = Dt.Rows[0]["SchedModDesc"].ToString().Trim();

                txtNonParticipation_hdn.Text = System.Uri.EscapeDataString(txtNonParticipation.InnerHtml);
                txtSceduleModification_hdn.Text = System.Uri.EscapeDataString(txtSceduleModification.InnerHtml);


                int nonParticipationYesNo = (Dt.Rows[0]["RemovedInd2"].ToString() == "True") ? 1 : 0;
                int transport = (Dt.Rows[0]["TransportInd2"].ToString() == "True") ? 1 :0;
                int modificationShort = 0;
                int modificationLong = 0;

                if (Dt.Rows[0]["ShorterCd2"].ToString() == "True")
                {
                    modificationShort = 1;
                }
                if (Dt.Rows[0]["ShorterCd3"].ToString() == "True")
                {
                    modificationShort = 2;
                }

                if (Dt.Rows[0]["LongerCd2"].ToString() == "True")
                {
                    modificationLong = 1;
                }

                if (Dt.Rows[0]["LongerCd3"].ToString() == "True")
                {
                    modificationLong = 2;
                }


                rblNonParticipationNoYes.SelectedIndex = nonParticipationYesNo;
                rblScheduleModificationShort.SelectedIndex = modificationShort;
                rblScheduleModificationLong.SelectedIndex = modificationLong;
                if (transport == 1)
                {
                    rbtnTransportyes.Checked = true;
                    chkTransportServicesRegular.Checked = clsGeneral.getChecked(Dt.Rows[0]["RegTransInd"].ToString());
                    chkTransportServicesSpecial.Checked = clsGeneral.getChecked(Dt.Rows[0]["SpTransInd"].ToString());
                    txtTransportServicesRegular.Text = Dt.Rows[0]["RegTransDesc"].ToString().Trim();
                    txtTransportServicesSpecial.Text = Dt.Rows[0]["SpTransDesc"].ToString().Trim();
                }
                else
                    rbtnTransportno.Checked = true;

                btnSave.Text = "Save and continue";
            }

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            if (sess.IEPId <= 0) return;
            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }
            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                objData = new clsData();
                DataClass oData = new DataClass();
                string pendstatus = "";
                int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
                pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
                if (Convert.ToString(pendingApprove) == pendstatus)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
                }
                else
                {
                    tdMsg.InnerHtml = "";
                    submitIEP6(System.Uri.UnescapeDataString(txtNonParticipation_hdn.Text), System.Uri.UnescapeDataString(txtSceduleModification_hdn.Text));
                    SaveAndUpdate();
                  //  Fill();
                    ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(7);", true);
                }
            }
        
    }

    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

        if (StatusName == "Approved")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
            return;
        }
        else
        {
            objData = new clsData();
            DataClass oData = new DataClass();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
            }
            else
            {
                tdMsg.InnerHtml = "";
                SaveAndUpdate();
                //  Fill();
                //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP7();", true);
            }
        }

    }
    protected void rbtnTransportno_CheckedChanged(object sender, EventArgs e)
    {
        chkTransportServicesRegular.Checked = false;
        chkTransportServicesSpecial.Checked = false;
        txtTransportServicesRegular.Text = "";
        txtTransportServicesSpecial.Text = "";
        txtTransportServicesRegular.ReadOnly = true;
        txtTransportServicesSpecial.ReadOnly = true;
        chkTransportServicesRegular.Enabled = false;
        chkTransportServicesSpecial.Enabled = false;
    }
    protected void rbtnTransportyes_CheckedChanged(object sender, EventArgs e)
    {
        txtTransportServicesRegular.ReadOnly = false;
        txtTransportServicesSpecial.ReadOnly = false;
        chkTransportServicesRegular.Enabled = true;
        chkTransportServicesSpecial.Enabled = true;
    }
}