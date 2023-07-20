using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP3 : System.Web.UI.Page
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
            Fill();
           
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
            ViewAccReject();
        }

    }
    //[WebMethod]
    //public static string submitIEP3(string txtAccomodation3, string txtDisabilities3)
    //{
    //    objData = new clsData();
    //    sess = (clsSession)HttpContext.Current.Session["UserSession"];

    //    string strQuery = "Update  StdtIEPExt2 set AffectDesc='" + clsGeneral.convertQuotes(txtDisabilities3) + "',AccomDesc='" + clsGeneral.convertQuotes(txtAccomodation3) + "' where StdtIEPId=" + sess.IEPId + "";

    //    string retVal =Convert.ToString( objData.Execute(strQuery));
    //    return retVal;
    //}

    public string submitIEP3(string txtAccomodation3, string txtDisabilities3)
    {
        objData = new clsData();
        sess = (clsSession)HttpContext.Current.Session["UserSession"];

        string strQuery = "Update  StdtIEPExt2 set AffectDesc='" + clsGeneral.convertQuotes(txtDisabilities3) + "',AccomDesc='" + clsGeneral.convertQuotes(txtAccomodation3) + "' where StdtIEPId=" + sess.IEPId + "";

        string retVal = Convert.ToString(objData.Execute(strQuery));
        return retVal;
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


    private bool Validation()
    {
        //if (txtAccomodation3.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Accomodation");
        //    txtAccomodation3.Focus();
        //    return false;
        //}
        //else if (txtContent3.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Content");
        //    txtContent3.Focus();
        //    return false;
        //}
        //else if (txtDisabilities3.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Disabilities");
        //    txtDisabilities3.Focus();
        //    return false;
        //}

        return true;
    }
    protected void Save()
    {

        try
        {
            //if (Validation() == true)
            //{
                sess = (clsSession)Session["UserSession"];
                objData = new clsData();

                if (sess.IEPId == null)
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                    return;
                }
                //IEPId = Convert.ToInt32(objData.FetchValue("Select [StdtIEPId] from [dbo].[StdtIEP] where SchoolId=" + sess.SchoolId + " and StudentId=" + ddlStudent.SelectedValue + " and EffEndDate is Null"));
                IEPId = sess.IEPId;
                if (IEPId > 0)
                {
                    strQuery = "Update  StdtIEPExt2 set StdtIEPId=" + IEPId + ",PEInd='" + chkAdaptedPhyEdu.Checked + "',TechDevicesInd='" + chkAssTechDevices.Checked + "',BehaviorInd='" + chkBehavior.Checked + "',BrailleInd='" + chkBrailleNeeds0.Checked + "',CommInd='" + chkCommunicationAllStudent0.Checked + "', ";
                    strQuery += "CommDfInd='" + chkCommunicationDeaf.Checked + "',ExtCurInd='" + chkExtraCurrAct.Checked + "',LEPInd='" + chkLangNeeds.Checked + "',NonAcdInd='" + chkNonAcademicActivities.Checked + "',SocialInd='" + chkSocialNeeds.Checked + "',TravelInd='" + chkTravelTraining.Checked + "',VocInd='" + chkVocationalEdu.Checked + "', ";
                    strQuery += "OtherInd='" + chkOther.Checked + "',OtherDesc='" + clsGeneral.convertQuotes(txtOther3.Text) + "',AgeBand1Ind='" + chkChild3To5.Checked + "',AgeBand2Ind='" + chkChild14.Checked + "',AgeBand3Ind='" + chkChild16.Checked + "', ";
                    strQuery += "AffectDesc='" + clsGeneral.convertQuotes(txtDisabilities3.InnerHtml.Trim()) + "',AccomDesc='" + clsGeneral.convertQuotes(txtAccomodation3.InnerHtml.Trim()) + "',ContentModInd='" + chkContent3.Checked + "',ContentModDesc='" + clsGeneral.convertQuotes(txtContent3.Text) + "',MethodModInd='" + chkMethodology3.Checked + "',MethodModDesc='" + clsGeneral.convertQuotes(txtMethodology3.Text) + "',PerfModInd='" + chkPerformance3.Checked + "',PerfModDesc='" + clsGeneral.convertQuotes(txtPerformance3.Text) + "',StatusId=0,ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + IEPId + "";



                    //strQuery = "Insert into  StdIEPExt2(StdtIEPId,PEInd,TechDeviceInd,BehaviorInd,BrailleInd,CommInd,CommDfInd,ExtCurInd,LEPInd,NonAcdInd,SocialIInd,TravelInd,VocInd,OtherInd,OtherDesc,AgeBand1Ind,AgeBand2Ind,AgeBand3Ind,AffectDesc,AccomDesc,ContentModInd,ContentModDesc,MethodModInd,MethodModDesc,PerfModInd,PerfModDesc,StatusId,CreatedBy,CreatedOn) ";
                    //strQuery += "Values(" + IEPId + ",'" + chkAdaptedPhyEdu.Checked + "','" + chkAssTechDevices.Checked + "','" + chkBehavior.Checked + "','" + chkBrailleNeeds0.Checked + "','" + chkCommunicationAllStudent0.Checked + "', ";
                    //strQuery += "'" + chkCommunicationDeaf.Checked + "','" + chkExtraCurrAct.Checked + "','" + chkLangNeeds.Checked + "','" + chkNonAcademicActivities.Checked + "','" + chkSocialNeeds.Checked + "','" + chkTravelTraining.Checked + "','" + chkVocationalEdu.Checked + "','" + chkOther.Checked + "','" + txtOther3.Text + "','" + chkChild3To5.Checked + "','" + chkChild14.Checked + "', ";
                    //strQuery += "'" + chkChild16.Checked + "','" + txtDisabilities3.Text.Trim() + "','" + txtAccomodation3.Text.Trim() + "','" + chkContent3.Checked + "','" + txtContent3.Text + "','" + chkMethodology3.Checked + "','" + txtMethodology3.Text + "','" + chkPerformance3.Checked + "','" + txtPerformance3.Text + "',0," + sess.LoginId + ",getdate())";



                    retVal = objData.Execute(strQuery);
                }

                else if (IEPId == 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please Save IEP Page 1 for " + sess.StudentName + "!");
                    return;
                }

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(4);", true);
                    //Clear();
                    // ddlStudent.SelectedIndex = 0;
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation failed...");
                }
            //}
            //else
            //{
            //    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
            //    return;
            //}

        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
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
    protected void Update()
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
                //if (Validation() == true)
                //{
                    strQuery = "Update  StdtIEPExt2 set StdtIEPId=" + sess.IEPId + ",PEInd='" + chkAdaptedPhyEdu.Checked + "',TechDevicesInd='" + chkAssTechDevices.Checked + "',BehaviorInd='" + chkBehavior.Checked + "',BrailleInd='" + chkBrailleNeeds0.Checked + "',CommInd='" + chkCommunicationAllStudent0.Checked + "', ";
                    strQuery += "CommDfInd='" + chkCommunicationDeaf.Checked + "',ExtCurInd='" + chkExtraCurrAct.Checked + "',LEPInd='" + chkLangNeeds.Checked + "',NonAcdInd='" + chkNonAcademicActivities.Checked + "',SocialInd='" + chkSocialNeeds.Checked + "',TravelInd='" + chkTravelTraining.Checked + "',VocInd='" + chkVocationalEdu.Checked + "', ";
                    strQuery += "OtherInd='" + chkOther.Checked + "',OtherDesc='" + clsGeneral.convertQuotes(txtOther3.Text) + "',AgeBand1Ind='" + chkChild3To5.Checked + "',AgeBand2Ind='" + chkChild14.Checked + "',AgeBand3Ind='" + chkChild16.Checked + "', ";
                    strQuery += "ContentModInd='" + chkContent3.Checked + "',ContentModDesc='" + clsGeneral.convertQuotes(txtContent3.Text.Trim()) + "',MethodModInd='" + chkMethodology3.Checked + "',MethodModDesc='" + clsGeneral.convertQuotes(txtMethodology3.Text.Trim()) + "',PerfModInd='" + chkPerformance3.Checked + "',PerfModDesc='" + clsGeneral.convertQuotes(txtPerformance3.Text.Trim()) + "',StatusId='" + sess.IEPStatus + "',CreatedBy=" + sess.LoginId + ",CreatedOn=getdate(),ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + sess.IEPId + "";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page3='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page3) values(" + sess.IEPId + ",'true')");
                        }
                        // Clear();
                        //ddlStudent.SelectedIndex = 0;
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(4);", true);
                    }
                //}
                //else
                //{
                //    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                //    return;
                //}
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation Failed!");
            throw Ex;
        }
    }
    protected void Update1()
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
                //if (Validation() == true)
                //{
                strQuery = "Update  StdtIEPExt2 set StdtIEPId=" + sess.IEPId + ",PEInd='" + chkAdaptedPhyEdu.Checked + "',TechDevicesInd='" + chkAssTechDevices.Checked + "',BehaviorInd='" + chkBehavior.Checked + "',BrailleInd='" + chkBrailleNeeds0.Checked + "',CommInd='" + chkCommunicationAllStudent0.Checked + "', ";
                strQuery += "CommDfInd='" + chkCommunicationDeaf.Checked + "',ExtCurInd='" + chkExtraCurrAct.Checked + "',LEPInd='" + chkLangNeeds.Checked + "',NonAcdInd='" + chkNonAcademicActivities.Checked + "',SocialInd='" + chkSocialNeeds.Checked + "',TravelInd='" + chkTravelTraining.Checked + "',VocInd='" + chkVocationalEdu.Checked + "', ";
                strQuery += "OtherInd='" + chkOther.Checked + "',OtherDesc='" + clsGeneral.convertQuotes(txtOther3.Text) + "',AgeBand1Ind='" + chkChild3To5.Checked + "',AgeBand2Ind='" + chkChild14.Checked + "',AgeBand3Ind='" + chkChild16.Checked + "', ";
                strQuery += "ContentModInd='" + chkContent3.Checked + "',ContentModDesc='" + clsGeneral.convertQuotes(txtContent3.Text.Trim()) + "',MethodModInd='" + chkMethodology3.Checked + "',MethodModDesc='" + clsGeneral.convertQuotes(txtMethodology3.Text.Trim()) + "',PerfModInd='" + chkPerformance3.Checked + "',PerfModDesc='" + clsGeneral.convertQuotes(txtPerformance3.Text.Trim()) + "',StatusId='" + sess.IEPStatus + "',CreatedBy=" + sess.LoginId + ",CreatedOn=getdate(),ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate() where StdtIEPId=" + sess.IEPId + "";

                retVal = objData.Execute(strQuery);

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page3='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page3) values(" + sess.IEPId + ",'true')");
                    }
                    // Clear();
                    //ddlStudent.SelectedIndex = 0;
                   // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP4();", true); NO NEED FOR INCREEMNET
                }
                //}
                //else
                //{
                //    tdMsg.InnerHtml = clsGeneral.sucessMsg("Please fill Mandatory Fields...");
                //    return;
                //}
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation Failed!");
            throw Ex;
        }
    }
    private void Clear()
    {
        txtAccomodation3.InnerHtml = "";
        txtContent3.Text = "";
        txtDisabilities3.InnerHtml = "";
        txtMethodology3.Text = "";
        txtOther3.Text = "";
        txtPerformance3.Text = "";

        chkAdaptedPhyEdu.Checked = false;
        chkAssTechDevices.Checked = false;
        chkBehavior.Checked = false;
        chkBrailleNeeds0.Checked = false;
        chkChild14.Checked = false;
        chkChild16.Checked = false;
        chkChild3To5.Checked = false;
        chkCommunicationAllStudent0.Checked = false;
        chkCommunicationDeaf.Checked = false;
        chkContent3.Checked = false;
        chkExtraCurrAct.Checked = false;
        chkLangNeeds.Checked = false;
        chkMethodology3.Checked = false;
        chkNonAcademicActivities.Checked = false;
        chkOther.Checked = false;
        chkPerformance3.Checked = false;
        chkSocialNeeds.Checked = false;
        chkTravelTraining.Checked = false;
        chkVocationalEdu.Checked = false;


        btnSave.Text = "Save and continue";

    }
    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
        //sess.IEPId = getIepIdFromStudentId();
        objData = new clsData();
        string strQuery = "";
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        if (sess != null)
        {
            strQuery = "SELECT IEP1.* FROM  StdtIEP IEP INNER JOIN StdtIEPExt2 IEP1 ON IEP.StdtIEPId = IEP1.StdtIEPId WHERE IEP.SchoolId=" + sess.SchoolId + " AND IEP.StudentId=" + sess.StudentId + " AND IEP.StdtIEPId=" + sess.IEPId;
        }
        Dt = objData.ReturnDataTable(strQuery, false);
        Clear();
        if (Dt.Rows.Count > 0)
        {
            IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);
            txtOther3.Text = Dt.Rows[0]["OtherDesc"].ToString().Trim();

            txtDisabilities3.InnerHtml = Dt.Rows[0]["AffectDesc"].ToString().Trim();
            txtAccomodation3.InnerHtml = Dt.Rows[0]["AccomDesc"].ToString().Trim();

            txtDisabilities3_hdn.Text = System.Uri.EscapeDataString(txtDisabilities3.InnerHtml);
            txtAccomodation3_hdn.Text = System.Uri.EscapeDataString(txtAccomodation3.InnerHtml);

            txtContent3.Text = Dt.Rows[0]["ContentModDesc"].ToString().Trim();
            txtMethodology3.Text = Dt.Rows[0]["MethodModDesc"].ToString().Trim();
            txtPerformance3.Text = Dt.Rows[0]["PerfModDesc"].ToString().Trim();


            chkAdaptedPhyEdu.Checked = clsGeneral.getChecked(Dt.Rows[0]["PEInd"].ToString());
            chkAssTechDevices.Checked = clsGeneral.getChecked(Dt.Rows[0]["TechDevicesInd"].ToString());
            chkBehavior.Checked = clsGeneral.getChecked(Dt.Rows[0]["BehaviorInd"].ToString());
            chkBrailleNeeds0.Checked = clsGeneral.getChecked(Dt.Rows[0]["BrailleInd"].ToString());


            chkCommunicationAllStudent0.Checked = clsGeneral.getChecked(Dt.Rows[0]["CommInd"].ToString());
            chkCommunicationDeaf.Checked = clsGeneral.getChecked(Dt.Rows[0]["CommDfInd"].ToString());
            chkExtraCurrAct.Checked = clsGeneral.getChecked(Dt.Rows[0]["ExtCurInd"].ToString());
            chkLangNeeds.Checked = clsGeneral.getChecked(Dt.Rows[0]["LEPInd"].ToString());


            chkNonAcademicActivities.Checked = clsGeneral.getChecked(Dt.Rows[0]["NonAcdInd"].ToString());
            chkSocialNeeds.Checked = clsGeneral.getChecked(Dt.Rows[0]["SocialInd"].ToString());
            chkTravelTraining.Checked = clsGeneral.getChecked(Dt.Rows[0]["TravelInd"].ToString());
            chkVocationalEdu.Checked = clsGeneral.getChecked(Dt.Rows[0]["VocInd"].ToString());


            chkOther.Checked = clsGeneral.getChecked(Dt.Rows[0]["OtherInd"].ToString());
            chkChild3To5.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand1Ind"].ToString());
            chkChild14.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand2Ind"].ToString());
            chkChild16.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand3Ind"].ToString());



            chkContent3.Checked = clsGeneral.getChecked(Dt.Rows[0]["ContentModInd"].ToString());
            chkMethodology3.Checked = clsGeneral.getChecked(Dt.Rows[0]["MethodModInd"].ToString());
            chkPerformance3.Checked = clsGeneral.getChecked(Dt.Rows[0]["PerfModInd"].ToString());
            chkChild16.Checked = clsGeneral.getChecked(Dt.Rows[0]["AgeBand3Ind"].ToString());




            btnSave.Text = "Save and continue";




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
    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        if (sess.IEPId <= 0) return;
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
            submitIEP3(System.Uri.UnescapeDataString(txtAccomodation3_hdn.Text), System.Uri.UnescapeDataString(txtDisabilities3_hdn.Text));
            Update();
        }
       
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        if (sess.IEPId <= 0) return;
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
}