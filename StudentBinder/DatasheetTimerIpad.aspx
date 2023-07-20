<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DatasheetTimerIpad.aspx.cs" Inherits="StudentBinder_DatasheetTimerIpad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <title>Untitled Document</title>
    <link href="../Administration/CSS/BehaviorScoreTab.css" rel="stylesheet" id="sized" />
    <script src="../Administration/JS/jquery-1.8.0.js"></script>

    <script type="text/javascript" src="js/eye.js"></script>
    <script type="text/javascript" src="js/layout.js"></script>



    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/jquery.smartTab.js"></script>

    <%--<script type="text/javascript" src="js/cufon-yui.js"></script>--%>
    <%--<script type="text/javascript" src="js/Digital-7_Italic_italic_400.font.js"></script>--%>
    <%--<script type="text/javascript" src="js/cufon-replace.js"></script>--%>

    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />

    <script src="js/StopWatchNew.js"></script>
    <script src="js/fastclick.js"></script>
    <style type="text/css">
        #btnStartStopTimer, .btnFrq {
            -ms-touch-action: none !important;
        }

        #HdrDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 600px;
            height: 190px;
        }

        .txDrp {
            border: 4px solid #0d668e !important;
            border-radius: 10px 10px 10px 10px !important;
            float: left !important;
            height: 35px !important;
            margin: 0% 6px 15px 0 !important;
            min-width: 35px !important;
            padding: 0 2px !important;
            text-align: center !important;
            width: 45% !important;
        }

        .fullOverlay {
            display: none;
            top: 0px;
            left: 0px;
            position: fixed;
            z-index: 2000;
            width: 100%;
            height: 100%;
            background-image: url("../Administration/images/overlay.png");
        }

        #closeHdr {
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

        .poptitle {
            color: #9D9D9D;
            font-family: 'Oswald',sans-serif;
            font-size: 15px;
            font-style: normal;
            font-weight: normal;
            height: auto;
            margin: 0;
            padding: 0;
            text-align: left;
            width: 100%;
        }

        .scrollDiv {
            clear: both;
            display: inline-block;
            float: left;
            list-style-type: none;
            margin: 0 -1px 0 0;
            padding: 0;
            position: inherit;
            z-index: 100;
            overflow-y: scroll;
            overflow-x: hidden;
            direction: ltr;
            height: 595px;
            background: none repeat scroll 0 0 #0D668E;
        }

        .btns {
            background: none repeat scroll 0 0 #60C658;
            border: medium none;
            border-radius: 10px 10px 10px 10px;
            color: #FFFFFF;
            cursor: pointer;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            margin: 5px;
            min-width: 55px;
            padding: 9px;
            position: relative;
            top: -3px;
        }

        .btnYesClick {
            background: none repeat scroll 0 0 #60C658;
            border: medium none;
            border-radius: 10px 10px 10px 10px;
            color: #FFFFFF;
            cursor: pointer;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            margin: 5px;
            min-width: 63px;
            padding: 9px;
            position: relative;
            top: -3px;
        }

        .btnNoClick {
            background: none repeat scroll 0 0 #60C658; /* Red: #FF0000*/
            border: medium none;
            border-radius: 10px 10px 10px 10px;
            color: #FFFFFF;
            cursor: pointer;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            margin: 5px;
            min-width: 63px;
            padding: 9px;
            position: relative;
            top: -3px;
        }

        .btnNothingClick {
            background: none repeat scroll 0 0 #0d668e;
            border: medium none;
            border-radius: 10px 10px 10px 10px;
            color: #FFFFFF;
            cursor: pointer;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            margin: 5px;
            min-width: 63px;
            padding: 9px;
            position: relative;
            top: -3px;
        }

        .btnsGr {
            /*background: #60c658 none repeat scroll 0 0;*/
            background: #0d668e none repeat scroll 0 0;
            border: medium none;
            border-radius: 10px;
            color: #ffffff;
            cursor: pointer;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            margin: 5px;
            min-width: 63px;
            padding: 9px;
            position: relative;
            top: -3px;
        }

        .btnr {
            background: none repeat scroll 0 0 #FF0000;
            border: medium none;
            border-radius: 10px 10px 10px 10px;
            color: #FFFFFF;
            cursor: pointer;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            margin: 5px;
            min-width: 55px;
            padding: 9px;
            position: relative;
            top: -3px;
        }


        .drpcls {
            border: 5px solid #60C658;
            border-radius: 6px;
            font-size: 22px;
            font-weight: bold;
            height: 45px;
            padding: 2px;
            width: 85px;
        }

        input[type="button"] {
            -webkit-appearance: button;
            height: 40px !Important;
        }

        div[id^=dvTimeId] * {
            border: 1px solid #808080;
            clear: both;
            height: 25px;
            padding: 5px 10px 5px;
            font-size: 11px;
        }

        .auto_box1 {
            font-weight: bolder;
            text-align: center;
            vertical-align: bottom;
            width: 50%;
            overflow-y: scroll;
            overflow-x: hidden;
        }

        div[id^=dvTimeId] {
            width: 145px;
            background-color: #CCCCCC;
            border: 1px solid;
            /*position: fixed;*/
            top: 5px;
            /*left: 300px;*/
            z-index: 10001;
            max-height: 100px;
            overflow-y: scroll;
            position:relative;
        }

       .past {
            background-color: #DBDBDB;
            color: #000000;
            position: relative;
        }

        .future {
            background-color: white;
            color: black;
            position: relative;
        }

        .current {
            background-color: #13C179;
            color: white;
            position: relative;
        }
        .bselect {
             background-color: #7abd40 !important; /* Blue: #0d668e !important*/
            color: white;
            position: relative;
        }


        .simple {
            background-color: #ffcc00;
            color: white;
            position: relative;
        }

        .redBox {
            background-color: #0d668e; /* Green:  #7abd40;*/
            color: white;
        }


        /*------- Edit switch button ---------------*/


        .swt {
            position: relative;
            display: inline-block;
            vertical-align: top;
            width: 66px;
            height: 20px;
            padding: 3px;
            /*background-color: white;*/
            border-radius: 18px;
            /*box-shadow: inset 0 -1px white, inset 0 1px 1px rgba(0, 0, 0, 0.05);*/
            cursor: pointer;
            /*background-image: -webkit-linear-gradient(top, #eeeeee, white 25px);
  background-image: -moz-linear-gradient(top, #eeeeee, white 25px);
  background-image: -o-linear-gradient(top, #eeeeee, white 25px);
  background-image: linear-gradient(to bottom, #eeeeee, white 25px);*/
        }

        .swt_inp {
            position: absolute;
            top: 0;
            left: 0;
            opacity: 0;
        }

        .swt_lbl {
            font-family: Arial;
            position: relative;
            display: block;
            height: inherit;
            font-size: 10px;
            text-transform: uppercase;
            background: #0D668E;
            border-radius: inherit;
            box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.12), inset 0 0 2px rgba(0, 0, 0, 0.15);
            -webkit-transition: 0.15s ease-out;
            -moz-transition: 0.15s ease-out;
            -o-transition: 0.15s ease-out;
            transition: 0.15s ease-out;
            -webkit-transition-property: opacity background;
            -moz-transition-property: opacity background;
            -o-transition-property: opacity background;
            transition-property: opacity background;
        }

            .swt_lbl:before, .swt_lbl:after {
                position: absolute;
                top: 50%;
                margin-top: -.5em;
                line-height: 1;
                -webkit-transition: inherit;
                -moz-transition: inherit;
                -o-transition: inherit;
                transition: inherit;
            }

            .swt_lbl:before {
                content: attr(data-off);
                right: 20px;
                color: white;
                text-shadow: 0 1px rgba(255, 255, 255, 0.5);
            }

            .swt_lbl:after {
                content: attr(data-on);
                left: 15px;
                color: white;
                text-shadow: 0 1px rgba(0, 0, 0, 0.2);
                opacity: 0;
            }

        .swt_inp:checked ~ .swt_lbl {
            background: #47a8d8;
            box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.15), inset 0 0 3px rgba(0, 0, 0, 0.2);
        }

            .swt_inp:checked ~ .swt_lbl:before {
                opacity: 0;
            }

            .swt_inp:checked ~ .swt_lbl:after {
                opacity: 1;
            }

        .swt-handle {
            display:none;
            position: absolute;
            top: 4px;
            left: 4px;
            width: 18px;
            height: 18px;
            background: white;
            border-radius: 10px;
            box-shadow: 1px 1px 5px rgba(0, 0, 0, 0.2);
            background-image: -webkit-linear-gradient(top, white 40%, #f0f0f0);
            background-image: -moz-linear-gradient(top, white 40%, #f0f0f0);
            background-image: -o-linear-gradient(top, white 40%, #f0f0f0);
            background-image: linear-gradient(to bottom, white 40%, #f0f0f0);
            -webkit-transition: left 0.15s ease-out;
            -moz-transition: left 0.15s ease-out;
            -o-transition: left 0.15s ease-out;
            transition: left 0.15s ease-out;
        }

            .swt-handle:before {
                content: '';
                position: absolute;
                top: 50%;
                left: 50%;
                margin: -6px 0 0 -6px;
                width: 12px;
                height: 12px;
                background: #f9f9f9;
                border-radius: 6px;
                box-shadow: inset 0 1px rgba(0, 0, 0, 0.02);
                background-image: -webkit-linear-gradient(top, #eeeeee, white);
                background-image: -moz-linear-gradient(top, #eeeeee, white);
                background-image: -o-linear-gradient(top, #eeeeee, white);
                background-image: linear-gradient(to bottom, #eeeeee, white);
            }

        .swt_inp:checked ~ .swt-handle {
            left: 50px;
            box-shadow: -1px 1px 5px rgba(0, 0, 0, 0.2);
        }

        .swt-green > .swt_inp:checked ~ .swt_lbl {
            background: #4fb845;
        }

        /*------------------------------------------*/
    </style>
    <script type="text/javascript">
        //window.addEventListener('load', function () {
        //    new FastClick(document.body);
        //}, false);
    </script>
    <script type="text/javascript">

        var AutoSave = setInterval(function () { AutoSaveEveryHalfHour() }, 1800000);

        var pageWidth = $(window).width();
        var msgLeft = ((pageWidth / 2) - 50);

        var triggerEvntStatus = "";



        function adjustStyle() {
            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {
                //alert("hai");
                //$('#btnStartStopTimer').css("-webkit-appearance", 'button');
                //$('#btnStartStopTimer').css("height", '50px');
                //$('#btnStartStopTimern').css("font-size", '18px');

                $('.mainHeader').css("width", '90.5%');
                $("#IfrmTimer").css("min-height", "");
                $('#IfrmTimer').css("height", "auto");
            }
            else {
                $('.mainHeader').css("width", '348%');
                $('.Time-Box').addClass('Time-Box2');
            }

        }



        function loadTopTabs(id, valu) {
            triggerEvntStatus = "";
            //alert(id);
            //alert($('.sel').length);
            $('#hdnchecktabchange').val('false');
            $('#hdnAutoSave').val('False');
            if ($('.sel').length > 0) {
                var prevSelectedTab = $('.sel').attr('id').split('-')[1];
                var NewlySelectedTab = valu.id.split('-')[1];
                var TabIdName = 'tabs-' + prevSelectedTab;
                $("#" + TabIdName).find(".btns").trigger("click");

                $(".btns").click(function (event) {
                    if (event.originalEvent === undefined) {
                        //alert('not human')
                        triggerEvntStatus = "true";
                    } else {
                        //alert(' human');
                        triggerEvntStatus = "false";
            }
                });
            }
            if (id == 1) {

                var topTrue = $('#tabUl').find('.topTabTrue');
                $('#' + $(topTrue[0]).attr('id')).trigger("click");

                //show all true class only
                $('.topTabTrue').show();
                $('.topTabFalse').hide();

                //Set the acc/dcc selected
                $('.topTabtd').removeClass('topTabtdSelected');
                $('#topTab1').addClass('topTabtdSelected');


                //var prevSelectedTab = $('.sel').attr('id').split('-')[1];

                //var TabIdName = 'tabs-' + prevSelectedTab;
                //$("#" + TabIdName).find(".btns").trigger("click");



                if ($('.topTabTrue').length < 1) {

                    $('.rightPartContainerTimerNullVal').show();
                    $('.rightPartContainerTimer').hide();
                }
                else {
                    $('.rightPartContainerTimerNullVal').hide();
                    $('.rightPartContainerTimer').show();
                }


            } else if (id == 2) {

                var topFalse = $('#tabUl').find('.topTabFalse');
                $('#' + $(topFalse[0]).attr('id')).trigger("click");

                $('.topTabFalse').show();
                $('.topTabTrue').hide();
                $('.topTabtd').removeClass('topTabtdSelected');
                $('#topTab2').addClass('topTabtdSelected');
                //alert('dcc:' + $(topFalse).length);

                if ($('.topTabFalse').length < 1) {

                    $('.rightPartContainerTimerNullVal').show();
                    $('.rightPartContainerTimer').hide();
                }
                else {
                    $('.rightPartContainerTimerNullVal').hide();
                    $('.rightPartContainerTimer').show();
                }

            }
            triggerEvntStatus = "false";
        }

        function loadTopTabs2(id) {

            $('#hdnchecktabchange').val('false');

            if (id == 1) {

                var topTrue = $('#tabUl').find('.topTabTrue');
                $('#' + $(topTrue[0]).attr('id')).trigger("click");

                //show all true class only
                $('.topTabTrue').show();
                $('.topTabFalse').hide();

                //Set the acc/dcc selected
                $('.topTabtd').removeClass('topTabtdSelected');
                $('#topTab1').addClass('topTabtdSelected');

                if ($('.topTabTrue').length < 1) {

                    $('.rightPartContainerTimerNullVal').show();
                    $('.rightPartContainerTimer').hide();
                }
                else {
                    $('.rightPartContainerTimerNullVal').hide();
                    $('.rightPartContainerTimer').show();
                }

            } else if (id == 2) {

                var topFalse = $('#tabUl').find('.topTabFalse');
                $('#' + $(topFalse[0]).attr('id')).trigger("click");

                $('.topTabFalse').show();
                $('.topTabTrue').hide();
                $('.topTabtd').removeClass('topTabtdSelected');
                $('#topTab2').addClass('topTabtdSelected');

                if ($('.topTabFalse').length < 1) {

                    $('.rightPartContainerTimerNullVal').show();
                    $('.rightPartContainerTimer').hide();
                }
                else {
                    $('.rightPartContainerTimerNullVal').hide();
                    $('.rightPartContainerTimer').show();
                }
            }
        }


        //function AutoSaveEveryHalfHour()
        //{
        //    $('.stContainer').children('div').each(function () {
        //        var TabId = this.id.split('-')[1]; // "this" is the current element in the loop
        //        saveDurationOther(TabId);
        //        saveCount(TabId);
        //        saveCountOther(TabId);
        //        var item = 'tabs-' + TabId;
        //        var thisval = document.getElementById(item);
        //        saveDuraFreq(TabId, thisval);
        //    });


        //}

        function AutoSaveEveryHalfHour() {
            $('#hdnchecktabchange').val('false');
            var tabContentList = $('.stContainer').find('.tabContent');
            for (var i = 0; i < tabContentList.length; i++) {
                if ($(tabContentList[i]).css('display') == "block") {
                    $(tabContentList[i]).find('.btns').trigger("click");

                    //alert(currButton.css('display'));
                }

            }

            //.each(function () {
            //    $(thi).css
            //    style = "display: block;"
            //    var TabId = this.id.split('-')[1]; // "this" is the current element in the loop
            //    if ($(this).find("h4").html() == "Duration") {
            //        saveDurationOther(TabId);
            //        var item = 'tabs-' + TabId;
            //        var thisval = document.getElementById(item);
            //        saveDuraFreq(TabId, thisval);
            //    }
            //    else if ($(this).find("h4").html() == "Frequency") {
            //        saveCount(TabId);
            //        saveCountOther(TabId);
            //    }

            //    if ($(this).find("#Div1").length > 0) {
            //        saveCount(TabId);
            //        saveCountOther(TabId);
            //    }
            //    //alert($(this).find("h4").html());




            //});


        }

        function chktimerchanged(para, id) {
            if (para.checked) {
                //custom time
                $('#testtimer2' + id).show();
                $('#testtimer' + id).hide();
            } else {
                //timer
                $('#testtimer' + id).show();
                $('#testtimer2' + id).hide();
            }
        }
        function editTOE(elem) {
            if ($(elem).prop('checked') == true) {
                $(elem).parent().parent().find('.TOE_timer').hide();
                $(elem).parent().parent().find('.TOE_edit').show();
                $(elem).parent().parent().find('.divTimeEvt').show();
            }
            else {
                $(elem).parent().parent().find('.TOE_timer').show();
                $(elem).parent().parent().find('.TOE_edit').hide();
                $(elem).parent().parent().find('.divTimeEvt').hide();
            }

        }
        var t;
        function startTime() {
            var today = new Date();
            var h = today.getHours();
            var ampm = (h < 12) ? 'AM' : 'PM';
            h = (h > 12) ? (h - 12) : h;
            var m = today.getMinutes();
            var s = today.getSeconds();
            h = checkTime(h);
            m = checkTime(m);
            s = checkTime(s);
            $('.TOE_timer').val(h + ":" + m + ":" + s + " " + ampm);
            t = setTimeout(startTime, 500);
        }
        function checkTime(i) {
            if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
            return i;
        }

        setInterval(function () {
            // ashin

            var hdnTOEs = $('.hdnTOE'); // hidden field list
            var hdnToesCount = $(hdnTOEs).length;


            if (hdnToesCount > 0) {

                for (var i = 0; i < hdnToesCount; i++) { // loopin hiddenfield

                    var hdnToeId = $(hdnTOEs[i]).attr('id');
                    var behaveid = hdnToeId.split('_')[1];

                    var timeList = $('#hdnTOE_' + behaveid).val().split(';'); // timeList array


                    for (var j = 1; j < timeList.length - 1 ; j++) { // looping timeList array


                        var obj_currDate = new Date();
                        var currMin = obj_currDate.getMinutes();
                        var currHr = obj_currDate.getHours();

                        if (parseInt(currMin) < 10) {
                            currMin = "0" + currMin.toString();
                        }
                        var currTime = currHr.toString() + currMin.toString();

                        //var div = '<div id="testDiv" style="float:left;width:100%;background-color:yellow;width:200px;">' + hdnToeId + ')' + timeList[j] + '/' + currTime + '</div>';
                        //$('#msgDivtop').append(div);
                        // $('#hdnFldIsUpdate').val('False');
                        if (parseInt(timeList[0]) <= parseInt(currTime)) {
                            if (parseInt(timeList[j]) >= parseInt(currTime)) {

                                if ((parseInt(timeList[j]) == parseInt(currTime))) {

                                    if ($('#div' + behaveid + '_' + timeList[j]).parent().find('.bselect').length == 0) {

                                        $('#div' + behaveid + '_' + timeList[j]).parent().find('.current').removeClass('current').removeClass('simple').removeClass('bselect').addClass('past');
                                        $('#div' + behaveid + '_' + timeList[j]).addClass('current').addClass('simple').removeClass('future').removeClass('past');
                                        $('#div' + behaveid + '_' + timeList[j]).parent().find('.current,.past').css('border-color', 'black');


                                        $('#div' + behaveid + '_' + timeList[j]).trigger('click');

                                        var scrTop = $('#dvTimeId_' + behaveid).scrollTop() + $('#div' + behaveid + '_' + timeList[j]).position().top;

                                        $('#dvTimeId_' + behaveid).animate({ scrollTop: scrTop - 30 }, "fast");
                                    }
                                        //else if ($('#div' + behaveid + '_' + timeList[j]).find('.bselect').length > 0) {
                                        //    if ($('#div' + behaveid + '_' + timeList[j]).find('.bselect').hasClass('.current')) {
                                        //        $('#div' + behaveid + '_' + timeList[j]).parent().find('.current').removeClass('current').removeClass('simple').removeClass('bselect').addClass('past');
                                        //        $('#div' + behaveid + '_' + timeList[j]).addClass('current').addClass('simple').removeClass('future').removeClass('past');
                                        //        $('#div' + behaveid + '_' + timeList[j]).parent().find('.current,.past').css('border-color', 'black');


                                        //        $('#div' + behaveid + '_' + timeList[j]).trigger('click');

                                        //        var scrTop = $('#dvTimeId_' + behaveid).scrollTop() + $('#div' + behaveid + '_' + timeList[j]).position().top;

                                        //        $('#dvTimeId_' + behaveid).animate({ scrollTop: scrTop - 30 }, "fast");
                                        //    }
                                        //}
                                    else {
                                        $('#div' + behaveid + '_' + timeList[j]).addClass('past').removeClass('future');
                                        $('#div' + behaveid + '_' + timeList[j]).parent().find('.current,.past').css('border-color', 'black');
                                    }


                                }
                                if ((parseInt(timeList[j]) > parseInt(currTime))) {
                                    if ($('#div' + behaveid + '_' + timeList[j]).parent().find('.bselect').length == 0) {
                                        $('#div' + behaveid + '_' + timeList[j - 1]).parent().find('.current').removeClass('current').removeClass('simple').removeClass('bselect').addClass('past');
                                        $('#div' + behaveid + '_' + timeList[j - 1]).addClass('current').addClass('simple').removeClass('future').removeClass('past');
                                        $('#div' + behaveid + '_' + timeList[j - 1]).parent().find('.current,.past').css('border-color', 'black');
                                        //$('#div' + behaveid + '_' + timeList[j - 1]).trigger('click');
                                        //setTimeEvent($('#div' + behaveid + '_' + timeList[j]), behaveid);
                                        var scrTop = $('#dvTimeId_' + behaveid).scrollTop() + $('#div' + behaveid + '_' + timeList[j - 1]).position().top;
                                        $('#dvTimeId_' + behaveid).animate({ scrollTop: scrTop - 30 }, "fast");


                                    }
                                }
                                break;
                            }
                        }

                    }

                }

            }

        }, 30000);


        //hari

        function timerCalculate(interval, stime, etime, behaveId, period, PrevSaveList) {

            var stym = [];
            var etym = [];
            stym = stime.split(':');
            etym = etime.split(':');

            var starttym = parseInt(((stym[0]) * 60)) + parseInt((stym[1]));
            var endtym = parseInt(((etym[0]) * 60)) + parseInt((etym[1]));
            if (endtym > starttym) {
                setTimeDiv(interval, starttym, endtym, behaveId, period, PrevSaveList);
            }

            //alert(interval);
            //alert(stime);
            //alert(etime);
        }

        function setTimeDiv(int_vel, stime, etime, behaveId, period, PrevSaveList) {

            var nowTime = new Date();
            var interval = parseInt(int_vel);
            var startTime = parseInt(stime);
            var endTime = parseInt(etime);
            var curntHrs = (nowTime.getHours()) * 60;
            var curntMins = nowTime.getMinutes();
            var totalMin = curntHrs + curntMins;
            var int_list = [];


            if (endTime > startTime) {

                var sHr = Math.floor((startTime / 60));
                var sMin = (startTime % 60);
                var eHr = Math.floor(endTime / 60);
                var eMin = (endTime % 60);
                var AmPm = "";
                var sHr_24 = sHr;

                if (sHr >= 12) {
                    sHr = sHr - 12;
                    AmPm = "PM";
                }
                else {
                    if (sHr == 12) {
                        AmPm = "PM";
                    }
                    else
                        AmPm = "AM";
                }

                //Calculate Period 
                var strtPer = Math.floor(startTime) + Math.floor(period);
                var sHrPer = Math.floor(strtPer / 60);
                var sMinPer = Math.floor(strtPer % 60);
                var AmPmPer = "";

                if (sHrPer >= 12) {
                    sHrPer = sHrPer - 12;
                    AmPmPer = "PM";
                }
                else {
                    if (sHrPer == 12) {
                        AmPmPer = "PM";
                    }
                    else
                        AmPmPer = "AM";
                }

                if (parseInt(sMin) < 10) {
                    sMin = "0" + sMin;
                }

                if (parseInt(sMinPer) < 10) {
                    sMinPer = "0" + sMinPer;
                }
                if (parseInt(sHr) < 10) {
                    sHr = "0" + sHr;
                }
                if (parseInt(sHrPer) < 10) {
                    sHrPer = "0" + sHrPer;
                }
                if (parseInt(sHr_24) < 10) {
                    sHr_24 = "0" + sHr_24;
                }

                var firstClass = "";

                if (PrevSaveList != "") {
                    var split = PrevSaveList.split('#');

                    if (split.length > 0) {
                        for (var i = 0; i < split.length; i++) {
                            if (split[i] == (behaveId + '_' + sHr_24.toString() + sMin.toString())) {
                                firstClass = " redBox";
                            }
                        }
                    }

                }


                $('#dvTimeId_' + behaveId).append('<div class="future' + firstClass + '" id="div' + behaveId + '_' + sHr_24.toString() + sMin.toString() + '" onclick="execute(this,' + behaveId + ');">' + sHr + ':' + sMin + ' ' + AmPm + ' - ' + sHrPer + ':' + sMinPer + ' ' + AmPmPer + '</div>');
                int_list[0] = sHr_24.toString() + sMin.toString();

                for (var i = startTime, j = 1; i < endTime; i++, j++) {
                    var calcuTime = parseInt(i + interval);

                    var temp = startTime;
                    var tempCalc = calcuTime;
                    var initialHr = Math.floor((calcuTime / 60));
                    var initialMin = calcuTime % 60;
                    var initialHr_24 = initialHr;

                    //calculate period time

                    var calcuPer = Math.floor(calcuTime) + Math.floor(period);
                    var initialHrPer = Math.floor((calcuPer / 60));
                    var initialMinPer = Math.floor((calcuPer % 60));

                    i = (i + interval) - 1;

                    if (i <= endTime) {
                        if (initialHr > 12) {
                            initialHr = initialHr - 12;
                            AmPm = "PM";

                        }
                        else {
                            if (initialHr == 12) {
                                AmPm = "PM";
                            }
                            else
                                AmPm = "AM"
                        }


                        if (parseInt(initialMin) < 10) {
                            initialMin = "0" + initialMin;
                        }
                        if (parseInt(initialHr) < 10) {
                            initialHr = "0" + initialHr;
                        }
                        if (parseInt(initialHr_24) < 10) {
                            initialHr_24 = "0" + initialHr_24;
                        }

                        //calculate period time Am-Pm initialHr_24

                        if (initialHrPer > 12) {
                            initialHrPer = initialHrPer - 12;
                            AmPmPer = "PM";

                        }
                        else {
                            if (initialHrPer == 12) {
                                AmPmPer = "PM";
                            }
                            else
                                AmPmPer = "AM"
                        }


                        if (parseInt(initialMinPer) < 10) {
                            initialMinPer = "0" + initialMinPer;
                        }
                        if (parseInt(initialHrPer) < 10) {
                            initialHrPer = "0" + initialHrPer;
                        }

                        firstClass = "";

                        if (PrevSaveList != "") {
                            var split = PrevSaveList.split('#');

                            if (split.length > 0) {
                                for (var w = 0; w < split.length; w++) {
                                    if (split[w] == (behaveId + '_' + initialHr_24.toString() + initialMin.toString())) {
                                        firstClass = " redBox";
                                    }
                                }
                            }

                        }


                        $('#dvTimeId_' + behaveId).append('<div class="future' + firstClass + '" id="div' + behaveId + '_' + initialHr_24.toString() + initialMin.toString() + '"onclick="execute(this,' + behaveId + ');">' + initialHr + ':' + initialMin + ' ' + AmPm + ' - ' + initialHrPer + ':' + initialMinPer + ' ' + AmPmPer + ' ');
                        // $('#div_' + initialHr_24.toString() + initialMin.toString()).attr('disabled', true);
                        int_list[j] = initialHr_24.toString() + initialMin.toString();
                    }
                }

                var intListSting = "";
                for (var i = 0; i < int_list.length; i++) {

                    intListSting += int_list[i] + ";";
                }
                $('#hdnTOE_' + behaveId).val(intListSting);
                //pramod
                var obj_currDate = new Date();
                var currMin = obj_currDate.getMinutes();
                if (parseInt(currMin) < 10) {
                    curntMins = "0" + currMin;
                }
                var currTime = obj_currDate.getHours().toString() + curntMins;

                for (var i = 0; i < int_list.length; i++) {
                    if (parseInt(int_list[i]) < parseInt(currTime)) {
                        $('#div' + behaveId + '_' + int_list[i]).removeClass('future');
                        $('#div' + behaveId + '_' + int_list[i]).addClass('past');
                        //$('#div_' + int_list[i]).attr("disabled", false);
                    }
                    else {
                        if (parseInt(int_list[i]) == parseInt(currTime)) {
                            $('#div' + behaveId + '_' + int_list[i]).removeClass('future');
                            $('#div' + behaveId + '_' + int_list[i]).addClass('current').addClass('simple');


                            var scrTop = $('#dvTimeId_' + behaveId).scrollTop() + $('#div' + behaveId + '_' + int_list[i]).position().top;
                            $('#dvTimeId_' + behaveId).animate({ scrollTop: 20 }, "fast");


                            var time = $('.current').text();

                            var splitTime = time.split(':');
                            var splitFormat = time.split(' ');
                            var splitHr = splitTime[0];
                            var splitMinTemp = splitTime[1].split(' ');
                            var splitMin = splitMinTemp[0];
                            var splitAmPm = (splitFormat[1] == 'AM') ? '0' : '1';

                            $('#txtTimeHr' + behaveId).val(splitHr);
                            $('#txtTimeMin' + behaveId).val(splitMin);
                            $('#txtTimeSec' + behaveId).val("00");
                            $('#drpTimeAmPm' + behaveId).val(splitAmPm);



                        } else {
                            if (i > 0) {
                                $('#div' + behaveId + '_' + int_list[i - 1]).removeClass('past');
                                $('#div' + behaveId + '_' + int_list[i - 1]).addClass('current').addClass('simple');

                                var scrTop = $('#dvTimeId_' + behaveId).scrollTop() + $('#div' + behaveId + '_' + int_list[i - 1]).position().top;
                                $('#dvTimeId_' + behaveId).animate({ scrollTop: 20 }, "fast");

                                if ($('.current').length > 0) {
                                    var time = $('.current').text();
                                }
                                else {
                                    var ct = new Date();
                                    var cur_h = ct.getHours();
                                    var cur_m = ct.getMinutes();
                                    var cur_ap = "AM";
                                    if (cur_h > 12) {
                                        cur_h = cur_h - 12;
                                        cur_ap = "PM";
                                    }
                                    var time = cur_h.toString() + ":" + cur_m.toString() + " " + cur_ap.toString();
                                }

                                var splitTime = time.split(':');
                                var splitFormat = time.split(' ');
                                var splitHr = splitTime[0];
                                var splitMinTemp = splitTime[1].split(' ');
                                var splitMin = splitMinTemp[0];
                                var splitAmPm = (splitFormat[1] == 'AM') ? '0' : '1';

                                $('#txtTimeHr' + behaveId).val(splitHr);
                                $('#txtTimeMin' + behaveId).val(splitMin);
                                $('#txtTimeSec' + behaveId).val("00");
                                $('#drpTimeAmPm' + behaveId).val(splitAmPm);
                            }
                            else {
                                break;
                            }
                        }

                        break;
                    }
                }

                //end

                //for (var i = startTime; i < endTime; i++) {

                //    var calcuTime = i + interval;
                //    var tempory = i;
                //    var temp = startTime;
                //    var tempCalc = calcuTime;
                //    var initialHr = Math.floor((calcuTime / 60));
                //    var initialMin = calcuTime % 60;
                //    i = (i + interval) - 1;
                //    if (initialHr > 12) {
                //        initialHr = initialHr - 12;
                //        AmPm = "PM";

                //    }
                //    else {
                //        if (initialHr == 12) {
                //            AmPm = "PM";
                //        }
                //        else
                //            AmPm = "AM"
                //    }



                //    for (var a = tempory; a < calcuTime; a++) {
                //        if (a == totalMin) {
                //            $('#div_' + initialHr + '_' + initialMin).css('background-color', 'red');
                //            //$('#div_' + initialHr + '_' + initialMin).prevAll().addClass("before");
                //            //$('#dvTimeId div').slice(, 4).css('background-color', '#007ACC');
                //        }
                //    }
                //}
            }
        }
        //Updated by jis

        function execute(elm, behaveId) {

            $('#hdnchecktabchange').val('True');
            if (!$(elm).hasClass('future')) {
                resetOther(elm);
                $(elm).parent().parent().parent().find('#butSavReset').find('.btns').val('Save');
                var time = $(elm).text();
                //var f=$('.current').text();
                //alert("f:" + f);
                var splitTime = time.split(':');
                var splitFormat = time.split(' ');
                var splitHr = splitTime[0];
                var splitMinTemp = splitTime[1].split(' ');
                var splitMin = splitMinTemp[0];
                var splitAmPm = (splitFormat[1] == 'AM') ? '0' : '1';
                //alert("splitMin" + splitMin);
                $('#txtTimeHr' + behaveId).val(splitHr);
                $('#txtTimeMin' + behaveId).val(splitMin);
                $('#txtTimeSec' + behaveId).val("00");
                $('#drpTimeAmPm' + behaveId).val(splitAmPm);

                //alert($('#hdnFirstClick_' + behaveId).val());
                //if ($('#hdnFirstClick_' + behaveId).val() == "False") {

                //if ($(elm).hasClass('bselect')) { /* Commented for avoid deselection */
                //    $(elm).parent().find('.current').removeClass('current').removeClass('simple').removeClass('bselect').addClass('past');
                //    $(elm).parent().find('.current,.past').css('border-color', 'black');
                //    if ($(elm).hasClass('redBox')) {
                //        return;
                //    }
                //    //// setInterval();
                //}
                //else {   /* Commented for avoid deselection */
                //  ////resetOther(elm);
                    $(elm).parent().find('.current').removeClass('current').removeClass('simple').removeClass('bselect').addClass('past');
                    $(elm).addClass('current').addClass('simple').removeClass('future').addClass('bselect').removeClass('past');
                    $(elm).parent().find('.current,.past').css('border-color', 'black');
                //}   /* Commented for avoid deselection */
                ////alert("txtTimeHr" + splitAmPm).val();
                ////}
                ////$('#hdnFirstClick_' + behaveId).val("False")
                }
            if ($(elm).hasClass('redBox')) {
                var time = $(elm).text();
                var splitTime = time.split('-');
                var stdid = document.getElementById('hdnFldStudentId').value;
                var sTime = splitTime[0];
                var eTime = splitTime[1];
                var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                $.ajax(
                {

                    type: "POST",
                    url: "dataSheetTimer.aspx/loadBehaviourData",
                    data: "{'MeasurementId':'" + behaveId + "','StudentId':'" + stdid + "','StartTime':'" + sTime + "','EndTime':'" + eTime + "','chkIOA':'" + chkIOA + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {

                        var items = JSON.parse(data.d);



                        var len = items.length;
                        var freq;
                        var dur;
                        if (len > 0) {
                            for (var i = 0; i < len; i++) {
                                if (items[i].Status == "Frequency") {
                                    freq = items[i].FrequencyCount;
                                    $(elm).parent().parent().parent().find('.txDrp').val(freq);
                                }
                                else if (items[i].Status == "YesNo") {
                                    if (items[i].YesOrNo) {
                                        btnYesClicked(behaveId);
                                    }
                                    else
                                        btnNoClicked(behaveId);
                                }
                                else if (items[i].Status == "Duration") {
                                    dur = items[i].Duration;

                                    fillDuration(elm, dur);
                                    $('#chktimer' + behaveId).trigger('click');
                                    $('#testtimer2' + behaveId).show();
                                    $('#testtimer' + behaveId).hide();
                                    // $('#chktimer' + behaveId).prop("checked", false);
                                    //var chkEditTimer = $(elm).parent().parent().parent().find('.swt_inp');
                                    //$(elm).parent().parent().parent().find('.swt').find('input').trigger('click');
                                    // chktimerchanged(chkEditTimer, behaveId);


                                }

                            }
                            // $('#butSavReset').find('.btns ').val('Update');
                            $(elm).parent().parent().parent().find('#butSavReset').find('.btns').val('Update');
                            $('#hdnFldIsUpdate').val('True');
                        }


                        //$.each(items, function (index, val) {
                        //    var behavId = val.BehaviourId;
                        //    var behavFreq = val.FrequencyCount;
                        //    var behavDur = val.Duration;
                        //    var behavYesNo = val.YesOrNo;

                        //});
                    },
                    error: function (request, status, error) {
                    }


                });
            }
        }





        //hari ends


        $(document).ready(function () {

            // Smart Tab
            $('#tabUl').add('innerHTML', '');
            $('#tabs').add('innerHTML', '');

            var IsAcc = "True";
            var ulValue = document.getElementById('hdnFldUl').value.split('@#$');
            var ulId = document.getElementById('hdnFldUlId').value.split('@#$');
            var behavType = document.getElementById('hdnFldBehaveType').value.split('@#$');
            var FrequencyCount = document.getElementById('hdnFldFrequencyCnt').value.split('@#$');
            var IsAcceleration = document.getElementById('hdnFldIsAcceleration').value.split('@#$');
            var BehavDefinition = document.getElementById('hdnFldBehavDefinition').value.split('@#$');
            var BehavStrategy = document.getElementById('hdnFldBehavStrategy').value.split('@#$');
            var StartTime = document.getElementById('hdnFldStartTime').value.split('@#$');
            var EndTime = document.getElementById('hdnFldEndTime').value.split('@#$');
            var Interval = document.getElementById('hdnFldInterval').value.split('@#$');
            var Period = document.getElementById('hdnFldPeriod').value.split('@#$');

            var BeahvId = document.getElementById('hdnFldBehavId').value.split('@#$');
            var TOEId = document.getElementById('hdnFldTOE').value.split('@#$');

            var PrevSaveList = document.getElementById('hdnFldSavedTimes').value.split('@#$');




            for (var BehavCount = 0; BehavCount < ulId.length - 1; BehavCount++) {
                if (IsAcceleration[BehavCount] == "False") {
                    IsAcc = "False";
                } else {
                    IsAcc = "True";
                }

                $('#tabUl').append('<li><a id="t-' + ulId[BehavCount] + '" class="topTab' + IsAcc + '" onclick="SelectBehaviorTab(this);" href="#tabs-' + ulId[BehavCount] + '">' + ulValue[BehavCount] + '</a></li>');


                if (Interval[BehavCount] == "" || Period[BehavCount] == "") {
                    // IF INTERVAL AND PERIOD ARE NOT SAME
                    if (behavType[BehavCount] == 'Duration') {

                        //<h4>' + ulValue[BehavCount] + '</h4>
                        $('#tabs').append('   <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                            //+ '<input type="checkbox"  >Edit'

                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'



                            + '<div style="margin-bottom: 70px;" id="testtimer'
                            + ulId[BehavCount] + '">'
                            + '</div>'
                            + '<div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,'
                            + ulId[BehavCount] + '"  value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />&nbsp;'
                            + '</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:140px;">'
                            //start
                            + '<div>'
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            //+ '<input type="checkbox"  class="editTOE swt_inp" />Edit'

                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'

                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" onclick="timerFunction();" style="border: 3px solid green; width: 80%;"/>'
                           // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                           + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'

                            + '<input maxLength="2" id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:275px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div id="Div11" class="tabContent4" style="top:305px; border: 0px;">'
                            //start
                           + '<div id="butSavReset" style="margin-top:30px"><input type="button" id="btnDuration,' + ulId[BehavCount] + '" class="btns save" value="Save"/>'
                            + '<input type="button" class="btnr reset" value="Reset"  />'
                            + '</div>'
                            //end
                            + '</div>'

                            //start
                            + '<div id="Div11" class="tabContent4" style="top:460px; border: 0px;">'
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');


                        //else {
                        //    //<h4>' + ulValue[BehavCount] + '</h4>
                        //    $('#tabs').append('   <div><div id="tabs-' + ulId[BehavCount] + '"><h4>Duration</h4><input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit<div style="margin-bottom: 70px;" id="testtimer'
                        //        + ulId[BehavCount] + '"></div><div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                        //        + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="0" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                        //        + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="0" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                        //        + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="0" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                        //        + '<input id="txtDuration,'
                        //        + ulId[BehavCount] + '"  value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />&nbsp;</div>'

                        //        + '<div>'

                        //        + '<div style="width:68%;float:left;">Time of Event</div>'
                        //        + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE"  onclick="return false;" checked = "checked"/>Edit'
                        //        + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                        //       // + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" onclick="timerFunction();" style="border: 3px solid green; width: 90%;"/>'
                        //       // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'

                        //       + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:143px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                        //       + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="width:26px;border:0px;text-align:center;" type="text"/>:'
                        //        + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="width:26px;border:0px;text-align:center;" type="text"/>:'
                        //        + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="width:26px;border:0px;text-align:center;" type="text"/>'
                        //        + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                        //         + '</div>'
                        //        + '</div>'
                        //        + '<div id="dvTimeId_'+ulId[BehavCount]+'" class="auto_box1"></div>'

                        //       + '<div id="butSavReset" style="margin-top:30px"><input type="button" id="btnDuration,' + ulId[BehavCount] + '" class="btns save" value="Save"/>'
                        //        + '<input type="button" class="btnr reset" value="Reset"  />'
                        //        + '</div>'
                        //        + '<div id="Div11" class="tabContent3" style="margin-top: -50%!important;"><b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                        //        + '</div></div>');



                        //    timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount],ulId[BehavCount]);
                        //}
                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');

                    }
                    else if (behavType[BehavCount] == 'DuraFreq') {

                        //$('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '"><h4>' + ulValue[BehavCount] + '</h4><div style="margin-bottom: 92px" id="testtimer'
                        //    + ulId[BehavCount] + '"></div><h4>Duration (HH:MM:SS)</h4><div><input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="00" class="tx splitSec" style="width:40px; min-width:25px;" type="text"/>'
                        //    + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="00" class="tx splitSec" style="width:25px; min-width:40px;" type="text"/>'
                        //    + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="00" class="tx splitSec" style="width:25px; min-width:40px;" type="text"/>'
                        //    +'<input id="txtDuration,' + ulId[BehavCount]
                        //    + '" value="0" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'
                        //    + '</div><div id="Div1"><h4>'
                        //    + ulValue[BehavCount] + '</h4><div class="tapCount"><h4 > <label id="lblFrqCount' + ulId[BehavCount]
                        //    + '" value="sadadfa" class="lb lblFreq" style="background:transparent;color:Red;border:0px;width:50% !important;margin-left: 10px; text-align: left; font-size: 25px;">'
                        //    + '0</label></h4></div><input id="btnFrqCntSave,' + savDurResetulId[BehavCount]
                        //    + '" name="" type="button" class="btnFrq"  value="Count" onclick="count(this)"  /><h4>Frequency</h4><input id="txtFrequency,' + ulId[BehavCount]
                        //    + '" class="tx txtFreq" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'

                        //    + '</div><input id="saveDuraFreq,'+ulId[BehavCount]+'" type="button" class="btns save" value="Save" />'
                        //    + '<input type="button" class="btns reset" value="Reset"/></div></div>');

                        //<h4>' + ulValue[BehavCount] + '</h4>  <h4>' + ulValue[BehavCount] + '</h4>
                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                            //+ '<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'

                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'

                            + '<div style="margin-bottom: 70px" id="testtimer'
                            + ulId[BehavCount] + '">'
                            + '</div>'
                            + '<div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,' + ulId[BehavCount]
                            + '" value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'
                            + '</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:140px;">'
                            //start
                            //+ '<div id="Div1" class="tabContent2" style="height:270px; margin-top:0px;">'
                            + '<h4 style="font-size:12px;">Frequency</h4>'
                            + '<div>'
                            //+ '<h4 > <label id="lblFrqCount' + ulId[BehavCount]
                            //+ '" value="sadadfa" class="lb lblFreq" style="background:transparent;color:Black;border:0px;width:50% !important;margin-left: 10px; text-align: left; font-size: 25px;">'
                            //+ '0</label>'
                            + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'
                            //+'</h4>'
                            + '</div>'
                            + '<input id="btnFrqCntSave,' + ulId[BehavCount]
                            + '" name="" type="button" class="btnFrq"  value="Count" onclick="count(this)"  />'
                            + ' '
                            //+ '<select id="drpFrequency,' + ulId[BehavCount] + '" class="drpcls">'
                            //+ '<option value="0" selected="selected">0</option>'
                            //+ '<option value="1">1</option>'
                            //+ '<option value="2">2</option>'
                            //+ '<option value="3">3</option>'
                            //+ '<option value="4">4</option>'
                            //+ '<option value="5">5</option>'
                            //+ '<option value="6">6</option>'
                            //+ '<option value="7">7</option>'
                            //+ '<option value="8">8</option>'
                            //+ '<option value="9">9</option>'
                            //+ '</select>'
                            + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:290px;">'
                            //start
                             + '<div>'
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                           // + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE"  onclick="editTOE(this)"/>Edit'


                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'

                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;"/>'
                           // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                           + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:420px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:450px; border: 0px;">'
                            //start
                           + '<div id="butSavReset" style="margin-top:15px"> <input type="button" value="Save" class="btns save savDurReset" id="saveDuraFreq,' + ulId[BehavCount] + '">'
                           + '<input type="button" value="Reset" class="btnr reset savDurReset"></div>'
                            //+ '</div>'
                            //end
                            + '</div>'

                            //start
                            + '<div id="Div12" class="tabContent4" style="top:555px; border: 0px;">'
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');
                        //+'<input id="saveDuraFreq,' + ulId[BehavCount] + '" type="button" class="btns save" value="Save" />'
                        //+ '<input type="button" class="btnr reset" value="Reset"/>'

                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');

                    }
                    else if (behavType[BehavCount] == 'DuraFreqYesNo') {
                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                            //+ '<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'

                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'


                            + '<div style="margin-bottom: 70px" id="testtimer'
                            + ulId[BehavCount] + '">'
                            + '</div>'
                            + '<div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,' + ulId[BehavCount]
                            + '" value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'
                            + '</div>'
                            + '</div>'

                            //+ '<div id="Div1" class="tabContent2" style="height:300px; margin-top:0px;">'

                            + '<div class="tabContent4" style="top:135px;">'
                            //start
                            + '<h4 style="font-size:12px;">Frequency</h4>'
                            + '<div><input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" /></div>'
                            + '<input id="btnFrqCntSave,' + ulId[BehavCount] + '" name="" type="button" class="btnFrq"  value="Count" onclick="count(this)"  />'
                            + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:280px;">'
                            + '<div>'//start
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'//end
                            + '</div>'

                            + '<div class="tabContent4" style="top:355px;">'
                            + '<div>'//start
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            //+ '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE"  onclick="editTOE(this)"/>Edit'


                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'


                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                            + '</div>'
                            + '</div>'//end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:490px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div id="Div12" class="tabContent4" style="top:520px; border: 0px;">'
                            //start
                            + '<div id="butSavReset" style="margin-top:15px"> <input type="button" value="Save" onclick="saveDuraFreqYesOrNo(' + ulId[BehavCount] + ',this)" class="btns save savDurReset" id="btnDuraFreqYesOrNo,' + ulId[BehavCount] + '">'
                            + '<input type="button" value="Reset" class="btnr reset savDurReset">'
                            + '</div>'
                            //end
                            + '</div>'

                            //+ '</div>'

                            + '<div id="Div13" class="tabContent4" style="top:620px; border: 0px;">'
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'

                            + '</div></div>');

                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');

                    }
                    else if (behavType[BehavCount] == 'DuraYesNo') {
                        $('#tabs').append('   <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                           // + '<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'

                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'


                            +'<div style="margin-bottom: 70px;" id="testtimer'
                            + ulId[BehavCount] + '"></div><div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,'
                            + ulId[BehavCount] + '"  value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />&nbsp;</div>'
                            //end
                            + '</div>'


                            + '<div class="tabContent4" style="top:135px;">'
                            //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:210px;">'
                            //start
                            + '<div>'
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            //+ '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE"  onclick="editTOE(this)"/>Edit'

                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'

                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:340px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:370px; border: 0px;">'
                            //start
                           + '<div id="butSavReset" style="margin-top:30px"><input type="button" id="btnDurYesOrNo,' + ulId[BehavCount] + '" class="btns save" onclick="saveDurYesOrNo(' + ulId[BehavCount] + ',this)" value="Save"/>'
                            + '<input type="button" class="btnr reset" value="Reset"  />'
                            + '</div>'
                            //end
                            + '</div>'

                            //start
                            + '<div id="Div14" class="tabContent4" style="top:485px; border: 0px;">'
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');
                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');
                    }
                    else if (behavType[BehavCount] == 'FreqYesNo') {
                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="font-size:12px;">Frequency</h4>'
                            + '<div><h4>'
                           + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'
                           + '</h4></div style="width:100%;min-width:130px;"><input id="btnFrqCntSave,' + ulId[BehavCount]
                           + '"name="" type="button" class="btnFrq"  value="Count" onclick="count(this)" />'
                              + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" />'
                              + '</div>'
                              //end
                              + '</div>'

                              + '<div class="tabContent4" style="top:155px;">'
                              //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:235px;">'
                            //start
                              + '<div>'
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            //+ '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" onclick="editTOE(this)"/>Edit'


                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'

                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:370px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:400px; border: 0px;">'
                            //start
                          + '<div id="butSavReset" style="margin-top:25px"><input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveFreqYesOrNo(' + ulId[BehavCount] + ',this)"/>'
                           + '<input type="button" class="btnr reset" value="Reset" onclick="resetOther(this)"/>'
                           + '</div>'
                           //end
                           + '</div>'

                           //
                           + '<div id="Div15" class="tabContent4" style="top:520px; border: 0px;">'
                           + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                           //

                           + '</div></div>');
                    }
                    else if (behavType[BehavCount] == 'YesNo') {
                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                              //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:75px;">'
                            //start
                              + '<div>'
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                           // + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" onclick="editTOE(this)"/>Edit'


                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'



                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            //end
                            + '</div>'

                        ///start
                            + '<div class="tabContent4" style="margin-top:200px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:230px; border: 0px;">'
                            //
                          + '<div id="butSavReset" style="margin-top:25px"><input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveYesOrNo(' + ulId[BehavCount] + ',this)"/>'
                           + '<input type="button" class="btnr reset" value="Reset" onclick="resetOther(this)"/>'
                           + '</div>'
                           //
                           + '</div>'

                           //
                           + '<div id="Div16" class="tabContent4" style="top:360px; border: 0px;">'
                           + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                           //

                           + '</div></div>');
                    }
                    else {
                        //$('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '"><h4>' + ulValue[BehavCount] + '</h4><div class="tapCount"><h4 > <label id="lblFrqCount'
                        //  + ulId[BehavCount] + '" value="sadadfa" class="lb lblFreq" style="background:transparent;color:Red;border:0px;width:50% !important;margin-left: 10px; text-align: left; font-size: 25px;">'
                        //  + '0</label></h4></div style="width:100%;min-width:130px;"><input id="btnFrqCntSave,' + ulId[BehavCount]
                        //  + '" name="" type="button" class="btnFrq"  value="Count" onclick="count(this)" /><h4>Frequency</h4><input id="txtFrequency,' + ulId[BehavCount]
                        //  //id="btnSaveOther, ulId[BehavCount] + '"
                        //  + '" class="tx txtFreq" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'
                        //  + '<input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveOtherFreq(' + ulId[BehavCount] + ',this)"/>'
                        //  + '<input type="button" class="btns reset" value="Reset" onclick="resetOther(this)"/></div></div>');
                        //<h4>' + ulValue[BehavCount] + '</h4>
                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="font-size:12px;">Frequency</h4><div><h4>'
                           //+ '<label id="lblFrqCount' + ulId[BehavCount] + '" value="sadadfa" class="lb lblFreq"'
                           //+ 'style="background:transparent;color:Black;border:0px;width:50% !important;margin-left: 10px; text-align: left; font-size: 25px;">0</label>'
                           + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'
                           + '</h4></div style="width:100%;min-width:130px;"><input id="btnFrqCntSave,' + ulId[BehavCount]
                           + '"name="" type="button" class="btnFrq"  value="Count" onclick="count(this)" />'
                           + ' '
                             //+ '<select id="drpFrequency,' + ulId[BehavCount] + '" class="drpcls">'
                             //+ '<option value="0" selected="selected">0</option>'
                             //+ '<option value="1">1</option>'
                             //+ '<option value="2">2</option>'
                             //+ '<option value="3">3</option>'
                             //+ '<option value="4">4</option>'
                             //+ '<option value="5">5</option>'
                             //+ '<option value="6">6</option>'
                             //+ '<option value="7">7</option>'
                             //+ '<option value="8">8</option>'
                             //+ '<option value="9">9</option>'
                             //+ '</select>'
                              + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                              //end
                              + '</div>'

                              + '<div class="tabContent4" style="top:155px;">'
                              //start
                              + '<div>'
                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            //+ '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE"  onclick="editTOE(this)"/>Edit'

                            + '<label class="swt">'
                            + '<input id="chkTOE' + ulId[BehavCount] + '" type="checkbox" class="swt_inp editTOE" onclick="editTOE(this)">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'

                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;"/>'
                           // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                           + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:295px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:325px; border: 0px;">'
                            //
                          + '<div id="butSavReset" style="margin-top:25px"><input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveOtherFreq(' + ulId[BehavCount] + ',this)"/>'
                           + '<input type="button" class="btnr reset" value="Reset" onclick="resetOther(this)"/>'
                           + '</div>'
                           //
                           + '</div>'

                           //
                           + '<div id="Div17" class="tabContent4" style="top:450px; border: 0px;">'
                           + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                           //

                           + '</div></div>');
                    }
                }
                else {

                    //IF INTERVAL AND PERIOD ARE SAME

                    if (behavType[BehavCount] == 'Duration') {


                        //<h4>' + ulValue[BehavCount] + '</h4>
                        $('#tabs').append('   <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                           // + '<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'


                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'


                            +'<div style="margin-bottom: 70px;" id="testtimer'
                            + ulId[BehavCount] + '"></div><div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,'
                            + ulId[BehavCount] + '"  value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />&nbsp;</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:140px;">'
                            //start
                            + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE"  onclick="return false;" style="display:none;"  checked = "checked"/>'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" onclick="timerFunction();" style="border: 3px solid green; width: 80%; display:none;"/>'
                            + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                           // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'

                           + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                           + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                            + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:325px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:355px; border: 0px;">'
                            //start
                           + '<div id="butSavReset" style="margin-top:30px"><input type="button" id="btnDuration,' + ulId[BehavCount] + '" class="btns save" value="Save"/>'
                            + '<input type="button" class="btnr reset" value="Reset"  />'
                            + '</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:480px; border: 0px;">'
                            //start
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');



                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);

                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');

                    }
                    else if (behavType[BehavCount] == 'DuraFreq') {



                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                           // + '<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'


                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'


                            +'<div style="margin-bottom: 70px" id="testtimer'
                            + ulId[BehavCount] + '"></div><div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,' + ulId[BehavCount]
                            + '" value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'
                            + '</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:140px;">'
                            //start
                            //+ '<div id="Div1" class="tabContent2" style="height:320px; margin-top:0px;">'
                            + '<h4 style="font-size:12px;">Frequency</h4>'
                            + '<div>'
                            //+ '<h4 > <label id="lblFrqCount' + ulId[BehavCount]
                            //+ '" value="sadadfa" class="lb lblFreq" style="background:transparent;color:Black;border:0px;width:50% !important;margin-left: 10px; text-align: left; font-size: 25px;">'
                            //+ '0</label>'

                            + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'

                            //+'</h4>'
                            + '</div>'
                            

                            + '<input id="btnFrqCntSave,' + ulId[BehavCount]
                            + '" name="" type="button" class="btnFrq"  value="Count" onclick="count(this)"  />'
                            + ' '
                            //+ '<select id="drpFrequency,' + ulId[BehavCount] + '" class="drpcls">'
                            //+ '<option value="0" selected="selected">0</option>'
                            //+ '<option value="1">1</option>'
                            //+ '<option value="2">2</option>'
                            //+ '<option value="3">3</option>'
                            //+ '<option value="4">4</option>'
                            //+ '<option value="5">5</option>'
                            //+ '<option value="6">6</option>'
                            //+ '<option value="7">7</option>'
                            //+ '<option value="8">8</option>'
                            //+ '<option value="9">9</option>'
                            //+ '</select>'
                            + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                            //end
                            + '</div>'


                            + '<div class="tabContent4" style="top:290px;">'
                            //start
                             + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" checked="checked" style="display:none;"  />'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%; display:none;"/>'
                             + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                           // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                           + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:10px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:10px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:10px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'
                             + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                             //end
                             + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:455px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                             + '<div class="tabContent4" style="top:485px; border: 0px;">'
                             //start
                           + '<div id="butSavReset" style="margin-top:15px"> <input type="button" value="Save" class="btns save savDurReset" id="saveDuraFreq,' + ulId[BehavCount] + '">'
                           + '<input type="button" value="Reset" class="btnr reset savDurReset"></div>'
                            //+ '</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:610px; border: 0px;">'
                            //start
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');
                        //+'<input id="saveDuraFreq,' + ulId[BehavCount] + '" type="button" class="btns save" value="Save" />'
                        //+ '<input type="button" class="btnr reset" value="Reset"/>'

                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);

                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');



                    }
                    else if (behavType[BehavCount] == 'DuraFreqYesNo') {

                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                            //+'<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'
                            
                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'
                            
                            
                            +'<div style="margin-bottom: 70px" id="testtimer'
                            + ulId[BehavCount] + '"></div><div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,' + ulId[BehavCount]
                            + '" value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />'
                            + '</div>'
                            //end
                            + '</div>'

                            //+ '<div id="Div1" class="tabContent2" style="height:380px; margin-top:0px;">'

                            + '<div class="tabContent4" style="top:140px;">'
                            //start
                            + '<h4 style="font-size:12px;">Frequency</h4>'
                            + '<div>'
                            + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'

                            //+'</h4>'
                            + '</div>'
                            + '<input id="btnFrqCntSave,' + ulId[BehavCount]
                            + '" name="" type="button" class="btnFrq"  value="Count" onclick="count(this)"  />'
                            + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:290px;">'
                            //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:365px;">'
                            //start
                             + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" checked="checked" style="display:none;"/>'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;display:none;"/>'
                             + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'

                            + '</div>'

                            + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:555px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:585px; border: 0px;">'
                            //start
                           + '<div id="butSavReset" style="margin-top:15px"> <input type="button" value="Save" onclick="saveDuraFreqYesOrNo(' + ulId[BehavCount] + ',this)" class="btns save savDurReset" id="btnDuraFreqYesOrNo,' + ulId[BehavCount] + '">'
                           + '<input type="button" value="Reset" class="btnr reset savDurReset"></div>'
                           //end
                           + '</div>'
                            //+ '</div>'

                            + '<div class="tabContent4" style="top:705px; border: 0px;">'
                            //start
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');

                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);

                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');


                    }
                    else if (behavType[BehavCount] == 'DuraYesNo') {

                        $('#tabs').append('   <div><div style="height:300px;" id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="width:73px;font-size:12px;float:left">Duration</h4>'
                            //+'<input type="checkbox" id="chktimer' + ulId[BehavCount] + '" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">Edit'
                            
                            
                            + '<label class="swt">'
                            + '<input id="chktimer' + ulId[BehavCount] + '" type="checkbox" class="swt_inp" onclick="chktimerchanged(this,' + ulId[BehavCount] + ');">'
                            + '<span class="swt_lbl" data-on="Close" data-off="Edit"></span>'
                            + '<span class="swt-handle"></span>'
                            + '</label>'
                            
                            
                            +'<div style="margin-bottom: 70px;" id="testtimer'
                            + ulId[BehavCount] + '"></div><div id="testtimer2' + ulId[BehavCount] + '" style="width:100%;min-width:136px;width: 136px; height: 45px; border: 4px solid rgb(96, 198, 88); display: block; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumber(event);" onKeyUp="secConverter(event,this)" value="" class="tx1 splitSec" style="width:30px;border:0px;text-align:center;" type="text"/>'
                            + '<input id="txtDuration,'
                            + ulId[BehavCount] + '"  value="" class="tx totSec" style="width:50px; min-width:50px; display:none;" name="" type="text" onkeypress="return isNumber(event)" onpaste="return false" />&nbsp;</div>'
                            //end
                            + '</div>'


                            + '<div class="tabContent4" style="top:140px;">'
                            //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:220px;">'
                            //start
                            + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" checked="checked" style="display:none;"/>'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;display:none;"/>'
                             + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'

                            + '</div>'

                            + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:410px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:440px; border: 0px;">'
                            //start
                           + '<div id="butSavReset" style="margin-top:30px"><input type="button" id="btnDurYesOrNo,' + ulId[BehavCount] + '" class="btns save" onclick="saveDurYesOrNo(' + ulId[BehavCount] + ',this)" value="Save"/>'
                            + '<input type="button" class="btnr reset" value="Reset"  />'
                            + '</div>'
                            //end
                            + '</div>'

                            + '<div class="tabContent4" style="top:560px; border: 0px;">'
                            //start
                            + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                            //end

                            + '</div></div>');

                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);
                        addStopwatch('testtimer' + ulId[BehavCount], ulId[BehavCount], '1');



                    }
                    else if (behavType[BehavCount] == 'FreqYesNo') {

                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4 style="font-size:12px;">Frequency</h4><div><h4>'
                           + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'

                           + '</h4></div style="width:100%;min-width:130px;"><input id="btnFrqCntSave,' + ulId[BehavCount]
                           + '"name="" type="button" class="btnFrq"  value="Count" onclick="count(this)" />'
                              + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                              //end
                              + '</div>'

                              + '<div class="tabContent4" style="top:155px;">'
                              //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:235px;">'
                            //start
                              + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" checked="checked" style="display:none;"/>'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;display:none;"/>'
                             + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'

                            + '</div>'

                            + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                            //end
                            + '</div>'

                        ///start
                            + '<div class="tabContent4" style="margin-top:430px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:460px; border: 0px;">'
                            //start
                          + '<div id="butSavReset" style="margin-top:25px"><input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveFreqYesOrNo(' + ulId[BehavCount] + ',this)"/>'
                           + '<input type="button" class="btnr reset" value="Reset" onclick="resetOther(this)"/>'
                           + '</div>'
                           //end
                           + '</div>'

                           + '<div class="tabContent4" style="top:580px; border: 0px;">'
                           //start
                           + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                           //end

                           + '</div></div>');

                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);

                    }
                    else if (behavType[BehavCount] == 'YesNo') {

                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                              //div start
                            + '<div>'
                            //+ '<h4>Yes/No</h4>'
                            + '<div id="divYesOrNo">'
                            + '<input type="button" id="btnYes' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnYesClicked(' + ulId[BehavCount] + ');" value="Yes">'
                            + '<input type="button" id="btnNo' + ulId[BehavCount] + '" class="btnNothingClick" style="min-width:60px !important;" onclick="btnNoClicked(' + ulId[BehavCount] + ');" value="No">'
                            + '</div>'
                            + '</div>'
                            //div end
                            + '</div>'

                            + '<div class="tabContent4" style="top:75px;">'
                            //start
                              + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" checked="checked" style="display:none;" class="editTOE" onclick="editTOE(this)"/>'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;display:none;"/>'
                             + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                            //+ '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                            + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'

                            + '</div>'

                            + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                            //end
                            + '</div>'
                        ///start
                            + '<div class="tabContent4" style="top:265px; text-align:center; border: 0px;">'
                            + '<b><input  type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                            + '<div class="tabContent4" style="top:295px; border: 0px;">'
                            //start
                          + '<div id="butSavReset" style="margin-top:25px"><input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveYesOrNo(' + ulId[BehavCount] + ',this)"/>'
                           + '<input type="button" class="btnr reset" value="Reset" onclick="resetOther(this)"/>'
                           + '</div>'
                           //end
                           + '</div>'

                           + '<div class="tabContent4" style="top:420px; border: 0px;">'
                           //start
                           + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                           //end


                           + '</div></div>');

                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);

                    }
                    else {

                        $('#tabs').append('  <div><div id="tabs-' + ulId[BehavCount] + '" class="behavCont">'

                            + '<div class="tabContent4" style="top:0px;">'
                            //start
                            + '<h4>Frequency</h4><div><h4>'

                          

                           + '<input type="text" pattern="[0-9]*" id="lblFrqCount' + ulId[BehavCount] + '" class="txDrp" value="" maxlength="3"  onKeyPress="return isNumber(event);" />'

                           + '</h4></div style="width:100%;min-width:130px;"><input id="btnFrqCntSave,' + ulId[BehavCount]
                           + '"name="" type="button" class="btnFrq"  value="Count" onclick="count(this)" />'
                           + ' '
                            
                              + '<div><input type="button" value="Count & Save" class="btnsGr save savDurReset" id="" style="margin:0px;width:90%;display:none;" onclick="CountAndSave(' + ulId[BehavCount] + ');" /></div>'
                              //end
                              + '</div>'

                              + '<div class="tabContent4" style="top:155px;">'
                              //start
                              + '<div>'

                            + '<h4 style="width:72px;font-size:12px;float:left">Event Time</h4>'
                            + '<input type="checkbox" id="chkTOE' + ulId[BehavCount] + '" class="editTOE" checked="checked"  style="display:none;"/>'
                            + '<input type="text" id="txtTOE' + ulId[BehavCount] + '" class="TOE_timer" style="border: 3px solid green; width: 80%;display:none;"/>'
                             + '<input type="hidden" id="hdnTOE_' + ulId[BehavCount] + '" class="hdnTOE"/>'
                           // + '<input type="text" id="txtTOE_M' + ulId[BehavCount] + '" class="TOE_edit" style="border: 3px solid green; width: 90%;display:none;"/>'
                           + '<div id="divTimeEvt' + ulId[BehavCount] + '" class="divTimeEvt" style="width:100%;min-width:147px;width: 147px; height: 45px; border: 4px solid rgb(96, 198, 88); display: none; border-radius: 10px; font-weight: bold; font-size: 22px;">'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeHr(event);"  id="txtTimeHr' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style=font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeMin(event);"   id="txtTimeMin' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>:'
                            + '<input maxLength="2" onKeyPress="return isNumberTimeSec(event);"   id="txtTimeSec' + ulId[BehavCount] + '" value="0" class="txtTime timeEvent" style="font-size:11px;width:15px;border:0px;text-align:center;" type="text"/>'
                            + '<select id="drpTimeAmPm' + ulId[BehavCount] + '"  class="txtTime timeEvent" style="text-align: left; width: 45px; height: 30px; font-size: 15px;" type="select"><option value="0">AM</option><option value="1">PM</option></select>'
                             + '</div>'
                            + '</div>'

                             + '<div id="dvTimeId_' + ulId[BehavCount] + '" class="auto_box1"></div>'
                             //end
                             + '</div>'
                        ///start
                            + '<div class="tabContent4" style="margin-top:345px; text-align:center; border: 0px;">'
                            + '<b><input type="checkbox" id="chkIOA_' + ulId[BehavCount] + '"/> Start as IOA </b>'
                            + '</div>'
                        ///end
                             + '<div class="tabContent4" style="top:375px; border: 0px;">'
                             //start
                          + '<div id="butSavReset" style="margin-top:25px"><input type="button" id="btnSaveOther,' + ulId[BehavCount] + '" class="btns save" value="Save" onclick="saveOtherFreq(' + ulId[BehavCount] + ',this)"/>'
                           + '<input type="button" class="btnr reset" value="Reset" onclick="resetOther(this)"/>'
                           + '</div>'
                           //end
                           + '</div>'

                           + '<div class="tabContent4" style="top:490px; border: 0px;">'
                           //start
                           + '<b><u>Definition</u></b></br>' + BehavDefinition[BehavCount] + '</br><b><u>Strategy</u></b></br>' + BehavStrategy[BehavCount] + '</div>'
                           //end

                           + '</div></div>');
                        timerCalculate(Interval[BehavCount], StartTime[BehavCount], EndTime[BehavCount], ulId[BehavCount], Period[BehavCount], PrevSaveList[BehavCount]);
                    }
                }



            }
            $('#tabs').smartTab({});
            if (ulId.length - 1 == 0) {

                $('.rightPartContainerTimerNullVal').show();
                $('.rightPartContainerTimer').hide();
            }
            else {
                $('.rightPartContainerTimerNullVal').hide();
                $('.rightPartContainerTimer').show();
            }

            loadTopTabs2(2);


            //generate Time of Event's clock
            startTime();
        });

        function btnYesClicked(id) {
            if ($('#btnYes' + id).hasClass("btnNothingClick")) {
                $('#btnYes' + id).removeClass();
                $('#btnYes' + id).addClass("btnYesClick");
                $('#btnNo' + id).removeClass();
                $('#btnNo' + id).addClass("btnNothingClick");
            }
            else {
                $('#btnYes' + id).removeClass();
                $('#btnYes' + id).addClass("btnNothingClick");
            }
        }
        function btnNoClicked(id) {
            if ($('#btnNo' + id).hasClass("btnNothingClick")) {
                $('#btnNo' + id).removeClass();
                $('#btnNo' + id).addClass("btnNoClick");
                $('#btnYes' + id).removeClass();
                $('#btnYes' + id).addClass("btnNothingClick");
            }
            else {
                $('#btnNo' + id).removeClass();
                $('#btnNo' + id).addClass("btnNothingClick");
            }
        }

        function SelectBehaviorTab(valu) {
            triggerEvntStatus = "";
            $('#hdnchecktabchange').val('false');
            $('#hdnAutoSave').val('False');
            var prevSelectedTab = $('.sel').attr('id').split('-')[1];
            var NewlySelectedTab = valu.id.split('-')[1];
            var TabIdName = 'tabs-' + prevSelectedTab;
            $("#" + TabIdName).find(".btns").trigger("click");

            $(".btns").click(function (event) {
                if (event.originalEvent === undefined) {
                    //alert('not human')
                    triggerEvntStatus = "true";
                } else {
                    //alert(' human');
                    triggerEvntStatus = "false";
                }
            });
            //alert(prevSelectedTab);
            //alert(NewlySelectedTab);
            $('#testtimer' + NewlySelectedTab).show();
            $('#testtimer2' + NewlySelectedTab).hide();
            $('#chktimer' + NewlySelectedTab).prop("checked", false);
            triggerEvntStatus = "false";
        }

        function CountAndSave(BehavId) {
            var behaveId = BehavId;
            var stdid = document.getElementById('hdnFldStudentId').value;//pramod
            var clsid = document.getElementById('hdnFldClassId').value;
            var isUpdate = document.getElementById('hdnFldIsUpdate').value;
            var freValue = ($('#lblFrqCount' + behaveId).val() == "") ? 0 : parseInt($('#lblFrqCount' + behaveId).val());
            if (isNaN(freValue)) {
                freValue = 0;
            }
            var freq = freValue + 1;
            var amPm = "";
            var toe = "";

            if ($('#chkTOE' + behaveId).length > 0) {
                if ($('#chkTOE' + behaveId).prop('checked') == true) {
                    if ($('#drpTimeAmPm' + behaveId).val() == '0') {
                        amPm = 'AM';
                    }
                    else
                        amPm = 'PM';

                    toe = document.getElementById('txtTimeHr' + behaveId).value + ":" + document.getElementById('txtTimeMin' + behaveId).value + ":" + document.getElementById('txtTimeSec' + behaveId).value + " " + amPm;

                    var h = document.getElementById('txtTimeHr' + behaveId).value;
                    var m = document.getElementById('txtTimeMin' + behaveId).value;
                    var s = document.getElementById('txtTimeSec' + behaveId).value;
                    toe = convStringToDateTimeFormat(h, m, s, amPm);
                }
                else {

                    var time = document.getElementById('txtTOE' + behaveId).value;
                    toe = convStringToDateTimeFormat2(time);
                }
            }


            var hasInterval = $('#dvTimeId_' + inputBtn).length;
            var isSelected = $('#dvTimeId_' + inputBtn).find('.bselect').length;
            var saveVar = 1;
            if (hasInterval == 1 && isSelected == 0) {
                saveVar = 0;
            }

            if (saveVar == 1) {
                //btnFrq
                var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                $.ajax(
                {

                    type: "POST",
                    url: "dataSheetTimer.aspx/saveFrequency",
                    data: "{'MeasurementId':'" + behaveId + "','StudentId':'" + stdid + "','Count':'" + freq + "','toe':'" + toe + "','isBehUpdate':'" + isUpdate + "','chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        var contents = data.d;                       

                        //document.getElementById('lblFrqCount' + behaveId).innerHTML = contents;

                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px;'>Count Saved</div>";
                        $('body').append(alertBox);
                        // to check whether the normal score submitted within 5 min
                        if (data.d == "NoNormalData") {
                            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                            $('body').append(alertBox);
                            $('.alertBox').fadeOut(5000, function () {
                                $(this).remove();
                            });
                        }
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    },
                    error: function (request, status, error) {
                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px;'>Count Not Saved</div>";
                        $('body').append(alertBox);
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    }
                });
            }
        }

        function secConverter(e, elem) {
            var totalSec = 0;
            var splitSecElems = $(elem).parent().find('.splitSec');
            for (var i = 0; i < splitSecElems.length; i++) {
                var txtValue = $(splitSecElems[i]).val();
                if (txtValue != "") {
                    switch (i) {
                        case 0:
                            totalSec += parseInt(txtValue) * 60 * 60;
                            break;
                        case 1:
                            totalSec += parseInt(txtValue) * 60;
                            break;
                        case 2:
                            totalSec += parseInt(txtValue);
                            break;
                        default:
                            break;
                    }

                    $(elem).parent().find('.totSec').val(totalSec);
                }

            }
        }
        function fillDuration(elemt, durSec) {
            durSec = Number(durSec);
            var splitMin = Math.floor(durSec % 3600 / 60);
            var splitHr = Math.floor(durSec / 3600);
            var splitSec = Math.floor(durSec % 3600 % 60);

            var txtDurTime = $(elemt).parent().parent().parent().find('.splitSec');

            for (var i = 0; i < txtDurTime.length; i++) {

                switch (i) {
                    case 0:
                        $(txtDurTime[i]).val(splitHr);
                        break;
                    case 1:
                        $(txtDurTime[i]).val(splitMin);
                        break;
                    case 2:
                        $(txtDurTime[i]).val(splitSec);
                        break;
                    default:
                        break;
                }

                // $(elem).parent().find('.totSec').val(durSec);


            }
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function isNumberTimeHr(evt) {
            evt = (evt) ? evt : window.event;

            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function isNumberTimeMin(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function isNumberTimeSec(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }


        var counter = 0;
        var loop = 0;
        function count(elem) {
            $('#hdnchecktabchange').val('True');
            counter = parseInt($(elem).parent().find('.txDrp').val());
            //$(thisElem).parent().parent().find('.txDrp').val('');
            if (isNaN(counter)) {
                counter = 0;
            }
            counter = counter + 1;
            loop = loop + 1;
            $(elem).parent().find('.txDrp').val(counter.toString());
            //$(elem).parent().find('.lb').text(counter.toString());

        }

        function saveDuration(inputBtn, thisElem) {
            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {
                    var behaveId = inputBtn;
                    var parentBox = $(thisElem).parents('.tabContent');
                    var clockBtnStatus = $(parentBox).find('#btnStartStopTimer').val();
                    // var val = $(thisElem).parent().find('.lb').text();
                    var times = $(parentBox).find('.totSec').val();

                    //var stpSeconds = $(thisElem).parent().parent().find('.stpSeconds').text();
                    var stpSeconds = $(parentBox).find('.stpSeconds').text();
                    //alert(stpSeconds);

                    //if (stpSeconds == "") {
                    //stpSeconds = $(thisElem).parent().parent().parent().find('.stpSeconds').text();
                    //for duration with yes/no, parent div is different
                    //alert($(thisElem).parent().parent().html());
                    //alert($(thisElem).parent().parent().parent().html());
                    // }


                    if ($('#chktimer' + inputBtn)[0].checked) {
                        //custom time
                        $('#testtimer2' + inputBtn).show();
                        $('#testtimer' + inputBtn).hide();
                        stpSeconds = 0;
                    } else {
                        //timer
                        $('#testtimer' + inputBtn).show();
                        $('#testtimer2' + inputBtn).hide();
                        times = "";
                    }


                    var checktab = document.getElementById('hdnchecktabchange').value;
                    //$('#hdnchecktabchange').val('True');
                    var drtion = document.getElementById('txtDuration,' + behaveId).value;

                    if (stpSeconds > 0) {
                    }
                    else {
                        stpSeconds = 0;
                    }
                    if (drtion > 0) {


                    }
                    else {
                        var sd = 0;

                    }
                    if (stpSeconds == 0 && checktab == 'True' && sd == 0) {
                        var flag = confirm("Do you want to save zero duration");
                    }

                    if (flag) {
                        if (parseInt(stpSeconds) >= 0 && times != "") {
                            if (clockBtnStatus == "Start") {
                                saveDurationOther(inputBtn)
                                var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                                //saveTimeInDB(time, "", inputBtn);
                            }
                            else {
                                //alert('Timer Still Running');
                            }
                        }
                        else if (times == "" && stpSeconds >= 0) {
                            var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                            saveTimeInDB(inputBtn, thisElem);
                        }
                        else if (stpSeconds == 0 && times >= 0) {
                            if (clockBtnStatus == "Start") {
                                saveDurationOther(inputBtn)
                            }
                            else {
                                //alert('Timer Still Running');
                            }
                        }
                    }
                    else {
                        if (parseInt(stpSeconds) > 0 && times != "") {
                            if (clockBtnStatus == "Start") {
                                saveDurationOther(inputBtn)
                                var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                                //saveTimeInDB(time, "", inputBtn);
                            }
                            else {
                                //alert('Timer Still Running');
                            }
                        }
                        else if (times == "" && stpSeconds > 0) {
                            var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                            saveTimeInDB(inputBtn, thisElem);
                        }
                        else if (stpSeconds == 0 && parseInt(times) >= 0) {
                            if (clockBtnStatus == "Start") {
                                saveDurationOther(inputBtn)
                            }
                            else {
                                //alert('Timer Still Running');
                            }
                        }
                    }
                    $('#hdnchecktabchange').val('True');
                    if ($('#chkIOA_' + inputBtn).prop('checked'))
                        $('#chkIOA_' + inputBtn).prop('checked', false);
                    var display = $('#txtTOE' + inputBtn).css('display');
                    if (display == 'none')
                        $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');
                }
                    else {
                        if (triggerEvntStatus == "false") {
                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please select an interval first</div>";
                        $('body').append(alertBox);
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    }
                }
                }
                else { 
            var behaveId = inputBtn;
            var parentBox = $(thisElem).parents('.tabContent');
            var clockBtnStatus = $(parentBox).find('#btnStartStopTimer').val();
            // var val = $(thisElem).parent().find('.lb').text();
            var times = $(parentBox).find('.totSec').val();

            //var stpSeconds = $(thisElem).parent().parent().find('.stpSeconds').text();
            var stpSeconds = $(parentBox).find('.stpSeconds').text();
            //alert(stpSeconds);

            //if (stpSeconds == "") {
            //stpSeconds = $(thisElem).parent().parent().parent().find('.stpSeconds').text();
            //for duration with yes/no, parent div is different
            //alert($(thisElem).parent().parent().html());
            //alert($(thisElem).parent().parent().parent().html());
            // }


            if ($('#chktimer' + inputBtn)[0].checked) {
                //custom time
                $('#testtimer2' + inputBtn).show();
                $('#testtimer' + inputBtn).hide();
                stpSeconds = 0;
            } else {
                //timer
                $('#testtimer' + inputBtn).show();
                $('#testtimer2' + inputBtn).hide();
                times = "";
            }


            var checktab = document.getElementById('hdnchecktabchange').value;
            //$('#hdnchecktabchange').val('True');
            var drtion = document.getElementById('txtDuration,' + behaveId).value;

            if (stpSeconds > 0) {
            }
            else {
                stpSeconds = 0;
            }
            if (drtion > 0) {


            }
            else {
                var sd = 0;

            }
            if (stpSeconds == 0 && checktab == 'True' && sd == 0) {
                var flag = confirm("Do you want to save zero duration");
            }

            if (flag) {
                if (parseInt(stpSeconds) >= 0 && times != "") {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                        var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                        //saveTimeInDB(time, "", inputBtn);
                    }
                    else {
                        //alert('Timer Still Running');
                    }
                }
                else if (times == "" && stpSeconds >= 0) {
                    var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                    saveTimeInDB(inputBtn, thisElem);
                }
                else if (stpSeconds == 0 && times >= 0) {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                    }
                    else {
                        //alert('Timer Still Running');
                    }
                }
            }
            else {
                if (parseInt(stpSeconds) > 0 && times != "") {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                        var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                        //saveTimeInDB(time, "", inputBtn);
                    }
                    else {
                        //alert('Timer Still Running');
                    }
                }
                else if (times == "" && stpSeconds > 0) {
                    var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                    saveTimeInDB(inputBtn, thisElem);
                }
                else if (stpSeconds == 0 && parseInt(times) >= 0) {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                    }
                    else {
                        //alert('Timer Still Running');
                    }
                }
            }
            $('#hdnchecktabchange').val('True');
            if ($('#chkIOA_' + inputBtn).prop('checked'))
                $('#chkIOA_' + inputBtn).prop('checked', false);
            var display = $('#txtTOE' + inputBtn).css('display');
            if (display == 'none')
                $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');
        }
        }

        function saveDuration2(inputBtn, thisElem) {

            var behaveId = inputBtn;
            var parentBox = $(thisElem).parents('.tabContent');
            var clockBtnStatus = $(parentBox).find('#btnStartStopTimer').val();
            var times = $(parentBox).find('.totSec').val();
            var stpSeconds = $(parentBox).find('.stpSeconds').text();

            if ($('#chktimer' + inputBtn)[0].checked) { //custom time
                $('#testtimer2' + inputBtn).show();
                $('#testtimer' + inputBtn).hide();
                stpSeconds = 0;
            } else {  //timer
                $('#testtimer' + inputBtn).show();
                $('#testtimer2' + inputBtn).hide();
                times = "";
            }

            var checktab = document.getElementById('hdnchecktabchange').value;
            //$('#hdnchecktabchange').val('True');

            var drtion = document.getElementById('txtDuration,' + behaveId).value;

            if (stpSeconds > 0) {
            }
            else {
                stpSeconds = 0;
            }

            if (drtion > 0) {

            }
            else {
                var sd = 0;

            }
            if (stpSeconds == 0 && checktab == 'True' && sd == 0) {
                var flag = confirm("Do you want to save zero duration");
            }

            if (flag) {
                if (parseInt(stpSeconds) >= 0 && times != "") {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                        var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                    }
                }
                else if (times == "" && stpSeconds >= 0) {
                    var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                    saveTimeInDB(inputBtn, thisElem);
                }
                else if (stpSeconds == 0 && times >= 0) {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                    }
                }
            }
            else {
                if (parseInt(stpSeconds) > 0 && times != "") {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                        var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                    }
                }
                else if (times == "" && stpSeconds > 0) {
                    var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
                    saveTimeInDB(inputBtn, thisElem);
                }
                else if (stpSeconds == 0 && parseInt(times) >= 0) {
                    if (clockBtnStatus == "Start") {
                        saveDurationOther(inputBtn)
                    }
                }
            }
                  $('#hdnchecktabchange').val('True');        }
        function resetOther(thisElem) {

            //$(thisElem).parent().find('.txtFreq').val('');
            //var drp = $(thisElem).parent().parent().find('.drpcls');
            //drp.selectedIndex = 0;

            //$(thisElem).parent().find('.txDrp').val('0');
            //$(thisElem).parent().parent().find('.txDrp').val('0');
            //$(thisElem).parent().parent().find('.tx1').val('0');

            //$(thisElem).parent().parent().find('.lblFreq').text('0');

            $('.txDrp').val('');
            $('.tx1').val('');
            $('.txtTime').val('0');

            //alert($(this.Element).parents('.behavCont').find('.txDrp').html);

            $(this.Element).parents('.behavCont').find('.txDrp').val('0');
            $(this.Element).parents('.behavCont').find('.tx1').val('0');

            //$('.txDrp').val('0');
            //$('.tx1').val('0');

            $('#hdnAutoSave').val('False');

            //$('.btnYesClick').removeClass().addClass('btnNothingClick');
            //$('.btnNoClick').removeClass().addClass('btnNothingClick');
            $(thisElem).parent().find('.btns').val('Save');
            $('#hdnFldIsUpdate').val('False');
        }

        function saveOtherFreq(inputBtn, thisElem) {
            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {

                    saveOther(inputBtn, thisElem);

                    resetOther(thisElem);
                }
                else {
                    if (triggerEvntStatus == "false") {
                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please select an interval first</div>";
                    $('body').append(alertBox);
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
            }
            }
            }
            else {
                saveOther(inputBtn, thisElem);

                resetOther(thisElem);
            }
            if ($('#chkIOA_' + inputBtn).prop('checked'))
                $('#chkIOA_' + inputBtn).prop('checked', false);
            var display = $('#txtTOE' + inputBtn).css('display');
            if (display == 'none')
                $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');

        }

        function saveFreqYesOrNo(inputBtn, thisElem) {


            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {

                    saveOther(inputBtn, thisElem);
                    saveYesOrNo(inputBtn, thisElem);

                    resetOther(thisElem);
                }
                else {
                    //var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px; '>Time not selected</div>";
                    //$('body').append(alertBox);
                    //$('.alertBox').fadeOut(5000, function () {
                    //    $(this).remove();
                    //});

                }
            }
            else {
                saveOther(inputBtn, thisElem);
                saveYesOrNo(inputBtn, thisElem);

                resetOther(thisElem);
            }
        }

        function saveYesOrNo(inputBtn, thisElem) {

            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {
            var behaveId = inputBtn;
            var stdid = document.getElementById('hdnFldStudentId').value;
            var clsid = document.getElementById('hdnFldClassId').value;
            var autosav = document.getElementById('hdnAutoSave').value;
            var isUpdate = document.getElementById('hdnFldIsUpdate').value;
            var frq = "";
            var drpfrq = document.getElementById('lblFrqCount' + inputBtn);
            if (drpfrq != null) {
                 frq = drpfrq.value;
            }

            var yesOrNoStatus = "";
            if ($('#btnYes' + inputBtn).hasClass("btnYesClick")) {
                yesOrNoStatus = "1";
                frq = "1";
            } else if ($('#btnNo' + inputBtn).hasClass("btnNoClick")) {
                yesOrNoStatus = "0";
                frq = "0";
            }



            if ($('#chkTOE' + inputBtn).length > 0) {
                if ($('#chkTOE' + inputBtn).prop('checked') == true) {
                    if ($('#drpTimeAmPm' + inputBtn).val() == '0') {
                        amPm = 'AM';
                    }
                    else
                        amPm = 'PM';

                    toe = document.getElementById('txtTimeHr' + inputBtn).value + ":" + document.getElementById('txtTimeMin' + inputBtn).value + ":" + document.getElementById('txtTimeSec' + inputBtn).value + " " + amPm;

                    var h = document.getElementById('txtTimeHr' + behaveId).value;
                    var m = document.getElementById('txtTimeMin' + behaveId).value;
                    var s = document.getElementById('txtTimeSec' + behaveId).value;
                    toe = convStringToDateTimeFormat(h, m, s, amPm);
                }
                else {

                    var time = document.getElementById('txtTOE' + inputBtn).value;
                    toe = convStringToDateTimeFormat2(time);
                }
            }

            if (yesOrNoStatus == "0" || yesOrNoStatus == "1") {
                var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');
                var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                $.ajax(
                    {

                        type: "POST",
                        url: "dataSheetTimer.aspx/saveYesOrNo",
                        data: "{'MeasurementId':'" + inputBtn + "','StudentId':'" + stdid + "','yesOrNo':'" + yesOrNoStatus + "','FrequencyCount':'" + frq + "','toe':'" + toe + "','YNautoSave':'" + autosav + "','isBehUpdate':'" + isUpdate + "', 'chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",

                        contentType: "application/json; charset=utf-8",
                        //dataType: "json",
                        async: false,
                        success: function (data) {

                            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px; '>Yes or No saved</div>";

                            $('body').append(alertBox);
                            // to check whether the normal score submitted within 5 min
                            if (data.d == "NoNormalData") {
                                var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                                $('body').append(alertBox);
                                $('.alertBox').fadeOut(5000, function () {
                                    $(this).remove();
                                });
                            }
                            $('.alertBox').fadeOut(5000, function () {
                                $(this).remove();
                            });
                            if ($('#' + selToe).hasClass('bselect')) {
                                $('#' + selToe).removeClass('bselect');
                            }
                            $('#' + selToe).addClass('redBox');
                        },
                        error: function (request, status, error) {
                            alert("Error");
                        }
                    });

                            $('.btnYesClick').removeClass().addClass('btnNothingClick');
                            $('.btnNoClick').removeClass().addClass('btnNothingClick');
            }
                    else {
                        if (triggerEvntStatus == "false") {
                            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please record data first</div>";
                            $('body').append(alertBox);
                            $('.alertBox').fadeOut(5000, function () {
                                $(this).remove();
                            });
                        }
                    }
                    resetOther(thisElem);
                    if ($('#chkIOA_' + inputBtn).prop('checked'))
                        $('#chkIOA_' + inputBtn).prop('checked', false);
                    var display = $('#txtTOE' + inputBtn).css('display');
                    if (display == 'none')
                        $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');
                }
                else {
                    if (triggerEvntStatus == "false") {
                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please record data first</div>";
                    $('body').append(alertBox);
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
                }
            }
            }
            else{
                var behaveId = inputBtn;
                var stdid = document.getElementById('hdnFldStudentId').value;
                var clsid = document.getElementById('hdnFldClassId').value;
                var autosav = document.getElementById('hdnAutoSave').value;
                var isUpdate = document.getElementById('hdnFldIsUpdate').value;
                var frq = "";
                var drpfrq = document.getElementById('lblFrqCount' + inputBtn);
                if (drpfrq != null) {
                    frq = drpfrq.value;
                }

                var yesOrNoStatus = "";
                if ($('#btnYes' + inputBtn).hasClass("btnYesClick")) {
                    yesOrNoStatus = "1";
                    frq = "1";
                } else if ($('#btnNo' + inputBtn).hasClass("btnNoClick")) {
                    yesOrNoStatus = "0";
                    frq = "0";
                }



                if ($('#chkTOE' + inputBtn).length > 0) {
                    if ($('#chkTOE' + inputBtn).prop('checked') == true) {
                        if ($('#drpTimeAmPm' + inputBtn).val() == '0') {
                            amPm = 'AM';
                        }
                        else
                            amPm = 'PM';

                        toe = document.getElementById('txtTimeHr' + inputBtn).value + ":" + document.getElementById('txtTimeMin' + inputBtn).value + ":" + document.getElementById('txtTimeSec' + inputBtn).value + " " + amPm;

                        var h = document.getElementById('txtTimeHr' + behaveId).value;
                        var m = document.getElementById('txtTimeMin' + behaveId).value;
                        var s = document.getElementById('txtTimeSec' + behaveId).value;
                        toe = convStringToDateTimeFormat(h, m, s, amPm);
                    }
                    else {

                        var time = document.getElementById('txtTOE' + inputBtn).value;
                        toe = convStringToDateTimeFormat2(time);
                    }
                }

                if (yesOrNoStatus == "0" || yesOrNoStatus == "1") {
                    var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');
                    var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                    $.ajax(
                        {

                            type: "POST",
                            url: "dataSheetTimer.aspx/saveYesOrNo",
                            data: "{'MeasurementId':'" + inputBtn + "','StudentId':'" + stdid + "','yesOrNo':'" + yesOrNoStatus + "','FrequencyCount':'" + frq + "','toe':'" + toe + "','YNautoSave':'" + autosav + "','isBehUpdate':'" + isUpdate + "', 'chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",

                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            async: false,
                            success: function (data) {

                                var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px; '>Yes or No saved</div>";

                                $('body').append(alertBox);
                                // to check whether the normal score submitted within 5 min
                                if (data.d == "NoNormalData") {
                                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                                    $('body').append(alertBox);
                                    $('.alertBox').fadeOut(5000, function () {
                                        $(this).remove();
                                    });
                                }
                                $('.alertBox').fadeOut(5000, function () {
                                    $(this).remove();
                                });
                                if ($('#' + selToe).hasClass('bselect')) {
                                    $('#' + selToe).removeClass('bselect');
                                }
                                $('#' + selToe).addClass('redBox');
                            },
                            error: function (request, status, error) {
                                alert("Error");
                            }
                        });

                    $('.btnYesClick').removeClass().addClass('btnNothingClick');
                    $('.btnNoClick').removeClass().addClass('btnNothingClick');
                }
                else {
                    if (triggerEvntStatus == "false") {
                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please record data first</div>";
                        $('body').append(alertBox);
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    }
                }
            resetOther(thisElem);
            if ($('#chkIOA_' + inputBtn).prop('checked'))
                $('#chkIOA_' + inputBtn).prop('checked', false);
            var display = $('#txtTOE' + inputBtn).css('display');
            if (display == 'none')
                $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');
        }
        }

        function saveYesOrNoWithDur(inputBtn, thisElem) {

                var behaveId = inputBtn;
                var stdid = document.getElementById('hdnFldStudentId').value;
                var clsid = document.getElementById('hdnFldClassId').value;
                var autosav = document.getElementById('hdnAutoSave').value;
                var isUpdate = document.getElementById('hdnFldIsUpdate').value;
                var frq = "";
                var drpfrq = document.getElementById('lblFrqCount' + inputBtn);
                if (drpfrq != null) {
                    frq = drpfrq.value;
                }

                var yesOrNoStatus = "";
                if ($('#btnYes' + inputBtn).hasClass("btnYesClick")) {
                    yesOrNoStatus = "1";
                    frq = "1";
                } else if ($('#btnNo' + inputBtn).hasClass("btnNoClick")) {
                    yesOrNoStatus = "0";
                    frq = "0";
                }



                if ($('#chkTOE' + inputBtn).length > 0) {
                    if ($('#chkTOE' + inputBtn).prop('checked') == true) {
                        if ($('#drpTimeAmPm' + inputBtn).val() == '0') {
                            amPm = 'AM';
                        }
                        else
                            amPm = 'PM';

                        toe = document.getElementById('txtTimeHr' + inputBtn).value + ":" + document.getElementById('txtTimeMin' + inputBtn).value + ":" + document.getElementById('txtTimeSec' + inputBtn).value + " " + amPm;

                        var h = document.getElementById('txtTimeHr' + behaveId).value;
                        var m = document.getElementById('txtTimeMin' + behaveId).value;
                        var s = document.getElementById('txtTimeSec' + behaveId).value;
                        toe = convStringToDateTimeFormat(h, m, s, amPm);
                    }
                    else {

                        var time = document.getElementById('txtTOE' + inputBtn).value;
                        toe = convStringToDateTimeFormat2(time);
                    }
                }

                if (yesOrNoStatus == "0" || yesOrNoStatus == "1") {
                    var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');
                    var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                    $.ajax(
                        {

                            type: "POST",
                            url: "dataSheetTimer.aspx/saveYesOrNo",
                            data: "{'MeasurementId':'" + inputBtn + "','StudentId':'" + stdid + "','yesOrNo':'" + yesOrNoStatus + "','FrequencyCount':'" + frq + "','toe':'" + toe + "','YNautoSave':'" + autosav + "','isBehUpdate':'" + isUpdate + "', 'chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",

                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            async: false,
                            success: function (data) {

                                var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px; '>Yes or No saved</div>";

                                $('body').append(alertBox);
                                // to check whether the normal score submitted within 5 min
                                if (data.d == "NoNormalData") {
                                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                                    $('body').append(alertBox);
                                    $('.alertBox').fadeOut(5000, function () {
                                        $(this).remove();
                                    });
                                }
                                $('.alertBox').fadeOut(5000, function () {
                                    $(this).remove();
                                });
                                if ($('#' + selToe).hasClass('bselect')) {
                                    $('#' + selToe).removeClass('bselect');
                                }
                                $('#' + selToe).addClass('redBox');
                            },
                            error: function (request, status, error) {
                                alert("Error");
                            }
                        });

                    $('.btnYesClick').removeClass().addClass('btnNothingClick');
                    $('.btnNoClick').removeClass().addClass('btnNothingClick');
                }
                else {
                    if (triggerEvntStatus == "false") {
                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please record data first</div>";
                        $('body').append(alertBox);
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    }
                }
                resetOther(thisElem);
                if ($('#chkIOA_' + inputBtn).prop('checked'))
                    $('#chkIOA_' + inputBtn).prop('checked', false);
                var display = $('#txtTOE' + inputBtn).css('display');
                if (display == 'none')
                    $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');            
        }

        function saveDuraFreq(inputBtn, thisElem) {
            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {
                    saveOther(inputBtn, thisElem);
                    saveDuration2(inputBtn, thisElem);
                    //saveOther(inputBtn, thisElem);

                    resetOther(thisElem);
                }
                else {
                    if (triggerEvntStatus == "false") {
                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px; '>Please select an interval first</div>";
                    $('body').append(alertBox);
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
                    }

                }
            }
            else {
                saveOther(inputBtn, thisElem);
                saveDuration2(inputBtn, thisElem);
                //saveOther(inputBtn, thisElem);

                resetOther(thisElem);
            }
            //var parentBox = $(thisElem).parents('.tabContent');
            //var clockBtnStatus = $(parentBox).find('#btnStartStopTimer').val();
            //if (clockBtnStatus == "Start") {

            //    saveCountOther(inputBtn);
            //    saveCount(inputBtn);
            //    saveDurationOther(inputBtn);
            //    var time = parseInt($(thisElem).parents('.tabContent').find('.stpSeconds').html());
            //    saveTimeInDB(time, "", inputBtn);
            //}
            //else {
            //    alert('Timer Still Running');
            //}
            if ($('#chkIOA_' + inputBtn).prop('checked'))
                $('#chkIOA_' + inputBtn).prop('checked', false);
            var display = $('#txtTOE' + inputBtn).css('display');
            if (display == 'none')
                $('#chkTOE' + inputBtn).parent().find('.swt_lbl').trigger('click');

        }

        function saveDuraFreqYesOrNo(inputBtn, thisElem) {
            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {

                    //saveDuration2(inputBtn, thisElem);
                    saveOther(inputBtn, thisElem);
                    saveDuration2(inputBtn, thisElem);
                    saveYesOrNo(inputBtn, thisElem);
                    resetOther(thisElem);
                    //alert($(thisElem).parents('#dvTimeId_' + thisElem).find('.bselect').length);
                    $(thisElem).parents('.behavCont').find('.stpSeconds').text('0');
                }
                else {
                    //var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px; '>Time not selected</div>";
                    //$('body').append(alertBox);
                    //$('.alertBox').fadeOut(5000, function () {
                    //    $(this).remove();
                    //});

                }
            }
            else {
                //saveDuration2(inputBtn, thisElem);
                saveOther(inputBtn, thisElem);
                saveDuration2(inputBtn, thisElem);
                saveYesOrNo(inputBtn, thisElem);
                resetOther(thisElem);
                //alert($(thisElem).parents('#dvTimeId_' + thisElem).find('.bselect').length);
                $(thisElem).parents('.behavCont').find('.stpSeconds').text('0');
            }
        }

        function saveDurYesOrNo(inputBtn, thisElem) {
            if ($(thisElem).parents('.tabContent').find('.auto_box1').length > 0) {
                if ($(thisElem).parents('.tabContent').find('.bselect').length > 0) {

                    saveDuration2(inputBtn, thisElem);
                    saveYesOrNoWithDur(inputBtn, thisElem); //For fix select interval issue with duration and YN with interval

                    resetOther(thisElem);
                    $(thisElem).parents('.behavCont').find('.stpSeconds').text('0');
                }
                else {
                    if (triggerEvntStatus == "false") {
                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px; '>Please select an interval first</div>";
                    $('body').append(alertBox);
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
                }
            }
            }
            else {
                saveDuration2(inputBtn, thisElem);
                saveYesOrNo(inputBtn, thisElem);

                resetOther(thisElem);
                $(thisElem).parents('.behavCont').find('.stpSeconds').text('0');
            }
        }

        function saveOther(inputBtn, thisElem) {
            //txtFrequency,10
            //var freqCount = $(thisElem).parent().find('.txtFreq').val();

            // var drpfrq = document.getElementById('drpFrequency,' + behaveId);

            var drpfrq = document.getElementById('lblFrqCount' + inputBtn);
            var checktab = document.getElementById('hdnchecktabchange').value;
              if (parseInt(drpfrq.value) >= 0) {
                $('#hdnchecktabchange').val('True');
            } 
             if ($('#lblFrqCount' + inputBtn).length > 0) {
                var freqCount = drpfrq.value;

                if (freqCount >= 0 && freqCount != "") {
                var count = $(thisElem).parent().find('.lb').text();
                if (counter > 0 && (parseInt(freqCount) > 0 || (parseInt(freqCount)) == 0)) {
                    saveCountOther(inputBtn);
                    //saveCount(inputBtn);
                    counter = 0;
                }
                else
                    if (freqCount == null || freqCount == "") {  //|| freqCount == "0"
                        if (counter > 0) {
                            saveCount(inputBtn);
                            counter = 0;
                        }
                    }
                    else {
                        //if (parseInt(freqCount) > 0) {
                        saveCountOther(inputBtn);
                        counter = 0;
                        //}
                    }
            }
                else {
                    if (triggerEvntStatus == "false") {
                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:Red; top:10px; '>Please record data first</div>";
                    $('body').append(alertBox);
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
        }
            }
        }
        }

        function saveCount(inputBtn) {

            var behaveId = inputBtn;
            var stdid = document.getElementById('hdnFldStudentId').value;
            var clsid = document.getElementById('hdnFldClassId').value;
            var freq = $('#lblFrqCount' + behaveId).text();
            var isUpdate = document.getElementById('hdnFldIsUpdate').value;
            //btnFrq

            var amPm = "";
            var toe = "";

            if ($('#chkTOE' + behaveId).length > 0) {
                if ($('#chkTOE' + behaveId).prop('checked') == true) {
                    if ($('#drpTimeAmPm' + behaveId).val() == '0') {
                        amPm = 'AM';
                    }
                    else
                        amPm = 'PM';

                    toe = document.getElementById('txtTimeHr' + behaveId).value + ":" + document.getElementById('txtTimeMin' + behaveId).value + ":" + document.getElementById('txtTimeSec' + behaveId).value + " " + amPm;

                    var h = document.getElementById('txtTimeHr' + behaveId).value;
                    var m = document.getElementById('txtTimeMin' + behaveId).value;
                    var s = document.getElementById('txtTimeSec' + behaveId).value;
                    toe = convStringToDateTimeFormat(h, m, s, amPm);
                }
                else {

                    var time = document.getElementById('txtTOE' + behaveId).value;
                    toe = convStringToDateTimeFormat2(time);
                }
            }

            var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');

            $.ajax(
            {

                type: "POST",
                url: "dataSheetTimer.aspx/saveFrequency",
                data: "{'MeasurementId':'" + behaveId + "','StudentId':'" + stdid + "','Count':'" + freq + "','toe':'" + toe + "','isBehUpdate':'" + isUpdate + "','ClassId':'" + clsid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    var contents = data.d;

                    document.getElementById('lblFrqCount' + behaveId).innerHTML = contents;

                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px;'>Count Saved</div>";
                    $('body').append(alertBox);
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
                    if ($('#' + selToe).hasClass('bselect')) {
                        $('#' + selToe).removeClass('bselect');
                    }
                    $('#' + selToe).addClass('redBox');
                },
                error: function (request, status, error) {
                    alert("Error");
                }
            });

        }
        function saveCountOther(inputBtn) {

            var behaveId = inputBtn;
            var stdid = document.getElementById('hdnFldStudentId').value;
            var clsid = document.getElementById('hdnFldClassId').value;
            //var frq = document.getElementById('txtFrequency,' + behaveId).value;
            var drpfrq = document.getElementById('lblFrqCount' + inputBtn);
            //  var drpfrq = document.getElementById('drpFrequency,' + behaveId);

            //   var frq = drpfrq.options[drpfrq.selectedIndex].value;
            var frq = drpfrq.value;

            var hdnAutoSave = document.getElementById('hdnAutoSave').value;
            var isUpdate = document.getElementById('hdnFldIsUpdate').value;

            var toe = "";
            var amPm = "";
            if ($('#chkTOE' + behaveId).length > 0) {
                if ($('#chkTOE' + behaveId).prop('checked') == true) {
                    if ($('#drpTimeAmPm' + behaveId).val() == '0') {
                        amPm = 'AM';
                    }
                    else
                        amPm = 'PM';

                    toe = document.getElementById('txtTimeHr' + behaveId).value + ":" + document.getElementById('txtTimeMin' + behaveId).value + ":" + document.getElementById('txtTimeSec' + behaveId).value +" "+ amPm; //pramod

                    var h = document.getElementById('txtTimeHr' + behaveId).value;
                    var m = document.getElementById('txtTimeMin' + behaveId).value;
                    var s = document.getElementById('txtTimeSec' + behaveId).value;
                    toe = convStringToDateTimeFormat(h, m, s, amPm);
                }
                else {

                    var time = document.getElementById('txtTOE' + behaveId).value;
                    toe = convStringToDateTimeFormat2(time);
                }
            }

            //var freq = document.getElementById('lblFrqCount' + behaveId).value;
            if (frq != "") {
                if (hdnAutoSave == "True") { }
                else {
                    var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');
                    var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                    $.ajax(
                    {

                        type: "POST",
                        url: "dataSheetTimer.aspx/saveFrequencyOther",
                        data: "{'MeasurementId':'" + behaveId + "','StudentId':'" + stdid + "','frequency':'" + frq + "','FrqSave':'" + hdnAutoSave + "','toe':'" + toe + "','isBehUpdate':'" + isUpdate + "','chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var contents = data.d;
                            
                            //      document.getElementById('lblFrqCount' + behaveId).innerHTML = contents;
                            //document.getElementById('txtFrequency,' + behaveId).value = '';
                            //     document.getElementById('drpFrequency,' + behaveId).value = '';
                            drpfrq.value = '';
                            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px;'>Count Saved</div>";

                            $('body').append(alertBox);
                            // to check whether the normal score submitted within 5 min
                            if (data.d == "NoNormalData") {
                                var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                                $('body').append(alertBox);
                                $('.alertBox').fadeOut(5000, function () {
                                    $(this).remove();
                                });
                            }
                            $('.alertBox').fadeOut(5000, function () {
                                $(this).remove();
                            });
                            if ($('#' + selToe).hasClass('bselect')) {
                                $('#' + selToe).removeClass('bselect');
                            }
                            $('#' + selToe).addClass('redBox');
                        },
                        error: function (request, status, error) {
                            alert("Error");
                        }
                    });
                }
            }
        }

        function convStringToDateTimeFormat(hrs, min, sec, ampm) {

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();

            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            if (hrs < 10) {
                hrs = '0' + hrs
            }
            if (min < 10) {
                min = '0' + min
            }
            if (sec < 10) {
                sec = '0' + sec
            }


            var timeString = yyyy + '-' + mm + '-' + dd + " " + hrs + ":" + min + ":" + sec + " " + ampm;
            return timeString;
        }

        function convStringToDateTimeFormat2(time) {

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();

            //if (dd < 10) {
            //    dd = '0' + dd
            //}
            //if (mm < 10) {
            //    mm = '0' + mm
            //}


            var timeString = yyyy + '-' + mm + '-' + dd + " " + time;
            return timeString;
        }

        function saveDurationOther(inputBtn) {

            var behaveId = inputBtn;
            var stdid = document.getElementById('hdnFldStudentId').value;
            var clsid = document.getElementById('hdnFldClassId').value;
            var drtion = document.getElementById('txtDuration,' + behaveId).value;



            var isUpdate = document.getElementById('hdnFldIsUpdate').value;
            var autosav = document.getElementById('hdnAutoSave').value;
            var toe = "";
            var amPm = "";
            if ($('#chkTOE' + behaveId).length > 0) {
                if ($('#chkTOE' + behaveId).prop('checked') == true) {
                    if ($('#drpTimeAmPm' + behaveId).val() == '0') {
                        amPm = 'AM';
                    }
                    else
                        amPm = 'PM';

                    toe = document.getElementById('txtTimeHr' + behaveId).value + ":" + document.getElementById('txtTimeMin' + behaveId).value + ":" + document.getElementById('txtTimeSec' + behaveId).value + " " + amPm;

                    if (drtion > 0) {

                    }
                    else {
                        drtion = 0;
                    }
                    if (drtion >= "0") {
                        var h = document.getElementById('txtTimeHr' + behaveId).value;
                        var m = document.getElementById('txtTimeMin' + behaveId).value;
                        var s = document.getElementById('txtTimeSec' + behaveId).value;
                        toe = convStringToDateTimeFormat(h, m, s, amPm);
                    }
                }
                else {

                    var time = document.getElementById('txtTOE' + behaveId).value;
                    toe = convStringToDateTimeFormat2(time);
                }
            }


            if (drtion != "" || (drtion == 0 && checktab == 'True')) {
                if (drtion == "" && autosav == "True") { }
                else {
                    var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');
                    var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
                    $.ajax(
                    {

                        type: "POST",
                        url: "dataSheetTimer.aspx/saveDurationOther",
                        data: "{'MeasurementId':'" + behaveId + "','StudentId':'" + stdid + "','duration':'" + drtion + "','FrqSave':'" + autosav + "','toe':'" + toe + "','isBehUpdate':'" + isUpdate + "','chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            
                            document.getElementById('txtDuration,' + behaveId).value = '';

                            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px; '>Duration Saved</div>";

                            $('body').append(alertBox);
                            // to check whether the normal score submitted within 5 min
                            if (data.d == "NoNormalData") {
                                var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                                $('body').append(alertBox);
                                $('.alertBox').fadeOut(5000, function () {
                                    $(this).remove();
                                });
                            }
                            $('.alertBox').fadeOut(5000, function () {
                                $(this).remove();
                            });
                            if ($('#' + selToe).hasClass('bselect')) {
                                $('#' + selToe).removeClass('bselect');
                            }
                            $('#' + selToe).addClass('redBox');
                        },
                        error: function (request, status, error) {
                            alert("Error");
                        }
                    });
                }
            }

            //$('#hdnAutoSave').val('False');
        }
        //THIS FUNCTION IS TO SAVE THE TIMER VALUE IN THE DATABASE. CALLED WHEN TIMER IS STOPPED.
        function saveTimeInDB(inputBtn, thisElem) {
           
            var behaveId = inputBtn;
            var stdid = document.getElementById('hdnFldStudentId').value;
            var clsid = document.getElementById('hdnFldClassId').value;
            var drtion = $(thisElem).parents().find('.stpSeconds').html();
            var autosav = document.getElementById('hdnAutoSave').value;
            var isUpdate = document.getElementById('hdnFldIsUpdate').value;
            var amPm = "";
            var toe = "";
            if ($('#chkTOE' + behaveId).length > 0) {
                if ($('#chkTOE' + behaveId).prop('checked') == true) {
                    if ($('#drpTimeAmPm' + behaveId).val() == '0') {
                        amPm = 'AM';
                    }
                    else
                        amPm = 'PM';

                    toe = document.getElementById('txtTimeHr' + behaveId).value + ":" + document.getElementById('txtTimeMin' + behaveId).value + ":" + document.getElementById('txtTimeSec' + behaveId).value + " " + amPm;


                    if (drtion > 0) {

                    }
                    else {
                        drtion = 0;

                    }
                    if (drtion >= "0") {
                        var h = document.getElementById('txtTimeHr' + behaveId).value;
                        var m = document.getElementById('txtTimeMin' + behaveId).value;
                        var s = document.getElementById('txtTimeSec' + behaveId).value;
                        toe = convStringToDateTimeFormat(h, m, s, amPm);
                    }
                }
                else {

                    var time = document.getElementById('txtTOE' + behaveId).value;
                    toe = convStringToDateTimeFormat2(time);
                }
            }
            var selToe = $('#dvTimeId_' + behaveId + '').find('.simple').attr('id');
            var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';

            $.ajax(
            {

                type: "POST",
                url: "dataSheetTimer.aspx/saveDurationOther",
                data: "{'MeasurementId':'" + behaveId + "','StudentId':'" + stdid + "','duration':'" + drtion + "','FrqSave':'" + autosav + "','toe':'" + toe + "','isBehUpdate':'" + isUpdate + "','chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    
                    document.getElementById('txtDuration,' + behaveId).value = '';

                    var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px; '>Duration Saved</div>";

                    $('body').append(alertBox);
                    // to check whether the normal score submitted within 5 min
                    if (data.d == "NoNormalData") {
                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                        $('body').append(alertBox);
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    }
                    $('.alertBox').fadeOut(5000, function () {
                        $(this).remove();
                    });
                    if ($('#' + selToe).hasClass('bselect')) {
                        $('#' + selToe).removeClass('bselect');
                    }
                    $('#' + selToe).addClass('redBox');
                },
                error: function (request, status, error) {
                    alert("Error");
                }
            });
        }

        function saveStartTime(elem) {

            var pageWidth = $(window).width();
            var msgLeft = ((pageWidth / 2) - 50) + 10;
            var thisParent = $(elem).parent().attr('id');
            var measureId = $(elem).find('.measurementId').html();
            var c_studId = $(elem).find('.c_studentId').html();
            var clsid = document.getElementById('hdnFldClassId').value;
            var thisElement = $(elem).parents().eq(4);


            $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/34.gif'/>");
            var chkIOA = ($('#chkIOA_' + behaveId).prop('checked')) ? 'true' : 'false';
            $.ajax(
            {

                type: "POST",
                url: "dataSheetTimer.aspx/saveDurationStartTime",
                data: "{'MeasurementId':'" + measureId + "','StudentId':'" + c_studId + "','chkIOA':'" + chkIOA + "','ClassId':'" + clsid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    
                    if (data.d != '-1') {
                        $(elem).find('.retId').html(data.d);

                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:yellow; top:10px; '>Duration Saved</div>";
                        $('body').append(alertBox);
                        $('.alertBox').html('Clock Started').fadeOut(5000, function () {
                            $(this).remove();
                        });


                        $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/clockRunning.gif'/>");
                    }
                    else {
                        $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/notsave.png'/>");

                        $(elem).remove();
                        addStopwatch(thisParent);

                        $('body').append(alertBox);
                        $('.alertBox').html('Clock Starting Failed. Try again!').fadeOut(10000, function () {
                            $(this).remove();
                        });
                    }
                    // to check whether the normal score submitted within 5 min
                    if (data.d == "NoNormalData") {
                        var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative; background-color:red; top:10px;'>No Normal Score Within 5 min.</div>";

                        $('body').append(alertBox);
                        $('.alertBox').fadeOut(5000, function () {
                            $(this).remove();
                        });
                    }


                },
                error: function (request, status, error) {
                    alert("Error");
                    $(thisElement).find('.catFreqDiv').html("");

                }
            });
        }
    </script>

    <script type="text/javascript">
        function closePOP() {
            $('#HdrDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });

        }
        function showPop(type) {
            if (type == 1) {
                $('#tblNewIOA').css('display', 'none');
                $('#tblIOAndNorm').css('display', 'inline-table');
            }
            if (type == 2) {
                $('#tblIOAndNorm').css('display', 'none');
                $('#tblNewIOA').css('display', 'inline-table');
            }
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#HdrDiv').fadeIn();
            });
        }
    </script>
