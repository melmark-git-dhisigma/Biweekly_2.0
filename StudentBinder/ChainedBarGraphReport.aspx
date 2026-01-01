<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChainedBarGraphReport.aspx.cs" Inherits="StudentBinder_ChainedBarGraphReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js"></script>
      <script src="../Scripts/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Scripts/highcharts/7.1.2/modules/accessibility.js"></script>
     <script src="../Scripts/highcharts/7.1.2/modules/xrange.js"></script>
<%--    <script src="../Scripts/highcharts/7.1.2/modules/exporting.js"></script>--%>
        <script src="../Scripts/html2canvas.min.js"></script>
            <script src="../Scripts/es6-promise.auto.min.js"></script>
        <script src="../Scripts/es6-promise.min.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />


     <style type="text/css">
        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
        }

        .divGrid1 {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
            display: none;
        }

        .divBackgrnd {
            padding: 26px 16px 16px 16px;
            width: 90%;
            height: 400px;
            overflow-y: scroll;
            overflow-x: hidden;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: rgba(87,197,239,0.2);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .pnlCSS {
        }
        /* FOR LOADING IMAGE AT PAGE LOAD */
        .loading {
            display: block;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../Administration/images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

         #fullContents {
            display: none;
        }
           #prdiv td {
             height:auto !important;
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
            background: url("../Administration/images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            position: fixed;
            width: 1040px;
            height: 350px;
            overflow: auto;
            top: 23%;
            left: 30%;
            margin-left: -190px;
            margin-top: -275px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 102;
            background: #f8f7fc url(../images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

         .web_dialog13 {
            background: url("../images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            border: 5px solid #B2CCCA;
            color: #333333;
            display: none;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            height: auto;
            left: 30%;
            top: 5%;
            margin-left: -190px;
            padding: 5px 5px 30px;
            position: fixed;         
            width: 800px;
            z-index: 102;
        }


        /*LOADING IMAGE CLOSE */
    </style>
    <script type="text/javascript">

        //$(function () {
        //    adjustStyle();

        //});

        //function adjustStyle() {
        //    var isiPad = navigator.userAgent.match(/iPad/i);
        //    if (isiPad != null) {
        //        $('graphPopup').css('width', '91% !Important');
        //        $('graphPopup').css('left', ' 220px !Important');
        //    }

        //}

        function HideAndDisplay() {
            window.parent.PopupLessonPlans('ChainGraphTab');
        }


        function loadWait() {
            $('.loading').fadeIn('fast');//, function () { });
        }
        function HideWait() {
            $('.loading').fadeOut('fast');//, function () { });
        }
        function HideWait2() {
            $('.loading').fadeOut('fast');//, function () { });
            if (document.getElementById('highcheck').checked) {
                CloseDownload();
            }
        }
        $(document).ready(function () {

            $('.loading').fadeOut('slow', function () {
                $('#fullContents').fadeIn('fast');
            });
          
        });


    </script>

    <script type="text/javascript">

        window.onload = function () {
            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });


            
            $('#<%=btnExport.ClientID %>').click(function () {
            $('.loading').fadeIn('fast');
            });

        };
        function DownloadPopup() {
            $('.loading').fadeOut('fast');
            $('#overlay').fadeIn('fast', function () {
                $('#downloadPopup').fadeIn('fast');
            });
        }

        function CloseDownload() {
            document.getElementById("hdnExport").value = "";
            $('#overlay').fadeOut('fast', function () {
                $('#downloadPopup').fadeOut('fast');
            });
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
        <asp:HiddenField ID="hfPopUpValue" runat="server" Value=""/>
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="fullContents" style="width:98%;margin-left:1%">
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="180000">
                            </asp:ScriptManager>
          
        </div>
      <div id="tdMsg" runat="server">
          
      </div>
       <div id="overlay" class="web_dialog_overlay">
        </div>
        <div id="graphPopup"  style="width: 73%">
            
                       
                <table style="width: 100%">
                    
                     <tr>
                         <td style="text-align: center;width:20%" class="tdText">Report Start Date</td>
                          <td style="text-align: left">
                            <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                         <td  >
                        <div style="float:left">
                          <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="Dotted" BorderWidth="1px" >
                            <asp:ListItem Value="Day">Day</asp:ListItem>
                            <asp:ListItem Value="Residence">Residence</asp:ListItem>
                            <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:CheckBox id="highcheck" runat="server" Text=""></asp:CheckBox>

                            </div>
                         </td>
                        <td style="text-align: left; margin-left: 1880px" rowspan="2">
                            <asp:Button ID="btnsubmit" runat="server"  Text="" ToolTip="Show Graph" style="float:left;margin:0 0 0 0" CssClass="showgraph" OnClick="btnsubmit_Click" OnClientClick="javascript:loadWait();" />
                         <input type="button" id="btnRefresh" style="float:left;margin:0 0 0 0" class="refresh" onclick="HideAndDisplay()" />
                            <asp:Button ID="btnExport" runat="server" style="float:left;margin:0 0 0 0" ToolTip="Export To PDF"  CssClass="pdfPrint" OnClick="btnExport_Click"  />
             

                        
                        </td>
                         
                         </tr>
                    <tr>
                      
                       
                        <td style="text-align: center" class="tdText">Report End Date</td>
                        <td style="text-align: left" rowspan="1">
                            <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                         <td>  <div style="float:left">
                        <asp:RadioButtonList ID="RbtnPloatType" runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="Dotted" BorderWidth="1px" >                            
                            <asp:ListItem Value="Current" Selected="True">Current Prompts</asp:ListItem>
                            <asp:ListItem Value="Step" >Step Prompt</asp:ListItem>
                        </asp:RadioButtonList>
                         </div></td>
                        
                    </tr>
                   
                  
                  
                  
                </table>
          
        </div>

        
            <div>
                <table style="width: 100%">
                    
                    <tr>
                        
                        <td style="text-align: center">
                             <div id="prdiv">
                            <rsweb:ReportViewer ID="RV_LPReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="false" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false"  SizeToReportContent="true" Width="100%"  Visible="false" AsyncRendering="true" co>

                                    <ServerReport     ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                              <%--    ReportPath="<%$ appSettings:ReportPath %>"--%>

                                </rsweb:ReportViewer>
                                 </div>
                        </td>
                    </tr>
                </table>

            </div>
        <div id="downloadPopup" class="web_dialog13" style="width: 600px;">

            <div id="Div53" style="width: 700px;">


                <table style="width: 97%">
                    <tr>
                        <td colspan="2">
                            <table style="width: 85%">
                                <tr>
                                    <td runat="server" id="tdMsgExport" style="height: 50px"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="HideWait2();" />

                        </td>
                        <td style="text-align: left">
                            <%-- <input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />--%>
                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClientClick="CloseDownload();" onClick="btnDone_Click"/>

                        </td>
                    </tr>
                </table>

            </div>
        </div>
        <div id="HighchartGraph">
            <center>
            <asp:label id="sname" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                <br />
            <asp:label id="lnam" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                <br />
            <asp:label id="daterang" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                <br />
                </center>
            <br />
            <center>
              <asp:Label ID="trtmnt" runat="server" Text="" Font-Bold="true" visibility="false" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:Label>
            <asp:label runat="server" text="" id="lbgraph"  Font-Bold="true" font-size="15px"></asp:label>
                <span id="lbgraphSpan"></span>
                <br />
            <div id = "cont" runat="server"  style = "width: 931.84809449pt; height: 362.23228346pt; margin: 0 auto"></div>
            <br />
        </center>
           <asp:label id="deftxt" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:150px;"  Font-Bold="true"></asp:label>
                <br />
       <asp:label id="mel" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:1200px;" Font-Bold="true"></asp:label>
                </div>
                </div>
        </div>
       <asp:hiddenfield id="hdnExport" runat="server" value="" />
        <script>

            if (document.getElementById('highcheck').checked) {
                var treatment = '';
                // set cont size start
                var myDiv = document.getElementById('cont');
                if ("<%=reptype%>" == "true" || "<%=reptype%>" == "True") {
                    myDiv.style.width = '931.84809449pt';
                    myDiv.style.height = '434.678740152pt';
                } else {
                    myDiv.style.width = '931.84809449pt';
                    myDiv.style.height = '434.678740152pt';
                }
                var inputsDate = "<%=sdate%>";
                inputsDate = inputsDate.replace(/-/g, '/');
                var inputeDate = "<%=edate%>";
                        inputeDate = inputeDate.replace(/-/g, '/');
                // set cont size End
                var sdate = "<%=sdate%>";
                var edate = "<%=edate%>";
                var sid = "<%=sid%>";
                var lid = "<%=lid%>";
                var scid = "<%=scid%>";
                var tempid = "<%=tempid%>";
                var prompt = "<%=prompttype%>";
                var cls = "<%=classtype%>";
                var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, AllLesson: lid, SchoolId: scid, Templateid: tempid, PromptType: prompt,Clstype: cls });
                $.ajax({
                    type: "POST",
                    url: "ChainedBarGraphReport.aspx/getChainedBarReport",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var res = JSON.parse(response.d);
                        if (res.length === 0) {
                            var span = document.getElementById('lbgraphSpan');
                            span.innerHTML = "No Data Available";
                            $('#sname').css("display", "block");
                            $('#trtmnt').css("display", "block");
                            $('#cont').css("display", "none");
                            $('#lnam').css("display", "block");
                            $('#daterang').css("display", "block");
                            $('#mel').css("display", "block");
                            $('#deftxt').css("display", "block");
                            document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                            $.ajax({
                                type: "POST",
                                url: "ChainedBarGraphReport.aspx/getNodataavail",
                                data: JSON.stringify({ lplan: lid, studid: sid }),
                                contentType: "application/json; charset=utf-8",
                                async: false,
                                dataType: "json",
                                success: function (resp) {
                                    var resu = JSON.parse(resp.d);
                                    $.map(resu, function (items, index) {
                                        var trtmnt = document.getElementById('trtmnt');
                                        trtmnt.style.fontSize = '12px';
                                        if (items['Treatment'] != null) {
                                            document.getElementById('trtmnt').innerHTML = items['Treatment'];
                                        } else {
                                            document.getElementById('trtmnt').innerHTML = 'Tx: ';
                                        }
                                        document.getElementById('lnam').innerHTML = items['LessonPlanName'];
                                        var bname = document.getElementById('lnam');
                                        bname.style.fontSize = '12px';

                                        var deftxt = document.getElementById('deftxt');
                                        deftxt.style.fontSize = '12px';
                                        if (items['Deftn'] != null) {
                                            document.getElementById('deftxt').innerHTML = 'Definition:    ' + items['Deftn'];

                        }
                        else {
                                            document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                        }
                                        document.getElementById('sname').innerHTML = items['StudentName'];
                                        var studname = document.getElementById('sname');
                                        studname.style.fontSize = '12px';
                                    });

                                },
                                error: function (xhr, status, error) {

                                    console.error("AJAX request failed:", error);
                                }
                            });
                            if (scid == 1) {
                                document.getElementById('mel').innerHTML = 'Melmark New England';
                                document.getElementById('mel').style.fontSize = '12px';
                            }
                            else {
                                document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                document.getElementById('mel').style.fontSize = '12px';
                            }
                            var drange = document.getElementById('daterang');
                            drange.style.fontSize = '12px';

                        }
                        else {
                            $('#trtmnt').css("display", "none");
                            $.map(res, function (item, index) {
                                document.getElementById('sname').innerHTML = res[0].StudentName;
                                document.getElementById('lnam').innerHTML = res[0].LessonPlanName;
                                document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                            document.getElementById('mel').innerHTML = 'Melmark New England';
                            if (res[0].Deftn != null) {
                                document.getElementById('deftxt').innerHTML = 'Definition:    ' + res[0].Deftn;
                            }
                            else {
                                document.getElementById('deftxt').innerHTML = 'Definition:    ';
                            }
                            treatment = res[0].Treatment + '<br><br>';
                            });
                            var yAxisname = res[0].ChainType;
                            if (yAxisname == "Forward chain")
                            {
                                yAxisname = "Forward chain ->";
                            }
                            else if (yAxisname = "Total Task") {
                                yAxisname = "Total Task"
                            }
                            else {
                                yAxisname = "<-Backward chain ";
                            }
                            var medicData = [];
                            $.map(res, function (item, index) {
                                if (item['StepName'] != null) {
                                    var meddata = {};
                                    meddata['name'] = item['ShortName'];
                                    meddata['x'] = item['SessNbr'];
                                    meddata['x2'] = item['SessNbr'] + 1;
                                    meddata['y'] = item['StepName'];
                                    meddata['tooltp'] = item['Prompt'];
                                    meddata['color'] = item['Color'];
                                    medicData.push(meddata);
                                }
                            });
                            var plotdata = [];
                            $.map(res, function (item, index) {
                                 var wid;
                                if (item['EventName'] != null) {
                                    var plotdict = new Object;
                                    plotdict['color'] = "black";
                                    plotdict['dashStyle'] = 'solid';
                                    plotdict['value'] = (item['Offset']);
                                    plotdict['width'] = 2;
                                    if (item['EventName'] == null) {
                                        item['EventName'] = "";
                                    }
                                    var event = item['EventName'];
                                    var bgcolor;
                                    if (item['CurrentPrompt'] == null) {
                                        bgcolor = 'black';
                                    }
                                    else {
                                        bgcolor = 'gray';
                                    }
                                    plotdict['label'] = {
                                        text: event,
                                        size: 10,
                                        align: 'top',
                                        textAlign: 'right',
                                        rotation: 0,
                                        useHTML: true,
                                        style: {
                                            fontSize: '12px',
                                            color: 'white',
                                            backgroundColor: bgcolor,
                                        },
                                    };
                                    plotdict['label']['overflow'] = 'justify';
                                    plotdict['label']['crop'] = false;
                                    plotdata.push(plotdict);
                                }
                            });
                            var sessarr = [];
                            var ymax = [];
                            $.map(res, function (item, index) {
                                if (item['SessNbr'] != null)
                                {
                                    sessarr.push(item['SessNbr']);
                                }
                                if (item['StepName'] != null) {
                                    ymax.push(item['StepName']);
                                }
                            });
                            minx = findMinimum(sessarr);
                            maxx = findMaximum(sessarr);
                            maxy = findMaximum(ymax);
                            DrawChart(treatment, medicData, yAxisname, plotdata, minx, maxx, maxy);
                        }
                    },
                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {

                    alert("something went wrong!");
                }
            }
            function DrawChart(treatment, medicData, yAxisname, plotdata, minx, maxx, maxy) {
                Highcharts.chart('cont', {
                    exporting: {
                        enabled: false
                    },
                    chart: {
                        type: 'xrange',
                        marginTop: 100,
                    },
                    title: {
                        text: ''
                    },
                    subtitle: {
                        text: treatment,
                        align: 'left',
                        useHTML: true,
                        style: {
                            fontWeight: 'bold',
                            color: 'black',
                            fontSize: '12px',
                            fontFamily: 'Arial'
                        }
                    },
                    xAxis: {
                        plotLines: plotdata,
                        title: {
                            text: 'Number of Sessions Run',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'white',
                                fontSize: '12px',
                                fontFamily: 'Arial'
                            }
                        },
                        tickInterval: 1,
                        min: minx,
                        max: maxx + 1,
                        labels: {
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    yAxis: {
                        gridLineWidth: 0,
                        title: {
                            text: yAxisname,
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
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                        max:maxy,
                        tickInterval: 1,
                        reversed: false
                    },
                    tooltip: {
                        formatter: function () {
                            return this.point.tooltp;

                        }
                    },
                    series: [{
                        data: medicData,
                        color:'black',
                        showInLegend: false,
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
                        },

                    }]
                });
            }
            function findMinimum(arr) {
                var minNum = arr[0];
    
                for (var i = 1; i < arr.length; i++) {
                    if (arr[i] < minNum) {
                        minNum = arr[i];
                }
            }
    
            return minNum;
            }
            function findMaximum(arr) {
                var max = arr[0];

                for (var i = 1; i < arr.length; i++) {
                    if (arr[i] > max) {
                        max = arr[i];
                    }
                }

                return max;
            }
            function convertDateFormat(inputDate) {
                var parts = inputDate.split('/');
                var year = parseInt(parts[0], 10);
                var month = parseInt(parts[1], 10);
                var day = parseInt(parts[2], 10);
                var formattedDate = (
(month < 10 ? "0" : "") + month + '/' +
(day < 10 ? "0" : "") + day + '/' +
year
);
                return formattedDate;
            }
            var charts1, charts2;
            var lessgroup = "<%=lid%>";
                var less = lessgroup.split(",").map(Number);
                var lessnum = less.length;
                var i = 0;
                function exportChart() {
                    if (document.getElementById('highcheck').checked) {
                        processNextChart();
                    }
                }
                function processNextChart() {
                    if (i < lessnum) {
                        generateHighchart(less[i], function () {
                            captureAndSend(less[i], function () {
                                i++;
                                processNextChart();
                            });
                        });
                    }
                    else {
                        DownloadPopup();
                    }
                }
                function generateHighchart(lessid, callback) {
                    var treatment = '';
                    // set cont size start
                    var myDiv = document.getElementById('cont');
                    if ("<%=reptype%>" == "true" || "<%=reptype%>" == "True") {
                    myDiv.style.width = '931.84809449pt';
                    myDiv.style.height = '434.678740152pt';
                } else {
                        myDiv.style.width = '931.84809449pt';
                        myDiv.style.height = '434.678740152pt';
                }
                var inputsDate = "<%=sdate%>";
                    inputsDate = inputsDate.replace(/-/g, '/');
                    var inputeDate = "<%=edate%>";
                inputeDate = inputeDate.replace(/-/g, '/');
                    // set cont size End
                var sdate = "<%=sdate%>";
                        var edate = "<%=edate%>";
                    var sid = "<%=sid%>";
                    var lid = lessid;
                    var scid = "<%=scid%>";
                    var tempid = "<%=tempid%>";
                    var prompt = "<%=prompttype%>";
                    var cls = "<%=classtype%>";
                    var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, AllLesson: lid, SchoolId: scid, Templateid: tempid, PromptType: prompt,Clstype: cls });
                    $.ajax({
                        type: "POST",
                        url: "ChainedBarGraphReport.aspx/getChainedBarReport",
                        data: jsonData,
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        success: function (response) {
                            var res = JSON.parse(response.d);
                            if (res.length === 0) {
                                var span = document.getElementById('lbgraphSpan');
                                span.innerHTML = "No Data Available";
                                $('#sname').css("display", "block");
                                $('#trtmnt').css("display", "block");
                                $('#cont').css("display", "none");
                                $('#lnam').css("display", "block");
                                $('#daterang').css("display", "block");
                                $('#mel').css("display", "block");
                                $('#deftxt').css("display", "block");
                                document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                                $.ajax({
                                    type: "POST",
                                    url: "ChainedBarGraphReport.aspx/getNodataavail",
                                    data: JSON.stringify({ lplan: lid, studid: sid }),
                                    contentType: "application/json; charset=utf-8",
                                    async: false,
                                    dataType: "json",
                                    success: function (resp) {
                                        var resu = JSON.parse(resp.d);
                                        $.map(resu, function (items, index) {
                                            var trtmnt = document.getElementById('trtmnt');
                                            trtmnt.style.fontSize = '12px';
                                            if (items['Treatment'] != null) {
                                                document.getElementById('trtmnt').innerHTML = items['Treatment'];
                                            } else {
                                                document.getElementById('trtmnt').innerHTML = 'Tx: ';
                                            }
                                            document.getElementById('lnam').innerHTML = items['LessonPlanName'];
                                            var bname = document.getElementById('lnam');
                                            bname.style.fontSize = '12px';

                                            var deftxt = document.getElementById('deftxt');
                                            deftxt.style.fontSize = '12px';
                                            if (items['Deftn'] != null) {
                                                document.getElementById('deftxt').innerHTML = 'Definition:    ' + items['Deftn'];

                            }
                            else {
                                                document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                            }
                                            document.getElementById('sname').innerHTML = items['StudentName'];
                                            var studname = document.getElementById('sname');
                                            studname.style.fontSize = '12px';
                                        });

                                    },
                                    error: function (xhr, status, error) {

                                        console.error("AJAX request failed:", error);
                                    }
                                });
                                if (scid == 1) {
                                    document.getElementById('mel').innerHTML = 'Melmark New England';
                                    document.getElementById('mel').style.fontSize = '12px';
                                }
                                else {
                                    document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                    document.getElementById('mel').style.fontSize = '12px';
                                }
                                var drange = document.getElementById('daterang');
                                drange.style.fontSize = '12px';


                            }
                            else {
                                $('#trtmnt').css("display", "none");
                                $.map(res, function (item, index) {
                                    document.getElementById('sname').innerHTML = res[0].StudentName;
                                    var studname = document.getElementById('sname');
                                    studname.style.fontSize = '16px';
                                    document.getElementById('lnam').innerHTML = res[0].LessonPlanName;
                                    var lessname = document.getElementById('lnam');
                                    lessname.style.fontSize = '16px';
                                    document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                                    var drange = document.getElementById('daterang');
                                    drange.style.fontSize = '16px';
                                    document.getElementById('mel').innerHTML = 'Melmark New England';
                                    var melm = document.getElementById('mel');
                                    melm.style.fontSize = '16px';
                                    var deft = document.getElementById('deftxt');
                                    deft.style.fontSize = '16px';
                                    if (res[0].Deftn != null) {
                                        document.getElementById('deftxt').innerHTML = 'Definition:    ' + res[0].Deftn;
                                    }
                                    else {
                                        document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                    }
                                    treatment = res[0].Treatment + '<br><br>';
                                });
                                var yAxisname = res[0].ChainType;
                                if (yAxisname == "Forward chain")
                                {
                                    yAxisname = "Forward chain ->";
                                }
                                else if (yAxisname = "Total Task") {
                                    yAxisname = "Total Task"
                                }
                                else {
                                    yAxisname = "<-Backward chain ";
                                }
                                var medicData = [];
                                $.map(res, function (item, index) {
                                    if (item['StepName'] != null) {
                                        var meddata = {};
                                        meddata['name'] = item['ShortName'];
                                        meddata['x'] = item['SessNbr'];
                                        meddata['x2'] = item['SessNbr'] + 1;
                                        meddata['y'] = item['StepName'];
                                        meddata['tooltp'] = item['Prompt'];
                                        meddata['color'] = item['Color'];
                                        medicData.push(meddata);
                                    }
                                });
                                var plotdata = [];
                                $.map(res, function (item, index) {
                                    var wid;
                                    if (item['EventName'] != null) {
                                        var plotdict = new Object;
                                        plotdict['color'] = "black";
                                        plotdict['dashStyle'] = 'solid';
                                        plotdict['value'] = (item['Offset']);
                                        plotdict['width'] = 2;
                                        if (item['EventName'] == null) {
                                            item['EventName'] = "";
                                        }
                                        var event = item['EventName'];
                                        var bgcolor;
                                        if (item['CurrentPrompt'] == null) {
                                            bgcolor = 'black';
                                        }
                                        else {
                                            bgcolor = 'gray';
                                        }
                                        plotdict['label'] = {
                                            text: event,
                                            size: 10,
                                            align: 'top',
                                            textAlign: 'right',
                                            rotation: 0,
                                            useHTML: true,
                                            style: {
                                                fontSize: '12px',
                                                color: 'white',
                                                backgroundColor: bgcolor,
                                            },
                                        };
                                        plotdict['label']['overflow'] = 'justify';
                                        plotdict['label']['crop'] = false;
                                        plotdata.push(plotdict);
                                    }
                                });
                                var sessarr = [];
                                var ymax = [];
                                $.map(res, function (item, index) {
                                    if (item['SessNbr'] != null)
                                    {
                                        sessarr.push(item['SessNbr']);
                                    }
                                    if (item['StepName'] != null) {
                                        ymax.push(item['StepName']);
                                    }
                                });
                                minx = findMinimum(sessarr);
                                maxx = findMaximum(sessarr);
                                maxy = findMaximum(ymax);
                                DrawChart(treatment, medicData, yAxisname, plotdata, minx, maxx, maxy);
                            }
                        },
                        error: OnErrorCall_
                    });
                    function OnErrorCall_(response) {

                        alert("something went wrong!");
                    }
                    setTimeout(callback, 1000);
                }
            function captureAndSend(lessid, callback) {
                //html2canvas($("#HighchartGraph")[0]).then(function (canvas) {
                //    base64 = canvas.toDataURL();
                //    var dataToSend = {
                //        base64: base64, chartId: lessid
                //    };

                var element = document.getElementById("HighchartGraph");

                var scale = window.devicePixelRatio;
                html2canvas(element, {
                    scale: scale,
                    width: element.offsetWidth * scale,
                    height: element.offsetHeight * scale
                }).then(function (canvas) {
                    var base64 = canvas.toDataURL();
                    var dataToSend = {
                        base64: base64,
                        chartId: lessid
                    };
                    $.ajax({
                        type: "POST",
                        url: "ChainedBarGraphReport.aspx/getgraphs",
                        data: JSON.stringify(dataToSend),
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        success: function (response) {

                        },
                        error: function (error) {
                            console.error("Error sending images to the server: " + error);
                        },

                    });
                });
                setTimeout(callback, 1000);
            }
            </script>
    </form>
</body>
</html>
