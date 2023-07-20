<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeachPage_mouseEditor.aspx.cs" Inherits="TeachPage_mouseEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <link href="styles/mouseStyle.css" rel="stylesheet" />
    <link href="scripts/ui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />



    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>
    <script src="scripts/StopWatch.js"></script>
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

        
        

    </script>
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>
                    <li class="user" style="width: 88%;">
                        <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
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


                    <a href="homePage.aspx" class="homecls"></a>
                    <a href="LessonManagement.aspx" class="lessnClass"></a>
                    <a href="repository-manag.aspx" class="repocls"></a>
                </div>

            </div>


            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHS">

                    <div class="dashboard-RHS-content">

                        <%--<div id="main-heading-panel">Student Assesment Plan</div>--%>
                        <!-- NEW CODES -->

                        <div class="previewAreaTeach">
                            <div id="imgValue">
                            </div>
                        </div>

                    </div>

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

        <%-- Reinforcement Tag--%>

        <div id="div_Correct" class="web_dialog">
            <%--<button type="button" id="btn_closeCrct" onclick="javascript:HideDialog();">Close</button>--%>
            <a id="close_x" class="close sprited" href="#" onclick="javascript:HideDialog();">
                <img src="../Administration/images/clb.PNG" style="border: 0px; float: right; padding: 5px" width="25" alt="" />
            </a>
            <iframe id="iframeCorrect" width="100%" height="100%" frameborder="1"></iframe>
        </div>
        <div id="div_Wrong" class="web_dialog">
            <%--<button type="button" id="btn_closeWrng" onclick="javascript:HideDialog();">Close</button>--%>
            <a id="close" class="close sprited" href="#" onclick="javascript:HideDialog();">
                <img src="../Administration/images/clb.PNG" style="border: 0px; float: right; padding: 5px" width="25" alt="" />
            </a>
            <iframe id="iframeWrong" width="100%" height="100%" frameborder="1"></iframe>
        </div>
    </form>
</body>

