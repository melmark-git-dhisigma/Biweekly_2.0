using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


public partial class StudentBinder_CreateIEP5 : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    string strQuery = "";
    static int IEPId = 0;
    int retVal = 0;
    DataTable Dt = null;
    static int StdtIEPId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        sess = (clsSession)Session["UserSession"];

        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }

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

        if (!IsPostBack)
        {
            clsIEP IEPObj = new clsIEP();
            //sess.IEPId = getIepIdFromStudentId();
            fillGridviews();
            ViewAccReject();
            filldays();
            string Status = IEPObj.GETIEPStatus(sess.IEPId,sess.StudentId,sess.SchoolId);
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

    private void filldays()
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        DataTable dtday = new DataTable();
        string daysother = "select [5DayInd],[6DayInd],[10DayInd],DayOther from StdtIEPExt2 where StdtIEPId=" + sess.IEPId + "";
        dtday = objData.ReturnDataTable(daysother, false);
        if (dtday.Rows.Count > 0)
        {

            if (Convert.ToString(dtday.Rows[0]["5DayInd"]) == "True")
            {
                rblSchoolDistCycle.SelectedValue = "0";
            }
            else if (Convert.ToString(dtday.Rows[0]["6DayInd"]) == "True")
            {
                rblSchoolDistCycle.SelectedValue = "1";
            }
            else if (Convert.ToString(dtday.Rows[0]["10DayInd"]) == "True")
            {
                rblSchoolDistCycle.SelectedValue = "2";
            }
            else if (Convert.ToString(dtday.Rows[0]["DayOther"]) == "True")
            {
                rblSchoolDistCycle.SelectedValue = "3";
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



    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        // RemoveRowFromGrid();
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex + 1;
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTable"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["PreviousTable"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeA.DataSource = dt;
            gvDelTypeA.DataBind();
            int rowIndex = 0;
            foreach (GridViewRow row in gvDelTypeA.Rows)
            {
                TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalA");
                TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceA");
                TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelA");
                TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                //*********************************************************************************************
                box0.Text = dt.Rows[rowIndex]["GoalId"].ToString();

                box1.Text = dt.Rows[rowIndex]["SvcTypDesc"].ToString();
                box2.Text = dt.Rows[rowIndex]["PersonalTypDesc"].ToString();
                box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();


                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
                LinkButton1.Visible = true;
            }


        }
        else if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["CurrentTable"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeA.DataSource = dt;
            gvDelTypeA.DataBind();
            int rowIndex = 0;

            //Set Previous Data on Postbacks
            SetPreviousData();
            foreach (GridViewRow row in gvDelTypeA.Rows)
            {

                TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalA");
                TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceA");
                TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelA");
                TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");


                //*******************************************************************************************
                box0.Text = dt.Rows[rowIndex]["GoalId"].ToString();

                box1.Text = dt.Rows[rowIndex]["SvcTypDesc"].ToString();
                box2.Text = dt.Rows[rowIndex]["PersonalTypDesc"].ToString();
                box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();
                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
                LinkButton1.Visible = true;
            }
        }
        fillGoalInAllDropdown(5);
    }
    protected void LinkButtonB_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex + 1;
        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTableB"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["PreviousTableB"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeB.DataSource = dt;
            gvDelTypeB.DataBind();
            int rowIndex = 0;
            foreach (GridViewRow row in gvDelTypeB.Rows)
            {
                TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");

                //*************************************************************************************
                box0.Text = dt.Rows[rowIndex]["GoalId"].ToString();

                box1.Text = dt.Rows[rowIndex]["SvcTypDesc"].ToString();
                box2.Text = dt.Rows[rowIndex]["PersonalTypDesc"].ToString();
                box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();


                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButtonB = gvDelTypeB.FooterRow.FindControl("LinkButtonB") as LinkButton;
                LinkButtonB.Visible = true;
            }
        }
        else if (ViewState["CurrentTableB"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTableB"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["CurrentTableB"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeB.DataSource = dt;
            gvDelTypeB.DataBind();
            int rowIndex = 0;

            //Set Previous Data on Postbacks
            SetPreviousDataB();
            foreach (GridViewRow row in gvDelTypeB.Rows)
            {
                TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");

                //***************************************************************************************************
                box0.Text = dt.Rows[rowIndex]["GoalId"].ToString();
                box1.Text = dt.Rows[rowIndex]["SvcTypDesc"].ToString();
                box2.Text = dt.Rows[rowIndex]["PersonalTypDesc"].ToString();
                box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();
                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButtonB = gvDelTypeB.FooterRow.FindControl("LinkButtonB") as LinkButton;
                LinkButtonB.Visible = true;
            }
        }
        fillGoalInAllDropdown(5);
    }
    protected void LinkButtonC_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex + 1;
        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTableC"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["PreviousTableC"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeC.DataSource = dt;
            gvDelTypeC.DataBind();
            int rowIndex = 0;
            foreach (GridViewRow row in gvDelTypeC.Rows)
            {
                TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");



                //***********************************************************************************************
                box0.Text = dt.Rows[rowIndex]["GoalId"].ToString();

                box1.Text = dt.Rows[rowIndex]["SvcTypDesc"].ToString();
                box2.Text = dt.Rows[rowIndex]["PersonalTypDesc"].ToString();
                box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();


                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButtonC = gvDelTypeC.FooterRow.FindControl("LinkButtonC") as LinkButton;
                LinkButtonC.Visible = true;
            }
        }
        else if (ViewState["CurrentTableC"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTableC"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["CurrentTableC"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeC.DataSource = dt;
            gvDelTypeC.DataBind();
            int rowIndex = 0;

            //Set Previous Data on Postbacks
            SetPreviousDataC();
            foreach (GridViewRow row in gvDelTypeC.Rows)
            {

                TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");


                //****************************************************************************************************
                box0.Text = dt.Rows[rowIndex]["GoalId"].ToString();

                box1.Text = dt.Rows[rowIndex]["SvcTypDesc"].ToString();
                box2.Text = dt.Rows[rowIndex]["PersonalTypDesc"].ToString();
                box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();
                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButtonC = gvDelTypeC.FooterRow.FindControl("LinkButtonC") as LinkButton;
                LinkButtonC.Visible = true;
            }
        }
        fillGoalInAllDropdown(5);
    }

    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalA");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceA");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelA");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                    //**********************************************************************************************************
                    box0.Text = dt.Rows[i]["GoalId"].ToString();
                    box1.Text = dt.Rows[i]["SvcTypDesc"].ToString();
                    box2.Text = dt.Rows[i]["PersonalTypDesc"].ToString();
                    box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    box4.Text = dt.Rows[i]["StartDate"].ToString();
                    box5.Text = dt.Rows[i]["EndDate"].ToString();

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTable"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
            LinkButton1.Visible = true;
        }
    }
    private void SetPreviousDataB()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTableB"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTableB"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");


                    //***********************************************************************************************
                    box0.Text = dt.Rows[i]["GoalId"].ToString();

                    box1.Text = dt.Rows[i]["SvcTypDesc"].ToString();
                    box2.Text = dt.Rows[i]["PersonalTypDesc"].ToString();
                    box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    box4.Text = dt.Rows[i]["StartDate"].ToString();
                    box5.Text = dt.Rows[i]["EndDate"].ToString();

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTableB"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButtonB = gvDelTypeB.FooterRow.FindControl("LinkButtonB") as LinkButton;
            LinkButtonB.Visible = true;
        }
    }
    private void SetPreviousDataC()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTableC"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTableC"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");


                    //**************************************************************************************
                    box0.Text = dt.Rows[i]["GoalId"].ToString();

                    box1.Text = dt.Rows[i]["SvcTypDesc"].ToString();
                    box2.Text = dt.Rows[i]["PersonalTypDesc"].ToString();
                    box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    box4.Text = dt.Rows[i]["StartDate"].ToString();
                    box5.Text = dt.Rows[i]["EndDate"].ToString();

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTableC"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButtonC = gvDelTypeC.FooterRow.FindControl("LinkButtonC") as LinkButton;
            LinkButtonC.Visible = true;
        }
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];

            if (dtCurrentTable.Rows.Count > 4)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry !! Limited to 5 Rows ");
                return;
            }
        }
        AddNewRowToGrid();
        fillGoalInAllDropdown(1);
    }
    protected void ButtonAddB_Click(object sender, EventArgs e)
    {

        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTableB"];

            if (dtCurrentTable.Rows.Count > 4)
            {
                tdMsgB.InnerHtml = clsGeneral.warningMsg("Sorry !! Limited to 5 Rows ");
                return;
            }
        }
        AddNewRowToGridB();
        fillGoalInAllDropdown(2);
    }
    protected void ButtonAddC_Click(object sender, EventArgs e)
    {
        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTableC"];

            if (dtCurrentTable.Rows.Count > 4)
            {
                tdMsgC.InnerHtml = clsGeneral.warningMsg("Sorry !! Limited to 5 Rows ");
                return;
            }
        }
        AddNewRowToGridC();
        fillGoalInAllDropdown(3);
    }

    private string ReturnDate()
    {
        string tempDate = "";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        string getStdGoalSvcDetails = "select convert(varchar(50), EffStartDate,101) as StartDate,convert(varchar(50),EffEndDate,101) as EndDate FROM  StdtIEP where StdtIEPId =" + sess.IEPId;
        // getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='A' ";
        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    tempDate = dr["StartDate"].ToString() + "," + dr["EndDate"].ToString();

                }
            }
        }
        return tempDate;
    }

    private void AddNewRowToGrid()
    {
        tdMsgB.InnerHtml = "";
        tdMsgC.InnerHtml = "";
        tdMsg.InnerHtml = "";
        int rowIndex = 0;
        string[] tmpDt = ReturnDate().Split(',');
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];

            //if (dtCurrentTable.Rows.Count > 5)
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 5 Rows ");
            //    return false; 
                
            //}


            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalA");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceA");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelA");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTable.Rows[i - 1]["EndDate"] = tmpDt[1].ToString();
                    dtCurrentTable.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                int NewRowcnt = dtCurrentTable.Rows.Count;
                if (NewRowcnt > 1)
                {                    
                    dtCurrentTable.Rows[NewRowcnt - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTable.Rows[NewRowcnt - 1]["EndDate"] = tmpDt[1].ToString();
                }
                ViewState["PreviousTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();

                SetPreviousDB();
            }
        }
        else if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalA");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceA");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelA");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    drCurrentRow = dtCurrentTable.NewRow();
                    //**************************************************************************************************
                    dtCurrentTable.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTable.Rows[i - 1]["EndDate"] = tmpDt[1].ToString();
                    dtCurrentTable.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                int NewRowcnt = dtCurrentTable.Rows.Count;
                if (NewRowcnt > 1)
                {
                    dtCurrentTable.Rows[NewRowcnt - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTable.Rows[NewRowcnt - 1]["EndDate"] = tmpDt[1].ToString();
                }
                ViewState["CurrentTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();

                //Set Previous Data on Postbacks
                SetPreviousData();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
    }
    private void AddNewRowToGridB()
    {
        tdMsg.InnerHtml = "";
        tdMsgC.InnerHtml = "";
        tdMsgB.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        int rowIndex = 0;
        string[] tmpDt = ReturnDate().Split(',');
        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dtCurrentTableB = (DataTable)ViewState["PreviousTableB"];
            if (dtCurrentTableB.Rows.Count > 5)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 5 Rows");
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTableB.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableB.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");
                    Label lbl_goalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //**************************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableB.Rows[i - 1]["EndDate"] = tmpDt[1].ToString();
                    dtCurrentTableB.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                int NewRowcnt = dtCurrentTableB.Rows.Count;
                if (NewRowcnt > 1)
                {
                    dtCurrentTableB.Rows[NewRowcnt - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableB.Rows[NewRowcnt - 1]["EndDate"] = tmpDt[1].ToString();
                }
                ViewState["PreviousTableB"] = dtCurrentTableB;

                gvDelTypeB.DataSource = dtCurrentTableB;
                gvDelTypeB.DataBind();

                SetPreviousDB_B();
            }
        }
        else if (ViewState["CurrentTableB"] != null)
        {
            DataTable dtCurrentTableB = (DataTable)ViewState["CurrentTableB"];
            DataRow drCurrentRow = null;
            if (dtCurrentTableB.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableB.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");
                    Label lbl_goalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //****************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableB.Rows[i - 1]["EndDate"] = tmpDt[0].ToString();
                    dtCurrentTableB.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                int NewRowcnt = dtCurrentTableB.Rows.Count;
                if (NewRowcnt > 1)
                {
                    dtCurrentTableB.Rows[NewRowcnt - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableB.Rows[NewRowcnt - 1]["EndDate"] = tmpDt[1].ToString();
                }
                ViewState["CurrentTableB"] = dtCurrentTableB;

                gvDelTypeB.DataSource = dtCurrentTableB;
                gvDelTypeB.DataBind();

                //Set Previous Data on Postbacks
                SetPreviousDataB();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
    }
    private void AddNewRowToGridC()
    {
        tdMsg.InnerHtml = "";
        tdMsgB.InnerHtml = "";
        tdMsgC.InnerHtml = "";
        int rowIndex = 0;
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        string[] tmpDt = ReturnDate().Split(',');
        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dtCurrentTableC = (DataTable)ViewState["PreviousTableC"];

            if (dtCurrentTableC.Rows.Count > 5)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 5 Rowsx");
                return;
            }


            DataRow drCurrentRow = null;
            if (dtCurrentTableC.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableC.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");
                    Label lbl_goalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //**************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableC.Rows[i - 1]["EndDate"] = tmpDt[1].ToString();
                    dtCurrentTableC.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableC.Rows.Add(drCurrentRow);
                int NewRowcnt = dtCurrentTableC.Rows.Count;
                if (NewRowcnt > 1)
                {
                    dtCurrentTableC.Rows[NewRowcnt - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableC.Rows[NewRowcnt - 1]["EndDate"] = tmpDt[1].ToString();
                }
                ViewState["PreviousTableC"] = dtCurrentTableC;

                gvDelTypeC.DataSource = dtCurrentTableC;
                gvDelTypeC.DataBind();

                SetPreviousDB_C();
            }
        }
        else if (ViewState["CurrentTableC"] != null)
        {
            DataTable dtCurrentTableC = (DataTable)ViewState["CurrentTableC"];
            DataRow drCurrentRow = null;
            if (dtCurrentTableC.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableC.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");
                    Label lbl_goalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //**************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableC.Rows[i - 1]["EndDate"] = tmpDt[1].ToString();
                    dtCurrentTableC.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }
                dtCurrentTableC.Rows.Add(drCurrentRow);
                int NewRowcnt = dtCurrentTableC.Rows.Count;
                if (NewRowcnt > 1)
                {
                    dtCurrentTableC.Rows[NewRowcnt - 1]["StartDate"] = tmpDt[0].ToString();
                    dtCurrentTableC.Rows[NewRowcnt - 1]["EndDate"] = tmpDt[1].ToString();
                }
                ViewState["CurrentTableC"] = dtCurrentTableC;

                gvDelTypeC.DataSource = dtCurrentTableC;
                gvDelTypeC.DataBind();


                //Set Previous Data on Postbacks
                SetPreviousDataC();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
    }

    private void SetPreviousDB_B()
    {
        int rowIndex = 0;
        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTableB"];
            if (dt.Rows.Count > 0)
            {
                // Response.Write(gvDelTypeB.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["GoalId"].ToString();

                    box1.Text = dt.Rows[i]["SvcTypDesc"].ToString();
                    box2.Text = dt.Rows[i]["PersonalTypDesc"].ToString();
                    box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    box4.Text = dt.Rows[i]["StartDate"].ToString();
                    box5.Text = dt.Rows[i]["EndDate"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTableB"];

    }
    private void SetPreviousDB_C()
    {
        int rowIndex = 0;
        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTableC"];
            if (dt.Rows.Count > 0)
            {
                // Response.Write(gvDelTypeC.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");

                    //***************************************************************************************
                    box0.Text = dt.Rows[i]["GoalId"].ToString();
                    // GoalId
                    box1.Text = dt.Rows[i]["SvcTypDesc"].ToString();
                    box2.Text = dt.Rows[i]["PersonalTypDesc"].ToString();
                    box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    box4.Text = dt.Rows[i]["StartDate"].ToString();
                    box5.Text = dt.Rows[i]["EndDate"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTableC"];

    }
    private void SetPreviousDB()
    {
        int rowIndex = 0;
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTable"];
            if (dt.Rows.Count > 0)
            {
                // Response.Write(gvDelTypeA.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalA");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceA");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelA");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");


                    //*************************************************************************************************
                    box0.Text = dt.Rows[i]["GoalId"].ToString();

                    box1.Text = dt.Rows[i]["SvcTypDesc"].ToString();
                    box2.Text = dt.Rows[i]["PersonalTypDesc"].ToString();
                    box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    box4.Text = dt.Rows[i]["StartDate"].ToString();
                    box5.Text = dt.Rows[i]["EndDate"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTable"];

    }

    private void setInitialGrid1()
    {
        Int32 i = 0;
        int j = 0;
        int flag = 0;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        string getStdtGoalId = "select StdtGoalId from StdtGoal where StdtIEPId=" + sess.IEPId;
        DataTable dt_stdGoal = objData.ReturnDataTable(getStdtGoalId, false);

        DataTable dt = new DataTable();
        Int32 n = 0;
        if (dt_stdGoal != null)
        {
            n = dt_stdGoal.Rows.Count;
        }

        //***************************************************************************************************************

        dt.Columns.Add("GoalId", typeof(string));
        //dt.Columns.Add("GoalId",i.ToString());
        dt.Columns.Add("StdtGoalSvcId", typeof(string));
        dt.Columns.Add("SvcTypDesc", typeof(string));
        dt.Columns.Add("PersonalTypDesc", typeof(string));
        dt.Columns.Add("FreqDurDesc", typeof(string));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));
        //=============================================

        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT SGS.StdtGoalId as GoalId,SGS.StdtGoalSvcId, SGS.SvcDelTyp, SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId ";
        getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='A' ";

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);

        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    if ((dr["GoalId"].ToString() == null) && (dr["StdtGoalSvcId"].ToString() == null))
                    {
                        flag++;
                    }
                    //======================================================
                    else
                    {
                        dt.Rows.Add(dr["GoalId"].ToString(), dr["StdtGoalSvcId"].ToString(), dr["SvcTypDesc"].ToString(), dr["PersonalTypDesc"].ToString(), dr["FreqDurDesc"].ToString(), dr["StartDate"].ToString(), dr["EndDate"].ToString());
                        flag = 0;
                    }
                }
            }
            else
            {
                // dt.Rows.Add("", "0", "", "", "", "", "");
                getStdGoalSvcDetails = "select convert(varchar(50), EffStartDate,101) as StartDate,convert(varchar(50),EffEndDate,101) as EndDate FROM  StdtIEP where StdtIEPId =" + sess.IEPId;
                // getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='A' ";
                dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
                if (dt_goalDetails != null)
                {
                    if (dt_goalDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_goalDetails.Rows)
                        {
                            dt.Rows.Add("", "0", "", "", "", dr["StartDate"].ToString(), dr["EndDate"].ToString());
                        }
                    }
                }

            }
        }
        else
        {
            dt.Rows.Add("", "0", "", "", "", "", "");

        }
        if (flag > 0)
        {
            getStdGoalSvcDetails = "select convert(varchar(50), EffStartDate,101) as StartDate,convert(varchar(50),EffEndDate,101) as EndDate FROM  StdtIEP where StdtIEPId =" + sess.IEPId;
            // getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='A' ";
            dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
            if (dt_goalDetails != null)
            {
                if (dt_goalDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt_goalDetails.Rows)
                    {
                        dt.Rows.Add("", "0", "", "", "", dr["StartDate"].ToString(), dr["EndDate"].ToString());
                    }
                }
            }
        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");
        //}


        ViewState["PreviousTable"] = dt;

        gvDelTypeA.DataSource = dt;
        gvDelTypeA.DataBind();

    }
    private void setInitialGrid2()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        string getStdtGoalId = "select StdtGoalId from StdtGoal where StdtIEPId=" + sess.IEPId;
        DataTable dt_stdGoal = objData.ReturnDataTable(getStdtGoalId, false);

        DataTable dt = new DataTable();
        //***************************************************************************************************************

        //GoalId
        dt.Columns.Add("GoalId", typeof(string));

        dt.Columns.Add("StdtGoalSvcId", typeof(string));
        dt.Columns.Add("SvcTypDesc", typeof(string));
        dt.Columns.Add("PersonalTypDesc", typeof(string));
        dt.Columns.Add("FreqDurDesc", typeof(string));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT     SGS.StdtGoalId as GoalId,SGS.StdtGoalSvcId, SGS.SvcDelTyp, SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId ";
        getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='B' ";

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//============================================
                    dt.Rows.Add(dr["GoalId"].ToString(), dr["StdtGoalSvcId"].ToString(), dr["SvcTypDesc"].ToString(), dr["PersonalTypDesc"].ToString(), dr["FreqDurDesc"].ToString(), dr["StartDate"].ToString(), dr["EndDate"].ToString());
                }
            }
            else
            {
                //dt.Rows.Add("", "0", "", "", "", "", "");
                getStdGoalSvcDetails = "select convert(varchar(50), EffStartDate,101) as StartDate,convert(varchar(50),EffEndDate,101) as EndDate FROM  StdtIEP where StdtIEPId =" + sess.IEPId;
                // getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='A' ";
                dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
                if (dt_goalDetails != null)
                {
                    if (dt_goalDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_goalDetails.Rows)
                        {
                            dt.Rows.Add("", "0", "", "", "", dr["StartDate"].ToString(), dr["EndDate"].ToString());
                        }
                    }
                }

            }
        }
        else
        {
            dt.Rows.Add("", "0", "", "", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");

        //}

        ViewState["PreviousTableB"] = dt;

        gvDelTypeB.DataSource = dt;
        gvDelTypeB.DataBind();
    }
    private void setInitialGrid3()
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string getStdtGoalId = "select StdtGoalId from StdtGoal where StdtIEPId=" + sess.IEPId;
        DataTable dt_stdGoal = objData.ReturnDataTable(getStdtGoalId, false);

        DataTable dt = new DataTable();
        //***************************************************************************************************************
        dt.Columns.Add("GoalId", typeof(string));
        dt.Columns.Add("StdtGoalSvcId", typeof(string));
        dt.Columns.Add("SvcTypDesc", typeof(string));
        dt.Columns.Add("PersonalTypDesc", typeof(string));
        dt.Columns.Add("FreqDurDesc", typeof(string));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT     SGS.StdtGoalId as GoalId,SGS.StdtGoalSvcId, SGS.SvcDelTyp, SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId ";
        getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='C' ";

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//===============================
                    dt.Rows.Add(dr["GoalId"].ToString(), dr["StdtGoalSvcId"].ToString(), dr["SvcTypDesc"].ToString(), dr["PersonalTypDesc"].ToString(), dr["FreqDurDesc"].ToString(), dr["StartDate"].ToString(), dr["EndDate"].ToString());
                }
            }
            else
            {
                //dt.Rows.Add("", "0", "", "", "", "", "");
                getStdGoalSvcDetails = "select convert(varchar(50), EffStartDate,101) as StartDate,convert(varchar(50),EffEndDate,101) as EndDate FROM  StdtIEP where StdtIEPId =" + sess.IEPId;
                // getStdGoalSvcDetails += " WHERE     IEP.StdtIEPId = " + sess.IEPId + " AND IEP.SchoolId = " + sess.SchoolId + " AND IEP.StudentId = " + sess.StudentId + " and SGS.SvcDelTyp='A' ";
                dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
                if (dt_goalDetails != null)
                {
                    if (dt_goalDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_goalDetails.Rows)
                        {
                            dt.Rows.Add("", "0", "", "", "", dr["StartDate"].ToString(), dr["EndDate"].ToString());
                        }
                    }
                }

            }
        }
        else
        {
            dt.Rows.Add("", "0", "", "", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");

        //}

        ViewState["PreviousTableC"] = dt;

        gvDelTypeC.DataSource = dt;
        gvDelTypeC.DataBind();
    }

    private void fillGoalInAllDropdown(int opt)
    {
        //opt = 2;
        try
        {
            if (opt == 1 || opt == 5)
            {
                foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
                {
                    objData = new clsData();
                    TextBox ddlGoalA = diTypeA.FindControl("txtFocusOnGoalA") as TextBox;

                    //fillGoal(ddlGoalA);
                    Label lbl = (Label)diTypeA.Cells[1].FindControl("lbl_goalId");

                    if (lbl.Text != "")
                        ddlGoalA.Text = lbl.Text;
                }
            }
            if (opt == 2 || opt == 5)
            {
                foreach (GridViewRow diTypeB in gvDelTypeB.Rows)
                {
                    objData = new clsData();
                    TextBox ddlGoalB = diTypeB.FindControl("txtFocusOnGoalB") as TextBox;
                    //fillGoal(ddlGoalB);

                    Label lbl = (Label)diTypeB.Cells[1].FindControl("lbl_goalId1");

                    if (lbl.Text != "")
                        ddlGoalB.Text = lbl.Text;

                }
            }
            if (opt == 3 || opt == 5)
            {
                foreach (GridViewRow diTypeC in gvDelTypeC.Rows)
                {
                    objData = new clsData();
                    TextBox ddlGoalC = diTypeC.FindControl("txtFocusOnGoalC") as TextBox;
                    //fillGoal(ddlGoalC);

                    Label lbl = (Label)diTypeC.Cells[1].FindControl("lbl_goalId");

                    if (lbl.Text != "")
                        ddlGoalC.Text = lbl.Text;

                }
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    //protected void fillGoal(TextBox ddl)
    //{
    //    objData = new clsData();
    //    strQuery = "select DISTINCT StdtGoal.StdtGoalId as Id,Goal.GoalName as Name from Goal inner Join StdtGoal on Goal.GoalId=StdtGoal.GoalId where Goal.GoalLevelId=1 and StdtGoal.IncludeIEP=1 and StdtGoal.StdtIEPId=" + sess.IEPId + " AND Goal.ActiveInd='A'";
    //    objData.ReturnDropDown(strQuery, ddl);
    //}


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                sess = (clsSession)Session["UserSession"];
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

                    bool result1 = SaveIEPPage5();
                    if (result1 == true)
                    {
                        result1 = SaveIEPPage5B();
                    }
                    if (result1 == true)
                    {
                        result1 = SaveIEPPage5C();
                    }
                    if (result1 == true)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
                        }
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(6);", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                sess = (clsSession)Session["UserSession"];
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

                    bool result1 = SaveIEPPage5();
                    if (result1 == true)
                    {
                        result1 = SaveIEPPage5B();
                    }
                    if (result1 == true)
                    {
                        result1 = SaveIEPPage5C();
                    }
                    if (result1 == true)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
                        }
                        // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP6();", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    public static bool Isdate(string date)
    {

        bool IsTrue = false;
        if (date != "")
        {
            string ValidationExpression = @"\d{1,2}\/\d{1,2}/\d{4}";
            if (System.Text.RegularExpressions.Regex.IsMatch(date, ValidationExpression))
            {
                IsTrue = true;
            }
        }
        return IsTrue;

    }
    protected bool SaveIEPPage5()
    {
        bool getA = true;
        objData = new clsData();

        foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
        {
            string dateStart = ((TextBox)diTypeA.FindControl("txtStartDateA")).Text;

            string dateEnd = ((TextBox)diTypeA.FindControl("txtEndDateA")).Text;
            TextBox ddlGoal = diTypeA.FindControl("txtFocusOnGoalA") as TextBox;
            //if (ddlGoal.SelectedIndex == 0)
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select the Goal");
            //    return false;
            //}
            //if (dateStart == "" && dateEnd == "")
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter the Date");
            //    return false;
            //}

            if (dateStart != "" && dateEnd != "")
            {

                if (Isdate(dateStart) == false && Isdate(dateEnd) == false)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("All date fields are must be in 'mm/dd/yyyy' format.");
                    return false;
                }
            }

        }
        try
        {
            sess = (clsSession)Session["UserSession"];
            bool status5day, status6day, status10day, statusother;
            if (rblSchoolDistCycle.SelectedIndex >= 0 && rblSchoolDistCycle.SelectedItem.Text != "")
            {
                string cycle = rblSchoolDistCycle.SelectedItem.Text;
                if (cycle == "5 day cycle")
                {
                    status5day = true;
                    status6day = false;
                    status10day = false;
                    statusother = false;
                }
                else if (cycle == "6 day cycle")
                {
                    status5day = false;
                    status6day = true;
                    status10day = false;
                    statusother = false;
                }
                else if (cycle == "10 day cycle")
                {
                    status5day = false;
                    status6day = false;
                    status10day = true;
                    statusother = false;
                }
                else
                {
                    status5day = false;
                    status6day = false;
                    status10day = false;
                    statusother = true;
                }
                string daycycle = "UPDATE StdtIEPExt2 set [5DayInd]='" + status5day + "',[6DayInd]='" + status6day + "',[10DayInd]='" + status10day + "',DayOther='" + statusother + "' where StdtIEPId=" + sess.IEPId + "";
                Boolean daycycleupdate = Convert.ToBoolean(objData.Execute(daycycle));
            }

            string save = "UPDATE StdtIEPExt2 set OtherDesc1='" + clsGeneral.convertQuotes(txtOther5.Text) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + sess.IEPId + "";
            Boolean index = Convert.ToBoolean(objData.Execute(save));

            foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
            {
                objData = new clsData();

                TextBox txtGoal = diTypeA.FindControl("txtFocusOnGoalA") as TextBox;
                TextBox txtTypeOfServiceA = diTypeA.FindControl("txtTypeOfServiceA") as TextBox;
                TextBox txtTypeOfPersonnelA = diTypeA.FindControl("txtTypeOfPersonnelA") as TextBox;
                TextBox txtFrequencyA = diTypeA.FindControl("txtFrequencyA") as TextBox;
                TextBox txtStartDateA = diTypeA.FindControl("txtStartDateA") as TextBox;
                TextBox txtEndDateA = diTypeA.FindControl("txtEndDateA") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int StdtGoalSvcId = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string a = txtTypeOfServiceA.Text;
                string b = txtTypeOfPersonnelA.Text;
                string c = txtFrequencyA.Text;
                string d = txtStartDateA.Text;
                string ea = txtEndDateA.Text;
                string ws = txtGoal.Text;

                string insDelivery = "";
                if (StdtGoalSvcId == 0)
                {
                    insDelivery = "INSERT INTO StdtGoalSvc (StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,CreatedBy,CreatedOn,StdtIEPId) " +
                                          "VALUES ('" + txtGoal.Text.Trim() + "', 'A', '" + a + "', '" + b + "', '" + c + "', " +
                                          "'" + d + "', '" + ea + "', '" + sess.LoginId + "' ,GETDATE()," + sess.IEPId + " )";


                    int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);

                }

                else
                {
                    insDelivery = "Update StdtGoalSvc set StdtGoalId='" + clsGeneral.convertQuotes(ws) + "', SvcTypDesc='" + clsGeneral.convertQuotes(txtTypeOfServiceA.Text) + "',PersonalTypDesc='" + clsGeneral.convertQuotes(txtTypeOfPersonnelA.Text) + "',FreqDurDesc='" + clsGeneral.convertQuotes(txtFrequencyA.Text) + "',StartDate='" + clsGeneral.convertQuotes(txtStartDateA.Text) + "',EndDate='" + clsGeneral.convertQuotes(txtEndDateA.Text) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtGoalSvcId=" + StdtGoalSvcId;

                    int i = objData.Execute(insDelivery);
                }






            }
        }
        catch (SqlException Ex)
        {
            getA = false;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed!");
            return false;
            throw Ex;
        }
        return getA;
    }
    protected bool SaveIEPPage5B()
    {
        bool getB = true;
        try
        {
            foreach (GridViewRow diTypeB in gvDelTypeB.Rows)
            {
                string dateStart = ((TextBox)diTypeB.FindControl("txtStartDateB")).Text;
                string dateEnd = ((TextBox)diTypeB.FindControl("txtEndDateB")).Text;
                TextBox ddlGoal = diTypeB.FindControl("txtFocusOnGoalB") as TextBox;

                if (dateStart != "" && dateEnd != "")
                {
                    if (Isdate(dateStart) == false && Isdate(dateEnd) == false)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("All date fields are must be in 'mm/dd/yyyy' format.");
                        return false;
                    }
                }


                TextBox txtGoalB = (TextBox)diTypeB.FindControl("txtFocusOnGoalB") as TextBox;
                TextBox txtTypeOfServiceB = diTypeB.FindControl("txtTypeOfServiceB") as TextBox;
                TextBox txtTypeOfPersonnelB = diTypeB.FindControl("txtTypeOfPersonnelB") as TextBox;
                TextBox txtFrequencyB = diTypeB.FindControl("txtFrequencyB") as TextBox;
                TextBox txtStartDateB = diTypeB.FindControl("txtStartDateB") as TextBox;
                TextBox txtEndDateB = diTypeB.FindControl("txtEndDateB") as TextBox;

                //HiddenField txtStartDateB = diTypeB.FindControl("txtstartDateB") as HiddenField;
                //HiddenField txtEndDateB = diTypeB.FindControl("txtendDateB") as HiddenField;


                Label lbl_StdtGoalSvcId = diTypeB.FindControl("lbl_svcid") as Label;
                int StdtGoalSvcId = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);

                //string goalb = ddlGoalB.SelectedItem.Text; 


                string a = txtTypeOfServiceB.Text;
                string b = txtTypeOfPersonnelB.Text;
                string c = txtFrequencyB.Text;
                string d = txtStartDateB.Text;
                string ea = txtEndDateB.Text;
                string goal = txtGoalB.Text.Trim();



                int intStdtGoalSvcId = 0;
                objData = new clsData();
                sess = (clsSession)Session["UserSession"];
                string insDelivery = "";
                if (StdtGoalSvcId == 0)
                {

                    insDelivery = "INSERT INTO StdtGoalSvc (StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,CreatedBy,CreatedOn,StdtIEPId) " +
                                          "VALUES ('" + goal + "', 'B', '" + a + "', '" + b + "', '" + c + "', " +
                                          "'" + d + "', '" + ea + "', '" + sess.LoginId + "' ,GETDATE()," + sess.IEPId + " )";


                    intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);

                }
                else
                {
                    insDelivery = "Update StdtGoalSvc set StdtGoalId='" + clsGeneral.convertQuotes(goal) + "', SvcTypDesc='" + clsGeneral.convertQuotes(txtTypeOfServiceB.Text) + "',PersonalTypDesc='" + clsGeneral.convertQuotes(txtTypeOfPersonnelB.Text) + "',FreqDurDesc='" + clsGeneral.convertQuotes(txtFrequencyB.Text) + "',StartDate='" + clsGeneral.convertQuotes(txtStartDateB.Text) + "',EndDate='" + clsGeneral.convertQuotes(txtEndDateB.Text) + "'  where StdtGoalSvcId=" + StdtGoalSvcId;

                    int i = objData.Execute(insDelivery);
                }



            }
        }
        catch (SqlException Ex)
        {
            getB = false;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed!");
            return false;
            throw Ex;
        }
        return getB;
    }
    protected bool SaveIEPPage5C()
    {
        bool getC = true;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        foreach (GridViewRow diTypeC in gvDelTypeC.Rows)
        {
            //string dateStart = ((HiddenField)diTypeC.FindControl("txtstartDateC")).Value;
            //string dateEnd = ((HiddenField)diTypeC.FindControl("txtendDateC")).Value;

            string dateStart = ((TextBox)diTypeC.FindControl("txtStartDateC")).Text;
            string dateEnd = ((TextBox)diTypeC.FindControl("txtEndDateC")).Text;
            TextBox ddlGoal = diTypeC.FindControl("txtFocusOnGoalC") as TextBox;
            //if (ddlGoal.SelectedIndex == 0)
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select the Goal");
            //    return false;
            //}
            //if (dateStart == "" && dateEnd == "")
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter the Date");
            //    return false;
            //}

            if (dateStart != "" && dateEnd != "")
            {
                if (Isdate(dateStart) == false && Isdate(dateEnd) == false)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("All date fields are must be in 'mm/dd/yyyy' format.");
                    return false;
                }
            }

        }
        try
        {
            foreach (GridViewRow diTypeC in gvDelTypeC.Rows)
            {
                objData = new clsData();

                TextBox txtGoalC = diTypeC.FindControl("txtFocusOnGoalC") as TextBox;
                TextBox txtTypeOfServiceC = diTypeC.FindControl("txtTypeOfServiceC") as TextBox;
                TextBox txtTypeOfPersonnelC = diTypeC.FindControl("txtTypeOfPersonnelC") as TextBox;
                TextBox txtFrequencyC = diTypeC.FindControl("txtFrequencyC") as TextBox;
                TextBox txtStartDateC = diTypeC.FindControl("txtStartDateC") as TextBox;
                TextBox txtEndDateC = diTypeC.FindControl("txtEndDateC") as TextBox;

                //HiddenField txtStartDateC = diTypeC.FindControl("txtstartDateC") as HiddenField;
                //HiddenField txtEndDateC = diTypeC.FindControl("txtendDateC") as HiddenField;

                Label lbl_StdtGoalSvcId = diTypeC.FindControl("lbl_svcid") as Label;
                int StdtGoalSvcId = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);


                string a = txtTypeOfServiceC.Text;
                string b = txtTypeOfPersonnelC.Text;
                string c = txtFrequencyC.Text;
                string d = txtStartDateC.Text;
                string ea = txtEndDateC.Text;
                string df = txtGoalC.Text;

                int intStdtGoalSvcId = 0;


                string insDelivery = "";
                if (StdtGoalSvcId == 0)
                {

                    insDelivery = "INSERT INTO StdtGoalSvc (StdtGoalId,SvcDelTyp,SvcTypDesc,PersonalTypDesc,FreqDurDesc,StartDate,EndDate,CreatedBy,CreatedOn,StdtIEPId) " +
                                        "VALUES ('" + txtGoalC.Text.Trim() + "', 'C', '" + a + "', '" + b + "', '" + c + "', " +
                                        "'" + d + "', '" + ea + "', '" + sess.LoginId + "' ,GETDATE() ," + sess.IEPId + ")";
                    intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);

                }
                else
                {
                    insDelivery = "Update StdtGoalSvc set  StdtGoalId='" + clsGeneral.convertQuotes(df) + "',SvcTypDesc='" + clsGeneral.convertQuotes(txtTypeOfServiceC.Text) + "',PersonalTypDesc='" + clsGeneral.convertQuotes(txtTypeOfPersonnelC.Text) + "',FreqDurDesc='" + clsGeneral.convertQuotes(txtFrequencyC.Text) + "',StartDate='" + clsGeneral.convertQuotes(txtStartDateC.Text) + "',EndDate='" + clsGeneral.convertQuotes(txtEndDateC.Text) + "'  where StdtGoalSvcId=" + StdtGoalSvcId;

                    int i = objData.Execute(insDelivery);
                }



            }
        }
        catch (SqlException Ex)
        {
            getC = false;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed!");
            return false;
            throw Ex;
        }
        return getC;
    }




    private void fillGridviews()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string getStdIEPDetails = "select IEP2.StdtIEPId,IEP2.OtherDesc1 from StdtIEPExt2 IEP2 INNER JOIN StdtIEP IEP ON IEP.StdTIEPId=IEP2.StdtIEPId WHERE IEP.StdtIEPId=" + sess.IEPId + " and IEP.AsmntYearId=" + sess.YearId;
        DataTable stdIEPDetails = objData.ReturnDataTable(getStdIEPDetails, false);

        if (stdIEPDetails.Rows.Count > 0)
        {
            StdtIEPId = Convert.ToInt32(stdIEPDetails.Rows[0]["StdtIEPid"].ToString());
            txtOther5.Text = stdIEPDetails.Rows[0]["OtherDesc1"].ToString();


        }
        else
        {
            txtOther5.Text = "";
        }

        setInitialGrid1();
        setInitialGrid2();
        setInitialGrid3();
        fillGoalInAllDropdown(5);
    }


    protected void gvDelTypeA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }
        }
    }
    protected void gvDelTypeB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId1");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }


        }
    }
    protected void gvDelTypeC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");
            int svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
        }
    }


    protected void gvDelTypeA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        tdMsgB.InnerHtml = "";
        tdMsgC.InnerHtml = "";
        int index = 0;
        if (e.CommandArgument.ToString() != "")
        {
            index = int.Parse(e.CommandArgument.ToString());
        }
        else
        {
            return;
        }
        GridViewRow row = gvDelTypeA.Rows[index];

        if (e.CommandName == "remove")
        {
            if (gvDelTypeA.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from StdtGoalSvc where StdtGoalSvcId=" + StdtGoalSvcId;

                    int i = objData.Execute(delRow);
                    deleteRowA(index);
                }
                else
                {
                    deleteRowA(index);
                }


            }
        }


    }
    protected void gvDelTypeB_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        tdMsg.InnerHtml = "";
        tdMsgC.InnerHtml = "";

        int index = 0;
        if (e.CommandArgument.ToString() != "")
        {
            index = int.Parse(e.CommandArgument.ToString());
        }
        else
        {
            return;
        }
        GridViewRow row = gvDelTypeB.Rows[index];

        if (e.CommandName == "remove")
        {
            if (gvDelTypeB.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from StdtGoalSvc where StdtGoalSvcId=" + StdtGoalSvcId;

                    int i = objData.Execute(delRow);
                    deleteRowB(index);
                }
                else
                {
                    deleteRowB(index);
                }
            }

        }

    }
    protected void gvDelTypeC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        tdMsg.InnerHtml = "";
        tdMsgB.InnerHtml = "";

        int index = 0;
        if (e.CommandArgument.ToString() != "")
        {
            index = int.Parse(e.CommandArgument.ToString());
        }
        else
        {
            return;
        }
        GridViewRow row = gvDelTypeC.Rows[index];

        if (e.CommandName == "remove")
        {
            if (gvDelTypeC.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from StdtGoalSvc where StdtGoalSvcId=" + StdtGoalSvcId;

                    int i = objData.Execute(delRow);
                    deleteRowC(index);
                }
                else
                {
                    deleteRowC(index);
                }
            }

        }
    }


    private void deleteRowA(int rowID)
    {
        tdMsg.InnerHtml = "";
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[1].FindControl("txtFocusOnGoalA");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[2].FindControl("txtTypeOfServiceA");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[3].FindControl("txtTypeOfPersonnelA");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[4].FindControl("txtFrequencyA");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtStartDateA");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[6].FindControl("txtEndDateA");
                    Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[1].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    //HiddenField box4 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtstartDate");
                    //HiddenField box5 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtendDate");

                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["StartDate"] = box4.Text;
                    dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                // dtCurrentTable.Rows.Add(drCurrentRow);

                dtCurrentTable.Rows.Remove(dtCurrentTable.Rows[rowID]);

                ViewState["PreviousTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();

                fillGoalInAllDropdown(1);

                SetPreviousDB();
            }
        }
    }
    private void deleteRowB(int rowID)
    {
        tdMsgB.InnerHtml = "";
        int rowIndex = 0;

        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dtCurrentTableB = (DataTable)ViewState["PreviousTableB"];
            DataRow drCurrentRow = null;
            if (dtCurrentTableB.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableB.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalB");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceB");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelB");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequencyB");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtStartDateB");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtEndDateB");
                    Label lbl_goalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    //HiddenField box4 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtstartDateB");
                    //HiddenField box5 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtendDateB");



                    drCurrentRow = dtCurrentTableB.NewRow();
                    dtCurrentTableB.Rows[i - 1]["GoalId"] = box0.Text;
                    dtCurrentTableB.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["StartDate"] = box4.Text;
                    dtCurrentTableB.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTableB.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }

                ViewState["PreviousTableB"] = dtCurrentTableB;

                dtCurrentTableB.Rows.Remove(dtCurrentTableB.Rows[rowID]);



                gvDelTypeB.DataSource = dtCurrentTableB;
                gvDelTypeB.DataBind();

                fillGoalInAllDropdown(2);

                SetPreviousDB_B();
            }
        }
    }
    private void deleteRowC(int rowID)
    {
        tdMsgC.InnerHtml = "";
        int rowIndex = 0;

        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dtCurrentTableC = (DataTable)ViewState["PreviousTableC"];
            DataRow drCurrentRow = null;
            if (dtCurrentTableC.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableC.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFocusOnGoalC");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfServiceC");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtTypeOfPersonnelC");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequencyC");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtStartDateC");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtEndDateC");
                    Label lbl_goalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    //HiddenField box4 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtstartDateC");
                    //HiddenField box5 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtendDateC");



                    drCurrentRow = dtCurrentTableC.NewRow();
                    dtCurrentTableC.Rows[i - 1]["GoalId"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["SvcTypDesc"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["PersonalTypDesc"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["StartDate"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTableC.Rows[i - 1]["StdtGoalSvcId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }

                ViewState["PreviousTableC"] = dtCurrentTableC;

                dtCurrentTableC.Rows.Remove(dtCurrentTableC.Rows[rowID]);

                gvDelTypeC.DataSource = dtCurrentTableC;
                gvDelTypeC.DataBind();

                fillGoalInAllDropdown(3);

                SetPreviousDB_C();
            }
        }
    }


    protected void gvDelTypeA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

