using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;
using System.Drawing;

public partial class Admin_CSVupload : System.Web.UI.Page
{
    //Declare Variable (property)
    string currFilePath = string.Empty; //File Full Path
    string currFileExtension = string.Empty;  //File Extension
    clsSession sess = null;
    static System.Data.DataTable dtCSV = new System.Data.DataTable();
    DataClass oData = new DataClass();
    //static string[] error = new string[50];
    List<String> error = new List<String>();
    static int index = 0;
    //Page_Load Event, Register Button Click Event

    protected void Page_Load(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(sess.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        if (!IsPostBack)
        {
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btn_Save.Visible = false;
                btnRead.Visible = false;
            }
            else
            {
                btn_Save.Visible = true;
                btnRead.Visible = true;
            }
        }
    }
    //Button Click Event  

    protected void btnRead_Click1(object sender, EventArgs e)
    {
        if (fileSelect.HasFile)
        {
            //lblMsg.Visible = false;
            Upload();  //Upload File Method
            if (this.currFileExtension == ".txt")
            {
                dtCSV = ReadExcelWithStream(currFilePath);  //Read .CSV File
                gdSample.DataSource = dtCSV;
                gdSample.DataBind();

                if (gdSample.Rows.Count > 0)
                {
                    btn_Save.Visible = true;
                }
                else
                    btn_Save.Visible = false;
            }
            else
            {
                File.Delete(currFilePath);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('Invalid File Format. Select .txt(Tab Delimited) files only');", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('Select a File');", true);
        }
    }

