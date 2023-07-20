using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;


public class clsTreePopulate
{

    clsData objData = null;
    string strQuery = "";
    string strHtml = "";

    int[] a = null;
    int[] b = null;
    int[] orig = null;



	public clsTreePopulate()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void getIds()
    {
        string ss = "";
        a = new int[25];
        b = new int[25];
        orig = new int[25];


         objData = new clsData();
         strQuery = "Select GoalId from Goal where ParntGoalId is Null";
         DataTable dt = objData.ReturnDataTable(strQuery, false);
         if (dt.Rows.Count > 0)
         {
             int i = 0;

             foreach (DataRow dr in dt.Rows)
             {
                 a[i] =Convert.ToInt32(dr["GoalId"]);
                
                 orig[i] = a[i];
                 string ab = Convert.ToString(a[i]);
                 ss = ss + ab + ",";
                 i++;
             }
         }

       


         strQuery = "Select GoalId from Goal where ParntGoalId in("+ss+")";
         dt = objData.ReturnDataTable(strQuery, false);
         if (dt.Rows.Count > 0)
         {
             int i = 0;

             foreach (DataRow dr in dt.Rows)
             {
                 b[i] = Convert.ToInt32(dr["GoalId"]);
                 i++;
             }
         }





    }


    public DataTable GetMenuChilds(int MenuId, int SchoolId, int Id)
    {
        if (Id > 3) return null ;

        objData = new clsData();
        strQuery = "Select GoalId,GoalName,ParntGoalId  from Goal  where  SchoolId=" + SchoolId + "  and ActiveInd='Y' and  ParntGoalId=" + MenuId + " ORDER BY GoalId";

        DataTable dtMenu = objData.ReturnDataTable(strQuery, false);
        return dtMenu;

    }

   

    //private DataTable getParents(int SchoolId)
    //{
    //    objData = new clsData();

    //    strQuery = "Select GoalId,GoalName,ParntGoalId  from Goal  where  SchoolId=" + SchoolId + "  and ActiveInd='Y' and  ParntGoalId is Null ORDER BY GoalId";

    //    DataTable dtRole = objData.ReturnDataTable(strQuery, false);
    //    return dtRole;
    //}

    //public string createMenuHtml(int SchoolId)
    //{        
    //    strHtml = "<div id='wrapper'><ul class='menu'>";

    //    DataTable DtParent = getParents(SchoolId);

    //    for (int ip = 0; ip < DtParent.Rows.Count; ip++)
    //    {
    //        int count = ip + 1;
    //        string ItemP = "item" + count.ToString();
    //        string menuNameP = DtParent.Rows[ip]["GoalName"].ToString();
    //        int menuId = Convert.ToInt32(DtParent.Rows[ip]["GoalId"]);
    //        strHtml += "<li class='" + ItemP + "'><a href='#'>" + menuNameP + "</a><ul>";



    //        DataTable DtChild = GetMenuChilds(menuId,SchoolId);

    //        if (DtChild.Rows.Count > 0)
    //        {
    //            for (int ic = 0; ic < DtChild.Rows.Count; ic++)
    //            {
    //                string c = count + (ic + 1).ToString();
    //                string ItemC = "subitem" + count.ToString();
    //                string menuNameC = DtChild.Rows[ic]["GoalName"].ToString();
    //                string menuLinkC = "";
    //                if (menuLinkC == "") menuLinkC = "#";
    //                strHtml += "<li class='" + ItemC + "'><a href='" + menuLinkC + "'>" + menuNameC + "<span>" + c + "</span></a></li>";
    //            }

    //            strHtml += "</ul></li>";
    //        }
    //    }


    //    strHtml += "</ul></div>";
    //    return strHtml;
    //}


    public DataTable GetMenus(int RoleId, int SchoolId)
    {
       
        objData = new clsData();
        strQuery = "Select GoalId,GoalName,ParntGoalId  from Goal  where  SchoolId=" + SchoolId + "  and ActiveInd='Y' and  ParntGoalId=" + RoleId + " ORDER BY GoalId";
    

        DataTable dtMenu = objData.ReturnDataTable(strQuery, false);
        return dtMenu;
    }

    public TreeView GetMenuByParent(TreeView tvMenus, int SchoolId)
    {

        objData = new clsData();

        strQuery = "Select GoalId,GoalName,ParntGoalId  from Goal  where  SchoolId=" + SchoolId + "  and ActiveInd='Y' and  ParntGoalId is Null ORDER BY GoalId";

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
                    TreeNode node = new TreeNode(dr["GoalName"].ToString(), dr["GoalId"].ToString());
                    node.PopulateOnDemand = true;
                    rootNode.ChildNodes.Add(node);
                }
            }
        }

        tvMenus.ExpandAll();
        return tvMenus;
    }
}
