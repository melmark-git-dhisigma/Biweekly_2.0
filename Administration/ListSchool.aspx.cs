using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class Admin_ListSchool : System.Web.UI.Page
{

    DataClass objDataClass = new DataClass(); //class declaration
    public static string sub, selCommand, name;
    public static int intStdId = 0;
    clsSession sess = null;
    static bool Disable = false;
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
            if (Disable == true)
            {
                Button_Add.Visible = false;
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
                Button_Add.Visible = true;
                if (GV_Student.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in GV_Student.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                        lb_delete.Visible = true;
                    }
                }
            }
            // Bind();
            tdMsg.InnerHtml = "";
            FillGrid();



        }
        //  DataUpdated();


    }


    public void FillGrid()
    {
        try
        {
            string selSchool = "SELECT sch.SchoolId,sch.SchoolName,sch.SchoolDesc,sch.DistrictName,u.UserLName+','+u.UserFName as ModifiedUser,CASE WHEN Sch.ModifiedOn is null then convert(varchar,Sch.CreatedOn,101) ELSE  convert(varchar,Sch.ModifiedOn,101)  END AS ModifiedDate,lu.LookupName  " +
                               " from School Sch " +
                                 "   LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State) " +
                                            "  ON Sch.DistAddrId = Adr.AddressId LEFT JOIN [User] u ON u.UserId = Sch.ModifiedBy WHERE sch.ActiveInd = 'A' ORDER BY sch.SchoolId ";

            DataTable gridLesson = objDataClass.fillData(selSchool);
            GV_Student.DataSource = gridLesson;
            GV_Student.DataBind();
            linkActive.ForeColor = System.Drawing.Color.Red;
            lnkInactive.ForeColor = System.Drawing.Color.Blue;
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try After Sometime!!!!!!!!!");
            throw Ex;
        }

    }








    protected void Button_Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddSchool.aspx");
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


    protected void Button_Update_Click(object sender, EventArgs e)
    {

        //UpdateStudent();
        // Session["ClassId"] = DropDownList_Class.SelectedIndex.ToString() + "/" + TextBox_StudentName.Text.Trim();
        //Response.Redirect("AddStudent.aspx?StudentId=" + intStdId + "&Type=Update");

    }
    protected void Button_Delete_Click(object sender, EventArgs e)
    {
        // UpdateStudent();
        // Response.Redirect("AddStudent.aspx?StudentId=" + intStdId + "&Type=Delete");
    }

    protected void viewStudent(int studentID)
    {
        DataTable Dt = new DataTable();


        string strQuery = " SELECT Usr.UserLName+','+Usr.UserFName as ModifiedUser , Sch.SchoolId,Sch.SchoolName,Sch.CreatedBy,Sch.CreatedOn,Adr.AddressLine1, Adr.AddressLine2, Adr.AddressLine3, Adr.City, Adr.State, " +
                               " Adr.Country, Adr.Zip, Adr.HomePhone, Adr.Mobile, Adr.Email, Adr.CreatedBy AS Expr1, Adr.CreatedOn AS Expr2 , " +
                                    " Adr.ModifiedBy, CASE WHEN Sch.ModifiedOn is null then Sch.CreatedOn ELSE Sch.ModifiedOn END AS ModifiedDate  FROM (School Sch FULL JOIN [User] Usr ON Usr.UserId=Sch.ModifiedBy ) INNER JOIN Address Adr ON Sch.AddressId = Adr.AddressId where Sch.SchoolId = " + studentID + "";

        Dt = objDataClass.fillData(strQuery);


        try
        {


            if (Dt.Rows.Count > 0)
            {
                lblSchool.Text = Dt.Rows[0]["SchoolName"].ToString();
                lblName.Text = Dt.Rows[0]["SchoolName"].ToString().Trim();
                lblAddr.Text = Dt.Rows[0]["AddressLine1"].ToString().Trim() + "," + Dt.Rows[0]["AddressLine2"].ToString().Trim() + "," + Dt.Rows[0]["AddressLine3"].ToString().Trim();
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
                    lblZip.Text = "";
                    throw Ex;
                }
                lblCity.Text = Dt.Rows[0]["City"].ToString();
                lblZip.Text = Dt.Rows[0]["Zip"].ToString();
                lblPhone.Text = Dt.Rows[0]["HomePhone"].ToString().Trim();
                lblMobile.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                lblEmail.Text = Dt.Rows[0]["Email"].ToString().Trim();
                lblModifiedBy.Text = Dt.Rows[0]["ModifiedUser"].ToString().Trim();
                lblModifiedOn.Text = Dt.Rows[0]["ModifiedDate"].ToString().Trim();
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
        tdMsg.InnerHtml = "";
        if (e.CommandName == "View")
        {
            viewStudent(Convert.ToInt32(e.CommandArgument));
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
        }

        if (e.CommandName == "Edit")
        {
            Session["EData"] = e.CommandArgument;
            Response.Redirect("AddSchool.aspx");
        }
        if (e.CommandName == "Delete")
        {
            string delStud = "UPDATE  School SET ActiveInd = 'D' WHERE  SchoolId=" + e.CommandArgument;
            Boolean index = Convert.ToBoolean(objDataClass.ExecuteNonQuery(delStud));

            if (index == true)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("School Deleted Successfully");
                FillGrid();
            }


        }
    }
    protected void GV_Student_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        tdMsg.InnerHtml = "";
        GV_Student.PageIndex = e.NewPageIndex;

        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {

            FillInactive();

        }

        else if (linkActive.ForeColor == System.Drawing.Color.Red)
        {
            FillGrid();

        }

        else
        {

            FillGrid();
        }
        gridPermission();

    }
    protected void GV_Student_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GV_Student_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int index = 0; index < e.Row.Cells.Count; index++)
            {
                if (e.Row.Cells[index].Text.Length > 30)
                {
                    e.Row.Cells[index].Text = e.Row.Cells[index].Text.Substring(0, 30);
                }
            }
        }


        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {
            foreach (GridViewRow row in GV_Student.Rows)
            {
                ImageButton edit = (ImageButton)row.FindControl("lb_Edit");
                ImageButton delete = (ImageButton)row.FindControl("lb_delete");
                edit.Visible = false;
                delete.Visible = false;
            }
        }
        else
        {
            foreach (GridViewRow row in GV_Student.Rows)
            {
                ImageButton edit = (ImageButton)row.FindControl("lb_Edit");
                ImageButton delete = (ImageButton)row.FindControl("lb_delete");
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

            GV_Student.Columns[7].Visible = false;
            GV_Student.Columns[8].Visible = false;
        }
        else
        {
            GV_Student.Columns[7].Visible = true;
            GV_Student.Columns[8].Visible = true;

        }
    }
    protected void linkActive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "1";
        tdMsg.InnerHtml = "";
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
        FillGrid();

    }
    protected void lnkInactive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "0";
        tdMsg.InnerHtml = "";
        lnkInactive.ForeColor = System.Drawing.Color.Red;
        linkActive.ForeColor = System.Drawing.Color.Blue;
        FillInactive();
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
}