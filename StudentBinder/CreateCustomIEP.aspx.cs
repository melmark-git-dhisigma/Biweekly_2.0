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
using System.Configuration;
using OpenXmlPowerTools;


public partial class StudentBinder_CreateCustomIEP : System.Web.UI.Page
{
    clsData objData = null;

    static string version;
    clsSession sess = null;
    clsSession oSession = null;

    public string Approvalnotes = null;
    static bool Disable = false;
    string[] NOTE, DATE;
    System.Data.DataTable _dtTest = null;
    DataClass objDataClass = new DataClass();

    static string[] columns;
    static string[] placeHolders;

    static string[] columnsCheck;


    static string[] columnsP4;

    static int checkCount = 0;
    // static string strQuery = "";
    //  bool Disable = false;
    static System.Data.DataTable dtIEP4 = null;




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
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
        if (buildName == "Local")
        {
            lnkbtnSignDetal.Visible = false;
        }
        if (!IsPostBack)
        {
            LoadData();
            FillRecentNotes();
            //clsDocumentasBinary.DeleteFiles(Server.MapPath("~\\Administration\\IEPTemp\\"));
        }
    }
      


    private void FillProgressIEP()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        object IepStatus = null;
        object InProgress = null;
        if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
        {
            IepStatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ").ToString();
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
        lnkBtnIEPYear.Visible = true;
        btndeleteIEP.Style.Add("display", "block");
        btnUploadBSP.Style.Add("display", "block");
        dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEPId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress') ORDER BY StdtIEPId DESC ", true);
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
        lnkBtnPlacement.Visible = true;
        lnkBtnSigPage.Visible = true;
        lnkBtnAddDataSheet.Visible = true;
        lnkBtnAttendSheet.Visible = true;
        btnIEPExport.Style.Add("display", "none");
        btnUpdateandExport.Style.Add("display", "none");

    }

    private void NOInprogressIEP()
    {
        lnkBtnIEPYear.Visible = false;
        lnkBtnIndiviEdu.Visible = false;
        lnkBtnPrestLvlEdu.Visible = false;
        lnkBtnPrestLvlEdu2.Visible = false;
        lnkBtnCurPre.Visible = false;
        LnkBtnSerDel.Visible = false;
        lnkBtnNonParJus.Visible = false;
        lnkBtnStateOfDist.Visible = false;
        lnkBtnAddInfo.Visible = false;
        btndeleteIEP.Style.Add("display", "none");
        btnUploadBSP.Style.Add("display", "none");
        lnkBtnIndiviEdu.Visible = false;
        lnkBtnPrestLvlEdu.Visible = false;
        lnkBtnPrestLvlEdu2.Visible = false;
        lnkBtnCurPre.Visible = false;
        LnkBtnSerDel.Visible = false;
        lnkBtnNonParJus.Visible = false;
        lnkBtnStateOfDist.Visible = false;
        lnkBtnAddInfo.Visible = false;
        lnkBtnPlacement.Visible = false;
        lnkBtnSigPage.Visible = false;
        lnkBtnAddDataSheet.Visible = false;
        lnkBtnAttendSheet.Visible = false;
        btnIEPExport.Style.Add("display", "none");
        btnUpdateandExport.Style.Add("display", "block");
    }
    private void LoadData()
    {
        lnkBtnIEPYear.Visible = false;
        lnkBtnIndiviEdu.Visible = false;
        lnkBtnPrestLvlEdu.Visible = false;
        lnkBtnPrestLvlEdu2.Visible = false;
        lnkBtnCurPre.Visible = false;
        LnkBtnSerDel.Visible = false;
        lnkBtnNonParJus.Visible = false;
        lnkBtnStateOfDist.Visible = false;
        lnkBtnAddInfo.Visible = false;
        lnkBtnPlacement.Visible = false;
        lnkBtnSigPage.Visible = false;
        lnkBtnAddDataSheet.Visible = false;
        lnkBtnAttendSheet.Visible = false;
        btnUploadBSP.Style.Add("display", "none");
        btndeleteIEP.Style.Add("display", "none");
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

        //if (sess.IEPId > 0)
        //{
        //    btnsubmit.Visible = false;
        //}
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

    protected void btnGennewIEP_Click(object sender, EventArgs e)
    {
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (validate() == true)
        {

            if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
            {
                pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ").ToString();

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
    //protected void bindLessonplan(object sender, EventArgs e)
    //{
    //    if (lList.SelectedValue == "1")
    //        Session["include"] = "1";
    //    else
    //        Session["include"] = "2";
    //}
    protected void btnGenIED_Click(object sender, EventArgs e)
    {
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (validate() == true)
        {

            if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
            {
                pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ").ToString();

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
        string strQry = "";
        string strQuy = "";
        string strQey = "";

        string currYearPair = DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddYears(1).Year.ToString();
       

        int ExistYear = Convert.ToInt32(objData.FetchValue("select COUNT(*) from AsmntYear WHERE AsmntYearCode='" + year + "'"));
        if (ExistYear != 0) //selected value exists in table
        {
            //check if CurrentInd = A. so no change.
            //if CurrentInd =D, make it to A and change all others to D
            strQry = "select CurrentInd from AsmntYear where AsmntYearCode ='" + year + "'";
            String curntInd = objData.FetchValue(strQry).ToString(); //returns the curentInd from table
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
        { //adding new row



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
        if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + "  ") == true) //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
        {
            pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + "  ORDER BY StdtIEPId DESC ").ToString(); //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
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
                string copyIEP = "SELECT MAX(StdtIEPId) AS StdtIEPId FROM StdtIEP WHERE StudentId=" + oSession.StudentId + " "; //and AsmntYearId=" + Convert.ToInt32(ddlYear.SelectedValue) + "
                int Oldiep = oData.ExecuteScalar(copyIEP);


                int IEP_id = 0;

                oSession = (clsSession)Session["UserSession"];
                objData = new clsData();
                version = "";
                if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + " AND AsmntYearId=(SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "')  ") == true) //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
                {
                    version = objData.FetchValue("Select TOP 1 Version from StdtIEP where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + " AND AsmntYearId=(SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "') ORDER BY StdtIEPId DESC ").ToString(); //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
                    version = Convert.ToString(Convert.ToDouble(version) + 0.1);
                }
                else if (version == "") version = "0.0";
                //else
                //{
                //    Double ver = Convert.ToDouble(version) + 1; version = Convert.ToString(ver) + ".0";
                //}

                if (Status == "NEW")
                {
                    Oldiep = 0;
                    int ver = (int)Math.Floor(Convert.ToDouble(version)) + 1; version = Convert.ToString(ver) + ".0";
                }

                if (Oldiep > 0)
                {
                    string insOLDIEP = "INSERT INTO StdtIEP([SchoolId],[StudentId],[AsmntYearId],[StatusId],[EffStartDate],[EffEndDate],[Concerns],[Strength],[Vision],[Version],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                              "Select " + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "'),'" + IEPstatus + "','" + dtst.ToString("yyyy-MM-dd") + "','" + dted.ToString("yyyy-MM-dd") + "',[Concerns],[Strength],[Vision],'" + version + "'," + //" + Convert.ToInt32(ddlYear.SelectedValue) + "
                              "" + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtIEP where StdtIEPId='" + Oldiep + "'\t\n" +
                              "SELECT SCOPE_IDENTITY()";
                    IEP_id = oData.ExecuteScalar(insOLDIEP);
                }
                else
                {
                    string insIEP = "INSERT INTO StdtIEP(SchoolId,StudentId,AsmntYearId,Version,EffStartDate,EffEndDate,StatusId,CreatedBy,CreatedOn) " +
                              "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "'),'" + version + "','" + dtst.ToString("yyyy-MM-dd") + "'," +
                              "'" + dted.ToString("yyyy-MM-dd") + "'," + IEPstatus + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                              "SELECT SCOPE_IDENTITY()";
                    IEP_id = oData.ExecuteScalar(insIEP);
                }


                if (IEP_id > 0)
                {

                    
                    if (Oldiep > 0)
                    {
                        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt4 WHERE StdtIEPId=" + Oldiep + " ") == false)
                        {
                            string insIEP4 = "INSERT INTO StdtIEPExt4(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + IEPstatus + "," + oSession.LoginId + "," +
                           "(SELECT convert(varchar, getdate(), 100)))";
                            int newVersion = oData.ExecuteNonQuery(insIEP4);
                        }
                        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt5 WHERE StdtIEPId=" + Oldiep + " ") == false)
                        {
                            string insIEP5 = "INSERT INTO StdtIEPExt5(StdtIEPId) VALUES(" + IEP_id + ")";
                            oData.ExecuteNonQuery(insIEP5);
                        }

                        string InsIEPONE = "Insert into StdtIEPExt1([StdtIEPId],[EngLangInd],[HistInd],[TechInd],[MathInd],[OtherInd],[OtherDesc],[AffectDesc],[AccomDesc],[ContentModInd]," +
                        "[ContentModDesc],[MethodModInd],[MethodModDesc],[PerfModInd],[PerfModDesc],[StatusId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) Select '" + IEP_id + "',[EngLangInd],[HistInd],[TechInd],[MathInd],[OtherInd],[OtherDesc],[AffectDesc],[AccomDesc],[ContentModInd]," +
                        "[ContentModDesc],[MethodModInd],[MethodModDesc],[PerfModInd],[PerfModDesc],'" + IEPstatus + "','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate() from StdtIEPExt1 Where [StdtIEPId]='" + Oldiep + "'";
                        int i = oData.ExecuteNonQuery(InsIEPONE);

                        string InsIEPTWO = "Insert into StdtIEPExt2([StdtIEPId],[PEInd],[TechDevicesInd],[BehaviorInd],[BrailleInd],[CommInd],[CommDfInd],[ExtCurInd],[LEPInd],[NonAcdInd]," +
                        "[SocialInd],[TravelInd],[VocInd],[OtherInd],[OtherDesc],[AgeBand1Ind],[AgeBand2Ind],[AgeBand3Ind],[AffectDesc],[AccomDesc],[ContentModInd]," +
                        "[ContentModDesc],[MethodModInd],[MethodModDesc],[PerfModInd],[PerfModDesc],[StatusId],[5DayInd],[6DayInd],[10DayInd],[DayOther],[OtherDesc1]," +
                        "[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) Select '" + IEP_id + "',[PEInd],[TechDevicesInd],[BehaviorInd],[BrailleInd],[CommInd],[CommDfInd],[ExtCurInd],[LEPInd],[NonAcdInd]," +
                        "[SocialInd],[TravelInd],[VocInd],[OtherInd],[OtherDesc],[AgeBand1Ind],[AgeBand2Ind],[AgeBand3Ind],[AffectDesc],[AccomDesc],[ContentModInd]," +
                        "[ContentModDesc],[MethodModInd],[MethodModDesc],[PerfModInd],[PerfModDesc],'" + IEPstatus + "',[5DayInd],[6DayInd],[10DayInd],[DayOther],[OtherDesc1]," +
                        "'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate() from  StdtIEPExt2 Where [StdtIEPId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(InsIEPTWO);

                        string InsIEPTHREE = "Insert into StdtIEPExt3([StdtIEPId],[RemovedInd1],[RemovedInd2],[RemovedDesc],[ShorterCd1],[ShorterCd2],[ShorterCd3],[LongerCd3]," +
                        "[LongerCd2],[LongerCd1],[SchedModDesc],[TransportInd1],[TransportInd2],[RegTransInd],[RegTransDesc],[SpTransInd],[SpTransDesc],[AsmntPlanned]," +
                        "[EngCol1],[HistCol1],[MathCol1],[TechCol1],[ReadCol1],[EngCol2],[HistCol2],[MathCol2],[TechCol2],[ReadCol2],[EngCol3],[HistCol3],[MathCol3]," +
                        "[TechCol3],[ReadCol3],[InfoCol2],[InfoCol3],[AddInfoCol1],[AddInfoCol2],[AddInfoCol3],[AddInfoCol3Desc],[StatusId],[CreatedBy],[CreatedOn],[ModifiedBy]," +
                        "[ModifiedOn]) Select '" + IEP_id + "',[RemovedInd1],[RemovedInd2],[RemovedDesc],[ShorterCd1],[ShorterCd2],[ShorterCd3],[LongerCd3]," +
                        "[LongerCd2],[LongerCd1],[SchedModDesc],[TransportInd1],[TransportInd2],[RegTransInd],[RegTransDesc],[SpTransInd],[SpTransDesc],[AsmntPlanned]," +
                        "[EngCol1],[HistCol1],[MathCol1],[TechCol1],[ReadCol1],[EngCol2],[HistCol2],[MathCol2],[TechCol2],[ReadCol2],[EngCol3],[HistCol3],[MathCol3]," +
                        "[TechCol3],[ReadCol3],[InfoCol2],[InfoCol3],[AddInfoCol1],[AddInfoCol2],[AddInfoCol3],[AddInfoCol3Desc],'" + IEPstatus + "','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate() from  StdtIEPExt3 Where [StdtIEPId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(InsIEPTHREE);

                        string InsIEPFOUR = "Insert into StdtIEPExt4([StdtIEPId],[SigRoleLEARep],[SigRep_date],[ParntAccptIEP],[ParntRejctIEP],[ParntDontRejctIEP]," +
                           "[ParntDontRejctDesc],[ParntReqMeetig],[SigParnt],[SigParnt_date],[ParntComnt],[PoMEliDeter],[PoMInitEval],[PoMReeval],[PoMIEPDev],[PoMInit]," +
                           "[PoMAnnRev],[PoMOtherCheck],[PoMOtherText],[PoMPlacement],[RoleDesc],[LanguageofInst],[ActonOwnBehalfCk],[CourtAppGrdCk]," +
                           "[SharedDecMakingCk],[DelegateDeciMakCk],[CourtAppGuardian],[PrLanguageGrd1],[PrLanguageGrd2],[DateOfMeeting],[TypeOfMeeting]," +
                           "[AnnualReviewMeeting],[ReevaluationMeeting],[CostSharedPnt],[SchoolPhone],[SchoolName],[SchAddress],[SchTelephone],[SchContact],[AtndDate],[SpecifyAgency],[PlOneEarlyPgm],[PlOneSeparatePgm],[PlOneBothPgm]," +
                       "[PlOneServiceLocation],[PlOneServiceLocation2],[PlOneHoursWkPgm],[PlOneEnrolledPrnt],[PlOnePlcdTeam],[PlOneTimeMore],[PlOneTimeTwo],[PlOneTimeThree],[PlOneSeparateClass]," +
                       "[PlOneSeparateDayScl],[PlOneSeparatePublic],[PlOneSeparatePvt],[PlOneResidentialFacility],[PlOneHome],[PlOneServiceLctn],[PlOnePsychiatric]," +
                       "[PlOneMassachusetts],[PlOneMassachusettsDay],[PlOneMassachusettsRes],[PlOneDoctorHme],[PlOneDoctorHsptl],[PlOneConsent],[PlOneRefuse]," +
                       "[PlOnePlacement],[PlOneSignParent],[PlOneDate],[PlTwoFullInclusionPgm],[PlTwoPartialPgm],[PlTwoSubstantially],[PlTwoSeparateScl],[PlTwoPublicScl],[PlTwoPrivateScl]," +
                       "[PlTwoYouth],[PlTwoResScl],[PlTwoOther],[PlTwoOtherDesc],[PlTwoPsychiatric],[PlTwoMassachusetts],[PlTwoMassachusettsDay],[PltwoMassachusettsRes],[PlTwoCorrectionFacility],[PlTwoDoctorHome]," +
                       "[PlTwoDoctorHsptl],[PlTwoConsent],[PlTwoPlacement],[PlTwoSignParent],[PlTwoDate],[PlTwoFullPgm],[StatusId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn])" +
                       "Select '" + IEP_id + "',[SigRoleLEARep],[SigRep_date],[ParntAccptIEP],[ParntRejctIEP],[ParntDontRejctIEP]," +
                           "[ParntDontRejctDesc],[ParntReqMeetig],[SigParnt],[SigParnt_date],[ParntComnt],[PoMEliDeter],[PoMInitEval],[PoMReeval],[PoMIEPDev],[PoMInit]," +
                           "[PoMAnnRev],[PoMOtherCheck],[PoMOtherText],[PoMPlacement],[RoleDesc],[LanguageofInst],[ActonOwnBehalfCk],[CourtAppGrdCk]," +
                           "[SharedDecMakingCk],[DelegateDeciMakCk],[CourtAppGuardian],[PrLanguageGrd1],[PrLanguageGrd2],[DateOfMeeting],[TypeOfMeeting]," +
                           "[AnnualReviewMeeting],[ReevaluationMeeting],[CostSharedPnt],[SchoolPhone],[SchoolName],[SchAddress],[SchTelephone],[SchContact],[AtndDate],[SpecifyAgency],[PlOneEarlyPgm],[PlOneSeparatePgm],[PlOneBothPgm]," +
                       "[PlOneServiceLocation],[PlOneServiceLocation2],[PlOneHoursWkPgm],[PlOneEnrolledPrnt],[PlOnePlcdTeam],[PlOneTimeMore],[PlOneTimeTwo],[PlOneTimeThree],[PlOneSeparateClass]," +
                       "[PlOneSeparateDayScl],[PlOneSeparatePublic],[PlOneSeparatePvt],[PlOneResidentialFacility],[PlOneHome],[PlOneServiceLctn],[PlOnePsychiatric]," +
                       "[PlOneMassachusetts],[PlOneMassachusettsDay],[PlOneMassachusettsRes],[PlOneDoctorHme],[PlOneDoctorHsptl],[PlOneConsent],[PlOneRefuse]," +
                       "[PlOnePlacement],[PlOneSignParent],[PlOneDate],[PlTwoFullInclusionPgm],[PlTwoPartialPgm],[PlTwoSubstantially],[PlTwoSeparateScl],[PlTwoPublicScl],[PlTwoPrivateScl]," +
                       "[PlTwoYouth],[PlTwoResScl],[PlTwoOther],[PlTwoOtherDesc],[PlTwoPsychiatric],[PlTwoMassachusetts],[PlTwoMassachusettsDay],[PltwoMassachusettsRes],[PlTwoCorrectionFacility],[PlTwoDoctorHome]," +
                       "[PlTwoDoctorHsptl],[PlTwoConsent],[PlTwoPlacement],[PlTwoSignParent],[PlTwoDate],[PlTwoFullPgm],'" + IEPstatus + "','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate() from  StdtIEPExt4 Where [StdtIEPId]='" + Oldiep + "'";
                        int InsertIepFour = oData.ExecuteNonQuery(InsIEPFOUR);


                        string InsIEPFIVE = "INSERT INTO StdtIEPExt5([StdtIEPId],[TMName],[TMRole],[InitialIfInAttn]) SELECT '" + IEP_id + "',[TMName],[TMRole],[InitialIfInAttn] FROM StdtIEPExt5 WHERE [StdtIEPId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(InsIEPFIVE);

                        string InsBsp = "INSERT INTO BSPDoc ([StdtIEPId],[Data],[BSPDocUrl],[SchoolId],[CreatedBy],[CreatedOn]) SELECT '" + IEP_id + "',[Data],[BSPDocUrl],[SchoolId],[CreatedBy],[CreatedOn] FROM BSPDoc WHERE [StdtIEPId]='" + Oldiep + "' ";
                        int InsBspVal = oData.ExecuteNonQuery(InsBsp);


                    }
                    else
                    {
                        string insIEP1 = "INSERT INTO StdtIEPExt1(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + IEPstatus + "," + oSession.LoginId + "," +
                            "(SELECT convert(varchar, getdate(), 100)))";
                        oData.ExecuteNonQuery(insIEP1);
                        string insIEP2 = "INSERT INTO StdtIEPExt2(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + IEPstatus + "," + oSession.LoginId + "," +
                            "(SELECT convert(varchar, getdate(), 100)))";
                        oData.ExecuteNonQuery(insIEP2);
                        string insIEP3 = "INSERT INTO StdtIEPExt3(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + IEPstatus + "," + oSession.LoginId + "," +
                            "(SELECT convert(varchar, getdate(), 100)))";
                        oData.ExecuteNonQuery(insIEP3);
                        string insIEP4 = "INSERT INTO StdtIEPExt4(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + IEP_id + "," + IEPstatus + "," + oSession.LoginId + "," +
                            "(SELECT convert(varchar, getdate(), 100)))";
                        oData.ExecuteNonQuery(insIEP4);
                        string insIEP5 = "INSERT INTO StdtIEPExt5(StdtIEPId) VALUES(" + IEP_id + ")";
                        oData.ExecuteNonQuery(insIEP5);




                    }
                }
                //}

                if (IEP_id > 0)
                {
                    SqlTransaction Trans = null;
                    SqlConnection Con = new SqlConnection();
                    clsData clsData = new clsData();
                    oSession = (clsSession)Session["UserSession"];
                    try
                    {
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
                                    string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,IEPGoalNo,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,[CurPerfLevel],[AnnualGoalDesc],[Objective1],[Objective2],[Objective3],GoalIEPNote,CreatedBy,CreatedOn) " +
                                                     "select " + oSession.SchoolId + "," + oSession.StudentId.ToString() + ",GoalId,IEPGoalNo," + IEP_id + ",(SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "')," +
                                                     "" + GlstatusID + ",'A',IncludeIEP,[CurPerfLevel],[AnnualGoalDesc],[Objective1],[Objective2],[Objective3],GoalIEPNote," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtGoal Where  StdtIEPId='" + Oldiep + "' and GoalId=" + Convert.ToInt32(row["GoalId"]) + " ";

                                    int newgoalid = clsData.ExecuteWithScopeandConnection(insGoal, Con, Trans);



                                }
                                //}
                            }
                            Object OldgoalSvcData = clsData.FetchValueTrans("Select count(*) as tot from StdtGoalSvc wHERE StdtIEPId=" + Oldiep + "", Trans, Con);
                            //string strStdtGoalSvc = "select StdtGoalSvcId from StdtGoalSvc where StdtIEPId=" + Oldiep + "";
                            //DataTable dtGoalSvc = new DataTable();
                            //dtGoalSvc = clsData.ReturnDataTableWithTransaction(strStdtGoalSvc, Con, Trans, false);
                            //if (Convert.ToInt32(OldgoalSvcData) > 0)
                            //{
                            //    foreach (DataRow row in dtGoalSvc.Rows)
                            //    {
                            if (OldgoalSvcData.ToString() != "0")
                            {
                                string goalsvc = "INSERT INTO StdtGoalSvc (StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,CreatedBy,CreatedOn,StdtIEPId,[ModifiedBy],[ModifiedOn]) " +
                                     "Select StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,'" + oSession.LoginId + "',getdate(),'" + IEP_id + "','" + oSession.LoginId + "',getdate() From StdtGoalSvc Where StdtIEPId='" + Oldiep + "' ";//and StdtGoalSvcId=" + Convert.ToInt32(row["StdtGoalSvcId"]) + "";
                                clsData.ExecuteWithScopeandConnection(goalsvc, Con, Trans);
                            }

                            // }
                            // }




                            string OldLPdata = "SELECT LessonPlanId,GoalId FROM StdtLessonPlan WHERE StdtIEPId=" + Oldiep + "";
                            dtLP = objData.ReturnDataTableWithTransaction(OldLPdata, Con, Trans, false);

                            clsAssignLessonPlan objAssign = new clsAssignLessonPlan();


                            //string insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,CreatedBy,CreatedOn) " +
                            //                "VALUES(" + oSession.SchoolId + "," + oSession.StudentId.ToString() + ",LessonPlanId,GoalId," + IEP_id + "," +
                            //                "(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A')," + LPstatusID + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";




                            if (dtLP != null)
                            {
                                if (dtLP.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtLP.Rows)
                                    {

                                        int visualLessonId = 0;
                                        int apprvdLessonId = 0;
                                        string insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,Objective1,Objective2,Objective3,IncludeIEP,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,CreatedOn) " +
                                        "Select SchoolId,StudentId,'" + Convert.ToInt32(row["LessonPlanId"]) + "','" + Convert.ToInt32(row["GoalId"]) + "','" + IEP_id + "',AsmntYearId," + LPstatusID + ",ActiveInd,Objective1,Objective2,Objective3,IncludeIEP,LessonPlanTypeDay,LessonPlanTypeResi," + oSession.LoginId + ",getdate() from StdtLessonPlan Where StdtIEPId='" + Oldiep + "' AND LessonPlanId='" + Convert.ToInt32(row["LessonPlanId"]) + "' AND GoalId='" + Convert.ToInt32(row["GoalId"]) + "'";
                                        int lpID = objData.ExecuteWithScopeandConnection(insLP, Con, Trans);
                                        System.Data.DataTable LessonPlan = objData.ReturnDataTableWithTransaction("SELECT LessonPlanName from LessonPlan where LessonPlanId=" + row["LessonPlanId"] + "", Con, Trans, false);
                                       
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
                            objData.ExecuteWithTrans("update StdtIEP set StatusId=(select LookupId from lookup where LookupType='IEP Status' and LookupName='Expired') where StdtIEPId=" + Oldiep, Con, Trans);
                            //objData.Execute("update StdtGoal set ActiveInd='D' where IncludeIEP=1 and StdtIEPId=" + Oldiep);
                            //objData.Execute("update StdtLessonPlan set ActiveInd='D' where IncludeIEP=1 and StdtIEPId=" + Oldiep);
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
                                //string strStdtGoal = "select DISTINCT GoalId,IEPGoalNo from StdtGoal where StudentId=" + oSession.StudentId + " AND ActiveInd='A' order by IEPGoalNo";
                                string strStdtGoal = "select DISTINCT GoalId,MAX(IEPGoalNo) AS IEPGoalNo from StdtGoal where StudentId=" + oSession.StudentId + " AND ActiveInd='A' Group By GoalId";
                                dtgoal = clsData.ReturnDataTableWithTransaction(strStdtGoal, Con, Trans, false);
                                foreach (DataRow row in dtgoal.Rows)
                                {
                                    string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,IEPGoalNo,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,CreatedBy,CreatedOn) " +
                                                     "VALUES( " + oSession.SchoolId + "," + oSession.StudentId.ToString() + "," + row["GoalId"] + "," + row["IEPGoalNo"] + "," + IEP_id + ",(SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "')," +
                                                     "" + GlstatusID + ",'A',0," + oSession.LoginId + ",GETDATE()) ";

                                    int newgoalid = clsData.ExecuteWithScopeandConnection(insGoal, Con, Trans);



                                }
                                //}
                            }

                            string OldLPdata = "SELECT DISTINCT LessonPlanId,GoalId,LessonPlanTypeDay,LessonPlanTypeResi FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND ActiveInd='A'";
                            dtLP = objData.ReturnDataTableWithTransaction(OldLPdata, Con, Trans, false);
                            string AsmntYr = Convert.ToString(objData.FetchValueTrans("SELECT AsmntYearId FROM AsmntYear WHERE AsmntYearDesc='" + year + "'", Trans, Con));

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
                string selctStudentName = "SELECT StudentLname + StudentFname As StudentName FROM Student WHERE StudentId = " + studId;
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

    public void checkExistingIepEditCreateStatus(bool permission)
    {
        tdMsg.InnerHtml = "";
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + "  ") == true)
        {
            pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
            if (IepStatus.ToString() == "Approved")
            {
                DataTable dtdates = objData.ReturnDataTable("Select TOP 1 EffStartDate,EffEndDate from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ", false);

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
                //dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEPId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ", true);
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


    public void checkIepEditCreateStatus(bool permission)
    {
        tdMsg.InnerHtml = "";
        object pendstatus = null;
        object IepStatus = null;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + "  ") == true)
        {
            pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ").ToString();

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
                //dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEPId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ", true);
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
            //string selectDates = "Select Convert(date,AsmntYearStartDt) AS StartDate,Convert(date,AsmntYearEndDt) AS EndDate from AsmntYear Where AsmntYearId=(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A') ";//'" + Convert.ToInt32(ddlYearF) + "'";
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

        //if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
        //{
        //    pendstatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ").ToString();

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
    public static int getInprogressIepId()
    {
        clsData objData = new clsData();
        clsSession sess = null;
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int IepId = 0;
        object IepStatus = null;
        object InProgress = null;
        if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
        {
            IepStatus = objData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ORDER BY StdtIEPId DESC ").ToString();
            InProgress = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(IepStatus.ToString()) + "");
            if (InProgress.ToString() == "In Progress")
            {
                object obj = objData.FetchValue(" Select StdtIEPId from stdtiep where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId=" + int.Parse(IepStatus.ToString()) + " ");
                if (obj != null) IepId = Convert.ToInt16(obj);

            }
        }
        return IepId;
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string IepUpdateStatus(string testVal)
    {
        clsData objDate = new clsData();
        System.Data.DataTable dt = new System.Data.DataTable();
        string returnVal = "";
        if (testVal != "")
        {

            dt = objDate.ReturnDataTable("select * from stdtIEPUpdateStatus where stdtIEPId=" + int.Parse(testVal), true);

            if (dt != null)
            {

                if (dt.Rows.Count > 0)
                    returnVal = dt.Rows[0]["Page1"] + ":" + dt.Rows[0]["Page2"] + ":" + dt.Rows[0]["Page3"] + ":" + dt.Rows[0]["Page4"] + ":" + dt.Rows[0]["Page5"] + ":" + dt.Rows[0]["Page6"] + ":" + dt.Rows[0]["Page7"] + ":" + dt.Rows[0]["Page8"] + ":" + dt.Rows[0]["Page9"] + ":" + dt.Rows[0]["Page10"] + ":" + dt.Rows[0]["Page11"] + ":" + dt.Rows[0]["Page12"];


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
        //   iepid = Convert.ToString(objData.FetchValue("Select StdtIEPId from [dbo].[StdtIEPExt1] Intersect Select StdtIEPId from [dbo].[StdtIEPExt2] Intersect Select StdtIEPId from [dbo].[StdtIEPExt3] Intersect Select StdtIEPId from [dbo].[StdtLessonPlan] Intersect Select StdtIEPId from [dbo].[StdtGoal] Intersect Select StdtIEPId from [dbo].[StdtGoalSvc] where StdtIEPId='" + sess.IEPId + "'"));
        //   if (iepid != "")
        //    {
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        string checkIEP = "Update StdtIEP set StatusId=" + pendingApprove + " where StdtIEPId=" + sess.IEPId + "";
        int result = oData.ExecuteNonQuery(checkIEP);
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
        LoadData();
        //FillPendingIEP();
        lnkBtnIEPYear.Text = "";
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
        
    }

    private void FillRecentNotes()
    {
        sess = (clsSession)Session["UserSession"];
        string notes = "SELECT ApprovalNotes,'IEP Version - '+Version AS Version from StdtIEP where StudentId='" + sess.StudentId + "' ";
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
            string SQLQRY = "UPDATE StdtIEP set ApprovalNotes='" + clsGeneral.convertQuotes(notes) + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEPId='" + sess.IEPId + "'";
            int retVal = objData.Execute(SQLQRY);
            if (retVal > 0)
            {
                string selIEPstatus = "Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ";
                object IEPstatus = objData.FetchValue(selIEPstatus);
                sess.IEPStatus = int.Parse(IEPstatus.ToString());
       

                if (btnAdd.Text == "Approve")
                {
                    btnAdd.Enabled=true;
                  
                    // enableApprove()
                    //{ 
                    // $('#btnadd').removeattr('disable');
                    //}

                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Approved Successfully");
 

                    // 
                    string strReplace = "StatusId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + " AND LessonPlanId=StdtLessonPlan.LessonPlanId AND StatusId IN " +
                                    "(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' ";
                    string strReplace2 = "DSTempHdrId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + "  AND LessonPlanId=StdtLessonPlan.LessonPlanId AND " +
                                    "StatusId = (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus'";

                    string updatequery = "";
                    updatequery = "UPDATE StdtLessonPlan SET DSTempHdrId = " +
"(SELECT ISNULL(CASE WHEN (SELECT " + strReplace + " AND LookupName='Approved')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Approved'))" +
"ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='Pending Approval')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Pending Approval'))" +
"ELSE CASE WHEN (SELECT " + strReplace + "  AND LookupName='In Progress')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='In Progress'))" +
"ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='Maintenance')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Maintenance'))" +
"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Expired')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Expired') ORDER BY DSTempHdrId DESC)" +
"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Inactive')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Inactive') ORDER BY DSTempHdrId DESC)" +
"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Deleted')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName = 'Deleted') ORDER BY DSTempHdrId DESC)" +
"END END END END END END END, 0) AS DSTempHdrId) WHERE StdtIEPId = " + sess.IEPId + " AND IncludeIEP = 1 AND " +
  "LessonPlanId IN (SELECT DISTINCT LessonPlanId FROM StdtLessonPlan WHERE StdtIEPId = " + sess.IEPId + " AND IncludeIEP = 1) ";
                    objData.Execute(updatequery);

                    //End of code
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

        string strQuery = "Select 'IEP '+yr.AsmntYearDesc +' Version - '+ iep.Version AS Name,iep.StdtIEPId As ID from [dbo].[StdtIEP] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + sess.StudentId + "  And iep.StatusId In (" + ApproveId + ")  Order By iep.StdtIEPId Desc";

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

                    for (int iepPage = 0; iepPage < 12; iepPage++)
                    {
                        string ieppageid = row["ID"] + "-Page" + iepPage;


                        string[] IEPContents = { "Individualized Education", "Present Levels Of Educational", "Present Levels Of Educational", "Current Performance", "Service Delivery", "Non Participation justification", "State or District-Wide", "Additional Information", "Team Placement Page", "Administrative Data Sheet", "Attendance Sheet", "Signature Page" };
                        string[] IEPFunctions = { "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP1.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP2.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP3.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP4.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP5.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP6.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP7.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP8.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP9.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP10.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP11.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP12.aspx');" };


                        ul_ApprovedIEP.InnerHtml += "<div class='grmb' onclick=" + IEPFunctions[iepPage] + "><a style='width:100px;' href='#' id='" + ieppageid + "'  >" +
                                   "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis;' title='" + IEPContents[iepPage] + "'>" + IEPContents[iepPage] +
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
        if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " ") == true)
        {
            object PendingApproveId = objData.FetchValue("Select LookupId  from LookUp where LookupType='IEP Status'  And LookupName ='Pending Approval' group by LookupId");

            string strQuery = "Select 'IEP '+yr.AsmntYearDesc +' Version - '+ iep.Version AS Name,iep.StdtIEPId As ID from [dbo].[StdtIEP] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + sess.StudentId + "  And iep.StatusId =" + PendingApproveId + "  Order By iep.StdtIEPId Desc";

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

                        for (int iepPage = 0; iepPage < 12; iepPage++)
                        {
                            string ieppageid = row["ID"] + "-Page" + iepPage;


                            string[] IEPContents = { "Individualized Education", "Present Levels Of Educational", "Present Levels Of Educational", "Current Performance", "Service Delivery", "Non Participation justification", "State or District-Wide", "Additional Information", "Team Placement Page", "Administrative Data Sheet", "Attendance Sheet", "Signature Page" };
                            string[] IEPFunctions = { "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP1.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP2.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP3.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP4.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP5.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP6.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP7.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP8.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP9.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP10.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP11.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP12.aspx');" };


                            ul_PendingIEP.InnerHtml += "<div class='grmb' onclick=" + IEPFunctions[iepPage] + "><a style='width:100px;' href='#' id='" + ieppageid + "'  >" +
                                       "<div style='height: 20px; white-space: nowrap; text-overflow: ellipsis;' title='" + IEPContents[iepPage] + "'>" + IEPContents[iepPage] +
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
                    //        string[] IEPContents = { "Individualized Education", "Present Levels Of Educational", "Present Levels Of Educational", "Current Performance", "Service Delivery", "Non Participation justification", "State or District-Wide", "Additional Information" };
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
            string note = Convert.ToString(objData.FetchValue("SELECT ApprovalNotes FROM StdtIEP WHERE StdtIEPId='" + sess.IEPId + "'"));
            if (note == "")
            {
                notes = "Reject_&_" + txtReason.Text.Trim() + "_&_" + DateTime.Now.ToString();
            }
            else
            {
                notes = note + "_&_" + "Reject_&_" + txtReason.Text.Trim() + "_&_" + DateTime.Now.ToString();
            }
            string SQLQRY = "UPDATE StdtIEP set ApprovalNotes='" + notes + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='in progress'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEPId='" + sess.IEPId + "'";
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
            object chkappr = objData.FetchValue("select count(*) from StdtIEP where StatusId = (select LookupId from Lookup where LookupType = 'IEP Status' and LookupName = 'Approved') and StdtIEPId = " + sess.IEPId + "");
            if (Convert.ToInt32(chkappr) == 0)
            {
                ExportAll(out contents, out fileNameDoc);
                string contentType1 = "application/msword";
                objBinary.ShowDocument(fileNameDoc, contents, contentType1);
            }
            

            else
            {
                string fileName = "", contentType = "";
                string strQuery = "Select Data,ContentType,DocumentName from binaryFiles Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And IEPId=" + sess.IEPId + " And Type='BW'  And ModuleName='IEP' ORDER BY BinaryId DESC";
                byte[] bytes = objBinary.getDocument(strQuery, out contentType, out fileName);
                objBinary.ShowDocument(fileName, bytes, contentType);
            }


        }
        catch (Exception Ex)
        {
            // throw Ex;
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
        string strQuery = "select distinct  Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId=" + sess.IEPId + " and StdtLessonPlan.IncludeIEP=1";
        Dt = objData.ReturnDataTable(strQuery, false);
        string GoalIdZ = "";
        int countForGoalId = 0;
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
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
            }
        }


        if (Dt.Rows.Count == 0)
        {
            GoalIdZ = "0";
        }

        strQuery = "SELECT    dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT  TOP 1  IEPGoalNo FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + sess.IEPId + "  And StudentId=" + sess.StudentId + ") IEPGoalNo,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, dbo.Goal.GoalName   FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "' and StdtLessonPlan.IncludeIEP=1    ORDER BY IEPGoalNo";


        Dt = objData.ReturnDataTable(strQuery, false);
        System.Data.DataTable dtRep = new System.Data.DataTable();
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                dtRep = Dt;
            }
        }


        if (dtRep != null)
        {
            if (dtRep.Rows.Count > 0)
            {
                dtRep.Columns.Remove("StudentId");
                dtRep.Columns.Remove("AsmntYearId");
            }
        }
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


    private string replaceWithTextsOld(string HtmlData, string[] plcT, string[] TextT)
    {
        int count = plcT.Count();

        for (int i = 0; i < count; i++)
        {
            if (TextT[i] != null)
            {
                HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
            }
            else
            {
                HtmlData = HtmlData.Replace(plcT[i], "");
            }
        }
        return HtmlData;
    }


    public void replaceWithTexts(MainDocumentPart mainPart, string[] plcT, string[] TextT)
    {
        int count = plcT.Count();
        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
        for (int i = 0; i < count; i++)
        {
            string textData = "";
            if (TextT[i] != null)
            {
                textData = TextT[i];
            }
            else
            {
                textData = "";
            }

            var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);

            string textDataNoSpace = textData.Replace(" ", "");

            //if (textData != "")
            //{
            //    textData = Regex.Replace(textData, "^<p>.*?</p>", "");
            //}
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

    public void replaceWithTextsSingle(MainDocumentPart mainPart, string plcT, string TextT)
    {

        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);

        string textData = "";
        if (TextT != null)
        {
            textData = TextT;
        }
        else
        {
            textData = "";
        }

        var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);

        string textDataNoSpace = textData.Replace(" ", "");
        foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
        {
            var paragraphs = converter.Parse(textData);
            if (paragraphs.Count == 0)
            {
                DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                para.Parent.Append(tempPara);
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
            }
            //para.RemoveAllChildren<Run>();
        }
        paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);
        foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
        {
            para.RemoveAllChildren<Run>();
        }
    }


    public void ExportAllOld()
    {
        Hashtable ht = new Hashtable();
        clsDocumentasBinary objBinary = new clsDocumentasBinary();
        clsData objData = new clsData();

        ht = bindData();
        string[] plcT, TextT, plcC, chkC;

        string[] totChkBox = new string[130];

        string Path = Server.MapPath("~\\Administration\\IEPTemplates\\NAIEPTemplateDupe.docx");
        string TemporyPath = Server.MapPath("~\\Administration\\IEPTemp\\");

        string NewPath = CopyTemplate(Path, "0");
        System.Threading.Thread.Sleep(3000);
        int x = 0, count = 0, lastCount = 0;

        for (int k = 1; k < 13; k++)
        {
            CreateQuery1("NE", "..\\Administration\\XMlIEPS\\IEPCreations" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
            chkC.CopyTo(totChkBox, count);
            count += lastCount;
        }

        //setCheckBox(NewPath, ht, totChkBox);

        sess = (clsSession)Session["UserSession"];

        string HtmlFileName = "";
        string HtmlData = objBinary.ConvertToHtml(NewPath, TemporyPath, out HtmlFileName);
        System.Threading.Thread.Sleep(3000);
        string Temp = Server.MapPath("~\\Administration") + "\\XMlIEPS\\";

        string[] filePaths = Directory.GetFiles(Temp);
        string[] totCkeckColums = new string[0];

        for (int k = 1; k < 13; k++)
        {
            if (k != 4)
            {
                CreateQuery1("NE", "..\\Administration\\XMlIEPS\\IEPCreations" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                replaceWithTextsOld(HtmlData, plcT, TextT);
            }
        }

        System.Data.DataTable DTS = getGoalData4();
        clsGetGoalData objGoals = new clsGetGoalData();
        string goalHtml4 = "";

        if (DTS != null && DTS.Rows.Count > 0)
        {
            goalHtml4 = objGoals.getGoals(DTS, sess.IEPId, sess.StudentId);
            HtmlData = HtmlData.Replace("plcGoalSession", goalHtml4);
        }
        else
        {
            HtmlData = HtmlData.Replace("plcGoalSession", "");
        }
        sess = (clsSession)Session["UserSession"];

        string strquery = "";
        string fileName = "";

        strquery = "Select 'IEP Doc :'+ S.StudentLname +'-'+AY.AsmntYearDesc+'-V'+IE.Version from StdtIEP IE Inner Join AsmntYear AY on IE.AsmntYearId=AY.AsmntYearId INNER JOIN Student S ON IE.StudentId =S.StudentId  Where IE.StdtIEPId=" + sess.IEPId + " And IE.StudentId=" + sess.StudentId + " And IE.SchoolId=" + sess.SchoolId + " ";
        fileName = Convert.ToString(objData.FetchValue(strquery));
        string fileNameDoc = fileName + ".doc";

        strquery = "select version from stdtiep where  schoolid=" + sess.SchoolId + " and studentid=" + sess.StudentId + " and stdtiepid=" + sess.IEPId + " ";
        object ver = objData.FetchValue(strquery);
        string ver1 = "";
        if (ver != null) ver1 = Convert.ToString(ver);
        if (ver1 == "") ver1 = "0";
        byte[] contents = objBinary.ConvertToByte(HtmlData, TemporyPath, HtmlFileName);
        System.Threading.Thread.Sleep(4000);
        strquery = "select lookupid from LookUp where LookupName='IEP' ";
        object lookupId = objData.FetchValue(strquery);
        int lookupId1 = Convert.ToInt32(lookupId);
        int docId = objBinary.saveDoc(sess.SchoolId, sess.StudentId, sess.LoginId, lookupId1);
        int binaryid = objBinary.saveDocument(contents, fileNameDoc, ver1, "BW", docId, sess.IEPId, "IEP", sess.SchoolId, sess.StudentId, sess.LoginId);



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

        string[] totChkBox = new string[130];
        string Path = Server.MapPath("~\\Administration\\IEPTemplates\\NAIEPTemplateDupe.docx");
        string TemporyPath = Server.MapPath("~\\Administration\\IEPTemp\\");

        string NewPath = CopyTemplate(Path, "0");

        string strQuery = "Select  ST.StudentNbr AS IDNO, ST.StudentLname+','+ST.StudentFname as Name FROM  Student ST Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + "";
        Dt = objData.ReturnDataTable(strQuery, false);

        //Simplify the Markup Code Here...

        CleanDoc(NewPath);


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

        for (int k = 1; k < 13; k++)
        {
            CreateQuery1("NE", "..\\Administration\\XMlIEPS\\IEPCreations" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
            chkC.CopyTo(totChkBox, count);
            count += lastCount;
        }

        //commented on 07-11-2018
        //setCheckBox(NewPath, ht, totChkBox);

        sess = (clsSession)Session["UserSession"];

        //string HtmlFileName = "";
        //string HtmlData = objBinary.ConvertToHtml(NewPath, TemporyPath, out HtmlFileName);
        //System.Threading.Thread.Sleep(3000);
        string Temp = Server.MapPath("~\\Administration") + "\\XMlIEPS\\";

        string[] filePaths = Directory.GetFiles(Temp);
        string[] totCkeckColums = new string[0];

        #region New changes for IEP Export

        string filepath1 = "";
        string strFileName2 = "";
        string strFolderValue2 = "";
        string SAFilePath2 = "";
        filepath1 = Server.MapPath("~\\Administration\\IEPTemp\\FullHtml.html");
        System.IO.File.Delete(filepath1);
        strFileName2 = "FullHtml.html";
        strFolderValue2 = Server.MapPath("~\\Administration\\IEPTemp\\");
        SAFilePath2 = strFolderValue2.ToString() + strFileName2.ToString();
        SautinSoft.RtfToHtml r2 = new SautinSoft.RtfToHtml();
        r2.OpenDocx(NewPath);
        r2.OutputFormat = SautinSoft.RtfToHtml.eOutputFormat.HTML_5;
        r2.ToHtml(SAFilePath2);
        //DocumentCore dc = DocumentCore.Load(NewPath);
        //dc.Save(SAFilePath2);

        string target = "plcspace";
        string replace = "<p style=\"margin:0pt 0pt 10pt 0pt;line-height:0;page-break-before: always;\"><span class=\"st2\">.</span></p>";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "plcspace";
        replace = "<p style=\"margin:0pt 0pt 10pt 0pt;line-height:0;page-break-before: always;\"><span class=\"st2\">.</span><span class=\"st20\"> </span></p>";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target="margin:0pt 0pt 10pt 0pt;";
        replace = "margin:0pt 0pt 4pt 0pt;";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "vertical-align:top;width:521.05pt;border:solid 1pt #000000;padding:0pt 5.4pt 0pt 5.4pt;";
        replace = "";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st1\" style=\"background:#000000;\">A. Consultation";
        replace = "background-color:black; color:white;\"><span class=\"st1\" style=\"background:#000000;\">A. Consultation";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st1\" style=\"background:#000000;\">B. Special Education";
        replace = "background-color:black; color:white;\"><span class=\"st1\" style=\"background:#000000;\">B. Special Education";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st1\" style=\"background:#000000;\">C. Special Education";
        replace = "background-color:black; color:white;\"><span class=\"st1\" style=\"background:#000000;\">C. Special Education";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st6\">CONTENT AREAS";
        replace = "background-color:black; color:white;font-family:Helvetica;font-size:8pt;font-weight:bold\"><span>CONTENT AREAS";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st6\">COLUMN 1";
        replace = "background-color:black; color:white;font-family:Helvetica;font-size:8pt;font-weight:bold\"><span>COLUMN 1";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st6\">COLUMN 2";
        replace = "background-color:black; color:white;font-family:Helvetica;font-size:8pt;font-weight:bold\"><span>COLUMN 2";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        target = "\"><span class=\"st6\">COLUMN 3";
        replace = "background-color:black; color:white;font-family:Helvetica;font-size:8pt;font-weight:bold\"><span>COLUMN 3";
        ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        //target = "width:513pt;border-left:solid 1pt";
        //replace = "width:513pt;border-left:solid 0pt";
        //ReplaceInFile(SAFilePath2, replace.ToString(), target.ToString());
        #endregion


        using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
        {
            for (int k = 1; k <= 12; k++)
            {
                //if (k == 2)
                //{
                //    //07-11-2018
                //    //setCheckBox(SAFilePath2, ht, totChkBox, k);
                //}
                if (k != 4)
                {
                    CreateQuery1("NE", "..\\Administration\\XMlIEPS\\IEPCreations" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                    //replaceWithTexts(theDoc.MainDocumentPart, plcT, TextT);
                    ReplaceInFileArray(SAFilePath2, TextT, plcT);
                    if (k == 2 || k == 3 || k == 5 || k == 6 || k == 7 || k == 8)
                    {
                        setCheckBox(SAFilePath2, ht, totChkBox, k);
                    }
                }
            }
        }

        System.Data.DataTable DTS = getGoalData4();
        clsGetGoalData objGoals = new clsGetGoalData();
        string goalHtml4 = "";

        using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
        {
            if (DTS != null && DTS.Rows.Count > 0)
            {
                goalHtml4 = objGoals.getGoals(DTS, sess.IEPId, sess.StudentId);
                //replaceWithTextsSingle(theDoc.MainDocumentPart, "plcGoalSession", goalHtml4);
                //HtmlData = HtmlData.Replace("plcGoalSession", goalHtml4);//replaceWithTextsSingle
            }
            else
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "plcGoalSession", "");
            }
        }
        sess = (clsSession)Session["UserSession"];

        string strquery = "";
        string fileName = "";

        strquery = "Select 'IEP Doc :'+ S.StudentLname +'-'+AY.AsmntYearDesc+'-V'+IE.Version from StdtIEP IE Inner Join AsmntYear AY on IE.AsmntYearId=AY.AsmntYearId INNER JOIN Student S ON IE.StudentId =S.StudentId  Where IE.StdtIEPId=" + sess.IEPId + " And IE.StudentId=" + sess.StudentId + " And IE.SchoolId=" + sess.SchoolId + " ";
        fileName = Convert.ToString(objData.FetchValue(strquery));
        fileNameDoc = fileName + ".doc";

        strquery = "select version from stdtiep where  schoolid=" + sess.SchoolId + " and studentid=" + sess.StudentId + " and stdtiepid=" + sess.IEPId + " ";
        object ver = objData.FetchValue(strquery);
        string ver1 = "";
        if (ver != null) ver1 = Convert.ToString(ver);
        if (ver1 == "") ver1 = "0";

        // convert the main document to html
        string filepath = Server.MapPath("~\\Administration\\IEPTemp\\goalHtml4.html");
        System.IO.File.Delete(filepath);
        string strFileName = "goalHtml4.html";
        string strFolderValue = Server.MapPath("~\\Administration\\IEPTemp\\");
        string SAFilePath = strFolderValue.ToString() + strFileName.ToString();
        string strFileName1 = "goalHtml4.doc";
        string strFolderValue1 = Server.MapPath("~\\Administration\\IEPTemp\\");
        string SAFilePath1 = strFolderValue1.ToString() + strFileName1.ToString();
        SautinSoft.RtfToHtml r1 = new SautinSoft.RtfToHtml();
        //replace the placeholder with the new html code
        ReplaceInFile(SAFilePath2, goalHtml4.ToString(), "plcGoalSession");
        //read full html content from the page
        WebClient MyWebClient = new WebClient();
        Byte[] PageHTMLBytes;
        PageHTMLBytes = MyWebClient.DownloadData(SAFilePath2);
        UTF8Encoding oUTF8 = new UTF8Encoding();
        string test = oUTF8.GetString(PageHTMLBytes);
        File.WriteAllText(SAFilePath2, test);

        contents = System.IO.File.ReadAllBytes(SAFilePath2);

        //contents = System.IO.File.ReadAllBytes(NewPath);
        // byte[] contents = objBinary.ConvertToByte(HtmlData, TemporyPath, HtmlFileName);
        System.Threading.Thread.Sleep(4000);
        strquery = "select lookupid from LookUp where LookupName='IEP' ";
        object lookupId = objData.FetchValue(strquery);
        int lookupId1 = Convert.ToInt32(lookupId);
        int docId = objBinary.saveDoc(sess.SchoolId, sess.StudentId, sess.LoginId, lookupId1);
        object chkapprov = objData.FetchValue("select count(*) from StdtIEP where StatusId = (select LookupId from Lookup where LookupType = 'IEP Status' and LookupName = 'Approved') and StdtIEPId = " + sess.IEPId + "");
        if (Convert.ToInt32(chkapprov) == 1)
        {
            int binaryid = objBinary.saveDocument(contents, fileNameDoc, ver1, "BW", docId, sess.IEPId, "IEP", sess.SchoolId, sess.StudentId, sess.LoginId);
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
        int RowsCount = 0; if (dtIEP4 != null) { RowsCount = dtIEP4.Rows.Count; dtIEP4.TableName = "Table"; }



        for (int Round = 0; Round < RowsCount; Round += 2)
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
        objExport.getIEP9(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(9))
        {
            htColumns.Add(9, data2);
            data2 = null;
        }
        objExport.getIEP10(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(9))
        {
            htColumns.Add(9, data2);
            data2 = null;
        }
        objExport.getIEP11(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(9))
        {
            htColumns.Add(9, data2);
            data2 = null;
        }
        objExport.getIEP12(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(9))
        {
            htColumns.Add(9, data2);
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

        if (PageNo == 4) return;
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
        if (PageNo == 1) objExport.getIEP1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 2) objExport.getIEP2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 3) objExport.getIEP3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (PageNo == 4)
        {
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

        }

        if (PageNo == 5) objExport.getIEP5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 6) objExport.getIEP6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (PageNo == 7) objExport.getIEP7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 8) objExport.getIEP8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 9) objExport.getIEP9(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 10) objExport.getIEP10(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 11) objExport.getIEP11(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 12) objExport.getIEP12(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
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

    private void setCheckBox(string document, Hashtable ht, string[] columnsChks, int nmbr)
    {
        string strQuery = "";
        objData = new clsData();
        DataTable Dt = new DataTable();
        if (nmbr == 2)
        {
            strQuery = "SELECT TOP 1  IE1.EngLangInd, IE1.HistInd, IE1.TechInd, IE1.MathInd, IE1.OtherInd, IE1.ContentModInd, IE1.MethodModInd, IE1.PerfModInd ";
            strQuery = strQuery + "FROM  StdtIEPExt1 IE1 Inner Join StdtIEP IE ON IE.StdtIEPId=IE1.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId ";
            strQuery += "where Sc.SchoolId=" + sess.SchoolId + " And S.StudentId=" + sess.StudentId + " And IE1.StdtIEPId=" + sess.IEPId + "  ";
        }
        if (nmbr == 3)
        {
            strQuery = "SELECT TOP 1 PEInd, TechDevicesInd, BehaviorInd, BrailleInd, CommInd, CommDfInd, ExtCurInd, LEPInd, NonAcdInd, SocialInd, TravelInd, VocInd, OtherInd, ";
            strQuery += "AgeBand1Ind, AgeBand2Ind, AgeBand3Ind, ContentModInd, MethodModInd,PerfModInd FROM StdtIEPExt2 IE2 ";
            strQuery += "Inner Join StdtIEP IE ON IE.StdtIEPId=IE2.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.StudentId=IE.StudentId ";
            strQuery += "where Sc.SchoolId=" + sess.SchoolId + " And S.StudentId=" + sess.StudentId + " And IE2.StdtIEPId=" + sess.IEPId + "  ";
        }
        if(nmbr==5)
        {
            strQuery = "SELECT TOP 1 StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther";
            strQuery += " from StdtIEP inner join Student on Student.StudentId=StdtIEP.StudentId ";
            strQuery += " inner join StdtIEPExt2 on StdtIEPExt2.StdtIEPId=StdtIEP.StdtIEPId ";
            strQuery += " where StdtIEP.StdtIEPId=" + sess.IEPId + " AND StdtIEP.SchoolId =" + sess.SchoolId + " AND StdtIEP.StudentId = " + sess.StudentId + " ";
        }
        if (nmbr == 6)
        {
            strQuery = "SELECT TOP 1 IEP3.RemovedInd1,IEP3.RemovedInd2,IEP3.ShorterCd1,IEP3.ShorterCd2,IEP3.ShorterCd3,IEP3.LongerCd1,IEP3.LongerCd2,IEP3.LongerCd3,IEP3.TransportInd1,IEP3.TransportInd2,IEP3.RegTransInd,IEP3.SpTransInd ";
            strQuery += " from StdtIEPExt3 IEP3  INNER JOIN StdtIEP IEP  ON IEP.StdtIEPId  =IEP3.StdtIEPId   ";
            strQuery += "Inner Join School Sc On IEP.SchoolId=Sc.SchoolId Inner Join Student St1 On IEP.StudentId=St1.StudentId ";
            strQuery += "where IEP.SchoolId=" + sess.SchoolId + " And IEP.StudentId=" + sess.StudentId + " And IEP.StdtIEPId=" + sess.IEPId + "";
        }
        if (nmbr == 7)
        {
            strQuery = "SELECT TOP 1 EngCol1,EngCol2,EngCol3,HistCol1,HistCol2 ,HistCol3,MathCol1,MathCol2,MathCol3 ,TechCol1,TechCol2,TechCol3,ReadCol1,ReadCol2,ReadCol3 ";
            strQuery += "from StdtIEPExt3 E3 Inner Join StdtIEP IE ON IE.StdtIEPId=E3.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId ";
            strQuery += "where Sc.SchoolId=" + sess.SchoolId + " And S.StudentId=" + sess.StudentId + "  And IE.StdtIEPId=" + sess.IEPId + " ";
        }
        if (nmbr == 8)
        {
            strQuery = "SELECT TOP 1 AddInfoCol1,AddInfoCol2,AddInfoCol3 from StdtIEPExt3 IE3 Inner Join StdtIEP IE ON IE.StdtIEPId=IE3.StdtIEPId Inner Join School Sc On ";
            strQuery += "IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId  ";
            strQuery += "where Sc.SchoolId=" + sess.SchoolId + " And S.StudentId=" + sess.StudentId + "  And IE.StdtIEPId=" + sess.IEPId + "  ";
        }


        Dt = objData.ReturnDataTable(strQuery, false);
        var CheckArray = Dt.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
        //if (nmbr == 5)
        //{
        //    for (int i = 0; i <= Dt.Columns.Count - 1; i++)
        //    {

        //        string target = nmbr + "Check" + i + "plc";
        //        string replace = "";

        //        replace = "<span style=\"margin:6pt 0pt 0pt 0pt;line-height:1.15; border-width:1px; border-style:solid; width:15px;\">✔</span>";
        //    }
        //}
        //else
        //{
        for (int i = 0; i <= Dt.Columns.Count - 1; i++)
        {
            string target = nmbr + "Check" + i + "plc";
            string replace = "";
            if (CheckArray[i] == "True" || CheckArray[i] == "1")
            {
                replace = "<span style=\"margin:6pt 0pt 0pt 0pt;line-height:1.15; border-width:1px; border-style:solid; width:15px;\">✔</span>";
            }
            else
            {
                //replace = "<span style=\"margin:6pt 0pt 0pt 0pt;line-height:1.15; border-width:1px; border-style:solid; width:15px;\"></span>";
                replace = "<span style=\"margin:6pt 0pt 0pt 0pt;line-height:1.15; border-width:1px; border-style:solid; width:15px; color:white\">✔</span>";
            }
            ReplaceInFile(document, replace.ToString(), target.ToString());
        }
        //}

        #region commented on 07-11-2018
        //bool IsCheck = false;
        //int i = 0;
        //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        //{

        //    foreach (DocumentFormat.OpenXml.Wordprocessing.CheckBox cb in wordDoc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.CheckBox>())
        //    {
        //        if (cb != null)
        //        {

        //            FormFieldName cbName = cb.Parent.ChildElements.First<FormFieldName>();

        //            try
        //            {
        //                DefaultCheckBoxFormFieldState defaultState = cb.GetFirstChild<DefaultCheckBoxFormFieldState>();
        //                if (i < columnsChks.Length)
        //                {
        //                    if (columnsChks[i] != "") IsCheck = Convert.ToBoolean(columnsChks[i]);
        //                    if (IsCheck == true) defaultState.Val = true;
        //                }
        //                i++;
        //            }
        //            catch
        //            {

        //            }
        //        }
        //    }
        //}
        #endregion
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

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        tdMsgExport.InnerHtml = "";
        string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
        Array.ForEach(Directory.GetFiles(path), File.Delete);
        ClientScript.RegisterStartupScript(GetType(), "", "DownloadDone();", true);

    }




    public string MergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\Administration") + "\\Temp\\";
            string Temp1 = Server.MapPath("~\\Administration") + "\\IEPMerged\\";
            const string DOC_URL = "/word/document.xml";

            string FolderName = "\\IEP_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FolderName;
            string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string OUTPUT_FILE = path + "\\IEP_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.docx";
            string FIRST_PAGE = Server.MapPath("~\\Administration\\IEPTemplates\\Dummy.docx");

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            File.Copy(FIRST_PAGE, fileName);

            string[] filePaths = Directory.GetFiles(Temp);

            int i = 1;
            for (int j = filePaths.Length - 1; j >= 0; j--)
            {
                makeWord(filePaths[j], fileName, i);
                i++;
            }

            ViewState["FileName"] = fileName;
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }


            return FolderName;

        }
        catch (Exception Ex)
        {
            return "";
        }
    }
    public void makeWord(string filenamePass, string fileName1, int i)
    {

        using (WordprocessingDocument myDoc =
            WordprocessingDocument.Open(fileName1, true))
        {
            string altChunkId = "AltChunkId" + i.ToString();
            MainDocumentPart mainPart = myDoc.MainDocumentPart;
            AlternativeFormatImportPart chunk =
                mainPart.AddAlternativeFormatImportPart(
                AlternativeFormatImportPartType.WordprocessingML, altChunkId);


            using (FileStream fileStream = File.Open(filenamePass, FileMode.Open))
                chunk.FeedData(fileStream);


            AltChunk altChunk = new AltChunk();
            altChunk.Id = altChunkId;
            mainPart.Document
                .Body
                .InsertAfter(altChunk, mainPart.Document.Body
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last());
            mainPart.Document.Save();
        }
    }


    protected void lnkApprovedIEP_Click(object sender, EventArgs e)
    {

    }

    protected void lnkPendingIEP_Click(object sender, EventArgs e)
    {

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
        lbl_CurrYear.Text = objData.FetchValue("Select TOP 1 yr.AsmntYearDesc +' Version - '+ iep.Version AS Name,iep.StdtIEPId As ID from [dbo].[StdtIEP] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + sess.StudentId + "  And iep.SchoolId=" + sess.SchoolId + " Order By iep.StdtIEPId Desc").ToString();

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadData();
    }
    [WebMethod]
    protected void lnkBtnSignDetails_Click(object sender, EventArgs e)
    {


    }

    [WebMethod]
    public static string getSignedUser()
    {
        DataClass oData = new DataClass();
        clsData objData2 = new clsData();
        System.Data.DataTable Dt = null;
        clsSession sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int ApproveStatus = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        string checkIEP = "Update StdtIEP set StatusId=" + ApproveStatus + " where StdtIEPId=" + sess.IEPId + "";
        int result = oData.ExecuteNonQuery(checkIEP);
        string name = "";
        int rowIndex = 0;
        if (result > 0)
        {
            string temp = "";
            string Querry = "select BF.BinaryId,SD.UserId,SD.UserType from binaryFiles BF inner join SignDetails SD on BF.BinaryId=SD.BinaryId  where DocId=" + sess.IEPId + " and" +
                " SD.StudentId=" + sess.StudentId;
            Dt = objData2.ReturnDataTable(Querry, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        temp = "";
                        if (Dt.Rows[rowIndex]["UserType"].ToString() == "Parent")
                        {
                            Querry = " select  Lname+','+Fname as UserName from Parent where ParentID=" + Convert.ToInt32(Dt.Rows[rowIndex]["UserId"]);
                            temp = objData2.FetchValue(Querry).ToString();
                        }
                        if (Dt.Rows[rowIndex]["UserType"].ToString() != "Parent")
                        {
                            Querry = " select UserLName +','+ UserFName as UserName from [User] where UserId=" + Convert.ToInt32(Dt.Rows[rowIndex]["UserId"]);
                            temp = objData2.FetchValue(Querry).ToString();
                        }
                        name = name + "<div class='grmb' >" + temp + "</div></br>";
                        rowIndex++;
                    }
                }
            }
        }
        return name;
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
            int inprogressId = Convert.ToInt32(objData.FetchValueTrans("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress') ORDER BY StdtIEPId DESC ", Transs, con));
            if (objData.IFExistsWithTranss("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Expired') ORDER BY StdtIEPId DESC ", Transs, con))
            {
                PrevIEPId = Convert.ToInt32(objData.FetchValueTrans("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Expired') ORDER BY StdtIEPId DESC", Transs, con));
                string PrevIEP = "UPDATE StdtIEP SET StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved') WHERE StdtIEPId='" + PrevIEPId + "'";
                objData.ExecuteWithTrans(PrevIEP, con, Transs);
            }
            string DeleteIEP = "DELETE FROM StdtIEP WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEP, con, Transs);
            string DeleteIEPExt1 = "DELETE FROM StdtIEPExt1 WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt1, con, Transs);
            string DeleteIEPExt2 = "DELETE FROM StdtIEPExt2 WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt2, con, Transs);
            string DeleteIEPExt3 = "DELETE FROM StdtIEPExt3 WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt3, con, Transs);
            string DeleteIEPExt4 = "DELETE FROM StdtIEPExt4 WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt4, con, Transs);
            string DeleteIEPExt5 = "DELETE FROM StdtIEPExt5 WHERE StdtIEPId='" + inprogressId + "'";
            objData.ExecuteWithTrans(DeleteIEPExt5, con, Transs);

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
                                    StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP WHERE StdtIEPId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') THEN 0 ELSE 1 END AS STATUS";
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
                                    StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP WHERE StdtIEPId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "') THEN 0 ELSE 1 END AS STATUS";
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
                                        StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP WHERE StdtIEPId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtGoal WHERE StdtGoalId='" + stdtGoalid + "') THEN 0 ELSE 1 END AS STATUS";
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
                                        StrQuery = "SELECT CASE WHEN (SELECT CreatedOn FROM StdtIEP WHERE StdtIEPId='" + inprogressId + "')>(SELECT CreatedOn FROM StdtLessonPlan WHERE StdtLessonPlanId='" + dtlessongoal.Rows[0]["StdtLessonPlanId"] + "') THEN 0 ELSE 1 END AS STATUS";
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

    protected void btnUploadBSP_Click(object sender, EventArgs e)
    {
        divMessage.InnerHtml = "";
        if (sess != null)
        {
            int headerId = sess.IEPId;
            FillDoc(headerId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
        }

    }
    protected void grdFile_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
        grdFile.DataBind();

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
        grdFile.PageIndex = e.NewPageIndex;
        int headerId = sess.IEPId;
        if (headerId != 0)
        {
            FillDoc(headerId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
        }

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
                        //string strquerry = "INSERT INTO BSPDoc(SchoolId,StdtIEPId,BSPDocUrl,Data,CreatedBy,CreatedOn) values(" + sess.SchoolId + "," + sess.IEPId + ",'" + filename + "',convert(varbinary(max),'"+myData+"')," + sess.LoginId + ",GETDATE())";
                        //int docid = objData.ExecuteWithScope(strquerry);
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
            //string name = System.IO.Path.GetFileNameWithoutExtension(fupDoc.FileName);
            strQuery = "Select ROW_NUMBER() OVER (ORDER BY BSPDoc) AS No,BSPDocUrl as Document, BSPDoc FROM BSPDoc Where BSPDocUrl<>'' And StdtIEPId = " + stdtIEPID + "";
            System.Data.DataTable Dt = objData.ReturnDataTable(strQuery, false);
            Dt.Columns.Add("Name", typeof(string));

            if (Dt != null)
            {
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
        }
        catch (Exception)
        {


        }
    }

    #endregion
    #region Export to word
    public void ReplaceInFile(string filepath, string replacetext, string findtext)
    {
        try
        {
            System.IO.StreamReader objReader;
            objReader = new StreamReader(filepath);
            string content = objReader.ReadToEnd();
            objReader.Close();
            content = Regex.Replace(content, findtext, replacetext);
            StreamWriter writer = new StreamWriter(filepath);
            writer.Write(content);
            writer.Close();
        }
        catch
        { }
    }
    public void ReplaceInFileArray(string filepath, string[] replacetext, string[] findtext)
    {
        try
        {
            System.IO.StreamReader objReader;
            objReader = new StreamReader(filepath);
            string content = objReader.ReadToEnd();
            objReader.Close();
            for (int k = 0; k <= findtext.Length - 1; k++)
            {
                content = Regex.Replace(content, findtext[k], replacetext[k]);
            }
            StreamWriter writer = new StreamWriter(filepath);
            writer.Write(content);
            writer.Close();
        }
        catch
        { }
    }
    #endregion
    protected void btnUpdateandExport_Click(object sender, EventArgs e)
    {
        btnUpdateandExport.Enabled = false;
        btnUpdateandExport.Attributes.Add("cssClass", "ExportWordDis");
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        tdMsgMain.InnerHtml = "";
        clsDocumentasBinary objBinary = new clsDocumentasBinary();

        try
        {
            byte[] contents;
            string fileNameDoc = "";
            ExportAll(out contents, out fileNameDoc);
            string contentType1 = "application/msword";
            objBinary.ShowDocument(fileNameDoc, contents, contentType1);

        }
        catch (Exception Ex)
        {
            // throw Ex;
        }
        finally
        {
            btnUpdateandExport.Enabled = true;
            btnUpdateandExport.Attributes.Add("cssClass", "ExportWord");
        }
    }
}