<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE3.aspx.cs" Inherits="StudentBinder_CreateIEP_PE3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/StylePE.css" rel="stylesheet" />
    <link href="CSS/StylePE15.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
         <div id="divIEPP3">
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
                     <td class="righ">
<h2 class="simble">PROCEDURAL SAFEGUARDS NOTICE</h2></td>
                 </tr>
                 <tr>
                     <td class="righ" style="line-height: 15px;">I have received a copy of the Procedural Safeguards Notice during this school year. The Procedural Safeguards Notice provides information about my rights,<br />
                     including the process for disagreeing with the IEP. The school has informed me whom I may contact if I need more information.</td>
                 </tr>

                 <tr>
                     <td class="righ"colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="top">
                         Signature of Parent/Guardian/Surrogate:
                                    </td>
<td class="top righ">
                      <asp:TextBox ID="TextBoxSign" CssClass="textfield" runat="server" MaxLength="100"></asp:TextBox></td>
                     </tr>
                 </tr>

                  <tr>
                     <td class="righ"  colspan="2">
                         
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue" OnClick="btnSave_Click"/>

                       <%--   <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSave_hdn_Click" style="display:none;"/>--%>
                     </td>
                  </tr>
                 </table>
                 <div class="clear"></div>
                 </div>
            </div>
    </form>
</body>
</html>
