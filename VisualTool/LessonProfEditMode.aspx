<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LessonProfEditMode.aspx.cs" Inherits="LessonProfEditMode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <title runat="server" id="TitleName">Melmark Pennsylvania</title>

    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/LandDesign.css" rel="stylesheet" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <script src="scripts/jquery-1.8.0.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style3 {
            font-family: Calibri;
            color: #5B5B5B;
            line-height: 22px;
            font-weight: bold;
            font-size: 13px;
            padding-right: 1px;
            text-align: center;
            width: 22%;
        }

        .auto-style4 {
            width: 22%;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txtLessonName.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Enter your Lesson Plan !!!!</dv> ";
                document.getElementById("<%=txtLessonName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlDomain.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select any Domain</dv> ";
                document.getElementById("<%=ddlDomain.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlCategory.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select any Category</dv> ";
                document.getElementById("<%=ddlCategory.ClientID%>").focus();
                return false;
            }
            return true;

        }

    </script>

    <script type="text/javascript">

        $(function () {
            // Patch fractional .x, .y form parameters for IE10.
            if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
                Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
                    if (element.disabled) {
                        return;
                    }
                    this._activeElement = element;
                    this._postBackSettings = this._getPostBackSettings(element, element.name);
                    if (element.name) {
                        var tagName = element.tagName.toUpperCase();
                        if (tagName === 'INPUT') {
                            var type = element.type;
                            if (type === 'submit') {
                                this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                            }
                            else if (type === 'image') {
                                this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                            }
                        }
                        else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                            this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                        }
                    }
                };
            }
        });

</script>
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>
                    <li class="user">
                        <img src="images/admin-icon.png" width="16" height="16" align="baseline">Super Admin</li>
                    <li>
                        <img src="images/time-icon.png" width="14" height="14" align="baseline">5:41 PM</li>
                    <li><a href="#">
                        <img src="images/srch-icon.png" width="16" height="16" align="baseline">Search</a></li>
                    <li></li>
                </ul>
            </div>
        </div>

        <!-- dashboard container panel -->
        <div id="db-container">
            <!-- header -->
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="images/dashboard-logo.jpg" width="200" height="50">
                </div>
                <div class="header-links">
                    <ul>
                        <li><a href="homePage.aspx">
                            <img src="icons/home.png" width="28" height="25"><br>
                            Home</a></li>
                    </ul>
                </div>
            </div>


            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHS">

                    <div class="dashboard-RHS-content">

                        <div id="main-heading-panel">Lesson Plan Profile</div>

                        <!-- CHANGABLE -->

                        <table style="width: 100%">

                            <tr>
                                <td colspan="2" runat="server" id="tdMsg" style="text-align: center;"></td>
                            </tr>

                            <tr>
                                <td class="auto-style3" style="text-align: right;">Lesson Name</td>
                                <td style="width: 80%; text-align: left;">

                                    <asp:TextBox ID="txtLessonName" runat="server" CssClass="Textbox001" MaxLength="50"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td class="auto-style3" style="text-align: right;">Description</td>
                                <td style="width: 60%; text-align: left;">

                                    <asp:TextBox ID="txtDescription" runat="server" Columns="10" CssClass="Textbox001" Rows="3" TextMode="MultiLine" Width="400px"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td class="auto-style3" style="text-align: right;">Keywords</td>
                                <td style="width: 60%; text-align: left;">

                                    <asp:TextBox ID="txtKeyword" runat="server" CssClass="Textbox001" Rows="3" TextMode="MultiLine" Width="400px" Columns="10"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td class="auto-style3" style="text-align: right;">Domain</td>
                                <td align="left" style="width: 60%; text-align: left;">


                                    <asp:DropDownList ID="ddlDomain" runat="server" CssClass="drpClass">
                                    </asp:DropDownList>


                                </td>
                            </tr>



                            <tr>
                                <td class="auto-style3" style="text-align: right;">Category</td>
                                <td align="left" style="width: 60%; text-align: left;">
                                    <table style="width: 100%;">
                                        <td align="left" style="width: 40%; text-align: left;">&nbsp;<asp:DropDownList ID="ddlCategory" runat="server" CssClass="drpClass" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            <asp:ListItem>coin</asp:ListItem>
                                            <asp:ListItem>mouse</asp:ListItem>
                                            <asp:ListItem>match</asp:ListItem>
                                            <asp:ListItem>time</asp:ListItem>
                                            <asp:ListItem>content</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                        <td align="left">

                                            <asp:RadioButtonList ID="rdbtnContents" runat="server" RepeatDirection="Horizontal" Visible="False">
                                                <asp:ListItem>Single</asp:ListItem>
                                                <asp:ListItem>PPT</asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>

                                    </table>



                                </td>
                            </tr>

                            <tr>
                                <td class="auto-style4">&nbsp;</td>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 15%;">

                                                <asp:Button ID="btnSave" runat="server" Text="Next" CssClass="buttonNew" OnClick="btnSave_Click" OnClientClick=" return validate();" ValidationGroup="a" />

                                            </td>
                                            <td style="width: 25%;">

                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonNew" OnClick="btnCancel_Click" />

                                            </td>
                                            <td>

                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Times New Roman" Font-Size="15px" ForeColor="Red"></asp:Label>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td class="auto-style3" style="text-align: right;">&nbsp;</td>
                                <td style="width: 60%; text-align: left;">&nbsp;</td>
                            </tr>

                        </table>

                        <!-- CHANGEABLE -->
                    </div>

                </div>
            </div>


            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>COPYRIGHT &copy; 2012 Melmark Inc. All rights reserved</li>
                </ul>
            </div>


        </div>
    </form>
</body>
</html>
