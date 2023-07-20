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

public partial class StudentBinder_CreatePEIEP : System.Web.UI.Page
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
    // static string strQuery = "";
    //  bool Disable = false;
    static System.Data.DataTable dtIEP4 = null;




    protected void Page_Load(object sender, EventArgs e)
    {
        //sess = (clsSession)Session["UserSession"];
        //if (sess == null)
        //{
        //    Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        //}
        //else
        //{
        //    bool flag = clsGeneral.PageIdentification(sess.perPage);
        //    if (flag == false)
        //    {
        //        Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
        //    }
        //}
        //if (!IsPostBack)
        //{
        //    LoadData();
        //}

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
        dttmp = objData.ReturnDataTable("Select TOP 1 StdtIEPId,CONVERT(varchar(10), EffStartDate,101) +' - '+ CONVERT(varchar(10), EffEndDate,101) as iepDate,StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " And StatusId =(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress') ORDER BY StdtIEPId DESC ", true);
        sess.IEPId = int.Parse(dttmp.Rows[0][0].ToString());
        lnkBtnIEPYear.Text = dttmp.Rows[0][1].ToString();
        hdnFieldIep.Value = dttmp.Rows[0][0].ToString();
        sess.IEPStatus = int.Parse(dttmp.Rows[0][2].ToString());
        hdnFieldIep.Value = sess.IEPId.ToString();
        //lnkBtnIEPYear.Visible = true;
        //lnkBtnIndiviEdu.Visible = true;
        //lnkBtnPrestLvlEdu.Visible = true;
        //lnkBtnPrestLvlEdu2.Visible = true;
        //lnkBtnCurPre.Visible = true;
        //LnkBtnSerDel.Visible = true;
        //lnkBtnNonParJus.Visible = true;
        //lnkBtnStateOfDist.Visible = true;
        //lnkBtnAddInfo.Visible = true;
    }

    private void NOInprogressIEP()
    {
        //lnkBtnIEPYear.Visible = false;
        //lnkBtnIndiviEdu.Visible = false;
        //lnkBtnPrestLvlEdu.Visible = false;
        //lnkBtnPrestLvlEdu2.Visible = false;
        //lnkBtnCurPre.Visible = false;
        //LnkBtnSerDel.Visible = false;
        //lnkBtnNonParJus.Visible = false;
        //lnkBtnStateOfDist.Visible = false;
        //lnkBtnAddInfo.Visible = false;
    }
    private void LoadData()
    {
        lnkBtnIEPYear.Visible = false;
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
                    createIepFunc();
                }
                else
                {
                    tdMessage.InnerHtml = clsGeneral.failedMsg("IEP Already Exist in Pending State");
                    //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);

                }
            }
            else
            {
                createIepFunc();
            }



        }
        if (hdFieldSucc.Value == "0")
        {
            string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        }
    }


    public void createIepFunc()
    {
        objData = new clsData();

        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DataClass oData = new DataClass();
        oSession = (clsSession)Session["UserSession"];
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
                if (objData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + "  ") == true) //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
                {
                    version = objData.FetchValue("Select TOP 1 Version from StdtIEP where StudentId=" + oSession.StudentId + " and SchoolId=" + oSession.SchoolId + "  ORDER BY StdtIEPId DESC ").ToString(); //And AsmntYearId='" + Convert.ToInt32(ddlYear.SelectedValue) + "'
                }
                if (version == "") version = "1.0";
                else
                {
                    Double ver = Convert.ToDouble(version) + 1; version = Convert.ToString(ver) + ".0";
                }

                if (Oldiep > 0)
                {
                    string insOLDIEP = "INSERT INTO StdtIEP([SchoolId],[StudentId],[AsmntYearId],[StatusId],[EffStartDate],[EffEndDate],[Concerns],[Strength],[Vision],[Version],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                              "Select " + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'),'" + IEPstatus + "','" + dtst.ToString("yyyy-MM-dd") + "','" + dted.ToString("yyyy-MM-dd") + "',[Concerns],[Strength],[Vision],'" + version + "'," + //" + Convert.ToInt32(ddlYear.SelectedValue) + "
                              "" + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtIEP where StdtIEPId='" + Oldiep + "'\t\n" +
                              "SELECT SCOPE_IDENTITY()";
                    IEP_id = oData.ExecuteScalar(insOLDIEP);
                }
                else
                {
                    string insIEP = "INSERT INTO StdtIEP(SchoolId,StudentId,AsmntYearId,Version,EffStartDate,EffEndDate,StatusId,CreatedBy,CreatedOn) " +
                              "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + ",(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'),'" + version + "','" + dtst.ToString("yyyy-MM-dd") + "'," +
                              "'" + dted.ToString("yyyy-MM-dd") + "'," + IEPstatus + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                              "SELECT SCOPE_IDENTITY()";
                    IEP_id = oData.ExecuteScalar(insIEP);
                }
                if (IEP_id > 0)
                {
                    if (Oldiep > 0)
                    {
                        string InsIEPONE = "Insert into StdtIEPExt1([StdtIEPId],[EngLangInd],[HistInd],[TechInd],[MathInd],[OtherInd],[OtherDesc],[AffectDesc],[AccomDesc],[ContentModInd]," +
                        "[ContentModDesc],[PerfModInd],[PerfModDesc],[StatusId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) Select '" + IEP_id + "',[EngLangInd],[HistInd],[TechInd],[MathInd],[OtherInd],[OtherDesc],[AffectDesc],[AccomDesc],[ContentModInd]," +
                        "[ContentModDesc],[PerfModInd],[PerfModDesc],'" + IEPstatus + "','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate() from StdtIEPExt1 Where [StdtIEPId]='" + Oldiep + "'";
                        oData.ExecuteNonQuery(InsIEPONE);

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
                    }
                }
                //}

                if (IEP_id > 0)
                {
                    oSession = (clsSession)Session["UserSession"];
                    SqlTransaction Trans = null;
                    SqlConnection Con = new SqlConnection();
                    clsData clsData = new clsData();
                    try
                    {
                        if (Oldiep > 0)
                        {
                            Con = clsData.Open();
                            Trans = Con.BeginTransaction();
                            System.Data.DataTable dtgoal = new System.Data.DataTable();
                            System.Data.DataTable dtLP = new System.Data.DataTable();
                            string Oldgoaldata = "SELECT GoalId,StdtGoalId FROM StdtGoal WHERE StdtIEPId=" + Oldiep + "";
                            dtgoal = clsData.ReturnDataTableWithTransaction(Oldgoaldata, Con, Trans, false);
                            if (dtgoal != null)
                            {
                                if (dtgoal.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtgoal.Rows)
                                    {
                                        string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,[CurPerfLevel],[AnnualGoalDesc],[Objective1],[Objective2],[Objective3],CreatedBy,CreatedOn) " +
                                                         "select " + oSession.SchoolId + "," + oSession.StudentId.ToString() + "," + Convert.ToInt32(row["GoalId"]) + "," + IEP_id + ",(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A')," +
                                                         "" + GlstatusID + ",'A',1,[CurPerfLevel],[AnnualGoalDesc],[Objective1],[Objective2],[Objective3]," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)) from StdtGoal Where StdtIEPId='" + Oldiep + "' AND GoalId='" + Convert.ToInt32(row["GoalId"]) + "'\t\n" +
                                                         "SELECT SCOPE_IDENTITY()";
                                        int newgoalid = clsData.ExecuteWithScopeandConnection(insGoal, Con, Trans);


                                        string stdtgoalid = "SELECT StdtGoalSvcId FROM StdtGoalSvc WHERE StdtGoalId=" + Convert.ToInt32(row["StdtGoalId"]) + " ";
                                        int goalsvcid = oData.ExecuteScalar(stdtgoalid);
                                        if (goalsvcid > 0)
                                        {
                                            System.Data.DataTable dtgoalsvc = new System.Data.DataTable();
                                            dtgoalsvc = clsData.ReturnDataTableWithTransaction(stdtgoalid, Con, Trans, false);
                                            if (dtgoalsvc.Rows.Count > 0)
                                            {
                                                foreach (DataRow svc in dtgoalsvc.Rows)
                                                {
                                                    string goalsvc = "INSERT INTO StdtGoalSvc (StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,CreatedBy,CreatedOn,StdtIEPId,[ModifiedBy],[ModifiedOn]) " +
                                                         "Select '" + newgoalid + "',SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,'" + oSession.LoginId + "',getdate(),'" + IEP_id + "','" + oSession.LoginId + "',getdate() From StdtGoalSvc Where StdtGoalSvcId='" + Convert.ToInt32(svc["StdtGoalSvcId"]) + "'";
                                                    clsData.ExecuteWithScopeandConnection(goalsvc, Con, Trans);
                                                }
                                            }
                                        }
                                    }
                                }
                            }




                            string OldLPdata = "SELECT LessonPlanId,GoalId FROM StdtLessonPlan WHERE StdtIEPId=" + Oldiep + "";
                            dtLP = clsData.ReturnDataTableWithTransaction(OldLPdata, Con, Trans, false);
                            if (dtLP != null)
                            {
                                if (dtLP.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtLP.Rows)
                                    {

                                        clsAssignLessonPlan objAssign = new clsAssignLessonPlan();
                                        System.Data.DataTable LessonPlan = objData.ReturnDataTable("SELECT LessonPlanName from LessonPlan where LessonPlanId=" + row["LessonPlanId"] + "", false);
                                        string insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,StdtIEPId,AsmntYearId,StatusId,ActiveInd,IncludeIEP,CreatedBy,CreatedOn) " +
                                                        "VALUES(" + oSession.SchoolId + "," + oSession.StudentId.ToString() + "," + Convert.ToInt32(row["LessonPlanId"]) + "," + Convert.ToInt32(row["GoalId"]) + "," + IEP_id + "," +
                                                        "(SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A')," + LPstatusID + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                        int lpID = clsData.ExecuteWithScopeandConnection(insLP, Con, Trans);
                                        objAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(row["LessonPlanId"]), LessonPlan.Rows[0]["LessonPlanName"].ToString(), oSession.LoginId, lpID, Con, Trans);

                                    }
                                }
                            }
                            objData.ExecuteWithScopeandConnection("update StdtIEP set StatusId=(select LookupId from lookup where LookupType='IEP Status' and LookupName='Expired') where StdtIEPId=" + Oldiep, Con, Trans);
                            objData.ExecuteWithScopeandConnection("update StdtGoal set ActiveInd='D' where IncludeIEP=1 and StdtIEPId=" + Oldiep, Con, Trans);
                            objData.ExecuteWithScopeandConnection("update StdtLessonPlan set ActiveInd='D' where IncludeIEP=1 and StdtIEPId=" + Oldiep, Con, Trans);
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


    public void checkIepEditCreateStatus(bool permission)
    {
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
                txtSdate.Text = DateTime.Now.Date.ToString("MM'-'dd'-'yyyy");
                DateTime Edate = DateTime.Now.Date.AddMonths(12);
                txtEdate.Text = Edate.Date.ToString("MM'-'dd'-'yyyy");
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
                    returnVal = dt.Rows[0]["Page1"] + ":" + dt.Rows[0]["Page2"] + ":" + dt.Rows[0]["Page3"] + ":" + dt.Rows[0]["Page4"] + ":" + dt.Rows[0]["Page5"] + ":" + dt.Rows[0]["Page6"] + ":" + dt.Rows[0]["Page7"] + ":" + dt.Rows[0]["Page8"];
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
        FillPendingIEP();
        lnkBtnIEPYear.Text = "";
    }
    //public void setSubmitAprove()
    //{
    //    object pendstatus = null;
    //    object IepStatus = null;
    //    objData = new clsData();
    //    sess = (clsSession)Session["UserSession"];
    //    pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + "").ToString();


    //    if (int.Parse(pendstatus.ToString()) > 0)
    //    {
    //        sess.IEPStatus = int.Parse(pendstatus.ToString());
    //        IepStatus = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));

    //        if (IepStatus.ToString() == "In Progress")
    //        {
    //            lnkBtnAprove.Visible = false;
    //            lnkBtnSubmit.Visible = true;
    //        }
    //        else if (IepStatus.ToString() == "Pending Approval")
    //        {
    //            lnkBtnAprove.Visible = true;
    //            lnkBtnSubmit.Visible = false;
    //        }
    //        else if (IepStatus.ToString() == "Approve")
    //        {
    //            lnkBtnAprove.Visible = false;
    //            lnkBtnSubmit.Visible = false;
    //        }
    //    }

    //}
    protected void lnkBtnAprove_Click(object sender, EventArgs e)
    {

        DataClass oData = new DataClass();
        int statusID;
        statusID = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        tdMsgMain.InnerHtml = null;
        txtReason.Text = string.Empty;
        FillRecentNotes();
        btnAdd.Text = "Approve";
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){$('#overlay').fadeIn('slow', function () { $('#DilogAprove').animate({ top: '5%' },{duration: 'slow',easing: 'linear'}) }); $('#close_x').click(function () {$('#dialog').animate({ top: '-300%' }, function () {$('#overlay').fadeOut('slow');});});});", true);
        int studentid = Convert.ToInt32(sess.StudentId);
        //ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);

    }

    private void FillRecentNotes()
    {
        sess = (clsSession)Session["UserSession"];
        string notes = "SELECT ApprovalNotes,ModifiedOn from StdtIEP where StdtIEPId='" + sess.IEPId + "' and ApprovalNotes<>''";
        System.Data.DataTable Dt = objDataClass.fillData(notes);
        if (Dt.Rows.Count > 0)
        {
            recentnote.Visible = true;
            Approvalnotes = Dt.Rows[0]["ApprovalNotes"].ToString().Trim();
            string dateofmodify = Dt.Rows[0]["ModifiedOn"].ToString().Trim();
            string[] results = Approvalnotes.Split(new[] { "@%@&@" }, StringSplitOptions.None);
            NOTE = new string[results.Count()];
            DATE = new string[results.Count()];
            for (int j = 1; j < results.Count(); j++)
            {
                string[] fulldata = results[j].Split(new[] { "@/&&/@" }, StringSplitOptions.None);
                NOTE[j - 1] = fulldata[0];
                DATE[j - 1] = fulldata[1];
            }
            _dtTest = new System.Data.DataTable();
            DataRow dr = null;
            ClassDatatable oDt = new ClassDatatable();
            _dtTest = oDt.CreateColumn("RecentNote", _dtTest);
            _dtTest = oDt.CreateColumn("RecentDate", _dtTest);
            int i = 0;
            foreach (string word in NOTE)
            {
                dr = _dtTest.NewRow();
                dr["RecentNote"] = NOTE[i];
                dr["RecentDate"] = DATE[i];
                _dtTest.Rows.Add(dr);
                i++;
            }
            Dlnotes.DataSource = _dtTest;
            Dlnotes.DataBind();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        tdMessage.InnerHtml = "";
        int RedirectFlag = 0;
        try
        {
            sess = (clsSession)Session["UserSession"];
            string notes = Approvalnotes + "@%@&@" + txtReason.Text.Trim() + "@/&&/@" + DateTime.Now.ToString();
            objData = new clsData();
            string SQLQRY = "UPDATE StdtIEP set ApprovalNotes='" + clsGeneral.convertQuotes(notes) + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEPId='" + sess.IEPId + "'";
            int retVal = objData.Execute(SQLQRY);


            recentnote.Visible = false;
            if (retVal > 0)
            {
                string selIEPstatus = "Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ";
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
                }

                FillProgressIEP();
                FillPendingIEP();
                FillApprovedIEP();

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

                    for (int iepPage = 0; iepPage < 8; iepPage++)
                    {
                        string ieppageid = row["ID"] + "-Page" + iepPage;
                        string[] IEPContents = { "Individualized Education", "Present Levels Of Educational", "Present Levels Of Educational", "Current Performance", "Service Delivery", "Non Participation justification", "State Of District-Wide", "Additional Information" };
                        string[] IEPFunctions = { "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP1.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP2.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP3.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP4.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP5.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP6.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP7.aspx');", "FindIEPPAge('Approve'," + row["ID"] + ",'CreateIEP8.aspx');" };
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

                        for (int iepPage = 0; iepPage < 8; iepPage++)
                        {
                            string ieppageid = row["ID"] + "-Page" + iepPage;
                            string[] IEPContents = { "Individualized Education", "Present Levels Of Educational", "Present Levels Of Educational", "Current Performance", "Service Delivery", "Non Participation justification", "State Of District-Wide", "Additional Information" };
                            string[] IEPFunctions = { "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP1.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP2.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP3.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP4.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP5.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP6.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP7.aspx');", "FindIEPPAge('Pending'," + row["ID"] + ",'CreateIEP8.aspx');" };
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
            sess = (clsSession)Session["UserSession"];
            string notes = Approvalnotes + "@%@&@" + txtReason.Text.Trim() + "@/&&/@" + DateTime.Now.ToString();
            objData = new clsData();
            string SQLQRY = "UPDATE StdtIEP set ApprovalNotes='" + notes + "',StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='in progress'),ModifiedBy='" + Convert.ToInt32(sess.LoginId) + "',ModifiedOn=getdate() where StdtIEPId='" + sess.IEPId + "'";
            int retVal = objData.Execute(SQLQRY);

            recentnote.Visible = false;


            if (retVal > 0)
            {
                tdMsgMain.InnerHtml = clsGeneral.sucessMsg("Rejected ...");
                //setSubmitAprove();
                /*
                btnApprove.Visible = false;
                imgpending.Visible = true;
                checkStatus();*/
            }

            else
            {
                tdMsgMain.InnerHtml = clsGeneral.failedMsg(btnAdd.Text + " Failed!!!");
            }

        }
        catch (Exception ex)
        {
            objData.RollBackTransation();

        }
        ClientScript.RegisterStartupScript(GetType(), "", "CreateIEP1();", true);
    }
    protected void btnIEPExport_Click(object sender, EventArgs e)
    {


        tdMsgMain.InnerHtml = "";
        AllInOne();


    }

    [WebMethod]
    public static void SetIEPId(int ID)
    {

        clsData objData = new clsData();
        clsSession sess = null;
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        if (sess == null) return;
        sess.IEPId = ID;

    }

    private void AllInOne()
    {        //Test();

      /*  sess = (clsSession)Session["UserSession"];
        string Path = "";
        string NewPath = "";

        if (sess.IEPId == 0)
        {
            tdMsgMain.InnerHtml = clsGeneral.warningMsg("Please Select IEP");
            return;
        }


        clsExport objExport = new clsExport();
        try
        {
            Path = Server.MapPath("~\\Administration\\IEPTemplates\\Dummy.docx");
            NewPath = CopyTemplate(Path, "0");
            //SearchAndReplace(Path);


            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations1.xml");
            objExport.getIEP1(out columns, sess.StudentId, sess.SchoolId, sess.IEPId);
            if (columns != null)
            {
                Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP1.docx");
                NewPath = CopyTemplate(Path, "1");
                if (NewPath != "")
                {

                    SearchAndReplace(NewPath);
                }
            }

            columns = new string[0];
            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations2.xml");
            objExport.getIEP2(out columns, sess.StudentId, sess.SchoolId, sess.IEPId);

            Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP2.docx");
            NewPath = CopyTemplate(Path, "2");
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
                if (columnsCheck.Length > 0)
                {
                    setCheckBox(NewPath);
                }
                columnsCheck[0] = null;
            }



            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations3.xml");
            objExport.getIEP3(out columns, sess.StudentId, sess.SchoolId, sess.IEPId);
            Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP3.docx");
            NewPath = CopyTemplate(Path, "3");
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
                if (columnsCheck.Length > 0)
                {
                    setCheckBox(NewPath);
                }
                columnsCheck[0] = null;
            }



            dtIEP4 = new System.Data.DataTable();
            bool Odd = false;

            dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);
            int RowsCount = dtIEP4.Rows.Count;
            dtIEP4.TableName = "Table";
            int K = 0;
            for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
            {
                System.Data.DataTable Dt4 = new System.Data.DataTable();
                Dt4 = objExport.ReturnRows(Round, dtIEP4);
                objExport.getIEP4(out columnsP4, Dt4);
                CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations4.xml");
                objExport.getIEP4(out columns, Dt4);
                if (Odd == true && Round + 2 == RowsCount)
                {
                    Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP41.docx");
                }
                else
                {
                    Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP4.docx");
                }
                NewPath = CopyTemplate(Path, "4." + (Round + 1).ToString());
                if (NewPath != "")
                {
                    SearchAndReplace(NewPath);
                }
            }



            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations5.xml");
            objExport.getIEP5(out columns, sess.StudentId, sess.SchoolId, sess.IEPId);
            Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP5.docx");
            NewPath = CopyTemplate(Path, "5");
            if (NewPath != "")
            {
                NewPath = NewPath.Replace("\\\\", "\\");
                SearchAndReplace(NewPath);
                if (columnsCheck.Length > 0)
                {
                    setCheckBox(NewPath);
                }
                columnsCheck[0] = null;
            }




            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations6.xml");
            objExport.getIEP6(out columns, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
            Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP6.docx");
            NewPath = CopyTemplate(Path, "6");
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
                if (columnsCheck.Length > 0)
                {
                    setCheckBox(NewPath);
                }
                columnsCheck[0] = null;
            }



            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations7.xml");
            objExport.getIEP7(out columns, sess.StudentId, sess.SchoolId, sess.IEPId);
            Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP7.docx");
            NewPath = CopyTemplate(Path, "7");
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
                if (columnsCheck.Length > 0)
                {
                    setCheckBox(NewPath);
                }
                columnsCheck[0] = null;
            }



            CreateQuery("NE", "..\\Administration\\XMlIEPS\\IEPCreations8.xml");
            objExport.getIEP8(out columns, sess.StudentId, sess.SchoolId, sess.IEPId);
            Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP8.docx");
            NewPath = CopyTemplate(Path, "8");
            if (NewPath != "")
            {
                checkCount = 3;
                SearchAndReplace(NewPath);
                if (columnsCheck.Length > 0)
                {
                    setCheckBox(NewPath);
                }
                columnsCheck[0] = null;
            }



            string iepDoneFlg = MergeFiles();

            if (iepDoneFlg == "")
            {
                tdMsgExport.InnerHtml = clsGeneral.failedMsg("IEP Document Creation Failed !");
            }
            else
            {
                tdMsgMain.InnerHtml = "";
                tdMsgExport.InnerHtml = clsGeneral.sucessMsg("IEP Documents Sucessfully Created ");
                string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '5%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
                //   btnIEPExport.Text = "Download";
                //   BtnCanel.Visible = true;
            }

        }
        catch (Exception eX)
        {
            tdMsgMain.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
        }*/
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

    private string CopyTemplate(string oldPath, string PageNo)
    {
        try
        {
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\Administration") + "\\Temp\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string newpath = path + "\\";
            string newFileName = "IEP" + PageNo;
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
            tdMsgMain.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
        }
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
    public void SearchAndReplace(string document)
    {
        int m = 0;

        //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        //{
        //    //string docText = null;
        //using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
        //{
        //    docText = sr.ReadToEnd();
        //}
        string col = "";
        string plc = "";

        columnsCheck = new string[checkCount];

        //MainDocumentPart mainPart = wordDoc.MainDocumentPart;


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
                col = col.Replace("<p", "<span ");
                col = col.Replace("</p>", "</span><br> ");

                //Regex regexText = new Regex(plc);
                // docText = regexText.Replace(docText, col);
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
                        HtmlConverter converter = new HtmlConverter(mainPart);
                        Body body = mainPart.Document.Body;

                        DocumentFormat.OpenXml.Wordprocessing.Paragraph para = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run((new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = DocumentFormat.OpenXml.Wordprocessing.BreakValues.Page })));

                        //body.Append(para);
                        //mainPart.Document.Save();
                        mainPart.Document.Body.InsertAfter(para, mainPart.Document.Body.LastChild);
                        mainPart.Document.Save();
                    }
                }
            }
        }
        //}
        //using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
        //{
        //    sw.Write(docText);
        //}


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
    public void replaceWithHtml(string fileName, string replace, string replaceTest)
    {

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            //     converter.ConsiderDivAsParagraph = false;

            //     SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            //paragraphProperties.Append(spacing);

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();





            if (replaceTest == "") replaceTest = "&nbsp;";
            if (replaceTest != "") replaceTest = replaceTest.Trim();
            try
            {
                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();

                var paragraphs = converter.Parse(replaceTest);



                for (int i = 0; i < paragraphs.Count; i++)
                {
                    var parent = placeholder.Parent;
                    if (parent != null)
                    {
                        paragraphs[i].Append(paragraphProperties);
                        parent.ReplaceChild(paragraphs[i], placeholder);
                    }
                }
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                tdMsgMain.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
            }


            //else
            //{



            //    string docText = null;
            //    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            //    {
            //        docText = sr.ReadToEnd();
            //    }

            //    Regex regexText = new Regex(replace);
            //    docText = regexText.Replace(docText, replaceTest);

            //    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            //    {
            //        sw.Write(docText);
            //    }


            //}

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

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();

    }
    protected void lnkApprovedIEP_Click(object sender, EventArgs e)
    {

    }

    protected void lnkPendingIEP_Click(object sender, EventArgs e)
    {

    }

    protected void btnNewIEP_Click(object sender, EventArgs e)
    {
        checkIepEditCreateStatus(true);
    }
}