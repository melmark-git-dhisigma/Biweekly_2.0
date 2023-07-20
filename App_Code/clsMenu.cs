using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for clsMenu
/// </summary>
public class clsMenu
{

    clsData objData = null;
    string strQuery = "";
    string strHtml = "";
    public clsMenu()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public TreeView GetMenuByParent(TreeView tvMenus, int SchoolId)
    {

        objData = new clsData();

        strQuery = "Select ObjectId as MenuID,ObjectName MenuName,ObjectUrl as MenuLink from [Object]  where ObjectType='M' AND SchoolId=" + SchoolId + " AND ParntObjectId=0 ORDER BY ObjectId";

        DataTable dtRole = objData.ReturnDataTable(strQuery, false);

        DataSet dsRoles = new DataSet("TableName");
        dsRoles.Tables.Add(dtRole);



        tvMenus.Nodes.Clear();
        TreeNode rootNode = new TreeNode("Menu", "0");
        rootNode.PopulateOnDemand = true;
        tvMenus.Nodes.Add(rootNode);

        if (dsRoles != null)
        {
            DataTable dtRoles = dsRoles.Tables[0];
            if (dtRoles != null && dtRoles.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRoles.Rows)
                {
                    TreeNode node = new TreeNode(dr["MenuName"].ToString(), dr["MenuID"].ToString());
                    node.PopulateOnDemand = true;
                    rootNode.ChildNodes.Add(node);
                }
            }
        }

