<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AsmntReview.aspx.cs" Inherits="StudentBinder_AsmntReview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js_asmnt/jquery-1.8.3.js"></script>
    <script src="js_asmnt/jq1.js"></script>

    <link href="CSS/AsmntReviewHome.css" rel="stylesheet" id="sized" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

    <style>
        .my_repeater_cssclass br {
            display: none;
        }

            .my_repeater_cssclass br.hav {
                display: block;
                clear: both;
            }

        /*Coppy Template option*/
        .warning_box {
            width: 115%;
            clear: both;
            background: url(../Administration/images/warning.png) no-repeat left #fcfae9;
            border: 1px #e9e6c7 solid;
            background-position: 10px 1px;
            padding-left: 10px;
            padding-top: 10px;
            padding-bottom: 5px;
            text-align: center;
            color: Red;
        }

        #HdrExportTemplate {
            display: none;
        }

        .BtnCreate{
            background-color: #03507D;
            width: 91px;
            height: 26px;
            color: #fff;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            background-position: 0 0;
            border: none;
            cursor: pointer;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
        }
        /*end*/

        #LpContainerDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 88%;
            min-height: 500px;
            height: auto !important;
        }


        iframe {
            border: medium none;
            min-height: 462px;
            height: auto;
            /*overflow: scroll;*/
            width: 100%;
        }



        /*-----------------------------------------------------------JQuery End-------------------------------------------*/
    </style>

    <script type="text/javascript">
       
        function adjustStyle(width) {
            width = parseInt(width);
            if (width >= 988) {
                $("#sized").attr("href", "CSS/AsmntReviewHome.css");
                return;
            }

            if (width < 988) {
                $("#sized").attr("href", "CSS/AsmntReviewTab.css");
                return;

            }
        }

        function disableButton() {
            $("#btnAddLP").prop("disabled", true);
            setTimeout(function () {
                $("#btnAddLP").prop("disabled", false);
            }, 3000);
        }


        $(function () {
            adjustStyle($(this).width());
            $(window).resize(function () {
                adjustStyle($(this).width());
            });
        });

        var temp = '<div id="LP_Loading" class="ingrContainer" style="opacity:0.4;">' +
                        '<span></span>' +
                        '<img width="100px" height="12px" src="images/load.gif" style="padding: 28px 41% 0;"><br clear="all" /><p></p></div>';
        //function to add stdtLPs.....
        function AssignGoalAndLPs(LP, LPname, goalid, goalname, LPdesc) {
            goalid = 'a_' + goalid;
            AssignGoal(goalid, goalname, null);
            var ind = 0;
            goalid = goalid.split("_")[1];
            var golid = '#li_' + goalid;
            var id; var chkIEP; var chkStat; var chkday; var chkdaystring;
            window.setTimeout(function () {     //delay 4 secnds 
                PageMethods.SaveLessons(LP, goalid, function (data) {
                    // alert("yes");
                    $('#ul_selGoals').find(golid + ' .wrapper #LP_Loading').remove();   //removes the loading div....
                    if (data == 'exists') {
                        ind = 1;
                    }
                    else if (data == "0") { ind = 1; }
                    else {
                        var datas = data.split('*');
                        id = datas[0];
                        chkIEP = datas[1];
                        chkday = datas[2];
                        if (chkIEP == "0") { chkStat = ""; }
                        else { chkStat = "checked=true"; }
                        if (chkday == "1") {
                            chkdaystring = "<input name='Day' class='rdoDay' checked='true' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + LP + ",&apos;LP&apos;,event);' />Day</span>"
                                + "<span style='float:left;'><img width='15px' height='7px' style='position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;' src='images/load.gif'>"
                                + "<input name='Resi' class='rdoResi' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + LP + ",&apos;LP&apos;,event);' />Residential</span>";

                        }
                        if (chkday == "0") {
                            chkdaystring = "<input name='Day' class='rdoDay'  type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + LP + ",&apos;LP&apos;,event);' />Day</span>"
                            +"<span style='float:left;'><img width='15px' height='7px' style='position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;' src='images/load.gif'>"
                            + "<input name='Resi' class='rdoResi' checked='true' type='checkbox' value='' onclick='updateLessonPlan(this,this.parentNode.parentNode.id," + LP + ",&apos;LP&apos;,event);' />Residential</span>";

                        }
                    }

                    if (ind == 0) {
                        var temp = 'div_' + LP;
                        var LPs = '<div id=' + temp + ' class="ingrContainer">' +
                            '<h3>' + LPname + '</h3><div id="' + id + '" class="container">' +
                            '<span style="float:left;"><img width="15px" height="7px" style="position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;" src="images/load.gif">'+chkdaystring+
                            '<span><img width="15px" height="7px" style="position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;" src="images/load.gif">' +
                            '<input name="" class="rdo" ' + chkStat + ' type="checkbox" value="" onclick="updateIEP(this,this.parentNode.parentNode.id,' + goalid + ',&apos;LP&apos;,event);" />Included in IEP</span>' +
                            
                          //  '<a href="#" class="dlt" onclick="deleteLPandGoal(this,this.parentNode.id,&apos;LP&apos;,event);"></a></div><br clear="all" />' +
                            '<p>' + LPdesc + ' </p></div>';
                        var li3 = $('#ul_selGoals').find(golid);
                        $(li3).find('.wrapper').append(LPs);
                        
                    }
                    if (data == 'exists') {
                        $('#HdrExportTemplate').show();    // to show the warning popup 
                        $("#divWarning").show();
                        $("#divMessage").hide();
                        $("#divCopyTempAdmin").hide();
                        $("#hdLessonId").val(LP);
                        $("#hdGoalId").val(goalid);
                    }
                    
                    __doPostBack('lbLPBank', '');
                });
               
                
                
            }, 4000);
            

        }

        function showdivCopyTempAdmin() {
            $("#divWarning").hide();
            $("#divMessage").hide();
            $("#divCopyTempAdmin").show();
        }

        function showdivMessage() {
            $("#HdrExportTemplate").show();
            $("#divWarning").hide();
            $("#divCopyTempAdmin").hide();
            $("#divMessage").show();
        }
        function closeDiv() {
            $('#HdrExportTemplate').fadeOut('slow');
        }

        function ExecuteLPExist() {
            if (LPExist() == true) {
                var Name = $("#txtCopyLP").val();
                $("#hdLessonName").val(Name);
                return true;
            }
            else {
                $("#hdLessonName").val("");
                return false;
            }
        }

        function LPExist() {
            var Name = $("#txtCopyLP").val();
            var clrName = Name.replace("'", "\\\'");
            if (Name == "") {
                $("#tdMsgExprt").html("<div class='warning_box' style='width: 275px'>Please enter lesson plan name.</div>");
                return false;
            }
            else {
                var dataresult = false;
                $.ajax(
                 {
                     type: "POST",
                     url: "AsmntReview.aspx/SearchLessonPlanList",
                     //data: "{'Name':'" + Name + "'}",
                     data: "{'Name':'" + clrName + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         if (data.d == "0") {
                             dataresult = true;
                         }
                         else {
                             $("#tdMsgExprt").html("<div class='warning_box'>  Lesson plan name already exist. Please enter another name...</div>");
                             dataresult = false;
                         }
                     },
                     error: function (request, status, error) {
                         alert("Error");
                     }
                 });
                return dataresult;
            }
        }

        //function to add stdtgoal .....
        var flag = true;
        function AssignGoal(Goalid, Goalname, eventHandle) {
            GoalTitle = $('#' + Goalid).attr('title');
            if (eventHandle != null) {
                if (!eventHandle) var eventHandle = window.event;                   //
                if (eventHandle) eventHandle.cancelBubble = true;                   // Code to stop event propogation......
                if (eventHandle.stopPropagation) eventHandle.stopPropagation();     //
                flag = false;
            } else {
                flag = true;
            }
            Goalid = Goalid.split("_")[1];
            $('#ul_selGoals').append('<li id="loading" style="position: static;opacity:0.5;" class="accordion">' +
				'<h2 class=""><span class="dd"></span><img width="100px" height="12px" src="images/load.gif" style="padding: 10px 0px 0px 40%;">' +
                '<div class="container"></div></h2></li>');

            PageMethods.SaveGoal(Goalid, flag, function (data) {
                var ind = 0;
                Goalid = 'li_' + Goalid;
                var d = data;
                var iepid;
                if (data == "0") { ind = 1; }
                else if (data != "0") {
                    data = d.split('*')[0];
                    iepid = d.split('*')[1];
                }
                if (data == 'exists') {
                    ind = 1;
                    var li = $('#ul_selGoals').find('#' + Goalid);
                    $('#ul_selGoals #loading').remove();

                    if (iepid != "0") {
                        var chk = $(li).find('.rdo')[0];
                        $(chk).attr('checked', true);
                    }
                }
                else {
                    var Glid = data;
                    var chkGlIEP = iepid;
                    if (flag == false) {
                        if (chkGlIEP == "0") { flag = false; }
                        else { flag = true; }
                    }
                    else {
                        if (chkGlIEP == "0") { flag = false; }
                    }
                    data = Glid;
                }
                if (ind == 0) {

                    var li = document.createElement('li');
                    li.setAttribute('id', Goalid);
                    li.setAttribute('class', 'accordion');
                    li.setAttribute('runat', 'server');

                    var htag = document.createElement('h2');
                    htag.setAttribute('class', '');
                    htag.setAttribute('onclick', 'h2click(this);');


                    var spn = document.createElement('span');
                    spn.setAttribute('class', 'dd');

                    htag.appendChild(spn);

                    var atag = document.createElement('a');
                    atag.setAttribute('href', '#');
                    atag.innerHTML = Goalname;
                    atag.title = GoalTitle;

                    htag.appendChild(atag);

                    var divCon = document.createElement('div');
                    divCon.setAttribute('class', 'container');
                    divCon.setAttribute('id', data);



                    var spn2 = document.createElement('span');


                    var imgload = document.createElement('img');
                    imgload.setAttribute('width', '15px');
                    imgload.setAttribute('height', '7px');
                    imgload.src = 'images/load.gif';
                    imgload.setAttribute('style', 'position: absolute; float: left ! important; padding-right: 0px; margin: 4px -15px 0px; display: none;');


                    var chk = document.createElement('input');
                    chk.setAttribute('type', 'checkbox');
                    chk.setAttribute('class', 'rdo');
                    chk.setAttribute('name', '');
                    chk.setAttribute('value', '');
                    if (flag) {
                        chk.setAttribute('checked', true);
                    }
                    chk.setAttribute('onclick', 'updateIEP(this,this.parentNode.parentNode.id,0,"Goal",event);');

                    spn2.appendChild(imgload);
                    spn2.appendChild(chk);
                    spn2.innerHTML += 'Included in IEP';


                    var atag2 = document.createElement('a');
                    atag2.setAttribute('href', '#');
                    atag2.setAttribute('class', 'dlt');
                    atag2.setAttribute('onclick', 'deleteLPandGoal(this.parentNode,this.parentNode.id,"Goal",event);');

                    divCon.appendChild(spn2);
                    divCon.appendChild(atag2);
                    htag.appendChild(divCon);

                    var divwrap = document.createElement('div');
                    divwrap.setAttribute('class', 'wrapper');
                    $(divwrap).css('display', 'none');
                    $(divwrap).css('background-color', '#E1E1E1');
                    $(divwrap).css('border-radius', '0 0 7px 7px');

                    li.appendChild(htag);
                    li.appendChild(divwrap);


                    $('#ul_selGoals #loading').remove();
                    $('#ul_selGoals').append(li);

                }
                if (eventHandle == null) {  //checks whether the user click on a Lessonplan or Goal..if Lessonplan the eventHandle is null.....
                    $('#ul_selGoals').find('#' + Goalid + ' .wrapper').append(temp);  //append a loading div ....
                }
                var li2 = document.getElementById(Goalid);
                var disp = li2.getElementsByClassName('wrapper')[0].style.display;
                if (disp == 'none') {
                    li2.getElementsByTagName('h2')[0].click();
                }
            }, function (error) { });
        }
        //function for expanding and collapsing tabs....
        function h2click(h2) {
            // if section is already open, return false
            if ($(h2).is('.active')) {
                $(h2).next('div').slideUp('fast');
                $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
                return false;
            }

            // open request and close open
            $(h2).parent().parent().find('.accordion > div').slideUp('fast');
            $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
            $(h2).addClass("active");
            $(h2).next('div').slideToggle('fast');

            // fix IE 6 bug.
            if (jQuery.browser.msie && jQuery.browser.version < 7) {
                $('.accordion div').addClass('iefix');
            }

            return false;
        }


        function dispLoad() {
            document.getElementById('cont_load').style.display = "block";
            $('#cont_load').parent().css('opacity', '0.4');
        }
        //function to delete stdtLPs and stdtgoals
        function deleteLPandGoal(elmnt, id, type, eventHandle) {
            if (eventHandle != null) {
                if (!eventHandle) var eventHandle = window.event;                   //
                if (eventHandle) eventHandle.cancelBubble = true;                   // Code to stop event propogation......
                if (eventHandle.stopPropagation) eventHandle.stopPropagation();     //
            }
            if (type == 'Goal') {
                var elem = $(elmnt).parent().parent().find('.wrapper');
                if ($(elem).children().length == 0) {       //if the stdtgoal does not have any stdtLPs, then delete possible....
                    if (confirm('Are you sure you want to Delete?')) {
                        PageMethods.DeleteLPandGoals(id, type, function (data) {
                            if (data != '1') {
                                alert(data);
                            }
                            else
                                $(elmnt).parent().parent().remove();
                            document.location.reload(true);
                        });
                    }
                }
                else {              //if the stdtgoal has one or more stdtLPs, then stdtgoal deletion not possible.....
                    alert('Delete not possible');
                }
            }
            if (type == 'LP') {
                if (confirm('Are you sure you want to Delete?')) {
                    PageMethods.DeleteLPandGoals(id, type, function (data) {
                        if (data != '1') {
                            alert(data);
                        }
                        else
                            $(elmnt).parent().parent().remove();
                        document.location.reload(true);
                    });
                }
            }


        }
        //function to update the IEP status of the stdtgoal or stdtLessonplan.......
        function updateIEP(chkbx, id, index, type, eventHandle) {
            chkbx.previousSibling.style.display = 'block';  //makes the loading image display to block.....
            chkbx.disabled = true;                          //disable the checkbox
            if (eventHandle != null) {
                if (!eventHandle) var eventHandle = window.event;                   //
                if (eventHandle) eventHandle.cancelBubble = true;                   // Code to stop event propogation......
                if (eventHandle.stopPropagation) eventHandle.stopPropagation();     //
            }
            var check;
            var chk;
            if (chkbx.checked == true) {
                check = '0';

                if (type == "LP") {
                    if (index != 0) {
                        var LPGoalid = 'li_' + index;
                        var li = $('#ul_selGoals').find('#' + LPGoalid);

                        chk = $(li).find('.rdo')[0];
                        $(chk).attr('checked', true);
                    }
                }
            }
            else
                check = '1';
            PageMethods.UpdateIEPStat(id, type, check, function (data) {
                if (data != 'success') {
                    if (data != "")
                        alert(data);
                    if (chkbx.checked == false)
                        chkbx.checked = true;
                    else {
                        chkbx.checked = false;
                        if (type == "LP") {
                            if (index != 0) {
                                $(chk).attr('checked', false);
                            }
                        }
                    }


                }
                //if (data == 'There is not any IEP in Progress. Updation not Possible') chkbx.checked = false;
                chkbx.previousSibling.style.display = 'none';   //hide loading image .....
                chkbx.disabled = false;                         //disable the checkbox
            }, function (error) {
                alert("Updation Failed !!!");
                chkbx.previousSibling.style.display = 'none';   //hide loading image .....
                chkbx.disabled = false;                         //disable the checkbox

            });
        }

        //function to update the IEP status of the stdtgoal or stdtLessonplan.......
        function updateLessonPlan(chkbx, id, index, type, eventHandle) {
            // chkbx.previousSibling.style.display = 'block';  //makes the loading image display to block.....
            chkbx.disabled = true;                          //disable the checkbox
            if (eventHandle != null) {
                if (!eventHandle) var eventHandle = window.event;                   //
                if (eventHandle) eventHandle.cancelBubble = true;                   // Code to stop event propogation......
                if (eventHandle.stopPropagation) eventHandle.stopPropagation();     //
            }
            var check;
            var chkDay;
            var chkResi;
            var name = chkbx.name;
            if (chkbx.checked == true) {
                check = '1';

                if (type == "LP") {
                    if (index != 0) {
                        var LPGoalid = 'li_' + index;
                        var li = $('#ul_selGoals').find('#' + LPGoalid);
                        if (name == "Day") {
                            chkDay = $(li).find('.rdoDay')[0];
                            $(chkDay).attr('checked', true);
                        }
                        if (name == "Resi") {
                            chkResi = $(li).find('.rdoResi')[0];
                            $(chkResi).attr('checked', true);
                        }
                    }
                }
            }
            else
                check = '0';
            PageMethods.UpdateLessonPlanStat(id, type, name, check, function (data) {
                if (data != 'success') {
                    if (data != "")
                        alert(data);
                    if (chkbx.checked == false)
                        chkbx.checked = true;
                    else {
                        chkbx.checked = false;
                        if (type == "LP") {
                            if (index != 0) {
                                $(chk).attr('checked', false);
                            }
                        }
                    }


                }
                //if (data == 'There is not any IEP in Progress. Updation not Possible') chkbx.checked = false;
                //chkbx.previousSibling.style.display = 'none';   //hide loading image .....
                chkbx.disabled = false;                         //disable the checkbox
            }, function (error) {
                alert("Updation Failed !!!");
                //chkbx.previousSibling.style.display = 'none';   //hide loading image .....
                chkbx.disabled = false;                         //disable the checkbox

            });
        }


        //function to Restrict input to textbox: allowing only numbers...
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function refreshGoal(ref_img) {
            $(ref_img).parent().css('opacity', '0.4');
            window.setTimeout(function () { $(ref_img).parent().css('opacity', '1.0'); }, 4000);
        }

        function LoadLessonView(LpId, goalId) {
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#ifrmeLp').attr('src', 'LessonPlanAttributes.aspx?lessonId=' + LpId + '&goalId=' + goalId + '');
                $('#LpContainerDiv').fadeIn();

            });

        }


        function closelp() {
            $('#LpContainerDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        }

        function reset() {
            var dlAsmnt = document.getElementById('<%=dlAsmnts.ClientID%>');
            if (dlAsmnt != null) {            //Condition added to avoid Js Exception --Biweekly compatibility fix 24-12-2020
                var chks = dlAsmnt.getElementsByClassName('radio');
                for (var i = 0; i < chks.length; i++) {
                    chks[i].checked = false;
                }
            }

            var dlSkill = document.getElementById('<%=dlSkills.ClientID%>');
            if (dlSkill != null) {             //Condition added to avoid Js Exception --Biweekly compatibility fix 24-12-2020
                chks = dlSkill.getElementsByClassName('radio');
                for (var i = 0; i < chks.length; i++) {
                    chks[i].checked = false;
                }
            }
            document.getElementById('<%=minPercnt.ClientID%>').value = '';
            document.getElementById('<%=maxPercnt.ClientID%>').value = '';
        }


    </script>

    <script type="text/javascript">
        function RefreshParent() {
            window.parent.location.href = window.parent.location.href;
        }
    </script>

    <style type="text/css">
        #AddLPDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 725px;
            height: 480px;
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

        .web_dialog2 {
            background: url("../Administration/images/smalllgomlmark.JPG") no-repeat scroll right bottom #f8f7fc;
            border: 5px solid #b2ccca;
            color: #333;
            display: none;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            left: 15%;
            min-height: 325px;
            padding: 5px 5px 30px;
            position: fixed;
            top: 2%;
            width: 900px;
            z-index: 9999;
        }

        }

        #closeLP {
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

        #grdLessonPlanDelete {
            width: 100%;
            margin-top: 20%;
        }

        .auto-style1 {
            height: 30px;
        }
    </style>
    <script type="text/javascript">
        function closePOP(goalId, LessonId) {
            $('#AddLPDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast', function () {
                    if (parseInt(goalId, 10) > 0) {
                        goalId = '#a_' + parseInt(goalId, 10);
                        $('#cont_load').parent().css('opacity', '0.4');
                        document.getElementById('cont_load').style.display = "block";
                        var GoalObj = $('#ul_Goals').parent().parent().parent().find(goalId);
                        ///window.setTimeout(function () { $('#ul_Goals').find(goalId).parent().click(); }, 2000);
                        window.setTimeout(function () { $(GoalObj).parent().click(); }, 2000);
                        LessonId = '#' + LessonId;
                        ///window.setTimeout(function () { $('#ul_Goals').find(goalId).parent().parent().find(LessonId).click(); }, 2000);
                        window.setTimeout(function () { $(GoalObj).parent().parent().find(LessonId).click(); }, 2000);
                        window.setTimeout(function () { LoapLPs(goalId) }, 2000);
                    }
                });
            });

        }
        function LoapLPs(goalId) {
            $('#cont_load').parent().css('opacity', '1.0');
            document.getElementById('cont_load').style.display = "none";
        }

        function ClearLPPop() {
            $('#txtLPname').val('');
            $('#ddlGoals').val('');
            $('#txtFrameworkAndStandard').val('');
            $('#txtSpecStandard').val('');
            $('#txtSpecEntryPoint').val('');
            $('#txtPreskills').val('');
            $('#txtMaterials').val('');
            $('#txtBaseline').val('');
            $('#txtObjective').val('');
        }
        function AddLP() {
            
            ClearLPPop();
            $('#tdMsgForLp').html('');
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#AddLPDiv').fadeIn();
            });
        }

        function DisableEnterKey(e) {
            var key;
            if (window.event)
                key = window.event.keyCode; //IE
            else
                key = e.which; //firefox     

            return (key != 13);
        }

        function listLesson() {


        }

        //function switchSearchMode(chk) {
        //    if (chk.checked == false) {
        //        document.getElementById('btnPercnt').disabled = true;
        //        document.getElementById('btnPercnt').style.cursor = 'default';
        //    } else {
        //        document.getElementById('btnPercnt').disabled = false;
        //        document.getElementById('btnPercnt').style.cursor = 'pointer';
        //    }
        //}
    </script>
