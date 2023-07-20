using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;



/// <summary>
/// Summary description for clsRoles
/// </summary>
public class clsRoles
{

    clsData objData = null;
    string strQuery = "";
    string menuHtml = "";

    public void fillGroups(DropDownList ddl, int SchoolId)
    {
        objData = new clsData();
        objData.ReturnDropDown("Select GroupId as Id, GroupName as Name from [Group] where SchoolId=" + SchoolId + "", ddl);
    }
    public bool UserRoleExit(int UserId, int RoleGroupId)
    {
        objData = new clsData();
        bool val = objData.IFExists("Select RoleGroupId from dbo.UserRoleGroup where UserId=" + UserId + " and RoleGroupId=" + RoleGroupId + " ");
        return val;
    }
    public string GetAD(int UserId, int RoleGroupId)
    {
        objData = new clsData();
        string val = objData.FetchValue("Select ActiveInd from dbo.UserRoleGroup where UserId=" + UserId + " and RoleGroupId=" + RoleGroupId + " ").ToString();
        return val;
    }

    public clsRoles()
    {
    }
    public void fillRole(DropDownList ddlRole)
    {
        objData = new clsData();
        strQuery = "Select RoleId as Id,RoleDesc as Name from dbo.Role order by RoleId";
        objData.ReturnDropDown(strQuery, ddlRole);
    }
    public DataTable GetAllRoles()
    {
        objData = new clsData();

        strQuery = "Select RoleId RoleID,RoleDesc as RoleDescription from dbo.Role order by RoleId";

        DataTable dtRole = objData.ReturnDataTable(strQuery, false);
        return dtRole;
    }
    public int DeletePermissionsByRoleId(int RoleId)
    {
        objData = new clsData();
        int retVal = 0;

        bool val = objData.IFExists("Select RoleId from RolePermission where RoleId=" + RoleId + "");
        if (val == true)
        {
            strQuery = "Delete  RolePermission where RoleId=" + RoleId + "";
            retVal = objData.Execute(strQuery);
        }
        else
        {
            retVal = 1;
        }
        return retVal;

    }
    public int InsertRoleToObject(int SchoolId, int RoleGroupId, int ObjectId, bool ReadInd, bool WriteInd, bool ApproveInd, int CreatedBy, int ModifiedBy)
    {
        objData = new clsData();

        strQuery = "Insert Into RoleGroupPerm (SchoolId,RoleGroupId,ObjectId, ReadInd, WriteInd,ApproveInd,CreatedBy,CreatedOn) ";
        strQuery += "values(" + SchoolId + "," + RoleGroupId + "," + ObjectId + ",'" + ReadInd + "','" + WriteInd + "','" + ApproveInd + "' ," + CreatedBy + ",  GETDATE())";
        return objData.Execute(strQuery);
    }

    public int UpdateRoleToObject(int SchoolId, int RoleGroupId, bool AccessInd, bool ReadInd, bool WriteInd, bool ApproveInd, int modifiedBy, int ObjectId)
    {
        objData = new clsData();

        strQuery = "Update RoleGroupPerm SET SchoolId='" + SchoolId + "',RoleGroupId='" + RoleGroupId + "',AccessInd='" + AccessInd + "', ReadInd='" + ReadInd + "', WriteInd='" + WriteInd + "', ApproveInd='" + ApproveInd + "',ModifiedBy='" + modifiedBy + "',ModifiedOn= GETDATE() where ObjectId='" + ObjectId + "'";
        return objData.Execute(strQuery);
    }
    public DataTable GetRolesByRoleId(int RoleId, int SchoolId)
    {

        objData = new clsData();

        strQuery = "Select Per.ObjectId as MenuID,Menu.ObjectName	from   RolePermission Per JOIN Object Menu On Per.ObjectId = Menu.ObjectId";
        strQuery += " Where Per.RoleId = " + RoleId + " And Per.SchoolId=" + SchoolId + " And ActiveInd='A' order by Menu.ObjectId";

        DataTable dtRole = objData.ReturnDataTable(strQuery, false);
        return dtRole;
    }
    public TreeView GetRoles(TreeView tvRoles)
    {

        objData = new clsData();

        strQuery = "Select RoleId as RoleID,RoleDesc as RoleDescription from dbo.Role order by RoleId";

        DataTable dtRole = objData.ReturnDataTable(strQuery, false);

        DataSet dsRoles = new DataSet("TableName");
        dsRoles.Tables.Add(dtRole);



        tvRoles.Nodes.Clear();
        TreeNode rootNode = new TreeNode("Roles", "0");
        rootNode.PopulateOnDemand = true;
        tvRoles.Nodes.Add(rootNode);

        if (dsRoles != null)
        {
            DataTable dtRoles = dsRoles.Tables[0];
            if (dtRoles != null && dtRoles.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRoles.Rows)
                {
                    TreeNode node = new TreeNode(dr["RoleDescription"].ToString(), dr["RoleID"].ToString());
                    node.PopulateOnDemand = true;
                    rootNode.ChildNodes.Add(node);
                }
            }
        }

