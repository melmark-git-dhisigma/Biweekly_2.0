using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Admin_AddRole : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    static int RoleId = 0;
    string strQuery = "";
    static bool retVal = false;
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

        if (!IsPostBack)
        {
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
                btnNext.Visible = false;
                if (grdRole.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdRole.Rows)
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
                if (grdRole.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdRole.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                        lb_delete.Visible = true;
                    }
                }
            }

            LoadRole();
            fillGroup();
            RoleId = 0;
            if (Session["GROUPID"] != null)
                ddlGroup.SelectedValue = Session["GROUPID"].ToString();

        }

    }
    private bool Validation()
    {
        objData = new clsData();
        if (ddlGroup.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Group");
            ddlGroup.Focus();
            return false;
        }
        else if (txtCode.Text == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Role Code");
            txtCode.Focus();
            return false;
        }
        else if (txtDescription.Text == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Role Description");
            txtDescription.Focus();
            return false;
        }

        return true;

    }
    private void fillGroup()
    {
        objData = new clsData();
        objData.ReturnDropDown("Select GroupId as Id,GroupName as Name from [Group]  WHERE GroupDesc<>'Admin'  order by GroupName Asc", ddlGroup);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            RoleId = 0;

            if (hidRoleId.Value != "")
            {
                RoleId = Convert.ToInt32(hidRoleId.Value);
            }
            if (btnSave.Text == "Save" || btnSave.Text == "Update")
            {
                if (Validation() == true)
                {
                    RoleId = Convert.ToInt32(hidRoleId.Value);
                    if (RoleId == 0 && btnSave.Text == "Save")
                    {

                        if (clsGeneral.IsExit("RoleCode", "Role", txtCode.Text.Trim()) == true)
                        {
                            tdMsg.InnerHtml = clsGeneral.warningMsg("Role Code already exit.Please choose another code.");
                            txtCode.Focus();
                            return;
                        }

                        int result = Save();
                        if (result > 0)
                            tdMsg.InnerHtml = clsGeneral.sucessMsg("Role Saved Successfully");

                    }
                    else if (RoleId != 0 && btnSave.Text == "Update")
                    {

                        bool returns = Update();
                        if (returns == true)
                            tdMsg.InnerHtml = clsGeneral.sucessMsg("Role Updated Successfully");

                    }
                }
                else
                {
                    return;
                }
            }

            if (RoleId != 0 && btnSave.Text == "Delete" && txtCode.Text != "")
            {
                    bool msg = Delete();
            }
            txtCode.Text = "";
            txtDescription.Text = "";
            hidRoleId.Value = "0";
            ddlGroup.SelectedIndex = 0;
            btnSave.Text = "Save";
            RoleId = 0;
            LoadRole();
        }

        catch (SqlException Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(" Please Try Again");
            throw Ex;
        }

    }

    private int Save()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        SqlTransaction Transs = null;
        int RoleID = 0;
        if (sess != null)
        {
            clsData.blnTrans = true;
            SqlConnection con = objData.Open();
            Transs = con.BeginTransaction();

            strQuery = "Insert into [Role](SchoolId,RoleCode,RoleDesc,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) values(" + sess.SchoolId + ",'" + txtCode.Text.Trim().ToUpper() + "','" + txtDescription.Text.Trim() + "'," + sess.LoginId + ",GETDATE()," + sess.LoginId + ",GETDATE())";
            int GroupId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));

            strQuery = "Insert Into RoleGroup (SchoolId,GroupId,RoleId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) values(" + sess.SchoolId + "," + Convert.ToInt32(ddlGroup.SelectedValue) + "," + GroupId + "," + sess.LoginId + ",GETDATE()," + sess.LoginId + ",GETDATE())";
            RoleID = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));

            objData.CommitTransation(Transs, con);
        }
        return RoleID;
    }
    private bool Update()
    {
        SqlTransaction Transs = null;
        SqlConnection con = objData.Open();
        Transs = con.BeginTransaction();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            strQuery = "Update [Role] SET SchoolId=" + sess.SchoolId + ",RoleCode='" + txtCode.Text.Trim().ToUpper() + "',RoleDesc='" + txtDescription.Text.Trim() + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() Where RoleId=" + RoleId + "";
            objData.ExecuteWithTrans(strQuery, con, Transs);

            strQuery = "Update RoleGroup SET SchoolId=" + sess.SchoolId + ",GroupId=" + Convert.ToInt32(ddlGroup.SelectedValue) + ",RoleId=" + RoleId + ",CreatedBy=" + sess.LoginId + ",CreatedOn=GETDATE()  Where RoleId=" + RoleId + "";
            objData.ExecuteWithTrans(strQuery, con, Transs);

            objData.CommitTransation(Transs, con);
            retVal = true;
        }
        return retVal;
    }
    private bool Delete()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            DataTable Dt = objData.ReturnDataTable("Select [Role].RoleId, [Role].RoleCode, UserRoleGroup.RoleGroupId FROM [Role] LEFT JOIN UserRoleGroup ON [Role].RoleId=UserRoleGroup.RoleGroupId WHERE UserRoleGroup.RoleGroupId='" + RoleId + "'AND UserRoleGroup.ActiveInd <> 'D'", false);
            if (Dt.Rows.Count == 0)
            {

                strQuery = "Delete from [Role] Where RoleId=" + RoleId + "";
                retVal = Convert.ToBoolean(objData.Execute(strQuery));
                strQuery = "Delete from [RoleGroup] Where RoleId=" + RoleId + "";
                retVal = Convert.ToBoolean(objData.Execute(strQuery));

                tdMsg.InnerHtml = clsGeneral.sucessMsg("Role Deleted Successfully");
            }
            else
                tdMsg.InnerHtml = clsGeneral.warningMsg("This role is assigned to User(s)!!!");

            retVal = true;
        }
        return retVal;
    }
    private void LoadRole()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            DataTable Dt = objData.ReturnDataTable("Select  R.RoleId,RG.GroupId,R.RoleCode,R.RoleDesc,Usr.UserLName+ ','+Usr.UserFName AS ModifiedUser, " +
                            " R.ModifiedOn AS ModifiedDate ,Gr.GroupName from ([Role] R LEFT JOIN [User] Usr ON Usr.UserId = R.CreatedBy) Inner Join (RoleGroup RG INNER JOIN [Group] " +
                                        " Gr ON Gr.GroupId = RG.GroupId) ON R.RoleId=RG.RoleId  order by Gr.GroupName,RoleDesc ", false);
            grdRole.DataSource = Dt;
            grdRole.DataBind();
        }
    }

    private void LoadRoleById()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null && RoleId != 0)
        {
            DataTable Dt = objData.ReturnDataTable("SELECT RG.GroupId, R.RoleCode AS [Role Code], R.RoleDesc AS [Role Description] FROM [Role] R INNER JOIN RoleGroup RG ON R.RoleId = RG.RoleId WHERE R.RoleId = " + RoleId + "", false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    ddlGroup.SelectedValue = Dt.Rows[0]["GroupId"].ToString();
                    txtCode.Text = Dt.Rows[0]["Role Code"].ToString();
                    txtDescription.Text = Dt.Rows[0]["Role Description"].ToString();
                    hidRoleId.Value = RoleId.ToString();
                }
            }
        }
    }


    protected void grdRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRole.PageIndex = e.NewPageIndex;
        LoadRole();
        clearfields();
    }
    protected void grdRole_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            btnSave.Text = "Update";
        }
        if (e.CommandName == "Delete")
        {
            btnSave.Text = "Delete";
        }
        RoleId = Convert.ToInt32(e.CommandArgument);
        LoadRoleById();
        tdMsg.InnerHtml = "";
    }
    protected void grdRole_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
    }
    protected void grdRole_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearfields();
    }

    private void clearfields()
    {
        RoleId = 0;
        btnSave.Text = "Save";
        txtCode.Text = "";
        txtDescription.Text = "";
        ddlGroup.SelectedIndex = 0;
        hidRoleId.Value = RoleId.ToString();
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (ddlGroup.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Role and Continue....");
            ddlGroup.Focus();
            return;
        }
        else
        {
            if (Validation() == true)
            {
                if (RoleId == 0)
                {
                    int SaveRole = Convert.ToInt32(Save());
                    if (SaveRole > 0)
                    {
                        Session["GROUPID"] = ddlGroup.SelectedItem.Value.ToString();
                        Session["ROLEGROUPID"] = SaveRole.ToString();
                        Response.Redirect("RolePermissions.aspx");
                    }

                }
                else
                {
                    objData = new clsData();
                    Session["GROUPID"] = ddlGroup.SelectedItem.Value.ToString();
                    DataTable Dt = objData.ReturnDataTable("SELECT [RoleGroupId] from RoleGroup WHERE RoleId = " + RoleId + " and [GroupId]=" + Convert.ToInt32(Session["GROUPID"].ToString()) + "", false);
                    if (Dt.Rows.Count > 0)
                    {
                        Session["ROLEGROUPID"] = Dt.Rows[0]["RoleGroupId"].ToString();
                    }
                    Response.Redirect("RolePermissions.aspx");
                }
            }
        }
    }
    protected void grdRole_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Disable == true)
        {
            //imgBtnEdi.Visible = false;
            //imgBtnDel.Visible = false;
            grdRole.Columns[5].Visible = false;
            grdRole.Columns[6].Visible = false;
        }
        else
        {
            grdRole.Columns[5].Visible = true;
            grdRole.Columns[6].Visible = true;
            //imgBtnEdi.Visible = true;
            //imgBtnDel.Visible = false;
        }
    }
}