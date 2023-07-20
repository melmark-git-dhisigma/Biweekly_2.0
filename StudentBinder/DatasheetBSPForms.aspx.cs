using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Xml;
using System.IO.Packaging;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Net;
using System.Xml.Linq;
using System.Text;
using System.Web.Services;
using Microsoft.Office.Interop.Word;
using System.Threading;


public partial class StudentBinder_DatasheetBSPForms : System.Web.UI.Page
{
    clsData objData = null;
    ClsTemplateSession ObjTempSess = null;
    clsSession Sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Sess = (clsSession)Session["UserSession"];
        if (Sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(Sess.perPage);
            if (flag == false)
            {
                Response.Redirect("~/Administration/Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }

        }
        if (!IsPostBack)
        {

            fillData(Sess.StudentId);
        }
       

    }
    protected void fillData(int studid)
    {
        try
        {
            divMessage.InnerHtml = "";
            objData = new clsData();
            string strQuery = "";
            strQuery = "select ROW_NUMBER() OVER (ORDER BY BSPDoc) as Slno,bsp.BSPDocUrl,bsp.CreatedOn,bsp.BSPDoc as BSPDoc FROM BSPDoc as bsp where bsp.StudentId=" + studid + "";
            System.Data.DataTable Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Dt.Columns.Add("Name", typeof(string));


                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        string name = Dt.Rows[i]["BSPDocUrl"].ToString();
                        string[] ext = name.Split('.');
                        string ext_name = ext[1];
                        if (name != "")
                        {
                            if (name.Length > 50)
                            {
                                name = name.Substring(0, 50) + "....";
                                name += ext_name;
                            }
                        }
                        Dt.Rows[i]["Name"] = name;
                    }
                    grdBSPView.DataSource = Dt;
                    grdBSPView.DataBind();
                }
                else
                    grdBSPView.DataBind();
            }
            else
                divMessage.InnerHtml = clsGeneral.warningMsg("No Data Found");
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    protected void grdBSPView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        string file = Convert.ToString(e.CommandArgument);
        clsData objData = new clsData();
        if (e.CommandName == "view")
        {
            if (Sess != null)
            {
                try
                {
                    HttpResponse Response = HttpContext.Current.Response;
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Byte[] data = (Byte[])objData.FetchValue("SELECT Data FROM BSPDoc WHERE BSPDoc='" + file + "'");
                    string docURL = Convert.ToString(objData.FetchValue("SELECT BSPDocUrl FROM BSPDoc WHERE BSPDoc='" + file + "'"));
                    string contentType = GetContentType(Path.GetExtension(docURL).ToLower().ToString());
                    Response.AddHeader("Content-type", contentType);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + docURL);
                    Response.BinaryWrite(data);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception Ex)
                {

                    throw Ex;
                }
                
            }
        }
        if (e.CommandName == "Edit")
        {
            try
            {
                if (Sess != null)
                {
                    int studid = Sess.StudentId;
                    deleteDocument(file);
                    fillData(studid);
                   
                    grdBSPView.DataBind();
                    
                }
                
                
                

            }
            catch (Exception Ex)
            {

                throw Ex;
            }

        }
        

    }
    protected void deleteDocument(string fileName)
    {
        try
        {
            clsData objdata = new clsData();
            string Query = "DELETE FROM BSPDoc WHERE BSPDoc='" + fileName + "'";
            int a = objdata.Execute(Query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private string GetContentType(string extension)
    {
        try
        {
            string ContentType = "";
            switch (extension)
            {
                case ".txt":
                    ContentType = "text/plain";
                    break;
                case ".doc":
                    ContentType = "application/msword";
                    break;
                case ".docx":
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".pdf":
                    ContentType = "application/pdf";
                    break;
                case ".xls":
                    ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    ContentType = "application/vnd.ms-excel";
                    break;
            }
            return ContentType;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void btUpload_Click(object sender, EventArgs e)
    {
        try
        {
            int StId = Sess.StudentId;

            if (StId != 0)
            {

                clsData objData = new clsData();
                divMessage.InnerHtml = "";
                clsDocumentasBinary objBinary = new clsDocumentasBinary();
                if (fupDoc.HasFile)
                {

                    string ext = System.IO.Path.GetExtension(fupDoc.FileName);
                    string name = System.IO.Path.GetFileNameWithoutExtension(fupDoc.FileName);
                    if (name != "")
                    {
                        if (name.Length > 50)
                        {
                            name = name.Substring(0, 50) + "....";
                            name += ext;
                        }
                    }
                    if (ext.ToLower() == ".txt" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg" || ext.ToLower() == ".png" || ext.ToLower() == ".docx" || ext.ToLower() == ".doc" || ext.ToLower() == ".pdf" || ext.ToLower() == ".csv" || ext.ToLower() == ".xlsx" || ext.ToLower() == ".xls")
                    {

                        string filename = System.IO.Path.GetFileName(fupDoc.FileName);
                        HttpPostedFile myFile = fupDoc.PostedFile;
                        int nFileLen = myFile.ContentLength;
                        byte[] myData = new byte[nFileLen];
                        myFile.InputStream.Read(myData, 0, nFileLen);
                        int binId = SaveDocument(Sess.SchoolId, StId, filename, myData, Sess.LoginId);
                        //string strquerry = "INSERT INTO BSPDoc(SchoolId,StdtIEPId,BSPDocUrl,Data,CreatedBy,CreatedOn) values(" + sess.SchoolId + "," + sess.IEPId + ",'" + filename + "',convert(varbinary(max),'"+myData+"')," + sess.LoginId + ",GETDATE())";
                        //int docid = objData.ExecuteWithScope(strquerry);
                        fillData(StId);
                        
                    }
                    else
                    {
                        divMessage.InnerHtml = clsGeneral.warningMsg("Invalid file format...");
                        

                    }
                }
                else
                {
                    divMessage.InnerHtml = clsGeneral.warningMsg("Please select file...");
                    
                }
            }
            
        }
        catch (Exception ex)
        {
            lMsg.ForeColor = System.Drawing.Color.Red;
            lMsg.Text = "Error:" + ex.Message.ToString();
        }
    }
    private int SaveDocument(int SchoolId,int studid, string filename, byte[] contents, int UserId)
    {
        clsData objData = new clsData();
        int BinaryId = 0;
        try
        {
            string query = "INSERT INTO BSPDoc(SchoolId,StudentId,BSPDocUrl,Data,CreatedBy,CreatedOn)VALUES (@SchoolId,@stid,@BSPDocUrl,@Data,@CreatedBy,@CreatedOn) \nSELECT SCOPE_IDENTITY()";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = objData.Open();
                cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                cmd.Parameters.AddWithValue("@stid", studid);
                cmd.Parameters.AddWithValue("@BSPDocUrl", filename);
                cmd.Parameters.AddWithValue("@Data", contents);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                BinaryId = Convert.ToInt16(cmd.ExecuteScalar());

            }


        }
        catch (Exception ex)
        {

            throw ex;
        }
        return BinaryId;
    }
    protected void grdBSPView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
        grdBSPView.DataBind();

    }
    protected void grdBSPView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBSPView.PageIndex = e.NewPageIndex;
        int studid = Sess.StudentId;
        if (studid != 0)
        {
            fillData(studid);
            
        }

    }

}