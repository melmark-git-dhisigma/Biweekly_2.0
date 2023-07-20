<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mouseLesson_Creator.aspx.cs"
    Inherits="mouseLesson_Creator" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/timeLesson.css" rel="stylesheet" />
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/mouseStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />

    <link href="styles/commonStyle.css" rel="stylesheet" />


    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="scripts/jquery.ui.slider.js"></script>
    <link href="styles/jquery.ui.theme.css" rel="stylesheet" />
    <link href="styles/jquery.ui.slider.css" rel="stylesheet" />
    <link rel="stylesheet" href="styles/demos.css">
    <style type="text/css">
        .menuBox {
            width: 110px;
            border: 1px groove red;
            position: absolute;
            background-color: white;
        }

            .menuBox .rightMenuList li {
                padding: 2px;
                margin: 1px;
                list-style: none;
            }

                .menuBox .rightMenuList li:hover {
                    background-color: #c3c1c1;
                    cursor: pointer;
                }

        .renameDiv {
            position: absolute;
            z-index: 1001;
            margin-top: 20px;
        }

        .renameSetOk, .renameSetNo, .renameStepOk, .renameStepNo {
            background-color: #C9C9C9;
            border: 1px groove #8C8C8C;
            margin: 2px;
            font-size: 11px;
        }

            .renameSetOk:hover, .renameSetNo:hover, .renameStepOk:hover, .renameStepNo:hover {
                background-color: #E8E8E8;
                cursor: pointer;
            }

        .renameText {
            height: 20px;
            border: 1px groove black;
            color: blue;
        }

        #closeNew {
            display: block;
            height: 23px;
            overflow: hidden;
            position: absolute;
            right: 4px;
            top: 2px;
            width: 24px;
        }

        a {
            text-decoration: none;
        }
    </style>
    <style>
        #red, #green, #blue {
            float: left;
            clear: left;
            width: 300px;
            margin: 15px;
        }

        #swatch {
            width: 120px;
            height: 100px;
            margin-top: 18px;
            margin-left: 350px;
            background-image: none;
        }

        .workArea {
            width: 30%;
            height: 30px;
        }

        #red .ui-slider-range {
            background: #ef2929;
        }

        #red .ui-slider-handle {
            border-color: #ef2929;
        }

        #green .ui-slider-range {
            background: #8ae234;
        }

        #green .ui-slider-handle {
            border-color: #8ae234;
        }

        #blue .ui-slider-range {
            background: #729fcf;
        }

        #blue .ui-slider-handle {
            border-color: #729fcf;
        }

        #demo-frame > div.demo {
            padding: 10px !important;
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

    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


    </script>





    <!-- SCRIPT TO RENAME STEP AND SETS -->
    <script type="text/javascript">
        var selOption;
        var selSetStep;
        var selText;
        $(document).ready(function () {



            listBoxMenu();

            //  makeAllDraggable();
            //  makeAllDroppable();
        });


        //function makeAllDraggable() {
        //    $('.draggable').draggable({
        //        revert: "invalid",
        //        helper: "clone",
        //        cursor: "move"
        //    });
        //}
        //function makeAllDroppable() {
        //    $('.droppable').droppable({
        //        accept: ".images,.audios",
        //        drop: function (event, ui) {

        //            if ($(ui.draggable).hasClass('images')) {


        //                var newElement = ui.draggable.clone();

        //                if ($(this).find('.vidDiv').length == 0) {
        //                    if ((newElement).hasClass('draggable')) {
        //                        $(newElement).removeClass('draggable');
        //                        $(newElement).css({ 'margin': '0px', 'height': '100%', 'width': '100%' });
        //                        newElement = $('<div style="position:absolute;width:50px;height:50px;" class="demo"><div  class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div></div>').append(newElement);
        //                        $(newElement).appendTo(this);
        //                        $(newElement).draggable({ containment: "parent" }).resizable();

        //                        hoverMenu();

        //                    }
        //                }
        //            }
        //            if ($(ui.draggable).hasClass('audios')) {
        //                if ($(this).find('.player').length > 0) {


        //                    var newElement = ui.draggable;
        //                    musicFile = $(newElement).attr('alt').replace('~/VisualTool/', '');


        //                    $(this).find('.player').empty();
        //                    $(this).find('.player').append('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /><param name="wmode" value="transparent" /></object>');

        //                }
        //                else {
        //                    alert('Enable audio first.');
        //                }
        //            }

        //        }
        //    });
        //}


        //EXTERNAL FUNCTIONS//

        function listBoxMenu() {

            $('#lstSets,#lstSteps').bind("contextmenu", function (event) {
                event.preventDefault();

                selctList = $(this).attr('id');


                if (selctList == 'lstSets') {
                    selOption = $('#lstSets option:selected');

                }
                else {
                    selOption = $('#lstSteps option:selected');
                }
                selSetStep = selOption.attr('value');
                selText = selOption.html();
                var listboxId = selOption.parent().attr('id');



                $('.menuBox').remove();
                var menuBox = "<div class='menuBox' style='top:" + event.pageY + "px;left:" + event.pageX + "px'><ul class='rightMenuList'><li class='menuRename'>Rename</li></ul></div>"

                $(menuBox).appendTo("body");


                $('.menuRename').click(function (event) {
                    if (listboxId == "lstSets") {
                        $('.renameSetText').val(selText);
                        $('.renameAreaSet').fadeIn();

                        $('.renameSetText').focus();
                        $('.renameSetText').select();
                    }
                    if (listboxId == "lstSteps") {
                        $('.renameStepText').val(selText);
                        $('.renameAreaStep').fadeIn();

                        $('.renameStepText').focus();
                        $('.renameStepText').select();

                    }
                });



            });


            $('.renameSetNo').click(function () {
                $('.renameSetText').val('');
                $('.renameAreaSet').fadeOut();

            });

            $('.renameSetOk').click(function () {

                var e = $("#lstSteps");
                var selStep = e.val();
                var value = $('.renameSetText').val();

                fn_renamer(selSetStep, value, selOption);
            });

            $('.renameStepNo').click(function () {
                $('.renameStepText').val('');
                $('.renameAreaStep').fadeOut();

            });

            $('.renameStepOk').click(function () {

                var e = $("#lstSets");
                var selSet = e.val();
                var value = $('.renameStepText').val();

                fn_renamer(selSetStep, value, selOption);
            });

            $(document).bind("click", function (event) {
                $('.menuBox').remove();
            });
        }

        function stripIt(x) {
            x.value = x.value.replace(/['"]/g, '');
        };


        function fn_renamer(S_No, value, control) {
            $.ajax({
                url: "mouseLesson_Creator.aspx/rename",
                data: "{'S_No':'" + S_No + "','value':'" + value + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    $('.renameSetText').val('');
                    $('.renameAreaSet').fadeOut();

                    $('.renameStepText').val('');
                    $('.renameAreaStep').fadeOut();


                    control.html(value);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);

                }
            });
        }

 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
    </script>
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>
                    <li class="user" style="width: 50%;">
                        <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
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
                   
                </ul>

            </div>
        </div>
        <!-- dashboard container panel -->
        <div id="db-container">
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="../Administration/images/student-logo.jpg" alt="">


                    <a href="homePage.aspx" class="homecls" title="Home"></a>
                    <a href="LessonManagement.aspx" class="lessnClass" title="Lesson Management"></a>
                    <a href="repository-manag.aspx" class="repocls" title="Repository Management"></a>
                </div>

            </div>
            <div class="clear"></div>
            <br />
            <!-- content right side -->
            <div id="dashboard-RHSVin">
                <div class="dashboard-RHS-content">
                    <h2 class=" mous">Mouse

                        <span id="td_LP" runat="server"></span>
                    </h2>
                    <hr />
                    <div id="mainContainer">
                        <table style="width: 100%;" cellpadding="0" cellspacing="0">

                            <tr>
                                <td width="2%" rowspan="2" style="vertical-align: top;">
                                    <div id="btn_profile" title="Profile" class="with-tip">
                                        <div class="btn btn-blue">
                                        </div>
                                    </div>
                                    <div id="btn_preview" style="margin-top: 2px;" title="View" class="with-tip">
                                        <div class="btn btn-purple">
                                        </div>
                                    </div>
                                    <div id="btn_finish" style="margin-top: 2px;" title="Save" class="with-tip">
                                        <div class="btn btn-black">
                                        </div>
                                    </div>
                                    <div id="btn_close" style="margin-top: 2px;" title="Close" class="with-tip">
                                        <div class="btn btn-redd">
                                        </div>
                                    </div>
                                </td>
                                <td width="15%" style="vertical-align: top;" rowspan="2">
                                    <div id="setstepContainer">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td class="spanSet">SETS
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center; vertical-align: top;">
                                                            <div class="setDiv">
                                                                <div class="renameAreaSet" style="display: none;">
                                                                    <div class="waitBlank rename" style="width: 110px; height: 207px; display: block; opacity: .8;">
                                                                    </div>
                                                                    <div class="renameDiv">
                                                                        <input type="text" class="renameText renameSetText" style="width: 100px;" onblur='stripIt(this);' /><br />
                                                                        <input type="button" class="renameSetOk" value="DONE" />
                                                                        <input type="button" class="renameSetNo" value="CANCEL" />
                                                                    </div>
                                                                </div>
                                                                <asp:UpdateProgress runat="server" ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1"
                                                                    DisplayAfter="100">
                                                                    <ProgressTemplate>
                                                                        <div class="waitBlank" style="width: 100px; height: 207px;">
                                                                            <div style="margin: 75% auto auto 33%;">
                                                                                <asp:Image ID="Image2" runat="server" src="icons/wait(s).gif" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td style="width: 95%;">
                                                                            <asp:ListBox ID="lstSets" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstSets_SelectedIndexChanged"></asp:ListBox>
                                                                        </td>
                                                                        <td>
                                                                            <div class="arrowBox">
                                                                                <div class="upArrow">
                                                                                    <asp:ImageButton ID="btn_setUp" runat="server" ImageUrl="icons/arrow_up.png" OnClick="btn_setUp_Click" />
                                                                                </div>
                                                                                <div class="downArrow">
                                                                                    <asp:ImageButton ID="btn_setDown" ImageUrl="~/VisualTool/icons/arrow_down.png" runat="server"
                                                                                        OnClick="btn_setDown_Click" />
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="width: 5px; margin: 0; padding: 0;">
                                                                        <div class="btn btn-red" style="margin-left: 25px; float: left;">
                                                                            <asp:ImageButton ID="imgBtn_setDelete" ImageUrl="~/VisualTool/images/DeleteSetLight.JPG" runat="server" OnClick="imgBtn_setDelete_Click" />
                                                                        </div>
                                                                        <div class="btn btn-green" style="margin-left: 25px; float: left;">
                                                                            <asp:ImageButton ID="imgBtn_AddSet" ImageUrl="~/VisualTool/images/AddSetLight.JPG" runat="server" OnClick="imgBtn_AddSet_Click" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%;">
                                                            <div class="StepDiv">
                                                                <div class="renameAreaStep" style="display: none;">
                                                                    <div class="waitBlank rename" style="width: 110px; height: 207px; display: block; opacity: .8;">
                                                                    </div>
                                                                    <div class="renameDiv">
                                                                        <input type="text" class="renameText renameStepText" style="width: 100px;" onblur='stripIt(this);' /><br />
                                                                        <input type="button" class="renameStepOk" value="DONE" />
                                                                        <input type="button" class="renameStepNo" value="CANCEL" />
                                                                    </div>
                                                                </div>
                                                                <asp:UpdateProgress runat="server" ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1"
                                                                    DisplayAfter="100">
                                                                    <ProgressTemplate>
                                                                        <div class="waitBlank" style="width: 100px; height: 207px;">
                                                                            <div style="margin: 75% auto auto 33%;">
                                                                                <asp:Image ID="rightWait2" runat="server" src="icons/wait(s).gif" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td class="spanSet">STEPS
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: center;">
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td style="width: 95%;">
                                                                                        <asp:ListBox ID="lstSteps" runat="server" Height="135px" Width="95%" AutoPostBack="True"
                                                                                            OnSelectedIndexChanged="lstSteps_SelectedIndexChanged"></asp:ListBox>
                                                                                    </td>
                                                                                    <td style="width: 5%; vertical-align: top;">
                                                                                        <div class="arrowBox">
                                                                                            <div class="upArrow">
                                                                                                <asp:ImageButton ID="btn_stepUp" runat="server" ImageUrl="icons/arrow_up.png" OnClick="btn_stepUp_Click" />
                                                                                            </div>
                                                                                            <div class="downArrow">
                                                                                                <asp:ImageButton ID="btn_stepDown" ImageUrl="~/VisualTool/icons/arrow_down.png" runat="server"
                                                                                                    OnClick="btn_stepDown_Click" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td style="width: 5px; margin: 0; padding: 0;">
                                                                                                    <div class="btn btn-red" style="margin-left: 25px; float: left;">
                                                                                                        <asp:ImageButton ID="imgBtn_stepDelete" ImageUrl="~/VisualTool/images/DeleteSetLight.JPG" runat="server" OnClick="imgBtn_stepDelete_Click" />
                                                                                                    </div>
                                                                                                    <div class="btn btn-green" style="margin-left: 25px; float: left;">
                                                                                                        <asp:ImageButton ID="imgBtn_AddStep" ImageUrl="~/VisualTool/images/AddSetLight.JPG" runat="server" OnClick="imgBtn_AddStep_Click" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td width="75%">
                                    <div id="workArea" class="droppable">
                                    </div>
                                </td>
                                <td width="5%">
                                    <div id="slideArea">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr style="vertical-align: top;">
                                                <td class="spanSet" style="font-size: 11px;">SPEED
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <div class="demo">
                                                        <p>
                                                        </p>
                                                        <div id="slider-vertical" style="height: 224px;">
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 65%;">
                                    <div id="mse_objectContainer">
                                        <asp:DataList ID="dlImages" runat="server" Width="100%" HorizontalAlign="Left" RepeatColumns="6"
                                            RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <div class="dl_images">
                                                                <asp:Image ID="image" runat="server" ImageUrl='<%# Eval("Path") %>' Height="85px"
                                                                    Width="85px" class="draggable" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>

                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div class="messageRibbon">
                        <div class="innerMsgRibbon">
                            HEO
                        </div>
                    </div>
                    <div id="previewBoardContainer" style="width: 100%; height: 100%;">
                        <div id="previewBoard" class="web_dialog" style="top: 10%; left: 18%; display: none;">
                            <div id="sign_up5">
                                <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300"
                                        width="18" height="18" alt="" /></a>
                                <h3>Lesson Profile
                                </h3>
                                <hr />
                                <iframe id="previewFrame" style="width: 100%" scrolling="auto"></iframe>
                                <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                            </div>
                            <div id="previewClose">
                            </div>
                        </div>
                    </div>
                    <div id="prevBoxContainer" style="width: 100%; height: 100%;">
                        <div id="previewBox" class="web_dialog" style="top: 10%; left: 6%; display: none;">
                            <div id="signupNew">
                                <a id="closeNew" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300"
                                        width="18" height="18" alt="" /></a>
                                <h3>Mouse Editor Preview
                                </h3>
                                <hr />
                                <iframe id="prevboxFrame" style="width: 100%; height: 530px;"></iframe>
                            </div>
                        </div>
                    </div>
                    <%--  <div id="previewBoardContainer" style="width: 905px;">
                            <div id="previewBoard" class="web_dialog" style="top:10%; left:18%; display:none;">
                                <iframe id="previewFrame" style="width: 100%" scrolling="auto"></iframe>
                                <input id="btn_previewOk" type="button" value="Ok" />
                            </div>
                            <div id="previewClose"></div>

                        </div>--%>
                    <div class="fullOverlay">
                    </div>
                </div>
                <div class="clear">
                </div>
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
<script type="text/javascript">

    var currentValue = "";
    var workVaild = 0;
    $(document).ready(function () {

        listBoxMenu();

        //  makeAllDraggable();
        //  makeAllDroppable();

        $('.draggable').draggable({
            revert: 'invalid',
            helper: 'clone',
            cursor: 'move'

        });

        $('#workArea').droppable({
            accept: ".draggable",
            drop: function (event, ui) {

                // var W = $('#workArea').offset();
                //  alert(W);
                var newElement = ui.draggable.clone();
                if ((newElement).hasClass('draggable')) {
                    $(newElement).removeClass('draggable');
                    $(newElement).removeClass('draggable2');
                    //  $(newElement).css({ height: '100%', width: '100%', margin: '0px' });
                    //newElement = $('<div style="width:200px; height:200px; padding:3px; position: absolute;" class="demo"></div>');
                    newElement = $('<div style="position:relative;height:75px;width:75px; float:left; margin:5px;" class="demo"><div class="menuButtons"><img class="smallDelete" src="images/deletnwblk.png" onclick="deleteItem(this)" /></div></div>').append(newElement);
                    $(newElement).appendTo(this);
                    hoverMenu();
                    workVaild = 1;
                    // $('.demo').draggable().resizable();
                    // hoverMenu();

                }
            }

        });

    });


    $('#btn_finish').click(function () {

        var save;
        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
        var speedvalue = $('#slider-vertical').slider('option', 'value');
        var list = $('#workArea').find('.demo');
        var x = list.length;
        var numObj = "";
        var arrData = [];

        for (var i = 0; i < list.length; i++) {

            numObj = list.length;
            var innerContent = $(list[i]);
            var htmlObject = $(innerContent).html();
            arrData[i] = $(innerContent).find('.ui-draggable').attr('src');

        }

        $.ajax({
            url: "mouseLesson_Creator.aspx/SaveData",
            data: "{ 'contents':'" + arrData + "','numObjects':'" + numObj + "','setNumber':'" + setValue + "','stepNumber':'" + stepValue + "','speedLevel':'" + speedvalue + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                save = true;
                $('.innerMsgRibbon').text('Saved');
                $('.messageRibbon').show().fadeOut(2000);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });

    });


    $('#btn_preview').click(function () {

        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
        //var speedvalue = $('#slider-vertical').slider('option', 'value');


        $('.fullOverlay').fadeIn('slow', function () {
            //showHtml();
            $('#prevboxFrame').attr('src', 'PreviewPage_MouseEditor.aspx?setNumber=' + setValue + '&stepNumber=' + stepValue);
            $('#previewBox').fadeIn();
            $('#textEditorDiv').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });

        //else {
        //    if (confirm('File not saved... Save now?')) {
        //        saveFn();
        //    }
        //}

    });

    $('#btn_previewOk').click(function () {
        $('#previewBoard').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('slow');
        });
    });


    $('#close_x').click(function () {
        $('#previewBoard').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });


    $('#closeNew').click(function () {
        $('#previewBox').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });

    //$('#lstSets').change(function() {
    //    var x = $("#lstSets option:selected").val();
    //    alert(x);
    //});
    //$('#btn_profile').click(function () {
    //    alert(currentValue);
    //});


    $('#lstSteps option').click(function () {

        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
        // alert(setValue);
        //  alert(stepValue);
    });


    $('#btn_close').click(function () {
        if (confirm('Any unsaved changes will be discarded...\nSure you want to close?')) {
            window.location = 'LessonManagement.aspx';
        }
    });


