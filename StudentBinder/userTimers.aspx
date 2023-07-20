<%@ Page Language="C#" AutoEventWireup="true" CodeFile="userTimers.aspx.cs" Inherits="StudentBinder_userTimers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        html, body {
            height: 100%;
        }

        .Time-Box {
            width: 50px;
            border: 0px solid;
            text-align: center;
            height: 25px;
            background-color: #DBF0FF;
            border-bottom: 1px solid #0A9DFF;
            font-weight: bold;
        }

        .start-stop, .addBtn, .reset, .dltBtn, .pause-resume {
            border: 0px solid;
            border-bottom: 1px solid;
            cursor: pointer;
            color: #007ACC;
            height: 30px;
            width: 30px;
        }

        .addBtn {
            background-color: #60c658;
            color: white;
            float: left;
        }

        .addBtnG {
            background-color: #60C658;
        }

        .addBtnB {
            background-color: #60c658;
        }

        .dltBtn {
            background-color: #FF0000; /*#F21616;*/
            color: white;
            float: left;
        }

        .lblText {
            width: 80px;
            border-width: 0px;
            border-bottom: 1px solid;
            height: 25px;
        }

        .studentName {
            background-color: #70C5FF;
            color: white;
            width: 160px;
            float: left;
            height: 29px;
            font-size: 14px;
            white-space: nowrap;
            text-align: center
        }

        .stdNameContainer {
            width: 250px;
            border: 1px solid;
            float: left;
        }

        #addStudentTimer {
            background-color: #000000;
            color: white;
            cursor: pointer;
            text-align: center;
            width: 252px;
            position: fixed;
            z-index: 1;
        }

        #genTimer {
            background-color: #60c658;
            color: white;
            cursor: pointer;
            text-align: center;
            width: 252px;
            position: fixed;
            z-index: 1;
        }

        .dummyForFreeze {
            width: 252px;
            height: 20px;
        }

        .timDel {
            border: 0px solid;
            border-bottom: 1px solid;
            cursor: pointer;
            color: #000;
            height: 30px;
            width: 30px;
        }

        .studentTimers {
            position: relative;
            float: left;
        }

        #TimerContainer {
            width: 250px;
        }

        .tMin, .tHour, .tSec {
            width: 20px;
            float: left;
            height: 26px;
            border: 1px solid #007ACC;
        }

        .addTimerBox {
            position: absolute;
            margin-top: 25px;
            border: 1px solid #007ACC;
            display: none;
            z-index: 2;
        }

        .timerName {
            width: 85px;
            float: left;
            height: 26px;
            border: 1px solid #007ACC;
        }

        .tBarCont {
            background-color: #ff0000;
            border: 0 solid #ff0000;
            height: 1px;
            width: 100%;
        }

        #studentList {
            border: 1px solid;
            z-index: 2;
            margin-top: 20px;
            position: fixed;
            width: 250px;
        }

        .timerStdList {
            background-color: #93C9FF;
            border-bottom: 1px solid;
            padding: 3px;
            width: 244px;
        }

        .bOverlay {
            background-color: black;
            height: 100%;
            opacity: 0.5;
            position: fixed;
            width: 308px;
            z-index: 1;
            display: none;
        }

        .validMessage {
            background-color: yellow;
            color: red;
            font-family: consolas;
            font-size: 12px;
            margin-top: -18px;
            position: absolute;
            text-align: center;
            width: 201px;
            display:none;
        }
         .timerWrapper {
            float: left;
            width:260px;
        }
    </style>

    <script src="js/jquery-1.8.0.min.js"></script>
    <script src="js/userStopWatch.js"></script>
    <script>

        function addNewTimer(elem) {
            var currIndex = $('#indexVal').val();
            var nextIndex = parseInt(currIndex) + 1;

            var newId = "stopwatch" + nextIndex;
            $('#indexVal').val(nextIndex);

            var timerHtml = "<div id = " + newId + " class='timerWrapper'></div>";
            $(elem).parent().find('.studentTimers').append(timerHtml);

            addStopwatch(newId);

        }
        function delTimer(elm) {

            var $el = $(elm), x = 3000, originalColor = $el.css("background"), confirm = $el.hasClass('confirm');

            if (confirm == false) {
                $el.addClass('confirm');

                var msgDiv = '<div class="instruction" style="position: absolute; background-color: yellow; margin-right: 0px; width: 210px; height: 24px;">click again to confirm delete</div>';
                $(elm).parent().prepend(msgDiv);


                $el.css("background", "#DA0B0B");
                setTimeout(function () {
                    $el.css("background", originalColor);
                    $el.removeClass('confirm');
                    $(elm).parent().find('.instruction').remove();
                }, x);
            }
            else {

                $(elm).parent().parent().remove();
                $(elm).parent().find('.instruction').remove();
            }


        }
        function dltStdContainer(elm) {
            var $el = $(elm), x = 3000, originalColor = $el.css("background"), confirm = $el.hasClass('confirm');

            if (confirm == false) {
                $el.addClass('confirm');

                var msgDiv = '<div class="instruction" style="position: absolute; background-color: yellow; margin-right: 0px; width: 210px; height: 24px;">click again to confirm delete</div>';
                $(elm).parent().prepend(msgDiv);


                $el.css("background", "#DA0B0B");
                setTimeout(function () {
                    $el.css("background", originalColor);
                    $el.removeClass('confirm');
                    $(elm).parent().find('.instruction').remove();
                }, x);
            }
            else {

                $(elm).parent().remove();
                $(elm).parent().find('.instruction').remove();
            }


        }
        function exp_coll(elm) {

            $(elm).parent().find('.studentTimers').toggle();

        }
        function addStudentTimer() {
            $.ajax(
                      {
                          type: "POST",
                          url: "userTimers.aspx/getStudentDetails",
                          //data: "{'Tab':'" + Type + "'}",
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          async: false,

                          success: function (data) {
                              var returnListArray = (data.d).split('^');
                              var studentId = returnListArray[0];
                              var studentName = returnListArray[1];
                              if (parseInt(studentId) > 0) {
                                  if ($('#stdTmr-' + studentId).length > 0) {
                                      var $el = $('#stdTmr-' + studentId).find('.studentName'), x = 1000, originalColor = $el.css("background");

                                      $el.css("background", "#FF7A7A");
                                      setTimeout(function () {
                                          $el.css("background", originalColor);
                                      }, x);
                                  }
                                  else {
                                      var stdDivContainer = "<div id='stdTmr-" + studentId + "' class='stdNameContainer'><div class='studentName' style='cursor:pointer' title='click to collapse/expand' onclick='exp_coll(this)'>" + studentName + " </div><input type='button' class='addBtn' value='+' onclick='addNewTimer(this)'/><input type='button' class='dltBtn' value='X' onclick='dltStdContainer(this)'/> <div class='studentTimers'></div> </div>";
                                      $('#TimerContainer').append(stdDivContainer);
                                  }
                              }
                              else {
                                  var $el = $('#addStudentTimer'), x = 3000, originalColor = $el.css("background"), text = $el.html();


                                  $el.css("background", "#DA0B0B");
                                  $el.html('Select a student first');
                                  setTimeout(function () {
                                      $el.css("background", originalColor);
                                      $el.html(text);
                                  }, x);
                              }
                          },
                          error: function (request, status, error) {

                              //alert(request + "/" + status + "/" + error);
                          }
                      });
        }


        function addStudentTab(elem, stdId, stdName) {

            var studentId = stdId;
            var studentName = stdName.replace(/^/g, "'");
            if (parseInt(studentId) > 0) {
                if ($('#stdTmrTb-' + studentId).length > 0) {
                    var $el = $('#stdTmr-' + studentId).find('.studentName'), x = 1000, originalColor = $el.css("background");

                    $el.css("background", "#FF7A7A");
                    setTimeout(function () {
                        $el.css("background", originalColor);
                    }, x);
                }
                else {
                    var stdDivContainer = "<div id='stdTmrTb-" + studentId + "' class='stdNameContainer'>"
                                          + "<div class='addTimerBox'>"
                                          + "<div class='validMessage'>Invalid Values</div>"
                                          + "<input type='text' class='timerName' onclick='$(this).select();'><input type='text' class='tHour' value='0' onclick='$(this).select();'  maxlength='2'><input type='text' class='tMin' value='0' onclick='$(this).select();'  maxlength='2'><input type='text' class='tSec' value='0' onclick='$(this).select();'  maxlength='2'><input type='button' onclick='addTimer(this)' value='&#x2713' class='addBtn'>"
                                          + "<input type='button' class='dltBtn' value='X' onclick='closeBox(this)'></div>"
                                          + "<div class='studentName' style='cursor:pointer' title='click to collapse/expand' onclick='exp_coll(this)'>" + studentName + " </div><input type='button' class='addBtn' value='+&#8987;' onclick='addTimerBox(this)'/><input type='button' class='addBtn' value='+&#x1f550;' onclick='addNewTimer(this)'/><input type='button' class='dltBtn' value='X' onclick='dltStdContainer(this)'/> <div class='studentTimers'></div> </div>";
                    $('#TimerContainer').append(stdDivContainer);
                    $('#studentList').toggle();

                    $(elem).css('background-color', '#4CA5FE');
                }
            }
            else {
                var $el = $('#addStudentTimer'), x = 3000, originalColor = $el.css("background"), text = $el.html();


                $el.css("background", "#DA0B0B");
                $el.html('Select a student first');
                setTimeout(function () {
                    $el.css("background", originalColor);
                    $el.html(text);
                }, x);
            }

        }

        function startTimer(elem) {
            var tContainer = $(elem).parents('.container');

            //tContainer.find('.btnStart').hide();
            //tContainer.find('.btnStop').show();

            tContainer.find('.start-stop').prop('disabled', true).css('color', '#cccccc');

            //tContainer.find('.start-stop').hide();
            //tContainer.find('.pause-resume').show();


            var tHour = tContainer.find('.tHour');
            var tMin = tContainer.find('.tMin');
            var tSec = tContainer.find('.tSec');

            var tText = tContainer.find('.Time-Box');

            var totSec = (parseInt(tHour.val()) * 60 * 60) + (parseInt(tMin.val()) * 60) + parseInt(tSec.val());

            tContainer.find('.thid').val(tHour.val() + ":" + tMin.val() + ":" + tSec.val());

            if (tHour.val() == "" || tMin.val() == "" || tSec.val() == "") {
                return;
            }
            if (!tHour.val().match(/^\d+$/) || !tMin.val().match(/^\d+$/) || !tSec.val().match(/^\d+$/)) {
                alert("Not a Number")
                return;
            }
            //alert(tMin.val().match(/^\d+$/))
            var interval = setInterval(function () {
                if (tSec.val() > 0) {
                    var sec = tSec.val();
                    tSec.val((parseInt(sec) - 1).toString());

                    tText.val(tHour.val() + ":" + tMin.val() + ":" + tSec.val());

                    var totSec_temp = (parseInt(tHour.val()) * 60 * 60) + (parseInt(tMin.val()) * 60) + parseInt(tSec.val());
                    var per = (totSec_temp / totSec) * 100;

                    tContainer.find('.tBarCont').animate({ width: per + '%' }, 1000);
                }
                else {

                    if (tMin.val() > 0) {
                        tSec.val("59");
                        var min = tMin.val();
                        tMin.val((parseInt(min) - 1).toString());

                        tText.val(tHour.val() + ":" + tMin.val() + ":" + tSec.val());

                        var totSec_temp = (parseInt(tHour.val()) * 60 * 60) + (parseInt(tMin.val()) * 60) + parseInt(tSec.val());
                        var per = (totSec_temp / totSec) * 100;

                        tContainer.find('.tBarCont').animate({ width: per + '%' }, 1000);
                    }
                    else {
                        if (tHour.val() > 0) {
                            tMin.val("59");
                            tSec.val("59");
                            var hour = tHour.val();
                            tHour.val((parseInt(hour) - 1).toString());

                            tText.val(tHour.val() + ":" + tMin.val() + ":" + tSec.val());

                            var totsec_temp = (parseInt(tHour.val()) * 60 * 60) + (parseInt(tMin.val()) * 60) + parseInt(tSec.val());
                            var per = (totsec_temp / totSec) * 100;

                            tContainer.find('.tBarCont').animate({ width: per + '%' }, 1000);
                        }
                        else {
                            clearInterval(interval);
                            tContainer.find('.lblText').css('background-color', '#FFDD65');
                            tContainer.find('.Time-Box').css('background-color', '#FECCE7');

                            parent.Notify(tContainer.find('.lblText').val());
                            //alert('Time Expired');
                        }
                    }
                }

            }, 1000);

            
            //tContainer.find('.pause-resume').click(function () {
            //    //tContainer.find('.btnStart').show();
            //    //tContainer.find('.start-stop').prop('disabled', false);

            //    tContainer.find('.start-stop').show();
            //    tContainer.find('.pause-resume').hide();

            //    clearInterval(interval);

            //    tContainer.find('.Time-Box').css('background-color', '#dbf0ff');
            //    tContainer.find('.lblText').css('background-color', '#ffffff');

            //});

           
            tContainer.find('.reset').click(function () {
                tContainer.find('.btnStart').show();
                tContainer.find('.start-stop').prop('disabled', false).css('color', '#007acc');

                //tContainer.find('.start-stop').show();
                //tContainer.find('.pause-resume').hide();
                clearInterval(interval);

                tContainer.find('.tBarCont').animate({ width: '100%' }, 1000);

                var tValue = tContainer.find('.thid').val().split(':');
                tHour.val(tValue[0]);
                tMin.val(tValue[1]);
                tSec.val(tValue[2]);

                tText.val(tHour.val() + ":" + tMin.val() + ":" + tSec.val());
                tContainer.find('.Time-Box').css('background-color', '#dbf0ff');
                tContainer.find('.lblText').css('background-color', '#ffffff');

            });
            tContainer.find('.timDel').click(function () {

                clearInterval(interval);

            });

        }

        function addTimerBox(elem) {

            var currIndexTimer = $('#indexValTimer').val();
            var nextIndexTimer = parseInt(currIndexTimer) + 1;

            var newId = "#timer" + nextIndexTimer;
            //$('#indexValTimer').val(nextIndexTimer);

            $(elem).parent().find('.addTimerBox').find('.timerName').val(newId);
            $(elem).parent().find('.addTimerBox').find('.tHour').val('0');
            $(elem).parent().find('.addTimerBox').find('.tMin').val('0');
            $(elem).parent().find('.addTimerBox').find('.tSec').val('0');

            $(elem).parent().find('.addTimerBox').toggle();
            $(elem).parent().find('.addTimerBox').find('.timerName').focus();
            $('.bOverlay').show();
        }
        function validTime(elem) {

        }
        function closeBox(elem) {
            $(elem).parent().toggle();

            $('.bOverlay').hide();
        }
        function addTimer(elem) {

            var currIndex = $('#indexValTimer').val();
            // alert('currIndex' + currIndex);
            var nextIndex = parseInt(currIndex) + 1;
            //var nextIndex = parseInt(currIndex);

            var newId = "timer" + nextIndex;
            $('#indexValTimer').val(nextIndex);

            var name = $(elem).parent().find('.timerName').val();
            var hr = $(elem).parent().find('.tHour').val();
            var mn = $(elem).parent().find('.tMin').val();
            var sc = $(elem).parent().find('.tSec').val();

            if (name == "" || isNaN(hr) || isNaN(mn) || isNaN(sc) || parseInt(hr) < 0 || parseInt(mn) < 0 || parseInt(mn) > 59 || parseInt(sc) < 0 || parseInt(sc) > 59) {
                $(elem).parent().find('.validMessage').show();
                setTimeout(function () {
                    $(elem).parent().find('.validMessage').hide();
                }, 3000);
                return;
            }
            else {
                var totSec = (parseInt(hr) * 60 * 60) + (parseInt(mn) * 60) + parseInt(sc);
                if (totSec < 1) {
                    $(elem).parent().find('.validMessage').show();
                    setTimeout(function () {
                        $(elem).parent().find('.validMessage').hide();
                    }, 3000);
                    return;
                }

            }


            $(elem).parent().find('.validMessage').hide();

            var timerHtml = "<div id = " + newId + " class='timerWrapper'></div>";
            $(elem).parent().parent().find('.studentTimers').append(timerHtml);

            //addStopwatch(newId);



            var newTimer = $('<div class="container">'
                + '<div class="tBarCont"></div>'
                + '<input type="hidden" id="Hidden1" class="tHour" value="' + hr + '" />'
                + '<input type="hidden" id="Hidden2" class="tMin" value="' + mn + '" />'
                + '<input type="hidden" id="Hidden3" class="tSec" value="' + sc + '" />'
                + '<input type="hidden" class="thid" />'

                + '<input type="button" class="addBtn addBtnB" value="⌛" style="height: 30px;width:25px">'
                + '<input type="text" class="lblText" value="' + name + '" onclick="$(this).select();">'
                + '<input type="text" class="Time-Box" value="' + hr + ':' + mn + ':' + sc + '" onkeypress="return false;">'
                + '<input type="button" class="start-stop" value="►" onclick="startTimer(this)">'
                //+ '<input type="button" class="pause-resume" value="&#10074&#10074" onclick="startTimer(this)" style="display: none">'
                + '<input type="button" class="reset" value="↺">'
                + '<input type="button" onclick="delTimer(this)" class="timDel" value="X">'
                + '</div>');
            $('#' + newId).append(newTimer);
            $(elem).parent().toggle();

            $('.bOverlay').hide();
        }


        function listStudents() {
            $.ajax(
                      {
                          type: "POST",
                          url: "userTimers.aspx/getStudentDetails",
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          async: false,

                          success: function (data) {
                              var returnListArray = (data.d).split('^');
                              var studentId = returnListArray[0];
                              var stdId = studentId.split(';');
                              var studentName = returnListArray[1];
                              var stdName = studentName.split(';');
                              $('#studentList').empty();
                              var sName = "";
                              for (var i = 0; i < stdId.length - 1; i++) {
                                  sName = stdName[i].replace(/'/g, "^");
                                  var stdDivContainer = "<div class='timerStdList' id='stdTmr-" + stdId[i] + "' style='cursor:pointer' title='click to Add Student' onclick='addStudentTab(this,\"" + stdId[i] + "\",\"" + sName + "\")'>" + stdName[i] + "</div>";
                                  $('#studentList').append(stdDivContainer);
                                  //var findDiv = $('#toAddStd');

                              }
                          },
                          error: function (request, status, error) {
                              alert('error');
                          }
                      });
        }

        function showStudents() {
            $('#studentList').toggle();
        }
    </script>
</head>
<body style="margin: 0px;">
    <form id="form1" runat="server">
        <input type="hidden" value="0" id="indexVal" />
        <input type="hidden" value="0" id="indexValTimer" />
        <div id="addStudentTimer" style="height: 20px;" onclick="showStudents()">Add Client</div>
        <div id="studentList"></div>
        <div class="dummyForFreeze"></div>
        <div class="bOverlay"></div>
        <div id="TimerContainer">
            <div id='Div1' class='stdNameContainer'>
            <div class='addTimerBox'>
            <div class='validMessage'>Invalid Values</div>
            <input type='text' class='timerName' onclick='$(this).select();'>
            <input type='text' class='tHour' value='0' onclick='    $(this).select();'  maxlength='2'>
            <input type='text' class='tMin' value='0' onclick='    $(this).select();'  maxlength='2'>
            <input type='text' class='tSec' value='0' onclick='    $(this).select();'  maxlength='2'>
            <input type='button' onclick='    addTimer(this)' value='&#x2713' class='addBtn'>
            <input type='button' class='dltBtn' value='X' onclick='closeBox(this)'></div>
            <div class='studentName' style='cursor:pointer; width:190px;' title='General Timer' onclick='exp_coll(this)'>General Timer</div>
            <input type='button' class='addBtn' value='+&#8987;' onclick='addTimerBox(this)'/>
            <input type='button' class='addBtn' value='+&#x1f550;' onclick='addNewTimer(this)'/>
            <div class='studentTimers'>
            </div> 
            </div>
        </div>
    </form>
</body>
<script>
    $(document).ready(function () {
        listStudents();
    });
</script>
</html>
