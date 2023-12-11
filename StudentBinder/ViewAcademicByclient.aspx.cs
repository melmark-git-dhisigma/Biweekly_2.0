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
 


public partial class StudentBinder_Default : System.Web.UI.Page
{

    public static clsData objData = null;
    
    public string name;
    public string cid;
    public string mis;
    public string sid;
    protected void Page_Load(object sender, EventArgs e)
    {
        objData = new clsData();
        lblData.Visible = false;
        cid = Session["cid"].ToString();
        sid = Session["sid"].ToString();
        mis = Session["mis"].ToString();
        String proc = "[dbo].[DashboardClientAcademic]";
        DataTable dt = objData.ReturnNewAcademicTable(proc, cid, sid, mis);
        //  SqlDataReader dr = cmd.ExecuteReader();
        int i = dt.Rows.Count;
        if (i < 1)
        {
            loadNodata();
        }
        
    }

     public void  loadNodata()
    {
        container.Visible = false;
        lblData.Text = "No Data Available";
        lblData.Visible = true;
        
    }
        
      
     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getAClient(string cid, string sid, string mis)
     {
         objData = new clsData();
         List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
         Dictionary<string, object> row;
         String proc = "[dbo].[DashboardClientAcademic]";

         DataTable dt = objData.ReturnNewAcademicTable(proc, cid, sid, mis);

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
         return json.Serialize(rows);
     }            
    }


    
   