</script>
<script type="text/javascript">

    function GetQueryStringParams(sParam) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                return sParameterName[1];
            }
        }
    }

    $(document).ready(function () {

        listBoxMenu();

        // makeAllDraggable();
        //  makeAllDroppable();


        var edit = GetQueryStringParams('edit');


        if (edit == '1') {

            var setValue = $('#lstSets').val();
            var stepValue = $('#lstSteps').val();

            $.ajax({
                url: "mouseLesson_Creator.aspx/GetElementData",
                data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    save = true;
                    //$('.innerMsgRibbon').text('Saved');
                    //$('.messageRibbon').show().fadeOut(2000);
                    var contents = data.d;
                    //  alert(contents);
                    var content_speed = contents.split('^');
                    var content_Data = contents.split(',');
                    speedValue = content_speed[content_speed.length - 1];
                    // alert(speedValue);

                    $("#slider-vertical").slider('option', 'value', speedValue);
                    //  alert(content_Data);

                    for (var i = 0; i < content_Data.length - 1; i++) {

                        var newElement = "";
                        $(newElement).css({ height: '100%', width: '100%', margin: '0px' });
                        //newElement = $('<div style="width:200px; height:200px; padding:3px; position: absolute;" class="demo"></div>');
                        newElement = $('<div style="position:relative;float:left; margin:5px; height:75px;width:75px;" class="demo"><img id = "imgAnimate' + i + '" class = "ui-draggable" Height = "75px" Width = "75px" src="' + content_Data[i] + '" " /><div class="menuButtons"><img class="smallDelete" src="images/deletnwblk.png" onclick="deleteItem(this)" /></div></div>');


                        //  $('#workArea').append('<li class ="demo"><img class = "a" id = "imgAnimate' + i + '" Height = "100px" Width = "100px" src="' + content_Data[i] + '" style="position:relative;float:left;" " /></li><div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div>');
                        $(newElement).appendTo('#workArea');
                        hoverMenu();

                    }


                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });



        }

    });
