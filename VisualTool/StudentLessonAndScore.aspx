<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentLessonAndScore.aspx.cs" Inherits="StudentLessonAndScore" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <title>Home</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" />
    <link href="styles/MenuStyle.css" rel="stylesheet" />
    <link href="styles/StudentLesson.css" rel="stylesheet" />
    <link href="styles/LessonDesignNew.css" rel="stylesheet" />
    <link href="styles/LandDesign.css" rel="stylesheet" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="styles/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>

    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>


    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


    </script>

    <style type="text/css">
        .lblStudentId {
            visibility: hidden;
        }

        div.students a {
            color: #FFFFFF;
            float: right;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 11px;
            font-weight: bold;
            height: 23px;
            letter-spacing: 1px;
            margin: 8px 0 0;
            padding: 4px 0 0 5px;
            text-align: left;
            width: 94px;
        }

        div.students img {
            float: left;
            height: 37px;
            margin: 3px;
            width: 38px;
        }

        }

        .content {
            width: 260px;
            height: 600px;
            overflow: auto;
        }

        .stCol {
            margin: 1PX;
        }

        .content img {
            width: 38px;
            height: 38px;
            margin: 3px;
        }

        .content p:nth-child(even) {
            color: #999;
            font-family: Georgia,serif;
            font-size: 17px;
            font-style: italic;
        }

        .content p:nth-child(3n+0) {
            color: #c96;
        }


        a.homecls, a.homecls:link, a.homecls:visited {
            background: url("images/hme.png")repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 2% 0 0;
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

    <script src="scripts/jquery-1.8.0.js"></script>
    <script src="scripts/cookies.js"></script>

    <script type="text/javascript">

        function getCookie(elem) {


            var g = $("#" + elem).children(".lblStudentId").text();
            //var par = $('#dlistStudent');
            var par = document.getElementById('dlistStudent');
            var child = par.getElementsByTagName("div");
            //var child = $(par).children();
            //var g = $(par).children("div");
            for (var i = 0; i < child.length; i++) {

                $(child[i]).attr('class', 'students');
            }
            $("#" + elem).attr('class', 'students-active');

            setCookie('studentId', g, 1);
        }


        $(document).ready(function () {
            $(".stud").click(function () {

                $("#stud").attr('class', 'students-active');
                var g = $(this).find('.lblStudentId').text();
                setCookie('studentId', g, 1);

            });
        });
 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



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
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHS">

                    <div class="dashboard-RHS-content">

                        <div id="main-heading-panel">Assign Reinforcement</div>
                        <!-- CHANGABLE -->

                        <div class="clear"></div>
                        <hr />
                        <div>

                            <table style="width: 100%; height: 100px">

                                <tr>
                                    <td style="text-align: center;">
                                        <table style="width: 99%;">
                                            <tr>
                                                <td style="text-align: left; padding-left: 150px" class="tdText1">Select Class:
                                    <asp:DropDownList ID="ddlClassName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlClassName_SelectedIndexChanged" CssClass="drpClass"></asp:DropDownList>
                                                </td>
                                                <td style="text-align: right; padding-right: 135px" class="tdText1">


                                                    <%--<asp:Button ID="btnLessonAssign" runat="server" Text="Lesson Assign" CssClass="buttonNew" Width="200px" OnClick="btnLessonAssign_Click" Height="40px" />--%>

                                                    <asp:Button ID="btnReinforce" runat="server" Text="" Style="background: url('images/Reinforce.png') no-repeat scroll left center rgb(252, 250, 233); height: 39px; width: 138px; border: medium none;" OnClick="btnReinforce_Click" />

                                                    <%--                                                    <asp:Button ID="Btnscores" runat="server" Text="Scores" CssClass="buttonNew" Width="200px" OnClick="Btnscores_Click" Height="40px" />--%>

                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>

                                <tr>
                                    <td style="height: 400px;">
                                        <div style="width: 100%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="padding-left: 155px; color: #0B0063; font-family: Georgia">Select a Student and click Reinforcement</td>
                                                </tr>
                                                <tr>
                                                    <td style="padding: 0px;">
                                                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <div style="width: 76%; background-color: #fff; border: none; margin: 0 auto;" class="divScroll content">
                                                                    <asp:DataList ID="dlistStudent" runat="server" RepeatDirection="Horizontal" RepeatColumns="6" OnItemDataBound="dlistStudent_ItemDataBound">
                                                                        <ItemTemplate>

                                                                            <div class="students" id="stud<%# Eval("stdId") %>" onclick="getCookie(this.id)">
                                                                                <asp:Image ID="imgStudents" runat="server" ImageUrl=""  />
                                                                                <a href="#"><%# Eval("studName") %></a>

                                                                                <asp:Label ID="lblStudentId" runat="server" CssClass="lblStudentId" Text='<%# Eval("stdId") %>' ></asp:Label>

                                                                            </div>
                                                                            <asp:HiddenField runat="server" id="hdnURL" Value='<%# Eval("ImageURL") %>' ></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                    </td>
                                                </tr>
                                            </table>

                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </div>


                        <!-- CHANGEABLE -->
                    </div>

                </div>
            </div>



        </div>
    </form>
</html>
