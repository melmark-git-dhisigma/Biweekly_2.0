using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Xml;
using System.IO.Packaging;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Net;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Text;
using OpenXmlPowerTools;

public partial class StudentBinder_CreateCustomIepPE : System.Web.UI.Page
{
    clsData objData = null;

    static string version;
    clsSession sess = null;
    clsSession oSession = null;

    static string Approvalnotes = null;
    static bool Disable = false;
    string[] NOTE, DATE;
    System.Data.DataTable _dtTest = null;
    DataClass objDataClass = new DataClass();

    static string[] columns;
    static string[] placeHolders;

    static string[] columnsCheck;


    static string[] columnsP4;

    static int checkCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(sess.perPage);
            if (flag == false)
            {
                // Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        if (!IsPostBack)
        {
            LoadData();
            FillRecentNotes();
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {

    }

    private void FillProgressIEP()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        object IepStatus = null;
        object InProgress = null;
        if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
        {
            IepStatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEP_PEId DESC ").ToString();
            InProgress = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(IepStatus.ToString()) + "");
            if (InProgress.ToString() == "In Progress")
            {
                ViewInProgressIEP();
                // ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
            }
            else
            {
                NOInprogressIEP();
            }
        }
        else
        {
            //NOInprogressIEP();
        }
    }

    private void ViewInProgressIEP()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        System.Data.DataTable dttmp = new System.Data.DataTable();
        btndeleteIEP.Style.Add("display", "block");
        lnkBtnIEPYear.Visible = true;
        dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEP_PEId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress') ORDER BY StdtIEP_PEId DESC ", true);
        sess.IEPId = int.Parse(dttmp.Rows[0][0].ToString());
        lnkBtnIEPYear.Text = dttmp.Rows[0][1].ToString();
        hdnFieldIep.Value = dttmp.Rows[0][0].ToString();
        sess.IEPStatus = int.Parse(dttmp.Rows[0][2].ToString());
        hdnFieldIep.Value = sess.IEPId.ToString();
        lnkBtnIEPYear.Visible = true;
        lnkBtnIndiviEdu.Visible = true;
        lnkBtnPrestLvlEdu.Visible = true;
        lnkBtnPrestLvlEdu2.Visible = true;
        lnkBtnCurPre.Visible = true;
        LnkBtnSerDel.Visible = true;
        lnkBtnNonParJus.Visible = true;
        lnkBtnStateOfDist.Visible = true;
        lnkBtnAddInfo.Visible = true;
        lnkBtnLocalAsmnt.Visible = true;
        lnkBtnGoalndObj.Visible = true;
        lnkBtnSpclConsider.Visible = true;
        lnkBtnGftdSupport.Visible = true;
        lnkBtnEdulPlcmnt.Visible = true;
        lnkBtnEdulPlcmnt2.Visible = true;
        lnkBtnPennData.Visible = true;
        btnUploadBSP.Style.Add("display", "block");
        btnIEPExport.Style.Add("display", "none");
    }

    private void NOInprogressIEP()
    {
        btndeleteIEP.Style.Add("display", "none");
        lnkBtnIEPYear.Visible = false;
        lnkBtnIndiviEdu.Visible = false;
        lnkBtnPrestLvlEdu.Visible = false;
        lnkBtnPrestLvlEdu2.Visible = false;
        lnkBtnCurPre.Visible = false;
        LnkBtnSerDel.Visible = false;
        lnkBtnNonParJus.Visible = false;
        lnkBtnStateOfDist.Visible = false;
        lnkBtnAddInfo.Visible = false;
        lnkBtnLocalAsmnt.Visible = false;
        lnkBtnGoalndObj.Visible = false;
        lnkBtnSpclConsider.Visible = false;
        lnkBtnGftdSupport.Visible = false;
        lnkBtnEdulPlcmnt.Visible = false;
        lnkBtnEdulPlcmnt2.Visible = false;
        lnkBtnPennData.Visible = false;
        btnUploadBSP.Style.Add("display", "none");
        btnIEPExport.Style.Add("display", "block");

        //  lnkBtnAddInfo.Visible = false;
    }
    private void LoadData()
    {
        btndeleteIEP.Style.Add("display", "none");
        btnUploadBSP.Style.Add("display", "none");
        lnkBtnIEPYear.Visible = false;
        lnkBtnIndiviEdu.Visible = false;
        lnkBtnPrestLvlEdu.Visible = false;
        lnkBtnPrestLvlEdu2.Visible = false;
        lnkBtnCurPre.Visible = false;
        LnkBtnSerDel.Visible = false;
        lnkBtnNonParJus.Visible = false;
        lnkBtnStateOfDist.Visible = false;
        lnkBtnAddInfo.Visible = false;
        lnkBtnLocalAsmnt.Visible = false;
        lnkBtnGoalndObj.Visible = false;
        lnkBtnSpclConsider.Visible = false;
        lnkBtnGftdSupport.Visible = false;
        lnkBtnEdulPlcmnt.Visible = false;
        lnkBtnEdulPlcmnt2.Visible = false;
        lnkBtnPennData.Visible = false;

        tdMsg.InnerHtml = "";
        if (sess != null)
        {
            sess.IEPId = 0;
        }

        Session["IEPid"] = null;

        //clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        hdFieldSucc.Value = "0";
        //if (Disable == true)
        //{
        //    lnkBtnSubmit.Visible = false;
        //    btnNewIEP.Visible = false;
        //    //checkIepEditCreateStatus(false);
        //}
        //else
        //{
        //    lnkBtnSubmit.Visible = true;
        //    btnNewIEP.Visible = true;
        //    //checkIepEditCreateStatus(true);
        //}
        setApprovePermission();
        FillProgressIEP();
        FillPendingIEP();
        FillApprovedIEP();
    }

    private void setApprovePermission()
    {
        clsGeneral.ApprovePermissions(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            hfCheckError.Value = "false";
        }
        else
        {
            hfCheckError.Value = "true";
        }
    }

    protected void btnNewIEP_Click(object sender, EventArgs e)
    {
        btnGennewIEP.Visible = true;
        btnGenIED.Visible = false;

        /// POPUP PANELS
        pnl_createNewIEPmsg.Visible = true;
        pnl_currIEPdetails.Visible = false;
        pnl_iepDatePicker.Visible = true;

        checkIepEditCreateStatus(true);
    }
    protected void btnNewExistIEP_Click(object sender, EventArgs e)
    {
        btnGennewIEP.Visible = false;
        btnGenIED.Visible = true;

        /// POPUP PANELS
        pnl_createNewIEPmsg.Visible = false;
        pnl_currIEPdetails.Visible = true;
        pnl_iepDatePicker.Visible = false;

        checkExistingIepEditCreateStatus(true);

        setCurrentIEP();

        string popup = " $(document).ready(function () { $('#sign_up5').show(); });";
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
    }

    private void setCurrentIEP()
    {
        objData = new clsData();
        lbl_CurrYear.Text = objData.FetchValue("Select TOP 1 yr.AsmntYearDesc +' Version - '+ iep.Version AS Name,iep.StdtIEP_PEId As ID from [dbo].[StdtIEP_PE] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + sess.StudentId + "  And iep.SchoolId=" + sess.SchoolId + " Order By iep.StdtIEP_PEId Desc").ToString();

    }

    public void checkExistingIepEditCreateStatus(bool permission)
    {
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + "  ") == true)
        {
            pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEP_PEId DESC ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
            if (IepStatus.ToString() == "Approved")
            {
                DataTable dtdates = objData.ReturnDataTable("Select TOP 1 EffStartDate,EffEndDate from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEP_PEId DESC ", false);

                txtSdate.Text = Convert.ToDateTime(dtdates.Rows[0]["EffStartDate"]).ToString("MM'/'dd'/'yyyy");
                txtEdate.Text = Convert.ToDateTime(dtdates.Rows[0]["EffEndDate"]).ToString("MM'/'dd'/'yyyy");
                string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
            }

            else
            {


                //System.Data.DataTable dttmp = new System.Data.DataTable();
                ////   tdMsgMain.InnerHtml = clsGeneral.warningMsg("New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ");
                ////   ClientScript.RegisterStartupScript(GetType(), "", "<script type='text/javascript'>alert('New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ');</script>", true);
                //lnkBtnIEPYear.Visible = true;
                //dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEP_PEId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEP_PEId DESC ", true);
                //sess.IEPId = int.Parse(dttmp.Rows[0][0].ToString());
                //lnkBtnIEPYear.Text = dttmp.Rows[0][1].ToString();
                //hdnFieldIep.Value = dttmp.Rows[0][0].ToString();
                //sess.IEPStatus = int.Parse(dttmp.Rows[0][2].ToString());
                //if (permission == true)
                //{
                //    setSubmitAprove();
                //}
                //hdnFieldIep.Value = sess.IEPId.ToString();
                //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "CreateIEP1();", true);
                ClientScript.RegisterStartupScript(GetType(), "", "showMessage('New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ');", true);
                //ClientScript.RegisterStartupScript(GetType(), "", "showMessage('New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ');", true);


            }


        }
        else
        {
            string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        }




        //IEP Status select lookupname from LookUp where LookupId=
    }

    

    protected void btnGennewIEP_Click(object sender, EventArgs e)
    {
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (validate() == true)
        {

            if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
            {
                pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEP_PEId DESC ").ToString();

                if (int.Parse(pendstatus.ToString()) > 0)
                {
                    IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
                }
                if (IepStatus.ToString() == "Approved")
                {
                    createIepFunc("NEW");
                }
                else
                {
                    tdMessage.InnerHtml = clsGeneral.failedMsg("IEP Already Exist in Pending State");
                    //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
                }
            }
            else
            {
                createIepFunc("NEW");
            }

        }
        if (hdFieldSucc.Value == "0")
        {
            string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        }
    }
    
    protected void btnGenIED_Click(object sender, EventArgs e)
    {
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (validate() == true)
        {

            if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
            {
                pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEP_PEId DESC ").ToString();

                if (int.Parse(pendstatus.ToString()) > 0)
                {
                    IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
                }
                if (IepStatus.ToString() == "Approved")
                {
                    createIepFunc("OLD");
                }
                else
                {
                    tdMessage.InnerHtml = clsGeneral.failedMsg("IEP Already Exist in Pending State");
                    //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
                }
            }
            else
            {
                createIepFunc("OLD");
            }

        }
        if (hdFieldSucc.Value == "0")
        {
            string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        }
    }

    public void createIepFunc(string Status)
    {
        objData = new clsData();

        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DataClass oData = new DataClass();
        oSession = (clsSession)Session["UserSession"];
        String yrsd = dtst.Year.ToString();
        String yred = (dtst.Year + 1).ToString();
        String year = yrsd + "-" + yred;
        string sQuery = "";
        string stQuery = "";
        string strQy = "";
        string strQuy = "";
        string strQey = "";

        string currYearPair = DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddYears(1).Year.ToString();

        int ExistYear = Convert.ToInt32(objData.FetchValue("select COUNT(*) from AsmntYear WHERE AsmntYearCode='" + year + "'"));
        if (ExistYear != 0) //selected value exists in table
        {
            //check if CurrentInd = A. so no change.
            //if CurrentInd =D, make it to A and change all others to D
            strQy = "select CurrentInd from AsmntYear where AsmntYearCode ='" + year + "'";
            String curntInd = objData.FetchValue(strQy).ToString(); //returns the curentInd from table
            if (curntInd == "D")
            {
                if (currYearPair == year)
                {
                    strQuy = "update AsmntYear set CurrentInd = 'A', ModifiedBy = '" + sess.LoginId + "', ModifiedOn = GETDATE() where AsmntYearCode ='" + year + "'";
                    objData.Execute(strQuy);
                    strQey = "update AsmntYear set CurrentInd = 'D', ModifiedBy = '" + sess.LoginId + "', ModifiedOn = GETDATE() where AsmntYearCode <>'" + year + "'";
                    objData.Execute(strQey);
                }

            }
        }
        else
        {
            if (currYearPair == year)
            {
                sQuery = "INSERT INTO AsmntYear(SchoolId, AsmntYearCode, AsmntYearDesc, AsmntYearStartDt,AsmntYearEndDt, CurrentInd, CreatedBy, CreateOn) VALUES ('" + sess.SchoolId + "','" + year + "','" + year + "','" + dtst.ToString("yyyy-MM-dd") + "','" + dted.ToString("yyyy-MM-dd") + "','A','" + sess.LoginId + "',GETDATE())";
                objData.Execute(sQuery);

                stQuery = "UPDATE AsmntYear set CurrentInd = 'D', ModifiedBy = '" + sess.LoginId + "', ModifiedOn = GETDATE() where AsmntYearCode <>'" + year + "'";
                objData.Execute(stQuery);
            }
            else
            {
                sQuery = "INSERT INTO AsmntYear(SchoolId, AsmntYearCode, AsmntYearDesc, AsmntYearStartDt,AsmntYearEndDt, CurrentInd, CreatedBy, CreateOn) VALUES ('" + sess.SchoolId + "','" + year + "','" + year + "','" + dtst.ToString("yyyy-MM-dd") + "','" + dted.ToString("yyyy-MM-dd") + "','D','" + sess.LoginId + "',GETDATE())";
                objData.Execute(sQuery);
            }
        }
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + "  ") == true) //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
        {
            pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + "  ORDER BY StdtIEP_PEId DESC ").ToString(); //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
        }

        int GlstatusID = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='Goal Status' AND LookupName='In Progress'");
        int LPstatusID = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='LP Status' AND LookupName='In Progress'");
        if ((LPstatusID > 0) && (GlstatusID > 0))
        {
            oSession = (clsSession)Session["UserSession"];
            //string selIEPstatus = "SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'";
            string selIEPstatus = "SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'";
            int IEPstatus = oData.ExecuteScalar(selIEPstatus);
            if (IEPstatus > 0)
            {

                int IdApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
                string copyIEP = "SELECT ISNULL(MAX(StdtIEP_PEId),0) AS StdtIEP_PEId FROM StdtIEP_PE WHERE StudentId=" + oSession.StudentId + " "; //and AsmntYearId=" + Convert.ToInt32(ddlYear.SelectedValue) + "
                int Oldiep = Convert.ToInt32(objData.FetchValue(copyIEP));

                int IEP_id = 0;

                oSession = (clsSession)Session["UserSession"];
                objData = new clsData();
                version = "";

                if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + " AND AsmntYearId=(SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "')  ") == true) //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
                {
                    version = objData.FetchValue("Select TOP 1 Version from StdtIEP_PE where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + " AND AsmntYearId=(SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "') ORDER BY StdtIEP_PEId DESC ").ToString(); //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
                    version = Convert.ToString(Convert.ToDouble(version) + 0.1);
                }
                else if (version == "") version = "0.0";

                if (Status == "NEW")
                {
                    Oldiep = 0;
                    int ver = (int)Math.Floor(Convert.ToDouble(version)) + 1; version = Convert.ToString(ver) + ".0";
                }


                if (Oldiep > 0)
                {
                    string insOLDIEP = "INSERT INTO StdtIEP_PE([SchoolId],[StudentId],[AsmntYearId],[StatusId],[EffStartDate],[EffEndDate],[IepTeamMeetingDate],[IepImplementationDate],[AnticipatedDurationofServices],[AnticipatedYearOfGraduation],[LocalEducationAgency],[OtherInformation],[DocumentedBy],[CountyOfResidance],[Concerns],[Strength],[Vision],[Version],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                              "Select " + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "'),'" + IEPstatus + "','" + dtst.ToString("yyyy-MM-dd") + "','" + dted.ToString("yyyy-MM-dd") + "',[IepTeamMeetingDate],[IepImplementationDate],[AnticipatedDurationofServices],[AnticipatedYearOfGraduation],[LocalEducationAgency],[OtherInformation],[DocumentedBy],[CountyOfResidance],[Concerns],[Strength],[Vision],'" + version + "'," + //" + Convert.ToInt32(ddlYear.SelectedValue) + "
                              "" + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtIEP_PE where StdtIEP_PEId='" + Oldiep + "'\t\n" +
                              "SELECT SCOPE_IDENTITY()";
                    IEP_id = oData.ExecuteScalar(insOLDIEP);

                    string InsBsp = "INSERT INTO BSPDoc ([StdtIEPId],[Data],[BSPDocUrl],[SchoolId],[CreatedBy],[CreatedOn]) SELECT '" + IEP_id + "',[Data],[BSPDocUrl],[SchoolId],[CreatedBy],[CreatedOn] FROM BSPDoc WHERE [StdtIEPId]='" + Oldiep + "' ";
                    int InsBspVal = oData.ExecuteNonQuery(InsBsp);


                    //string IEPNE = "INSERT INTO StdtIEP([SchoolId],[StudentId],[AsmntYearId],[StatusId],[EffStartDate],[EffEndDate],[Concerns],[Strength],[Vision],[Version],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                    //          "Select " + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'),'" + IEPstatus + "','" + dtst.ToString("yyyy-MM-dd") + "','" + dted.ToString("yyyy-MM-dd") + "',[Concerns],[Strength],[Vision],'" + version + "'," + //" + Convert.ToInt32(ddlYear.SelectedValue) + "
                    //          "" + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtIEP where StdtIEPId='" + Oldiep + "'\t\n" +
                    //          "SELECT SCOPE_IDENTITY()";
                    //int IEPNE_Id = oData.ExecuteScalar(IEPNE);
                }
                else
                {
                    string insIEP = "INSERT INTO StdtIEP_PE(SchoolId,StudentId,AsmntYearId,Version,EffStartDate,EffEndDate,StatusId,CreatedBy,CreatedOn) " +
                              "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "'),'" + version + "','" + dtst.ToString("yyyy-MM-dd") + "'," +
                              "'" + dted.ToString("yyyy-MM-dd") + "'," + IEPstatus + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                              "SELECT SCOPE_IDENTITY()";
                    IEP_id = oData.ExecuteScalar(insIEP);

                    //string insIEPNE = "INSERT INTO StdtIEP(SchoolId,StudentId,AsmntYearId,Version,EffStartDate,EffEndDate,StatusId,CreatedBy,CreatedOn) " +
                    //          "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'),'" + version + "','" + dtst.ToString("yyyy-MM-dd") + "'," +
                    //          "'" + dted.ToString("yyyy-MM-dd") + "'," + IEPstatus + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                    //          "SELECT SCOPE_IDENTITY()";
                    //int IEPNE_id = oData.ExecuteScalar(insIEPNE);
                }
                if (IEP_id > 0)
                {
                    if (Oldiep > 0)
                    {

                        string strQry = "insert into IEP_PE_Details([StdtIEP_PEId],[IEP3ParentSign],[IEP4IsBlind],[IEP4Isdeaf],[IEP4CommNeeded],[IEP4AssistiveTechNeeded],"
                               + "[IEP4EnglishProficiency],[IEP4ImpedeLearning],[IEP5Disability],[IEP5OtherSpecify],[IEP5CIPCode],[IEP6TrainingGoal],[IEP6TrainCoursesStudy],[IEP6EmploymentGoal],[IEP6EmpCoursesStudy],"
                               + "[IEP6LivingGoal],[IEP6LivingCoursesStudy],[IEP6MeasurableCheck1],[IEP6MeasurableCheck2],[IEP6MeasurableCheck3],[IEP8PSSAReading],[IEP8PSSAappropriate],[IEP8Videotape],[IEP8WrittenNarrative],"
                               + "[IEP8AsmtStdtGrade],[IEP8AsmtWithoutAcc],[IEP8LocalAltrntAssmnt],[IEP8AsmtNotAdministred],[IEP8ReadPartcptPSSAWithoutAcmdtn],[IEP8ReadPartcptPSSAWithFollowingAcmdtn],[IEP8ReadPartcptPSSAModiWithoutAcmdtn],"
                               + "[IEP8ReadPartcptPSSAModiWithFollowingAcmdtn],[IEP8SciencePartcptPSSAWithoutAcmdtn],[IEP8SciencePartcptPSSAWithFollowingAcmdtn],[IEP8SciencePartcptPSSAModiWithoutAcmdtn],[IEP8SciencePartcptPSSAModiWithFollowingAcmdtn],"
                               + " [IEP8MathPartcptPSSAWithoutAcmdtn],[IEP8MathPartcptPSSAWithFollowingAcmdtn],[IEP8MathPartcptPSSAModiWithoutAcmdtn],[IEP8MathPartcptPSSAModiWithFollowingAcmdtn],[IEP8WritePartcptPSSAWithoutAcmdtn],"
                               + " [IEP8WritePartcptPSSAWithFollowingAcmdtn],[IEP8AltrntAssmntAppropriate],[IEP8PSSAParticipate],[IEP8AsmtWithAcc],[IEP8AssmtAcc],[IEP9AlernativeAsmt],[IEP9NoRegAsmt],[IEP9AssmtAppropriate],[IEP10Benchmarks1],"
                               + "[IEP10Benchmarks2],[IEP10Benchmarks3],[IEP12ElegibleForESY],[IEP12ElegibleForESYInfo],[IEP12NotElegibleForESY],[IEP12NotElegibleForESYInfo],[IEP12ShortTermObjectives],[IEP13RegularEdu],[IEP13GeneralEdu],[IEP14Itinerant],[IEP14Supplemental],"
                               + "[IEP14FullTime],[IEP14AutisticSupport],[IEP14Blind],[IEP14Deaf],[IEP14Emotional],[IEP14Learning],[IEP14LifeSkills],[IEP14MultipleDisabilities],"
                               + "[IEP14Physical],[IEP14Speech],[IEP14SchoolDistrict],[IEP14SchoolBuilding],[IEP14IsNeighbour],[IEP14IsNeibhourNo],[IEP14SpclEdu],[IEP14Other],"
                               + "[IEP15RegularCls80],[IEP15Regular79],[IEP15Regular40],[IEP15ApprovePrivateSchool],[IEP15ApprovePrivateText],[IEP15OtherPublic],[IEP15OtherPublicText],"
                               + "[IEP15ApproveResidential],[IEP15ApproveResidentialText],[IEP15Hospital],[IEP15HospitalText],[IEP15PrivateFacility],[IEP15PrivateFacilityText],[IEP15CorrectionalFacility],"
                               + "[IEP15CorrectionText],[IEP15PrivateResi],[IEP15PrivateResText],[IEP15ChkoutState],[IEP15ChkoutStateText],[IEP15PublicFacility],[IEP15PublicFacilityText],[IEP15InstructionConducted],[IEP15InstructionText])"

                               + "select '" + IEP_id + "',[IEP3ParentSign],[IEP4IsBlind],[IEP4Isdeaf],[IEP4CommNeeded],[IEP4AssistiveTechNeeded],"
                               + "[IEP4EnglishProficiency],[IEP4ImpedeLearning],[IEP5Disability],[IEP5OtherSpecify],[IEP5CIPCode],[IEP6TrainingGoal],[IEP6TrainCoursesStudy],[IEP6EmploymentGoal],[IEP6EmpCoursesStudy],"
                               + "[IEP6LivingGoal],[IEP6LivingCoursesStudy],[IEP6MeasurableCheck1],[IEP6MeasurableCheck2],[IEP6MeasurableCheck3],[IEP8PSSAReading],[IEP8PSSAappropriate],[IEP8Videotape],[IEP8WrittenNarrative],"
                               + "[IEP8AsmtStdtGrade],[IEP8AsmtWithoutAcc],[IEP8LocalAltrntAssmnt],[IEP8AsmtNotAdministred],[IEP8ReadPartcptPSSAWithoutAcmdtn],[IEP8ReadPartcptPSSAWithFollowingAcmdtn],[IEP8ReadPartcptPSSAModiWithoutAcmdtn],"
                               + "[IEP8ReadPartcptPSSAModiWithFollowingAcmdtn],[IEP8SciencePartcptPSSAWithoutAcmdtn],[IEP8SciencePartcptPSSAWithFollowingAcmdtn],[IEP8SciencePartcptPSSAModiWithoutAcmdtn],[IEP8SciencePartcptPSSAModiWithFollowingAcmdtn],"
                               + " [IEP8MathPartcptPSSAWithoutAcmdtn],[IEP8MathPartcptPSSAWithFollowingAcmdtn],[IEP8MathPartcptPSSAModiWithoutAcmdtn],[IEP8MathPartcptPSSAModiWithFollowingAcmdtn],[IEP8WritePartcptPSSAWithoutAcmdtn],"
                               + " [IEP8WritePartcptPSSAWithFollowingAcmdtn],[IEP8AltrntAssmntAppropriate],[IEP8PSSAParticipate],[IEP8AsmtWithAcc],[IEP8AssmtAcc],[IEP9AlernativeAsmt],[IEP9NoRegAsmt],[IEP9AssmtAppropriate],[IEP10Benchmarks1],"
                               + "[IEP10Benchmarks2],[IEP10Benchmarks3],[IEP12ElegibleForESY],[IEP12ElegibleForESYInfo],[IEP12NotElegibleForESY],[IEP12NotElegibleForESYInfo],[IEP12ShortTermObjectives],[IEP13RegularEdu],[IEP13GeneralEdu],[IEP14Itinerant],[IEP14Supplemental],"
                               + "[IEP14FullTime],[IEP14AutisticSupport],[IEP14Blind],[IEP14Deaf],[IEP14Emotional],[IEP14Learning],[IEP14LifeSkills],[IEP14MultipleDisabilities],"
                               + "[IEP14Physical],[IEP14Speech],[IEP14SchoolDistrict],[IEP14SchoolBuilding],[IEP14IsNeighbour],[IEP14IsNeibhourNo],[IEP14SpclEdu],[IEP14Other],"
                               + "[IEP15RegularCls80],[IEP15Regular79],[IEP15Regular40],[IEP15ApprovePrivateSchool],[IEP15ApprovePrivateText],[IEP15OtherPublic],[IEP15OtherPublicText],"
                               + "[IEP15ApproveResidential],[IEP15ApproveResidentialText],[IEP15Hospital],[IEP15HospitalText],[IEP15PrivateFacility],[IEP15PrivateFacilityText],[IEP15CorrectionalFacility],"
                               + "[IEP15CorrectionText],[IEP15PrivateResi],[IEP15PrivateResText],[IEP15ChkoutState],[IEP15ChkoutStateText],[IEP15PublicFacility],[IEP15PublicFacilityText],[IEP15InstructionConducted],[IEP15InstructionText]"
                               + "from IEP_PE_Details where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into IEP_PE10_Benchmark ([StdIEP_PEId],[Benchmark])"
                                    + "select '" + IEP_id + "',[Benchmark] from "
                                    + "IEP_PE10_Benchmark where [StdIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into IEP_PE11_SchoolPer ([StdtIEP_PEId],[SchoolPerson],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[SchoolPerson],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from "
                                    + "IEP_PE11_SchoolPer where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE11_SDI] ([StdtIEP_PEId],[SDI],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[SDI],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from "
                                    + "[IEP_PE11_SDI] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE11_Service] ([StdtIEP_PEId],[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from "
                                    + "[IEP_PE11_Service] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE12_DateOfRev] ([StdtIEP_PEId],[DateofRevision],[Participants],[IEPSection])"
                                    + "select '" + IEP_id + "',[DateofRevision],[Participants],[IEPSection] from "
                                    + " [dbo].[IEP_PE12_DateOfRev] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE12_ESY] ([StdtIEP_PEId],[ESY],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[ESY],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from "
                                    + "[dbo].[IEP_PE12_ESY] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE12_SupportService] ([StdIEP_PEId],[SupportService])"
                                    + "select '" + IEP_id + "',[SupportService] from "
                                    + "[dbo].[IEP_PE12_SupportService] where [StdIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE2_Team] ([StdtIEP_PEId],[ParentName],[ParentSing],[StudentName],[StudentSign],[RegEduTeacherName],[regEduTeacherSign],"
                                    + "[SpclEduTeacherName],[SpclEduTeacherSign],[LocalEdAgencyName],[localEdAgencySign],[CareerEdRepName],[careerEdRepSign],[CommunityAgencyName],"
                                    + "[CommunityAgencySign],[TeacherGiftedName],[TeacherGiftedSign],[WittenInput])"
                                    + "select '" + IEP_id + "',[ParentName],[ParentSing],"
                                    + "[StudentName],[StudentSign],[RegEduTeacherName],[regEduTeacherSign],[SpclEduTeacherName],[SpclEduTeacherSign],[LocalEdAgencyName],"
                                    + "[localEdAgencySign],[CareerEdRepName],[careerEdRepSign],[CommunityAgencyName],[CommunityAgencySign],[TeacherGiftedName],[TeacherGiftedSign],"
                                    + "[WittenInput] from [dbo].[IEP_PE2_Team] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE6_Edu] ([StdtIEP_PEId],[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from [dbo].[IEP_PE6_Edu]"
                                    + "where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into  [dbo].[IEP_PE6_Goal] ([StdtIEP_PEId],[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from [dbo].[IEP_PE6_Goal]"
                                    + "where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE6_GoalsObj] ([StdtIEP_PEId],[AnnualGoal],[StudentsProgress],[ParentProgessReport],[ProgressReport])"
                                    + "select '" + IEP_id + "',[AnnualGoal],[StudentsProgress],[ParentProgessReport],[ProgressReport] from [dbo].[IEP_PE6_GoalsObj]"
                                    + "where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEP_PE6_Living] ([StdtIEP_PEId],[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person])"
                                    + "select '" + IEP_id + "',[Service],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from "
                                    + "[dbo].[IEP_PE6_Living] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[IEPPA1Extension] ([IepPAId],[DateOfRevisions],[Participants],[IEPSections],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn])"
                                    + "select '" + IEP_id + "',[DateOfRevisions],[Participants],[IEPSections],'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate() from [dbo].[IEPPA1Extension]"
                                    + "where [IepPAId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                        strQry = "insert into [dbo].[StdtIEP_PE10_GoalsObj] ([StdtIEP_PEId],[MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress],[StatusId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn])"
                                    + "select '" + IEP_id + "',[MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress]," + IEPstatus + "," + oSession.LoginId + ",getdate(),'" + oSession.LoginId + "',getdate() from "
                                    + "[dbo].[StdtIEP_PE10_GoalsObj] where [StdtIEP_PEId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(strQry);

                    }
                    else
                    {

                        string insIepPE = "INSERT INTO [dbo].[IEP_PE_Details] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        int id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE10_Benchmark] (StdIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE11_SchoolPer] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE11_SDI] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE11_Service] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE12_DateOfRev] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE12_ESY] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE12_SupportService] (StdIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE2_Team] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE6_Edu] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE6_Goal] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE6_GoalsObj] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEP_PE6_Living] (StdtIEP_PEId) VALUES(" + IEP_id + ")";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[IEPPA1Extension] (IepPAId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + oSession.LoginId + ", (SELECT convert(varchar, getdate(), 100)))";
                        id = oData.ExecuteNonQuery(insIepPE);

                        insIepPE = "INSERT INTO [dbo].[StdtIEP_PE10_GoalsObj] (StdtIEP_PEId,StatusId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + IEPstatus + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                        id = oData.ExecuteNonQuery(insIepPE);


                    }
                }
                //}

                if (IEP_id > 0)
                {
                    SqlTransaction Trans = null;
                    SqlConnection Con = new SqlConnection();
                    clsData clsData = new clsData();
                    try
                    {

                        oSession = (clsSession)Session["UserSession"];

                        if (Oldiep > 0)
                        {
                            Con = clsData.Open();
                            Trans = Con.BeginTransaction();
                            System.Data.DataTable dtgoal = new System.Data.DataTable();
                            System.Data.DataTable dtLP = new System.Data.DataTable();
                            Object Oldgoaldata = clsData.FetchValueTrans("Select count(*) as tot from StdtGoal wHERE StdtIEPId=" + Oldiep + "", Trans, Con);
                            string strStdtGoal = "select DISTINCT GoalId from StdtGoal where StdtIEPId=" + Oldiep + "";
                            dtgoal = clsData.ReturnDataTableWithTransaction(strStdtGoal, Con, Trans, false);

                            if (Convert.ToInt32(Oldgoaldata) > 0)
                            {
                                foreach (DataRow row in dtgoal.Rows)
                                {
                                    string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,[CurPerfLevel],[AnnualGoalDesc],[Objective1],[Objective2],[Objective3],CreatedBy,CreatedOn) " +
                                                 "select " + oSession.SchoolId + "," + oSession.StudentId.ToString() + ",GoalId," + IEP_id + ",(SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "')," +
                                                 "" + GlstatusID + ",'A',1,[CurPerfLevel],[AnnualGoalDesc],[Objective1],[Objective2],[Objective3]," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtGoal Where StdtIEPId='" + Oldiep + "' AND GoalId=" + Convert.ToInt32(row["GoalId"]) + " ";


                                    clsData.ExecuteWithTrans(insGoal, Con, Trans);


                                    //string stdtgoalid = "Select count(*) as tot from StdtGoalSvc wHERE StdtIEP_PEId=" + Convert.ToInt32(row["StdtGoalId"]) + " ";
                                    //int goalsvcid = oData.ExecuteScalar(stdtgoalid);
                                    //if (newgoalid > 0)
                                    //{
                                    //System.Data.DataTable dtgoalsvc = new System.Data.DataTable();
                                    //dtgoalsvc = objData.ReturnDataTable(stdtgoalid, false);
                                    //if (dtgoalsvc.Rows.Count > 0)
                                    //{
                                    //    foreach (DataRow svc in dtgoalsvc.Rows)
                                    //    {
                                    //string goalsvc = "INSERT INTO StdtGoalSvc (StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,CreatedBy,CreatedOn,StdtIEPId,[ModifiedBy],[ModifiedOn]) " +
                                    // "Select StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,'" + oSession.LoginId + "',getdate(),'" + IEP_id + "','" + oSession.LoginId + "',getdate() From StdtGoalSvc Where StdtIEPId='" + Oldiep + "'";
                                    //clsData.ExecuteWithTrans(goalsvc, Con, Trans);
                                    //}
                                    //}
                                    //}
                                }
                            }
                            








                            clsAssignLessonPlan objAssign = new clsAssignLessonPlan();


                            //string insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,StdtIEP_PEId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,CreatedBy,CreatedOn) " +
                            //                "VALUES(" + oSession.SchoolId + "," + oSession.StudentId.ToString() + ",LessonPlanId,GoalId," + IEP_id + "," +
                            //                "(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A')," + LPstatusID + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";




                            string insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,Objective1,Objective2,Objective3,IncludeIEP,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,CreatedOn) " +
                                "Select SchoolId,StudentId,LessonPlanId,GoalId,'" + IEP_id + "',AsmntYearId," + LPstatusID + ",ActiveInd,Objective1,Objective2,Objective3,IncludeIEP,LessonPlanTypeDay,LessonPlanTypeResi," + oSession.LoginId + ",getdate() from StdtLessonPlan Where StdtIEPId='" + Oldiep + "'";
                            clsData.ExecuteWithTrans(insLP, Con, Trans);

                            string OldLPdata = "SELECT StdtLessonPlanId,LessonPlanId,GoalId FROM StdtLessonPlan WHERE StdtIEPId=" + IEP_id + "";
                            dtLP = clsData.ReturnDataTableWithTransaction(OldLPdata, Con, Trans, false);



                            if (dtLP != null)
                            {
                                if (dtLP.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtLP.Rows)
                                    {
                                        int visualLessonId = 0;
                                        int apprvdLessonId = 0;
                                      
                                        System.Data.DataTable LessonPlan = objData.ReturnDataTableWithTransaction("SELECT LessonPlanName from LessonPlan where LessonPlanId=" + row["LessonPlanId"] + "", Con, Trans, false);
                                        // objAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(row["LessonPlanId"]), LessonPlan.Rows[0]["LessonPlanName"].ToString(), oSession.LoginId, lpID, Con, Trans);
                                        string strQuery = "SELECT max(DSTempHdrId) FROM dbo.DSTempHdr WHERE LessonPlanId=" + Convert.ToInt32(row["LessonPlanId"]) + " AND StudentId=" + oSession.StudentId + " AND (StatusId=(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Approved') OR StatusId=(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Maintenance'))";
                                        object app = objData.FetchValueTrans(strQuery, Trans, Con);
                                        if (app.ToString() != "")
                                        {
                                            apprvdLessonId = Convert.ToInt32(app);
                                            visualLessonId = ReturnNewVLessonId(apprvdLessonId);
                                            if (objData.IFExistsWithTranss("SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + Convert.ToInt32(row["LessonPlanId"]) + " AND StudentId=" + oSession.StudentId + " AND StatusId=(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress')", Trans, Con) != true)
                                            {
                                                objAssign.CopyCustomtemplate(apprvdLessonId, oSession.LoginId, visualLessonId, Convert.ToInt32(row["StdtLessonPlanId"]));
                                            }
                                        }

                                    }
                                }
                            }
                            objData.ExecuteWithTrans("update StdtIEP_PE set StatusId=(select LookupId from lookup where LookupType='IEP Status' and LookupName='Expired') where StdtIEP_PEId=" + Oldiep, Con, Trans);
                            objData.ExecuteWithTrans("update StdtIEP set StatusId=(select LookupId from lookup where LookupType='IEP Status' and LookupName='Expired') where StdtIEPId=" + Oldiep, Con, Trans);
                            //objData.Execute("update StdtGoal set ActiveInd='D' where IncludeIEP=1 and StdtIEP_PEId=" + Oldiep);
                            //objData.Execute("update StdtLessonPlan set ActiveInd='D' where IncludeIEP=1 and StdtIEP_PEId=" + Oldiep);
                            clsData.CommitTransation(Trans, Con);
                        }
                        else if (Status == "NEW")
                        {
                            Con = clsData.Open();
                            Trans = Con.BeginTransaction();
                            System.Data.DataTable dtgoal = new System.Data.DataTable();
                            System.Data.DataTable dtLP = new System.Data.DataTable();
                            Object Oldgoaldata = clsData.FetchValueTrans("Select count(*) as tot from StdtGoal WHERE StudentId=" + oSession.StudentId + " AND ActiveInd='A'", Trans, Con);

                            if (Convert.ToInt32(Oldgoaldata) > 0)
                            {
                                string strStdtGoal = "select DISTINCT GoalId,IEPGoalNo from StdtGoal where StudentId=" + oSession.StudentId + " AND ActiveInd='A' AND IEPGoalNo IS NOT NULL order by IEPGoalNo";
                                dtgoal = clsData.ReturnDataTableWithTransaction(strStdtGoal, Con, Trans, false);
                                foreach (DataRow row in dtgoal.Rows)
                                {
                                    string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,IEPGoalNo,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,CreatedBy,CreatedOn) " +
                                                     "VALUES( " + oSession.SchoolId + "," + oSession.StudentId.ToString() + "," + row["GoalId"] + "," + row["IEPGoalNo"] + "," + IEP_id + ",(SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "')," +
                                                     "" + GlstatusID + ",'A',0," + oSession.LoginId + ",GETDATE()) ";

                                    int newgoalid = clsData.ExecuteWithScopeandConnection(insGoal, Con, Trans);



                                }
                                //}
                            }

                            string OldLPdata = "SELECT DISTINCT LessonPlanId,GoalId,LessonPlanTypeDay,LessonPlanTypeResi FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND ActiveInd='A'";
                            dtLP = objData.ReturnDataTableWithTransaction(OldLPdata, Con, Trans, false);
                            string AsmntYr = Convert.ToString(objData.FetchValueTrans("SELECT AsmntYearId FROM AsmntYear WHERE  AsmntYearDesc='" + year + "'", Trans, Con));

                            clsAssignLessonPlan objAssign = new clsAssignLessonPlan();

                            if (lList.SelectedValue == "2")
                            {
                                if (dtLP != null)
                                {
                                    if (dtLP.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in dtLP.Rows)
                                        {

                                            int visualLessonId = 0;
                                            int apprvdLessonId = 0;
                                            string insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,CreatedOn) " +
                                            "VALUES( " + oSession.SchoolId + "," + oSession.StudentId + ",'" + Convert.ToInt32(row["LessonPlanId"]) + "','" + Convert.ToInt32(row["GoalId"]) + "','" + IEP_id + "','" + AsmntYr + "'," + LPstatusID + ",'A',0,'" + row["LessonPlanTypeDay"] + "','" + row["LessonPlanTypeResi"] + "'," + oSession.LoginId + ",getdate())";
                                            int lpID = objData.ExecuteWithScopeandConnection(insLP, Con, Trans);
                                            System.Data.DataTable LessonPlan = objData.ReturnDataTableWithTransaction("SELECT LessonPlanName from LessonPlan where LessonPlanId=" + row["LessonPlanId"] + "", Con, Trans, false);
                                            // objAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(row["LessonPlanId"]), LessonPlan.Rows[0]["LessonPlanName"].ToString(), oSession.LoginId, lpID, Con, Trans);
                                            string strQuery = "SELECT max(DSTempHdrId) FROM dbo.DSTempHdr WHERE LessonPlanId=" + Convert.ToInt32(row["LessonPlanId"]) + " AND StudentId=" + oSession.StudentId + " AND (StatusId=(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Approved') OR StatusId=(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Maintenance'))";
                                            object app = objData.FetchValueTrans(strQuery, Trans, Con);
                                            if (app.ToString() != "")
                                            {
                                                apprvdLessonId = Convert.ToInt32(app);
                                                visualLessonId = ReturnNewVLessonId(apprvdLessonId);
                                                if (objData.IFExistsWithTranss("SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + Convert.ToInt32(row["LessonPlanId"]) + " AND StudentId=" + oSession.StudentId + " AND StatusId=(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress')", Trans, Con) != true)
                                                {
                                                    objAssign.CopyCustomtemplate(apprvdLessonId, oSession.LoginId, visualLessonId, lpID);
                                                }
                                            }



                                        }
                                    }
                                }
                            }
                                objData.ExecuteWithTrans("update StdtIEP set StatusId=(select LookupId from lookup where LookupType='IEP Status' and LookupName='Expired') where StdtIEPId=" + Oldiep, Con, Trans);
                                //objData.Execute("update StdtGoal set ActiveInd='D' where IncludeIEP=1 and StdtIEPId=" + Oldiep);
                                //objData.Execute("update StdtLessonPlan set ActiveInd='D' where IncludeIEP=1 and StdtIEPId=" + Oldiep);
                                clsData.CommitTransation(Trans, Con);
                            
                        }

                        oSession.IEPId = IEP_id;
                        oSession.IEPStatus = IEPstatus;
                        hdnFieldIep.Value = IEP_id.ToString();
                        lnkBtnIEPYear.Text = txtSdate.Text.Replace('-', '/') + " - " + txtEdate.Text.Replace('-', '/');
                        // tdmsg.InnerHtml = clsGeneral.sucessMsg("Successfully Inserted");
                        //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
                        hdFieldSucc.Value = "1";
                    }

                    catch (Exception Ex)
                    {
                        clsData.RollBackTransation(Trans, Con);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
                        throw Ex;
                    }
                }
                else
                {
                    //tdmsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed in IEP Table");
                    ClientScript.RegisterStartupScript(GetType(), "", "alert('Insertion Failed in IEP Table')", true);
                    hdFieldSucc.Value = "0";
                }

            }





        }
        else
        {
            tdMessage.InnerHtml = clsGeneral.warningMsg("Invalid Skill");
            hdFieldSucc.Value = "0";
        }

        if (oSession.IEPId != 0)
        {
            ViewInProgressIEP();
        }
        tdMsg.InnerHtml = "";


    }
    protected int ReturnNewVLessonId(int templateId)
    {
        objData = new clsData();
        DataClass oData = new DataClass();
        int studId = sess.StudentId;
        int newVisualLessonId = 0;
        string selctQuerry = "";
        string studentName = "";
        selctQuerry = "SELECT VTLessonId FROM DSTempHdr WHERE DSTempHdrId = " + templateId;
        object objVt = objData.FetchValue(selctQuerry);
        if (objVt != null)
        {

            if (objVt.ToString() != "")
            {
                string selctStudentName = "SELECT StudentLname + StudentFname As StudentName FROm Student WHERE StudentId = " + studId;
                DataTable dtNew = objData.ReturnDataTable(selctStudentName, false);
                if (dtNew.Rows.Count > 0)
                {
                    studentName = dtNew.Rows[0]["StudentName"].ToString();

                }
                int vtId = Convert.ToInt32(objVt);
                if (vtId > 0)
                {
                    try
                    {
                        int isStEdit = 1;
                        int isCcEdit = 0;
                        string selctSpQuerry = "sp_copyLessonPlan";     // Stored Procedure call for duplicate Lessonplan
                        //int newLessonId = oData.Execute_SpCopyLesson(selctSpQuerry, vtId, isStEdit, isCcEdit, studentName);
                        int newLessonId = oData.Execute_SpCopyLesson(selctSpQuerry, vtId, isStEdit, isCcEdit);

                        if (newLessonId > 0)
                        {
                            string selctLp = "SELECT MAX(LessonId) FROM LE_Lesson";
                            newVisualLessonId = Convert.ToInt32(objData.FetchValue(selctLp));
                        }

                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }
                }
            }
        }

        return newVisualLessonId;
    }


    [WebMethod]
    public static int GetInProgressIEPId()
    {
        clsData objdata = new clsData();
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        int IepId = Convert.ToInt32(objdata.FetchValue("Select StdtIEP_PEId FROM StdtIEP_PE WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='in progress') "));
        oSession.IEPId = IepId;
        return IepId;
    }
    public void checkIepEditCreateStatus(bool permission)
    {
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + "  ") == true)
        {
            pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEP_PEId DESC ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
            if (IepStatus.ToString() == "Approved")
            {
                txtSdate.Text = "";
                txtEdate.Text = "";
                //lblYear.Text = "IEP Year: " + objData.FetchValue("SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A' ").ToString();
                string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
            }

            else
            {


                //System.Data.DataTable dttmp = new System.Data.DataTable();
                ////   tdMsgMain.InnerHtml = clsGeneral.warningMsg("New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ");
                ////   ClientScript.RegisterStartupScript(GetType(), "", "<script type='text/javascript'>alert('New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ');</script>", true);
                //lnkBtnIEPYear.Visible = true;
                //dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEP_PEId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEP_PEId DESC ", true);
                //sess.IEPId = int.Parse(dttmp.Rows[0][0].ToString());
                //lnkBtnIEPYear.Text = dttmp.Rows[0][1].ToString();
                //hdnFieldIep.Value = dttmp.Rows[0][0].ToString();
                //sess.IEPStatus = int.Parse(dttmp.Rows[0][2].ToString());
                //if (permission == true)
                //{
                //    setSubmitAprove();
                //}
                //hdnFieldIep.Value = sess.IEPId.ToString();
                //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "CreateIEP1();", true);
                ClientScript.RegisterStartupScript(GetType(), "", "showMessage('New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ');", true);
                //ClientScript.RegisterStartupScript(GetType(), "", "showMessage('New IEP can not be created because there is one IEP in " + IepStatus.ToString() + " Status ');", true);


            }


        }
        else
        {
            string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        }




        //IEP Status select lookupname from LookUp where LookupId=
    }




    private bool validate()
    {
        objData = new clsData();
        bool result = true;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (txtSdate.Text == "")
        {
            result = false;
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select the IEP Start Date");
            return result;
        }
        else if (txtEdate.Text == "")
        {
            result = false;
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select the IEP End Date");
            return result;
        }
        else if (txtSdate.Text != "" && txtEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (dtst > dted)
            {
                result = false;
                tdMessage.InnerHtml = clsGeneral.warningMsg("IEP Start date is must before the IEP End date");
                return result;
            }
            //System.Data.DataTable dtdates = new System.Data.DataTable();
            //string selectDates = "Select Convert(date,AsmntYearStartDt) AS StartDate,Convert(date,AsmntYearEndDt) AS EndDate from AsmntYear Where AsmntYearId=(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A') ";//'" + Convert.ToInt32(ddlYear.SelectedValue) + "'";
            //dtdates = objData.ReturnDataTable(selectDates, false);
            //if (Convert.ToDateTime(txtSdate.Text) < Convert.ToDateTime(dtdates.Rows[0]["StartDate"]))
            //{
            //    result = false;
            //    tdMessage.InnerHtml = clsGeneral.warningMsg("Start Date is not in the Academic Year period");
            //    return result;
            //}
            //if (Convert.ToDateTime(txtEdate.Text) > Convert.ToDateTime(dtdates.Rows[0]["EndDate"]))
            //{
            //    result = false;
            //    tdMessage.InnerHtml = clsGeneral.warningMsg("End Date is not in the Academic Year period");
            //    return result;
            //}
        }

        //object pendstatus = null;
        //object IepStatus = null;

        //if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
        //{
        //    pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEP_PEId DESC ").ToString();

        //    if (int.Parse(pendstatus.ToString()) > 0)
        //    {
        //        IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
        //    }
        //    if (IepStatus.ToString() == "Approved")
        //    { }
        //}
        return result;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string IepUpdateStatus(string testVal)
    {
        clsData objDate = new clsData();
        System.Data.DataTable dt = new System.Data.DataTable();
        string returnVal = "";
        if (testVal != "")
        {

            dt = objDate.ReturnDataTable("select * from StdtIEP_PEUpdateStatus where stdtIEPId=" + int.Parse(testVal), true);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    returnVal = dt.Rows[0]["Page1"] + ":" + dt.Rows[0]["Page2"] + ":" + dt.Rows[0]["Page3"] + ":" + dt.Rows[0]["Page4"] + ":" + dt.Rows[0]["Page5"] + ":" + dt.Rows[0]["Page6"] + ":" + dt.Rows[0]["Page7"] + ":" + dt.Rows[0]["Page8"] + ":" + dt.Rows[0]["Page9"] + ":" + dt.Rows[0]["Page10"] + ":" + dt.Rows[0]["Page11"] + ":" + dt.Rows[0]["Page12"] + ":" + dt.Rows[0]["Page13"] + ":" + dt.Rows[0]["Page14"] + ":" + dt.Rows[0]["Page15"];
            }

        }
        return returnVal;

    }


    protected void lnkBtnSubmit_Click(object sender, EventArgs e)
    {
        // string iepid = "";
        DataClass oData = new DataClass();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        //   iepid = Convert.ToString(objData.FetchValue("Select StdtIEP_PEId from [dbo].[StdtIEP_PEExt1] Intersect Select StdtIEP_PEId from [dbo].[StdtIEP_PEExt2] Intersect Select StdtIEP_PEId from [dbo].[StdtIEP_PEExt3] Intersect Select StdtIEP_PEId from [dbo].[StdtLessonPlan] Intersect Select StdtIEP_PEId from [dbo].[StdtGoal] Intersect Select StdtIEP_PEId from [dbo].[StdtGoalSvc] where StdtIEP_PEId='" + sess.IEPId + "'"));
        //   if (iepid != "")
        //    {
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        string checkIEP = "Update StdtIEP_PE set StatusId=" + pendingApprove + " where StdtIEP_PEId=" + sess.IEPId + "";
        int result = oData.ExecuteNonQuery(checkIEP);

        string checkIEPNE = "Update StdtIEP set StatusId=" + pendingApprove + " where StdtIEPId=" + sess.IEPId + "";
        int resultNE = oData.ExecuteNonQuery(checkIEPNE);

        if (result > 0)
        {
            //  btnapprove.Visible = true;
            //btnApprove.Visible = true;
            // imgpending.Visible = false;
            //  checkStatus();*/
            //setSubmitAprove();
        }
        //if (lnkBtnAprove.Visible == true) setApprovePermission();
        //    }
        //else
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Complete IEP Document before Submitting");
        //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
        tdMsg.InnerHtml = clsGeneral.sucessMsg("IEP Document Successfully Submitted");
        hdnFieldIep.Value = "";
        //FillPendingIEP();
        LoadData();
        lnkBtnIEPYear.Text = "";
    }

    protected void lnkBtnAprove_Click(object sender, EventArgs e)
    {

        DataClass oData = new DataClass();
        int statusID;
        statusID = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        tdMsgMain.InnerHtml = null;
        txtReason.Text = string.Empty;
        btnAdd.Text = "Approve";
        FillRecentNotes();
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){$('#overlay').fadeIn('slow', function () { $('#DilogAprove').animate({ top: '5%' },{duration: 'slow',easing: 'linear'}) }); $('#close_x').click(function () {$('#dialog').animate({ top: '-300%' }, function () {$('#overlay').fadeOut('slow');});});});", true);
        int studentid = Convert.ToInt32(sess.StudentId);
        //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
        //ExportAll();

    }
    protected void Dlnotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Dlnotes.PageIndex = e.NewPageIndex;
            FillRecentNotes();
            ClientScript.RegisterStartupScript(GetType(), "", "showApprRej();", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillRecentNotes()
    {
        sess = (clsSession)Session["UserSession"];
        string notes = "SELECT ApprovalNotes,'IEP Version - '+Version AS Version from StdtIEP_PE where StudentId='" + sess.StudentId + "' ";
        System.Data.DataTable Dt = objDataClass.fillData(notes);
        _dtTest = new System.Data.DataTable();
        ClassDatatable oDt = new ClassDatatable();
        _dtTest = oDt.CreateColumn("RecentNote", _dtTest);
        _dtTest = oDt.CreateColumn("RecentDate", _dtTest);
        _dtTest = oDt.CreateColumn("Version", _dtTest);
        _dtTest = oDt.CreateColumn("Type", _dtTest);
        DataRow drnote = null;
        foreach (DataRow dr in Dt.Rows)
        {
            int i = 0;
            Approvalnotes = dr["ApprovalNotes"].ToString().Trim();
            string[] results = Approvalnotes.Split(new[] { "_&_" }, StringSplitOptions.None);
            for (int j = 0; j < (results.Count()) / 3; j++)
            {
                drnote = _dtTest.NewRow();
                drnote["RecentNote"] = results[i + 1];
                if (Convert.ToString(results[i + 2]).Contains("/") == true)
                {
                    drnote["RecentDate"] = Convert.ToString(results[i + 2]);
                }
                else
                {
                    string[] dte = results[i + 2].Replace(" ", "-").Split(new[] { "-" }, StringSplitOptions.None);
                    drnote["RecentDate"] = dte[1] + "/" + dte[0] + "/" + dte[2].ToString();
                }
                drnote["Version"] = dr["Version"];
                drnote["Type"] = results[i];
                _dtTest.Rows.Add(drnote);
                i = i + 3;
            }
        }
        Dlnotes.DataSource = _dtTest;
        Dlnotes.DataBind();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        tdMessage.InnerHtml = "";
        int RedirectFlag = 0;
        try
        {
            sess = (clsSession)Session["UserSession"];
            objData = new clsData();
            if (txtReason.Text == "") txtReason.Text = " ";
            sess = (clsSession)Session["UserSession"];
            string notes = "";
            string note = Convert.ToString(objData.FetchValue("SELECT ApprovalNotes FROM StdtIEP WHERE StdtIEPId='" + sess.IEPId + "'"));
            if (note == "")
            {
                notes = "Approve_&_" + txtReason.Text.Trim() + "_&_" + DateTime.Now.ToString();
            }
            else
            {
                notes = note + "_&_" + "Approve_&_" + txtReason.Text.Trim() + "_&_" + DateTime.Now.ToString();
            }
            string SQLQRY = "UPDATE StdtIEP_PE set ApprovalNotes='" + clsGeneral.convertQuotes(notes) + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEP_PEId='" + sess.IEPId + "'";
            int retVal = objData.Execute(SQLQRY);

            //string SQLQRYNE = "UPDATE StdtIEP set ApprovalNotes='" + clsGeneral.convertQuotes(notes) + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEPId='" + sess.IEPId + "'";
            //int retValNE = objData.Execute(SQLQRYNE);
            if (retVal > 0)
            {
                string selIEPstatus = "Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId+ " ";
                object IEPstatus = objData.FetchValue(selIEPstatus);
                sess.IEPStatus = int.Parse(IEPstatus.ToString());
                if (btnAdd.Text == "Approve")
                {
                    
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Approved Successfully");
                    hdnFieldIep.Value = "0";

                    lnkBtnIEPYear.Text = "";
                    txtEdate.Text = "";
                    txtSdate.Text = "";



                    RedirectFlag = 1;
                    byte[] contents;
                    string fileNameDoc = "";
                    ExportAll(out contents, out fileNameDoc);
                }

                FillProgressIEP();
                FillPendingIEP();
                FillApprovedIEP();
                FillRecentNotes();
            }
            else
            {
                tdMsgMain.InnerHtml = clsGeneral.failedMsg(btnAdd.Text + " Failed!!!");
                RedirectFlag = 0;
            }

        }
        catch (Exception ex)
        {
            objData.RollBackTransation();
            if (btnAdd.Text == "Approve")
                tdMsgMain.InnerHtml = clsGeneral.failedMsg("Approval Failed! <br> '" + ex.Message + "' ");
            else
                tdMsgMain.InnerHtml = clsGeneral.failedMsg("Rejection Failed! <br> '" + ex.Message + "' ");
            RedirectFlag = 0;
        }

        //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
    }

    private void FillApprovedIEP()
    {
        objData = new clsData();
        ul_ApprovedIEP.InnerHtml = "";
        string ApproveId = "";
        object Approved = objData.FetchValue("Select STUFF((SELECT ','+CONVERT(VARCHAR, LookupId) from (Select   LookupId  from LookUp where LookupType='IEP Status'  And (LookupName ='Approved' or LookupName='Expired') group by LookupId) LP FOR XML PATH('')),1,1,'')");
        if (Approved != null) ApproveId = Convert.ToString(Approved);

        string strQuery = "Select 'IEP '+yr.AsmntYearDesc +' Version - '+ iep.Version AS Name,iep.StdtIEP_PEId As ID from [dbo].[StdtIEP_PE] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + sess.StudentId + "  And iep.StatusId In (" + ApproveId + ")  Order By iep.StdtIEP_PEId Desc";

        System.Data.DataTable dt = objData.ReturnDataTable(strQuery, false);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ul_ApprovedIEP.InnerHtml += "<li style='position: static;' class='accordion'>" +
                                                    "<h2 class='BG' onclick='h2click(this," + row["ID"] + ");'><a id='" + row["ID"] + "' class='kk' href='#' >" + row["Name"] + "</a></h2>" +
                                                    "<div style='display: none;' class='wrapper nomar'>" +
                                                        "<div class='nobdrrcontainer'>";

                    for (int iepPage = 0; iepPage < 15; iepPage++)
                    {
                        string ieppageid = row["ID"] + "-Page" + iepPage;
                        string[] IEPContents = { "Individualized Education", "IEP Team/Signatures", "Procedural Safeguards Notice", "Special Considerations", "Present Levels Of Academic Achievement And Functional Performance", "Postsecondary Education and Training Goal", "Participation In State And Local Assessments", "Additional Information", "Local Assesments", "Goals And Objectives", "Special Considerations", "Gifted Support Service For a Student", "Educational Placement", "Educational Placement- II", "Penndata Reporting" };
                        string[] IEPFunctions = { "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE1.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE2.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE3.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE4.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE5.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE6.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE7.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE8.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE9.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE10.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE11.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE12.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE13.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE14.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP-PE15.aspx');" };
                        string iepcontent = (IEPContents[iepPage].ToString().Length > 30) ? (IEPContents[iepPage].Substring(0, 28) + "...") : IEPContents[iepPage].ToString();
                        ul_ApprovedIEP.InnerHtml += "<div class='grmb' onclick=" + IEPFunctions[iepPage] + "><a style='width:100px;' href='#' id='" + ieppageid + "'  >" +
                                   "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis;' title='" + IEPContents[iepPage] + "'>" + iepcontent +
                                   "</div></a></div>";
                    }
                    ul_ApprovedIEP.InnerHtml += "<div class='clear'></div>" +
                                         "</div>" +
                                         "<div class='clear'></div>" +
                                        "</div>" +
                                       "</li>";
                }
            }
        }
    }

    private void FillPendingIEP()
    {
        objData = new clsData();
        ul_PendingIEP.InnerHtml = "";
        if (objData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
        {
            object PendingApproveId = objData.FetchValue("Select LookupId  from LookUp where LookupType='IEP Status'  And LookupName ='Pending Approval' group by LookupId");

            string strQuery = "Select 'IEP '+yr.AsmntYearDesc +' Version - '+ iep.Version AS Name,iep.StdtIEP_PEId As ID from [dbo].[StdtIEP_PE] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + sess.StudentId + "  And iep.StatusId =" + PendingApproveId + "  Order By iep.StdtIEP_PEId Desc";

            System.Data.DataTable dt = objData.ReturnDataTable(strQuery, false);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ul_PendingIEP.InnerHtml += "<li style='position: static;' class='accordion'>" +
                                                        "<h2 class='BG' onclick='h2click(this," + row["ID"] + ");'><a id='" + row["ID"] + "' class='kk' href='#' >" + row["Name"] + "</a></h2>" +
                                                        "<div style='display: none;' class='wrapper nomar'>" +
                                                            "<div class='nobdrrcontainer'>";

                        for (int iepPage = 0; iepPage < 15; iepPage++)
                        {
                            string ieppageid = row["ID"] + "-Page" + iepPage;
                            string[] IEPContents = { "Individualized Education", "IEP Team/Signatures", "Procedural Safeguards Notice", "Special Considerations", "Present Levels Of Academic Achievement And Functional Performance", "Postsecondary Education and Training Goal", "Participation In State And Local Assessments", "Additional Information", "Local Assesments", "Goals And Objectives", "Special Considerations", "Gifted Support Service For a Student", "Educational Placement", "Educational Placement- II", "Penndata Reporting" };
                            string[] IEPFunctions = { "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE1.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE2.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE3.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE4.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE5.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE6.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE7.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE8.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE9.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE10.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE11.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE12.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE13.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE14.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP-PE15.aspx');" };
                            string iepcontent = (IEPContents[iepPage].ToString().Length > 30) ? (IEPContents[iepPage].Substring(0, 28) + "...") : IEPContents[iepPage].ToString();
                            ul_PendingIEP.InnerHtml += "<div class='grmb' onclick=" + IEPFunctions[iepPage] + "><a style='width:100px;' href='#' id='" + ieppageid + "'  >" +
                                       "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis;' title='" + IEPContents[iepPage] + "'>" + iepcontent +
                                       "</div></a></div>";
                        }
                        ul_PendingIEP.InnerHtml += "<div class='clear'></div>" +
                                             "</div>" +
                                             "<div class='clear'></div>" +
                                            "</div>" +
                                           "</li>";
                    }




                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    PendingIEP.InnerHtml+=" <div class='accord-header' onclick='h2click(this," + row["ID"] + ");'>" + row["Name"] + "</div>";
                    //    for (int iepPage = 0; iepPage < 8; iepPage++)
                    //    {
                    //        string ieppageid = row["ID"] + "-Page" + iepPage;
                    //        string[] IEPContents = { "Individualized Education", "Present Levels Of Educational", "Present Levels Of Educational", "Current Performance", "Service Delivery", "Non Participation justification", "State Of District-Wide", "Additional Information" };
                    //        string[] IEPFunctions = { "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP1.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP2.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP3.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP4.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP5.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP6.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP7.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP8.aspx');" };
                    //        PendingIEP.InnerHtml += "<div class='accord-content' onclick=" + IEPFunctions[iepPage] + " title='" + IEPContents[iepPage] + "' id='" + ieppageid + "'>" + IEPContents[iepPage] +"</div>";

                    //    }

                    //}
                }
            }
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            string notes = "";
            if (txtReason.Text == "") txtReason.Text = " ";
            string note = Convert.ToString(objData.FetchValue("SELECT ApprovalNotes FROM StdtIEP_PE WHERE StdtIEP_PEId='" + sess.IEPId + "'"));
            if (note == "")
            {
                notes = "Reject_&_" + txtReason.Text.Trim() + "_&_" + DateTime.Now.ToString();
            }
            else
            {
                notes = note + "_&_" + "Reject_&_" + txtReason.Text.Trim() + "_&_" + DateTime.Now.ToString();
            }
            string SQLQRY = "UPDATE StdtIEP_PE set ApprovalNotes='" + notes + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEP_PEId='" + sess.IEPId + "'";
            int retVal = objData.Execute(SQLQRY);
            FillRecentNotes();

            if (retVal > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Rejected ...");
                LoadData();
            }

            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg(btnAdd.Text + " Failed!!!");

            }

        }
        catch (Exception ex)
        {
            objData.RollBackTransation();
            throw ex;

        }
        ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
    }
    protected void btnIEPExport_Click(object sender, EventArgs e)
    {

        btnIEPExport.Enabled = false;
        btnIEPExport.Attributes.Add("cssClass", "ExportWordDis");
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        tdMsgMain.InnerHtml = "";
        clsDocumentasBinary objBinary = new clsDocumentasBinary();

        try
        {

            byte[] contents;
            string fileNameDoc = "";
            object chkappr = objData.FetchValue("select count(*) from StdtIEP_PE where StatusId = (select LookupId from Lookup where LookupType = 'IEP Status' and LookupName = 'Approved') and StdtIEP_PEId = " + sess.IEPId + "");
            if (Convert.ToInt32(chkappr) == 0)
            {
                ExportAll(out contents, out fileNameDoc);
                string contentType2 = "application/msword";
                objBinary.ShowDocument(fileNameDoc, contents, contentType2);
            }
            else
            {

                string fileName = "", contentType = "";                
                string strQuery = "Select Data,ContentType,DocumentName from binaryFiles Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And IEPId=" + sess.IEPId + " And Type='BW'  And ModuleName='IEP'";
                byte[] bytes = objBinary.getDocument(strQuery, out contentType, out fileName);
                objBinary.ShowDocument(fileName, bytes, contentType);
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            btnIEPExport.Enabled = true;
            btnIEPExport.Attributes.Add("cssClass", "ExportWord");
        }

    }




    //******************************************************************************************************
    protected System.Data.DataTable getGoalData4()
    {
        //Page Number 4 
        System.Data.DataTable Dt = null;
        objData = new clsData();

        Dt = new System.Data.DataTable();
        string strQuery = "select distinct  Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEP_PEId=" + sess.IEPId + " and StdtLessonPlan.IncludeIEP=1";
        Dt = objData.ReturnDataTable(strQuery, false);
        string GoalIdZ = "";
        int countForGoalId = 0;
        foreach (DataRow dr in Dt.Rows)
        {
            if (countForGoalId == 0)
            {
                GoalIdZ += dr["Id"].ToString();
            }
            else if (countForGoalId > 0)
            {
                GoalIdZ += "," + dr["Id"].ToString();
            }
            countForGoalId++;
        }

        if (Dt.Rows.Count == 0)
        {
            GoalIdZ = "0";
        }

        strQuery = "SELECT    dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName,(SELECT StdtGoalId FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEP_PEId=" + sess.IEPId + ") StdtGoalId, dbo.StdtLessonPlan.GoalId,StdtIEP_PE.AsmntYearId,(SELECT IEPGoalNo FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEP_PEId=" + sess.IEPId + ") IEPGoalNo,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, dbo.Goal.GoalName   FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join dbo.StdtIEP_PE ON dbo.StdtLessonPlan.StdtIEP_PEId=dbo.StdtIEP_PE.StdtIEP_PEId where StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEP_PEId =  '" + sess.IEPId + "'    ORDER BY dbo.StdtLessonPlan.GoalId ";


        Dt = objData.ReturnDataTable(strQuery, false);


        System.Data.DataTable dtRep = Dt;
        dtRep.Columns.Remove("StudentId");
        dtRep.Columns.Remove("StdtGoalId");
        dtRep.Columns.Remove("AsmntYearId");
        //  dtRep.Columns.Remove("GoalId");
        return dtRep;
    }




    [STAThread]
    public static void ConvertHTMLTOWORD(string url)
    {

        var file = new FileInfo(url);
        Microsoft.Office.Interop.Word.Application app
            = new Microsoft.Office.Interop.Word.Application();
        try
        {
            app.Visible = true;
            object missing = Missing.Value;
            object visible = true;
            _Document doc = app.Documents.Add(ref missing,
                     ref missing,
                     ref missing,
                     ref visible);
            var bookMark = doc.Words.First.Bookmarks.Add("entry");
            bookMark.Range.InsertFile(file.FullName);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            app.Quit();
        }
    }


    private string CopyTemplate(string oldPath, string PageNo)
    {
        try
        {
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\Administration") + "\\IEPTemp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid g;

            g = Guid.NewGuid();

            sess = (clsSession)Session["UserSession"];
            string newpath = path + "\\";
            string ids = g.ToString();
            ids = ids.Replace("-", "");

            string newFileName = "IEPTemporyTemplate" + ids.ToString();
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
            }
            return newpath + newFileName + f1.Extension;
        }
        catch (Exception Ex)
        {
            tdMsgExport.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
        }
    }

    static int xmlcolumn = 0;


    private void ExportToWord(string htmlData, string newPath)
    {
        try
        {
            StringBuilder strBody = new StringBuilder();
            strBody.Append(htmlData);
            Response.AppendHeader("Content-Type", "application/msword");
            Response.AppendHeader("Content-disposition", "attachment; filename=IEPDocument.doc");
            Response.Write(strBody);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void replaceWithTexts(MainDocumentPart mainPart, string[] plcT, string[] TextT)
    {
        TimeSpan tempDatetime;
        int count = plcT.Count();
        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
        for (int i = 0; i < count; i++)
        {


            string textData = "";

            if (TextT[i] != null)
            {
                //TextT[i] = TextT[i].Replace("##", "'");
                //TextT[i] = TextT[i].Replace("?bs;", "\\");
                if (plcT[i] == "plcDOB")
                {
                    DateTime Datetime = Convert.ToDateTime(TextT[i]);

                    TextT[i] = Datetime.ToShortDateString();
                }
                textData = TextT[i];
            }
            else
            {
                if (plcT[i] == "plcAge")
                {
                    tempDatetime = DateTime.Now - Convert.ToDateTime(TextT[2].ToString());
                    double dats = tempDatetime.TotalDays;
                    int age = Convert.ToInt32(dats / 360);
                    textData = age.ToString();
                }

            }

            var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);

            string textDataNoSpace = textData.Replace(" ", "");
            foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
            {
                var paragraphs = converter.Parse(textData);
                if (paragraphs.Count == 0)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                    para.Parent.Append(tempPara);
                    para.Remove();
                }
                else
                {
                    for (int k = 0; k < paragraphs.Count; k++)
                    {
                        bool isBullet = false;
                        if (textDataNoSpace.Contains("<li>" + paragraphs[k].InnerText.Trim()))
                            isBullet = true;
                        if (isBullet)
                        {
                            ParagraphProperties paraProp = new ParagraphProperties();
                            ParagraphStyleId paraStyleid = new ParagraphStyleId() { Val = "BulletPara" };
                            NumberingProperties numProp = new NumberingProperties();
                            NumberingLevelReference numLvlRef = new NumberingLevelReference() { Val = 0 };
                            NumberingId numID = new NumberingId() { Val = 1 };
                            numProp.Append(numLvlRef);
                            numProp.Append(numID);
                            paraProp.Append(paraStyleid);
                            paraProp.Append(numProp);

                            if (((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties != null)
                            {
                                //Assign Bullet point property to paragraph
                                ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties.Append(paraProp);
                            }
                        }
                        para.Parent.Append(paragraphs[k]);
                    }
                    para.Remove();
                }
                //para.RemoveAllChildren<Run>();
            }
            //paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);
            //foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
            //{
            //    para.RemoveAllChildren<Run>();
            //}
            
        }

    }

    private string replaceWithTextsOLD(string HtmlData, string[] plcT, string[] TextT)
    {
        TimeSpan tempDatetime;
        int count = plcT.Count();

        for (int i = 0; i < count; i++)
        {
            if (TextT[i] != null)
            {
                TextT[i] = TextT[i].Replace("##", "'");
                TextT[i] = TextT[i].Replace("?bs;", "\\");
                if (plcT[i] == "plcDOB")
                {
                    DateTime Datetime = Convert.ToDateTime(TextT[i]);

                    TextT[i] = Datetime.ToShortDateString();
                }
                HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
            }
            else
            {
                if (plcT[i] == "plcAge")
                {
                    tempDatetime = DateTime.Now - Convert.ToDateTime(TextT[2].ToString());
                    double dats = tempDatetime.TotalDays;
                    int age = Convert.ToInt32(dats / 360);
                    HtmlData = HtmlData.Replace(plcT[i], age.ToString());
                }
                HtmlData = HtmlData.Replace(plcT[i], "");
            }
        }
        return HtmlData;
    }

    public static void PageConvert(string input, string output, WdSaveFormat format)
    {
        try
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object bConfirmDialog = false;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = format;
            object oFileShare = true;

            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref bConfirmDialog, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();

            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

            using (var fs = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Close();
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }




    }

    public static String URLTOHTML(string Url)
    {
        string result = "";
        try
        {

            using (StreamReader reader = new StreamReader(Url))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }
        return result;

    }

    public void ExportAllOld()
    {

        Hashtable ht = new Hashtable();
        clsData objData = new clsData();
        System.Data.DataTable Dt = new System.Data.DataTable();

        clsDocumentasBinary objBinary = new clsDocumentasBinary();
        //ht = bindData();
        string[] plcT, TextT, plcC, chkC;

        string[] totChkBox = new string[68];
        string Path = Server.MapPath("~\\Administration\\IEPTemplates\\PA IEP Template.docx");

        string TemporyPath = Server.MapPath("~\\Administration\\IEPTemp\\");
        string NewPath = CopyTemplate(Path, "0");
        string strQuery = "Select  ST.StudentNbr AS IDNO, ST.StudentLname+','+ST.StudentFname as Name FROM  Student ST Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + "";
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                string colName = Dt.Rows[0]["Name"].ToString();
                using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
                {
                    MainDocumentPart mainPart = theDoc.MainDocumentPart;
                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "plcNameStud").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(colName))));
                            break;
                        }
                    }
                    mainPart.Document.Save();
                }
            }
        }
        System.Threading.Thread.Sleep(3000);
        int x = 0, count = 0, lastCount = 0;

        for (int k = 1; k < 17; k++)
        {
            //  k = 13;
            if (k != 9)
            {
                //  count = chkC.Length;
                CreateQuery1("NE", "..\\Administration\\XMlIEP_PA\\IEP_PA" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                chkC.CopyTo(totChkBox, count);
                count += lastCount;

            }
        }
        setCheckBox(NewPath, ht, totChkBox);
        Guid g = Guid.NewGuid();

        sess = (clsSession)Session["UserSession"];
        string ids = g.ToString();

        Thread thread = new Thread(new ThreadStart(WorkThreadFunction));

        string hPath = Server.MapPath("~\\Administration") + "\\IEPTemp\\HTML" + ids + ".html";
        PageConvert(NewPath, hPath, WdSaveFormat.wdFormatHTML);
        System.Threading.Thread.Sleep(3000);



        thread.Abort();

        string HtmlFileName = "";
        string HtmlData = objBinary.ConvertToHtml(NewPath, TemporyPath, out HtmlFileName);

        string Temp = Server.MapPath("~\\Administration") + "\\XMlIEP_PA\\";

        string[] filePaths = Directory.GetFiles(Temp);
        string[] totCkeckColums = new string[0];




        for (int k = 1; k < 17; k++)
        {
            if (k != 9)
            {
                CreateQuery1("NE", "..\\Administration\\XMlIEP_PA\\IEP_PA" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                HtmlData = replaceWithTextsOLD(HtmlData, plcT, TextT);
            }
        }


        string strquery = "";
        string fileName = "";
        objData = new clsData();
        strquery = "Select 'IEP Doc :'+ S.StudentLname +'-'+AY.AsmntYearDesc+'-V'+IE.Version from StdtIEP_PE IE Inner Join AsmntYear AY on IE.AsmntYearId=AY.AsmntYearId INNER JOIN Student S ON IE.StudentId =S.StudentId  Where IE.StdtIEP_PEId=" + sess.IEPId + " And IE.StudentId=" + sess.StudentId + " And IE.SchoolId=" + sess.SchoolId + " ";
        fileName = Convert.ToString(objData.FetchValue(strquery));
        fileName = fileName + ".doc";

        byte[] contents = objBinary.ConvertToByte(HtmlData, TemporyPath, HtmlFileName);
        System.Threading.Thread.Sleep(3000);
        strquery = "select version from stdtiep where  schoolid=" + sess.SchoolId + " and studentid=" + sess.StudentId + " and stdtiepid=" + sess.IEPId + " ";
        object ver = objData.FetchValue(strquery);
        string ver1 = "";
        if (ver != null) ver1 = Convert.ToString(ver);
        if (ver1 == "") ver1 = "0";
        strquery = "select lookupid from LookUp where LookupName='IEP' ";
        object lookupId = objData.FetchValue(strquery);
        int lookupId1 = Convert.ToInt32(lookupId);
        int docId = objBinary.saveDoc(sess.SchoolId, sess.StudentId, sess.LoginId, lookupId1);
        int binaryid = objBinary.saveDocument(contents, fileName, ver1, "BW", docId, sess.IEPId, "IEP", sess.SchoolId, sess.StudentId, sess.LoginId);
        //objBinary.ShowDocument(fileName, contents, "application/msword");


        FileInfo f1 = new FileInfo(NewPath);

        if (f1.Exists)
        {
            f1.Delete();
        }

        f1 = new FileInfo(hPath);
        if (f1.Exists)
        {
            f1.Delete();
        }
        string path = Server.MapPath("~\\Administration") + "\\IEPTemp";
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(path);
        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
        {
            dir.Delete(true);
        }


    }
    private void CleanDoc(string DocName)
    {
        using (WordprocessingDocument doc =
            WordprocessingDocument.Open(DocName, true))
        {
            SimplifyMarkupSettings settings = new SimplifyMarkupSettings
            {
                RemoveComments = true,
                RemoveContentControls = true,
                RemoveEndAndFootNotes = true,
                RemoveFieldCodes = false,
                RemoveLastRenderedPageBreak = true,
                RemovePermissions = true,
                RemoveProof = true,
                RemoveRsidInfo = true,
                RemoveSmartTags = true,
                RemoveSoftHyphens = true,
                ReplaceTabsWithSpaces = true,
            };
            MarkupSimplifier.SimplifyMarkup(doc, settings);
        }
    }
    public void ExportAll(out byte[] contents, out string fileNameDoc)
    {
        Hashtable ht = new Hashtable();
        clsDocumentasBinary objBinary = new clsDocumentasBinary();
        clsData objData = new clsData();
        
        ht = bindData();
        string[] plcT, TextT, plcC, chkC;
        System.Data.DataTable Dt = new System.Data.DataTable();

        string[] totChkBox = new string[68];
        string Path = Server.MapPath("~\\Administration\\IEPTemplates\\PA IEP Template.docx");
        string TemporyPath = Server.MapPath("~\\Administration\\IEPTemp\\");

        string NewPath = CopyTemplate(Path, "0");

        string strQuery = "Select  ST.StudentNbr AS IDNO, ST.StudentLname+','+ST.StudentFname as Name FROM  Student ST Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + "";
        Dt = objData.ReturnDataTable(strQuery, false);

        //Simplify the Markup Code Here...

        CleanDoc(NewPath);



        ///
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                string colName = Dt.Rows[0]["Name"].ToString();
                using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
                {
                    MainDocumentPart mainPart = theDoc.MainDocumentPart;
                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "plcNameStud").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(colName))));
                            break;
                        }
                    }
                    mainPart.Document.Save();
                }
            }
        }
        //System.Threading.Thread.Sleep(3000);
        int x = 0, count = 0, lastCount = 0;

        for (int k = 1; k < 17; k++)
        {
            //  k = 13;
            if (k != 9)
            {
                //  count = chkC.Length;
                CreateQuery1("NE", "..\\Administration\\XMlIEP_PA\\IEP_PA" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                chkC.CopyTo(totChkBox, count);
                count += lastCount;

            }
        }
        setCheckBox(NewPath, ht, totChkBox);
        //Guid g = Guid.NewGuid();

        sess = (clsSession)Session["UserSession"];
        //string ids = g.ToString();

        //Thread thread = new Thread(new ThreadStart(WorkThreadFunction));

        //string hPath = Server.MapPath("~\\Administration") + "\\IEPTemp\\HTML" + ids + ".html";
        //PageConvert(NewPath, hPath, WdSaveFormat.wdFormatHTML);
        //System.Threading.Thread.Sleep(3000);



        //thread.Abort();

        //string HtmlFileName = "";
        //string HtmlData = objBinary.ConvertToHtml(NewPath, TemporyPath, out HtmlFileName);

        string Temp = Server.MapPath("~\\Administration") + "\\XMlIEP_PA\\";

        string[] filePaths = Directory.GetFiles(Temp);
        string[] totCkeckColums = new string[0];


        using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
        {

            for (int k = 1; k < 17; k++)
            {
                if (k != 9)
                {
                    CreateQuery1("NE", "..\\Administration\\XMlIEP_PA\\IEP_PA" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                    replaceWithTexts(theDoc.MainDocumentPart, plcT, TextT);
                }
            }
            //theDoc.MainDocumentPart.Document.Save();


        }

        string strquery = "";
        string fileName = "";
        objData = new clsData();
        strquery = "Select 'IEP Doc :'+ S.StudentLname +'-'+AY.AsmntYearDesc+'-V'+IE.Version from StdtIEP_PE IE Inner Join AsmntYear AY on IE.AsmntYearId=AY.AsmntYearId INNER JOIN Student S ON IE.StudentId =S.StudentId  Where IE.StdtIEP_PEId=" + sess.IEPId + " And IE.StudentId=" + sess.StudentId + " And IE.SchoolId=" + sess.SchoolId + " ";
        fileName = Convert.ToString(objData.FetchValue(strquery));
        fileNameDoc = fileName + ".doc";

        //byte[] contents = objBinary.ConvertToByte(HtmlData, TemporyPath, HtmlFileName);
        //
        strquery = "select version from stdtiep where  schoolid=" + sess.SchoolId + " and studentid=" + sess.StudentId + " and stdtiepid=" + sess.IEPId + " ";
        object ver = objData.FetchValue(strquery);
        string ver1 = "";
        if (ver != null) ver1 = Convert.ToString(ver);
        if (ver1 == "") ver1 = "0";
        contents = System.IO.File.ReadAllBytes(NewPath);
        System.Threading.Thread.Sleep(3000);
        strquery = "select lookupid from LookUp where LookupName='IEP' ";
        object lookupId = objData.FetchValue(strquery);
        int lookupId1 = Convert.ToInt32(lookupId);
        int docId = objBinary.saveDoc(sess.SchoolId, sess.StudentId, sess.LoginId, lookupId1);
        object chkapprov = objData.FetchValue("select count(*) from StdtIEP_PE where StatusId = (select LookupId from Lookup where LookupType = 'IEP Status' and LookupName = 'Approved') and StdtIEP_PEId = " + sess.IEPId + "");
        if (Convert.ToInt32(chkapprov) == 1)
        {
            int binaryid = objBinary.saveDocument(contents, fileName, ver1, "BW", docId, sess.IEPId, "IEP", sess.SchoolId, sess.StudentId, sess.LoginId);
        }
        //objBinary.ShowDocument(fileName, contents, "application/msword");


        FileInfo f1 = new FileInfo(NewPath);

        if (f1.Exists)
        {
            f1.Delete();
        }

        //f1 = new FileInfo(hPath);
        //if (f1.Exists)
        //{
        //    f1.Delete();
        //}
        string path = Server.MapPath("~\\Administration") + "\\IEPTemp";
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(path);
        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
        {
            dir.Delete(true);
        }


    }

    public void WorkThreadFunction()
    {
        try
        {

            Thread.Sleep(5000);
        }
        catch (Exception ex)
        {
            // log errors
        }
    }

    private Hashtable bindData()
    {
        Hashtable htColumns = new Hashtable();
        sess = (clsSession)Session["UserSession"];
        clsExport objExport = new clsExport();
        string[] data;
        string[] data2;

        objExport.getIEP1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(1))
        {
            htColumns.Add(1, data2);
            data2 = null;
        }
        objExport.getIEP2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(2))
        {
            htColumns.Add(2, data2);
            data2 = null;
        }
        objExport.getIEP3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (!htColumns.ContainsKey(3))
        {
            htColumns.Add(3, data2);
            data2 = null;
        }

        System.Data.DataTable dtIEP4 = new System.Data.DataTable();
        bool Odd = false;
        dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);
        int RowsCount = dtIEP4.Rows.Count;
        dtIEP4.TableName = "Table";

        for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
        {
            System.Data.DataTable Dt4 = new System.Data.DataTable();
            Dt4 = objExport.ReturnRows(Round, dtIEP4);
            objExport.getIEP4(out data, Dt4);
        }
        if (!htColumns.ContainsKey(4))
        {
            htColumns.Add(4, data2);
            data2 = null;
        }
        objExport.getIEP5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(5))
        {
            htColumns.Add(5, data2);
            data2 = null;
        }

        objExport.getIEP6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (!htColumns.ContainsKey(6))
        {
            htColumns.Add(6, data2);
            data2 = null;
        }

        objExport.getIEP7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (!htColumns.ContainsKey(7))
        {
            htColumns.Add(7, data2);
            data2 = null;
        }
        objExport.getIEP8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(8))
        {
            htColumns.Add(8, data2);
            data2 = null;
        }
        return htColumns;
    }

    private string[] getCheckBoxes(string StateName, string Path, int PageNo, string[] chkData)
    {
        string[] chkC = new string[0];

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");

        checkCount = 0;

        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                int chkCount = 0;

                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        chkCount++;
                    }
                }

                chkC = new string[chkCount];

                columns = getColumns(PageNo, chkCount + 1);

                int j = 0, l = 0;


                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        checkCount++;

                        chkC[j] = columns[l];
                        j++;
                        //l++;
                    }
                    else
                    {
                        j++;
                        //l++;
                    }
                }
            }
        }
        Array.Copy(chkC, chkData, chkC.Length);
        //return chkData.Join<Array>(chkC[j], chkData);

        return chkData;
    }

    private void CreateQuery1(string StateName, string Path, int PageNo, out string[] plcT, out string[] TextT, out string[] plcC, out string[] chkC, bool check, out int lastValue)
    {



        lastValue = 0;
        chkC = new string[0];
        plcC = new string[0];

        TextT = new string[0];
        plcT = new string[0];

        //  if (PageNo == 4) return;
        string[] textValues;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");
        checkCount = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                int chkCount = 0, textCount = 0;

                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        chkCount++;
                    }
                    else
                    {
                        textCount++;
                    }
                }



                chkC = new string[chkCount];
                plcC = new string[chkCount];

                TextT = new string[textCount];
                plcT = new string[textCount];

                columns = getColumns(PageNo, textCount);


                int j = 0, k = 0, l = 0;

                if (check == true)
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                            chkC[j] = columns[l];
                            plcC[j] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            j++;
                            lastValue++;
                        }
                        else
                        {
                            TextT[k] = columns[l];
                            plcT[k] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            k++;
                        }
                        l++;
                    }
                }
                else
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                            chkC[j] = columns[l];
                            plcC[j] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            j++;
                        }
                        else
                        {
                            j++;
                        }


                    }
                }
                columns = null;
            }
        }

    }

    private string[] getColumns(int PageNo, int Count)
    {
        sess = (clsSession)Session["UserSession"];
        clsExport objExport = new clsExport();
        string[] data = new string[Count];
        string[] data2 = new string[2];
        string temp = "";
        int counter = 0;
        if (PageNo == 1) objExport.getIEP_PE1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 2) objExport.getIEP_PE2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 3) objExport.getIEP_PE3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 4) objExport.getIEP_PE4(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        //if (PageNo == 4)
        //{
        //    System.Data.DataTable dtIEP4 = new System.Data.DataTable();
        //    bool Odd = false;

        //    dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);
        //    int RowsCount = dtIEP4.Rows.Count;
        //    dtIEP4.TableName = "Table";

        //    for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
        //    {

        //        System.Data.DataTable Dt4 = new System.Data.DataTable();
        //        Dt4 = objExport.ReturnRows(Round, dtIEP4);
        //        objExport.getIEP4(out data, Dt4);


        //    }

        //}

        if (PageNo == 5) objExport.getIEP_PE5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 6) objExport.getIEP_PE6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (PageNo == 7) objExport.getIEP_PE7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 8) objExport.getIEP_PE8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        //if (PageNo == 9) objExport.getIEP_PE9(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 10) objExport.getIEP_PE10(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (PageNo == 11) objExport.getIEP_PE11(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 12) objExport.getIEP_PE12(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 13) objExport.getIEP_PE13(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 14) objExport.getIEP_PE14(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 15) objExport.getIEP_PE15(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 16) objExport.getIEP_PE16(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        return data;
    }

    public void SearchAndReplace(string document)
    {
        int m = 0;


        string col = "";
        string plc = "";

        columnsCheck = new string[checkCount];

        for (int i = 0; i < columns.Length; i++)
        {
            plc = placeHolders[i].ToString().Trim();
            col = columns[i].ToString().Trim();


            if (plc == "abcdefgh")
            {
                columnsCheck[m] = col;
                m++;
            }
            else
            {

                if (col == null) col = "";

                replaceWithHtml(document, plc, col);
            }
        }
        if (document.Contains("IEP1") == false)
        {
            if (document.Contains("IEP5") == false)
            {
                if (document.Contains("IEP8") == false)
                {

                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
                    {
                        MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
                        Body body = mainPart.Document.Body;

                        DocumentFormat.OpenXml.Wordprocessing.Paragraph para = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run((new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = DocumentFormat.OpenXml.Wordprocessing.BreakValues.Page })));


                        mainPart.Document.Body.InsertAfter(para, mainPart.Document.Body.LastChild);
                        mainPart.Document.Save();
                    }
                }
            }
        }



    }

    private void setCheckBox(string document, Hashtable ht, string[] columnsChks)
    {

        bool IsCheck = false;
        int i = 0;

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        {

            foreach (DocumentFormat.OpenXml.Wordprocessing.CheckBox cb in wordDoc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.CheckBox>())
            {
                if (cb != null)
                {

                    FormFieldName cbName = cb.Parent.ChildElements.First<FormFieldName>();

                    try
                    {
                        DefaultCheckBoxFormFieldState defaultState = cb.GetFirstChild<DefaultCheckBoxFormFieldState>();
                        if (i < columnsChks.Length)
                        {
                            if (columnsChks[i] != "") IsCheck = Convert.ToBoolean(columnsChks[i]);
                            if (IsCheck == true) defaultState.Val = true;
                        }
                        i++;
                    }
                    catch
                    {

                    }
                }
            }
        }
    }

    public void replaceWithHtml(string fileName, string replace, string replaceTest)
    {

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            ParagraphProperties paragraphProperties = new ParagraphProperties();

            DocumentFormat.OpenXml.Wordprocessing.Paragraph par = wordDoc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0); //hardcoded a paragraph index for testing

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();
            //      paragraphProperties.TextAlignment.

            if (replaceTest == "") replaceTest = "&nbsp;";
            if (replaceTest != "") replaceTest = replaceTest.Trim();
            try
            {

                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();
                string[] ParaList = replaceTest.Split(new string[] { "\n\n" }, StringSplitOptions.None);
                string orginal = "";
                IList<OpenXmlCompositeElement> element = new List<OpenXmlCompositeElement>();
                foreach (var item in ParaList)
                {
                    var paragraphs = converter.Parse(item);
                    Run[] runArr = par.Descendants<Run>().ToArray();

                    // foreach each run

                    foreach (var eachel in paragraphs)
                    {
                        //if (eachel.InnerXml.Contains("table"))
                        //{
                        //}

                        element.Add(eachel);
                    }
                }
                bool flag = false;

                string txt = "";
                char vt = (char)13;

                //         OpenXmlElement elm;
                //     Microsoft.Office.Interop.Word.Paragraph para;
                string modifiedString = "";
                var parent = placeholder.Parent;
                for (int i = 0; i < element.Count; i++)
                {
                    if (element.Count == 1)
                    {
                        parent.ReplaceChild(element[i], placeholder);
                    }
                    else if (element[i].InnerText.Trim() != "")
                    {
                        Run text = (Run)placeholder.Descendants();
                        //foreach (Run run in text)
                        //{
                        string innerText = text.InnerText;
                        modifiedString = text.InnerText.Replace(innerText, "My New Text");
                        // if the InnerText doesn't modify
                        if (modifiedString != text.InnerText)
                        {
                            Text t = new Text(modifiedString);
                            text.RemoveAllChildren<Text>();
                            text.AppendChild<Text>(t);
                        }
                        // }
                        txt = txt + element[i].InnerText + vt;

                        //     elm.InnerText.Concat(elm.InnerText, element[i].InnerText);
                    }
                    // element[i].Append(paragraphProperties);

                }


                // get all runs under the paragraph and convert to be array






                if (element.Count > 1)
                {

                    // element[0].InnerText = txt;
                    placeholder.Text = txt;
                    flag = true;

                    //parent.ReplaceChild(element[i], placeholder);
                }
                //if (!flag)
                //{
                //    parent.ReplaceChild(element[i], placeholder);
                //}
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                tdMsgExport.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
            }
            /**/


            //  byte[] byteArray = File.ReadAllBytes(DocxFilePath);


            /**/



        }
    }




    //******************************************************************************************************

    [WebMethod]
    public static void SetIEPId(int ID)
    {

        clsData objData = new clsData();
        clsSession sess = null;
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        if (sess == null) return;
        sess.IEPId = ID;

    }



    public void downloadfile()
    {
        try
        {

            string FileName = ViewState["FileName"].ToString();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");
            byte[] data = req.DownloadData(FileName);
            response.BinaryWrite(data);
            ClientScript.RegisterStartupScript(GetType(), "", "HideWait();", true);
            response.End();


        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('Submission not possible because another user made some changes in this Assessment');", true);
            //ClientScript.RegisterStartupScript(GetType(), "", "alert('sd');", true);
        }
        ViewState["FileName"] = "";
    }



    private void CreateQuery(string StateName, string Path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");
        checkCount = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                columns = new string[xmlListColumns.Count];
                placeHolders = new string[xmlListColumns.Count];




                int i = 0, j = 0;
                foreach (XmlNode stMs in xmlListColumns)
                {
                    columns[i] = stMs.Attributes["Column"].Value;
                    i++;
                }
                foreach (XmlNode stMs in xmlListColumns)
                {
                    placeHolders[j] = stMs.Attributes["PlaceHolder"].Value;

                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        checkCount++;
                    }
                    j++;
                }

            }
        }

    }

    private void setCheckBox(string document)
    {
        bool IsCheck = false;
        int i = 0;
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        {

            foreach (DocumentFormat.OpenXml.Wordprocessing.CheckBox cb in wordDoc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.CheckBox>())
            {
                if (cb != null)
                {

                    FormFieldName cbName = cb.Parent.ChildElements.First<FormFieldName>();

                    try
                    {
                        DefaultCheckBoxFormFieldState defaultState = cb.GetFirstChild<DefaultCheckBoxFormFieldState>();
                        if (i < checkCount)
                        {
                            IsCheck = Convert.ToBoolean(columnsCheck[i]);
                            if (IsCheck == true) defaultState.Val = true;
                        }
                        i++;
                    }
                    catch
                    {

                    }
                }
            }
        }
    }


    public byte[] generateWordDocument(string HtmlString)
    {
        byte[] bytes = null;

        try
        {
            clsDocumentasBinary objBinary = new clsDocumentasBinary();

            Guid g = Guid.NewGuid();

            string fileName = "Template.docx";
            string filePath = Server.MapPath("~\\Administration") + "\\IEPTemp\\" + fileName;
            string copyPath = Server.MapPath("~\\Administration") + "\\IEPTemp\\IEPDocBlank.docx";
            File.Copy(copyPath, filePath);

            string pageTitle = HtmlString;

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                int altChunkIdCounter = 1;
                int blockLevelCounter = 1;

                string mainhtml = HtmlString;// "<html><head><style type='text/css'>.catalogGeneralTable{border-collapse: collapse;text-align: left;} .catalogGeneralTable td, th{ padding: 5px; border: 1px solid #999999; }</style></head><body style='font-family:Trebuchet MS;font-size:.9em;'>" + HtmlString + "</body></html>";
                string altChunkId = String.Format("AltChunkId{0}", altChunkIdCounter++);

                //Import data as html content using Altchunk
                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);

                using (Stream chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stringWriter = new StreamWriter(chunkStream, Encoding.UTF8)) //Encoding.UTF8 is important to remove special characters
                    {
                        stringWriter.Write(mainhtml);
                    }
                }

                AltChunk altChunk = new AltChunk();
                altChunk.Id = altChunkId;

                mainPart.Document.Body.InsertAt(altChunk, blockLevelCounter++);
                mainPart.Document.Save();
            }


            DownloadFile(filePath, "Catalog2010-2011" + ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            bytes = System.IO.File.ReadAllBytes(filePath);
            //  File.Copy(copyPath, filePath);
            File.Delete(filePath);



        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
        finally
        {

        }
        return bytes;
    }
    private void DownloadFile(string completeFilePath, string fileName, string contentType)
    {
        Stream iStream = null;

        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Length of the file:
        int length;

        // Total bytes to read:
        long dataToRead;

        try
        {
            // Open the file.
            iStream = new FileStream(completeFilePath, FileMode.Open,
            FileAccess.Read, FileShare.Read);

            // Total bytes to read:
            dataToRead = iStream.Length;

            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

            // Read the bytes.
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = iStream.Read(buffer, 0, 10000);

                    // Write the data to the current output stream.
                    Response.OutputStream.Write(buffer, 0, length);

                    // Flush the data to the HTML output.
                    Response.Flush();

                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
            Response.Close();
        }
    }


    protected void btndeleteIEP_Click(object sender, EventArgs e)
    {
        try
        {
            clsData.blnTrans = true;
            SqlTransaction Transs = null;
            objData = new clsData();
            SqlConnection con = objData.Open();
            clsData.blnTrans = true;
            Transs = con.BeginTransaction();
            sess = (clsSession)Session["UserSession"];
            int PrevIEPId = 0;
            int inprogressId = Convert.ToInt32(objData.FetchValueTrans("Select TOP 1 StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress') ORDER BY StdtIEP_PEId DESC ", Transs, con));
            if (objData.IFExistsWithTranss("Select TOP 1 StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Expired') ORDER BY StdtIEP_PEId DESC ", Transs, con))
            {
                PrevIEPId = Convert.ToInt32(objData.FetchValueTrans("Select TOP 1 StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Expired') ORDER BY StdtIEP_PEId DESC", Transs, con));
                string PrevIEP = "UPDATE StdtIEP_PE SET StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved') WHERE StdtIEP_PEId='" + PrevIEPId + "'";
                objData.ExecuteWithTrans(PrevIEP, con, Transs);
            }
            string DeleteIEP = "DELETE FROM StdtIEP_PE WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEP, con, Transs);
            string DeleteIEPExt1 = "DELETE FROM IEP_PE_Details WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt1, con, Transs);
            string DeleteIEPExt2 = "DELETE FROM IEP_PE10_Benchmark WHERE StdIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt2, con, Transs);
            string DeleteIEPExt3 = "DELETE FROM IEP_PE11_SchoolPer WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt3, con, Transs);
            string DeleteIEPExt4 = "DELETE FROM IEP_PE11_SDI WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt4, con, Transs);
            string DeleteIEPExt5 = "DELETE FROM IEP_PE11_Service WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt5, con, Transs);
            string DeleteIEPExt6 = "DELETE FROM IEP_PE11_SchoolPer WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt6, con, Transs);
            string DeleteIEPExt7 = "DELETE FROM IEP_PE12_DateOfRev WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt7, con, Transs);
            string DeleteIEPExt8 = "DELETE FROM IEP_PE12_ESY WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt8, con, Transs);
            string DeleteIEPExt9 = "DELETE FROM IEP_PE12_SupportService WHERE StdIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt9, con, Transs);
            string DeleteIEPExt10 = "DELETE FROM IEP_PE2_Team WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt10, con, Transs);
            string DeleteIEPExt11 = "DELETE FROM IEP_PE6_Edu WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt11, con, Transs);
            string DeleteIEPExt12 = "DELETE FROM IEP_PE6_Goal WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt12, con, Transs);
            string DeleteIEPExt13 = "DELETE FROM IEP_PE6_GoalsObj WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt13, con, Transs);
            string DeleteIEPExt14 = "DELETE FROM IEP_PE6_Living WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt14, con, Transs);
            string DeleteIEPExt15 = "DELETE FROM IEPPA1Extension WHERE IepPAId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt15, con, Transs);
            string DeleteIEPExt16 = "DELETE FROM StdtIEP_PE10_GoalsObj WHERE StdtIEP_PEId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt16, con, Transs);


            string DeleteStdtGoalSvc = "DELETE FROM StdtGoalSvc WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteStdtGoalSvc, con, Transs);
            string LessonPlans = "SELECT LessonPlanId FROM StdtLessonPlan WHERE StdtIEPId='" + inprogressId + "'";
            System.Data.DataTable dtLesson = objData.ReturnDataTableWithTransaction(LessonPlans, con, Transs, false);
            if (dtLesson != null)
            {
                foreach (DataRow row in dtLesson.Rows)
                {
                    System.Data.DataTable dtdstemphdr = objData.ReturnDataTableWithTransaction("SELECT PrevStatus,DSTempHdrId FROM DSTempHdr WHERE LessonPlanId = '" + row["LessonPlanId"] + "' AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName='In Progress') ", con, Transs, false);
                    if (dtdstemphdr != null && dtdstemphdr.Rows.Count > 0)
                    {
                        if (dtdstemphdr.Rows[0]["PrevStatus"] != null || Convert.ToString(dtdstemphdr.Rows[0]["PrevStatus"]) != "")
                        {
                            if (Convert.ToString(dtdstemphdr.Rows[0]["PrevStatus"]).Contains("ST"))
                            {
                                string[] results = dtdstemphdr.Rows[0]["PrevStatus"].ToString().Split(new[] { "ST-" }, StringSplitOptions.None);
                                string strQuery = "UPDATE DSTempHdr SET StatusId='" + results[1] + "' WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                objData.ExecuteWithTrans(strQuery, con, Transs);
                                string StrQuery = "";
                                StrQuery = "SELECT StdtLessonPlanId,GoalId FROM StdtLessonPlan WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId='" + dtdstemphdr.Rows[0]["DSTempHdrId"] + "') AND StdtIEPId='" + inprogressId + "'";
                                System.Data.DataTable dtlessongoal = objData.ReturnDataTableWithTransaction(StrQuery, con, Transs, false);
                                if (dtlessongoal != null && dtlessongoal.Rows.Count > 0)
                                {
                                    StrQuery = "SELECT StdtGoalId FROM StdtGoal WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND GoalId='" + dtlessongoal.Rows[0]["GoalId"] + "' AND StdtIEPId='" + inprogressId + "'";
                                    int stdtGoalid = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                    StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP_PE WHERE StdtIEP_PEId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') THEN 0 ELSE 1 END AS STATUS";
                                    int Status = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                    if (Status == 1)
                                    {
                                        if (Convert.ToInt32(objData.FetchValueTrans("SELECT COUNT(*) FROM StdtLessonPlan WHERE GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') AND StudentId='" + sess.StudentId + "' AND ActiveInd='A' ", Transs, con)) == 1)
                                        {
                                            string DeleteStdtGoal = "DELETE FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "'";
                                            objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                        }
                                        else if (Convert.ToInt32(objData.FetchValueTrans("SELECT COUNT(*) FROM StdtGoal WHERE GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') AND StudentId='" + sess.StudentId + "' AND ActiveInd='A'", Transs, con)) > 1)
                                        {
                                            string DeleteStdtGoal = "DELETE FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "'";
                                            objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                        }
                                        else
                                        {
                                            string DeleteStdtGoal = "UPDATE StdtGoal SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtGoalId='" + stdtGoalid + "'";
                                            objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                        }
                                    }
                                    else
                                    {
                                        string DeleteStdtGoal = "UPDATE StdtGoal SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtGoalId='" + stdtGoalid + "'";
                                        objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                    }
                                    StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP_PE WHERE StdtIEP_PEId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "') THEN 0 ELSE 1 END AS STATUS";
                                    int LPStatus = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                    if (LPStatus == 1)
                                    {
                                        string DeleteStdtLessonPlan = "DELETE FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "'";
                                        objData.ExecuteWithTrans(DeleteStdtLessonPlan, con, Transs);
                                    }
                                    else
                                    {
                                        string DeleteStdtLessonPlan = "UPDATE StdtLessonPlan SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "'";
                                        objData.ExecuteWithTrans(DeleteStdtLessonPlan, con, Transs);
                                    }
                                }
                                if (PrevIEPId > 0)
                                {
                                    System.Data.DataTable dtstdtlessonplan = objData.ReturnDataTableWithTransaction("SELECT StdtLessonPlanId FROM StdtLessonPlan WHERE StdtIEPId='" + PrevIEPId + "'", con, Transs, false);
                                    foreach (DataRow dr in dtstdtlessonplan.Rows)
                                    {
                                        if (objData.IFExistsWithTranss("Select StdtLessonPlanId FROM StdtLessonPlan WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND LessonPlanId=(SELECT LessonPlanId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') AND GoalId=(SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') ", Transs, con))
                                        {
                                            objData.ExecuteWithTrans("DELETE FROM StdtLessonPlan WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND LessonPlanId=(SELECT LessonPlanId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') AND GoalId=(SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') ", con, Transs);
                                        }
                                    }
                                    System.Data.DataTable dtstdtgoal = objData.ReturnDataTableWithTransaction("SELECT StdtGoalId FROM StdtGoal WHERE StdtIEPId='" + PrevIEPId + "'", con, Transs, false);
                                    foreach (DataRow dr in dtstdtgoal.Rows)
                                    {
                                        if (objData.IFExistsWithTranss("Select StdtGoalId FROM StdtGoal WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + dr["StdtGoalId"] + "') ", Transs, con))
                                        {
                                            objData.ExecuteWithTrans("DELETE FROM StdtGoal WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + dr["StdtGoalId"] + "') ", con, Transs);
                                        }
                                    }
                                }
                            }
                            else if (Convert.ToString(dtdstemphdr.Rows[0]["PrevStatus"]).Contains("IEP"))
                            {
                                string[] results = dtdstemphdr.Rows[0]["PrevStatus"].ToString().Split(new[] { "IEP-" }, StringSplitOptions.None);
                                if (Convert.ToInt32(results[1]) == inprogressId)
                                {
                                    string StrQuery = "";
                                    StrQuery = "SELECT StdtLessonPlanId,GoalId FROM StdtLessonPlan WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId='" + dtdstemphdr.Rows[0]["DSTempHdrId"] + "') AND StdtIEPId='" + inprogressId + "'";
                                    System.Data.DataTable dtlessongoal = objData.ReturnDataTableWithTransaction(StrQuery, con, Transs, false);
                                    if (dtlessongoal != null && dtlessongoal.Rows.Count > 0)
                                    {
                                        StrQuery = "SELECT StdtGoalId FROM StdtGoal WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND GoalId='" + dtlessongoal.Rows[0]["GoalId"] + "' AND StdtIEPId='" + inprogressId + "'";
                                        int stdtGoalid = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                        StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP_PE WHERE StdtIEP_PEId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') THEN 0 ELSE 1 END AS STATUS";
                                        int Status = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                        if (Status == 1)
                                        {
                                            if (Convert.ToInt32(objData.FetchValueTrans("SELECT COUNT(*) FROM StdtLessonPlan WHERE GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') AND StudentId='" + sess.StudentId + "' AND ActiveInd='A' ", Transs, con)) == 1)
                                            {
                                                string DeleteStdtGoal = "DELETE FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "'";
                                                objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                            }
                                            else if (Convert.ToInt32(objData.FetchValueTrans("SELECT COUNT(*) FROM StdtGoal WHERE GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') AND StudentId='" + sess.StudentId + "' AND ActiveInd='A'", Transs, con)) > 1)
                                            {
                                                string DeleteStdtGoal = "DELETE FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "'";
                                                objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                            }
                                            else
                                            {
                                                string DeleteStdtGoal = "UPDATE StdtGoal SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtGoalId='" + stdtGoalid + "'";
                                                objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                            }
                                        }
                                        else
                                        {
                                            string DeleteStdtGoal = "UPDATE StdtGoal SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtGoalId='" + stdtGoalid + "'";
                                            objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                        }
                                        StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP_PE WHERE StdtIEP_PEId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "') THEN 0 ELSE 1 END AS STATUS";
                                        int LPStatus = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                        if (LPStatus == 1)
                                        {
                                            string DeleteStdtLessonPlan = "DELETE FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "'";
                                            objData.ExecuteWithTrans(DeleteStdtLessonPlan, con, Transs);
                                        }
                                        else
                                        {
                                            string DeleteStdtLessonPlan = "UPDATE StdtLessonPlan SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "'";
                                            objData.ExecuteWithTrans(DeleteStdtLessonPlan, con, Transs);
                                        }
                                    }
                                    string strQuery = "";
                                    strQuery = "DELETE FROM DSTempHdr WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    strQuery = "DELETE FROM DSTempPrompt WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    strQuery = "DELETE FROM DSTempSet WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    strQuery = "DELETE FROM DSTempStep WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    strQuery = "DELETE FROM DSTempParentStep WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    strQuery = "DELETE FROM DSTempRule WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    string DSTempSetColid = Convert.ToString(objData.FetchValueTrans("SELECT STUFF((SELECT ','+ CONVERT(VARCHAR(50),DSTempSetColId) FROM (SELECT DSTempSetColId FROM DSTempSetCol WHERE DSTempHdrId='" + dtdstemphdr.Rows[0]["DSTempHdrId"] + "' ) CLS FOR XML PATH('')),1,1,'')", Transs, con));
                                    DSTempSetColid = (DSTempSetColid == "") ? "0" : DSTempSetColid;
                                    strQuery = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId IN (" + DSTempSetColid + ")";
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    strQuery = "DELETE FROM DSTempSetCol WHERE DSTempHdrId=" + dtdstemphdr.Rows[0]["DSTempHdrId"];
                                    objData.ExecuteWithTrans(strQuery, con, Transs);
                                    if (PrevIEPId > 0)
                                    {
                                        System.Data.DataTable dtstdtlessonplan = objData.ReturnDataTableWithTransaction("SELECT StdtLessonPlanId FROM StdtLessonPlan WHERE StdtIEPId='" + PrevIEPId + "'", con, Transs, false);
                                        foreach (DataRow dr in dtstdtlessonplan.Rows)
                                        {
                                            if (objData.IFExistsWithTranss("Select StdtLessonPlanId FROM StdtLessonPlan WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND LessonPlanId=(SELECT LessonPlanId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') AND GoalId=(SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') ", Transs, con))
                                            {
                                                objData.ExecuteWithTrans("DELETE FROM StdtLessonPlan WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND LessonPlanId=(SELECT LessonPlanId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') AND GoalId=(SELECT GoalId FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dr["StdtLessonPlanId"] + "') ", con, Transs);
                                            }
                                        }
                                        System.Data.DataTable dtstdtgoal = objData.ReturnDataTableWithTransaction("SELECT StdtGoalId FROM StdtGoal WHERE StdtIEPId='" + PrevIEPId + "'", con, Transs, false);
                                        foreach (DataRow dr in dtstdtgoal.Rows)
                                        {
                                            if (objData.IFExistsWithTranss("Select StdtGoalId FROM StdtGoal WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + dr["StdtGoalId"] + "') ", Transs, con))
                                            {
                                                objData.ExecuteWithTrans("DELETE FROM StdtGoal WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND StdtIEPId IS NULL AND IncludeIEP=0 AND GoalId=(SELECT GoalId FROM StdtGoal WHERE StdtGoalId='" + dr["StdtGoalId"] + "') ", con, Transs);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string StrQuery = "";
                                    StrQuery = "SELECT StdtLessonPlanId,GoalId FROM StdtLessonPlan WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId='" + dtdstemphdr.Rows[0]["DSTempHdrId"] + "') AND StdtIEPId='" + inprogressId + "'";
                                    System.Data.DataTable dtlessongoal = objData.ReturnDataTableWithTransaction(StrQuery, con, Transs, false);
                                    if (dtlessongoal != null && dtlessongoal.Rows.Count > 0)
                                    {
                                        StrQuery = "SELECT StdtGoalId FROM StdtGoal WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND GoalId='" + dtlessongoal.Rows[0]["GoalId"] + "' AND StdtIEPId='" + inprogressId + "'";
                                        int stdtGoalid = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                        string DeleteStdtGoal = "UPDATE StdtGoal SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtGoalId='" + stdtGoalid + "'";
                                        objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                                        string DeleteStdtLessonPlan = "UPDATE StdtLessonPlan SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "'";
                                        objData.ExecuteWithTrans(DeleteStdtLessonPlan, con, Transs);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string StrQuery = "";
                        StrQuery = "SELECT StdtLessonPlanId,GoalId FROM StdtLessonPlan WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StdtIEPId='" + inprogressId + "'";
                        System.Data.DataTable dtlessongoal = objData.ReturnDataTableWithTransaction(StrQuery, con, Transs, false);
                        if (dtlessongoal != null && dtlessongoal.Rows.Count > 0)
                        {
                            if (objData.IFExistsWithTranss("SELECT StdtGoalId FROM StdtGoal WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StdtIEPId='" + inprogressId + "' AND GoalId='" + dtlessongoal.Rows[0]["GoalId"] + "'", Transs, con))
                            {
                                StrQuery = "SELECT StdtGoalId FROM StdtGoal WHERE StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND StdtIEPId='" + inprogressId + "' AND GoalId='" + dtlessongoal.Rows[0]["GoalId"] + "'";
                                int stdtGoalid = Convert.ToInt32(objData.FetchValueTrans(StrQuery, Transs, con));
                                string DeleteStdtGoal = "UPDATE StdtGoal SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtGoalId='" + stdtGoalid + "'";
                                objData.ExecuteWithTrans(DeleteStdtGoal, con, Transs);
                            }
                            string DeleteStdtLessonPlan = "UPDATE StdtLessonPlan SET IncludeIEP=0, StdtIEPId=NULL WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "'";
                            objData.ExecuteWithTrans(DeleteStdtLessonPlan, con, Transs);
                        }
                    }
                }
            }

            objData.CommitTransation(Transs, con);
            LoadData();
            //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('IEP deleted successfully...');", true);
            tdMsg.InnerHtml = clsGeneral.sucessMsg("IEP deleted successfully...");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    #region BSP Form Upload 05-05-2015 Hari

    protected void grdFile_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void grdFile_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            divMessage.InnerHtml = "";
            string file = Convert.ToString(e.CommandArgument);
            objData = new clsData();
            if (e.CommandName == "Edit")
            {
                try
                {
                    if (sess != null)
                    {
                        int headerId = sess.IEPId;
                        deleteDocument(file);
                        FillDoc(headerId);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);

                }
                catch
                {

                }

            }
        }
        catch (Exception)
        {

        }

    }

    private void deleteDocument(string fileName)
    {
        try
        {
            clsData objdata = new clsData();
            string Query = "DELETE FROM BSPDoc WHERE BSPDoc='" + fileName + "'";
            int a = objdata.Execute(Query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }




    protected void grdFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void grdFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btUpload_Click(object sender, EventArgs e)
    {
        try
        {
            int headerId = sess.IEPId;

            if (headerId != null)
            {

                clsData objData = new clsData();
                divMessage.InnerHtml = "";
                clsDocumentasBinary objBinary = new clsDocumentasBinary();
                if (fupDoc.HasFile)
                {

                    string ext = System.IO.Path.GetExtension(fupDoc.FileName);
                    string name = System.IO.Path.GetFileNameWithoutExtension(fupDoc.FileName);
                    if (name != "")
                    {
                        if (name.Length > 50)
                        {
                            name = name.Substring(0, 30) + "....";
                            name += ext;
                        }
                    }
                    if (ext.ToLower() == ".txt" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg" || ext.ToLower() == ".png" || ext.ToLower() == ".docx" || ext.ToLower() == ".doc" || ext.ToLower() == ".pdf" || ext.ToLower() == ".csv" || ext.ToLower() == ".xlsx" || ext.ToLower() == ".xls")
                    {

                        string filename = System.IO.Path.GetFileName(fupDoc.FileName);
                        HttpPostedFile myFile = fupDoc.PostedFile;
                        int nFileLen = myFile.ContentLength;
                        byte[] myData = new byte[nFileLen];
                        myFile.InputStream.Read(myData, 0, nFileLen);
                        int binId = SaveDocument(sess.SchoolId, sess.IEPId, filename, myData, sess.LoginId);
                        FillDoc(headerId);
                        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
                    }
                    else
                    {
                        divMessage.InnerHtml = clsGeneral.warningMsg("Invalid file format...");
                        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);

                    }
                }
                else
                {
                    divMessage.InnerHtml = clsGeneral.warningMsg("Please select file...");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('Please select a Student');", true);
            }
        }
        catch (Exception ex)
        {
            lMsg.ForeColor = System.Drawing.Color.Red;
            lMsg.Text = "Error:" + ex.Message.ToString();
        }
    }

    private int SaveDocument(int SchoolId, int IEPId, string filename, byte[] contents, int UserId)
    {
        clsData objData = new clsData();
        int BinaryId = 0;
        try
        {
            string query = "INSERT INTO BSPDoc(SchoolId,StdtIEPId,BSPDocUrl,Data,CreatedBy,CreatedOn)VALUES (@SchoolId,@StdtIEPId,@BSPDocUrl,@Data,@CreatedBy,@CreatedOn) \nSELECT SCOPE_IDENTITY()";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = objData.Open();
                cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                cmd.Parameters.AddWithValue("@StdtIEPId", IEPId);
                cmd.Parameters.AddWithValue("@BSPDocUrl", filename);
                cmd.Parameters.AddWithValue("@Data", contents);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                BinaryId = Convert.ToInt16(cmd.ExecuteScalar());

            }


        }
        catch (Exception ex)
        {

            throw ex;
        }
        return BinaryId;
    }

    private void FillDoc(int stdtIEPID)
    {
        try
        {
            objData = new clsData();
            string strQuery = "";
            string ext = System.IO.Path.GetExtension(fupDoc.FileName);
            strQuery = "Select ROW_NUMBER() OVER (ORDER BY BSPDoc) AS No,BSPDocUrl as Document, BSPDoc FROM BSPDoc Where BSPDocUrl<>'' And StdtIEPId = " + stdtIEPID + "";
            System.Data.DataTable Dt = objData.ReturnDataTable(strQuery, false);

            Dt.Columns.Add("Name", typeof(string));


            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string name = Dt.Rows[i]["Document"].ToString();

                if (name != "")
                {
                    if (name.Length > 50)
                    {
                        name = name.Substring(0, 30) + "....";
                        name += ext;
                    }
                }
                Dt.Rows[i]["Name"] = name;
            }
            grdFile.DataSource = Dt;
            grdFile.DataBind();
        }
        catch (Exception)
        {


        }
    }
    protected void btnUploadBSP_Click(object sender, EventArgs e)
    {
        int headerId = sess.IEPId;
        FillDoc(headerId);
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
    }
}

    # endregion


