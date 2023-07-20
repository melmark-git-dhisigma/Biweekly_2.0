<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MedicationReport.aspx.cs" Inherits="StudentBinder_MedicationReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/jsDatePick.min.1.3.js"></script>

   
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
            background-image: url("../images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        #fullContents {
            display: none;
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

        /*LOADING IMAGE CLOSE */
    </style>
    <script type="text/javascript">
        $(window).load(function () {
            $('.loading').fadeOut('slow', function () {
                $('#fullContents').fadeIn('fast');
            });
        });

        function loadWait() {
            $('.loading').fadeIn('fast');//, function () { });
        }


    </script>

    <script type="text/javascript">
        window.onload = function () {
            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m-%d-%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m-%d-%Y",
            });


        };

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
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="fullContents">
      <div>
    <table style="width:100%">
    
        <tr>
            <td colspan="4" runat="server" id="tdMsg"></td>
        </tr>
        <tr>
            <td style="text-align:center">
                
                
                
                
                
            </td>
            <td style="text-align:center">
                
                
                
                
                
            </td>
            <td style="text-align:center">
                
                
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td></td>
        </tr>
       
        </table>
          <p>
          </p>
          <table style="width:100%">
    
        <tr>
            <td style="text-align:center" class="tdText">
                Report
                Start Date</td>
            <td style="text-align:left">
                <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox>
                </td>
            <td style="text-align:center" class="tdText">
                Report End Date
                </td>
            <td style="text-align:left" rowspan="1">                             
                 
               
                 
                <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
               
               
                 
             </td>
            <td style="text-align:left" rowspan="1"><asp:Button ID="btnsubmit" runat="server" Text="Show Graph" CssClass="NFButton" OnClick="btnsubmit_Click" OnClientClick="javascript:loadWait();" />
            </td>
        </tr>
       
        <tr>
            <td style="text-align:center" class="tdText">&nbsp;</td>
            <td style="text-align:left">
                &nbsp;</td>
            <td style="text-align:center" class="tdText">
                &nbsp;</td>
        </tr>
       <tr><td colspan="5"></td></tr>        
         <tr><td colspan="5"></td></tr>
        </table>
    </div>
        <div >
            <table style="width:100%"><tr><td style="width:5%"></td><td style="text-align:center" >
                <rsweb:ReportViewer ID="RV_MedReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="true" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="1000px" >

                    <ServerReport ReportPath="<%$ appSettings:MedicationReportPath %>" ReportServerUrl="<%$ appSettings:ReportUrl %>"  />
               
                     </rsweb:ReportViewer>
                       </td></tr></table>
            
        </div>
       </div>
    
        
            
       
    </form>
</body>
</html>
