
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
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />


    <style type="text/css">
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
                            
                             <asp:Button ID="btnPrevious1" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0px 250px" Visible="false" Text="Previous" OnClick="btnPrevious1_Click" />
                             <asp:DropDownList ID="ddlLessonplan1" runat="server" CssClass="drpClass" Style="float: left; margin: 0 1px 0px 10px" Height="26px" Width="290px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLessonplan1_SelectedIndexChanged" >
                             </asp:DropDownList>
                            <asp:Button ID="btnNext1" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0 10px" Visible="false" Text="Next" OnClick="btnNext1_Click"  />
                           
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
        <div id="graphPopup" class="web_dialog" style="width: 73%">
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

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="HideWait();" />

                        </td>
                        <td style="text-align: left">
                             <input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />
                         <%--   <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClientClick="CloseDownload();" />--%>

                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <asp:hiddenfield id="hdnExport" runat="server" value="" />
        <asp:HiddenField ID="hdnType" runat="server" Value="" />
    </form>
</body>
</html>
