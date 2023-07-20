using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

public partial class Admin_UserRights1 : System.Web.UI.Page
{
    DataClass objDataClass = new DataClass();
    clsGeneral objGeneral = null;
    clsData objData = null;
    clsRoles objRole = null;
    clsSession sess = null;  
    int GroupId = 0;   
    int RoleGroupId = 0;  
    public bool AccessInd;


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

            fillData();
            fillGroup();

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
            if (Session["ROLEGROUPID"] != null && Session["GROUPID"] != null)
            {
                ddlGroup.Enabled = true;
                ddlRole.Enabled = true;
                ddlGroup.SelectedValue = Session["GROUPID"].ToString();
                ddlGroup_SelectedIndexChanged(this, EventArgs.Empty);
                ddlRole.SelectedValue = Session["ROLEGROUPID"].ToString();
                ddlRole_SelectedIndexChanged(this, EventArgs.Empty);
                Session["ROLEGROUPID"] = null;
                Session["GROUPID"] = null;
            }
        }
    }

    private void fillRole()
    {

        objData = new clsData();
        if (ddlGroup.SelectedIndex > 0)
        {
            ddlRole.Items.Clear();
            objData.ReturnDropDown("SELECT DISTINCT RoleGroup.RoleGroupId as Id,Role.RoleDesc as Name FROM Role INNER JOIN RoleGroup ON Role.RoleId = RoleGroup.RoleId WHERE RoleGroup.GroupId = " + ddlGroup.SelectedValue + "  order by RoleDesc Asc", ddlRole);

            if (ddlRole.Items.Count > 0)
            {
                tdMsg.InnerHtml = "";
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("No Roles Found  Under " + ddlGroup.SelectedItem.Text + " !!!..");
                ddlRole.Items.Clear();
                ddlRole.Items.Add("---------------Select--------------");
            }
        }
        else if (ddlGroup.SelectedIndex == 0)
        {
            ddlRole.Items.Clear();
            ddlRole.Items.Add("---------------Select--------------");
        }

    }
    private void fillData()
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        dl_screenPermission.DataSource = objData.ReturnDataTable("select ObjectId,'  '+ObjectName as [ObjectName] from object where  [ObjectType] In ('M','MS') order by SortOrder", false);
        dl_screenPermission.DataBind();        
    }
    private void fillSubItem(DataList d, int Id)
    {
        sess = (clsSession)Session["UserSession"];
        d.DataSource = objData.ReturnDataTable("Select ObjectId,ObjectName from [Object] where ([ObjectType]='P' OR [ObjectType]='B') AND [ParntObjectId]=" + Id + " ORDER BY SortOrder", false);
        d.DataBind();
    }

    protected void dl_screenPermission_ItemDataBound(object sender, DataListItemEventArgs e)
    {        
        Label id = (Label)e.Item.FindControl("ID");
        int catid = Convert.ToInt16(id.Text.ToString());
        Panel p = (Panel)e.Item.FindControl("panelsubcat");
        string n = (string)e.Item.FindControl(p.ID).ClientID;
        TableCell td = (TableCell)e.Item.FindControl("tdItem");
        HyperLink l = (HyperLink)e.Item.FindControl("MainCat");
        l.Attributes.Add("onclick", "showsubmenu('" + n + "')");
        DataList d = (DataList)e.Item.FindControl("subcat");
        fillSubItem(d, catid);
    }



    private void fillGroup()
    {
        objData = new clsData();
        objData.ReturnDropDown("Select GroupId as Id,GroupName as Name from [Group]  order by GroupName Asc", ddlGroup);
    }

    protected void btnCompositeSave_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (ddlGroup.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Group ...Try Again");
            ddlGroup.Focus();
            return;
        }
        else
        {
            try
            {
                fillRole();
            }
            catch (SqlException Ex)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Error... Please Try Again or Contact Administrator");
                throw Ex;
            }
        }
    }

    private void SetGui(string mode)
    {

        switch (mode)
        {
            
            case "DELETE":
                {
                    ddlGroup.Enabled = true;
                    btnSave.Enabled = false;
                    break;
                }
            default:
                {
                    ClearFields();
                    ddlGroup.SelectedIndex = 0;
                    ddlRole.SelectedIndex = 0;
                    btnSave.Text = "Save";

                    btnSave.Enabled = true;
                    ddlRole.Enabled = true;
                    ddlGroup.Enabled = true;
                    break;
                }
        }
    }
    private void ClearFields()
    {
        foreach (DataListItem di in dl_screenPermission.Items)
        {
            DataList dl2 = (DataList)di.FindControl("subcat");
            foreach (DataListItem di2 in dl2.Items)
            {
                CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");
                CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");
                HiddenField hidObjectId = (HiddenField)di2.FindControl("hidObjectId");
                chkRead.Checked = false;
                chkWrite.Checked = false;
                chkApprove.Checked = false;
                hidObjectId.Value = "";
            }
            CheckBox chkReadAll = (CheckBox)di.FindControl("chkReadAll");
            CheckBox chkWriteAll = (CheckBox)di.FindControl("chkWriteAll");
            CheckBox chkApproveAll = (CheckBox)di.FindControl("chkApproveAll");
            chkReadAll.Checked = false;
            chkWriteAll.Checked = false;
            chkApproveAll.Checked = false;
        }
    }

    private void Update()
    {

    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Message = "";
        if (ddlGroup.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Group...Try Again");
            ddlGroup.Focus();
            return;
        }
        else
        {
            objRole = new clsRoles();
            GroupId = Convert.ToInt32(ddlGroup.SelectedValue);

            if (GroupId > 0)
            {
                try
                {
                    objRole = new clsRoles();
                    if (btnSave.Text.Trim().ToUpper() == "UPDATE")
                    {                        
                        Message = "Screen Permissions Updated Successfully!!!..";
                    }
                    else
                    {
                        Message = "Screen Permissions Saved Successfully!!!..";
                    }
                    objRole.DeleteItems(Convert.ToInt32(ddlRole.SelectedValue));
                    InsertToRoleToMenu(GroupId);
                    ClearFields();
                    tdMsg.InnerHtml = clsGeneral.sucessMsg(Message);
                    SetGui("");
                }
                catch (SqlException Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error... Please Try Again or Contact Administrator");
                    throw Ex;
                }

            }
        }
    }



    private void InsertToRoleToMenu(int GroupId)
    {
        objRole = new clsRoles();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        int retVal = 0;

        if (ddlRole.SelectedIndex > 0)
        {
            RoleGroupId = Convert.ToInt32(ddlRole.SelectedValue);

            if (RoleGroupId > 0)
            {
                foreach (DataListItem di in dl_screenPermission.Items)
                {                    
                    DataList dl2 = (DataList)di.FindControl("subcat");
                    foreach (DataListItem di2 in dl2.Items)
                    {
                        CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                        CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");                   
                        CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");            
                        
                        HiddenField hidObjectId = (HiddenField)di2.FindControl("hidObjectId");
                        retVal = objRole.InsertRoleToObject(sess.SchoolId, RoleGroupId, Convert.ToInt32(hidObjectId.Value), chkRead.Checked, chkWrite.Checked, chkApprove.Checked, sess.LoginId, 0);
                    }

                }
            }
        }




    }
    protected void btnModify_Click(object sender, EventArgs e)
    {
        SetGui("MODIFY");
    }

    private void fillAllPermissions()
    {
        try
        {
            tdMsg.InnerText = "";
            // fillRole();
            fillData();
            if (ddlGroup.SelectedIndex > 0)
            {
                sess = (clsSession)Session["UserSession"];
                if (sess != null)
                {
                    objRole = new clsRoles();
                    objRole.fillPermissions(dl_screenPermission, Convert.ToInt32(ddlGroup.SelectedValue), Convert.ToInt32(ddlRole.SelectedValue), sess.SchoolId,sess.LoginId);
                }
                else
                {
                    Response.Redirect("Error.aspx");
                }
            }

        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }
    }
    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRole.SelectedIndex != 0)
        {
            fillAllPermissions();
            displayAll();
        }
    }

    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillRole();
    }
    private void displayAll()
    {
        foreach (DataListItem di in dl_screenPermission.Items)
        {
            DataList dl2 = (DataList)di.FindControl("subcat");
            Panel pnl = (Panel)di.FindControl("panelsubcat");
            foreach (DataListItem di2 in dl2.Items)
            {
                CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");                
                CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");
                //------------------------------------------------
                if ( chkRead.Checked == true || chkWrite.Checked == true || chkApprove.Checked == true)
                {
                    //pnl.Attributes.CssStyle.Add("Display", "block");
                    btnSave.Text = "Update";
                }
                else
                {
                    //pnl.Attributes.CssStyle.Add("Display", "none");
                }
            }
        }
    }



    protected void chkReadAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkReadAll = (CheckBox)sender;
            DataListItem item = (DataListItem)chkReadAll.NamingContainer;
            HiddenField hdnReadAll = (HiddenField)item.FindControl("hdnReadAll");
            Panel p = (Panel)item.FindControl("panelsubcat");
            DataList dl2 = (DataList)item.FindControl("subcat");
            int chkReadcnt = 0;
            int Subcnt = dl2.Items.Count;
            foreach (DataListItem di2 in dl2.Items)
            {
                CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                if (chkRead.Checked == true)
                    chkReadcnt++;
                if (chkReadAll.Checked)
                {
                    chkRead.Checked = true;
                }
            }
            if (Subcnt == chkReadcnt && chkReadAll.Checked == false)
            {
                foreach (DataListItem di2 in dl2.Items)
                {
                    CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                    chkRead.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
        //objGeneral = new clsGeneral();
        //foreach (DataListItem di in dl_screenPermission.Items)
        //{
        //    CheckBox chkReadAll = (CheckBox)di.FindControl("chkReadAll");
        //    Panel pnl = (Panel)di.FindControl("panelsubcat");

        //    if (chkReadAll.Checked == true)
        //    {
        //        objGeneral.CheckStatus(di, chkReadAll, 1, "R");
        //        //pnl.Attributes.CssStyle.Add("Display", "block");
        //    }
        //    else
        //    {
        //        objGeneral.CheckStatus(di, chkReadAll, 0, "R");
        //        //pnl.Attributes.CssStyle.Add("Display", "none");
        //    }
        //}
    }
    protected void chkWriteAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkWriteAll = (CheckBox)sender;
            DataListItem item = (DataListItem)chkWriteAll.NamingContainer;
            HiddenField hdnReadAll = (HiddenField)item.FindControl("hdnReadAll");
            CheckBox chkReadAll = (CheckBox)item.FindControl("chkReadAll");           
            if (chkWriteAll.Checked == true) chkReadAll.Checked = true;
            Panel p = (Panel)item.FindControl("panelsubcat");
            DataList dl2 = (DataList)item.FindControl("subcat");
            int chkWritecnt = 0;
            int Subcnt = dl2.Items.Count;
            foreach (DataListItem di2 in dl2.Items)
            {
                CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");
                CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                if (chkWrite.Checked == true)
                    chkWritecnt++;
                if (chkWriteAll.Checked)
                {
                    chkWrite.Checked = true;
                    chkRead.Checked = true;
                }
            }
            if (Subcnt == chkWritecnt && chkWriteAll.Checked == false)
            {
                foreach (DataListItem di2 in dl2.Items)
                {
                    CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");
                    chkWrite.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //objGeneral = new clsGeneral();
        //foreach (DataListItem di in dl_screenPermission.Items)
        //{
        //    CheckBox chkWriteAll = (CheckBox)di.FindControl("chkWriteAll");
        //    CheckBox chkReadAll = (CheckBox)di.FindControl("chkReadAll");
        //    Panel pnl = (Panel)di.FindControl("panelsubcat");
        //    if (chkWriteAll.Checked == true)
        //    {
        //        objGeneral.CheckStatus(di, chkWriteAll, 1, "W");
        //        chkReadAll.Checked = true;
        //        //pnl.Attributes.CssStyle.Add("Display", "block");
        //    }
        //    else
        //    {
        //        objGeneral.CheckStatus(di, chkWriteAll, 0, "W");
        //        //pnl.Attributes.CssStyle.Add("Display", "none");
        //    }

        //    if (chkReadAll.Checked == true)
        //    {
        //        objGeneral.CheckStatus(di, chkReadAll, 1, "R");
        //        //pnl.Attributes.CssStyle.Add("Display", "block");
        //    }
        //    else
        //    {
        //        objGeneral.CheckStatus(di, chkReadAll, 0, "R");
        //        //pnl.Attributes.CssStyle.Add("Display", "none");
        //    }
        //}
    }
    protected void chkNoAccess_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkNoAcess = (CheckBox)sender;
        DataListItem item = (DataListItem)chkNoAcess.NamingContainer;
        CheckBox Read = (CheckBox)item.FindControl("chkRead");
        CheckBox Access = (CheckBox)item.FindControl("chkNoAccess");
        CheckBox Approve = (CheckBox)item.FindControl("chkApprove");
        CheckBox Write = (CheckBox)item.FindControl("chkWrite");


        if (Access.Checked == true)
        {
            Read.Checked = false;
            Approve.Checked = false;
            Write.Checked = false;
        }
        getExpand();

    }
    private void getExpand()
    {
        foreach (DataListItem di in dl_screenPermission.Items)
        {
            Panel pnl = (Panel)di.FindControl("panelsubcat");
            //pnl.Attributes.CssStyle.Add("Display", "block");
        }
    }

    
    protected void chkApproveAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkApproveAll = (CheckBox)sender;
            DataListItem item = (DataListItem)chkApproveAll.NamingContainer;
            HiddenField hdnReadAll = (HiddenField)item.FindControl("hdnReadAll");            
            Panel p = (Panel)item.FindControl("panelsubcat");
            DataList dl2 = (DataList)item.FindControl("subcat");
            int chkApprovecnt = 0;
            int Subcnt = dl2.Items.Count;
            foreach (DataListItem di2 in dl2.Items)
            {
                CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");
                if (chkApprove.Checked == true)
                    chkApprovecnt++;
                if (chkApproveAll.Checked)
                {
                    chkApprove.Checked = true;
                }
            }
            if (Subcnt == chkApprovecnt && chkApproveAll.Checked == false)
            {
                foreach (DataListItem di2 in dl2.Items)
                {
                    CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");
                    chkApprove.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //objGeneral = new clsGeneral();
        //foreach (DataListItem di in dl_screenPermission.Items)
        //{
        //    CheckBox chkApproveAll = (CheckBox)di.FindControl("chkApproveAll");
        //    Panel pnl = (Panel)di.FindControl("panelsubcat");

        //    if (chkApproveAll.Checked == true)
        //    {
        //        objGeneral.CheckStatus(di, chkApproveAll, 1, "A");
        //        //pnl.Attributes.CssStyle.Add("Display", "block");
        //    }
        //    else
        //    {
        //        objGeneral.CheckStatus(di, chkApproveAll, 0, "A");
        //        //pnl.Attributes.CssStyle.Add("Display", "none");
        //    }
        //}
    }
    //-------------------------------------------------------------------------------

    private int ObjCnt(int Objid)
    {
        string ObjectID = "SELECT COUNT(*) FROM RoleGroupPerm where ObjectId='" + Objid + "' ";
        int objectcnt = 0;
        objectcnt = objDataClass.ExecuteScalar(ObjectID);
        return objectcnt;
    }

    protected void chkRead_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            objData = new clsData();
            CheckBox chkRead = (CheckBox)sender;
            DataListItem item = (DataListItem)chkRead.NamingContainer;
            HiddenField hidObjectId = (HiddenField)item.FindControl("hidObjectId");           
            int ParentObjId = Convert.ToInt32(objData.FetchValue("SELECT ParntObjectId FROM [Object] WHERE ObjectId='" + hidObjectId.Value + "'"));
            if (chkRead.Checked == false)
            {                
                foreach (DataListItem di2 in dl_screenPermission.Items)
                {
                    HiddenField ObjectId = (HiddenField)di2.FindControl("hdnReadAll");
                    if (ParentObjId == Convert.ToInt32(ObjectId.Value))
                    {
                        CheckBox chkReadAll = (CheckBox)di2.FindControl("chkReadAll");
                        chkReadAll.Checked = false;
                    }
                }
            }
            else
            {
                foreach (DataListItem di2 in dl_screenPermission.Items)
                {
                    HiddenField ObjectId = (HiddenField)di2.FindControl("hdnReadAll");
                    CheckBox chkReadAll = (CheckBox)di2.FindControl("chkReadAll");
                    DataList dl2 = (DataList)di2.FindControl("subcat");
                    int subcnt = dl2.Items.Count;
                    int Cnt = 0;
                    if (ParentObjId == Convert.ToInt32(ObjectId.Value))
                    {
                        foreach (DataListItem subcat in dl2.Items)
                        {
                            CheckBox chkReadNew = (CheckBox)subcat.FindControl("chkRead");
                            if (chkReadNew.Checked == true)
                                Cnt++;
                        }
                    }
                    if (subcnt == Cnt)
                        chkReadAll.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void chkWrite_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            objData = new clsData();
            CheckBox chkWrite = (CheckBox)sender;
            DataListItem item = (DataListItem)chkWrite.NamingContainer;
            HiddenField hidObjectId = (HiddenField)item.FindControl("hidObjectId");
            int ParentObjId = Convert.ToInt32(objData.FetchValue("SELECT ParntObjectId FROM [Object] WHERE ObjectId='" + hidObjectId.Value + "'"));
            if (chkWrite.Checked == false)
            {
                foreach (DataListItem di2 in dl_screenPermission.Items)
                {
                    HiddenField ObjectId = (HiddenField)di2.FindControl("hdnReadAll");
                    if (ParentObjId == Convert.ToInt32(ObjectId.Value))
                    {
                        CheckBox chkWriteAll = (CheckBox)di2.FindControl("chkWriteAll");
                        chkWriteAll.Checked = false;
                    }
                }
            }
            else
            {
                foreach (DataListItem di2 in dl_screenPermission.Items)
                {
                    HiddenField ObjectId = (HiddenField)di2.FindControl("hdnReadAll");
                    CheckBox chkReadAll = (CheckBox)di2.FindControl("chkReadAll");
                    CheckBox chkWriteAll = (CheckBox)di2.FindControl("chkWriteAll");
                    DataList dl2 = (DataList)di2.FindControl("subcat");
                    int subcnt = dl2.Items.Count;
                    int Cnt = 0;
                    if (ParentObjId == Convert.ToInt32(ObjectId.Value))
                    {
                        foreach (DataListItem subcat in dl2.Items)
                        {
                            CheckBox chkWritenew = (CheckBox)subcat.FindControl("chkWrite");
                            if (chkWritenew.Checked == true)
                                Cnt++;
                        }
                    }
                    if (subcnt == Cnt)
                    {
                        chkReadAll.Checked = true;
                        chkWriteAll.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void chkApprove_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            objData = new clsData();
            CheckBox chkApprove = (CheckBox)sender;
            DataListItem item = (DataListItem)chkApprove.NamingContainer;
            HiddenField hidObjectId = (HiddenField)item.FindControl("hidObjectId");
            int ParentObjId = Convert.ToInt32(objData.FetchValue("SELECT ParntObjectId FROM [Object] WHERE ObjectId='" + hidObjectId.Value + "'"));
            if (chkApprove.Checked == false)
            {
                foreach (DataListItem di2 in dl_screenPermission.Items)
                {
                    HiddenField ObjectId = (HiddenField)di2.FindControl("hdnReadAll");
                    if (ParentObjId == Convert.ToInt32(ObjectId.Value))
                    {
                        CheckBox chkApproveAll = (CheckBox)di2.FindControl("chkApproveAll");
                        chkApproveAll.Checked = false;
                    }
                }
            }
            else
            {
                foreach (DataListItem di2 in dl_screenPermission.Items)
                {
                    HiddenField ObjectId = (HiddenField)di2.FindControl("hdnReadAll");
                    CheckBox chkApproveAll = (CheckBox)di2.FindControl("chkApproveAll");
                    DataList dl2 = (DataList)di2.FindControl("subcat");
                    int subcnt = dl2.Items.Count;
                    int Cnt = 0;
                    if (ParentObjId == Convert.ToInt32(ObjectId.Value))
                    {
                        foreach (DataListItem subcat in dl2.Items)
                        {
                            CheckBox chkApprovenew = (CheckBox)subcat.FindControl("chkApprove");
                            if (chkApprovenew.Checked == true)
                                Cnt++;
                        }
                    }
                    if (subcnt == Cnt)
                        chkApproveAll.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}