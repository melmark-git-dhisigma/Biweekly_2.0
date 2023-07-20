<%@ Page Language="C#" AutoEventWireup="true" CodeFile="time_editorPage.aspx.cs"
    Inherits="time_editorPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" type="text/css" />
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/timeLesson.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <%-- <script type="text/javascript" src="js/jquery.min.js"></script>--%>
    <script type="text/javascript" src="scripts/anologClock.js"></script>
    <link href="styles/commonStyle.css" rel="stylesheet" />
    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
     <script type="text/javascript">
         $(function ($) {
             $('.jclock').jclock();
         });


    </script>
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

        #dashboard-RHSVis {
            background-color: #FFFFFF;
            border: 2px solid #B6D1DD;
            border-radius: 5px 5px 5px 5px !important;
            box-shadow: 0 1px 5px 3px #BFC0C1;
            margin: 0 auto 15px auto;
            padding: 5px;
            width: 90%;
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


        #loadingOverlay {
            position: fixed;
            width: 100%;
            height: 800px;
            opacity: .6;
            background-color: silver;
            top: 0px;
            left: 0px;
            z-index: 1000;
            display: none;
        }

        /*Loading*/
        #loading {
            display: none;
            font-size: 25px;
            padding-top: 211px;
            z-index: 1001;
            position: absolute;
            top: 0px;
            left: 622px;
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

        .removeImage {
        color:blue;
        }
        .removeImage:hover {
        color:red;
        cursor:pointer;
        }
    </style>

   


    <!-- SCRIPT TO RENAME STEP AND SETS -->
    <script type="text/javascript">
        var selOption;
        var selSetStep;
        var selText;
        $(document).ready(function () {

            listBoxMenu();

            makeAllDraggable();
            makeAllDroppable();
        });


        function makeAllDraggable() {
            $('.draggable').draggable({
                revert: "invalid",
                helper: "clone",
                cursor: "move"
            });
        }
        function makeAllDroppable() {
            $('.droppable').droppable({
                accept: ".images,.audios",
                drop: function (event, ui) {

                    if ($(ui.draggable).hasClass('images')) {


                        var newElement = ui.draggable.clone();

                        if ($(this).find('.vidDiv').length == 0) {
                            if ((newElement).hasClass('draggable')) {
                                $(newElement).removeClass('draggable');
                                $(newElement).css({ 'margin': '0px', 'height': '100%', 'width': '100%' });
                                newElement = $('<div style="position:absolute;width:50px;height:50px;" class="demo"><div  class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div></div>').append(newElement);
                                $(newElement).appendTo(this);
                                $(newElement).draggable({ containment: "parent" }).resizable();

                                hoverMenu();

                            }
                        }
                    }
                    if ($(ui.draggable).hasClass('audios')) {
                        if ($(this).find('.player').length > 0) {


                            var newElement = ui.draggable;
                            musicFile = $(newElement).attr('alt').replace('~/VisualTool/', '');


                            $(this).find('.player').empty();
                            $(this).find('.player').append('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /><param name="wmode" value="transparent" /></object>');

                        }
                        else {
                            alert('Enable audio first.');
                        }
                    }

                }
            });
        }


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
                url: "time_editorPage.aspx/rename",
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


    </script>


    <script type="text/javascript">
        var IsDigitsClock = true;
        var clockBaground = null;
        function draw_clock(imgPath, hTime, mTime, IsDigits) {
            imgColor = '#FF0000'
            var complementColor = "#444444";
            var nwSrc = imgColor;
            if (imgPath != null && imgPath != '1' && imgPath != "") {
                var img = new Image();
                img.src = imgPath;
                var rgb = getAverageColourAsRGB(img);
                var complementRgb = { r: 102, g: 102, b: 102 }
                //var rgbMax = Math.max(rgb.r, Math.min(rgb.g, rgb.b));
                //var rgbMin = Math.min(rgb.r, Math.max(rgb.g, rgb.b));
                //var rgbMinplusMax = rgbMax + rgbMin;
                complementRgb.r = 255 - rgb.r;
                complementRgb.g = 255 - rgb.g;
                complementRgb.b = 255 - rgb.b;

                complementColor = "#" + complementRgb.r.toString(16) + complementRgb.g.toString(16) + complementRgb.b.toString(16);
            }
            var hourTime = hTime;
            var minuteTime = mTime;
            var theImage;
            // alert(nwSrc);
            canvas = Raphael("clockAreaInner", 200, 200);
            var clock = canvas.circle(100, 100, 95);
            if (imgPath != null) {
                theImage = canvas.image(imgPath, 0, 0, 200, 200);
                // theImage.attr({ "clip-rect": "0 0 200 200" });
            }


            clock.attr({ "fill-opacity": 0, "stroke": complementColor, "stroke-width": "5" })

            var hour_sign;
            for (i = 0; i < 12; i++) {
                var start_x = 100 + Math.round(80 * Math.cos(30 * i * Math.PI / 180));
                var start_y = 100 + Math.round(80 * Math.sin(30 * i * Math.PI / 180));
                var end_x = 100 + Math.round(90 * Math.cos(30 * i * Math.PI / 180));
                var end_y = 100 + Math.round(90 * Math.sin(30 * i * Math.PI / 180));
                if (IsDigits == true) {
                    hour_sign = canvas.path("M" + start_x + " " + start_y + "L" + end_x + " " + end_y);
                    hour_sign.attr({ stroke: complementColor, "stroke-width": 2 })
                }
            }
            hour_hand = canvas.path("M100 100L100 50");
            hour_hand.attr({ stroke: complementColor, "stroke-width": 6 });
            minute_hand = canvas.path("M100 100L100 40");
            minute_hand.attr({ stroke: complementColor, "stroke-width": 4 });
            //second_hand = canvas.path("M100 110L100 25");
            //second_hand.attr({ stroke: "#444444", "stroke-width": 2 });
            var pin = canvas.circle(100, 100, 5);
            pin.attr("fill", "#000000");
            update_clock(hTime, mTime)
            //setInterval("update_clock()", 1000);
        }

        function update_clock(hourTime, minuteTime) {
            var now = new Date();
            var hours = hourTime;
            var minutes = minuteTime;
            var seconds = 23;
            hour_hand.rotate(30 * hours + (minutes / 2.5), 100, 100);
            minute_hand.rotate(6 * minutes, 100, 100);
            //second_hand.rotate(6 * seconds, 100, 100);

        }

        function changeCheckDigit(id) {
            if (id.checked) {
                IsDigitsClock = true;
                minuteTime = $("#ddlMinute option:selected").val();
                hourTime = $("#ddlHour option:selected").val();
                $('.clockContainer').empty();
                draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
            }
            else {
                IsDigitsClock = false;
                minuteTime = $("#ddlMinute option:selected").val();
                hourTime = $("#ddlHour option:selected").val();
                $('.clockContainer').empty();
                draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
            }
        }

        function getAverageColourAsRGB(img) {
            var canvas = document.createElement('canvas');
            var context = canvas.getContext && canvas.getContext('2d');
            var rgb = { r: 102, g: 102, b: 102 };  // Set a base colour as a fallback for non-compliant browsers
            var pixelInterval = 5; // Rather than inspect every single pixel in the image inspect every 5th pixel
            var count = 0;
            var i = -4;
            var data, length;

            // return the base colour for non-compliant browsers
            if (!context) { return rgb; }

            // set the height and width of the canvas element to that of the image
            var height = canvas.height = img.naturalHeight || img.offsetHeight || img.height,
            width = canvas.width = img.naturalWidth || img.offsetWidth || img.width;

            context.drawImage(img, 0, 0)

            try {
                data = context.getImageData(0, 0, width, height);
            } catch (e) {
                // catch errors - usually due to cross domain security issues
                //  alert(e);
                return rgb;
            }

            data = data.data;
            length = data.length;
            while ((i += pixelInterval * 4) < length) {
                count++;
                rgb.r += data[i];
                rgb.g += data[i + 1];
                rgb.b += data[i + 2];
            }

            // floor the average values to give correct rgb values (ie: round number values)
            rgb.r = Math.floor(rgb.r / count);
            rgb.g = Math.floor(rgb.g / count);
            rgb.b = Math.floor(rgb.b / count);

            return rgb;
        }
 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
    </script>

