<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeachPage_timeEditor.aspx.cs" Inherits="TeachPage_timeEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />





    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="styles/commonStyle.css" rel="stylesheet" />

    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="scripts/ui/jquery.ui.all.css" rel="stylesheet" type="text/css" />

    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" type="text/css" />


    <link href="styles/timeLesson.css" rel="stylesheet" />

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>
    <script type="text/javascript" src="scripts/anologClock.js"></script>

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="scripts/StopWatchNew.js"></script>


    <style type="text/css">
        .web_dialog {
            display: none;
            position: fixed;
            /*width: 1040px;
            height: 550px;*/
            width: 100%;
            height: 100%;
            overflow: auto;
            /*top: 50%;
            left: 50%;
            margin-left: -520px;
            margin-top: -275px;*/
            font-size: 100%;
            font: 13px/20px "Helvetica Neue", Helvetica, Arial, sans-serif;
            color: White;
            z-index: 102;
            background: #FFFFFF;
            top: 0px;
            left: 0px;
            /*-moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border: 1px solid #536376;*/
        }
    </style>

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
            canvas = Raphael("clockDivTeach", 200, 200);
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
                //alert(e);
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

    </script>




    <style type="text/css">
        .reinforcement {
            height: 501px;
            left: 18px;
            position: absolute;
            top: 90px;
            width: 100%;
            z-index: 1000;
            display: none;
        }

        .AnswerHour {
            font-size: 30px;
        }

        .fined {
            font-size: 30px;
        }

        .AnswerMinute {
            font-size: 30px;
        }
    </style>
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>

                    <li class="user" style="width: 87%;">
                        <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
                    </li>





                </ul>






            </div>
        </div>

        <!-- dashboard container panel -->
        <div id="header-panel">
            <div class="Dashboard-logo">
                <img src="../Administration/images/student-logo.jpg">
            </div>
            <div class="clear">
            </div>

        </div>
        <br />

        <!-- content panel -->
        <div id="dashboard-content-panel">
            <!-- content Left side -->


            <!-- content right side -->
            <div id="dashboard-RHS">

                <div class="dashboard-RHS-content">


                    <div class="previewAreaTeach">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <div class="headingRow">
                                        <span class="clockHeadingSpan">Clock Test</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; text-align: center;">

                                    <div id="clockDivTeach" class="clockContainerPreviewTeach">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="questionAskTeach">
                                        <span class="tdTextLarge" style="vertical-align: central">What is the time now?</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;">
                                    <div id="distractorDivTeach" class="distractorAreaPreview">
                                    </div>

                                </td>
                            </tr>
                        </table>
                    </div>



                </div>

            </div>
            <div id="div_Correct" class="web_dialog">
                <%--<button type="button" id="btn_closeCrct" onclick="javascript:HideDialog('correct');">Close</button>--%>
                <a id="close_x" class="close sprited" href="#" onclick="javascript:HideDialog();">
                    <img src="../Administration/images/clb.PNG" style="border: 0px; float: right; padding: 5px" width="25" alt="" />
                </a>
                <iframe id="iframeCorrect" width="100%" height="100%" frameborder="1"></iframe>
            </div>
            <div id="div_Wrong" class="web_dialog">
                <%--<button type="button" id="btn_closeWrng" onclick="javascript:HideDialog('wrong');">Close</button>--%>
                <a id="close" class="close sprited" href="#" onclick="javascript:HideDialog();">
                    <img src="../Administration/images/clb.PNG" style="border: 0px; float: right; padding: 5px" width="25" alt="" />
                </a>
                <iframe id="iframeWrong" width="100%" height="100%" frameborder="1"></iframe>
            </div>
             <asp:HiddenField ID="corrReinTemp" runat="server" />
                                        <asp:HiddenField ID="wrongReinTemp" runat="server" />

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



