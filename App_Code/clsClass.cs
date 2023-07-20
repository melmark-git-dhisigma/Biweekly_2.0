using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsClass
/// </summary>
public class clsClass
{
    clsData objData = null;
    string strQuery = "";

    public clsClass()
    {

    }
    public bool StudentClassExit(int StdId, int ClassId)
    {
        objData = new clsData();
        bool val = objData.IFExists("Select StdtId from StdtClass where StdtId=" + StdId + " and ClassId=" + ClassId + " ");
        return val;
    }
    public string GetADClassStd(int StdId, int ClassId)
    {
        objData = new clsData();
        string val = objData.FetchValue("Select ActiveInd from StdtClass where StdtId=" + StdId + " and ClassId=" + ClassId + " ").ToString();
        return val;
    }

    public bool UserClassExit(int UserId, int ClassId)
    {
        objData = new clsData();
        bool val = objData.IFExists("Select [UserId] from [dbo].[UserClass] where UserId=" + UserId + " and ClassId=" + ClassId + " ");
        return val;
    }
    public bool UserRoleExit(int UserId)
    {
        objData = new clsData();
        bool val = objData.IFExists("Select [UserId] from [dbo].[UserRoleGroup] where UserId=" + UserId + " ");
        return val;
    }
    public string GetADClass(int UserId, int ClassId)
    {
        objData = new clsData();
        string val = objData.FetchValue("Select ActiveInd from UserClass where UserId=" + UserId + " and ClassId=" + ClassId + " ").ToString();
        return val;
    }
    public string GetADRole(int UserId, int RoleGpid)
    {
        objData = new clsData();
        string val = objData.FetchValue("Select ActiveInd from UserRoleGroup where UserId=" + UserId + " and RoleGroupId=" + RoleGpid + " ").ToString();
        return val;
    }
    public void fillUserCheckBox(CheckBoxList chkUser,int ClassId, int SchoolId)
    {
        objData = new clsData();
        strQuery = "Select UserId from dbo.UserClass";
        strQuery += " WHERE  ActiveInd = 'A' AND classId=" + ClassId + " AND  SchoolId = " + SchoolId + "";


        DataTable Dt = objData.ReturnDataTable(strQuery, false);


        if (Dt != null && Dt.Rows.Count > 0)
        {
            string[] s = new string[Dt.Rows.Count];
            for (int h = 0; h < Dt.Rows.Count; h++)
            {
                s[h] = Dt.Rows[h]["UserId"].ToString();

            }
            int length = s.Length;
            for (int i = 0; i <= s.Length - 1; i++)
            {

                for (int j = 0; j <= chkUser.Items.Count - 1; j++)
                {
                    if (chkUser.Items[j].Value == s[i])
                    {
                        chkUser.Items[j].Selected = true;
                        break;
                    }
                }
            }
           
        }

    }

    public void fillStudentCheckBox(CheckBoxList chkStudent, int ClassId, int SchoolId)
    {
        objData = new clsData();
        strQuery = "Select StdtId from StdtClass";
        strQuery += " WHERE  ActiveInd = 'A' AND classId=" + ClassId + " AND  SchoolId = " + SchoolId + "";


        DataTable Dt = objData.ReturnDataTable(strQuery, false);


        if (Dt != null && Dt.Rows.Count > 0)
        {
            string[] s = new string[Dt.Rows.Count];
            for (int h = 0; h < Dt.Rows.Count; h++)
            {
                s[h] = Dt.Rows[h]["StdtId"].ToString();

            }
            int length = s.Length;
            for (int i = 0; i <= s.Length - 1; i++)
            {

                for (int j = 0; j <= chkStudent.Items.Count - 1; j++)
                {
                    if (chkStudent.Items[j].Value == s[i])
                    {
                        chkStudent.Items[j].Selected = true;
                        break;
                    }
                }
            }

        }

    }
}