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

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="HideWait();" />

                        </td>
                        <td style="text-align: left">
                             <input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />
                           <%-- <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClientClick="CloseDownload();" />--%>

                        </td>
                    </tr>
                </table>

            </div>
        </div>
       <asp:hiddenfield id="hdnExport" runat="server" value="" />
    </form>
</body>
</html>