<script type="text/javascript">
    var _Stopwatch = new StopWatch();
    //_Stopwatch.start();
    var noOftries = 1;
    var attemptsAsGlobal = 1;
    var Ischained = "";
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
        noOftries = GetQueryStringParams('noofretry');
        if (noOftries == null) {
            $.ajax({
                url: "TeachPage_timeEditor.aspx/getNoOfTrials",
                data: "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    noOftries = data.d;


                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }


            });
        }
        attemptsAsGlobal = noOftries;

        //  var setValue = 4141;
        //var stepValue = 4143;


        var imgSrc = '#f5f5f5'
        var hourTime = 12
        var minuteTime = 0
        var arryData = [];
        var stepCount = "";

        returnAnswr();
        // draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock)
        $.ajax({
            url: "TeachPage_timeEditor.aspx/GetClockImageSrc",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var clockData = data.d;

                var splitData = clockData.split('^');
                clockBaground = splitData[0];
                //IsDigitsClock = splitData[1];
                if (splitData[1] == 'true') {
                    IsDigitsClock = true;
                }
                else {
                    IsDigitsClock = false;
                }
                //  alert(clockBaground);
                /* if (IsDigitsClock == true) {
                     //document.getElementById("chkDigit").checked = true;
                     $('#chkDigit').attr('checked', true);
                 }
                 else {
                     $('#chkDigit').attr('checked', false);
                 } */

                $.ajax({
                    url: "TeachPage_timeEditor.aspx/GetElementData",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        var contents = data.d;

                        if (contents != "") {
                            //alert(contents);
                            $('#distractorDivTeach').empty();
                        }
                        var content_Array = contents.split('@');

                        for (indI = 0; indI < content_Array.length - 1; indI++) {

                            var contentSub_array = content_Array[indI].split('^');


                            for (indJ = 0; indJ < contentSub_array.length - 1; indJ++) {
                                statusValue = contentSub_array[1];
                                // alert(status);
                                //alert('hour+min:' + contentSub_array[0] + "// status: " + contentSub_array[1]);
                                var content_hour = contentSub_array[0].split('.');

                                hourValue = content_hour[0];
                                minuteValue = content_hour[1];
                                //alert(statusValue);
                                //  alert(hourValue);
                                // alert(minuteValue);

                                if (statusValue == "C") {
                                    var imgSrc = '#f5f5f5'
                                    //var rightDivDistractor = '<div class = "DivDistractorPreview" id = "DisDiv"><div class = "optionDis" style = "height:50px;background-color: yellow;margin-top: 35px;"><span id = "answerHour" style = "textalign:center;">' + hourValue + '</span><span id = "answerMinute"style = "textalign:center;">' + minuteValue + '</span></div></div>"';

                                    var rightDivDistractor = '<div id="optionAnswer" class="distTemplateSmallAnswerPreview"><div class="statusDivSmall"><div></div></div><div class="tempBPreview">' +
                                '<div class ="datacontent"><table style = "width:100%;"><tr><td><span class="AnswerHour">' + hourValue + '</span><span class="fined">:</span>' +
                                '<span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span></div></div>'
                                    $('#distractorDivTeach').append(rightDivDistractor);

                                    $('.clockContainerPreviewTeach').empty();
                                    //alert(clockBaground);
                                    draw_clock(clockBaground, hourValue, minuteValue, IsDigitsClock);

                                }
                                else {

                                    var imgSrc = '#f5f5f5'
                                    //var wrongDivSistractor = '<div class = "DivDistractorPreview" id = "DisDiv"><div class = "optionDis" style = "height:50px;background-color: yellow;margin-top: 35px;"><input type="label" class = "answerHour" value = "'+hourValue+'"></input></div></div>"';
                                    var wrongDivSistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div></div></div>' +
                                '<div class="tempBPreview"><div class ="datacontent"><table style = "width:100%;"><tr><td><span class="AnswerHour">' + hourValue + '</span>' +
                                '<span class="fined">:</span><span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div><div class="tempL">' +
                                '<span class="leOptId"></span></div></div>'
                                    $('#distractorDivTeach').append(wrongDivSistractor);


                                }

                            }
                        }
                        _Stopwatch.start();
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




        $(document).on("click", ".distTemplateSmallAnswerPreview", function () {

            ShowCorrect();

        });

        $(document).on("click", ".distTemplateSmall", function () {

            ShowWrong();

        });


        $('#btn_previewOk').click(function () {
            $('.reinforcement').fadeOut('slow', function () {
                $('.reinforcement').fadeOut('slow');
            });
        });

        //var _Stopwatch = new StopWatch();

    });

    function ResultSaving(CrctWrng) {
        //  alert('Duration : ' + _Stopwatch.duration() + '\nRetries : ' + (3 - noOftries));
        $.ajax({
            url: "TeachPage_timeEditor.aspx/SaveData",
            data: "{'val1':'" + _Stopwatch.duration() + "','val2':'" + CrctWrng + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                $.ajax({
                    url: "TeachPage_timeEditor.aspx/GetStepNumbrs",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {

                        if (data.d == 0) {
                            //  alert('Completed');
                            window.location = "ExamOverPage.aspx";
                        }
                        else {
                            
                            $.ajax({
                                url: "TeachPage_timeEditor.aspx/getNoOfTrials",
                                data: "{}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataFilter: function (data) { return data; },
                                success: function (data) {

                                    var url = "TeachPage_timeEditor.aspx?noofretry=" + data.d + "";
                                    $(location).attr('href', url);

                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert(textStatus);
                                }


                            });



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

    function ShowCorrect() {
        _Stopwatch.stop();
        //ResultSaving();
        $("#div_Correct").fadeIn(300);

        setInterval(function () { HideDialog('correct') }, 4000);           //setting timer for 4 sec to direct to next question if selected answer is correct


    }
    function ShowWrong() {
        _Stopwatch.stop();
        //ResultSaving();
        $("#div_Wrong").fadeIn(300);

        setInterval(function () { HideDialog('wrong') }, 4000);           //setting timer for 4 sec to direct to next question if selected answer is wrong


    }

    function HideDialog(valid) {

        $("#div_Correct").fadeOut(300);
        $("#div_Wrong").fadeOut(300);

        //  alert(valid);
        if (valid == 'correct') {

            $(document).ready(function () {
                ResultSaving('+');
            });
        }
        else if (valid == 'wrong') {


            noOftries--;
            if (noOftries > 0) {
                var url = "TeachPage_timeEditor.aspx?noofretry=" + noOftries + "";
                $(location).attr('href', url);

            }
            else {
                _Stopwatch.stop();
                // alert('Attempt Completed!! Sorry !! You are Failed!!!!');
                ResultSaving('-');
            }

        }
    }

    function returnAnswr() {
        //var studID = 12;
        //$.ajax({
        //    url: "TeachPage_timeEditor.aspx/GetCorrectAns",
        //    //data: "{'studId':'" + studID + "'}",
        //    dataType: "json",
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    dataFilter: function (data) { return data; },
        //    success: function (data) {
        //        var contents = data.d;
        //        var nwValue = String(contents).split(',');

        //        var ifrmCrct = document.getElementById('iframeCorrect');
        //        var ifrmWrng = document.getElementById('iframeWrong');

        //     //   alert(nwValue[0]);
        //      //  alert(nwValue[1]);

        //        if ((nwValue[0] != null && nwValue[1] != null) && (nwValue[0] != undefined && nwValue[1] != undefined)) {
        //            ifrmCrct.src = nwValue[0];
        //            ifrmWrng.src = nwValue[1];
        //        }
        //        else {
        //            ifrmCrct.src = "Repository/reinforcement/" + $('#corrReinTemp').val() + ".swf";
        //            ifrmWrng.src = "Repository/reinforcement/" + $('#wrongReinTemp').val() + ".swf";
        //        }
        //        // ifrmCrct.src = "Repository/reinforcement/birthday.swf";
        //        // ifrmWrng.src = "Repository/reinforcement/falling-stars.swf";
        //    },
        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        alert(textStatus);
        //    }
        //});

        $('#iframeCorrect').attr('src', "Repository/reinforcement/" + $('#corrReinTemp').val() + ".swf");
        $('#iframeWrong').attr('src', "Repository/reinforcement/" + $('#wrongReinTemp').val() + ".swf");
    }
</script>
</html>
