using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP_PE12 : System.Web.UI.Page
{
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    public clsSession sess = null;
    DataTable DtESY = new DataTable();
    DataTable DtSuptServc = new DataTable();
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
            //loadGrd("GridViewESY");
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


    private void fillGridviews()
    {
        int eligible = 0;
        int notEligible = 0;
        setInitialGrid1();
        setInitialGrid2();
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

            string strQuery = "SELECT IEP12ElegibleForESY,IEP12ElegibleForESYInfo,IEP12NotElegibleForESY,IEP12NotElegibleForESYInfo,IEP12ShortTermObjectives FROM IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    TextBox2.InnerHtml = Convert.ToString(Dt.Rows[0]["IEP12ElegibleForESYInfo"]).Trim().Replace("#gt;", ">").Replace("#lt;", "<");
                    TextBox3.InnerHtml = Convert.ToString(Dt.Rows[0]["IEP12NotElegibleForESYInfo"]).Trim().Replace("#gt;", ">").Replace("#lt;", "<");
                    TextBox4.InnerHtml = Convert.ToString(Dt.Rows[0]["IEP12ShortTermObjectives"]).Trim().Replace("#gt;", ">").Replace("#lt;", "<");

                    TextBox2_hdn.Text = System.Uri.EscapeDataString(TextBox2.InnerHtml);
                    TextBox3_hdn.Text = System.Uri.EscapeDataString(TextBox3.InnerHtml);
                    TextBox4_hdn.Text = System.Uri.EscapeDataString(TextBox4.InnerHtml);

                    if (Dt.Rows[0]["IEP12ElegibleForESY"].ToString() != "")
                        eligible = Convert.ToInt32(Dt.Rows[0]["IEP12ElegibleForESY"]);
                    if (Dt.Rows[0]["IEP12NotElegibleForESY"].ToString() != "")
                        notEligible = Convert.ToInt32(Dt.Rows[0]["IEP12NotElegibleForESY"]);

                    if (eligible == 1) CheckBox1.Checked = true; else CheckBox1.Checked = false;
                    if (notEligible == 1) CheckBox2.Checked = true; else CheckBox2.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
       // setInitialGrid3();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool chkbxEligible = false;
        bool chkbxNotEligible = false;
        if (CheckBox1.Checked)
        {
            chkbxEligible = true;
        }
        if (CheckBox2.Checked)
        {
            chkbxNotEligible = true;
        }
        
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
                string IEP12ElegibleForESYInfo = System.Uri.UnescapeDataString(TextBox2_hdn.Text);
                string IEP12NotElegibleForESYInfo = System.Uri.UnescapeDataString(TextBox3_hdn.Text);
                string IEP12ShortTermObjectives = System.Uri.UnescapeDataString(TextBox4_hdn.Text);
                strQuery = "UPDATE IEP_PE_Details SET IEP12ElegibleForESYInfo='" + clsGeneral.convertQuotes(IEP12ElegibleForESYInfo) + "',IEP12NotElegibleForESYInfo='" + clsGeneral.convertQuotes(IEP12NotElegibleForESYInfo) + "',"
                    + "IEP12ElegibleForESY='" + chkbxEligible + "',IEP12NotElegibleForESY='" + chkbxNotEligible + "',IEP12ShortTermObjectives='" + clsGeneral.convertQuotes(IEP12ShortTermObjectives) + "' WHERE StdtIEP_PEId=" + sess.IEPId;
                //strQuery = "UPDATE IEP_PE_Details SET IEP12ElegibleForESYInfo='" + other + "',IEP12NotElegibleForESYInfo='" + CIPcode + "' WHERE StdtIEP_PEId=" + sess.IEPId;
                int id = oData.ExecuteNonQuery(strQuery);
                if (id > 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    setIEPPEupdateStatus();
                    //fillBasicDetails();
                }
               // tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                
                //fillGridviews();
            }
        }
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        bool chkbxEligible = false;
        bool chkbxNotEligible = false;
        if (CheckBox1.Checked)
        {
            chkbxEligible = true;
        }
        if (CheckBox2.Checked)
        {
            chkbxNotEligible = true;
        }

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
                string IEP12ElegibleForESYInfo = System.Uri.UnescapeDataString(TextBox2_hdn.Text);
                string IEP12NotElegibleForESYInfo = System.Uri.UnescapeDataString(TextBox3_hdn.Text);
                string IEP12ShortTermObjectives = System.Uri.UnescapeDataString(TextBox4_hdn.Text);
                strQuery = "UPDATE IEP_PE_Details SET IEP12ElegibleForESYInfo='" + clsGeneral.convertQuotes(IEP12ElegibleForESYInfo) + "',IEP12NotElegibleForESYInfo='" + clsGeneral.convertQuotes(IEP12NotElegibleForESYInfo) + "',"
                    + "IEP12ElegibleForESY='" + chkbxEligible + "',IEP12NotElegibleForESY='" + chkbxNotEligible + "',IEP12ShortTermObjectives='" + clsGeneral.convertQuotes(IEP12ShortTermObjectives) + "' WHERE StdtIEP_PEId=" + sess.IEPId;
                //strQuery = "UPDATE IEP_PE_Details SET IEP12ElegibleForESYInfo='" + other + "',IEP12NotElegibleForESYInfo='" + CIPcode + "' WHERE StdtIEP_PEId=" + sess.IEPId;
                int id = oData.ExecuteNonQuery(strQuery);
                if (id > 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    setIEPPEupdateStatus1();
                    //fillBasicDetails();
                }
                // tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");

                //fillGridviews();
            }
        }
    }

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page12='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page12) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(13);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page12='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page12) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP13('saved');", true);
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
        dt.Columns.Add("ESY", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("PrjBegDate", typeof(string));
        dt.Columns.Add("AnticipatedDue", typeof(string));
        // dt.Columns.Add("AgencyResponsible", typeof(string));
        //=============================================

        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,ESY,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE12_ESY WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            string dateString = "";
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    dateString = dr["PrjBeginning"].ToString();
                    if (dateString.Contains("01/01/1900")) dateString = "";
                    //======================================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["ESY"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dateString, dr["AnticipatedDur"].ToString());
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

        GridViewESY.DataSource = dt;
        GridViewESY.DataBind();

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
        dt.Columns.Add("SupportService", typeof(string));
       
        //   dt.Columns.Add("AgencyResponsible", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,SupportService FROM dbo.IEP_PE12_SupportService WHERE StdIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//============================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["SupportService"].ToString());
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

        GridViewsupportService.DataSource = dt;
        GridViewsupportService.DataBind();
    }

    protected bool SaveIEPPage11()
    {
        bool getA = true;
        objData = new clsData();
        try
        {
            sess = (clsSession)Session["UserSession"];

            foreach (GridViewRow diTypeA in GridViewESY.Rows)
            {
                objData = new clsData();

                TextBox txtESY = diTypeA.FindControl("TxtESY") as TextBox;
                TextBox txtLocation = diTypeA.FindControl("TxtLocation") as TextBox;
                TextBox txtFrequency = diTypeA.FindControl("TxtFrequency") as TextBox;
                TextBox txtPrjBegDate = diTypeA.FindControl("TxtPrjBegDate") as TextBox;
                TextBox txtAnticipated = diTypeA.FindControl("TxtAnticipatedDue") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);

                /*FOR DEFAULT DATE IN PAGE-12
                 * BUG ID=289
                 */



                string insDelivery = "";
                if (Id == 0)
                {
                    if (txtESY.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtPrjBegDate.Text != "" || txtAnticipated.Text != "")
                    {
                        //if (txtPrjBegDate.Text == "")
                        //    txtPrjBegDate.Text = System.DateTime.Now.ToString();
                        insDelivery = "INSERT INTO IEP_PE12_ESY (ESY,Location,Frequency,PrjBeginning,AnticipatedDur,StdtIEP_PEId) " +
                                      "VALUES ('" + txtESY.Text.Trim() + "', '" + txtLocation.Text.Trim() + "', '" + txtFrequency.Text.Trim() + "', '" + txtPrjBegDate.Text.Trim() + "', " +
                                      "'" + txtAnticipated.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtESY.Text != "" || txtLocation.Text != "" || txtFrequency.Text != "" || txtPrjBegDate.Text != "" || txtAnticipated.Text != "")
                    {
                        //if (txtPrjBegDate.Text == "")
                        //    txtPrjBegDate.Text = System.DateTime.Now.ToString();
                        insDelivery = "UPDATE IEP_PE12_ESY SET ESY='" + txtESY.Text.Trim() + "',Location='" + txtLocation.Text.Trim() + "',Frequency='" + txtFrequency.Text.Trim() + "',"
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
            foreach (GridViewRow diTypeB in GridViewsupportService.Rows)
            {

                TextBox txtServices = diTypeB.FindControl("TxtSupportService") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeB.FindControl("lbl_sprtid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {
                    if (txtServices.Text != "")
                    {
                        insDelivery = "INSERT INTO IEP_PE12_SupportService (SupportService,StdIEP_PEId) " +
                                              "VALUES ('" + txtServices.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtServices.Text != "")
                    {
                        insDelivery = "UPDATE IEP_PE12_SupportService SET SupportService='" + txtServices.Text.Trim() + "' where Id=" + Id;

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

    protected void GridViewESY_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRow(e.RowIndex, "GridViewESY");
    }
    protected void GridViewESY_RowCommand(object sender, GridViewCommandEventArgs e)
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
        GridViewRow row = GridViewESY.Rows[index];

        if (e.CommandName == "remove")
        {
            if (GridViewESY.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from IEP_PE12_ESY where Id=" + StdtGoalSvcId;

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

                    TextBox box0 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtESY");
                    TextBox box1 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    Label lbl_svcGoalId = (Label)GridViewESY.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["ESY"] = box0.Text;

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

                GridViewESY.DataSource = dtCurrentTable;
                GridViewESY.DataBind();


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

                    TextBox box0 = (TextBox)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    Label lbl_svcGoalId = (Label)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("lbl_sprtid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["SupportService"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }

                ViewState["PreviousTableB"] = dtCurrentTableB;

                dtCurrentTableB.Rows.Remove(dtCurrentTableB.Rows[rowID]);



                GridViewsupportService.DataSource = dtCurrentTableB;
                GridViewsupportService.DataBind();
                SetPreviousDB_B();
            }
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
                // Response.Write(GridViewModSDI.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtESY");
                    TextBox box1 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");


                    //*********************************************************************************************

                    box0.Text = dt.Rows[i]["ESY"].ToString();
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
                    TextBox box0 = (TextBox)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");
                    
                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["SupportService"].ToString();
                    

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTableB"];

    }

    public void delRow(int intexRow, string grd)
    {
        if (grd == "GridViewESY")
        {
            DtESY.Columns.Add("ESY");
            DtESY.Columns.Add("Location");
            DtESY.Columns.Add("Frequency");
            DtESY.Columns.Add("PrjBegDate");
            DtESY.Columns.Add("AnticipatedDue");

            foreach (GridViewRow gdVr in GridViewESY.Rows)
            {
                DataRow drtemp = DtESY.NewRow();

                drtemp["ESY"] = ((TextBox)gdVr.FindControl("TxtESY")).Text;
                drtemp["Location"] = ((TextBox)gdVr.FindControl("TxtLocation")).Text;
                drtemp["Frequency"] = ((TextBox)gdVr.FindControl("TxtFrequency")).Text;
                drtemp["PrjBegDate"] = ((TextBox)gdVr.FindControl("TxtPrjBegDate")).Text;
                drtemp["AnticipatedDue"] = ((TextBox)gdVr.FindControl("TxtAnticipatedDue")).Text;
                DtESY.Rows.Add(drtemp);
            }
            if (DtESY.Rows.Count == 1)
            {
                DtESY.Rows.RemoveAt(intexRow);
                loadGrd("GridViewESY");
            }
            else
            {
                DtESY.Rows.RemoveAt(intexRow);
            }

            GridViewESY.DataSource = DtESY;
            GridViewESY.DataBind();
        }


    }
    public void addRowAll(string grd)
    {
        if (grd == "GridViewESY")
        {

            DtESY.Columns.Add("ESY");
            DtESY.Columns.Add("Location");
            DtESY.Columns.Add("Frequency");
            DtESY.Columns.Add("PrjBegDate");
            DtESY.Columns.Add("AnticipatedDue");


            foreach (GridViewRow gdVr in GridViewESY.Rows)
            {
                DataRow drtemp = DtESY.NewRow();

                drtemp["ESY"] = ((TextBox)gdVr.FindControl("TxtESY")).Text;
                drtemp["Location"] = ((TextBox)gdVr.FindControl("TxtLocation")).Text;
                drtemp["Frequency"] = ((TextBox)gdVr.FindControl("TxtFrequency")).Text;
                drtemp["PrjBegDate"] = ((TextBox)gdVr.FindControl("TxtPrjBegDate")).Text;
                drtemp["AnticipatedDue"] = ((TextBox)gdVr.FindControl("TxtAnticipatedDue")).Text;
                DtESY.Rows.Add(drtemp);
            }
            DataRow dr = DtESY.NewRow();
            dr["ESY"] = "";
            dr["Location"] = "";
            dr["Frequency"] = "";
            dr["PrjBegDate"] = "";
            dr["AnticipatedDue"] = "";
            DtESY.Rows.Add(dr);
            GridViewESY.DataSource = DtESY;
            GridViewESY.DataBind();
        }


    }
    public void loadGrd(string grd)
    {
        if (grd == "GridViewESY")
        {
            //DtESY = new DataTable();
            //DtESY.Columns.Add("ESY");
            //DtESY.Columns.Add("Location");
            //DtESY.Columns.Add("Frequency");
            //DtESY.Columns.Add("PrjBegDate");
            //DtESY.Columns.Add("AnticipatedDue");

            //DataRow Rw = DtESY.NewRow();
            //Rw["ESY"] = "";
            //Rw["Location"] = "";
            //Rw["Frequency"] = "";
            //Rw["PrjBegDate"] = "";
            //Rw["AnticipatedDue"] = "";
            //DtESY.Rows.Add(Rw);
            //GridViewESY.DataSource = DtESY;
            //GridViewESY.DataBind();

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
            dt.Columns.Add("ESY", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Columns.Add("Frequency", typeof(string));
            dt.Columns.Add("PrjBeginning", typeof(string));
            dt.Columns.Add("AnticipatedDur", typeof(string));
            //=============================================

            //if (dt_stdGoal.Rows.Count > 0)
            //{

            string getStdGoalSvcDetails = "select [Id],[ESY],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from [dbo].[IEP_PE12_ESY] where [StdtIEP_PEId]=" + sess.IEPId;

            DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
            if (dt_goalDetails != null)
            {
                if (dt_goalDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt_goalDetails.Rows)
                    {
                        //======================================================
                        dt.Rows.Add(dr["Id"].ToString(), dr["ESY"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString());
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

            GridViewESY.DataSource = dt;
            GridViewESY.DataBind();
        }

    }
    protected void GridViewsupportService_RowCommand(object sender, GridViewCommandEventArgs e)
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
        GridViewRow row = GridViewsupportService.Rows[index];


        if (e.CommandName == "remove")
        {
            if (GridViewsupportService.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_sprtid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from IEP_PE12_SupportService where Id=" + StdtGoalSvcId;

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
    protected void GridViewsupportService_DataBound(object sender, EventArgs e)
    {

    }
    protected void GridViewsupportService_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GridViewESY_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridViewsupportService_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();

    }
    protected void ButtonAddA_Click(object sender, EventArgs e)
    {
        AddNewRowToGridB();

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
                //string msg = tdMsg.InnerHtml;
                //ClientScript.RegisterStartupScript(this.GetType(), "", "ShowQuickMsq('Sorry!!Limited to 5 Rows');", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "", "ShowQuickMsq('Sorry!!Limited to 5 Rows');", true);
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values



                    TextBox box0 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtESY");
                    TextBox box1 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    Label lbl_svcGoalId = (Label)GridViewESY.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["ESY"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDue"] = box4.Text;

                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PreviousTable"] = dtCurrentTable;

                GridViewESY.DataSource = dtCurrentTable;
                GridViewESY.DataBind();

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
                    TextBox box0 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtESY");
                    TextBox box1 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtLocation");
                    TextBox box2 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtFrequency");
                    TextBox box3 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    Label lbl_svcGoalId = (Label)GridViewESY.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["ESY"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["PrjBegDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDue"] = box4.Text;

                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                GridViewESY.DataSource = dtCurrentTable;
                GridViewESY.DataBind();

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
        int rowIndex = 0;

        if (ViewState["PreviousTableB"] != null)
        {
            DataTable dtCurrentTableB = (DataTable)ViewState["PreviousTableB"];
            if (dtCurrentTableB.Rows.Count > 2)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 3 Rows");
                //string msg = tdMsg.Inn;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "any name", "ShowQuickMsq('Sorry! Limited to 3 Rows');", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('hello world');", true);
                
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTableB.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableB.Rows.Count; i++)
                {
                    //extract the TextBox values



                    TextBox box0 = (TextBox)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");

                    Label lbl_svcGoalId = (Label)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("lbl_sprtid");

                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["SupportService"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                ViewState["PreviousTableB"] = dtCurrentTableB;

                GridViewsupportService.DataSource = dtCurrentTableB;
                GridViewsupportService.DataBind();

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
                    TextBox box0 = (TextBox)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");
                    
                    Label lbl_svcGoalId = (Label)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("lbl_sprtid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["SupportService"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
                ViewState["CurrentTableB"] = dtCurrentTableB;

                GridViewsupportService.DataSource = dtCurrentTableB;
                GridViewsupportService.DataBind();

                //Set Previous Data on Postbacks
                SetPreviousDataB();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
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
                    TextBox box0 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtESY");
                    TextBox box1 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtPrjBegDate");
                    TextBox box4 = (TextBox)GridViewESY.Rows[rowIndex].Cells[0].FindControl("TxtAnticipatedDue");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["ESY"].ToString();
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

            LinkButton LinkButton1 = GridViewESY.FooterRow.FindControl("LinkButton1") as LinkButton;
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
                    TextBox box0 = (TextBox)GridViewsupportService.Rows[rowIndex].Cells[0].FindControl("TxtSupportService");
                   
                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["SupportService"].ToString();
                    

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTableB"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButtonB = GridViewsupportService.FooterRow.FindControl("LinkButtonB") as LinkButton;
            LinkButtonB.Visible = true;
        }
    }
}