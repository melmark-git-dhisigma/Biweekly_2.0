<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP6.aspx.cs" Inherits="StudentBinder_CreateIEP6" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {
            width: 100%;
            height: 230px;
        }

        .style2 {
            width: 20%;
        }

        .checkBoxStyle {
        }

        .style3 {
            color: #808080;
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
            var txtNonParticipation = document.getElementById('<%=txtNonParticipation.ClientID %>').innerHTML;
            var txtSceduleModification = document.getElementById('<%=txtSceduleModification.ClientID %>').innerHTML;
            PageMethods.submitIEP6(txtNonParticipation, txtSceduleModification);
        }

        function GetFreetextval(content, divid) {
            if (divid == 'txtNonParticipation') {
                document.getElementById('<%=txtNonParticipation.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtNonParticipation.ClientID %>').innerHTML = content;
                document.getElementById('<%=txtNonParticipation_hdn.ClientID %>').value = window.escape(content);
            }
            else if (divid == 'txtSceduleModification') {
                document.getElementById('<%=txtSceduleModification.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtSceduleModification.ClientID %>').innerHTML = content;
                document.getElementById('<%=txtSceduleModification_hdn.ClientID %>').value = window.escape(content);
            }
        }


        function setDisable() {

            document.getElementById('chkTransportServicesRegular').disabled = true;
            document.getElementById('chkTransportServicesSpecial').disabled = true;
            document.getElementById('txtTransportServicesRegular').disabled = true;
            document.getElementById('txtTransportServicesSpecial').disabled = true;
           
        }
        function setEnable() {

            document.getElementById('chkTransportServicesRegular').disabled = false;
            document.getElementById('chkTransportServicesSpecial').disabled = false;
            document.getElementById('txtTransportServicesRegular').disabled = false;
            document.getElementById('txtTransportServicesSpecial').disabled = false;

        }

    function OnClientClicking1() {
        var button2 = document.getElementById('rblTransportServicesNo');
        var button3 = document.getElementById('rblTransportServicesYes');
        if (button2.checked) {
            button3.checked = false;
        }
        else {
            button3.checked = true;
        }
        if (button3.checked) {
            button2.checked = false;
        }
        else {
            button2.checked = true;
        }
    }
    </script>
    <script type="text/javascript">
        function chkLen() {
            var val = document.getElementById('txtNonParticipation').value;
            var val1 = document.getElementById('txtSceduleModification').value;

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
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <br />

                <div id="divIep6" style="width: 95%; border-radius: 3px 3px 3px 3px; padding: 7px;">
                    <table width="98%" cellpadding="0" cellspacing="0">





                        <tr>
                            <td height="10px">


                                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
                                </asp:ToolkitScriptManager>


                            </td>
                        </tr>


                        <tr>
                            <td runat="server" id="tdMsg" style="color: #FF0000"></td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Nonparticipation Justification
                            </td>
                        </tr>
                        <tr>
                            <td>


                                <table class="style1">
                                    <tr>
                                        <td colspan="2">Is the student removed from the general education classroom at anytime?
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdText"
                                            style="border-right-style: dashed; border-right-width: thin; border-right-color: #C0C0C0">
                                            <asp:RadioButtonList ID="rblNonParticipationNoYes" runat="server"
                                                RepeatDirection="Horizontal" CssClass="checkBoxStyle">
                                                <asp:ListItem Value="0">NO</asp:ListItem>
                                                <asp:ListItem Value="1">YES</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="tdText">&nbsp; If yes, why is removal considered critical to the student&#39;s program?
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNonParticipation_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                                            <div id="txtNonParticipation" style="width:96%" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP6.aspx',this); "></div>

                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Schedule Modification</td>
                        </tr>
                        <tr>
                            <td align="left" class="tdText">

                                <table class="style1">
                                    <tr>
                                        <td colspan="2">
                                            <strong>Shorter:</strong>&nbsp;Does this student require a shorter school day or
                        shorter school year?
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right-style: dashed; border-right-width: thin; border-right-color: #C0C0C0">
                                            <asp:RadioButtonList ID="rblScheduleModificationShort" runat="server"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0">NO</asp:ListItem>
                                                <asp:ListItem Value="1">Yes-shorter day</asp:ListItem>
                                                <asp:ListItem Value="2">Yes-shorter year</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="style3">&nbsp; If yes, answer the questions below.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <strong>Longer:</strong> Does this student require a longer school day or a longer
                        school year to prevent substantial loss of previously learned skills
                        and / or substantial difficulty in relearning skills?
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right-style: dashed; border-right-width: thin; border-right-color: #C0C0C0">
                                            <asp:RadioButtonList ID="rblScheduleModificationLong" runat="server"
                                                RepeatDirection="Horizontal" CssClass="checkBoxStyle">
                                                <asp:ListItem Value="0">NO</asp:ListItem>
                                                <asp:ListItem Value="1">Yes-longer day</asp:ListItem>
                                                <asp:ListItem Value="2">Yes-longer year</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="style3">&nbsp; If yes, answer the questions below.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr style="height: 55px;">
                                        <td colspan="2">How will the student’s schedule be modified? Why is this schedule modification being
                        recommended?<br />
                                            If a longer day or year is recommended, how will the school district coordinate
                        services across program components?
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSceduleModification_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                                            <div id="txtSceduleModification" style="width:96%" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP6.aspx',this); "></div>


                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="tdTextLeft">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Transportation Services
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="tdText">



                                <table width="98%">
                                    <tr>
                                        <td colspan="2">Does the student require transportation as a result of the disability(ies)? <%--<asp:RadioButton ID="fff" runat="server" Text="No" CssClass="checkBoxStyle" GroupName="transport" onclick="OnClientClicking1();"/>--%></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <input type="radio" id="rbtnTransportno" runat="server" name="rbtTransport" value="No"  onclick="setDisable()"/>No
                                            <%--<asp:RadioButton ID="rbtnTransportno" runat="server" GroupName="rbtTransport" Text="No"  onClick="setDisable()"  AutoPostBack="false" OnCheckedChanged="rbtnTransportno_CheckedChanged" />--%></td>
                                        <td style="width: 90%">Regular transportation will be provided in the same manner as it would be provided
                        for students without disabilities. If&nbsp;&nbsp;&nbsp;the child is placed away from the local school, transportation will be provided.</td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <input type="radio" id="rbtnTransportyes" runat="server" name="rbtTransport" value="Yes"  onclick="setEnable()"/>Yes
                                            <%--<asp:RadioButton ID="rbtnTransportyes" runat="server" GroupName="rbtTransport" Text="Yes" onClick="setEnable()" AutoPostBack="false" OnCheckedChanged="rbtnTransportyes_CheckedChanged" />--%>

                                        </td>
                                        <td>Special transportation will be provided in the following manner:<br />
                                            <asp:CheckBox ID="chkTransportServicesRegular" runat="server" Text="on a regular transportation vehicle with the following modifications and/or specialized equipment and precautions:"
                                                Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                                CssClass="checkBoxStyle" />
                                            <asp:TextBox ID="txtTransportServicesRegular" runat="server" CssClass="textClass"
                                                Width="100%"></asp:TextBox>
                                            <br />
                                            <asp:CheckBox runat="server" Text="on a special transportation vehicle with the following modifications and/or specialized equipment and precautions:"
                                                ID="chkTransportServicesSpecial"
                                                Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' CssClass="checkBoxStyle"></asp:CheckBox>
                                            <br />
                                            <asp:TextBox ID="txtTransportServicesSpecial" runat="server" CssClass="textClass" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" ></td>
                                       
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px double;">
                                After the team makes a transportation decision and after a placement decision
                        has been made, a parent may choose to provide transportation and may be eligible
                        for reimbursement under certain circumstances. Any parent who plans to transport
                        their child to school should notify the school district contact person.
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="tdTextLeft">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" style="margin-left: 40px">
                             
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue"
                        OnClick="btnSave_Click" OnClientClick="" />

                                <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy"
                        OnClick="btnSave_hdn_Click" OnClientClick="submitClick();"  style="display:none;"/>--%>
                            </td>
                        </tr>


                    </table>
                </div>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
</body>
</html>
