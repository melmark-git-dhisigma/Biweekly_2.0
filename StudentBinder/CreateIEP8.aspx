<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP8.aspx.cs" Inherits="StudentBinder_CreateIEP8" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            height: 42px;
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
            var txtAddInfoColDesc3 = document.getElementById('<%=txtAddInfoColDesc3.ClientID %>').innerHTML;
            PageMethods.submitIEP8(txtAddInfoColDesc3);
        }

        function GetFreetextval(content, divid) {
            if (divid == 'txtAddInfoColDesc3') {
                document.getElementById('<%=txtAddInfoColDesc3.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtAddInfoColDesc3.ClientID %>').innerHTML = content;
                document.getElementById('<%=txtAddInfoColDesc3_hdn.ClientID %>').value = window.escape(content);
            }

        }

        function chkLen() {
            var val = document.getElementById('txtAddInfoColDesc3').value;

            if ((parseInt(val.length) < 200)) {
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

        <div id="divIep8" style="width: 97%; border-radius: 3px 3px 3px 3px; padding: 7px;">
            <table width="95%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;"
                        class="auto-style1">Additional Information
                    </td>
                </tr>
                <br />
                <tr>
                    <td runat="server" id="tdMsg" style="color: #FF0000" colspan="1"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr style="height: 43px;">
                    <td align="left" class="tdText">
                        <asp:CheckBox ID="chkAddInfo1" runat="server" Text="Include the following transition information: the anticipated graduation date; a statement of interagency responsibilities or needed linkages;<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;the discussion of transfer of rights at least one year before age of majority; and a recommendation for Chapter 688 Referral." />
                    </td>
                </tr>
                <tr style="height: 29px;">
                    <td align="left" class="tdText">
                        <asp:CheckBox ID="chkAddInfo2" runat="server" Text="Document efforts to obtain participation if a parent and if student did not attend meeting or provide input." />
                    </td>
                </tr>
                <tr style="height: 46px;">
                    <td align="left" class="tdText">
                        <asp:CheckBox runat="server" Text="Record other relevant IEP information not previously stated."
                            ID="chkAddInfo3"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:TextBox ID="txtAddInfoColDesc3_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtAddInfoColDesc3" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP8.aspx',this); "></div>


                    </td>
                </tr>
                <tr>
                    <td align="left">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="text-align: center">&nbsp;
                                    <asp:Button ID="btnSave"
                                        runat="server" CssClass="NFButtonWithNoImage" Text="Save and Continue"
                                        OnClick="btnSave_Click" OnClientClick="" />


                                   <%-- <asp:Button ID="Button1_hdn"
                                        runat="server" CssClass="NFButton" Text="dummy"
                                        OnClick="btnSave_hdn_Click" OnClientClick="submitClick();" Style="display: none;" />--%>
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
