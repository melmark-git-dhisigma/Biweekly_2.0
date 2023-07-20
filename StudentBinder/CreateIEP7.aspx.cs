using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP7 : System.Web.UI.Page
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
            //sess.IEPId = getIepIdFromStudentId();
            clsIEP IEPObj = new clsIEP();
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnUpdate.Visible = false;
            }
            else
            {
                btnUpdate.Visible = true;
            }
            Fill();
            ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" ||Status.Trim()== "Expired")
            {
                btnUpdate.Visible = false;
            }
            else
            {
                btnUpdate.Visible = true;
            }
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
            btnUpdate.Visible = false;
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


    //    }
    //}
   
  
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            submitIEP7(System.Uri.UnescapeDataString(txtInfoCol2_hdn.Text), System.Uri.UnescapeDataString(txtInfoCol3_hdn.Text));
            Update();
        }
        
    }

    protected void btnUpdate_hdn_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
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

    //[WebMethod]
    //public static string submitIEP7(string txtInfoCol2, string txtInfoCol3)
    //{
    //    objData = new clsData();
    //    sess = (clsSession)HttpContext.Current.Session["UserSession"];
    //    string strQuery = " Update StdtIEPExt3 set InfoCol2='" + clsGeneral.convertQuotes(txtInfoCol2) + "', InfoCol3='" + clsGeneral.convertQuotes(txtInfoCol3) + "'  where StdtIEPId=" + IEPId + " ";   
    //    string retVal = Convert.ToString(objData.Execute(strQuery));
    //    return retVal;
    //}
    public string submitIEP7(string txtInfoCol2, string txtInfoCol3)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        string strQuery = " Update StdtIEPExt3 set InfoCol2='" + clsGeneral.convertQuotes(txtInfoCol2) + "', InfoCol3='" + clsGeneral.convertQuotes(txtInfoCol3) + "'  where StdtIEPId=" + IEPId + " ";
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
                    sess = (clsSession)Session["UserSession"];
                    objData = new clsData();
                    IEPId = sess.IEPId;
                    strQuery = " Update StdtIEPExt3 set EngCol1='" + chkEnglishLangArtC1.Checked + "',EngCol2='" + chkEnglishLangArtC2.Checked + "',EngCol3='" + chkEnglishLangArtC3.Checked + "',HistCol1='" + chkHistoryC1.Checked + "',HistCol2='" + chkHistoryC2.Checked + "',HistCol3='" + chkHistoryC3.Checked + "',MathCol1='" + chkMathematicsC1.Checked + "',MathCol2='" + chkMathematicsC2.Checked + "',MathCol3='" + chkMathematicsC3.Checked + "',TechCol1='" + chkScienceAndTechC1.Checked + "',TechCol2='" + chkScienceAndTechC2.Checked + "',TechCol3='" + chkScienceAndTechC3.Checked + "',ReadCol1='" + chkReadingC1.Checked + "', ReadCol2='" + chkReadingC2.Checked + "', ReadCol3='" + chkReadingC3.Checked + "',AsmntPlanned='" + clsGeneral.convertQuotes(txtStateOrDistrict.Text) + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page7='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page7) values(" + sess.IEPId + ",'true')");
                        }
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(8);", true);
                        //Clear();
                    }
                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation failed...");
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
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
                    sess = (clsSession)Session["UserSession"];
                    objData = new clsData();
                    IEPId = sess.IEPId;
                    strQuery = " Update StdtIEPExt3 set EngCol1='" + chkEnglishLangArtC1.Checked + "',EngCol2='" + chkEnglishLangArtC2.Checked + "',EngCol3='" + chkEnglishLangArtC3.Checked + "',HistCol1='" + chkHistoryC1.Checked + "',HistCol2='" + chkHistoryC2.Checked + "',HistCol3='" + chkHistoryC3.Checked + "',MathCol1='" + chkMathematicsC1.Checked + "',MathCol2='" + chkMathematicsC2.Checked + "',MathCol3='" + chkMathematicsC3.Checked + "',TechCol1='" + chkScienceAndTechC1.Checked + "',TechCol2='" + chkScienceAndTechC2.Checked + "',TechCol3='" + chkScienceAndTechC3.Checked + "',ReadCol1='" + chkReadingC1.Checked + "', ReadCol2='" + chkReadingC2.Checked + "', ReadCol3='" + chkReadingC3.Checked + "',AsmntPlanned='" + clsGeneral.convertQuotes(txtStateOrDistrict.Text) + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page7='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page7) values(" + sess.IEPId + ",'true')");
                        }
                        //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP8();", true);
                        //Clear();
                    }
                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation failed...");
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }
    private void Clear()
    {
        IEPId = 0;
        txtStateOrDistrict.Text = "";
        txtInfoCol2.InnerHtml = "";
        txtInfoCol3.InnerHtml = "";

        chkEnglishLangArtC1.Checked = false;
        chkEnglishLangArtC2.Checked = false;
        chkEnglishLangArtC3.Checked = false;

        chkHistoryC1.Checked = false;
        chkHistoryC2.Checked = false;
        chkHistoryC3.Checked = false;

        chkMathematicsC1.Checked = false;
        chkMathematicsC2.Checked = false;
        chkMathematicsC3.Checked = false;

        chkReadingC1.Checked = false;
        chkReadingC2.Checked = false;
        chkReadingC3.Checked = false;

        chkScienceAndTechC1.Checked = false;
        chkScienceAndTechC2.Checked = false;
        chkScienceAndTechC3.Checked = false;


    }
    private bool Validation()
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.StudentId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
            return false;
        }
        //else if (txtStateOrDistrict.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter State/District");
        //    txtStateOrDistrict.Focus();
        //    return false;
        //}
        
        return true;
    }
    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill();
    }
    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        
        if (sess != null)
        {
            if (sess.IEPId <= 0) return;
            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }
            objData = new clsData();
            string strQuery = "SELECT * FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt3 IEP3 ON IEP1.StdtIEPId = IEP3.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + "";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count > 0)
            {
                IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);
                txtStateOrDistrict.Text = Dt.Rows[0]["AsmntPlanned"].ToString().Trim();

                txtInfoCol2.InnerHtml = Dt.Rows[0]["InfoCol2"].ToString().Trim();
                txtInfoCol3.InnerHtml = Dt.Rows[0]["InfoCol3"].ToString().Trim();

                txtInfoCol2_hdn.Text = System.Uri.EscapeDataString(txtInfoCol2.InnerHtml);
                txtInfoCol3_hdn.Text = System.Uri.EscapeDataString(txtInfoCol3.InnerHtml);

                chkEnglishLangArtC1.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngCol1"].ToString()); ;
                chkEnglishLangArtC2.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngCol2"].ToString()); ;
                chkEnglishLangArtC3.Checked = clsGeneral.getChecked(Dt.Rows[0]["EngCol3"].ToString()); ;

                chkHistoryC1.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistCol1"].ToString()); ;
                chkHistoryC2.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistCol2"].ToString()); ;
                chkHistoryC3.Checked = clsGeneral.getChecked(Dt.Rows[0]["HistCol3"].ToString()); ;

                chkMathematicsC1.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathCol1"].ToString()); ;
                chkMathematicsC2.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathCol2"].ToString()); ;
                chkMathematicsC3.Checked = clsGeneral.getChecked(Dt.Rows[0]["MathCol3"].ToString()); ;

                chkReadingC1.Checked = clsGeneral.getChecked(Dt.Rows[0]["ReadCol1"].ToString()); ;
                chkReadingC2.Checked = clsGeneral.getChecked(Dt.Rows[0]["ReadCol2"].ToString()); ;
                chkReadingC3.Checked = clsGeneral.getChecked(Dt.Rows[0]["ReadCol3"].ToString()); ;

                chkScienceAndTechC1.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechCol1"].ToString()); ;
                chkScienceAndTechC2.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechCol2"].ToString()); ;
                chkScienceAndTechC3.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechCol3"].ToString()); ;

                btnUpdate.Text = "Save and continue";
            }
        }

    }
}