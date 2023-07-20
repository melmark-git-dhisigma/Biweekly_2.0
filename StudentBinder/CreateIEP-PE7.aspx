<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE7.aspx.cs" Inherits="StudentBinder_CreateIEP_PE7" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/StylePE.css" rel="stylesheet" />
     <link href="CSS/StylePE15.css" rel="stylesheet" />
   <style>
        div.ContentAreaContainer table td {
            width:auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   
        <div id="divIEPP1">
              <div class="ContentAreaContainer">
                 <br />
                 <div class="clear"></div>
            <table cellpadding="0" cellspacing="0" width="96%">
                 <tr>
                    <td id="tdMsg" runat="server" class="top righ"></td>
                </tr>

                <tr>
                    <td class="righ" >Individualized Education Program(IEP)
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0">
                <tr>
                   <td style="width:50%;width:300px"><label style="float:left;">Individual's Name &nbsp:&nbsp </label> <label style="float:left" runat="server" id="lblStudentName"></label></td>
                </tr>
  
                <tr>
                   <td class="righ" colspan="2">
<h2 class="simble">IV. PARTICIPATION IN STATE AND LOCAL ASSESSMENTS</h2></td>
                   
                </tr>
                <tr>
                    <td colspan="2" class="righ">Instructions for IEP Teams:</td>
                  
                </tr>
                <tr>
                    <td class="righ" colspan="2" style="line-height: 20px;">
                        Please check the appropriate assessments. If the Individual will be assessed using the PSSA or the PSSA-Modified, the IEP Team must choose which assessment will be administered for each content area (Reading, Mathematics, and Science). For example, a Individual may take the PSSA-Modified for Reading and the PSSA for Mathematics and Science. If the Individual will be assessed using the PASA, the IEP Team need not select content areas because ALL content areas will be assessed using the PASA.
                       <ul style="list-style:disc outside;">
                           <li style="line-height: 20px;">
                           PSSA (Please choose the appropriate option and content areas for the Individual. A Individual may be eligible to be assessed using the PSSA-Modified assessment for one or more content areas and be assessed using the PSSA for other content areas.)<br />
                          
                           </li>
                           <li style="line-height: 20px;">
                           PSSA-Modified (Please choose the appropriate option and content areas for the Individual. A Individual may be eligible to be assessed using the
                           PSSA-Modified assessment for one or more content areas and be assessed using the PSSA for other content areas.)
                          </li>
                       </ul>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2" class="righ" style="line-height: 20px;">Allowable accommodations may be found in the PSSA Accommodations Guidelines at:<br />
<a href="http://www.portal.state.pa.us/portal/server.pt/community/testing_accommodations__security/7448">www.portal.state.pa.us/portal/server.pt/community/testing_accommodations__security/7448</a><br />
                        Criteria regarding PSSA-Modified eligibility may be found in Guidelines for IEP Teams: Assigning Individuals with IEPs to State Tests (ASIST) at:<br /> <a href="http://www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491">www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491</a><br />
Criteria regarding PASA eligibility may be found in Guidelines for IEP Teams: Assigning Individuals with IEPs to State Tests (ASIST) at:<br /> <a href="http://www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491">www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491</a>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1 righ" colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" class="righ">Not Assessed (Please select if Individual is not being assessed by a state assessment this year)</td>
                    
                </tr>
                <tr>
                    <td colspan="2" class="righ">
                          <table border="0" cellpadding="0" cellspacing="0">
                            <tr>

                                <td style="width:35px"><asp:CheckBox ID="IEP8AsmtNotAdministred" runat="server"/></td>
                                <td class="righ">Assessment is not administered at this Individual’s grade level</td>                                

                            </tr>                          
                        </table>
                    </td>
                </tr>
       
                <tr>
                    <td colspan="2" class="righ">Reading (PSSA grades 3-8, 11; PSSA-M grades 4-8, 11)</td>
                   
                </tr>
                <tr>
                    <td colspan="2" class="righ">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>

                                <td ><asp:CheckBox ID="IEP8ReadPartcptPSSAWithoutAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA without accommodations</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8ReadPartcptPSSAWithFollowingAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA with the following appropriate accommodations:</td>

                            </tr>
                            <tr>
                                <td><asp:CheckBox ID="IEP8ReadPartcptPSSAModiWithoutAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA-Modified without accommodations</td>

                            </tr>
                            <tr>
                                <td><asp:CheckBox ID="IEP8ReadPartcptPSSAModiWithFollowingAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA-Modified with the following appropriate accommodations:</td>
                           </tr>
                          </table>
                    </td>
                </tr>
            
                <tr>
                    <td class="righ" style="width:100%">Math (PSSA grades 3-8, 11; PSSA-M grades 4-8, 11)</td>
                  
                </tr>
                <tr>
                    <td colspan="2" class="righ">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>

                                <td><asp:CheckBox ID="IEP8MathPartcptPSSAWithoutAcmdtn" runat="server" /></td>
                                <td class="top righ">Individual will participate in the PSSA without accommodations</td>

                            </tr>
                            <tr>
                               <td><asp:CheckBox ID="IEP8MathPartcptPSSAWithFollowingAcmdtn" runat="server" /></td>
                                <td class="righ">Individual will participate in the PSSA with the following appropriate accommodations:</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8MathPartcptPSSAModiWithoutAcmdtn" runat="server" /></td>
                                <td class="righ">Individual will participate in the PSSA-Modified without accommodations</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8MathPartcptPSSAModiWithFollowingAcmdtn" runat="server" /></td>
                                <td class="righ">Individual will participate in the PSSA-Modified with the following appropriate accommodations:</td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style2 righ">&nbsp;</td>
                </tr>
              
            
                <tr>
                    <td colspan="2" class="righ">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue"/>

                         <%--<asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy" style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
                  <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
