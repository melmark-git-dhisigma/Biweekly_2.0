<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddLessonProfile.aspx.cs" Inherits="AddLessonProfile" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/LandDesign.css" rel="stylesheet" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />
    <script src="scripts/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="../Administration/js/jquery.min.js"></script>
    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });

 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
    </script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

    <style type="text/css">
        .buttonNew1 {
            border: 1px groove black;
            padding: 5px;
        }

        input[type="text"] {
            background-color: white;
            border: 1px solid #D7CECE;
            border-radius: 3px 3px 3px 3px;
            color: #676767;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 13px;
            height: 28px;
            line-height: 26px;
            margin-right: 28px;
            padding: 0 5px 0 10px;
            width: 355px;
        }

        textarea {
            background-color: white;
            border: 1px solid #D7CECE;
            border-radius: 5px 5px 5px 5px;
            color: #676767;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 13px;
            padding: 0 0 0 10px;
            width: 390px;
            margin: 5px 0 0 3px;
        }

        .drpClass {
            background-color: white;
            border: 1px solid #D7CECE;
            border-radius: 3px 3px 3px 3px;
            color: #676767;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 13px;
            height: 26px;
            line-height: 27px;
            padding: 1px 2px 0 10px;
            width: 336px;
        }

        a.homecls, a.homecls:link, a.homecls:visited {
            background: url("images/hme.png")repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 5% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.homecls:hover {
                background-position: 0 -57px;
            }


        a.lessnClass, a.lessnClass:link, a.lessnClass:visited {
            background: url("images/lessonmanagement.png") repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 1% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.lessnClass:hover {
                background-position: 0 -57px;
            }

        a.repocls, a.repocls:link, a.repocls:visited {
            background: url("images/repo.png") repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 1% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.repocls:hover {
                background-position: 0 -57px;
            }
    </style>
    <script language="javascript" type="text/javascript">
        function validate() {
            var numbers = /^[0-9]+$/;
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


</head>

<body>
    <!-- top panel -->
    <form runat="server" id="form1">

        <div id="dashboard-top-panel">

            <div id="top-panel-container">
                <ul>

                    <li class="user" style="width: 50%;">
                        <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
                    </li>

                    <li class="timeSs">
                        <div>
                            <div style="float: left; width: auto;">
                            </div>
                            <div style="float: left; width: auto;" class="jclock"></div>
                        </div>
                    </li>
                     <li>
                        <a href="#" title="StartUp Page" onclick="loadmaster();">
                            <img src="images/StartHome.png" width="10" height="10" align="baseline" />
                            Landing Portal</a>

                    </li>
                    <li class="box40">
                        <a href="../Administration/AdminHome.aspx">Administration</a>
                    </li>




                </ul>






            </div>
        </div>
        <!-- dashboard container panel -->
        <div id="db-container">
            <!-- header -->
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="../Administration/images/student-logo.jpg" alt="">


                    <a href="homePage.aspx" class="homecls" title="Home"></a>
                    <a href="LessonManagement.aspx" class="lessnClass" title="Lesson Management"></a>
                    <a href="repository-manag.aspx" class="repocls" title="Repository Management"></a>

                </div>

            </div>

            <!-- content panel -->



            <div class="clear"></div>
            <br />

            <!-- content right side -->
            <div id="dashboard-RHSVin">
                 <div id="main-heading-panel" style="">Add Lesson Plan Profile</div>

                <div class="clear"></div><hr />

                <div class="dashboard-RHS-content">



                    <!-- CHANGABLE -->

                    <table style="width: 100%">
                        

                        <tr>
                            <td colspan="2" runat="server" id="tdMsg" style="text-align: center;"></td>
                        </tr>

                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;"></td>
                                        <td class="tdText">Lesson Name
                                        </td>
                                    </tr>

                                </table>

                            </td>
                            <td style="width: 80%; text-align: left;">
                                <span class="spanStyle">&nbsp;&nbsp;&nbsp;&nbsp;*</span>
                                <asp:TextBox ID="txtLessonName" runat="server" MaxLength="50"></asp:TextBox>

                            </td>
                        </tr>

                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;"></td>
                                        <td class="tdText">Description
                                        </td>
                                    </tr>
                                </table>

                            </td>
                            <td style="width: 60%; text-align: left;">
                                <span style="color: white;">&nbsp;&nbsp;&nbsp;&nbsp;*</span>
                                <asp:TextBox ID="txtDescription" runat="server" Columns="20" Rows="4" TextMode="MultiLine" Width="360px" MaxLength="30"></asp:TextBox>

                            </td>
                        </tr>

                        <tr>
                            <td class="tdText">

                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;"></td>
                                        <td class="tdText">Keywords
                                        </td>
                                    </tr>
                                </table>


                            </td>
                            <td style="width: 60%; text-align: left;">
                                <span style="color: white;">&nbsp;&nbsp;&nbsp;&nbsp;*</span>
                                <asp:TextBox ID="txtKeyword" runat="server" Rows="4" TextMode="MultiLine" Width="360px" MaxLength="30"></asp:TextBox>

                            </td>
                        </tr>

                        <tr>
                            <td>

                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;"></td>
                                        <td class="tdText">Domain
                                        </td>
                                    </tr>
                                </table>



                            </td>
                            <td align="left" style="width: 60%; text-align: left;">
                                <%--  <span class="spanStyle" style ="color:white;">*</span>--%>
                                <span class="spanStyle">&nbsp;&nbsp;&nbsp;&nbsp;*</span>
                                <asp:DropDownList ID="ddlDomain" runat="server" CssClass="drpClass">
                                </asp:DropDownList>


                            </td>
                        </tr>


                        <tr>


                            <td class="tdText">

                                <table style="width: 100%;">

                                    <tr>
                                        <td style="width: 50%;"></td>
                                        <td class="tdText">Category
                                        </td>
                                    </tr>
                                </table>

                            </td>
                            <td align="left" style="width: 60%; text-align: left;">
                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="left" style="width: 40%; text-align: left;"><span class="spanStyle">&nbsp;&nbsp;&nbsp;*</span>
                                                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                        <asp:ListItem>coin</asp:ListItem>
                                                        <asp:ListItem>mouse</asp:ListItem>
                                                        <asp:ListItem>match</asp:ListItem>
                                                        <asp:ListItem>time</asp:ListItem>
                                                        <asp:ListItem>content</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 40%; text-align: left;">
                                                                <asp:RadioButtonList ID="rdbtnContents" runat="server" RepeatDirection="Horizontal" Visible="False">
                                                                    <asp:ListItem>Single</asp:ListItem>
                                                                    <asp:ListItem Value="PPT">Presentation</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:RadioButtonList ID="rdbtnotherCat" runat="server" OnSelectedIndexChanged="rdbtnotherCat_SelectedIndexChanged" RepeatDirection="Horizontal" Visible="false">
                                                                    <asp:ListItem Value="Discreate">Discrete</asp:ListItem>
                                                                    <asp:ListItem Value="Chainned">Chained</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td style="width: 60%; text-align: left;">
                                                                <%--    <asp:Panel ID="panelSetStepNumber" runat="server" Visible="false">
                                                                        <table style="width: 100%;">
                                                                            <tr>
                                                                                <td class="auto-style2">No: Of Sets: </td>
                                                                                <td class="auto-style3">
                                                                                    <asp:TextBox ID="txtsetNumber" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="tdText1" style="width: 60%;">No: Of Steps/Trials: </td>
                                                                                <td style="width: 40%;">
                                                                                    <asp:TextBox ID="txtstepNumber" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </td>

                        </tr>

                        <tr>
                            <td class="tdTextSet" style="width: 20%; text-align: right;">&nbsp;</td>
                            <td style="width: 60%; text-align: left;">&nbsp</td>
                        </tr>

                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 15%;">

                                            <asp:Button ID="btnSave" runat="server" Text="Next" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick=" return validate();" ValidationGroup="a" />

                                        </td>
                                        <td style="width: 15%;">

                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="NFButton" OnClick="btnCancel_Click" />

                                        </td>
                                        <td>

                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Times New Roman" Font-Size="15px" ForeColor="Red"></asp:Label>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td class="tdTextSet" style="width: 20%; text-align: right;">&nbsp;</td>
                            <td style="width: 60%; text-align: left;">&nbsp;</td>
                        </tr>

                    </table>

                    <!-- CHANGEABLE -->
                </div>
                <div class="clear"></div>
            </div>



            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>&copy; Copyright 2015, Melmark, Inc. All rights reserved.</li>
                </ul>
            </div>


        </div>
    </form>
</body>

</html>
