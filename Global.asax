<%@ Application Language="C#" %>


<script RunAt="server">
    
  
    void Application_Start(object sender, EventArgs e)
    {

    }
    void Application_End(object sender, EventArgs e)
    {
       
    }
    void Session_Start(object sender, EventArgs e)
    {
        Session.Timeout = 300000;

        clsLoginValues objLoginVal = new clsLoginValues();
        ArrayList ar = new ArrayList();

        objLoginVal.IsActiveLogin = ConfigurationManager.AppSettings["IsActiveLogin"].ToString();
        objLoginVal.DomainName = ConfigurationManager.AppSettings["DomainName"].ToString();

        string USServer1 = ConfigurationManager.AppSettings["USServer1"].ToString();
        string USServer2 = ConfigurationManager.AppSettings["USServer2"].ToString();
        string USServer3 = ConfigurationManager.AppSettings["USServer3"].ToString();

        ar.Add(USServer1);
        ar.Add(USServer2);
        ar.Add(USServer3);

        objLoginVal.LoginVals = ar;
        Session["ActiveLogin"] = objLoginVal;
    }

    void Session_End(object sender, EventArgs e)
    {
        Session.Abandon();
    }
    protected void Application_Error(object sender, EventArgs e)
    {
        Exception exc = Server.GetLastError();
        ClsErrorLog errlog = new ClsErrorLog();
        errlog.WriteToLog("Page Name: " + clsGeneral.getPageURL() + "\t" + exc.ToString());
        Server.ClearError();
    }
       
</script>
