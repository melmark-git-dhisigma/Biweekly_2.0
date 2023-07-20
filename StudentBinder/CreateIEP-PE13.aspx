<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE13.aspx.cs" Inherits="StudentBinder_CreateIEP_PE13" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.8.0.min.js"></script>
    <link href="CSS/StylePE.css" rel="stylesheet" />
        <link href="CSS/StylePE15.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
         <div id="divIEPP13">
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

                 <tr>
                     <td class="righ"><h2 class="simble">VII. EDUCATIONAL PLACEMENT</h2></td>
                 </tr>
                 <tr>
                     <td class="righ"></td>
                 </tr>
                 <tr>
                       <td class="righ" style="line-height: 20px;">A. QUESTIONS FOR IEP TEAM – The following questions must be reviewed and discussed by the IEP team prior to providing the explanations regarding
                         participation with Individuals without disabilities.</td>
                 </tr>
                  <tr>
                      <td class="righ"></td>
                 </tr>
                 <tr>
                      <td class="righ">
                         <table cellpadding="0" cellspacing="0" style="border:0;">
                             <tr>
                                  <td class="righ top" colspan="3" style="line-height: 20px;">
                                      It is the responsibility of each public agency to ensure that, to the maximum extent appropriate, Individuals with disabilities, including those in public
                                             or private institutions or other care facilities, are educated with Individuals who are not disabled. Special classes, separate schooling or other removal
                                             of Individuals with disabilities from the general educational environment occurs only when the nature or severity of the disability is such that education
                                             in general education classes, EVEN WITH the use of supplementary aids and services, cannot be achieved satisfactorily.
                                 </td>
                                 
                             </tr>
                            
                             <tr>
                                  <td class="righ" colspan="3">
                                     <ul style="list-style:disc outside;">
                                          <li style="line-height: 20px;">
                                             What supplementary aids and services were considered? What supplementary aids and services were rejected? Explain why the supplementary
                                     aids and services will or will not enable the Individual to make progress on the goals and objectives (if applicable) in this IEP in the general
                                     education class.
                                         </li>
                                         <li style="line-height: 20px;">
                                             What benefits are provided in the general education class with supplementary aids and services versus the benefits provided in the special
                                     education class?
                                         </li>
                                         <li style="line-height: 20px;">
                                             What potentially beneficial effects and/or harmful effects might be expected on the Individual with disabilities or the other Individuals in the
                                     class, even with supplementary aids and services?
                                         </li>
                                         <li style="line-height: 20px;">
                                             To what extent, if any, will the Individual participate with nondisabled peers in extracurricular activities or other nonacademic activities?
                                         </li>
                                         </ul>
                                     </td>
                                  
                             </tr>
                             
                         </table>
                     </td>
                 </tr>
                 <tr>
                      <td class="righ"></td>
                 </tr>
                 <tr>
                      <td class="righ" style="line-height: 20px;">
                         Explanation of the extent, if any, to which the Individual will not participate with Individuals without disabilities in the regular education class:
                     </td>
                 </tr>
                 <tr>
                      <td class="righ">
                         <asp:TextBox ID="TextBoxExplanationregularClass" runat="server" CssClass="FreeTextDivContent" TextMode="MultiLine" Width="100%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                      <td class="righ"></td>
                 </tr>
                 <tr>
                      <td class="righ" style="line-height: 20px;">
                         Explanation of the extent, if any, to which the Individual will not participate with Individuals without disabilities in the general education curriculum:
                     </td>
                 </tr>
                 <tr>
                      <td class="righ">
                         <asp:TextBox ID="TextBoxExplanationGeneralCurriculam" CssClass="FreeTextDivContent" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                      <td class="righ" colspan="2">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue" OnClick="btnSave_Click"/>

                          <%--<asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSave_Click" style="display:none;"/>--%>
                     </td>
                  </tr>
                 </table>
                  <div class="clear"></div>
                 </div>
            </div>
    </form>
</body>
</html>
