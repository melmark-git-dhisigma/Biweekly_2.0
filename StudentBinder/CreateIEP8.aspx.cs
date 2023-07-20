using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP8 : System.Web.UI.Page
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
            submitIEP8(System.Uri.UnescapeDataString(txtAddInfoColDesc3_hdn.Text));
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
    //[WebMethod]
    //public static string submitIEP8(string txtAddInfoColDesc3)
    //{
    //    objData = new clsData();
    //    sess = (clsSession)HttpContext.Current.Session["UserSession"];

    //    string strQuery = "Update StdtIEPExt3 set AddInfoCol3Desc='" + clsGeneral.convertQuotes(txtAddInfoColDesc3) + "'  where StdtIEPId=" + IEPId + " ";
    //    string retVal = Convert.ToString(objData.Execute(strQuery));
    //    return retVal;
    //}

    [WebMethod]
    public string submitIEP8(string txtAddInfoColDesc3)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];

        string strQuery = "Update StdtIEPExt3 set AddInfoCol3Desc='" + clsGeneral.convertQuotes(txtAddInfoColDesc3) + "'  where StdtIEPId=" + IEPId + " ";
        string retVal = Convert.ToString(objData.Execute(strQuery));
        return retVal;
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

                    strQuery = " Insert into StdtIEPExt3( StdtIEPId, AddInfoCol1, AddInfoCol2, AddInfoCol3, AddInfoCol3Desc,CreatedBy,CreatedOn) Values  (" + IEPId + ",'" + chkAddInfo1.Checked + "','" + chkAddInfo2.Checked + "','" + chkAddInfo3.Checked + "','" + clsGeneral.convertQuotes(txtAddInfoColDesc3.InnerHtml) + "'," + sess.LoginId + ",getdate())";
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
                    IEPId = sess.IEPId;
                    strQuery = " Update StdtIEPExt3 set StdtIEPId=" + IEPId + ", AddInfoCol1='" + chkAddInfo1.Checked + "', AddInfoCol2='" + chkAddInfo2.Checked + "', AddInfoCol3='" + chkAddInfo3.Checked + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page8='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page8) values(" + sess.IEPId + ",'true')");
                        }
                        //Clear();
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(9);", true);
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
                    IEPId = sess.IEPId;
                    strQuery = " Update StdtIEPExt3 set StdtIEPId=" + IEPId + ", AddInfoCol1='" + chkAddInfo1.Checked + "', AddInfoCol2='" + chkAddInfo2.Checked + "', AddInfoCol3='" + chkAddInfo3.Checked + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page8='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page8) values(" + sess.IEPId + ",'true')");
                        }
                        //Clear();
                        //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP9();", true);
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
    private void Clear()
    {
        IEPId = 0;
        txtAddInfoColDesc3.InnerHtml = "";
        chkAddInfo1.Checked = false;
        chkAddInfo2.Checked = false;
        chkAddInfo3.Checked = false;
        //btnSave.Text = "Update";

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
            string strQuery = "SELECT   IEP3.StdtIEPId,  IEP3.AddInfoCol1, IEP3.AddInfoCol2, IEP3.AddInfoCol3, IEP3.AddInfoCol3Desc FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt3 IEP3 ON IEP1.StdtIEPId = IEP3.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + " ";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count > 0)
            {
                IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);

                txtAddInfoColDesc3.InnerHtml = Dt.Rows[0]["AddInfoCol3Desc"].ToString().Trim();

                txtAddInfoColDesc3_hdn.Text = System.Uri.EscapeDataString(txtAddInfoColDesc3.InnerHtml);

                chkAddInfo1.Checked = clsGeneral.getChecked(Dt.Rows[0]["AddInfoCol1"].ToString());
                chkAddInfo2.Checked = clsGeneral.getChecked(Dt.Rows[0]["AddInfoCol2"].ToString());
                chkAddInfo3.Checked = clsGeneral.getChecked(Dt.Rows[0]["AddInfoCol3"].ToString());
                //if ((Dt.Rows[0]["AddInfoCol3Desc"].ToString() != "") || (Dt.Rows[0]["AddInfoCol1"].ToString() != "") || (Dt.Rows[0]["AddInfoCol2"].ToString() != "") || (Dt.Rows[0]["AddInfoCol3"].ToString() != ""))
                //{
                //    btnSave.Text = "Update";
                //}
                //else btnSave.Text = "Save";
            }
        }

    }

   
   
}