<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Facesheet.aspx.cs" Inherits="Administration_Facesheet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="CSS/styleFacesheet.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">

        <div class="ContentAreaContainer" runat="server" id="divFacesheet">
            &nbsp;<div class="titleContainer">
                <table style="width: 364px !important; min-width: 364px !important;">
                    <tr>
                        <td width="260px;">
                            <h2 class="ri">Student Face Sheet</h2>
                            <h3 class="ri" id="tdCapitalName" runat="server"></h3>
                        </td>
                        <td>
                            <asp:Button ID="btnExport"  class="ExportWord" runat="server" Text="" OnClick="btnExport_Click" OnClientClick=" alert(' Please Wait... Downloading in Progress..This should take no more than 3 minutes to complete');" />
                            <%--<input type="submit" class="ExportWord" title="Export To Word" id="btExport" value="" name="btnExportWord">--%>
                            </td>
                    </tr>
                </table>
            </div>
            <div class="clear"></div>

            <h2>This document provides critical information to be used in the event of an emergency involving a Melmark New England student.</h2>

            <br />
            <table border="0" cellpadding="0" cellspacing="0" class="inr">


                <% System.Data.DataTable Dt = getStudentDetailsNE("SM");
                   if (Dt.Rows.Count > 0 && Dt != null)
                   {

                       for (int i = 0; i < Dt.Rows.Count; i++)
                       {
                           //tdCapitalName.InnerText = Dt.Rows[0]["LastName"] + "," + Dt.Rows[0]["FirstName"] + "," + Dt.Rows[0]["MiddleName"].ToString();
                           tdCapitalName.InnerText = Dt.Rows[0]["Name"].ToString();
                           if (Dt.Rows[0]["ImageUrl"].ToString() == "defaultStudent.png" || Dt.Rows[0]["ImageUrl"].ToString() == "" || Dt.Rows[0]["ImageUrl"] == null)
                           {
                               dvPhoto.InnerHtml = "<img id='imgStudPhoto' height='150px' width='150px' src='../Administration/StudentsPhoto/defaultStudent.png'>";
                           }
                           else
                           {
                               dvPhoto.InnerHtml = "<img id='imgStudPhoto' height='150px' width='150px' src=data:image/gif;base64," + Dt.Rows[0]["ImageUrl"] + ">";
                           }  
                %>


                <tr>
                    <td colspan="2" rowspan="6" class="top">
                        <div id="dvPhoto" style="text-align: center" runat="server"></div>
                    </td>
                    <td class="top" width="10%">Legal Name (Last, First, MI)</td>
                    <td class="top righ" width="25%" id="tdName"><%=Dt.Rows[0]["Name"]%></td>
                </tr>
                <tr>
                    <td width="10%">Nickname</td>
                    <td class="righ" width="25%" id="tdNickName"><%=Dt.Rows[0]["NickName"] %></td>
                </tr>
                <tr>
                    <td width="10%">Date of Birth</td>
                    <td class="righ" width="25%" id="tdDob"><%=Dt.Rows[0]["BirthDate"] %></td>
                </tr>
                <tr>
                    <td width="10%">Current Address or Residential Service Setting</td>
                    <td class="righ" width="25%" id="tdAddress"><%=Dt.Rows[0]["Address"] %>
                    </td>
                </tr>
                <tr>
                    <td width="10%">Date of Admission</td>
                    <td class="righ" width="25%" id="tdAdmissionDate"><%=Dt.Rows[0]["AdmissionDate"] %></td>
                </tr>
                <tr>
                    <td width="10%">Place of Birth</td>
                    <td class="righ" width="25%" id="tdPlaceOfBirth"><%=Dt.Rows[0]["PlaceOfBirth"] %> </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">Picture Date taken:<%=Dt.Rows[0]["Photodate"] %></td>
                    <td>Citizenship</td>
                    <td class="righ" id="tdCitizenship"><%=Dt.Rows[0]["CountryOfCitizenship"] %></td>
                </tr>
                <tr>
                    <td width="10%">Race</td>
                    <td width="10%" id="tdRace"><%=Dt.Rows[0]["Race"] %></td>
                    <td>Primary Language</td>
                    <td class="righ" id="tdPLang"><%=Dt.Rows[0]["PrimaryLanguage"] %></td>
                </tr>
                <tr>
                    <td width="10%">Height (date)</td>
                    <td width="10%" id="tdHeight"><%=Dt.Rows[0]["Height"] %> in(<%=Dt.Rows[0]["modified"] %>)</td>
                    <td>Gender</td>
                    <td class="righ" id="tdGender"><%=Dt.Rows[0]["Gender"] %></td>
                </tr>
                <tr>
                    <td width="10%">Weight (date)</td>
                    <td width="10%" id="tdWeight"><%=Dt.Rows[0]["Weight"] %> lbs(<%=Dt.Rows[0]["modified"] %>)</td>
                    <td>Legal Competency Status</td>
                    <td class="righ" id="tdCompS"><%=Dt.Rows[0]["LegalCompetencyStatus"] %></td>
                </tr>
                <tr>
                    <td width="10%">Hair Color</td>
                    <td width="10%" id="tdHaiColor"><%=Dt.Rows[0]["HairColor"] %></td>
                    <td>Guardianship Status</td>
                    <td class="righ" id="tdGuardians"><%=Dt.Rows[0]["GuardianShip"] %></td>
                </tr>
                <tr>
                    <td width="10%">Eye Color</td>
                    <td width="10%" id="tdEyeColor"><%=Dt.Rows[0]["EyeColor"] %></td>
                    <td>Other State Agencies Involved With Student</td>
                    <td class="righ" id="tdOtherState"><%=Dt.Rows[0]["OtherStateAgenciesInvolvedWithStudent"] %></td>
                </tr>
                <tr>
                    <td width="10%">Distinguishing Marks</td>
                    <td width="10%" id="tdDistinguishMarks"><%=Dt.Rows[0]["DistingushingMarks"] %></td>
                    <td>Marital Status of Both Parents</td>
                    <td class="righ" id="tdMaritulParents"><%=Dt.Rows[0]["MaritalStatusofBothParents"] %></td>
                </tr>
                <tr>
                    <td colspan="2">Case Manager Residential</td>
                    <td colspan="2" class="righ" id="tdCaseManagerR"><%=Dt.Rows[0]["CaseManagerResidential"] %></td>
                </tr>
                <tr>
                    <td colspan="2">Case Manager Educational</td>
                    <td colspan="2" class="righ" id="tdCaseManagerE"><%=Dt.Rows[0]["CaseManagerEducational"] %></td>
                </tr>
                <tr>
                    <td colspan="2">Educational Surrogate:(if applicable)</td>
                    <td colspan="2" class="righ" id="tdEducationSurrogate"><%=Dt.Rows[0]["EducationalSurrogate"] %></td>
                </tr>

                <%}
                    } %>
            </table>
            <h2>Emergency Contacts – Personal</h2>

            <table border="0" cellpadding="0" cellspacing="0" class="inr">

                <%
                    Dt = getStudentDetailsNE("ED");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {                 
                %>

                <tr>
                    <td width="25px" rowspan="4" class="top bgClr"><% =(i+1).ToString() %></td>
                    <td class="top" width="10%">Relation</td>
                    <td class="top" width="10%"><% =Dt.Rows[i]["Relation"]%></td>
                    <td class="top" width="10%">Full Name</td>
                    <td class="top" width="20%"><% =Dt.Rows[i]["Name"]%></td>
                    <td class="top" width="15%">Primary Language</td>
                    <td class="top righ" width="35%"><% =Dt.Rows[i]["PrimaryLanguage"]%></td>
                </tr>
                <tr>
                    <td rowspan="3" colspan="1">Address</td>
                    <td width="15%" colspan="3" rowspan="3"><% =Dt.Rows[i]["Address"] %>
      
                    </td>
                    <td width="15%">Home Phone</td>
                    <td class="righ" width="15%"><% =Dt.Rows[i]["Phone"]%></td>
                </tr>
                <tr>
                    <td width="15%">Other Phone</td>
                    <td class="righ" width="15%"><% =Dt.Rows[i]["OtherPhone"]%></td>
                </tr>
                <tr>
                    <td width="15%"><strong>E-mail</strong></td>
                    <td class="righ" width="15%"><% =Dt.Rows[i]["PrimaryEmail"]%></td>
                </tr>


                <%}
                    }
                    else
                    {%>
                <tr>
                    <td class="top" colspan="7">No Data Found</td>
                </tr>
                <%
                   }
                   
                   
                %>
            </table>
            <h2>Emergency Contacts – School</h2>
            <table border="0" cellpadding="0" cellspacing="0" class="inr">

                <%
                    Dt = getStudentDetailsNE("SD");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            string NameTitle = Dt.Rows[i]["FullName"].ToString();
                            //string fullName = "";
                            //string title = "";
                            //if (NameTitle != "")
                            //{
                            //    string[] splitVal = NameTitle.Split(',');
                            //    fullName = splitVal[0];
                            //    title = splitVal[1];
                            //}
                %>

                <tr>
                    <td class="top bgClr"><% =(i+1).ToString() %></td>
                    <td class="top">Full Name, Title </td>
                    <td class="top"><% =NameTitle%></td>
                    <%--<td class="top">Title</td>
                    <td class="top"><% =title%></td>--%>
                    <td class="top">Phone</td>
                    <td class="top righ"><% =Dt.Rows[i]["Phone"]%></td>
                </tr>
                <%}
                    }
                    else
                    {%>
                <tr>
                    <td class="top" colspan="5">No Data Found</td>
                </tr>
                <%
                   }
                   
                   
                %>
            </table>
            <h2>Medical and Insurance</h2>
            <table border="0" cellpadding="0" cellspacing="0" class="inr">

                 <%
                    Dt = getStudentDetailsNE("PP");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {                 
                %>



                <tr>
                    <td rowspan="2" class="top bgClr">Primary Physician</td>
                    <td class="top">Full Name </td>
                    <td class="top"><% =Dt.Rows[i]["Name"]%></td>
                    <td class="top">Office Phone</td>
                    <td class="top righ"><% =Dt.Rows[i]["OfficePhone"]%></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td><% =Dt.Rows[i]["Address"]%></td>
                    <td></td>
                    <td class="righ"></td>
                </tr>

                <%}
                    } %>



            </table>
            <br />

            <table border="0" cellpadding="0" cellspacing="0" class="inr">

                <%
                    Dt = getStudentDetailsNE("IN");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {                 
                %>

                <tr>
                    <td rowspan="2" class="top bgClr">Insurance</td>
                    <td class="top">Insurance Type</td>
                    <td class="top"><% =Dt.Rows[i]["InsuranceType"]%></td>
                    <td class="top">Policy Number</td>
                    <td class="top righ"><% =Dt.Rows[i]["PolicyNumber"]%></td>
                </tr>
                <tr>
                    <td>Policy Holder</td>
                    <td><% =Dt.Rows[i]["PolicyHolder"]%></td>
                    <td></td>
                    <td class="righ"></td>
                </tr>

                <%}
                     }
                     else
                     {%>

                <tr>
                    <td width="202px" class="top bgClr">Insurance</td>
                    <td class="top" colspan="4">No Data Found</td>
                </tr>
                <%
                   }
                   
                   
                %>
            </table>
            <br />

            <table border="0" cellpadding="0" cellspacing="0" class="inr">



                <%
                    Dt = getStudentDetailsNE("MT");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            string DateOfLastPhysicalExam = "";
                            try
                            {
                                DateOfLastPhysicalExam = Convert.ToDateTime(Dt.Rows[0]["DateOfLastPhysicalExam"]).ToString("MM/dd/yyyy");
                            }
                            catch (Exception Ex)
                            {
                            }
                %>

                <tr>
                    <td class="top" width="250px">Date of Last Physical Exam</td>
                    <td class="top righ"><%=DateOfLastPhysicalExam.Replace('-', '/') %></td>
                </tr>
                <tr>
                    <td>Medical Conditions/Diagnosis</td>
                    <td class=" righ"><% =Dt.Rows[i]["MedicalConditionsDiagnosis"]%></td>
                </tr>
                <tr>
                    <td>Allergies</td>
                    <td class="righ"><% =Dt.Rows[i]["Allergies"]%>.</td>
                </tr>
                <tr>
                    <td>Current Medications</td>
                    <td class="righ"><% =Dt.Rows[i]["CurrentMedications"]%>.</td>
                </tr>
                <tr>
                    <td>Self Preservation Ability</td>
                    <td class="righ"><% =Dt.Rows[i]["SelfPreservationAbility"]%>.</td>
                </tr>
                <tr>
                    <td>Significant Behavior Characteristics</td>
                    <td class="righ"><% =Dt.Rows[i]["SignificantBehaviorCharacteristics"]%></td>
                </tr>
                <tr>
                    <td rowspan="3">Relevant Capabilities, Limitations, and Preferences</td>
                    <td class="righ">
                        <h3>Capabilities</h3>
                        <% =Dt.Rows[i]["Capabilities"]%>.
                    </td>
                </tr>
                <tr>
                    <td class="righ">
                        <h3>Limitations</h3>
                        <% =Dt.Rows[i]["Limitations"]%>.
                    </td>
                </tr>
                <tr>
                    <td class="righ">
                        <h3>Preferences</h3>
                        <% =Dt.Rows[i]["Preferances"]%>.
                    </td>
                </tr>

                <%}
                     } %>
            </table>

            <h2>Referral/IEP Information</h2>

            <table border="0" cellpadding="0" cellspacing="0" class="inr">
                <%
                    Dt = getStudentDetailsNE("IEP");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {

                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            string NameTitle = Dt.Rows[i]["Name"].ToString();
                            string fullName = "";
                            string title = "";
                            if (NameTitle != "")
                            {
                                string[] splitVal = NameTitle.Split(',');
                                fullName = splitVal[0];
                                title = splitVal[1];
                            }
                              %>
                <tr>
                    <td class="top bgClr">Liaison</td>
                    <td class="top">Full Name, Title</td>
                    <%if (fullName.ToString().Trim() == null || fullName.ToString().Trim() == "" || title.ToString().Trim() == null || title.ToString().Trim() == "")
                      {
                          %>
                    <td class="top"> <% =fullName%><% =title%></td>
                          
                    <%
                    }else
                      {
                          %>
                    <td class="top"> <% =fullName%><b>, </b><% =title%></td>
                    <%
                    }
                         %>
                    
                    <td class="top">Phone</td>
                    <td class="top righ"><% =Dt.Rows[i]["IEPReferralPhone"]%></td>
                </tr>
                <tr>
                    <td class="top">Referring Agency</td>
                    <td class="top righ"> <% =Dt.Rows[i]["RAgency"]%></td>
                </tr>
                <tr>
                    <td>Source of Tuition</td>
                    <td class="righ"> <%=Dt.Rows[i]["RTuition"]%></td>
                </tr>
                   <%}
                     }
                      %>
                
            </table>
            <br />

            <table border="0" cellpadding="0" cellspacing="0" class="inr">
                
            </table>
            <h2>Education History</h2>

            <table border="0" cellpadding="0" cellspacing="0" class="inr">

                <%
                    Dt = getStudentDetailsNE("DD");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {
                        try
                        {
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {                 
                %>
                <tr>
                    <td class="top" style="width: 35%">Date Initially Eligible for Special Education</td>
                    <td class="top righ"><%=Dt.Rows[0]["DateInitiallyEligibleforSpecialEducation"]%></td>
                </tr>
                <tr>
                    <td>Date of Most Recent Special Education Evaluations</td>
                    <td class="righ"><%=Dt.Rows[0]["DateofMostRecentSpecialEducationEvaluations"] %></td>
                </tr>
                <tr>
                    <td>Date of Next Scheduled 3-Year Evaluation</td>
                    <td class="righ"><%=Dt.Rows[0]["DateofNextScheduled3YearEvaluation"] %></td>
                </tr>
                <tr>
                    <td>Current IEP Expiration Date</td>
                    <td class="righ"><%=Dt.Rows[0]["CurrentIEPStartDate"] %></td>
                </tr>

                <%
                         }
                         }
                         catch (Exception Ex)
                         {

                         }
                     }
                     else
                     {%>
                <tr>
                    <td class="top" colspan="2">No Data Found</td>
                </tr>
                <%
                   }
                   
                   
                %>
            </table>







            <h2>Schools Attended</h2>

            <table border="0" cellpadding="0" cellspacing="0" class="inr">


                 <tr>
                    <td class="top bgClr">Name</td>
                    <td class="top bgClr">Address</td>
                    <td class="top bgClr righ">Dates Attended</td>
                </tr>
                  <%
                    Dt = getStudentDetailsNE("SA");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {                      

                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {                 
                %>


                <tr>
                    <td><%=Dt.Rows[i]["SchoolName"] %></td>
                    <td><%=Dt.Rows[i]["Address"] %></td>
                    <td class="righ"><%=Dt.Rows[i]["DateAttended"] %></td>
                </tr>
                 <%
                   }
                   }else
                   {
                %>

                  <tr>
                    <td colspan="3">No Data Found</td>
                      </tr>

                 <%} %>
            </table>
            <h2>Discharge Information</h2>
           
            
             <table border="0" cellpadding="0" cellspacing="0" class="inr">

                  <%
                    Dt = getStudentDetailsNE("DI");
                    if (Dt.Rows.Count > 0 && Dt != null)
                    {                      

                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {                 
                %>


                <tr>
                    <td class="top" style="width:379px;">Discharge Date</td>
                    <td class="top righ"><%=Dt.Rows[0]["DischargeDate"] %></td>
                </tr>
                <tr>
                    <td>Location After Discharge</td>
                    <td class="righ"><%=Dt.Rows[0]["LocationAfterDischarge"] %></td>
                </tr>
                <tr>
                    <td>Melmark New England’s Follow Up Responsibility</td>
                    <td class="righ"><%=Dt.Rows[0]["MelmarkNewEnglandsFollowUpResponsibilities"] %></td>
                </tr>

                 <%
                   }
                   }else
                   {
                %>

                  <tr>
                    <td colspan="3">No Data Found</td>
                      </tr>

                 <%} %>


            </table>



            <div class="clear"></div>
        </div>

        <div class="clear"></div>

    </form>
</body>
</html>
