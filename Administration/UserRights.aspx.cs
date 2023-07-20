using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_UserRights : System.Web.UI.Page
{
    clsData objData = null;
    clsRoles objRole = null;
    clsSession sess = null;
    static string[] userRolesValues = null;
    string strQuery = "";
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
            }
            else
            {
                btnSave.Visible = true;
            }

            fillGroups();
            fillUsers();
            tdMsg.InnerHtml = "";
        }

    }





    private void fillGroups()
    {
        sess = (clsSession)Session["UserSession"];
        objRole = new clsRoles();
        if (sess != null)
        {
            objRole.fillGroups(ddlGroups, sess.SchoolId);
        }
    }
    private void fillRole()
    {
        chkRole.Items.Clear();
        objData = new clsData();

        objData.ReturnCheckBoxList("SELECT DISTINCT RoleGroup.RoleGroupId as Id,Role.RoleDesc as Name FROM Role INNER JOIN RoleGroup ON Role.RoleId = RoleGroup.RoleId WHERE RoleGroup.GroupId = " + ddlGroups.SelectedValue + " order by RoleDesc Asc", chkRole);

        if (chkRole.Items.Count > 0)
        {
            tdMsg.InnerHtml = "";
            lblrole.Visible = true;
            chkRole.Visible = true;
            chkRole.Enabled = true;
            lblrolestar.Visible = true;
        }
        else
        {
            chkRole.Visible = false;
            lblrole.Visible = false;
            lblrolestar.Visible = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("No Roles Found  Under " + ddlGroups.SelectedItem.Text + " !!!..");
        }

    }

    private void fillUsers()
    {
        ddlUser.Items.Clear();
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        if (sess != null)
        {
            objData.ReturnDropDown("SELECT  UserId as Id,UserLName +','+ UserFName as Name from [dbo].[User] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' order by UserFName Asc ", ddlUser);


            if (ddlUser.Items.Count > 0)
            {
                tdMsg.InnerHtml = "";
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("No Roles Found  Under " + ddlGroups.SelectedItem.Text + " !!!..");
            }
        }

    }

    public void ClearFields()
    {
        ddlGroups.SelectedIndex = 0;
        txtDescription.Text = "";

        foreach (ListItem li in ddlUser.Items)
        {
            li.Selected = false;
        }
        foreach (ListItem li in chkRole.Items)
        {
            li.Selected = false;
        }
        lblrole.Visible = false;
        chkRole.Visible = false;
        lblrolestar.Visible = false;
    }
    private void SetGui(string mode)
    {

        switch (mode)
        {
            //case "NEW":
            //    {
            //        btnSave.Text = "Save";
            //        chkRole.Enabled = true;
            //        ddlUser.Enabled = true;
            //        ddlGroups.Enabled = true;
            //        ddlUser.Enabled = true;
            //        txtDescription.ReadOnly = false;
            //        break;
            //    }
            //case "MODIFY":
            //    {
            //        btnSave.Text = "Update";
            //        chkRole.Enabled = true;
            //        ddlUser.Enabled = true;
            //        ddlGroups.Enabled = true;
            //        ddlUser.Enabled = true;
            //        txtDescription.ReadOnly = false;
            //        break;
            //    }
            case "DELETE":
                {
                    chkRole.Enabled = false;
                    ddlUser.Enabled = false;
                    btnSave.Enabled = false;
                    break;
                }
            default:
                {
                    ClearFields();
                    btnSave.Text = "Save";
                    ddlUser.Enabled = true;
                    ddlGroups.Enabled = true;
                    chkRole.Visible = false;
                    lblrole.Visible = false;
                    lblrolestar.Visible = false;
                    break;
                }

        }


    }

    private void FillData()
    {
        objRole = new clsRoles();
        sess = (clsSession)Session["UserSession"];
        objRole.fillRolePermissionsUserRights(chkRole, Convert.ToInt32(ddlGroups.SelectedValue), Convert.ToInt32(ddlUser.SelectedValue), sess.SchoolId, txtDescription);

    }
    private void Save()
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            objRole = new clsRoles();
            if (sess != null)
            {
                if (ddlUser.SelectedIndex > 0)
                {
                    foreach (ListItem ri in chkRole.Items)
                    {
                        if (ri.Selected == true)
                        {
                            if (objRole.UserRoleExit(Convert.ToInt32(ddlUser.SelectedValue), Convert.ToInt32(ri.Value)) == false)
                            {
                                strQuery = "Insert into UserRoleGroup(SchoolId,UserRoleGroupDesc,UserId,RoleGroupId,EffStartDate,ActiveInd,CreatedBy,CreatedOn) ";
                                strQuery += "values(" + sess.SchoolId + " ,'" + txtDescription.Text.Trim() + "'," + ddlUser.SelectedValue + "," + ri.Value + ", GETDATE() ,'A'," + sess.LoginId + ",GETDATE() ) ";
                                objData.ExecuteWithScope(strQuery);
                            }
                            else if (objRole.GetAD(Convert.ToInt32(ddlUser.SelectedValue), Convert.ToInt32(ri.Value)) == "D")
                            {
                                strQuery = "Update UserRoleGroup set EffEndDate=getdate(),ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where UserId=" + ddlUser.SelectedValue + " and RoleGroupId=" + Convert.ToInt32(ri.Value) + "";
                                objData.Execute(strQuery);
                            }
                        }
                        else
                        {
                            strQuery = "Update UserRoleGroup set EffEndDate=getdate(),ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where UserId=" + ddlUser.SelectedValue + " and RoleGroupId=" + Convert.ToInt32(ri.Value) + "";
                            objData.Execute(strQuery);
                        }

                    }
                } ClearFields();
                tdMsg.InnerHtml = clsGeneral.sucessMsg("User Rights Saved Successfully !!!");
            }
            grdGPRole.Visible = false;
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }

    }
    private void Update()
    {
        try
        {
            objData = new clsData();
            objRole = new clsRoles();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                if (ddlUser.SelectedIndex > 0)
                {
                    foreach (ListItem ri in chkRole.Items)
                    {
                        if (ri.Selected == true)
                        {
                            if (objRole.UserRoleExit(Convert.ToInt32(ddlUser.SelectedValue), Convert.ToInt32(ri.Value)) == false)
                            {
                                strQuery = "Insert into UserRoleGroup(SchoolId,UserRoleGroupDesc,UserId,RoleGroupId,EffStartDate,ActiveInd,CreatedBy,CreatedOn) ";
                                strQuery += "values(" + sess.SchoolId + " ,'" + txtDescription.Text.Trim() + "'," + ddlUser.SelectedValue + "," + ri.Value + ",GETDATE(),'A'," + sess.LoginId + ",GETDATE() ) ";
                                objData.ExecuteWithScope(strQuery);
                            }
                            else
                            {
                                if (objRole.GetAD(Convert.ToInt32(ddlUser.SelectedValue), Convert.ToInt32(ri.Value)) == "D")
                                {
                                    strQuery = "Update UserRoleGroup set EffEndDate=getdate(),ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where UserId=" + ddlUser.SelectedValue + " and RoleGroupId=" + Convert.ToInt32(ri.Value) + "";
                                    objData.Execute(strQuery);
                                }
                            }


                        }
                        else
                        {
                            if (objRole.UserRoleExit(Convert.ToInt32(ddlUser.SelectedValue), Convert.ToInt32(ri.Value)) == true)
                            {
                                if (objRole.GetAD(Convert.ToInt32(ddlUser.SelectedValue), Convert.ToInt32(ri.Value)) == "A")
                                {
                                    strQuery = "Update UserRoleGroup set EffEndDate=getdate(),ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where UserId=" + ddlUser.SelectedValue + " and RoleGroupId=" + Convert.ToInt32(ri.Value) + "";
                                    objData.Execute(strQuery);
                                }
                            }
                        }
                    }


                } ClearFields();
                tdMsg.InnerHtml = clsGeneral.sucessMsg("User Rights Updated Successfully !!!");
            }
            grdGPRole.Visible = false;
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }

    }
    private bool validation()
    {
        if (ddlUser.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select a User ");
            ddlUser.Focus();
            return false;
        }
        else if (ddlGroups.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select a group ");
            ddlGroups.Focus();
            return false;
        }
        else if (chkRole.SelectedIndex == -1)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Role ");
            chkRole.Focus();
            return false;
        }
        return true;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (validation() == true)
        {

            if (btnSave.Text.Trim().ToUpper() == "SAVE")
            {
                Save();
            }
            else if (btnSave.Text.Trim().ToUpper() == "UPDATE")
            {
                Update();

            }

            SetGui("");
        }
        
    }
   
    private void checkFillData()
    {
        int i = 0;
        int count = 0;
        foreach (ListItem li in chkRole.Items)
        {
            if (li.Selected == true)
            {
                count++;
            }
        }

        userRolesValues = new string[count];
        foreach (ListItem li in chkRole.Items)
        {
            if (li.Selected == true)
            {
                userRolesValues[i] = li.Value;
            }
        }

    }
    private int ItemChecked()
    {
        int count = 0;
        foreach (ListItem li in chkRole.Items)
        {
            if (li.Selected == true)
            {
                count++;
            }
        }

        return count;
    }

    protected void ddlGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;

        if (ddlGroups.SelectedIndex > 0)
        {
            fillRole();
            FillData();
            checkFillData();
            count = ItemChecked();
            if (count > 0)
            {
                btnSave.Text = "Update";
            }
        }
        else
        {
            tdMsg.InnerHtml = "";
            lblrole.Visible = false;
            lblrolestar.Visible = false;
        }
    }
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (ddlUser.SelectedIndex > 0)
        {
            filluserRole(ddlUser.SelectedItem.Value);
        }
        if (ddlGroups.SelectedIndex > 0)
        {
            fillRole();
            FillData();
            checkFillData();
            count = ItemChecked();
            if (count > 0)
            {
                btnSave.Text = "Update";
            }            
        }
        else
        {
            tdMsg.InnerHtml = "";
        }
    }

    private void filluserRole(string user)
    {        
        objData = new clsData();

        DataTable dt=objData.ReturnDataTable(" SELECT GroupDesc,R.RoleDesc,R.RoleCode FROM UserRoleGroup URG INNER JOIN RoleGroup RG ON RG.RoleGroupId=URG.RoleGroupId INNER JOIN [Group] GP ON GP.GroupId=RG.GroupId INNER JOIN [Role] R ON R.RoleId=RG.RoleId WHERE UserId="+user+" AND ActiveInd='A'",false);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                grdGPRole.Visible = true;
                grdGPRole.DataSource = dt;
                grdGPRole.DataBind();
            }
            else
            {
                grdGPRole.Visible = false;
            }
        }
        else
        {
            grdGPRole.Visible = false;

        }
        
    }
}