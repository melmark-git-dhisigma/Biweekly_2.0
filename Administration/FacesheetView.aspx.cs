using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacesheetPA;
using System.Configuration;
using System.Text;
using System.IO;
using System.Threading;

public partial class Administration_FacesheetView : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        var osession = (clsSession)HttpContext.Current.Session["UserSession"];
        string url = "../../../ClientDB/(S(" + osession.SessionID + "))/ProtocolSummary/GetProtocolSummary?StudentId=" + osession.StudentId + "";
        pageFrame1.Attributes["src"] = url;
    }

    
}