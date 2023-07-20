using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


/// <summary>
/// Summary description for clsUser
/// </summary>
public class clsUser
{
    string strQuery = "";
    clsData objData = null;
    bool IsLogin = false;
    int intUserId = 0;
    DataTable dTRoles = null;
    clsSession objSess = null;

	public clsUser()
	{		

	}


    public void Login(string UsernName, string Password,out object objSession,out bool IsUserLogin)
    {

        strQuery = "SELECT UserId FROM [User] WHERE (UserNo = '" + UsernName + "') AND (Password = '" + Password + "')";

        intUserId = Convert.ToInt32(objData.FetchValue(strQuery));
        if (intUserId != 0) IsLogin = true;

        if (IsLogin)
        {
            strQuery= "SELECT Role.RoleId,Role.RoleCode,URole.UserId FROM Role JOIN UserRole URole ON URole.UserId=" + intUserId + " AND URole.RoleId=Role.RoleId";
            dTRoles=objData.ReturnDataTable(strQuery,false);

            if (dTRoles.Rows.Count > 0)
            {
                objSess = new clsSession();

                objSess.RoleId = Convert.ToInt32(dTRoles.Rows[0]["RoleId"]);
                objSess.RoleName = Convert.ToString(dTRoles.Rows[0]["RoleCode"]);
                objSess.LoginId = Convert.ToInt32(dTRoles.Rows[0]["UserId"]);
            }


        }
        else
        {


        }

        IsUserLogin = true;
        objSession = null;
        
    }
}