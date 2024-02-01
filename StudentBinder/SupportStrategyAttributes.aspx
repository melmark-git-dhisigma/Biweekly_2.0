<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SupportStrategyAttributes.aspx.cs" Inherits="StudentBinder_SupportStrategyAttributes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <style type="text/css">
        .warning_box {
            width: 94%;
            clear: both;
            background: url(../Administration/images/warning.png) no-repeat left #fcfae9;
            border: 1px #e9e6c7 solid;
            background-position: 10px 1px;
            padding-left: 50px;
            padding-top: 10px;
            padding-bottom: 5px;
            text-align: left;
            color: Red;
        }

        .web_dialog {
            background: url("../images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            border: 5px solid #B2CCCA;
            color: #333333;
            display: block;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            height: auto;
            left: 40%;
            margin-left: -190px;
            padding: 5px 5px 30px;
            position: fixed;
            display: none;
            top: -100%;
            width: 800px;
            z-index: 102;
        }


        .web_dialog_overlay {
            background: none repeat scroll 0 0 #000000;
            bottom: 0;
            display: none;
            height: 100%;
            left: 0;
            margin: 0;
            opacity: 0.15;
            padding: 0;
            position: fixed;
            right: 0;
            top: 0;
            width: 100%;
            z-index: 101;
        }


        .NFButton {
            /* background: url(../images/masterbtnbg.png) left top;*/
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

            .NFButton:visited {
                color: #FFF;
                /*  background-position: 0 -31px!important;*/
            }

        .NFButtonWithNoImage :hover {
        }

        .NFButton:hover {
            /*   background-position: 0 -31px!important;*/
            background-color: #09646A;
        }


        .border {
            border-right: thin #CCC solid;
            border-top: thin #CCC solid;
        }

        .borderLeft {
            border-left: thin #CCC solid;
        }

        .borderBottom {
            border-bottom: thin #CCC solid;
        }

        table {
            margin: 0 auto;
        }

            table tr {
            }

                table tr td {
                    font-weight: normal;
                    font-size: 14px;
                    padding-left: 6px;
                    font-family: Arial, Helvetica, sans-serif;
                    vertical-align: top;
                    padding: 5px 0 5px 4px;
                    font-size: 13px;
                }

        .setColor {
            color: red;
        }

        .title {
            /*text-transform: uppercase;*/
            font-weight: bold;
            border: none;
            padding: 0;
        }
    </style>

    <style type="text/css">
        #dvStimuliActivity {
            margin-left: -4px;
        }
    </style>

    <script type="text/javascript">

        function PrintDivData(elementId) {

            var printContents = document.getElementById(elementId).innerHTML;
            var originalContents = document.body.innerHTML;

            document.body.innerHTML = printContents;

            window.print();

            document.body.innerHTML = originalContents;
        }

        function DownloadDone() {
            HideWait();
        }

        function showWait() {

            $('#PlzWait').show();

        }

        function hidePrint() {
            $('#Button1').css("display", "none");
        }
        //btnExportWord
        function hideButton() {

            document.getElementById('<%=(btnExportWord).ClientID%>').style.display = 'none';


        }


        function HideWait() {
            $('#PlzWait').hide();
        }
        function showMessage(msg) {
            alert('dfgd');
            alert(msg);
        }

        function DownloadDone() {
            HideWait();
        }


        function checkPostbackExport() {

            showWait();
            return true;
        }


    </script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="overlay" class="web_dialog_overlay"></div>

        <div id="PopDownload" class="web_dialog" style="width: 600px; top: 15%;">

            <div id="Div53" style="width: 700px;">


                <table style="width: 97%">
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%">
                                <tr>
                                    <td runat="server" id="tdMsgExport" class="tdText" style="height: 50px; width: 76%"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" />

                        </td>
                        <td style="text-align: left">

                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClick="btnDone_Click" />

                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div style="float: left; margin-left: 3%">
            <img runat="server" alt="Please Wait..." id="PlzWait" src="../Administration/images/PleaseWait.gif" style="width: 40px; height: 40px; display: none; float: left" />

            <input id="Button1" style="display: none" type="button" name="Print" class="print" title="Print PDF" onclick="javascript: PrintDivData('lessonPrintDiv');" />
            <asp:Button ID="btnExport" Style="display: none" runat="server" CssClass="Export" ToolTip="Export To PDF" Text="" OnClick="btnExport_Click" />
            &nbsp<asp:Button ID="btnExportWord" runat="server" ToolTip="Export To Word" CssClass="ExportWord" Text="" OnClick="btnExportWord_Click" OnClientClick="return checkPostbackExport();" />
        </div>
        <div style="float: right; margin-right: 3%">
            <asp:Button ID="btnRefresh" runat="server" Text="" CssClass="refresh" OnClick="btnRefresh_Click" />

        </div>



        <div id="DivExport" runat="server">
            <div id="lessonPrintDiv" runat="server">

                <table width="95%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="padding: 0" runat="server" id="tdMsg"></td>
                    </tr>
                </table>

                <table width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td class="title" runat="server" id="tdLesson" colspan="2">Title</td>
                    </tr>
                    <tr>
                        <td class="border borderLeft" style="width: 50%" runat="server" id="tdStudent"></td>
                        <td class="border" runat="server" id="tdIEPDate"><strong>ISP Dates:</strong> </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft" style="height: 50px; vertical-align: top; text-align: left;" id="tdObjective" runat="server"><strong>OBJECTIVE:</strong></td>
                    </tr>
                    <%--<tr>
                        <td rowspan="2" class="border  borderLeft" style="vertical-align: top; text-align: left; word-wrap: break-word" runat="server" id="tdFrameWork"><strong>Framework and Strand:</strong></td>
                        <td class="border" runat="server" id="tdSpecificStandard" style="word-wrap: break-word">
                            <strong>Specific Standard:</strong> </td>
                    </tr>
                    <tr>
                        <td class="border" runat="server" id="tdSpecificEntry" style="word-wrap: break-word"><strong>Specific Entry Point:</strong> </td>
                    </tr>--%>
                    <tr>
                        <td class="border  borderLeft" id="tdTypeIns" runat="server" style="word-wrap: break-word"><strong>Type of Instruction:</strong> </td>
                        <td class="border" id="tdMajorSet" runat="server" style="word-wrap: break-word"><strong>SETTING:</strong></td>
                        <%--<td class="border" id="tdMinorSet" runat="server" style="word-wrap: break-word"><strong>Setting: </strong></td>--%>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft " runat="server" id="tdMaterials" style="word-wrap: break-word"><strong>MATERIALS(all items needed + their location): </strong></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft  borderBottom" style="vertical-align: top; text-align: left; word-wrap: break-word" id="tdMeasurement" runat="server"><strong>MEASUREMENT SYSTEM:</strong>
                        </td>
                    </tr>
                </table>


                <br />

                <table id="tblLessonProc" width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td colspan="2" style="text-align: center" class="title">PROCEDURE</td>
                    </tr>


                    <tr>
                        <td class="border borderLeft" style="text-align: center"><strong>INSTRUCTOR<br />
                            (Instruction)</strong><br />
                        </td>
                        <td class="border" style="text-align: center"><strong>INDIVIDUAL<br />
                            (response/desired outcome)</strong><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="border-top: thin solid #CCCCCC;"><strong>Teacher preparation for instruction:</strong></td>
                        <td style="border-top: thin solid #CCCCCC;"><strong>Individual preparation for instruction: </strong></td>
                    </tr>
                    <tr>
                        <td id="q1" runat="server" style="border-top: thin solid #CCCCCC;"></td>
                        <td id="q2" runat="server" style="border-top: thin solid #CCCCCC;"></td>
                    </tr>
                    <tr>
                        <td class="border borderLeft" id="tdLessonDelivary" runat="server" style="word-wrap: break-word"><strong>Delivery Instructions: </strong>
                            <br />
                        </td>
                        <td class="border" id="tdNCorrect" runat="server" style="word-wrap: break-word"><strong>Correct Response:</strong></td>

                    </tr>



                    <%--<tr>

                        <td class="border borderBottom" id="tdNICorrect" runat="server" style="word-wrap: break-word"><strong>Incorrect Response:</strong></td>


                    </tr>--%>

                    <%--<tr>

                        <td class="border borderBottom" id="tdMistrialResponse" runat="server" style="word-wrap: break-word"><strong>Mistrial Response:</strong></td>

                    </tr>--%>
                </table>

                <br />


                <table width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td colspan="2" runat="server" id="tdBaseLine"><strong>POST CHECK PROCEDURE:</strong></td>
                        <%--</br>
                        <span runat="server" id="tdBaseLine"></span>
                        </br>
                        <span runat="server" id="tdGenProcedure"></span>--%>
                    </tr>
                </table>

            </div>
        </div>



    </form>
</body>
</html>
