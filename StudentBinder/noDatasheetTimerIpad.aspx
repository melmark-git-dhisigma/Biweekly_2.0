<%@ Page Language="C#" AutoEventWireup="true" CodeFile="noDatasheetTimerIpad.aspx.cs" Inherits="StudentBinder_noDatashettTimerIpad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Untitled Document</title>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="js/eye.js"></script>
    <script type="text/javascript" src="js/layout.js"></script>
    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/jquery.smartTab.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/DatasheetStyle.css" rel="stylesheet" id="sized" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />

    <style type="text/css">
        input[type="text"] {
            width: 60%;
        }

        .btnRefresh {
            float:right;
        }

        .scrollDiv {
            clear: both;
            display: inline-block;
            float: left;
            list-style-type: none;
            margin: 0 -1px 0 0;
            padding: 0;
            position: inherit;
            z-index: 100;
            overflow-y: scroll;
            overflow-x: hidden;
            direction: ltr;
            height: 484px;
        }

        .web_dialog2 {
            display: none;
            position: fixed;
            min-width: 290px;
            min-height: 200px;
            left: 50%;
            margin-left: -162px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }
    </style>

    <script type="text/javascript">


        function viewDet() {
            $('#viewDettab').slideToggle();

        }

        function RefreshPage() {
            $('#IfrmTimer').attr('src', 'dataSheetTimer.aspx');
        }

        $(document).ready(function () {

            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {
                //alert("hai");
                //$('#btnStartStopTimer').css("-webkit-appearance", 'button');
                //$('#btnStartStopTimer').css("height", '50px');
                //$('#btnStartStopTimern').css("font-size", '18px');
                $("#IfrmTimer").css("min-height", "");
                $('#IfrmTimer').css("height", "auto");
            }

            // Smart Tab
            $('#tabs').smartTab({ autoProgress: false, stopOnFocus: true, transitionEffect: 'vSlide' });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="5000"></asp:Timer>
            <div class="containerMain">
                <div class="lftPartContainer" style="border: none; width: 66%">

                    <div class="clear"></div>
                </div>
                <div><asp:ImageButton ID="btnRefresh" runat="server" CssClass="btnRefresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png"  ToolTip="Refresh" OnClientClick="RefreshPage();" /></div>
                <div class="rightPartContainer" style="width: 33%;float:right">
                    <iframe id="IfrmTimer" src="DatasheetTimerIpad.aspx" scrolling="no" frameborder="0" style="min-height: 497px; width: 100%;" runat="server"></iframe>
                </div>





                <div class="clear"></div>
            </div>
            <div id="HdrContainer" style="">
                <div id="HdrDiv" class="web_dialog" style="top: 7%; left: 26%;">

                    <div id="Hdr_Stat">
                        <%--<a id="closeHdr" onclick="closePOP();" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>--%>


                        <table id="tblNewIOA" style="display: inline-table; width: 100%; border: 0px; height: auto" class="popsecondver">

                            <tr>
                                <td align="right" colspan="2"></td>
                            </tr>
                            <tr>

                                <td align="left" colspan="2">
                                    <b></b>
                                    <hr>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdText" colspan="2" style="text-align: center;"><b>Do you want to start an IOA session ?</b></td>

                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;"></td>
                            </tr>

                        </table>
                        <table id="tblIOAUser" style="width: 100%; display: none;">
                            <tr>
                                <td align="right" colspan="3"></td>
                            </tr>
                            <tr>

                                <td align="left" colspan="3">
                                    <b></b>
                                    <hr>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdText" colspan="3" style="text-align: center;"><b>Do you want to start an IOA session ?</b></td>

                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdText" style="text-align: right; width: 25%;"><b>Select User :</b></td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlIOAUsers" runat="server" CssClass="drpClass"></asp:DropDownList>
                                </td>
                                <td style="text-align: left; width: 30%;"></td>
                            </tr>
                        </table>
                        <table id="tblIOAndNorm" style="width: 100%; display: none;">
                            <tr>

                                <td align="left" colspan="3">
                                    <b>Select IOA or Normal Session</b>
                                    <hr>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <table style="width: 85%;">
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <b>IOA Session</b>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <b>Normal Session</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 88px;" class="tdText">User:</label>
                                                <label class="txtnormal" runat="server" id="lblIOAUsr">Username</label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 80px;" class="tdText">User:</label>
                                                <label class="txtnormal" runat="server" id="lblNormalUsr">Username</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 54px;" class="tdText">Start Time:</label>
                                                <label runat="server" id="lblIOAStime">10:00</label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 46px;" class="tdText">Start Time:</label>
                                                <label runat="server" class="txtnormal" id="lblNormalStime">10:00</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 59px;" class="tdText">End Time:</label>
                                                <label runat="server" class="txtnormal" id="lblIOAEtime"></label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 51px;" class="tdText">End Time:</label>
                                                <label runat="server" class="txtnormal" id="lblNormalEtime"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 18px;" class="tdText">Session Number:</label></label>
                                                <label runat="server" class="txtnormal" id="lblIOASessNo">1200</label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 12px;" class="tdText">Session Number:</label>
                                                <label runat="server" id="lblSessNo" class="txtnormal">2300</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;"></td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;"></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center; width: 49%;"></td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: center; width: 49%;"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                        </table>

                    </div>
                </div>
            </div>
            <div class="fullOverlay">
            </div>

            <div id="divPrmpts" class="web_dialog2">
                <table width="100%">
                    <tr>
                        <td>
                            <div id="divStpPrmpts" runat="server"></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