</script>
<script type="text/javascript">
    function hoverMenu() {
        $('.demo').mouseover(function () {
            $(this).find('.menuButtons').show();
        });

        $('.demo').mouseout(function () {
            $(this).find('.menuButtons').hide();
        });

    }

    function deleteItem(e) {
        if (confirm('Do you want to delete?')) {
            $(e).parents('div.demo').remove();
        }
    }

    function setListBoxFun() {


        //$('#lstSteps option').click(function () {

        $('#workArea').empty();
        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
        // alert(setValue);
        // alert(stepValue);

        $.ajax({
            url: "mouseLesson_Creator.aspx/GetElementData",
            data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                save = true;
                //$('.innerMsgRibbon').text('Saved');
                //$('.messageRibbon').show().fadeOut(2000);
                var contents = data.d;
                //  alert(contents);

                var content_speed = contents.split('^');
                var content_Data = contents.split(',');
                speedValue = content_speed[content_speed.length - 1];
                // alert(speedValue);

                $("#slider-vertical").slider('option', 'value', speedValue);
                //  alert(content_Data);

                for (var i = 0; i < content_Data.length - 1; i++) {

                    var newElement = "";
                    $(newElement).css({ height: '100%', width: '100%', margin: '0px' });
                    //newElement = $('<div style="width:200px; height:200px; padding:3px; position: absolute;" class="demo"></div>');
                    newElement = $('<div style="position:relative;height:75px;width:75px; float:left; margin:5px;" class="demo"><img id = "imgAnimate' + i + '"  class = "ui-draggable" Height = "75px" Width = "75px" src="' + content_Data[i] + '" " /><div class="menuButtons"><img class="smallDelete" src="images/deletnwblk.png" onclick="deleteItem(this)" /></div></div>');

                    //  $('#workArea').append('<li class ="demo"><img class = "a" id = "imgAnimate' + i + '" Height = "100px" Width = "100px" src="' + content_Data[i] + '" style="position:relative;float:left;" " /></li><div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div>');
                    $(newElement).appendTo('#workArea');
                    hoverMenu();
                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });


        //});



    }

    function onLoadStepData() {

        // alert('asdasdasd');

        $('#workArea').empty();
        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
         alert(setValue);
        alert(stepValue);

        $.ajax({
            url: "mouseLesson_Creator.aspx/GetElementData",
            data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                save = true;
                //$('.innerMsgRibbon').text('Saved');
                //$('.messageRibbon').show().fadeOut(2000);
                var contents = data.d;
                //  alert(contents);
                var content_speed = contents.split('^');
                var content_Data = contents.split(',');
                speedValue = content_speed[content_speed.length - 1];
                //alert(speedValue);

                $("#slider-vertical").slider('option', 'value', speedValue);
                //  alert(content_Data);

                for (var i = 0; i < content_Data.length - 1; i++) {

                    var newElement = "";
                    $(newElement).css({ height: '100%', width: '100%', margin: '0px' });
                    //newElement = $('<div style="width:200px; height:200px; padding:3px; position: absolute;" class="demo"></div>');
                    newElement = $('<div style="position:relative;height:75px;width:75px; float:left; margin:5px;" class="demo"><img  id = "imgAnimate' + i + '"  class = "ui-draggable" Height = "75px" Width = "75px" src="' + content_Data[i] + '" " /><div class="menuButtons"><img class="smallDelete" src="images/deletnwblk.png" onclick="deleteItem(this)" /></div></div>');


                    //  $('#workArea').append('<li class ="demo"><img class = "a" id = "imgAnimate' + i + '" Height = "100px" Width = "100px" src="' + content_Data[i] + '" style="position:relative;float:left;" " /></li><div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div>');
                    $(newElement).appendTo('#workArea');
                    hoverMenu();

                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });

    }

    function imgAddClick() {
        $('#workArea').empty();
    }

    function deleteFun() {
        alert('No deletion posible in this criteria. it needs minimum one set and one step !!! ');
    }

    $(function () {
        $("#slider-vertical").slider({
            orientation: "vertical",
            range: "min",
            min: 1,
            max: 4,
            value: 2,
            slide: function (event, ui) {
                $("#amount").val(ui.value);
            }
        });
        $("#amount").val($("#slider-vertical").slider("value"));

    });

    $('#btn_profile').click(function () {
        $('.fullOverlay').fadeIn('slow', function () {
            $('#previewFrame').attr('src', 'profilePreview.aspx');
            $('#previewBoard').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
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

</script>
</html>
