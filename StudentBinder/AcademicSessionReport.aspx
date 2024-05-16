
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcademicSessionReport.aspx.cs" Inherits="StudentBinder_AcademicSessionReport" %>

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
    <script src="../Scripts/highcharts/7.1.2/grouped-categories.js"></script>
     <script src="../Scripts/highcharts/7.1.2/modules/xrange.js"></script>
    <script src="../Scripts/highcharts/7.1.2/modules/exporting.js"></script>
        <script src="../Scripts/html2canvas.min.js"></script>
            <script src="../Scripts/es6-promise.auto.min.js"></script>
        <script src="../Scripts/es6-promise.min.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />


    <style type="text/css">
         .popup {
    display: none;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.7);
}

.popup-content {
    background-color: white;
    position: absolute;
    top: 3%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    text-align: center;
}

.close {
    position: absolute;
    top: 0;
    right: 0;
    padding: 10px;
    cursor: pointer;
}

        .styleBorder {
            border:none;
        }

        .block { 
            display:inline-block;
            vertical-align:top;
            overflow-x:scroll;
            overflow-y:scroll;
            border:solid grey 1px;
            width:300px;
            height:150px;
        }
        .block select { 
            padding:10px; 
            margin:0px 0px 0px -10px;
        }
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
            height: auto !important;
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
            height: 450px;
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

         .web_dialog11 {
            background: url("../images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            border: 5px solid #B2CCCA;
            color: #333333;
            display: block;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            height: auto;
            left: 30%;
            top: 5%;
            margin-left: -190px;
            padding: 5px 5px 30px;
            position: fixed;
            display: none;            
            width: 800px;
            z-index: 102;
        }


        /*LOADING IMAGE CLOSE */
    </style>
    <script type="text/javascript">

        $(function () {
            adjustStyle();

        });

        function adjustStyle() {
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                $('graphPopup').css('width', '91% !Important');
                $('graphPopup').css('left', ' 220px !Important');
            }

        }

        function HideAndDisplay() {

            var val = document.getElementById("hfPopUpValue").value;
            var valLesson = document.getElementById("hdnallLesson").value;

            if (valLesson == "AllLessons") {
                $('#graphPopup').fadeIn();
            }
        }

        function showPopup() {
            $('#graphPopup').hide();
            $('#overlay').hide();
            var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
             hchart.style.display = 'none';
             var popup = document.getElementById('popup');
             popup.style.display = 'block';
         }

         function closePopup() {
             var popup = document.getElementById('popup');
             popup.style.display = 'none';
             var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
             hchart.style.display = 'block';
             $('#graphPopup').hide();
             $('#overlay').hide();
         }
        function loadWait() {
            $('.loading').fadeIn('fast');//, function () { });
        }
        function HideWait() {
            $('.loading').fadeOut('fast');//, function () { });
        }
        $(document).ready(function () {

            $('.loading').fadeOut('slow', function () {
                $('#fullContents').fadeIn('fast');
            });
            var val = document.getElementById("hfPopUpValue").value;
            var valLesson = document.getElementById("hdnallLesson").value;
            if (val != "true") {
                if (valLesson == "AllLessons") {
                    $('#overlay').fadeIn('slow', function () {
                        $('#graphPopup').fadeIn('fast');
                    });
                    $('#close_x').click(function () {
                        $('#graphPopup').fadeOut('slow', function () {
                            $('#overlay').fadeOut('slow');
                        });
                    });
                }
                else {
                    $('#graphPopup').hide();
                    $('#overlay').hide();
                }
            }
            if (val == "true") {
                //$('#overlay').fadeIn('slow', function () {
                //    $('#graphPopup').fadeIn('fast');
                //    $('#tblListbox').css("display", "none");
                //});
                $('#graphPopup').hide();
                $('#overlay').hide();
            }
            $('#close_x').click(function () {
                $('#graphPopup').fadeOut('slow', function () {
                    $('#overlay').fadeOut('slow');
                });
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
            new JsDatePick({
                useMode: 2,
                target: "<%=txtStartDate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEndDate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });

            $('#<%=btnExport.ClientID %>').click(function () {
                $('.loading').fadeIn('fast');
            });
            var valExport = document.getElementById("hdnExport").value;
            if (valExport == "true") {
                $('#overlay').show();
            }
        };
        //----------------alphonsa-------------------//
        function disp() {

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
            new JsDatePick({
                useMode: 2,
                target: "<%=txtStartDate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEndDate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
        }
        //-------------------------------------------//
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
        <asp:HiddenField ID="hfPopUpValue" runat="server" Value="" />
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="popup" class="popup" >
    <div class="popup-content" id="popup-content">
        <p>Please wait...</p>
    </div>
</div>
        <div id="fullContents" style="width: 98%; margin-left: 1%">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div runat="server" id="LessonDiv" visible="false">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="4" id="tdMsg1" runat="server"></td>
                        </tr>

                        <tr>
                            <td style="text-align: center; width: 25%" class="tdText">Report Start Date
                                <br />
                                <br />
                                <br />
                                Report End Date
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="txtStartDate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                <br />
                                <br />
                                <asp:TextBox ID="txtEndDate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            </td>
                            <td style="text-align: center" class="tdText"></td>
                            <td style="text-align: left; width: 25%" rowspan="2">                              
                                <table style = "width: 100%">
                                    <tr>
                                        <td>  <asp:DropDownList ID="drpSetname" CssClass="drpClass" Width="250px" runat="server"  AddJQueryReference="true" AutoPostBack="true" OnSelectedIndexChanged="drpSetname_SelectedIndexChanged" >                                       
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                </table>                                                                                                                                                              
                                <asp:CheckBox ID="chktrend" runat="server" Text="Include Trend Line"></asp:CheckBox>
                                <br />                                
                                <asp:CheckBox ID="chkarrow" runat="server" Text="Arrow Notes"></asp:CheckBox>
                                <br />                                                                                
                                <asp:CheckBox ID="chkmajor" runat="server" Text="Major Events"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkminor" runat="server" Text="Minor Events"></asp:CheckBox>
                                <br />  
                                <asp:CheckBox ID="chkioa" runat="server" Text="Include IOA"></asp:CheckBox>
                                <br />                                                 
                            </td>
                            <td style="text-align: left; width: 25%" rowspan="2">
                                <label></label>
                                <asp:RadioButtonList ID="rbtnLsnClassType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Day">Day</asp:ListItem>
                                    <asp:ListItem Value="Residence">Residence</asp:ListItem>
                                    <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="rbtnLsnIncidentalRegular" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Incidental">Incidental Teaching Graph</asp:ListItem>
                                    <asp:ListItem Value="Regular" Selected="True">Regular Graph</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>


                    </table>
                </div>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                              <asp:UpdatePanel ID="dropdownpanel" runat="server">
                                    <ContentTemplate>
                             <asp:Button ID="btnPrevious1" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0px 250px" Visible="false" Text="Previous" OnClick="btnPrevious1_Click" OnClientClick="javascript:showPopup();" />
                             <asp:DropDownList ID="ddlLessonplan1" runat="server" CssClass="drpClass" Style="float: left; margin: 0 1px 0px 10px" Height="26px" Width="290px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLessonplan1_SelectedIndexChanged" onchange="showPopup()">
                             </asp:DropDownList>
                            <asp:Button ID="btnNext1" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0 10px" Visible="false" Text="Next" OnClick="btnNext1_Click" OnClientClick="javascript:showPopup();" />
                           </ContentTemplate>
                                </asp:UpdatePanel> 
                            <input type="button" id="btnRefresh" style="float: right; margin: 0 1px 0 1px" class="refresh" runat="server" onclick="HideAndDisplay()" />
                            <asp:Button ID="RefreshMaintenance" style="float: right; margin: 0 1px 0 1px" class="refresh" runat="server" OnClick="RefreshMaintenance_Click" />
                            <asp:Button ID="btnMaintenanceGraph" runat="server" class="showgraph" Style="float: right; margin: 0 1px 0 1px" Text="" CssClass="showgraph" Visible="false" OnClick="btnMaintenanceGraph_Click"/>
                            <asp:Button ID="btnExport" runat="server" Style="float: right; margin: 0 1px 0 1px" ToolTip="Export To PDF" CssClass="pdfPrint" OnClick="btnExport_Click" />
                            <asp:Button ID="btnPrint" runat="server" Style="float: right; margin: 0 1px 0 1px" ToolTip="Print" CssClass="print" OnClick="btnPrint_Click" Visible="false" />
                        </td>
                        <td style="width: 10%"></td>
                    </tr>
                </table>
            </div>
            <div>
                
                <table style="width: 100%">

                    <tr>

                        <td style="text-align: center">
                            <div id="prdiv">
                                <rsweb:ReportViewer ID="RV_LPReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="false" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="100%" Visible="false" AsyncRendering="true">

                                    <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                                    <%--    ReportPath="<%$ appSettings:ReportPath %>"--%>
                                </rsweb:ReportViewer>
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
        <div id="overlay" class="web_dialog_overlay">
        </div>
        <div id="graphPopup" runat="server" class="web_dialog" style="width: 73%">
            <div>

                <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                <asp:UpdatePanel ID="updatepnlSession" runat="server"> 
                    <Triggers>
                         <asp:PostBackTrigger ControlID="btnsubmit" />
                    </Triggers>
                    <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td colspan="4" runat="server" id="tdMsg"></td>
                    </tr>
                    <tr>
                        <td style="text-align: center"></td>
                        <td style="text-align: center">
                            <asp:HiddenField ID="hdnallLesson" runat="server" />
                        </td>
                        <td style="text-align: center"></td>
                        <td></td>
                    </tr>
                </table>
                <p>
                </p>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: center; width: 20%" class="tdText">Report Start Date</td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                        <td>
                            <div style="float: left">
                               <%--<asp:CheckBox ID="chkrepevents" runat="server" Text="Display All Events" ></asp:CheckBox>--%>
                                <br />
                                <div id="divevents">
                                    <asp:CheckBox ID="chkrepmajor" runat="server" Text="Display Major Events" ></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chkrepminor" runat="server" Text="Display Minor Events"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chkreparrow" runat="server" Text="Display Arrow Notes"></asp:CheckBox>
                                    <br />
                                </div>

                                <asp:CheckBox ID="chkrepioa" runat="server" Text="Include IOA"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkreptrend" runat="server" Text="Include Trend Line"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkrepmedi" runat="server" Text="Include Medication"></asp:CheckBox>
                                <br />
                                <asp:RadioButtonList ID="rbtnIncidentalRegular" runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="Dotted" BorderWidth="1px">
                                    <asp:ListItem Value="Incidental">Incidental Teaching Graph</asp:ListItem>
                                    <asp:ListItem Value="Regular" Selected="True">Regular Graph</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td style="text-align: left; margin-left: 1880px" rowspan="2">
                            <asp:Button ID="btnsubmit" runat="server" Text="" ToolTip="Show Graph" CssClass="showgraph" OnClick="btnsubmit_Click" OnClientClick="javascript:loadWait();" />
                            <asp:Button ID="btnsubmith" runat="server" Text="" ToolTip="Show Graph" CssClass="showgraph" Style="display: none;" OnClick="btnsubmit_Click" OnClientClick="showPopup();" />

                        </td>

                    </tr>
                    <tr>


                        <td style="text-align: center" class="tdText">Report End Date</td>
                        <td style="text-align: left" rowspan="1">
                            <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                        <td>
                            <div style="float: left">
                                <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="Dotted" BorderWidth="1px">
                                    <asp:ListItem Value="Day">Day</asp:ListItem>
                                    <asp:ListItem Value="Residence">Residence</asp:ListItem>
                                    <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:CheckBox id="highcheck" runat="server" onchange="btnsubmitVisibility();" Text=""></asp:CheckBox>

                            </div>
                        </td>

                    </tr>

                    <tr>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">
                            <asp:CheckBoxList ID="chkStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="chkStatus_SelectedIndexChanged">
                                <asp:ListItem Selected="True">Active</asp:ListItem>
                                <asp:ListItem Selected="True">Maintenance</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:CheckBoxList></td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                    </tr>
                    <tr>
                        <td id="tblListbox" colspan="5">
                            <table style="margin: 0 auto;">
                                <tr>
                                    <td style="text-align: center" class="tdText">
                                        <div class="block">                                         
                                        <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple" Width="500px"  Font-Names="Arial Narrow" CssClass="styleBorder" ></asp:ListBox>
                                            </div>
                                    </td>
                                    <td style="min-width: 92px" class="tdText">
                                        <asp:Button ID="Button1" CssClass="NFButton" runat="server" Text=">" OnClick="Button1_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="Button2" CssClass="NFButton" runat="server" Text=">>" OnClick="Button2_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="Button3" CssClass="NFButton" runat="server" Text="<" OnClick="Button3_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="Button4" CssClass="NFButton" runat="server" Text="<<" OnClick="Button4_Click" />
                                    </td>
                                    <td style="text-align: center" class="tdText">
                                         <div class="block"> 
                                        <asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple" Width="500px" Font-Names="Arial Narrow" CssClass="styleBorder"></asp:ListBox>
                                             </div>
                                    </td>
                                </tr>
                            </table>
                        </td>


                    </tr>
                    <tr>
                        <td colspan="5" style="color: red">
                            <asp:Label ID="lbltxt" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="5"></td>
                    </tr>
                </table>
                    </ContentTemplate> </asp:UpdatePanel>
            </div>
        </div>
        <div id="downloadPopup" class="web_dialog11" style="width: 600px;">

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

                                    <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="CloseDownload();" />

                        </td>
                        <td style="text-align: left">
                             <%--<input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />--%>
                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClientClick="CloseDownload();"  onClick="btnDone_Click"/>

                        </td>
                    </tr>
                </table>

            </div>
        </div>
        <asp:UpdatePanel ID="highchartupdate" runat="server">
                                    <ContentTemplate>
         <div id="HighchartGraph" runat="server">
            <center>
            <asp:label id="sname" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                <br />
            <asp:label id="lnam" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                <br />
            <asp:label id="daterang" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                </center>
            <br />
            <center>
                 <asp:label runat="server" text="" id="mednodata"  Font-Bold="true" font-size="15px"></asp:label>
           <div id = "medcont" runat="server"  style = "width: 935.43pt; margin: 0 auto"></div>
                <br />
            <asp:label runat="server" text="" id="lbgraph"  Font-Bold="true" font-size="15px"></asp:label>
                <span id="lbgraphSpan"></span>
                <br />
           
            <div id = "cont" runat="server"  style = "width: 1000px; height: 750px; margin: 0 auto"></div>
            <br /><br /><br />
        </center>
           <asp:label id="deftxt" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:150px;"  Font-Bold="true" ></asp:label>
                <br />
       <asp:label id="mel" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:1200px;" Font-Bold="true"></asp:label>
                </div>
                </div>
        
        </div>

                                         </ContentTemplate>
                                </asp:UpdatePanel> 
        <asp:hiddenfield id="hdnExport" runat="server" value="" />
        <asp:HiddenField ID="hdnType" runat="server" Value="" />
        <script>
            function btnsubmitVisibility() {
                var checkbox = document.getElementById('<%= highcheck.ClientID %>');
                 var button1 = document.getElementById('<%= btnsubmit.ClientID %>');
                 var button2 = document.getElementById('<%= btnsubmith.ClientID %>');
                 if (checkbox.checked) {
                     button1.style.display = 'none';
                     button2.style.display = 'block';
                 } else {
                     button1.style.display = 'block';
                     button2.style.display = 'none';
                 }
             }
            function loadchart( sDate , eDate , sid , lid , scid , evnt , trend , ioa , cls ,med,lpstatus, medno ,reptype, inctype,lname){
                var treatment = '';
                //medication report size start
                if (medno == "false" || medno == "False") {
                var meddiv = document.getElementById('medcont');
                    if (med == "true" || med== "True") {
                        if (reptype == "true" || reptype == "True") {
                            meddiv.style.width = '671.976pt';
                            meddiv.style.height = '87.807911808pt';

                        }
                        else {
                         meddiv.style.width = '935.43307pt';
                            meddiv.style.height = '164.05358746pt';
                     }
                 }
                }
                // medication report size end
                // set cont size start
                var myDiv = document.getElementById('cont');
                if (reptype == "true" || reptype == "True") {
                     myDiv.style.width = '671.976pt';
                     myDiv.style.height = '510.2364pt';
                } else {
                    myDiv.style.width = '935.43307pt';
                     myDiv.style.height = '688.901215752pt';
                }
                // set cont size End
                var lplan = lid;
                var stid = sid;
                var jsonData = JSON.stringify({ lplan: lplan, studid: stid });
                $.ajax({
                    type: "POST",
                    url: "AcademicSessionReport.aspx/getSessionReportNext",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var inputsDate = sDate;
                        inputsDate = inputsDate.replace(/-/g, '/');
                        var inputeDate = eDate;
                        inputeDate = inputeDate.replace(/-/g, '/');
                        var obj = JSON.parse(response.d);
                        $.map(obj, function (item, index) {
                            document.getElementById('sname').innerHTML = item['StudentName'];
                            document.getElementById('lnam').innerHTML = lname;
                            document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                            document.getElementById('mel').innerHTML = 'Melmark New England';
                            if (item['Deftn'] != null) {
                                document.getElementById('deftxt').innerHTML = 'Definition:    ' + item['Deftn'];
                            }
                            else {
                                document.getElementById('deftxt').innerHTML = 'Definition:    ';
                            }
                            treatment = item['Treatment']+'<br><br>';
                        });
                    },
                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {

                    alert("something went wrong!");
                }
                var sdate = sDate;
                var edate = eDate;
                var sid = sid;
                var lid = lid;
                var scid = scid;
                var evnt = evnt;
                var trend = trend;
                var ioa = ioa;
                var cls = cls;
                var medjson=JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid,SchoolId: scid});

                if ((med == "true" || med == "True") && (medno == "false" || medno == "False")) {
                    $.ajax({
                        type: "POST",
                        url: "AcademicSessionReport.aspx/getmedAcademicReport",
                        data: medjson,
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        success: function (response) {
                            var resp = JSON.parse(response.d);
                            var medicData = []; var val = 0;
                            resp.forEach(function (item) {
                                item.duration = extractdate(item.EndTime) - extractdate(item.EvntTs);
                            });
                            resp.sort(function (a, b) {
                                return b.duration - a.duration;
                            });
                            $.map(resp, function (item, index) {
                                var meddata = {};
                                meddata['name'] = item['EventName'] + item['Comment'];
                                meddata['x'] = extractdate(item['EvntTs']);
                                meddata['x2'] = extractdate(item['EndTime']);
                                meddata['y'] = val;
                                medicData.push(meddata);
                                val = val + 1
                            });
                            Highcharts.chart('medcont', {
                                chart: {
                                    type: 'xrange',
                                    plotBorderWidth: 2,
                                    plotBorderColor: 'black',
                                    spacingTop: 20,
                                    spacingRight: 20,
                                    spacingBottom: 20,
                                    spacingLeft: 20,
                                },
                                title: {
                                    text: ''
                                },
                                subtitle: {
                                    text: ' Medication Details',
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

                                    //visible: false,
                                    type: 'datetime',
                                    //linkedTo: 0,
                                    min: extractdateOther(sdate.replace('/', '-')),
                                    max: extractdateOther(edate.replace('/', '-')),
                                    tickInterval: 2 * 24 * 3600 * 1000,
                                    tickPositioner: function () {
                                        var ticks = [];
                                        var interval = this.options.tickInterval;
                                        var currentTick = this.min;

                                        while (currentTick <= this.max) {
                                            ticks.push(currentTick);
                                            currentTick += interval;
                                        }

                                        return ticks;
                                    },
                                    lineWidth: 0,
                                    minorGridLineWidth: 0,
                                    lineColor: 'transparent',
                                    minorTickLength: 0,
                                    tickLength: 0,
                                    startOnTick: true,
                                    endOnTick: true,
                                    labels: {
                                        enabled: false,
                                        //  rotation: -90,
                                        //formatter: function () {
                                        //    return Highcharts.dateFormat('%m/%d/%Y', this.value);
                                        //}
                                    }
                                },
                                credits: {
                                    enabled: false
                                },
                                yAxis: {
                                    gridLineWidth: 0,
                                    offset: 60,
                                    title: {
                                        text: ''
                                    },
                                    labels: {
                                        enabled: false
                                    },
                                    reversed: true
                                },
                                tooltip: {
                                    formatter: function () {
                                        return Highcharts.dateFormat('%m/%d/%Y', this.point.x) + '-' + Highcharts.dateFormat('%m/%d/%Y', this.point.x2);

                                    }
                                },
                                series: [{
                                    data: medicData,
                                    showInLegend: false,
                                    dataLabels: {
                                        enabled: true,
                                        formatter: function () {
                                            return this.point.name;
                                        }
                                    },

                                }],
                                exporting: {
                                    sourceWidth: 935.43307,
                                    sourceHeight: 164.05358746,
                                    // scale: 2 (default)
                                    //chartOptions: {
                                    //    subtitle: null
                                    //}
                                }
                            });
                        },
                        error: OnErrorCal
                    });
                    function OnErrorCal(response) {

                        alert("Something went wrong! Error: ");

                    }
                }

                var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, AllLesson: lid, SchoolId: scid, Events: evnt, Trendtype: trend, IncludeIOA: ioa, Clstype: cls });
                $.ajax({
                    type: "POST",
                    url: "AcademicSessionReport.aspx/getSessionReport",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var res = JSON.parse(response.d);
                        if (res.length === 0) {
                            var hchart = document.getElementById('<%= cont.ClientID %>');
                            if (hchart) {
                                hchart.style.display = 'none';
                            }
                            var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                            if (hchart2) {
                                hchart2.style.display = 'none';
                            }
                            var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                            if (hchart4) {
                                hchart4.style.display = 'none';
                            }
                            var hchart5 = document.getElementById('<%= mel.ClientID %>');
                            if (hchart5) {
                                hchart5.style.display = 'none';
                            }
                            var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                            if (hchart6) {
                                hchart6.style.display = 'none';
                            }
                            var span = document.getElementById('lbgraphSpan');
                            span.innerHTML = "No Data Available";
                            $('#sname').css("display", "none");
                            $('#cont').css("display", "none");
                            $('#lnam').css("display", "none");
                            $('#daterang').css("display", "none");
                            $('#mel').css("display", "none");
                            $('#deftxt').css("display", "none");
                        }
                        else {
                            var hchart = document.getElementById('<%= cont.ClientID %>');
                            if (hchart) {
                                hchart.style.display = 'block';
                            }
                            var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                            if (hchart2) {
                                hchart2.style.display = 'block';
                            }
                            var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                            if (hchart4) {
                                hchart4.style.display = 'block';
                            }
                            var hchart5 = document.getElementById('<%= mel.ClientID %>');
                            if (hchart5) {
                                hchart5.style.display = 'block';
                            }
                            var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                            if (hchart6) {
                                hchart6.style.display = 'block';
                            }
                            var leftmax, rightmax;
                            leftmax = res[0].MaxScore;
                            rightmax = res[0].MaxDummyScore;
                            res.sort(sortByDate);
                            var secstatus = true;
                            // primary and secondary y axis start.
                            var leftY = res[0].LeftYaxis;
                            var rightY = res[0].RightYAxis;
                            if (rightY == null) {
                                secstatus = false;
                            }
                            // primary and secondary y axis end.


                            // remove all null session (incident) start
                            if (inctype == "True" || inctype== "true") {
                                for (var i = res.length - 1; i >= 0; i--) {
                                    if (res[i].Rownum == null) {
                                        res.splice(i, 1);
                                    }
                                }
                            }
                            // remove all null session (incident) end
                            // check 1 session for event start
                           
                            // check 1 session for event start
                            //start newrow correction
                            var prevRow = minumumSession(res);
                            $.map(res, function (item, index) {
                                if (item['NewRow'] != null) {
                                        item['NewRow'] = item['NewRow'];
                                        prevRow = item['NewRow'];
                                    }
                                    else {
                                        $.map(res, function (ite, index) {
                                            if (ite['NewRow'] != null && ite['NewRow'] >= prevRow + 1) {
                                                ite['NewRow'] = ite['NewRow'] + 1;
                                            }
                                        });
                                        item['NewRow'] = prevRow + 1;
                                        prevRow = item['NewRow']
                                    }
                            });
                                //end newrow correction
                                //x category start // regular graph category start
                                var catarr = [];
                                var processedNames = [];
                                var xval = 0;
                                var xvalarr = [];

                                $.map(res, function (item, index) {
                                    var ssdate = extractTimestamp(item['SessionDate']);
                                    var name = formatDate(ssdate);

                                    var isDuplicate = false;
                                    for (var i = 0; i < processedNames.length; i++) {
                                        if (processedNames[i] === name) {
                                            isDuplicate = true;
                                            break;
                                        }
                                    }

                                    if (!isDuplicate) {
                                        var catdata = new Object;
                                        catdata['name'] = name;
                                        var carray = [];

                                        $.map(res, function (ite, index) {
                                            if (ssdate === extractTimestamp(ite['SessionDate'])) {
                                                if (ite['Rownum'] == null && ite['CalcType'] == 'Event') {
                                                    ite['Rownum'] = "";
                                                    xvalarr.push(xval);
                                                }
                                                if (!CheckforRow(carray, ite['Rownum'])) {
                                                    carray.push(ite['Rownum']);
                                                    xval = xval + 1;
                                                }
                                            }
                                        });
                                        catdata['categories'] = carray;
                                        catarr.push(catdata);
                                        processedNames.push(name);
                                    }
                                });
                                
                                var finalsort = catarr.map(function (order, key) {
                                    var oldname = order.name.split('/');
                                    var newname = oldname[0] + '-' + oldname[1];
                                    var narr = {

                                        "name": newname,
                                        "categories": order.categories

                                    }

                                    return narr;
                                });
                            // regular graph category end
                            var incCat = [];
                            // incident graph category start
                            if (inctype == "True" || inctype == "true") {
                                $.map(res, function (item, index) {
                                    var existsInArray = false;
                                    for (var i = 0; i < res.length; i++) {
                                        if (incCat[i] === item['Rownum']) {
                                            existsInArray = true;
                                            break;
                                        }
                                    }

                                    if (!existsInArray) {
                                        incCat.push(item['Rownum']);
                                    }
                                });
                            }
                                // incident graph category end
                                // x category end
                                // Plot Data Start
                                var data = [];
                                var yAxis = [];
                                var color = [];
                                var symbol = [];
                                var yAxisAr = [];
                                var colorAr = [];
                                var symbolAr = [];
                                var arrowArray = [];
                                var arrowNull = [];
                                var calc = []; var perc = []; var nonperc = [];
                                var dummy = [];
                                var prevtype;
                                $.map(res, function (item, index) {
                                    if (item['CalcType'] != 'Event') {
                                        var yax;

                                        if (item['LeftYaxis'] != null && item['RightYAxis'] == null || item['LeftYaxis'] == null && item['RightYAxis'] != null) {
                                            yax = 0;
                                        }
                                        else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYAxis']) {
                                            yax = 0;
                                        }
                                        else {
                                            yax = 1;
                                        }

                                        if (yax == 0 || item['RptLabel'] == item['LeftYaxis']) {
                                            var catdata = {};
                                            var symb = {};

                                            catdata['date'] = item['NewRow'] - 1;
                                            catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                            catdata['Color'] = item['Color'];
                                            catdata['IOAPerc'] = item['IOAPerc'];
                                            catdata['dateval'] = extractdate(item['SessionDate']);
                                            symb['symbol'] = item['Shape'].toLowerCase();
                                            symb['enabled'] = true;
                                            symb['states'] = { hover: { enabled: true } }
                                            catdata['symbol'] = symb;
                                            catdata['yAxis'] = 0;
                                            catdata['value'] = (item['Score']);
                                            data.push(catdata);
                                            yAxis[catdata['name']] = catdata['yAxis'];
                                            color[catdata['name']] = catdata['Color'];
                                            symbol[catdata['name']] = catdata['symbol'];
                                            calc[catdata['name']] = catdata['CalcType'];
                                        }
                                        else {
                                            var catdata = {};
                                            var symb = {};

                                            catdata['date'] = item['NewRow'] - 1;
                                            catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                            catdata['Color'] = item['Color'];
                                            catdata['IOAPerc'] = item['IOAPerc'];
                                            catdata['dateval'] = extractdate(item['SessionDate']);
                                            symb['symbol'] = item['Shape'].toLowerCase();
                                            symb['enabled'] = true;
                                            symb['states'] = { hover: { enabled: true } }
                                            catdata['symbol'] = symb;
                                            catdata['yAxis'] = 1;
                                            catdata['value'] = (item['DummyScore']);
                                            data.push(catdata);
                                            yAxis[catdata['name']] = catdata['yAxis'];
                                            color[catdata['name']] = catdata['Color'];
                                            symbol[catdata['name']] = catdata['symbol'];
                                            calc[catdata['name']] = catdata['CalcType'];
                                        }
                                    }
                                    
                                });
                           
                            // score and dummy score start
                                var newData = []; var names = [];
                                data.forEach(function (item) {
                                    if (!names[item.name]) {
                                        names[item.name] = true;
                                        newData.push(item.name);
                                    }
                                });
                                var ser = newData.map(function (name) {
                                    return {
                                        name: name,
                                        yAxis: yAxis[name],
                                        color: color[name],
                                        marker: symbol[name],
                                        dataGrouping: { enabled: true },
                                        data: data.filter(function (item) {
                                            return item.name === name;
                                        }).map(function (item) {
                                            return {
                                                x: item.date,
                                                y: item.value,
                                                IOAPerc: item.IOAPerc,
                                                dateval:item.dateval
                                            };
                                        })
                                    };

                                });
                             // score and dummy score start
                           
                                //set duplicate series (case:no series) start
                                dupdata = [];
                                if (data.length === 0) {
                                    var samdata = {};
                                    var sd = rownum[0]; var ed = rownum[rownum.length - 1];
                                    samdata['data'] = [{ x: sd, y: null }, { x: ed, y: null }];
                                    samdata['showInLegend'] = false;
                                    dupdata.push(samdata);
                                    ser = dupdata;
                                    leftmax = 1; rightmax = 1;
                                }
                                //set duplicate series (case:no series) end
                                //trend start
                                var arrData = []; var yAxis1 = []; var color1 = []; var symbol1 = [];
                                $.map(res, function (item, index) {
                                   
                                    if (item['CalcType'] != 'Event' && trend == "Quarter") {
                                        var yax;

                                        if (item['LeftYaxis'] != null && item['RightYAxis'] == null || item['LeftYaxis'] == null && item['RightYAxis'] != null) {
                                            yax = 0;
                                        }
                                        else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYAxis']) {
                                            yax = 0;
                                        }
                                        else {
                                            yax = 1;
                                        }
                                        if (yax == 0 || item['RptLabel'] == item['LeftYaxis'] || (item['RptLabel'] != item['LeftYaxis'] && secstatus == false)) {
                                            var trnddata = {};
                                            var symbarr = {};
                                            trnddata['date'] = (item['NewRow'] - 1);
                                            trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                            trnddata['value'] = item['Trend'];
                                            trnddata['Color'] = "Gray";
                                            symbarr['enabled'] = false;
                                            trnddata['symbol'] = symbarr;
                                            trnddata['yAxis1'] = 0;
                                            arrData.push(trnddata);
                                            yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                            color1[trnddata['name']] = trnddata['Color'];
                                            symbol1[trnddata['name']] = trnddata['symbol'];
                                        }
                                        else {
                                            var trnddata = {};
                                            var symbarr = {};
                                            trnddata['date'] = (item['NewRow'] - 1);
                                            trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                            trnddata['value'] = item['Trend'];
                                            trnddata['Color'] = "Gray";
                                            symbarr['enabled'] = false;
                                            trnddata['symbol'] = symbarr;
                                            trnddata['yAxis1'] = 1;
                                            arrData.push(trnddata);
                                            yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                            color1[trnddata['name']] = trnddata['Color'];
                                            symbol1[trnddata['name']] = trnddata['symbol'];

                                        }
                                    }
                                });
                                var newData1 = []; var name1 = [];
                                // var newdata1 = [...new Set(arrData.map(item => item.name))];
                                arrData.forEach(function (item) {
                                    if (!name1[item.name]) {
                                        name1[item.name] = true;
                                        newData1.push(item.name);
                                    }
                                });
                                var ser1 = newData1.map(function (name) {
                                    return {
                                        name: "",
                                        yAxis: yAxis1[name],
                                        color: color1[name],
                                        marker: symbol1[name],
                                        dashStyle: "DashDotDot",
                                        lineWidth: 2,
                                        showInLegend: false,
                                        data: arrData.filter(function (item) {
                                            return item.name === name;
                                        }).map(function (item) {
                                            return {
                                                x: item.date,
                                                y: item.value
                                            };
                                        })
                                    };
                                });
                            //trend  end
                                ser = ser.concat(ser1);
                                //Event plot Start
                                var plotdata = [];
                                var maxYPosition = 300;
                                var currentYPosition = 0;
                                $.map(res, function (item, index) {
                                    var style; var wid;
                                    if (item['CalcType'] == "Event" && item['EventType'] != null && item['EventType'] != 'Arrow notes') {
                                       
                                            if (item['EventType'] == "Major") {
                                                style = 'solid';
                                                wid = 1;
                                            }
                                            else {
                                                style = 'dot';
                                                wid = 2;
                                            }

                                            var plotdict = new Object;
                                            plotdict['color'] = "black";
                                            plotdict['dashStyle'] = style;

                                            plotdict['value'] = (item['NewRow'] - 1);
                                            plotdict['width'] = wid;
                                            if (item['EventName'] == null)
                                            {
                                                item['EventName'] = "";
                                            }
                                            var str = item['EventName'];
                                        var txtal; var xax;
                                        var valign;
                                        if (item['EventType'] == "Major") {
                                            txtal = 'right';
                                            xax = -2;
                                            valign='middle'
                                            }
                                        else {
                                            txtal = 'left';
                                            xax = 9;
                                            valign='middle';
                                            }
                                            plotdict['label'] = {
                                                text: str,
                                                //textAlign: txtal,
                                                formatter: function () {
                                                    var labelText = this.options.text; 
                                                    labelText = labelText.replace(/↑/g, '<span style="font-size: larger;">↑</span>')
                                                                       .replace(/↓/g, '<span style="font-size: larger;">↓</span>');
                                                    return labelText; 
                                                },
                                                useHTML: true,
                                                style: {
                                                    fontWeight: 'normal',
                                                    color: 'black',
                                                    fontSize: '12px',
                                                    fontFamily: 'Arial',
                                                    textShadow: 'none',
                                                },
                                               // align: 'right',
                                                x: xax,
                                                verticalAlign:valign,
                                                rotation: -90
                                            };
                                            plotdict['label']['overflow'] = 'justify';
                                            plotdict['label']['crop'] = false;
                                            plotdata.push(plotdict);
                                        }
                                });
                                //arrow start( null session)
                                var arrows = [];
                                var plotarr = new Object;
                                var plotNew
                                plotarr['type'] = 'scatter';
                                plotarr['name'] = ser[0].name;
                                plotarr['showInLegend'] = true;
                                if (inctype== "True" || inctype == "true") {
                                    plotarr['color'] = 'black',
                                    plotarr['marker'] = {
                                        symbol: 'square',
                                        radius: 4,
                                    }
                                }
                                else {
                                    plotarr['marker'] = {
                                        symbol: 'url(../Scripts/arrownote.png)',
                                        width: 6,
                                        height: 8,
                                    }
                                }
                                $.map(res, function (item, index) {
                                    if (item['ArrowNote'] != null) {

                                        arrows.push({ x: (item['NewRow'] - 1), y: item['Score'], arrowval: item['ArrowNote'] });

                                    }
                                });
                                var mergedarr = [];
                                var mergedKeys = {};

                                for (var i = 0; i < arrows.length; i++) {
                                    var key = arrows[i].x + '-' + arrows[i].y;
                                    if (!mergedKeys[key]) {
                                        mergedarr.push(arrows[i]);
                                        mergedKeys[key] = true;
                                    } else {
                                        for (var j = 0; j < mergedarr.length; j++) {
                                            if (mergedarr[j].x === arrows[i].x && mergedarr[j].y === arrows[i].y) {
                                                mergedarr[j].arrowval += ',' + arrows[i].arrowval;
                                                break;
                                            }
                                        }
                                    }
                                }
                                plotarr['data'] = mergedarr;
                                if (plotarr['data'].length != 0) {
                                    ser.push(plotarr);
                                }
                                var rownum = JSON.parse(JSON.stringify(finalsort));
                                var minxvalue = minx(res)-1;
                                var maxxvalue = maxx(res)-1;
                                if (inctype == "True" || inctype == "true") {
                                    drawGraphInc(treatment, secstatus, leftY, rightY, ser, plotdata, incCat, leftmax, rightmax, minxvalue, maxxvalue)
                                }
                                else {
                                    drawGraph(treatment, secstatus, leftY, rightY, ser, plotdata, rownum, leftmax, rightmax, minxvalue, maxxvalue)
                                }

                            }
                    },
                    error: OnErrorCall_
                });

                function OnErrorCall_(response) {
                    alert("something went wrong!");

                }
                closePopup();
                HideWait();

            }
            
            function drawGraph(treatment, secstatus, leftY, rightY, ser, plotdata, rownum, leftmax, rightmax, minxvalue, maxxvalue) {
                            var chart = Highcharts.chart('cont', {
                                chart: {
                                    plotBorderWidth: 2,
                                    plotBorderColor: 'black',
                                    spacingTop: 20,
                                    spacingRight: 20,
                                    spacingBottom: 20,
                                    spacingLeft: 20,
                                    marginTop:100,
                                },

                                title: {
                                    text: '',
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
                                credits: {
                                    enabled: false
                                },
                                yAxis:
                          [{ // Primary yAxis 
                              min: 0,
                              gridLineWidth: 0,
                            //  tickInterval: lintrvl,
                              softMin: 0,
                              max: leftmax,
                              title: {
                                  text: leftY,
                                  useHTML: true,
                                  style: {
                                      fontWeight: 'bold',
                                      color: 'black',
                                      fontSize: '12px',
                                      fontFamily: 'Arial'
                                  }
                              },
                              labels: {
                                  style: {
                                      color: 'black',
                                      fontSize: '12px',
                                      fontWeight: 'normal',
                                      fontFamily: 'Arial'
                                  }
                              }

                          }, { // Secondary yAxis
                              visible: secstatus,
                              gridLineWidth: 0,
                          //    tickInterval: rintrvl,
                              min: 0,
                              softMin: 0,
                              max: rightmax,
                              title: {
                                  text: rightY,
                                  useHTML: true,
                                  style: {
                                      fontWeight: 'bold',
                                      color: 'black',
                                      fontSize: '12px',
                                      fontFamily: 'Arial'
                                  }
                              },
                              labels: {
                                  style: {
                                      color: 'black',
                                      fontSize: '12px',
                                      fontWeight: 'noraml',
                                      fontFamily: 'Arial'
                                  }
                              },
                              opposite: true,
                          }],

                                xAxis: {
                                    plotLines:
                                        plotdata,

                                    //  categories: rownum,
                                    categories: rownum,
                                    min: minxvalue,
                                    max:maxxvalue,
                                    title: {
                                        text: 'Dates',
                                        useHTML: true,
                                    style: {
                                        color: 'black',
                                        fontWeight: 'bold',
                                        fontSize: '12px',
                                        fontFamily: 'Arial'

                                    }

                                },
                                    labels: {
                                        style: {
                                            color: 'black',
                                            fontSize: '12px',
                                            fontWeight: 'normal',
                                            fontFamily: 'Arial'
                                        }
                                    },
                                },
                                    legend: {
                                        layout: 'horizontal',
                                        align: 'right',
                                        verticalAlign: 'bottom',
                                        labelFormatter: function () {
                                            return '<span style="color: black; font-weight: normal;font-size: 12px; font-family:Arial">' + this.name + '</span>';
                                        }
                                    },
                                    plotOptions: {
                                        series: {
                                            turboThreshold: 5000,
                                            dataLabels: {
                                                allowOverlap: true,
                                                enabled: true,
                                                rotation: -90,
                                                align: 'top',
                                                y: -5,
                                                overflow: 'justify',
                                                crop: false,
                                                style: {
                                                    textOutline: '1px contrast'
                                                },

                                                formatter: function () {
                                                    var point = this.point;
                                                    if (point.IOAPerc) {
                                                        if (point.y > 0) {
                                                            var words = point.IOAPerc.split(' ');
                                                            var newpoint = '';
                                                            for (var i = 0; i < words.length; i++) {
                                                                if (words[i].endsWith('%') && i < words.length - 1) {
                                                                    newpoint += '<span style="font-size: 12px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span><br> ';
                                                                } else {
                                                                    newpoint += '<span style="font-size: 12px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span> ';
                                                            }
                                                        }
                                                            return '<div style="white-space: nowrap; font-weight: normal;font-size: 12px; font-family: Arial; color: black">' + newpoint + '</div>';
                                                        }
                                                        else {
                                                            return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 12px;font-family:Arial;color:black;text-shadow: none;font-weight:normal;">' + point.IOAPerc + '</span>';
                                                        }
                                                    }
                                                    else if (point.arrowval) {
                                                        return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 12px; font-family:Arial;font-color:black;text-shadow: none;font-weight:normal;">' + point.arrowval;
                                                    }
                                                    else {
                                                        return '';
                                                    }
                                                },
                                            },
                                        },
                                    },
                                
                                    tooltip: {
                                        formatter: function () {
                                            return '<b>' + Highcharts.dateFormat('%m/%d/%Y', new Date(this.point.dateval)) + '</b>: ' + this.y;
                                        }
                                    },
                                    series: ser,
                                    exporting: {
                                        sourceWidth: 935.43307,
                                        sourceHeight: 688.901215725,
                                        // scale: 2 (default)
                                        //chartOptions: {
                                        //    subtitle: null
                                        //}
                                    }
                            });
            }
                

            //incident graph start
            function drawGraphInc(treatment, secstatus, leftY, rightY, ser, plotdata, incCat,leftmax, rightmax, minxvalue, maxxvalue) {
                var chart = Highcharts.chart('cont', {
                    chart: {
                        type:'scatter',
                        plotBorderWidth: 2,
                        plotBorderColor: 'black',
                        spacingTop: 20,
                        spacingRight: 20,
                        spacingBottom: 20,
                        spacingLeft: 20,
                        marginTop: 100,
                    },

                    title: {
                        text: '',
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
                    credits: {
                        enabled: false
                    },
                    yAxis:
              [{ // Primary yAxis 
                  min: 0,
                  gridLineWidth: 0,
                 //   tickInterval: lintrvl,
                  softMin: 0,
                  max: leftmax,
                  title: {
                      text: leftY,
                      useHTML: true,
                      style: {
                          fontWeight: 'bold',
                          color: 'black',
                          fontSize: '12px',
                          fontFamily: 'Arial'

                      }
                  },
                  labels: {
                      style: {
                          color: 'black',
                          fontSize: '12px',
                          fontWeight: 'normal',
                          fontFamily: 'Arial'
                      }
                  },

              }, { // Secondary yAxis
                  visible: secstatus,
                  gridLineWidth: 0,
                //    tickInterval: rintrvl,
                  min: 0,
                  softMin: 0,
                  max: rightmax,
                  title: {
                      text: rightY,
                      useHTML: true,
                      style: {
                          fontWeight: 'bold',
                          color: 'black',
                          fontSize: '12px',
                          fontFamily: 'Arial'

                      }
                  },
                  labels: {
                      style: {
                          color: 'black',
                          fontSize: '12px',
                          fontWeight: 'normal',
                          fontFamily: 'Arial'
                      }
                  },
                  opposite: true,
              }],

                    xAxis: {
                        categories: incCat,
                        min: minxvalue,
                        max: maxxvalue,
                        plotLines:
                            plotdata,
                        title: {
                            text: 'Dates',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'normal',
                                fontFamily: 'Arial'
                            }
                        },

                    },
                    legend: {
                        layout: 'horizontal',
                        align: 'right',
                        verticalAlign: 'bottom',
                        labelFormatter: function () {
                            return '<span style="color: black; font-weight: normal;font-size: 12px; font-family:Arial">' + this.name + '</span>';
                        }
                    },
                    plotOptions: {
                        series: {
                            turboThreshold: 5000,
                            dataLabels: {
                                allowOverlap: true,
                                enabled: true,
                                rotation: -90,
                                align: 'top',
                                y: -5,
                                overflow: 'justify',
                                crop: false,
                                style: {
                                    textOutline: '1px contrast'
                                },

                                formatter: function () {
                                    var point = this.point;
                                    if (point.IOAPerc) {
                                        if (point.y > 0) {
                                            var words = point.IOAPerc.split(' ');
                                            var newpoint = '';
                                            for (var i = 0; i < words.length; i++) {
                                                if (words[i].endsWith('%') && i < words.length - 1) {
                                                    newpoint += '<span style="font-size: 12px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span><br> ';
                                                } else {
                                                    newpoint += '<span style="font-size: 12px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span> ';
                                            }
                                        }
                                            return '<div style="white-space: nowrap; font-weight: normal;font-size: 12px; font-family: Arial; color: black">' + newpoint + '</div>';
                                        }
                                        else {
                                            return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 12px;font-family:Arial;color:black;text-shadow: none;font-weight:normal;">' + point.IOAPerc + '</span>';
                                        }
                                    }
                                    else if (point.arrowval) {
                                        return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 12px; font-family:Arial;font-color:black;text-shadow: none;font-weight:normal;">' + point.arrowval;
                                    }
                                    else {
                                        return '';
                                    }
                                },
                            },
                        },
                    },

                    tooltip: {
                        formatter: function () {
                            return '<b>' + Highcharts.dateFormat('%m/%d/%Y', new Date(this.point.dateval)) + '</b>: ' + this.y; 
                        }
                    },
                    series: ser,
                    exporting: {
                        sourceWidth: 935.43307,
                        sourceHeight: 688.901215725,
                        // scale: 2 (default)
                        //chartOptions: {
                        //    subtitle: null
                        //}
                    }
                });
            }

            //incident graph end
            function formatDate(ssdate) {
                var date = new Date(ssdate);
                var month = zerolead(date.getMonth() + 1);
                var day = zerolead(date.getDate());
                var year = date.getFullYear();
                return month + '/' + day + '/' + year;
            }
            function zerolead(number) {
                return number < 10 ? '0' + number : number;
            }
            function extractTimestamp(dateString) {
                var timestampRegex = /\/Date\((\d+)\)\//;
                var match = dateString.match(timestampRegex);
                if (match && match.length >= 2) {
                    return parseInt(match[1], 10);
                }
                return null;
            }
            function CheckforRow(arr, item) {
                if (arr.length != 0) {
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i] == item) {
                            return true;
                        }
                    }
                    return false;
                }
                else {
                    return false;
                }
            }
            function extractdate(dateString) {
                var regex = /\/Date\((\d+)\)\//;
                var match = dateString.match(regex);
                var milliseconds = parseInt(match[1]);
                var date = new Date(milliseconds);
                var year = date.getFullYear();
                var month = date.getMonth();
                var day = date.getDate();
                return Date.UTC(year, month, day);
            }
            function extractdateOther(dateString) {
                var dateParts = dateString.split("-");
                var year = parseInt(dateParts[0]);
                var month = parseInt(dateParts[1]) - 1;
                var day = parseInt(dateParts[2]);
                var sdateval = Date.UTC(year, month, day);
                return sdateval
            }
            function sortByDate(a, b) {
                var timestampA = parseInt(a.SessionDate.match(/\d+/)[0]);
                var timestampB = parseInt(b.SessionDate.match(/\d+/)[0]);
                if (timestampA < timestampB) return -1;
                if (timestampA > timestampB) return 1;

                // If the dates are equal, sort by the number field in descending order
                return a.Rownum - b.Rownum;
            }
            function minumumSession(arr)
            {
                
                var minValue = arr[0].Rownum;
                for (var i = 1; i < arr.length; i++) {
                    if (arr[i].Rownum != null) {
                        if (arr[i].Rownum < minValue) {
                            minValue = arr[i].Rownum;
                        }
                    }
                }
                return minValue;

            }
            function minx(arr)
            {
                var minValue = arr[0].NewRow;
                for (var i = 1; i < arr.length; i++) {
                    if (arr[i].NewRow < minValue) {
                        minValue = arr[i].NewRow;
                    }
                }
                return minValue;
            }
            function maxx(arr) {
                var maxValue = arr[0].NewRow;
                for (var i = 1; i < arr.length; i++) {
                    if (arr[i].NewRow > maxValue) {
                        maxValue = arr[i].NewRow;
                    }
                }
                return maxValue;
            }
            function plotlinedata(plotdata) {
                var combinedArray = [];

                for (var i = 0; i < plotdata.length; i++) {
                    var item = plotdata[i];
                    var existingItem = findExistingItem(combinedArray, item);

                    if (existingItem) {
                        existingItem.label += ',<br>' + item.label;
                    } else {
                        combinedArray.push(item);
                    }
                }
                return combinedArray;

                function findExistingItem(arr, item) {
                    for (var j = 0; j < arr.length; j++) {
                        var combinedItem = arr[j];
                        if (
                            combinedItem.color === item.color &&
                            combinedItem.dashStyle === item.dashStyle &&
                            combinedItem.value === item.value &&
                            combinedItem.width === item.width
                        ) {
                            return combinedItem;
                        }
                    }
                    return null;
                }
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
            var lessnamegroup = "<%=lname%>";
            var lessnm = [];
            var currentItem = '';
            var insideQuotes = false;
            for (var i = 0; i < lessnamegroup.length; i++) {
                if (lessnamegroup[i] === '"') {
                    insideQuotes = !insideQuotes; 
                } else if (lessnamegroup[i] === ',' && !insideQuotes) {
                    lessnm.push(currentItem);
                    currentItem = '';
                } else {
                    currentItem += lessnamegroup[i];
                }
            }
            if (currentItem) {
                lessnm.push(currentItem);
            }
            var lessnamenum = lessnm.length;
            var j = 0;
            var lessgroup = "<%=lid%>";
                var less = lessgroup.split(",").map(Number);
                var lessnum = less.length;
                var i = 0;
                function exportChart() {
                        processNextChart();
                }
                function processNextChart() {
                    if (i < lessnum && j < lessnamenum) {
                        generateHighchart(less[i],lessnm[j], function () {
                            captureAndSend(less[i], function () {
                                i++;
                                j++;
                                processNextChart();
                            });
                        });
                    }
                    else {
                         DownloadPopup();
                        closePopup();
                    }
                }
                function generateHighchart(lessid, lessnameid, callback) {
                    var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
                    hchart.style.display = 'block';
                    var span = document.getElementById('lbgraphSpan');
                    span.innerHTML = "";
                    $('#sname').css("display", "block");
                    $('#cont').css("display", "block");
                    $('#lnam').css("display", "block");
                    $('#daterang').css("display", "block");
                    $('#mel').css("display", "block");
                    $('#deftxt').css("display", "block");
                    var treatment = '';
                    //medication report size start
                    if ("<%=medno%>" == "false" || "<%=medno%>" == "False") {
                        var meddiv = document.getElementById('medcont');
                        if ("<%=med%>" == "true" || "<%=med%>" == "True") {
                    if ("<%=reptype%>" == "true" || "<%=reptype%>" == "True") {
                            meddiv.style.width = '671.976pt';
                            meddiv.style.height = '87.807911808pt';

                        }
                        else {
                            meddiv.style.width = '935.43307pt';
                            meddiv.style.height = '164.05358746pt';

                        }
                    }
                }
                    // medication report size end
                    // set cont size start
                var myDiv = document.getElementById('cont');
                if ("<%=reptype%>" == "true" || "<%=reptype%>" == "True") {
                    myDiv.style.width = '671.976pt';
                    myDiv.style.height = '510.2364pt';
                } else {
                    myDiv.style.width = '935.43307pt';
                    myDiv.style.height = '688.901215752pt';
                }
                    // set cont size End
                var lplan = lessid;
                    var stid = "<%=sid%>";
                    var jsonData = JSON.stringify({ lplan: lplan, studid: stid });
                    $.ajax({
                        type: "POST",
                        url: "AcademicSessionReport.aspx/getSessionReportNext",
                        data: jsonData,
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        success: function (response) {
                            var inputsDate = "<%=sDate%>";
                        inputsDate = inputsDate.replace(/-/g, '/');
                        var inputeDate = "<%=eDate%>";
                        inputeDate = inputeDate.replace(/-/g, '/');
                        var obj = JSON.parse(response.d);
                        $.map(obj, function (item, index) {
                            document.getElementById('sname').innerHTML = item['StudentName'];
                            document.getElementById('lnam').innerHTML = lessnameid;
                            document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                            document.getElementById('mel').innerHTML = 'Melmark New England';
                            if (item['Deftn'] != null) {
                                document.getElementById('deftxt').innerHTML = 'Definition:    ' + item['Deftn'];
                            }
                            else {
                                document.getElementById('deftxt').innerHTML = 'Definition:    ';
                            }
                            treatment = item['Treatment'] + '<br><br>';
                        });
                    },
                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {

                    alert("something went wrong!");
                }
                var sdate = "<%=sDate%>";
                var edate = "<%=eDate%>";
                    var sid = "<%=sid%>";
                    var lid = lessid;
                    var scid = "<%=scid%>";
                    var evnt = "<%=evnt%>";
                    var trend = "<%=trend%>";
                    var ioa = "<%=ioa%>";
                    var cls = "<%=cls%>";
                    var medjson = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, SchoolId: scid });

                    if (("<%=med%>" == "true" || "<%=med%>" == "True") && ("<%=medno%>" == "false" || "<%=medno%>" == "False")) {
                    $.ajax({
                        type: "POST",
                        url: "AcademicSessionReport.aspx/getmedAcademicReport",
                        data: medjson,
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        success: function (response) {
                            var resp = JSON.parse(response.d);
                            var medicData = []; var val = 0;
                            resp.forEach(function (item) {
                                item.duration = extractdate(item.EndTime) - extractdate(item.EvntTs);
                            });
                            resp.sort(function (a, b) {
                                return b.duration - a.duration;
                            });
                            $.map(resp, function (item, index) {
                                var meddata = {};
                                meddata['name'] = item['EventName'] + item['Comment'];
                                meddata['x'] = extractdate(item['EvntTs']);
                                meddata['x2'] = extractdate(item['EndTime']);
                                meddata['y'] = val;
                                medicData.push(meddata);
                                val = val + 1
                            });
                            Highcharts.chart('medcont', {
                                chart: {
                                    type: 'xrange',
                                    plotBorderWidth: 2,
                                    plotBorderColor: 'black',
                                    spacingTop: 20,
                                    spacingRight: 20,
                                    spacingBottom: 20,
                                    spacingLeft: 20,
                                },
                                title: {
                                    text: ''
                                },
                                subtitle: {
                                    text: ' Medication Details',
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

                                    //visible: false,
                                    type: 'datetime',
                                    //linkedTo: 0,
                                    min: extractdateOther(sdate.replace('/', '-')),
                                    max: extractdateOther(edate.replace('/', '-')),
                                    tickInterval: 2 * 24 * 3600 * 1000,
                                    tickPositioner: function () {
                                        var ticks = [];
                                        var interval = this.options.tickInterval;
                                        var currentTick = this.min;

                                        while (currentTick <= this.max) {
                                            ticks.push(currentTick);
                                            currentTick += interval;
                                        }

                                        return ticks;
                                    },
                                    lineWidth: 0,
                                    minorGridLineWidth: 0,
                                    lineColor: 'transparent',
                                    minorTickLength: 0,
                                    tickLength: 0,
                                    startOnTick: true,
                                    endOnTick: true,
                                    labels: {
                                        enabled: false,
                                        //  rotation: -90,
                                        //formatter: function () {
                                        //    return Highcharts.dateFormat('%m/%d/%Y', this.value);
                                        //}
                                    }
                                },
                                credits: {
                                    enabled: false
                                },
                                yAxis: {
                                    gridLineWidth: 0,
                                    offset: 60,
                                    title: {
                                        text: ''
                                    },
                                    labels: {
                                        enabled: false
                                    },
                                    reversed: true
                                },
                                tooltip: {
                                    formatter: function () {
                                        return Highcharts.dateFormat('%m/%d/%Y', this.point.x) + '-' + Highcharts.dateFormat('%m/%d/%Y', this.point.x2);

                                    }
                                },
                                series: [{
                                    data: medicData,
                                    showInLegend: false,
                                    dataLabels: {
                                        enabled: true,
                                        formatter: function () {
                                            return this.point.name;
                                        }
                                    },

                                }],
                                exporting: {
                                    sourceWidth: 935.43307,
                                    sourceHeight: 164.05358746,
                                    // scale: 2 (default)
                                    //chartOptions: {
                                    //    subtitle: null
                                    //}
                                }
                            });
                        },
                        error: OnErrorCal
                    });
                    function OnErrorCal(response) {

                        alert("Something went wrong! Error: ");

                    }
                }
                var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, AllLesson: lid, SchoolId: scid, Events: evnt, Trendtype: trend, IncludeIOA: ioa, Clstype: cls });
                $.ajax({
                    type: "POST",
                    url: "AcademicSessionReport.aspx/getSessionReport",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var res = JSON.parse(response.d);
                        if (res.length === 0) {
                            var hchart = document.getElementById('<%= cont.ClientID %>');
                            if (hchart) {
                                hchart.style.display = 'none';
                            }
                            var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                            if (hchart2) {
                                hchart2.style.display = 'none';
                            }
                            var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                            if (hchart4) {
                                hchart4.style.display = 'none';
                            }
                            var hchart5 = document.getElementById('<%= mel.ClientID %>');
                            if (hchart5) {
                                hchart5.style.display = 'none';
                            }
                            var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                            if (hchart6) {
                                hchart6.style.display = 'none';
                            }
                            var span = document.getElementById('lbgraphSpan');
                            span.innerHTML = "No Data Available";
                            $('#sname').css("display", "none");
                            $('#cont').css("display", "none");
                            $('#lnam').css("display", "none");
                            $('#daterang').css("display", "none");
                            $('#mel').css("display", "none");
                            $('#deftxt').css("display", "none");
                        }
                        else {
                            var hchart = document.getElementById('<%= cont.ClientID %>');
                            if (hchart) {
                                hchart.style.display = 'block';
                            }
                            var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                            if (hchart2) {
                                hchart2.style.display = 'block';
                            }
                            var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                            if (hchart4) {
                                hchart4.style.display = 'block';
                            }
                            var hchart5 = document.getElementById('<%= mel.ClientID %>');
                            if (hchart5) {
                                hchart5.style.display = 'block';
                            }
                            var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                            if (hchart6) {
                                hchart6.style.display = 'block';
                            }
                            var leftmax, rightmax;
                            leftmax = res[0].MaxScore;
                            rightmax = res[0].MaxDummyScore;
                            res.sort(sortByDate);
                            var secstatus = true;
                            // primary and secondary y axis start.
                            var leftY = res[0].LeftYaxis;
                            var rightY = res[0].RightYAxis;
                            if (rightY == null) {
                                secstatus = false;
                            }
                            // primary and secondary y axis end.


                            // remove all null session (incident) start
                            if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                                for (var i = res.length - 1; i >= 0; i--) {
                                    if (res[i].Rownum == null) {
                                        res.splice(i, 1);
                                    }
                                }
                            }
                            // remove all null session (incident) end
                            // check 1 session for event start

                            // check 1 session for event start
                            //start newrow correction
                            var prevRow = minumumSession(res);
                            $.map(res, function (item, index) {
                                if (item['NewRow'] != null) {
                                    item['NewRow'] = item['NewRow'];
                                    prevRow = item['NewRow'];
                                }
                                else {
                                    $.map(res, function (ite, index) {
                                        if (ite['NewRow'] != null && ite['NewRow'] >= prevRow + 1) {
                                            ite['NewRow'] = ite['NewRow'] + 1;
                                        }
                                    });
                                    item['NewRow'] = prevRow + 1;
                                    prevRow = item['NewRow']
                                }
                            });
                            //end newrow correction
                            //x category start // regular graph category start
                            var catarr = [];
                            var processedNames = [];
                            var xval = 0;
                            var xvalarr = [];

                            $.map(res, function (item, index) {
                                var ssdate = extractTimestamp(item['SessionDate']);
                                var name = formatDate(ssdate);

                                var isDuplicate = false;
                                for (var i = 0; i < processedNames.length; i++) {
                                    if (processedNames[i] === name) {
                                        isDuplicate = true;
                                        break;
                                    }
                                }

                                if (!isDuplicate) {
                                    var catdata = new Object;
                                    catdata['name'] = name;
                                    var carray = [];

                                    $.map(res, function (ite, index) {
                                        if (ssdate === extractTimestamp(ite['SessionDate'])) {
                                            if (ite['Rownum'] == null && ite['CalcType'] == 'Event') {
                                                ite['Rownum'] = "";
                                                xvalarr.push(xval);
                                            }
                                            if (!CheckforRow(carray, ite['Rownum'])) {
                                                carray.push(ite['Rownum']);
                                                xval = xval + 1;
                                            }
                                        }
                                    });
                                    catdata['categories'] = carray;
                                    catarr.push(catdata);
                                    processedNames.push(name);
                                }
                            });

                            var finalsort = catarr.map(function (order, key) {
                                var oldname = order.name.split('/');
                                var newname = oldname[0] + '-' + oldname[1];
                                var narr = {

                                    "name": newname,
                                    "categories": order.categories

                                }

                                return narr;
                            });
                            // regular graph category end
                            var incCat = [];
                            // incident graph category start
                            if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                                $.map(res, function (item, index) {
                                    var existsInArray = false;
                                    for (var i = 0; i < res.length; i++) {
                                        if (incCat[i] === item['Rownum']) {
                                            existsInArray = true;
                                            break;
                                        }
                                    }

                                    if (!existsInArray) {
                                        incCat.push(item['Rownum']);
                                    }
                                });
                            }
                            // incident graph category end
                            // x category end
                            // Plot Data Start
                            var data = [];
                            var yAxis = [];
                            var color = [];
                            var symbol = [];
                            var yAxisAr = [];
                            var colorAr = [];
                            var symbolAr = [];
                            var arrowArray = [];
                            var arrowNull = [];
                            var calc = []; var perc = []; var nonperc = [];
                            var dummy = [];
                            var prevtype;
                            $.map(res, function (item, index) {
                                if (item['CalcType'] != 'Event') {
                                    var yax;

                                    if (item['LeftYaxis'] != null && item['RightYAxis'] == null || item['LeftYaxis'] == null && item['RightYAxis'] != null) {
                                        yax = 0;
                                    }
                                    else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYAxis']) {
                                        yax = 0;
                                    }
                                    else {
                                        yax = 1;
                                    }

                                    if (yax == 0 || item['RptLabel'] == item['LeftYaxis']) {
                                        var catdata = {};
                                        var symb = {};

                                        catdata['date'] = item['NewRow'] - 1;
                                        catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                        catdata['Color'] = item['Color'];
                                        catdata['IOAPerc'] = item['IOAPerc'];
                                        catdata['dateval'] = extractdate(item['SessionDate']);
                                        symb['symbol'] = item['Shape'].toLowerCase();
                                        symb['enabled'] = true;
                                        symb['states'] = { hover: { enabled: true } }
                                        catdata['symbol'] = symb;
                                        catdata['yAxis'] = 0;
                                        catdata['value'] = (item['Score']);
                                        data.push(catdata);
                                        yAxis[catdata['name']] = catdata['yAxis'];
                                        color[catdata['name']] = catdata['Color'];
                                        symbol[catdata['name']] = catdata['symbol'];
                                        calc[catdata['name']] = catdata['CalcType'];
                                    }
                                    else {
                                        var catdata = {};
                                        var symb = {};

                                        catdata['date'] = item['NewRow'] - 1;
                                        catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                        catdata['Color'] = item['Color'];
                                        catdata['IOAPerc'] = item['IOAPerc'];
                                        catdata['dateval'] = extractdate(item['SessionDate']);
                                        symb['symbol'] = item['Shape'].toLowerCase();
                                        symb['enabled'] = true;
                                        symb['states'] = { hover: { enabled: true } }
                                        catdata['symbol'] = symb;
                                        catdata['yAxis'] = 1;
                                        catdata['value'] = (item['DummyScore']);
                                        data.push(catdata);
                                        yAxis[catdata['name']] = catdata['yAxis'];
                                        color[catdata['name']] = catdata['Color'];
                                        symbol[catdata['name']] = catdata['symbol'];
                                        calc[catdata['name']] = catdata['CalcType'];
                                    }
                                }

                            });

                            // score and dummy score start
                            var newData = []; var names = [];
                            data.forEach(function (item) {
                                if (!names[item.name]) {
                                    names[item.name] = true;
                                    newData.push(item.name);
                                }
                            });
                            var ser = newData.map(function (name) {
                                return {
                                    name: name,
                                    yAxis: yAxis[name],
                                    color: color[name],
                                    marker: symbol[name],
                                    dataGrouping: { enabled: true },
                                    data: data.filter(function (item) {
                                        return item.name === name;
                                    }).map(function (item) {
                                        return {
                                            x: item.date,
                                            y: item.value,
                                            IOAPerc: item.IOAPerc,
                                            dateval: item.dateval
                                        };
                                    })
                                };

                            });
                            // score and dummy score start

                            //set duplicate series (case:no series) start
                            dupdata = [];
                            if (data.length === 0) {
                                var samdata = {};
                                var sd = rownum[0]; var ed = rownum[rownum.length - 1];
                                samdata['data'] = [{ x: sd, y: null }, { x: ed, y: null }];
                                samdata['showInLegend'] = false;
                                dupdata.push(samdata);
                                ser = dupdata;
                                leftmax = 1; rightmax = 1;
                            }
                            //set duplicate series (case:no series) end
                            //trend start
                            var arrData = []; var yAxis1 = []; var color1 = []; var symbol1 = [];
                            $.map(res, function (item, index) {

                                if (item['CalcType'] != 'Event' && "<%=trend%>" == "Quarter") {
                                        var yax;

                                        if (item['LeftYaxis'] != null && item['RightYAxis'] == null || item['LeftYaxis'] == null && item['RightYAxis'] != null) {
                                            yax = 0;
                                        }
                                        else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYAxis']) {
                                            yax = 0;
                                        }
                                        else {
                                            yax = 1;
                                        }
                                        if (yax == 0 || item['RptLabel'] == item['LeftYaxis'] || (item['RptLabel'] != item['LeftYaxis'] && secstatus == false)) {
                                            var trnddata = {};
                                            var symbarr = {};
                                            trnddata['date'] = (item['NewRow'] - 1);
                                            trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                            trnddata['value'] = item['Trend'];
                                            trnddata['Color'] = "Gray";
                                            symbarr['enabled'] = false;
                                            trnddata['symbol'] = symbarr;
                                            trnddata['yAxis1'] = 0;
                                            arrData.push(trnddata);
                                            yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                            color1[trnddata['name']] = trnddata['Color'];
                                            symbol1[trnddata['name']] = trnddata['symbol'];
                                        }
                                        else {
                                            var trnddata = {};
                                            var symbarr = {};
                                            trnddata['date'] = (item['NewRow'] - 1);
                                            trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                            trnddata['value'] = item['Trend'];
                                            trnddata['Color'] = "Gray";
                                            symbarr['enabled'] = false;
                                            trnddata['symbol'] = symbarr;
                                            trnddata['yAxis1'] = 1;
                                            arrData.push(trnddata);
                                            yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                            color1[trnddata['name']] = trnddata['Color'];
                                            symbol1[trnddata['name']] = trnddata['symbol'];

                                        }
                                    }
                                });
                                var newData1 = []; var name1 = [];
                            // var newdata1 = [...new Set(arrData.map(item => item.name))];
                                arrData.forEach(function (item) {
                                    if (!name1[item.name]) {
                                        name1[item.name] = true;
                                        newData1.push(item.name);
                                    }
                                });
                                var ser1 = newData1.map(function (name) {
                                    return {
                                        name: "",
                                        yAxis: yAxis1[name],
                                        color: color1[name],
                                        marker: symbol1[name],
                                        dashStyle: "DashDotDot",
                                        lineWidth: 2,
                                        showInLegend: false,
                                        data: arrData.filter(function (item) {
                                            return item.name === name;
                                        }).map(function (item) {
                                            return {
                                                x: item.date,
                                                y: item.value
                                            };
                                        })
                                    };
                                });
                            //trend  end
                                ser = ser.concat(ser1);
                            //Event plot Start
                                var plotdata = [];
                                var maxYPosition = 300;
                                var currentYPosition = 0;
                                $.map(res, function (item, index) {
                                    var style; var wid;
                                    if (item['CalcType'] == "Event" && item['EventType'] != null && item['EventType'] != 'Arrow notes') {

                                        if (item['EventType'] == "Major") {
                                            style = 'solid';
                                            wid = 1;
                                        }
                                        else {
                                            style = 'dot';
                                            wid = 2;
                                        }

                                        var plotdict = new Object;
                                        plotdict['color'] = "black";
                                        plotdict['dashStyle'] = style;

                                        plotdict['value'] = (item['NewRow'] - 1);
                                        plotdict['width'] = wid;
                                        if (item['EventName'] == null) {
                                            item['EventName'] = "";
                                        }
                                        var str = item['EventName'];
                                        var txtal; var xax;
                                        var valign;
                                        if (item['EventType'] == "Major") {
                                            txtal = 'right';
                                            xax = -2;
                                            valign = 'middle'
                                        }
                                        else {
                                            txtal = 'left';
                                            xax = 9;
                                            valign = 'middle';
                                        }
                                        plotdict['label'] = {
                                            text: str,
                                            //textAlign: txtal,
                                            formatter: function () {
                                                var labelText = this.options.text;
                                                labelText = labelText.replace(/↑/g, '<span style="font-size: larger;">↑</span>')
                                                                   .replace(/↓/g, '<span style="font-size: larger;">↓</span>');
                                                return labelText;
                                            },
                                            useHTML: true,
                                            style: {
                                                fontWeight: 'normal',
                                                color: 'black',
                                                fontSize: '12px',
                                                fontFamily: 'Arial',
                                                textShadow: 'none',
                                            },
                                            // align: 'right',
                                            x: xax,
                                            verticalAlign: valign,
                                            rotation: -90
                                        };
                                        plotdict['label']['overflow'] = 'justify';
                                        plotdict['label']['crop'] = false;
                                        plotdata.push(plotdict);
                                    }
                                });
                            //arrow start( null session)
                                var arrows = [];
                                var plotarr = new Object;
                                var plotNew
                                plotarr['type'] = 'scatter';
                                plotarr['name'] = ser[0].name;
                                plotarr['showInLegend'] = true;
                                if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                                    plotarr['color'] = 'black',
                                    plotarr['marker'] = {
                                        symbol: 'square',
                                        radius: 4,
                                    }
                                }
                                else {
                                    plotarr['marker'] = {
                                        symbol: 'url(../Scripts/arrownote.png)',
                                        width: 6,
                                        height: 8,
                                    }
                                }
                                $.map(res, function (item, index) {
                                    if (item['ArrowNote'] != null) {

                                        arrows.push({ x: (item['NewRow'] - 1), y: item['Score'], arrowval: item['ArrowNote'] });

                                    }
                                });
                                var mergedarr = [];
                                var mergedKeys = {};

                                for (var i = 0; i < arrows.length; i++) {
                                    var key = arrows[i].x + '-' + arrows[i].y;
                                    if (!mergedKeys[key]) {
                                        mergedarr.push(arrows[i]);
                                        mergedKeys[key] = true;
                                    } else {
                                        for (var j = 0; j < mergedarr.length; j++) {
                                            if (mergedarr[j].x === arrows[i].x && mergedarr[j].y === arrows[i].y) {
                                                mergedarr[j].arrowval += ',' + arrows[i].arrowval;
                                                break;
                                            }
                                        }
                                    }
                                }
                                plotarr['data'] = mergedarr;
                                if (plotarr['data'].length != 0) {
                                    ser.push(plotarr);
                                }
                                var rownum = JSON.parse(JSON.stringify(finalsort));
                                var minxvalue = minx(res) - 1;
                                var maxxvalue = maxx(res) - 1;
                                if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                                    drawGraphInc(treatment, secstatus, leftY, rightY, ser, plotdata, incCat, leftmax, rightmax, minxvalue, maxxvalue)
                                }
                                else {
                                    drawGraph(treatment, secstatus, leftY, rightY, ser, plotdata, rownum, leftmax, rightmax, minxvalue, maxxvalue)
                                }

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
                html2canvas($("#HighchartGraph")[0]).then(function (canvas) {
                    base64 = canvas.toDataURL();
                    var dataToSend = {
                        base64: base64, chartId: lessid
                    };
                    $.ajax({
                        type: "POST",
                        url: "AcademicSessionReport.aspx/getgraphs",
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
