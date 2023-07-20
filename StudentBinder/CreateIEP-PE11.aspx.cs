using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE11 : System.Web.UI.Page
{
    DataTable DtModSdi = new DataTable();
    DataTable DtRelserv = new DataTable();
    DataTable DtSpprtSchol = new DataTable();
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    public clsSession sess = null;

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


            fillGridviews();
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
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {

            bool result1 = SaveIEPPage11();
            if (result1 == true)
            {
                result1 = SaveIEPPage11B();
            }
            if (result1 == true)
            {
                result1 = SaveIEPPage11C();
            }
            if (result1 == true)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();
                //if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                //{
                //    objData.Execute("update stdtIEPUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
                //}
                //else
                //{
                //    objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
                //}
                // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP6();", true);
                fillGridviews();
            }
        }
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {

            bool result1 = SaveIEPPage11();
            if (result1 == true)
            {
                result1 = SaveIEPPage11B();
            }
            if (result1 == true)
            {
                result1 = SaveIEPPage11C();
            }
            if (result1 == true)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();
                //if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                //{
                //    objData.Execute("update stdtIEPUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
                //}
                //else
                //{
                //    objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
                //}
                // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP6();", true);
                fillGridviews();
            }
        }
    }
    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page11='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page11) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(12);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page11='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page11) values(" + sess.IEPId + ",'true')");
        }

       // ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP12('saved');", true);
    }

    private void fillGridviews()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        DataTable dataStud = new DataTable();
        try
        { 
            //display student name
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
        }
        catch (Exception ex)
        {
            throw ex;
        }

        setInitialGrid1();
        setInitialGrid2();
        setInitialGrid3();

    }

    protected bool SaveIEPPage11()
    {
        bool getA = true;
        objData = new clsData();
        try
        {
            sess = (clsSession)Session["UserSession"];

            foreach (GridViewRow diTypeA in GridViewModSDI.Rows)
            {
                objData = new clsData();

                TextBox txtModsdi = diTypeA.FindControl("TxtModsdi") as TextBox;
                TextBox txtLocation = diTypeA.FindControl("txtLocation") as TextBox;
                TextBox txtFrequency = diTypeA.FindControl("txtFrequency") as TextBox;
                TextBox txtPrjBegDate = diTypeA.FindControl("TxtPrjBegDate") as TextBox;
                TextBox txtAnticipated = diTypeA.FindControl("TxtAnticipatedDue") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {
                    if (txtModsdi.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtAnticipated.Text != "" || txtPrjBegDate.Text != "")
                    {
                        if (txtPrjBegDate.Text == "")
                            txtPrjBegDate.Text = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        insDelivery = "INSERT INTO IEP_PE11_SDI (SDI,Location,Frequency,PrjBeginning,AnticipatedDur,StdtIEP_PEId) " +
                                      "VALUES ('" + txtModsdi.Text.Trim() + "', '" + txtLocation.Text.Trim() + "', '" + txtFrequency.Text.Trim() + "', '" + txtPrjBegDate.Text.Trim() + "', " +
                                      "'" + txtAnticipated.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtModsdi.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtAnticipated.Text != "" || txtPrjBegDate.Text != "")
                    {
                        if (txtPrjBegDate.Text == "")
                            txtPrjBegDate.Text = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        insDelivery = "UPDATE IEP_PE11_SDI SET SDI='" + txtModsdi.Text.Trim() + "',Location='" + txtLocation.Text.Trim() + "',Frequency='" + txtFrequency.Text.Trim() + "',"
                            + "PrjBeginning='" + txtPrjBegDate.Text.Trim() + "',AnticipatedDur='" + txtAnticipated.Text.Trim() + "' where Id=" + Id;

                        int i = objData.Execute(insDelivery);
                    }
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
    protected bool SaveIEPPage11B()
    {
        bool getB = true;
        try
        {
            foreach (GridViewRow diTypeB in GridViewRelServ.Rows)
            {

                TextBox txtServices = diTypeB.FindControl("TxtService") as TextBox;
                TextBox txtLocation = diTypeB.FindControl("TxtLocation") as TextBox;
                TextBox txtFrequency = diTypeB.FindControl("TxtFrequency") as TextBox;
                TextBox txtPrjBegDate = diTypeB.FindControl("TxtPrjBegDate") as TextBox;
                TextBox txtAnticipated = diTypeB.FindControl("TxtAnticipatedDue") as TextBox;


                Label lbl_StdtGoalSvcId = diTypeB.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {
                    if (txtServices.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtAnticipated.Text != "" || txtPrjBegDate.Text != "")
                    {
                        if (txtPrjBegDate.Text == "")
                            txtPrjBegDate.Text = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        insDelivery = "INSERT INTO IEP_PE11_Service (Service,Location,Frequency,PrjBeginning,AnticipatedDur,StdtIEP_PEId) " +
                                              "VALUES ('" + txtServices.Text.Trim() + "', '" + txtLocation.Text.Trim() + "', '" + txtFrequency.Text.Trim() + "', '" + txtPrjBegDate.Text.Trim() + "', " +
                                              "'" + txtAnticipated.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtServices.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtAnticipated.Text != "" || txtPrjBegDate.Text != "")
                    {
                        if (txtPrjBegDate.Text == "")
                            txtPrjBegDate.Text = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        insDelivery = "UPDATE IEP_PE11_Service SET Service='" + txtServices.Text.Trim() + "',Location='" + txtLocation.Text.Trim() + "',Frequency='" + txtFrequency.Text.Trim() + "',"
                            + "PrjBeginning='" + txtPrjBegDate.Text.Trim() + "',AnticipatedDur='" + txtAnticipated.Text.Trim() + "' where Id=" + Id;

                        int i = objData.Execute(insDelivery);
                    }
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
    protected bool SaveIEPPage11C()
    {
        bool getC = true;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {
            foreach (GridViewRow diTypeC in GridViewSpprtSchol.Rows)
            {
                TextBox txtSchoolPer = diTypeC.FindControl("TxtScholPer") as TextBox;
                TextBox txtSupport = diTypeC.FindControl("TxtService") as TextBox;
                TextBox txtLocation = diTypeC.FindControl("TxtLocation") as TextBox;
                TextBox txtFrequency = diTypeC.FindControl("TxtFrequency") as TextBox;
                TextBox txtPrjBegDate = diTypeC.FindControl("TxtPrjBegDate") as TextBox;
                TextBox txtAnticpated = diTypeC.FindControl("TxtAnticipatedDue") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeC.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {//StdtIEP_PEId
                    if (txtSchoolPer.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtAnticpated.Text != "" || txtSupport.Text != "" || txtPrjBegDate.Text != "")
                    {
                        if (txtPrjBegDate.Text == "")
                            txtPrjBegDate.Text = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        insDelivery = "INSERT INTO IEP_PE11_SchoolPer (SchoolPerson,Location,Frequency,PrjBeginning,AnticipatedDur,Person,StdtIEP_PEId) " +
                                              "VALUES ('" + txtSchoolPer.Text.Trim() + "', '" + txtLocation.Text.Trim() + "', '" + txtFrequency.Text.Trim() + "', '" + txtPrjBegDate.Text.Trim() + "', " +
                                              "'" + txtAnticpated.Text.Trim() + "', '" + txtSupport.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtSchoolPer.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtAnticpated.Text != "" || txtSupport.Text != "" || txtPrjBegDate.Text != "")
                    {
                        if (txtPrjBegDate.Text == "")
                            txtPrjBegDate.Text = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        insDelivery = "UPDATE IEP_PE11_SchoolPer SET SchoolPerson='" + txtSchoolPer.Text.Trim() + "',Location='" + txtLocation.Text.Trim() + "',Frequency='" + txtFrequency.Text.Trim() + "',"
                            + "PrjBeginning='" + txtPrjBegDate.Text.Trim() + "',AnticipatedDur='" + txtAnticpated.Text.Trim() + "',Person='" + txtSupport.Text.Trim() + "' "
                            + "  where Id=" + Id;

                        int i = objData.Execute(insDelivery);
                    }
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


    private void setInitialGrid1()
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
        //   (SDI,Location,Frequency,PrjBeginning,AnticipatedDur,CreatedBy,CreatedOn,
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("Modsdi", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("PrjBegDate", typeof(string));
        dt.Columns.Add("AnticipatedDue", typeof(string));
        // dt.Columns.Add("AgencyResponsible", typeof(string));
        //=============================================

        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,SDI,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE11_SDI WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    //======================================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["SDI"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("0", "", "", "", "", "");

            }
        }
        else
        {
            dt.Rows.Add("0", "", "", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");
        //}


        ViewState["PreviousTable"] = dt;

        GridViewModSDI.DataSource = dt;
        GridViewModSDI.DataBind();

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
        dt.Columns.Add("Service", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("PrjBegDate", typeof(string));
        dt.Columns.Add("AnticipatedDue", typeof(string));
     //   dt.Columns.Add("AgencyResponsible", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE11_Service WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//============================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["Service"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("0", "", "", "", "", "");

            }
        }
        else
        {
            dt.Rows.Add("0", "", "", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");

        //}

        ViewState["PreviousTableB"] = dt;

        GridViewRelServ.DataSource = dt;
        GridViewRelServ.DataBind();
    }
    private void setInitialGrid3()
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        DataTable dt = new DataTable();
        //***************************************************************************************************************
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("ScholPer", typeof(string));
        dt.Columns.Add("Support", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("PrjBegDate", typeof(string));
        dt.Columns.Add("AnticipatedDue", typeof(string));
        // dt.Columns.Add("AgencyResponsible", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,SchoolPerson,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE11_SchoolPer WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//===============================
                    dt.Rows.Add(dr["Id"].ToString(), dr["SchoolPerson"].ToString(), dr["Person"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("0", "", "", "", "", "", "");

            }
        }
        else
        {
            dt.Rows.Add("0", "", "", "", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");

        //}

        ViewState["PreviousTableC"] = dt;

        GridViewSpprtSchol.DataSource = dt;
        GridViewSpprtSchol.DataBind();
    }




    protected void GridViewModSDI_RowCommand(object sender, GridViewCommandEventArgs e)
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
        GridViewRow row = GridViewModSDI.Rows[index];

        if (e.CommandName == "remove")
        {
            if (GridViewModSDI.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from IEP_PE11_SDI where Id=" + StdtGoalSvcId;

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
    protected void GridViewModSDI_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        

    }


    protected void GridViewRelServ_RowCommand(object sender, GridViewCommandEventArgs e)
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
        GridViewRow row = GridViewRelServ.Rows[index];

      
        if (e.CommandName == "remove")
        {
            if (GridViewRelServ.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from IEP_PE11_Service where Id=" + StdtGoalSvcId;

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
    protected void GridViewRelServ_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // delRow(e.RowIndex, "GridViewRelServ");
       
    }


    protected void GridViewSpprtSchol_RowCommand(object sender, GridViewCommandEventArgs e)
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
        GridViewRow row = GridViewSpprtSchol.Rows[index];

     
        if (e.CommandName == "remove")
        {
            if (GridViewSpprtSchol.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from IEP_PE11_SchoolPer where Id=" + StdtGoalSvcId;

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
    protected void GridViewSpprtSchol_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
       
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

                    TextBox box0 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtModsdi");
                    TextBox box1 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    Label lbl_svcGoalId = (Label)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["Modsdi"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDue"] = box4.Text;
                                       
                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }
                // dtCurrentTable.Rows.Add(drCurrentRow);

                dtCurrentTable.Rows.Remove(dtCurrentTable.Rows[rowID]);

                ViewState["PreviousTable"] = dtCurrentTable;

                GridViewModSDI.DataSource = dtCurrentTable;
                GridViewModSDI.DataBind();


                SetPreviousDB();
            }
        }
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

                    TextBox box0 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box1 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                                   
                    Label lbl_svcGoalId = (Label)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["AnticipatedDue"] = box4.Text;

                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }

                ViewState["PreviousTableB"] = dtCurrentTableB;

                dtCurrentTableB.Rows.Remove(dtCurrentTableB.Rows[rowID]);



                GridViewRelServ.DataSource = dtCurrentTableB;
                GridViewRelServ.DataBind();
                SetPreviousDB_B();
            }
        }
    }
    private void deleteRowC(int rowID)
    {
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
                    TextBox box0 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtScholPer");
                    TextBox box1 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box2 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box3 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box4 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box5 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                                       
                    Label lbl_svcGoalId = (Label)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //***************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["ScholPer"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["Support"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["Location"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["Frequency"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["PrjBegDate"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["AnticipatedDue"] = box5.Text;

                    
                    dtCurrentTableC.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }

                ViewState["PreviousTableC"] = dtCurrentTableC;

                dtCurrentTableC.Rows.Remove(dtCurrentTableC.Rows[rowID]);

                GridViewSpprtSchol.DataSource = dtCurrentTableC;
                GridViewSpprtSchol.DataBind();
                SetPreviousDB_C();
            }
        }
    }


    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();

    }
    protected void ButtonAddB_Click(object sender, EventArgs e)
    {
        AddNewRowToGridB();

    }
    protected void ButtonAddC_Click(object sender, EventArgs e)
    {
        AddNewRowToGridC();
    }



    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];
            if (dtCurrentTable.Rows.Count > 2)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 3 Rows");
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values

                  

                    TextBox box0 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtModsdi");
                    TextBox box1 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                  
                    Label lbl_svcGoalId = (Label)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");
                                     
                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["Modsdi"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDue"] = box4.Text;
                   
                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PreviousTable"] = dtCurrentTable;

                GridViewModSDI.DataSource = dtCurrentTable;
                GridViewModSDI.DataBind();

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
                    TextBox box0 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtModsdi");
                    TextBox box1 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    Label lbl_svcGoalId = (Label)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["Modsdi"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDue"] = box4.Text;

                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                GridViewModSDI.DataSource = dtCurrentTable;
                GridViewModSDI.DataBind();

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

              

                    TextBox box0 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box1 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                   
                    Label lbl_svcGoalId = (Label)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["AnticipatedDue"] = box4.Text;
                   
                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                ViewState["PreviousTableB"] = dtCurrentTableB;

                GridViewRelServ.DataSource = dtCurrentTableB;
                GridViewRelServ.DataBind();

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
                    TextBox box0 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box1 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    Label lbl_svcGoalId = (Label)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["AnticipatedDue"] = box4.Text;
                   
                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                ViewState["CurrentTableB"] = dtCurrentTableB;

                GridViewRelServ.DataSource = dtCurrentTableB;
                GridViewRelServ.DataBind();

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
        int rowIndex = 0;

        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dtCurrentTableC = (DataTable)ViewState["PreviousTableC"];
            if (dtCurrentTableC.Rows.Count > 1)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 2 Rows");
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTableC.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableC.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtScholPer");
                    TextBox box1 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box2 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box3 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box4 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box5 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                    Label lbl_svcGoalId = (Label)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //***************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["ScholPer"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["Support"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["Location"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["Frequency"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["PrjBegDate"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["AnticipatedDue"] = box5.Text;
                    dtCurrentTableC.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableC.Rows.Add(drCurrentRow);
                ViewState["PreviousTableC"] = dtCurrentTableC;

                GridViewSpprtSchol.DataSource = dtCurrentTableC;
                GridViewSpprtSchol.DataBind();

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
                    TextBox box0 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtScholPer");
                    TextBox box1 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box2 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box3 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box4 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box5 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                    Label lbl_svcGoalId = (Label)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");



                    drCurrentRow = dtCurrentTableC.NewRow();
                    //***************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["ScholPer"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["Support"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["Location"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["Frequency"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["PrjBegDate"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["AnticipatedDue"] = box5.Text;
                    dtCurrentTableC.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }
                dtCurrentTableC.Rows.Add(drCurrentRow);
                ViewState["CurrentTableC"] = dtCurrentTableC;

                GridViewSpprtSchol.DataSource = dtCurrentTableC;
                GridViewSpprtSchol.DataBind();


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
                // Response.Write(GridViewRelServ.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box1 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                   
                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["PrjBegDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDue"].ToString();
                    

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
                // Response.Write(GridViewSpprtSchol.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtScholPer");
                    TextBox box1 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box2 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box3 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box4 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box5 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");
                    Label lbl_svcGoalId = (Label)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    //*********************************************************************************************

                    box0.Text = dt.Rows[i]["ScholPer"].ToString();
                    box1.Text = dt.Rows[i]["Support"].ToString();
                    box2.Text = dt.Rows[i]["Location"].ToString();
                    box3.Text = dt.Rows[i]["Frequency"].ToString();
                    box4.Text = dt.Rows[i]["PrjBegDate"].ToString();
                    box5.Text = dt.Rows[i]["AnticipatedDue"].ToString();


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
                // Response.Write(GridViewModSDI.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtModsdi");
                    TextBox box1 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                   
                    //*********************************************************************************************
                   
                    box0.Text = dt.Rows[i]["Modsdi"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["PrjBegDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDue"].ToString();
                   

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTable"];

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
                    TextBox box0 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtModsdi");
                    TextBox box1 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewModSDI.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Modsdi"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["PrjBegDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDue"].ToString();

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTable"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButton1 = GridViewModSDI.FooterRow.FindControl("LinkButton1") as LinkButton;
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
                    TextBox box0 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box1 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewRelServ.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["PrjBegDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDue"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTableB"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButtonB = GridViewRelServ.FooterRow.FindControl("LinkButtonB") as LinkButton;
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
                    TextBox box0 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtScholPer");
                    TextBox box1 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtService");
                    TextBox box2 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box3 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box4 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box5 = (TextBox)GridViewSpprtSchol.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["ScholPer"].ToString();
                    box1.Text = dt.Rows[i]["Support"].ToString();
                    box2.Text = dt.Rows[i]["Location"].ToString();
                    box3.Text = dt.Rows[i]["Frequency"].ToString();
                    box4.Text = dt.Rows[i]["PrjBegDate"].ToString();
                    box5.Text = dt.Rows[i]["AnticipatedDue"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTableC"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButtonC = GridViewSpprtSchol.FooterRow.FindControl("LinkButtonC") as LinkButton;
            LinkButtonC.Visible = true;
        }
    }



    protected void GridViewModSDI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }
        }
    }
    protected void GridViewRelServ_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }
        }
    }
    protected void GridViewSpprtSchol_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }
        }
    }
}