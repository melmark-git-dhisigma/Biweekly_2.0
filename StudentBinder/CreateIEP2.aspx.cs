using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP2 : System.Web.UI.Page
{
    public static clsData objData = null;
    public static clsSession sess = null;
    string strQuery = "";
    static int IEPId = 0;
    int retVal = 0;
    DataTable Dt = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
      
       
        if (!IsPostBack)
        {
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }
            Fill();
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
            btnSave.Visible = false;
        }

    }
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

    //private void fillSudentsDetails(int studentId)
    //{
    //    objData = new clsData();
    //    string query = "select * from Student where StudentId=" + studentId;
    //    Dt = objData.ReturnDataTable(query, false);
    //    if (Dt.Rows.Count > 0)
    //    {
            //lbliepDateFrom.Text=Dt.Rows[0]["
            //lblDob.Text = Dt.Rows[0]["DOB"].ToString();
            //lblGrade.Text = Dt.Rows[0]["GradeLevel"].ToString();
            //lblStudentName.Text = Dt.Rows[0]["StudentLname"].ToString() + ", " + Dt.Rows[0]["StudentFname"].ToString();
            //lblId.Text = Dt.Rows[0]["StudentNbr"].ToString();



    //    }
    //}
   
    protected void btnSave_Click(object sender, EventArgs e)
    {

        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            submitIEP2(System.Uri.UnescapeDataString(txtAccomodation_hdn.Text), System.Uri.UnescapeDataString(txtDisabilities_hdn.Text));
            Update();
            
        }
        
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            Update1();
        }
        
    }

    
   
    private void Save()
    {
        try
        {
            if (Validation() == true)
            {
                sess = (clsSession)Session["UserSession"];
                IEPId = sess.IEPId;
                objData = new clsData();
                
                if (IEPId > 0)
                {
                    strQuery = "Update StdtIEPExt1 set StdtIEPId=" + IEPId + ", EngLangInd='" + chkEnglishLangArts.Checked + "',HistInd='" + chkHistoryAndSocial.Checked + "',TechInd='" + chkScienceAndTech.Checked + "',MathInd='" + chkMathematics.Checked + "'	,OtherInd='" + chkOtherCurriculumActivities.Checked + "',OtherDesc='" + clsGeneral.convertQuotes(txtSpecify.Text.Trim()) + "',AffectDesc='" + clsGeneral.convertQuotes(txtDisabilities.InnerHtml.Trim()) + "',AccomDesc='" + clsGeneral.convertQuotes(txtAccomodation.InnerHtml.Trim()) + "',ContentModInd='" + chkContent.Checked + "',ContentModDesc='" + clsGeneral.convertQuotes(txtContent.Text.Trim()) + "',MethodModInd='" + chkMethodology.Checked + "',MethodModDesc='" + clsGeneral.convertQuotes(txtMethodology.Text.Trim()) + "',PerfModInd='" + chkPerformanceCriteria.Checked + "',PerfModDesc='" + clsGeneral.convertQuotes(txtPerformance.Text.Trim()) + "',StatusId=0,ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + IEPId + " ";
                    //strQuery = "Insert into StdtIEPExt1(StdtIEPId, EngLangInd,HistInd,TechInd,MathInd	,OtherInd,OtherDesc,AffectDesc,AccomDesc,ContentModInd,ContentModDesc	,MethodModInd,MethodModDesc,PerfModInd,PerfModDesc,StatusId,CreatedBy,CreatedOn) ";
                    //strQuery += " Values("+IEPId+",'" + chkEnglishLangArts.Checked + "','" + chkHistoryAndSocial.Checked + "','" + chkScienceAndTech.Checked + "','" + chkMathematics.Checked + "','" + chkOtherCurriculumActivities.Checked + "','" + txtSpecify.Text + "','" + txtDisabilities.Text + "','" + txtAccomodation.Text + "','" + chkContent.Checked + "','" + txtContent.Text + "','" + chkMethodology.Checked + "','" + txtMethodology.Text + "','" + chkPerformanceCriteria.Checked + "','" + txtPerformance.Text + "',0," + sess.LoginId + ",getdate())";

                    retVal = objData.Execute(strQuery);
                }
                
                else if (IEPId == 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Saved Successfully");
                    return;
                }

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");

                    
                    //Clear();
                   // ddlStudent.SelectedIndex = 0;
                }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                return;
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(Ex.Message.ToString() + " Updation Failed!");
        }
    }
    private bool Validation()
    {
        if (sess.StudentId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
            return false;
        }
        //if (txtAccomodation.Text == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Accomodation");
        //    txtAccomodation.Focus();
        //    return false;
        //}
        //else if (txtContent.Text == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Content");
        //    txtContent.Focus();
        //    return false;
        //}
        //else if (txtDisabilities.Text == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Disabilities");
        //    txtDisabilities.Focus();
        //    return false;
        //}
        //else if (txtMethodology.Text == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Methodology");
        //    txtMethodology.Focus();
        //    return false;
        //}
        return true;
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
    //[WebMethod]
    //public static string submitIEP2(string txtAccomodation, string txtDisabilities)
    //{
    //    objData = new clsData();
    //    sess = (clsSession)HttpContext.Current.Session["UserSession"];
    //    string strQuery = "Update StdtIEPExt1 set AffectDesc='" + clsGeneral.convertQuotes(txtDisabilities) + "',AccomDesc='" + clsGeneral.convertQuotes(txtAccomodation) + "' where StdtIEPId=" + sess.IEPId + " ";

    //    string retVal =Convert.ToString( objData.Execute(strQuery));
    //    return retVal;
    //}

    private string submitIEP2(string txtAccomodation, string txtDisabilities)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        string strQuery = "Update StdtIEPExt1 set AffectDesc='" + clsGeneral.convertQuotes(txtDisabilities) + "',AccomDesc='" + clsGeneral.convertQuotes(txtAccomodation) + "' where StdtIEPId=" + sess.IEPId + " ";

        string retVal = Convert.ToString(objData.Execute(strQuery));
        return retVal;
    }
    private void Update()
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            
            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                if (Validation() == true)
                {
                    strQuery = "Update StdtIEPExt1 set StdtIEPId=" + sess.IEPId + ", EngLangInd='" + chkEnglishLangArts.Checked + "',HistInd='" + chkHistoryAndSocial.Checked + "',TechInd='" + chkScienceAndTech.Checked + "',MathInd='" + chkMathematics.Checked + "'	,OtherInd='" + chkOtherCurriculumActivities.Checked + "',OtherDesc='" + clsGeneral.convertQuotes(txtSpecify.Text.Trim()) + "',ContentModInd='" + chkContent.Checked + "',ContentModDesc='" + clsGeneral.convertQuotes(txtContent.Text.Trim()) + "',MethodModInd='" + chkMethodology.Checked + "',MethodModDesc='" + clsGeneral.convertQuotes(txtMethodology.Text.Trim()) + "',PerfModInd='" + chkPerformanceCriteria.Checked + "',PerfModDesc='" + clsGeneral.convertQuotes(txtPerformance.Text.Trim()) + "',StatusId='" + sess.IEPStatus + "',CreatedBy=" + sess.LoginId + ",CreatedOn=getdate(),ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + sess.IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page2='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page2) values(" + sess.IEPId + ",'true')");
                        }
                        //Clear();
                        //ddlStudent.SelectedIndex = 0;
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(3);", true);
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please fill Mandatory Fields...");
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(Ex.Message.ToString() + " Updation Failed!");
        }
    }
    private void Update1()
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                if (Validation() == true)
                {
                    strQuery = "Update StdtIEPExt1 set StdtIEPId=" + sess.IEPId + ", EngLangInd='" + chkEnglishLangArts.Checked + "',HistInd='" + chkHistoryAndSocial.Checked + "',TechInd='" + chkScienceAndTech.Checked + "',MathInd='" + chkMathematics.Checked + "'	,OtherInd='" + chkOtherCurriculumActivities.Checked + "',OtherDesc='" + clsGeneral.convertQuotes(txtSpecify.Text.Trim()) + "',ContentModInd='" + chkContent.Checked + "',ContentModDesc='" + clsGeneral.convertQuotes(txtContent.Text.Trim()) + "',MethodModInd='" + chkMethodology.Checked + "',MethodModDesc='" + clsGeneral.convertQuotes(txtMethodology.Text.Trim()) + "',PerfModInd='" + chkPerformanceCriteria.Checked + "',PerfModDesc='" + clsGeneral.convertQuotes(txtPerformance.Text.Trim()) + "',StatusId='" + sess.IEPStatus + "',CreatedBy=" + sess.LoginId + ",CreatedOn=getdate(),ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + sess.IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page2='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page2) values(" + sess.IEPId + ",'true')");
                        }
                        //Clear();
                        //ddlStudent.SelectedIndex = 0;
                        // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP3();", true);     //NO NEED FOR AUTO INCREEMENT
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please fill Mandatory Fields...");
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(Ex.Message.ToString() + " Updation Failed!");
        }
    }
    private void Clear()
    {
        IEPId = 0;
        txtAccomodation.InnerHtml = "";
        txtContent.Text = "";
        txtDisabilities.InnerHtml = "";
        txtMethodology.Text = "";
        txtPerformance.Text = "";
        txtSpecify.Text = "";
        chkContent.Checked = false;
        chkEnglishLangArts.Checked = false;
        chkHistoryAndSocial.Checked = false;
        chkMathematics.Checked = false;
        chkMethodology.Checked = false;
        chkOtherCurriculumActivities.Checked = false;
        chkPerformanceCriteria.Checked = false;
        chkScienceAndTech.Checked = false;
        
    }
    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
        //sess.IEPId = getIepIdFromStudentId();
        if (sess != null)
        {
            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }

            string strQuery = "SELECT IEP1.* FROM StdtIEP IEP INNER JOIN StdtIEPExt1 IEP1 ON IEP.StdtIEPId = IEP1.StdtIEPId WHERE IEP.SchoolId=" + sess.SchoolId + " AND IEP.StudentId=" + sess.StudentId + " And IEP.StdtIEPId="+sess.IEPId+"";
            objData = new clsData();
            Dt = objData.ReturnDataTable(strQuery, false);
            Clear();
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                   
                    IEPId = sess.IEPId;
                    txtSpecify.Text = Dt.Rows[0]["OtherDesc"].ToString().Trim();

                    txtDisabilities.InnerHtml = Dt.Rows[0]["AffectDesc"].ToString().Trim();
                    txtAccomodation.InnerHtml = Dt.Rows[0]["AccomDesc"].ToString().Trim();

                    txtDisabilities_hdn.Text = System.Uri.EscapeDataString(txtDisabilities.InnerHtml);
                    txtAccomodation_hdn.Text = System.Uri.EscapeDataString(txtAccomodation.InnerHtml);

                    txtContent.Text = Dt.Rows[0]["ContentModDesc"].ToString().Trim();
                    txtMethodology.Text = Dt.Rows[0]["MethodModDesc"].ToString().Trim();
                    txtPerformance.Text = Dt.Rows[0]["PerfModDesc"].ToString().Trim();

                    chkEnglishLangArts.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngLangInd"].ToString());
                    chkHistoryAndSocial.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistInd"].ToString());
                    chkScienceAndTech.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechInd"].ToString());
                    chkMathematics.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathInd"].ToString());
                    chkOtherCurriculumActivities.Checked = clsGeneral.getChecked(Dt.Rows[0]["OtherInd"].ToString());
                    chkContent.Checked = clsGeneral.getChecked(Dt.Rows[0]["ContentModInd"].ToString());
                    chkMethodology.Checked = clsGeneral.getChecked(Dt.Rows[0]["MethodModInd"].ToString());
                    chkPerformanceCriteria.Checked = clsGeneral.getChecked(Dt.Rows[0]["PerfModInd"].ToString());

                    btnSave.Text = "Save and continue";
                }
            }
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" ||Status.Trim()== "Expired")
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

        }
    }

    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill();       
    }
}