<script type="text/javascript">

    var _Stopwatch = new StopWatch();
    var clickTime = 0;
    var nmbrofImage = 0;
    //_Stopwatch.start();
    function clickMe(id) {
        var imgId = $('#' + id)
        imgId.hide();
        var count = --nmbrofImage;
        if (count == 0) {
            _Stopwatch.stop();
            //alert('Lesson Plan Completed');


            //alert(_Stopwatch.duration());

            //$.ajax({
            //    url: "TeachPage_mouseEditor.aspx/GetStepNumbrs",
            //    dataType: "json",
            //    type: "POST",
            //    contentType: "application/json; charset=utf-8",
            //    dataFilter: function (data) { return data; },
            //    success: function (data) {

            //        if (data.d == 0)
            //            alert('Completed');
            //        else {
            //            var url = "TeachPage_mouseEditor.aspx";
            //            $(location).attr('href', url);
            //        }
            //    },
            //    error: function (XMLHttpRequest, textStatus, errorThrown) {
            //        alert(textStatus);
            //    }
            //});


            ShowCorrect();
            //ShowWrong();
        }

    }

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

    //function saveTimeInDB(duration) {
    //    alert(duration);
    //}
    function ResultSaving(CrctWrng) {
        //alert('Duration : ' + _Stopwatch.duration() + '\nRetries : ' + (3 - noOftries));
        $.ajax({
            url: "TeachPage_mouseEditor.aspx/SaveData",
            data: "{'val1':'" + _Stopwatch.duration() + "','val2':'" + CrctWrng + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                $.ajax({
                    url: "TeachPage_mouseEditor.aspx/GetStepNumbrs",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {

                        if (data.d == 0) {
                            // alert('Completed');
                            window.location = "ExamOverPage.aspx";
                        }
                        else {
                            var url = "TeachPage_mouseEditor.aspx";
                            $(location).attr('href', url);
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
    // Reinforcement IFRAME CLose Tag

    function ShowCorrect() {
        //ResultSaving();
        $("#div_Correct").fadeIn(300);

        setInterval(function () { HideDialog('correct') }, 4000);           //setting timer for 4 sec to direct to next question if selected answer is correct



    }
    function ShowWrong() {
        //ResultSaving();
        $("#div_Wrong").fadeIn(300);

        setInterval(function () { HideDialog('wrong') }, 4000);           //setting timer for 4 sec to direct to next question if selected answer is wrong
    }

    function HideDialog() {

        $("#div_Correct").fadeOut(300);
        $("#div_Wrong").fadeOut(300);

        $(document).ready(function () {

            ResultSaving('+');

            //$.ajax({
            //    url: "TeachPage_mouseEditor.aspx/GetStepNumbrs",
            //    dataType: "json",
            //    type: "POST",
            //    contentType: "application/json; charset=utf-8",
            //    dataFilter: function (data) { return data; },
            //    success: function (data) {

            //        if (data.d == 0) {
            //            alert('Completed');
            //            window.location = "ThankYou.aspx";
            //        }
            //        else {
            //            var url = "TeachPage_mouseEditor.aspx";
            //            $(location).attr('href', url);
            //        }
            //    },
            //    error: function (XMLHttpRequest, textStatus, errorThrown) {
            //        alert(textStatus);
            //    }
            //});
        });

    }
    // Reinforcement tag
    function returnAnswr() {
        //var studID = 12;
        $.ajax({
            url: "TeachPage_mouseEditor.aspx/GetCorrectAns",
            //data: "{'studId':'" + studID + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {

                var contents = data.d;
                var nwValue = String(contents).split(',');

                var ifrmCrct = document.getElementById('iframeCorrect');
                var ifrmWrng = document.getElementById('iframeWrong');

                if ((nwValue[0] != null && nwValue[1] != null) && (nwValue[0] != undefined && nwValue[1] != undefined)) {
                    ifrmCrct.src = nwValue[0];
                    ifrmWrng.src = nwValue[1];
                }
                else {
                    ifrmCrct.src = "Repository/reinforcement/" + $('#corrReinTemp').val() + ".swf";
                    ifrmWrng.src = "Repository/reinforcement/" + $('#wrongReinTemp').val() + ".swf";
                }

                //var contents = data.d;

                //var ifrmCrct = document.getElementById('iframeCorrect');
                //var ifrmWrng = document.getElementById('iframeWrong');

                //ifrmCrct.onerror = "this.src = 'Repository/images/noimage.png'";


                //ifrmCrct.src = "Repository/reinforcement/birthday.swf";
                //ifrmWrng.src = "Repository/reinforcement/falling-stars.swf";
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    }

    $(document).ready(function () {
        addStopwatch('InnerWatchContainer')
        var setNumber = 2068
        var stepNumber = 2069
        var speedValue = 0;
        var speedMeasure = 0;

        returnAnswr();        // Call Reinforcement on Page Load

        $.ajax({
            url: "TeachPage_mouseEditor.aspx/GetElementData",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                save = true;
                $('.innerMsgRibbon').text('Saved');
                $('.messageRibbon').show().fadeOut(2000);
                var contents = data.d;
                //  alert(contents);
                var content_speed = contents.split('^');
                var content_Data = contents.split(',');
                speedValue = content_speed[content_speed.length - 1];
                nmbrofImage = content_Data.length - 1;
                if (speedValue == 0) {
                    speedMeasure = 0.1;
                }
                else if (speedValue == 1) {
                    speedMeasure = 0.2;
                }
                else if (speedValue == 2) {
                    speedMeasure = 0.4;
                }
                else if (speedValue == 3) {
                    speedMeasure = 0.6;
                }
                else if (speedValue == 4) {
                    speedMeasure = 0.8;
                }

                //alert(speedMeasure);

                for (var i = 0; i < content_Data.length - 1; i++) {
                    //alert(content_Data[i]);
                    $('.previewAreaTeach').append('<li><img class = "a" id = "imgAnimate' + i + '" Height = "100px" Width = "100px" src="' + content_Data[i] + '" onclick="clickMe(this.id)" /></li>');

                    animateDiv(speedMeasure);

                }

                _Stopwatch.start();


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });

        $('#imgAnimate').click(function () {

            //  alert('asdadasd');
        });

        function makeNewPosition() {
            // Get viewport dimensions (remove the dimension of the div)
            var h = $('.previewAreaTeach').height();
            var w = $('.previewAreaTeach').width();
            h = h - Math.floor($('.a').height());
            w = w - Math.floor($('.a').width());

            var nh = Math.floor(Math.random() * h);
            var nw = Math.floor(Math.random() * w);

            return [nh, nw];

        }

        function animateDiv(speedMeasure) {
            // alert(speedMeasure);
            var newq = makeNewPosition();
            var oldq = $('.a').offset();
            var speed = calcSpeed([oldq.top, oldq.left], newq, speedMeasure);

            $('.a').animate({ top: newq[0], left: newq[1] }, speed, function () {
                animateDiv(speedMeasure);
            });

        }

        function calcSpeed(prev, next, Getspeed) {
            next[0] = Math.floor(next[0]) - Math.floor($('.a').height());
            //next[1] = Math.floor(next[1]) - Math.floor($('.a').width());
            var x = Math.abs(prev[1] - next[1]);
            var y = Math.abs(prev[0] - next[0]);

            var greatest = x > y ? x : y;

            var speedModifier = Getspeed;

            var speed = Math.ceil(greatest / speedModifier);

            return speed;

        }


    });



</script>



</html>
