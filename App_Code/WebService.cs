using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public static System.Data.DataTable MyMethod(int value)
    {
        return GetDataSet(value);
    }

    public static System.Data.DataTable GetDataSet(int value)
    {

        DataTable dt = new DataTable("Author");
        DataRow dr;
        dt.Columns.Add(new DataColumn("Id", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Author", typeof(string)));

        for (int i = 0; i <= 10; i++)
        {
            dr = dt.NewRow();
            dr[0] = i;
            dr[1] = "Author" + i.ToString();
            dt.Rows.Add(dr);
        }
        for (int i = 20; i <= 40; i++)
        {
            dr = dt.NewRow();
            dr[0] = i;
            dr[1] = "Author" + i.ToString();
            dt.Rows.Add(dr);
        }


        DataView dv = new DataView(dt);
        dv.RowFilter = "Id='" + value + "'";



        return dv.Table;
    }
    
}