</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
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
                    <li class="box4">
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
                    <img src="../Administration/images/student-logo.jpg" class="dash-logo">

                    <a href="homePage.aspx" class="homecls" title="Home"></a>
                    <a href="LessonManagement.aspx" class="lessnClass" title="Lesson Management"></a>
                    <a href="repository-manag.aspx" class="repocls" title="Repository Management"></a>
                </div>
                <div class="clear">
                </div>

            </div>
            <br />
            <!-- content panel -->
            <div id="dashboard-RHSVis">

                <h2 class="cokin">Time

                        <span id="td_LP" runat="server"></span>
                </h2>
                <div class="clear"></div>
                <hr />
                <div style="background: #fff; padding: 10px; margin: 10px;">


                    <%--MY Work Area
                    --%>

                    <div style="float: left; margin-left: 30px; padding-right: 2px;">
                        <tfoot>
                        </tfoot>
                    </div>
                    <div class="clear"></div>

                    <div id="fullDivision">
                        <div class="clear"></div>

                        <table style="width: 95%; margin: 0 auto;">
                            <tr>
                                <td rowspan="4" style="vertical-align: top;">
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
                                    <div id="addDist" style="" title="Add Distractor" class="with-tip">
                                        <div class="btn btn-green5"></div>
                                    </div>



                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; vertical-align: top;" rowspan="2">
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
                                                                <asp:UpdateProgress runat="server" ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="100">
                                                                    <ProgressTemplate>
                                                                        <div class="waitBlank" style="width: 100px; height: 207px;">
                                                                            <div style="margin: 75% auto auto 33%;">
                                                                                <asp:Image ID="Image2" runat="server" src="icons/wait(s).gif" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>

                                                                </asp:UpdateProgress>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="width: 95%;">
                                                                            <asp:ListBox ID="lstSets" runat="server" Height="105px" Width="90%" AutoPostBack="True" OnSelectedIndexChanged="lstSets_SelectedIndexChanged1"></asp:ListBox>
                                                                        </td>
                                                                        <td style="width: 5%; vertical-align: top;">
                                                                            <div class="arrowBox">
                                                                                <div class="upArrow">
                                                                                    <asp:ImageButton ID="btn_setUp" runat="server" ImageUrl="icons/arrow_up.png" OnClick="btn_setUp_Click" />
                                                                                </div>
                                                                                <div class="downArrow">
                                                                                    <asp:ImageButton ID="btn_setDown" ImageUrl="~/VisualTool/icons/arrow_down.png" runat="server" OnClick="btn_setDown_Click" />
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
                                                                    <td style="text-align: center;">

                                                                        <div class="btn btn-red" style="margin-left: 25px; float: left;">
                                                                            <asp:ImageButton ID="imgBtn_setDelete" ImageUrl="~/VisualTool/images/DeleteSetLight.JPG" runat="server" OnClick="imgBtn_setDelete_Click" />
                                                                        </div>

                                                                    </td>
                                                                    <td style="text-align: center;">
                                                                        <div class="btn btn-green" style="margin-left: 25px; float: left;">
                                                                            <asp:ImageButton ID="imgBtn_AddSet" ImageUrl="~/VisualTool/images/AddSetLight.JPG" runat="server" OnClick="imgBtn_AddSet_Click" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <span class="spanStep">STEPS</span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <div class="stepDiv">

                                                                            <div class="renameAreaStep" style="display: none;">
                                                                                <div class="waitBlank rename" style="width: 110px; height: 207px; display: block; opacity: .8;">
                                                                                </div>
                                                                                <div class="renameDiv">

                                                                                    <input type="text" class="renameText renameStepText" style="width: 100px;" onblur='stripIt(this);' /><br />
                                                                                    <input type="button" class="renameStepOk" value="DONE" />
                                                                                    <input type="button" class="renameStepNo" value="CANCEL" />
                                                                                </div>
                                                                            </div>
                                                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="100">
                                                                                <ProgressTemplate>
                                                                                    <div class="waitBlank" style="width: 100px; height: 202px;">
                                                                                        <div style="margin: 75% auto auto 33%;">
                                                                                            <asp:Image ID="Image3" runat="server" src="icons/wait(s).gif" />
                                                                                        </div>
                                                                                    </div>

                                                                                </ProgressTemplate>
                                                                            </asp:UpdateProgress>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td style="width: 95%;">
                                                                                        <asp:ListBox ID="lstSteps" runat="server" Height="105px" Width="90%" AutoPostBack="True" OnSelectedIndexChanged="lstSteps_SelectedIndexChanged"></asp:ListBox>
                                                                                    </td>
                                                                                    <td style="width: 5%; vertical-align: top;">
                                                                                        <div class="arrowBox">
                                                                                            <div class="upArrow">
                                                                                                <asp:ImageButton ID="btn_stepUp" runat="server" ImageUrl="icons/arrow_up.png" OnClick="btn_stepUp_Click" />
                                                                                            </div>
                                                                                            <div class="downArrow">
                                                                                                <asp:ImageButton ID="btn_stepDown" ImageUrl="~/VisualTool/icons/arrow_down.png" runat="server" OnClick="btn_stepDown_Click" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td style="text-align: center;">

                                                                                                    <div class="btn btn-red" style="margin-left: 25px; float: left;">
                                                                                                        <asp:ImageButton ID="imgBtn_stepDelete" ImageUrl="~/VisualTool/images/DeleteSetLight.JPG" runat="server" OnClick="imgBtn_stepDelete_Click" />
                                                                                                    </div>

                                                                                                </td>
                                                                                                <td style="text-align: center;">
                                                                                                    <div class="btn btn-green" style="margin-left: 25px; float: left;">
                                                                                                        <asp:ImageButton ID="imgBtn_AddStep" ImageUrl="~/VisualTool/images/AddSetLight.JPG" runat="server" OnClick="imgBtn_AddStep_Click" />
                                                                                                    </div>
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
                                                        </td>

                                                    </tr>

                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td style="width: 70%; vertical-align: top;">
                                    <div id="clockArea" style="text-align: center;">
                                        <table style="width: 70%; margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    
                                                    <div id="clockAreaInner" style="text-align: center;" class="clockContainer">
                                                    </div>
                                                    <div style="text-align:center;"><span class="removeImage">Remove Image</span></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 40%; text-align: right; padding: 5px 0 0 0;" class="tdText1">
                                                                <input id="chkDigit" align="middle" class="chkIsDigit" type="checkbox"
                                                                    value="Disable Digits" onchange="changeCheckDigit(this);"
                                                                    checked="checked">Enable Digits</input></td>
                                                            <td style="width: 60%; text-align: left">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="width: 15%; text-align: right;">
                                                                            <asp:DropDownList ID="ddlHour" runat="server" class="clsdropHour">
                                                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                                                <asp:ListItem Value="1">01</asp:ListItem>
                                                                                <asp:ListItem Value="2">02</asp:ListItem>
                                                                                <asp:ListItem Value="3">03</asp:ListItem>
                                                                                <asp:ListItem Value="4">04</asp:ListItem>
                                                                                <asp:ListItem Value="5">05</asp:ListItem>
                                                                                <asp:ListItem Value="6">06</asp:ListItem>
                                                                                <asp:ListItem Value="7">07</asp:ListItem>
                                                                                <asp:ListItem Value="8">08</asp:ListItem>
                                                                                <asp:ListItem Value="9">09</asp:ListItem>
                                                                                <asp:ListItem>10</asp:ListItem>
                                                                                <asp:ListItem>11</asp:ListItem>
                                                                                <asp:ListItem>12</asp:ListItem>

                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 15%; text-align: left;">
                                                                            <asp:DropDownList ID="ddlMinute" runat="server" class="clsdropMinute">
                                                                                <asp:ListItem Value="00">00</asp:ListItem>
                                                                                <asp:ListItem Value="05">05</asp:ListItem>
                                                                                <asp:ListItem>10</asp:ListItem>
                                                                                <asp:ListItem>15</asp:ListItem>
                                                                                <asp:ListItem>20</asp:ListItem>
                                                                                <asp:ListItem>25</asp:ListItem>
                                                                                <asp:ListItem>30</asp:ListItem>
                                                                                <asp:ListItem>35</asp:ListItem>
                                                                                <asp:ListItem>40</asp:ListItem>
                                                                                <asp:ListItem>45</asp:ListItem>
                                                                                <asp:ListItem>50</asp:ListItem>
                                                                                <asp:ListItem>55</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 70%; text-align: center;">&nbsp;</td>
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
                                <td style="width: 20%; vertical-align: top;" rowspan="2">
                                    <div id="clockImageArea nomarg">
                                        <asp:DataList ID="dlImages" runat="server" Width="20%" HorizontalAlign="Left" RepeatColumns="4" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <table style="text-align: left; width: 100%;">
                                                    <tr>
                                                        <td style="width: 10%">
                                                            <div class="dl_images">
                                                                <asp:Image ID="image" runat="server" ImageUrl='<%# Eval("Path") %>' Height="75px" Width="75px" class="draggable" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 65%; vertical-align: central;">

                                    <div id="answerContainer">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <span class="tdTextLarge">What is the time now?</span></td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 100%;">
                                                <div id="distractorArea">
                                                    <div id="distBox" class="clsDistractor">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>

                        <div class="messageRibbon">
                            <div class="innerMsgRibbon">HEO</div>
                        </div>


                        <div id="loadingOverlay">
                            <div id="loading">
                                <div>Please Wait</div>
                                <img src="icons/Loading.gif" style="height: 50px; width: 50px;" />

                            </div>
                        </div>

                        <div id="previewBoardContainer" style="width: 100%; height: 100%;">
                            <div id="previewBoard" class="web_dialog" style="top: 8%; left: 15%;">

                                <div id="sign_up5">
                                    <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                        <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                                    <h3>Lesson Profile
               
                                    </h3>
                                    <hr />
                                    <iframe class="iframeCls" id="previewFrame" style="width: 100%" scrolling="auto"></iframe>
                                    <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                                </div>
                                <div id="previewClose"></div>

                            </div>

                        </div>


                        <div id="prevBoxContainer" style="width: 100%; height: 100%;">
                            <div id="previewBox" class="web_dialog" style="top: 5%; left: 5%; display: none;">
                                <div id="signupNew">
                                    <a id="closeNew" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                        <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300"
                                            width="18" height="18" alt="" /></a>
                                    <h3>Time Editor Preview
                                    </h3>
                                    <hr />
                                    <iframe id="prevboxFrame" style="width: 100%; height: 550px;"></iframe>
                                </div>
                            </div>
                        </div>


                        <%--<div id="previewBoardContainer" style="left: 241px; width: 905px;">
                                <div id="previewBoard" style="width: 920px; left: 229px; margin-top: 60px;">
                                    <iframe id="previewFrame" style="width: 100%" scrolling="auto"></iframe>
                                    <input id="btn_previewOk" type="button" value="Ok" />
                                </div>
                                <div id="previewClose"></div>

                            </div>--%>

                        <div class="fullOverlay">
                        </div>


                    </div>




                </div>
                <div class="clear"></div>
            </div>
        </div>
     
            </table>
        </div>

        </div>
            <!-- footer -->
        <div id="footer-panel">
            <ul style="text-align: center;">
                <li>&copy; Copyright 2015, Melmark, Inc. All rights reserved.</li>
            </ul>
        </div>
        </div>
    </form>
