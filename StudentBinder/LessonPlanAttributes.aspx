<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LessonPlanAttributes.aspx.cs" Inherits="StudentBinder_LessonPlanNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            text-transform: uppercase;
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
                        <td class="title" runat="server" id="tdLesson" colspan="2">Lesson Title</td>
                    </tr>
                    <tr>
                        <td class="border borderLeft" style="width: 50%" runat="server" id="tdStudent"></td>
                        <td class="border" runat="server" id="tdIEPDate"><strong>IEP Dates:</strong> </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft" runat="server" id="tdGoal"><strong>Goal:</strong> </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border borderLeft" runat="server" id="tdLessonPlanGoal"><strong>Lesson Plan Goal:</strong>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft" style="height: 50px; vertical-align: top; text-align: left;" id="tdObjective" runat="server"><strong>Objective:</strong></td>
                    </tr>
                    <tr>
                        <td rowspan="2" class="border  borderLeft" style="vertical-align: top; text-align: left; word-wrap: break-word" runat="server" id="tdFrameWork"><strong>Framework and Strand:</strong></td>
                        <td class="border" runat="server" id="tdSpecificStandard" style="word-wrap: break-word">
                            <strong>Specific Standard:</strong> </td>
                    </tr>
                    <tr>
                        <td class="border" runat="server" id="tdSpecificEntry" style="word-wrap: break-word"><strong>Specific Entry Point:</strong> </td>
                    </tr>
                    <tr>
                        <td class="border  borderLeft" rowspan="2" id="tdTypeIns" runat="server" style="word-wrap: break-word"><strong>Type of Instruction:</strong> </td>
                        <td class="border" id="tdMajorSet" runat="server" style="word-wrap: break-word"><strong>Major Setting:</strong></td>
                    </tr>
                    <tr>
                        <td class="border" id="tdMinorSet" runat="server" style="word-wrap: break-word"><strong>Minor Setting: </strong></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft" runat="server" id="tdPreReqSkills" style="word-wrap: break-word"><strong>Pre-requisite Skills:</strong></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border  borderLeft " runat="server" id="tdMaterials" style="word-wrap: break-word"><strong>Materials: </strong></td>
                    </tr>
                    <tr>
                        <td rowspan="3" class="border  borderLeft  borderBottom" style="vertical-align: top; text-align: left; word-wrap: break-word" id="tdMeasurement" runat="server"><strong>Measurement System:</strong>
                            <br />
                        </td>
                        <td class="border " style="word-wrap: break-word"><strong>Response Definitions: </strong></td>
                    </tr>
                    <tr>
                        <td class="border " id="tdCorrect" runat="server" style="word-wrap: break-word"><strong>Correct Response:</td>
                    </tr>
                    <tr>
                        <td class="border borderBottom " id="tdIncorrect" runat="server" style="word-wrap: break-word"><strong>Incorrect Response:</td>
                    </tr>
                </table>


                <br />

                <table id="tblLessonProc" width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td colspan="3" class="title">LESSON PROCEDURE</td>
                    </tr>


                    <tr>
                        <td class="border borderLeft" style="text-align: center"><strong>TEACHER<br />
                            (Sd/Instruction)</strong><br />
                        </td>
                        <td class="border" style="text-align: center"><strong>INDIVIDUAL<br />
                            (response/desired outcome)</strong><br />
                        </td>
                        <td class="border" style="text-align: center"><strong>CONSEQUENCE<br />
                            (R+ delivery or correction procedure)<br />
                        </strong></td>
                    </tr>
                    <tr>
                        <td style="border-top: thin solid #CCCCCC;">What does teacher do to prepare to teach lesson?</td>
                        <td style="border-top: thin solid #CCCCCC;">What does Individual need to do to prepare for being taught this lesson?</td>
                        <td style="border-top: thin solid #CCCCCC;">What does the teacher do or say in response to the Individual being ready for lesson?</td>
                    </tr>
                    <tr>
                        <td id="q1" runat="server" style="border-top: thin solid #CCCCCC;"></td>
                        <td id="q2" runat="server" style="border-top: thin solid #CCCCCC;"></td>
                        <td id="q3" runat="server" style="border-top: thin solid #CCCCCC;"></td>
                    </tr>
                    <tr>
                        <td rowspan="3" class="border borderLeft" id="tdLessonDelivary" runat="server" style="word-wrap: break-word"><strong>Lesson Delivery Instructions: </strong>
                            <br />
                        </td>
                        <td class="border" id="tdNCorrect" runat="server" style="word-wrap: break-word"><strong>Correct Response:</strong></td>
                        <td class="border" id="tdReInfo" runat="server" style="word-wrap: break-word"><strong>Reinforcement Procedure:</strong></td>

                        <%--<td class="border" id="tdReadiness" runat="server" style="word-wrap: break-word"><strong>Student Readiness Criteria:</strong></td>
                        <td class="border" id="tdRestoReadiness" runat="server" style="word-wrap: break-word"><strong>Teacher Response to Readiness:</strong></td>--%>
                    </tr>



                    <tr>

                        <td class="border borderBottom" id="tdNICorrect" runat="server" style="word-wrap: break-word"><strong>Incorrect Response:</strong></td>
                        <td class="border borderBottom" runat="server" id="tdCorrProc" style="word-wrap: break-word"><strong>Correction Procedure:</strong></td>


                    </tr>

                    <tr>

                        <td class="border borderBottom" id="tdMistrialResponse" runat="server" style="word-wrap: break-word"><strong>Mistrial Response:</strong></td>
                        <td class="border borderBottom" runat="server" id="tdMistrialProcedure" style="word-wrap: break-word"><strong>Mistrial Procedure:</strong></td>

                    </tr>
                    <tr>
                        <td colspan="3">
                            <table style="width: 100%; padding: 0">
                                <tr>
                                    <td runat="server" id="tdStep" style="word-wrap: break-word; border-right: 2px groove;"><strong>Step Description(s):</strong>
                                    </td>
                                    <td runat="server" id="tdSet" style="word-wrap: break-word"><strong>Set Description(s):</strong>
                                    </td>

                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>

                <br />

                <table width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td colspan="3" class="title">CRITERIA:</td>
                    </tr>


                    <tr>
                        <td class="border borderLeft" id="tdAdvPrompt" runat="server" style="width: 33%; word-wrap: break-word"><strong>To Advance a Prompt:</strong> </td>
                        <td class="border" id="tdMovePrompt" runat="server" style="word-wrap: break-word"><strong>To Move Back:</strong> </td>
                        <td class="border" id="tdModPrompt" runat="server" style="word-wrap: break-word"><strong>For Modification:</strong> </td>
                    </tr>
                    <tr>
                        <td class="border borderLeft" id="tdAdvStep" runat="server" style="width: 33%; word-wrap: break-word"><strong>To Advance a Step:</strong> </td>
                        <td class="border" id="tdMoveStep" runat="server" style="word-wrap: break-word"><strong>To Move Back:</strong> </td>
                        <td class="border" id="tdModStep" runat="server" style="word-wrap: break-word"><strong>For Modification: </strong></td>
                    </tr>
                    <tr>
                        <td class="border borderLeft borderBottom" id="tdAdvSet" runat="server" style="width: 34%; word-wrap: break-word"><strong>To Advance a  Set:  </strong></td>
                        <td class="border borderBottom" id="tdMoveSet" runat="server" style="word-wrap: break-word"><strong>To Move Back:</strong> </td>
                        <td class="border borderBottom" id="tdModSet" runat="server" style="word-wrap: break-word"><strong>For Modification:</strong> </td>
                    </tr>
                </table>

                <br />


                <table width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td colspan="3" class="title">BASELINE PROCEDURE:</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 50px; vertical-align: top;" class="border borderBottom borderLeft" runat="server" id="tdBaseLine"></td>
                    </tr>
                </table>
                <table width="95%" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td colspan="3" class="title">GENERALIZATION PROCEDURE</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 50px; vertical-align: top;" class="border borderBottom borderLeft" runat="server" id="tdGenProcedure"></td>
                    </tr>
                </table>

                <table border="0" style="width: 95%;">
                    <tr>

                        <td>
                            <asp:GridView ID="dvStimuliActivity" runat="server" AutoGenerateColumns="False" Width="100.3%" BackColor="#D8D8D8">
                                <Columns>
                                    <asp:BoundField DataField="NAME" HeaderText="STIMULI/ACTIVITY/CONCEPT">
                                        <HeaderStyle Height="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="START DATE (Introduced)" DataField="StartDate">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="MASTERED DATE (Teaching steps done)" DataField="DateMastered">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="END DATE (Generalization done) ">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="DATE CLOSED (Post checks complete)" DataField="DateClosed">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>


            </div>
            <table style="width: 95%; margin: 0px 0px 0px 20px !important;">
                <tr>
                    <%--<td style="width:92px; float:left;" colspan="3" id="tdDate" runat="server"></td>--%>
                    <td style="width:85%; float:left;" id="tdReasonNew" runat="server"></td>
                </tr>
            </table>
        </div>



    </form>
</body>
</html>