    ///<summary>
    ///Upload File to Temporary Category
    ///</summary>
    private void Upload()
    {


        HttpPostedFile file = this.fileSelect.PostedFile;
        string fileName = file.FileName;
        //string tempPath = System.IO.Path.GetTempPath();   //Get Temporary File Path
        if (Directory.Exists(Server.MapPath("CSVDownloads\\temp")) == false)    //check whether the temp folder exists or not
        {
            Directory.CreateDirectory(Server.MapPath("CSVDownloads\\temp"));    //if not, create a temp folder..
        }

        string tempPath = Server.MapPath("CSVDownloads\\temp\\");
        fileName = System.IO.Path.GetFileName(fileName); //Get File Name (not including path)

        this.currFileExtension = System.IO.Path.GetExtension(fileName);   //Get File Extension
        if (this.currFileExtension == ".csv")
            fileName = "file_" + sess.LoginId.ToString() + ".csv"; //if the file is csv, then change the file name to the user id for later reference.....
        this.currFilePath = tempPath + fileName; //Get File Path after Uploading and Record to Former Declared Global Variable
        file.SaveAs(this.currFilePath);  //Upload
    }
    private System.Data.DataTable ReadExcelWithStream(string path)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        bool isDtHasColumn = false;   //Mark if DataTable Generates Column
        StreamReader reader = new StreamReader(path, System.Text.Encoding.Default);  //Data Stream
        while (!reader.EndOfStream)
        {
            string message = reader.ReadLine();
            string[] splitResult = message.Split(new char[] { '\t' }, StringSplitOptions.None);  //Read One Row and Separate by Comma, Save to Array

            if (splitResult.Length > 0)
            {
                string[] colNames = splitResult[0].Split(',');

                DataRow row = dt.NewRow();
                for (int i = 0; i < colNames.Length; i++)
                {
                    if (!isDtHasColumn) //If not Generate Column
                    {
                        dt.Columns.Add(colNames[i], typeof(string));
                    }
                    else
                        row[i] = colNames[i];
                }
                if (isDtHasColumn == true)
                    dt.Rows.Add(row);  //Add Row
                isDtHasColumn = true;  //Mark the Existed Column after Read the First Row, Not Generate Column after Reading Later Rows
            }
        }
        reader.Close();
        return dt;
    }
    protected bool Validation(DataRow drStudnt, int rowPositn)
    {

        const string ddmm = "MM-DD-YYYY";
        //StudentNbr	SASID	StudentFname	StudentLname	Grade	Gender	JoinDt	DOB	ActiveInd	Address1	Address2	
        //Address3	City	State	Country	Homephone	Mobile	Email	Zip	Dist_Code	SchoolId
        bool valid = true;
        double result = 0;
        try
        {
            if (drStudnt["StudentNbr"].ToString().Length <= 0)
            {
                valid = false;

                //error[index] = "Null value not allowed for StudentNbr column. Error in Row " + rowPositn.ToString() + ", column StudentNbr";
                error.Add("Null value not allowed for StudentNbr column. Error in Row " + rowPositn.ToString() + ", column StudentNbr");
                index++;
            }

            if (drStudnt["StudentFname"].ToString().Length <= 0)
            {
                valid = false;
                //error[index] = "Null value not allowed for StudentFname column. Error in Row " + rowPositn.ToString() + ", column StudentFname";
                error.Add("Null value not allowed for StudentFname column. Error in Row " + rowPositn.ToString() + ", column StudentFname");
                index++;
            }

            if (drStudnt["ActiveInd"].ToString().Length <= 0)
            {
                valid = false;
                //error[index] = "Null value not allowed for StudentFname column. Error in Row " + rowPositn.ToString() + ", column StudentFname";
                error.Add("Null value not allowed for ActiveInd column. Error in Row " + rowPositn.ToString() + ", column ActiveInd");
                index++;
            }
            if (drStudnt["StudentLname"].ToString().Length <= 0)
            {
                valid = false;
                //error[index] = "Null value not allowed for StudentLname column. Error in Row " + rowPositn.ToString() + ", column StudentLname";
                error.Add("Null value not allowed for StudentLname column. Error in Row " + rowPositn.ToString() + ", column StudentLname");
                index++;
            }
            if (drStudnt["Homephone"].ToString() != "" && clsGeneral.IsItValidPhone(drStudnt["Homephone"].ToString().Trim()) == false)
            {
                valid = false;
                error.Add("Invalid Phone Number.Phone Number Must Be Entered As: (xxx)xxx-xxxx.  Error in Row " + rowPositn.ToString() + ", column Homephone");
                index++;
            }
            if (drStudnt["Mobile"].ToString() != "")
            {
                if (clsGeneral.IsItValidPhone(drStudnt["Mobile"].ToString().Trim()) == false)
                {
                    valid = false;
                    //error[index] = "Invalid Mobile Number. Error in Row " + rowPositn.ToString() + ", column Mobile";
                    error.Add("Invalid Mobile Number. Error in Row " + rowPositn.ToString() + ", column Mobile");
                    index++;
                }
            }

            if (double.TryParse(drStudnt["Zip"].ToString(), out result) == false)
            {
                valid = false;
                //error[index] = "Invalid Zip Code. Error in Row " + rowPositn.ToString() + ", column Zip";
                error.Add("Invalid Zip Code. Error in Row " + rowPositn.ToString() + ", column Zip");
                index++;
            }
            if (drStudnt["Email"].ToString() != "")
            {
                if (IsValidEmail(drStudnt["Email"].ToString()) == false)
                {
                    valid = false;
                    //error[index] = "Invalid Email. Error in Row " + rowPositn.ToString() + ", column Email";
                    error.Add("Invalid Email. Error in Row " + rowPositn.ToString() + ", column Email");
                    index++;
                }
            }
            if (drStudnt["SchoolId"].ToString().Length <= 0)
            {
                valid = false;
                //error[index] = "Null value not allowed for SchoolName column. Error in Row " + rowPositn.ToString() + ", column SchoolName";
                error.Add("Null value not allowed for SchoolName column. Error in Row " + rowPositn.ToString() + ", column SchoolName");
                index++;
            }
            if (drStudnt["JoinDt (" + ddmm + ")"].ToString().Length > 0)
            {
                valid = ValidateDate(drStudnt["JoinDt (" + ddmm + ")"].ToString());
                if (!valid) error.Add("Invalid Joining Date. Error in Row " + rowPositn.ToString() + ", column JoinDt");
                index++;
            }
            if (drStudnt["DOB  (" + ddmm + ")"].ToString().Length > 0)
            {
                valid = ValidateDate(drStudnt["DOB  (" + ddmm + ")"].ToString());
                if (!valid) error.Add("Invalid Date of Birth. Error in Row " + rowPositn.ToString() + ", column DOB");
                index++;
            }
            //if (drStudnt["City"].ToString().Length <= 0)
            //{
            //    valid = false;
            //    //error[index] = "Null value not allowed for SchoolName column. Error in Row " + rowPositn.ToString() + ", column SchoolName";
            //    error.Add("Null value not allowed for City column. Error in Row " + rowPositn.ToString() + ", column City");
            //    index++;
            //}
            //if (drStudnt["State"].ToString().Length <= 0)
            //{
            //    valid = false;
            //    //error[index] = "Null value not allowed for StudentLname column. Error in Row " + rowPositn.ToString() + ", column StudentLname";
            //    error.Add("Null value not allowed for State column. Error in Row " + rowPositn.ToString() + ", column State");
            //    index++;
            //}
            //if (drStudnt["Country"].ToString().Length <= 0)
            //{
            //    valid = false;
            //    //error[index] = "Null value not allowed for StudentLname column. Error in Row " + rowPositn.ToString() + ", column StudentLname";
            //    error.Add("Null value not allowed for Country column. Error in Row " + rowPositn.ToString() + ", column Country");
            //    index++;
            //}
            if (drStudnt["Dist_Code"].ToString().Length <= 0)
            {
                valid = false;
                //error[index] = "Null value not allowed for StudentLname column. Error in Row " + rowPositn.ToString() + ", column StudentLname";
                error.Add("Null value not allowed for Dist_Code column. Error in Row " + rowPositn.ToString() + ", column StudentLname");
                index++;
            }
            //
            //   
            return valid;
        }
        catch (Exception e)
        {
            return valid = false;
            throw e;
        }
    }
    bool invalid = false;
    private bool ValidateDate(string stringDateValue)
    {
        stringDateValue = stringDateValue.Replace("-", "/");
        try
        {
            DateTime dateTime = DateTime.ParseExact(stringDateValue, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return true;
        }
        catch (Exception Ex)
        {
            return false;
            throw Ex;
        }
    }
    public bool IsValidEmail(string strIn)
    {
        invalid = false;
        if (String.IsNullOrEmpty(strIn))
            return false;
        // Use IdnMapping class to convert Unicode domain names.
        strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper);
        if (invalid)
            return false;
        // Return true if strIn is in valid e-mail format.
        return Regex.IsMatch(strIn,
               @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
               RegexOptions.IgnoreCase);
    }
    private string DomainMapper(Match match)
    {
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();
        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException Ex)
        {
            invalid = true;
            throw Ex;
        }
        return match.Groups[1].Value + domainName;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dt"></param>
    private void SavetoDatabase(System.Data.DataTable dt)
    {
        sess = (clsSession)Session["UserSession"];
        int rowCount = 1;
        try
        {
            foreach (DataRow dr in dt.Rows)
            {
                bool valid = Validation(dr, rowCount);
                //rowCount++;
                //Find the School ID
                string selSchool = "SELECT COUNT(SchoolId) FROM School WHERE SchoolId='" + dr["SchoolId"].ToString() + "'";
                int checkSchool = oData.ExecuteScalar(selSchool);
                int schoolAddrId = 0, schoolId = 0;
                if (checkSchool == 0)
                {
                    error.Add("School does not exist. Invalid SchoolName . Error in Row " + rowCount.ToString() + ", column SchoolId");
                    index++;
                    valid = false;
                }
                else
                {
                    schoolId = Convert.ToInt32(dr["SchoolId"].ToString());
                }
                string selCountry = "SELECT LookupId FROM LookUp WHERE LookupName='" + dr["Country"].ToString() + "'";
                int checkCountry = oData.ExecuteScalar(selCountry);
                if (checkCountry == 0)
                {
                    error.Add("Country does not exist. Invalid Country " + dr["Country"].ToString() + ". Error in Row " + rowCount.ToString() + ", column Country");
                    index++;
                    valid = false;
                }//SELECT LookupId FROM LookUp WHERE LookupName='" + dr["State"].ToString() + "' AND LookupType='State'
                string selState = "SELECT LookupId FROM LookUp WHERE LookupName='" + dr["State"].ToString() + "' AND LookupType='State'";

                int checkState = oData.ExecuteScalar(selState);
                if (checkState == 0)
                {
                    error.Add("State does not exist. Invalid State " + dr["State"].ToString() + ". Error in Row " + rowCount.ToString() + ", column State");
                    index++;
                    valid = false;
                }

                if (clsGeneral.IsExit("StudentNbr", "Student", dr["StudentNbr"].ToString().Trim()) == true)
                {
                    error.Add("Student Number Already Existed...!  Error in Row " + rowCount.ToString() + ", column StudentNbr");
                    index++;
                    valid = false;
                }




                if (valid == false)
                    continue;
                else
                {
                    string insStudAddr = "INSERT INTO Address(AddressLine1,AddressLine2,AddressLine3,City,State,Country,HomePhone,Mobile,Email,CreatedBy,CreatedOn,Zip)" +
                                 " VALUES('" + dr["Address1"] + "','" + dr["Address2"] + "','" + dr["Address3"] + "','" + dr["City"] + "','" + checkState + "','" + checkCountry + "'" +
                                 ",'" + dr["Homephone"] + "','" + dr["Mobile"] + "','" + dr["Email"] + "'," + sess.LoginId + ",GETDATE(),'" + dr["Zip"] + "')\r\n" +
                                 "SELECT SCOPE_IDENTITY()";
                    int studAddrId = oData.ExecuteScalar(insStudAddr);
                    string insStudent = "INSERT INTO Student(AddressId,SchoolId,DistrictCode,StudentNbr,SASID,StudentFname,StudentLname,GradeLevel,ResidenceInd,CreatedBy,CreatedOn,Gender,JoinDt,DOB,ActiveInd)" +
                        " VALUES(" + studAddrId + "," + schoolId + ",'" + dr["Dist_Code"] + "','" + dr["StudentNbr"] + "','" + dr["SASID"] + "','" + dr["StudentFname"] + "','" + dr["StudentLname"] + "','" + dr["Grade"] + "',0," +
                        "" + sess.LoginId + ",GETDATE(),'" + dr["Gender"] + "','" + dr["JoinDt (MM-DD-YYYY)"].ToString() + "','" + dr["DOB  (MM-DD-YYYY)"].ToString() + "','" + dr["ActiveInd"] + "')\r\nSELECT SCOPE_IDENTITY()";
                    int studId = oData.ExecuteScalar(insStudent);
                }
                rowCount++;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (dtCSV.Rows.Count == 0)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Student Data not Uploaded..");
                return;
            }
            SavetoDatabase(dtCSV);
            string err = "";
            if (File.Exists(Server.MapPath("CSVDownloads\\temp\\file_" + sess.LoginId.ToString() + ".csv")))
                File.Delete(Server.MapPath("CSVDownloads\\temp\\file_" + sess.LoginId.ToString() + ".csv"));
            if (error.Count > 0)
            {
                for (int count = 0; count < error.Count; count++)
                {
                    int errNumbr = count + 1;
                    if (error[count] != null)
                        err = err + "\\n" + errNumbr + "." + error[count];
                }
                if (err.Length > 0)
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Errors\\n\\n" + err + "\\n\\n All other Rows are successfully Uploaded');", true);
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Student Data Successfully Uploaded..");
                //Response.Clear();
                //Response.Redirect("StudentMenu.aspx",false);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }

            index = 0;
            error = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string strURL = "~/Administration/CSVDownloads/StudentList.txt";
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"StudentList.txt\"");
            byte[] data = req.DownloadData(Server.MapPath(strURL));
            response.BinaryWrite(data);
            response.End();
            // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('File Successfully Downloaded...Desktop');", true);
            //tdMsg.InnerHtml =clsGeneral.sucessMsg("File Successfully downloaded on your desktop");
            //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('File Successfully Downloaded On Your Desktop');", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
}