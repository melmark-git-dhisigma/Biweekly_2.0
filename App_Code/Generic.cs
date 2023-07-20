using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Generic
/// </summary>
public class Generic
{
	public Generic()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void DropdownFill(DropDownList ddl,SqlDataReader ddlDt)
    {
        ddl.DataSource = ddlDt;
        ddl.DataTextField = "name";
        //ddl.DataValueField = "id";
        ddl.DataBind();

    }


}