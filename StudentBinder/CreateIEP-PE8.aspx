<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE8.aspx.cs" Inherits="StudentBinder_CreateIEP_PE8" %>

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
                    <td class="righ" >INDIVIDUALISED EDUCATION PROGRAM(IEP)
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0"> 
                <tr>
                    <td colspan="2" class="righ">Individual's Name : <asp:Label runat="server" id="lblStudentName"></asp:Label></td>
                </tr>
				<tr>
                  <td class="righ"><h2 class="simble">Additional Information</h2>
                    </td>
					</tr>
                <tr>
                    <td class="righ"><strong>Science </strong>(PSSA grades 4, 8, 11; PSSA-M grades 8, 11)</td>
                    
                </tr>
                <tr>
                    <td colspan="2" class="righ">
                        <table border="0" cellpadding="0" cellspacing="0" >
                            <tr>

                                <td><asp:CheckBox ID="IEP8SciencePartcptPSSAWithoutAcmdtn" runat="server"/></td>
                                <td class="top righ">Individual will participate in the PSSA without accommodations</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8SciencePartcptPSSAWithFollowingAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA with the following appropriate accommodations:</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8SciencePartcptPSSAModiWithoutAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA-Modified without accommodations</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8SciencePartcptPSSAModiWithFollowingAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA-Modified with the following appropriate accommodations:</td>

                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr>
                    <td class="auto-style1 righ " colspan="2"><strong>Writing</strong> (PSSA grades 5, 8, 11)<br />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1 righ" colspan="2">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:CheckBox ID="IEP8WritePartcptPSSAWithoutAcmdtn" runat="server"/></td>
                                <td class="righ top">Individual will participate in the PSSA without accommodations</td>

                            </tr>
                            <tr>

                                <td><asp:CheckBox ID="IEP8WritePartcptPSSAWithFollowingAcmdtn" runat="server"/></td>
                                <td class="righ">Individual will participate in the PSSA with the following appropriate accommodations:</td>

                            </tr>                            
                        </table>
                    </td>
                </tr>
               
                <tr>
                    <td class="righ">
                        <strong>PASA</strong> (PASA grades 3-8, 11 for Reading and Math; Grades 4, 8, 11 for Science)</td>
                </tr>
                <tr>
                    <td colspan="2" class="righ">
                          <table border="0" cellpadding="0" cellspacing="0">
                            <tr>

                                <td><asp:CheckBox ID="IEP8PSSAParticipate" runat="server"/>
                               Individual will participate in the PASA</td>                                

                            </tr>                          
                        </table>
                    </td>
                </tr>
               
                <tr>
                    <td colspan="2" class="righ">
                         <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                           
                                <td class="top righ">

                                    Explain why the Individual cannot participate in the PSSA or the PSSA-M for Reading, Math, or Science:</td>
                            </tr>
                            <tr>
                                
                                <td class="righ">                                    
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="FreeTextDivContent" TextMode="MultiLine"></asp:TextBox>                               

                                </td>
                            </tr>
                             <tr>
                               
                                <td class="righ">

                                    Explain why the PASA is appropriate:</td>
                            </tr>
                            <tr>
                                
                                <td class="righ">
                                    <asp:TextBox ID="TextBox2" CssClass="FreeTextDivContent" runat="server" TextMode="MultiLine" ></asp:TextBox>
                                </td>
                            </tr>
                           
                            <tr>
                                
                                <td class="righ">

                                    Choose how the Individual’s performance on the PASA will be documented.</td>
                            </tr>
                            <tr>
                                <td><asp:CheckBox ID="chkVideoTape" runat="server"/>
                              

                                   Videotape ( will be kept confidential as all other school records)
                                </td>
                                
                            </tr>
                            <tr>
                                <td><asp:CheckBox ID="chkwrittenNative" runat="server" />
                               
                                     Written narrative (will be kept confidential as all other school records)
                                </td>
                            </tr>
                            
                           
                        </table>
                    </td>
                </tr>
               
                
                <tr>
                    <td colspan="2" class="righ">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue"/>

                      <%--  <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
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
