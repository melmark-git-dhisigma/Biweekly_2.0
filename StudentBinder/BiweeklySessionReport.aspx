<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BiweeklySessionReport.aspx.cs" Inherits="StudentBinder_BiweeklySessionReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/JS/jquery-1.8.0.js" type="text/javascript"></script>
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
            left: 50%;
            margin-left: -520px;
            margin-top: -275px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 102;
            background: #f8f7fc url(../images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

        /*LOADING IMAGE CLOSE */
    </style>
    <script type="text/javascript">
        //$(window).load(function () {
        //    $('.loading').fadeOut('slow', function () {
        //        $('#fullContents').fadeIn('fast');
        //    });
        //});
        $(document).ready(function () {
            var val = document.getElementById("hfPopUpValue").value;
            var valLesson = document.getElementById("hdnallLesson").value;
            alert(val);
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

        //function loadWait() {
        //    $('.loading').fadeIn('fast');//, function () { });
        //}


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
        <asp:HiddenField ID="hfPopUpValue" runat="server" Value=""/>
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="fullContents">

            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="text-align: center">
                            <rsweb:ReportViewer ID="RV_LPReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="true" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="1000px">

                                <ServerReport ReportPath="<%$ appSettings:SessionReportPath %>" ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
        <div id="overlay" class="web_dialog_overlay">
        </div>
        <div id="graphPopup" class="web_dialog" style="width: 950px;">
            <div>

                <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
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
                        <td style="text-align: center" class="tdText">Report Start Date</td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                        <td style="text-align: center" class="tdText">Report End Date</td>
                        <td style="text-align: left" rowspan="1">
                            <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                        <td style="text-align: left; margin-left: 1880px;" rowspan="1">
                            <asp:Button ID="btnsubmit" runat="server" Text="Show Graph" CssClass="NFButton" OnClick="btnsubmit_Click" OnClientClick="javascript:loadWait();" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                        <td style="text-align: center" class="tdText">&nbsp;</td>
                    </tr>
                    <tr>
                        <td id="tblListbox" colspan="5">
                            <table style="margin-left:200px;">
                                <tr>
                                    <td style="text-align: center" class="tdText" colspan="2">
                                        <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple" Rows="10" Width="200px"></asp:ListBox>
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
                                    <td style="text-align: center" class="tdText" colspan="2">
                                        <asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple" Rows="10" Width="200px"></asp:ListBox>
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
            </div>
        </div>



    </form>
</body>
</html>
