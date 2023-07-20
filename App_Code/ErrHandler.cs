using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;
using System.Text;

/// <summary>
/// Summary description for ErrHandler
/// </summary>
public class ErrHandler
{
	public ErrHandler()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// Handles error by accepting the error message 
    /// Displays the page on which the error occured
    public static void WriteError(Exception e, string path)
    {
        StringBuilder errorDetails = new StringBuilder();
        errorDetails.AppendLine("Message : " + e.Message.ToString() + "\n");
        errorDetails.AppendLine("Exception : " + e.GetType().ToString() + "\n");

        if (e.TargetSite != null)
            errorDetails.AppendLine("Targetsite : " + e.TargetSite.ToString() + "\n");

        errorDetails.AppendLine("Source : " + e.Source + "\n");
        errorDetails.AppendLine("StackTrace : " + e.StackTrace.ToString().Replace(Environment.NewLine, "\n") + "\n");

        errorDetails.AppendLine("Data count : " + e.Data.Count.ToString() + "\n");
        errorDetails.AppendLine("\n");
        errorDetails.AppendLine("Exception : " + e.ToString().Replace(Environment.NewLine, "\n") + "\n");
        
        using (StreamWriter sWriter = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
        {
            sWriter.WriteLine("\r\nLog Entry : ");
            sWriter.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                          ". Error Message:" + errorDetails.ToString();
            sWriter.WriteLine(err);
            sWriter.WriteLine("__________________________");
            sWriter.Flush();
            sWriter.Close();
        }
        
    }
}
