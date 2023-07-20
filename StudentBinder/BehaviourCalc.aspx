<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BehaviourCalc.aspx.cs" Inherits="StudentBinder_BehaviarCalc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" type="text/css" />
    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../Administration/JS/tabber.js"></script>
    <link rel="stylesheet" href="../Administration/CSS/tabmenu.css" type="text/css" media="screen" />

    <script src="timePicker/jquery.timeentry.js"></script>
    <script src="timePicker/jquery.mousewheel.js"></script>
    <link href="timePicker/jquery.timeentry.css" rel="stylesheet" />

    <title></title>
    <script type="text/javascript">

        $(function () {
            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {
                var styl = document.getElementById('timerDiv').style;
                styl.width = "91%";
            }
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
    <script type="text/javascript">


        $(function () {
            $('.txtTime').timeEntry({
                showSeconds: true,
                beforeShow: customRange
                // beforeSetTime:testing
            });
        });
        $(function () {
            $('.txtTime1').timeEntry({ showSeconds: true, beforeShow: customRange });
        });

        //$(document).ready(function () {

        // if (document.getElementById('<%= hdndelete.ClientID %>').value == "notpartial") {                    
        //document.getElementById('Partialdiv').setAttribute("class", "tabbertab tabbertabhide");
        //document.getElementById('notpartial').setAttribute("class", "tabbertab ");
        //$('.tabbernav:eq(0)').removeClass("tabbernav");
        //$('.tabbernav:eq(1)').addClass("tabbernav");
        //}
        //});



        function tabselection() {
            document.getElementById('<%= hdndelete.ClientID %>').value = "notpartial";
        }


        function deleteSystem() {
            var flag;
            flag = confirm("Are you sure you want to delete this Behavior - IOA Interval?");
            document.getElementById('<%= hdndelete.ClientID %>').value = flag;

        }
        function deleteSystem1() {
            var flag;
            flag = confirm("Are you sure you want to delete this Behavior - IOA Interval?");
            document.getElementById('<%= hdndelete1.ClientID %>').value = flag;

        }


            function customRange(input) {
                return {
                    minTime: (input.id == 'txtEnd' ?
                $('#txtStart').timeEntry('getTime') : null),
                    maxTime: (input.id == 'txtStart' ?
            $('#txtEnd').timeEntry('getTime') : null)
                };
            }



            function timeTo24Hr(timeString, period) {
                // alert(timeString);
                var Alltime = timeString.split(':');
                // alert('hi');
                var h = parseInt(Alltime[0], 10);
                var m = parseInt(Alltime[1], 10);
                var s = parseInt(Alltime[2].substring(0, 2), 10);
                var prd = parseInt(period, 10);


                var startAMPM = Alltime[2].substring(2, 4);
                //alert(startAMPM);


                if ((startAMPM == "PM") || (h >= 12)) {
                    if (h != 12) {
                        h += 12;
                    }
                }


                m += prd;
                if ((m > 60) || (m == 60)) {
                    //  alert(m);
                    m = m - 60;
                    h = h + 1;
                    //  alert(h);
                }

                var hh, mm, ss, ap;
                if (h < 12) {
                    ap = 'AM';
                }
                else {
                    if (h != 12) {
                        h -= 12;
                    }
                    ap = 'PM';
                }
                if (h < 10) {
                    hh = '0' + h;
                }
                else {
                    hh = h;
                }
                if (m < 10) {
                    mm = '0' + m;
                }
                else {
                    mm = m;
                }
                if (s < 10) {
                    ss = '0' + s;

                }
                else {
                    ss = s;
                }

                // alert(hh + ":" + mm + ":" + ss+ap);
                return (hh + ":" + mm + ":" + ss + ap);
            }

            function timeTo24Hrsub(timeString, period) {
                //alert(timeString);
                var Alltime = timeString.split(':');
                var len = Alltime.length;

                // alert('hi');
                var h = parseInt(Alltime[0], 10);
                var m = parseInt(Alltime[1], 10);
                var s = parseInt(Alltime[2].substring(0, 2), 10);
                var prd = parseInt(period, 10);


                var startAMPM = Alltime[2].substring(2, 4);
                //alert(startAMPM);
                // alert(h);
                // alert(m);


                if ((startAMPM == "PM") || (h >= 12)) {
                    if (h != 12) {
                        h += 12;
                        // alert(h);
                    }
                }

                m -= prd;

                if (m < 0) {
                    m = 60 + m;
                    //   alert(m);
                    h = h - 1;
                    //  alert(h);
                }

                //  alert(h);
                var hh, mm, ss, ap;
                if (h < 12) {
                    ap = 'AM';
                }
                else {
                    if (h != 12) {
                        h -= 12;
                    }

                    ap = 'PM';
                }
                if (h < 10) {
                    hh = '0' + h;
                }
                else {
                    hh = h;
                }
                if (m < 10) {
                    mm = '0' + m;
                }
                else {
                    mm = m;
                }
                if (s < 10) {
                    ss = '0' + s;

                }
                else {
                    ss = s;
                }

                // alert(hh + ":" + mm + ":" + ss+ap);
                return (hh + ":" + mm + ":" + ss + ap);
            }

            function blurFortxt(txtId) {
                alert(txtId);
                if (document.getElementById(txtId).value != '') { 
                    var checkInput = txtId.indexOf("txtStime");
                    alert(checkInput);
                    if (checkInput>=0) {
                        // lblPeriod
                        var endid = txtId.substring(9, txtId.length);
                        var stdin24 = timeTo24Hr(document.getElementById(txtId).value, document.getElementById('hdPeriod' + endid).value);
                        // alert(stdin24);

                        endidFinal = 'txtEndtime' + endid;
                        // alert(endidFinal);
                        document.getElementById(endidFinal).value = stdin24;
                        // document.getElementById(endidFinal).focus();

                    }
                    else {
                        // timeTo24Hrsub
                        var startdid = txtId.substring(10, txtId.length);
                        var stdin24 = timeTo24Hrsub(document.getElementById(txtId).value, document.getElementById('hdPeriod' + startdid).value);
                        // alert(stdin24);                   

                        startidFinal = 'txtStTime' + startdid;
                        // alert(endidFinal);
                        document.getElementById(startidFinal).value = stdin24;
                        // document.getElementById(startidFinal).focus();
                        //ButtonSave

                    }
                }
                else {
                    //alert('drgergf');
                    // $('#tdMsg').html("<div class='error_box'> sgfnsjnbgsj.</div>");

                }
            }

            function customRange(input) {

                //   $('body').append('hello');

                var checkInput = input.id.substring(0, 9);

                var endidFinal;
                var startidFinal;
                if (checkInput == 'txtStTime') {
                    var endid = input.id.substring(9, input.id.length);
                    endidFinal = 'txtEndtime' + endid;
                    startidFinal = input.id;

                    //time = input.value.toString('HH:mm:ss');
                    //tt = time.split(":");
                    //sec = tt[0] * 3600 + tt[1] * 60 + tt[2] * 1;
                    ////alert(sec);
                }
                else {
                    var startdid = input.id.substring(10, input.id.length);
                    endidFinal = input.id;
                    startidFinal = 'txtStTime' + startdid;
                }
                //alert(startidFinal+','+endidFinal);    

                /*   return {
                       minTime: (input.id == endidFinal ?
              $('#'+startidFinal).timeEntry('getTime') : null),
                       maxTime: (input.id == startidFinal ?
               $('#'+endidFinal).timeEntry('getTime') : null)
                   };*/
                return {
                    minTime: (input.id == endidFinal ?
                       '03:00:00AM' : null),
                    maxTime: (input.id == startidFinal ?
                       '11:30:00PM' : null)
                };





            }



            var flagSaveChange = 0;
            var prev_val = '';

            //getAlarm();



            function IOAMarker(val) {

                var splitId = val.split(',');

                //alert(splitId[1]);
                var street = splitId[1];
                // var che = document.getElementById(val).checked;
                //alert(che);
                if (document.getElementById(val).checked == true) {

                    document.getElementById('dRpUserName' + splitId[1]).style.display = "inline";
                    // document.getElementById(val).checked = true;
                }
                else {
                    document.getElementById('dRpUserName' + splitId[1]).style.display = "none";
                    // document.getElementById(val).checked = false;
                }


            }

            function delDynRow(val) {
                var idtr = "tbRow" + val;
                // var txt1 = "textboxEndTime" + val;
                //   var txt2 = "textboxStartTime" + val;
                var hdval = "hd" + val;

                //  document.getElementById(txt1).style.display = "none";
                //   document.getElementById(txt2).style.display = "none";
                document.getElementById(idtr).style.display = "none";
                document.getElementById("HiddenFieldDelStatus").value = document.getElementById("HiddenFieldDelStatus").value + val + ',';

            }

            function succesMsg(msg) {
                if (msg == "Student Reminder Saved Successfully") {
                    parent.getAlarm();
                }
                //alert(msg);
                document.getElementById('tdMsg').innerHTML = "<div class='valid_box'>" + msg + ".</div>";
                HideLabel('tdMsg');
            }

            function outerSuccesMsg(msg) {
                if (msg == "Student Reminder Saved Successfully") {
                    parent.getAlarm();
                }
                //alert(msg);
                //document.getElementById('tdMsg').innerHTML = "<div class='valid_box'>" + msg + ".</div>";
                //document.getElementById('tdMsgOuter').innerHTML = "<div class='valid_box'>" + msg + ".</div>";
                $("#tdMsgOuter").html("<div class='valid_box'>Student Reminder Saved Successfully.</div>");
                //HideLabel('tdMsg');
                HideLabel('tdMsgOuter');
            }

            function ErrorMsg(msg) {
                //alert(msg);
                document.getElementById('tdMsg').innerHTML = "<div class='error_box'>" + msg + ".</div>";

            }

            function HideLabel(label) {
                var seconds = 5;
                setTimeout(function () {
                    //document.getElementById(""+label).style.display = "none";
                    document.getElementById("" + label).innerHTML = "";
                }, seconds * 1000);
            };

            function fn_checkTime(e, type) {



                var statTime = $(e).parent().find('.drpStart').val();
                var statTimeAP = $(e).parent().find('.drpStartAP').val();
                var endTime = $(e).parent().find('.drpEnd').val();
                var endTimeAP = $(e).parent().find('.drpEndAP').val();


                //alert('startTime: ' + statTime + statTimeAP + '/ end time: ' + endTime + endTimeAP);


                if ((statTimeAP == 'PM') && (endTimeAP == 'AM')) {
                    //if ((type == 'drpStartAP') || (type == 'drpEndAP'))
                    //{
                    flagSaveChange = flagSaveChange + 1;
                    //}
                    //  alert(flagSaveChange);

                }
                else {
                    var StrhrMin = statTime.split(':');
                    var EndhrMin = endTime.split(':');

                    if (statTimeAP == "PM") {
                        StrhrMin[0] = (parseInt(StrhrMin[0]) + 12).toString();
                    }

                    if (endTimeAP == "PM") {
                        EndhrMin[0] = (parseInt(EndhrMin[0]) + 12).toString();
                    }

                    if ((parseInt(StrhrMin[0]) > parseInt(EndhrMin[0])) && (parseInt(StrhrMin[0]) != 24)) {
                        // alert(parseInt(StrhrMin[0]));
                        //  alert(parseInt(EndhrMin[0]));

                        flagSaveChange = flagSaveChange + 1;
                    }
                    else if (parseInt(StrhrMin[0]) == parseInt(EndhrMin[0])) {
                        if (parseInt(StrhrMin[1]) > parseInt(EndhrMin[1])) {
                            flagSaveChange = flagSaveChange + 1;
                        }
                    }
                    else {
                        if (flagSaveChange > 0) {
                            flagSaveChange = flagSaveChange - 1;
                        }
                    }

                }

                // alert(flagSaveChange + 'end');
                // alert(flagSaveChange);
                document.getElementById('tdMsg').innerHTML = "";
                if (flagSaveChange == 0) {

                }
                else {
                    var oldVal = prev_val;
                    $(e).val(oldVal);
                    flagSaveChange = 0;

                    return false;
                }
                return false;
            }

            function checkSave() {
                alert(flagSaveChange);
                document.getElementById('tdMsg').innerHTML = "";
                if (flagSaveChange == 0) {
                    alert(flagSaveChange);
                    flagSaveChange = 0;
                    return true;
                }
                else {
                    alert(flagSaveChange);
                    succesMsg('Invalid Dates Please Check');
                    flagSaveChange = 0;
                    return false;
                }
            }

            function dropFocus(e) {
                prev_val = $(e).val();
            }

            //function chekall() {

                //var checkallStatus = document.getElementById('').checked;
            //    if (checkallStatus == true) {
            //        $(".chkRem input[type='checkbox']").attr('checked', true);
            //    }
            //    else {
            //        $(".chkRem input[type='checkbox']").removeAttr('checked');
            //    }

        //}

            function scrollToTop() {
                window.scrollTo(0, 0);
                window.parent.parent.scrollTo(0, 0);
            }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }

        .auto-style2 {
            width: 100%;
        }

        .auto-style3 {
            width: 189px;
        }

        .auto-style4 {
            width: 188px;
        }

        .auto-style5 {
            width: 164px;
        }
    </style>
    <style type="text/css">
        .Initial {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            /*background: url("../Images/InitialImage.png") no-repeat right top;*/
            background-color: #0054A0;
            color: Black;
            font-weight: bold;
        }

            .Initial:hover {
                color: White;
                /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
                background-color: #0099B5;
            }

        .Clicked {
            float: left;
            display: block;
            /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
            background-color: #1EB53A;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left: 8%">


            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>





        </div>
        <div id="timerDiv">
            <h2 style="margin-top:10px;"><span id="status" runat="server" style="color: #001666;font-family: sans-serif;font-size:Medium;white-space:nowrap;font-weight: normal;">Current Status: Timers Off</span></h2>
            <button id="play" style="display:none">Play</button>
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right">
                        <asp:ImageButton ID="btnrefeshing" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnrefeshing_Click" />
                    </td>
                </tr>

            </table>
            <asp:Button Text="Timers On" runat="server" onclick="AllTimersOnClick" id="ButtonOn" class="NFButton" style="width:150px;height:50px;margin-left: 15%;font-size:17px;white-space: normal;background-color: #4CAF50;"/>
                    <asp:Button Text="Timers Off" runat="server" onclick="AllTimersOffClick" id="ButtonOff" class="NFButton" style="width:150px;height:50px;margin-left: 1%;font-size:17px;white-space: normal;background-color: #a7a7a7;"/>
                    <h2 style="margin-top:10px;"><span style="color: #001666;font-family: sans-serif;font-size:Medium;white-space:nowrap;margin-left:17%;font-weight: normal;">Or customize timer options below:</span></h2>


            <table style="width: 100%">
                <tr>
                    <td>
                    <%--<asp:Button Text="Set Reminders/IOA - Interval" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"--%>
                        <asp:Button Text="Fixed Interval" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                            OnClick="Tab1_Click" ForeColor="White" />
                        <%--&nbsp;&nbsp; 
          <asp:Button Text="IOA - Others" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" ForeColor="White" />--%>
                        <asp:MultiView ID="MainView" runat="server">
                            <asp:View ID="View1" runat="server">
                                <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
               
                                    <tr>
                                        <td>

                                            <%--<h2>Set Reminders/IOA - Interval</h2>--%>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                            <table style="width: 100%;">
                                                                     <tr>
                                        <td width="100%" id="tdMsg" runat="server"></td>

                                    </tr>
                                                <tr>
                                                    <asp:CheckBox ID="check_All" runat="server" style="margin-left: 348px;" AutoPostBack="true" Text="Select All" OnCheckedChanged = "checkAll"/>
                                                <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Save" CssClass="NFButton" Style="margin-left: 2%" Width="100px" OnClientClick="javascript: scrollToTop();"/>
                                                                            <asp:Label ID="LabelMsg" runat="server"></asp:Label>
                                                    </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DataList ID="DataList1" runat="server" OnItemDataBound="DataList1_ItemDataBound" Width="80%">
                                                            <ItemTemplate>
                                                                <table style="width:100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblbehavior" runat="server" Text='<%# Eval("Behaviour") %>' Font-Size="Medium" ForeColor="#006600"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="grd_Behaviorpartial" runat="server" AutoGenerateColumns="False" DataKeyNames="MeasurmentId" EmptyDataText="No Data Found.." GridLines="None" OnRowCommand="grd_Behaviorpartial_RowCommand" OnRowDataBound="grd_Behaviorpartial_RowDataBound" OnRowDeleting="grd_Behaviorpartial_RowDeleting" Width="75%" AllowPaging="True" OnPageIndexChanging="grd_Behaviorpartial_PageIndexChanging" >
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Start Time">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtStime" runat="server" Width="100px" Text='<%# Eval("StartTime") %>' Enabled="false" Font-Bold="True"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="150px" />
                                                                                        <ItemStyle Width="150px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="End Time">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtEtime" runat="server" Width="100px" Text='<%# Eval("EndTime") %>' Enabled="false" Font-Bold="True"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="150px" />
                                                                                        <ItemStyle Width="150px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Remind Me">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkRM" runat="server" Checked='<%# Convert.ToBoolean(Eval("Remainder")) %>' OnCheckedChanged="chkRM_CheckedChanged" AutoPostBack="True"/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="IOA">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkIOA" runat="server" AutoPostBack="True" Checked='<%# Convert.ToBoolean(Eval("IOAFlag")) %>' OnCheckedChanged="chkIOA_CheckedChanged" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="IOA User">
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlUser0" runat="server" CssClass="drpClass" Enabled="false" AutoPostBack="True" OnSelectedIndexChanged="ddlUser0_SelectedIndexChanged" >
                                                                                            </asp:DropDownList>
                                                                                            <asp:HiddenField ID="IOAUser" runat="server" Value='<%# Eval("IOAUser") %>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Delete">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="lb_delete0" runat="server" class="btn btn-red" CommandArgument='<%# Eval("BehaviourCalcId") %>' CommandName="Delete" ImageUrl="~/Administration/images/trash.png" OnClientClick="javascript:deleteSystem1();" />
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
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>

                                                                            <asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("MeasurementId") %>' />

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                      <%--  <table class="display" style="width: 100%; margin-left: 12%; min-width: 900px">

                                                            <tr>
                                                                <td>

                                                                    <asp:HiddenField ID="HdnFldBehavCalcId" runat="server" />

                                                                    <asp:HiddenField ID="HiddenFieldtxtIdz" runat="server" />

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style3" colspan="2">
                                                                    <asp:Label ID="LabelHidden" runat="server" Visible="False"></asp:Label>
                                                                    <asp:Label ID="LabelHiddenNo" runat="server" Visible="False"></asp:Label>
                                                                    <asp:HiddenField ID="HiddenFieldDelStatus" runat="server" />
                                                                    <asp:CheckBox ID="chkAllBox" runat="server" Text="Check All" Style="margin-left: 470px" OnClick='chekall();' />
                                                                </td>
                                                                <td class="auto-style4">&nbsp;</td>
                                                            </tr>

                                                            <tr>
                                                                <td class="auto-style2" colspan="3">
                                                                    <asp:PlaceHolder ID="PlaceHolderTime" runat="server"></asp:PlaceHolder>

                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="auto-style1" colspan="3">

                                                                    <asp:HiddenField ID="HiddenFieldStudentId" runat="server" />

                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="auto-style2" colspan="3" style="text-align: center; height: 20px">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5" style="text-align: right;">
                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Save" CssClass="NFButton" Style="margin-left: -30%" Width="100px" />
                                                                            <asp:Label ID="LabelMsg" runat="server"></asp:Label>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>

                                                                <td class="auto-style2" style="text-align: left">&nbsp;</td>

                                                                <td class="auto-style2" style="text-align: center"></td>

                                                            </tr>

                                                        </table>--%>
                                                         <asp:HiddenField ID="hdndelete1" runat="server" />
                                                         <%-- <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Save" CssClass="NFButton" Style="margin-left: 25%" Width="100px" OnClientClick="javascript: scrollToTop();"/>
                                                                            <asp:Label ID="LabelMsg" runat="server"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>


                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                    <tr>
                                        <td runat="server" id="tdMsg1"></td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <h2>IOA - Others</h2>

                                            <table style="width: 100%">

                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grd_Behavior" runat="server" Width="98%" AutoGenerateColumns="False" AllowPaging="True"
                                                            EmptyDataText="No Data Found.." PageSize="5"
                                                            GridLines="None" OnRowCommand="grd_Behavior_RowCommand" OnPageIndexChanging="grd_Behavior_PageIndexChanging" OnRowDataBound="grd_Behavior_RowDataBound" DataKeyNames="MeasurementId" OnRowDeleting="grd_Behavior_RowDeleting">



                                                            <Columns>
                                                                <asp:BoundField DataField="Behaviour" HeaderText="Behavior" />
                                                                <asp:TemplateField HeaderText="Start Time">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlHourstart" CssClass="drpClass" runat="server" Width="60px">
                                                                            <asp:ListItem Value="0">Hr</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlMinutestart" CssClass="drpClass" runat="server" Width="60px">
                                                                            <asp:ListItem Value="0">Min</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlSecondstart" CssClass="drpClass" runat="server" Width="60px" Visible="false">
                                                                            <asp:ListItem Value="0">00</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="DropDownListAMPMstart" runat="server" CssClass="drpClass" Visible="true" Width="60px">
                                                                            <asp:ListItem>AM</asp:ListItem>
                                                                            <asp:ListItem>PM</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="End Time">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlHourEnd" CssClass="drpClass" runat="server" Width="60px">
                                                                            <asp:ListItem Value="0">Hr</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlMinuteEnd" CssClass="drpClass" runat="server" Width="60px">
                                                                            <asp:ListItem Value="0">Min</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlSecondEnd" CssClass="drpClass" runat="server" Width="60px" Visible="false">
                                                                            <asp:ListItem Value="0">00</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="DropDownListAMPMEnd" runat="server" CssClass="drpClass" Visible="true" Width="60px">
                                                                            <asp:ListItem>AM</asp:ListItem>
                                                                            <asp:ListItem>PM</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IOA User">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="drpClass">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="lb_delete" OnClientClick="javascript:deleteSystem();"
                                                                            CommandName="Delete" runat="server"
                                                                            CommandArgument='<%# Eval("MeasurementId") %>'
                                                                            ImageUrl="~/Administration/images/trash.png" class="btn btn-red"></asp:ImageButton>
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
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hdndelete" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick="javascript:tabselection(); scrollToTop();" />
                                                    </td>
                                                </tr>
                                            </table>


                                        </td>
                                    </tr>
                                </table>
                            </asp:View>

                        </asp:MultiView>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <div id="noDataMsg" runat="server" style="width:90%;font-size:10pt;padding:20px;color: #002060;margin:0 auto;margin-left: 10px;"><p>Note: Only <b>fixed interval</b> behaviors are listed above. If this client has none, then none will be listed. For all-purpose, unscheduled timers, please click the blue "Timers" bar above the Behavior Panel on the right instead.</p>
        </div>
        </div>
        <%--<div class="tabber" style="width: 100%;" id="tabformgoal" runat="server">


                    <div class="tabbertab" id="Partialdiv" runat="server">

                      
                    </div>


                    <div class="tabbertab" id="notpartial" runat="server">

                       
                    </div>


                </div>--%>
    </form>
</body>
</html>
