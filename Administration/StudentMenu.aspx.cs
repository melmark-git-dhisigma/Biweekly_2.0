using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Admin_StudentMenu : System.Web.UI.Page
{
    clsData objData = null;
    DataClass objDataClass = new DataClass(); //class declaration
    public static string sub, selCommand, name;
    public static int intStdId = 0;
    clsSession sess = null;
    static bool Disable = false;
    public static bool showImage = false;


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
        GV_Student.PageSize = sess.GridPagingSize;
        if (!IsPostBack)
        {
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            HdFldActiveInactive.Value = "1";
            
            // Bind();
            tdMsg.InnerHtml = "";
            FillGrid();
            string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
            if (buildName == "Local")
            {
                btnAdd.Visible = true;
                if (Disable == true)
                {
                    btnAdd.Visible = false;
                    if (GV_Student.Rows.Count > 0)
                    {
                        foreach (GridViewRow rows in GV_Student.Rows)
                        {
                            ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                            lb_delete.Visible = false;
                        }
                    }
                }
                else
                {
                    btnAdd.Visible = true;
                    if (GV_Student.Rows.Count > 0)
                    {
                        foreach (GridViewRow rows in GV_Student.Rows)
                        {
                            ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                            lb_delete.Visible = true;
                        }
                    }
                }
            }
            else
            {
                btnAdd.Visible = false;
            }


        }
        //  DataUpdated();


    }


    public void FillGrid()
    {
        try
        {
            name = clsGeneral.convertQuotes(TextBox_StudentName.Text.Trim());
            string SearchQuery = "";
            GV_Student.DataSource = null;
            string sTestCondition = "";
            if (name != "") sTestCondition = name;
            string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
            if (buildName == "Local")
            {
                SearchQuery = "SELECT stdt.StudentId,stdt.StudentNbr,stdt.StudentFname,stdt.StudentLname,stdt.MiddleName,convert(VARCHAR, stdt.ModifiedOn, 101) AS ModifiedDate,usr.UserLName+" +
                " ','+usr.UserFName AS ModifiedUser  FROM Student stdt Inner Join [User] usr on usr.UserId=stdt.CreatedBy WHERE CONCAT( stdt.StudentFName,stdt.StudentLname)" +
                " like '%'+'" + sTestCondition + "'+'%'  And stdt.ActiveInd <> 'D' order by stdt.StudentId";
            }
            else
            SearchQuery = "SELECT stdt.StudentId,stdt.StudentNbr,stdt.StudentFname,stdt.StudentLname,stdt.MiddleName,convert(VARCHAR, stdt.ModifiedOn, 101) AS ModifiedDate,usr.UserLName+" +
                " ','+usr.UserFName AS ModifiedUser  FROM Student stdt Inner Join [User] usr on usr.UserId=stdt.CreatedBy WHERE CONCAT( stdt.StudentFName,stdt.StudentLname)" +
                " like '%'+'" + sTestCondition + "'+'%'  And stdt.ActiveInd <> 'D' order by stdt.StudentId";

            GV_Student.Visible = true;
            lnkInactive.Visible = true;
            linkActive.Visible = true;
            linkActive.ForeColor = System.Drawing.Color.Red;
            lnkInactive.ForeColor = System.Drawing.Color.Blue;
            tdMsg.InnerHtml = "";
            string selClassList = SearchQuery;// "select * from Student inner join Class on Class.ClassId=Student.ClassId where Class.ClassId= " + sub + " ";
            DataTable fillTable = objDataClass.fillData(selClassList);
            DataSet myDataSet = new DataSet();
            myDataSet.Tables.Add(fillTable);
            //SetViewState(myDataSet);
            GV_Student.DataSource = fillTable;
            GV_Student.DataBind();

            if (fillTable.Rows.Count < 1)
            {
                GV_Student.Visible = true;
                lnkInactive.Visible = false;
                linkActive.Visible = false;

            }
            if (buildName == "Local")
            {
                GV_Student.Columns[6].Visible = true;
                GV_Student.Columns[7].Visible = true;
                GV_Student.Columns[9].Visible = true;
            }
            else
            {
                GV_Student.Columns[6].Visible = false;
                GV_Student.Columns[7].Visible = false;
                GV_Student.Columns[9].Visible = false;
            }
        }

        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }


    }









    protected void GV_Student_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void GV_Student_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GV_Student_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void GV_Student_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GV_Student.EditIndex = -1;
    }




    protected void Button_Search_Click(object sender, EventArgs e)
    {
        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {
            FillGridInactive();

        }

        else
        {
            FillGrid();

        }
    }
    protected void UpdateStudent()
    {
        int intRowsCount = GV_Student.Rows.Count;

        if (intRowsCount > 0)
        {

            foreach (GridViewRow rows in GV_Student.Rows)
            {
                CheckBox chkSelect = ((CheckBox)rows.FindControl("chkSelectStd"));
                if (chkSelect != null && chkSelect.Checked)
                {
                    intStdId = Convert.ToInt16(rows.Cells[1].Text);

                }
            }
        }
    }
    protected DataTable BindGrid()
    {
        objData = new clsData();
        string selectQuerry = "SELECT stdt.StudentId,stdt.StudentNbr,stdt.StudentFname,stdt.StudentLname,stdt.ModifiedOn AS ModifiedDate,usr.UserLName+ ','+" +
            "usr.UserFName AS ModifiedUser  FROM Student stdt Inner Join [User] usr on usr.UserId=stdt.CreatedBy WHERE stdt.ActiveInd <> 'D'  order by stdt.StudentLname,stdt.StudentFname";
        DataTable dtList = objData.ReturnDataTable(selectQuerry, false);
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
        tdMsg.InnerHtml = "";
        return dtList;
    }

    protected void viewStudent(int studentID)
    {
        DataTable Dt = new DataTable();

        string strQuery = "SELECT   CASE WHEN Std.ModifiedBy is null then (Select UserLName+ ','+UserFName from [User] where UserId=Std.CreatedBy) ELSE (Select UserLName+ ','+UserFName from [User] where UserId=Std.ModifiedBy) END AS ModifiedUser,Std.StudentId, Std.StudentNbr, Std.StudentFname, Std.StudentLname,Std.GradeLevel , Std.Gender, Std.JoinDt, Std.DOB,  " +
                             " Std.CreatedBy, Std.CreatedOn, Adr.AddressLine1, Adr.AddressLine2, Adr.AddressLine3, Adr.City, Adr.State,  " +
                               " Adr.Country, Adr.Zip, Adr.HomePhone, Adr.Mobile, Adr.Email, Adr.CreatedBy AS Expr1, Adr.CreatedOn AS Expr2 , " +
                                   " CASE WHEN Std.ModifiedOn is null then Std.CreatedOn ELSE Std.ModifiedOn END AS ModifiedDate,Adr.AddressId as AddId " +
                                     "  FROM (Student Std FULL JOIN [User] Usr ON Usr.UserId=Std.ModifiedBy) INNER JOIN " +
                                        " Address Adr ON Std.AddressId = Adr.AddressId " +
                                                  " where Std.StudentId= " + studentID + " ";

        Dt = objDataClass.fillData(strQuery);

        try
        {


            if (Dt.Rows.Count > 0)
            {
                //intStudentId = Convert.ToInt16(Dt.Rows[0][0]);
                lblStudentHead.Text = Dt.Rows[0]["StudentFname"].ToString();
                lblNumber1.Text = Dt.Rows[0]["StudentNbr"].ToString().Trim();
                lblStudent.Text = Dt.Rows[0]["StudentLname"].ToString().Trim() + "," + Dt.Rows[0]["StudentFname"].ToString().Trim();

                lblGender.Text = Dt.Rows[0]["Gender"].ToString();
                lblJoin.Text = Convert.ToDateTime(Dt.Rows[0]["JoinDt"]).ToString("dd/MM/yyyy").Replace("-", "/");
                lblDob.Text = Convert.ToDateTime(Dt.Rows[0]["DOB"]).ToString("dd/MM/yyyy").Replace("-", "/");
                lblAddr.Text = Dt.Rows[0]["AddressLine1"].ToString().Trim() + "," + Dt.Rows[0]["AddressLine2"].ToString().Trim() + "," + Dt.Rows[0]["AddressLine3"].ToString().Trim();

                lblCity.Text = Dt.Rows[0]["City"].ToString().Trim();

                try
                {
                    int country = Convert.ToInt32(Dt.Rows[0]["Country"]);
                    string selctCountry = "SELECT LookupId,LookupName from LookUp WHERE LookupId = " + country + " AND LookupType = 'Country'";
                    DataTable dtcountry = objDataClass.fillData(selctCountry);
                    lblCountry.Text = dtcountry.Rows[0]["LookupName"].ToString().Trim();
                    int state = Convert.ToInt32(Dt.Rows[0]["State"]);
                    string selctState = "SELECT LookupId,LookupName from LookUp WHERE LookupId = " + state + " AND LookupType = 'State'";
                    DataTable dtState = objDataClass.fillData(selctState);
                    lblState.Text = dtState.Rows[0]["LookupName"].ToString().Trim();

                }
                catch (Exception Ex)
                {
                    lblState.Text = "";
                    lblCountry.Text = "";
                   
                }
                lblZip.Text = Dt.Rows[0]["Zip"].ToString();
                lblPhone.Text = Dt.Rows[0]["HomePhone"].ToString().Trim();
                lblMobile.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                lblEmail.Text = Dt.Rows[0]["Email"].ToString().Trim();
                //intAddressId = Convert.ToInt16(Dt.Rows[0]["AddId"]);
                lblGrade.Text = Dt.Rows[0]["GradeLevel"].ToString().Trim();
                lblModifiedBy.Text = Dt.Rows[0]["ModifiedUser"].ToString().Trim();
                lblModifiedOn.Text = Dt.Rows[0]["ModifiedDate"].ToString().Trim();
            }
            else
            {
                lblStudentHead.Text = "-";
                lblNumber1.Text = "-";
                lblStudent.Text = "-";
                lblGender.Text = "-";
                lblJoin.Text = "-";
                lblDob.Text = "-";
                lblAddr.Text = "-";
                lblCity.Text = "-";
                lblCountry.Text = "-";
                lblState.Text = "-";
                lblZip.Text = "-";
                lblPhone.Text = "-";
                lblMobile.Text = "-";
                lblEmail.Text = "-";
                lblGrade.Text = "-";
                lblModifiedBy.Text = "-";
                lblModifiedOn.Text = "-";
            }
        }

        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Error Data Input!!!!!");
            throw Ex;
        }


    }

    protected void GV_Student_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        tdMsg.InnerHtml = "";
        if (e.CommandName == "View")
        {
             string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
             if (buildName == "Local")
             {

                 viewStudent(Convert.ToInt32(e.CommandArgument));
                 ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
             }
             else
             {
                 sess.StudentId = Convert.ToInt32(e.CommandArgument);
                 //sess.AdmStudentId = Convert.ToInt32(e.CommandArgument);
                 Response.Redirect("AddStudentNew.aspx");
             }

        }
        else if (e.CommandName == "IntakeVal")
        {
            try
            {
                Session["StudentIntakeId"] = e.CommandArgument;
                Response.Redirect("StudentIntakeAssessment.aspx?Permission=" + Disable);
            }
            catch (Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
                throw Ex;
            }
        }
        else if (e.CommandName == "Edit")
        {
            sess.AdmStudentId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("AddStudent.aspx");
        }
        else if (e.CommandName == "Delete")
        {
            intStdId = Convert.ToInt32(e.CommandArgument);
            string delStud = "UPDATE Student Set ActiveInd = 'D' WHERE StudentId = " + intStdId + "";
            Boolean index = Convert.ToBoolean(objDataClass.ExecuteNonQuery(delStud));

            FillGrid();
            if (index == true)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Deleted SuccessFully");
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Error : Deletion Failed");
            }


        }
    }
    protected void GV_Student_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FillGrid();
        GV_Student.PageIndex = e.NewPageIndex;
        GV_Student.DataBind();
        gridPermission();

    }
    protected DataView SortDataTable(DataTable myDataTable, bool isPageIndexChanging)
    {
        if (myDataTable != null)
        {
            DataView myDataView = new DataView(myDataTable);
            if (GridViewSortExpression != string.Empty)
            {
                if (isPageIndexChanging)
                {
                    myDataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                }
                else
                {
                    myDataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                }
            }
            return myDataView;
        }
        else
        {

            return new DataView();
        }
    }
    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
    }
    private string GridViewSortExpression
    {
        get
        {
            return ViewState["SortExpression"] as string ?? string.Empty;
        }
        set
        {
            ViewState["SortExpression"] = value;
        }
    }
    private string GridViewSortDirection
    {
        get
        {
            return ViewState["SortDirection"] as string ?? "DESC";
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }

    protected void linkActive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "1";
        FillGridActive();
        //foreach (GridViewRow row in GV_Student.Rows)
        //{
        //    LinkButton intake = (LinkButton)row.FindControl("lnkIntake");
        //    ImageButton edit = (ImageButton)row.FindControl("lnkIntake1");
        //    ImageButton delete = (ImageButton)row.FindControl("lb_delete");
        //    intake.Enabled = true;
        //    edit.Enabled = true;
        //    delete.Enabled = true;
        //}
    }
    protected void lnkInactive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "0";
        FillGridInactive();
        //foreach (GridViewRow row in GV_Student.Rows)
        //{
        //    LinkButton intake = (LinkButton)row.FindControl("lnkIntake");
        //    ImageButton edit = (ImageButton)row.FindControl("lnkIntake1");
        //    ImageButton delete = (ImageButton)row.FindControl("lb_delete");
        //    intake.Enabled = false;
        //    edit.Enabled = true;
        //    delete.Enabled = true;
        //}
    }

    protected void FillGridActive()
    {
        try
        {
            FillGrid();
        }

        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }

    }


    public void FillGridInactive()
    {
        try
        {
            linkActive.ForeColor = System.Drawing.Color.Blue;
            lnkInactive.ForeColor = System.Drawing.Color.Red;
            name = TextBox_StudentName.Text;
            string SearchQuery = "";
            GV_Student.DataSource = null;

            string sTestCondition = "";
            if (name != "") sTestCondition = name;

            SearchQuery = "SELECT stdt.StudentId,stdt.StudentNbr,stdt.StudentFname,stdt.StudentLname,stdt.MiddleName,stdt.ModifiedOn AS ModifiedDate,usr.UserLName+" +
                " ','+usr.UserFName AS ModifiedUser  FROM Student stdt Inner Join [User] usr on usr.UserId=stdt.CreatedBy WHERE CONCAT( stdt.StudentFName,stdt.StudentLname)" +
                " like '%'+'" + sTestCondition + "'+'%'  And stdt.ActiveInd <> 'A' order by stdt.StudentId";

            GV_Student.Visible = true;
            tdMsg.InnerHtml = "";
            string selClassList = SearchQuery;// "select * from Student inner join Class on Class.ClassId=Student.ClassId where Class.ClassId= " + sub + " ";
            DataTable fillTable = objDataClass.fillData(selClassList);
            DataSet myDataSet = new DataSet(); //Set your Dataset here
            myDataSet.Tables.Add(fillTable);
            SetViewState(myDataSet);
            GV_Student.DataSource = GetViewState();
            GV_Student.DataBind();

            if (fillTable.Rows.Count < 1)
            {
                GV_Student.Visible = true;

            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }
    }
    private void SetViewState(DataSet myDataSet)
    {
        ViewState["myDataSet"] = myDataSet;
    }
    protected void GV_Student_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {
            foreach (GridViewRow row in GV_Student.Rows)
            {
                LinkButton intake = (LinkButton)row.FindControl("lnkIntake");
                ImageButton edit = (ImageButton)row.FindControl("lnkIntake1");
                ImageButton delete = (ImageButton)row.FindControl("lb_delete");
                intake.Visible = false;
                edit.Visible = false;
                delete.Visible = false;

            }
        }
        else
        {
            foreach (GridViewRow row in GV_Student.Rows)
            {
                LinkButton intake = (LinkButton)row.FindControl("lnkIntake");
                ImageButton edit = (ImageButton)row.FindControl("lnkIntake1");
                ImageButton delete = (ImageButton)row.FindControl("lb_delete");
                intake.Visible = true;
                edit.Visible = true;
                delete.Visible = true;

            }
        }


        gridPermission();
    }

    private void gridPermission()
    {
        if (Disable == true)
        {

            GV_Student.Columns[6].Visible = false;
            GV_Student.Columns[8].Visible = false;
        }
        else
        {
            GV_Student.Columns[6].Visible = true;
            GV_Student.Columns[8].Visible = true;

        }
    }
    protected void GV_Student_Sorting(object sender, GridViewSortEventArgs e)
    {
        showImage = true;
        DataSet myDataSet = GetViewState();
        DataTable myDataTable = myDataSet.Tables[0];
        GridViewSortExpression = e.SortExpression;

        int iPageIndex = GV_Student.PageIndex;
        GV_Student.DataSource = SortDataTable(myDataTable, false);
        GV_Student.DataBind();
        GV_Student.PageIndex = iPageIndex;

    }

    protected int GetSortColumnIndex()
    {
        foreach (DataControlField field in GV_Student.Columns)
        {
            if (field.SortExpression == GridViewSortExpression)
            {
                return GV_Student.Columns.IndexOf(field);
            }
        }
        return -1;
    }
    protected void GV_Student_RowCreated(object sender, GridViewRowEventArgs e)
    {
        int sortColumnIndex = 0;

        if (e.Row.RowType == DataControlRowType.Header)
            sortColumnIndex = GetSortColumnIndex();

        if (sortColumnIndex != -1)
        {
            AddSortImage(sortColumnIndex, e.Row);
        }
    }
    protected void ddCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        {
            GridViewRow myGridViewRow = GV_Student.BottomPagerRow;
            DropDownList ddCurrentPage = (DropDownList)myGridViewRow.Cells[0].FindControl("ddCurrentPage");

            GV_Student.PageIndex = ddCurrentPage.SelectedIndex;


            DataSet myDataSet = GetViewState();
            DataTable myDataTable = myDataSet.Tables[0];

            GV_Student.DataSource = SortDataTable(myDataTable, true);
            GV_Student.DataBind();
        }
    }
    protected void AddSortImage(int columnIndex, GridViewRow HeaderRow)
    {
        Image sortImage = new Image();

        if (showImage)
        {
            if (ViewState["SortDirection"] != null)
            {
                if (ViewState["SortDirection"].ToString() == "DESC")
                {
                    sortImage.ImageUrl = "images/up.png";
                    sortImage.AlternateText = " Ascending Order";
                }

                else
                {
                    sortImage.ImageUrl = "images/down.png";
                    sortImage.AlternateText = " Descending Order";
                }
            }
            if (columnIndex == -1) columnIndex = 0;
            HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
        }

    }
    protected void GV_Student_DataBound(object sender, EventArgs e)
    {
        GridViewRow myGridViewRow = GV_Student.BottomPagerRow;

        if (myGridViewRow == null) return;

        DropDownList ddCurrentPage = (DropDownList)myGridViewRow.Cells[0].FindControl("ddCurrentPage");
        Label lblTotalPage = (Label)myGridViewRow.Cells[0].FindControl("lblTotalPage");

        if (ddCurrentPage != null)
        {

            for (int i = 0; i < GV_Student.PageCount; i++)
            {
                int iPageNumber = i + 1;
                ListItem myListItem = new ListItem(iPageNumber.ToString());

                if (i == GV_Student.PageIndex)
                    myListItem.Selected = true;

                ddCurrentPage.Items.Add(myListItem);
            }
        }


        if (lblTotalPage != null)
            lblTotalPage.Text = GV_Student.PageCount.ToString();
    }
    protected void imgPageFirst_Command(object sender, CommandEventArgs e)
    {
        Paginate(sender, e);
    }
    protected void imgPagePrevious_Command(object sender, CommandEventArgs e)
    {
        Paginate(sender, e);
    }
    protected void imgPageNext_Command(object sender, CommandEventArgs e)
    {
        Paginate(sender, e);
    }
    protected void imgPageLast_Command(object sender, CommandEventArgs e)
    {
        Paginate(sender, e);
    }

    protected void Paginate(object sender, CommandEventArgs e)
    {

        int iCurrentIndex = GV_Student.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                GV_Student.PageIndex = 0;
                break;
            case "prev":
                if (GV_Student.PageIndex != 0)
                {
                    GV_Student.PageIndex = iCurrentIndex - 1;
                }
                break;
            case "next":
                GV_Student.PageIndex = iCurrentIndex + 1;
                break;
            case "last":
                GV_Student.PageIndex = GV_Student.PageCount;
                break;
        }


        DataSet myDataSet = GetViewState();
        DataTable myDataTable = myDataSet.Tables[0];

        GV_Student.DataSource = SortDataTable(myDataTable, true);
        GV_Student.DataBind();
    }
    private DataSet GetViewState()
    {
        return (DataSet)ViewState["myDataSet"];
    }

    protected void GV_Student_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {

    }





    protected void FillInactive()
    {
        try
        {
            string selSchool = "SELECT sch.SchoolId,sch.SchoolName,sch.SchoolDesc,sch.DistrictName,u.UserLName+','+u.UserFName as ModifiedUser,CASE WHEN Sch.ModifiedOn is null then Sch.CreatedOn ELSE Sch.ModifiedOn END AS ModifiedDate,lu.LookupName  " +
                               " from School Sch " +
                                 "   INNER JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State) " +
                                            "  ON Sch.DistAddrId = Adr.AddressId LEFT JOIN [User] u ON u.UserId = Sch.ModifiedBy WHERE sch.ActiveInd = 'D' ORDER BY sch.SchoolId ";

            DataTable gridLesson = objDataClass.fillData(selSchool);
            GV_Student.DataSource = gridLesson;
            GV_Student.DataBind();
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try After Sometime!!!!!!!!!");
            throw Ex;
        }


    }



    protected void btnAdd_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        sess.AdmStudentId = 0;
        Response.Redirect("AddStudent.aspx");
    }
}