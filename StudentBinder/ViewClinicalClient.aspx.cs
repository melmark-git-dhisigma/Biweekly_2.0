using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Web.Script.Services;
using System.Diagnostics;

public partial class StudentBinder_ViewClinicalClient : System.Web.UI.Page
{
    public static clsData objData = null;
    public string cid;
    public string sid;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblData.Visible = false;
        cid = Session["cid"].ToString();
        sid = Session["sid"].ToString();
        string str = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        SqlConnection cn = new SqlConnection(str);
        cn.Open();
        SqlCommand cmd = new SqlCommand("[dbo].[DashboardClientClinical]", cn);
        cmd.CommandType = CommandType.StoredProcedure;

        SqlParameter param = new SqlParameter();
        param = cmd.Parameters.Add("@ParamClassid", SqlDbType.VarChar);
        param.Direction = ParameterDirection.Input;
        param.Value = cid;

        param = cmd.Parameters.Add("@ParamStudid", SqlDbType.VarChar);
        param.Direction = ParameterDirection.Input;
        param.Value = sid;


        DataTable dt = new DataTable();
        //  SqlDataReader dr = cmd.ExecuteReader();
        dt.Load(cmd.ExecuteReader());

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        //   GridView1.DataSource = dt;
        //   GridView1.DataBind();
        int i = dt.Rows.Count;
        if (i < 1)
        {
            loadNodata();
        }

    }

    public void loadNodata()
    {
        container.Visible = false;
        lblData.Text = "No Data Available";
        lblData.Visible = true;

    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAClientClinic(string cid, string sid)
    {
        objData = new clsData();
        string str = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
        SqlConnection cn = new SqlConnection(str);
        cn.Open();

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        String proc = "[dbo].[DashboardClientClinical]";

        DataTable dt = objData.ReturnNewTableClinicClient(proc, cid, sid);

        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                row.Add(dc.ColumnName, dr[dc]);
            }
            rows.Add(row);

        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        string dat = json.Serialize(rows);
        Debug.WriteLine(dat);
        return json.Serialize(rows);
    }

        

    
}