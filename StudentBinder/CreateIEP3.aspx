<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP3.aspx.cs" Inherits="StudentBinder_CreateIEP3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 25%;
        }

        .style4 {
            width: 31%;
        }

        .style5 {
            width: 20%;
        }
        .FreeTextDivContent {
            width: 98%;
            min-height: 200px;
            height: 200px;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
            overflow:auto;
        }
    </style>
     <script type="text/javascript">

         function submitClick() {
             var txtAccomodation3 = document.getElementById('<%=txtAccomodation3.ClientID %>').innerHTML;
             var txtDisabilities3 = document.getElementById('<%=txtDisabilities3.ClientID %>').innerHTML;
             PageMethods.submitIEP3(txtAccomodation3, txtDisabilities3);
         }
         function GetFreetextval(content, divid) {
             if (divid == 'txtAccomodation3') {
                 document.getElementById('<%=txtAccomodation3.ClientID %>').innerHTML = "";
                 document.getElementById('<%=txtAccomodation3.ClientID %>').innerHTML = content;
                 document.getElementById('<%=txtAccomodation3_hdn.ClientID %>').value = window.escape(content);
           }
             else if (divid == 'txtDisabilities3') {
               document.getElementById('<%=txtDisabilities3.ClientID %>').innerHTML = "";
                 document.getElementById('<%=txtDisabilities3.ClientID %>').innerHTML = content;
                 document.getElementById('<%=txtDisabilities3_hdn.ClientID %>').value = window.escape(content);
           }
         }

         function chkLen() {
             var val = document.getElementById('txtDisabilities3').value;
             var val1 = document.getElementById('txtAccomodation3').value;

             if ((parseInt(val.length) < 200) && (parseInt(val1.length) < 200)) {
                 return true
             }
             else {
                 tdMsg.innerHTML = '<div class=error_box>Text in Editor should be less than 200 charecters.</div>'
                 return false;
             }
         }

         function scrollToTop() {
             window.scrollTo(0, 0);
             window.parent.parent.scrollTo(0, 100);
         }
   </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ToolkitScriptManager>
        <div id="divIep2" style="width: 97%; border-radius: 3px 3px 3px 3px; padding: 7px;">
            <table width="97%">

                <tr>
                    <td colspan="3" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Present Levels of Educational Performance
                    </td>
                </tr>
                <tr>
                    <td id="tdMsg" runat="server" colspan="4"></td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <h3>B: Other Educational Needs</h3>
                    </td>

                </tr>

                <tr>
                    <td class="tdText" style="width: 15%; font-weight: bold; padding-left: 7px;">Check all that apply
                    </td>
                    <td class="tdText" style="width: 15%; font-weight: bold; padding-left: 7px;">General Considerations
                    </td>
                    <td class="tdText" style="width: 15%; font-weight: bold;"></td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <table class="style1">
                            <tr>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkAdaptedPhyEdu" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle"
                                        Text="Adapted physical education" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkBrailleNeeds0" runat="server" Text="Braille needs (blind/visually impaired)"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkExtraCurrAct" runat="server" Text="Extra curriculum activities"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkSocialNeeds" runat="server"
                                        Text="Social/emotional needs"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkOther" runat="server" Text="Other:"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkChild3To5" runat="server" Text="For children ages 3 to 5"
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp; 
                                    <asp:CheckBox runat="server" Text="For children ages 14+ (or younger if appropriate)"
                                        ID="chkChild14" Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkChild16" runat="server" Text="For children ages 16 (or younger if appropriate) to 22 "
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                            CssClass="checkBoxStyle" />
                                </td>
                            </tr>

                        </table>

                    </td>
                    <td style="vertical-align: top;">
                        <table class="style1" style="text-align: left">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkAssTechDevices" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle"
                                        Text="Assistive tech devices/services" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkCommunicationAllStudent0" runat="server" Text="Communication (all students)"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkLangNeeds" runat="server" Text="Language needs (LEP students)"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkTravelTraining" runat="server" Text="Travel training"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtOther3" runat="server" CssClass="textClass"
                                        Width="90%" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>

                        </table>

                    </td>
                    <td style="vertical-align: top;">

                        <table class="style1" style="text-align: left">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBehavior" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle"
                                        Text="Behavior" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkCommunicationDeaf" runat="server" Text="Communication (deaf/hard of hearing students)"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkNonAcademicActivities" runat="server" Text="Nonacademic activities"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkVocationalEdu" runat="server"
                                        Text="Vocational education" Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        CssClass="checkBoxStyle" />
                                </td>
                            </tr>

                        </table>

                    </td>
                </tr>

                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>

                <tr style ="height:37px;">
                    <td align="center" colspan="3" class="tdText">How does the disability(ies) affect progress in the indicated area(s) of other 
                        educational needs?
                    </td>
                    </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtDisabilities3_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtDisabilities3" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP3.aspx',this); "></div>

    
                        

                    </td>
                </tr>

                <tr style ="height:37px;">
                    <td align="center" colspan="3" class="tdText">What type(s) of accommodation, if any, is necessary for the student to make 
                        effective progress?
                    </td>
                    </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtAccomodation3_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                         <div id="txtAccomodation3" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP3.aspx',this); "></div>

    
                       

                    </td>
                </tr>

                <tr style ="height:51px;">
                    <td style="text-align: center" colspan="3" class="tdText">What type(s) of specially designed instruction, if any, is necessary for the 
                        student to make effective progress?<br />
                        Check the neccessary instructional modification(s) and describe how such 
                        modification(s) will be made.</td>
                </tr>

                <tr>
                    <td class="tdText" colspan="3">
                        <table style="width: 100%;">
                            <tr>
                                <td class="tdText" style="width: 25%">&nbsp;</td>
                                <td class="tdText">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" class="tdText" style="width: 10%">
                                    <asp:CheckBox ID="chkContent3" runat="server" Text="Content:"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle" />
                                </td>
                                <td align="left" class="tdText">
                                    <asp:TextBox ID="txtContent3" runat="server" Width="100%" CssClass="textClass" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tdText" style="width: 10%">
                                    <asp:CheckBox runat="server" Text="Methodology/Delivery of Instruction:" ID="chkMethodology3"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle"></asp:CheckBox>
                                </td>
                                <td align="left" class="tdText">
                                    <asp:TextBox runat="server" CssClass="textClass" Width="100%" ID="txtMethodology3" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tdText" style="width: 10%">
                                    <asp:CheckBox ID="chkPerformance3" runat="server" Text="Performance Criteria:"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle" />
                                </td>
                                <td align="left" class="tdText">
                                    <asp:TextBox ID="txtPerformance3" runat="server" CssClass="textClass" Width="100%" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" 
                                        OnClick="btnSave_Click" Text="Save and continue" />

                                      <%--<asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" 
                                        OnClick="btnSave_hdn_Click" Text="dummy" OnClientClick="submitClick();" style="display:none;"/>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>


    </form>
</body>
</html>
