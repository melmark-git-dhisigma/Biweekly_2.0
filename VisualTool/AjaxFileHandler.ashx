<%@ WebHandler Language="C#" Class="AjaxFileHandler" %>

using System;
using System.Web;
using System.IO;

public class AjaxFileHandler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.Files.Count > 0)
        {
            string folderName = HttpContext.Current.Request.QueryString["type"];
            string id = HttpContext.Current.Request.QueryString["id"];
           
            string path = context.Server.MapPath("~/Repository/"+folderName+"/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            
            
            var file = context.Request.Files[0];

            string ext = Path.GetExtension(file.FileName).Substring(1);
            string filename = Path.Combine(path, id + "." + ext);
            file.SaveAs(filename);

            context.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new { name = file.FileName };
            context.Response.Write(filename);
            context.Response.Write(serializer.Serialize(result));
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}