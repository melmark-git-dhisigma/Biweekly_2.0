using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StudentBinder_CreateIEP_PE1 : System.Web.UI.Page
{

    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    public clsSession sess = null;
    static string x = "", y = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
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


            fillBasicDetails();
            setInitialGrid();


            ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }
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
            btnSave.Visible = false;
        }

    }

    private void fillGridviews()
    {

        setInitialGrid();

    }

    //protected void Fill()
    //{
    //    objData = new clsData();
    //    clsIEP IEPObj = new clsIEP();
    //    sess = (clsSession)Session["UserSession"];
    //    strQuery = "Select StdtIEPId,Concerns, Strength, Vision FROM StdtIEP where StudentId=" + sess.StudentId + " AND StdtIEPId=" + sess.IEPId;
    //    Dt = objData.ReturnDataTable(strQuery, false);
    //    if (Dt != null)
    //    {
    //        if (Dt.Rows.Count > 0)
    //        {
    //            //txtConcerns.InnerHtml = Dt.Rows[0]["Concerns"].ToString().Trim();
    //            //txtStrengths.InnerHtml = Dt.Rows[0]["Strength"].ToString().Trim();
    //            //txtVision.InnerHtml = Dt.Rows[0]["Vision"].ToString().Trim();
    //            //hidIEPId.Value = Dt.Rows[0]["StdtIEPId"].ToString().Trim();
    //            //btnSubmitIEP1.Text = "Save and continue";
    //            //sess.IEPId = Convert.ToInt32(hidIEPId.Value);
    //        }
    //    }
    //    string Status = IEPObj.GETIEPStatus(sess.IEPId,sess.StudentId,sess.SchoolId);
    //    if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
    //    {
    //        // btnSubmitIEP1.Visible = false;
    //    }
    //    else
    //    {
    //        // btnSubmitIEP1.Visible = true;
    //    }

    //}


    private void fillBasicDetails()
    {
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
        TimeSpan tempDatetime;


        sess = (clsSession)Session["UserSession"];
        try
        {
            strQuery = "Select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS, ST.DOB,ST.GradeLevel,ADR.ApartmentType,ADR.StreetName,ADR.City,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) AS Phone,"
            + "(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=2)) AS Mobile from Student ST Inner Join StudentAddresRel SAR ON ST.StudentId=SAR.StudentPersonalId "
            + "Inner Join AddressList ADR ON SAR.AddressId=ADR.AddressId Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + " And SAR.ContactSequence=0";

            //strQuery = "select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS,ST.DOB,ST.GradeLevel,ADR.AddressLine1+'</br>'+ADR.AddressLine2+'</br>'+"
            //            + "ADR.AddressLine3 as Addres,ADR.HomePhone,ADR.Mobile from Student ST inner join Address ADR on ADR.AddressId=ST.AddressId where StudentId=" + sess.StudentId + ""
            //            + "and SchoolId=" + sess.SchoolId;

            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblPhoneHome.Text = Dt.Rows[0]["Phone"].ToString().Trim();
                    lblPhoneWork.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                    lblStudName.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    lblStudentName.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
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
            //strQuery = "select Lname+','+ Fname as Name,P.ContactPersonalId,ADR.AddressID,ADR.AddressLine1,ADR.AddressLine2,ADR.AddressLine3,ADR.Phone,ADR.Mobile  from Parent P "
            //+ " inner join StudentParentRel SPR on P.ParentID=SPR.ParentID"
            //+ " inner join StudentPersonal SP on SPR.StudentPersonalId=SP.StudentPersonalId"
            //+ " inner join StudentAddresRel SAR on SP.StudentPersonalId=SAR.StudentPersonalId"
            //+ " inner join AddressList ADR on SAR.AddressId=ADR.AddressId Where SP.StudentPersonalId=" + sess.StudentId + " And SP.SchoolId=" + sess.SchoolId + " and SAR.ContactSequence<>0";

            //Neethu 9/10/14

          //  strQuery = "select top(1) CP.LastName +','+ CP.FirstName as Name,CP.ContactPersonalId,ADR.AddressID,ADR.AddressLine1,ADR.AddressLine2,ADR.AddressLine3,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) AS Phone,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT AddressId FROM StudentAddresRel  WHERE StudentPersonalId="+sess.StudentId+" AND ContactSequence=2)) AS Mobile  from StudentPersonal SP"

          //             + " inner join StudentAddresRel SAR on SP.StudentPersonalId=SAR.StudentPersonalId"
          //             + " inner join AddressList ADR on SAR.AddressId=ADR.AddressId"
          //             + "  inner join StudentContactRelationship SCR on SAR.ContactPersonalId =SCR.ContactPersonalId"
          //             + " inner join ContactPersonal CP on SCR.ContactPersonalId=CP.ContactPersonalId"
          //             + " inner join LookUp  lk on SCR.RelationshipId=lk.LookupId"
          //              + " Where SP.StudentPersonalId=" + sess.StudentId + " And SP.SchoolId=" + sess.SchoolId + " and SAR.ContactSequence<>0  and SCR.ContactPersonalId=SAR.ContactPersonalId and lk.LookupType='Relationship' and( lk.LookupName='Legal Guardian 1' or  lk.LookupName='Legal Guardian 2')";
	  
          //   Dt = objData.ReturnDataTable(strQuery, false);
          //   if (Dt != null)
          //   {
          //       if (Dt.Rows.Count > 0)
          //       {
          //           lblPhoneHome.Text = Dt.Rows[0]["Phone"].ToString().Trim();
          //           lblPhoneWork.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
          //           //  lblOtherInformations.Text = Dt.Rows[0]["StdtIEPId"].ToString().Trim();
          //           lblAddress.Text = Dt.Rows[0]["Name"].ToString().Trim() + "</br>" + Dt.Rows[0]["AddressLine1"].ToString().Trim() + "</br>" + Dt.Rows[0]["AddressLine2"].ToString().Trim() + "</br>" + Dt.Rows[0]["AddressLine3"].ToString().Trim();
          //       }
          //   }
          //else {
          //    strQuery = "select  P.Lname+','+ P.Fname as Name,P.ContactPersonalId,ADR.AddressID,ADR.AddressLine1,ADR.AddressLine2,ADR.AddressLine3,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) AS Phone,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT AddressId FROM StudentAddresRel  WHERE StudentPersonalId="+sess.StudentId+" AND ContactSequence=2)) AS Mobile  from StudentPersonal SP"
          //                  + " inner join  StudentParentRel SPR on SP.StudentPersonalId=SPR.StudentPersonalId"
          //                  + " inner join  Parent P on SPR.ParentID=P.ParentID"
          //                  + " inner join StudentAddresRel SAR on SP.StudentPersonalId=SAR.StudentPersonalId"
          //                  + " inner join AddressList ADR on SAR.AddressId=ADR.AddressId"
          //                  + " inner join StudentContactRelationship SCR on P.ContactPersonalId=SCR.ContactPersonalId"
          //                  + " inner join LookUp  lk on SCR.RelationshipId=lk.LookupId"
          //                  + " Where SP.StudentPersonalId=" + sess.StudentId + " And SP.SchoolId=" + sess.SchoolId + "  and SAR.ContactSequence<>0 and SCR.ContactPersonalId=SAR.ContactPersonalId and lk.LookupType='Relationship' and lk.LookupName='Parent'";

          //       Dt = objData.ReturnDataTable(strQuery, false);
          //       if (Dt != null)
          //       {
          //           if (Dt.Rows.Count > 0)
          //           {
                         
          //               //  lblOtherInformations.Text = Dt.Rows[0]["StdtIEPId"].ToString().Trim();
          //               lblAddress.Text = Dt.Rows[0]["Name"].ToString().Trim() + "</br>" + Dt.Rows[0]["AddressLine1"].ToString().Trim() + "</br>" + Dt.Rows[0]["AddressLine2"].ToString().Trim() + "</br>" + Dt.Rows[0]["AddressLine3"].ToString().Trim();
          //           }
          //       }
             
             
          //   }
          //  //end

            strQuery = "SELECT Convert(varchar,IepTeamMeetingDate,101)as IepTeamMeetingDate,Convert(varchar,IepImplementationDate,101)as IepImplementationDate," +
                "AnticipatedDurationofServices,AnticipatedYearOfGraduation,LocalEducationAgency,"
                + " OtherInformation,DocumentedBy,CountyOfResidance FROM StdtIEP_PE WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    txtOtherInformation.InnerHtml = Dt.Rows[0]["OtherInformation"].ToString().Trim();

                    //txtOtherInformation.InnerHtml = txtOtherInformation.InnerHtml.Replace("##", "'");
                    //txtOtherInformation.InnerHtml = txtOtherInformation.InnerHtml.Replace("?bs;", "\\");

                    hdnOtherInformationTxt.Text = System.Uri.EscapeDataString(txtOtherInformation.InnerHtml);

                    TextBoxImplementationDate.Text = Dt.Rows[0]["IepImplementationDate"].ToString().Trim();
                    TextBoxResidenceCountry.Text = Dt.Rows[0]["CountyOfResidance"].ToString().Trim();
                    TextBoxGraduationYear.Text = Dt.Rows[0]["AnticipatedYearOfGraduation"].ToString().Trim();
                    TextBoxLEA.Text = Dt.Rows[0]["LocalEducationAgency"].ToString().Trim();
                    TextBoxSeviceDuration.Text = Dt.Rows[0]["AnticipatedDurationofServices"].ToString().Trim();
                    TextBoxDocumentedBy.InnerHtml = Dt.Rows[0]["DocumentedBy"].ToString().Trim();

                    //TextBoxDocumentedBy.InnerHtml = TextBoxDocumentedBy.InnerHtml.Replace("##", "'");
                    //TextBoxDocumentedBy.InnerHtml = TextBoxDocumentedBy.InnerHtml.Replace("?bs;", "\\");

                    hdnFieldDocByText.Text = System.Uri.EscapeDataString(TextBoxDocumentedBy.InnerHtml);

                    TextBoxMeetingDate.Text = Dt.Rows[0]["IepTeamMeetingDate"].ToString().Trim();




                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    public string ConvertDate(DateTime dateString)
    {
        string result = "";
        DateTime temp = (DateTime)dateString;
        result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
        return result;
    }

    [WebMethod]
    public static void submitIepPE1(string arg1, string arg2)
    {
        x = arg1;
        y = arg2;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string otherInfo = System.Uri.UnescapeDataString(hdnOtherInformationTxt.Text);
        string docBy = System.Uri.UnescapeDataString(hdnFieldDocByText.Text);
        
        saveTodb(otherInfo, docBy);
        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(2);", true);
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        string otherInfo = System.Uri.UnescapeDataString(hdnOtherInformationTxt.Text);
        string docBy = System.Uri.UnescapeDataString(hdnFieldDocByText.Text);
        saveTodb(otherInfo, docBy);

    }



    public string saveTodb(string x, string y)
    {
        clsData objData = new clsData();
        DataClass oData = new DataClass();
        string result = "";
        string strQuery = "";
        sess = (clsSession)Session["UserSession"];
        try
        {
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
                            strQuery = "update StdtIEP_PE set IepTeamMeetingDate='" + TextBoxMeetingDate.Text + "',IepImplementationDate='" + TextBoxImplementationDate.Text + "',CountyOfResidance='" + TextBoxResidenceCountry.Text + "',"
                                    + "AnticipatedYearOfGraduation='" + TextBoxGraduationYear.Text + "',LocalEducationAgency='" + TextBoxLEA.Text + "',AnticipatedDurationofServices='" + TextBoxSeviceDuration.Text + "',"
                                    + "DocumentedBy='" + clsGeneral.convertQuotes(y) + "',OtherInformation='" + clsGeneral.convertQuotes(x) + "' where StdtIEP_PEId=" + sess.IEPId + " and SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId;
                            int id = oData.ExecuteNonQuery(strQuery);
                            if (id > 0)
                            {
                                try
                                {
                                    SaveIEPPage();
                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        return result;
    }


    //Add new Rows


    //Delete Row

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        // fillGoalInAllDropdown(1);
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        // RemoveRowFromGrid();
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex + 1;
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTable"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["PreviousTable"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeA.DataSource = dt;
            gvDelTypeA.DataBind();
            int rowIndex = 0;
            foreach (GridViewRow row in gvDelTypeA.Rows)
            {
                TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                //*********************************************************************************************
                box0.Text = dt.Rows[rowIndex]["DateOfRevisions"].ToString();
                box1.Text = dt.Rows[rowIndex]["Participants"].ToString();
                box2.Text = dt.Rows[rowIndex]["IEPSections"].ToString();
                //box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                //box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                //box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();


                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
                LinkButton1.Visible = true;
            }


        }
        else if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data
                    dt.Rows.Remove(dt.Rows[dt.Rows.Count - 1]);
                }
            }
            ViewState["CurrentTable"] = dt;
            //Re bind the GridView for the updated data
            gvDelTypeA.DataSource = dt;
            gvDelTypeA.DataBind();
            int rowIndex = 0;

            //Set Previous Data on Postbacks
            SetPreviousData();
            foreach (GridViewRow row in gvDelTypeA.Rows)
            {

                TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                //*********************************************************************************************
                box0.Text = dt.Rows[rowIndex]["DateOfRevisions"].ToString();
                box1.Text = dt.Rows[rowIndex]["Participants"].ToString();
                box2.Text = dt.Rows[rowIndex]["IEPSections"].ToString();
                //box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                //box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                //box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();
                rowIndex++;
            }
            if (dt.Rows.Count > 1)
            {
                LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
                LinkButton1.Visible = true;
            }
        }

    }

    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                    //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                    //**********************************************************************************************************
                    box0.Text = dt.Rows[i]["DateOfRevisions"].ToString();
                    box1.Text = dt.Rows[i]["Participants"].ToString();
                    box2.Text = dt.Rows[i]["IEPSections"].ToString();
                    //box3.Text = dt.Rows[i]["FreqDurDesc"].ToString();
                    //box4.Text = dt.Rows[i]["StartDate"].ToString();
                    //box5.Text = dt.Rows[i]["EndDate"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTable"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButton1 = gvDelTypeA.FooterRow.FindControl("LinkButton1") as LinkButton;
            LinkButton1.Visible = true;
        }
    }

    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];

            if (dtCurrentTable.Rows.Count > 4)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 5 Rows");
                return;
            }

            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                    //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    // Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");


                    drCurrentRow = dtCurrentTable.NewRow();

                    //***************************************************************************************
                    dtCurrentTable.Rows[i - 1]["DateOfRevisions"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["Participants"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["IEPSections"] = box2.Text;
                    //dtCurrentTable.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    //dtCurrentTable.Rows[i - 1]["StartDate"] = box4.Text;
                    //dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["IEPPA1ExtensionId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PreviousTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();

                SetPreviousDB();
            }
        }
        else if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                    //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    //Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    drCurrentRow = dtCurrentTable.NewRow();
                    //**************************************************************************************************
                    dtCurrentTable.Rows[i - 1]["DateOfRevisions"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["Participants"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["IEPSections"] = box2.Text;
                    //dtCurrentTable.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    //dtCurrentTable.Rows[i - 1]["StartDate"] = box4.Text;
                    //dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["IEPPA1ExtensionId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();

                //Set Previous Data on Postbacks
                SetPreviousData();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
    }

    private void SetPreviousDB()
    {
        int rowIndex = 0;
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTable"];
            if (dt.Rows.Count > 0)
            {
                // Response.Write(gvDelTypeA.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                    //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");

                    //*********************************************************************************************

                    // DateofRevision Participants IEPSection
                    box0.Text = dt.Rows[rowIndex]["DateOfRevisions"].ToString();
                    box1.Text = dt.Rows[rowIndex]["Participants"].ToString();
                    box2.Text = dt.Rows[rowIndex]["IEPSections"].ToString();
                    //box3.Text = dt.Rows[rowIndex]["FreqDurDesc"].ToString();
                    //box4.Text = dt.Rows[rowIndex]["StartDate"].ToString();
                    //box5.Text = dt.Rows[rowIndex]["EndDate"].ToString();


                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTable"];

    }

    private void setInitialGrid()
    {
        Int32 i = 0;
        int j = 0;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }

        DataTable dt = new DataTable();

        //***************************************************************************************************************
        dt.Columns.Add("IEPPA1ExtensionId", typeof(string));
        dt.Columns.Add("DateOfRevisions", typeof(string));
        dt.Columns.Add("Participants", typeof(string));
        dt.Columns.Add("IEPSections", typeof(string));

        //=============================================

        //if (dt_stdGoal.Rows.Count > 0)
        //{

        string getPageOneGridDetails = "select [IEPPA1ExtensionId],Convert(varchar,DateOfRevisions,101) as DateOfRevisions,[Participants],[IEPSections] from [dbo].[IEPPA1Extension] where [IepPAId]=" + sess.IEPId;
        DataTable dt_goalDetails = objData.ReturnDataTable(getPageOneGridDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    //======================================================
                    //dt.Rows.Add(dr["Participants"].ToString(), dr["Id"].ToString(), dr["SvcTypDesc"].ToString(), dr["IEPSection"].ToString(), dr["FreqDurDesc"].ToString(), dr["StartDate"].ToString(), dr["EndDate"].ToString());
                    dt.Rows.Add(dr["IEPPA1ExtensionId"].ToString(), dr["DateOfRevisions"].ToString(), dr["Participants"].ToString(), dr["IEPSections"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("", "", "", "");

            }
        }
        else
        {
            dt.Rows.Add("", "", "", "");

        }
        //}
        //else
        //{
        //    dt.Rows.Add("", "0", "", "", "", "", "");
        //}


        ViewState["PreviousTable"] = dt;

        gvDelTypeA.DataSource = dt;
        gvDelTypeA.DataBind();

    }

    protected void gvDelTypeA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lbl_goalId = (Label)e.Row.Cells[1].FindControl("lbl_goalId");
            Label lbl_svcGoalId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcGoalId != null)
            {
                svcGoalId = (lbl_svcGoalId.Text == "") ? -1 : Convert.ToInt32(lbl_svcGoalId.Text);
            }
        }
    }

    protected void gvDelTypeA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        if (e.CommandArgument.ToString() != "")
        {
            index = int.Parse(e.CommandArgument.ToString());
        }
        else
        {
            return;
        }
        //int index = int.Parse(e.CommandArgument.ToString());
        GridViewRow row = gvDelTypeA.Rows[index];

        if (e.CommandName == "remove")
        {
            if (gvDelTypeA.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from [dbo].[IEPPA1Extension] where IEPPA1ExtensionId=" + StdtGoalSvcId;

                    int i = objData.Execute(delRow);
                    deleteRowA(index);
                }
                else
                {
                    deleteRowA(index);
                }


            }
        }


    }

    private void deleteRowA(int rowID)
    {
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values

                    TextBox box0 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRevisionDate");
                    TextBox box1 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtRoles");
                    TextBox box2 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtSection");
                    //TextBox box3 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtFrequencyA");
                    //TextBox box4 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtStartDateA");
                    //TextBox box5 = (TextBox)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("txtEndDateA");
                    //    Label lbl_goalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[1].FindControl("lbl_goalId");
                    Label lbl_svcGoalId = (Label)gvDelTypeA.Rows[rowIndex].Cells[0].FindControl("lbl_svcid");

                    //HiddenField box4 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtstartDate");
                    //HiddenField box5 = (HiddenField)gvDelTypeA.Rows[rowIndex].Cells[5].FindControl("txtendDate");
                    // DateofRevision Participants IEPSection

                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i - 1]["DateOfRevisions"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["Participants"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["IEPSections"] = box2.Text;
                    //dtCurrentTable.Rows[i - 1]["FreqDurDesc"] = box3.Text;
                    //dtCurrentTable.Rows[i - 1]["StartDate"] = box4.Text;
                    //dtCurrentTable.Rows[i - 1]["EndDate"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["IEPPA1ExtensionId"] = lbl_svcGoalId.Text;

                    rowIndex++;
                }
                // dtCurrentTable.Rows.Add(drCurrentRow);

                dtCurrentTable.Rows.Remove(dtCurrentTable.Rows[rowID]);

                ViewState["PreviousTable"] = dtCurrentTable;

                gvDelTypeA.DataSource = dtCurrentTable;
                gvDelTypeA.DataBind();



                SetPreviousDB();
            }
        }
    }

    protected void gvDelTypeA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected bool SaveIEPPage()
    {
        bool getA = true;
        objData = new clsData();


        try
        {
            sess = (clsSession)Session["UserSession"];
            foreach (GridViewRow diTypeA in gvDelTypeA.Rows)
            {
                objData = new clsData();

                TextBox txtRevisionDate = diTypeA.FindControl("txtRevisionDate") as TextBox;
                TextBox txtRoles = diTypeA.FindControl("txtRoles") as TextBox;
                TextBox txtSection = diTypeA.FindControl("txtSection") as TextBox;
                //TextBox txtFrequencyA = diTypeA.FindControl("txtFrequencyA") as TextBox;
                //TextBox txtStartDateA = diTypeA.FindControl("txtStartDateA") as TextBox;
                //TextBox txtEndDateA = diTypeA.FindControl("txtEndDateA") as TextBox;

                Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
                int Id = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);

                string insDelivery = "";
                if (Id == 0)
                {

                    insDelivery = "insert into [dbo].[IEPPA1Extension] ([IepPAId],[DateOfRevisions],[Participants],[IEPSections],[CreatedBy],[CreatedOn]) " +
                                          "VALUES ('" + sess.IEPId + "', '" + txtRevisionDate.Text + "', '" + txtRoles.Text + "', '" + txtSection.Text + "'," +
                                          "'" + sess.LoginId + "' ,GETDATE())";


                    int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);


                }

                else
                {
                    insDelivery = "Update IEPPA1Extension set DateOfRevisions='" + clsGeneral.convertQuotes(txtRevisionDate.Text) + "',"
                    + "Participants='" + clsGeneral.convertQuotes(txtRoles.Text) + "',ModifiedBy=" + sess.LoginId + ","
                    + "IEPSections='" + clsGeneral.convertQuotes(txtSection.Text) + "',ModifiedOn=getdate()  where IEPPA1ExtensionId=" + Id;

                    int i = objData.Execute(insDelivery);
                }

                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");

                if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                {
                    objData.Execute("update StdtIEP_PEUpdateStatus set Page1='true' where stdtIEPId=" + sess.IEPId);
                }
                else
                {
                    objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page1) values(" + sess.IEPId + ",'true')");
                }

            }
        }
        catch (SqlException Ex)
        {
            getA = false;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed!");
            return false;
            throw Ex;
        }
        return getA;
    }

}