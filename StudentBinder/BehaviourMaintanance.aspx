<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BehaviourMaintanance.aspx.cs" Inherits="StudentBinder_BehaviourMaintanance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />

    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <script src="timePicker/jquery.timeentry.min.js"></script>
    <link href="timePicker/jquery.timeentry.css" rel="stylesheet" />
    <script src="timePicker/jquery.mousewheel.js"></script>

    <script src="../Administration/jsDatePick.jquery.min.1.3.js"></script>
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <link href="../Administration/jsDatePick_ltr.css" rel="stylesheet" />

    <script>
        function hideBehEdit() {
            var x = document.getElementById('<%= BehNamediv.ClientID %>');
            x.style.display = "none";
            return false;
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

        function drpChanged() {

            if ($('#txtInterval').val() != "" &&
                $('#ddlHour').val() != null && $('#ddlMinute').val() != null && $('#txtInterval').val() != null &&
                $('#ddlEndHr').val() != null && $('#ddlEndMin').val() != null && $('#ddlEndFrmt').val() != null &&
                $('#ddlHour').val() != "0" && $('#ddlMinute').val() != "0" &&
                $('#ddlEndHr').val() != "0" && $('#ddlEndMin').val() != "0") {

                var startTime = $('#ddlHour').val() + ":" + $('#ddlMinute').val() + "" + $('#DropDownListAMPM').val();
                var endTime = $('#ddlEndHr').val() + ":" + $('#ddlEndMin').val() + "" + $('#ddlEndFrmt').val();

                var splitStartTime = startTime.split(":");
                var startHour = splitStartTime[0];
                var startMinute = splitStartTime[1].substring(0, 2);
                var startAmPm = splitStartTime[1].substring(2, 4);
                if (startAmPm == "PM") {
                    if (startHour != 12) {
                        startHour = parseInt(startHour) + 12;
                    }
                } else if (startAmPm == "AM") {
                    if (startHour == 12) {
                        startHour == 0;
                    }
                }

                var splitEndTime = endTime.split(":");
                var endHour = splitEndTime[0];
                var endMinute = splitEndTime[1].substring(0, 2);
                var endAmPm = splitEndTime[1].substring(2, 4);
                if (endAmPm == "PM") {
                    if (endHour != 12) {
                        endHour = parseInt(endHour) + 12;
                    }
                } else if (endAmPm == "AM") {
                    if (endHour == 12) {
                        endHour == 0;
                    }
                }

                var startDate = "12/01/2015";
                var endDate = "12/01/2015";

                if (endMinute == "59") {
                    endMinute = "00";
                    endHour++;
                    if (endHour == "24") {
                        endHour = "00";
                        endDate = "12/02/2015";
                    }
                }
                else {
                    endMinute++;
                }

                var finalStartTime = startDate + " " + startHour + ":" + startMinute + ":00";
                var finalEndTime = endDate + " " + endHour + ":" + endMinute + ":00";

                finalEndTime1 = new Date(finalEndTime);
                finalStartTime1 = new Date(finalStartTime);

                var hours1 = Math.abs(finalEndTime1 - finalStartTime1) / 36e5;

                var intv = $('#txtInterval').val();


                var noOfTimes = 0;

                if (finalStartTime1 < finalEndTime1) {
                    noOfTimes = Math.floor((hours1 * 60) / intv);
                }

                $('#txtNoOfTimes').val(noOfTimes);
            }
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

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 0);
        }
    </script>


    <style type="text/css">
        .chkStyle {
        }

            .chkStyle input {
                width: auto !important;
                float: left !important;
                margin-right: 3px;
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
            background-color: gray;
            opacity: .8;
            filter: alpha(opacity=.8);
            -moz-opacity: .8;
            z-index: 101;
            display: none;
        }

        .web_dialog {
            display: none;
            position: fixed;
            width: 570px;
            height: auto;
            left: 50%;
            margin-left: -277px;
            margin-top: -200px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 102;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

        .auto-style1 {
            height: 27px;
        }

        input.checkbox, input.radio {
            margin-left: 10px;
            display: block;
        }


        .centerTd {
            text-align: center;
        }



        .auto-style2 {
            height: 19px;
        }
    </style>
    <script type="text/javascript">



        function loadTime() {



            $('.txtTime').timeEntry({ showSeconds: true });
            new JsDatePick({
                useMode: 2,
                target: '<%=(txtStartDate).ClientID %>',
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: '<%=(txtEndsOn).ClientID %>',
                dateFormat: "%m/%d/%Y",
            });
            return false;

        }
        /* $(function () {
             $('.txtTime').timeEntry({ showSeconds: true, beforeShow: customRange });
         });*/

        function deleteSystem() {
            var r = confirm("Confirm Deletion");
            if (r == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function deleteSystemBeh() {
            var r = confirm("Are you sure you want to delete this behavior data?");
            if (r == true) {
                return true;
            }
            else {
                return false;
            }
        }


        function isNumberKey(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;
            //alert(charCode);

            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;
            else if (charCode == 46)
                return true;
            // if ((charCode < 48 || charCode > 57 )&& charCode > 31)
            //      return false;
            return true;
        }

        function isNumberKey10(evt) {

            alert(evt);
            var charCode = (evt.which) ? evt.which : event.keyCode;
            //alert(charCode);

            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;
            else if (charCode == 46)
                return true;
            // if ((charCode < 48 || charCode > 57 )&& charCode > 31)
            //      return false;
            return true;
        }


        function nokeyPressInDate(evt) {
            return false;
        }


        function ifWeekCheck(valueCheck) {
            //   alert(valueCheck);
            //checkBxWeek

            if (valueCheck == 'Weekly') {
                $('#<%=(ChbxSun).ClientID %>').attr('checked', false);
                $('#<%=(ChbxMon).ClientID %>').attr('checked', true);
                $('#<%=(ChbxTue).ClientID %>').attr('checked', false);
                $('#<%=(ChbxWed).ClientID %>').attr('checked', false);
                $('#<%=(ChbxThu).ClientID %>').attr('checked', false);
                $('#<%=(ChbxFri).ClientID %>').attr('checked', false);
                $('#<%=(ChbxSat).ClientID %>').attr('checked', false);

                document.getElementById('<%=(checkBxWeek).ClientID%>').style.display = 'block';

            }
            else
                document.getElementById('<%=(checkBxWeek).ClientID%>').style.display = 'none';
            return false;
        }


        function checkChecked() {
            alert('dfgd');
            var chkIfChecked = document.getElementById('<%=(CheckBoxCostLessPlan).ClientID %>');
            alert(chkIfChecked.value);
            if (chkIfChecked.checked == true) {
                document.getElementById('DivLesion').style.display = 'block';
            }
            else {
                document.getElementById('DivLesion').style.display = 'none';
            }
        }

        function checkTick() {
            if ($('#chk24Hr').is(':checked')) {
                $("#DropDownListAMPM").prop('selectedIndex', 0);
                $("#ddlHour").prop('selectedIndex', 1);
                $("#ddlMinute").prop('selectedIndex', 1);
                $("#ddlEndHr").prop('selectedIndex', 12);
                $("#ddlEndMin").prop('selectedIndex', 60);
                $("#ddlEndFrmt").prop('selectedIndex', 1);

                $("#DropDownListAMPM").prop('disabled', true);
                $("#ddlHour").prop('disabled', true);
                $("#ddlMinute").prop('disabled', true);
                $("#ddlEndHr").prop('disabled', true);
                $("#ddlEndMin").prop('disabled', true);
                $("#ddlEndFrmt").prop('disabled', true);
            }
            else {
                $("#DropDownListAMPM").prop('selectedIndex', 0);
                $("#ddlHour").prop('selectedIndex', 0);
                $("#ddlMinute").prop('selectedIndex', 0);
                $("#ddlEndHr").prop('selectedIndex', 0);
                $("#ddlEndMin").prop('selectedIndex', 0);
                $("#ddlEndFrmt").prop('selectedIndex', 0);

                $("#DropDownListAMPM").prop('disabled', false);
                $("#ddlHour").prop('disabled', false);
                $("#ddlMinute").prop('disabled', false);
                $("#ddlEndHr").prop('disabled', false);
                $("#ddlEndMin").prop('disabled', false);
                $("#ddlEndFrmt").prop('disabled', false);
            }
            drpChanged();
        }

        $(document).ready(function () {

        });

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 0);
        }

    </script>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>

</head>

<body>
   
    <form id="form1" runat="server">
        <div>
            <div style="width: 100%; margin-left: auto; margin-right: auto">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="updatebehaviour" runat="server">
                    <ContentTemplate>


                        <table class="display" style="width: 95%; margin-left: 48px; margin-right: auto">
                            <tr>
                                <td style="width: 92%" id="tdMsg" runat="server"></td>
                                <td style="width: 3%; text-align: right">
                                    <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <table class="display">
                                        <tr>
                                            <td style="width: 150px">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">Behavior Name</td>
                                            <td class="auto-style1">
                                                <asp:TextBox ID="txtBehaviour" runat="server" MaxLength="30"></asp:TextBox>

                                            </td>
                                        </tr>


                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="auto-style6">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">General Settings:</td>
                                            <td class="auto-style6">
                                                <asp:CheckBox ID="chkPartial" runat="server" AutoPostBack="true" OnCheckedChanged="chkPartial_CheckedChanged" Text=" Interval" Style="margin-left: 10px" />
                                                <asp:CheckBox ID="CheckBoxCostLessPlan" runat="server" Text=" Custom Lesson Plan" Width="200px" OnCheckedChanged="CheckBoxCostLessPlan_CheckedChanged" AutoPostBack="True" />
                                                <asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" Style="margin-left: 10px" Visible="false" />
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdoAcceleration" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                    <asp:ListItem Selected="True" Value="1">Acceleration</asp:ListItem>
                                                    <asp:ListItem Value="0">Deceleration</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="auto-style6">
                                                <div id="DivLesion">
                                                    <asp:Button ID="ButtonAddLessionPlan" runat="server" OnClick="ButtonAddLessionPlan_Click" Text="Add Lesson Plan" CssClass="NFButtonNew" /><br />
                                                    <asp:CheckBox ID="CheckBoxLessonPlan" runat="server" Enabled="False" />
                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="auto-style6">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>Measurement&nbsp; </td>
                                            <td class="auto-style6">
                                                <asp:CheckBox ID="chkDuration" runat="server" Text=" Duration" />
                                                <asp:CheckBox ID="chkFrequency" runat="server" Text=" Frequency" Style="margin-left: 10px" AutoPostBack="true" OnCheckedChanged="chkFrequency_CheckedChanged1" />
                                                <asp:CheckBox ID="chkYesOrNo" runat="server" Text="Yes/No"  AutoPostBack="true" OnCheckedChanged="chkYesOrNo_CheckedChanged"/>
                                                <%--<asp:CheckBox ID="chkPartial" runat="server" AutoPostBack="true" OnCheckedChanged="chkPartial_CheckedChanged" Text=" Interval" Style="margin-left: 10px" />--%>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td></td>
                                            <td class="auto-style6">
                                                <%--<asp:RadioButtonList ID="rdoAcceleration" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                        <asp:ListItem Selected="True" Value="1">Acceleration</asp:ListItem>
                                                        <asp:ListItem Value="0">Deceleration</asp:ListItem>
                                                    </asp:RadioButtonList>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td class="auto-style6"></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="lblCalc" runat="server" Text="Calculation"></asp:Label></td>
                                            <td class="auto-style6">
                                                <asp:RadioButtonList ID="rdoSumTotal" runat="server" RepeatDirection="Horizontal" Visible="false" >
                                                    <asp:ListItem Selected="True" Value="0">Total</asp:ListItem>
                                                    <asp:ListItem Value="1">%Interval</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                       


                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="auto-style6">
                                                <%--<asp:CheckBox ID="CheckBoxCostLessPlan" runat="server" Text=" Custom Lesson Plan" Width="200px" OnCheckedChanged="CheckBoxCostLessPlan_CheckedChanged" AutoPostBack="True" />--%>
                                            </td>
                                            <td>
                                                <%--<asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" Style="margin-left: 10px" Visible="false" />--%>
                                            </td>
                                        </tr>






                                        <%--<tr>
                                                <td>&nbsp;</td>
                                                <td class="auto-style6">
                                                    <div id="DivLesion">
                                                        <asp:Button ID="ButtonAddLessionPlan" runat="server" OnClick="ButtonAddLessionPlan_Click" Text="Add Lesson Plan" CssClass="NFButtonNew" /><br />
                                                        <asp:CheckBox ID="CheckBoxLessonPlan" runat="server" Enabled="False" />
                                                    </div>

                                                </td>
                                            </tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <%--ashin--%>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="auto-style6">&nbsp;</td>
                                         </tr>
                                         <tr>
                                            <td style="width: 150px;">                                                
                                                <asp:Label ID="Label5" runat="server" Text="IEP Objective"></asp:Label>
                                            </td>
                                            <td>                                                
                                                <asp:TextBox ID="txtbehIEPobjtve" runat="server" TextMode="MultiLine" Rows="30" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">                                                
                                                <asp:Label ID="Label6" runat="server" Text="Baseline of Current Performance Level"></asp:Label>
                                            </td>
                                            <td>                                                
                                                <asp:TextBox ID="txtbehBasperlvl" runat="server" TextMode="MultiLine" Rows="15" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <%--definition label--%>
                                                <asp:Label ID="lblBehavDefinition" runat="server" Text="Definition"></asp:Label>
                                            </td>
                                            <td>
                                                <%--definition textarea--%>
                                                <asp:TextBox ID="txtBehavDefinition" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <%--strategy label--%>
                                                <asp:Label ID="lblBehavStrategy" runat="server" Text="Strategy"></asp:Label>
                                            </td>
                                            <td>
                                                <%--strategy textarea--%>
                                                <asp:TextBox ID="txtBehavStrategy" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <%--strategy label--%>
                                                <asp:Label ID="lblGoalDesc" runat="server" Text="Goal Description"></asp:Label>
                                            </td>
                                            <td>
                                                <%--strategy textarea--%>
                                                <asp:TextBox ID="txtGoalDesc" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panal_Partia" runat="server" BorderWidth="0px">
                                        <table class="display">
                                            <tr>
                                                <td style="width: 150px">Start Time</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlHour" CssClass="drpClass" runat="server" Width="60px" onchange="drpChanged()">
                                                        <asp:ListItem Value="0">Hr</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlMinute" CssClass="drpClass" runat="server" Width="60px" onchange="drpChanged()">
                                                        <asp:ListItem Value="0">Min</asp:ListItem>
                                                    </asp:DropDownList>

                                                    <%--<asp:DropDownList ID="ddlSecond" CssClass="drpClass" runat="server" Width="60px">
                                                            <asp:ListItem Value="0">Sec</asp:ListItem>
                                                        </asp:DropDownList>--%>
                                                    <asp:DropDownList ID="DropDownListAMPM" runat="server" CssClass="drpClass" Visible="true" Width="60px" onchange="drpChanged()">
                                                        <asp:ListItem>AM</asp:ListItem>
                                                        <asp:ListItem>PM</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;OR
                                                        <%-- <asp:TextBox ID="txtStartime" runat="server" Class="txtTime" MaxLength="20"></asp:TextBox>
                                    <asp:DropDownList ID="DropDownListTime" runat="server" CssClass="drpClass" Width="150px" Visible="False">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListAMPM" runat="server" CssClass="drpClass" Width="80px" Visible="False">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>--%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk24Hr" Text="24 Hours" runat="server" OnClick="checkTick();" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>End Time</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEndHr" CssClass="drpClass" runat="server" Width="60px" onchange="drpChanged()">
                                                        <asp:ListItem Value="0">Hr</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlEndMin" CssClass="drpClass" runat="server" Width="60px" onchange="drpChanged()">
                                                        <asp:ListItem Value="0">Min</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlEndFrmt" CssClass="drpClass" runat="server" Width="60px" onchange="drpChanged()">
                                                        <asp:ListItem>AM</asp:ListItem>
                                                        <asp:ListItem>PM</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td><span lang="EN-US"></span></td>

                                                <td>
                                                    <div>
                                                        <div style="width: 100%; height: 27px; position: absolute; z-index: 1000; background-color: white; opacity: 0;"></div>
                                                        <asp:TextBox ID="txtNoOfTimes" runat="server" Style="display: none;" onkeypress="return false;" onkeydown="return false;" MaxLength="5"></asp:TextBox>

                                                    </div>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="auto-style2"></td>
                                                <td class="auto-style2"></td>
                                            </tr>
                                            <tr>
                                                <td><span lang="EN-US">Period</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtPeriod" runat="server" onkeypress="return isNumberKey(event)" MaxLength="4"></asp:TextBox>
                                                    (minutes)</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td><span lang="EN-US">Interval</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtInterval" runat="server" onkeypress="return isNumberKey(event)" onkeyup="drpChanged()" MaxLength="4"></asp:TextBox>

                                                    (minutes)</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>Repeat </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlRepeat" runat="server" CssClass="drpClass" onchange="return ifWeekCheck(this.value)">
                                                        <asp:ListItem>Daily</asp:ListItem>
                                                        <asp:ListItem>Weekly</asp:ListItem>
                                                        <asp:ListItem>Every WeekDay(Monday to Friday)</asp:ListItem>
                                                        <asp:ListItem>Every Monday,Wednesday &amp; Friday</asp:ListItem>
                                                        <asp:ListItem>Every tuesday &amp; thursday</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <div id="checkBxWeek" style="display: none" runat="server">
                                                        <asp:CheckBox ID="ChbxSun" runat="server" Text=" Sun" />
                                                        <asp:CheckBox ID="ChbxMon" runat="server" Text=" Mon" Style="margin-left: 10px" />
                                                        <asp:CheckBox ID="ChbxTue" runat="server" Text=" Tue" Style="margin-left: 10px" />
                                                        <asp:CheckBox ID="ChbxWed" runat="server" Text=" Wed" Style="margin-left: 10px" />
                                                        <asp:CheckBox ID="ChbxThu" runat="server" Text=" Thu" Style="margin-left: 10px" />
                                                        <asp:CheckBox ID="ChbxFri" runat="server" Text=" Fri" Style="margin-left: 10px" />
                                                        <asp:CheckBox ID="ChbxSat" runat="server" Text=" Sat" Style="margin-left: 10px" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>Start Date</td>
                                                <td>
                                                    <asp:TextBox ID="txtStartDate" runat="server" onkeypress="return nokeyPressInDate(event)" onkeydown='allowBackSpace(event,this)' onpaste='return false'></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>End Date&nbsp;&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtEndsOn" runat="server" onkeypress="return nokeyPressInDate(event)" onkeydown='allowBackSpace(event,this)' onpaste='return false'></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: center" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSubmitIEP1_Click" Text="Save" Width="80px" OnClientClick="scrollToTop();"/>
                                    <asp:Button ID="btnCancel" runat="server" CssClass="NFButton" Text="Cancel" Width="80px" OnClick="btnCancel_Click" OnClientClick="scrollToTop();"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="GrdMeasurement" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." GridLines="None" OnRowCommand="GrdMeasurement_RowCommand" OnRowDataBound="GrdMeasurement_RowDataBound" OnRowDeleting="GrdMeasurement_RowDeleting" OnRowEditing="GrdMeasurement_RowEditing" Width="100%" AllowPaging="True" OnPageIndexChanging="GrdMeasurement_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Behavior">

                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Behaviour") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frequency" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkfrequency" runat="server" CssClass="chkStyle" Checked='<%# (Eval("Frequency").ToString()=="True")? true:false %>' Enabled="false" />
                                                    <asp:HiddenField ID="hdnfrequency" runat="server" Value='<%# Eval("Frequency") %>' />
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duration" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkduration" runat="server" CssClass="chkStyle" Checked='<%# (Eval("Duration").ToString()=="True")? true:false %>' Enabled="false" />
                                                    <asp:HiddenField ID="hdnduration" runat="server" Value='<%# Eval("Duration") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Yes/No" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkYesOrNo" runat="server" CssClass="chkStyle" Checked='<%# (Eval("YesOrNo").ToString()=="True")? true:false %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Interval" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkParIn" runat="server" CssClass="chkStyle" Checked='<%# (Eval("PartialInterval").ToString()=="True")? true:false %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custom Lesson Plan" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCstPln" runat="server" CssClass="chkStyle" Checked='<%# (Eval("cost").ToString()!="")? true:false %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Acceleration" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAcl" runat="server" CssClass="chkStyle" Checked='<%# (Eval("IsAcceleration").ToString()=="True")? true:false %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inactive" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkInactive" runat="server" CssClass="chkStyle" Checked='<%# (Eval("ActiveInd").ToString()=="N")? true:false %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lb_Edit" runat="server" AlternateText="Edit" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Edit" Height="20px" ImageUrl="~/Administration/images/user_edit.png" Width="20px" OnClientClick="scrollToTop();"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit prior Submissions">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lb_Edit2" runat="server" AlternateText="Edit prior Submissions" class="btn btn-blue" style="margin-left:30px" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Edit" Height="20px" ImageUrl="~/Administration/images/user_edit.png" Width="20px" OnClick="FillDetails3"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lb_delete" runat="server" AlternateText="Delete" class="btn btn-red" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Delete" Height="20px" ImageUrl="~/Administration/images/trash.png" OnClientClick="return deleteSystem();" Width="20px" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>

                                        <HeaderStyle CssClass="HeaderStyle" />
                                        <RowStyle CssClass="RowStyle" />
                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="HiddenFieldCheckPopup" runat="server" Value="0" />
                                    <asp:HiddenField ID="HiddenFieldMeaId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnid" runat="server" />
                                     <div id="BehNamediv" runat="server" visible="false" style="width:600px; padding: 5px 5px 30px 5px;border: 5px solid #b2ccca; height:auto">
                                         <img onclick="return hideBehEdit();"  style="float:right; width: 25px; height: 25px; z-index: 9999; left: 96%; top: 0.5%; margin-top: 1px;" src="../Administration/images/button_red_close.png" alt="jdnjk">

                                         <br />
                                         <h2 id="behName" runat="server" style="font-family: Calibri, sans-serif; color: #6b6b6b; font-size: 20px; margin-top: 15px;margin-left:10px"></h2>
                                         <hr style="width:2px;color:blue"/>
                                         <p style="margin-left:10px">
                                             <b>Choose Edit Option</b></p>
                                         <asp:RadioButtonList ID="EditList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ShowGrid" RepeatDirection="Horizontal">
                                             <asp:ListItem Enabled="true" Selected="True" Value="1">Frequency</asp:ListItem>
                                             <asp:ListItem Enabled="true" Value="2">Duration</asp:ListItem>
                                             <asp:ListItem Enabled="true" Value="3">YesOrNo</asp:ListItem>
                                         </asp:RadioButtonList>
                                        <br />
                                         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." GridLines="None" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" style="margin-left:50px" AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging_1" OnRowDeleting="GridView1_RowDeleting">
                                             <HeaderStyle CssClass="HeaderStyle" />
                                             <RowStyle CssClass="RowStyle" />
                                             <AlternatingRowStyle CssClass="AltRowStyle" />
                                             <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                             <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                             <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                             <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                             <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                             <SortedAscendingHeaderStyle BackColor="#487575" />
                                             <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                             <SortedDescendingHeaderStyle BackColor="#275353" />
                                             <Columns>
                                                 <asp:TemplateField HeaderText="Edit">
                                                     <ItemTemplate>
                                                         <asp:ImageButton ID="Edit" runat="server" AlternateText="Edit" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Edit" Height="20px" ImageUrl="~/Administration/images/user_edit.png" Width="20px" />
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" />
                                                     <EditItemTemplate>
                                                         <asp:Button ID="btn_Update" runat="server" CommandName="Update" Text="Update" />
                                                         <asp:Button ID="btn_Cancel" runat="server" CommandName="Cancel" Text="Reset" />
                                                     </EditItemTemplate>
                                                 </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Delete">
                                                     <ItemTemplate>
                                                         <asp:ImageButton ID="Delete" runat="server" AlternateText="Delete" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Delete" Height="20px" ImageUrl="~/Administration/images/Trash.png " Width="20px" OnClientClick="return deleteSystemBeh();"/>
                                                     </ItemTemplate>
                                                  </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="BehaviorId" Visible="false">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_Id" runat="server" Text='<%#Eval("BehaviourId") %>'></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Frequecy Count">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_frq" runat="server" style="margin-left:20px" Text='<%#Eval("FrequencyCount") %>'></asp:Label>
                                                     </ItemTemplate>
                                                     <EditItemTemplate>
                                                         <asp:TextBox ID="txt_frq" runat="server" style="width:60px" Text='<%#Eval("FrequencyCount") %>'></asp:TextBox>
                                                     </EditItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Event Time">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_City" runat="server" Text='<%#Eval("EventTime") %>'></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                             </Columns>
                                         </asp:GridView>
                                         <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." GridLines="None" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" style="margin-left:50px " AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging_2" OnRowDeleting="GridView2_RowDeleting">
                                             <HeaderStyle CssClass="HeaderStyle" />
                                             <RowStyle CssClass="RowStyle" />
                                             <AlternatingRowStyle CssClass="AltRowStyle" />
                                             <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                             <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                             <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                             <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                             <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                             <SortedAscendingHeaderStyle BackColor="#487575" />
                                             <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                             <SortedDescendingHeaderStyle BackColor="#275353" />
                                             <Columns>
                                                 <asp:TemplateField HeaderText="Edit">
                                                     <ItemTemplate>
                                                         <asp:ImageButton ID="Edit" runat="server" AlternateText="Edit" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Edit" Height="20px" ImageUrl="~/Administration/images/user_edit.png" Width="20px" />
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" />
                                                     <EditItemTemplate>
                                                         <asp:Button ID="btn_Update" runat="server" CommandName="Update" Text="Update" />
                                                         <asp:Button ID="btn_Cancel" runat="server" CommandName="Cancel" Text="Reset" />
                                                     </EditItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Delete">
                                                     <ItemTemplate>
                                                         <asp:ImageButton ID="Delete" runat="server" AlternateText="Delete" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Delete" Height="20px" ImageUrl="~/Administration/images/Trash.png " Width="20px" OnClientClick="return deleteSystemBeh();"/>
                                                     </ItemTemplate>
                                                  </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="BehaviorId" Visible="false">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_Id" runat="server" Text='<%#Eval("BehaviourId") %>'></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Duration&lt;/br&gt;(In Seconds)">
                                                     <ItemTemplate>
                                                         <asp:Label ID="Label2" runat="server" Text='<%# Eval("Hr")+ " Hr " %>' Visible="false"></asp:Label>
                                                         <asp:Label ID="Label3" runat="server" Text='<%#Eval("min")+ " Min"  %>' Visible="false"></asp:Label>
                                                         <asp:Label ID="Label4" runat="server" Text='<%#Eval("Secs")+ " Sec"  %>' Visible="false"></asp:Label>
                                                         <asp:Label ID="lbl_dur" runat="server" Text='<%#Eval("Duration") %>'></asp:Label>
                                                     </ItemTemplate>
                                                     <EditItemTemplate>
                                                         <asp:TextBox ID="txt_dur_Hr" runat="server" BorderStyle="None" style="width:40px ;border-radius:0px" Text='<%# Eval("Hr")+ " Hr " %>'></asp:TextBox>
                                                         <asp:TextBox ID="txt_dur_Min" runat="server" BorderStyle="None" style="width:40px; margin-left:-3px ; border-radius:0px;" Text='<%#Eval("min") + " Min" %>'></asp:TextBox>
                                                         <asp:TextBox ID="txt_dur_Sec" runat="server" BorderStyle="None" style="width:42px; margin-left:-4px; border-radius:0px;" Text='<%#Eval("Secs") + " Sec"%>'></asp:TextBox>
                                                     </EditItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Event Time">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_City" runat="server" Text='<%#Eval("EventTime") %>'></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                             </Columns>
                                         </asp:GridView>
                                         <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." GridLines="None" OnRowCancelingEdit="GridView3_RowCancelingEdit" OnRowEditing="GridView3_RowEditing" OnRowUpdating="GridView3_RowUpdating" style="margin-left:50px" AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging_3" OnRowDeleting="GridView3_RowDeleting">
                                             <HeaderStyle CssClass="HeaderStyle" />
                                             <RowStyle CssClass="RowStyle" />
                                             <AlternatingRowStyle CssClass="AltRowStyle" />
                                             <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                             <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                             <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                             <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                             <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                             <SortedAscendingHeaderStyle BackColor="#487575" />
                                             <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                             <SortedDescendingHeaderStyle BackColor="#275353" />
                                             <Columns>
                                                 <asp:TemplateField HeaderText="Edit">
                                                     <ItemTemplate>
                                                         <asp:ImageButton ID="Edit" runat="server" AlternateText="Edit" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Edit" Height="20px" ImageUrl="~/Administration/images/user_edit.png" Width="20px" />
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" />
                                                     <EditItemTemplate>
                                                         <asp:Button ID="btn_Update" runat="server" CommandName="Update" Text="Update" />
                                                         <asp:Button ID="btn_Cancel" runat="server" CommandName="Cancel" Text="Reset" />
                                                     </EditItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Delete">
                                                     <ItemTemplate>
                                                         <asp:ImageButton ID="Delete" runat="server" AlternateText="Delete" class="btn btn-blue" CommandArgument='<%# Eval("MeasurementId") %>' CommandName="Delete" Height="20px" ImageUrl="~/Administration/images/Trash.png " Width="20px" OnClientClick="return deleteSystemBeh();"/>
                                                     </ItemTemplate>
                                                  </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="BehaviorId" Visible="false">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_Id" runat="server" Text='<%#Eval("BehaviourId") %>'></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Yes Or No">
                                                     <ItemTemplate>
                                                         <asp:CheckBox ID="chk_YoN" runat="server" Checked='<%# (Eval("YesOrNo").ToString()=="True")? true:false %>' Enabled="false" />
                                                     </ItemTemplate>
                                                     <EditItemTemplate>
                                                         <asp:CheckBox ID="yesOrNo" runat="server" Checked='<%# (Eval("YesOrNo").ToString()=="True")? true:false %>' CssClass="chkStyle" />
                                                     </EditItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Event Time">
                                                     <ItemTemplate>
                                                         <asp:Label ID="lbl_City" runat="server" Text='<%#Eval("EventTime") %>'></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                             </Columns>
                                         </asp:GridView>
                                         
                                                       
                                </div>
                               
                                </td>
                            </tr>
                        </table>


                        <div id="overlay" class="web_dialog_overlay">
                        </div>

                        <div id="dialog" class="web_dialog">
                            <a id="close_x" class="close sprited" href="#">
                                <img src="../Administration/images/clb.PNG" style="border: 0px; margin-top: -25px; margin-right: -25px; float: right; padding: 5px" width="25" alt="" />
                            </a>
                            <div id="sign_up" style="width: 570px;">
                                <h3>Add LessonPlan</h3>

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>

                                        <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left; margin-left: 10px">
                                            <tr>
                                                <td class="auto-style1" colspan="3">
                                                    <asp:Label ID="lbl_msg" runat="server" Text="" ForeColor="Orange"></asp:Label></td>

                                            </tr>
                                            <tr>
                                                <td class="tdText" style="width: 25%; text-align: left">Lesson Plan Name</td>
                                                <td style="width: 5%; text-align: center">&nbsp;</td>
                                                <td style="width: 50%; text-align: left">
                                                    <asp:TextBox ID="txtLP" runat="server" ToolTip="Lesson Plan Name"
                                                        Width="256px" CssClass="textClass" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>

                                                    <asp:Button ID="btn_LPSearch" runat="server" OnClick="btn_LPSearch_Click" CssClass="btn btn-orange" Style="display: inline; float: left" />
                                            </tr>
                                            <tr>
                                                <td class="tdText" style="width: 25%; text-align: left">Lesson Plan</td>
                                                <td style="width: 5%; text-align: center">&nbsp;</td>
                                                <td class="tdText" style="width: 50%; text-align: left">
                                                    <asp:ListBox ID="lstLP" runat="server" SelectionMode="Multiple" Width="300px" Height="200px" CssClass="textClass"
                                                        BorderStyle="Solid" BorderWidth="1px" AutoPostBack="True" OnSelectedIndexChanged="lstLP_SelectedIndexChanged"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdText" style="width: 25%; text-align: left">
                                                    <asp:HiddenField ID="hdnsort" runat="server" />
                                                </td>
                                                <td style="width: 5%; text-align: center">&nbsp;</td>
                                                <td class="tdText" style="width: 50%; text-align: left">&nbsp;</td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>




                                <div style="text-align: right;">
                                </div>

                            </div>
                        </div>





                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