        tvMenus.ExpandAll();
        return tvMenus;
    }


    public DataTable GetMenus(int RoleId, int SchoolId)
    {
        objData = new clsData();
        strQuery = "SELECT     Menu.ObjectId AS MenuID, Menu.ObjectName AS MenuName, Menu.ParntObjectId AS ParentID FROM  dbo.RolePermission Per INNER JOIN";
        strQuery += " Object Menu ON Per.ObjectId = Menu.ObjectId WHERE     (Per.SchoolId = 1) AND (Per.RoleId = 3) AND (Per.ActiveInd = 'A')";

        DataTable dtMenu = objData.ReturnDataTable(strQuery, false);
        return dtMenu;
    }


    private DataTable getParents(int SchoolId, int UserId)
    {
        objData = new clsData();

        strQuery = "SELECT ObjectId AS MenuID, ObjectName AS MenuName, ObjectUrl from Object Where  ObjectType = 'M' AND ParntObjectId = 0 And ObjectId In( ";
        strQuery += "Select O.ParntObjectId from Object O Where O.ObjectId IN ( Select O.ParntObjectId  from Object O Inner Join  RoleGroupPerm RGP ON O.ObjectId=RGP.ObjectId ";
        strQuery += "Inner Join UserRoleGroup URG ON RGP.RoleGroupId= URG.RoleGroupId where URG.UserId=" + UserId + "  And RGP.ReadInd=1 And URG.ActiveInd='A' And O.SchoolId=" + SchoolId + ") ) ";

      

        DataTable dtRole = objData.ReturnDataTable(strQuery, false);
        return dtRole;
    }
    public DataTable GetMenuChilds(int MenuId, int SchoolId, int UserId)
    {

        objData = new clsData();
        strQuery = " SELECT     O.ObjectId AS MenuID, O.ObjectName AS MenuName, O.ObjectUrl as MenuLink FROM RoleGroupPerm RGP INNER JOIN UserRoleGroup URG ON RGP.RoleGroupId = URG.RoleGroupId INNER JOIN ";
        strQuery += " [Object] O ON RGP.ObjectId = O.ObjectId WHERE        (RGP.WriteInd = 1 or RGP.ReadInd=1) AND URG.ActiveInd = 'A' AND URG.SchoolId =  '" + SchoolId + "' AND O.ObjectType = 'M' AND  URG.UserId = '" + UserId + "' AND O.ParntObjectId = '" + MenuId + "' order by SortOrder Asc ";
        //strQuery = "Select O.ObjectId as MenuID,O.ObjectName MenuName,O.ObjectUrl as MenuLink from [Object] O INNER JOIN  RolePermission ON O.ObjectId = RolePermission.ObjectId  where O.ObjectType='M' AND O.ParntObjectId=" + MenuId + " AND RolePermission.RoleId = " + RoleId + " ORDER BY O.ObjectId";

        DataTable dtMenu = objData.ReturnDataTable(strQuery, false);
        return dtMenu;
    }
    public DataTable GetMenuChildsTree(int MenuId, int SchoolId)
    {
        objData = new clsData();
        strQuery = "Select ObjectId as MenuID,ObjectName MenuName,ObjectUrl as MenuLink from [Object]  where ObjectType='M' AND SchoolId=" + SchoolId + " AND ParntObjectId=" + MenuId + " ORDER BY ObjectId";

        DataTable dtMenu = objData.ReturnDataTable(strQuery, false);
        return dtMenu;
    }
    public void createMenuHtml(int SchoolId, int RoleId, out string menuHtml)
    {

        DataTable DtParent = getParents(SchoolId, RoleId);
        if (DtParent != null)
         {
            if (DtParent.Rows.Count > 0)
            {
                strHtml = "<div id='wrapper'>";

                for (int ip = 0; ip < DtParent.Rows.Count; ip++)
                {

                    string c = (ip + 1).ToString();
                    string ItemC = "item" + c.ToString();
                    string menuNameP = DtParent.Rows[ip]["MenuName"].ToString();
                    int menuId = Convert.ToInt32(DtParent.Rows[ip]["MenuID"]);
                    string menuLinkC = "AdminHome.aspx?Id=" + menuId + "";

                    strHtml += "<div class='menu'><a href='" + menuLinkC + "' class='" + ItemC + "'>" + menuNameP + "</a></div>";
                }

                strHtml += "</div>";


            }
        }
        if (strHtml.Contains("<div id='wrapper'><ul class='menu'></ul></div>")) strHtml = "";
        menuHtml = strHtml;

    }
    //public void createMenuHtml(int SchoolId, int RoleId, out ArrayList arUrl, out Hashtable arName, out string menuHtml)
    //{
    //    arName = new Hashtable();
    //    arUrl = new ArrayList();

    //    DataTable DtParent = getParents(SchoolId, RoleId);
    //    if (DtParent.Rows.Count > 0)
    //    {

    //        strHtml = "<div id='wrapper'><ul class='menu'>";
    //        for (int ip = 0; ip < DtParent.Rows.Count; ip++)
    //        {
    //            int count = ip + 1;
    //            string ItemP = "item" + count.ToString();
    //            string menuNameP = DtParent.Rows[ip]["MenuName"].ToString();
    //            int menuId = Convert.ToInt32(DtParent.Rows[ip]["MenuID"]);

    //            DataTable DtChild = GetMenuChilds(menuId, SchoolId, RoleId);

    //            if (DtChild.Rows.Count > 0)
    //            {
    //                strHtml += "<li class='" + ItemP + "'><a href='#'>" + menuNameP + "</a><ul>";
    //            }

    //            if (DtChild.Rows.Count > 0)
    //            {
    //                for (int ic = 0; ic < DtChild.Rows.Count; ic++)
    //                {
    //                    string c = count + (ic + 1).ToString();
    //                    string ItemC = "subitem" + count.ToString();
    //                    string menuNameC = DtChild.Rows[ic]["MenuName"].ToString();
    //                    string menuLinkC = DtChild.Rows[ic]["MenuLink"].ToString();


    //                    try
    //                    {
    //                        arUrl.Add(menuLinkC);
    //                        arName.Add(menuLinkC, menuNameC);
    //                    }
    //                    catch { }
    //                    //menuLinkC = "AdminHome.aspx?Id=10";
    //                    if (menuLinkC == "") menuLinkC = "#";
    //                    strHtml += "<li class='" + ItemC + "'><a href='" + menuLinkC + "'>" + menuNameC + "<span>" + c + "</span></a></li>";
    //                }

    //                strHtml += "</ul></li>";
    //            }
    //        }
    //        strHtml += "</ul></div>";

    //    }
    //    if (strHtml.Contains("<div id='wrapper'><ul class='menu'></ul></div>")) strHtml = "";
    //    menuHtml = strHtml;

    //}
}