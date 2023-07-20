<%@ WebHandler Language="C#" Class="AjaxFileHandler2" %>

using System;
using System.Web;
using System.IO;

public class AjaxFileHandler2 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.Files.Count > 0)
        {
            string path = context.Server.MapPath("~/Temp");
            if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
            
            var file=context.Request.Files[0];
            string filename=Path.Combine(path,file.FileName);
            file.SaveAs (filename);
            
            context.Response.ContentType="text/plain";
            var serializer=new System.Web.Script.Serialization.JavaScriptSerializer();
            var result=new {name=file.FileName};
            context.Response.Write(serializer.Serialize(result));
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}