</head>
<body style="margin: 0px;">
    <%--<div id="msgDivtop" style="position:fixed;top:0;left:0;width:200px;border:1px solid red; height:400px;overflow-y:auto;"></div>--%>
    <form id="form1" runat="server">


        <div>
            <div class="mainHeader" style="max-width: 100% !important; margin: 0;">
                Behavior
            </div>
            <div class="topTabHeader">
                <table class="topTabTable">
                    <tr class="topTabtr">
                        <td class="topTabtd" onclick="loadTopTabs(2,this);" id="topTab2">Deceleration
                        </td>
                        <td class="topTabtd" onclick="loadTopTabs(1,this);" id="topTab1">Acceleration
                        </td>
                    </tr>
                </table>
            </div>
            <div class="rightPartContainerTimer">


                <!------------------------------------------------------------------------ Right Container-------------------------------------------------------------------->



                <div class="clear"></div>
                <table align="left" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <!-- Tabs -->
                            <div id="tabs" class="scrollDiv">
                                <ul id="tabUl">
                                </ul>

                            </div>
                        </td>
                    </tr>
                </table>

                <!------------------------------------------------------------------------ Right Container-------------------------------------------------------------------->

            </div>
        </div>
        <div>
            <div class="rightPartContainerTimerNullVal">


                <!------------------------------------------------------------------------ Right Container-------------------------------------------------------------------->

                <%-- <table style="width: 100%">
                    <tr>
                        <td>
                            <h3>Behavior</h3>
                        </td>
                        <td style="text-align: right">
                            <asp:ImageButton ID="btnRefresh1" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                        </td>
                    </tr>
                </table>--%>
                <div class="clear"></div>

                <h3>No Behavior Found</h3>
                <!------------------------------------------------------------------------ Right Container-------------------------------------------------------------------->

            </div>
        </div>

        <asp:HiddenField ID="hdnFldUl" runat="server" />
        <asp:HiddenField ID="hdnFldUlId" runat="server" />
        <asp:HiddenField ID="hdnFldStudentId" runat="server" />
        <asp:HiddenField ID="hdnFldClassId" runat="server" />
        <asp:HiddenField ID="hdnFldBehaveType" runat="server" />
        <asp:HiddenField ID="hdnFldFrequencyCnt" runat="server" />
        <asp:HiddenField ID="hdnFldIsAcceleration" runat="server" />
        <asp:HiddenField ID="hdnAutoSave" runat="server" Value="False" />
        <asp:HiddenField ID="hdnFldBehavDefinition" runat="server" />
        <asp:HiddenField ID="hdnFldBehavStrategy" runat="server" />
        <asp:HiddenField ID="hdnFldStartTime" runat="server" />
        <asp:HiddenField ID="hdnFldEndTime" runat="server" />
        <asp:HiddenField ID="hdnFldInterval" runat="server" />
        <asp:HiddenField ID="hdnFldPeriod" runat="server" />
        <asp:HiddenField ID="hdnFldBehavId" runat="server" />
        <asp:HiddenField ID="hdnFldTOE" runat="server" />
        <asp:HiddenField ID="hdnFldSavedTimes" runat="server" />

        <asp:HiddenField ID="hdnFldIsUpdate" runat="server" Value="True"/>
        <asp:HiddenField ID="hdnFldFirstLoad" runat="server" Value="True" />
        <asp:HiddenField ID="hdnchecktabchange" runat="server" Value="True"/>

    </form>
</body>
<script type="text/javascript">
    $(function () {

        adjustStyle();
        $(window).resize(function () {
            adjustStyle();
        });
    });
</script>
</html>
