<%@ Page Language="C#" AutoEventWireup="true" CodeFile="homePage.aspx.cs" Inherits="homePage" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />

    <script type="text/javascript" src="../Administration/js/jquery.min.js"></script>


    <style>
        #dashboard-RHSVis {
            background-color: white;
            border: 2px solid #B6D1DD;
            border-radius: 5px 5px 5px 5px !important;
            box-shadow: 0 1px 5px 3px #BFC0C1;
            padding: 5px;
            width: 90%;
            margin: 0 auto;
            margin-bottom: 15px;
        }

        div.commonContainer {
            width: 62%;
            height: 325px;
            margin: 120px auto 120px auto;
        }

            div.commonContainer div.gryContainer {
                width: 222px;
                height: 283px;
                background: #dddddd;
                float: left;
                margin: 0 10px 0 0;
                padding: 20px 10px 10px 10px;
                cursor: pointer;
            }

            div.commonContainer div.nomarg {
                margin: 0;
            }

            div.commonContainer div.gryContainer:hover {
                background: #95cd80;
                color: #000;
                cursor: pointer;
            }

        .langinFieldset legend {
            padding: 2px;
        }

        .langinFieldset {
            width: 100%;
            padding: 0px;
            border: 0;
            margin: 35px auto 0 auto;
        }

        .opt-header {
            border-bottom: 1px solid #998A60;
            border-top: 1px solid #998A60;
            color: #333332;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 17px;
            font-weight: normal;
            margin-top: 106px;
            padding: 6px 0;
            text-align: center;
        }
    </style>
    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


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

                        <asp:Label ID="lblLoginName" runat="server" Text=""></asp:Label>
                    </li>
                    <li class="timeSs">
                        <div>
                            <div style="float: left; width: auto;"></div>
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
                      <li class="box7">
                           <a href="../../LoginContinue.aspx" title="StartUp Page" >Landing Board</a>
                        </li>
                    <li class="box50">
                        <a href="../Logout.aspx">Logout</a>
                    </li>
                </ul>

            </div>
        </div>
        <!-- dashboard container panel -->
        <div id="db-container">
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="../Administration/images/student-logo.jpg" alt="">
                </div>
                <div class="header-links"></div>
            </div>


            <!-- content panel -->
            <!-- content Left side -->
            <div class="clear"></div>

            <!-- content right side -->
            <div id="dashboard-RHSVis">




                <div id="main-heading-panel">Visual Lesson Plan</div>


                <div class="clear"></div>
                <hr />
                <hr />
                <div class="commonContainer">
                    <div class="gryContainer" onclick="window.location.href='LessonManagement.aspx';">
                        <fieldset class="langinFieldset">
                            <legend align="center">
                                <asp:ImageButton ID="imgeManagement" ImageUrl="~/VisualTool/images/lessionmanagement.png" runat="server" ToolTip="Lesson Management" Height="74px" Width="74px" PostBackUrl="~/VisualTool/LessonManagement.aspx" /></legend>
                            <div class="opt-header">Lesson Management</div>

                        </fieldset>

                    </div>
                    <div class="gryContainer" onclick="window.location.href='repository-manag.aspx';">

                        <fieldset class="langinFieldset">
                            <legend align="center">
                                <asp:ImageButton ID="imgRepository" ImageUrl="~/VisualTool/images/Repository Management.png" runat="server" ToolTip="Repository Management" Height="74px" Width="74px" PostBackUrl="~/VisualTool/repository-manag.aspx" /></legend>
                            <div class="opt-header">Repository Management</div>
                        </fieldset>
                    </div>
                    <div class="gryContainer nomarg" onclick="window.location.href='StudentLessonAndScore.aspx';">
                        <fieldset class="langinFieldset">
                            <legend align="center">
                                <asp:ImageButton ID="imgLessonScore" ImageUrl="~/VisualTool/images/Assign.png" runat="server" ToolTip="Lesson Assign and Score" Height="74px" Width="55px" PostBackUrl="~/VisualTool/StudentLessonAndScore.aspx" /></legend>
                            <div class="opt-header">
                                Assign
                                           Reinforcements
                            </div>

                        </fieldset>

                    </div>
                    <div class="clear"></div>
                </div>


                <div class="clear"></div>




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

