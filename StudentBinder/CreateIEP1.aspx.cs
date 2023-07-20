using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Xml.Linq;

public partial class StudentBinder_CreateIEP1 : System.Web.UI.Page
{
    public static clsData objData = null;
    public static clsSession sess = null;
    string strQuery = "";
    DataTable Dt = null;
    static int intIEPId = 0;
    static bool Disable = false;
    static string version;
    string active_pages = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        //sess.StudentId = (Session["StudentId"] != null && Session["StudentId"].ToString() != "") ? Convert.ToInt32(Session["StudentId"].ToString()) : 0;


        if (!IsPostBack)
        {
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSubmitIEP1.Visible = false;
            }
            else
            {
                btnSubmitIEP1.Visible = true;
            }
            if (sess.SessionID != null)
            {
                active_pages = Request.QueryString["active"];
                //getDetails();
                if (sess.IEPId != 0)
                {
                    Fill();
                }
            }
            else
            {
                active_pages = "1,4,6";
            }

            ViewAccReject();
        }


    }

    public int getIepIdFromStudentId()
    {

        object pendstatus = null;
        object IepStatus = null;
        object IepId = null;
        int IEP_id = 0;
        clsData oData = new clsData();
        sess = (clsSession)Session["UserSession"];
        sess.IEPId = 0;
        if (oData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
        {
            pendstatus = oData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                IepStatus = oData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
            if ((IepStatus.ToString() == "Approved") || (IepStatus.ToString() == "Expired"))
            {
                IEP_id = 0;
            }

            else
            {
                IepId = oData.FetchValue("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ");
                IEP_id = int.Parse(IepId.ToString());
            }


        }
        return IEP_id;
    }
    private void ViewAccReject()
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        int reject = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Rejected'");
        int approve = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        if (sess.IEPStatus == approve || sess.IEPStatus == reject)
        {
            btnSubmitIEP1.Visible = false;
        }

    }
    public void a() { }
    //private void fillSudentsDetails(int studentId)
    //{
    //    objData = new clsData();
    //    string query = "select * from Student where StudentId=" + studentId;
    //    Dt = objData.ReturnDataTable(query, false);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        //lbliepDateFrom.Text=Dt.Rows[0]["
    //        lblDob.Text = Dt.Rows[0]["DOB"].ToString();
    //        lblGrade.Text = Dt.Rows[0]["GradeLevel"].ToString();
    //        lblStudentName.Text = Dt.Rows[0]["StudentLname"].ToString() + ", " + Dt.Rows[0]["StudentFname"].ToString();
    //        lblId.Text = Dt.Rows[0]["StudentNbr"].ToString();
    //    }
    //}
    [WebMethod]
    public static string submitIEP1(string txtConcerns,string txtStrengths,string txtVision)
    {
        string result = "";
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int ddd = sess.IEPId;
        int ssffs = sess.IEPStatus;
        if (sess == null)
        {
            HttpContext.Current.Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
        }
        try
        {
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            if (sess.IEPId <= 0) return result;
            if (sess.IEPId == null)
            {
                result = "IEP not Properly Selected";
                return result;
            }
            pendstatus = Convert.ToString(objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " "));
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                result = "IEP is in Pending State.";
                return result;
                
            }
            else
            {
                string StatusName = Convert.ToString(objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus));

                if (StatusName == "Approved" || StatusName == "Rejected")
                {
                    result = "Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!";
                    return result;
                }
                else
                {
                    if (sess != null)
                    {
                        if (sess.StudentId == 0)
                        {
                            result= "Please select Student..";
                            return result;
                        }

                        else
                        {

                            objData = new clsData();
                            //IEPId = sess.IEPId;
                            string strQuery = "Update StdtIEP Set Concerns = '" + clsGeneral.convertQuotes(txtConcerns) + "',StatusId=" + sess.IEPStatus + ", Strength = '" + clsGeneral.convertQuotes(txtStrengths) + "', Vision= '" + clsGeneral.convertQuotes(txtVision) + "' " +
                                            ",ModifiedBy='" + sess.LoginId + "' , ModifiedOn= GETDATE() where StdtIEPID= " + sess.IEPId + "";
                            Boolean index = Convert.ToBoolean(objData.Execute(strQuery));
                            if (index == true)
                            {
                                //  tdMsg.InnerHtml = clsGeneral.sucessMsg("Data IEP Page 1 Updated for " + sess.StudentName + "!");

                                result="Data Updated Successfully";

                                if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                                {
                                    objData.Execute("update stdtIEPUpdateStatus set Page1='true' where stdtIEPId=" + sess.IEPId);
                                }
                                else
                                {
                                    objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page1) values(" + sess.IEPId + ",'true')");
                                }
                                // Clear();
                                //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP2();", true);
                                
                            }
                        }
                    }
                    return result;
                }
            }
        }
        catch (SqlException Ex)
        {
            result = "Updation Failed!";
                return result; 
        }
    }
    [WebMethod]
    public static string submitIEP1_hdn(string txtConcerns, string txtStrengths, string txtVision)
    {
        string result = "";
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int ddd = sess.IEPId;
        int ssffs = sess.IEPStatus;
        if (sess == null)
        {
            HttpContext.Current.Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
        }
        try
        {
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            if (sess.IEPId <= 0) return result;
            if (sess.IEPId == null)
            {
                result = "IEP not Properly Selected";
                return result;
            }
            pendstatus = Convert.ToString(objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " "));
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                result = "IEP is in Pending State.";
                return result;

            }
            else
            {
                string StatusName = Convert.ToString(objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus));

                if (StatusName == "Approved" || StatusName == "Rejected")
                {
                    result = "Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!";
                    return result;
                }
                else
                {
                    if (sess != null)
                    {
                        if (sess.StudentId == 0)
                        {
                            result = "Please select Student..";
                            return result;
                        }

                        else
                        {

                            objData = new clsData();
                            //IEPId = sess.IEPId;
                            string strQuery = "Update StdtIEP Set Concerns = '" + clsGeneral.convertQuotes(txtConcerns) + "',StatusId=" + sess.IEPStatus + ", Strength = '" + clsGeneral.convertQuotes(txtStrengths) + "', Vision= '" + clsGeneral.convertQuotes(txtVision) + "' " +
                                            ",ModifiedBy='" + sess.LoginId + "' , ModifiedOn= GETDATE() where StdtIEPID= " + sess.IEPId + "";
                            Boolean index = Convert.ToBoolean(objData.Execute(strQuery));
                            if (index == true)
                            {
                                //  tdMsg.InnerHtml = clsGeneral.sucessMsg("Data IEP Page 1 Updated for " + sess.StudentName + "!");

                                result = "Data Updated Successfully";

                                if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                                {
                                    objData.Execute("update stdtIEPUpdateStatus set Page1='true' where stdtIEPId=" + sess.IEPId);
                                }
                                else
                                {
                                    objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page1) values(" + sess.IEPId + ",'true')");
                                }
                                // Clear();
                                //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP2();", true);

                            }
                        }
                    }
                    return result;
                }
            }
        }
        catch (SqlException Ex)
        {
            result = "Updation Failed!";
            return result;
        }
    }
    protected void btnSubmitIEP1_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        int ddd = sess.IEPId;
        int ssffs = sess.IEPStatus;
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }


        try
        {
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            if (sess.IEPId <= 0) return;
            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }
            pendstatus = Convert.ToString(objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " "));
            if (Convert.ToString(pendingApprove) == pendstatus)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
            }
            else
            {
                Update();
            }
        }
        catch (SqlException Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(Ex.Message.ToString() + " Updation Failed!");
        }
        //if (btnSubmitIEP1.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}
    }


    protected void Update()
    {
       
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            string StatusName = Convert.ToString(objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus));

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                if (sess != null)
                {
                    if (sess.StudentId == 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student..");
                        return;
                    }
                    
                    else
                    {

                        if (txtConcerns.InnerText != null)
                        {
                            string docData = "";
                            docData = txtConcerns.InnerText;
                            byte[] byteArray = Encoding.ASCII.GetBytes(docData);
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                memoryStream.Write(byteArray, 0, byteArray.Length);
                                using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                                {
                                    HtmlConverterSettings settings = new HtmlConverterSettings()
                                    {
                                        PageTitle = "My Page Title"
                                    };
                                    XElement html = OpenXmlPowerTools.HtmlConverter.ConvertToHtml(doc, settings);

                                    File.WriteAllText("d:\test.txt", html.ToStringNewLineOnAttributes());
                                }
                            }//txtStrengths txtVision

                        }
                        
                        objData = new clsData();
                        //IEPId = sess.IEPId;
                        strQuery = "Update StdtIEP Set Concerns = '" + clsGeneral.convertQuotes(txtConcerns.InnerHtml.Trim()) + "',StatusId=" + sess.IEPStatus + ", Strength = '" + clsGeneral.convertQuotes(txtStrengths.InnerHtml.Trim()) + "', Vision= '" + clsGeneral.convertQuotes(txtVision.InnerHtml.Trim()) + "' " +
                                        ",ModifiedBy='" + sess.LoginId + "' , ModifiedOn= GETDATE() where StdtIEPID= " + sess.IEPId + "";
                        Boolean index = Convert.ToBoolean(objData.Execute(strQuery));
                        if (index == true)
                        {
                            //  tdMsg.InnerHtml = clsGeneral.sucessMsg("Data IEP Page 1 Updated for " + sess.StudentName + "!");

                            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");

                            if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                            {
                                objData.Execute("update stdtIEPUpdateStatus set Page1='true' where stdtIEPId=" + sess.IEPId);
                            }
                            else
                            {
                                objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page1) values(" + sess.IEPId + ",'true')");
                            }
                            // Clear();
                            ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP2();", true);
                        }
                    }
                }
            }
        }
        catch (SqlException Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(Ex.Message.ToString() + " Updation Failed!");
        }

    }

 

    private void Clear()
    {
        hidIEPId.Value = "0";
        txtConcerns.InnerHtml = "";
        txtStrengths.InnerHtml = "";
        txtVision.InnerHtml = "";
    }
    protected void Save() // Consider SchoolId, StudentId, AsmntYearId, StatusId
    {
        try
        {
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                if (sess.StudentId == 0)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student..");
                    return;
                }
                else if (txtConcerns.InnerHtml.Trim() == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter Concerns..");
                    txtConcerns.Focus();
                    return;
                }
                else if (txtStrengths.InnerHtml.Trim() == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter Strengths..");
                    txtStrengths.Focus();
                    return; ;
                }
                else if (txtVision.InnerHtml.Trim() == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter Vision..");
                    txtVision.Focus();
                    return;
                }
                else
                {
                    objData = new clsData();
                    if (objData.IFExists("Select * from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=" + sess.YearId + "") == true)//sess.SchoolId
                    {
                        version = objData.FetchValue("Select TOP 1 Version from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=" + sess.YearId + " ORDER BY StdtIEPId DESC ").ToString();
                    }
                    if (version == "") version = "1.0";
                    else { Double ver = Convert.ToDouble(version) + 1; version = Convert.ToString(ver) + ".0"; }


                    strQuery = "Insert into StdtIEP(SchoolId, StudentId, AsmntYearId, StatusId, Concerns, Strength, Vision, CreatedBy, CreatedOn,ModifiedBy,ModifiedOn) " +
                                  "values( '" + sess.SchoolId + "', '" + sess.StudentId + "' , '" + sess.YearId + "' , '1', '" + clsGeneral.convertQuotes(txtConcerns.InnerHtml.Trim()) + "', '" + clsGeneral.convertQuotes(txtStrengths.InnerHtml.Trim()) + "', '" + clsGeneral.convertQuotes(txtVision.InnerHtml.Trim()) + "', " +
                                  " '" + sess.LoginId + "' ,GETDATE(),'" + sess.LoginId + "' ,GETDATE())";

                    intIEPId = objData.ExecuteWithScope(strQuery);

                    strQuery = "Insert into StdtIEPExt1 (StdtIEPId,StatusId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values(" + intIEPId + "," + 1 + "," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate())";
                    objData.Execute(strQuery);

                    strQuery = "Insert into StdIEPExt2 (StdtIEPId,StatusId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values(" + intIEPId + "," + 1 + "," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate())";
                    objData.Execute(strQuery);

                    strQuery = "Insert into StdtIEPExt3 (StdtIEPId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values(" + intIEPId + "," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate())";
                    objData.Execute(strQuery);


                    //tdMsg.InnerHtml = clsGeneral.sucessMsg("Data IEP Page 1 Saved for " + sess.StudentName + "!");

                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Saved Successfully");
                    sess.IEPId = intIEPId;


                    Clear();
                }
            }
        }
        catch (SqlException Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(Ex.Message.ToString() + "Insertion Failed!");
        }

    }
    protected void Fill()
    {
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
        sess = (clsSession)Session["UserSession"];
        strQuery = "Select StdtIEPId,Concerns, Strength, Vision FROM StdtIEP where StudentId=" + sess.StudentId + " AND StdtIEPId=" + sess.IEPId;
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                txtConcerns.InnerHtml = Dt.Rows[0]["Concerns"].ToString().Trim();
                txtStrengths.InnerHtml = Dt.Rows[0]["Strength"].ToString().Trim();
                txtVision.InnerHtml = Dt.Rows[0]["Vision"].ToString().Trim();

                txtConcerns_hdn.Text = System.Uri.EscapeDataString(txtConcerns.InnerHtml);
                txtStrengths_hdn.Text = System.Uri.EscapeDataString(txtStrengths.InnerHtml);
                txtVision_hdn.Text = System.Uri.EscapeDataString(txtVision.InnerHtml);


                hidIEPId.Value = Dt.Rows[0]["StdtIEPId"].ToString().Trim();
                btnSubmitIEP1.Text = "Save and continue";
                sess.IEPId = Convert.ToInt32(hidIEPId.Value);
            }
        }
        string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
        if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
        {
            btnSubmitIEP1.Visible = false;
        }
        else
        {
            btnSubmitIEP1.Visible = true;
        }

    }


    private void getDetails()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (objData.IFExists("Select * from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + "") == true)
        {

        }
        else
        {

        }
        //Fill();
        //tdMsg.InnerHtml = "";
    }

    private int getInProgressId()
    {
        objData = new clsData();
        strQuery = "select LookupId from LookUp where LookupName='In Progress' and LookupType='IEP Status'";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt.Rows.Count > 0)
        {
            return Convert.ToInt32(Dt.Rows[0]["LookupId"].ToString());
        }
        else
        {
            return 0;
        }


    }
    protected void btnSubmitIEP1_Click1(object sender, EventArgs e)
    {

        string txtConcerns =System.Uri.UnescapeDataString(txtConcerns_hdn.Text);
        string txtStrengths = System.Uri.UnescapeDataString(txtStrengths_hdn.Text);
        string txtVision = System.Uri.UnescapeDataString(txtVision_hdn.Text);

        string result = "";
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int ddd = sess.IEPId;
        int ssffs = sess.IEPStatus;
        if (sess == null)
        {
            HttpContext.Current.Response.Redirect("~/Administration/Error.aspx?Error=Your session has expired. Please log-in again");
        }
        try
        {
            DataClass oData = new DataClass();
            objData = new clsData();
            string pendstatus = "";
            int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
            if (sess.IEPId <= 0)
            {
                result = "";
            }
            else
            {
                if (sess.IEPId == null)
                {
                    result = "IEP not Properly Selected";
                    //return result;
                }
                else {
                    pendstatus = Convert.ToString(objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " "));
                    if (Convert.ToString(pendingApprove) == pendstatus)
                    {
                        result = "IEP is in Pending State.";
                        //return result;

                    }
                    else
                    {
                        string StatusName = Convert.ToString(objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus));

                        if (StatusName == "Approved" || StatusName == "Rejected")
                        {
                            result = "Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!";
                           // return result;
                        }
                        else
                        {
                            if (sess != null)
                            {
                                if (sess.StudentId == 0)
                                {
                                    result = "Please select Student..";
                                   // return result;
                                }

                                else
                                {

                                    objData = new clsData();
                                    //IEPId = sess.IEPId;
                                    string strQuery = "Update StdtIEP Set Concerns = '" + clsGeneral.convertQuotes(txtConcerns) + "',StatusId=" + sess.IEPStatus + ", Strength = '" + clsGeneral.convertQuotes(txtStrengths) + "', Vision= '" + clsGeneral.convertQuotes(txtVision) + "' " +
                                                    ",ModifiedBy='" + sess.LoginId + "' , ModifiedOn= GETDATE() where StdtIEPID= " + sess.IEPId + "";
                                    Boolean index = Convert.ToBoolean(objData.Execute(strQuery));
                                    if (index == true)
                                    {
                                        //  tdMsg.InnerHtml = clsGeneral.sucessMsg("Data IEP Page 1 Updated for " + sess.StudentName + "!");

                                        result = "Data Updated Successfully";

                                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                                        {
                                            objData.Execute("update stdtIEPUpdateStatus set Page1='true' where stdtIEPId=" + sess.IEPId);
                                        }
                                        else
                                        {
                                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page1) values(" + sess.IEPId + ",'true')");
                                        }
                                        // Clear();
                                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(2);", true);

                                    }
                                }
                            }
                           // return result;
                        }
                    }
                }
                
            }
        }
        catch (SqlException Ex)
        {
            result = "Updation Failed!";
            //return result;
        }
    }
}