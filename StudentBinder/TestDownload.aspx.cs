using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class StudentBinder_TestDownload : System.Web.UI.Page
{
    clsData objData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            BindGrid();
        }
    }



    protected void Upload(object sender, EventArgs e)
    {

        objData = new clsData();
        int index = 0;
        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
        string contentType = FileUpload1.PostedFile.ContentType;

        string[] validFileTypes = {"doc", "xls","docx","pdf" };

        string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile)
        {

        }
        else
        {
            using (Stream fs = FileUpload1.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    index = objData.SaveLessonPlanData(filename, contentType, bytes);

                }
            }
            Response.Redirect(Request.Url.AbsoluteUri);

            BindGrid();
        }
    }


    private void BindGrid()
    {
        objData = new clsData();
        string selQuerry = "SELECT DocId as ID, Name as Name From tbl_Files";
        DataTable dt = objData.ReturnDataTable(selQuerry, false);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
    }

    protected void DownloadFile(object sender, EventArgs e)
    {
        objData = new clsData();
        int id = int.Parse((sender as LinkButton).CommandArgument);
        byte[] bytes = null;
        string fileName = "";
        string contentType = "";       
        int index = 0;

        index =  objData.DownloadLpData(id, out fileName, out contentType, out bytes);      

        fileName = fileName.Replace(' ', '_');

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName.Trim());
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }
}