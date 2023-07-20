using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for cls_connection
/// </summary>
public class cls_connection
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
	
	public cls_connection()
	{
        con.Close();
	}
    public SqlConnection Connect()
    {
        if(con.State==ConnectionState.Closed) con.Open();
        return con;
    }
    public SqlConnection CloseConnection()
    {
        if (con.State == ConnectionState.Open) con.Close();
        return con;
    }
}