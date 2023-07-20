<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/Calender.aspx.cs" Inherits="StudentBinder_Calender" EnableEventValidation="false" %>

<head id="head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../Administration/JS/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery-ui.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        .weekday {
            background-color: white;
        }

            .weekday:hover {
                background-color: #F8F1C6;
                cursor: pointer;
            }

        .holiday {
            background-color: #F2EFEF;
        }

            .holiday:hover {
                background-color: #F8F1C6;
            }

        .fontwrap {
            overflow: hidden;
            text-overflow: ellipsis;
            -ms-text-overflow: ellipsis;
        }

        .web_dialog_overlay {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
            text-align: center;
        }

        .web_dialog {
            display: none;
            position: fixed;
            width: 320px;
            height: 170px;
            left: 50%;
            margin-left: -162px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 102;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

        .web_dialog2 {
            display: none;
            position: fixed;
            width: 290px;
            height: 210px;
            left: 50%;
            margin-left: -162px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

        .txtBoxClass {
            border: 1px solid;
            border-color: silver #D9D9D9 #D9D9D9;
            width: 100%;
            padding: 2px;
        }

        .NFBtn {
            border-style: none;
            border-color: inherit;
            border-width: medium;
            width: auto;
            padding: 5px 15px;
            font-weight: bold;
            cursor: pointer;
            color: black;
            vertical-align: middle;
            width: 100px;
            background-color: none;
        }

        .divLeftPanel {
            margin-top: 33px;
            margin-left: 4px;
            display: none;
            width: 260px;
            float: left;
            position: absolute;
            z-index: 1002;
            height: 500px;
            text-align: center;
            background-color: #ECE9D8;
            border-bottom: 1px solid black;
            border-left: 1px solid black;
            border-right: 1px solid black;
        }

        .divRightPanel {
            width: 100%;
            float: left;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .divMenu {
            width: 100%;
            height: 100%;
            float: left;
            vertical-align: central;
            cursor: pointer;
        }

        .popclass {
            text-align: center;
            border: 1px solid lightgray;
            background: White;
            width: 430px;
            height: 210px;
            display: none;
            position: absolute;
            z-index: 999;
        }

        .divCell {
            width: auto;
            height: 30px;
            border: 1px solid black;
        }

            .divCell:hover {
                cursor: pointer;
                background-color: #F8F1C6;
            }
    </style>
    <script>

        printDivCSS = new String('<link href="myprintstyle.css" rel="stylesheet" type="text/css">')
        function printDiv(divId) {
            //var det1 = document.getElementsByClassName('det');
            //for (var i = 0; i < det1.length; i++) {
            //    det1[i].style.visibility = "hidden";
            //}
            var det1 = document.getElementById('view');
            det1.parentNode.style.visibility = "hidden";
            window.frames["print_frame"].document.body.innerHTML = printDivCSS + '<div align="center"style="font-weight:bold;margin-bottom: 10px;">' + document.getElementById('lblDate').innerHTML + '</div>' + document.getElementById('UpdatePanel3').innerHTML;
            window.frames["print_frame"].window.focus();
            window.frames["print_frame"].window.print();
            det1.parentNode.style.visibility = "visible";
        }
        window.onafterprint = function () {
            var det1 = document.getElementById('view');
            det1.parentNode.style.visibility = "visible";
        //    var det1 = document.getElementsByClassName('det');
        //    for (var i = 0; i < det1.length; i++) {
        //        det1[i].style.visibility = "visible";
        //    }
        }
    </script>
    <script type="text/javascript">
        function view(viewDiv) {
            HideDialog();
            HideEditPopup();
            HidePopup();
            //var div = document.getElementById("Calndr");
            var div = document.getElementsByClassName("divLeftPanel");
            div = div[0];
            //div.style.display = 'block';
            if (div.style.display == 'block') {
                div.style.display = 'none';
                var img = document.getElementById("imgArrow");
                img.src = '../Administration/images/calendar.gif';
                var img1 = document.getElementById("imgArrow1");
                img1.src = '../Administration/images/downarrow.png';
                viewDiv.style.background = '#ECE9D8';
            }
            else {
                div.style.display = 'block';
                var img = document.getElementById("imgArrow");
                img.src = '../Administration/images/calendar.gif';
                var img1 = document.getElementById("imgArrow1");
                img1.src = '../Administration/images/uparrow.png';
                viewDiv.style.background = '#DFDFC7';
            }
        }
        function hide() {
            //var div = document.getElementById("Calndr");
            var div = document.getElementsByClassName("divLeftPanel");
            div = div[0];
            div.style.display = 'none';
        }
    </script>
    <script type="text/javascript">
        var mouseX, mouseY, windowWidth, windowHeight;
        var popupLeft, popupTop;
        var endTime;
        var editEndtime;
        //function to find the position of mouse pointer
        $(document).ready(function () {
            LoadEndTime();
            $(document).mousemove(function (e) {
                mouseX = e.pageX;
                mouseY = e.pageY;
                //To Get the relative position
                if (this.offsetLeft != undefined)
                    mouseX = e.pageX - this.offsetLeft;
                if (this.offsetTop != undefined)
                    mouseY = e.pageY; -this.offsetTop;

                if (mouseX < 0)
                    mouseX = 0;
                if (mouseY < 0)
                    mouseY = 0;

                windowWidth = $(window).width() + $(window).scrollLeft();
                windowHeight = $(window).height() + $(window).scrollTop();
            });


        });
        function ShowDialog(modal) {
            //var div = document.getElementById("Calndr");
            var div = document.getElementsByClassName("divLeftPanel");
            div = div[0];
            //div.style.display = 'block';
            if (div.style.display == 'block') {
                div.style.display = 'none';
                var img = document.getElementById("imgArrow");
                img.src = '../Administration/images/downarrow.png';
            }
            $("#overlay").show();
            $("#PopUpStud").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }


        function HideDialog() {
            $("#overlay").hide();
            $("#PopUpStud").fadeOut(300);

        }
        var parntDivTag = null; var childDivTag = null; var nRetVal = null; var retLP = null;
        function LoadEndTime() {
           
            PageMethods.GetValues("SELECT EndTime FROM SchoolCal WHERE EndTime=(SELECT MAX(EndTime) FROM SchoolCal) GROUP BY EndTime", OnSuccessEndTimeReturn, OnFailure);
           
        }
        var strtTime = null;
        //function which shows the popup window for creating an event
        function popup(divid, time, time1) {
            var stime;
            var smin;
            var shour;
            var start;
            HideEditPopup();
            //LoadEndTime();
            $("#pop").show();
          
            strtTime = time;
            strtTime1 = time1;
            stime = time1.split(":");
            shour = stime[0];
            smin = stime[1];
            if (strtTime1 >= "12:00" && strtTime1 < "24:00")
            {
                if (shour!=12)
                {
                    shour = parseInt(shour, 10) - 12;
                    if (shour.toString().length == 1)
                        shour = '0' + shour;
                }
                
              
                time1 = shour +":"+ smin +" PM";
            }
            else if (strtTime1 >= "24:00") {
                shour = parseInt(shour, 10) - 12;
                if (shour.toString().length == 1)
                    shour = '0' + shour;
                time1 = shour + ":" + smin + " PM";
            }

            else {
                if (shour.toString().length == 1)
                    shour = '0' + shour;
                time1 = shour + ":" + smin + " AM";
            }

            document.getElementById('<%=lblFrom.ClientID %>').innerHTML = time1;
            document.getElementById('<%=hfCellID.ClientID %>').value = divid.id;
            document.getElementById('<%=ddlTime.ClientID %>').options.length = 0;

            if (document.getElementById('<%=ddlLP.ClientID %>').selectedIndex > 0) {
                document.getElementById('<%=ddlLP.ClientID %>').selectedIndex = 0;
            }

            var hfmode = document.getElementById('<%=hfMode.ClientID%>');
            var ress = document.getElementById('<%=hidRes_Value.ClientID%>');

            if (hfmode.value == 'Week')
                PageMethods.GetEndTime(divid.parentNode.id, 'Week',ress.value, OnSuccessgetEndTime, OnFailure);
            if (hfmode.value == 'Day')
                PageMethods.GetEndTime('12-10-2012', 'Day', ress.value, OnSuccessgetEndTime, OnFailure);
            //fillDrop(document.getElementById('<%=ddlTime.ClientID %>'), time);
            var popupWidth = $("#pop").outerWidth();
            var popupHeight = $("#pop").outerHeight();

            if (mouseX + popupWidth > windowWidth)
                popupLeft = mouseX - popupWidth;
            else
                popupLeft = mouseX;

            if (mouseY + popupHeight > windowHeight)
                popupTop = mouseY - popupHeight;
            else
                popupTop = mouseY;

            if (popupLeft < $(window).scrollLeft()) {
                popupLeft = $(window).scrollLeft();
            }

            if (popupTop < $(window).scrollTop()) {
                popupTop = $(window).scrollTop();
            }

            if (popupLeft < 0 || popupLeft == undefined)
                popupLeft = 0;
            if (popupTop < 0 || popupTop == undefined)
                popupTop = 0;

            $("#pop").offset({ top: popupTop, left: popupLeft });
            parntDivTag = divid;

            var result_style = document.getElementById('cusTxt').style;
            result_style.display = 'none';

            document.getElementById('ddCustmMsg').value = "";

        }
        function OnSuccessEndTimeReturn(result) {
            if (result == "No datas found!! Please update the School Calendar Table.")
                alert(result);
            else

                endTime = result.substring(0, 5);
           
        }

        function OnSuccessgetEndTime(result) {
            if (result.length > 4) {
                endTime = result.substring(0, 5);
                fillDrop(document.getElementById('<%=ddlTime.ClientID %>'), strtTime, strtTime1);
            }
        }

        //function which shows the popup to delete the clicked event
        function editPopup(divEvent) {
            //LoadEndTime();
            HidePopup();
            //getTimes(divEvent.id);
            //PageMethods.GetValues("SELECT SUBSTRING(CONVERT(varchar,StartTime),0,6) as starttime FROM StdtLPSched WHERE StdtLPSchedId=" + divEvent.id, OnSuccessStartTimeReturn, OnFailure);
            //PageMethods.GetValues("SELECT SUBSTRING(CONVERT(varchar,EndTime),0,6) as endtime FROM StdtLPSched WHERE StdtLPSchedId=" + divEvent.id, EndTimeReturnForEdit, OnFailure);
            $("#editPop").show();

            var popupWidth = $("#editPop").outerWidth();
            var popupHeight = $("#editPop").outerHeight();

            if (mouseX + popupWidth > windowWidth)
                popupLeft = mouseX - popupWidth;
            else
                popupLeft = mouseX;

            if (mouseY + popupHeight > windowHeight)
                popupTop = mouseY - popupHeight;
            else
                popupTop = mouseY;

            if (popupLeft < $(window).scrollLeft()) {
                popupLeft = $(window).scrollLeft();
            }

            if (popupTop < $(window).scrollTop()) {
                popupTop = $(window).scrollTop();
            }

            if (popupLeft < 0 || popupLeft == undefined)
                popupLeft = 0;
            if (popupTop < 0 || popupTop == undefined)
                popupTop = 0;

            $("#editPop").offset({ top: popupTop, left: popupLeft });
            parntDivTag = divEvent.parentNode;
        }
        //function getTimes(eventId) {

        //    PageMethods.GetValues("SELECT SUBSTRING(CONVERT(varchar,EndTime),0,6) as endtime FROM StdtLPSched WHERE StdtLPSchedId=" + eventId, EndTimeReturnForEdit, OnFailure);
        //    PageMethods.GetValues("SELECT SUBSTRING(CONVERT(varchar,StartTime),0,6) as starttime FROM StdtLPSched WHERE StdtLPSchedId=" + eventId, OnSuccessStartTimeReturn, OnFailure);
        //}
        function EndTimeReturnForEdit(result) {
            //document.getElementById('<%=ddlEndTime.ClientID %>').selectedValue = result;
            editEndtime = result;
            //PageMethods.GetValues("SELECT SUBSTRING(CONVERT(varchar,StartTime),0,6) as starttime FROM StdtLPSched WHERE StdtLPSchedId=" + childDivTag.id, OnSuccessStartTimeReturn, OnFailure);           
            PageMethods.GetValues2("SELECT FORMAT(CAST(StartTime AS DATETIME),'hh:mm tt') as starttime FROM StdtLPSched WHERE StdtLPSchedId=" + childDivTag.id, childDivTag.id, OnSuccessStartTimeReturn, OnFailure);
        }
        function OnSuccessStartTimeReturn(result) {
            document.getElementById('<%=lblTime.ClientID %>').innerHTML = result;
            document.getElementById('<%=ddlEndTime.ClientID %>').options.length = 0;
            strtTime = result;
            var hfmode = document.getElementById('<%=hfMode.ClientID%>');
            var ress = document.getElementById('<%=hidRes_Value.ClientID%>');
        
            if (hfmode.value == 'Week')
                PageMethods.GetEndTime(parntDivTag.parentNode.id, 'Week', ress.value.toString(), OnSuccessgetEndTimeEdit, OnFailure);
            else
                PageMethods.GetEndTime('12-10-2012', 'Day',ress.value.toString(), OnSuccessgetEndTimeEdit, OnFailure);
            //fillDrop(document.getElementById('<%=ddlEndTime.ClientID %>'), result);

        }
        function OnSuccessgetEndTimeEdit(result) {
            var strtTime1;
            if (result.length > 4) {
                endTime = result.substring(0, 5);
                if (strtTime == "00:00")
                    strtTime = "12:00";
                if (strtTime == "00:30")
                    strtTime = "12:30";

                if (strtTime >= "12:00") {
                    if (strtTime != "12:00" || strtTime != "12:30")
                    strtTime1 = parseInt(strtTime)- 12;
                    strtTime1 = strtTime1 + " PM";
                }
                else {
                    strtTime1 = strtTime + " AM";
                }
                

                fillDrop(document.getElementById('<%=ddlEndTime.ClientID %>'), strtTime, strtTime1);
                var ddlEndtime = document.getElementById('<%=ddlEndTime.ClientID %>');
                for (var i = 0; i < ddlEndtime.options.length; i++) {
                    if (ddlEndtime.options[i].text == editEndtime) {
                        ddlEndtime.selectedIndex = i;
                        break;
                    }
                }
                editEventWidLP(childDivTag);
            }
        }
        //function to fill the time in dropdownlist
        function fillDrop(ddl, startTime, startTime3) {

            

            var index = 0;
            var stime = new Array();
            var times = new Array();
            var startTime1 = startTime3;
            var startTime2 = startTime;
            var tt = "";
            var check = 0;
            var edit = "";
            var edit1 = "";
            
            edit = startTime.split(" ");
            if (edit[1] == "AM")
                startTime1 = edit[0];
            if (edit[1] == "PM") {
                edit1 = edit[0].split(":");
                //if (edit1[1] == "30") {
                //    edit1[0] = parseInt(edit1[0], 10) + 1;
                //    edit1[1] = "00";
                //}
                //else
                    //edit1[1] = "30";
                //if (edit1 = "12.30")
                //    edit1 = "01.00";
                if(parseInt(edit1[0], 10)!=12)
                    edit1[0] = parseInt(edit1[0], 10) + 12;
                    startTime1 = edit1[0] + ":" + edit1[1];
            }

            
            //times[index] = startTime;
            if (endTime == '23:59') {
                endTime='24:00';
            }

            while (startTime1 != endTime) {
                stime = startTime2.split(":");
                stime1 = startTime1.split(":");
                var shour = stime[0];
                var smin = stime[1];
                var shour1 = stime1[0];
                var smin1 = stime1[1];

                if (shour == 12 && smin==30)
                {
                    
                    shour = shour - 12;
                }
                if (edit[1] != "AM" || edit[1] != "PM") {
                    if ((parseInt(shour1, 10) + 1) >= 12) {
                        tt = "PM";
                    }

                    else {
                        tt = "AM";
                    }
                }

                if (smin == "30") {
                    shour = parseInt(shour, 10) + 1;
                    smin = "00";
                }
                else {
                    smin = "30";
                }
                if (shour == "11") {
                    tt = "AM";
                }
                if (smin1 == "30") {
                    shour1 = parseInt(shour1, 10) + 1;
                    smin1 = "00";
                }
                else {
                    smin1 = "30";
                }
                if (shour1 == '24')
                    tt = "AM";
                if (shour1.toString().length == 1)
                    shour1 = '0' + shour1;
                startTime1 = shour1 + ':' + smin1;

                if (shour.toString().length == 1)
                    shour = '0' + shour;
                startTime2 = shour + ':' + smin;
                startTime = shour + ':' + smin + ' ' + tt;
                times[index] = startTime;
                index = index + 1;
            }
            //alert(endTime + "   " + startTime);
            //var times = new Array("08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00");
            var check = 0;
            var pos = 0;
            for (var index = 0; index < times.length; index++) {
                //if (check == 1) {
                ddl.options[index] = new Option(times[index], times[index]);
                pos++;
                //}
                //if (startTime == times[index])
                //    check = 1;
            }

            check = 0;
        }
        function editEventWidLP(divChild) {
            //divChild.style.zIndex = "2";

            editPopup(divChild);
        }
        //fuction which calls when user needs to edit the clicked event...
        function editEvent(divChild, eventHandle) {
            if (!eventHandle) var eventHandle = window.event;
            if (eventHandle) eventHandle.cancelBubble = true;
            if (eventHandle.stopPropagation) eventHandle.stopPropagation();
            HideEditPopup();
        
            PageMethods.GetValues2("SELECT LessonPlanName FROM LessonPlan LP INNER JOIN StdtLPSched LPSched ON LPSched.LPId=LP.LessonPlanId WHERE StdtLPSchedId=" + divChild.id, divChild.id, OnSuccessLPReturn, OnFailure);
            var divParnt = divChild.parentNode;
            parntDivTag = divParnt;
            childDivTag = divChild;
            //editPopup(divChild);



        }
        function OnSuccessLPReturn(result) {
            retLP = result;
            var ddlLP = document.getElementById('<%=ddlEditLP.ClientID%>');
            var splittedStr = new Array();
            splittedStr = result.split("_");
             
            if (splittedStr != null) {
                if (splittedStr[0] == "Other") {
                    var result_style = document.getElementById('cusTxt1').style;
                    result_style.display = 'table-row';

                    document.getElementById('ddCustmsgEdit').value = splittedStr[1];

                    for (var i = 0; i < ddlLP.options.length; i++) {
                        if (ddlLP.options[i].text == splittedStr[0]) {
                            ddlLP.selectedIndex = i;
                            break;
                        }
                    }
                }
                else {
                    var result_style = document.getElementById('cusTxt1').style;
                    result_style.display = 'none';
                    for (var i = 0; i < ddlLP.options.length; i++) {
                        if (ddlLP.options[i].text == result) {
                            ddlLP.selectedIndex = i;
                            break;
                        }
                    }
                }
            }
            //PageMethods.GetValues("SELECT SUBSTRING(CONVERT(varchar,EndTime),0,6) as endtime FROM StdtLPSched WHERE StdtLPSchedId=" + childDivTag.id, EndTimeReturnForEdit, OnFailure);
            PageMethods.GetValues2("SELECT FORMAT(CAST(EndTime AS DATETIME),'hh:mm tt') as endtime FROM StdtLPSched WHERE StdtLPSchedId=" + childDivTag.id, childDivTag.id, EndTimeReturnForEdit, OnFailure);

        }
        //function to update the selected event
        function UpdateEvent() {
            var ddlStd = document.getElementById('<%=ddlStudents.ClientID %>');
            var ddlEndTime = document.getElementById('<%=ddlEndTime.ClientID %>');
            var ddleditLP = document.getElementById('<%=ddlEditLP.ClientID%>');
            if (ddleditLP.selectedIndex == 0) {
                alert("Please select a LessonPlan");
            }
            else {
                //if (ddlStd.disabled == true) {
                //    PageMethods.UpdateEvent(childDivTag.id, parntDivTag.parentNode.id, ddlEndTime.options[ddlEndTime.selectedIndex].text, ddleditLP.options[ddleditLP.selectedIndex].value, OnSuccessUpdate, OnFailure);
                //}
                //else {
                //    PageMethods.UpdateEvent(childDivTag.id, ddlStd.options[ddlStd.selectedIndex].value, ddlEndTime.options[ddlEndTime.selectedIndex].text, ddleditLP.options[ddleditLP.selectedIndex].value, OnSuccessUpdate, OnFailure);
                //}

                var hfmode = document.getElementById('<%=hfMode.ClientID%>');
                var othdescedit = document.getElementById('<%=ddCustmsgEdit.ClientID %>').value;

                if (hfmode.value == 'Week') {
                    if ((ddleditLP.value == 00000) && (ddleditLP.options[ddleditLP.selectedIndex].innerHTML == "Other") && othdescedit != "") {
                        PageMethods.UpdateEvent(childDivTag.id, ddlStd.options[ddlStd.selectedIndex].value, ddlEndTime.options[ddlEndTime.selectedIndex].text, ddleditLP.options[ddleditLP.selectedIndex].value, 'Week', othdescedit, OnSuccessUpdate, OnFailure);
                    }
                    else {
                        PageMethods.UpdateEvent(childDivTag.id, ddlStd.options[ddlStd.selectedIndex].value, ddlEndTime.options[ddlEndTime.selectedIndex].text, ddleditLP.options[ddleditLP.selectedIndex].value, 'Week', null, OnSuccessUpdate, OnFailure);
                    }
                }
                if (hfmode.value == 'Day') {
                    if ((ddleditLP.value == 00000) && (ddleditLP.options[ddleditLP.selectedIndex].innerHTML == "Other") && othdescedit != "") {
                        PageMethods.UpdateEvent(childDivTag.id, parntDivTag.parentNode.id, ddlEndTime.options[ddlEndTime.selectedIndex].text, ddleditLP.options[ddleditLP.selectedIndex].value, 'Day', othdescedit, OnSuccessUpdate, OnFailure);
                    }
                    else {
                        PageMethods.UpdateEvent(childDivTag.id, parntDivTag.parentNode.id, ddlEndTime.options[ddlEndTime.selectedIndex].text, ddleditLP.options[ddleditLP.selectedIndex].value, 'Day', null, OnSuccessUpdate, OnFailure);
                    }
                }
            }
        }
        function OnSuccessUpdate(result) {
            if (result > 0) {
                //alert('Event ' + childDivTag.id + ' successfully updated');
                alert('Successfully Updated');
                var ddlEndTime = document.getElementById('<%=ddlEndTime.ClientID %>');
                var timeslot = difference(document.getElementById('<%=lblTime.ClientID %>').innerHTML, ddlEndTime.options[ddlEndTime.selectedIndex].text);
                var diffHr = Math.floor(timeslot / 60);
                var diffMin = (timeslot - (diffHr * 60));
                var result = diffHr + "hr " + diffMin + "min";
                var cellHeight = parseInt(timeslot) + parseInt(Math.floor(timeslot / 30)) + parseInt(Math.floor(timeslot / 60));
                childDivTag.style.height = cellHeight + "px";
                var ddlEndTime = document.getElementById('<%=ddlEndTime.ClientID %>');
                var ddleditLP = document.getElementById('<%=ddlEditLP.ClientID%>');
                //childDivTag.innerHTML = "<div style='width:100%;height:10px;background:Black;color:White;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;-ms-text-overflow: ellipsis;'>" + document.getElementById('<%=lblTime.ClientID %>').innerHTML + "-" + ddlEndTime.options[ddlEndTime.selectedIndex].text + "</div>" + ddleditLP.options[ddleditLP.selectedIndex].text;

                if (ddleditLP.options[ddleditLP.selectedIndex].text == "Other") {
                    childDivTag.innerHTML = document.getElementById('<%=ddCustmsgEdit.ClientID %>').value;
                    document.getElementById('<%=ddCustmsgEdit.ClientID %>').value = '';
                }
                else {
                    childDivTag.innerHTML = ddleditLP.options[ddleditLP.selectedIndex].text;
                }

                HideEditPopup();
                formatChilds();
            }
        }

        //function to delete the selected event
        function DeleteEvent() {
            var divID = childDivTag.id;
            var ddlStd = document.getElementById('<%=ddlStudents.ClientID %>');
            //if (ddlStd.disabled == true) {
            //    PageMethods.DeleteEvent(childDivTag.id, parntDivTag.parentNode.id, OnSuccessDelete, OnFailure);
            //}
            //else {
            //    PageMethods.DeleteEvent(childDivTag.id, ddlStd.options[ddlStd.selectedIndex].value, OnSuccessDelete, OnFailure);
            //}

            var hfmode = document.getElementById('<%=hfMode.ClientID%>');
            if (hfmode.value == 'Week') {
                PageMethods.DeleteEvent(childDivTag.id, ddlStd.options[ddlStd.selectedIndex].value, 'Week', OnSuccessDelete, OnFailure);
            }
            if (hfmode.value == 'Day') {
                PageMethods.DeleteEvent(childDivTag.id, parntDivTag.parentNode.id, 'Day', OnSuccessDelete, OnFailure);
            }
        }
        //function which calls when the event is successfully deleted
        function OnSuccessDelete(result) {
            if (result > 0) {
                //alert('Event ' + childDivTag.id + ' successfully Removed');
                //alert('Successfully Removed');
                parntDivTag.removeChild(childDivTag);
                HideEditPopup();
                formatChilds();
            }
        }
        //function to hide the create Popup window
        function HidePopup() {
            $("#pop").hide();
        }
        //functiom to hide the edit popup window
        function HideEditPopup() {
            $("#editPop").hide();
        }
        var bgColor;
        function mouseOver(div) {
            div.style.cursor = "pointer";
            bgColor = div.style.background;
            div.style.background = '#F8F1C6';
        }
        function mouseOut(div) {
            div.style.cursor = "default";
            div.style.background = bgColor;
        }
        var divZindex = null;
        function mouseOvrEvent(divEvent, eventHandle) {
            if (!eventHandle) var eventHandle = window.event;
            if (eventHandle) eventHandle.cancelBubble = true;
            if (eventHandle.stopPropagation) eventHandle.stopPropagation();
            divEvent.style.opacity = 1;
            divZindex = divEvent.style.zIndex;
            divEvent.style.zIndex = 999;
        }
        function mouseOutEvent(divEvent, eventHandle) {
            if (!eventHandle) var eventHandle = window.event;
            if (eventHandle) eventHandle.cancelBubble = true;
            if (eventHandle.stopPropagation) eventHandle.stopPropagation();
            divEvent.style.opacity = 0.6;
            divEvent.style.zIndex = divZindex;
        }
        //function to create an Event
        function CreateTimeSlot() {
            var from = document.getElementById('<%=lblFrom.ClientID %>');
            var ddl = document.getElementById('<%=ddlTime.ClientID %>');
            var to = ddl.options[ddl.selectedIndex].text;
            var height = parntDivTag.style.height;
            var timeslot = difference(from.innerHTML, to);
            var diffHr = Math.floor(timeslot / 60);
            var diffMin = (timeslot - (diffHr * 60));
            var result = diffHr + "hr " + diffMin + "min";
            var cellHeight = parseInt(timeslot) + parseInt(Math.floor(timeslot / 30)) + parseInt(Math.floor(timeslot / 60));

            var divEventID = parseInt(parntDivTag.id) + parseInt(parntDivTag.childNodes.length + 1);
            var zindex = parseInt(parntDivTag.childNodes.length) + 1;
            var color = get_random_color();//#E4EFF8
            var ddlLP = document.getElementById('<%=ddlLP.ClientID %>');
            if (ddlLP.selectedIndex == 0) {
                alert("Please select a LessonPlan");
            }
            else if (ddlLP.selectedIndex == -1) {
                alert("Please select a LessonPlan");
            }
            else {
                //parntDivTag.innerHTML = parntDivTag.innerHTML + "<div id='" + divEventID + "' style='width: 75%; overflow: hidden;text-overflow: ellipsis;-ms-text-overflow: ellipsis;z-index:" + zindex + "; height: " + cellHeight + "px;position:absolute;font-size:xx-small;text-align:left; border: 1px solid #9FC6E7;background: " + color + ";opacity:0.6; cursor:pointer;' onclick=editEvent(this,event); onmouseover=mouseOvrEvent(this,event); onmouseout=mouseOutEvent(this,event);><div style='width:100%;height:10px;background:Black;color:White;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;-ms-text-overflow: ellipsis;'>" + from.innerHTML + "-" + to + "</div>" + ddlLP.options[ddlLP.selectedIndex].text + "</div>";
                parntDivTag.innerHTML = parntDivTag.innerHTML + "<div id='" + divEventID + "' title='" + ddlLP.options[ddlLP.selectedIndex].text + "' style='width: 75%; overflow: hidden;z-index:" + zindex + "; height: " + cellHeight + "px;position:absolute;font-size:11px;font-weight:bold;word-wrap:break-word;text-align:left; border: 1px solid #9FC6E7;background: " + color + ";opacity:0.6; cursor:pointer;' onclick=editEvent(this,event); onmouseover=mouseOvrEvent(this,event); onmouseout=mouseOutEvent(this,event);>" + ddlLP.options[ddlLP.selectedIndex].text + "</div>";
                formatChilds();
                //parntDivTag.onclick = null;
                //parntDivTag.onmouseover = null;
                HidePopup();
                //var hfmode = document.getElementById('<%=hfMode.ClientID%>');

                var ddlStd = document.getElementById('<%=ddlStudents.ClientID %>');

                //if (ddlStd.disabled == true) {
                //    PageMethods.SaveEvent(from.innerHTML, to, parntDivTag.parentNode.id, ddlLP.options[ddlLP.selectedIndex].value, 'Day', null, OnSuccess, OnFailure);
                //}
                //else {
                //    PageMethods.SaveEvent(from.innerHTML, to, ddlStd.options[ddlStd.selectedIndex].value, ddlLP.options[ddlLP.selectedIndex].value, 'Week', parntDivTag.parentNode.id, OnSuccess, OnFailure);
                //}
          
                var hfmode = document.getElementById('<%=hfMode.ClientID%>');
                var othdesc = document.getElementById('<%=ddCustmMsg.ClientID %>').value;

                if (hfmode.value == 'Week') {
                    if ((ddlLP.value == 00000) && (ddlLP.options[ddlLP.selectedIndex].innerHTML == "Other") && othdesc != "") {
                        PageMethods.SaveEvent(from.innerHTML, to, ddlStd.options[ddlStd.selectedIndex].value, ddlLP.options[ddlLP.selectedIndex].value, 'Week', parntDivTag.parentNode.id, othdesc, OnSuccess, OnFailure);
                    }
                    else {
                        PageMethods.SaveEvent(from.innerHTML, to, ddlStd.options[ddlStd.selectedIndex].value, ddlLP.options[ddlLP.selectedIndex].value, 'Week', parntDivTag.parentNode.id, null, OnSuccess, OnFailure);
                    }
                }
                if (hfmode.value == 'Day') {
                    if ((ddlLP.value == 00000) && (ddlLP.options[ddlLP.selectedIndex].innerHTML == "Other") && othdesc != "") {
                        PageMethods.SaveEvent(from.innerHTML, to, parntDivTag.parentNode.id, ddlLP.options[ddlLP.selectedIndex].value, 'Day', null, othdesc, OnSuccess, OnFailure);
                    }
                    else {
                        PageMethods.SaveEvent(from.innerHTML, to, parntDivTag.parentNode.id, ddlLP.options[ddlLP.selectedIndex].value, 'Day', null, null, OnSuccess, OnFailure);
                    }
                }
            }
    }
    //function which calls when an event is created
    function OnSuccess(result) {
        if (result > 0) {
            var divEvent = parntDivTag.lastChild;
            if (divEvent.innerHTML == "Other") {
                divEvent.id = result;
                divEvent.innerHTML = document.getElementById('<%=ddCustmMsg.ClientID %>').value;
                childDivTag = divEvent;
            }
            else {
                divEvent.id = result;
                childDivTag = divEvent;
            }
            //alert('Event Added ' + divEvent.id);
        }
    }
    //function which calls when an event creation or deletion is failed...
    function OnFailure(error) {
        if (error) {
            alert("Error!!!!!!!");
        }
    }
    //function which finds the corresponding div height for the time duration....
    function difference(from, to) {
        var stime = new Array();
        stime = from.split(":");
        var etime = new Array();
        etime = to.split(":");
        var shour = stime[0];
        var smin = stime[1];
        var ehour = etime[0];
        var emin = etime[1];

        var strtTime = parseInt(shour * 60) + parseInt(smin);
        var endTime = parseInt(ehour * 60) + parseInt(emin);
        var addon = (ehour - shour) * 2;
        var diff = endTime - strtTime;
        return diff;

    }
    //function which aligns the events based on its number in its parent div...
    function formatChilds() {
        var xPos = parntDivTag.offsetParent.offsetLeft;
        var childs = parntDivTag.childNodes;

        var leftalign = 0;
        for (var count = 0; count < childs.length; count++) {
            var partitn = Math.floor(75 / childs.length);
            childs[count].style.left = (partitn * count) + "%";
            //leftalign = parseInt(leftalign) + 10;
            childs[count].style.width = partitn + "%";
        }
    }
    //function which generates random color code for the newly created event divs...
    function get_random_color() {
        var colorcodes = new Array('F8D8D8', 'F2CDEE', 'EACDF2', 'DCCDF2', 'CDD7F2', 'CDEEF2', '7AEAB6', 'B5EAAF', 'D0EAAF', 'F7F72F', 'F7C22F', 'FADABA', 'C1E1E3');
        var color = '#' + colorcodes[Math.floor(Math.random() * colorcodes.length)];
        //var letters = '0123456789ABCDEF'.split('');
        //var color = '#';
        //for (var i = 0; i < 6; i++) {
        //    color += letters[Math.round(Math.random() * 15)];
        //}
        return color;
    }

    $(function () {
        $("#ddlLP").change(function () {
            //alert($('option:selected', this).text());
            if ($('option:selected', this).text() == 'Other') {
                var result_style = document.getElementById('cusTxt').style;
                result_style.display = 'table-row';
            }

            if ($('option:selected', this).text() != 'Other') {
                var result_style = document.getElementById('cusTxt').style;
                result_style.display = 'none';
            }
        });
    });

    $(function () {
        $("#ddlEditLP").change(function () {
            //alert($('option:selected', this).text());
            if ($('option:selected', this).text() == 'Other') {
                var result_style = document.getElementById('cusTxt1').style;
                result_style.display = 'table-row';
            }

            if ($('option:selected', this).text() != 'Other') {
                var result_style = document.getElementById('cusTxt1').style;
                result_style.display = 'none';
            }
        });
    });

    </script>

</head>

<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidRes_Value" runat="server" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 25%;" align="left">
                            <asp:Button ID="btnReplicate" runat="server" Text="Copy To Current Week" CssClass="NFButtonWithNoImage" Width="235px" OnClick="btnReplicate_Click" /></td>
                        <td style="width: 25%;" align="right">
                            <asp:Label ID="lblDate" runat="server" Font-Bold="True"></asp:Label>
                            <asp:HiddenField ID="hfMode" runat="server" />
                            <asp:HiddenField ID="hfDate" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <div id="resident" visible="false" runat="server">

                                <asp:Button ID="btn_day" runat="server" Text="Day" CssClass="NFButtonWithNoImage" OnClick="btn_day_Click" Width="80px" />
                                <asp:Button ID="btnResident" runat="server" Text="Resident" CssClass="NFButtonWithNoImage" OnClick="btnResident_Click" Width="80px" />

                            </div>

                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                            <div id="cal_mode" style="display: none;">
                                <!-- <fieldset style="width: 210px;">
                                <legend>Mode</legend>-->
                                <asp:Button ID="btnDay" runat="server" Text="Day" CssClass="NFButtonWithNoImage" OnClick="btnDay_Click" Width="80px" />
                                <input type="button" id="btnWeek" runat="server" value="Week" onclick="javascript: ShowDialog(true);" class="NFButtonWithNoImage" style="width: 80px" />

                                <!--</fieldset>-->
                            </div>
                            <iframe name="print_frame" width="0" height="0" frameborder="0" src="about:blank"></iframe> 
                                <br />
                            <a href="#" style="float:right; padding-right:20px; padding-top:10px; font-weight:bold; font-size:13px" onclick="javascript: printDiv();">Print</a>
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="divRightPanel">

            <asp:HiddenField ID="hfCellID" runat="server" />

            <table width="100%">
                <tr>
                    <td colspan="2" align="right"></td>
                </tr>

                <tr>
                    <td colspan="2">

                        <div id="Calndr" runat="server" class="divLeftPanel">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%">


                                        <tr>
                                            <td align="center" style="width: 15%; vertical-align: top;">
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right" style="width: 25%; font-size: small;">Class :</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlClass" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="width: 25%; font-size: small;">Student :</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlStudents" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged"></asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr style="height: 170px; overflow: auto;">
                                                            <td align="right" style="width: 25%; vertical-align: middle; font-size: small;">Students :</td>
                                                            <td style="vertical-align: middle;">
                                                                <div style="height: 160px; overflow-y: scroll;">
                                                                    <table style="height: 100%;">
                                                                        <tr style="height: 100%; overflow: auto;">
                                                                            <td style="vertical-align: middle;">
                                                                                <asp:CheckBoxList ID="chkStudnts" runat="server"></asp:CheckBoxList></td>
                                                                        </tr>

                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="right">
                                                                <asp:Button ID="btnAddStud" runat="server" Text="Filter" OnClick="btnAddStud_Click" Width="80px" /></td>
                                                        </tr>
                                                    </table>

                                                    <hr />

                                                    <asp:Calendar ID="Calendar1" runat="server" Width="250px" Height="190px"
                                                        FirstDayOfWeek="Monday"
                                                        NextMonthText="&gt;&gt;"
                                                        PrevMonthText="&lt;&lt;"
                                                        DayStyle-BackColor="White"
                                                        DayStyle-ForeColor="Black"
                                                        DayStyle-Font-Names="Arial" OnSelectionChanged="Calendar1_SelectionChanged" OnDayRender="Calendar1_DayRender" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" NextPrevFormat="FullMonth">
                                                        <DayHeaderStyle
                                                            Font-Names="Arial Black" Font-Bold="True" Font-Size="8pt" />
                                                        <SelectedDayStyle
                                                            BackColor="#333399"
                                                            Font-Names="Arial"
                                                            ForeColor="White" />
                                                        <WeekendDayStyle
                                                            Font-Names="Arial" />
                                                        <OtherMonthDayStyle
                                                            Font-Names="Arial"
                                                            ForeColor="#999999" />
                                                        <TodayDayStyle
                                                            Font-Names="Arial" BackColor="#CCCCCC" />

                                                        <DayStyle Font-Names="Arial"></DayStyle>
                                                        <WeekendDayStyle ForeColor="Red" />
                                                        <NextPrevStyle
                                                            Font-Names="Arial"
                                                            ForeColor="#333333" Font-Bold="True" Font-Size="8pt" VerticalAlign="Bottom" />
                                                        <TitleStyle
                                                            BackColor="#ECE9D8"
                                                            Font-Names="Arial Black"
                                                            ForeColor="#000000"
                                                            HorizontalAlign="Center" BorderColor="Black" BorderWidth="1 px" Font-Bold="True" Font-Size="10pt" />
                                                    </asp:Calendar>
                                                    <hr />
                                                </div>
                                            </td>
                                            <td align="center" style="vertical-align: top;"></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div style="width: 99.5%; border: 1px solid black;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <table id="tblCalndr" runat="server" width="100%">
                                        <tr>
                                            <td align="center" style="width: 5%; border: 1px solid lightgrey; background-color: #ECE9D8;">
                                                <div id="view" onclick="view(this);" class="divMenu">
                                                    <img id="imgArrow" src="../Administration/images/calendar.gif" height="20px" width="20px" />
                                                    <img id="imgArrow1" src="../Administration/images/downarrow.png" height="20px" width="20px" />
                                                </div>

                                            </td>
                                        </tr>

                                        <tr>

                                            <td id="tdTime" runat="server" style="width: 50px; position: relative; left: 2px;"></td>

                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>


        </div>

        <div id="pop" class="web_dialog2">
            <table width="100%">
                <tr>
                    <td align="right">
                        <img src="../Administration/images/clb.PNG" style="border: 0px; margin-top: -23px; margin-right: -24px; float: right; padding: 5px" alt="" height="18px" width="18px" onclick="javascript:HidePopup();" />
                    </td>
                </tr>
                <tr id="addEvent" runat="server">
                    <td>
                        <table style="width: 100%; height: 170px">
                            <tr>
                                <td colspan="2" align="center">Add Event<br />
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 25%;" class="tdText">Start Time </td>
                                <td style="width: auto;" align="left">
                                    <asp:Label ID="lblFrom" runat="server" Text="Label" CssClass="tdText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdText">End Time </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTime" runat="server" CssClass="drpClass" Width="200px"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="tdText">LessonPlan </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlLP" runat="server" CssClass="drpClass" Width="200px"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="cusTxt" style="display: none;">
                                <td class="tdText">Other</td>
                                <td align="left">
                                    <asp:TextBox ID="ddCustmMsg" runat="server" CssClass="tdText" Text="" placeholder="" style="width:200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <br />
                                    <input id="btnSubmit" type="button" value="Create" onclick="javascript: CreateTimeSlot();" class="NFButtonWithNoImage" width="80px" style="margin-right: 8px" />

                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>

            </table>
        </div>
        <div id="editPop" class="web_dialog2">
            <table style="width: 100%; height: 170px">
                <tr>
                    <td align="right">
                        <img src="../Administration/images/clb.PNG" style="border: 0px; margin-top: -23px; margin-right: -24px; float: right; padding: 5px" alt="" height="18px" width="18px" onclick="javascript:HideEditPopup();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td colspan="2" align="center">Edit Event<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;" class="tdText">Start Time </td>
                                <td style="width: auto;" align="left">
                                    <asp:Label ID="lblTime" runat="server" Text="Label" CssClass="tdText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 25%;" class="tdText">End Time </td>
                                <td style="width: auto;" align="left">
                                    <asp:DropDownList ID="ddlEndTime" runat="server" CssClass="drpClass" Width="200px"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 25%;" class="tdText">LessonPlan </td>
                                <td style="width: auto;" align="left">
                                    <asp:DropDownList ID="ddlEditLP" runat="server" CssClass="drpClass" Width="200px"></asp:DropDownList></td>
                            </tr>
                            <tr id="cusTxt1" style="display: none;">
                                <td class="tdText">Other</td>
                                <td align="left">
                                    <asp:TextBox ID="ddCustmsgEdit" runat="server" CssClass="tdText" Text="" placeholder="" style="width:200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <br />
                                    <input id="btnUpdate" type="button" value="Update" onclick="javascript: UpdateEvent();" class="NFButtonWithNoImage" width="80px" />
                                    <input id="btnDelete" type="button" value="Delete" onclick="javascript: DeleteEvent();" class="NFButtonWithNoImage" width="80px" style="margin-right: 8px" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="overlay" class="web_dialog_overlay">
        </div>
        <div id="PopUpStud" class="web_dialog">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table width="100%" style="height: 160px;">
                        <tr>
                            <td colspan="2" align="right">
                                <img src="../Administration/images/clb.PNG" height="18px" width="18px" onclick="HideDialog();" style="border: 0px; margin-top: -23px; margin-right: -24px; float: right; padding: 5px" width="25" alt="" /></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">Select a Student</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;" class="tdText">Select Class </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPopClass" CssClass="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPopClass_SelectedIndexChanged" Width="200px"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;" class="tdText">Select Student </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPopStdnts" runat="server" CssClass="drpClass" Width="200px"></asp:DropDownList></td>
                        </tr>
                        <tr style="vertical-align: bottom;">
                            <td colspan="2" align="right">
                                <asp:Button ID="btn_Week" runat="server" Text="OK" CssClass="NFButtonWithNoImage" OnClick="btnWeek_Click" Width="80px" Style="margin-right: 22px" />
                            </td>

                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </form>
</body>

