using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_UserList : System.Web.UI.Page
{

    public string roles = "";
    public bool IsDeleted = false;
    clsSession sess = null;
    clsData objData = null;
    DataClass objDataClass = null;
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
        grd_Users.PageSize = sess.GridPagingSize;
        if (!IsPostBack)
        {
            HdFldActiveInactive.Value = "1";
            fillGrid();
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            gridPermission();
            //if (Disable == true)
            //{
            //    if (grd_Users.Rows.Count > 0)
            //    {
            //        foreach (GridViewRow rows in grd_Users.Rows)
            //        {
            //            ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
            //            lb_delete.Visible = false;
            //            ImageButton edit = (ImageButton)rows.FindControl("lb_Edit");
            //            edit.Visible = false;
            //        }
            //    }

            //}
            //else
            //{
            //    if (grd_Users.Rows.Count > 0)
            //    {
            //        foreach (GridViewRow rows in grd_Users.Rows)
            //        {
            //            ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
            //            lb_delete.Visible = true;
            //            ImageButton edit = (ImageButton)rows.FindControl("lb_Edit");
            //            edit.Visible = true;
            //        }
            //    }
            //}


        }
    }
    protected void fillGrid()
    {
        DataClass oData = new DataClass();
        this.grd_Users.DataSource = oData.fillData("  SELECT U.UserId,U.UserNo as [UserName],U.UserLName+','+U.UserFName as Name " +
                                                     "  ,U.UserFName,CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END ModId " +
                                                     "  ,CASE WHEN U.ModifiedOn is null then U.CreatedOn ELSE U.ModifiedOn " +
                                                    "      END AS ModifiedDate   " +
                                                   "  ,Modu.UserLName+','+Modu.UserFName As ModBy " +
                                                  "  FROM [User] U, [User] ModU   " +
                                                  "  WHERE       CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END = ModU.UserId AND U.ActiveInd = 'A' ORDER BY U.UserNo  ");
        this.grd_Users.DataBind();
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
    }




    protected void grd_Users_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
            Session["StatusUser"] = "Inactive";

        if (e.CommandName == "Edit")
        {
            try
            {
                tdMsg.InnerHtml = "";
                Session["UserEdit"] = e.CommandArgument;
                Response.Redirect("UserCreate.aspx");
            }
            catch (Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
                throw Ex;
            }
        }

        if (e.CommandName == "View")
        {
            try
            {
                tdMsg.InnerHtml = "";
                viewStudent(Convert.ToInt32(e.CommandArgument));
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
            }
            catch (Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
                throw Ex;
            }
        }

        if (e.CommandName == "Delete")
        {
            try
            {
                tdMsg.InnerHtml = "";
                objData = new clsData();
                int id = Convert.ToInt32(e.CommandArgument);
                string deltUser = "Update [User] Set ActiveInd ='D' WHERE UserId = " + id + "";
                Boolean retVal = Convert.ToBoolean(objData.Execute(deltUser));
                string delFrmUserRlGrp = "Update [UserRoleGroup] Set ActiveInd ='D' WHERE UserId = " + id + "";
                Boolean retValRlGrp = Convert.ToBoolean(objData.Execute(delFrmUserRlGrp));
                tdMsg.InnerHtml = clsGeneral.sucessMsg("User Deleted Successfully");
                fillGrid();

            }
            catch (Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
                throw Ex;
            }


        }


    }


    protected void viewStudent(int studentID)
    {
        clsUser oUser = new clsUser();
        objData = new clsData();
        objDataClass = new DataClass();
        DataTable Dt = new DataTable();


        string strQuery = " SELECT U.UserId,U.UserNo,U.UserInitial,U.UserFName,U.UserLName,U.Gender,U.UserNo as [UserName],U.UserLName+','+U.UserFName as Name  " +
                               "  ,U.UserFName,CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END ModId " +
                                    " ,CASE WHEN U.ModifiedOn is null then U.CreatedOn ELSE U.ModifiedOn  " +
                                           "    END AS ModifiedDate   " +
                                           "     ,Modu.UserLName+','+Modu.UserFName As ModBy, " +
                                                  "   Adr.AddressLine1+ ','+ Adr.AddressLine2 +' ,'+ Adr.AddressLine3 as Address, Adr.City, Adr.State,Adr.Mobile, Adr.Email,Adr.Country ,Adr.Zip, Adr.HomePhone " +
                                                  " FROM ([User] U LEFT JOIN [Address] Adr ON U.AddressId = Adr.AddressId), [User] ModU   " +
                                                     "WHERE       CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END = ModU.UserId AND U.ActiveInd = 'A' AND  U.UserId = " + studentID + " " +
                                                        " ORDER BY U.UserFName  ";


        Dt = objDataClass.fillData(strQuery);

        try
        {
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblClass.Text = Convert.ToString(Dt.Rows[0]["UserNo"]);
                    lblNumber.Text = Convert.ToString(Dt.Rows[0]["UserInitial"]);
                    lblName.Text = Convert.ToString(Dt.Rows[0]["UserLName"]) + "," + Convert.ToString(Dt.Rows[0]["UserFName"]);
                    lblGender.Text = Convert.ToString(Dt.Rows[0]["Gender"]);
                    lblAddr.Text = Convert.ToString(Dt.Rows[0]["Address"]);
                    lblCity.Text = Convert.ToString(Dt.Rows[0]["City"]);
                    try
                    {
                        int country = Convert.ToInt32(Dt.Rows[0]["Country"]);
                        if (country > 0)
                        {
                            string selctCountry = "SELECT LookupId,LookupName from LookUp WHERE LookupId = " + country + " AND LookupType = 'Country'";
                            DataTable dtcountry = objDataClass.fillData(selctCountry);
                            lblCountry.Text = dtcountry.Rows[0]["LookupName"].ToString().Trim();
                        }
                        else
                            lblCountry.Text = "";
                        int state = Convert.ToInt32(Dt.Rows[0]["State"]);
                        if (state > 0)
                        {
                            string selctState = "SELECT LookupId,LookupName from LookUp WHERE LookupId = " + state + " AND LookupType = 'State'";
                            DataTable dtState = objDataClass.fillData(selctState);
                            lblState.Text = dtState.Rows[0]["LookupName"].ToString().Trim();
                        }
                        else
                            lblState.Text = "";
                    }
                    catch (Exception Ex)
                    {
                        lblCountry.Text = "";
                        lblState.Text = "";
                        throw Ex;
                    }
                    lblZip.Text = Dt.Rows[0]["Zip"].ToString();
                    lblPhone.Text = Dt.Rows[0]["HomePhone"].ToString().Trim();
                    lblMobile.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                    lblEmail.Text = Dt.Rows[0]["Email"].ToString().Trim();
                    lblModifiedBy.Text = Dt.Rows[0]["ModBy"].ToString().Trim();
                    lblModifiedOn.Text = Dt.Rows[0]["ModifiedDate"].ToString().Trim();

                }
            }
        }

        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg(" Input Data Error.....!!!!!!");
            throw Ex;

        }




    }
    protected void grd_Users_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {
            foreach (GridViewRow row in grd_Users.Rows)
            {
                ImageButton edit = (ImageButton)row.FindControl("lb_Edit");
                ImageButton delete = (ImageButton)row.FindControl("lb_delete");
                edit.Visible = false;
                delete.Visible = false;
            }
        }
        else
        {
            foreach (GridViewRow row in grd_Users.Rows)
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

            grd_Users.Columns[5].Visible = false;
            grd_Users.Columns[6].Visible = false;
        }
        else
        {
            grd_Users.Columns[5].Visible = true;
            grd_Users.Columns[6].Visible = true;

        }
    }

    protected void grd_Users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_Users.PageIndex = e.NewPageIndex;


        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {

            fillGridInActive();
        }

        else if (linkActive.ForeColor == System.Drawing.Color.Red)
        {
            fillGrid();
        }

        else
        {
            fillGrid();
        }
        gridPermission();

    }

    protected void linkActive_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void lnkInactive_Click(object sender, ImageClickEventArgs e)
    {

    }


    protected void fillGridActive()
    {
        tdMsg.InnerHtml = "";
        DataClass oData = new DataClass();
        this.grd_Users.DataSource = oData.fillData("  SELECT U.UserId,U.UserNo as [UserName],U.UserLName+','+U.UserFName as Name " +
                                                     "  ,U.UserFName,CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END ModId " +
                                                     "  ,CASE WHEN U.ModifiedOn is null then U.CreatedOn ELSE U.ModifiedOn " +
                                                    "      END AS ModifiedDate   " +
                                                   "  ,Modu.UserLName+','+Modu.UserFName As ModBy " +
                                                  "  FROM [User] U, [User] ModU   " +
                                                  "  WHERE       CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END = ModU.UserId AND U.ActiveInd = 'A' ORDER BY UserFName  ");
        this.grd_Users.DataBind();
    }


    protected void fillGridInActive()
    {
        tdMsg.InnerHtml = "";
        DataClass oData = new DataClass();
        this.grd_Users.DataSource = oData.fillData("  SELECT U.UserId,U.UserNo as [UserName],U.UserLName+','+U.UserFName as Name " +
                                                     "  ,U.UserFName,CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END ModId " +
                                                     "  ,CASE WHEN U.ModifiedOn is null then U.CreatedOn ELSE U.ModifiedOn " +
                                                    "      END AS ModifiedDate   " +
                                                   "  ,Modu.UserLName+','+Modu.UserFName As ModBy " +
                                                  "  FROM [User] U, [User] ModU   " +
                                                  "  WHERE       CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END = ModU.UserId AND U.ActiveInd = 'D' ORDER BY UserFName  ");
        this.grd_Users.DataBind();
    }

    protected void linkActive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "1";
        ViewState["activeStatus"] = true;
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
        fillGridActive();
    }

    protected void lnkInactive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "0";
        ViewState["activeStatus"] = false;
        lnkInactive.ForeColor = System.Drawing.Color.Red;
        linkActive.ForeColor = System.Drawing.Color.Blue;
        fillGridInActive();
    }
    protected void grd_Users_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void Button_Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserCreate.aspx");
    }
    protected void Button_Search_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";

        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        {
            FillInActiveList();
        }
        else
        {
            FillActiveList();
        }
    }



    protected void FillActiveList()
    {
        DataClass oData = new DataClass();
        this.grd_Users.DataSource = oData.fillData("  SELECT U.UserId,U.UserNo as [UserName],U.UserLName+','+U.UserFName as Name " +
                                                     "  ,U.UserFName,CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END ModId " +
                                                     "  ,CASE WHEN U.ModifiedOn is null then U.CreatedOn ELSE U.ModifiedOn " +
                                                    "      END AS ModifiedDate   " +
                                                   "  ,Modu.UserLName+','+Modu.UserFName As ModBy " +
                                                  "  FROM [User] U, [User] ModU   " +
                                                  "  WHERE  CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END = ModU.UserId AND U.ActiveInd = 'A' And (U.UserLName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%' OR U.UserFName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%' OR U.UserLName+','+U.UserFName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%' OR U.UserFName+','+U.UserLName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%')  ORDER BY UserFName  ");
        this.grd_Users.DataBind();
    }

    protected void FillInActiveList()
    {
        DataClass oData = new DataClass();
        this.grd_Users.DataSource = oData.fillData("  SELECT U.UserId,U.UserNo as [UserName],U.UserLName+','+U.UserFName as Name " +
                                                     "  ,U.UserFName,CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END ModId " +
                                                     "  ,CASE WHEN U.ModifiedOn is null then U.CreatedOn ELSE U.ModifiedOn " +
                                                    "      END AS ModifiedDate   " +
                                                   "  ,Modu.UserLName+','+Modu.UserFName As ModBy " +
                                                  "  FROM [User] U, [User] ModU   " +
                                                  "  WHERE  CASE WHEN U.ModifiedBy IS NULL Then U.CreatedBy ELSE U.ModifiedBy END = ModU.UserId AND U.ActiveInd  = 'D' And (U.UserLName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%' OR U.UserFName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%' OR U.UserLName+','+U.UserFName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%' OR U.UserFName+','+U.UserLName like '%'+'" + TextBox_StudentName.Text.Trim() + "'+'%')  ORDER BY UserFName  ");
        this.grd_Users.DataBind();
    }
}