using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ClassUser
/// </summary>
public class ClassUser
{
    //DataClass c1 = new DataClass();

    private string m_FirstName, m_LastName, m_UserName, m_Passwrd, m_Email;
	public ClassUser()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //public ClassUser(Int32 UserId)
    //{
    //    Load(UserId);
    //}

    private void Init()
    {
        this.m_Email = "";
        this.m_FirstName = "";
        this.m_LastName = "";
        this.m_Passwrd = "";
        this.m_UserName = "";
    }

    //public DataTable CheckRole(Int32 UserId)
    //{
    //    DataTable oDtRole = new DataTable();
    //    if (UserId != 0)
    //    {
    //        DataClass oData = new DataClass();
    //        DataSet oDsRoles = oData.ExecuteDataSet("SELECT Role.RoleCode,Role.RoleId,URole.UserId,URole.RoleId FROM Role JOIN UserRole URole ON URole.UserId=" + UserId + " AND URole.RoleId=Role.RoleId");
            
    //        if (oDsRoles.Tables.Count>0)
    //        {
    //            oDtRole = oDsRoles.Tables[0];
    //        }
    //    }
    //    return oDtRole;
    //}

    //public DataSet loadRoles()
    //{
    //    DataClass oData = new DataClass();
    //    DataSet oDsRoles = oData.ExecuteDataSet("SELECT * FROM Role");
    //    return oDsRoles;
    //}

    //public DataSet loadRoles(Int32 UserId)
    //{
    //    DataClass oData = new DataClass();
    //    DataSet oDsRoles = oData.ExecuteDataSet("SELECT Role.RoleCode,Role.RoleId,URole.UserId,URole.RoleId FROM Role JOIN UserRole URole ON URole.UserId=" + UserId + " AND URole.RoleId=Role.RoleId");
    //    return oDsRoles;
    //}

    //public Int32 Logon(string UsrNam, string UsrPwd)
    //{
    //    Int32 UserId = 0;
    //    //ClassEncrypt oEnc = new ClassEncrypt();
    //    DataSet oDs;
    
   

    //    oDs = Db.SqlQuery(string.Format("SELECT UserId FROM [User] WHERE (UserNo = '{0}') AND (Password = '{1}')", UsrNam, UsrPwd));


    //    if (Db.HasRecord(oDs) == true)
    //    {
            
    //        //	User has been found.

    //        UserId = Convert.ToInt32(oDs.Tables[0].Rows[0]["UserId"].ToString());


    //    }
    


    //    return UserId;

    //}
    //public DataSet ShowDropDown(string selQry)
    //{
    //    return Db.SqlQuery(selQry);
    //}

    public int Save(string sqlQry, int UserId)
    {
        int nRetVal = 0;


        //nRetVal = Db.SaveUser(UserId, 9, 1, this.m_FirstName, this.m_LastName, this.m_UserName, this.m_Passwrd);
        //
        DataClass oData = new DataClass();
        System.Data.SqlClient.SqlCommand oCmd = new SqlCommand();

        oCmd.Connection = oData.Connect();
        oCmd.CommandType = System.Data.CommandType.Text;

        StringBuilder oBld = new StringBuilder("");

        if (UserId == 0)
        {
            //	New User.
            oCmd.CommandText = sqlQry;
            oCmd.Parameters.AddWithValue("@Address", 9);
            oCmd.Parameters.AddWithValue("@School", 1);
            oCmd.Parameters.AddWithValue("@Ufname", m_FirstName);
            oCmd.Parameters.AddWithValue("@Ulname", m_LastName);
            oCmd.Parameters.AddWithValue("@UNo", m_UserName);
            oCmd.Parameters.AddWithValue("@Passwrd", m_Passwrd);
            oCmd.Parameters.AddWithValue("@Createdby", 1);
            oCmd.Parameters.AddWithValue("@Createdon", DateTime.Now);

            nRetVal = Convert.ToInt32(oCmd.ExecuteScalar());


        }
        else
        {
            //	Update existing User.

            oCmd.CommandText = sqlQry;

            oCmd.Parameters.AddWithValue("@Ufname", m_FirstName);
            oCmd.Parameters.AddWithValue("@Ulname", m_LastName);
            oCmd.Parameters.AddWithValue("@UNo", m_UserName);
            oCmd.Parameters.AddWithValue("@Modifiedby", 1);
            oCmd.Parameters.AddWithValue("@Modifiedon", DateTime.Now);
            oCmd.Parameters.AddWithValue("@UsrID", UserId);
            oCmd.ExecuteNonQuery();

            nRetVal = UserId;
        }



        //

        oData.CloseConnection();
        return nRetVal;
    }

    public void MapRoletoUser(string sqlQry, Int32 UserId, Int32 RoleId)
    {
        //Db.MapRoletoUser(UserId, RoleId);

        DataClass oData = new DataClass();
        System.Data.SqlClient.SqlCommand oCmd = new SqlCommand();

        oCmd.Connection = oData.Connect();
        oCmd.CommandType = System.Data.CommandType.Text;
        oCmd.CommandText = sqlQry;

        oCmd.Parameters.AddWithValue("@School", 1);
        oCmd.Parameters.AddWithValue("@Desc", "Role Description");
        oCmd.Parameters.AddWithValue("@Userid", UserId);
        oCmd.Parameters.AddWithValue("@Roleid", RoleId);
        oCmd.Parameters.AddWithValue("@Createdby", 1);
        oCmd.Parameters.AddWithValue("@Createdon", DateTime.Now);

        oCmd.ExecuteNonQuery();

        oData.CloseConnection();
    }

    //public void DeleteProfForUser(Int32 UserId)
    //{
    //    Db.SqlAction(string.Format("DELETE FROM UserRole WHERE (UserId = {0})", UserId));
    //}
    //public void DeleteUser(Int32 UserId)
    //{
    //    Db.SqlAction(string.Format("DELETE FROM [User] WHERE (UserId = {0})", UserId));
    //}


    public string FirstName
    {
        get
        {
            return m_FirstName;
        }
        set
        {
            m_FirstName = value;
        }
    }

    public string LastName
    {
        get
        {
            return m_LastName;
        }
        set
        {
            m_LastName = value;
        }
    }

    public string UserName
    {
        get
        {
            return m_UserName;
        }
        set
        {
            m_UserName = value;
        }
    }

    public string Password
    {
        get
        {
            return m_Passwrd;
        }
        set
        {
            m_Passwrd = value;
        }
    }

    public string Email
    {
        get
        {
            return m_Email;
        }
        set
        {
            m_Email = value;
        }
    }
}