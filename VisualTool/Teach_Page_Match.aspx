<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Teach_Page_Match.aspx.cs" Inherits="Phase002_1_Teach_Page_Match" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="scripts/StopWatchNew.js"></script>
    <link href="styles/matchingLesson.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />

    <link href="styles/commonStyle.css" rel="stylesheet" />


    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .ml-base {
            height: 170px;
            width: 300px;
        }

        .ml-label {
            height: 30px;
        }

        .ml_answerTemplate, .questionTemp {
            float: left;
            width: 415px;
            height: 200px;
        }

        p {
            margin: 0px;
        }

        .heading-Board {
            height: 80px;
            width: 100%;
            text-align: left;
        }

        .h1Design {
            color: purple;
            font-family: 'Times New Roman';
            font-size: 30px;
            font-weight: lighter;
            padding: 30px;
        }

        .web_dialog {
            background: none repeat scroll 0 0 #FFFFFF;
            color: White;
            display: none;
            font: 13px/20px "Helvetica Neue",Helvetica,Arial,sans-serif;
            height: 634px;
            left: 50%;
            margin-left: -670px;
            margin-top: -568px;
            position: relative;
            top: 0px;
            width: 100%;
            z-index: 102;
        }

        #dashboard-content-panel {
            margin: 0 auto 0 35px;
            width: 1311px;
        }
    </style>

    <script src="scripts/jquery-1.8.0.js"></script>
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <div id="dashboard-top-panel">

            <div id="top-panel-container">
                <ul>

                    <li class="user" style="width: 90%;">
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

                <div class="header-links">
                </div>
            </div>
            <div class="clear"></div>
            <br />


            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHS">

                    <div class="dashboard-RHS-content">
                        <div style="font-weight: 700; width: 100%;">

                            <table class="auto-style1">
                                <tr>
                                    <td>
                                        <div class="heading-Board">
                                            <span class="h1Design">Match Lesson</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <div style="height: 200px; width: 415px; margin: 0 auto; padding: 2px; border: 1px dashed red;">
                                            <div class="questionTemp">
                                                <div class="player" style="float: left; margin-top: -21px;">
                                                </div>
                                                <div class="ml_base">
                                                </div>
                                                <div class="ml_baseLable">
                                                </div>

                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="corrReinTemp" runat="server" />
                                        <asp:HiddenField ID="wrongReinTemp" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="distContainer"></div>
                                    </td>
                                </tr>
                            </table>
                            <%--  <div class="reinforcement" style="position: absolute; top: 0px; left: 0px; width: 100%; height: 500px; z-index: 1000; display: none;">
                                <iframe style="width: 100%; height: 500px;" id="reinfIframe"></iframe>
                            </div>--%>


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
                        </div>

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




