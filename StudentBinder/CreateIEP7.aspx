<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP7.aspx.cs" Inherits="StudentBinder_CreateIEP7" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        .style1 {
            width: 903px;
        }

        .style2 {
            width: 20%;
        }

        .auto-style2 {
            height: 35px;
            width: 181px;
        }

        .auto-style3 {
            font-family: Arial;
            color: #000;
            line-height: 22px;
            font-weight: normal;
            font-size: 12px;
            padding-right: 1px;
            text-align: left;
            height: 35px;
            width: 181px;
        }

        .auto-style4 {
            width: 181px;
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

        .auto-style5 {
            height: 37px;
        }

        .auto-style6 {
            height: 35px;
        }
    </style>
    <script type="text/javascript">

        function submitClick() {
            var txtInfoCol2 = document.getElementById('<%=txtInfoCol2.ClientID %>').innerHTML;
                var txtInfoCol3 = document.getElementById('<%=txtInfoCol3.ClientID %>').innerHTML;
                PageMethods.submitIEP7(txtInfoCol2, txtInfoCol3);
            }

            function GetFreetextval(content, divid) {
                if (divid == 'txtInfoCol2') {
                    document.getElementById('<%=txtInfoCol2.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtInfoCol2.ClientID %>').innerHTML = content;
                    document.getElementById('<%=txtInfoCol2_hdn.ClientID %>').value = window.escape(content);
                }
                else if (divid == 'txtInfoCol3') {
                    document.getElementById('<%=txtInfoCol3.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtInfoCol3.ClientID %>').innerHTML = content;
                    document.getElementById('<%=txtInfoCol3_hdn.ClientID %>').value = window.escape(content);
                }
        }

        function chkLen() {
            var val = document.getElementById('txtInfoCol2').value;
            var val1 = document.getElementById('txtInfoCol3').value;

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
        <br />


        <div id="divIep2" style="width: 95%; border-radius: 3px 3px 3px 3px; padding: 7px;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;" class="auto-style6">State or District-Wide Assessment
                    </td>
                </tr>
                <td runat="server" id="tdMsg" style="color: #FF0000" colspan="4"></td>

                <tr>
                    <td align="left">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4" class="auto-style5">Identify state or district-wide assessments planned during this IEP period:
                    </td>
                    <td class="auto-style5"></td>
                    <td class="auto-style5"></td>
                    <td class="auto-style5"></td>

                </tr>
                <tr>
                    <td align="left" colspan="4">

                        <asp:TextBox ID="txtStateOrDistrict" runat="server" Width="98%"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap" align="left" colspan="4" class="tdText">Fill out the table below. Consider any state or district-wide assessment to be administered
                        during the time span covered by this IEP. For each<br />
                        content area, identify the student’s assessment participation status by putting
                        an “X” in the corresponding box for column 1,2, or 3.
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" nowrap="nowrap" class="auto-style4"></td>
                    <td align="left" class="tdText" nowrap="nowrap" style="border: 1px solid gray; padding-left: 3px;">1. Assessment participation:
                        <br />
                        Student participates in<br />
                        on-demand testing under routine<br />
                        conditions in this content area.
                    </td>
                    <td align="left" class="tdText" nowrap="nowrap" style="border: 1px solid gray; padding-left: 3px;">
                        <br />
                        2. Assessment participation:<br />
                        Student participates in<br />
                        on-demand testing with<br />
                        accommodations in this
                        <br />
                        content area.<br />
                        (See<strong> #</strong> below)
                    </td>
                    <td align="left" class="tdText" nowrap="nowrap" style="border: 1px solid gray; padding-left: 3px;">3. Assessment participation:<br />
                        Student participates in alternate<br />
                        assessment in this content area.<br />
                        &nbsp;(See ## below)
                    </td>
                </tr>
                <tr class="HeaderStyle">
                    <td>CONTENT AREAS
                    </td>
                    <td>COLUMN 1
                    </td>
                    <td>COLUMN 2
                    </td>
                    <td>COLUMN 3
                    </td>
                </tr>
                <tr class="RowStyle" style="height: 27px;">
                    <td>English Language Arts
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnglishLangArtC1" runat="server" Text=" " />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnglishLangArtC2" runat="server" Text=" " />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnglishLangArtC3" runat="server" Text=" " />
                    </td>
                </tr>
                <tr class="AltRowStyle" style="height: 27px;">
                    <td>History and Social Sciences
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHistoryC1" runat="server" Text=" " />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHistoryC2" runat="server" Text=" " />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHistoryC3" runat="server" Text=" " />
                    </td>
                </tr>
                <tr class="RowStyle" style="height: 27px;">
                    <td>Mathematics
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text=" " ID="chkMathematicsC1"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text=" " ID="chkMathematicsC2"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text=" " ID="chkMathematicsC3"></asp:CheckBox>
                    </td>
                </tr>
                <tr class="AltRowStyle" style="height: 27px;">
                    <td>Science and Technology
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text=" " ID="chkScienceAndTechC1"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text=" " ID="chkScienceAndTechC2"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text=" " ID="chkScienceAndTechC3"></asp:CheckBox>
                    </td>
                </tr>
                <tr class="RowStyle" style="height: 27px;">
                    <td>Reading
                    </td>
                    <td>
                        <asp:CheckBox ID="chkReadingC1" runat="server" Text=" " />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkReadingC2" runat="server" Text=" " />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkReadingC3" runat="server" Text=" " />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;"></td>
                </tr>
                <tr>
                    <td align="center" colspan="4">&nbsp;</td>
                </tr>
                <tr style="height: 75px;">
                    <td align="left" colspan="4" class="tdText">
                        <strong>#</strong> For each content area identified by an X in the column 2 above:
                        note in the space below, the content area and describe the<br />
                        accommodations necessary for participation in the on-demand testing. Any accommodations
                        used for assessment purposes<br />
                        should be closely modeled on the accommodations that are provided to the student
                        as part of his/her instructional program
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="4">
                         <asp:TextBox ID="txtInfoCol2_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtInfoCol2" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP7.aspx',this); "></div>


                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4" nowrap="nowrap">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;"></td>

                </tr>
                <tr>
                    <td align="center" colspan="4" nowrap="nowrap">&nbsp;</td>
                </tr>
                <tr style="height: 93px;">
                    <td align="left" colspan="4" class="tdText" style="margin-bottom: 5px;">
                        <strong>##</strong> For each content area identified by an X in column 3 above:
                        note in the space below, the content area, why the on-demand<br />
                        assessment is not appropriate and how that content area will be alternately assessed.
                        Make sure to include the learning<br />
                        standards that will be addressed in each content area, the recommended assessment
                        method(s) and the recommended<br />
                        evaluation and reporting method(s) for the student’s performance on the alternate
                        assessment.
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3" rowspan="2"
                        style="padding-right: 3px; border-right-style: dotted; border-right-width: thin; border-right-color: #C0C0C0;">
                         <asp:TextBox ID="txtInfoCol3_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtInfoCol3" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP7.aspx',this); " style="width: 95%"></div>

                    </td>
                    <td align="center" bgcolor="Black" style="font-weight: 700; color: #FFFFFF; margin-right: 3px">NOTE
                    </td>
                </tr>
                <tr>
                    
                    <td align="ce"  >When state model(s) for alternate<br />
                        assessment are adopted, the
                        <br />
                        district may enter use of state
                        <br />
                        model(s) for how content area(s)<br />
                        &nbsp;will be assessed.
                    </td>

                </tr>
                <tr style="height: 20px;">
                    <td colspan="4" style="text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; margin-top: 10px;"></td>
                </tr>
                <tr>
                    <td colspan="4" style="margin-left: 40px; text-align: center">
                        <asp:Button ID="btnUpdate" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue"
                            OnClick="btnUpdate_Click" OnClientClick="" />

                      <%--  <asp:Button ID="btnUpdate_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy"
                            OnClick="btnUpdate_hdn_Click" OnClientClick="submitClick();" Style="display: none;" />--%>
                    </td>
                    <td align="right" style="margin-left: 40px">&nbsp;
                    </td>
                    <td align="left" style="margin-left: 40px">&nbsp;
                    </td>
                </tr>
            </table>



        </div>
    </form>
</body>
</html>
