using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
public partial class StudentBinder_CreateIEP10 : System.Web.UI.Page
{
    public static clsData objData = null;
    public static clsSession sess = null;
    string strQuery = "";
    string strQuery1 = "";
    string strQuery2 = "";
    string strQuery3 = "";
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
                // btnSave.Visible = false;
            }
            else
            {
                // btnSave.Visible = true;
            }
            fillBasicDetails();
            Fill();
            //fillRole();
            ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
            {
                //btnSave.Visible = false;
            }
            else
            {
                // btnSave.Visible = true;
            }
        }
    }
    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        //DataTable Dt1= null;
        DataTable Dt3= null;
        DataTable Dt4= null;
        if (sess != null)
        {
            if (sess.IEPId <= 0) return;
            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }
            objData = new clsData();
            string strQuery = "SELECT   IEP4.StdtIEPId,IEP4.LanguageofInst,IEP4.RoleDesc,IEP4.ActonOwnBehalfCk, IEP4.CourtAppGrdCk, IEP4.SharedDecMakingCk,IEP4.DelegateDeciMakCk, IEP4.CourtAppGuardian, IEP4.PrLanguageGrd1, IEP4.PrLanguageGrd2, IEP4.DateOfMeeting, IEP4.TypeOfMeeting,IEP4.AnnualReviewMeeting, IEP4.ReevaluationMeeting, IEP4.SchoolName,IEP4.SchoolPhone,IEP4.SchAddress,IEP4.SchContact,IEP4.SchTelephone, IEP4.CostSharedPnt, IEP4.SpecifyAgency FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId = " + sess.IEPId + " ";
            Dt = objData.ReturnDataTable(strQuery, false);
            
            
            if (Dt.Rows.Count > 0)
            {
                IEPId = Convert.ToInt32(Dt.Rows[0]["StdtIEPId"]);
                //txtCourtAppointed.InnerHtml = Dt.Rows[0]["CourtAppGuardian"].ToString().Trim();
                TextBoxInstruction.Text = Dt.Rows[0]["LanguageofInst"].ToString().Trim();
                chkActingon.Checked = clsGeneral.getChecked(Dt.Rows[0]["ActonOwnBehalfCk"].ToString());
                chkCourtAppointed.Checked = clsGeneral.getChecked(Dt.Rows[0]["CourtAppGrdCk"].ToString());
                chkDecisionMaking.Checked = clsGeneral.getChecked(Dt.Rows[0]["SharedDecMakingCk"].ToString());
                ChkDelegateDecision.Checked = clsGeneral.getChecked(Dt.Rows[0]["DelegateDeciMakCk"].ToString());
                txtCourtAppointed.Text = Dt.Rows[0]["CourtAppGuardian"].ToString().Trim();
                TextBoxPrLanguageParent.Text = Dt.Rows[0]["PrLanguageGrd1"].ToString().Trim();
                TextBoxPrLanguageParent1.Text = Dt.Rows[0]["PrLanguageGrd2"].ToString().Trim();
                //if (Dt.Rows[0]["DateOfMeeting"].ToString().Trim() != null && Dt.Rows[0]["DateOfMeeting"].ToString().Trim() != "")
                if ((Dt.Rows[0]["DateOfMeeting"].ToString().Trim()) == "01-01-1900 AM 12:00:00" || (Dt.Rows[0]["DateOfMeeting"].ToString().Trim()) == "01-01-1900")
                {
                    TextBoxMeetingDate.Text = "";
                }
                else
                {
                    string dateVal = (Dt.Rows[0]["DateOfMeeting"]).ToString();
                    if (dateVal != "")
                    {
                        
                        dateVal = dateVal.Replace("-", "/");
                        string[] splitVal = dateVal.Split(' ');
                        string selectVal = splitVal[0];
                        TextBoxMeetingDate.Text = selectVal;
                    }
                    else
                    {
                        TextBoxMeetingDate.Text = "";
                    }
                }
               
                TextBoxMeetingType.Text = Dt.Rows[0]["TypeOfMeeting"].ToString().Trim();
                TextBoxAnnualReview.Text = Dt.Rows[0]["AnnualReviewMeeting"].ToString().Trim();
                TextBoxReevaluation.Text = Dt.Rows[0]["ReevaluationMeeting"].ToString().Trim();
                //   RadioButtonList1.Text = Dt.Rows[0]["CostSharedPnt"].ToString().Trim();
                TextBoxAgency.Text = Dt.Rows[0]["SpecifyAgency"].ToString().Trim();
                if ((Dt.Rows[0]["ActonOwnBehalfCk"].ToString() != "") || (Dt.Rows[0]["CourtAppGrdCk"].ToString() != "") || (Dt.Rows[0]["SharedDecMakingCk"].ToString() != "") || (Dt.Rows[0]["DelegateDeciMakCk"].ToString() != ""))
                {
                    // btnSave.Text = "Update";
                }
                else
                {// btnSave.Text = "Save";
                }
                txtDesc.Text= Dt.Rows[0]["RoleDesc"].ToString();

                if (Convert.ToString(Dt.Rows[0]["CostSharedPnt"]) == "True")
                {
                    RadioButtonList1.SelectedValue = "1";
                }
                else if (Convert.ToString(Dt.Rows[0]["CostSharedPnt"]) == "False")
                {
                    RadioButtonList1.SelectedValue = "0";
                }




            }
            string Query = "SELECT IEP4.SchoolName,IEP4.SchoolPhone,IEP4.SchAddress,IEP4.SchContact,IEP4.SchTelephone  FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE IEP4.SchoolName IS NOT NULL and IEP1.StdtIEPId = " + sess.IEPId + " ";
            Dt3 = objData.ReturnDataTable(Query, false);
            string stQuery = "SELECT sch.SchoolName As SName,sch.DistContact As cont,sch.DistPhone As Phone  ,Adr.AddressLine1,Adr.AddressLine2,Adr.AddressLine3 As ScAdd,sch.DistPhone As DistCon  from School Sch  LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State)  ON Sch.DistAddrId = Adr.AddressId  WHERE sch.ActiveInd = 'A'  And sch.SchoolId=" + sess.SchoolId + " ";
            Dt4 = objData.ReturnDataTable(stQuery, false);
            if (Dt3 != null)
            {
                if (Dt3.Rows.Count > 0)
                {
                    txtSchoolName.Text = Dt3.Rows[0]["SchoolName"].ToString().Trim();
                    txtSchoolPhone.Text = Dt3.Rows[0]["SchoolPhone"].ToString().Trim();
                    txtSchAddress.Text = Dt3.Rows[0]["SchAddress"].ToString().Trim();
                    txtSchContact.Text = Dt3.Rows[0]["SchContact"].ToString().Trim();
                    txtSchTelephone.Text = Dt3.Rows[0]["SchTelephone"].ToString().Trim();
                }
                else
                {
                    txtSchoolName.Text = Dt4.Rows[0]["SName"].ToString().Trim();
                    txtSchoolPhone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                    txtSchAddress.Text = Dt4.Rows[0]["ScAdd"].ToString().Trim();
                    txtSchContact.Text = Dt4.Rows[0]["cont"].ToString().Trim();
                    txtSchTelephone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                }
            }
            else
            {
                txtSchoolName.Text = Dt4.Rows[0]["SName"].ToString().Trim();
                txtSchoolPhone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
                txtSchAddress.Text = Dt4.Rows[0]["ScAdd"].ToString().Trim();
                txtSchContact.Text = Dt4.Rows[0]["cont"].ToString().Trim();
                txtSchTelephone.Text = Dt4.Rows[0]["Phone"].ToString().Trim();
            }
        }

    }
    private void fillBasicDetails()
    {
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
        TimeSpan tempDatetime;


        sess = (clsSession)Session["UserSession"];
        try
        {
            strQuery = "Select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS, ST.DOB,SP.MostRecentGradeLevel AS GradeLevel,ADR.ApartmentType,ADR.StreetName,ADR.City, ST.SchoolId,SP.SASID,ST.Gender,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) AS Phone,"
            + "(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=2)) AS Mobile from Student ST Inner Join StudentAddresRel SAR ON ST.StudentId=SAR.StudentPersonalId "
            + " INNER JOIN StudentPersonal SP ON SP.StudentPersonalId=ST.StudentPersonalId"
            + " Inner Join AddressList ADR ON SAR.AddressId=ADR.AddressId Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + " And SAR.ContactSequence=0";


            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblPhoneHome.Text = Dt.Rows[0]["Phone"].ToString().Trim();
                    lblsasid.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                    lblStudentSch.Text = Dt.Rows[0]["SchoolId"].ToString().Trim();
                    lblStudentName.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    lblsasid.Text = Dt.Rows[0]["SASID"].ToString().Trim();
                    string gender = Dt.Rows[0]["Gender"].ToString().Trim();
                    if (gender == "0")
                        lblSex.Text = " ";
                    else if (gender == "1")
                        lblSex.Text = "Male";
                    else if (gender == "2")
                        lblSex.Text = "Female";

                    lblDOB.Text = Dt.Rows[0]["DOBS"].ToString().Trim();
                    tempDatetime = DateTime.Now - Convert.ToDateTime(Dt.Rows[0]["DOB"].ToString().Trim());
                    double dats = tempDatetime.TotalDays;
                    int age = Convert.ToInt32(dats / 360);
                    if (age > 0)
                    {
                        lblAge.Text = age.ToString();
                    }
                    else lblAge.Text = "";
                    lblGrade.Text = Dt.Rows[0]["GradeLevel"].ToString().Trim();
                    lblAddress.Text = Dt.Rows[0]["ApartmentType"].ToString().Trim() + "</br>" + Dt.Rows[0]["StreetName"].ToString().Trim() + "</br>" + Dt.Rows[0]["City"].ToString().Trim();
                    // sess.IEPId = Convert.ToInt32(hidIEPId.Value);
                }
            }


            //strQuery1 = "Select distinct AL.AddressLine1+','+AL.AddressLine2+','+AL.AddressLine3 AS PrimaryContactAddress,CP.LastName+','+CP.FirstName As GuardianName,SP.PlaceOfBirth As PB,SP.PrimaryLanguage As PrLang,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone,AL.Mobile AS CellPhone,AL.PrimaryEmail AS Email from AddressList AL"
            //            + " Inner Join StudentAddresRel ADR on AL.AddressId=ADR.AddressId"
            //            + " Inner Join StudentPersonal SP on SP.StudentPersonalId=ADR.StudentPersonalId"
            //            + " Inner join ContactPersonal CP on CP.StudentPersonalId=SP.StudentPersonalId"
            //            + " Inner join StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
            //            + " Inner join LookUp LK on LK.LookupId=SCR.RelationshipId"
            //           + " where ADR.ContactSequence=1  AND SP.StudentPersonalId=" + sess.StudentId + " And SP.StudentType='Client' "
            //            + " And SP.SchoolId=" + sess.SchoolId + "  And LK.LookupName='Legal Guardian 1' AND CP.Status=1";
            //Dt = objData.ReturnDataTable(strQuery1, false);

            string Query1 = "SELECT SP.PlaceOfBirth As PB,SP.PrimaryLanguage As PrLang FROM StudentPersonal SP WHERE StudentPersonalId='" + sess.StudentId + "'";
            DataTable DT1 = objData.ReturnDataTable(Query1, false);

            if (DT1 != null)
            {
                if (DT1.Rows.Count > 0)
                {
                    lblPlace.Text = DT1.Rows[0]["PB"].ToString().Trim();
                    lblPrLanguage.Text = DT1.Rows[0]["PrLang"].ToString().Trim();
                }
            }

            string Query2 = "SELECT CP.LastName+','+CP.FirstName As GuardianName FROM ContactPersonal CP INNER JOIN StudentContactRelationship SCR ON CP.ContactPersonalId=SCR.ContactPersonalId INNER JOIN LookUp LK on LK.LookupId=SCR.RelationshipId WHERE LK.LookupName='Legal Guardian' AND CP.StudentPersonalId='"+sess.StudentId+"' ";
            DataTable DT2 = objData.ReturnDataTable(Query2, false);

            if (DT2 != null)
            {
                if (DT2.Rows.Count > 0)
                {
                    string GuardianName = "";
                    foreach (DataRow data in DT2.Rows)
                    {
                        GuardianName += data["GuardianName"].ToString() + ";";
                    }
                    lblName.Text = GuardianName;
                    lblRelationship.Text = "Legal Guardian";
                }
            }

            string Query3 = "SELECT AL.StreetName+','+AL.ApartmentType+','+AL.City AS PrimaryContactAddress,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone FROM AddressList AL"
            + " INNER JOIN StudentAddresRel ADR ON AL.AddressId=ADR.AddressId"
            + " INNER JOIN StudentPersonal SP ON SP.StudentPersonalId=ADR.StudentPersonalId"
            + " INNER JOIN ContactPersonal CP on CP.ContactPersonalId=ADR.ContactPersonalId"
            + " INNER JOIN StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
            + " INNER JOIN LookUp LK on LK.LookupId=SCR.RelationshipId"
            + " WHERE SP.StudentPersonalId='" + sess.StudentId + "' AND ADR.ContactSequence=1 AND LK.LookupName='Legal Guardian' AND CP.Status=1";
            DataTable DT3 = objData.ReturnDataTable(Query3, false);
            if (DT3 != null)
            {
                if (DT3.Rows.Count > 0)
                {

                    lblstudAddress.Text = DT3.Rows[0]["PrimaryContactAddress"].ToString().Trim();
                    lblstudPhoneHome.Text = DT3.Rows[0]["HomePhone"].ToString().Trim();
                    lblstudPhoneOther.Text = DT3.Rows[0]["WorkPhone"].ToString().Trim();
                }

            }


            strQuery2 = "Select distinct AL.StreetName+','+AL.ApartmentType+','+AL.City AS PrimaryContactAddress,CP.LastName+','+CP.FirstName As GuardianName,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone,AL.Mobile AS CellPhone,AL.PrimaryEmail AS Email from AddressList AL"
                        + " Inner Join StudentAddresRel ADR on AL.AddressId=ADR.AddressId"
                        + " Inner Join StudentPersonal SP on SP.StudentPersonalId=ADR.StudentPersonalId"
                        + " Inner join ContactPersonal CP on CP.ContactPersonalId=ADR.ContactPersonalId"
                        + " Inner join StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
                        + " Inner join LookUp LK on LK.LookupId=SCR.RelationshipId"
                       + "  where ADR.ContactSequence=1  AND SP.StudentPersonalId=" + sess.StudentId + " And SP.StudentType='Client' "
                        + " And SP.SchoolId=" + sess.SchoolId + "  And LK.LookupName='Parent' AND CP.Status=1";
            Dt = objData.ReturnDataTable(strQuery2, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblName1.Text = Dt.Rows[0]["GuardianName"].ToString().Trim();
                    lblRelationship1.Text = "Parent";
                    lblstudAddress1.Text = Dt.Rows[0]["PrimaryContactAddress"].ToString().Trim();
                    lblstudPhoneHome1.Text = Dt.Rows[0]["HomePhone"].ToString().Trim();
                    lblstudPhoneOther1.Text = Dt.Rows[0]["WorkPhone"].ToString().Trim();

                }

            }
            Dt = null;

            strQuery3 = "SELECT sch.SchoolId,sch.SchoolName As SName,sch.SchoolDesc,sch.DistrictName,sch.DistContact As cont,sch.DistPhone As Phone ,Adr.AddressLine1,Adr.AddressLine2,Adr.AddressLine3 As ScAdd,sch.DistContact +','+ sch.DistPhone As DistCon " +
                               " from School Sch " +
                                 "   LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State) " +
                                            "  ON Sch.DistAddrId = Adr.AddressId  WHERE sch.ActiveInd = 'A'  And sch.SchoolId=" + sess.SchoolId + " ";
            Dt = objData.ReturnDataTable(strQuery3, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    // lblSchName.Text = Dt.Rows[0]["DistrictName"].ToString().Trim();
                    //lblSchAdd.Text = Dt.Rows[0]["ScAdd"].ToString().Trim();
                  //  lblSchoolPhone.Text = Dt.Rows[0]["Phone"].ToString().Trim();
                    //lblSchCon.Text = Dt.Rows[0]["DistCon"].ToString().Trim();
                    lblSchoolName.Text = Dt.Rows[0]["SName"].ToString().Trim();
                    lblSchContact.Text = Dt.Rows[0]["cont"].ToString().Trim();
                    lblSchTelephone.Text = Dt.Rows[0]["Phone"].ToString().Trim();
                    lblSchAddress.Text = Dt.Rows[0]["ScAdd"].ToString().Trim();

                }

            }



        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    private void ViewAccReject()
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        int reject = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Rejected'");
        int approve = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        if (sess.IEPStatus == approve || sess.IEPStatus == reject)
        {
            // btnSave.Visible = false;
        }

    }


    //private void fillRole()
    //{

    //    objData = new clsData();

    //    objData.ReturnDropDown("SELECT DISTINCT Role.RoleId as Id,Role.RoleDesc as Name FROM Role  order by RoleDesc Asc", ddlRole);
    //}

    protected void btnSave_Click(object sender, EventArgs e)
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
            Update();
            //Fill();
        }
        //if (btnSave.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}

    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
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
            //Fill();
        }
        //if (btnSave.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}

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
                ifExist();
                IEPId = sess.IEPId;
               // strQuery = " Update StdtIEPExt4 set StdtIEPId=" + IEPId + ",LanguageofInst='" + TextBoxInstruction.Text + "',ActonOwnBehalfCk='" + chkActingon.Checked + "',CourtAppGrdCk='" + chkCourtAppointed.Checked + "',SharedDecMakingCk='" + chkDecisionMaking.Checked + "',DelegateDeciMakCk='" + ChkDelegateDecision.Checked + "',CourtAppGuardian='" + txtCourtAppointed.Text + "',PrLanguageGrd1='" + TextBoxPrLanguageParent.Text + "',PrLanguageGrd2='" + TextBoxPrLanguageParent1.Text + "',DateOfMeeting=CONVERT(datetime,'" + TextBoxMeetingDate.Text + "',101),TypeOfMeeting='" + TextBoxMeetingType.Text + "',AnnualReviewMeeting='" + TextBoxAnnualReview.Text + "',ReevaluationMeeting='" + TextBoxReevaluation.Text + "',SchoolName='"+txtSchoolName.Text+"',SchoolPhone='"+txtSchoolPhone.Text+"',SchAddress='"+txtSchAddress.Text+"',SchContact='"+txtSchContact.Text+"',SchTelephone='"+txtSchTelephone.Text+"',RoleDesc='" + txtDesc.Text + "',CostSharedPnt='" + RadioButtonList1.Text + "',SpecifyAgency='" + TextBoxAgency.Text + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";
                strQuery = " Update StdtIEPExt4 set StdtIEPId=" + IEPId + ",LanguageofInst='" + TextBoxInstruction.Text + "',ActonOwnBehalfCk='" + chkActingon.Checked + "',CourtAppGrdCk='" + chkCourtAppointed.Checked + "',SharedDecMakingCk='" + chkDecisionMaking.Checked + "',DelegateDeciMakCk='" + ChkDelegateDecision.Checked + "',CourtAppGuardian='" + txtCourtAppointed.Text + "',PrLanguageGrd1='" + TextBoxPrLanguageParent.Text + "',PrLanguageGrd2='" + TextBoxPrLanguageParent1.Text + "',DateOfMeeting='" + TextBoxMeetingDate.Text + "',TypeOfMeeting='" + TextBoxMeetingType.Text + "',AnnualReviewMeeting='" + TextBoxAnnualReview.Text + "',ReevaluationMeeting='" + TextBoxReevaluation.Text + "',SchoolName='" + txtSchoolName.Text + "',SchoolPhone='" + txtSchoolPhone.Text + "',SchAddress='" + txtSchAddress.Text + "',SchContact='" + txtSchContact.Text + "',SchTelephone='" + txtSchTelephone.Text + "',RoleDesc='" + txtDesc.Text + "',CostSharedPnt='" + RadioButtonList1.Text + "',SpecifyAgency='" + TextBoxAgency.Text + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE()  where StdtIEPId=" + IEPId + " ";
                retVal = objData.Execute(strQuery);

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page10='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page10) values(" + sess.IEPId + ",'true')");
                    }
                    //Clear();
                    ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(11);", true);
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
                ifExist();
                IEPId = sess.IEPId;
                strQuery = " Update StdtIEPExt4 set StdtIEPId=" + IEPId + ",LanguageofInst='" + TextBoxInstruction.Text + "',ActonOwnBehalfCk='" + chkActingon.Checked + "',CourtAppGrdCk='" + chkCourtAppointed.Checked + "',SharedDecMakingCk='" + chkDecisionMaking.Checked + "',DelegateDeciMakCk='" + ChkDelegateDecision.Checked + "',CourtAppGuardian='" + txtCourtAppointed.Text + "',PrLanguageGrd1='" + TextBoxPrLanguageParent.Text + "',PrLanguageGrd2='" + TextBoxPrLanguageParent1.Text + "',DateOfMeeting='" + TextBoxMeetingDate.Text + "',TypeOfMeeting='" + TextBoxMeetingType.Text + "'AnnualReviewMeeting='" + TextBoxAnnualReview.Text + "',ReevaluationMeeting='" + TextBoxReevaluation.Text + "',SchoolName='" + txtSchoolName.Text + "',SchoolPhone='" + txtSchoolPhone.Text + "',SchAddress='" + txtSchAddress.Text + "',SchContact='" + txtSchContact.Text + "',SchTelephone='" + txtSchTelephone.Text + "',RoleDesc='" + txtDesc.Text + "',CostSharedPnt='" + RadioButtonList1.Text + "',SpecifyAgency='" + TextBoxAgency.Text + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate()  where StdtIEPId=" + IEPId + " ";

                retVal = objData.Execute(strQuery);

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page10='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page10) values(" + sess.IEPId + ",'true')");
                    }
                    //Clear();
                   // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP11();", true);
                }

            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }
    private void Save()
    {
        try
        {

            sess = (clsSession)Session["UserSession"];
            objData = new clsData();
            IEPId = Convert.ToInt32(objData.FetchValue("Select [StdtIEPId] from [dbo].[StdtIEP] where SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + " and EffEndDate is Null"));
            if (IEPId > 0)
            {

                strQuery = " Insert into StdtIEPExt4( LanguageofInst, ActonOwnBehalfCk, CourtAppGrdCk, SharedDecMakingCk, DelegateDeciMakCk,CourtAppGuardian,PrLanguageGrd1,PrLanguageGrd2,DateOfMeeting,TypeOfMeeting,AnnualReviewMeeting,ReevaluationMeeting,CostSharedPnt,SpecifyAgency,SchoolName,SchoolPhone,SchAddress,SchContact,SchTelephone) Values  ('" + TextBoxInstruction.Text + "','" + chkActingon.Checked + "','" + chkCourtAppointed.Checked + "','" + chkDecisionMaking.Checked + "','" + ChkDelegateDecision.Checked + "','" + txtCourtAppointed.Text + "','" + TextBoxPrLanguageParent.Text + "','" + TextBoxPrLanguageParent1.Text + "','" + TextBoxMeetingDate.Text + "','" + TextBoxMeetingType.Text + "','" + TextBoxAnnualReview.Text + "','" + TextBoxReevaluation.Text + "','" + RadioButtonList1.Text + "','" + TextBoxAgency.Text + "','" + txtSchoolName.Text + "','" + txtSchoolPhone.Text + "','" + txtSchAddress.Text + "','" + txtSchContact.Text + "','" + txtSchTelephone.Text + "')";
                retVal = objData.Execute(strQuery);
            }

            else if (IEPId == 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Please Save IEP Page 1 for " + sess.StudentName + "!");
                return;
            }

            if (retVal != 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Saved Successfully");
                //Clear();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation failed...");
            }

        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }

    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {

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