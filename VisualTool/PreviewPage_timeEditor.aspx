<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreviewPage_timeEditor.aspx.cs"
    Inherits="PreviewPage_timeEditor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/timeLesson.css" rel="stylesheet" />
    <link href="scripts/ui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>
    <script type="text/javascript" src="scripts/anologClock.js"></script>



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
            canvas = Raphael("clockDiv", 200, 200);
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
                alert(e);
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

</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: auto;">
            <div class="previewArea">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <div id="clockDiv" class="clockContainerPreview">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="questionAsk">
                                <span class="tdTextLarge" style="vertical-align: central">What is the time now?</span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <div id="distractorDiv" class="distractorAreaPreview">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="reinforcement" style="position: absolute; top: 0px; left: 0px; width: 100%; height: 500px; z-index: 1000; display: none;">
                <iframe style="width: 100%; height: 465px;" id="reinfIframe"></iframe>
                <input id="btn_previewOk" type="button" value="Ok" />
            </div>
            <div id="previewClose">
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">

    var iPad = 0;
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
        var setValue = GetQueryStringParams('setNumber');
        var stepValue = GetQueryStringParams('stepNumber');
        var imgSrc = '#f5f5f5'
        var hourTime = 12
        var minuteTime = 0
        var arryData = [];
        IsDigitsClock = true;
        draw_clock(clockBaground, hourTime, minuteTime, IsDigitsClock);
        $.ajax({
            url: "PreviewPage_timeEditor.aspx/GetClockImageSrc",
            data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var clockData = data.d;

                // alert(clockData);
                var splitData = clockData.split('^');
                clockBaground = splitData[0];
                //IsDigitsClock = splitData[1];
                if (splitData[1] == 'true') {
                    IsDigitsClock = true;
                }
                else {
                    IsDigitsClock = false;
                }
                /* if (IsDigitsClock == true) {
                     //document.getElementById("chkDigit").checked = true;
                     $('#chkDigit').attr('checked', true);
                 }
                 else {
                     $('#chkDigit').attr('checked', false);
                 } */

                $.ajax({
                    url: "PreviewPage_timeEditor.aspx/GetElementData",
                    data: "{'setNumber':'" + setValue + "','stepNumber':'" + stepValue + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        var contents = data.d;

                        //  alert(contents);
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
                                var content_hour = contentSub_array[0].split('.');

                                hourValue = content_hour[0];
                                minuteValue = content_hour[1];
                                //  alert(statusValue);
                                // alert(hourValue);
                                //  alert(minuteValue);

                                if (statusValue == "C") {
                                    var imgSrc = '#f5f5f5'
                                    //var rightDivDistractor = '<div class = "DivDistractorPreview" id = "DisDiv"><div class = "optionDis" style = "height:50px;background-color: yellow;margin-top: 35px;"><span id = "answerHour" style = "textalign:center;">' + hourValue + '</span><span id = "answerMinute"style = "textalign:center;">' + minuteValue + '</span></div></div>"';

                                    var rightDivDistractor = '<div id="optionAnswer" class="distTemplateSmallAnswerPreview"><div class="statusDivSmall"><div></div></div><div class="tempBPreview">' +
                                '<div class ="datacontent"><table style = "width:100%;"><tr><td><span class="AnswerHour">' + hourValue + '</span><span class="fined">:</span>' +
                                '<span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div><div class="tempL"><span class="leOptId"></span></div></div>'
                                    $('#distractorDiv').append(rightDivDistractor);

                                    $('.clockContainerPreview').empty();
                                    draw_clock(clockBaground, hourValue, minuteValue, IsDigitsClock);

                                }
                                else {

                                    var imgSrc = '#f5f5f5'
                                    //var wrongDivSistractor = '<div class = "DivDistractorPreview" id = "DisDiv"><div class = "optionDis" style = "height:50px;background-color: yellow;margin-top: 35px;"><input type="label" class = "answerHour" value = "'+hourValue+'"></input></div></div>"';
                                    var wrongDivSistractor = '<div id="option" class="distTemplateSmall"><div class="statusDivSmall"><div></div></div>' +
                                '<div class="tempBPreview"><div class ="datacontent"><table style = "width:100%;"><tr><td><span class="AnswerHour">' + hourValue + '</span>' +
                                '<span class="fined">:</span><span class="AnswerMinute">' + minuteValue + '</span></td></tr></table></div></div><div class="tempL">' +
                                '<span class="leOptId"></span></div></div>'
                                    $('#distractorDiv').append(wrongDivSistractor);


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


        $(document).on("click", ".distTemplateSmallAnswerPreview", function () {

            var realPath = 'Repository/reinforcement/2012.swf';
            $('#reinfIframe').attr('src', 'Repository/reinforcement/fireworks.swf');
            $('.reinforcement').show();

        });

        $(document).on("click", ".distTemplateSmall", function () {

            alert('Wrong Answer');
        });


        $('#btn_previewOk').click(function () {
            $('.reinforcement').fadeOut('slow', function () {
                $('.reinforcement').fadeOut('slow');
            });
        });
    });
</script>
</html>