</head>
<body>
    <form id="form1" runat="server">
       
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

        <div>
            <div class="boxContainerContainer">
                <!-------------------------Top Container Start----------------------->
                <div class="itlepartContainer">
                    <h2 style="margin: 0px 0px 0px 5px;">1 <span>Choose Source</span></h2>
                    <h2 style="margin: 0px 0px 0px 40px;">2 <span>Choose Goals and Lessons</span></h2>
                    <h2 style="margin: 0px 0px 0px 45px;">3 <span>Assign Goals and Lessons</span></h2>  
                    <div style="float: right; width: 61px;">
                        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="float: left; padding-top: 6px; margin-right: 10px;">
                            <ContentTemplate>
                                <asp:Button ID="btnListDelLessonPlan" Text="Inactive LessonPlans" style="display:none" ToolTip="Inactive LessonPlans" runat="server" CssClass="NFButtonWithNoImage" OnClientClick="popPrompts();" OnClick="btnListDelLessonPlan_Click" />
                                <%--<input type="button" id="btnListDelLessonPlan" value="Inactive LessonPlans" contextmenu="Inactive LessonPlans" class="btn" onclick="listLesson();" />
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <asp:ImageButton ID="btnRefresh" runat="server" text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                    </div>
                </div>
                <!-------------------------Top Container End----------------------->

                <!-------------------------Middle Container start----------------------->
                <div class="btContainerPart">
                    <div id="lBxpart" runat="server" class="lBxpartContainer">
                        <%--<h3>Search</h3>--%>
                        <br clear="all" />
                        <input id="txtSearch" onkeypress="return DisableEnterKey(event);" runat="server" style="font-style: italic; width: 130px;" value="Type search terms here" onblur="if(this.value=='') this.value='Type search terms here'" autocomplete="off" onfocus="if(this.value =='Type search terms here' ) this.value=''" class="txtfld" name="" type="text" />

                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnSearch" ToolTip="Search" runat="server" Style="float: left !important;" CssClass="btn" OnClientClick="dispLoad();" OnClick="btnSearch_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br clear="all" />
                        <br clear="all" />

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="lbLPBank" ToolTip="All LessonPlans" CssClass="grb" runat="server" Font-Bold="true" OnClientClick="reset();dispLoad();" OnClick="lbLPBank_Click">See All Lessons in Bank</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--<br clear="all" />--%>


                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="lbAllAsmnts" ToolTip="All Assessments" runat="server" CssClass="grb" Font-Bold="true" OnClientClick="reset();dispLoad();" OnClick="lbAllAsmnts_Click">All Latest Assessments</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br clear="all" />


                        <h3>Filter LessonPlans</h3>
                        <hr />

                        <h5><i>Check one or more filters and Apply</i></h5>

                        <br clear="all">
                        <div id="page">
                            <div id="content">
                                <ul>
                                    <li style="position: static;" class="accordion">
                                        <h2 class="tBG" onclick="h2click(this);"><a class="jj" href="#our-company">By Latest Assessment(s)</a></h2>
                                        <div style="display: none;" class="wrapper nomar">
                                            <div class="nobdrrcontainer">
                                                <asp:DataList ID="dlAsmnts" runat="server" RepeatLayout="Flow" CssClass="my_repeater_cssclass" OnItemDataBound="dlAsmnts_ItemDataBound">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfStat" runat="server" Value='<%# Eval("Status") %>' />
                                                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("AsmntId") %>' />
                                                        <input id="chkAsmnt" runat="server" name="" class="radio" type="checkbox" value="" />

                                                        <a class="lpb" title='<%#Eval("AsmntName")+" - "+Eval("name")+" - "+Eval("AsmntStartTs") %>' href="#"><%#Eval("AsmntName") %></a>
                                                        <br clear="all" class="hav" />

                                                    </ItemTemplate>
                                                </asp:DataList>
                                                <div class="clear"></div>

                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>

                        <div id="page">
                            <div id="content">
                                <ul>
                                    <li style="position: static;" class="accordion">
                                        <h2 class="tBG" onclick="h2click(this);"><a class="jj" href="#our-company">By Skill(s)</a></h2>
                                        <div style="display: none;" class="wrapper nomar">
                                            <div class="nobdrrcontainer">
                                                <asp:DataList ID="dlSkills" runat="server" RepeatLayout="Flow" CssClass="my_repeater_cssclass" OnItemDataBound="dlAsmnts_ItemDataBound">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("AsmntId") %>' />
                                                        <input id="chkAsmnt" runat="server" name="" class="radio" type="checkbox" value="" />

                                                        <a class="lpb" title='<%#Eval("GoalName")+" - "+Eval("name")+" - "+Eval("AsmntStartTs") %>' href="#"><%#Eval("GoalName") %></a>
                                                        <br clear="all" class="hav" />

                                                    </ItemTemplate>
                                                </asp:DataList>
                                                <div class="clear"></div>

                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSubmit" runat="server" ToolTip="Submit" CssClass="btn" OnClientClick="dispLoad();" OnClick="btnSubmit_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                        <br clear="all" />
                        <%--<h3>By Skill</h3>--%>


                        <input id="chkSearch" runat="server" name="" class="radio" type="checkbox" value="" />
                        <p>
                            Only Show Lessons With a % Skill Acquisition between:
                        </p>

                        <input id="minPercnt" runat="server" class="smltxtfld" onkeypress="return isNumber(event)" onpaste="return false" name="" type="text" />

                        <input id="maxPercnt" runat="server" class="smltxt" onkeypress="return isNumber(event)" onpaste="return false" name="" type="text" />
                        <br clear="all" />
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnPercnt" Style="margin: 15px 0 0 100px;" runat="server" ToolTip="Apply" Text="Apply" CssClass="NFButtonWithNoImage" OnClientClick="dispLoad();" OnClick="btnPercnt_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="RegularExpressionValidator"></asp:RegularExpressionValidator>--%>
                        <%--<asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="RangeValidator"></asp:RangeValidator>--%>

                        <br />
                        <br />
                        <br />
                        <br />
                        <div class="clear"></div>
                    </div>

                    <!------------------------------------MContainer Start----------------------------------->

                    <div id="mBx" runat="server" class="mBxContainer">
                        <h3>Skills Filtered by Source(s) </h3>



                        <%----%>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div id="page">
                                    <div id="content" style="opacity: 1;">
                                        <div id="cont_load" runat="server" style="display: none; margin-top: -25px; position: fixed; width: 17%; height: 530px;">
                                            <img src="images/loading.gif" style="margin: 200px 38% 0;">
                                        </div>
                                        <input name="" type="button" class="NFButtonWithNoImage" style="height: 32px !important; width: 224px !important;" onclick="AddLP();" value="Create new Lesson Plan  and Assign " />
                                        <ul id="ul_Goals" runat="server">
                                        </ul>

                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                    </div>



                    <!------------------------------------MContainer End----------------------------------->

                    <!------------------------------------End Container Start----------------------------------->
                    <div class="righMainContainer">
                        <h3> Assigned Goals and Lessons </h3>
                        <div id="Div7">

                            <div id="Div8">
                                <ul id="ul_selGoals" runat="server">
                                </ul>
                            </div>
                        </div>







                    </div>

                    <!------------------------------------End Container End----------------------------------->


                    <!-------------------------Middle Container End----------------------->


                    <div class="clear"></div>
                </div>

                <div id="HdrExport" style="width: 100%; height: 100%;">
                    <div id="HdrExportTemplate" class="web_dialog" style="top: 7%; height:auto; width:625px">
                        <div id="Hdr_Stat">
                            <a id="closeHdr" onclick="closeDiv();" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <div id="divWarning" runat="server" >
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>                            
                                        <table style="align-content: center; padding-top:25px">
                                            <tr>
                                                <td>
                                                    <div  id="divApprMsg" class='warning_box' >A lesson exists with the same name. Do you want to create the same lesson with a new name?</div>
                                                </td>
                                            </tr>
                                            <tr><td></td></tr>
                                            <tr><td></td></tr>
                                            <tr><td></td></tr>
                                            <tr><td></td></tr>
                                            <tr> 
                                                <td align="right">                                                       
                                                    <input id="BtnCreate" class="BtnCreate" type="submit" name="BtnCreate" value="Create" onclick="showdivCopyTempAdmin();"/>
                                                </td>
                                            </tr>
                                        </table>                            
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div id="divCopyTempAdmin" style="display: none" runat="server" >
                                <table  style="padding-top:30px">
                                        <tr>
                                            <td id="tdMsgExprt"> </td>
                                        </tr>
                                        <tr><td></td></tr>
                                 </table>
                                <br />
                                 <table>
                                    <tbody>
                                        <tr>
                                            <td>Lesson Plan Name</td>
                                            <td><span style="color: red">*</span></td>
                                            <td>
                                                <input type="text" id="txtCopyLP" style="width: 180px;" />
                                            </td>
                                            <td style="width:300px"></td>
                                        </tr>
                                        <tr></tr>
                                        <tr><td>
                                            <asp:Button ID="btnCopyLP" runat="server" CssClass="NFButton" Style="width: 180px" Text="Copy Lesson Plan" OnClick="btnCopyLP_Click" OnClientClick="return ExecuteLPExist(); " />
                                        </td></tr>
                                    </tbody>
                                </table>
                                <asp:HiddenField ID="hdLessonName" runat="server" />
                                <asp:HiddenField ID="hdLessonId" runat="server" />
                                <asp:HiddenField ID="hdGoalId" runat="server" />
                            </div>     
                            
                            <div id="divMessage" style="display: none" runat="server" >
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 75%; padding-top:30px">
                                            <tr>
                                                <td>
                                                    <div class="valid_box">Template Successfully Copied.</div>
                                                </td>
                                            </tr>
                                            <tr><td></td></tr>
                                            <tr>
                                                <td align="right">
                                                    <input id="BtnOK" class="BtnCreate" type="submit" name="BtnOK" value="OK" onclick="closeDiv();"/>
                                                </td>
                                            </tr>
                                        </table> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>                       
                        </div>
                    </div>
                </div>
            </div>

            <div id="LPContainer" style="width: 100%; height: 100%;">
                <div id="AddLPDiv" class="web_dialog" style="top: 5%; left: 22%;">

                    <div id="Add_LP">
                        <a id="closeLP" onclick="closePOP(0,0);" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>

                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <table style="width: 100%; border: 0px; height: auto;" class="popsecondver">
                                    <tbody>
                                        <tr>
                                            <td align="right" colspan="2"></td>
                                        </tr>
                                        <tr>

                                            <td align="left" colspan="2">
                                                <h4>Add Lesson Plan</h4>
                                                <hr>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdText">Lesson Plan Name  </td>
                                            <td>
                                                <asp:TextBox ID="txtLPname" runat="server" MaxLength="30"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdText">Goal  </td>
                                            <td>
                                                <asp:DropDownList ID="ddlGoals" CssClass="drpClass" runat="server"></asp:DropDownList></td>
                                        </tr>


                                        <tr>
                                            <td class="tdText">Framework and Strand  </td>
                                            <td>
                                                <asp:TextBox ID="txtFrameworkAndStandard" runat="server" MaxLength="300"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdText">Specific Standard  </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecStandard" runat="server" MaxLength="300"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdText">Specific Entry Point  </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecEntryPoint" runat="server" MaxLength="300"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdText">Pre-requisite Skills  </td>
                                            <td>
                                                <asp:TextBox ID="txtPreskills" runat="server" MaxLength="300"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdText">Materials  </td>
                                            <td>
                                                <asp:TextBox ID="txtMaterials" runat="server" MaxLength="300"></asp:TextBox></td>
                                        </tr>
                                         <tr>
                                            <td class="tdText">Baseline Procedures  </td>
                                            <td>
                                                <asp:TextBox ID="txtBaseline" runat="server" MaxLength="300" TextMode="MultiLine" Rows="5" Columns="5" Width="226px"></asp:TextBox></td>
                                        </tr>
                                         <tr>
                                            <td class="tdText">Lesson Objectives  </td>
                                            <td>
                                                <asp:TextBox ID="txtObjective" runat="server" MaxLength="300" TextMode="MultiLine" Rows="5" Columns="5" Width="226px"></asp:TextBox></td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style1"></td>
                                            <td class="auto-style1">
                                                <asp:Button ID="btnAddLP" runat="server" CssClass="NFButtonWithNoImage" Width="100px" Text="Submit"  OnClick="btnAddLP_Click" /></td> 
                                           <%-- OnClientClick="disableButton();"--%>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table style="width: 98%">
                                                    <tr>
                                                        <td id="tdMsgForLp" runat="server" style="width: 100%"></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>


            <div id="LessonPlanContainer" style="width: 100%; height: 100%;">
                <div id="LpContainerDiv" class="web_dialog" style="top: 2%; left: 2%;">

                    <div id="sign_up5">
                        <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="closelp();">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                        <h3>View LessonPlan
               
                                    <iframe id="ifrmeLp" style="width: 100%" scrolling="auto"></iframe>
                    </div>
                    <div id="previewClose"></div>

                </div>

            </div>



            <div class="fullOverlay">
            </div>

            <div id="divPrmpts" class="web_dialog2">
                <a id="A1" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>


                <asp:GridView ID="grdLessonPlanDelete" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="No Data Found.." PageSize="12" AllowPaging="True"
                    OnPageIndexChanging="grdLessonPlanDelete_PageIndexChanging" GridLines="None" CellPadding="4" Style="margin-top: 20px" OnRowCommand="grdLessonPlanDelete_RowCommand">

                    <Columns>

                        <asp:BoundField DataField="GoalName" HeaderText="Goal" />
                        <asp:BoundField DataField="LessonPlanName" HeaderText="Lesson Plan" />
                        <asp:BoundField DataField="LessonPlanDesc" HeaderText="Lesson Plan Description" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Deleted On" />
                        <asp:BoundField DataField="CreatedOn" HeaderText="Created On" />

                        <asp:TemplateField HeaderText="Activate">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("StdtLessonPlanId") %>'
                                    CommandName="Activate">Activate</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" Style="border-width: 0px; background: none repeat scroll 0px 0px purple; height: 23px; width: 25px;"
                                    runat="server" ImageUrl="~/Administration/images/view_02.png" CommandArgument='<%# Eval("StdtLessonPlanId") %>'
                                    CommandName="View" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                    <RowStyle CssClass="RowStyle" />
                    <AlternatingRowStyle CssClass="AltRowStyle" />
                    <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView>

            </div>
    </form>
    <script type="text/javascript">

        function popPrompts() {

            $(".fullOverlay").show();
            $(".web_dialog2").show();



        }
        function HidePopup() {
            $(".fullOverlay").hide();
            $(".web_dialog2").hide();
        }

        $(document).ready(function () {

            parent.closeMessage();//call parent method for closing message

            var cglList = $('.kk');
            for (var i = 0; i < cglList.length; i++) {
                var innerContentLength = $(cglList[i]).html().length;
                $(cglList[i]).attr('title', $(cglList[i]).html());

                if (innerContentLength > 25) {

                    $(cglList[i]).html($(cglList[i]).html().substring(0, 25) + "...");

                }
            }

            var assgnGoalsList = $('.dd').parent().find('a').not('a.dlt');

            for (var i = 0; i < assgnGoalsList.length; i++) {
                var innerContentLength = $(assgnGoalsList[i]).html().length;
                $(assgnGoalsList[i]).attr('title', $(assgnGoalsList[i]).html());

                if (innerContentLength > 25) {

                    $(assgnGoalsList[i]).html($(assgnGoalsList[i]).html().substring(0, 25) + "...");

                }
            }

        });
    </script>
</body>
</html>
