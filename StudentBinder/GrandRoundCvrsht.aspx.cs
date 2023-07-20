using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Xml;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Globalization;
using System.Net;
using System.Web.Services;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class StudentBinder_GrandRoundCvrsht : System.Web.UI.Page
{
    System.Data.DataTable dtRecChang = new System.Data.DataTable();
    System.Data.DataTable dtChangMed = new System.Data.DataTable();
    System.Data.DataTable dtChangPsy = new System.Data.DataTable();
    System.Data.DataTable dtChangBhvUp = new System.Data.DataTable();
    System.Data.DataTable dtChangMedUp = new System.Data.DataTable();
    System.Data.DataTable dtChangPsyUp = new System.Data.DataTable();
    clsSession sess = null;
    System.Data.DataTable Dt = null;
    clsData objData = null;

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
            //LoadBasicData();
            PanelMain.Visible = false;
            LoadDate();
            LoadDashboard();
            //FillReport();
            tdMsg.InnerHtml = "<span class='tdtext' style='color: #0D668E; font-family:times new roman;margin-left:20px;font-size:18px;'>Please Select a date to load the data</span>";
        }


        //LoadData(date);


    }

    protected void LoadDate()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string query = "SELECT CONVERT(varchar(10),StartDate,101) +'-'+CONVERT(varchar(10),EndDate,101) as EDate FROM StdtGrandRoundCvrsheet WHERE StudentId=" + sess.StudentId + " ORDER BY StartDate DESC";
        //string query = "SELECT CONVERT(varchar(10),StartDate,101) +'-'+CONVERT(varchar(10),EndDate,101) as EndDate FROM StdtGrandRoundCvrsheet WHERE StudentId=" + sess.StudentId + " ORDER BY EndDate DESC";
        Dt = new DataTable();
        Dt = objData.ReturnDataTable(query, false);
        grDateList.DataSource = Dt;
        grDateList.DataBind();
    }

    protected void LoadBasicData()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];

        string CSdate = "";
        if (ViewState["CurrentDate"] != null)
            CSdate = ViewState["CurrentDate"].ToString();
        string stDate = "";
        string endDate = "";
        if (CSdate != null)
        {
            stDate = CSdate.Split('-')[0];
            endDate = CSdate.Split('-')[1];
        }
        string qry = "SELECT FirstName,CAST(DATEDIFF(DD,BirthDate,GETDATE())/365.25 AS INT) AS Age,LastName,CONVERT(VARCHAR,BirthDate,101) AS BirthDate,CONVERT(VARCHAR,AdmissionDate,101)AS AdmissionDate,CaseManagerEducational,CONVERT(VARCHAR,null) as RevFromDate, CONVERT(VARCHAR,null) as RevToDate FROM StudentPersonal WHERE StudentPersonalId=" + sess.StudentId + "";
        Dt = objData.ReturnDataTable(qry, false);

        if (stDate != "" && endDate != "")
        {
            foreach (DataRow dr in Dt.Rows)
            {
                dr["RevFromDate"] = stDate;
                dr["RevToDate"] = endDate;
            }
        }

        GVBasicDetails.DataSource = Dt;
        GVBasicDetails.DataBind();
    }
    protected void ClearAllData()
    {
        txtInjury.Text = "";
        txtChallenge.Text = "";
        chkInjury.Checked = false;
        chkChallenge.Checked = false;
        GVBehave.DataSource = null;
        GVBehave.DataBind();
        GVMedical.DataSource = null;
        GVMedical.DataBind();
        GVPsy.DataSource = null;
        GVPsy.DataBind();
        GVBhvUp.DataSource = null;
        GVBhvUp.DataBind();
        GVMedicalUp.DataSource = null;
        GVMedicalUp.DataBind();
        GVPsyUp.DataSource = null;
        GVPsyUp.DataBind();
    }

    protected void FillReport()
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string currentData = dtst.ToString("MM'/'dd'/'yyyy") + "-" + dted.ToString("MM'/'dd'/'yyyy");
        ViewState["CurrentDate"] = currentData;
        FillReportGrand(currentData);

    }


    protected void FillReportGrand(string date)
    {

        sess = (clsSession)Session["UserSession"];
        DataTable dt = new DataTable();

        ClearAllData();
        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        PanelMain.Visible = true;
        tdMsg.InnerHtml = "";
        dtRecChang.Columns.Add("Recommendation");
        dtRecChang.Columns.Add("PersonResponsible");
        //dtRecChang.Columns.Add(
        dtRecChang.Columns.Add("Status");
        dtRecChang.Columns.Add("Notes");
        dtRecChang.Columns.Add("CDGoal");
        dtRecChang.Columns.Add("RevPeriod");
        dtRecChang.Columns.Add("CmpDate");
        dtRecChang.Columns.Add("PrvsId");



        dtChangMed.Columns.Add("Recommendation_Med");
        dtChangMed.Columns.Add("PersonResponsible_Med");
        dtChangMed.Columns.Add("Status_Med");
        dtChangMed.Columns.Add("Notes_Med");
        dtChangMed.Columns.Add("CDGoal_Med");
        dtChangMed.Columns.Add("RevPeriod_Med");
        dtChangMed.Columns.Add("CmpDate_Med");
        dtChangMed.Columns.Add("PrvsId");


        dtChangPsy.Columns.Add("Recommendation_Psy");
        dtChangPsy.Columns.Add("PersonResponsible_Psy");
        dtChangPsy.Columns.Add("Status_Psy");
        dtChangPsy.Columns.Add("Notes_Psy");
        dtChangPsy.Columns.Add("CDGoal_Psy");
        dtChangPsy.Columns.Add("RevPeriod_Psy");
        dtChangPsy.Columns.Add("CmpDate_Psy");
        dtChangPsy.Columns.Add("PrvsId");


        dtChangBhvUp.Columns.Add("Notes_Bhv");
        dtChangBhvUp.Columns.Add("Recommendation_Bhv");
        dtChangBhvUp.Columns.Add("PersonResponsible_Bhv");
        dtChangBhvUp.Columns.Add("hdPerRespBhv");
        dtChangBhvUp.Columns.Add("Goal_bhv");

        dtChangMedUp.Columns.Add("Notes_Med_Up");
        dtChangMedUp.Columns.Add("Recommendation_Med_Up");
        dtChangMedUp.Columns.Add("PersonResponsible_Med_Up");
        dtChangMedUp.Columns.Add("hdPerRespMed");
        dtChangMedUp.Columns.Add("Goal_Med_Up");

        dtChangPsyUp.Columns.Add("Notes_Psy_Up");
        dtChangPsyUp.Columns.Add("Recommendation_Psy_Up");
        dtChangPsyUp.Columns.Add("PersonResponsible_Psy_Up");
        dtChangPsyUp.Columns.Add("hdPerRespPsy");
        dtChangPsyUp.Columns.Add("Goal_Psy_Up");

        loadRecChange();
        loadChangMed();
        loadChangPsy();
        loadBhvUp();
        loadMedUp();
        loadPsyUp();

        if (objData.IFExists("SELECT StdtCvrId FROM StdtGrandRoundCvrsheet WHERE CAST (StartDate AS date)=cast('" + stDate + "' as date) AND  CAST (EndDate AS date)=cast('" + endDate + "' as date) AND SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + "") == false)
        {

            DataLoad(date);
            FullData(date);
            btnSave.Text = "Submit";
            btnDraft.Visible = true;
            btnSave.Visible = true;
            divSubByOn.Visible = false;

        }
        else
        {
            DataLoad(date);
            FillCvrShtHfVal(date);
            FullData(date);
            //btnSave.Text = "Update";

            string sDate = "";
            string sBy = "";
            string q = "select CONVERT(VARCHAR,grc.SubmittedDate,101)AS SubmittedDate, usr.UserLName+', '+usr.UserFName as CreatedBy from StdtGrandRoundCvrsheet grc left join [User] usr on usr.UserId=grc.CreatedBy where CAST (StartDate AS date)=cast('" + stDate + "' as date) AND  CAST (EndDate AS date)=cast('" + endDate + "' as date) AND grc.SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + "";

            dt = objData.ReturnDataTable(q, false);
            if (dt.Rows.Count > 0)
            {
                sDate = dt.Rows[0]["SubmittedDate"].ToString();
                sBy = dt.Rows[0]["CreatedBy"].ToString();
            }

            if (sDate == "" || sDate == null)
            {
                btnDraft.Visible = true;
                btnSave.Visible = true;
                divSubByOn.Visible = false;
            }
            else
            {
                btnDraft.Visible = false;
                btnSave.Visible = false;
                divSubByOn.Visible = true;
                lblSubBy.Text = sBy;
                lblSubOn.Text = sDate;
            }
        }
    }

    protected void FullData(string date)
    {

        Dt = new DataTable();
        DataTable DtPRec = new DataTable();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string stDate = "";
        string endDate = "";
        if (date != "")
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }

        string hfCSId = "0";
        if (hdFldCvid.Value != "")
            hfCSId = hdFldCvid.Value;
        string subDate="";
        string query = "select SubmittedDate from StdtGrandRoundCvrsheet where StdtCvrId=" + hfCSId;
        if(objData.FetchValue(query) != null)
        subDate = objData.FetchValue(query).ToString();
        string extQuery = "";
        extQuery = " and p.SubmissionStatus is null";

        Dt = objData.ReturnDataTable("select StdtCvrId,SchoolId,ClassId,StudentId,CONVERT(VARCHAR(10),StartDate,101) as StartDate,CONVERT(VARCHAR(10),EndDate,101) as EndDate,SignificantInjuries,SignificantNotes,Challenges,ChallengesNotes from StdtGrandRoundCvrsheet where CAST( StartDate as date)= cast('" + stDate + "' as date) and  CAST( EndDate as date)= cast('" + endDate + "' as date) and SchoolId='" + sess.SchoolId + "' and StudentId='" + sess.StudentId + "'", true);

        if (Dt.Rows.Count > 0)
        {
            bool injury = bool.Parse(Dt.Rows[0]["SignificantInjuries"].ToString());
            bool chlng = bool.Parse(Dt.Rows[0]["Challenges"].ToString());
            if (injury)
            {
                chkInjury.Checked = true;
            }
            else
            {
                chkInjury.Checked = false;
            }
            if (chlng)
            {
                chkChallenge.Checked = true;
            }
            else
            {
                chkChallenge.Checked = false;
            }
            txtChallenge.Text = Dt.Rows[0]["ChallengesNotes"].ToString();
            txtInjury.Text = Dt.Rows[0]["SignificantNotes"].ToString();
        }

        DtPRec = objData.ReturnDataTable("select p.PrvsId, p.PreRecom as Recommendation, u.UserLName+', '+u.UserFName as PersonResponsible, p.StatusUpdate as Status, CONVERT(varchar(10),p.CompletedDate,101) as CmpDate, p.RecNotes as Notes, CONVERT(varchar(10),p.CompletionDateGoal,101) as CDGoal, CONVERT(varchar(10),s.StartDate,101) + ' to ' + CONVERT(varchar(10),s.EndDate,101) as RevPeriod from PrvsRecomendations p left join StdtGrandRoundCvrsheet s on s.StdtCvrId=p.StdtCvrId left join [User] u on u.UserId=p.PersonResp2 where p.PrvsRecordType='BehaveUp' and p.StdtCvrId in (select StdtCvrId from StdtGrandRoundCvrsheet where StudentId=" + sess.StudentId + " and StdtCvrId <> " + hfCSId + ")" + extQuery + " and CONVERT(varchar(10),s.SubmittedDate,101) <= '" + endDate + "'", true);
        if (DtPRec.Rows.Count > 0)
        {
            GVPrev.DataSource = DtPRec;
            GVPrev.DataBind();
        }

        DtPRec = objData.ReturnDataTable("select p.PrvsId, p.PreRecom as Recommendation_Med, u.UserLName+', '+u.UserFName as PersonResponsible_Med, p.StatusUpdate as Status_Med, CONVERT(varchar(10),p.CompletedDate,101) as CmpDate_Med, p.RecNotes as Notes_Med, CONVERT(varchar(10),p.CompletionDateGoal,101) as CDGoal_Med, CONVERT(varchar(10),s.StartDate,101) + ' to ' + CONVERT(varchar(10),s.EndDate,101) as RevPeriod_Med from PrvsRecomendations p left join StdtGrandRoundCvrsheet s on s.StdtCvrId=p.StdtCvrId left join [User] u on u.UserId=p.PersonResp2 where p.PrvsRecordType='MedicalUp' and p.StdtCvrId in (select StdtCvrId from StdtGrandRoundCvrsheet where StudentId=" + sess.StudentId + " and StdtCvrId <> " + hfCSId + ")" + extQuery + " and CONVERT(varchar(10),s.SubmittedDate,101) <= '" + endDate + "'", true);
        if (DtPRec.Rows.Count > 0)
        {
            GVMedical.DataSource = DtPRec;
            GVMedical.DataBind();
        }

        DtPRec = objData.ReturnDataTable("select p.PrvsId, p.PreRecom as Recommendation_Psy, u.UserLName+', '+u.UserFName as PersonResponsible_Psy, p.StatusUpdate as Status_Psy, CONVERT(varchar(10),p.CompletedDate,101) as CmpDate_Psy, p.RecNotes as Notes_Psy, CONVERT(varchar(10),p.CompletionDateGoal,101) as CDGoal_Psy, CONVERT(varchar(10),s.StartDate,101) + ' to ' + CONVERT(varchar(10),s.EndDate,101) as RevPeriod_Psy from PrvsRecomendations p left join StdtGrandRoundCvrsheet s on s.StdtCvrId=p.StdtCvrId left join [User] u on u.UserId=p.PersonResp2 where p.PrvsRecordType='PsychiatryUp' and p.StdtCvrId in (select StdtCvrId from StdtGrandRoundCvrsheet where StudentId=" + sess.StudentId + " and StdtCvrId <> " + hfCSId + ")" + extQuery + " and CONVERT(varchar(10),s.SubmittedDate,101) <= '" + endDate + "'", true);
        if (DtPRec.Rows.Count > 0)
        {
            GVPsy.DataSource = DtPRec;
            GVPsy.DataBind();
        }

        if (Dt.Rows.Count > 0)
        {
            int CvrId = int.Parse(Dt.Rows[0]["StdtCvrId"].ToString());


            //Dt = objData.ReturnDataTable("select PrvsId,RecNotes AS Notes_Bhv,PreRecom AS Recommendation_Bhv,PersonResp2 AS PersonResponsible_Bhv,CONVERT (VARCHAR(10),CompletionDateGoal,101) AS Goal_bhv from PrvsRecomendations where StdtCvrId=" + CvrId + " AND PrvsRecordType='BehaveUp'", true);
            Dt = objData.ReturnDataTable("select PrvsId,u.UserId as hdPerRespBhv, RecNotes AS Notes_Bhv,PreRecom AS Recommendation_Bhv,u.UserLName+','+u.UserFName AS PersonResponsible_Bhv,CONVERT (VARCHAR(10),CompletionDateGoal,101) AS Goal_bhv from PrvsRecomendations p join [User] u on p.PersonResp2 = u.UserId and StdtCvrId=" + CvrId + " AND PrvsRecordType='BehaveUp'", true);
            if (Dt.Rows.Count > 0)
            {
                GVBhvUp.DataSource = Dt;
                GVBhvUp.DataBind();
            }

            //Dt = objData.ReturnDataTable("select PrvsId,RecNotes AS Notes_Med_Up,PreRecom AS Recommendation_Med_Up,PersonResp2 AS PersonResponsible_Med_Up,CONVERT (VARCHAR(10),CompletionDateGoal,101) AS Goal_Med_Up from PrvsRecomendations where StdtCvrId=" + CvrId + " AND PrvsRecordType='MedicalUp'", true);
            Dt = objData.ReturnDataTable("select PrvsId,u.UserId as hdPerRespMed, RecNotes AS Notes_Med_Up,PreRecom AS Recommendation_Med_Up,u.UserLName+','+u.UserFName AS PersonResponsible_Med_Up,CONVERT (VARCHAR(10),CompletionDateGoal,101) AS Goal_Med_Up from PrvsRecomendations p join [User] u on p.PersonResp2 = u.UserId and StdtCvrId=" + CvrId + " AND PrvsRecordType='MedicalUp'", true);
            if (Dt.Rows.Count > 0)
            {
                GVMedicalUp.DataSource = Dt;
                GVMedicalUp.DataBind();
            }

            //Dt = objData.ReturnDataTable("select PrvsId,RecNotes AS Notes_Psy_Up,PreRecom AS Recommendation_Psy_Up,PersonResp2 AS PersonResponsible_Psy_Up,CONVERT (VARCHAR(10),CompletionDateGoal,101) AS Goal_Psy_Up from PrvsRecomendations where StdtCvrId=" + CvrId + " AND PrvsRecordType='PsychiatryUp'", true);
            Dt = objData.ReturnDataTable("select PrvsId, u.UserId as hdPerRespPsy, RecNotes AS Notes_Psy_Up,PreRecom AS Recommendation_Psy_Up,u.UserLName+','+u.UserFName AS PersonResponsible_Psy_Up,CONVERT (VARCHAR(10),CompletionDateGoal,101) AS Goal_Psy_Up from PrvsRecomendations p join [User] u on p.PersonResp2 = u.UserId and StdtCvrId=" + CvrId + " AND PrvsRecordType='PsychiatryUp'", true);
            if (Dt.Rows.Count > 0)
            {
                GVPsyUp.DataSource = Dt;
                GVPsyUp.DataBind();
            }

        }


    }



    public void loadRecChange()
    {

        DataRow dr = dtRecChang.NewRow();
        dr["Recommendation"] = "";
        dr["PersonResponsible"] = "";
        dr["Status"] = "";
        dr["Notes"] = "";
        dr["CDGoal"] = "";
        dr["RevPeriod"] = "";
        dr["CmpDate"] = "";
        dr["PrvsId"] = "";
        dtRecChang.Rows.Add(dr);
        GVPrev.DataSource = dtRecChang;
        GVPrev.DataBind();
    }

    public void loadChangMed()
    {

        DataRow dr = dtChangMed.NewRow();
        dr["Recommendation_Med"] = "";
        dr["PersonResponsible_Med"] = "";
        dr["Status_Med"] = "";
        dr["Notes_Med"] = "";
        dr["CDGoal_Med"] = "";
        dr["RevPeriod_Med"] = "";
        dr["CmpDate_Med"] = "";
        dr["PrvsId"] = "";
        dtChangMed.Rows.Add(dr);
        GVMedical.DataSource = dtChangMed;
        GVMedical.DataBind();
    }

    public void loadChangPsy()
    {

        DataRow dr = dtChangPsy.NewRow();
        dr["Recommendation_Psy"] = "";
        dr["PersonResponsible_Psy"] = "";
        dr["Status_Psy"] = "";
        dr["Notes_Psy"] = "";
        dr["CDGoal_Psy"] = "";
        dr["RevPeriod_Psy"] = "";
        dr["CmpDate_Psy"] = "";
        dr["PrvsId"] = "";
        dtChangPsy.Rows.Add(dr);
        GVPsy.DataSource = dtChangPsy;
        GVPsy.DataBind();
        //dtRecChang.Rows.Add(
    }

    public void loadBhvUp()
    {

        DataRow dr = dtChangBhvUp.NewRow();
        dr["Notes_Bhv"] = "";
        dr["Recommendation_Bhv"] = "";
        dr["PersonResponsible_Bhv"] = "";
        dr["hdPerRespBhv"] = "";
        dr["Goal_bhv"] = "";
        dtChangBhvUp.Rows.Add(dr);
        GVBhvUp.DataSource = dtChangBhvUp;
        GVBhvUp.DataBind();
        //dtRecChang.Rows.Add(
    }

    public void loadMedUp()
    {

        DataRow dr = dtChangMedUp.NewRow();
        dr["Notes_Med_Up"] = "";
        dr["Recommendation_Med_Up"] = "";
        dr["PersonResponsible_Med_Up"] = "";
        dr["hdPerRespMed"] = "";
        dr["Goal_Med_Up"] = "";
        dtChangMedUp.Rows.Add(dr);
        GVMedicalUp.DataSource = dtChangMedUp;
        GVMedicalUp.DataBind();
        //dtRecChang.Rows.Add(
    }

    public void loadPsyUp()
    {

        DataRow dr = dtChangPsyUp.NewRow();
        dr["Notes_Psy_Up"] = "";
        dr["Recommendation_Psy_Up"] = "";
        dr["PersonResponsible_Psy_Up"] = "";
        dr["hdPerRespPsy"] = "";
        dr["Goal_Psy_Up"] = "";
        dtChangPsyUp.Rows.Add(dr);
        GVPsyUp.DataSource = dtChangPsyUp;
        GVPsyUp.DataBind();
        //dtRecChang.Rows.Add(
    }

    public void DataLoad(string date)
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];
        //  DateTime todayDate=DateTime.Parse(drpSelectDate.SelectedItem.Text);
        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        DateTime todayDate = DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime lastDayDate = DateTime.ParseExact(stDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

        Dt = objData.ExecuteCoversheetBehavior(lastDayDate, todayDate, sess.SchoolId, sess.StudentId);

        GVBehave.DataSource = Dt;
        GVBehave.DataBind();

    }
    //public void LoadData(string date)
    //{
    //    dtRecChang.Columns.Add("Recommendation");
    //    dtRecChang.Columns.Add("Person Responsible");
    //    dtRecChang.Columns.Add("Status");
    //}


    public void addRow()
    {
        dtRecChang.Columns.Add("Recommendation", typeof(string));
        dtRecChang.Columns.Add("PersonResponsible", typeof(string));
        dtRecChang.Columns.Add("Status", typeof(string));
        foreach (GridViewRow gdVr in GVPrev.Rows)
        {
            DataRow drtemp = dtRecChang.NewRow();

            drtemp["Recommendation"] = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            drtemp["PersonResponsible"] = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            drtemp["Status"] = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            dtRecChang.Rows.Add(drtemp);
        }
        DataRow dr = dtRecChang.NewRow();
        dr["Recommendation"] = "";
        dr["PersonResponsible"] = "";
        dr["Status"] = "";
        dtRecChang.Rows.Add(dr);
        GVPrev.DataSource = dtRecChang;
        GVPrev.DataBind();
    }

    public void addRowMed()
    {
        dtChangMed.Columns.Add("Recommendation_Med", typeof(string));
        dtChangMed.Columns.Add("PersonResponsible_Med", typeof(string));
        dtChangMed.Columns.Add("Status_Med", typeof(string));
        foreach (GridViewRow gdVr in GVMedical.Rows)
        {
            DataRow drtemp = dtChangMed.NewRow();

            drtemp["Recommendation_Med"] = ((TextBox)gdVr.FindControl("txtRecomdMed")).Text;
            drtemp["PersonResponsible_Med"] = ((TextBox)gdVr.FindControl("txtTimeLineMed")).Text;
            drtemp["Status_Med"] = ((TextBox)gdVr.FindControl("txtPerResMed")).Text;
            dtChangMed.Rows.Add(drtemp);
        }
        DataRow dr = dtChangMed.NewRow();
        dr["Recommendation_Med"] = "";
        dr["PersonResponsible_Med"] = "";
        dr["Status_Med"] = "";
        dtChangMed.Rows.Add(dr);
        GVMedical.DataSource = dtChangMed;
        GVMedical.DataBind();
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "call", "destroy()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanelMedUp, UpdatePanelMedUp.GetType(), "", "destroy();", true);
    }

    public void addRowPsy()
    {
        dtChangPsy.Columns.Add("Recommendation_Psy", typeof(string));
        dtChangPsy.Columns.Add("PersonResponsible_Psy", typeof(string));
        dtChangPsy.Columns.Add("Status_Psy", typeof(string));
        foreach (GridViewRow gdVr in GVPsy.Rows)
        {
            DataRow drtemp = dtChangPsy.NewRow();

            drtemp["Recommendation_Psy"] = ((TextBox)gdVr.FindControl("txtRecomdPsy")).Text;
            drtemp["PersonResponsible_Psy"] = ((TextBox)gdVr.FindControl("txtTimeLineMPsy")).Text;
            drtemp["Status_Psy"] = ((TextBox)gdVr.FindControl("txtPerResPsy")).Text;
            dtChangPsy.Rows.Add(drtemp);
        }
        DataRow dr = dtChangPsy.NewRow();
        dr["Recommendation_Psy"] = "";
        dr["PersonResponsible_Psy"] = "";
        dr["Status_Psy"] = "";
        dtChangPsy.Rows.Add(dr);
        GVPsy.DataSource = dtChangPsy;
        GVPsy.DataBind();
    }


    public void addRowBhvUp()
    {
        dtChangBhvUp.Columns.Add("Notes_Bhv", typeof(string));
        dtChangBhvUp.Columns.Add("Recommendation_Bhv", typeof(string));
        dtChangBhvUp.Columns.Add("PersonResponsible_Bhv", typeof(string));
        dtChangBhvUp.Columns.Add("Goal_bhv", typeof(string));
        dtChangBhvUp.Columns.Add("hdPerRespBhv", typeof(string));
        foreach (GridViewRow gdVr in GVBhvUp.Rows)
        {
            DataRow drtemp = dtChangBhvUp.NewRow();

            drtemp["Notes_Bhv"] = ((TextBox)gdVr.FindControl("txtRecomdBhvUp")).Text;
            drtemp["Recommendation_Bhv"] = ((TextBox)gdVr.FindControl("txtTimeBhvup")).Text;
            //drtemp["PersonResponsible_Bhv"] = ((DropDownList)gdVr.FindControl("ddlPerRespBhv")).SelectedValue;
            drtemp["PersonResponsible_Bhv"] = ((TextBox)gdVr.FindControl("txtPerRespBhv")).Text;
            drtemp["Goal_bhv"] = ((TextBox)gdVr.FindControl("txtDateBhv")).Text;
            drtemp["hdPerRespBhv"] = ((TextBox)gdVr.FindControl("hdPerRespBhv")).Text;
            dtChangBhvUp.Rows.Add(drtemp);
        }
        DataRow dr = dtChangBhvUp.NewRow();
        dr["Notes_Bhv"] = "";
        dr["Recommendation_Bhv"] = "";
        dr["PersonResponsible_Bhv"] = "";
        dr["hdPerRespBhv"] = "";
        dr["Goal_bhv"] = "";
        dtChangBhvUp.Rows.Add(dr);
        
        GVBhvUp.DataSource = dtChangBhvUp;
        GVBhvUp.DataBind();

        Page.ClientScript.RegisterStartupScript(this.GetType(), "call", "destroy()", true);
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "somekey", "destroy();", true); 
    }

    public void addRowMedUp()
    {
        dtChangMedUp.Columns.Add("Notes_Med_Up", typeof(string));
        dtChangMedUp.Columns.Add("Recommendation_Med_Up", typeof(string));
        dtChangMedUp.Columns.Add("PersonResponsible_Med_Up", typeof(string));
        dtChangMedUp.Columns.Add("hdPerRespMed", typeof(string));
        dtChangMedUp.Columns.Add("Goal_Med_Up", typeof(string));
        foreach (GridViewRow gdVr in GVMedicalUp.Rows)
        {
            DataRow drtemp = dtChangMedUp.NewRow();

            drtemp["Notes_Med_Up"] = ((TextBox)gdVr.FindControl("txtRecomdMedUp")).Text;
            drtemp["Recommendation_Med_Up"] = ((TextBox)gdVr.FindControl("txtTimeLineMedUp")).Text;
            //drtemp["PersonResponsible_Med_Up"] = ((DropDownList)gdVr.FindControl("ddlPerRespMed")).SelectedValue;
            drtemp["PersonResponsible_Med_Up"] = ((TextBox)gdVr.FindControl("txtPerRespMed")).Text;
            drtemp["hdPerRespMed"] = ((TextBox)gdVr.FindControl("hdPerRespMed")).Text;
            drtemp["Goal_Med_Up"] = ((TextBox)gdVr.FindControl("txtDateMed")).Text;
            dtChangMedUp.Rows.Add(drtemp);
        }
        DataRow dr = dtChangMedUp.NewRow();
        dr["Notes_Med_Up"] = "";
        dr["Recommendation_Med_Up"] = "";
        dr["PersonResponsible_Med_Up"] = "";
        dr["hdPerRespMed"] = "";
        dr["Goal_Med_Up"] = "";
        dtChangMedUp.Rows.Add(dr);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "call", "destroy()", true);
        //ScriptManager.RegisterClientScriptBlock(UpdatePanelMedUp, UpdatePanelMedUp.GetType(), "", "destroy();", true);
        GVMedicalUp.DataSource = dtChangMedUp;
        GVMedicalUp.DataBind();
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "somekey", "destroy();", true);
    }

    public void addRowPsyUp()
    {
        dtChangPsyUp.Columns.Add("Notes_Psy_Up", typeof(string));
        dtChangPsyUp.Columns.Add("Recommendation_Psy_Up", typeof(string));
        dtChangPsyUp.Columns.Add("PersonResponsible_Psy_Up", typeof(string));
        dtChangPsyUp.Columns.Add("hdPerRespPsy", typeof(string));
        dtChangPsyUp.Columns.Add("Goal_Psy_Up", typeof(string));
        foreach (GridViewRow gdVr in GVPsyUp.Rows)
        {
            DataRow drtemp = dtChangPsyUp.NewRow();

            drtemp["Notes_Psy_Up"] = ((TextBox)gdVr.FindControl("txtRecomdPsyUp")).Text;
            drtemp["Recommendation_Psy_Up"] = ((TextBox)gdVr.FindControl("txtTimeLineMPsyUp")).Text;
            //drtemp["PersonResponsible_Psy_Up"] = ((DropDownList)gdVr.FindControl("ddlPerRespPsy")).SelectedValue;
            drtemp["PersonResponsible_Psy_Up"] = ((TextBox)gdVr.FindControl("txtPerRespPsy")).Text;
            drtemp["hdPerRespPsy"] = ((TextBox)gdVr.FindControl("hdPerRespPsy")).Text;
            drtemp["Goal_Psy_Up"] = ((TextBox)gdVr.FindControl("txtDatePsy")).Text;
            dtChangPsyUp.Rows.Add(drtemp);
        }
        DataRow dr = dtChangPsyUp.NewRow();
        dr["Notes_Psy_Up"] = "";
        dr["Recommendation_Psy_Up"] = "";
        dr["PersonResponsible_Psy_Up"] = "";
        dr["hdPerRespPsy"] = "";
        dr["Goal_Psy_Up"] = "";
        dtChangPsyUp.Rows.Add(dr);
        GVPsyUp.DataSource = dtChangPsyUp;
        GVPsyUp.DataBind();
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "somekey", "destroy();", true);
    }


    public void delRow(int intexRow)
    {
        dtRecChang.Columns.Add("Recommendation", typeof(string));
        dtRecChang.Columns.Add("PersonResponsible", typeof(string));
        dtRecChang.Columns.Add("Status", typeof(string));
        dtRecChang.Columns.Add("Notes", typeof(string));
        dtRecChang.Columns.Add("CDGoal", typeof(string));
        dtRecChang.Columns.Add("RevPeriod", typeof(string));
        dtRecChang.Columns.Add("CmpDate", typeof(string));
        dtRecChang.Columns.Add("PrvsId", typeof(string));
        foreach (GridViewRow gdVr in GVPrev.Rows)
        {
            DataRow drtemp = dtRecChang.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            temp = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            temp = ((TextBox)gdVr.FindControl("txtRecNotes")).Text;
            temp = ((TextBox)gdVr.FindControl("txtCDG")).Text;
            temp = ((TextBox)gdVr.FindControl("txtRevPeriod")).Text;
            temp = ((TextBox)gdVr.FindControl("txtCmpDateDate")).Text;
            temp = ((Label)gdVr.FindControl("hfPrIdBhv")).Text;
            drtemp["Recommendation"] = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            drtemp["PersonResponsible"] = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            drtemp["Status"] = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            drtemp["Notes"] = ((TextBox)gdVr.FindControl("txtRecNotes")).Text;
            drtemp["CDGoal"] = ((TextBox)gdVr.FindControl("txtCDG")).Text;
            drtemp["RevPeriod"] = ((TextBox)gdVr.FindControl("txtRevPeriod")).Text;
            drtemp["CmpDate"] = ((TextBox)gdVr.FindControl("txtCmpDateDate")).Text;
            drtemp["PrvsId"] = ((Label)gdVr.FindControl("hfPrIdBhv")).Text;
            dtRecChang.Rows.Add(drtemp);
        }
        if (dtRecChang.Rows.Count == 1)
        {
            dtRecChang.Rows.RemoveAt(intexRow);
            loadRecChange();
        }
        else
        {
            dtRecChang.Rows.RemoveAt(intexRow);
        }

        GVPrev.DataSource = dtRecChang;
        GVPrev.DataBind();

    }

    public void delRowMed(int intexRow)
    {
        dtChangMed.Columns.Add("Recommendation_Med", typeof(string));
        dtChangMed.Columns.Add("PersonResponsible_Med", typeof(string));
        dtChangMed.Columns.Add("Status_Med", typeof(string));
        dtChangMed.Columns.Add("Notes_Med", typeof(string));
        dtChangMed.Columns.Add("CDGoal_Med", typeof(string));
        dtChangMed.Columns.Add("RevPeriod_Med", typeof(string));
        dtChangMed.Columns.Add("CmpDate_Med", typeof(string));
        dtChangMed.Columns.Add("PrvsId", typeof(string));
        foreach (GridViewRow gdVr in GVMedical.Rows)
        {
            DataRow drtemp = dtChangMed.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomdMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLineMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtPerResMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtRecNotesMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtCDGMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtRevPeriodMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtCmpDateDateMed")).Text;
            temp = ((Label)gdVr.FindControl("hfPrIdMed")).Text;
            drtemp["Recommendation_Med"] = ((TextBox)gdVr.FindControl("txtRecomdMed")).Text;
            drtemp["PersonResponsible_Med"] = ((TextBox)gdVr.FindControl("txtTimeLineMed")).Text;
            drtemp["Status_Med"] = ((TextBox)gdVr.FindControl("txtPerResMed")).Text;
            drtemp["Notes_Med"] = ((TextBox)gdVr.FindControl("txtRecNotesMed")).Text;
            drtemp["CDGoal_Med"] = ((TextBox)gdVr.FindControl("txtCDGMed")).Text;
            drtemp["RevPeriod_Med"] = ((TextBox)gdVr.FindControl("txtRevPeriodMed")).Text;
            drtemp["CmpDate_Med"] = ((TextBox)gdVr.FindControl("txtCmpDateDateMed")).Text;
            drtemp["PrvsId"] = ((Label)gdVr.FindControl("hfPrIdMed")).Text;
            dtChangMed.Rows.Add(drtemp);
        }
        if (dtChangMed.Rows.Count == 1)
        {
            dtChangMed.Rows.RemoveAt(intexRow);
            loadChangMed();
        }
        else
        {
            dtChangMed.Rows.RemoveAt(intexRow);
        }

        GVMedical.DataSource = dtChangMed;
        GVMedical.DataBind();

    }


    public void delRowPsy(int intexRow)
    {
        dtChangPsy.Columns.Add("Recommendation_Psy", typeof(string));
        dtChangPsy.Columns.Add("PersonResponsible_Psy", typeof(string));
        dtChangPsy.Columns.Add("Status_Psy", typeof(string));
        dtChangPsy.Columns.Add("Notes_Psy", typeof(string));
        dtChangPsy.Columns.Add("CDGoal_Psy", typeof(string));
        dtChangPsy.Columns.Add("RevPeriod_Psy", typeof(string));
        dtChangPsy.Columns.Add("CmpDate_Psy", typeof(string));
        dtChangPsy.Columns.Add("PrvsId", typeof(string));
        foreach (GridViewRow gdVr in GVPsy.Rows)
        {
            DataRow drtemp = dtChangPsy.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomdPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLineMPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtPerResPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtRecNotesPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtCDGPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtRevPeriodPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtCmpDateDatePsy")).Text;
            temp = ((Label)gdVr.FindControl("hfPrIdPsy")).Text;
            drtemp["Recommendation_Psy"] = ((TextBox)gdVr.FindControl("txtRecomdPsy")).Text;
            drtemp["PersonResponsible_Psy"] = ((TextBox)gdVr.FindControl("txtTimeLineMPsy")).Text;
            drtemp["Status_Psy"] = ((TextBox)gdVr.FindControl("txtPerResPsy")).Text;
            drtemp["Notes_Psy"] = ((TextBox)gdVr.FindControl("txtRecNotesPsy")).Text;
            drtemp["CDGoal_Psy"] = ((TextBox)gdVr.FindControl("txtCDGPsy")).Text;
            drtemp["RevPeriod_Psy"] = ((TextBox)gdVr.FindControl("txtRevPeriodPsy")).Text;
            drtemp["CmpDate_Psy"] = ((TextBox)gdVr.FindControl("txtCmpDateDatePsy")).Text;
            drtemp["PrvsId"] = ((Label)gdVr.FindControl("hfPrIdPsy")).Text;
            dtChangPsy.Rows.Add(drtemp);
        }
        if (dtChangPsy.Rows.Count == 1)
        {
            dtChangPsy.Rows.RemoveAt(intexRow);
            loadChangPsy();
        }
        else
        {
            dtChangPsy.Rows.RemoveAt(intexRow);
        }

        GVPsy.DataSource = dtChangPsy;
        GVPsy.DataBind();

    }

    public void delRowBhvUp(int intexRow)
    {
        dtChangBhvUp.Columns.Add("Notes_Bhv", typeof(string));
        dtChangBhvUp.Columns.Add("Recommendation_Bhv", typeof(string));
        dtChangBhvUp.Columns.Add("PersonResponsible_Bhv", typeof(string));
        dtChangBhvUp.Columns.Add("Goal_bhv", typeof(string));
        dtChangBhvUp.Columns.Add("hdPerRespBhv", typeof(string));
        foreach (GridViewRow gdVr in GVBhvUp.Rows)
        {
            DataRow drtemp = dtChangBhvUp.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomdBhvUp")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeBhvup")).Text;
            //temp = ((DropDownList)gdVr.FindControl("ddlPerRespBhv")).SelectedValue;
            temp = ((TextBox)gdVr.FindControl("txtPerRespBhv")).Text;
            temp = ((TextBox)gdVr.FindControl("txtDateBhv")).Text;
            drtemp["Notes_Bhv"] = ((TextBox)gdVr.FindControl("txtRecomdBhvUp")).Text;
            drtemp["Recommendation_Bhv"] = ((TextBox)gdVr.FindControl("txtTimeBhvup")).Text;
            //drtemp["PersonResponsible_Bhv"] = ((DropDownList)gdVr.FindControl("ddlPerRespBhv")).SelectedValue;
            drtemp["PersonResponsible_Bhv"] = ((TextBox)gdVr.FindControl("txtPerRespBhv")).Text;
            drtemp["hdPerRespBhv"] = ((TextBox)gdVr.FindControl("hdPerRespBhv")).Text;
            drtemp["Goal_bhv"] = ((TextBox)gdVr.FindControl("txtDateBhv")).Text;
            dtChangBhvUp.Rows.Add(drtemp);
        }
        if (dtChangBhvUp.Rows.Count == 1)
        {
            dtChangBhvUp.Rows.RemoveAt(intexRow);
            loadBhvUp();
        }
        else
        {
            dtChangBhvUp.Rows.RemoveAt(intexRow);
        }

        GVBhvUp.DataSource = dtChangBhvUp;
        GVBhvUp.DataBind();

    }

    public void delRowMedUp(int intexRow)
    {
        dtChangMedUp.Columns.Add("Notes_Med_Up", typeof(string));
        dtChangMedUp.Columns.Add("Recommendation_Med_Up", typeof(string));
        dtChangMedUp.Columns.Add("PersonResponsible_Med_Up", typeof(string));
        dtChangMedUp.Columns.Add("hdPerRespMed", typeof(string));
        dtChangMedUp.Columns.Add("Goal_Med_Up", typeof(string));
        foreach (GridViewRow gdVr in GVMedicalUp.Rows)
        {
            DataRow drtemp = dtChangMedUp.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomdMedUp")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLineMedUp")).Text;
            //temp = ((DropDownList)gdVr.FindControl("ddlPerRespMed")).SelectedValue;
            temp = ((TextBox)gdVr.FindControl("txtPerRespMed")).Text;
            temp = ((TextBox)gdVr.FindControl("txtDateMed")).Text;
            drtemp["Notes_Med_Up"] = ((TextBox)gdVr.FindControl("txtRecomdMedUp")).Text;
            drtemp["Recommendation_Med_Up"] = ((TextBox)gdVr.FindControl("txtTimeLineMedUp")).Text;
            //drtemp["PersonResponsible_Med_Up"] = ((DropDownList)gdVr.FindControl("ddlPerRespMed")).SelectedValue ;
            drtemp["PersonResponsible_Med_Up"] = ((TextBox)gdVr.FindControl("txtPerRespMed")).Text;
            drtemp["hdPerRespMed"] = ((TextBox)gdVr.FindControl("hdPerRespMed")).Text;
            drtemp["Goal_Med_Up"] = ((TextBox)gdVr.FindControl("txtDateMed")).Text;
            dtChangMedUp.Rows.Add(drtemp);
        }
        if (dtChangMedUp.Rows.Count == 1)
        {
            dtChangMedUp.Rows.RemoveAt(intexRow);
            loadMedUp();
        }
        else
        {
            dtChangMedUp.Rows.RemoveAt(intexRow);
        }

        GVMedicalUp.DataSource = dtChangMedUp;
        GVMedicalUp.DataBind();

    }

    public void delRowPsyUp(int intexRow)
    {
        dtChangPsyUp.Columns.Add("Notes_Psy_Up", typeof(string));
        dtChangPsyUp.Columns.Add("Recommendation_Psy_Up", typeof(string));
        dtChangPsyUp.Columns.Add("PersonResponsible_Psy_Up", typeof(string));
        dtChangPsyUp.Columns.Add("hdPerRespPsy", typeof(string));
        dtChangPsyUp.Columns.Add("Goal_Psy_Up", typeof(string));
        foreach (GridViewRow gdVr in GVPsyUp.Rows)
        {
            DataRow drtemp = dtChangPsyUp.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomdPsyUp")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLineMPsyUp")).Text;
            //temp = ((DropDownList)gdVr.FindControl("ddlPerRespPsy")).SelectedValue;
            temp = ((TextBox)gdVr.FindControl("txtPerRespPsy")).Text;
            temp = ((TextBox)gdVr.FindControl("txtDatePsy")).Text;
            drtemp["Notes_Psy_Up"] = ((TextBox)gdVr.FindControl("txtRecomdPsyUp")).Text;
            drtemp["Recommendation_Psy_Up"] = ((TextBox)gdVr.FindControl("txtTimeLineMPsyUp")).Text;
            //drtemp["PersonResponsible_Psy_Up"] = ((DropDownList)gdVr.FindControl("ddlPerRespPsy")).SelectedValue;
            drtemp["PersonResponsible_Psy_Up"] = ((TextBox)gdVr.FindControl("txtPerRespPsy")).Text;
            drtemp["hdPerRespPsy"] = ((TextBox)gdVr.FindControl("hdPerRespPsy")).Text;
            drtemp["Goal_Psy_Up"] = ((TextBox)gdVr.FindControl("txtDatePsy")).Text;
            dtChangPsyUp.Rows.Add(drtemp);
        }
        if (dtChangPsyUp.Rows.Count == 1)
        {
            dtChangPsyUp.Rows.RemoveAt(intexRow);
            loadPsyUp();
        }
        else
        {
            dtChangPsyUp.Rows.RemoveAt(intexRow);
        }

        GVPsyUp.DataSource = dtChangPsyUp;
        GVPsyUp.DataBind();

    }



    protected void GVPrev_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (GVPrev.Rows.Count == 5) return;
            addRow();
        }
        if (e.CommandName == "delete")
        {
            int rowId = int.Parse(e.CommandArgument.ToString());
            delPreRecFromDB(rowId);
        }

    }

    protected void delPreRecFromDB(int rowId)
    {
        if (divSubByOn.Visible == false)
        {
            objData = new clsData();
            string sQuery = "delete from PrvsRecomendations where PrvsId=" + rowId;
            objData.Execute(sQuery);
        }
    }

    protected void GVPrev_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRow(e.RowIndex);
    }
    protected void GVMedical_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanelMed, UpdatePanelMed.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (GVMedical.Rows.Count == 5) return;
            addRowMed();
        }
        if (e.CommandName == "delete")
        {
            int rowId = int.Parse(e.CommandArgument.ToString());
            delPreRecFromDB(rowId);
        }

    }
    protected void GVMedical_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowMed(e.RowIndex);
    }
    protected void GVPsy_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanelPsy, UpdatePanelPsy.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (GVPsy.Rows.Count == 5) return;
            addRowPsy();
        }
        if (e.CommandName == "delete")
        {
            int rowId = int.Parse(e.CommandArgument.ToString());
            delPreRecFromDB(rowId);
        }
    }
    protected void GVPsy_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowPsy(e.RowIndex);
    }
    protected void GVBhvUp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanelPUp, UpdatePanelPUp.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (GVBhvUp.Rows.Count == 5) return;
            addRowBhvUp();
        }

    }
    protected void GVBhvUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowBhvUp(e.RowIndex);
    }
    protected void GVMedicalUp_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        ScriptManager.RegisterClientScriptBlock(UpdatePanelMedUp, UpdatePanelMedUp.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (GVMedicalUp.Rows.Count == 5) return;
            addRowMedUp();
            //$(".autosuggest").autocomplete({Source: data});
        }

    }
    protected void GVMedicalUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowMedUp(e.RowIndex);
    }

    protected void GVPsyUp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanelPsyUp, UpdatePanelPsyUp.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (GVPsyUp.Rows.Count == 5) return;
            addRowPsyUp();
        }

    }
    protected void GVPsyUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowPsyUp(e.RowIndex);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string date = "";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        string injury = "False";
        string challenge = "False";

        Button b1 = (Button)sender;
        string bText = b1.Text;

        //if (bText == "Submit")
        //{
        //    nowDate = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        //    lastDate = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        //}
        if (chkInjury.Checked == true)
        {
            injury = "True";
        }
        if (chkChallenge.Checked == true)
        {
            challenge = "True";
        }

        if (ViewState["CurrentDate"] != null)
        {
            date = ViewState["CurrentDate"].ToString();
        }

        string RFrom = "";
        string RTo = "";
        //int ddlId = 0;
        //int ddlIdCRecBhv = 0;
        //int ddlIdCRecMed = 0;
        //int ddlIdCRecPsy = 0;
        int? txtId = 0;
        int? txtIdCRecBhv = 0;
        int? txtIdCRecMed = 0;
        int? txtIdCRecPsy = 0;
        //int width;
        foreach (GridViewRow Gvw in GVBasicDetails.Rows)
        {
            if ((((TextBox)Gvw.FindControl("txtRevFrom")).Text != "") || (((TextBox)Gvw.FindControl("txtRevTo")).Text != ""))
            {
                RFrom = clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRevFrom")).Text);
                RTo = clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRevTo")).Text);
            }
            //DropDownList ddlCME = (DropDownList)Gvw.FindControl("ddlCaseManEdu");
            //string ddlCMESelVal = ddlCME.SelectedValue;
            //ddlId = Convert.ToInt32(ddlCMESelVal);
            //TextBox txtCME = (TextBox)Gvw.FindControl("txtCaseManEdu");
            //HiddenField hdFldCME = (HiddenField)Gvw.FindControl("hdFldtxtCaseManEdu");
            TextBox txtCME = (TextBox)Gvw.FindControl("hdCaseManEdu");
            string txtCMESelVal = txtCME.Text;
            //string CMESelVal = txtCMESelVal.ToString().Trim();
            if (txtCMESelVal != "")
            {
                txtId = Convert.ToInt32(txtCMESelVal);
            }
            else
                txtId = null;
           
        }

        if (bText == "Submit")
        {
            if (hdFldCvid.Value != "")
            {    //changed ddlId to txtId
                string strquery = "UPDATE StdtGrandRoundCvrsheet SET SignificantInjuries='" + injury + "',SignificantNotes='" + clsGeneral.convertQuotes(txtInjury.Text) + "',Challenges='" + challenge + "',ChallengesNotes='" + clsGeneral.convertQuotes(txtChallenge.Text) + "',CaseManEdu='" + txtId + "',SubmittedDate=GETDATE(),CreatedBy='"+sess.LoginId+"',CreatedOn=GETDATE() WHERE StdtCvrId=" + int.Parse(hdFldCvid.Value) + " AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + "";
                objData.Execute(strquery);
                tdMsg2.InnerHtml = clsGeneral.sucessMsg("Submitted Successfully");

                strquery = "DELETE FROM PrvsRecomendations WHERE StdtCvrId=" + int.Parse(hdFldCvid.Value);
                objData.Execute(strquery);

                foreach (GridViewRow Gvw in GVPrev.Rows)
                {
                    if ((((TextBox)Gvw.FindControl("txtPerRes")).Text != ""))
                    {
                        strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerRes")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                        objData.Execute(strquery);
                    }
                    if ((((TextBox)Gvw.FindControl("txtCmpDateDate")).Text != ""))
                    {
                        strquery = "update PrvsRecomendations set SubmissionStatus='S',CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                        objData.Execute(strquery);
                    }
                }
                foreach (GridViewRow Gvw in GVMedical.Rows)
                {
                    if ((((TextBox)Gvw.FindControl("txtPerResMed")).Text != ""))
                    {
                        strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResMed")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                        objData.Execute(strquery);
                    }
                    if ((((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text != ""))
                    {
                        strquery = "update PrvsRecomendations set SubmissionStatus='S',CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                        objData.Execute(strquery);
                    }
                }

                foreach (GridViewRow Gvw in GVPsy.Rows)
                {
                    if ((((TextBox)Gvw.FindControl("txtPerResPsy")).Text != ""))
                    {
                        strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResPsy")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                        objData.Execute(strquery);
                    }
                    if ((((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text != ""))
                    {
                        strquery = "update PrvsRecomendations set SubmissionStatus='S',CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                        objData.Execute(strquery);
                    }
                }

                foreach (GridViewRow Gvw in GVBhvUp.Rows)
                {
                    //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespBhv");
                    //string ddlPRSelVal = ddlPR.SelectedValue;
                    //ddlIdCRecBhv = Convert.ToInt32(ddlPRSelVal);
                    //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespBhv");
                    //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespBhv");
                    TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespBhv");
                    string txtPRSelVal = txtPR.Text;
                    if (txtPRSelVal != "")
                    {
                        txtIdCRecBhv = Convert.ToInt32(txtPRSelVal);
                    }
                    else
                        txtIdCRecBhv = null;

                    if ((((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeBhvup")).Text != "") || txtIdCRecBhv > 0 || (((TextBox)Gvw.FindControl("txtDateBhv")).Text != ""))
                    {
                        strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeBhvup")).Text) + "','" + txtIdCRecBhv + "','BehaveUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' END)";
                        objData.Execute(strquery);
                    }
                }

                foreach (GridViewRow Gvw in GVMedicalUp.Rows)
                {
                    //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespMed");
                    //string ddlPRSelVal = ddlPR.SelectedValue;
                    //ddlIdCRecMed = Convert.ToInt32(ddlPRSelVal);
                    //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespMed");
                   // HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespMed");
                    TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespMed");
                    string txtPRSelVal = txtPR.Text;
                    if (txtPRSelVal != "")
                    {
                        txtIdCRecMed = Convert.ToInt32(txtPRSelVal);
                    }
                    else
                        txtIdCRecMed = null;

                    if ((((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text != "") || txtIdCRecMed > 0 || (((TextBox)Gvw.FindControl("txtDateMed")).Text != ""))
                    {
                        strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text) + "','" + txtIdCRecMed + "','MedicalUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' END)";
                        objData.Execute(strquery);
                    }
                }

                foreach (GridViewRow Gvw in GVPsyUp.Rows)
                {
                    //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespPsy");
                    //string ddlPRSelVal = ddlPR.SelectedValue;
                    //ddlIdCRecPsy = Convert.ToInt32(ddlPRSelVal);
                    //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespPsy");
                    //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespPsy");
                    TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespPsy");
                    string txtPRSelVal = txtPR.Text;
                    if (txtPRSelVal != "")
                    {
                        txtIdCRecPsy = Convert.ToInt32(txtPRSelVal);
                    }
                    else
                        txtIdCRecPsy = null;

                    if ((((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text != "") || txtIdCRecPsy > 0 || (((TextBox)Gvw.FindControl("txtDatePsy")).Text != ""))
                    {
                        strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text) + "','" + txtIdCRecPsy + "','PsychiatryUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' END)";
                        objData.Execute(strquery);
                    }
                }
            }
            else
            {

                string strquery = "insert into StdtGrandRoundCvrsheet(SchoolId,ClassId,StudentId,StartDate,EndDate,SignificantInjuries,SignificantNotes,Challenges,ChallengesNotes,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,CaseManEdu,SubmittedDate) values('" + sess.SchoolId + "','" + sess.Classid + "','" + sess.StudentId + "','" + RFrom + "','" + RTo + "','" + injury + "','" + clsGeneral.convertQuotes(txtInjury.Text) + "','" + challenge + "','" + clsGeneral.convertQuotes(txtChallenge.Text) + "',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + txtId + "',GETDATE())";
                int CvId = objData.ExecuteWithScope(strquery);
                if (CvId > 0)
                {
                    tdMsg2.InnerHtml = clsGeneral.sucessMsg("Submitted Successfully");
                    foreach (GridViewRow Gvw in GVPrev.Rows)
                    {
                        if ((((TextBox)Gvw.FindControl("txtPerRes")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerRes")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                            objData.Execute(strquery);
                        }
                        if ((((TextBox)Gvw.FindControl("txtCmpDateDate")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set SubmissionStatus='S',CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                            objData.Execute(strquery);
                        }
                    }
                    foreach (GridViewRow Gvw in GVMedical.Rows)
                    {
                        if ((((TextBox)Gvw.FindControl("txtPerResMed")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResMed")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                            objData.Execute(strquery);
                        }
                        if ((((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set SubmissionStatus='S',CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVPsy.Rows)
                    {
                        if ((((TextBox)Gvw.FindControl("txtPerResPsy")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResPsy")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                            objData.Execute(strquery);
                        }
                        if ((((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set SubmissionStatus='S',CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVBhvUp.Rows)
                    {
                        //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespBhv");
                        //string ddlPRSelVal = ddlPR.SelectedValue;
                        //ddlIdCRecBhv = Convert.ToInt32(ddlPRSelVal);
                        //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespBhv");
                        //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespBhv");
                        TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespBhv");
                        string txtPRSelVal = txtPR.Text;
                        if (txtPRSelVal != "")
                        {
                            txtIdCRecBhv = Convert.ToInt32(txtPRSelVal);
                        }
                        else
                            txtIdCRecBhv = null;

                        if ((((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeBhvup")).Text != "") || txtIdCRecBhv > 0 || (((TextBox)Gvw.FindControl("txtDateBhv")).Text != ""))
                        {
                            strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + CvId + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeBhvup")).Text) + "','" + txtIdCRecBhv + "','BehaveUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' END)";
                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVMedicalUp.Rows)
                    {
                        //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespMed");
                        //string ddlPRSelVal = ddlPR.SelectedValue;
                        //ddlIdCRecMed = Convert.ToInt32(ddlPRSelVal);
                        //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespMed");
                       // HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespMed");
                        TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespMed");
                        string txtPRSelVal = txtPR.Text;
                        if (txtPRSelVal != "")
                        {
                            txtIdCRecMed = Convert.ToInt32(txtPRSelVal);
                        }
                        else
                            txtIdCRecMed = null;

                        if ((((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text != "") || txtIdCRecMed > 0 || (((TextBox)Gvw.FindControl("txtDateMed")).Text != ""))
                        {
                            strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + CvId + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text) + "','" + txtIdCRecMed + "','MedicalUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' END)";
                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVPsyUp.Rows)
                    {
                        //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespPsy");
                        //string ddlPRSelVal = ddlPR.SelectedValue;
                        //ddlIdCRecPsy = Convert.ToInt32(ddlPRSelVal);
                        //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespPsy");
                        //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespPsy");
                        TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespPsy");
                        string txtPRSelVal = txtPR.Text;
                        if (txtPRSelVal != "")
                        {
                            txtIdCRecPsy = Convert.ToInt32(txtPRSelVal);
                        }
                        else
                            txtIdCRecPsy = null;

                        if ((((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text != "") || txtIdCRecPsy > 0 || (((TextBox)Gvw.FindControl("txtDatePsy")).Text != ""))
                        {
                            strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + CvId + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text) + "','" + txtIdCRecPsy + "','PsychiatryUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' END)";
                            objData.Execute(strquery);
                        }
                    }
                }
            }
        }
        if (bText == "Save Draft")
        {
            
            string query = "";

            if (hdFldCvid.Value == "")
            {
                string strquery = "insert into StdtGrandRoundCvrsheet(SchoolId,ClassId,StudentId,StartDate,EndDate,SignificantInjuries,SignificantNotes,Challenges,ChallengesNotes,CaseManEdu) values('" + sess.SchoolId + "','" + sess.Classid + "','" + sess.StudentId + "','" + RFrom + "','" + RTo + "','" + injury + "','" + clsGeneral.convertQuotes(txtInjury.Text) + "','" + challenge + "','" + clsGeneral.convertQuotes(txtChallenge.Text) + "','" + txtId + "')";
                int CvId = objData.ExecuteWithScope(strquery);
                if (CvId > 0)
                {
                    tdMsg2.InnerHtml = clsGeneral.sucessMsg("Saved Successfully");
                    foreach (GridViewRow Gvw in GVPrev.Rows)
                    {
                        if ((((TextBox)Gvw.FindControl("txtPerRes")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerRes")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                            objData.Execute(strquery);
                        }
                        if ((((TextBox)Gvw.FindControl("txtCmpDateDate")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                            objData.Execute(strquery);
                        }
                    }
                    foreach (GridViewRow Gvw in GVMedical.Rows)
                    {
                        if ((((TextBox)Gvw.FindControl("txtPerResMed")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResMed")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                            objData.Execute(strquery);
                        }
                        if ((((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVPsy.Rows)
                    {
                        if ((((TextBox)Gvw.FindControl("txtPerResPsy")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResPsy")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                            objData.Execute(strquery);
                        }
                        if ((((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text != ""))
                        {
                            strquery = "update PrvsRecomendations set CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVBhvUp.Rows)
                    {
                        //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespBhv");
                        //string ddlPRSelVal = ddlPR.SelectedValue;
                        //ddlIdCRecBhv = Convert.ToInt32(ddlPRSelVal);
                        //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespBhv");
                        //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespBhv");
                        TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespBhv");
                        string txtPRSelVal = txtPR.Text;
                        if (txtPRSelVal != "")
                        {
                            txtIdCRecBhv = Convert.ToInt32(txtPRSelVal);
                        }
                        else
                            txtIdCRecBhv = null;

                        if ((((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeBhvup")).Text != "") || txtIdCRecBhv > 0 || (((TextBox)Gvw.FindControl("txtDateBhv")).Text != ""))
                        {
                            strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + CvId + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeBhvup")).Text) + "','" + txtIdCRecBhv + "','BehaveUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' END)";
                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVMedicalUp.Rows)
                    {
                        //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespMed");
                        //string ddlPRSelVal = ddlPR.SelectedValue;
                        //ddlIdCRecMed = Convert.ToInt32(ddlPRSelVal);
                        //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespMed");
                        //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespMed");
                        TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespMed");
                        string txtPRSelVal = txtPR.Text;
                        if (txtPRSelVal != "")
                        {
                            txtIdCRecMed = Convert.ToInt32(txtPRSelVal);
                        }
                        else
                            txtIdCRecMed = null;

                        if ((((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text != "") || txtIdCRecMed > 0 || (((TextBox)Gvw.FindControl("txtDateMed")).Text != ""))
                        {
                            strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + CvId + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text) + "','" + txtIdCRecMed + "','MedicalUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' END)";
                            objData.Execute(strquery);
                        }
                    }

                    foreach (GridViewRow Gvw in GVPsyUp.Rows)
                    {
                        //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespPsy");
                        //string ddlPRSelVal = ddlPR.SelectedValue;
                        //ddlIdCRecPsy = Convert.ToInt32(ddlPRSelVal);
                        //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespPsy");
                        //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespPsy");
                        TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespPsy");
                        string txtPRSelVal = txtPR.Text;
                        if (txtPRSelVal != "")
                        {
                            txtIdCRecPsy = Convert.ToInt32(txtPRSelVal);
                        }
                        else
                            txtIdCRecPsy = null;

                        if ((((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text != "") || txtIdCRecPsy > 0 || (((TextBox)Gvw.FindControl("txtDatePsy")).Text != ""))
                        {
                            strquery = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + CvId + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text) + "','" + txtIdCRecPsy + "','PsychiatryUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' END)";
                            objData.Execute(strquery);
                        }
                    }
                }
            }
            else
            {
                
                query = "UPDATE StdtGrandRoundCvrsheet SET SignificantInjuries='" + injury + "',SignificantNotes='" + clsGeneral.convertQuotes(txtInjury.Text) + "',Challenges='" + challenge + "',ChallengesNotes='" + clsGeneral.convertQuotes(txtChallenge.Text) + "',CaseManEdu='" + txtId + "' WHERE StdtCvrId=" + int.Parse(hdFldCvid.Value) + " AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + "";
                objData.Execute(query);
                tdMsg2.InnerHtml = clsGeneral.sucessMsg("Saved Successfully");
                
                query = "DELETE FROM PrvsRecomendations WHERE StdtCvrId=" + int.Parse(hdFldCvid.Value);
                objData.Execute(query);

                foreach (GridViewRow Gvw in GVPrev.Rows)
                {
                    if ((((TextBox)Gvw.FindControl("txtPerRes")).Text != ""))
                    {
                        query = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerRes")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                        objData.Execute(query);
                    }
                    if ((((TextBox)Gvw.FindControl("txtCmpDateDate")).Text != ""))
                    {
                        query = "update PrvsRecomendations set CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDate")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdBhv")).Text;
                        objData.Execute(query);
                    }
                }
                foreach (GridViewRow Gvw in GVMedical.Rows)
                {
                    if ((((TextBox)Gvw.FindControl("txtPerResMed")).Text != ""))
                    {
                        query = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResMed")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                        objData.Execute(query);
                    }
                    if ((((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text != ""))
                    {
                        query = "update PrvsRecomendations set CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDateMed")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdMed")).Text;

                        objData.Execute(query);
                    }
                }

                foreach (GridViewRow Gvw in GVPsy.Rows)
                {
                    if ((((TextBox)Gvw.FindControl("txtPerResPsy")).Text != ""))
                    {
                        query = "update PrvsRecomendations set StatusUpdate='" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtPerResPsy")).Text) + "' where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                        objData.Execute(query);
                    }
                    if ((((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text != ""))
                    {
                        query = "update PrvsRecomendations set CompletedDate=CASE WHEN'" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtCmpDateDatePsy")).Text + "' END where PrvsId=" + ((Label)Gvw.FindControl("hfPrIdPsy")).Text;

                        objData.Execute(query);
                    }
                }

                foreach (GridViewRow Gvw in GVBhvUp.Rows)
                {
                    //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespBhv");
                    //string ddlPRSelVal = ddlPR.SelectedValue;
                    //ddlIdCRecBhv = Convert.ToInt32(ddlPRSelVal);
                    //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespBhv");
                    //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespBhv");
                    TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespBhv");
                    string txtPRSelVal = txtPR.Text;
                    if (txtPRSelVal != "")
                    {
                        txtIdCRecBhv = Convert.ToInt32(txtPRSelVal);
                    }
                    else
                        txtIdCRecBhv = null;

                    if ((((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeBhvup")).Text != "") || txtIdCRecBhv > 0 || (((TextBox)Gvw.FindControl("txtDateBhv")).Text != ""))
                    {
                        query = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeBhvup")).Text) + "','" + txtIdCRecBhv + "','BehaveUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdBhvUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateBhv")).Text + "' END)";
                        objData.Execute(query);
                    }
                }

                foreach (GridViewRow Gvw in GVMedicalUp.Rows)
                {
                    //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespMed");
                    //string ddlPRSelVal = ddlPR.SelectedValue;
                    //ddlIdCRecMed = Convert.ToInt32(ddlPRSelVal);
                    //TextBox txtPR = (TextBox)Gvw.FindControl("txtPerRespMed");
                    //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespMed");
                    TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespMed");
                    string txtPRSelVal = txtPR.Text;
                    if (txtPRSelVal != "")
                    {
                        txtIdCRecMed = Convert.ToInt32(txtPRSelVal);
                    }
                    else
                        txtIdCRecMed = null;

                    if ((((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text != "") || txtIdCRecMed > 0 || (((TextBox)Gvw.FindControl("txtDateMed")).Text != ""))
                    {
                        query = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMedUp")).Text) + "','" + txtIdCRecMed + "','MedicalUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdMedUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDateMed")).Text + "' END)";
                        objData.Execute(query);
                    }
                }

                foreach (GridViewRow Gvw in GVPsyUp.Rows)
                {
                    //DropDownList ddlPR = (DropDownList)Gvw.FindControl("ddlPerRespPsy");
                    //string ddlPRSelVal = ddlPR.SelectedValue;
                    //ddlIdCRecPsy = Convert.ToInt32(ddlPRSelVal);
                    
                    //HiddenField hdFldPR = (HiddenField)Gvw.FindControl("hdFldtxtPerRespPsy");
                    TextBox txtPR = (TextBox)Gvw.FindControl("hdPerRespPsy");
                    string txtPRSelVal = txtPR.Text;
                    if (txtPRSelVal != "")
                    {
                        txtIdCRecPsy = Convert.ToInt32(txtPRSelVal);
                    }
                    else
                        txtIdCRecPsy = null;

                    if ((((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text != "") || (((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text != "") || txtIdCRecPsy > 0 || (((TextBox)Gvw.FindControl("txtDatePsy")).Text != ""))
                    {
                        query = "insert into PrvsRecomendations(StdtCvrId,PreRecom,PersonResp2,PrvsRecordType,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,RecNotes,CompletionDateGoal) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtTimeLineMPsyUp")).Text) + "','" + txtIdCRecPsy + "','PsychiatryUp',GETDATE(),'" + sess.LoginId + "',GETDATE(),'" + sess.LoginId + "','" + clsGeneral.convertQuotes(((TextBox)Gvw.FindControl("txtRecomdPsyUp")).Text) + "',CASE WHEN'" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' = '' THEN NULL ELSE '" + ((TextBox)Gvw.FindControl("txtDatePsy")).Text + "' END)";
                        objData.Execute(query);
                    }
                }
            }
        }
        

        LoadBasicData();
        FillReportGrand(date);
        LoadDate();



    }
    protected void grDateList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        objData = new clsData();
        string date = "";
        date = e.CommandArgument.ToString();
        ViewState["CurrentDate"] = date;
        FillCvrShtHfVal(date);
        LoadBasicData();
        FillReportGrand(date);
        tdMsg2.InnerHtml = "";
    }

    protected void GVBasicDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string CSdate = "";
            string CMEduVal = "0";
            if (ViewState["CurrentDate"] != null)
            {
                CSdate = ViewState["CurrentDate"].ToString();
            }
            string stDate = "";
            string endDate = "";
            if (CSdate != null)
            {
                stDate = CSdate.Split('-')[0];
                endDate = CSdate.Split('-')[1];
            }
            if (stDate != "" && endDate != "")
            {
                System.Data.DataTable DtRev = new System.Data.DataTable();
                string sQuery = "select CaseManEdu from StdtGrandRoundCvrsheet where StartDate='" + stDate + "' and EndDate='" + endDate + "' and SchoolId='" + sess.SchoolId + "' and StudentId='" + sess.StudentId + "'";
                DtRev = objData.ReturnDataTable(sQuery, false);
                if (DtRev.Rows.Count > 0)
                {
                    if (DtRev.Rows[0]["CaseManEdu"].ToString() != "")
                        CMEduVal = DtRev.Rows[0]["CaseManEdu"].ToString();
                }
            }
            System.Data.DataTable Dtv = new System.Data.DataTable();
            string sRQuery = "select UserId, UserLName+','+UserFName as Name from [User] where UserId="+CMEduVal;
            Dtv = objData.ReturnDataTable(sRQuery, false);
            TextBox txtCME = (TextBox)e.Row.FindControl("txtCaseManEdu");
            TextBox hdCaseManEdu = (TextBox)e.Row.FindControl("hdCaseManEdu");
            if (Dtv.Rows.Count > 0)
            {
                if (Dtv.Rows[0]["Name"].ToString() != "")
                {
                    txtCME.Text = Dtv.Rows[0]["Name"].ToString();
                    hdCaseManEdu.Text = Dtv.Rows[0]["UserId"].ToString();
                }
            }
           // DropDownList ddlCME = (DropDownList)e.Row.FindControl("ddlCaseManEdu");
           // objData.ReturnDropDown("select UserId as Id,UserLName+', '+UserFName as Name from [user] where ActiveInd='A'", ddlCME);
           // ddlCME.Items.FindByValue(CMEduVal).Selected = true;
            
           
        }
    }
    
    [WebMethod]
    public static string[] GetAutoCompleteData(string prefix)
    {  
        List<string> names = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select CONVERT(VARCHAR(10),UserId)+'-'+UserLName+', '+UserFName as Name from [user] where ActiveInd='A' and UserLName like @SearchText + '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //names.Add(string.Format("{0}-{1}", sdr["Name"], sdr["Id"]));
                        
                        names.Add(sdr["Name"].ToString());
                        
                    }
                    
                }
                
            }
            conn.Close();
        }
        return names.ToArray();
        
    }
    


    


    protected void btnGenNewRTFSheet_Click(object sender, EventArgs e)  //Date popup
    {

        txtEdate.Text = DateTime.Now.Date.ToString("MM'/'dd'/'yyyy");
        DateTime Edate = DateTime.Now.Date.AddDays(-90);
        txtSdate.Text = Edate.Date.ToString("MM'/'dd'/'yyyy");
        string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        //LoadBasicData();
        //LoadDate();
        //FillReport();
    }
    protected void btnGenRTF_Click(object sender, EventArgs e)  //New RTF sheet generator
    {
        if (validate() == true)
        {
            hdFldCvid.Value = "";
            FillReport();
            LoadBasicData();
            LoadDate();
            
        }
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
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select  Start Date");
            return result;
        }
        else if (txtEdate.Text == "")
        {
            result = false;
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select End Date");
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
                tdMessage.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
                return result;
            }

        }

        return result;
    }
    protected void GVBhvUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            objData = new clsData();
            string CMEduVal = "0";
            Label lblCvId = (Label)e.Row.FindControl("hfPerRespBhv");
            if(lblCvId.Text != "")
            CMEduVal = lblCvId.Text;
           // DropDownList ddlCME = (DropDownList)e.Row.FindControl("ddlPerRespBhv");
            //objData.ReturnDropDown("select UserId as Id,UserLName+', '+UserFName as Name from [user] where ActiveInd='A'", ddlCME);
            //ddlCME.Items.FindByValue(CMEduVal).Selected = true;
            //HiddenField txtCME = (HiddenField)e.Row.FindControl("hdFld
            //TextBox txtCME = (TextBox)e.Row.FindControl("txtPerRespBhv");
           // (e.Row.FindControl("txtPerRespBhv") as TextBox).Attributes["on focus"] = "return SearchText();";
            //TextBox txtPerRespBhv = (e.Row.FindControl("txtPerRespBhv") as TextBox).Attributes["on focus"] = "return SearchText();";
        }
    }
    protected void GVMedicalUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            objData = new clsData();
            string CMEduVal = "0";
            Label lblCvId = (Label)e.Row.FindControl("hfPerRespMed");
            if (lblCvId.Text != "")
                CMEduVal = lblCvId.Text;
            //DropDownList ddlCME = (DropDownList)e.Row.FindControl("ddlPerRespMed");
            //objData.ReturnDropDown("select UserId as Id,UserLName+', '+UserFName as Name from [user] where ActiveInd='A'", ddlCME);
            //ddlCME.Items.FindByValue(CMEduVal).Selected = true;
        }
    }
    protected void GVPsyUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            objData = new clsData();
            string CMEduVal = "0";
            Label lblCvId = (Label)e.Row.FindControl("hfPerRespPsy");
            if (lblCvId.Text != "")
                CMEduVal = lblCvId.Text;
            //DropDownList ddlCME = (DropDownList)e.Row.FindControl("ddlPerRespPsy");
            //objData.ReturnDropDown("select UserId as Id,UserLName+', '+UserFName as Name from [user] where ActiveInd='A'", ddlCME);
            //ddlCME.Items.FindByValue(CMEduVal).Selected = true;
        }
    }
    protected void FillCvrShtHfVal(string date)
    {
        string sDate = "";
        string eDate = "";
        objData = new clsData();
        if (date != null)
        {
            sDate = date.Split('-')[0];
            eDate = date.Split('-')[1];
        }
        string query = "select StdtCvrId from StdtGrandRoundCvrsheet where StudentId=" + sess.StudentId + " and StartDate='" + sDate + "' and EndDate='" + eDate + "'";
        string cvrshtId = objData.FetchValue(query).ToString();
        hdFldCvid.Value = cvrshtId;
    }
    protected void LoadDashboard()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        string query = "select case p.PrvsRecordType when 'BehaveUp' then 'Behavioral' when 'MedicalUp' then 'Medical' when 'PsychiatryUp' then 'Psychiatry' end as [Recommendation Type],p.RecNotes as [Recommendation Notes],p.PreRecom as [Previous Recommendations],u.UserLName+', '+u.UserFName as [Person Responsible],CONVERT(varchar(10),p.CompletionDateGoal,101) as [Completion Date Goal],CONVERT(varchar(10),s.StartDate,101) + ' to ' + CONVERT(varchar(10),s.EndDate,101) as [Review Period],p.StatusUpdate as [Status Update],CONVERT(varchar(10),p.CompletedDate,101) as [Completed Date] " +
                            "from PrvsRecomendations p left join StdtGrandRoundCvrsheet s on s.StdtCvrId=p.StdtCvrId left join [User] u on u.UserId=p.PersonResp2 where p.StdtCvrId in (select StdtCvrId from StdtGrandRoundCvrsheet where StudentId=" + sess.StudentId + " and SubmittedDate is not null)";

        Dt = new DataTable();
        Dt = objData.ReturnDataTable(query, false);
        GridView1.DataSource = Dt;
        GridView1.DataBind();
    }
    protected void btnDashboardFilter_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        string ddlRecType = ddlDashboardRecType.SelectedValue;      //0 - all; 1 - Behavioral; 2 - Medical; 3 - Psychiatry
        string ddlCmpDate = ddlDashboardCmpDate.SelectedValue;     //0 - all; 1 - completed; 2 - not completed
        string calendarFrom = txtCalendarFrom.Text;
        string calendatTo = txtCalendarTo.Text;
        string subQuery = "";

        if (ddlRecType == "Behavioral")
        {
            subQuery += " and p.PrvsRecordType='BehaveUp'";
        }
        else if (ddlRecType == "Medical")
        {
            subQuery += " and p.PrvsRecordType='MedicalUp'";
        }
        else if (ddlRecType == "Psychiatry")
        {
            subQuery += " and p.PrvsRecordType='PsychiatryUp'";
        }

        if (ddlCmpDate == "Completed")
        {
            subQuery += " and p.CompletedDate is not null";
        }
        else if (ddlCmpDate == "Not Completed")
        {
            subQuery += " and p.CompletedDate is null";
        }

        if (calendarFrom != "" && calendatTo != "")
        {
            //subQuery += " and s.SubmittedDate between '" + calendarFrom + "' and '" + calendatTo + "'";
            subQuery += " and CONVERT(varchar(10),s.SubmittedDate,101) >= '" + calendarFrom + "' and CONVERT(varchar(10),s.SubmittedDate,101) <= '" + calendatTo + "'";
        }

        string query = "select case p.PrvsRecordType when 'BehaveUp' then 'Behavioral' when 'MedicalUp' then 'Medical' when 'PsychiatryUp' then 'Psychiatry' end as [Recommendation Type],p.RecNotes as [Recommendation Notes],p.PreRecom as [Previous Recommendations],u.UserLName+', '+u.UserFName as [Person Responsible],CONVERT(varchar(10),p.CompletionDateGoal,101) as [Completion Date Goal],CONVERT(varchar(10),s.StartDate,101) + ' to ' + CONVERT(varchar(10),s.EndDate,101) as [Review Period],p.StatusUpdate as [Status Update],CONVERT(varchar(10),p.CompletedDate,101) as [Completed Date] " +
                            "from PrvsRecomendations p left join StdtGrandRoundCvrsheet s on s.StdtCvrId=p.StdtCvrId left join [User] u on u.UserId=p.PersonResp2 where p.StdtCvrId in (select StdtCvrId from StdtGrandRoundCvrsheet where StudentId=" + sess.StudentId + " and SubmittedDate is not null)" + subQuery;

        Dt = new DataTable();
        Dt = objData.ReturnDataTable(query, false);
        GridView1.DataSource = Dt;
        GridView1.DataBind();
    }
}