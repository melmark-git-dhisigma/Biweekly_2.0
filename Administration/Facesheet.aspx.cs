using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Facesheet;
using System.Configuration;
using System.Text;
using System.IO;
public partial class Administration_Facesheet : System.Web.UI.Page
{
    FacesheetClass objFacesheet = null;
    clsData objData = null;
    clsSession sess = null;
    DataTable Dt = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public DataTable getStudentDetailsNE(string Type)
    {
        objData = new clsData();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            Dt = objData.ExecuteStudentsDetails("StudentMoreDetailsNE", sess.SchoolId, sess.StudentId, Type);
        }
        return Dt;
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string[] args = new string[10];
        string connection = ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString();
        string FileLocation = "";
        FileLocation = Server.MapPath("~\\Administration\\FacesheetNE");
        string qry = "SELECT LastName+','+ FirstName AS Name,format(ModifiedOn,'MM/yyyy') AS ModifiedOn,format(ModifiedOn,'MM/dd/yyyy') AS UpdatedOn,format(BirthDate,'MM/dd/yyyy') AS BirthDate FROM StudentPersonal WHERE StudentPersonalId='" + sess.StudentId + "'AND StudentType='Client'";
        DataTable dt = objData.ReturnDataTable(qry,false);
        string studentName = dt.Rows[0]["Name"].ToString();
        string modifiedDate = dt.Rows[0]["ModifiedOn"].ToString();
        string UpdatedOn = dt.Rows[0]["UpdatedOn"].ToString();
        string dateOfBirth = dt.Rows[0]["BirthDate"].ToString();
        args[0] = "NE";
        args[1] = connection;
        args[2] = FileLocation;
        args[3] = sess.StudentId.ToString();
        args[4] = sess.SchoolId.ToString();
        args[5] = sess.LoginId.ToString();
        args[6] = studentName;
        args[7] = modifiedDate;
        args[8] = UpdatedOn;
        args[9] = dateOfBirth;
        objFacesheet = new FacesheetClass();
        string path = FileLocation + "\\Temp";
        //liju Changed function
        //string HtmlData = objFacesheet.Main(args);

        string newPath = objFacesheet.ProcessTemplateFile(args);
        //string newPath = "";
        ////liju
        //HtmlData = HtmlData.Replace("’", "'");
        //HtmlData = HtmlData.Replace("…", "...");
        //HtmlData = HtmlData.Replace("‘", "'");
        //HtmlData = HtmlData.Replace("·", "- ");
        //HtmlData = HtmlData.Replace("“", "'");
        //HtmlData = HtmlData.Replace("”", "'");
        //HtmlData = HtmlData.Replace("�", "'");
        //CreateDocument(HtmlData, path, out newPath);
        ////liju
        //System.Threading.Thread.Sleep(2000);
        ExportToWord("", newPath);
    }

    private void ExportToWord(string htmlData, string path)
    {
        try
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                Response.ContentType = "application/msword";
                Response.AddHeader("Content-Disposition", "Attachment; filename= Facesheet.doc");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.TransmitFile(file.FullName);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    //Create document method
    private void CreateDocument(string HtmlData, string path, out string newpath)
    {

        byte[] byteData = null;

        string temppath = path;
        Guid g = Guid.NewGuid();

        string ids = g.ToString();
        ids = ids.Replace("-", "");
        //liju
        path = path + "\\temphtml" + ids + ".mht";
        newpath = temppath + "\\temphtml" + ids + ".docx";
        FileInfo f1 = new FileInfo(path);
        if (!f1.Exists)
        {
            using (FileStream fs = File.Create(path))
            {
                //Byte[] info = new UTF8Encoding(true).GetBytes(HtmlData);
                // Add some information to the file.
                //liju
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(HtmlData);
                    sw.Flush();
                }
                
                //fs.Write(info, 0, info.Length);
            }

        }
        string input = path;
        string output = newpath;
        try
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;
            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault;
            object oFileShare = true;
            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            // Make this document the active document.
            oDoc.Activate();
            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            //using (var fs = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{
            //    fs.Close();
            //}
        }
        catch (IOException ex)
        {
            throw ex;
        }

    }
}