        tvRoles.ExpandAll();
        return tvRoles;
    }

    public void DeleteItems(int RoleGroupId)
    {
        objData = new clsData();
        if (RoleGroupId > 0)
        {
            strQuery = "Delete from RoleGroupPerm where RoleGroupId=" + RoleGroupId + "";
            objData.Execute(strQuery);
        }
    }

    public void fillPermissions(DataList DataList1, int GroupId, int RoleId, int SchoolId, int UserId)
    {
        //int c = 0;
        objData = new clsData();
        DataTable DT = objData.ReturnDataTable("SELECT RGP.AccessInd, RGP.ReadInd, RGP.WriteInd, RGP.ApproveInd, RGP.ObjectId FROM RoleGroup RG INNER JOIN RoleGroupPerm RGP ON RG.RoleGroupId = RGP.RoleGroupId   WHERE  RG.RoleGroupId=" + RoleId + " and  RG.GroupId = " + GroupId + " ", false);

        bool Write = false;
        bool Read = false;
        bool App = false;

        try
        {
            if (DT.Rows.Count > 0)
            {
                foreach (DataListItem di in DataList1.Items)
                {
                    DataList dl2 = (DataList)di.FindControl("subcat");
                    CheckBox chkReadAll = (CheckBox)di.FindControl("chkReadAll");
                    CheckBox chkWriteAll = (CheckBox)di.FindControl("chkWriteAll");
                    CheckBox chkApproveAll = (CheckBox)di.FindControl("chkApproveAll");
                    int Subcnt = dl2.Items.Count;
                    int writecnt = 0;
                    int readcnt = 0;
                    int appcnt = 0;
                    foreach (DataListItem di2 in dl2.Items)
                    {
                        CheckBox chkRead = (CheckBox)di2.FindControl("chkRead");
                        CheckBox chkWrite = (CheckBox)di2.FindControl("chkWrite");
                        CheckBox chkApprove = (CheckBox)di2.FindControl("chkApprove");
                        HiddenField hidObjectId = (HiddenField)di2.FindControl("hidObjectId");

                        string searchExpression = "ObjectId = "+hidObjectId.Value;
                        DataRow[] foundRows = DT.Select(searchExpression);

                        foreach(DataRow dr in foundRows)
                        {
   
                            Write = Convert.ToBoolean(dr["WriteInd"]);
                            Read = Convert.ToBoolean(dr["ReadInd"]);
                            App = Convert.ToBoolean(dr["ApproveInd"]);
    
                        }

                        //Write = Convert.ToBoolean(DT.Rows[c]["WriteInd"]);
                        //Read = Convert.ToBoolean(DT.Rows[c]["ReadInd"]);
                        //App = Convert.ToBoolean(DT.Rows[c]["ApproveInd"]);
                        
                        if (Write == true)
                        {
                            chkWrite.Checked = true;
                            writecnt++;
                        }
                        else
                        {
                            chkWrite.Checked = false;
                        }

                        if (Read == true)
                        {
                            chkRead.Checked = true;
                            readcnt++;
                        }
                        else
                        {
                            chkRead.Checked = false;
                        }

                        if (App == true)
                        {
                            chkApprove.Checked = true;
                            appcnt++;
                        }
                        else
                        {
                            chkApprove.Checked = false;
                        }

                        Write = false;
                        Read = false;
                        App = false;
                    }

                       // hidObjectId.Value = Convert.ToString(DT.Rows[c]["ObjectId"]);

                        //c = c + 1;
                    //}
                    if (writecnt == Subcnt)
                        chkWriteAll.Checked = true;
                    if (readcnt == Subcnt)
                        chkReadAll.Checked = true;
                    if (appcnt == Subcnt)
                        chkApproveAll.Checked = true;

                }
            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
    }

    public void fillRolePermissionsUserRights(CheckBoxList chkRole, int GroupId, int UserId, int SchoolId, TextBox Desc)
    {
        objData = new clsData();
        strQuery = "SELECT URG.RoleGroupId,URG.UserRoleGroupDesc FROM  UserRoleGroup URG INNER JOIN RoleGroup  RG ON ";
        strQuery += " URG.RoleGroupId =RG.RoleGroupId WHERE       URG.ActiveInd = 'A' AND URG.UserId=" + UserId + " AND  URG.SchoolId = " + SchoolId + " AND RG.GroupId = " + GroupId + "";


        DataTable Dt = objData.ReturnDataTable(strQuery, false);


        if (Dt != null && Dt.Rows.Count > 0)
        {

            string[] s = new string[Dt.Rows.Count];
            for (int h = 0; h < Dt.Rows.Count; h++)
            {
                s[h] = Dt.Rows[h]["RoleGroupId"].ToString();

            }
            int length = s.Length;
            for (int i = 0; i <= s.Length - 1; i++)
            {

                for (int j = 0; j <= chkRole.Items.Count - 1; j++)
                {
                    if (chkRole.Items[j].Value == s[i])
                    {
                        chkRole.Items[j].Selected = true;
                        break;
                    }
                }
            }
            Desc.Text = Dt.Rows[0]["UserRoleGroupDesc"].ToString();
        }

    }

    public void fillRolePermissions(DropDownList ddlRole, int GroupId, int SchoolId)
    {
        objData = new clsData();
        DataTable Dt = objData.ReturnDataTable("SELECT  distinct RG.RoleId FROM Role R INNER JOIN RoleGroup RG ON R.RoleId =RG.RoleId INNER JOIN RoleGroupPerm RGP ON RG.RoleGroupId =RGP.RoleGroupId WHERE       RG.GroupId = " + GroupId + " AND RG.SchoolId = " + SchoolId + "", false);
        ddlRole.SelectedValue = Dt.Rows[0]["RoleId"].ToString();
    }


    public void DatasheetAndEdit(int UserId, int SchoolId, out bool Data, out bool Edit)
    {
        Data = false;
        Edit = false;

        objData = new clsData();
        string PageName = "In ('Datasheets-Menu','Edit-Menu')";
        string strSql = " SELECT Max(convert(Int,RGP.WriteInd)) AS WriteInd  FROM RoleGroupPerm RGP  INNER JOIN UserRoleGroup URG ON RGP.RoleGroupId = URG.RoleGroupId INNER JOIN  [Object] O ";
        strSql += "ON RGP.ObjectId = O.ObjectId WHERE    URG.SchoolId =  '" + SchoolId + "' AND  URG.UserId = '" + UserId + "'  and O.ObjectName " + PageName + "  Group by O.ObjectName Order by O.ObjectName Asc ";

        DataTable Dt = objData.ReturnDataTable(strSql, false);

        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                Data = Convert.ToBoolean(Dt.Rows[0]["WriteInd"]);
                Edit = Convert.ToBoolean(Dt.Rows[1]["WriteInd"]);
            }
        }


    }
    public static string BinderPageLister(ArrayList arMenuList, bool Datasheet, bool Edit)
    {

        string menuList = "";
        string[] lis = new string[] {
                  
            " <li class='alpha1'><a href='#' id='STUDENT INFO' onclick='error();'><span><img src='../Administration/images/chideren.PNG' alt='' align='left'>STUDENT INFO</span></a></li>",
           " <li class='alpha1'><a href='#' id='ASSESSMENTS' onclick='error();'><span><img src='../Administration/images/asignment.PNG' alt='' align='left'>ASSESSMENTS</span></a></li>",
           " <li class='alpha1'><a href='#' id='IEPS' onclick='error();'><span><img src='../Administration/images/ieps.PNG' alt='' align='left'>IEPS</span></a></li>",
           "<li class='alpha1'><a href='#' id='BSP' onclick='error();'><span><img src='../Administration/images/BSPForms.PNG' alt='' align='left'>BSP FORMS</span></a></li>",
           " <li class='alpha1'><a href='#' id='LESSON PLANS' onclick='error();'><span><img src='../Administration/images/lessonplan.PNG' alt='' align='left'>LESSON PLANS</span></a></li>",
           " <li class='alpha1'><a href='#' id='DATASHEETS' onclick='error();'><span><img src='../Administration/images/listt.PNG' alt='' align='left'>DATASHEETS</span></a></li>",
           " <li class='alpha1'><a href='#' id='TIMERS' onclick='error();'><span><img src='../Administration/images/behaver.PNG' alt='' align='left'>TIMERS</span></a></li>",
           " <li class='alpha1'><a href='#' id='GRAPHS' onclick='error();'><span><img src='../Administration/images/shaire.PNG' alt='' align='left'>GRAPHS</span></a></li>",
           " <li class='alpha1'><a href='#' id='COVERSHEETS' onclick='error();'><span><img src='../Administration/images/transilate.PNG' alt='' align='left'>COVERSHEETS</span></a></li>",
           " <li class='alpha1' id='idEdit'><a href='#' id='EDIT' onclick='error();'><span><img src='../Administration/images/edit-icon.PNG' alt='' align='left'>EDIT</span></a></li>",
           //" <li class='alpha1'><a href='#' onclick='error();'><span><img src='../Administration/images/menu-icon13.png' alt='' align='left'>REPORTING</span></a></li>"
        };
        string[] menus = new string[] { "Student Info-Menu", "Assessments-Menu", "Ieps-Menu","BSP-Menu", "Lesson Plans-Menu", "Datasheets-Menu", "Behavior-Menu", "Graphs-Menu", "Coversheets-Menu", "Event Showing Graph-Menu", "Edit-Menu" };
        foreach (string Item in arMenuList)
        {

            if (Item == "Student Info-Menu")
            {
                lis[0] = " <li class='alpha1'><a href='#' id='STUDENT INFO' onclick='selMenu(this);'><span><img src='../Administration/images/chideren.PNG' alt='' align='left'>CLIENT&nbsp;INFO</span></a></li>";
            }
            if (Item == "Assessments-Menu")
            {
                lis[1] = " <li class='alpha1'><a href='#' id='ASSESSMENTS' onclick='selMenu(this);'><span><img src='../Administration/images/asignment.PNG' alt='' align='left'>ASSESSMENTS</span></a></li>";
            }
            if (Item == "Ieps-Menu")
            {
                lis[2] = "<li class='alpha1'><a href='#' id='IEPS' onclick='selMenu(this);'><span><img src='../Administration/images/ieps.PNG' alt='' align='left'>IEPS</span></a></li>";
            }
            if (Item == "BSP Forms-Menu")
            {
                lis[3] = "<li class='alpha1'><a href='#' id='BSP' onclick='selMenu(this);'><span><img src='../Administration/images/BSPForms.png' style='height: 20px; width: 16px;' alt='' align='left'>BSP FORMS</span></a></li>";
            }
            if (Item == "Lesson Plans-Menu")
            {
                lis[4] = " <li class='alpha1'><a href='#' id='LESSON PLANS' onclick='selMenu(this);'><span><img src='../Administration/images/lessonplan.PNG' alt='' align='left'>LESSON&nbsp;PLANS</span></a></li>";
            }
            if (Item == "Datasheets-Menu")
            {
                if (Datasheet == true)
                {
                    lis[5] = " <li class='alpha1'><a href='#' id='DATASHEETS' onclick='selMenu(this);'><span><img src='../Administration/images/listt.PNG' alt='' align='left'>DATASHEETS</span></a></li>";
                }
                else
                {
                    lis[5] = " <li class='alpha1'><a href='#' id='DATASHEETS' onclick='NoDatasheetAcess();'><span><img src='../Administration/images/listt.PNG' alt='' align='left'>DATASHEETS</span></a></li>";
                }
            }
            if (Item == "Behavior-Menu")
            {
                lis[6] = " <li class='alpha1'><a href='#' id='TIMERS' onclick='selMenu(this);'><span><img src='../Administration/images/behaver.PNG' alt='' align='left'>SET ALERTS</span></a></li>";
            }
            if (Item == "Graphs-Menu")
            {
                lis[7] = " <li class='alpha1'><a href='#' id='GRAPHS' onclick='selMenu(this);'><span><img src='../Administration/images/shaire.PNG' alt='' align='left'>GRAPHS</span></a></li>";
            }
            if (Item == "Coversheets-Menu")
            {
                lis[8] = " <li class='alpha1'><a href='#' id='COVERSHEETS' onclick='selMenu(this);'><span><img src='../Administration/images/transilate.PNG' alt='' align='left'>COVERSHEETS</span></a></li>";
            }
            if (Item == "Edit-Menu")
            {
                if (Edit == true)
                {
                    lis[9] = " <li class='alpha1'><a href='#' id='EDIT' onclick='selMenu(this);'><span><img src='../Administration/images/edit-icon.PNG' alt='' align='left'>EDIT</span></a></li>";
                }
                else
                {
                    lis[9] = " <li class='alpha1'><a href='#' id='EDIT' onclick='NoDatasheetAcess();'><span><img src='../Administration/images/edit-icon.PNG' alt='' align='left'>EDIT</span></a></li>";
                }
            }

        }
        foreach (var item in lis)
        {
            menuList += item;
        }
        return menuList;

    }

}