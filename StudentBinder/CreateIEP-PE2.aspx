<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE2.aspx.cs" Inherits="StudentBinder_CreateIEP_PE2" %>

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
        <div id="divIEPP2">

            <div class="ContentAreaContainer">
            <div class="clear"></div>
                <br />
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
                     <td class=" righ">
                        <h2 class="simble">IEP TEAM/SIGNATURES</h2></td>
                 </tr>
                 <tr>
                     <td style="line-height: 15px;" class="righ">The Individualized Education Program team makes the decisions about the’s program and placement. The Individual’s parent(s), the Individual’s special
                         education teacher, and a representative from the Local Education Agency are required members of this team. Signature on this IEP documents attendance, not
                         agreement.</td>
                 </tr>
           
                 <tr>
                     <td class="righ">
                         <table cellpadding="0" cellspacing="0" width="100%" class="innerpag">
                             <tr>
                                 <td class=" top">Role</td>
                                 <td class=" top">Printed Name</td>
                                 <td class=" top righ">Signature</td>
                             </tr>
                             <tr>
                                 <td>Parent/Guardian/Surrogate</td>
                                 <td>
                                     <asp:TextBox ID="TxtParent" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                                <td class="righ">
                                     <asp:TextBox ID="TextBoxParentSign" CssClass="textfield" runat="server" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Individual*</td>
                                 <td>
                                     <asp:TextBox ID="TxtStudent" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxStudentSign" CssClass="textfield" runat="server" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Regular Education Teacher**</td>
                                 <td>
                                     <asp:TextBox ID="TxtRegularTeacher" CssClass="textfield" runat="server" Text="" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxRegularTeacherSign" CssClass="textfield" runat="server" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Special Education Teacher</td>
                                 <td>
                                     <asp:TextBox ID="TxtSpecialTeacher" CssClass="textfield" runat="server" Text="" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxSpecialTeacherSign" CssClass="textfield" runat="server" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Local Ed Agency Rep</td>
                                 <td>
                                     <asp:TextBox ID="TxtLocalEd" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxLocalEdSign" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Career/Tech Ed Rep***</td>
                                 <td>
                                     <asp:TextBox ID="TxtCareer" runat="server" Text="" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxCareerSign" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Community Agency Rep</td>
                                 <td>
                                     <asp:TextBox ID="TxtCommunity" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxCommunitySign" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td>Teacher of the Gifted****</td>
                                 <td>
                                     <asp:TextBox ID="TxtTeacherGifted" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                                  <td class="righ">
                                     <asp:TextBox ID="TextBoxTeacherGiftedSign" runat="server" CssClass="textfield" Text="" MaxLength="100"></asp:TextBox></td>
                             </tr>
                         </table>
                     </td>
                 </tr>
            
                 <tr>
                     <td class="righ">* The IEP team must invite the Individual if transition services are being planned or if the parents choose to have the Individual participate.</td>
                 </tr>
                 <tr>
                     <td class="righ">** If the Individual is, or may be, participating in the regular education environment</td>
                 </tr>
                 <tr>
                     <td class="righ">*** As determined by the LEA as needed for transition services and other community services</td>
                 </tr>
                 <tr>
                     <td class="righ">**** A teacher of the gifted is required when writing an IEP for a Individual with a disability who also is gifted.</td>
                 </tr>
                 <tr>
                     <td class="righ">One individual listed above must be able to interpret the instructional implications of any evaluation results.</td>
                 </tr>
         
                 <tr>
                     <td class="righ">Written input received from the following members:</td>
                 </tr>
                 <tr>
                     <td class="righ"><asp:TextBox ID="TextBoxWritteninput" runat="server" TextMode="MultiLine" CssClass="FreeTextDivContent" ></asp:TextBox></td>
                 </tr>
                 <tr>
                     <td class="righ">Transfer of Rights at Age of Majority</td>
                 </tr>
                 <tr>
                     <td style="line-height: 15px;" class="righ">For purposes of education, the age of majority is reached in Pennsylvania when the individual reaches 21 years of age. Likewise, for purposes of the Individuals<br />
                     with Disabilities Education Act, the age of majority is reached for Individuals with disabilities when they reach 21 years of age.</td>
                 </tr>
                  <tr>
                     <td colspan="2" class="righ">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue" OnClick="btnSave_Click"/>

                        <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSave_hdn_Click" style="display:none;"/>--%>
                     </td>
                  </tr>
                 </table>
                <div class="clear"></div>
                </div>
             </div>
    </form>
</body>
</html>
