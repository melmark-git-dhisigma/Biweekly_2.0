using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;

public partial class StudentBinder_CreateIEP_PE10 : System.Web.UI.Page
{
    DataTable DtMeshGoal = new DataTable();
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    public clsSession sess = null;
    static string x = "", y = "";
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
            setInitialGrid();
            setInitialGrid2();
            // }
            //}
            //else
            //{
            //    active_pages = "1,4,6";
            //}

            //ViewAccReject();
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

    [WebMethod]
    public static void submitIepPE1(string arg1)
    {
        x = arg1;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        saveTodb(x);
        bool result1 = SaveIEPPage11B();
        fillBasicDetails();
        gvDelTypeA.DataBind();
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        saveTodb1(x);
        bool result1 = SaveIEPPage11B();
        fillBasicDetails();
    }
    private void fillBasicDetails()
    {
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
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

            //strQuery = "SELECT IEP10Benchmarks1 FROM IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            //Dt = objData.ReturnDataTable(strQuery, false);
            //if (Dt != null)
            //{
            //    if (Dt.Rows.Count > 0)
            //    {

            //        TextBoxBenchmarks.InnerHtml = Dt.Rows[0]["IEP10Benchmarks1"].ToString().Trim();
            //        //TextBoxImplementationDate.Text = Dt.Rows[0]["IepImplementationDate"].ToString().Trim();
            //        //TextBoxResidenceCountry.Text = Dt.Rows[0]["CountyOfResidance"].ToString().Trim();
            //        //TextBoxGraduationYear.Text = Dt.Rows[0]["AnticipatedYearOfGraduation"].ToString().Trim();
            //        //TextBoxLEA.Text = Dt.Rows[0]["LocalEducationAgency"].ToString().Trim();
            //        //TextBoxSeviceDuration.Text = Dt.Rows[0]["AnticipatedDurationofServices"].ToString().Trim();
            //        //TextBoxDocumentedBy.InnerHtml = Dt.Rows[0]["DocumentedBy"].ToString().Trim();
            //        //TextBoxMeetingDate.Text = Dt.Rows[0]["IepTeamMeetingDate"].ToString().Trim();

            //    }
            //}
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    public string saveTodb(string x)
    {
        clsData objData = new clsData();
        DataClass oData = new DataClass();
        string result = "";
        string strQuery = "";
        sess = (clsSession)Session["UserSession"];
        try
        {
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            if (sess.IEPId <= 0) return result;
            if (sess.IEPId == null)
            {
                result = "IEP not Properly Selected";
                return result;
            }
            pendstatus = Convert.ToString(objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " "));
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                result = "IEP is in Pending State.";
                return result;

            }
            else
            {
                string StatusName = Convert.ToString(objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus));

                if (StatusName == "Approved" || StatusName == "Rejected")
                {
                    result = "Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!";
                    return result;
                }
                else
                {
                    if (sess != null)
                    {
                        if (sess.StudentId == 0)
                        {
                            result = "Please select Student..";
                            return result;
                        }

                        else
                        {
                            //strQuery = "update IEP_PE_Details set IEP10Benchmarks1='" + x + "' where StdtIEP_PEId=" + sess.IEPId;
                            //int id = oData.ExecuteNonQuery(strQuery);
                            //if (id > 0)
                            //{
                            SaveIEPPage();

                            //}

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        return result;
    }
    public string saveTodb1(string x)
    {
        clsData objData = new clsData();
        DataClass oData = new DataClass();
        string result = "";
        string strQuery = "";
        sess = (clsSession)Session["UserSession"];
        try
        {
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            if (sess.IEPId <= 0) return result;
            if (sess.IEPId == null)
            {
                result = "IEP not Properly Selected";
                return result;
            }
            pendstatus = Convert.ToString(objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " "));
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                result = "IEP is in Pending State.";
                return result;

            }
            else
            {
                string StatusName = Convert.ToString(objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus));

                if (StatusName == "Approved" || StatusName == "Rejected")
                {
                    result = "Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!";
                    return result;
                }
                else
                {
                    if (sess != null)
                    {
                        if (sess.StudentId == 0)
                        {
                            result = "Please select Student..";
                            return result;
                        }

                        else
                        {
                            //strQuery = "update IEP_PE_Details set IEP10Benchmarks1='" + x + "' where StdtIEP_PEId=" + sess.IEPId;
                            //int id = oData.ExecuteNonQuery(strQuery);
                            //if (id > 0)
                            //{
                            SaveIEPPage1();

                            //}

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        return result;
    }

    protected bool SaveIEPPage()
    {
        bool getA = true;
        objData = new clsData();


        try
        {
            sess = (clsSession)Session["UserSession"];
            foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
            {
                objData = new clsData();

                DropDownList ddlGoals = diTypeA.FindControl("ddlGoals") as DropDownList;
                TextBox txtRevisionDate = diTypeA.FindControl("TxtMeasureAnualGoal") as TextBox;
                TextBox txtRoles = diTypeA.FindControl("TxtStudentsProgress") as TextBox;
                TextBox txtSection = diTypeA.FindControl("TxtDescReportProgress") as TextBox;
                TextBox txtFrequencyA = diTypeA.FindControl("TxtReportProgress") as TextBox;
                //TextBox txtStartDateA = diTypeA.FindControl("txtStartDateA") as TextBox;
                //TextBox txtEndDateA = diTypeA.FindControl("txtEndDateA") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);

                int tempGoalId = 0;
                try
                {
                    tempGoalId = Convert.ToInt32(ddlGoals.SelectedValue);
                }
                catch
                {
                    tempGoalId = 0;
                }


                string insDelivery = "";
                if (Id == 0)
                {

                    insDelivery = "insert into StdtIEP_PE10_GoalsObj ([StdtIEP_PEId],[GoalID],[MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress],[CreatedBy],[CreatedOn]) " +
                                          "VALUES ('" + sess.IEPId + "'," + tempGoalId + ", '" + txtRevisionDate.Text + "', '" + txtRoles.Text + "', '" + txtSection.Text + "', '" + txtFrequencyA.Text + "', " +
                                          "'" + sess.LoginId + "' ,GETDATE())";


                    int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);


                }

                else
                {
                    insDelivery = "Update StdtIEP_PE10_GoalsObj set GoalId=" + tempGoalId + ", MeasureAnualGoal='" + clsGeneral.convertQuotes(txtRevisionDate.Text) + "',"
                    + "StudentsProgress='" + clsGeneral.convertQuotes(txtRoles.Text) + "',ModifiedBy=" + sess.LoginId + ","
                    + "DescReportProgress='" + clsGeneral.convertQuotes(txtSection.Text) + "',ReportProgress='" + clsGeneral.convertQuotes(txtFrequencyA.Text) + "',ModifiedOn=getdate()  where Id=" + Id;

                    int i = objData.Execute(insDelivery);
                }

                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();

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
    protected bool SaveIEPPage1()
    {
        bool getA = true;
        objData = new clsData();


        try
        {
            sess = (clsSession)Session["UserSession"];
            foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
            {
                objData = new clsData();

                DropDownList ddlGoals = diTypeA.FindControl("ddlGoals") as DropDownList;
                TextBox txtRevisionDate = diTypeA.FindControl("TxtMeasureAnualGoal") as TextBox;
                TextBox txtRoles = diTypeA.FindControl("TxtStudentsProgress") as TextBox;
                TextBox txtSection = diTypeA.FindControl("TxtDescReportProgress") as TextBox;
                TextBox txtFrequencyA = diTypeA.FindControl("TxtReportProgress") as TextBox;
                //TextBox txtStartDateA = diTypeA.FindControl("txtStartDateA") as TextBox;
                //TextBox txtEndDateA = diTypeA.FindControl("txtEndDateA") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);

                string insDelivery = "";
                if (Id == 0)
                {

                    insDelivery = "insert into StdtIEP_PE10_GoalsObj ([StdtIEP_PEId],[GoalID],[MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress],[CreatedBy],[CreatedOn]) " +
                                          "VALUES ('" + sess.IEPId + "'," + Convert.ToInt32(ddlGoals.SelectedValue) + ", '" + txtRevisionDate.Text + "', '" + txtRoles.Text + "', '" + txtSection.Text + "', '" + txtFrequencyA.Text + "', " +
                                          "'" + sess.LoginId + "' ,GETDATE())";


                    int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);


                }

                else
                {
                    insDelivery = "Update StdtIEP_PE10_GoalsObj set GoalId=" + Convert.ToInt32(ddlGoals.SelectedValue) + ",MeasureAnualGoal='" + clsGeneral.convertQuotes(txtRevisionDate.Text) + "',"
                    + "StudentsProgress='" + clsGeneral.convertQuotes(txtRoles.Text) + "',ModifiedBy=" + sess.LoginId + ","
                    + "DescReportProgress='" + clsGeneral.convertQuotes(txtSection.Text) + "',ReportProgress='" + clsGeneral.convertQuotes(txtFrequencyA.Text) + "',ModifiedOn=getdate()  where Id=" + Id;

                    int i = objData.Execute(insDelivery);
                }

                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();

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

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page10='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page10) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(11);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page10='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page10) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP11('saved');", true);
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
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtMeasureAnualGoal");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtStudentsProgress");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtDescReportProgress");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtReportProgress");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                    //**********************************************************************************************************
                    box0.Text =Convert.ToString(dt.Rows[i]["MeasureAnualGoal"]);
                    box1.Text = dt.Rows[i]["StudentsProgress"].ToString();
                    box2.Text = dt.Rows[i]["DescReportProgress"].ToString();
                    box3.Text = dt.Rows[i]["ReportProgress"].ToString();
                    //box4.Text = dt.Rows[i]["StartDate"].ToString();
                    //box5.Text = dt.Rows[i]["EndDate"].ToString();


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

    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];
            if (dtCurrentTable.Rows.Count > 4)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 5 Rows");
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    DropDownList ddlg0 = (DropDownList)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("ddlGoals");
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtMeasureAnualGoal");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtStudentsProgress");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtDescReportProgress");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtReportProgress");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    // Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();

                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["GoalID"] = ddlg0.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["MeasureAnualGoal"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["StudentsProgress"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["DescReportProgress"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["ReportProgress"] = box3.Text;
                    //dtCurrentTable.Rows[i - 1]["StartDate"] = box4.Text;
                    //dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["Id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
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
                    DropDownList ddlg0 = (DropDownList)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("ddlGoals");
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtMeasureAnualGoal");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtStudentsProgress");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtDescReportProgress");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtReportProgress");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    // Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();

                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["GoalID"] = ddlg0.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["MeasureAnualGoal"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["StudentsProgress"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["DescReportProgress"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["ReportProgress"] = box3.Text;
                    //dtCurrentTable.Rows[i - 1]["StartDate"] = box4.Text;
                    //dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["Id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
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
                    DropDownList ddlg0 = (DropDownList)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("ddlGoals");
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtMeasureAnualGoal");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtStudentsProgress");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtDescReportProgress");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtReportProgress");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                    //**********************************************************************************************************
                    ddlg0.SelectedValue = dt.Rows[i]["GoalID"].ToString();
                    box0.Text =Convert.ToString(dt.Rows[i]["MeasureAnualGoal"]);
                    box1.Text = dt.Rows[i]["StudentsProgress"].ToString();
                    box2.Text = dt.Rows[i]["DescReportProgress"].ToString();
                    box3.Text = dt.Rows[i]["ReportProgress"].ToString();
                    //box4.Text = dt.Rows[i]["StartDate"].ToString();
                    //box5.Text = dt.Rows[i]["EndDate"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTable"];

    }

    private void setInitialGrid()
    {
        Int32 i = 0;
        int j = 0;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }

        DataTable dt = new DataTable();

        //***************************************************************************************************************
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("GoalID", typeof(string));
        dt.Columns.Add("MeasureAnualGoal", typeof(string));
        dt.Columns.Add("StudentsProgress", typeof(string));
        dt.Columns.Add("DescReportProgress", typeof(string));
        dt.Columns.Add("ReportProgress", typeof(string));

        //=============================================

        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getPageOneGridDetails = "select [Id],[GoalID],[MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress] from [dbo].[StdtIEP_PE10_GoalsObj] where [StdtIEP_PEId]=" + sess.IEPId;
        DataTable dt_goalDetails = objData.ReturnDataTable(getPageOneGridDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    //======================================================
                    //dt.Rows.Add(dr["Participants"].ToString(), dr["Id"].ToString(), dr["SvcTypDesc"].ToString(), dr["IEPSection"].ToString(), dr["FreqDurDesc"].ToString(), dr["StartDate"].ToString(), dr["EndDate"].ToString());
                    dt.Rows.Add(dr["Id"].ToString(), dr["GoalID"].ToString(),Convert.ToString(dr["MeasureAnualGoal"]), dr["StudentsProgress"].ToString(), dr["DescReportProgress"].ToString(), dr["ReportProgress"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("", "", "", "", "", "");

            }
        }
        else
        {
            dt.Rows.Add("", "", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");
        //}



        ViewState["PreviousTable"] = dt;

        gvDelTypeA.DataSource = dt;
        gvDelTypeA.DataBind();
        DropDownList ddl1 = (DropDownList)gvDelTypeA.Rows[0].Cells[1].FindControl("ddlGoals");
        FillDropDownList(ddl1);
        HiddenField hfDepartmentId = gvDelTypeA.Rows[0].Cells[1].FindControl("hfGoalID") as HiddenField;
        if (hfDepartmentId == null)
            ddl1.SelectedValue = "0";
        else
            ddl1.SelectedValue = hfDepartmentId.Value;


    }

    private void FillDropDownList(DropDownList ddl1)
    {
        objData = new clsData();
        string sqlster = "  select SG.GoalId as ID,G.GoalName as Name from stdtgoal SG inner join Goal G on g.GoalId=SG.GoalId where  StdtIEPId=" + sess.IEPId + " and IncludeIEP=1";
        objData.ReturnDropDown(sqlster, ddl1);

    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        // fillGoalInAllDropdown(1);
    }

    protected void gvDelTypeA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }
            DropDownList ddl1 = (DropDownList)e.Row.Cells[1].FindControl("ddlGoals");
            FillDropDownList(ddl1);
            HiddenField hfDepartmentId = e.Row.Cells[1].FindControl("hfGoalID") as HiddenField;
            if (hfDepartmentId == null)
                ddl1.SelectedValue = "0";
            else
                ddl1.SelectedValue = hfDepartmentId.Value;
        }
    }

    protected void gvDelTypeA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //int index = int.Parse(e.CommandArgument.ToString());
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
                    string delRow = "delete from [dbo].[StdtIEP_PE10_GoalsObj] where Id=" + StdtGoalSvcId;

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

    private void deleteRowA(int rowID)
    {
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

                    DropDownList ddlg0 = (DropDownList)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("ddlGoals");
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtMeasureAnualGoal");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtStudentsProgress");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtDescReportProgress");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("TxtReportProgress");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    //    Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[1].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    //HiddenField box4 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtstartDate");
                    //HiddenField box5 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtendDate");
                    // DateofRevision Participants IEPSection

                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i - 1]["GoalID"] = ddlg0.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["MeasureAnualGoal"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["StudentsProgress"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["DescReportProgress"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["ReportProgress"] = box3.Text;
                    //dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["Id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                // dtCurrentTable.Rows.Add(drCurrentRow);

                dtCurrentTable.Rows.Remove(dtCurrentTable.Rows[rowID]);

                ViewState["PreviousTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();



                SetPreviousDB();
            }
        }
    }

    protected void gvDelTypeA_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        DataTable dt = new DataTable();
        //***************************************************************************************************************

        //GoalId
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("Benchmark", typeof(string));

        //   dt.Columns.Add("AgencyResponsible", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,Benchmark FROM dbo.IEP_PE10_Benchmark WHERE StdIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//============================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["Benchmark"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("0", "");

            }
        }
        else
        {
            dt.Rows.Add("0", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");

        //}

        ViewState["PreviousTableB"] = dt;

        GridViewBenchmark.DataSource = dt;
        GridViewBenchmark.DataBind();
    }

    protected bool SaveIEPPage11B()
    {
        bool getB = true;
        try
        {
            foreach (GridViewRow diTypeB in GridViewBenchmark.Rows)
            {

                TextBox txtServices = diTypeB.FindControl("TxtSupportService") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeB.FindControl("lbl_sprtid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {
                    insDelivery = "INSERT INTO IEP_PE10_Benchmark (Benchmark,StdIEP_PEId) " +
                                          "VALUES ('" + txtServices.Text.Trim() + "'," + sess.IEPId + " )";
                    int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);

                }

                else
                {
                    insDelivery = "UPDATE IEP_PE10_Benchmark SET Benchmark='" + txtServices.Text.Trim() + "' where Id=" + Id;

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

    private void deleteRowB(int rowID)
    {
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

                    TextBox box0 = (TextBox)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    Label lbl_svcGoalId = (Label)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("lbl_sprtid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Benchmark"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }

                ViewState["PreviousTableB"] = dtCurrentTableB;

                dtCurrentTableB.Rows.Remove(dtCurrentTableB.Rows[rowID]);



                GridViewBenchmark.DataSource = dtCurrentTableB;
                GridViewBenchmark.DataBind();
                SetPreviousDB_B();
            }
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
                // Response.Write(GridViewRelServ.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Benchmark"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTableB"];

    }

    protected void ButtonAddA_Click(object sender, EventArgs e)
    {
        AddNewRowToGridB();

    }

    private void AddNewRowToGridB()
    {
        int rowIndex = 0;

        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dtCurrentTableB = (DataTable)ViewState["PreviousTableB"];
            if (dtCurrentTableB.Rows.Count > 4)
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



                    TextBox box0 = (TextBox)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    Label lbl_svcGoalId = (Label)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("lbl_sprtid");

                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Benchmark"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                ViewState["PreviousTableB"] = dtCurrentTableB;

                GridViewBenchmark.DataSource = dtCurrentTableB;
                GridViewBenchmark.DataBind();

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
                    TextBox box0 = (TextBox)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    Label lbl_svcGoalId = (Label)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("lbl_sprtid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Benchmark"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                ViewState["CurrentTableB"] = dtCurrentTableB;

                GridViewBenchmark.DataSource = dtCurrentTableB;
                GridViewBenchmark.DataBind();

                //Set Previous Data on Postbacks
                SetPreviousDataB();
            }
        }
        else
        {
            Response.Write("ViewState is null");
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
                    TextBox box0 = (TextBox)GridViewBenchmark.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Benchmark"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTableB"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButtonB = GridViewBenchmark.FooterRow.FindControl("LinkButtonB") as LinkButton;
            LinkButtonB.Visible = true;
        }
    }
    protected void GridViewBenchmark_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //int index = int.Parse(e.CommandArgument.ToString());
        int index = 0;
        if (e.CommandArgument.ToString() != "")
        {
            index = int.Parse(e.CommandArgument.ToString());
        }
        else
        {
            return;
        }
        GridViewRow row = GridViewBenchmark.Rows[index];


        if (e.CommandName == "remove")
        {
            if (GridViewBenchmark.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_sprtid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from IEP_PE10_Benchmark where Id=" + StdtGoalSvcId;

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
    protected void GridViewBenchmark_DataBound(object sender, EventArgs e)
    {

    }
    protected void GridViewBenchmark_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}