<script type="text/javascript">
    var _Stopwatch = new StopWatch();
    //_Stopwatch.start();
    // var lessonId = 1;
    // var lessDetId = 1;
    var noOftries = 1;
    var optList = "";



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
    //var inputArray = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);


    $(document).ready(function () {


        noOftries = GetQueryStringParams('noofretry');
        if (noOftries == null) {
            $.ajax({
                url: "Teach_Page_Match.aspx/getNoOfTrials",
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
        // fn_randomizer(inputArray);

        fn_getLeOptIds();

        returnAnswr();
    });

    //EXTERNAL FUNCTIONS

    function fn_randomizer(inputArray) {

        for (i = 0; i < inputArray.length; i++) {
            if (Math.floor((Math.random() * 2) + 1) == 1) {
                var j = Math.floor((Math.random() * (inputArray.length - 1)) + 1);

                var temp = inputArray[i];
                inputArray[i] = inputArray[j];
                inputArray[j] = temp;

            }
        }

        //alert(inputArray);
        return inputArray;
    }


    function fn_getLeOptIds() {
        $.ajax({
            url: "Teach_Page_Match.aspx/getLeOptId",
            data: "{}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var optList = data.d;
                var optL = optList.split(',');
                var optL2 = new Array();
                var optL22 = new Array();
                var j = 0;

                for (var i = 0; i < optL.length - 1; i++) {

                    var optLstat = optL[i].split('^');

                    optL2[i] = optLstat[0];

                    if (optLstat[1] != "Q") {
                        optL22[j] = optLstat[0];
                        j++;
                        //optL2[i] = optL[i];
                    }
                }

                var optRandList = new Array(optL22.length);

                for (var i = 0; i < optL22.length; i++) {
                    optRandList[i] = i;
                }

                optRandList = fn_randomizer(optRandList);

                for (i = 0; i < optL22.length; i++) {
                    if (optL[optRandList[i]] != "") {
                        var ansTemp = '<div id="ansTemp' + optL22[optRandList[i]] + '" class="ml_answerTemplate"><div class="ml_base"></div><div class="ml_baseLable"></div></div>';

                        $('.distContainer').append(ansTemp);
                    }
                }
                _Stopwatch.start();

                $('.ml_answerTemplate').click(function () {





                    if ($(this).hasClass('C')) {
                        //var realPath = 'Repository/reinforcement/2012.swf';
                        //$('#reinfIframe').attr('src', 'Repository/reinforcement/fireworks.swf');
                        //$('.reinforcement').show();
                        //  alert('Success');

                        ShowCorrect();


                        //$.ajax({
                        //    url: "Teach_Page_Match.aspx/GetStepNumbrs",
                        //    dataType: "json",
                        //    type: "POST",
                        //    contentType: "application/json; charset=utf-8",
                        //    dataFilter: function (data) { return data; },
                        //    success: function (data) {

                        //        if (data.d == 0)
                        //            alert('Completed');
                        //        else {
                        //            var url = "Teach_Page_Match.aspx?noofretry=3";
                        //            $(location).attr('href', url);
                        //        }

                        //    },
                        //    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //        alert(textStatus);
                        //    }
                        //});


                    }
                    else {


                        // alert('Wrong Answer');
                        ShowWrong();

                        //alert('Wrong Answer');
                        //noOftries--;
                        //alert(noOftries);
                        //if (noOftries > 0) {
                        //    //alert('Enter');
                        //    var url = "Teach_Page_Match.aspx?noofretry=" + noOftries + "";
                        //    $(location).attr('href', url);

                        //}
                        //else {
                        //    alert('Attempt Completed!! Sorry !! You are Failed!!!!');

                        //    $.ajax({
                        //        url: "Teach_Page_Match.aspx/GetStepNumbrs",
                        //        dataType: "json",
                        //        type: "POST",
                        //        contentType: "application/json; charset=utf-8",
                        //        dataFilter: function (data) { return data; },
                        //        success: function (data) {

                        //            if (data.d == 0)
                        //                alert('Completed');
                        //            else {
                        //                var url = "Teach_Page_Match.aspx?noofretry=3";
                        //                $(location).attr('href', url);
                        //            }
                        //        },
                        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //            alert(textStatus);
                        //        }
                        //    });
                        //}
                    }
                });

                fn_getTempData(optL2);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });


        // alert(optList);
    }


    function fn_getTempData(leOptId) {
        $.ajax({
            url: "Teach_Page_Match.aspx/getTempData",
            data: "{'leOptId':'" + leOptId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var recContent = data.d;

                var recContentArray = recContent.split('☺');

                for (k = 0; k < recContentArray.length - 1; k++) {

                    var recContSubArray = recContentArray[k].split('^');



                    if (recContent.indexOf("System.IndexOutOfRangeException") == -1) {
                        if (recContSubArray[1] != 'Q') {

                            $('#ansTemp' + recContSubArray[2]).addClass(recContSubArray[1]);


                            var listNew = $(recContSubArray[0]).find('.saveElem');
                            var lblContent = $(recContSubArray[0]).find('.label').html();
                            var details = "<div>";

                            var W = '#ansTemp' + recContSubArray[2];


                            // if ($(W).length > 0) {
                            var Woff = $(W).offset();

                            for (var i = 0; i < listNew.length; i++) {

                                var height = $(listNew[i]).find('.height').html();
                                var width = $(listNew[i]).find('.width').html();
                                var top = parseFloat($(listNew[i]).find('.top').html()) + Woff.top;
                                var left = parseFloat($(listNew[i]).find('.left').html()) + Woff.left;
                                var data = $(listNew[i]).find('.data').html();


                                data = data.replace(/&gt;/g, '>');
                                data = data.replace(/&lt;/g, '<');


                                $('#ansTemp' + recContSubArray[2] + ' .ml_base').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');

                            }



                            $('#ansTemp' + recContSubArray[2] + ' .ml_baseLable').append(lblContent);
                            // }
                        }

                        else {


                            $('#ansTemp' + recContSubArray[2]).remove();

                            var listNew = $(recContSubArray[0]).find('.saveElem');
                            var lblContent = $(recContSubArray[0]).find('.label').html();
                            var musicFile = $(recContSubArray[0]).find('.music').html();
                            var details = "<div>";
                            var W = '.questionTemp';
                            var Woff = $(W).offset();

                            for (var i = 0; i < listNew.length; i++) {

                                var height = $(listNew[i]).find('.height').html();
                                var width = $(listNew[i]).find('.width').html();
                                var top = parseFloat($(listNew[i]).find('.top').html()) + Woff.top;
                                var left = parseFloat($(listNew[i]).find('.left').html()) + Woff.left;
                                var data = $(listNew[i]).find('.data').html();


                                data = data.replace(/&gt;/g, '>');
                                data = data.replace(/&lt;/g, '<');


                                $(W + ' .ml_base').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');

                            }



                            $(W + ' .ml_baseLable').append(lblContent);
                            $('.player').empty();
                            if (musicFile != '') {

                                // $('.player').append('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /><param name="wmode" value="transparent" /></object>');
                                $('.player').append(musicFile);
                            }
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }

    function ResultSaving(CrctWrng) {
        // alert('Duration : ' + _Stopwatch.duration() + '\nRetries : ' + (3 - noOftries));
        $.ajax({
            url: "Teach_Page_Match.aspx/SaveData",
            data: "{'val1':'" + _Stopwatch.duration() + "','val2':'" + CrctWrng + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                $.ajax({
                    url: "Teach_Page_Match.aspx/GetStepNumbrs",
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
                                url: "Teach_Page_Match.aspx/getNoOfTrials",
                                data: "{}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataFilter: function (data) { return data; },
                                success: function (data) {

                                    var url = "Teach_Page_Match.aspx?noofretry=3";
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
            ResultSaving('+');
            //$.ajax({
            //    url: "Teach_Page_Match.aspx/GetStepNumbrs",
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
            //            var url = "Teach_Page_Match.aspx?noofretry=3";
            //            $(location).attr('href', url);
            //        }

            //    },
            //    error: function (XMLHttpRequest, textStatus, errorThrown) {
            //        alert(textStatus);
            //    }
            //});

        }
        else if (valid == 'wrong') {


            noOftries--;
            //  alert(noOftries);
            if (noOftries > 0) {
                //alert('Enter');
                var url = "Teach_Page_Match.aspx?noofretry=" + noOftries + "";
                $(location).attr('href', url);

            }
            else {
                _Stopwatch.stop();

                // alert('Attempt Completed!! Sorry !! You are Failed!!!!');
                ResultSaving('-');

                //$.ajax({
                //    url: "Teach_Page_Match.aspx/GetStepNumbrs",
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
                //            var url = "Teach_Page_Match.aspx?noofretry=3";
                //            $(location).attr('href', url);
                //        }
                //    },
                //    error: function (XMLHttpRequest, textStatus, errorThrown) {
                //        alert(textStatus);
                //    }
                //});
            }
        }
    }


    function returnAnswr() {
        ////var studID = 12;
        //$.ajax({
        //    url: "Teach_Page_Match.aspx/GetCorrectAns",
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

        //        if ((nwValue[0] != null && nwValue[1] != null) && (nwValue[0] != undefined && nwValue[1] != undefined)) {
        //            ifrmCrct.src = nwValue[0];
        //            ifrmWrng.src = nwValue[1];
        //        }
        //        else {
        //            ifrmCrct.src = "Repository/reinforcement/" + $('#corrReinTemp').val() + ".swf";
        //            ifrmWrng.src = "Repository/reinforcement/" + $('#wrongReinTemp').val() + ".swf";
        //        }
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
