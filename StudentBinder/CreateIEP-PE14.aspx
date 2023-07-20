<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE14.aspx.cs" Inherits="StudentBinder_CreateIEP_PE14" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/StylePE.css" rel="stylesheet" />
     <link href="CSS/StylePE15.css" rel="stylesheet" />
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
                    <td class="righ" >INDIVIDUALISED EDUCATION PROGRAM(IEP)
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0"> 
                <tr>
                    <td colspan="2" class="righ">Individual's Name : <asp:Label runat="server" id="lblStudentName"></asp:Label></td>
                </tr>
                <%--<tr>
                    <td>Student's Name:<label runat="server" id="lblStudentName"></label></td>
                    <td>&nbsp;</td>

                </tr>--%>
            <tr>
                 <td class="righ" colspan="2"><h2 class="simble">Educational Placement- II</h2>
                    </td>
					</tr>
                
                <tr>
                    <td colspan="2" class="auto-style1 righ">
                        B. Type of Support&nbsp;</td>
                
                </tr>
            
                <tr>
                    <td class="righ" colspan="2">
                  
                      1. Amount of special education supports
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkItinerant" runat="server" /></td>
                     <td class="righ">
                         Itinerant: Special education supports and services provided by special education personnel for 20% or less of the school day 
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkSupplemental" runat="server" /></td>
                    <td class="righ">
                         Supplemental: Special education supports and services provided by special education personnel for more than 20% of the day but less than 80% of the school day
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkFullTime" runat="server" /></td>
                    <td class="righ">
                         Full-Time: Special education supports and services provided by special education personnel for 80% or more of the school day
                    </td>
                   
                </tr>
    
                <tr>
                     <td class="righ" colspan="2">2. Type of special education supports</td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkAutistic" runat="server" /></td>
                     <td class="righ">
                         Autistic Support
                    </td>
                  
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkBlind" runat="server" /></td>
                    <td class="righ">
                         Blind-Visually Impaired Support
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkDeaf" runat="server" /></td>
                     <td class="righ">
                        Deaf and Hard of Hearing Support
                    </td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkEmotional" runat="server" /></td>
                    <td class="righ">
                         Emotional Support
                    </td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkLearning" runat="server" /></td>
                     <td class="righ">
                         Learning Support
                    </td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkLifeskills" runat="server" /></td>
                    <td class="righ">
                         Life Skills Support
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkMultipleDis" runat="server" /></td>
                     <td class="righ">
                        Multiple Disabilities Support
                    </td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkPhysical" runat="server" /></td>
                     <td class="righ">
                        Physical Support
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkSpeech" runat="server" /></td>
                     <td class="righ" >
                         Speech and Language Support
                    </td>
                    
                </tr>
              
                <tr>
                     <td class="righ" colspan="2">C. Location of Individual’s program</td>
                  
                </tr>
                
                <tr>
                     <td class="righ" colspan="2">Name of School District where the IEP will be implemented&nbsp; :
                        <asp:TextBox ID="TxtSchoolDisr" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                    </td>
                   
                </tr>
                <tr>
                     <td class="righ" colspan="2">Name of School Building where the IEP will be implemented :
                        <asp:TextBox ID="TextBuilding" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                    </td>
                  
                </tr>
               
                <tr>
                    <td class="righ" colspan="2"> Is this school the Individual’s neighborhood school (i.e., the school the Individual would attend if he/she did not have an IEP)?</td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkYes" runat="server" /></td>
                    <td class="righ">
                         Yes                     
                    </td>
                   
                </tr>
                <tr>
                    <td><asp:CheckBox ID="ChkNo"  runat="server" /></td>
                    <td class="righ">
                         No. If the answer is “no,” select the reason why not.
                    </td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="ChKSpecialEducation" runat="server" /></td>
                    <td class="righ" >
                         Special education supports and services required in the Individual’s IEP cannot be provided in the neighborhood school
                    </td>
                    
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkOther" runat="server" /></td>
                    <td class="righ">
                         Other. Please explain:
                        <asp:TextBox ID="TextBoxOther" runat="server" TextMode="MultiLine" rows="1"></asp:TextBox></>
                    </td>
                </tr>
                

                <tr>
                     <td class="righ" colspan="3">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue" />

                         <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy"  style="display:none;"/>--%>
                    </td>
                     
                </tr>
                </table>
               <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
