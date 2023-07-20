using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Admin_AddGroup : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    int GroupId = 0;
    string strQuery = "";
    bool retVal = false;
    int retValsave = 0;
    bool Disable = false;
    int GroupIds = 0;

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
        grdGroup.PageSize = 10;
        if (!IsPostBack)
        {
            LoadGroup();
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
                btnNext.Visible = false;
                if (grdGroup.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdGroup.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                        lb_delete.Visible = false;
                    }
                }
            }
            else
            {
                btnSave.Visible = true;
                btnNext.Visible = true;
                if (grdGroup.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdGroup.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                        lb_delete.Visible = true;
                    }
                }
            }

        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            GroupId = 0;
            if (hidGroupId.Value != "0")
            {
                GroupId = Convert.ToInt32(hidGroupId.Value);
            }

            if (btnSave.Text == "Save" || btnSave.Text == "Update")
            {
                if (Validation() == true)
                {
                    if (GroupId == 0 && btnSave.Text == "Save")
                    {
                        if (clsGeneral.IsExitGroup("GroupCode", "Group", txtCode.Text.Trim()) == true)
                        {
                            tdMsg.InnerHtml = clsGeneral.warningMsg("Group Code already exit.Please choose another code.");
                            txtDescription.Focus();
                            return;
                        }


                        GroupIds = Save();
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Group Saved Successfully");
                    }
                    else if (GroupId != 0 && btnSave.Text == "Update")
                    {
                        Update();
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Group Updated Successfully");
                    }
                }
                else
                {
                    return;
                }
            }
            if (GroupId != 0 && btnSave.Text == "Delete" && txtCode.Text != "")
            {
                    Delete();
            }
            txtCode.Text = "";
            txtname.Text = "";
            txtDescription.Text = "";
            GroupId = 0;
            btnSave.Text = "Save";
            LoadGroup();
            hidGroupId.Value = "0";
        }


        catch (SqlException Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }

    }

    private int Save()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            strQuery = "Insert into [Group](SchoolId,GroupCode,GroupName,GroupDesc,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) values(" + sess.SchoolId + ",'" + txtCode.Text.Trim().ToUpper() + "','" + txtname.Text.Trim() +"','" + txtDescription.Text.Trim() + "'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            retValsave = objData.ExecuteWithScope(strQuery);
        }
        return retValsave;
    }
    private bool Update()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            strQuery = "Update [Group] SET SchoolId=" + sess.SchoolId + ",GroupCode='" + txtCode.Text.Trim().ToUpper() +"',GroupName='" + txtname.Text.Trim()+ "',GroupDesc='" + txtDescription.Text.Trim() + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) Where GroupId=" + GroupId + "";
            retVal = Convert.ToBoolean(objData.Execute(strQuery));
        }
        return retVal;
    }
    private bool Delete()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            DataTable Dt = objData.ReturnDataTable("Select Grp.GroupId, Grp.GroupCode, RoleGroup.GroupId AS RoleGroupId FROM [Group] Grp LEFT JOIN RoleGroup ON  RoleGroup.GroupId =Grp.GroupId WHERE RoleGroup.GroupId='" + GroupId + "' ", false);

            if (Dt.Rows.Count == 0)
            {
                strQuery = "Delete from [Group] Where GroupId=" + GroupId + "";
                retVal = Convert.ToBoolean(objData.Execute(strQuery));
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Group Deleted Successfully");
            }
            else
                tdMsg.InnerHtml = clsGeneral.warningMsg("Cannot Delete this Group !!!");
        }
        return retVal;
    }



    private void LoadGroup()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            DataTable Dt = objData.ReturnDataTable("Select Grp.GroupId,Grp.GroupCode,Grp.GroupName,Grp.GroupDesc,Usr.UserLName+ ','+Usr.UserFName AS ModifiedUser,  CONVERT(varchar(27), Grp.ModifiedOn, 101) as ModifiedDate from  ([Group] Grp LEFT JOIN [User] Usr ON Usr.UserId=Grp.CreatedBy) WHERE Usr.ActiveInd<>'D' order by Grp.GroupCode", false);

            grdGroup.DataSource = Dt;
            grdGroup.DataBind();
        }


    }
    private void LoadGroupById()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null && GroupId != 0)
        {
            DataTable Dt = objData.ReturnDataTable("Select GroupCode,GroupName,GroupDesc from [Group] Where GroupId=" + GroupId + "", false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    txtCode.Text = Dt.Rows[0]["GroupCode"].ToString();
                    txtDescription.Text = Dt.Rows[0]["GroupDesc"].ToString();
                    txtname.Text = Dt.Rows[0]["GroupName"].ToString();
                    hidGroupId.Value = GroupId.ToString();
                }
            }
        }
    }
    protected void grdGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
    }
    protected void grdGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGroup.PageIndex = e.NewPageIndex;
        LoadGroup();
        clearfields();
        gridPermission();
    }
    protected void grdGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            btnSave.Text = "Update";
        }
        if (e.CommandName == "Delete")
        {
            btnSave.Text = "Delete";
        }
        GroupId = Convert.ToInt32(e.CommandArgument);
        LoadGroupById();
        tdMsg.InnerHtml = "";
    }
    protected void grdGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearfields();
    }

    private void clearfields()
    {
        GroupId = 0;
        btnSave.Text = "Save";
        txtCode.Text = "";
        txtDescription.Text = "";
        txtname.Text = "";
        hidGroupId.Value = GroupId.ToString();
    }

    //protected void grdGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton imgBtnEdi = (ImageButton)e.Row.FindControl("lb_Edit");
    //        ImageButton imgBtnDel = (ImageButton)e.Row.FindControl("lb_delete");
    //        if (Disable == true)
    //        {
    //            //imgBtnEdi.Enabled = false;
    //            //imgBtnDel.Enabled = false;
    //            grdGroup.Columns[3].Visible = false;
    //            grdGroup.Columns[4].Visible = false;
    //        }
    //        else
    //        {
    //            grdGroup.Columns[3].Visible = true;
    //            grdGroup.Columns[4].Visible = true;
    //            //imgBtnEdi.Enabled = true;
    //            //imgBtnDel.Enabled = true;
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {

    //    }
    //}
    private bool Validation()
    {
        objData = new clsData();
        if (txtCode.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Group Code");
            txtCode.Focus();
            return false;
        }
        else if (txtname.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Group Name");
            txtname.Focus();
            return false;
        }
        else if (txtDescription.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Group Description");
            txtDescription.Focus();
            return false;
        }
        


        return true;

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (hidGroupId.Value != "0")
        {
            GroupId = Convert.ToInt32(hidGroupId.Value);
            Session["GROUPID"] = GroupId.ToString();
            Response.Redirect("AddRole.aspx");
        }
        else
        {
            if (Validation() == true)
            {
                btnSave_Click(sender, e);
                Session["GROUPID"] = GroupIds.ToString();
                Response.Redirect("AddRole.aspx");
            }

        }


    }


    private void gridPermission()
    {
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            grdGroup.Columns[4].Visible = false;
            grdGroup.Columns[5].Visible = false;

        }
        else
        {
            grdGroup.Columns[4].Visible = true;
            grdGroup.Columns[5].Visible = true;
        }
    }
    protected void grdGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridPermission();
    }
}