using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class StudentBinder_ReviewAssessmnt : System.Web.UI.Page
{
    DataClass oClass = new DataClass();
    clsSession oSession = null;
    static bool Disable = false;
  

    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }
    private string _sortDirection;
    protected void Page_Load(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
        if (oSession == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(oSession.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }

        if (Page.IsPostBack == false)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        fillDrop();
        fillGrid();

        if (oSession.StudentId != 0)
        {
            divPanel.Visible = true;
            fillGrid();
        }
        else
        {
            divPanel.Visible = false;
        }


        setVisible();

        if (Request.QueryString["studid"] != null)
        {
            int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
            oSession.StudentId = studid;
        }
    }
    private void setVisible()
    {
        //Query String From Form Assess : Prior Assessments
        if (Request.QueryString["Edit"] != null)
        {
            setVisibleEdit(true);
        }
        else
        {
            setVisibleEdit(false);
        }
        /////////////////////////////////////

        clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
        if (Disable == true)
        {
            setVisibleEdit(false);

        }
        else
        {
            if (Request.QueryString["Edit"] != null)
            {
                setVisibleEdit(true);
            }
        }
    }

    private void setVisibleEdit(bool Val)
    {
        if (grd_ReviewAssess.Rows.Count > 0)
        {
            bool values = Val;
            foreach (GridViewRow rows in grd_ReviewAssess.Rows)
            {
                string status = rows.Cells[4].Text;
                if (status.Trim() == "Complete") Val = false;
                ImageButton lb_update = ((ImageButton)rows.FindControl("imgbtnUpdate"));
                lb_update.Visible = Val;

                ImageButton lb_view = ((ImageButton)rows.FindControl("imgbtnView"));
                lb_view.Visible = Val;

                Val = values;
            }
        }
    }


    protected void fillDrop()
    {
        // Fill the Students Name and ID into the Dropdownlist
        DataClass oData = new DataClass();
        //this.ddl_Student.DataSource = oData.fillData("SELECT StudentId, StudentLname" + "+','+" + "StudentFname as StudentName FROM Student where ActiveInd<>'D' ORDER BY StudentFname");
        //this.ddl_Student.DataTextField = "StudentName";
        //this.ddl_Student.DataValueField = "StudentId";
        //this.ddl_Student.DataBind();
        //this.ddl_Student.Items.Insert(0, new ListItem("-- Select Student --", "0"));

        this.ddl_Year.DataSource = oData.fillData("SELECT AsmntYearId, AsmntYearCode FROM AsmntYear");
        this.ddl_Year.DataTextField = "AsmntYearCode";
        this.ddl_Year.DataValueField = "AsmntYearId";
        this.ddl_Year.DataBind();
        this.ddl_Year.Items.Insert(0, new ListItem("-- All Year --", "0"));
        ddl_Year.SelectedValue = oData.ExecuteScalarString("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");

        ddlStatus.DataSource = oData.fillData("SELECT LookupName,LookupId FROM LookUp WHERE LookupType='Assessment Status'");
        ddlStatus.DataTextField = "LookupName";
        ddlStatus.DataValueField = "LookupId";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-- All Status --", "0"));
        //string actYear = oData.ExecuteScalarString("SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='Y'");
        //lblYear.Text = actYear;
    }
    /// <summary>
    /// Function used to fill data into the gridview
    /// </summary>
    protected void fillGrid()
    {
        string selStdtAssmnt = "";

        if (ddlStatus.SelectedValue == "0")
            selStdtAssmnt = "SELECT AsmntId,AsmntName,AsmntTemplateName,AsmntTyp,Usr.UserLname+','+Usr.UserFname as Username,Asmnt.ModifiedOn,AsmntStatusId FROM (Assessment Asmnt INNER JOIN [User] Usr ON Usr.UserId=Asmnt.ModifiedBy) WHERE StudentId=" + oSession.StudentId + " AND AsmntYearId='" + ddl_Year.SelectedValue + "' Order By Asmnt.ModifiedOn DESC";
        else
            selStdtAssmnt = "SELECT AsmntId,AsmntName,AsmntTemplateName,AsmntTyp,Usr.UserLname+','+Usr.UserFname as Username,Asmnt.ModifiedOn,AsmntStatusId FROM (Assessment Asmnt INNER JOIN [User] Usr ON Usr.UserId=Asmnt.ModifiedBy) WHERE StudentId=" + oSession.StudentId + " AND AsmntYearId='" + ddl_Year.SelectedValue + "' AND AsmntStatusId='" + ddlStatus.SelectedValue + "' Order By Asmnt.ModifiedOn DESC ";
        oClass.Connect();


        DataTable dtStdtAsmnt = oClass.fillData(selStdtAssmnt);
        grd_ReviewAssess.DataSource = dtStdtAsmnt;
        grd_ReviewAssess.DataBind();

        oClass.CloseConnection();
    }
    /// <summary>
    /// Assessment updation..when user click update..it redirects the user to its corresponding page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Update_Click(object sender, EventArgs e)
    {
        //if (ddl_Student.SelectedIndex != 0)
        //{
        //    int validate = 0, asmntID = 0;
        //    string assessType = "", assessTempName = "";
        //    foreach (GridViewRow grSelected in grd_ReviewAssess.Rows)
        //    {
        //        CheckBox chkSelect = (CheckBox)grSelected.FindControl("chk_Select");
        //        if (chkSelect.Checked == true)
        //        {
        //            HiddenField hfAsmntID = (HiddenField)grSelected.FindControl("hfAssmntID");
        //            asmntID = Convert.ToInt32(hfAsmntID.Value);
        //            validate = 1;
        //            assessType = grSelected.Cells[3].Text;
        //            assessTempName = grSelected.Cells[1].Text;
        //        }
        //    }
        //    if (validate == 0)  // it works if the user does not select any assessment...
        //    {
        //        //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Select any Assessment');", true);
        //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Select any Assessment');", true);
        //    }
        //    else
        //    {
        //        oSession = (clsSession)Session["UserSession"];
        //        Session["goalname"] = assessTempName;
        //        if (oSession != null)
        //            oSession.StudentId = Convert.ToInt32(ddl_Student.SelectedValue);
        //        //Session["studID"] = ddl_Student.SelectedValue;
        //        Session["Mode"] = "Edit";
        //        Session["stdtAsmntID"] = asmntID;
        //        Session["skill"] = "All";
        //        Session["xmlname"] = asmntID;
        //        if (assessType == "By Skill")
        //            Response.Redirect("GoalAssess.aspx");
        //        else
        //            Response.Redirect("FormAssess.aspx");
        //    }
        //}
        //else
        //{
        //    lbl_Msg.InnerHtml = clsGeneral.warningMsg("Please Select Student");
        //}
    }
    protected void grdTrades_PageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        grd_ReviewAssess.PageIndex = e.NewPageIndex;
        fillGrid();
    }
    protected void lbSelect_OnClick(object sender, EventArgs e)
    {

    }
    protected void grd_ReviewAssess_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void selectedRow()
    {
    }
    protected void grd_ReviewAssess_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void ddl_Year_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grd_ReviewAssess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (grd_ReviewAssess.SortExpression == "")
            ViewState["SortExpression"] = "AsmntName";
        
        if (Disable == true)
        {
            grd_ReviewAssess.Columns[3].Visible = false;
            grd_ReviewAssess.Columns[7].Visible = false;
            grd_ReviewAssess.Columns[8].Visible = false;
        }
        else
        {
            grd_ReviewAssess.Columns[3].Visible = true;
            grd_ReviewAssess.Columns[7].Visible = true;
            grd_ReviewAssess.Columns[8].Visible = true;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataClass oData = new DataClass();
            int YearID = oData.ExecuteScalar("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
            string status = oData.ExecuteScalarString("SELECT LookupName FROM LookUp WHERE LookupType='Assessment Status' AND LookupId=" + e.Row.Cells[4].Text);
            e.Row.Cells[4].Text = status;
            CheckBox includIEP = (CheckBox)e.Row.FindControl("chk_IncScr");
            //CheckBox chkSel = (CheckBox)e.Row.FindControl("chk_Select");
            ImageButton imgView = (ImageButton)e.Row.FindControl("imgbtnView");
            ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgbtnUpdate");
            if (e.Row.Cells[4].Text != "Complete")
            {
                includIEP.Enabled = false;
                ImageButton imgPrvw = (ImageButton)e.Row.FindControl("imgbtnPreview");
                imgPrvw.Visible = false;
            }
            else
            {
                //chkSel.Enabled = false;
                imgUpdate.Visible = false;
            }
            if (e.Row.Cells[2].Text == "By Assessment")
            {
                string assess = e.Row.Cells[0].Text;
                if (e.Row.Cells[4].Text != "Complete")
                {
                    imgView.Visible = true;
                }

            }
            if (YearID.ToString() != ddl_Year.SelectedValue)    //If the Assessmnt Year is not the Current Year, then disavle all Edit buttons...
            {
                includIEP.Enabled = false;
                //chkSel.Enabled = false;
                imgView.Visible = false;
                imgUpdate.Visible = false;
            }
        }
    }

   
    protected void grd_ReviewAssess_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Sort")
        {
            ImageButton imgbtn = (ImageButton)e.CommandSource;

            if (e.CommandName == "View")
            {
                Response.Redirect("AssessmentSkills.aspx?AsmntId=" + imgbtn.CommandArgument + "&Value=" + e.CommandArgument + "");
            }
            if (e.CommandName == "Preview")
            {
                Response.Redirect("AssessmentPreview.aspx?AssessmentId=" + e.CommandArgument + "");
            }
            if (e.CommandName == "Update")
            {
                ImageButton imgUpd = (ImageButton)e.CommandSource;
                GridViewRow grAsmntRow = (GridViewRow)imgUpd.NamingContainer;
                int asmntID = 0;
                string assessType = "", assessTempName = "";

                asmntID = Convert.ToInt32(e.CommandArgument);
                assessType = grAsmntRow.Cells[2].Text;
                assessTempName = grAsmntRow.Cells[0].Text;

                oSession = (clsSession)Session["UserSession"];
                Session["goalname"] = assessTempName;
                DataClass oData = new DataClass();

                Session["Asmnt_ModDate"] = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + asmntID);   //Add the last modified date into a session before editing.....
                //if (oSession != null)
                //    oSession.StudentId = Convert.ToInt32(ddl_Student.SelectedValue);
                //Session["studID"] = ddl_Student.SelectedValue;
                Session["Mode"] = "Edit";
                Session["stdtAsmntID"] = asmntID;
                Session["skill"] = "All";
                Session["xmlname"] = asmntID;
                if (assessType == "By Skill")
                    Response.Redirect("AssessmentBySkill.aspx");
                else
                    Response.Redirect("AssessmentByType.aspx");

            }
        }

    }
    protected void ddl_Year_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (Convert.ToInt32(oSession.StudentId) != 0)
        {
            divPanel.Visible = true;
            fillGrid();
        }
    }
    protected void btn_GenerateIEP_Click(object sender, EventArgs e)
    {
        Session["selYear"] = ddl_Year.SelectedItem.Text;
        string IDs = "";
        int validate = 0;
        Hashtable htAsmnts = new Hashtable();
        foreach (GridViewRow grReview in grd_ReviewAssess.Rows)
        {
            CheckBox chkIEP = (CheckBox)grReview.FindControl("chk_IncScr");
            if (chkIEP.Checked == true)
            {
                HiddenField hfStdAsmntID = (HiddenField)grReview.FindControl("hfAssmntID");
                string asmntID = "AssessmentId=" + hfStdAsmntID.Value + " OR ";
                IDs = IDs + asmntID;
                validate = 1;
            }
        }
        if (validate == 0)
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Select any completed Assessment');", true);
        else
        {
            IDs = IDs.Substring(0, IDs.Length - 4);

            Session["asmntIDs"] = IDs;

            Response.Redirect("IEPGeneration.aspx");
        }
    }
    protected void btn_manualIEP_Click(object sender, EventArgs e)
    {
        Session["selYear"] = ddl_Year.SelectedItem.Text;
        Session["asmntIDs"] = null;
        string IDs = "";
        int validate = 0;
        foreach (GridViewRow grReview in grd_ReviewAssess.Rows)
        {
            CheckBox chkIEP = (CheckBox)grReview.FindControl("chk_IncScr");
            if (chkIEP.Checked == true)
            {
                HiddenField hfStdAsmntID = (HiddenField)grReview.FindControl("hfAssmntID");
                string asmntID = "AssessmentId=" + hfStdAsmntID.Value + " OR ";
                IDs = IDs + asmntID;
                validate = 1;
            }
        }
        if (validate == 0)
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Select any completed Assessment');", true);
        else
        {
            IDs = IDs.Substring(0, IDs.Length - 4);
            Session["asmntIDs"] = IDs;
            oSession = (clsSession)Session["UserSession"];
            //if (oSession != null)
            //    oSession.StudentId = Convert.ToInt32(ddl_Student.SelectedValue);
            //Session["studID"] = ddl_Student.SelectedValue;
            Response.Redirect("ManualIEPGenerate.aspx");
        }
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(oSession.StudentId) != 0)
        {
            divPanel.Visible = true;
            fillGrid();
            setVisible();
        }
    }
    protected void dlSkillView_ItemDataBound(object sender, DataListItemEventArgs e)
    {


    }
    protected void dlSkillView_ItemCommand(object source, DataListCommandEventArgs e)
    {
        HiddenField hfAsmntID = (HiddenField)e.Item.FindControl("hfAsmntID");
        int asmntID = Convert.ToInt32(hfAsmntID.Value);
        //oSession = (clsSession)Session["UserSession"];
        //if (oSession != null)
        //    oSession.StudentId = Convert.ToInt32(ddl_Student.SelectedValue);

        Session["skill"] = e.CommandArgument;
        Session["xmlname"] = asmntID;
        Response.Redirect("AssessmentByType.aspx");
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }


    protected void grd_ReviewAssess_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadData();
        SetSortDirection(SortDireaction);
        DataTable dataTable = grd_ReviewAssess.DataSource as DataTable;
        if (dataTable != null)
        {
            //Sort the data.
            dataTable.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            grd_ReviewAssess.DataSource = dataTable;
            grd_ReviewAssess.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grd_ReviewAssess.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grd_ReviewAssess.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }

            setVisible();
        }
    }

    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";
            

        }
        else
        {
            _sortDirection = "ASC";
            
        }
    }

}