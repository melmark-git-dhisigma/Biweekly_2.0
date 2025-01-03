<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardReportNew.aspx.cs" Inherits="Graph" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script>
        Array.prototype.includes = null;
        Array.prototype.indexOf = null;
        if (!Array.prototype.includes) {
            Array.prototype.includes = function (search) {
                return !!~this.indexOf(search);
            }
        }
        if (!Array.prototype.indexOf) {
            Array.prototype.indexOf = (function (Object, max, min) {
                "use strict";
                return function indexOf(member, fromIndex) {
                    if (this === null || this === undefined) throw TypeError("Array.prototype.indexOf called on null or undefined");
                    var that = Object(this), Len = that.length >>> 0, i = min(fromIndex | 0, Len);
                    if (i < 0) i = max(0, Len + i); else if (i >= Len) return -1;
                    if (member === void 0) {
                        for (; i !== Len; ++i) if (that[i] === void 0 && i in that) return i;
                    } else if (member !== member) {
                        for (; i !== Len; ++i) if (that[i] !== that[i]) return i;
                    } else for (; i !== Len; ++i) if (that[i] === member) return i;
                    return -1;
                };
            })(Object, Math.max, Math.min);
        }
    </script>

    <script src="js/jquery-2.1.0.js"></script>
     <script src="../Scripts/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Scripts/highcharts/7.1.2/modules/accessibility.js"></script>
    <script src="js/d3.v3.js"></script>
    <script src="js/nv.d3.js"></script>
    <link href="../Administration/CSS/GraphStyle.css" rel="stylesheet" />
    <link href="CSS/nv.d3.css" rel="stylesheet" />

    <style type="text/css">

        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../Administration/images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }
        .innerLoading {
            margin: auto;
            height: 50px;
            width: 250px;
            text-align: center;
            font-weight: bold;
            font-size: large;
        }

            .innerLoading img {
                margin-top: 200px;
                height: 10px;
                width: 100px;
            }

        .nv-controlsWrap {
            visibility: hidden;
        }

        .nv-legendWrap {
            visibility: hidden;
        }

        .nvtooltip {
            left: 100px !important;
            position: fixed !important;
        }

        .NFButton {            
            /*background-color: #00549f;*/
            width: 105px;
            height: 50px;
            color: #fff;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            word-wrap:break-word;
            background-position: 0 0;
            margin-left:10px;
            border: 10px;
            cursor: pointer;
        }

        .divchkbox label {
          vertical-align: text-bottom;
        }
    </style>


    <script type="text/javascript">

        window.onload = function () {


            var d = new Date();
            var strDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            $('#lblDates').html(strDate);
            $('#lblDate').html(strDate);

        };
        function loadWait() {
            $('.loading').fadeIn('fast');//, function () { });
        }
        function HideWait() {
            $('.loading').fadeOut('fast');//, function () { });
        }
        
        $(function () {

            //drawGraph();
            //drawGraphs();
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                $(".Head").attr("style", "padding-left: 25%")
            }

            $(".nv-controlsWrap").hide();

        });


        function setSGraph() {
            $('.mainDivS').slideDown();
            $('.mainDivU').slideUp();

            $('#liBeh').html("<div class='session1'></div> Behavior Count ");
            $('#liLess').html(" <div class='session2'></div> Lesson Plans % ");

        }
        function setUGraph() {

            var d = new Date();
            var strDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();


            $('.mainDivS').slideUp();
            $('.mainDivU').slideDown();
            $(".mainDivU").attr("style", "visibility: visible");
            //   $('.mainDivU').attr("visibility", "visible");
            $('#divHeading').html("<font color='#1F77B4'>Number of Teaching Sessions  on " + strDate + " </font>");

            $('#liBehav').html("<div class='session1'></div> Behavior Count ");
            $('#liLessons').html("<div class='session2'></div>Lesson Plan Count ");
        }

        $(document).ready(function () {
            $('#rbtnClassType').change(function () {
                var selected_value = $('#<%=rbtnClassType.ClientID %> input:checked').val();
                //alert('Radio Box has been changed! ' + selected_value);
                $('#<%=Txt_clstype.ClientID %>').val(selected_value);
                $('#btn_clstype_Click').click();
            });
        });

        function onSelectionChange() {
            var selectedVals = '';
            $.each($('#ddllanguage input[type=checkbox]:checked'), function () {
                selectedVals += $("label[for='" + this.id + "']").html() + ",";
            });
            $('#txtValue').val(selectedVals);
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="180000"> </asp:ScriptManager>
        <div class="mainDivS">
            <div class="Head" style="float: none; font-size: 35px; text-align:center; font-family:Tahoma; color: #00549f;margin-top:-45px">
                <h3 style="font-family:Tahoma">Dashboard - Today's <!--(<label id="lblDate"></label>)--> Progress</h3>
            </div>
            <div style="width:100%">
                <div class="divdrop" style="margin-top:-30px">
                    <table>
                        <tr>
                            <td style="text-align:center;width:25%">
                                <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <span style="text-align:right;font-weight:bolder;font-size:15px;color: #00549f;padding-right: 50px;padding-left: 45px;">Choose Clients:</span>
                                    <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Location(s) :</span> 
		                            <asp:DropDownCheckBoxes ID="ddcb_clas" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="true" UseSelectAllNode="false" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="ddcb_clas_SelectedIndexChanged">
			                            <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="400" DropDownBoxCssClass="ddchkLesson"/>
			                            <Texts SelectBoxCaption="Select Location"/>
		                            </asp:DropDownCheckBoxes>		      
		                            <span style="text-align:right;font-weight:bolder;font-size:12px;padding-left:15px;margin-right:15px;color: #00549f;">or</span>
                                    <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Client Name(s) :</span> 
		                            <asp:DropDownCheckBoxes ID="ddcb_stud" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="true" UseSelectAllNode="false" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="ddcb_stud_SelectedIndexChanged" OnDataBound="ddcb_stud_DataBound">
			                            <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="400" DropDownBoxCssClass="ddchkLesson"/>
			                            <Texts SelectBoxCaption="Select Client"/>
		                            </asp:DropDownCheckBoxes> 
                                </ContentTemplate>
                                </asp:UpdatePanel>  
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" BorderColor="#00549f" BorderStyle="None" BorderWidth="1px" style="margin-left:-90px; vertical-align: bottom; display: inline-table" RepeatLayout="Flow" OnSelectedIndexChanged="rbtnClassType_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="DAY" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Day</asp:ListItem>
                                    <asp:ListItem Value="RES" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Residence</asp:ListItem>                                
                                    <asp:ListItem Value="BOTH" Selected="True" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Both</asp:ListItem>                                
                                </asp:RadioButtonList>    
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divbtns" style="padding-top:60px;padding-right: 140px;margin-top:-50px">
                    <table>
                        <tr>
                            <td colspan="3" style="text-align:center;padding-left: 200px;width:15%">
                                 <span style="text-align:center;font-weight:bolder;font-size:15px;color: #00549f;margin-left:-150px">Choose Reports:</span>
                            </td>
                            <td colspan="3" style="text-align:left;width: 20%;">
                                 <asp:button id="BtnClientAcademic" runat="server" text="" cssclass="NFButton" tooltip="Academic by Client" OnClick="BtnClientAcademic_Click" BackColor="#00549F"  />
                                 <asp:button id="BtnStaffAcademic" runat="server" text="" cssclass="NFButton" tooltip="Academic by Staff" OnClick="BtnStaffAcademic_Click" BackColor="#00549F"   />
                                 <asp:button id="BtnClientClinical" runat="server" text="" cssclass="NFButton" tooltip="Clinical by Client" OnClick="BtnClientClinical_Click" BackColor="#00549F"   />
                                 <asp:Button id="BtnStaffClinical" runat="server" text="" cssclass="NFButton" style="width:102px" tooltip="Clinical by Staff" OnClick="BtnStaffClinical_Click" BackColor="#00549F"  />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkbx_Mistrial" runat="server" style=" color: #00549f;font-size:12px" Checked="True" OnCheckedChanged="chkbx_Mistrial_CheckedChanged"/>
                                <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;">Count Mistrial</span>
                            </td>
                            <td>
                                        <asp:CheckBox id="highcheck" runat="server"  Text=""></asp:CheckBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divchkbox" style="padding-top:20px;margin-top:-30px">
                    <table>
                        <tr>
                            <td>
                                 <asp:button id="BtnRefresh" runat="server" text="Refresh" cssclass="NFButton" style="background-color:#34b233" tooltip="Refresh the Dashboard" OnClick="BtnRefresh_Click"      />
                            </td>
                            <td style="text-align:center;width:70%">
                                <div id="chkbox-selection" style="padding-top:10px">
                                <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;">Show progress as: </span>
                                <asp:CheckBox ID="chkbx_leson_deliverd" runat="server" Text="total lessons delivered today or" style=" color: #00549f;font-size:12px" AutoPostBack="True" Checked="True" Enabled="False" OnCheckedChanged="chkbx_leson_deliverd_CheckedChanged"/>
                                <asp:CheckBox ID="chkbx_block_sch" runat="server" Text="percentage of block-scheduled lessons" style=" color: #00549f;font-size:12px" AutoPostBack="True" OnCheckedChanged="chkbx_block_sch_CheckedChanged"/>
                                </div>
                                <div id="selection-label" style="margin-top:0px;max-width:1100px;word-wrap: break-word;" >
                                <p><span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom">
                                    <asp:Label ID="Label1_CrntSelctn" runat="server" style="font-weight:100">Current Selections:</asp:Label>
                                    <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;font-weight:bold;padding-left:10px">Location :
                                    <asp:Label ID="Label_location" runat="server" style="font-weight:100"></asp:Label>
                                    <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;font-weight:bold;padding-left:10px">Client :
                                    <asp:Label ID="Label_Client" runat="server" style="font-weight:100"></asp:Label>
                                    </span></p>
                                 </div>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                
                                </td><td id="tdMsg" runat="server" style="width: 97%;font-size: 15px;color:green;text-align:center;font-weight:bold"></td>
                            <td style="text-align:center;width:70%">
                                <div id="valdisplay" style="visibility: visible;display:none;">
                                <asp:TextBox ID="Txt_StudSelcted" runat="server" BackColor="#FF9966" Width="70px"></asp:TextBox>
                                <asp:TextBox ID="Txt_All" runat="server" ReadOnly="True" Width="50px" BackColor="#77FF66">0</asp:TextBox>
                                <asp:TextBox ID="Txt_Clasid" runat="server" ReadOnly="True" Width="218px" BackColor="#99FF66"></asp:TextBox>
                                <asp:TextBox ID="Txt_Studid" runat="server" ReadOnly="True" Width="249px" BackColor="#00CCFF"></asp:TextBox>
                                <asp:TextBox ID="Txt_Userid" runat="server" ReadOnly="True" Width="204px" BackColor="#99FF66"></asp:TextBox>
                                <asp:TextBox ID="Txt_clstype" runat="server" BackColor="#FF9966" Width="70px">BOTH</asp:TextBox>
                                <asp:TextBox ID="Txt_graphid" runat="server" BackColor="#CC9966" Width="70px"></asp:TextBox>
                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <!--olddashboard-view--start-->
                <div id="olddashboard-view" style="display:none">
                     <!--<div class="Head" style="position: absolute; width: 300px; height: 30px; left: 40%; top: 28%;">-->
                        <a class="student" href="#" onclick="setSGraph();">Teaching Progress</a>
                        <a class="user" href="#" onclick="setUGraph();">Teaching Sessions</a>
                    <!--</div>-->
                </div>
                <!--olddashboard-view--end-->
            </div>
            <hr style="color:#1f497d;font-size:smaller;margin-top:-20px"/>
          
            <div class="main-dashboard" >
                 <div>
                    <table style="width: 100%">   
                        <tr>
                            <td style="text-align: center">
                                 <div id="prdiv" style="margin-left:-50px; position:absolute; height: 100%"">
                                    <rsweb:ReportViewer ID="RV_DBReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowToolBar="false" ShowWaitControlCancelLink="true"  Width="100%"  Height="100%" Visible="False" ShowParameterPrompts="False">
                                        <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />
                                    </rsweb:ReportViewer>
                                 </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <!--olddashboard-student--start-->
        <div id="olddashboard-student" style="visibility: hidden;display:none"> 
            <div class="headings" style="width:100%;float:left;">
                <ul style="float: right">
                    <li id="liBeh">
                        <div class="session1"></div>
                        Behavior Count
                    </li>
                    <li id="liLess">
                        <div class="session2"></div>
                        Lesson Plans %  
                    </li>
                </ul>
            </div>

            <div style="width: 45%;display:inline-block; float:left" id="Student" runat="server">
                <svg></svg>
            </div>
            <div  style="width: 45%; display:inline-block; float:left;margin-left:1%" id="StudentBehv" runat="server">
                <svg></svg>
            </div>
        </div> 
        <!--olddashboard-student--end-->

        <!--olddashboard-teacher--start-->
        <div id="olddashboard-teacher" style="visibility: hidden;display:none"> 
            <div class="mainDivU" style="visibility: hidden;">
                <div id="divHeading" class="Head" style="float: none; font-size: 29px; margin-left: 35%; color: #1F77B4;">
                    User Reports on
                    <label id="lblDates"></label>
                </div>
                <div class="headings">

                    <ul style="float: right">
                        <li id="liBehav">Behavior<div class="session1"></div>
                        </li>
                        <li id="liLessons">Lesson Plans<div class="session2"></div>
                        </li>
                    </ul>
                </div>

                <div style="width: 99%;" id="Teacher" runat="server">
                    <svg></svg>
                </div>

            </div>
        </div> 
        <div>
             <asp:Label ID="lblNoData" runat="server" Text=""></asp:Label>
     <div id = "graphcontainer" style = "width: 1116.899603148px; height: 578.268px; margin: 0 auto" runat="server"></div>
        </div>
        <!--olddashboard-teacher--end-->
        <script>
            function loadClinicbyClient(classid, studentid) {
    var jsonData = JSON.stringify({ cid: classid, sid: studentid });
    $.ajax({
        type: "POST",
        url: "DashboardReportNew.aspx/getClientClinic",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function (response) {
            var parsed = JSON.parse(response.d);
            if (parsed.length != 0) {
                var aData = parsed;
                var behav = [];
                var categoriesdata = [];
                var seriesdata = [];
                var result = [];
                $.map(aData, function (item, index) {
                    categoriesdata.push(item['StudentName']);
                });
                $.map(categoriesdata, function (item, index) {
                    if (!result.includes(item)) {
                        result.push(item);
                    }

                });
                var len = result.length;
                $.map(aData, function (item, index) {
                    if (result.includes(item['StudentName'])) {
                        var index;
                        result.some(function (elem, inx) {
                            if (elem === item['StudentName']) {
                                index = inx;
                            }
                        });
                        var dt = [];
                        var i = 0;
                        while (i < len) {
                            if (i == index) {

                                dt.push({ y: item['BehaviourSession'], name: item['BehaviourName'] });
                            }
                            else {
                                dt.push({ y: 0, name: '' });
                            }


                            i = i + 1;

                        }

                        var daa = JSON.parse(JSON.stringify(dt));
                        var seriesdict = new Object;
                        var ser = new Object;
                        seriesdict['name'] = item['BehaviourName'];
                        seriesdict['data'] = daa;
                        seriesdata.push(seriesdict);
                        behav.push(item['BehaviorNameToolTip']);

                        while (dt.length > 0) {
                            dt.pop();
                        }
                    }


                });
                var cat = JSON.parse(JSON.stringify(result));
                var dat = JSON.parse(JSON.stringify(seriesdata));
                var beh = JSON.parse(JSON.stringify(behav));
                var titletext='Clinical by client';
                drawChartClinic(cat, dat, beh,titletext);
                HideWait();
            }
            else {
                HideWait();
                var nodata = document.getElementById('lblNoData');
                nodata.innerHTML = "No Data Available";
            }
        },
        error: OnErrorCall_
    });

    function OnErrorCall_(response) {
        alert("Whoops something went wrong!");

    }
}
            function loadAcbyClient(classid, studentid, mistrial) {
                var jsonData = JSON.stringify({ classid: classid, studentid: studentid, mistrial: mistrial });
                $.ajax({
                    type: "POST",
                    url: "DashboardReportNew.aspx/getAClient",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var parsed = JSON.parse(response.d);
                        if (parsed.length != 0) {
                            var aData = parsed;
                            var categoriesdata = [];
                            var seriesdata = [];
                            var result = [];
                            $.map(aData, function (item, index) {
                                categoriesdata.push(item['StudentName']);
                            });
                            $.map(categoriesdata, function (item, index) {
                                if (!result.includes(item)) {
                                    result.push(item);
                                }
                            });
                            var len = result.length;
                            $.map(aData, function (item, index) {
                                if (result.includes(item['StudentName'])) {
                                    var index;
                                    result.some(function (elem, inx) {
                                        if (elem === item['StudentName']) {
                                            index = inx;
                                        }
                                    });
                                    var dt = [];
                                    var i = 0;
                                    while (i < len) {
                                        if (i == index) {

                                            dt.push({ y: item['SessionCount'], name: item['LessonName'] });
                                        }
                                        else {
                                            dt.push({ y: 0, name: '' });
                                        }
                                        i = i + 1;
                                    }
                                    var daa = JSON.parse(JSON.stringify(dt));
                                    var seriesdict = new Object;
                                    seriesdict['name'] = item['LessonName'];
                                    seriesdict['data'] = daa;
                                    seriesdata.push(seriesdict);
                                    while (dt.length > 0) {
                                        dt.pop();
                                    }
                                }
                            });
                            var cat = JSON.parse(JSON.stringify(result));
                            var dat = JSON.parse(JSON.stringify(seriesdata));
                            drawChartAc(cat, dat);
                            HideWait();
                        }
                        else {
                            var nodata = document.getElementById('lblNoData');
                            nodata.innerHTML = "No Data Available";
                            HideWait();
                        }
                        },

                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {
                    alert("Whoops something went wrong!");

                }
            }
            function drawChartClinic(cat, da, be, titletext) {
                var intrvl = 1;
                var maxy = null;
                var ytext = '';
                if (titletext != 'Clinical by client') {
                    intrvl = 10;
                    maxy = 100;
                }
                else {
                    ytext = 'Total Behavior Count';
                }
                Highcharts.chart('graphcontainer', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: titletext,
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        title: {
                            text: 'Client Name',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },
                    yAxis: {
                        min: 0,
                        max: maxy,
                        tickInterval: intrvl,
                        opposite:true,
                        title: {
                            text: ytext,
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },

                    legend: {
                        reversed: true,
                        enabled: false,
                    },
                    plotOptions: {
                        series: {
                            stacking: 'normal'
                        },
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    if (titletext != 'Clinical by client') {
                                        return this.point.y + "%";
                                    } else {
                                        return this.point.name;
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                    },
                    tooltip: {
                        formatter: function () {
                            if (titletext != 'Clinical by client') {
                                return this.point.ToolTip;
                            }
                            else {
                                var serieI = this.series.index;
                                return be[serieI];
                            }
                        }

                    },

                    series: da

                });

            }
            function drawChartAc(cat, da) {

                Highcharts.chart('graphcontainer', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: 'Academic by client'
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        //   categories:['a','b'],
                        title: {
                            text: 'Client Name',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },
                    yAxis: {
                        min: 0,
                        tickInterval: 1,
                        opposite:true,
                        title: {
                            text: 'Total Sessions',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },
                    legend: {
                        reversed: true,
                        enabled: false,
                    },
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    return this.point.name;
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                        series: {
                            stacking: 'normal'
                        }

                    },
                    series: da
                });

            }
            function loadAcbyClientPerc(classid, studentid) {
                var jsonData = JSON.stringify({ classid: classid, studentid: studentid });
                $.ajax({
                    type: "POST",
                    url: "DashboardReportNew.aspx/getAClientPerc",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var parsed = JSON.parse(response.d);
                        if (parsed.length != 0) {
                            var aData = parsed;
                            var categoriesdata = [];
                            var seriesdata = [];
                            var result = [];
                            tooltiparr = [];
                            $.map(aData, function (item, index) {
                                categoriesdata.push(item['StudentName']);
                            });
                            $.map(categoriesdata, function (item, index) {
                                if (!result.includes(item)) {
                                    result.push(item);
                                }
                            });
                            var len = result.length;
                            var percData = []; var avgscore = [];
                            result.forEach(function (item) {
                                var score = 0, count = 0;
                                var tooltip = '';
                                aData.forEach(function (it) {
                                    if (item == it.StudentName) {
                                        score = score + it.Percentage;
                                        count = count + 1;
                                        tooltip=it.LessonNameToolTip;
                                    }
                                });
                                var ydict = new Object;
                                ydict['y'] = score / count;
                                ydict['ToolTip'] = tooltip;
                                avgscore.push(ydict);
                            });
                            var seriesdict = new Object;
                            seriesdict['name'] = "Percentage";
                            seriesdict['data'] = avgscore;
                            seriesdict['colorByPoint'] = true;
                            percData.push(seriesdict);
                            var cat = JSON.parse(JSON.stringify(result));
                            var dat = JSON.parse(JSON.stringify(percData));
                            var titletext='Percentage of Block Scheduled Lessons';
                            drawChartClinic(cat, dat, tooltiparr,titletext);
                            HideWait();
                        }
                        else {
                            var nodata = document.getElementById('lblNoData');
                            nodata.innerHTML = "No Data Available";
                            HideWait();
                        }
                    },

                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {
                    alert("Whoops something went wrong!");

                }
            
            }


            function loadAcademicbyStaff(acstaffdata)
            {
                var aData = JSON.parse(acstaffdata);
                if (aData.length != 0) {
                    aData = aData.reverse();
                    var categoriesdata = [];
                    var seriesdata = [];
                    var result = [];
                    var lessonname = []; var uniqueless = [];
                    $.map(aData, function (item, index) {
                        categoriesdata.push(item['StaffName']);
                    });
                    $.map(categoriesdata, function (item, index) {
                        if (!result.includes(item)) {
                            result.push(item);
                        }
                    });
                    $.map(aData, function (item, index) {
                        lessonname.push(item['LessonNameToolTip']);
                    });
                    $.map(lessonname, function (item, index) {
                        if (!uniqueless.includes(item)) {
                            uniqueless.push(item);
                        }
                    });
                    var colorDictionary = createDictionary(uniqueless);
                    var len = result.length;
                    $.map(aData, function (item, index) {
                        if (result.includes(item['StaffName'])) {
                            var index;
                            result.some(function (elem, inx) {
                                if (elem === item['StaffName']) {
                                    index = inx;
                                }
                            });
                            var dt = [];
                            var i = 0;
                            while (i < len) {
                                if (i == index) {
                                    var key = item['LessonNameToolTip'];
                                    var color = colorDictionary[key];
                                    dt.push({ y: item['SessionCount'], name: item['LessonName'], ToolTip: item['LessonNameToolTip'] + '(' + item['StudentName'] + ')', color: color });
                                }
                                else {
                                    dt.push({ y: 0, name: '' });
                                }
                                i = i + 1;
                            }
                            var daa = JSON.parse(JSON.stringify(dt));
                            var seriesdict = new Object;
                            seriesdict['name'] = item['LessonName'];
                            seriesdict['data'] = daa;
                            seriesdata.push(seriesdict);
                            while (dt.length > 0) {
                                dt.pop();
                            }
                        }
                    });
                    var cat = JSON.parse(JSON.stringify(result));
                    var dat = JSON.parse(JSON.stringify(seriesdata));
                    DrawAcStaff(cat, dat);
                    HideWait();
                }
                else {
                    HideWait();
                    var nodata = document.getElementById('lblNoData');
                    nodata.innerHTML = "No Data Available";
                }
            }
            function DrawAcStaff(cat, da) {

                Highcharts.chart('graphcontainer', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: 'Academic by Staff'
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        title: {
                            text: 'Staff Name',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },
                    yAxis: {
                        min: 0,
                        tickInterval: 1,
                        opposite: true,
                        title: {
                            text: 'Total Sessions',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },
                    legend: {
                        reversed: true,
                        enabled: false,
                    },
                    tooltip: {
                        formatter: function () {
                            return this.point.ToolTip;
                        }
                    },
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    return this.point.name;
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                        series: {
                            stacking: 'normal'
                        }

                    },
                    series: da
                });

            }
            function createDictionary(uniqueless) {
                var dictionary = {};
                for (var i = 0; i < uniqueless.length; i++) {
                    var item = uniqueless[i];
                    dictionary[item] = generateRandomColor();
                }

                return dictionary;
            }
            function generateRandomColor() {
                var randomColor = '#' + Math.floor(Math.random() * 16777215).toString(16);
                return randomColor;
            }
        </script>
    </form>
</body>
</html>
