using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP9 : System.Web.UI.Page
{
    public static clsSession sess = null;
    public static clsData objData = null;
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
            clsIEP IEPObj = new clsIEP();
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSaveIep9.Visible = false;
            }
            else
            {
                btnSaveIep9.Visible = true;
            }
            Fill();
            //ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
            {
                btnSaveIep9.Visible = false;
            }
            else
            {
                btnSaveIep9.Visible = true;
            }
        }
    }
    protected void btnSaveIep9_Click(object sender, EventArgs e)
    {

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
            Update();
            Fill();
        }

    }
    protected void btnSaveIep9_hdn_Click(object sender, EventArgs e)
    {

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
            Fill();
        }

    }
    private bool Validation()
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.StudentId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
            return false;
        }
        return true;
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
            string strQuery = "SELECT * FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + "";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count > 0)
            {
                IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);
                txtStateOrDistrict.Text = Dt.Rows[0]["PlOneHoursWkPgm"].ToString().Trim();
                txtSignParentOne.Text = Dt.Rows[0]["PlOneSignParent"].ToString().Trim();
                txtSignParentTwo.Text = Dt.Rows[0]["PlTwoSignParent"].ToString().Trim();
                txtLocationService.Text = Dt.Rows[0]["PlOneServiceLocation"].ToString().Trim();
                txtLocationService2.Text = Dt.Rows[0]["PlOneServiceLocation2"].ToString().Trim();
                txtOther.Text = Dt.Rows[0]["PlTwoOtherDesc"].ToString().Trim();
                if (Dt.Rows[0]["PlOneDate"].ToString().Trim() != null && Dt.Rows[0]["PlOneDate"].ToString().Trim() != "")
                    txtDateOne.Text = Convert.ToDateTime(Dt.Rows[0]["PlOneDate"]).ToString("MM/dd/yyyy").Replace('-', '/');
                else
                    txtDateOne.Text = "";

                if (Dt.Rows[0]["PlTwoDate"].ToString().Trim() != null && Dt.Rows[0]["PlTwoDate"].ToString().Trim() != "")
                    txtDateTwo.Text = Convert.ToDateTime(Dt.Rows[0]["PlTwoDate"]).ToString("MM/dd/yyyy").Replace('-', '/');
                else
                    txtDateTwo.Text = "";
                chkEarlyChild.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneEarlyPgm"].ToString()); ;
                chkSeparate.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparatePgm"].ToString()); ;
                chkBoth.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneBothPgm"].ToString()); ;

                chkEnrolled.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneEnrolledPrnt"].ToString()); ;
                chkPlaced.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOnePlcdTeam"].ToString()); ;
                chkTimeMore.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneTimeMore"].ToString()); ;

                chkOfTime.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneTimeTwo"].ToString()); ;
                chk39Time.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneTimeThree"].ToString()); ;
                chkSubstancialy.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparateClass"].ToString()); ;

                chkDaySchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparateDayScl"].ToString()); ;
                chkPublic.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparatePublic"].ToString()); ;
                chkPrivate.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneSeparatePvt"].ToString()); ;

                chkResidential.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneResidentialFacility"].ToString()); ;
                chkHome.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneHome"].ToString()); ;
                chkService.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneServiceLctn"].ToString()); ;

                chkDepartment.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOnePsychiatric"].ToString()); ;
                chkMassachusetts.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneMassachusetts"].ToString()); ;
                chkDay.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneMassachusettsDay"].ToString()); ;

                chkRes.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneMassachusettsRes"].ToString()); ;
                chkHmeBasedPgm.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneDoctorHme"].ToString()); ;
                chkHospital.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneDoctorHsptl"].ToString()); ;

                chkConcent.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneConsent"].ToString()); ;
                chkPlacement.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOneRefuse"].ToString()); ;
                chkRefused.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlOnePlacement"].ToString()); ;



                chkFullPgm.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoFullInclusionPgm"].ToString()); ;
                chkPartialPgm.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPartialPgm"].ToString()); ;
                chkSepClass.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoSubstantially"].ToString()); ;

                chkSepDaySchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoSeparateScl"].ToString()); ;
                chkPublicSep.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPublicScl"].ToString()); ;
                chkPrivateSep.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPrivateScl"].ToString()); ;

                chkResSchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoResScl"].ToString()); ;
                chkOtherSep.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoOther"].ToString()); ;
                chkDetained.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoYouth"].ToString()); ;

                chkTreatment.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPsychiatric"].ToString()); ;
                chkHospitalSchool.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoMassachusetts"].ToString()); ;
                chkDayPlace.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoMassachusettsDay"].ToString()); ;
                chkResPlace.Checked = clsGeneral.getChecked(Dt.Rows[0]["PltwoMassachusettsRes"].ToString()); ;


                chkFacility.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoCorrectionFacility"].ToString()); ;
                chkHomeDoctor.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoDoctorHome"].ToString()); ;
                chkHospitalDoctor.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoDoctorHsptl"].ToString()); ;
                chkConsent2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoConsent"].ToString()); ;

                chkRefuse2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoPlacement"].ToString()); ;
                chkPlacement2.Checked = clsGeneral.getChecked(Dt.Rows[0]["PlTwoFullPgm"].ToString()); ;

                btnSaveIep9.Text = "Save and continue";
            }
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
                if (Validation() == true)
                {
                    sess = (clsSession)Session["UserSession"];
                    objData = new clsData();
                    IEPId = sess.IEPId;
                    ifExist();



                    strQuery = " Update StdtIEPExt4 set PlOneEarlyPgm ='" + chkEarlyChild.Checked + "',PlOneSeparatePgm	='" + chkSeparate.Checked + "',PlOneBothPgm	='" + chkBoth.Checked + "',PlOneServiceLocation='" + clsGeneral.convertQuotes(txtLocationService.Text) + "',PlOneServiceLocation2='" + clsGeneral.convertQuotes(txtLocationService2.Text) + "',PlOneHoursWkPgm ='" + clsGeneral.convertQuotes(txtStateOrDistrict.Text) + "',PlOneEnrolledPrnt ='" + chkEnrolled.Checked + "',PlOnePlcdTeam	='" + chkPlaced.Checked + "',PlOneTimeMore	='" + chkTimeMore.Checked + "',PlOneTimeTwo	='" + chkOfTime.Checked + "',PlOneTimeThree	='" + chk39Time.Checked + "',PlOneSeparateClass	='" + chkSubstancialy.Checked + "',PlOneSeparateDayScl ='" + chkDaySchool.Checked + "',PlOneSeparatePublic ='" + chkPublic.Checked + "',PlOneSeparatePvt ='" + chkPrivate.Checked + "', PlOneResidentialFacility ='" + chkResidential.Checked + "', PlOneHome ='" + chkHome.Checked + "'," +
                    "PlOneServiceLctn	='" + chkService.Checked + "',PlOnePsychiatric	='" + chkDepartment.Checked + "',PlOneMassachusetts	='" + chkMassachusetts.Checked + "',PlOneMassachusettsDay	='" + chkDay.Checked + "',PlOneMassachusettsRes	='" + chkRes.Checked + "',PlOneDoctorHme		='" + chkHmeBasedPgm.Checked + "',PlOneDoctorHsptl		='" + chkHospital.Checked + "',PlOneConsent			='" + chkConcent.Checked + "',PlOneRefuse		='" + chkPlacement.Checked + "',PlOnePlacement		='" + chkRefused.Checked + "',PlOneSignParent ='" + clsGeneral.convertQuotes(txtSignParentOne.Text) + "',PlOneDate ='" + clsGeneral.convertQuotes(txtDateOne.Text) + "',PlTwoFullInclusionPgm		='" + chkFullPgm.Checked + "',PlTwoPartialPgm		='" + chkPartialPgm.Checked + "',PlTwoSubstantially		='" + chkSepClass.Checked + "',PlTwoSeparateScl	='" + chkSepDaySchool.Checked + "',PlTwoPublicScl		='" + chkPublicSep.Checked + "'," +
                    "PlTwoPrivateScl	='" + chkPrivateSep.Checked + "',PlTwoResScl		='" + chkResSchool.Checked + "',PlTwoOther		='" + chkOtherSep.Checked + "',PlTwoOtherDesc='" + clsGeneral.convertQuotes(txtOther.Text) + "',PlTwoYouth	='" + chkDetained.Checked + "',PlTwoPsychiatric	='" + chkTreatment.Checked + "',PlTwoMassachusetts		='" + chkHospitalSchool.Checked + "',PlTwoMassachusettsDay	='" + chkDayPlace.Checked + "',PltwoMassachusettsRes		='" + chkResPlace.Checked + "',PlTwoCorrectionFacility		='" + chkFacility.Checked + "',PlTwoDoctorHome		='" + chkHomeDoctor.Checked + "',PlTwoDoctorHsptl	='" + chkHospitalDoctor.Checked + "',PlTwoConsent	='" + chkConsent2.Checked + "',PlTwoPlacement	='" + chkRefuse2.Checked + "',PlTwoFullPgm='" + chkPlacement2.Checked + "',PlTwoSignParent ='" + clsGeneral.convertQuotes(txtSignParentTwo.Text) + "',PlTwoDate ='" + clsGeneral.convertQuotes(txtDateTwo.Text) + "'," +
                     "ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page9='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page9) values(" + sess.IEPId + ",'true')");
                        }
                        ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(10);", true);
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
                if (Validation() == true)
                {
                    sess = (clsSession)Session["UserSession"];
                    objData = new clsData();
                    IEPId = sess.IEPId;
                    ifExist();



                    strQuery = " Update StdtIEPExt4 set PlOneEarlyPgm ='" + chkEarlyChild.Checked + "',PlOneSeparatePgm	='" + chkSeparate.Checked + "',PlOneBothPgm	='" + chkBoth.Checked + "',PlOneServiceLocation='" + clsGeneral.convertQuotes(txtLocationService.Text) + "',PlOneServiceLocation2='" + clsGeneral.convertQuotes(txtLocationService2.Text) + "',PlOneHoursWkPgm ='" + clsGeneral.convertQuotes(txtStateOrDistrict.Text) + "',PlOneEnrolledPrnt ='" + chkEnrolled.Checked + "',PlOnePlcdTeam	='" + chkPlaced.Checked + "',PlOneTimeMore	='" + chkTimeMore.Checked + "',PlOneTimeTwo	='" + chkOfTime.Checked + "',PlOneTimeThree	='" + chk39Time.Checked + "',PlOneSeparateClass	='" + chkSubstancialy.Checked + "',PlOneSeparateDayScl ='" + chkDaySchool.Checked + "',PlOneSeparatePublic ='" + chkPublic.Checked + "',PlOneSeparatePvt ='" + chkPrivate.Checked + "', PlOneResidentialFacility ='" + chkResidential.Checked + "', PlOneHome ='" + chkHome.Checked + "'," +
                    "PlOneServiceLctn	='" + chkService.Checked + "',PlOnePsychiatric	='" + chkDepartment.Checked + "',PlOneMassachusetts	='" + chkMassachusetts.Checked + "',PlOneMassachusettsDay	='" + chkDay.Checked + "',PlOneMassachusettsRes	='" + chkRes.Checked + "',PlOneDoctorHme		='" + chkHmeBasedPgm.Checked + "',PlOneDoctorHsptl		='" + chkHospital.Checked + "',PlOneConsent			='" + chkConcent.Checked + "',PlOneRefuse		='" + chkPlacement.Checked + "',PlOnePlacement		='" + chkRefused.Checked + "',PlOneSignParent ='" + clsGeneral.convertQuotes(txtSignParentOne.Text) + "',PlOneDate ='" + clsGeneral.convertQuotes(txtDateOne.Text) + "',PlTwoFullInclusionPgm		='" + chkFullPgm.Checked + "',PlTwoPartialPgm		='" + chkPartialPgm.Checked + "',PlTwoSubstantially		='" + chkSepClass.Checked + "',PlTwoSeparateScl	='" + chkSepDaySchool.Checked + "',PlTwoPublicScl		='" + chkPublicSep.Checked + "'," +
                    "PlTwoPrivateScl	='" + chkPrivateSep.Checked + "',PlTwoResScl		='" + chkResSchool.Checked + "',PlTwoOther		='" + chkOtherSep.Checked + "',PlTwoOtherDesc='" + clsGeneral.convertQuotes(txtOther.Text) + "',PlTwoYouth	='" + chkDetained.Checked + "',PlTwoPsychiatric	='" + chkTreatment.Checked + "',PlTwoMassachusetts		='" + chkHospitalSchool.Checked + "',PlTwoMassachusettsDay	='" + chkDayPlace.Checked + "',PltwoMassachusettsRes		='" + chkResPlace.Checked + "',PlTwoCorrectionFacility		='" + chkFacility.Checked + "',PlTwoDoctorHome		='" + chkHomeDoctor.Checked + "',PlTwoDoctorHsptl	='" + chkHospitalDoctor.Checked + "',PlTwoConsent	='" + chkConsent2.Checked + "',PlTwoPlacement	='" + chkRefuse2.Checked + "',PlTwoFullPgm='" + chkPlacement2.Checked + "',PlTwoSignParent ='" + clsGeneral.convertQuotes(txtSignParentTwo.Text) + "',PlTwoDate ='" + clsGeneral.convertQuotes(txtDateTwo.Text) + "'," +
                     "ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                    retVal = objData.Execute(strQuery);

                    if (retVal != 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                        {
                            objData.Execute("update stdtIEPUpdateStatus set Page9='true' where stdtIEPId=" + sess.IEPId);
                        }
                        else
                        {
                            objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page9) values(" + sess.IEPId + ",'true')");
                        }
                        //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP10();", true);
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

    protected void ifExist()
    {

        DataClass oData = new DataClass();
        clsSession oSession = (clsSession)Session["UserSession"];
        string selIEPstatus = "SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'";
        int IEPstatus = oData.ExecuteScalar(selIEPstatus);
        string copyIEP = "SELECT MAX(StdtIEPId) AS StdtIEPId FROM StdtIEP WHERE StudentId=" + oSession.StudentId + " ";
        int newIEP = oData.ExecuteScalar(copyIEP);

        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt4 WHERE StdtIEPId=" + newIEP + " ") == false)
        {
            string insIEP4 = "INSERT INTO StdtIEPExt4(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + newIEP + "," + IEPstatus + "," + oSession.LoginId + "," +
           "(SELECT convert(varchar, getdate(), 100)))";
            int newVersion = oData.ExecuteNonQuery(insIEP4);
        }
        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt5 WHERE StdtIEPId=" + newIEP + " ") == false)
        {
            string insIEP5 = "INSERT INTO StdtIEPExt5(StdtIEPId) VALUES(" + newIEP + ")";
            oData.ExecuteNonQuery(insIEP5);
        }
        
    }
}