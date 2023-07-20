using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP_PE6 : System.Web.UI.Page
{
    //clsData objData = null;
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    public clsSession sess = null;
    static string args1 = "", args2 = "", args3 = "", args4 = "", args5 = "", args6 = "";
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



    //----------------------------------------------------------------------------------------------------------------------------------------//




    //protected void LinkButton1_Click(object sender, EventArgs e)
    //{
    //    // RemoveRowFromGrid();
    //    LinkButton lb = (LinkButton)sender;
    //    GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
    //    int rowID = gvRow.RowIndex + 1;
    //    if (ViewState["PreviousTable"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["PreviousTable"];
    //        if (dt.Rows.Count > 1)
    //        {
    //            if (gvRow.RowIndex < dt.Rows.Count - 1)
    //            {
    //                //Remove the Selected Row data
    //                dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
    //            }
    //        }
    //        ViewState["PreviousTable"] = dt;
    //        //Re bind the GridView for the updated data
    //        gvDelTypeA.DataSource = dt;
    //        gvDelTypeA.DataBind();
    //        int rowIndex = 0;
    //        foreach (GridViewRow row in gvDelTypeA.Rows)
    //        {
    //            // txtService txtLocation txtFrequency txtBeginningDate txtAnticipatedDuration txtAgencyResponsible

    //            TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
    //            TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
    //            TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
    //            TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
    //            TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
    //            TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

    //            //*********************************************************************************************
    //            box0.Text = dt.Rows[rowIndex]["Service"].ToString();
    //            box1.Text = dt.Rows[rowIndex]["Location"].ToString();
    //            box2.Text = dt.Rows[rowIndex]["Frequency"].ToString();
    //            box3.Text = dt.Rows[rowIndex]["BeginningDate"].ToString();
    //            box4.Text = dt.Rows[rowIndex]["AnticipatedDuration"].ToString();
    //            box5.Text = dt.Rows[rowIndex]["AgencyResponsible"].ToString();


    //            rowIndex++;
    //        }
    //        if (dt.Rows.Count > 1)
    //        {
    //            LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
    //            LinkButton1.Visible = true;
    //        }


    //    }
    //    else if (ViewState["CurrentTable"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["CurrentTable"];
    //        if (dt.Rows.Count > 1)
    //        {
    //            if (gvRow.RowIndex < dt.Rows.Count - 1)
    //            {
    //                //Remove the Selected Row data
    //                dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
    //            }
    //        }
    //        ViewState["CurrentTable"] = dt;
    //        //Re bind the GridView for the updated data
    //        gvDelTypeA.DataSource = dt;
    //        gvDelTypeA.DataBind();
    //        int rowIndex = 0;

    //        //Set Previous Data on Postbacks
    //        SetPreviousData();
    //        foreach (GridViewRow row in gvDelTypeA.Rows)
    //        {

    //            TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
    //            TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
    //            TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
    //            TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
    //            TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
    //            TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

    //            //*********************************************************************************************
    //            box0.Text = dt.Rows[rowIndex]["Service"].ToString();
    //            box1.Text = dt.Rows[rowIndex]["Location"].ToString();
    //            box2.Text = dt.Rows[rowIndex]["Frequency"].ToString();
    //            box3.Text = dt.Rows[rowIndex]["BeginningDate"].ToString();
    //            box4.Text = dt.Rows[rowIndex]["AnticipatedDuration"].ToString();
    //            box5.Text = dt.Rows[rowIndex]["AgencyResponsible"].ToString();
    //            rowIndex++;
    //        }
    //        if (dt.Rows.Count > 1)
    //        {
    //            LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
    //            LinkButton1.Visible = true;
    //        }
    //    }

    //}
    //protected void LinkButtonB_Click(object sender, EventArgs e)
    //{
    //    LinkButton lb = (LinkButton)sender;
    //    GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
    //    int rowID = gvRow.RowIndex + 1;
    //    if (ViewState["PreviousTableB"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["PreviousTableB"];
    //        if (dt.Rows.Count > 1)
    //        {
    //            if (gvRow.RowIndex < dt.Rows.Count - 1)
    //            {
    //                //Remove the Selected Row data
    //                dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
    //            }
    //        }
    //        ViewState["PreviousTableB"] = dt;
    //        //Re bind the GridView for the updated data
    //        gvDelTypeB.DataSource = dt;
    //        gvDelTypeB.DataBind();
    //        int rowIndex = 0;
    //        foreach (GridViewRow row in gvDelTypeB.Rows)
    //        {
    //            TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
    //            TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
    //            TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
    //            TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
    //            TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
    //            TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

    //            //*********************************************************************************************
    //            box0.Text = dt.Rows[rowIndex]["Service"].ToString();
    //            box1.Text = dt.Rows[rowIndex]["Location"].ToString();
    //            box2.Text = dt.Rows[rowIndex]["Frequency"].ToString();
    //            box3.Text = dt.Rows[rowIndex]["BeginningDate"].ToString();
    //            box4.Text = dt.Rows[rowIndex]["AnticipatedDuration"].ToString();
    //            box5.Text = dt.Rows[rowIndex]["AgencyResponsible"].ToString();


    //            rowIndex++;
    //        }
    //        if (dt.Rows.Count > 1)
    //        {
    //            LinkButton LinkButtonB = gvDelTypeB.FooterRow.FindControl("LinkButtonB") as LinkButton;
    //            LinkButtonB.Visible = true;
    //        }
    //    }
    //    else if (ViewState["CurrentTableB"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["CurrentTableB"];
    //        if (dt.Rows.Count > 1)
    //        {
    //            if (gvRow.RowIndex < dt.Rows.Count - 1)
    //            {
    //                //Remove the Selected Row data
    //                dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
    //            }
    //        }
    //        ViewState["CurrentTableB"] = dt;
    //        //Re bind the GridView for the updated data
    //        gvDelTypeB.DataSource = dt;
    //        gvDelTypeB.DataBind();
    //        int rowIndex = 0;

    //        //Set Previous Data on Postbacks
    //        SetPreviousDataB();
    //        foreach (GridViewRow row in gvDelTypeB.Rows)
    //        {
    //            TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
    //            TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
    //            TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
    //            TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
    //            TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
    //            TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

    //            //*********************************************************************************************
    //            box0.Text = dt.Rows[rowIndex]["Service"].ToString();
    //            box1.Text = dt.Rows[rowIndex]["Location"].ToString();
    //            box2.Text = dt.Rows[rowIndex]["Frequency"].ToString();
    //            box3.Text = dt.Rows[rowIndex]["BeginningDate"].ToString();
    //            box4.Text = dt.Rows[rowIndex]["AnticipatedDuration"].ToString();
    //            box5.Text = dt.Rows[rowIndex]["AgencyResponsible"].ToString();
    //            rowIndex++;
    //        }
    //        if (dt.Rows.Count > 1)
    //        {
    //            LinkButton LinkButtonB = gvDelTypeB.FooterRow.FindControl("LinkButtonB") as LinkButton;
    //            LinkButtonB.Visible = true;
    //        }
    //    }

    //}
    //protected void LinkButtonC_Click(object sender, EventArgs e)
    //{
    //    LinkButton lb = (LinkButton)sender;
    //    GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
    //    int rowID = gvRow.RowIndex + 1;
    //    if (ViewState["PreviousTableC"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["PreviousTableC"];
    //        if (dt.Rows.Count > 1)
    //        {
    //            if (gvRow.RowIndex < dt.Rows.Count - 1)
    //            {
    //                //Remove the Selected Row data
    //                dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
    //            }
    //        }
    //        ViewState["PreviousTableC"] = dt;
    //        //Re bind the GridView for the updated data
    //        gvDelTypeC.DataSource = dt;
    //        gvDelTypeC.DataBind();
    //        int rowIndex = 0;
    //        foreach (GridViewRow row in gvDelTypeC.Rows)
    //        {
    //            TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
    //            TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
    //            TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
    //            TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
    //            TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
    //            TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

    //            //*********************************************************************************************
    //            box0.Text = dt.Rows[rowIndex]["Service"].ToString();
    //            box1.Text = dt.Rows[rowIndex]["Location"].ToString();
    //            box2.Text = dt.Rows[rowIndex]["Frequency"].ToString();
    //            box3.Text = dt.Rows[rowIndex]["BeginningDate"].ToString();
    //            box4.Text = dt.Rows[rowIndex]["AnticipatedDuration"].ToString();
    //            box5.Text = dt.Rows[rowIndex]["AgencyResponsible"].ToString();


    //            rowIndex++;
    //        }
    //        if (dt.Rows.Count > 1)
    //        {
    //            LinkButton LinkButtonC = gvDelTypeC.FooterRow.FindControl("LinkButtonC") as LinkButton;
    //            LinkButtonC.Visible = true;
    //        }
    //    }
    //    else if (ViewState["CurrentTableC"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["CurrentTableC"];
    //        if (dt.Rows.Count > 1)
    //        {
    //            if (gvRow.RowIndex < dt.Rows.Count - 1)
    //            {
    //                //Remove the Selected Row data
    //                dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
    //            }
    //        }
    //        ViewState["CurrentTableC"] = dt;
    //        //Re bind the GridView for the updated data
    //        gvDelTypeC.DataSource = dt;
    //        gvDelTypeC.DataBind();
    //        int rowIndex = 0;

    //        //Set Previous Data on Postbacks
    //        SetPreviousDataC();
    //        foreach (GridViewRow row in gvDelTypeC.Rows)
    //        {

    //            TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
    //            TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
    //            TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
    //            TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
    //            TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
    //            TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

    //            //*********************************************************************************************
    //            box0.Text = dt.Rows[rowIndex]["Service"].ToString();
    //            box1.Text = dt.Rows[rowIndex]["Location"].ToString();
    //            box2.Text = dt.Rows[rowIndex]["Frequency"].ToString();
    //            box3.Text = dt.Rows[rowIndex]["BeginningDate"].ToString();
    //            box4.Text = dt.Rows[rowIndex]["AnticipatedDuration"].ToString();
    //            box5.Text = dt.Rows[rowIndex]["AgencyResponsible"].ToString();
    //            rowIndex++;
    //        }
    //        if (dt.Rows.Count > 1)
    //        {
    //            LinkButton LinkButtonC = gvDelTypeC.FooterRow.FindControl("LinkButtonC") as LinkButton;
    //            LinkButtonC.Visible = true;
    //        }
    //    }

    //}

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
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["BeginningDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDuration"].ToString();
                    box5.Text = dt.Rows[i]["AgencyResponsible"].ToString();

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
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["BeginningDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDuration"].ToString();
                    box5.Text = dt.Rows[i]["AgencyResponsible"].ToString();


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
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["BeginningDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDuration"].ToString();
                    box5.Text = dt.Rows[i]["AgencyResponsible"].ToString();


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

                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTable.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

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
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTable.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

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

                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTableB.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
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
                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableB.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTableB.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableB.Rows.Add(drCurrentRow);
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
        int rowIndex = 0;

        if (ViewState["PreviousTableC"] != null)
        {
            DataTable dtCurrentTableC = (DataTable)ViewState["PreviousTableC"];
            if (dtCurrentTableC.Rows.Count > 4)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 5 Rows");
                return;
            }
            DataRow drCurrentRow = null;
            if (dtCurrentTableC.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTableC.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //***************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTableC.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTableC.Rows.Add(drCurrentRow);
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
                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //***************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTableC.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTableC.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }
                dtCurrentTableC.Rows.Add(drCurrentRow);
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
                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["BeginningDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDuration"].ToString();
                    box5.Text = dt.Rows[i]["AgencyResponsible"].ToString();

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
                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["BeginningDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDuration"].ToString();
                    box5.Text = dt.Rows[i]["AgencyResponsible"].ToString();


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
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");

                    //*********************************************************************************************
                    box0.Text = dt.Rows[i]["Service"].ToString();
                    box1.Text = dt.Rows[i]["Location"].ToString();
                    box2.Text = dt.Rows[i]["Frequency"].ToString();
                    box3.Text = dt.Rows[i]["BeginningDate"].ToString();
                    box4.Text = dt.Rows[i]["AnticipatedDuration"].ToString();
                    box5.Text = dt.Rows[i]["AgencyResponsible"].ToString();

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
        dt.Columns.Add("Service", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("BeginningDate", typeof(string));
        dt.Columns.Add("AnticipatedDuration", typeof(string));
        dt.Columns.Add("AgencyResponsible", typeof(string));
        //=============================================

        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Edu WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    //======================================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["Service"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString(), dr["Person"].ToString());
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
        DataTable dt = new DataTable();
        //***************************************************************************************************************

        //GoalId
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("Service", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("BeginningDate", typeof(string));
        dt.Columns.Add("AnticipatedDuration", typeof(string));
        dt.Columns.Add("AgencyResponsible", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Goal WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//============================================
                    dt.Rows.Add(dr["Id"].ToString(), dr["Service"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString(), dr["Person"].ToString());
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

        ViewState["PreviousTableB"] = dt;

        gvDelTypeB.DataSource = dt;
        gvDelTypeB.DataBind();
    }
    private void setInitialGrid3()
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        DataTable dt = new DataTable();
        //***************************************************************************************************************
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("Service", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Frequency", typeof(string));
        dt.Columns.Add("BeginningDate", typeof(string));
        dt.Columns.Add("AnticipatedDuration", typeof(string));
        dt.Columns.Add("AgencyResponsible", typeof(string));


        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getStdGoalSvcDetails = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Living WHERE StdtIEP_PEId=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getStdGoalSvcDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {//===============================
                    dt.Rows.Add(dr["Id"].ToString(), dr["Service"].ToString(), dr["Location"].ToString(), dr["Frequency"].ToString(), dr["PrjBeginning"].ToString(), dr["AnticipatedDur"].ToString(), dr["Person"].ToString());
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

        gvDelTypeC.DataSource = dt;
        gvDelTypeC.DataBind();
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        string TrainingGoal = System.Uri.UnescapeDataString(TextBoxTrainingGoal_hdn.Text);
        string TrainingCourse = System.Uri.UnescapeDataString(TextBoxTrainingCourse_hdn.Text);
        string EmploymentGoal = System.Uri.UnescapeDataString(TextBoxEmploymentGoal_hdn.Text);
        string EmploymentCourse = System.Uri.UnescapeDataString(TextBoxEmploymentCourse_hdn.Text);
        string IndependentLivingGoal = System.Uri.UnescapeDataString(TextBoxIndependentLivingGoal_hdn.Text);
        string IndependentLivingCourse = System.Uri.UnescapeDataString(TextBoxIndependentLivingCourse_hdn.Text);
        bool chkbxMeasure1 = false;
        bool chkbxMeasure2 = false;
        bool chkbxMeasure3 = false;
        if (ChkMeasure1.Checked)
        {
            chkbxMeasure1 = true;
        }
        if (ChkMeasure2.Checked)
        {
            chkbxMeasure2 = true;
        }
        if (ChkMeasure3.Checked)
        {
            chkbxMeasure3 = true;
        }
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];


            sess = (clsSession)Session["UserSession"];
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId + " ").ToString();
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
            }
            else
            {
                strQuery = "UPDATE IEP_PE_Details SET IEP6TrainingGoal='" + clsGeneral.convertQuotes(TrainingGoal) + "',IEP6TrainCoursesStudy='" + clsGeneral.convertQuotes(TrainingCourse) + "',IEP6EmploymentGoal='" + clsGeneral.convertQuotes(EmploymentGoal) + "',"
                    + "IEP6EmpCoursesStudy='" + clsGeneral.convertQuotes(EmploymentCourse) + "',IEP6LivingGoal='" + clsGeneral.convertQuotes(IndependentLivingGoal) + "',IEP6LivingCoursesStudy='" + clsGeneral.convertQuotes(IndependentLivingCourse) + "',"
                    + "IEP6MeasurableCheck1='" + chkbxMeasure1 + "',IEP6MeasurableCheck2='" + chkbxMeasure2 + "',IEP6MeasurableCheck3='" + chkbxMeasure3 + "' WHERE StdtIEP_PEId=" + sess.IEPId;
                int id = oData.ExecuteNonQuery(strQuery);
                if (id > 0)
                {
                    // tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");


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
                        //if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        //{
                        //    objData.Execute("update stdtIEPUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
                        //}
                        //else
                        //{
                        //    objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
                        //}
                        // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP6();", true);
                        //fillBasicDetails();
                        setIEPPEupdateStatus();
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
        string TrainingGoal = args1;
        string TrainingCourse = args2;
        string EmploymentGoal = args3;
        string EmploymentCourse = args4;
        string IndependentLivingGoal = args5;
        string IndependentLivingCourse = args6;
        bool chkbxMeasure1 = false;
        bool chkbxMeasure2 = false;
        bool chkbxMeasure3 = false;
        if (ChkMeasure1.Checked)
        {
            chkbxMeasure1 = true;
        }
        if (ChkMeasure2.Checked)
        {
            chkbxMeasure2 = true;
        }
        if (ChkMeasure3.Checked)
        {
            chkbxMeasure3 = true;
        }
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];


            sess = (clsSession)Session["UserSession"];
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StdtIEP_PEId=" + sess.IEPId + " ").ToString();
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
            }
            else
            {
                strQuery = "UPDATE IEP_PE_Details SET IEP6TrainingGoal='" + TrainingGoal + "',IEP6TrainCoursesStudy='" + TrainingCourse + "',IEP6EmploymentGoal='" + EmploymentGoal + "',"
                    + "IEP6EmpCoursesStudy='" + EmploymentCourse + "',IEP6LivingGoal='" + IndependentLivingGoal + "',IEP6LivingCoursesStudy='" + IndependentLivingCourse + "',"
                    + "IEP6MeasurableCheck1='" + chkbxMeasure1 + "',IEP6MeasurableCheck2='" + chkbxMeasure2 + "',IEP6MeasurableCheck3='" + chkbxMeasure3 + "' WHERE StdtIEP_PEId=" + sess.IEPId;
                int id = oData.ExecuteNonQuery(strQuery);
                if (id > 0)
                {
                    // tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");


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
                        //if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        //{
                        //    objData.Execute("update stdtIEPUpdateStatus set Page5='true' where stdtIEPId=" + sess.IEPId);
                        //}
                        //else
                        //{
                        //    objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page5) values(" + sess.IEPId + ",'true')");
                        //}
                        // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP6();", true);
                        //fillBasicDetails();
                        setIEPPEupdateStatus1();
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page6='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page6) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(7);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page6='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page6) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP7('saved');", true);
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
        try
        {
            sess = (clsSession)Session["UserSession"];

            foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
            {
                objData = new clsData();


                TextBox txtGoal = diTypeA.FindControl("txtService") as TextBox;
                TextBox txtTypeOfServiceA = diTypeA.FindControl("txtLocation") as TextBox;
                TextBox txtTypeOfPersonnelA = diTypeA.FindControl("txtFrequency") as TextBox;
                TextBox txtFrequencyA = diTypeA.FindControl("txtBeginningDate") as TextBox;
                TextBox txtStartDateA = diTypeA.FindControl("txtAnticipatedDuration") as TextBox;
                TextBox txtEndDateA = diTypeA.FindControl("txtAgencyResponsible") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {
                    if (txtGoal.Text != "" || txtTypeOfServiceA.Text != "" || txtTypeOfPersonnelA.Text != "" || txtFrequencyA.Text != "" || txtStartDateA.Text != "" || txtEndDateA.Text != "")
                    {
                        if (txtFrequencyA.Text == "")
                            txtFrequencyA.Text = (System.DateTime.Now).Date.ToString("MM/dd/yyyy");
                        insDelivery = "INSERT INTO IEP_PE6_Edu (Service,Location,Frequency,PrjBeginning,AnticipatedDur,Person,StdtIEP_PEId) " +
                                              "VALUES ('" + txtGoal.Text.Trim() + "', '" + txtTypeOfServiceA.Text.Trim() + "', '" + txtTypeOfPersonnelA.Text.Trim() + "', '" + txtFrequencyA.Text.Trim() + "', " +
                                              "'" + txtStartDateA.Text.Trim() + "', '" + txtEndDateA.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtGoal.Text != "" || txtTypeOfServiceA.Text != "" || txtTypeOfPersonnelA.Text != "" || txtFrequencyA.Text != "" || txtStartDateA.Text != "" || txtEndDateA.Text != "")
                    {
                        if (txtFrequencyA.Text == "")
                            txtFrequencyA.Text = (System.DateTime.Now).Date.ToString("MM/dd/yyyy");
                        insDelivery = "UPDATE IEP_PE6_Edu SET Service='" + txtGoal.Text.Trim() + "',Location='" + txtTypeOfServiceA.Text.Trim() + "',Frequency='" + txtTypeOfPersonnelA.Text.Trim() + "',"
                            + "PrjBeginning='" + txtFrequencyA.Text.Trim() + "',AnticipatedDur='" + txtStartDateA.Text.Trim() + "',Person='" + txtEndDateA.Text.Trim() + "'"
                            + " where Id=" + Id;

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
    protected bool SaveIEPPage5B()
    {
        bool getB = true;
        try
        {
            foreach (GridViewRow diTypeB in gvDelTypeB.Rows)
            {
                TextBox txtGoal = diTypeB.FindControl("txtService") as TextBox;
                TextBox txtTypeOfServiceA = diTypeB.FindControl("txtLocation") as TextBox;
                TextBox txtTypeOfPersonnelA = diTypeB.FindControl("txtFrequency") as TextBox;
                TextBox txtFrequencyA = diTypeB.FindControl("txtBeginningDate") as TextBox;
                TextBox txtStartDateA = diTypeB.FindControl("txtAnticipatedDuration") as TextBox;
                TextBox txtEndDateA = diTypeB.FindControl("txtAgencyResponsible") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeB.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {
                    if (txtGoal.Text != "" || txtTypeOfServiceA.Text != "" || txtTypeOfPersonnelA.Text != "" || txtFrequencyA.Text != "" || txtStartDateA.Text != "" || txtEndDateA.Text != "")
                    {
                        if (txtFrequencyA.Text == "")
                            txtFrequencyA.Text = (System.DateTime.Now).Date.ToString("MM/dd/yyyy");
                        //if (txtEndDateA.Text == "")
                        //    txtEndDateA.Text = (System.DateTime.Now).ToString();
                        insDelivery = "INSERT INTO IEP_PE6_Goal (Service,Location,Frequency,PrjBeginning,AnticipatedDur,Person,StdtIEP_PEId) " +
                                              "VALUES ('" + txtGoal.Text.Trim() + "', '" + txtTypeOfServiceA.Text.Trim() + "', '" + txtTypeOfPersonnelA.Text.Trim() + "', '" + txtFrequencyA.Text.Trim() + "', " +
                                              "'" + txtStartDateA.Text.Trim() + "', '" + txtEndDateA.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtGoal.Text != "" || txtTypeOfServiceA.Text != "" || txtTypeOfPersonnelA.Text != "" || txtFrequencyA.Text != "" || txtStartDateA.Text != "" || txtEndDateA.Text != "")
                    {
                        if (txtFrequencyA.Text == "")
                            txtFrequencyA.Text = (System.DateTime.Now).Date.ToString("MM/dd/yyyy");
                        //if (txtEndDateA.Text == "")
                        //    txtEndDateA.Text = (System.DateTime.Now).ToString();
                        insDelivery = "UPDATE IEP_PE6_Goal SET Service='" + txtGoal.Text.Trim() + "',Location='" + txtTypeOfServiceA.Text.Trim() + "',Frequency='" + txtTypeOfPersonnelA.Text.Trim() + "',"
                            + "PrjBeginning='" + txtFrequencyA.Text.Trim() + "',AnticipatedDur='" + txtStartDateA.Text.Trim() + "',Person='" + txtEndDateA.Text.Trim() + "'"
                            + " where Id=" + Id;

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
    protected bool SaveIEPPage5C()
    {
        bool getC = true;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {
            foreach (GridViewRow diTypeC in gvDelTypeC.Rows)
            {
                TextBox txtGoal = diTypeC.FindControl("txtService") as TextBox;
                TextBox txtTypeOfServiceA = diTypeC.FindControl("txtLocation") as TextBox;
                TextBox txtTypeOfPersonnelA = diTypeC.FindControl("txtFrequency") as TextBox;
                TextBox txtFrequencyA = diTypeC.FindControl("txtBeginningDate") as TextBox;
                TextBox txtStartDateA = diTypeC.FindControl("txtAnticipatedDuration") as TextBox;
                TextBox txtEndDateA = diTypeC.FindControl("txtAgencyResponsible") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeC.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
                string insDelivery = "";
                if (Id == 0)
                {//StdtIEP_PEId
                    if (txtGoal.Text != "" || txtTypeOfServiceA.Text != "" || txtTypeOfPersonnelA.Text != "" || txtFrequencyA.Text != "" || txtStartDateA.Text != "" || txtEndDateA.Text != "")
                    {
                        if (txtFrequencyA.Text == "")
                            txtFrequencyA.Text = (System.DateTime.Now).Date.ToString("MM/dd/yyyy");
                        insDelivery = "INSERT INTO IEP_PE6_Living (Service,Location,Frequency,PrjBeginning,AnticipatedDur,Person,StdtIEP_PEId) " +
                                              "VALUES ('" + txtGoal.Text.Trim() + "', '" + txtTypeOfServiceA.Text.Trim() + "', '" + txtTypeOfPersonnelA.Text.Trim() + "', '" + txtFrequencyA.Text.Trim() + "', " +
                                              "'" + txtStartDateA.Text.Trim() + "', '" + txtEndDateA.Text.Trim() + "'," + sess.IEPId + " )";
                        int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                    }
                }

                else
                {
                    if (txtGoal.Text != "" || txtTypeOfServiceA.Text != "" || txtTypeOfPersonnelA.Text != "" || txtFrequencyA.Text != "" || txtStartDateA.Text != "" || txtEndDateA.Text != "")
                    {
                        if (txtFrequencyA.Text == "")
                            txtFrequencyA.Text = (System.DateTime.Now).Date.ToString("MM/dd/yyyy");
                        insDelivery = "UPDATE IEP_PE6_Living SET Service='" + txtGoal.Text.Trim() + "',Location='" + txtTypeOfServiceA.Text.Trim() + "',Frequency='" + txtTypeOfPersonnelA.Text.Trim() + "',"
                            + "PrjBeginning='" + txtFrequencyA.Text.Trim() + "',AnticipatedDur='" + txtStartDateA.Text.Trim() + "',Person='" + txtEndDateA.Text.Trim() + "'"
                            + "where Id=" + Id;

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




    private void fillGridviews()
    {
        setInitialGrid1();
        setInitialGrid2();
        setInitialGrid3();

    }


    protected void gvDelTypeA_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvDelTypeB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //   Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId1");
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
            //      Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");
            int svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
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
                    string delRow = "delete from IEP_PE6_Edu where Id=" + StdtGoalSvcId;

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
                    string delRow = "delete from IEP_PE6_Goal where Id=" + StdtGoalSvcId;

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
                    string delRow = "delete from IEP_PE6_Living where Id=" + StdtGoalSvcId;

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

                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();
                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["Service"] = box0.Text;

                    dtCurrentTable.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTable.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


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

                    TextBox box0 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeB.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableB.NewRow();
                    //***************************************************************************************
                    dtCurrentTableB.Rows[i - 1]["Service"] = box0.Text;
                    dtCurrentTableB.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableB.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableB.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTableB.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTableB.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTableB.Rows[i - 1]["id"] = lbl_svcGoalId.Text;


                    rowIndex++;
                }

                ViewState["PreviousTableB"] = dtCurrentTableB;

                dtCurrentTableB.Rows.Remove(dtCurrentTableB.Rows[rowID]);



                gvDelTypeB.DataSource = dtCurrentTableB;
                gvDelTypeB.DataBind();
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

                    TextBox box0 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtService");
                    TextBox box1 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtLocation");
                    TextBox box2 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtFrequency");
                    TextBox box3 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtBeginningDate");
                    TextBox box4 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAnticipatedDuration");
                    TextBox box5 = (TextBox)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("txtAgencyResponsible");
                    Label lbl_svcGoalId = (Label)gvDelTypeC.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTableC.NewRow();
                    //***************************************************************************************
                    dtCurrentTableC.Rows[i - 1]["Service"] = box0.Text;
                    dtCurrentTableC.Rows[i - 1]["Location"] = box1.Text;
                    dtCurrentTableC.Rows[i - 1]["Frequency"] = box2.Text;
                    dtCurrentTableC.Rows[i - 1]["BeginningDate"] = box3.Text;
                    dtCurrentTableC.Rows[i - 1]["AnticipatedDuration"] = box4.Text;
                    dtCurrentTableC.Rows[i - 1]["AgencyResponsible"] = box5.Text;
                    dtCurrentTableC.Rows[i - 1]["id"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }

                ViewState["PreviousTableC"] = dtCurrentTableC;

                dtCurrentTableC.Rows.Remove(dtCurrentTableC.Rows[rowID]);

                gvDelTypeC.DataSource = dtCurrentTableC;
                gvDelTypeC.DataBind();
                SetPreviousDB_C();
            }
        }
    }


    protected void gvDelTypeA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    //----------------------------------------------------------------------------------------------------------------------------------------//




    [WebMethod]
    public static void submitIepPE6(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6)
    {
        args1 = arg1;
        args2 = arg2;
        args3 = arg3;
        args4 = arg4;
        args5 = arg5;
        args6 = arg6;
    }


    private void fillBasicDetails()
    {

        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        ChkMeasure1.Checked = false;
        ChkMeasure2.Checked = false;
        ChkMeasure3.Checked = false;
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

            strQuery = "select IEP6TrainingGoal,IEP6TrainCoursesStudy,IEP6EmploymentGoal,IEP6EmpCoursesStudy,IEP6LivingGoal,IEP6LivingCoursesStudy,IEP6MeasurableCheck1, "
                + "IEP6MeasurableCheck2,IEP6MeasurableCheck3 from dbo.IEP_PE_Details where StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        TextBoxEmploymentCourse.InnerHtml = Dt.Rows[0]["IEP6EmpCoursesStudy"].ToString().Trim();
                        TextBoxEmploymentCourse.InnerHtml = TextBoxEmploymentCourse.InnerHtml.Replace("##", "'");
                        TextBoxEmploymentCourse.InnerHtml = TextBoxEmploymentCourse.InnerHtml.Replace("?bs;", "\\");

                        TextBoxEmploymentCourse_hdn.Text = System.Uri.EscapeDataString(TextBoxEmploymentCourse.InnerHtml);

                        TextBoxEmploymentGoal.InnerHtml = Dt.Rows[0]["IEP6EmploymentGoal"].ToString().Trim();
                        TextBoxEmploymentGoal.InnerHtml = TextBoxEmploymentGoal.InnerHtml.Replace("##", "'");
                        TextBoxEmploymentGoal.InnerHtml = TextBoxEmploymentGoal.InnerHtml.Replace("?bs;", "\\");

                        TextBoxEmploymentGoal_hdn.Text = System.Uri.EscapeDataString(TextBoxEmploymentGoal.InnerHtml);

                        TextBoxIndependentLivingCourse.InnerHtml = Dt.Rows[0]["IEP6LivingCoursesStudy"].ToString().Trim();
                        TextBoxIndependentLivingCourse.InnerHtml = TextBoxIndependentLivingCourse.InnerHtml.Replace("##", "'");
                        TextBoxIndependentLivingCourse.InnerHtml = TextBoxIndependentLivingCourse.InnerHtml.Replace("?bs;", "\\");

                        TextBoxIndependentLivingCourse_hdn.Text = System.Uri.EscapeDataString(TextBoxIndependentLivingCourse.InnerHtml);

                        TextBoxIndependentLivingGoal.InnerHtml = Dt.Rows[0]["IEP6LivingGoal"].ToString().Trim();
                        TextBoxIndependentLivingGoal.InnerHtml = TextBoxIndependentLivingGoal.InnerHtml.Replace("##", "'");
                        TextBoxIndependentLivingGoal.InnerHtml = TextBoxIndependentLivingGoal.InnerHtml.Replace("?bs;", "\\");

                        TextBoxIndependentLivingGoal_hdn.Text = System.Uri.EscapeDataString(TextBoxIndependentLivingGoal.InnerHtml);

                        TextBoxTrainingCourse.InnerHtml = Dt.Rows[0]["IEP6TrainCoursesStudy"].ToString().Trim();
                        TextBoxTrainingCourse.InnerHtml = TextBoxTrainingCourse.InnerHtml.Replace("##", "'");
                        TextBoxTrainingCourse.InnerHtml = TextBoxTrainingCourse.InnerHtml.Replace("?bs;", "\\");

                        TextBoxTrainingCourse_hdn.Text = System.Uri.EscapeDataString(TextBoxTrainingCourse.InnerHtml);

                        TextBoxTrainingGoal.InnerHtml = Dt.Rows[0]["IEP6TrainingGoal"].ToString().Trim();
                        TextBoxTrainingGoal.InnerHtml = TextBoxTrainingGoal.InnerHtml.Replace("##", "'");
                        TextBoxTrainingGoal.InnerHtml = TextBoxTrainingGoal.InnerHtml.Replace("?bs;", "\\");

                        TextBoxTrainingGoal_hdn.Text = System.Uri.EscapeDataString(TextBoxTrainingGoal.InnerHtml);


                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck1"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck1"].ToString())))
                        {
                            ChkMeasure1.Checked = true;
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck2"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck2"].ToString())))
                        {
                            ChkMeasure2.Checked = true;
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck3"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck3"].ToString())))
                        {
                            ChkMeasure3.Checked = true;
                        }
                    }


                }
            }
            fillGridviews();
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
}