</body>
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

        makeAllDraggable();
        makeAllDroppable();

        var saveData = "false";
        var edit = GetQueryStringParams('edit');

        if (edit == '1') {

            $('#loadingOverlay').show();
            $('#loading').show();

            // alert('EditMode');
            var setValue = $('#lstSets').val();
            var stepValue = $('#lstSteps').val();
            var indexId = 1;
            var imgSrc = '#f5f5f5'
            var hourTime = 12
            var minuteTime = 00
            var arryData = [];
            var clockBg = "";
            draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
            var newDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong">' +
                '<img src="images/enable-icon.png" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td></td></tr>' +
                '<tr><td><span class="AnswerHour">' + hourTime + '</span><span class="fined">:</span><span class="AnswerMinute">00</span></td></tr></table>' +
                '</div></div><div class="tempL"><span class="leOptId"></span></div></div>'

            $('#distractorArea').append(newDivDistractor);


            $.ajax({
                url: "time_editorPage.aspx/GetClockImageSrc",
                data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    var clockData = data.d;

                    var splitData = clockData.split('^');
                    clockBaground = splitData[0];
                    //IsDigitsClock = splitData[1];
                    if (splitData[1] == 'true' || splitData[1] == "") {
                        IsDigitsClock = true;
                    }
                    else {
                        IsDigitsClock = false;
                    }
                    if (IsDigitsClock == true) {
                        //document.getElementById("chkDigit").checked = true;
                        $('#chkDigit').attr('checked', true);
                    }
                    else {
                        $('#chkDigit').attr('checked', false);
                    }

                    $.ajax({
                        url: "time_editorPage.aspx/GetElementData",
                        data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            var contents = data.d;


                            if (contents != "") {
                                //   alert(contents);
                                $('#distractorArea').empty();

                            }
                            var content_Array = contents.split('@');

                            for (indI = 0; indI < content_Array.length - 1; indI++) {

                                var contentSub_array = content_Array[indI].split('^');

                                for (indJ = 0; indJ < contentSub_array.length - 1; indJ++) {
                                    statusValue = contentSub_array[1];
                                    // alert(status);
                                    //alert('hour+min:' + contentSub_array[0] + "// status: " + contentSub_array[1]);
                                    var content_hour = contentSub_array[0].split(',');

                                    hourValue = content_hour[0];
                                    minuteValue = content_hour[1];
                                    //  alert(statusValue);
                                    // alert(hourValue);
                                    //  alert(minuteValue);

                                    if (statusValue == "C") {
                                        var imgSrc = '#f5f5f5'
                                        var rightDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong"><img src="images/enable-icon.png" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td></td></tr><tr><td><span class="AnswerHour">' + hourValue + '</span><span class="fined">:</span><span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span></div></div>'

                                        $('#distractorArea').append(rightDivDistractor);
                                        //$('.clsDropHourAnswer').val(hourTime);
                                        // $('.clsDropMinuteAnswer').val(minuteValue);
                                        $('#selctHourdropList0').val(hourValue);
                                        $('#selctMindropList0').val(minuteValue);
                                        $('.clockContainer').empty();
                                        draw_clock(clockBaground, hourValue, minuteValue, IsDigitsClock);
                                        $('#ddlHour').val(hourValue);
                                        $('#ddlMinute').val(minuteValue);
                                        // $('.clockContainer').empty();
                                        // draw_clock(imgSrc, hourValue, minuteValue);

                                    }
                                    else {
                                        var wrongDivSistractor = '<div class="distTemplateSmall"><div class="statusDivSmall">' +
                                            '<div class="status-btn wrong" ><img src="images/deletnwblk.png" height = "15px" width = "15px" ' +
                                            'onclick = "deleteItem(this)" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td>' +
                                            '<select id="selctHourdropList' + indI + '" class = "clsDropHour"><option value ="12">12</option><option value ="1">01</option>' +
                                            '<option value ="2">02</option><option value ="3">03</option><option value ="4">04</option><option value ="5">05</option>' +
                                            '<option value ="6">06</option><option value ="7">07</option><option value ="8">08</option><option value ="9">09</option>' +
                                            '<option value ="10">10</option><option value ="11">11</option></select></td></tr><tr><td>' +
                                            '<select id="selctMindropList' + indI + '" class = "clsDropMinute" ><option value ="00">00</option><option value ="05">05</option>' +
                                            '<option value ="10">10</option><option value ="15">15</option><option value ="20">20</option><option value ="25">25</option>' +
                                            '<option value ="30">30</option><option value ="35">35</option><option value ="40">40</option><option value ="45">45</option>' +
                                            '<option value ="50">50</option><option value ="55">55</option></select> </td></tr></table></div></div><div class="tempL">' +
                                            '<span class="leOptId"></span></div></div>';

                                        $('#distractorArea').append(wrongDivSistractor);
                                        // $('.clsDropHour').val(hourTime);
                                        // $('.clsDropMinute').val(minuteValue);
                                        $("#selctHourdropList" + indI).val(hourValue);
                                        $("#selctMindropList" + indI).val(minuteValue);
                                        indexId++;

                                    }

                                }
                            }

                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


            $('#loadingOverlay').hide();
            $('#loading').hide();

        }
        else {
            //  alert('NormalMode');
            var saveData = false;
            var imgSrc = '#f5f5f5'
            var hourTime = 12
            var minuteTime = 00
            var index = 1;
            var arryData = [];
            var dropHour = $('#ddlHour').clone();



            draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
            var newDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong">' +
                '<img src="images/enable-icon.png" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td></td></tr>' +
                '<tr><td><span class="AnswerHour">' + hourTime + '</span><span class="fined">:</span><span class="AnswerMinute">00</span></td></tr></table>' +
                '</div></div><div class="tempL"><span class="leOptId"></span></div></div>'

            $('#distractorArea').append(newDivDistractor);
            $('.clsDropHourAnswer').val(hourTime);
            $('.clsDropMinuteAnswer').val(minuteTime);
        }

        $('.draggable').click(function () {

            var img = $(this).attr('src');
            clockBaground = img;
            minuteTime = $("#ddlMinute option:selected").val();
            hourTime = $("#ddlHour option:selected").val();
            $('.clockContainer').empty();
            draw_clock(img, hourTime, minuteTime, IsDigitsClock);

        });

        $('.removeImage').click(function () {
            clockBaground = '';
            minuteTime = $("#ddlMinute option:selected").val();
            hourTime = $("#ddlHour option:selected").val();
            $('.clockContainer').empty();
            draw_clock('', hourTime, minuteTime, IsDigitsClock);
        });

        $('#ddlHour').change(function () {
            hourTime = $("#ddlHour option:selected").val();
            minuteTime = $("#ddlMinute option:selected").val();
            $('.clockContainer').empty();
            draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
            $('.AnswerHour').html(hourTime);
            //  $('.clsDropHourAnswer').val(hourTime);
            //alert(hourTime);
        });
        $('#ddlMinute').change(function () {
            hourTime = $("#ddlHour option:selected").val();
            minuteTime = $("#ddlMinute option:selected").val();
            $('.clockContainer').empty();
            draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
            $('.AnswerMinute').html(minuteTime);
            // $('.clsDropMinuteAnswer').val(minuteTime);
            // alert(minuteTime);
        });
        $('.clsDropHourAnswer').change(function () {
            //alert('asdasd');
            hourTime = $("#selctHourdropList0 option:selected").val();
            $('#ddlHour').val(hourTime);
            $('.clockContainer').empty();
            draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
        });
        $('.clsDropMinuteAnswer').change(function () {
            minuteTime = $("#selctMindropList0 option:selected").val();
            $('#ddlMinute').val(minuteTime);
            $('.clockContainer').empty();
            draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
        });

        $('#addDist').click(function () {
            newDivDistractor = '<div class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong" >' +
                '<img src="images/deletnwblk.png" height = "15px" width = "15px" onclick = "deleteItem(this)" /></div></div><div class="tempB"><div class ="datacontent">' +
                '<table style = "width:100%;"><tr><td><select id="selctHourdropList' + index + '" class = "clsDropHour"><option value ="12">12</option>' +
                '<option value ="1">01</option><option value ="2">02</option><option value ="3">03</option><option value ="4">04</option><option value ="5">05</option>' +
                '<option value ="6">06</option><option value ="7">07</option><option value ="8">08</option><option value ="9">09</option><option value ="10">10</option>' +
                '<option value ="11">11</option></select></td></tr><tr><td><select id="selctMindropList' + index + '" class = "clsDropMinute" ><option value ="00">00</option>' +
                '<option value ="05">05</option><option value ="10">10</option><option value ="15">15</option><option value ="20">20</option><option value ="25">25</option>' +
                '<option value ="30">30</option><option value ="35">35</option><option value ="40">40</option><option value ="45">45</option><option value ="50">50</option>' +
                '<option value ="55">55</option></select> </td></tr></table></div></div><div class="tempL"><span class="leOptId"></span></div></div>';

            $('#distractorArea').append(newDivDistractor);
            index++;

        });

        $('#btn_profile').click(function () {

            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                //document.getElementById('previewFrame').style.height = 1000;
                $('#previewFrame').attr('src', 'profilePreview.aspx');
                //  $('#previewBoard').css('display', 'block');
                $('#previewBoard').fadeIn();
                //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
                //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
            });
        });




        $('#btn_preview').click(function () {

            var setValue = $("#lstSets option:selected").val();
            var stepValue = $("#lstSteps option:selected").val();

            //    alert(setValue);
            //  alert(stepValue);

            //var speedvalue = $('#slider-vertical').slider('option', 'value');


            $('.fullOverlay').empty();

            $('.fullOverlay').fadeIn('slow', function () {
                //showHtml();               
                $('#prevboxFrame').attr('src', 'PreviewPage_timeEditor.aspx?setNumber=' + setValue + '&stepNumber=' + stepValue);
                $('#previewBox').fadeIn('fast');
                $('#textEditorDiv').hide();
                //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
            });



            //$('#btn_preview').click(function () {

            //    var setValue = $("#lstSets option:selected").val();
            //    var stepValue = $("#lstSteps option:selected").val();
            //    //var speedvalue = $('#slider-vertical').slider('option', 'value');
            //    $('.fullOverlay').empty();

            //    $('.fullOverlay').fadeIn('fast', function () {
            //        //showHtml();
            //        document.getElementById('previewFrame').style.height = 5000;
            //        $('#previewFrame').attr('src', 'PreviewPage_timeEditor.aspx?setNumber=' + setValue + '&stepNumber=' + stepValue);
            //        $('#previewBoard').fadeIn('fast');
            //        $('#textEditorDiv').hide();
            //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
            //    });

            //else {
            //    if (confirm('File not saved... Save now?')) {
            //        saveFn();
            //    }
            //}

        });

        $('#btn_previewOk').click(function () {

            $('#previewBoard').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
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

        $('#btn_close').click(function () {
            if (confirm('Any unsaved changes will be discarded...\nSure you want to close?')) {
                window.location = 'LessonManagement.aspx';
            }
        });


        $('#btn_finish').click(function () {
            var hour = "";
            var minute = "";
            var status = "C";
            var setValue = $("#lstSets option:selected").val();
            var stepValue = $("#lstSteps option:selected").val();
            var contents = "";
            var noDistBox = $('#distractorArea').find('.distTemplateSmall');
            hour = $('.AnswerHour').html();
            minute = $('.AnswerMinute').html();
            var hAnswer = hour;
            var hMinute = minute;
            contents = hour + ',' + minute + '^' + status + '@';
            //  alert(noDistBox.length);
            // alert(hour);
            //  alert(minute);
            //   alert(status);
            for (var i = 1; i < noDistBox.length; i++) {

                hour = $(noDistBox[i]).find(".clsDropHour").val();
                minute = $(noDistBox[i]).find(".clsDropMinute").val();

                if ((hour == hAnswer) && (minute == hMinute)) {
                }
                else {

                    status = "W";

                    contents = contents + (hour + ',' + minute + '^' + status + '@');
                }


            }
            $.ajax({
                url: "time_editorPage.aspx/SaveData",
                data: "{ 'contents':'" + contents + "','setNumber':'" + setValue + "','stepNumber':'" + stepValue + "','clockBg':'" + clockBaground + "','isDigit':'" + IsDigitsClock + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    saveData = true;
                    $('.innerMsgRibbon').text('Saved');
                    $('.messageRibbon').show().fadeOut(2000);


                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    saveData = false;
                    alert(textStatus);
                }
            });

        });
    });
</script>
<script type="text/javascript">
    function deleteItem(e) {
        if (confirm('Do you want to delete?')) {
            $(e).parents('div.distTemplateSmall').remove();
        }
    }
    function setIndexSelectFunction() {
        $('#distractorArea').empty();
        hourTime = $("#ddlHour option:selected").val();
        minuteTime = $("#ddlMinute option:selected").val();
        var newDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong"><img src="images/enable-icon.png" /></div>' +
            '</div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td></td></tr><tr><td><span class="AnswerHour">' + hourTime + '</span>' +
            '<span class="fined">:</span><span class="AnswerMinute">' + minuteTime + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span></div></div>'

        $('#distractorArea').append(newDivDistractor);

        // draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
        // $('#selctHourdropList0').val(hourTime);
        // $('#selctMindropList0').val(minuteTime);
        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
        var statusValue = "";
        var hourValue = "";
        var minuteValue = "";
        var indexId = 1;

        $.ajax({
            url: "time_editorPage.aspx/GetClockImageSrc",
            data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var clockData = data.d;

                var splitData = clockData.split('^');
                clockBaground = splitData[0];
                //  alert(clockBaground);
                if (splitData[1] == 'true') {
                    IsDigitsClock = true;
                }
                else {
                    IsDigitsClock = false;
                }
                if (IsDigitsClock == true) {
                    //document.getElementById("chkDigit").checked = true;
                    $('#chkDigit').attr('checked', true);
                }
                else {
                    $('#chkDigit').attr('checked', false);
                }

                $.ajax({
                    url: "time_editorPage.aspx/GetElementData",
                    data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        var contents = data.d;


                        if (contents != "") {
                            //alert(contents);
                            $('#distractorArea').empty();
                        }
                        var content_Array = contents.split('@');

                        for (indI = 0; indI < content_Array.length - 1; indI++) {

                            var contentSub_array = content_Array[indI].split('^');

                            for (indJ = 0; indJ < contentSub_array.length - 1; indJ++) {
                                statusValue = contentSub_array[1];
                                // alert(status);
                                //alert('hour+min:' + contentSub_array[0] + "// status: " + contentSub_array[1]);
                                var content_hour = contentSub_array[0].split(',');

                                hourValue = content_hour[0];
                                minuteValue = content_hour[1];
                                // alert(statusValue);
                                //  alert(hourValue);
                                //  alert(minuteValue);

                                if (statusValue == "C") {
                                    var imgSrc = '#f5f5f5';
                                    var rightDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall">' +
                                        '<div class="status-btn wrong"><img src="images/enable-icon.png" /></div></div><div class="tempB">' +
                                        '<div class ="datacontent"><table style = "width:100%;"><tr><td><span class="AnswerHour">' + hourValue + '</span>' +
                                        '<span class="fined">:</span><span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div>' +
                                        '<div class="tempL"><span class="leOptId"></span></div></div>'

                                    $('#distractorArea').append(rightDivDistractor);
                                    //$('.clsDropHourAnswer').val(hourTime);
                                    // $('.clsDropMinuteAnswer').val(minuteValue);
                                    // $('#selctHourdropList0').val(hourValue);
                                    // $('#selctMindropList0').val(minuteValue);
                                    $('.clockContainer').empty();
                                    draw_clock(clockBaground, hourValue, minuteValue, IsDigitsClock);
                                    $('#ddlHour').val(hourValue);
                                    $('#ddlMinute').val(minuteValue);
                                    //  $('.clockContainer').empty();
                                    //  draw_clock(imgSrc, hourValue, minuteValue);

                                }
                                else {
                                    var wrongDivSistractor = '<div class="distTemplateSmall"><div class="statusDivSmall">' +
                                    '<div class="status-btn wrong"><img src="images/deletnwblk.png" height = "15px" width = "15px"' +
                                    ' onclick = "deleteItem(this)" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td>' +
                                    '<select id="selctHourdropList' + indI + '" class = "clsDropHour"><option value ="12">12</option><option value ="1">01</option>' +
                                    '<option value ="2">02</option><option value ="3">03</option><option value ="4">04</option><option value ="5">05</option>' +
                                    '<option value ="6">06</option><option value ="7">07</option><option value ="8">08</option><option value ="9">09</option>' +
                                    '<option value ="10">10</option><option value ="11">11</option></select></td></tr><tr><td>' +
                                    '<select id="selctMindropList' + indI + '" class = "clsDropMinute" ><option value ="00">00</option><option value ="05">05</option>' +
                                    '<option value ="10">10</option><option value ="15">15</option><option value ="20">20</option><option value ="25">25</option>' +
                                    '<option value ="30">30</option><option value ="35">35</option><option value ="40">40</option><option value ="45">45</option>' +
                                    '<option value ="50">50</option><option value ="55">55</option></select> </td></tr></table></div></div><div class="tempL">' +
                                    '<span class="leOptId"></span></div></div>';

                                    $('#distractorArea').append(wrongDivSistractor);
                                    // $('.clsDropHour').val(hourTime);
                                    // $('.clsDropMinute').val(minuteValue);
                                    $("#selctHourdropList" + indI).val(hourValue);
                                    $("#selctMindropList" + indI).val(minuteValue);
                                    indexId++;
                                }

                            }
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    }
    function stepIndexSelectFunction() {
        $('#distractorArea').empty();
        hourTime = $("#ddlHour option:selected").val();
        minuteTime = $("#ddlMinute option:selected").val();
        var newDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong"><img src="images/enable-icon.png" /></div>' +
            '</div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td></td></tr><tr><td><span class="AnswerHour">' + hourTime + '</span>' +
            '<span class="fined">:</span><span class="AnswerMinute">' + minuteTime + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span>' +
            '</div></div>'

        $('#distractorArea').append(newDivDistractor);

        //$('#selctHourdropList0').val(hourTime);
        //$('#selctMindropList0').val(minuteTime);
        var setValue = $("#lstSets option:selected").val();
        var stepValue = $("#lstSteps option:selected").val();
        var statusValue = "";
        var hourValue = "";
        var minuteValue = "";
        var indexId = 1;


        $.ajax({
            url: "time_editorPage.aspx/GetClockImageSrc",
            data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var clockData = data.d;

                var splitData = clockData.split('^');
                clockBaground = splitData[0];
                // alert(clockBaground);
                if (splitData[1] == 'true') {
                    IsDigitsClock = true;
                }
                else {
                    IsDigitsClock = false;
                }
                if (IsDigitsClock == true) {
                    //document.getElementById("chkDigit").checked = true;
                    $('#chkDigit').attr('checked', true);
                }
                else {
                    $('#chkDigit').attr('checked', false);
                }


                $.ajax({
                    url: "time_editorPage.aspx/GetElementData",
                    data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        var contents = data.d;


                        if (contents != "") {
                            //   alert(contents);
                            $('#distractorArea').empty();
                        }
                        var content_Array = contents.split('@');

                        for (indI = 0; indI < content_Array.length - 1; indI++) {

                            var contentSub_array = content_Array[indI].split('^');

                            for (indJ = 0; indJ < contentSub_array.length - 1; indJ++) {
                                statusValue = contentSub_array[1];
                                // alert(status);
                                //alert('hour+min:' + contentSub_array[0] + "// status: " + contentSub_array[1]);
                                var content_hour = contentSub_array[0].split(',');

                                hourValue = content_hour[0];
                                minuteValue = content_hour[1];
                                //  alert(statusValue);
                                // alert(hourValue);
                                //  alert(minuteValue);

                                if (statusValue == "C") {
                                    var imgSrc = '#f5f5f5'
                                    var rightDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong">' +
                                        '<img src="images/enable-icon.png" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr>' +
                                        '<td></td></tr><tr><td><span class="AnswerHour">' + hourValue + '</span><span class="fined">:</span>' +
                                        '<span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span></div></div>'

                                    $('#distractorArea').append(rightDivDistractor);
                                    //$('.clsDropHourAnswer').val(hourTime);
                                    // $('.clsDropMinuteAnswer').val(minuteValue);
                                    $('#selctHourdropList0').val(hourValue);
                                    $('#selctMindropList0').val(minuteValue);
                                    $('.clockContainer').empty();
                                    draw_clock(clockBaground, hourValue, minuteValue, IsDigitsClock);
                                    $('#ddlHour').val(hourValue);
                                    $('#ddlMinute').val(minuteValue);
                                    // $('.clockContainer').empty();
                                    // draw_clock(imgSrc, hourValue, minuteValue);

                                }
                                else {
                                    var wrongDivSistractor = '<div class="distTemplateSmall"><div class="statusDivSmall">' +
                                        '<div class="status-btn wrong" ><img src="images/deletnwblk.png" height = "15px" width = "15px"' +
                                        ' onclick = "deleteItem(this)" /></div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr>' +
                                        '<td><select id="selctHourdropList' + indI + '" class = "clsDropHour"><option value ="12">12</option>' +
                                        '<option value ="1">01</option><option value ="2">02</option><option value ="3">03</option><option value ="4">04</option>' +
                                        '<option value ="5">05</option><option value ="6">06</option><option value ="7">07</option><option value ="8">08</option>' +
                                        '<option value ="9">09</option><option value ="10">10</option><option value ="11">11</option></select></td></tr><tr><td>' +
                                        '<select id="selctMindropList' + indI + '" class = "clsDropMinute" ><option value ="00">00</option><option value ="05">05</option>' +
                                        '<option value ="10">10</option><option value ="15">15</option><option value ="20">20</option><option value ="25">25</option>' +
                                        '<option value ="30">30</option><option value ="35">35</option><option value ="40">40</option><option value ="45">45</option>' +
                                        '<option value ="50">50</option><option value ="55">55</option></select> </td></tr></table></div></div><div class="tempL">' +
                                        '<span class="leOptId"></span></div></div>';

                                    $('#distractorArea').append(wrongDivSistractor);
                                    // $('.clsDropHour').val(hourTime);
                                    // $('.clsDropMinute').val(minuteValue);
                                    $("#selctHourdropList" + indI).val(hourValue);
                                    $("#selctMindropList" + indI).val(minuteValue);
                                    indexId++;

                                }

                            }
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });

    }
    function imgAddStepClick() {

        $('.clockContainer').empty();
        $('#distractorArea').empty();
        hourTime = $("#ddlHour option:selected").val();
        minuteTime = $("#ddlMinute option:selected").val();
        IsDigitsClock = true;
        clockBaground = "";
        draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
        var newDivDistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong"><img src="images/enable-icon.png" />' +
            '</div></div><div class="tempB"><div class ="datacontent"><table style = "width:100%;"><tr><td></td></tr><tr><td><span class="AnswerHour">' + hourTime + '</span>' +
            '<span class="fined">:</span><span class="AnswerMinute">' + minuteTime + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span>' +
            '</div></div>'

        $('#distractorArea').append(newDivDistractor);

        //  alert(hourTime);
        //  alert(minuteTime);
        // $('#selctHourdropList0').val(hourTime);
        //$('#selctMindropList0').val(minuteTime);



    }

    function deleteFun() {
        alert('No deletion posible in this criteria. it needs minimum one set and one step !!! ');
    }

    function swapFun() {
        alert('No Swapping posible in this criteria. !!! ');
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
</html>
